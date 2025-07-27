Imports System.Windows.Forms.DataVisualization.Charting

Public Class ucOnTimePerformance

    Private organizationID As Integer
    Private businessRule1 As List(Of businessRuleFilter)
    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)

    Private strDataDimension As String = " [Day] > [dbo].[ufn_GetFirstDayOfMonth](getDate()) "
    Private strDataDimensionTarget As String = " [validFrom] > [dbo].[ufn_GetFirstDayOfMonth](getDate()) "
    Private strAdditionalBusinessDimension As String = ""
    Private strDateHorizon As String = "Month to Date"
    Private myForm As frmIntegratedWorkflow

    Public Sub New(ByVal organizationId As Integer)

        Me.organizationID = organizationId

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        'Me.myForm = frm
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
    
    End Sub
    Private filterStringValues1() As String
    Private filterStringValues2() As String

    

    Private Sub ucOnTimePerformance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

        
            If Me.organizationID = 255 Then
                Dim filterStringView() As String = {"Pleuger", "Byron Jackson", "Recips", "Packaging", "Decoker", "Thruster"}
                Dim filterStringValuest() As String = {" substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),1,1) = 1", "  substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),1,1) = 2", "  substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),1,1) = 3", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),1,1) =  4 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),1,1) = 5 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),1,1) = 6 "}

                'Save in private variable
                filterStringValues1 = filterStringValuest

                Dim filterStringView2() As String = {"Units", "Pump", "Motor", "Parts", "Buyout", "Repair", "Service", "Warranty"}
                Dim filterStringValuest2() As String = {" substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),2,1) = 1 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),2,1) = 2 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),2,1) = 3 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),2,1) = 4 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),2,1) = 5 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),2,1) = 6 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),2,1) = 7 ", " substring(CONVERT(varchar,CONVERT(integer,[ORDER_NUMBER])),2,1) = 9 "}
                filterStringValues2 = filterStringValuest2

                'Add the views to the checked list boxes All items will be checked from the beginning
                For i As Integer = 0 To filterStringView.Count - 1
                    clbBusiness.Items.Add(filterStringView(i), True)
                Next
                For i As Integer = 0 To filterStringView2.Count - 1
                    clbBusiness2.Items.Add(filterStringView2(i), True)
                Next

                Me.strAdditionalBusinessDimension = ""
            End If

            Me.resetChartFormat()
            Me.loadBookings()
            Me.loadBillings()
            Me.loadOTP()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub resetChartFormat(Optional ByVal addHeader As String = "Month to Date")

        'Format the charts
        Me.chBooking.ResetAutoValues()
        Me.chBooking.Series.Clear()
        Me.chBooking.Legends("Legend1").Docking = DataVisualization.Charting.Docking.Bottom
        Me.chBooking.Legends("Legend1").Alignment = StringAlignment.Center
        Me.chBooking.Titles.Clear()
        Me.chBooking.Titles.Add("Daily Bookings " & addHeader)

        'Format the billing
        Me.chBilling.Titles.Clear()
        Me.chBilling.Titles.Add("Daily Billings " & addHeader)
        Me.chBilling.ResetAutoValues()
        Me.chBilling.Series.Clear()
        Me.chBilling.Legends("Legend1").Docking = DataVisualization.Charting.Docking.Bottom
        Me.chBilling.Legends("Legend1").Alignment = StringAlignment.Center

        'Format the OTP
        Me.chOTP.Titles.Clear()
        Me.chOTP.Titles.Add("Daily On Time Performance " & addHeader)
        Me.chOTP.ResetAutoValues()
        Me.chOTP.Series.Clear()
        Me.chOTP.Legends("Legend1").Docking = DataVisualization.Charting.Docking.Bottom
        Me.chOTP.Legends("Legend1").Alignment = StringAlignment.Center

    End Sub

    Private Sub loadOTP()

        Dim ds As DataSet = db.Query(" SELECT " & _
                                     " Convert(datetime,CONVERT(char(8), [Day], 112), 104)  day                              " & _
                                     " ,SUM([totalPrice]) total, COUNT(*) numberShipped, SUM(late) countLate                        " & _
                                     " FROM  [BBBBillingsAggregatView]                                                              " & _
                                     " WHERE organization_id = 255 And " & strDataDimension & strAdditionalBusinessDimension & _
                                     " GROUP BY Convert(datetime,CONVERT(char(8), [Day], 112), 104)                          " & _
                                     " ORDER BY Convert(datetime,CONVERT(char(8), [Day], 112), 104)")

        Dim dt As DataTable = ds.Tables(0)

        'calculate percentage on time
        Dim dc As New DataColumn("PercentageOnTime", System.Type.GetType("System.Double"))
        dt.Columns.Add(dc)
        Dim runTotal As Double = 0
        For i As Integer = 0 To dt.Rows.Count - 1
            Try
                If Not DBNull.Value.Equals(dt.Rows(i).Item("countLate")) AndAlso Not DBNull.Value.Equals(dt.Rows(i).Item("numberShipped")) Then
                    runTotal = 1 - Double.Parse(dt.Rows(i).Item("countLate")) / Double.Parse(dt.Rows(i).Item("numberShipped"))
                End If
                dt.Rows(i).Item("PercentageOnTime") = runTotal
            Catch ex As Exception
                'lngNumberHits += 1
            End Try
        Next

        Dim dr2 As DataTableReader = dt.CreateDataReader

        Me.chOTP.Series.Add("Daily Number of Shipments")
        'Me.chBooking.Series("DailyBookings").ChartType = DataVisualization.Charting.SeriesChartType.Bar
        Me.chOTP.Series("Daily Number of Shipments").Points.DataBindXY(dr2, "day", dr2, "numberShipped")
        ' Me.chBilling.ChartAreas(0).AxisY.LabelStyle.Format = "C"

        Dim dr3 As DataTableReader = dt.CreateDataReader
        'Add the running total
        Me.chOTP.Series.Add("Daily Number Late")
        Me.chOTP.Series("Daily Number Late").Points.DataBindXY(dr3, "day", dr3, "countLate")
        Me.chOTP.Series("Daily Number Late").Color = Color.Red

        Dim dr4 As DataTableReader = dt.CreateDataReader
        'Add the running total
        Me.chOTP.Series.Add("Percentage on Time")
        Me.chOTP.Series("Percentage on Time").Points.DataBindXY(dr4, "day", dr4, "PercentageOnTime")
        Me.chOTP.Series("Percentage on Time").YAxisType = AxisType.Secondary
        Me.chOTP.Series("Percentage on Time").ChartType = SeriesChartType.Line
        Me.chOTP.Series("Percentage on Time").Color = Color.Blue
        Me.chOTP.Series("Percentage on Time").LabelFormat = "P0"


        ''Add the weighted average against the value
        'If Me.chOTP.Series("Percentage on Time").Points.Count > 200 Then
        '    Me.chOTP.DataManipulator.FinancialFormula(FinancialFormula.WeightedMovingAverage, "60", "Percentage on Time", "Weighted OTP (60)")
        '    Me.chOTP.Series("Weighted OTP (60)").ChartType = SeriesChartType.Line
        '    Me.chOTP.Series("Weighted OTP (60)").YAxisType = AxisType.Secondary
        '    Me.chOTP.Series("Weighted OTP (60)").BorderWidth = 3
        '    Me.chOTP.Series("Weighted OTP (60)").Color = Color.Green
        '    Me.chOTP.Series("Weighted OTP (60)").LabelFormat = "P0"

        '    Me.chOTP.ChartAreas("ChartArea1").AxisY2.LabelStyle.Format = "P0"
        '    strWeightedName = "Weighted OTP (60)"
        'ElseIf Me.chOTP.Series("Percentage on Time").Points.Count > 100 Then
        '    Me.chOTP.DataManipulator.FinancialFormula(FinancialFormula.WeightedMovingAverage, "30", "Percentage on Time", "Weighted OTP (30)")
        '    Me.chOTP.Series("Weighted OTP (30)").ChartType = SeriesChartType.Line
        '    Me.chOTP.Series("Weighted OTP (30)").YAxisType = AxisType.Secondary
        '    Me.chOTP.Series("Weighted OTP (30)").BorderWidth = 3
        '    Me.chOTP.Series("Weighted OTP (30)").Color = Color.Green
        '    Me.chOTP.Series("Weighted OTP (30)").LabelFormat = "P0"

        '    Me.chOTP.ChartAreas("ChartArea1").AxisY2.LabelStyle.Format = "P0"
        '    strWeightedName = "Weighted OTP (30)"
        'ElseIf Me.chOTP.Series("Percentage on Time").Points.Count > 5 Then
        '    Me.chOTP.DataManipulator.FinancialFormula(FinancialFormula.WeightedMovingAverage, "5", "Percentage on Time", "Weighted OTP (5)")
        '    Me.chOTP.Series("Weighted OTP (5)").ChartType = SeriesChartType.Line
        '    Me.chOTP.Series("Weighted OTP (5)").YAxisType = AxisType.Secondary
        '    Me.chOTP.Series("Weighted OTP (5)").BorderWidth = 3
        '    Me.chOTP.Series("Weighted OTP (5)").Color = Color.Green
        '    Me.chOTP.Series("Weighted OTP (5)").LabelFormat = "P0"
        '    Me.chOTP.ChartAreas("ChartArea1").AxisY2.LabelStyle.Format = "P0"

        '    strWeightedName = "Weighted OTP (5)"
        'End Ifles

        Me.lblOTPValue.Text = FormatPercent(Me.chOTP.DataManipulator.Statistics.Mean("Percentage on Time"), 2, TriState.True, TriState.True, TriState.True)
        Me.loadTargets()


    End Sub

    Private Sub loadTargets()
        Try

            Dim ds As New DataSet
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            strDataDimension.Replace("[Day]", "validFrom")
            strDataDimension.Replace(">", ">=")

            ds = db.SecureQueryParams(" SELECT [ID] " & _
                                      " ,[performanceID] " & _
                                      " ,[organizationID] " & _
                                      " ,[value] " & _
                                      " ,[validFrom] " & _
                                      " ,[validUntil] " & _
                                      " ,[name] " & _
                                      " ,[createdBY] " & _
                                      " ,[creationDate] " & _
                                  " FROM [performanceTargets] WHERE performanceID = 39 " & _
                                  " AND organizationID = @1 AND validUntil < @2  AND " & strDataDimensionTarget & _
                                  " ORDER BY validFrom", Me.organizationID, Date.Now.AddMonths(1))

            Dim dc As New DataColumn("value", System.Type.GetType("System.Double"))
            Dim dc2 As New DataColumn("date", System.Type.GetType("System.DateTime"))
            Dim dt As New DataTable


            dt.Columns.Add(dc)
            dt.Columns.Add(dc2)

            Dim newRow As Integer = 0
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                Dim dtNewRow As DataRow
                dtNewRow = dt.NewRow()

                dtNewRow.Item("value") = ds.Tables(0).Rows(i).Item("value")
                dtNewRow.Item("date") = ds.Tables(0).Rows(i).Item("validFrom")
                newRow += 1
                dt.Rows.Add(dtNewRow)

                dtNewRow = dt.NewRow()
                dtNewRow.Item("value") = ds.Tables(0).Rows(i).Item("value")
                dtNewRow.Item("date") = ds.Tables(0).Rows(i).Item("validUntil")
                newRow += 1
                dt.Rows.Add(dtNewRow)
            Next

            Dim dr4 As DataTableReader = dt.CreateDataReader
            'Add the running total
            Me.chOTP.Series.Add("Target")
            Me.chOTP.Series("Target").Points.DataBindXY(dr4, "date", dr4, "value")
            Me.chOTP.Series("Target").Color = Color.Green
            Me.chOTP.Series("Target").BorderDashStyle = ChartDashStyle.Solid
            Me.chOTP.Series("Target").YAxisType = AxisType.Secondary
            Me.chOTP.Series("Target").ChartType = SeriesChartType.Line
            Me.chOTP.Series("Target").BorderWidth = 4

        Catch ex As Exception

        End Try
    End Sub

    Private strWeightedName As String


    Private Sub loadBillings()
        Dim ds As DataSet = db.Query(" SELECT " & _
                                    "  Convert(datetime,CONVERT(char(8), [Day], 112), 104)  day " & _
                                    "  ,SUM([totalPrice]) total " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBillingsAggregatView] " & _
                                    "    WHERE organization_id = 255 And " & strDataDimension & strAdditionalBusinessDimension & _
                                      " GROUP BY Convert(datetime,CONVERT(char(8), [Day], 112), 104) " & _
                                      " ORDER BY Convert(datetime,CONVERT(char(8), [Day], 112), 104)")

        Dim dt As DataTable = ds.Tables(0)
        'calculate running total
        Dim dc As New DataColumn("Running Total", System.Type.GetType("System.Double"))
        dt.Columns.Add(dc)
        Dim runTotal As Double = 0
        For i As Integer = 0 To dt.Rows.Count - 1
            Try
                If Not DBNull.Value.Equals(dt.Rows(i).Item("total")) Then
                    runTotal += Double.Parse(dt.Rows(i).Item("total"))
                End If
                dt.Rows(i).Item("Running Total") = runTotal
            Catch ex As Exception
                'lngNumberHits += 1
            End Try
        Next

        Dim dr2 As DataTableReader = dt.CreateDataReader
        Me.chBilling.Series.Add("Daily Billings")
        'Me.chBooking.Series("DailyBookings").ChartType = DataVisualization.Charting.SeriesChartType.Bar
        Me.chBilling.Series("Daily Billings").Points.DataBindXY(dr2, "day", dr2, "total")
        Me.chBilling.ChartAreas(0).AxisY.LabelStyle.Format = "C"

        Dim dr3 As DataTableReader = dt.CreateDataReader
        'Add the running total
        Me.chBilling.Series.Add("Daily Billings Cumulated")
        Me.chBilling.Series("Daily Billings Cumulated").Points.DataBindXY(dr3, "day", dr3, "Running Total")
        Me.chBilling.Series("Daily Billings Cumulated").ChartType = SeriesChartType.StepLine
        Me.chBilling.Series("Daily Billings Cumulated").Color = Color.Green
        Me.chBilling.Series("Daily Billings Cumulated").BorderWidth = 3
        Me.chBilling.Series("Daily Billings Cumulated").Points(Me.chBilling.Series("Daily Billings Cumulated").Points.Count - 1).IsValueShownAsLabel = True
        Me.chBilling.Series("Daily Billings Cumulated").Points(Me.chBilling.Series("Daily Billings Cumulated").Points.Count - 1).LabelFormat = "C"

        'Same maximum on Y-Axis
        If Me.chBilling.Series("Daily Billings Cumulated").Points(Me.chBilling.Series("Daily Billings Cumulated").Points.Count - 1).YValues(0) > Me.chBooking.Series("Daily Bookings Cumulated").Points(Me.chBooking.Series("Daily Bookings Cumulated").Points.Count - 1).YValues(0) Then
            Me.chBooking.ChartAreas("ChartArea1").AxisY.Maximum = Me.chBilling.Series("Daily Billings Cumulated").Points(Me.chBilling.Series("Daily Billings Cumulated").Points.Count - 1).YValues(0)
            Me.chBilling.ChartAreas("ChartArea1").AxisY.Maximum = Me.chBilling.Series("Daily Billings Cumulated").Points(Me.chBilling.Series("Daily Billings Cumulated").Points.Count - 1).YValues(0)
        Else
            Me.chBilling.ChartAreas("ChartArea1").AxisY.Maximum = Me.chBooking.Series("Daily Bookings Cumulated").Points(Me.chBooking.Series("Daily Bookings Cumulated").Points.Count - 1).YValues(0)
            Me.chBooking.ChartAreas("ChartArea1").AxisY.Maximum = Me.chBooking.Series("Daily Bookings Cumulated").Points(Me.chBooking.Series("Daily Bookings Cumulated").Points.Count - 1).YValues(0)
        End If


        'Add the aggregat for the values:
        Dim ds2 As DataSet = db.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBillingsAggregatView] " & _
                                    "    WHERE organization_id = 255 And  " & strDataDimension & strAdditionalBusinessDimension & _
                                      " ")
        Try

            lblCompleteBillings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblDriiverBillings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblPartsBillings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblRepairBillings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblSpareBillings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblGlobalBillings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True)

        Catch ex As Exception

        End Try

        'Add the billing functionality
        Dim ds3 As DataSet = db.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBillingsAggregatView] " & _
                                    "    WHERE organization_id = 255 AND customer_type_code = 'I'  And  " & strDataDimension & strAdditionalBusinessDimension & _
                                      " ")
        Try

            lblInterCompleteBilling.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterDriverBilling.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterPartsBilling.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterRepairBilling.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterSpareBilling.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterGlobalBilling.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True)

        Catch ex As Exception

        End Try

        'Add the billing functionality
        Dim ds4 As DataSet = db.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBillingsAggregatView] " & _
                                    "    WHERE organization_id = 255 AND customer_type_code = 'R'  And  " & strDataDimension & strAdditionalBusinessDimension & _
                                      " ")
        Try

            lblCustCompleteBilling.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustDriverBilling.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustPartsBilling.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustRepairBilling.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustSpareBilling.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustGlobalBilling.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True)

        Catch ex As Exception

        End Try

    End Sub

    Private Sub loadBookings()
        Try


            Dim ds As DataSet = db.Query(" SELECT " & _
                                        "  Convert(datetime,CONVERT(char(8), [Day], 112), 104)  day " & _
                                        "  ,SUM([totalPrice]) total " & _
                                        "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBookingsAggregatView] " & _
                                        "    WHERE organization_id = 255 And  " & strDataDimension & strAdditionalBusinessDimension & _
                                          " GROUP BY Convert(datetime,CONVERT(char(8), [Day], 112), 104) " & _
                                          " ORDER BY Convert(datetime,CONVERT(char(8), [Day], 112), 104)")

            Dim dt As DataTable = ds.Tables(0)
            'calculate running total
            Dim dc As New DataColumn("Running Total", System.Type.GetType("System.Double"))
            dt.Columns.Add(dc)
            Dim runTotal As Double = 0
            For i As Integer = 0 To dt.Rows.Count - 1
                Try
                    If Not DBNull.Value.Equals(dt.Rows(i).Item("total")) Then
                        runTotal += Double.Parse(dt.Rows(i).Item("total"))
                    End If
                    dt.Rows(i).Item("Running Total") = runTotal
                Catch ex As Exception
                    'lngNumberHits += 1
                End Try
            Next

            Dim dr2 As DataTableReader = dt.CreateDataReader
            Me.chBooking.Series.Add("DailyBookings")
            'Me.chBooking.Series("DailyBookings").ChartType = DataVisualization.Charting.SeriesChartType.Bar
            Me.chBooking.Series("DailyBookings").Points.DataBindXY(dr2, "day", dr2, "total")
            Me.chBooking.ChartAreas(0).AxisY.LabelStyle.Format = "C"

            Dim dr3 As DataTableReader = dt.CreateDataReader
            'Add the running total
            Me.chBooking.Series.Add("Daily Bookings Cumulated")
            Me.chBooking.Series("Daily Bookings Cumulated").Points.DataBindXY(dr3, "day", dr3, "Running Total")
            Me.chBooking.Series("Daily Bookings Cumulated").ChartType = SeriesChartType.StepLine
            Me.chBooking.Series("Daily Bookings Cumulated").Color = Color.Green
            Me.chBooking.Series("Daily Bookings Cumulated").BorderWidth = 3
            Me.chBooking.Series("Daily Bookings Cumulated").Points(Me.chBooking.Series("Daily Bookings Cumulated").Points.Count - 1).IsValueShownAsLabel = True
            Me.chBooking.Series("Daily Bookings Cumulated").Points(Me.chBooking.Series("Daily Bookings Cumulated").Points.Count - 1).LabelFormat = "C"
        Catch ex As Exception

        End Try

        'Add the aggregat for the values:
        Dim ds2 As DataSet = db.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBookingsAggregatView] " & _
                                    "    WHERE organization_id = 255 And  " & strDataDimension & strAdditionalBusinessDimension & _
                                      " ")

        Try
            lblCompleteBbookings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblDriverBookings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblPartsBookings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblRepairBookings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblSpareBookings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblGlobalBookings.Text = FormatCurrency(ds2.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True)


        Catch ex As Exception

        End Try

        'Interco

        Dim ds3 As DataSet = db.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBookingsAggregatView] " & _
                                    "    WHERE organization_id = 255 AND customer_type_code = 'I' And  " & strDataDimension & strAdditionalBusinessDimension & _
                                      " ")

        Try
            lblInterComplete.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterDriver.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterParts.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterRepair.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterSpare.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblInterGlobal.Text = FormatCurrency(ds3.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True)

        Catch ex As Exception

        End Try

        'Customer

        Dim ds4 As DataSet = db.Query(" SELECT " & _
                                   "   " & _
                                   "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                   " ,SUM([DriverPrice]) DriverPrice " & _
                                   " ,SUM([partsPrice])  partsPrice " & _
                                   " ,SUM([repairPrice]) repairPrice " & _
                                    " ,SUM([sparePrice])  sparePrice " & _
                                   "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBookingsAggregatView] " & _
                                   "    WHERE organization_id = 255 AND customer_type_code = 'R' And  " & strDataDimension & strAdditionalBusinessDimension & _
                                     " ")

        Try
            lblCustComplet.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustDriver.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustParts.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustRepair.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustSpare.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True)
            lblCustGlobal.Text = FormatCurrency(ds4.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True)

        Catch ex As Exception

        End Try

    End Sub




    Private Sub cbxHorizontalPossible_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxHorizontalPossible.SelectedIndexChanged

        Dim strNewAddHeader As String = " Month to Date"
        If cbxHorizontalPossible.SelectedIndex = 0 Then
            strDataDimension = "[Day] >  [dbo].[ufn_GetFirstDayOfMonth](getDate()) "
            strDataDimensionTarget = "[validFrom] >=  [dbo].[ufn_GetFirstDayOfMonth](getDate()) "
            Me.strDateHorizon = "Month to Date"
        ElseIf cbxHorizontalPossible.SelectedIndex = 1 Then
            strDataDimension = " datepart(year,[Day]) >= datepart(year,getdate()) "
            strDataDimensionTarget = " datepart(year,[validFrom]) >= datepart(year,getdate()) "
            Me.strDateHorizon = "Year to Date"
        ElseIf cbxHorizontalPossible.SelectedIndex = 2 Then
            strDataDimension = "  datepart(qq,[Day]) >= datepart(qq, getDate()) AND  datepart(year,[Day]) >= datepart(year,getdate()) "
            strDataDimensionTarget = "  datepart(qq,[validFrom]) >= datepart(qq, getDate()) AND  datepart(year,[validFrom]) >= datepart(year,getdate()) "
            Me.strDateHorizon = "Quarter to Date"
        ElseIf cbxHorizontalPossible.SelectedIndex = 3 Then
            strDataDimension = " [Day] >  dateadd(day,-30,getdate()) "
            strDataDimensionTarget = " [Day] >=  dateadd(validFrom,-30,getdate()) "
            Me.strDateHorizon = "Last 30 Days"
        ElseIf cbxHorizontalPossible.SelectedIndex = 4 Then
            strDataDimension = " [Day] >  dateadd(day,-60,getdate()) "
            strDataDimensionTarget = " [validFrom] >=  dateadd(validFrom,-60,getdate()) "
            Me.strDateHorizon = "Last 60 Days"
        ElseIf cbxHorizontalPossible.SelectedIndex = 5 Then
            strDataDimension = " [Day] >  dateadd(day,-90,getdate()) "
            strDataDimensionTarget = " [validFrom] >=  dateadd(validFrom,-90,getdate()) "
            Me.strDateHorizon = "Last 90 Days"
        ElseIf cbxHorizontalPossible.SelectedIndex = 6 Then
            strDataDimension = " [Day] >  dateadd(day,-120,getdate()) "
            strDataDimensionTarget = " [validFrom] >=  dateadd(validFrom,-120,getdate()) "
            Me.strDateHorizon = "Last 120 Days"
        End If

        Me.resetChartFormat(Me.strDateHorizon)
        Me.loadBookings()
        Me.loadBillings()
        Me.loadOTP()

    End Sub

    Private Sub createBusinessDimensionFilterString()
        'Iterate over the selected listbox items and create the approrpiate filter string
        Dim strFilter1 As String = " AND ("
        Dim strFilter2 As String = " AND ("

        If clbBusiness.Items.Count > clbBusiness.CheckedItems.Count AndAlso clbBusiness.CheckedItems.Count <> 0 Then
            Dim intRunning As Integer = 0
            For Each i In clbBusiness.CheckedItems
                If intRunning = 0 Then
                ElseIf intRunning = clbBusiness.CheckedItems.Count Then
                Else
                    strFilter1 += " OR "
                End If
                strFilter1 += Me.filterStringValues1(clbBusiness.Items.IndexOf(clbBusiness.CheckedItems(intRunning)))

                intRunning += 1
            Next
            strFilter1 += " )"
        ElseIf clbBusiness.CheckedItems.Count = 0 Then
            strFilter1 = ""
        Else
            'Just create a filter when we do not want to see all
            strFilter1 = ""
        End If


        If clbBusiness2.Items.Count > clbBusiness2.CheckedItems.Count AndAlso clbBusiness2.CheckedItems.Count <> 0 Then
            Dim intRunning As Integer = 0
            For Each i In clbBusiness2.CheckedItems
                If intRunning = 0 Then
                ElseIf intRunning = clbBusiness2.CheckedItems.Count Then
                Else
                    strFilter2 += " OR "
                End If
                strFilter2 += Me.filterStringValues2(clbBusiness2.Items.IndexOf(clbBusiness2.CheckedItems(intRunning)))

                intRunning += 1
            Next
            strFilter2 += " )"
        ElseIf clbBusiness2.CheckedItems.Count = 0 Then
            strFilter2 = ""
        Else
            'Just create a filter when we do not want to see all
            strFilter2 = ""
        End If
        Try

            Me.strAdditionalBusinessDimension = strFilter1 & strFilter2
            If strAdditionalBusinessDimension <> "" Then
                Me.resetChartFormat(strDateHorizon)
                Me.loadBookings()
                Me.loadBillings()
                Me.loadOTP()
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub clbBusiness_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbBusiness.SelectedIndexChanged
        Me.createBusinessDimensionFilterString()
    End Sub

    Private Sub clbBusiness2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbBusiness2.SelectedIndexChanged
        Me.createBusinessDimensionFilterString()
    End Sub

    Private Sub btnReload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReload.Click
        strDateHorizon = " From " & FormatDateTime(dtpStart.Value, DateFormat.ShortDate) & " Until " & FormatDateTime(dtpEnd.Value, DateFormat.ShortDate)
        strDataDimension = "[Day] >= '" & Format(dtpStart.Value, "yyyyMMdd") & "' AND [Day] <= '" & Format(dtpEnd.Value, "yyyyMMdd") & "' "
        strDataDimensionTarget = " [validFrom] >=  '" & Format(dtpStart.Value, "yyyyMMdd") & "' AND [validFrom] <= '" & Format(dtpEnd.Value, "yyyyMMdd") & "'"

        Me.resetChartFormat(Me.strDateHorizon)
        Me.loadBookings()
        Me.loadBillings()
        Me.loadOTP()
    End Sub

    Public ReadOnly Property charControlOTP()
        Get
            Dim chOTP2 As Chart = chOTP
            Return chOTP2
        End Get
       
    End Property

   
    Private Sub cbxAggreation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxAggreation.SelectedIndexChanged
        '***Check if we still have daily information we can aggregat from!
        Try

       
            ' Group series data points by interval.
            Dim formula As String = "AVE, X:CENTER"

            If cbxAggreation.Text = "Week" Then
                chOTP.DataManipulator.Group(formula, 1, IntervalType.Weeks, _
                    "Percentage on Time", "OTP Percentage Grouped")

                chOTP.DataManipulator.Group("SUM, X:CENTER", 1, IntervalType.Weeks, _
                    "Daily Number of Shipments", "Shipments Grouped")

                chOTP.DataManipulator.Group("SUM, X:CENTER", 1, IntervalType.Weeks, _
                    "Daily Number Late", "Late Grouped")
            Else
                If cbxAggreation.Text = "2 Weeks" Then
                    chOTP.DataManipulator.Group(formula, 2, IntervalType.Weeks, _
                        "Percentage on Time", "OTP Percentage Grouped")

                    chOTP.DataManipulator.Group("SUM, X:CENTER", 2, IntervalType.Weeks, _
                   "Daily Number of Shipments", "Shipments Grouped")

                    chOTP.DataManipulator.Group("SUM, X:CENTER", 2, IntervalType.Weeks, _
                      "Daily Number Late", "Late Grouped")
                Else
                    If cbxAggreation.Text = "Month" Then
                        chOTP.DataManipulator.Group(formula, 1, IntervalType.Months, _
                            "Percentage on Time", "OTP Percentage Grouped")

                        chOTP.DataManipulator.Group("SUM, X:CENTER", 1, IntervalType.Months, _
                           "Daily Number of Shipments", "Shipments Grouped")

                        chOTP.DataManipulator.Group("SUM, X:CENTER", 1, IntervalType.Months, _
                            "Daily Number Late", "Late Grouped")

                    End If
                End If
            End If

            ' Change chart type of the grouped series
            If cbxFormula.Text = "High Low Op" Then
                chOTP.Series("OTP Percentage Grouped").ChartType = SeriesChartType.Stock

            Else
                If cbxFormula.Text = "High Low" Then
                    chOTP.Series("OTP Percentage Grouped").ChartType = SeriesChartType.SplineRange
                Else
                    chOTP.Series("OTP Percentage Grouped").ChartType = SeriesChartType.Point
                End If
            End If

            Me.chOTP.Series("OTP Percentage Grouped").YAxisType = AxisType.Secondary

            'Finally remove the daily numbers!
            chOTP.Series.Remove(chOTP.Series("Percentage on Time"))
            ' chOTP.Series.Remove(chOTP.Series(strWeightedName))
            chOTP.Series.Remove(chOTP.Series("Daily Number of Shipments"))
            chOTP.Series.Remove(chOTP.Series("Daily Number Late"))

        Catch ex As Exception

        End Try
    End Sub

    


    Private Sub chOTP_GetToolTipText(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs) Handles chOTP.GetToolTipText
        Try
            Select Case e.HitTestResult.ChartElementType

                Case ChartElementType.Axis
                    e.Text = e.HitTestResult.Axis.Name

                Case ChartElementType.DataPoint
                    Dim i As Integer = e.HitTestResult.PointIndex
                    Dim h As DataPoint = e.HitTestResult.Object

                    e.Text = "Y Value:" + h.YValues(0).ToString
            End Select
        Catch ex As Exception

        End Try
    End Sub

   
   

 
    
    Private Sub lblInterSpare_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblInterSpare.Click

    End Sub
End Class

Public Class businessRuleFilter
    Private disp As String
    Private filt As String

    Public Property display() As String
        Get
            Return disp
        End Get
        Set(ByVal value As String)
            disp = value
        End Set
    End Property

    Public Property filter() As String
        Get
            Return filt
        End Get
        Set(ByVal value As String)
            filt = value
        End Set
    End Property
End Class
