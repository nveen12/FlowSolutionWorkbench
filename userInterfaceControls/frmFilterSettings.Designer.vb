<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFilterSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFilterSettings))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cbFilters = New System.Windows.Forms.ComboBox
        Me.lblMessages = New System.Windows.Forms.Label
        Me.cbObjects = New System.Windows.Forms.ComboBox
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnAddFilter = New System.Windows.Forms.Button
        Me.lblObject = New System.Windows.Forms.Label
        Me.tlpCurrentFilters = New System.Windows.Forms.TableLayoutPanel
        Me.cbUserFilterName = New System.Windows.Forms.ComboBox
        Me.cbUserImplementedTemplate = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnSaveChanges = New System.Windows.Forms.Button
        Me.cbColumns = New System.Windows.Forms.ComboBox
        Me.btnDelete = New System.Windows.Forms.Button
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ShareFiltersWithUsersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShareFiltersWithTheCurrentOrganizationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShareFilterWithAllUsersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.cbFilters)
        Me.GroupBox1.Controls.Add(Me.lblMessages)
        Me.GroupBox1.Controls.Add(Me.cbObjects)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 24)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(449, 80)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filter "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 52)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Filter"
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "View"
        '
        'cbFilters
        '
        Me.cbFilters.FormattingEnabled = True
        Me.cbFilters.Location = New System.Drawing.Point(48, 48)
        Me.cbFilters.Name = "cbFilters"
        Me.cbFilters.Size = New System.Drawing.Size(233, 21)
        Me.cbFilters.TabIndex = 5
        Me.cbFilters.Visible = False
        '
        'lblMessages
        '
        Me.lblMessages.AutoSize = True
        Me.lblMessages.Location = New System.Drawing.Point(287, 20)
        Me.lblMessages.Name = "lblMessages"
        Me.lblMessages.Size = New System.Drawing.Size(55, 13)
        Me.lblMessages.TabIndex = 4
        Me.lblMessages.Text = "Messages"
        '
        'cbObjects
        '
        Me.cbObjects.FormattingEnabled = True
        Me.cbObjects.Location = New System.Drawing.Point(48, 21)
        Me.cbObjects.Name = "cbObjects"
        Me.cbObjects.Size = New System.Drawing.Size(233, 21)
        Me.cbObjects.TabIndex = 0
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(370, 152)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(90, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = "Add column"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnAddFilter
        '
        Me.btnAddFilter.Location = New System.Drawing.Point(13, 151)
        Me.btnAddFilter.Name = "btnAddFilter"
        Me.btnAddFilter.Size = New System.Drawing.Size(75, 23)
        Me.btnAddFilter.TabIndex = 1
        Me.btnAddFilter.Text = "Add new"
        Me.btnAddFilter.UseVisualStyleBackColor = True
        '
        'lblObject
        '
        Me.lblObject.AutoSize = True
        Me.lblObject.Location = New System.Drawing.Point(385, 9)
        Me.lblObject.Name = "lblObject"
        Me.lblObject.Size = New System.Drawing.Size(75, 13)
        Me.lblObject.TabIndex = 5
        Me.lblObject.Text = "Current Object"
        Me.lblObject.Visible = False
        '
        'tlpCurrentFilters
        '
        Me.tlpCurrentFilters.AutoScroll = True
        Me.tlpCurrentFilters.ColumnCount = 1
        Me.tlpCurrentFilters.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpCurrentFilters.Location = New System.Drawing.Point(12, 180)
        Me.tlpCurrentFilters.Name = "tlpCurrentFilters"
        Me.tlpCurrentFilters.RowCount = 2
        Me.tlpCurrentFilters.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53.0!))
        Me.tlpCurrentFilters.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53.0!))
        Me.tlpCurrentFilters.Size = New System.Drawing.Size(449, 439)
        Me.tlpCurrentFilters.TabIndex = 6
        '
        'cbUserFilterName
        '
        Me.cbUserFilterName.FormattingEnabled = True
        Me.cbUserFilterName.Location = New System.Drawing.Point(12, 125)
        Me.cbUserFilterName.Name = "cbUserFilterName"
        Me.cbUserFilterName.Size = New System.Drawing.Size(173, 21)
        Me.cbUserFilterName.TabIndex = 6
        '
        'cbUserImplementedTemplate
        '
        Me.cbUserImplementedTemplate.FormattingEnabled = True
        Me.cbUserImplementedTemplate.Location = New System.Drawing.Point(191, 125)
        Me.cbUserImplementedTemplate.Name = "cbUserImplementedTemplate"
        Me.cbUserImplementedTemplate.Size = New System.Drawing.Size(173, 21)
        Me.cbUserImplementedTemplate.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Enter Your Filter Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(189, 109)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Implemented Template"
        '
        'btnSaveChanges
        '
        Me.btnSaveChanges.Location = New System.Drawing.Point(370, 123)
        Me.btnSaveChanges.Name = "btnSaveChanges"
        Me.btnSaveChanges.Size = New System.Drawing.Size(91, 23)
        Me.btnSaveChanges.TabIndex = 10
        Me.btnSaveChanges.Text = "Save Changes"
        Me.btnSaveChanges.UseVisualStyleBackColor = True
        '
        'cbColumns
        '
        Me.cbColumns.FormattingEnabled = True
        Me.cbColumns.Location = New System.Drawing.Point(192, 151)
        Me.cbColumns.Name = "cbColumns"
        Me.cbColumns.Size = New System.Drawing.Size(173, 21)
        Me.cbColumns.TabIndex = 11
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(94, 152)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnDelete.TabIndex = 12
        Me.btnDelete.Text = "Delete View"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShareFiltersWithUsersToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(472, 24)
        Me.MenuStrip1.TabIndex = 13
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ShareFiltersWithUsersToolStripMenuItem
        '
        Me.ShareFiltersWithUsersToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ShareFiltersWithTheCurrentOrganizationToolStripMenuItem, Me.ShareFilterWithAllUsersToolStripMenuItem})
        Me.ShareFiltersWithUsersToolStripMenuItem.Name = "ShareFiltersWithUsersToolStripMenuItem"
        Me.ShareFiltersWithUsersToolStripMenuItem.Size = New System.Drawing.Size(132, 20)
        Me.ShareFiltersWithUsersToolStripMenuItem.Text = "Share Filters with Users"
        '
        'ShareFiltersWithTheCurrentOrganizationToolStripMenuItem
        '
        Me.ShareFiltersWithTheCurrentOrganizationToolStripMenuItem.Name = "ShareFiltersWithTheCurrentOrganizationToolStripMenuItem"
        Me.ShareFiltersWithTheCurrentOrganizationToolStripMenuItem.Size = New System.Drawing.Size(362, 22)
        Me.ShareFiltersWithTheCurrentOrganizationToolStripMenuItem.Text = "Share one Filter with one user from the current organization"
        '
        'ShareFilterWithAllUsersToolStripMenuItem
        '
        Me.ShareFilterWithAllUsersToolStripMenuItem.Name = "ShareFilterWithAllUsersToolStripMenuItem"
        Me.ShareFilterWithAllUsersToolStripMenuItem.Size = New System.Drawing.Size(362, 22)
        Me.ShareFilterWithAllUsersToolStripMenuItem.Text = "Share one Filter with one user from the warehouse"
        '
        'frmFilterSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(472, 631)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.cbColumns)
        Me.Controls.Add(Me.btnSaveChanges)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnAddFilter)
        Me.Controls.Add(Me.cbUserImplementedTemplate)
        Me.Controls.Add(Me.cbUserFilterName)
        Me.Controls.Add(Me.tlpCurrentFilters)
        Me.Controls.Add(Me.lblObject)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximumSize = New System.Drawing.Size(480, 665)
        Me.MinimumSize = New System.Drawing.Size(480, 665)
        Me.Name = "frmFilterSettings"
        Me.Text = "Filter Settings"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbObjects As System.Windows.Forms.ComboBox
    Friend WithEvents lblObject As System.Windows.Forms.Label
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnAddFilter As System.Windows.Forms.Button
    Friend WithEvents tlpCurrentFilters As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblMessages As System.Windows.Forms.Label
    Friend WithEvents cbFilters As System.Windows.Forms.ComboBox
    Friend WithEvents cbUserFilterName As System.Windows.Forms.ComboBox
    Friend WithEvents cbUserImplementedTemplate As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSaveChanges As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbColumns As System.Windows.Forms.ComboBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ShareFiltersWithUsersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShareFiltersWithTheCurrentOrganizationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShareFilterWithAllUsersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
