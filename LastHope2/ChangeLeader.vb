Imports MySql.Data.MySqlClient
Public Class ChangeLeader
    Dim conn
    Dim db
    Dim dtAll = New DataTable()
    Private Sub ChangeLeader_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbleader.Items.Clear()
        db = New Database()
        conn = db.OpenConnection()
        Dim idstring = ""

        Dim cmd As New MySqlCommand("SELECT DISTINCT leaderid FROM leader", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            If reader("leaderid") <> "" Then
                idstring = idstring + reader("leaderid") + ","
            End If
        End While
        idstring = idstring.Trim().Remove(idstring.Length - 1)
        idstring = "(" + idstring + ")"

        reader.Close()
        reader.Dispose()
        cmd.Dispose()



        Dim cmd_inner As New MySqlCommand("SELECT partnerid,partnername,middlename,lastname FROM partner WHERE partnerid in " + idstring, conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader_inner As MySqlDataReader = cmd_inner.ExecuteReader()

        While reader_inner.Read()
            cmbleader.Items.Add(reader_inner("partnerid") + " | " + reader_inner("partnername") + " " + reader_inner("middlename") + " " + reader_inner("lastname"))
        End While
        reader_inner.Close()
        reader_inner.Dispose()
        cmd_inner.Dispose()
    End Sub

    Private Sub cmbleader_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbleader.SelectedIndexChanged
        Dim words As String() = cmbleader.Text.Split(New Char() {"|"c})
        PublicModule.mainleaderid = words(0)
        Dim dtleader = New DataTable()

        'Get the mainleader info
        Dim partnerid
        Dim cmd As New MySqlCommand("SELECT partner.partnerid, partner.partnername, partner.middlename,partner.lastname,partner.birthdate, partner.baptismdate,area.areaname ,partner.regular, partner.active, partner.mobileno, partner.phoneno FROM partner, area WHERE partnerid=@PID AND partner.areaid=area.AreaId", conn)
        cmd.Parameters.AddWithValue("@PID", words(0))
        'cmd.Parameters.AddWithValue("@MN", words(1))
        'cmd.Parameters.AddWithValue("@LN", words(2))

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            partnerid = reader("partnerid")
        End While
        reader.Close()
        reader.Dispose()

        Dim reader2 As MySqlDataReader = cmd.ExecuteReader()
        dtleader.Load(reader2)
        dgvSubleader.AutoGenerateColumns = True
        dgvSubleader.DataSource = dtleader
        dgvSubleader.Refresh()

        reader2.Close()
        reader2.Dispose()
        cmd.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim mainleader As String() = cmbleader.Text.Split(New Char() {"|"c})
        Dim newmainleader = mainleader(0)

        Dim cmd As New MySqlCommand("DELETE FROM leader WHERE leaderid=@LID AND subleaderid=@SLID", conn)
        cmd.Parameters.AddWithValue("@LID", PublicModule.oldmainleaderid)
        cmd.Parameters.AddWithValue("@SLID", PublicModule.subleaderid)


        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        reader.Close()
        reader.Dispose()
        cmd.Dispose()






        'insert new
        Dim cmdinsert As New MySqlCommand("INSERT INTO leader VALUES(@LID, @SLID)", conn)
        cmdinsert.Parameters.AddWithValue("@LID", newmainleader)
        cmdinsert.Parameters.AddWithValue("@SLID", PublicModule.subleaderid)


        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim readerinsert As MySqlDataReader = cmdinsert.ExecuteReader()

        readerinsert.Close()
        readerinsert.Dispose()
        cmdinsert.Dispose()

        MsgBox("Leader Changed!")

    End Sub
End Class