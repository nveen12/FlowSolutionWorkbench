Public Class frmOneDashboard

    Private orgID As Integer

    ' create a printing component
    Private WithEvents pd As Printing.PrintDocument
    ' storage for form image
    Dim formImage As Bitmap
    ' Make a bitmap for the result.
    Dim bm_dest As New Bitmap( _
        600, _
        850)
    ' create API prototype
    Private Declare Function BitBlt Lib "gdi32.dll" Alias _
       "BitBlt" (ByVal hdcDest As IntPtr, _
       ByVal nXDest As Integer, ByVal nYDest As _
       Integer, ByVal nWidth As Integer, _
       ByVal nHeight As Integer, ByVal _
       hdcSrc As IntPtr, ByVal nXSrc As Integer, _
       ByVal nYSrc As Integer, _
       ByVal dwRop As System.Int32) As Long

 


    Public Sub New(ByVal organizationID As Integer)

        orgID = organizationID
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub frmOneDashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim uc1 As New ucOnTimePerformance(Me.orgID)
        Me.tcOverall.TabPages(0).Controls.Add(uc1)
        uc1.Dock = DockStyle.Fill

        Me.dtpStart.Value = Now.AddDays(-30)

    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        'PrintForm1.Form = Me

        'PrintForm1.Print(Me, PowerPacks.Printing.PrintForm.PrintOption.Scrollable)

        ' create an instance of the PrintDocument component
        pd = New Printing.PrintDocument

        Me.StartPosition = FormStartPosition.CenterScreen

        ' initiate the printdocument component
        GetFormImage()

        pd.Print()


    End Sub

    ' Callback from PrintDocument component to 
    ' do the actual printing
    Private Sub pd_PrintPage(ByVal sender As Object, _
       ByVal e As System.Drawing.Printing.PrintPageEventArgs) _
       Handles pd.PrintPage
        e.Graphics.DrawImage(bm_dest, 100, 100)


    End Sub

    Private Sub GetFormImage()
        Dim g As Graphics = Me.CreateGraphics()
        Dim s As Size = Me.Size
        formImage = New Bitmap(s.Width, s.Height, g)
        Dim mg As Graphics = Graphics.FromImage(formImage)
        Dim dc1 As IntPtr = g.GetHdc
        Dim dc2 As IntPtr = mg.GetHdc
        ' added code to compute and capture the form 
        ' title bar and borders 
        Dim widthDiff As Integer = _
           (Me.Width - Me.ClientRectangle.Width)
        Dim heightDiff As Integer = _
           (Me.Height - Me.ClientRectangle.Height)
        Dim borderSize As Integer = widthDiff \ 2
        Dim heightTitleBar As Integer = heightDiff - borderSize
        BitBlt(dc2, 0, 0, _
           Me.ClientRectangle.Width + widthDiff, _
           Me.ClientRectangle.Height + heightDiff, dc1, _
           0 - borderSize, 0 - heightTitleBar, 13369376)

        g.ReleaseHdc(dc1)
        mg.ReleaseHdc(dc2)

        formImage.RotateFlip(RotateFlipType.Rotate90FlipNone)
        ' Get the source bitmap.

        Dim bm_source As New Bitmap(formImage)

        

        ' Make a Graphics object for the result Bitmap.
        Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)

        ' Copy the source image into the destination bitmap.
        gr_dest.DrawImage(bm_source, 0, 0, _
            bm_dest.Width + 1, _
            bm_dest.Height + 1)




    End Sub


    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Try

        
            'Check if user really selected a row
            If dgvBacklog.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a few rows first.")
                Exit Sub
            End If

            If cbReasons.SelectedIndex = -1 Then
                MessageBox.Show("Please select a Reason first.")
                Exit Sub
            End If

            For i As Integer = 0 To dgvBacklog.SelectedRows.Count - 1

                If DBNull.Value.Equals(dgvBacklog.SelectedRows(i).Cells("Reason").Value) Then
                    db.SecureNonQueryParams("INSERT INTO BBBOTPReasons ([ORGANIZATION_ID] " & _
                                           ",[ORDER_NUMBER] " & _
                                           ",[PERIOD_YEAR] " & _
                                           ",[PERIOD_NUM] " & _
                                           ",[createdBy] " & _
                                           ",[creationDate] " & _
                                           ",[reason] " & _
                                           ",[description] " & _
                                           ",[notLate])  VALUES (@1,@2,@3,@4,@5,getDate(),@6,@7,1) ", _
                                        Me.orgID, dgvBacklog.SelectedRows(i).Cells("Order Number").Value _
                                        , dgvBacklog.SelectedRows(i).Cells("Period Year").Value _
                                        , dgvBacklog.SelectedRows(i).Cells("Period Num").Value _
                                        , Environment.UserName _
                                        , cbReasons.SelectedItem, txtDescription.Text)


                End If
            Next

            Me.loadData()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoad.Click

        Me.loadData()

    End Sub

    Private Sub loadData()
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As DataSet = db.SecureQueryParams("SELECT a.[ORDER_NUMBER] AS [Order Number]" & _
                                                  ",a.[PERIOD_NUM] AS [Period Num]" & _
                                                  ",a.[PERIOD_YEAR] AS [Period Year] " & _
                                                  ",a.[Day] " & _
                                                  ",a.[totalPrice] As [Total Price]" & _
                                                  ",a.[PROMISE_DATE] as [Promise Date] " & _
                                                  ",a.[Late] " & _
                                                  ",a.[completePrice] AS [Complete Price] " & _
                                                  ",a.[driverPrice] AS [Driver Price] " & _
                                                  ",a.[partsPrice] AS [Parts Price] " & _
                                                  ",a.[repairPrice] AS [Repair Price]" & _
                                                  ",a.[sparePrice] AS [Spare Price] " & _
                                                  " , (SELECT TOP 1 b.reason FROM BBBOTPReasons b WHERE a.ORDER_NUMBER = b.ORDER_NUMBER AND a.ORGANIZATION_ID = b.ORGANIZATION_ID AND a.PERIOD_YEAR = b.PERIOD_YEAR AND a.PERIOD_NUM = b.PERIOD_NUM) AS Reason" & _
                                                  " FROM BBBBillingsAggregatView a WHERE a.Day > @1 AND a.Day < @2 AND a.organization_id = @3", dtpStart.Value, _
                                                  dtpEnd.Value, Me.orgID)

        dgvBacklog.DataSource = ds.Tables(0)
    End Sub

    Private Sub dgvBacklog_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvBacklog.CellFormatting
        'every second row will become beige
        If e.RowIndex Mod 2 = 0 Then
            e.CellStyle.BackColor = Color.Beige
        End If
        If Not DBNull.Value.Equals(e.Value) Then
            'Round the extended price to a visual value
            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Total Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Complete Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Driver Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Parts Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Repair Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

            If dgvBacklog.Columns(e.ColumnIndex).Name.Equals("Spare Price") Then
                e.Value = FormatCurrency(e.Value, 2, TriState.True, TriState.True, TriState.True)
                e.CellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End If

        End If
        


    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Try


            'Check if user really selected a row
            If dgvBacklog.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a few rows first.")
                Exit Sub
            End If


            For i As Integer = 0 To dgvBacklog.SelectedRows.Count - 1

                If Not DBNull.Value.Equals(dgvBacklog.SelectedRows(i).Cells("Reason").Value) Then
                    db.SecureNonQueryParams("DELETE FROM BBBOTPReasons  WHERE [ORGANIZATION_ID] = @1 " & _
                                           " AND [ORDER_NUMBER] = @2 " & _
                                           " AND [PERIOD_YEAR] = @3 " & _
                                           " AND [PERIOD_NUM] = @4", _
                                        Me.orgID, dgvBacklog.SelectedRows(i).Cells("Order Number").Value _
                                        , dgvBacklog.SelectedRows(i).Cells("Period Year").Value _
                                        , dgvBacklog.SelectedRows(i).Cells("Period Num").Value)


                End If
            Next

            Me.loadData()

        Catch ex As Exception

        End Try
    End Sub
End Class