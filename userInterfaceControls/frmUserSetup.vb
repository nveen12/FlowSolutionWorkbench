Public Class frmUserSetup
    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)

    Private Sub frmUserSetup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Display all the users
            loadUsers()

            'Display languages
            Dim alVDP As New ArrayList
            alVDP.Add(New ValueDescriptionPair("Standard", "Standard"))
            alVDP.Add(New ValueDescriptionPair("de", "German"))
            cbLanguage.DataSource = alVDP
            cbLanguage.DisplayMember = "Description"
            cbLanguage.ValueMember = "Value"

            'Display organizations
            Dim dsOrganizations As DataSet = db.SecureQueryParams("SELECT * FROM whOrganizationDefinition ORDER BY organizationName")
            cbUserDefaultOrganization.DataSource = dsOrganizations.Tables(0)
            cbUserDefaultOrganization.DisplayMember = "organizationName"
            cbUserDefaultOrganization.ValueMember = "organizationId"

            'Display User roles
            Dim dsUserInRoles As DataSet = db.SecureQueryParams("SELECT * FROM whUserRoles ORDER BY roleName")
            clbUserInRoles.DataSource = dsUserInRoles.Tables(0)
            clbUserInRoles.DisplayMember = "roleName"
            clbUserInRoles.ValueMember = "roleId"

            'Display the user groups
            'Dim dsUserGroups As DataSet = db.SecureQueryParams("SELECT * FROM warehouseMessagingGroups")
            'clbUserInGroups.DataSource = dsUserGroups.Tables(0)
            'clbUserInGroups.DisplayMember = "warehouseMessagingGroupName"
            'clbUserInGroups.ValueMember = "warehouseMessagingGroupID"

            loadUserValues()
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("frmUserSetup_Load", ex, 1)
        End Try
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            'Update language and user default organization
            db.SecureNonQueryParams("UPDATE warehouse_users SET [language] = @1, [userDefaultOrganization] = @2, firstName = @4, lastName = @5, email = @6 WHERE user_Name = @3", cbLanguage.SelectedValue, cbUserDefaultOrganization.SelectedValue, lbUsers.SelectedValue, _
                                    txtFirstName.Text, _
                                    txtLastName.Text, _
                                    txtEMail.Text)

            Dim userId As Int64
            userId = CType(db.SecureQueryParams("SELECT TOP 1 [user_id] FROM warehouse_users WHERE user_Name = @1", lbUsers.SelectedValue).Tables(0).Rows(0).Item(0).ToString(), Integer)

            'Clear the existing roles
            db.SecureNonQueryParams("DELETE FROM whUserInRoles WHERE userId = @1", userId)

            Dim ds As DataSet = db.SecureQueryParams("SELECT TOP 1 [relationId] FROM whUserInRoles ORDER BY relationId DESC")
            Dim relationId As Int64

            If ds.Tables(0).Rows.Count > 0 Then
                relationId = CType(ds.Tables(0).Rows(0).Item(0).ToString(), Integer)
            End If

            'Insert the newly selected roles
            For i As Integer = 0 To clbUserInRoles.Items.Count - 1
                Dim drv As DataRowView = clbUserInRoles.Items(i)
                If clbUserInRoles.GetItemChecked(i) Then
                    db.SecureNonQueryParams("INSERT INTO [whUserInRoles] ([relationId],[userId],[roleId],[creationDate],[createdBy]) VALUES (@1,@2,@3,GETDATE(),@4)", (relationId + i + 1), userId, drv("roleId").ToString(), Environment.UserName)
                End If
            Next

            ''Clear the existing groups
            'db.SecureNonQueryParams("DELETE FROM warehouseMessagingUserInGroupsID WHERE userId = @1", userId)

            ''Insert the newly selected groups
            'For i As Integer = 0 To clbUserInGroups.Items.Count - 1
            '    Dim drv As DataRowView = clbUserInGroups.Items(i)
            '    If clbUserInGroups.GetItemChecked(i) Then
            '        db.SecureNonQueryParams("INSERT INTO [warehouseMessagingUserInGroupsID] (groupID, userID, userName) VALUES (@1,@2,@3)", drv("warehouseMessagingGroupID"), userId, lbUsers.SelectedValue)
            '    End If
            'Next

            Me.Cursor = Cursors.Default

            MessageBox.Show("User details updated successfully.")

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("btnUpdate_Click", ex, 1)
        End Try
    End Sub

    Private Sub loadUserValues()
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT * FROM warehouse_users WHERE user_Name = @1", lbUsers.SelectedValue.ToString())
            'Populate user language
            cbLanguage.SelectedValue = ds.Tables(0).Rows(0).Item("language").ToString()
            'Populate user default organization
            cbUserDefaultOrganization.SelectedValue = ds.Tables(0).Rows(0).Item("userDefaultOrganization").ToString()

            Dim dsUserInRoles As DataSet = db.SecureQueryParams("SELECT * FROM whUserInRoles WHERE userId = @1", ds.Tables(0).Rows(0).Item("user_id").ToString())
            'Clear CheckedListBox selection
            uncheckAllUserInRoles()

            Try
                'Populate current user roles
                For Each dr As DataRow In dsUserInRoles.Tables(0).Rows
                    For i As Integer = 0 To clbUserInRoles.Items.Count - 1
                        Dim drv As DataRowView = clbUserInRoles.Items(i)
                        If dr("roleId").ToString() = drv("roleId").ToString() Then
                            clbUserInRoles.SetItemChecked(i, True)
                            Exit For
                        End If
                    Next
                Next

                'Load the user groups
                uncheckAllUsersInGroups()
                Dim dsUserInGroups As DataSet = db.SecureQueryParams("SELECT * FROM whUserInGroupsView WHERE user_id = @1", ds.Tables(0).Rows(0).Item("user_id").ToString())

                'Populate current user roles
                For Each dr As DataRow In dsUserInGroups.Tables(0).Rows
                    For i As Integer = 0 To clbUserInGroups.Items.Count - 1
                        Dim drv As DataRowView = clbUserInGroups.Items(i)
                        If dr("warehouseMessagingGroupID").ToString() = drv("warehouseMessagingGroupID").ToString() Then
                            clbUserInGroups.SetItemChecked(i, True)
                            Exit For
                        End If
                    Next
                Next
            Catch ex As Exception

            End Try
            

            'Show first name and last name
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("firstName")) Then
                txtFirstName.Text = ds.Tables(0).Rows(0).Item("firstName").ToString
            Else
                txtFirstName.Text = String.Empty
            End If

            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("lastName")) Then
                txtLastName.Text = ds.Tables(0).Rows(0).Item("lastName").ToString
            Else
                txtLastName.Text = String.Empty
            End If

            'E Mail
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("eMail")) Then
                txtEMail.Text = ds.Tables(0).Rows(0).Item("eMail").ToString
            Else
                txtEMail.Text = String.Empty
            End If

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("loadUserValues", ex, 1)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            'Adding new user
            Dim newUser As New clsSetupOperations(txtUserName.Text)
            loadUsers()
            Me.Cursor = Cursors.Default
            MessageBox.Show("New user added successfully.")
            txtUserName.Text = String.Empty
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("btnAdd_Click", ex, 1)
        End Try
    End Sub

    Private Sub lbUsers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbUsers.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor
            loadUserValues()
            Me.Cursor = Cursors.Default
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("lbUsers_SelectedIndexChanged", ex, 1)
        End Try
    End Sub

    Private Sub uncheckAllUsersInGroups()
        Try
            For i As Integer = 0 To clbUserInGroups.Items.Count - 1
                clbUserInGroups.SetItemChecked(i, False)
            Next
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("uncheckAllUsersInGroups", ex, 1)
        End Try
    End Sub

    Private Sub uncheckAllUserInRoles()
        Try
            For i As Integer = 0 To clbUserInRoles.Items.Count - 1
                clbUserInRoles.SetItemChecked(i, False)
            Next
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("uncheckAllUserInRoles", ex, 1)
        End Try
    End Sub

    Private Sub loadUsers()
        'Load all the users
        Dim dsUsers As DataSet = db.SecureQueryParams("SELECT CASE [firstName] WHEN NULL THEN user_name WHEN '' THEN user_name ELSE [firstName] + ' ' + [lastName] END [displayName], [user_Name] [value] " & _
                                                          "FROM warehouse_users ORDER BY [displayName]")
        lbUsers.DataSource = dsUsers.Tables(0)
        lbUsers.DisplayMember = "displayName"
        lbUsers.ValueMember = "value"
        lbUsers.SelectedIndex = 0
    End Sub

    Private Sub btnSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnSearch.Click
        'Load all the users
        Dim dsUsers As DataSet = db.SecureQueryParams("SELECT CASE [firstName] WHEN NULL THEN user_name WHEN '' THEN user_name ELSE [firstName] + ' ' + [lastName] END [displayName], [user_Name] [value] " & _
                                                          "FROM warehouse_users WHERE (firstName LIKE '%" & txtSearch.Text & "%' OR lastName LIKE '%" & txtSearch.Text & "%' OR user_Name LIKE '%" & txtSearch.Text & "%') AND (enabled is null or enabled = 1)  ORDER BY [displayName]")
        lbUsers.DataSource = dsUsers.Tables(0)
        lbUsers.DisplayMember = "displayName"
        lbUsers.ValueMember = "value"
        lbUsers.SelectedIndex = 0
    End Sub

End Class
Public Class ValueDescriptionPair

    Private m_Value As Object
    Private m_Description As String

    Public ReadOnly Property Value() As Object
        Get
            Return m_Value
        End Get
    End Property

    Public ReadOnly Property Description() As String
        Get
            Return m_Description
        End Get
    End Property

    Public Sub New(ByVal NewValue As Object, ByVal NewDescription As String)
        m_Value = NewValue
        m_Description = NewDescription
    End Sub

    Public Overrides Function ToString() As String
        Return m_Description
    End Function

End Class

