<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LeaderMaster
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbleader = New System.Windows.Forms.ComboBox()
        Me.dgvSubleader = New System.Windows.Forms.DataGridView()
        Me.gbSubleader = New System.Windows.Forms.GroupBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gbMainleader = New System.Windows.Forms.GroupBox()
        Me.dgvMainleader = New System.Windows.Forms.DataGridView()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        CType(Me.dgvSubleader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSubleader.SuspendLayout()
        Me.gbMainleader.SuspendLayout()
        CType(Me.dgvMainleader, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 25)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select a Leader"
        '
        'cmbleader
        '
        Me.cmbleader.FormattingEnabled = True
        Me.cmbleader.Location = New System.Drawing.Point(156, 23)
        Me.cmbleader.Margin = New System.Windows.Forms.Padding(4)
        Me.cmbleader.Name = "cmbleader"
        Me.cmbleader.Size = New System.Drawing.Size(299, 24)
        Me.cmbleader.TabIndex = 1
        '
        'dgvSubleader
        '
        Me.dgvSubleader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSubleader.Location = New System.Drawing.Point(8, 55)
        Me.dgvSubleader.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvSubleader.Name = "dgvSubleader"
        Me.dgvSubleader.Size = New System.Drawing.Size(937, 508)
        Me.dgvSubleader.TabIndex = 2
        '
        'gbSubleader
        '
        Me.gbSubleader.Controls.Add(Me.Button5)
        Me.gbSubleader.Controls.Add(Me.TextBox1)
        Me.gbSubleader.Controls.Add(Me.Label2)
        Me.gbSubleader.Controls.Add(Me.dgvSubleader)
        Me.gbSubleader.Location = New System.Drawing.Point(493, 164)
        Me.gbSubleader.Margin = New System.Windows.Forms.Padding(4)
        Me.gbSubleader.Name = "gbSubleader"
        Me.gbSubleader.Padding = New System.Windows.Forms.Padding(4)
        Me.gbSubleader.Size = New System.Drawing.Size(953, 564)
        Me.gbSubleader.TabIndex = 3
        Me.gbSubleader.TabStop = False
        Me.gbSubleader.Text = "Sub Leader Info"
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(848, 21)
        Me.Button5.Margin = New System.Windows.Forms.Padding(4)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(88, 28)
        Me.Button5.TabIndex = 7
        Me.Button5.Text = "Clear"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(587, 23)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(252, 22)
        Me.TextBox1.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(508, 23)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Search"
        '
        'gbMainleader
        '
        Me.gbMainleader.Controls.Add(Me.dgvMainleader)
        Me.gbMainleader.Location = New System.Drawing.Point(493, 15)
        Me.gbMainleader.Margin = New System.Windows.Forms.Padding(4)
        Me.gbMainleader.Name = "gbMainleader"
        Me.gbMainleader.Padding = New System.Windows.Forms.Padding(4)
        Me.gbMainleader.Size = New System.Drawing.Size(953, 142)
        Me.gbMainleader.TabIndex = 4
        Me.gbMainleader.TabStop = False
        Me.gbMainleader.Text = "Main Leader Info"
        '
        'dgvMainleader
        '
        Me.dgvMainleader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMainleader.Location = New System.Drawing.Point(8, 23)
        Me.dgvMainleader.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvMainleader.Name = "dgvMainleader"
        Me.dgvMainleader.Size = New System.Drawing.Size(937, 105)
        Me.dgvMainleader.TabIndex = 4
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 16
        Me.ListBox1.Location = New System.Drawing.Point(8, 44)
        Me.ListBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(447, 244)
        Me.ListBox1.TabIndex = 5
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(8, 297)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(220, 28)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Add New Sub Leader"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(236, 297)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(220, 28)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "Remove Sub Leader"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(8, 332)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(220, 28)
        Me.Button3.TabIndex = 8
        Me.Button3.Text = "Swap Leader"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(8, 60)
        Me.Button4.Margin = New System.Windows.Forms.Padding(4)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(220, 28)
        Me.Button4.TabIndex = 9
        Me.Button4.Text = "Add New Leader"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 25)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(306, 17)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "List of Sub Leaders Under the Selected Leader"
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(236, 60)
        Me.Button6.Margin = New System.Windows.Forms.Padding(4)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(220, 28)
        Me.Button6.TabIndex = 11
        Me.Button6.Text = "Remove Leader"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbleader)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Controls.Add(Me.Button6)
        Me.GroupBox1.Location = New System.Drawing.Point(21, 47)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(464, 96)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Main Leader Controls"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button3)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.ListBox1)
        Me.GroupBox2.Controls.Add(Me.Button2)
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Location = New System.Drawing.Point(21, 150)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Size = New System.Drawing.Size(464, 375)
        Me.GroupBox2.TabIndex = 15
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Sub Leader Controls"
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(21, 15)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(153, 23)
        Me.Button7.TabIndex = 16
        Me.Button7.Text = "Search Leaders"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(21, 542)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(464, 29)
        Me.Button8.TabIndex = 17
        Me.Button8.Text = "Export Leader Master"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'LeaderMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1445, 727)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbMainleader)
        Me.Controls.Add(Me.gbSubleader)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "LeaderMaster"
        Me.Text = "LeaderMaster"
        CType(Me.dgvSubleader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSubleader.ResumeLayout(False)
        Me.gbSubleader.PerformLayout()
        Me.gbMainleader.ResumeLayout(False)
        CType(Me.dgvMainleader, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents cmbleader As ComboBox
    Friend WithEvents dgvSubleader As DataGridView
    Friend WithEvents gbSubleader As GroupBox
    Friend WithEvents gbMainleader As GroupBox
    Friend WithEvents dgvMainleader As DataGridView
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Button7 As Button
    Friend WithEvents Button8 As Button
End Class
