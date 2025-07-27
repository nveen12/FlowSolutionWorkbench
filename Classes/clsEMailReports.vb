Imports System.Net.Mail
Imports System.IO
Imports Microsoft.Reporting.WinForms
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Public Class clsEMailReports
    Private dbs As New DB.ServerDB(My.Settings.FLOWConnectionString)
    Private dboEDC As New DB.ORacleServerDB(My.Settings.OracleEDCConnectionString)
    Private eMailAdressFrom As String = "info@pleugerindustries.com"
    Private eMailServerName As String = "smtp.office365.com"

    ''' <summary>
    ''' Standard contructor of the object, which creates the emails.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        'Me.mailOffsiteInvReport()
        ' Me.mailSalesOrderBacklog("(881,883)")
        Me.ASCPmailOffsiteInvReport()

    End Sub

    Public Sub New(ByVal orgid As String)

        If orgid = "Hamburg" Then
            ' Me.mailBookingAndBillings()
            Me.mailOTP(0)
        ElseIf orgid = "HamburgShop" Then
            Me.mailShopFloorPriorization()
        ElseIf orgid = "Newark" Then
            Me.mailShopFloorPriorizationNWK()
        ElseIf orgid = "ASCPCHPOffsiteAndBacklog" Then
            Me.ASCPmailOffsiteInvReport()
            Me.ASCPmailSalesOrderBacklog("(881,883)")
        ElseIf orgid = "CHPDailyReschedules" Then
            Me.DailyReschedulingReport()
        ElseIf orgid = "Hamburg OTP Real" Then
            Me.mailOTP(0)
        ElseIf orgid = "HamburgPhantoms" Then
            Me.mailPhantomAlert()
        ElseIf orgid = "Hamburg Export Dual Use" Then
            Me.mailExportComplianceList()
        End If

    End Sub

    Public Sub mailExportComplianceList()
        Dim spath As String = ""
        Try

            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                        " FROM [warehouseUserReports] WHERE reportID = @1 ", 14)


            Dim sBody As String = Me.createBodyDualUse("11")

            Me.SendMail("Autobahn@ingravure.org", ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                       "", My.Settings.smtpMailhost, True)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailPhantomAlert", ex, 1, spath)
        End Try
    End Sub

    Public Function createBodyDualUse(ByVal reportID As String) As String
        Try

            Dim sBody As String = ""
            Dim ds As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                            " FROM [warehouseUserReports] WHERE reportID = @1 ", reportID)

            'This is just the header information

            sBody = "<table width='500' bgcolor='#6699ff'> " &
                    "    <tr> " &
                    "        <td class='style4' colspan='2' bgcolor='White'> " &
                    " Business Cockpit - Auto E-Mail Report " &
                    "    </td> " &
                    "    </tr> " &
                    "    <tr> " &
                    "        <td class='style4' > " &
                    " Description</td> " &
                    "       <td class='style3'> " &
                    " " & ds.Tables(0).Rows(0).Item("description") & " </td> " &
                    "    </tr> " &
                    "    <tr bgcolor='White'> " &
                    "        <td class='style4'> " &
                    " Key Points</td> " &
                    "        <td class='style3'> " &
                    " " & ds.Tables(0).Rows(0).Item("keyPoints") & "</td> " &
                    "    </tr> " &
                    "    <tr>  " &
                    "        <td class='style4'> " &
                    "  Purpose</td> " &
                    "        <td class='style3'> " &
                    " " & ds.Tables(0).Rows(0).Item("purpose") & "</td> " &
                    "    </tr> " &
                    "    <tr bgcolor='White'> " &
                    "        <td class='style4'> " &
                    " Date</td> " &
                    "        <td class='style3'> " &
                    " " & FormatDateTime(Now, DateFormat.ShortDate) & "</td> " &
                    "    </tr> " &
                    "    <tr> " &
                    "        <td class='style4'> " &
                    " Schedule</td> " &
                    "        <td class='style3'> " &
                    " " & ds.Tables(0).Rows(0).Item("schedule") & "</td> " &
                    "    </tr> " &
                    " </table> <br><p></p><hr></hr>"

            Dim dsCases As DataSet = dboEDC.Query("SELECT " &
                         "   COHDR.order_number order_no   " &
                   " ,COLINES.line_number  line_no " &
                   " ,NVL(PARTY.party_name,' ') party_name  " &
                   " ,TRUNC(COHDR.ordered_date) AS ordered_date " &
                   " ,COLINES.ordered_item item_no " &
                   " ,COLINES.USER_ITEM_DESCRIPTION description " &
                   " ,COLINES.ordered_quantity   " &
                   " ,(SELECT " &
                   "         MIN(MCR1.CROSS_REFERENCE) " &
                   "         FROM  " &
                   "         apps.MTL_CROSS_REFERENCES_B MCR1 " &
                   "         WHERE " &
                   "         msi.INVENTORY_ITEM_ID = MCR1.INVENTORY_ITEM_ID " &
                   " AND  MCR1.ORGANIZATION_ID      IS NULL  " &
                   " AND ( MCR1.last_update_date > sysdate - 1.5 OR MCR1.CROSS_REFERENCE_TYPE LIKE'4-MATERIAL')) Material_1 " &
                   " ,(SELECT " &
                   "         MIN(MCR1.DESCRIPTION) " &
                   "         FROM " &
                   "         apps.MTL_CROSS_REFERENCES_B(MCR1) " &
                   "         WHERE " &
                   "         msi.INVENTORY_ITEM_ID = MCR1.INVENTORY_ITEM_ID " &
                   " AND  MCR1.ORGANIZATION_ID      IS NULL  " &
                   " AND ( MCR1.last_update_date > sysdate - 1.5 OR MCR1.CROSS_REFERENCE_TYPE LIKE'4-MATERIAL')) Material_DESC_1 " &
                   " ,(SELECT " &
                   "         MAX(MCR1.CROSS_REFERENCE) " &
                   "         FROM " &
                   "         apps.MTL_CROSS_REFERENCES_B(MCR1) " &
                   "         WHERE " &
                   "         msi.INVENTORY_ITEM_ID = MCR1.INVENTORY_ITEM_ID " &
                   " AND  MCR1.ORGANIZATION_ID      IS NULL  " &
                   " AND ( MCR1.last_update_date > sysdate - 1.5 OR MCR1.CROSS_REFERENCE_TYPE LIKE'4-MATERIAL')) Material_2 " &
                   " ,(SELECT " &
                   "         MAX(MCR1.DESCRIPTION) " &
                   "         FROM " &
                   "         apps.MTL_CROSS_REFERENCES_B(MCR1) " &
                   "         WHERE " &
                   "         msi.INVENTORY_ITEM_ID = MCR1.INVENTORY_ITEM_ID " &
                   " AND  MCR1.ORGANIZATION_ID      IS NULL  " &
                   " AND ( MCR1.last_update_date > sysdate - 1.5 OR MCR1.CROSS_REFERENCE_TYPE LIKE'4-MATERIAL')) Material_DESC_2  " &
                   " , (SELECT ter.territory_code ship_country_code   " &
                   "         FROM " &
                   "         HZ_CUST_ACCOUNTS CUST  " &
                   "         ,HZ_PARTIES                                      PARTY " &
                   "         ,   hz_cust_site_uses_all                       b " &
                   "         ,   hz_cust_acct_sites_all                      c " &
                   "         ,   hz_party_sites                              hps " &
                   "         ,   hz_locations                                hl " &
                   "         ,   fnd_territories                             ter   " &
                   "         WHERE " &
                   "         CUST.cust_account_id = COHDR.sold_to_org_id " &
                   "         and COHDR.ship_to_org_id                      = b.site_use_id  " &
                   "         and b.cust_acct_site_id                     = c.cust_acct_site_id  " &
                   "         and b.cust_acct_site_id                     = c.cust_acct_site_id  " &
                   "         and c.party_site_id                         = hps.party_site_id " &
                   "         and hps.location_id                         = hl.location_id " &
                   "         AND hl.country                              = ter.territory_code(+) " &
                   "         AND CUST.party_id           = PARTY.party_id   )          territoryCode  " &
                   "         FROM " &
                   "         OE_ORDER_HEADERS_ALL COHDR         " &
                   " ,OE_ORDER_LINES_ALL         COLINES        " &
                   " ,HZ_CUST_ACCOUNTS           CUST           " &
                   " ,HZ_PARTIES                 PARTY          " &
                   " ,OE_TRANSACTION_TYPES_ALL   OT             " &
                   " ,OE_TRANSACTION_TYPES_TL    OTL            " &
                   " ,RA_TERMS_TL                RTT            " &
                   " , mtl_system_items_b        msi            " &
                   "         WHERE                              " &
                   "         COLINES.header_id = COHDR.header_id                    " &
                   " AND OT.transaction_type_id  = COHDR.order_type_id              " &
                   " AND OTL.transaction_type_id = OT.transaction_type_id           " &
                   " AND OTL.language            = 'US'                             " &
                   " AND CUST.cust_account_id    = COHDR.sold_to_org_id             " &
                   " AND CUST.party_id           = PARTY.party_id                   " &
                   " AND msi.inventory_item_id   = COLINES.inventory_item_id        " &
                   " AND  COHDR.ship_from_org_id = msi.organization_id              " &
                   " AND COLINES.ordered_quantity > NVL(COLINES.shipped_quantity,0)  " &
                   " AND COLINES.open_flag       <> 'N'                             " &
                   " AND COLINES.item_type_code  = 'STANDARD'                       " &
                   " AND OT.attribute1           <> 'PROPOSAL'                      " &
                   " AND RTT.term_id            (+)= COLINES.payment_term_id        " &
                   " AND RTT.language              = 'US'                           " &
                   " AND COLINES.flow_status_code NOT IN ('DRAFT','LOST')       " &
                   " AND COHDR.ship_from_org_id       = 255 ")




            Return sBody
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("createBodyDailyPhantomList", ex, 1)
        End Try

    End Function

    Public Sub mailPhantomAlert()
        Dim spath As String = ""
        Try

            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                                                        " FROM [warehouseUserReports] WHERE reportID = @1 ", 12)


            Dim sBody As String = Me.createBodyDailyPhantomList("12")

            Me.SendMail(Me.eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                       "", Me.eMailServerName, True)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailPhantomAlert", ex, 1, spath)
        End Try
    End Sub

    Private Function createBodyDailyPhantomList(ByVal reportID As String) As String

        Dim sBody As String = ""
        Dim ds As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                " FROM [warehouseUserReports] WHERE reportID = @1 ", reportID)
        Try



            'This is just the header information
            sBody = "<table width='500' bgcolor='#6699ff'> " &
                    "    <tr> " &
                    "        <td class='style4' colspan='2' bgcolor='White'> " &
                    " Business Cockpit - Auto E-Mail Report " &
                    "    </td> " &
                    "    </tr> " &
                    "    <tr> " &
                    "        <td class='style4' > " &
                    " Description</td> " &
                    "       <td class='style3'> " &
                    " " & ds.Tables(0).Rows(0).Item("description") & " </td> " &
                    "    </tr> " &
                    "    <tr bgcolor='White'> " &
                    "        <td class='style4'> " &
                    " Key Points</td> " &
                    "        <td class='style3'> " &
                    " " & ds.Tables(0).Rows(0).Item("keyPoints") & "</td> " &
                    "    </tr> " &
                    "    <tr>  " &
                    "        <td class='style4'> " &
                    "  Purpose</td> " &
                    "        <td class='style3'> " &
                    " " & ds.Tables(0).Rows(0).Item("purpose") & "</td> " &
                    "    </tr> " &
                    "    <tr bgcolor='White'> " &
                    "        <td class='style4'> " &
                    " Date</td> " &
                    "        <td class='style3'> " &
                    " " & FormatDateTime(Now, DateFormat.ShortDate) & "</td> " &
                    "    </tr> " &
                    "    <tr> " &
                    "        <td class='style4'> " &
                    " Schedule</td> " &
                    "        <td class='style3'> " &
                    " " & ds.Tables(0).Rows(0).Item("schedule") & "</td> " &
                    "    </tr> " &
                    " </table> <br>"

            Dim dsCases As DataSet = dboEDC.Query("SELECT                                   " &
                                                   " COHDR.order_number order_no            " &
                                                   " ,COLINES.line_number  line_no          " &
                                                   " ,NVL(PARTY.party_name,' ') party_name  " &
                                                   " ,(SELECT item_type FROM mtl_system_items_b msi WHERE msi.inventory_item_id = COLINES.inventory_item_id AND msi.organization_id = COHDR.ship_from_org_id) item_type " &
                                                   " ,COLINES.ordered_item item_no          " &
                                                   " ,COLINES.USER_ITEM_DESCRIPTION description  " &
                                                   " ,COLINES.ordered_quantity              " &
                                                   "     FROM                               " &
                                                   "     OE_ORDER_HEADERS_ALL       COHDR   " &
                                                   " ,OE_ORDER_LINES_ALL            COLINES " &
                                                   " ,HZ_CUST_ACCOUNTS              CUST    " &
                                                   " ,HZ_PARTIES                    PARTY   " &
                                                   " ,OE_TRANSACTION_TYPES_ALL      OT      " &
                                                   " ,OE_TRANSACTION_TYPES_TL       OTL     " &
                                                   "     WHERE                              " &
                                                   "     COLINES.header_id = COHDR.header_id " &
                                                   " AND OT.transaction_type_id  = COHDR.order_type_id " &
                                                   " AND OTL.transaction_type_id = OT.transaction_type_id " &
                                                   " AND OTL.language            = 'US' " &
                                                   " AND CUST.cust_account_id    = COHDR.sold_to_org_id   " &
                                                   " AND CUST.party_id           = PARTY.party_id " &
                                                   " AND COLINES.ordered_quantity > NVL(COLINES.shipped_quantity,0)   " &
                                                   " AND COLINES.open_flag       <> 'N' " &
                                                   " AND COLINES.item_type_code  = 'STANDARD' " &
                                                   " AND OT.attribute1           <> 'PROPOSAL' " &
                                                   " AND COLINES.flow_status_code NOT IN ('DRAFT','LOST') " &
                                                   " AND COHDR.ship_from_org_id = 255 " &
                                                   " AND (SELECT msi.segment1 FROM mtl_system_items_b msi WHERE msi.inventory_item_id = COLINES.inventory_item_id AND msi.organization_id = COHDR.ship_from_org_id) NOT IN ('1H-FV','1H-D042', '1H-D941')  " &
                                                   " AND ((SELECT item_type FROM mtl_system_items_b msi WHERE msi.inventory_item_id = COLINES.inventory_item_id AND msi.organization_id = COHDR.ship_from_org_id) = 'H-PHA'" &
                                                   " OR (SELECT wip_supply_type FROM mtl_system_items_b msi WHERE msi.inventory_item_id = COLINES.inventory_item_id AND msi.organization_id = COHDR.ship_from_org_id) =6) ")

            sBody += "<br>"
            sBody += "<table>"

            sBody += "<tr>"

            sBody += "<td>"
            sBody += "Auftragsnummer"
            sBody += "</td>"

            sBody += "<td>"
            sBody += "Auftragszeile"
            sBody += "</td>"

            sBody += "<td>"
            sBody += "Kunde"
            sBody += "</td>"

            sBody += "<td>"
            sBody += "Teiletyp"
            sBody += "</td>"

            sBody += "<td>"
            sBody += "Teilenummer"
            sBody += "</td>"


            sBody += "<td>"
            sBody += "Bestellte Menge"
            sBody += "</td>"


            sBody += "</tr>"

            For Each row As DataRow In dsCases.Tables(0).Rows
                sBody += "<tr>"

                sBody += "<td>"
                sBody += row.Item("order_no").ToString
                sBody += "</td>"

                sBody += "<td>"
                sBody += row.Item("line_no").ToString
                sBody += "</td>"

                sBody += "<td>"
                sBody += row.Item("party_name").ToString
                sBody += "</td>"

                sBody += "<td>"
                sBody += row.Item("item_type").ToString
                sBody += "</td>"

                sBody += "<td>"
                sBody += row.Item("item_no").ToString
                sBody += "</td>"


                sBody += "<td>"
                sBody += row.Item("ordered_quantity").ToString
                sBody += "</td>"


                sBody += "</tr>"
            Next

            sBody += "</table>"

            Return sBody
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("createBodyDailyPhantomList", ex, 1)
        End Try

    End Function

    ''' <summary>
    ''' This creates an HTML which we do send to the site to understand the re-scheduling activities
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DailyReschedulingReport()
        Dim spath As String = ""
        Try

            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                        " FROM [warehouseUserReports] WHERE reportID = @1 ", 11)


            Dim sBody As String = Me.createBodyDailyReschedulingReport("11", "881")

            Me.SendMail(Me.eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                       "", Me.eMailServerName, True)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailBookingAndBillings", ex, 1, spath)
        End Try
    End Sub

    Private Function createBodyDailyReschedulingReport(ByVal reportID As String, ByVal organizationID As String) As String

        Dim sBody As String = ""
        Dim ds As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                " FROM [warehouseUserReports] WHERE reportID = @1 ", reportID)

        'This is just the header information
        sBody = "<table width='500' bgcolor='#6699ff'> " &
                "    <tr> " &
                "        <td class='style4' colspan='2' bgcolor='White'> " &
                " Business Cockpit - Auto E-Mail Report " &
                "    </td> " &
                "    </tr> " &
                "    <tr> " &
                "        <td class='style4' > " &
                " Description</td> " &
                "       <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("description") & " </td> " &
                "    </tr> " &
                "    <tr bgcolor='White'> " &
                "        <td class='style4'> " &
                " Key Points</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("keyPoints") & "</td> " &
                "    </tr> " &
                "    <tr>  " &
                "        <td class='style4'> " &
                "  Purpose</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("purpose") & "</td> " &
                "    </tr> " &
                "    <tr bgcolor='White'> " &
                "        <td class='style4'> " &
                " Date</td> " &
                "        <td class='style3'> " &
                " " & FormatDateTime(Now, DateFormat.ShortDate) & "</td> " &
                "    </tr> " &
                "    <tr> " &
                "        <td class='style4'> " &
                " Schedule</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("schedule") & "</td> " &
                "    </tr> " &
                " </table> <br>"

        'Now we do understand some trending
        Dim dsTrendNumbers As DataSet = dbs.SecureQueryParams("SELECT count(*) numberOfComments, datepart(year,serverCreationDate) year, datepart(week, serverCreationDate) week " &
            " FROM comments " &
            " WHERE organizationID = " & organizationID &
            " AND forecastDAte IS NOT NULL " &
            " AND serverCreationDate > dateadd(day,-70,getDate()) " &
            " GROUP BY datepart(year,serverCreationDate), datepart(week, serverCreationDate) " &
            " ORDER BY datepart(year,serverCreationDate),  datepart(week, serverCreationDate)")

        Dim sTrend As String
        sTrend = "<h2>Development of the last 70 days in weekly buckets</h2><p> <table width='1000' bgcolor='#6699ff' > " &
               "    <tr bgcolor='white'> <td>Time Slot (Year-Week):</td>"
        Dim skipFirst As Integer = 0
        For Each datrow As DataRow In dsTrendNumbers.Tables(0).Rows
            If skipFirst > 0 Then
                sTrend += "<td>" + datrow.Item("year").ToString + " - " + datrow.Item("week").ToString + "</td>"
            End If

            skipFirst += 1

        Next

        sTrend += "</tr> <tr><td>No of new build dates:</td>"
        skipFirst = 0
        For Each datrow As DataRow In dsTrendNumbers.Tables(0).Rows
            If skipFirst > 0 Then
                sTrend += "<td>" + datrow.Item("numberOfComments").ToString + "</td>"
            End If

            skipFirst += 1
        Next
        sTrend += "</tr></table>"

        'Here we do understand some details
        Dim sDetails As String = "<h2>Re-Schedules of the last 24 hours into the horizon of 5 days before the request date</h2><p>  <table width='1000' bgcolor='#6699ff'>"

        'Create the header lines
        sDetails += "<tr bgcolor = 'white'><td>Customer Name</td> <td>Order No</td><td>Line No</td> <td>Item No</td> <td>Description</td> <td>Extended Price</td> <td>Last Build Date</td> <td>Request Date</td> <td>Creation date of build date</td> </tr>"

        Dim dsDetails As DataSet = dbs.Query(" SELECT so.party_name, so.order_no, so.line_no, so.item_no,so.description, so.extended_Price " &
                                             " ,CAST(CONVERT(varchar, CAST(so.extended_Price  AS money), 1) AS varchar) extended " &
                                             " , so.lastForecast, so.request_Date   " &
                                             " ,(SELECT TOP (1) serverCreationDate " &
                                             "                              FROM            dbo.comments AS comm " &
                                             "                              WHERE        (forecast = 1) AND (ENTITY_ID = 'salesOrderLinesView') AND (so.HEADER_ID = HEADER_ID) AND (so.LINE_NO = LINE_ID) " &
                                             "                              ORDER BY CommentID DESC) creationDateForecast " &
                                             " FROM mscSalesOrderLinesView so " &
                                             "       WHERE so.lastForecast > DateAdd(Day, -5, so.request_Date) " &
                                             " AND so.organization_ID = " & organizationID &
                                             " AND (SELECT TOP (1) serverCreationDate " &
                                             "                              FROM            dbo.comments AS comm " &
                                             "                              WHERE        (forecast = 1) AND (ENTITY_ID = 'salesOrderLinesView') AND (so.HEADER_ID = HEADER_ID) AND (so.LINE_NO = LINE_ID) " &
                                             "                              ORDER BY CommentID DESC) > dateadd(day,-1,getDate()) " &
                                             "	ORDER BY so.party_name")
        Dim everySecondRow As Integer = 0
        For Each datRow As DataRow In dsDetails.Tables(0).Rows
            Dim rowFormat As String = ""

            If everySecondRow Mod 2 = 0 Then
                rowFormat = "bgcolor = 'white'"
            End If

            sDetails += "<tr " & rowFormat & "><td> " & datRow.Item("party_name").ToString & " </td>" &
            "<td> " & datRow.Item("order_no").ToString & " </td>" &
            "<td> " & datRow.Item("line_no").ToString & " </td>" &
            "<td> " & datRow.Item("item_no").ToString & " </td>" &
            "<td> " & datRow.Item("description").ToString & " </td>" &
            "<td> " & datRow.Item("extended").ToString & " </td>" &
            "<td> " & FormatDateTime(datRow.Item("lastForecast"), DateFormat.ShortDate).ToString & " </td>" &
            "<td> " & FormatDateTime(datRow.Item("request_Date"), DateFormat.ShortDate).ToString & " </td>" &
            "<td> " & FormatDateTime(datRow.Item("creationDateForecast"), DateFormat.ShortDate).ToString & " </td>" &
                          "</tr>"
            everySecondRow += 1

        Next

        sDetails += "</table>"

        dsDetails = dbs.Query(" SELECT so.party_name, so.order_no, so.line_no, so.item_no,so.description, so.extended_Price " &
                                             " ,CAST(CONVERT(varchar, CAST(so.extended_Price  AS money), 1) AS varchar) extended " &
                                             " , so.lastForecast, so.request_Date   " &
                                             " ,(SELECT TOP (1) serverCreationDate " &
                                             "                              FROM            dbo.comments AS comm " &
                                             "                              WHERE        (forecast = 1) AND (ENTITY_ID = 'salesOrderLinesView') AND (so.HEADER_ID = HEADER_ID) AND (so.LINE_NO = LINE_ID) " &
                                             "                              ORDER BY CommentID DESC) creationDateForecast " &
                                             " FROM mscSalesOrderLinesView so " &
                                             "       WHERE so.lastForecast > DateAdd(Day, -5, so.request_Date) " &
                                             " AND so.organization_ID = " & organizationID &
                                             " AND (SELECT TOP (1) serverCreationDate " &
                                             "                              FROM            dbo.comments AS comm " &
                                             "                              ORDER BY CommentID DESC) > dateadd(day,-1,getDate()) " &
                                             "	ORDER BY so.party_name")


        sDetails += "<h2>Re-Schedules - Complete list </h2><p>  <table width='1000' bgcolor='#6699ff'>"

        'Create the header lines
        sDetails += "<tr bgcolor = 'white'><td>Customer Name</td> <td>Order No</td><td>Line No</td> <td>Item No</td> <td>Description</td> <td>Extended Price</td> <td>Last Build Date</td> <td>Request Date</td> <td>Creation date of build date</td> </tr>"

        For Each datRow As DataRow In dsDetails.Tables(0).Rows
            Dim rowFormat As String = ""

            If everySecondRow Mod 2 = 0 Then
                rowFormat = "bgcolor = 'white'"
            End If

            sDetails += "<tr " & rowFormat & "><td> " & datRow.Item("party_name").ToString & " </td>" &
            "<td> " & datRow.Item("order_no").ToString & " </td>" &
            "<td> " & datRow.Item("line_no").ToString & " </td>" &
            "<td> " & datRow.Item("item_no").ToString & " </td>" &
            "<td> " & datRow.Item("description").ToString & " </td>" &
            "<td> " & datRow.Item("extended").ToString & " </td>" &
            "<td> " & FormatDateTime(datRow.Item("lastForecast"), DateFormat.ShortDate).ToString & " </td>" &
            "<td> " & FormatDateTime(datRow.Item("request_Date"), DateFormat.ShortDate).ToString & " </td>" &
            "<td> " & FormatDateTime(datRow.Item("creationDateForecast"), DateFormat.ShortDate).ToString & " </td>" &
                          "</tr>"
            everySecondRow += 1

        Next


        sDetails += "</table>"

        Return sBody + sTrend + sDetails

    End Function

    ''' <summary>
    ''' Send eMail to the managers with the daily values.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub mailBookingAndBillings()
        Dim spath As String = ""
        Try

            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                        " FROM [warehouseUserReports] WHERE reportID = @1 ", 4)


            Dim sBody As String = Me.createBodyBookBillHTML("4")

            Me.SendMail(Me.eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                       "", Me.eMailServerName, True)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailBookingAndBillings", ex, 1, spath)
        End Try
    End Sub

    ''' <summary>
    ''' Send eMail to the managers with the daily values.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub mailOTP(Optional ByVal ingMinusMonth As Integer = -1)
        Dim spath As String = ""
        Try


            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                        " FROM [warehouseUserReports] WHERE reportID = @1 ", 10)


            Dim sBody As String = Me.createBodyOTP("10", ingMinusMonth)

            Me.SendMail(eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                       "", Me.eMailServerName, True)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailBookingAndBillings", ex, 1, spath)
        End Try
    End Sub


    ''' <summary>
    ''' Mails the priorization report for the Hamburg shop floor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub mailShopFloorPriorization()
        Dim spath As String = ""
        Try

            Dim ds As New DataSet

            ds = dbs.Query("SELECT * FROM  wipQueueView WHERE organization_id = 255  AND (  MEANING LIKE   '%Released%'  ) AND ( QUANTITY_IN_QUEUE <> 0) AND ( SalesOrderLine IS NOT  NULL  ) AND RESOURCE_CODE IN ('505','561','620','707','709','729','733','741')")

            Dim frm As New frmReportViewer("WIP Report Connection to Sales Order", Application.StartupPath & "\ProjectManagerWorkbench\Reporting\repWIPSO.rdlc", "dsWIP_WIPQueueView", ds)

            Dim ds2 As DataSet = dbs.SecureQueryParams(" Select  [reportID], [description], [purpose], [keyPoints], [schedule], [sendTo], [lastRun], [countRuns] " &
                        " FROM [warehouseUserReports] WHERE reportID = @1 ", 3)
            spath = Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString

            frm.Show()
            Dim bytes As Byte()
            Dim mimeType As String = Nothing
            Dim encoding As String = Nothing
            Dim extension As String = Nothing
            'Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing


            bytes = frm.rvStandard.LocalReport.Render("PDF", Nothing, mimeType,
            encoding, extension, streamids, Nothing)



            Dim fs As New FileStream(spath & ".pdf", FileMode.Create)

            fs.Write(bytes, 0, bytes.Length)

            fs.Close()


            'frm.saveCurrentReportToExcel(Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString)
            frm.Close()

            Dim sBody As String = Me.createBodyHTML("3")
            Me.SendMail(eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                        Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString & ".pdf", Me.eMailServerName, True)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailShopFloorPriorities", ex, 1, spath)
        End Try
    End Sub


    ''' <summary>
    ''' Mails the priorization report for the Hamburg shop floor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub mailShopFloorPriorizationNWK()

        Dim spath As String = ""
        Dim strAttachmentList As String = ""
        Dim intFilename As Integer = 0

        Try

            'components we need multiple times
            'could be also tuned to just query the specific ones
            Dim dsComp As DataSet = dboEDC.Query("Select wro.wip_entity_id " &
                                                            ", wro.operation_seq_num " &
                                                            ", wro.segment1 || wro.segment2 As concatenated_segments " &
                                                            ", wro.required_quantity " &
                                                            ", wro.quantity_issued " &
                                                            " FROM WIP_REQUIREMENT_OPERATIONS WRO " &
                                                            ", WIP_DISCRETE_JOBS              HDR   " &
                                                            "            WHERE wro.organization_id = 318 " &
                                                            "   And HDR.organization_id      = WRO.organization_id " &
                                                            "   And HDR.wip_entity_id        = WRO.wip_entity_id " &
                                                            "   And HDR.status_type          < 7")

            'set up the iterations for the different files to attach
            Dim dsAllReports As DataSet = dbs.Query("Select * FROM warehouseUserReports WHERE groupID = 1")

            For i As Integer = 0 To dsAllReports.Tables(0).Rows.Count - 1

                'now split the filter string accordingly
                Dim strDifferentFiles() As String = dsAllReports.Tables(0).Rows(i).Item("additionalFilterString").Split("|")
                For Each strDifferentFilter In strDifferentFiles

                    intFilename += 1    'Increment the filename
                    spath = ""

                    Dim ds As New DataSet

                    ds = dbs.Query("Select  a.[ORGANIZATION_ID] " &
              ", a.[WIP_ENTITY_ID] " &
              ", a.[JOB_NO] " &
              ", a.[OPERATION_SEQ_NO] " &
              ", a.[INVENTORY_ITEM_ID] " &
              ", a.[ITEM_NO] " &
              ", a.[DESCRIPTION] " &
              ", a.[DATE_RELEASED] " &
              ", a.[CLASS_CODE] " &
              ", a.[STATUS_TYPE] " &
              ", a.[START_QUANTITY_HDR] " &
              ", a.[QUANTITY_COMPLETED_HDR] " &
              ", a.[QUANTITY_SCRAPPED_HDR] " &
              ", a.[MEANING] " &
              ", a.[DEPARTMENT_ID] " &
              ", a.[DEPARTMENT_CODE] " &
              ", a.[RESOURCE_ID] " &
              ", a.[RESOURCE_CODE] " &
              ", a.[RESOURCE_SEQ_NO] " &
              ", a.[DESCRIPTION_OPER] " &
              ", a.[START_DATE] " &
              ", a.[COMPLETION_DATE] " &
              ", a.[RESOURCE_END_DATE] " &
              ", a.[USAGE_RATE_OR_AMOUNT] " &
              ", a.[UOM_CODE] " &
              ", a.[SCHEDULED_QUANTITY] " &
              ", a.[QUANTITY_IN_QUEUE] " &
              ", a.[QUANTITY_SCRAPPED] " &
              ", a.[QUANTITY_COMPLETED] " &
              ", a.[DATE_LAST_MOVED] " &
              ", a.[FIRST_UNIT_START_DATE] " &
              ", a.[LAST_UNIT_COMPLETION_DATE] " &
              ", a.[PREVIOUS_OPERATION_SEQ_NO] " &
              ", a.[NEXT_OPERATION_SEQ_NO] " &
              ", a.[BASIS_TYPE] " &
              ", a.[RESOURCE_TYPE] " &
              ", a.[COUNT_POINT_TYPE] " &
              ", a.[SCHEDULED_FLAG] " &
              ", a.[REF_SALES_ORDER_HDR] " &
              ", a.[REF_SALES_ORDER_LINE] " &
              ", a.[LAST_UPDATE_DATE_JOB] " &
              ", a.[SALES_ORDER_NO] " &
              ", a.[SALES_LINE_NO] " &
              ", a.[PARTY_NAME] " &
              ", a.[MRP_DATE] " &
              ", a.[FIRST_OPEN_SEQ] " &
              ", a.[QUANTITY_OPEN] " &
              ", a.[HOURS_OPEN] " &
              ", a.[TEMP] " &
              ", a.[CREATED_BY] " &
              ", a.[CREATION_DATE] " &
              ", a.[LAST_UPDATED_BY] " &
              ", a.[LAST_UPDATE_DATE] " &
              ", a.[LAST_UPDATE_LOGIN]" &
              ", a.[JobNoOperationSeq] " &
              ", a.[SalesOrderLine] " &
              ", a.[DRAWING] " &
              ", a.[PROJECT_ID] " &
              ", a.[PROJECT_NUMBER] " &
              ", a.[PROJECT_NAME] " &
              ", a.[FSG_MATERIAL_CODE] " &
              ", a.[END_DEMAND_ID] " &
              ", a.[firstPeggingId] " &
              ", a.[qtyAvailable] " &
              ", a.[neededBy] " &
              ", a.[Usage12Month] " &
              ", a.[Usage6Month] " &
              ", a.[Usage3Month] " &
              ", a.[AverageLast10days] " &
              ", a.[QtyTotalDemand] " &
              ", a.[PLANNER_CODE] " &
              ", a.[operationSequenceIndicator] " &
              ", a.[componentRequirements] " &
              ", a.[SAFETY_STOCK_QUANTITY] " &
              ", a.[FSG_PRODUCT_CODE] " &
              ", a.[parentJobNo] " &
              ", a.[parentJobStartDate] " &
              ", a.[parentJobStatus] " &
              ", a.[FSG_SALES_TYPE] " &
              ", a.[lowerLevelShortages] " &
              ", a.[qty_onhand] " &
              ", a.[ospItem] " &
              ", a.[FSG_SPARES_TYPE] " &
              ", a.[FSG_IND_PAINT_PARTS] " &
              ", a.[FSG_COMMODITY_CODE] " &
              ", a.[FIXED_LEAD_TIME] " &
              ", a.[BUYER] " &
              ", a.[no_of_purchased_shortages] " &
              ", a.[parentJobPlannerCode] " &
              ", a.[scheduled_completion_date] " &
              ", a.[job_creation_date] " &
              ", 'In Queue @: ' + (SELECT TOP 1 'Seq: ' + cast(operation_seq_no as nvarchar) + ' Res: ' + resource_code  FROM  wipQueueView b  " &
              "      WHERE(a.organization_id = b.organization_id) " &
              "					AND a.wip_entity_id = b.wip_entity_id  " &
              "					AND b.quantity_in_queue <> 0) AS lastComment " &
              " FROM wipQueueView a " &
              "          WHERE(organization_id = 318) AND " & strDifferentFilter &
              " AND (  MEANING LIKE   '%Released%'  ) AND ( scheduled_quantity > quantity_completed)  " &
              " AND a.[QUANTITY_IN_QUEUE] <> 0 " &
              " ORDER BY  a.[FIRST_UNIT_START_DATE]")
                    'Parameter for the different file types = strDifferentFilter



                    For k As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        For j As Integer = 0 To dsComp.Tables(0).Rows.Count - 1
                            'Business rule: only material associated with this operation sequence
                            If ds.Tables(0).Rows(k).Item("WIP_ENTITY_ID") = dsComp.Tables(0).Rows(j).Item("WIP_ENTITY_ID") _
                            AndAlso ds.Tables(0).Rows(k).Item("OPERATION_SEQ_NO") = dsComp.Tables(0).Rows(j).Item("operation_seq_num") Then
                                ds.Tables(0).Rows(k).Item("componentRequirements") = ds.Tables(0).Rows(k).Item("componentRequirements") & dsComp.Tables(0).Rows(j).Item("concatenated_segments") & " " & "Req: " & dsComp.Tables(0).Rows(j).Item("required_quantity") & vbCr
                            End If
                        Next
                    Next

                    Dim frm As New frmReportViewer("WIP Report Connection to Sales Order" _
                                                   , Application.StartupPath & "\ProjectManagerWorkbench\Reporting\repWIPSONWK.rdlc", "dsWIP_WIPQueueView", ds)

                    'Set the report parameters
                    If ds.Tables(0).Rows.Count > 0 Then
                        With frm.rvStandard.LocalReport
                            Dim parameters(0) As ReportParameter
                            parameters(0) = New ReportParameter("repResourceName", ds.Tables(0).Rows(0).Item("RESOURCE_CODE").ToString)

                            .SetParameters(parameters)
                        End With
                    End If


                    Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                                " FROM [warehouseUserReports] WHERE reportID = @1 ", 6)

                    spath = Application.StartupPath & "\" & dsAllReports.Tables(0).Rows(i).Item("description").ToString & intFilename.ToString _
                            & "." & dsAllReports.Tables(0).Rows(i).Item("saveAs").ToString.ToLower

                    frm.Show()
                    Dim bytes As Byte()
                    Dim mimeType As String = Nothing
                    Dim encoding As String = Nothing
                    Dim extension As String = Nothing
                    'Dim warnings As Warning() = Nothing
                    Dim streamids As String() = Nothing

                    bytes = frm.rvStandard.LocalReport.Render(dsAllReports.Tables(0).Rows(i).Item("renderType"), Nothing, mimeType,
                    encoding, extension, streamids, Nothing)

                    Dim fs As New FileStream(spath, FileMode.Create)

                    fs.Write(bytes, 0, bytes.Length)

                    fs.Close()

                    'frm.saveCurrentReportToExcel(Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString)
                    frm.Close()

                    'now create the consolidated attachment list
                    If strAttachmentList = String.Empty Then
                        strAttachmentList = spath
                    Else
                        strAttachmentList += ";" & spath
                    End If
                Next

                'now send the mail for one batch with all attachments

                Dim sBody As String = Me.createBodyHTML(dsAllReports.Tables(0).Rows(i).Item("reportID"))
                Me.SendMail(eMailAdressFrom, dsAllReports.Tables(0).Rows(i).Item("sendTo").ToString, "",
                            dsAllReports.Tables(0).Rows(i).Item("description").ToString, sBody,
                            strAttachmentList, Me.eMailServerName, True)
                strAttachmentList = ""

            Next

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailShopFloorPriorities", ex, 1, spath)
        End Try

    End Sub

    ''' <summary>
    ''' Creates and sends an email for the offsite report.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub mailOffsiteInvReport()
        Dim spath As String = ""

        Try

            Dim sSQL As String

            'for Givens report
            'mark each MRP demand for components of released, scheduled pumps
            dbs.NonQuery("UPDATE mrp_demand SET temp = NULL")
            sSQL = "UPDATE d2 SET temp = 'need for pumps' FROM mrp_demand d2 INNER JOIN  mrp_demand d ON d.END_DEMAND_ID = d2.END_DEMAND_ID and d.organization_id = d2.organization_id " &
             "WHERE (d.neededBy < { fn NOW() } + 60 AND d.START_DATE < { fn NOW() } + 7 AND d.PARENT_SALES_ORDER_NO IS NOT NULL " &
             "AND (d.PLANNER_CODE LIKE 'BAY' OR d.PLANNER_CODE LIKE 'LINE%' OR d.PLANNER_CODE LIKE 'PARTS')) AND d.STATUS LIKE 'Released%' AND d2.LEVEL_NO > 0"
            dbs.NonQuery(sSQL)

            'for released shop jobs needed within 7 days where the parent pump job is not released
            sSQL = "UPDATE d  SET temp = 'Need for shop'  FROM mrp_demand d INNER JOIN mrp_demand d2 ON d.ORGANIZATION_ID = d2.ORGANIZATION_ID AND " &
            "d.USING_ASSEMBLY_ITEM_ID = d2.INVENTORY_ITEM_ID AND d.END_DEMAND_ID = d2.END_DEMAND_ID AND d.PARENT_JOB_ORDER_ID = d2.JOB_ID " &
            "WHERE d.temp IS NULL AND d2.STATUS LIKE 'Released%' AND d2.neededBy < { fn NOW() } + 7 AND NOT d2.PLANNER_CODE LIKE 'BAY' AND NOT d2.PLANNER_CODE LIKE 'LINE%' AND NOT d2.PLANNER_CODE LIKE 'PARTS' AND d2.parentJobStatus IS NOT NULL "
            dbs.NonQuery(sSQL)

            'add in aftermarket requirements too - if inv is available to ship 
            sSQL = "UPDATE d SET temp = 'need for AM'  FROM mrp_demand d  WHERE temp IS NULL AND SHORT IS NULL AND PARENT_SALES_ORDER_NO IS NOT NULL AND neededBy < { fn NOW() } + 30 AND d.STATUS LIKE 'Released%'"
            dbs.NonQuery(sSQL)

            'put summary of all demand into work table
            dbs.NonQuery("DELETE FROM tblWork")
            sSQL = "INSERT INTO tblWork (Number1, Number2, Number3, Text1, Text2, Text3, Number4) " &
            "SELECT ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID, ITEM_NO, DESCRIPTION, PROJECT_NAME, SUM(MRP_QTY) AS sumMRPQty FROM mrp_demand " &
            "WHERE temp is not null GROUP BY ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID, ITEM_NO, DESCRIPTION, PROJECT_NAME "
            dbs.NonQuery(sSQL)

            'put onsite inventory into 2nd work table
            dbs.NonQuery("DELETE FROM tblWork2")
            sSQL = "INSERT INTO tblWork2 (Number1, Number2, Number3, Number4) " &
            "SELECT ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID, SUM(QTY_ONHAND) AS SumQtyOH " &
            "FROM Items_QOH_Detail  WHERE  (LOCATOR NOT LIKE '%givens%') " &
            "GROUP BY ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID "
            dbs.NonQuery(sSQL)
            'copy on site inventory into first working table
            sSQL = "UPDATE w  SET Number5 = w2.Number4 FROM tblWork w INNER JOIN tblWork2 w2 ON w.Number1 = w2.Number1  AND  w.Number2 = w2.Number2 " &
            "WHERE (w.Number3 = w2.Number3 Or (w.Number3 Is NULL And w2.Number3 Is NULL)) "
            dbs.NonQuery(sSQL)

            'put offsite inventory into 2nd work table
            dbs.NonQuery("DELETE FROM tblWork2")
            sSQL = "INSERT INTO tblWork2 (Number1, Number2, Number3, Number4) " &
            "SELECT ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID, SUM(QTY_ONHAND) AS SumQtyOH " &
            "FROM Items_QOH_Detail WHERE (LOCATOR  LIKE '%givens%') " &
            "GROUP BY ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID "
            dbs.NonQuery(sSQL)

            'copy off site inventory into first working table
            sSQL = "UPDATE w  SET Number6 = w2.Number4 FROM tblWork w INNER JOIN tblWork2 w2 ON w.Number1 = w2.Number1  AND  w.Number2 = w2.Number2 " &
              "WHERE (w.Number3 = w2.Number3 Or (w.Number3 Is NULL And w2.Number3 Is NULL)) "
            dbs.NonQuery(sSQL)

            dbs.NonQuery("UPDATE tblWork SET Number5 = 0 WHERE Number5 IS NULL") 'replace null qty on hand values
            dbs.NonQuery("UPDATE tblWork SET Number6 = 0 WHERE Number6 IS NULL")

            Dim ds As New DataSet
            ds = dbs.Query("SELECT     Text1, Text2, Text3, Number4, Number5, Number6  FROM  tblWork WHERE (Number5 < Number4) ORDER BY Text1, Text3")

            Dim operation As New clsGridOperations
            Dim frm As New frmReportViewer("Sales Order Backlog", Application.StartupPath & "\ProjectManagerWorkbench\Reporting\repOffsiteInventory.rdlc", "dsWork_tblWork", ds)

            'Get the information for the report

            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                    " FROM [warehouseUserReports] WHERE reportID = @1 ", 2)
            spath = Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString

            frm.Show()

            frm.saveCurrentReportToExcel(Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString)
            frm.Close()

            Dim sBody As String = Me.createBodyHTML("2")
            Me.SendMail(eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                        Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString & ".xls", Me.eMailServerName, True)

            ' Stop

            'Here is the output:
            'report:
            '                   Item   Descr  Proj     Need       in house   offsite 
            ' SELECT     Text1, Text2, Text3, Number4, Number5, Number6  FROM  tblWork WHERE (Number5 < Number4) ORDER BY Text1, Text3

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailOffsiteInvReport", ex, 1, spath)
        End Try
    End Sub


    ''' <summary>
    ''' Creates and sends an email for the offsite report.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ASCPmailOffsiteInvReport()
        Dim spath As String = ""

        Try

            Dim sSQL As String

            'for Givens report
            'mark each MRP demand for components of released, scheduled pumps
            dbs.NonQuery("UPDATE msc_demand SET temp = NULL")
            sSQL = "UPDATE d2 SET temp = 'need for pumps' FROM msc_demand d2 INNER JOIN  msc_demand d ON d.END_DEMAND_ID = d2.END_DEMAND_ID and d.organization_id = d2.organization_id " &
             "WHERE (d.mrp_date < { fn NOW() } + 60 AND d.PARENT_SALES_ORDER_NO IS NOT NULL " &
             "AND (d.PLANNER_CODE LIKE 'BAY' OR d.PLANNER_CODE LIKE 'LINE%' OR d.PLANNER_CODE LIKE 'PARTS')) AND d.STATUS LIKE 'Released%' AND d2.LEVEL_NO > 0"
            dbs.NonQuery(sSQL)

            'for released shop jobs needed within 7 days where the parent pump job is not released
            sSQL = "UPDATE d  SET temp = 'Need for shop'  FROM msc_demand d INNER JOIN msc_demand d2 ON d.ORGANIZATION_ID = d2.ORGANIZATION_ID AND " &
            "d.USING_ASSEMBLY_ITEM_ID = d2.INVENTORY_ITEM_ID AND d.END_DEMAND_ID = d2.END_DEMAND_ID AND d.PARENT_JOB_ORDER_ID = d2.JOB_ID " &
            "WHERE d.temp IS NULL AND d2.STATUS LIKE 'Released%' AND d2.mrp_date < { fn NOW() } + 7 AND NOT d2.PLANNER_CODE LIKE 'BAY' AND NOT d2.PLANNER_CODE LIKE 'LINE%' AND NOT d2.PLANNER_CODE LIKE 'PARTS' AND d2.parentJobStatus IS NOT NULL AND d.STATUS LIKE 'Released%' "
            dbs.NonQuery(sSQL)

            'add in aftermarket requirements too - if inv is available to ship 
            sSQL = "UPDATE d SET temp = 'need for AM'  FROM msc_demand d  WHERE temp IS NULL AND SHORT IS NULL AND PARENT_SALES_ORDER_NO IS NOT NULL AND mrp_date < { fn NOW() } + 30 AND d.STATUS LIKE 'Released%' "
            dbs.NonQuery(sSQL)

            'put summary of all demand into work table
            dbs.NonQuery("DELETE FROM tblWork")
            sSQL = "INSERT INTO tblWork (Number1, Number2, Number3, Text1, Text2, Text3, Number4) " &
            "SELECT ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID, ITEM_NO, DESCRIPTION, PROJECT_NAME, SUM(MRP_QTY) AS sumMRPQty FROM msc_demand " &
            "WHERE temp is not null GROUP BY ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID, ITEM_NO, DESCRIPTION, PROJECT_NAME "
            dbs.NonQuery(sSQL)

            'put onsite inventory into 2nd work table
            dbs.NonQuery("DELETE FROM tblWork2")
            sSQL = "INSERT INTO tblWork2 (Number1, Number2, Number3, Number4) " &
            "SELECT ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID, SUM(QTY_ONHAND) AS SumQtyOH " &
            "FROM Items_QOH_Detail  WHERE  (LOCATOR NOT LIKE '%givens%') " &
            "GROUP BY ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID "
            dbs.NonQuery(sSQL)
            'copy on site inventory into first working table
            sSQL = "UPDATE w  SET Number5 = w2.Number4 FROM tblWork w INNER JOIN tblWork2 w2 ON w.Number1 = w2.Number1  AND  w.Number2 = w2.Number2 " &
            "WHERE (w.Number3 = w2.Number3 Or (w.Number3 Is NULL And w2.Number3 Is NULL)) "
            dbs.NonQuery(sSQL)

            'put offsite inventory into 2nd work table
            dbs.NonQuery("DELETE FROM tblWork2")
            sSQL = "INSERT INTO tblWork2 (Number1, Number2, Number3, Number4) " &
            "SELECT ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID, SUM(QTY_ONHAND) AS SumQtyOH " &
            "FROM Items_QOH_Detail WHERE (LOCATOR  LIKE '%givens%') " &
            "GROUP BY ORGANIZATION_ID, INVENTORY_ITEM_ID, PROJECT_ID "
            dbs.NonQuery(sSQL)

            'copy off site inventory into first working table
            sSQL = "UPDATE w  SET Number6 = w2.Number4 FROM tblWork w INNER JOIN tblWork2 w2 ON w.Number1 = w2.Number1  AND  w.Number2 = w2.Number2 " &
              "WHERE (w.Number3 = w2.Number3 Or (w.Number3 Is NULL And w2.Number3 Is NULL)) "
            dbs.NonQuery(sSQL)

            dbs.NonQuery("UPDATE tblWork SET Number5 = 0 WHERE Number5 IS NULL") 'replace null qty on hand values
            dbs.NonQuery("UPDATE tblWork SET Number6 = 0 WHERE Number6 IS NULL")

            Dim ds As New DataSet
            ds = dbs.Query("SELECT     Text1, Text2, Text3, Number4, Number5, Number6  FROM  tblWork WHERE (Number5 < Number4) ORDER BY Text1, Text3")

            Dim operation As New clsGridOperations
            'Dim frm As New frmReportViewer("Sales Order Backlog", Application.StartupPath & "\ProjectManagerWorkbench\Reporting\repOffsiteInventory.rdlc", "dsWork_tblWork", ds)

            'Get the information for the report

            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                    " FROM [warehouseUserReports] WHERE reportID = @1 ", 2)
            spath = Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString

            'frm.Show()

            'frm.saveCurrentReportToExcel(Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString)
            'frm.Close()

            Dim sBody As String = Me.createBodyHTML("2")
            Me.SendMail(eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                        Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString & ".xls", Me.eMailServerName, True)

            ' Stop

            'Here is the output:
            'report:
            '                   Item   Descr  Proj     Need       in house   offsite 
            ' SELECT     Text1, Text2, Text3, Number4, Number5, Number6  FROM  tblWork WHERE (Number5 < Number4) ORDER BY Text1, Text3

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailOffsiteInvReport", ex, 1, spath)
        End Try
    End Sub

    ''' <summary>
    ''' E-Mail the sales order line backlog report to certain people.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ASCPmailSalesOrderBacklog(ByVal organizationIDList As String)
        Dim spath As String = ""
        Try

            Dim ds As New DataSet

            ds = dbs.Query("SELECT * FROM  MSCsalesOrderLinesView WHERE organization_id IN " & organizationIDList & " ")

            Dim frm As New frmReportViewer("Sales Order Backlog" _
                                           , Application.StartupPath & "\ProjectManagerWorkbench\Reporting\repSalesOrderDGV.rdlc", "dsSalesOrderBacklog_sales_orders", ds)

            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                        " FROM [warehouseUserReports] WHERE reportID = @1 ", 1)
            spath = Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString

            frm.saveCurrentReportToExcel(Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString)
            frm.Close()

            Dim sBody As String = Me.createBodyHTML("1")
            Me.SendMail(Me.eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                        Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString & ".xls", Me.eMailServerName, True)

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("mailSalesOrderBacklog", ex, 1, spath)
        End Try

    End Sub

    ''' <summary>
    ''' E-Mail the sales order line backlog report to certain people.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub mailSalesOrderBacklog(ByVal organizationIDList As String)
        Dim spath As String = ""
        Try

            Dim ds As New DataSet

            ds = dbs.Query("SELECT * FROM  salesOrderLinesView WHERE organization_id IN " & organizationIDList & " ")

            Dim frm As New frmReportViewer("Sales Order Backlog" _
                                           , Application.StartupPath & "\ProjectManagerWorkbench\Reporting\repSalesOrderDGV.rdlc", "dsSalesOrderBacklog_sales_orders", ds)

            Dim ds2 As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " &
                        " FROM [warehouseUserReports] WHERE reportID = @1 ", 1)
            spath = Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString

            frm.saveCurrentReportToExcel(Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString)
            frm.Close()

            Dim sBody As String = Me.createBodyHTML("1")
            Me.SendMail(Me.eMailAdressFrom, ds2.Tables(0).Rows(0).Item("sendTo").ToString, "",
                        ds2.Tables(0).Rows(0).Item("description").ToString, sBody,
                        Application.StartupPath & "\" & ds2.Tables(0).Rows(0).Item("description").ToString & ".xls", Me.eMailServerName, True)

        Catch ex As Exception

            Dim err As New clsExceptionManagement
            err.createErrorLog("mailSalesOrderBacklog", ex, 1, spath)

        End Try

    End Sub

    Public Sub mailErrorReport(ByVal description As String, ByVal errorLine As String, ByVal noOfSteps As String, ByVal procedureOrFunction As String, ByVal invokedBy As String, ByVal creationDate As String, ByVal causingObject As String, ByVal InnerException As String)
        Dim sBody As String = Me.createBodyProblemHTML(description, errorLine, noOfSteps, procedureOrFunction, invokedBy, creationDate, causingObject, InnerException)
        Try
            Me.SendMail(Me.eMailAdressFrom, "NRuemmeli@Flowserve.com", "",
                        description, sBody, "", Me.eMailServerName, True)

        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' Send a mail with the specific properties.
    ''' </summary>
    ''' <param name="strFrom">The sender of the mail.</param>
    ''' <param name="strTo">The destination of the mail.</param>
    ''' <param name="strCC">The copied persons.</param>
    ''' <param name="strSubject">The subject this mail is about.</param>
    ''' <param name="strBody">The text of the body.</param>
    ''' <param name="strAttachments">The files to attach to the mail.</param>
    ''' <param name="strSMTPServer">The smtp server we want to use.</param>
    ''' <remarks></remarks>
    Public Sub SendMail(ByVal strFrom As String, ByVal strTo As String, ByVal strCC As String, ByVal strSubject As String, ByVal strBody As String, ByVal strAttachments As String, ByVal strSMTPServer As String, ByVal isHTML As Boolean)
        Try
            Dim insMail As New MailMessage()

            'Setting up the mail propertis

            'From whom is the mail?
            If Not strFrom.Equals(String.Empty) Then
                insMail.From = New MailAddress(strFrom)
            End If


            'Who should get the mail. List;List is seperated
            If Not strTo.Equals(String.Empty) Then
                Dim strToNew As String
                Dim strToAr() As String = strTo.Split(";")
                For Each strToNew In strToAr
                    insMail.To.Add(New MailAddress(strToNew))
                Next
            End If

            insMail.Subject = strSubject
            insMail.IsBodyHtml = isHTML

            insMail.Body = strBody
            If Not strCC.Equals(String.Empty) Then
                insMail.CC.Add(New MailAddress(strCC))
            End If

            insMail.Sender = New MailAddress(strFrom)

            If Not strFrom.Equals(String.Empty) Then
                insMail.ReplyTo = New MailAddress(strFrom)
            End If

            'Adds a series of attachments seperated with ; 
            If Not strAttachments.Equals(String.Empty) Then
                Dim strFile As String
                Dim strAttach() As String = strAttachments.Split(";")
                For Each strFile In strAttach
                    Dim attach As New Attachment(strFile.Trim())
                    insMail.Attachments.Add(attach)
                Next
            End If

            ' create smtp client and add credentials
            Dim smtpCl As New SmtpClient(strSMTPServer)
            smtpCl.Port = 587

            smtpCl.EnableSsl = True
            'smtpCl.UseDefaultCredentials = False
            smtpCl.Credentials = New System.Net.NetworkCredential("info@pleugerindustries.com", "Ipleuger123!")

            smtpCl.Host = strSMTPServer
            smtpCl.DeliveryMethod = SmtpDeliveryMethod.Network
            smtpCl.Timeout = 1000000
            ' System.Net.ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

            smtpCl.Send(insMail)
            'MessageBox.Show("Mail sent.")

        Catch e As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("SendMail", e, 1)
        End Try
    End Sub

    Public Shared Function ValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean

        If sslPolicyErrors = SslPolicyErrors.None Then

            Return True

        Else

            Return True

            'If System.Windows.Forms.MessageBox.Show("The server certificate is not valid." & vbLf & "Accept?", "Certificate Validation", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
            '    Return True
            'Else
            '    Return False
            'End If
        End If

    End Function


    Private Function createBodyHTML(ByVal reportId As String) As String
        Dim sBody As String = ""
        Dim ds As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " & _
                " FROM [warehouseUserReports] WHERE reportID = @1 ", reportId)


        sBody = "<table width='500' bgcolor='#6699ff'> " & _
                "    <tr> " & _
                "        <td class='style4' colspan='2' bgcolor='White'> " & _
                "Autobahn - Auto E-Mail Report " & _
                "    </td> " & _
                "    </tr> " & _
                "    <tr> " & _
                "        <td class='style4' > " & _
                " Description</td> " & _
                "       <td class='style3'> " & _
                " " & ds.Tables(0).Rows(0).Item("description") & " </td> " & _
                "    </tr> " & _
                "    <tr bgcolor='White'> " & _
                "        <td class='style4'> " & _
                " Key Points</td> " & _
                "        <td class='style3'> " & _
                " " & ds.Tables(0).Rows(0).Item("keyPoints") & "</td> " & _
                "    </tr> " & _
                "    <tr>  " & _
                "        <td class='style4'> " & _
                "  Purpose</td> " & _
                "        <td class='style3'> " & _
                " " & ds.Tables(0).Rows(0).Item("purpose") & "</td> " & _
                "    </tr> " & _
                "    <tr bgcolor='White'> " & _
                "        <td class='style4'> " & _
                " Date</td> " & _
                "        <td class='style3'> " & _
                " " & FormatDateTime(Now, DateFormat.ShortDate) & "</td> " & _
                "    </tr> " & _
                "    <tr> " & _
                "        <td class='style4'> " & _
                " Schedule</td> " & _
                "        <td class='style3'> " & _
                " " & ds.Tables(0).Rows(0).Item("schedule") & "</td> " & _
                "    </tr> " & _
                " </table>"


        Return sBody

    End Function

    Private Function createBodyOTP(ByVal reportId As String, Optional ByVal ingMonthMinus As Integer = -1) As String
        Dim sBody As String = ""
        Dim ds As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " & _
                " FROM [warehouseUserReports] WHERE reportID = @1 ", reportId)
        Dim strYear As String
        If Now.Month = 1 AndAlso Now.DayOfYear > 2 Then           'For the january, make sure we just january dates
            strYear = Now.Year.ToString
        ElseIf Now.Month = 1 AndAlso Now.DayOfYear <= 2 Then
            strYear = DateAdd(DateInterval.Month, -1, Now).Year.ToString
        Else
            strYear = DateAdd(DateInterval.Month, -1, Now).Year.ToString
        End If

        Dim strMonth As String
        'Here we need during the month the current month and when we are in the first three days 
        If Now.DayOfYear < 2 Then
            strMonth = DateAdd(DateInterval.Month, -1, Now).Month
        Else
            strMonth = DateAdd(DateInterval.Month, ingMonthMinus, Now).Month
        End If


        Dim strDetails As String = "<table width='800' bgcolor='#6699ff'> "
        strDetails += "<tr>"
        strDetails += "<td>"
        strDetails += "Order Number"
        strDetails += "</td>"

        strDetails += "<td>"
        strDetails += "Customer Name"
        strDetails += "</td>"

        strDetails += "<td>"
        strDetails += "KD"
        strDetails += "</td>"

        strDetails += "<td>"
        strDetails += "Billing"
        strDetails += "</td>"

        strDetails += "<td>"
        strDetails += "Prom"
        strDetails += "</td>"

        strDetails += "<td>"
        strDetails += "Deliv"
        strDetails += "</td>"

        strDetails += "<td>"
        strDetails += "Late"
        strDetails += "</td>"

        strDetails += "<td>"
        strDetails += "Count"
        strDetails += "</td>"

        strDetails += "</tr>"




        sBody = "<table width='500' bgcolor='#6699ff'> " &
                "    <tr> " &
                "        <td class='style4' colspan='2' bgcolor='White'> " &
                " Business Cockpit - Auto E-Mail Report " &
                "    </td> " &
                "    </tr> " &
                "    <tr> " &
                "        <td class='style4' > " &
                " Description</td> " &
                "       <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("description") & " For year: " & strYear & " And Month: " & strMonth & " </td> " &
                "    </tr> " &
                "    <tr bgcolor='White'> " &
                "        <td class='style4'> " &
                " Key Points</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("keyPoints") & "</td> " &
                "    </tr> " &
                "    <tr>  " &
                "        <td class='style4'> " &
                "  Purpose</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("purpose") & "</td> " &
                "    </tr> " &
                "    <tr bgcolor='White'> " &
                "        <td class='style4'> " &
                " Date</td> " &
                "        <td class='style3'> " &
                " " & FormatDateTime(Now, DateFormat.ShortDate) & "</td> " &
                "    </tr> " &
                "    <tr> " &
                "        <td class='style4'> " &
                " Schedule</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("schedule") & "</td> " &
                "    </tr> " &
                " </table> <br>"
        Try

            'Dim dsO As DataSet = dboEDC.Connect
            dboEDC.Connect()
            dboEDC.NonQuery("begin mo_global.set_policy_context('S','254'); end;")

            Dim ds2 As DataSet = dboEDC.Query("SELECT OH.ORDER_NUMBER " & _
                   " ,ac.CUSTOMER_NAME " & _
                   " ,BI.CUSTOMER_TYPE_CODE AS KD " & _
                   " ,trunc(sum((BI.ordered_quantity * BI.SELLING_PRICE * bi.conversion_rate *(bi.salesrep_percent/100))),2) As Billing " & _
                    " ,TO_CHAR(nvl(OL.Promise_Date,INV.TRX_DATE) ,'YYYYIW') Prom " & _
                   " ,TO_CHAR(INV.TRX_DATE,'YYYYIW') Deliv " & _
                   " ,decode(to_number(TO_CHAR(OL.Promise_Date,'YYYYIW') - TO_CHAR(INV.TRX_DATE,'YYYYIW')) " & _
                   "                 / decode(abs(to_number(TO_CHAR(OL.Promise_Date,'YYYYIW') - TO_CHAR(INV.TRX_DATE,'YYYYIW'))),NULL,1,0,1, " & _
                   "                 abs(to_number(TO_CHAR(OL.Promise_Date,'YYYYIW') - TO_CHAR(INV.TRX_DATE,'YYYYIW'))) ),-1,1,NULL) As Late " & _
                   " ,decode(to_number(TO_CHAR(OL.Promise_Date,'YYYYIW') - TO_CHAR(INV.TRX_DATE,'YYYYIW')),9999,0,1)  As Anzahl     " & _
                   " FROM   APPS.BBB_BILLING BI " & _
                   "                ,APPS.OE_ORDER_LINES OL " & _
                   "                ,APPS.OE_ORDER_HEADERS OH " & _
                   "                ,APPS.RA_CUSTOMER_TRX_ALL INV  " & _
                   "                ,APPS.GL_PERIODS_V GLP " & _
                   "                ,APPS.RA_CUSTOMER_TRX_LINES_ALL LALL, " & _
                   " APPS.AR_CUSTOMERS ac, " & _
                     " hz_cust_site_uses_all hcs_ship , " & _
                     " hz_cust_acct_sites_all hca_ship , " & _
                     " hz_party_sites hps_ship , " & _
                     " hz_parties hp_ship , " & _
                     " hz_locations hl_ship " & _
                   "        where BI.PERIOD_NUM = '" & strMonth & _
                   "' AND       BI.PERIOD_YEAR = '" & strYear & _
                   "' AND       BI.LINE_ID = OL.LINE_ID " & _
                   " AND       OH.HEADER_ID = OL.HEADER_ID " & _
                   " AND       BI.ORDER_NUMBER = OH.ORDER_NUMBER " & _
                   " AND       LALL.CUSTOMER_TRX_ID = INV.CUSTOMER_TRX_ID " & _
                   " AND       LALL.LINE_TYPE = 'LINE' " & _
                   " AND       LALL.INTERFACE_LINE_ATTRIBUTE1 = to_char(BI.ORDER_NUMBER) " & _
                   " AND       BI.LINE_ID = LALL.INTERFACE_LINE_ATTRIBUTE6 " & _
                   " AND       to_char(BI.ORDER_NUMBER) = to_char(INV.INTERFACE_HEADER_ATTRIBUTE1) " & _
                   " AND       INV.ORG_ID = 254 " & _
                   " AND       GLP.PERIOD_SET_NAME = 'FPD_HAMBURG_CAL' " & _
                   " AND       GLP.PERIOD_YEAR = BI.PERIOD_YEAR " & _
                   " AND       GLP.PERIOD_NUM = BI.PERIOD_NUM " & _
                   " AND       INV.TRX_DATE >= GLP.START_DATE " & _
                   " AND       INV.TRX_DATE <= GLP.END_DATE " & _
                   " AND ac.CUSTOMER_ID = hca_ship.CUST_ACCOUNT_ID " & _
                    " AND oh.ship_to_org_id = hcs_ship.site_use_id  " & _
                    " AND hcs_ship.cust_acct_site_id = hca_ship.cust_acct_site_id  " & _
                    " AND hca_ship.party_site_id = hps_ship.party_site_id  " & _
                    " AND hps_ship.party_id = hp_ship.party_id  " & _
                    " AND hps_ship.location_id = hl_ship.location_id  " & _
                    " AND hca_ship.org_id = 254 " & _
                   " GROUP BY BI.CUSTOMER_TYPE_CODE  " & _
                   "                ,OH.ORDER_NUMBER " & _
                   "                ,ac.CUSTOMER_NAME " & _
                   "                ,TO_CHAR(nvl(OL.Promise_Date,INV.TRX_DATE) ,'YYYYIW')    " & _
                   "                ,TO_CHAR(INV.TRX_DATE,'YYYYIW') " & _
                   "                ,decode(to_number(TO_CHAR(OL.Promise_Date,'YYYYIW') - TO_CHAR(INV.TRX_DATE,'YYYYIW')) " & _
                   "                                / decode(abs(to_number(TO_CHAR(OL.Promise_Date,'YYYYIW') - TO_CHAR(INV.TRX_DATE,'YYYYIW'))),NULL,1,0,1, " & _
                   "                                abs(to_number(TO_CHAR(OL.Promise_Date,'YYYYIW') - TO_CHAR(INV.TRX_DATE,'YYYYIW'))) ),-1,1,NULL) " & _
                   "                ,decode(to_number(TO_CHAR(OL.Promise_Date,'YYYYIW') - TO_CHAR(INV.TRX_DATE,'YYYYIW')),9999,0,1)  " & _
                   " order by BI.CUSTOMER_TYPE_CODE, " & _
                   "        OH.ORDER_NUMBER ")

            'Count the occurances
            ' And make the right table
            Dim overCount As Integer = 0
            Dim overOTP As Integer = 0
            Dim over3Weeks As Integer = 0
            Dim overUntil3Weeks As Integer = 0

            Dim custPartsCount As Integer = 0
            Dim custPartsOTP As Integer = 0
            Dim custParts3Weeks As Integer = 0
            Dim custPartsUntil3Weeks As Integer = 0

            Dim custRepairCount As Integer = 0
            Dim custRepairOTP As Integer = 0
            Dim custRepair3Weeks As Integer = 0
            Dim custRepairUntil3Weeks As Integer = 0

            Dim custServiceCount As Integer = 0
            Dim custServiceOTP As Integer = 0
            Dim custService3Weeks As Integer = 0
            Dim custServiceUntil3Weeks As Integer = 0

            Dim custUnitsAndBuyoutsCount As Integer = 0
            Dim custUnitsAndBuyoutsOTP As Integer = 0
            Dim custUnitsAndBuyouts3Weeks As Integer = 0
            Dim custUnitsAndBuyoutsUntil3Weeks As Integer = 0


            Dim intercompanyPartsCount As Integer = 0
            Dim intercompanyPartsOTP As Integer = 0
            Dim intercompanyParts3Weeks As Integer = 0
            Dim intercompanyPartsUntil3Weeks As Integer = 0

            Dim intercompanyRepairCount As Integer = 0
            Dim intercompanyRepairOTP As Integer = 0
            Dim intercompanyRepair3Weeks As Integer = 0
            Dim intercompanyRepairUntil3Weeks As Integer = 0

            Dim intercompanyServiceCount As Integer = 0
            Dim intercompanyServiceOTP As Integer = 0
            Dim intercompanyService3Weeks As Integer = 0
            Dim intercompanyServiceUntil3Weeks As Integer = 0

            Dim intercompanyUnitsAndBuyoutsCount As Integer = 0
            Dim intercompanyUnitsAndBuyoutsOTP As Integer = 0
            Dim intercompanyUnitsAndBuyouts3Weeks As Integer = 0
            Dim intercompanyUnitsAndBuyoutsUntil3Weeks As Integer = 0

            For i As Integer = 0 To ds2.Tables(0).Rows.Count - 1
                'Exclude from the calculation: Gutschriften, Rückgaben und Garantien
                If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) <> "9" AndAlso ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(3, 1) <> "3" AndAlso ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(3, 1) <> "5" Then


                    'Understand the weeks difference. More or equal 21 days show it as late in that indicator
                    'This will now drive the whole 
                    Dim weekDiff As Integer = 0
                    If Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Prom")) AndAlso Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Deliv")) Then
                        weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                    End If




                    'Overall
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    'Gutschriften sollten nicht gezählt werden!
                    overCount += 1
                    If Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Late")) Then
                        If Not weekDiff >= -1 Then
                        Else
                            overOTP += 1
                        End If
                    Else
                        overOTP += 1
                    End If

                    If weekDiff <= -3 Then
                        over3Weeks += 1
                    ElseIf weekDiff > -3 Then
                        overUntil3Weeks += 1
                    End If

                    'Customer Parts
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "4" AndAlso ds2.Tables(0).Rows(i).Item("KD").ToString.Trim = "R" Then
                        custPartsCount += 1
                        If Not weekDiff >= -1 Then
                            If ds2.Tables(0).Rows(i).Item("Late") Then

                            End If
                        Else
                            custPartsOTP += 1
                        End If

                        weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                        If weekDiff <= -3 Then
                            custParts3Weeks += 1
                        ElseIf weekDiff > -3 Then
                            custPartsUntil3Weeks += 1
                        End If

                    End If


                    'Customer Repair
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "6" AndAlso ds2.Tables(0).Rows(i).Item("KD").ToString.Trim = "R" Then
                        custRepairCount += 1
                        If Not weekDiff >= -1 Then
                            If ds2.Tables(0).Rows(i).Item("Late") Then

                            End If
                        Else
                            custRepairOTP += 1
                        End If

                        weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                        If weekDiff <= -3 Then
                            custRepair3Weeks += 1
                        ElseIf weekDiff > -3 Then
                            custRepairUntil3Weeks += 1
                        End If

                    End If


                    'Customer Service
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "7" AndAlso ds2.Tables(0).Rows(i).Item("KD").ToString.Trim = "R" Then
                        custServiceCount += 1
                        If Not weekDiff >= -1 Then
                            If ds2.Tables(0).Rows(i).Item("Late") Then

                            End If
                        Else
                            custServiceOTP += 1
                        End If

                        weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                        If weekDiff <= -3 Then
                            custService3Weeks += 1
                        ElseIf weekDiff > -3 Then
                            custServiceUntil3Weeks += 1
                        End If

                    End If



                    'Customer Units and Buyouts
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "1" Or ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "2" Or ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "3" Or ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "5" Then
                        If ds2.Tables(0).Rows(i).Item("KD").ToString.Trim = "R" Then
                            custUnitsAndBuyoutsCount += 1
                            If Not weekDiff >= -1 Then
                                If ds2.Tables(0).Rows(i).Item("Late") Then

                                End If
                            Else
                                custUnitsAndBuyoutsOTP += 1
                            End If

                            weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                            If weekDiff <= -3 Then
                                custUnitsAndBuyouts3Weeks += 1
                            ElseIf weekDiff > -3 Then
                                custUnitsAndBuyoutsUntil3Weeks += 1
                            End If

                        End If
                    End If

                    '#####################################################################################
                    'Intercompany Parts
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "4" AndAlso ds2.Tables(0).Rows(i).Item("KD").ToString.Trim = "I" Then
                        intercompanyPartsCount += 1
                        If Not weekDiff >= -1 Then
                            If ds2.Tables(0).Rows(i).Item("Late") Then

                            End If
                        Else
                            intercompanyPartsOTP += 1
                        End If

                        weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                        If weekDiff <= -3 Then
                            intercompanyParts3Weeks += 1
                        ElseIf weekDiff > -3 Then
                            intercompanyPartsUntil3Weeks += 1
                        End If

                    End If


                    'Intercompany Repair
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "6" AndAlso ds2.Tables(0).Rows(i).Item("KD").ToString.Trim = "I" Then
                        intercompanyRepairCount += 1
                        If Not weekDiff >= -1 Then
                            If ds2.Tables(0).Rows(i).Item("Late") Then

                            End If
                        Else
                            intercompanyRepairOTP += 1
                        End If
                        If Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Prom")) AndAlso Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Deliv")) Then
                            weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                        Else
                            weekDiff = 0
                        End If

                        If weekDiff <= -3 Then
                            intercompanyRepair3Weeks += 1
                        ElseIf weekDiff > -3 Then
                            intercompanyRepairUntil3Weeks += 1
                        End If

                    End If

                    'Intercompany Service
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "7" AndAlso ds2.Tables(0).Rows(i).Item("KD").ToString.Trim = "I" Then
                        intercompanyServiceCount += 1
                        If Not weekDiff >= -1 Then
                            If ds2.Tables(0).Rows(i).Item("Late") Then

                            End If
                        Else
                            intercompanyServiceOTP += 1
                        End If

                        If Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Prom")) AndAlso Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Deliv")) Then
                            weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                        Else
                            weekDiff = 0
                        End If
                        If weekDiff <= -3 Then
                            intercompanyService3Weeks += 1
                        ElseIf weekDiff > -3 Then
                            intercompanyServiceUntil3Weeks += 1
                        End If

                    End If


                    'Intercompany Units and Buyouts
                    'Deliveries # | Deliveries On Time | Percentage
                    'OTP
                    'Until 3 weeks
                    'Above 3 weeks
                    If ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "1" Or ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "2" Or ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "3" Or ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString.Substring(1, 1) = "5" Then
                        If ds2.Tables(0).Rows(i).Item("KD").ToString.Trim = "I" Then
                            intercompanyUnitsAndBuyoutsCount += 1
                            If Not weekDiff >= -1 Then
                                If ds2.Tables(0).Rows(i).Item("Late") Then

                                End If
                            Else
                                intercompanyUnitsAndBuyoutsOTP += 1
                            End If

                            If Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Prom")) AndAlso Not DBNull.Value.Equals(ds2.Tables(0).Rows(i).Item("Deliv")) Then
                                weekDiff = Integer.Parse(ds2.Tables(0).Rows(i).Item("Prom")) - Integer.Parse(ds2.Tables(0).Rows(i).Item("Deliv"))
                            Else
                                weekDiff = 0
                            End If
                            If weekDiff <= -3 Then
                                intercompanyUnitsAndBuyouts3Weeks += 1
                            ElseIf weekDiff > -3 Then
                                intercompanyUnitsAndBuyoutsUntil3Weeks += 1
                            End If

                        End If
                    End If


                    'While looping we have to parse the details as well

                End If

                'white?
                Dim strWhite As String = ""
                If i Mod 2 = 0 Then
                    strWhite = " bgcolor = 'White' "
                End If

                strDetails += "<tr " & strWhite & ">"
                strDetails += "<td>"
                strDetails += ds2.Tables(0).Rows(i).Item("ORDER_NUMBER").ToString
                strDetails += "</td>"

                strDetails += "<td>"
                strDetails += ds2.Tables(0).Rows(i).Item("customer_name").ToString
                strDetails += "</td>"

                strDetails += "<td>"
                strDetails += ds2.Tables(0).Rows(i).Item("KD").ToString
                strDetails += "</td>"

                strDetails += "<td>"
                strDetails += ds2.Tables(0).Rows(i).Item("Billing").ToString
                strDetails += "</td>"

                strDetails += "<td>"
                strDetails += ds2.Tables(0).Rows(i).Item("PROM").ToString
                strDetails += "</td>"

                strDetails += "<td>"
                strDetails += ds2.Tables(0).Rows(i).Item("DELIV").ToString
                strDetails += "</td>"

                strDetails += "<td>"
                strDetails += ds2.Tables(0).Rows(i).Item("LATE").ToString
                strDetails += "</td>"

                strDetails += "<td>"
                strDetails += ds2.Tables(0).Rows(i).Item("ANZAHL").ToString
                strDetails += "</td>"

                strDetails += "</tr>"


            Next

            strDetails += " </table>"



            dboEDC.Disconnect()
            'Get the details of bookings and billings


            'Now create a table with the details in it.
            Dim sOTP As String = "<table width='800' bgcolor='#6699ff'> "
            sOTP += "<tr>"

            sOTP += "<td>"

            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += "Deliveries"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += "Deliveries On Time"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += "Percentage OTP"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += "Until 3 Weeks"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += "Above 3 Weeks"
            sOTP += "</td>"

            sOTP += "</tr>"

            'Now the values

            'Overall
            sOTP += "<tr bgcolor='White'>"

            sOTP += "<td>"
            sOTP += "Overall"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(overCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += overOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(overOTP / overCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(overUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(over3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"


            'Customer Parts
            sOTP += "<tr >"

            sOTP += "<td>"
            sOTP += "Customer Parts"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custPartsCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += custPartsOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(custPartsOTP / custPartsCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custPartsUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custParts3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"

            'Customer Repair
            sOTP += "<tr bgcolor='White'>"

            sOTP += "<td>"
            sOTP += "Customer Repair"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custRepairCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += custRepairOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(custRepairOTP / custRepairCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custRepairUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custRepair3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"

            'Customer Service
            sOTP += "<tr >"

            sOTP += "<td>"
            sOTP += "Customer Service"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custServiceCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += custServiceOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(custServiceOTP / custServiceCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custServiceUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custService3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"

            'Customer UnitsAndBuyouts
            sOTP += "<tr bgcolor='White'>"

            sOTP += "<td>"
            sOTP += "Customer Units And Buyouts"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custUnitsAndBuyoutsCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += custUnitsAndBuyoutsOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(custUnitsAndBuyoutsOTP / custUnitsAndBuyoutsCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custUnitsAndBuyoutsUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(custUnitsAndBuyouts3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"

            '######################################### Intercompany ##################################

            sOTP += "<tr >"

            sOTP += "<td>"
            sOTP += "Intercompanyomer Parts"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyPartsCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += intercompanyPartsOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(intercompanyPartsOTP / intercompanyPartsCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyPartsUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyParts3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"

            'Intercompanyomer Repair
            sOTP += "<tr bgcolor='White'>"

            sOTP += "<td>"
            sOTP += "Intercompanyomer Repair"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyRepairCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += intercompanyRepairOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(intercompanyRepairOTP / intercompanyRepairCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyRepairUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyRepair3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"

            'Intercompanyomer Service
            sOTP += "<tr >"

            sOTP += "<td>"
            sOTP += "Intercompanyomer Service"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyServiceCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += intercompanyServiceOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(intercompanyServiceOTP / intercompanyServiceCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyServiceUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyService3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"

            'Intercompanyomer UnitsAndBuyouts
            sOTP += "<tr bgcolor='White'>"

            sOTP += "<td>"
            sOTP += "Intercompanyomer Units And Buyouts"
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyUnitsAndBuyoutsCount, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += intercompanyUnitsAndBuyoutsOTP.ToString
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatPercent(intercompanyUnitsAndBuyoutsOTP / intercompanyUnitsAndBuyoutsCount, 2, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyUnitsAndBuyoutsUntil3Weeks, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "<td>"
            sOTP += FormatNumber(intercompanyUnitsAndBuyouts3Weeks.ToString, 0, TriState.True, TriState.True, TriState.True)
            sOTP += "</td>"

            sOTP += "</tr>"

            sOTP += " </table>"

            sBody += sOTP

            sBody += strDetails

        Catch ex As Exception
        Finally
            dboEDC.Disconnect()

        End Try
        Return sBody
    End Function




    Private Function createBodyBookBillHTML(ByVal reportId As String) As String
        Dim sBody As String = ""
        Dim ds As DataSet = dbs.SecureQueryParams(" SELECT  [reportID],[description],[purpose],[keyPoints],[schedule],[sendTo],[lastRun] ,[countRuns] " & _
                " FROM [warehouseUserReports] WHERE reportID = @1 ", reportId)


        sBody = "<table width='500' bgcolor='#6699ff'> " &
                "    <tr> " &
                "        <td class='style4' colspan='2' bgcolor='White'> " &
                " Business Cockpit - Auto E-Mail Report " &
                "    </td> " &
                "    </tr> " &
                "    <tr> " &
                "        <td class='style4' > " &
                " Description</td> " &
                "       <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("description") & " </td> " &
                "    </tr> " &
                "    <tr bgcolor='White'> " &
                "        <td class='style4'> " &
                " Key Points</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("keyPoints") & "</td> " &
                "    </tr> " &
                "    <tr>  " &
                "        <td class='style4'> " &
                "  Purpose</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("purpose") & "</td> " &
                "    </tr> " &
                "    <tr bgcolor='White'> " &
                "        <td class='style4'> " &
                " Date</td> " &
                "        <td class='style3'> " &
                " " & FormatDateTime(Now, DateFormat.ShortDate) & "</td> " &
                "    </tr> " &
                "    <tr> " &
                "        <td class='style4'> " &
                " Schedule</td> " &
                "        <td class='style3'> " &
                " " & ds.Tables(0).Rows(0).Item("schedule") & "</td> " &
                "    </tr> " &
                " </table> <br>"

        'Get the details of bookings and billings
        Dim sBookings As String = "<table width='800' bgcolor='#6699ff'> " & _
                " <tr bgcolor='White'>Bookings<td></td> <td>Complete</td> <td>Driver</td> <td>Parts</td> <td>Repair</td><td>Spare</td> <td>Global</td> </tr> "

        Dim strDataDimension As String = "[Day] >  [dbo].[ufn_GetFirstDayOfMonth](getDate()) "

        Dim strPeriodYear As String = DatePart(DateInterval.Year, Now)
        Dim strPeriodNum As String = DatePart(DateInterval.Month, Now)

        'For testing purposes we can overwrite these settings
        'strPeriodYear = "2018"
        'strPeriodNum = "12"

        strDataDimension = " PERIOD_YEAR = '" & strPeriodYear & "' AND PERIOD_NUM = '" & strPeriodNum & "'"
        'Interco

        Dim ds3 As DataSet = dbs.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [BBBBookingsAggregatView] " & _
                                    "    WHERE organization_id = 255 AND customer_type_code = 'I' And  " & strDataDimension & _
                                      " ")

        sBookings += "<tr><td>Intercompany:</td>"
        Try

            sBookings += "<td align='right'>" & FormatNumber(ds3.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds3.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds3.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds3.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds3.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds3.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "</tr>"

        Catch ex As Exception

        End Try

        'Customer

        Dim ds4 As DataSet = dbs.Query(" SELECT " & _
                                   "   " & _
                                   "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                   " ,SUM([DriverPrice]) DriverPrice " & _
                                   " ,SUM([partsPrice])  partsPrice " & _
                                   " ,SUM([repairPrice]) repairPrice " & _
                                    " ,SUM([sparePrice])  sparePrice " & _
                                   "    FROM [BBBBookingsAggregatView] " & _
                                   "    WHERE organization_id = 255 AND customer_type_code = 'R' And  " & strDataDimension & _
                                     " ")

        sBookings += "<tr bgcolor='White'><td>Customer:</td>"

        Try
            sBookings += "<td align='right'>" & FormatNumber(ds4.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds4.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds4.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds4.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds4.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds4.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True) & "</td>"

            sBookings += "</tr>"

        Catch ex As Exception

        End Try

        'Add the aggregat for the values:
        Dim ds2 As DataSet = dbs.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBookingsAggregatView] " & _
                                    "    WHERE organization_id = 255 And  " & strDataDimension & _
                                      " ")

        sBookings += "<tr><td>Total:</td>"

        Try
            sBookings += "<td align='right'>" & FormatNumber(ds2.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds2.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds2.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds2.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds2.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBookings += "<td align='right'>" & FormatNumber(ds2.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True) & "</td>"

            sBookings += "</tr>"

        Catch ex As Exception

        End Try

        sBookings += " </table><br>"

        sBody += sBookings

        Dim sBillings As String = "<table width='800' bgcolor='#6699ff'> " & _
                " <tr bgcolor='White'>Billings<td></td> <td>Complete</td> <td>Driver</td> <td>Parts</td> <td>Repair</td><td>Spare</td> <td>Global</td> </tr> "


        'Add the billing functionality
        Dim ds6 As DataSet = dbs.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBillingsAggregatView] " & _
                                    "    WHERE organization_id = 255 AND customer_type_code = 'I'  And  " & strDataDimension & _
                                      " ")
        Try
            sBillings += "<tr><td>Intercompany:</td>"

            sBillings += "<td align='right'>" & FormatNumber(ds6.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds6.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds6.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds6.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds6.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds6.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True) & "</td>"

            sBillings += "</tr>"
        Catch ex As Exception

        End Try

        'Add the billing functionality
        Dim ds7 As DataSet = dbs.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBillingsAggregatView] " & _
                                    "    WHERE organization_id = 255 AND customer_type_code = 'R'  And  " & strDataDimension & _
                                      " ")
        Try
            sBillings += "<tr bgcolor='White'><td>Customer:</td>"

            sBillings += "<td align='right'>" & FormatNumber(ds7.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds7.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds7.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds7.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds7.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds7.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True) & "</td>"

            sBillings += "</tr>"

        Catch ex As Exception

        End Try

        'Add the aggregat for the values:
        Dim ds5 As DataSet = dbs.Query(" SELECT " & _
                                    "   " & _
                                    "  SUM([totalPrice]) total ,SUM([completePrice])  completePrice" & _
                                    " ,SUM([DriverPrice]) DriverPrice " & _
                                    " ,SUM([partsPrice])  partsPrice " & _
                                    " ,SUM([repairPrice]) repairPrice " & _
                                     " ,SUM([sparePrice])  sparePrice " & _
                                    "    FROM [FSG_IND_WORKBENCH].[dbo].[BBBBillingsAggregatView] " & _
                                    "    WHERE organization_id = 255 And  " & strDataDimension & _
                                      " ")
        sBillings += "<tr><td>Total:</td>"
        Try

            sBillings += "<td align='right'>" & FormatNumber(ds5.Tables(0).Rows(0).Item("completePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds5.Tables(0).Rows(0).Item("DriverPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds5.Tables(0).Rows(0).Item("partsPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds5.Tables(0).Rows(0).Item("repairPrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds5.Tables(0).Rows(0).Item("sparePrice"), 2, TriState.True, TriState.True, TriState.True) & "</td>"
            sBillings += "<td align='right'>" & FormatNumber(ds5.Tables(0).Rows(0).Item("total"), 2, TriState.True, TriState.True, TriState.True) & "</td>"

            sBillings += "</tr>"

        Catch ex As Exception

        End Try

        sBillings += " </table>"

        sBody += sBillings

        Return sBody

    End Function


    Private Function createBodyProblemHTML(ByVal description As String, ByVal errorLine As String, ByVal noOfSteps As String, ByVal procedureOrFunction As String, ByVal invokedBy As String, ByVal creationDate As String, ByVal causingObject As String, ByVal InnerException As String) As String
        Dim sBody As String = ""



        sBody = "<table width='500' bgcolor='#6699ff'> " & _
                "    <tr> " & _
                "        <td class='style4' colspan='2' bgcolor='White'> " & _
                "Autobahn - Auto E-Mail Report " & _
                "    </td> " & _
                "    </tr> " & _
                "    <tr> " & _
                "        <td class='style4' > " & _
                " Description</td> " & _
                "       <td class='style3'> " & _
                " " & description & " </td> " & _
                "    </tr> " & _
                "    <tr bgcolor='White'> " & _
                "        <td class='style4'> " & _
                " Error Line</td> " & _
                "        <td class='style3'> " & _
                " " & errorLine & "</td> " & _
                "    </tr> " & _
                "    <tr>  " & _
                "        <td class='style4'> " & _
                "  No of Steps </td> " & _
                "        <td class='style3'> " & _
                " " & noOfSteps & "</td> " & _
                "    </tr> " & _
                "    <tr bgcolor='White'> " & _
                "        <td class='style4'> " & _
                " Date</td> " & _
                "        <td class='style3'> " & _
                " " & FormatDateTime(Now, DateFormat.ShortDate) & "</td> " & _
                "    </tr> " & _
                "    <tr> " & _
                "        <td class='style4'> " & _
                " Procedure or Function</td> " & _
                "        <td class='style3'> " & _
                " " & procedureOrFunction & "</td> " & _
                "    </tr> " & _
                 "    <tr> " & _
                "        <td class='style4'> " & _
                " Invoked By</td> " & _
                "        <td class='style3'> " & _
                " " & invokedBy & "</td> " & _
                "    </tr> " & _
                 "    <tr> " & _
                "        <td class='style4'> " & _
                " Causing Object</td> " & _
                "        <td class='style3'> " & _
                " " & causingObject & "</td> " & _
                "    </tr> " & _
                "    <tr> " & _
                "        <td class='style4'> " & _
                " Inner Exception</td> " & _
                "        <td class='style3'> " & _
                " " & InnerException & "</td> " & _
                "    </tr> " & _
                " </table>"


        Return sBody

    End Function

End Class
