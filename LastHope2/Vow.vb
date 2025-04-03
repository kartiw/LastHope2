Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar
Imports MySql.Data.MySqlClient
Public Class Vow
    Dim conn
    Dim db

    Dim vowid As String
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
        dtpVowDate.ResetText()
        tbVowDetails.Text = ""
        tbTestimoney.Text = ""
        tbVowID.Text = ""
        ComboBox1.Items.Clear()

        tbFIrstName.Text = ""
        tbMiddleName.Text = ""
        tbLastName.Text = ""

        dtpVowDate.Value = DateTime.Now.Date
    End Sub

    Private Sub LoadFromPartnerSearch(Optional pID As Integer = Nothing)
        tbPartnerID.Text = SPPartnerID
        If pID <> Nothing Then
            tbPartnerID.Text = pID
        End If
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

        ' Vow detials

        Dim cmdrequest As New MySqlCommand("SELECT vowid from vow where partnerid = @PID;", conn)
        cmdrequest.Parameters.AddWithValue("@PID", tbPartnerID.Text)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readerrequest As MySqlDataReader = cmdrequest.ExecuteReader()

        While readerrequest.Read()
            ComboBox1.Items.Add(NotNull(readerrequest("vowid")))
        End While

        readerrequest.Close()
        readerrequest.Dispose()
        cmdrequest.Dispose()
        conn.close()

        btnVowNew.Enabled = True
    End Sub

    Private Sub Vow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()

        btnVowNew.Enabled = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        SearchPartners.ShowDialog()
        If SPPartnerID <> String.Empty Then
            LoadFromPartnerSearch()
        End If
    End Sub

    Private Sub btnPartnerIDSearch_Click(sender As Object, e As EventArgs) Handles btnPartnerIDSearch.Click
        Dim input = InputBox("Enter the Partner ID to show details")

        If input = String.Empty Then
            MsgBox("Please Enter a valid Partner ID")
        Else

            'getting partner info
            Dim partneridAsInt As Integer
            If Integer.TryParse(input, partneridAsInt) Then
                LoadFromPartnerSearch(input)
            Else
                MsgBox("Enter a numeric value for Partner ID")

            End If

        End If
    End Sub

    Private Sub btnVowNew_Click(sender As Object, e As EventArgs) Handles btnVowNew.Click
        tbVowDetails.Text = ""
        dtpVowDate.Value = DateTime.Now.Date
        tbTestimoney.Text = ""

        Dim cmd As New MySqlCommand("Select COALESCE(MAX(CAST(vowid As unsigned)), 0) as vowid From vow;", conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            vowid = reader("vowid") + 1
            tbVowID.Text = vowid
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.close()
    End Sub

    Private Sub btnVowSave_Click(sender As Object, e As EventArgs) Handles btnVowSave.Click
        Dim result = MsgBox("Are you sure you want to save?", MsgBoxStyle.OkCancel)

        If result = 1 And tbVowDetails.Text <> String.Empty And vowid <> String.Empty Then
            PleaseWait.Show()
            Dim cmd_del As New MySqlCommand(String.Format("DELETE FROM vow WHERE vowid=@VID"), conn)
            cmd_del.Parameters.AddWithValue("@VID", vowid)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader_del As MySqlDataReader = cmd_del.ExecuteReader()
            reader_del.Close()
            reader_del.Dispose()
            cmd_del.Dispose()
            'conn.Close()

            Dim cmd As New MySqlCommand(String.Format("INSERT INTO vow VALUES(@VID,@PID,@VDate,@VDetails,@testimony)"), conn)
            cmd.Parameters.AddWithValue("@VID", vowid)
            cmd.Parameters.AddWithValue("@PID", tbPartnerID.Text)
            cmd.Parameters.AddWithValue("@VDate", dtpVowDate.Value.Date)
            cmd.Parameters.AddWithValue("@VDetails", tbVowDetails.Text)
            cmd.Parameters.AddWithValue("@testimony", tbTestimoney.Text)


            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            cmd.ExecuteReader()
            'MsgBox(vowid)
            reader_del.Close()
            reader_del.Dispose()
            cmd.Dispose()
            conn.Close()
            PleaseWait.Close()
            MsgBox("Vow Saved!")
        Else
            MsgBox("'Vow Details' and 'Vow ID' cannot be empty. Please press the New button if its a new vow. ")
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        vowid = ComboBox1.Text

        Dim cmdvow As New MySqlCommand("SELECT * FROM vow
                                            Where vowid = @VID;", conn)
        cmdvow.Parameters.AddWithValue("@VID", vowid)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim readervow As MySqlDataReader = cmdvow.ExecuteReader()

        While readervow.Read()
            tbVowID.Text = vowid
            dtpVowDate.Value = NotNull_Date(readervow("vowdate"))
            tbVowDetails.Text = NotNull(readervow("vowdetails"))
            tbTestimoney.Text = NotNull(readervow("testimony"))
        End While

        readervow.Close()
        readervow.Dispose()
        cmdvow.Dispose()
        conn.close()
    End Sub
End Class