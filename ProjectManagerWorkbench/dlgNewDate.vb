Imports System.Windows.Forms

Public Class dlgNewDate
    Private datSource As DataTable
    Private cName As String
    Private rwIndex As Integer
    Private newID As Long
    Private entity As String
	Private headerNo As String
	Private lineNo As Double
	Private organizationID As Double
	Private releaseNo As String
	Private shipmentNo As String
    Private m_poPromiseDateCell As DataGridViewCell
    Private m_dateToChange As Date
    Private m_form As frmWorkbenchProjects
    '*** Add more fields for the purchasing department 
    '4.  AB then simulates the user clicking on the comment.  The comment dialog should prompt the user "Please Enter Will Ship Date, Ship Qty, Carrier and Who Told You. 
    '         Example: "w/s 12/26 4pc UPS per Laura"

    Public Sub New(ByVal oldValue As String, ByRef datTable As DataTable, ByVal columnName As String, ByVal rowIndex As Integer, ByVal entity As String _
    , ByVal headerNo As String, ByVal lineNo As Double, ByVal organizationID As Integer, ByRef formMe As frmWorkbenchProjects, Optional ByVal releaseNumber As String = "0", Optional ByVal shipmentNumber As String = "0")

        InitializeComponent()

        'All values required to overwrite the old value
        Me.m_form = formMe
        Me.txtOldDate.Text = oldValue
        datSource = datTable
        rwIndex = rowIndex
        cName = columnName
        Me.entity = entity
        Me.headerNo = headerNo
        Me.lineNo = lineNo
        Me.organizationID = organizationID
        Me.releaseNo = releaseNumber
        Me.shipmentNo = shipmentNumber

    End Sub


    ''' <summary>
    ''' Saves the transaction to the sql server
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub saveTransactionToBackend()
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As New DataSet
        ds = db.Query("SELECT ISNULL(MAX(transactionID), 0) + 1 FROM warehouseTransactions")
        Me.newID = ds.Tables(0).Rows(0).Item(0)

        'Setting up the values for the items
        Dim oldDate As String
        If txtOldDate.Text = String.Empty Then
            oldDate = Format(Date.Parse("01.01.2000"), "yyyyMMdd")
        Else
            oldDate = Format(Date.Parse(txtOldDate.Text), "yyyyMMdd")
        End If

        Dim newDate As String
        If txtNewDate.Text = String.Empty Then
            newDate = Format(Date.Parse("01.01.2000"), "yyyyMMdd")
        Else
            newDate = Format(Date.Parse(txtNewDate.Text), "yyyyMMdd")
        End If

		db.SecureInsertQueryParams("INSERT INTO [warehouseTransactions] " & _
		   "([transactionID]            " & _
		   ",[transactionType]        " & _
		   ",[organizationID]           " & _
		   ",[headerNo]                  " & _
		   ",[lineID]                       " & _
		   ",[oldDate]                    " & _
		   ",[newDate]                  " & _
		   ", createdBY                 " & _
		   ", creationDate              " & _
		   ", releaseNumber           " & _
		   ", shipmentNumber )      " & _
		   "VALUES(@1,@2,@3,@4,@5,@6,@7, @8,@9,@10,@11)", Me.newID, Me.entity, Me.organizationID, _
		   Me.headerNo, Me.lineNo, oldDate, newDate, Environment.UserName, Format(Now, "yyyyMMdd HH:mm:ss"), Me.releaseNo, Me.shipmentNo)

    End Sub

    ''' <summary>
    ''' Saves the values to the 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub saveTransactionToSambaShare()
        Dim gridObject As New clsGridOperations
		Dim sTmp As String

		Try
			If Me.entity.ToLower = "poqueueview" Then

				'Dim strFilePath As String = "\\gildv224.flowserve.net\st2dev2_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chppo"
				'Dim strFilePath As String = "\\gildv224.flowserve.net\st2it2_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chppo"
                'Dim strFilePath As String = "\\gildv218.flowserve.net\st1uat_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chppo"          'Test
                'Dim strFilePath As String = "\\gilap139.flowserve.net\st1prod_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chppo"        'Production
                Dim strFilePath As String = "\\gilis80\sr1prod_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chppo"        'Production


                If Me.releaseNo.ToString = "" Then
                    sTmp = ""
                Else
                    sTmp = Str(Me.releaseNo)
                End If

                ' If CInt(Me.releaseNo.ToString) > 0 Then sTmp = Str(Me.releaseNo) Else sTmp = ""
                'Enable this again for production publish
                gridObject.writePromiseDateToSambaShare(strFilePath, Me.headerNo, Me.lineNo, sTmp, Me.shipmentNo, Format(Date.Parse(txtNewDate.Text), "MM/dd/yyyy"))
			Else
				'Dim strFilePath As String = "\\gildv224.flowserve.net\st2dev2_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chpwo"
				'Dim strFilePath As String = "\\gildv224.flowserve.net\st2it2_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chpwo"
                'Dim strFilePath As String = "\\gildv218.flowserve.net\st1uat_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chpwo"

                'Dim strFilePath As String = "\\gilap139.flowserve.net\st1prod_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chpwo"
                'Dim strFilePath As String = "C:\Temp\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chpwo"
                Dim strFilePath As String = "\\gilis80\sr1prod_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chpwo"

                gridObject.writeJobStartDateToSambaShare(strFilePath, Me.headerNo, Format(Date.Parse(txtNewDate.Text), "MM/dd/yyyy"))
			End If

		Catch ex As Exception
			Dim err As New clsExceptionManagement
			err.createErrorLog("savePOPromiseUpdateTrxToSambaDrive", ex, 1)
		End Try

	End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            'if no new date was entered, close the dialogue
            Dim strDate As String = ""
            If txtNewDate.Text = "" Then
                MessageBox.Show("No date to parse.")
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If

            'Parsing the date and send it to the same share
            'If entity = "POQueueView" Then
            '    For i As Integer = 0 To datSource.Rows.Count
            '        If Not DBNull.Value.Equals(datSource.Rows(i).Item("po_no")) AndAlso Not DBNull.Value.Equals(datSource.Rows(i).Item("line_no")) Then
            '            If datSource.Rows(i).Item("po_no").Value = Me.headerNo AndAlso datSource.Rows(i).Item("line_no").Value = Me.lineNo _
            '        AndAlso datSource.Rows(i).Item("RELEASE_NO").Value = Me.releaseNo _
            '        AndAlso datSource.Rows(i).Item("SHIPMENT_NO").Value = Me.shipmentNo Then
            '                datSource.Rows(i).Item("promised_date").Value = Date.Parse(txtNewDate.Text)
            '            End If
            '        End If
            '    Next
            '    For Each row As System.Data.DataRow In datSource.Rows


            '    Next
            'End If
            Me.m_form.dateToChange = Date.Parse(txtNewDate.Text)
            'Me.m_dateToChange = Date.Parse(txtNewDate.Text)
            ' datSource.Rows(Me.rwIndex).Item(cName) = Date.Parse(txtNewDate.Text)      'This one is causing the redundant confusion or false info on the screen
            'Me.m_poPromiseDateCell.Value = Date.Parse(txtNewDate.Text)

            Me.saveTransactionToBackend()
            Me.saveTransactionToSambaShare()


        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("createPOPromiseDateTransaction", ex, 1)
        End Try

		Me.DialogResult = System.Windows.Forms.DialogResult.OK
		Me.Close()
	End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub mcSelectADate_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mcSelectADate.DateChanged
        Dim d As Date
        'Push everything to monday if the user selects a weekend.
        If e.Start.DayOfWeek = DayOfWeek.Sunday Then
            d = e.Start.AddDays(1)
        Else
            d = e.Start
        End If

        txtNewDate.Text = Format(d, "Short Date")

    End Sub

    Private Sub dlgNewDate_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mcSelectADate.MinDate = Today
        ' mcSelectADate.SetDate(Today.AddDays(500))

    End Sub

    Private Sub mcSelectADate_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles mcSelectADate.MouseDown
        If mcSelectADate.SelectionStart.DayOfYear = Today.DayOfYear AndAlso mcSelectADate.SelectionStart.Year = Today.Year Then
            Dim d As Date
            If mcSelectADate.SelectionStart.DayOfWeek = DayOfWeek.Saturday Then
                ' d = e.Start.AddDays(2)
            ElseIf mcSelectADate.SelectionStart.DayOfWeek = DayOfWeek.Sunday Then
                d = mcSelectADate.SelectionStart.AddDays(1)
            Else
                d = mcSelectADate.SelectionStart
            End If

            txtNewDate.Text = Format(d, "Short Date")
        End If
    End Sub
End Class
