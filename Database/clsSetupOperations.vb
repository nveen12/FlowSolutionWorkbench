Imports System.Configuration
Imports System.DirectoryServices
Imports Microsoft.Exchange.WebServices.Data
Imports Microsoft.Exchange.WebServices.Autodiscover
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Public Class clsSetupOperations
    Private db As New DB.ServerDB(My.Settings("FLOWConnectionString"))
    Private _userName As String

    ''' <summary>
    ''' Start of the object where it implements a new user in the database.
    ''' </summary>
    ''' <param name="strUserName">The user name, which should be created</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal strUserName As String)
        _userName = strUserName
        Me.insertNewUser()
        Me.import_grid_column_for_sizing(_userName)

    End Sub

    ''' <summary>
    ''' This just creates the object enabling single opeations.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    Private Sub insertNewUser()
        Try
            ' Dim propValueList As System.Collections.Generic.Dictionary(Of String, String) = GetUserPropertiesFromAD(_userName)
            Dim firstName As String = String.Empty
            Dim lastName As String = String.Empty
            Dim email As String = String.Empty

            'If propValueList.Count = 3 Then
            '    firstName = propValueList("givenname")
            '    lastName = propValueList("sn")
            '    email = propValueList("mail")
            'End If

            '*** For next customers we need to implement a standard organization to pull the data from

            db.SecureNonQueryParams("INSERT INTO [warehouse_users] " &
               "([user_Name] " &
               ",[standard_operating_unit_id] " &
               ",[standard_organization_id] " &
               ",[language] " &
               ",[userDefaultOrganization] " &
               ",[lastRoleSelected] " &
               ",[firstName] " &
               ",[lastName] " &
               ",[eMail] ) " &
               " VALUES " &
               "(@1,254,255, 'Standard', 255, 1, @2, @3, @4)", _userName, firstName, lastName, email)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("insertNewUser", ex, 1)
            'Inserts only the basic information when there is an exception while getting data from LDAP
            db.SecureNonQueryParams("INSERT INTO [warehouse_users] " & _
               "([user_Name] " & _
               ",[standard_operating_unit_id] " & _
               ",[standard_organization_id] " & _
               ",[language] " & _
               ",[userDefaultOrganization] " & _
               ",[lastRoleSelected] " & _
               " VALUES " & _
               "(@1,881,881, 'Standard', 881, 1)", _userName)



        Finally
        End Try
    End Sub
    ''' <summary>
    ''' This method retrieves the First Name, Last Name and Email from the Active directory for the specified user.
    ''' </summary>
    ''' <param name="UserName">Provide the username for which the data to be retrieved from active directory</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUserPropertiesFromAD(ByVal UserName As String) As System.Collections.Generic.Dictionary(Of String, String)
        Dim myDirectory As DirectoryEntry = Nothing
        Dim mySearcher As DirectorySearcher = Nothing
        Dim mySearchResultColl As SearchResultCollection = Nothing
        Dim mySearchResult As SearchResult
        Dim myResultPropColl As ResultPropertyCollection
        Dim myResultPropValueColl As ResultPropertyValueCollection
        Dim propValue As String = String.Empty
        Dim propValueList As New System.Collections.Generic.Dictionary(Of String, String)

        Try
            Dim sPath As String = ConfigurationManager.AppSettings("LDAP").ToString()
            myDirectory = New DirectoryEntry(sPath, ConfigurationManager.AppSettings("LDAPUserName").ToString(), ConfigurationManager.AppSettings("LDAPPassword").ToString(), AuthenticationTypes.Secure)
            Dim filter As String = "samaccountname=" & UserName
            mySearcher = New DirectorySearcher(myDirectory, filter)

            Try
                mySearchResultColl = mySearcher.FindAll()
                If mySearchResultColl.Count = 0 Then
                    Return propValueList
                End If

                mySearchResult = mySearchResultColl(0)
                myResultPropColl = mySearchResult.Properties

                'Retrieve from the properties collection the first name, last name and email of the user                
                For Each propertyItem As DictionaryEntry In mySearchResult.Properties
                    Select Case propertyItem.Key.ToString().ToLower()
                        Case "givenname"
                            myResultPropValueColl = myResultPropColl(propertyItem.Key.ToString().ToLower())
                            If myResultPropValueColl.Count > 0 Then
                                propValueList.Add(propertyItem.Key.ToString().ToLower(), myResultPropValueColl(0).ToString())
                            End If
                        Case "sn"
                            myResultPropValueColl = myResultPropColl(propertyItem.Key.ToString().ToLower())
                            If myResultPropValueColl.Count > 0 Then
                                propValueList.Add(propertyItem.Key.ToString().ToLower(), myResultPropValueColl(0).ToString())
                            End If
                        Case "mail"
                            myResultPropValueColl = myResultPropColl(propertyItem.Key.ToString().ToLower())
                            If myResultPropValueColl.Count > 0 Then
                                propValueList.Add(propertyItem.Key.ToString().ToLower(), myResultPropValueColl(0).ToString())
                            End If
                        Case Else
                            Exit Select
                    End Select
                Next

            Catch ex As Exception
                Dim err As New clsExceptionManagement
                err.createErrorLog("GetUserPropertiesFromAD", ex, 1)
            End Try
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("GetUserPropertiesFromAD", ex, 2)
        Finally
            mySearchResultColl = Nothing
            mySearcher = Nothing
            myDirectory = Nothing
        End Try
        Return propValueList
    End Function

    Public Sub insertLogoutStamp()

        Dim ds As DataSet = db.Query("SELECT MAX(ISNULL(loginID,1)) + 1 As ID FROM warehouseUsersLogins")
        db.SecureNonQueryParams("INSERT INTO [warehouseUsersLogins] " & _
           "([loginID] " & _
           ",[creationDate] " & _
           ",[userName] " & _
           ",[type]) " & _
        " VALUES " & _
           "(@1, getDate(), @2, 'Logout')", ds.Tables(0).Rows(0).Item(0), Environment.UserName)

    End Sub

    Public Sub insertLoginStamp()

        'The new data set to go for.
        Dim ds As DataSet = db.Query("SELECT MAX(ISNULL(loginID,1)) + 1 As ID FROM warehouseUsersLogins")

        db.SecureNonQueryParams("INSERT INTO [warehouseUsersLogins] " & _
           "([loginID] " & _
           ",[creationDate] " & _
           ",[userName] " & _
           ",[type]) " & _
        " VALUES " & _
           "(@1, getDate(), @2, 'Login')", ds.Tables(0).Rows(0).Item(0), Environment.UserName)

    End Sub

    ''' <summary>
    ''' Imports all new columns from the template table to the user specific table.
    ''' </summary>
    ''' <param name="user_name">The user who needs the update.</param>
    ''' <remarks>Just imports the new columns, not yet available for the user.</remarks>
    Public Sub import_grid_column_for_sizing(ByVal user_name As String)
        Try


            db.SecureNonQueryParams("INSERT INTO [warehouse_grid_user_size_order] (user_name, entity_name, column_id, column_size, column_order, visibility) " &
        "(SELECT " &
        "(SELECT user_Name FROM [warehouse_users] WHERE [user_Name] = @1) " &
        ",wgc.entity_name , wgc.id,wgc.columnWidth, columnOrder, wgc.user_visibility as visible " &
        "FROM [warehouse_grid_columns] wgc  WHERE wgc.id > (SELECT ISNULL(MAX(column_id),0) FROM [warehouse_grid_user_size_order] WHERE [user_name] = @2))", user_name, user_name)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("SendMail", ex, 1)
        End Try
    End Sub

    Public Sub resetColumnWidth()


        db.SecureNonQueryParams("UPDATE warehouse_grid_user_size_order   SET warehouse_grid_user_size_order.column_size = warehouse_grid_columns.columnWidth FROM warehouse_grid_columns " & _
        " INNER JOIN warehouse_grid_user_size_order " & _
        " ON  warehouse_grid_user_size_order.column_id = warehouse_grid_columns.id WHERE warehouse_grid_user_size_order.user_name = @1", Environment.UserName)


    End Sub


    Public Sub import_entity_column_translation()

        db.SecureQueryParams("INSERT INTO [warehouse_grid_column_translations] (entity_name, column_id, column_name, language) " & _
                             "(SELECT entity_name ,id, database_column, 'de' FROM [warehouse_grid_columns] )", Nothing)

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="dgv_column_name"></param>
    ''' <remarks></remarks>
    Public Sub import_grid_header_names(ByVal id As String, ByVal dgv_column_name As String)
        db.SecureNonQueryParams("UPDATE [wh_column_user_visible_v] SET [data_grid_name] = @1 WHERE id = @2", dgv_column_name, id)
    End Sub
    ''' <summary>
    ''' Method to update first name, last name and email from the active directory
    ''' </summary>
    ''' <param name="strUserName"></param>
    ''' <remarks></remarks>
    Public Sub updateUserDetails(ByVal strUserName As String)
        Try
            Dim propValueList As System.Collections.Generic.Dictionary(Of String, String) = GetUserPropertiesFromAD(strUserName)
            Dim firstName As String = String.Empty
            Dim lastName As String = String.Empty
            Dim email As String = String.Empty
            If propValueList.Count = 3 Then
                firstName = propValueList("givenname")
                lastName = propValueList("sn")
                email = propValueList("mail")
            End If
            If firstName <> String.Empty AndAlso lastName <> String.Empty AndAlso email <> String.Empty Then
                db.SecureNonQueryParams("UPDATE [warehouse_users] SET [firstName] = @1, [lastName] = @2, [eMail] = @3 WHERE [user_Name] = @4 ", firstName, lastName, email, strUserName)
            End If

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("updateUserDetails", ex, 1)
        End Try
    End Sub
    '''' <summary>
    '''' Get the out of office from exchange server for the logged in users and update details in the database
    '''' </summary>
    '''' <remarks></remarks>
    'Public Sub UpdateOutOfOfficeInfo()
    '    Try
    '        Dim service As New ExchangeService(ExchangeVersion.Exchange2007_SP1)
    '        service.Credentials = CredentialCache.DefaultNetworkCredentials
    '        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf CertificateValidationCallBack)
    '        service.Url = New Uri("https://webmail.flowserve.com/ews/Exchange.asmx")
    '        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
    '        Dim ds As New DataSet
    '        Dim myOofSettings As New OofSettings()

    '        ds = db.SecureQueryParams("SELECT [eMail] FROM [warehouse_users] WHERE [user_Name] = @1", Environment.UserName)
    '        If ds.Tables.Count > 0 Then
    '            If ds.Tables(0).Rows.Count > 0 Then
    '                If ds.Tables(0).Rows(0).Item(0).ToString() <> String.Empty Then
    '                    myOofSettings = service.GetUserOofSettings(ds.Tables(0).Rows(0).Item(0).ToString())
    '                    Dim IsOutofOffice As Boolean = False
    '                    If myOofSettings.State <> Microsoft.Exchange.WebServices.Data.OofState.Disabled Then
    '                        IsOutofOffice = True
    '                    End If
    '                    UpdateOutofOfficeDetails(Environment.UserName, IsOutofOffice, myOofSettings.Duration.StartTime, myOofSettings.Duration.EndTime)
    '                End If
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Dim err As New clsExceptionManagement
    '        err.createErrorLog("UpdateOutOfOfficeInfo", ex, 1)
    '    End Try
    'End Sub

    Private Shared Function CertificateValidationCallBack(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function
    ''' <summary>
    ''' Update out of office details in the database
    ''' </summary>
    ''' <param name="strUserName">Logged in User Name</param>
    ''' <param name="IsOutofOffice">Out of office is active or not </param>
    ''' <param name="StartDate">Start date and time of out of office</param>
    ''' <param name="EndDate">End date and time of out of office</param>
    ''' <remarks></remarks>
    Public Sub UpdateOutofOfficeDetails(ByVal strUserName As String, ByVal IsOutofOffice As Int32, ByVal StartDate As DateTime, ByVal EndDate As DateTime)
        db.SecureNonQueryParams("UPDATE [warehouse_users] SET [outOfOffice] = @1, [outFromDate] = @2, [outUnitilDate] = @3 WHERE [user_Name] = @4 ", IsOutofOffice, StartDate, EndDate, strUserName)
    End Sub
End Class
