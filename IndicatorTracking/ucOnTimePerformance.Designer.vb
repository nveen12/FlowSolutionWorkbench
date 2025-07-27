<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucOnTimePerformance
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucOnTimePerformance))
        Dim ChartArea2 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea
        Dim Legend2 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend
        Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series
        Dim ChartArea3 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea
        Dim Legend3 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series
        Me.chBooking = New System.Windows.Forms.DataVisualization.Charting.Chart
        Me.lblStart = New System.Windows.Forms.Label
        Me.lblEnddate = New System.Windows.Forms.Label
        Me.dtpStart = New System.Windows.Forms.DateTimePicker
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker
        Me.btnReload = New System.Windows.Forms.Button
        Me.lblBD1 = New System.Windows.Forms.Label
        Me.lblBD2 = New System.Windows.Forms.Label
        Me.pf1 = New Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(Me.components)
        Me.btnPrint = New System.Windows.Forms.Button
        Me.chBilling = New System.Windows.Forms.DataVisualization.Charting.Chart
        Me.chOTP = New System.Windows.Forms.DataVisualization.Charting.Chart
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cbxFormula = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.cbxAggreation = New System.Windows.Forms.ComboBox
        Me.lblOTPValue = New System.Windows.Forms.Label
        Me.lblAverageOnTime = New System.Windows.Forms.Label
        Me.clbBusiness2 = New System.Windows.Forms.CheckedListBox
        Me.clbBusiness = New System.Windows.Forms.CheckedListBox
        Me.cbxHorizontalPossible = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.lblCustComplet = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblCompleteBbookings = New System.Windows.Forms.Label
        Me.lblDriverBookings = New System.Windows.Forms.Label
        Me.lblPartsBookings = New System.Windows.Forms.Label
        Me.lblRepairBookings = New System.Windows.Forms.Label
        Me.lblSpareBookings = New System.Windows.Forms.Label
        Me.lblGlobalBookings = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.lblInterDriver = New System.Windows.Forms.Label
        Me.lblInterParts = New System.Windows.Forms.Label
        Me.lblInterRepair = New System.Windows.Forms.Label
        Me.lblInterSpare = New System.Windows.Forms.Label
        Me.lblInterComplete = New System.Windows.Forms.Label
        Me.lblInterGlobal = New System.Windows.Forms.Label
        Me.lblCustDriver = New System.Windows.Forms.Label
        Me.lblCustParts = New System.Windows.Forms.Label
        Me.lblCustRepair = New System.Windows.Forms.Label
        Me.lblCustSpare = New System.Windows.Forms.Label
        Me.lblCustGlobal = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.lblInterCompleteBilling = New System.Windows.Forms.Label
        Me.lblGlobalBillings = New System.Windows.Forms.Label
        Me.lblSpareBillings = New System.Windows.Forms.Label
        Me.lblRepairBillings = New System.Windows.Forms.Label
        Me.lblPartsBillings = New System.Windows.Forms.Label
        Me.lblDriiverBillings = New System.Windows.Forms.Label
        Me.lblCompleteBillings = New System.Windows.Forms.Label
        Me.Label45 = New System.Windows.Forms.Label
        Me.lblCustGlobalBilling = New System.Windows.Forms.Label
        Me.lblCustSpareBilling = New System.Windows.Forms.Label
        Me.lblCustRepairBilling = New System.Windows.Forms.Label
        Me.lblCustPartsBilling = New System.Windows.Forms.Label
        Me.lblCustDriverBilling = New System.Windows.Forms.Label
        Me.lblCustCompleteBilling = New System.Windows.Forms.Label
        Me.Label38 = New System.Windows.Forms.Label
        Me.lblInterGlobalBilling = New System.Windows.Forms.Label
        Me.lblInterSpareBilling = New System.Windows.Forms.Label
        Me.lblInterRepairBilling = New System.Windows.Forms.Label
        Me.lblInterPartsBilling = New System.Windows.Forms.Label
        Me.lblInterDriverBilling = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        CType(Me.chBooking, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chBilling, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chOTP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'chBooking
        '
        Me.chBooking.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea1.Name = "ChartArea1"
        Me.chBooking.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.chBooking.Legends.Add(Legend1)
        Me.chBooking.Location = New System.Drawing.Point(3, 3)
        Me.chBooking.Name = "chBooking"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chBooking.Series.Add(Series1)
        Me.chBooking.Size = New System.Drawing.Size(536, 293)
        Me.chBooking.TabIndex = 0
        Me.chBooking.Text = "Chart1"
        '
        'lblStart
        '
        Me.lblStart.AutoSize = True
        Me.lblStart.Location = New System.Drawing.Point(186, 14)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(50, 13)
        Me.lblStart.TabIndex = 4
        Me.lblStart.Text = "Startdate"
        Me.lblStart.Visible = False
        '
        'lblEnddate
        '
        Me.lblEnddate.AutoSize = True
        Me.lblEnddate.Location = New System.Drawing.Point(186, 54)
        Me.lblEnddate.Name = "lblEnddate"
        Me.lblEnddate.Size = New System.Drawing.Size(47, 13)
        Me.lblEnddate.TabIndex = 5
        Me.lblEnddate.Text = "Enddate"
        Me.lblEnddate.Visible = False
        '
        'dtpStart
        '
        Me.dtpStart.Location = New System.Drawing.Point(186, 31)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(200, 20)
        Me.dtpStart.TabIndex = 6
        '
        'dtpEnd
        '
        Me.dtpEnd.Location = New System.Drawing.Point(186, 71)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(200, 20)
        Me.dtpEnd.TabIndex = 7
        '
        'btnReload
        '
        Me.btnReload.Location = New System.Drawing.Point(395, 31)
        Me.btnReload.Name = "btnReload"
        Me.btnReload.Size = New System.Drawing.Size(75, 23)
        Me.btnReload.TabIndex = 8
        Me.btnReload.Text = "Apply"
        Me.btnReload.UseVisualStyleBackColor = True
        '
        'lblBD1
        '
        Me.lblBD1.AutoSize = True
        Me.lblBD1.Location = New System.Drawing.Point(7, 140)
        Me.lblBD1.Name = "lblBD1"
        Me.lblBD1.Size = New System.Drawing.Size(108, 13)
        Me.lblBD1.TabIndex = 9
        Me.lblBD1.Text = "Business dimension 1"
        '
        'lblBD2
        '
        Me.lblBD2.AutoSize = True
        Me.lblBD2.Location = New System.Drawing.Point(181, 140)
        Me.lblBD2.Name = "lblBD2"
        Me.lblBD2.Size = New System.Drawing.Size(108, 13)
        Me.lblBD2.TabIndex = 11
        Me.lblBD2.Text = "Business dimension 2"
        '
        'pf1
        '
        Me.pf1.DocumentName = "document"
        Me.pf1.Form = Nothing
        Me.pf1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter
        Me.pf1.PrinterSettings = CType(resources.GetObject("pf1.PrinterSettings"), System.Drawing.Printing.PrinterSettings)
        Me.pf1.PrintFileName = Nothing
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(395, 68)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(75, 23)
        Me.btnPrint.TabIndex = 13
        Me.btnPrint.Text = "Print Form"
        Me.btnPrint.UseVisualStyleBackColor = True
        Me.btnPrint.Visible = False
        '
        'chBilling
        '
        Me.chBilling.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea2.Name = "ChartArea1"
        Me.chBilling.ChartAreas.Add(ChartArea2)
        Legend2.Name = "Legend1"
        Me.chBilling.Legends.Add(Legend2)
        Me.chBilling.Location = New System.Drawing.Point(2, 3)
        Me.chBilling.Name = "chBilling"
        Series2.ChartArea = "ChartArea1"
        Series2.Legend = "Legend1"
        Series2.Name = "Series1"
        Me.chBilling.Series.Add(Series2)
        Me.chBilling.Size = New System.Drawing.Size(536, 293)
        Me.chBilling.TabIndex = 14
        Me.chBilling.Text = "Chart2"
        '
        'chOTP
        '
        Me.chOTP.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea3.BorderColor = System.Drawing.Color.Transparent
        ChartArea3.BorderWidth = 0
        ChartArea3.Name = "ChartArea1"
        Me.chOTP.ChartAreas.Add(ChartArea3)
        Legend3.Name = "Legend1"
        Me.chOTP.Legends.Add(Legend3)
        Me.chOTP.Location = New System.Drawing.Point(0, 3)
        Me.chOTP.Name = "chOTP"
        Series3.ChartArea = "ChartArea1"
        Series3.Legend = "Legend1"
        Series3.Name = "Series1"
        Me.chOTP.Series.Add(Series3)
        Me.chOTP.Size = New System.Drawing.Size(539, 318)
        Me.chOTP.TabIndex = 15
        Me.chOTP.Text = "Chart3"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.cbxFormula)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.cbxAggreation)
        Me.GroupBox1.Controls.Add(Me.lblOTPValue)
        Me.GroupBox1.Controls.Add(Me.lblAverageOnTime)
        Me.GroupBox1.Controls.Add(Me.clbBusiness2)
        Me.GroupBox1.Controls.Add(Me.clbBusiness)
        Me.GroupBox1.Controls.Add(Me.cbxHorizontalPossible)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.lblStart)
        Me.GroupBox1.Controls.Add(Me.btnPrint)
        Me.GroupBox1.Controls.Add(Me.lblEnddate)
        Me.GroupBox1.Controls.Add(Me.dtpStart)
        Me.GroupBox1.Controls.Add(Me.lblBD2)
        Me.GroupBox1.Controls.Add(Me.dtpEnd)
        Me.GroupBox1.Controls.Add(Me.btnReload)
        Me.GroupBox1.Controls.Add(Me.lblBD1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(547, 327)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Steering Tools"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(9, 100)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(90, 13)
        Me.Label9.TabIndex = 23
        Me.Label9.Text = "Grouping Formula"
        '
        'cbxFormula
        '
        Me.cbxFormula.FormattingEnabled = True
        Me.cbxFormula.Items.AddRange(New Object() {"Average", "High Low", "High Low Op"})
        Me.cbxFormula.Location = New System.Drawing.Point(6, 116)
        Me.cbxFormula.Name = "cbxFormula"
        Me.cbxFormula.Size = New System.Drawing.Size(168, 21)
        Me.cbxFormula.TabIndex = 22
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(9, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 13)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Grouping Interval"
        '
        'cbxAggreation
        '
        Me.cbxAggreation.FormattingEnabled = True
        Me.cbxAggreation.Items.AddRange(New Object() {"Week", "Two Weeks", "Month"})
        Me.cbxAggreation.Location = New System.Drawing.Point(6, 72)
        Me.cbxAggreation.Name = "cbxAggreation"
        Me.cbxAggreation.Size = New System.Drawing.Size(168, 21)
        Me.cbxAggreation.TabIndex = 20
        '
        'lblOTPValue
        '
        Me.lblOTPValue.AutoSize = True
        Me.lblOTPValue.Location = New System.Drawing.Point(126, 298)
        Me.lblOTPValue.Name = "lblOTPValue"
        Me.lblOTPValue.Size = New System.Drawing.Size(0, 13)
        Me.lblOTPValue.TabIndex = 19
        '
        'lblAverageOnTime
        '
        Me.lblAverageOnTime.AutoSize = True
        Me.lblAverageOnTime.Location = New System.Drawing.Point(7, 298)
        Me.lblAverageOnTime.Name = "lblAverageOnTime"
        Me.lblAverageOnTime.Size = New System.Drawing.Size(113, 13)
        Me.lblAverageOnTime.TabIndex = 18
        Me.lblAverageOnTime.Text = "On Time Performance:"
        '
        'clbBusiness2
        '
        Me.clbBusiness2.FormattingEnabled = True
        Me.clbBusiness2.Location = New System.Drawing.Point(181, 156)
        Me.clbBusiness2.Name = "clbBusiness2"
        Me.clbBusiness2.Size = New System.Drawing.Size(148, 139)
        Me.clbBusiness2.TabIndex = 17
        '
        'clbBusiness
        '
        Me.clbBusiness.FormattingEnabled = True
        Me.clbBusiness.Location = New System.Drawing.Point(10, 156)
        Me.clbBusiness.Name = "clbBusiness"
        Me.clbBusiness.Size = New System.Drawing.Size(148, 139)
        Me.clbBusiness.TabIndex = 16
        '
        'cbxHorizontalPossible
        '
        Me.cbxHorizontalPossible.FormattingEnabled = True
        Me.cbxHorizontalPossible.Items.AddRange(New Object() {"Month to date", "Year to date", "Quarter to date", "Last 30 days", "Last 60 days", "Last 90 days", "Last 120 days"})
        Me.cbxHorizontalPossible.Location = New System.Drawing.Point(6, 32)
        Me.cbxHorizontalPossible.Name = "cbxHorizontalPossible"
        Me.cbxHorizontalPossible.Size = New System.Drawing.Size(168, 21)
        Me.cbxHorizontalPossible.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(98, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Horizontal Features"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer3)
        Me.SplitContainer1.Size = New System.Drawing.Size(1108, 672)
        Me.SplitContainer1.SplitterDistance = 546
        Me.SplitContainer1.TabIndex = 17
        '
        'SplitContainer2
        '
        Me.SplitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer2.Panel1.Controls.Add(Me.chBooking)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.chOTP)
        Me.SplitContainer2.Size = New System.Drawing.Size(546, 672)
        Me.SplitContainer2.SplitterDistance = 353
        Me.SplitContainer2.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 7
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel1.Controls.Add(Me.lblCustComplet, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label20, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label7, 6, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 5, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 4, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblCompleteBbookings, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblDriverBookings, 2, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblPartsBookings, 3, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblRepairBookings, 4, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblSpareBookings, 5, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblGlobalBookings, 6, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label11, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblInterDriver, 2, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblInterParts, 3, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblInterRepair, 4, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblInterSpare, 5, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblInterComplete, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblInterGlobal, 6, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblCustDriver, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblCustParts, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblCustRepair, 4, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblCustSpare, 5, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblCustGlobal, 6, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label10, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 296)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(535, 52)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'lblCustComplet
        '
        Me.lblCustComplet.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustComplet.AutoSize = True
        Me.lblCustComplet.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblCustComplet.Location = New System.Drawing.Point(83, 26)
        Me.lblCustComplet.Name = "lblCustComplet"
        Me.lblCustComplet.Size = New System.Drawing.Size(14, 13)
        Me.lblCustComplet.TabIndex = 15
        Me.lblCustComplet.Text = "0"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.Label20.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label20.Location = New System.Drawing.Point(3, 39)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(34, 13)
        Me.Label20.TabIndex = 14
        Me.Label20.Text = "Total"
        '
        'Label7
        '
        Me.Label7.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label7.Location = New System.Drawing.Point(495, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(37, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Global"
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label6.Location = New System.Drawing.Point(224, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Spare"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label5.Location = New System.Drawing.Point(181, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Repair"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label4.Location = New System.Drawing.Point(144, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Parts"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label3.Location = New System.Drawing.Point(103, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Driver"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!)
        Me.Label2.Location = New System.Drawing.Point(46, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Complete"
        '
        'lblCompleteBbookings
        '
        Me.lblCompleteBbookings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCompleteBbookings.AutoSize = True
        Me.lblCompleteBbookings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblCompleteBbookings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblCompleteBbookings.Location = New System.Drawing.Point(83, 39)
        Me.lblCompleteBbookings.Name = "lblCompleteBbookings"
        Me.lblCompleteBbookings.Size = New System.Drawing.Size(14, 13)
        Me.lblCompleteBbookings.TabIndex = 6
        Me.lblCompleteBbookings.Text = "0"
        '
        'lblDriverBookings
        '
        Me.lblDriverBookings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblDriverBookings.AutoSize = True
        Me.lblDriverBookings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblDriverBookings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblDriverBookings.Location = New System.Drawing.Point(124, 39)
        Me.lblDriverBookings.Name = "lblDriverBookings"
        Me.lblDriverBookings.Size = New System.Drawing.Size(14, 13)
        Me.lblDriverBookings.TabIndex = 7
        Me.lblDriverBookings.Text = "0"
        '
        'lblPartsBookings
        '
        Me.lblPartsBookings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblPartsBookings.AutoSize = True
        Me.lblPartsBookings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblPartsBookings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblPartsBookings.Location = New System.Drawing.Point(161, 39)
        Me.lblPartsBookings.Name = "lblPartsBookings"
        Me.lblPartsBookings.Size = New System.Drawing.Size(14, 13)
        Me.lblPartsBookings.TabIndex = 8
        Me.lblPartsBookings.Text = "0"
        '
        'lblRepairBookings
        '
        Me.lblRepairBookings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblRepairBookings.AutoSize = True
        Me.lblRepairBookings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblRepairBookings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblRepairBookings.Location = New System.Drawing.Point(204, 39)
        Me.lblRepairBookings.Name = "lblRepairBookings"
        Me.lblRepairBookings.Size = New System.Drawing.Size(14, 13)
        Me.lblRepairBookings.TabIndex = 9
        Me.lblRepairBookings.Text = "0"
        '
        'lblSpareBookings
        '
        Me.lblSpareBookings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblSpareBookings.AutoSize = True
        Me.lblSpareBookings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblSpareBookings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblSpareBookings.Location = New System.Drawing.Point(245, 39)
        Me.lblSpareBookings.Name = "lblSpareBookings"
        Me.lblSpareBookings.Size = New System.Drawing.Size(14, 13)
        Me.lblSpareBookings.TabIndex = 10
        Me.lblSpareBookings.Text = "0"
        '
        'lblGlobalBookings
        '
        Me.lblGlobalBookings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblGlobalBookings.AutoSize = True
        Me.lblGlobalBookings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblGlobalBookings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblGlobalBookings.Location = New System.Drawing.Point(518, 39)
        Me.lblGlobalBookings.Name = "lblGlobalBookings"
        Me.lblGlobalBookings.Size = New System.Drawing.Size(14, 13)
        Me.lblGlobalBookings.TabIndex = 11
        Me.lblGlobalBookings.Text = "0"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.Label11.Location = New System.Drawing.Point(3, 26)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(36, 13)
        Me.Label11.TabIndex = 13
        Me.Label11.Text = "Cust."
        '
        'lblInterDriver
        '
        Me.lblInterDriver.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterDriver.AutoSize = True
        Me.lblInterDriver.BackColor = System.Drawing.Color.Transparent
        Me.lblInterDriver.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblInterDriver.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterDriver.Location = New System.Drawing.Point(124, 13)
        Me.lblInterDriver.Name = "lblInterDriver"
        Me.lblInterDriver.Size = New System.Drawing.Size(14, 13)
        Me.lblInterDriver.TabIndex = 16
        Me.lblInterDriver.Text = "0"
        '
        'lblInterParts
        '
        Me.lblInterParts.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterParts.AutoSize = True
        Me.lblInterParts.BackColor = System.Drawing.Color.Transparent
        Me.lblInterParts.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblInterParts.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterParts.Location = New System.Drawing.Point(161, 13)
        Me.lblInterParts.Name = "lblInterParts"
        Me.lblInterParts.Size = New System.Drawing.Size(14, 13)
        Me.lblInterParts.TabIndex = 17
        Me.lblInterParts.Text = "0"
        '
        'lblInterRepair
        '
        Me.lblInterRepair.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterRepair.AutoSize = True
        Me.lblInterRepair.BackColor = System.Drawing.Color.Transparent
        Me.lblInterRepair.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblInterRepair.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterRepair.Location = New System.Drawing.Point(204, 13)
        Me.lblInterRepair.Name = "lblInterRepair"
        Me.lblInterRepair.Size = New System.Drawing.Size(14, 13)
        Me.lblInterRepair.TabIndex = 18
        Me.lblInterRepair.Text = "0"
        '
        'lblInterSpare
        '
        Me.lblInterSpare.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterSpare.AutoSize = True
        Me.lblInterSpare.BackColor = System.Drawing.Color.Transparent
        Me.lblInterSpare.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblInterSpare.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterSpare.Location = New System.Drawing.Point(245, 13)
        Me.lblInterSpare.Name = "lblInterSpare"
        Me.lblInterSpare.Size = New System.Drawing.Size(14, 13)
        Me.lblInterSpare.TabIndex = 19
        Me.lblInterSpare.Text = "0"
        '
        'lblInterComplete
        '
        Me.lblInterComplete.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterComplete.AutoSize = True
        Me.lblInterComplete.BackColor = System.Drawing.Color.Transparent
        Me.lblInterComplete.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblInterComplete.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterComplete.Location = New System.Drawing.Point(83, 13)
        Me.lblInterComplete.Name = "lblInterComplete"
        Me.lblInterComplete.Size = New System.Drawing.Size(14, 13)
        Me.lblInterComplete.TabIndex = 8
        Me.lblInterComplete.Text = "0"
        '
        'lblInterGlobal
        '
        Me.lblInterGlobal.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterGlobal.AutoSize = True
        Me.lblInterGlobal.BackColor = System.Drawing.Color.Transparent
        Me.lblInterGlobal.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblInterGlobal.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterGlobal.Location = New System.Drawing.Point(518, 13)
        Me.lblInterGlobal.Name = "lblInterGlobal"
        Me.lblInterGlobal.Size = New System.Drawing.Size(14, 13)
        Me.lblInterGlobal.TabIndex = 20
        Me.lblInterGlobal.Text = "0"
        '
        'lblCustDriver
        '
        Me.lblCustDriver.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustDriver.AutoSize = True
        Me.lblCustDriver.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblCustDriver.Location = New System.Drawing.Point(124, 26)
        Me.lblCustDriver.Name = "lblCustDriver"
        Me.lblCustDriver.Size = New System.Drawing.Size(14, 13)
        Me.lblCustDriver.TabIndex = 21
        Me.lblCustDriver.Text = "0"
        '
        'lblCustParts
        '
        Me.lblCustParts.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustParts.AutoSize = True
        Me.lblCustParts.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblCustParts.Location = New System.Drawing.Point(161, 26)
        Me.lblCustParts.Name = "lblCustParts"
        Me.lblCustParts.Size = New System.Drawing.Size(14, 13)
        Me.lblCustParts.TabIndex = 22
        Me.lblCustParts.Text = "0"
        '
        'lblCustRepair
        '
        Me.lblCustRepair.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustRepair.AutoSize = True
        Me.lblCustRepair.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblCustRepair.Location = New System.Drawing.Point(204, 26)
        Me.lblCustRepair.Name = "lblCustRepair"
        Me.lblCustRepair.Size = New System.Drawing.Size(14, 13)
        Me.lblCustRepair.TabIndex = 23
        Me.lblCustRepair.Text = "0"
        '
        'lblCustSpare
        '
        Me.lblCustSpare.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustSpare.AutoSize = True
        Me.lblCustSpare.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblCustSpare.Location = New System.Drawing.Point(245, 26)
        Me.lblCustSpare.Name = "lblCustSpare"
        Me.lblCustSpare.Size = New System.Drawing.Size(14, 13)
        Me.lblCustSpare.TabIndex = 24
        Me.lblCustSpare.Text = "0"
        '
        'lblCustGlobal
        '
        Me.lblCustGlobal.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustGlobal.AutoSize = True
        Me.lblCustGlobal.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblCustGlobal.Location = New System.Drawing.Point(518, 26)
        Me.lblCustGlobal.Name = "lblCustGlobal"
        Me.lblCustGlobal.Size = New System.Drawing.Size(14, 13)
        Me.lblCustGlobal.TabIndex = 25
        Me.lblCustGlobal.Text = "0"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold)
        Me.Label10.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label10.Location = New System.Drawing.Point(3, 13)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(37, 13)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "Inter."
        '
        'SplitContainer3
        '
        Me.SplitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.TableLayoutPanel2)
        Me.SplitContainer3.Panel1.Controls.Add(Me.chBilling)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer3.Size = New System.Drawing.Size(558, 672)
        Me.SplitContainer3.SplitterDistance = 354
        Me.SplitContainer3.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 7
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.TableLayoutPanel2.Controls.Add(Me.lblInterCompleteBilling, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblGlobalBillings, 6, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblSpareBillings, 5, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblRepairBillings, 4, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblPartsBillings, 3, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblDriiverBillings, 2, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCompleteBillings, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label45, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCustGlobalBilling, 6, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCustSpareBilling, 5, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCustRepairBilling, 4, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCustPartsBilling, 3, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCustDriverBilling, 2, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblCustCompleteBilling, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label38, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.lblInterGlobalBilling, 6, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblInterSpareBilling, 5, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblInterRepairBilling, 4, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblInterPartsBilling, 3, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblInterDriverBilling, 2, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label19, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label18, 6, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label17, 5, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label16, 4, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label15, 3, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label14, 2, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label13, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(1, 296)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 4
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(552, 52)
        Me.TableLayoutPanel2.TabIndex = 15
        '
        'lblInterCompleteBilling
        '
        Me.lblInterCompleteBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterCompleteBilling.AutoSize = True
        Me.lblInterCompleteBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterCompleteBilling.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterCompleteBilling.Location = New System.Drawing.Point(83, 13)
        Me.lblInterCompleteBilling.Name = "lblInterCompleteBilling"
        Me.lblInterCompleteBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblInterCompleteBilling.TabIndex = 28
        Me.lblInterCompleteBilling.Text = "0"
        '
        'lblGlobalBillings
        '
        Me.lblGlobalBillings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblGlobalBillings.AutoSize = True
        Me.lblGlobalBillings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGlobalBillings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblGlobalBillings.Location = New System.Drawing.Point(535, 39)
        Me.lblGlobalBillings.Name = "lblGlobalBillings"
        Me.lblGlobalBillings.Size = New System.Drawing.Size(14, 13)
        Me.lblGlobalBillings.TabIndex = 27
        Me.lblGlobalBillings.Text = "0"
        '
        'lblSpareBillings
        '
        Me.lblSpareBillings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblSpareBillings.AutoSize = True
        Me.lblSpareBillings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpareBillings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblSpareBillings.Location = New System.Drawing.Point(245, 39)
        Me.lblSpareBillings.Name = "lblSpareBillings"
        Me.lblSpareBillings.Size = New System.Drawing.Size(14, 13)
        Me.lblSpareBillings.TabIndex = 26
        Me.lblSpareBillings.Text = "0"
        '
        'lblRepairBillings
        '
        Me.lblRepairBillings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblRepairBillings.AutoSize = True
        Me.lblRepairBillings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRepairBillings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblRepairBillings.Location = New System.Drawing.Point(204, 39)
        Me.lblRepairBillings.Name = "lblRepairBillings"
        Me.lblRepairBillings.Size = New System.Drawing.Size(14, 13)
        Me.lblRepairBillings.TabIndex = 25
        Me.lblRepairBillings.Text = "0"
        '
        'lblPartsBillings
        '
        Me.lblPartsBillings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblPartsBillings.AutoSize = True
        Me.lblPartsBillings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartsBillings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblPartsBillings.Location = New System.Drawing.Point(161, 39)
        Me.lblPartsBillings.Name = "lblPartsBillings"
        Me.lblPartsBillings.Size = New System.Drawing.Size(14, 13)
        Me.lblPartsBillings.TabIndex = 24
        Me.lblPartsBillings.Text = "0"
        '
        'lblDriiverBillings
        '
        Me.lblDriiverBillings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblDriiverBillings.AutoSize = True
        Me.lblDriiverBillings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDriiverBillings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblDriiverBillings.Location = New System.Drawing.Point(124, 39)
        Me.lblDriiverBillings.Name = "lblDriiverBillings"
        Me.lblDriiverBillings.Size = New System.Drawing.Size(14, 13)
        Me.lblDriiverBillings.TabIndex = 23
        Me.lblDriiverBillings.Text = "0"
        '
        'lblCompleteBillings
        '
        Me.lblCompleteBillings.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCompleteBillings.AutoSize = True
        Me.lblCompleteBillings.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompleteBillings.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblCompleteBillings.Location = New System.Drawing.Point(83, 39)
        Me.lblCompleteBillings.Name = "lblCompleteBillings"
        Me.lblCompleteBillings.Size = New System.Drawing.Size(14, 13)
        Me.lblCompleteBillings.TabIndex = 22
        Me.lblCompleteBillings.Text = "0"
        '
        'Label45
        '
        Me.Label45.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label45.AutoSize = True
        Me.Label45.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label45.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label45.Location = New System.Drawing.Point(6, 39)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(34, 13)
        Me.Label45.TabIndex = 21
        Me.Label45.Text = "Total"
        '
        'lblCustGlobalBilling
        '
        Me.lblCustGlobalBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustGlobalBilling.AutoSize = True
        Me.lblCustGlobalBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustGlobalBilling.Location = New System.Drawing.Point(535, 26)
        Me.lblCustGlobalBilling.Name = "lblCustGlobalBilling"
        Me.lblCustGlobalBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblCustGlobalBilling.TabIndex = 20
        Me.lblCustGlobalBilling.Text = "0"
        '
        'lblCustSpareBilling
        '
        Me.lblCustSpareBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustSpareBilling.AutoSize = True
        Me.lblCustSpareBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustSpareBilling.Location = New System.Drawing.Point(245, 26)
        Me.lblCustSpareBilling.Name = "lblCustSpareBilling"
        Me.lblCustSpareBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblCustSpareBilling.TabIndex = 19
        Me.lblCustSpareBilling.Text = "0"
        '
        'lblCustRepairBilling
        '
        Me.lblCustRepairBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustRepairBilling.AutoSize = True
        Me.lblCustRepairBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustRepairBilling.Location = New System.Drawing.Point(204, 26)
        Me.lblCustRepairBilling.Name = "lblCustRepairBilling"
        Me.lblCustRepairBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblCustRepairBilling.TabIndex = 18
        Me.lblCustRepairBilling.Text = "0"
        '
        'lblCustPartsBilling
        '
        Me.lblCustPartsBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustPartsBilling.AutoSize = True
        Me.lblCustPartsBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustPartsBilling.Location = New System.Drawing.Point(161, 26)
        Me.lblCustPartsBilling.Name = "lblCustPartsBilling"
        Me.lblCustPartsBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblCustPartsBilling.TabIndex = 17
        Me.lblCustPartsBilling.Text = "0"
        '
        'lblCustDriverBilling
        '
        Me.lblCustDriverBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustDriverBilling.AutoSize = True
        Me.lblCustDriverBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustDriverBilling.Location = New System.Drawing.Point(124, 26)
        Me.lblCustDriverBilling.Name = "lblCustDriverBilling"
        Me.lblCustDriverBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblCustDriverBilling.TabIndex = 16
        Me.lblCustDriverBilling.Text = "0"
        '
        'lblCustCompleteBilling
        '
        Me.lblCustCompleteBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCustCompleteBilling.AutoSize = True
        Me.lblCustCompleteBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCustCompleteBilling.Location = New System.Drawing.Point(83, 26)
        Me.lblCustCompleteBilling.Name = "lblCustCompleteBilling"
        Me.lblCustCompleteBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblCustCompleteBilling.TabIndex = 15
        Me.lblCustCompleteBilling.Text = "0"
        '
        'Label38
        '
        Me.Label38.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label38.AutoSize = True
        Me.Label38.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.Location = New System.Drawing.Point(4, 26)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(36, 13)
        Me.Label38.TabIndex = 14
        Me.Label38.Text = "Cust."
        '
        'lblInterGlobalBilling
        '
        Me.lblInterGlobalBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterGlobalBilling.AutoSize = True
        Me.lblInterGlobalBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterGlobalBilling.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterGlobalBilling.Location = New System.Drawing.Point(535, 13)
        Me.lblInterGlobalBilling.Name = "lblInterGlobalBilling"
        Me.lblInterGlobalBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblInterGlobalBilling.TabIndex = 13
        Me.lblInterGlobalBilling.Text = "0"
        '
        'lblInterSpareBilling
        '
        Me.lblInterSpareBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterSpareBilling.AutoSize = True
        Me.lblInterSpareBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterSpareBilling.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterSpareBilling.Location = New System.Drawing.Point(245, 13)
        Me.lblInterSpareBilling.Name = "lblInterSpareBilling"
        Me.lblInterSpareBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblInterSpareBilling.TabIndex = 12
        Me.lblInterSpareBilling.Text = "0"
        '
        'lblInterRepairBilling
        '
        Me.lblInterRepairBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterRepairBilling.AutoSize = True
        Me.lblInterRepairBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterRepairBilling.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterRepairBilling.Location = New System.Drawing.Point(204, 13)
        Me.lblInterRepairBilling.Name = "lblInterRepairBilling"
        Me.lblInterRepairBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblInterRepairBilling.TabIndex = 11
        Me.lblInterRepairBilling.Text = "0"
        '
        'lblInterPartsBilling
        '
        Me.lblInterPartsBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterPartsBilling.AutoSize = True
        Me.lblInterPartsBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterPartsBilling.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterPartsBilling.Location = New System.Drawing.Point(161, 13)
        Me.lblInterPartsBilling.Name = "lblInterPartsBilling"
        Me.lblInterPartsBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblInterPartsBilling.TabIndex = 10
        Me.lblInterPartsBilling.Text = "0"
        '
        'lblInterDriverBilling
        '
        Me.lblInterDriverBilling.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblInterDriverBilling.AutoSize = True
        Me.lblInterDriverBilling.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInterDriverBilling.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblInterDriverBilling.Location = New System.Drawing.Point(124, 13)
        Me.lblInterDriverBilling.Name = "lblInterDriverBilling"
        Me.lblInterDriverBilling.Size = New System.Drawing.Size(14, 13)
        Me.lblInterDriverBilling.TabIndex = 9
        Me.lblInterDriverBilling.Text = "0"
        '
        'Label19
        '
        Me.Label19.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label19.Location = New System.Drawing.Point(3, 13)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(37, 13)
        Me.Label19.TabIndex = 7
        Me.Label19.Text = "Inter."
        '
        'Label18
        '
        Me.Label18.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(512, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(37, 13)
        Me.Label18.TabIndex = 6
        Me.Label18.Text = "Global"
        '
        'Label17
        '
        Me.Label17.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(224, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(35, 13)
        Me.Label17.TabIndex = 5
        Me.Label17.Text = "Spare"
        '
        'Label16
        '
        Me.Label16.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(181, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(37, 13)
        Me.Label16.TabIndex = 4
        Me.Label16.Text = "Repair"
        '
        'Label15
        '
        Me.Label15.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(144, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(31, 13)
        Me.Label15.TabIndex = 3
        Me.Label15.Text = "Parts"
        '
        'Label14
        '
        Me.Label14.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(103, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(35, 13)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "Driver"
        '
        'Label13
        '
        Me.Label13.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(46, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(51, 13)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Complete"
        '
        'PrintDialog1
        '
        Me.PrintDialog1.UseEXDialog = True
        '
        'ucOnTimePerformance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "ucOnTimePerformance"
        Me.Size = New System.Drawing.Size(1108, 672)
        CType(Me.chBooking, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chBilling, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chOTP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        Me.SplitContainer3.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chBooking As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents lblStart As System.Windows.Forms.Label
    Friend WithEvents lblEnddate As System.Windows.Forms.Label
    Friend WithEvents dtpStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnReload As System.Windows.Forms.Button
    Friend WithEvents lblBD1 As System.Windows.Forms.Label
    Friend WithEvents lblBD2 As System.Windows.Forms.Label
    Friend WithEvents pf1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents chBilling As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents chOTP As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbxHorizontalPossible As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents clbBusiness2 As System.Windows.Forms.CheckedListBox
    Friend WithEvents clbBusiness As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblOTPValue As System.Windows.Forms.Label
    Friend WithEvents lblAverageOnTime As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblGlobalBookings As System.Windows.Forms.Label
    Friend WithEvents lblSpareBookings As System.Windows.Forms.Label
    Friend WithEvents lblRepairBookings As System.Windows.Forms.Label
    Friend WithEvents lblPartsBookings As System.Windows.Forms.Label
    Friend WithEvents lblDriverBookings As System.Windows.Forms.Label
    Friend WithEvents lblCompleteBbookings As System.Windows.Forms.Label
    Friend WithEvents cbxAggreation As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cbxFormula As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents lblCustComplet As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblInterDriver As System.Windows.Forms.Label
    Friend WithEvents lblInterParts As System.Windows.Forms.Label
    Friend WithEvents lblInterRepair As System.Windows.Forms.Label
    Friend WithEvents lblInterSpare As System.Windows.Forms.Label
    Friend WithEvents lblInterGlobal As System.Windows.Forms.Label
    Friend WithEvents lblCustDriver As System.Windows.Forms.Label
    Friend WithEvents lblCustParts As System.Windows.Forms.Label
    Friend WithEvents lblCustRepair As System.Windows.Forms.Label
    Friend WithEvents lblCustSpare As System.Windows.Forms.Label
    Friend WithEvents lblCustGlobal As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblInterComplete As System.Windows.Forms.Label
    Friend WithEvents lblInterCompleteBilling As System.Windows.Forms.Label
    Friend WithEvents lblGlobalBillings As System.Windows.Forms.Label
    Friend WithEvents lblSpareBillings As System.Windows.Forms.Label
    Friend WithEvents lblRepairBillings As System.Windows.Forms.Label
    Friend WithEvents lblPartsBillings As System.Windows.Forms.Label
    Friend WithEvents lblDriiverBillings As System.Windows.Forms.Label
    Friend WithEvents lblCompleteBillings As System.Windows.Forms.Label
    Friend WithEvents Label45 As System.Windows.Forms.Label
    Friend WithEvents lblCustGlobalBilling As System.Windows.Forms.Label
    Friend WithEvents lblCustSpareBilling As System.Windows.Forms.Label
    Friend WithEvents lblCustRepairBilling As System.Windows.Forms.Label
    Friend WithEvents lblCustPartsBilling As System.Windows.Forms.Label
    Friend WithEvents lblCustDriverBilling As System.Windows.Forms.Label
    Friend WithEvents lblCustCompleteBilling As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents lblInterGlobalBilling As System.Windows.Forms.Label
    Friend WithEvents lblInterSpareBilling As System.Windows.Forms.Label
    Friend WithEvents lblInterRepairBilling As System.Windows.Forms.Label
    Friend WithEvents lblInterPartsBilling As System.Windows.Forms.Label
    Friend WithEvents lblInterDriverBilling As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog

End Class
