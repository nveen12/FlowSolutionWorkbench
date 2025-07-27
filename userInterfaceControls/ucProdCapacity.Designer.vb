<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucProdCapacity
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
        Me.dgvProdCapacity = New System.Windows.Forms.DataGridView
        Me.btnHide = New System.Windows.Forms.Button
        Me.cmsWorkOngrid = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ExportToXMLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveOrderOfColumnsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.sfdExcel = New System.Windows.Forms.SaveFileDialog
        CType(Me.dgvProdCapacity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsWorkOngrid.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvProdCapacity
        '
        Me.dgvProdCapacity.AllowUserToAddRows = False
        Me.dgvProdCapacity.AllowUserToDeleteRows = False
        Me.dgvProdCapacity.AllowUserToOrderColumns = True
        Me.dgvProdCapacity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProdCapacity.ContextMenuStrip = Me.cmsWorkOngrid
        Me.dgvProdCapacity.Location = New System.Drawing.Point(0, 27)
        Me.dgvProdCapacity.Name = "dgvProdCapacity"
        Me.dgvProdCapacity.Size = New System.Drawing.Size(1192, 772)
        Me.dgvProdCapacity.TabIndex = 0
        '
        'btnHide
        '
        Me.btnHide.Location = New System.Drawing.Point(3, 3)
        Me.btnHide.Name = "btnHide"
        Me.btnHide.Size = New System.Drawing.Size(109, 23)
        Me.btnHide.TabIndex = 1
        Me.btnHide.Text = "Remove Screen"
        Me.btnHide.UseVisualStyleBackColor = True
        '
        'cmsWorkOngrid
        '
        Me.cmsWorkOngrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportToXMLToolStripMenuItem, Me.SaveOrderOfColumnsToolStripMenuItem})
        Me.cmsWorkOngrid.Name = "cmsWorkOngrid"
        Me.cmsWorkOngrid.Size = New System.Drawing.Size(182, 48)
        '
        'ExportToXMLToolStripMenuItem
        '
		Me.ExportToXMLToolStripMenuItem.Image = Global.Flow_Solution_Workbenches.My.Resources.Resources.ExportToXMLFil
        Me.ExportToXMLToolStripMenuItem.Name = "ExportToXMLToolStripMenuItem"
        Me.ExportToXMLToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.ExportToXMLToolStripMenuItem.Text = "Export to XML"
        '
        'SaveOrderOfColumnsToolStripMenuItem
        '
        Me.SaveOrderOfColumnsToolStripMenuItem.Name = "SaveOrderOfColumnsToolStripMenuItem"
        Me.SaveOrderOfColumnsToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.SaveOrderOfColumnsToolStripMenuItem.Text = "Save order of columns"
        '
        'sfdExcel
        '
        Me.sfdExcel.Filter = "XML | *.xml"
        '
        'ucProdCapacity
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnHide)
        Me.Controls.Add(Me.dgvProdCapacity)
        Me.Name = "ucProdCapacity"
        Me.Size = New System.Drawing.Size(1192, 802)
        CType(Me.dgvProdCapacity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsWorkOngrid.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvProdCapacity As System.Windows.Forms.DataGridView
    Friend WithEvents btnHide As System.Windows.Forms.Button
    Friend WithEvents cmsWorkOngrid As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ExportToXMLToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveOrderOfColumnsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sfdExcel As System.Windows.Forms.SaveFileDialog

End Class
