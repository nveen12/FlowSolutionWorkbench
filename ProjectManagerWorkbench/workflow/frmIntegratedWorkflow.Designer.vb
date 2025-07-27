<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIntegratedWorkflow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmIntegratedWorkflow))
        Me.tblWorkBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.dsWork = New Flow_Solution_Workbenches.dsWork()
        Me.tcWorkbenches = New System.Windows.Forms.TabControl()
        Me.tpOrderManagement = New System.Windows.Forms.TabPage()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.btnOTP = New System.Windows.Forms.Button()
        Me.btnBookAndBill = New System.Windows.Forms.Button()
        Me.btnBacklog = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.btnSORC0 = New System.Windows.Forms.Button()
        Me.btnSORC7 = New System.Windows.Forms.Button()
        Me.btnSORC6 = New System.Windows.Forms.Button()
        Me.btnSORC5 = New System.Windows.Forms.Button()
        Me.btnSORC4 = New System.Windows.Forms.Button()
        Me.btnSORC3 = New System.Windows.Forms.Button()
        Me.btnSORC2 = New System.Windows.Forms.Button()
        Me.btnSORC1 = New System.Windows.Forms.Button()
        Me.btnTrendNumberLatePlannedSO = New System.Windows.Forms.Button()
        Me.btnTrendNumberSupplyProblems = New System.Windows.Forms.Button()
        Me.btnTrendShippedSalesOrderLines = New System.Windows.Forms.Button()
        Me.btnTrendNewSalesOrderLines = New System.Windows.Forms.Button()
        Me.btnTrendValueOpenSalesOrderLines = New System.Windows.Forms.Button()
        Me.btnTrendNumberOfOpenSalesOrderLines = New System.Windows.Forms.Button()
        Me.btnSOBySeverity = New System.Windows.Forms.Button()
        Me.btnSOByDateRange = New System.Windows.Forms.Button()
        Me.btnSOByOrder = New System.Windows.Forms.Button()
        Me.btnSOByValue = New System.Windows.Forms.Button()
        Me.btnSOByProduct = New System.Windows.Forms.Button()
        Me.btnSOByCustomer = New System.Windows.Forms.Button()
        Me.btnSOCountProblems = New System.Windows.Forms.Button()
        Me.btnSalesOrderLinesProblems = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.btnSOLinesWithoutBOMCompletion = New System.Windows.Forms.Button()
        Me.btnSOwithoutSSD = New System.Windows.Forms.Button()
        Me.btnSOShipNextDays = New System.Windows.Forms.Button()
        Me.btnSOShipToday = New System.Windows.Forms.Button()
        Me.btnSOLatePlannedLines = New System.Windows.Forms.Button()
        Me.btnSOSupplyProblems = New System.Windows.Forms.Button()
        Me.btnSOShippedLines = New System.Windows.Forms.Button()
        Me.btnSONewLines = New System.Windows.Forms.Button()
        Me.btnSOValueLines = New System.Windows.Forms.Button()
        Me.btnSoNumberLines = New System.Windows.Forms.Button()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.tpPurchasing = New System.Windows.Forms.TabPage()
        Me.rtbLiveTicker = New System.Windows.Forms.RichTextBox()
        Me.btnTrendRC4 = New System.Windows.Forms.Button()
        Me.btnTrendRC3 = New System.Windows.Forms.Button()
        Me.btnTrendRC2 = New System.Windows.Forms.Button()
        Me.btnTrendRC1 = New System.Windows.Forms.Button()
        Me.btnTrendUsedSuppliers = New System.Windows.Forms.Button()
        Me.btnTrendRequests = New System.Windows.Forms.Button()
        Me.txtRC4 = New System.Windows.Forms.TextBox()
        Me.btnRC4Severity = New System.Windows.Forms.Button()
        Me.btnRC4Item = New System.Windows.Forms.Button()
        Me.btnRC4Order = New System.Windows.Forms.Button()
        Me.btnRC4Supplier = New System.Windows.Forms.Button()
        Me.btnRC4Count = New System.Windows.Forms.Button()
        Me.btnReasonCode4 = New System.Windows.Forms.Button()
        Me.txtRC3 = New System.Windows.Forms.TextBox()
        Me.btnRC3Severity = New System.Windows.Forms.Button()
        Me.btnRC3Item = New System.Windows.Forms.Button()
        Me.btnRC3Order = New System.Windows.Forms.Button()
        Me.btnRC3Supplier = New System.Windows.Forms.Button()
        Me.btnRC3Count = New System.Windows.Forms.Button()
        Me.btnReasonCode3 = New System.Windows.Forms.Button()
        Me.txtRC2 = New System.Windows.Forms.TextBox()
        Me.btnRC2Severity = New System.Windows.Forms.Button()
        Me.btnRC2Item = New System.Windows.Forms.Button()
        Me.btnRC2Order = New System.Windows.Forms.Button()
        Me.btnRC2Supplier = New System.Windows.Forms.Button()
        Me.btnRC2Count = New System.Windows.Forms.Button()
        Me.btnReasonCode2 = New System.Windows.Forms.Button()
        Me.txtLastReviewRC1 = New System.Windows.Forms.TextBox()
        Me.btnRC1Severity = New System.Windows.Forms.Button()
        Me.btnRC1Item = New System.Windows.Forms.Button()
        Me.btnRC1Supplier = New System.Windows.Forms.Button()
        Me.btnRC1Count = New System.Windows.Forms.Button()
        Me.btnReasonCode1 = New System.Windows.Forms.Button()
        Me.btnMaterialReceiptsYesterday = New System.Windows.Forms.Button()
        Me.btnNumberPOLinesCreated = New System.Windows.Forms.Button()
        Me.btnCurrentSupplyProblems = New System.Windows.Forms.Button()
        Me.btnUsedSuppliers = New System.Windows.Forms.Button()
        Me.btnTotalValuePo = New System.Windows.Forms.Button()
        Me.btnNoOfOpenPOs = New System.Windows.Forms.Button()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnDiscussionTracking = New System.Windows.Forms.Button()
        Me.dgvAggregation = New System.Windows.Forms.DataGridView()
        Me.btnRC1Order = New System.Windows.Forms.Button()
        Me.tpScheduling = New System.Windows.Forms.TabPage()
        Me.tpBacklog = New System.Windows.Forms.TabPage()
        Me.tpBookAndBill = New System.Windows.Forms.TabPage()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tpOTP = New System.Windows.Forms.TabPage()
        Me.tpRevenue = New System.Windows.Forms.TabPage()
        Me.ttExplanations = New System.Windows.Forms.ToolTip(Me.components)
        Me.tblWorkTableAdapter = New Flow_Solution_Workbenches.dsWorkTableAdapters.tblWorkTableAdapter()
        Me.pf1 = New Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(Me.components)
        Me.rvBacklog = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.rvRevenuePlanning = New Microsoft.Reporting.WinForms.ReportViewer()
        CType(Me.tblWorkBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dsWork, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcWorkbenches.SuspendLayout()
        Me.tpOrderManagement.SuspendLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpPurchasing.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvAggregation, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpBacklog.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.tpRevenue.SuspendLayout()
        Me.SuspendLayout()
        '
        'tblWorkBindingSource
        '
        Me.tblWorkBindingSource.DataMember = "tblWork"
        Me.tblWorkBindingSource.DataSource = Me.dsWork
        '
        'dsWork
        '
        Me.dsWork.DataSetName = "dsWork"
        Me.dsWork.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'tcWorkbenches
        '
        Me.tcWorkbenches.Controls.Add(Me.tpOrderManagement)
        Me.tcWorkbenches.Controls.Add(Me.tpPurchasing)
        Me.tcWorkbenches.Controls.Add(Me.tpScheduling)
        Me.tcWorkbenches.Controls.Add(Me.tpBacklog)
        Me.tcWorkbenches.Controls.Add(Me.tpBookAndBill)
        Me.tcWorkbenches.Controls.Add(Me.tpOTP)
        Me.tcWorkbenches.Controls.Add(Me.tpRevenue)
        Me.tcWorkbenches.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcWorkbenches.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.tcWorkbenches.Location = New System.Drawing.Point(0, 0)
        Me.tcWorkbenches.Margin = New System.Windows.Forms.Padding(4)
        Me.tcWorkbenches.Name = "tcWorkbenches"
        Me.tcWorkbenches.SelectedIndex = 0
        Me.tcWorkbenches.Size = New System.Drawing.Size(1371, 918)
        Me.tcWorkbenches.TabIndex = 0
        '
        'tpOrderManagement
        '
        Me.tpOrderManagement.BackColor = System.Drawing.Color.LightYellow
        Me.tpOrderManagement.Controls.Add(Me.Button9)
        Me.tpOrderManagement.Controls.Add(Me.Button6)
        Me.tpOrderManagement.Controls.Add(Me.btnOTP)
        Me.tpOrderManagement.Controls.Add(Me.btnBookAndBill)
        Me.tpOrderManagement.Controls.Add(Me.btnBacklog)
        Me.tpOrderManagement.Controls.Add(Me.Button1)
        Me.tpOrderManagement.Controls.Add(Me.Button2)
        Me.tpOrderManagement.Controls.Add(Me.Button3)
        Me.tpOrderManagement.Controls.Add(Me.btnSORC0)
        Me.tpOrderManagement.Controls.Add(Me.btnSORC7)
        Me.tpOrderManagement.Controls.Add(Me.btnSORC6)
        Me.tpOrderManagement.Controls.Add(Me.btnSORC5)
        Me.tpOrderManagement.Controls.Add(Me.btnSORC4)
        Me.tpOrderManagement.Controls.Add(Me.btnSORC3)
        Me.tpOrderManagement.Controls.Add(Me.btnSORC2)
        Me.tpOrderManagement.Controls.Add(Me.btnSORC1)
        Me.tpOrderManagement.Controls.Add(Me.btnTrendNumberLatePlannedSO)
        Me.tpOrderManagement.Controls.Add(Me.btnTrendNumberSupplyProblems)
        Me.tpOrderManagement.Controls.Add(Me.btnTrendShippedSalesOrderLines)
        Me.tpOrderManagement.Controls.Add(Me.btnTrendNewSalesOrderLines)
        Me.tpOrderManagement.Controls.Add(Me.btnTrendValueOpenSalesOrderLines)
        Me.tpOrderManagement.Controls.Add(Me.btnTrendNumberOfOpenSalesOrderLines)
        Me.tpOrderManagement.Controls.Add(Me.btnSOBySeverity)
        Me.tpOrderManagement.Controls.Add(Me.btnSOByDateRange)
        Me.tpOrderManagement.Controls.Add(Me.btnSOByOrder)
        Me.tpOrderManagement.Controls.Add(Me.btnSOByValue)
        Me.tpOrderManagement.Controls.Add(Me.btnSOByProduct)
        Me.tpOrderManagement.Controls.Add(Me.btnSOByCustomer)
        Me.tpOrderManagement.Controls.Add(Me.btnSOCountProblems)
        Me.tpOrderManagement.Controls.Add(Me.btnSalesOrderLinesProblems)
        Me.tpOrderManagement.Controls.Add(Me.Button7)
        Me.tpOrderManagement.Controls.Add(Me.Button8)
        Me.tpOrderManagement.Controls.Add(Me.btnSOLinesWithoutBOMCompletion)
        Me.tpOrderManagement.Controls.Add(Me.btnSOwithoutSSD)
        Me.tpOrderManagement.Controls.Add(Me.btnSOShipNextDays)
        Me.tpOrderManagement.Controls.Add(Me.btnSOShipToday)
        Me.tpOrderManagement.Controls.Add(Me.btnSOLatePlannedLines)
        Me.tpOrderManagement.Controls.Add(Me.btnSOSupplyProblems)
        Me.tpOrderManagement.Controls.Add(Me.btnSOShippedLines)
        Me.tpOrderManagement.Controls.Add(Me.btnSONewLines)
        Me.tpOrderManagement.Controls.Add(Me.btnSOValueLines)
        Me.tpOrderManagement.Controls.Add(Me.btnSoNumberLines)
        Me.tpOrderManagement.Controls.Add(Me.PictureBox6)
        Me.tpOrderManagement.Controls.Add(Me.PictureBox5)
        Me.tpOrderManagement.Controls.Add(Me.PictureBox4)
        Me.tpOrderManagement.Location = New System.Drawing.Point(4, 26)
        Me.tpOrderManagement.Margin = New System.Windows.Forms.Padding(4)
        Me.tpOrderManagement.Name = "tpOrderManagement"
        Me.tpOrderManagement.Padding = New System.Windows.Forms.Padding(4)
        Me.tpOrderManagement.Size = New System.Drawing.Size(1363, 888)
        Me.tpOrderManagement.TabIndex = 0
        Me.tpOrderManagement.Text = "Central Order Planning Workbench"
        Me.tpOrderManagement.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.BackColor = System.Drawing.Color.Navy
        Me.Button9.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.Button9.FlatAppearance.BorderSize = 8
        Me.Button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.Button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button9.ForeColor = System.Drawing.Color.White
        Me.Button9.Location = New System.Drawing.Point(1065, 7)
        Me.Button9.Margin = New System.Windows.Forms.Padding(4)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(256, 68)
        Me.Button9.TabIndex = 52
        Me.Button9.Text = "Revenue versus Backlog"
        Me.ttExplanations.SetToolTip(Me.Button9, "Your current number of open sales orders.")
        Me.Button9.UseVisualStyleBackColor = False
        Me.Button9.Visible = False
        '
        'Button6
        '
        Me.Button6.BackColor = System.Drawing.Color.Navy
        Me.Button6.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.Button6.FlatAppearance.BorderSize = 8
        Me.Button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.Button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button6.ForeColor = System.Drawing.Color.White
        Me.Button6.Location = New System.Drawing.Point(801, 7)
        Me.Button6.Margin = New System.Windows.Forms.Padding(4)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(256, 68)
        Me.Button6.TabIndex = 51
        Me.Button6.Text = "Sales And Operation Planning"
        Me.ttExplanations.SetToolTip(Me.Button6, "Your current number of open sales orders.")
        Me.Button6.UseVisualStyleBackColor = False
        Me.Button6.Visible = False
        '
        'btnOTP
        '
        Me.btnOTP.BackColor = System.Drawing.Color.Navy
        Me.btnOTP.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnOTP.FlatAppearance.BorderSize = 8
        Me.btnOTP.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnOTP.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnOTP.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOTP.ForeColor = System.Drawing.Color.White
        Me.btnOTP.Location = New System.Drawing.Point(537, 7)
        Me.btnOTP.Margin = New System.Windows.Forms.Padding(4)
        Me.btnOTP.Name = "btnOTP"
        Me.btnOTP.Size = New System.Drawing.Size(256, 68)
        Me.btnOTP.TabIndex = 50
        Me.btnOTP.Text = "On Time Performance"
        Me.ttExplanations.SetToolTip(Me.btnOTP, "Your current number of open sales orders.")
        Me.btnOTP.UseVisualStyleBackColor = False
        '
        'btnBookAndBill
        '
        Me.btnBookAndBill.BackColor = System.Drawing.Color.Navy
        Me.btnBookAndBill.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnBookAndBill.FlatAppearance.BorderSize = 8
        Me.btnBookAndBill.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnBookAndBill.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnBookAndBill.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBookAndBill.ForeColor = System.Drawing.Color.White
        Me.btnBookAndBill.Location = New System.Drawing.Point(275, 7)
        Me.btnBookAndBill.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBookAndBill.Name = "btnBookAndBill"
        Me.btnBookAndBill.Size = New System.Drawing.Size(256, 68)
        Me.btnBookAndBill.TabIndex = 49
        Me.btnBookAndBill.Text = "Book And Bill"
        Me.ttExplanations.SetToolTip(Me.btnBookAndBill, "Your current number of open sales orders.")
        Me.btnBookAndBill.UseVisualStyleBackColor = False
        '
        'btnBacklog
        '
        Me.btnBacklog.BackColor = System.Drawing.Color.Navy
        Me.btnBacklog.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnBacklog.FlatAppearance.BorderSize = 8
        Me.btnBacklog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnBacklog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnBacklog.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBacklog.ForeColor = System.Drawing.Color.White
        Me.btnBacklog.Location = New System.Drawing.Point(11, 7)
        Me.btnBacklog.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBacklog.Name = "btnBacklog"
        Me.btnBacklog.Size = New System.Drawing.Size(256, 68)
        Me.btnBacklog.TabIndex = 48
        Me.btnBacklog.Text = "Backlog"
        Me.ttExplanations.SetToolTip(Me.btnBacklog, "Your current number of open sales orders.")
        Me.btnBacklog.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Navy
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.Button1.FlatAppearance.BorderSize = 8
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(536, 862)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(256, 68)
        Me.Button1.TabIndex = 47
        Me.Button1.Text = "Missed Billing Value Daily"
        Me.ttExplanations.SetToolTip(Me.Button1, "The number of your distinct suppliers used in the open purchase orders.")
        Me.Button1.UseVisualStyleBackColor = False
        Me.Button1.Visible = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Navy
        Me.Button2.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.Button2.FlatAppearance.BorderSize = 8
        Me.Button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(272, 862)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(256, 68)
        Me.Button2.TabIndex = 46
        Me.Button2.Text = "Billings Value"
        Me.ttExplanations.SetToolTip(Me.Button2, "The total value of the open sales orders.")
        Me.Button2.UseVisualStyleBackColor = False
        Me.Button2.Visible = False
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.Navy
        Me.Button3.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.Button3.FlatAppearance.BorderSize = 8
        Me.Button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.ForeColor = System.Drawing.Color.White
        Me.Button3.Location = New System.Drawing.Point(8, 862)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(256, 68)
        Me.Button3.TabIndex = 45
        Me.Button3.Text = "Bookings Values"
        Me.ttExplanations.SetToolTip(Me.Button3, "Your current number of open sales orders.")
        Me.Button3.UseVisualStyleBackColor = False
        Me.Button3.Visible = False
        '
        'btnSORC0
        '
        Me.btnSORC0.BackColor = System.Drawing.Color.Navy
        Me.btnSORC0.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSORC0.FlatAppearance.BorderSize = 8
        Me.btnSORC0.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSORC0.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSORC0.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSORC0.ForeColor = System.Drawing.Color.White
        Me.btnSORC0.Location = New System.Drawing.Point(176, 622)
        Me.btnSORC0.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSORC0.Name = "btnSORC0"
        Me.btnSORC0.Size = New System.Drawing.Size(141, 68)
        Me.btnSORC0.TabIndex = 44
        Me.btnSORC0.Text = "Reason Code 0"
        Me.btnSORC0.UseVisualStyleBackColor = False
        '
        'btnSORC7
        '
        Me.btnSORC7.BackColor = System.Drawing.Color.Navy
        Me.btnSORC7.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSORC7.FlatAppearance.BorderSize = 8
        Me.btnSORC7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSORC7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSORC7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSORC7.ForeColor = System.Drawing.Color.White
        Me.btnSORC7.Location = New System.Drawing.Point(1223, 622)
        Me.btnSORC7.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSORC7.Name = "btnSORC7"
        Me.btnSORC7.Size = New System.Drawing.Size(141, 68)
        Me.btnSORC7.TabIndex = 43
        Me.btnSORC7.Text = "Reason Code 7"
        Me.btnSORC7.UseVisualStyleBackColor = False
        '
        'btnSORC6
        '
        Me.btnSORC6.BackColor = System.Drawing.Color.Navy
        Me.btnSORC6.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSORC6.FlatAppearance.BorderSize = 8
        Me.btnSORC6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSORC6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSORC6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSORC6.ForeColor = System.Drawing.Color.White
        Me.btnSORC6.Location = New System.Drawing.Point(1072, 622)
        Me.btnSORC6.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSORC6.Name = "btnSORC6"
        Me.btnSORC6.Size = New System.Drawing.Size(141, 68)
        Me.btnSORC6.TabIndex = 42
        Me.btnSORC6.Text = "Reason Code 6"
        Me.btnSORC6.UseVisualStyleBackColor = False
        '
        'btnSORC5
        '
        Me.btnSORC5.BackColor = System.Drawing.Color.Navy
        Me.btnSORC5.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSORC5.FlatAppearance.BorderSize = 8
        Me.btnSORC5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSORC5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSORC5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSORC5.ForeColor = System.Drawing.Color.White
        Me.btnSORC5.Location = New System.Drawing.Point(923, 622)
        Me.btnSORC5.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSORC5.Name = "btnSORC5"
        Me.btnSORC5.Size = New System.Drawing.Size(141, 68)
        Me.btnSORC5.TabIndex = 41
        Me.btnSORC5.Text = "Reason Code 5"
        Me.btnSORC5.UseVisualStyleBackColor = False
        '
        'btnSORC4
        '
        Me.btnSORC4.BackColor = System.Drawing.Color.Navy
        Me.btnSORC4.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSORC4.FlatAppearance.BorderSize = 8
        Me.btnSORC4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSORC4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSORC4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSORC4.ForeColor = System.Drawing.Color.White
        Me.btnSORC4.Location = New System.Drawing.Point(773, 622)
        Me.btnSORC4.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSORC4.Name = "btnSORC4"
        Me.btnSORC4.Size = New System.Drawing.Size(141, 68)
        Me.btnSORC4.TabIndex = 40
        Me.btnSORC4.Text = "Reason Code 4"
        Me.btnSORC4.UseVisualStyleBackColor = False
        '
        'btnSORC3
        '
        Me.btnSORC3.BackColor = System.Drawing.Color.Navy
        Me.btnSORC3.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSORC3.FlatAppearance.BorderSize = 8
        Me.btnSORC3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSORC3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSORC3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSORC3.ForeColor = System.Drawing.Color.White
        Me.btnSORC3.Location = New System.Drawing.Point(624, 622)
        Me.btnSORC3.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSORC3.Name = "btnSORC3"
        Me.btnSORC3.Size = New System.Drawing.Size(141, 68)
        Me.btnSORC3.TabIndex = 39
        Me.btnSORC3.Text = "Reason Code 3"
        Me.btnSORC3.UseVisualStyleBackColor = False
        '
        'btnSORC2
        '
        Me.btnSORC2.BackColor = System.Drawing.Color.Navy
        Me.btnSORC2.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSORC2.FlatAppearance.BorderSize = 8
        Me.btnSORC2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSORC2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSORC2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSORC2.ForeColor = System.Drawing.Color.White
        Me.btnSORC2.Location = New System.Drawing.Point(475, 622)
        Me.btnSORC2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSORC2.Name = "btnSORC2"
        Me.btnSORC2.Size = New System.Drawing.Size(141, 68)
        Me.btnSORC2.TabIndex = 38
        Me.btnSORC2.Text = "Reason Code 2"
        Me.btnSORC2.UseVisualStyleBackColor = False
        '
        'btnSORC1
        '
        Me.btnSORC1.BackColor = System.Drawing.Color.Navy
        Me.btnSORC1.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSORC1.FlatAppearance.BorderSize = 8
        Me.btnSORC1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSORC1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSORC1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSORC1.ForeColor = System.Drawing.Color.White
        Me.btnSORC1.Location = New System.Drawing.Point(325, 622)
        Me.btnSORC1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSORC1.Name = "btnSORC1"
        Me.btnSORC1.Size = New System.Drawing.Size(141, 68)
        Me.btnSORC1.TabIndex = 37
        Me.btnSORC1.Text = "Reason Code 1"
        Me.btnSORC1.UseVisualStyleBackColor = False
        '
        'btnTrendNumberLatePlannedSO
        '
        Me.btnTrendNumberLatePlannedSO.BackColor = System.Drawing.Color.Navy
        Me.btnTrendNumberLatePlannedSO.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendNumberLatePlannedSO.FlatAppearance.BorderSize = 8
        Me.btnTrendNumberLatePlannedSO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendNumberLatePlannedSO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendNumberLatePlannedSO.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendNumberLatePlannedSO.ForeColor = System.Drawing.Color.White
        Me.btnTrendNumberLatePlannedSO.Location = New System.Drawing.Point(1328, 786)
        Me.btnTrendNumberLatePlannedSO.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendNumberLatePlannedSO.Name = "btnTrendNumberLatePlannedSO"
        Me.btnTrendNumberLatePlannedSO.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendNumberLatePlannedSO.TabIndex = 36
        Me.btnTrendNumberLatePlannedSO.Text = "Number of Late Planned Sales Order Lines:"
        Me.ttExplanations.SetToolTip(Me.btnTrendNumberLatePlannedSO, "Material receipts against your purchase orders from yesterday:")
        Me.btnTrendNumberLatePlannedSO.UseVisualStyleBackColor = False
        '
        'btnTrendNumberSupplyProblems
        '
        Me.btnTrendNumberSupplyProblems.BackColor = System.Drawing.Color.Navy
        Me.btnTrendNumberSupplyProblems.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendNumberSupplyProblems.FlatAppearance.BorderSize = 8
        Me.btnTrendNumberSupplyProblems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendNumberSupplyProblems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendNumberSupplyProblems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendNumberSupplyProblems.ForeColor = System.Drawing.Color.White
        Me.btnTrendNumberSupplyProblems.Location = New System.Drawing.Point(1064, 786)
        Me.btnTrendNumberSupplyProblems.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendNumberSupplyProblems.Name = "btnTrendNumberSupplyProblems"
        Me.btnTrendNumberSupplyProblems.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendNumberSupplyProblems.TabIndex = 35
        Me.btnTrendNumberSupplyProblems.Text = "Total Number of Supply Problems:"
        Me.ttExplanations.SetToolTip(Me.btnTrendNumberSupplyProblems, "The number of PO -Lines that have been created from you yesterday:")
        Me.btnTrendNumberSupplyProblems.UseVisualStyleBackColor = False
        '
        'btnTrendShippedSalesOrderLines
        '
        Me.btnTrendShippedSalesOrderLines.BackColor = System.Drawing.Color.Navy
        Me.btnTrendShippedSalesOrderLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendShippedSalesOrderLines.FlatAppearance.BorderSize = 8
        Me.btnTrendShippedSalesOrderLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendShippedSalesOrderLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendShippedSalesOrderLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendShippedSalesOrderLines.ForeColor = System.Drawing.Color.White
        Me.btnTrendShippedSalesOrderLines.Location = New System.Drawing.Point(800, 786)
        Me.btnTrendShippedSalesOrderLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendShippedSalesOrderLines.Name = "btnTrendShippedSalesOrderLines"
        Me.btnTrendShippedSalesOrderLines.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendShippedSalesOrderLines.TabIndex = 34
        Me.btnTrendShippedSalesOrderLines.Text = "Shipped Sales Order Lines within the last 30 days:"
        Me.ttExplanations.SetToolTip(Me.btnTrendShippedSalesOrderLines, "The number of shipped sales order lines.")
        Me.btnTrendShippedSalesOrderLines.UseVisualStyleBackColor = False
        Me.btnTrendShippedSalesOrderLines.Visible = False
        '
        'btnTrendNewSalesOrderLines
        '
        Me.btnTrendNewSalesOrderLines.BackColor = System.Drawing.Color.Navy
        Me.btnTrendNewSalesOrderLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendNewSalesOrderLines.FlatAppearance.BorderSize = 8
        Me.btnTrendNewSalesOrderLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendNewSalesOrderLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendNewSalesOrderLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendNewSalesOrderLines.ForeColor = System.Drawing.Color.White
        Me.btnTrendNewSalesOrderLines.Location = New System.Drawing.Point(536, 786)
        Me.btnTrendNewSalesOrderLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendNewSalesOrderLines.Name = "btnTrendNewSalesOrderLines"
        Me.btnTrendNewSalesOrderLines.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendNewSalesOrderLines.TabIndex = 33
        Me.btnTrendNewSalesOrderLines.Text = "New Sales Order Lines within the last 30 days:"
        Me.ttExplanations.SetToolTip(Me.btnTrendNewSalesOrderLines, "The number of your distinct suppliers used in the open purchase orders.")
        Me.btnTrendNewSalesOrderLines.UseVisualStyleBackColor = False
        '
        'btnTrendValueOpenSalesOrderLines
        '
        Me.btnTrendValueOpenSalesOrderLines.BackColor = System.Drawing.Color.Navy
        Me.btnTrendValueOpenSalesOrderLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendValueOpenSalesOrderLines.FlatAppearance.BorderSize = 8
        Me.btnTrendValueOpenSalesOrderLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendValueOpenSalesOrderLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendValueOpenSalesOrderLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendValueOpenSalesOrderLines.ForeColor = System.Drawing.Color.White
        Me.btnTrendValueOpenSalesOrderLines.Location = New System.Drawing.Point(272, 786)
        Me.btnTrendValueOpenSalesOrderLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendValueOpenSalesOrderLines.Name = "btnTrendValueOpenSalesOrderLines"
        Me.btnTrendValueOpenSalesOrderLines.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendValueOpenSalesOrderLines.TabIndex = 32
        Me.btnTrendValueOpenSalesOrderLines.Text = "Total Value of Open Sales Order Lines:"
        Me.ttExplanations.SetToolTip(Me.btnTrendValueOpenSalesOrderLines, "The total value of the open sales orders.")
        Me.btnTrendValueOpenSalesOrderLines.UseVisualStyleBackColor = False
        '
        'btnTrendNumberOfOpenSalesOrderLines
        '
        Me.btnTrendNumberOfOpenSalesOrderLines.BackColor = System.Drawing.Color.Navy
        Me.btnTrendNumberOfOpenSalesOrderLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendNumberOfOpenSalesOrderLines.FlatAppearance.BorderSize = 8
        Me.btnTrendNumberOfOpenSalesOrderLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendNumberOfOpenSalesOrderLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendNumberOfOpenSalesOrderLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendNumberOfOpenSalesOrderLines.ForeColor = System.Drawing.Color.White
        Me.btnTrendNumberOfOpenSalesOrderLines.Location = New System.Drawing.Point(8, 786)
        Me.btnTrendNumberOfOpenSalesOrderLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendNumberOfOpenSalesOrderLines.Name = "btnTrendNumberOfOpenSalesOrderLines"
        Me.btnTrendNumberOfOpenSalesOrderLines.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendNumberOfOpenSalesOrderLines.TabIndex = 31
        Me.btnTrendNumberOfOpenSalesOrderLines.Text = "Number of Open Sales Order Lines:"
        Me.ttExplanations.SetToolTip(Me.btnTrendNumberOfOpenSalesOrderLines, "Your current number of open sales orders.")
        Me.btnTrendNumberOfOpenSalesOrderLines.UseVisualStyleBackColor = False
        '
        'btnSOBySeverity
        '
        Me.btnSOBySeverity.BackColor = System.Drawing.Color.Navy
        Me.btnSOBySeverity.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOBySeverity.FlatAppearance.BorderSize = 8
        Me.btnSOBySeverity.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOBySeverity.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOBySeverity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOBySeverity.ForeColor = System.Drawing.Color.White
        Me.btnSOBySeverity.Location = New System.Drawing.Point(1249, 546)
        Me.btnSOBySeverity.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOBySeverity.Name = "btnSOBySeverity"
        Me.btnSOBySeverity.Size = New System.Drawing.Size(141, 68)
        Me.btnSOBySeverity.TabIndex = 30
        Me.btnSOBySeverity.Text = "By Severity"
        Me.btnSOBySeverity.UseVisualStyleBackColor = False
        '
        'btnSOByDateRange
        '
        Me.btnSOByDateRange.BackColor = System.Drawing.Color.Navy
        Me.btnSOByDateRange.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOByDateRange.FlatAppearance.BorderSize = 8
        Me.btnSOByDateRange.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOByDateRange.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOByDateRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOByDateRange.ForeColor = System.Drawing.Color.White
        Me.btnSOByDateRange.Location = New System.Drawing.Point(1100, 546)
        Me.btnSOByDateRange.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOByDateRange.Name = "btnSOByDateRange"
        Me.btnSOByDateRange.Size = New System.Drawing.Size(141, 68)
        Me.btnSOByDateRange.TabIndex = 29
        Me.btnSOByDateRange.Text = "By Date Range"
        Me.btnSOByDateRange.UseVisualStyleBackColor = False
        '
        'btnSOByOrder
        '
        Me.btnSOByOrder.BackColor = System.Drawing.Color.Navy
        Me.btnSOByOrder.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOByOrder.FlatAppearance.BorderSize = 8
        Me.btnSOByOrder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOByOrder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOByOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOByOrder.ForeColor = System.Drawing.Color.White
        Me.btnSOByOrder.Location = New System.Drawing.Point(951, 546)
        Me.btnSOByOrder.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOByOrder.Name = "btnSOByOrder"
        Me.btnSOByOrder.Size = New System.Drawing.Size(141, 68)
        Me.btnSOByOrder.TabIndex = 28
        Me.btnSOByOrder.Text = "By Order"
        Me.btnSOByOrder.UseVisualStyleBackColor = False
        '
        'btnSOByValue
        '
        Me.btnSOByValue.BackColor = System.Drawing.Color.Navy
        Me.btnSOByValue.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOByValue.FlatAppearance.BorderSize = 8
        Me.btnSOByValue.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOByValue.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOByValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOByValue.ForeColor = System.Drawing.Color.White
        Me.btnSOByValue.Location = New System.Drawing.Point(801, 546)
        Me.btnSOByValue.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOByValue.Name = "btnSOByValue"
        Me.btnSOByValue.Size = New System.Drawing.Size(141, 68)
        Me.btnSOByValue.TabIndex = 27
        Me.btnSOByValue.Text = "By Value"
        Me.btnSOByValue.UseVisualStyleBackColor = False
        '
        'btnSOByProduct
        '
        Me.btnSOByProduct.BackColor = System.Drawing.Color.Navy
        Me.btnSOByProduct.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOByProduct.FlatAppearance.BorderSize = 8
        Me.btnSOByProduct.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOByProduct.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOByProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOByProduct.ForeColor = System.Drawing.Color.White
        Me.btnSOByProduct.Location = New System.Drawing.Point(652, 546)
        Me.btnSOByProduct.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOByProduct.Name = "btnSOByProduct"
        Me.btnSOByProduct.Size = New System.Drawing.Size(141, 68)
        Me.btnSOByProduct.TabIndex = 26
        Me.btnSOByProduct.Text = "By Order Type"
        Me.btnSOByProduct.UseVisualStyleBackColor = False
        '
        'btnSOByCustomer
        '
        Me.btnSOByCustomer.BackColor = System.Drawing.Color.Navy
        Me.btnSOByCustomer.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOByCustomer.FlatAppearance.BorderSize = 8
        Me.btnSOByCustomer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOByCustomer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOByCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOByCustomer.ForeColor = System.Drawing.Color.White
        Me.btnSOByCustomer.Location = New System.Drawing.Point(503, 546)
        Me.btnSOByCustomer.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOByCustomer.Name = "btnSOByCustomer"
        Me.btnSOByCustomer.Size = New System.Drawing.Size(141, 68)
        Me.btnSOByCustomer.TabIndex = 25
        Me.btnSOByCustomer.Text = "By Customer"
        Me.btnSOByCustomer.UseVisualStyleBackColor = False
        '
        'btnSOCountProblems
        '
        Me.btnSOCountProblems.BackColor = System.Drawing.Color.Navy
        Me.btnSOCountProblems.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOCountProblems.FlatAppearance.BorderSize = 8
        Me.btnSOCountProblems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOCountProblems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOCountProblems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOCountProblems.ForeColor = System.Drawing.Color.White
        Me.btnSOCountProblems.Location = New System.Drawing.Point(415, 546)
        Me.btnSOCountProblems.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOCountProblems.Name = "btnSOCountProblems"
        Me.btnSOCountProblems.Size = New System.Drawing.Size(80, 68)
        Me.btnSOCountProblems.TabIndex = 24
        Me.btnSOCountProblems.Text = "#"
        Me.btnSOCountProblems.UseVisualStyleBackColor = False
        '
        'btnSalesOrderLinesProblems
        '
        Me.btnSalesOrderLinesProblems.BackColor = System.Drawing.Color.Navy
        Me.btnSalesOrderLinesProblems.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSalesOrderLinesProblems.FlatAppearance.BorderSize = 8
        Me.btnSalesOrderLinesProblems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSalesOrderLinesProblems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSalesOrderLinesProblems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalesOrderLinesProblems.ForeColor = System.Drawing.Color.White
        Me.btnSalesOrderLinesProblems.Location = New System.Drawing.Point(151, 546)
        Me.btnSalesOrderLinesProblems.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSalesOrderLinesProblems.Name = "btnSalesOrderLinesProblems"
        Me.btnSalesOrderLinesProblems.Size = New System.Drawing.Size(256, 68)
        Me.btnSalesOrderLinesProblems.TabIndex = 23
        Me.btnSalesOrderLinesProblems.Text = "Sales Order Lines with Problems"
        Me.btnSalesOrderLinesProblems.UseVisualStyleBackColor = False
        '
        'Button7
        '
        Me.Button7.BackColor = System.Drawing.Color.Navy
        Me.Button7.Enabled = False
        Me.Button7.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.Button7.FlatAppearance.BorderSize = 8
        Me.Button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.Button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button7.ForeColor = System.Drawing.Color.White
        Me.Button7.Location = New System.Drawing.Point(1325, 330)
        Me.Button7.Margin = New System.Windows.Forms.Padding(4)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(256, 68)
        Me.Button7.TabIndex = 22
        Me.Button7.Text = "Blank"
        Me.ttExplanations.SetToolTip(Me.Button7, "Material receipts against your purchase orders from yesterday:")
        Me.Button7.UseVisualStyleBackColor = False
        Me.Button7.Visible = False
        '
        'Button8
        '
        Me.Button8.BackColor = System.Drawing.Color.Navy
        Me.Button8.Enabled = False
        Me.Button8.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.Button8.FlatAppearance.BorderSize = 8
        Me.Button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.Button8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.Button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button8.ForeColor = System.Drawing.Color.White
        Me.Button8.Location = New System.Drawing.Point(1061, 330)
        Me.Button8.Margin = New System.Windows.Forms.Padding(4)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(256, 68)
        Me.Button8.TabIndex = 21
        Me.Button8.Text = "Blank"
        Me.ttExplanations.SetToolTip(Me.Button8, "The number of PO -Lines that have been created from you yesterday:")
        Me.Button8.UseVisualStyleBackColor = False
        Me.Button8.Visible = False
        '
        'btnSOLinesWithoutBOMCompletion
        '
        Me.btnSOLinesWithoutBOMCompletion.BackColor = System.Drawing.Color.Navy
        Me.btnSOLinesWithoutBOMCompletion.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOLinesWithoutBOMCompletion.FlatAppearance.BorderSize = 8
        Me.btnSOLinesWithoutBOMCompletion.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOLinesWithoutBOMCompletion.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOLinesWithoutBOMCompletion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOLinesWithoutBOMCompletion.ForeColor = System.Drawing.Color.White
        Me.btnSOLinesWithoutBOMCompletion.Location = New System.Drawing.Point(797, 330)
        Me.btnSOLinesWithoutBOMCompletion.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOLinesWithoutBOMCompletion.Name = "btnSOLinesWithoutBOMCompletion"
        Me.btnSOLinesWithoutBOMCompletion.Size = New System.Drawing.Size(256, 68)
        Me.btnSOLinesWithoutBOMCompletion.TabIndex = 20
        Me.btnSOLinesWithoutBOMCompletion.Text = "Sales Order Lines without BOM Completion:"
        Me.ttExplanations.SetToolTip(Me.btnSOLinesWithoutBOMCompletion, "The number of shipped sales order lines.")
        Me.btnSOLinesWithoutBOMCompletion.UseVisualStyleBackColor = False
        '
        'btnSOwithoutSSD
        '
        Me.btnSOwithoutSSD.BackColor = System.Drawing.Color.Navy
        Me.btnSOwithoutSSD.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOwithoutSSD.FlatAppearance.BorderSize = 8
        Me.btnSOwithoutSSD.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOwithoutSSD.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOwithoutSSD.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOwithoutSSD.ForeColor = System.Drawing.Color.White
        Me.btnSOwithoutSSD.Location = New System.Drawing.Point(533, 330)
        Me.btnSOwithoutSSD.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOwithoutSSD.Name = "btnSOwithoutSSD"
        Me.btnSOwithoutSSD.Size = New System.Drawing.Size(256, 68)
        Me.btnSOwithoutSSD.TabIndex = 19
        Me.btnSOwithoutSSD.Text = "Sales Order Lines without SSD: "
        Me.ttExplanations.SetToolTip(Me.btnSOwithoutSSD, "The number of your distinct suppliers used in the open purchase orders.")
        Me.btnSOwithoutSSD.UseVisualStyleBackColor = False
        '
        'btnSOShipNextDays
        '
        Me.btnSOShipNextDays.BackColor = System.Drawing.Color.Navy
        Me.btnSOShipNextDays.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOShipNextDays.FlatAppearance.BorderSize = 8
        Me.btnSOShipNextDays.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOShipNextDays.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOShipNextDays.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOShipNextDays.ForeColor = System.Drawing.Color.White
        Me.btnSOShipNextDays.Location = New System.Drawing.Point(269, 330)
        Me.btnSOShipNextDays.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOShipNextDays.Name = "btnSOShipNextDays"
        Me.btnSOShipNextDays.Size = New System.Drawing.Size(256, 68)
        Me.btnSOShipNextDays.TabIndex = 18
        Me.btnSOShipNextDays.Text = "Sales Order Lines with SSD within the next 30 days:"
        Me.ttExplanations.SetToolTip(Me.btnSOShipNextDays, "The total value of the open sales orders.")
        Me.btnSOShipNextDays.UseVisualStyleBackColor = False
        '
        'btnSOShipToday
        '
        Me.btnSOShipToday.BackColor = System.Drawing.Color.Navy
        Me.btnSOShipToday.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOShipToday.FlatAppearance.BorderSize = 8
        Me.btnSOShipToday.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOShipToday.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOShipToday.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOShipToday.ForeColor = System.Drawing.Color.White
        Me.btnSOShipToday.Location = New System.Drawing.Point(5, 330)
        Me.btnSOShipToday.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOShipToday.Name = "btnSOShipToday"
        Me.btnSOShipToday.Size = New System.Drawing.Size(256, 68)
        Me.btnSOShipToday.TabIndex = 17
        Me.btnSOShipToday.Text = "Sales Order Lines with SSD today:"
        Me.ttExplanations.SetToolTip(Me.btnSOShipToday, "Your current number of open sales orders.")
        Me.btnSOShipToday.UseVisualStyleBackColor = False
        '
        'btnSOLatePlannedLines
        '
        Me.btnSOLatePlannedLines.BackColor = System.Drawing.Color.Navy
        Me.btnSOLatePlannedLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOLatePlannedLines.FlatAppearance.BorderSize = 8
        Me.btnSOLatePlannedLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOLatePlannedLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOLatePlannedLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOLatePlannedLines.ForeColor = System.Drawing.Color.White
        Me.btnSOLatePlannedLines.Location = New System.Drawing.Point(1325, 255)
        Me.btnSOLatePlannedLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOLatePlannedLines.Name = "btnSOLatePlannedLines"
        Me.btnSOLatePlannedLines.Size = New System.Drawing.Size(256, 68)
        Me.btnSOLatePlannedLines.TabIndex = 16
        Me.btnSOLatePlannedLines.Text = "Number of Late Planned Sales Order Lines:"
        Me.ttExplanations.SetToolTip(Me.btnSOLatePlannedLines, "Material receipts against your purchase orders from yesterday:")
        Me.btnSOLatePlannedLines.UseVisualStyleBackColor = False
        '
        'btnSOSupplyProblems
        '
        Me.btnSOSupplyProblems.BackColor = System.Drawing.Color.Navy
        Me.btnSOSupplyProblems.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOSupplyProblems.FlatAppearance.BorderSize = 8
        Me.btnSOSupplyProblems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOSupplyProblems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOSupplyProblems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOSupplyProblems.ForeColor = System.Drawing.Color.White
        Me.btnSOSupplyProblems.Location = New System.Drawing.Point(1061, 255)
        Me.btnSOSupplyProblems.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOSupplyProblems.Name = "btnSOSupplyProblems"
        Me.btnSOSupplyProblems.Size = New System.Drawing.Size(256, 68)
        Me.btnSOSupplyProblems.TabIndex = 15
        Me.btnSOSupplyProblems.Text = "Total Number of Supply Problems:"
        Me.ttExplanations.SetToolTip(Me.btnSOSupplyProblems, "The number of PO -Lines that have been created from you yesterday:")
        Me.btnSOSupplyProblems.UseVisualStyleBackColor = False
        '
        'btnSOShippedLines
        '
        Me.btnSOShippedLines.BackColor = System.Drawing.Color.Navy
        Me.btnSOShippedLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOShippedLines.FlatAppearance.BorderSize = 8
        Me.btnSOShippedLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOShippedLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOShippedLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOShippedLines.ForeColor = System.Drawing.Color.White
        Me.btnSOShippedLines.Location = New System.Drawing.Point(797, 255)
        Me.btnSOShippedLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOShippedLines.Name = "btnSOShippedLines"
        Me.btnSOShippedLines.Size = New System.Drawing.Size(256, 68)
        Me.btnSOShippedLines.TabIndex = 14
        Me.btnSOShippedLines.Text = "Shipped Sales Order Lines within the last 30 days:"
        Me.ttExplanations.SetToolTip(Me.btnSOShippedLines, "The number of shipped sales order lines.")
        Me.btnSOShippedLines.UseVisualStyleBackColor = False
        '
        'btnSONewLines
        '
        Me.btnSONewLines.BackColor = System.Drawing.Color.Navy
        Me.btnSONewLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSONewLines.FlatAppearance.BorderSize = 8
        Me.btnSONewLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSONewLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSONewLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSONewLines.ForeColor = System.Drawing.Color.White
        Me.btnSONewLines.Location = New System.Drawing.Point(533, 255)
        Me.btnSONewLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSONewLines.Name = "btnSONewLines"
        Me.btnSONewLines.Size = New System.Drawing.Size(256, 68)
        Me.btnSONewLines.TabIndex = 13
        Me.btnSONewLines.Text = "New Sales Order Lines within the last 30 days:"
        Me.ttExplanations.SetToolTip(Me.btnSONewLines, "The number of your distinct suppliers used in the open purchase orders.")
        Me.btnSONewLines.UseVisualStyleBackColor = False
        '
        'btnSOValueLines
        '
        Me.btnSOValueLines.BackColor = System.Drawing.Color.Navy
        Me.btnSOValueLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSOValueLines.FlatAppearance.BorderSize = 8
        Me.btnSOValueLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSOValueLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSOValueLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSOValueLines.ForeColor = System.Drawing.Color.White
        Me.btnSOValueLines.Location = New System.Drawing.Point(269, 255)
        Me.btnSOValueLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSOValueLines.Name = "btnSOValueLines"
        Me.btnSOValueLines.Size = New System.Drawing.Size(256, 68)
        Me.btnSOValueLines.TabIndex = 12
        Me.btnSOValueLines.Text = "Total Value of Open Sales Order Lines:"
        Me.ttExplanations.SetToolTip(Me.btnSOValueLines, "The total value of the open sales orders.")
        Me.btnSOValueLines.UseVisualStyleBackColor = False
        '
        'btnSoNumberLines
        '
        Me.btnSoNumberLines.BackColor = System.Drawing.Color.Navy
        Me.btnSoNumberLines.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnSoNumberLines.FlatAppearance.BorderSize = 8
        Me.btnSoNumberLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnSoNumberLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnSoNumberLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSoNumberLines.ForeColor = System.Drawing.Color.White
        Me.btnSoNumberLines.Location = New System.Drawing.Point(5, 255)
        Me.btnSoNumberLines.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSoNumberLines.Name = "btnSoNumberLines"
        Me.btnSoNumberLines.Size = New System.Drawing.Size(256, 68)
        Me.btnSoNumberLines.TabIndex = 11
        Me.btnSoNumberLines.Text = "Number of Open Sales Order Lines:"
        Me.ttExplanations.SetToolTip(Me.btnSoNumberLines, "Your current number of open sales orders.")
        Me.btnSoNumberLines.UseVisualStyleBackColor = False
        '
        'PictureBox6
        '
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(8, 695)
        Me.PictureBox6.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(1567, 84)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox6.TabIndex = 6
        Me.PictureBox6.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(5, 431)
        Me.PictureBox5.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(1567, 84)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox5.TabIndex = 5
        Me.PictureBox5.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(5, 164)
        Me.PictureBox4.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(1567, 84)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox4.TabIndex = 4
        Me.PictureBox4.TabStop = False
        '
        'tpPurchasing
        '
        Me.tpPurchasing.BackColor = System.Drawing.Color.Lavender
        Me.tpPurchasing.Controls.Add(Me.rtbLiveTicker)
        Me.tpPurchasing.Controls.Add(Me.btnTrendRC4)
        Me.tpPurchasing.Controls.Add(Me.btnTrendRC3)
        Me.tpPurchasing.Controls.Add(Me.btnTrendRC2)
        Me.tpPurchasing.Controls.Add(Me.btnTrendRC1)
        Me.tpPurchasing.Controls.Add(Me.btnTrendUsedSuppliers)
        Me.tpPurchasing.Controls.Add(Me.btnTrendRequests)
        Me.tpPurchasing.Controls.Add(Me.txtRC4)
        Me.tpPurchasing.Controls.Add(Me.btnRC4Severity)
        Me.tpPurchasing.Controls.Add(Me.btnRC4Item)
        Me.tpPurchasing.Controls.Add(Me.btnRC4Order)
        Me.tpPurchasing.Controls.Add(Me.btnRC4Supplier)
        Me.tpPurchasing.Controls.Add(Me.btnRC4Count)
        Me.tpPurchasing.Controls.Add(Me.btnReasonCode4)
        Me.tpPurchasing.Controls.Add(Me.txtRC3)
        Me.tpPurchasing.Controls.Add(Me.btnRC3Severity)
        Me.tpPurchasing.Controls.Add(Me.btnRC3Item)
        Me.tpPurchasing.Controls.Add(Me.btnRC3Order)
        Me.tpPurchasing.Controls.Add(Me.btnRC3Supplier)
        Me.tpPurchasing.Controls.Add(Me.btnRC3Count)
        Me.tpPurchasing.Controls.Add(Me.btnReasonCode3)
        Me.tpPurchasing.Controls.Add(Me.txtRC2)
        Me.tpPurchasing.Controls.Add(Me.btnRC2Severity)
        Me.tpPurchasing.Controls.Add(Me.btnRC2Item)
        Me.tpPurchasing.Controls.Add(Me.btnRC2Order)
        Me.tpPurchasing.Controls.Add(Me.btnRC2Supplier)
        Me.tpPurchasing.Controls.Add(Me.btnRC2Count)
        Me.tpPurchasing.Controls.Add(Me.btnReasonCode2)
        Me.tpPurchasing.Controls.Add(Me.txtLastReviewRC1)
        Me.tpPurchasing.Controls.Add(Me.btnRC1Severity)
        Me.tpPurchasing.Controls.Add(Me.btnRC1Item)
        Me.tpPurchasing.Controls.Add(Me.btnRC1Supplier)
        Me.tpPurchasing.Controls.Add(Me.btnRC1Count)
        Me.tpPurchasing.Controls.Add(Me.btnReasonCode1)
        Me.tpPurchasing.Controls.Add(Me.btnMaterialReceiptsYesterday)
        Me.tpPurchasing.Controls.Add(Me.btnNumberPOLinesCreated)
        Me.tpPurchasing.Controls.Add(Me.btnCurrentSupplyProblems)
        Me.tpPurchasing.Controls.Add(Me.btnUsedSuppliers)
        Me.tpPurchasing.Controls.Add(Me.btnTotalValuePo)
        Me.tpPurchasing.Controls.Add(Me.btnNoOfOpenPOs)
        Me.tpPurchasing.Controls.Add(Me.PictureBox3)
        Me.tpPurchasing.Controls.Add(Me.PictureBox1)
        Me.tpPurchasing.Controls.Add(Me.PictureBox2)
        Me.tpPurchasing.Controls.Add(Me.Panel1)
        Me.tpPurchasing.Location = New System.Drawing.Point(4, 26)
        Me.tpPurchasing.Margin = New System.Windows.Forms.Padding(4)
        Me.tpPurchasing.Name = "tpPurchasing"
        Me.tpPurchasing.Padding = New System.Windows.Forms.Padding(4)
        Me.tpPurchasing.Size = New System.Drawing.Size(1363, 888)
        Me.tpPurchasing.TabIndex = 1
        Me.tpPurchasing.Text = "Purchasing Workbench"
        Me.tpPurchasing.UseVisualStyleBackColor = True
        '
        'rtbLiveTicker
        '
        Me.rtbLiveTicker.Location = New System.Drawing.Point(803, 174)
        Me.rtbLiveTicker.Margin = New System.Windows.Forms.Padding(4)
        Me.rtbLiveTicker.Name = "rtbLiveTicker"
        Me.rtbLiveTicker.Size = New System.Drawing.Size(773, 160)
        Me.rtbLiveTicker.TabIndex = 45
        Me.rtbLiveTicker.Text = ""
        '
        'btnTrendRC4
        '
        Me.btnTrendRC4.BackColor = System.Drawing.Color.Navy
        Me.btnTrendRC4.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendRC4.FlatAppearance.BorderSize = 8
        Me.btnTrendRC4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendRC4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendRC4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendRC4.ForeColor = System.Drawing.Color.White
        Me.btnTrendRC4.Location = New System.Drawing.Point(1331, 929)
        Me.btnTrendRC4.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendRC4.Name = "btnTrendRC4"
        Me.btnTrendRC4.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendRC4.TabIndex = 44
        Me.btnTrendRC4.Text = "Reason Code 4"
        Me.ttExplanations.SetToolTip(Me.btnTrendRC4, "Material receipts against your purchase orders from yesterday.")
        Me.btnTrendRC4.UseVisualStyleBackColor = False
        '
        'btnTrendRC3
        '
        Me.btnTrendRC3.BackColor = System.Drawing.Color.Navy
        Me.btnTrendRC3.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendRC3.FlatAppearance.BorderSize = 8
        Me.btnTrendRC3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendRC3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendRC3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendRC3.ForeColor = System.Drawing.Color.White
        Me.btnTrendRC3.Location = New System.Drawing.Point(1067, 929)
        Me.btnTrendRC3.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendRC3.Name = "btnTrendRC3"
        Me.btnTrendRC3.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendRC3.TabIndex = 43
        Me.btnTrendRC3.Text = "Reason Code 3"
        Me.ttExplanations.SetToolTip(Me.btnTrendRC3, "The number of PO -Lines that have been created from you yesterday.")
        Me.btnTrendRC3.UseVisualStyleBackColor = False
        '
        'btnTrendRC2
        '
        Me.btnTrendRC2.BackColor = System.Drawing.Color.Navy
        Me.btnTrendRC2.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendRC2.FlatAppearance.BorderSize = 8
        Me.btnTrendRC2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendRC2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendRC2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendRC2.ForeColor = System.Drawing.Color.White
        Me.btnTrendRC2.Location = New System.Drawing.Point(803, 929)
        Me.btnTrendRC2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendRC2.Name = "btnTrendRC2"
        Me.btnTrendRC2.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendRC2.TabIndex = 42
        Me.btnTrendRC2.Text = "Reason Code 2"
        Me.ttExplanations.SetToolTip(Me.btnTrendRC2, "The number of current supply problem ( reason code 1 - 4) from your purchase orde" &
        "rs.")
        Me.btnTrendRC2.UseVisualStyleBackColor = False
        '
        'btnTrendRC1
        '
        Me.btnTrendRC1.BackColor = System.Drawing.Color.Navy
        Me.btnTrendRC1.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendRC1.FlatAppearance.BorderSize = 8
        Me.btnTrendRC1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendRC1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendRC1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendRC1.ForeColor = System.Drawing.Color.White
        Me.btnTrendRC1.Location = New System.Drawing.Point(539, 929)
        Me.btnTrendRC1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendRC1.Name = "btnTrendRC1"
        Me.btnTrendRC1.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendRC1.TabIndex = 41
        Me.btnTrendRC1.Text = "Reason Code 1"
        Me.ttExplanations.SetToolTip(Me.btnTrendRC1, "The number of your distinct suppliers used in the open purchase orders.")
        Me.btnTrendRC1.UseVisualStyleBackColor = False
        '
        'btnTrendUsedSuppliers
        '
        Me.btnTrendUsedSuppliers.BackColor = System.Drawing.Color.Navy
        Me.btnTrendUsedSuppliers.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendUsedSuppliers.FlatAppearance.BorderSize = 8
        Me.btnTrendUsedSuppliers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendUsedSuppliers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendUsedSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendUsedSuppliers.ForeColor = System.Drawing.Color.White
        Me.btnTrendUsedSuppliers.Location = New System.Drawing.Point(275, 929)
        Me.btnTrendUsedSuppliers.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendUsedSuppliers.Name = "btnTrendUsedSuppliers"
        Me.btnTrendUsedSuppliers.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendUsedSuppliers.TabIndex = 40
        Me.btnTrendUsedSuppliers.Text = "Number of Used Suppliers"
        Me.ttExplanations.SetToolTip(Me.btnTrendUsedSuppliers, "The total value of the open purchase orders.")
        Me.btnTrendUsedSuppliers.UseVisualStyleBackColor = False
        '
        'btnTrendRequests
        '
        Me.btnTrendRequests.BackColor = System.Drawing.Color.Navy
        Me.btnTrendRequests.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTrendRequests.FlatAppearance.BorderSize = 8
        Me.btnTrendRequests.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTrendRequests.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTrendRequests.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTrendRequests.ForeColor = System.Drawing.Color.White
        Me.btnTrendRequests.Location = New System.Drawing.Point(11, 929)
        Me.btnTrendRequests.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTrendRequests.Name = "btnTrendRequests"
        Me.btnTrendRequests.Size = New System.Drawing.Size(256, 68)
        Me.btnTrendRequests.TabIndex = 39
        Me.btnTrendRequests.Text = "Number of MRP-Requests"
        Me.ttExplanations.SetToolTip(Me.btnTrendRequests, "Your current number of open purchase orders.")
        Me.btnTrendRequests.UseVisualStyleBackColor = False
        '
        'txtRC4
        '
        Me.txtRC4.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.txtRC4.Location = New System.Drawing.Point(1420, 730)
        Me.txtRC4.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRC4.Multiline = True
        Me.txtRC4.Name = "txtRC4"
        Me.txtRC4.Size = New System.Drawing.Size(156, 67)
        Me.txtRC4.TabIndex = 38
        '
        'btnRC4Severity
        '
        Me.btnRC4Severity.BackColor = System.Drawing.Color.Navy
        Me.btnRC4Severity.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC4Severity.FlatAppearance.BorderSize = 8
        Me.btnRC4Severity.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC4Severity.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC4Severity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC4Severity.ForeColor = System.Drawing.Color.White
        Me.btnRC4Severity.Location = New System.Drawing.Point(1155, 730)
        Me.btnRC4Severity.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC4Severity.Name = "btnRC4Severity"
        Me.btnRC4Severity.Size = New System.Drawing.Size(256, 68)
        Me.btnRC4Severity.TabIndex = 37
        Me.btnRC4Severity.Text = "By Severity"
        Me.btnRC4Severity.UseVisualStyleBackColor = False
        '
        'btnRC4Item
        '
        Me.btnRC4Item.BackColor = System.Drawing.Color.Navy
        Me.btnRC4Item.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC4Item.FlatAppearance.BorderSize = 8
        Me.btnRC4Item.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC4Item.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC4Item.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC4Item.ForeColor = System.Drawing.Color.White
        Me.btnRC4Item.Location = New System.Drawing.Point(891, 730)
        Me.btnRC4Item.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC4Item.Name = "btnRC4Item"
        Me.btnRC4Item.Size = New System.Drawing.Size(256, 68)
        Me.btnRC4Item.TabIndex = 36
        Me.btnRC4Item.Text = "By Item"
        Me.btnRC4Item.UseVisualStyleBackColor = False
        '
        'btnRC4Order
        '
        Me.btnRC4Order.BackColor = System.Drawing.Color.Navy
        Me.btnRC4Order.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC4Order.FlatAppearance.BorderSize = 8
        Me.btnRC4Order.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC4Order.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC4Order.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC4Order.ForeColor = System.Drawing.Color.White
        Me.btnRC4Order.Location = New System.Drawing.Point(627, 730)
        Me.btnRC4Order.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC4Order.Name = "btnRC4Order"
        Me.btnRC4Order.Size = New System.Drawing.Size(256, 68)
        Me.btnRC4Order.TabIndex = 35
        Me.btnRC4Order.Text = "By Order"
        Me.btnRC4Order.UseVisualStyleBackColor = False
        '
        'btnRC4Supplier
        '
        Me.btnRC4Supplier.BackColor = System.Drawing.Color.Navy
        Me.btnRC4Supplier.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC4Supplier.FlatAppearance.BorderSize = 8
        Me.btnRC4Supplier.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC4Supplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC4Supplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC4Supplier.ForeColor = System.Drawing.Color.White
        Me.btnRC4Supplier.Location = New System.Drawing.Point(363, 730)
        Me.btnRC4Supplier.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC4Supplier.Name = "btnRC4Supplier"
        Me.btnRC4Supplier.Size = New System.Drawing.Size(256, 68)
        Me.btnRC4Supplier.TabIndex = 34
        Me.btnRC4Supplier.Text = "By Supplier"
        Me.btnRC4Supplier.UseVisualStyleBackColor = False
        '
        'btnRC4Count
        '
        Me.btnRC4Count.BackColor = System.Drawing.Color.Navy
        Me.btnRC4Count.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC4Count.FlatAppearance.BorderSize = 8
        Me.btnRC4Count.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC4Count.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC4Count.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC4Count.ForeColor = System.Drawing.Color.White
        Me.btnRC4Count.Location = New System.Drawing.Point(275, 730)
        Me.btnRC4Count.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC4Count.Name = "btnRC4Count"
        Me.btnRC4Count.Size = New System.Drawing.Size(80, 68)
        Me.btnRC4Count.TabIndex = 33
        Me.btnRC4Count.Text = "#"
        Me.btnRC4Count.UseVisualStyleBackColor = False
        '
        'btnReasonCode4
        '
        Me.btnReasonCode4.BackColor = System.Drawing.Color.Navy
        Me.btnReasonCode4.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnReasonCode4.FlatAppearance.BorderSize = 8
        Me.btnReasonCode4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnReasonCode4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnReasonCode4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReasonCode4.ForeColor = System.Drawing.Color.White
        Me.btnReasonCode4.Location = New System.Drawing.Point(11, 730)
        Me.btnReasonCode4.Margin = New System.Windows.Forms.Padding(4)
        Me.btnReasonCode4.Name = "btnReasonCode4"
        Me.btnReasonCode4.Size = New System.Drawing.Size(256, 68)
        Me.btnReasonCode4.TabIndex = 32
        Me.btnReasonCode4.Text = "Reason Code 4"
        Me.btnReasonCode4.UseVisualStyleBackColor = False
        '
        'txtRC3
        '
        Me.txtRC3.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.txtRC3.Location = New System.Drawing.Point(1420, 655)
        Me.txtRC3.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRC3.Multiline = True
        Me.txtRC3.Name = "txtRC3"
        Me.txtRC3.Size = New System.Drawing.Size(156, 67)
        Me.txtRC3.TabIndex = 31
        '
        'btnRC3Severity
        '
        Me.btnRC3Severity.BackColor = System.Drawing.Color.Navy
        Me.btnRC3Severity.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC3Severity.FlatAppearance.BorderSize = 8
        Me.btnRC3Severity.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC3Severity.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC3Severity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC3Severity.ForeColor = System.Drawing.Color.White
        Me.btnRC3Severity.Location = New System.Drawing.Point(1155, 655)
        Me.btnRC3Severity.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC3Severity.Name = "btnRC3Severity"
        Me.btnRC3Severity.Size = New System.Drawing.Size(256, 68)
        Me.btnRC3Severity.TabIndex = 30
        Me.btnRC3Severity.Text = "By Severity"
        Me.btnRC3Severity.UseVisualStyleBackColor = False
        '
        'btnRC3Item
        '
        Me.btnRC3Item.BackColor = System.Drawing.Color.Navy
        Me.btnRC3Item.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC3Item.FlatAppearance.BorderSize = 8
        Me.btnRC3Item.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC3Item.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC3Item.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC3Item.ForeColor = System.Drawing.Color.White
        Me.btnRC3Item.Location = New System.Drawing.Point(891, 655)
        Me.btnRC3Item.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC3Item.Name = "btnRC3Item"
        Me.btnRC3Item.Size = New System.Drawing.Size(256, 68)
        Me.btnRC3Item.TabIndex = 29
        Me.btnRC3Item.Text = "By Item"
        Me.btnRC3Item.UseVisualStyleBackColor = False
        '
        'btnRC3Order
        '
        Me.btnRC3Order.BackColor = System.Drawing.Color.Navy
        Me.btnRC3Order.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC3Order.FlatAppearance.BorderSize = 8
        Me.btnRC3Order.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC3Order.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC3Order.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC3Order.ForeColor = System.Drawing.Color.White
        Me.btnRC3Order.Location = New System.Drawing.Point(627, 655)
        Me.btnRC3Order.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC3Order.Name = "btnRC3Order"
        Me.btnRC3Order.Size = New System.Drawing.Size(256, 68)
        Me.btnRC3Order.TabIndex = 28
        Me.btnRC3Order.Text = "By Order"
        Me.btnRC3Order.UseVisualStyleBackColor = False
        '
        'btnRC3Supplier
        '
        Me.btnRC3Supplier.BackColor = System.Drawing.Color.Navy
        Me.btnRC3Supplier.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC3Supplier.FlatAppearance.BorderSize = 8
        Me.btnRC3Supplier.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC3Supplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC3Supplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC3Supplier.ForeColor = System.Drawing.Color.White
        Me.btnRC3Supplier.Location = New System.Drawing.Point(363, 655)
        Me.btnRC3Supplier.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC3Supplier.Name = "btnRC3Supplier"
        Me.btnRC3Supplier.Size = New System.Drawing.Size(256, 68)
        Me.btnRC3Supplier.TabIndex = 27
        Me.btnRC3Supplier.Text = "By Supplier"
        Me.btnRC3Supplier.UseVisualStyleBackColor = False
        '
        'btnRC3Count
        '
        Me.btnRC3Count.BackColor = System.Drawing.Color.Navy
        Me.btnRC3Count.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC3Count.FlatAppearance.BorderSize = 8
        Me.btnRC3Count.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC3Count.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC3Count.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC3Count.ForeColor = System.Drawing.Color.White
        Me.btnRC3Count.Location = New System.Drawing.Point(275, 655)
        Me.btnRC3Count.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC3Count.Name = "btnRC3Count"
        Me.btnRC3Count.Size = New System.Drawing.Size(80, 68)
        Me.btnRC3Count.TabIndex = 26
        Me.btnRC3Count.Text = "#"
        Me.btnRC3Count.UseVisualStyleBackColor = False
        '
        'btnReasonCode3
        '
        Me.btnReasonCode3.BackColor = System.Drawing.Color.Navy
        Me.btnReasonCode3.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnReasonCode3.FlatAppearance.BorderSize = 8
        Me.btnReasonCode3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnReasonCode3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnReasonCode3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReasonCode3.ForeColor = System.Drawing.Color.White
        Me.btnReasonCode3.Location = New System.Drawing.Point(11, 655)
        Me.btnReasonCode3.Margin = New System.Windows.Forms.Padding(4)
        Me.btnReasonCode3.Name = "btnReasonCode3"
        Me.btnReasonCode3.Size = New System.Drawing.Size(256, 68)
        Me.btnReasonCode3.TabIndex = 25
        Me.btnReasonCode3.Text = "Reason Code 3"
        Me.btnReasonCode3.UseVisualStyleBackColor = False
        '
        'txtRC2
        '
        Me.txtRC2.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.txtRC2.Location = New System.Drawing.Point(1420, 580)
        Me.txtRC2.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRC2.Multiline = True
        Me.txtRC2.Name = "txtRC2"
        Me.txtRC2.Size = New System.Drawing.Size(156, 67)
        Me.txtRC2.TabIndex = 24
        '
        'btnRC2Severity
        '
        Me.btnRC2Severity.BackColor = System.Drawing.Color.Navy
        Me.btnRC2Severity.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC2Severity.FlatAppearance.BorderSize = 8
        Me.btnRC2Severity.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC2Severity.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC2Severity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC2Severity.ForeColor = System.Drawing.Color.White
        Me.btnRC2Severity.Location = New System.Drawing.Point(1155, 580)
        Me.btnRC2Severity.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC2Severity.Name = "btnRC2Severity"
        Me.btnRC2Severity.Size = New System.Drawing.Size(256, 68)
        Me.btnRC2Severity.TabIndex = 23
        Me.btnRC2Severity.Text = "By Severity"
        Me.btnRC2Severity.UseVisualStyleBackColor = False
        '
        'btnRC2Item
        '
        Me.btnRC2Item.BackColor = System.Drawing.Color.Navy
        Me.btnRC2Item.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC2Item.FlatAppearance.BorderSize = 8
        Me.btnRC2Item.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC2Item.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC2Item.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC2Item.ForeColor = System.Drawing.Color.White
        Me.btnRC2Item.Location = New System.Drawing.Point(891, 580)
        Me.btnRC2Item.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC2Item.Name = "btnRC2Item"
        Me.btnRC2Item.Size = New System.Drawing.Size(256, 68)
        Me.btnRC2Item.TabIndex = 22
        Me.btnRC2Item.Text = "By Item"
        Me.btnRC2Item.UseVisualStyleBackColor = False
        '
        'btnRC2Order
        '
        Me.btnRC2Order.BackColor = System.Drawing.Color.Navy
        Me.btnRC2Order.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC2Order.FlatAppearance.BorderSize = 8
        Me.btnRC2Order.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC2Order.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC2Order.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC2Order.ForeColor = System.Drawing.Color.White
        Me.btnRC2Order.Location = New System.Drawing.Point(627, 580)
        Me.btnRC2Order.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC2Order.Name = "btnRC2Order"
        Me.btnRC2Order.Size = New System.Drawing.Size(256, 68)
        Me.btnRC2Order.TabIndex = 21
        Me.btnRC2Order.Text = "By Order"
        Me.btnRC2Order.UseVisualStyleBackColor = False
        '
        'btnRC2Supplier
        '
        Me.btnRC2Supplier.BackColor = System.Drawing.Color.Navy
        Me.btnRC2Supplier.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC2Supplier.FlatAppearance.BorderSize = 8
        Me.btnRC2Supplier.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC2Supplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC2Supplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC2Supplier.ForeColor = System.Drawing.Color.White
        Me.btnRC2Supplier.Location = New System.Drawing.Point(363, 580)
        Me.btnRC2Supplier.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC2Supplier.Name = "btnRC2Supplier"
        Me.btnRC2Supplier.Size = New System.Drawing.Size(256, 68)
        Me.btnRC2Supplier.TabIndex = 20
        Me.btnRC2Supplier.Text = "By Supplier"
        Me.btnRC2Supplier.UseVisualStyleBackColor = False
        '
        'btnRC2Count
        '
        Me.btnRC2Count.BackColor = System.Drawing.Color.Navy
        Me.btnRC2Count.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC2Count.FlatAppearance.BorderSize = 8
        Me.btnRC2Count.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC2Count.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC2Count.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC2Count.ForeColor = System.Drawing.Color.White
        Me.btnRC2Count.Location = New System.Drawing.Point(275, 580)
        Me.btnRC2Count.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC2Count.Name = "btnRC2Count"
        Me.btnRC2Count.Size = New System.Drawing.Size(80, 68)
        Me.btnRC2Count.TabIndex = 19
        Me.btnRC2Count.Text = "#"
        Me.btnRC2Count.UseVisualStyleBackColor = False
        '
        'btnReasonCode2
        '
        Me.btnReasonCode2.BackColor = System.Drawing.Color.Navy
        Me.btnReasonCode2.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnReasonCode2.FlatAppearance.BorderSize = 8
        Me.btnReasonCode2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnReasonCode2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnReasonCode2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReasonCode2.ForeColor = System.Drawing.Color.White
        Me.btnReasonCode2.Location = New System.Drawing.Point(11, 580)
        Me.btnReasonCode2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnReasonCode2.Name = "btnReasonCode2"
        Me.btnReasonCode2.Size = New System.Drawing.Size(256, 68)
        Me.btnReasonCode2.TabIndex = 18
        Me.btnReasonCode2.Text = "Reason Code 2"
        Me.btnReasonCode2.UseVisualStyleBackColor = False
        '
        'txtLastReviewRC1
        '
        Me.txtLastReviewRC1.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(177, Byte))
        Me.txtLastReviewRC1.Location = New System.Drawing.Point(1420, 505)
        Me.txtLastReviewRC1.Margin = New System.Windows.Forms.Padding(4)
        Me.txtLastReviewRC1.Multiline = True
        Me.txtLastReviewRC1.Name = "txtLastReviewRC1"
        Me.txtLastReviewRC1.Size = New System.Drawing.Size(156, 67)
        Me.txtLastReviewRC1.TabIndex = 17
        Me.txtLastReviewRC1.Visible = False
        '
        'btnRC1Severity
        '
        Me.btnRC1Severity.BackColor = System.Drawing.Color.Navy
        Me.btnRC1Severity.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC1Severity.FlatAppearance.BorderSize = 8
        Me.btnRC1Severity.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC1Severity.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC1Severity.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC1Severity.ForeColor = System.Drawing.Color.White
        Me.btnRC1Severity.Location = New System.Drawing.Point(1155, 505)
        Me.btnRC1Severity.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC1Severity.Name = "btnRC1Severity"
        Me.btnRC1Severity.Size = New System.Drawing.Size(256, 68)
        Me.btnRC1Severity.TabIndex = 16
        Me.btnRC1Severity.Text = "By Severity"
        Me.btnRC1Severity.UseVisualStyleBackColor = False
        '
        'btnRC1Item
        '
        Me.btnRC1Item.BackColor = System.Drawing.Color.Navy
        Me.btnRC1Item.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC1Item.FlatAppearance.BorderSize = 8
        Me.btnRC1Item.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC1Item.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC1Item.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC1Item.ForeColor = System.Drawing.Color.White
        Me.btnRC1Item.Location = New System.Drawing.Point(891, 505)
        Me.btnRC1Item.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC1Item.Name = "btnRC1Item"
        Me.btnRC1Item.Size = New System.Drawing.Size(256, 68)
        Me.btnRC1Item.TabIndex = 15
        Me.btnRC1Item.Text = "By Item"
        Me.btnRC1Item.UseVisualStyleBackColor = False
        '
        'btnRC1Supplier
        '
        Me.btnRC1Supplier.BackColor = System.Drawing.Color.Navy
        Me.btnRC1Supplier.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC1Supplier.FlatAppearance.BorderSize = 8
        Me.btnRC1Supplier.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC1Supplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC1Supplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC1Supplier.ForeColor = System.Drawing.Color.White
        Me.btnRC1Supplier.Location = New System.Drawing.Point(363, 505)
        Me.btnRC1Supplier.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC1Supplier.Name = "btnRC1Supplier"
        Me.btnRC1Supplier.Size = New System.Drawing.Size(256, 68)
        Me.btnRC1Supplier.TabIndex = 13
        Me.btnRC1Supplier.Text = "By Schedule Ship Date"
        Me.btnRC1Supplier.UseVisualStyleBackColor = False
        '
        'btnRC1Count
        '
        Me.btnRC1Count.BackColor = System.Drawing.Color.Navy
        Me.btnRC1Count.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC1Count.FlatAppearance.BorderSize = 8
        Me.btnRC1Count.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC1Count.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC1Count.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC1Count.ForeColor = System.Drawing.Color.White
        Me.btnRC1Count.Location = New System.Drawing.Point(275, 505)
        Me.btnRC1Count.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC1Count.Name = "btnRC1Count"
        Me.btnRC1Count.Size = New System.Drawing.Size(80, 68)
        Me.btnRC1Count.TabIndex = 12
        Me.btnRC1Count.Text = "#"
        Me.btnRC1Count.UseVisualStyleBackColor = False
        '
        'btnReasonCode1
        '
        Me.btnReasonCode1.BackColor = System.Drawing.Color.Navy
        Me.btnReasonCode1.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnReasonCode1.FlatAppearance.BorderSize = 8
        Me.btnReasonCode1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnReasonCode1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnReasonCode1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReasonCode1.ForeColor = System.Drawing.Color.White
        Me.btnReasonCode1.Location = New System.Drawing.Point(11, 505)
        Me.btnReasonCode1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnReasonCode1.Name = "btnReasonCode1"
        Me.btnReasonCode1.Size = New System.Drawing.Size(256, 68)
        Me.btnReasonCode1.TabIndex = 11
        Me.btnReasonCode1.Text = "Reason Code 1"
        Me.btnReasonCode1.UseVisualStyleBackColor = False
        '
        'btnMaterialReceiptsYesterday
        '
        Me.btnMaterialReceiptsYesterday.BackColor = System.Drawing.Color.Navy
        Me.btnMaterialReceiptsYesterday.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnMaterialReceiptsYesterday.FlatAppearance.BorderSize = 8
        Me.btnMaterialReceiptsYesterday.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnMaterialReceiptsYesterday.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnMaterialReceiptsYesterday.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMaterialReceiptsYesterday.ForeColor = System.Drawing.Color.White
        Me.btnMaterialReceiptsYesterday.Location = New System.Drawing.Point(1331, 98)
        Me.btnMaterialReceiptsYesterday.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMaterialReceiptsYesterday.Name = "btnMaterialReceiptsYesterday"
        Me.btnMaterialReceiptsYesterday.Size = New System.Drawing.Size(256, 68)
        Me.btnMaterialReceiptsYesterday.TabIndex = 10
        Me.btnMaterialReceiptsYesterday.Text = "Material Receipts from Yesterday"
        Me.ttExplanations.SetToolTip(Me.btnMaterialReceiptsYesterday, "Material receipts against your purchase orders from yesterday:")
        Me.btnMaterialReceiptsYesterday.UseVisualStyleBackColor = False
        Me.btnMaterialReceiptsYesterday.Visible = False
        '
        'btnNumberPOLinesCreated
        '
        Me.btnNumberPOLinesCreated.BackColor = System.Drawing.Color.Navy
        Me.btnNumberPOLinesCreated.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnNumberPOLinesCreated.FlatAppearance.BorderSize = 8
        Me.btnNumberPOLinesCreated.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnNumberPOLinesCreated.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnNumberPOLinesCreated.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNumberPOLinesCreated.ForeColor = System.Drawing.Color.White
        Me.btnNumberPOLinesCreated.Location = New System.Drawing.Point(1067, 98)
        Me.btnNumberPOLinesCreated.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNumberPOLinesCreated.Name = "btnNumberPOLinesCreated"
        Me.btnNumberPOLinesCreated.Size = New System.Drawing.Size(256, 68)
        Me.btnNumberPOLinesCreated.TabIndex = 9
        Me.btnNumberPOLinesCreated.Text = "Number of PO-Lines Created Yesterday:"
        Me.ttExplanations.SetToolTip(Me.btnNumberPOLinesCreated, "The number of PO -Lines that have been created from you yesterday:")
        Me.btnNumberPOLinesCreated.UseVisualStyleBackColor = False
        '
        'btnCurrentSupplyProblems
        '
        Me.btnCurrentSupplyProblems.BackColor = System.Drawing.Color.Navy
        Me.btnCurrentSupplyProblems.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnCurrentSupplyProblems.FlatAppearance.BorderSize = 8
        Me.btnCurrentSupplyProblems.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnCurrentSupplyProblems.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnCurrentSupplyProblems.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCurrentSupplyProblems.ForeColor = System.Drawing.Color.White
        Me.btnCurrentSupplyProblems.Location = New System.Drawing.Point(803, 98)
        Me.btnCurrentSupplyProblems.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCurrentSupplyProblems.Name = "btnCurrentSupplyProblems"
        Me.btnCurrentSupplyProblems.Size = New System.Drawing.Size(256, 68)
        Me.btnCurrentSupplyProblems.TabIndex = 8
        Me.btnCurrentSupplyProblems.Text = "Number of Current Supply Problems:"
        Me.ttExplanations.SetToolTip(Me.btnCurrentSupplyProblems, "The number of current supply problem ( reason code 1 - 4) from your purchase orde" &
        "rs.")
        Me.btnCurrentSupplyProblems.UseVisualStyleBackColor = False
        '
        'btnUsedSuppliers
        '
        Me.btnUsedSuppliers.BackColor = System.Drawing.Color.Navy
        Me.btnUsedSuppliers.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnUsedSuppliers.FlatAppearance.BorderSize = 8
        Me.btnUsedSuppliers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnUsedSuppliers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnUsedSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUsedSuppliers.ForeColor = System.Drawing.Color.White
        Me.btnUsedSuppliers.Location = New System.Drawing.Point(539, 98)
        Me.btnUsedSuppliers.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUsedSuppliers.Name = "btnUsedSuppliers"
        Me.btnUsedSuppliers.Size = New System.Drawing.Size(256, 68)
        Me.btnUsedSuppliers.TabIndex = 7
        Me.btnUsedSuppliers.Text = "Number of used Suppliers:"
        Me.ttExplanations.SetToolTip(Me.btnUsedSuppliers, "The number of your distinct suppliers used in the open purchase orders.")
        Me.btnUsedSuppliers.UseVisualStyleBackColor = False
        '
        'btnTotalValuePo
        '
        Me.btnTotalValuePo.BackColor = System.Drawing.Color.Navy
        Me.btnTotalValuePo.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnTotalValuePo.FlatAppearance.BorderSize = 8
        Me.btnTotalValuePo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnTotalValuePo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnTotalValuePo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTotalValuePo.ForeColor = System.Drawing.Color.White
        Me.btnTotalValuePo.Location = New System.Drawing.Point(275, 98)
        Me.btnTotalValuePo.Margin = New System.Windows.Forms.Padding(4)
        Me.btnTotalValuePo.Name = "btnTotalValuePo"
        Me.btnTotalValuePo.Size = New System.Drawing.Size(256, 68)
        Me.btnTotalValuePo.TabIndex = 6
        Me.btnTotalValuePo.Text = "Total Value of Open PO's:"
        Me.ttExplanations.SetToolTip(Me.btnTotalValuePo, "The total value of the open purchase orders.")
        Me.btnTotalValuePo.UseVisualStyleBackColor = False
        '
        'btnNoOfOpenPOs
        '
        Me.btnNoOfOpenPOs.BackColor = System.Drawing.Color.Navy
        Me.btnNoOfOpenPOs.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnNoOfOpenPOs.FlatAppearance.BorderSize = 8
        Me.btnNoOfOpenPOs.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnNoOfOpenPOs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnNoOfOpenPOs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNoOfOpenPOs.ForeColor = System.Drawing.Color.White
        Me.btnNoOfOpenPOs.Location = New System.Drawing.Point(11, 98)
        Me.btnNoOfOpenPOs.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNoOfOpenPOs.Name = "btnNoOfOpenPOs"
        Me.btnNoOfOpenPOs.Size = New System.Drawing.Size(256, 68)
        Me.btnNoOfOpenPOs.TabIndex = 5
        Me.btnNoOfOpenPOs.Text = "Number of Open PO's:"
        Me.ttExplanations.SetToolTip(Me.btnNoOfOpenPOs, "Your current number of open purchase orders.")
        Me.btnNoOfOpenPOs.UseVisualStyleBackColor = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(11, 414)
        Me.PictureBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(1567, 84)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox3.TabIndex = 4
        Me.PictureBox3.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(11, 7)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1567, 84)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(11, 838)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(1567, 84)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 2
        Me.PictureBox2.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.btnDiscussionTracking)
        Me.Panel1.Controls.Add(Me.dgvAggregation)
        Me.Panel1.Controls.Add(Me.btnRC1Order)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(4, 4)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1355, 880)
        Me.Panel1.TabIndex = 46
        '
        'btnDiscussionTracking
        '
        Me.btnDiscussionTracking.BackColor = System.Drawing.Color.Navy
        Me.btnDiscussionTracking.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnDiscussionTracking.FlatAppearance.BorderSize = 8
        Me.btnDiscussionTracking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnDiscussionTracking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnDiscussionTracking.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDiscussionTracking.ForeColor = System.Drawing.Color.White
        Me.btnDiscussionTracking.Location = New System.Drawing.Point(799, 338)
        Me.btnDiscussionTracking.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDiscussionTracking.Name = "btnDiscussionTracking"
        Me.btnDiscussionTracking.Size = New System.Drawing.Size(256, 68)
        Me.btnDiscussionTracking.TabIndex = 9
        Me.btnDiscussionTracking.Text = "Discussion Tracking"
        Me.ttExplanations.SetToolTip(Me.btnDiscussionTracking, "The number of current supply problem ( reason code 1 - 4) from your purchase orde" &
        "rs.")
        Me.btnDiscussionTracking.UseVisualStyleBackColor = False
        '
        'dgvAggregation
        '
        Me.dgvAggregation.BackgroundColor = System.Drawing.Color.White
        Me.dgvAggregation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAggregation.Location = New System.Drawing.Point(7, 170)
        Me.dgvAggregation.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvAggregation.Name = "dgvAggregation"
        Me.dgvAggregation.RowHeadersWidth = 51
        Me.dgvAggregation.Size = New System.Drawing.Size(784, 233)
        Me.dgvAggregation.TabIndex = 0
        '
        'btnRC1Order
        '
        Me.btnRC1Order.BackColor = System.Drawing.Color.Navy
        Me.btnRC1Order.FlatAppearance.BorderColor = System.Drawing.Color.Navy
        Me.btnRC1Order.FlatAppearance.BorderSize = 8
        Me.btnRC1Order.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White
        Me.btnRC1Order.FlatAppearance.MouseOverBackColor = System.Drawing.Color.ForestGreen
        Me.btnRC1Order.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRC1Order.ForeColor = System.Drawing.Color.White
        Me.btnRC1Order.Location = New System.Drawing.Point(623, 501)
        Me.btnRC1Order.Margin = New System.Windows.Forms.Padding(4)
        Me.btnRC1Order.Name = "btnRC1Order"
        Me.btnRC1Order.Size = New System.Drawing.Size(256, 68)
        Me.btnRC1Order.TabIndex = 14
        Me.btnRC1Order.Text = "By Order"
        Me.btnRC1Order.UseVisualStyleBackColor = False
        '
        'tpScheduling
        '
        Me.tpScheduling.BackColor = System.Drawing.Color.Wheat
        Me.tpScheduling.Location = New System.Drawing.Point(4, 26)
        Me.tpScheduling.Margin = New System.Windows.Forms.Padding(4)
        Me.tpScheduling.Name = "tpScheduling"
        Me.tpScheduling.Padding = New System.Windows.Forms.Padding(4)
        Me.tpScheduling.Size = New System.Drawing.Size(1363, 888)
        Me.tpScheduling.TabIndex = 2
        Me.tpScheduling.Text = "Scheduling Workbench"
        Me.tpScheduling.UseVisualStyleBackColor = True
        '
        'tpBacklog
        '
        Me.tpBacklog.Controls.Add(Me.rvBacklog)
        Me.tpBacklog.Location = New System.Drawing.Point(4, 26)
        Me.tpBacklog.Margin = New System.Windows.Forms.Padding(4)
        Me.tpBacklog.Name = "tpBacklog"
        Me.tpBacklog.Padding = New System.Windows.Forms.Padding(4)
        Me.tpBacklog.Size = New System.Drawing.Size(1363, 888)
        Me.tpBacklog.TabIndex = 3
        Me.tpBacklog.Text = "Backlog"
        Me.tpBacklog.UseVisualStyleBackColor = True
        '
        'tpBookAndBill
        '
        Me.tpBookAndBill.ContextMenuStrip = Me.ContextMenuStrip1
        Me.tpBookAndBill.Location = New System.Drawing.Point(4, 26)
        Me.tpBookAndBill.Margin = New System.Windows.Forms.Padding(4)
        Me.tpBookAndBill.Name = "tpBookAndBill"
        Me.tpBookAndBill.Size = New System.Drawing.Size(1363, 888)
        Me.tpBookAndBill.TabIndex = 4
        Me.tpBookAndBill.Text = "Book And Bill"
        Me.tpBookAndBill.UseVisualStyleBackColor = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(109, 28)
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(108, 24)
        Me.PrintToolStripMenuItem.Text = "Print"
        '
        'tpOTP
        '
        Me.tpOTP.Location = New System.Drawing.Point(4, 26)
        Me.tpOTP.Margin = New System.Windows.Forms.Padding(4)
        Me.tpOTP.Name = "tpOTP"
        Me.tpOTP.Size = New System.Drawing.Size(1363, 888)
        Me.tpOTP.TabIndex = 5
        Me.tpOTP.Text = "On Time Performance"
        Me.tpOTP.UseVisualStyleBackColor = True
        '
        'tpRevenue
        '
        Me.tpRevenue.Controls.Add(Me.rvRevenuePlanning)
        Me.tpRevenue.Location = New System.Drawing.Point(4, 26)
        Me.tpRevenue.Margin = New System.Windows.Forms.Padding(4)
        Me.tpRevenue.Name = "tpRevenue"
        Me.tpRevenue.Padding = New System.Windows.Forms.Padding(4)
        Me.tpRevenue.Size = New System.Drawing.Size(1363, 888)
        Me.tpRevenue.TabIndex = 6
        Me.tpRevenue.Text = "Revenue Planning"
        Me.tpRevenue.UseVisualStyleBackColor = True
        '
        'tblWorkTableAdapter
        '
        Me.tblWorkTableAdapter.ClearBeforeFill = True
        '
        'pf1
        '
        Me.pf1.DocumentName = "document"
        Me.pf1.Form = Me
        Me.pf1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter
        Me.pf1.PrinterSettings = CType(resources.GetObject("pf1.PrinterSettings"), System.Drawing.Printing.PrinterSettings)
        Me.pf1.PrintFileName = Nothing
        '
        'rvBacklog
        '
        Me.rvBacklog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rvBacklog.Location = New System.Drawing.Point(4, 4)
        Me.rvBacklog.Name = "rvBacklog"
        Me.rvBacklog.ServerReport.BearerToken = Nothing
        Me.rvBacklog.Size = New System.Drawing.Size(1355, 880)
        Me.rvBacklog.TabIndex = 0
        '
        'rvRevenuePlanning
        '
        Me.rvRevenuePlanning.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rvRevenuePlanning.Location = New System.Drawing.Point(4, 4)
        Me.rvRevenuePlanning.Name = "rvRevenuePlanning"
        Me.rvRevenuePlanning.ServerReport.BearerToken = Nothing
        Me.rvRevenuePlanning.Size = New System.Drawing.Size(1355, 880)
        Me.rvRevenuePlanning.TabIndex = 0
        '
        'frmIntegratedWorkflow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1371, 918)
        Me.Controls.Add(Me.tcWorkbenches)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmIntegratedWorkflow"
        Me.Text = "Business Cockpit"
        CType(Me.tblWorkBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dsWork, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcWorkbenches.ResumeLayout(False)
        Me.tpOrderManagement.ResumeLayout(False)
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpPurchasing.ResumeLayout(False)
        Me.tpPurchasing.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        CType(Me.dgvAggregation, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpBacklog.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.tpRevenue.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tcWorkbenches As System.Windows.Forms.TabControl
    Friend WithEvents tpOrderManagement As System.Windows.Forms.TabPage
    Friend WithEvents tpPurchasing As System.Windows.Forms.TabPage
    Friend WithEvents tpScheduling As System.Windows.Forms.TabPage
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnNoOfOpenPOs As System.Windows.Forms.Button
    Friend WithEvents btnTotalValuePo As System.Windows.Forms.Button
    Friend WithEvents btnMaterialReceiptsYesterday As System.Windows.Forms.Button
    Friend WithEvents btnNumberPOLinesCreated As System.Windows.Forms.Button
    Friend WithEvents btnCurrentSupplyProblems As System.Windows.Forms.Button
    Friend WithEvents btnUsedSuppliers As System.Windows.Forms.Button
    Friend WithEvents btnReasonCode1 As System.Windows.Forms.Button
    Friend WithEvents ttExplanations As System.Windows.Forms.ToolTip
    Friend WithEvents btnRC1Supplier As System.Windows.Forms.Button
    Friend WithEvents btnRC1Count As System.Windows.Forms.Button
    Friend WithEvents txtLastReviewRC1 As System.Windows.Forms.TextBox
    Friend WithEvents btnRC1Severity As System.Windows.Forms.Button
    Friend WithEvents btnRC1Item As System.Windows.Forms.Button
    Friend WithEvents btnRC1Order As System.Windows.Forms.Button
    Friend WithEvents btnTrendRC4 As System.Windows.Forms.Button
    Friend WithEvents btnTrendRC3 As System.Windows.Forms.Button
    Friend WithEvents btnTrendRC2 As System.Windows.Forms.Button
    Friend WithEvents btnTrendRC1 As System.Windows.Forms.Button
    Friend WithEvents btnTrendUsedSuppliers As System.Windows.Forms.Button
    Friend WithEvents btnTrendRequests As System.Windows.Forms.Button
    Friend WithEvents txtRC4 As System.Windows.Forms.TextBox
    Friend WithEvents btnRC4Severity As System.Windows.Forms.Button
    Friend WithEvents btnRC4Item As System.Windows.Forms.Button
    Friend WithEvents btnRC4Order As System.Windows.Forms.Button
    Friend WithEvents btnRC4Supplier As System.Windows.Forms.Button
    Friend WithEvents btnRC4Count As System.Windows.Forms.Button
    Friend WithEvents btnReasonCode4 As System.Windows.Forms.Button
    Friend WithEvents txtRC3 As System.Windows.Forms.TextBox
    Friend WithEvents btnRC3Severity As System.Windows.Forms.Button
    Friend WithEvents btnRC3Item As System.Windows.Forms.Button
    Friend WithEvents btnRC3Order As System.Windows.Forms.Button
    Friend WithEvents btnRC3Supplier As System.Windows.Forms.Button
    Friend WithEvents btnRC3Count As System.Windows.Forms.Button
    Friend WithEvents btnReasonCode3 As System.Windows.Forms.Button
    Friend WithEvents txtRC2 As System.Windows.Forms.TextBox
    Friend WithEvents btnRC2Severity As System.Windows.Forms.Button
    Friend WithEvents btnRC2Item As System.Windows.Forms.Button
    Friend WithEvents btnRC2Order As System.Windows.Forms.Button
    Friend WithEvents btnRC2Supplier As System.Windows.Forms.Button
    Friend WithEvents btnRC2Count As System.Windows.Forms.Button
    Friend WithEvents btnReasonCode2 As System.Windows.Forms.Button
    Friend WithEvents rtbLiveTicker As System.Windows.Forms.RichTextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents dgvAggregation As System.Windows.Forms.DataGridView
    Friend WithEvents btnDiscussionTracking As System.Windows.Forms.Button
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents btnSOLatePlannedLines As System.Windows.Forms.Button
    Friend WithEvents btnSOSupplyProblems As System.Windows.Forms.Button
    Friend WithEvents btnSOShippedLines As System.Windows.Forms.Button
    Friend WithEvents btnSONewLines As System.Windows.Forms.Button
    Friend WithEvents btnSOValueLines As System.Windows.Forms.Button
    Friend WithEvents btnSoNumberLines As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents btnSOLinesWithoutBOMCompletion As System.Windows.Forms.Button
    Friend WithEvents btnSOwithoutSSD As System.Windows.Forms.Button
    Friend WithEvents btnSOShipNextDays As System.Windows.Forms.Button
    Friend WithEvents btnSOShipToday As System.Windows.Forms.Button
    Friend WithEvents btnSOByOrder As System.Windows.Forms.Button
    Friend WithEvents btnSOByValue As System.Windows.Forms.Button
    Friend WithEvents btnSOByProduct As System.Windows.Forms.Button
    Friend WithEvents btnSOByCustomer As System.Windows.Forms.Button
    Friend WithEvents btnSOCountProblems As System.Windows.Forms.Button
    Friend WithEvents btnSalesOrderLinesProblems As System.Windows.Forms.Button
    Friend WithEvents btnSOBySeverity As System.Windows.Forms.Button
    Friend WithEvents btnSOByDateRange As System.Windows.Forms.Button
    Friend WithEvents btnTrendNumberLatePlannedSO As System.Windows.Forms.Button
    Friend WithEvents btnTrendNumberSupplyProblems As System.Windows.Forms.Button
    Friend WithEvents btnTrendShippedSalesOrderLines As System.Windows.Forms.Button
    Friend WithEvents btnTrendNewSalesOrderLines As System.Windows.Forms.Button
    Friend WithEvents btnTrendValueOpenSalesOrderLines As System.Windows.Forms.Button
    Friend WithEvents btnTrendNumberOfOpenSalesOrderLines As System.Windows.Forms.Button
    Friend WithEvents btnSORC7 As System.Windows.Forms.Button
    Friend WithEvents btnSORC6 As System.Windows.Forms.Button
    Friend WithEvents btnSORC5 As System.Windows.Forms.Button
    Friend WithEvents btnSORC4 As System.Windows.Forms.Button
    Friend WithEvents btnSORC3 As System.Windows.Forms.Button
    Friend WithEvents btnSORC2 As System.Windows.Forms.Button
    Friend WithEvents btnSORC1 As System.Windows.Forms.Button
    Friend WithEvents btnSORC0 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents btnOTP As System.Windows.Forms.Button
    Friend WithEvents btnBookAndBill As System.Windows.Forms.Button
    Friend WithEvents btnBacklog As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents tpBacklog As System.Windows.Forms.TabPage
    Friend WithEvents tpBookAndBill As System.Windows.Forms.TabPage
    Friend WithEvents tpOTP As System.Windows.Forms.TabPage
    Friend WithEvents tblWorkBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents dsWork As Flow_Solution_Workbenches.dsWork
    Friend WithEvents tpRevenue As System.Windows.Forms.TabPage
    Friend WithEvents pf1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
    Private WithEvents tblWorkTableAdapter As Flow_Solution_Workbenches.dsWorkTableAdapters.tblWorkTableAdapter
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rvBacklog As Microsoft.Reporting.WinForms.ReportViewer
    Friend WithEvents rvRevenuePlanning As Microsoft.Reporting.WinForms.ReportViewer
End Class
