Imports Microsoft.Reporting.WinForms
Imports System.IO
Imports System.Drawing.Printing



Public Class frmIntegratedWorkflow

    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)
    Private plannerName As String
    Private buyerName As String
    Private organizationID As Integer
    Private frmWorkbenchProjects As frmWorkbenchProjects

    Private timer As New Timer              'To reload the live ticker

   



    Public Sub New(ByVal userName As String, ByVal organizationId As Integer, ByRef frmAutobahn As frmWorkbenchProjects)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.organizationID = organizationId
        Me.frmWorkbenchProjects = frmAutobahn
        Dim dsName As DataSet = db.SecureQueryParams("SELECT [organizationName] FROM [whOrganizationDefinition] WHERE [organizationId] = @1", Me.organizationID)

        Try
            Me.Text += " " & dsName.Tables(0).Rows(0).Item(0)

            Dim ds As DataSet = db.SecureQueryParams("SELECT firstName, lastName, plannerName, buyerName FROM warehouse_users WHERE user_Name = @1", userName)
            plannerName = ds.Tables(0).Rows(0).Item("plannerName").ToString
            buyerName = ds.Tables(0).Rows(0).Item("buyerName").ToString
            Me.Text += " " & ds.Tables(0).Rows(0).Item("firstName").ToString & " " & ds.Tables(0).Rows(0).Item("lastName").ToString

            ' Add any initialization after the InitializeComponent() call.
        Catch ex As Exception

        End Try
    End Sub

    Private Sub frmIntegratedWorkflow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Hide the tabs not ready yet
        'Me.tcWorkbenches.TabPages.Remove(Me.tpOrderManagement)
        Me.loadButtonInformationSO()

        Me.tcWorkbenches.TabPages.Remove(Me.tpScheduling)

        Dim roles As New clsUserControl
        If roles.userInRole(Environment.UserName, "Purchasing Manager") Then
            Dim ds As DataSet = db.SecureQueryParams("SELECT  [user_Name] " & _
                                                      " ,[standard_organization_id] " & _
                                                      " ,[userDefaultOrganization] " & _
                                                      " ,[roleName] " & _
                                                      " ,[roleDescription]" & _
                                                      " ,[rolePurpose] " & _
                                                      " ,[roleId] " & _
                                                      " ,[lastName] + ', ' + [firstName] name " & _
                                                      "  ,[plannerName] " & _
                                                      " ,[buyerName] " & _
                                                      "      FROM [FSG_IND_WORKBENCH].[dbo].[userInRolesView] " & _
                                                  " WHERE roleName = 'Purchasing IWI user' and userDefaultOrganization = @1 ORDER BY [lastName], [firstName]", Me.organizationID)

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "name", "user_name", "Please select the buyer you want to see.")
            dlg.ShowDialog()
            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim ds2 As DataSet = db.SecureQueryParams("SELECT firstName, lastName, plannerName, buyerName FROM warehouse_users WHERE user_Name = @1", dlg.propValueMember)
                plannerName = ds2.Tables(0).Rows(0).Item("plannerName").ToString
                buyerName = ds2.Tables(0).Rows(0).Item("buyerName").ToString
                ' Me.Text = "Integrated Workbench Integration - " & ds2.Tables(0).Rows(0).Item("lastName").ToString & ", " & ds2.Tables(0).Rows(0).Item("firstName").ToString
            End If
        End If

        Me.loadButtonInformationPO()
        'Load the live ticker
        Dim comments As New clsComments(Me.rtbLiveTicker)
        comments.loadLatestNotReviewedNotifications(Me.organizationID)

        timer.Interval = 10800
        AddHandler timer.Tick, AddressOf loadTicker

        Me.loadBacklogReport()
        Me.rvBacklog.RefreshReport()
        Me.loadRevenueReport()
        Me.rvRevenuePlanning.RefreshReport()

        'Load the daily bookings and billings user control
        Dim uc As New ucOnTimePerformance(Me.organizationID)
        Me.tcWorkbenches.TabPages("tpBookAndBill").Controls.Add(uc)
        uc.Dock = DockStyle.Fill

        Dim uc2 As New ucOTP(Me.organizationID)
        Me.tcWorkbenches.TabPages("tpOTP").Controls.Add(uc2)
        uc2.Dock = DockStyle.Fill
        uc2.chartOTP(uc.charControlOTP)

        If Me.organizationID <> 255 Then
            Me.tcWorkbenches.TabPages.Remove(Me.tpBacklog)
            Me.tcWorkbenches.TabPages.Remove(Me.tpBookAndBill)
            Me.tcWorkbenches.TabPages.Remove(Me.tpOTP)
            Me.tcWorkbenches.TabPages.Remove(Me.tpRevenue)
        End If

        'Me.rvRevenuePlanning.RefreshReport()
        'Me.rvBacklog.RefreshReport
        'Me.rvRevenuePlanning.RefreshReport
        Me.rvBacklog.RefreshReport()
        Me.rvRevenuePlanning.RefreshReport()
    End Sub

    Private Sub loadBacklogReport()
        ' Add any initialization after the InitializeComponent() call.
        'Reset report and change to local processing mode
        Me.rvBacklog.Visible = True
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
        rvBacklog.Reset()
        rvBacklog.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        '_dsReportSource = dsReportData
        'Change the report source to local processing
        Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvBacklog.LocalReport
        rep.Refresh()
        rep.ReportPath = "ProjectManagerWorkbench\Reporting\repBacklogOverview.rdlc"
        rds.Name = "dsWork_tblWork"

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.Query("SELECT TOP 100 * FROM tblWork")

        'set to delegated data source
        rds.Value = ds.Tables(0)
        rep.DataSources.Add(rds)

        'Go for the backlog pieces

        Me.createBacklogData()

        Me.rvBacklog.Refresh()

        'rep.Refresh()
    End Sub
    Private Sub createBacklogData()
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Try
            Dim totOrdern As Double = 0
            Dim totQty As Double = 0
            Dim totSumme As Double = 0

            Dim totLateOrdern As Double = 0
            Dim totLateQty As Double = 0
            Dim totLateSumme As Double = 0

            Dim totIntercoOrdern As Double = 0
            Dim totIntercoQty As Double = 0
            Dim totIntercoSumme As Double = 0

            Dim totIntercoLateOrdern As Double = 0
            Dim totIntercoLateQty As Double = 0
            Dim totIntercoLateSumme As Double = 0

            db.Connect()

            'Get the pleuger filter
            Dim ds As DataSet = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Pleuger", 255)
            'With this information go into the backlog and grab the relevant information
            Dim dsBak As DataSet = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("pleugerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("pleugerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("pleugerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Backlog Pleuger
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < dateadd(day, datediff(day, 0, getdate()),0)  " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("latePleugerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("latePleugerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("latePleugerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Pleuger Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoPleugerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoPleugerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoPleugerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Pleuger Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice   FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < dateadd(day, datediff(day, 0, getdate()),0)  " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLatePleugerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLatePleugerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLatePleugerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '#### Byron Jackson  ######
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Byron Jackson", 255)

            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice   FROM salesOrderLinesView WHERE organization_id = @1 " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("bjQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("bjOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("bjSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Backlog Byron Jackson
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateBJQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("lateBJOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("lateBJSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Byron Jackson Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice   FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoBJQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoBJOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoBJSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Byron Jackson Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateBJQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLateBJOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLateBJSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With


            '##### Recips #####
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Recips u. Pack", 255)

            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("recipsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("recipsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("recipsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Recips
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateRecipsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("lateRecipsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("lateRecipsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Recips Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoRecipsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoRecipsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoRecipsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'BacklogRecips Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateRecipsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLateRecipsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLateRecipsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '##### Packaging #####

            '##### Decoker ######
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Other Decoker", 255)

            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("decokerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("decokerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("decokerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Decoker
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateDecokerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("lateDecokerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("lateDecokerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Decoker Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoDecokerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoDecokerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoDecokerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Decoker Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateDecokerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLateDecokerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLateDecokerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '##### Thruster #####
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Thruster", 255)

            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("thrusterQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("thrusterOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("thrusterSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Thruster
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateThrusterQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("lateThrusterOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("lateThrusterSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Thruster Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoThrusterQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoThrusterOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoThrusterSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Thruster Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateThrusterQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLateThrusterOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLateThrusterSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '##### Geothermie #####
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Geothermie", 255)

            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("geothermieQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("geothermieOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("geothermieSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Geothermie
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateGeothermieQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("lateGeothermieOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("lateGeothermieSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Geothermie Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoGeothermieQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoGeothermieOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoGeothermieSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Geothermie Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateGeothermieQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLateGeothermieOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLateGeothermieSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With


            '##### Units/Buyouts - Total

            '#### Units / Buyouts - Total
            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("unitsTotalQty", totQty.ToString)
                parameters(1) = New ReportParameter("unitsTotalOrdern", totOrdern.ToString)
                parameters(2) = New ReportParameter("unitsTotalSumme", FormatCurrency(totSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '#### Units / Buyouts - Total - Late
            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateUnitsQty", totLateQty.ToString)
                parameters(1) = New ReportParameter("lateUnitsOrdern", totLateOrdern.ToString)
                parameters(2) = New ReportParameter("lateUnitsSumme", FormatCurrency(totLateSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Calculate the percentage value late
            Dim lateP As Double = 0
            Try
                lateP = totLateSumme / totSumme
                With Me.rvBacklog.LocalReport
                    Dim parameters(0) As ReportParameter
                    parameters(0) = New ReportParameter("latePercentageUnits", FormatPercent(lateP, 2, TriState.True, TriState.True, TriState.True))
                    .SetParameters(parameters)
                End With

            Catch ex As Exception

            End Try

            '#### Units / Buyouts - Interco
            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoUnitsQty", totIntercoQty.ToString)
                parameters(1) = New ReportParameter("intercoUnitsOrdern", totIntercoOrdern.ToString)
                parameters(2) = New ReportParameter("intercoUnitsSumme", FormatCurrency(totIntercoSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '#### Units / Buyouts - Interco - Late
            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateUnitsQty", totIntercoLateQty.ToString)
                parameters(1) = New ReportParameter("intercoLateUnitsOrdern", totIntercoLateOrdern.ToString)
                parameters(2) = New ReportParameter("intercoLateUnitsSumme", FormatCurrency(totIntercoLateSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With


            '##### Repair - Total #####
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Repair", 255)

            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("repairTotalQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("repairTotalOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("repairTotalSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Repair
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateRepairQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("lateRepairOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("lateRepairSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Repair Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoRepairQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoRepairOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoRepairSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Repair Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateRepairQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLateRepairOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLateRepairSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With


            '##### Parts #####
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Parts - Alle Aufträge", 255)

            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("partsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("partsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("partsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Parts
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("latePartsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("latePartsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("latePartsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Parts Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoPartsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoPartsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoPartsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Parts Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < dateadd(day, datediff(day, 0, getdate()),0) " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLatePartsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLatePartsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLatePartsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '##### Total
            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("totalQty", totQty.ToString)
                parameters(1) = New ReportParameter("totalOrdern", totOrdern.ToString)
                parameters(2) = New ReportParameter("totalSumme", FormatCurrency(totSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With


            '#### Total - Late
            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateTotalQty", totLateQty.ToString)
                parameters(1) = New ReportParameter("lateTotalOrdern", totLateOrdern.ToString)
                parameters(2) = New ReportParameter("lateTotalSumme", FormatCurrency(totLateSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            Try
                lateP = totLateSumme / totSumme
                With Me.rvBacklog.LocalReport
                    Dim parameters(0) As ReportParameter
                    parameters(0) = New ReportParameter("latePercentageTotal", FormatPercent(lateP, 2, TriState.True, TriState.True, TriState.True))
                    .SetParameters(parameters)
                End With

            Catch ex As Exception

            End Try

            '#### Total - Interco
            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoTotalQty", totIntercoQty.ToString)
                parameters(1) = New ReportParameter("intercoTotalOrdern", totIntercoOrdern.ToString)
                parameters(2) = New ReportParameter("intercoTotalSumme", FormatCurrency(totIntercoSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '#### Total - Interco - Late 
            With Me.rvBacklog.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateTotalQty", totIntercoLateQty.ToString)
                parameters(1) = New ReportParameter("intercoLateTotalOrdern", totIntercoLateOrdern.ToString)
                parameters(2) = New ReportParameter("intercoLateTotalSumme", FormatCurrency(totIntercoLateSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

        Catch ex As Exception
        Finally
            db.Disconnect()

        End Try

    End Sub



    Private Sub loadTicker()

        rtbLiveTicker.Clear()
        Dim comments As New clsComments(Me.rtbLiveTicker)
        comments.loadLatestNotReviewedNotifications(Me.organizationID)
    End Sub

    Private Sub loadButtonInformationPO()
        Dim ds As New DataSet
        Try
            db.Connect()

            'Currently get the number of po lines
            ds = db.SecureQueryParams("SELECT COUNT(*) FROM poQueueView WHERE buyer = @1 AND ship_to_organization_id = @2", Me.buyerName, Me.organizationID)
            btnNoOfOpenPOs.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString

            'Volumen on po lines
            ds = db.SecureQueryParams("SELECT sum(unit_price*quantity_ordered) FROM poQueueView WHERE buyer = @1 AND SHIP_TO_ORGANIZATION_ID = @2 ", Me.buyerName, Me.organizationID)
            btnTotalValuePo.Text += " " + FormatCurrency(ds.Tables(0).Rows(0).Item(0).ToString, 2, TriState.True, TriState.True, TriState.True)

            'Distinct vendors
            ds = db.SecureQueryParams("SELECT COUNT(DISTINCT(vendor_id)) FROM poQueueView WHERE buyer = @1 AND ship_to_organization_id = @2", Me.buyerName, Me.organizationID)
            btnUsedSuppliers.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString

            'yesterdays po lines
            ds = db.SecureQueryParams("SELECT COUNT(*) FROM poQueueView WHERE buyer = @1 AND ship_to_organization_id = @2 AND CREATION_DATE_POL > dateadd(day,-1,getDate()) ", Me.buyerName, Me.organizationID)
            btnNumberPOLinesCreated.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString

            'current supply problems        'Possible to select from the view now
            ds = db.SecureQueryParams("SELECT a.reason_code, a.jobpoLine, a.make_buy, a.item_no, a.planner_code, a.organization_id, a.purchase_order_id, a.purch_line_no, a.purchasingReviewDate " & _
                                    " FROM mrpShortagesView a WHERE a.reason_code IN ('RC1 - Planned Compressed Order','RC2 - Missing Promise Date','RC3 - Late Promise Date','RC4 - Receiving Required')  AND a.make_buy = 'B' AND a.organization_id = @3 AND " & _
                                    " (a.buyer = @1 " & _
                                    " OR a.planner_code = @2) AND a.short is NOT NULL " & _
                                    " GROUP BY a.reason_code, a.jobpoLine, a.make_buy, a.item_no, a.planner_code, a.organization_id, a.purchase_order_id, a.purch_line_no,a.purchasingReviewDate ", Me.buyerName, Me.plannerName, Me.organizationID)

            btnCurrentSupplyProblems.Text += " " + ds.Tables(0).Rows.Count.ToString

            'Reason code 1
            Dim rc1 As Integer = 0
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i).Item("reason_code").ToString.Contains("RC1") Then
                    rc1 += 1
                End If
            Next

            btnRC1Count.Text = rc1.ToString
            Dim lastDate As Date = Now.AddDays(-800)
            'Reason code 2
            Dim rc2 As Integer = 0
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i).Item("reason_code").ToString.Contains("RC2") Then
                    rc2 += 1
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("purchasingReviewDate")) Then
                        If ds.Tables(0).Rows(i).Item("purchasingReviewDate") > lastDate Then lastDate = ds.Tables(0).Rows(i).Item("purchasingReviewDate")
                    End If
                End If
            Next
            'Post the last date to the textbox
            If lastDate > Now.AddDays(-799) Then
                txtRC2.Text = FormatDateTime(lastDate, DateFormat.ShortDate)
            End If

            btnRC2Count.Text = rc2.ToString
            lastDate = Now.AddDays(-800)
            'Reason code 3
            Dim rc3 As Integer = 0
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i).Item("reason_code").ToString.Contains("RC3") Then
                    rc3 += 1
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("purchasingReviewDate")) Then
                        If ds.Tables(0).Rows(i).Item("purchasingReviewDate") > lastDate Then lastDate = ds.Tables(0).Rows(i).Item("purchasingReviewDate")
                    End If
                End If
            Next

            If lastDate > Now.AddDays(-799) Then
                txtRC3.Text = FormatDateTime(lastDate, DateFormat.ShortDate)
            End If


            btnRC3Count.Text = rc3.ToString
            lastDate = Now.AddDays(-800)

            Dim rc4 As Integer = 0
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i).Item("reason_code").ToString.Contains("RC4") Then
                    rc4 += 1
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("purchasingReviewDate")) Then
                        If ds.Tables(0).Rows(i).Item("purchasingReviewDate") > lastDate Then lastDate = ds.Tables(0).Rows(i).Item("purchasingReviewDate")
                    End If
                End If
            Next

            If lastDate > Now.AddDays(-799) Then
                txtRC4.Text = FormatDateTime(lastDate, DateFormat.ShortDate)
            End If

            btnRC4Count.Text = rc4.ToString

        Catch ex As Exception
        Finally
            db.Disconnect()

        End Try



    End Sub

    Private Sub btnReasonCode1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode1.Click
        txtLastReviewRC1.Text = FormatDateTime(Now, DateFormat.ShortDate)

        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC1 - Planned Compressed Order' ", "")

        'db.SecureNonQueryParams("INSERT INTO comments (entity, organizationID, description, creation_date, created_by) VALUES (@1,@2,@3,@4,@5 )", "poQueueViewRC1Review", _
        '                        Me.organizationID, "Reviewed", Format(Now, "yyyyMMdd HH:mm:ss"), Environment.UserName)
        'Run the filter

    End Sub

    Private Sub btnNoOfOpenPOs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoOfOpenPOs.Click
        frmWorkbenchProjects.loadFilterOnPurchaseOrders("My Purchase Orders", " AND buyer ='" & Me.buyerName & "'", "")
    End Sub


    Private Sub btnTotalValuePo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTotalValuePo.Click
        frmWorkbenchProjects.loadFilterOnPurchaseOrders("My Purchase Orders", " AND buyer ='" & Me.buyerName & "'", " QUANTITY_ORDERED * UNIT_PRICE DESC")
    End Sub

    Private Sub btnCurrentSupplyProblems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentSupplyProblems.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code IN ('RC1 - Planned Compressed Order','RC2 - Missing Promise Date','RC3 - Late Promise Date','RC4 - Receiving Required') ", "")

    End Sub

    Private Sub btnUsedSuppliers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUsedSuppliers.Click
        frmWorkbenchProjects.loadFilterOnPurchaseOrders("My Purchase Orders", " AND buyer ='" & Me.buyerName & "'", "")
        Dim ds As DataSet = db.SecureQueryParams("SELECT COUNT(*) [No of Lines], vendor_name [Vendor], SUM(unit_price*quantity_ordered) [Value Per Vendor] FROM poQueueView WHERE ship_to_organization_id = @1 AND buyer ='" & Me.buyerName & _
                                                 "' GROUP BY vendor_name ORDER BY COUNT(*) DESC", Me.organizationID)
        dgvAggregation.DataSource = ds.Tables(0)

    End Sub

    Private Sub btnNumberPOLinesCreated_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNumberPOLinesCreated.Click
        frmWorkbenchProjects.loadFilterOnPurchaseOrders("My Purchase Orders Created Yesterday", " AND buyer ='" & Me.buyerName & "' AND CREATION_DATE_POL > '" & Format(Now.Date.AddDays(-1), "yyyyMMdd HH:mm:ss") & "'", "")
    End Sub

    Private Sub btnReasonCode2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode2.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems: RC2 - Missing Promise Date", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC2 - Missing Promise Date' ", "")
    End Sub

    Private Sub btnReasonCode3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode3.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems: RC3 - Late Promise Date", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC3 - Late Promise Date' ", "")
    End Sub

    Private Sub btnReasonCode4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReasonCode4.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems: RC4 - Receiving Required", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC4 - Receiving Required' ", "")
    End Sub

    Private Sub btnRC1Count_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC1Count.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems: RC1 - Planned Compressed Order", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC1 - Planned Compressed Order' ", "")
    End Sub

    Private Sub btnRC2Count_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC2Count.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems: RC2 - Missing Promise Date", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC2 - Missing Promise Date' ", "")
    End Sub

    Private Sub btnRC3Count_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC3Count.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems: RC3 - Late Promise Date", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                           " AND reason_code = 'RC3 - Late Promise Date' ", "")
    End Sub

    Private Sub btnRC4Count_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC4Count.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems: RC4 - Receiving Required", " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC4 - Receiving Required' ", "")
    End Sub


    Private Sub btnRC1Order_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC1Order.Click
        Try



            Dim ds As DataSet = db.SecureQueryParams("SELECT a.OrderNoLine, COUNT(*) as numberOf, a.end_demand_id, a.organization_id, (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) as value  FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                       " AND a.reason_code = 'RC1 - Planned Compressed Order' AND a.short is NOT NULL GROUP BY OrderNoLine, a.end_demand_id, a.organization_id")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND OrderNoLine = '" & ds.Tables(0).Rows(i).Item("OrderNoLine") & "'"
                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString & " Sales Value: " & FormatCurrency(ds.Tables(0).Rows(i).Item("value").ToString, 2, TriState.True, TriState.True, TriState.True)
                Catch ex As Exception

                End Try


            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC1 - Planned compressed orders for Order line", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC1 - Planned Compressed Order' ", "")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC2Order_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC2Order.Click
        Dim ds As DataSet = db.SecureQueryParams("SELECT a.OrderNoLine, COUNT(*) as numberOf, a.end_demand_id, a.organization_id, (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) as value  FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                   " AND a.reason_code = 'RC2 - Missing Promise Date' AND a.short is NOT NULL AND a.OrderNoLine IS NOT NULL GROUP BY OrderNoLine, a.end_demand_id, a.organization_id " & _
                                                   " ORDER BY (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) DESC")

        Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
        ds.Tables(0).Columns.Add(dc)
        Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
        ds.Tables(0).Columns.Add(dc2)

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Try


                'Create the filter phrase
                ds.Tables(0).Rows(i).Item("filter") = "AND OrderNoLine = '" & ds.Tables(0).Rows(i).Item("OrderNoLine") & "'"
                'Create the thing to display
                ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString & " Sales Value: " & FormatCurrency(ds.Tables(0).Rows(i).Item("value"), 2, TriState.True, TriState.True, TriState.True)
            Catch ex As Exception

            End Try

        Next

        Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
        dlg.ShowDialog()

        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim strFilterPhrase As String = dlg.propValueMember
            frmWorkbenchProjects.loadFilterOnShortages("RC2 - Missing Promise Date", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC2 - Missing Promise Date' ", "")
        End If

    End Sub

    Private Sub btnRC3Order_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC3Order.Click
        Dim ds As DataSet = db.SecureQueryParams("SELECT a.OrderNoLine, COUNT(*) as numberOf, a.end_demand_id, a.organization_id, (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) as value  FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                  " AND a.reason_code = 'RC3 - Late Promise Date' AND a.short is NOT NULL AND a.OrderNoLine IS NOT NULL GROUP BY OrderNoLine, a.end_demand_id, a.organization_id " & _
                                                  " ORDER BY (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) DESC")

        Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
        ds.Tables(0).Columns.Add(dc)
        Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
        ds.Tables(0).Columns.Add(dc2)

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            Try


                'Create the filter phrase
                ds.Tables(0).Rows(i).Item("filter") = "AND OrderNoLine = '" & ds.Tables(0).Rows(i).Item("OrderNoLine") & "'"
                'Create the thing to display
                If DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("value")) Then
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Else
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString & " Sales Value: " & FormatCurrency(ds.Tables(0).Rows(i).Item("value").ToString, 2, TriState.True, TriState.True, TriState.True)
                End If
            Catch ex As Exception

            End Try


        Next

        Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
        dlg.ShowDialog()

        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim strFilterPhrase As String = dlg.propValueMember
            frmWorkbenchProjects.loadFilterOnShortages("RC3 - Late Promise Date ", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC3 - Late Promise Date' ", "")
        End If
    End Sub

    Private Sub btnRC4Order_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC4Order.Click
        Try


            Dim ds As DataSet = db.SecureQueryParams("SELECT a.OrderNoLine, COUNT(*) as numberOf, a.end_demand_id, a.organization_id, (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) as value  FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                      " AND a.reason_code = 'RC4 - Receiving Required' AND a.short is NOT NULL GROUP BY OrderNoLine, a.end_demand_id, a.organization_id" & _
                                                      " ORDER BY (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) DESC ")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND OrderNoLine = '" & ds.Tables(0).Rows(i).Item("OrderNoLine") & "'"
                    'Create the thing to display
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("value")) Then
                        ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString & " Sales Value: " & FormatCurrency(ds.Tables(0).Rows(i).Item("value").ToString, 2, TriState.True, TriState.True, TriState.True)
                    Else
                        ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString
                    End If

                Catch ex As Exception

                End Try

            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC4 - Receiving Required ", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC4 - Receiving Required' ", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC2Supplier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC2Supplier.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT  a.OP1, COUNT(*) as numberOf,  a.organization_id, MAX(purchasingReviewDate) lastReviewDate FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                     " AND a.reason_code = 'RC2 - Missing Promise Date' AND a.short is NOT NULL AND a.OrderNoLine IS NOT NULL GROUP BY a.OP1,  a.organization_id ORDER BY count(*) DESC")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND OP1 = '" & ds.Tables(0).Rows(i).Item("OP1") & "'"
                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OP1").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try
            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC2 - Missing Promise Date", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC2 - Missing Promise Date' ", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC3Supplier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC3Supplier.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT  a.OP1, COUNT(*) as numberOf,  a.organization_id FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                     " AND a.reason_code = 'RC3 - Late Promise Date' AND a.short is NOT NULL AND a.OrderNoLine IS NOT NULL GROUP BY a.OP1,  a.organization_id ORDER BY count(*) DESC")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND OP1 = '" & ds.Tables(0).Rows(i).Item("OP1") & "'"
                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OP1").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try
            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC2 - Missing Promise Date", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC3 - Late Promise Date' ", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC4Supplier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC4Supplier.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT  a.OP1, COUNT(*) as numberOf,  a.organization_id FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                     " AND a.reason_code = 'RC4 - Receiving Required' AND a.short is NOT NULL AND a.OrderNoLine IS NOT NULL GROUP BY a.OP1,  a.organization_id ORDER BY count(*) DESC")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND OP1 = '" & ds.Tables(0).Rows(i).Item("OP1") & "'"
                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OP1").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try
            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC2 - Missing Promise Date", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC4 - Receiving Required' ", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC1Item_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC1Item.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT  a.item_no, COUNT(*) as numberOf,  a.organization_id, a.description FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                     " AND a.reason_code = 'RC1 - Planned Compressed Order' AND a.short is NOT NULL  GROUP BY a.item_no,  a.organization_id, a.description ORDER BY count(*) DESC")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND item_no = '" & ds.Tables(0).Rows(i).Item("item_no") & "'"
                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("item_no").ToString & " - " & ds.Tables(0).Rows(i).Item("description").ToString & " : " & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try
            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC2 - Missing Promise Date", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC1 - Planned Compressed Order' ", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC2Item_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC2Item.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT  a.item_no, COUNT(*) as numberOf,  a.organization_id, a.description FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                     " AND a.reason_code = 'RC2 - Missing Promise Date' AND a.short is NOT NULL  GROUP BY a.item_no,  a.organization_id, a.description ORDER BY count(*) DESC")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND item_no = '" & ds.Tables(0).Rows(i).Item("item_no") & "'"
                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("item_no").ToString & " - " & ds.Tables(0).Rows(i).Item("description").ToString & " : " & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try
            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC2 - Missing Promise Date", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC2 - Missing Promise Date' ", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC3Item_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC3Item.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT  a.item_no, COUNT(*) as numberOf,  a.organization_id, a.description FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                     " AND a.reason_code = 'RC3 - Late Promise Date' AND a.short is NOT NULL  GROUP BY a.item_no,  a.organization_id, a.description ORDER BY count(*) DESC")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND item_no = '" & ds.Tables(0).Rows(i).Item("item_no") & "'"
                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("item_no").ToString & " - " & ds.Tables(0).Rows(i).Item("description").ToString & " : " & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try
            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC2 - Missing Promise Date", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC3 - Late Promise Date' ", "")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC4Item_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC4Item.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT  a.item_no, COUNT(*) as numberOf,  a.organization_id, a.description FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                     " AND a.reason_code = 'RC4 - Receiving Required' AND a.short is NOT NULL  GROUP BY a.item_no,  a.organization_id , a.description ORDER BY count(*) DESC")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try


                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND item_no = '" & ds.Tables(0).Rows(i).Item("item_no") & "'"
                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("item_no").ToString & " - " & ds.Tables(0).Rows(i).Item("description").ToString & " : " & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try
            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC4 - Receiving Required", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC4 - Receiving Required' ", "")
            End If
        Catch ex As Exception

        End Try
    End Sub


    'Handling the schedule ship date button
    Private Sub btnRC1Supplier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC1Supplier.Click
        Dim ds As New DataSet
        Dim dt As New DataTable
        Dim dc1 As New DataColumn("filter", System.Type.GetType("System.String"))
        Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))

        ds.Tables.Add(dt)

        dt.Columns.Add(dc1)
        dt.Columns.Add(dc2)

        Dim dr As DataRow = ds.Tables(0).NewRow
        dr.Item("display") = "Schedule ship date less than today"
        dr.Item("filter") = "AND (SELECT max(schedule_ship_date) FROM salesOrderLinesView b WHERE b.demand_id = end_demand_id AND b.organization_id = organization_id) < getDate()"

        dt.Rows.Add(dr)

        dr = ds.Tables(0).NewRow
        dr.Item("display") = " Schedule ship within the next 31 days "
        dr.Item("filter") = " AND (SELECT max(schedule_ship_date) FROM salesOrderLinesView b WHERE b.demand_id = end_demand_id AND b.organization_id = organization_id) < dateadd(day, 31, getDate()) "

        dt.Rows.Add(dr)

        Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems by schedule ship date.")
        dlg.ShowDialog()

        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Dim strFilterPhrase As String = dlg.propValueMember
            frmWorkbenchProjects.loadFilterOnShortages("RC1 - Planned compressed orders for Order lines", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                   " AND reason_code = 'RC1 - Planned Compressed Order' ", "")

        End If


    End Sub

    Private Sub btnRC1Severity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC1Severity.Click
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT a.OrderNoLine, COUNT(*) as numberOf, a.end_demand_id, a.organization_id, a.delay, " & _
                                                     " (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) as value " & _
                                                     " FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                       " AND a.reason_code = 'RC1 - Planned Compressed Order' AND a.short is NOT NULL GROUP BY OrderNoLine, a.end_demand_id, a.organization_id, a.delay")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Create the filter phrase
                ds.Tables(0).Rows(i).Item("filter") = "AND OrderNoLine = '" & ds.Tables(0).Rows(i).Item("OrderNoLine") & "'"
                'Calculate the severity
                Try
                    ds.Tables(0).Rows(i).Item("severity") = ds.Tables(0).Rows(i).Item("value") * ds.Tables(0).Rows(i).Item("delay")
                    ds.Tables(0).DefaultView.Sort = "severity DESC"

                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString & " Sales Value: " & FormatCurrency(ds.Tables(0).Rows(i).Item("value").ToString, 2, TriState.True, TriState.True, TriState.True)
                Catch ex As Exception

                End Try


            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC1 - Planned compressed orders for Order line", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC1 - Planned Compressed Order' ", "")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC2Severity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC2Severity.Click
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT a.OrderNoLine, COUNT(*) as numberOf, a.end_demand_id, a.organization_id, a.delay, " & _
                                                     " (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) as value " & _
                                                     " FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                       " AND a.reason_code = 'RC2 - Missing Promise Date' AND a.short is NOT NULL GROUP BY OrderNoLine, a.end_demand_id, a.organization_id, a.delay")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try

                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND OrderNoLine = '" & ds.Tables(0).Rows(i).Item("OrderNoLine") & "'"
                    'Calculate the severity
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("delay")) Then
                        ds.Tables(0).Rows(i).Item("severity") = Double.Parse(ds.Tables(0).Rows(i).Item("value")) * Double.Parse(ds.Tables(0).Rows(i).Item("delay"))

                    Else
                        ds.Tables(0).Rows(i).Item("severity") = Double.Parse(ds.Tables(0).Rows(i).Item("value"))

                    End If


                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString & " Sales Value: " & FormatCurrency(ds.Tables(0).Rows(i).Item("value").ToString, 2, TriState.True, TriState.True, TriState.True)

                Catch ex As Exception

                End Try

            Next
            ds.Tables(0).DefaultView.Sort = "severity DESC"
            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC2 - Missing Promise Date", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC2 - Missing Promise Date' ", "")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC3Severity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC3Severity.Click
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT a.OrderNoLine, COUNT(*) as numberOf, a.end_demand_id, a.organization_id, a.delay, " & _
                                                     " (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) as value " & _
                                                     " FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                       " AND a.reason_code = 'RC3 - Late Promise Date' AND a.short is NOT NULL GROUP BY OrderNoLine, a.end_demand_id, a.organization_id, a.delay")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try

                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND OrderNoLine = '" & ds.Tables(0).Rows(i).Item("OrderNoLine") & "'"
                    'Calculate the severity

                    ds.Tables(0).Rows(i).Item("severity") = ds.Tables(0).Rows(i).Item("value") * ds.Tables(0).Rows(i).Item("delay")
                    ds.Tables(0).DefaultView.Sort = "severity DESC"

                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString & " Sales Value: " & FormatCurrency(ds.Tables(0).Rows(i).Item("value").ToString, 2, TriState.True, TriState.True, TriState.True)

                Catch ex As Exception

                End Try

            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC1 - Planned compressed orders for Order line", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC3 - Late Promise Date' ", "")

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRC4Severity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRC4Severity.Click
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT a.OrderNoLine, COUNT(*) as numberOf, a.end_demand_id, a.organization_id, a.delay, " & _
                                                     " (SELECT SUM(extended_price) FROM salesOrderLinesView b WHERE b.demand_id = a.end_demand_id AND b.organization_id = a.organization_id) as value " & _
                                                     " FROM mrpShortagesView a WHERE  (a.buyer ='" & Me.buyerName & "' OR a.planner_code = '" & Me.plannerName & "') " & _
                                                       " AND a.reason_code = 'RC4 - Receiving Required' AND a.short is NOT NULL GROUP BY OrderNoLine, a.end_demand_id, a.organization_id, a.delay")

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Try

                    'Create the filter phrase
                    ds.Tables(0).Rows(i).Item("filter") = "AND OrderNoLine = '" & ds.Tables(0).Rows(i).Item("OrderNoLine") & "'"
                    'Calculate the severity

                    ds.Tables(0).Rows(i).Item("severity") = ds.Tables(0).Rows(i).Item("value") * ds.Tables(0).Rows(i).Item("delay")
                    ds.Tables(0).DefaultView.Sort = "severity DESC"

                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("OrderNoLine").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString & " Sales Value: " & FormatCurrency(ds.Tables(0).Rows(i).Item("value").ToString, 2, TriState.True, TriState.True, TriState.True)

                Catch ex As Exception

                End Try

            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order _ Line : Number of RC1 problems for this line.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnShortages("RC1 - Planned compressed orders for Order line", strFilterPhrase & " AND (buyer ='" & Me.buyerName & "' OR planner_code = '" & Me.plannerName & "') " & _
                                                       " AND reason_code = 'RC4 - Receiving Required' ", "")

            End If
        Catch ex As Exception

        End Try
    End Sub


    Private Sub btnDiscussionTracking_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDiscussionTracking.Click
        Dim frm As New frmDiscussionTracking(Me.organizationID, Me.frmWorkbenchProjects)
        frm.Show()

    End Sub



    'Implementation of the open sales order lines
    Private Sub loadButtonInformationSO()
        Dim ds As New DataSet
        Try
            db.Connect()
            ds = db.SecureQueryParams("SELECT COUNT(*) FROM salesOrderLinesView WHERE organization_id = @1", Me.organizationID)
            btnSoNumberLines.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString

            ds = db.SecureQueryParams("SELECT SUM(extended_price) FROM salesOrderLinesView WHERE organization_id = @1", Me.organizationID)
            btnSOValueLines.Text += " " + FormatCurrency(ds.Tables(0).Rows(0).Item(0), 0, TriState.True, TriState.True, TriState.True)

            ds = db.SecureQueryParams("SELECT COUNT(*) FROM salesOrderLinesView WHERE organization_id = @1 AND creation_date_coline > dateadd(day, -30, getDate())", Me.organizationID)
            btnSONewLines.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString

            '***missing number of shipped sales order lines


            ds = db.SecureQueryParams("SELECT  COUNT(*) noOfProblemsTotal FROM saveMRPdemand WHERE reasonCode is not NULL AND organization_id = @1", Me.organizationID)
            btnSOSupplyProblems.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString
            btnSOCountProblems.Text = ds.Tables(0).Rows(0).Item(0).ToString


            ds = db.SecureQueryParams("SELECT  COUNT(distinct(end_demand_id)) noProblemLine FROM saveMRPdemand WHERE reasonCode is not NULL AND organization_id = @1", Me.organizationID)
            btnSOLatePlannedLines.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString

            ds = db.SecureQueryParams("SELECT  COUNT(*) Lines FROM salesOrderLinesView WHERE convert(varchar,schedule_ship_date, 1) = convert(varchar, getDate(),1) AND organization_id = @1 ", Me.organizationID)
            btnSOShipToday.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString

            ds = db.SecureQueryParams("SELECT  COUNT(*) Lines FROM salesOrderLinesView WHERE schedule_ship_date IS NOT NULL AND schedule_ship_date < dateAdd(day,30,getDate()) AND organization_id = @1 ", Me.organizationID)
            btnSOShipNextDays.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString

            ds = db.SecureQueryParams("SELECT  COUNT(*) Lines FROM salesOrderLinesView WHERE schedule_ship_date IS NULL  AND organization_id = @1 ", Me.organizationID)
            btnSOwithoutSSD.Text += " " + ds.Tables(0).Rows(0).Item(0).ToString



        Catch ex As Exception
        Finally
            db.Disconnect()
        End Try
    End Sub


    Private Sub btnSoNumberLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSoNumberLines.Click
        frmWorkbenchProjects.loadFilterOnSO("Open Sales Order Lines", " ")
    End Sub

    Private Sub btnSOValueLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOValueLines.Click
        frmWorkbenchProjects.loadFilterOnSO("Open Sales Order Lines", " ")
    End Sub

    Private Sub btnSONewLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSONewLines.Click
        frmWorkbenchProjects.loadFilterOnSO("Sales Order Lines Created within the last 30 Days", " AND creation_date_coline > dateadd(day, -30, getDate()) ")
    End Sub

    Private Sub btnSOShippedLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOShippedLines.Click

    End Sub

    Private Sub btnSOSupplyProblems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOSupplyProblems.Click
        frmWorkbenchProjects.loadFilterOnShortages("Current Supply Problems", "  " & _
                                                  " AND reason_code IS NOT NULL ", "")
    End Sub

    Private Sub btnSOLatePlannedLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOLatePlannedLines.Click
        frmWorkbenchProjects.loadFilterOnSO("Sales Order Lines with Problems", " AND no_of_problems IS NOT NULL ")
    End Sub

    Private Sub btnSOShipToday_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOShipToday.Click
        frmWorkbenchProjects.loadFilterOnSO("Schedule Ship Date Today", " AND convert(varchar,schedule_ship_date, 1) = convert(varchar, getDate(),1) ")
    End Sub

    Private Sub btnSOShipNextDays_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOShipNextDays.Click
        frmWorkbenchProjects.loadFilterOnSO("Schedule Ship Date within the next 30 days", " AND schedule_ship_date IS NOT NULL AND schedule_ship_date < dateAdd(day,30,getDate()) ")
    End Sub

    Private Sub btnSOwithoutSSD_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOwithoutSSD.Click
        frmWorkbenchProjects.loadFilterOnSO("Sales Order Lines without Schedule Ship Date", " AND schedule_ship_date IS NULL ")
    End Sub

    Private Sub btnSOLinesWithoutBOMCompletion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOLinesWithoutBOMCompletion.Click

    End Sub


    Private Sub btnSOByCustomer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOByCustomer.Click
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT party_name, SUM(no_of_problems) numberOf FROM salesOrderLinesView WHERE no_of_problems IS NOT NULL AND organization_id = @1 GROUP BY party_name ORDER By sum(no_of_problems) DESC", Me.organizationID)

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Create the filter phrase
                ds.Tables(0).Rows(i).Item("filter") = "AND party_name = '" & ds.Tables(0).Rows(i).Item("party_name") & "'"

                Try

                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("party_name").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try


            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Customer : Number of problems for this customer.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnSO("Filter on specific customer", strFilterPhrase)

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSalesOrderLinesProblems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalesOrderLinesProblems.Click
        frmWorkbenchProjects.loadFilterOnSO("Sales Order Lines without Schedule Ship Date", " AND no_of_problems IS NOT NULL ")
    End Sub

    Private Sub btnSOCountProblems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOCountProblems.Click
        frmWorkbenchProjects.loadFilterOnSO("Sales Order Lines without Schedule Ship Date", " AND no_of_problems IS NOT NULL ")
    End Sub

    Private Sub btnSOByProduct_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOByProduct.Click
        Try
            Dim ds As DataSet = db.SecureQueryParams("SELECT order_type, SUM(no_of_problems) numberOf FROM salesOrderLinesView WHERE no_of_problems IS NOT NULL AND organization_id = @1 GROUP BY order_type ORDER By sum(no_of_problems) DESC", Me.organizationID)

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Create the filter phrase
                ds.Tables(0).Rows(i).Item("filter") = "AND order_type = '" & ds.Tables(0).Rows(i).Item("order_type") & "'"

                Try

                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("order_type").ToString & ":" & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try


            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order Type : Number of Problems for this Order Types.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnSO("Filter on specific customer", strFilterPhrase)

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSOByValue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOByValue.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT order_no, SUM(no_of_problems) numberOf, sum(extended_price) valueSO  FROM salesOrderLinesView WHERE no_of_problems IS NOT NULL AND organization_id = @1 GROUP BY order_no ORDER By sum(extended_price) DESC", Me.organizationID)

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Create the filter phrase
                ds.Tables(0).Rows(i).Item("filter") = "AND order_no = '" & ds.Tables(0).Rows(i).Item("order_no") & "'"

                Try

                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("order_no").ToString & " - " & FormatCurrency(ds.Tables(0).Rows(i).Item("valueSO"), 0, TriState.True, TriState.True, TriState.True) & " : " & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try


            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order Number - Value of this Order : Number of Problems for this Order Types.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnSO("Filter on specific order number.", strFilterPhrase)

            End If
        Catch ex As Exception

        End Try




    End Sub

    Private Sub btnSOByOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOByOrder.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT order_no, SUM(no_of_problems) numberOf, sum(extended_price) valueSO  FROM salesOrderLinesView WHERE no_of_problems IS NOT NULL AND organization_id = @1 GROUP BY order_no ORDER By order_no", Me.organizationID)

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Create the filter phrase
                ds.Tables(0).Rows(i).Item("filter") = "AND order_no = '" & ds.Tables(0).Rows(i).Item("order_no") & "'"

                Try

                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("order_no").ToString & " - " & FormatCurrency(ds.Tables(0).Rows(i).Item("valueSO"), 0, TriState.True, TriState.True, TriState.True) & " : " & ds.Tables(0).Rows(i).Item("numberOf").ToString
                Catch ex As Exception

                End Try


            Next

            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order Number - Value of this Order : Number of Problems for this Order Types.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnSO("Filter on specific order number.", strFilterPhrase)

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSOByDateRange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOByDateRange.Click

        Try
            Dim ds As New DataSet
            Dim dt As New DataTable
            Dim dc1 As New DataColumn("filter", System.Type.GetType("System.String"))
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))

            ds.Tables.Add(dt)

            dt.Columns.Add(dc1)
            dt.Columns.Add(dc2)

            Dim dr As DataRow = ds.Tables(0).NewRow
            dr.Item("display") = "Schedule ship date less than today"
            dr.Item("filter") = "AND schedule_ship_date < getDate() AND no_of_problems IS NOT NULL"

            dt.Rows.Add(dr)

            dr = ds.Tables(0).NewRow
            dr.Item("display") = " Schedule ship within the next 31 days "
            dr.Item("filter") = " AND schedule_ship_date < dateadd(day, 31, getDate()) AND no_of_problems IS NOT NULL "

            dt.Rows.Add(dr)

            dr = ds.Tables(0).NewRow
            dr.Item("display") = " Ordered after 16.05.2011 "
            dr.Item("filter") = " AND ordered_date >= '20110516' AND no_of_problems IS NOT NULL "

            dt.Rows.Add(dr)


            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Filtered to a specific date range.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnSO("Filtered to a specific date.", strFilterPhrase)

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSOBySeverity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSOBySeverity.Click
        Try

            Dim ds As DataSet = db.SecureQueryParams("SELECT order_no, SUM(no_of_problems) numberOf, sum(extended_price) valueSO  FROM salesOrderLinesView WHERE no_of_problems IS NOT NULL AND organization_id = @1 GROUP BY order_no ORDER By order_no", Me.organizationID)

            Dim dc As New DataColumn("filter", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc)
            Dim dc2 As New DataColumn("display", System.Type.GetType("System.String"))
            ds.Tables(0).Columns.Add(dc2)
            Dim dc3 As New DataColumn("severity", System.Type.GetType("System.Double"))
            ds.Tables(0).Columns.Add(dc3)

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                'Create the filter phrase
                ds.Tables(0).Rows(i).Item("filter") = "AND order_no = '" & ds.Tables(0).Rows(i).Item("order_no") & "'"

                Try

                    'Create the thing to display
                    ds.Tables(0).Rows(i).Item("display") = ds.Tables(0).Rows(i).Item("order_no").ToString & " - " & FormatCurrency(ds.Tables(0).Rows(i).Item("valueSO"), 0, TriState.True, TriState.True, TriState.True) & " : " & ds.Tables(0).Rows(i).Item("numberOf").ToString
                    ds.Tables(0).Rows(i).Item("severity") = Double.Parse(ds.Tables(0).Rows(i).Item("valueSO").ToString) * Double.Parse(ds.Tables(0).Rows(i).Item("numberOf").ToString)
                Catch ex As Exception

                End Try


            Next
            ds.Tables(0).DefaultView.Sort = "severity desc"
            Dim dlg As New dlgChooseSubFilter(ds.Tables(0), "display", "filter", "Order Number - Value of this Order : Number of Problems for this Order Types.")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                Dim strFilterPhrase As String = dlg.propValueMember
                frmWorkbenchProjects.loadFilterOnSO("Filter on specific order number.", strFilterPhrase)

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSORC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSORC1.Click
        frmWorkbenchProjects.loadFilterOnShortages("My Current Supply Problems", _
                                                  " AND reason_code = 'RC1 - Planned Compressed Order' ", "")

    End Sub

    Private Sub btnSORC2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSORC2.Click
        frmWorkbenchProjects.loadFilterOnShortages("Current Supply Problems with RC2", _
                                                 " AND reason_code = 'RC2 - Missing Promise Date' ", "")
    End Sub

    Private Sub btnSORC3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSORC3.Click
        frmWorkbenchProjects.loadFilterOnShortages("Current Supply Problems with RC3", _
                                                " AND reason_code = 'RC3 - Late Promise Date' ", "")
    End Sub

    Private Sub btnSORC4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSORC4.Click
        frmWorkbenchProjects.loadFilterOnShortages("Current Supply Problems with RC4", _
                                               " AND reason_code = 'RC4 - Receiving Required' ", "")
    End Sub

    Private Sub btnSORC5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSORC5.Click

        frmWorkbenchProjects.loadFilterOnShortages("Current Supply Problems with RC5", _
                                               " AND reason_code = 'RC5 - Planned Compressed Order' ", "")

    End Sub

    Private Sub btnSORC6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSORC6.Click
        frmWorkbenchProjects.loadFilterOnShortages("Current Supply Problems with RC6", _
                                              " AND reason_code = 'RC6 - Late Start Date' ", "")
    End Sub

    Private Sub btnSORC7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSORC7.Click
        frmWorkbenchProjects.loadFilterOnShortages("Current Supply Problems with RC7", _
                                              " AND reason_code = 'RC7 - Late Completion Date' ", "")

    End Sub


    Private Sub btnTrendNumberOfOpenSalesOrderLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendNumberOfOpenSalesOrderLines.Click

        Dim uc As New ucIndicatorTracker(Me.organizationID, 3, 20, "Central Order Planning Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()

    End Sub

    Private Sub btnTrendValueOpenSalesOrderLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendValueOpenSalesOrderLines.Click

        Dim uc As New ucIndicatorTracker(Me.organizationID, 3, 21, "Central Order Planning Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnTrendNewSalesOrderLines_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendNewSalesOrderLines.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 3, 22, "Central Order Planning Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnTrendNumberSupplyProblems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendNumberSupplyProblems.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 3, 23, "Central Order Planning Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnTrendNumberLatePlannedSO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendNumberLatePlannedSO.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 3, 24, "Central Order Planning Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnSORC0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSORC0.Click

        Dim strFilterPhrase As String = " AND (SELECT TOP 1 critical_component_flag FROM items b WHERE salesOrderLinesView.inventory_item_id = b.inventory_item_id AND salesOrderLinesView.organization_id = b.organization_id ) = 1" & _
                                         " AND possibleLeadtime <> 0 " & _
                                         "  AND currentLeadtime <> 0  " & _
                                         " AND (case when possibleLeadtime = 0 THEN 1 ELSE currentLeadtime / possibleLeadtime END )> (SELECT TOP 1 calculation_value FROM warehouse_lovs b WHERE ORDER_TYPE = b.lov_value_char)  " & _
                                         " AND (SELECT     TOP (1) DESCRIPTION  " & _
                                         "                       FROM          comments AS comm  " & _
                                         "                       WHERE      (salesOrderLinesView.HEADER_ID = HEADER_ID) AND (salesOrderLinesView.LINE_NO = LINE_ID) AND (ENTITY_ID = 'salesOrderLinesView') AND DESCRIPTION LIKE '%BOM COMPLETED%' " & _
                                         "                       ORDER BY CommentID DESC) IS NULL "

        frmWorkbenchProjects.loadFilterOnSO("Filter on sales orders which should complete their BOMs.", strFilterPhrase)

    End Sub


    Private Sub btnTrendRC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendRC1.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 2, 9, Me.buyerName, "Purchasing Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()

    End Sub

    Private Sub btnTrendRC2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendRC2.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 2, 16, Me.buyerName, "Purchasing Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnTrendRC3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendRC3.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 2, 11, Me.buyerName, "Purchasing Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnTrendRC4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendRC4.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 2, 12, Me.buyerName, "Purchasing Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnTrendRequests_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendRequests.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 2, 26, Me.buyerName, "Purchasing Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnTrendUsedSuppliers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTrendUsedSuppliers.Click
        Dim uc As New ucIndicatorTracker(Me.organizationID, 2, 25, Me.buyerName, "Purchasing Workbench")
        Dim frm As New frmOneIndicator(Me.organizationID, uc)
        frm.Show()
    End Sub

    Private Sub btnBacklog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBacklog.Click
        Me.tcWorkbenches.SelectedTab = Me.tcWorkbenches.TabPages("tpBacklog")
    End Sub

    Private Sub btnBookAndBill_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBookAndBill.Click
        Me.tcWorkbenches.SelectedTab = Me.tcWorkbenches.TabPages("tpBookAndBill")
    End Sub

    Private Sub btnOTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOTP.Click
        Me.tcWorkbenches.SelectedTab = Me.tcWorkbenches.TabPages("tpOTP")
    End Sub

    Private Sub loadRevenueReport()
        'Load event of the report
        ' Add any initialization after the InitializeComponent() call.
        'Reset report and change to local processing mode
        Me.rvRevenuePlanning.Visible = True
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
        rvRevenuePlanning.Reset()
        rvRevenuePlanning.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        '_dsReportSource = dsReportData
        'Change the report source to local processing
        Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvRevenuePlanning.LocalReport
        rep.Refresh()
        rep.ReportPath = "ProjectManagerWorkbench\Reporting\repRevenuePlanning.rdlc"
        rds.Name = "dsWork_tblWork"

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.Query("SELECT TOP 100 * FROM tblWork")

        'set to delegated data source
        rds.Value = ds.Tables(0)
        rep.DataSources.Add(rds)

        'Go for the backlog pieces

        Me.createRevenueData()

        Me.rvRevenuePlanning.Refresh()

        'rep.Refresh()

    End Sub


    '' <summary>
    '' Procedure to create the information for the revenue planning.
    '' </summary>
    '' <remarks></remarks>
    Private Sub createRevenueData()
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        Try
            Dim totOrdern As Double = 0
            Dim totQty As Double = 0
            Dim totSumme As Double = 0

            Dim totLateOrdern As Double = 0
            Dim totLateQty As Double = 0
            Dim totLateSumme As Double = 0

            Dim totIntercoOrdern As Double = 0
            Dim totIntercoQty As Double = 0
            Dim totIntercoSumme As Double = 0

            Dim totIntercoLateOrdern As Double = 0
            Dim totIntercoLateQty As Double = 0
            Dim totIntercoLateSumme As Double = 0

            db.Connect()

            'Get the PLEUGER filter
            Dim ds As DataSet = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Pleuger", 255)

            'With this information go into the backlog and grab the relevant information
            Dim dsBak As DataSet = db.SecureQueryParams("SELECT ISNULL(SUM(extended_price),0) extendedPrice, datepart(month, promise_date) month_promised, datepart(month,getdate()) currentMonth  FROM salesOrderLinesView " &
                                                        "   WHERE organization_id = @1 " &
                                                        " AND datepart(year,promise_date) = datepart(year,getDate()) AND substring(CONVERT(varchar,CONVERT(integer,order_no)),2,1) <> '5' " & ds.Tables(0).Rows(0).Item(0) &
                                                        " group by datepart(month,promise_date)     " &
                                                        " ORDER BY datepart(month,promise_date)     ", 255)

            Dim value As Double = 0
            Dim total As Double = 0
            For i As Integer = 0 To dsBak.Tables(0).Rows.Count - 1
                total += dsBak.Tables(0).Rows(i).Item("extendedPrice")

                If dsBak.Tables(0).Rows(i).Item("month_promised") < dsBak.Tables(0).Rows(i).Item("currentMonth") Then
                    value += dsBak.Tables(0).Rows(i).Item("extendedPrice")
                Else
                    If value > 0 Then
                        'We need to add this month
                        value += dsBak.Tables(0).Rows(i).Item("extendedPrice")
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("pleugerUnits" & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                        value = 0
                    Else
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("pleugerUnits" & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                    End If

                End If
            Next

            With Me.rvRevenuePlanning.LocalReport
                Dim parameters(0) As ReportParameter
                ' parameters(0) = New ReportParameter("pleugerUnitsYear", FormatNumber(total / 1000, 0, TriState.True, TriState.True, TriState.True))
                parameters(0) = New ReportParameter("pleugerUnitsYear", total / 1000)
                .SetParameters(parameters)
            End With

            '### Get the PLEUGER Buyouts filter
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Pleuger", 255)

            'With this information go into the backlog and grab the relevant information
            dsBak = db.SecureQueryParams("SELECT ISNULL(SUM(extended_price),0) extendedPrice, datepart(month, promise_date) month_promised, datepart(month,getdate()) currentMonth  FROM salesOrderLinesView " &
                                                        "   WHERE organization_id = @1 " &
                                                        " AND datepart(year,promise_date) = datepart(year,getDate()) AND substring(CONVERT(varchar,CONVERT(integer,order_no)),2,1) = '5' " & ds.Tables(0).Rows(0).Item(0) &
                                                        " group by datepart(month,promise_date)     " &
                                                        " ORDER BY datepart(month,promise_date)     ", 255)

            value = 0
            total = 0

            For i As Integer = 0 To dsBak.Tables(0).Rows.Count - 1
                total += dsBak.Tables(0).Rows(i).Item("extendedPrice")

                If dsBak.Tables(0).Rows(i).Item("month_promised") < dsBak.Tables(0).Rows(i).Item("currentMonth") Then
                    value += dsBak.Tables(0).Rows(i).Item("extendedPrice")
                Else
                    If value > 0 Then
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("pleugerBuyouts" & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                        value = 0
                    Else
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("pleugerBuyouts" & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                    End If

                End If
            Next

            With Me.rvRevenuePlanning.LocalReport
                Dim parameters(0) As ReportParameter
                parameters(0) = New ReportParameter("pleugerBuyoutsYear", FormatNumber(total / 1000, 0, TriState.True, TriState.True, TriState.True))
                .SetParameters(parameters)
            End With


            '#### Byron Jackson ####
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Byron Jackson", 255)

            'With this information go into the backlog and grab the relevant information
            dsBak = db.SecureQueryParams("SELECT ISNULL(SUM(extended_price),0) extendedPrice, datepart(month, promise_date) month_promised, datepart(month,getdate()) currentMonth  FROM salesOrderLinesView " &
                                                        "   WHERE organization_id = @1 " &
                                                        " AND datepart(year,promise_date) = datepart(year,getDate()) AND substring(CONVERT(varchar,CONVERT(integer,order_no)),2,1) <> '5' " & ds.Tables(0).Rows(0).Item(0) &
                                                        " group by datepart(month,promise_date)     " &
                                                        " ORDER BY datepart(month,promise_date)     ", 255)

            value = 0
            total = 0

            For i As Integer = 0 To dsBak.Tables(0).Rows.Count - 1
                total += dsBak.Tables(0).Rows(i).Item("extendedPrice")

                If dsBak.Tables(0).Rows(i).Item("month_promised") < dsBak.Tables(0).Rows(i).Item("currentMonth") Then
                    value += dsBak.Tables(0).Rows(i).Item("extendedPrice")
                Else
                    If value > 0 Then
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("bjUnits" & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                        value = 0
                    Else
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("bjUnits" & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                    End If

                End If
            Next

            With Me.rvRevenuePlanning.LocalReport
                Dim parameters(0) As ReportParameter
                parameters(0) = New ReportParameter("bjUnitsYear", FormatNumber(total / 1000, 0, TriState.True, TriState.True, TriState.True))
                .SetParameters(parameters)
            End With

            '#### Byron Jackson Buyout ####
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Byron Jackson", 255)

            'With this information go into the backlog and grab the relevant information
            dsBak = db.SecureQueryParams("SELECT ISNULL(SUM(extended_price),0) extendedPrice, datepart(month, promise_date) month_promised, datepart(month,getdate()) currentMonth  FROM salesOrderLinesView " &
                                                        "   WHERE organization_id = @1 " &
                                                        " AND datepart(year,promise_date) = datepart(year,getDate()) AND substring(CONVERT(varchar,CONVERT(integer,order_no)),2,1) = '5' " & ds.Tables(0).Rows(0).Item(0) &
                                                        " group by datepart(month,promise_date)     " &
                                                        " ORDER BY datepart(month,promise_date)     ", 255)

            value = 0
            total = 0

            For i As Integer = 0 To dsBak.Tables(0).Rows.Count - 1
                total += dsBak.Tables(0).Rows(i).Item("extendedPrice")

                If dsBak.Tables(0).Rows(i).Item("month_promised") < dsBak.Tables(0).Rows(i).Item("currentMonth") Then
                    value += dsBak.Tables(0).Rows(i).Item("extendedPrice")
                Else
                    If value > 0 Then
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("bjBuyout" & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                        value = 0
                    Else
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("bjBuyout" & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                    End If

                End If
                If dsBak.Tables(0).Rows.Count = 1 Then
                    If value > 0 Then
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("bjBuyout" & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                        value = 0
                    Else
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("bjBuyout" & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                    End If
                End If
            Next

            With Me.rvRevenuePlanning.LocalReport
                Dim parameters(0) As ReportParameter
                parameters(0) = New ReportParameter("bjBuyoutYear", FormatNumber(total / 1000, 0, TriState.True, TriState.True, TriState.True))
                .SetParameters(parameters)
            End With


            '#### Recips Units ####
            ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", "Aufträge - Recips u. Pack", 255)

            'With this information go into the backlog and grab the relevant information
            dsBak = db.SecureQueryParams("SELECT ISNULL(SUM(extended_price),0) extendedPrice, datepart(month, promise_date) month_promised, datepart(month,getdate()) currentMonth  FROM salesOrderLinesView " &
                                                        "   WHERE organization_id = @1 " &
                                                        " AND datepart(year,promise_date) = datepart(year,getDate()) AND substring(CONVERT(varchar,CONVERT(integer,order_no)),2,1) <> '5' " & ds.Tables(0).Rows(0).Item(0) &
                                                        " group by datepart(month,promise_date)     " &
                                                        " ORDER BY datepart(month,promise_date)     ", 255)

            value = 0
            total = 0

            For i As Integer = 0 To dsBak.Tables(0).Rows.Count - 1
                total += dsBak.Tables(0).Rows(i).Item("extendedPrice")

                If dsBak.Tables(0).Rows(i).Item("month_promised") < dsBak.Tables(0).Rows(i).Item("currentMonth") Then
                    value += dsBak.Tables(0).Rows(i).Item("extendedPrice")
                Else
                    If value > 0 Then
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("recipsUnits" & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                        value = 0
                    Else
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("recipsUnits" & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                    End If

                End If
                If dsBak.Tables(0).Rows.Count = 1 Then
                    If value > 0 Then
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("recipsUnits" & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                        value = 0
                    Else
                        With Me.rvRevenuePlanning.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("recipsUnits" & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                            .SetParameters(parameters)
                        End With
                    End If
                End If
            Next

            With Me.rvRevenuePlanning.LocalReport
                Dim parameters(0) As ReportParameter
                parameters(0) = New ReportParameter("recipsUnitsYear", FormatNumber(total / 1000, 0, TriState.True, TriState.True, TriState.True))
                .SetParameters(parameters)
            End With

            Me.addAnotherParameter("Aufträge - Recips u. Pack", "recipsBuyouts", "recipsBuyoutsYear", " = '5'")
            Me.addAnotherParameter("Aufträge - Other Decoker", "decokerUnits", "decokerUnitsYear", " <> '5'")
            Me.addAnotherParameter("Aufträge - Other Decoker", "decokerBuyouts", "decokerBuyoutsYear", " = '5'")
            Me.addAnotherParameter("Aufträge - Thruster", "thrusterUnits", "thrusterUnitsYear", " <> '5'")
            Me.addAnotherParameter("Aufträge - Thruster", "thrusterBuyouts", "thrusterBuyoutsYear", " = '5'")
            Me.addAnotherParameter("Aufträge - Geothermie", "geothermieUnits", "geothermieUnitsYear", " <> '5'")
            Me.addAnotherParameter("Aufträge - Geothermie", "geothermieBuyouts", "geothermieBuyoutsYear", " = '5'")


            With Me.rvRevenuePlanning.LocalReport
                Dim parameters(1) As ReportParameter
                parameters(0) = New ReportParameter("createdBy", Environment.UserName)
                parameters(1) = New ReportParameter("planningYear", Now.Year)
                .SetParameters(parameters)
            End With


        Catch

        End Try

    End Sub


    Private Sub addAnotherParameter(ByVal filterName As String, ByVal seriesName As String, ByVal seriesTotalName As String, ByVal buyUnits As String)
        '#### Recips Units ####
        Dim ds = db.SecureQueryParams("SELECT filterString FROM [warehouseFilterTemplates] WHERE filterName = @1 AND organizationID = @2", filterName, 255)

        'With this information go into the backlog and grab the relevant information
        Dim dsBak = db.SecureQueryParams("SELECT ISNULL(SUM(extended_price),0) extendedPrice, datepart(month, promise_date) month_promised, datepart(month,getdate()) currentMonth  FROM salesOrderLinesView " &
                                                    "   WHERE organization_id = @1 " &
                                                    " AND datepart(year,promise_date) = datepart(year,getDate()) AND substring(CONVERT(varchar,CONVERT(integer,order_no)),2,1)  " & buyUnits & ds.Tables(0).Rows(0).Item(0) &
                                                    " group by datepart(month,promise_date)     " &
                                                    " ORDER BY datepart(month,promise_date)     ", 255)

        Dim value = 0
        Dim total = 0

        For i As Integer = 0 To dsBak.Tables(0).Rows.Count - 1
            total += dsBak.Tables(0).Rows(i).Item("extendedPrice")

            If dsBak.Tables(0).Rows(i).Item("month_promised") < dsBak.Tables(0).Rows(i).Item("currentMonth") Then
                value += dsBak.Tables(0).Rows(i).Item("extendedPrice")
            Else
                If value > 0 Then
                    With Me.rvRevenuePlanning.LocalReport
                        Dim parameters(0) As ReportParameter
                        parameters(0) = New ReportParameter(seriesName & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                        .SetParameters(parameters)
                    End With
                    value = 0
                Else
                    With Me.rvRevenuePlanning.LocalReport
                        Dim parameters(0) As ReportParameter
                        parameters(0) = New ReportParameter(seriesName & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                        .SetParameters(parameters)
                    End With
                End If

            End If
            If dsBak.Tables(0).Rows.Count = 1 Then
                If value > 0 Then
                    With Me.rvRevenuePlanning.LocalReport
                        Dim parameters(0) As ReportParameter
                        parameters(0) = New ReportParameter(seriesName & dsBak.Tables(0).Rows(i).Item("currentMonth").ToString, FormatNumber(value / 1000, 0, TriState.True, TriState.True, TriState.True))
                        .SetParameters(parameters)
                    End With
                    value = 0
                Else
                    With Me.rvRevenuePlanning.LocalReport
                        Dim parameters(0) As ReportParameter
                        parameters(0) = New ReportParameter(seriesName & dsBak.Tables(0).Rows(i).Item("month_promised").ToString, FormatNumber(dsBak.Tables(0).Rows(i).Item("extendedPrice") / 1000, 0, TriState.True, TriState.True, TriState.True))
                        .SetParameters(parameters)
                    End With
                End If
            End If
        Next

        With Me.rvRevenuePlanning.LocalReport
            Dim parameters(0) As ReportParameter
            parameters(0) = New ReportParameter(seriesTotalName, FormatNumber(total / 1000, 0, TriState.True, TriState.True, TriState.True))
            .SetParameters(parameters)
        End With
    End Sub


    Private Sub PrintToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        ' Copy the form's image into a bitmap.
        mPrintBitMap = New Bitmap(Me.Width, Me.Width)
        Dim lRect As System.Drawing.Rectangle
        lRect.Width = Me.Width
        lRect.Height = Me.Width
        Me.DrawToBitmap(mPrintBitMap, lRect)


        ' Make a PrintDocument and print.
        mPrintDocument = New PrintDocument
        mPrintDocument.DefaultPageSettings.Landscape = True
        'mPrintDocument.DefaultPageSettings.PrinterSettings.
        mPrintDocument.Print()

    End Sub

    Dim WithEvents mPrintDocument As New PrintDocument
    Dim mPrintBitMap As Bitmap

    Private Sub m_PrintDocument_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles mPrintDocument.PrintPage
        ' Draw the image centered.
        Dim lWidth As Integer = e.MarginBounds.X + (e.MarginBounds.Width - mPrintBitMap.Width) \ 2
        Dim lHeight As Integer = e.MarginBounds.Y + (e.MarginBounds.Height - mPrintBitMap.Height) \ 2
        e.Graphics.DrawImage(mPrintBitMap, lWidth, lHeight)

        ' There's only one page.
        e.HasMorePages = False
    End Sub


End Class