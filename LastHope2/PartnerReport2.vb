Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ListView
Imports MySql.Data.MySqlClient
Imports System.Linq
Public Class PartnerReport2
    Dim conn
    Dim db
    Dim requestforid As String
    Dim datatable_area = New DataTable()
    Dim dataview_area = New DataView()

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

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim start_day = ComboBox1.SelectedItem.ToString()
        Dim start_month = ComboBox2.SelectedItem.ToString()
        Dim end_day = ComboBox3.SelectedItem.ToString()
        Dim end_month = ComboBox4.SelectedItem.ToString()

        'If start_day = "Day" Or end_day = "Day" Or start_month = "Month" Or end_month = "Month" Then
        'MsgBox("Please Select both day and month for start and end dates")
        'End If

        ' Birth Date
        If RadioButton5.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and DAY(birthdate) >= @SDay and month(birthdate) >= @SMonth and DAY(birthdate) <= @EDay and month(birthdate) <= @EMonth;", conn)
                cmd.Parameters.AddWithValue("@SDay", start_day)
                cmd.Parameters.AddWithValue("@SMonth", start_month)
                cmd.Parameters.AddWithValue("@EDay", end_day)
                cmd.Parameters.AddWithValue("@EMonth", end_month)

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE DAY(birthdate) >= @SDay and month(birthdate) >= @SMonth and DAY(birthdate) <= @EDay and month(birthdate) <= @EMonth;", conn)
                cmd.Parameters.AddWithValue("@SDay", start_day)
                cmd.Parameters.AddWithValue("@SMonth", start_month)
                cmd.Parameters.AddWithValue("@EDay", end_day)
                cmd.Parameters.AddWithValue("@EMonth", end_month)

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton5.Text.ToString() + "_Between_" + start_day + "_" + start_month + "_and_" + end_day + "_" + end_month + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If

        ' Wedding Date
        If RadioButton2.Checked = True Then
            Dim cmd As New MySqlCommand()

            If CheckBox4.Checked = True Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE active=1 and DAY(weddingdate) >= @SDay and month(weddingdate) >= @SMonth and DAY(weddingdate) <= @EDay and month(weddingdate) <= @EMonth;", conn)
                cmd.Parameters.AddWithValue("@SDay", start_day)
                cmd.Parameters.AddWithValue("@SMonth", start_month)
                cmd.Parameters.AddWithValue("@EDay", end_day)
                cmd.Parameters.AddWithValue("@EMonth", end_month)

            ElseIf CheckBox4.Checked = False Then
                cmd = New MySqlCommand("SELECT * FROM view_partner_area WHERE DAY(weddingdate) >= @SDay and month(weddingdate) >= @SMonth and DAY(weddingdate) <= @EDay and month(weddingdate) <= @EMonth;", conn)
                cmd.Parameters.AddWithValue("@SDay", start_day)
                cmd.Parameters.AddWithValue("@SMonth", start_month)
                cmd.Parameters.AddWithValue("@EDay", end_day)
                cmd.Parameters.AddWithValue("@EMonth", end_month)

            End If

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            Dim irregular As New DataTable
            irregular.Load(reader)
            Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton2.Text.ToString() + "_Between_" + start_day + "_" + start_month + "_and_" + end_day + "_" + end_month + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
                WriteDataTable(irregular, writer, True)
            End Using

            reader.Close()
            reader.Dispose()
            cmd.Dispose()
            conn.Close()

            MsgBox("Report Exported!")
        End If

    End Sub

    Private Sub PartnerReport2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()

        'ComboBox1.Items.Add("Day")
        'ComboBox3.Items.Add("Day")
        'ComboBox2.Items.Add("Month")
        'ComboBox4.Items.Add("Month")

        For i As Integer = 1 To 31
            ComboBox1.Items.Add(i)
            ComboBox3.Items.Add(i)
        Next

        For i As Integer = 1 To 12
            ComboBox2.Items.Add(i)
            ComboBox4.Items.Add(i)
        Next

        ' Area Report
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim cmd As New MySqlCommand()

        cmd = New MySqlCommand("SELECT DISTINCT AreaName FROM area;", conn)
        Dim reader As MySqlDataReader = cmd.ExecuteReader()
        ComboBox5.Items.Add("All")
        While reader.Read()
            ComboBox5.Items.Add(reader("AreaName"))
        End While
        reader.Close()

        cmd = New MySqlCommand("SELECT DISTINCT Region FROM area;", conn)
        reader = cmd.ExecuteReader()
        ComboBox6.Items.Add("All")
        While reader.Read()
            ComboBox6.Items.Add(reader("Region"))
        End While
        reader.Close()

        cmd = New MySqlCommand("SELECT DISTINCT City FROM area;", conn)
        reader = cmd.ExecuteReader()
        ComboBox7.Items.Add("All")
        While reader.Read()
            ComboBox7.Items.Add(reader("City"))
        End While
        reader.Close()
        conn.close()

        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
        ComboBox4.SelectedIndex = 0
        ComboBox5.SelectedIndex = 0
        ComboBox6.SelectedIndex = 0
        ComboBox7.SelectedIndex = 0

        tbPRPartnerName.Enabled = False
        Button2.Enabled = False

        If (Not System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory + "\Reports")) Then
            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory + "\Reports")
        End If

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs)
        tbPRPartnerName.Enabled = False
        Button2.Enabled = False

        PublicModule.SPPartnerID = ""
        PublicModule.SPPartnerName = ""
        tbPRPartnerName.Text = ""
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        tbPRPartnerName.Enabled = True
        Button2.Enabled = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SearchPartners.ShowDialog()
        If PublicModule.SPPartnerID <> String.Empty Then
            requestforid = PublicModule.SPPartnerID
            tbPRPartnerName.Text = PublicModule.SPPartnerName
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim save_path = ""
        Dim cmd As New MySqlCommand()


        If RadioButton1.Checked = True Then
            cmd = New MySqlCommand("SELECT * FROM prayer_request_view WHERE partnerid = @partnerid;", conn)
            cmd.Parameters.AddWithValue("@partnerid", PublicModule.SPPartnerID)

            save_path = System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_Prayer_Request_" + RadioButton1.Text.ToString() + "_" + tbPRPartnerName.Text + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv"

        End If


        ' Latest Prayer Date
        If RadioButton4.Checked = True Then
            cmd = New MySqlCommand("SELECT * FROM prayer_request_view WHERE latest_prayer_date between DATE(@StartDate) and DATE(@EndDate);", conn)
            cmd.Parameters.AddWithValue("@StartDate", DateTimePicker1.Value.Date)
            cmd.Parameters.AddWithValue("@EndDate", DateTimePicker2.Value.Date)

            save_path = System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton4.Text.ToString() + "_Between_" + DateTimePicker1.Value.Date + "_and_" + DateTimePicker2.Value.Date + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv"

        End If


        ' Payer Request Date
        If RadioButton3.Checked = True Then
            cmd = New MySqlCommand("SELECT * FROM prayer_request_view WHERE requestdate between DATE(@StartDate) and DATE(@EndDate);", conn)
            cmd.Parameters.AddWithValue("@StartDate", DateTimePicker4.Value.Date)
            cmd.Parameters.AddWithValue("@EndDate", DateTimePicker3.Value.Date)

            save_path = System.IO.Directory.GetCurrentDirectory + "\Reports\Partners_With_" + RadioButton3.Text.ToString() + "_Between_" + DateTimePicker4.Value.Date + "_and_" + DateTimePicker3.Value.Date + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv"

        End If

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        Dim irregular As New DataTable
        irregular.Load(reader)
        Using writer As StreamWriter = New StreamWriter(save_path)
            WriteDataTable(irregular, writer, True)
        End Using

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()

        MsgBox("Report Exported!")
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged

    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged

    End Sub

    Private Sub GroupBox2_Enter(sender As Object, e As EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim column_name = ""
        Dim column_value = ""
        Dim cmd_str_all = ""
        Dim cmd_str = ""

        If RadioButton6.Checked Then
            column_name = "AreaName"
            column_value = ComboBox5.Text.ToString
            cmd_str_all = "SELECT AreaName, COUNT(AreaName) FROM lasthope.view_partner_area GROUP BY 1;"
            cmd_str = "SELECT AreaName, COUNT(AreaName) FROM lasthope.view_partner_area WHERE AreaName ='" + column_value + "' GROUP BY 1;"
        End If

        If RadioButton7.Checked Then
            column_name = "Region"
            column_value = ComboBox6.Text.ToString
            cmd_str_all = "SELECT Region, COUNT(Region) FROM lasthope.view_partner_area GROUP BY 1;"
            cmd_str = "SELECT Region, COUNT(Region) FROM lasthope.view_partner_area WHERE Region ='" + column_value + "' GROUP BY 1;"
        End If

        If RadioButton8.Checked Then
            column_name = "City"
            column_value = ComboBox7.Text.ToString
            cmd_str_all = "SELECT City, COUNT(City) FROM lasthope.view_partner_area GROUP BY 1;"
            cmd_str = "SELECT City, COUNT(City) FROM lasthope.view_partner_area WHERE City ='" + column_value + "' GROUP BY 1;"
        End If

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim cmd As New MySqlCommand()

        If column_value = "All" Then
            cmd = New MySqlCommand(cmd_str_all, conn)
        Else
            cmd = New MySqlCommand(cmd_str, conn)
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()
        Dim report As New DataTable

        report.Load(reader)
        Using writer As StreamWriter = New StreamWriter(System.IO.Directory.GetCurrentDirectory + "\Reports\" + "Number_of_members_coming_from_" + column_value + "_" + column_name + "_" + Date.Today.Date.ToString("dd_MM_yyyy") + ".csv")
            WriteDataTable(report, writer, True)
        End Using

        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()

        MsgBox("Report Exported!")

        reader.Close()
    End Sub
End Class