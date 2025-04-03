Imports MySql.Data.MySqlClient

Public Class EditRelation
    Dim conn
    Dim db
    Dim dtrelation As New DataTable

    Private Sub EditRelation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        TextBox2.Text = PublicModule.ERPartnerID
        TextBox5.Enabled = False

        ComboBox1.Items.Clear()

        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = PublicModule.relationtable
        DataGridView1.Refresh()

        Dim cmdrelatives As New MySqlCommand("SELECT partner.partnerid, partner.partnername,partner.middlename,partner.lastname,familytree.relationname,area.AreaName,partner.birthdate,partner.baptismdate,partner.regular,partner.active,partner.phoneno,partner.mobileno 
                                                FROM partner, familytree, area
                                                Where partner.partnerid=familytree.relativeid and familytree.partnerid=@PID and partner.areaid=area.AreaId;", conn)
        cmdrelatives.Parameters.AddWithValue("@PID", PublicModule.ERPartnerID)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerrelatives As MySqlDataReader = cmdrelatives.ExecuteReader()

        While readerrelatives.Read()
            ComboBox1.Items.Add(readerrelatives("partnerid") + " | " + readerrelatives("partnername") + " " + readerrelatives("middlename") + " " + readerrelatives("lastname"))
        End While

        readerrelatives.Close()
        readerrelatives.Dispose()
        cmdrelatives.Dispose()
        conn.Close()

        Dim cmdrelation As New MySqlCommand("SELECT * from relation", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerrelation As MySqlDataReader = cmdrelation.ExecuteReader()

        dtrelation.Load(readerrelation)
        DataGridView2.AutoGenerateColumns = True
        DataGridView2.DataSource = dtrelation
        DataGridView2.Refresh()

        readerrelation.Close()
        readerrelation.Dispose()
        cmdrelation.Dispose()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim selectedvalue = ComboBox1.Text

        Dim words As String() = selectedvalue.Split(New Char() {"|"c})
        TextBox3.Text = words(0)

        Dim result() As DataRow = PublicModule.relationtable.Select("partnerid = '" + words(0) + "'")
        TextBox5.Text = result(0)(4)

        Dim result2() As DataRow = dtrelation.Select("relationname = '" + TextBox5.Text + "'")
        TextBox4.Text = result2(0)(0)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox3.Text <> String.Empty And TextBox4.Text <> String.Empty Then

            Dim cmd As New MySqlCommand("UPDATE familytree SET relationid=@RID, relationname=@RN WHERE partnerid=@PID AND relativeid=@ReID", conn)
            cmd.Parameters.AddWithValue("@PID", TextBox2.Text)
            cmd.Parameters.AddWithValue("@ReID", TextBox3.Text)
            cmd.Parameters.AddWithValue("@RID", TextBox4.Text)
            cmd.Parameters.AddWithValue("@RN", TextBox5.Text)


            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.close()

            MsgBox("Relation Updated!")
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