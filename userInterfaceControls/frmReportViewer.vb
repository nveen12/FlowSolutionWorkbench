Imports Microsoft.Reporting.WinForms
Imports System.IO


Public Class frmReportViewer

    Private _dsReportSource As DataSet
    Private _dsReportSource2 As DataSet

    ''' <summary>
    ''' Generic constructor with data set and report path. No parameters handled
    ''' </summary>
    ''' <param name="reportName"></param>
    ''' <param name="reportPath"></param>
    ''' <param name="dataSetName"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal reportName As String, ByVal reportPath As String, ByVal dataSetName As String, ByVal dsReportData As DataSet, ByVal parameter1 As String)
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Reset report and change to local processing mode
        Me.rvStandard.Visible = True
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
        rvStandard.Reset()
        rvStandard.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        _dsReportSource = dsReportData
        'Change the report source to local processing
        Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvStandard.LocalReport
        rep.Refresh()
        rep.ReportPath = reportPath
        rds.Name = dataSetName

        'set to delegated data source
        rds.Value = _dsReportSource.Tables(0)
        rep.DataSources.Add(rds)

        'Add parameter for the sales order printout
        If reportPath = "ProjectManagerWorkbench\Reporting\repSOPrint.rdlc" Then
            With Me.rvStandard.LocalReport
                Dim parameters(0) As ReportParameter
                parameters(0) = New ReportParameter("NameOfFilter", parameter1)
                .SetParameters(parameters)
            End With
        End If


    End Sub

    ''' <summary>
    ''' Constructor for the discussion tracking printout.
    ''' </summary>
    ''' <param name="reportName"></param>
    ''' <param name="reportPath"></param>
    ''' <param name="parameter1"></param>
    ''' <param name="parameter2"></param>
    ''' <remarks></remarks>
    'Public Sub New(ByVal reportName As String, ByVal reportPath As String, ByVal parameter1 As String, ByVal parameter2 As String)
    '    InitializeComponent()
    '    Me.rvStandard.Visible = True
    '    Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
    '    rvStandard.Reset()
    '    rvStandard.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local


    '    'Change the report source to local processing
    '    Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvStandard.LocalReport
    '    rep.Refresh()
    '    rep.ReportPath = reportPath




    '    'Add parameter for the sales order printout

    '    With Me.rvStandard.LocalReport

    '        Dim parameters(1) As ReportParameter
    '        parameters(0) = New ReportParameter("discusssion", parameter2)
    '        parameters(1) = New ReportParameter("orderHeader", parameter1)
    '        .SetParameters(parameters)

    '    End With


    'End Sub


    ''' <summary>
    ''' Generic constructor with data set and report path. No parameters handled
    ''' </summary>
    ''' <param name="reportName"></param>
    ''' <param name="reportPath"></param>
    ''' <param name="dataSetName"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal reportName As String, ByVal reportPath As String, ByVal dataSetName As String, ByVal dsReportData As DataSet)
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Reset report and change to local processing mode
        Me.rvStandard.Visible = True
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
        rvStandard.Reset()
        rvStandard.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        _dsReportSource = dsReportData
        'Change the report source to local processing
        Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvStandard.LocalReport
        rep.Refresh()
        rep.ReportPath = reportPath
        rds.Name = dataSetName

        'set to delegated data source
        rds.Value = _dsReportSource.Tables(0)
        rep.DataSources.Add(rds)

        'Go for the backlog pieces
        If reportPath = "ProjectManagerWorkbench\Reporting\repBacklogOverview.rdlc" Then
            Me.createBacklogData()

        End If
        rep.Refresh()
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("pleugerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("pleugerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("pleugerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Backlog Pleuger
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            'With Me.rvStandard.LocalReport
            '    Dim parameters(2) As ReportParameter
            '    parameters(0) = New ReportParameter("latePleugerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
            '    parameters(1) = New ReportParameter("latePleugerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
            '    parameters(2) = New ReportParameter("latePleugerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

            '    .SetParameters(parameters)
            'End With

            'Backlog Pleuger Interco
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I'" & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            'With Me.rvStandard.LocalReport
            '    Dim parameters(2) As ReportParameter
            '    parameters(0) = New ReportParameter("intercoPleugerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
            '    parameters(1) = New ReportParameter("intercoPleugerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
            '    parameters(2) = New ReportParameter("intercoPleugerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

            '    .SetParameters(parameters)
            'End With

            'Backlog Pleuger Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice   FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("bjQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("bjOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("bjSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Backlog Byron Jackson
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoBJQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoBJOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoBJSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Byron Jackson Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("recipsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("recipsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("recipsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Recips
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoRecipsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoRecipsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoRecipsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'BacklogRecips Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("decokerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("decokerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("decokerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Decoker
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoDecokerQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoDecokerOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoDecokerSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Decoker Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("thrusterQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("thrusterOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("thrusterSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Thruster
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoThrusterQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoThrusterOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoThrusterSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Thruster Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("geothermieQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("geothermieOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("geothermieSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Geothermie
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoGeothermieQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoGeothermieOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoGeothermieSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Geothermie Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLateGeothermieQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLateGeothermieOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLateGeothermieSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With


            '##### Units/Buyouts - Total

            '#### Units / Buyouts - Total
            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("unitsTotalQty", totQty.ToString)
                parameters(1) = New ReportParameter("unitsTotalOrdern", totOrdern.ToString)
                parameters(2) = New ReportParameter("unitsTotalSumme", FormatCurrency(totSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '#### Units / Buyouts - Total - Late
            With Me.rvStandard.LocalReport
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
                With Me.rvStandard.LocalReport
                    Dim parameters(0) As ReportParameter
                    parameters(0) = New ReportParameter("latePercentageUnits", FormatPercent(lateP, 2, TriState.True, TriState.True, TriState.True))
                    .SetParameters(parameters)
                End With

            Catch ex As Exception

            End Try

            '#### Units / Buyouts - Interco
            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoUnitsQty", totIntercoQty.ToString)
                parameters(1) = New ReportParameter("intercoUnitsOrdern", totIntercoOrdern.ToString)
                parameters(2) = New ReportParameter("intercoUnitsSumme", FormatCurrency(totIntercoSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '#### Units / Buyouts - Interco - Late
            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("repairTotalQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("repairTotalOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("repairTotalSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Repair
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoRepairQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoRepairOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoRepairSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Repair Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("partsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("partsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("partsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'late Parts
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice  FROM salesOrderLinesView WHERE organization_id = @1 AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
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

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoPartsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoPartsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoPartsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            'Backlog Parts Interco Late
            dsBak = db.SecureQueryParams("SELECT COUNT(distinct(order_no)) distinctOrder, ISNULL(SUM(ORDERED_QUANTITY),0) ordered_quantity, ISNULL(SUM(extended_price),0) extendedPrice FROM salesOrderLinesView WHERE organization_id = @1 AND customer_type = 'I' AND promise_date < getDate() " & ds.Tables(0).Rows(0).Item(0) & "", 255)

            totIntercoLateOrdern += dsBak.Tables(0).Rows(0).Item("distinctOrder")
            totIntercoLateQty += dsBak.Tables(0).Rows(0).Item("ordered_quantity")
            totIntercoLateSumme += dsBak.Tables(0).Rows(0).Item("extendedPrice")

            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoLatePartsQty", dsBak.Tables(0).Rows(0).Item("ordered_quantity").ToString)
                parameters(1) = New ReportParameter("intercoLatePartsOrdern", dsBak.Tables(0).Rows(0).Item("distinctOrder").ToString)
                parameters(2) = New ReportParameter("intercoLatePartsSumme", FormatCurrency(dsBak.Tables(0).Rows(0).Item("extendedPrice"), 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '##### Total
            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("totalQty", totQty.ToString)
                parameters(1) = New ReportParameter("totalOrdern", totOrdern.ToString)
                parameters(2) = New ReportParameter("totalSumme", FormatCurrency(totSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With


            '#### Total - Late
            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("lateTotalQty", totLateQty.ToString)
                parameters(1) = New ReportParameter("lateTotalOrdern", totLateOrdern.ToString)
                parameters(2) = New ReportParameter("lateTotalSumme", FormatCurrency(totLateSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            Try
                lateP = totLateSumme / totSumme
                With Me.rvStandard.LocalReport
                    Dim parameters(0) As ReportParameter
                    parameters(0) = New ReportParameter("latePercentageTotal", FormatPercent(lateP, 2, TriState.True, TriState.True, TriState.True))
                    .SetParameters(parameters)
                End With

            Catch ex As Exception

            End Try

            '#### Total - Interco
            With Me.rvStandard.LocalReport
                Dim parameters(2) As ReportParameter
                parameters(0) = New ReportParameter("intercoTotalQty", totIntercoQty.ToString)
                parameters(1) = New ReportParameter("intercoTotalOrdern", totIntercoOrdern.ToString)
                parameters(2) = New ReportParameter("intercoTotalSumme", FormatCurrency(totIntercoSumme, 2, TriState.True, TriState.True, TriState.True))

                .SetParameters(parameters)
            End With

            '#### Total - Interco - Late 
            With Me.rvStandard.LocalReport
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

    ''' <summary>
    ''' Constructor for the NAFTA certificate
    ''' </summary>
    ''' <param name="reportName">The report name - This contructor maybe used from another place later</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal reportName As String, ByVal orderNo As String, ByVal headerID As String, ByVal organizationID As Integer, ByVal dsValues As DataSet)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ''Header text of the form to the delegated name
        Me.Text = reportName

        ' Add any initialization after the InitializeComponent() call.
        Me.rvStandard.Visible = True
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
        rvStandard.Reset()
        rvStandard.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        'Chan
        Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvStandard.LocalReport
        rep.Refresh()
        rep.ReportPath = "ProjectManagerWorkbench\Reporting\repNaftaSummary.rdlc"
        rds.Name = "dsNaftaSummary_NAFTA"


        'rds.Value = dsUserReport.Tables(0)
        'Go to the method to fill the data set
        Dim ds As New DataSet
        'Fill the dataSet manually
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        'ds = db.Query("SELECT [vendorNo] " & _
        '                  ",[vendorName]  " & _
        '                  ",[vendorItem]  " & _
        '                  ",[itemNo]  " & _
        '                  ",[description]  " & _
        '                  ",[drawing]  " & _
        '                  ",[productCode]  " & _
        '                  ",[planner]  " & _
        '                  ",[tariff]  " & _
        '                  ",[prefCriterion]  " & _
        '                  ",[producer]  " & _
        '                  ",[netCost]  " & _
        '                  ",[countryOfOrigin]  " & _
        '                  ",[comments]  " & _
        '                  ",[NAFTAPart]  " & _
        '                  "FROM [NAFTA] WHERE itemNo IN " & listOfItems & "")

        'New select statement for it:
        ds = db.Query("SELECT null as [vendorNo], text1 as vendorName, text2 as vendorItem, text3 as description, text4 as drawing, text5 as productCode, text6 as planner, text7 as tariff, text8 as prefCriterion, text9 as producer, text10 as netCost, text11 as countryOfOrigin, text12 as comments, text13 as NAFTAPart, text14 as itemNo  FROM tblWork2")

        'In between we have to display the data, save and redirect it.
        _dsReportSource = ds

        rds.Value = ds.Tables(0)
        rep.DataSources.Add(rds)

        'Get the adress of the customer
        Dim pub As New clsPublicVariables
        Dim dbO As New DB.ORacleServerDB(My.Settings.OracleSTARConnectionString)

        Dim dsAdress As DataSet
        dsAdress = dbO.Query("SELECT PARTY.party_name ,PARTY.address1, PARTY.address2, PARTY.city, PARTY.postal_code, PARTY.state, PARTY.country " &
                                            "FROM OE_ORDER_HEADERS_ALL       COHDR " &
                                            "    ,OE_ORDER_LINES_ALL         COLINES " &
                                            "    ,HZ_CUST_ACCOUNTS           CUST " &
                                            "    ,HZ_PARTIES                 PARTY " &
                                            "        WHERE " &
                                            "        COLINES.header_id = COHDR.header_id    " &
                                            "AND CUST.cust_account_id    = COHDR.sold_to_org_id  " &
                                            "AND CUST.party_id           = PARTY.party_id  " &
                                            "AND rownum                  = 1  " &
                                            "AND COHDR.header_id = " & headerID & "")

        'Should have a value in the data set, so for the time ignore that there may be no value

        With Me.rvStandard.LocalReport

            Dim parameters(7) As ReportParameter
            parameters(0) = New ReportParameter("salesOrder", orderNo)
            parameters(1) = New ReportParameter("importerName1", dsAdress.Tables(0).Rows(0).Item("party_name").ToString)
            parameters(2) = New ReportParameter("importerName2", dsAdress.Tables(0).Rows(0).Item("address1").ToString)
            parameters(3) = New ReportParameter("importerStreet", dsAdress.Tables(0).Rows(0).Item("address2").ToString)
            parameters(4) = New ReportParameter("importerCity", dsAdress.Tables(0).Rows(0).Item("city").ToString)
            parameters(5) = New ReportParameter("importerPostalCode", dsAdress.Tables(0).Rows(0).Item("postal_code").ToString)
            parameters(6) = New ReportParameter("importerState", dsAdress.Tables(0).Rows(0).Item("state").ToString)
            parameters(7) = New ReportParameter("importerCountry", dsAdress.Tables(0).Rows(0).Item("country").ToString)
            .SetParameters(parameters)
        End With
    End Sub

    Public Property propCurrentDataSource()
        Get
            Return _dsReportSource
        End Get
        Set(ByVal value)
            _dsReportSource = value
        End Set
    End Property

    ''' <summary>
    ''' Should refresh the data source of the report.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub refreshReport()
        Me.rvStandard.Refresh()
    End Sub

    ''' <summary>
    ''' Constructor for the detail NAFTA report.
    ''' </summary>
    ''' <param name="strReportName"></param>
    ''' <param name="salesOrderHeader">The sales OrderHeader </param>
    ''' <remarks></remarks>
    Public Sub New(ByVal strReportName As String, ByVal salesOrderHeader As String, ByVal salesOrderLine As String, ByVal itemNo As String, ByVal unitSellingPrice As Double, ByVal nonNAFTACost As Double, ByVal NAFTAPercent As Double, ByVal blnTmp As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Text = "Report - NAFTA Detail Report for Order Number: " & salesOrderHeader & " Line Number:" & salesOrderLine

        Me.rvStandard.Visible = True
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
        rvStandard.Reset()
        rvStandard.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        'Chan
        Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvStandard.LocalReport
        rep.Refresh()
        rep.ReportPath = "ProjectManagerWorkbench\Reporting\repNaftaDetail.rdlc"
        rds.Name = "dsNAFTA_DataTable1"

        'rds.Value = dsUserReport.Tables(0)
        'Go to the method to fill the data set
        Dim ds As New DataSet
        'Fill the dataSet manually
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        ds = db.Query("SELECT     dbo.NAFTA.itemNo AS Item, dbo.tblWork3.Text2 AS Description, dbo.tblWork3.Number3 AS PurMfg, dbo.tblWork3.Number2 AS QtyPer, " &
                         " dbo.tblWork3.Number8 AS Cost, dbo.NAFTA.vendorNo, dbo.NAFTA.tariff, dbo.NAFTA.prefCriterion AS Criteria, dbo.NAFTA.netCost, dbo.NAFTA.producer, " &
                         " dbo.NAFTA.countryOfOrigin AS Country, dbo.NAFTA.NAFTAPart AS Qualify " &
                            "FROM         dbo.NAFTA INNER JOIN " &
                            "                      dbo.tblWork3 ON dbo.NAFTA.itemNo = dbo.tblWork3.Text1 ")

        rds.Value = ds.Tables(0)
        rep.DataSources.Add(rds)

        Try
            With rvStandard.LocalReport
                Dim rpParameters(6) As ReportParameter
                rpParameters(0) = New ReportParameter("salesOrderHeader", salesOrderHeader)
                rpParameters(1) = New ReportParameter("salesOrderLine", salesOrderLine)
                rpParameters(2) = New ReportParameter("item", itemNo)
                rpParameters(3) = New ReportParameter("unitSellingPrice", unitSellingPrice)
                rpParameters(4) = New ReportParameter("nonNAFTACost", nonNAFTACost)
                rpParameters(5) = New ReportParameter("NAFTAPercent", NAFTAPercent)
                rpParameters(6) = New ReportParameter("qualifies", Str(blnTmp))
                .SetParameters(rpParameters)
            End With

        Catch ex As Exception

        End Try

        rvStandard.RefreshReport()


    End Sub

    ''' <summary>
    ''' Constructor for the country of origin check.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal blnCountryOfOrigin As Boolean, ByVal strSalesOrder As String, ByVal strHeaderId As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim pub As New clsPublicVariables
        Dim dbO As New DB.ORacleServerDB(My.Settings("OracleConnectionStringTNS"))
        Me.Text = "Report - Country of origin for: " & strSalesOrder

        ' Add any initialization after the InitializeComponent() call.


        'Setting up the details of the dataset
        '-----------------------------------------------------------------------------------------------------------------------
        Me.rvStandard.Visible = True
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
        rvStandard.Reset()
        rvStandard.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        'Change to local mode
        Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvStandard.LocalReport
        rep.Refresh()
        rep.ReportPath = "ProjectManagerWorkbench\Reporting\repSOShipCheck.rdlc"
        rds.Name = "dsOrderShipping_sales_orders"

        'rds.Value = dsUserReport.Tables(0)
        'Go to the method to fill the data set
        Dim ds As New DataSet
        'Fill the dataSet manually
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        _dsReportSource = dbO.Query(" SELECT " &
                        " COHDR.order_number            order_no " &
                        " ,COLINES.line_number          line_no " &
                        " ,COLINES.ordered_item         item_no " &
                        " ,NVL(COLINES.attribute3,' ')  description " &
                        " ,COLINES.ordered_quantity " &
                        " ,NVL(COLINES.shipped_quantity,0)    shipped_quantity " &
                        " ,DECODE(MSI.planning_make_buy_code, 1, 'Make',2,'Buy') planning_make_buy_code " &
                        " ,COLINES.inventory_item_id " &
                        " , NULL po_no " &
                        " , NULL quantity_received " &
                        " , NULL vendor_name " &
                        " , NULL voucher_no " &
                        " , NULL invoice_no " &
                        "FROM " &
                        "    OE_ORDER_HEADERS_ALL     COHDR " &
                        "   ,OE_ORDER_LINES_ALL         COLINES " &
                        "   ,mtl_system_items_b         MSI " &
                        "WHERE " &
                        "    COLINES.header_id = COHDR.header_id " &
                        " AND COLINES.inventory_item_id   = MSI.inventory_item_id " &
                        " AND MSI.organization_id         = 255 " &
                        " AND COHDR.header_id             = " & strHeaderId &
                        " AND COHDR.ship_from_org_id      = 255 ORDER BY COLINES.line_number ASC")





        ' Setting up the adress of the customer
        Dim dsAdress As DataSet
        dsAdress = dbO.Query("SELECT PARTY.party_name ,PARTY.address1, PARTY.address2, PARTY.city, PARTY.postal_code, PARTY.state, PARTY.country " &
                                            "FROM OE_ORDER_HEADERS_ALL       COHDR " &
                                            "    ,OE_ORDER_LINES_ALL         COLINES " &
                                            "    ,HZ_CUST_ACCOUNTS           CUST " &
                                            "    ,HZ_PARTIES                 PARTY " &
                                            "        WHERE " &
                                            "        COLINES.header_id = COHDR.header_id    " &
                                            "AND CUST.cust_account_id    = COHDR.sold_to_org_id  " &
                                            "AND CUST.party_id           = PARTY.party_id  " &
                                            "AND rownum                  = 1  " &
                                            "AND COHDR.header_id = " & strHeaderId & "")

        'Should have a value in the data set, so for the time ignore that there may be no value

        With Me.rvStandard.LocalReport
            Dim parameters(7) As ReportParameter
            parameters(0) = New ReportParameter("salesOrder", strSalesOrder)
            parameters(1) = New ReportParameter("importerName1", dsAdress.Tables(0).Rows(0).Item("party_name").ToString)
            parameters(2) = New ReportParameter("importerName2", dsAdress.Tables(0).Rows(0).Item("address1").ToString)
            parameters(3) = New ReportParameter("importerStreet", dsAdress.Tables(0).Rows(0).Item("address2").ToString)
            parameters(4) = New ReportParameter("importerCity", dsAdress.Tables(0).Rows(0).Item("city").ToString)
            parameters(5) = New ReportParameter("importerPostalCode", dsAdress.Tables(0).Rows(0).Item("postal_code").ToString)
            parameters(6) = New ReportParameter("importerState", dsAdress.Tables(0).Rows(0).Item("state").ToString)
            parameters(7) = New ReportParameter("importerCountry", dsAdress.Tables(0).Rows(0).Item("country").ToString)
            .SetParameters(parameters)
        End With

        Dim strListItems As String = "("
        Dim blnRequired As Boolean = False
        'Create the list of buy items and send it to the next report
        For i As Integer = 0 To _dsReportSource.Tables(0).Rows.Count - 1
            If _dsReportSource.Tables(0).Rows(i).Item("planning_make_buy_code") = "Buy" Then
                If strListItems.ToCharArray.Count > 1 Then
                    strListItems += ","
                End If
                strListItems += _dsReportSource.Tables(0).Rows(i).Item("inventory_item_id").ToString
                blnRequired = True
            End If
        Next
        strListItems += ")"
        If blnRequired Then
            Dim frm As New frmReportViewer(True, strListItems, Str(255), strSalesOrder)
            frm.Show()
        End If


        rds.Value = _dsReportSource.Tables(0)
        rep.DataSources.Add(rds)

    End Sub

    ''' <summary>
    ''' Constructor for the detail of the country of origin report.
    ''' </summary>
    ''' <param name="blnDetails"></param>
    ''' <param name="strOrderInventoryItemIds">(1454457, 4548787, 464676, 456466)</param>
    ''' <param name="strOrganizationId">The organization this report should be created for.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal blnDetails As Boolean, ByVal strOrderInventoryItemIds As String, ByVal strOrganizationId As String, ByVal strSalesOrder As String)

        InitializeComponent()
        Dim pub As New clsPublicVariables
        Dim dbO As New DB.ORacleServerDB(pub.connectionStringEDC)
        Me.Text = "Report - Country of origin for: " & strSalesOrder

        'Setting up the details of the dataset
        '-----------------------------------------------------------------------------------------------------------------------
        Me.rvStandard.Visible = True
        Dim rds As New Microsoft.Reporting.WinForms.ReportDataSource
        rvStandard.Reset()
        rvStandard.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local

        'Change to local mode
        Dim rep As Microsoft.Reporting.WinForms.LocalReport = rvStandard.LocalReport
        rep.Refresh()
        rep.ReportPath = "ProjectManagerWorkbench\Reporting\repOldPOLines.rdlc"
        rds.Name = "dsOrderShippingPO_DataTable1"

        'rds.Value = dsUserReport.Tables(0)
        'Go to the method to fill the data set
        Dim ds As New DataSet
        ds = dbO.Query(" SELECT         " &
                        "   POH.segment1         po_no  " &
                        " , POH.po_header_id                " &
                        " , POL.line_num                      line_no  " &
                        " , POL.item_id  " &
                        " , POH.type_lookup_code  " &
                        " , pol.item_description  " &
                        " , PLOC.quantity                     quantity_ordered  " &
                        " , NVL(PLOC.quantity_received,0) as  qty_received  " &
                        " , POH.printed_date  " &
                        " , VEND.vendor_name  " &
                        " ,(SELECT pvs.country   " &
                        "       FROM PO_VENDOR_SITES_ALL PVS WHERE POH.VENDOR_ID = PVS.VENDOR_ID AND ROWNUM = 1) country   " &
                        " ,sum(1) over (  " &
                        "                            partition by POL.item_id   " &
                        "                           order by POH.segment1  " &
                        "                          ) running_count_poLines  " &
                        " ,msi.segment1                       item_no      " &
                        "     FROM   " &
                        "    po_headers_all           POH  " &
                        " ,po_lines_all               POL  " &
                        " ,po_line_locations_all      PLOC  " &
                        " ,po_vendors                 VEND  " &
                        " ,mtl_system_items_b         MSI  " &
                        "    WHERE  " &
                        "    POL.po_header_id = poh.po_header_id  " &
                        " AND VEND.vendor_id            (+)= poh.vendor_id  " &
                        " AND ploc.po_line_id             = POL.po_line_id  " &
                        " AND msi.inventory_item_id       = POL.item_id  " &
                        " AND msi.organization_id         = PLOC.ship_to_organization_id  " &
                        " AND POL.item_id IN " & strOrderInventoryItemIds & " ORDER BY msi.segment1, POH.printed_date DESC ")

        Dim dsFiltered As New DataSet
        Dim foundRows() As DataRow

        ' Use the Select method to find all rows matching the filter.
        foundRows = ds.Tables(0).Select("running_count_poLines < 6")


        'Create a list of po_headers to find the invoices
        Dim strListpo As String = "("

        'Create the list of buy items and send it to the next report
        For i As Integer = 0 To foundRows.Count - 1
            If strListpo.ToCharArray.Count > 1 Then
                strListpo += ","
            End If
            strListpo += foundRows(i).Item("po_header_id").ToString
        Next
        strListpo += ")"
        Dim dsInv As DataSet

        dsInv = dbO.Query("SELECT  voucher_num, invoice_num, invoice_date  FROM apps.ap_invoices_all " &
                            " WHERE org_id = 254 " &
                            " AND po_header_id IN " & strListpo & " ")

        'Change the top level datasource to the required type
        For i As Integer = 0 To _dsReportSource.Tables(0).Rows.Count
            'add the po information to the items. append the first, then add another row
            For j As Integer = 0 To foundRows.Count

            Next
        Next


        rds.Value = ds.Tables(0)
        rep.DataSources.Add(rds)

        With Me.rvStandard.LocalReport
            Dim parameters(0) As ReportParameter
            parameters(0) = New ReportParameter("salesOrder", strSalesOrder)
            .SetParameters(parameters)
        End With

    End Sub

    ''' <summary>
    ''' Sets and gets the header of the viewer form.
    ''' </summary>
    ''' <value>The displayed text.</value>
    ''' <returns>The displayed text.</returns>
    ''' <remarks></remarks>
    Public Property formText()
        Get
            Return Me.Text
        End Get
        Set(ByVal value)
            Me.Text = value
        End Set
    End Property
    Private Sub frmReportViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'dsUserReport.whUserOrganizationsView' table. You can move, or remove it, as needed.
        'Me.whUserOrganizationsViewTableAdapter.Fill(Me.dsUserReport.whUserOrganizationsView)

        Me.rvStandard.RefreshReport()
        Me.rvStandard.RefreshReport()
        Me.rvStandard.RefreshReport
        Me.rvStandard.RefreshReport
    End Sub

    Public Sub saveCurrentReportToExcel(ByVal path As String)
        Dim myByte() As Byte
        Dim warnings As Warning() = Nothing
        Dim streamids As String() = Nothing
        Dim mimeType As String = Nothing
        Dim encoding As String = Nothing
        Dim extension As String = Nothing

        myByte = rvStandard.LocalReport.Render("Excel", Nothing, mimeType,
            encoding, extension, streamids, warnings)

        Dim fs As FileStream = New FileStream(path & ".xls", FileMode.Create)
        Try

            fs.Write(myByte, 0, myByte.Length)
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("saveCurrentReportToExcel", ex, 1, path & ".xls")

        Finally
            fs.Close()
        End Try


    End Sub

    Public Sub printTheReport()
        Me.rvStandard.PrintDialog()

    End Sub

End Class