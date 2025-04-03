Imports MySql.Data.MySqlClient

Public Class AddNewMainLeader
    Dim conn
    Dim db
    Dim lastrecordID
    Dim dtAll = New DataTable()

    Private Sub ListAllPartners_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()

        Dim cmd As New MySqlCommand("SELECT partner.partnerid, partner.partnername, partner.middlename,partner.lastname, partner.birthdate, partner.baptismdate,area.areaname ,partner.regular, partner.active, partner.mobileno, partner.phoneno FROM partner left join area on partner.areaid=area.AreaId", conn)
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
        dv.RowFilter = "partnername Like '%" + seachfilter + "%' OR mobileno Like '%" + seachfilter + "%' OR phoneno Like '%" + seachfilter + "%' OR areaname Like '%" + seachfilter + "%' OR middlename Like '%" + seachfilter + "%' OR lastname Like '%" + seachfilter + "%'"
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim partnerid = TextBox2.Text

        If partnerid <> String.Empty Then

            Dim partneridAsInt As Integer
            If Integer.TryParse(partnerid, partneridAsInt) Then
                ' AreaID successfully parsed as Integer
                'MsgBox(partneridAsInt)
            Else
                MsgBox("Invalid Entry! Please enter a number")
            End If


            Dim cmd As New MySqlCommand("INSERT INTO leader VALUES(@leaderid,@subleaderid)", conn)
            cmd.Parameters.AddWithValue("@leaderid", partnerid)
            cmd.Parameters.AddWithValue("@subleaderid", partnerid)


            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            MsgBox("New Leader Added!")
        End If
    End Sub
End Class