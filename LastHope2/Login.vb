Imports MySql.Data.MySqlClient

Public Class Login
    Public Shared isAdmin As Boolean
    Public Shared PartnerID As Integer
    Public Shared PartnerName As String
    Dim conn
    Dim db

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tbUsername.Text = ""
        tbPassword.Text = ""
        db = New Database()
        conn = db.OpenConnection()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username = tbUsername.Text
        Dim password = tbPassword.Text

        Dim cmd As New MySqlCommand(String.Format("SELECT * FROM login WHERE username='{0}' And password='{1}'", arg0:=username, arg1:=password), conn)
        'MsgBox(cmd.CommandText)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If
        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            'MsgBox(String.Format("{0}, {1}", reader(0), reader(1)))
            isAdmin = reader("isadmin")
            PartnerID = reader("partnerid")
            Exit While
        End While
        reader.Close()
        reader.Dispose()
        cmd.Dispose()
        conn.Close()

        If PartnerID <> 0 Then
            MainForm.Show()
            Me.Hide()
        Else
            MsgBox("Incorrect Username or Password")
        End If

    End Sub

    Private Sub btnLogin_Enter(sender As Object, e As EventArgs) Handles btnLogin.Enter
        Me.btnLogin_Click(sender, e)

    End Sub

End Class
