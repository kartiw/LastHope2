Imports MySql.Data.MySqlClient
Public Class AddNewRelation
    Dim conn
    Dim db
    Dim dtPartner As New DataTable()
    Dim dtrelation As New DataTable

    Private Sub AddNewRelation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox2.Enabled = False
        TextBox2.Text = PublicModule.ARParntnerID
        TextBox5.Enabled = False

        Dim cmd As New MySqlCommand("SELECT partner.partnerid, partner.partnername, partner.middlename,partner.lastname, partner.birthdate, partner.baptismdate,area.areaname ,partner.regular, partner.active, partner.mobileno, partner.phoneno FROM partner left join area on partner.areaid=area.AreaId", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()


        dtPartner.Load(reader)
        dgvPartnerDetails.AutoGenerateColumns = True
        dgvPartnerDetails.DataSource = dtPartner
        dgvPartnerDetails.Refresh()

        reader.Close()
        reader.Dispose()
        cmd.Dispose()

        Dim cmdrelation As New MySqlCommand("SELECT * from relation", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerrelation As MySqlDataReader = cmdrelation.ExecuteReader()

        dtrelation.Load(readerrelation)
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = dtrelation
        DataGridView1.Refresh()

        readerrelation.Close()
        readerrelation.Dispose()
        cmdrelation.Dispose()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim seachfilter = TextBox1.Text
        Dim dv As New DataView(dtPartner)
        dv.RowFilter = "partnername Like '%" + seachfilter + "%' OR mobileno Like '%" + seachfilter + "%' OR phoneno Like '%" + seachfilter + "%' OR areaname Like '%" + seachfilter + "%' OR middlename Like '%" + seachfilter + "%' OR lastname Like '%" + seachfilter + "%'"
        dgvPartnerDetails.DataSource = dv
        dgvPartnerDetails.Refresh()
    End Sub

    Private Sub ToolTip1_Popup(sender As Object, e As PopupEventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox3.Text <> String.Empty And TextBox4.Text <> String.Empty Then

            Dim cmd As New MySqlCommand("Insert into familytree values(@PID,@ReID,@RID,@RN)", conn)
            cmd.Parameters.AddWithValue("@PID", TextBox2.Text)
            cmd.Parameters.AddWithValue("@ReID", TextBox3.Text)
            cmd.Parameters.AddWithValue("@RID", TextBox4.Text)
            cmd.Parameters.AddWithValue("@RN", TextBox5.Text)


            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            MsgBox("Relation Added!")
        Else
            MsgBox("Please fill in all fields")
        End If

    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

        If TextBox4.Text <> String.Empty Then
            Dim result() As DataRow = dtrelation.Select("relationid = " + TextBox4.Text.ToString())
            TextBox5.Text = result(0)(1)
        End If
    End Sub
End Class