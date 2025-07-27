Public Class frmDiscussionTracking
    Private organizationID As Integer
    Private frmWorkbenchProjects As frmWorkbenchProjects
    Private strCurrentUser As String
    Private gridDisplay As New clsGridDisplay
    Private myPlanningType As String

    'The views and tables we want to touch
    Private strPO As String = " poQueueView "
    Private strWIP As String = " wipQueueView "
    Private strSalesOrder As String = " salesOrderLinesView "
    Private strSaveMRPDemand As String = " saveMRPDemand "


    Public Sub New(ByVal organizationID As Integer, ByRef frmAutobahn As frmWorkbenchProjects)
        InitializeComponent()
        Me.organizationID = frmAutobahn.currentorganizationID
        Me.frmWorkbenchProjects = frmAutobahn
        Me.strCurrentUser = Environment.UserName        'Saves the user we want to display
        Me.myPlanningType = frmAutobahn.currentPlanningType

        If frmAutobahn.currentPlanningType = "ASCP" Then
            strPO = " mscPOQueueView "
            strWIP = " MSCWIPQueueView "
            strSalesOrder = " mscsalesOrderLinesView  "
            strSaveMRPDemand = " saveMSCDemand "

        End If
    End Sub

    Private Sub frmDiscussionTracking_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'Saves the settings of the form

        My.Settings.discussionTrackingSc1Panel1 = Me.SplitContainer1.Panel1.Size
        My.Settings.discussionTrackingSc1Panel2 = Me.SplitContainer1.Panel2.Size
        My.Settings.discussionTrackingSc2Panel1 = Me.SplitContainer2.Panel1.Size
        My.Settings.discussionTrackingSc2Panel2 = Me.SplitContainer2.Panel2.Size
        My.Settings.discussionTrackingSc1 = Me.SplitContainer1.Size
        My.Settings.discussionTrackingSc2 = Me.SplitContainer2.Size
        My.Settings.discussionTrackingSc1SplitterDistance = Me.SplitContainer1.SplitterDistance
        My.Settings.discussionTrackingSc2SplitterDistance = Me.SplitContainer2.SplitterDistance

        My.Settings.discussionTrackingSize = Me.Size

        My.Settings.Save()

        For i As Integer = 0 To dgvDiscussion.Columns.Count - 1
            gridDisplay.saveChangesOfSingleColumnDisplayIndex(dgvDiscussion.Columns(i).Name, "discussionTracking", dgvDiscussion.Columns(i).DisplayIndex, dgvDiscussion)
        Next
        frmWorkbenchProjects.gridDisplay.loadColumnSettings()

    End Sub

    Private Sub frmDiscussionTracking_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        cbxNoRecords.SelectedIndex = 1
        Try

            loadCurrentDiscussion()
            loadSettingsOfDiscussionTracking()
            For Each dgvCol As DataGridViewColumn In dgvDiscussion.Columns
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            'Get the focus to the discussion, so people can scroll right away.
            'dgvDiscussion.Select()
            'dgvDiscussion.Focus()

            'Fixed the right panel
            SplitContainer2.FixedPanel = FixedPanel.Panel1
            SplitContainer1.Panel1MinSize = 515
            SplitContainer2.Panel1MinSize = 100

        Catch ex As Exception

        End Try

    End Sub
    Private Sub loadSettingsOfDiscussionTracking()

        Me.Size = My.Settings.discussionTrackingSize
        Me.SplitContainer1.SplitterDistance = My.Settings.discussionTrackingSc1SplitterDistance
        Me.SplitContainer2.SplitterDistance = My.Settings.discussionTrackingSc2SplitterDistance

    End Sub
    Private Sub loadCurrentDiscussion(Optional ByVal filter As String = "")

        'Has to be adjusted to ASCP planning

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.SecureQueryParams("SELECT TOP " & cbxNoRecords.SelectedItem & " co.[description] [Comment], co.creation_date [Creation Date], co.created_by, wun.notificationTo " & _
                                              " , wu1.lastName + ', ' +  wu1.firstName [Creation Name], co.commentID, co.ENTITY_ID, co.HEADER_ID, co.LINE_ID, co.SHIPMENT_NO,co.organizationID, co.releaseNo " & _
                                              "  , wu2.lastName + ', ' + wu2.firstName [Notification To], co.headerName [Order],co.headerName, wun.notificationid " & _
                                              ", (SELECT COUNT(*) FROM " & strSalesOrder & " so WHERE (so.HEADER_ID = co.HEADER_ID) AND (so.LINE_NO = co.LINE_ID) AND so.ORGANIZATION_ID = co.organizationid )  " & _
                                              " + (SELECT count(*) FROM " & strPO & " po WHERE (po.PO_HEADER_ID = co.HEADER_ID) AND (po.LINE_NO = co.LINE_ID) AND (isnull(po.SHIPMENT_NO,0) = co.SHIPMENT_NO) AND     (isnull(po.RELEASE_NO,0) = co.releaseNo) AND po.ship_to_organization_id = co.organizationID  )   " & _
                                              " + (SELECT COUNT(*) FROM " & strWIP & " jo WHERE (jo.WIP_ENTITY_ID = co.HEADER_ID) AND (jo.OPERATION_SEQ_NO = co.LINE_ID) AND jo.organization_id = co.organizationID )   " & _
                                              " + (SELECT COUNT(*) FROM " & strSaveMRPDemand & " md WHERE      (co.HEADER_ID = md.ORDER_NO) AND  (co.organizationID = md.ORGANIZATION_ID) AND (co.LINE_ID = md.LINE_NO) AND  (co.SHIPMENT_NO = md.INVENTORY_ITEM_ID)  ) AS [Open]  " & _
                                              " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                              " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                              " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                              " WHERE  co.entity_id IN ('poQueueView', 'wipQueueView', 'salesOrderLinesView', 'plannedOrder') AND co.organizationID = @1 " & filter & "  ORDER BY co.creation_date DESC", organizationID)

        Dim comments As New clsComments(rtbDiscussion)
        comments.loadLiveTicker(Me.organizationID)
        dgvDiscussion.DataSource = ds.Tables(0)
        Me.alignDataTable(dgvDiscussion)
        Me.applyHeaderName(ds.Tables(0))
        'Display grid columns with user specific column widths
        'gridDisplay.column_size(dgvDiscussion, "discussionTracking")
        gridDisplay.formatGrid(dgvDiscussion, "discussionTracking")
        gridDisplay.column_reordering(dgvDiscussion, "discussionTracking")
        'Add the order details
        'Create one combined order field to display
    End Sub


    ''' <summary>
    ''' Applies a combined order name to the data table
    ''' </summary>
    ''' <param name="dt">The data table that contains the values</param>
    ''' <remarks></remarks>
    Private Sub applyHeaderName(ByRef dt As DataTable)
        'SO:   110600535 Line 2
        'WIP:   67897   Sequence 20
        'PO:    76767  Line 2 Release 3 Shipment 4
        Try

        
            For Each i As DataRow In dt.Rows
                If i.Item("ENTITY_ID") = "salesOrderLinesView" Then
                    i.Item("Order") = "SO: " + i.Item("Order") + " Line: " + i.Item("LINE_ID").ToString
                ElseIf i.Item("ENTITY_ID") = "POQueueView" Then
                    i.Item("Order") = "PO: " + i.Item("Order") + " Line: " + i.Item("LINE_ID").ToString + " Release: " + i.Item("releaseNo").ToString + " Shipment: " + i.Item("SHIPMENT_NO").ToString
                ElseIf i.Item("ENTITY_ID") = "WIPQueueView" Then
                    i.Item("Order") = "WDJ: " + i.Item("Order") + " Sequence: " + i.Item("LINE_ID").ToString
                ElseIf i.Item("ENTITY_ID") = "plannedOrder" Then
                    i.Item("Order") = "PO on SO: " + i.Item("Order") + " Line: " + i.Item("LINE_ID").ToString
                End If

            Next
        Catch ex As Exception

        End Try

    End Sub

    Private Sub alignDataTable(ByVal dgv As DataGridView)

        dgvDiscussion.Columns("created_by").Visible = False
        dgvDiscussion.Columns("notificationTo").Visible = False
        If dgvDiscussion.Columns.Contains("notificationiD") Then
            dgvDiscussion.Columns("notificationiD").Visible = False
        End If

        dgvDiscussion.Columns("commentID").Visible = False

        dgvDiscussion.Columns("ENTITY_ID").Visible = False
        dgvDiscussion.Columns("HEADER_ID").Visible = False
        dgvDiscussion.Columns("LINE_ID").Visible = False
        dgvDiscussion.Columns("SHIPMENT_NO").Visible = False
        dgvDiscussion.Columns("releaseNo").Visible = False
        dgvDiscussion.Columns("organizationID").Visible = False



    End Sub


    Private Sub btnBeingNotified_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBeingNotified.Click
        Me.loadIWasNotified()
        'Get the focus to the discussion, so people can scroll right away.
        dgvDiscussion.Select()
        dgvDiscussion.Focus()
    End Sub

    Public Sub loadIWasNotified()
        Try

            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            btnDone.Visible = True
            btnDone.Enabled = True

            Dim ds As DataSet = db.SecureQueryParams("SELECT TOP " & cbxNoRecords.SelectedItem & " co.[description] [Comment], co.creation_date [Creation Date], co.created_by, wun.notificationTo " & _
                                                  " , wu1.lastName + ', ' +  wu1.firstName [Creation Name], co.commentID, co.ENTITY_ID, co.HEADER_ID, co.LINE_ID, co.SHIPMENT_NO,co.organizationID, co.releaseNo  " & _
                                                  "  , wu2.lastName + ', ' + wu2.firstName [Notification To], wun.reviewed [Reviewed], wun.notificationId, co.headerName [Order], co.headerName " & _
                                                     ", (SELECT COUNT(*) FROM " & strSalesOrder & " so WHERE (so.HEADER_ID = co.HEADER_ID) AND (so.LINE_NO = co.LINE_ID) AND so.ORGANIZATION_ID = co.organizationid  )  " & _
                                              " + (SELECT count(*) FROM " & strPO & " po WHERE (po.PO_HEADER_ID = co.HEADER_ID) AND (po.LINE_NO = co.LINE_ID) AND (po.SHIPMENT_NO = co.SHIPMENT_NO) AND     (isnull(po.RELEASE_NO,0) = co.releaseNo) AND po.ship_to_organization_id = co.organizationID)   " & _
                                              " + (SELECT COUNT(*) FROM " & strWIP & " jo WHERE (jo.WIP_ENTITY_ID = co.HEADER_ID) AND (jo.OPERATION_SEQ_NO = co.LINE_ID) AND jo.organization_id = co.organizationID )   " & _
                                              " + (SELECT COUNT(*) FROM " & strSaveMRPDemand & " md WHERE      (co.HEADER_ID = md.ORDER_NO) AND  (co.organizationID = md.ORGANIZATION_ID) AND (co.LINE_ID = md.LINE_NO) AND  (co.SHIPMENT_NO = md.INVENTORY_ITEM_ID)  ) AS [Open]  " & _
                                                  " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                                  " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                                  " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                                  " WHERE  co.entity_id IN ('poQueueView', 'wipQueueView', 'salesOrderLinesView', 'plannedOrder') AND co.organizationID = @1 AND wun.notificationTo = @2  ORDER BY co.creation_date DESC", organizationID, strCurrentUser)

            Dim comments As New clsComments(rtbDiscussion)

            dgvDiscussion.DataSource = ds.Tables(0)
            dgvDiscussion.Columns("notificationId").Visible = False

            'Iterate over the content and make everything that is not checked bold
            For i As Integer = 0 To dgvDiscussion.Rows.Count - 1
                If Not dgvDiscussion.Rows(i).Cells("Reviewed").Value Then
                    dgvDiscussion.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
                    Dim f As New Font("Verdana", 11, FontStyle.Bold, GraphicsUnit.Pixel)
                    dgvDiscussion.Rows(i).DefaultCellStyle.Font = f
                End If
            Next

            Me.alignDataTable(dgvDiscussion)
            '            dgvDiscussion.Columns("Reviewed").
            Me.applyHeaderName(ds.Tables(0))
        Catch ex As Exception

        End Try
    End Sub


    Private Sub btnMyNotifications_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMyNotifications.Click
        btnDone.Enabled = False
        btnDone.Visible = False
        Try

        
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            Dim ds As DataSet = db.SecureQueryParams("SELECT TOP " & cbxNoRecords.SelectedItem & " co.[description] [Comment], co.creation_date [Creation Date], co.created_by, wun.notificationTo " & _
                                                  " , wu1.lastName + ', ' +  wu1.firstName [Creation Name], co.commentID, co.ENTITY_ID, co.HEADER_ID, co.LINE_ID, co.SHIPMENT_NO,co.organizationID, co.releaseNo  " & _
                                                  " , wu2.lastName + ', ' + wu2.firstName [Notification To], co.headerName [Order], co.headerName " & _
                                                     ", (SELECT COUNT(*) FROM " & strSalesOrder & " so WHERE (so.HEADER_ID = co.HEADER_ID) AND (so.LINE_NO = co.LINE_ID) AND so.ORGANIZATION_ID = co.organizationid )  " & _
                                                  " + (SELECT count(*) FROM " & strPO & " po WHERE (po.PO_HEADER_ID = co.HEADER_ID) AND (po.LINE_NO = co.LINE_ID) AND (po.SHIPMENT_NO = co.SHIPMENT_NO) AND     (isnull(po.RELEASE_NO,0) = co.releaseNo) AND po.ship_to_organization_id = co.organizationID )   " & _
                                                  " + (SELECT COUNT(*) FROM " & strWIP & " jo WHERE (jo.WIP_ENTITY_ID = co.HEADER_ID) AND (jo.OPERATION_SEQ_NO = co.LINE_ID) AND jo.organization_id = co.organizationID )   " & _
                                                  " + (SELECT COUNT(*) FROM " & strSaveMRPDemand & " md WHERE      (co.HEADER_ID = md.ORDER_NO) AND  (co.organizationID = md.ORGANIZATION_ID) AND (co.LINE_ID = md.LINE_NO) AND  (co.SHIPMENT_NO = md.INVENTORY_ITEM_ID)  ) AS [Open]  " & _
                                                  " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                                  " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                                  " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                                  " WHERE  co.entity_id IN ('poQueueView', 'wipQueueView', 'salesOrderLinesView', 'plannedOrder') AND co.organizationID = @1 AND wun.createdBy = @2  ORDER BY co.creation_date DESC", organizationID, strCurrentUser)


            Dim comments As New clsComments(rtbDiscussion)

            dgvDiscussion.DataSource = ds.Tables(0)
            Me.alignDataTable(dgvDiscussion)
            Me.applyHeaderName(ds.Tables(0))

            'Get the focus to the discussion, so people can scroll right away.
            dgvDiscussion.Select()
            dgvDiscussion.Focus()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnMyComments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMyComments.Click
        Try

       
            btnDone.Enabled = False
            btnDone.Visible = False
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            Dim ds As DataSet = db.SecureQueryParams("SELECT TOP " & cbxNoRecords.SelectedItem & " co.[description] [Comment], co.creation_date [Creation Date], co.created_by, wun.notificationTo " & _
                                                  " , wu1.lastName + ', ' +  wu1.firstName [Creation Name], co.commentID, co.ENTITY_ID, co.HEADER_ID, co.LINE_ID, co.SHIPMENT_NO,co.organizationID, co.releaseNo  " & _
                                                  " , wu2.lastName + ', ' + wu2.firstName [Notification To], co.headerName [Order],co.headerName " & _
                                                     ", (SELECT COUNT(*) FROM " & strSalesOrder & " so WHERE (so.HEADER_ID = co.HEADER_ID) AND (so.LINE_NO = co.LINE_ID) AND so.ORGANIZATION_ID = co.organizationid )  " & _
                                                  " + (SELECT count(*) FROM " & strPO & "  po WHERE (po.PO_HEADER_ID = co.HEADER_ID) AND (po.LINE_NO = co.LINE_ID) AND (po.SHIPMENT_NO = co.SHIPMENT_NO) AND     (po.RELEASE_NO = co.releaseNo) AND po.ship_to_organization_id = co.organizationID)   " & _
                                                  " + (SELECT COUNT(*) FROM " & strWIP & " jo WHERE (jo.WIP_ENTITY_ID = co.HEADER_ID) AND (jo.OPERATION_SEQ_NO = co.LINE_ID) AND jo.organization_id = co.organizationID )   " & _
                                                  " + (SELECT COUNT(*) FROM " & strSaveMRPDemand & "  WHERE      (co.HEADER_ID = dbo.saveMRPdemand.ORDER_NO) AND  (co.organizationID = dbo.saveMRPdemand.ORGANIZATION_ID) AND (co.LINE_ID = dbo.saveMRPdemand.LINE_NO) AND  (co.SHIPMENT_NO = dbo.saveMRPdemand.INVENTORY_ITEM_ID)  ) AS [Open]  " & _
                                                  " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                                  " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                                  " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                                  " WHERE  co.entity_id IN ('poQueueView', 'wipQueueView', 'salesOrderLinesView', 'plannedOrder') AND co.organizationID = @1 AND co.created_by = @2  ORDER BY co.creation_date DESC", organizationID, strCurrentUser)

            Dim comments As New clsComments(rtbDiscussion)
            'comments.loadLiveTicker(Me.organizationID)
            dgvDiscussion.DataSource = ds.Tables(0)
            Me.alignDataTable(dgvDiscussion)
            Me.applyHeaderName(ds.Tables(0))

            'Get the focus to the discussion, so people can scroll right away.
            dgvDiscussion.Select()
            dgvDiscussion.Focus()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        'Perform a full text search on the data source to shrink down the list
        Me.loadCurrentDiscussion(" AND (co.headerName LIKE '%" & txtSearchPhrase.Text & "%' OR co.[description] LIKE '%" & txtSearchPhrase.Text & "%' OR wu1.lastName LIKE '%" & txtSearchPhrase.Text & "%' OR wu1.firstName LIKE '%" & txtSearchPhrase.Text & "%') ")

        'mark all of them
        For i As Integer = 0 To dgvDiscussion.Rows.Count - 1
            For j As Integer = 0 To dgvDiscussion.Columns.Count - 1
                If dgvDiscussion.Columns(j).Visible Then
                    Try
                        If dgvDiscussion.Rows(i).Cells(j).Value.ToString.ToLower.Contains(txtSearchPhrase.Text.ToLower) Then
                            dgvDiscussion.FirstDisplayedScrollingRowIndex = i
                            dgvDiscussion.Rows(i).Cells(j).Selected = True
                        End If
                    Catch ex As Exception

                    End Try
                   
                End If
            Next
        Next

        'checkout the first one
        For i As Integer = 0 To dgvDiscussion.Rows.Count - 1
            For j As Integer = 0 To dgvDiscussion.Columns.Count - 1
                If dgvDiscussion.Columns(j).Visible Then
                    Try
                        If dgvDiscussion.Rows(i).Cells(j).Value.ToString.ToLower.Contains(txtSearchPhrase.Text.ToLower) Then
                            dgvDiscussion.FirstDisplayedScrollingRowIndex = i
                            dgvDiscussion.Rows(i).Cells(j).Selected = True
                            Exit Sub
                        End If
                    Catch ex As Exception

                    End Try

                End If
            Next
        Next

        'Get the focus to the discussion, so people can scroll right away.
        dgvDiscussion.Select()
        dgvDiscussion.Focus()
    End Sub

    Private ucDis As ucDiscussion = Nothing

    Private Sub dgvDiscussion_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDiscussion.CellClick
        If e.RowIndex <> -1 AndAlso e.ColumnIndex <> -1 Then
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            If dgvDiscussion.SelectedRows.Count = 0 Then
                dgvDiscussion.Rows(e.RowIndex).Selected = True
            End If

            Dim setOfComments As DataSet = db.SecureQueryParams("SELECT co.[description], co.creation_date, co.created_by, wun.notificationTo " & _
                                                  " , wu1.firstName firstNameCreation, wu1.lastName lastNameCreation " & _
                                                  " , wu2.firstName firstNameDestination, wu2.lastName lastNameDestination, co.headerName " & _
                                                  " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " & _
                                                  " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " & _
                                                  " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " & _
                                                  " WHERE co.header_id = @1 AND co.line_id = @2 AND co.entity_id = @3 AND co.shipment_no = @4 AND (co.releaseNo = @5 OR co.releaseNo IS NULL)  ORDER BY co.serverCreationDate", _
                                                  dgvDiscussion.SelectedRows(0).Cells("header_id").Value _
                                                  , dgvDiscussion.SelectedRows(0).Cells("line_id").Value _
                                                  , dgvDiscussion.SelectedRows(0).Cells("ENTITY_ID").Value _
                                                  , dgvDiscussion.SelectedRows(0).Cells("shipment_no").Value _
                                                  , dgvDiscussion.SelectedRows(0).Cells("releaseNo").Value)
            rtbDiscussion.Clear()

            Dim com As New clsComments(Me.rtbDiscussion, setOfComments.Tables(0))
            'perform the jump and find      '*** needs rework
            Me.frmWorkbenchProjects.jumpAndFindNotificationWithoutUpdate(dgvDiscussion.SelectedRows(0).Cells("CommentID").Value)

            If dgvDiscussion.Columns(e.ColumnIndex).Name = "Reviewed" Then
                If Not dgvDiscussion.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Then
                    'Fire an update against the table
                    Dim notificationId As Integer = dgvDiscussion.Rows(e.RowIndex).Cells("notificationId").Value
                    Dim currentFirstDisplay As Integer = dgvDiscussion.FirstDisplayedScrollingRowIndex              'Store the first displayed row index
                    Dim currentColumnDisplay As Integer = dgvDiscussion.FirstDisplayedScrollingColumnIndex

                    db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", notificationId)
                    Me.loadIWasNotified()
                    dgvDiscussion.ClearSelection()
                    dgvDiscussion.Rows(e.RowIndex).Selected = True

                    'Put the user to the postion he was seing before so he does not see the tool jumping around
                    dgvDiscussion.FirstDisplayedScrollingRowIndex = currentFirstDisplay
                    dgvDiscussion.FirstDisplayedScrollingColumnIndex = currentColumnDisplay

                    Me.frmWorkbenchProjects.loadNumberOfNotifications()

                    'Make the value true!   Comes from the data source
                    ' dgvDiscussion.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = True

                End If
            End If

        End If

        Try
            Dim notificaionId As Integer
            If dgvDiscussion.Columns.Contains("notificationiD") Then
                If Not DBNull.Value.Equals(dgvDiscussion.SelectedRows(0).Cells("notificationiD").Value) Then
                    notificaionId = dgvDiscussion.SelectedRows(0).Cells("notificationiD").Value
                Else
                    notificaionId = 0
                End If

            Else
                notificaionId = 0
            End If
            Dim reviewed As Boolean
            If dgvDiscussion.Columns.Contains("Reviewed") Then
                reviewed = dgvDiscussion.SelectedRows(0).Cells("Reviewed").Value
            Else
                reviewed = True
            End If

            'Just have to execute the new user control
            rtbDiscussion.Visible = False
            If ucDis Is Nothing Then
                If dgvDiscussion.SelectedRows.Count > 0 Then
                    Dim uc As New ucDiscussion(dgvDiscussion.SelectedRows(0).Cells("header_id").Value, dgvDiscussion.SelectedRows(0).Cells("headerName").Value, dgvDiscussion.SelectedRows(0).Cells("line_id").Value, _
                                             dgvDiscussion.SelectedRows(0).Cells("shipment_no").Value, dgvDiscussion.SelectedRows(0).Cells("ENTITY_ID").Value, Nothing, Nothing, Me.organizationID, Me, notificaionId, reviewed, dgvDiscussion.SelectedRows(0).Cells("releaseNo").Value)

                    


                    Me.SplitContainer1.Panel2.Controls.Add(uc)
                    If dgvDiscussion.SelectedRows(0).Cells("Open").Value <> 0 Then
                        uc.propAllowToSave = True
                    Else
                        uc.propAllowToSave = False
                    End If

                    Me.ucDis = uc
                End If

            Else
                If dgvDiscussion.SelectedRows.Count > 0 Then

                    Me.ucDis.refreshComments(dgvDiscussion.SelectedRows(0).Cells("header_id").Value, "", dgvDiscussion.SelectedRows(0).Cells("line_id").Value, _
                                       dgvDiscussion.SelectedRows(0).Cells("shipment_no").Value, dgvDiscussion.SelectedRows(0).Cells("ENTITY_ID").Value, Nothing, Nothing, Me.organizationID, notificaionId, dgvDiscussion.SelectedRows(0).Cells("releaseNo").Value)


                    If dgvDiscussion.SelectedRows(0).Cells("Open").Value <> 0 Then
                        ucDis.propAllowToSave = True
                    Else
                        ucDis.propAllowToSave = False
                    End If

            End If

            End If
            Me.ucDis.cbDirect.Checked = False
        Catch ex As Exception

        End Try
       
    End Sub

    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        If dgvDiscussion.SelectedRows.Count > 0 Then
            'See if the user is allowed to review the notifications
            If strCurrentUser = Environment.UserName Then
                Dim notificationId As Integer = dgvDiscussion.SelectedRows(0).Cells("notificationId").Value
                If dgvDiscussion.SelectedRows(0).Cells("Reviewed").Value Then
                Else
                    db.SecureNonQueryParams("UPDATE whUserNotificationsView SET reviewed = 1 WHERE notificationId = @1", notificationId)
                    Me.loadIWasNotified()
                    Me.frmWorkbenchProjects.loadNumberOfNotifications()
                End If

            Else
                MessageBox.Show("You can view other people notification but not deselect the review field.")
            End If


        Else
            MessageBox.Show("Please select a row first.")
        End If

    End Sub

    Private Sub dgvDiscussion_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvDiscussion.CellFormatting
        'every second row will become beige
        If e.RowIndex Mod 2 = 0 Then
            e.CellStyle.BackColor = Color.Beige
        End If

    End Sub

    Private Sub btnCurrentDiscussion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrentDiscussion.Click
        btnDone.Enabled = False
        btnDone.Visible = False
        Me.loadCurrentDiscussion()

        'Get the focus to the discussion, so people can scroll right away.
        dgvDiscussion.Select()
        dgvDiscussion.Focus()

    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        btnDone.Enabled = False
        btnDone.Visible = False

        'Get the focus to the discussion, so people can scroll right away.
        dgvDiscussion.Select()
        dgvDiscussion.Focus()
    End Sub

   
    Private Sub CheckUserToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckUserToolStripMenuItem.Click
        Dim dlg As New dlgChooseName(Me.organizationID, "Users")
        dlg.ShowDialog()

        If dlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Me.strCurrentUser = dlg.propReturnUserName
            Me.loadIWasNotified()

        End If

    End Sub

    Private Sub SetupOutOfOfficeStatusToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetupOutOfOfficeStatusToolStripMenuItem.Click
        Dim dlg As New dlgOutOfOfficeTime
        dlg.ShowDialog()


    End Sub

    Private Sub dgvDiscussion_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvDiscussion.ColumnWidthChanged
        gridDisplay.saveChangesOfSingleColumn(e.Column.Name, "discussionTracking", e.Column.Width)
    End Sub

    Private Sub btnForReview_Click(sender As Object, e As EventArgs) Handles btnForReview.Click

        Me.loadIWasNotifiedAndDidNotReview()
        'Get the focus to the discussion, so people can scroll right away.
        dgvDiscussion.Select()
        dgvDiscussion.Focus()


    End Sub

    Public Sub loadIWasNotifiedAndDidNotReview()
        Try

            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            btnDone.Visible = True
            btnDone.Enabled = True

            Dim ds As DataSet = db.SecureQueryParams("SELECT TOP " & cbxNoRecords.SelectedItem & " co.[description] [Comment], co.creation_date [Creation Date], co.created_by, wun.notificationTo " &
                                                  " , wu1.lastName + ', ' +  wu1.firstName [Creation Name], co.commentID, co.ENTITY_ID, co.HEADER_ID, co.LINE_ID, co.SHIPMENT_NO,co.organizationID, co.releaseNo  " &
                                                  "  , wu2.lastName + ', ' + wu2.firstName [Notification To], wun.reviewed [Reviewed], wun.notificationId, co.headerName [Order], co.headerName " &
                                                     ", (SELECT COUNT(*) FROM " & strSalesOrder & " so WHERE (so.HEADER_ID = co.HEADER_ID) AND (so.LINE_NO = co.LINE_ID) AND so.ORGANIZATION_ID = co.organizationid  )  " &
                                              " + (SELECT count(*) FROM " & strPO & " po WHERE (po.PO_HEADER_ID = co.HEADER_ID) AND (po.LINE_NO = co.LINE_ID) AND (po.SHIPMENT_NO = co.SHIPMENT_NO) AND     (isnull(po.RELEASE_NO,0) = co.releaseNo) AND po.ship_to_organization_id = co.organizationID)   " &
                                              " + (SELECT COUNT(*) FROM " & strWIP & " jo WHERE (jo.WIP_ENTITY_ID = co.HEADER_ID) AND (jo.OPERATION_SEQ_NO = co.LINE_ID) AND jo.organization_id = co.organizationID )   " &
                                              " + (SELECT COUNT(*) FROM " & strSaveMRPDemand & " md WHERE      (co.HEADER_ID = md.ORDER_NO) AND  (co.organizationID = md.ORGANIZATION_ID) AND (co.LINE_ID = md.LINE_NO) AND  (co.SHIPMENT_NO = md.INVENTORY_ITEM_ID)  ) AS [Open]  " &
                                                  " FROM comments co LEFT JOIN warehouseUserNotifications wun ON co.CommentID = wun.commentid " &
                                                  " LEFT JOIN warehouse_users wu1 ON co.created_by = wu1.[user_Name] " &
                                                  " LEFT JOIN warehouse_users wu2 ON wun.notificationTo = wu2.[user_Name] " &
                                                  " WHERE wun.reviewed = 0 AND  co.entity_id IN ('poQueueView', 'wipQueueView', 'salesOrderLinesView', 'plannedOrder') AND co.organizationID = @1 AND wun.notificationTo = @2  ORDER BY co.creation_date DESC", organizationID, strCurrentUser)

            Dim comments As New clsComments(rtbDiscussion)

            dgvDiscussion.DataSource = ds.Tables(0)
            dgvDiscussion.Columns("notificationId").Visible = False

            'Iterate over the content and make everything that is not checked bold
            For i As Integer = 0 To dgvDiscussion.Rows.Count - 1
                If Not dgvDiscussion.Rows(i).Cells("Reviewed").Value Then
                    dgvDiscussion.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
                    Dim f As New Font("Verdana", 11, FontStyle.Bold, GraphicsUnit.Pixel)
                    dgvDiscussion.Rows(i).DefaultCellStyle.Font = f
                End If
            Next

            Me.alignDataTable(dgvDiscussion)
            '            dgvDiscussion.Columns("Reviewed").
            Me.applyHeaderName(ds.Tables(0))
        Catch ex As Exception

        End Try
    End Sub
End Class