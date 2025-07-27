Public Class clsUserControl

    Private db As New DB.ServerDB(My.Settings("FlowConnectionString"))
    Private lngRoleID As Long

    Public Sub New(ByVal roleID As Long)
        lngRoleID = roleID
    End Sub

    Public Sub New()

    End Sub


    ''' <summary>
    ''' Copies the order of columns from one user to the other.
    ''' </summary>
    ''' <param name="copyTo">The person who should get it.</param>
    ''' <param name="copyFrom">The person we want to copy from.</param>
    ''' <remarks></remarks>
    Public Sub shareViewOfColumns(ByVal copyTo As String, ByVal copyFrom As String)

        db.SecureNonQueryParams("UPDATE b SET b.column_order =  a.column_order  FROM warehouse_grid_user_size_order a JOIN warehouse_grid_user_size_order b " & _
                                "	ON a.column_id = b.column_id AND b.user_name = @1 " & _
                                " WHERE a.user_name = @2", copyTo, copyFrom)

    End Sub


    Public Sub applyAllUserRights(ByRef frm As frmWorkbenchProjects)
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT controlName, visibility, enabled FROM userInRollesAllRightsView WHERE user_Name= @1 AND formName = @2", Environment.UserName, frm.Name)

        Dim objControls() As Object = frm.roleSpecificControls
        'Make every control visible to the specific value.
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            For h As Integer = 0 To objControls.Length - 1
                If Not objControls(h) Is Nothing Then
                    If objControls(h).name = ds.Tables(0).Rows(i).Item("controlName").ToString Then
                        objControls(h).visible = ds.Tables(0).Rows(i).Item("visibility")
                        objControls(h).enabled = ds.Tables(0).Rows(i).Item("enabled")
                    End If
                End If
            Next
        Next

    End Sub

    ''' <summary>
    ''' Looks for controls on the form which should be made invisible.
    ''' </summary>
    ''' <param name="frm">The form this should be applied to.</param>
    ''' <remarks>The values and definitions are stored in the backend.</remarks>
    Public Sub applyUserRightsOnForm(ByRef frm As frmWorkbenchProjects)
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT controlName, visibility, enabled FROM whRoleRightsOnForms WHERE roleID = @1 AND formName = @2", lngRoleID, frm.Name)

        Dim objControls() As Object = frm.roleSpecificControls
        'Make every control visible to the specific value.
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            For h As Integer = 0 To objControls.Length - 1
                If Not objControls(h) Is Nothing Then
                    If objControls(h).name = ds.Tables(0).Rows(i).Item("controlName").ToString Then
                        objControls(h).visible = ds.Tables(0).Rows(i).Item("visibility")
                        objControls(h).enabled = ds.Tables(0).Rows(i).Item("enabled")
                    End If
                End If
            Next
        Next

    End Sub


    ''' <summary>
    ''' Function that looks for the user and role.
    ''' </summary>
    ''' <param name="userName"></param>
    ''' <param name="roleName"></param>
    ''' <returns>Returns true when the user is assigned to this role.</returns>
    ''' <remarks></remarks>
    Public Function userInRole(ByVal userName As String, ByVal roleName As String) As Boolean
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT * FROM userInRolesView WHERE roleName = @1 AND user_Name = @2", roleName, userName)

        If ds.Tables(0).Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Looks if the user is not allowed to view the data of the site.
    ''' </summary>
    ''' <param name="userName">The user name we want to check.</param>
    ''' <param name="organizationID">The organization id that has to be checked.</param>
    ''' <returns>True if the user is not allowed to view the data.</returns>
    ''' <remarks></remarks>
    Public Function userRejectedForSite(ByVal userName As String, ByVal organizationID As Integer) As Boolean
        Try

       
            Dim ds As New DataSet
            ds = db.SecureQueryParams("SELECT * FROM whUserFactories WHERE userName = @1 AND organizationID = @2", userName, organizationID)

            If ds.Tables(0).Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception

        End Try

    End Function

    Public Sub translateForms(ByRef frm As frmWorkbenchProjects, ByVal language As String)

        Dim ds As DataSet = db.SecureQueryParams("SELECT  [formName] " & _
                                                 " ,[controlName] " & _
                                                 " ,[language] " & _
                                                 " ,[translation] " & _
                                                 " FROM [whFormsTranslation] WHERE formName = @1 AND language = @2", frm.Name, language)
        Dim objControls() As Object = frm.roleSpecificControls

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Try
                For h As Integer = 0 To objControls.Length - 1
                    If Not objControls(h) Is Nothing Then
                        If objControls(h).name = ds.Tables(0).Rows(i).Item("controlName").ToString Then
                            objControls(h).Text = ds.Tables(0).Rows(i).Item("translation").ToString
                        End If
                    End If
                Next


            Catch ex As Exception

            End Try

        Next

    End Sub

End Class
