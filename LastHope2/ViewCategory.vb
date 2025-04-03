Imports MySql.Data.MySqlClient

Public Class ViewCategory
    Dim conn
    Dim db
    Dim lastrecordID
    Dim dtAll = New DataTable()
    Dim SelectedCategoryID As String
    Dim SelectedCategoryName As String

    Private Sub ViewCategory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()

        Dim cmd As New MySqlCommand("SELECT *  FROM category", conn)
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
        dv.RowFilter = "categoryname Like '%" + seachfilter + "%'"
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub DataGridView1_Click(sender As Object, e As EventArgs) Handles DataGridView1.Click
        SelectedCategoryName = DataGridView1.CurrentRow.Cells(1).Value.ToString()
        SelectedCategoryID = DataGridView1.CurrentRow.Cells(0).Value.ToString()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SelectedCategoryName <> String.Empty Then
            PublicModule.VCCategoryID = SelectedCategoryID
            PublicModule.VCCategoryName = SelectedCategoryName
            Me.Close()
        Else
            MsgBox("Please select a category to return its details")
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class