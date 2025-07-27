Imports System
Imports System.Reflection
Imports System.Windows.Forms 'these are needed to change properties of the data grid view (double buffered).  setting double buffered to True makes the form render on the screen much faster


''' <summary>
''' Class which contains operations for the grid view.
''' </summary>
''' <remarks>Has to get in the constructor the entity which should be formatted. Currently a mixture of hardcoded and table based procedures.</remarks>
Public Class clsGridDisplay


    Private dsHeaders As New DataSet
    Private columnSize As New DataSet
    Private columnConditionalFormat As New DataSet

    'Indicator if the object is working or not
    Private bReloading As Boolean = True
    Private bColumnReordering As Boolean = False
    Private dsColumnsSettting As DataSet            'Contains the setting from the server
    Private userLanguage As String                  'Contains the user language for the warehouse
    Private dbS As New DB.ServerDB(My.Settings.FLOWConnectionString)


    ''' <summary>
    ''' Constructor which mainly loads the settings into memory.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        loadColumnSettings()
    End Sub

    ''' <summary>
    ''' Procedure which loads the datagridview in a double buffered mode, so drilling is much faster.
    ''' </summary>
    ''' <param name="dgv">The datagridview which should be accelerated.</param>
    ''' <param name="setting">True, when it should be applied.</param>
    ''' <remarks>Solves also the scroll down issue.</remarks>
    Public Sub DoubleBuffered(ByVal dgv As DataGridView, ByVal setting As Boolean)

        Dim dgvType As Type = dgv.[GetType]()
        Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, setting, Nothing)

    End Sub

    ''' <summary>
    ''' Saves the column width change of a single column.
    ''' </summary>
    ''' <param name="columnName">The database column name.</param>
    ''' <param name="entity">The entitiy of the database column.</param>
    ''' <param name="columnWidth">The new width which should be saved.</param>
    ''' <remarks>Is invoked after a column width change which was invoked from the user.</remarks>
    Public Sub saveChangesOfSingleColumn(ByVal columnName As String, ByVal entity As String, ByVal columnWidth As Integer)
        'Dim db As New DB.ServerDB(My.Settings("FlowConnectionString").ToString)
        dbS.SecureNonQueryParams("UPDATE whColumnUserVisibleView SET column_size = @1 WHERE user_name = @2 AND database_column = @3 AND entity_name = @4", columnWidth _
                                    , Environment.UserName, columnName, entity)
        Me.loadColumnSettings()
    End Sub

    ''' <summary>
    ''' Saves the column order of a single column. Also saves all the new values after this column.
    ''' </summary>
    ''' <param name="columnName">The name of the column that should be saved.</param>
    ''' <param name="entity">The entity this column belongs to.</param>
    ''' <param name="columnOrder">The new display index.</param>
    ''' <remarks>Also updates the memory stored collection.</remarks>
    Public Sub saveChangesOfSingleColumnDisplayIndex(ByVal columnName As String, ByVal entity As String, ByVal columnOrder As Integer, ByRef dgv As DataGridView)
        'Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        dbS.Connect()
        Try
            Dim lngTest As Integer
            'Saving things to the backend
            lngTest = dbS.SecureNonQueryParams("UPDATE whColumnUserVisibleView SET column_order = @1 WHERE user_name = @2 AND database_column = @3 AND entity_name = @4", columnOrder _
                                , Environment.UserName, columnName, entity)

            Dim x As Integer = 0


        Catch ex As Exception
        Finally
            dbS.Disconnect()
        End Try

    End Sub

    ''' <summary>
    ''' Saves the changes of the column width to the backend.
    ''' </summary>
    ''' <param name="dgv">The datagridview which should be saved</param>
    ''' <param name="entity">The entity currently behind the data grid view</param>
    ''' <remarks>The width is stored at the backend so you can access your settings from different computers like switching between accountability room and private desk.</remarks>
    Public Sub saveChangesOfWidth(ByVal dgv As DataGridView, ByVal entity As String)
        'Dim db As New DB.ServerDB(My.Settings("FlowConnectionString").ToString)
        Dim affectedRows As Integer

        'Save the changes of the column width to the users 
        For i As Integer = 0 To dgv.Columns.Count - 1
            affectedRows = dbS.SecureNonQueryParams("UPDATE whColumnUserVisibleView SET column_size = @1 WHERE user_name = @2 AND database_column = @3 AND entity_name = @4", dgv.Columns(i).Width _
                                    , Environment.UserName, dgv.Columns(i).Name, entity)

        Next
        Me.loadColumnSettings()

    End Sub


    ''' <summary>
    ''' Applies the format to the grid.
    ''' </summary>
    ''' <param name="dgv">The data grid view you want to transform</param>
    ''' <param name="entityName">The name of the entity behind the data grid view.</param>
    ''' <remarks>Hide and Unhide columns, conditional formats of cells and rows, Order of the columns, size of the columns</remarks>
    Public Sub formatGrid(ByRef dgv As DataGridView, ByVal entityName As String, Optional ByVal organizationId As Integer = 0)
        ' Dim db As New DB.ServerDB(My.Settings("FlowConnectionString").ToString)
        Me.bReloading = True
        Try
            Debug.Print(Now)
            If dsColumnsSettting Is Nothing Then
                dbS.Connect()
                Me.loadColumnSettings()
            End If

            visibility(dgv, entityName)
            Debug.Print(Now.Second & " -" & Now.Millisecond)
            rename_Headers(dgv, entityName, organizationId)
            Debug.Print(Now.Second & " -" & Now.Millisecond)
            column_size(dgv, entityName)
            Debug.Print(Now.Second & " -" & Now.Millisecond)
            column_reordering(dgv, entityName)
            Debug.Print(Now.Second & " -" & Now.Millisecond)
            'conditionalFormatGrids(dgv, entityName)
            Debug.Print("Cond. Formatting:" & Now.Second & " -" & Now.Millisecond)

            'Me.replaceOldGrid(entityName, dgv)
            Me.bReloading = False
        Catch ex As Exception
        Finally
            dbS.Disconnect()
        End Try


    End Sub


    '''' <summary>
    '''' Changes the order of the columns in the datagrid.
    '''' </summary>
    '''' <param name="entity">The entity like wipQueueView</param>
    '''' <param name="dgv">The datagridview which should be reordered.</param>
    '''' <remarks>Hard coded in the source code.</remarks>
    'Private Sub replaceOldGrid(ByVal entity As String, ByVal dgv As DataGridView)

    '    If entity = "wipQueueView" Then

    '        'Reorder the columns programmatically
    '        dgv.Columns("DEPARTMENT_CODE").DisplayIndex = 0
    '        dgv.Columns("RESOURCE_CODE").DisplayIndex = 1
    '        dgv.Columns("PLANNER_CODE").DisplayIndex = 2
    '        dgv.Columns("ITEM_NO").DisplayIndex = 3
    '        dgv.Columns("DESCRIPTION").DisplayIndex = 4
    '        dgv.Columns("JobNoOperationSeq").DisplayIndex = 5
    '        dgv.Columns("FIRST_UNIT_START_DATE").DisplayIndex = 6
    '        dgv.Columns("SalesOrderLine").DisplayIndex = 7
    '        dgv.Columns("PARTY_NAME").DisplayIndex = 8
    '        dgv.Columns("MEANING").DisplayIndex = 9
    '        dgv.Columns("SCHEDULED_QUANTITY").DisplayIndex = 10
    '        dgv.Columns("QUANTITY_OPEN").DisplayIndex = 11
    '        dgv.Columns("QUANTITY_IN_QUEUE").DisplayIndex = 12
    '        dgv.Columns("HOURS_OPEN").DisplayIndex = 13
    '        dgv.Columns("lastComment").DisplayIndex = 14


    '    End If
    'End Sub

    ''' <summary>
    ''' Subroutine which formats the grids conditionally.
    ''' </summary>
    ''' <param name="dgv">The datagrid</param>
    ''' <param name="entityName"></param>
    ''' <remarks>This is the place where the business rules are placed and the results are displayed in a colour. Red is always something wrong. Green means that you can act positively like release the job.</remarks>
    Private Sub conditionalFormatGrids(ByRef dgv As DataGridView, ByVal entityName As String)

        'Draw every second row
        Dim lngSecondRow As Long = 0
        For i As Integer = 0 To dgv.Rows.Count - 1
            lngSecondRow += 1
            If lngSecondRow = 2 Then
                lngSecondRow = 0
                dgv.Rows(i).DefaultCellStyle.BackColor = Color.Beige
            End If

        Next

        If entityName = "SalesOrderLinesView" Then

            For i As Integer = 0 To dgv.Rows.Count - 1
                For j As Integer = 0 To dgv.Columns.Count - 1
                    'Make unreleased jobs green, when there are no lower level shortages any more
                    If dgv.Columns(j).Name.Equals("JOB_PO_STATUS") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value = "U" AndAlso DBNull.Value.Equals(dgv.Rows(i).Cells("NO_OF_PROCESSES").Value) Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Green
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkGreen
                            End If
                        End If
                    End If



                    'Past due schedule ship date
                    If dgv.Columns(j).Name.Equals("SCHEDULE_SHIP_DATE") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value < Now Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            Else
                                dgv.Rows(i).Cells(j).Style.BackColor = Nothing
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            End If
                        End If
                    End If

                    'Past due request date
                    If dgv.Columns(j).Name.Equals("REQUEST_DATE") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value < Now Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            Else
                                dgv.Rows(i).Cells(j).Style.BackColor = Nothing
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Nothing
                            End If
                        End If
                    End If


                    'Order complete for partial shipment at this date - included projected quantity
                    If dgv.Columns(j).Name.Equals("QTY_ONHAND") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) AndAlso Not DBNull.Value.Equals(dgv.Rows(i).Cells("checkForPartialShipment").Value) Then
                            If CInt(dgv.Rows(i).Cells(j).Value) > 0 AndAlso dgv.Rows(i).Cells("checkForPartialShipment").Value = "Y" Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Yellow
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkMagenta
                            End If
                        End If
                    End If


                    'Possible shipment of the top assembly item, refer to positive projected quantity on hand
                    If dgv.Columns(j).Name.Equals("QTY_ONHAND") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) AndAlso Not DBNull.Value.Equals(dgv.Rows(i).Cells("orderCompleteForShipping").Value) Then
                            If CInt(dgv.Rows(i).Cells(j).Value) > 0 AndAlso dgv.Rows(i).Cells("orderCompleteForShipping").Value = "Y" Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Green
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkGreen
                            End If
                        End If
                    End If

                    'Round the extended price to two digits
                    If dgv.Columns(j).Name.Equals("EXTENDED_PRICE") Then
                        Dim value As Double
                        value = Math.Round(dgv.Rows(i).Cells(j).Value, 2)
                        dgv.Rows(i).Cells(j).Value = value
                    End If

                Next
            Next

        ElseIf entityName = "MRPShortagesView" Or entityName = "supplyDemandMRPView" Then

            For i As Integer = 0 To dgv.Rows.Count - 1
                For j As Integer = 0 To dgv.Columns.Count - 1

                    'level number > 3
                    If dgv.Columns(j).Name.Equals("LEVEL_NO") Then

                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If CInt(dgv.Rows(i).Cells(j).Value) > 3 Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            End If
                        End If

                    End If

                    'past due start date
                    If dgv.Columns(j).Name.Equals("START_DATE") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value < Now Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            End If
                        End If
                    End If

                    'past due mrp date
                    If dgv.Columns(j).Name.Equals("MRP_DATE") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value < Now Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            End If
                        End If
                    End If

                    'reason code in the field
                    If dgv.Columns(j).Name.Equals("REASON_CODE") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                            dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                        End If
                    End If

                    If dgv.Columns(j).Name.Equals("DELAY") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            Dim value As Double
                            value = Math.Round(dgv.Rows(i).Cells(j).Value, 0)
                            dgv.Rows(i).Cells(j).Value = value

                        End If
                    End If
                Next
            Next

        End If

        'distinct between po and wip
        If entityName = "poQueueView" Then

            For i As Integer = 0 To dgv.Rows.Count - 1
                For j As Integer = 0 To dgv.Columns.Count - 1


                    'past due need by date
                    If dgv.Columns(j).Name.Equals("NEED_BY_DATE") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value < Now Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            Else
                                dgv.Rows(i).Cells(j).Style.BackColor = Nothing
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Nothing
                            End If
                        End If
                    End If

                    'past due promised date
                    If dgv.Columns(j).Name.Equals("PROMISED_DATE") Then
                        ' dgv.Rows(i).Cells(j).Style.BackColor = Color.MintCream
                        'dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.MintCream

                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value < Now Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            Else
                                dgv.Rows(i).Cells(j).Style.BackColor = Nothing
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Nothing
                            End If
                        Else
                           
                        End If

                        'promised date past need_by_date ***second red view!
                        'If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) AndAlso Not DBNull.Value.Equals(dgv.Rows(i).Cells("NEED_BY_DATE").Value) Then
                        '    If dgv.Rows(i).Cells(j).Value > dgv.Rows(i).Cells("NEED_BY_DATE").Value Then
                        '        dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                        '        dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                        '    Else
                        '        'dgv.Rows(i).Cells(j).Style.BackColor = Nothing
                        '        'dgv.Rows(i).Cells(j).Style.SelectionBackColor = Nothing
                        '    End If
                        'End If

                        'promised date past mrp_date?

                    End If

                    'old purchase order
                    If dgv.Columns(j).Name.Equals("AGE") Then


                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If CInt(dgv.Rows(i).Cells(j).Value) > 100 Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Yellow
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.Orange
                            Else
                                dgv.Rows(i).Cells(j).Style.BackColor = Nothing
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Nothing
                            End If
                        End If

                    End If

                    'format the age column
                    If dgv.Columns(j).Name.Equals("AGE") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            Dim k As Integer = CInt(dgv.Rows(i).Cells(j).Value)
                            dgv.Rows(i).Cells(j).Value = k

                        End If

                    End If


                Next

            Next

        ElseIf entityName = "wipQueueView" Then


            For i As Integer = 0 To dgv.Rows.Count - 1
                For j As Integer = 0 To dgv.Columns.Count - 1


                    'Unreleased jobs will be set to green, when there are no lower level shortages
                    If dgv.Columns(j).Name.Equals("MEANING") Then
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value = "Unreleased Available" Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Green
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkGreen
                            End If
                        End If
                    End If



                    If dgv.Columns(j).Name.Equals("HOURS_OPEN") Then
                        'large job
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If CInt(dgv.Rows(i).Cells(j).Value) > 10 Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Yellow
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.Orange
                            End If
                            Dim d As Double
                            d = dgv.Rows(i).Cells(j).Value
                            d = Math.Round(d, 2)
                            dgv.Rows(i).Cells(j).Value = d
                        End If

                    End If

                    If dgv.Columns(j).Name.Equals("FIRST_UNIT_START_DATE") Then
                        'large job
                        If Not DBNull.Value.Equals(dgv.Rows(i).Cells(j).Value) Then
                            If dgv.Rows(i).Cells(j).Value < Now Then
                                dgv.Rows(i).Cells(j).Style.BackColor = Color.Red
                                dgv.Rows(i).Cells(j).Style.SelectionBackColor = Color.DarkRed
                            End If
                        End If

                    End If


                Next
            Next





        End If

    End Sub




    ''' <summary>
    ''' Reorders the columns and adapts the size of the columns to the saved values.
    ''' </summary>
    ''' <param name="dgv">The data grid view we want to reorder.</param>
    ''' <param name="entityName">The entity of the data grid view.</param>
    ''' <remarks>Has to take care about the visibility of objects. Reordering fires the event of reorder the columns
    ''' and this forces us to distinct to real user interaction to enable saving the new user order.</remarks>
    Public Sub column_reordering(ByRef dgv As DataGridView, ByVal entityName As String)
        Me.bColumnReordering = True
        ' Dim db As New DB.ServerDB(My.Settings("FlowConnectionString").ToString)
        Dim dsOrder As New DataSet
        dsOrder = dbS.SecureQueryParams("SELECT [database_column], [column_order] FROM [whColumnUserVisibleView] " &
                             " WHERE entity_name = @1 AND [user_name] = @2 ORDER BY [column_order] ASC", entityName, Environment.UserName.ToLower)

        'Deployment process:    1. Check template of column order in the backend. 
        '                       2. Forward template information to user values
        '                       3. Enable possibility to query the backend and 

        'For k As Integer = 0 To dsOrder.Tables(0).Rows.Count - 1

        '    If dgv.Columns(dsOrder.Tables(0).Rows(k).Item("database_column").ToString).Index > 0 Then

        '    End If
        '    dgv.Columns(dsOrder.Tables(0).Rows(k).Item("database_column").ToString).DisplayIndex = dsOrder.Tables(0).Rows(k).Item("column_order")

        'Next
        Try
            'Process is to iterate over the selected values of the dataset and reorder the column to that point
            For i As Integer = 0 To dsColumnsSettting.Tables(0).Rows.Count - 1
                '*** requires testing
                'Will fail if the column does not exist
                Try
                    If dsColumnsSettting.Tables(0).Rows(i).Item("entity_name").ToString.Trim.ToLower = entityName.Trim.ToLower Then
                        'dgv.Columns(dsColumnsSettting.Tables(0).Rows(i).Item("database_column")).DisplayIndex = dsColumnsSettting.Tables(0).Rows(i).Item("column_order")
                    End If

                Catch ex As Exception

                End Try

            Next
        Catch ex As Exception

        End Try


        Me.bColumnReordering = False
    End Sub

    ''' <summary>
    ''' Return true if this object is reordering the columns.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property propReorderingColumns() As Boolean
        Get
            Return bColumnReordering
        End Get
    End Property

    ''' <summary>
    ''' Apply the sizes of the columns to the grid.
    ''' </summary>
    ''' <param name="dgv">The grid where the new size should be applied to.</param>
    ''' <param name="entityName">The entity name behind the grid view.</param>
    ''' <remarks></remarks>
    Public Sub column_size(ByRef dgv As DataGridView, ByVal entityName As String)
        'Dim db As New DB.ServerDB(My.Settings("FlowConnectionString").ToString)
        Dim dsSize As New DataSet
        'Dim dvSize As DataView
        'dsSize = db.SecureQueryParams("SELECT [database_column], [column_size] FROM [whColumnUserVisibleView] " & _
        '                     " WHERE entity_name = @1 AND [user_name] = @2", entityName, Environment.UserName.ToLower)
        'dvSize = New DataView(dsColumnsSettting.Tables(0), "entity_name =  '" & entityName & "'", "database_column", DataViewRowState.CurrentRows)

        For i As Integer = 0 To dsColumnsSettting.Tables(0).Rows.Count - 1
            If dsColumnsSettting.Tables(0).Rows(i).Item("entity_name").ToString.Trim.ToLower = entityName.Trim.ToLower Then
                For h As Integer = 0 To dgv.Columns.Count - 1
                    If dgv.Columns(h).Name = dsColumnsSettting.Tables(0).Rows(i).Item("database_column").ToString Then
                        dgv.Columns(h).Width = dsColumnsSettting.Tables(0).Rows(i).Item("column_size")
                    End If
                Next
            End If

        Next

    End Sub


    ''' <summary>
    ''' Handles the default column visibility.
    ''' </summary>
    ''' <param name="dgv">The data grid view to be adapted.</param>
    ''' <param name="entityName">The entity name behind it.</param>
    ''' <remarks></remarks>
    Private Sub visibility(ByRef dgv As DataGridView, ByVal entityName As String)
        'Dim db As New DB.ServerDB(My.Settings("FlowConnectionString").ToString)
        Dim dsVisible As DataSet
        'Dim dvVisible As DataView

        'dsVisible = dbS.SecureQueryParams("SELECT database_column, user_visibility FROM [warehouse_grid_columns] " & _
        '                     " WHERE entity_name = @1", entityName)


        'dvVisible = New DataView(dsColumnsSettting.Tables(0), "entity_name = '" & entityName & " '", "database_column ", DataViewRowState.CurrentRows)

        'Loading the standard values
        Dim oldHeader As String
        For h As Integer = 0 To dgv.Columns.Count - 1

            oldHeader = dgv.Columns(h).HeaderText

            For g As Integer = 0 To dsColumnsSettting.Tables(0).Rows.Count - 1

                If oldHeader = dsColumnsSettting.Tables(0).Rows(g).Item("database_column").ToString.Trim AndAlso dsColumnsSettting.Tables(0).Rows(g).Item("entity_name").ToString.ToLower = entityName.ToLower Then
                    dgv.Columns(h).Visible = Boolean.Parse(dsColumnsSettting.Tables(0).Rows(g).Item("visibility").ToString)
                End If
            Next

        Next


        'Loading the individual, customn values

        'dsVisible = dbS.SecureQueryParams("SELECT [database_column], visibility FROM [whColumnUserVisibleView] " & _
        '                     " WHERE entity_name = @1 AND [user_name] = @2", entityName, Environment.UserName.ToLower)

        'For h As Integer = 0 To dgv.Columns.Count - 1

        '    oldHeader = dgv.Columns(h).HeaderText

        '    For g As Integer = 0 To dsVisible.Tables(0).Rows.Count - 1

        '        If oldHeader = dsVisible.Tables(0).Rows(g).Item("database_column").ToString.Trim Then
        '            If dgv.Columns(h).Visible Then
        '                dgv.Columns(h).Visible = Boolean.Parse(dsVisible.Tables(0).Rows(g).Item("visibility").ToString)
        '            End If

        '        End If
        '    Next

        'Next

    End Sub

    ''' <summary>
    ''' Handles the renaming of the headers.
    ''' </summary>
    ''' <param name="dgv">The data grid view which should be renamed.</param>
    ''' <param name="entityName">The underlying entity</param>
    ''' <remarks>Downloads the information from the database.</remarks>
    Private Sub rename_Headers(ByRef dgv As DataGridView, ByVal entityName As String, Optional ByVal organizationId As Integer = 0)
        ' Dim db As New DB.ServerDB(My.Settings("FlowConnectionString").ToString)
        Dim ds As New DataSet
        Dim dvHeaders As DataView
        'ds = db.SecureQueryParams("SELECT language FROM [warehouse_users] WHERE [user_name] = @1", Environment.UserName.ToLower)

        'If language is set to standard look into the definition table
        'If not go to the translation table
        '-------------------------------------------------- Adapt headers


        'dsHeaders = db.SecureQueryParams("SELECT database_column, column_header FROM [warehouse_grid_columns] " & _
        '                                 " WHERE [entity_name] = @1", entityName)
        dvHeaders = New DataView(dsColumnsSettting.Tables(0), " entity_name = '" & entityName & "'", "database_column", DataViewRowState.CurrentRows)

        Dim c As New Integer
        Dim oldHeader As String

        'Iterate over the columns and exchange the headers
        For h As Integer = 0 To dgv.Columns.Count - 1
            ' c = dgv.Columns.IndexOf(dgv.Columns(dsHeaders.Tables(0).Rows(h).Item("database_column").ToString))

            'Change to datetime values to short date time
            If dgv.Columns(h).ValueType.ToString = "System.DateTime" Then
                'MessageBox.Show("Hallo")
                dgv.Columns(h).DefaultCellStyle.Format = "d"
            End If
            'c = dgv.Columns.IndexOf(dgv.Columns("ORDER_NO"))
            oldHeader = dgv.Columns(h).HeaderText
            oldHeader = dgv.Columns(h).Name
            Dim testMe As Integer = 0
            For g As Integer = 0 To dvHeaders.Table.Rows.Count - 1
                If oldHeader = dvHeaders.Table.Rows(g).Item("database_column").ToString.Trim AndAlso dsColumnsSettting.Tables(0).Rows(g).Item("entity_name").ToString.ToLower = entityName.ToLower Then
                    dgv.Columns(h).HeaderText = dvHeaders.Table.Rows(g).Item("column_header").ToString.Trim
                End If

                If oldHeader = dvHeaders.Table.Rows(g).Item("database_column").ToString.Trim Then
                    testMe += 1
                End If
            Next
        Next

        '*** Implement the translation possibility
        'dsHeaders = db.SecureQueryParams("SELECT column_name, translation FROM [warehouse_grid_column_translations] " & _
        '                                 " WHERE [entity_name] = @1 AND [language] = @2", entityName, ds.Tables(0).Rows(0).Item(0).ToString)

        'Dim c As New Integer
        'Dim oldHeader As String

        ''Iterate over the columns and exchange the headers
        'For h As Integer = 0 To dsHeaders.Tables(0).Rows.Count - 1
        '    ' c = dgv.Columns.IndexOf(dgv.Columns(dsHeaders.Tables(0).Rows(h).Item("database_column").ToString))

        '    'c = dgv.Columns.IndexOf(dgv.Columns("ORDER_NO"))
        '    oldHeader = dgv.Columns(h).HeaderText
        '    For g As Integer = 0 To dsHeaders.Tables(0).Rows.Count - 1
        '        If oldHeader = dsHeaders.Tables(0).Rows(g).Item("column_name").ToString.Trim Then
        '            dgv.Columns(h).HeaderText = dsHeaders.Tables(0).Rows(g).Item("translation").ToString.Trim
        '        End If
        '    Next

        'Next


        If organizationId = 881 AndAlso entityName = "SalesOrderLinesView" Then
            dgv.Columns("lastForecast").HeaderText = "Build Date"
        ElseIf entityName = "SalesOrderLinesView" Then
            dgv.Columns("lastForecast").HeaderText = "Last Forecast"
        End If

    End Sub

    ''' <summary>
    ''' Property which is indicating if the object is reloaded from the database 
    ''' </summary>
    ''' <value>Boolean value to set true of the obkect reloads the width from the backend</value>
    ''' <returns>True if the values are loaded from the backend.</returns>
    ''' <remarks>Required to enable saving the new column widt changes of the users.</remarks>
    Public ReadOnly Property reloading() As Boolean
        Get
            Return Me.bReloading
        End Get
   
    End Property

    ''' <summary>
    ''' Loads the settings (order, etc for the grids) from the backend.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub loadColumnSettings()
        'Dim db As New DB.ServerDB(My.Settings("FlowConnectionString").ToString)
        Try
            'db.Connect()
            '*** According to the user language we have to pick the right translation for the columns.
            userLanguage = dbS.SecureQueryParams("SELECT language FROM [warehouse_users] WHERE [user_name] = @1", Environment.UserName.ToLower).Tables(0).Rows(0).Item(0)

            'here we need to implement multilanguage
            If userLanguage = "Standard" Then
                dsColumnsSettting = dbS.SecureQueryParams("SELECT entity_name, [database_column], [column_header], [column_order], column_size, user_visibility, visibility FROM [whColumnUserVisibleView] " & _
                                 " WHERE  [user_name] = @1 ORDER BY entity_name, [column_order] ASC", Environment.UserName.ToLower)
            Else
                dsColumnsSettting = dbS.SecureQueryParams("SELECT entity_name, [database_column], translation as column_header, [column_order], column_size, user_visibility, visibility FROM [whColumnUserVisibleViewTL] " & _
                                                " WHERE  [user_name] = @1 AND language = @2 ORDER BY entity_name, [column_order] ASC", Environment.UserName.ToLower, userLanguage)
            End If
            


        Catch ex As Exception
        Finally
            '  db.Disconnect()
        End Try


    End Sub
    ''' <summary>
    ''' To hide the columns for EDC instance
    ''' </summary>
    ''' <param name="organizationId">Organization ID</param>
    ''' <remarks></remarks>
    Public Sub hideColumnsforEDC(ByVal organizationId As Integer)

        Dim ds As New DataSet
        Dim lngTest As Integer

        Try
            dbS.Connect()
            ds = dbS.SecureQueryParams("SELECT currentInstance FROM dbo.whOrganizationDefinition WHERE organizationId = @1", organizationId)

            If ds.Tables(0).Rows(0).Item(0).ToString() = "EDC PROD" Then
                lngTest = dbS.SecureNonQueryParams("UPDATE whColumnUserVisibleView SET visibility = 0 WHERE entity_name = 'salesOrderLinesView' and user_name = @1" & _
                                                   " and database_column in ('FSG_SPARES_CLASS', 'FSG_IND_PAINT_PARTS', 'ATTRIBUTE5', 'ATTRIBUTE6', 'ATTRIBUTE16', 'ATTRIBUTE17', " & _
                                                   "'ATTRIBUTE18', 'ATTRIBUTE19', 'ATTRIBUTE20', 'UNIT_ACCESSORIES_VALUE', 'TRADING_ACCT', 'SHIP_SET_ID', 'NAME')", Environment.UserName)
                lngTest = dbS.SecureNonQueryParams("UPDATE whColumnUserVisibleViewTL SET visibility = 0 WHERE entity_name = 'salesOrderLinesView' and user_name = @1" & _
                                                   " and database_column in ('FSG_SPARES_CLASS', 'FSG_IND_PAINT_PARTS', 'ATTRIBUTE5', 'ATTRIBUTE6', 'ATTRIBUTE16', 'ATTRIBUTE17', " & _
                                                   "'ATTRIBUTE18', 'ATTRIBUTE19', 'ATTRIBUTE20', 'UNIT_ACCESSORIES_VALUE', 'TRADING_ACCT', 'SHIP_SET_ID', 'NAME')", Environment.UserName)
            Else
                lngTest = dbS.SecureNonQueryParams("UPDATE whColumnUserVisibleView SET visibility = user_visibility WHERE entity_name = 'salesOrderLinesView' and user_name = @1", Environment.UserName)
                lngTest = dbS.SecureNonQueryParams("UPDATE whColumnUserVisibleViewTL SET visibility = user_visibility WHERE entity_name = 'salesOrderLinesView' and user_name = @1", Environment.UserName)
            End If
            Me.loadColumnSettings()
        Catch ex As Exception
        Finally
            dbS.Disconnect()
        End Try

    End Sub
End Class
