<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgTwoGridsDisplay
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.dgvHeader = New System.Windows.Forms.DataGridView
        Me.dgvFooter = New System.Windows.Forms.DataGridView
        Me.lblTextInformation = New System.Windows.Forms.Label
        Me.lblNonNaftaContent = New System.Windows.Forms.Label
        Me.lblSalesOrderInformation = New System.Windows.Forms.Label
        Me.lblBOMInformation = New System.Windows.Forms.Label
        Me.txtReportMessages = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.dgvHeader, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvFooter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(611, 610)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'dgvHeader
        '
        Me.dgvHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvHeader.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.dgvHeader.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvHeader.Location = New System.Drawing.Point(12, 33)
        Me.dgvHeader.Name = "dgvHeader"
        Me.dgvHeader.Size = New System.Drawing.Size(731, 181)
        Me.dgvHeader.TabIndex = 1
        '
        'dgvFooter
        '
        Me.dgvFooter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvFooter.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.dgvFooter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFooter.Location = New System.Drawing.Point(12, 342)
        Me.dgvFooter.Name = "dgvFooter"
        Me.dgvFooter.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvFooter.RowHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvFooter.Size = New System.Drawing.Size(731, 237)
        Me.dgvFooter.TabIndex = 3
        '
        'lblTextInformation
        '
        Me.lblTextInformation.AutoSize = True
        Me.lblTextInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTextInformation.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblTextInformation.Location = New System.Drawing.Point(12, 217)
        Me.lblTextInformation.Name = "lblTextInformation"
        Me.lblTextInformation.Size = New System.Drawing.Size(121, 13)
        Me.lblTextInformation.TabIndex = 4
        Me.lblTextInformation.Text = "Non NAFTA content"
        '
        'lblNonNaftaContent
        '
        Me.lblNonNaftaContent.AutoSize = True
        Me.lblNonNaftaContent.Location = New System.Drawing.Point(123, 219)
        Me.lblNonNaftaContent.Name = "lblNonNaftaContent"
        Me.lblNonNaftaContent.Size = New System.Drawing.Size(0, 13)
        Me.lblNonNaftaContent.TabIndex = 5
        '
        'lblSalesOrderInformation
        '
        Me.lblSalesOrderInformation.AutoSize = True
        Me.lblSalesOrderInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSalesOrderInformation.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblSalesOrderInformation.Location = New System.Drawing.Point(12, 17)
        Me.lblSalesOrderInformation.Name = "lblSalesOrderInformation"
        Me.lblSalesOrderInformation.Size = New System.Drawing.Size(137, 13)
        Me.lblSalesOrderInformation.TabIndex = 6
        Me.lblSalesOrderInformation.Text = "Sales order information"
        '
        'lblBOMInformation
        '
        Me.lblBOMInformation.AutoSize = True
        Me.lblBOMInformation.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBOMInformation.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblBOMInformation.Location = New System.Drawing.Point(12, 326)
        Me.lblBOMInformation.Name = "lblBOMInformation"
        Me.lblBOMInformation.Size = New System.Drawing.Size(153, 13)
        Me.lblBOMInformation.TabIndex = 7
        Me.lblBOMInformation.Text = "Bill of material information"
        '
        'txtReportMessages
        '
        Me.txtReportMessages.Location = New System.Drawing.Point(15, 234)
        Me.txtReportMessages.Multiline = True
        Me.txtReportMessages.Name = "txtReportMessages"
        Me.txtReportMessages.Size = New System.Drawing.Size(728, 89)
        Me.txtReportMessages.TabIndex = 8
        '
        'dlgTwoGridsDisplay
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Silver
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(769, 651)
        Me.Controls.Add(Me.txtReportMessages)
        Me.Controls.Add(Me.lblBOMInformation)
        Me.Controls.Add(Me.lblSalesOrderInformation)
        Me.Controls.Add(Me.lblNonNaftaContent)
        Me.Controls.Add(Me.lblTextInformation)
        Me.Controls.Add(Me.dgvFooter)
        Me.Controls.Add(Me.dgvHeader)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgTwoGridsDisplay"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.dgvHeader, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvFooter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents dgvHeader As System.Windows.Forms.DataGridView
    Friend WithEvents dgvFooter As System.Windows.Forms.DataGridView
    Friend WithEvents lblTextInformation As System.Windows.Forms.Label
    Friend WithEvents lblNonNaftaContent As System.Windows.Forms.Label
    Friend WithEvents lblSalesOrderInformation As System.Windows.Forms.Label
    Friend WithEvents lblBOMInformation As System.Windows.Forms.Label
    Friend WithEvents txtReportMessages As System.Windows.Forms.TextBox

End Class
