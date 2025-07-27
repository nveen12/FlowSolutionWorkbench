Public Class clsMiscelleneousCalculations

    Private dbO As New DB.ORacleServerDB(My.Settings("OracleConnectionString"))  'connect to Oracle database (UAT)
    Private dbS As New DB.ServerDB(My.Settings("FlowConnectionString"), False)  'connect to SQL Server database  
    Private _naftaPartNumber As String

    'determine if a sales order line is NAFTA compliant.  (has less than 40% non-NAFTA purchased content)
    Public Function CalculateNAFTA(ByVal organizationID As Long, ByVal salesOrderNo As Long, ByVal salesLineNo As Long, ByVal strItemNo As String, ByVal lngProject As Long, ByVal dblunitSellingPrice As Double, ByVal rowNumber As Integer, ByRef dsValues As DataSet)
        _naftaPartNumber = strItemNo

        'For the time being just hardcode it to assembly which works in order to show the process!!!!!!!!!!!

        'Procedure to call to calculate the first NAFTA report...
        Dim sTmp As String
        Dim i As Integer = 0
        Dim lngMfgItemID As Long
        Dim lngSeq As Long
        Dim lngCategoryStructureID As Long = 0
        Dim dblTmp As Double
        Dim dblTmp2 As Double
        Dim blnTmp As Boolean
        Dim dsO As New DataSet 'Oracle
        Dim dsS As New DataSet 'from the SQL Server database
        Dim dbS As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim dbO As New DB.ORacleServerDB(My.Settings("OracleSTARConnectionString"))
        Dim lngOrg As Integer = organizationID
        Dim MARK_FOR_DELETION As String = "DELETE"

        'MAKE A SUMMARIZED BILL OF MATERIAL of all purchased components, thru all levels of the bom's
        dbS.NonQuery("DELETE FROM tblWork3")  'first empty the work table

        'lookup the inventory item ID for this assembly
        dsS = dbS.Query("SELECT INVENTORY_ITEM_ID FROM dbo.items WHERE ITEM_NO LIKE '" & strItemNo & "'")
        If dsS.Tables(0).Rows.Count > 0 Then
            lngMfgItemID = dsS.Tables(0).Rows(0).Item("INVENTORY_ITEM_ID")
        Else
            MsgBox("Item not available in data warehouse")
            Exit Function
        End If

        Do Until i > 99
            'get the bill of material for this manufactured part
            sTmp = "Select b.organization_id, b.bill_sequence_id, b.assembly_item_id, b.assembly_type, b.alternate_bom_designator, b.implementation_date, " & _
             "c.component_item_id, c.effectivity_date, c.disable_date, c.component_quantity, c.component_sequence_id, " & _
             "(SELECT segment1 FROM mtl_system_items_b msi WHERE msi.inventory_item_id = c.component_item_id AND rownum =1) comp_item_no, " & _
             "(SELECT description FROM mtl_system_items_b msi WHERE msi.inventory_item_id = c.component_item_id AND msi.organization_ID = b.organization_id AND rownum =1) description, " & _
             "(SELECT planning_make_buy_code FROM mtl_system_items_b msi WHERE msi.inventory_item_id = c.component_item_id AND msi.organization_ID = b.organization_id AND rownum =1) planning_make_buy_code " & _
             "FROM BOM_STRUCTURES_B b, BOM_COMPONENTS_B c " & _
             "WHERE b.bill_sequence_id = c.bill_sequence_id AND b.implementation_date <= SYSDATE AND b.alternate_bom_designator IS NULL " & _
             "AND b.assembly_type = 1 AND c.effectivity_date <= SYSDATE AND (c.disable_date IS NULL OR c.disable_date >= SYSDATE) " & _
             "AND b.organization_id = " & lngOrg & " AND b.assembly_item_id = " & lngMfgItemID
            dsO = dbO.Query(sTmp)

            'copy it into the work table
            For x = 0 To dsO.Tables(0).Rows.Count - 1   '                     item id        qty per      make/buy sequence id  item no  description, parent item_id (for testing only)                                                                                                                                            
                dbS.SecureNonQueryParams("INSERT INTO [tblWork3] ([Number1], [Number2], [Number3], [Number4], [Text1], [Text2], [Number9]) VALUES (@1, @2, @3, @4, @5, @6, @7)", _
                dsO.Tables(0).Rows(x).Item("component_item_id"), dsO.Tables(0).Rows(x).Item("component_quantity"), dsO.Tables(0).Rows(x).Item("planning_make_buy_code"), dsO.Tables(0).Rows(x).Item("component_sequence_id"), dsO.Tables(0).Rows(x).Item("comp_item_no"), dsO.Tables(0).Rows(x).Item("description"), dsO.Tables(0).Rows(x).Item("assembly_item_id"))
            Next x

            'pick the first manufactured item we find that has not been processed yet
            sTmp = "SELECT [Number1], [Number4], [Text1] FROM [tblWork3] WHERE [Number3] = 1 AND [Text3] IS NULL"
            dsS = dbS.Query(sTmp)

            'if we found something, set it up as the next mfg part to be decoded into its components, and mark it for deletion
            If dsS.Tables(0).Rows.Count > 0 Then
                lngSeq = dsS.Tables(0).Rows(0).Item("Number4") 'grab the component sequence id. this is unique across the organization
                lngMfgItemID = dsS.Tables(0).Rows(0).Item("Number1")    'and the component inventory item id
                dbS.NonQuery("UPDATE w SET [Text3] = '" & MARK_FOR_DELETION & "' FROM dbo.tblWork3 AS w WHERE [Number4] = " & lngSeq)    'mark it as processed, although we really will process it on the next go around thru the loop
            Else
                i = 100 'if we can't find any more mfg items, set the counter to the limit so we exit the loop
            End If
        Loop

        'place all Seiban PO's for this sales order line into 2nd working table
        If lngProject > 0 Then
            'select all seiban po's including closed ones, for this project   '*** how do we filter this by item_id's we need
            sTmp = "SELECT PLOC.ship_to_organization_id, POH.po_header_id, POH.segment1 po_no, POL.line_num line_no, POL.item_id, POL.unit_price, " & _
             "(SELECT project_id FROM PO_DISTRIBUTIONS_ALL PDA WHERE PDA.po_header_id = POL.po_header_id AND PDA.po_line_id = POL.po_line_id AND rownum = 1) as project_id " & _
             "FROM PO_HEADERS_ALL POH, PO_LINES_ALL POL, PO_LINE_LOCATIONS_ALL PLOC " & _
             "WHERE POL.po_header_id = POH.po_header_id AND PLOC.po_line_id = POL.po_line_id AND PLOC.ship_to_organization_id = " & lngOrg & " " & _
             "AND (SELECT project_id FROM PO_DISTRIBUTIONS_ALL PDA WHERE PDA.po_header_id = POL.po_header_id AND PDA.po_line_id = POL.po_line_id AND rownum = 1) = " & lngProject
            dsO = dbO.Query(sTmp)
            
            'place into 2nd work table
            dbS.NonQuery("DELETE FROM tblWork2")
            For x = 0 To dsO.Tables(0).Rows.Count - 1   '                       item id         cost         po no                                                                                                                             
                dbS.SecureNonQueryParams("INSERT INTO [tblWork2] ([Number1], [Number2], [Text1]) VALUES (@1, @2, @3)", _
                 dsO.Tables(0).Rows(x).Item("item_id"), dsO.Tables(0).Rows(x).Item("unit_price"), dsO.Tables(0).Rows(x).Item("po_no"))
            Next x
            
            'copy Seiban info into 1st work table: po number              unit price
            sTmp = "UPDATE w SET [Text5] = w2.Text1, [Number5] = w2.Number2 FROM dbo.tblWork3 AS w INNER JOIN dbo.tblWork2 AS w2 ON w.Number1 = w2.Number1 "
            dbS.NonQuery(sTmp)
            
        End If

        'copy item cost (Average and Pending) into work table
        sTmp = "UPDATE w SET [Number6] = i.item_cost, [Number7] = i.pending_cost FROM dbo.tblWork3 AS w INNER JOIN dbo.Items as i ON w.Number1 = i.inventory_item_id " & _
          "WHERE i.organization_id = " & lngOrg
        dbS.NonQuery(sTmp)

        'choose best cost option for NAFTA cost.   If there is a Seiban cost, use it.  otherwise, if there is an Average Cost, use it.  last choice is Pending Cost.
        sTmp = "UPDATE w SET [Number8] = [Number5] FROM dbo.tblWork3 AS w WHERE [Number5] IS NOT NULL"   'seiban
        dbS.NonQuery(sTmp)
        sTmp = "UPDATE w set [Number8] = [Number6] FROM dbo.tblWork3 AS w WHERE [Number8] IS NULL AND [Number6] > 0" 'average
        dbS.NonQuery(sTmp)
        sTmp = "UPDATE w set [Number8] = [Number7] FROM dbo.tblWork3 AS w WHERE [Number8] IS NULL AND [Number7] > 0" 'pending
        dbS.NonQuery(sTmp)

        '*** at this point we need to display the possible information and act on the missing information
        'Make a leftjoin on the Nafta Table and iterate into the table to add the appropriate information
        Me.appendPossibleNaftaInformation()


        'append all the NAFTA info to the summarized bom
        sTmp = "UPDATE w SET text6 = n.tariff, text7 = n.prefCriterion, text8 = n.netCost, text9 = n.producer, text10 = n.countryOfOrigin, text11 = n.vendorNo, boolean1 = n.NAFTAPart " & _
         "FROM dbo.tblWork3 w INNER JOIN dbo.NAFTA n ON n.itemNo = w.text1"
        dbS.NonQuery(sTmp)

        'calculate (b) non-Nafta content as the sum of all purchased parts costs (qty per x Nafta cost) where Nafta code is null or nafta code not like 'Y'
        dsS = dbS.Query("SELECT SUM(Number8 * Number2) AS nonNaftaContent FROM dbo.tblWork3 WHERE (Number3 = 2) AND (Number8 > 0) AND ([boolean1] = 0)")

        If Not DBNull.Value.Equals(dsS.Tables(0).Rows(0).Item("nonNaftaContent")) Then
            dblTmp = Math.Round(dsS.Tables(0).Rows(0).Item("nonNaftaContent"), 2)
        Else
            dblTmp = 0
        End If



        'calculate if the assy is NAFTA	 qualified
        blnTmp = False
        If dblunitSellingPrice > 0 Then
            dblTmp2 = Math.Round((dblunitSellingPrice - dblTmp) / dblunitSellingPrice, 2)
            If dblTmp2 >= 0.6 Then blnTmp = True 'if non qualifying content is less than 40% then it qualifies  *** business rule
        End If

        'Store the gathered information also in the handed data set
        dsValues.Tables(0).Rows(rowNumber).Item("NAFTAPart") = blnTmp
        dsValues.Tables(0).Rows(rowNumber).Item("producer") = "YES"
        dsValues.Tables(0).Rows(rowNumber).Item("netCost") = "NO"
        dsValues.Tables(0).Rows(rowNumber).Item("countryOfOrigin") = "USA"



        'Open the report viewer for the nearly created detail.
        Dim frm As New frmReportViewer("", salesOrderNo, salesLineNo, strItemNo, dblunitSellingPrice, dblTmp, dblTmp2, blnTmp)
        frm.Show()

        Return blnTmp

    End Function

    Private Sub appendPossibleNaftaInformation()
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        Dim ds As DataSet = db.Query("SELECT w.text1 as Item, w.text2 as [Description], w.number3 as [Pur Mfg], w.number2 as [Qty Per],w.Number8 AS Cost, n.vendorNo AS [Vendor No] ,n.tariff AS Tariff, n.prefCriterion AS [Criteria],n.netCost AS [Net Cost], n.producer AS [Producer] " & _
                                    " , n.countryOfOrigin AS [Country], n.NAFTAPart as Qualifies" & _
                                    " FROM tblWork3 w LEFT JOIN dbo.NAFTA n ON n.itemNo = w.text1 ")

        Dim dlg As New dlgNaftaItemConfirm(ds)
        dlg.ShowDialog()

    End Sub

    Public ReadOnly Property currentPartNumberForNafta()
        Get
            Return _naftaPartNumber
        End Get
    End Property
End Class
