<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucFilterSetup
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
        Me.cbSQLOperation = New System.Windows.Forms.ComboBox
        Me.lblColumn = New System.Windows.Forms.Label
        Me.cbValues = New System.Windows.Forms.ComboBox
        Me.lblStart = New System.Windows.Forms.Label
        Me.lblEnd = New System.Windows.Forms.Label
        Me.txtEnd = New System.Windows.Forms.TextBox
        Me.txtStart = New System.Windows.Forms.TextBox
        Me.cbMiddleString = New System.Windows.Forms.CheckBox
        Me.btnDel = New System.Windows.Forms.Button
        Me.cbConnection = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'cbSQLOperation
        '
        Me.cbSQLOperation.FormattingEnabled = True
        Me.cbSQLOperation.Location = New System.Drawing.Point(58, 22)
        Me.cbSQLOperation.Name = "cbSQLOperation"
        Me.cbSQLOperation.Size = New System.Drawing.Size(123, 21)
        Me.cbSQLOperation.TabIndex = 0
        '
        'lblColumn
        '
        Me.lblColumn.AutoSize = True
        Me.lblColumn.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblColumn.Location = New System.Drawing.Point(56, 1)
        Me.lblColumn.Name = "lblColumn"
        Me.lblColumn.Size = New System.Drawing.Size(93, 13)
        Me.lblColumn.TabIndex = 1
        Me.lblColumn.Text = "Current Column"
        '
        'cbValues
        '
        Me.cbValues.FormattingEnabled = True
        Me.cbValues.Location = New System.Drawing.Point(185, 22)
        Me.cbValues.Name = "cbValues"
        Me.cbValues.Size = New System.Drawing.Size(161, 21)
        Me.cbValues.TabIndex = 2
        '
        'lblStart
        '
        Me.lblStart.AutoSize = True
        Me.lblStart.Location = New System.Drawing.Point(343, 6)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(29, 13)
        Me.lblStart.TabIndex = 3
        Me.lblStart.Text = "Start"
        Me.lblStart.Visible = False
        '
        'lblEnd
        '
        Me.lblEnd.AutoSize = True
        Me.lblEnd.Location = New System.Drawing.Point(346, 31)
        Me.lblEnd.Name = "lblEnd"
        Me.lblEnd.Size = New System.Drawing.Size(26, 13)
        Me.lblEnd.TabIndex = 4
        Me.lblEnd.Text = "End"
        Me.lblEnd.Visible = False
        '
        'txtEnd
        '
        Me.txtEnd.Location = New System.Drawing.Point(378, 27)
        Me.txtEnd.Name = "txtEnd"
        Me.txtEnd.Size = New System.Drawing.Size(47, 20)
        Me.txtEnd.TabIndex = 5
        Me.txtEnd.Visible = False
        '
        'txtStart
        '
        Me.txtStart.Location = New System.Drawing.Point(378, 3)
        Me.txtStart.Name = "txtStart"
        Me.txtStart.Size = New System.Drawing.Size(47, 20)
        Me.txtStart.TabIndex = 6
        Me.txtStart.Visible = False
        '
        'cbMiddleString
        '
        Me.cbMiddleString.AutoSize = True
        Me.cbMiddleString.Location = New System.Drawing.Point(274, 4)
        Me.cbMiddleString.Name = "cbMiddleString"
        Me.cbMiddleString.Size = New System.Drawing.Size(70, 17)
        Me.cbMiddleString.TabIndex = 7
        Me.cbMiddleString.Text = "Substring"
        Me.cbMiddleString.UseVisualStyleBackColor = True
        Me.cbMiddleString.Visible = False
        '
        'btnDel
        '
        Me.btnDel.Location = New System.Drawing.Point(3, -1)
        Me.btnDel.Name = "btnDel"
        Me.btnDel.Size = New System.Drawing.Size(40, 20)
        Me.btnDel.TabIndex = 8
        Me.btnDel.Text = "Del"
        Me.btnDel.UseVisualStyleBackColor = True
        '
        'cbConnection
        '
        Me.cbConnection.FormattingEnabled = True
        Me.cbConnection.Items.AddRange(New Object() {"AND", "OR"})
        Me.cbConnection.Location = New System.Drawing.Point(3, 22)
        Me.cbConnection.Name = "cbConnection"
        Me.cbConnection.Size = New System.Drawing.Size(52, 21)
        Me.cbConnection.TabIndex = 9
        '
        'ucFilterSetup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cbConnection)
        Me.Controls.Add(Me.btnDel)
        Me.Controls.Add(Me.cbMiddleString)
        Me.Controls.Add(Me.txtStart)
        Me.Controls.Add(Me.txtEnd)
        Me.Controls.Add(Me.lblEnd)
        Me.Controls.Add(Me.lblStart)
        Me.Controls.Add(Me.cbValues)
        Me.Controls.Add(Me.lblColumn)
        Me.Controls.Add(Me.cbSQLOperation)
        Me.Name = "ucFilterSetup"
        Me.Size = New System.Drawing.Size(431, 51)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbSQLOperation As System.Windows.Forms.ComboBox
    Friend WithEvents lblColumn As System.Windows.Forms.Label
    Friend WithEvents cbValues As System.Windows.Forms.ComboBox
    Friend WithEvents lblStart As System.Windows.Forms.Label
    Friend WithEvents lblEnd As System.Windows.Forms.Label
    Friend WithEvents txtEnd As System.Windows.Forms.TextBox
    Friend WithEvents txtStart As System.Windows.Forms.TextBox
    Friend WithEvents cbMiddleString As System.Windows.Forms.CheckBox
    Friend WithEvents btnDel As System.Windows.Forms.Button
    Friend WithEvents cbConnection As System.Windows.Forms.ComboBox

End Class
