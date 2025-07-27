<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModeratingChat
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.btnDeleteComment = New System.Windows.Forms.Button
        Me.gbBanUsers = New System.Windows.Forms.GroupBox
        Me.btnBanUser = New System.Windows.Forms.Button
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.btnReleaseUser = New System.Windows.Forms.Button
        Me.cbx = New System.Windows.Forms.ComboBox
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbBanUsers.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(12, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(1369, 419)
        Me.DataGridView1.TabIndex = 0
        '
        'btnDeleteComment
        '
        Me.btnDeleteComment.Location = New System.Drawing.Point(12, 437)
        Me.btnDeleteComment.Name = "btnDeleteComment"
        Me.btnDeleteComment.Size = New System.Drawing.Size(116, 23)
        Me.btnDeleteComment.TabIndex = 1
        Me.btnDeleteComment.Text = "Hide Comment"
        Me.btnDeleteComment.UseVisualStyleBackColor = True
        '
        'gbBanUsers
        '
        Me.gbBanUsers.Controls.Add(Me.btnBanUser)
        Me.gbBanUsers.Controls.Add(Me.ComboBox2)
        Me.gbBanUsers.Controls.Add(Me.btnReleaseUser)
        Me.gbBanUsers.Controls.Add(Me.cbx)
        Me.gbBanUsers.Location = New System.Drawing.Point(12, 466)
        Me.gbBanUsers.Name = "gbBanUsers"
        Me.gbBanUsers.Size = New System.Drawing.Size(416, 98)
        Me.gbBanUsers.TabIndex = 2
        Me.gbBanUsers.TabStop = False
        Me.gbBanUsers.Text = "Ban Users"
        '
        'btnBanUser
        '
        Me.btnBanUser.Location = New System.Drawing.Point(272, 53)
        Me.btnBanUser.Name = "btnBanUser"
        Me.btnBanUser.Size = New System.Drawing.Size(120, 23)
        Me.btnBanUser.TabIndex = 3
        Me.btnBanUser.Text = "Ban User"
        Me.btnBanUser.UseVisualStyleBackColor = True
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(7, 56)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(237, 21)
        Me.ComboBox2.TabIndex = 2
        '
        'btnReleaseUser
        '
        Me.btnReleaseUser.Location = New System.Drawing.Point(272, 17)
        Me.btnReleaseUser.Name = "btnReleaseUser"
        Me.btnReleaseUser.Size = New System.Drawing.Size(120, 23)
        Me.btnReleaseUser.TabIndex = 1
        Me.btnReleaseUser.Text = "Release User"
        Me.btnReleaseUser.UseVisualStyleBackColor = True
        '
        'cbx
        '
        Me.cbx.FormattingEnabled = True
        Me.cbx.Location = New System.Drawing.Point(7, 20)
        Me.cbx.Name = "cbx"
        Me.cbx.Size = New System.Drawing.Size(237, 21)
        Me.cbx.TabIndex = 0
        '
        'frmModeratingChat
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1393, 752)
        Me.Controls.Add(Me.gbBanUsers)
        Me.Controls.Add(Me.btnDeleteComment)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "frmModeratingChat"
        Me.Text = "Moderating Chat"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbBanUsers.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnDeleteComment As System.Windows.Forms.Button
    Friend WithEvents gbBanUsers As System.Windows.Forms.GroupBox
    Friend WithEvents btnBanUser As System.Windows.Forms.Button
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents btnReleaseUser As System.Windows.Forms.Button
    Friend WithEvents cbx As System.Windows.Forms.ComboBox
End Class
