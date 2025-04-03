Imports System.IO
Imports MySql.Data.MySqlClient

Public Class AreaMaster
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

    Public Function NotNull(ByVal Value)
        If Value Is Nothing OrElse IsDBNull(Value) Then
            Return ""
        Else
            Return Value
        End If
    End Function

    Private Sub AreaMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()

        Dim cmd As New MySqlCommand("SELECT * FROM area", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()


        dtAll.Load(reader)
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = dtAll
        DataGridView1.Refresh()


        With DataGridView1
            lastrecordID = .Rows(.RowCount - 2).Cells(.ColumnCount - 2).Value
        End With

        reader.Close()
        cmd.Dispose()
        conn.close()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        tbAreaID.Text = ""
        tbAreaName.Text = ""
        Dim dr() As DataRow = dtAll.Select("AreaId = max(AreaId)")
        tbAreaID.Text = dr(0)(0) + 1
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If tbAreaID.Text = String.Empty Or tbAreaName.Text = String.Empty Or tbPincode.Text = String.Empty Or tbCity.Text = String.Empty Then
            MsgBox("Make sure both textbox is populated before saving")
        Else
            Dim cmd_del As New MySqlCommand(String.Format("DELETE FROM area WHERE AreaID=@AreaID"), conn)
            cmd_del.Parameters.AddWithValue("@AreaID", tbAreaID.Text)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader_del As MySqlDataReader = cmd_del.ExecuteReader()
            reader_del.Close()
            reader_del.Dispose()
            cmd_del.Dispose()

            Dim cmd_insert As New MySqlCommand(String.Format("INSERT INTO area VALUES(@AreaID,@AreaName,@Region,@Pincode,@City)"), conn)
            cmd_insert.Parameters.AddWithValue("@AreaID", tbAreaID.Text)
            cmd_insert.Parameters.AddWithValue("@AreaName", tbAreaName.Text)
            cmd_insert.Parameters.AddWithValue("@Region", ComboBox1.Text)
            cmd_insert.Parameters.AddWithValue("@Pincode", tbPincode.Text)
            cmd_insert.Parameters.AddWithValue("@City", tbCity.Text)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader_insert As MySqlDataReader = cmd_insert.ExecuteReader()
            reader_insert.Close()
            reader_insert.Dispose()
            cmd_insert.Dispose()

            MsgBox("Area Inserted!")
            AreaMaster_Load(sender, e)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim AreaID = InputBox("Enter the Area ID to delete")
        Dim AreaIDAsInt As Integer
        If Integer.TryParse(AreaID, AreaIDAsInt) Then
            ' AreaID successfully parsed as Integer
        Else
            MsgBox("Invalid Entry! Please enter a number")
        End If

        Dim cmd_del As New MySqlCommand(String.Format("DELETE FROM area WHERE AreaID=@AreaID"), conn)
        cmd_del.Parameters.AddWithValue("@AreaID", AreaID)
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader_del As MySqlDataReader = cmd_del.ExecuteReader()
        reader_del.Close()
        reader_del.Dispose()
        cmd_del.Dispose()

        MsgBox("Area Deleted!")
        AreaMaster_Load(sender, e)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim AreaID = InputBox("Enter the Area ID to edit")
        Dim AreaIDAsInt As Integer
        If Integer.TryParse(AreaID, AreaIDAsInt) Then
            ' AreaID successfully parsed as Integer
        Else
            MsgBox("Invalid Entry! Please enter a number")
        End If

        Dim cmd_edit As New MySqlCommand(String.Format("SELECT * FROM area WHERE AreaID=@AreaID"), conn)
        cmd_edit.Parameters.AddWithValue("@AreaID", AreaID)
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader_edit As MySqlDataReader = cmd_edit.ExecuteReader()

        While reader_edit.Read()
            tbAreaID.Text = reader_edit("AreaID")
            tbAreaName.Text = reader_edit("AreaName")
            ComboBox1.Text = NotNull(reader_edit("Region"))
            tbPincode.Text = NotNull(reader_edit("Pincode"))
            tbCity.Text = NotNull(reader_edit("City"))
        End While
        reader_edit.Close()
        reader_edit.Dispose()
        cmd_edit.Dispose()

    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs)
        Dim dv As New DataView(dtAll)
        dv.RowFilter = "AreaName Like '%g%'"
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim seachfilter = TextBox1.Text
        Dim dv As New DataView(dtAll)
        dv.RowFilter = "AreaName Like '%" + seachfilter + "%' OR Region Like '%" + seachfilter + "%' OR Pincode Like '%" + seachfilter + "%' OR City Like '%" + seachfilter + "%'"
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        If (Not System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory + "\Exports")) Then
            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory + "\Exports")
        End If

        Dim cmd = New MySqlCommand("SELECT * from area", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        Dim master As New DataTable
        master.Load(reader)
        Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Exports\AreaMaster_" + Date.Today.Date + ".csv")
            WriteDataTable(master, writer, True)
        End Using

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()

        MsgBox("Master Exported!")
    End Sub
End Class