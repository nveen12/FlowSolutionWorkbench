Imports System.Windows.Forms




Public Class dlgCustomFilter

    Private filterStringView() As String = {"Equals", "Contains", "Does Not Equal", "Is Greater Than", "Is Greater Than Or Equal To", "Is Less Than", "Is Less Than Or Equal To", "Does Not Contain", "IN"}
    Private filterStringValues() As String = {"=", "LIKE", "<>", ">", ">=", "<", "<=", " NOT LIKE", "IN"}

    Private _organizationId As Integer
    Private _entity As String
    Private _columnHeader As String
    Private _columnName As String
    Private _dgvFilterGrid As DataGridView
    Private _dgvFilterReference As DataGridView

    Private filterId As Integer
    Private _detailId1 As Integer
    Private _detailId2 As Integer

    Private strCurrentFilterEntity As String




    ''' <summary>
    ''' Constructor to really show the dialogue to the user.
    ''' </summary>
    ''' <param name="dgv"></param>
    ''' <param name="dgvHard"></param>
    ''' <param name="organizationId"></param>
    ''' <param name="entity"></param>
    ''' <param name="columnHeader"></param>
    ''' <param name="columnName"></param>
    ''' <param name="currentFilterString"></param>
    ''' <remarks></remarks>

    Public Sub New(ByVal dgv As DataGridView, ByVal dgvHard As DataGridView, ByVal organizationId As Double, ByVal entity As String, ByVal columnHeader As String, ByVal columnName As String, ByRef currentFilterString As String)

        _organizationId = organizationId
        _entity = entity
        _columnHeader = columnHeader
        _columnName = columnName


        _dgvFilterGrid = dgvHard
        _dgvFilterReference = dgv

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim lngNewBorder As New Long

        Me.determineCurrentFilterEntity(entity)

        Me.Text = Me.Text & " For Column: " & columnHeader
        Me.gbColumn.Text = columnHeader


        'Load the the values of the filter string view into the combobox
        For i As Integer = 0 To filterStringView.Count - 1
            cbSQLOperation1.Items.Add(filterStringView(i))
            cbSQlOperation2.Items.Add(filterStringView(i))
        Next
        cbSQLOperation1.SelectedIndex = 1


        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT filterId FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", strCurrentFilterEntity, Environment.UserName)

        If ds.Tables(0).Rows.Count = 0 Then
            Me.createFilter(strCurrentFilterEntity)
        Else
            Me.filterId = ds.Tables(0).Rows(0).Item(0)
            Me.loadSubFilter()
        End If



        cbValueList1.Focus()
        cbValueList1.Select()


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="entity"></param>
    ''' <remarks></remarks>
    Private Sub determineCurrentFilterEntity(ByVal entity As String)

        If entity = "salesOrderLinesView" Then
            strCurrentFilterEntity = "SalesOrderLinesView"
        ElseIf entity = "POQueueView" Then
            strCurrentFilterEntity = "POQueueView"
        ElseIf entity = "MRPShortagesView" Then
            strCurrentFilterEntity = "MRPShortagesView"
        ElseIf entity = "WIPQueueView" Then
            strCurrentFilterEntity = "WIPQueueView"
        End If

    End Sub

    Private Sub loadSubFilter()
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet


        ds = db.SecureQueryParams("SELECT subFilterId FROM warehouseUserFilterDetail WHERE currentColumn = @1 AND filterid = @2 AND " & _
                                  " columnNumber = 1", Me._columnName, Me.filterId)

        If ds.Tables(0).Rows.Count > 0 Then
            _detailId1 = ds.Tables(0).Rows(0).Item(0)

            ds = db.SecureQueryParams("SELECT  subFilterId " & _
                                         " ,filterId " & _
                                         " ,currentColumn " & _
                                         " ,sqlOperation " & _
                                         " ,phraseValue " & _
                                         " ,substring " & _
                                         " ,startPosition " & _
                                         " ,endPosition " & _
                                         " ,connectionElements " & _
                                         " ,creationDate " & _
                                         " ,createdBy " & _
                                         " ,columnNumber " & _
                                         "    FROM warehouseUserFilterDetail WHERE subFilterId = @1 ", _detailId1)


            If ds.Tables(0).Rows(0).Item("connectionElements").ToString.Contains("AND") Then
                rbAnd.Checked = True
            Else
                rbOr.Checked = True
            End If

            For i As Integer = 0 To filterStringValues.Count - 1
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("sqlOperation")) Then
                    If filterStringValues(i) = ds.Tables(0).Rows(0).Item("sqlOperation") Then
                        cbSQLOperation1.SelectedIndex = i
                        Exit For
                    End If
                End If

            Next

            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("phraseValue")) Then
                Me.cbValueList1.Text = ds.Tables(0).Rows(0).Item("phraseValue")
            End If

        End If

        ds = db.SecureQueryParams("SELECT subFilterId FROM warehouseUserFilterDetail WHERE currentColumn = @1 AND filterid = @2 AND " & _
                                 " columnNumber = 2", Me._columnName, Me.filterId)

        If ds.Tables(0).Rows.Count > 0 Then
            _detailId2 = ds.Tables(0).Rows(0).Item(0)

            ds = db.SecureQueryParams("SELECT  subFilterId " & _
     " ,filterId " & _
     " ,currentColumn " & _
     " ,sqlOperation " & _
     " ,phraseValue " & _
     " ,substring " & _
     " ,startPosition " & _
     " ,endPosition " & _
     " ,connectionElements " & _
     " ,creationDate " & _
     " ,createdBy " & _
     " ,columnNumber " & _
     "    FROM warehouseUserFilterDetail WHERE subFilterId = @1", _detailId2)

            For i As Integer = 0 To filterStringValues.Count - 1
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("sqlOperation")) Then
                    If filterStringValues(i) = ds.Tables(0).Rows(0).Item("sqlOperation") Then
                        cbSQlOperation2.SelectedIndex = i
                        Exit For
                    End If
                End If

            Next

            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("phraseValue")) Then
                Me.cbValueList2.Text = ds.Tables(0).Rows(0).Item("phraseValue")
            End If

        End If
    End Sub

    Private Sub createFilter(ByVal creationEntity As String)
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        Dim ds2 As DataSet = db.Query("SELECT MAX(filterId) + 1 As newId FROM warehouseUserFilters")

        db.SecureNonQueryParams("INSERT INTO warehouseUserFilters (userName, entity, filterName, filterId) VALUES (@1, @2,'TEMP', @3)", Environment.UserName, creationEntity, ds2.Tables(0).Rows(0).Item(0).ToString)

        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT filterId FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", strCurrentFilterEntity, Environment.UserName)
        Me.filterId = ds.Tables(0).Rows(0).Item(0)

    End Sub

    Private Sub saveSubFilter()

        Dim strConnection As String
        Dim detailId1 As Integer

        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        If rbAnd.Checked Then
            strConnection = " AND "
        Else
            strConnection = " OR "
        End If

        'Logic to check if this detail already exists
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT subFilterId FROM warehouseUserFilterDetail WHERE currentColumn = @1 AND filterid = @2 AND " & _
                                  " columnNumber = 1", Me._columnName, Me.filterId)

        'Select a new identifier
        If ds.Tables(0).Rows.Count = 0 Then
            Dim ds2 As New DataSet
            ds2 = db.Query("SELECT ISNULL(MAX(subFilterId),1) + 1 AS newId FROM warehouseUserFilterDetail")
            detailId1 = ds2.Tables(0).Rows(0).Item(0)

            'Insert a new value
            db.SecureQueryParams("INSERT INTO warehouseUserFilterDetail (subFilterId, filterId, currentColumn, connectionElements, columnNumber, creationDate, createdBy) VALUES (@1, @2, @3, @4, 1, getDate(), @5)" _
                                    , ds2.Tables(0).Rows(0).Item(0), Me.filterId, Me._columnName, strConnection, Environment.UserName)

        Else
            detailId1 = ds.Tables(0).Rows(0).Item(0)
        End If

        'Selects the stored details for this filter.
        db.SecureNonQueryParams("UPDATE [warehouseUserFilterDetail] " & _
                "SET  " & _
                "[currentColumn] = @1 " & _
                ",[sqlOperation] = @2 " & _
                ",[phraseValue] =  @3 " & _
                ",[connectionElements] = @4 " & _
                "WHERE subFilterId = @5 ", _columnName, filterStringValues(cbSQLOperation1.SelectedIndex), cbValueList1.Text, strConnection, detailId1)

        If cbSQlOperation2.SelectedIndex = -1 Then

        Else
            'Repeat the procedure for the second element
            'Logic to check if this detail already exists
            ds = db.SecureQueryParams("SELECT subFilterId FROM warehouseUserFilterDetail WHERE currentColumn = @1 AND filterid = @2 AND " & _
                                      " columnNumber = 2", Me._columnName, Me.filterId)


            If ds.Tables(0).Rows.Count = 0 Then
                Dim ds2 As New DataSet
                ds2 = db.Query("SELECT ISNULL(MAX(subFilterId),1) + 1 AS newId FROM warehouseUserFilterDetail")
                detailId1 = ds2.Tables(0).Rows(0).Item(0)

                'Insert a new value
                db.SecureQueryParams("INSERT INTO warehouseUserFilterDetail (subFilterId, filterId, currentColumn, connectionElements, columnNumber, creationDate, createdBy) VALUES (@1, @2, @3, @4, 2, getDate(), @5)" _
                                        , ds2.Tables(0).Rows(0).Item(0), Me.filterId, Me._columnName, strConnection, Environment.UserName)

            Else

                detailId1 = ds.Tables(0).Rows(0).Item(0)
            End If


            db.SecureNonQueryParams("UPDATE [warehouseUserFilterDetail] " & _
                    "SET  " & _
                    "[currentColumn] = @1 " & _
                    ",[sqlOperation] = @2 " & _
                    ",[phraseValue] =  @3 " & _
                    ",[connectionElements] = @4 " & _
                    "WHERE subFilterId = @5 ", _columnName, filterStringValues(cbSQlOperation2.SelectedIndex), cbValueList2.Text, strConnection, detailId1)



        End If

    End Sub

    Public Sub saveTranslatedFilterPhrase(ByVal currentFilterId As Integer)
        Try
            Dim strPhraseValue As String = Me.getFilterString(Me.filterId)
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            db.SecureNonQueryParams("UPDATE warehouseUserFilters SET filterString = @1 WHERE filterId = @2", strPhraseValue, currentFilterId)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("saveTranslatedFilterPhrase", ex, 1)
        End Try
        

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
                        ds.Tables(0).Rows(i).Item("phraseValue") = ds.Tables(0).Rows(i).Item("phraseValue").ToString.Replace("'", "")
                    End If

                    '----------------------------------------------------------------------------
                    'All string operations
                    If _dgvFilterReference.Columns(columnName).ValueType.Name = "String" Then

                        If ds.Tables(0).Rows(i).Item("sqlOperation") = "LIKE" Or ds.Tables(0).Rows(i).Item("sqlOperation").ToString.Trim = "NOT LIKE" Then
                            If ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "y" Then
                                strSQL += " 1 "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "n" Then
                                strSQL += " 0 "
                                ' deal with the _ not as a wildcard but an escape sequence: AND ( JobNoOperationSeq LIKE '%n_5%' ESCAPE 'n')  
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.Contains("_") Then
                                'detect the underscores and tranlate into the new structure: 
                                Dim charValues() As Char = ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToCharArray
                                Dim strNewPhraseValue As String = ""
                                For g As Integer = 0 To charValues.Length - 1
                                    If charValues(g) = "_" Then
                                        strNewPhraseValue += "n" & charValues(g)
                                    Else
                                        strNewPhraseValue += charValues(g)
                                    End If
                                Next
                                strSQL += "  '" & strNewPhraseValue & "' ESCAPE 'n' "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.Contains("?") Then
                                strSQL += "  '" & ds.Tables(0).Rows(i).Item("phraseValue").ToString.Replace("?", "_") & "'  "
                            Else
                                strSQL += "  '%" & ds.Tables(0).Rows(i).Item("phraseValue").ToString & "%'  "
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
                            For k As Integer = 0 To ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToCharArray.Length - 1
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
                            'Handle the other values <> like, not like
                            If ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "y" Then
                                strSQL += " 1 "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToLower = "n" Then
                                strSQL += " 0 "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.Contains("_") Then
                                'detect the underscores and tranlate into the new structure: 
                                Dim charValues() As Char = ds.Tables(0).Rows(i).Item("phraseValue").ToString.ToCharArray
                                Dim strNewPhraseValue As String = ""
                                For g As Integer = 0 To charValues.Length - 1
                                    If charValues(g) = "_" Then
                                        strNewPhraseValue += "n" & charValues(g)
                                    Else
                                        strNewPhraseValue += charValues(g)
                                    End If
                                Next
                                strSQL += "  '" & strNewPhraseValue & "' ESCAPE 'n' "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue").ToString.Contains("?") Then
                                strSQL += "  '" & ds.Tables(0).Rows(i).Item("phraseValue").ToString.Replace("?", "_") & "'  "
                            Else
                                strSQL += "  '" & ds.Tables(0).Rows(i).Item("phraseValue").ToString & "'  "
                            End If

                        End If


                    ElseIf _dgvFilterReference.Columns(columnName).ValueType.Name = "DateTime" Then
                        Dim d As New DateTime
                        'Look for the generic time ranges in the values
                        If ds.Tables(0).Rows(i).Item("phraseValue") = "Today" Or ds.Tables(0).Rows(i).Item("phraseValue") = "+7 Days" Or ds.Tables(0).Rows(i).Item("phraseValue") = "Month End" _
                        Or ds.Tables(0).Rows(i).Item("phraseValue") = "+30 Days" _
                        Or ds.Tables(0).Rows(i).Item("phraseValue") = "+14 Days" _
                        Or ds.Tables(0).Rows(i).Item("phraseValue") = "+90 Days" _
                        Or ds.Tables(0).Rows(i).Item("phraseValue") = "+180 Days" _
                        Or ds.Tables(0).Rows(i).Item("phraseValue") = "-7 Days" _
                        Or ds.Tables(0).Rows(i).Item("phraseValue") = "-14 Days" _
                        Or ds.Tables(0).Rows(i).Item("phraseValue") = "-90 Days" _
                        Or ds.Tables(0).Rows(i).Item("phraseValue") = "-180 Days" Then

                            If ds.Tables(0).Rows(i).Item("phraseValue") = "Today" Then
                                strSQL += " dbo.DateOnly(getdate())  "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "+7 Days" Then
                                strSQL += "DATEADD(day,7,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "+14 Days" Then
                                strSQL += "DATEADD(day,14,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "+90 Days" Then
                                strSQL += "DATEADD(day,90,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "+180 Days" Then
                                strSQL += "DATEADD(day,180,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "Month End" Then
                                strSQL += " dbo.ufn_GetLastDayOfMonth(getDate()) "
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "+30 Days" Then
                                strSQL += "DATEADD(day,30,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "-7 Days" Then
                                strSQL += "DATEADD(day,-7,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "-14 Days" Then
                                strSQL += "DATEADD(day,-14,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "-90 Days" Then
                                strSQL += "DATEADD(day,-90,getDate())"
                            ElseIf ds.Tables(0).Rows(i).Item("phraseValue") = "-180 Days" Then
                                strSQL += "DATEADD(day,-180,getDate())"
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

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        'No filter operation was selected
        If cbSQLOperation1.SelectedIndex = -1 Then
            MessageBox.Show("Please select an operation for the first filter", "Save error")
        Else

            'Save the sub filter
            Me.saveSubFilter()
            Me.saveTranslatedFilterPhrase(Me.filterId)

            _dgvFilterReference.Columns(_columnName).HeaderCell.Style.ForeColor = Color.AliceBlue
            _dgvFilterGrid.Columns(_columnName).HeaderCell.Style.ForeColor = Color.AliceBlue

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub



    Private Sub cbValueList1_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbValueList1.DropDown

        cbValueList1.Items.Clear()

        'The important column is moved to a list and then sorted in order to avoid viewing changes on the displayed piece.
        Dim ilist As New List(Of String)
        For i As Integer = 0 To _dgvFilterGrid.Rows.Count - 1
            If Not DBNull.Value.Equals(_dgvFilterGrid.Rows(i).Cells(_columnName).Value) Then
                ilist.Add(_dgvFilterGrid.Rows(i).Cells(_columnName).Value.ToString)
            End If

        Next
        ilist.Sort()
        '_dgvFilterGrid.Sort(_dgvFilterGrid.Columns(_columnName), System.ComponentModel.ListSortDirection.Ascending)

        'If the column we are loading is a datetime field, add generic range handler
        If _dgvFilterGrid.Columns(_columnName).ValueType.Name = "DateTime" Then
            cbValueList1.Items.Add("Today")
            cbValueList1.Items.Add("+7 Days")
            cbValueList1.Items.Add("+14 Days")
            cbValueList1.Items.Add("+30 Days")
            cbValueList1.Items.Add("+90 Days")
            cbValueList1.Items.Add("+180 Days")
            cbValueList1.Items.Add("Month End")
            cbValueList1.Items.Add("-7 Days")
            cbValueList1.Items.Add("-14 Days")
            cbValueList1.Items.Add("-90 Days")
            cbValueList1.Items.Add("-180 Days")


            cbValueList2.Items.Add("Today")
            cbValueList2.Items.Add("+7 Days")
            cbValueList2.Items.Add("+14 Days")
            cbValueList2.Items.Add("+30 Days")
            cbValueList2.Items.Add("+90 Days")
            cbValueList2.Items.Add("+180 Days")
            cbValueList2.Items.Add("Month End")
            cbValueList2.Items.Add("-7 Days")
            cbValueList2.Items.Add("-14 Days")
            cbValueList2.Items.Add("-90 Days")
            cbValueList2.Items.Add("-180 Days")
        Else
            For i As Integer = 0 To ilist.Count - 1

                If cbValueList1.Items.Contains(ilist(i)) Then
                Else
                    cbValueList1.Items.Add(ilist(i))
                End If

                If cbValueList2.Items.Contains(ilist(i)) Then
                Else
                    cbValueList2.Items.Add(ilist(i))
                End If

            Next
        End If

        


    End Sub




    Private Sub cbValueList2_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbValueList2.DropDown
        cbValueList2.Items.Clear()

        _dgvFilterGrid.Sort(_dgvFilterGrid.Columns(_columnName), System.ComponentModel.ListSortDirection.Descending)

        If _dgvFilterGrid.Columns(_columnName).ValueType.Name = "DateTime" Then
            cbValueList1.Items.Add("Today")
            cbValueList1.Items.Add("+7 Days")
            cbValueList1.Items.Add("Month End")
            cbValueList1.Items.Add("+30 Days")




            cbValueList2.Items.Add("Today")
            cbValueList2.Items.Add("+7 Days")
            cbValueList2.Items.Add("Month End")
            cbValueList2.Items.Add("+30 Days")
        Else
            For i As Integer = 0 To _dgvFilterGrid.Rows.Count - 1

                If cbValueList1.Items.Contains(_dgvFilterGrid.Rows(i).Cells(_columnName).Value) Then
                Else
                    cbValueList1.Items.Add(_dgvFilterGrid.Rows(i).Cells(_columnName).Value)

                End If

                If cbValueList2.Items.Contains(_dgvFilterGrid.Rows(i).Cells(_columnName).Value) Then
                Else
                    cbValueList2.Items.Add(_dgvFilterGrid.Rows(i).Cells(_columnName).Value)
                End If

            Next
        End If

      

    End Sub

  
    Private Sub dlgCustomFilter_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim globe As New clsPublicVariables
        Me.Text = globe.nameOfTool & " Auto Filter"
    End Sub
End Class
