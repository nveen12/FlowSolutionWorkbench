Imports System.Deployment.Application

Public Class clsApplication

    Public Sub New()
        Dim deploy As ApplicationDeployment = ApplicationDeployment.CurrentDeployment


        Dim update As UpdateCheckInfo = deploy.CheckForDetailedUpdate()
        If deploy.CheckForUpdate() Then
           
            MessageBox.Show("You can update to version: " + update.AvailableVersion.ToString(), "Update", MessageBoxButtons.OK)
            deploy.Update()
            Application.Restart()

        Else
            MessageBox.Show("You are running the newest version.")
        End If

    End Sub
End Class
