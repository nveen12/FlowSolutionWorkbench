Public Class frmToolKaizen

    Private Sub frmToolKaizen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DsConcernAction.whUserNotificationsView' table. You can move, or remove it, as needed.

        Dim barCode As New clsBarcodeRendering("Test")
        pbBarCode.Image = barCode.GenerateBarcodeImage(300, 140, "12345")


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.PrintForm1.Print()

    End Sub

    Private Sub btnLetItFlow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLetItFlow.Click

        Dim db As New DB.ORacleServerDB(My.Settings.OracleEDCConnectionString)
        Dim dbS As New DB.ServerDB(My.Settings.FLOWConnectionString)

        Try


            db.Connect()
            Dim ds As DataSet = db.Query("SELECT " & _
                                             "   wtv.primary_item_id " & _
                                        ", wtv.department_code " & _
                                        ", wtv.resource_code " & _
                                        ", CAST(ROUND(SUM(wtv.transaction_quantity) ,4) AS number(25,5)) transacted_hours " & _
                                        "FROM        WIP_TRANSACTIONS_V  wtv    " & _
                                        "WHERE       TRUNC(Transaction_Date)  > TO_DATE('01-JAN-2010')  " & _
                                        "AND         TRUNC(Transaction_date)  < TO_DATE('31-DEC-2011')  " & _
                                        "AND primary_uom     = 'MN' " & _
                                        "AND organization_id = 255  " & _
                                        "        GROUP(BY)  " & _
                                        "        wtv.department_code  " & _
                                        "        , wtv.resource_code    " & _
                                        "        , wtv.primary_item_id")

            'For Each i As DataRow In ds.Tables(0).Rows
            '    db.SecureNonQueryParams("INSERT INTO [FSHHMBFactoryVision].[dbo].[jobInformation] " & _
            '           "([PRIMARY_ITEM_ID] " & _
            '           ",[DEPARTMENT_CODE] " & _
            '           ",[RESOURCE_CODE] " & _
            '           ",[TRANSACTED_HOURS]) " & _
            '           "     VALUES (@1,@2,@3,@4) ", i.Item("primary_item_id"), i.Item("department_code"), _
            '           i.Item("resource_code"), i.Item("transacted_hours"))

            'Next

            ds = db.Query("SELECT  " & _
           " msi.inventory_item_id  " & _
", msi.segment1  " & _
", msi.description " & _
", (SELECT mcr.CROSS_REFERENCE FROM mtl_cross_references mcr WHERE mcr.inventory_item_id = msi.inventory_item_id AND mcr.organization_id IS NULL AND MCR.CROSS_REFERENCE_TYPE LIKE'1-DRAWING' AND rownum = 1 ) AS drawing " & _
", (SELECT mcr2.CROSS_REFERENCE FROM mtl_cross_references mcr2 WHERE mcr2.inventory_item_id = msi.inventory_item_id AND mcr2.organization_id IS NULL AND MCR2.CROSS_REFERENCE_TYPE LIKE'4-MATERIAL' AND rownum = 1 ) AS material " & _
", (SELECT mcr3.description FROM mtl_cross_references mcr3 WHERE mcr3.inventory_item_id = msi.inventory_item_id AND mcr3.organization_id IS NULL AND MCR3.CROSS_REFERENCE_TYPE LIKE'4-MATERIAL' AND rownum = 1 ) AS material_description " & _
",CAST( (SELECT CIC.item_cost   FROM cst_item_costs cic WHERE CIC.inventory_item_id       = MSI.inventory_item_id " & _
"                                                    AND CIC.organization_id         = MSI.organization_id " & _
"                                                    AND CIC.cost_type_id            = 1 AND rownum = 1 )  AS number(25,5))  frozen_cost " & _
",CAST( (SELECT CIC.item_cost   FROM cst_item_costs cic WHERE CIC.inventory_item_id       = MSI.inventory_item_id " & _
"                                                    AND CIC.organization_id         = MSI.organization_id " & _
"                                                    AND CIC.cost_type_id            = 3 AND rownum = 1 ) AS number(25,5)) pending_costs  " & _
",CAST( (SELECT CIC.pl_material_overhead    FROM cst_item_costs cic WHERE CIC.inventory_item_id    = MSI.inventory_item_id " & _
"                                                    AND CIC.organization_id         = MSI.organization_id " & _
"                                                    AND CIC.cost_type_id            = 1 AND rownum = 1 ) AS number(25,5)) frozen_material_overhead  " & _
", CAST((SELECT CIC.resource_cost   FROM cst_item_costs cic WHERE CIC.inventory_item_id     = MSI.inventory_item_id " & _
"                                                    AND CIC.organization_id         = MSI.organization_id " & _
"                                                    AND CIC.cost_type_id            = 1 AND rownum = 1 ) AS number(25,5)) frozen_resource " & _
",CAST( (SELECT CIC.outside_processing_cost  FROM cst_item_costs cic WHERE CIC.inventory_item_id     = MSI.inventory_item_id " & _
"                                                    AND CIC.organization_id         = MSI.organization_id " & _
"                                                    AND CIC.cost_type_id            = 1 AND rownum = 1 ) AS number(25,5)) frozen_outside_processing " & _
",CAST( (SELECT CIC.overhead_cost   FROM cst_item_costs cic WHERE CIC.inventory_item_id     = MSI.inventory_item_id " & _
"                                                    AND CIC.organization_id         = MSI.organization_id " & _
"                                                    AND CIC.cost_type_id            = 1 AND rownum = 1 ) AS number(25,5))   frozen_overhead         " & _
", gcc.segment3 as Department " & _
", gcc.segment4 as ProductLine " & _
", gcc.segment6 as Breakdown " & _
", gcc.segment7 as Country " & _
", gcc.segment8 as Market     " & _
"FROM mtl_system_items_b       msi " & _
", gl_code_combinations      	gcc   " & _
"            WHERE msi.organization_id = 255 " & _
"AND   msi.sales_account         	(+)  = gcc.code_combination_id")





        Catch ex As Exception

        Finally
            db.Disconnect()

        End Try


    End Sub
End Class