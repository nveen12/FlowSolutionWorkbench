<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgCopyColumnOrder
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
        Me.cbUsers = New System.Windows.Forms.ComboBox
        Me.lblSelectUser = New System.Windows.Forms.Label
        Me.lblSelectView = New System.Windows.Forms.Label
        Me.cbViewtoCopy = New System.Windows.Forms.ComboBox
        Me.lblPreview = New System.Windows.Forms.Label
        Me.lbColumnNewOrder = New System.Windows.Forms.ListBox
        Me.btnCopy = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'cbUsers
        '
        Me.cbUsers.FormattingEnabled = True
        Me.cbUsers.Location = New System.Drawing.Point(183, 23)
        Me.cbUsers.Name = "cbUsers"
        Me.cbUsers.Size = New System.Drawing.Size(121, 21)
        Me.cbUsers.TabIndex = 0
        '
        'lblSelectUser
        '
        Me.lblSelectUser.AutoSize = True
        Me.lblSelectUser.Location = New System.Drawing.Point(13, 23)
        Me.lblSelectUser.Name = "lblSelectUser"
        Me.lblSelectUser.Size = New System.Drawing.Size(127, 13)
        Me.lblSelectUser.TabIndex = 1
        Me.lblSelectUser.Text = "Select User to Copy From"
        '
        'lblSelectView
        '
        Me.lblSelectView.AutoSize = True
        Me.lblSelectView.Location = New System.Drawing.Point(13, 60)
        Me.lblSelectView.Name = "lblSelectView"
        Me.lblSelectView.Size = New System.Drawing.Size(126, 13)
        Me.lblSelectView.TabIndex = 2
        Me.lblSelectView.Text = "Select View to be Copied"
        '
        'cbViewtoCopy
        '
        Me.cbViewtoCopy.FormattingEnabled = True
        Me.cbViewtoCopy.Items.AddRange(New Object() {"Select", "MRPShortagesView", "POQueueView", "salesOrderLinesView", "supplyDemandMRPView", "WIPQueueView"})
        Me.cbViewtoCopy.Location = New System.Drawing.Point(183, 60)
        Me.cbViewtoCopy.Name = "cbViewtoCopy"
        Me.cbViewtoCopy.Size = New System.Drawing.Size(121, 21)
        Me.cbViewtoCopy.TabIndex = 3
        '
        'lblPreview
        '
        Me.lblPreview.AutoSize = True
        Me.lblPreview.Location = New System.Drawing.Point(16, 98)
        Me.lblPreview.Name = "lblPreview"
        Me.lblPreview.Size = New System.Drawing.Size(149, 13)
        Me.lblPreview.TabIndex = 4
        Me.lblPreview.Text = "Preview new order of columns"
        '
        'lbColumnNewOrder
        '
        Me.lbColumnNewOrder.FormattingEnabled = True
        Me.lbColumnNewOrder.Location = New System.Drawing.Point(19, 124)
        Me.lbColumnNewOrder.Name = "lbColumnNewOrder"
        Me.lbColumnNewOrder.ScrollAlwaysVisible = True
        Me.lbColumnNewOrder.Size = New System.Drawing.Size(285, 368)
        Me.lbColumnNewOrder.TabIndex = 5
        '
        'btnCopy
        '
        Me.btnCopy.Location = New System.Drawing.Point(176, 512)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(75, 23)
        Me.btnCopy.TabIndex = 6
        Me.btnCopy.Text = "Copy"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(266, 512)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'dlgCopyColumnOrder
        '
        Me.AcceptButton = Me.btnCopy
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(353, 547)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.lbColumnNewOrder)
        Me.Controls.Add(Me.lblPreview)
        Me.Controls.Add(Me.cbViewtoCopy)
        Me.Controls.Add(Me.lblSelectView)
        Me.Controls.Add(Me.lblSelectUser)
        Me.Controls.Add(Me.cbUsers)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgCopyColumnOrder"
        Me.Text = "Copy Column Order"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbUsers As System.Windows.Forms.ComboBox
    Friend WithEvents lblSelectUser As System.Windows.Forms.Label
    Friend WithEvents lblSelectView As System.Windows.Forms.Label
    Friend WithEvents cbViewtoCopy As System.Windows.Forms.ComboBox
    Friend WithEvents lblPreview As System.Windows.Forms.Label
    Friend WithEvents lbColumnNewOrder As System.Windows.Forms.ListBox
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
