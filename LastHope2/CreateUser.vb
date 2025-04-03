Imports MySql.Data.MySqlClient

Public Class CreateUser
    Dim conn
    Dim db

    Private Sub CreateUser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        db = New Database()
        conn = db.OpenConnection()

        Dim cmd As New MySqlCommand(String.Format("SELECT partnerid, partnername, lastname FROM partner ORDER BY partnername"), conn)

        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim reader As MySqlDataReader = cmd.ExecuteReader()

        While reader.Read()
            ComboBox1.Items.Add(reader("partnerid") + "." + reader("partnername") + " " + reader("lastname"))
        End While

        reader.Close()
        reader.Dispose()
        cmd.Dispose()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username = TextBox2.Text
        Dim password = TextBox1.Text
        Dim partnername = ComboBox1.Text
        Dim names As String()
        Dim isadmin = Convert.ToInt32(CheckBox1.Checked)

        If partnername <> String.Empty Then
            names = partnername.Split(".") 'splits the id and the names

            If username <> String.Empty And password <> String.Empty Then
                'Delete If the user already exist
                Dim cmd As New MySqlCommand(String.Format("DELETE FROM login WHERE partnerid=@pid"), conn)
                cmd.Parameters.AddWithValue("@pid", names(0))
                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Using reader = cmd.ExecuteReader()

                End Using

                cmd.Dispose()

                'Create the new user
                cmd = New MySqlCommand(String.Format("INSERT INTO login VALUES(@uname,@pass,@pid,@admin)"), conn)
                cmd.Parameters.AddWithValue("@uname", username)
                cmd.Parameters.AddWithValue("@pass", password)
                cmd.Parameters.AddWithValue("@pid", names(0))
                cmd.Parameters.AddWithValue("@admin", isadmin)

                If conn.State = ConnectionState.Closed Then
                    conn.Open()
                End If

                Using reader = cmd.ExecuteReader()

                End Using

                cmd.Dispose()
                'MsgBox(username + password + partnername + names(0) + names(1) + isadmin.ToString())
                MsgBox("User Created Sucessfully!")
            Else
                MsgBox("Please enter a Username and Password")
            End If
        Else
            MsgBox("Please select an Associated User")
        End If


    End Sub
End Class