Imports System.Windows.Forms
Imports System.Text

Public Class dlgForecastDates
    Private _header_id As Integer
    Private _headerName As String
    Private _line_id As Integer
    Private _shipment_no As Double
    Private _entity_name As String
    Private _dgv As DataGridView
    Private _row As Integer
    Private _organizationId As Integer
    Private _releaseNo As Integer
    Private strJobNumber As String
    Private m_PrintBitmap As Bitmap
    Private newDate As Date
    Private oldDate As Date


    Private _new_key As Integer
    Private _newKeyNotification As Integer
    '   Private mbrComments As brComments
    Private db As New DB.ServerDB(My.Settings("FlowConnectionString"))

    Private _setOfComments As DataSet
    Public Sub New(ByVal header_id As Double, ByVal headerName As String, ByVal line_id As Double, ByVal shipmentNo As Double, ByVal entity_name As String, ByVal dgv As DataGridView, ByVal row As Integer, ByVal organizationID As Integer, Optional ByVal releaseNo As Integer = 0, Optional ByVal jobNumber As String = "")

        'Storing the variables within the object
        _header_id = header_id
        _line_id = line_id
        _entity_name = entity_name
        _dgv = dgv
        _row = row
        _organizationId = organizationID
        _headerName = headerName
        _shipment_no = shipmentNo
        _releaseNo = releaseNo
        Me.strJobNumber = jobNumber

        InitializeComponent()
        '
        Me.loadForecasts()


    End Sub

    Private Sub loadForecasts()

        _setOfComments = db.SecureQueryParams("SELECT co.forecastDate, co.[description], co.creation_date, co.created_by, wun.notificationTo " & _
                                             " , wu1.firstName firstNameCreation, wu1.lastName lastNameCreation " & _
                                             " , wu2.firstName firstNameDestination, wu2.lastName lastNameDestination, wu1.eMail as eMailFrom, wu2.eMail as eMailTo, wun.reviewed " & _
                                             " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                             " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                             " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                             " WHERE co.header_id = @1 AND co.line_id = @2 AND co.entity_id = @3 AND co.shipment_no = @4 AND (co.releaseNo = @5 OR co.releaseNo IS NULL) AND co.organizationid =@6 AND forecast = 1 ORDER BY co.serverCreationDate", _header_id, _line_id, _entity_name, _shipment_no, _releaseNo, _organizationId)

        Dim strComment As String = ""
        For i As Integer = 0 To _setOfComments.Tables(0).Rows.Count - 1
            strComment += _setOfComments.Tables(0).Rows(i).Item("firstNameCreation").ToString & " " & _setOfComments.Tables(0).Rows(i).Item("lastNameCreation").ToString & " on "

            'Format the date accordingly
            If Not DBNull.Value.Equals(_setOfComments.Tables(0).Rows(i).Item("creation_date")) Then

                Dim dateNow As Date = _setOfComments.Tables(0).Rows(i).Item("creation_date")
                strComment += dateNow.ToShortDateString & ": "

            End If


            strComment += _setOfComments.Tables(0).Rows(i).Item("description") + vbCr

        Next

        If _setOfComments.Tables(0).Rows.Count > 0 Then
            mc0.SetDate(_setOfComments.Tables(0).Rows(_setOfComments.Tables(0).Rows.Count - 1).Item("forecastDate"))
        End If

        rtbForecastHistory.Text = strComment
    End Sub

    Private Sub loadOldReasons()
        Dim ds As New DataSet
        'Will give you not the pure reason!
        ds = db.SecureQueryParams("SELECT TOP 100 description FROM comments WHERE CREATED_BY = @1 AND forecast = 1 ORDER BY CREATION_DATE DESC", Environment.UserName)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            txtReason.AutoCompleteCustomSource.Add(ds.Tables(0).Rows(i).Item("description"))
        Next
    End Sub

    ''' <summary>
    ''' New key from the backend.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getNewKey()

        Dim ds As New DataSet
        ds = db.Query("SELECT MAX(commentId) + 1 as NewKey FROM comments")
        _new_key = ds.Tables(0).Rows(0).Item(0)

    End Sub
    Private Sub saveNewForecast(Optional ByVal completeOrder As Boolean = False)
        Try
            Dim sb As New StringBuilder
            Dim ds As New DataSet
            Dim ds1 As New DataSet
            Dim strComment As String

            If Not cbComplete.Checked Then           'Normal way of saving is that for one order line
                Me.getNewKey()
                strComment = "Forecasted for: " & FormatDateTime(Me.newDate, DateFormat.ShortDate)
                If txtReason.Text <> "" Then
                    strComment += "  Because of: " & txtReason.Text
                End If
                db.SecureNonQueryParams("INSERT INTO comments ([entity_id], [header_id], [line_id], description, [creation_date], commentId, [created_by], headerName, organizationID, shipment_no, releaseNO, serverCreationDate, forecast, forecastDate) " & _
                                          " VALUES (@1, @2, @3, @4,@5, @6, @7, @8, @9, @10, @11, getDate(),1,@12)", _entity_name, _header_id, _line_id, strComment, Format(Now, "yyyyMMdd HH:mm:ss"), _new_key, Environment.UserName, _headerName, Me._organizationId, Me._shipment_no, _releaseNo, Me.newDate)
                Me._dgv.Rows(Me._row).Cells("lastForecast").Value = Me.newDate

            Else                            'if we want to save it for the complete order we need to apply the following: Open the order, all lines, iterate into the lines and apply the comment to all lines
                Dim dsLines As DataSet = db.SecureQueryParams("SELECT header_id, line_no, order_no  FROM salesOrderLinesView WHERE organization_id = @1 AND header_id = @2", Me._organizationId, Me._header_id)

                'Comments are the same for every single line
                strComment = "Forecasted for: " & FormatDateTime(Me.newDate, DateFormat.ShortDate)
                If txtReason.Text <> "" Then
                    strComment += "  Because of: " & txtReason.Text
                End If

                'Iterate over the possible lines and set the values to the new date
                For j As Integer = 0 To dsLines.Tables(0).Rows.Count - 1
                    Me.getNewKey()

                    db.SecureNonQueryParams("INSERT INTO comments ([entity_id], [header_id], [line_id], description, [creation_date], commentId, [created_by], headerName, organizationID, shipment_no, releaseNO, serverCreationDate, forecast, forecastDate) " & _
                                              " VALUES (@1, @2, @3, @4,@5, @6, @7, @8, @9, @10, @11, getDate(),1,@12)", _entity_name, dsLines.Tables(0).Rows(j).Item("header_id"), dsLines.Tables(0).Rows(j).Item("line_no"), strComment, Format(Now, "yyyyMMdd HH:mm:ss"), _new_key, Environment.UserName, dsLines.Tables(0).Rows(j).Item("order_no"), Me._organizationId, Me._shipment_no, _releaseNo, Me.newDate)

                Next
            End If

            'Send email when notification changes
            ds = db.SecureQueryParams("SELECT eMail FROM warehouse_users WHERE firstName + ' ' + lastName like (" & _
                                        "SELECT attribute1 FROM salesOrderLinesView WHERE header_id = @1 and line_no = @2 and organization_id = @3)", _header_id, _line_id, Me._organizationId)
            ds1 = db.SecureQueryParams("SELECT firstName, lastName FROM warehouse_users WHERE user_Name = @1", Environment.UserName)

            If ds.Tables(0).Rows.Count > 0 Then
                If ds.Tables(0).Rows(0).Item(0).ToString() <> "" Then
                    sb.Append("EXEC msdb.dbo.sp_send_dbmail @recipients='" & ds.Tables(0).Rows(0).Item(0).ToString() & "',")
                    If _organizationId = 881 Then
                        sb.Append("@subject = 'Build Date Change',")
                    Else
                        sb.Append("@subject = 'Forecast Change',")
                    End If
                    sb.Append("@body = '" & emailBody(ds1.Tables(0).Rows(0).Item("firstName").ToString() & " " & ds1.Tables(0).Rows(0).Item("lastName").ToString(), strComment) & "', ")
                    sb.Append("@body_format = 'HTML'")

                    db.SecureNonQueryParams(sb.ToString())

                    db.SecureNonQueryParams("UPDATE comments SET emailSent = 1, emailSentDate = GETDATE() WHERE commentId = @1", _new_key)
                End If
            Else
                MessageBox.Show("Users with no match for this Contact Name, unable to send email.")
            End If

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("saveNewForecast", ex, 1)
        End Try

    End Sub
    ''' <summary>
    ''' Create email body with specific table format
    ''' </summary>
    ''' <param name="notificationFrom">Notification sent from</param>
    ''' <param name="strComment">New comment inserted</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function emailBody(ByVal notificationFrom As String, ByVal strComment As String) As String
        Dim sb As New StringBuilder

        sb.Append("<table border=""0"" cellpadding=""0"" style=""background: #6699FF; mso-cellspacing: 1.5pt; mso-yfti-tbllook: 1184; mso-padding-alt: 0in 0in 0in 0in"" Width = ""500"" > ")
        sb.Append("<tr><td colspan=2 style=""background: white; padding: .75pt .75pt .75pt .75pt"">Autobahn – Notification E-Mail</td></tr>")
        sb.Append("<tr><td>Notified From</td><td>" & notificationFrom & "</td></tr>")
        sb.Append("<tr><td style=""background: white; padding: .75pt .75pt .75pt .75pt"">Message</td><td style=""background: white; padding: .75pt .75pt .75pt .75pt"">New Comment: " & strComment & "</td></tr>")
        sb.Append("<tr><td>Notified For</td><td>Sales Order: " & _headerName & "_" & _line_id & "</td></tr>")
        sb.Append("<tr><td style=""background: white; padding: .75pt .75pt .75pt .75pt"">Date</td><td style=""background: white; padding: .75pt .75pt .75pt .75pt"">" & DateTime.Now.ToShortDateString() & "</td></tr>")
        sb.Append("</table>")

        Return sb.ToString()
    End Function
    Private Sub saveBuildDateToSambaShare()
        'Check if the user has the role
        Dim roles As New clsUserControl()
        If Not roles.userInRole(Environment.UserName, "Planning") Then
            Exit Sub
        End If
        'Check if the user is in this organization home.
        If Me._organizationId <> db.SecureQueryParams("SELECT userDefaultOrganization FROM warehouse_users WHERE user_name = @1", Environment.UserName).Tables(0).Rows(0).Item(0) Then
            Exit Sub
        End If
        'Write the file!
        Dim gridObject As New clsGridOperations
        'Dim strFilePath As String = "\\gilap139.flowserve.net\st1prod_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chpwo"
        Dim strFilePath As String = "\\gilis80\sr1prod_chp_int\chp\inbound\" & "[" & Environment.UserName & "]_[" & Format(Now, "yyyyMMdd_HH_mm_ss") & "].chpwo"

        If Me.strJobNumber <> "" Then   'Only create transaction when the job number was passed over
            '  gridObject.writeJobStartDateToSambaShare(strFilePath, Me.strJobNumber, Format(Date.Parse(txtNewDate.Text), "MM/dd/yyyy"), ", BUILD_DATE", ", " & Format(Date.Parse(txtNewDate.Text), "MM/dd/yyyy").ToString)
        End If

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If txtNewDate.Text <> "" Then
            Me.saveNewForecast()
            If Me._organizationId = 881 Then
                '   Me.saveBuildDateToSambaShare()
            End If
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private bload As Boolean = True
    Private Sub dlgForecastDates_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Text += " Order:" & Me._headerName & " Line:" & Me._line_id

        mc1.SetDate(DateAdd(DateInterval.Day, 30, mc0.SelectionRange.Start))

        mc1.SetDate(Nothing)

        bload = False

    End Sub

    Private Sub mc0_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mc0.DateChanged

        txtNewDate.Text = FormatDateTime(mc0.SelectionRange.Start, DateFormat.ShortDate)
        Me.newDate = mc0.SelectionRange.Start

    End Sub

    Private Sub mc1_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles mc1.DateChanged

        If Not bload Then

            txtNewDate.Text = FormatDateTime(mc1.SelectionRange.Start, DateFormat.ShortDate)
            Me.newDate = mc1.SelectionRange.Start

        End If

    End Sub

  
End Class
