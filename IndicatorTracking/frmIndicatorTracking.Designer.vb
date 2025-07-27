<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIndicatorTracking
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmIndicatorTracking))
        Me.scMain = New System.Windows.Forms.SplitContainer
        Me.tvIndicator = New System.Windows.Forms.TreeView
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MyBoardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'scMain
        '
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.Location = New System.Drawing.Point(0, 24)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.Controls.Add(Me.tvIndicator)
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.AutoScroll = True
        Me.scMain.Panel2.Controls.Add(Me.tlpMain)
        Me.scMain.Size = New System.Drawing.Size(1046, 697)
        Me.scMain.SplitterDistance = 171
        Me.scMain.TabIndex = 0
        '
        'tvIndicator
        '
        Me.tvIndicator.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvIndicator.Location = New System.Drawing.Point(0, 0)
        Me.tvIndicator.Name = "tvIndicator"
        Me.tvIndicator.Size = New System.Drawing.Size(171, 697)
        Me.tvIndicator.TabIndex = 0
        '
        'tlpMain
        '
        Me.tlpMain.AutoScroll = True
        Me.tlpMain.AutoScrollMargin = New System.Drawing.Size(30, 0)
        Me.tlpMain.AutoScrollMinSize = New System.Drawing.Size(40, 0)
        Me.tlpMain.AutoSize = True
        Me.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetPartial
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 1
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 706.0!))
        Me.tlpMain.Size = New System.Drawing.Size(871, 697)
        Me.tlpMain.TabIndex = 0
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1046, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MyBoardToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'MyBoardToolStripMenuItem
        '
        Me.MyBoardToolStripMenuItem.Name = "MyBoardToolStripMenuItem"
        Me.MyBoardToolStripMenuItem.Size = New System.Drawing.Size(119, 22)
        Me.MyBoardToolStripMenuItem.Text = "My Board"
        '
        'frmIndicatorTracking
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1046, 721)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmIndicatorTracking"
        Me.Text = "Indicator Tracking"
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.Panel2.PerformLayout()
        Me.scMain.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents tvIndicator As System.Windows.Forms.TreeView
    Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MyBoardToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
