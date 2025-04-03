Imports MySql.Data.MySqlClient

Public Class ViewArea
    Dim conn
    Dim db
    Dim lastrecordID
    Dim dtAll = New DataTable()
    Dim SelectedAreaID As String

    Private Sub ViewArea_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()
        SelectedAreaID = ""

        Dim cmd As New MySqlCommand("SELECT *  FROM area", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()


        dtAll.Load(reader)
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = dtAll
        DataGridView1.Refresh()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim seachfilter = TextBox1.Text
        Dim dv As New DataView(dtAll)
        dv.RowFilter = "AreaName Like '%" + seachfilter + "%' OR Region Like '%" + seachfilter + "%' OR Pincode Like '%" + seachfilter + "%' OR City Like '%" + seachfilter + "%'"
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SelectedAreaID <> String.Empty Then
            PublicModule.SPAreaID = SelectedAreaID
            Me.Close()
        Else
            MsgBox("Please select a Area")
        End If
    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As EventArgs) Handles DataGridView1.Click
        SelectedAreaID = DataGridView1.CurrentRow.Cells(0).Value.ToString()
    End Sub
End Class