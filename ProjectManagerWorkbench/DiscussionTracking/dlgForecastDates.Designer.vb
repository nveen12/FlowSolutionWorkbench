<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgForecastDates
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
        Me.mc0 = New System.Windows.Forms.MonthCalendar
        Me.mc1 = New System.Windows.Forms.MonthCalendar
        Me.rtbForecastHistory = New System.Windows.Forms.RichTextBox
        Me.txtNewDate = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtReason = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.cbComplete = New System.Windows.Forms.CheckBox
        Me.TableLayoutPanel1.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(393, 350)
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
        'mc0
        '
        Me.mc0.Location = New System.Drawing.Point(36, 18)
        Me.mc0.Name = "mc0"
        Me.mc0.ShowWeekNumbers = True
        Me.mc0.TabIndex = 1
        '
        'mc1
        '
        Me.mc1.Location = New System.Drawing.Point(344, 18)
        Me.mc1.Name = "mc1"
        Me.mc1.ShowToday = False
        Me.mc1.ShowTodayCircle = False
        Me.mc1.ShowWeekNumbers = True
        Me.mc1.TabIndex = 2
        '
        'rtbForecastHistory
        '
        Me.rtbForecastHistory.Location = New System.Drawing.Point(11, 196)
        Me.rtbForecastHistory.Name = "rtbForecastHistory"
        Me.rtbForecastHistory.Size = New System.Drawing.Size(528, 122)
        Me.rtbForecastHistory.TabIndex = 4
        Me.rtbForecastHistory.Text = ""
        '
        'txtNewDate
        '
        Me.txtNewDate.Location = New System.Drawing.Point(73, 350)
        Me.txtNewDate.Name = "txtNewDate"
        Me.txtNewDate.ReadOnly = True
        Me.txtNewDate.Size = New System.Drawing.Size(180, 20)
        Me.txtNewDate.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 353)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "New Date"
        '
        'txtReason
        '
        Me.txtReason.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.txtReason.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.txtReason.Location = New System.Drawing.Point(73, 324)
        Me.txtReason.Name = "txtReason"
        Me.txtReason.Size = New System.Drawing.Size(466, 20)
        Me.txtReason.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 327)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Reason"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(34, 1)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(148, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "This month / Current Forecast"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(341, 1)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Next month"
        '
        'cbComplete
        '
        Me.cbComplete.AutoSize = True
        Me.cbComplete.Location = New System.Drawing.Point(259, 353)
        Me.cbComplete.Name = "cbComplete"
        Me.cbComplete.Size = New System.Drawing.Size(99, 17)
        Me.cbComplete.TabIndex = 11
        Me.cbComplete.Text = "Complete Order"
        Me.cbComplete.UseVisualStyleBackColor = True
        '
        'dlgForecastDates
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(551, 391)
        Me.Controls.Add(Me.cbComplete)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtReason)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtNewDate)
        Me.Controls.Add(Me.rtbForecastHistory)
        Me.Controls.Add(Me.mc1)
        Me.Controls.Add(Me.mc0)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgForecastDates"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Forecasting"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents mc0 As System.Windows.Forms.MonthCalendar
    Friend WithEvents mc1 As System.Windows.Forms.MonthCalendar
    Friend WithEvents rtbForecastHistory As System.Windows.Forms.RichTextBox
    Friend WithEvents txtNewDate As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtReason As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbComplete As System.Windows.Forms.CheckBox

End Class
