Imports MySql.Data.MySqlClient

Public Class PrayerRequests
    Dim conn
    Dim db
    Dim prophecyid As String
    Dim requestid As String
    Dim categoryid As String
    Dim requestforid As String

    Public Function NotNull(ByVal Value)
        If Value Is Nothing OrElse IsDBNull(Value) Then
            Return ""
        Else
            Return Value
        End If
    End Function

    Public Function NotNull_Date(ByVal Value)
        If Value Is Nothing OrElse IsDBNull(Value) Then
            Return DateTime.Now.Date
        Else
            Return Value
        End If
    End Function

    Sub Clear()
        ComboBox1.Items.Clear()
        tbRequesttitle.Text = ""
        tbRequestFor.Text = ""
        tbCategory.Text = ""
        dtpPRDate.ResetText()
        dtpPRDoneDate.ResetText()
        tbPRRequestDetails.Text = ""
        tbTestemoney.Text = ""

        ComboBox2.Items.Clear()
        TextBox1.Text = ""
        DateTimePicker2.ResetText()
        RichTextBox1.Text = ""
        RichTextBox2.Text = ""

        tbFIrstName.Text = ""
        tbMiddleName.Text = ""
        tbLastName.Text = ""

        dtpPRDate.Value = DateTime.Now.Date
        dtpPRDoneDate.Value = DateTime.Now.Date
        DateTimePicker2.Value = DateTime.Now.Date
    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SearchPartners.ShowDialog()
        If SPPartnerID <> String.Empty Then
            LoadFromPartnerSearch()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ViewCategory.ShowDialog()
    End Sub

    Private Sub PrayerRequests_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()
        tbPartnerID.Enabled = False
        tbRequestFor.Enabled = False
        tbCategory.Enabled = False
        Button1.Enabled = False
        Button2.Enabled = False
        Button7.Enabled = False
        Button6.Enabled = False

        dtpPRDate.Value = DateTime.Now.Date
        dtpPRDoneDate.Value = DateTime.Now.Date
        DateTimePicker2.Value = DateTime.Now.Date

    End Sub

    Private Sub btnPartnerIDSearch_Click(sender As Object, e As EventArgs) Handles btnPartnerIDSearch.Click
        Dim input = InputBox("Enter the Partner ID to show details")

        If input = String.Empty Then
            MsgBox("Please Enter a valid Partner ID")
        Else

            'getting partner info
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                tbPartnerID.Text = input
                Clear()
                Dim partnerid = tbPartnerID.Text
                Dim cmd As New MySqlCommand("SELECT partnerid, partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmd.Parameters.AddWithValue("@PID", partnerid)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    tbFIrstName.Text = NotNull(reader("partnername"))
                    tbMiddleName.Text = NotNull(reader("middlename"))
                    tbLastName.Text = NotNull(reader("lastname"))
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.close()

                'getting prayer details
                Dim cmdrequest As New MySqlCommand("SELECT requestid, requesttitle from request where partnerid = @PID;", conn)
                cmdrequest.Parameters.AddWithValue("@PID", tbPartnerID.Text)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim readerrequest As MySqlDataReader = cmdrequest.ExecuteReader()

                While readerrequest.Read()
                    ComboBox1.Items.Add(NotNull(readerrequest("requestid")) + " | " + NotNull(readerrequest("requesttitle")))
                End While

                readerrequest.Close()
                readerrequest.Dispose()
                cmdrequest.Dispose()
                conn.close()

                'Get phrophecy details

                Dim cmdprophecy As New MySqlCommand("SELECT prophecyid, prophecytitle from prophecy where partnerid = @PID;", conn)
                cmdprophecy.Parameters.AddWithValue("@PID", tbPartnerID.Text)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim readerprophecy As MySqlDataReader = cmdprophecy.ExecuteReader()

                While readerprophecy.Read()
                    ComboBox2.Items.Add(NotNull(readerprophecy("prophecyid")) + " | " + NotNull(readerprophecy("prophecytitle")))
                End While

                readerprophecy.Close()
                readerprophecy.Dispose()
                cmdprophecy.Dispose()
                conn.close()

                Button1.Enabled = True
                Button7.Enabled = True

            Else
                MsgBox("Enter a numeric value for Partner ID")

            End If

        End If


    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim words As String() = ComboBox1.Text.Split(New Char() {"|"c})

        Dim cmdrequest As New MySqlCommand("SELECT request.*, partner.partnername, partner.middlename, partner.lastname, category.categoryname
                                            FROM request left join partner on request.relationid=partner.partnerid, category
                                            Where request.categoryid=category.categoryid and request.requestid = @PID;", conn)
        cmdrequest.Parameters.AddWithValue("@PID", words(0))

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerrequest As MySqlDataReader = cmdrequest.ExecuteReader()

        While readerrequest.Read()
            requestid = readerrequest("requestid")
            categoryid = readerrequest("categoryid")
            requestforid = readerrequest("relationid")
            tbRequesttitle.Text = NotNull(readerrequest("requesttitle"))
            tbRequestFor.Text = NotNull(readerrequest("partnername")) + " " + NotNull(readerrequest("middlename")) + " " + NotNull(readerrequest("lastname"))
            tbCategory.Text = NotNull(readerrequest("categoryname"))
            dtpPRDate.Value = NotNull_Date(readerrequest("requestdate"))
            dtpPRDoneDate.Value = NotNull_Date(readerrequest("requestdonedate"))
            tbPRRequestDetails.Text = NotNull(readerrequest("prayerdetails"))
            tbTestemoney.Text = NotNull(readerrequest("testemony"))
            dtpLatestPrayerDate.Value = NotNull_Date(readerrequest("latest_prayer_date"))
        End While

        readerrequest.Close()
        readerrequest.Dispose()
        cmdrequest.Dispose()
        conn.close()

        Button2.Enabled = True
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim words As String() = ComboBox2.Text.Split(New Char() {"|"c})

        Dim cmdrequest As New MySqlCommand("SELECT * FROM prophecy WHERE prophecyid = @PID;", conn)
        cmdrequest.Parameters.AddWithValue("@PID", words(0))

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerrequest As MySqlDataReader = cmdrequest.ExecuteReader()

        While readerrequest.Read()
            prophecyid = readerrequest("prophecyid")
            TextBox1.Text = NotNull(readerrequest("prophecytitle"))
            DateTimePicker2.Value = NotNull_Date(readerrequest("prophecydate"))
            RichTextBox2.Text = NotNull(readerrequest("prophecydetails"))
            RichTextBox1.Text = NotNull(readerrequest("testemony"))
        End While

        readerrequest.Close()
        readerrequest.Dispose()
        cmdrequest.Dispose()
        conn.close()

        Button6.Enabled = True
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        TextBox1.Text = ""
        DateTimePicker2.Value = DateTime.Now.Date
        RichTextBox1.Text = ""
        RichTextBox2.Text = ""
        ComboBox2.Text = ""


        Dim cmd As New MySqlCommand("Select COALESCE(MAX(CAST(prophecyid As unsigned)), 0) as prophecyid From prophecy;", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            prophecyid = reader("prophecyid") + 1
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()

        Button6.Enabled = True
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim result = MsgBox("Are you sure you want to save?", MsgBoxStyle.OkCancel)

        If result = 1 And TextBox1.Text <> String.Empty Then
            PleaseWait.Show()
            Dim cmd_del As New MySqlCommand(String.Format("DELETE FROM prophecy WHERE prophecyid=@PID"), conn)
            cmd_del.Parameters.AddWithValue("@PID", prophecyid)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader_del As MySqlDataReader = cmd_del.ExecuteReader()
            reader_del.Close()
            reader_del.Dispose()
            cmd_del.Dispose()
            conn.Close()

            Dim cmd As New MySqlCommand(String.Format("INSERT INTO prophecy VALUES(@ID,@PID,@Ptitle,@PDate,@PDetails,@testemony)"), conn)
            cmd.Parameters.AddWithValue("@ID", prophecyid)
            cmd.Parameters.AddWithValue("@PID", tbPartnerID.Text)
            cmd.Parameters.AddWithValue("@Ptitle", TextBox1.Text)
            cmd.Parameters.AddWithValue("@PDate", DateTimePicker2.Value.Date)
            cmd.Parameters.AddWithValue("@PDetails", RichTextBox2.Text)
            cmd.Parameters.AddWithValue("@testemony", RichTextBox1.Text)


            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            'MsgBox(reader)
            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.CLose()
            PleaseWait.Close()
            MsgBox("Prophecy Saved!")
        Else
            MsgBox("Prophecy title cannot be empty")
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        'Dim input = InputBox("Enter the Category ID to change")

        'If input = String.Empty Then
        '    MsgBox("Please Enter a valid Category ID")
        'Else

        '    'getting partner info
        '    Dim partneridAsInt As Integer
        '    If Integer.TryParse(input, partneridAsInt) Then
        '        Dim cmd As New MySqlCommand("SELECT * FROM category WHERE categoryid=@PID", conn)
        '        cmd.Parameters.AddWithValue("@PID", input)

        '        If conn.State = ConnectionState.Closed Then
        '            conn.Open()
        '        End If
        '        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        '        While reader.Read()
        '            categoryid = reader("categoryid")
        '            tbCategory.Text = NotNull(reader("categoryname"))

        '        End While

        '        reader.Close()
        '        reader.Dispose()
        '        cmd.Dispose()
        '        conn.close()
        '    End If

        'End If
        ViewCategory.ShowDialog()
        If PublicModule.VCCategoryID <> String.Empty Then
            categoryid = PublicModule.VCCategoryID
            tbCategory.Text = PublicModule.VCCategoryName
        End If

    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        'Dim input = InputBox("Enter the Partner ID to change the request for")

        'If input = String.Empty Then
        '    MsgBox("Please Enter a valid Partner ID")
        'Else

        '    'getting partner info
        '    Dim partneridAsInt As Integer
        '    If Integer.TryParse(input, partneridAsInt) Then
        '        Dim cmd As New MySqlCommand("SELECT partnerid, partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
        '        cmd.Parameters.AddWithValue("@PID", input)

        '        If conn.State = ConnectionState.Closed Then
        '            conn.Open()
        '        End If
        '        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        '        While reader.Read()
        '            requestforid = reader("partnerid")
        '            tbRequestFor.Text = NotNull(reader("partnername")) + " " + NotNull(reader("middlename")) + " " + NotNull(reader("lastname"))

        '        End While

        '        reader.Close()
        '        reader.Dispose()
        '        cmd.Dispose()
        '        conn.close()
        '    End If

        'End If
        SearchPartners.ShowDialog()
        If PublicModule.SPPartnerID <> String.Empty Then
            requestforid = PublicModule.SPPartnerID
            tbRequestFor.Text = PublicModule.SPPartnerName
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        tbRequesttitle.Text = ""
        tbRequestFor.Text = ""
        tbCategory.Text = ""
        dtpPRDate.Value = Date.Parse("01-01-2000")
        dtpPRDoneDate.Value = Date.Parse("01-01-2000")
        dtpLatestPrayerDate.Value = Date.Parse("01-01-2000")
        tbPRRequestDetails.Text = ""
        tbTestemoney.Text = ""
        ComboBox1.Text = ""

        Dim cmd As New MySqlCommand("Select COALESCE(MAX(CAST(requestid As unsigned)), 0) as requestid From request;", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            requestid = reader("requestid") + 1
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()
        Button2.Enabled = True
        Button3.Enabled = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim result = MsgBox("Are you sure you want to save?", MsgBoxStyle.OkCancel)

        If result = 1 And tbRequesttitle.Text <> String.Empty And tbCategory.Text <> String.Empty And tbRequestFor.Text <> String.Empty Then
            PleaseWait.Show()
            Dim cmd_del As New MySqlCommand(String.Format("DELETE FROM request WHERE requestid=@PID"), conn)
            cmd_del.Parameters.AddWithValue("@PID", requestid)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader_del As MySqlDataReader = cmd_del.ExecuteReader()
            reader_del.Close()
            reader_del.Dispose()
            cmd_del.Dispose()
            'conn.Close()

            Dim cmd As New MySqlCommand(String.Format("INSERT INTO request VALUES(@RID,@PID,@ReID,@RDate,@RTitle,@RDetails,@RDoneDate,@testemony,@Category,@RLastestPrayerDate)"), conn)
            cmd.Parameters.AddWithValue("@RID", requestid)
            cmd.Parameters.AddWithValue("@PID", tbPartnerID.Text)
            cmd.Parameters.AddWithValue("@ReID", requestforid)
            cmd.Parameters.AddWithValue("@Rtitle", tbRequesttitle.Text)
            cmd.Parameters.AddWithValue("@RDate", dtpPRDate.Value.Date)
            cmd.Parameters.AddWithValue("@RDetails", tbPRRequestDetails.Text)
            cmd.Parameters.AddWithValue("@testemony", tbTestemoney.Text)
            cmd.Parameters.AddWithValue("@RDoneDate", dtpPRDoneDate.Value.Date)
            cmd.Parameters.AddWithValue("@Category", categoryid)
            cmd.Parameters.AddWithValue("@RLastestPrayerDate", dtpLatestPrayerDate.Value.Date)


            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            cmd.ExecuteReader()
            'MsgBox(reader)
            reader_del.Close()
            reader_del.Dispose()
            cmd.Dispose()
            conn.Close()
            PleaseWait.Close()
            MsgBox("Request Saved!")
        Else
            MsgBox("'Request Title', 'Request for' and 'Category' cannot be empty." & vbCrLf & " If the request is for self put in the his/her own Partner ID")
        End If
    End Sub

    Private Sub LoadFromPartnerSearch()
        tbPartnerID.Text = SPPartnerID
        Clear()
        Dim partnerid = tbPartnerID.Text
        Dim cmd As New MySqlCommand("SELECT partnerid, partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
        cmd.Parameters.AddWithValue("@PID", partnerid)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            tbFIrstName.Text = NotNull(reader("partnername"))
            tbMiddleName.Text = NotNull(reader("middlename"))
            tbLastName.Text = NotNull(reader("lastname"))
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()

        'getting prayer details
        Dim cmdrequest As New MySqlCommand("SELECT requestid, requesttitle from request where partnerid = @PID;", conn)
        cmdrequest.Parameters.AddWithValue("@PID", tbPartnerID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerrequest As MySqlDataReader = cmdrequest.ExecuteReader()

        While readerrequest.Read()
            ComboBox1.Items.Add(NotNull(readerrequest("requestid")) + " | " + NotNull(readerrequest("requesttitle")))
        End While

        readerrequest.Close()
        readerrequest.Dispose()
        cmdrequest.Dispose()
        conn.close()

        'Get phrophecy details

        Dim cmdprophecy As New MySqlCommand("SELECT prophecyid, prophecytitle from prophecy where partnerid = @PID;", conn)
        cmdprophecy.Parameters.AddWithValue("@PID", tbPartnerID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerprophecy As MySqlDataReader = cmdprophecy.ExecuteReader()

        While readerprophecy.Read()
            ComboBox2.Items.Add(NotNull(readerprophecy("prophecyid")) + " | " + NotNull(readerprophecy("prophecytitle")))
        End While

        readerprophecy.Close()
        readerprophecy.Dispose()
        cmdprophecy.Dispose()
        conn.close()

        Button1.Enabled = True
        Button7.Enabled = True
    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles dtpLatestPrayerDate.ValueChanged

    End Sub
End Class