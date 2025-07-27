Imports System.Windows.Forms

Public Class dlgOneGridConfirm
    Private _bNafta As Boolean

    Public Sub New(ByRef ds As DataSet, ByVal nafta As Boolean)

        _bNafta = nafta
        InitializeComponent()
        dgvCurrentData.DataSource = ds.Tables(0)

        dgvCurrentData.Columns("itemNo").HeaderText = "Item"
        dgvCurrentData.Columns("project_id").Visible = False
        dgvCurrentData.Columns("planning_make_buy_code").Visible = False
        dgvCurrentData.Columns("planning_make_buy_code").Visible = False
        dgvCurrentData.Columns("unit_selling_price").Visible = False
        dgvCurrentData.Columns("line_no").Visible = False
        dgvCurrentData.Columns("description").HeaderText = "Description"
        dgvCurrentData.Columns("inventory_item_id").Visible = False
        dgvCurrentData.Columns("vendorName").Visible = False
        dgvCurrentData.Columns("vendorItem").Visible = False
        dgvCurrentData.Columns("description1").Visible = False
        dgvCurrentData.Columns("productCode").Visible = False
        dgvCurrentData.Columns("drawing").Visible = False
        dgvCurrentData.Columns("vendorNo").Visible = False
        dgvCurrentData.Columns("planner").Visible = False
        dgvCurrentData.Columns("tariff").HeaderText = "Tariff"
        dgvCurrentData.Columns("prefCriterion").HeaderText = "Preference Criterion"
        dgvCurrentData.Columns("producer").HeaderText = "Producer"
        dgvCurrentData.Columns("netCost").HeaderText = "Net Cost"
        dgvCurrentData.Columns("countryOfOrigin").HeaderText = "Country Of Origin"
        dgvCurrentData.Columns("comments").Visible = False
        dgvCurrentData.Columns("NAFTAPart").Visible = False

    End Sub


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If _bNafta Then

        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub saveAndInsertNAFTA()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


End Class
