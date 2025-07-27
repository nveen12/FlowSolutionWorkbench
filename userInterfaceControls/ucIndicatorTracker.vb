Imports System.Windows.Forms.DataVisualization.Charting
'Imports System.Windows.Forms.DataVisualization.Charting.Utilities
Imports System.IO

Imports System.Data



Public Class ucIndicatorTracker

    Private _entity_name As String
    Private _header_id As String
    Private _line_id As String
    Private _new_key As Integer
    Private _newkeynotification As Integer
    Private _organizationID As Integer
    Private _dsInfo As DataSet
    Private db As New DB.ServerDB(My.Settings("FlowConnectionString"))
    Private _seriesName As String
    Private _organizationName As String
    Private _headline As String
    Private _nameNoOfValues As String
    Private lastLevelId As Integer
    Private nameFirstTarget As String
    Private bLoadFirstTime As Boolean = False
    Private levelSelected As Integer


    Private bOnlyAbsolut As Boolean = False                                 'Indicates wether or not we have relative values
    Private level1id As Integer                                             'Parent level of this indicator node
    Private level2id As Integer                                             'Oerating level for indicators

    Private fromDate As Date
    Private untilDate As Date
    Private _areaName As String

    ''' <summary>
    ''' Constructor to load the tool with an level ids and an area.
    ''' </summary>
    ''' <param name="organizationID"></param>
    ''' <param name="level1id"></param>
    ''' <param name="level2id"></param>
    ''' <param name="areaName"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal organizationID As Integer, ByVal level1id As Integer, ByVal level2id As Integer, ByVal areaName As String)
        'just loads everything for the second level
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        dtpEndDate.Value = Today
        dtpStartDate.Value = Today.AddDays(-90)
        Me.loadTwoLevels(organizationID, level1id, level2id, areaName)

    End Sub

    Private Sub loadTwoLevels(ByVal organizationID As Integer, ByVal level1id As Integer, ByVal level2id As Integer, ByVal areaName As String, Optional ByVal dateRange As String = " AND [creation_date] > dateadd(day,-31,getdate()) ")
        'Setting up the standard date ranges
       

        If levelSelected = 2 Then
            _dsInfo = db.SecureQueryParams("SELECT TOP 1000 [organization_id] " & _
                                                   ",[organization_name] " & _
                                                   ",[level1] " & _
                                                   ",[level2] " & _
                                                   ",SUM([value]) value " & _
                                                   ",AVG([no_of_values]) no_of_values" & _
                                                   ",[creation_date] " & _
                                                   ",[created_by] " & _
                                                   ",[level1ID] " & _
                                                   ",[level2ID] " & _
                                                   "      FROM [performance] " & _
                                                   "      WHERE organization_id = @1 " & _
                                                   "AND level1ID = @2 " & _
                                                   "AND level2ID = @3 AND private IS NULL " & dateRange & _
                                                   "GROUP BY [organization_id],[organization_name],[level1],[level2],[creation_date],[created_by],[level1ID],[level2ID] ORDER BY creation_date", organizationID, level1id, level2id)

        Else
            _dsInfo = db.SecureQueryParams("SELECT TOP 1000 [organization_id] " & _
                                                   ",[organization_name] " & _
                                                   ",[level1] " & _
                                                   ",[level2] " & _
                                                   ",SUM([value]) value " & _
                                                   ",AVG([no_of_values]) no_of_values" & _
                                                   ",[creation_date] " & _
                                                   ",[created_by] " & _
                                                   ",[level1ID] " & _
                                                   ",[level2ID] " & _
                                                   "      FROM [performance] " & _
                                                   "      WHERE organization_id = @1 " & _
                                                   "AND level2ID = @2 " & _
                                                   "AND level3ID = @3 AND private IS NULL " & dateRange & _
                                                   "GROUP BY [organization_id],[organization_name],[level1],[level2],[creation_date],[created_by],[level1ID],[level2ID] ORDER BY creation_date ", organizationID, level1id, level2id)


        End If
      
        '_nameNoOfValues = db.SecureQueryParams("SELECT nameNoOfValues FROM performanceNames WHERE indicatorID = @1", level2id).Tables(0).Rows(0).Item(0).ToString
        '_seriesName = db.SecureQueryParams("SELECT indicatorName FROM performanceNames WHERE indicatorID = @1", level2id).Tables(0).Rows(0).Item(0).ToString

        '_organizationID = organizationID
        '_organizationName = db.SecureQueryParams("SELECT organizationName FROM whOrganizationDefinition WHERE organizationId = @1", organizationID).Tables(0).Rows(0).Item(0).ToString
        _headline = _organizationName & " - " & areaName & " - " & _seriesName
        Me._areaName = areaName

        Me.level1id = level1id
        Me.level2id = level2id

        Me.formatChartArea()
    End Sub

    ''' <summary>
    ''' Constructor to load the tool with an level ids and an area.
    ''' </summary>
    ''' <param name="organizationID"></param>
    ''' <param name="level1id"></param>
    ''' <param name="level2id"></param>
    ''' <param name="areaName"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal organizationID As Integer, ByVal level1id As Integer, ByVal level2id As Integer, ByVal level3Name As String, ByVal areaName As String)
        'just loads everything for the second level
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        _dsInfo = db.SecureQueryParams("SELECT TOP 1000 [organization_id] " & _
                                         ",[organization_name] " & _
                                         ",[level1] " & _
                                         ",[level2] " & _
                                         ",SUM([value]) value " & _
                                         ",AVG([no_of_values]) no_of_values" & _
                                         ",[creation_date] " & _
                                         ",[created_by] " & _
                                         ",[level1ID] " & _
                                         ",[level2ID] " & _
                                         "      FROM [performance] " & _
                                         "      WHERE organization_id = @1 " & _
                                         "AND level1ID = @2 " & _
                                         "AND level2ID = @3 AND level3 = @4 GROUP BY " & _
                                         " [organization_id],[organization_name],[level1] " & _
                                         " ,[level2],[creation_date],[created_by],[level1ID],[level2ID] ", organizationID, level1id, level2id, level3Name)

        _nameNoOfValues = db.SecureQueryParams("SELECT nameNoOfValues FROM performanceNames WHERE indicatorID = @1", level2id).Tables(0).Rows(0).Item(0).ToString
        _seriesName = db.SecureQueryParams("SELECT indicatorName FROM performanceNames WHERE indicatorID = @1", level2id).Tables(0).Rows(0).Item(0).ToString

        _organizationID = organizationID
        _organizationName = db.SecureQueryParams("SELECT organizationName FROM whOrganizationDefinition WHERE organizationId = @1", organizationID).Tables(0).Rows(0).Item(0).ToString
        _headline = _organizationName & " - " & areaName & " - " & _seriesName
        Me.level1id = level1id
        Me.level2id = level2id

        Me.formatChartArea()

    End Sub


    ''' <summary>
    ''' Constructor for the tree definition object, which is instantiated with a given data set.
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="headerName"></param>
    ''' <param name="organizationName"></param>
    ''' <param name="entityName"></param>
    ''' <param name="headerID"></param>
    ''' <param name="lineID"></param>
    ''' <param name="seriesName"></param>
    ''' <param name="areaName"></param>
    ''' <param name="noOfValuesName"></param>
    ''' <param name="organizationId"></param>
    ''' <param name="lastLevelId"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal ds As DataSet, ByVal headerName As String, ByVal organizationName As String, _
                   ByVal entityName As String, ByVal headerID As Integer, ByVal lineID As Integer, _
                   ByVal seriesName As String, ByVal areaName As String, ByVal noOfValuesName As String, _
                   ByVal organizationId As Integer, ByVal lastLevelId As Integer, Optional ByVal levelSelected As Integer = 2)

        _entity_name = entityName
        _header_id = headerID
        Me.level1id = headerID
        Me.level2id = lineID
        Me.levelSelected = levelSelected
        Me.levelSelected = levelSelected

        _line_id = lineID
        _dsInfo = ds
        _seriesName = seriesName
        _organizationName = organizationName

        _headline = organizationName & " - " & areaName & " - " & seriesName
        _nameNoOfValues = noOfValuesName
        Me.lastLevelId = lastLevelId

        Me._organizationID = organizationId

        '***This tool needs to know the sql query ... in order to requery the data

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.formatChartArea()


        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub formatChartArea()
        Me.chIndicatorTracker.Legends("Legend1").Docking = Docking.Bottom
        chIndicatorTracker.Legends("Legend1").Alignment = StringAlignment.Center
    End Sub


    'Standard constructor
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Me.Visible = False
    End Sub



    Private Sub calculatePercentage(ByRef table As DataTable)
        Try
            Me.chIndicatorTracker.ResetAutoValues()
            Me.chIndicatorTracker.Series.Clear()
       
            Dim dc As New DataColumn("Percentage", System.Type.GetType("System.Double"))
            table.Columns.Add(dc)

            'Test if there is a no_of_values
            Dim lngNumberHits As Integer = 0

            For i As Integer = 0 To table.Rows.Count - 1
                Try
                    table.Rows(i).Item("Percentage") = Double.Parse(table.Rows(i).Item("value") / table.Rows(i).Item("no_of_values"))
                Catch ex As Exception
                    lngNumberHits += 1
                End Try
            Next

            'if we do not have number of values, we want to disable the relative ones
            If lngNumberHits = table.Rows.Count Then
                cbRelativ.Enabled = False
                Me.bOnlyAbsolut = True
                'cbAnsolut.Checked = True
            End If

            
        Catch ex As Exception

        End Try
    End Sub
    Private Sub createPercentageSeries(ByRef table As DataTable)
        Try
            Me.chIndicatorTracker.Titles.Clear()
            Me.chIndicatorTracker.Titles.Add(_headline)

            'Retrieve the maximim of the data table
            Dim maxValue As Double
            Dim dv As DataView = table.DefaultView
            dv.Sort = "Percentage DESC"
            maxValue = dv(0).Item("Percentage")

            'Add the new series to the chart
            Dim dr2 As DataTableReader
            'add the no_of_values we are measuring against
            dr2 = table.CreateDataReader
            Me.chIndicatorTracker.Series.Add("Percentage")
            Me.chIndicatorTracker.Series("Percentage").ChartType = SeriesChartType.Point
            Me.chIndicatorTracker.Series("Percentage").Points.DataBindXY(dr2, "creation_date", dr2, "percentage")
            Me.chIndicatorTracker.Series("Percentage").YAxisType = AxisType.Secondary
            Me.chIndicatorTracker.Series("Percentage").LabelFormat = "P0"

            'detect how small the value really is
            If maxValue < 0.1 Then
                Me.chIndicatorTracker.ChartAreas(0).AxisY2.LabelStyle.Format = "P2"
            ElseIf maxValue < 0.01 Then
                Me.chIndicatorTracker.ChartAreas(0).AxisY2.LabelStyle.Format = "P4"
            Else
                Me.chIndicatorTracker.ChartAreas(0).AxisY2.LabelStyle.Format = "P0"
            End If

            Me.chIndicatorTracker.ChartAreas("ChartArea1").AxisY2.Maximum = maxValue
            Me.chIndicatorTracker.ChartAreas("ChartArea1").AxisY2.Minimum = 0

            'adds the moving average weighted
            Me.chIndicatorTracker.DataManipulator.FinancialFormula(FinancialFormula.WeightedMovingAverage, "10", "Percentage", "Weighted")
            Me.chIndicatorTracker.Series("Weighted").ChartType = SeriesChartType.Line
            Me.chIndicatorTracker.Series("Weighted").YAxisType = AxisType.Secondary
            Me.chIndicatorTracker.Series("Weighted").LabelFormat = "P0"

            'Me.chIndicatorTracker.Series.MapAreaAttributes = "onmouseover=showTooltip('#VALY')"

        Catch ex As Exception

        End Try
    End Sub

    Private Sub createTargetSeries(ByRef table As DataTable)
        Try

       
            Dim dr2 As DataTableReader
            'add the series
            dr2 = _dsInfo.Tables(0).CreateDataReader
            Me.chIndicatorTracker.Series.Add(Me.nameFirstTarget)
            Me.chIndicatorTracker.Series(Me.nameFirstTarget).ChartType = SeriesChartType.Line
            Me.chIndicatorTracker.Series(Me.nameFirstTarget).BorderWidth = 3
            Me.chIndicatorTracker.Series(Me.nameFirstTarget).ShadowOffset = 2

            Me.chIndicatorTracker.Series(Me.nameFirstTarget).Points.DataBindXY(dr2, "creation_date", dr2, Me.nameFirstTarget)
            Me.chIndicatorTracker.Series(Me.nameFirstTarget).YAxisType = AxisType.Secondary
            Me.chIndicatorTracker.Series(Me.nameFirstTarget).LabelFormat = "P0"

            ' Me.chIndicatorTracker.Series(Me.nameFirstTarget).ToolTip = ttValues

        Catch ex As Exception

        End Try
    End Sub
    Private Sub loadTargets()
        Try

            Dim ds As New DataSet
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

            ds = db.SecureQueryParams(" SELECT [ID] " & _
                                      " ,[performanceID] " & _
                                      " ,[organizationID] " & _
                                      " ,[value] " & _
                                      " ,[validFrom] " & _
                                      " ,[validUntil] " & _
                                      " ,[name] " & _
                                      " ,[createdBY] " & _
                                      " ,[creationDate] " & _
                                  " FROM [performanceTargets] WHERE performanceID = @1 AND organizationID = @2", Me.lastLevelId, Me._organizationID)

            Dim dc As New DataColumn(ds.Tables(0).Rows(0).Item("name"), System.Type.GetType("System.Double"))
            _dsInfo.Tables(0).Columns.Add(dc)
            Me.nameFirstTarget = ds.Tables(0).Rows(0).Item("name")

            For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                For j As Integer = 0 To _dsInfo.Tables(0).Rows.Count - 1
                    If _dsInfo.Tables(0).Rows(j).Item("creation_date") > ds.Tables(0).Rows(i).Item("validFrom") AndAlso _dsInfo.Tables(0).Rows(j).Item("creation_date") < ds.Tables(0).Rows(i).Item("validUntil") Then
                        _dsInfo.Tables(0).Rows(j).Item(ds.Tables(0).Rows(0).Item("name")) = ds.Tables(0).Rows(i).Item("value")
                    End If
                Next
            Next

            
        Catch ex As Exception

        End Try
    End Sub
    Private Sub btnAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAction.Click

        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim concern As String = txtConcern.Text
        Dim action As String = txtAction.Text

        Me.getNewKey()
        Me.getNewKeyNotification()

        'Insert the comment 
        db.SecureNonQueryParams("INSERT INTO comments ([entity_id], [header_id], [line_id], description, description2, [creation_date], commentId, [created_by]) " & _
                               " VALUES (@1, @2, @3, @4,@5, @6, @7, @8)", _entity_name, _header_id, _line_id, concern, action, Format(Now, "yyyyMMdd HH:mm"), _new_key, Environment.UserName)

        '    'Inform the warehouse user of this change
        db.SecureNonQueryParams("INSERT INTO warehouseUserNotifications (notificationId, createdBy, notificationEntity, creationDate, headerID, lineID, reviewed, notificationTo, commentId, organizationID, closed) " & _
                                   " VALUES (@1, @2, @3, @4, @5, @6, 0, @7, @8, @9, 0)", _newkeynotification, Environment.UserName, _entity_name, Format(Now, "yyyyMMdd HH:mm"), _header_id, _line_id, cbResponsible.Text, _new_key, Me._organizationID)
        loadConcernActionTab()

    End Sub

    Private Sub getNewKey()


        Dim ds As New DataSet

        ds = db.Query("SELECT MAX(commentId) + 1 as NewKey FROM comments")
        _new_key = ds.Tables(0).Rows(0).Item(0)

    End Sub

    Private Sub getNewKeyNotification()

        Dim ds As New DataSet

        ds = db.Query("SELECT ISNULL(MAX(notificationId),0) + 1 as NewKey FROM warehouseUserNotifications")
        _newKeyNotification = ds.Tables(0).Rows(0).Item(0)

    End Sub

    Private Sub ucIndicatorTracker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.bLoadFirstTime = True
        Me.completeLoadActitvity()
        Me.dtpStartDate.Value = Now.AddDays(-90)
        Me.bLoadFirstTime = False

        'Constraint the date time picker
        dtpEndDate.MaxDate = Today
    End Sub

    Private Sub completeLoadActitvity()

        'Load the the from value of the calendat pick


        loadConcernActionTab()
        lblOrganization.Text = _organizationName
        Me.loadTargets()
        Me.calculatePercentage(_dsInfo.Tables(0))

        'If relative possible take this series otherwise get the absolut ones
        If Me.bOnlyAbsolut Then
            cbAnsolut.Checked = True
            Me.loadAbsolutValues()
        Else
            Me.createPercentageSeries(_dsInfo.Tables(0))
            Me.createTargetSeries(_dsInfo.Tables(0))
            cbRelativ.Checked = True

        End If
        Me.loadCurrentValue()

    End Sub

    Private Sub loadAbsolutValues(Optional ByVal onlyLeadingWithoutCompare As Boolean = False)
        'Implement load of indicator tracking
        'Me.chIndicatorTracker.ResetAutoValues()
        'Me.chIndicatorTracker.Series.Clear()
        Try



            Me.chIndicatorTracker.Series.Add(_seriesName)
            Me.chIndicatorTracker.Titles.Clear()
            Me.chIndicatorTracker.Titles.Add(_headline)

            Dim dr As DataTableReader
            dr = _dsInfo.Tables(0).CreateDataReader

            Me.chIndicatorTracker.Series(_seriesName).ChartType = SeriesChartType.Column
            Me.chIndicatorTracker.Series(_seriesName).Points.DataBindXY(dr, "creation_date", dr, "value")


            If Me.bOnlyAbsolut Then
                'Add the weighted average against the value
                Me.chIndicatorTracker.DataManipulator.FinancialFormula(FinancialFormula.WeightedMovingAverage, "10", _seriesName, "Weighted")
                Me.chIndicatorTracker.Series("Weighted").ChartType = SeriesChartType.Line
                Me.chIndicatorTracker.Series("Weighted").YAxisType = AxisType.Secondary
                Me.chIndicatorTracker.Series("Weighted").LabelFormat = "P0"
            Else
                If onlyLeadingWithoutCompare Then
                    'we want nothing to appear
                Else
                    Dim dr2 As DataTableReader
                    'add the no_of_values we are measuring against
                    dr2 = _dsInfo.Tables(0).CreateDataReader
                    Me.chIndicatorTracker.Series.Add(_nameNoOfValues)
                    Me.chIndicatorTracker.Series(_nameNoOfValues).ChartType = SeriesChartType.Column
                    Me.chIndicatorTracker.Series(_nameNoOfValues).Points.DataBindXY(dr2, "creation_date", dr2, "no_of_values")
                End If
                
            End If
        Catch ex As Exception

        End Try

    End Sub



    Private _dsConcernAction As New DataSet

    Private Sub loadConcernActionTab()
        Try

       
            ' Dim dsConcernAction As DataSet
            _dsConcernAction = db.SecureQueryParams("SELECT a.creation_date ,a.created_by ,b.notificationTo  ,a.description , a.description2   , b.closed , b.closedDate  " & _
                                "FROM comments a INNER JOIN warehouseUserNotifications b ON a.commentID = b.commentId WHERE a.entity_id = @1 " & _
                                "AND a.header_id = @2 AND a.line_id = @3 AND b.closed = @4", _entity_name, _header_id, _line_id, cbxClosed.Checked)

            '*** crashes when no values are handed over

            dgvRecentCommunication.DataSource = db.SecureQueryParams("SELECT a.creation_date [Creation Date],a.created_by [Created By],b.notificationTo [Notification To] ,a.description as [Concern], a.description2 as [Action]  , b.closed [Closed], b.closedDate [Closed Date] " & _
                                "FROM comments a INNER JOIN warehouseUserNotifications b ON a.commentID = b.commentId WHERE a.entity_id = @1 " & _
                                "AND a.header_id = @2 AND a.line_id = @3 AND b.closed = @4", _entity_name, _header_id, _line_id, cbxClosed.Checked).Tables(0)

            ' dgvRecentCommunication.DataSource = _dsConcernAction.Tables(0)
        Catch ex As Exception
            Me.gbConcernAction.Visible = False

        End Try

    End Sub

    ''' <summary>
    ''' Creates a printout with concern action tab.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim dsPicture As DataSet
        Try
            Dim strHTMLFile As String = Application.StartupPath & "\myFile.html"
            Dim strPictureSave = Application.StartupPath & "\update.png"
            Dim strLogoLocation As String = Application.StartupPath & "\ProjectManagerWorkbench\Reporting\Flowserve.jpg"


            'Save the picture, create the html content and load it to internet explorer
            Me.chIndicatorTracker.SaveImage(strPictureSave, ChartImageFormat.Png)
            Dim strContent As String = Me.createHTMLString(strPictureSave, _dsConcernAction.Tables(0), "", strLogoLocation)

            'Load the image back into a variable
            Dim fs As New System.IO.StreamWriter(strHTMLFile)
            fs.Write(strContent)
            fs.Close()

            System.Diagnostics.Process.Start(strHTMLFile)

        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnSavePicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavePicture.Click
        Me.sfdPicture.ShowDialog()
    End Sub
    Private Sub sfdPicture_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles sfdPicture.FileOk
        Dim heightBefore As Integer = Me.chIndicatorTracker.Height
        Dim widthBefore As Integer = Me.chIndicatorTracker.Width

        Dim chtFont As Font = New Font("Arial", 84)
        Dim chAxis As Font = New Font("Arial", 44)

        'Resize to high resolution
        Me.chIndicatorTracker.Width = 4200
        Me.chIndicatorTracker.Height = 3000
        'chIndicatorTracker.ti
        'Change titles etc
        'Me.chIndicatorTracker.Titles.
        chIndicatorTracker.Legends("Legend1").Font = chtFont
        Me.chIndicatorTracker.Titles(0).Font = chtFont
        For Each ser As Series In chIndicatorTracker.Series
            ser.Font = chtFont
            '  ser.
            ser.BorderWidth = 15
            ser.MarkerSize = 30
        Next
        'Me.chIndicatorTracker.
        'Me.chIndicatorTracker.Series(0).Font = chtFont
        'Me.chIndicatorTracker.Series(0).MarkerSize = 25
        Me.chIndicatorTracker.ChartAreas(0).AxisX.LabelStyle.Font = chAxis
        Me.chIndicatorTracker.ChartAreas(0).AxisY.LabelStyle.Font = chAxis
        Me.chIndicatorTracker.ChartAreas(0).AxisY2.LabelStyle.Font = chAxis

        Dim bmp As Bitmap = New Bitmap(4200, 3000)
        Me.chIndicatorTracker.DrawToBitmap(bmp, New Rectangle(0, 0, 4200, 3000))
        bmp.Save(sfdPicture.FileName)
        'Me.chIndicatorTracker.SaveImage(sfdPicture.FileName, ChartImageFormat.Jpeg)

        'Set it back to the setting from before!

    End Sub

    Private Function createHTMLString(ByVal pictureLocation As String, ByVal concernAction As DataTable, ByVal indicatorDefinition As String, ByVal logoLoaction As String)

        Dim strContent As String = ""
        strContent += " <!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.1//EN' 'http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd'> " & _
                 " <html xmlns='http://www.w3.org/1999/xhtml'> " & _
                 "   <head>" & _
                 "       <title>Indicator Tracking</title>" & _
                 "       <style type='text/css'>" & _
                 "       .style1()" & _
                 "       {" & _
                 "           width: 275px;" & _
                 "       }" & _
                 "      " & _
                 "       {" & _
                 "           width: 429px;" & _
                 "           height: 79px;" & _
                 "       }" & _
                 "       .style2()" & _
                 "       {" & _
                 "           width: 359px;" & _
                 "       }" & _
                 "       .style3()" & _
                 "       {" & _
                 "           width: 328px;" & _
                 "       }" & _
                 "       .style4()" & _
                 "       {" & _
                 "           width: 449px;" & _
                 "       }" & _
                 "       .style5()" & _
                 "       {" & _
                 "           width: 101px;" & _
                 "       }" & _
                 "   </style>" & _
                 "   </head>" & _
                 "   <body>" & _
                 "       <hr/>" & _
                 "       <p>" & _
                 "           <table style='width: 101%;'>" & _
                 "               <tr>" & _
                 "                   <td class='style1'>"

        strContent += " <img alt='' src='" & logoLoaction & "' style='height: 89px; width: 263px'/></td>" & _
                 "                   <td class='style2'>" & _
                 "                   &nbsp;</td>" & _
                 "                   <td>" & _
                 "                   &nbsp;</td>" & _
                 "               </tr>" & _
                 "           </table>" & _
                 "       </p>" & _
                 "       <p>"

        strContent += "           <img alt='' src='" & pictureLocation & "' style='height: 400px; width: 660px'/></p>" & _
        "       <p>" & _
        "           <table style='border-style: dotted; border-width: thin; width: 100%; table-layout: fixed;'>" & _
        "               <tr bgcolor='LightSteelBlue' style='border-width: thin; border-style: solid;'>" & _
        "                   <td class='style3'>" & _
        "                   Concern</td>" & _
        "                   <td class='style4'>" & _
         "                  Action</td>" & _
        "                   <td class='style5'>" & _
        "                   Creation Date</td>" & _
        "                   <td>" & _
        "                   Created By</td>" & _
        "               </tr>"

        For i As Integer = 0 To concernAction.Rows.Count - 1
            strContent += " <tr>" & _
        "                   <td class='style3'>" & _
        "                   " & concernAction.Rows(i).Item("description") & "</td> " & _
        "                   <td class='style4'>" & _
        "                   " & concernAction.Rows(i).Item("description2") & "</td>" & _
        "                   <td class='style5'>" & _
        "                   " & concernAction.Rows(i).Item("creation_Date") & "</td>" & _
        "                   <td>" & _
        "                   " & concernAction.Rows(i).Item("created_By") & "</td>" & _
        "               </tr>"

        Next

        strContent += "           </table> " & _
        "       </p> " & _
        "       <p> " & _
        "       &nbsp;</p> " & _
        "       <hr/> " & _
        "   </body> " & _
       " </html> "

        Return strContent
    End Function

    Private Sub cbAnsolut_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAnsolut.CheckedChanged

        Try
            If cbAnsolut.Checked Then
                Me.loadAbsolutValues()
            Else
                Me.chIndicatorTracker.Series.Remove(Me.chIndicatorTracker.Series(_seriesName))
                Me.chIndicatorTracker.Series.Remove(Me.chIndicatorTracker.Series(Me._nameNoOfValues))
            End If
            Me.loadCurrentValue()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cbRelativ_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbRelativ.CheckedChanged

        If Not bLoadFirstTime Then
            If cbRelativ.Checked Then
                Me.createPercentageSeries(_dsInfo.Tables(0))
                Me.createTargetSeries(_dsInfo.Tables(0))

                'Me.loadTargets()
            Else
                Try
                    'Target may not exist therefore use a try catch block
                    Try
                        Me.chIndicatorTracker.Series.Remove(Me.chIndicatorTracker.Series(Me.nameFirstTarget))
                    Catch ex As Exception

                    End Try

                    Me.chIndicatorTracker.Series.Remove(Me.chIndicatorTracker.Series("Percentage"))
                    Me.chIndicatorTracker.Series.Remove(Me.chIndicatorTracker.Series("Weighted"))
                Catch ex As Exception

                End Try

            End If
            Me.loadCurrentValue()

        End If
       
    End Sub

    Private Sub chIndicatorTracker_GetToolTipText(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs) Handles chIndicatorTracker.GetToolTipText
        Try
            Select Case e.HitTestResult.ChartElementType

                Case ChartElementType.Axis
                    e.Text = e.HitTestResult.Axis.Name

                Case ChartElementType.DataPoint
                    Dim i As Integer = e.HitTestResult.PointIndex
                    Dim h As DataPoint = e.HitTestResult.Object

                    e.Text = "Y Value:" + h.YValues(0).ToString
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Try
            'Add the creation of the date range
            Dim strDateRange As String = " AND [creation_date] > '" & Format(dtpStartDate.Value, "yyyyMMdd") & "' AND [creation_date] < '" & Format(dtpEndDate.Value, "yyyyMMdd") & "' "

            Me.loadTwoLevels(Me._organizationID, Me.level1id, Me.level2id, Me._areaName, strDateRange)
            Me.completeLoadActitvity()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub loadCurrentValue()
        Me.lbxValueItems.Items.Clear()

        For Each a As DataVisualization.Charting.Series In chIndicatorTracker.Series
            Dim strDescription As String = a.Name & " - X:" & Date.FromOADate(a.Points(a.Points.Count - 1).XValue) & " - >:" & a.Points(a.Points.Count - 1).YValues(0).ToString
            lbxValueItems.Items.Add(strDescription)
        Next

    End Sub

    Private Sub dtpStartDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpStartDate.ValueChanged

        If dtpStartDate.Value > dtpEndDate.Value Then
            MessageBox.Show("Please select a start date less than the end date.")
            dtpStartDate.Value = DateAdd(DateInterval.Day, -1, dtpEndDate.Value)
        End If

    End Sub

    Private Sub cbAbsolute_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles cbAbsoluteOne.CheckedChanged
        Try
            If cbAbsoluteOne.Checked Then
                Me.loadAbsolutValues(True)
            Else
                Me.chIndicatorTracker.Series.Remove(Me.chIndicatorTracker.Series(_seriesName))
                Me.chIndicatorTracker.Series.Remove(Me.chIndicatorTracker.Series(Me._nameNoOfValues))
            End If
            Me.loadCurrentValue()

        Catch ex As Exception

        End Try
    End Sub
End Class
