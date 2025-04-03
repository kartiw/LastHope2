Imports System.IO
Imports MySql.Data.MySqlClient

Public Class RelationMaster
    Dim conn
    Dim db
    Dim lastrecordID
    Dim dtAll = New DataTable()
    Public Shared Sub WriteDataTable(ByVal sourceTable As DataTable,
        ByVal writer As TextWriter, ByVal includeHeaders As Boolean)
        If (includeHeaders) Then
            Dim headerValues As IEnumerable(Of String) = sourceTable.Columns.OfType(Of DataColumn).Select(Function(column) QuoteValue(column.ColumnName))

            writer.WriteLine(String.Join(",", headerValues))
        End If

        Dim items As IEnumerable(Of String) = Nothing
        For Each row As DataRow In sourceTable.Rows
            items = row.ItemArray.Select(Function(obj) QuoteValue(If(obj?.ToString(), String.Empty)))
            writer.WriteLine(String.Join(",", items))
        Next

        writer.Flush()
    End Sub

    Private Shared Function QuoteValue(ByVal value As String) As String
        Return String.Concat("""", value.Replace("""", """"""), """")
    End Function


    Private Sub RelationMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()

        Dim cmd As New MySqlCommand("SELECT * FROM relation", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()


        dtAll.Load(reader)

        Dim dr() As DataRow = dtAll.Select("relationid = max(relationid)")
        lastrecordID = dr(0)(0)

        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = dtAll
        DataGridView1.Refresh()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        tbRelationID.Text = ""
        tbRelation.Text = ""
        tbRelationID.Text = lastrecordID + 1
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim RelationID = InputBox("Enter the Relation ID to edit")
        Dim RelationIDAsInt As Integer
        If Integer.TryParse(RelationID, RelationIDAsInt) Then
            ' AreaID successfully parsed as Integer
        Else
            MsgBox("Invalid Entry! Please enter a number")
        End If

        Dim cmd_edit As New MySqlCommand(String.Format("SELECT * FROM relation WHERE relationid=@RelationID"), conn)
        cmd_edit.Parameters.AddWithValue("@RelationID", RelationID)
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader_edit As MySqlDataReader = cmd_edit.ExecuteReader()

        While reader_edit.Read()
            tbRelationID.Text = reader_edit("relationid")
            tbRelation.Text = reader_edit("relationname")
        End While
        reader_edit.Close()
        reader_edit.Dispose()
        cmd_edit.Dispose()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If tbRelationID.Text = String.Empty Or tbRelation.Text = String.Empty Then
            MsgBox("Make sure both textbox is populated before saving")
        Else
            Dim cmd_del As New MySqlCommand(String.Format("DELETE FROM relation WHERE relationid=@RelationID"), conn)
            cmd_del.Parameters.AddWithValue("@RelationID", tbRelationID.Text)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader_del As MySqlDataReader = cmd_del.ExecuteReader()
            reader_del.Close()
            reader_del.Dispose()
            cmd_del.Dispose()

            Dim cmd_insert As New MySqlCommand(String.Format("INSERT INTO relation VALUES(@RelationID,@Relation)"), conn)
            cmd_insert.Parameters.AddWithValue("@RelationID", tbRelationID.Text)
            cmd_insert.Parameters.AddWithValue("@Relation", tbRelation.Text)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader_insert As MySqlDataReader = cmd_insert.ExecuteReader()
            reader_insert.Close()
            reader_insert.Dispose()
            cmd_insert.Dispose()

            MsgBox("Relation Inserted!")
            RelationMaster_Load(sender, e)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim RelationID = InputBox("Enter the Relation ID to delete")
        Dim RelationIDAsInt As Integer
        If Integer.TryParse(RelationID, RelationIDAsInt) Then
            ' AreaID successfully parsed as Integer
        Else
            MsgBox("Invalid Entry! Please enter a number")
        End If

        Dim cmd_del As New MySqlCommand(String.Format("DELETE FROM relation WHERE relationid=@RelationID"), conn)
        cmd_del.Parameters.AddWithValue("@RelationID", RelationID)
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader_del As MySqlDataReader = cmd_del.ExecuteReader()
        reader_del.Close()
        reader_del.Dispose()
        cmd_del.Dispose()

        MsgBox("Relation Deleted!")
        RelationMaster_Load(sender, e)
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim seachfilter = TextBox1.Text
        Dim dv As New DataView(dtAll)
        dv.RowFilter = "relationname Like '%" + seachfilter + "%'"
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If (Not System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory + "\Exports")) Then
            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory + "\Exports")
        End If

        Dim cmd = New MySqlCommand("SELECT * from relation", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        Dim master As New DataTable
        master.Load(reader)
        Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Exports\RelationMaster_" + Date.Today.Date + ".csv")
            WriteDataTable(master, writer, True)
        End Using

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()

        MsgBox("Master Exported!")
    End Sub
End Class