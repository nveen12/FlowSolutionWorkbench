Imports System.Windows.Forms
Imports System.Drawing
Imports Outlook = Microsoft.Office.Interop.Outlook
Imports System.Text

'Imports FlowserveDBO.clsField.brFieldTypeConstants

Public Class dlgComments

    Private _header_id As Integer
    Private _headerName As String
    Private _line_id As Integer
    Private _shipment_no As Double
    Private _entity_name As String
    Private _dgv As DataGridView
    Private _row As Integer
    Private _organizationId As Integer
    Private _releaseNo As Integer
    Private m_PrintBitmap As Bitmap
    Private m_itemNo As String = ""
    Private m_itemDescription As String = ""


    Private _new_key As Integer
    Private _newKeyNotification As Integer
    '   Private mbrComments As brComments
    Private db As New DB.ServerDB(My.Settings("FlowConnectionString"))

    Private _setOfComments As DataSet
    ' List holds the properties of the selected cells
    Private _listSelectedCells As New List(Of clsSelectedCells)

    Public Sub New(ByVal header_id As Double, ByVal headerName As String, ByVal line_id As Double, ByVal shipmentNo As Double, ByVal entity_name As String, ByVal dgv As DataGridView, ByVal row As Integer, ByVal organizationID As Integer, Optional ByVal releaseNo As Integer = 0)

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
        Try
            m_itemNo = dgv.Rows(row).Cells("item_no").Value.ToString

            If entity_name = "salesOrderLinesView" Or entity_name = "WIPQueueView" Then
                m_itemDescription = dgv.Rows(row).Cells("description").Value.ToString
            ElseIf entity_name = "POQueueView" Then
                m_itemDescription = dgv.Rows(row).Cells("item_description").Value.ToString

            End If

        Catch ex As Exception

        End Try

        Try
            If entity_name = "salesOrderLines" Then

                If dgv.Rows(row).Cells("item_description").ToString <> "" Then
                    m_itemDescription = dgv.Rows(row).Cells("item_description").ToString
                Else
                    m_itemDescription = dgv.Rows(row).Cells("description").ToString
                End If
            ElseIf entity_name = "POQueueView" Then
                If dgv.Rows(row).Cells("item_description").ToString <> "" Then
                    m_itemDescription = dgv.Rows(row).Cells("item_description").Value.ToString
                End If
            End If

        Catch ex As Exception

        End Try

        ' This call is required by the Windows Form Designer.
        ' Add any initialization after the InitializeComponent() call.
        InitializeComponent()

        ' Load the values only when single line is selected
        If _listSelectedCells.Count <= 1 Then
            loadComments()
            loadOldComments()
            loadUsersInOrganization()

            txtComment.Select()
            txtComment.Focus()

            Dim strText As String = ""
            If Me._entity_name = "salesOrderLinesView" Then
                strText = " for Sales Order: "
            ElseIf Me._entity_name = "WIPQueueView" Then
                strText = " for Operation Sequence: "
            ElseIf Me._entity_name = "POQueueView" Then
                strText = " for Purchase Order Line: "

            End If
            strText += headerName & "_" & line_id

            Me.Text += strText
        End If
    End Sub
    ' Overlaoded constructor for loading comments when multiple lines selected
    Public Sub New(ByVal listSelectedCells As List(Of clsSelectedCells))
        _listSelectedCells = listSelectedCells
        InitializeComponent()
        Dim dlg As New dlgComments(listSelectedCells(0).header_id, listSelectedCells(0).headerName, listSelectedCells(0).line_id, listSelectedCells(0).shipment_no, listSelectedCells(0).entity_name, listSelectedCells(0).dgv, listSelectedCells(0).row, listSelectedCells(0).organizationId, listSelectedCells(0).releaseNo)
        cbDirect.Enabled = False
        loadOldComments()
    End Sub
    Private Sub loadUsersInOrganization()
        Try

            Dim ds As New DataSet
            ds = db.SecureQueryParams("SELECT user_Name as userid, user_Name, firstName, lastName, outOfOffice  ,outUnitilDate, outFromDate, eMail FROM warehouse_users WHERE (standard_organization_id = @1 OR userDefaultOrganization = @1) AND (enabled IS NULL OR enabled = 1) ORDER BY lastName, firstName", Me._organizationId)

            'When first and last name exist it will look like: Nils, Ruemmeli not the windows user Name: NRuemmeli
            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("firstName")) Then
                    If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("outOfOffice")) AndAlso ds.Tables(0).Rows(i).Item("outOfOffice") AndAlso ds.Tables(0).Rows(i).Item("outFromDate") < Today.AddDays(1) Then
                        ds.Tables(0).Rows(i).Item("user_Name") = ds.Tables(0).Rows(i).Item("lastName") + ", " + ds.Tables(0).Rows(i).Item("firstName") + " Return: " + FormatDateTime(ds.Tables(0).Rows(i).Item("outUnitilDate"), DateFormat.ShortDate)
                    Else
                        ds.Tables(0).Rows(i).Item("user_Name") = ds.Tables(0).Rows(i).Item("lastName") + ", " + ds.Tables(0).Rows(i).Item("firstName")
                    End If
                End If
            Next

            'ds.Tables(0).DefaultView.Sort = "user_Name"

            cbWarehouseUsers.DataSource = ds.Tables(0)
            cbWarehouseUsers.DisplayMember = "user_Name"
            cbWarehouseUsers.ValueMember = "userid"

        Catch ex As Exception

        End Try

    End Sub

    Private Sub loadComments()

        ' _setOfComments = db.SecureQueryParams("SELECT description, creation_date, created_by FROM comments WHERE header_id = @1 AND line_id = @2 AND entity_id = @3", _header_id, _line_id, _entity_name)
        _setOfComments = db.SecureQueryParams("SELECT co.[description], co.creation_date, co.created_by, wun.notificationTo " &
                                              " , wu1.firstName firstNameCreation, wu1.lastName lastNameCreation " &
                                              " , wu2.firstName firstNameDestination, wu2.lastName lastNameDestination, wu1.eMail as eMailFrom, wu2.eMail as eMailTo, wun.reviewed " &
                                              " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " &
                                              " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " &
                                              " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " &
                                              " WHERE co.header_id = @1 AND co.line_id = @2 AND co.entity_id = @3 AND co.shipment_no = @4 AND (co.releaseNo = @5 OR co.releaseNo IS NULL) AND co.organizationid =@6 AND forecastDate is null  ORDER BY co.serverCreationDate", _header_id, _line_id, _entity_name, _shipment_no, _releaseNo, _organizationId)

        Dim com As New clsComments(Me.rtbText, _setOfComments.Tables(0), Me)
        'Set the last comment into the textbox
        If _setOfComments.Tables(0).Rows.Count > 0 Then
            txtComment.Text = _setOfComments.Tables(0).Rows(_setOfComments.Tables(0).Rows.Count - 1).Item("description")
        End If

    End Sub



    Private Sub loadOldComments()
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT TOP 2000 description description FROM comments WHERE CREATED_BY = @1 GROUP BY description ORDER BY description DESC", Environment.UserName)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            txtComment.AutoCompleteCustomSource.Add(ds.Tables(0).Rows(i).Item("description"))
        Next
    End Sub

    Private Sub getNewKey()

        Dim ds As New DataSet

        ds = db.Query("SELECT MAX(commentId) + 1 as NewKey FROM comments")
        _new_key = ds.Tables(0).Rows(0).Item(0)

    End Sub

    Private Sub getNewKeyNotification()

        Dim ds As New DataSet

        ds = db.Query("SELECT ISNULL(MAX(notificationId),0) + 1 as NewKey FROM warehouseUserNotifications")
        _newKeyNotification = ds.Tables(0).Rows(0).Item(0)

    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            'Save the new comment, when it differs from the last comment in the data set
            'If there is a privious comment existing

            If _listSelectedCells.Count > 1 Then
                ' To update multiple lines selected
                For Each item As clsSelectedCells In _listSelectedCells

                    _header_id = item.header_id
                    _line_id = item.line_id
                    _entity_name = item.entity_name
                    _dgv = item.dgv
                    _row = item.row
                    _organizationId = item.organizationId
                    _headerName = item.headerName
                    _shipment_no = item.shipment_no
                    _releaseNo = item.releaseNo

                    _setOfComments = db.SecureQueryParams("SELECT co.[description], co.creation_date, co.created_by, wun.notificationTo " & _
                                                  " , wu1.firstName firstNameCreation, wu1.lastName lastNameCreation " & _
                                                  " , wu2.firstName firstNameDestination, wu2.lastName lastNameDestination, wu1.eMail as eMailFrom, wu2.eMail as eMailTo, wun.reviewed " & _
                                                  " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                                  " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                                  " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                                  " WHERE co.header_id = @1 AND co.line_id = @2 AND co.entity_id = @3 AND co.shipment_no = @4 AND (co.releaseNo = @5 OR co.releaseNo IS NULL) AND co.organizationid =@6  ORDER BY co.serverCreationDate", _header_id, _line_id, _entity_name, _shipment_no, _releaseNo, _organizationId)

                    If _setOfComments.Tables(0).Rows.Count > 0 Then
                        If txtComment.Text <> _setOfComments.Tables(0).Rows(_setOfComments.Tables(0).Rows.Count - 1).Item("description") Then
                            Me.saveNewComment()
                        End If
                    Else
                        Me.saveNewComment()
                    End If
                Next
            Else
                ' To update single line selected
                If _setOfComments.Tables(0).Rows.Count > 0 Then
                    If txtComment.Text <> _setOfComments.Tables(0).Rows(_setOfComments.Tables(0).Rows.Count - 1).Item("description") Then
                        Me.saveNewComment()
                    End If
                Else
                    Me.saveNewComment()
                End If
            End If
            
            Me.Close()
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("dlgDiscussionSave", ex, 1)

        End Try


    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub saveNewComment()
        Try
            Dim strComment As String = txtComment.Text
            Dim dt As DataTable = cbWarehouseUsers.DataSource
            Dim ds As New DataSet
            Dim ds1 As New DataSet
            Dim sb As New StringBuilder

            'Add the out of office status to the comment when a user is select who is currently out 
            If cbDirect.Checked Then
                If Not DBNull.Value.Equals(dt.Rows(cbWarehouseUsers.SelectedIndex).Item("OutOfOffice")) Then    'Avoid the dbnull value
                    If dt.Rows(cbWarehouseUsers.SelectedIndex).Item("OutOfOffice") AndAlso dt.Rows(cbWarehouseUsers.SelectedIndex).Item("outFromDate") < Today.AddDays(1) Then
                        strComment = "Out of Office until:" & FormatDateTime(dt.Rows(cbWarehouseUsers.SelectedIndex).Item("outUnitilDate"), DateFormat.ShortDate) & " " & txtComment.Text
                    End If
                End If
            End If


            Me.getNewKey()

            db.SecureNonQueryParams("INSERT INTO comments ([entity_id], [header_id], [line_id], description, [creation_date], commentId, [created_by], headerName, organizationID, shipment_no, releaseNO, serverCreationDate) " & _
                                    " VALUES (@1, @2, @3, @4,@5, @6, @7, @8, @9, @10, @11, getDate())", _entity_name, _header_id, _line_id, strComment, Format(Now, "yyyyMMdd HH:mm:ss"), _new_key, Environment.UserName, _headerName, Me._organizationId, Me._shipment_no, _releaseNo)

            'For wip transactions this seems to be a sales order grid
            Try
                'here we are getting false advise for some occasions.
                'Like when the grid was sorted
                _dgv.Rows(_row).Cells("lastComment").Value = txtComment.Text
            Catch ex As Exception

            End Try



            Me.getNewKeyNotification()


            'Enter notification for a user
            If cbDirect.Checked Then
                If cbWarehouseUsers.SelectedIndex = -1 Then
                    MessageBox.Show("Please select a user of the current organization you want to inform.")
                End If

                db.SecureNonQueryParams("INSERT INTO warehouseUserNotifications (notificationId, createdBy, notificationEntity, creationDate, headerID, lineID, reviewed, notificationTo, commentId, organizationID) " & _
                                        " VALUES (@1, @2, @3, @4, @5, @6, 0, @7, @8, @9)", _newKeyNotification, Environment.UserName, _entity_name, Format(Now, "yyyyMMdd HH:mm:ss"), _header_id, _line_id, cbWarehouseUsers.SelectedValue, _new_key, Me._organizationId)

                'Send email when notification changes
                ds = db.SecureQueryParams("SELECT eMail FROM warehouse_users WHERE user_Name = @1", cbWarehouseUsers.SelectedValue)
                ds1 = db.SecureQueryParams("SELECT firstName, lastName FROM warehouse_users WHERE user_Name = @1", Environment.UserName)

                If ds.Tables(0).Rows.Count > 0 Then
                    If ds.Tables(0).Rows(0).Item(0).ToString() <> "" Then
                        sb.Append("EXEC msdb.dbo.sp_send_dbmail @recipients='" & ds.Tables(0).Rows(0).Item(0).ToString() & "',")
                        sb.Append("@subject = 'Notification Change',")
                        sb.Append("@body = '" & emailBody(ds1.Tables(0).Rows(0).Item("firstName").ToString() & " " & ds1.Tables(0).Rows(0).Item("lastName").ToString(), strComment, m_itemNo, m_itemDescription) & "', ")
                        sb.Append("@body_format = 'HTML'")

                        db.SecureNonQueryParams(sb.ToString())

                        db.SecureNonQueryParams("UPDATE comments SET emailSent = 1, emailSentDate = GETDATE() WHERE commentId = @1", _new_key)
                    End If
                Else
                    MessageBox.Show("Users with no match for this Contact Name, unable to send email.")
                End If

            End If

            Me.getNewKeyNotification()
            'Enter assingment for a user - This name then appears also on the frontend
            If cbAssign.Checked Then
                If cbWarehouseUsers.SelectedIndex = -1 Then
                    MessageBox.Show("Please select a user of the current organization you want to assign.")
                Else
                    db.SecureNonQueryParams("INSERT INTO warehouseUserNotifications (notificationId, createdBy, notificationEntity, creationDate, headerID, lineID, reviewed, notificationTo, commentId, organizationID, assignment) " & _
                                     " VALUES (@1, @2, @3, @4, @5, @6, 0, @7, @8, @9, 1)", _newKeyNotification, Environment.UserName, _entity_name, Format(Now, "yyyyMMdd HH:mm:ss"), _header_id, _line_id, cbWarehouseUsers.Text, _new_key, Me._organizationId)
                    _dgv.Rows(_row).Cells("lastAssignment").Value = cbWarehouseUsers.Text
                End If
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("saveNewComment", ex, 1)
        End Try

    End Sub
    ''' <summary>
    ''' Create email body with specific table format
    ''' </summary>
    ''' <param name="notificationFrom">Notification sent from</param>
    ''' <param name="strComment">New comment inserted</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function emailBody(ByVal notificationFrom As String, ByVal strComment As String, Optional ByVal strItem As String = "", Optional ByVal strItemDescription As String = "") As String
        Dim sb As New StringBuilder

        sb.Append("<table border=""0"" cellpadding=""0"" style=""background: #6699FF; mso-cellspacing: 1.5pt; mso-yfti-tbllook: 1184; mso-padding-alt: 0in 0in 0in 0in"" Width = ""500"" > ")
        sb.Append("<tr><td colspan=2 style=""background: white; padding: .75pt .75pt .75pt .75pt"">Autobahn – Notification E-Mail</td></tr>")
        sb.Append("<tr><td>Notified From</td><td>" & notificationFrom & "</td></tr>")
        sb.Append("<tr><td style=""background: white; padding: .75pt .75pt .75pt .75pt"">Message</td><td style=""background: white; padding: .75pt .75pt .75pt .75pt"">New Comment: " & strComment & "</td></tr>")
        sb.Append("<tr><td>Notified For</td><td>Sales Order: " & _headerName & "_" & _line_id & "</td></tr>")
        If strItem <> "" Or strItemDescription <> "" Then
            sb.Append("<tr><td>Item</td><td> " & strItem & " - " & strItemDescription & "</td></tr>")
        End If

        sb.Append("<tr><td style=""background: white; padding: .75pt .75pt .75pt .75pt"">Date</td><td style=""background: white; padding: .75pt .75pt .75pt .75pt"">" & DateTime.Now.ToShortDateString() & "</td></tr>")
        sb.Append("</table>")

        Return sb.ToString()
    End Function


    Private Sub cbDirect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDirect.CheckedChanged
        If cbDirect.Checked Then
            cbWarehouseUsers.Visible = True

        End If
    End Sub

    ''' <summary>
    ''' When the assign check box is pressed the dropdownlist becomes visible.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub cbAssign_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAssign.CheckedChanged
        If cbAssign.Checked Then
            cbWarehouseUsers.Visible = True
            'Change text box entry to standard comment.
            txtComment.Text = "Change of resposibility requested."
        End If

    End Sub

    ''' <summary>
    ''' De - Assigns the last guy for this order.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub btnDeassign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeassign.Click
        Try
            'Use as a reply function!
            Dim strToBeSelected As String

            strToBeSelected = _setOfComments.Tables(0).Rows(_setOfComments.Tables(0).Rows.Count - 1).Item("created_by").ToString
            If Not strToBeSelected = "" Then
                Dim dt As DataTable = cbWarehouseUsers.DataSource
                For i As Integer = 0 To cbWarehouseUsers.Items.Count - 1
                    If dt.Rows(i).Item("userid").ToString.ToLower = strToBeSelected.ToLower Then
                        cbWarehouseUsers.SelectedIndex = i
                        cbDirect.Checked = True
                    End If

                Next
            Else

            End If
        Catch ex As Exception
            MessageBox.Show("There is no person to reply to.")
        End Try

        'Me.getNewKeyNotification()
        ''db.SecureNonQueryParams("INSERT INTO warehouseUserNotifications (notificationId, createdBy, notificationEntity, creationDate, headerID, lineID, reviewed, notificationTo, commentId, organizationID, assignment) " & _
        ''                           " VALUES (@1, @2, @3, @4, @5, @6, 0, NULL, @8, @9, NULL)", _newKeyNotification, Environment.UserName, _entity_name, Format(Now, "yyyyMMdd HH:mm"), _header_id, _line_id, cbWarehouseUsers.Text, _new_key, Me._organizationId)

        'db.SecureNonQueryParams("DELETE FROM warehouseUserNotifications WHERE headerID = @1 AND lineID = @2 AND assignment IS NOT NULL AND notificationEntity = @3 AND organizationID = @4" _
        '                        , _header_id, _line_id, _entity_name, Me._organizationId)

        '_dgv.Rows(_row).Cells("lastAssignment").Value = " "

    End Sub


    Private Sub dlgComments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Assign button is just visible for salesOrderLinesView
        If Not _entity_name = "salesOrderLinesView" Then
            'Me.btnDeassign.Visible = False
            Me.cbAssign.Visible = False
        End If
        ' Scroll to the bottom of the rich text box
        rtbText.SelectionStart = rtbText.Text.Length
        rtbText.ScrollToCaret()

        Me.loadAttachments()

    End Sub

    'Private Sub Link_Clicked(ByVal sender As Object, ByVal e As EventArgs)
    '    MessageBox.Show("clicked")
    'End Sub


    Private Sub cbxActions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxActions.SelectedIndexChanged
        Dim strAction As String
        If cbxActions.SelectedItem.ToString.Contains("-") Then
            strAction = cbxActions.SelectedItem.Substring(0, cbxActions.SelectedItem.IndexOf("-"))
        Else
            strAction = cbxActions.SelectedItem
        End If

        txtComment.Text = ""
        txtComment.Text = strAction

    End Sub

    Private Sub cbWarehouseUsers_DrawItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles cbWarehouseUsers.DrawItem
        Dim useBrush As Brush
        Dim dt As DataTable = cbWarehouseUsers.DataSource
        Dim fn As Font = New Font("System", 9, FontStyle.Regular)
        Dim r As Rectangle = e.Bounds
        Dim sf As StringFormat = New StringFormat()
        sf.Alignment = StringAlignment.Near

        cbWarehouseUsers.BackColor = Color.White

        If (e.Index = -1) Then Return

        If Not DBNull.Value.Equals(dt.Rows(e.Index).Item("OutOfOffice")) AndAlso dt.Rows(e.Index).Item("OutOfOffice") AndAlso dt.Rows(e.Index).Item("outFromDate") < Today.AddDays(1) Then

            e.DrawBackground()

            useBrush = New SolidBrush(Color.FromName(CStr("Red")))
            e.Graphics.FillRectangle(useBrush, _
                e.Bounds.Left + 2, e.Bounds.Top + 2, _
                e.Bounds.Width, e.Bounds.Height)
            e.Graphics.DrawString(dt.Rows(e.Index).Item("user_Name").ToString, fn, New SolidBrush(Color.Black), r, sf)

            useBrush.Dispose()
        Else
            e.DrawBackground()
            useBrush = New SolidBrush(Color.White)
            e.Graphics.FillRectangle(useBrush, _
                e.Bounds.Left + 2, e.Bounds.Top + 2, _
                e.Bounds.Width - 4, e.Bounds.Height - 4)
            e.Graphics.DrawString(dt.Rows(e.Index).Item("user_Name").ToString, fn, New SolidBrush(Color.Black), r, sf)

            useBrush.Dispose()
        End If


    End Sub

    Private Sub cbWarehouseUsers_SelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWarehouseUsers.SelectedValueChanged
        'Make sure also on arrow down to draw the red values
        Try
            Dim dt As DataTable = cbWarehouseUsers.DataSource
            If Not DBNull.Value.Equals(dt.Rows(cbWarehouseUsers.SelectedIndex).Item("OutOfOffice")) Then


                If dt.Rows(cbWarehouseUsers.SelectedIndex).Item("OutOfOffice") AndAlso dt.Rows(cbWarehouseUsers.SelectedIndex).Item("outFromDate") < Today.AddDays(1) Then
                    Dim i As Integer = cbWarehouseUsers.SelectedIndex
                    'dt.Rows(i).Item("user_Name") = dt.Rows(i).Item("lastName") + ", " + dt.Rows(i).Item("firstName") + " Return:" + FormatDateTime(dt.Rows(i).Item("outUnitilDate"), DateFormat.ShortDate)
                    'cbWarehouseUsers.ForeColor = Color.Red
                    cbWarehouseUsers.BackColor = Color.Red

                End If
            Else
                cbWarehouseUsers.BackColor = Color.White
            End If
        Catch ex As Exception

        End Try
    End Sub



    Private Sub rtbText_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles rtbText.LinkClicked
        System.Diagnostics.Process.Start(e.LinkText)

    End Sub

    Private Sub btnAttachDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAttachDoc.Click
        Dim entityId As String = Me._header_id & Me._line_id & Me._releaseNo & Me._shipment_no
        Dim dlg As New dlgAvailableDocuments(Me._organizationId, entityId, Me._entity_name)
        dlg.ShowDialog()

        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            loadAttachments()
            Me.txtComment.Text = "Added attachment: " & dlg.attachmentName
            Me.saveNewComment()
            rtbText.Text = Nothing
            Me.loadComments()

        End If

    End Sub

    Private Sub loadAttachments()
        Dim entityId As String = Me._header_id & Me._line_id & Me._releaseNo & Me._shipment_no
        Dim ds As DataSet
        ds = db.SecureQueryParams(" SELECT a.[attachementID] " & _
                                  " ,a.[commentName], b.attachmentName FROM warehouseAttachementComments a, warehouseAttachements b  WHERE a.attachementID = b.attachementID AND  a.commentName = @1", entityId)

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            If i = 0 Then
                ll1.Visible = True
                ll1.Text = ds.Tables(0).Rows(i).Item("attachmentName")
                ll1.Tag = ds.Tables(0).Rows(i).Item("attachementID")
            End If

        Next


    End Sub

    Private Sub ll1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ll1.LinkClicked
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        If e.Button = Windows.Forms.MouseButtons.Left Then

            Dim ds As DataSet = db.SecureQueryParams("SELECT [organizationID] ,[attachementID] ,[fileName],[attachmentName], " & _
                                         " [attachmentPurpose] ,[creationDate] ,[createdBy],[attachment] FROM [warehouseAttachements] WHERE attachementID = @1 ", ll1.Tag)

            Dim strPath As String = Application.StartupPath & ds.Tables(0).Rows(0).Item("fileName")
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

        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            Dim entityId As String = Me._header_id & Me._line_id & Me._releaseNo & Me._shipment_no
            Dim dlg As New dlgConfirmAction("Delete relation from attachment to order.", "Do you want to delete this attachment in the discussion?")
            dlg.ShowDialog()

            If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
                ll1.Visible = False
                db.SecureNonQueryParams("DELETE FROM warehouseAttachementComments WHERE attachementID = @1 AND commentName = @2", ll1.Tag, entityId)
                Me.txtComment.Text = "Deleted attachment: " & ll1.Text
                Me.saveNewComment()
                rtbText.Text = Nothing
                Me.loadComments()

            End If



        End If

    End Sub

    Private Sub btnMail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMail.Click
        Dim strSubject As String = Me.Text
        Dim strEmailTo As String = ""
        Dim strMailCC As String = ""
        Dim sParam As String = ""

        'Create the cc list
        For i As Integer = 0 To _setOfComments.Tables(0).Rows.Count - 1
            If Not DBNull.Value.Equals(_setOfComments.Tables(0).Rows(i).Item("eMailFrom")) Then
                If Not strMailCC.Contains(_setOfComments.Tables(0).Rows(i).Item("eMailFrom")) Then
                    strMailCC += _setOfComments.Tables(0).Rows(i).Item("eMailFrom") + ";"
                End If
            End If

            If Not DBNull.Value.Equals(_setOfComments.Tables(0).Rows(i).Item("eMailTo")) Then
                If Not strMailCC.Contains(_setOfComments.Tables(0).Rows(i).Item("eMailTo")) Then
                    strMailCC += _setOfComments.Tables(0).Rows(i).Item("eMailTo") + ";"
                End If
            End If
        Next

        Dim strBody As String = rtbText.Text

        'createDraftEmail(strSubject, "", strBody, "mailhost.flowserve.com", strMailCC)
        createOutlookDraftEmail(strSubject, "", strBody, "mailhost.flowserve.com", strMailCC)

    End Sub


    Private Sub createDraftEmail(ByVal EmailSubject As String, ByVal EMailSendTo As String, ByVal EMailBody As String, ByVal MailServer As String, ByVal strMaillCC As String)

        Dim objNotesSession As Object
        Dim objNotesMailFile As Object
        Dim objNotesDocument As Object
        Dim objNotesField As Object
        Dim sendmail As Boolean

        'added for integration into reporting tool
        Dim dbString As String

        dbString = "mail\" & Environment.UserName & ".nsf"

        On Error GoTo SendMailError
        'Establish Connection to Notes
        objNotesSession = CreateObject("Notes.NotesSession")
        On Error Resume Next
        'Establish Connection to Mail File
        objNotesMailFile = objNotesSession.GETDATABASE(MailServer, dbString)
        'Open Mail
        objNotesMailFile.OPENMAIL()
        On Error GoTo 0

        'Create New Memo
        objNotesDocument = objNotesMailFile.createdocument

        Dim oWorkSpace As Object, oUIdoc As Object
        oWorkSpace = CreateObject("Notes.NotesUIWorkspace")
        oUIdoc = oWorkSpace.CurrentDocument

        'Create 'Subject Field'
        objNotesField = objNotesDocument.APPENDITEMVALUE("Subject", EmailSubject)

        'Create 'Send To' Field
        objNotesField = objNotesDocument.APPENDITEMVALUE("SendTo", EMailSendTo)

        'Create 'Copy To' Field
        objNotesField = objNotesDocument.APPENDITEMVALUE("CopyTo", strMaillCC)

        'Create 'Blind Copy To' Field
        ' objNotesField = objNotesDocument.APPENDITEMVALUE("BlindCopyTo", EMailBCCTo)

        'Create 'Body' of memo
        objNotesField = objNotesDocument.CREATERICHTEXTITEM("Body")

        With objNotesField
            .APPENDTEXT(EMailBody)
            .ADDNEWLINE(1)
        End With

        'Send the e-mail

        Call objNotesDocument.Save(True, False, False)
        objNotesDocument.SaveMessageOnSend = True
        'objNotesDocument.Save
        'objNotesDocument.Send(0)

        'Release storage
        objNotesSession = Nothing
        objNotesMailFile = Nothing
        objNotesDocument = Nothing
        objNotesField = Nothing

        'Set return code
        sendmail = True

        Exit Sub

SendMailError:
        Dim Msg
        Msg = "Error # " & Str(Err.Number) & " was generated by " _
                    & Err.Source & Chr(13) & Err.Description
        MessageBox.Show(Msg, "Error")

        sendmail = False
    End Sub
    Private Sub createOutlookDraftEmail(ByVal EmailSubject As String, ByVal EMailSendTo As String, ByVal EMailBody As String, ByVal MailServer As String, ByVal strMaillCC As String)

        Dim objOutlk As New Outlook.Application 'Outlook
        Const olMailItem As Integer = 0
        Dim objMail As New System.Object

        objMail = objOutlk.CreateItem(olMailItem) 'Email item

        'Insert your "To" address...it can by dynamically populated
        objMail.To = EMailSendTo

        'Insert your "CC" address...it can by dynamically populated
        objMail.cc = strMaillCC 'Enter an address here To include a carbon copy; bcc is For blind carbon copy's

        'Set up Subject Line
        objMail.subject = EmailSubject

        'To add an attachment, use:
        'objMail.attachments.add("enter your attachment path here")
        ''otherwise, if no attachment, you can comment the objMail.attachments.add("") out with an apostrophe

        'Set up your message body        
        objMail.body = EMailBody

        'Use this To display before sending, otherwise call (use) objMail.Send to send without reviewing
        objMail.Display()
        'Clean up
        objMail = Nothing
        objOutlk = Nothing

    End Sub
    ' Hilfsfunktion: EMail-Parameter zusammenstellen
    Private Sub AddMailParam(ByRef sAllParam As String, ByVal sParam As String)
        If sAllParam = String.Empty Then
            sAllParam = "?" & sParam
        Else
            sAllParam &= "&" & sParam
        End If
    End Sub


    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        'Dim frm As New frmReportViewer("Sales Order Backlog", "ProjectManagerWorkbench\Reporting\repDiscussionReport.rdlc", Me.Text, rtbText.Text)
        'frm.Show()
        'frm.printTheReport()
        'frm.Close()
        Me.rtbText.Text += vbCrLf + Me.Text
        Me.Refresh()

        pf1.Form = Me
        pf1.Print()



    End Sub





    Private Sub ofdAttachements_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ofdAttachements.FileOk

    End Sub
    Private Sub ll3_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ll3.LinkClicked

    End Sub
    Private Sub ll2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ll2.LinkClicked

    End Sub
    Private Sub lblAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblAction.Click

    End Sub
    Private Sub cbWarehouseUsers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWarehouseUsers.SelectedIndexChanged

    End Sub
    Private Sub txtData_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtData.TextChanged

    End Sub
    Private Sub lblComments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblComments.Click

    End Sub
    Private Sub txtComment_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtComment.TextChanged

    End Sub
    Private Sub rtbText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtbText.TextChanged

    End Sub
    Private Sub TableLayoutPanel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub
End Class

Public Class clsComments

    Private rtb2 As RichTextBox

    ''' <summary>
    ''' Constructor that direcly loads the comments
    ''' </summary>
    ''' <param name="rtb">Richtextbox that should be loaded.</param>
    ''' <param name="listOfComments">The list of comments that should be loaded to the rich text box</param>
    ''' <remarks></remarks>
    Public Sub New(ByRef rtb As RichTextBox, ByRef listOfComments As DataTable, Optional ByRef dlg As dlgComments = Nothing)
        Me.LoadCommentsToRichText(rtb, listOfComments, dlg)

    End Sub




    Public Sub New(ByVal rtb As RichTextBox)
        rtb2 = rtb
    End Sub
    ''' <summary>
    ''' Just the object without creation logic.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    Public Sub loadLiveTicker(ByVal organizationId As Integer)
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.SecureQueryParams("SELECT TOP 50 co.[description], co.creation_date, co.created_by, wun.notificationTo " & _
                                              " , wu1.firstName firstNameCreation, wu1.lastName lastNameCreation " & _
                                              " , wu2.firstName firstNameDestination, wu2.lastName lastNameDestination " & _
                                              " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                              " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                              " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                              " WHERE  co.entity_id IN ('poQueueView', 'wipQueueView', 'salesOrderLineView') AND co.organizationID = @1    ORDER BY co.creation_date DESC", organizationId, Environment.UserName)

        Me.LoadCommentsToRichText(rtb2, ds.Tables(0))

    End Sub
    Public Sub loadLatestNotReviewedNotifications(ByVal organizationId As Integer)
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.SecureQueryParams("SELECT TOP 50 co.[description], co.creation_date, co.created_by, wun.notificationTo " & _
                                              " , wu1.firstName firstNameCreation, wu1.lastName lastNameCreation " & _
                                              " , wu2.firstName firstNameDestination, wu2.lastName lastNameDestination " & _
                                              " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                              " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                              " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                              " WHERE  co.entity_id IN ('poQueueView', 'wipQueueView', 'salesOrderLineView') AND co.organizationID = @1  AND wun.notificationTo = @2 AND wun.reviewed = 0  ORDER BY co.creation_date DESC", organizationId, Environment.UserName)

        Me.LoadCommentsToRichText(rtb2, ds.Tables(0))
    End Sub
    ''' <summary>
    ''' Function that returns a string which can be passed to the process.start for emailing
    ''' </summary>
    ''' <param name="listOfComments"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function returnCommentsForMail(ByRef listOfComments As DataTable)
        Dim strComments As String = ""

        For i As Integer = 0 To listOfComments.Rows.Count - 1
            If strComments <> "" Then strComments &= " %0A%0A "
            Dim strNotificationTo As String
            If DBNull.Value.Equals(listOfComments.Rows(i).Item("notificationTo")) Then
                strNotificationTo = ""
            ElseIf listOfComments.Rows(i).Item("notificationTo").ToString.Count >= 5 Then

                'If we can use last name, first name otherwise choose the windows user name
                If Not DBNull.Value.Equals(listOfComments.Rows(i).Item("firstNameDestination")) Then

                    'Be careful with name less than three characters
                    If listOfComments.Rows(i).Item("lastNameDestination").ToString.Count >= 3 AndAlso listOfComments.Rows(i).Item("firstNameDestination").ToString.Count >= 2 Then
                        strNotificationTo = listOfComments.Rows(i).Item("lastNameDestination") + " " + listOfComments.Rows(i).Item("firstNameDestination")
                    Else
                        strNotificationTo = listOfComments.Rows(i).Item("lastNameDestination").ToString.Substring(0, listOfComments.Rows(i).Item("lastNameDestination").ToString.Count) + listOfComments.Rows(i).Item("firstNameDestination").ToString.Substring(0, listOfComments.Rows(i).Item("firstNameDestination").ToString.Count)
                    End If
                Else
                    strNotificationTo = listOfComments.Rows(i).Item("notificationTo").ToString.Substring(0, 5)
                End If

            Else
                'No first and last name available
                strNotificationTo = listOfComments.Rows(i).Item("notificationTo").ToString
            End If

            Dim strCreator As String = ""
            'Combine the creator of the comments with first name, last name or if not available from the windows user name
            If Not DBNull.Value.Equals(listOfComments.Rows(i).Item("firstNameCreation")) AndAlso Not DBNull.Value.Equals(listOfComments.Rows(i).Item("lastNameCreation")) Then
                'Take about the things with a very short name
                If listOfComments.Rows(i).Item("firstNameCreation").ToString.Count >= 3 AndAlso listOfComments.Rows(i).Item("lastNameCreation").ToString.Count >= 2 Then
                    strCreator = listOfComments.Rows(i).Item("lastNameCreation") + " " + listOfComments.Rows(i).Item("firstNameCreation")
                Else
                    strCreator = listOfComments.Rows(i).Item("lastNameCreation").ToString.Substring(0, listOfComments.Rows(i).Item("lastNameCreation").ToString.Count) + listOfComments.Rows(i).Item("firstNameCreation").ToString.Substring(0, listOfComments.Rows(i).Item("firstNameCreation").ToString.Count)
                End If
            Else
                'Take care of the names with a very short name
                If listOfComments.Rows(i).Item("created_by").ToString.Count >= 5 Then
                    strCreator = listOfComments.Rows(i).Item("created_by").ToString.Substring(0, 5)
                Else
                    strCreator = listOfComments.Rows(i).Item("created_by").ToString.Substring(0, listOfComments.Rows(i).Item("created_by").ToString.Count)
                End If
            End If

            Dim strDesription As String = listOfComments.Rows(i).Item("description")
            Dim strNewDesription As String = ""

            strNewDesription = strDesription
            'Replace Umlaute



            'Substring to the first 20 characters of the names
            If strCreator.Count > 21 Then
                strCreator = strCreator.Substring(0, 21)
            End If
            If strNotificationTo.Count > 21 Then
                strNotificationTo = strNotificationTo.Substring(0, 21)
            End If
            Dim strPersonalDescription As String
            If strNotificationTo.Count > 0 Then
                strPersonalDescription = strCreator & " to " & strNotificationTo
            Else
                strPersonalDescription = strCreator & strNotificationTo
            End If


            Dim strHeaderInformation As String = strPersonalDescription & " " & Format(listOfComments.Rows(i).Item("creation_date"), "short date").Substring(0, 5) + " "
            strComments += strHeaderInformation + strNewDesription



        Next
        'Replace strange character
        strComments = strComments.Replace("ü", "ue")
        strComments = strComments.Replace("ä", "ae")
        strComments = strComments.Replace("ö", "ae")
        strComments = strComments.Replace("?", " ")
        strComments = strComments.Replace("&", " ")

        strComments = strComments.Replace("Ü", "Ue")
        strComments = strComments.Replace("Ä", "ae")
        strComments = strComments.Replace("Ö", "ae")
        strComments &= " %0A%0A "

        Return strComments
    End Function

    ''' <summary>
    ''' Procedure that loads the comments to the richtextbox
    ''' </summary>
    ''' <param name="rtb"></param>
    ''' <param name="listOfComments"></param>
    ''' <param name="dlg"></param>
    ''' <remarks></remarks>
    Private Sub LoadCommentsToRichText(ByRef rtb As RichTextBox, ByRef listOfComments As DataTable, Optional ByRef dlg As dlgComments = Nothing)
        rtb2 = rtb
        For i As Integer = 0 To listOfComments.Rows.Count - 1


            If rtb.Text.Length > 0 Then rtb.Text += vbCrLf
            'to do: different kind of formats for the different regions

            Dim strNotificationTo As String
            If DBNull.Value.Equals(listOfComments.Rows(i).Item("notificationTo")) Then
                strNotificationTo = ""
            ElseIf listOfComments.Rows(i).Item("notificationTo").ToString.Count >= 5 Then

                'If we can use last name, first name otherwise choose the windows user name
                If Not DBNull.Value.Equals(listOfComments.Rows(i).Item("firstNameDestination")) Then

                    'Be careful with name less than three characters
                    If listOfComments.Rows(i).Item("lastNameDestination").ToString.Count >= 3 AndAlso listOfComments.Rows(i).Item("firstNameDestination").ToString.Count >= 2 Then
                        strNotificationTo = vbCrLf + listOfComments.Rows(i).Item("lastNameDestination") + " " + listOfComments.Rows(i).Item("firstNameDestination")
                    Else
                        strNotificationTo = vbCrLf + listOfComments.Rows(i).Item("lastNameDestination").ToString.Substring(0, listOfComments.Rows(i).Item("lastNameDestination").ToString.Count) + listOfComments.Rows(i).Item("firstNameDestination").ToString.Substring(0, listOfComments.Rows(i).Item("firstNameDestination").ToString.Count)
                    End If
                Else
                    strNotificationTo = vbCrLf + listOfComments.Rows(i).Item("notificationTo").ToString.Substring(0, 5)
                End If

            Else
                'No first and last name available
                strNotificationTo = listOfComments.Rows(i).Item("notificationTo").ToString
            End If

            Dim strCreator As String = ""
            'Combine the creator of the comments with first name, last name or if not available from the windows user name
            If Not DBNull.Value.Equals(listOfComments.Rows(i).Item("firstNameCreation")) AndAlso Not DBNull.Value.Equals(listOfComments.Rows(i).Item("lastNameCreation")) Then
                'Take about the things with a very short name
                If listOfComments.Rows(i).Item("firstNameCreation").ToString.Count >= 3 AndAlso listOfComments.Rows(i).Item("lastNameCreation").ToString.Count >= 2 Then
                    strCreator = listOfComments.Rows(i).Item("lastNameCreation") + " " + listOfComments.Rows(i).Item("firstNameCreation")
                Else
                    strCreator = listOfComments.Rows(i).Item("lastNameCreation").ToString.Substring(0, listOfComments.Rows(i).Item("lastNameCreation").ToString.Count) + listOfComments.Rows(i).Item("firstNameCreation").ToString.Substring(0, listOfComments.Rows(i).Item("firstNameCreation").ToString.Count)
                End If
            Else
                'Take care of the names with a very short name
                If listOfComments.Rows(i).Item("created_by").ToString.Count >= 5 Then
                    strCreator = listOfComments.Rows(i).Item("created_by").ToString.Substring(0, 5)
                Else
                    strCreator = listOfComments.Rows(i).Item("created_by").ToString.Substring(0, listOfComments.Rows(i).Item("created_by").ToString.Count)
                End If
            End If

            Dim strDesription As String = listOfComments.Rows(i).Item("description")
            Dim strNewDesription As String = ""

            strNewDesription = strDesription + vbCr  'Nothing to do

            'Substring to the first 20 characters of the names
            If strCreator.Count > 21 Then
                strCreator = strCreator.Substring(0, 21)
            End If
            If strNotificationTo.Count > 21 Then
                strNotificationTo = strNotificationTo.Substring(0, 21)
            End If
            Dim strPersonalDescription As String
            If strNotificationTo.Count > 0 Then
                strPersonalDescription = strCreator & " to " & strNotificationTo
            Else
                strPersonalDescription = strCreator & strNotificationTo
            End If


            Dim strHeaderInformation As String = strPersonalDescription & " " & CType(listOfComments.Rows(i).Item("creation_date"), Date).ToString("dd/MM", System.Globalization.CultureInfo.InvariantCulture) + ": "

            Dim startLength As Integer = rtb.Text.Length        'The start of the selection



            'Distinct between black and blue
            'rtb.SelectionStart = Nothing
            'rtb.SelectedText = Nothing


            'rtb.SelectionColor = Color.Blue
            rtb.Text += strHeaderInformation
            Dim endlenth As Integer = rtb.Text.Length
            rtb.SelectionStart = startLength
            rtb.SelectionLength = endlenth - startLength
            rtb.SelectionColor = Color.Blue



            rtb.Text += strNewDesription

        Next

        'NExt loop to colour the text
        For i As Integer = 0 To listOfComments.Rows.Count - 1
            Dim strDescription = listOfComments.Rows(i).Item("description")

            Dim start As Integer = rtb.Find(strDescription)
            If Not start = -1 Then

                rtb.SelectionStart = start
                rtb.SelectionLength = Len(strDescription)
                rtb.SelectionColor = Color.Black
                If listOfComments.Columns.Contains("reviewed") Then
                    If Not DBNull.Value.Equals(listOfComments.Rows(i).Item("reviewed")) Then
                        If Not listOfComments.Rows(i).Item("reviewed") Then
                            'Highligh the notification to name when it was not reviewed yet
                            Dim ifont As New Font(rtb.Font, FontStyle.Bold)
                            rtb.SelectionFont = ifont

                        End If
                    End If
                End If
            End If

        Next

    End Sub

End Class