<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOneDashboard
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOneDashboard))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PrintForm1 = New Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(Me.components)
        Me.paContent = New System.Windows.Forms.Panel
        Me.tcOverall = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.tpBillings = New System.Windows.Forms.TabPage
        Me.dgvBacklog = New System.Windows.Forms.DataGridView
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnLoad = New System.Windows.Forms.Button
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblStart = New System.Windows.Forms.Label
        Me.dtpStart = New System.Windows.Forms.DateTimePicker
        Me.btnDelete = New System.Windows.Forms.Button
        Me.gbRules = New System.Windows.Forms.GroupBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.btnApply = New System.Windows.Forms.Button
        Me.cbReasons = New System.Windows.Forms.ComboBox
        Me.MenuStrip1.SuspendLayout()
        Me.paContent.SuspendLayout()
        Me.tcOverall.SuspendLayout()
        Me.tpBillings.SuspendLayout()
        CType(Me.dgvBacklog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.gbRules.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1107, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PrintToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'PrintToolStripMenuItem
        '
        Me.PrintToolStripMenuItem.Name = "PrintToolStripMenuItem"
        Me.PrintToolStripMenuItem.Size = New System.Drawing.Size(96, 22)
        Me.PrintToolStripMenuItem.Text = "Print"
        '
        'PrintForm1
        '
        Me.PrintForm1.DocumentName = "document"
        Me.PrintForm1.Form = Me
        Me.PrintForm1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter
        Me.PrintForm1.PrinterSettings = CType(resources.GetObject("PrintForm1.PrinterSettings"), System.Drawing.Printing.PrinterSettings)
        Me.PrintForm1.PrintFileName = Nothing
        '
        'paContent
        '
        Me.paContent.Controls.Add(Me.tcOverall)
        Me.paContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.paContent.Location = New System.Drawing.Point(0, 24)
        Me.paContent.Name = "paContent"
        Me.paContent.Size = New System.Drawing.Size(1107, 703)
        Me.paContent.TabIndex = 1
        '
        'tcOverall
        '
        Me.tcOverall.Controls.Add(Me.TabPage1)
        Me.tcOverall.Controls.Add(Me.tpBillings)
        Me.tcOverall.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcOverall.Location = New System.Drawing.Point(0, 0)
        Me.tcOverall.Name = "tcOverall"
        Me.tcOverall.SelectedIndex = 0
        Me.tcOverall.Size = New System.Drawing.Size(1107, 703)
        Me.tcOverall.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1099, 677)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Booking Billings OTP"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'tpBillings
        '
        Me.tpBillings.Controls.Add(Me.dgvBacklog)
        Me.tpBillings.Controls.Add(Me.GroupBox1)
        Me.tpBillings.Controls.Add(Me.gbRules)
        Me.tpBillings.Location = New System.Drawing.Point(4, 22)
        Me.tpBillings.Name = "tpBillings"
        Me.tpBillings.Padding = New System.Windows.Forms.Padding(3)
        Me.tpBillings.Size = New System.Drawing.Size(1099, 677)
        Me.tpBillings.TabIndex = 1
        Me.tpBillings.Text = "Billings"
        Me.tpBillings.UseVisualStyleBackColor = True
        '
        'dgvBacklog
        '
        Me.dgvBacklog.AllowUserToAddRows = False
        Me.dgvBacklog.AllowUserToDeleteRows = False
        Me.dgvBacklog.AllowUserToOrderColumns = True
        Me.dgvBacklog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvBacklog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvBacklog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBacklog.Location = New System.Drawing.Point(6, 152)
        Me.dgvBacklog.Name = "dgvBacklog"
        Me.dgvBacklog.Size = New System.Drawing.Size(1085, 517)
        Me.dgvBacklog.TabIndex = 2
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnLoad)
        Me.GroupBox1.Controls.Add(Me.dtpEnd)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.lblStart)
        Me.GroupBox1.Controls.Add(Me.dtpStart)
        Me.GroupBox1.Controls.Add(Me.btnDelete)
        Me.GroupBox1.Location = New System.Drawing.Point(215, 10)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(222, 135)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Delete Reasons"
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(127, 16)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(89, 41)
        Me.btnLoad.TabIndex = 5
        Me.btnLoad.Text = "Load Data"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'dtpEnd
        '
        Me.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEnd.Location = New System.Drawing.Point(42, 40)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.Size = New System.Drawing.Size(79, 20)
        Me.dtpEnd.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "End"
        '
        'lblStart
        '
        Me.lblStart.AutoSize = True
        Me.lblStart.Location = New System.Drawing.Point(7, 20)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(29, 13)
        Me.lblStart.TabIndex = 2
        Me.lblStart.Text = "Start"
        '
        'dtpStart
        '
        Me.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStart.Location = New System.Drawing.Point(42, 16)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.Size = New System.Drawing.Size(79, 20)
        Me.dtpStart.TabIndex = 1
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(6, 106)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(117, 23)
        Me.btnDelete.TabIndex = 0
        Me.btnDelete.Text = "Delete Reasons"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'gbRules
        '
        Me.gbRules.Controls.Add(Me.txtDescription)
        Me.gbRules.Controls.Add(Me.btnApply)
        Me.gbRules.Controls.Add(Me.cbReasons)
        Me.gbRules.Location = New System.Drawing.Point(9, 7)
        Me.gbRules.Name = "gbRules"
        Me.gbRules.Size = New System.Drawing.Size(200, 138)
        Me.gbRules.TabIndex = 0
        Me.gbRules.TabStop = False
        Me.gbRules.Text = "Reasons not being late"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(7, 47)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(180, 56)
        Me.txtDescription.TabIndex = 2
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(7, 109)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(120, 23)
        Me.btnApply.TabIndex = 1
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'cbReasons
        '
        Me.cbReasons.FormattingEnabled = True
        Me.cbReasons.Items.AddRange(New Object() {"Combined Delivery", "Awaiting Payment", "Different Appointment forAcceptance Test", "Wrong Promise Date", "Communication with Customer"})
        Me.cbReasons.Location = New System.Drawing.Point(6, 19)
        Me.cbReasons.Name = "cbReasons"
        Me.cbReasons.Size = New System.Drawing.Size(181, 21)
        Me.cbReasons.TabIndex = 0
        '
        'frmOneDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1107, 727)
        Me.Controls.Add(Me.paContent)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmOneDashboard"
        Me.Text = "Business Cockpit "
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.paContent.ResumeLayout(False)
        Me.tcOverall.ResumeLayout(False)
        Me.tpBillings.ResumeLayout(False)
        CType(Me.dgvBacklog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gbRules.ResumeLayout(False)
        Me.gbRules.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PrintForm1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
    Friend WithEvents paContent As System.Windows.Forms.Panel
    Friend WithEvents tcOverall As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents tpBillings As System.Windows.Forms.TabPage
    Friend WithEvents gbRules As System.Windows.Forms.GroupBox
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents cbReasons As System.Windows.Forms.ComboBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents dgvBacklog As System.Windows.Forms.DataGridView
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents dtpEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblStart As System.Windows.Forms.Label
    Friend WithEvents dtpStart As System.Windows.Forms.DateTimePicker
End Class
