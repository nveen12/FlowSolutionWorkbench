<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucIndicatorTracker
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.chIndicatorTracker = New System.Windows.Forms.DataVisualization.Charting.Chart
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnMyTracking = New System.Windows.Forms.Button
        Me.btnMinimum = New System.Windows.Forms.Button
        Me.btnMaximum = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.gbConcernAction = New System.Windows.Forms.GroupBox
        Me.cbxClosed = New System.Windows.Forms.CheckBox
        Me.btnAction = New System.Windows.Forms.Button
        Me.cbResponsible = New System.Windows.Forms.ComboBox
        Me.dgvRecentCommunication = New System.Windows.Forms.DataGridView
        Me.txtAction = New System.Windows.Forms.TextBox
        Me.txtConcern = New System.Windows.Forms.TextBox
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnSavePicture = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.ComboBox4 = New System.Windows.Forms.ComboBox
        Me.lblOrganization = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.sfdPicture = New System.Windows.Forms.SaveFileDialog
        Me.cbAnsolut = New System.Windows.Forms.CheckBox
        Me.cbRelativ = New System.Windows.Forms.CheckBox
        Me.ttValues = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnLoad = New System.Windows.Forms.Button
        Me.lbxValueItems = New System.Windows.Forms.ListBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.cbAbsoluteOne = New System.Windows.Forms.CheckBox
        CType(Me.chIndicatorTracker, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbConcernAction.SuspendLayout()
        CType(Me.dgvRecentCommunication, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'chIndicatorTracker
        '
        Me.chIndicatorTracker.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.chIndicatorTracker.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.HorizontalCenter
        Me.chIndicatorTracker.BackImageTransparentColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.chIndicatorTracker.BorderlineColor = System.Drawing.Color.Transparent
        ChartArea1.BackColor = System.Drawing.Color.White
        ChartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash
        ChartArea1.Name = "ChartArea1"
        ChartArea1.ShadowOffset = 1
        Me.chIndicatorTracker.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.chIndicatorTracker.Legends.Add(Legend1)
        Me.chIndicatorTracker.Location = New System.Drawing.Point(3, 3)
        Me.chIndicatorTracker.Name = "chIndicatorTracker"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.chIndicatorTracker.Series.Add(Series1)
        Me.chIndicatorTracker.Size = New System.Drawing.Size(564, 414)
        Me.chIndicatorTracker.TabIndex = 0
        Me.chIndicatorTracker.Text = "Chart1"
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(574, 41)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(106, 20)
        Me.dtpStartDate.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(574, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Start Date"
        '
        'btnMyTracking
        '
        Me.btnMyTracking.Location = New System.Drawing.Point(581, 92)
        Me.btnMyTracking.Name = "btnMyTracking"
        Me.btnMyTracking.Size = New System.Drawing.Size(213, 23)
        Me.btnMyTracking.TabIndex = 3
        Me.btnMyTracking.Text = "Add to my board"
        Me.btnMyTracking.UseVisualStyleBackColor = True
        Me.btnMyTracking.Visible = False
        '
        'btnMinimum
        '
        Me.btnMinimum.Location = New System.Drawing.Point(580, 148)
        Me.btnMinimum.Name = "btnMinimum"
        Me.btnMinimum.Size = New System.Drawing.Size(104, 23)
        Me.btnMinimum.TabIndex = 4
        Me.btnMinimum.Text = "Adjust lower aim"
        Me.btnMinimum.UseVisualStyleBackColor = True
        Me.btnMinimum.Visible = False
        '
        'btnMaximum
        '
        Me.btnMaximum.Location = New System.Drawing.Point(690, 148)
        Me.btnMaximum.Name = "btnMaximum"
        Me.btnMaximum.Size = New System.Drawing.Size(104, 23)
        Me.btnMaximum.TabIndex = 5
        Me.btnMaximum.Text = "Adjust higher aim"
        Me.btnMaximum.UseVisualStyleBackColor = True
        Me.btnMaximum.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(561, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Concern"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(561, 82)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Action"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(561, 153)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Owner"
        '
        'gbConcernAction
        '
        Me.gbConcernAction.Controls.Add(Me.cbxClosed)
        Me.gbConcernAction.Controls.Add(Me.btnAction)
        Me.gbConcernAction.Controls.Add(Me.cbResponsible)
        Me.gbConcernAction.Controls.Add(Me.dgvRecentCommunication)
        Me.gbConcernAction.Controls.Add(Me.txtAction)
        Me.gbConcernAction.Controls.Add(Me.txtConcern)
        Me.gbConcernAction.Controls.Add(Me.DateTimePicker1)
        Me.gbConcernAction.Controls.Add(Me.Label5)
        Me.gbConcernAction.Controls.Add(Me.Label4)
        Me.gbConcernAction.Controls.Add(Me.Label3)
        Me.gbConcernAction.Controls.Add(Me.Label2)
        Me.gbConcernAction.Location = New System.Drawing.Point(3, 423)
        Me.gbConcernAction.Name = "gbConcernAction"
        Me.gbConcernAction.Size = New System.Drawing.Size(803, 227)
        Me.gbConcernAction.TabIndex = 10
        Me.gbConcernAction.TabStop = False
        Me.gbConcernAction.Text = "Concern Action Tab"
        '
        'cbxClosed
        '
        Me.cbxClosed.AutoSize = True
        Me.cbxClosed.Location = New System.Drawing.Point(573, 200)
        Me.cbxClosed.Name = "cbxClosed"
        Me.cbxClosed.Size = New System.Drawing.Size(58, 17)
        Me.cbxClosed.TabIndex = 17
        Me.cbxClosed.Text = "Closed"
        Me.cbxClosed.UseVisualStyleBackColor = True
        '
        'btnAction
        '
        Me.btnAction.Location = New System.Drawing.Point(722, 195)
        Me.btnAction.Name = "btnAction"
        Me.btnAction.Size = New System.Drawing.Size(75, 23)
        Me.btnAction.TabIndex = 16
        Me.btnAction.Text = "Add action"
        Me.btnAction.UseVisualStyleBackColor = True
        '
        'cbResponsible
        '
        Me.cbResponsible.FormattingEnabled = True
        Me.cbResponsible.Items.AddRange(New Object() {"A", "B", "C", "D"})
        Me.cbResponsible.Location = New System.Drawing.Point(614, 145)
        Me.cbResponsible.Name = "cbResponsible"
        Me.cbResponsible.Size = New System.Drawing.Size(183, 21)
        Me.cbResponsible.TabIndex = 15
        '
        'dgvRecentCommunication
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvRecentCommunication.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvRecentCommunication.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvRecentCommunication.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvRecentCommunication.Location = New System.Drawing.Point(7, 20)
        Me.dgvRecentCommunication.Name = "dgvRecentCommunication"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvRecentCommunication.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvRecentCommunication.RowTemplate.Height = 88
        Me.dgvRecentCommunication.Size = New System.Drawing.Size(556, 201)
        Me.dgvRecentCommunication.TabIndex = 14
        '
        'txtAction
        '
        Me.txtAction.Location = New System.Drawing.Point(614, 82)
        Me.txtAction.Multiline = True
        Me.txtAction.Name = "txtAction"
        Me.txtAction.Size = New System.Drawing.Size(183, 61)
        Me.txtAction.TabIndex = 13
        '
        'txtConcern
        '
        Me.txtConcern.Location = New System.Drawing.Point(614, 20)
        Me.txtConcern.Multiline = True
        Me.txtConcern.Name = "txtConcern"
        Me.txtConcern.Size = New System.Drawing.Size(183, 61)
        Me.txtConcern.TabIndex = 12
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTimePicker1.Location = New System.Drawing.Point(614, 169)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(183, 20)
        Me.DateTimePicker1.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(561, 173)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Due date"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(699, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "End date"
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(699, 41)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(102, 20)
        Me.dtpEndDate.TabIndex = 11
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(581, 121)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(103, 23)
        Me.btnPrint.TabIndex = 13
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnSavePicture
        '
        Me.btnSavePicture.Location = New System.Drawing.Point(690, 121)
        Me.btnSavePicture.Name = "btnSavePicture"
        Me.btnSavePicture.Size = New System.Drawing.Size(104, 23)
        Me.btnSavePicture.TabIndex = 14
        Me.btnSavePicture.Text = "Save"
        Me.btnSavePicture.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(580, 177)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(213, 23)
        Me.btnRemove.TabIndex = 17
        Me.btnRemove.Text = "Close indicator"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'ComboBox4
        '
        Me.ComboBox4.FormattingEnabled = True
        Me.ComboBox4.Items.AddRange(New Object() {"Daily", "Weekly", "Monthly"})
        Me.ComboBox4.Location = New System.Drawing.Point(570, 282)
        Me.ComboBox4.Name = "ComboBox4"
        Me.ComboBox4.Size = New System.Drawing.Size(104, 21)
        Me.ComboBox4.TabIndex = 18
        Me.ComboBox4.Visible = False
        '
        'lblOrganization
        '
        Me.lblOrganization.AutoSize = True
        Me.lblOrganization.Location = New System.Drawing.Point(577, 4)
        Me.lblOrganization.Name = "lblOrganization"
        Me.lblOrganization.Size = New System.Drawing.Size(39, 13)
        Me.lblOrganization.TabIndex = 19
        Me.lblOrganization.Text = "Label7"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(568, 267)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(114, 13)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Time aggregation level"
        Me.Label7.Visible = False
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(676, 280)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(104, 23)
        Me.btnUpdate.TabIndex = 21
        Me.btnUpdate.Text = "Update"
        Me.btnUpdate.UseVisualStyleBackColor = True
        Me.btnUpdate.Visible = False
        '
        'sfdPicture
        '
        Me.sfdPicture.Filter = "PNG | *.png"
        '
        'cbAnsolut
        '
        Me.cbAnsolut.AutoSize = True
        Me.cbAnsolut.Location = New System.Drawing.Point(581, 206)
        Me.cbAnsolut.Name = "cbAnsolut"
        Me.cbAnsolut.Size = New System.Drawing.Size(92, 17)
        Me.cbAnsolut.TabIndex = 22
        Me.cbAnsolut.Text = "Absolut - Both"
        Me.cbAnsolut.UseVisualStyleBackColor = True
        '
        'cbRelativ
        '
        Me.cbRelativ.AutoSize = True
        Me.cbRelativ.Location = New System.Drawing.Point(580, 229)
        Me.cbRelativ.Name = "cbRelativ"
        Me.cbRelativ.Size = New System.Drawing.Size(59, 17)
        Me.cbRelativ.TabIndex = 23
        Me.cbRelativ.Text = "Relativ"
        Me.cbRelativ.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.Location = New System.Drawing.Point(698, 67)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(103, 23)
        Me.btnLoad.TabIndex = 24
        Me.btnLoad.Text = "Reload"
        Me.btnLoad.UseVisualStyleBackColor = True
        '
        'lbxValueItems
        '
        Me.lbxValueItems.FormattingEnabled = True
        Me.lbxValueItems.HorizontalScrollbar = True
        Me.lbxValueItems.Location = New System.Drawing.Point(574, 323)
        Me.lbxValueItems.Name = "lbxValueItems"
        Me.lbxValueItems.Size = New System.Drawing.Size(236, 95)
        Me.lbxValueItems.TabIndex = 25
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(574, 306)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 13)
        Me.Label8.TabIndex = 26
        Me.Label8.Text = "Current Values"
        '
        'cbAbsoluteOne
        '
        Me.cbAbsoluteOne.AutoSize = True
        Me.cbAbsoluteOne.Location = New System.Drawing.Point(679, 206)
        Me.cbAbsoluteOne.Name = "cbAbsoluteOne"
        Me.cbAbsoluteOne.Size = New System.Drawing.Size(90, 17)
        Me.cbAbsoluteOne.TabIndex = 27
        Me.cbAbsoluteOne.Text = "Absolut - One"
        Me.cbAbsoluteOne.UseVisualStyleBackColor = True
        '
        'ucIndicatorTracker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cbAbsoluteOne)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lbxValueItems)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.cbRelativ)
        Me.Controls.Add(Me.cbAnsolut)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblOrganization)
        Me.Controls.Add(Me.ComboBox4)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnSavePicture)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.dtpEndDate)
        Me.Controls.Add(Me.gbConcernAction)
        Me.Controls.Add(Me.btnMaximum)
        Me.Controls.Add(Me.btnMinimum)
        Me.Controls.Add(Me.btnMyTracking)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtpStartDate)
        Me.Controls.Add(Me.chIndicatorTracker)
        Me.Name = "ucIndicatorTracker"
        Me.Size = New System.Drawing.Size(810, 653)
        CType(Me.chIndicatorTracker, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbConcernAction.ResumeLayout(False)
        Me.gbConcernAction.PerformLayout()
        CType(Me.dgvRecentCommunication, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chIndicatorTracker As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnMyTracking As System.Windows.Forms.Button
    Friend WithEvents btnMinimum As System.Windows.Forms.Button
    Friend WithEvents btnMaximum As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents gbConcernAction As System.Windows.Forms.GroupBox
    Friend WithEvents txtAction As System.Windows.Forms.TextBox
    Friend WithEvents txtConcern As System.Windows.Forms.TextBox
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbResponsible As System.Windows.Forms.ComboBox
    Friend WithEvents dgvRecentCommunication As System.Windows.Forms.DataGridView
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnSavePicture As System.Windows.Forms.Button
    Friend WithEvents btnAction As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents ComboBox4 As System.Windows.Forms.ComboBox
    Friend WithEvents cbxClosed As System.Windows.Forms.CheckBox
    Friend WithEvents lblOrganization As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents sfdPicture As System.Windows.Forms.SaveFileDialog
    Friend WithEvents cbAnsolut As System.Windows.Forms.CheckBox
    Friend WithEvents cbRelativ As System.Windows.Forms.CheckBox
    Friend WithEvents ttValues As System.Windows.Forms.ToolTip
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents lbxValueItems As System.Windows.Forms.ListBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbAbsoluteOne As System.Windows.Forms.CheckBox

End Class
