﻿Imports MySql.Data.MySqlClient

Public Class SearchPartners
    Dim conn
    Dim db
    Dim lastrecordID
    Dim dtAll = New DataTable()
    Dim dtSubset = New DataTable()
    Dim SelectedPartnerID As String
    Dim SelectedPartnerName As String


    Private Sub SearchPartners_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PublicModule.SPPartnerID = ""
        PublicModule.SPPartnerName = ""
        SelectedPartnerID = ""
        SelectedPartnerName = ""
        db = New Database()
        conn = db.OpenConnection()

        Dim cmd As New MySqlCommand("SELECT partner.partnerid, partner.partnername, partner.middlename,partner.lastname, area.city,partner.birthdate, partner.baptismdate,area.areaname ,partner.regular, partner.active,partner.address ,partner.mobileno, partner.phoneno  FROM partner left join area on partner.areaid=area.AreaId", conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()


        dtAll.Load(reader)
        dtSubset = dtAll

        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = dtAll
        DataGridView1.Refresh()
        SPPartnerID = String.Empty
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim seachfilter = TextBox1.Text
        Dim dv As New DataView(dtAll)
        dv.RowFilter = "partnername Like '%" + seachfilter + "%' OR mobileno Like '%" + seachfilter + "%' OR phoneno Like '%" + seachfilter + "%' OR areaname Like '%" + seachfilter + "%' OR middlename Like '%" + seachfilter + "%' OR lastname Like '%" + seachfilter + "%' OR address Like '%" + seachfilter + "%' OR areaname Like '%" + seachfilter + "%'"
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If SelectedPartnerID <> String.Empty Then
            PublicModule.SPPartnerID = SelectedPartnerID
            PublicModule.SPPartnerName = SelectedPartnerName
            Me.Close()
        Else
            MsgBox("Please select a Partner to see its details")
        End If
    End Sub


    Private Sub DataGridView1_Click(sender As Object, e As EventArgs) Handles DataGridView1.Click
        SelectedPartnerID = DataGridView1.CurrentRow.Cells(0).Value.ToString()
        SelectedPartnerName = DataGridView1.CurrentRow.Cells(1).Value.ToString() + " " + DataGridView1.CurrentRow.Cells(2).Value.ToString() + " " + DataGridView1.CurrentRow.Cells(3).Value.ToString()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        dtSubset = dtAll
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = dtAll
        DataGridView1.Refresh()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        'Dim seachfilter = TextBox3.Text
        Dim dv As New DataView(dtSubset)
        dv.RowFilter = search_filters()
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()


    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        'Dim seachfilter = TextBox2.Text
        Dim dv As New DataView(dtSubset)
        dv.RowFilter = search_filters()
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()

    End Sub

    Function search_filters()
        Dim search_string = ""
        If TextBox3.Text <> String.Empty Then
            search_string = search_string + "partnername Like '%" + TextBox3.Text + "%' AND "
        End If

        If TextBox2.Text <> String.Empty Then
            search_string = search_string + "lastname Like '%" + TextBox2.Text + "%' AND "
        End If

        If TextBox4.Text <> String.Empty Then
            search_string = search_string + "phoneno Like '%" + TextBox4.Text + "%' AND "
        End If

        If TextBox7.Text <> String.Empty Then
            search_string = search_string + "address Like '%" + TextBox7.Text + "%' AND "
        End If

        If TextBox6.Text <> String.Empty Then
            search_string = search_string + "areaname Like '%" + TextBox6.Text + "%' AND "
        End If

        If TextBox5.Text <> String.Empty Then
            search_string = search_string + "city Like '%" + TextBox5.Text + "%' AND "
        End If

        If search_string.Length > 0 Then
            search_string = search_string.Substring(0, search_string.Length - 5)
        End If
        Console.WriteLine(search_string)

        Return search_string
    End Function

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        Dim dv As New DataView(dtSubset)
        dv.RowFilter = search_filters()
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        Dim dv As New DataView(dtSubset)
        dv.RowFilter = search_filters()
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        Dim dv As New DataView(dtSubset)
        dv.RowFilter = search_filters()
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        Dim dv As New DataView(dtSubset)
        dv.RowFilter = search_filters()
        DataGridView1.DataSource = dv
        DataGridView1.Refresh()
    End Sub

End Class