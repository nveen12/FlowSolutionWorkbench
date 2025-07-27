<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucOTP
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
        Me.scMain = New System.Windows.Forms.SplitContainer
        Me.scTop = New System.Windows.Forms.SplitContainer
        Me.dgvOTP = New System.Windows.Forms.DataGridView
        Me.dgvBacklog = New System.Windows.Forms.DataGridView
        Me.gbDeleteReasons = New System.Windows.Forms.GroupBox
        Me.cbLate = New System.Windows.Forms.CheckBox
        Me.btnLoad = New System.Windows.Forms.Button
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.lblStart = New System.Windows.Forms.Label
        Me.dtpStart = New System.Windows.Forms.DateTimePicker
        Me.gbRules = New System.Windows.Forms.GroupBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.btnApply = New System.Windows.Forms.Button
        Me.cbReasons = New System.Windows.Forms.ComboBox
        Me.btnDelete = New System.Windows.Forms.Button
        Me.gbTargets = New System.Windows.Forms.GroupBox
        Me.btnDeleteTarget = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpUntilTarget = New System.Windows.Forms.DateTimePicker
        Me.lblStart2 = New System.Windows.Forms.Label
        Me.lblTarget = New System.Windows.Forms.Label
        Me.dgvTargets = New System.Windows.Forms.DataGridView
        Me.txtTarget = New System.Windows.Forms.TextBox
        Me.btnCreateTarget = New System.Windows.Forms.Button
        Me.dtpStartTarget = New System.Windows.Forms.DateTimePicker
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.scTop.Panel2.SuspendLayout()
        Me.scTop.SuspendLayout()
        CType(Me.dgvOTP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvBacklog, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDeleteReasons.SuspendLayout()
        Me.gbRules.SuspendLayout()
        Me.gbTargets.SuspendLayout()
        CType(Me.dgvTargets, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'scMain
        '
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.Location = New System.Drawing.Point(0, 0)
        Me.scMain.Name = "scMain"
        Me.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.scTop)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.gbTargets)
        Me.scMain.Panel2.Controls.Add(Me.dgvBacklog)
        Me.scMain.Panel2.Controls.Add(Me.gbDeleteReasons)
        Me.scMain.Panel2.Controls.Add(Me.gbRules)
        Me.scMain.Size = New System.Drawing.Size(930, 675)
        Me.scMain.SplitterDistance = 310
        Me.scMain.TabIndex = 0
        '
        'scTop
        '
        Me.scTop.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scTop.Location = New System.Drawing.Point(0, 0)
        Me.scTop.Name = "scTop"
        '
        'scTop.Panel2
        '
        Me.scTop.Panel2.Controls.Add(Me.dgvOTP)
        Me.scTop.Size = New System.Drawing.Size(930, 310)
        Me.scTop.SplitterDistance = 453
        Me.scTop.TabIndex = 0
        '
        'dgvOTP
        '
        Me.dgvOTP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvOTP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvOTP.Location = New System.Drawing.Point(0, 0)
        Me.dgvOTP.Name = "dgvOTP"
        Me.dgvOTP.Size = New System.Drawing.Size(473, 310)
        Me.dgvOTP.TabIndex = 0
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
        Me.dgvBacklog.Location = New System.Drawing.Point(0, 159)
        Me.dgvBacklog.Name = "dgvBacklog"
        Me.dgvBacklog.Size = New System.Drawing.Size(930, 202)
        Me.dgvBacklog.TabIndex = 3
        '
        'gbDeleteReasons
        '
        Me.gbDeleteReasons.Controls.Add(Me.cbLate)
        Me.gbDeleteReasons.Controls.Add(Me.btnLoad)
        Me.gbDeleteReasons.Controls.Add(Me.dtpEnd)
        Me.gbDeleteReasons.Controls.Add(Me.Label1)
        Me.gbDeleteReasons.Controls.Add(Me.btnSearch)
        Me.gbDeleteReasons.Controls.Add(Me.txtSearch)
        Me.gbDeleteReasons.Controls.Add(Me.lblStart)
        Me.gbDeleteReasons.Controls.Add(Me.dtpStart)
        Me.gbDeleteReasons.Location = New System.Drawing.Point(209, 6)
        Me.gbDeleteReasons.Name = "gbDeleteReasons"
        Me.gbDeleteReasons.Size = New System.Drawing.Size(222, 147)
        Me.gbDeleteReasons.TabIndex = 2
        Me.gbDeleteReasons.TabStop = False
        Me.gbDeleteReasons.Text = "Delete Reasons"
        '
        'cbLate
        '
        Me.cbLate.AutoSize = True
        Me.cbLate.Checked = True
        Me.cbLate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbLate.Location = New System.Drawing.Point(42, 67)
        Me.cbLate.Name = "cbLate"
        Me.cbLate.Size = New System.Drawing.Size(47, 17)
        Me.cbLate.TabIndex = 6
        Me.cbLate.Text = "Late"
        Me.cbLate.UseVisualStyleBackColor = True
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
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(138, 86)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(75, 24)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "Search"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(6, 90)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(126, 20)
        Me.txtSearch.TabIndex = 5
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
        'gbRules
        '
        Me.gbRules.Controls.Add(Me.txtDescription)
        Me.gbRules.Controls.Add(Me.btnApply)
        Me.gbRules.Controls.Add(Me.cbReasons)
        Me.gbRules.Controls.Add(Me.btnDelete)
        Me.gbRules.Location = New System.Drawing.Point(3, 3)
        Me.gbRules.Name = "gbRules"
        Me.gbRules.Size = New System.Drawing.Size(200, 150)
        Me.gbRules.TabIndex = 1
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
        Me.btnApply.Size = New System.Drawing.Size(71, 23)
        Me.btnApply.TabIndex = 1
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'cbReasons
        '
        Me.cbReasons.FormattingEnabled = True
        Me.cbReasons.Items.AddRange(New Object() {"Combined Delivery", "Awaiting Payment", "Different Appointment for Acceptance Test", "Wrong Promise Date", "Communication with Customer"})
        Me.cbReasons.Location = New System.Drawing.Point(6, 19)
        Me.cbReasons.Name = "cbReasons"
        Me.cbReasons.Size = New System.Drawing.Size(181, 21)
        Me.cbReasons.TabIndex = 0
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(84, 109)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(103, 23)
        Me.btnDelete.TabIndex = 0
        Me.btnDelete.Text = "Delete Reasons"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'gbTargets
        '
        Me.gbTargets.Controls.Add(Me.btnDeleteTarget)
        Me.gbTargets.Controls.Add(Me.Label2)
        Me.gbTargets.Controls.Add(Me.dtpUntilTarget)
        Me.gbTargets.Controls.Add(Me.lblStart2)
        Me.gbTargets.Controls.Add(Me.lblTarget)
        Me.gbTargets.Controls.Add(Me.dgvTargets)
        Me.gbTargets.Controls.Add(Me.txtTarget)
        Me.gbTargets.Controls.Add(Me.btnCreateTarget)
        Me.gbTargets.Controls.Add(Me.dtpStartTarget)
        Me.gbTargets.Location = New System.Drawing.Point(437, 2)
        Me.gbTargets.Name = "gbTargets"
        Me.gbTargets.Size = New System.Drawing.Size(489, 153)
        Me.gbTargets.TabIndex = 15
        Me.gbTargets.TabStop = False
        Me.gbTargets.Text = "Targets"
        '
        'btnDeleteTarget
        '
        Me.btnDeleteTarget.Location = New System.Drawing.Point(356, 32)
        Me.btnDeleteTarget.Name = "btnDeleteTarget"
        Me.btnDeleteTarget.Size = New System.Drawing.Size(84, 23)
        Me.btnDeleteTarget.TabIndex = 23
        Me.btnDeleteTarget.Text = "Delete Target"
        Me.btnDeleteTarget.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(173, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(28, 13)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Until"
        '
        'dtpUntilTarget
        '
        Me.dtpUntilTarget.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpUntilTarget.Location = New System.Drawing.Point(176, 35)
        Me.dtpUntilTarget.Name = "dtpUntilTarget"
        Me.dtpUntilTarget.Size = New System.Drawing.Size(84, 20)
        Me.dtpUntilTarget.TabIndex = 21
        '
        'lblStart2
        '
        Me.lblStart2.AutoSize = True
        Me.lblStart2.Location = New System.Drawing.Point(83, 20)
        Me.lblStart2.Name = "lblStart2"
        Me.lblStart2.Size = New System.Drawing.Size(30, 13)
        Me.lblStart2.TabIndex = 20
        Me.lblStart2.Text = "From"
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.Location = New System.Drawing.Point(4, 20)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(38, 13)
        Me.lblTarget.TabIndex = 19
        Me.lblTarget.Text = "Target"
        '
        'dgvTargets
        '
        Me.dgvTargets.AllowUserToAddRows = False
        Me.dgvTargets.AllowUserToDeleteRows = False
        Me.dgvTargets.BackgroundColor = System.Drawing.SystemColors.ActiveCaption
        Me.dgvTargets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTargets.Location = New System.Drawing.Point(5, 60)
        Me.dgvTargets.Name = "dgvTargets"
        Me.dgvTargets.ReadOnly = True
        Me.dgvTargets.Size = New System.Drawing.Size(478, 91)
        Me.dgvTargets.TabIndex = 18
        '
        'txtTarget
        '
        Me.txtTarget.Location = New System.Drawing.Point(5, 36)
        Me.txtTarget.Name = "txtTarget"
        Me.txtTarget.Size = New System.Drawing.Size(75, 20)
        Me.txtTarget.TabIndex = 17
        '
        'btnCreateTarget
        '
        Me.btnCreateTarget.Location = New System.Drawing.Point(266, 32)
        Me.btnCreateTarget.Name = "btnCreateTarget"
        Me.btnCreateTarget.Size = New System.Drawing.Size(84, 23)
        Me.btnCreateTarget.TabIndex = 16
        Me.btnCreateTarget.Text = "Create Target"
        Me.btnCreateTarget.UseVisualStyleBackColor = True
        '
        'dtpStartTarget
        '
        Me.dtpStartTarget.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartTarget.Location = New System.Drawing.Point(86, 35)
        Me.dtpStartTarget.Name = "dtpStartTarget"
        Me.dtpStartTarget.Size = New System.Drawing.Size(84, 20)
        Me.dtpStartTarget.TabIndex = 15
        '
        'ucOTP
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.scMain)
        Me.Name = "ucOTP"
        Me.Size = New System.Drawing.Size(930, 675)
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.ResumeLayout(False)
        Me.scTop.Panel2.ResumeLayout(False)
        Me.scTop.ResumeLayout(False)
        CType(Me.dgvOTP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvBacklog, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDeleteReasons.ResumeLayout(False)
        Me.gbDeleteReasons.PerformLayout()
        Me.gbRules.ResumeLayout(False)
        Me.gbRules.PerformLayout()
        Me.gbTargets.ResumeLayout(False)
        Me.gbTargets.PerformLayout()
        CType(Me.dgvTargets, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents scTop As System.Windows.Forms.SplitContainer
    Friend WithEvents dgvOTP As System.Windows.Forms.DataGridView
    Friend WithEvents gbRules As System.Windows.Forms.GroupBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents cbReasons As System.Windows.Forms.ComboBox
    Friend WithEvents gbDeleteReasons As System.Windows.Forms.GroupBox
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents dtpEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblStart As System.Windows.Forms.Label
    Friend WithEvents dtpStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents dgvBacklog As System.Windows.Forms.DataGridView
    Friend WithEvents cbLate As System.Windows.Forms.CheckBox
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents gbTargets As System.Windows.Forms.GroupBox
    Friend WithEvents btnDeleteTarget As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpUntilTarget As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblStart2 As System.Windows.Forms.Label
    Friend WithEvents lblTarget As System.Windows.Forms.Label
    Friend WithEvents dgvTargets As System.Windows.Forms.DataGridView
    Friend WithEvents txtTarget As System.Windows.Forms.TextBox
    Friend WithEvents btnCreateTarget As System.Windows.Forms.Button
    Friend WithEvents dtpStartTarget As System.Windows.Forms.DateTimePicker

End Class
