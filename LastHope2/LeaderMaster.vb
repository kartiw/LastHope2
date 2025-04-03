Imports System.IO
Imports MySql.Data.MySqlClient

Public Class LeaderMaster
    Dim conn
    Dim db
    Dim lastrecordID
    Dim mainleaderid
    Dim dtsubleader = New DataTable()
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

    Private Sub LeaderMaster_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbleader.Items.Clear()
        Button1.Enabled = False
        Button2.Enabled = False
        Button6.Enabled = False
        Button3.Enabled = False

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
        Button1.Enabled = True


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
        dgvMainleader.AutoGenerateColumns = True
        dgvMainleader.DataSource = dtleader
        dgvMainleader.Refresh()

        reader2.Close()
        reader2.Dispose()
        cmd.Dispose()

        'Get the list of subleaders
        Dim subleaderids = ""
        Dim cmdsubleaderlist As New MySqlCommand("SELECT subleaderid FROM leader WHERE leaderid=@LID", conn)
        cmdsubleaderlist.Parameters.AddWithValue("@LID", partnerid)


        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readersubleaderlist As MySqlDataReader = cmdsubleaderlist.ExecuteReader()

        While readersubleaderlist.Read()
            subleaderids = subleaderids + readersubleaderlist("subleaderid") + ","
        End While
        If subleaderids.Length > 0 Then
            subleaderids = subleaderids.Trim().Remove(subleaderids.Length - 1)
        Else
            subleaderids = "0"
        End If
        subleaderids = "(" + subleaderids + ")"

        readersubleaderlist.Close()
        readersubleaderlist.Dispose()
        cmdsubleaderlist.Dispose()

        'Get the subleaders info
        Dim cmdsubleader As New MySqlCommand("SELECT partner.partnerid, partner.partnername, partner.middlename,partner.lastname, partner.birthdate, partner.baptismdate,area.areaname ,partner.regular, partner.active, partner.mobileno, partner.phoneno FROM partner, area WHERE partner.areaid=area.AreaId AND partnerid IN " + subleaderids, conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readersubleader As MySqlDataReader = cmdsubleader.ExecuteReader()

        dtsubleader = New DataTable()
        dtsubleader.Load(readersubleader)
        dgvSubleader.AutoGenerateColumns = True
        dgvSubleader.DataSource = dtsubleader
        dgvSubleader.Refresh()

        readersubleader.Close()
        readersubleader.Dispose()


        'populate the listbox
        ListBox1.Items.Clear()
        Dim readersubleader_listbox As MySqlDataReader = cmdsubleader.ExecuteReader()
        While readersubleader_listbox.Read()
            ListBox1.Items.Add(readersubleader_listbox("partnerid") + " | " + readersubleader_listbox("partnername") + " " + readersubleader_listbox("middlename") + " " + readersubleader_listbox("lastname"))

        End While

        readersubleader_listbox.Close()
        readersubleader_listbox.Dispose()
        cmdsubleader.Dispose()
        Button6.Enabled = True
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim seachfilter = TextBox1.Text
        Dim dv As New DataView(dtsubleader)
        dv.RowFilter = "partnername Like '%" + seachfilter + "%' OR mobileno Like '%" + seachfilter + "%' OR phoneno Like '%" + seachfilter + "%' OR areaname Like '%" + seachfilter + "%' OR middlename Like '%" + seachfilter + "%' OR lastname Like '%" + seachfilter + "%'"
        dgvSubleader.DataSource = dv
        dgvSubleader.Refresh()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim selectedvalue = ListBox1.SelectedItem

        If selectedvalue = String.Empty Then
            'MsgBox("Hello")
        Else
            selectedvalue = selectedvalue.ToString()
            Dim words As String() = selectedvalue.Split(New Char() {"|"c})
            Dim name As String() = selectedvalue.Split(New Char() {" "c})
            TextBox1.Text = name(2)

            Button2.Enabled = True
            Button3.Enabled = True
        End If


    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBox1.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Adds New sub Leader to the selected leader
        'Dim ansl As New AddNewSubLeader(mainleaderid)
        'ansl.ShowDialog()
        AddNewSubLeader.ShowDialog()
        cmbleader_SelectedIndexChanged(sender, e)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        'Add New Leader
        AddNewMainLeader.ShowDialog()
        LeaderMaster_Load(sender, e)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)
        'Show Partner Details
        AddNewMainLeader.ShowDialog()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        'Remove Leader
        Dim words As String() = cmbleader.Text.Split(New Char() {"|"c})
        Dim partnerid = words(0)

        If partnerid <> String.Empty Then
            Dim partneridAsInt As Integer
            If Integer.TryParse(partnerid, partneridAsInt) Then
                ' AreaID successfully parsed as Integer
            Else
                MsgBox("Invalid Entry! Please enter a number")
            End If

            Dim result As Integer = MsgBox("All sub-leaders associated with this leader will also be deleted, is that OK?", MessageBoxButtons.OKCancel)

            If result = 1 Then
                Dim cmd As New MySqlCommand("DELETE FROM leader WHERE leaderid=@LID", conn)
                cmd.Parameters.AddWithValue("@LID", partnerid)


                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                MsgBox("Leader Deleted!")
                LeaderMaster_Load(sender, e)

            End If
        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim selectedvalue = ListBox1.SelectedItem.ToString()
        Dim words As String() = selectedvalue.Split(New Char() {"|"c})
        Dim subleaderid = words(0)

        Dim mainleader As String() = cmbleader.Text.Split(New Char() {"|"c})
        Dim partnerid = mainleader(0)

        Dim result As Integer = MsgBox("Are you sure you want to delete '" + words(1) + "' as a sub leader?", MessageBoxButtons.YesNo)

        If result = 6 Then
            If selectedvalue = cmbleader.Text.ToString() Then
                MsgBox("Cannot delete this sub-leader as it is the Main Leader, If you want to remove this sub-leader please directly remove the main leader with the same name.")
            Else
                Dim cmd As New MySqlCommand("DELETE FROM leader WHERE leaderid=@LID AND subleaderid=@SLID", conn)
                cmd.Parameters.AddWithValue("@LID", partnerid)
                cmd.Parameters.AddWithValue("@SLID", subleaderid)


                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                MsgBox("Sub Leader Deleted!")
                cmbleader_SelectedIndexChanged(sender, e)
            End If
        End If


    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim selectedvalue = ListBox1.SelectedItem.ToString()
        Dim words As String() = selectedvalue.Split(New Char() {"|"c})
        Dim subleaderid = words(0)

        Dim mainleader As String() = cmbleader.Text.Split(New Char() {"|"c})
        Dim partnerid = mainleader(0)


        If selectedvalue = cmbleader.Text.ToString() Then
            MsgBox("Cannot change this sub-leader as it is the Main Leader")
        Else
            PublicModule.oldmainleaderid = partnerid
            PublicModule.subleaderid = subleaderid

            ChangeLeader.ShowDialog()
            cmbleader_SelectedIndexChanged(sender, e)
        End If
    End Sub

    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        SearchLeaders.ShowDialog()
        If SLPartnerID <> String.Empty Then
            Dim index = cmbleader.FindStringExact(PublicModule.SLPartnerID + " | " + PublicModule.SLPartnerName)
            cmbleader.SelectedIndex = index
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If (Not System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory + "\Exports")) Then
            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory + "\Exports")
        End If

        Dim cmd = New MySqlCommand("SELECT * from leader", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        Dim master As New DataTable
        master.Load(reader)
        Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Exports\LeaderMaster_" + Date.Today.Date + ".csv")
            WriteDataTable(master, writer, True)
        End Using

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()

        MsgBox("Master Exported!")
    End Sub
End Class