<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgReleaseNotes
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

    Friend WithEvents TableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lblReleaseNotes As System.Windows.Forms.Label
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents lblReleaseDate As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel
        Me.lblVersion = New System.Windows.Forms.Label
        Me.lblReleaseDate = New System.Windows.Forms.Label
        Me.lblReleaseNotes = New System.Windows.Forms.Label
        Me.TextBoxDescription = New System.Windows.Forms.TextBox
        Me.OKButton = New System.Windows.Forms.Button
        Me.cbVersion = New System.Windows.Forms.ComboBox
        Me.lblRelease = New System.Windows.Forms.Label
        Me.TableLayoutPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel
        '
        Me.TableLayoutPanel.ColumnCount = 2
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.65647!))
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.34354!))
        Me.TableLayoutPanel.Controls.Add(Me.lblVersion, 0, 0)
        Me.TableLayoutPanel.Controls.Add(Me.lblReleaseDate, 0, 1)
        Me.TableLayoutPanel.Controls.Add(Me.lblReleaseNotes, 0, 2)
        Me.TableLayoutPanel.Controls.Add(Me.TextBoxDescription, 0, 3)
        Me.TableLayoutPanel.Controls.Add(Me.OKButton, 0, 4)
        Me.TableLayoutPanel.Controls.Add(Me.cbVersion, 1, 0)
        Me.TableLayoutPanel.Controls.Add(Me.lblRelease, 1, 1)
        Me.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel.Location = New System.Drawing.Point(9, 9)
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        Me.TableLayoutPanel.RowCount = 5
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.55556!))
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel.Size = New System.Drawing.Size(719, 258)
        Me.TableLayoutPanel.TabIndex = 0
        '
        'lblVersion
        '
        Me.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblVersion.Location = New System.Drawing.Point(6, 0)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.lblVersion.MaximumSize = New System.Drawing.Size(0, 17)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(82, 17)
        Me.lblVersion.TabIndex = 0
        Me.lblVersion.Text = "Version"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblReleaseDate
        '
        Me.lblReleaseDate.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblReleaseDate.Location = New System.Drawing.Point(6, 28)
        Me.lblReleaseDate.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.lblReleaseDate.MaximumSize = New System.Drawing.Size(0, 17)
        Me.lblReleaseDate.Name = "lblReleaseDate"
        Me.lblReleaseDate.Size = New System.Drawing.Size(82, 17)
        Me.lblReleaseDate.TabIndex = 0
        Me.lblReleaseDate.Text = "Release Date"
        Me.lblReleaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblReleaseNotes
        '
        Me.lblReleaseNotes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblReleaseNotes.Location = New System.Drawing.Point(6, 56)
        Me.lblReleaseNotes.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.lblReleaseNotes.MaximumSize = New System.Drawing.Size(0, 17)
        Me.lblReleaseNotes.Name = "lblReleaseNotes"
        Me.lblReleaseNotes.Size = New System.Drawing.Size(82, 17)
        Me.lblReleaseNotes.TabIndex = 0
        Me.lblReleaseNotes.Text = "Release Notes"
        Me.lblReleaseNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBoxDescription
        '
        Me.TableLayoutPanel.SetColumnSpan(Me.TextBoxDescription, 2)
        Me.TextBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxDescription.Location = New System.Drawing.Point(6, 87)
        Me.TextBoxDescription.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
        Me.TextBoxDescription.Multiline = True
        Me.TextBoxDescription.Name = "TextBoxDescription"
        Me.TextBoxDescription.ReadOnly = True
        Me.TextBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBoxDescription.Size = New System.Drawing.Size(710, 137)
        Me.TextBoxDescription.TabIndex = 0
        Me.TextBoxDescription.TabStop = False
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel.SetColumnSpan(Me.OKButton, 2)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OKButton.Location = New System.Drawing.Point(641, 232)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 0
        Me.OKButton.Text = "&OK"
        '
        'cbVersion
        '
        Me.cbVersion.Dock = System.Windows.Forms.DockStyle.Left
        Me.cbVersion.FormattingEnabled = True
        Me.cbVersion.Location = New System.Drawing.Point(94, 3)
        Me.cbVersion.Name = "cbVersion"
        Me.cbVersion.Size = New System.Drawing.Size(121, 21)
        Me.cbVersion.TabIndex = 1
        '
        'lblRelease
        '
        Me.lblRelease.AutoSize = True
        Me.lblRelease.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblRelease.Location = New System.Drawing.Point(94, 28)
        Me.lblRelease.MaximumSize = New System.Drawing.Size(0, 17)
        Me.lblRelease.Name = "lblRelease"
        Me.lblRelease.Size = New System.Drawing.Size(622, 17)
        Me.lblRelease.TabIndex = 2
        Me.lblRelease.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dlgReleaseNotes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.OKButton
        Me.ClientSize = New System.Drawing.Size(737, 276)
        Me.Controls.Add(Me.TableLayoutPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgReleaseNotes"
        Me.Padding = New System.Windows.Forms.Padding(9)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Release Notes"
        Me.TableLayoutPanel.ResumeLayout(False)
        Me.TableLayoutPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cbVersion As System.Windows.Forms.ComboBox
    Friend WithEvents lblRelease As System.Windows.Forms.Label
    Friend WithEvents TextBoxDescription As System.Windows.Forms.TextBox

End Class
