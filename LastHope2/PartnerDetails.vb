Imports MySql.Data.MySqlClient
Public Class PartnerDetails
    Dim conn
    Dim db
    Dim dtAll As New DataTable()
    Dim areaid As String
    Dim arrImage() As Byte


    Public Function NotNull(ByVal Value)
        If Value Is Nothing OrElse IsDBNull(Value) Then
            Return ""
        Else
            Return Value
        End If
    End Function

    Public Function NotNull_Date(ByVal Value)
        If Value Is Nothing OrElse IsDBNull(Value) Then
            Return Date.Parse("1900-01-01")
        Else
            Return Value
        End If
    End Function

    Public Function NotNull_num(ByVal Value)
        If Value Is Nothing OrElse IsDBNull(Value) Then
            Return 0
        Else
            Return Value
        End If
    End Function

    Private Sub PartnerDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button7.Enabled = False
        tbPartnerID.Enabled = False

        dtpDateOfBaptism.Value = Date.Parse("01-01-1990")
        dtpDateOfBirth.Value = Date.Parse("01-01-1990")
        dtpDateOfEnrollment.Value = Date.Parse("01-01-1990")
        dtpDateOfInactivity.Value = Date.Parse("01-01-1990")
        dtpDateOfMembershipEligibility.Value = Date.Parse("01-01-1990")
        dtpDeathDate.Value = Date.Parse("01-01-1990")
        dtpWeddingDate.Value = Date.Parse("01-01-1990")
    End Sub

    Private Sub btnPartnerIDSearch_Click(sender As Object, e As EventArgs) Handles btnPartnerIDSearch.Click
        Dim input = InputBox("Enter the Partner ID to show details")

        If input = String.Empty Then
            MsgBox("Please Enter a valid Partner ID")
        Else
            Clear()
            PleaseWait.Show()
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                tbPartnerID.Text = input
                dtAll = New DataTable()

                Dim partnerid = tbPartnerID.Text

                Dim cmd As New MySqlCommand("SELECT partner.*,area.* FROM partner left join area on partner.areaid=area.AreaId WHERE partnerid=@PID", conn)
                cmd.Parameters.AddWithValue("@PID", partnerid)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    tbPartnerID.Text = NotNull(reader("partnerid"))
                    cbRegular.Checked = NotNull_num(reader("regular"))
                    cbActive.Checked = NotNull_num(reader("active"))
                    cbRetreat.Checked = NotNull_num(reader("retreat"))
                    cbFullFamily.Checked = NotNull_num(reader("fullfamily"))
                    cbDonor.Checked = NotNull_num(reader("donor"))
                    cbDontVisit.Checked = NotNull_num(reader("dontvisit"))

                    Try
                        arrImage = reader("photo")
                        Dim mstream As New IO.MemoryStream(arrImage)
                        PictureBox1.Image = System.Drawing.Image.FromStream(mstream)
                    Catch
                        PictureBox1.Image = My.Resources.no_img
                    End Try
                    areaid = NotNull(reader("areaid"))
                    tbFIrstName.Text = NotNull(reader("partnername"))
                    tbMiddleName.Text = NotNull(reader("middlename"))
                    tbLastName.Text = NotNull(reader("lastname"))
                    tbBiometricID.Text = NotNull(reader("biometricid"))
                    tbBiometricCardNumber.Text = NotNull(reader("biometriccardnumber"))
                    cmbGender.Text = NotNull(reader("gender"))
                    dtpDateOfBaptism.Value = NotNull_Date(reader("baptismdate"))
                    dtpDateOfBirth.Value = NotNull_Date(reader("birthdate"))
                    dtpDateOfEnrollment.Value = NotNull_Date(reader("dateofenrollment"))
                    dtpDateOfInactivity.Value = NotNull_Date(reader("inactivedate"))
                    dtpDateOfMembershipEligibility.Value = NotNull_Date(reader("dateofmembershipeligibility"))
                    dtpDeathDate.Value = NotNull_Date(reader("deathdate"))
                    dtpWeddingDate.Value = NotNull_Date(reader("weddingdate"))
                    tbAddress.Text = NotNull(reader("address"))
                    tbEmail.Text = NotNull(reader("email"))
                    tbMobile.Text = NotNull(reader("mobileno"))
                    tbPhone.Text = NotNull(reader("phoneno"))
                    tbAreaName.Text = NotNull(reader("AreaName"))
                    tbCity.Text = NotNull(reader("City"))
                    tbState.Text = NotNull(reader("state"))
                    tbCountry.Text = NotNull(reader("country"))
                    tbPincode.Text = NotNull(reader("Pincode"))
                    cmbRegion.Text = NotNull(reader("Region"))
                    tbReferencePartnerID.Text = NotNull(reader("referencepartnerid"))
                    tbMainLeaderID.Text = NotNull(reader("followedid1"))
                    tbSubLeader2ID.Text = NotNull(reader("followedid2"))
                    tbSubLeader3ID.Text = NotNull(reader("followedid3"))
                    tbSubLeader4ID.Text = NotNull(reader("followedid4"))
                    tbVisitDetails.Text = NotNull(reader("visitremarks"))
                    'dtpLatestPrayerDate.Value = NotNull_Date(reader("latestprayerdate"))
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()

                Dim cmdref As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmdref.Parameters.AddWithValue("@PID", tbReferencePartnerID.Text)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim readerref As MySqlDataReader = cmdref.ExecuteReader()

                While readerref.Read()
                    tbReferanceName.Text = readerref("partnername") + " " + readerref("middlename") + " " + readerref("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.close()

                Dim cmdmainleader As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmdmainleader.Parameters.AddWithValue("@PID", tbMainLeaderID.Text)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim readermailleader As MySqlDataReader = cmdmainleader.ExecuteReader()

                While readermailleader.Read()
                    tbMainLeaderName.Text = readermailleader("partnername") + " " + readermailleader("middlename") + " " + readermailleader("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.close()

                Dim cmdsubleader2 As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmdsubleader2.Parameters.AddWithValue("@PID", tbSubLeader2ID.Text)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim readersubleader2 As MySqlDataReader = cmdsubleader2.ExecuteReader()

                While readersubleader2.Read()
                    tbSubLeader2Name.Text = readersubleader2("partnername") + " " + readersubleader2("middlename") + " " + readersubleader2("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.close()

                Dim cmdsubleader3 As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmdsubleader3.Parameters.AddWithValue("@PID", tbSubLeader3ID.Text)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim readersubleader3 As MySqlDataReader = cmdsubleader3.ExecuteReader()

                While readersubleader3.Read()
                    tbSubLeader3Name.Text = readersubleader3("partnername") + " " + readersubleader3("middlename") + " " + readersubleader3("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.close()

                Dim cmdsubleader4 As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmdsubleader4.Parameters.AddWithValue("@PID", tbSubLeader4ID.Text)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim readersubleader4 As MySqlDataReader = cmdsubleader4.ExecuteReader()

                While readersubleader4.Read()
                    tbSubLeader4Name.Text = readersubleader4("partnername") + " " + readersubleader4("middlename") + " " + readersubleader4("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.close()

                'Relatives info
                Dim cmdrelatives As New MySqlCommand("SELECT partner.partnerid, partner.partnername,partner.middlename,partner.lastname,familytree.relationname,area.AreaName,partner.birthdate,partner.baptismdate,partner.regular,partner.active,partner.phoneno,partner.mobileno 
                                                FROM partner, familytree, area
                                                Where partner.partnerid=familytree.relativeid and familytree.partnerid=@PID and partner.areaid=area.AreaId;", conn)
                cmdrelatives.Parameters.AddWithValue("@PID", tbPartnerID.Text)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim readerrelatives As MySqlDataReader = cmdrelatives.ExecuteReader()


                dtAll.Load(readerrelatives)
                DataGridView1.AutoGenerateColumns = True
                DataGridView1.DataSource = dtAll
                DataGridView1.Refresh()

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.close()

                Button1.Enabled = True
                Button2.Enabled = True
                Button3.Enabled = True
                Button7.Enabled = True
            Else
                MsgBox("Enter a numeric value for Partner ID")
            End If
        End If
        PleaseWait.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PublicModule.ARParntnerID = tbPartnerID.Text
        AddNewRelation.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PublicModule.ERPartnerID = tbPartnerID.Text
        PublicModule.relationtable = dtAll
        EditRelation.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim partnerid = InputBox("Enter the Partner ID to delete from Family Details")
        If partnerid = String.Empty Then
            MsgBox("Please Enter a Partner ID")
        Else
            Dim partneridAsInt As Integer
            If Integer.TryParse(partnerid, partneridAsInt) Then
                Dim cmd As New MySqlCommand("DELETE FROM familytree WHERE partnerid=@PID AND relativeid=@RID", conn)
                cmd.Parameters.AddWithValue("@PID", tbPartnerID.Text)
                cmd.Parameters.AddWithValue("@RID", partnerid)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.close()
                MsgBox("Relation Deleted!")
            Else
                MsgBox("Invalid Entry! Please enter a number")
            End If

        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        OpenFileDialog1.Title = "Please select a image file"
        OpenFileDialog1.InitialDirectory = "C:\"
        OpenFileDialog1.Filter = "Images Files|*.jpg;*.jpeg;*.png"

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            PictureBox1.Load(OpenFileDialog1.FileName())

        End If
    End Sub



    Private Sub Clear()
        'Clearing values from all controls

        cbRegular.Checked = False
        cbActive.Checked = False
        cbRetreat.Checked = False
        cbFullFamily.Checked = False
        cbDonor.Checked = False
        cbDontVisit.Checked = False
        tbBiometricID.Text = ""
        tbBiometricCardNumber.Text = ""
        tbFIrstName.Text = ""
        tbMiddleName.Text = ""
        tbLastName.Text = ""
        cmbGender.ResetText()
        cmbRegion.ResetText()
        dtpDateOfBaptism.ResetText()
        dtpDateOfBirth.ResetText()
        dtpDateOfEnrollment.ResetText()
        dtpDateOfInactivity.ResetText()
        dtpDateOfMembershipEligibility.ResetText()
        dtpDeathDate.ResetText()
        dtpWeddingDate.ResetText()
        tbAddress.Text = ""
        tbAreaName.Text = ""
        tbCity.Text = ""
        tbCountry.Text = ""
        tbEmail.Text = ""
        tbMobile.Text = ""
        tbPhone.Text = ""
        tbPincode.Text = ""
        tbState.Text = ""
        tbMainLeaderID.Text = ""
        tbMainLeaderName.Text = ""
        tbSubLeader2ID.Text = ""
        tbSubLeader2Name.Text = ""
        tbSubLeader3ID.Text = ""
        tbSubLeader3Name.Text = ""
        tbSubLeader4ID.Text = ""
        tbSubLeader4Name.Text = ""
        tbVisitDetails.Text = ""
        tbReferanceName.Text = ""
        tbReferencePartnerID.Text = ""

        PictureBox1.Image = My.Resources.no_img
        OpenFileDialog1.FileName = ""

        DataGridView1.DataSource = New DataTable()
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.Refresh()

        dtpDateOfBaptism.Value = Date.Parse("01-01-1990")
        dtpDateOfBirth.Value = Date.Parse("01-01-1990")
        dtpDateOfEnrollment.Value = Date.Parse("01-01-1990")
        dtpDateOfInactivity.Value = Date.Parse("01-01-1990")
        dtpDateOfMembershipEligibility.Value = Date.Parse("01-01-1990")
        dtpDeathDate.Value = Date.Parse("01-01-1990")
        dtpWeddingDate.Value = Date.Parse("01-01-1990")

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Clear()
        dtpDateOfEnrollment.Value = DateAndTime.Now.Date
        dtpDateOfMembershipEligibility.Value = dtpDateOfEnrollment.Value.AddMonths(3)
        tbCountry.Text = "India"
        tbState.Text = "Maharashtra"
        areaid = 0
        cbActive.Checked = True

        Dim cmd As New MySqlCommand("Select MAX(CAST(partnerid As unsigned)) as partnerid From partner;", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            tbPartnerID.Text = reader("partnerid") + 1
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()
        Button7.Enabled = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SearchPartners.ShowDialog()
        If SPPartnerID <> String.Empty Then
            LoadFromPartnerSearch()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ViewArea.ShowDialog()
        If SPAreaID <> String.Empty Then
            Dim cmd As New MySqlCommand("SELECT * FROM area WHERE AreaId=@AID", conn)
            cmd.Parameters.AddWithValue("@AID", PublicModule.SPAreaID)

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                areaid = NotNull_num(reader("AreaId"))
                tbAreaName.Text = NotNull(reader("AreaName"))
                cmbRegion.Text = NotNull(reader("Region"))
                tbPincode.Text = NotNull(reader("Pincode"))
                tbCity.Text = NotNull(reader("City"))
            End While
            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()
        End If
    End Sub

    Private Sub btnAreaNameSearch_Click(sender As Object, e As EventArgs) Handles btnAreaNameSearch.Click
        Dim input = InputBox("Enter the Area ID")

        If input = String.Empty Then
            MsgBox("Please Enter a valid Area ID")
        Else
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                Dim cmd As New MySqlCommand("SELECT * FROM area WHERE AreaId=@AID", conn)
                cmd.Parameters.AddWithValue("@AID", input)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    areaid = NotNull_num(reader("AreaId"))
                    tbAreaName.Text = NotNull(reader("AreaName"))
                    cmbRegion.Text = NotNull(reader("Region"))
                    tbPincode.Text = NotNull(reader("Pincode"))
                    tbCity.Text = NotNull(reader("City"))
                End While
                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()

            Else
                MsgBox("Enter a numeric value for Area Id")
            End If
        End If

    End Sub

    Private Sub btnReferencePartnerSearch_Click(sender As Object, e As EventArgs) Handles btnReferencePartnerSearch.Click
        SearchPartners.ShowDialog()
        If SPPartnerID <> String.Empty Then
            tbReferencePartnerID.Text = PublicModule.SPPartnerID
            tbReferanceName.Text = PublicModule.SPPartnerName

        End If


    End Sub

    Private Sub btnMainLeaderIDSearch_Click(sender As Object, e As EventArgs) Handles btnMainLeaderIDSearch.Click
        SearchLeaders.ShowDialog()
        If SLPartnerID <> String.Empty Then
            tbMainLeaderID.Text = PublicModule.SLPartnerID
            tbMainLeaderName.Text = PublicModule.SLPartnerName
        End If

    End Sub

    Private Sub btnSubLeader2IDSearch_Click(sender As Object, e As EventArgs) Handles btnSubLeader2IDSearch.Click
        SearchLeaders.ShowDialog()
        If SLPartnerID <> String.Empty Then
            tbSubLeader2ID.Text = PublicModule.SLPartnerID
            tbSubLeader2Name.Text = PublicModule.SLPartnerName

        End If

    End Sub

    Private Sub btnSubLeader3IDSearch_Click(sender As Object, e As EventArgs) Handles btnSubLeader3IDSearch.Click
        SearchLeaders.ShowDialog()
        If PublicModule.SLPartnerID <> String.Empty Then
            tbSubLeader3ID.Text = PublicModule.SLPartnerID
            tbSubLeader3Name.Text = PublicModule.SLPartnerName

        End If

    End Sub

    Private Sub btnSubLeader4IDSearch_Click(sender As Object, e As EventArgs) Handles btnSubLeader4IDSearch.Click
        SearchLeaders.ShowDialog()
        If PublicModule.SLPartnerID <> String.Empty Then
            tbSubLeader4ID.Text = PublicModule.SLPartnerID
            tbSubLeader4Name.Text = PublicModule.SLPartnerName

        End If

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim result = MsgBox("Are you sure you want to save?", MsgBoxStyle.OkCancel)

        If result = 1 Then
            PleaseWait.Show()
            Dim cmd_del As New MySqlCommand(String.Format("DELETE FROM partner WHERE PartnerID=@PartnerID"), conn)
            cmd_del.Parameters.AddWithValue("@PartnerID", tbPartnerID.Text)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader_del As MySqlDataReader = cmd_del.ExecuteReader()
            reader_del.Close()
            reader_del.Dispose()
            cmd_del.Dispose()
            conn.Close()

            Dim cmd As New MySqlCommand(String.Format("INSERT INTO partner VALUES(@ID,@FirstName,
                                                        @DateOfEnrollment,@Active,@InavtiveDate,@DateOfBirth,@DateofBaptism,
                                                        @weddingDate,@DeathDate,@Address,@AreaID,@MobileNumber,
                                                        @PhoneNo,@ReferencePartnerID,@MainLeaderID,@SubLeader2ID,@SubLeader3ID,
                                                        @SubLeader4ID,@EntryDate,@Regular,@MiddleName, @State, @Country,@LastName,
                                                        @Gender,@photo,@FullFamily,@EmailID,@Donor,@Retreat,@VisitRemarks,
                                                        @BiometicID,@BiometricCardNumber, @DateOfMembershipEligibility,@DontVisit)"), conn)
            cmd.Parameters.AddWithValue("@ID", tbPartnerID.Text)
            cmd.Parameters.AddWithValue("@Regular", cbRegular.Checked)
            cmd.Parameters.AddWithValue("@Active", cbActive.Checked)
            cmd.Parameters.AddWithValue("@Retreat", cbRetreat.Checked)
            cmd.Parameters.AddWithValue("@FullFamily", cbFullFamily.Checked)
            cmd.Parameters.AddWithValue("@Donor", cbDonor.Checked)
            cmd.Parameters.AddWithValue("@DontVisit", cbDontVisit.Checked)
            If OpenFileDialog1.FileName = "" Then
                cmd.Parameters.AddWithValue("@photo", arrImage)
            Else
                Dim filesize As UInt32
                Dim mstream As New IO.MemoryStream()
                PictureBox1.Image.Save(mstream, System.Drawing.Imaging.ImageFormat.Png)
                Dim arrImg() As Byte = mstream.GetBuffer
                filesize = mstream.Length
                mstream.Close()
                cmd.Parameters.AddWithValue("@photo", arrImg)
            End If
            cmd.Parameters.AddWithValue("@FirstName", tbFIrstName.Text)
            cmd.Parameters.AddWithValue("@MiddleName", tbMiddleName.Text)
            cmd.Parameters.AddWithValue("@LastName", tbLastName.Text)
            cmd.Parameters.AddWithValue("@Gender", cmbGender.Text)
            cmd.Parameters.AddWithValue("@DateOfEnrollment", dtpDateOfEnrollment.Value.Date)
            cmd.Parameters.AddWithValue("@InavtiveDate", dtpDateOfInactivity.Value.Date)
            cmd.Parameters.AddWithValue("@DateOfBirth", dtpDateOfBirth.Value.Date)
            cmd.Parameters.AddWithValue("@DateofBaptism", dtpDateOfBaptism.Value.Date)
            cmd.Parameters.AddWithValue("@weddingDate", dtpWeddingDate.Value.Date)
            cmd.Parameters.AddWithValue("@DeathDate", dtpDeathDate.Value.Date)
            cmd.Parameters.AddWithValue("@DateOfMembershipEligibility", dtpDateOfMembershipEligibility.Value.Date)
            cmd.Parameters.AddWithValue("@BiometicID", tbBiometricID.Text)
            cmd.Parameters.AddWithValue("@BiometricCardNumber", tbBiometricCardNumber.Text)
            cmd.Parameters.AddWithValue("@Address", tbAddress.Text)
            cmd.Parameters.AddWithValue("@AreaID", areaid)
            cmd.Parameters.AddWithValue("@MobileNumber", tbMobile.Text)
            cmd.Parameters.AddWithValue("@PhoneNo", tbPhone.Text)
            cmd.Parameters.AddWithValue("@EmailID", tbEmail.Text)
            cmd.Parameters.AddWithValue("@ReferencePartnerID", tbReferencePartnerID.Text)
            cmd.Parameters.AddWithValue("@VisitRemarks", tbVisitDetails.Text)
            cmd.Parameters.AddWithValue("@MainLeaderID", tbMainLeaderID.Text)
            cmd.Parameters.AddWithValue("@SubLeader2ID", tbSubLeader2ID.Text)
            cmd.Parameters.AddWithValue("@SubLeader3ID", tbSubLeader3ID.Text)
            cmd.Parameters.AddWithValue("@SubLeader4ID", tbSubLeader4ID.Text)
            'cmd.Parameters.AddWithValue("@City", tbCity.Text)
            'cmd.Parameters.AddWithValue("@Pincode", tbPincode.Text)
            cmd.Parameters.AddWithValue("@EntryDate", Date.Now.Date)
            cmd.Parameters.AddWithValue("@State", tbState.Text)
            cmd.Parameters.AddWithValue("@Country", tbCountry.Text)
            'cmd.Parameters.AddWithValue("@LatestPrayerDate", dtpLatestPrayerDate.Value.Date)

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
            MsgBox("Partner Saved!")
        End If

    End Sub

    Private Sub LoadFromPartnerSearch()
        tbPartnerID.Text = SPPartnerID
        dtAll = New DataTable()

        Dim partnerid = tbPartnerID.Text

        Dim cmd As New MySqlCommand("SELECT partner.*,area.* FROM partner left join area on partner.areaid=area.AreaId WHERE partnerid=@PID", conn)
        cmd.Parameters.AddWithValue("@PID", partnerid)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            tbPartnerID.Text = NotNull(reader("partnerid"))
            cbRegular.Checked = NotNull_num(reader("regular"))
            cbActive.Checked = NotNull_num(reader("active"))
            cbRetreat.Checked = NotNull_num(reader("retreat"))
            cbFullFamily.Checked = NotNull_num(reader("fullfamily"))
            cbDonor.Checked = NotNull_num(reader("donor"))
            cbDontVisit.Checked = NotNull_num(reader("dontvisit"))

            Try
                arrImage = reader("photo")
                Dim mstream As New IO.MemoryStream(arrImage)
                PictureBox1.Image = System.Drawing.Image.FromStream(mstream)
            Catch
                PictureBox1.Image = My.Resources.no_img
            End Try
            areaid = NotNull(reader("areaid"))
            tbFIrstName.Text = NotNull(reader("partnername"))
            tbMiddleName.Text = NotNull(reader("middlename"))
            tbLastName.Text = NotNull(reader("lastname"))
            tbBiometricID.Text = NotNull(reader("biometricid"))
            tbBiometricCardNumber.Text = NotNull(reader("biometriccardnumber"))
            cmbGender.Text = NotNull(reader("gender"))
            dtpDateOfBaptism.Value = NotNull_Date(reader("baptismdate"))
            dtpDateOfBirth.Value = NotNull_Date(reader("birthdate"))
            dtpDateOfEnrollment.Value = NotNull_Date(reader("dateofenrollment"))
            dtpDateOfInactivity.Value = NotNull_Date(reader("inactivedate"))
            dtpDateOfMembershipEligibility.Value = NotNull_Date(reader("dateofmembershipeligibility"))
            dtpDeathDate.Value = NotNull_Date(reader("deathdate"))
            dtpWeddingDate.Value = NotNull_Date(reader("weddingdate"))
            tbAddress.Text = NotNull(reader("address"))
            tbEmail.Text = NotNull(reader("email"))
            tbMobile.Text = NotNull(reader("mobileno"))
            tbPhone.Text = NotNull(reader("phoneno"))
            tbAreaName.Text = NotNull(reader("AreaName"))
            tbCity.Text = NotNull(reader("City"))
            tbState.Text = NotNull(reader("state"))
            tbCountry.Text = NotNull(reader("country"))
            tbPincode.Text = NotNull(reader("Pincode"))
            cmbRegion.Text = NotNull(reader("Region"))
            tbReferencePartnerID.Text = NotNull(reader("referencepartnerid"))
            tbMainLeaderID.Text = NotNull(reader("followedid1"))
            tbSubLeader2ID.Text = NotNull(reader("followedid2"))
            tbSubLeader3ID.Text = NotNull(reader("followedid3"))
            tbSubLeader4ID.Text = NotNull(reader("followedid4"))
            tbVisitDetails.Text = NotNull(reader("visitremarks"))
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()

        Dim cmdref As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
        cmdref.Parameters.AddWithValue("@PID", tbReferencePartnerID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerref As MySqlDataReader = cmdref.ExecuteReader()

        While readerref.Read()
            tbReferanceName.Text = readerref("partnername") + " " + readerref("middlename") + " " + readerref("lastname")
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()

        Dim cmdmainleader As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
        cmdmainleader.Parameters.AddWithValue("@PID", tbMainLeaderID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readermailleader As MySqlDataReader = cmdmainleader.ExecuteReader()

        While readermailleader.Read()
            tbMainLeaderName.Text = readermailleader("partnername") + " " + readermailleader("middlename") + " " + readermailleader("lastname")
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()

        Dim cmdsubleader2 As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
        cmdsubleader2.Parameters.AddWithValue("@PID", tbSubLeader2ID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readersubleader2 As MySqlDataReader = cmdsubleader2.ExecuteReader()

        While readersubleader2.Read()
            tbSubLeader2Name.Text = readersubleader2("partnername") + " " + readersubleader2("middlename") + " " + readersubleader2("lastname")
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()

        Dim cmdsubleader3 As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
        cmdsubleader3.Parameters.AddWithValue("@PID", tbSubLeader3ID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readersubleader3 As MySqlDataReader = cmdsubleader3.ExecuteReader()

        While readersubleader3.Read()
            tbSubLeader3Name.Text = readersubleader3("partnername") + " " + readersubleader3("middlename") + " " + readersubleader3("lastname")
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()

        Dim cmdsubleader4 As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
        cmdsubleader4.Parameters.AddWithValue("@PID", tbSubLeader4ID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readersubleader4 As MySqlDataReader = cmdsubleader4.ExecuteReader()

        While readersubleader4.Read()
            tbSubLeader4Name.Text = readersubleader4("partnername") + " " + readersubleader4("middlename") + " " + readersubleader4("lastname")
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()

        'Relatives info
        Dim cmdrelatives As New MySqlCommand("SELECT partner.partnerid, partner.partnername,partner.middlename,partner.lastname,familytree.relationname,area.AreaName,partner.birthdate,partner.baptismdate,partner.regular,partner.active,partner.phoneno,partner.mobileno 
                                                FROM partner, familytree, area
                                                Where partner.partnerid=familytree.relativeid and familytree.partnerid=@PID and partner.areaid=area.AreaId;", conn)
        cmdrelatives.Parameters.AddWithValue("@PID", tbPartnerID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerrelatives As MySqlDataReader = cmdrelatives.ExecuteReader()


        dtAll.Load(readerrelatives)
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = dtAll
        DataGridView1.Refresh()

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()

        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button7.Enabled = True

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        'Doesnt clear everything

        dtpDateOfEnrollment.Value = DateAndTime.Now.Date
        dtpDateOfMembershipEligibility.Value = dtpDateOfEnrollment.Value.AddMonths(3)
        tbCountry.Text = "India"
        tbState.Text = "Maharashtra"
        cbActive.Checked = True
        tbReferanceName.Clear()
        tbReferencePartnerID.Clear()
        tbVisitDetails.Clear()
        cbActive.Checked = True


        Dim cmd As New MySqlCommand("Select MAX(CAST(partnerid As unsigned)) as partnerid From partner;", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            tbPartnerID.Text = reader("partnerid") + 1
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()
        Button7.Enabled = True

        tbFIrstName.Text = ""
        tbMiddleName.Text = ""
        tbLastName.Text = ""
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        PictureBox1.Image = My.Resources.no_img
        OpenFileDialog1.FileName = "No Img"
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        tbMainLeaderID.Clear()
        tbMainLeaderName.Clear()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        tbSubLeader2ID.Clear()
        tbSubLeader2Name.Clear()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        tbSubLeader3ID.Clear()
        tbSubLeader3Name.Clear()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        tbSubLeader4ID.Clear()
        tbSubLeader4Name.Clear()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        tbReferanceName.Clear()
        tbReferencePartnerID.Clear()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        tbAreaName.Clear()
        tbState.Clear()
        tbCity.Clear()
        tbCountry.Clear()
        tbPincode.Clear()
        areaid = 0
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        Dim input = tbReferencePartnerID.Text

        If input = String.Empty Then
            MsgBox("Please Enter a value for Reference ID first")
        Else
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                Dim cmd As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmd.Parameters.AddWithValue("@PID", input)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    tbReferanceName.Text = reader("partnername") + " " + reader("middlename") + " " + reader("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()
            Else
                MsgBox("Enter a numeric value for Reference ID")
            End If
        End If
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        Dim input = tbMainLeaderID.Text

        If input = String.Empty Then
            MsgBox("Please Enter a value for Main Leader ID first")
        Else
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                Dim cmd As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmd.Parameters.AddWithValue("@PID", input)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    tbMainLeaderName.Text = reader("partnername") + " " + reader("middlename") + " " + reader("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()
            Else
                MsgBox("Enter a numeric value for Leader ID")
            End If
        End If
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        Dim input = tbSubLeader2ID.Text

        If input = String.Empty Then
            MsgBox("Please Enter a value for Sub Leader ID first")
        Else
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                Dim cmd As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmd.Parameters.AddWithValue("@PID", input)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    tbSubLeader2Name.Text = reader("partnername") + " " + reader("middlename") + " " + reader("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()
            Else
                MsgBox("Enter a numeric value for Leader ID")
            End If
        End If
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        Dim input = tbSubLeader3ID.Text

        If input = String.Empty Then
            MsgBox("Please Enter a value for Sub Leader ID first")
        Else
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                Dim cmd As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmd.Parameters.AddWithValue("@PID", input)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    tbSubLeader3Name.Text = reader("partnername") + " " + reader("middlename") + " " + reader("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()
            Else
                MsgBox("Enter a numeric value for Leader ID")
            End If
        End If
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Dim input = tbSubLeader4ID.Text

        If input = String.Empty Then
            MsgBox("Please Enter a value for Sub Leader ID first")
        Else
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                Dim cmd As New MySqlCommand("SELECT partnername, middlename, lastname FROM partner WHERE partnerid=@PID", conn)
                cmd.Parameters.AddWithValue("@PID", input)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If
                Dim reader As MySqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    tbSubLeader4Name.Text = reader("partnername") + " " + reader("middlename") + " " + reader("lastname")
                End While

                reader.Close()
                reader.Dispose()
                cmd.Dispose()
                conn.Close()
            Else
                MsgBox("Enter a numeric value for Leader ID")
            End If
        End If
    End Sub
End Class