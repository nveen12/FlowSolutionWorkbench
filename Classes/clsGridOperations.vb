Imports System
Imports System.Management
Imports System.IO
'Imports managementObject


'Impersonate
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Security.Permissions
Imports Microsoft.VisualBasic
Imports Microsoft.Win32.SafeHandles
Imports System.Runtime.ConstrainedExecution
Imports System.Security


Public Class clsGridOperations

	'Private strSambaShare As String = "\\gildv224.flowserve.net\st2dev2_chp_int"
	'Private strSambaShare As String = "\\gildv224.flowserve.net\st2it2_chp_int"
    'Private strSambaShare As String = "\\gildv218.flowserve.net\st1uat_chp_int"
    'Private strSambaShare As String = "\\gilap139.flowserve.net\st1prod_chp_int"
    Private strSambaShare As String = "\\gilis80\sr1prod_chp_int"

    Private strCurrentMappedDrive As String
    Private strPossibleDrives() As String = {"A", "B", "F", "H", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W"}
    Private strMappedDrives() As String

    Public Declare Function WNetAddConnection2 Lib "mpr.dll" Alias "WNetAddConnection2A" _
(ByRef lpNetResource As NETRESOURCE, ByVal lpPassword As String, _
ByVal lpUserName As String, ByVal dwFlags As Integer) As Integer

    Public Declare Function WNetCancelConnection2 Lib "mpr" Alias "WNetCancelConnection2A" _
  (ByVal lpName As String, ByVal dwFlags As Integer, ByVal fForce As Integer) As Integer

    ' <StructLayout(LayoutKind.Sequential)> _
    Public Structure NETRESOURCE
        Public dwScope As Integer
        Public dwType As Integer
        Public dwDisplayType As Integer
        Public dwUsage As Integer
        Public lpLocalName As String
        Public lpRemoteName As String
        Public lpComment As String
        Public lpProvider As String
    End Structure

    Public Const ForceDisconnect As Integer = 1
    Public Const RESOURCETYPE_DISK As Long = &H1

    Public Function MapDriveAdUser(ByVal DriveLetter As String, ByVal UNCPath As String) As Boolean

        Dim nr As NETRESOURCE
        Dim strUsername As String
        Dim strPassword As String
        strCurrentMappedDrive = DriveLetter & ":"


        nr = New NETRESOURCE
        nr.lpRemoteName = UNCPath
        nr.lpLocalName = strCurrentMappedDrive

        nr.dwType = RESOURCETYPE_DISK

        Dim result As Integer
        result = WNetAddConnection2(nr, "", "", 0)

        If result = 0 Then
            Return True
        Else
            'Retry once to find another open drive to map to
            Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_NetworkConnection ")
            Dim obj As ManagementObject
            Dim nextInt As Integer = 0
            Dim strMappedDrives As String = ""
            For Each obj In searcher.Get
                If Not obj.Item("LocalName") Is Nothing Then
                    strMappedDrives += obj.Item("LocalName").ToString
                End If

            Next

            'Find out if we try to map to an existing drive
            If strMappedDrives.Contains(DriveLetter) Then
                For Each h As String In strPossibleDrives
                    If Not strMappedDrives.Contains(h) Then
                        strCurrentMappedDrive = h + ":"         'Changes to the next open file path
                        Exit For
                    End If
                Next
            End If

            nr.lpLocalName = strCurrentMappedDrive
            result = WNetAddConnection2(nr, strPassword, strUsername, 0)
            If result = 0 Then
                Return True
            Else
                Return False
            End If

        End If

    End Function


    ''' <summary>
    ''' Maps a samba share to the local windows machine
    ''' </summary>
    ''' <param name="DriveLetter">The drive letter to choose.</param>
    ''' <param name="UNCPath">The unc path of the samba share for example</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function MapDrive(ByVal DriveLetter As String, ByVal UNCPath As String) As Boolean

        Dim nr As NETRESOURCE
        Dim strUsername As String
        Dim strPassword As String
        strCurrentMappedDrive = DriveLetter & ":"

     

        nr = New NETRESOURCE
        nr.lpRemoteName = UNCPath
        nr.lpLocalName = strCurrentMappedDrive
		'strUsername = "st2dev2_chp_int" '(add parameters to pass this if necessary)
		'strPassword = "tr2Spech" '(add parameters to pass this if necessary)
		'strUsername = "st2it2_chp_int"
		'strPassword = "4atred5A" 
        'strUsername = "st1uat_chp_int"             'UAT account
        'strPassword = "HadA29ew"
        strUsername = "st1prod_chp_int"             'Production account
        strPassword = "ZuSWe55a"

		nr.dwType = RESOURCETYPE_DISK

        Dim result As Integer
        '    result = WNetAddConnection2(nr, strPassword, strUsername, 0)

        'This is the new r12 publication style
        result = WNetAddConnection2(nr, Nothing, Nothing, 0)

        If result = 0 Then
            Return True
        Else
            'Retry once to find another open drive to map to
            Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_NetworkConnection ")
            Dim obj As ManagementObject
            Dim nextInt As Integer = 0
            Dim strMappedDrives As String = ""
            For Each obj In searcher.Get
                If Not obj.Item("LocalName") Is Nothing Then
                    strMappedDrives += obj.Item("LocalName").ToString
                End If

            Next

            'Find out if we try to map to an existing drive
            If strMappedDrives.Contains(DriveLetter) Then
                For Each h As String In strPossibleDrives
                    If Not strMappedDrives.Contains(h) Then
                        strCurrentMappedDrive = h + ":"         'Changes to the next open file path
                        Exit For
                    End If
                Next
            End If

            nr.lpLocalName = strCurrentMappedDrive
            'result = WNetAddConnection2(nr, strPassword, strUsername, 0)
            result = WNetAddConnection2(nr, Nothing, Nothing, 0)

            If result = 0 Then
                Return True
            Else
                Return False
            End If

        End If
    End Function

    Public Function UnMapDrive(ByVal DriveLetter As String) As Boolean
        Dim rc As Integer
        rc = WNetCancelConnection2(strCurrentMappedDrive, 0, ForceDisconnect)

        If rc = 0 Then
            Return True
        Else
            Return False
        End If

    End Function


    ''' <summary>
    ''' Export procedure that create comma delimited file on a specific folder
    ''' </summary>
    ''' <param name="grdView">The gridview that contains the data.</param>
    ''' <param name="rows">The rows that should be exported.</param>
    ''' <param name="columnNames">The columns that should be exported.</param>
    ''' <param name="filePath">The destination for the export.</param>
    ''' <remarks></remarks>
    Public Sub exportPipeDelimited(ByVal grdView As DataTable, ByVal rows() As Integer, ByVal columnNames() As String, ByVal filePath As String, ByVal numberOfValues As Integer)
        Try

        

            If numberOfValues = 0 Then
                Exit Sub
            End If
            Dim fs As IO.StreamWriter
            ' Try

            'Write the transaction to the backend.
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            Dim ds As New DataSet

            db.Connect()

            For i As Integer = 0 To numberOfValues - 1
                ds = db.Query("SELECT ISNULL(MAX(transactionID), 0) + 1 FROM warehouseTransactions")
                Dim newID As Integer = ds.Tables(0).Rows(0).Item(0)
                db.SecureInsertQueryParams("INSERT INTO [warehouseTransactions] " & _
                        "([transactionID]            " & _
                        ",[transactionType]          " & _
                        ",[organizationID]           " & _
                        ",[headerID]                 " & _
                        ",[scheduleShipDate]       " & _
                        ", createdBY                 " & _
                        ", creationDate)              " & _
                     "VALUES(@1,@2,@3,@4,@5,@6,@7)", newID, "salesOrderLinesView", grdView.Rows(i).Item("organization_id").ToString, _
                     grdView.Rows(i).Item("order_no").ToString, Format(grdView.Rows(i).Item("schedule_ship_date"), "yyyyMMdd"), Environment.UserName, Format(Now, "yyyyMMdd HH:mm:ss"))

            Next
            db.Disconnect()

            'connect to the samba drive
            'Me.UnMapDrive("K")
            ' Me.mapDriveSamba()              'Map drive is not working, handled manually 
            Me.MapDrive("B", Me.strSambaShare)

            Dim fs2 As New IO.StreamWriter(filePath, False)
            fs = fs2

            'create the columns
            Dim header As String = ""
            'For j As Integer = 0 To columnNames.Count - 1
            '    header += columnNames(j).Trim + ","
            'Next
            'Specific to the sales order pick release
            header = "SO_NUMBER,SCHEDULED_SHIP_DATE,ORGANIZATION"

            fs.WriteLine(header)

            'Have to select the maximum schedule ship date
            For i As Integer = 0 To numberOfValues - 1
                Dim row As String = ""
                ' For j As Integer = 0 To columnNames.Count - 1
                'row += grdView.Rows(rows(i)).Cells(columnNames(j)).Value.ToString + ","
                If grdView.Rows(i).Item("organization_id") = 881 Or grdView.Rows(i).Item("organization_id") = 1004 Then
                    row += grdView.Rows(i).Item("order_no").ToString + "," + Format(grdView.Rows(i).Item("schedule_ship_date"), "MM/dd/yyyy").ToString.Replace(".", "/") + "," + "CHP" + ","
                End If
                fs.WriteLine(row)
                'Saves the transaction to the backend
            Next
            fs.Close()
            Me.UnMapDrive("B")
            MessageBox.Show("Pick Release File has been placed succesfully. " & " Used drive - " & Me.strCurrentMappedDrive)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Dim err As New clsExceptionManagement
            err.createErrorLog("exportPipeDelimited", ex, 1)
        End Try

    End Sub

    ''' <summary>
    ''' Same function as export pipe delimited but executed from a server.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub exportPipeDelimitedFromServer(ByVal grdView As DataTable, ByVal rows() As Integer, ByVal columnNames() As String, ByVal filePath As String, ByVal numberOfValues As Integer)
        Try



            If numberOfValues = 0 Then
                Exit Sub
            End If
            Dim fs As IO.StreamWriter

            Dim fs2 As New IO.StreamWriter(filePath, False)
            fs = fs2

            'create the columns
            Dim header As String = ""

            'Specific to the sales order pick release
            header = "SO_NUMBER,SCHEDULED_SHIP_DATE,ORGANIZATION"

            fs.WriteLine(header)

            'Have to select the maximum schedule ship date
            For i As Integer = 0 To numberOfValues - 1
                Dim row As String = ""
                ' For j As Integer = 0 To columnNames.Count - 1
                'row += grdView.Rows(rows(i)).Cells(columnNames(j)).Value.ToString + ","
                If grdView.Rows(i).Item("organization_id") = 881 Then
                    row += grdView.Rows(i).Item("order_no").ToString + "," + Format(grdView.Rows(i).Item("schedule_ship_date"), "MM/dd/yyyy").ToString.Replace(".", "/") + "," + "CHP" + ","
                End If
                fs.WriteLine(row)
                'Saves the transaction to the backend
            Next

            fs.Close()


        Catch ex As Exception

            Dim err As New clsExceptionManagement
            err.createErrorLog("exportPipeDelimitedFromServer", ex, 1)
        End Try
    End Sub



	Public Sub writePromiseDateToSambaShare(ByVal filePath As String, ByVal poNumber As String, ByVal poLineNumber As String, ByVal releaseNumber As String, ByVal shipmentNumber As String, ByVal promiseDate As String)
		Try
			'connect to the samba drive
            'Me.UnMapDrive("T")
            Me.MapDrive("A", Me.strSambaShare)

			'Writing the file to the samba share
			Dim fs As New IO.StreamWriter(filePath, False)
			'Dim header As String = "PO_NUMBER,PO_LINE_NUMBER,RELEASE_NO,SHIPMENT_NO,PROMISE_DATE"
			'fs.WriteLine(header)
			'fs.WriteLine(poNumber + "," + poLineNumber + "," + releaseNumber + "," + shipmentNumber + "," + promiseDate)
			Dim header As String = "PO_NUMBER,PO_LINE_NUMBER,RELEASE_NO,SHIPMENT_NO,PROMISE_DATE, OPERATING_UNIT_NAME"
			fs.WriteLine(header)
			fs.WriteLine(poNumber + "," + poLineNumber + "," + releaseNumber + "," + shipmentNumber + "," + promiseDate + "," + "US FSG Operating Unit" + ",")	'*** remember to soft code the operating unit name.  add it to the po extract 
			fs.Close()
            Me.UnMapDrive("A")
		Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("writePromiseDateToSambaShare", ex, 1)
		End Try
	End Sub

    Public Sub writeJobStartDateToSambaShare(ByVal filePath As String, ByVal jobNo As String, ByVal startDate As String, Optional ByVal strHeaderBuildDate As String = "", Optional ByVal strBuildDate As String = "")
        Try
            'connect to the samba drive
            Me.MapDrive("A", Me.strSambaShare)

            'Writing the file to the samba share
            Dim fs As New IO.StreamWriter(filePath, False)
            Dim header As String = "ORG_CODE,JOB_NUMBER,START_DATE" & strHeaderBuildDate
            fs.WriteLine(header)
            '**** Need to remember to soft code a mapping here, so we are able to handle multiple sites
            fs.WriteLine("CHP" + "," + jobNo + "," + startDate + strBuildDate)
            fs.Close()
            Me.UnMapDrive("A")

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("writeJobStartDateToSambaShare", ex, 1)
        End Try
    End Sub

    Public Sub exportExcel(ByVal grdView As DataGridView, ByVal filePath As String)

        ' Open the file and write the headers
        Dim fs As New IO.StreamWriter(filePath, False)
        Dim newStyleID As Integer = 1                  ' The next style number for the xml parsing

        fs.WriteLine("<?xml version=""1.0""?>")
        fs.WriteLine("<?mso-application progid=""Excel.Sheet""?>")
        fs.WriteLine("<ss:Workbook xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet"">")
        ' Create the styles for the worksheet
        fs.WriteLine("  <ss:Styles>")
        ' Style for the column headers
        fs.WriteLine("    <ss:Style ss:ID=""1"">")
        fs.WriteLine("      <ss:Font ss:Bold=""1""/>")
        fs.WriteLine("      <ss:Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" " & _
            "ss:WrapText=""1""/>")
        fs.WriteLine("      <ss:Interior ss:Color=""#C0C0C0"" ss:Pattern=""Solid""/>")
        fs.WriteLine("<Style ss:ID=""s4"">  " & _
                     " <NumberFormat ss:Format=""yyyy-mm-dd""/> " & _
                     " </Style>")

        fs.WriteLine("    </ss:Style>")
        ' Style for the column information
        fs.WriteLine("    <ss:Style ss:ID=""2"">")
        fs.WriteLine("      <ss:Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>")
        fs.WriteLine("    </ss:Style>")
        'Save the specific short dates
        'For i As Integer = 0 To grdView.Columns.Count - 1
        '    If grdView.Columns(i).Visible Then
        '        'Distinct between datetime and the rest - When datetime use a different format
        '        If grdView.Columns(i).ValueType.Name = "DateTime" Then
        '            fs.WriteLine("<ss:Style ss:ID=""" & 60 + i & """>")
        '            fs.WriteLine("<ss:NumberFormat ss:Format=""Short Date""/>")
        '            fs.WriteLine("</Style>")
        '        End If
        '    End If
        'Next
        fs.WriteLine("      <ss:Style ss:ID=""3"">")
        fs.WriteLine("      <ss:NumberFormat ss:Format=""yyyy-mm-dd""/>")
        fs.WriteLine("      </Style>")
        'Finish the style section
        fs.WriteLine("  </ss:Styles>")
        ' Write the worksheet contents
        fs.WriteLine("<ss:Worksheet ss:Name=""Measurement Results"">")
        fs.WriteLine("  <ss:Table>")

        For i As Integer = 0 To grdView.Columns.Count - 1
            If grdView.Columns(i).Visible Then
                'Distinct between datetime and the rest - When datetime use a different format
                If grdView.Columns(i).ValueType.Name = "DateTime" Then
                    fs.WriteLine(String.Format("    <ss:Column ss:Width=""{0}""/>", _
         grdView.Columns.Item(i).Width))
                Else
                    fs.WriteLine(String.Format("    <ss:Column ss:Width=""{0}""/>", _
          grdView.Columns.Item(i).Width))
                End If

            End If
        Next

        fs.WriteLine("    <ss:Row>")
        For i As Integer = 0 To grdView.Columns.Count - 1

            If grdView.Columns(i).Visible Then

                'writes the header of the file
                fs.WriteLine(String.Format("      <ss:Cell ss:StyleID=""1"">" & _
               "<ss:Data ss:Type=""String"">{0}</ss:Data></ss:Cell>", _
               grdView.Columns.Item(i).HeaderText))
            End If

        Next
        fs.WriteLine("    </ss:Row>")

        ' Check for an empty row at the end due to Adding allowed on the DataGridView
        Dim subtractBy As Integer, cellText As String
        If grdView.AllowUserToAddRows = True Then subtractBy = 2 Else subtractBy = 1
        ' Write contents for each cell
        For i As Integer = 0 To grdView.RowCount - subtractBy
            fs.WriteLine(String.Format("    <ss:Row ss:Height=""{0}"">", _
                grdView.Rows(i).Height))
            For intCol As Integer = 0 To grdView.Columns.Count - 1

                If grdView.Columns(intCol).Visible Then

                    If DBNull.Value.Equals(grdView.Item(intCol, i).Value) Then
                        cellText = ""
                    Else
                        cellText = grdView.Item(intCol, i).Value.ToString.Replace("<", "")
                        cellText = cellText.Replace(">", "")
                        'cellText = cellText.Replace("/", "")
                        cellText = cellText.Replace("&", "")
                    End If

                    ' Check for null cell and change it to empty to avoid error
                    Dim strCurrentType As String = grdView.Columns(intCol).ValueType.Name
                    'Write date time
                    If strCurrentType = "DateTime" Then

                        'Distinct between dbnull and not dbnull
                        If Not DBNull.Value.Equals(grdView.Item(intCol, i).Value) Then
                            fs.WriteLine(String.Format("      <ss:Cell ss:StyleID=""3"">" & _
                       "<ss:Data ss:Type=""DateTime"">{0}</ss:Data></ss:Cell>", _
                        Format(grdView.Item(intCol, i).Value, "yyyy-MM-dd")))
                        Else
                            fs.WriteLine(String.Format("      <ss:Cell ss:StyleID=""3"">" & _
                       "<ss:Data ss:Type=""String"">{0}</ss:Data></ss:Cell>", _
                        ""))
                        End If


                        'Write a number
                    ElseIf strCurrentType = "Double" Then
                        If Not DBNull.Value.Equals(grdView.Item(intCol, i).Value) Then
                            fs.WriteLine(String.Format("      <ss:Cell ss:StyleID=""2"">" & _
                       "<ss:Data ss:Type=""Number"">{0}</ss:Data></ss:Cell>", _
                       FormatNumber(grdView.Item(intCol, i).Value, 5, TriState.False, TriState.False, TriState.False).Replace(",", ".")))
                        Else
                            fs.WriteLine(String.Format("      <ss:Cell ss:StyleID=""2"">" & _
                      "<ss:Data ss:Type=""Number"">{0}</ss:Data></ss:Cell>", ""))
                        End If
                        

                        'Everything else is written as string
                    Else
                        fs.WriteLine(String.Format("      <ss:Cell ss:StyleID=""2"">" & _
                        "<ss:Data ss:Type=""String"">{0}</ss:Data></ss:Cell>", _
                        cellText.ToString))
                    End If
                End If

            Next
            fs.WriteLine("    </ss:Row>")
        Next
        ' Close  the document
        fs.WriteLine("  </ss:Table>")
        fs.WriteLine("</ss:Worksheet>")
        fs.WriteLine("</ss:Workbook>")
        fs.Close()

    End Sub




    Public Sub saveReportViewerFile(ByRef dsSalesOrder As DataSet)
        'Method that gathers the rquired information for the automatic e-mail system

        ' Dim frm As New frmReportViewer("Sales Order Backlog", "ProjectManagerWorkbench\Reporting\repSalesOrderDGV.rdlc", "dsSalesOrderBacklog_sales_orders", dsSalesOrder)



    End Sub





End Class


Public Class ImpersonationDemo

    'Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As [String], _
    '    ByVal lpszDomain As [String], ByVal lpszPassword As [String], _
    '    ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, _
    '    ByRef phToken As IntPtr) As Boolean

    Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As [String], _
        ByVal lpszDomain As [String], ByVal lpszPassword As [String], _
        ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, _
        <Out()> ByRef phToken As SafeTokenHandle) As Boolean

    Public Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean

    ' Test harness.
    ' If you incorporate this code into a DLL, be sure to demand FullTrust.
    <PermissionSetAttribute(SecurityAction.Demand, Name:="FullTrust")> _
    Public Overloads Shared Sub Main(ByVal domainName As String, ByVal userName As String, ByVal password As String)
        Dim safeTokenHandle As SafeTokenHandle
        Dim tokenHandle As New IntPtr(0)
        Try


            ' Dim userName, domainName As String

            ' Get the user token for the specified user, domain, and password using the 
            ' unmanaged LogonUser method.  
            ' The local machine name can be used for the domain name to impersonate a user on this machine.
            'Console.Write("Enter the name of a domain on which to log on: ")
            'domainName = Console.ReadLine()

            'Console.Write("Enter the login of a user on {0} that you wish to impersonate: ", domainName)
            'userName = Console.ReadLine()

            'Console.Write("Enter the password for {0}: ", userName)

            Const LOGON32_PROVIDER_DEFAULT As Integer = 0
            'This parameter causes LogonUser to create a primary token.
            Const LOGON32_LOGON_INTERACTIVE As Integer = 2

            ' Call LogonUser to obtain a handle to an access token.
            Dim returnValue As Boolean = LogonUser(userName, domainName, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, safeTokenHandle)

            Console.WriteLine("LogonUser called.")

            If False = returnValue Then
                Dim ret As Integer = Marshal.GetLastWin32Error()
                Console.WriteLine("LogonUser failed with error code : {0}", ret)
                Throw New System.ComponentModel.Win32Exception(ret)

                Return
            End If
            Using safeTokenHandle
                Dim success As String
                If returnValue Then success = "Yes" Else success = "No"
                Console.WriteLine(("Did LogonUser succeed? " + success))
                Console.WriteLine(("Value of Windows NT token: " + safeTokenHandle.DangerousGetHandle().ToString()))

                ' Check the identity.
                Console.WriteLine(("Before impersonation: " + WindowsIdentity.GetCurrent().Name))

                ' Use the token handle returned by LogonUser.
                Dim newId As New WindowsIdentity(safeTokenHandle.DangerousGetHandle())
                Using impersonatedUser As WindowsImpersonationContext = newId.Impersonate()

                    ' Check the identity.
                    Console.WriteLine(("After impersonation: " + WindowsIdentity.GetCurrent().Name))

                    ' Free the tokens.
                End Using
            End Using
        Catch ex As Exception
            Console.WriteLine(("Exception occurred. " + ex.Message))
        End Try
    End Sub 'Main 
End Class 'Class1


Public NotInheritable Class SafeTokenHandle
    Inherits SafeHandleZeroOrMinusOneIsInvalid

    Private Sub New()
        MyBase.New(True)

    End Sub 'New

    Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As [String], _
            ByVal lpszDomain As [String], ByVal lpszPassword As [String], _
            ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, _
            ByRef phToken As IntPtr) As Boolean
    <DllImport("kernel32.dll"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), SuppressUnmanagedCodeSecurity()> _
    Private Shared Function CloseHandle(ByVal handle As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean

    End Function
    Protected Overrides Function ReleaseHandle() As Boolean
        Return CloseHandle(handle)

    End Function 'ReleaseHandle
End Class 'SafeTokenHandle


