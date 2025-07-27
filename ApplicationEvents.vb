Imports System.Deployment.Application
Imports System.Reflection


Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

            'Look at the command line and look if there is a parameter coming through
            'Distinct between starting a window and running the transformation code
            'Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            'db.SecureNonQueryParams("INSERT INTO tblWork (text1) VALUES (@1)", Command.Length)

            'Dim clsGrid As New clsGridOperations
            'clsGrid.MapDriveAdUser("X", "\\gildv128\c$")

            If Command.Length = 10 Then

                Dim emailObject As New clsEMailReports

                'Exit the program after execution
                Environment.Exit(0)
            ElseIf Command() = "HamburgPhantoms" Then
                Dim emailObject As New clsEMailReports(Command)
                Environment.Exit(0)
            ElseIf Command.Length = 7 And Command() = "Hamburg" Then
                Dim emailObject As New clsEMailReports(Command)
                Environment.Exit(0)
            ElseIf Command() = "HamburgShop" Then
                Dim emailObject As New clsEMailReports(Command)
                Environment.Exit(0)
            ElseIf Command() = "Newark" Then
                Dim emailObject As New clsEMailReports(Command)
                Environment.Exit(0)
            ElseIf Command() = "ASCPCHPOffsiteAndBacklog" Then
                Dim emailObject As New clsEMailReports(Command)
                Environment.Exit(0)
            ElseIf Command() = "UpdateOutOfOfficeInfo" Then
                Dim setup As New clsSetupOperations
                'setup.UpdateOutOfOfficeInfo()
                Environment.Exit(0)
            ElseIf Command() = "CHPDailyReschedules" Then
                Dim emailObject As New clsEMailReports(Command)
                Environment.Exit(0)
            ElseIf Command() = "Hamburg OTP Real" Then
                Dim emailObject As New clsEMailReports(Command)
                Environment.Exit(0)
            ElseIf Command() = "Hamburg Export Dual Use" Then
                Dim emailObject As New clsEMailReports(Command)
                Environment.Exit(0)
            ElseIf Command.Length > 0 Then

                'Here we do distinct between ASCP transformation and MRP transformations
                If Command() = "ASCP" Then
                    Dim exitCode As Integer = -1
                    Dim transform As New clsTransformationASCP(Date.Today, Command)

                    'Run the transformation exit Code will return -1 when an error occured
                    exitCode = transform.runTransformation()

                    Environment.Exit(exitCode)
                Else
                    'An exit code of -1 does indicate and error!
                    Dim exitCode As Integer = -1
                    Dim transform As New clsTransformation(Date.Today, Command)

                    'Run the transformation exit Code will return -1 when an error occured
                    exitCode = transform.runTransformation

                    Environment.Exit(exitCode)
                End If

            Else
                Dim frm As New frmWorkbenchProjects
                'frm.Show()
                'Me.MainForm.Show()
            End If
            'CheckForShortcut() '*** need to get out before publish!

        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MsgBox(e.Exception.ToString)
            e.ExitApplication = False
        End Sub

        ''' <summary>
        ''' This will create a Application Reference file on the users desktop
        ''' if they do not already have one when the program is loaded.
        ''' Check for them running the deployed version before doing this,
        ''' so it doesn't kick it when you're running it from Visual Studio.
        ''' Need to import: System.Deployment.Application and System.Reflection
        ''' </summary>
        Private Shared Sub CheckForShortcut()
            Try

                        ' if (system.deployment.application.applicationdeployment.isnetworkdeployed) then
                'Dim ad As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
                'If (ad.IsFirstRun) Then 'first time user has run the app since installation or update
                Dim code As Assembly = Assembly.GetExecutingAssembly()
                Dim company As String = String.Empty
                Dim description As String = String.Empty
                If (Attribute.IsDefined(code, GetType(AssemblyCompanyAttribute))) Then
                    Dim ascompany As AssemblyCompanyAttribute = _
                        CType(Attribute.GetCustomAttribute(code, _
                        GetType(AssemblyCompanyAttribute)), AssemblyCompanyAttribute)
                    company = ascompany.Company
                End If
                If (Attribute.IsDefined(code, GetType(AssemblyDescriptionAttribute))) Then
                    Dim asdescription As AssemblyDescriptionAttribute = _
                        CType(Attribute.GetCustomAttribute(code, _
                        GetType(AssemblyDescriptionAttribute)), AssemblyDescriptionAttribute)
                    description = "Autobahn"
                End If
                If (company.Length > 0 AndAlso description.Length > 0) Then
                    Dim desktopPath As String = String.Empty
                    desktopPath = String.Concat( _
                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop), _
                        "\", description, ".appref-ms")
                    Dim shortcutName As String = String.Empty
                    shortcutName = String.Concat(
                        Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                        "\", "Autobahn", "\", description, ".appref-ms")
                    System.IO.File.Copy(shortcutName, desktopPath, True)
                End If
                '   End If
                'End If
            Catch ex As Exception
                'MessageBox.Show(ex.Message)
                Dim err As New clsExceptionManagement
                err.createErrorLog("CreateShortcut", ex, 1)
            End Try
        End Sub
    End Class

End Namespace

