Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class MainForm
    Dim conn
    Dim db

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'db = New Database()
        'conn = db.OpenConnection()
        PleaseWait.Show()
        PleaseWait.Hide()
    End Sub
    Private Sub MainForm_Close(sender As Object, e As EventArgs) Handles MyBase.Closing
        PleaseWait.Close()
        Login.Close()
    End Sub

    Private Sub MainForm_Closed(sender As Object, e As EventArgs) Handles MyBase.Closed
        Login.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles BtnCreateUser.Click
        CreateUser.Show()
    End Sub

    Private Sub BtnAreaMaster_Click(sender As Object, e As EventArgs) Handles BtnAreaMaster.Click
        Panel2.Controls.Clear()
        Dim f As New AreaMaster With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub BtnRelationMaster_Click(sender As Object, e As EventArgs) Handles BtnRelationMaster.Click
        Panel2.Controls.Clear()
        Dim f As New RelationMaster With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Panel2.Controls.Clear()
        Dim f As New PrayerCategoryMaster With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Panel2.Controls.Clear()
        Dim f As New PartnerDetails With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Panel2.Controls.Clear()
        Dim f As New LeaderMaster With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Panel2.Controls.Clear()
        Dim f As New PrayerRequests With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Panel2.Controls.Clear()
        Dim f As New PartnerReport With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Login.Show()
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Panel2.Controls.Clear()
        Dim f As New Progress With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)

        BackgroundWorker1.RunWorkerAsync()


    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        If (Not System.IO.Directory.Exists(System.IO.Directory.GetCurrentDirectory + "\Backup")) Then
            System.IO.Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory + "\Backup")
        End If

        Dim file = System.IO.Directory.GetCurrentDirectory + "\Backup\LastHopeDB.sql"

        db = New Database()

        Using conn = db.OpenConnection()
            Using cmd As New MySqlCommand()

                Using mb As New MySqlBackup(cmd)
                    cmd.Connection = conn
                    conn.Open()
                    mb.ExportToFile(file)
                    conn.Close()
                End Using
            End Using

        End Using
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Panel2.Controls.Clear()
        Dim f As New ProgressComplete With {
           .TopLevel = False,
           .WindowState = FormWindowState.Maximized,
           .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
           .Visible = True,
           .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
       }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Panel2.Controls.Clear()
        Dim f As New PartnerReport2 With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel2.Controls.Clear()
        Dim f As New Vow With {
            .TopLevel = False,
            .WindowState = FormWindowState.Maximized,
            .FormBorderStyle = Windows.Forms.FormBorderStyle.SizableToolWindow,
            .Visible = True,
            .Size = New System.Drawing.Size(Panel2.Size.Width, Panel2.Size.Height)
        }
        Panel2.Controls.Add(f)
    End Sub
End Class
