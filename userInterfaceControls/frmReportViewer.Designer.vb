<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReportViewer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReportViewer))
        Me.rvStandard = New Microsoft.Reporting.WinForms.ReportViewer()
        Me.SuspendLayout()
        '
        'rvStandard
        '
        Me.rvStandard.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rvStandard.Location = New System.Drawing.Point(0, 0)
        Me.rvStandard.Name = "rvStandard"
        Me.rvStandard.ServerReport.BearerToken = Nothing
        Me.rvStandard.Size = New System.Drawing.Size(1268, 798)
        Me.rvStandard.TabIndex = 0
        '
        'frmReportViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1268, 798)
        Me.Controls.Add(Me.rvStandard)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmReportViewer"
        Me.Text = "Report"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents rvStandard As Microsoft.Reporting.WinForms.ReportViewer
End Class
