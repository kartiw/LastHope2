Imports System.IO
Imports MySql.Data.MySqlClient

Public Class PartnerReport
    Dim conn
    Dim db

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



    Private Sub PartnerReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        conn.Close()



        Dim cmd_inner As New MySqlCommand("SELECT partnerid,partnername,middlename,lastname FROM partner WHERE partnerid in " + idstring + " ORDER BY partnername", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader_inner As MySqlDataReader = cmd_inner.ExecuteReader()

        While reader_inner.Read()
            ComboBox2.Items.Add(reader_inner("partnerid") + " | " + reader_inner("partnername") + " " + reader_inner("middlename") + " " + reader_inner("lastname"))
            ComboBox3.Items.Add(reader_inner("partnerid") + " | " + reader_inner("partnername") + " " + reader_inner("middlename") + " " + reader_inner("lastname"))
        End While
        reader_inner.Close()
        reader_inner.Dispose()
        cmd_inner.Dispose()
        conn.Close()


        'get area
        Dim cmdarea As New MySqlCommand("SELECT * FROM area ORDER BY AreaName", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerarea As MySqlDataReader = cmdarea.ExecuteReader()

        While readerarea.Read()

            ComboBox1.Items.Add(String.Concat(readerarea("AreaId"), ".", readerarea("AreaName")))

        End While

        readerarea.Close()
        readerarea.Dispose()
        cmdarea.Dispose()
        conn.Close()

        RadioButton1.Checked = True


        If (Not System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory + "\Reports")) Then
            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory + "\Reports")
        End If
        RadioButton8.Enabled = False
        RadioButton9.Enabled = False
        RadioButton10.Enabled = False

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim cmd As New MySqlCommand("SELECT * FROM view_partner_area WHERE active=0", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        Dim irregular As New DataTable
        irregular.Load(reader)
        Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\InactiveMembers_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
            WriteDataTable(irregular, writer, True)
        End Using

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()

        MsgBox("Report Exported!")

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim cmd As New MySqlCommand("SELECT * FROM view_partner_area WHERE regular=0", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        Dim irregular As New DataTable
        irregular.Load(reader)
        Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\IrregularMembers_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
            WriteDataTable(irregular, writer, True)
        End Using

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()

        MsgBox("Report Exported!")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim OKflag = True
        Dim cmd As New MySqlCommand()

        If TextBox1.Text <> String.Empty Then

            If TextBox1.Text.ToLower() = "all" Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area", conn)
                'MsgBox(cmd.CommandText)


            Else
                Dim partnerids = TextBox1.Text.Split(",")

                For Each partnerid In partnerids
                    Dim partneridAsInt As Integer
                    If Integer.TryParse(partnerid, partneridAsInt) Then
                    Else
                        OKflag = False
                    End If
                Next



                If OKflag And CheckBox1.Checked = True Then
                    cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE partnerid IN (" + TextBox1.Text + ") and active=1", conn)
                ElseIf OKflag And CheckBox1.Checked = False Then
                    cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE partnerid IN (" + TextBox1.Text + ")", conn)
                Else
                    MsgBox("Please Check the input. Either enter 'all', a single value or multiple values separated by comma(,)")
                End If
            End If

            If OKflag Then
                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                Dim irregular As New DataTable
                irregular.Load(reader)
                Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\PartnerDetails_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                    WriteDataTable(irregular, writer, True)
                End Using

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()

                MsgBox("Report Exported!")
            End If

        Else
            MsgBox("Textbox cannot be empty")
        End If




    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim OKflag = True
        Dim cmd As New MySqlCommand()

        If ComboBox2.Text <> String.Empty Then

            Dim words As String() = ComboBox2.Text.Split(New Char() {"|"c})

            If CheckBox3.Checked = True Then
                cmd = New MySqlCommand("select * from view_partner_area where (followedid1=@LID or followedid2=@LID or followedid3=@LID or followedid4=@LID) and active=1", conn)
                cmd.Parameters.AddWithValue("@LID", words(0))
            ElseIf CheckBox3.Checked = False Then
                cmd = New MySqlCommand("select * from view_partner_area where followedid1=@LID or followedid2=@LID or followedid3=@LID or followedid4=@LID", conn)
                cmd.Parameters.AddWithValue("LID", words(0))
            End If

            If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                Dim irregular As New DataTable
                irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\PartnerInfo_Under_" + words(1) + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()

                MsgBox("Report Exported!")


            Else
                MsgBox("Select an leader before generating")
        End If

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim OKflag = True
        Dim cmd As New MySqlCommand()

        If ComboBox1.Text <> String.Empty Then

            Dim words As String() = ComboBox1.Text.Split(New Char() {"."c})


            If OKflag And CheckBox2.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE areaid=@AID and active=1", conn)
                cmd.Parameters.AddWithValue("@AID", words(0))
            ElseIf OKflag And CheckBox2.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE areaid=@AID", conn)
                cmd.Parameters.AddWithValue("@AID", words(0))
            End If


            If OKflag Then
                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                Dim irregular As New DataTable
                irregular.Load(reader)
                Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\AreaWise_" + words(1) + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                    WriteDataTable(irregular, writer, True)
                End Using

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()

                MsgBox("Report Exported!")
            End If

        Else
            MsgBox("Select an area before generating")
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If RadioButton1.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and dateofenrollment BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE dateofenrollment BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton1.Text.ToString() + "_Between_" + Format(DateTimePicker1.Value.Date, "yyyy-MM-dd") + "_and_" + Format(DateTimePicker2.Value.Date, "yyyy-MM-dd") + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If

        If RadioButton2.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and weddingdate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE weddingdate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton2.Text.ToString() + "_Between_" + Format(DateTimePicker1.Value.Date, "yyyy-MM-dd") + "_and_" + Format(DateTimePicker2.Value.Date, "yyyy-MM-dd") + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If

        If RadioButton3.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and inactivedate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE inactivedate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton3.Text.ToString() + "_Between_" + Format(DateTimePicker1.Value.Date, "yyyy-MM-dd") + "_and_" + Format(DateTimePicker2.Value.Date, "yyyy-MM-dd") + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If

        If RadioButton4.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and deathdate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE deathdate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton4.Text.ToString() + "_Between_" + Format(DateTimePicker1.Value.Date, "yyyy-MM-dd") + "_and_" + Format(DateTimePicker2.Value.Date, "yyyy-MM-dd") + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If

        If RadioButton5.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and birthdate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE birthdate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton5.Text.ToString() + "_Between_" + Format(DateTimePicker1.Value.Date, "yyyy-MM-dd") + "_and_" + Format(DateTimePicker2.Value.Date, "yyyy-MM-dd") + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If

        If RadioButton6.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and baptismdate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE baptismdate BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton6.Text.ToString() + "_Between_" + Format(DateTimePicker1.Value.Date, "yyyy-MM-dd") + "_and_" + Format(DateTimePicker2.Value.Date, "yyyy-MM-dd") + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If

        If RadioButton7.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and dateofmembershipeligibility BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE dateofmembershipeligibility BETWEEN @SD AND @ED", conn)
                cmd.Parameters.AddWithValue("@SD", Format(DateTimePicker1.Value.Date, "yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@ED", Format(DateTimePicker2.Value.Date, "yyyy-MM-dd"))

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton7.Text.ToString() + "_Between_" + Format(DateTimePicker1.Value.Date, "yyyy-MM-dd") + "_and_" + Format(DateTimePicker2.Value.Date, "yyyy-MM-dd") + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim cmd As New MySqlCommand()
        Dim irregular As New DataTable
        Dim type = ""

        For Each partner In CheckedListBox1.CheckedItems

            If partner <> "All" Then

                Dim words As String() = partner.Split(New Char() {"|"c})

                If RadioButton8.Checked = True Then
                    type = "Pending"
                    cmd = New MySqlCommand("select request.requestid, view_partner_area.partnerid as 'Request by Partner ID',view_partner_area.partnername as 'Request by Firstname', view_partner_area.middlename as 'Request by Middlename', view_partner_area.lastname as 'Request by Lastname' ,partner.partnerid as 'Request for Partner ID' ,partner.partnername as 'Request for Firstname',partner.middlename as 'Request For Middlename',partner.lastname as 'Request For Lastname',categoryname,requestdate,requesttitle,prayerdetails,requestdonedate,testemony 
                                        from request, partner, category, view_partner_area 
                                        where partner.partnerid=request.relationid and view_partner_area.partnerid=request.partnerid and category.categoryid=request.categoryid and request.partnerid='1009' and request.requestdonedate='1900-01-01'", conn)
                    'cmd.Parameters.AddWithValue("@PID", words(0))
                End If

                If RadioButton10.Checked = True Then
                    type = "Completed"
                    cmd = New MySqlCommand("select request.requestid, view_partner_area.partnerid as 'Request by Partner ID',view_partner_area.partnername as 'Request by Firstname', view_partner_area.middlename as 'Request by Middlename', view_partner_area.lastname as 'Request by Lastname' ,partner.partnerid as 'Request for Partner ID' ,partner.partnername as 'Request for Firstname',partner.middlename as 'Request For Middlename',partner.lastname as 'Request For Lastname',categoryname,requestdate,requesttitle,prayerdetails,requestdonedate,testemony 
                                        from request, partner, category, view_partner_area 
                                        where partner.partnerid=request.relationid and category.categoryid=request.categoryid and request.partnerid=@PID and request.requestdonedate>'1900-01-01' and view_partner_area.partnerid=request.partnerid", conn)
                    cmd.Parameters.AddWithValue("@PID", words(0))
                End If

                If RadioButton9.Checked = True Then
                    type = "All"
                    cmd = New MySqlCommand("select request.requestid, view_partner_area.partnerid as 'Request by Partner ID',view_partner_area.partnername as 'Request by Firstname', view_partner_area.middlename as 'Request by Middlename', view_partner_area.lastname as 'Request by Lastname' ,partner.partnerid as 'Request for Partner ID' ,partner.partnername as 'Request for Firstname',partner.middlename as 'Request For Middlename',partner.lastname as 'Request For Lastname',categoryname,requestdate,requesttitle,prayerdetails,requestdonedate,testemony 
                                        from request, partner, category, view_partner_area 
                                        where partner.partnerid=request.relationid and category.categoryid=request.categoryid and request.partnerid=@PID and view_partner_area.partnerid=request.partnerid", conn)
                    cmd.Parameters.AddWithValue("@PID", words(0))
                End If


                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Dim reader As MySqlDataReader = cmd.ExecuteReader()


                irregular.Load(reader)



                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()

            End If


        Next

        Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\ParayerRequest__Under" + ComboBox3.Text.Split(New Char() {"|"c})(1) + "_" + type + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
            WriteDataTable(irregular, writer, True)
        End Using

        MsgBox("Report Exported!")


    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        SearchPartners.ShowDialog()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        RadioButton8.Enabled = True
        RadioButton9.Enabled = True
        RadioButton10.Enabled = True



    End Sub

    Private Sub RadioButton8_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton8.Click
        'MsgBox("Pending")
        CheckedListBox1.Items.Clear()
        Dim words As String() = ComboBox3.Text.Split(New Char() {"|"c})
        Dim cmd As New MySqlCommand()

        cmd = New MySqlCommand("select * from view_partner_area, request where (followedid1=@LID or followedid2=@LID or followedid3=@LID or followedid4=@LID) and view_partner_area.partnerid=request.partnerid and request.requestdonedate='1900-01-01' GROUP BY request.partnerid ORDER BY partnername", conn)
        cmd.Parameters.AddWithValue("@LID", words(0))
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        CheckedListBox1.Items.Add("All")
        While reader.Read()
            CheckedListBox1.Items.Add(reader("partnerid") + " | " + reader("partnername") + " " + reader("middlename") + " " + reader("lastname"))
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()
    End Sub

    Private Sub RadioButton10_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton10.Click
        'MsgBox("Completed")
        CheckedListBox1.Items.Clear()
        Dim words As String() = ComboBox3.Text.Split(New Char() {"|"c})
        Dim OKflag = True
        Dim cmd As New MySqlCommand()
        cmd = New MySqlCommand("select * from view_partner_area, request where (followedid1=@LID or followedid2=@LID or followedid3=@LID or followedid4=@LID) and view_partner_area.partnerid=request.partnerid and request.requestdonedate>'1900-01-01' GROUP BY request.partnerid ORDER BY partnername", conn)
        cmd.Parameters.AddWithValue("@LID", words(0))
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        CheckedListBox1.Items.Add("All")
        While reader.Read()
            CheckedListBox1.Items.Add(reader("partnerid") + " | " + reader("partnername") + " " + reader("middlename") + " " + reader("lastname"))
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()
    End Sub

    Private Sub RadioButton9_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton9.Click
        'MsgBox("All")
        CheckedListBox1.Items.Clear()
        Dim words As String() = ComboBox3.Text.Split(New Char() {"|"c})
        Dim OKflag = True
        Dim cmd As New MySqlCommand()

        cmd = New MySqlCommand("select * from view_partner_area, request where (followedid1=@LID or followedid2=@LID or followedid3=@LID or followedid4=@LID) and view_partner_area.partnerid=request.partnerid GROUP BY request.partnerid ORDER BY partnername", conn)
        cmd.Parameters.AddWithValue("@LID", words(0))
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        CheckedListBox1.Items.Add("All")
        While reader.Read()
            CheckedListBox1.Items.Add(reader("partnerid") + " | " + reader("partnername") + " " + reader("middlename") + " " + reader("lastname"))
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged
        If CheckedListBox1.SelectedItem.ToString() = "All" Then
            For i As Integer = 0 To CheckedListBox1.Items.Count - 1
                CheckedListBox1.SetItemChecked(i, True)
            Next
        End If

    End Sub

End Class