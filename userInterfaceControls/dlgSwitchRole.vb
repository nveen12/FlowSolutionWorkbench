Imports System.Windows.Forms

Public Class dlgSwitchRole

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        'Save the current selection to the backend.
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        db.SecureNonQueryParams("UPDATE warehouse_users SET [lastRoleSelected] = @1 WHERE [user_Name] = @2", cboCurrentRole.SelectedValue, Environment.UserName)

        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgSwitchRole_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT TOP 1000 [user_Name] " & _
      ",[standard_organization_id]  " & _
      ",[userDefaultOrganization]  " & _
      ",[roleName]  " & _
      ",[roleDescription]  " & _
      ",[rolePurpose]  " & _
      ",[roleID] " & _
      "  FROM [userInRolesView] WHERE user_name = @1", Environment.UserName)

        cboCurrentRole.DataSource = ds.Tables(0)
        cboCurrentRole.DisplayMember = "roleName"
        cboCurrentRole.ValueMember = "roleID"

    End Sub

    Private Sub cboCurrentRole_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCurrentRole.SelectedIndexChanged

    End Sub

End Class
