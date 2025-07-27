Public Class clsFiltering
    Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))


    ''' <summary>
    ''' Creates an individual filter string to the entity and user which filters just to the 
    ''' </summary>
    ''' <param name="requestedEntity">The entity: salesOrderLinesView, WIPQueueView, POQueueView, MRPShortagesView</param>
    ''' <returns>An additional filter string to hand over to the query obejects.</returns>
    ''' <remarks>Filters for bot: Notifications and comments.</remarks>
    Public Function individualInvolvementString(ByVal requestedEntity As String)

        Dim strAdditionalFilter As String = ""
        Dim strUser As String = Environment.UserName

        If requestedEntity = "salesOrderLinesView" Then
            strAdditionalFilter = "AND ( convert(varchar,convert(integer,header_id)) + convert(varchar,convert(integer,line_no)) IN " & _
                               "(SELECT convert(varchar,convert(integer,comments.header_id)) + convert(varchar,convert(integer,comments.line_id)) FROM comments INNER JOIN salesOrderLinesView " & _
                               "ON comments.header_id = salesOrderLinesView.header_id  AND comments.line_id = salesOrderLinesView.line_no  AND comments.created_by = '" & strUser & "' AND entity_id = '" & requestedEntity & "') " & _
                               "OR  " & _
                               "convert(varchar,convert(integer,header_id)) + convert(varchar,convert(integer,line_no)) IN " & _
                               "(SELECT convert(varchar,convert(integer,warehouseUserNotifications.headerID)) + convert(varchar,convert(integer,warehouseUserNotifications.lineID)) " & _
                               "FROM warehouseUserNotifications INNER JOIN salesOrderLinesView  " & _
                               "ON warehouseUserNotifications.headerID = salesOrderLinesView.header_id  AND warehouseUserNotifications.lineID = salesOrderLinesView.line_no  " & _
                               " AND warehouseUserNotifications.createdBy = '" & strUser & "' AND notificationEntity = '" & requestedEntity & "')) "

        ElseIf requestedEntity = "POQueueView" Then

            strAdditionalFilter = "AND ( convert(varchar,convert(integer,po_header_id)) + convert(varchar,convert(integer,line_no)) IN " & _
                           "(SELECT convert(varchar,convert(integer,comments.header_id)) + convert(varchar,convert(integer,comments.line_id)) FROM comments INNER JOIN poQueueView " & _
                           "ON comments.header_id = poQueueView.po_header_id  AND comments.line_id = poQueueView.line_no  AND comments.created_by = '" & strUser & "' AND entity_id = '" & requestedEntity & "') " & _
                           "OR  " & _
                           "convert(varchar,convert(integer,po_header_id)) + convert(varchar,convert(integer,line_no)) IN " & _
                           "(SELECT convert(varchar,convert(integer,warehouseUserNotifications.headerID)) + convert(varchar,convert(integer,warehouseUserNotifications.lineID)) " & _
                           "FROM warehouseUserNotifications INNER JOIN poQueueView  " & _
                           "ON warehouseUserNotifications.headerID = poQueueView.po_header_id  AND warehouseUserNotifications.lineID = poQueueView.line_no  " & _
                           " AND warehouseUserNotifications.createdBy = '" & strUser & "' AND notificationEntity = '" & requestedEntity & "')) "

        ElseIf requestedEntity = "WIPQueueView" Then

            strAdditionalFilter = "AND ( convert(varchar,convert(integer,wip_entity_id)) + convert(varchar,convert(integer,OPERATION_SEQ_NO)) IN " & _
                           "(SELECT convert(varchar,convert(integer,comments.header_id)) + convert(varchar,convert(integer,comments.line_id)) FROM comments INNER JOIN WIPQueueView " & _
                           "ON comments.header_id = WIPQueueView.wip_entity_id  AND comments.line_id = WIPQueueView.operation_seq_no  AND comments.created_by = '" & strUser & "' AND entity_id = '" & requestedEntity & "') " & _
                           "OR  " & _
                           "convert(varchar,convert(integer,wip_entity_id)) + convert(varchar,convert(integer,operation_seq_no)) IN " & _
                           "(SELECT convert(varchar,convert(integer,warehouseUserNotifications.headerID)) + convert(varchar,convert(integer,warehouseUserNotifications.lineID)) " & _
                           "FROM warehouseUserNotifications INNER JOIN WIPQueueView  " & _
                           "ON warehouseUserNotifications.headerID = WIPQueueView.wip_entity_id  AND warehouseUserNotifications.lineID = WIPQueueView.operation_seq_no  " & _
                           " AND warehouseUserNotifications.createdBy = '" & strUser & "' AND notificationEntity = '" & requestedEntity & "')) "


        End If
       
        Return strAdditionalFilter

    End Function


    ''' <summary>
    ''' Adds the line to the watchlist of the user.
    ''' </summary>
    ''' <param name="entity">The entity this filter string should be created for.</param>
    ''' <param name="header">The header this watch should be attached to.</param>
    ''' <param name="line">The line, this watch should be attached to.</param>
    ''' <remarks>Just adds and watch if it is not already created for this order line.</remarks>
    Public Sub addToWatch(ByVal entity As String, ByVal header As Double, ByVal line As Double)
        'Find out the new watchID
        Dim ds As DataSet = db.Query("SELECT ISNULL(watchID,0) + 1 AS watchID FROM warehouseWatchList")
        'Look if the entity already is in the watchlist of this user, then do not add it.
        db.SecureNonQueryParams("INSERT INTO warehouseWatchList (watchID, entity, heaaderID, lineId, creationDate, createdBy) " & _
                                "VALUES (@1, @2, @3, @4, @5, @6)", ds.Tables(0).Rows(0).Item(0), entity, header, line, Format(Now(), "yyyyMMdd HH:mm"), Environment.UserName)

    End Sub

    ''' <summary>
    ''' Deletes the line from the user watch list.
    ''' </summary>
    ''' <param name="entity">The entity this watch entry is related to.</param>
    ''' <param name="header">The header information of the line. Example: Sales Order Line Header</param>
    ''' <param name="line">The line for the watch list.</param>
    Public Sub deleteFromWatch(ByVal entity As String, ByVal header As Double, ByVal line As Double)
        'Delte statement
        db.SecureNonQueryParams("DELETE FROM warehouseWatchList WHERE entity = @1 AND headerID = @2 AND lineID = @3", entity, header, line)
    End Sub


    ''' <summary>
    ''' Procedure that creates a query string for the backend.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub createQueryString()

    End Sub


End Class
