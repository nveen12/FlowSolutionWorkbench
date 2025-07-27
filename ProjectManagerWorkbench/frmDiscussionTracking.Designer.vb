<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiscussionTracking
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDiscussionTracking))
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.btnForReview = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnMyComments = New System.Windows.Forms.Button()
        Me.btnDone = New System.Windows.Forms.Button()
        Me.btnFind = New System.Windows.Forms.Button()
        Me.txtSearchPhrase = New System.Windows.Forms.TextBox()
        Me.btnCurrentDiscussion = New System.Windows.Forms.Button()
        Me.btnMyNotifications = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnBeingNotified = New System.Windows.Forms.Button()
        Me.cbxNoRecords = New System.Windows.Forms.ComboBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.OutOfOfficeAgentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckUserToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetupOutOfOfficeStatusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.dgvDiscussion = New System.Windows.Forms.DataGridView()
        Me.rtbDiscussion = New System.Windows.Forms.RichTextBox()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        CType(Me.dgvDiscussion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
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
        Me.SplitContainer1.Panel2.Controls.Add(Me.rtbDiscussion)
        Me.SplitContainer1.Size = New System.Drawing.Size(1112, 502)
        Me.SplitContainer1.SplitterDistance = 513
        Me.SplitContainer1.TabIndex = 0
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.MinimumSize = New System.Drawing.Size(513, 502)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnForReview)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnRefresh)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnMyComments)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnDone)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnFind)
        Me.SplitContainer2.Panel1.Controls.Add(Me.txtSearchPhrase)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnCurrentDiscussion)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnMyNotifications)
        Me.SplitContainer2.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer2.Panel1.Controls.Add(Me.btnBeingNotified)
        Me.SplitContainer2.Panel1.Controls.Add(Me.cbxNoRecords)
        Me.SplitContainer2.Panel1.Controls.Add(Me.MenuStrip1)
        Me.SplitContainer2.Panel1.Margin = New System.Windows.Forms.Padding(5)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.dgvDiscussion)
        Me.SplitContainer2.Size = New System.Drawing.Size(513, 502)
        Me.SplitContainer2.SplitterDistance = 250
        Me.SplitContainer2.TabIndex = 0
        '
        'btnForReview
        '
        Me.btnForReview.Location = New System.Drawing.Point(258, 28)
        Me.btnForReview.Name = "btnForReview"
        Me.btnForReview.Size = New System.Drawing.Size(81, 23)
        Me.btnForReview.TabIndex = 12
        Me.btnForReview.Text = "For review"
        Me.btnForReview.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(454, 28)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(52, 23)
        Me.btnRefresh.TabIndex = 10
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        Me.btnRefresh.Visible = False
        '
        'btnMyComments
        '
        Me.btnMyComments.Location = New System.Drawing.Point(172, 27)
        Me.btnMyComments.Name = "btnMyComments"
        Me.btnMyComments.Size = New System.Drawing.Size(80, 23)
        Me.btnMyComments.TabIndex = 8
        Me.btnMyComments.Text = "My comments"
        Me.btnMyComments.UseVisualStyleBackColor = True
        '
        'btnDone
        '
        Me.btnDone.Location = New System.Drawing.Point(282, 56)
        Me.btnDone.Name = "btnDone"
        Me.btnDone.Size = New System.Drawing.Size(68, 23)
        Me.btnDone.TabIndex = 7
        Me.btnDone.Text = "Reviewed"
        Me.btnDone.UseVisualStyleBackColor = True
        Me.btnDone.Visible = False
        '
        'btnFind
        '
        Me.btnFind.Location = New System.Drawing.Point(234, 56)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(42, 23)
        Me.btnFind.TabIndex = 6
        Me.btnFind.Text = "Find"
        Me.btnFind.UseVisualStyleBackColor = True
        '
        'txtSearchPhrase
        '
        Me.txtSearchPhrase.Location = New System.Drawing.Point(13, 58)
        Me.txtSearchPhrase.Name = "txtSearchPhrase"
        Me.txtSearchPhrase.Size = New System.Drawing.Size(215, 20)
        Me.txtSearchPhrase.TabIndex = 5
        '
        'btnCurrentDiscussion
        '
        Me.btnCurrentDiscussion.Location = New System.Drawing.Point(345, 28)
        Me.btnCurrentDiscussion.Name = "btnCurrentDiscussion"
        Me.btnCurrentDiscussion.Size = New System.Drawing.Size(105, 23)
        Me.btnCurrentDiscussion.TabIndex = 4
        Me.btnCurrentDiscussion.Text = "Current discussion"
        Me.btnCurrentDiscussion.UseVisualStyleBackColor = True
        '
        'btnMyNotifications
        '
        Me.btnMyNotifications.Location = New System.Drawing.Point(97, 28)
        Me.btnMyNotifications.Name = "btnMyNotifications"
        Me.btnMyNotifications.Size = New System.Drawing.Size(69, 23)
        Me.btnMyNotifications.TabIndex = 3
        Me.btnMyNotifications.Text = "I notified"
        Me.btnMyNotifications.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(356, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "No of Records"
        '
        'btnBeingNotified
        '
        Me.btnBeingNotified.Location = New System.Drawing.Point(12, 28)
        Me.btnBeingNotified.Name = "btnBeingNotified"
        Me.btnBeingNotified.Size = New System.Drawing.Size(79, 23)
        Me.btnBeingNotified.TabIndex = 1
        Me.btnBeingNotified.Text = "I was notified"
        Me.btnBeingNotified.UseVisualStyleBackColor = True
        '
        'cbxNoRecords
        '
        Me.cbxNoRecords.FormattingEnabled = True
        Me.cbxNoRecords.Items.AddRange(New Object() {"10", "100", "1000", "10000"})
        Me.cbxNoRecords.Location = New System.Drawing.Point(438, 58)
        Me.cbxNoRecords.Name = "cbxNoRecords"
        Me.cbxNoRecords.Size = New System.Drawing.Size(68, 21)
        Me.cbxNoRecords.TabIndex = 0
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.Left
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OutOfOfficeAgentToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(131, 250)
        Me.MenuStrip1.TabIndex = 11
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'OutOfOfficeAgentToolStripMenuItem
        '
        Me.OutOfOfficeAgentToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckUserToolStripMenuItem, Me.SetupOutOfOfficeStatusToolStripMenuItem})
        Me.OutOfOfficeAgentToolStripMenuItem.Name = "OutOfOfficeAgentToolStripMenuItem"
        Me.OutOfOfficeAgentToolStripMenuItem.Size = New System.Drawing.Size(118, 19)
        Me.OutOfOfficeAgentToolStripMenuItem.Text = "Out Of Office Agent"
        '
        'CheckUserToolStripMenuItem
        '
        Me.CheckUserToolStripMenuItem.Name = "CheckUserToolStripMenuItem"
        Me.CheckUserToolStripMenuItem.Size = New System.Drawing.Size(213, 22)
        Me.CheckUserToolStripMenuItem.Text = "Check user"
        '
        'SetupOutOfOfficeStatusToolStripMenuItem
        '
        Me.SetupOutOfOfficeStatusToolStripMenuItem.Name = "SetupOutOfOfficeStatusToolStripMenuItem"
        Me.SetupOutOfOfficeStatusToolStripMenuItem.Size = New System.Drawing.Size(213, 22)
        Me.SetupOutOfOfficeStatusToolStripMenuItem.Text = "Setup Out Of Office Status"
        '
        'dgvDiscussion
        '
        Me.dgvDiscussion.AllowUserToAddRows = False
        Me.dgvDiscussion.AllowUserToDeleteRows = False
        Me.dgvDiscussion.AllowUserToOrderColumns = True
        Me.dgvDiscussion.BackgroundColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDiscussion.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvDiscussion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvDiscussion.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvDiscussion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDiscussion.Location = New System.Drawing.Point(0, 0)
        Me.dgvDiscussion.Name = "dgvDiscussion"
        Me.dgvDiscussion.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDiscussion.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvDiscussion.Size = New System.Drawing.Size(513, 248)
        Me.dgvDiscussion.TabIndex = 0
        '
        'rtbDiscussion
        '
        Me.rtbDiscussion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbDiscussion.Location = New System.Drawing.Point(0, 0)
        Me.rtbDiscussion.Name = "rtbDiscussion"
        Me.rtbDiscussion.Size = New System.Drawing.Size(595, 502)
        Me.rtbDiscussion.TabIndex = 0
        Me.rtbDiscussion.Text = ""
        '
        'frmDiscussionTracking
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(1112, 502)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1128, 540)
        Me.Name = "frmDiscussionTracking"
        Me.Text = "Discussion Tracking"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.PerformLayout()
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        CType(Me.dgvDiscussion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBeingNotified As System.Windows.Forms.Button
    Friend WithEvents cbxNoRecords As System.Windows.Forms.ComboBox
    Friend WithEvents dgvDiscussion As System.Windows.Forms.DataGridView
    Friend WithEvents rtbDiscussion As System.Windows.Forms.RichTextBox
    Friend WithEvents btnCurrentDiscussion As System.Windows.Forms.Button
    Friend WithEvents btnMyNotifications As System.Windows.Forms.Button
    Friend WithEvents btnDone As System.Windows.Forms.Button
    Friend WithEvents btnFind As System.Windows.Forms.Button
    Friend WithEvents txtSearchPhrase As System.Windows.Forms.TextBox
    Friend WithEvents btnMyComments As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents OutOfOfficeAgentToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckUserToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetupOutOfOfficeStatusToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnForReview As Button
End Class
