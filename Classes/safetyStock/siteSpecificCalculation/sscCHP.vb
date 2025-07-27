


Public Class sscCHP

    'Mehtods from the overall safety stock program
    Inherits safetyStockCalculation

    Private dbs As New DB.ServerDB(My.Settings("FlowConnectionString"))


    ''' <summary>
    ''' Overall function which calculates safety stock for chesapeake.
    ''' </summary>
    ''' <param name="organizationID">The current organization id of Chesapeake.</param>
    ''' <returns>True if no problem occured.</returns>
    ''' <remarks></remarks>
    Public Function calculateSafetyStock(ByVal organizationID As Integer)
        Try
            Dim dblQuantity As Double = 0
            Me.calculateMonthlySupply(2.5, 861, 10077998)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    'Implement subprocedures here...


    'temporary - put in PM Workbench!
    Public Sub CalculateSafetyStock(ByVal lngOrg As Long)

        Dim lngPeriod(0 To 3) As Long
        Dim sSql As String
        Dim sTmp As String

        Dim dblSafetyFactor(0 To 9999) As Double     'this belongs in the Org setup table. remove from here. ***
        dblSafetyFactor(lngOrg) = 1.15  'we need a value like this for each inv org.  it is the amount to inflate the safety stock value to cover slippage

        'set the number of months history to consider, for each usage summary column.  poor design, these numbers are hardcoded into the column names *** fix later
        lngPeriod(0) = 3
        lngPeriod(1) = 6
        lngPeriod(2) = 12
        lngPeriod(3) = 24

        ''enhance the USAGE table with the org info we need
        sSql = "UPDATE u SET organizationID = o.ORGANIZATION_ID " & _
         "FROM dbo.USAGE u  INNER JOIN dbo.tblOrgs o  ON u.warehouse = o.legacyWarehouse"
        dbs.NonQuery(sSql)

        'update items table with usages
        sSql = "UPDATE i SET i.soSalesQty12Month = 0, i.soSalesCnt12Month = 0, i.SAFETY_STOCK_QUANTITY = 0 FROM dbo.Items AS i WHERE i.Organization_ID = " & lngOrg 'clear stats from prior update
        dbs.NonQuery(sSql)

        For x = 0 To UBound(lngPeriod)
            'clear all usage stats
            sSql = "UPDATE i SET i.Usage" & lngPeriod(x) & "Month = 0, i.TrxCnt" & lngPeriod(x) & "Month = 0  FROM dbo.Items AS i WHERE i.Organization_ID = " & lngOrg
            dbs.NonQuery(sSql)

            'use the temporary table to post the sum's (insert into), then copy to items table
            dbs.NonQuery("DELETE FROM tblWork")
            '                                             org id, part number, sales   wip use qty  sales cnt  wip cnt
            sSql = "INSERT INTO tblWork (Number1, Text1, Number2, Number3, Number4, Number5) " & _
             "SELECT organizationId, itemNo, SUM(soSalesQty) As Expr1, SUM(wipUseQty) AS Expr2, SUM(soSalesCnt) As Expr3, SUM(wipUseCnt) AS Expr4 " & _
             "FROM USAGE WHERE (date >= { fn NOW() } - " & lngPeriod(x) * 30 & ")  AND organizationID = " & lngOrg & "  GROUP BY organizationId, itemNo "   '& example date range
            dbs.NonQuery(sSql)

            'copy the usage into the Items table
            sSql = "UPDATE i SET i.Usage" & lngPeriod(x) & "Month = -(w.Number2 + w.Number3), i.TrxCnt" & lngPeriod(x) & "Month = (w.Number4 + w.Number5) FROM dbo.Items AS i INNER JOIN dbo.tblWork AS w ON w.Number1 = i.Organization_ID " & _
              "AND w.Text1 = i.Item_No AND i.Organization_ID = " & lngOrg
            dbs.NonQuery(sSql)

            If lngPeriod(x) = 12 Then 'separate sales from wip at 1 year
                sSql = "UPDATE i SET i.soSalesQty12Month = w.Number2, i.soSalesCnt12Month = w.Number4 FROM dbo.Items AS i INNER JOIN dbo.tblWork AS w ON w.Number1 = i.Organization_ID " & _
                 "AND w.Text1 = i.Item_No AND i.Organization_ID = " & lngOrg
                dbs.NonQuery(sSql)
            End If
        Next x

        'reset safety stock related values 
        sSql = "UPDATE i SET manualSafetyStock = NULL, keepInStock = 0, suggSafetyStock = 0, suggUpdate = 0 FROM dbo.Items AS i WHERE ORGANIZATION_ID = " & lngOrg  'clear all suggested safety stocks
        dbs.NonQuery(sSql)
        sSql = "UPDATE i SET SAFETY_STOCK_QUANTITY = s.SAFETY_STOCK_QUANTITY FROM dbo.Items AS i INNER JOIN dbo.itemSafetyStocks AS s ON i.ORGANIZATION_ID = s.ORGANIZATION_ID AND i.Item_No = s.Item_No  WHERE  i.ORGANIZATION_ID = " & lngOrg 'copy existing safety stocks
        dbs.NonQuery(sSql)
        sSql = "UPDATE i SET manualSafetyStock = e.manualSafetyStock FROM dbo.Items as i INNER JOIN dbo.tblExcludedItems AS e ON i.ORGANIZATION_ID = e.ORGANIZATION_ID AND i.INVENTORY_ITEM_ID = e.INVENTORY_ITEM_ID "  'set manual stocks
        dbs.NonQuery(sSql)

        'Apply "Do Stock" CHP business rules
        sTmp = " AND PLANNING_MAKE_BUY_CODE = 2  AND manualSafetyStock IS NULL AND ORGANIZATION_ID = " & lngOrg 'common selection criteria
        sSql = "UPDATE i SET keepInStock = 1 FROM dbo.Items AS i WHERE PLANNER_CODE IN ('FD%', 'FP%', 'FXB', 'PLF') AND Usage12Month >= 6 " & sTmp 'stock purchased castings w/usage of 6pc per year or more
        dbs.NonQuery(sSql)
        sSql = "UPDATE i SET keepInStock = 1 FROM dbo.Items AS i WHERE ((Usage12Month >= 6 and TrxCnt12Month >= 4) OR Usage12Month >=15) " & sTmp   'stock all other purchased parts with usage of 15pc or 4 hits/6pc usage
        dbs.NonQuery(sSql)
        sSql = "UPDATE i SET keepInStock = 1 FROM dbo.Items AS i WHERE Usage12Month > 1 AND TrxCnt12Month >= 1 AND FIXED_LEAD_TIME > 56 AND ITEM_COST < 250 " & sTmp    'stock long lead time, low volume castings
        dbs.NonQuery(sSql)

        'Apply "Do Not Stock"  CHP business rules 
        sSql = "UPDATE i SET keepInStock = 0  FROM dbo.Items AS i WHERE (PLANNER_CODE LIKE 'ZX%' OR PLANNER_CODE LIKE '14%') " & sTmp    'don't stock planner ZX, 14 parts
        dbs.NonQuery(sSql)
        sSql = "UPDATE i SET keepInStock = 0  FROM dbo.Items AS i WHERE (DESCRIPTION LIKE 'BP %' OR DESCRIPTION LIKE 'BASEPLATE%') " & sTmp  'don't stock baseplates
        dbs.NonQuery(sSql)

        'set the safety stock quantity to "Usage thru lead time," plus fudge factor (15% for chpk sites).  Min 1 pc.
        sSql = "UPDATE i SET suggSafetyStock = CAST(.5 + Usage12Month * FIXED_LEAD_TIME / 365 * " & Str(dblSafetyFactor(lngOrg)) & " as INT) FROM dbo.Items AS i WHERE keepInStock = 1 AND ORGANIZATION_ID = " & lngOrg
        dbs.NonQuery(sSql)


        '*** put some maximum limit on safety stock qty based on dollar value


        'Round suggested safety stocks.  trying this new method of writing, so that can update specific record, one at a time.  this requires the table to have a key
        Dim conn As New SqlClient.SqlConnection(My.Settings("TrainerConnectionString"))
        Dim adp As New SqlClient.SqlDataAdapter("SELECT * FROM Items WHERE suggSafetyStock > 0 AND ORGANIZATION_ID = " & lngOrg, conn)
        Dim cb As New SqlClient.SqlCommandBuilder(adp)  'The Command builder will generate the Insert, Update & Delete commands for you based on your Select command in your Adapter.
        Dim dt As New DataTable

        adp.Fill(dt)    'Fills the datatable with the data from your query. The adapter will open & close the database connection for you automagically.

        ' If the datatable has any rows, then process them
        If dt.Rows.Count > 0 Then
            For Each Row As DataRow In dt.Rows
                Row("suggSafetyStock") = SuperRound(Row("suggSafetyStock"))  'Update the field in the row
            Next
            adp.Update(dt)   ' Send the changes made in the datatable back to the database.  this will not work unless the table has a primary key defined.  for items, I used inventory_item_id and organization_id
        End If
        conn.Dispose()
        adp.Dispose()
        dt.Dispose()

        'damping: indicate if the suggested change is significant 
        sSql = "UPDATE i SET suggUpdate = 1 FROM dbo.Items AS i WHERE ABS((suggSafetyStock - SAFETY_STOCK_QUANTITY)) / (.00001 + SAFETY_STOCK_QUANTITY) > " & (dblSafetyFactor(lngOrg) - 1) / 2 & " AND ORGANIZATION_ID = " & lngOrg
        dbs.NonQuery(sSql)


    End Sub



    'rounding function  surely a clever person could simplify this.  perhaps a case statement in the sql could replace this function
    Public Function SuperRound(ByVal lngNumber As Long)

        If lngNumber > 4000 Then
            lngNumber = Math.Round(lngNumber / 1000, 0) * 1000
        Else
            If lngNumber > 2000 Then
                lngNumber = Math.Round(lngNumber / 500, 0) * 500
            Else
                If lngNumber > 400 Then
                    lngNumber = Math.Round(lngNumber / 100, 0) * 100
                Else
                    If lngNumber > 200 Then
                        lngNumber = Math.Round(lngNumber / 50, 0) * 50
                    Else
                        If lngNumber > 40 Then
                            lngNumber = Math.Round(lngNumber / 10, 0) * 10
                        Else
                            If lngNumber > 20 Then lngNumber = Math.Round(lngNumber / 5, 0) * 5
                        End If
                    End If
                End If
            End If
        End If

        SuperRound = lngNumber

    End Function

End Class
