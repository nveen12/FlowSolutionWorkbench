Public Class ucProdCapacity
    Private _organizationId As Integer
    Private language As String
    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)
    Private gridDisplay As New clsGridDisplay
    Private _frmWorkbench As frmWorkbenchProjects



    Public Sub New(ByVal organizationId As Integer, ByRef frmAutobahn As frmWorkbenchProjects)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        _organizationId = organizationId
        _frmWorkbench = frmAutobahn
        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub dgvProdCapacity_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvProdCapacity.CellContentClick


    End Sub



	'assign the production capacity data to a dataset so it can be assigned to a grid
    Private Sub ucProdCapacity_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

        
            Dim ds As New DataSet

            ds = db.SecureQueryParams(" SELECT [ORGANIZATION_ID] " & _
             ",[PLANNER_CODE]  " & _
             ",[DEPARTMENT_CODE]  " & _
             ",[RESOURCE_CODE]  " & _
             ",[FSG_PRODUCT_CODE]  " & _
             ",[FSG_SALES_TYPE]  " & _
             ",[targetEarnedHours]  " & _
             ",[targetEarnedPc]  " & _
             ",[loadPercent]  " & _
             ",[queueHours]  " & _
             ",[queuePc]  " & _
             ",[earnedHoursYest]  " & _
             ",[earnedHoursWeek0]  " & _
             ",[earnedHoursMonth0]  " & _
             ",[prodPcYest]  " & _
             ",[prodPcWeek0]  " & _
             ",[prodPcMonth0]  " & _
             ",[CustJobPcLate]  " & _
             ",[CustJobPcDay0]  " & _
             ",[CustJobPcDay1]  " & _
             ",[CustJobPcWeek0]  " & _
             ",[CustJobPcWeek1]  " & _
             ",[CustJobPcWeek2]  " & _
             ",[CustJobPcWeek3]  " & _
             ",[OtherJobPc]  " & _
             "  FROM [prodCapacity] WHERE organization_id = @1 ", 881)

            dgvProdCapacity.DataSource = ds.Tables(0)

            gridDisplay.formatGrid(dgvProdCapacity, "prodCapacity")
        Catch ex As Exception

        End Try
    End Sub



	'user clicked the 'Hide this form' button
    Private Sub btnHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHide.Click
		Me.Visible = False


    End Sub



	'conditional formatting for Production Capacity grid
	Private Sub dgvProdCapacity_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvProdCapacity.CellFormatting

		Dim lngTmp As Long
		Dim lngTargetPc As Long
		Dim lngCustJobPc As Long
		Dim dblLoad As Double
		Dim dblTmp As Double
		Dim lngQueue As Double
		Dim dtTmp As Date
		Dim sName As String
		Dim sTmp As String

		If e.RowIndex Mod 2 = 0 Then e.CellStyle.BackColor = Color.Beige 'color alternating rows beige 
		sName = dgvProdCapacity.Columns(e.ColumnIndex).Name

		lngTargetPc = 0
		If Not DBNull.Value.Equals(dgvProdCapacity.Rows(e.RowIndex).Cells("targetEarnedPc").Value) Then lngTargetPc = Integer.Parse(dgvProdCapacity.Rows(e.RowIndex).Cells("targetEarnedPc").Value) 'remember the target production qty
		dblLoad = 0
		If Not DBNull.Value.Equals(dgvProdCapacity.Rows(e.RowIndex).Cells("loadPercent").Value) Then dblLoad = Double.Parse(dgvProdCapacity.Rows(e.RowIndex).Cells("loadPercent").Value) 'remember the 4 week load on this resource
		lngQueue = 0
		If Not DBNull.Value.Equals(dgvProdCapacity.Rows(e.RowIndex).Cells("queuePC").Value) Then lngQueue = Integer.Parse(dgvProdCapacity.Rows(e.RowIndex).Cells("queuePC").Value) 'remember the qty of material waiting for this resource

		If sName = "loadPercent" And dblLoad > 0 Then
			If dblLoad > 80 Then e.CellStyle.BackColor = Color.Orange 'if load is more than 80% color orange
			If dblLoad > 120 Then e.CellStyle.BackColor = Color.Red 'if load is greater than 120%, color red
			e.Value = Format(e.Value / 100, "p0") 'format to %, with zero decimals
		End If

		If sName = "queuePc" And lngQueue > 0 Then
			If dblLoad > 50 And lngQueue < lngTargetPc * 0.5 Then e.CellStyle.BackColor = Color.Yellow 'queue is too low
			If lngQueue > lngTargetPc * 5 Then e.CellStyle.BackColor = Color.Orange 'queue is too high
			If lngQueue > lngTargetPc * 10 Then e.CellStyle.BackColor = Color.Red 'queue is really too high
		End If

		If sName = "prodPcYest" AndAlso Not DBNull.Value.Equals(e.Value) Then
			If dblLoad > 80 And e.Value < lngTargetPc * 0.8 Then e.CellStyle.BackColor = Color.Orange 'yesterday we did not produce enough
			If dblLoad > 50 And e.Value < lngTargetPc * 0.5 Then e.CellStyle.BackColor = Color.Red 'we really didn't produce enough!
		End If

		lngTmp = Math.Max(Weekday(Now()) - 2, 1)
		If sName = "prodPcWeek0" AndAlso Not DBNull.Value.Equals(e.Value) Then
			If dblLoad > 80 And e.Value < lngTargetPc * 0.8 * lngTmp Then e.CellStyle.BackColor = Color.Orange 'this week we have not produced enough
			If dblLoad > 50 And e.Value < lngTargetPc * 0.5 * lngTmp Then e.CellStyle.BackColor = Color.Red
		End If

		If sName = "prodPcMonth0" AndAlso Not DBNull.Value.Equals(e.Value) Then
			'calculate number of workdays that have occurred so far this month and indicate if production is below target
			dtTmp = Now.Date.AddDays((Now.Day - 1) * -1)	'first day of current month
			lngTmp = 0
			Do Until dtTmp = Now.Date 'add up how many weekdays have already occurred.  note that it will not execute the loop for today
				If dtTmp.DayOfWeek <> DayOfWeek.Saturday And dtTmp.DayOfWeek <> DayOfWeek.Sunday Then lngTmp += 1
				dtTmp = dtTmp.AddDays(1)
			Loop
			If dblLoad > 80 And e.Value < lngTargetPc * 0.8 * lngTmp Then e.CellStyle.BackColor = Color.Orange 'this month we have not produced enough
			If dblLoad > 50 And e.Value < lngTargetPc * 0.5 * lngTmp Then e.CellStyle.BackColor = Color.Red
		End If

		'calculate if the load on each resource exceeds capacity for Week0,1,2,3.  include total prior demand so that we can see how many weeks we are in excess of capacity
		If lngTargetPc > 0 And sName.Substring(0, Len(sName) - 1) = "CustJobPcWeek" Then
			lngCustJobPc = 0
			If Not DBNull.Value.Equals(dgvProdCapacity.Rows(e.RowIndex).Cells("CustJobPcLate").Value) Then lngCustJobPc = Integer.Parse(dgvProdCapacity.Rows(e.RowIndex).Cells("CustJobPcLate").Value) 'remember the late qty
			lngTmp = Val(sName.Substring(Len(sName) - 1, 1)) 'determine which week we are displaying
			For x = 0 To lngTmp
				sTmp = "CustJobPcWeek" & Trim(Str(x))
				If Not DBNull.Value.Equals(dgvProdCapacity.Rows(e.RowIndex).Cells(sTmp).Value) Then lngCustJobPc += Integer.Parse(dgvProdCapacity.Rows(e.RowIndex).Cells(sTmp).Value) 'add each additional week's demand
			Next
			dblTmp = lngCustJobPc / (lngTargetPc * 5 * (lngTmp + 1)) 'determine the cumulative load for this week (considering carryover from late orders and prior week excess)
			If dblTmp > 0.8 Then e.CellStyle.BackColor = Color.Orange 'if in excess of 80% capacity, give warning
			If dblTmp > 1.2 Then e.CellStyle.BackColor = Color.Red 'give stronger warning
		End If

		'calculate if there is more than 1 week's worth of late jobs
		If lngTargetPc > 0 And sName = "CustJobPcLate" Then
			lngCustJobPc = 0
			If Not DBNull.Value.Equals(dgvProdCapacity.Rows(e.RowIndex).Cells("CustJobPcLate").Value) Then lngCustJobPc = Integer.Parse(dgvProdCapacity.Rows(e.RowIndex).Cells("CustJobPcLate").Value) 'remember the late qty
			dblTmp = lngCustJobPc / (lngTargetPc * 5) 'determine the cumulative load for this week (considering carryover from late orders and prior week excess)
			If dblTmp > 1 Then e.CellStyle.BackColor = Color.Orange 'if in excess of 80% capacity, give warning
			If dblTmp > 2 Then e.CellStyle.BackColor = Color.Red 'give stronger warning
		End If

	End Sub



	'user clicked a cell
	Private Sub dgvProdCapacity_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvProdCapacity.CellClick
		If Not e.ColumnIndex = -1 AndAlso Not e.RowIndex = -1 Then
			If dgvProdCapacity.Columns(e.ColumnIndex).Name = "PLANNER_CODE" AndAlso Not DBNull.Value.Equals(dgvProdCapacity.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then 'user wants to jump to the first job with this planner code in the WIP grid
				Me._frmWorkbench.filterBottonWipGrid = " AND PLANNER_CODE = '" & dgvProdCapacity.Rows(e.RowIndex).Cells(e.ColumnIndex).Value & "'"
				Me._frmWorkbench.Show()
			End If
		End If

	End Sub

	Private Sub dgvProdCapacity_ColumnWidthChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvProdCapacity.ColumnWidthChanged
		'Distinguish when the user really changes the column width versus the normal backend reload process
		If Me.gridDisplay.reloading Then  Else gridDisplay.saveChangesOfSingleColumn(e.Column.Name, "prodCapacity", e.Column.Width)

	End Sub

	Private Sub ExportToXMLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToXMLToolStripMenuItem.Click
		sfdExcel.Tag = "prodCapacity"
		sfdExcel.ShowDialog()

	End Sub

	Private Sub SaveOrderOfColumnsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveOrderOfColumnsToolStripMenuItem.Click
		For i As Integer = 0 To dgvProdCapacity.Columns.Count - 1
			gridDisplay.saveChangesOfSingleColumnDisplayIndex(dgvProdCapacity.Columns(i).Name, "prodCapacity", dgvProdCapacity.Columns(i).DisplayIndex, dgvProdCapacity)
		Next
		gridDisplay.loadColumnSettings()
		MessageBox.Show("Changes saved succesfully.")
	End Sub


	Private Sub sfdExcel_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles sfdExcel.FileOk

		Dim gridOperation As New clsGridOperations
		gridOperation.exportExcel(dgvProdCapacity, sfdExcel.FileName)

	End Sub

End Class
