Public Class ucOTP

    Private organizationId As Integer = 0

    Public Sub New(ByVal organizationID As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.organizationId = organizationID
        Me.loadPerformanceData()

        Dim user As New clsUserControl
        If Not user.userInRole(Environment.UserName, "OTP User") Then
            gbDeleteReasons.Visible = False
            gbRules.Visible = False
            gbTargets.Visible = False
        End If

    End Sub


    

    Private Sub ucOTP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.loadOTPTable()


    End Sub

    Public Sub chartOTP(ByRef uc As System.Windows.Forms.DataVisualization.Charting.Chart)
        Me.scTop.Panel1.Controls.Add(uc)
        uc.Dock = DockStyle.Fill

    End Sub

   
    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Me.loadData()
    End Sub

    Private Sub loadData()

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        Dim ds As DataSet = db.SecureQueryParams("SELECT a.[ORDER_NUMBER] AS [Order Number]" & _
                                                  ",a.[PARTY_NAME] AS [Party Name] " & _
                                                  ",a.[PERIOD_NUM] AS [Period Num]" & _
                                                  ",a.[PERIOD_YEAR] AS [Period Year] " & _
                                                  ",a.[Day] " & _
                                                  ",a.[totalPrice] As [Total Price]" & _
                                                  ",a.[PROMISE_DATE] as [Promise Date] " & _
                                                  ",a.[Late] " & _
                                                  ",a.[CUSTOMER_TYPE_CODE] as [Customer Type Code]" & _
                                                  ",a.[completePrice] AS [Complete Price] " & _
                                                  ",a.[driverPrice] AS [Driver Price] " & _
                                                  ",a.[partsPrice] AS [Parts Price] " & _
                                                  ",a.[repairPrice] AS [Repair Price]" & _
                                                  ",a.[sparePrice] AS [Spare Price] " & _
                                                  " , (SELECT TOP 1 b.reason FROM BBBOTPReasons b WHERE a.ORDER_NUMBER = b.ORDER_NUMBER AND a.ORGANIZATION_ID = b.ORGANIZATION_ID AND a.PERIOD_YEAR = b.PERIOD_YEAR AND a.PERIOD_NUM = b.PERIOD_NUM) AS Reason" & _
                                                  " FROM BBBBillingsAggregatView a WHERE a.Day > @1 AND a.Day < @2 AND a.organization_id = @3 AND a.[Late]  = @4", dtpStart.Value, _
                                                  dtpEnd.Value, Me.organizationId, cbLate.Checked)

        dgvBacklog.DataSource = ds.Tables(0)

    End Sub




    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Try


            'Check if user really selected a row
            If dgvBacklog.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a few rows first.")
                Exit Sub
            End If


            For i As Integer = 0 To dgvBacklog.SelectedRows.Count - 1

                If Not DBNull.Value.Equals(dgvBacklog.SelectedRows(i).Cells("Reason").Value) Then
                    db.SecureNonQueryParams("DELETE FROM BBBOTPReasons  WHERE [ORGANIZATION_ID] = @1 " & _
                                           " AND [ORDER_NUMBER] = @2 " & _
                                           " AND [PERIOD_YEAR] = @3 " & _
                                           " AND [PERIOD_NUM] = @4", _
                                        Me.organizationId, dgvBacklog.SelectedRows(i).Cells("Order Number").Value _
                                        , dgvBacklog.SelectedRows(i).Cells("Period Year").Value _
                                        , dgvBacklog.SelectedRows(i).Cells("Period Num").Value)


                End If
            Next

            Me.loadData()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Try


            'Check if user really selected a row
            If dgvBacklog.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a few rows first.")
                Exit Sub
            End If

            If cbReasons.SelectedIndex = -1 Then
                MessageBox.Show("Please select a Reason first.")
                Exit Sub
            End If

            For i As Integer = 0 To dgvBacklog.SelectedRows.Count - 1

                If DBNull.Value.Equals(dgvBacklog.SelectedRows(i).Cells("Reason").Value) Then
                    db.SecureNonQueryParams("INSERT INTO BBBOTPReasons ([ORGANIZATION_ID] " & _
                                           ",[ORDER_NUMBER] " & _
                                           ",[PERIOD_YEAR] " & _
                                           ",[PERIOD_NUM] " & _
                                           ",[createdBy] " & _
                                           ",[creationDate] " & _
                                           ",[reason] " & _
                                           ",[description] " & _
                                           ",[notLate])  VALUES (@1,@2,@3,@4,@5,getDate(),@6,@7,1) ", _
                                        Me.organizationId, dgvBacklog.SelectedRows(i).Cells("Order Number").Value _
                                        , dgvBacklog.SelectedRows(i).Cells("Period Year").Value _
                                        , dgvBacklog.SelectedRows(i).Cells("Period Num").Value _
                                        , Environment.UserName _
                                        , cbReasons.SelectedItem, txtDescription.Text)


                End If
            Next

            Me.loadData()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub dgvBacklog_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvBacklog.CellFormatting

        'every second row will become beige
        If e.RowIndex Mod 2 = 0 Then
            e.CellStyle.BackColor = Color.Beige
        End If

        If Not DBNull.Value.Equals(e.Value) Then
            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Total Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Complete Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Driver Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Repair Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Spare Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Parts Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If
        End If

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        Dim ds As DataSet = db.SecureQueryParams("SELECT a.[ORDER_NUMBER] AS [Order Number]" & _
                                                  ",a.[PERIOD_NUM] AS [Period Num]" & _
                                                  ",a.[PERIOD_YEAR] AS [Period Year] " & _
                                                  ",a.[Day] " & _
                                                  ",a.[totalPrice] As [Total Price]" & _
                                                  ",a.[PROMISE_DATE] as [Promise Date] " & _
                                                  ",a.[Late] " & _
                                                  ",a.[CUSTOMER_TYPE_CODE] as [Customer Type Code]" & _
                                                  ",a.[completePrice] AS [Complete Price] " & _
                                                  ",a.[driverPrice] AS [Driver Price] " & _
                                                  ",a.[partsPrice] AS [Parts Price] " & _
                                                  ",a.[repairPrice] AS [Repair Price]" & _
                                                  ",a.[sparePrice] AS [Spare Price] " & _
                                                  " , (SELECT TOP 1 b.reason FROM BBBOTPReasons b WHERE a.ORDER_NUMBER = b.ORDER_NUMBER AND a.ORGANIZATION_ID = b.ORGANIZATION_ID AND a.PERIOD_YEAR = b.PERIOD_YEAR AND a.PERIOD_NUM = b.PERIOD_NUM) AS Reason" & _
                                                  " FROM BBBBillingsAggregatView a WHERE a.organization_id = @1 AND (a.[ORDER_NUMBER] LIKE '%" & _
                                                  txtSearch.Text & "%' OR a.[ORDER_NUMBER] = '" & txtSearch.Text & "')", Me.organizationId)

        dgvBacklog.DataSource = ds.Tables(0)

    End Sub

    Private Sub btnCreateTarget_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try




            'find if we are really creating a new target:
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

            'Find if we have valid from or until within a given range
            Dim ds As DataSet = db.SecureQueryParams("SELECT *  FROM performanceTargets WHERE validFrom < @1 and validUntil > @2", dtpStartTarget.Value, dtpStartTarget.Value)
            If ds.Tables(0).Rows.Count > 0 Then
                MessageBox.Show("Your Valid From Date is within an existing range")
                Exit Sub
            End If

            ds = db.SecureQueryParams("SELECT *  FROM performanceTargets WHERE validFrom < @1 and validUntil > @2", dtpUntilTarget.Value, dtpUntilTarget.Value)
            If ds.Tables(0).Rows.Count > 0 Then
                MessageBox.Show("Your Valid Until Date is within an existing range")
                Exit Sub
            End If


            Dim newId As Integer = db.Query("SELECT (ISNULL(MAX(ID),0) + 1) id FROM performanceTargets").Tables(0).Rows(0).Item(0)

            db.SecureNonQueryParams("INSERT INTO performanceTargets ([ID],[performanceID] " & _
          ",[organizationID]    " & _
          ",[value]             " & _
          ",[validFrom]         " & _
          ",[validUntil]        " & _
          ",[name]              " & _
          ",[createdBY]         " & _
          ",[creationDate]) " & _
          " VALUES (@1, @2, @3, @4, @5, @6, @7, @8, getDate())", newId, 39, Me.organizationId, txtTarget.Text, _
          dtpStartTarget.Value, dtpUntilTarget.Value, "Upper Limit", Environment.UserName)

            Me.loadPerformanceData()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub loadPerformanceData()

        'Loads data to the performance screen
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.SecureQueryParams("SELECT ID, value [Value], validFrom [Valid From], validUntil [Valid Until], createdBY [Created By], creationDate [Creation Date] " & _
                                                 " FROM performanceTargets WHERE organizationID = @1 AND performanceID = 39", Me.organizationId)

        dgvTargets.DataSource = ds.Tables(0)

    End Sub


    Private Sub btnDeleteTarget_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Check if user selected a row
        If dgvTargets.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a target before you try to delete one.")
            Exit Sub
        End If

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        For i As Integer = 0 To dgvTargets.SelectedRows.Count - 1
            db.SecureNonQueryParams("DELETE FROM performanceTargets WHERE ID = @1", dgvTargets.SelectedRows(i).Cells("ID").Value)
        Next
        Me.loadPerformanceData()


    End Sub

    Private Sub loadOTPTable()
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.Query("SELECT COUNT(*) number  FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate()) AND [partsPrice] <> 0  AND CUSTOMER_TYPE_CODE = 'R'")
        Dim ds2 As DataSet = db.Query("SELECT COUNT(*) number FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate()) AND [partsPrice] <> 0 AND Late = 0  AND CUSTOMER_TYPE_CODE = 'R'")

        Dim dc As New DataColumn("Name", System.Type.GetType("System.String"))
        Dim dc2 As New DataColumn("Value", System.Type.GetType("System.Int32"))
        Dim dc3 As New DataColumn("Percentage", System.Type.GetType("System.Double"))

        Dim dt As New DataTable

        dt.Columns.Add(dc)
        dt.Columns.Add(dc2)
        dt.Columns.Add(dc3)

        Dim newRow As Integer = 0

        Dim dtNewRow As DataRow
        dtNewRow = dt.NewRow()

        'Parts
        dtNewRow.Item("Name") = "Ontime Cust Aftermarket Parts"
        dtNewRow.Item("Value") = ds2.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Total Shipm to Cust Aftermarket Parts"
        dtNewRow.Item("Value") = ds.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "OTP Cust Aftermarket Parts"
        Try
            dtNewRow.Item("Percentage") = ds2.Tables(0).Rows(0).Item(0) / ds.Tables(0).Rows(0).Item(0)
        Catch ex As Exception

        End Try
        dt.Rows.Add(dtNewRow)

        'Repair Cust

        ds = db.Query("SELECT COUNT(*) number  FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate()) AND [repairPrice] <> 0  AND CUSTOMER_TYPE_CODE = 'R' AND substring(CONVERT(varchar,CONVERT(integer,order_number)),2,1) = '6' ")
        ds2 = db.Query("SELECT COUNT(*) number FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate()) AND [repairPrice] <> 0 AND Late = 0  AND CUSTOMER_TYPE_CODE = 'R' AND substring(CONVERT(varchar,CONVERT(integer,order_number)),2,1) = '6'")

        dtNewRow = dt.NewRow()


        dtNewRow.Item("Name") = "Ontime Cust Aftermarket Repair"
        dtNewRow.Item("Value") = ds2.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Total Shipm to Cust Aftermarket Repair"
        dtNewRow.Item("Value") = ds.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "OTP Cust Aftermarket Repair"
        Try
            dtNewRow.Item("Percentage") = ds2.Tables(0).Rows(0).Item(0) / ds.Tables(0).Rows(0).Item(0)
        Catch ex As Exception

        End Try
        dt.Rows.Add(dtNewRow)

        'Service Cust


        ds = db.Query("SELECT COUNT(*) number  FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate()) AND [repairPrice] <> 0  AND CUSTOMER_TYPE_CODE = 'R' AND substring(CONVERT(varchar,CONVERT(integer,order_number)),2,1) = '7' ")
        ds2 = db.Query("SELECT COUNT(*) number FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate()) AND [repairPrice] <> 0 AND Late = 0  AND CUSTOMER_TYPE_CODE = 'R' AND substring(CONVERT(varchar,CONVERT(integer,order_number)),2,1) = '7'")

        dtNewRow = dt.NewRow()


        dtNewRow.Item("Name") = "Ontime Cust Aftermarket Service"
        dtNewRow.Item("Value") = ds2.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Total Shipm to Cust Aftermarket Service"
        dtNewRow.Item("Value") = ds.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "OTP Cust Aftermarket Service"
        Try
            dtNewRow.Item("Percentage") = ds2.Tables(0).Rows(0).Item(0) / ds.Tables(0).Rows(0).Item(0)
        Catch ex As Exception

        End Try
        dt.Rows.Add(dtNewRow)


        'OE Customer

        ds = db.Query("SELECT COUNT(*) number  FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate()) AND ([completePrice] <> 0 OR [driverPrice] <> 0 ) AND CUSTOMER_TYPE_CODE = 'R'")
        ds2 = db.Query("SELECT COUNT(*) number FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate()) AND ([completePrice] <> 0 OR [driverPrice] <> 0 ) AND Late = 0  AND CUSTOMER_TYPE_CODE = 'R'")

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Ontime Cust Original Equipment"
        dtNewRow.Item("Value") = ds2.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Total Shipm to Cust Original Equipment"
        dtNewRow.Item("Value") = ds.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "OTP Cust Original Equipment"
        Try
            dtNewRow.Item("Percentage") = ds2.Tables(0).Rows(0).Item(0) / ds.Tables(0).Rows(0).Item(0)
        Catch ex As Exception

        End Try
        dt.Rows.Add(dtNewRow)


        'Interco OTP

        ds = db.Query("SELECT COUNT(*) number  FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate())  AND CUSTOMER_TYPE_CODE = 'I'")
        ds2 = db.Query("SELECT COUNT(*) number FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate())  AND Late = 0  AND CUSTOMER_TYPE_CODE = 'I'")

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Ontime Intercompany"
        dtNewRow.Item("Value") = ds2.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Total Shipm to Intercompany"
        dtNewRow.Item("Value") = ds.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "OTP Intercompany"
        Try
            dtNewRow.Item("Percentage") = ds2.Tables(0).Rows(0).Item(0) / ds.Tables(0).Rows(0).Item(0)
        Catch ex As Exception

        End Try
        dt.Rows.Add(dtNewRow)

        'Interco Original Equipment
        ds = db.Query("SELECT COUNT(*) number  FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate())  AND CUSTOMER_TYPE_CODE = 'I' AND ([completePrice] <> 0 OR [driverPrice] <> 0 )")
        ds2 = db.Query("SELECT COUNT(*) number FROM BBBBillingsAggregatView WHERE datepart(year,[Day]) >= datepart(year,getdate())  AND Late = 0  AND CUSTOMER_TYPE_CODE = 'I' AND ([completePrice] <> 0 OR [driverPrice] <> 0 )")

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Ontime Intercompany Original Equipment"
        dtNewRow.Item("Value") = ds2.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "Total Shipm Intercompany Original Equipment"
        dtNewRow.Item("Value") = ds.Tables(0).Rows(0).Item(0)
        dt.Rows.Add(dtNewRow)

        dtNewRow = dt.NewRow()

        dtNewRow.Item("Name") = "OTP Intercompany Original Equipment"
        Try
            dtNewRow.Item("Percentage") = ds2.Tables(0).Rows(0).Item(0) / ds.Tables(0).Rows(0).Item(0)
        Catch ex As Exception

        End Try
        dt.Rows.Add(dtNewRow)


        dgvOTP.DataSource = dt

    End Sub

    Private Sub dgvOTP_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvOTP.CellFormatting
        'every second row will become beige
        If e.RowIndex Mod 2 = 0 Then
            e.CellStyle.BackColor = Color.Beige
        End If
        If e.ColumnIndex > 0 AndAlso e.RowIndex > 0 Then
            If dgvOTP.Columns(e.ColumnIndex).Name.Equals("Percentage") AndAlso Not DBNull.Value.Equals(e.Value) Then
                e.Value = FormatPercent(e.Value, 2, TriState.True, TriState.True, TriState.True)
            End If

        End If
    End Sub

   
End Class
