Imports System.Windows.Forms

Public Class dlgTwoGridsDisplay

    Private dbS As New DB.ServerDB(My.Settings("FlowConnectionString"), False)  'connect to SQL Server database  
    Private _orderNo As Integer
    Private _lineNo As Integer
    Private _organizationID As Integer
    Private gridDisplay As New clsGridDisplay


    Public Sub New(ByVal strSQLQuery As String, ByRef dsValues As DataSet, ByVal strIndicatorName As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        dgvHeader.DataSource = dsValues.Tables(0)
        txtReportMessages.Text = strSQLQuery

    End Sub
    ''' <summary>
    ''' Incarnation for Nafta report
    ''' </summary>
    ''' <param name="organizationID">The organization ID for the report.</param>
    ''' <param name="orderNo">The order number to calculate NAFTA for.</param>
    ''' <param name="lineNo">The line number we want to go for.</param>
    ''' <remarks>Import for US regulations.</remarks>
    Public Sub New(ByVal organizationID As Integer, ByVal orderNo As Integer, ByVal lineNo As Integer)
        _orderNo = orderNo
        _lineNo = lineNo
        _organizationID = organizationID


        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = "NAFTA Report for " & _orderNo & " Line: " & _lineNo
        '**** NILS: SOMETHING LIKE THIS WOULD GO IN YOUR CLICK EVENT AFTER THIS SUB FINISHES
        'we need to display the following information for the user on either grid shortage, or the misc grid, and allow them to output to XML report or copy to clipboard
        'Main header:   sales order header, line, Item number, qty ordered, Sales Price per unit (a), non-Nafta Content (b),  Nafta Percent  (c) = (a - b) / a, Qualifies (Yes if c >= .6) 

        ' calculate (b) non-Nafta content as the sum of all purchased parts costs (qty per x Nafta cost) where Nafta code is null or nafta code not like 'Y'
        'dsS = dbS.Query("SELECT SUM(Number8 * Number2) AS nonNaftaContent FROM dbo.tblWork WHERE (Number3 = 2) AND (Number8 > 0) AND ([Text4] NOT LIKE 'Y' OR [Text4] IS NULL) ")
        'dblTmp = dsS.Tables(0).Rows(0).Item("nonNaftaContent")  'THIS IS THE TOTAL NON NAFTA $ VALUE OF THE SALES ORDER LINE

        'Detail Header:  Component Item, Description, Qty Per, P/M code, NAFTA Cost, Nafta Code, Seiban PO.
        'SELECT     Text1 AS item_no, Text2 AS description, Number2 AS qty_per, Number3 AS make_buy, Number8 AS NAFTA_Cost, Text4 AS NAFTA_code, Text5 AS seiban_PO_no
        'FROM dbO.tblWork
        Try
            Dim ds As New DataSet


            '- column 6:  non-nafta content
            '- column 7:  Nafta percent  ---> =  round((Selling Price - non-nafta content)/selling price,2)  
            '- column 8:  Qualifies    --->  Yes if column 7  >= .6, otherwise N

            ds = dbS.SecureQueryParams("SELECT OrderNoLine [Cust Order], item_No [Item No.], Description, ordered_Quantity [Ordered Quantity], unit_Selling_Price [Unit Selling Price] " & _
                                       ",NULL [Non-nafta content], NULL [Nafta percent], NULL [Qualifies] " & _
                           " FROM salesOrderLinesView WHERE organization_id = @1 AND order_no = @2 AND line_no = @3 ", Me._organizationID, Me._orderNo, Me._lineNo)

            dgvHeader.DataSource = ds.Tables(0)

            Dim ds2 As New DataSet

            ds2 = dbS.SecureQueryParams("SELECT SUM(Number8 * Number2) AS nonNaftaContent FROM dbo.tblWork WHERE (Number3 = 2) AND (Number8 > 0) AND ([Text4] NOT LIKE 'Y' OR [Text4] IS NULL)")

            'calculating the three extra column's in the sales order grid
            ds.Tables(0).Rows(0).Item("Non-nafta content") = Format(ds2.Tables(0).Rows(0).Item(0), "C1")
            Dim dblNaftaPercent As Double = ds2.Tables(0).Rows(0).Item(0) / ds.Tables(0).Rows(0).Item("Non-nafta content")
            ds.Tables(0).Rows(0).Item("Nafta percent") = FormatPercent(dblNaftaPercent, 2)

            'calculate the percentage according to the unit selling price


            If Not DBNull.Value.Equals(ds2.Tables(0).Rows(0).Item("nonNaftaContent")) Then
                Dim dblTmp As Double = ds2.Tables(0).Rows(0).Item("nonNaftaContent")  'THIS IS THE TOTAL NON NAFTA $ VALUE OF THE SALES ORDER LINE
                lblNonNaftaContent.Text = dblTmp.ToString
            Else
                lblNonNaftaContent.Text = "Nothing"
            End If

            Dim ds3 As New DataSet
            ds3 = dbS.Query("SELECT     Text1 AS [Item No.], Text2 AS Description, Number2 AS [Qty Per], Number3 AS [Make Buy], Number8 AS [NAFTA Cost], Text4 AS [NAFTA code], Text5 AS [Seiban PO No.] " & _
                            "FROM dbO.tblWork ")

            dgvFooter.DataSource = ds3.Tables(0)
            gridDisplay.formatGrid(dgvFooter, "test")
            gridDisplay.formatGrid(dgvHeader, "test")


        Catch ex As Exception

        End Try


    End Sub

    ''' <summary>
    ''' Checks for buy items on the sales order line item or below it for country of origin check.
    ''' </summary>
    ''' <param name="organizationId">The organization id for this report is currently HMB.</param>
    ''' <param name="salesOrderHeader">The sales order header which should be checked.</param>
    ''' <param name="numberOfDisplayedPO">Quantity of purchase orders which should be displayed for the item.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal organizationId As Integer, ByVal salesOrderHeader As Double, ByVal numberOfDisplayedPO As Integer, ByVal official As Integer, ByVal salesorderNumber As Integer, ByVal organizationName As String)

        InitializeComponent()
        Me.Text = "Country of Origin Check for: " & salesorderNumber.ToString & " Shipping from: " & organizationName

        lblTextInformation.Text = "Possible buy items from non-eu countries:"
        'Load the right information into the top grid
        Dim ds As New DataSet
        ds = dbS.SecureQueryParams("SELECT order_no [Order Number],inventory_item_id, line_no [Line Number], item_no [Item Number], Description " & _
        ", (SELECT CASE WHEN planning_make_buy_code = 1 THEN 'Make' ELSE 'Buy' END FROM active_items WHERE salesOrderLinesView.organization_id = active_items.organization_id and salesOrderLinesView.inventory_item_id = active_items.inventory_item_id) As [Make Buy] " & _
        " FROM salesOrderLinesView WHERE header_id = @1 AND organization_id = @2 ORDER BY line_no", salesOrderHeader, organizationId)


        dgvHeader.DataSource = ds.Tables(0)
        dgvHeader.Columns("inventory_item_id").Visible = False
        gridDisplay.formatGrid(dgvHeader, "")
        'Distinct between the open planning of mrp and the transaction history of purchase orders.
        'Display purchase orders just for the buy items on the order lines
        Dim strInventoryItems As String = "("
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            If ds.Tables(0).Rows(i).Item("Make Buy") = "Buy" Then
                If i < ds.Tables(0).Rows.Count AndAlso strInventoryItems.Count > 1 Then
                    strInventoryItems += ","
                End If
                strInventoryItems += ds.Tables(0).Rows(i).Item("inventory_item_id").ToString
            End If
        Next
        strInventoryItems += ")"

        If strInventoryItems.Count > 2 Then
            Dim ds2 As New DataSet
            ds2 = dbS.SecureQueryParams("SELECT po_no, line_no, item_no, [item_description], quantity_ordered, vendor_name FROM POQueueView WHERE inventory_item_id IN " & strInventoryItems & " AND ship_to_organization_id = @1", organizationId)
            dgvFooter.DataSource = ds2.Tables(0)
            gridDisplay.formatGrid(dgvFooter, "")
        End If


        'First thing is to look if the sales order line item is already a buy item.

        'Go into the BOM and look for buy items.

    End Sub


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgNaftaCheck_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Loading the two grids into double buffered memory in order to accelerate speed in scrolling
        gridDisplay.DoubleBuffered(dgvFooter, True)
        gridDisplay.DoubleBuffered(dgvHeader, True)
    End Sub
End Class
