Public Class frmFilterSettings

    Private bLoaded As Boolean

    'Variables of standard filter setting
    Private sMajorOrder As String = ""
    Private sMinorOrder As String = ""
    Private sOrdersPastDue As String = ""
    Private entities As String()
    Private _organizationId As Integer
    Private bLoadedUserFilters As Boolean
    Private obfilterDetailObject As New List(Of Object)
    Private _lngFilterId As Long
    Private bUserFilterClicked As Boolean = False
    Private _filterForm As frmWorkbenchProjects
    Private _dgvFilterReference As New DataSet

    Private db As New DB.ServerDB(My.Settings("FlowConnectionString"))

    Public Sub New(ByVal objects() As String, ByVal objectNames() As String, ByVal organizationId As Integer, ByRef filterForm As frmWorkbenchProjects)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        entities = objects
        _organizationId = organizationId
        _filterForm = filterForm

        ' Add any initialization after the InitializeComponent() call.
        For i As Integer = 0 To objectNames.Count - 1
            cbObjects.Items.Add(objectNames(i))
        Next

        Me.loadFilters(objects(0))
        bLoaded = True

    End Sub



    Private Sub loadFilters(ByVal entity As String)
        Dim ds As New DataSet
        bLoadedUserFilters = False


        ds = db.SecureQueryParams("SELECT templateId, entity, filterString, filterName FROM warehouseFilterTemplates WHERE entity = @1 AND (organizationId IS NULL OR organizationId = @2) ORDER BY filterName ASC", entity, _organizationId)
        cbFilters.DataSource = Nothing

        cbFilters.DataSource = ds.Tables(0)
        cbFilters.ValueMember = "templateId"
        cbFilters.DisplayMember = "filterName"

        Dim ds3 As New DataSet
        cbUserImplementedTemplate.DataSource = Nothing

        ds3 = db.SecureQueryParams("SELECT templateId, entity, filterString, filterName FROM warehouseFilterTemplates WHERE entity = @1 AND (organizationId IS NULL OR organizationId = @2) ORDER BY filterName ASC", entity, _organizationId)
        cbUserImplementedTemplate.DataSource = ds3.Tables(0)
        cbUserImplementedTemplate.ValueMember = "templateId"
        cbUserImplementedTemplate.DisplayMember = "filterName"

        Dim row As System.Data.DataRow
        row = ds3.Tables(0).NewRow
        ds3.Tables(0).Rows.Add(row)
        cbUserImplementedTemplate.SelectedIndex = cbUserImplementedTemplate.Items.IndexOf(DBNull.Value)


        Dim ds2 As New DataSet

        ds2 = db.SecureQueryParams("SELECT filterId, filterName FROM warehouseUserFilters WHERE userName = @1 ORDER BY filterName ASC", Environment.UserName)
        cbUserFilterName.DataSource = ds2.Tables(0)
        cbUserFilterName.ValueMember = "filterId"
        cbUserFilterName.DisplayMember = "filterName"



        bLoadedUserFilters = True

    End Sub

    Private Sub loadSettingsForFilter()
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT subFilterId FROM warehouseUserFilterDetail WHERE filterId = @1", cbUserFilterName.SelectedValue)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Dim uc2 As New ucFilterSetup(ds.Tables(0).Rows(i).Item("subFilterId"), cbUserFilterName.SelectedValue, "Non", "Non", False, "Non", _organizationId)
            tlpCurrentFilters.Controls.Add(uc2)
            obfilterDetailObject.Add(uc2)
        Next

    End Sub

    Private Sub saveFilterSettings()
        Dim strFilter As String = Me.getFilterString(_lngFilterId)
        db.SecureNonQueryParams("UPDATE warehouseUserFilters SET filterString = @1 WHERE filterId = @2", strFilter, _lngFilterId)

    End Sub


    

    Private Sub cbObjects_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbObjects.SelectedIndexChanged

        If Me.bLoaded Then
            Dim index As Integer
            index = cbObjects.SelectedIndex
            'Selected a user filter
            If Not Me.bUserFilterClicked Then

                loadFilters(entities(index))
                Dim ds As DataSet = db.SecureQueryParams("SELECT [entity_name] " & _
          ",[database_column] " & _
          ",[column_header] " & _
          ",[column_size] " & _
          ",[column_order] " & _
          ",[user_visibility] " & _
          ",[visibility] " & _
          ",[user_name] " & _
          ",[column_id] " & _
          ",[data_gid_name] " & _
          ",[id] " & _
      "FROM [whColumnUserVisibleView] WHERE entity_name = @1 AND user_name = @2 AND user_visibility = 1 AND visibility = 1 AND column_header IS NOT NULL AND column_header <> '' ORDER BY column_header ASC", entities(index), Environment.UserName)

                cbColumns.DataSource = Nothing
                cbColumns.DataSource = ds.Tables(0)
                cbColumns.ValueMember = "database_column"
                cbColumns.DisplayMember = "column_header"

            Else

                index = cbObjects.SelectedIndex
                Dim ds As DataSet = db.SecureQueryParams("SELECT [entity_name] " & _
         ",[database_column] " & _
         ",[column_header] " & _
         ",[column_size] " & _
         ",[column_order] " & _
         ",[user_visibility] " & _
         ",[visibility] " & _
         ",[user_name] " & _
         ",[column_id] " & _
         ",[data_gid_name] " & _
         ",[id] " & _
     "FROM [whColumnUserVisibleView] WHERE entity_name = @1 AND user_name = @2 AND user_visibility = 1 AND visibility = 1 AND column_header IS NOT NULL AND column_header <> '' ORDER BY column_header ASC ", entities(index), Environment.UserName)

                cbColumns.DataSource = Nothing
                cbColumns.DataSource = ds.Tables(0)
                cbColumns.ValueMember = "database_column"
                cbColumns.DisplayMember = "column_header"


            End If

            index = cbObjects.SelectedIndex

            If entities(index) = "salesOrderLinesView" Then
                _dgvFilterReference = Me._filterForm.propDsSalesOrders
            ElseIf entities(index) = "POQueueView" Then
                _dgvFilterReference = Me._filterForm.propDsPurchaseOrders
            ElseIf entities(index) = "WIPQueueView" Then
                _dgvFilterReference = Me._filterForm.propDsWIP
            End If



        End If



    End Sub

    Private Sub btnAddFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFilter.Click

        'Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        If cbUserFilterName.Text = "" Then
            lblMessages.Text = "Please enter" & ControlChars.CrLf & "a new user filter name"
        ElseIf cbObjects.SelectedIndex = -1 Then
            lblMessages.Text = "Please select" & ControlChars.CrLf & "a form object you want to filter"
        Else
            Dim ds As DataSet = db.SecureQueryParams("SELECT * FROM  warehouseUserFilters  WHERE userName = @1 AND filterName = @2 ", Environment.UserName, cbUserFilterName.Text)
            Dim ds3 As DataSet = db.SecureQueryParams("SELECT * FROM  warehouseFilterTemplates WHERE  filterName = @1 ", cbUserFilterName.Text)

            If ds.Tables(0).Rows.Count > 0 Or ds3.Tables(0).Rows.Count > 0 Then
                lblMessages.Text = "Filter already" & ControlChars.CrLf & "exists"

            Else
                Dim index As Integer
                index = cbObjects.SelectedIndex

                Dim ds2 As DataSet = db.Query("SELECT MAX(filterId) + 1 As newId FROM warehouseUserFilters")
                db.SecureNonQueryParams("INSERT INTO warehouseUserFilters (userName, entity, filterName, filterId) VALUES (@1, @2, @3, @4)", Environment.UserName, entities(index), cbUserFilterName.Text, ds2.Tables(0).Rows(0).Item(0))
                lblMessages.Text = "New filter" & ControlChars.CrLf & "created"
                Me.loadFilters(entities(0))

            End If

        End If


    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        If cbUserFilterName.SelectedIndex <> -1 Then
            'look if we are adding with the same column.
            Dim ds2 As New DataSet
            ds2 = db.SecureQueryParams("SELECT entity FROM warehouseUserFilters WHERE filterId = @1", _lngFilterId)
            Dim index As Integer
            index = cbObjects.SelectedIndex
            
            If ds2.Tables(0).Rows.Count = 0 Then
                MessageBox.Show("Please create a new filter or select a filter first.")
            Else

                If ds2.Tables(0).Rows(0).Item(0) = entities(index) Then
                    Dim ds As New DataSet
                    ds = db.Query("SELECT ISNULL(MAX(subFilterId),1) + 1 AS newId FROM warehouseUserFilterDetail")

                    Dim uc1 As New ucFilterSetup(ds.Tables(0).Rows(0).Item(0), cbUserFilterName.SelectedValue, cbColumns.SelectedValue, cbColumns.Text, True, entities(index), _organizationId)
                    tlpCurrentFilters.Controls.Add(uc1)
                    obfilterDetailObject.Add(uc1)
                    lblMessages.Text = "New column" & ControlChars.CrLf & "created"

                Else
                    lblMessages.Text = "Please reselect the" & ControlChars.CrLf & "filter"
                End If
            End If




        Else
            lblMessages.Text = "Please select" & ControlChars.CrLf & "your filter"
        End If


    End Sub

    Private Sub cbUserFilterName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbUserFilterName.SelectedIndexChanged

        If bLoadedUserFilters Then

            Me.bUserFilterClicked = True

            tlpCurrentFilters.Controls.Clear()
            Me.loadSettingsForFilter()

            _lngFilterId = cbUserFilterName.SelectedValue

            Dim index As Integer
            index = cbObjects.SelectedIndex

            Dim ds5 As New DataSet
            ds5 = db.SecureQueryParams("SELECT  [filterString] " & _
                ",[filterName] " & _
                ",[filterId] " & _
                ",[templateId] " & _
                ",[userName] " & _
                ",[entity] " & _
                ",[filterStringUser] " & _
                ",[filterNameUser] " & _
                ",[creationDate] " & _
                ",[templateCreationDate] " & _
                "      FROM [userfilterTemplateView] WHERE  filterId = @1 ", cbUserFilterName.SelectedValue)

            If ds5.Tables(0).Rows.Count > 0 Then
                Dim dt As DataTable = cbUserImplementedTemplate.DataSource

                For i As Integer = 0 To dt.Rows.Count - 1

                    If Not DBNull.Value.Equals(dt.Rows(i).Item("templateId")) AndAlso Not DBNull.Value.Equals(ds5.Tables(0).Rows(0).Item("templateId")) Then

                        If dt.Rows(i).Item("templateId") = ds5.Tables(0).Rows(0).Item("templateId") Then

                            cbUserImplementedTemplate.SelectedIndex = i

                        End If
                    End If

                Next
                'cbUserImplementedTemplate.SelectedIndex = cbUserImplementedTemplate.Items.IndexOf(ds.Tables(0).Rows(0).Item("templateId").ToString)
                'Select the right entity for this filter

            Else
                cbUserImplementedTemplate.SelectedIndex = cbUserImplementedTemplate.Items.IndexOf(DBNull.Value)
            End If

            Dim ds2 As New DataSet
            ds2 = db.SecureQueryParams("SELECT  [filterString] " & _
                ",[filterName] " & _
                ",[filterId] " & _
                ",[templateId] " & _
                ",[userName] " & _
                ",[entity] " & _
                " FROM warehouseUserFilters WHERE filterId = @1 ", cbUserFilterName.SelectedValue)

            If ds2.Tables(0).Rows.Count > 0 Then
                For j As Integer = 0 To entities.Count - 1
                    If entities(j).ToString.ToLower = ds2.Tables(0).Rows(0).Item("entity").ToString.ToLower Then
                        cbObjects.SelectedIndex = j
                        Exit For

                    End If
                Next
            End If

            Me.bUserFilterClicked = False
        End If



    End Sub

    Private Sub btnSaveChanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveChanges.Click

        If cbUserFilterName.SelectedIndex <> -1 AndAlso cbUserImplementedTemplate.SelectedIndex <> -1 Then
            db.SecureNonQueryParams("UPDATE warehouseUserFilters SET templateId = @1 WHERE filterId = @2", cbUserImplementedTemplate.SelectedValue, _lngFilterId)

        End If
        'After the closed routine, the state of the checkbox should also be saved
        For Each h In obfilterDetailObject
            If h.visible Then
                Try
                    h.SaveUserControl()
                Catch ex As Exception

                End Try

            End If

        Next
        Me.saveFilterSettings()
       
    End Sub



    Private Function getFilterString(ByVal currentFilterId As Integer)
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim strSQL As String = ""
        Dim ds As New DataSet

        'current Selected Column
        Dim columnName As String = ""
        Dim bNoConnection As Boolean = True


        'Add the lower level filter items
        ds = db.SecureQueryParams("SELECT currentColumn, sqlOperation, phraseValue, connectionElements FROM warehouseUserFilterDetail WHERE filterId = @1 ORDER BY currentColumn", currentFilterId)
        'Logic to implement the filter logic
        Dim ds2 As DataSet = db.SecureQueryParams("SELECT filterString FROM userfilterTemplateView WHERE filterId = @1 AND filterString IS NOT NULL", currentFilterId)
        If Not ds2.Tables(0).Rows.Count = 0 Then
            strSQL += ds2.Tables(0).Rows(0).Item(0)
        End If

        If ds.Tables(0).Rows.Count > 0 Then
            strSQL += " AND ( "
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Check for new elements
                'This is not allowed to take place at the last element
                bNoConnection = True

                If columnName <> ds.Tables(0).Rows(i).Item("currentColumn") AndAlso columnName <> "" Then
                    strSQL += ") AND ("
                    bNoConnection = False
                End If

                columnName = ds.Tables(0).Rows(i).Item("currentColumn")

                'When there is no value in the phrase the programs assumes we are searching for null values
                If ds.Tables(0).Rows(i).Item("phraseValue") = "" AndAlso ds.Tables(0).Rows(i).Item("sqlOperation") = "=" Then
                    strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " IS "
                    'If we have a does not equal, we have to add IS NOT
                ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "" AndAlso ds.Tables(0).Rows(i).Item("sqlOperation") = "<>" Then
                    strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " IS NOT "
                    'If we have a list of values applied, we also have to open the parenthesis
                ElseIf ds.Tables(0).Rows(i).Item("sqlOperation") = "IN" Then
                    strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " " & ds.Tables(0).Rows(i).Item("sqlOperation") & " "          'Not needed any more
                    'When the user enter a % we will use like instead of the sql operation
                ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.Contains("%") Then
                    strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " " & " LIKE " & " "
                Else
                    'Add the current sql operation to the filter string
                    If ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "n" Then
                        If ds.Tables(0).Rows(i).Item("sqlOperation") = "=" Or ds.Tables(0).Rows(i).Item("sqlOperation") = "Like" Then
                            strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " = "
                        ElseIf ds.Tables(0).Rows(i).Item("sqlOperation") = "Not Like" Then
                            strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " <> "
                        Else
                            strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " " & ds.Tables(0).Rows(i).Item("sqlOperation") & " "
                        End If

                    ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "y" Then
                        If ds.Tables(0).Rows(i).Item("sqlOperation") = "=" Or ds.Tables(0).Rows(i).Item("sqlOperation") = "Like" Then
                            strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " = "
                        ElseIf ds.Tables(0).Rows(i).Item("sqlOperation") = "Not Like" Then
                            strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " <> "
                        Else
                            strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " " & ds.Tables(0).Rows(i).Item("sqlOperation") & " "
                        End If

                    Else
                        strSQL += " " & ds.Tables(0).Rows(i).Item("currentColumn") & " " & ds.Tables(0).Rows(i).Item("sqlOperation") & " "
                    End If

                End If

                '
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("phraseValue")) Then

                    'Operating the replacements of characters
                    If ds.Tables(0).Rows(i).Item("phraseValue").ToString.Contains("'") Then
                        ds.Tables(0).Rows(i).Item("phraseValue") = ds.Tables(0).Rows(i).Item("phraseValue").ToString.Replace("'", "%")
                    End If

                    'capture if there is not table 
                    If _dgvFilterReference.Tables.Count = 0 Then
                        MessageBox.Show("Please select a view first.")
                        Exit Function
                    End If
                    
                    If _dgvFilterReference.Tables(0).Columns(columnName).DataType.Name = "String" Then

                        If ds.Tables(0).Rows(i).Item("sqlOperation") = "LIKE" Or ds.Tables(0).Rows(i).Item("sqlOperation").ToString.Trim = "NOT LIKE" Then
                            If ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "y" Then
                                strSQL += " 1 "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "n" Then
                                strSQL += " 0 "
                            Else
                                strSQL += "  '%" & ds.Tables(0).Rows(i).Item("phraseValue") & "%'  "
                            End If


                            'Also have to consider this for string values
                        ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "" Then
                            strSQL += " NULL "
                            'Look for the list of value IN operator, add the required apostrophes
                        ElseIf ds.Tables(0).Rows(i).Item("sqlOperation") = "IN" Then
                            'We are adding apostrophes at the beginning, end and around every single ,
                            'Adding the start
                            Dim strCreatedList As String = "('"
                            'Parsing the string into a character array
                            Dim strArrChar() As Char = ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToCharArray
                            For k As Integer = 0 To ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToCharArray.Length - 2
                                If Not strArrChar(k) = "," Then
                                    'just add the character
                                    strCreatedList += strArrChar(k)
                                Else
                                    'If the operator is a comma, also add the apostrophe
                                    strCreatedList += "'"
                                    strCreatedList += strArrChar(k)
                                    strCreatedList += "'"
                                End If
                            Next
                            'Assign the new created list to the sql statement, also ending apostrophe and close the parenthesis
                            strSQL += strCreatedList & "')"
                        Else
                            If ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "y" Then
                                strSQL += " 1 "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "n" Then
                                strSQL += " 0 "
                            Else
                                strSQL += "  '" & ds.Tables(0).Rows(i).Item("phraseValue") & "'  "
                            End If

                        End If


                    ElseIf _dgvFilterReference.Tables(0).Columns(columnName).DataType.Name = "DateTime" Then
                        Dim d As New DateTime
                        'Look for the generic time ranges in the values
                        If ds.Tables(0).Rows(i).Item("phraseValue") = "Today" Or ds.Tables(0).Rows(i).Item("phraseValue") = "+7 Days" Or ds.Tables(0).Rows(i).Item("phraseValue") = "Month End" Or ds.Tables(0).Rows(i).Item("phraseValue") = "+30 Days" Then

                            If ds.Tables(0).Rows(i).Item("phraseValue") = "Today" Then
                                strSQL += " getDate() "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "+7 Days" Then
                                strSQL += "DATEADD(day,7,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "Month End" Then
                                strSQL += " dbo.ufn_GetLastDayOfMonth(getDate()) "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "+30 Days" Then
                                strSQL += "DATEADD(day,30,getDate())"
                            End If

                        Else

                            Try
                                d = ds.Tables(0).Rows(i).Item("phraseValue")
                                strSQL += "'" & Format(d, "yyyyMMdd") & "'"
                            Catch ex As Exception
                                MessageBox.Show("Please enter a valid date time value. Will use today instead.")
                                strSQL += "'" & Format(d, "yyyyMMdd") & "'"
                            End Try

                        End If


                    Else
                        If ds.Tables(0).Rows(i).Item("phraseValue") = "" Then
                            'This should represent dbNull.
                            strSQL += " NULL "
                            ' strSQL += "''"
                            'Make the list of values if we have an IN field
                        ElseIf ds.Tables(0).Rows(i).Item("sqlOperation") = "IN" Then
                            strSQL += " ( " & ds.Tables(0).Rows(i).Item("phraseValue") & " ) "
                        Else
                            strSQL += ds.Tables(0).Rows(i).Item("phraseValue")

                        End If

                    End If

                End If

                'find out if the next column is a change
                'Implement the connection string if the next column is the same as the last colum
                'Be aware this is different to th calculation above because we now did change the column already
                'Also exclude the last element of the table, because it does not need a SQL conncetion at the end

                Dim bexcludeConnectionString As Boolean = True

                If i <= ds.Tables(0).Rows.Count - 2 Then
                    If columnName = ds.Tables(0).Rows(i + 1).Item("currentColumn") Then
                        bexcludeConnectionString = False
                    ElseIf i = ds.Tables(0).Rows.Count - 1 Then

                    End If
                End If



                'Dim bColumnChange As Boolean = False

                'If ds.Tables(0).Rows.Count > i + 1 Then
                '    If columnName <> ds.Tables(0).Rows(i + 1).Item("currentColumn") AndAlso columnName <> "" Then
                '        bColumnChange = True
                '    End If
                'Else
                '    bColumnChange = True
                'End If

                'If ds.Tables(0).Rows.Count > 2 AndAlso i = (ds.Tables(0).Rows.Count - 1) AndAlso columnName <> "" AndAlso columnName <> ds.Tables(0).Rows(i - 1).Item("currentColumn") Then
                '    bColumnChange = True
                '    bNoConnection = True
                'End If

                'If bNoConnection And Not bColumnChange Then
                '    strSQL += ds.Tables(0).Rows(i).Item("connectionElements")
                'End If

                If Not bexcludeConnectionString Then
                    strSQL += ds.Tables(0).Rows(i).Item("connectionElements")
                End If


            Next
            strSQL += " )"
        End If
        

        Return strSQL

    End Function

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        db.SecureNonQueryParams("DELETE FROM warehouseUserFilters WHERE filterId = @1", _lngFilterId)
        db.SecureNonQueryParams("DELETE FROM warehouseUserFilterDetail WHERE filterId = @1", _lngFilterId)

        'Make sure that the default relation is also deleted
        db.SecureNonQueryParams("DELETE FROM warehouseUserDefaultFilter WHERE userFilterID = @1", _lngFilterId)

        'Reload the content
        Me.loadFilters(entities(0))
        Me._filterForm.reloadUserViews()
        lblMessages.Text = "Filter deleted" & ControlChars.CrLf & "succesfully"

    End Sub

    

    Private Sub ShareFilterWithAllUsersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShareFilterWithAllUsersToolStripMenuItem.Click
        Dim dlg As New dlgChooseName("Select a user you want to share your filter with.", cbUserFilterName.SelectedText)
        dlg.ShowDialog()

        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Me.shareFilterToUser(dlg.propReturnUserName)
        End If

    End Sub

    Private Sub ShareFiltersWithTheCurrentOrganizationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShareFiltersWithTheCurrentOrganizationToolStripMenuItem.Click
        Dim dlg As New dlgChooseName(Me._organizationId)
        dlg.ShowDialog()
        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Me.shareFilterToUser(dlg.propReturnUserName)
        End If

    End Sub

    ''' <summary>
    ''' Procedure that copies filters from one user to the other.
    ''' </summary>
    ''' <param name="newOwner">The user this filter should go to.</param>
    ''' <remarks></remarks>
    Private Sub shareFilterToUser(ByVal newOwner As String)
        If cbFilters.SelectedIndex = -1 Then
            MessageBox.Show("Please select a filter first.")
            Exit Sub
        End If

        'Select the old header information
        Dim oldfilterID As Integer = cbUserFilterName.SelectedValue
        Dim newFilterId As Integer = db.Query("SELECT MAX(filterId) + 1 As newId FROM warehouseUserFilters").Tables(0).Rows(0).Item(0)

        Dim dsHeader As DataSet = db.SecureQueryParams("SELECT [filterId] " & _
                                          ",[templateId] " & _
                                          ",[userName] " & _
                                          ",[entity] " & _
                                          ",[filterString] " & _
                                          ",[filterName] " & _
                                          ",[creationDate] " & _
                                          ",[numberOfUsages] " & _
                                          ",[isDefault] " & _
                                      "FROM [warehouseUserFilters] WHERE filterID = @1", oldfilterID)


        Dim dsChildren As DataSet = db.SecureQueryParams("SELECT  [subFilterId] " & _
                                          ",[filterId] " & _
                                          ",[currentColumn] " & _
                                          ",[sqlOperation] " & _
                                          ",[phraseValue] " & _
                                          ",[substring] " & _
                                          ",[startPosition] " & _
                                          ",[endPosition] " & _
                                          ",[connectionElements] " & _
                                          ",[creationDate] " & _
                                          ",[createdBy] " & _
                                          ",[columnNumber] " & _
                                          "  FROM  warehouseUserFilterDetail  " & _
                                          " WHERE  filterId = @1 ORDER BY subFilterID DESC", oldfilterID)

        'Make sure we do not copy to the other user and he already has this filter in his menu
        Dim dsAlready As DataSet = db.SecureQueryParams("SELECT filterID FROM warehouseUserFilters WHERE userName = @1 AND filterName = @2", newOwner, dsHeader.Tables(0).Rows(0).Item("filterName"))
        If dsAlready.Tables(0).Rows.Count > 0 Then
            MessageBox.Show("This user has already this filter:" & dsHeader.Tables(0).Rows(0).Item("filterName"))
            Exit Sub
        End If

        'Insert the header
        db.SecureInsertQueryParams("INSERT INTO [warehouseUserFilters] " & _
                                   "([filterId] " & _
                                   ",[templateId] " & _
                                   ",[userName] " & _
                                   ",[entity] " & _
                                   ",[filterString] " & _
                                   ",[filterName] " & _
                                   ",[creationDate] " & _
                                   " ) " & _
                                   "  VALUES (@1,@2,@3,@4,@5,@6,getDate()) ", newFilterId, dsHeader.Tables(0).Rows(0).Item("templateId") _
                                   , newOwner _
                                   , dsHeader.Tables(0).Rows(0).Item("entity") _
                                   , dsHeader.Tables(0).Rows(0).Item("filterString") _
                                   , dsHeader.Tables(0).Rows(0).Item("filterName"))

        'Insert the child elements
        For Each dr As DataRow In dsChildren.Tables(0).Rows

            Dim newSubFilterId As Integer = db.Query("SELECT ISNULL(MAX(subFilterId),1) + 1 AS newId FROM warehouseUserFilterDetail").Tables(0).Rows(0).Item(0)
            'get the new sub filter id for every row
            db.SecureInsertQueryParams(" INSERT INTO [warehouseUserFilterDetail] " & _
                                      " ([subFilterId] " & _
                                      " ,[filterId] " & _
                                      " ,[currentColumn] " & _
                                      " ,[sqlOperation] " & _
                                      " ,[phraseValue] " & _
                                      " ,[substring] " & _
                                      " ,[startPosition] " & _
                                      " ,[endPosition] " & _
                                      " ,[connectionElements] " & _
                                      " ,[creationDate] " & _
                                      " ,[createdBy] " & _
                                      " ,[columnNumber]) " & _
                                      "  VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,getDate(),@10, @11) ", newSubFilterId, newFilterId, dr.Item("currentColumn") _
                                      , dr.Item("sqlOperation"), dr.Item("phraseValue"), dr.Item("substring") _
                                      , dr.Item("startPosition"), dr.Item("endPosition"), dr.Item("connectionElements") _
                                      , newOwner, dr.Item("columnNumber"))


        Next

    End Sub

    
End Class