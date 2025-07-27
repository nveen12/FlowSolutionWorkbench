<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgCustomFilter
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblAdvice = New System.Windows.Forms.Label
        Me.gbColumn = New System.Windows.Forms.GroupBox
        Me.rbOr = New System.Windows.Forms.RadioButton
        Me.rbAnd = New System.Windows.Forms.RadioButton
        Me.cbValueList2 = New System.Windows.Forms.ComboBox
        Me.cbSQlOperation2 = New System.Windows.Forms.ComboBox
        Me.cbValueList1 = New System.Windows.Forms.ComboBox
        Me.cbSQLOperation1 = New System.Windows.Forms.ComboBox
        Me.TableLayoutPanel1.SuspendLayout()
        Me.gbColumn.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(598, 172)
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
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Show rows where"
        '
        'lblAdvice
        '
        Me.lblAdvice.AutoSize = True
        Me.lblAdvice.Location = New System.Drawing.Point(12, 179)
        Me.lblAdvice.Name = "lblAdvice"
        Me.lblAdvice.Size = New System.Drawing.Size(211, 13)
        Me.lblAdvice.TabIndex = 3
        Me.lblAdvice.Text = "Use % to represent any series of characters"
        '
        'gbColumn
        '
        Me.gbColumn.Controls.Add(Me.rbOr)
        Me.gbColumn.Controls.Add(Me.rbAnd)
        Me.gbColumn.Controls.Add(Me.cbValueList2)
        Me.gbColumn.Controls.Add(Me.cbSQlOperation2)
        Me.gbColumn.Controls.Add(Me.cbValueList1)
        Me.gbColumn.Controls.Add(Me.cbSQLOperation1)
        Me.gbColumn.Location = New System.Drawing.Point(12, 41)
        Me.gbColumn.Name = "gbColumn"
        Me.gbColumn.Size = New System.Drawing.Size(741, 121)
        Me.gbColumn.TabIndex = 4
        Me.gbColumn.TabStop = False
        Me.gbColumn.Text = "GroupBox1"
        '
        'rbOr
        '
        Me.rbOr.AutoSize = True
        Me.rbOr.Location = New System.Drawing.Point(61, 46)
        Me.rbOr.Name = "rbOr"
        Me.rbOr.Size = New System.Drawing.Size(41, 17)
        Me.rbOr.TabIndex = 5
        Me.rbOr.Text = "OR"
        Me.rbOr.UseVisualStyleBackColor = True
        '
        'rbAnd
        '
        Me.rbAnd.AutoSize = True
        Me.rbAnd.Checked = True
        Me.rbAnd.Location = New System.Drawing.Point(7, 47)
        Me.rbAnd.Name = "rbAnd"
        Me.rbAnd.Size = New System.Drawing.Size(48, 17)
        Me.rbAnd.TabIndex = 4
        Me.rbAnd.TabStop = True
        Me.rbAnd.Text = "AND"
        Me.rbAnd.UseVisualStyleBackColor = True
        '
        'cbValueList2
        '
        Me.cbValueList2.FormattingEnabled = True
        Me.cbValueList2.Location = New System.Drawing.Point(186, 74)
        Me.cbValueList2.Name = "cbValueList2"
        Me.cbValueList2.Size = New System.Drawing.Size(543, 21)
        Me.cbValueList2.TabIndex = 3
        '
        'cbSQlOperation2
        '
        Me.cbSQlOperation2.FormattingEnabled = True
        Me.cbSQlOperation2.Location = New System.Drawing.Point(6, 74)
        Me.cbSQlOperation2.Name = "cbSQlOperation2"
        Me.cbSQlOperation2.Size = New System.Drawing.Size(174, 21)
        Me.cbSQlOperation2.TabIndex = 2
        '
        'cbValueList1
        '
        Me.cbValueList1.FormattingEnabled = True
        Me.cbValueList1.Location = New System.Drawing.Point(186, 19)
        Me.cbValueList1.Name = "cbValueList1"
        Me.cbValueList1.Size = New System.Drawing.Size(543, 21)
        Me.cbValueList1.TabIndex = 1
        '
        'cbSQLOperation1
        '
        Me.cbSQLOperation1.FormattingEnabled = True
        Me.cbSQLOperation1.Location = New System.Drawing.Point(6, 19)
        Me.cbSQLOperation1.Name = "cbSQLOperation1"
        Me.cbSQLOperation1.Size = New System.Drawing.Size(174, 21)
        Me.cbSQLOperation1.TabIndex = 0
        '
        'dlgCustomFilter
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(756, 213)
        Me.Controls.Add(Me.gbColumn)
        Me.Controls.Add(Me.lblAdvice)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgCustomFilter"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.gbColumn.ResumeLayout(False)
        Me.gbColumn.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblAdvice As System.Windows.Forms.Label
    Friend WithEvents gbColumn As System.Windows.Forms.GroupBox
    Friend WithEvents rbOr As System.Windows.Forms.RadioButton
    Friend WithEvents rbAnd As System.Windows.Forms.RadioButton
    Friend WithEvents cbValueList2 As System.Windows.Forms.ComboBox
    Friend WithEvents cbSQlOperation2 As System.Windows.Forms.ComboBox
    Friend WithEvents cbValueList1 As System.Windows.Forms.ComboBox
    Friend WithEvents cbSQLOperation1 As System.Windows.Forms.ComboBox

End Class
