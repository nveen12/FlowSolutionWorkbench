<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgManageColumnOrder
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
        Me.lblSelectView = New System.Windows.Forms.Label
        Me.cbView = New System.Windows.Forms.ComboBox
        Me.lblManage = New System.Windows.Forms.Label
        Me.lbColumnOrder = New System.Windows.Forms.ListBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnTop = New System.Windows.Forms.Button
        Me.btnUp = New System.Windows.Forms.Button
        Me.btnDown = New System.Windows.Forms.Button
        Me.btnBottom = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblSelectView
        '
        Me.lblSelectView.AutoSize = True
        Me.lblSelectView.Location = New System.Drawing.Point(16, 20)
        Me.lblSelectView.Name = "lblSelectView"
        Me.lblSelectView.Size = New System.Drawing.Size(63, 13)
        Me.lblSelectView.TabIndex = 2
        Me.lblSelectView.Text = "Select View"
        '
        'cbView
        '
        Me.cbView.FormattingEnabled = True
        Me.cbView.Items.AddRange(New Object() {"Select", "MRPShortagesView", "POQueueView", "salesOrderLinesView", "supplyDemandMRPView", "WIPQueueView"})
        Me.cbView.Location = New System.Drawing.Point(128, 20)
        Me.cbView.Name = "cbView"
        Me.cbView.Size = New System.Drawing.Size(176, 21)
        Me.cbView.TabIndex = 3
        '
        'lblManage
        '
        Me.lblManage.AutoSize = True
        Me.lblManage.Location = New System.Drawing.Point(16, 55)
        Me.lblManage.Name = "lblManage"
        Me.lblManage.Size = New System.Drawing.Size(127, 13)
        Me.lblManage.TabIndex = 4
        Me.lblManage.Text = "Manage order of columns"
        '
        'lbColumnOrder
        '
        Me.lbColumnOrder.FormattingEnabled = True
        Me.lbColumnOrder.Location = New System.Drawing.Point(19, 81)
        Me.lbColumnOrder.Name = "lbColumnOrder"
        Me.lbColumnOrder.ScrollAlwaysVisible = True
        Me.lbColumnOrder.Size = New System.Drawing.Size(285, 368)
        Me.lbColumnOrder.TabIndex = 5
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(177, 465)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 6
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(267, 465)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnTop
        '
        Me.btnTop.BackgroundImage = Global.Flow_Solution_Workbenches.My.Resources.Resources.moveTopIcon
        Me.btnTop.Location = New System.Drawing.Point(326, 181)
        Me.btnTop.Name = "btnTop"
        Me.btnTop.Size = New System.Drawing.Size(16, 20)
        Me.btnTop.TabIndex = 8
        Me.btnTop.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.BackgroundImage = Global.Flow_Solution_Workbenches.My.Resources.Resources.moveUpIcon
        Me.btnUp.Location = New System.Drawing.Point(326, 218)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(16, 20)
        Me.btnUp.TabIndex = 9
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'btnDown
        '
        Me.btnDown.BackgroundImage = Global.Flow_Solution_Workbenches.My.Resources.Resources.moveDownIcon
        Me.btnDown.Location = New System.Drawing.Point(326, 258)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(16, 20)
        Me.btnDown.TabIndex = 10
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnBottom
        '
        Me.btnBottom.BackgroundImage = Global.Flow_Solution_Workbenches.My.Resources.Resources.moveBottomIcon
        Me.btnBottom.Location = New System.Drawing.Point(326, 297)
        Me.btnBottom.Name = "btnBottom"
        Me.btnBottom.Size = New System.Drawing.Size(16, 20)
        Me.btnBottom.TabIndex = 11
        Me.btnBottom.UseVisualStyleBackColor = True
        '
        'dlgManageColumnOrder
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(361, 501)
        Me.Controls.Add(Me.btnBottom)
        Me.Controls.Add(Me.btnDown)
        Me.Controls.Add(Me.btnUp)
        Me.Controls.Add(Me.btnTop)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lbColumnOrder)
        Me.Controls.Add(Me.lblManage)
        Me.Controls.Add(Me.cbView)
        Me.Controls.Add(Me.lblSelectView)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgManageColumnOrder"
        Me.Text = "Manage Column Order"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSelectView As System.Windows.Forms.Label
    Friend WithEvents cbView As System.Windows.Forms.ComboBox
    Friend WithEvents lblManage As System.Windows.Forms.Label
    Friend WithEvents lbColumnOrder As System.Windows.Forms.ListBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnTop As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents btnDown As System.Windows.Forms.Button
    Friend WithEvents btnBottom As System.Windows.Forms.Button
End Class
