Imports System.Globalization
Imports System.Configuration
Imports System.DirectoryServices

Public Class clsTransformationASCP

    Private dbS As New DB.ServerDB(My.Settings("FlowConnectionString"), False)
    Private db As New DB.ServerDB(My.Settings("FlowConnectionString"), False)
    Private currentStep As Integer = 0
    Private instTime As Date
    Private runningInst
    Private r12Instance As Boolean = False



    Public Sub New(ByVal d As Date, ByVal command As String)
        'createExceptionMessages()
    End Sub

    Public Function runTransformation()




        'transformation steps to be executed for ASCP
        cleanse_dw_items()
        PrepareToCalculateShortages()
        CalculateShortages()
        EnrichOrderDataMSC()
        buildSupplyTypeDEscription()
        AttachComponentDetails()
        displayShortagesForOnHand()
        buildAdditionalInfoForOneDemandWithMultiplSOLinesUpwards()
        CalculateShortages()
        buildSalesOrderShortageDetail()
        CreateUnreleasedPartialAvailable()

        createExceptionMessages()
        Me.createInternalAndSupplyAlerts()

        Return 0

    End Function

    Private Sub createInternalAndSupplyAlerts()

        Dim x As Integer
        Dim sSQL As String
        Dim sTmp As String = ""
        Dim sTmp2 As String = ""
        Dim sPrevJobNo As String = ""
        
        Dim lngPrevOrg As Long = 0
        Dim lngPrevItem As Long = 0

        Dim dsS As New DataSet
        Dim conn As New SqlClient.SqlConnection(My.Settings("FLOWConnectionString"))

        Try
            'COPY SALES ORDER HOLDS into sales orders table
            Dim adpTmp2 As New SqlClient.SqlDataAdapter("SELECT * FROM sales_orders ORDER BY operating_unit_id, header_id, line_id ", conn)
            Dim cbTmp2 As New SqlClient.SqlCommandBuilder(adpTmp2)
            Dim dtTmp2 As New DataTable
            adpTmp2.Fill(dtTmp2)
            If dtTmp2.Rows.Count > 0 Then
                For Each rowTmp As DataRow In dtTmp2.Rows
                    'grab the header hold details for each sales order
                    sSQL = "SELECT * FROM sales_order_holds WHERE org_id = " & rowTmp("operating_unit_id") & " AND header_id = " & rowTmp("header_id") & " AND line_id IS NULL  ORDER BY last_update_date "
                    dsS = dbS.Query(sSQL)
                    If dsS.Tables(0).Rows.Count > 0 Then
                        sTmp = ""
                        For x = 1 To dsS.Tables(0).Rows.Count
                            sTmp += dsS.Tables(0).Rows(x - 1).Item("hold_name") & " (" & dsS.Tables(0).Rows(x - 1).Item("last_update_date").ToString & ") " & dsS.Tables(0).Rows(x - 1).Item("user_name") & "..."
                        Next x
                        rowTmp("holds_header") = sTmp
                    End If

                    'grab the line hold details for each sales order.  mostly redundant, try nesting w/above
                    sSQL = "SELECT * FROM sales_order_holds WHERE org_id = " & rowTmp("operating_unit_id") & " AND header_id = " & rowTmp("header_id") & " AND line_id = " & rowTmp("line_id") & "  ORDER BY last_update_date  "
                    dsS = dbS.Query(sSQL)
                    If dsS.Tables(0).Rows.Count > 0 Then
                        sTmp = ""
                        For x = 1 To dsS.Tables(0).Rows.Count
                            sTmp += dsS.Tables(0).Rows(x - 1).Item("hold_name") & " (" & dsS.Tables(0).Rows(x - 1).Item("last_update_date").ToString & ") " & dsS.Tables(0).Rows(x - 1).Item("user_name") & "..."
                        Next x
                        rowTmp("holds_line") = sTmp
                    End If

                Next
                adpTmp2.Update(dtTmp2)   'update the table
            End If
            dtTmp2.Dispose()
            adpTmp2.Dispose()
        Catch ex As Exception

        End Try

        createLoggingStamp("EnrichOrderDataMSC 3")

    End Sub

    Private Sub cleanse_dw_items()
        Try

            Dim x As Integer
            Dim y As Integer
            Dim sTmp As String
            Dim sSQL As String
            Dim sTable(0 To 3) As String
            Dim sColumn(0 To 6) As String
            Dim numberTouched As Integer = 0

            db.Connect()
            createLoggingStamp("cleanse_dw_items")

            'MAKE A LIST OF ALL ACTIVE ITEMS - to use for item-related queries, much faster than using the full items table
            'Insert all active items into the working table
            numberTouched = db.NonQuery("DELETE FROM tblWorkASCP")

            numberTouched = db.NonQuery("INSERT INTO tblWorkASCP (number1, number2)  (SELECT inventory_item_id, organization_id FROM open_boms              GROUP BY inventory_item_id, organization_id)")      'Open WIP Bill of materials

            numberTouched = db.NonQuery("INSERT INTO tblWorkASCP (number1, number2)  (SELECT inventory_item_id, organization_id FROM sales_orders           GROUP BY inventory_item_id, organization_id)")      'Open sales orders

            numberTouched = db.NonQuery("INSERT INTO tblWorkASCP (number1, number2)  (SELECT inventory_item_id, organization_id FROM jobs                   GROUP BY inventory_item_id, organization_id)")      'Open jobs

            numberTouched = db.NonQuery("INSERT INTO tblWorkASCP (number1, number2)  (SELECT inventory_item_id, organization_id FROM items_qty_oh           GROUP BY inventory_item_id, organization_id)")      'QTY on hand

            numberTouched = db.NonQuery("INSERT INTO tblWorkASCP (number1, number2)  (SELECT inventory_item_id, organization_id FROM items_safety_stock     GROUP BY inventory_item_id, organization_id)")      'Safety stocks

            numberTouched = db.NonQuery("INSERT INTO tblWorkASCP (number1, number2)  (SELECT inventory_item_id, organization_id FROM msc_demand             GROUP BY inventory_item_id, organization_id)")      'ASCP demand

            numberTouched = db.NonQuery("INSERT INTO tblWorkASCP (number1, number2)  (SELECT inventory_item_id, organization_id FROM msc_supply             GROUP BY inventory_item_id, organization_id)")      'ASCP supply

            numberTouched = db.NonQuery("INSERT INTO tblWorkASCP (number1, number2)  (SELECT inventory_item_id, ship_to_organization_id FROM po             GROUP BY inventory_item_id, ship_to_organization_id)")  'po's 
            'Group another time to eliminate duplicates, clear destination table
            numberTouched = db.NonQuery("DELETE FROM tblWork2ASCP")

            numberTouched = db.NonQuery("INSERT INTO tblWork2ASCP (number1, number2) (SELECT wt1.number1, wt1.number2 FROM tblWorkASCP wt1 GROUP BY wt1.number1, wt1.number2)")

            numberTouched = db.NonQuery("DELETE FROM active_items")

            'Insert unique items into active items table.
            sSQL = "INSERT INTO active_items (INVENTORY_ITEM_ID, ITEM_NO, DESCRIPTION, ORGANIZATION_ID, FULL_LEAD_TIME, FIXED_LEAD_TIME, STD_LOT_SIZE, PLANNING_MAKE_BUY_CODE, ITEM_COST, ITEM_TYPE, BOM_ITEM_TYPE, MINIMUM_ORDER_QUANTITY, FIXED_LOT_MULTIPLIER, PLANNER_CODE, WIP_SUPPLY_TYPE, WIP_SUPPLY_LOCATOR_ID, WIP_SUPPLY_SUBINVENTORY, ITEM_CATALOG_GROUP_ID, LAST_UPDATE_DATE_CST, CREATION_DATE_MSI, LAST_UPDATE_DATE_MSI, START_DATE_ACTIVE, END_DATE_ACTIVE, QTY_ONHAND, QTY_WIP, QTY_ALLOCATED, QTY_ALLOC_JOB, QTY_ON_ORDER, SAFETY_STOCK_QUANTITY, CREATED_BY, CREATION_DATE, LAST_UPDATED_BY, LAST_UPDATE_DATE, LAST_UPDATE_LOGIN, UNIT_WEIGHT, WEIGHT_UOM_CODE, VOLUME_UOM_CODE, PRIMARY_UOM_CODE, PRIMARY_UNIT_OF_MEASURE, BUYER, ITEM_STATUS, MAX_MINMAX_QUANTITY, MIN_MINMAX_QUANTITY, MRP_PLANNING_CODE, END_ASSEMBLY_PEGGING_FLAG) " & _
               " SELECT i.INVENTORY_ITEM_ID, i.ITEM_NO, i.DESCRIPTION, i.ORGANIZATION_ID, i.FULL_LEAD_TIME, i.FIXED_LEAD_TIME, i.STD_LOT_SIZE, i.PLANNING_MAKE_BUY_CODE, i.ITEM_COST, i.ITEM_TYPE, i.BOM_ITEM_TYPE, i.MINIMUM_ORDER_QUANTITY, i.FIXED_LOT_MULTIPLIER, i.PLANNER_CODE, i.WIP_SUPPLY_TYPE, i.WIP_SUPPLY_LOCATOR_ID, i.WIP_SUPPLY_SUBINVENTORY, i.ITEM_CATALOG_GROUP_ID, i.LAST_UPDATE_DATE_CST, i.CREATION_DATE_MSI, i.LAST_UPDATE_DATE_MSI, i.START_DATE_ACTIVE, i.END_DATE_ACTIVE, i.QTY_ONHAND, i.QTY_WIP, i.QTY_ALLOCATED, i.QTY_ALLOC_JOB, i.QTY_ON_ORDER, i.SAFETY_STOCK_QUANTITY, i.CREATED_BY, i.CREATION_DATE, i.LAST_UPDATED_BY, i.LAST_UPDATE_DATE, i.LAST_UPDATE_LOGIN, i.UNIT_WEIGHT, i.WEIGHT_UOM_CODE, i.VOLUME_UOM_CODE, i.PRIMARY_UOM_CODE, i.PRIMARY_UNIT_OF_MEASURE, i.BUYER, i.INVENTORY_ITEM_STATUS_CODE, i.MAX_MINMAX_QUANTITY, i.MIN_MINMAX_QUANTITY, i.MRP_PLANNING_CODE, i.END_ASSEMBLY_PEGGING_FLAG  " & _
              " FROM [items] i  INNER JOIN tblWork2ASCP w  ON i.inventory_item_id = w.number1 AND i.organization_id = w.number2 "
            db.NonQuery(sSQL)
            db.NonQuery("UPDATE active_items SET qty_onhand = 0, qty_wip = 0, qty_allocated = 0, qty_alloc_job = 0, qty_on_order = 0, safety_stock_quantity = 0") 'get rid of null values
            createLoggingStamp("cleanse_dw_items")

            ''COPY CATEGORIES AND DRAWINGS to the active items and open orders tables
            sTable(0) = "ACTIVE_ITEMS"
            sTable(1) = "JOBS"
            sTable(2) = "PO"
            sTable(3) = "SALES_ORDERS"
            sColumn(0) = "FSG_COMMODITY_CODE"
            sColumn(1) = "FSG_IND_PAINT_PARTS"
            sColumn(2) = "FSG_MATERIAL_CODE"
            sColumn(3) = "FSG_PRODUCT_CODE"
            sColumn(4) = "FSG_SALES_TYPE"
            sColumn(5) = "FSG_SPARES_CLASS"
            sColumn(6) = "DRAWING"
            'copy categories into active items table
            For x = 0 To 5
                If x = 3 Then sTmp = "+ '.' + ic.segment2 + '.' + ic.segment3 + '.' + ic.segment4 + '.' + ic.segment5 " Else sTmp = ""
                sSQL = "UPDATE t  SET " & sColumn(x) & " = ic.segment1" & sTmp & " FROM " & sTable(0) & " t  INNER JOIN itemCategories ic  " & _
                 "ON t.organization_id = t.organization_id AND t.inventory_item_id = ic.inventory_item_id AND ic.category_description LIKE '" & sColumn(x) & "'"
                db.NonQuery(sSQL)
            Next
            'copy drawing numbers into active items table 
            sSQL = "UPDATE ai  SET Drawing = d.Drawing FROM active_items ai INNER JOIN whOrganizationDefinition od ON ai.organization_ID = od.organizationID INNER JOIN Drawings d ON od.masterItemOrgID = d.organization_id  AND ai.inventory_item_id = d.inventory_item_id"
            db.NonQuery(sSQL)

            'copy the categories, drawing no's from active items to order tables
            For x = 1 To UBound(sTable)
                If sTable(x) = "PO" Then sTmp = "ship_to_" Else sTmp = "" 'we really should change this org id name in the po table ***
                sSQL = "UPDATE t  SET "
                For y = 0 To UBound(sColumn)
                    sSQL += sColumn(y) & " = t0." & sColumn(y) & ", "
                Next
                sSQL = Left(sSQL, Len(sSQL) - 2) 'get rid of the final comma 
                sSQL += "  FROM " & sTable(x) & " t  INNER JOIN " & sTable(0) & " t0 ON t." & sTmp & "organization_id = t0.organization_id AND t.inventory_item_id = t0.inventory_item_id "
                'keep the old values - no wipe out of already mapped columns
                For y = 0 To UBound(sColumn)
                    sSQL += " AND t." & sColumn(y) & " IS NOT NULL "
                Next

                db.NonQuery(sSQL)
            Next
            createLoggingStamp("cleanse_dw_items")

            'Copy latest safety stocks into active items table 
            db.NonQuery("DELETE FROM tblWorkASCP")
            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3) (SELECT s1.organization_id, s1.inventory_item_id  " & _
              ", s1.safety_stock_quantity FROM items_safety_stock s1 WHERE s1.effectivity_date = (SELECT max(effectivity_date) ed " & _
              " FROM items_safety_stock s2 WHERE s1.inventory_item_id = s2.inventory_item_id " & _
              " AND s1.organization_id = s2.organization_id) AND s1.safety_stock_quantity > 0 )")
            db.NonQuery("UPDATE ai SET safety_stock_quantity = wt1.number3 FROM active_items ai JOIN tblWorkASCP wt1 ON ai.organization_id = wt1.number1 " & _
              " AND ai.inventory_item_id = wt1.number2 ")

            'copy qty on hand to active items table  
            db.NonQuery("DELETE FROM tblWorkASCP")
            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3) (SELECT organization_id, inventory_item_id, sum(qty_onhand) AS SumOfQtyOnhand FROM items_qty_oh " & _
               " GROUP BY organization_id, inventory_item_id)")
            db.NonQuery("UPDATE ai SET qty_onhand = wt1.number3 FROM active_items ai JOIN tblWorkASCP wt1 ON ai.organization_id = wt1.number1 AND ai.inventory_item_id = wt1.number2")

            'copy qty on open PO's to active items table  
            db.NonQuery("DELETE FROM tblWorkASCP")
            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3) (SELECT ship_to_organization_id, inventory_item_id, SUM(quantity_ordered - qty_received) AS qtyOnOrders FROM po GROUP BY ship_to_organization_id, inventory_item_id )")
            db.NonQuery("UPDATE ai SET qty_on_order = wt1.number3 FROM active_items ai JOIN tblWorkASCP wt1 ON ai.organization_id = wt1.number1 AND ai.inventory_item_id = wt1.number2")

            'copy qty allocated for jobs to active items table  
            db.NonQuery("DELETE FROM tblWorkASCP")
            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3) (SELECT organization_id, inventory_item_id, sum(required_quantity - quantity_issued) AS qtyAlloc " & _
             " FROM open_boms GROUP BY organization_id, inventory_item_id)")
            db.NonQuery("UPDATE ai SET qty_alloc_job = wt1.number3 FROM active_items ai JOIN tblWorkASCP wt1 ON ai.organization_id = wt1.number1 AND ai.inventory_item_id = wt1.number2")

            'copy qty allocated to sales orders  to active items table
            db.NonQuery("DELETE FROM tblWorkASCP")
            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3) (SELECT organization_id, inventory_item_id, Sum(ordered_quantity ) AS allocSO " & _
             " FROM sales_orders  GROUP BY organization_id, inventory_item_id)")
            db.NonQuery("UPDATE ai SET qty_allocated = wt1.number3 FROM active_items ai JOIN tblWorkASCP wt1 ON ai.organization_id = wt1.number1 AND ai.inventory_item_id = wt1.number2")
            db.NonQuery("UPDATE po SET age = CONVERT(FLOAT,(getDate() - creation_date_pol))") 'age of PO's.  really should go in Enrich Order Data routine ***

            'Chesapeake specific item cleansing:
            db.NonQuery("UPDATE active_items SET planning_make_buy_code = 2 WHERE organization_id IN (881,882,883) AND planner_code LIKE 'S-CNTR'")

            createLoggingStamp("cleanse_dw_items")

        Catch ex As Exception
            createErrorLog("cleanse_dw_items", ex)
        Finally
            db.Disconnect()
        End Try
    End Sub

    Private Sub PrepareToCalculateShortages()

        Try

            Dim sSQL As String
            Dim sTmp As String
            Dim sTable(0 To 4) As String
            Dim x As Integer

            'Connect to the database
            dbS.Connect()
            createLoggingStamp("PrepareToCalculateShortages")

            'Data cleansing - we can only see true project numbers. This should also drive the correct on hand situation
            dbS.NonQuery("TRUNCATE table tblWorkASCP ")
            dbS.NonQuery("INSERT INTO tblWorkASCP(number1, number2)(SELECT d.demand_id, d.organization_id FROM msc_demand d INNER JOIN msc_supply s ON d.demand_id = s.demand_id AND d.organization_id = s.organization_id WHERE d.project_id IS NOT NULL AND s.project_id IS NULL GROUP BY  d.demand_id, d.organization_id) ")
            dbS.NonQuery("UPDATE d SET project_id = NULL FROM msc_demand d INNER JOIN tblWorkASCP t ON d.organization_id = t.number2 AND d.demand_id = t.number1 ")

            dbS.NonQuery("UPDATE msc_demand SET usingAssemblyLeadTime = NULL, parentJobOrderLeadTime = NULL, end_demand_id = NULL, level_no = NULL, purchase_order_id = NULL, purch_line_no = NULL, po_promised_date = NULL, purchase_order_no = NULL, qty_open = NULL, vendor_name = NULL, source_vendor_id = NULL, neededBy = NULL, PARENT_SALES_ORDER_ID = NULL, PARENT_SALES_ORDER_LINE = NULL, Short = NULL, temp = NULL, consumptionPriorityDate = NULL, job_id = NULL, job_no = NULL, start_quantity_hdr = NULL, start_date = NULL, status = NULL, jobDuration = NULL ") 'Reset the mrp demand table
            dbS.NonQuery("UPDATE sales_orders SET demand_id = NULL, short = NULL, job_no = NULL, po_no = NULL, qty_onhand = NULL ") 'Reset the sales orders table
            dbS.NonQuery("UPDATE po SET qty_onhand = NULL")
            dbS.NonQuery("UPDATE jobs SET qty_onhand = NULL")
            dbS.NonQuery("UPDATE msc_demand SET qty_onhand = NULL")
            dbS.NonQuery("UPDATE msc_supply SET qty_onhand = NULL")

            'copy wip supply type, make buy code, safety stock into MRP Demand
            sSQL = "UPDATE d SET WIP_SUPPLY_TYPE = i.WIP_SUPPLY_TYPE, " & _
            " SAFETY_STOCK_QUANTITY = i.SAFETY_STOCK_QUANTITY, MAKE_BUY = i.PLANNING_MAKE_BUY_CODE, PLANNER_CODE = i.PLANNER_CODE  " & _
             "FROM msc_demand d  INNER JOIN active_items i  ON d.organization_id = i.organization_id " & _
             " AND d.inventory_item_id = i.inventory_item_id "
            dbS.NonQuery(sSQL)
            'Update planner code in msc supply
            dbS.NonQuery("UPDATE s SET s.planner_code = a.planner_code FROM msc_supply s INNER JOIN active_items a ON s.organization_id = a.organization_id AND s.inventory_item_id = a.inventory_item_id WHERE s.planner_code is NULL")

            '*** Here we need to transact before in order to re-enable the link
            'POST JOB DURATIONS into mrp demand 
            dbS.NonQuery("DELETE FROM tblWorkASCP")
            sSQL = "INSERT INTO tblWorkASCP (Number1, Number2, Date1, Date2) SELECT d.organization_id, d.parent_job_order_id, Max(jobs.last_unit_completion_date), Min(jobs.first_unit_start_date)  " & _
               "FROM msc_demand d  INNER JOIN jobs  ON d.organization_id = jobs.organization_id AND d.parent_job_order_id = jobs.wip_entity_id  " & _
               "GROUP BY d.organization_id, d.parent_job_order_id "
            dbS.NonQuery(sSQL)
            sSQL = "UPDATE d SET parentJobOrderLeadTime = ROUND(CONVERT(FLOAT, w.Date1 - w.Date2)+.51,0)  FROM msc_demand d  INNER JOIN tblWorkASCP w  " & _
             "ON d.parent_job_order_id = w.Number2 AND d.organization_id = w.Number1  "
            dbS.NonQuery(sSQL)
            'also post item fixed lead time in case we can't find a job for this demand  'override to 7 days lead time!  lead times are messed up ***
            sSQL = "UPDATE d SET usingAssemblyLeadTime = i.fixed_lead_time FROM msc_demand d  INNER JOIN active_items i  " & _
             "ON d.using_assembly_item_id = i.inventory_item_id AND d.organization_id = i.organization_id  " & _
             "WHERE d.using_assembly_item_id <> d.inventory_item_id"
            dbS.NonQuery(sSQL)
            createLoggingStamp("PrepareToCalculateShortages")

            '*** Here we need to prepare to the different approach of ASCP towards the tables
            'update the string concatenation from ascp into the former approache
            sSQL = "UPDATE msc_demand  SET parent_sales_order_no = (left(msc_parent_sales_order_no,charindex('.',msc_parent_sales_order_no)))"
            dbS.NonQuery(sSQL)
            dbS.NonQuery("UPDATE msc_demand SET parent_sales_order_line = SUBSTRING ( msc_parent_sales_order_no ,charindex('(',msc_parent_sales_order_no) +1, len(msc_parent_sales_order_no) - charindex('(',msc_parent_sales_order_no) -1)")
            dbS.NonQuery("UPDATE msc_demand SET parent_sales_order_line = SUBSTRING ( parent_sales_order_line,0, charindex('.',parent_sales_order_line))")


            'This will leave the., so get rid of this
            sSQL = "UPDATE msc_demand SET parent_sales_order_no = REPLACE(parent_sales_order_no,'.','')"
            dbS.NonQuery(sSQL)

            'Sometimes we have the condition of not seeing the sales order line number in the msc_parent_sales_order_no but only the following value: 1186083.CHP Progress Billing.ORDER ENTRY this we need to correct
            dbS.NonQuery("UPDATE d SET parent_sales_order_line =   so.line_no FROM msc_demand d INNER JOIN sales_orders so ON d.item_no = so.item_no AND d.parent_sales_order_no = so.order_no and d.organization_id = so.organization_id WHERE parent_sales_order_no = parent_sales_order_line ")


            'update po header information
            db.NonQuery(" UPDATE s SET s.purchase_order_id = po.po_header_id FROM msc_supply s INNER JOIN po ON po.ship_to_organization_id = s.organization_id AND po.po_line_id = s.po_line_id  " & _
                       " WHERE s.po_line_id IS NOT NULL ")

            'update job id information for the work orders
            db.NonQuery(" UPDATE s SET s.job_id = j.wip_entity_id FROM msc_supply s INNER JOIN jobs j ON j.job_no = s.order_number WHERE s.order_number IS NOT NULL AND s.supply_type_desc IN ('Work order','Work order co-product/by-product') ")

            'Update the sales order line table with the correspondin demand id to connect msc_demand to sales_orders
            '*** May have to reconsider the project id - Assuming ASCP will handle it
            sSQL = "UPDATE S SET s.demand_id = d.demand_id FROM sales_orders s LEFT JOIN msc_demand d ON s.order_no = d.parent_sales_order_no AND s.line_no = d.parent_sales_order_line" & _
                    " AND s.inventory_item_id = d.inventory_item_id  "
            dbS.NonQuery(sSQL)

            'sSQL = "UPDATE S SET s.demand_id = d.demand_id FROM sales_orders s LEFT JOIN msc_demand d ON s.order_no = d.parent_sales_order_no " & _
            '        " AND s.inventory_item_id = d.inventory_item_id WHERE d.demand_type_desc = 'Sales Orders' AND s.demand_id is null"
            'dbS.NonQuery(sSQL)

            'Update top level information in msc demand table
            sSQL = " UPDATE d SET END_DEMAND_ID = d.DEMAND_ID, LEVEL_NO = 0, neededBy = d.MRP_DATE " & _
                    " FROM msc_demand d WHERE d.demand_type_desc = 'Sales order MDS'; "
            dbS.NonQuery(sSQL)

            'Update top level information in msc demand table
            sSQL = " UPDATE d SET END_DEMAND_ID = d.DEMAND_ID, LEVEL_NO = 0, neededBy = d.MRP_DATE " & _
                    " FROM msc_demand d WHERE d.demand_type_desc = 'Sales Orders' AND d.END_DEMAND_ID IS NULL; "
            dbS.NonQuery(sSQL)

            'update the MRP Demand table to show which demands are "top level" - i.e. from sales orders - ***1 here we need to follow the approach from ASCP
            sSQL = "UPDATE d SET PARENT_SALES_ORDER_ID = s.HEADER_ID FROM sales_orders s INNER JOIN msc_demand d ON s.order_no = d.parent_sales_order_no " & _
                     " AND s.inventory_item_id = d.inventory_item_id WHERE d.demand_type_desc = 'Sales order MDS'  "
            dbS.NonQuery(sSQL)

            sSQL = "UPDATE d SET PARENT_SALES_ORDER_ID = s.HEADER_ID FROM sales_orders s INNER JOIN msc_demand d ON s.order_no = d.parent_sales_order_no " & _
                     " AND s.inventory_item_id = d.inventory_item_id WHERE d.demand_type_desc = 'Sales Orders'  "
            dbS.NonQuery(sSQL)

            createLoggingStamp("PrepareToCalculateShortages")

            dbS.NonQuery("UPDATE msc_demand SET QTY_ONHAND = 0")                          ' do not forget to switch this back in order to - needs to be done because the statement above does not touch everything

            'sSQL = "UPDATE d SET project_ID = w.Number3 FROM dbo.mrp_demand AS d INNER JOIN dbo.tblWorkASCP AS w " & _
            '     "ON d.ORGANIZATION_ID = w.Number1 AND d.DEMAND_ID = w.Number2"
            'dbS.NonQuery(sSQL)

            'List for end_assembly_pegging_flag

            'I Hard Pegging
            'y End Assembly Pegging
            'A Soft Pegging
            'B End Assembly / Soft Pegging
            'x End Assembly / Hard Pegging
            'N None

            'POST project On Hand Inventory to MRP Demand, Sales Orders, Jobs and PO's
            'these are the five tables that need the on hand qty updated, segregated by Seiban 
            sTable(0) = "msc_demand"
            sTable(1) = "sales_orders"
            sTable(2) = "jobs"
            sTable(4) = "po"
            sTable(3) = "msc_supply"
            sTmp = "organization_id"
            For x = LBound(sTable) To UBound(sTable)
                If x = UBound(sTable) Then sTmp = "ship_to_organization_id"
                'POST Project On Hand Inventory to each MRP Demand....                    *** will break if site uses multiple uoms for same item
                sSQL = "UPDATE d SET QTY_ONHAND = q.QTY_ONHAND FROM dbo." & sTable(x) & " AS d  INNER JOIN dbo.items_qty_oh AS q  " & _
                 "ON q.ORGANIZATION_ID = d." & sTmp & " AND q.INVENTORY_ITEM_ID = d.INVENTORY_ITEM_ID AND isnull(q.PROJECT_ID,0) = isnull(d.PROJECT_ID,0)  "
                dbS.NonQuery(sSQL)
                ''...and the non-project inventory to all non-project demand . This assumes that we have always a SEIBAN number on the sales order line!
                'sSQL = "UPDATE d SET QTY_ONHAND = q.QTY_ONHAND FROM dbo." & sTable(x) & "  AS d  INNER JOIN dbo.items_qty_oh AS q  " & _
                ' "ON q.ORGANIZATION_ID = d." & sTmp & "  AND q.INVENTORY_ITEM_ID = d.INVENTORY_ITEM_ID INNER JOIN items i ON q.organization_id = i.organization_id AND q.inventory_item_id = i.inventory_item_id  " & _
                ' "WHERE i.end_assembly_pegging_flag NOT IN ('X', 'I') "                                                                     '*** need to do this only for the hard pegged items
                ''this will leave items which were switched back to 

                'dbS.NonQuery(sSQL)
                dbS.NonQuery("UPDATE " & sTable(x) & " SET QTY_ONHAND = 0 WHERE QTY_ONHAND IS NULL")
            Next



            createLoggingStamp("PrepareToCalculateShortages")

            'now that we are finished determining how long each job takes, we can delete all completed jobs that don't have any mrp demand.    if there is demand, we need TO OPEN UP THE LAST OP (say ISSUE MATERIAL) ***
            'we need to prepare the demand table to show the right information here like the column order_number in the supply table will hold the discrete job number
            dbS.NonQuery("UPDATE jobs SET temp = NULL")
            dbS.NonQuery("UPDATE j SET temp = 'temp' FROM jobs j INNER JOIN msc_demand d  ON j.organization_id = d.organization_id AND j.wip_entity_id = d.parent_job_order_id")
            dbS.NonQuery("DELETE FROM jobs WHERE temp IS NULL and STATUS_TYPE NOT IN (1,3,6)")

            'BEGIN PROCESS OF CALCULATING TRUE 'NEED BY' DATES
            'show which jobs and po's will fulfill the sales orders.  first match jobs, that have lower level demand
            sSQL = "UPDATE d SET job_no = j.job_no, job_id = j.wip_entity_id, status = j.status_type, start_quantity_hdr = j.start_quantity_hdr  FROM mrp_demand d  INNER JOIN jobs j  " & _
              "ON d.ORGANIZATION_ID = j.ORGANIZATION_ID AND d.INVENTORY_ITEM_ID = j.INVENTORY_ITEM_ID AND d.PROJECT_ID = j.PROJECT_ID  " & _
              "WHERE j.temp IS NOT NULL "
            dbS.NonQuery(sSQL)
            '2nd match jobs that have no lower level demand  most of these are probably seiban jobs that mrp wants to cancel
            sSQL = "UPDATE d SET job_no = j.job_no, job_id = j.wip_entity_id, status = j.status_type, start_quantity_hdr = j.start_quantity_hdr  FROM mrp_demand d  INNER JOIN jobs j  " & _
             "ON d.ORGANIZATION_ID = j.ORGANIZATION_ID AND d.INVENTORY_ITEM_ID = j.INVENTORY_ITEM_ID AND d.PROJECT_ID = j.PROJECT_ID  " & _
             "WHERE d.job_id IS NULL "
            dbS.NonQuery(sSQL)
            '3rd match PO's
            sSQL = "UPDATE d SET purchase_order_id = p.PO_HEADER_ID, purchase_order_no = p.PO_NO, purch_line_no = p.LINE_NO, qty_open = (p.QUANTITY_ORDERED - p.QTY_RECEIVED), source_vendor_id = p.VENDOR_ID, vendor_name = p.VENDOR_NAME  " & _
              "FROM dbo.mrp_demand d  INNER JOIN dbo.PO p  ON d.INVENTORY_ITEM_ID = p.INVENTORY_ITEM_ID AND d.PROJECT_ID = p.PROJECT_ID AND d.ORGANIZATION_ID = p.SHIP_TO_ORGANIZATION_ID  "
            dbS.NonQuery(sSQL)
            'copy any 'freebie' end-demand-id's we get, because the project numbers match
            sSQL = "UPDATE d set end_demand_id = d2.end_demand_id, level_no = d2.level_no + 1 FROM mrp_demand d  INNER JOIN mrp_demand d2  ON d.organization_id = d2.organization_id AND d.project_id = d2.project_id " & _
             "WHERE d.end_demand_id IS NULL  AND d2.end_demand_id IS NOT NULL"
            dbS.NonQuery(sSQL)
            createLoggingStamp("PrepareToCalculateShortages")

            'set the Need By date for lower level demand, because the top level job is scheduled differently than the sales order scheduled ship date  
            sSQL = "UPDATE d SET level_no = 1, end_demand_id = d2.end_demand_id, neededBy = DATEADD(d, - d.parentJobOrderLeadTime, d2.neededBy)  FROM mrp_demand d  INNER JOIN mrp_demand d2  " & _
              "ON d.parent_job_order_id = d2.job_id AND d.organization_id = d2.organization_id   WHERE d2.level_no = 0"
            dbS.NonQuery(sSQL)

            'for project demand with no Parent Job, set the Need By date based on the linked project sales order.  this is only necessary because Chpk is using soft pegging so MRP doesn't link them properly   
            sSQL = "UPDATE d  SET neededBy = DATEADD(d, - d.usingAssemblyLeadTime, d2.neededBy)  FROM mrp_demand AS d  INNER JOIN mrp_demand AS d2 " & _
             "ON d.USING_ASSEMBLY_ITEM_ID = d2.INVENTORY_ITEM_ID AND d.PROJECT_ID = d2.PROJECT_ID AND d.END_DEMAND_ID = d2.END_DEMAND_ID AND d.ORGANIZATION_ID = d2.ORGANIZATION_ID " & _
             "WHERE d2.LEVEL_NO = 0 AND d.LEVEL_NO = 1 AND d.neededBy IS NULL AND d.PARENT_PLANNED_ORDER_ID IS NOT NULL"
            dbS.NonQuery(sSQL)

            dbS.NonQuery("UPDATE d SET neededBy = DATEADD(d,-2,neededBy) FROM mrp_demand d WHERE DATEPART(dw,neededBy) = 1")    'change Sunday to prior Friday
            dbS.NonQuery("UPDATE d SET neededBy = DATEADD(d,-1,neededBy) FROM mrp_demand d WHERE DATEPART(dw,neededBy) = 7")    'change Saturday to prior Friday

            'for second level down, correct the MRP date by matching parent item & seiban jobs to child.   *** &&& why are we only doing two levels here?  HMB might need 5!
            sSQL = "UPDATE d SET level_no = 2, end_demand_id = d2.end_demand_id, neededBy = DATEADD(d, - d.parentJobOrderLeadTime, d2.neededBy)  " & _
             "FROM dbo.mrp_demand d  INNER JOIN dbo.mrp_demand d2  ON d.ORGANIZATION_ID = d2.ORGANIZATION_ID AND d.PARENT_JOB_ORDER_ID = d2.JOB_ID " & _
             "WHERE d.LEVEL_NO IS NULL AND d2.LEVEL_NO = 1"
            dbS.NonQuery(sSQL)
            dbS.NonQuery("UPDATE d SET neededBy = DATEADD(d,-2,neededBy) FROM mrp_demand d WHERE DATEPART(dw,neededBy) = 1")    'change Sunday to prior Friday duplicated
            dbS.NonQuery("UPDATE d SET neededBy = DATEADD(d,-1,neededBy) FROM mrp_demand d WHERE DATEPART(dw,neededBy) = 7")    'change Saturday to prior Friday duplicated
            createLoggingStamp("PrepareToCalculateShortages")

            'Added the next levels: +++++ Change to apply for more levels
            'Level 3
            sSQL = "UPDATE d SET level_no = 3, end_demand_id = d2.end_demand_id, neededBy = DATEADD(d, - d.parentJobOrderLeadTime, d2.neededBy)  " & _
                "FROM dbo.mrp_demand d  INNER JOIN dbo.mrp_demand d2  ON d.ORGANIZATION_ID = d2.ORGANIZATION_ID AND d.PARENT_JOB_ORDER_ID = d2.JOB_ID " & _
                "WHERE d.LEVEL_NO IS NULL AND d2.LEVEL_NO = 2"
            dbS.NonQuery(sSQL)

            createLoggingStamp("PrepareToCalculateShortages")

            'Level 4
            sSQL = "UPDATE d SET level_no = 4, end_demand_id = d2.end_demand_id, neededBy = DATEADD(d, - d.parentJobOrderLeadTime, d2.neededBy)  " & _
                       "FROM dbo.mrp_demand d  INNER JOIN dbo.mrp_demand d2  ON d.ORGANIZATION_ID = d2.ORGANIZATION_ID AND d.PARENT_JOB_ORDER_ID = d2.JOB_ID " & _
                       "WHERE d.LEVEL_NO IS NULL AND d2.LEVEL_NO = 3"
            dbS.NonQuery(sSQL)

            'Level 5
            sSQL = "UPDATE d SET level_no = 5, end_demand_id = d2.end_demand_id, neededBy = DATEADD(d, - d.parentJobOrderLeadTime, d2.neededBy)  " & _
                    "FROM dbo.mrp_demand d  INNER JOIN dbo.mrp_demand d2  ON d.ORGANIZATION_ID = d2.ORGANIZATION_ID AND d.PARENT_JOB_ORDER_ID = d2.JOB_ID " & _
                    "WHERE d.LEVEL_NO IS NULL AND d2.LEVEL_NO = 4"
            dbS.NonQuery(sSQL)

            dbS.NonQuery("UPDATE d SET neededBy = DATEADD(d,-2,neededBy) FROM mrp_demand d WHERE DATEPART(dw,neededBy) = 1")    'change Sunday to prior Friday duplicated
            dbS.NonQuery("UPDATE d SET neededBy = DATEADD(d,-1,neededBy) FROM mrp_demand d WHERE DATEPART(dw,neededBy) = 7")    'change Saturday to prior Friday duplicated
            createLoggingStamp("PrepareToCalculateShortages")

            createLoggingStamp("PrepareToCalculateShortages - change project ID")

        Catch ex As Exception
            createErrorLog("PrepareToCalculateShortages", ex)
        Finally
            dbS.Disconnect()
        End Try


    End Sub

    Private Sub CalculateShortages()
        Try

            Dim sTmp As String = ""
            Dim sSQL As String
            Dim sItemProj As String = ""
            Dim sItemProjPrev As String = ""
            Dim dblNetShort As Double
            Dim dblShortPrev As Double
            Dim dblSupply As Double
            Dim x As Integer
            Dim y As Integer
            Dim dblQtyOnHand As Double
            Dim dsS As New DataSet

            'Connect to the database
            dbS.Connect()
            createLoggingStamp("CalculateShortages")
            'Reset the variables
            dbS.NonQuery("UPDATE msc_supply SET end_demand_id = NULL, level_no = NULL, parent_sales_order_no = NULL, PARENT_SALES_ORDER_LINE = NULL, order_no = NULL;")


            'Post the end_demand_id into the msc_supply table
            sSQL = " UPDATE s SET s.end_demand_id = d.end_demand_id, level_no = 0, s.parent_sales_order_no = d.parent_sales_order_no, s.PARENT_SALES_ORDER_LINE = d.PARENT_SALES_ORDER_LINE, s.order_no = d.parent_sales_order_no FROM msc_demand d INNER JOIN msc_supply s ON d.demand_id = s.demand_id " & _
                    " AND d.inventory_item_id = s.inventory_item_id WHERE d.end_demand_id IS NOT NULL AND demand_type_desc = 'Sales order MDS'; "
            dbS.NonQuery(sSQL)

            sSQL = " UPDATE s SET s.end_demand_id = d.end_demand_id, level_no = 0, s.parent_sales_order_no = d.parent_sales_order_no, s.PARENT_SALES_ORDER_LINE = d.PARENT_SALES_ORDER_LINE, s.order_no = d.parent_sales_order_no FROM msc_demand d INNER JOIN msc_supply s ON d.demand_id = s.demand_id " & _
                    " AND d.inventory_item_id = s.inventory_item_id WHERE d.end_demand_id IS NOT NULL AND demand_type_desc = 'Sales Orders' AND s.end_demand_id IS NULL; "
            dbS.NonQuery(sSQL)

            'end_demand_id can be propagated where possible
            sSQL = "UPDATE s2 SET   s2.end_demand_id = s1.end_demand_id, s2.parent_sales_order_no = s1.parent_sales_order_no, s2.PARENT_SALES_ORDER_LINE = s1.PARENT_SALES_ORDER_LINE,  s2.order_no = s1.order_no FROM msc_supply s1 " & _
                    " INNER JOIN msc_supply s2 ON s1.pegging_id = s2.end_pegging_id WHERE s1.end_demand_id IS NOT NULL " & _
                    " AND s2.end_demand_id IS NULL;"
            dbS.NonQuery(sSQL)

            'From the propagation down the supply table we can put it back into the demand table

            'Here we are building the level_no logic down the pegging tree with pegging_id and previous pegging_id
            For i As Integer = 0 To 10
                dbS.NonQuery("UPDATE s2 SET  s2.level_no = s1.level_no + 1 FROM msc_supply s1 INNER JOIN msc_supply s2 ON s1.pegging_id = s2.prev_pegging_id WHERE s1.level_no = " & i.ToString & " AND s2.level_no IS NULL;")
            Next


            'now we can put it back into the demand table as it was propagated into the supply table
            dbS.NonQuery("UPDATE d SET d.end_demand_id = s.end_demand_id, d.level_no = s.level_no FROM msc_supply s INNER JOIN msc_demand d ON d.demand_id = s.demand_id AND d.inventory_item_id = s.inventory_item_id WHERE s.end_demand_id IS NOT NULL AND d.end_demand_id IS NULL;")


            '*** Simply map down from the pegging tree and identify as short as whatever is listed from the list of supply types
            '            Planned(order)                     5
            '            PO in receiving                    8
            '            Purchase(order)                    1
            '            Intransit(shipment)                11
            '            Purchase(requisition)              2
            '            Work(order)                        3
            '            Work order co-product/by-product   14
            'We assume supply type "On Hand -> 18" is the only one which indicates we are not short

            'For the first run we will just flag what is short in supply
            sSQL = "UPDATE msc_supply SET short = 1 WHERE supply_type IN (1,2,3,5,8,11,14)"
            dbS.NonQuery(sSQL)

            'Post this info to demand table as well
            sSQL = "UPDATE a SET a.short = b.short FROM msc_demand a INNER JOIN msc_supply b ON a.demand_id = b.demand_id AND a.organization_id = b.organization_id AND a.inventory_item_id = b.inventory_item_id;"
            dbS.NonQuery(sSQL)

            'Short really represents the quantity we are short
            'this we need to calculate
            'open the supply table sorted by new_schedule_date and -> best would be to download without the pegging
            'as including the pegging it will be calculation work what we really want to avoid.
            'so we will have short column in msc_supply table
            'the first record on item level will be propagated to demand table and then the rest will be done from demand table
            'like on project demand it will be always the demand quantity
            'and all other will be acumulated!

            '1. for the project shortages we can set short to the quantity itself - This has to gather the real shortages from ASCP as well
            sSQL = "UPDATE d SET d.short = d.mrp_qty FROM msc_demand d WHERE d.short = 1 and project_id IS NOT NULL AND d.short <> d.mrp_qty;"
            'dbS.NonQuery(sSQL)

            dbS.NonQuery("UPDATE msc_demand SET temp = NULL WHERE temp is not null")
            dbS.NonQuery("TRUNCATE TABLE tblWorkASCP")
            dbS.NonQuery("TRUNCATE TABLE tblWork2ASCP")

            dbS.NonQuery("INSERT INTO tblWork2ASCP (number1, number2, number3, number4) (SELECT demand_id, project_id, organization_id, inventory_item_id FROM msc_demand WHERe project_id IS NOT NULL )")
            dbS.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3, number4, number5)( " & _
                            " SELECT isnull(SUM( s.allocated_quantity),0), s.project_id, s.demand_id, s.inventory_item_id, s.organization_id " & _
                            " FROM msc_supply s INNER JOIN tblWork2ASCP t2 ON s.inventory_item_id = t2.number4 AND s.organization_id = t2.number3 AND s.demand_id = t2.number1  WHERE  s.supply_type IN (1,2,3,5,8,11,14) " & _
                            " GROUP BY s.project_id, s.demand_id, s.inventory_item_id, s.organization_id)")
            dbS.NonQuery("UPDATE d SET short = t.number1, temp = 'Not me anymore' FROm msc_demand d INNER JOIN tblWorkASCP t ON d.inventory_item_id = t.number4 AND d.organization_id = t.number5 AND d.demand_id = t.number3")

            'Propagate the demand_priority through the pegging
            db.NonQuery(" UPDATE a SET a.demand_priority = b.demand_priority FROM msc_demand a INNER JOIN msc_demand b ON a.end_demand_id = b.demand_id WHERE a.demand_priority IS NULL ")

            'CALCULATE QTY SHORT for each item-project
            'Open MRP demand table sorted by org, item, project and consumption priority 
            sSQL = "SELECT  d.demand_id, d.inventory_item_id, d.mrp_qty, d.mrp_date, d.short, d.project_id, d.qty_onhand, d.organization_id " & _
                        ",(SELECT isnull(SUM( s.allocated_quantity),0) FROM msc_supply s WHERE d.demand_id = s.demand_id AND s.supply_type IN (1,2,3,5,8,11,14)) fromShort " & _
                        ",(SELECT isnull(SUM( s.allocated_quantity),0) FROM msc_supply s WHERE d.demand_id = s.demand_id AND s.supply_type IN (18)) fromOnHand  " & _
                        "FROM msc_demand d  " & _
                        "            WHERE d.project_id Is NULL And d.short Is Not null  AND d.temp IS NULL " & _
                        "ORDER BY d.inventory_item_id,d.demand_priority, d.mrp_date, d.demand_id ;"
            ' AND d.item_no = 'BY55746AA-304' Use this to test specific AND demand_id =25643203
            'List for end_assembly_pegging_flag

            'I Hard Pegging
            'y End Assembly Pegging
            'A Soft Pegging
            'B End Assembly / Soft Pegging
            'x End Assembly / Hard Pegging
            'N None

            Dim conn As New SqlClient.SqlConnection(My.Settings("FLOWConnectionString"))
            Dim adp As New SqlClient.SqlDataAdapter(sSQL, conn)
            Dim cb As New SqlClient.SqlCommandBuilder(adp)
            Dim dt As New DataTable
            adp.Fill(dt)

            'cycle through each demand, and calculate if the part has any net shortages
            If dt.Rows.Count > 0 Then
                For Each Row As DataRow In dt.Rows
                    sItemProj = Row("ORGANIZATION_ID").ToString & Row("INVENTORY_ITEM_ID").ToString & Row("PROJECT_ID").ToString
                    'Here we have to decide between hard pegged and not hard pegged material
                    'The soft pegged stuff will have to go through like on item level

                    If sItemProj <> sItemProjPrev Then 'If new item number, reset variables
                        sItemProjPrev = sItemProj
                        dblQtyOnHand = 0
                    End If
                    dblQtyOnHand += Row("fromOnHand")
                    dblQtyOnHand -= Row("MRP_QTY")  'subtract MRP needed qty from qty on hand and add what mrp want to supply additionally
                    'if we are short, update the MRP Demand table
                    If dblQtyOnHand < 0 Then
                        Row("Short") = -dblQtyOnHand
                    End If
                Next
                adp.Update(dt) ' Send the changes made in the datatable back to the database.  this will not work unless the table has a primary key defined.  i used demand_id for MRP Demand
            End If
            adp.Dispose()
            dt.Dispose()

            createLoggingStamp("CalculateShortages")

        Catch ex As Exception
            createErrorLog("CalculateShortages", ex)
        Finally
            dbS.Disconnect()
        End Try
    End Sub


    Private Sub buildSupplyTypeDEscription()
        Try
            Dim SSQL As String
            Dim conn As New SqlClient.SqlConnection(My.Settings("FLOWConnectionString"))
            'WHERE demand_id =25643203
            SSQL = "SELECT demand_id, supplyDescription FROM msc_demand  ORDER BY demand_id ASC"
            'make and adapter and another table to write this together 'make sure all sorts of supply are shown
            'AND d.demand_id =25643203
            Dim dsSupply As DataSet = db.Query("SELECT d.demand_id, s.supply_type_desc, s.order_number, s.po_line_id, s.po_line_location_id, s.purchase_order_id, s.job_id, s.allocated_quantity, s.purchase_order_no, s.purch_line_no, s.RELEASE_NO, s.SHIPMENT_NO, s.supply_type FROM msc_demand d INNER JOIN msc_supply s ON d.organization_id = s.organization_id AND d.demand_id = s.demand_id  ORDER by d.demand_id ASC;")

            '  Supply_type_description | " " | order_number |: | allocated quantity | " "
            'link each mrp shortage to a replenishment, start by opening the demand table selecting net shortages only for this level
            Dim adpmrpdemand As New SqlClient.SqlDataAdapter(SSQL, conn)
            Dim cbmrpdemand As New SqlClient.SqlCommandBuilder(adpmrpdemand)
            Dim dtmrpdemand As New DataTable
            adpmrpdemand.Fill(dtmrpdemand)

            'cycle through each net shortage
            If dtmrpdemand.Rows.Count > 0 Then
                Dim startInteger As Integer = 0
                For Each rowmrpdemand As DataRow In dtmrpdemand.Rows
                    Dim strSupplyComment As String = ""
                    Dim bFirstHit As Boolean = False
                    For i As Integer = startInteger To dsSupply.Tables(0).Rows.Count - 1

                        If dsSupply.Tables(0).Rows(i).Item("demand_id") = rowmrpdemand.Item("demand_id") Then
                            Dim strOrderNumber As String = ""

                            If dsSupply.Tables(0).Rows(i).Item("supply_type") = "8" _
                            Or dsSupply.Tables(0).Rows(i).Item("supply_type") = "1" _
                            Or dsSupply.Tables(0).Rows(i).Item("supply_type") = "11" Then
                                strOrderNumber = dsSupply.Tables(0).Rows(i).Item("purchase_order_no").ToString + "_" + dsSupply.Tables(0).Rows(i).Item("purch_line_no").ToString + "(R " + dsSupply.Tables(0).Rows(i).Item("RELEASE_NO").ToString + ")" + "(S " + dsSupply.Tables(0).Rows(i).Item("SHIPMENT_NO").ToString + ")"
                            Else
                                strOrderNumber = dsSupply.Tables(0).Rows(i).Item("order_number").ToString
                            End If

                            'We may need really a first hit
                            strSupplyComment += dsSupply.Tables(0).Rows(i).Item("supply_type_desc").ToString + " " + strOrderNumber + ": " + dsSupply.Tables(0).Rows(i).Item("allocated_quantity").ToString + " | "
                            startInteger += 1
                            bFirstHit = True
                        Else

                            Exit For

                        End If

                    Next

                    rowmrpdemand.Item("supplyDescription") = strSupplyComment
                Next
            End If
            createLoggingStamp("buildSupplyTypeDEscription")
            adpmrpdemand.Update(dtmrpdemand)     'update the mrp demand table
            createLoggingStamp("buildSupplyTypeDEscription")

        Catch ex As Exception
            createErrorLog("CalculateShortages", ex)
        Finally
            'dbS.Disconnect()

        End Try
    End Sub


    Private Sub EnrichOrderDataMSC()
        Try

            Dim x As Integer
            Dim lngTmp As Long
            Dim sSQL As String
            Dim sTmp As String = ""
            Dim sTmp2 As String = ""
            Dim sPrevJobNo As String = ""
            Dim dtPrevNeededBy As Date
            Dim dtPrevMRPDate As Date
            Dim lngPrevOrg As Long = 0
            Dim lngPrevItem As Long = 0
            Dim lngPrevSO As Long
            Dim lngPrevLine As Long
            Dim lngPrevStatus As Long
            Dim lngPrevDemand As Long
            Dim dsS As New DataSet
            Dim conn As New SqlClient.SqlConnection(My.Settings("FLOWConnectionString"))

            'Keep connection to the database
            dbS.Connect()
            createLoggingStamp("EnrichOrderDataMSC 1 ")

            'fill in additional PO fields on MSC Supply table 
            sSQL = "UPDATE d SET Source_Vendor_ID = p.Vendor_ID, Vendor_Name = p.Vendor_Name, Purchase_Order_No = p.PO_NO, Qty_Open = (p.Quantity_Ordered - p.Qty_Received), PO_Promised_Date = p.Promised_Date " & _
              "FROM dbo.msc_supply AS d  INNER JOIN dbo.PO AS p  " & _
              "ON p.SHIP_TO_ORGANIZATION_ID = d.ORGANIZATION_ID AND p.PO_HEADER_ID = d.PURCHASE_ORDER_ID And p.LINE_NO = d.PURCH_LINE_NO "              '***po need to join also on release number and shipment number
            dbS.NonQuery(sSQL)

            'this will leave some records for PO in receiving as they seem to only have po line id as identifier


            dbS.NonQuery("TRUNCATE TABLE tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Text1, Number7, Date2, Text2, Number1, Number3) SELECT job_no, start_quantity_hdr, start_date, meaning, organization_id, wip_entity_id  FROM jobs GROUP BY job_no, start_quantity_hdr, start_date, meaning, organization_id, wip_entity_id ")

            'fill in additional Job fields on MSC Supply table  
            sSQL = "UPDATE d SET JOB_NO = w.Text1, START_QUANTITY_HDR = w.Number7, START_DATE = w.Date2, STATUS =w.Text2, jobDuration = w.Number8  " & _
             "FROM dbo.msc_supply d  INNER JOIN dbo.tblWorkASCP w  " & _
             "ON d.ORGANIZATION_ID = w.Number1 AND d.JOB_ID = w.Number3  "
            dbS.NonQuery(sSQL)


            'reset all values that are developed within this procedure (only needed for testing)
            dbS.NonQuery("UPDATE jobs SET quantity_open = NULL, first_open_seq = NULL, hours_open = NULL, operationSequenceIndicator = NULL, end_demand_id = NULL, mrp_date = NULL, neededBy = NULL, sales_order_no = NULL, sales_line_no = NULL, lowerLevelShortages = NULL, party_name = NULL, parentJobNo = NULL, parentJobStartDate = NULL, parentJobStatus = NULL, Buyer = NULL, no_of_purchased_shortages =  NULL")
            dbS.NonQuery("UPDATE jobs SET meaning = 'Unreleased'  WHERE status_type = 1") 'reset status message.  need to put this in a messages table***
            dbS.NonQuery("UPDATE jobs SET meaning = 'Released'  WHERE status_type = 3") 'reset status messages
            dbS.NonQuery("UPDATE po SET age = NULL, end_demand_id = NULL, mrp_qty = NULL, mrp_date = NULL, sales_order_no = NULL, sales_line_no = NULL,  qtyAvailable = NULL, fixed_lead_time = NULL, safety_stock_quantity = NULL, topLevelJobNo = NULL, topLevelJobStartDate =  NULL, topLevelJobStatus =  NULL, party_name =  NULL, no_of_purchased_shortages =  NULL, topLevelPlannerCode = NULL")
            dbS.NonQuery("UPDATE msc_demand SET op1 = NULL, op2 = NULL, op3 = NULL, op4 = NULL, op5 = NULL ")
            dbS.NonQuery("UPDATE sales_orders SET job_no = NULL, short = NULL, po_no = NULL, po_line = NULL, job_po_status = NULL, op1 = NULL, op2 = NULL, op3 = NULL, op4 = NULL, job_po_mrp_date = NULL, job_start_date = NULL, vendor_id = NULL, neededBy = NULL, jobDuration = NULL, checkForPartialShipment = NULL, lowest_level_shortage = NULL, no_of_processes = NULL, planner_code = NULL, holds_header = NULL, holds_line = NULL, Replenishment_Details = NULL")

            'Clean up and enhance the jobs table, with info we can get from jobs table itself
            dbS.NonQuery("DELETE FROM jobs WHERE resource_code LIKE 'QUEUE'") 'get rid of queue operations
            dbS.NonQuery("DELETE FROM jobs WHERE basis_type = 2") 'get rid of setups
            dbS.NonQuery("UPDATE jobs SET quantity_open = scheduled_quantity - quantity_scrapped - quantity_completed") 'calculate qty open for each operation
            dbS.NonQuery("DELETE FROM tblWorkASCP")  'mark the first open operation seq for each job
            dbS.NonQuery("INSERT INTO tblWorkASCP(Number1, Number2, Number3)  SELECT organization_id, wip_entity_id, MIN(operation_seq_no) as OpSeq  FROM jobs WHERE quantity_open > 0 GROUP BY organization_id, wip_entity_id ")
            dbS.NonQuery("UPDATE j SET first_open_seq = w.Number3 FROM jobs j  INNER JOIN tblWorkASCP w  ON j.organization_id = w.Number1 AND j.wip_entity_id = w.Number2")
            dbS.NonQuery("UPDATE jobs SET hours_open = ROUND(quantity_open * usage_rate_or_amount,1) WHERE uom_code LIKE 'HR'")

            'add minutes calculation here! ***

            'post parentJobInformation into supply and demand
            dbS.NonQuery("UPDATE a SET a.parentJobOrderNo = b.job_no FROM msc_supply a INNER JOIN  jobs b ON a.organization_id = b.organization_id AND a.parent_job_order_id = b.wip_entity_id ")
            dbS.NonQuery("UPDATE d SET d.parentJobOrderNo = s.parentJobOrderNo FROM msc_supply s INNER JOIN msc_demand d ON d.demand_id = s.demand_id AND d.inventory_item_id = s.inventory_item_id ")

            'delete redundant operations from the jobs table.  if there is more than one record for an op, save the one that has the lowest resource sequence number   *** this may not be scalable logic.  check if other sites need the info in the other resources (setup times, run times, etc)
            sSQL = "UPDATE j SET job_no = 'PLEASE_DELETE_ME'  FROM jobs j INNER JOIN jobs j1 ON j.ORGANIZATION_ID = j1.ORGANIZATION_ID " & _
             "AND j.WIP_ENTITY_ID = j1.WIP_ENTITY_ID AND j.OPERATION_SEQ_NO = j1.OPERATION_SEQ_NO " & _
             "WHERE j.RESOURCE_SEQ_NO > j1.RESOURCE_SEQ_NO"
            dbS.NonQuery(sSQL)
            dbS.NonQuery("DELETE FROM jobs WHERE job_no = 'PLEASE_DELETE_ME'")
            'mark the last open operation seq for each job
            dbS.NonQuery("DELETE FROM tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP(Number1, Number2, Number3)  SELECT organization_id, wip_entity_id, MAX(operation_seq_no) as OpSeq  FROM jobs WHERE quantity_open > 0 GROUP BY organization_id, wip_entity_id ")
            dbS.NonQuery("UPDATE j SET operationSequenceIndicator = 'Last Sequence' FROM jobs j  INNER JOIN tblWorkASCP w  ON j.organization_id = w.Number1 AND j.wip_entity_id = w.Number2 AND j.operation_seq_no = w.Number3 ")  '*** put this in a messages table!

            'Enhance the PO table, with info we can get from PO table itself
            dbS.NonQuery("UPDATE po SET age = CONVERT(FLOAT,(getDate() - approved_date))")
            'dbS.NonQuery("UPDATE po SET release_no = 0 WHERE release_no IS NULL")          '**** We want the same look and feel in Oracle and Autobahn
            sSQL = "UPDATE p SET alternateContact = p1.alternateContact, vend_contact = p1.vend_contact, email_address = p1.email_address " & _
             "FROM PO p  INNER JOIN PO p1  ON p.ORGANIZATION_ID = p1.ORGANIZATION_ID AND p.VENDOR_ID = p1.VENDOR_ID " & _
             "WHERE  p.alternateContact IS NULL AND p1.alternateContact IS NOT NULL " 'some PO's do not have a site code, so we have to copy the supplier site info from PO's that do
            dbS.NonQuery(sSQL)
            dbS.NonQuery("UPDATE po SET alternateContact = buyer WHERE alternateContact IS NULL")

            'Enhance the Sales Orders table, with info we can get from the Sales Orders table itself
            dbS.NonQuery("UPDATE sales_orders SET extended_price = ordered_quantity * unit_selling_price")

            'Enhance the MRP Demand table with info we can get from MRP Demand table itself
            dbS.NonQuery("UPDATE msc_supply SET op1 = vendor_name WHERE vendor_name IS NOT NULL") 'for display on Sales order grid and shortage grid
            dbS.NonQuery(" UPDATE msc_supply SET mrp_qty = allocated_quantity ")

            'Make buy into the supply table
            dbS.NonQuery("UPDATE S SET s.make_buy = ai.planning_make_buy_code FROM msc_supply s INNER JOIN active_items ai on s.inventory_item_id = ai.inventory_item_id AND s.organization_id = ai.organization_id ")
            dbS.NonQuery("UPDATE msc_supply SET make_buy = 'M' where make_buy = '1' ")
            dbS.NonQuery("UPDATE msc_supply SET make_buy = 'B' where make_buy = '2' ")
            'Has to be done for demand as well
            dbS.NonQuery("UPDATE S SET s.make_buy = ai.planning_make_buy_code FROM msc_demand s INNER JOIN active_items ai on s.inventory_item_id = ai.inventory_item_id AND s.organization_id = ai.organization_id ")
            dbS.NonQuery("UPDATE msc_demand SET make_buy = 'M' where make_buy = '1' ")
            dbS.NonQuery("UPDATE msc_demand SET make_buy = 'B' where make_buy = '2' ")

            'put the next open job operations into the MRP Demand table
            Dim adp As New SqlClient.SqlDataAdapter("SELECT * FROM msc_supply  WHERE job_id IS NOT NULL  ORDER BY organization_id, job_id ", conn)
            Dim cb As New SqlClient.SqlCommandBuilder(adp)
            Dim dt As New DataTable
            adp.Fill(dt)
            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    Try


                        'grab the resource names of the open operations for this job
                        sSQL = "SELECT resource_code from jobs WHERE organization_id = " & row("organization_id") & " AND wip_entity_id = " & row("job_id") & "  AND quantity_open > 0  AND resource_code IS NOT NULL  ORDER BY operation_seq_no "
                        dsS = dbS.Query(sSQL)
                        lngTmp = dsS.Tables(0).Rows.Count
                        If lngTmp > 5 Then lngTmp = 5
                        If lngTmp > 0 Then
                            For x = 1 To lngTmp
                                row("OP" & x) = dsS.Tables(0).Rows(x - 1).Item("resource_code") 'put the next four open operation's resource (work center) names in the msc supply Table
                            Next x
                        End If
                    Catch ex As Exception

                    End Try
                Next
                adp.Update(dt)   'update the table
            End If
            dt.Dispose()
            adp.Dispose()
            createLoggingStamp("EnrichOrderDataMSC 2")

            'Update order_no and line_no in the sales order line table
            dbS.NonQuery("UPDATE s SET  s.order_no = so.order_no, s.line_no = so.line_no FROM msc_supply s INNER JOIN sales_orders so on s.end_demand_id = so.demand_id AND s.organization_id = so.organization_id WHERE end_demand_id IS NOT NULL;")


            'put the sales order demand pegging and lower level shortage info into the Jobs table  
            dbS.NonQuery("UPDATE msc_supply SET temp = NULL")
            dbS.NonQuery("DELETE FROM tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Number1, Number2, Number3, Date1)  SELECT organization_id, job_id, MIN(short), Min(NEW_SCHEDULE_DATE) FROM msc_supply WHERE  job_id IS NOT NULL  GROUP BY organization_id, job_id ") 'put earliest demand into work table
            dbS.NonQuery("UPDATE d SET temp = 'first'  FROM msc_supply d  INNER JOIN tblWorkASCP w  ON d.organization_id = w.Number1 AND d.job_id = w.Number2  AND d.short = w.Number3 ") 'mark the earliest demand in the MRP table
            dbS.NonQuery("UPDATE j SET end_demand_id = d.end_demand_id  FROM jobs j  INNER JOIN msc_supply d  ON  j.organization_id = d.organization_id AND j.wip_entity_id = d.job_id  WHERE d.temp = 'first'") 'place the end demand id into the jobs
            dbS.NonQuery("UPDATE j SET mrp_date = w.Date1, neededBy = Date1  FROM jobs j  INNER JOIN tblWorkASCP w  ON j.organization_id = w.Number1 AND j.wip_entity_id = w.Number2 ")  'put need date of first sales order into job
            dbS.NonQuery("UPDATE j SET sales_order_no = d.parent_sales_order_no, sales_line_no = d.parent_sales_order_line  FROM jobs j  INNER JOIN msc_supply d  ON j.organization_id = d.organization_id AND j.end_demand_id = d.demand_id ") 'put first sales order needed into job
            dbS.NonQuery("UPDATE j SET lowerLevelShortages = 1 FROM jobs j  INNER JOIN msc_demand d  ON j.organization_id = d.organization_id AND j.wip_entity_id = d.parent_job_order_id  WHERE d.short IS NOT NULL ") 'flag each job that has a lower level shortage 

            'Update the jobs table to Unreleased Partial Available
            '*** Needed By date forward to all purchase orders and discrete jobs


            'select all non-project jobs and determine if any should be combined, and copy the pegging info to the earlier jobs that didn't get it, because their supply was insufficient to meet the actual demand
            sSQL = "SELECT j.ORGANIZATION_ID, j.INVENTORY_ITEM_ID, j.WIP_ENTITY_ID, j.JOB_NO, j.STATUS_TYPE, j.MEANING, d.SHORT, j.START_DATE, j.neededBy, j.MRP_DATE, j.END_DEMAND_ID, j.SALES_ORDER_NO, j.SALES_LINE_NO " & _
             "FROM jobs j LEFT OUTER JOIN msc_demand d ON j.ORGANIZATION_ID = d.ORGANIZATION_ID AND j.WIP_ENTITY_ID = d.JOB_ID " & _
             "WHERE j.operationSequenceIndicator IS NOT NULL AND j.PROJECT_ID IS NULL " & _
             "ORDER BY j.ORGANIZATION_ID, j.INVENTORY_ITEM_ID, j.START_DATE DESC, d.SHORT DESC "
            dsS = dbS.Query(sSQL)
            lngTmp = dsS.Tables(0).Rows.Count
            If lngTmp > 0 Then
                For x = 0 To lngTmp - 1
                    'if this job doesn't tie to a sales order, tie it to previous sales order, provided it is for the same org / item and scheduled earlier
                    If DBNull.Value.Equals(dsS.Tables(0).Rows(x).Item("sales_order_no")) Then
                        Try
                            If dsS.Tables(0).Rows(x).Item("organization_id") = lngPrevOrg And dsS.Tables(0).Rows(x).Item("inventory_item_id") = lngPrevItem Then

                                sSQL = "UPDATE j SET neededBy = '" & dtPrevNeededBy & "', sales_order_no = " & lngPrevSO & ", sales_line_no = " & lngPrevLine & ", mrp_date = '" & dtPrevMRPDate & "', end_demand_id = " & lngPrevDemand
                                sSQL += ", party_name = 'how now brown cow'"

                                If lngPrevStatus = 1 And dsS.Tables(0).Rows(x).Item("status_type") = 1 Then 'if both jobs are unreleasd, tell the user to combine them
                                    sTmp = dsS.Tables(0).Rows(x).Item("meaning").ToString
                                    sSQL += ", meaning = '" & sTmp & " combine with " & sPrevJobNo & "'"   'put this phrase in messages table
                                End If
                                sSQL += " FROM jobs j  WHERE organization_id = " & lngPrevOrg & " AND wip_entity_id = " & dsS.Tables(0).Rows(x).Item("wip_entity_id")
                                dbS.NonQuery(sSQL)

                            End If
                        Catch

                        End Try
                    Else
                        Try
                            'remember the pegging info for the current record       'Crashes for some reason
                            sPrevJobNo = dsS.Tables(0).Rows(x).Item("job_no").ToString
                            lngPrevStatus = dsS.Tables(0).Rows(x).Item("status_type")
                            dtPrevMRPDate = dsS.Tables(0).Rows(x).Item("mrp_date")
                            dtPrevNeededBy = dsS.Tables(0).Rows(x).Item("neededBy")
                            lngPrevDemand = dsS.Tables(0).Rows(x).Item("end_demand_id")
                            lngPrevOrg = dsS.Tables(0).Rows(x).Item("organization_id")
                            lngPrevItem = dsS.Tables(0).Rows(x).Item("inventory_item_id")
                            lngPrevSO = dsS.Tables(0).Rows(x).Item("sales_order_no")
                            lngPrevLine = dsS.Tables(0).Rows(x).Item("sales_line_no")
                        Catch ex As Exception

                        End Try
                    End If
                Next
            End If

            'copy the parent job start date info into each job
            dbS.NonQuery("UPDATE j SET parentJobNo = d.job_no, parentJobStartDate = DATEADD(dd,CASE WHEN DATENAME(dw, d.start_date) like 'Monday' THEN -3 ELSE -1 END,d.start_date), parentJobStatus = d.status  FROM jobs j  INNER JOIN msc_demand d  ON j.organization_id = d.organization_id AND j.end_demand_id = d.demand_id  WHERE j.wip_entity_id <> d.job_id ") 'put parent job details into the job
            dbS.NonQuery("UPDATE j SET FSG_SALES_TYPE = s.FSG_SALES_TYPE  FROM jobs j  INNER JOIN sales_orders s  ON j.organization_id = s.organization_id AND j.end_demand_id = s.demand_id AND j.parentJobNo = s.job_no  WHERE j.parentJobStartDate IS NOT NULL") 'if this job has a parent job linked to a sales order, change the sales type to the parent sales type
            dbS.NonQuery("UPDATE jobs SET parentJobStartDate = (getdate() + 720) WHERE parentJobStatus LIKE 'ON HOLD'") 'delay job if parent job is on hold   *** Put status in messaging table
            dbS.NonQuery("UPDATE jobs SET parentJobStartDate = neededBy WHERE parentJobStartDate IS NULL AND neededBy IS NOT NULL AND status_type IN (1,3)") 'copy balance of need by dates into the shop start date
            dbS.NonQuery("UPDATE jobs SET parentJobStartDate = (getdate() + 720) WHERE status_type = 6")    'delay jobs that are on hold

            'Populate the parent job order id
            dbS.NonQuery(" UPDATE b SET b.parent_job_order_id = a.job_id FROM msc_supply a INNER JOIN msc_supply b on a.pegging_id = b.prev_pegging_id and a.organization_id = b.organization_id where a.job_id is not null; ")

            'if the job has no lower level demand, then MRP wants to cancel it or there is no bom. mark each job accordingly.  *** adjust this for safety stock requirements
            dbS.NonQuery("UPDATE jobs SET temp = NULL, lowerLevelShortages = null")
            dbS.NonQuery("UPDATE j SET lowerLevelShortages = 1 FROM jobs j  INNER JOIN msc_supply d  ON j.organization_id = d.organization_id AND j.wip_entity_id = d.parent_job_order_id  WHERE d.short IS NOT NULL")
            dbS.NonQuery("UPDATE j SET temp = 'has lower level demand' FROM jobs j INNER JOIN msc_supply d  ON j.organization_id = d.organization_id AND j.wip_entity_id = d.parent_job_order_id")
            dbS.NonQuery("UPDATE jobs SET meaning = 'Unreleased Available'  WHERE lowerLevelShortages IS NULL AND meaning LIKE 'Unreleased'")    'change the status from un releasd to un releasd available if there are no lower level shortages  *** put these phrases in a messages table
            dbS.NonQuery("UPDATE j SET meaning = 'unrel NO BOM' FROM jobs j  FULL OUTER JOIN open_boms b  ON j.wip_entity_id = b.wip_entity_id AND j.organization_id = b.organization_id  WHERE b.wip_entity_id IS NULL AND (j.meaning LIKE 'Unreleased Available' OR j.meaning LIKE 'Released%')")
            dbS.NonQuery("UPDATE j SET meaning = 'unrel CANCEL/RESCHED'  FROM jobs j  WHERE j.temp IS NULL AND j.end_demand_id IS NULL AND j.meaning LIKE 'Unreleased Available'")  'we need to put these phrases in a messages table ***
            createLoggingStamp("EnrichOrderDataMSC")

            'mark jobs that are idle
            dbS.NonQuery("DELETE FROM tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Number1, Number2)  SELECT j.ORGANIZATION_ID, j.WIP_ENTITY_ID FROM jobs j  GROUP BY j.ORGANIZATION_ID, j.WIP_ENTITY_ID  HAVING (MAX(DATE_LAST_MOVED) < { fn NOW() } - 7)") '*** place this control limit in the org setup table
            dbS.NonQuery("UPDATE j  SET meaning = 'Released Idle' FROM jobs j  INNER JOIN tblWorkASCP w  ON j.organization_id = w.Number1 AND j.wip_entity_id = w.Number2  AND j.status_type = 3")

            'copy the updated job statuses into the MRP Demand table
            sSQL = "UPDATE d SET status = j.meaning  FROM msc_supply d  INNER JOIN jobs j ON d.ORGANIZATION_ID = j.ORGANIZATION_ID AND d.job_id = j.wip_entity_id  "
            dbS.NonQuery(sSQL)

            ''Make sure the null becomes a 0 so the needed by is correct in the purchase order table '**** Changed to not do this step
            'dbS.NonQuery("UPDATE msc_supply SET release_no = 0 WHERE release_no IS NULL AND purchase_order_no IS NOT NULL")
            'dbS.NonQuery("UPDATE msc_supply SET shipment_no = 0 WHERE shipment_no IS NULL AND purchase_order_no IS NOT NULL")

            'Use line_location_id to populate shipment_no and release_no  in msc_supply table '**** This information is coming from the msc_supply table itself!
            'dbS.NonQuery("UPDATE s SET s.release_no = po.release_no, s.shipment_no = po.shipment_no FROM msc_supply s INNER JOIN po ON s.po_line_location_id = po.line_location_id AND s.organization_id = s.organization_id;")

            '*** Here we do need to apply the change for R12 in order to deal with the three ()()() and now release and shipment number is within the last two brackets

            Dim dsCheckBackground As DataSet = db.Query("SELECT MAX(len(order_number) - len(replace(order_number,')',''))) FROM msc_Supply")

            If Not DBNull.Value.Equals(dsCheckBackground.Tables(0).Rows(0).Item(0)) Then
                If dsCheckBackground.Tables(0).Rows(0).Item(0) = 3 Then
                    r12Instance = True
                End If
            End If
            '***R12 remediation - we now have three parenthesis
            If r12Instance Then
                '*** Need to check for R12
                Try
                    dbS.NonQuery("UPDATE msc_Supply SET SHIPMENT_NO = replace(LTRIM(RTRIM(replace(right(substring(order_number,(charindex(')',order_number) +2), (len(order_number) - (charindex(')',order_number) +2))),charindex(')',substring(order_number,(charindex(')',order_number) +2), (len(order_number) - (charindex(')',order_number) +2))))-1),'(','')) ),')','') WHERE (len(order_number) - len(replace(order_number, '(', ''))) = 3 AND supply_type_desc = 'Purchase Order'")
                    dbS.NonQuery("UPDATE msc_Supply SET RELEASE_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))) WHERE  (len(order_number) - len(replace(order_number, '(', ''))) = 3 AND supply_type_desc = 'Purchase Order'")
                Catch ex As Exception
                    createErrorLog("shipment and release number", ex)
                End Try

                '11 i query to do with old stuff
                'Try
                '    dbS.NonQuery("UPDATE msc_Supply SET SHIPMENT_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))) WHERE short is NOT null AND (len(order_number) - len(replace(order_number, '(', ''))) =1")
                '    dbS.NonQuery("UPDATE msc_Supply SET RELEASE_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))), SHIPMENT_NO = substring(order_number,(charindex(')',order_number) +2), (len(order_number) - (charindex(')',order_number) +2))) WHERE short is NOT null AND (len(order_number) - len(replace(order_number, '(', ''))) =2")

                'Catch ex As Exception

                'End Try
                
            Else
                'Do the string extraction in order to show the real values - no relase means it is just a shipment and we want to display it as a NULL value or empty field
                dbS.NonQuery("UPDATE msc_Supply SET SHIPMENT_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))) WHERE short is NOT null AND (len(order_number) - len(replace(order_number, '(', ''))) =1")
                dbS.NonQuery("UPDATE msc_Supply SET RELEASE_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))), SHIPMENT_NO = substring(order_number,(charindex(')',order_number) +2), (len(order_number) - (charindex(')',order_number) +2))) WHERE short is NOT null AND (len(order_number) - len(replace(order_number, '(', ''))) =2")

            End If
            
            'Forward this information to msc demand
            dbS.NonQuery("UPDATE d SET d.release_no = s.release_no, d.shipment_no = s.shipment_no FROM msc_demand d INNER JOIN msc_supply s ON d.organization_id = s.organization_id AND d.demand_id = s.demand_id AND s.release_no IS NOT NULL")

            'put the sales order demand pegging into the PO table   '*** DO we want to switch to using line_id instead of line_no, in order to distinguish blanket releases ?   '***po need to match on the release and shipment number also
            dbS.NonQuery("UPDATE msc_supply SET temp = NULL")
            dbS.NonQuery("TRUNCATE TABLE tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Number1, Number2, Number3, Date1, Number4, Number5, Date2)  SELECT organization_id, purchase_order_id, purch_line_no, MIN(new_schedule_date), RELEASE_NO, SHIPMENT_NO, MIN(NEW_DOCK_DATE) FROM msc_supply WHERE  purchase_order_id IS NOT NULL GROUP BY organization_id, purchase_order_id, purch_line_no, RELEASE_NO, SHIPMENT_NO ") 'put earliest demand into work table
            dbS.NonQuery("UPDATE d SET temp = 'first'  FROM msc_supply d  INNER JOIN tblWorkASCP w  ON d.organization_id = w.Number1 AND d.purchase_order_id = w.Number2  AND d.purch_line_no = w.Number3 AND d.new_schedule_date = w.Date1 ") 'mark the earliest demand in the MRP table
            dbS.NonQuery("UPDATE po SET end_demand_id = d.end_demand_id, mrp_qty = d.mrp_qty  FROM po  INNER JOIN msc_supply d  ON po.ship_to_organization_id = d.organization_id AND po.po_header_id = d.purchase_order_id AND po.line_no = d.purch_line_no  WHERE d.temp = 'first' ")  'place the end demand id into the PO's
            dbS.NonQuery("UPDATE po SET mrp_date = w.Date1, neededBy = Date1, NEW_DOCK_DATE = Date2  FROM po  INNER JOIN tblWorkASCP w  ON po.ship_to_organization_id = w.Number1 AND po.po_header_id = w.Number2 AND po.line_no = w.Number3 AND isnull(po.RELEASE_NO,0) = isnull(w.Number4,0) AND po.SHIPMENT_NO = w.Number5 ")     '***po put need date of first sales order into PO
            dbS.NonQuery("UPDATE po SET sales_order_no = d.parent_sales_order_no, sales_line_no = d.parent_sales_order_line  FROM po  INNER JOIN msc_supply d  ON po.ship_to_organization_id = d.organization_id AND po.end_demand_id = d.demand_id ")  'put first sales order needed into PO

            'LINK THE OUTSIDE PROCESS PO to the same MRP Demand, Sales Order, Job records as the corresponding OSP job - make sure we copy over release and shipment as well!
            sSQL = "UPDATE d SET Source_Vendor_ID = p.Vendor_ID, Vendor_Name = p.Vendor_Name, OP1 = p.Vendor_Name, Purchase_Order_No = p.PO_NO, Purch_Line_No = p.LINE_NO, Qty_Open = (p.Quantity_Ordered - p.Qty_Received), PO_Promised_Date = p.Promised_Date, release_no = p.release_no, shipment_no = p.shipment_no  " & _
             "FROM jobs j INNER JOIN PO p ON j.ospItem = p.ITEM_NO AND j.WIP_ENTITY_ID = p.WIP_ENTITY_ID AND j.ORGANIZATION_ID = p.SHIP_TO_ORGANIZATION_ID " & _
             "INNER JOIN msc_supply d ON j.WIP_ENTITY_ID = d.JOB_ID AND j.ORGANIZATION_ID = d.ORGANIZATION_ID "
            dbS.NonQuery(sSQL)
            'for OSP outside process po's (none have any MRP Demand), copy the sales order pegging info from the osp job - these are for # and s part numbers in chpk
            sSQL = "UPDATE p SET sales_order_no = j.sales_order_no, sales_line_no = j.sales_line_no, end_demand_id = j.end_demand_id, neededBy = j.neededBy, job_no = j.job_no, mrp_date = j.mrp_date " & _
             "FROM PO p  INNER JOIN jobs j  ON p.ship_to_organization_id = j.organization_id  AND  p.wip_entity_id = j.wip_entity_id  "
            db.NonQuery(sSQL)

            ''select all non-project PO's and determine if any should be combined, and copy the pegging info to the earlier PO's that didn't get it, because their supply was insufficient to meet the actual demand
            'sSQL = "SELECT p.SHIP_TO_ORGANIZATION_ID, p.INVENTORY_ITEM_ID, p.PO_HEADER_ID, p.LINE_NO, p.SHIPMENT_NO, d.SHORT, p.NEED_BY_DATE, p.neededBy, p.MRP_DATE, p.END_DEMAND_ID, p.SALES_ORDER_NO, p.SALES_LINE_NO " & _
            ' "FROM PO p LEFT OUTER JOIN msc_supply d ON p.SHIP_TO_ORGANIZATION_ID = d.ORGANIZATION_ID AND p.PO_HEADER_ID = d.PURCHASE_ORDER_ID  AND p.LINE_NO = d.PURCH_LINE_NO  " & _
            ' "WHERE p.PROJECT_ID IS NULL  AND p.INVENTORY_ITEM_ID IS NOT NULL ORDER BY p.SHIP_TO_ORGANIZATION_ID, p.INVENTORY_ITEM_ID, p.NEED_BY_DATE DESC, d.SHORT DESC "
            'dsS = dbS.Query(sSQL)
            'lngTmp = dsS.Tables(0).Rows.Count
            'If lngTmp > 0 Then
            '    For x = 0 To lngTmp - 1
            '        'if this PO doesn't tie to a sales order, tie it to previous sales order, provided it is for the same org / item and scheduled earlier
            '        If DBNull.Value.Equals(dsS.Tables(0).Rows(x).Item("sales_order_no")) Then
            '            Try


            '                If dsS.Tables(0).Rows(x).Item("ship_to_organization_id") = lngPrevOrg And dsS.Tables(0).Rows(x).Item("inventory_item_id") = lngPrevItem Then
            '                    sSQL = "UPDATE p SET neededBy = '" & dtPrevNeededBy & "', sales_order_no = " & lngPrevSO & ", sales_line_no = " & lngPrevLine & ", mrp_date = '" & dtPrevMRPDate & "', end_demand_id = " & lngPrevDemand
            '                    sSQL += " FROM PO p WHERE ship_to_organization_id = " & lngPrevOrg & " AND po_header_id = " & dsS.Tables(0).Rows(x).Item("po_header_id") & " AND line_no = " & dsS.Tables(0).Rows(x).Item("line_no")
            '                    dbS.NonQuery(sSQL)
            '                End If
            '            Catch ex As Exception

            '            End Try
            '        Else
            '            Try
            '                'remember the pegging info for the current record
            '                dtPrevMRPDate = dsS.Tables(0).Rows(x).Item("mrp_date")
            '                dtPrevNeededBy = dsS.Tables(0).Rows(x).Item("neededBy")
            '                lngPrevDemand = dsS.Tables(0).Rows(x).Item("end_demand_id")
            '                lngPrevOrg = dsS.Tables(0).Rows(x).Item("ship_to_organization_id")
            '                lngPrevItem = dsS.Tables(0).Rows(x).Item("inventory_item_id")
            '                lngPrevSO = dsS.Tables(0).Rows(x).Item("sales_order_no")
            '                lngPrevLine = dsS.Tables(0).Rows(x).Item("sales_line_no")
            '            Catch ex As Exception

            '            End Try
            '        End If
            '    Next
            'End If

            'copy the job and PO supply info into the Sales Order table 
            sSQL = "UPDATE s SET job_no = d.job_no, short = d.short, po_no = d.purchase_order_no, po_line = d.purch_line_no, job_po_status = d.status, op1 = d.op1, op2 = d.op2, op3 = d.op3, op4 = d.op4, job_po_mrp_date = d.consumptionPriorityDate, job_start_date = (SELECT min(start_date) FROM jobs WHERE jobs.job_no = d.job_no), vendor_id = d.source_vendor_id, neededBy = d.consumptionPriorityDate, jobDuration = d.jobDuration, manufacturing_date = (SELECT max(completion_date) FROM jobs WHERE jobs.job_no = d.job_no) , make_buy = d.make_buy   " & _
             "FROM sales_orders s  INNER JOIN msc_supply d  ON s.organization_id = d.organization_id AND s.demand_id = d.demand_id "
            dbS.NonQuery(sSQL)

            'additional update statement in order to show up job information for the partial shipped sales order lines
            dbS.NonQuery("UPDATE s SET job_no = d.job_no, short = d.short, po_no = d.purchase_order_no, po_line = d.purch_line_no, job_po_status = d.status, " & _
                         " op1 = d.op1, op2 = d.op2, op3 = d.op3, op4 = d.op4, job_po_mrp_date = d.consumptionPriorityDate, " & _
                         " job_start_date = (SELECT min(start_date) FROM jobs WHERE jobs.job_no = d.job_no), vendor_id = d.source_vendor_id, neededBy = d.consumptionPriorityDate, " & _
                         " jobDuration = d.jobDuration, manufacturing_date = (SELECT max(completion_date) FROM jobs WHERE jobs.job_no = d.job_no), " & _
                         "          make_buy = d.make_buy " & _
                         "           FROM sales_orders s  " & _
                         "			 INNER JOIN msc_supply d  ON s.organization_id = d.organization_id  " & _
                         "			 AND s.project_id   = d.project_id " & _
                         "			 AND s.order_no 	= d.order_no " & _
                         "			 AND s.inventory_item_id = d.inventory_item_id " & _
                         "			 WHERE s.job_no       IS NULL  AND d.job_no IS NOT NULL")


            'Update the job completion date
            dbS.NonQuery("TRUNCATE TABLE tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Number1, text1, Date1) SELECT organization_id, job_no, MAX(completion_date) from jobs GROUp BY organization_id, job_no")
            dbS.NonQuery("UPDATE a SET a.manufacturing_date =  b.date1 FROM [sales_orders] a INNER JOIN tblWorkASCP b ON a.job_no = b.text1 and a.organization_id = b.Number1 WHERE a.job_no IS NOT NULL")

            'copy the demand priority infor into the sales order table
            dbS.NonQuery(" UPDATE a SET  a.demand_priority = b.demand_priority FROm sales_orders a LEFT JOIN msc_demand b ON a.demand_id = b.demand_id ")

            'this updates make and buy information only when the sales order is in the mrp demand table
            dbS.NonQuery("UPDATE b SET b.make_buy = a.planning_make_buy_code FROM items a JOIN sales_orders b " & _
                        "ON a.organization_id = b.organization_id  " & _
                        "AND a.inventory_item_id = b.inventory_item_id " & _
                        "WHERE b.make_buy IS NULL ")
            dbS.NonQuery("UPDATE sales_orders SET make_buy = 'M' where make_buy = '1'")
            dbS.NonQuery("UPDATE sales_orders SET make_buy = 'B' where make_buy = '2'")

            'copy item description, planner code into the sales order table
            dbS.NonQuery("UPDATE s SET s.item_description = i.description FROM sales_orders s  INNER JOIN active_items i  ON s.organization_id = i.organization_id AND s.inventory_item_id = i.inventory_item_id  ") 'always get the description from the active items table
            dbS.NonQuery("UPDATE s SET planner_code = i.planner_code FROM sales_orders s  INNER JOIN active_items i  ON s.organization_id = i.organization_id AND s.inventory_item_id = i.inventory_item_id") 'copy item planner code into sales order

            'update sales order partial shipment indicator
            dbS.NonQuery("UPDATE sales_orders SET checkForPartialShipment = 1")
            dbS.NonQuery("UPDATE sales_orders SET checkForPartialShipment = 0 WHERE schedule_ship_date IS NULL OR (SCHEDULE_SHIP_DATE > { fn NOW() } + 365 * 2) ") 'if the order is not being planned by MRP, set the shippable flag to false
            dbS.NonQuery("UPDATE s SET checkForPartialShipment = 0  FROM sales_orders s  INNER JOIN msc_demand d  ON s.organization_id = d.organization_id AND s.demand_id = d.demand_id  WHERE d.short IS NOT NULL ") 'if it is MRP Planned and there are shortages, set to false

            'check for ship set constraint. if fails, set all lines to "No" - inv is not available to ship, place explanation at end of so line description.  *** need message in messaging table for this
            dbS.NonQuery("UPDATE so  SET description = substring(so.description,1,charindex('(ship set line',so.description)-3)  FROM sales_orders so WHERE charindex('(ship set line',so.description) > 0")
            dbS.NonQuery("DELETE FROM tblWorkASCP")
            sSQL = "INSERT INTO tblWorkASCP (Number1, Number2, Number3, Text1)  SELECT so.organization_id, so.header_id, so.line_id, so.description + '   (ship set line ' + rtrim(ltrim(str(so2.line_no))) + ' not avail)' " & _
              "FROM sales_orders so  INNER JOIN sales_orders so2  ON so.ORGANIZATION_ID = so2.ORGANIZATION_ID AND so.SHIP_SET_ID = so2.SHIP_SET_ID " & _
              "WHERE so.checkForPartialShipment = 1 And so2.checkForPartialShipment = 0 "
            dbS.NonQuery(sSQL)
            dbS.NonQuery("DELETE FROM tblWork2ASCP")
            dbS.NonQuery("INSERT INTO tblWork2ASCP (Number1, Number2, Number3, Text1) SELECT Number1, Number2, Number3, Max(Text1) as Expr1 FROM tblWorkASCP GROUP BY Number1, Number2, Number3") 'remove duplicates
            dbS.NonQuery("UPDATE s SET checkForPartialShipment = 0, description = w.Text1  FROM sales_orders s INNER JOIN tblWork2ASCP w  ON s.organization_id = w.Number1 AND s.header_id = w.Number2 AND s.line_id = w.Number3")

            'update sales order complete shipment indicator  (all lines have inventory available to ship)
            dbS.NonQuery("UPDATE sales_orders  SET orderCompleteForShipping = 1")
            dbS.NonQuery("UPDATE so2  SET orderCompleteForShipping = 0 FROM sales_orders so  INNER JOIN sales_orders so2 ON so.ORGANIZATION_ID = so2.ORGANIZATION_ID AND so.header_id = so2.header_id  WHERE so.checkForPartialShipment = 0")

            'calculate lowest level shortage for each sales order
            dbS.NonQuery("DELETE FROM tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Number1, Number2, Number3, Number4)  SELECT organization_id, end_demand_id, MAX(level_no), COUNT(pegging_id) FROM msc_supply  WHERE short IS NOT NULL AND level_no > 0 GROUP BY organization_id, end_demand_id ") 'put lowest level shortage into into work table
            dbS.NonQuery("UPDATE s SET lowest_level_shortage = w.Number3, no_of_processes = w.Number4  FROM sales_orders s  INNER JOIN tblWorkASCP w  ON s.organization_id = w.Number1 AND s.demand_id = w.Number2 ") 'copy into sales order table 

            'calculate number of purchased shortages for each sales order
            dbS.NonQuery("DELETE FROM tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Number1, Number2, Number3)  SELECT organization_id, end_demand_id, COUNT(pegging_id) FROM msc_supply  WHERE short IS NOT NULL AND level_no > 0 AND make_buy LIKE 'B' AND level_no > 0 GROUP BY organization_id, end_demand_id ") 'put number of purchased shortages in work table
            dbS.NonQuery("UPDATE s SET no_of_purchased_shortages = w.Number3 FROM sales_orders s  INNER JOIN tblWorkASCP w  ON s.organization_id = w.Number1 AND s.demand_id = w.Number2 ") 'update sales orders with number of purchased shortages 

            'Calculate the number of problems

            'copy the customer name and number of shortages from sales orders to Jobs and PO tables
            dbS.NonQuery("UPDATE j SET party_name = s.party_name, parentJobPlannerCode = s.planner_code, no_of_purchased_shortages = s.no_of_purchased_shortages  FROM jobs j  INNER JOIN sales_orders s  ON j.organization_id = s.organization_id AND j.end_demand_id = s.demand_id ")
            dbS.NonQuery("UPDATE po SET party_name = s.party_name  FROM po  INNER JOIN sales_orders s  ON po.organization_id = s.organization_id AND po.end_demand_id = s.demand_id ")

            'copy the job release date and qty open to the sales order
            sSQL = "UPDATE s SET job_release_date = j.date_released, job_po_quantity = j.start_quantity_hdr - j.quantity_completed_hdr - j.quantity_scrapped_hdr " & _
             "FROM sales_orders s  INNER JOIN jobs j  ON s.organization_id = j.organization_id AND s.job_no = j.job_no "
            dbS.NonQuery(sSQL)
            createLoggingStamp("EnrichOrderDataMSC")

            'Copy the top level job info into the PO table (top level job linked to same sales order as PO is linked)
            sSQL = "UPDATE p SET topLevelJobNo = s.job_no, topLevelJobStartDate = s.job_start_date, topLevelJobStatus = s.job_po_status, topLevelPlannerCode = s.PLANNER_CODE, party_name = s.party_name, no_of_purchased_shortages = s.no_of_purchased_shortages " & _
              "FROM po p  INNER JOIN sales_orders s  ON p.ship_to_organization_id = s.organization_id AND p.end_demand_id = s.demand_id "
            dbS.NonQuery(sSQL)

            'calculate qty available by subtracting all the demand that is required within item lead time from the qty on hand.  This is NOT done for project material (seiban)
            dbS.NonQuery("DELETE FROM tblWorkASCP")
            dbS.NonQuery("UPDATE items_qty_oh  SET qtyAvailable = NULL")
            dbS.NonQuery("UPDATE active_items   SET qtyAvailable = NULL")
            sSQL = "INSERT INTO tblWorkASCP (Number1, Number2, Number3)  SELECT q.organization_id, q.inventory_item_id, SUM(d.mrp_qty) AS qtyAllocated  FROM items_qty_oh q  " & _
             "INNER JOIN  active_items i  ON q.organization_id = i.organization_id AND q.inventory_item_id = i.inventory_item_id  " & _
             "INNER JOIN msc_demand d ON q.organization_id = d.organization_id AND q.inventory_item_id = d.inventory_item_id  " & _
             "WHERE q.project_id IS NULL AND d.project_id IS NULL AND d.consumptionPriorityDate <  { fn NOW() } + i.fixed_lead_time " & _
             "GROUP BY q.organization_id, q.inventory_item_id "
            dbS.NonQuery(sSQL) 'put qty allocated in work table
            dbS.NonQuery("UPDATE q SET qtyAvailable = q.qty_onhand - w.Number3  FROM items_qty_oh q  INNER JOIN tblWorkASCP w  ON q.organization_id = w.Number1 AND q.inventory_item_id = w.Number2  WHERE q.project_id IS NULL") 'calc qty available. the reason we post in the qty on hand table is that this table separates project from normal inventory
            dbS.NonQuery("UPDATE items_qty_oh SET qtyAvailable = 0 WHERE qtyAvailable < 0")
            dbS.NonQuery("UPDATE i SET qtyAvailable = q.qtyAvailable  FROM active_items i  INNER JOIN items_qty_oh q  ON  i.organization_id = q.organization_id AND i.inventory_item_id = q.inventory_item_id WHERE q.qtyAvailable IS NOT NULL")  'the active items table does not separate project inventory from normal inventory

            'now copy the Qty Available, fixed lead time, safety_stock_quantity into the jobs and PO tables
            dbS.NonQuery("UPDATE po SET qtyAvailable = i.qtyAvailable, safety_stock_quantity = i.safety_stock_quantity, fixed_lead_time = i.fixed_lead_time  FROM po  INNER JOIN active_items i  ON po.ship_to_organization_id = i.organization_id AND po.inventory_item_id = i.inventory_item_id  WHERE po.project_id IS NULL")
            dbS.NonQuery("UPDATE j   SET qtyAvailable = i.qtyAvailable, safety_stock_quantity = i.safety_stock_quantity, fixed_lead_time = i.fixed_lead_time  FROM jobs j  INNER JOIN active_items i  ON j.organization_id = i.organization_id AND j.inventory_item_id = i.inventory_item_id    WHERE j.project_id IS NULL")
            dbS.NonQuery("UPDATE j SET planner_code = i.planner_code,  j.buyer = i.buyer  FROM jobs j  INNER JOIN active_items i  ON j.organization_id = i.organization_id AND j.inventory_item_id = i.inventory_item_id")

            'Copy Qty Available, fixed lead time, safety_stock_quantity into the msc demand and supply table
            dbS.NonQuery("UPDATE d SET safety_stock_quantity = i.safety_stock_quantity FROM msc_demand d  INNER JOIN active_items i  ON d.organization_id = i.organization_id AND d.inventory_item_id = i.inventory_item_id  WHERE i.end_assembly_pegging_flag NOT IN ('X','I') ")
            dbS.NonQuery("UPDATE s   SET safety_stock_quantity = i.safety_stock_quantity FROM msc_supply s  INNER JOIN active_items i  ON s.organization_id = i.organization_id AND s.inventory_item_id = i.inventory_item_id    WHERE i.end_assembly_pegging_flag NOT IN ('X','I') ")


            'copy project name into sales orders, jobs and PO's, demand and supply   *** this needs an org id !
            dbS.NonQuery("UPDATE po SET project_name = p.project_name, project_number = p.project_number  FROM po  INNER JOIN pjmSeibanNumbers p  ON po.project_id = p.project_id")
            dbS.NonQuery("UPDATE s SET project_name = p.project_name, project_number = p.project_number  FROM sales_orders s INNER JOIN pjmSeibanNumbers p  ON s.project_id = p.project_id")
            dbS.NonQuery("UPDATE j SET project_name = p.project_name, project_number = p.project_number  FROM jobs j INNER JOIN pjmSeibanNumbers p  ON j.project_id = p.project_id")
            dbS.NonQuery("UPDATE d SET project_name = p.project_name  FROM msc_demand d  INNER JOIN pjmSeibanNumbers p  ON d.project_id = p.project_id")
            dbS.NonQuery("UPDATE d SET d.project_name = p.project_name FROM msc_demand d INNER JOIN pjmSeibanNumbers p ON d.project_id = p.project_id; ")
            dbS.NonQuery("UPDATE d SET d.project_name = p.project_name FROM msc_supply d INNER JOIN pjmSeibanNumbers p ON d.project_id = p.project_id; ")

            'Update sales order information for combined job and po numbers
            db.NonQuery("UPDATE sales_orders SET job_po = job_no WHERE job_no IS NOT NULL ")
            db.NonQuery("UPDATE sales_orders SET job_po  = (po_no + '_' + CONVERT(varchar,po_line)) WHERE po_no IS NOT NULL ")
            createLoggingStamp("EnrichOrderDataMSC")

            Try
                'COPY SALES ORDER HOLDS into sales orders table
                Dim adpTmp2 As New SqlClient.SqlDataAdapter("SELECT * FROM sales_orders ORDER BY operating_unit_id, header_id, line_id ", conn)
                Dim cbTmp2 As New SqlClient.SqlCommandBuilder(adpTmp2)
                Dim dtTmp2 As New DataTable
                adpTmp2.Fill(dtTmp2)
                If dtTmp2.Rows.Count > 0 Then
                    For Each rowTmp As DataRow In dtTmp2.Rows
                        'grab the header hold details for each sales order
                        sSQL = "SELECT * FROM sales_order_holds WHERE org_id = " & rowTmp("operating_unit_id") & " AND header_id = " & rowTmp("header_id") & " AND line_id IS NULL  ORDER BY last_update_date "
                        dsS = dbS.Query(sSQL)
                        If dsS.Tables(0).Rows.Count > 0 Then
                            sTmp = ""
                            For x = 1 To dsS.Tables(0).Rows.Count
                                sTmp += dsS.Tables(0).Rows(x - 1).Item("hold_name") & " (" & dsS.Tables(0).Rows(x - 1).Item("last_update_date").ToString & ") " & dsS.Tables(0).Rows(x - 1).Item("user_name") & "..."
                            Next x
                            rowTmp("holds_header") = sTmp
                        End If

                        'grab the line hold details for each sales order.  mostly redundant, try nesting w/above
                        sSQL = "SELECT * FROM sales_order_holds WHERE org_id = " & rowTmp("operating_unit_id") & " AND header_id = " & rowTmp("header_id") & " AND line_id = " & rowTmp("line_id") & "  ORDER BY last_update_date  "
                        dsS = dbS.Query(sSQL)
                        If dsS.Tables(0).Rows.Count > 0 Then
                            sTmp = ""
                            For x = 1 To dsS.Tables(0).Rows.Count
                                sTmp += dsS.Tables(0).Rows(x - 1).Item("hold_name") & " (" & dsS.Tables(0).Rows(x - 1).Item("last_update_date").ToString & ") " & dsS.Tables(0).Rows(x - 1).Item("user_name") & "..."
                            Next x
                            rowTmp("holds_line") = sTmp
                        End If

                    Next
                    adpTmp2.Update(dtTmp2)   'update the table
                End If
                dtTmp2.Dispose()
                adpTmp2.Dispose()
            Catch ex As Exception

            End Try

            createLoggingStamp("EnrichOrderDataMSC 3")

            'Count the number of problems and forward the order number information
            db.NonQuery("DELETE FROM tblWorkASCP")

            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3, number4, number5) " & _
                        " (SELECT a.organization_id, a.demand_id, COUNT(*), a.order_no, a.line_no FROM sales_Orders a LEFT JOIN msc_demand b ON a.demand_id = b.end_demand_id AND a.organization_id = b.organization_id " & _
                        "           WHERE a.demand_id Is Not NULL  " & _
                        " AND b.reasonCode IS NOT NULL  " & _
                        " GROUP BY a.organization_id, a.demand_id, a.order_no, a.line_no)")

            db.NonQuery(" UPDATE a SET a.no_of_problems = b.number3 " & _
                        " FROM sales_Orders a LEFT JOIN tblWorkASCP b  " & _
                        " ON a.organization_id = b.number1 AND a.demand_id = b.number2 " & _
                        " AND a.order_no = b.number4 AND a.line_no = b.number5")

            '--Update the order number
            db.NonQuery("DELETE FROM tblWorkASCP")

            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3,number4, date1) " & _
                        "( " & _
                        " SELECT a.order_no, a.line_no, a.demand_id, a.organization_id, min(schedule_ship_date) " & _
                        "  FROM Sales_Orders a LEFT JOIN msc_demand b " & _
                        "			ON a.demand_id = b.end_demand_id  " & _
                        "				AND a.organization_id = b.organization_id  " & _
                        "            WHERE a.demand_id Is Not NULL " & _
                        "  GROUP BY  a.order_no, a.line_no, a.demand_id, a.organization_id)")

            db.NonQuery("UPDATE b SET b.order_no = a.number1,b.line_no = a.number2 " & _
                        " FROM tblWorkASCP a LEFT JOIN msc_demand b " & _
                        " ON a.number3 = b.end_demand_id ")

            createLoggingStamp("EnrichOrderDataMSC")

            'update the supply table with data from closed purchase orders 
            dbS.NonQuery("UPDATE a SET a.purchase_order_no = a.order_number  FROM msc_supply a WHERE a.supply_type_desc IN ('PO in receiving','Intransit shipment','Purchase order') AND a.purchase_order_no IS NULL;")
            dbS.NonQuery("UPDATE a SET a.purchase_order_no = (left(a.order_number,charindex('(',a.order_number)))  FROM msc_supply a WHERE a.supply_type_desc IN ('PO in receiving','Intransit shipment','Purchase order') AND a.purchase_order_no LIKE '%(%';")
            dbS.NonQuery("UPDATE a SET a.purchase_order_no = REPLACE(a.purchase_order_no,'(','')  FROM msc_supply a WHERE a.supply_type_desc IN ('PO in receiving','Intransit shipment','Purchase order') AND a.purchase_order_no LIKE '%(%';")
            dbS.NonQuery("UPDATE a SET a.purchase_order_no = REPLACE(a.purchase_order_no,')','')  FROM msc_supply a WHERE a.supply_type_desc IN ('PO in receiving','Intransit shipment','Purchase order') AND a.purchase_order_no LIKE '%)%';")
            'Update the requisition numbers
            dbS.NonQuery("UPDATE msc_supply SET PURCHASE_ORDER_NO = 'PO Req:' + order_number WHERE supply_type_desc = 'Purchase requisition' AND PURCHASE_ORDER_NO is NULL")
            dbS.NonQuery("UPDATE a SET a.purchase_order_no = order_number FROM msc_supply a WHERE a.supply_type_desc IN ('Intransit shipment')")

            createLoggingStamp("EnrichOrderDataMSC")

            'Put drilldown information into the demand table
            dbS.NonQuery(" UPDATE a SET   a.job_no = b.job_no, a.op1 = b.op1, a.op2 = b.op2, a.op3 = b.op3, a.op4 =b.op4, a.op5 = b.op5 FROM msc_demand a INNER JOIN msc_supply b ON a.demand_id = b.demand_id WHERE b.job_no IS NOT NULL; ")
            dbS.NonQuery(" UPDATE a SET   a.PURCHASE_ORDER_NO = b.PURCHASE_ORDER_NO, a.op1 = b.op1 FROM msc_demand a INNER JOIN msc_supply b ON a.demand_id = b.demand_id WHERE b.PURCHASE_ORDER_NO IS NOT NULL; ")
            dbS.NonQuery(" UPDATE a SET   a.PURCH_LINE_NO = b.PURCH_LINE_NO FROM msc_demand a INNER JOIN msc_supply b ON a.demand_id = b.demand_id WHERE b.PURCH_LINE_NO IS NOT NULL; ")

            'Propagate the demand_priority through the pegging
            dbS.NonQuery(" UPDATE a SET a.demand_priority = b.demand_priority FROM msc_demand a INNER JOIN msc_demand b ON a.end_demand_id = b.demand_id WHERE a.demand_priority IS NULL; ")

            createLoggingStamp("EnrichOrderDataMSC")

            'Populate the operations sequence details on the sales order
            dbS.NonQuery(" UPDATE s SET operationSequenceDetails = j.hours_open FROM jobs j INNER JOIN sales_orders s on j.organization_id = s.organization_id " & _
                        " AND j.job_no = s.job_no  AND operation_seq_no = 20 ")

            'Delete redundant safety stock information
            dbS.NonQuery("UPDATE a SET a.temp ='DELETE ME' FROM msc_demand a LEFT join msc_supply b ON a.organization_id = b.organization_id AND a.demand_id = b.demand_id WHERe b.demand_id IS NULL AND a.safety_stock_quantity <> 0 AND a.demand_priority = '9999999' AND a.end_demand_id IS NULL ")
            dbS.NonQuery("DELETE FROM msc_demand WHERE temp = 'DELETE ME';")

            'dbS.NonQuery("UPDATE po SET release_no = 1 WHERE release_no = 0") '**** We want to go with the null values

            'Upstream we also have repetetive suggested schedules creating some demands for us which will leave us with empty demand priorities
            dbS.NonQuery("UPDATE msc_demand SET demand_priority = 99999 WHERE demand_type_desc = 'Work order demand' AND demand_priority is null")
            dbS.NonQuery("UPDATE msc_demand SET demand_priority = 999999 WHERE demand_type_desc = 'Planned order demand' AND demand_priority is null")

            'Update the parent job order number
            dbS.NonQuery("UPDATE msc_demand SET parentJobOrderNo = msc_parent_sales_order_no WHERE demand_type_desc = 'Work order demand'")

            'Update the parent job order status
            dbS.NonQuery("UPDATE d SET  d.STATUS = j.meaning FROM msc_demand d LEFT JOIN jobs j ON d.organization_id = j.organization_id AND d.msc_parent_sales_order_no = j.job_no  WHERE d.demand_type_desc = 'Work order demand'")

            'Really apply the purchase order promise date from release and shipment level
            dbS.NonQuery("UPDATE s SET s.PO_PROMISED_DATE = po.promised_date FROM msc_supply s INNER JOIn po ON po.po_header_id = s.purchase_order_id AND po.release_no = s.release_no AND po.shipment_no = s.shipment_no WHERE purchase_order_id IS NOT NULL")

            'Populate the qty open in the demand grid. It is the sum of upcoming supply not on hand
            dbS.NonQuery("TRUNCATE TABLE tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Number1, Number2, Number3)( " & _
                            " SELECT  d.organization_id, d.demand_id , " & _
                            " (SELECT isnull(SUM( s.supply_quantity),0) FROM msc_supply s WHERE d.demand_id = s.demand_id AND s.supply_type IN (1,2,3,5,8,11,14)) FROM msc_demand d) ")
            dbS.NonQuery("UPDATE d SET qty_open = t.number3 FROM msc_demand d INNER JOIN tblWorkASCP t ON d.organization_id = t.number1 AND d.demand_id = t.number2 ")

            'Update the firmed planning idea
            dbS.NonQuery("UPDATE msc_supply SET firm_planned_flag_display = 'Yes' WHERE firm_planned_type = 1")
            dbS.NonQuery("UPDATE msc_supply SET firm_planned_flag_display = 'No' WHERE firm_planned_type = 2")

            'Make sure to set the PO Release right ' We are going to propagate it from the ASCP table
            dbS.NonQuery("UPDATE po SET po.release_firm = 'No'")
            dbS.NonQuery("UPDATE po SET po.release_firm = s.firm_planned_flag_display FROM msc_supply s INNER JOIN po ON s.purchase_order_id = po.po_header_id AND s.shipment_no = po.shipment_no AND po.inventory_item_id = s.inventory_item_id WHERE s.release_no IS NULL ")
            dbS.NonQuery("UPDATE po SET po.release_firm = s.firm_planned_flag_display FROM msc_supply s INNER JOIN po ON s.purchase_order_id = po.po_header_id AND s.release_no = po.release_no AND s.shipment_no = po.shipment_no AND po.inventory_item_id = s.inventory_item_id ")

            dbS.NonQuery("UPDATE po SET neededBy = s.NEW_SCHEDULE_DATE FROM po INNER JOIN msc_supply s ON po.po_header_id = s.purchase_order_id AND po.line_no = s.purch_line_no AND po.shipment_no = s.shipment_no WHERE po.neededBy IS NULL")

            'Put in the sales order information on all levels of the demand table
            dbS.NonQuery("UPDATE d SET  d.parent_sales_order_no = CONVERT(VARCHAR,CONVERT(INTEGER,s.order_no)), d.parent_sales_order_line = CONVERT(VARCHAR,CONVERT(INTEGER,s.line_no)), d.order_no = CONVERT(VARCHAR,CONVERT(INTEGER,s.order_no)), d.line_no = CONVERT(VARCHAR,CONVERT(INTEGER,s.line_no)) FROM msc_demand d LEFT JOIN sales_orders s ON s.demand_id = d.end_demand_id WHERE s.order_no IS NOT NULL AND (d.parent_sales_order_no IS NULL OR d.parent_sales_order_no = '') ")

        Catch ex As Exception
            createErrorLog("EnrichOrderDataMSC", ex)
        Finally
            dbS.Disconnect()
        End Try

    End Sub

    Private Sub buildSalesOrderShortageDetail()

        createLoggingStamp("buildSalesOrderShortageDetail")
        Dim sTmp2 As String
        Dim sTmp As String
        Dim dsS As DataSet
        Dim sSQL As String
        Dim lngTmp As Integer
        Dim conn As New SqlClient.SqlConnection(My.Settings("FLOWConnectionString"))

        Try
            'COPY REPLENISHMENT DETAILS into sales order table
            Dim adpTmp As New SqlClient.SqlDataAdapter("SELECT * FROM sales_orders  WHERE demand_id IS NOT NULL  AND (no_of_processes IS NOT NULL or job_no IS NOT NULL) ORDER BY organization_id, demand_id  ", conn)
            Dim cbTmp As New SqlClient.SqlCommandBuilder(adpTmp)
            Dim dtTmp As New DataTable
            adpTmp.Fill(dtTmp)
            If dtTmp.Rows.Count > 0 Then
                For Each rowTmp As DataRow In dtTmp.Rows
                    sTmp2 = ""
                    sTmp = ""
                    'grab all open operation numbers for this order
                    If Not DBNull.Value.Equals(rowTmp("job_no")) Then
                        sSQL = "SELECT OPERATION_SEQ_NO  FROM jobs  WHERE organization_id = " & rowTmp("organization_id") & " AND QUANTITY_OPEN > 0 AND JOB_NO = '" & rowTmp("job_no") & "'  ORDER BY OPERATION_SEQ_NO "
                        dsS = dbS.Query(sSQL)
                        lngTmp = dsS.Tables(0).Rows.Count
                        If lngTmp > 0 Then
                            For x = 0 To lngTmp - 1
                                sTmp2 += "{" & dsS.Tables(0).Rows(x).Item("operation_seq_no") & "}"
                            Next x
                        End If
                    End If
                    'grab the lower level shortage details for this order
                    If Not DBNull.Value.Equals(rowTmp("no_of_processes")) Then
                        sSQL = "SELECT mrp_qty, item_no, SUBSTRING(description,1,6) AS descr, planner_code, purchase_order_no, purch_line_no, po_promised_date, Job_no, start_date  FROM msc_supply  WHERE organization_id = " & rowTmp("organization_id") & " AND end_demand_id = " & rowTmp("demand_id") & "  AND [short] > 0 AND level_no > 0  ORDER BY make_buy, demand_id "
                        dsS = dbS.Query(sSQL)
                        lngTmp = dsS.Tables(0).Rows.Count
                        If lngTmp > 0 Then
                            For x = 0 To lngTmp - 1 'create a string that shows each lower level shortage for each sales order line (abbreviated)
                                sTmp += "(" & dsS.Tables(0).Rows(x).Item("mrp_qty").ToString & ")" & dsS.Tables(0).Rows(x).Item("item_no").ToString & "|" & dsS.Tables(0).Rows(x).Item("descr").ToString & "|" & dsS.Tables(0).Rows(x).Item("planner_code").ToString
                                'enter PO fulfillment info
                                If Not DBNull.Value.Equals(dsS.Tables(0).Rows(x).Item("purch_line_no")) Then
                                    sTmp += "(" & dsS.Tables(0).Rows(x).Item("purchase_order_no") & "-" & dsS.Tables(0).Rows(x).Item("purch_line_no")
                                    If Not DBNull.Value.Equals(dsS.Tables(0).Rows(x).Item("po_promised_date")) Then sTmp += "|" & Format(dsS.Tables(0).Rows(x).Item("po_promised_date"), "dd-MMM-yy")
                                    sTmp += ")"
                                End If
                                'enter job fulfillment info
                                If Not DBNull.Value.Equals(dsS.Tables(0).Rows(x).Item("job_no")) Then
                                    sTmp += "(" & dsS.Tables(0).Rows(x).Item("job_no").ToString
                                    If Not DBNull.Value.Equals(dsS.Tables(0).Rows(x).Item("start_date")) Then sTmp += "|" & Format(dsS.Tables(0).Rows(x).Item("start_date"), "dd-MMM-yy")
                                    sTmp += ")"
                                End If
                                sTmp += ". . ."
                            Next x
                        End If
                    End If
                    If sTmp <> "" Or sTmp2 <> "" Then rowTmp("Replenishment_Details") = Mid(sTmp2, 1, 1000) & "   " & Mid(sTmp, 1, 1900)
                Next
                adpTmp.Update(dtTmp)     'update the table
            End If
            dtTmp.Dispose()
            adpTmp.Dispose()
            createLoggingStamp("buildSalesOrderShortageDetail")
        Catch ex As Exception
            createErrorLog("buildSalesOrderShortageDetail", ex)
        Finally
            db.Disconnect()

        End Try
    End Sub

    Private Sub AttachComponentDetails()

        Dim x As Integer
        Dim lngTmp As Long
        Dim sSQL As String
        Dim sTmp As String
        Dim sTmp2 As String
        Dim sProjectComponent As String
        Dim dsS As New DataSet
        Dim conn As New SqlClient.SqlConnection(My.Settings("FLOWConnectionString"))

        Try

            'Keep connection to the database
            dbS.Connect()
            createLoggingStamp("AttachComponentDetails")

            'copy the need by date from the job to the bom so we can allocate inventory from oldest locators to the jobs that will run first
            sSQL = "UPDATE b SET neededBy = j.neededBy, stockDetails = NULL  FROM open_boms b INNER JOIN jobs j  ON b.organization_id = j.organization_id " & _
             "AND b.wip_entity_id = j.wip_entity_id "
            dbS.NonQuery(sSQL)

            'cycle through each open wip component requirement and place the locator / on-hand details in it, observing Seiban constraints.  ignore backflushed material
            sSQL = "SELECT * FROM open_boms WHERE (supply_subinventory IS NULL)  ORDER BY inventory_item_id, seiban_controlled desc, neededBy"
            Dim adpTmp As New SqlClient.SqlDataAdapter(sSQL, conn)
            Dim cbTmp As New SqlClient.SqlCommandBuilder(adpTmp)
            Dim dtTmp As New DataTable
            adpTmp.Fill(dtTmp)
            If dtTmp.Rows.Count > 0 Then
                For Each rowTmp As DataRow In dtTmp.Rows
                    If rowTmp("quantity_issued") < rowTmp("required_quantity") Then 'if we haven't issued the material to this operation
                        'get the locators & qty on hand for this item / seiban
                        sTmp = ""
                        sProjectComponent = ""
                        sSQL = "SELECT * FROM items_QOH_detail d WHERE d.organization_id = " & rowTmp("organization_id") & " AND d.inventory_item_id = " & rowTmp("inventory_item_id") & " AND d.project_id "
                        sTmp2 = "IS NULL"
                        If Not DBNull.Value.Equals(rowTmp("seiban_controlled")) And Not DBNull.Value.Equals(rowTmp("project_id")) Then
                            sTmp = rowTmp("seiban_controlled").ToString.ToLower
                            If Left(sTmp, 1) <> "n" Then
                                sTmp2 = "= " & rowTmp("project_id") 'if this bill calls for seiban controlled material, select only seiban locators.  if the user entered N or No for Seiban then it is not seiban controlled.  if it is blank it is not seiban controlled.   *** business rule
                                sProjectComponent = "(CM) " 'NEED a messaging table ***
                            End If
                        End If
                        sSQL += sTmp2 & " ORDER BY subinventory_code, last_update_date"
                        dsS = dbS.Query(sSQL)
                        lngTmp = dsS.Tables(0).Rows.Count
                        If lngTmp > 0 Then
                            sTmp = sProjectComponent
                            For x = 0 To lngTmp - 1
                                If x > 0 Then sTmp += "..."
                                sTmp += "(" & dsS.Tables(0).Rows(x).Item("Qty_Onhand") & ")" & dsS.Tables(0).Rows(x).Item("SubInventory_Code") & " " & dsS.Tables(0).Rows(x).Item("Locator")
                            Next
                            If sTmp <> "" Then rowTmp("stockDetails") = Left(sTmp, 100) 'there is no reason to show more than 100 characters of stock locator info, that's about 10 locations.
                        End If
                        If String.IsNullOrEmpty(sTmp) = True Or Len(sTmp) = 1 Then rowTmp("stockDetails") = "no stock" 'indicate if no stock exists for this requirement    '*** this should be in the messages table, along with all other string constants in this program
                    Else
                        rowTmp("stockDetails") = "ISSUED"   '*** THIS string belongs in a messaging table!!
                    End If
                Next
                adpTmp.Update(dtTmp)                        'update the table
            End If
            dtTmp.Dispose()
            adpTmp.Dispose()
            createLoggingStamp("AttachComponentDetails")

            'now that the component locator info is in the open boms table, we can copy that info into the job operations.
            'mark each releasd job operation sequence where there are some push components
            dbS.NonQuery("UPDATE jobs SET componentRequirements = NULL")
            sSQL = "UPDATE j SET componentRequirements = 'temp'  FROM jobs j  INNER JOIN open_boms b  ON  j.organization_id = b.organization_id AND " & _
             "j.wip_entity_id = b.wip_entity_id AND j.operation_seq_no = b.operation_seq_no WHERE b.stockDetails IS NOT NULL"
            dbS.NonQuery(sSQL)

            'cycle thru each releasd job with unissued components and pick up each component tied to each job operation. Write this info to job component requirements
            Dim adpTmp2 As New SqlClient.SqlDataAdapter("SELECT * FROM jobs WHERE componentRequirements IS NOT NULL ORDER BY neededBy, wip_entity_id, operation_seq_no ", conn)
            Dim cbTmp2 As New SqlClient.SqlCommandBuilder(adpTmp2)
            Dim dtTmp2 As New DataTable
            adpTmp2.Fill(dtTmp2)
            If dtTmp2.Rows.Count > 0 Then
                For Each rowTmp As DataRow In dtTmp2.Rows
                    'grab the component requirements info for this job operation
                    sSQL = "SELECT b.item_no, b.description, b.required_quantity, b.quantity_issued, b.seiban_controlled, b.project_id, b.stockDetails, ai.drawing, ai.fsg_material_code " & _
                     "FROM open_boms b LEFT JOIN active_items ai ON b.organization_id = ai.organization_id AND b.inventory_item_id = ai.inventory_item_id " & _
                     "WHERE b.organization_id =  " & rowTmp("organization_id") & " AND b.wip_entity_id = " & rowTmp("wip_entity_id") & " AND b.operation_seq_no = " & rowTmp("operation_seq_no") & _
                     " AND stockDetails IS NOT NULL   ORDER BY b.description"
                    dsS = dbS.Query(sSQL)
                    lngTmp = dsS.Tables(0).Rows.Count
                    If lngTmp > 0 Then
                        sTmp = ""
                        'build a long string describing all the components that are attached to this operation
                        For x = 0 To lngTmp - 1
                            If x > 0 Then sTmp += "    " & vbCrLf
                            sTmp += dsS.Tables(0).Rows(x).Item("item_no").ToString & " " & Left(dsS.Tables(0).Rows(x).Item("Description"), 10).ToString & " " & dsS.Tables(0).Rows(x).Item("drawing").ToString & " " & dsS.Tables(0).Rows(x).Item("fsg_material_code").ToString & " Req:" & dsS.Tables(0).Rows(x).Item("required_quantity").ToString & " Iss:" & dsS.Tables(0).Rows(x).Item("quantity_issued").ToString & " "
                            If Not DBNull.Value.Equals(dsS.Tables(0).Rows(x).Item("seiban_controlled")) Then
                                sTmp2 = dsS.Tables(0).Rows(x).Item("seiban_controlled").ToString.ToLower
                                If Left(sTmp2, 1) <> "n" Then
                                    sTmp += "(CM)" 'this is necessary because sometimes there are no stock details (no stock available), and we need to flag the user that this is controlled material
                                End If
                            End If
                            sTmp += " " & dsS.Tables(0).Rows(x).Item("stockDetails").ToString
                        Next
                        If sTmp <> "" Then rowTmp("componentRequirements") = Left(sTmp, 2000) '*** should pull the field length from the database!
                    End If
                Next
                createLoggingStamp("AttachComponentDetails")
                adpTmp2.Update(dtTmp2)   'update the table 
            End If
            dtTmp2.Dispose()
            adpTmp2.Dispose()
            createLoggingStamp("AttachComponentDetails")

        Catch ex As Exception
            createErrorLog("AttachComponentDetails", ex)
        Finally
            dbS.Disconnect()
        End Try
    End Sub

    Private Sub displayShortagesForOnHand()

        Try

            db.Connect()
            Dim ds As New DataSet
            Dim sSQL As String

            db.NonQuery("DELETE FROM tblWorkASCP")

            'Site specific SQL statement to determine the items with problems
            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, text1, number3, text2) " & _
                        " SELECT SUM(QTY_ONHAND) qty_oh, inventory_item_id, LOCATOR, organization_id, subinventory_code  " & _
                        "   FROM Items_QOH_Detail WHERE locator LIKE '%REWORK%'  " & _
                        "    OR locator LIKE '%INSP%'   " & _
                        "    OR locator LIKE '%7 GIVENS%'    " & _
                        "   OR locator LIKE '4%'             " & _
                        "    OR locator LIKE '5%'            " & _
                        "    OR locator LIKE '7%'            " & _
                        "    OR (subinventory_code = 'OS' AND locator LIKE '%7%') " & _
                        "    GROUP BY inventory_item_id, LOCATOR, organization_id, subinventory_code ")
            'Statement for Hamburg


            Dim sTmp As String = ""
            Dim sItemProj As String = ""
            Dim sItemProjPrev As String = ""
            Dim dblNetShort As Double
            Dim dblShortPrev As Double
            Dim dblSupply As Double
            Dim x As Integer
            Dim y As Integer
            Dim dblQtyOnHand As Double
            Dim dsS As New DataSet
            Dim sMessage As String = ""

            'Seperate the quantity that is not in these inventories
            'Iterate in the on - hand inventories and 
            'CALCULATE QTY SHORT for each item-project
            'Open MRP demand table sorted by org, item, project and consumption priority 
            sSQL = "SELECT a.*, (SELECT SUM(number1) FROM tblWorkASCP c WHERE a.organization_id = c.number3 AND a.inventory_item_id = c.number2) qtyFirstThreshould FROM SAVE_MSC_DEMAND a INNER JOIN tblWorkASCP b ON a.organization_id = b.number3 AND a.inventory_item_id = b.number2  WHERE a.demand_id IS NOT NULL  ORDER BY a.ORGANIZATION_ID, a.INVENTORY_ITEM_ID, a.PROJECT_ID, consumptionPriorityDate, MRP_QTY"
            Dim dt As DataTable = db.Query(sSQL).Tables(0)


            ''cycle through each demand, and calculate if the part has any difficult supplies before the net shortages
            For Each Row As DataRow In dt.Rows
                sItemProj = Row("ORGANIZATION_ID").ToString & Row("INVENTORY_ITEM_ID").ToString & Row("PROJECT_ID").ToString
                'Only required for non project stuff   *** also project stuff can reside in a difficult 
                'If DBNull.Value.Equals(Row("PROJECT_ID")) Then
                If sItemProj <> sItemProjPrev Then 'If new item number, reset variables
                    sItemProjPrev = sItemProj
                    If DBNull.Value.Equals(Row("QTY_ONHAND")) Or DBNull.Value.Equals(Row("qtyFirstThreshould")) Then
                        dblQtyOnHand = 0
                    Else
                        dblQtyOnHand = Row("QTY_ONHAND") - Row("qtyFirstThreshould")
                    End If
                    'Creating the supply message
                    Dim dtMessage As DataTable = db.SecureQueryParams("SELECT number1, text1, text2 FROM tblWorkASCP WHERE number3 = @1 AND number2 = @2", Row("ORGANIZATION_ID"), Row("INVENTORY_ITEM_ID")).Tables(0)
                    sMessage = ""
                    For Each row2 As DataRow In dtMessage.Rows
                        sMessage += row2.Item("text2").ToString + " - " + row2.Item("text1").ToString + "(" + row2.Item("number1").ToString + ") | "
                    Next
                End If
                dblQtyOnHand -= Row("MRP_QTY") 'subtract MRP needed qty from qty on hand
                'if we are short, update the MRP Demand table
                If dblQtyOnHand < 0 AndAlso DBNull.Value.Equals(Row("Short")) Then    'We are not going for the shortages
                    'Row("Short") = -dblQtyOnHand
                    db.SecureNonQueryParams("UPDATE msc_demand SET supply_type = @1 WHERE demand_id = @2", sMessage, Row("demand_id"))
                End If
                'End If
            Next

            'Set the top sales order info that we are running with on hand items that may cause problems
            sSQL = "SELECT end_demand_id, supply_type, item_no, organization_id FROM msc_demand WHERE supply_type IS NOT NULL AND supply_type NOT IN ('Planned order', 'On Hand', 'Discrete Job', 'Purchase Order')  AND end_demand_id IS NOT NULL ORDER BY end_demand_id, item_no "
            dt = db.Query(sSQL).Tables(0)
            sMessage = ""
            For Each row As DataRow In dt.Rows
                sItemProj = row("end_demand_id")
                If sItemProj <> sItemProjPrev Then
                    If Not sMessage = "" Then
                        db.SecureNonQueryParams("UPDATE sales_orders SET onHandSupplyHolds = @1 WHERE demand_id = @2", sMessage, sItemProjPrev)
                    End If
                    sItemProjPrev = sItemProj
                    sMessage = ""
                End If
                sMessage += row("item_no") + row("supply_type") + "||"
            Next


            db.NonQuery("DELETE FROM tblWorkASCP")

            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3,number4, date1) " & _
                        "( " & _
                        " SELECT a.order_no, a.line_no, a.demand_id, a.organization_id, min(schedule_ship_date) " & _
                        "  FROM Sales_Orders a LEFT JOIN msc_demand b " & _
                          "	ON a.demand_id = b.end_demand_id " & _
                           "	AND a.organization_id = b.organization_id  " & _
                                  "  WHERE(a.demand_id Is Not NULL) " & _
                         " GROUP BY  a.order_no, a.line_no, a.demand_id, a.organization_id " & _
                         " ) ")

            '--missing piece for chesapeake to operate again
            db.NonQuery("UPDATE b SET b.order_no = a.number1,b.line_no = a.number2 " & _
                       " FROM tblWorkASCP a LEFT JOIN msc_demand b " & _
                       " ON a.number3 = b.end_demand_id")

            createLoggingStamp("displayShortagesForOnHand")



            'Set the shop floor due date
            db.NonQuery("DELETE FROM tblWorkASCP")

            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, date1)  " & _
               " SELECT organization_id, wip_entity_id, MIN (first_unit_start_date) " & _
               " FROM jobs GROUP BY organization_id, wip_entity_id ")

            db.NonQuery("DELETE FROM tblWork2ASCP ")

            'Find out the end assembly jobs
            db.NonQuery("INSERT INTO tblWork2ASCP (number1, number2, number3, date1) " & _
                        " SELECT a.job_id, a.organization_id,a.demand_id, b.date1  FROM msc_demand a JOIN tblWorkASCP b ON a.organization_id = b.number1 AND a.job_id = b.number2 " & _
                        "       WHERE(a.demand_id = a.end_demand_id) " & _
                        "	GROUP BY a.job_id, a.organization_id, b.date1, a.demand_id ")

            db.NonQuery("DELETE FROM tblWorkASCP")
            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, date1) " & _
                        " SELECT a.job_id,a.organization_id, MIN(b.date1) FROM msc_demand a JOIN tblWork2ASCP b ON a.end_demand_id = b.number3 " & _
                        " and a.organization_id = b.number2 GROUP BY a.job_id,a.organization_id ")

            db.NonQuery("UPDATE a SET a.parentJobStartDate = dateadd(day,-1,b.date1) FROM jobs a JOIN tblWorkASCP b ON a.wip_entity_id = b.number1 " & _
                        " AND a.organization_id = b.number2 ")

            'update the number of problems
            db.NonQuery("DELETE FROM tblWorkASCP")

            db.NonQuery("INSERT INTO tblWorkASCP (number1, number2, number3, number4, number5) " & _
                        " (SELECT a.organization_id, a.demand_id, COUNT(*), a.order_no, a.line_no FROM sales_Orders a LEFT JOIN msc_demand b ON a.demand_id = b.end_demand_id AND a.organization_id = b.organization_id " & _
                        "    WHERE(a.demand_id Is Not NULL) " & _
                        " AND b.reasonCode IS NOT NULL " & _
                        " GROUP BY a.organization_id, a.demand_id, a.order_no, a.line_no) ")

            db.NonQuery("UPDATE a SET a.no_of_problems = b.number3  " & _
                        " FROM sales_Orders a LEFT JOIN tblWorkASCP b " & _
                        " ON a.organization_id = b.number1 AND a.demand_id = b.number2 " & _
                        " AND a.order_no = b.number4 AND a.line_no = b.number5 ")

            'Job firm flag
            ' 1 = firm
            ' 2 = not firmed
            db.NonQuery("UPDATE jobs SET firm_planned_flag_display = 'Yes' WHERE firm_planned_flag = 1")
            db.NonQuery("UPDATE jobs SET firm_planned_flag_display = 'No' WHERE firm_planned_flag = 2")

            'Not we have to populate this column in supply and demand and sales order line level
            db.NonQuery("UPDATE a SET a.firm_planned_flag_display = b.firm_planned_flag_display FROM  msc_demand a JOIN jobs b on a.organization_id = b.organization_id and a.job_no = b.job_no ")
            db.NonQuery("UPDATE a SET a.firm_planned_flag_display = b.firm_planned_flag_display FROM  msc_supply a JOIN jobs b on a.organization_id = b.organization_id and a.job_no = b.job_no ")
            db.NonQuery("UPDATE a SET a.firm_planned_flag_display = b.firm_planned_flag_display FROM  sales_orders a JOIN jobs b on a.organization_id = b.organization_id and a.job_no = b.job_no")

        Catch ex As Exception

            createErrorLog("displayShortagesForOnHand", ex)

        Finally
            db.Disconnect()
        End Try

    End Sub

    Private Sub buildAdditionalInfoForOneDemandWithMultiplSOLinesUpwards()
        dbS.Connect()
        createLoggingStamp("buildAdditionalInfoForOneDemandWithMultiplSOLinesUpwards - 1")

        dbS.NonQuery("TRUNCATE TABLE tblWorkASCP")
        dbS.NonQuery("INSERT INTO tblWorkASCP (Number1, Number2, Number3) (SELECT  d.organization_id,d.demand_id, " & _
                        "(SELECT count(distinct(s.end_demand_id)) FROM msc_supply s WHERE d.demand_id = s.demand_id AND d.organization_id = s.organization_id ) countDistinctOrders " & _
                        "FROM msc_demand d WHERE d.order_no IS NOT NULL " & _
                        "AND (SELECT count(distinct(s.end_demand_id)) FROM msc_supply s WHERE d.demand_id = s.demand_id AND d.organization_id = s.organization_id )  <> 1)")
        dbS.NonQuery("UPDATE d SET d.countDistinctSalesOrderLines = t.Number3 FROm msc_demand d INNER JOIN tblWorkASCP t ON d.demand_id = t.Number2 AND d.organization_id = t.Number1")


        Dim SSQL As String
        Dim conn As New SqlClient.SqlConnection(My.Settings("FLOWConnectionString"))
        'WHERE demand_id =25643203
        SSQL = "SELECT d.demand_id, d.listOfSalesOrderLines FROM msc_demand d INNER JOIN tblWorkASCP t ON d.demand_id = t.Number2 AND d.organization_id = t.Number1 ORDER BY demand_id ASC"
        'make and adapter and another table to write this together 'make sure all sorts of supply are shown
        'AND d.demand_id =25643203
        Dim dsSupply As DataSet = db.Query("SELECT s.demand_id, s.order_no, s.line_no, s.allocated_quantity FROM tblWorkASCP t INNER JOIN msc_supply s ON s.demand_id = t.Number2 and s.organization_id = t.Number1 WHERE s.order_no IS NOT NULL ORDER BY s.demand_id")

        '   order_number _ line_no: | allocated quantity | " "
        'link each mrp shortage to a replenishment, start by opening the demand table selecting net shortages only for this level
        Dim adpmrpdemand As New SqlClient.SqlDataAdapter(SSQL, conn)
        Dim cbmrpdemand As New SqlClient.SqlCommandBuilder(adpmrpdemand)
        Dim dtmrpdemand As New DataTable
        adpmrpdemand.Fill(dtmrpdemand)

        'cycle through each net shortage
        If dtmrpdemand.Rows.Count > 0 Then
            Dim startInteger As Integer = 0
            For Each rowmrpdemand As DataRow In dtmrpdemand.Rows
                Dim strNewMessage As String = ""
                For i As Integer = startInteger To dsSupply.Tables(0).Rows.Count - 1
                    If dsSupply.Tables(0).Rows(i).Item("demand_id") = rowmrpdemand.Item("demand_id") Then
                        strNewMessage += dsSupply.Tables(0).Rows(i).Item("order_no").ToString + "_" + dsSupply.Tables(0).Rows(i).Item("line_no").ToString + " :" + dsSupply.Tables(0).Rows(i).Item("allocated_quantity").ToString + " | "
                        startInteger += 1
                    Else
                        Exit For
                    End If
                Next
                rowmrpdemand.Item("listOfSalesOrderLines") = strNewMessage
            Next
        End If
        createLoggingStamp("buildAdditionalInfoForOneDemandWithMultiplSOLinesUpwards")
        adpmrpdemand.Update(dtmrpdemand)     'update the mrp demand table
        createLoggingStamp("buildAdditionalInfoForOneDemandWithMultiplSOLinesUpwards")

        dbS.Disconnect()
    End Sub


    Private Sub CreateUnreleasedPartialAvailable()
        Try
            Dim sSQL As String

            dbS.Connect()
            createLoggingStamp("CreateUnreleasedPartialAvailable - 1")

            dbS.NonQuery("DELETE FROM tblWorkASCP")
            dbS.NonQuery("DELETE FROM tblWork2ASCP")

            'Join the work table 1 against the mrp demand table
            dbS.NonQuery("INSERT INTO tblWork2ASCP (number2, text1) " & _
                        " (SELECT  d.organization_id, d.job_no " & _
                        " FROM msc_demand d WHERE d.job_no IS NOT NULL " & _
                        " AND (SELECT top 1 j.meaning FROM jobs j WHERe j.organization_id = d.organization_id AND j.job_no = d.job_no) = 'Unreleased' " & _
                        " AND  (SELECT COUNT(distinct(s2.inventory_item_id)) FROM msc_supply s INNER JOIN msc_supply s2 ON s.organization_id = s2.organization_id AND s.pegging_id = s2.prev_pegging_id WHERE d.organization_id = s.organization_id AND d.demand_id = s.demand_id) =(SELECT COUNT(distinct(s2.inventory_item_id)) FROM msc_supply s INNER JOIN msc_supply s2 ON s.organization_id = s2.organization_id AND s.pegging_id = s2.prev_pegging_id WHERE d.organization_id = s.organization_id AND d.demand_id = s.demand_id AND s2.supply_type_desc = 'On Hand') " & _
                        " GROUP BY d.organization_id, d.job_no) ")

            dbS.NonQuery("UPDATE a SET a.meaning = 'Unreleased Partial Available' FROM jobs a JOIN tblWork2ASCP b	ON a.job_no = b.text1 AND a.organization_id = b.number2")
            dbS.NonQuery("UPDATE a SET a.meaning = 'Unreleased' FROM jobs a WHERE a.meaning = 'Unreleased Partial Available' AND a.componentRequirements LIKE '%no stock%'")

            'Post the new job statuses to the msc demand and supply table
            sSQL = "UPDATE d SET status = j.meaning  FROM msc_supply d  INNER JOIN jobs j ON d.ORGANIZATION_ID = j.ORGANIZATION_ID AND d.job_id = j.wip_entity_id  "
            dbS.NonQuery(sSQL)
            'Update the parent job order status
            dbS.NonQuery("UPDATE d SET  d.STATUS = j.meaning FROM msc_demand d LEFT JOIN jobs j ON d.organization_id = j.organization_id AND d.msc_parent_sales_order_no = j.job_no  WHERE d.demand_type_desc = 'Work order demand'")
            dbS.NonQuery("UPDATE msc_demand SET parent_sales_order_no = NULL, parent_sales_order_line = NULL WHERE parent_sales_order_no = '' OR parent_sales_order_line = ''")
            dbS.Disconnect()

        Catch ex As Exception
            createErrorLog("displayShortagesForOnHand", ex)
        End Try
    End Sub

    Private Sub createExceptionMessages()

        Try

            dbS.Connect()

            createLoggingStamp("createExceptionMessages - 1")
            dbS.NonQuery("UPDATE po SET po.exceptionMessage = NULL, po.cancellationMessage = NULL, po.rescheduleIN = NULL, po.rescheduleOUT = NULL ")

            'Prepare the table for the required transformation steps
            '***R12 Remediation
            dbS.NonQuery("UPDATE mscPlanningExceptions SET make_buy = 'B', purchase_order_no = left(order_number, charindex('(',order_number) - 1) WHERE order_number LIKE '%(%' ")
            dbS.NonQuery("UPDATE mscPlanningExceptions SET make_buy = 'M', job_no = order_number WHERE order_number NOT LIKE '%(%' ")


            If r12Instance Then
                '*** Make it work again
                Try
                    dbS.NonQuery("UPDATE mscPlanningExceptions SET RELEASE_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))) WHERE  (len(order_number) - len(replace(order_number, '(', ''))) = 3 ")
                    dbS.NonQuery("UPDATE mscPlanningExceptions SET SHIPMENT_NO = replace(replace(right(substring(order_number,(charindex(')',order_number) +2), (len(order_number) - (charindex(')',order_number) +2))),charindex(')',substring(order_number,(charindex(')',order_number) +2), (len(order_number) - (charindex(')',order_number) +2))))-1),'(',''),')','') WHERE (len(order_number) - len(replace(order_number, '(', ''))) = 3 ")

                Catch ex As Exception
                    createErrorLog("Shipment and release number", ex)
                End Try
                'Try
                '    dbS.NonQuery("UPDATE mscPlanningExceptions SET SHIPMENT_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))) WHERE (len(order_number) - len(replace(order_number, '(', ''))) =1 ")
                '    dbS.NonQuery("UPDATE mscPlanningExceptions SET RELEASE_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))), SHIPMENT_NO = substring(order_number,(charindex(')',order_number) +2), (len(order_number) - (charindex(')',order_number) +2))) WHERE (len(order_number) - len(replace(order_number, '(', ''))) =2 ")

                'Catch ex As Exception

                'End Try
               
            Else
                dbS.NonQuery("UPDATE mscPlanningExceptions SET SHIPMENT_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))) WHERE (len(order_number) - len(replace(order_number, '(', ''))) =1 ")
                dbS.NonQuery("UPDATE mscPlanningExceptions SET RELEASE_NO = substring(order_number,(charindex('(',order_number) +1), (charindex(')',order_number) - (charindex('(',order_number) +1))), SHIPMENT_NO = substring(order_number,(charindex(')',order_number) +2), (len(order_number) - (charindex(')',order_number) +2))) WHERE (len(order_number) - len(replace(order_number, '(', ''))) =2 ")

            End If
            
            dbS.NonQuery("UPDATE mpe SET mpe.purchase_order_no = p.po_no, mpe.release_no = p.release_no, mpe.shipment_no = p.shipment_no, mpe.purch_line_num = p.line_no , mpe.ospJobNo = j.job_no " & _
                            " FROM jobs j INNER JOIN PO p ON j.ospItem = p.ITEM_NO AND j.WIP_ENTITY_ID = p.WIP_ENTITY_ID AND j.ORGANIZATION_ID = p.SHIP_TO_ORGANIZATION_ID " & _
                            " INNER JOIN mscPlanningExceptions mpe ON mpe.job_no = j.job_no AND mpe.organization_id = j.organization_id ")


            'Now we want to post the cancellation messages into the purchase order table
            'dbS.NonQuery("UPDATE po SET po.exceptionMessage = mpe.meaning  FROM po INNER JOIN  mscPlanningExceptions mpe ON mpe.purchase_order_no = po.po_no AND isnull(mpe.release_no,0) = isnull(po.release_no,0) AND mpe.shipment_no = po.shipment_no AND po.item_no = mpe.item_name AND isnull(mpe.project_number,0) = isnull(po.project_name,0) AND mpe.purch_line_num = po.line_no  ")
            dbS.NonQuery("UPDATE po SET po.cancellationMessage = 'Yes', po.exceptionMessage =  mpe.meaning,po.ospJobNumber = mpe.ospJobNo     FROM po INNER JOIN  mscPlanningExceptions mpe ON mpe.purchase_order_no = po.po_no AND isnull(mpe.release_no,0) = isnull(po.release_no,0) AND mpe.shipment_no = po.shipment_no AND isnull(mpe.project_number,0) = isnull(po.project_name,0) AND mpe.purch_line_num = po.line_no  WHERE mpe.meaning = 'Orders to be cancelled' ")

            dbS.NonQuery("UPDATE po SET po.rescheduleOUT = mpe.date2, po.exceptionMessage =  mpe.meaning, po.ospJobNumber = mpe.ospJobNo    FROM po INNER JOIN  mscPlanningExceptions mpe ON mpe.purchase_order_no = po.po_no AND isnull(mpe.release_no,0) = isnull(po.release_no,0) AND mpe.shipment_no = po.shipment_no AND isnull(mpe.project_number,0) = isnull(po.project_name,0) AND mpe.purch_line_num = po.line_no  WHERE mpe.meaning = 'Orders to be rescheduled out' ")
            dbS.NonQuery("UPDATE po SET po.rescheduleOUT = getDate(), po.exceptionMessage =  mpe.meaning, po.ospJobNumber = mpe.ospJobNo    FROM po INNER JOIN  mscPlanningExceptions mpe ON mpe.purchase_order_no = po.po_no AND isnull(mpe.release_no,0) = isnull(po.release_no,0) AND mpe.shipment_no = po.shipment_no AND isnull(mpe.project_number,0) = isnull(po.project_name,0) AND mpe.purch_line_num = po.line_no  WHERE mpe.meaning = 'Past due orders' ")

            dbS.NonQuery("UPDATE po SET po.rescheduleIN = isnull(po.new_dock_date, mpe.date1), po.exceptionMessage =   mpe.meaning, po.ospJobNumber = mpe.ospJobNo     FROM po INNER JOIN  mscPlanningExceptions mpe ON mpe.purchase_order_no = po.po_no AND isnull(mpe.release_no,0) = isnull(po.release_no,0) AND mpe.shipment_no = po.shipment_no AND isnull(mpe.project_number,0) = isnull(po.project_name,0) AND mpe.purch_line_num = po.line_no  WHERE mpe.meaning = 'Orders to be rescheduled in' ")

            'Do the: Additional Autobahn Exception - Reschedule into planning time fence!
            dbS.NonQuery("TRUNCATE TABLE tblWorkASCP")
            dbS.NonQuery("INSERT INTO tblWorkASCP (Date1,text1,number2,number3,number4,date2, date3)( SELECT  MIN(d.mrp_date), s.purchase_order_no,s.purch_line_no, s.release_no,isnull(s.shipment_no,1) shipment_no, s.PO_PROMISED_DATE, d.planning_time_fence_date  FROM msc_demand d INNER JOIN msc_supply s ON d.demand_id = s.demand_id AND s.organization_id = d.organization_id  AND d.mrp_date < d.planning_time_fence_date AND (s.PO_PROMISED_DATE > d.mrp_date OR s.PO_PROMISED_DATE IS NULL) WHERE (d.demand_type_desc = 'Sales Orders' OR d.END_DEMAND_ID IS NOT NULL) GROUP BY s.purchase_order_no,s.purch_line_no, s.release_no,s.shipment_no, s.PO_PROMISED_DATE, d.planning_time_fence_date )")
            'Put in another step to truly identify the ones we want to know
            dbS.NonQuery("DELETE FROM tblWorkASCP WHERE Date1 > date3")

            dbS.NonQuery("UPDATE po SET po.exceptionMessage = 'Additional Autobahn Exception - Reschedule into planning time fence! ', po.rescheduleIN = t.date1, po.rescheduleOUT = NULL  FROM  tblWorkASCP t INNER JOIN po ON t.text1 = po.po_no AND t.number2 = po.line_no  AND isnull(t.number3,0) = isnull(po.release_no,0) AND isnull(t.number4,0) = isnull(po.shipment_no,0) ")

            dbS.NonQuery(" UPDATE PO SET rescheduleOUT = NULL  WHERE rescheduleIn IS NOT NULL AND rescheduleOut IS NOT NULL ")

            'Correct the reschedule in message 
            dbS.NonQuery(" UPDATE PO SET po.rescheduleIn = (SELECT MIN(md.mrp_date) FROM msc_demand md WHERE md.job_no = j.job_no AND md.organization_id = j.organization_id) FROM  po " & _
                        " INNER JOIN jobs j ON j.ospItem = po.ITEM_NO AND j.WIP_ENTITY_ID = po.WIP_ENTITY_ID AND j.ORGANIZATION_ID = po.SHIP_TO_ORGANIZATION_ID " & _
                        " INNER JOIN msc_demand md ON md.job_no = j.job_no AND md.organization_id = j.organization_id " & _
                        " WHERE po.ospJobNumber IS NOT NULL AND po.exceptionMessage = 'Orders to be rescheduled in' ")

            'We have reschedule in messages for purchase order promised dates being early enough
            dbS.NonQuery("UPDATE po SET po.rescheduleIn = NULL, po.exceptionMessage = NULL FROM po WHERE po.rescheduleIn > po.promised_Date AND ospJobNumber IS NOT NULL AND exceptionMessage = 'Orders to be rescheduled in'")

            'Here we are correcting reschedule in messages for osp jobs purchase orders which do have 
            dbS.NonQuery("UPDATE po SET rescheduleIn = NULL, exceptionMessage = NULL FROM  po INNER JOIN jobs j ON j.ospItem = po.ITEM_NO AND j.WIP_ENTITY_ID = po.WIP_ENTITY_ID AND j.ORGANIZATION_ID = po.SHIP_TO_ORGANIZATION_ID INNER JOIN msc_demand md ON md.job_no = j.job_no AND md.organization_id = j.organization_id " & _
                         " WHERE po.ospJobNumber Is Not NULL " & _
                         "			 AND demand_type_desc = 'Safety Stock' AND po.exceptionMessage = 'Orders to be rescheduled in' " & _
                         "			 AND (SELECT count(distinct((md.demand_type_desc))) FROM msc_demand md WHERE md.job_no = j.job_no AND md.organization_id = j.organization_id)  = 1")


            'Also update the missing job number to really always show the requesting OSP job number.
            dbS.NonQuery(" UPDATE p SET p.ospJobNumber =  j.job_no FROM jobs j INNER JOIN PO p ON j.WIP_ENTITY_ID = p.WIP_ENTITY_ID  " & _
                         " AND j.ORGANIZATION_ID = p.SHIP_TO_ORGANIZATION_ID  " & _
                         " WHERE p.ospJobNumber Is NULL ")

            ''Do the:'Additional Autobahn Exception - Expedite to safety stock!
            'dbS.NonQuery("TRUNCATE TABLE tblWorkASCP")
            'dbS.NonQuery("INSERT INTO tblWorkASCP (Date1, text1,number2,number3,number4,date2)( SELECT  d.mrp_date, s.purchase_order_no,s.purch_line_no, s.release_no,s.shipment_no, s.PO_PROMISED_DATE FROM msc_demand d INNER JOIN msc_supply s ON d.demand_id = s.demand_id AND s.organization_id = d.organization_id WHERE d.demand_priority = '9999999' AND (d.qty_onhand - (SELECT safetyStockThreshould FROM whOrganizationDefinition wod WHERE d.organization_id = wod.organizationid)*d.safety_stock_quantity) < 0 GROUP BY d.mrp_date, s.purchase_order_no,s.purch_line_no, s.release_no,s.shipment_no, s.PO_PROMISED_DATE) ")
            'dbS.NonQuery("UPDATE po SET po.exceptionMessage = 'Additional Autobahn Exception - Expedite to safety stock! ', po.rescheduleIN = t.date1   FROM  tblWorkASCP t  INNER JOIN po ON t.text1 = po.po_no  AND t.number2 = po.line_no  AND isnull(t.number3,0) = isnull(po.release_no,0)  AND isnull(t.number4,0) = isnull(po.shipment_no,0) WHERE po.exceptionMessage IS NULL ")

        Catch ex As Exception
            createErrorLog("createExceptionMessages", ex)
        Finally

        End Try
    End Sub

    ''' <summary>
    ''' Creates a logging stamp in the log table of the data warehouse.
    ''' </summary>
    ''' <param name="procedureName">The name of the procedure calling this subprogram</param>
    ''' <remarks>To see how long transformation runs and who invoked it. </remarks>
    Private Sub createLoggingStamp(ByVal procedureName As String)

        Me.IncreaseStep()

        Dim invokedBy As String = Environment.UserName

        db.SecureNonQueryParams("INSERT INTO [log] " & _
           "([procedure_name] " & _
           ",[current_step] " & _
           ",[calling_Object] " & _
           ",[invoked_by] " & _
           ",[time]) " & _
        " VALUES (@1, @2, @3, @4, getDate())", procedureName, Me.currentStep, MyBase.ToString, invokedBy)


    End Sub



    ''' <summary>
    ''' Creates an error stamp in the backend.
    ''' </summary>
    ''' <param name="procedureName"></param>
    ''' <param name="exceptionMessage"></param>
    ''' <remarks>We save the error line, the description and the object which caused the error</remarks>
    Private Sub createErrorLog(ByVal procedureName As String, ByVal exceptionMessage As Exception)
        Try
            Dim strCausingObject As String = ""
            Dim strDescription As String = ""
            Dim strStackTrace As String = ""
            Dim strSource As String = ""

            Try
                strCausingObject = MyBase.ToString.Replace("'", "")
                strDescription = exceptionMessage.Message.ToString.Replace("'", "")
                strStackTrace = exceptionMessage.StackTrace.ToString.Replace("'", "")
                strSource = exceptionMessage.Source.ToString.Replace("'", "")

            Catch ex As Exception

            End Try

            Dim invokedBy As String = Environment.UserName

            db.SecureInsertQueryParams("INSERT INTO [error_log] " & _
               "([calling_object] " & _
               ",[description] " & _
               ",[error_line] " & _
               ",[NO_OF_STEPS] " & _
               ",[PROCEDURE_OR_FUNCTION] " & _
               ",[invoked_by] " & _
               ",[CREATION_DATE] " & _
               ",[causing_object] )" & _
             " VALUES(@1, @2, @3, @4, @5, @6, getDate(), @7)", strCausingObject, strDescription, strStackTrace, _
             Me.currentStep, procedureName _
             , invokedBy, strSource)

            Dim email As New clsEMailReports("nothing")
            email.mailErrorReport(exceptionMessage.Message, exceptionMessage.StackTrace, Me.currentStep, procedureName, invokedBy, "", exceptionMessage.Source, "")

        Catch ex As Exception

            EventLog.WriteEntry(ex.Source, ex.Message, EventLogEntryType.Error)

        End Try


    End Sub

    Private Sub IncreaseStep()
        Me.currentStep = Me.currentStep + 1
    End Sub

End Class
