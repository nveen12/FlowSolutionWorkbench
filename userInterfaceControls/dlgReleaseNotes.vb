Public NotInheritable Class dlgReleaseNotes
    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)

    Private Sub dlgReleaseNotes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            loadVersions()
            If cbVersion.Items.Count > 0 Then
                cbVersion.SelectedIndex = 0
            End If
            loadReleaseNotes()
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("dlgReleaseNotes_Load", ex, 1)
        End Try
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

    Private Sub cbVersion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbVersion.SelectedIndexChanged
        loadReleaseNotes()
    End Sub

    Private Sub loadVersions()
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT [version], [releaseId] from whReleaseNotes ORDER BY version DESC")

            cbVersion.DataSource = ds.Tables(0)
            cbVersion.DisplayMember = "version"
            cbVersion.ValueMember = "releaseId"

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("loadVersions", ex, 1)
        End Try
    End Sub

    Private Sub loadReleaseNotes()
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT [version], CONVERT(nvarchar(50),[releaseDate], 101) [releaseDate], [releaseNotes] from whReleaseNotes WHERE releaseId = @1", cbVersion.SelectedValue.ToString())

            If ds.Tables(0).Rows.Count > 0 Then
                lblRelease.Text = ds.Tables(0).Rows(0).Item("releaseDate").ToString
                TextBoxDescription.Text = ds.Tables(0).Rows(0).Item("releaseNotes").ToString
            End If

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("dlgReleaseNotes_Load", ex, 1)
        End Try
    End Sub
End Class
