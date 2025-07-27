<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUserSetup
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
        Me.lblUsers = New System.Windows.Forms.Label
        Me.lbUsers = New System.Windows.Forms.ListBox
        Me.clbUserInRoles = New System.Windows.Forms.CheckedListBox
        Me.lblUserinRoles = New System.Windows.Forms.Label
        Me.lblLanguage = New System.Windows.Forms.Label
        Me.lblUserDefaultOrganization = New System.Windows.Forms.Label
        Me.cbLanguage = New System.Windows.Forms.ComboBox
        Me.cbUserDefaultOrganization = New System.Windows.Forms.ComboBox
        Me.gbAddNewUser = New System.Windows.Forms.GroupBox
        Me.btnAdd = New System.Windows.Forms.Button
        Me.txtUserName = New System.Windows.Forms.TextBox
        Me.lblUserName = New System.Windows.Forms.Label
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblSearch = New System.Windows.Forms.Label
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.clbUserInGroups = New System.Windows.Forms.CheckedListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtFirstName = New System.Windows.Forms.TextBox
        Me.txtLastName = New System.Windows.Forms.TextBox
        Me.txtEMail = New System.Windows.Forms.TextBox
        Me.gbAddNewUser.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblUsers
        '
        Me.lblUsers.AutoSize = True
        Me.lblUsers.Location = New System.Drawing.Point(10, 35)
        Me.lblUsers.Name = "lblUsers"
        Me.lblUsers.Size = New System.Drawing.Size(34, 13)
        Me.lblUsers.TabIndex = 0
        Me.lblUsers.Text = "Users"
        '
        'lbUsers
        '
        Me.lbUsers.DisplayMember = "user_id"
        Me.lbUsers.FormattingEnabled = True
        Me.lbUsers.Location = New System.Drawing.Point(9, 51)
        Me.lbUsers.Name = "lbUsers"
        Me.lbUsers.Size = New System.Drawing.Size(142, 381)
        Me.lbUsers.TabIndex = 1
        Me.lbUsers.ValueMember = "user_id"
        '
        'clbUserInRoles
        '
        Me.clbUserInRoles.BackColor = System.Drawing.SystemColors.Window
        Me.clbUserInRoles.Location = New System.Drawing.Point(160, 125)
        Me.clbUserInRoles.Name = "clbUserInRoles"
        Me.clbUserInRoles.Size = New System.Drawing.Size(190, 304)
        Me.clbUserInRoles.TabIndex = 2
        '
        'lblUserinRoles
        '
        Me.lblUserinRoles.AutoSize = True
        Me.lblUserinRoles.Location = New System.Drawing.Point(157, 109)
        Me.lblUserinRoles.Name = "lblUserinRoles"
        Me.lblUserinRoles.Size = New System.Drawing.Size(70, 13)
        Me.lblUserinRoles.TabIndex = 3
        Me.lblUserinRoles.Text = "User in Roles"
        '
        'lblLanguage
        '
        Me.lblLanguage.AutoSize = True
        Me.lblLanguage.Location = New System.Drawing.Point(157, 51)
        Me.lblLanguage.Name = "lblLanguage"
        Me.lblLanguage.Size = New System.Drawing.Size(55, 13)
        Me.lblLanguage.TabIndex = 4
        Me.lblLanguage.Text = "Language"
        '
        'lblUserDefaultOrganization
        '
        Me.lblUserDefaultOrganization.AutoSize = True
        Me.lblUserDefaultOrganization.Location = New System.Drawing.Point(157, 81)
        Me.lblUserDefaultOrganization.Name = "lblUserDefaultOrganization"
        Me.lblUserDefaultOrganization.Size = New System.Drawing.Size(128, 13)
        Me.lblUserDefaultOrganization.TabIndex = 6
        Me.lblUserDefaultOrganization.Text = "User Default Organization"
        '
        'cbLanguage
        '
        Me.cbLanguage.FormattingEnabled = True
        Me.cbLanguage.Location = New System.Drawing.Point(290, 51)
        Me.cbLanguage.Name = "cbLanguage"
        Me.cbLanguage.Size = New System.Drawing.Size(167, 21)
        Me.cbLanguage.TabIndex = 7
        '
        'cbUserDefaultOrganization
        '
        Me.cbUserDefaultOrganization.FormattingEnabled = True
        Me.cbUserDefaultOrganization.Location = New System.Drawing.Point(290, 78)
        Me.cbUserDefaultOrganization.Name = "cbUserDefaultOrganization"
        Me.cbUserDefaultOrganization.Size = New System.Drawing.Size(167, 21)
        Me.cbUserDefaultOrganization.TabIndex = 9
        '
        'gbAddNewUser
        '
        Me.gbAddNewUser.Controls.Add(Me.btnAdd)
        Me.gbAddNewUser.Controls.Add(Me.txtUserName)
        Me.gbAddNewUser.Controls.Add(Me.lblUserName)
        Me.gbAddNewUser.Location = New System.Drawing.Point(9, 437)
        Me.gbAddNewUser.Name = "gbAddNewUser"
        Me.gbAddNewUser.Size = New System.Drawing.Size(421, 74)
        Me.gbAddNewUser.TabIndex = 14
        Me.gbAddNewUser.TabStop = False
        Me.gbAddNewUser.Text = "Add New User"
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(315, 28)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(93, 32)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(183, 20)
        Me.txtUserName.TabIndex = 1
        '
        'lblUserName
        '
        Me.lblUserName.AutoSize = True
        Me.lblUserName.Location = New System.Drawing.Point(6, 32)
        Me.lblUserName.Name = "lblUserName"
        Me.lblUserName.Size = New System.Drawing.Size(60, 13)
        Me.lblUserName.TabIndex = 0
        Me.lblUserName.Text = "User Name"
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(436, 437)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(75, 23)
        Me.btnUpdate.TabIndex = 15
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(436, 465)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 16
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Location = New System.Drawing.Point(10, 9)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(41, 13)
        Me.lblSearch.TabIndex = 17
        Me.lblSearch.Text = "Search"
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(57, 9)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(228, 20)
        Me.txtSearch.TabIndex = 18
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(290, 4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 23)
        Me.btnSearch.TabIndex = 19
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(575, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "User in Groups"
        Me.Label1.Visible = False
        '
        'clbUserInGroups
        '
        Me.clbUserInGroups.BackColor = System.Drawing.SystemColors.Window
        Me.clbUserInGroups.Location = New System.Drawing.Point(668, 16)
        Me.clbUserInGroups.Name = "clbUserInGroups"
        Me.clbUserInGroups.Size = New System.Drawing.Size(190, 304)
        Me.clbUserInGroups.TabIndex = 20
        Me.clbUserInGroups.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(360, 124)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "First Name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(360, 146)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Last Name"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(360, 170)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "E Mail"
        '
        'txtFirstName
        '
        Me.txtFirstName.Location = New System.Drawing.Point(423, 121)
        Me.txtFirstName.Name = "txtFirstName"
        Me.txtFirstName.Size = New System.Drawing.Size(132, 20)
        Me.txtFirstName.TabIndex = 25
        '
        'txtLastName
        '
        Me.txtLastName.Location = New System.Drawing.Point(423, 146)
        Me.txtLastName.Name = "txtLastName"
        Me.txtLastName.Size = New System.Drawing.Size(132, 20)
        Me.txtLastName.TabIndex = 26
        '
        'txtEMail
        '
        Me.txtEMail.Location = New System.Drawing.Point(423, 172)
        Me.txtEMail.Multiline = True
        Me.txtEMail.Name = "txtEMail"
        Me.txtEMail.Size = New System.Drawing.Size(239, 257)
        Me.txtEMail.TabIndex = 27
        '
        'frmUserSetup
        '
        Me.AcceptButton = Me.btnUpdate
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(865, 547)
        Me.Controls.Add(Me.txtEMail)
        Me.Controls.Add(Me.txtLastName)
        Me.Controls.Add(Me.txtFirstName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.clbUserInGroups)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblSearch)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.gbAddNewUser)
        Me.Controls.Add(Me.cbUserDefaultOrganization)
        Me.Controls.Add(Me.cbLanguage)
        Me.Controls.Add(Me.lblUserDefaultOrganization)
        Me.Controls.Add(Me.lblLanguage)
        Me.Controls.Add(Me.lblUserinRoles)
        Me.Controls.Add(Me.clbUserInRoles)
        Me.Controls.Add(Me.lbUsers)
        Me.Controls.Add(Me.lblUsers)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmUserSetup"
        Me.Text = "User Settings"
        Me.gbAddNewUser.ResumeLayout(False)
        Me.gbAddNewUser.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblUsers As System.Windows.Forms.Label
    Friend WithEvents lbUsers As System.Windows.Forms.ListBox


    Friend WithEvents clbUserInRoles As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblUserinRoles As System.Windows.Forms.Label
    Friend WithEvents lblLanguage As System.Windows.Forms.Label
    Friend WithEvents lblUserDefaultOrganization As System.Windows.Forms.Label
    Friend WithEvents cbLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents cbUserDefaultOrganization As System.Windows.Forms.ComboBox
    Friend WithEvents gbAddNewUser As System.Windows.Forms.GroupBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents txtUserName As System.Windows.Forms.TextBox
    Friend WithEvents lblUserName As System.Windows.Forms.Label
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents clbUserInGroups As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtFirstName As System.Windows.Forms.TextBox
    Friend WithEvents txtLastName As System.Windows.Forms.TextBox
    Friend WithEvents txtEMail As System.Windows.Forms.TextBox
End Class
