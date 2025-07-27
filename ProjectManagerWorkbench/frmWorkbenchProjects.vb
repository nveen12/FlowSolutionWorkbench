
'Imports DataGridViewAutoFilter
Imports System.Timers.Timer
Imports DataGridViewAutoFilter

Public Class frmWorkbenchProjects


    ''' <summary>
    ''' Set of variable for the life cycle of the object.
    ''' </summary>
    ''' <remarks>The data sets are queried once and the filtered to views upon them. Operating Unit and Oraganization Id can change
    '''  over the time.</remarks>
    Private dsSalesOrder As New DataSet         'Dataset for the sales orders
    Private dsSalesOrderGrouped As New DataSet  'Dataset for the shortages
    Private dsMRPShortages As New DataSet
    Private dsWIP As New DataSet
    Private dsPO As New DataSet
    'Private db As New DB.ServerDB(My.Settings("FlowConnectionString"))

    Private organizationId As Double = 0
    Private organizationName As String
    Private organizationLanguage As String = ""
    Private operatingUnitId As Double = 0
    Public gridDisplay As New clsGridDisplay

    Private loadFirstTime As Boolean = True
    Private lngCurrentNotification As Long = 0

    Private searchString As String
    Private bottonGrid As String = ""
    Private strMiddleGrid As String = ""
    Private lngNumberOfHits As Integer
    Private lngCurrentNumberOfHit As Integer

    Private searchGrid As String = ""
    Private row As Integer = -1
    Private column As Integer = -1

    Private timer As New Timer
    Private lastMouseMoveEvent As New DateTime

    Private timerMessage As New Timer
    Private lngCountTwinkle As New Long
    Private bReource As Boolean

    Private strFilterSalesOrder As String = ""
    Private strFilterSalesOrderName As String = ""
    Private strFilterPO As String = ""
    Private strFilterWip As String = ""

    'temporary filterStrings to be added for load
    Private strFilterSalesOrderTemp As String = ""
    Private strFilterPOTemp As String = ""
    Private strFilterWIPTemp As String = ""
    Private strFilterMRPShortagesTemp As String = ""

    'Data to repopulate the shortages with a filter
    Private lnDemandId As Long
    Private strSupplDemandItem As String            'Item for the supply demand view


    'Variables to determine the filter type clicked
    Private strFilterEntity As String = ""
    Private strColumnName As String
    Private strColumnHeader As String

    'Variables which contain the total number of records in the grid to enable Custom Filter Applied  34 / 4300 records displayed 
    Private lngCountSalesOrderLines As Long
    Private lngCountPurchaseOrderLines As Long
    Private lngCountWIPDiscreteJobsOperationSequences As Long

    'Store the current selected role id in the form environment
    Private lngRoleID As Long
    Private objRoleSpecificControls(100) As Object

    Private searchPoList As New List(Of poSearch)
    Private seachPOListPOLine As New List(Of poSearch)
    Private searchJobList As New List(Of jobSearch)

    'Store the update time from the server in order to check for a fresh compile
    Private updateTime As Date
    ' Variable to check if connected via VPN
    Private IsVPNActive As Boolean = False
    'Variable to check CTRL key pressed while selecting comments
    Private IsCtrlKeyPressed As Boolean = False
    Private idleTimebyUser As Int32 = 0

    Private planningType As String = "MRP"
    Private supplyDemandMiddleGrid As String = "supplyDemandMRPView"
    Private poBottonGrid As String = "poQueueView"
    Private wipBottonGrid As String = "wipQueueView"


    'Private currentPlanningType As String
    Public ReadOnly Property currentPlanningType() As String
        Get
            Return planningType
        End Get
    End Property

    Public ReadOnly Property currentorganizationID() As Double
        Get
            Return organizationId
        End Get
    End Property

    Public ReadOnly Property roleSpecificControls()
        Get
            Return objRoleSpecificControls
        End Get
    End Property

    ''' <summary>
    ''' Handles the load event of the program. Mainly timers are initiated and basic org is loaded to the user.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub frmWorkbenchProjects_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim frm As New frmLoading
        Try
            
            frm.Show()
            frm.Update()

            'Me.deleteTemporaryFilters(False, False, False, True)

            objRoleSpecificControls(0) = SafetyStockCalculationToolStripMenuItem
            objRoleSpecificControls(1) = NAFTACheckToolStripMenuItem
            objRoleSpecificControls(2) = ExportCheckToolStripMenuItem
            objRoleSpecificControls(3) = CreatePickReleaseToolStripMenuItem
            objRoleSpecificControls(4) = IndicatorTrackingToolStripMenuItem
            objRoleSpecificControls(5) = ViewToolStripMenuItem
            objRoleSpecificControls(6) = YourActualInvolvementToolStripMenuItem
            objRoleSpecificControls(7) = SellToolStripMenuItem
            objRoleSpecificControls(8) = BuyToolStripMenuItem
            objRoleSpecificControls(9) = MakeToolStripMenuItem
            objRoleSpecificControls(10) = SaveCurrentTemporarySelectionToolStripMenuItem
            objRoleSpecificControls(11) = SetDefaultViewToolStripMenuItem

            'load setting of the user
            frm.statusBar = "Loading User Settings"
            frm.statusProgress = 20
            frm.Update()

            lastMouseMoveEvent = Now

            'Test if the user is already in the database, otherwise force injection to it
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim ds As New DataSet
            
            ds = db.SecureQueryParams("SELECT * FROM warehouse_users WHERE user_Name = @1", Environment.UserName)
            Try
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim newUser As New clsSetupOperations(Environment.UserName)
                    ds = db.SecureQueryParams("SELECT * FROM warehouse_users WHERE user_Name = @1", Environment.UserName)
                Else
                    ' Imports all new columns from the template table to the user specific table.
                    Dim objClsSetupOperations As New clsSetupOperations
                    objClsSetupOperations.import_grid_column_for_sizing(Environment.UserName)
                    ' Update first name, last name and email if empty or null
                    If ds.Tables(0).Rows(0).Item("firstName").ToString() = String.Empty Or ds.Tables(0).Rows(0).Item("lastName").ToString() = String.Empty Or ds.Tables(0).Rows(0).Item("eMail").ToString() = String.Empty Then

                        '*** Here we need to implement getting the properties from Active Directory

                        'objClsSetupOperations.updateUserDetails(Environment.UserName)
                    End If
                    'Check out of office
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("OutOfOffice")) AndAlso ds.Tables(0).Rows(0).Item("OutOfOffice") AndAlso ds.Tables(0).Rows(0).Item("outFromDate") < Today.AddDays(1) Then
                        Dim dlg As New dlgOutOfOfficeTime
                        dlg.ShowDialog()
                        frm.Update()
                    End If
                    'Translate the form for the user
                    Dim userTranslation As New clsUserControl()
                    userTranslation.translateForms(Me, ds.Tables(0).Rows(0).Item("language"))
                End If
            Catch ex As Exception

            End Try

            'Apply the user rights
            frm.statusBar = "Loading User Roles and Rights"
            frm.statusProgress = 40
            frm.Update()

            'Apply the user rights to the form:
            'Get the role id first
            ds = db.SecureQueryParams("SELECT warehouse_users.lastRoleSelected, whUserRoles.roleName FROM warehouse_users INNER JOIN whUserRoles ON whUserRoles.roleId = warehouse_users.lastRoleSelected AND warehouse_users.user_name = @1", Environment.UserName)
            Try
                Dim userRights As New clsUserControl(ds.Tables(0).Rows(0).Item(0))
                userRights.applyAllUserRights(Me)
                lblCurrentRole.Text = "Login: " & ds.Tables(0).Rows(0).Item("roleName")
                lblCurrentRole.Visible = True

                If userRights.userInRole(Environment.UserName, "Admin user") Then
                    ManageUsersToolStripMenuItem.Visible = True
                End If

            Catch ex As Exception

            End Try

            'Message timer - makes the icon twinkle
            timerMessage.Interval = 250
            AddHandler timerMessage.Tick, AddressOf ControlTwinkleIcon

            timer.Interval = 30000
            timer.Start()
            AddHandler timer.Tick, AddressOf ControlOperation

            'Apply the user rights
            frm.statusBar = "Loading Rows and Values"
            frm.statusProgress = 45
            frm.Update()

            loadWorkbench(frm)

            'Site specific right click menu's
            'If Me.organizationId = 255 Then
            'Dim onClickHandler As System.EventHandler = New System.EventHandler(AddressOf ToolStripMenuCountrOrigin_Click)
            'cmsSalesOrderLines.Items.Add("Country of Origin Check", Nothing, onClickHandler)
            'End If


            Dim createLogin As New clsSetupOperations
            createLogin.insertLoginStamp()

            'Setup of the tool tip
            ttHoverActions.AutomaticDelay = 5
            ttHoverActions.InitialDelay = 4
            ttHoverActions.UseFading = False
            ttHoverActions.UseAnimation = False

            'Find out if we are on a testmachine
            If Not My.Settings.FLOWConnectionString.Contains(My.Settings.prodMachineRecognizeName) Then
                If Not My.Settings.FLOWConnectionString = My.Settings.prodMachineRecognizeName Then         'It can also equal a production server
                    Me.BackColor = Color.Red
                    Me.ForeColor = Color.Black
                End If
            End If

            'Apply the user rights
            frm.statusBar = "Loading All Settings"
            frm.statusProgress = 100
            frm.Update()

            Me.loadSettingsOfWorkbench()

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("frmWorkbenchProjects_Load", ex, 1)
        Finally
            ' frm.Close()
        End Try
       
    End Sub
    Sub ToolStripMenuCountrOrigin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim dlg As New dlgTwoGridsDisplay(dgvSalesOrders.SelectedRows(0).Cells("organization_id").Value, dgvSalesOrders.SelectedRows(0).Cells("header_id").Value, 5, 2, dgvSalesOrders.SelectedRows(0).Cells("order_no").Value, Me.organizationName)
        dlg.ShowDialog()

    End Sub


    ''' <summary>
    ''' Subprocedure which loads the different types of data like the grids, Organization information, notifications, Indicators on the entities.
    ''' </summary>
    ''' <remarks>Is just used for the first time instance load.</remarks>
    Private Sub loadWorkbench(ByRef loadWindow As frmLoading)

        'Apply the user rights
        loadWindow.statusBar = "Populate the grids"
        loadWindow.statusProgress = 50
        loadWindow.Update()

        Me.populateGrids(loadWindow)
        'Debug.Print(Now)

        'Apply the user rights
        loadWindow.statusBar = "Load Organizations"
        loadWindow.statusProgress = 85
        loadWindow.Update()

        Me.loadOrganizations()
        Me.loadUserViews()
        Me.loadCurrentNotifications()
        'Debug.Print(Now)

        'Apply the user rights
        loadWindow.statusBar = "Accelerate Grids and Load Indicators"
        loadWindow.statusProgress = 90
        loadWindow.Update()

        Me.accelerateGrids()
        Me.calculateIndicators()
        'Debug.Print(Now)
        Me.loadFirstTime = False
        'Debug.Print(Now)

    End Sub

    Private bAskUserForUpdate As Boolean = False
    Private Sub checkForNewUpdate()
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.SecureQueryParams("SELECT transformationRun FROM whOrganizationDefinition WHERE organizationId = @1", Me.organizationId)

        If Me.updateTime < ds.Tables(0).Rows(0).Item("transformationRun") Then
            'Find if we have an instance of this dialogue already
            If bAskUserForUpdate Then
                Exit Sub
            End If

            Dim dlgAction As New dlgConfirmAction("Autobahn downloads new information", "New information available and loaded to your screen.")
            bAskUserForUpdate = True
            dlgAction.Focus()
            dlgAction.BringToFront()

            dlgAction.Show()
            Me.loadComleteWorkbench(Me.organizationId)

            bAskUserForUpdate = False
        End If

    End Sub


    ''' <summary>
    ''' Controls the reload of notifications.
    ''' </summary>
    ''' <remarks>Currently the notifications are reloaded every 30 seconds and the whole data is refreshed every 5 minutes.</remarks>


    Private Sub ControlOperation(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim d As New TimeSpan
        d = Now() - lastMouseMoveEvent
        Dim notesLoad As TimeSpan = Now - notificationsLastLoad
        Try
            'Reload the notifications and let them twinkle after three minutes
            If notesLoad.Minutes > 3 Then
                loadCurrentNotifications()
                checkForNewUpdate()
            End If

            'After 15 minutes of idle time and when the user returns to the computer refresh the grid
            'Test if the user wants this to do, does not have a dialogue open
            '*** Enable a personal setting to avoid this and just go with the fresh snapshot
            'How can we deliver the new notifications correctly? This here makes the user wait when he comes back and wants to use it.

            'If idleTimebyUser > 15 And d.Minutes <= 1 Then
            '    'Me.deleteTemporaryFilters(True, True, True, True)
            '    'Restoring the first displayed row after automatic refresh
            '    Dim topRowIndex As Integer = dgvSalesOrders.FirstDisplayedScrollingRowIndex
            '    Dim bottonRowIndex As Integer = dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex

            '    loadSalesOrder(strFilterSalesOrder & strFilterSalesOrderTemp)
            '    '***crashes when no rows displayed
            '    If dgvSalesOrders.RowCount > 0 Then
            '        dgvSalesOrders.FirstDisplayedScrollingRowIndex = topRowIndex
            '    End If

            '    If Me.bottonGrid = "wip" Then
            '        loadWip(strFilterWip & strFilterWIPTemp)
            '        If dgvWipSupplierQueue.RowCount > 0 Then
            '            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = bottonRowIndex
            '        End If

            '    Else
            '        loadPO(strFilterPO & strFilterPOTemp)
            '        If dgvWipSupplierQueue.RowCount > 0 Then
            '            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = bottonRowIndex
            '        End If

            '    End If
            '    lastMouseMoveEvent = Now
            'End If

            ' loadCurrentNotifications()
            checkNewTransactions()
            idleTimebyUser = d.Minutes
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("ControlOperation", ex, 1)
        End Try
    End Sub

    ''' <summary>
    ''' Procedure which accelerates the four grids of the form.
    ''' </summary>
    ''' <remarks>Also enables the user to copy over STR + C to the clibboard inclusive header information.</remarks>
    Private Sub accelerateGrids()

        gridDisplay.DoubleBuffered(dgvFurtherDetails, True)
        gridDisplay.DoubleBuffered(dgvSalesOrders, True)
        gridDisplay.DoubleBuffered(dgvShortages, True)
        gridDisplay.DoubleBuffered(dgvWipSupplierQueue, True)

        Me.dgvSalesOrders.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText 'make it so user can copy text from the data grid
        Me.dgvWipSupplierQueue.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Me.dgvShortages.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText

    End Sub

    ''' <summary>
    ''' Loads the possible organizations to the dropdownlist.
    ''' </summary>
    ''' <remarks>May be changed to just accessible organizations.</remarks>
    Private Sub loadOrganizations()
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        Dim dsOrgs As New DataSet

        'Fill the tool strip combo box with different possible organizations
        dsOrgs = db.Query("SELECT organizationId, organizationName FROM whorganizationDefinition WHERE organizationName IS NOT NULL ORDER BY organizationName")
        cboOrganization.DataSource = dsOrgs.Tables(0)
        cboOrganization.DisplayMember = "organizationName"
        cboOrganization.ValueMember = "organizationId"

        For i As Integer = 0 To dsOrgs.Tables(0).Rows.Count - 1
            If Me.organizationName = dsOrgs.Tables(0).Rows(i).Item("organizationName") Then
                cboOrganization.SelectedIndex = i
            End If
        Next

    End Sub

    ''' <summary>
    ''' Procedure which loads the data into the grids and prepared the wip dataset in the background.
    ''' </summary>
    ''' <remarks>There can only be one botton grid and this is the reason why it only can have one botton grid.</remarks>
    Private Sub populateGrids(Optional ByRef loadWindow As frmLoading = Nothing)

        lblSalesOrderHeader.Text = "Open Sales Orders - No Filters"
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet
        'Load the organization id from the template organization
        Try
            'If organizationId = 0 Then

            If Me.loadFirstTime Then
                ds = db.SecureQueryParams("SELECT userDefaultOrganization, organizationLanguage, planningType FROM [whUserOrganizationsView] WHERE [user_Name] = @1", Environment.UserName)
                organizationId = ds.Tables(0).Rows(0).Item(0)
                organizationLanguage = ds.Tables(0).Rows(0).Item("organizationLanguage").ToString

                If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("planningType")) Then
                    Me.planningType = ds.Tables(0).Rows(0).Item("planningType")
                Else
                    Me.planningType = "MRP"
                End If


                'Hide few columns of Sales Order View, if the current instance is EDC 
                If organizationId <> 0 Then
                    gridDisplay.hideColumnsforEDC(organizationId)
                End If
            Else
                ds = db.SecureQueryParams("SELECT standard_organization_id FROM [warehouse_users] WHERE [user_Name] = @1", Environment.UserName)
                organizationId = ds.Tables(0).Rows(0).Item(0)
            End If

            'Change the name of the form, so you can see which organization we currently are looking at
            ds = db.SecureQueryParams("SELECT organizationName, currentInstance, transformationRun FROM whOrganizationDefinition WHERE organizationId = @1", organizationId)
            Me.Text = "Autobahn " & " - " & ds.Tables(0).Rows(0).Item(0).ToString & " From " & ds.Tables(0).Rows(0).Item(1).ToString & " " & ds.Tables(0).Rows(0).Item("transformationRun").ToString & " " & Me.planningType
            Me.organizationName = ds.Tables(0).Rows(0).Item(0).ToString

            'Store the transformation time
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("transformationRun")) Then
                Me.updateTime = ds.Tables(0).Rows(0).Item("transformationRun")
            Else
                Me.updateTime = Now
            End If


            'Load the operating unit id into the Variable
            ds = db.SecureQueryParams("SELECT [standard_operating_unit_id] FROM [warehouse_users] WHERE [user_Name] = @1", Environment.UserName)
            operatingUnitId = ds.Tables(0).Rows(0).Item(0)

            'Creation of the four data sources
            'Check for VPN connection active
            If Me.loadFirstTime Then
                'Just load the top 100
                'IsVPNActive = True      '*** We can have this a setting for the user.

                Dim prArr As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName("dsNetworkConnect")
                If prArr.Length >= 1 Then
                    IsVPNActive = True
                End If
                ' As connection is via VPN, load top 100 records only
                If IsVPNActive Then
                    If Not loadWindow Is Nothing Then
                        loadDataSets(loadWindow, True)
                    Else
                        loadDataSets()
                    End If

                    lblSalesOrderHeader.Text = "Open Sales Orders - Top 100 Lines"
                    If bottonGrid = "po" Then
                        lblWIPPOFilter.Text = "Open Purchase Order Lines - Top 100 Lines"
                    Else
                        lblWIPPOFilter.Text = "Open WIP Operation Sequences - Top 100 Lines"
                    End If
                    IsVPNActive = False
                Else
                    If Not loadWindow Is Nothing Then
                        loadDataSets(loadWindow, True)
                    Else
                        loadDataSets()
                    End If
                    'loadDataSets(loadWindow)
                End If
            Else
                If Not loadWindow Is Nothing Then
                    loadDataSets(loadWindow, True)
                Else
                    loadDataSets()
                End If
                ' loadDataSets(loadWindow)
            End If

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("populateSupplyDemand", ex, 1)
        End Try


    End Sub

    ''' <summary>
    ''' Subroutine which calculates the indicators displayed on the lower level of the user interface.
    ''' </summary>
    ''' <remarks>Should distinct between the different currencies.</remarks>
    Private Sub calculateIndicators()

        Try

            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim ds As New DataSet
            lblTotalOrgIndicators.Text = ""
            ds = db.SecureQueryParams("SELECT [ORDER_NO] FROM [SalesOrderLinesView] WHERE [organization_id] = @1 GROUP BY [ORDER_NO]", Me.organizationId)
            lblTotalOrgIndicators.Text += "#Sales Orders: " & ds.Tables(0).Rows.Count
            lblTotalOrgIndicators.Text += "  #SO Lines: " & lngCountSalesOrderLines

            ds = db.SecureQueryParams("SELECT SUM(ORDERED_QUANTITY*UNIT_SELLING_PRICE) " & _
                                        "	FROM [SalesOrderLinesView] WHERE [organization_id] = @1", Me.organizationId)

            If Me.organizationId = 255 Or Me.organizationId = 318 Or Me.organizationId = 2 Then
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                    'If Format(Math.Round(ds.Tables(0).Rows(0).Item(0) / 1000, 0)).ToString.Contains("€") Then
                    lblTotalOrgIndicators.Text += " SO Value:" & Format(Math.Round(ds.Tables(0).Rows(0).Item(0) / 1000, 0), "C0") & "k"
                    'Else
                    '   lblTotalOrgIndicators.Text += " SO Value:" & Format(Math.Round(ds.Tables(0).Rows(0).Item(0) * 1.28 / 1000, 0), "C0") & "k"
                    'End If
                End If
            Else
                lblTotalOrgIndicators.Text += " SO Value:" & Format(Math.Round(ds.Tables(0).Rows(0).Item(0) / 1000, 0), "C0") & "k"
            End If

        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Gets everything going on for this specific item number in supply demand.
    ''' </summary>
    ''' <param name="itemNo">The item number for which supply demand should be populated.</param>
    ''' <remarks>Asks itself for an organization id.</remarks>
    Private Sub populateSupplyDemand(ByVal itemNo As String)

        'New approach means we just have to do the last bit.
        Dim lng As Integer = 0
        Dim lng2 As Integer = 2
        'Create combined supply and demand view which really shows everything going on for the part number
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        strSupplDemandItem = itemNo
        lblDemandView.Visible = False

        'put it into the datagrid view - apply filter, and reordering later.
        Try
            strMiddleGrid = "supplyDemandMRPView"

            Me.calculateRunningNumberOnSupplyDemandTable(dsMRPShortages, dgvShortages, 0, 0, itemNo)
            'dgvShortages.DataSource = dsMRPShortages.Tables(0)
            'Me.calculateRunningNumberForSupplyDemand(dsMRPShortages, dgvShortages, True)
            'gridDisplay.formatGrid(dgvShortages, "")

            lblShortageDescription.Text = "Supply & Demand For Part: " & itemNo & ControlChars.Tab & "    "
            'Starting On Hand Quantity:" & quantityOnHand.ToString & ControlChars.Tab & _
            '"   Ending On Hand Quantity:" & runningQuantity.ToString

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("populateSupplyDemand", ex, 1, itemNo & " - " & Me.organizationId.ToString)
        End Try

    End Sub

    ''' <summary>
    ''' Calculates the running number on the supply and demand view.
    ''' </summary>
    ''' <param name="ds">The data set for which this should be calculated.</param>
    ''' <param name="dgv">The datagridview on which the data set should be displayed.</param>
    ''' <param name="calculatLikeOracle">If the procedure should automatically test which way of calculation to use.</param>
    ''' <remarks>Was designed to imitate the way Oracle calculates the supply and demand</remarks>
    Private Sub calculateRunningNumberForSupplyDemand(ByRef ds As DataSet, ByRef dgv As DataGridView, ByVal calculatLikeOracle As Boolean)
        'Be careful, items may be used also on sales order lines 
        Dim adc1 As DataColumn
        adc1 = New DataColumn("Running Total", System.Type.GetType("System.Decimal"))
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        'If the user switches to the view the column will be already there
        If ds.Tables(0).Columns.Contains("Running Total") Then
        Else
            ds.Tables(0).Columns.Add(adc1)
        End If

        dgv.EnableHeadersVisualStyles = False

        Dim quantityOnHand As Double = 0
        Dim runningQuantity As Double
        Dim dblTotalDemand As Double = 0
        Dim strPart As String = ""

        Dim strlastJobPo As String = ""
        Dim strnewJobPo As String = ""

        'When eventually coming from a lower leve correction repopulate the supply and demand view.

        'If Not calculatLikeOracle Then
        '    ds = db.SecureQueryParams("SELECT " & _
        '"  [SalesOrderLine]  " & _
        '", [ITEM_NO]  " & _
        '", [DESCRIPTION]  " & _
        '", [MAKE_BUY]  " & _
        '", [MRP_QTY]  " & _
        '", JobPOLine  " & _
        '", [MRP_DATE]  " & _
        '", [STATUS]  " & _
        '", [DEMAND_QUANTITY]  " & _
        '", [SUPPLY_QUANTITY] " & _
        '", [OP1]  " & _
        '", [OP2]  " & _
        '", [OP3]  " & _
        '", [OP4]  " & _
        '", [OP5]  " & _
        '", [SAFETY_STOCK_QUANTITY]  " & _
        '", [QTY_ONHAND]  " & _
        '", [SUPPLY_TYPE]" & _
        '", [JOB_NO]" & _
        '", [PURCHASE_ORDER_NO]" & _
        '", [PURCH_LINE_NO]" & _
        '", [ORDER_NO]" & _
        '", [LINE_NO]" & _
        '"  FROM [supplyDemandMRPView] WHERE [organization_id] = @1 AND [item_no] = @2 " & _
        '"  ORDER BY [MRP_DATE] ASC", Me.organizationId, strSupplDemandItem)
        '    dgvShortages.DataSource = dsMRPShortages.Tables(0)
        '    ds.Tables(0).Columns.Add(adc1)
        '    gridDisplay.formatGrid(dgvShortages, "supplyDemandMRPView")

        'End If

        'Just calculate when there is a record available
        If ds.Tables(0).Rows.Count > 0 Then
            quantityOnHand = ds.Tables(0).Rows(0).Item("QTY_ONHAND")
            runningQuantity = quantityOnHand
            strPart = ds.Tables(0).Rows(0).Item("ITEM_NO")


            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                runningQuantity = runningQuantity - (ds.Tables(0).Rows(i).Item("DEMAND_QUANTITY"))
                dblTotalDemand += (ds.Tables(0).Rows(i).Item("DEMAND_QUANTITY"))

                'Remove the starting on hand quantity
                If ds.Tables(0).Rows.Count > 1 AndAlso i > 0 AndAlso ds.Tables(0).Rows(i).Item("SUPPLY_TYPE") = "On Hand" Then
                    ds.Tables(0).Rows(i).Item("SUPPLY_QUANTITY") = DBNull.Value
                End If

                'Add quantity the first time a supply is available
                If ds.Tables(0).Rows(i).Item("SUPPLY_TYPE") <> "On Hand" Then
                    strnewJobPo = ds.Tables(0).Rows(i).Item("JobPOLine")
                    If strnewJobPo <> strlastJobPo Then
                        strlastJobPo = strnewJobPo
                        runningQuantity += ds.Tables(0).Rows(i).Item("SUPPLY_QUANTITY")
                    Else
                        '                        dgv.Rows(i).Cells("SUPPLY_QUANTITY") = Nothing
                        ds.Tables(0).Rows(i).Item("SUPPLY_QUANTITY") = DBNull.Value
                    End If
                End If

                'Place the calculated quantity in the table
                ds.Tables(0).Rows(i).Item("Running Total") = runningQuantity
                If runningQuantity < 0 Then
                    dgv.Rows(i).Cells("Running Total").Style.BackColor = Color.Red
                    dgv.Rows(i).Cells("Running Total").Style.SelectionBackColor = Color.DarkRed
                End If

            Next
            'Just go into the testing cycle if it is called for a part number.
            If calculatLikeOracle Then
                runningQuantity = Me.calculateRunningNumberOnMRPQuantity(ds, dgv, runningQuantity, dblTotalDemand, strPart)
            End If

        End If

        'Creating the label header of the supply and demand view.
        lblShortageDescription.Text = "Supply & Demand For Part: " & strPart & ControlChars.Tab & "    Starting On Hand Quantity:" & quantityOnHand.ToString & ControlChars.Tab & _
        "   Ending On Hand Quantity:" & runningQuantity.ToString

    End Sub

    ''' <summary>
    ''' Calculates the projected quantity into the future over the currrent supply and demand situation.
    ''' </summary>
    ''' <param name="ds">The dataset which contains the current supply and demand situation.</param>
    ''' <param name="dgv">The datagridview the supply and demand situation is attached to.</param>
    ''' <param name="runningQA">The running quantity over the whole time period.</param>
    ''' <param name="dbltotalDem">The current total demand.</param>
    ''' <param name="strPart">The part number this procedure is looking for.</param>
    ''' <returns>The new running quantity.</returns>
    ''' <remarks>Changes when the real sales order demand is manually changed from the people to fixed lot sizes.</remarks>
    Private Function calculateRunningNumberOnMRPQuantity(ByRef ds As DataSet, ByRef dgv As DataGridView, ByVal runningQA As Double, ByVal dbltotalDem As Double, ByVal strPart As String)

        Dim quantityOnHand As Double = 0
        Dim runningQuantity As Double = runningQA
        Dim dblTotalDemand As Double = dbltotalDem


        Dim strlastJobPo As String = ""
        Dim strnewJobPo As String = ""

        '---------------------------------------------------------------------------------------------------
        'Cross check if there is a different allocation on the jobs, so we have to use the mrp quantity.
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds2 As New DataSet
        ds2 = db.SecureQueryParams("SELECT TOP 1 QTY_ALLOC_JOB FROM active_items WHERE item_no = @1 AND organization_id = @2", strPart, Me.organizationId)

        Try
            'If the allocation on jobs of this part is higher than
            'If ds2.Tables(0).Rows(0).Item(0) > dblTotalDemand Then

            'Because we changed the calculation to the next upper jobs level, I would like to enable the user to switch back just 
            lblDemandView.Visible = True

            'If ds.Tables(0).Rows.Count > 0 Then

            quantityOnHand = ds.Tables(0).Rows(0).Item("QTY_ONHAND")
            runningQuantity = quantityOnHand
            strPart = ds.Tables(0).Rows(0).Item("ITEM_NO")

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                runningQuantity = runningQuantity - (ds.Tables(0).Rows(i).Item("MRP_QTY"))



                'Add quantity the first time a supply is available
                If ds.Tables(0).Rows(i).Item("SUPPLY_TYPE") <> "On Hand" Then
                    strnewJobPo = ds.Tables(0).Rows(i).Item("JobPOLine")
                    If strnewJobPo <> strlastJobPo Then
                        strlastJobPo = strnewJobPo
                        runningQuantity += ds.Tables(0).Rows(i).Item("SUPPLY_QUANTITY")
                    End If
                End If

                'Place the calculated quantity in the table
                ds.Tables(0).Rows(i).Item("Running Total") = runningQuantity
                If runningQuantity > 0 Then
                    dgv.Rows(i).Cells("Running Total").Style.BackColor = Color.MediumVioletRed
                    dgv.Rows(i).Cells("Running Total").Style.SelectionBackColor = Color.DarkRed

                End If

            Next
            ' End If
            runningQuantity = Me.calculateRunningNumberOnSupplyDemandTable(ds, dgv, runningQuantity, dblTotalDemand, strPart)

            'End If

            'Control it the third time and then switch to the other view
            Return runningQuantity


        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("populateSupplyDemand", ex, 1)

        End Try
        Return runningQuantity
    End Function


    ''' <summary>
    ''' Last function which calculates the quantities on the other table. This is done over a check if the jobs on this level are running with a different quantity.
    ''' </summary>
    ''' <param name="ds">The dataset which contains the current supply and demand situation.</param>
    ''' <param name="dgv">The datagridview the supply and demand situation is attached to.</param>
    ''' <param name="runningQA">The running quantity over the whole time period.</param>
    ''' <param name="dbltotalDem">The current total demand.</param>
    ''' <param name="strPart">The part number this procedure is looking for.</param>
    ''' <returns>The new running quantity.</returns>
    ''' <remarks></remarks>
    Private Function calculateRunningNumberOnSupplyDemandTable(ByRef ds As DataSet, ByRef dgv As DataGridView, ByVal runningQA As Double, ByVal dbltotalDem As Double, ByVal strPart As String)

        Dim quantityOnHand As Double = 0
        Dim runningQuantity As Double = runningQA
        Dim dblTotalDemand As Double = dbltotalDem
        Dim strAdditionalQueryConstraint As String = ""
        Dim strSalesOrderLineColumn As String = "	CONVERT(VARCHAR,CONVERT(INTEGER,[PARENT_SALES_ORDER_NO])) + '_' + CONVERT(VARCHAR,CONVERT(INTEGER,[PARENT_SALES_ORDER_LINE])) AS SalesOrderLine "

        Dim queryView As String = "supplyDemandSalesOrderView2"
        Dim mrpColumns As String = ", [consumptionPriorityDate]  "
        Dim sortExperience As String = " consumptionPriorityDate, MRP_Date "

        Dim strlastJobPo As String = ""
        Dim strnewJobPo As String = ""

        'Create combined supply and demand view which really shows everything going on for the part number
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        'Understand which planning type to query
        If Me.planningType = "ASCP" Then
            strSalesOrderLineColumn = "PARENT_SALES_ORDER_NO + '_' + PARENT_SALES_ORDER_LINE AS SalesOrderLine "
            queryView = "MSCsupplyDemandSalesOrderView"
            mrpColumns = " , DEMAND_TYPE_DESC, supplyDescription, demand_priority ,listOfSalesOrderLines , countDistinctSalesOrderLines, statusSupplyingJob, PO_ACCEPTED_FLAG "
            sortExperience = " demand_priority,  mrp_date, CASE WHEN short IS NULL THEN 0 ELSE 1 end, fromOnHand DESC,  demand_id  "
            Me.supplyDemandMiddleGrid = "MSCsupplyDemandMRPView"
            strAdditionalQueryConstraint = " AND mrp_qty <> 0"
        End If

        'Dim ds2 As New DataSet
        'ds2 = db.SecureQueryParams("SELECT TOP 1 QTY_ALLOC_JOB FROM active_items WHERE item_no = @1 AND organization_id = @2", strPart, Me.organizationId)

        Try

            dsMRPShortages = db.SecureQueryParams("     SELECT " & strSalesOrderLineColumn & _
             ",	[ITEM_NO] " & _
             ",	[DESCRIPTION] " & _
             ",	[MAKE_BUY] " & _
             ",	[MRP_QTY] " & _
             ",	JobPOLine " & _
             ",	[MRP_DATE] " & _
                 mrpColumns & _
             ",	[STATUS] " & _
             ",	[QTY_OPEN] as 	[SUPPLY_QUANTITY] " & _
             ",	[OP1] " & _
             ",	[OP2] " & _
             ",	[OP3] " & _
             ",	[OP4] " & _
             ",	[OP5] " & _
             ",	[SAFETY_STOCK_QUANTITY] " & _
             ",	[QTY_ONHAND] " & _
             ", [short] " & _
             ",	[JOB_NO] " & _
             ",	[PURCHASE_ORDER_NO] " & _
             ",	[PURCH_LINE_NO] " & _
             ",	[PARENT_SALES_ORDER_NO] AS [ORDER_NO] " & _
             ",	[PARENT_SALES_ORDER_LINE] AS [LINE_NO] " & _
             ", [PROJECT_NAME] " & _
             ", [parentJobOrderNo] " & _
             ",  Drawing  " & _
             ", [BUYER] " & _
             ", [RELEASE_NO] " & _
             ", [SHIPMENT_NO]" & _
             "   FROM " & queryView & " WHERE organization_id = @1 AND (item_no = @2 OR drawing LIKE '%" & strPart & "%') " & strAdditionalQueryConstraint & " ORDER BY " & sortExperience & "  ", Me.organizationId, strPart)

			dgvShortages.DataSource = dsMRPShortages.Tables(0)
            gridDisplay.formatGrid(dgvShortages, Me.supplyDemandMiddleGrid) 'this is how we rename the columns, set the column order and widths

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("populateSupplyDemand", ex, 1)
        End Try

        Return runningQuantity


    End Function



    ''' <summary>
    ''' Populates the shortages to 
    ''' </summary>
    ''' <param name="end_demand_id">The end demand for the running number.</param>
    ''' <param name="organization_id">The organization id you want to look for.</param>
    ''' <param name="additionalFilterString"></param>
    ''' <param name="peggingId">Pegging id for example for lookup below a job.</param>
    ''' <remarks></remarks>
    Private Sub populateMRPShortages(ByVal end_demand_id As Double, ByVal organization_id As Double, ByVal additionalFilterString As String, Optional ByVal peggingId As Double = 0, Optional ByVal listOfdemands As String = "", Optional ByVal plannedOrders As Boolean = False _
                                     , Optional ByVal problemFilter As Boolean = False, Optional ByVal shortInfo As String = " AND short is not null " _
                                     , Optional ByVal allShortages As Boolean = False)
        Dim queryView As String = " MRPShortagesView "
        Dim strAdditionalColumns = ""
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim id As Integer = CInt(end_demand_id)
        lblDemandView.Visible = False

        'Understand if we want to query ASCP
        If Me.planningType = "ASCP" Then
            queryView = " MSCShortagesView "
            strAdditionalColumns = ", [ALLOCATED_QUANTITY], [SUPPLY_QUANTITY], [NEW_SCHEDULE_DATE], firm_planned_flag_display, RELEASE_NO, SHIPMENT_NO, PO_ACCEPTED_FLAG  "
        Else
            strAdditionalColumns = " , [neededBy], RELEASE_NO, SHIPMENT_NO , [MRP_DATE]"
        End If

        If peggingId <> 0 Then
            additionalFilterString += " AND prev_pegging_id = " & peggingId.ToString
        End If

        'Adopt the level number - for on hand items we want to see it
        Dim strLevelNumber As String = ""
        If shortInfo = " AND short is not null " Then
         
            strLevelNumber = " AND LEVEL_NO > 0  "

        Else
            strLevelNumber = " "
        End If

        If listOfdemands = "" Then
            dsMRPShortages = db.SecureQueryParams("SELECT [ORGANIZATION_ID], [END_DEMAND_ID] " & _
                                                ",[ITEM_NO]" & _
                                                ",[DESCRIPTION]" & _
                                                ",[MBLevel]" & _
                                                ",[MAKE_BUY]" & _
                                                ",[LEVEL_NO]" & _
                                                ",[MRP_QTY]" & _
                                                ",[JobPOLine]" & _
                                                ",[JOB_NO]" & _
                                                ",[PURCHASE_ORDER_NO]" & _
                                                ",[PURCH_LINE_NO]" & _
                                                ",[START_DATE]" & _
                                                ",[STATUS]" & _
                                                ",[QTY_OPEN]" & _
                                                ",[OP1]" & _
                                                ",[OP2]" & _
                                                ",[OP3]" & _
                                                ",[OP4]" & _
                                                ",[OP5]" & _
                                                ",[SAFETY_STOCK_QUANTITY]" & _
                                                ",[QTY_ONHAND]" & _
                                                ",[REASON_CODE]" & _
                                                ",[DELAY]" & strAdditionalColumns & _
                                                ",[SUPPLY_TYPE]" & _
                                                ",[PLANNED_ORDER_ID]" & _
                                                ",[PLANNED_PO_ID]" & _
                                                ",[PO_NEED_BY_DATE]" & _
                                                ",[PO_PROMISED_DATE]" & _
                                                ",[SCHEDULE_COMPRESSION_DAYS]" & _
                                                ",[PREV_PEGGING_ID]" & _
                                                ",[PEGGING_ID]" & _
                                                ",[PO_PRINTED_DATE]" & _
                                                ",[FULL_LEAD_TIME]" & _
                                                ",[ITEM_COSTS]" & _
                                                ",[EXTENDED_COSTS]" & _
                                                ",[SOURCE_VENDOR_ID]" & _
                                                ",[BUYER]" & _
                                                ",[PLANNER_CODE]" & _
                                                ",[PROJECT_NAME]" & _
                                                ",[ITEM_STATUS] " & _
                                                ",[SEQUENCE]" & _
                                                ",[lastComment] " & _
                                                ",[INVENTORY_ITEM_ID] " & _
                                                ",[ORDER_NO] " & _
                                                ",[LINE_NO] " & _
                                                ", Drawing " & _
                                                "FROM " & queryView & " WHERE  [end_demand_id] = @1  " & _
                                                " " & strLevelNumber & additionalFilterString & shortInfo & "  ORDER BY neededBy DESC", id)

            'here is only one order selected in the user interface
            'For the lookup value it just remains at the first selected value
            'When populating from a job there is sometimes no cell in the sales order line queue selected.
            If dgvSalesOrders.SelectedCells.Count > 0 Then
                lblShortageDescription.Text = "Shortages For Order: " & dgvSalesOrders.SelectedCells(0).Value
            Else
                lblShortageDescription.Text = "Shortages For Job:"
            End If


        Else
            'Distinct between display of planned orders and shortages
            Dim strPlanned As String = ""
            If plannedOrders AndAlso Not problemFilter Then
                If allShortages Then            'Here we do distinct between planned orders and all shortages
                    strPlanned = "end_demand_id IS NOT NULL AND "
                Else
                    strPlanned = "end_demand_id IS NOT NULL AND OP1 IS NULL AND status IS NULL AND "
                End If

            ElseIf problemFilter Then
                strPlanned = ""
            Else
                strPlanned = " [end_demand_id] IN " & listOfdemands & "  AND "
            End If

            'Select statement for the combinations
            dsMRPShortages = db.SecureQueryParams("SELECT [ORGANIZATION_ID], [END_DEMAND_ID] " & _
                                               ",[OrderNoLine] " & _
                                               ",[ITEM_NO]" & _
                                               ",[DESCRIPTION]" & _
                                               ",[MBLevel]" & _
                                               ",[MAKE_BUY]" & _
                                               ",[LEVEL_NO]" & _
                                               ",[MRP_QTY]" & _
                                               ",[JobPOLine]" & _
                                               ",[JOB_NO]" & _
                                               ",[PURCHASE_ORDER_NO]" & _
                                               ",[PURCH_LINE_NO]" & _
                                               ",[START_DATE]" & _
                                               ",[STATUS]" & _
                                               ",[QTY_OPEN]" & _
                                               ",[OP1]" & _
                                               ",[OP2]" & _
                                               ",[OP3]" & _
                                               ",[OP4]" & _
                                               ",[OP5]" & _
                                               ",[SAFETY_STOCK_QUANTITY]" & _
                                               ",[QTY_ONHAND]" & _
                                               ",[REASON_CODE]" & _
                                               ",[DELAY]" & strAdditionalColumns & _
                                               ",[SUPPLY_TYPE]" & _
                                               ",[PLANNED_ORDER_ID]" & _
                                               ",[PLANNED_PO_ID]" & _
                                               ",[PO_NEED_BY_DATE]" & _
                                               ",[PO_PROMISED_DATE]" & _
                                               ",[SCHEDULE_COMPRESSION_DAYS]" & _
                                               ",[PREV_PEGGING_ID]" & _
                                               ",[PEGGING_ID]" & _
                                               ",[PO_PRINTED_DATE]" & _
                                               ",[FULL_LEAD_TIME]" & _
                                               ",[ITEM_COSTS]" & _
                                               ",[EXTENDED_COSTS]" & _
                                               ",[SOURCE_VENDOR_ID]" & _
                                               ",[BUYER]" & _
                                               ",[PLANNER_CODE] " & _
                                               ",[PROJECT_NAME]" & _
                                               ",[ITEM_STATUS] " & _
                                               ",[SEQUENCE]" & _
                                               ",[lastComment] " & _
                                               ",[INVENTORY_ITEM_ID] " & _
                                               ",[ORDER_NO] " & _
                                               ",[LINE_NO] " & _
                                               ", Drawing " & _
                                               "FROM " & queryView & " WHERE    " & strPlanned & _
                                               "  organization_id = @1  " & shortInfo & additionalFilterString & " ORDER BY [OrderNoLine], neededBy DESC", Me.organizationId)

            'here is only one order selected in the user interface
            'For the lookup value it just remains at the first selected value
            lblShortageDescription.Text = "Shortages For Multiple Order Lines."

        End If

        'put it into the datagrid view - apply filter, and reordering later.
        strMiddleGrid = "MRPShortagesView"
        dgvShortages.DataSource = Nothing
        Try

            dgvShortages.DataSource = dsMRPShortages.Tables(0)
            'Me.sortShortageGrid(dsMRPShortages)
            dgvShortages.Sort(dgvShortages.Columns("sequence"), System.ComponentModel.ListSortDirection.Ascending)
            dgvShortages.Columns("sequence").Visible = False
            gridDisplay.formatGrid(dgvShortages, "MRPShortagesView")

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("toolStripMenuItemBuy", ex, 1)
        End Try


    End Sub

    Private Sub sortShortageGrid(ByRef ds As DataSet)


        Dim sequence As Integer = 0
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            If DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("sequence")) Then
                ds.Tables(0).Rows(i).Item("sequence") = sequence
                sequence += 1
            End If

            For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If DBNull.Value.Equals(ds.Tables(0).Rows(j).Item("sequence")) AndAlso ds.Tables(0).Rows(j).Item("prev_pegging_id") = ds.Tables(0).Rows(i).Item("pegging_id") Then
                    ds.Tables(0).Rows(i).Item("sequence") = sequence
                    sequence += 1
                End If
            Next

        Next


    End Sub

    Private Sub loadSalesOrder(ByVal additionalFilterString As String)
        Try
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim viewName As String = "SalesOrderLinesView"
            Dim strAdditionalColumn As String = " "

            Dim Top100 As String = ""

            If planningType = "ASCP" Then
                viewName = " MSCSalesOrderLinesView "
                strAdditionalColumn = " , DEMAND_PRIORITY , firm_planned_flag_display, plannerCode, internalAlerts, supplyAlerts "
            End If

            If IsVPNActive Then
                Top100 = " TOP 100 "
            End If

            dsSalesOrder = db.SecureQueryParams("SELECT " & Top100 & " [ORGANIZATION_ID], [HEADER_ID], [NO_OF_PROCESSES] " & _
              ",[OrderNoLine] " & _
              ",[ORDER_NO] " & _
              ",[LINE_NO] " & _
              ",[ORDER_TYPE] " & _
              ",[ITEM_NO] " & _
              ",[DESCRIPTION] " & _
              ",ITEM_DESCRIPTION " & _
              ",[INVENTORY_ITEM_ID] " & _
              ",[SCHEDULE_SHIP_DATE] " & _
              ",[REQUEST_DATE] " & _
              ",[ORDERED_QUANTITY] " & _
              ",[SHIPPED_QUANTITY] " & _
              ",[OP1] " & _
              ",[OP2] " & _
              ",[OP3] " & _
              ",[OP4] " & _
              ",[PLANNER_CODE] " & _
              ",[QTY_ONHAND] " & _
              ",lastComment " & _
              ",[PARTY_NAME] " & _
              ",[CUST_PO_NO] " & _
              ",[FREIGHT_CARRIER_CODE] " & _
              ",[FREIGHT_TERMS_CODE] " & _
              ",[JOB_PO] " & _
              ",[JOB_PO_STATUS] " & _
              ",[JOB_START_DATE] " & _
              ",[JOB_PO_QUANTITY] " & _
              ",[JOB_RELEASE_DATE] " & _
              ",[neededBy] " & _
              ",[UNIT_COSTS] " & _
              ",[DEMAND_ID] " & _
              ",[UNIT_SELLING_PRICE] " & _
              ",[EXTENDED_PRICE] " & _
              ", ProcessProblems " & _
              ",[TRANSACTIONAL_CURR_CODE] " & _
              ",[EXTENDED_COSTS] " & _
              ",[MARGIN] " & _
              ",[MARGIN_PERCENTAGE]" & _
              ",[JOB_PO_MRP_DATE]" & _
              ",[PO_LINE]" & _
              ",[VENDOR_ID]" & _
              ",[REPLENISHMENT_DETAILS]" & _
              ",[ORDERED_DATE]" & _
              ",[BOM_DATE]" & _
              ",[PURCHASING_DATE]" & strAdditionalColumn & _
              ",[MANUFACTURING_DATE]" & _
              ",[MRP_PLANNING_CODE]" & _
              ",[REASON_CODE] " & _
              ",[NAME] " & _
              ",[PROJECT_ID] " & _
              ",[PROJECT_NUMBER] " & _
              ",[PROJECT_NAME] " & _
              ",[FSG_SALES_TYPE] " & _
              ",[FSG_SPARES_CLASS] " & _
              ",[FSG_PRODUCT_CODE] " & _
              ",[FSG_IND_PAINT_PARTS] " & _
              ",[FLOW_STATUS_CODE] " & _
              ",[PICK_STATUS] " & _
              ",CASE WHEN [orderCompleteForShipping] = 1 THEN 'Y' ELSE 'N' END orderCompleteForShipping " & _
              ",CASE WHEN [checkForPartialShipment] = 1 THEN 'Y' ELSE 'N' END AS checkForPartialShipment " & _
              ",[HOLDS_HEADER]" & _
              ",[HOLDS_LINE]" & _
              ",[ATTRIBUTE1] " & _
              ",[ATTRIBUTE2] " & _
              ",[ATTRIBUTE3] " & _
              ",[ATTRIBUTE5] " & _
              ",[ATTRIBUTE6] " & _
              ",[ATTRIBUTE16] " & _
              ",[ATTRIBUTE17] " & _
              ",[ATTRIBUTE18] " & _
              ",[ATTRIBUTE19] " & _
              ",[ATTRIBUTE20] " & _
              ",[ATTRIBUTE10] " & _
              ",[SHIPMENT_PRIORITY_CODE] " & _
              ",[CONTEXT] " & _
              ",[CUSTOMER_TYPE] " & _
              ",[UNIT_ACCESSORIES_VALUE] " & _
              ",[TRADING_ACCT] " & _
              ",[PROMISE_DATE] " & _
              ",[PAYMENT_TERM_DESCRIPTION] " & _
              ",[JOB_NO] " & _
              ",[PO_NO] " & _
              ",[NO_OF_PURCHASED_SHORTAGES] " & _
              ",[MAKE_BUY] " & _
              ",[no_of_problems] " & _
              ",[SHIP_SET_ID] " & _
              ",[reasonCode] " & _
              ",[potentialSlippage] " & _
              ",[onHandSupplyHolds] " & _
              ", Drawing " & _
              ", CREATION_DATE_COLINE " & _
              ", lastForecast " & _
              ",HEADER_SHIPPING_INSTRUCTIONS " & _
              ",FOB_POINT_CODE " & _
              ",operationSequenceDetails " & _
              ",SHIP_TO_ADDRESS1 " & _
              ",SHIP_TO_ADDRESS2 " & _
              ",SHIP_TO_ADDRESS3 " & _
              ",SHIP_TO_ADDRESS4 " & _
              ",SHIP_TO_ADDRESS5 " & _
              " FROM " & viewName & " WHERE [organization_id] = @1 " & additionalFilterString & " ORDER BY [schedule_ship_date], [order_no], [line_no]", Me.organizationId, additionalFilterString)

            'Before assigning the data table to the gridview, implemented in try catch block and error logging within this.
            'Test if we have data retrieved
            If dsSalesOrder.Tables.Count = 0 Then
                MessageBox.Show("Please retry your query.")     '*** Tell the user why the query did not work
                dgvSalesOrders.DataSource = Nothing
                Exit Sub
            Else
                dgvSalesOrders.DataSource = dsSalesOrder.Tables(0)
                gridDisplay.formatGrid(dgvSalesOrders, "SalesOrderLinesView", Me.organizationId)
            End If

            
        Catch ex As Exception
            MessageBox.Show("Please retry your query.")
            'Dim err As New clsExceptionManagement          '*** no exception handling here -> store everything on the client
            'err.createErrorLog("loadSalesOrder", ex, 1)
        End Try



    End Sub


    Private Sub loadSalesOrderGrouped(ByVal additionalFilterString As String)

        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        dsSalesOrderGrouped = db.SecureQueryParams("SELECT [ORGANIZATION_ID] " & _
                                                  "    ,[OPERATING_UNIT_ID] " & _
                                                  "    ,[HEADER_ID] " & _
                                                  "    ,[ORDER_NO] " & _
                                                  "    ,[no_of_open_lines] " & _
                                                  "    ,[ORDER_TYPE_ID] " & _
                                                  "    ,[ORDER_TYPE] " & _
                                                  "    ,[PARTY_NAME] " & _
                                                  "    ,[processes_total] " & _
                                                  "    ,[problems_total] " & _
                                                  "    ,[price_total] " & _
                                                  "    ,[startShipping] " & _
                                                  "    ,[endShipping] " & _
                                                  "    ,[quantity_total] " & _
                                                  "    ,[cancelled_total] " & _
                                                  "    ,[shipped_total] " & _
                                                  "    ,[FREIGHT_CARRIER_CODE] " & _
                                                  "    ,[FREIGHT_TERMS_CODE] " & _
                                                  "    ,[CUST_PO_NO] " & _
                                                  "    ,[lastComment] " & _
                                                  "    ,[noOfComments] " & _
                                                 " FROM [salesOrdersView] WHERE [organization_id] = @1 " & additionalFilterString & " ORDER BY [startShipping], [order_no]", Me.organizationId, additionalFilterString)


        'Bind the dataSets to the grid views

        dgvSalesOrders.DataSource = dsSalesOrderGrouped.Tables(0)
        'gridDisplay.formatGrid(dgvSalesOrders, "salesOrdersView")

    End Sub


    Private Sub loadPO(ByVal additionalFilterString As String, Optional ByVal orderString As String = "[VENDOR_NAME] ASC, CASE WHEN neededBy IS NULL THEN 1 ELSE 0 end, [neededBy] ASC ,[NEED_BY_DATE] ASC")
        Try
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim Top100 As String = ""
            Dim viewName As String = "poQueueView"
            Dim additionalASCPColumn As String = ""

            If planningType = "ASCP" Then
                viewName = "MSCpoQueueView"
                Me.poBottonGrid = "MSCpoQueueView"
                additionalASCPColumn = ", RELEASE_FIRM, rescheduleIN, rescheduleOUT, exceptionMessage, cancelllationMessage, ospJobNumber, ACCEPTED_FLAG, ISUPPLIERVENDORDESIGNATION "
            End If

            If IsVPNActive Then
                Top100 = "TOP 100"
            End If
            dsPO = db.SecureQueryParams("SELECT " & Top100 & "                " & _
              " [BUYER]              " & _
              ",[ITEM_NO]            " & _
              ",[ITEM_DESCRIPTION]   " & _
              ",[SalesOrderLine]     " & _
              ",[SALES_ORDER_NO]     " & _
              ",[SALES_LINE_NO]      " & _
              ",[VENDOR_NAME]        " & _
              ",[VENDOR_ID]          " & _
              ",PONoLine             " & _
              ",[NEED_BY_DATE]       " & _
              ",[neededBy]           " & _
              ",[PO_NO]              " & _
              ",[RELEASE_NO]         " & _
              ",[LINE_NO]            " & _
              ",[SHIPMENT_NO]        " & _
              ",[CREATION_DATE]      " & _
              ",[CREATION_DATE_POL]  " & _
              ",[QUANTITY_ORDERED]   " & _
              ",[QTY_RECEIVED]       " & _
              ",[lastComment]        " & _
              ",[AGE]                " & _
              ",[EMAIL_ADDRESS]      " & _
              ",[PHONE]              " & _
              ",[VEND_CONTACT]       " & _
              ",[PROMISED_DATE]      " & _
              ",[PRINTED_DATE]       " & _
              ",[UNIT_PRICE]         " & _
              ",[ORGANIZATION_ID]    " & _
              ",[PO_HEADER_ID]       " & _
              ",[PROJECT_NUMBER]     " & _
              ",[PROJECT_NAME]                                  " & _
              ",[FSG_COMMODITY_CODE]                            " & _
              ",[FSG_MATERIAL_CODE]                             " & _
              ",[qtyAvailable]                                  " & _
              ",[fixed_lead_time]                               " & _
              ",[safety_stock_quantity]                         " & _
              ",[APPROVED_FLAG]                                 " & _
              ",[PARTY_NAME]                                    " & _
              ",[topLevelJobNo]                                 " & _
              ",[topLevelJobStartDate]                          " & _
              ",[topLevelJobStatus]                             " & _
              ",[topLevelPlannerCode]                           " & _
              ",[NO_OF_PURCHASED_SHORTAGES]                     " & _
              ",[attachmentText]                                " & _
              ",[alternateContact]                              " & _
              ",[QTY_ONHAND]                                    " & _
              ",[CURRENCY_CODE]                                 " & _
              ",[UNIT_MEAS_LOOKUP_CODE]                         " & _
              ",[QUANTITY_ORDERED]*[UNIT_PRICE] EXTENDED_PRICE  " & _
              ", Drawing                                        " & _
              ", percentageSafetyStock                          " & _
              ", comments                                       " & additionalASCPColumn & _
              " FROM " & viewName & "                              " & _
              " WHERE ship_to_organization_id = @1 " & additionalFilterString & " ORDER BY " & orderString & " ", Me.organizationId)

            searchPoList.Clear()        'Reset the search array
            seachPOListPOLine.Clear()

            Dim vendor_name As String = ""
            'Iterare of the dataset and insert a new row, when the supplier changes
            Dim countCycles As Integer = dsPO.Tables(0).Rows.Count - 1
            For i As Integer = 0 To countCycles + 20000
                If i >= dsPO.Tables(0).Rows.Count - 1 Then
                    Exit For
                End If
                If Not DBNull.Value.Equals(dsPO.Tables(0).Rows(i).Item("vendor_name")) Then
                    If dsPO.Tables(0).Rows(i).Item("vendor_name") <> vendor_name Then
                        Dim row As System.Data.DataRow
                        row = dsPO.Tables(0).NewRow
                        vendor_name = dsPO.Tables(0).Rows(i).Item("vendor_name")
                        dsPO.Tables(0).Rows.InsertAt(row, i)
                    End If
                End If
            Next

            'Create the search list and sort it
            For i As Integer = 0 To dsPO.Tables(0).Rows.Count - 1
                searchPoList.Add(New poSearch(i, dsPO.Tables(0).Rows(i).Item("vendor_name").ToString & dsPO.Tables(0).Rows(i).Item("po_no").ToString & dsPO.Tables(0).Rows(i).Item("line_no").ToString & dsPO.Tables(0).Rows(i).Item("RELEASE_NO").ToString & dsPO.Tables(0).Rows(i).Item("SHIPMENT_NO").ToString))
                seachPOListPOLine.Add(New poSearch(i, dsPO.Tables(0).Rows(i).Item("vendor_name").ToString & dsPO.Tables(0).Rows(i).Item("po_no").ToString & dsPO.Tables(0).Rows(i).Item("line_no").ToString))

            Next
            bottonGrid = "po"
            searchPoList.Sort(AddressOf sortListPo)
            seachPOListPOLine.Sort(AddressOf sortListPo)

            dgvWipSupplierQueue.DataSource = dsPO.Tables(0)
            gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("loadPO", ex, 1)
        End Try
    End Sub


    ''' <summary>
    ''' Procedure that keeps the generic list model for searching purchase orders.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub createSearchListPo()
        searchPoList.Clear()
        seachPOListPOLine.Clear()
        For i As Integer = 0 To dgvWipSupplierQueue.DataSource.Rows.Count - 1
            searchPoList.Add(New poSearch(i, dgvWipSupplierQueue.Rows(i).Cells("vendor_name").Value.ToString & dgvWipSupplierQueue.Rows(i).Cells("po_no").Value.ToString & dgvWipSupplierQueue.Rows(i).Cells("line_no").Value.ToString))
            seachPOListPOLine.Add(New poSearch(i, dgvWipSupplierQueue.Rows(i).Cells("vendor_name").Value.ToString & dgvWipSupplierQueue.Rows(i).Cells("po_no").Value.ToString & dgvWipSupplierQueue.Rows(i).Cells("line_no").Value.ToString))
        Next
    End Sub



    Private Shared Function sortListPo(ByVal x As poSearch, ByVal y As poSearch) As Integer
        Return x.searchPhrase.CompareTo(y.searchPhrase)
    End Function

    'Private dgvWIP As New DataGridView

    Private Sub loadWip(ByVal additionalFilterString As String, Optional ByVal strOrderByClause As String = " [DEPARTMENT_CODE], [RESOURCE_CODE], [FIRST_UNIT_START_DATE] ")
        Dim strAdditionalColumne As String = ""

        If Me.organizationId = 881 Then
			strOrderByClause = " [PLANNER_CODE],CASE WHEN parentJobStartDate IS NULL THEN 1 ELSE 0 end, [parentJobStartDate], [FIRST_UNIT_START_DATE] "
        End If

        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim Top100 As String = ""

        If IsVPNActive Then
            Top100 = "TOP 100"
        End If

        Dim viewName As String = "wipQueueView"
        If planningType = "ASCP" Then
            viewName = "MSCwipQueueView"
            strAdditionalColumne = " ,firm_planned_flag_display"
        End If

        dsWIP = db.SecureQueryParams("SELECT " & Top100 & "                   " & _
         "  [DEPARTMENT_CODE]                                   " & _
         ", [RESOURCE_CODE]                                     " & _
         ", [ITEM_NO]                                           " & _
         ", [DESCRIPTION]                                       " & _
         ", [PARTY_NAME]                                        " & _
         ", [JOB_NO]                                            " & _
         ", [SCHEDULED_QUANTITY]                                " & _
         ", [QUANTITY_OPEN]                                     " & _
         ", [QUANTITY_IN_QUEUE]                                 " & _
         ", lastComment                                         " & _
         ", [SalesOrderLine]                                    " & _
         ", [JobNoOperationSeq]                                 " & _
         ", [MEANING]                                           " & _
         ", [PROJECT_NAME]                                      " & _
         ", [HOURS_OPEN]                                        " & _
         ", [FSG_MATERIAL_CODE]                                 " & _
         ", [FIRST_UNIT_START_DATE]                             " & _
         ", [RESOURCE_END_DATE]                                 " & _
         ", [WIP_ENTITY_ID]                                     " & _
         ", [PROJECT_NUMBER]                                    " & _
         ", [firstPeggingId]                                    " & _
         ", [end_demand_id]                                     " & _
         ", [ORGANIZATION_ID]                                   " & _
         ", [QUANTITY_SCRAPPED]                                 " & _
         ", [QUANTITY_COMPLETED]                                " & _
         ", [COMPLETION_DATE]                                   " & _
         ", [SALES_ORDER_NO]                                    " & _
         ", [SALES_LINE_NO]                                     " & _
         ", [OPERATION_SEQ_NO]                                  " & _
         ", operationSequenceIndicator                          " & _
         ", [QtyTotalDemand]                                    " & _
         ", [AverageLast10days]                                 " & _
         ", [Usage3Month]                                       " & _
         ", [Usage6Month]                                       " & _
         ", [Usage12Month]                                      " & _
         ", [DESCRIPTION_OPER]                                  " & _
         ", [PLANNER_CODE]                                      " & _
         ", [qtyAvailable]                                      " & _
         ", [neededBy]                                          " & _
         ", [componentRequirements]                             " & _
         ", [parentJobNo]                                       " & _
         ", [parentJobStartDate]                                " & _
         ", [parentJobStatus]                                   " & _
         ", [parentJobPlannerCode]                              " & _
         ", [FSG_PRODUCT_CODE]                                  " & _
         ", [FSG_SALES_TYPE]                                    " & _
         ", [Drawing]                                           " & _
         ", [SAFETY_STOCK_QUANTITY]                             " & _
         ", [Buyer]                                             " & _
         ", [no_of_purchased_shortages]                         " & _
         ", [DATE_RELEASED]                                     " & _
         ", [scheduled_completion_date]                         " & _
         ", [job_creation_date]                                 " & strAdditionalColumne & _
         " FROM " & viewName & " WHERE [ORGANIZATION_ID] = @1 " & additionalFilterString & " ORDER BY  " & strOrderByClause & "", Me.organizationId)

        Dim resource_name As String = ""
        searchJobList.Clear()

        Try
            If Me.organizationId = 881 Then
                'Iterare of the dataset and insert a new row, when the supplier changes - org specific break rule
                For i As Integer = 0 To dsWIP.Tables(0).Rows.Count + 20000
                    'Force null value replaced but kept in the backend, in order to maintain filter ability
                    If DBNull.Value.Equals(dsWIP.Tables(0).Rows(i).Item("PLANNER_CODE")) Then
                    Else
                        If i >= dsWIP.Tables(0).Rows.Count - 1 Then
                            Exit For
                        End If

                        If dsWIP.Tables(0).Rows(i).Item("PLANNER_CODE") <> resource_name Then
                            Dim row As System.Data.DataRow
                            row = dsWIP.Tables(0).NewRow

                            resource_name = dsWIP.Tables(0).Rows(i).Item("PLANNER_CODE")
                            dsWIP.Tables(0).Rows.InsertAt(row, i)
                        End If
                    End If
                Next

            Else
                'Iterare of the dataset and insert a new row, when the supplier changes - org specific break rule
                For i As Integer = 0 To dsWIP.Tables(0).Rows.Count + 20000
                    If i >= dsWIP.Tables(0).Rows.Count - 1 Then
                        Exit For
                    End If

                    If dsWIP.Tables(0).Rows(i).Item("RESOURCE_CODE").ToString <> resource_name Then
                        Dim row As System.Data.DataRow
                        row = dsWIP.Tables(0).NewRow

                        resource_name = dsWIP.Tables(0).Rows(i).Item("RESOURCE_CODE").ToString
                        dsWIP.Tables(0).Rows.InsertAt(row, i)
                    End If
                Next

            End If

            For i As Integer = 0 To dsWIP.Tables(0).Rows.Count - 1
                searchJobList.Add(New jobSearch(i, dsWIP.Tables(0).Rows(i).Item("RESOURCE_CODE").ToString & dsWIP.Tables(0).Rows(i).Item("JOB_NO").ToString))
            Next

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("loadWIP", ex, 1)
        End Try

    End Sub

    Private Sub createSearchListWIP()

        searchJobList.Clear()
        For i As Integer = 0 To dgvWipSupplierQueue.DataSource.Rows.Count - 1
            searchJobList.Add(New jobSearch(i, dgvWipSupplierQueue.Rows(i).Cells("RESOURCE_CODE").Value.ToString & dgvWipSupplierQueue.Rows(i).Cells("JOB_NO").Value.ToString))
        Next
    End Sub

    ''' <summary>
    ''' Procedure which binds the WIP dataSet to the datagridview.
    ''' </summary>
    ''' <remarks>Also updates the label of the datagrid.</remarks>
    Private Sub bindWipToGrid()

        Try
            Me.bottonGrid = "wip"
            'Debug.Print("start:" & Now)
            dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
            gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
            ' dgvWipSupplierQueue = dgvWIP

            'Debug.Print("formatted:" & Now)
            'See if a filter was applied earlier
            If dgvWipSupplierQueue.Rows.Count = lngCountWIPDiscreteJobsOperationSequences Then
                lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters " '& dgvWipSupplierQueue.Rows.Count 'Change request
            Else
                lblWIPPOFilter.Text = "Open WIP Operation Sequences: " & dgvWipSupplierQueue.Rows.Count & " / " & lngCountWIPDiscreteJobsOperationSequences & " Records Displayed"
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("bindWipToGrid", ex, 1)
        End Try
        
    End Sub

    Private Sub loadDataSets(Optional ByRef loadWindow As frmLoading = Nothing, Optional ByVal updateStatus As Boolean = False)
        Dim strPOView As String = " poQueueView "
        Dim strWIPView As String = " wipQueueView "
        Dim strSalesOrderLines As String = " salesOrderLinesView "

        'We have to distinct the planning types
        If Me.planningType = "ASCP" Then
            strPOView = " MSCpoQueueView "
            poBottonGrid = "MSCpoQueueView"
            strWIPView = " MSCwipQueueView "
            wipBottonGrid = "MSCwipQueueView"
            strSalesOrderLines = " MSCsalesOrderLinesView "
        End If

        'Replace the maximum sales order number
        'Apply the user rights
        'If loadWindow is Not Nothing
        If updateStatus Then
            loadWindow.statusBar = "Load Sales Orders"
            loadWindow.statusProgress = 55
            loadWindow.Update()
        End If
       
        Me.loadSalesOrder("")

        If updateStatus Then
            loadWindow.statusBar = "Load Purchase Orders"
            loadWindow.statusProgress = 60
            loadWindow.Update()
        End If
       
        Me.loadPO("")

        If updateStatus Then
            loadWindow.statusBar = "Load Discrete Jobs"
            loadWindow.statusProgress = 65
            loadWindow.Update()
        End If
        

        Me.loadWip("")

        Try
            

            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            Dim ds As New DataSet
            ds = db.SecureQueryParams("SELECT COUNT(*) FROM " & strSalesOrderLines & " WHERE organization_id = @1", Me.organizationId)
            lngCountSalesOrderLines = Convert.ToInt64(ds.Tables(0).Rows(0).Item(0).ToString())
            ds = db.SecureQueryParams("SELECT count(PONoLine) FROM " & strPOView & " WHERE ship_to_organization_id = @1 and PONoLine is not null", Me.organizationId)
            lngCountPurchaseOrderLines = Convert.ToInt64(ds.Tables(0).Rows(0).Item(0).ToString())
            ds = db.SecureQueryParams("SELECT count(JobNoOperationSeq) FROM " & strWIPView & " WHERE organization_id = @1 and JobNoOperationSeq is not null", Me.organizationId)
            lngCountWIPDiscreteJobsOperationSequences = Convert.ToInt64(ds.Tables(0).Rows(0).Item(0).ToString())

            'Load the default view of the user from startup, store the query strings within the containers, write the labels

            Dim dsDefault As New DataSet
            dsDefault = db.SecureQueryParams("SELECT  [relationID] " & _
                                              " ,[userFilterID] " & _
                                              " ,[userTemplateID] " & _
                                              " ,[filterStringUser] " & _
                                              " ,[filterNameUser] " & _
                                              " ,[filterStringTemplate] " & _
                                              " ,[filterNameTemplate] " & _
                                              " ,[isDefault] " & _
                                              " ,[entityUser] " & _
                                              " ,[entityTemplate] " & _
                                              " FROM [whUserFiltersDefaultView] WHERE userName = @1", Environment.UserName)

            For i As Integer = 0 To dsDefault.Tables(0).Rows.Count - 1
                If DBNull.Value.Equals(dsDefault.Tables(0).Rows(i).Item("entityUser")) Then
                    'Handle the template stuff
                    If dsDefault.Tables(0).Rows(i).Item("entityTemplate") = "salesOrderLinesView" Then
                        'load the values sales order line
                        Me.loadSalesOrder(dsDefault.Tables(0).Rows(i).Item("filterStringTemplate"))
                        Me.strFilterSalesOrder = dsDefault.Tables(0).Rows(i).Item("filterStringTemplate")
                        Me.strFilterSalesOrderName = dsDefault.Tables(0).Rows(i).Item("filterNameTemplate").ToString
                        lblSalesOrderHeader.Text = dsDefault.Tables(0).Rows(i).Item("filterNameTemplate").ToString & " - " & dsSalesOrder.Tables(0).Rows.Count & " / " & lngCountSalesOrderLines & " records displayed"
                    ElseIf dsDefault.Tables(0).Rows(i).Item("entityTemplate") = "POQueueView" Then
                        'load the values for po
                        Me.loadPO(dsDefault.Tables(0).Rows(i).Item("filterStringTemplate"))
                        Me.strFilterPO = dsDefault.Tables(0).Rows(i).Item("filterStringTemplate")
                        Dim numberOfRecords As Integer = Me.numberOfRecordsDisplayedForWipPO(dsPO.Tables(0))
                        lblWIPPOFilter.Text = dsDefault.Tables(0).Rows(i).Item("filterNameTemplate").ToString & " " & numberOfRecords & " / " & lngCountPurchaseOrderLines & " records displayed"
                    ElseIf dsDefault.Tables(0).Rows(i).Item("entityTemplate") = "wipQueueView" Then
                        Me.loadWip(dsDefault.Tables(0).Rows(i).Item("filterStringTemplate"))
                        Me.bindWipToGrid()
                        Me.strFilterWip = dsDefault.Tables(0).Rows(i).Item("filterStringTemplate")
                        Dim numberOfRecorsds As Integer = Me.numberOfRecordsDisplayedForWipPO(dsWIP.Tables(0))
                        lblWIPPOFilter.Text = dsDefault.Tables(0).Rows(i).Item("filterNameTemplate").ToString & " " & numberOfRecorsds.ToString & " / " & lngCountWIPDiscreteJobsOperationSequences & " records displayed"
                    End If
                Else
                    'Handle the user filter stuff
                    If dsDefault.Tables(0).Rows(i).Item("entityUser") = "SalesOrderLinesView" Then
                        'load the values sales order line
                        Me.loadSalesOrder(dsDefault.Tables(0).Rows(i).Item("filterStringUser"))
                        Me.strFilterSalesOrder = dsDefault.Tables(0).Rows(i).Item("filterStringUser")
                        lblSalesOrderHeader.Text = dsDefault.Tables(0).Rows(i).Item("filterNameUser").ToString & " - " & dsSalesOrder.Tables(0).Rows.Count & " / " & lngCountSalesOrderLines & " records displayed"
                    ElseIf dsDefault.Tables(0).Rows(i).Item("entityUser") = "POQueueView" Then
                        'load the values for po
                        Me.loadPO(dsDefault.Tables(0).Rows(i).Item("filterStringUser"))
                        Me.strFilterPO = dsDefault.Tables(0).Rows(i).Item("filterStringUser")
                        Dim numberOfRecords As Integer = Me.numberOfRecordsDisplayedForWipPO(dsPO.Tables(0))
                        lblWIPPOFilter.Text = dsDefault.Tables(0).Rows(i).Item("filterNameUser").ToString & " " & numberOfRecords.ToString & " / " & lngCountPurchaseOrderLines & " records displayed"
                    ElseIf dsDefault.Tables(0).Rows(i).Item("entityUser") = "WIPQueueView" Then
                        Me.loadWip(dsDefault.Tables(0).Rows(i).Item("filterStringUser"))
                        Me.bindWipToGrid()
                        Me.strFilterWip = dsDefault.Tables(0).Rows(i).Item("filterStringUser")
                        Dim numberOfRecorsds As Integer = Me.numberOfRecordsDisplayedForWipPO(dsWIP.Tables(0))
                        lblWIPPOFilter.Text = dsDefault.Tables(0).Rows(i).Item("filterNameUser").ToString & " " & numberOfRecorsds & " / " & lngCountWIPDiscreteJobsOperationSequences & " records displayed"
                    End If
                End If

            Next

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("loadDefaultStartup", ex, 1)
        End Try
    End Sub

    ''' <summary>
    ''' Handles the click logic for the drill down on the data grid view
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub dgvSalesOrders_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSalesOrders.CellClick

        dgvFurtherDetails.Visible = False

        'Implement comment popup
        Dim name As String = ""

        If Not e.ColumnIndex = -1 AndAlso Not e.RowIndex = -1 Then

            'Display the comment form when single line is selected
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "lastComment" And Not IsCtrlKeyPressed Then
                If Not e.RowIndex = -1 Then
                    Dim dlg As New dlgComments(dgvSalesOrders.Rows(e.RowIndex).Cells("HEADER_ID").Value, dgvSalesOrders.Rows(e.RowIndex).Cells("ORDER_NO").Value, dgvSalesOrders.Rows(e.RowIndex).Cells("LINE_NO").Value, 0, "salesOrderLinesView", dgvSalesOrders, e.RowIndex, Me.organizationId)
                    dlg.ShowDialog()
                    If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                        Me.loadCurrentNotifications()
                    End If
                End If

            End If

            'Display the forecast dialogue
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "lastForecast" Then
                Try
                    If Not e.RowIndex = -1 Then
                        Dim dlg As New dlgForecastDates(dgvSalesOrders.Rows(e.RowIndex).Cells("HEADER_ID").Value, dgvSalesOrders.Rows(e.RowIndex).Cells("ORDER_NO").Value, dgvSalesOrders.Rows(e.RowIndex).Cells("LINE_NO").Value, 0, "salesOrderLinesView", dgvSalesOrders, e.RowIndex, Me.organizationId, 0, dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_NO").Value.ToString)
                        dlg.ShowDialog()
                        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                            Me.loadCurrentNotifications()
                        End If
                    End If
                Catch ex As Exception

                End Try

            End If

            'Click on a sales order to show the shortages for this sales order
            'Implementation of the order.
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "JOB_PO" Then

                If Not DBNull.Value.Equals(dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO").Value) Then
                    If dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO_STATUS").Value.ToString.Trim = "R" Or dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO_STATUS").Value.ToString.Trim = "U" Then

                        Debug.Print(Now)
                        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
                        Dim ds As New DataSet   'Why to query the backend again here?
                        ds = db.SecureQueryParams("SELECT resource_code FROM WIPQueueVIEW WHERE organization_id = @1 " & _
                                " AND job_no = @2 " & _
                                " ORDER BY operation_seq_no ASC", Me.organizationId, dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO").Value)
                        Try
                            'Can also be the op1 content!
                            Me.searchJob(ds.Tables(0).Rows(0).Item(0), dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO").Value)

                        Catch ex As Exception
                            Dim err As New clsExceptionManagement
                            err.createErrorLog("salesOrderClick", ex, 1)
                        End Try

                    ElseIf dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO_STATUS").Value.ToString.Trim = "" Then
                        Dim strPo As String
                        Dim strLine As String
                        Dim splitPoint As Integer
                        Dim length As Integer = dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO").Value.ToString.Length
                        splitPoint = dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO").Value.ToString.IndexOf("_")

                        strPo = dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO").Value.ToString.Substring(0, splitPoint)
                        strLine = dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO").Value.ToString.Substring(splitPoint + 1, length - 1 - splitPoint)
                        'Gather the supplier

                        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
                        Dim ds As New DataSet
                        ds = db.SecureQueryParams("SELECT vendor_name FROM POQueueVIEW WHERE  " & _
                                "  po_no = @1  AND line_no = @2 " & _
                                " ", strPo, strLine)
                        Try
                            Me.searchPO(ds.Tables(0).Rows(0).Item(0), strPo, strLine)
                        Catch ex As Exception
                            Dim err As New clsExceptionManagement
                            err.createErrorLog("salesOrderClick", ex, 1)
                        End Try

                    End If
                End If

            End If

            'Click on a sales order to show the shortages for this sales order
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "ORDER_NO" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OrderNoLine" Then

                If Not DBNull.Value.Equals(dgvSalesOrders.Rows(e.RowIndex).Cells("demand_id").Value) AndAlso Not DBNull.Value.Equals(dgvSalesOrders.Rows(e.RowIndex).Cells("organization_id").Value) Then
                    If dgvSalesOrders.SelectedCells.Count = 1 Then
                        'just dive into shortages when one order line is selected
                        Me.populateMRPShortages(dgvSalesOrders.Rows(e.RowIndex).Cells("demand_id").Value, dgvSalesOrders.Rows(e.RowIndex).Cells("organization_id").Value, "")
                        lnDemandId = dgvSalesOrders.Rows(e.RowIndex).Cells("demand_id").Value   'Save the demand id
                    Else
                        'dive into more than one shortage when more than on shortage is selected.
                        Dim strFilter As String = "("
                        For i As Integer = 0 To dgvSalesOrders.SelectedCells.Count - 1
                            If dgvSalesOrders.Columns(dgvSalesOrders.SelectedCells(i).ColumnIndex).Name = "OrderNoLine" Then
                                If Not DBNull.Value.Equals(dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(i).RowIndex).Cells("demand_id").Value) Then
                                    If strFilter.Count > 3 Then strFilter += ","
                                    strFilter += dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(i).RowIndex).Cells("demand_id").Value.ToString
                                End If
                            End If


                        Next
                        strFilter += ")"
                        Me.populateMRPShortages(dgvSalesOrders.Rows(e.RowIndex).Cells("demand_id").Value, Me.organizationId, "", 0, strFilter)
                    End If
                    lnDemandId = dgvSalesOrders.Rows(e.RowIndex).Cells("demand_id").Value
                End If

            End If

            'Click on the item number to see the current suppply and demand for an item.
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "ITEM_NO" Then
                If Not e.RowIndex = -1 Then
                    Me.populateSupplyDemand(dgvSalesOrders.Rows(e.RowIndex).Cells("item_no").Value)
                End If
            End If

            'Jump to job or po when operation 1 to 4 are selected
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP1" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP2" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP3" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP4" Then
                If Not e.RowIndex = -1 Then
                    'determine if the user clicked on a PO, or on a Job
                    If dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP1" And Not DBNull.Value.Equals(dgvSalesOrders.Rows(e.RowIndex).Cells("OP1").Value) AndAlso Not DBNull.Value.Equals(dgvSalesOrders.Rows(e.RowIndex).Cells("PO_NO").Value) Then
                        'po
                        If dsPO.Tables(0).Rows.Count < lngCountPurchaseOrderLines Then
                            If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                Me.loadPO("")
                                lblWIPPOFilter.Text = "Open Purchase Order Lines - No Filters"
                            End If
                        End If
                        Me.searchPO(dgvSalesOrders.Rows(e.RowIndex).Cells("OP1").Value.ToString, dgvSalesOrders.Rows(e.RowIndex).Cells("po_no").Value, dgvSalesOrders.Rows(e.RowIndex).Cells("po_line").Value)  'assumes that the vendor name only goes in Op1, based on xform process
                    Else
                        'job
                        If dsWIP.Tables(0).Rows.Count < lngCountWIPDiscreteJobsOperationSequences Then
                            If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                Me.loadWip("")
                                lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters"
                            End If
                        End If
                        Me.searchJob(dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString, dgvSalesOrders.Rows(e.RowIndex).Cells("job_no").Value.ToString)
                    End If
                End If
            End If

            'Open the new date dialogue when the user clicks the job start date
            Dim rolesObj As New clsUserControl
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "JOB_START_DATE" AndAlso Not DBNull.Value.Equals(dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                If rolesObj.userInRole(Environment.UserName, "Planning") Then
                    'Dim dlgWipStartDate As New dlgNewDate(dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, dgvWipSupplierQueue.DataSource, "FIRST_UNIT_START_DATE", e.RowIndex, "WIPQueueView", dgvWipSupplierQueue.Rows(e.RowIndex).Cells("WIP_ENTITY_ID").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("OPERATION_SEQ_NO").Value, Me.organizationId)
                    Dim dlgWipStartDate As New dlgNewDate(dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, dgvSalesOrders.DataSource, "JOB_START_DATE", e.RowIndex, "wipQueueView", dgvSalesOrders.Rows(e.RowIndex).Cells("JOB_PO").Value, 10, Me.organizationId, dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
                    dlgWipStartDate.ShowDialog()
                End If
            End If
        End If
        IsCtrlKeyPressed = False
    End Sub


    Private m_listOfSalesOrderLines As String()
    Private m_clickedSalesOrderLineContent As String
    Private m_currentClickedIndex As Integer
    Private bPOIntransitShipment As Boolean = False

	Private Sub dgvShortages_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvShortages.CellClick
        Try

        
            dgvFurtherDetails.Visible = False 'hide the popup grid if the user clicks on the shortage grid

            If Not e.ColumnIndex = -1 AndAlso Not e.RowIndex = -1 AndAlso Not DBNull.Value.Equals(dgvShortages.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then    'make sure user didn't click on a header column or row, or an empty cell
                'logic to understand if we did hit a purchase order requisiton
                Dim bPORequisition As Boolean = False
                If strMiddleGrid = "MRPShortagesView" Then
                    If dgvShortages.Rows(e.RowIndex).Cells("SUPPLY_TYPE").Value.ToString = "Purchase requisition" Then 'code section with issues as it is sometimes not existing
                        bPORequisition = True
                    ElseIf dgvShortages.Rows(e.RowIndex).Cells("SUPPLY_TYPE").Value.ToString = "Intransit shipment" Then
                        bPOIntransitShipment = True

                    End If
                End If


                'Show item supply / demand when a user clicks the item column
                If strMiddleGrid = "MRPShortagesView" Then
                    If dgvShortages.Columns(e.ColumnIndex).Name = "ITEM_NO" Then Me.populateSupplyDemand(dgvShortages.Rows(e.RowIndex).Cells("ITEM_NO").Value.ToString)
                    'Jump to the customer order from planned order view
                    If dgvShortages.Columns(e.ColumnIndex).Name = "OrderNoLine" Then
                        If dsSalesOrder.Tables(0).Rows.Count < lngCountSalesOrderLines Then
                            If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                Me.loadSalesOrder("")
                                lblSalesOrderHeader.Text = "Open Sales Orders - No Filters"
                            End If
                        End If
                        Me.searchSalesOrder(dgvShortages.Rows(e.RowIndex).Cells("ORDER_NO").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("LINE_NO").Value.ToString, False)
                    End If

                End If

                'Jump to the sales order when the user clicks the sales order number on the supply and demand grid
                If strMiddleGrid = "supplyDemandMRPView" Then

                    'Jump to the distinct demanding sales order lines
                    If dgvShortages.Columns(e.ColumnIndex).Name = "listOfSalesOrderLines" Then
                        If m_clickedSalesOrderLineContent = dgvShortages.Rows(e.RowIndex).Cells("listOfSalesOrderLines").Value.ToString AndAlso m_currentClickedIndex + 1 < m_listOfSalesOrderLines.Count Then
                            m_listOfSalesOrderLines = dgvShortages.Rows(e.RowIndex).Cells("listOfSalesOrderLines").Value.ToString.Split("|")
                            Dim searchAdvice As String() = m_listOfSalesOrderLines(m_currentClickedIndex).ToString.Split("_")
                            Dim strLine As String() = searchAdvice(1).ToString.Split(":")
                            Me.searchSalesOrder(searchAdvice(0), strLine(0).Trim, True)

                            m_currentClickedIndex += 1
                        Else
                            m_clickedSalesOrderLineContent = dgvShortages.Rows(e.RowIndex).Cells("listOfSalesOrderLines").Value.ToString
                            m_currentClickedIndex = 0
                            m_listOfSalesOrderLines = dgvShortages.Rows(e.RowIndex).Cells("listOfSalesOrderLines").Value.ToString.Split("|")
                            Dim searchAdvice As String() = m_listOfSalesOrderLines(m_currentClickedIndex).ToString.Split("_")
                            Dim strLine As String() = searchAdvice(1).ToString.Split(":")
                            Me.searchSalesOrder(searchAdvice(0), strLine(0).Trim, True)
                            m_currentClickedIndex += 1
                        End If
                    End If

                    If dgvShortages.Columns(e.ColumnIndex).Name = "SalesOrderLine" Then
                        If dsSalesOrder.Tables(0).Rows.Count < lngCountSalesOrderLines Then
                            If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                Me.loadSalesOrder("")
                                lblSalesOrderHeader.Text = "Open Sales Orders - No Filters"
                            End If
                        End If
                        Me.searchSalesOrder(dgvShortages.Rows(e.RowIndex).Cells("ORDER_NO").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("LINE_NO").Value.ToString)
                    End If
                    If dgvShortages.Columns(e.ColumnIndex).Name = "parentJobOrderNo" Then
                        If dsWIP.Tables(0).Rows.Count < lngCountWIPDiscreteJobsOperationSequences Then
                            If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                Me.loadWip("")
                                lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters"
                            End If
                        End If
                        Me.searchJob("*", dgvShortages.Rows(e.RowIndex).Cells("parentJobOrderNo").Value.ToString)
                    End If
                    '*** have to enable searching just for the job
                    'If dgvShortages.Columns(e.ColumnIndex).Name = "parentJobOrderNo" Then
                    '    Me.searchJob(dgvShortages.Rows(e.RowIndex).Cells("OP1").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("job_no").Value.ToString)  'if the user clicked on the job number, send the first operation resource as the search parameter
                    'End If




                End If

                'Jump to job or po when operation 1 to 4 are selected
                If dgvShortages.Columns(e.ColumnIndex).Name = "OP1" Or dgvShortages.Columns(e.ColumnIndex).Name = "OP2" Or dgvShortages.Columns(e.ColumnIndex).Name = "OP3" Or dgvShortages.Columns(e.ColumnIndex).Name = "OP4" Or dgvShortages.Columns(e.ColumnIndex).Name = "JobPOLine" Then
                    'determine if the user clicked on a PO, or on a Job
                    Dim strRelease As String
                    Dim strShipment As String

                    If Not DBNull.Value.Equals(dgvShortages.Rows(e.RowIndex).Cells("RELEASE_NO").Value) Then
                        strRelease = dgvShortages.Rows(e.RowIndex).Cells("RELEASE_NO").Value.ToString
                    Else
                        strRelease = 0
                    End If

                    If Not DBNull.Value.Equals(dgvShortages.Rows(e.RowIndex).Cells("SHIPMENT_NO").Value) Then
                        strShipment = dgvShortages.Rows(e.RowIndex).Cells("SHIPMENT_NO").Value.ToString
                    Else
                        strShipment = 0
                    End If

                    If (dgvShortages.Columns(e.ColumnIndex).Name = "OP1" Or dgvShortages.Columns(e.ColumnIndex).Name = "JobPOLine") And Not DBNull.Value.Equals(dgvShortages.Rows(e.RowIndex).Cells("OP1").Value) AndAlso Not DBNull.Value.Equals(dgvShortages.Rows(e.RowIndex).Cells("PURCHASE_ORDER_NO").Value) Then  'if the user clicked on Op1 or the job/po and there is a PO, then search for the PO
                        'po
                        If dsPO.Tables(0).Rows.Count < lngCountPurchaseOrderLines Then
                            If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                Me.loadPO("")
                                lblWIPPOFilter.Text = "Open Purchase Order Lines - No Filters"
                            End If
                        End If
                        'Take care about the dbnull in release and shipment
                        
                        Me.searchPO(dgvShortages.Rows(e.RowIndex).Cells("OP1").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("PURCHASE_ORDER_NO").Value, dgvShortages.Rows(e.RowIndex).Cells("PURCH_LINE_NO").Value, strRelease, strShipment)  'assumes that the vendor name only goes in Op1, based on xform process 'need to jump to the correct release and shipment
                    ElseIf (dgvShortages.Columns(e.ColumnIndex).Name = "OP1" Or dgvShortages.Columns(e.ColumnIndex).Name = "JobPOLine") AndAlso Not DBNull.Value.Equals(dgvShortages.Rows(e.RowIndex).Cells("PURCHASE_ORDER_NO").Value) Then  'if the user clicked on Op1 or the job/po and there is a PO, then search for the PO
                        Me.searchPO(dgvShortages.Rows(e.RowIndex).Cells("OP1").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("PURCHASE_ORDER_NO").Value, dgvShortages.Rows(e.RowIndex).Cells("PURCH_LINE_NO").Value, strRelease, strShipment)

                    Else
                        'job
                        'AndAlso dgvShortages.Rows(e.RowIndex).Cells("SUPPLY_TYPE").Value.ToString <> "Purchase requisition"

                        If dgvShortages.Columns(e.ColumnIndex).Name = "JobPOLine" Then
                            If dsWIP.Tables(0).Rows.Count < lngCountWIPDiscreteJobsOperationSequences AndAlso Not bPORequisition AndAlso Not bPOIntransitShipment Then
                                If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    Me.loadWip("")
                                    lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters"
                                End If
                            End If
                            If Not bPORequisition Then
                                Me.searchJob(dgvShortages.Rows(e.RowIndex).Cells("OP1").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("job_no").Value.ToString)  'if the user clicked on the job number, send the first operation resource as the search parameter

                            End If
                            'if the user clicked on the job number, send the first operation resource as the search parameter
                        Else
                            If dsWIP.Tables(0).Rows.Count < lngCountWIPDiscreteJobsOperationSequences Then
                                If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                                    Me.loadWip("")
                                    lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters"
                                End If
                            End If
                            If Not bPORequisition Then
                                Me.searchJob(dgvShortages.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("job_no").Value.ToString)  'otherwise send the resource that the user clicked on
                            End If
                            ' If dgvShortages.Rows(e.RowIndex).Cells("SUPPLY_TYPE").Value.ToString <> "Purchase requisition" Then 'code section with issues as it is sometimes not existing
                            'End If
                        End If
                    End If
                End If



            End If

            If Not e.ColumnIndex = -1 AndAlso Not e.RowIndex = -1 Then
                'Open comment section if we have a planned order
                If dgvShortages.Columns(e.ColumnIndex).Name = "lastComment" And Not IsCtrlKeyPressed Then
                    If dgvShortages.Rows(e.RowIndex).Cells("SUPPLY_TYPE").Value.ToString = "Planned order" _
                    Or dgvShortages.Rows(e.RowIndex).Cells("SUPPLY_TYPE").Value.ToString = "PO in receiving" _
                    Or dgvShortages.Rows(e.RowIndex).Cells("SUPPLY_TYPE").Value.ToString = "Purchase requisition" _
                    Or dgvShortages.Rows(e.RowIndex).Cells("SUPPLY_TYPE").Value.ToString = "Intransit shipment" Then
                        Try


                            Dim dlg As New dlgComments(dgvShortages.Rows(e.RowIndex).Cells("ORDER_NO").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("ORDER_NO").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("LINE_NO").Value.ToString, dgvShortages.Rows(e.RowIndex).Cells("INVENTORY_ITEM_ID").Value.ToString, "plannedOrder", dgvShortages, e.RowIndex, Me.organizationId)
                            dlg.ShowDialog()
                            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                                Me.loadCurrentNotifications()
                            End If
                        Catch ex As Exception

                        End Try
                    Else
                        MessageBox.Show("Comments in the shortage section are just possible for planned orders.")
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
        IsCtrlKeyPressed = False
    End Sub

    Private m_dateToChange As Date
    Public Property dateToChange() As Date
        Get
            Return m_dateToChange
        End Get
        Set(ByVal value As Date)
            m_dateToChange = value
        End Set
    End Property

    ''' <summary>
    ''' Handles the click logic for the po or wip grid
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub dgvWipSupplierQueue_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWipSupplierQueue.CellClick
        dgvFurtherDetails.Visible = False

        If Not e.ColumnIndex = -1 And Not e.RowIndex = -1 Then

            'Handle the new dateTime change of the promise Date
            'Also checks the role of the user
            Dim rolesObj As New clsUserControl
            If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "PROMISED_DATE" _
                AndAlso rolesObj.userInRole(Environment.UserName, "Purchasing") AndAlso dgvWipSupplierQueue.Rows(e.RowIndex).Cells("APPROVED_FLAG").Value.ToString = "Y" Then

                If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then
                    Me.dateToChange = dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
                End If

                Dim dlgPromDate As New dlgNewDate(dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString, dgvWipSupplierQueue.DataSource, _
                                                  "PROMISED_DATE", e.RowIndex, "POQueueView", dgvWipSupplierQueue.Rows(e.RowIndex).Cells("PO_NO").Value.ToString, _
                                                  dgvWipSupplierQueue.Rows(e.RowIndex).Cells("LINE_NO").Value, Me.organizationId, Me, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("release_no").Value.ToString, _
                                                  dgvWipSupplierQueue.Rows(e.RowIndex).Cells("SHIPMENT_NO").Value.ToString)
                dlgPromDate.ShowDialog()
                If dlgPromDate.DialogResult = Windows.Forms.DialogResult.OK Then
                    dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Me.dateToChange
                End If

            ElseIf dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "PROMISED_DATE" _
            AndAlso rolesObj.userInRole(Environment.UserName, "Purchasing") AndAlso dgvWipSupplierQueue.Rows(e.RowIndex).Cells("APPROVED_FLAG").Value.ToString <> "Y" Then
                MessageBox.Show("Purchase Order has not been approved.")
            End If

            If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "FIRST_UNIT_START_DATE" AndAlso Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) _
                AndAlso rolesObj.userInRole(Environment.UserName, "Planning") Then
				'Dim dlgWipStartDate As New dlgNewDate(dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, dgvWipSupplierQueue.DataSource, "FIRST_UNIT_START_DATE", e.RowIndex, "WIPQueueView", dgvWipSupplierQueue.Rows(e.RowIndex).Cells("WIP_ENTITY_ID").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("OPERATION_SEQ_NO").Value, Me.organizationId)
                Dim dlgWipStartDate As New dlgNewDate(dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, dgvWipSupplierQueue.DataSource, "FIRST_UNIT_START_DATE", e.RowIndex, "WIPQueueView", dgvWipSupplierQueue.Rows(e.RowIndex).Cells("JOB_NO").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("OPERATION_SEQ_NO").Value, Me.organizationId, dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
				dlgWipStartDate.ShowDialog()
            End If

            If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "lastComment" And Not IsCtrlKeyPressed Then
                'Display the comment form
                'Distinct between wip discrete jobs and purchase orders
                If Me.bottonGrid = "po" Then

                    If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("PO_HEADER_ID").Value) Then
                        'Invalid cast exceptio when a value is missing.
                        Dim dlg As dlgComments

                        If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("RELEASE_NO").Value) Then
                            dlg = New dlgComments(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("PO_HEADER_ID").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("PO_NO").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("LINE_NO").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("SHIPMENT_NO").Value, "POQueueView", dgvWipSupplierQueue, e.RowIndex, Me.organizationId, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("RELEASE_NO").Value.ToString)
                        Else
                            dlg = New dlgComments(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("PO_HEADER_ID").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("PO_NO").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("LINE_NO").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("SHIPMENT_NO").Value, "POQueueView", dgvWipSupplierQueue, e.RowIndex, Me.organizationId, 0)
                        End If

                        dlg.ShowDialog()
                        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                            Me.loadCurrentNotifications()
                        End If
                    End If


                Else
                    If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("WIP_ENTITY_ID").Value) Then
                        Dim dlg As New dlgComments(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("WIP_ENTITY_ID").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("JOB_NO").Value, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("OPERATION_SEQ_NO").Value, 0, "WIPQueueView", dgvWipSupplierQueue, e.RowIndex, Me.organizationId)
                        dlg.ShowDialog()
                        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                            Me.loadCurrentNotifications()
                        End If
                    End If


                End If

            End If

            'Show the pegging to a job, when a unreleased Job is clicked.
            If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "MEANING" Then
                If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("end_demand_id").Value) AndAlso Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("firstPeggingId").Value) Then
                    Me.populateMRPShortages(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("end_demand_id").Value, Me.organizationId, "", dgvWipSupplierQueue.Rows(e.RowIndex).Cells("firstPeggingId").Value)
                    lnDemandId = dgvWipSupplierQueue.Rows(e.RowIndex).Cells("end_demand_id").Value
                    'View the tooltip on hover action?
                Else
                    dgvShortages.DataSource = Nothing
                End If

            End If

            'Show supply and demand, when a item number is clicked
            If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "ITEM_NO" Then
                Me.populateSupplyDemand(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("ITEM_NO").Value.ToString)
            End If


            'Selected the sales order column induces jump on the top grid
            If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "SalesOrderLine" Then
                If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("sales_order_no").Value) AndAlso Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("SALES_LINE_NO").Value) Then
                    If dsSalesOrder.Tables(0).Rows.Count < lngCountSalesOrderLines Then
                        If MessageBox.Show("Do you want to remove filter applied on target grid?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                            Me.loadSalesOrder("")
                            lblSalesOrderHeader.Text = "Open Sales Orders - No Filters"
                        End If
                    End If
                    searchSalesOrder(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("sales_order_no").Value.ToString, dgvWipSupplierQueue.Rows(e.RowIndex).Cells("SALES_LINE_NO").Value.ToString, False)
                    lblShortageDescription.Text = "Shortages For Order: " & dgvWipSupplierQueue.Rows(e.RowIndex).Cells("sales_order_no").Value.ToString & "  Line: " & dgvWipSupplierQueue.Rows(e.RowIndex).Cells("SALES_LINE_NO").Value.ToString

                End If

            End If

            'Select the details of the job, so the user do not need to jump across the operations
            If Me.bottonGrid = "wip" Then
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "JobNoOperationSeq" Then
                    If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("Job_No").Value) Then
                        Me.populateJobsDetails(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("Job_No").Value)
                    End If
                End If
            End If
        End If
        IsCtrlKeyPressed = False

    End Sub

    Private Sub populateJobsDetails(ByVal jobNo As String)
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet

        ds = db.SecureQueryParams("SELECT CONVERT(VARCHAR,jobs.JOB_NO) AS JOB_NO " & _
            ", jobs.WIP_ENTITY_ID        " & _
            ", jobs.OPERATION_SEQ_NO     " & _
            ", jobs.RESOURCE_CODE	     " & _
            ", jobs.SCHEDULED_QUANTITY	 " & _
            ", jobs.QUANTITY_IN_QUEUE	 " & _
            ", jobs.QUANTITY_SCRAPPED		 " & _
            ", jobs.QUANTITY_COMPLETED	 " & _
            ", jobs.DATE_LAST_MOVED     " & _
            ", (SELECT     TOP (1) DESCRIPTION " & _
            "                           FROM         comments AS comm " & _
            "                           WHERE      (jobs.WIP_ENTITY_ID = comm.HEADER_ID) AND (jobs.OPERATION_SEQ_NO = comm.LINE_ID) AND (comm.ENTITY_ID = 'wip_discrete_jobs') " & _
            "                           ORDER BY comm.CREATION_DATE DESC) AS lastComment " & _
            "       FROM saveJobs jobs " & _
            " WHERE organization_id = @1 AND job_no = @2 " & _
            " ORDER BY JOB_NO, OPERATION_SEQ_NO ", Me.organizationId, jobNo)

        Try
            dgvFurtherDetails.DataSource = ds.Tables(0)
            gridDisplay.formatGrid(dgvFurtherDetails, "jobFurtherDetails")
            dgvFurtherDetails.Visible = True

            'Crashes for example with a slow connection
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("salesOrderMenuFilterClick", ex, 1)
        End Try


    End Sub



    Private Sub deleteTemporaryFilters(ByVal bSalesOrder As Boolean, ByVal bMRPShortages As Boolean, ByVal bPurchaseOrders As Boolean, ByVal bWipDiscreteJobs As Boolean)
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet

        If bSalesOrder Then
            ds = db.SecureQueryParams("SELECT filterId FROM warehouseUserFilters WHERE userName = @1 AND entity = 'salesOrderLinesView' AND filterName = 'TEMP' ", Environment.UserName)
            If ds.Tables(0).Rows.Count > 0 Then
                db.SecureNonQueryParams("DELETE FROM warehouseUserFilterDetail WHERE filterId = @1", ds.Tables(0).Rows(0).Item(0))
                db.SecureNonQueryParams("DELETE FROM warehouseUserFilters WHERE filterId = @1", ds.Tables(0).Rows(0).Item(0))
            End If
        End If

        If bMRPShortages Then
            ds = db.SecureQueryParams("SELECT filterId FROM warehouseUserFilters WHERE userName = @1 AND entity = 'MRPShortagesView' AND filterName = 'TEMP' ", Environment.UserName)
            If ds.Tables(0).Rows.Count > 0 Then
                db.SecureNonQueryParams("DELETE FROM warehouseUserFilterDetail WHERE filterId = @1", ds.Tables(0).Rows(0).Item(0))
                db.SecureNonQueryParams("DELETE FROM warehouseUserFilters WHERE filterId = @1", ds.Tables(0).Rows(0).Item(0))
            End If
        End If

        If bPurchaseOrders Then
            ds = db.SecureQueryParams("SELECT filterId FROM warehouseUserFilters WHERE userName = @1 AND entity = 'POQueueView' AND filterName = 'TEMP' ", Environment.UserName)
            If ds.Tables(0).Rows.Count > 0 Then
                db.SecureNonQueryParams("DELETE FROM warehouseUserFilterDetail WHERE filterId = @1", ds.Tables(0).Rows(0).Item(0))
                db.SecureNonQueryParams("DELETE FROM warehouseUserFilters WHERE filterId = @1", ds.Tables(0).Rows(0).Item(0))
            End If
        End If

        If bWipDiscreteJobs Then
            ds = db.SecureQueryParams("SELECT filterId FROM warehouseUserFilters WHERE userName = @1 AND entity = 'WIPQueueView' AND filterName = 'TEMP' ", Environment.UserName)
            If ds.Tables(0).Rows.Count > 0 Then
                db.SecureNonQueryParams("DELETE FROM warehouseUserFilterDetail WHERE filterId = @1", ds.Tables(0).Rows(0).Item(0))
                db.SecureNonQueryParams("DELETE FROM warehouseUserFilters WHERE filterId = @1", ds.Tables(0).Rows(0).Item(0))
            End If
        End If

        Me.reloadUserViews()

    End Sub


    Private Sub cboOrganization_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboOrganization.SelectedIndexChanged

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        If Not loadFirstTime Then
            Dim userRights As New clsUserControl()

            'Look if the user is not able to see sites values
            If userRights.userRejectedForSite(Environment.UserName, cboOrganization.SelectedValue) Then
                MessageBox.Show("You are not allowed to view this site. Please contact your system administrator.")
                cboOrganization.SelectedValue = Me.organizationId
            Else
                Me.deleteTemporaryFilters(True, True, True, True)

                db.SecureNonQueryParams("UPDATE [warehouse_users] SET [standard_organization_id] = @1 WHERE [user_Name] = @2", _
                                    cboOrganization.SelectedValue, Environment.UserName)
                Dim ds As New DataSet

                ds = db.SecureQueryParams("SELECT operatingUnitId,transformationRun, planningType  FROM whOrganizationDefinition WHERE organizationId = @1", cboOrganization.SelectedValue)

                If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("transformationRun")) Then
                    Me.updateTime = ds.Tables(0).Rows(0).Item("transformationRun")
                End If
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("planningType")) Then
                    planningType = ds.Tables(0).Rows(0).Item("planningType")
                Else
                    planningType = "MRP"

                End If

                db.SecureNonQueryParams("UPDATE [warehouse_users] SET [standard_operating_unit_id] = @1 WHERE [user_Name] = @2", _
                                    ds.Tables(0).Rows(0).Item("operatingUnitId"), Environment.UserName)

                populateGrids()
                dgvSalesOrders.Focus()
                dgvSalesOrders.Select()


                'Reset the filter temporary and normal filter strings
                Me.resetFilterStrings()
                Me.resetcolumnHeaders()
                Me.reloadUserViews()
                Me.calculateIndicators()

            End If
            
        End If

        Me.resetSearchCapabilities()
        Me.loadCurrentNotifications()


    End Sub


    ''' <summary>
    ''' Procedure which load the complete information to the workbench.
    ''' </summary>
    ''' <param name="organizationId"></param>
    ''' <remarks></remarks>
    Private Sub loadComleteWorkbench(ByVal organizationId As Integer)
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        Me.deleteTemporaryFilters(True, True, True, True)

    
        Dim ds As New DataSet

        ds = db.SecureQueryParams("SELECT operatingUnitId,transformationRun  FROM whOrganizationDefinition WHERE organizationId = @1", organizationId)

        'Make sure we are not switching organization  from the reload
        db.SecureNonQueryParams("UPDATE [warehouse_users] SET [standard_organization_id] = @1 WHERE [user_Name] = @2", _
                                    Me.organizationId, Environment.UserName)

        If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("transformationRun")) Then
            Me.updateTime = ds.Tables(0).Rows(0).Item("transformationRun")
        End If


        populateGrids()
        dgvSalesOrders.Focus()
        dgvSalesOrders.Select()
        'Reset the filter temporary and normal filter strings
        Me.resetFilterStrings()
        Me.resetcolumnHeaders()
        Me.reloadUserViews()
        Me.calculateIndicators()

        Me.resetSearchCapabilities()
        Me.loadCurrentNotifications()
    End Sub

    ''' <summary>
    ''' Subroutine which resets the column header colours of the three grids
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub resetcolumnHeaders()
        dgvSalesOrders.EnableHeadersVisualStyles = True
        dgvWipSupplierQueue.EnableHeadersVisualStyles = True
        dgvShortages.EnableHeadersVisualStyles = True

    End Sub

    ''' <summary>
    ''' Resets all the stored filters for the user.
    ''' </summary>
    ''' <remarks>Next version may save the latest filter setings on user and organization level, so when he comes back the program remembers the state.</remarks>
    Private Sub resetFilterStrings()

        Me.strFilterEntity = ""
        Me.strFilterMRPShortagesTemp = ""
        Me.strFilterPO = ""
        Me.strFilterPOTemp = ""
        Me.strFilterSalesOrder = ""
        Me.strFilterSalesOrderTemp = ""
        Me.strFilterWip = ""
        Me.strFilterWIPTemp = ""

    End Sub

    ''' <summary>
    ''' Searches a sales order from the lower level shortages.
    ''' </summary>
    ''' <param name="salesOrder">The sales order we should search for.</param>
    ''' <param name="salesOrderLine">The sales order line the procedure should search for.</param>
    ''' <remarks>This is called from po or job grid to automatically populate shortages from there.</remarks>
    Private Overloads Sub searchSalesOrder(ByVal salesOrder As String, ByVal salesOrderLine As String)

        For i As Integer = 0 To dgvSalesOrders.Rows.Count - 1
            If dgvSalesOrders.Rows(i).Cells("ORDER_NO").Value.ToString.Contains(salesOrder) Then
                If dgvSalesOrders.Rows(i).Cells("LINE_NO").Value.ToString = salesOrderLine Then
                    dgvSalesOrders.Rows(i).Selected = True
                    dgvSalesOrders.FirstDisplayedScrollingRowIndex = i
                    'Populate the supply demand for this sales order
                    Try
                        If Not Me.strMiddleGrid = "supplyDemandMRPView" Then
                            Me.populateMRPShortages(dgvSalesOrders.Rows(i).Cells("demand_id").Value, dgvSalesOrders.Rows(i).Cells("organization_id").Value, "")
                        End If

                        Me.lnDemandId = dgvSalesOrders.Rows(i).Cells("demand_id").Value


                    Catch ex As Exception
                        Dim err As New clsExceptionManagement
                        err.createErrorLog("toolStripMenuItemBuy", ex, 1)
                    End Try

                End If
            End If
        Next

    End Sub

    ''' <summary>
    ''' Procedure which jumps to the right sales order line without populating the shortages for this sales order line.
    ''' </summary>
    ''' <param name="salesOrder">The sales order this procedure should search for</param>
    ''' <param name="salesOrderLine">The sales order line this procedure should search for.</param>
    ''' <param name="bNoShortageRefresh"></param>
    ''' <remarks>This procedure is user from the supply and demand grid.</remarks>
    Private Overloads Sub searchSalesOrder(ByVal salesOrder As String, ByVal salesOrderLine As String, ByVal bNoShortageRefresh As Boolean)

        For i As Integer = 0 To dgvSalesOrders.Rows.Count - 1
            If dgvSalesOrders.Rows(i).Cells("ORDER_NO").Value.ToString.Contains(salesOrder.Trim) Or dgvSalesOrders.Rows(i).Cells("ORDER_NO").Value.ToString.Trim = salesOrder.Trim Then
                If dgvSalesOrders.Rows(i).Cells("LINE_NO").Value.ToString = salesOrderLine Then
                    dgvSalesOrders.Rows(i).Selected = True
                    dgvSalesOrders.FirstDisplayedScrollingRowIndex = i
                    If Not bNoShortageRefresh Then
                        Me.populateMRPShortages(dgvSalesOrders.Rows(i).Cells("demand_id").Value, dgvSalesOrders.Rows(i).Cells("organization_id").Value, "")
                    End If
                End If
            End If
        Next



    End Sub


    ''' <summary>
    ''' Searches the job grid for the supplied data.
    ''' </summary>
    ''' <param name="jobcenter"></param>
    ''' <param name="jobNo"></param>
    ''' <remarks>In the moment it may not jump to the right operation sequence number.</remarks>
    Private Sub searchJob(ByVal jobcenter As String, ByVal jobNo As String, Optional ByVal listOfPriorResources() As String = Nothing )
        Debug.Print("start searching job:" & Now.Second & " -" & Now.Millisecond)
        Dim numberOfPriorHits As Integer = 0
        'See if there are prior hits to the search.
        If Not listOfPriorResources Is Nothing Then
            For i As Integer = 0 To listOfPriorResources.Length - 1
                For j As Integer = i + 1 To listOfPriorResources.Length - 1
                    'Not just look for duplicates, but also for this resource
                    If listOfPriorResources(i) = listOfPriorResources(j) AndAlso jobcenter = listOfPriorResources(j) Then
                        numberOfPriorHits += 1
                        'Because we are looking into the next elements, just get the first next one
                        Exit For
                    End If
                Next
            Next
        End If
        'First of all switch the datasource to wip
        If Me.bottonGrid = "wip" Then

        Else
            'Me.bottonGrid = "wip"
            Me.bindWipToGrid()
        End If
        Dim numberOfCurrentHits As Integer = 0
        ' Dim dv As DataView = dgvWipSupplierQueue.DataSource
        Debug.Print("start new search :" & Now.Second & " -" & Now.Millisecond)
        'New lookup
        Me.searchPhraseJob = jobcenter & jobNo
        Dim sublist As List(Of jobSearch) = Me.searchJobList.FindAll(AddressOf findJobLine)
        Debug.Print("start old search :" & Now.Second & " -" & Now.Millisecond)

        Try
            Dim newRow As Integer
            Dim numRow As Integer = 0

            '*** beware aware of the previous resources

            newRow = sublist.Item(numberOfPriorHits).rownumber

            dgvWipSupplierQueue.Rows(newRow).Selected = True
            dgvWipSupplierQueue.Rows(newRow).Cells("job_no").Selected = True
            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = newRow
        Catch ex As Exception

        End Try
       

        'For i As Integer = 0 To dgvWipSupplierQueue.Rows.Count - 1
        '    If dgvWipSupplierQueue.Rows(i).Cells("resource_code").Value.ToString = jobcenter Then

        '        'Cycle also into the columns to find the wip entity id
        '        If dgvWipSupplierQueue.Rows(i).Cells("job_no").Value.ToString.Contains(jobNo) Then

        '            dgvWipSupplierQueue.Rows(i).Selected = True
        '            dgvWipSupplierQueue.Rows(i).Cells("job_no").Selected = True
        '            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i
        '            'Increase the number of hits

        '            'just exit the for loop when the number of current hits is bigger -> We are at the right point to stop.
        '            ' If numberOfCurrentHits = numberOfPriorHits Then
        '            Exit For
        '            ' End If
        '            numberOfCurrentHits += 1
        '        End If

        '    End If
        'Next
        Debug.Print("Finish searching job:" & Now.Second & " -" & Now.Millisecond)
    End Sub
    Private searchPhraseJob As String           'variable that keeps the search phrase for the job


    Private Function findJobLine(ByVal j As jobSearch) As Boolean
        If j.searchPhrase = searchPhraseJob Then
            Return True
        ElseIf searchPhraseJob.Contains("*") Then
            If j.searchPhrase.Contains(searchPhraseJob.Remove(0, 1)) Then Return True
        Else
            Return False
        End If
    End Function

    Private searchPhrasePo As String

    Private Sub searchPO(ByVal supplier As String, ByVal po_no As String, ByVal line_no As Double, Optional ByVal releaseNo As String = "0", Optional ByVal shipmentNo As String = "0")
        Debug.Print("start po:" & Now.Second & "-" & Now.Millisecond)
        If Me.bottonGrid = "po" Then
        Else
            dgvWipSupplierQueue.DataSource = dsPO.Tables(0)
            'lblWIPPOFilter.Text = "Open Purchase Order Lines - No Filters" ' & dgvWipSupplierQueue.Rows.Count
            Me.bottonGrid = "po"
            gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
            If dgvWipSupplierQueue.Rows.Count = lngCountPurchaseOrderLines Then
                lblWIPPOFilter.Text = "Open Purchase Order Lines - No Filters " '& dgvWipSupplierQueue.Rows.Count 'Change request
            Else
                lblWIPPOFilter.Text = "Open Purchase Order Lines: " & dgvWipSupplierQueue.Rows.Count & " / " & lngCountPurchaseOrderLines & " Records Displayed"
            End If

        End If
        Debug.Print("before new po:" & Now.Second & "-" & Now.Millisecond)
        'New search implementation
        Dim strReleaseNoSearch As String = ""
        'If planningType = "ASCP" Then       'We have a db null for the org
        If releaseNo = 0 Then
            strReleaseNoSearch = ""
        Else
            strReleaseNoSearch = releaseNo.ToString
        End If
        'End If
        Me.searchPhrasePo = supplier & po_no & line_no & strReleaseNoSearch & shipmentNo
        Dim sublist As List(Of poSearch) = Me.searchPoList.FindAll(AddressOf findPoLine)
        Debug.Print("start old search :" & Now.Second & " -" & Now.Millisecond)

        If sublist.Count = 0 Then
            Me.searchPhrasePo = supplier & po_no & line_no
            sublist = Me.seachPOListPOLine.FindAll(AddressOf findPoLine)
            Debug.Print("start old search :" & Now.Second & " -" & Now.Millisecond)

        End If
        Try
            '*** beware aware of the previous resources
            Dim newRow As Integer = sublist.Item(0).rownumber

            dgvWipSupplierQueue.Rows(newRow).Selected = True
            dgvWipSupplierQueue.Rows(newRow).Cells("po_header_id").Selected = True
            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = newRow
        Catch ex As Exception
        End Try


        Debug.Print("after new po:" & Now.Second & "-" & Now.Millisecond)
        'For i As Integer = 0 To dgvWipSupplierQueue.Rows.Count - 1
        '    If dgvWipSupplierQueue.Rows(i).Cells("vendor_name").Value.ToString.Contains(supplier) Then

        '        'Cycle also into the columns to find the purchase order number
        '        If dgvWipSupplierQueue.Rows(i).Cells("po_no").Value.ToString = po_no Then
        '            If dgvWipSupplierQueue.Rows(i).Cells("LINE_NO").Value.ToString = line_no Then
        '                dgvWipSupplierQueue.Rows(i).Selected = True
        '                dgvWipSupplierQueue.Rows(i).Cells("po_header_id").Selected = True
        '                dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i
        '            End If


        '        End If

        '    End If
        'Next
        Debug.Print("end po:" & Now.Second & "-" & Now.Millisecond)
    End Sub

    Private Function findPoLine(ByVal j As poSearch) As Boolean
        If j.searchPhrase = searchPhrasePo Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub txtSearch_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            searchCrossGrids()
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        searchCrossGrids()
    End Sub


    Private Sub btnFindFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindFirst.Click
        'Procedure which leads just to the first occurance...
        Me.resetSearchCapabilities()
        Me.searchCrossGrids()
    End Sub

    Private Sub searchCrossGrids()
        Try
            'In case we have been on the wip grid we want to go back to it
            Dim strBottonGridStartValue As String = bottonGrid
            Dim lngDisplayedRow As Integer = dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex

            'Save what we have found in a two dimensional array which will be cicled trhough
            'if the user recklicks the find button and did not changed the search string
            'Get into the lifeycle the number of occurance which was found.

            '>> Possible to speed up the search when we do not cycle through the whole grid, instead look if???
            dgvSalesOrders.ClearSelection()

            'Cycle into the grids and jump to the first occurance
            'Cycle into the tables and find the data equivalent to the entered style
            If txtSearch.Text <> "" AndAlso (Me.searchGrid = "" Or Me.searchGrid = "so") Then
                searchString = txtSearch.Text.ToLower.Trim

                'Reset for the first cycle
                If Me.searchGrid = "" Then
                    Me.searchGrid = "so"
                End If

                'Count the number of occurances
                If lblOccurance.Text = "" Then

                    For i As Integer = 0 To dgvSalesOrders.RowCount - 1
                        For j As Integer = 0 To dgvSalesOrders.Columns.Count - 1
                            If dgvSalesOrders.Rows(i).Cells(j).Value.ToString.ToLower.Contains(searchString) AndAlso dgvSalesOrders.Columns(j).Visible Then
                                lngNumberOfHits += 1
                                'Just the count the lines with hits
                                Exit For
                            End If
                        Next
                    Next

                    'Load the purchase orders ....
                    If Me.bottonGrid = "wip" Then
                        Me.loadPO(strFilterPO)
                    End If

                    For i As Integer = 0 To dgvWipSupplierQueue.RowCount - 1
                        For j As Integer = 0 To dgvWipSupplierQueue.Columns.Count - 1
                            If dgvWipSupplierQueue.Rows(i).Cells(j).Value.ToString.ToLower.Contains(searchString) AndAlso dgvWipSupplierQueue.Columns(j).Visible Then
                                lngNumberOfHits += 1
                                Exit For
                            End If
                        Next
                    Next

                    'Iterate over the dataset, looking if the column is visible ... do not ask the backend all the time, perhaps just the first ... columns, displayed
                    'Visibility of the data of the wip discrete jobs is not really there...
                    For i As Integer = 0 To dsWIP.Tables(0).Rows.Count - 1
                        For j As Integer = 0 To 15
                            If dsWIP.Tables(0).Rows(i).Item(j).ToString.ToLower.Contains(searchString) Then
                                lngNumberOfHits += 1
                                Exit For
                            End If
                        Next
                    Next

                    lblOccurance.Text = "Line: 0 of " & lngNumberOfHits.ToString
                End If

                'Toggle to wip display
                'If bottonGrid = "po" Then
                'dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
                'End If

                For i As Integer = 0 To dgvSalesOrders.RowCount - 1

                    For j As Integer = 0 To dgvSalesOrders.Columns.Count - 1
                        'Just save the visible columns in the array
                        If dgvSalesOrders.Rows(i).Cells(j).Visible Then

                            'Search the whole row will be a little bit more sophisticated, but implement later
                            If Me.searchGrid = "so" AndAlso i > Me.row Then
                                'Look if the value contains the search strsing
                                If dgvSalesOrders.Rows(i).Cells(j).Value.ToString.ToLower.Contains(searchString) Then
                                    'activate the row and the cell we found
                                    'dgvSalesOrders.Rows(i).Selected = True
                                    dgvSalesOrders.FirstDisplayedScrollingRowIndex = i

                                    'dgvSalesOrders.FirstDisplayedCell = dgvSalesOrders.Rows(i).Cells(j)
                                    dgvSalesOrders.Rows(i).Cells(j).Selected = True
                                    Me.row = i
                                    Me.column = j
                                    lngCurrentNumberOfHit += 1
                                    lblOccurance.Text = "Line: " & lngCurrentNumberOfHit.ToString & " of " & lngNumberOfHits.ToString
                                    'After we have found a record, exit the subroutine
                                    Exit Sub

                                End If
                            End If

                        End If
                    Next

                Next

            End If

            'When cycled through the top grid just reset the row and column
            'Just when the first time change to the next grid
            If Me.searchGrid = "so" Then
                Me.row = -1
                Me.column = -1
            End If


            'Repeat this exercise with purchase orders and wip discrete jobs - just when in the sales order was nothing found
            'Cycle into the grids and jump to the first occurance
            'Cycle into the tables and find the data equivalent to the entered style
            'Search the purchase order if it is visible
            If txtSearch.Text <> "" Then
                searchString = txtSearch.Text.ToLower
                If Me.searchGrid = "so" Then
                    Me.searchGrid = "powip"

                End If

                'Toggle to wip display
                For i As Integer = 0 To dgvWipSupplierQueue.RowCount - 1

                    For j As Integer = 0 To dgvWipSupplierQueue.Columns.Count - 1
                        'Just save the visible columns in the array
                        If dgvWipSupplierQueue.Rows(i).Cells(j).Visible Then

                            'Search the whole row will be a little bit more sophisticated, but implement later
                            If Me.searchGrid = "powip" AndAlso i > Me.row Then
                                'Look if the value contains the search string
                                If dgvWipSupplierQueue.Rows(i).Cells(j).Value.ToString.ToLower.Contains(searchString) Then
                                    'activate the row and the cell we found
                                    'dgvWipSupplierQueue.Rows(i).Selected = True
                                    dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i

                                    'dgvWipSupplierQueue.FirstDisplayedCell = dgvWipSupplierQueue.Rows(i).Cells(j)
                                    dgvWipSupplierQueue.Rows(i).Cells(j).Selected = True
                                    Me.row = i
                                    Me.column = j
                                    lngCurrentNumberOfHit += 1
                                    lblOccurance.Text = "Line: " & lngCurrentNumberOfHit.ToString & " of " & lngNumberOfHits.ToString

                                    Exit Sub

                                End If
                            End If

                        End If
                    Next

                Next

            End If

            'This paragraph has to be jumped over, the second time search
            'Change from po to wip grid and vice versa to really toggle through all three grids.
            If Me.bottonGrid = "po" AndAlso Me.searchGrid = "powip" AndAlso lngNumberOfHits > 0 Then
                'dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
                Me.bindWipToGrid()
                lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters"    '& dgvWipSupplierQueue.Rows.Count
                gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
                Me.bottonGrid = "wip"
                Me.searchGrid = "wippo"
                Me.row = -1
                Me.column = -1

            ElseIf Me.bottonGrid = "wip" AndAlso Me.searchGrid = "powip" AndAlso lngNumberOfHits > 0 Then
                Me.loadPO("")
                Me.bottonGrid = "po"
                Me.row = -1
                Me.column = -1
                Me.searchGrid = "wippo"
            End If


            'Repeat this exercise with purchase orders and wip discrete jobs - just when in the sales order was nothing found
            'Cycle into the grids and jump to the first occurance
            'Cycle into the tables and find the data equivalent to the entered style
            'Search the purchase order if it is visible
            If txtSearch.Text <> "" Then
                searchString = txtSearch.Text.ToLower
                If Me.searchGrid = "so" Then
                    Me.searchGrid = "wippo"
                End If

                'Toggle to wip display
                For i As Integer = 0 To dgvWipSupplierQueue.RowCount - 1

                    For j As Integer = 0 To dgvWipSupplierQueue.Columns.Count - 1
                        'Just save the visible columns in the array
                        If dgvWipSupplierQueue.Rows(i).Cells(j).Visible Then

                            'Search the whole row will be a little bit more sophisticated, but implement later
                            If Me.searchGrid = "wippo" AndAlso i > Me.row Then
                                'Look if the value contains the search string
                                If dgvWipSupplierQueue.Rows(i).Cells(j).Value.ToString.ToLower.Contains(searchString) Then
                                    'activate the row and the cell we found
                                    'dgvWipSupplierQueue.Rows(i).Selected = True
                                    dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i

                                    'dgvWipSupplierQueue.FirstDisplayedCell = dgvWipSupplierQueue.Rows(i).Cells(j)
                                    dgvWipSupplierQueue.Rows(i).Cells(j).Selected = True
                                    Me.row = i
                                    Me.column = j
                                    lngCurrentNumberOfHit += 1
                                    lblOccurance.Text = "Line: " & lngCurrentNumberOfHit.ToString & " of " & lngNumberOfHits.ToString

                                    Exit Sub

                                End If
                            End If

                        End If
                    Next

                Next

            End If

            'Look if we have to reset the search capabilities when the end was reached
            If lngCurrentNumberOfHit = lngNumberOfHits AndAlso lngNumberOfHits <> 0 Then
                Me.resetSearchCapabilities()
                Me.searchCrossGrids()
            ElseIf lngNumberOfHits = 0 Then
                MessageBox.Show("Not Found. Please Refine Your Search.")
                If strBottonGridStartValue = "wip" AndAlso bottonGrid = "po" Then
                    Me.loadWip(strFilterWip & strFilterWIPTemp)
                    Me.bindWipToGrid()
                    dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = lngDisplayedRow
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub txtSearch_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtSearch.MouseClick
        txtSearch.SelectAll()
    End Sub



    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        Me.resetSearchCapabilities()
    End Sub

    Private Sub resetSearchCapabilities()
        Me.searchString = ""
        Me.searchGrid = ""
        Me.row = -1
        Me.column = -1
        'Also resets the number of occurances of the entity
        lblOccurance.Text = ""
        lngNumberOfHits = 0
        lngCurrentNumberOfHit = 0
    End Sub



    Sub ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

        
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim ds As New DataSet

            ds = db.SecureQueryParams("SELECT filterString, filterName FROM warehouseFilterTemplates WHERE filterName = @1 AND entity = @2", sender.ToString, "salesOrderLinesView")

            'Handle the event for the different views
            If ds.Tables(0).Rows.Count > 0 Then
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                    Me.deleteTemporaryFilters(True, False, False, False)
                    Me.loadSalesOrder(ds.Tables(0).Rows(0).Item(0))
                    'Reset the header color to beige
                    For Each h In dgvSalesOrders.Columns
                        dgvSalesOrders.Columns(h.name).HeaderCell.Style.BackColor = Color.Beige
                        dgvSalesOrders.Columns(h.name).HeaderCell.Style.ForeColor = Color.Black
                    Next


                    'Reset the filter string to nothing
                    Me.strFilterSalesOrderTemp = ""
                    'Disable the grid of showing blue
                    dgvSalesOrders.EnableHeadersVisualStyles = True

                    'Loading the aggregate value of the sales order
                    Dim strValue As String = ""
                    Try
                        Dim sumObject As Object
                        sumObject = dsSalesOrder.Tables(0).Compute("Sum(Extended_Price)", "") / 1000
                        strValue = Format(sumObject, "C0") + "k"
                    Catch ex As Exception

                    End Try

                    lblSalesOrderHeader.Text = sender.ToString & " - " & dsSalesOrder.Tables(0).Rows.Count & " / " & lngCountSalesOrderLines & " records displayed" & " - Value: " & strValue
                    Me.strFilterSalesOrder = ds.Tables(0).Rows(0).Item(0)
                    Me.strFilterSalesOrderName = ds.Tables(0).Rows(0).Item("filterName")
                End If
            Else
                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE userName = @1 AND filterName = @2", Environment.UserName, sender.ToString)
                Try
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                        Me.deleteTemporaryFilters(True, False, False, False)
                        Me.loadSalesOrder(ds.Tables(0).Rows(0).Item(0))
                        'Reset the header color to beige
                        For Each h In dgvSalesOrders.Columns
                            dgvSalesOrders.Columns(h.name).HeaderCell.Style.BackColor = Color.Beige
                            dgvSalesOrders.Columns(h.name).HeaderCell.Style.ForeColor = Color.Black
                        Next

                        dgvSalesOrders.EnableHeadersVisualStyles = True
                        Me.strFilterSalesOrderTemp = ""
                        'Loading the aggregate value of the sales order
                        Dim sumObject As Object
                        sumObject = dsSalesOrder.Tables(0).Compute("Sum(Extended_Price)", "") / 1000
                        Dim strValue As String = ""
                        Try
                            strValue = Format(sumObject, "C0") + "k"
                        Catch ex As Exception

                        End Try

                        lblSalesOrderHeader.Text = sender.ToString & " - " & dsSalesOrder.Tables(0).Rows.Count & " / " & lngCountSalesOrderLines & " records displayed" & " - Value: " & strValue
                        Me.strFilterSalesOrder = ds.Tables(0).Rows(0).Item(0)
                        Me.strFilterSalesOrderName = ds.Tables(0).Rows(0).Item("filterName")
                    End If
                Catch ex As Exception
                    Dim err As New clsExceptionManagement
                    err.createErrorLog("salesOrderMenuFilterClick", ex, 1)
                End Try


            End If

            dgvSalesOrders.EnableHeadersVisualStyles = True
        Catch ex As Exception

        End Try
    End Sub


    Private Sub ToolStripMenuItemBuy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try

        
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim ds As New DataSet

            ds = db.SecureQueryParams("SELECT filterString FROM warehouseFilterTemplates WHERE filterName = @1 AND entity = @2", sender.ToString, "poQueueView")

            If ds.Tables(0).Rows.Count > 0 Then
                'Handle the event for the different views
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                    Me.deleteTemporaryFilters(False, False, True, False)
                    Me.loadPO(ds.Tables(0).Rows(0).Item(0))
                    lblWIPPOFilter.Text = sender.ToString & " " & Me.numberOfRecordsDisplayedForWipPO(dsPO.Tables(0)).ToString & " / " & lngCountPurchaseOrderLines & " records displayed"
                    Me.strFilterPO = ds.Tables(0).Rows(0).Item(0)

                    'Reset the colour og the headers
                    For Each h In dgvWipSupplierQueue.Columns
                        dgvWipSupplierQueue.Columns(h.name).HeaderCell.Style.BackColor = Color.Beige
                        dgvWipSupplierQueue.Columns(h.name).HeaderCell.Style.ForeColor = Color.Black
                    Next
                    dgvWipSupplierQueue.EnableHeadersVisualStyles = True
                End If
            Else
                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE userName = @1 AND filterName = @2", Environment.UserName, sender.ToString)
                Try
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                        Me.loadPO(ds.Tables(0).Rows(0).Item(0))
                        lblWIPPOFilter.Text = sender.ToString & " " & Me.numberOfRecordsDisplayedForWipPO(dsPO.Tables(0)).ToString & " / " & lngCountPurchaseOrderLines & " records displayed"
                        Me.strFilterPO = ds.Tables(0).Rows(0).Item(0)

                        'Reset the colour og the headers
                        For Each h In dgvWipSupplierQueue.Columns
                            dgvWipSupplierQueue.Columns(h.name).HeaderCell.Style.BackColor = Color.Beige
                            dgvWipSupplierQueue.Columns(h.name).HeaderCell.Style.ForeColor = Color.Black
                        Next

                        dgvWipSupplierQueue.EnableHeadersVisualStyles = True
                    End If
                Catch ex As Exception
                    Dim err As New clsExceptionManagement
                    err.createErrorLog("toolStripMenuItemBuy", ex, 1)

                End Try

            End If

            dgvWipSupplierQueue.EnableHeadersVisualStyles = True
        Catch

        End Try

    End Sub

    Private Sub ToolStripMenuItemShortages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet

        ds = db.SecureQueryParams("SELECT filterString FROM warehouseFilterTemplates WHERE filterName = @1 AND entity = @2", sender.ToString, "MRPShortagesView")
        If ds.Tables(0).Rows.Count > 0 Then
            'Handle the event for the different views
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                Me.populateMRPShortages(lnDemandId, organizationId, ds.Tables(0).Rows(0).Item(0))

            End If
        Else
            ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE userName = @1 AND filterName = @2", Environment.UserName, sender.ToString)
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                Me.populateMRPShortages(lnDemandId, organizationId, ds.Tables(0).Rows(0).Item(0))

            End If
        End If

        dgvShortages.EnableHeadersVisualStyles = True
    End Sub

    Private Sub ToolStripMenuItemMake_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet

        ds = db.SecureQueryParams("SELECT filterString FROM warehouseFilterTemplates WHERE filterName = @1 AND entity = @2", sender.ToString, "WIPQueueView")
        If ds.Tables(0).Rows.Count > 0 Then
            Me.deleteTemporaryFilters(False, False, False, True)
            'Handle the event for the different views
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                Me.loadWip(ds.Tables(0).Rows(0).Item(0))
                Dim numberOfRecords = Me.numberOfRecordsDisplayedForWipPO(dsWIP.Tables(0))
                lblWIPPOFilter.Text = sender.ToString & " " & numberOfRecords & " / " & lngCountWIPDiscreteJobsOperationSequences & " records displayed"
                Me.strFilterWip = ds.Tables(0).Rows(0).Item(0)
                'Reset the colour og the headers
                For Each h In dgvWipSupplierQueue.Columns
                    dgvWipSupplierQueue.Columns(h.name).HeaderCell.Style.BackColor = Color.Beige
                    dgvWipSupplierQueue.Columns(h.name).HeaderCell.Style.ForeColor = Color.Black
                Next

                dgvWipSupplierQueue.EnableHeadersVisualStyles = True
            End If
        Else
            ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE userName = @1 AND filterName = @2  AND entity = @3", Environment.UserName, sender.ToString, "WIPQueueView")
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
                Me.loadWip(ds.Tables(0).Rows(0).Item(0))
                Dim numberOfRecords = Me.numberOfRecordsDisplayedForWipPO(dsWIP.Tables(0))
                lblWIPPOFilter.Text = sender.ToString & " " & numberOfRecords & " / " & lngCountWIPDiscreteJobsOperationSequences & " records displayed"
                Me.strFilterWip = ds.Tables(0).Rows(0).Item(0)

                'Reset the colour og the headers
                For Each h In dgvWipSupplierQueue.Columns
                    dgvWipSupplierQueue.Columns(h.name).HeaderCell.Style.BackColor = Color.Beige
                    dgvWipSupplierQueue.Columns(h.name).HeaderCell.Style.ForeColor = Color.Black
                Next

                dgvWipSupplierQueue.EnableHeadersVisualStyles = True
            End If
        End If

        bottonGrid = "wip"

        dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
        gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)

        dgvWipSupplierQueue.EnableHeadersVisualStyles = True
    End Sub

    Public Sub reloadUserViews()

        SellToolStripMenuItem.DropDownItems.Clear()
        FilterToolStripMenuItem.DropDownItems.Clear()

        BuyToolStripMenuItem.DropDownItems.Clear()
        POFilterToolStripMenuItem.DropDownItems.Clear()

        ShortageFilterToolStripMenuItem.DropDownItems.Clear()
        MakeToolStripMenuItem.DropDownItems.Clear()
        WIPFilterToolStripMenuItem.DropDownItems.Clear()
        'Also reset the header line


        Me.loadUserViews()


    End Sub

    ''' <summary>
    ''' Setting up the childrend of the dropdown collection programmatically.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub loadUserViews()
        Try


            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

            'Load the templates for the users
            Dim dsSales As DataSet
            dsSales = db.SecureQueryParams("SELECT filterName, grouping FROM warehouseFilterTemplates WHERE entity = 'salesOrderLinesView' AND (organizationId IS NULL OR organizationid = @1) ORDER BY filterName", Me.organizationId)

            Dim onClickHandler As System.EventHandler = New System.EventHandler(AddressOf ToolStripMenuItem_Click)

            For i As Integer = 0 To dsSales.Tables(0).Rows.Count - 1
                'Add the item to the specific group
                If Not DBNull.Value.Equals(dsSales.Tables(0).Rows(i).Item("grouping")) Then
                    'Look if the group already exists
                    If SellToolStripMenuItem.DropDownItems.ContainsKey(dsSales.Tables(0).Rows(i).Item("grouping")) Then
                        For k As Integer = 0 To SellToolStripMenuItem.DropDownItems.Count - 1
                            'Add the items to the existing group
                            If SellToolStripMenuItem.DropDownItems(k).Name = dsSales.Tables(0).Rows(i).Item("grouping") Then
                                Dim fruitToolStripMenuItem As ToolStripMenuItem = SellToolStripMenuItem.DropDownItems(k)
                                fruitToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler)

                            End If
                        Next
                        For k As Integer = 0 To FilterToolStripMenuItem.DropDownItems.Count - 1
                            If FilterToolStripMenuItem.DropDownItems(k).Name = dsSales.Tables(0).Rows(i).Item("grouping") Then
                                Dim tsmi As ToolStripMenuItem = FilterToolStripMenuItem.DropDownItems(k)
                                tsmi.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler)
                            End If
                        Next
                    Else
                        Dim fruitToolStripMenuItem As New ToolStripMenuItem(dsSales.Tables(0).Rows(i).Item("grouping").ToString, Nothing, Nothing, dsSales.Tables(0).Rows(i).Item("grouping").ToString)
                        fruitToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler)
                        SellToolStripMenuItem.DropDownItems.Add(fruitToolStripMenuItem)
                        Dim tsmi As New ToolStripMenuItem(dsSales.Tables(0).Rows(i).Item("grouping").ToString, Nothing, Nothing, dsSales.Tables(0).Rows(i).Item("grouping").ToString)
                        tsmi.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler)
                        FilterToolStripMenuItem.DropDownItems.Add(tsmi)
                    End If
                Else
                    SellToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler)
                    FilterToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler)
                End If
            Next

            'Load the user specific views for the sales order lines
            dsSales = db.SecureQueryParams("SELECT filterName FROM [FSG_IND_WORKBENCH].[dbo].[warehouseUserFilters] WHERE entity = 'salesOrderLinesView' AND userName = @1 ORDER BY filterName", Environment.UserName)
            For i As Integer = 0 To dsSales.Tables(0).Rows.Count - 1
                SellToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler)
                FilterToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler)
            Next

            'Load the purchase order queue

            dsSales = db.SecureQueryParams("SELECT filterName FROM warehouseFilterTemplates WHERE entity = 'poQueueView' AND (organizationId IS NULL OR organizationId = @1) ORDER BY filterName ", Me.organizationId)

            Dim onClickHandler2 As System.EventHandler = New System.EventHandler(AddressOf ToolStripMenuItemBuy_Click)
            For i As Integer = 0 To dsSales.Tables(0).Rows.Count - 1
                BuyToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler2)
                POFilterToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler2)
            Next

            'User specific PO queue
            dsSales = db.SecureQueryParams("SELECT filterName FROM [FSG_IND_WORKBENCH].[dbo].[warehouseUserFilters] WHERE entity = 'poQueueView' AND userName = @1 ORDER BY filterName", Environment.UserName)
            For i As Integer = 0 To dsSales.Tables(0).Rows.Count - 1
                BuyToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler2)
                POFilterToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler2)
            Next


            'load mrp shortages
            Dim onClickHandler3 As System.EventHandler = New System.EventHandler(AddressOf ToolStripMenuItemShortages_Click)
            dsSales = db.SecureQueryParams("SELECT filterName FROM warehouseFilterTemplates WHERE entity = 'MRPShortagesView' AND (organizationId IS NULL OR organizationId = @1) ORDER BY filterName", Me.organizationId)

            For i As Integer = 0 To dsSales.Tables(0).Rows.Count - 1
                'BuyToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler2)
                ShortageFilterToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler3)
            Next

            'User specific mrp shortages
            dsSales = db.SecureQueryParams("SELECT filterName FROM [FSG_IND_WORKBENCH].[dbo].[warehouseUserFilters] WHERE entity = 'MRPShortagesView' AND userName = @1 ORDER BY filterName", Environment.UserName)
            For i As Integer = 0 To dsSales.Tables(0).Rows.Count - 1
                'BuyToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler2)
                ShortageFilterToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler3)
            Next



            'load wip queue view
            Dim onClickHandler4 As System.EventHandler = New System.EventHandler(AddressOf ToolStripMenuItemMake_Click)
            dsSales = db.SecureQueryParams("SELECT filterName FROM warehouseFilterTemplates WHERE entity = 'WIPQueueView' AND (organizationId IS NULL OR organizationId = @1) ORDER BY filterName", Me.organizationId)

            For i As Integer = 0 To dsSales.Tables(0).Rows.Count - 1
                MakeToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler4)
                WIPFilterToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler4)
            Next

            'User specific WIP queue
            dsSales = db.SecureQueryParams("SELECT filterName FROM [FSG_IND_WORKBENCH].[dbo].[warehouseUserFilters] WHERE entity = 'WIPQueueView' AND userName = @1 ORDER BY filterName", Environment.UserName)
            For i As Integer = 0 To dsSales.Tables(0).Rows.Count - 1
                MakeToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler4)
                WIPFilterToolStripMenuItem.DropDownItems.Add(dsSales.Tables(0).Rows(i).Item("filterName"), Nothing, onClickHandler4)
            Next
        Catch ex As Exception

        End Try
    End Sub


    Private Sub Reload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Reload.Click
        Me.populateGrids()
        Me.loadCurrentNotifications()

    End Sub



    Private Sub CreateViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreateViewToolStripMenuItem.Click

        Dim objects() As String = {"salesOrderLinesView", "MRPShortagesView", "WIPQueueView", "POQueueView"}
        Dim objectNames() As String = {"Sales Order Lines", "MRP Shortages", "Work In Process Queue", "Purchase Order Queue"}

        Dim frm As New frmFilterSettings(objects, objectNames, Me.organizationId, Me)
        frm.Show()

    End Sub


    ''' <summary>
    ''' Copies relevant informations for the supplier to the clipboard.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CopyToClipboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToClipboardToolStripMenuItem.Click
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        Dim data_object As New DataObject
        Dim stext As String = ""
        Dim strTable As String = " poQueueView "

        If Me.planningType = "ASCP" Then
            strTable = " MSCPOQueueView "
        End If


        If bottonGrid = "po" Then
            'make a row selected, when a cell is selected
            If dgvWipSupplierQueue.SelectedRows.Count = 0 Then
                If dgvWipSupplierQueue.SelectedCells.Count = 0 Then
                    MessageBox.Show("Please select a cell first.")
                Else
                    dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(0).RowIndex).Selected = True
                End If
            End If

            'Gather the names from the grid
            If dgvWipSupplierQueue.SelectedRows.Count > 0 AndAlso Me.planningType = "MRP" Then

                stext += "Name:" & ControlChars.Tab & ControlChars.Tab & dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedRows(0).Index).Cells("VEND_CONTACT").Value.ToString
                stext += vbCrLf
                stext += "E-Mail:" & ControlChars.Tab & ControlChars.Tab & dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedRows(0).Index).Cells("EMAIL_ADDRESS").Value.ToString
                stext += vbCrLf
                stext += "Supplier:" & ControlChars.Tab & dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedRows(0).Index).Cells("VENDOR_NAME").Value.ToString
                stext += vbCrLf
                stext += vbCrLf
                stext += "Please advise ship date and carrier for the orders listed below. Thanks."
                stext += vbCrLf
                stext += vbCrLf
                'stext += "<table width='500' bgcolor='#6699ff'> " & _
                '            "    <tr> " & _
                '            "        <td class='style4' colspan='8' bgcolor='White'> " & _
                '            " Expediting Lines " & _
                '            "    </td> <td></td> " & _
                '            "    </tr> <tr>"

                stext += "PO_Line_Re" & ControlChars.Tab & "Needed By" & ControlChars.Tab & "Promised" & ControlChars.Tab & "Request Date" & ControlChars.Tab & "QTY Open" & ControlChars.Tab & "Item" & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & "Description" & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & "Comment"
                'stext += "<td> PO-Line </td>" & "<td>Needed By </td>" & "<td>Promised</td>" & "<td>Request Date</td>" & "<td>QTY Open</td>" & "<td>Item</td>" & "<td>Description</td>" & "<td>Comment</td></tr>"

                'Loop to add the informations from the grid
                'Get the vendor, query the relevant information, copy it to the textstring

                'Take care: The user can reorder the columns - > New Query
                Dim ds As New DataSet
                'We also have to take care about the currently applied filters
                ds = db.SecureQueryParams("SELECT PoNoLine  , ISNULL(NeededBy, '01/01/1900') AS Need_by_date " & _
                                          ", ISNULL(Promised_date,'01/01/1900') AS Promised_date, lastComment, " & _
                                          " ISNULL(Need_by_date, '01/01/1900') AS Approved_date, Item_no, " & _
                                          " Item_description, ISNULL(release_no,0) AS release_no,ISNULL(QUANTITY_ORDERED,0) AS QUANTITY_ORDERED, ISNULL(QTY_RECEIVED,0) AS QTY_RECEIVED FROM  " & strTable & _
                                          "WHERE ship_to_organization_id = @1 AND vendor_id = @2  " & strFilterPO & " " & strFilterPOTemp & _
                                          "ORDER BY CASE WHEN neededBy IS NULL THEN 1 ELSE 0 end, NeededBy, PoNoLine, Release_No, Shipment_No, Promised_date  ", Me.organizationId, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedRows(0).Index).Cells("VENDOR_ID").Value.ToString)

                'Take care of localization of the following piece of code
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    'stext += "<tr>"
                    stext += vbCrLf
                    If ds.Tables(0).Rows(i).Item("release_no") = 0 Then
                        stext += ds.Tables(0).Rows(i).Item("PoNoLine").ToString
                    Else
                        stext += ds.Tables(0).Rows(i).Item("PoNoLine").ToString & "_" & ds.Tables(0).Rows(i).Item("release_no").ToString
                    End If

                    'stext += "<td>" & ds.Tables(0).Rows(i).Item("PoNoLine").ToString & "</td>"
                    'Add an empty string if we find a date like 19000101
                    If ds.Tables(0).Rows(i).Item("Need_by_date") = DateTime.ParseExact("19000101", "yyyyMMdd", Nothing) Then
                        stext += ControlChars.Tab & ControlChars.Tab
                        'stext = "<td></td>"
                    Else
                        stext += ControlChars.Tab & Format(ds.Tables(0).Rows(i).Item("Need_by_date"), "Short Date")
                        'stext += "<td>" & Format(ds.Tables(0).Rows(i).Item("Need_by_date"), "Short Date") & "</td>"
                    End If
                    If ds.Tables(0).Rows(i).Item("Promised_date") = DateTime.ParseExact("19000101", "yyyyMMdd", Nothing) Then
                        stext += ControlChars.Tab & ControlChars.Tab
                        'stext = "<td></td>"
                    Else
                        stext += ControlChars.Tab & Format(ds.Tables(0).Rows(i).Item("Promised_date"), "Short Date")
                        'stext += "<td>" & Format(ds.Tables(0).Rows(i).Item("Promised_date"), "Short Date") & "</td>"
                    End If
                    If ds.Tables(0).Rows(i).Item("Approved_date") = DateTime.ParseExact("19000101", "yyyyMMdd", Nothing) Then
                        stext += ControlChars.Tab & ControlChars.Tab
                        'stext = "<td></td>"
                    Else
                        stext += ControlChars.Tab & Format(ds.Tables(0).Rows(i).Item("Approved_date"), "Short Date")
                        'stext += "<td>" & Format(ds.Tables(0).Rows(i).Item("Approved_date"), "Short Date") & "</>"
                    End If


                    stext += ControlChars.Tab & ControlChars.Tab & (ds.Tables(0).Rows(i).Item("QUANTITY_ORDERED") - ds.Tables(0).Rows(i).Item("QTY_RECEIVED")).ToString
                    'stext += "<td>" & (ds.Tables(0).Rows(i).Item("QUANTITY_ORDERED") - ds.Tables(0).Rows(i).Item("QTY_RECEIVED")).ToString & "</td>"
                    'Shrink the item to 25 characters
                    If ds.Tables(0).Rows(i).Item("Item_no").ToString.Count >= 25 Then
                        stext += ControlChars.Tab & ds.Tables(0).Rows(i).Item("Item_no").ToString.Substring(0, 25)
                    Else
                        stext += ControlChars.Tab & ds.Tables(0).Rows(i).Item("Item_no")
                    End If

                    'stext += "<td>" & ds.Tables(0).Rows(i).Item("Item_no") & "</td>"

                    'shrink the item description to 20 characters 
                    Dim strDescription As String = ""
                    If ds.Tables(0).Rows(i).Item("Item_description").ToString.Count >= 20 Then
                        strDescription = ds.Tables(0).Rows(i).Item("Item_description").ToString.Substring(0, 20)
                    Else
                        strDescription = ds.Tables(0).Rows(i).Item("Item_description")
                    End If
                    ''Remove carriage return    ***Not ready yet
                    strDescription = Replace(strDescription, vbCrLf, "")
                    strDescription = strDescription.Replace(ControlChars.Cr, "")
                    strDescription = strDescription.Replace(ControlChars.NewLine, "")
                    strDescription = strDescription.Trim("/[\r\n]+/g")
                    'strDescription = strDescription.Replace(vbCrLf, "")
                    strDescription = strDescription.Trim()


                    If ds.Tables(0).Rows(i).Item("Item_no").ToString.Count <= 5 Then
                        stext += ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & strDescription & ControlChars.Tab
                    ElseIf ds.Tables(0).Rows(i).Item("Item_no").ToString.Count <= 12 Then
                        stext += ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & strDescription & ControlChars.Tab
                    ElseIf ds.Tables(0).Rows(i).Item("Item_no").ToString.Count <= 15 Then
                        stext += ControlChars.Tab & ControlChars.Tab & strDescription & ControlChars.Tab
                    ElseIf ds.Tables(0).Rows(i).Item("Item_no").ToString.Count <= 22 Then
                        stext += ControlChars.Tab & strDescription & ControlChars.Tab
                    Else
                        stext += strDescription & ControlChars.Tab
                    End If
                    'stext += "<td>" & strDescription & "</td>"

                    'Shrink down the size of comments
                    Dim strComments As String = ""
                    If ds.Tables(0).Rows(i).Item("lastComment").ToString.Count >= 35 Then
                        strComments = ds.Tables(0).Rows(i).Item("lastComment").ToString.Substring(0, 35)
                    Else
                        strComments = ds.Tables(0).Rows(i).Item("lastComment").ToString
                    End If

                    'Add the last comment column
                    If strDescription.ToCharArray.Length <= 16 Then
                        stext += ControlChars.Tab & ControlChars.Tab & strComments
                    Else
                        stext += ControlChars.Tab & strComments
                    End If
                    'stext += "<td>" & ds.Tables(0).Rows(i).Item("lastComment").ToString & "</td>"
                    'stext += "</tr>"
                Next
                ' stext += " </table>"


            ElseIf dgvWipSupplierQueue.SelectedRows.Count > 0 AndAlso Me.planningType = "ASCP" Then
                stext = Me.createExpeditingMessageASCP(strTable)
            Else
                MessageBox.Show("Please select a row first.")
            End If

            'Loop to add the informations from the grid
            'Get the vendor, query the relevant information, copy it to the textstring

            If Me.planningType = "MRP" Then

                ' Add the data in various formats.
                data_object.SetData(DataFormats.StringFormat, stext)

                ' Copy data to the clipboard.
                Clipboard.SetDataObject(data_object)
            Else
                Dim sContextStart As String = "<HTML><BODY>"
                Dim sContextEnd As String = "</BODY></HTML>"
                Dim sHtmlFragment As String = stext

                sContextStart = sContextStart & "<!--StartFragment -->"
                sContextEnd = "<!--EndFragment -->" & sContextEnd


                Dim m_sDescription As String = _
                 "Version:1.0" & vbCrLf & _
                "StartHTML:aaaaaaaaaa" & vbCrLf & _
                "EndHTML:bbbbbbbbbb" & vbCrLf & _
                "StartFragment:cccccccccc" & vbCrLf & _
                "EndFragment:dddddddddd <br>" & vbCrLf

                Dim sData As String = m_sDescription & sContextStart & sHtmlFragment & sContextEnd


                sData = Replace(sData, "aaaaaaaaaa", _
                   Format(Len(m_sDescription), "0000000000"))
                sData = Replace(sData, "bbbbbbbbbb", Format(Len(sData), "0000000000"))
                sData = Replace(sData, "cccccccccc", Format(Len(m_sDescription & _
                                sContextStart), "0000000000"))
                sData = Replace(sData, "dddddddddd", Format(Len(m_sDescription & _
                                sContextStart & sHtmlFragment), "0000000000"))

                ' Add the data in various formats.
                data_object.SetData(DataFormats.Html, sData)

                ' Copy data to the clipboard.
                Clipboard.SetDataObject(data_object)

                'Dim m_sDescription As String = _
                '  "Version:1.0" & vbCrLf & _
                ' "StartHTML:aaaaaaaaa" & vbCrLf & _
                ' "EndHTML:bbbbbbbbbb" & vbCrLf & _
                ' "StartFragment:cccccccccc" & vbCrLf & _
                ' "EndFragment:dddddddddd <br>" & vbCrLf

                ''m_sDescription = m_sDescription.Replace("aaaaaaaaa", Format(Len(m_sDescription), "0000000000"))
                ''m_sDescription = m_sDescription.Replace("bbbbbbbbbb", Format(Len(stext), "0000000000"))
                ''m_sDescription = m_sDescription.Replace("cccccccccc", Format(Len(m_sDescription), "0000000000"))
                ''m_sDescription = m_sDescription.Replace("dddddddddd", Format((Len(stext) + Len(stext)), "0000000000"))

                '' Add the data in various formats.
                'data_object.SetData(DataFormats.Html, m_sDescription & stext)

                '' Copy data to the clipboard.
                'Clipboard.SetDataObject(data_object)

            End If

        End If

        

    End Sub

    Private Function createExpeditingMessageASCP(ByVal strTable As String) As String
        Dim stext As String
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        stext += "<table><tr>"
        stext += "<td>Name:</td><td>" & ControlChars.Tab & ControlChars.Tab & dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedRows(0).Index).Cells("VEND_CONTACT").Value.ToString
        stext += "</td></tr>"
        stext += "<tr><td>E-Mail:</td><td>" & ControlChars.Tab & ControlChars.Tab & dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedRows(0).Index).Cells("EMAIL_ADDRESS").Value.ToString
        stext += "</td></tr>"
        stext += "<tr><td>Supplier:</td><td>" & ControlChars.Tab & dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedRows(0).Index).Cells("VENDOR_NAME").Value.ToString
        stext += "</td></tr></table>"
        stext += "<br>"
        stext += "<br>"
        stext += "Please advise ship date and carrier for the orders listed below. Thanks."
        stext += "<br>"
        stext += "<br>"
        stext += "<table  bgcolor='#6699ff'> " & _
                    "    <tr> " & _
                    "        <td class='style4' colspan='10' bgcolor='White'> " & _
                    " Expediting Lines " & _
                    "    </td>  " & _
                    "    </tr> <tr>"

        'stext += "PO_Line_Re" & ControlChars.Tab & "Needed By" & ControlChars.Tab & "Promised" & ControlChars.Tab & "Request Date" & ControlChars.Tab & "QTY Open" & ControlChars.Tab & "Item" & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & "Description" & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & "Comment"
        stext += "<td> PO-Line </td>" & "<td>Promised</td>" & "<td>Request Date</td>" & "<td>QTY Open</td>" & "<td>Item</td>" & "<td>Description</td>" & "<td>Reschedule In</td><td>Reschedule Out</td><td>Cancel?</td><td>Comment</td></tr>"

        'Loop to add the informations from the grid
        'Get the vendor, query the relevant information, copy it to the textstring

        'Take care: The user can reorder the columns - > New Query
        Dim ds As New DataSet
        'We also have to take care about the currently applied filters
        ds = db.SecureQueryParams("SELECT PoNoLine  , ISNULL(NeededBy, '01/01/1900') AS Need_by_date " & _
                                  ", ISNULL(Promised_date,'01/01/1900') AS Promised_date, lastComment, " & _
                                  " ISNULL(Need_by_date, '01/01/1900') AS Approved_date, Item_no, " & _
                                  " Item_description, ISNULL(release_no,0) AS release_no, shipment_no,ISNULL(QUANTITY_ORDERED,0) AS QUANTITY_ORDERED, ISNULL(QTY_RECEIVED,0) AS QTY_RECEIVED, exceptionMessage, rescheduleIn, rescheduleOut, cancelllationMessage FROM  " & strTable & _
                                  "WHERE ship_to_organization_id = @1 AND vendor_id = @2  " & strFilterPO & " " & strFilterPOTemp & _
                                  "ORDER BY CASE WHEN neededBy IS NULL THEN 1 ELSE 0 end, NeededBy, PoNoLine, Release_No, Shipment_No, Promised_date  ", Me.organizationId, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedRows(0).Index).Cells("VENDOR_ID").Value.ToString)

        'Take care of localization of the following piece of code
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            'white?
            Dim strWhite As String = " bgcolor = 'White'"
            If i Mod 2 = 0 Then
                strWhite = " bgcolor = 'White' "
            End If

            stext += "<tr " & strWhite & "><td>"
            'stext += vbCrLf
            If ds.Tables(0).Rows(i).Item("release_no") = 0 Then
                stext += ds.Tables(0).Rows(i).Item("PoNoLine").ToString
            Else
                stext += ds.Tables(0).Rows(i).Item("PoNoLine").ToString & "_" & ds.Tables(0).Rows(i).Item("release_no").ToString '& "_S:" & ds.Tables(0).Rows(i).Item("shipment_no").ToString
            End If
            stext += "</td>"

            ''stext += "<td>" & ds.Tables(0).Rows(i).Item("PoNoLine").ToString & "</td>"
            ''Add an empty string if we find a date like 19000101
            'If ds.Tables(0).Rows(i).Item("Need_by_date") = DateTime.ParseExact("19000101", "yyyyMMdd", Nothing) Then
            '    'stext += ControlChars.Tab & ControlChars.Tab
            '    stext += "<td></td>"
            'Else
            '    'stext += ControlChars.Tab & Format(ds.Tables(0).Rows(i).Item("Need_by_date"), "Short Date")
            '    stext += "<td>" & Format(ds.Tables(0).Rows(i).Item("Need_by_date"), "Short Date") & "</td>"
            'End If
            If ds.Tables(0).Rows(i).Item("Promised_date") = DateTime.ParseExact("19000101", "yyyyMMdd", Nothing) Then
                'stext += ControlChars.Tab & ControlChars.Tab
                stext += "<td></td>"
            Else
                'stext += ControlChars.Tab & Format(ds.Tables(0).Rows(i).Item("Promised_date"), "Short Date")
                stext += "<td>" & Format(ds.Tables(0).Rows(i).Item("Promised_date"), "Short Date") & "</td>"
            End If
            If ds.Tables(0).Rows(i).Item("Approved_date") = DateTime.ParseExact("19000101", "yyyyMMdd", Nothing) Then
                'stext += ControlChars.Tab & ControlChars.Tab
                stext += "<td></td>"
            Else
                'stext += ControlChars.Tab & Format(ds.Tables(0).Rows(i).Item("Approved_date"), "Short Date")
                stext += "<td>" & Format(ds.Tables(0).Rows(i).Item("Approved_date"), "Short Date") & "</>"
            End If


            'stext += ControlChars.Tab & ControlChars.Tab & (ds.Tables(0).Rows(i).Item("QUANTITY_ORDERED") - ds.Tables(0).Rows(i).Item("QTY_RECEIVED")).ToString
            stext += "<td>" & (ds.Tables(0).Rows(i).Item("QUANTITY_ORDERED") - ds.Tables(0).Rows(i).Item("QTY_RECEIVED")).ToString & "</td>"
            'Shrink the item to 25 characters
            'If ds.Tables(0).Rows(i).Item("Item_no").ToString.Count >= 25 Then
            '    stext += ds.Tables(0).Rows(i).Item("Item_no").ToString.Substring(0, 25)
            'Else
            '    stext += ds.Tables(0).Rows(i).Item("Item_no")
            'End If

            stext += "<td>" & ds.Tables(0).Rows(i).Item("Item_no") & "</td>"

            'shrink the item description to 20 characters 
            Dim strDescription As String = ""
            If ds.Tables(0).Rows(i).Item("Item_description").ToString.Count >= 20 Then
                strDescription = ds.Tables(0).Rows(i).Item("Item_description").ToString.Substring(0, 20)
            Else
                strDescription = ds.Tables(0).Rows(i).Item("Item_description")
            End If
            ''Remove carriage return    ***Not ready yet
            strDescription = Replace(strDescription, vbCrLf, "")
            strDescription = strDescription.Replace(ControlChars.Cr, "")
            strDescription = strDescription.Replace(ControlChars.NewLine, "")
            strDescription = strDescription.Trim("/[\r\n]+/g")
            'strDescription = strDescription.Replace(vbCrLf, "")
            strDescription = strDescription.Trim()


            'If ds.Tables(0).Rows(i).Item("Item_no").ToString.Count <= 5 Then
            '    stext += ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & strDescription & ControlChars.Tab
            'ElseIf ds.Tables(0).Rows(i).Item("Item_no").ToString.Count <= 12 Then
            '    stext += ControlChars.Tab & ControlChars.Tab & ControlChars.Tab & strDescription & ControlChars.Tab
            'ElseIf ds.Tables(0).Rows(i).Item("Item_no").ToString.Count <= 15 Then
            '    stext += ControlChars.Tab & ControlChars.Tab & strDescription & ControlChars.Tab
            'ElseIf ds.Tables(0).Rows(i).Item("Item_no").ToString.Count <= 22 Then
            '    stext += ControlChars.Tab & strDescription & ControlChars.Tab
            'Else
            '    stext += strDescription & ControlChars.Tab
            'End If

            stext += "<td>" & strDescription & "</td>"

            'Shrink down the size of comments
            'Dim strComments As String = ""
            'If ds.Tables(0).Rows(i).Item("lastComment").ToString.Count >= 35 Then
            '    strComments = ds.Tables(0).Rows(i).Item("lastComment").ToString.Substring(0, 35)
            'Else
            '    strComments = ds.Tables(0).Rows(i).Item("lastComment").ToString
            'End If

            ''Add the last comment column
            'If strDescription.ToCharArray.Length <= 16 Then
            '    stext += ControlChars.Tab & ControlChars.Tab & strComments
            'Else
            '    stext += ControlChars.Tab & strComments
            'End If
            If DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("rescheduleIn")) Then
                stext += "<td></td>"
            Else
                stext += "<td>" & FormatDateTime(ds.Tables(0).Rows(i).Item("rescheduleIn"), DateFormat.ShortDate) & "</td>"
            End If

            If DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("rescheduleOut")) Then
                stext += "<td></td>"
            Else
                stext += "<td>" & FormatDateTime(ds.Tables(0).Rows(i).Item("rescheduleOut"), DateFormat.ShortDate) & "</td>"
            End If


            stext += "<td>" & ds.Tables(0).Rows(i).Item("cancelllationMessage").ToString & "</td>"
            stext += "<td>" & ds.Tables(0).Rows(i).Item("lastComment").ToString & "</td>"
            stext += "</tr>"
        Next
        stext += " </table>"


        Return stext
    End Function



    Private Sub ChangeGridToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeGridToolStripMenuItem.Click
        Try
            'Disable and delete the temporary filters
            dgvWipSupplierQueue.EnableHeadersVisualStyles = True
            Me.deleteTemporaryFilters(False, False, True, True)

            If bottonGrid = "po" Then
                bottonGrid = "wip"

                'Find out before if a filter was applied.
                If dsWIP.Tables(0).Rows.Count < lngCountWIPDiscreteJobsOperationSequences Then
                    Me.loadWip("")
                End If
                dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
                gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
                dgvWipSupplierQueue.ContextMenuStrip = cmsBottonWip
                dgvWipSupplierQueue.Columns("JOB_NO").DisplayIndex = 4
                lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters"
            Else
                bottonGrid = "po"
                If dsPO.Tables(0).Rows.Count < lngCountPurchaseOrderLines Then
                    Me.loadPO("")
                End If
                dgvWipSupplierQueue.DataSource = dsPO.Tables(0)
                gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
                dgvWipSupplierQueue.ContextMenuStrip = cmsBotton
                lblWIPPOFilter.Text = "Open Purchase Order Lines - No Filters"

            End If
        Catch ex As Exception

        End Try

    End Sub



    Private Sub dgvFurtherDetails_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFurtherDetails.CellClick
        If Not e.ColumnIndex = -1 And Not e.RowIndex = -1 Then

            If dgvFurtherDetails.Columns(e.ColumnIndex).Name = "lastComment" Then
                'Display the comment form
                Dim dlg As New dlgComments(dgvFurtherDetails.Rows(e.RowIndex).Cells("WIP_ENTITY_ID").Value, dgvFurtherDetails.Rows(e.RowIndex).Cells("JOB_NO").Value, dgvFurtherDetails.Rows(e.RowIndex).Cells("OPERATION_SEQ_NO").Value, 0, "WIPQueueView", dgvFurtherDetails, e.RowIndex, Me.organizationId)
                dlg.ShowDialog()
                If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                    Me.loadCurrentNotifications()
                End If
            End If

            If dgvFurtherDetails.Columns(e.ColumnIndex).Name = "JOB_NO" Or dgvFurtherDetails.Columns(e.ColumnIndex).Name = "RESOURCE_CODE" Then
                Me.searchJob(dgvFurtherDetails.Rows(e.RowIndex).Cells("RESOURCE_CODE").Value.ToString, dgvFurtherDetails.Rows(e.RowIndex).Cells("JOB_NO").Value)
            End If

        End If
    End Sub

    Private Sub ExportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToolStripMenuItem.Click

        sfdExcelPlace.Tag = "salesOrderLinesView"
        sfdExcelPlace.ShowDialog()


    End Sub

    Private Sub sfdExcelPlace_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles sfdExcelPlace.FileOk

        Dim gridOperation As New clsGridOperations
        If sfdExcelPlace.Tag = "salesOrderLinesView" Then
            'dsSalesOrder.WriteXml(sfdExcelPlace.FileName)
            gridOperation.exportExcel(dgvSalesOrders, sfdExcelPlace.FileName)
        ElseIf sfdExcelPlace.Tag = "MRPShortagesView" Then
            gridOperation.exportExcel(dgvShortages, sfdExcelPlace.FileName)
        ElseIf sfdExcelPlace.Tag = "POQueueView" Then
            gridOperation.exportExcel(dgvWipSupplierQueue, sfdExcelPlace.FileName)
        ElseIf sfdExcelPlace.Tag = "WIPQueueView" Then
            gridOperation.exportExcel(dgvWipSupplierQueue, sfdExcelPlace.FileName)
        End If

    End Sub


    'Navigates to the specific notifications
    Private Sub lblNotifications_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblNotifications.MouseEnter
        Try

       
            Dim ds As New DataSet
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim strComments As String = ""

            ds = db.SecureQueryParams("SELECT organizationName, COUNT(*) as messages FROM whUserNotificationsView WHERE  notificationTo = @1 AND reviewed = 0 GROUP BY organizationName", Environment.UserName)

            If ds.Tables(0).Rows.Count > 0 Then
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    strComments += ds.Tables(0).Rows(i).Item("organizationName") & ":" & ControlChars.Tab & ds.Tables(0).Rows(i).Item("messages")
                    strComments += vbCrLf
                Next

            End If

            ds = db.SecureQueryParams("SELECT TOP 1000 [notificationId] " & _
          ",[createdBy] " & _
          ",[notificationEntity] " & _
          ",[creationDate] " & _
          ",[headerID] " & _
          ",[lineID] " & _
          ",[reviewed] " & _
          ",[DESCRIPTION] " & _
          ",[notificationTo] " & _
          ",[commentId] " & _
      "FROM [whUserNotificationsView] WHERE  notificationTo = @1 AND reviewed = 0 AND organizationID = @2 ORDER BY [creationDate] ASC", Environment.UserName, Me.organizationId)

            ttNotifications.RemoveAll()


            If ds.Tables(0).Rows.Count > 0 Then

                strComments += "SO/WIP/PO:" & ControlChars.Tab & "Order:" & ControlChars.Tab & ControlChars.Tab & "Created By:" & ControlChars.Tab & "Creation Date:" & ControlChars.Tab & "Comment:" & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab
                strComments += vbCrLf

                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    Dim strEntity As String
                    If ds.Tables(0).Rows(i).Item("notificationEntity") = "salesOrderLinesView" Then
                        strEntity = "SO"
                    ElseIf ds.Tables(0).Rows(i).Item("notificationEntity") = "WIPQueueView" Then
                        strEntity = "WIP"
                    Else
                        strEntity = "PO"
                    End If
                    strComments += strEntity & ControlChars.Tab & ControlChars.Tab

                    strComments += ds.Tables(0).Rows(i).Item("headerID").ToString & ControlChars.Tab & ControlChars.Tab

                    strComments += ds.Tables(0).Rows(i).Item("createdBy").ToString & ControlChars.Tab
                    strComments += Format(ds.Tables(0).Rows(i).Item("creationDate"), "dd/MM/yy") & ControlChars.Tab

                    If ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString.Count < 20 Then
                        strComments += ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString & ControlChars.Tab & ControlChars.Tab & ControlChars.Tab
                    ElseIf ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString.Count >= 20 AndAlso ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString.Count < 40 Then
                        strComments += ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString & ControlChars.Tab & ControlChars.Tab

                    ElseIf ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString.Count >= 40 AndAlso ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString.Count < 60 Then
                        strComments += ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString & ControlChars.Tab
                    Else
                        strComments += ds.Tables(0).Rows(i).Item("DESCRIPTION").ToString.Substring(0, 60) & ControlChars.Tab
                    End If


                    strComments += vbCrLf
                Next


            End If
            Dim x As Point = lblNotifications.Location
            x.Y = x.Y + 20
            x.X = x.X - 400

            'Show tooltip, a little aside the label!
            ttNotifications.Show(strComments, lblNotifications, x)
        Catch ex As Exception

        End Try


    End Sub


    Private notificationsLastLoad As Date = Now

    Private Sub loadCurrentNotifications()

        notificationsLastLoad = Now         'Setting the last load to now
        Dim ds As New DataSet
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        ds = db.SecureQueryParams("SELECT ISNULL(COUNT(*),0) FROM warehouseUserNotifications WHERE notificationTo = @1 AND reviewed = 0 AND organizationID = @2", Environment.UserName, Me.organizationId)

        Try
            If ds.Tables(0).Rows(0).Item(0) > 0 Then
                timerMessage.Start()
                lngCountTwinkle = 0
                lngCurrentNotification = ds.Tables(0).Rows(0).Item(0)
            ElseIf ds.Tables(0).Rows(0).Item(0) = 0 Then
                Me.Icon = My.Resources.Autobahn
            End If

            lblNotifications.Text = "Open Notifications: " & ds.Tables(0).Rows(0).Item(0).ToString

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("toolStripMenuItemBuy", ex, 1)
        End Try

    End Sub

    Public Sub loadNumberOfNotifications()
        Dim ds As New DataSet
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        ds = db.SecureQueryParams("SELECT ISNULL(COUNT(*),0) FROM warehouseUserNotifications WHERE notificationTo = @1 AND reviewed = 0 AND organizationID = @2", Environment.UserName, Me.organizationId)

        Try
            lblNotifications.Text = "Open Notifications: " & ds.Tables(0).Rows(0).Item(0).ToString

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("loadNumberOfNotifications", ex, 1)
        End Try
    End Sub



	'Rob is still working on this  mar9 2011 ***
	Private Sub checkNewTransactions()

		'Dim ds As New DataSet
		'Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
		'Dim sSql As String
		'Dim lngOrgID As Long
		'Dim lngHeaderID As Long


		'sSql = "SELECT * FROM warehouseTransactions WHERE  transactionType = 'salesOrderLinesView' AND (transactionAttempts IS NULL or transactionAttempts < 10"
		'Dim conn As New SqlClient.SqlConnection(My.Settings("FLOWConnectionString"))
		'Dim adp As New SqlClient.SqlDataAdapter(sSql, conn)
		'Dim cb As New SqlClient.SqlCommandBuilder(adp)
		'Dim dt As New DataTable
		'adp.Fill(dt)

		''cycle through each demand, and calculate if the part has any net shortages
		'If dt.Rows.Count > 0 Then
		'	For Each Row As DataRow In dt.Rows
		'		lngOrgID = Row("organizationID")
		'		lngHeaderID = Row("headerID")

		'		'look in Oracle to see if they succeeded - get org_id, header_id, line_id, and pick_status

		'		'if so, set the validatedDate, and update save Table
		'		'either way, increment the validationAttempts

		'	Next
		'	adp.Update(dt) ' Send the changes made in the datatable back to the database.  this will not work unless the table has a primary key defined.  i used demand_id for MRP Demand
		'End If
		'adp.Dispose()
		'dt.Dispose()


	End Sub



    Private Sub ControlTwinkleIcon(ByVal sender As Object, ByVal e As System.EventArgs)

        If Me.bReource Then
            Me.Icon = My.Resources.aütobahninform
            Me.bReource = False
            Me.BackColor = Color.DarkBlue
        Else
            Me.Icon = My.Resources.Autobahn
            Me.bReource = True
            Me.BackColor = Color.Beige
        End If

        If lngCountTwinkle = 5 Then
            timerMessage.Stop()
            Me.BackColor = Color.Beige
            Me.ForeColor = Color.Black
            Me.Icon = My.Resources.Autobahn

            'Find out if we are on a testmachine
            If Not My.Settings.FLOWConnectionString.Contains(My.Settings.prodMachineRecognizeName) Then
                If Not My.Settings.FLOWConnectionString = My.Settings.prodMachineRecognizeName Then         'It can also equal a production server
                    Me.BackColor = Color.Red
                    Me.ForeColor = Color.Black
                End If
            End If
        End If

        lngCountTwinkle += 1

    End Sub

    Private strColumnSorts(100) As String
    Private strSortStatement As String = ""
    Private strColumnSortsText(100) As String
    Private strSortDisplayStatement As String

    Private Sub dgvSalesOrders_ColumnHeaderMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvSalesOrders.ColumnHeaderMouseClick

        'Call the conditional format event
        'gridDisplay.formatGrid(dgvSalesOrders, "SalesOrderLinesView")
        'Create the multiple column sort
        If ModifierKeys = Keys.Control Then
            'Find out if the column is already in the list.
            For i As Integer = 0 To strColumnSorts.Length - 1
                If strColumnSorts(i) = dgvSalesOrders.Columns(e.ColumnIndex).Name Then
                    Exit For
                ElseIf strColumnSorts(i) = "" Then
                    strColumnSorts(i) = dgvSalesOrders.Columns(e.ColumnIndex).Name
                    strColumnSortsText(i) = dgvSalesOrders.Columns(e.ColumnIndex).HeaderText
                    Exit For
                End If
            Next
            'Create the combined sort string
            For i As Integer = 0 To strColumnSorts.Length - 1
                If i = 0 Then
                    strSortStatement = strColumnSorts(i)
                    strSortDisplayStatement = strColumnSortsText(i)
                ElseIf i > 0 AndAlso strColumnSorts(i) <> "" Then
                    strSortStatement += "," & strColumnSorts(i)
                    strSortDisplayStatement += "," & strColumnSortsText(i)
                End If
            Next
            'Finally call the sort procedure with the created string
            Try
                dsSalesOrder.Tables(0).DefaultView.Sort = strSortStatement
            Catch ex As Exception
                For i As Integer = 0 To strColumnSorts.Length - 1
                    strColumnSorts(i) = ""
                    strColumnSortsText(i) = ""
                    strSortStatement = ""
                    strSortDisplayStatement = ""
                Next
            End Try

            gridDisplay.formatGrid(dgvSalesOrders, "SalesOrderLinesView", Me.organizationId)

            If lblSalesOrderHeader.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblSalesOrderHeader.Text.Substring(0, lblSalesOrderHeader.Text.IndexOf("Sorted"))
                strNewStatement = strNewStatement.Trim() & "  Sorted By " & strSortDisplayStatement.Trim()
                lblSalesOrderHeader.Text = strNewStatement
            Else
                lblSalesOrderHeader.Text += "  Sorted By " & strSortDisplayStatement
            End If
        Else
            'Resetting the variable when the control button is not pressed
            If lblSalesOrderHeader.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblSalesOrderHeader.Text.Substring(0, lblSalesOrderHeader.Text.IndexOf("Sorted"))
                lblSalesOrderHeader.Text = strNewStatement.Trim()
            End If
            'Sorting Cust Order in Sales Order Grid
            If dsSalesOrder.Tables(0).DefaultView.Sort = "[OrderNoLine]" Then
                dsSalesOrder.Tables(0).DefaultView.Sort = "[Order_No], [Line_No]"
            ElseIf dsSalesOrder.Tables(0).DefaultView.Sort = "[Order_No], [Line_No]" Then
                dsSalesOrder.Tables(0).DefaultView.Sort = "[Order_No] DESC, [Line_No] DESC"
            ElseIf dsSalesOrder.Tables(0).DefaultView.Sort = "[Order_No] DESC, [Line_No] DESC" Then
                dsSalesOrder.Tables(0).DefaultView.Sort = "[Order_No], [Line_No]"
            ElseIf dgvSalesOrders.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Programmatic Then
                dsSalesOrder.Tables(0).DefaultView.Sort = "[Order_No], [Line_No]"
            End If

            dgvSalesOrders.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Automatic
            If dsSalesOrder.Tables(0).DefaultView.Sort.Contains("[Order_No], [Line_No]") Then
                dgvSalesOrders.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Programmatic
                dgvSalesOrders.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = SortOrder.Ascending
            ElseIf dsSalesOrder.Tables(0).DefaultView.Sort.Contains("[Order_No] DESC, [Line_No] DESC") Then
                dgvSalesOrders.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Programmatic
                dgvSalesOrders.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = SortOrder.Descending
            End If
            gridDisplay.formatGrid(dgvSalesOrders, "SalesOrderLinesView", Me.organizationId)

            For i As Integer = 0 To strColumnSorts.Length - 1
                strColumnSorts(i) = ""
                strColumnSortsText(i) = ""
                strSortStatement = ""
                strSortDisplayStatement = ""
            Next
            End If

    End Sub

    Private Sub dgvShortages_ColumnHeaderMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvShortages.ColumnHeaderMouseClick

        '***have to consider that the old variables may not be empty
        'Call the conditional format event
        'gridDisplay.formatGrid(dgvSalesOrders, "SalesOrderLinesView")
        'Create the multiple column sort
        If ModifierKeys = Keys.Control Then
            'Find out if the column is already in the list.
            For i As Integer = 0 To strColumnSorts.Length - 1
                If strColumnSorts(i) = dgvShortages.Columns(e.ColumnIndex).Name Then
                    Exit For
                ElseIf strColumnSorts(i) = "" Then
                    strColumnSorts(i) = dgvShortages.Columns(e.ColumnIndex).Name
                    strColumnSortsText(i) = dgvShortages.Columns(e.ColumnIndex).HeaderText
                    Exit For
                End If
            Next
            'Create the combined sort string
            For i As Integer = 0 To strColumnSorts.Length - 1
                If i = 0 Then
                    strSortStatement = strColumnSorts(i)
                    strSortDisplayStatement = strColumnSortsText(i)
                ElseIf i > 0 AndAlso strColumnSorts(i) <> "" Then
                    strSortStatement += "," & strColumnSorts(i)
                    strSortDisplayStatement += "," & strColumnSortsText(i)
                End If
            Next
            'Finally call the sort procedure with the created string
            Try
                dgvShortages.DataSource.DefaultView.Sort = strSortStatement
            Catch ex As Exception
                For i As Integer = 0 To strColumnSorts.Length - 1
                    strColumnSorts(i) = ""
                    strColumnSortsText(i) = ""
                    strSortStatement = ""
                    strSortDisplayStatement = ""
                Next
            End Try

            'Distinct between the two possibilities
            'Distinct between supply demand and shortages!
            If Me.strMiddleGrid = "MRPShortagesView" Then
                gridDisplay.formatGrid(dgvShortages, "MRPShortagesView")
            Else
                gridDisplay.formatGrid(dgvShortages, "supplyDemandMRPView")
            End If
            If lblShortageDescription.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblShortageDescription.Text.Substring(0, lblShortageDescription.Text.IndexOf("Sorted"))
                strNewStatement = strNewStatement.Trim() & "  Sorted By " & strSortDisplayStatement.Trim()
                lblShortageDescription.Text = strNewStatement
            Else
                lblShortageDescription.Text += "  Sorted By " & strSortDisplayStatement
            End If
        Else
            'Resetting the variable when the control button is not pressed
            If lblShortageDescription.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblShortageDescription.Text.Substring(0, lblShortageDescription.Text.IndexOf("Sorted"))
                lblShortageDescription.Text = strNewStatement.Trim()
            End If
            '
            If Me.strMiddleGrid = "MRPShortagesView" Then
                gridDisplay.formatGrid(dgvShortages, "MRPShortagesView")
            Else
                gridDisplay.formatGrid(dgvShortages, "supplyDemandMRPView")
            End If
            For i As Integer = 0 To strColumnSorts.Length - 1
                strColumnSorts(i) = ""
                strColumnSortsText(i) = ""
                strSortStatement = ""
                strSortDisplayStatement = ""
            Next
        End If

        'Distinct between supply demand and shortages!
        'If Me.strMiddleGrid = "MRPShortagesView" Then
        '    gridDisplay.formatGrid(dgvShortages, "MRPShortagesView")
        'Else
        '    gridDisplay.formatGrid(dgvShortages, "supplyDemandMRPView")
        'End If

    End Sub

    Private Sub dgvWipSupplierQueue_ColumnHeaderMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvWipSupplierQueue.ColumnHeaderMouseClick

        '***have to consider that the old variables may not be empty
        'Call the conditional format event
        'gridDisplay.formatGrid(dgvSalesOrders, "SalesOrderLinesView")
        'Create the multiple column sort
        If ModifierKeys = Keys.Control Then
            'Find out if the column is already in the list.
            For i As Integer = 0 To strColumnSorts.Length - 1
                If strColumnSorts(i) = dgvWipSupplierQueue.Columns(e.ColumnIndex).Name Then
                    Exit For
                ElseIf strColumnSorts(i) = "" Then
                    strColumnSorts(i) = dgvWipSupplierQueue.Columns(e.ColumnIndex).Name
                    strColumnSortsText(i) = dgvWipSupplierQueue.Columns(e.ColumnIndex).HeaderText
                    Exit For
                End If
            Next
            'Create the combined sort string
            For i As Integer = 0 To strColumnSorts.Length - 1
                If i = 0 Then
                    strSortStatement = strColumnSorts(i)
                    strSortDisplayStatement = strColumnSortsText(i)
                ElseIf i > 0 AndAlso strColumnSorts(i) <> "" Then
                    strSortStatement += "," & strColumnSorts(i)
                    strSortDisplayStatement += "," & strColumnSortsText(i)
                End If
            Next
            'Finally call the sort procedure with the created string
            Try
                dgvWipSupplierQueue.DataSource.DefaultView.Sort = strSortStatement
            Catch ex As Exception
                For i As Integer = 0 To strColumnSorts.Length - 1
                    strColumnSorts(i) = ""
                    strColumnSortsText(i) = ""
                    strSortStatement = ""
                    strSortDisplayStatement = ""
                Next
            End Try

            'Distinct between the two possibilities
            'Distinct between supply demand and shortages!
            If Me.bottonGrid = "po" Then
                'dsWIP.Tables(0)
                gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
            Else
                gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
            End If
            If lblWIPPOFilter.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblWIPPOFilter.Text.Substring(0, lblWIPPOFilter.Text.IndexOf("Sorted"))
                strNewStatement += "  Sorted By " & strSortDisplayStatement
                lblPOWIPIndicators.Text = strNewStatement
            Else
                lblWIPPOFilter.Text += "  Sorted By " & strSortDisplayStatement
            End If
        Else
            'Resetting the variable when the control button is not pressed
            If lblWIPPOFilter.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblWIPPOFilter.Text.Substring(0, lblWIPPOFilter.Text.IndexOf("Sorted"))
                lblWIPPOFilter.Text = strNewStatement.Trim()
            End If
            '
            If Me.bottonGrid = "po" Then
                gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
            Else
                'Sorting the Job field for WIP Operations Sequence grid
                If dgvWipSupplierQueue.DataSource.DefaultView.Sort = "[JobNoOperationSeq]" Then
                    dgvWipSupplierQueue.DataSource.DefaultView.Sort = "[JOB_NO], [OPERATION_SEQ_NO]"
                ElseIf dgvWipSupplierQueue.DataSource.DefaultView.Sort = "[JOB_NO], [OPERATION_SEQ_NO]" Then
                    dgvWipSupplierQueue.DataSource.DefaultView.Sort = "[JOB_NO] DESC, [OPERATION_SEQ_NO] DESC"
                ElseIf dgvWipSupplierQueue.DataSource.DefaultView.Sort = "[JOB_NO] DESC, [OPERATION_SEQ_NO] DESC" Then
                    dgvWipSupplierQueue.DataSource.DefaultView.Sort = "[JOB_NO], [OPERATION_SEQ_NO]"
                ElseIf dgvWipSupplierQueue.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Programmatic Then
                    dgvWipSupplierQueue.DataSource.DefaultView.Sort = "[JOB_NO], [OPERATION_SEQ_NO]"
                End If

                dgvWipSupplierQueue.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Automatic
                If dgvWipSupplierQueue.DataSource.DefaultView.Sort.Contains("[JOB_NO], [OPERATION_SEQ_NO]") Then
                    dgvWipSupplierQueue.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Programmatic
                    dgvWipSupplierQueue.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = SortOrder.Ascending
                ElseIf dgvWipSupplierQueue.DataSource.DefaultView.Sort.Contains("[JOB_NO] DESC, [OPERATION_SEQ_NO] DESC") Then
                    dgvWipSupplierQueue.Columns(e.ColumnIndex).SortMode = DataGridViewColumnSortMode.Programmatic
                    dgvWipSupplierQueue.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = SortOrder.Descending
                End If
                gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
            End If
            For i As Integer = 0 To strColumnSorts.Length - 1
                strColumnSorts(i) = ""
                strColumnSortsText(i) = ""
                strSortStatement = ""
                strSortDisplayStatement = ""
            Next
        End If

        If Me.bottonGrid = "po" Then
            Me.createSearchListPo()
        Else
            Me.createSearchListWIP()
        End If

    End Sub

    Private Sub ResetFilterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetFilterToolStripMenuItem.Click
        Me.populateGrids()


    End Sub

    Private Sub ChangeGridToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeGridToolStripMenuItem1.Click

        'Turns off the eventually applied temporary filter and deletes po and wip filters of the user. 
        dgvWipSupplierQueue.EnableHeadersVisualStyles = True
        Me.deleteTemporaryFilters(False, False, True, True)

        'Switches between purchase orders and wip discrete jobs.
        If bottonGrid = "po" Then
            'Find out before if a filter was applied.
            If dsWIP.Tables(0).Rows.Count < lngCountWIPDiscreteJobsOperationSequences Then
                Me.loadWip("")
            End If

            bottonGrid = "wip"
            dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
            gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
            dgvWipSupplierQueue.ContextMenuStrip = cmsBottonWip
            lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters"
        Else
            bottonGrid = "po"
            If dsPO.Tables(0).Rows.Count < lngCountPurchaseOrderLines Then
                Me.loadPO("")
            End If
            dgvWipSupplierQueue.DataSource = dsPO.Tables(0)
            gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
            dgvWipSupplierQueue.ContextMenuStrip = cmsBotton
            lblWIPPOFilter.Text = "Open Purchase Order Lines - No Filters"

        End If

    End Sub

    Private Sub CopyToClipboardToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToClipboardToolStripMenuItem1.Click
        Me.copyGridOrCell(dgvShortages)
    End Sub

    Private Sub copyGridOrCell(ByRef dgv As DataGridView)
        Try
            Dim data_object As New DataObject
            Dim stext As String = ""

            If dgv.SelectedRows.Count > 0 Then
                'Add the headers
                For j As Integer = 0 To dgv.Columns.Count - 1
                    If dgv.Columns(j).Visible Then
                        ' stext += dgv.Columns(j).HeaderText & ControlChars.Tab
                    End If
                    '
                Next

                stext += vbCrLf
                For i As Integer = 0 To dgv.SelectedRows.Count - 1
                    'Add the content
                    For j As Integer = 0 To dgv.Columns.Count - 1
                        If dgv.Columns(j).Visible Then
                            'Just if not more than 25 charactes
                            If dgv.SelectedRows(i).Cells(j).Value.ToString.Count < 20 Then
                                stext += dgv.SelectedRows(i).Cells(j).Value.ToString & ControlChars.Tab
                            Else
                                stext += dgv.SelectedRows(i).Cells(j).Value.ToString.Substring(0, 20) & ControlChars.Tab
                            End If
                        End If
                        '
                    Next
                    stext += vbCrLf
                Next
            Else
                'Just add the item
                stext += dgv.SelectedCells(0).Value.ToString
            End If

            ' Add the data in various formats.
            data_object.SetData(DataFormats.Text, stext)

            ' Copy data to the clipboard.
            Clipboard.SetDataObject(data_object)
        Catch ex As Exception

        End Try
        
    End Sub


    Private Sub ExportToXMLToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToXMLToolStripMenuItem1.Click
        sfdExcelPlace.Tag = "POQueueView"
        sfdExcelPlace.ShowDialog()

    End Sub

    Private Sub ExportToXMLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToXMLToolStripMenuItem.Click
        sfdExcelPlace.Tag = "MRPShortagesView"
        sfdExcelPlace.ShowDialog()

    End Sub

    Private Sub ExportToXMLToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToXMLToolStripMenuItem3.Click
        sfdExcelPlace.Tag = "WIPQueueView"
        sfdExcelPlace.ShowDialog()

    End Sub

    Private Sub CopyCellOrRowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyCellOrRowToolStripMenuItem.Click
        Me.copyGridOrCell(dgvWipSupplierQueue)
    End Sub

    Private Sub CopyCellOrRowsToClipboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyCellOrRowsToClipboardToolStripMenuItem.Click
        Me.copyGridOrCell(dgvSalesOrders)
    End Sub

    Private Sub CopyCellOrRowsToClipboardToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyCellOrRowsToClipboardToolStripMenuItem1.Click
        Me.copyGridOrCell(dgvWipSupplierQueue)
    End Sub

    Private Sub lblNotifications_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblNotifications.MouseLeave
        Dim x As Point = lblNotifications.Location
        x.Y = x.Y + 20

        ttNotifications.Show("", lblNotifications, x)


    End Sub


    Public Sub jumpAndFindNotificationWithoutUpdate(ByVal commentId As Integer)
        Try
            'Dim bFound As Boolean = False
            Dim ds As New DataSet
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

            ds = db.SecureQueryParams("SELECT TOP 1000 [created_By] ,[Entity_id] " & _
                                    ",[creation_Date] ,[header_ID] ,[line_ID] ,[DESCRIPTION]  " & _
                                    ",[commentId] ,[headerName] ,[shipment_no] ,[releaseNo] " & _
                                    " FROM comments WHERE  commentId = @1 " & _
                                    " AND organizationID = @2 ORDER BY [creation_Date] ASC", commentId, Me.organizationId)

            'Jump to the point of the grid and open the comment box
            Me.row = 0

            If ds.Tables(0).Rows.Count > 0 Then

                '>>>> BE CAREFUL IF A FILTER IS APPLIED YOU MAY NOT BE GUIDEDE TO THE ROW YET
                'Looking if one of the filter was applied

                If Me.bottonGrid = "wip" Then
                    If strFilterWip.Count > 0 Or strFilterWIPTemp.Count > 0 Then
                        Me.loadWip("")
                        Me.bindWipToGrid()
                        strFilterWip = ""
                        strFilterWIPTemp = ""
                    End If
                Else
                    If strFilterPO <> "" Or strFilterPOTemp <> "" Then
                        Me.loadPO("")
                        strFilterPO = ""
                        strFilterPOTemp = ""
                    End If
                End If

                If strFilterSalesOrder <> "" Or strFilterSalesOrderTemp <> "" Then
                    Me.loadSalesOrder("")
                    strFilterSalesOrder = ""
                    strFilterSalesOrderTemp = ""
                End If

                If ds.Tables(0).Rows(0).Item("Entity_id") = "salesOrderLinesView" Then


                    'Gather the row cycle to it and hand it over to the dialogue
                    searchString = ds.Tables(0).Rows(0).Item("header_id").ToString
                    For i As Integer = 0 To dgvSalesOrders.RowCount - 1


                        'Search the whole row will be a little bit more sophisticated, but implement later

                        'Look if the value contains the search string
                        If dgvSalesOrders.Rows(i).Cells("HEADER_ID").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("header_id")) AndAlso dgvSalesOrders.Rows(i).Cells("LINE_NO").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("line_id")) Then
                            'activate the row and the cell we found
                            dgvSalesOrders.Rows(i).Selected = True
                            dgvSalesOrders.FirstDisplayedScrollingRowIndex = i
                            dgvSalesOrders.Rows(i).Cells("lastComment").Value = ds.Tables(0).Rows(0).Item("DESCRIPTION")

                            dgvSalesOrders.FirstDisplayedCell = dgvSalesOrders.Rows(i).Cells("OrderNoLine")
                            dgvSalesOrders.Rows(i).Cells("HEADER_ID").Selected = True
                            Me.row = i

                            Exit For
                        End If
                    Next

                    ' Dim dlg As New dlgComments(ds.Tables(0).Rows(0).Item("headerID"), ds.Tables(0).Rows(0).Item("headerName"), ds.Tables(0).Rows(0).Item("lineID"), 0, "salesOrderLinesView", dgvSalesOrders, Me.row, Me.organizationId)
                    ' dlg.ShowDialog()
                    'If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                    'Me.loadCurrentNotifications()
                    'db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", ds.Tables(0).Rows(0).Item("notificationId"))
                    'End If

                End If

                If ds.Tables(0).Rows(0).Item("Entity_id") = "POQueueView" Then

                    If bottonGrid = "wip" Then
                        bottonGrid = "po"
                        dgvWipSupplierQueue.DataSource = dsPO.Tables(0)
                        gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
                        dgvWipSupplierQueue.ContextMenuStrip = cmsBotton
                    End If

                    'Gather the row cycle to it and hand it over to the dialogue
                    searchString = ds.Tables(0).Rows(0).Item("header_id").ToString
                    For i As Integer = 0 To dgvWipSupplierQueue.RowCount - 1

                        'Search the whole row will be a little bit more sophisticated, but implement later
                        Dim strRelease As String = ds.Tables(0).Rows(0).Item("releaseNo").ToString
                        If strRelease = "0" Then strRelease = ""

                        'Look if the value contains the search string
                        If dgvWipSupplierQueue.Rows(i).Cells("po_header_id").Value.ToString.Equals(ds.Tables(0).Rows(0).Item("header_id").ToString) _
                        AndAlso dgvWipSupplierQueue.Rows(i).Cells("LINE_NO").Value.ToString.Equals(ds.Tables(0).Rows(0).Item("line_id").ToString) _
                        AndAlso dgvWipSupplierQueue.Rows(i).Cells("SHIPMENT_NO").Value.ToString.Equals(ds.Tables(0).Rows(0).Item("shipment_no").ToString) _
                        AndAlso dgvWipSupplierQueue.Rows(i).Cells("RELEASE_NO").Value.ToString.Equals(strRelease) Then
                            'activate the row and the cell we found
                            dgvWipSupplierQueue.Rows(i).Selected = True
                            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i
                            'This was a dangerous acitivty with the contains staements
                            dgvWipSupplierQueue.Rows(i).Cells("lastComment").Value = ds.Tables(0).Rows(0).Item("DESCRIPTION")

                            'dgvWipSupplierQueue.FirstDisplayedCell = dgvWipSupplierQueue.Rows(i).Cells("ITEM_NO")
                            'dgvWipSupplierQueue.Rows(i).Cells("ITEM_NO").Selected = True
                            Me.row = i

                            Exit For
                        End If
                    Next

                    'Dim dlg As New dlgComments(ds.Tables(0).Rows(0).Item("headerID"), ds.Tables(0).Rows(0).Item("headerName"), ds.Tables(0).Rows(0).Item("lineID"), ds.Tables(0).Rows(0).Item("shipment_no"), "POQueueView", dgvWipSupplierQueue, Me.row, Me.organizationId, ds.Tables(0).Rows(0).Item("releaseNo"))
                    'dlg.ShowDialog()
                    'If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                    '    Me.loadCurrentNotifications()
                    '    ' db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", ds.Tables(0).Rows(0).Item("notificationId"))
                    'End If

                End If

                If ds.Tables(0).Rows(0).Item("Entity_id") = "WIPQueueView" Then

                    If bottonGrid = "po" Then
                        bottonGrid = "wip"
                        dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
                        gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
                        dgvWipSupplierQueue.ContextMenuStrip = cmsBottonWip
                    End If

                    'Gather the row cycle to it and hand it over to the dialogue
                    searchString = ds.Tables(0).Rows(0).Item("header_ID").ToString
                    For i As Integer = 0 To dgvWipSupplierQueue.RowCount - 1

                        'Search the whole row will be a little bit more sophisticated, but implement later

                        'Look if the value contains the search string
                        If dgvWipSupplierQueue.Rows(i).Cells("wip_entity_id").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("header_id")) AndAlso dgvWipSupplierQueue.Rows(i).Cells("OPERATION_SEQ_NO").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("line_id")) Then
                            'activate the row and the cell we found
                            dgvWipSupplierQueue.Rows(i).Selected = True
                            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i
                            dgvWipSupplierQueue.Rows(i).Cells("lastComment").Value = ds.Tables(0).Rows(0).Item("DESCRIPTION")

                            dgvWipSupplierQueue.FirstDisplayedCell = dgvWipSupplierQueue.Rows(i).Cells("DEPARTMENT_CODE")
                            dgvWipSupplierQueue.Rows(i).Cells("DEPARTMENT_CODE").Selected = True
                            Me.row = i

                            Exit For
                        End If
                    Next

                    'Dim dlg As New dlgComments(ds.Tables(0).Rows(0).Item("headerID"), ds.Tables(0).Rows(0).Item("headerName"), ds.Tables(0).Rows(0).Item("lineID"), 0, "WIPQueueView", dgvSalesOrders, Me.row, Me.organizationId)
                    'dlg.ShowDialog()
                    'If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                    '    Me.loadCurrentNotifications()

                    '    'Just update the notification when the user presses ok, if he presses cancel he can go back
                    '    ' db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", ds.Tables(0).Rows(0).Item("notificationId"))
                    '    'lngCurrentNotification -= 1
                    'End If

                End If

                If ds.Tables(0).Rows(0).Item("Entity_id") = "plannedOrder" Then
                    Me.populateMRPShortages(0, Double.Parse(Me.organizationId), " AND ORDER_NO = " & ds.Tables(0).Rows(0).Item("HEADER_ID") & " AND LINE_NO = " & ds.Tables(0).Rows(0).Item("LINE_ID") & "", 0, " ", True, True)
                    'can happen to have one end_demand_id against multiple sales orders
                    If dgvShortages.Rows.Count = 0 Then
                        Me.populateMRPShortages(0, Double.Parse(Me.organizationId), " AND ORDER_NO = " & ds.Tables(0).Rows(0).Item("HEADER_ID") & " ", 0, " ", True, True)
                    End If
                    For i As Integer = 0 To dgvShortages.Rows.Count - 1
                        If dgvShortages.Rows(i).Cells("INVENTORY_ITEM_ID").Value = ds.Tables(0).Rows(0).Item("shipment_no") Then
                            dgvShortages.Rows(i).Selected = True
                            dgvShortages.FirstDisplayedScrollingRowIndex = i
                            dgvShortages.Rows(i).Cells("lastComment").Value = ds.Tables(0).Rows(0).Item("DESCRIPTION")
                            Exit For
                        End If
                    Next
                End If

            End If

            'Me.loadCurrentNotifications()
        Catch ex As Exception

        End Try


    End Sub


    Private Sub jumpAndFindNotification(Optional ByVal notificationId As Integer = 0)

        Try

            Dim ds As New DataSet
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

            ds = db.SecureQueryParams("SELECT TOP 1000 [notificationId] " & _
                                      ",[createdBy] " & _
                                      ",[notificationEntity] " & _
                                      ",[creationDate] " & _
                                      ",[headerID] " & _
                                      ",[lineID] " & _
                                      ",[reviewed] " & _
                                      ",[DESCRIPTION] " & _
                                      ",[notificationTo] " & _
                                      ",[commentId] " & _
                                      ",[headerName] " & _
                                      ",[shipment_no] " & _
                                      ",[releaseNo] " & _
                                    "FROM [whUserNotificationsView] WHERE  notificationTo = @1 AND reviewed = 0 AND organizationID = @2 ORDER BY [creationDate] ASC", Environment.UserName, Me.organizationId)

            'Jump to the point of the grid and open the comment box
            Me.row = 0

            If ds.Tables(0).Rows.Count > 0 Then

                '>>>> BE CAREFUL IF A FILTER IS APPLIED YOU MAY NOT BE GUIDEDE TO THE ROW YET
                'Looking if one of the filter was applied

                If Me.bottonGrid = "wip" Then
                    If strFilterWip <> "" Or strFilterWIPTemp <> "" Then
                        Me.loadWip("")
                    End If
                Else
                    If strFilterPO <> "" Or strFilterPOTemp <> "" Then
                        Me.loadPO("")
                    End If
                End If

                If strFilterSalesOrder <> "" Or strFilterSalesOrderTemp <> "" Then
                    Me.loadSalesOrder("")
                End If

                If ds.Tables(0).Rows(0).Item("notificationEntity") = "salesOrderLinesView" Then


                    'Gather the row cycle to it and hand it over to the dialogue
                    searchString = ds.Tables(0).Rows(0).Item("headerID").ToString
                    For i As Integer = 0 To dgvSalesOrders.RowCount - 1


                        'Search the whole row will be a little bit more sophisticated, but implement later

                        'Look if the value contains the search string
                        If dgvSalesOrders.Rows(i).Cells("HEADER_ID").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("headerID")) AndAlso dgvSalesOrders.Rows(i).Cells("LINE_NO").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("lineID")) Then
                            'activate the row and the cell we found
                            dgvSalesOrders.Rows(i).Selected = True
                            dgvSalesOrders.FirstDisplayedScrollingRowIndex = i
                            dgvSalesOrders.Rows(i).Cells("lastComment").Value = ds.Tables(0).Rows(0).Item("DESCRIPTION")

                            dgvSalesOrders.FirstDisplayedCell = dgvSalesOrders.Rows(i).Cells("OrderNoLine")
                            dgvSalesOrders.Rows(i).Cells("HEADER_ID").Selected = True
                            Me.row = i

                            Exit For
                        End If
                    Next

                    Dim dlg As New dlgComments(ds.Tables(0).Rows(0).Item("headerID"), ds.Tables(0).Rows(0).Item("headerName"), ds.Tables(0).Rows(0).Item("lineID"), 0, "salesOrderLinesView", dgvSalesOrders, Me.row, Me.organizationId)
                    dlg.ShowDialog()

                    If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                        Me.loadCurrentNotifications()
                        db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", ds.Tables(0).Rows(0).Item("notificationId"))
                    End If

                End If

                If ds.Tables(0).Rows(0).Item("notificationEntity") = "POQueueView" Then

                    If bottonGrid = "wip" Then
                        bottonGrid = "po"
                        dgvWipSupplierQueue.DataSource = dsPO.Tables(0)
                        gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
                        dgvWipSupplierQueue.ContextMenuStrip = cmsBotton
                    End If

                    'Gather the row cycle to it and hand it over to the dialogue
                    searchString = ds.Tables(0).Rows(0).Item("headerID").ToString
                    For i As Integer = 0 To dgvWipSupplierQueue.RowCount - 1

                        'Search the whole row will be a little bit more sophisticated, but implement later
                        'We want to apply that Release NO from db is 0.0 though in the grid it is "" an empty string
                        Dim strReleaseNo As String = ds.Tables(0).Rows(0).Item("releaseNo").ToString
                        If strReleaseNo = "0.0" Or strReleaseNo = "0" Then strReleaseNo = ""

                        'Look if the value contains the search string
                        If dgvWipSupplierQueue.Rows(i).Cells("po_header_id").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("headerID")) _
                        AndAlso dgvWipSupplierQueue.Rows(i).Cells("LINE_NO").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("lineID")) _
                        AndAlso dgvWipSupplierQueue.Rows(i).Cells("SHIPMENT_NO").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("shipment_no")) _
                        AndAlso dgvWipSupplierQueue.Rows(i).Cells("RELEASE_NO").Value.ToString.Contains(strReleaseNo) Then
                            'activate the row and the cell we found
                            dgvWipSupplierQueue.Rows(i).Selected = True
                            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i
                            dgvWipSupplierQueue.Rows(i).Cells("lastComment").Value = ds.Tables(0).Rows(0).Item("DESCRIPTION")


                            dgvWipSupplierQueue.FirstDisplayedCell = dgvWipSupplierQueue.Rows(i).Cells("ITEM_NO")
                            dgvWipSupplierQueue.Rows(i).Cells("ITEM_NO").Selected = True
                            Me.row = i

                            Exit For
                        End If
                    Next

                    Dim dlg As New dlgComments(ds.Tables(0).Rows(0).Item("headerID"), ds.Tables(0).Rows(0).Item("headerName"), ds.Tables(0).Rows(0).Item("lineID"), ds.Tables(0).Rows(0).Item("shipment_no"), "POQueueView", dgvWipSupplierQueue, Me.row, Me.organizationId, ds.Tables(0).Rows(0).Item("releaseNo"))
                    dlg.ShowDialog()
                    If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                        Me.loadCurrentNotifications()
                        db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", ds.Tables(0).Rows(0).Item("notificationId"))
                    End If

                End If

                If ds.Tables(0).Rows(0).Item("notificationEntity") = "WIPQueueView" Then

                    If bottonGrid = "po" Then
                        bottonGrid = "wip"
                        dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
                        gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
                        dgvWipSupplierQueue.ContextMenuStrip = cmsBottonWip
                    End If
                    Me.bindWipToGrid()

                    'Gather the row cycle to it and hand it over to the dialogue
                    searchString = ds.Tables(0).Rows(0).Item("headerID").ToString
                    For i As Integer = 0 To dgvWipSupplierQueue.RowCount - 1

                        'Search the whole row will be a little bit more sophisticated, but implement later

                        'Look if the value contains the search string
                        If dgvWipSupplierQueue.Rows(i).Cells("wip_entity_id").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("headerID")) AndAlso dgvWipSupplierQueue.Rows(i).Cells("OPERATION_SEQ_NO").Value.ToString.Contains(ds.Tables(0).Rows(0).Item("lineID")) Then
                            'activate the row and the cell we found
                            dgvWipSupplierQueue.Rows(i).Selected = True
                            dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i
                            dgvWipSupplierQueue.Rows(i).Cells("lastComment").Value = ds.Tables(0).Rows(0).Item("DESCRIPTION")

                            dgvWipSupplierQueue.FirstDisplayedCell = dgvWipSupplierQueue.Rows(i).Cells("DEPARTMENT_CODE")
                            dgvWipSupplierQueue.Rows(i).Cells("DEPARTMENT_CODE").Selected = True
                            Me.row = i

                            Exit For
                        End If
                    Next

                    Dim dlg As New dlgComments(ds.Tables(0).Rows(0).Item("headerID"), ds.Tables(0).Rows(0).Item("headerName"), ds.Tables(0).Rows(0).Item("lineID"), 0, "WIPQueueView", dgvWipSupplierQueue, Me.row, Me.organizationId)
                    dlg.ShowDialog()
                    If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                        Me.loadCurrentNotifications()

                        'Just update the notification when the user presses ok, if he presses cancel he can go back
                        db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", ds.Tables(0).Rows(0).Item("notificationId"))
                        lngCurrentNotification -= 1
                    End If

                End If

                If ds.Tables(0).Rows(0).Item("notificationEntity") = "plannedOrder" Then
                    'Me.populateMRPShortages(0, Double.Parse(Me.organizationId), filterString, 0, " ", True, True)
                    Me.populateMRPShortages(0, Double.Parse(Me.organizationId), " AND ORDER_NO = " & ds.Tables(0).Rows(0).Item("headerID") & " AND LINE_NO = " & ds.Tables(0).Rows(0).Item("lineID") & "", 0, " ", True, True)
                    For i As Integer = 0 To dgvShortages.Rows.Count - 1
                        If dgvShortages.Rows(i).Cells("INVENTORY_ITEM_ID").Value = ds.Tables(0).Rows(0).Item("shipment_no") Then
                            dgvShortages.Rows(i).Selected = True
                            dgvShortages.FirstDisplayedScrollingRowIndex = i
                            dgvShortages.Rows(i).Cells("lastComment").Value = ds.Tables(0).Rows(0).Item("DESCRIPTION")
                            Exit For
                        End If
                    Next

                    Dim dlg As New dlgComments(ds.Tables(0).Rows(0).Item("headerID"), ds.Tables(0).Rows(0).Item("headerName"), ds.Tables(0).Rows(0).Item("lineID"), ds.Tables(0).Rows(0).Item("shipment_no"), "plannedOrder", dgvShortages, Me.row, Me.organizationId)
                    dlg.ShowDialog()
                    If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                        Me.loadCurrentNotifications()

                        'Just update the notification when the user presses ok, if he presses cancel he can go back
                        db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", ds.Tables(0).Rows(0).Item("notificationId"))
                        lngCurrentNotification -= 1
                    End If

                End If

            End If

            Me.loadCurrentNotifications()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lblNotifications_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblNotifications.Click
        Me.jumpAndFindNotification()

    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.pfAll.Form = Me
        Me.pfAll.DocumentName = "Test"
        Me.pfAll.PrintAction = Printing.PrintAction.PrintToPreview
        Me.pfAll.Print()
    End Sub

    'Following four funtions handle the selected value event.
    Private Sub SelectedValueToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectedValueToolStripMenuItem.Click
        Dim strFilterString As String = Me.filterGridForSelectedValue(dgvShortages)
        Me.populateMRPShortages(lnDemandId, Me.organizationId, strFilterString)
        'Implement later on also the supply demand logic!
    End Sub

    Private Sub SelectedValueToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectedValueToolStripMenuItem1.Click
        Dim strFilterString As String = Me.filterGridForSelectedValue(dgvWipSupplierQueue)
        Me.loadPO(strFilterString)
        Dim numberOfRecordsDisplayed As Integer = dsWIP.Tables(0).Compute("COUNT(PONoLine)", "PONoLine IS NOT NULL")
        lblWIPPOFilter.Text = "Filtered to Selected Value: " & numberOfRecordsDisplayed & " of " & Me.lngCountPurchaseOrderLines & " Lines Displayed."
    End Sub

    Private Sub SelectedValueToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectedValueToolStripMenuItem3.Click
        Dim strFilterString As String = Me.filterGridForSelectedValue(dgvWipSupplierQueue)
        Me.loadWip(strFilterString)
        Me.bindWipToGrid()
        Dim numberOfRecordsDisplayed As Integer = Me.numberOfRecordsDisplayedForWipPO(dsWIP.Tables(0))

        lblWIPPOFilter.Text = "Filtered to Selected Value: " & numberOfRecordsDisplayed & " of " & Me.lngCountWIPDiscreteJobsOperationSequences & " Operations Displayed."
    End Sub


    Private Sub SelectedValueToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectedValueToolStripMenuItem2.Click
        Dim strValue As String = ""
        Try
            Dim strFilterString As String = Me.filterGridForSelectedValue(dgvSalesOrders)
            Me.loadSalesOrder(strFilterString)

            'Loading the aggregate value of the sales order and set the label to the value
            Dim sumObject As Object
            sumObject = dsSalesOrder.Tables(0).Compute("Sum(Extended_Price)", "") / 1000
            strValue = Format(sumObject, "C0") + "k"
            lblSalesOrderHeader.Text = "Filtered to Selected Value: " & dgvSalesOrders.Rows.Count & " of " & Me.lngCountSalesOrderLines & " Lines Displayed." & " - Value:" & strValue
        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' Function which handles the selected value functionality.
    ''' </summary>
    ''' <param name="dgv">The datagridview which should be filtered.</param>
    ''' <returns>The filter string to be handed over to the backend.</returns>
    ''' <remarks></remarks>
    Private Function filterGridForSelectedValue(ByRef dgv As DataGridView) As String

        Dim strFilterString As String = " AND ( "
        Dim lngNotNull As Double = 0

        'See if there was a value selected
        If dgv.SelectedCells.Count > 0 Then
            For i As Integer = 0 To dgv.SelectedCells.Count - 1
                'If it is a dbnull value jump to the else statement and just say is null
                If Not DBNull.Value.Equals(dgv.SelectedCells(i).Value) Then

                    'For string values include apostrophe
                    If dgv.SelectedCells(i).ValueType.Name = "String" Then
                        strFilterString += dgv.SelectedCells(i).OwningColumn.Name & " = '" & dgv.SelectedCells(i).Value & "'"

                        'Filter the datetime column
                    ElseIf dgv.SelectedCells(i).ValueType.Name = "DateTime" Then
                        Dim d As New DateTime
                        Try
                            'Try to parse the value of the field
                            d = dgv.SelectedCells(i).Value
                            strFilterString += dgv.SelectedCells(i).OwningColumn.Name & " = " & "'" & Format(d, "yyyyMMdd") & "'"
                        Catch ex As Exception
                            MessageBox.Show("Please enter a valid date time value. Will use today instead.")

                        End Try
                    Else
                        'The last element can just be a number, so no apostrophes
                        strFilterString += dgv.SelectedCells(i).OwningColumn.Name & " = " & dgv.SelectedCells(i).Value

                    End If

                If dgv.SelectedCells.Count > 2 Then
                        strFilterString += " OR "
                End If
                Else
                    'handle the is null event
                    strFilterString += dgv.SelectedCells(i).OwningColumn.Name & " IS NULL "
                End If
            Next

        End If

        strFilterString += ")"
        Return strFilterString

    End Function




    'Updates the filter status label. 
    Private Sub dgvWipSupplierQueue_DataBindingComplete(ByVal sender As Object, _
        ByVal e As DataGridViewBindingCompleteEventArgs) _
        Handles dgvWipSupplierQueue.DataBindingComplete

        If bottonGrid = "wip" AndAlso strFilterWip = "" Then
            lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters" ' & dgvWipSupplierQueue.Rows.Count
        ElseIf bottonGrid = "po" AndAlso strFilterPO = "" Then
            lblWIPPOFilter.Text = "Open Purchase Order Lines - No Filters"  ' & dgvWipSupplierQueue.Rows.Count
        End If

    End Sub


    ''' <summary>
    ''' Handles the change from sales orders to sales order lines.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub lblTopLevelDensity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblTopLevelDensity.Click

        If lblTopLevelDensity.Text = "-" Then
            lblTopLevelDensity.Text = "+"
            Me.loadSalesOrderGrouped("")
        Else
            lblTopLevelDensity.Text = "-"
            Me.loadSalesOrder(strFilterSalesOrder)
        End If
    End Sub


    Private Sub dgvSalesOrders_CellContextMenuStripNeeded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventArgs) Handles dgvSalesOrders.CellContextMenuStripNeeded

        If e.RowIndex = -1 AndAlso e.ColumnIndex > -1 Then
            dgvSalesOrders.ContextMenuStrip = cmsHeaderFilter
            strColumnHeader = dgvSalesOrders.Columns(e.ColumnIndex).HeaderText
            strColumnName = dgvSalesOrders.Columns(e.ColumnIndex).Name
            strFilterEntity = "salesOrderLinesView"
        Else
            dgvSalesOrders.ContextMenuStrip = cmsSalesOrderLines
        End If

    End Sub

    Private Sub EnterFilterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnterFilterToolStripMenuItem.Click

        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet

        'Distinct between the three different entities on the grid
        'A:Sales Order Line Filter
        If strFilterEntity = "salesOrderLinesView" Then

            'Open the dialogue to filter
            Dim dlg As New dlgCustomFilter(dgvSalesOrders, dgvSalesOrders, Me.organizationId, Me.strFilterEntity, strColumnHeader, strColumnName, strFilterSalesOrder)
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                dgvSalesOrders.EnableHeadersVisualStyles = False
                gridDisplay.formatGrid(dgvSalesOrders, "SalesOrderLinesView", Me.organizationId)

                'Setting all columns to a default value
                dgvSalesOrders.RowHeadersDefaultCellStyle.BackColor = Color.Beige
                dgvSalesOrders.Columns(strColumnName).HeaderCell.Style.BackColor = Color.CornflowerBlue
                dgvSalesOrders.Columns(strColumnName).HeaderCell.Style.ForeColor = Color.AliceBlue

                'Load the custom created filter and attach it to the temporary filter
                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)
                Try
                    strFilterSalesOrderTemp = ds.Tables(0).Rows(0).Item(0)
                    Me.loadSalesOrder(strFilterSalesOrder & " " & strFilterSalesOrderTemp)

                    'Loading the aggregate value of the sales order
                    Dim sumObject As Object
                    'The label is placed when rows have been retrieved from the server
                    If dsSalesOrder.Tables(0).Rows.Count > 0 Then
                        sumObject = dsSalesOrder.Tables(0).Compute("Sum(Extended_Price)", "") / 1000
                        Dim strValue As String = ""
                        Try
                            strValue = Format(sumObject, "C0") + "k"
                        Catch ex As Exception

                        End Try

                        'Change the label of the sales order line to a specific type
                        'If lblSalesOrderHeader.Text.Contains("No Filter") Or lblSalesOrderHeader.Text.Contains("Custom Filter") Then
                        lblSalesOrderHeader.Text = "Custom Filter Applied: " & dgvSalesOrders.Rows.Count & " / " & lngCountSalesOrderLines & " records displayed" & " - Value: " & strValue
                        'End If
                    Else
                        lblSalesOrderHeader.Text = "Nothing to display."
                        Exit Sub
                    End If
                    


                Catch ex As Exception
                    Dim err As New clsExceptionManagement
                    err.createErrorLog("toolStripMenuItemBuy", ex, 1)
                    MessageBox.Show("Please rerun the query.")
                End Try



            Else
                gridDisplay.formatGrid(dgvSalesOrders, "SalesOrderLinesView", Me.organizationId)

            End If
        End If

        If strFilterEntity = "WIPQueueView" Then

            Dim dlg As New dlgCustomFilter(dgvWipSupplierQueue, dgvWipSupplierQueue, Me.organizationId, Me.strFilterEntity, strColumnHeader, strColumnName, strFilterSalesOrder)
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                dgvWipSupplierQueue.EnableHeadersVisualStyles = False
                gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
                dgvWipSupplierQueue.Columns(strColumnName).HeaderCell.Style.BackColor = Color.CornflowerBlue
                dgvWipSupplierQueue.Columns(strColumnName).HeaderCell.Style.ForeColor = Color.AliceBlue

                'Load the custom created filter and attach it to the temporary filter
                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)
                strFilterWIPTemp = ds.Tables(0).Rows(0).Item(0)
                Me.loadWip(strFilterWip & " " & strFilterWIPTemp)

                Try
                    dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
                    gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)

                    Dim numberOfRecordsDisplayed As Integer = Me.numberOfRecordsDisplayedForWipPO(dsWIP.Tables(0))
                    'Change the label of the wip po queue to a specific line
                    'If lblWIPPOFilter.Text.Contains("Open") Or lblWIPPOFilter.Text.Contains("Custom Filter") Then
                    lblWIPPOFilter.Text = "Custom Filter Applied:  " & numberOfRecordsDisplayed & " / " & lngCountWIPDiscreteJobsOperationSequences & " records displayed"
                    ' End If


                Catch ex As Exception
                    Dim err As New clsExceptionManagement
                    err.createErrorLog("toolStripMenuItemBuy", ex, 1)
                    MessageBox.Show("Please rerun the query.")
                End Try


            Else
                gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
            End If
        End If

        If strFilterEntity = "POQueueView" Then
            Dim dlg As New dlgCustomFilter(dgvWipSupplierQueue, dgvWipSupplierQueue, Me.organizationId, Me.strFilterEntity, strColumnHeader, strColumnName, strFilterSalesOrder)
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                dgvWipSupplierQueue.EnableHeadersVisualStyles = False
                gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
                dgvWipSupplierQueue.Columns(strColumnName).HeaderCell.Style.BackColor = Color.CornflowerBlue
                dgvWipSupplierQueue.Columns(strColumnName).HeaderCell.Style.ForeColor = Color.AliceBlue

                Try
                    'Load the custom created filter and attach it to the temporary filter
                    ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)
                    strFilterPOTemp = ds.Tables(0).Rows(0).Item(0)
                    Me.loadPO(strFilterPO & " " & strFilterPOTemp)

                    Dim numberOfRecordsDisplayed As Integer = Me.numberOfRecordsDisplayedForWipPO(dsPO.Tables(0))

                    'Change the label of the wip po queue to a specific line
                    'If lblWIPPOFilter.Text.Contains("Open") Or lblWIPPOFilter.Text.Contains("Custom Filter") Then
                    lblWIPPOFilter.Text = "Custom Filter Applied:  " & numberOfRecordsDisplayed & " / " & lngCountPurchaseOrderLines & " records displayed"
                    'End If

                Catch ex As Exception
                    Dim err As New clsExceptionManagement
                    err.createErrorLog("toolStripMenuItemBuy", ex, 1)
                End Try




            Else
                gridDisplay.formatGrid(dgvWipSupplierQueue, poBottonGrid)
            End If
        End If

        If strFilterEntity = "MRPShortagesView" Then
            Dim dlg As New dlgCustomFilter(dgvShortages, dgvShortages, Me.organizationId, Me.strFilterEntity, strColumnHeader, strColumnName, strFilterSalesOrder)
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                dgvShortages.EnableHeadersVisualStyles = False
                gridDisplay.formatGrid(dgvShortages, "MRPShortagesView")
                dgvShortages.Columns(strColumnName).HeaderCell.Style.BackColor = Color.CornflowerBlue
                dgvShortages.Columns(strColumnName).HeaderCell.Style.ForeColor = Color.AliceBlue

                'Load the custom created filter and attach it to the temporary filter
                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)
                strFilterMRPShortagesTemp = ds.Tables(0).Rows(0).Item(0)
                Me.populateMRPShortages(Me.lnDemandId, Me.organizationId, strFilterMRPShortagesTemp)

            Else
                gridDisplay.formatGrid(dgvShortages, "MRPShortagesView")
            End If
        End If

        'make all columns attached from the custom filter blue
        ds = db.SecureQueryParams("SELECT DISTINCT(currentColumn) FROM whUserFilterLinesView WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)

        If strFilterEntity = "salesOrderLinesView" Then
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                dgvSalesOrders.Columns(ds.Tables(0).Rows(i).Item(0).ToString.Trim).HeaderCell.Style.BackColor = Color.CornflowerBlue
                dgvSalesOrders.Columns(ds.Tables(0).Rows(i).Item(0).ToString.Trim).HeaderCell.Style.ForeColor = Color.AliceBlue
            Next

        End If

        If strFilterEntity = "MRPShortagesView" Then
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try
                    dgvShortages.Columns(ds.Tables(0).Rows(i).Item(0).ToString.Trim).HeaderCell.Style.BackColor = Color.CornflowerBlue
                    dgvShortages.Columns(ds.Tables(0).Rows(i).Item(0).ToString.Trim).HeaderCell.Style.ForeColor = Color.AliceBlue
                Catch ex As Exception

                End Try
            Next
        End If

        If strFilterEntity = "POQueueView" Or strFilterEntity = "WIPQueueView" Then
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                dgvWipSupplierQueue.Columns(ds.Tables(0).Rows(i).Item(0).ToString.Trim).HeaderCell.Style.BackColor = Color.CornflowerBlue
                dgvWipSupplierQueue.Columns(ds.Tables(0).Rows(i).Item(0).ToString.Trim).HeaderCell.Style.ForeColor = Color.AliceBlue
            Next

        End If

    End Sub

    Private Function numberOfRecordsDisplayedForWipPO(ByRef dt As DataTable) As Integer
        Dim numRecords As Integer = 0
        'Find out if we have the column
        If dt.Columns.Contains("PONoLine") Then
            numRecords = dsPO.Tables(0).Compute("COUNT(PONoLine)", "PONoLine IS NOT NULL")
        ElseIf dt.Columns.Contains("JobNoOperationSeq") Then
            numRecords = dsWIP.Tables(0).Compute("COUNT(JobNoOperationSeq)", "JobNoOperationSeq IS NOT NULL")
        End If

        Return numRecords
    End Function

    Private Sub ClearFilterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearFilterToolStripMenuItem.Click



        'Procedure to remove the details from the temporary filter 
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT filterId FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)

        If ds.Tables(0).Rows.Count > 0 Then
            db.SecureNonQueryParams("DELETE FROM warehouseUserFilterDetail WHERE filterId = @1 AND currentColumn = @2", ds.Tables(0).Rows(0).Item(0), strColumnName)

            If Me.strFilterEntity = "salesOrderLinesView" Then
                dgvSalesOrders.Columns(strColumnName).HeaderCell.Style.BackColor = Color.Beige
                dgvSalesOrders.Columns(strColumnName).HeaderCell.Style.ForeColor = Color.Black

                Dim dlg As New dlgCustomFilter(dgvSalesOrders, dgvSalesOrders, Me.organizationId, Me.strFilterEntity, strColumnHeader, strColumnName, strFilterSalesOrder)
                dlg.saveTranslatedFilterPhrase(ds.Tables(0).Rows(0).Item(0))

                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)
                strFilterSalesOrderTemp = ds.Tables(0).Rows(0).Item(0)

                Me.loadSalesOrder(strFilterSalesOrder & " " & strFilterSalesOrderTemp)

                If strFilterSalesOrderTemp <> "" Or strFilterSalesOrder <> "" Then
                    'Loading the aggregate value of the sales order
                    Dim sumObject As Object = ""
                    Try
                        sumObject = dsSalesOrder.Tables(0).Compute("Sum(Extended_Price)", "") / 1000
                    Catch ex As Exception
                        Dim err As New clsExceptionManagement
                        err.createErrorLog("toolStripMenuItemBuy", ex, 1)
                    End Try

                    Dim strValue As String = ""
                    Try
                        strValue = Format(sumObject, "C0") + "k"
                    Catch ex As Exception

                    End Try

                    lblSalesOrderHeader.Text = "Custom Filter Applied:  " & dgvSalesOrders.Rows.Count & " / " & lngCountSalesOrderLines & " records displayed" & " - Value: " & strValue
                Else
                    lblSalesOrderHeader.Text = "Open Sales Orders - No Filters"
                End If

            End If
            'Do this for the other entities

            If Me.strFilterEntity = "MRPShortagesView" Then
                dgvShortages.Columns(strColumnName).HeaderCell.Style.BackColor = Color.Beige
                dgvShortages.Columns(strColumnName).HeaderCell.Style.ForeColor = Color.Black

                Dim dlg As New dlgCustomFilter(dgvShortages, dgvShortages, Me.organizationId, Me.strFilterEntity, strColumnHeader, strColumnName, strFilterMRPShortagesTemp)
                dlg.saveTranslatedFilterPhrase(ds.Tables(0).Rows(0).Item(0))

                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)
                strFilterMRPShortagesTemp = ds.Tables(0).Rows(0).Item(0)

                Me.populateMRPShortages(Me.lnDemandId, Me.organizationId, Me.strFilterMRPShortagesTemp)

            End If

            If Me.strFilterEntity = "POQueueView" Then
                dgvWipSupplierQueue.Columns(strColumnName).HeaderCell.Style.BackColor = Color.Beige
                dgvWipSupplierQueue.Columns(strColumnName).HeaderCell.Style.ForeColor = Color.Black

                Dim dlg As New dlgCustomFilter(dgvWipSupplierQueue, dgvWipSupplierQueue, Me.organizationId, Me.strFilterEntity, strColumnHeader, strColumnName, strFilterPO)
                dlg.saveTranslatedFilterPhrase(ds.Tables(0).Rows(0).Item(0))

                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)
                strFilterPOTemp = ds.Tables(0).Rows(0).Item(0)

                Me.loadPO(strFilterPO & strFilterPOTemp)
                Dim numberOfRecordsDisplayed As Integer = Me.numberOfRecordsDisplayedForWipPO(dsWIP.Tables(0))
                'Setting the filter lable value 
                If strFilterPOTemp <> "" Or strFilterPO <> "" Then
                    lblWIPPOFilter.Text = "Custom Filter Applied:  " & numberOfRecordsDisplayed & " / " & lngCountPurchaseOrderLines & " records displayed"
                Else
                    lblWIPPOFilter.Text = "Open Purchase Order Lines - No Filters"
                End If
            End If

            If Me.strFilterEntity = "WIPQueueView" Then
                dgvWipSupplierQueue.Columns(strColumnName).HeaderCell.Style.BackColor = Color.Beige
                dgvWipSupplierQueue.Columns(strColumnName).HeaderCell.Style.ForeColor = Color.Black

                Dim dlg As New dlgCustomFilter(dgvWipSupplierQueue, dgvWipSupplierQueue, Me.organizationId, Me.strFilterEntity, strColumnHeader, strColumnName, strFilterWip)
                dlg.saveTranslatedFilterPhrase(ds.Tables(0).Rows(0).Item(0))

                ds = db.SecureQueryParams("SELECT filterString FROM warehouseUserFilters WHERE entity = @1 AND userName = @2 AND filterName = 'TEMP'", Me.strFilterEntity, Environment.UserName)
                strFilterWIPTemp = ds.Tables(0).Rows(0).Item(0)

                Me.loadWip(strFilterWip & strFilterWIPTemp)
                'Has to reload to the grid, because this is not done at the subroutine.
                Dim numberOfRecordsDisplayed As Integer = Me.numberOfRecordsDisplayedForWipPO(dsWIP.Tables(0))

                'Setting the value of the lable which is displaying the record count
                If strFilterWIPTemp <> "" Then

                    lblWIPPOFilter.Text = "Custom Filter Applied: " & numberOfRecordsDisplayed & " / " & lngCountWIPDiscreteJobsOperationSequences & " records displayed"
                ElseIf strFilterWip <> "" Then

                    lblWIPPOFilter.Text = "Custom Filter Applied: " & numberOfRecordsDisplayed & " / " & lngCountWIPDiscreteJobsOperationSequences & " records displayed"
                Else
                    lblWIPPOFilter.Text = "Open WIP Operation Sequences - No Filters"
                End If
                Try
                    dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
                    gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)

                Catch ex As Exception
                    Dim err As New clsExceptionManagement
                    err.createErrorLog("toolStripMenuItemBuy", ex, 1)
                End Try


            End If

        End If

    End Sub

    ''' <summary>
    ''' Return the item information like
    ''' </summary>
    ''' <param name="organizationId">The organization id we want to look for.</param>
    ''' <param name="inventoryItemId">The inventory item id we want to look for.</param>
    ''' <remarks>Returns a seperated string.</remarks>
    Private Function getItemInformation(ByVal organizationId As Integer, ByVal inventoryItemId As Integer) As String
        Dim ds As New DataSet
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim strReturn = ""

        ''get the item information
        'ds = db.SecureQueryParams("SELECT itemInformation FROM active_items WHERE organization_id = @1 AND inventory_item_id = @2", organizationId, inventoryItemId)
        'If ds.Tables(0).Rows.Count > 0 Then
        '    'Create the string which can be displayed in the tooltip menu

        '    If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item(0)) Then
        '        Dim strTemp As String
        '        strTemp = ds.Tables(0).Rows(0).Item(0)
        '        Dim chArray() As Char
        '        chArray = strTemp.ToCharArray
        '        For i As Integer = 0 To strTemp.Length - 1
        '            If chArray(i) = "|" Then
        '                strReturn += vbCrLf
        '            Else
        '                strReturn += chArray(i)
        '            End If
        '        Next
        '    End If
        'End If


        Return strReturn
    End Function

    'Series of events to capture the moment when the workbench can be refreshed
    Private Sub frmWorkbenchProjects_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove

        lastMouseMoveEvent = Now()
    End Sub


    'Mouse move event on panel1
    Private Sub sc1_Panel1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles sc1.Panel1.MouseMove

        lastMouseMoveEvent = Now()
    End Sub


    'Mouse move event on sales orders
    Private Sub dgvSalesOrders_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvSalesOrders.MouseMove

        lastMouseMoveEvent = Now()
    End Sub


    'Mouse move event on shortages grid view
    Private Sub dgvShortages_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvShortages.MouseMove

        lastMouseMoveEvent = Now()
    End Sub


    'Mouse move event on wip supplier queue
    Private Sub dgvWipSupplierQueue_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvWipSupplierQueue.MouseMove
        lastMouseMoveEvent = Now()
    End Sub


    'Mouse move event on further details of the job
    Private Sub dgvFurtherDetails_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvFurtherDetails.MouseMove
        lastMouseMoveEvent = Now()
    End Sub

    Private Sub SaveCurrentTemporarySelectionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveCurrentTemporarySelectionToolStripMenuItem.Click

        If strFilterEntity <> "" Then
            Dim dlgSave As New dlgSaveCurrentSelection(Me.strFilterEntity)
            dlgSave.ShowDialog()

            If dlgSave.DialogResult = Windows.Forms.DialogResult.OK Then
                Me.reloadUserViews()
                'Depending on the entity that was saved reset the data in the grid and start again to dive.
                lblSalesOrderHeader.Text = "Open Sales Orders - No Filters"

                If strFilterEntity = "salesOrderLinesView" Then
                    dgvSalesOrders.EnableHeadersVisualStyles = True
                    strFilterSalesOrderTemp = ""
                    Me.loadSalesOrder("")

                ElseIf strFilterEntity = "MRPShortages" Then
                    dgvShortages.EnableHeadersVisualStyles = True
                    strFilterMRPShortagesTemp = ""
                    Me.populateMRPShortages(Me.lnDemandId, Me.organizationId, "")

                ElseIf strFilterEntity = "POQueueView" Then
                    dgvWipSupplierQueue.EnableHeadersVisualStyles = True
                    strFilterPOTemp = ""
                    Me.loadPO("")

                ElseIf strFilterEntity = "WIPQueueView" Then
                    'The only one where we have to reload also the data grid view
                    dgvWipSupplierQueue.EnableHeadersVisualStyles = True
                    strFilterWip = ""
                    Me.loadWip("")
                    dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
                    gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)

                End If

            End If

        Else
            MessageBox.Show("Please Create A Selection First")
        End If

    End Sub

    Private Sub dgvShortages_CellContextMenuStripNeeded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventArgs) Handles dgvShortages.CellContextMenuStripNeeded

        'Switch the right click menues to the current demand
        If e.RowIndex = -1 AndAlso e.ColumnIndex > -1 AndAlso strMiddleGrid = "MRPShortagesView" Then
            'Deactivated until perfect implementation can be done.
            'Need to distinct between shortages and supply demand view for the fiters 
            dgvShortages.ContextMenuStrip = cmsHeaderFilter
            strColumnHeader = dgvShortages.Columns(e.ColumnIndex).HeaderText
            strColumnName = dgvShortages.Columns(e.ColumnIndex).Name
            strFilterEntity = "MRPShortagesView"
        Else
            dgvShortages.ContextMenuStrip = cmdGridOperations
        End If

        If strMiddleGrid <> "MRPShortagesView" Then
            dgvShortages.ContextMenuStrip = cmdGridOperations
        End If
    End Sub

    Private Sub dgvWipSupplierQueue_CellContextMenuStripNeeded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventArgs) Handles dgvWipSupplierQueue.CellContextMenuStripNeeded

        If e.RowIndex = -1 AndAlso e.ColumnIndex > -1 Then
            dgvWipSupplierQueue.ContextMenuStrip = cmsHeaderFilter
            strColumnHeader = dgvWipSupplierQueue.Columns(e.ColumnIndex).HeaderText
            strColumnName = dgvWipSupplierQueue.Columns(e.ColumnIndex).Name

            If bottonGrid = "wip" Then
                strFilterEntity = "WIPQueueView"
            Else
                strFilterEntity = "POQueueView"
            End If

        Else
            If bottonGrid = "wip" Then
                dgvWipSupplierQueue.ContextMenuStrip = cmsBottonWip
            Else
                dgvWipSupplierQueue.ContextMenuStrip = cmsBotton
            End If

        End If
    End Sub


    ''' <summary>
    ''' Saves the changes to the backend when a user changes the width.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub dgvSalesOrders_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvSalesOrders.ColumnWidthChanged
        'Distinct when the user really changes the column width or the normal reload process from the backend does so.
        If Me.gridDisplay.reloading Then
        Else
            gridDisplay.saveChangesOfSingleColumn(e.Column.Name, "salesOrderLinesView", e.Column.Width)
        End If
    End Sub

    Private Sub dgvShortages_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvShortages.ColumnWidthChanged
        'Distinct when the user really changes the column width or the normal reload process from the backend does so.
        If Me.gridDisplay.reloading Then

        Else
            If strMiddleGrid = "MRPShortagesView" Then
                gridDisplay.saveChangesOfSingleColumn(e.Column.Name, "MRPShortagesView", e.Column.Width)
            Else
                gridDisplay.saveChangesOfSingleColumn(e.Column.Name, supplyDemandMiddleGrid, e.Column.Width)
            End If

        End If
    End Sub

    Private Sub dgvWipSupplierQueue_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvWipSupplierQueue.ColumnWidthChanged
        'Distinct when the user really changes the column width or the normal reload process from the backend does so.
        If Me.gridDisplay.reloading Then

        Else
            If Me.bottonGrid = "wip" Then
                gridDisplay.saveChangesOfSingleColumn(e.Column.Name, wipBottonGrid, e.Column.Width)
            Else
                gridDisplay.saveChangesOfSingleColumn(e.Column.Name, poBottonGrid, e.Column.Width)
            End If

        End If
    End Sub


    Private Sub dgvFurtherDetails_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvFurtherDetails.ColumnWidthChanged
        'Distinct when the user really changes the column width or the normal reload process from the backend does so.
        If Me.gridDisplay.reloading Then

        Else
            gridDisplay.saveChangesOfSingleColumn(e.Column.Name, "jobFurtherDetails", e.Column.Width)

        End If


    End Sub


    Private Sub ResetColumnWidthToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetColumnWidthToolStripMenuItem.Click
        Dim dlg As New dlgResetAllColumnWidth
        dlg.ShowDialog()

        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Me.populateGrids()
            dgvSalesOrders.EnableHeadersVisualStyles = True
            dgvShortages.EnableHeadersVisualStyles = True
            dgvWipSupplierQueue.EnableHeadersVisualStyles = True

        End If
    End Sub

    Private Sub OrderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrderToolStripMenuItem.Click
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet

        'Add new columns to the collection of the user.
        Dim operation As New clsSetupOperations()
        ds = db.Query("SELECT user_name FROM warehouse_users")
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            operation.import_grid_column_for_sizing(ds.Tables(0).Rows(i).Item(0))
        Next

    End Sub

    Private Sub lblDemandView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDemandView.Click
        Me.calculateRunningNumberForSupplyDemand(dsMRPShortages, dgvShortages, False)
        lblDemandView.Visible = False

    End Sub


    Private Sub YourActualInvolvementToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YourActualInvolvementToolStripMenuItem.Click
        Dim filter As New clsFiltering
        Me.loadSalesOrder(filter.individualInvolvementString("salesOrderLinesView"))

        If bottonGrid = "wip" Then
            Me.loadWip(filter.individualInvolvementString("WIPQueueView"))
            dgvWipSupplierQueue.DataSource = dsWIP.Tables(0)
            gridDisplay.formatGrid(dgvWipSupplierQueue, wipBottonGrid)
        Else
            Me.loadPO(filter.individualInvolvementString("POQueueView"))
        End If

    End Sub

    'user wants to create nafta certifications and backup documentation
    Private Sub NAFTACheckToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NAFTACheckToolStripMenuItem.Click
        Try
            Dim strViewToSelect As String = ""
            If planningType = "ASCP" Then
                strViewToSelect = "MSCsalesOrderLinesView"
            Else
                strViewToSelect = "salesOrderLinesView"
            End If

            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim ds As New DataSet
            Dim dsCosts As New DataSet

            'remember the first sales order so that if multiple sales orders were picked, it can raise an error
            Dim strOrder As String = dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(0).RowIndex).Cells("ORDER_NO").Value
            Dim strHeaderId As String = dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(0).RowIndex).Cells("HEADER_ID").Value.ToString

            'if no row is selected, look for a selected cell
            If dgvSalesOrders.SelectedRows.Count = 0 Then dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(0).RowIndex).Selected = True

            'make a list of sales order lines that are selected by the user
            Dim strFilterStatement2 As String = "("
            For i As Integer = 0 To dgvSalesOrders.SelectedRows.Count - 1
                strFilterStatement2 += "'" & dgvSalesOrders.SelectedRows(i).Cells("OrderNoLine").Value & "'"
                If i < dgvSalesOrders.SelectedRows.Count - 1 Then
                    strFilterStatement2 += ","
                End If
                If strOrder <> dgvSalesOrders.SelectedRows(i).Cells("ORDER_NO").Value Then
                    MessageBox.Show("Nafta check works for one order at a time.")
                    Exit Sub
                End If
            Next
            strFilterStatement2 += ")"


            'combine the	NAFTA data with the selected sales order lines
            db.NonQuery("DELETE FROM tblWork2")
            db.SecureNonQueryParams("INSERT INTO tblWork2 (text1, number1, number2, number3, number4, text2, text3, number5) SELECT so.item_no, so.line_no, so.project_id, so.unit_selling_price, ai.planning_make_buy_code, so.description, so.order_no, so.inventory_item_id  " & _
              "FROM " & strViewToSelect & " so INNER JOIN saveActiveItems ai " & _
              "ON so.organization_id = ai.organization_id AND so.inventory_item_id = ai.inventory_item_id WHERE so.organization_id = @1 AND so.OrderNoLine IN " & strFilterStatement2 & " ORDER BY so.line_no", Me.organizationId)

            ds = db.Query("SELECT a.text1 as itemNo,a.number2 as project_id, a.number4 as planning_make_buy_code, b.[vendorNo],a.number3 as unit_selling_price, a.number1 AS line_no, text2 as description, number5 AS inventory_item_id" & _
             ",b.[vendorName]          " & _
             ",b.[vendorItem]          " & _
             ",b.[description]         " & _
             ",b.[drawing]             " & _
             ",b.[productCode]         " & _
             ",b.[planner]             " & _
             ",b.[tariff]              " & _
             ",b.[prefCriterion]       " & _
             ",b.[producer]            " & _
             ",b.[netCost]             " & _
             ",b.[countryOfOrigin]     " & _
             ",b.[comments]            " & _
             ",b.[NAFTAPart]  FROM tblWork2 a LEFT OUTER JOIN nafta b ON a.text1 = b.itemNo")

            db.NonQuery("DELETE FROM tblWork3")
            'Create detailed NAFTA reports
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim calc As New clsMiscelleneousCalculations
                Try
                    Dim blnNafta As String
                    'Run the NAFTA calculation when it is a "make" part
                    If ds.Tables(0).Rows(i).Item("planning_make_buy_code") = 1 Then   '1 equals manufacturing part!
                        Dim projectID As Long
                        If DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("project_id")) Then
                            projectID = 0
                        Else
                            projectID = ds.Tables(0).Rows(i).Item("project_id")
                        End If
                        blnNafta = calc.CalculateNAFTA(Me.organizationId, strOrder, ds.Tables(0).Rows(i).Item("line_no"), ds.Tables(0).Rows(i).Item("itemNo"), projectID, ds.Tables(0).Rows(i).Item("unit_selling_price"), i, ds)
                        'If blnNafta Then
                        '	If strFilterStatement.Count > 2 Then
                        '		strFilterStatement += ","
                        '	End If
                        '	strFilterStatement += "'" & calc.currentPartNumberForNafta & "'"
                        'End If
                    Else
                        'run the detailed report for purchased part. no calculation necessary - we just take the value from the NAFTA table
                        db.NonQuery("DELETE FROM tblWork2")   'first empty the work table
                        db.SecureNonQueryParams("INSERT INTO [tblWork2] ([Text1], [Text2], [Number3]) VALUES (@1, @2, @3)", ds.Tables(0).Rows(i).Item("itemNo"), ds.Tables(0).Rows(i).Item("description"), 2)
                        'append all the NAFTA info to the summarized bom  duplicated in the nafta function. consolidate! ***
                        db.NonQuery("UPDATE w SET text6 = n.tariff, text7 = n.prefCriterion, text8 = n.netCost, text9 = n.producer, text10 = n.countryOfOrigin, text11 = n.vendorNo, boolean1 = n.NAFTAPart " & _
                         "FROM dbo.tblWork2 w INNER JOIN dbo.NAFTA n ON n.itemNo = w.text1")
                        dsCosts = db.SecureQueryParams("SELECT item_cost, pending_cost FROM items WHERE inventory_item_id = @1 AND organization_id = @2", ds.Tables(0).Rows(i).Item("inventory_item_id"), Me.organizationId)
                        'Gaining NAFTA percentage from the value
                        Dim dblNaftaValue As Double = 0
                        If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("NAFTAPart")) Then
                            If ds.Tables(0).Rows(i).Item("NAFTAPart") Then dblNaftaValue = 1
                        Else
                            ds.Tables(0).Rows(i).Item("NAFTAPart") = False
                        End If
                        Dim dblCosts As Double = 0
                        If Not DBNull.Value.Equals(dsCosts.Tables(0).Rows(0).Item("item_cost")) Then dblCosts = dsCosts.Tables(0).Rows(0).Item("item_cost")
                        Dim frm As New frmReportViewer("", strOrder, ds.Tables(0).Rows(i).Item("line_no"), ds.Tables(0).Rows(i).Item("itemNo"), ds.Tables(0).Rows(i).Item("unit_selling_price"), dblCosts, dblNaftaValue, ds.Tables(0).Rows(i).Item("NAFTAPart"))
                        frm.Show()
                    End If

                Catch ex As Exception
                    Dim err As New clsExceptionManagement
                    err.createErrorLog("calculateNafta", ex, 1)
                    MessageBox.Show(ex.Message)
                End Try

            Next

            'display the data so the user can change it. Save it to nafta base table
            Dim dlg2 As New dlgOneGridConfirm(ds, True)
            dlg2.ShowDialog()

            'If the user confirms the results, create the certificate.
            If dlg2.DialogResult = Windows.Forms.DialogResult.OK Then
                'make a new list of sales order lines that passed NAFTA requirements. 
                db.NonQuery("DELETE tblWork2")
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("NAFTAPart")) Then
                        If ds.Tables(0).Rows(i).Item("NAFTAPart") = True Then
                            'insert the required values into the second work table - let it requery from the nafta report viewer!
                            db.SecureNonQueryParams("INSERT INTO tblWork2 (text1,text2, text3, text4, text5, text6, text7, text8, text9, text10, text11, text12, text13, text14, text15) VALUES (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15)", _
                                    ds.Tables(0).Rows(i).Item("vendorName"), ds.Tables(0).Rows(i).Item("vendorItem"), ds.Tables(0).Rows(i).Item("description"), ds.Tables(0).Rows(i).Item("drawing") _
                                    , ds.Tables(0).Rows(i).Item("productCode"), ds.Tables(0).Rows(i).Item("planner"), ds.Tables(0).Rows(i).Item("tariff"), ds.Tables(0).Rows(i).Item("prefCriterion") _
                                    , ds.Tables(0).Rows(i).Item("producer"), ds.Tables(0).Rows(i).Item("netCost"), ds.Tables(0).Rows(i).Item("countryOfOrigin"), _
                                    ds.Tables(0).Rows(i).Item("comments"), ds.Tables(0).Rows(i).Item("NAFTAPart"), ds.Tables(0).Rows(i).Item("itemNo"), ds.Tables(0).Rows(i).Item("description"))
                            '*** Update and save to the NAFTA table
                            'Dim lngRecordHits As Long
                            'lngRecordHits = db.SecureNonQueryParams("", ds.Tables(0).Rows(i).Item("vendorName"))
                        End If
                    End If
                Next

                'If we have any certs to print, make the reports (NAFTA report -> Certificate
                Dim ds2 As DataSet = db.Query("SELECT * FROM tblWork2 ")
                If ds2.Tables(0).Rows.Count > 0 Then
                    Dim frm As New frmReportViewer("ProjectManagerWorkbench\Reporting\repNaftaSummary.rdlc", strOrder, strHeaderId, Me.organizationId, ds)
                    frm.Text = "Report - NORTH AMERICAN FREE TRADE AGREEMENT"
                    frm.Show()
                Else
                    MessageBox.Show("No Items to Create a Certificate For.")
                End If
            End If



        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("calculateNafta", ex, 1)
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub CloseAllReportViewerWindowsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseAllReportViewerWindowsToolStripMenuItem.Click


        For i As Integer = 0 To Application.OpenForms.Count - 1
            Try
                If Application.OpenForms(1).Text.Contains("Report") Then
                    Application.OpenForms(1).Close()

                End If
            Catch ex As Exception
                Dim err As New clsExceptionManagement
                err.createErrorLog("calculateNafta", ex, 1)
            End Try

        Next
    End Sub

    Private Sub ChangeRoleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeRoleToolStripMenuItem.Click
        Dim dlg As New dlgSwitchRole
        dlg.ShowDialog()

        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            Dim ds As New DataSet
            ds = db.SecureQueryParams("SELECT warehouse_users.lastRoleSelected, whUserRoles.roleName FROM warehouse_users INNER JOIN whUserRoles ON whUserRoles.roleId = warehouse_users.lastRoleSelected AND warehouse_users.user_name = @1", Environment.UserName)

            Try
                Dim userRights As New clsUserControl(ds.Tables(0).Rows(0).Item("lastRoleSelected"))
                userRights.applyUserRightsOnForm(Me)
                lblCurrentRole.Text = "Login: " & ds.Tables(0).Rows(0).Item("roleName")
                lblCurrentRole.Visible = True

            Catch ex As Exception
                Dim err As New clsExceptionManagement
                err.createErrorLog("calculateNafta", ex, 1)
            End Try



        End If

    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        Me.copyGridOrCell(dgvWipSupplierQueue)
    End Sub

    Private Sub frmWorkbenchProjects_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            Dim createLogout As New clsSetupOperations
            createLogout.insertLogoutStamp()
            'createLogout.UpdateOutOfOfficeInfo()
            Me.deleteTemporaryFilters(True, True, True, True)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub AddToWatchGroupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If dgvSalesOrders.SelectedRows.Count = 0 Then
            ' dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells.)
        End If
    End Sub

    Private bFilterSeries As Boolean = False
    Private strColumnNames(0) As String

    Private Sub dgvWipSupplierQueue_Sorted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvWipSupplierQueue.Sorted

        If Me.bottonGrid = "wip" AndAlso bFilterSeries Then
            Dim strSortString As String = " "
            Dim lngSize As Long = strColumnNames.Length - 1

            'Redim of the current array
            ReDim Preserve strColumnNames(lngSize)
            strColumnNames(lngSize) = dgvWipSupplierQueue.SortedColumn.Name

            'Creates the following statement:  A, B, C
            For i As Integer = 0 To strColumnNames.Length - 1
                strSortString += strColumnNames(i)
                If i < strColumnNames.Length - 1 Then
                    strSortString += " , "
                End If
            Next
            'Requery and bind the 
            '*** The interface of the datagridview seems to be quite complex, so query the backend
            Me.loadWip(strFilterWip & strFilterWIPTemp, strSortString)

            'The two events just occure, when the element is focused

        End If

    End Sub

    Private Sub dgvWipSupplierQueue_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvWipSupplierQueue.KeyUp
        ReDim strColumnNames(0)
        strColumnNames(0) = ""
        bFilterSeries = False
        Me.eraseSortingArray(e)
    End Sub

    Private Sub dgvWipSupplierQueue_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvWipSupplierQueue.KeyDown
        bFilterSeries = True

        'Fill an array with the column sorted when the control key is pressed
        
        'Display the comment form
        Try
            If e.KeyValue <> 17 And e.KeyValue <> 16 And e.KeyValue <> 40 And e.KeyValue <> 38 Then 'Allow Ctrl, Shfit, Up Arrow and Down Arrow Keys to perform multiple selection
                If dgvSalesOrders.Columns.Count > 0 Then
                    If dgvWipSupplierQueue.Columns(dgvWipSupplierQueue.CurrentCell.ColumnIndex).Name = "lastComment" Then
                        If Not dgvWipSupplierQueue.CurrentCell.RowIndex = -1 Then
                            Dim listSelectedCells As New List(Of clsSelectedCells)
                            For index As Integer = 0 To dgvWipSupplierQueue.SelectedCells.Count - 1
                                Dim selectedItem As New clsSelectedCells
                                If dgvWipSupplierQueue.Columns(dgvWipSupplierQueue.SelectedCells(index).ColumnIndex).Name = "lastComment" Then
                                    If Me.bottonGrid = "po" Then
                                        If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("PO_HEADER_ID").Value) Then
                                            selectedItem.header_id = dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("PO_HEADER_ID").Value
                                            selectedItem.headerName = dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("PO_NO").Value
                                            selectedItem.line_id = dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("LINE_NO").Value
                                            selectedItem.shipment_no = dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("SHIPMENT_NO").Value
                                            selectedItem.entity_name = "POQueueView"
                                            selectedItem.dgv = dgvWipSupplierQueue
                                            selectedItem.row = dgvWipSupplierQueue.SelectedCells(index).RowIndex()
                                            selectedItem.organizationId = Me.organizationId
                                            If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("RELEASE_NO").Value) Then
                                                selectedItem.releaseNo = dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("RELEASE_NO").Value
                                            Else
                                                selectedItem.releaseNo = 0
                                            End If
                                        End If
                                    Else
                                        If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("WIP_ENTITY_ID").Value) Then
                                            selectedItem.header_id = dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("WIP_ENTITY_ID").Value
                                            selectedItem.headerName = dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("JOB_NO").Value
                                            selectedItem.line_id = dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.SelectedCells(index).RowIndex).Cells("OPERATION_SEQ_NO").Value
                                            selectedItem.shipment_no = 0
                                            selectedItem.entity_name = "WIPQueueView"
                                            selectedItem.dgv = dgvWipSupplierQueue
                                            selectedItem.row = dgvWipSupplierQueue.SelectedCells(index).RowIndex()
                                            selectedItem.organizationId = Me.organizationId
                                            selectedItem.releaseNo = 0
                                        End If
                                    End If
                                    listSelectedCells.Add(selectedItem)
                                End If
                            Next

                            If listSelectedCells.Count > 1 Then
                                'For multiple lines
                                Dim dlg As New dlgComments(listSelectedCells)
                                dlg.ShowDialog()
                                If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                                    Me.loadCurrentNotifications()
                                End If
                            Else
                                'For single line - this is existing functionality taken from CellContentClick
                                If Me.bottonGrid = "po" Then
                                    If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("PO_HEADER_ID").Value) Then
                                        Dim dlg As dlgComments
                                        If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("RELEASE_NO").Value) Then
                                            dlg = New dlgComments(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("PO_HEADER_ID").Value, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("PO_NO").Value, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("LINE_NO").Value, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("SHIPMENT_NO").Value, "POQueueView", dgvWipSupplierQueue, dgvWipSupplierQueue.CurrentCell.RowIndex, Me.organizationId, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("RELEASE_NO").Value.ToString)
                                        Else
                                            dlg = New dlgComments(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("PO_HEADER_ID").Value, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("PO_NO").Value, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("LINE_NO").Value, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("SHIPMENT_NO").Value, "POQueueView", dgvWipSupplierQueue, dgvWipSupplierQueue.CurrentCell.RowIndex, Me.organizationId, 0)
                                        End If

                                        dlg.ShowDialog()
                                        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                                            Me.loadCurrentNotifications()
                                        End If
                                    End If
                                Else
                                    If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("WIP_ENTITY_ID").Value) Then
                                        Dim dlg As New dlgComments(dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("WIP_ENTITY_ID").Value, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("JOB_NO").Value, dgvWipSupplierQueue.Rows(dgvWipSupplierQueue.CurrentCell.RowIndex).Cells("OPERATION_SEQ_NO").Value, 0, "WIPQueueView", dgvWipSupplierQueue, dgvWipSupplierQueue.CurrentCell.RowIndex, Me.organizationId)
                                        dlg.ShowDialog()
                                        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                                            Me.loadCurrentNotifications()
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                IsCtrlKeyPressed = True
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("wipSupplierQueueKeyDown", ex, 1)
        End Try
    End Sub

    Private lngCurrentItem As Integer
    ''' <summary>
    ''' Events to handle the mouse move event.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub dgvSalesOrders_CellMouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvSalesOrders.CellMouseMove
        'Implements the custom tool tip for items
        'The procedure saves which item is currently displayed to avoid requerying with little moves
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "ITEM_NO" Then
                If dgvSalesOrders.Rows(e.RowIndex).Cells("inventory_item_id").Value <> lngCurrentItem Then
                    lngCurrentItem = dgvSalesOrders.Rows(e.RowIndex).Cells("inventory_item_id").Value
                    Dim strDisplay As String = Me.getItemInformation(Me.organizationId, dgvSalesOrders.Rows(e.RowIndex).Cells("inventory_item_id").Value)
                    Dim point As Point = PointToClient(MousePosition)
                    Me.ttHoverActions.RemoveAll()
                    ttHoverActions.ShowAlways = True
                    Me.ttHoverActions.Show(strDisplay, Me, point)
                    'Hide the normal tooltip
                    dgvSalesOrders.Rows(e.RowIndex).Cells("item_no").ToolTipText = ""
                End If
            Else
                Dim point As Point = PointToClient(MousePosition)

                If (point.X - oldpoint.X) > 2 Or (point.X - oldpoint.X) < -2 Or (point.Y - oldpoint.Y) > 2 Or (point.Y - oldpoint.Y) < -2 Then
                    oldpoint = point
                    point.X = point.X + 4
                    point.Y = point.Y + 5
                    Me.ttHoverActions.Show(dgvSalesOrders(e.ColumnIndex, e.RowIndex).Value.ToString, Me, point)
                End If

            End If
            'Implement other mouse symbol for clickable columns

        End If

        'Triggers the refresh process
        'lastMouseMoveEvent = Now()
    End Sub

    Private Sub dgvSalesOrders_CellMouseLeave(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSalesOrders.CellMouseLeave
        Me.ttHoverActions.Hide(Me)

        If e.ColumnIndex > -1 And e.RowIndex > -1 Then
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "ITEM_NO" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "JOB_PO" _
           Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "ORDER_NO" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OrderNoLine" _
           Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP1" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP2" _
           Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP3" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP4" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "lastComment" Then
                dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Empty
            End If
        End If
    End Sub

    Private Sub getLowerLevelInformation()
        Dim ds As New DataSet
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

    End Sub

    ''' <summary>
    ''' Procedure that opens the different user manuals.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ManualToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManualToolStripMenuItem.Click
        Try
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            'Distinct between the languages
            If Me.organizationLanguage = "DE" Then
                Dim ds As DataSet = db.SecureQueryParams("SELECT [organizationID] ,[attachementID] ,[fileName],[attachmentName], " & _
                                       " [attachmentPurpose] ,[creationDate] ,[createdBy],[attachment] FROM [warehouseAttachements] WHERE attachmentName = 'Manual DE' ")

                Dim strPath As String = Application.StartupPath & "\" & ds.Tables(0).Rows(0).Item("fileName")
                Dim fileData As Byte() = DirectCast(ds.Tables(0).Rows(0).Item("attachment"), Byte())
                Try
                    Using fs As New System.IO.FileStream(strPath, IO.FileMode.Create, IO.FileAccess.Write)
                        fs.Write(fileData, 0, fileData.Length)
                        fs.Flush()
                        fs.Close()
                    End Using
                    System.Diagnostics.Process.Start(strPath)
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            Else
                Dim strPath As String
                strPath = Application.StartupPath + "\ProjectManagerWorkbench\Help\AutobahnUserManual.pdf"
                System.Diagnostics.Process.Start(strPath)
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("helpFileRequested", ex, 1)
        End Try


    End Sub

    Private Sub SupportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SupportToolStripMenuItem.Click
        Dim strShowSupport As String = ""
        Dim ab As New abWorkbenches
        ab.Show()

    End Sub

    Private Sub ExportCheckToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportCheckToolStripMenuItem.Click

        'if no row is selected, look for a selected cell
        If dgvSalesOrders.SelectedRows.Count = 0 Then dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(0).RowIndex).Selected = True

        Dim frm As New frmReportViewer(True, dgvSalesOrders.SelectedRows(0).Cells("order_no").Value.ToString, dgvSalesOrders.SelectedRows(0).Cells("header_id").Value.ToString)
        frm.Show()


    End Sub

    Private Sub UpdateToLatesVersionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateToLatesVersionToolStripMenuItem.Click
        Dim update As New clsApplication

    End Sub


    Private Sub UpdateStatisticsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateStatisticsToolStripMenuItem.Click
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        db.NonQuery("UPDATE STATISTICS mrp_supply")

    End Sub

    '''' <summary>
    '''' Saves the new column order of the top grid to the backend.
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub dgvSalesOrders_ColumnDisplayIndexChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvSalesOrders.ColumnDisplayIndexChanged
    '    'This event already fires within the binding context

    '    'Check if this object is reordering columns.
    '    If Not gridDisplay.propReorderingColumns Then
    '        gridDisplay.saveChangesOfSingleColumnDisplayIndex(e.Column.Name, "saleOrderLinesView", e.Column.DisplayIndex, dgvSalesOrders)
    '    End If
    'End Sub

    '''' <summary>
    '''' Saves the new column order of the middle grid to the backend.
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub dgvShortages_ColumnDisplayIndexChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvShortages.ColumnDisplayIndexChanged
    '    'Have to take care which entity is displayed in the middle
    '    If Not gridDisplay.propReorderingColumns Then
    '        gridDisplay.saveChangesOfSingleColumnDisplayIndex(e.Column.Name, strMiddleGrid, e.Column.DisplayIndex, dgvShortages)
    '    End If
    'End Sub

    ''' <summary>
    ''' Saves the new column order of the botton grid to the backend.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>


    Private Sub SaveReorderingColumnsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveReorderingColumnsToolStripMenuItem.Click

        For i As Integer = 0 To dgvSalesOrders.Columns.Count
            gridDisplay.saveChangesOfSingleColumnDisplayIndex(dgvSalesOrders.Columns(i).Name, "salesOrderLinesView", dgvSalesOrders.Columns(i).DisplayIndex, dgvSalesOrders)
        Next
    End Sub

    Private Sub SaveOrderOfColumnsToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveOrderOfColumnsToolStripMenuItem2.Click

        'wip
        For i As Integer = 0 To dgvWipSupplierQueue.Columns.Count - 1
            gridDisplay.saveChangesOfSingleColumnDisplayIndex(dgvWipSupplierQueue.Columns(i).Name, wipBottonGrid, dgvWipSupplierQueue.Columns(i).DisplayIndex, dgvWipSupplierQueue)
        Next
        gridDisplay.loadColumnSettings()
        MessageBox.Show("Changes saved succesfully.")
    End Sub

    Private Sub SaveOrderOfColumnsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveOrderOfColumnsToolStripMenuItem1.Click
        'po
        For i As Integer = 0 To dgvWipSupplierQueue.Columns.Count - 1
            gridDisplay.saveChangesOfSingleColumnDisplayIndex(dgvWipSupplierQueue.Columns(i).Name, poBottonGrid, dgvWipSupplierQueue.Columns(i).DisplayIndex, dgvWipSupplierQueue)
        Next
        gridDisplay.loadColumnSettings()

        MessageBox.Show("Changes saved succesfully.")
    End Sub

    Private Sub SaveOrderOfColumnsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveOrderOfColumnsToolStripMenuItem.Click
        'shortages
        If strMiddleGrid = "MRPShortagesView" Then
            For i As Integer = 0 To dgvShortages.Columns.Count - 1
                gridDisplay.saveChangesOfSingleColumnDisplayIndex(dgvShortages.Columns(i).Name, "MRPShortagesView", dgvShortages.Columns(i).DisplayIndex, dgvShortages)
            Next
        Else
            'supply demand view
            For i As Integer = 0 To dgvShortages.Columns.Count - 1
                gridDisplay.saveChangesOfSingleColumnDisplayIndex(dgvShortages.Columns(i).Name, supplyDemandMiddleGrid, dgvShortages.Columns(i).DisplayIndex, dgvShortages)
            Next
        End If
        gridDisplay.loadColumnSettings()            'Also the collection of settings has to be updated
        MessageBox.Show("Changes saved succesfully.")
    End Sub

    Private Sub SaveOrderOfColumnsToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveOrderOfColumnsToolStripMenuItem3.Click
        'sales order lines view
        For i As Integer = 0 To dgvSalesOrders.Columns.Count - 1
            gridDisplay.saveChangesOfSingleColumnDisplayIndex(dgvSalesOrders.Columns(i).Name, "salesOrderLinesView", dgvSalesOrders.Columns(i).DisplayIndex, dgvSalesOrders)
        Next
        gridDisplay.loadColumnSettings()

        MessageBox.Show("Changes saved succesfully.")
    End Sub

    Private Sub CutDownReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutDownReportToolStripMenuItem.Click
        'initializing the report to the current displayed data
        Dim frm As New frmReportViewer("WIP Cut Down Report", "ProjectManagerWorkbench\Reporting\repWIP.rdlc", "dsWIP_WIPQueueView", dsWIP)
        frm.Show()

    End Sub

    Private Sub SalesOrderBacklogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesOrderBacklogToolStripMenuItem.Click
        'initializing the report to the current displayed data
        'Dim frm As New frmReportViewer("Sales Order Backlog", "ProjectManagerWorkbench\Reporting\repSalesOrderDGV.rdlc", "dsSalesOrderBacklog_sales_orders", dsSalesOrder)
        'frm.Show()
    End Sub

    Private Sub SetDefaultViewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetDefaultViewToolStripMenuItem.Click
        Dim dlg As New dlgSelectDefaultView(Me.organizationId)
        dlg.ShowDialog()

    End Sub

    Public ReadOnly Property propDsSalesOrders()
        Get
            Return Me.dsSalesOrder
        End Get
    End Property

    Public ReadOnly Property propDsPurchaseOrders()
        Get
            Return Me.dsPO
        End Get
    End Property

    Public ReadOnly Property propDsWIP()
        Get
            Return Me.dsWIP
        End Get
    End Property

    ''' <summary>
    ''' Procedure that deletes the values of the sorting implementation.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub dgvSalesOrders_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvSalesOrders.MouseLeave

        'Resetting the sort variable 
        If lblSalesOrderHeader.Text.Contains("Sorted By") Then
            Dim strNewStatement As String = lblSalesOrderHeader.Text.Substring(0, lblSalesOrderHeader.Text.IndexOf("Sorted"))
            lblSalesOrderHeader.Text = strNewStatement.Trim()

            For i As Integer = 0 To strColumnSorts.Length - 1
                strColumnSorts(i) = ""
                strColumnSortsText(i) = ""
                strSortStatement = ""
                strSortDisplayStatement = ""
            Next
        End If
    End Sub

    Private Sub dgvShortages_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvShortages.MouseLeave

        'Resetting the sort variable
        If lblShortageDescription.Text.Contains("Sorted By") Then
            Dim strNewStatement As String = lblShortageDescription.Text.Substring(0, lblShortageDescription.Text.IndexOf("Sorted"))
            lblShortageDescription.Text = strNewStatement.Trim()

            For i As Integer = 0 To strColumnSorts.Length - 1
                strColumnSorts(i) = ""
                strColumnSortsText(i) = ""
                strSortStatement = ""
                strSortDisplayStatement = ""
            Next
        End If

    End Sub

    Private Sub dgvWipSupplierQueue_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dgvWipSupplierQueue.MouseLeave

        'Resetting the sort variable
        If lblWIPPOFilter.Text.Contains("Sorted By") Then
            Dim strNewStatement As String = lblWIPPOFilter.Text.Substring(0, lblWIPPOFilter.Text.IndexOf("Sorted"))
            lblWIPPOFilter.Text = strNewStatement.Trim()

            For i As Integer = 0 To strColumnSorts.Length - 1
                strColumnSorts(i) = ""
                strColumnSortsText(i) = ""
                strSortStatement = ""
                strSortDisplayStatement = ""
            Next
        End If
    End Sub

    Private Sub frmWorkbenchProjects_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        Me.eraseSortingArray(e)
    End Sub


    ''' <summary>
    ''' Procedure that handles the key down event of the regions to eliminate the sorting arguements.
    ''' </summary>
    ''' <param name="e">The key events</param>
    ''' <remarks></remarks>
    Private Sub eraseSortingArray(ByVal e As System.Windows.Forms.KeyEventArgs)
        'Deletes the memory of the sort array and the labels
        If e.KeyValue = 17 Then
            'Test if the labels contain the sorted by value and change it
            If lblWIPPOFilter.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblWIPPOFilter.Text.Substring(0, lblWIPPOFilter.Text.IndexOf("Sorted"))
                lblWIPPOFilter.Text = strNewStatement.Trim()
            End If
            If lblShortageDescription.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblShortageDescription.Text.Substring(0, lblShortageDescription.Text.IndexOf("Sorted"))
                lblShortageDescription.Text = strNewStatement.Trim()
            End If
            If lblSalesOrderHeader.Text.Contains("Sorted By") Then
                Dim strNewStatement As String = lblSalesOrderHeader.Text.Substring(0, lblSalesOrderHeader.Text.IndexOf("Sorted"))
                lblSalesOrderHeader.Text = strNewStatement.Trim()
            End If


            For i As Integer = 0 To strColumnSorts.Length - 1
                strColumnSorts(i) = ""
                strColumnSortsText(i) = ""
                strSortStatement = ""
                strSortDisplayStatement = ""
            Next
        End If
    End Sub


    Private Sub dgvShortages_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvShortages.KeyUp
        Me.eraseSortingArray(e)
    End Sub

    Private Sub sc1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles sc1.KeyUp
        Me.eraseSortingArray(e)
    End Sub

    Private Sub dgvSalesOrders_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvSalesOrders.KeyUp
        Me.eraseSortingArray(e)
        'also implements the indicator logic
    End Sub

    Private Sub EMailGridToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim operation As New clsGridOperations
        'Dim frm As New frmReportViewer("Sales Order Backlog", "ProjectManagerWorkbench\Reporting\repSalesOrderDGV.rdlc", "dsSalesOrderBacklog_sales_orders", dsSalesOrder)

        'frm.saveCurrentReportToExcel(Application.StartupPath & "\test")
        'frm.Show()
        'frm.Close()

        'operation.exportExcel(dgvSalesOrders, Application.StartupPath & "\test.xml")
        'operation.SendMail("NRuemmeli@Flowserve.com", "NRuemmeli@Flowserve.com", "RHodde@Flowserve.com", "Test for Autobahn Mail", "Sehr geehrte Damen und Herren, dies ist nur ein Test.", Application.StartupPath & "\test.xml", "mailhost.flowserve.com")

    End Sub

    ''' <summary>
    ''' Procedure that calculates the selected values.
    ''' </summary>
    ''' <param name="dgv"></param>
    ''' <remarks></remarks>
    Private Sub calculateSelectedIndicators(ByRef dgv As DataGridView)
        Dim lngSum As Double = 0
        Dim lngAverage As Double = 0
        Dim lngCount As Long = 0

        Try
            If dgv.SelectedCells.Count < 3000 Then
                For i As Integer = 0 To dgv.SelectedCells.Count - 1
                    If dgv.Columns(dgv.SelectedCells(i).ColumnIndex).ValueType.ToString = "System.Double" Then
                        If Not DBNull.Value.Equals(dgv.SelectedCells(i).Value) Then
                            lngCount += 1
                            lngSum += dgv.SelectedCells(i).Value
                            lngAverage = lngSum / lngCount
                        End If
                    Else
                        If Not DBNull.Value.Equals(dgv.SelectedCells(i).Value) Then
                            lngCount += 1
                        End If
                    End If
                Next
                lblSelectedIndicators.Text = "Average:  " & FormatNumber(lngAverage, 2, TriState.True, TriState.True, TriState.True) & " Count:  " & FormatNumber(lngCount, 0, TriState.True, TriState.True, TriState.True) & " Sum:  " & FormatNumber(lngSum, 2, TriState.True, TriState.True, TriState.True)
                lblSelectedIndicators.Visible = True
            End If

        Catch ex As Exception

        End Try


    End Sub


    Private Sub dgvSalesOrders_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvSalesOrders.MouseUp
        calculateSelectedIndicators(dgvSalesOrders)
    End Sub

    Private Sub dgvShortages_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvShortages.MouseUp
        calculateSelectedIndicators(dgvShortages)
    End Sub

    Private Sub dgvWipSupplierQueue_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvWipSupplierQueue.MouseUp
        calculateSelectedIndicators(dgvWipSupplierQueue)
    End Sub

    Private Sub dgvSalesOrders_CellToolTipTextNeeded(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellToolTipTextNeededEventArgs) Handles dgvSalesOrders.CellToolTipTextNeeded

    End Sub

    Private Sub dgvShortages_CellMouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvShortages.CellMouseMove

        If e.RowIndex > -1 AndAlso e.ColumnIndex > -1 Then
            Dim point As Point = PointToClient(MousePosition)
            If (point.X - oldpoint.X) > 2 Or (point.X - oldpoint.X) < -2 Or (point.Y - oldpoint.Y) > 2 Or (point.Y - oldpoint.Y) < -2 Then
                oldpoint = point
                point.X = point.X + 4
                point.Y = point.Y + 4
                Me.ttHoverActions.Show(dgvShortages(e.ColumnIndex, e.RowIndex).Value.ToString, Me, point)
            End If


        Else
            ttHoverActions.Hide(Me)

        End If

    End Sub



    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Me.resetSearchCapabilities()
        Me.searchCrossGrids()

    End Sub
    Private oldpoint As Point

    Private Sub dgvWipSupplierQueue_CellMouseMove_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgvWipSupplierQueue.CellMouseMove

        If e.RowIndex > -1 AndAlso e.ColumnIndex > -1 Then
            Dim point As Point = PointToClient(MousePosition)
            If (point.X - oldpoint.X) > 2 Or (point.X - oldpoint.X) < -2 Or (point.Y - oldpoint.Y) > 2 Or (point.Y - oldpoint.Y) < -2 Then
                oldpoint = point
                point.X = point.X + 20
                point.Y = point.Y - 5
                Me.ttHoverActions.Show(dgvWipSupplierQueue(e.ColumnIndex, e.RowIndex).Value.ToString, Me, point)

            End If


        Else
            ttHoverActions.Hide(Me)
        End If
    End Sub

    Private Sub dgvWipSupplierQueue_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvWipSupplierQueue.CellFormatting
        'every second row will become beige
        'If e.RowIndex Mod 2 = 0 Then
        '    e.CellStyle.BackColor = Color.Beige
        'End If

        If Not DBNull.Value.Equals(e.Value) Then
            If Me.bottonGrid.ToLower = "po" Then
                'Past due need by dates
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("NEED_BY_DATE") AndAlso e.Value < Now Then
                    e.CellStyle.BackColor = Color.Red
                    e.CellStyle.SelectionBackColor = Color.DarkRed
                End If

                'Past due promise dates
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("PROMISE_DATE") AndAlso e.Value < Now Then
                    e.CellStyle.BackColor = Color.Red
                    e.CellStyle.SelectionBackColor = Color.DarkRed
                End If

                'Old purchase order
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("AGE") AndAlso e.Value > 100 Then
                    e.CellStyle.BackColor = Color.Yellow
                    e.CellStyle.SelectionBackColor = Color.DarkMagenta
                ElseIf dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("AGE") Then
                    e.Value = FormatNumber(e.Value, 0, TriState.True, TriState.True, TriState.True)
                End If

                'If the NeededBy (calculated) date is not null AND the PO Promise Date >= Assy Start Date,  color Promise Date red
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("PROMISED_DATE") AndAlso Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("PROMISED_DATE").Value) AndAlso Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(e.RowIndex).Cells("topLevelJobStartDate").Value) Then
                    If dgvWipSupplierQueue.Rows(e.RowIndex).Cells("PROMISED_DATE").Value > dgvWipSupplierQueue.Rows(e.RowIndex).Cells("topLevelJobStartDate").Value Then
                        e.CellStyle.BackColor = Color.Red
                        e.CellStyle.SelectionBackColor = Color.DarkRed
                    End If
                End If

                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("EXTENDED_PRICE") Then
                    e.Value = FormatNumber(e.Value, 2, TriState.True, TriState.True, TriState.True)
                    e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight

                End If

                'Format the percentage stuff
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("percentageSafetyStock") Then
                    e.Value = FormatNumber(e.Value, 2, TriState.True, TriState.True, TriState.True)
                    e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
                End If
            Else

                'Format the wip columns
                'Turn unreleased available columns to green
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("MEANING") AndAlso e.Value = "Unreleased Available" Then
                    e.CellStyle.BackColor = Color.Green
                    e.CellStyle.SelectionBackColor = Color.DarkGreen
                End If

                'Turn unreleased parital available cell to yellow
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("MEANING") AndAlso e.Value = "Unreleased Partial Available" Then
                    e.CellStyle.BackColor = Color.Yellow
                    e.CellStyle.SelectionBackColor = Color.DarkMagenta
                End If

                'Large job turn yellow
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("HOURS_OPEN") AndAlso e.Value > 10 Then
                    e.CellStyle.BackColor = Color.Yellow
                    e.CellStyle.SelectionBackColor = Color.DarkMagenta
                End If

                'First unit start date past due
                If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name.Equals("FIRST_UNIT_START_DATE") AndAlso e.Value < Now Then
                    e.CellStyle.BackColor = Color.Red
                    e.CellStyle.SelectionBackColor = Color.DarkRed
                End If

            End If

        End If

    End Sub

    Private Sub dgvShortages_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvShortages.CellFormatting
        'every second row will become beige
        'If e.RowIndex Mod 2 = 0 Then
        '    e.CellStyle.BackColor = Color.Beige
        'End If

        If Not DBNull.Value.Equals(e.Value) Then

            'Too many levels in the BOM
            If dgvShortages.Columns(e.ColumnIndex).Name.Equals("LEVEL_NO") AndAlso e.Value > 3 Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

            'Past due start date of the job
            If dgvShortages.Columns(e.ColumnIndex).Name.Equals("START_DATE") AndAlso e.Value < Now Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

            'Past due mrp date
            If dgvShortages.Columns(e.ColumnIndex).Name.Equals("MRP_DATE") AndAlso e.Value < Now Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

            'Reason code in the field
            If dgvShortages.Columns(e.ColumnIndex).Name.Equals("REASON_CODE") Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

            'Delay
            If dgvShortages.Columns(e.ColumnIndex).Name.Equals("DELAY") Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

        End If
    End Sub

    Private Sub dgvSalesOrders_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvSalesOrders.CellFormatting
        'every second row will become beige
        'If e.RowIndex Mod 2 = 0 Then
        '    e.CellStyle.BackColor = Color.Beige
        'End If
        If Not DBNull.Value.Equals(e.Value) Then
            'Possible job releasing
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("JOB_PO_STATUS") AndAlso e.Value.ToString.Trim = "Unreleased Available" Then
                e.CellStyle.BackColor = Color.Green
                e.CellStyle.SelectionBackColor = Color.DarkGreen
            End If

            'Dangerous supplies
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("onHandSupplyHolds") Then
                e.CellStyle.BackColor = Color.Yellow
                e.CellStyle.SelectionBackColor = Color.DarkMagenta
            End If

            'Past due promise dates
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("PROMISE_DATE") AndAlso e.Value < Now Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

            'Past due schedule ship date
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("SCHEDULE_SHIP_DATE") AndAlso e.Value < Now Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

            'Past due request dates
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("REQUEST_DATE") AndAlso e.Value < Now Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

            'visualization of a possible partial shipment
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("QTY_ONHAND") AndAlso e.Value > 0 AndAlso dgvSalesOrders.Rows(e.RowIndex).Cells("checkForPartialShipment").Value = "Y" Then
                e.CellStyle.BackColor = Color.Yellow
                e.CellStyle.SelectionBackColor = Color.DarkMagenta
            End If

            'visualization of possible complete shipment
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("QTY_ONHAND") AndAlso e.Value > 0 AndAlso dgvSalesOrders.Rows(e.RowIndex).Cells("orderCompleteForShipping").Value = "Y" Then
                e.CellStyle.BackColor = Color.Green
                e.CellStyle.SelectionBackColor = Color.DarkGreen
            End If

            'Round the extended price to a visual value
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("EXTENDED_PRICE") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("potentialSlippage") Then
                e.Value = FormatNumber(e.Value, 0, TriState.True, TriState.True, TriState.True)
            End If

            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("reasonCode") Then
                e.CellStyle.BackColor = Color.Red
                e.CellStyle.SelectionBackColor = Color.DarkRed
            End If

            'Visualize the project managemenent hold and cancellation hold
            If dgvSalesOrders.Columns(e.ColumnIndex).Name.Equals("HOLDS_HEADER") Then
                If dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Contains("Project Management Review") Or dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString.Contains("Cancellation Hold") Then
                    e.CellStyle.BackColor = Color.Red
                    e.CellStyle.SelectionBackColor = Color.DarkRed
                End If
                
            End If


        End If
    End Sub

    Private Sub dgvFurtherDetails_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvFurtherDetails.CellFormatting
        'every second row will become beige
        If e.RowIndex Mod 2 = 0 Then
            e.CellStyle.BackColor = Color.Beige
        End If
    End Sub

    Private Sub btnDemand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDemand.Click
        Me.populateSupplyDemand(txtSearch.Text)

    End Sub


    Private lastPupup As Date
    Private Sub ttHoverActions_Popup(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PopupEventArgs) Handles ttHoverActions.Popup

        'If (Now - lastPupup).Milliseconds < 500 Then
        '    ttHoverActions.Hide(Me)
        '    lastPupup = Now
        'Else
        '    lastPupup = Now
        'End If

    End Sub

    Private Sub CreatePickReleaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CreatePickReleaseToolStripMenuItem.Click
        Try


            Dim gridOperation As New clsGridOperations
            Dim rows(dgvSalesOrders.SelectedRows.Count) As Integer
            Dim columnNames() As String = {"organization_id", "order_no", "schedule_ship_date"}
            Dim strMessages As String = ""
            Dim nextRow As Integer = 0

            If dgvSalesOrders.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a row first.")
                Exit Sub
            End If

            For i As Integer = 0 To dgvSalesOrders.SelectedRows.Count - 1
                'Make the test with the logic rules to determine the releaseable sales order lines.
                ' a) saveSalesOrders.flow_status_code NOT LIKE 'AWAITING_SHIPPING'
                'b) saveSalesOrders.holds_line IS NOT NULL
                'c) saveSalesOrders.holds_header IS NOT NULL
                'd) saveSalesOrders.checkForPartialShipment NOT LIKE 'Y'
                'e) saveSalesOrders.Pick_Status NOT IN ('Backordered','Ready to Release') 

                'Make an additional condition to check if the export filter was selected
                'Chesapeake business rule
                Dim bAdditionalExportRule As Boolean = False
                If strFilterSalesOrderName = "AM Avail No Holds Due30Days Invoice Doc Hold" Then
                    bAdditionalExportRule = True
                End If

                If dgvSalesOrders.SelectedRows(i).Cells("FLOW_STATUS_CODE").Value <> "AWAITING_SHIPPING" Then
                    strMessages += "Order:" & dgvSalesOrders.SelectedRows(i).Cells("Order_no").Value.ToString & " Line:" & dgvSalesOrders.SelectedRows(i).Cells("LINE_NO").Value.ToString & ControlChars.Tab & " Flow Status Code: " & dgvSalesOrders.SelectedRows(i).Cells("FLOW_STATUS_CODE").Value & vbCr
                    'Here we need to avoid checking the holds in case we want to release it for after market orders
               
                ElseIf dgvSalesOrders.SelectedRows(i).Cells("holds_line").Value.ToString <> "" AndAlso Not bAdditionalExportRule Then
                    strMessages += "Order:" & dgvSalesOrders.SelectedRows(i).Cells("Order_no").Value.ToString & " Line:" & dgvSalesOrders.SelectedRows(i).Cells("LINE_NO").Value.ToString & ControlChars.Tab & " Line Hold Information: " & dgvSalesOrders.SelectedRows(i).Cells("HOLDS_LINE").Value.ToString & vbCr
                ElseIf dgvSalesOrders.SelectedRows(i).Cells("holds_header").Value.ToString <> "" AndAlso Not bAdditionalExportRule Then
                    strMessages += "Order:" & dgvSalesOrders.SelectedRows(i).Cells("Order_no").Value.ToString & " Line:" & dgvSalesOrders.SelectedRows(i).Cells("LINE_NO").Value.ToString & ControlChars.Tab & " Header Hold Information: " & dgvSalesOrders.SelectedRows(i).Cells("HOLDS_HEADER").Value.ToString & vbCr
                ElseIf dgvSalesOrders.SelectedRows(i).Cells("checkForPartialShipment").Value.ToString <> "Y" Then
                    strMessages += "Order:" & dgvSalesOrders.SelectedRows(i).Cells("Order_no").Value.ToString & " Line:" & dgvSalesOrders.SelectedRows(i).Cells("LINE_NO").Value.ToString & ControlChars.Tab & " Not available " & vbCr
                ElseIf dgvSalesOrders.SelectedRows(i).Cells("pick_status").Value.ToString <> "Backordered" AndAlso dgvSalesOrders.SelectedRows(i).Cells("pick_status").Value.ToString <> "Ready to Release" Then
                    strMessages += "Order:" & dgvSalesOrders.SelectedRows(i).Cells("Order_no").Value.ToString & " Line:" & dgvSalesOrders.SelectedRows(i).Cells("LINE_NO").Value.ToString & ControlChars.Tab & " Pick Status: " & dgvSalesOrders.SelectedRows(i).Cells("PICK_STATUS").Value.ToString & vbCr

                Else
                    strMessages += vbCr
                    strMessages += "Order:" & dgvSalesOrders.SelectedRows(i).Cells("Order_no").Value.ToString & " Line:" & dgvSalesOrders.SelectedRows(i).Cells("LINE_NO").Value.ToString & ControlChars.Tab & " Added To Release Request. " & vbCr
                    rows(nextRow) = dgvSalesOrders.SelectedRows(i).Index
                    nextRow += 1
                End If
            Next

            If strMessages.ToCharArray.Count < 2 Then
                MessageBox.Show("No orders selected.")
                Exit Sub
            Else
                MessageBox.Show(strMessages)            '*** Sometimes needs to display more than 100 lines
            End If

            If nextRow = 0 Then
                MessageBox.Show("No orders to release.")
                Exit Sub
            End If

            'create connection to the samba share
            Dim strExtension As String = ""
            If dgvSalesOrders.SelectedRows(0).Cells("organization_id").Value = 881 Or dgvSalesOrders.SelectedRows(0).Cells("organization_id").Value = 1004 Then
                strExtension = "chpso"
            Else
                strExtension = dgvSalesOrders.SelectedRows(0).Cells("organization_id").Value.ToString
            End If
            'Dim strFileName As String = "\\gildv224.flowserve.net\st2dev2_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMddHHmmss") & "]." & strExtension
            'Dim strFileName As String = "\\gildv224.flowserve.net\st2it2_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMddHHmmss") & "]." & strExtension
            'Dim strFileName As String = "\\gilap139.flowserve.net\st1prod_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMddHHmmss") & "]." & strExtension     'Prod
            'Dim strFileName As String = "\\gildv218.flowserve.net\st1uat_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMddHHmmss") & "]." & strExtension       'uat
            Dim strFileName As String = "\\gilis80\sr1prod_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMddHHmmss") & "]." & strExtension   'R12 prod

            Dim strFilterPhrase As String = " IN ("

            For i As Integer = 0 To dgvSalesOrders.SelectedRows.Count - 1
                If rows.Contains(dgvSalesOrders.SelectedRows(i).Index) Then
                    If Not i = dgvSalesOrders.SelectedRows.Count - 1 Then
                        strFilterPhrase += "'" & dgvSalesOrders.SelectedRows(i).Cells("OrderNoLine").Value.ToString + "',"
                    Else
                        strFilterPhrase += "'" & dgvSalesOrders.SelectedRows(i).Cells("OrderNoLine").Value.ToString + "')"
                    End If
                End If

            Next
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

            'Understand which view to pick
            Dim strView As String

            If Me.planningType = "ASCP" Then
                strView = "MSCsalesOrderLinesView"
            Else
                strView = "salesOrderLinesView"
            End If
            Dim ds2 As DataSet = db.SecureQueryParams("SELECT organization_id, order_no, MAX(schedule_ship_date) as schedule_ship_date FROM  " & strView & _
                                                      " WHERE organization_id = @1 AND orderNoLine " & strFilterPhrase & " GROUP BY  organization_id, order_no ", Me.organizationId)
            'Exit Sub '*** TAke this out in order to make the program run again

            gridOperation.exportPipeDelimited(ds2.Tables(0), rows, columnNames, strFileName, ds2.Tables(0).Rows.Count)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("CreatePickReleaseToolStripMenuItem_Click", ex, 1)
        End Try

    End Sub

    Private Sub IndicatorTrackingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IndicatorTrackingToolStripMenuItem.Click
        Dim frm As New frmIndicatorTracking(Me)
        frm.Show()
        frm.WindowState = FormWindowState.Maximized

    End Sub

    ''' <summary>
    ''' Property that executes a filter on WIP.
    ''' </summary>
    ''' <value>The filter phrase that should be searched for.</value>
    ''' <returns>The current temporary filter for that grid.</returns>
    ''' <remarks></remarks>
    Public Property filterBottonWipGrid() As String
        Get
            Return strFilterWIPTemp
        End Get
        Set(ByVal value As String)
            strFilterWIPTemp = value
            Me.loadWip(strFilterWIPTemp)
            Me.bindWipToGrid()

        End Set
    End Property

    Public Sub jumpBottonWipGrid(ByVal value As String, ByVal columnName As String)

        If Me.bottonGrid = "po" Then
            Me.bottonGrid = "wip"
            Me.bindWipToGrid()

        End If
        For i As Integer = 0 To dgvWipSupplierQueue.RowCount - 1

            'For j As Integer = 0 To dgvWipSupplierQueue.Columns.Count - 1
            'Just save the visible columns in the array
            If dgvWipSupplierQueue.Rows(i).Cells(columnName).Visible Then

                'Search the whole row will be a little bit more sophisticated, but implement later
                'Look if the value contains the search strsing
                If dgvWipSupplierQueue.Rows(i).Cells(columnName).Value.ToString.ToLower.Contains(value.ToLower) Then

                    dgvWipSupplierQueue.FirstDisplayedScrollingRowIndex = i
                    'dgvSalesOrders.FirstDisplayedCell = dgvSalesOrders.Rows(i).Cells(j)
                    dgvWipSupplierQueue.Rows(i).Cells(columnName).Selected = True

                    Exit Sub
                End If

            End If
            'Next

        Next
    End Sub

    Private Sub PrintWithSOToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintWithSOToolStripMenuItem.Click
        'initializing the report to the current displayed data
        'dsWIP.Tables(0)
        dsWIP.Tables(0).DefaultView.Sort = "DEPARTMENT_CODE,RESOURCE_CODE,neededBy"
        Dim frm As New frmReportViewer("WIP Report Connection to Sales Order", "ProjectManagerWorkbench\Reporting\repWIPSO.rdlc", "dsWIP_WIPQueueView", dsWIP)
        frm.Show()
    End Sub

    ''' <summary>
    ''' Invokes the info tool to show general information about design of the product.
    ''' </summary>
    Private Sub InfoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InfoToolStripMenuItem.Click
        Dim frm As New abWorkbenches
        frm.Show()
    End Sub

    Private Sub SalesOrderPrintoutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesOrderPrintoutToolStripMenuItem.Click
        'Select the history of comments into one field
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        db.Connect()
        Dim numberOfComments As Integer = 5

        'Speed up the discussion view
        'Pull all the open orders into a table - be able to look up the values
        'Sort the data grid according the original format - shedule_ship_date

        For i As Integer = 0 To dsSalesOrder.Tables(0).Rows.Count - 1
            If Not DBNull.Value.Equals(dsSalesOrder.Tables(0).Rows(i).Item("lastComment")) Then
                Try

                    Dim strCombinedMessage As String = ""
                    Dim ds As DataSet = db.SecureQueryParams("SELECT TOP " & numberOfComments.ToString & " co.[description] [Comment], co.creation_date , co.created_by, wun.notificationTo " & _
                                                      " , wu1.lastName + ', ' +  wu1.firstName [Creation_Name], co.commentID, co.ENTITY_ID, co.HEADER_ID, co.LINE_ID, co.SHIPMENT_NO,co.organizationID, co.releaseNo " & _
                                                      "  , wu2.lastName + ', ' + wu2.firstName [Notification_To] " & _
                                                      " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                                      " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                                      " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                                      " WHERE  co.entity_id = 'salesOrderLinesView' AND co.organizationID = @1 AND co.header_id = @2 AND co.line_id = @3  ORDER BY co.creation_date DESC", _
                                                      organizationId, dsSalesOrder.Tables(0).Rows(i).Item("header_id"), dsSalesOrder.Tables(0).Rows(i).Item("line_no"))

                    For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If Not DBNull.Value.Equals(ds.Tables(0).Rows(j).Item("Notification_To")) Then
                            strCombinedMessage += ds.Tables(0).Rows(j).Item("Creation_Name").ToString + " To " + ds.Tables(0).Rows(j).Item("Notification_To").ToString + " " + ds.Tables(0).Rows(j).Item("creation_date").ToString + " :" + ds.Tables(0).Rows(j).Item("Comment").ToString
                        Else
                            strCombinedMessage += ds.Tables(0).Rows(j).Item("Creation_Name").ToString + " " + ds.Tables(0).Rows(j).Item("creation_date").ToString + ": " + ds.Tables(0).Rows(j).Item("Comment").ToString
                        End If

                        strCombinedMessage += vbCrLf + vbCrLf

                    Next
                    Dim dc1 As New DataColumn("SODiscussion", Type.GetType("System.String"))
                    'Check if this column is already in this table
                    If dsSalesOrder.Tables(0).Columns.Contains("SODiscussion") Then
                    Else
                        dsSalesOrder.Tables(0).Columns.Add(dc1)
                    End If

                    dsSalesOrder.Tables(0).Rows(i).Item("SODiscussion") = strCombinedMessage
                Catch ex As Exception

                End Try
            End If

            'Add the lower level discussion
            If Not DBNull.Value.Equals(dsSalesOrder.Tables(0).Rows(i).Item("checkForPartialShipment")) AndAlso dsSalesOrder.Tables(0).Rows(i).Item("checkForPartialShipment") = "N" Then

                'Subdivide buy and make
                Dim ds As DataSet = db.SecureQueryParams("SELECT [ITEM_NO] " & _
                                               ",[DESCRIPTION],STATUS, REASON_CODE,MAKE_BUY, job_no, OP1,OP2, OP3,OP4, purchase_order_no, QTY_ONHAND, purch_line_no, purchase_order_id, MRP_QTY, PO_PROMISED_DATE, LEVEL_NO, MBLevel  FROM [MRPShortagesView] WHERE  end_demand_id = @1  " & _
                                               " AND  organization_id = @2 AND short is not null ORDER BY [OrderNoLine], neededBy DESC ", dsSalesOrder.Tables(0).Rows(i).Item("demand_id"), Me.organizationId)

                Dim strMake As String = ""
                Dim strBuy As String = ""

                For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    If ds.Tables(0).Rows(j).Item("MAKE_BUY") = "M" Then
                        'Make discussion
                        strMake += ds.Tables(0).Rows(j).Item("MBLevel").ToString + " " + ds.Tables(0).Rows(j).Item("ITEM_NO") + " | " + ds.Tables(0).Rows(j).Item("DESCRIPTION").ToString + "|" + ds.Tables(0).Rows(j).Item("REASON_CODE").ToString + vbCrLf
                        strMake += "Job:" & ds.Tables(0).Rows(j).Item("job_no").ToString & " - " & _
                            "Status:" & ds.Tables(0).Rows(j).Item("Status").ToString & _
                            " - Quantity:" & ds.Tables(0).Rows(j).Item("MRP_QTY").ToString & _
                            " - On Hand:" & ds.Tables(0).Rows(j).Item("QTY_ONHAND").ToString & _
                            " - Res:" & ds.Tables(0).Rows(j).Item("OP1").ToString & " | " & ds.Tables(0).Rows(j).Item("OP2").ToString & " | " & ds.Tables(0).Rows(j).Item("OP3").ToString & " | " & ds.Tables(0).Rows(j).Item("OP4").ToString

                        'Only OSP description when we have a purchase order there
                        If Not DBNull.Value.Equals(ds.Tables(0).Rows(j).Item("purchase_order_no")) Then
                            strMake += " - OSP PO:" & ds.Tables(0).Rows(j).Item("purchase_order_no").ToString & _
                                       " - Promise Date:" & ds.Tables(0).Rows(j).Item("PO_PROMISED_DATE").ToString
                        End If

                        strMake += vbCrLf + vbCrLf
                        'Comments for the current operation sequence
                    Else
                        'Buy discussion
                        strBuy += ds.Tables(0).Rows(j).Item("MBLevel") + " " + ds.Tables(0).Rows(j).Item("ITEM_NO") + "|" + ds.Tables(0).Rows(j).Item("DESCRIPTION") + "| PO:" + ds.Tables(0).Rows(j).Item("purchase_order_no").ToString + "| Line: " + ds.Tables(0).Rows(j).Item("purch_line_no").ToString + "| Prom:" + ds.Tables(0).Rows(j).Item("PO_PROMISED_DATE").ToString + "|" + ds.Tables(0).Rows(j).Item("REASON_CODE")
                        strBuy += vbCrLf
                        'Comments for the buy part
                        Dim dsCo As DataSet = db.SecureQueryParams("SELECT TOP 2 co.[description] [Comment], co.creation_date , co.created_by, wun.notificationTo " & _
                                                      " , wu1.lastName + ', ' +  wu1.firstName [Creation_Name], co.commentID, co.ENTITY_ID, co.HEADER_ID, co.LINE_ID, co.SHIPMENT_NO,co.organizationID, co.releaseNo " & _
                                                      "  , wu2.lastName + ', ' + wu2.firstName [Notification_To] " & _
                                                      " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                                      " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                                      " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                                      " WHERE  co.entity_id = 'POQueueView' AND co.organizationID = @1 AND co.header_id = @2 AND co.line_id = @3  ORDER BY co.creation_date DESC", _
                                                      organizationId, ds.Tables(0).Rows(j).Item("purchase_order_id"), ds.Tables(0).Rows(j).Item("purch_line_no"))

                        For h As Integer = 0 To dsCo.Tables(0).Rows.Count - 1
                            If Not DBNull.Value.Equals(dsCo.Tables(0).Rows(h).Item("Notification_To")) Then
                                strBuy += ControlChars.Tab + dsCo.Tables(0).Rows(h).Item("Creation_Name").ToString + " To " + dsCo.Tables(0).Rows(h).Item("Notification_To").ToString + " " + dsCo.Tables(0).Rows(h).Item("creation_date").ToString + " :" + dsCo.Tables(0).Rows(h).Item("Comment").ToString
                            Else
                                strBuy += ControlChars.Tab + dsCo.Tables(0).Rows(h).Item("Creation_Name").ToString + " " + dsCo.Tables(0).Rows(h).Item("creation_date").ToString + ": " + dsCo.Tables(0).Rows(h).Item("Comment").ToString
                            End If

                            strBuy += vbCrLf + vbCrLf
                        Next

                    End If

                Next

                Dim dc1 As New DataColumn("WIPDiscussion", Type.GetType("System.String"))
                'Check if this column is already in this table
                If dsSalesOrder.Tables(0).Columns.Contains("WIPDiscussion") Then
                Else
                    dsSalesOrder.Tables(0).Columns.Add(dc1)
                End If
                dsSalesOrder.Tables(0).Rows(i).Item("WIPDiscussion") = strMake

                Dim dc2 As New DataColumn("PODiscussion", Type.GetType("System.String"))
                'Check if this column is already in this table
                If dsSalesOrder.Tables(0).Columns.Contains("PODiscussion") Then
                Else
                    dsSalesOrder.Tables(0).Columns.Add(dc2)
                End If
                dsSalesOrder.Tables(0).Rows(i).Item("PODiscussion") = strBuy

            End If

        Next
        Dim frm As New frmReportViewer("Sales Order Backlog", "ProjectManagerWorkbench\Reporting\repSOPrint.rdlc", "dsSalesOrderBacklog_sales_orders", dsSalesOrder, lblSalesOrderHeader.Text)
        frm.Show()

        db.Disconnect()
    End Sub

    Private Sub PlannedOrdersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlannedOrdersToolStripMenuItem.Click
        'Opens the shortages in the middle grid
        Me.populateMRPShortages(0, Double.Parse(Me.organizationId), strFilterMRPShortagesTemp, 0, " ", True)
    End Sub


    Private Sub IntegratedWorkbenchIntegrationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IntegratedWorkbenchIntegrationToolStripMenuItem.Click
        Try
        Dim frm As New frmIntegratedWorkflow(Environment.UserName, Me.organizationId, Me)
        frm.Show()
        Catch ex As Exception
            'Dim err As New clsExceptionManagement
            'err.createErrorLog("IntegratedWorkbenchIntegrationToolStripMenuItem_Click", ex, 1)
        End Try
    End Sub

    Private Sub addRecordCountToSoLabel()
        Try


            Dim sumObject As Double = dsSalesOrder.Tables(0).Compute("Sum(Extended_Price)", "") / 1000
            Dim strValue As String = Format(sumObject, "C0") + "k"
            lblSalesOrderHeader.Text += " " + dgvSalesOrders.Rows.Count.ToString + " / " + lngCountSalesOrderLines.ToString + " Records Displayed" + " - Value: " + strValue
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Subroutine that loads the sales orders with a defined filter.
    ''' </summary>
    ''' <param name="filterName">The name of the filter to be displayed on the label.</param>
    ''' <param name="filterString">The additional filter string.</param>
    ''' <remarks></remarks>
    Public Sub loadFilterOnSO(ByVal filterName As String, ByVal filterString As String)
        Me.loadSalesOrder(filterString)
        Me.lblSalesOrderHeader.Text = filterName            'has to go directly to the load so procedure
        Me.addRecordCountToSoLabel()
    End Sub
    ''' <summary>
    ''' Subroutine that loads the shortages with a defined filter.
    ''' </summary>
    ''' <param name="filterName">The name of the filter we are loading.</param>
    ''' <param name="filterString">The filter we want ot apply.</param>
    ''' <remarks></remarks>
    Public Sub loadFilterOnShortages(ByVal filterName As String, ByVal filterString As String, ByVal sortOrder As String)
        'Me.populateMRPShortages(0, Me.organizationId, filterString, , , True)
        Me.populateMRPShortages(0, Double.Parse(Me.organizationId), filterString, 0, " ", True, True)
        Try
            Dim dt As DataTable = Me.dgvShortages.DataSource
            dt.DefaultView.Sort = sortOrder
            lblShortageDescription.Text = filterName
        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' Subroutine that loads the purchase orders with a defined filter.
    ''' </summary>
    ''' <param name="filterName">The name of the filter.</param>
    ''' <param name="filterString">The filter we want to apply.</param>
    ''' <remarks></remarks>
    Public Sub loadFilterOnPurchaseOrders(ByVal filterName As String, ByVal filterString As String, ByVal sortOrder As String)
        If Not String.IsNullOrEmpty(sortOrder) Then
            Me.loadPO(filterString, sortOrder)
        Else
            Me.loadPO(filterString)
        End If

        lblWIPPOFilter.Text = filterName

    End Sub
    ''' <summary>
    ''' Subroutine that load the discrete jobs with a defined filter.
    ''' </summary>
    ''' <param name="filterName">The filter name we want to use.</param>
    ''' <param name="filterString">The filter string that should be applied.</param>
    ''' <remarks></remarks>
    Public Sub loadFilterOnDiscreteJobs(ByVal filterName As String, ByVal filterString As String, ByVal sortOrder As String)
        Me.loadWip(filterString)
        Me.bindWipToGrid()
        lblWIPPOFilter.Text = filterName
        Dim dt As DataTable = dgvWipSupplierQueue.DataSource
        dt.DefaultView.Sort = sortOrder
    End Sub

    Private Sub DiscussionTrackingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscussionTrackingToolStripMenuItem.Click
        Dim frm As New frmDiscussionTracking(Me.organizationId, Me)
        frm.Show()

    End Sub

    Private Sub lblNotifications_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblNotifications.DoubleClick
        Dim frm As New frmDiscussionTracking(Me.organizationId, Me)
        frm.Show()
        frm.loadIWasNotified()

    End Sub

    Private Sub lblNotifications_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lblNotifications.MouseDoubleClick
        Dim frm As New frmDiscussionTracking(Me.organizationId, Me)
        frm.Show()
        frm.loadIWasNotified()
    End Sub

    Private Sub OnHandItemsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OnHandItemsToolStripMenuItem.Click
        'Load it with the on hand values
        Me.populateMRPShortages(Me.lnDemandId, Me.organizationId, "", 0, "", False, False, "AND short is null")
        Try
            lblShortageDescription.Text = "On Hand Items for Order " + dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(0).RowIndex).Cells("orderNoline").Value.ToString
        Catch ex As Exception

        End Try
    End Sub

    Private Sub POPrintReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles POPrintReportToolStripMenuItem.Click
        'Loads the purchase order promise date
        'Select the history of comments into one field

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        db.Connect()
        Try


            Dim numberOfComments As Integer = 10
            For i As Integer = 0 To dgvWipSupplierQueue.Rows.Count - 1
                If Not DBNull.Value.Equals(dgvWipSupplierQueue.Rows(i).Cells("lastComment")) Then
                    Try


                        Dim strCombinedMessage As String = ""
                        Dim ds As DataSet = db.SecureQueryParams("SELECT TOP " & numberOfComments.ToString & " co.[description] [Comment], co.creation_date [Creation Date], co.created_by, wun.notificationTo " & _
                                                          " , wu1.lastName + ', ' +  wu1.firstName [Creation_Name], co.commentID, co.ENTITY_ID, co.HEADER_ID, co.LINE_ID, co.SHIPMENT_NO,co.organizationID, co.releaseNo " & _
                                                          "  , wu2.lastName + ', ' + wu2.firstName [Notification_To] " & _
                                                          " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                                          " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                                          " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                                          " WHERE  co.entity_id = 'POQueueView' AND co.organizationID = @1 AND co.header_id = @2 AND co.line_id = @3 AND co.shipment_no = @4 AND co.releaseNo = @5  ORDER BY co.creation_date DESC", _
                                                          organizationId, dgvWipSupplierQueue.Rows(i).Cells("po_header_id").Value, dgvWipSupplierQueue.Rows(i).Cells("line_no").Value, dgvWipSupplierQueue.Rows(i).Cells("shipment_no").Value, dgvWipSupplierQueue.Rows(i).Cells("release_No").Value)

                        For j As Integer = 0 To ds.Tables(0).Rows.Count - 1
                            If Not DBNull.Value.Equals(ds.Tables(0).Rows(j).Item("Notification_To")) Then
                                strCombinedMessage += FormatDateTime(ds.Tables(0).Rows(j).Item("Creation Date"), DateFormat.ShortDate) + ds.Tables(0).Rows(j).Item("Creation_Name").ToString + " To " + ds.Tables(0).Rows(j).Item("Notification_To").ToString + " :" + ds.Tables(0).Rows(j).Item("Comment").ToString
                            Else
                                strCombinedMessage += FormatDateTime(ds.Tables(0).Rows(j).Item("Creation Date"), DateFormat.ShortDate) + ds.Tables(0).Rows(j).Item("Creation_Name").ToString + ": " + ds.Tables(0).Rows(j).Item("Comment").ToString
                            End If

                            strCombinedMessage += vbCrLf
                        Next
                        dsPO.Tables(0).Rows(i).Item("lastComment") = strCombinedMessage
                    Catch ex As Exception

                    End Try
                End If
            Next
            Dim frm As New frmReportViewer("Purchase Order Backlog", "ProjectManagerWorkbench\Reporting\repPOSox.rdlc", "dsPO_POQueueView", dsPO)
            frm.Show()


            db.Disconnect()
        Catch ex As Exception
        Finally
            db.Disconnect()
        End Try
    End Sub

    Private Sub ViewDrawingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewDrawingsToolStripMenuItem.Click
        Me.createListOfDrawingsForGrid(dgvSalesOrders)
    End Sub


    Private Sub ViewDrawingsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewDrawingsToolStripMenuItem1.Click
        Me.createListOfDrawingsForGrid(dgvWipSupplierQueue)
    End Sub

    ''' <summary>
    ''' Procedure that creates a list of drawings for all selected rows in the grid
    ''' </summary>
    ''' <param name="grid">The grid you want to run the drawings on.</param>
    ''' <remarks></remarks>
    Public Sub createListOfDrawingsForGrid(ByVal grid As DataGridView)
        Try
            'Make sure the user can also select only the cell!
            If grid.SelectedRows.Count = 0 Then
                If grid.SelectedCells.Count > 0 Then
                    For i As Integer = 0 To grid.SelectedCells.Count - 1
                        grid.Rows(grid.SelectedCells(i).RowIndex).Selected = True
                    Next
                End If
            End If

            Dim listOfDrawings(grid.SelectedRows.Count - 1) As String
            Dim listOfRevisions(grid.SelectedRows.Count - 1) As String

            Dim j As Integer = 0

            For Each row As DataGridViewRow In grid.SelectedRows
                'Have to resplit the string
                Dim listOfWords() As String = row.Cells("drawing").Value.ToString.Split(";")
                'Make sure to redim the array
                If listOfWords.Count > 2 Then
                    'Have to distinct between STAR and EDC
                    'Adding all the possible drawings
                    Dim length As Integer = listOfDrawings.Count
                    ReDim Preserve listOfDrawings(length + listOfWords.Count - 2)
                    ReDim Preserve listOfRevisions(length + listOfWords.Count - 2)

                    For k As Integer = 0 To listOfWords.Count - 2
                        'Distinct between the presentation of STAR and EDC
                        If listOfWords(k).Contains("(") Then
                            listOfDrawings(j) = listOfWords(k).Substring(0, listOfWords(k).IndexOf("(")).Trim
                            listOfRevisions(j) = listOfWords(k).Substring(listOfWords(k).IndexOf("(") + 1, listOfWords(k).IndexOf(")") - listOfWords(k).IndexOf("(") - 1)
                        ElseIf listOfWords(k).Contains("Rev") Then

                        End If

                        j += 1
                    Next

                    'Adding all the possible revisions

                Else
                    If row.Cells("drawing").Value <> "" Then
                        'One possible Format is: drawing (rev);
                        If row.Cells("drawing").Value.ToString.Contains("(") Then
                            listOfDrawings(j) = row.Cells("drawing").Value.ToString.Substring(0, row.Cells("drawing").Value.ToString.IndexOf("(")).Trim
                            listOfRevisions(j) = row.Cells("drawing").Value.ToString.Substring(row.Cells("drawing").Value.ToString.IndexOf("(") + 1, row.Cells("drawing").Value.ToString.IndexOf(")") - row.Cells("drawing").Value.ToString.IndexOf("(") - 1).Trim
                        ElseIf row.Cells("drawing").Value.ToString.Contains("Rev") Then
                            Dim strDrawing = row.Cells("drawing").Value.ToString.Substring(row.Cells("drawing").Value.ToString.IndexOf(":") + 1, row.Cells("drawing").Value.ToString.IndexOf("Sht") - 4)
                            strDrawing = strDrawing.Replace("Sht:", " ").Trim

                            listOfDrawings(j) = strDrawing
                            Dim strRevision As String = row.Cells("drawing").Value.ToString.Substring(row.Cells("drawing").Value.ToString.IndexOf("Rev:"), row.Cells("drawing").Value.ToString.Count - row.Cells("drawing").Value.ToString.IndexOf("Rev:"))
                            strRevision = strRevision.Replace("Rev:", "")
                            strRevision = strRevision.Replace(";", "").Trim
                            listOfRevisions(j) = strRevision

                        End If

                    End If

                    j += 1
                End If

            Next
            'start the dialogue
            Dim dlg As New dlgDisplayDrawings(listOfDrawings, listOfRevisions)
            dlg.ShowDialog()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ViewDrawingsToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewDrawingsToolStripMenuItem2.Click
        Me.createListOfDrawingsForGrid(dgvShortages)

    End Sub

    Private Sub ViewDrawingsToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewDrawingsToolStripMenuItem3.Click
        Me.createListOfDrawingsForGrid(dgvWipSupplierQueue)
    End Sub

    Private Sub loadSettingsOfWorkbench()

        Me.Size = My.Settings.projectManagerWorkebenchSize
        Me.sc1.SplitterDistance = My.Settings.projectManagerWorkbenchSc1SplitterDistance
        Me.sc2.SplitterDistance = My.Settings.projectManagerWorkbenchSc2SplitterDistance

    End Sub

    Private Sub frmWorkbenchProjects_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Saves the settings of the form

        My.Settings.projectManagerWorkbenchSc1Panel1 = Me.sc1.Panel1.Size
        My.Settings.projectManagerWorkbenchSc1Panel2 = Me.sc1.Panel2.Size
        My.Settings.projectManagerWorkbenchSc2Panel1 = Me.sc2.Panel1.Size
        My.Settings.projectManagerWorkbenchSc2Panel2 = Me.sc2.Panel2.Size
        My.Settings.projectManagerWorkbenchSc1 = Me.sc1.Size
        My.Settings.projectManagerWorkbenchSc2 = Me.sc2.Size
        My.Settings.projectManagerWorkbenchSc1SplitterDistance = Me.sc1.SplitterDistance
        My.Settings.projectManagerWorkbenchSc2SplitterDistance = Me.sc2.SplitterDistance

        My.Settings.projectManagerWorkebenchSize = Me.Size

        My.Settings.Save()

    End Sub

    Private Sub DailyDashboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DailyDashboardToolStripMenuItem.Click
        Dim frm As New frmOneDashboard(Me.organizationId)
        frm.Show()

    End Sub

    Private Sub BacklogOverviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BacklogOverviewToolStripMenuItem.Click
        'initializing the report to the current displayed data
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.Query("SELECT TOP 100 * FROM tblWork")
        Dim frm As New frmReportViewer("Sales Order Backlog Overview", "ProjectManagerWorkbench\Reporting\repBacklogOverview.rdlc", "dsWork_tblWork", ds)
        frm.Show()

    End Sub

    Private Sub dgvSalesOrders_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvSalesOrders.KeyDown
        'Display the comment form
        Try
            If e.KeyValue <> 17 And e.KeyValue <> 16 And e.KeyValue <> 40 And e.KeyValue <> 38 Then 'Allow Ctrl, Shfit, Up Arrow and Down Arrow Keys to perform multiple selection
                If dgvSalesOrders.Columns.Count > 0 Then
                    If dgvSalesOrders.Columns(dgvSalesOrders.CurrentCell.ColumnIndex).Name = "lastComment" Then
                        If Not dgvSalesOrders.CurrentCell.RowIndex = -1 Then
                            Dim listSelectedCells As New List(Of clsSelectedCells)
                            For index As Integer = 0 To dgvSalesOrders.SelectedCells.Count - 1
                                Dim selectedItem As New clsSelectedCells
                                If dgvSalesOrders.Columns(dgvSalesOrders.SelectedCells(index).ColumnIndex).Name = "lastComment" Then
                                    selectedItem.header_id = dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(index).RowIndex).Cells("HEADER_ID").Value
                                    selectedItem.headerName = dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(index).RowIndex).Cells("ORDER_NO").Value
                                    selectedItem.line_id = dgvSalesOrders.Rows(dgvSalesOrders.SelectedCells(index).RowIndex).Cells("LINE_NO").Value()
                                    selectedItem.shipment_no = 0
                                    selectedItem.entity_name = "salesOrderLinesView"
                                    selectedItem.dgv = dgvSalesOrders
                                    selectedItem.row = dgvSalesOrders.SelectedCells(index).RowIndex()
                                    selectedItem.organizationId = Me.organizationId
                                    selectedItem.releaseNo = 0
                                    listSelectedCells.Add(selectedItem)
                                End If
                            Next
                            If listSelectedCells.Count > 1 Then
                                'For multiple lines
                                Dim dlg As New dlgComments(listSelectedCells)
                                dlg.ShowDialog()
                                If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                                    Me.loadCurrentNotifications()
                                End If
                            Else
                                'For single line - this is existing functionality taken from CellContentClick
                                Dim dlg As New dlgComments(dgvSalesOrders.Rows(dgvSalesOrders.CurrentCell.RowIndex).Cells("HEADER_ID").Value, dgvSalesOrders.Rows(dgvSalesOrders.CurrentCell.RowIndex).Cells("ORDER_NO").Value, dgvSalesOrders.Rows(dgvSalesOrders.CurrentCell.RowIndex).Cells("LINE_NO").Value, 0, "salesOrderLinesView", dgvSalesOrders, dgvSalesOrders.CurrentCell.RowIndex, Me.organizationId)
                                dlg.ShowDialog()
                                If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                                    Me.loadCurrentNotifications()
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                IsCtrlKeyPressed = True
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("salesOrderKeyDown", ex, 1)
        End Try
    End Sub

    Private Sub dgvShortages_KeyDown1(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvShortages.KeyDown

        'Display the comment form
        Try
            If e.KeyValue <> 17 And e.KeyValue <> 16 And e.KeyValue <> 40 And e.KeyValue <> 38 Then 'Allow Ctrl, Shfit, Up Arrow and Down Arrow Keys to perform multiple selection
                If dgvSalesOrders.Columns.Count > 0 Then
                    If dgvShortages.Columns(dgvShortages.CurrentCell.ColumnIndex).Name = "lastComment" Then
                        If Not dgvShortages.CurrentCell.RowIndex = -1 Then
                            Dim listSelectedCells As New List(Of clsSelectedCells)
                            For index As Integer = 0 To dgvShortages.SelectedCells.Count - 1
                                Dim selectedItem As New clsSelectedCells
                                If dgvShortages.Columns(dgvShortages.SelectedCells(index).ColumnIndex).Name = "lastComment" Then
                                    If dgvShortages.Rows(dgvShortages.SelectedCells(index).RowIndex).Cells("SUPPLY_TYPE").Value.ToString = "Planned order" Then
                                        selectedItem.header_id = dgvShortages.Rows(dgvShortages.SelectedCells(index).RowIndex).Cells("ORDER_NO").Value
                                        selectedItem.headerName = dgvShortages.Rows(dgvShortages.SelectedCells(index).RowIndex).Cells("ORDER_NO").Value
                                        selectedItem.line_id = dgvShortages.Rows(dgvShortages.SelectedCells(index).RowIndex).Cells("LINE_NO").Value
                                        selectedItem.shipment_no = dgvShortages.Rows(dgvShortages.SelectedCells(index).RowIndex).Cells("INVENTORY_ITEM_ID").Value
                                        selectedItem.entity_name = "plannedOrder"
                                        selectedItem.dgv = dgvShortages
                                        selectedItem.row = dgvShortages.SelectedCells(index).RowIndex()
                                        selectedItem.organizationId = Me.organizationId
                                        selectedItem.releaseNo = 0
                                        listSelectedCells.Add(selectedItem)
                                    Else
                                        MessageBox.Show("Comments in the shortage section are just possible for planned orders.")
                                        IsCtrlKeyPressed = False
                                        Exit Sub
                                    End If
                                End If
                            Next
                            If listSelectedCells.Count > 1 Then
                                'For multiple lines
                                Dim dlg As New dlgComments(listSelectedCells)
                                dlg.ShowDialog()
                                If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                                    Me.loadCurrentNotifications()
                                End If
                            Else
                                'For single line - this is existing functionality taken from CellContentClick
                                Dim dlg As New dlgComments(dgvShortages.Rows(dgvShortages.CurrentCell.RowIndex).Cells("HEADER_ID").Value, dgvShortages.Rows(dgvShortages.CurrentCell.RowIndex).Cells("ORDER_NO").Value, dgvShortages.Rows(dgvShortages.CurrentCell.RowIndex).Cells("LINE_NO").Value, dgvShortages.Rows(dgvShortages.CurrentCell.RowIndex).Cells("INVENTORY_ITEM_ID").Value, "plannedOrder", dgvShortages, dgvShortages.CurrentCell.RowIndex, Me.organizationId)
                                dlg.ShowDialog()
                                If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                                    Me.loadCurrentNotifications()
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                IsCtrlKeyPressed = True
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("shortagesKeyDown", ex, 1)
        End Try
    End Sub

    Private Sub CopyColumnOrderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyColumnOrderToolStripMenuItem.Click
        Dim dlg As New dlgCopyColumnOrder(Me.organizationId)
        dlg.ShowDialog()
    End Sub

    Private Sub ManageColumnOrderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageColumnOrderToolStripMenuItem.Click
        Dim dlg As New dlgManageColumnOrder(Me.organizationId, Me.planningType)
        dlg.ShowDialog()
    End Sub

    Private Sub ManageUsersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ManageUsersToolStripMenuItem.Click
        Dim frm As New frmUserSetup()
        frm.ShowDialog()
    End Sub

    Private Sub ReleaseNotesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReleaseNotesToolStripMenuItem.Click
        Dim dlg As New dlgReleaseNotes()
        dlg.ShowDialog()
    End Sub

    Private Sub dgvSalesOrders_CellMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSalesOrders.CellMouseEnter
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            If dgvSalesOrders.Columns(e.ColumnIndex).Name = "ITEM_NO" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "JOB_PO" _
            Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "ORDER_NO" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OrderNoLine" _
            Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP1" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP2" _
            Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP3" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "OP4" Or dgvSalesOrders.Columns(e.ColumnIndex).Name = "lastComment" Then
                dgvSalesOrders.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.LightBlue
            End If
        End If
    End Sub

    Private Sub dgvWipSupplierQueue_CellMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWipSupplierQueue.CellMouseEnter
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "ITEM_NO" Or dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "SalesOrderLine" _
            Or dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "JobNoOperationSeq" Or dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "lastComment" Then
                dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.LightBlue
            End If
        End If
    End Sub

    Private Sub dgvWipSupplierQueue_CellMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWipSupplierQueue.CellMouseLeave
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            If dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "ITEM_NO" Or dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "SalesOrderLine" _
            Or dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "JobNoOperationSeq" Or dgvWipSupplierQueue.Columns(e.ColumnIndex).Name = "lastComment" Then
                dgvWipSupplierQueue.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Empty
            End If
        End If
    End Sub

    Private Sub dgvShortages_CellMouseEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvShortages.CellMouseEnter
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            If dgvShortages.Columns(e.ColumnIndex).Name = "ITEM_NO" Or dgvShortages.Columns(e.ColumnIndex).Name = "SalesOrderLine" _
            Or dgvShortages.Columns(e.ColumnIndex).Name = "OrderNoLine" Or dgvShortages.Columns(e.ColumnIndex).Name = "parentJobOrderNo" _
            Or dgvShortages.Columns(e.ColumnIndex).Name = "OP1" Or dgvShortages.Columns(e.ColumnIndex).Name = "OP2" _
            Or dgvShortages.Columns(e.ColumnIndex).Name = "OP3" Or dgvShortages.Columns(e.ColumnIndex).Name = "OP4" _
            Or dgvShortages.Columns(e.ColumnIndex).Name = "JobPOLine" Then
                dgvShortages.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.LightBlue
            End If
        End If
    End Sub

    Private Sub dgvShortages_CellMouseLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvShortages.CellMouseLeave
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            If dgvShortages.Columns(e.ColumnIndex).Name = "ITEM_NO" Or dgvShortages.Columns(e.ColumnIndex).Name = "SalesOrderLine" _
            Or dgvShortages.Columns(e.ColumnIndex).Name = "OrderNoLine" Or dgvShortages.Columns(e.ColumnIndex).Name = "parentJobOrderNo" _
            Or dgvShortages.Columns(e.ColumnIndex).Name = "OP1" Or dgvShortages.Columns(e.ColumnIndex).Name = "OP2" _
            Or dgvShortages.Columns(e.ColumnIndex).Name = "OP3" Or dgvShortages.Columns(e.ColumnIndex).Name = "OP4" _
            Or dgvShortages.Columns(e.ColumnIndex).Name = "JobPOLine" Then
                dgvShortages.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Empty
            End If
        End If
    End Sub

    Private Sub PrintJobsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintJobsToolStripMenuItem.Click
        Dim frm As New frmReportViewer("WIP Report Connection to Sales Order", "ProjectManagerWorkbench\Reporting\repWIPSOJob.rdlc", "dsWIP_WIPQueueView", dsWIP)
        frm.Show()
    End Sub

    Private Sub ViewAllShortagesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewAllShortagesToolStripMenuItem.Click
        'Opens the shortages in the middle grid
        Me.populateMRPShortages(0, Double.Parse(Me.organizationId), strFilterMRPShortagesTemp, 0, " ", True, False, "AND short is not null", True)
    End Sub


End Class


Public Class poSearch
    Public rownumber As Integer
    Public searchPhrase As String

    Public Sub New(ByVal row As Integer, ByVal search As String)
        Me.rownumber = row
        Me.searchPhrase = search

    End Sub
End Class

Public Class jobSearch
    Public rownumber As Integer
    Public searchPhrase As String
    Public Sub New(ByVal row As Integer, ByVal search As String)
        Me.rownumber = row
        Me.searchPhrase = search
    End Sub
End Class