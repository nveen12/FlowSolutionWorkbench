Imports System.Windows.Forms
Imports System.Windows.Forms.CheckedListBox

Public Class dlgDisplayDrawings
    Private listOfDrawings() As String
    Private listOfRevision() As String
    Private mainServer As String

    Public Sub New(ByVal listOfDrawings() As String, ByVal listOfRevision() As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Dim i As Integer = 0
        'Add the drawings to the check box
        For Each h As String In listOfDrawings
            If Not h = Nothing Then
                If Not clbDocuments.Items.Contains(h.ToString & " (" + listOfRevision(i) + ")") Then
                    clbDocuments.Items.Add(h.ToString & " (" + listOfRevision(i) + ")", CheckState.Checked)

                End If
                i += 1
            Else
                i += 1
            End If
        Next

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'Open the imaging file
        'Me.openImaging()
        'Me.openNewImaging()
        'Me.startNewImageViewer()
    End Sub

    ''' <summary>
    ''' Opens the "old" imaging tool
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub openImaging()
        Try
            Dim strImages(clbDocuments.CheckedItems.Count - 1) As String
            Dim strSheets(clbDocuments.CheckedItems.Count - 1) As String
            Dim strServers(clbServers.CheckedItems.Count - 1) As String
            Dim strRevisions(clbDocuments.CheckedItems.Count - 1) As String

            For i As Integer = 0 To clbDocuments.CheckedItems.Count - 1
                strImages(i) = clbDocuments.CheckedItems(i).ToString.Substring(0, clbDocuments.CheckedItems(i).ToString.IndexOf("(")).Trim
                strSheets(i) = "1"
                strRevisions(i) = clbDocuments.CheckedItems(i).ToString.Substring(clbDocuments.CheckedItems(i).ToString.IndexOf("(") + 1, clbDocuments.CheckedItems(i).ToString.IndexOf(")") - clbDocuments.CheckedItems(i).ToString.IndexOf("(") - 1).Trim
            Next
            For j As Integer = 0 To clbServers.CheckedItems.Count - 1
                strServers(j) = clbServers.CheckedItems(j).ToString
            Next

            Me.startImageViewer(strImages, strSheets, strServers, strRevisions)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()

        Catch ex As Exception
            'Me.openNewImaging()
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Opens the new Imaging tool.
    ''' </summary>
    ''' <remarks></remarks>
    'Private Sub openNewImaging()
    '    Try
    '        Dim sDocs(clbDocuments.CheckedItems.Count - 1) As String 'dimension to the number of documents
    '        Dim sSht(clbDocuments.CheckedItems.Count - 1) As String  'these arrays need to match in size
    '        Dim sRev(clbDocuments.CheckedItems.Count - 1) As String  'these arrays need to match in size
    '        Dim sServ(clbServers.CheckedItems.Count - 1) As String 'frequently only one server to search



    '        For i As Integer = 0 To clbDocuments.CheckedItems.Count - 1
    '            sDocs(i) = clbDocuments.CheckedItems(i).ToString.Substring(0, clbDocuments.CheckedItems(i).ToString.IndexOf("(")).Trim
    '            sDocs(i) = Me.applyrulesToImageNames(sDocs(i))
    '            sSht(i) = "1"
    '            sRev(i) = clbDocuments.CheckedItems(i).ToString.Substring(clbDocuments.CheckedItems(i).ToString.IndexOf("(") + 1, clbDocuments.CheckedItems(i).ToString.IndexOf(")") - clbDocuments.CheckedItems(i).ToString.IndexOf("(") - 1).Trim
    '        Next
    '        For j As Integer = 0 To clbServers.CheckedItems.Count - 1
    '            sServ(j) = clbServers.CheckedItems(j).ToString
    '        Next

    '        startNewImageViewer(sDocs, sSht, sServ, sRev)
    '        Me.DialogResult = System.Windows.Forms.DialogResult.OK
    '        Me.Close()


    '    Catch ex As Exception
    '        'Me.openImaging()
    '        MessageBox.Show(ex.Message)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Function which applies different rule to image formats.
    ''' </summary>
    ''' <param name="imageName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function applyrulesToImageNames(ByVal imageName As String)
        imageName = imageName.Replace("A4", "")

        Return imageName
    End Function

    'Public Sub startNewImageViewer(ByVal DocumentIDs() As String, ByVal sheet() As String, ByVal Servers() As String, ByVal revision() As String)
    '    Try
    '        Dim objImaging As New FSGImageViewer.clsFSGImageViewerCOM

    '        For iServer As Integer = 0 To UBound(Servers)
    '            Dim sDrawings As String = ""
    '            Dim sSheets As String = ""
    '            Dim sRevisions As String = ""

    '            For x As Integer = 0 To UBound(DocumentIDs)
    '                If DocumentIDs(x).Length > 0 Then
    '                    If sDrawings.Length > 0 Then
    '                        'these three lists should contain the same number of entries
    '                        sDrawings &= ","
    '                        sSheets &= ","
    '                        sRevisions &= ","
    '                    End If
    '                    sDrawings &= DocumentIDs(x)
    '                    sSheets &= sheet(x)
    '                    sRevisions &= revision(x)
    '                End If
    '            Next x
    '            If Len(Trim(sDrawings)) > 0 Then objImaging.ShowImageList(Servers(iServer), sDrawings, sSheets, sRevisions)
    '        Next iServer

    '    Catch ex As Exception
    '        MessageBox.Show("Try clicking on the list items itself.: " & ex.Message)
    '    End Try
    'End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Sub startImageViewer(ByVal images() As String, ByVal sheet() As String, ByVal servers() As String, ByVal revision() As String)

        Try

            Dim imageViewer As Object = CreateObject("FSDImageViewer.clsImageDisplay")
            Dim numImages As Integer = 0

            'See if we have too many pictures assigned?
            If imageViewer.ViewerImagesAllowed < images.Count Then
                MessageBox.Show("Just " & imageViewer.ViewerImagesAllowed.ToString & " images allowed.")
                numImages = 20
            Else
                numImages = images.Count
            End If

            Dim errorMessage As String = ""

            For i As Integer = 0 To numImages - 1
                For Each server As String In servers
                    ' Replace string  with name of the remote computer.
                    Try
                        imageViewer.CurrentServer = server          'remote computer
                        imageViewer.ShowImage(images(i), sheet(i), revision(i))
                    Catch ex As Exception
                        errorMessage += "Could not look up drawing: " & images(i) & " Rev.: " & revision(i) & "  Displaying all possible instead!" & vbCr
                        Try
                            imageViewer.ShowImage(images(i), sheet(i), "")
                        Catch ex2 As Exception
                            errorMessage += vbTab & "Not possible to display this drawing " & images(i) & vbCr & vbCr
                        End Try

                    End Try
                Next
            Next

            If Not errorMessage = "" Then
                Dim dlg As New dlgMissingDrawings(errorMessage)
                dlg.ShowDialog()

            End If

        Catch ex As Exception
            'Me.openNewImaging()
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub dlgDisplayDrawings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        Dim ds As DataSet = db.SecureQueryParams("SELECT settingName FROM whUserSettings WHERE userName = @1 AND settingGroup = 'clbServers'", Environment.UserName)
        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

            'clbServers.Items(clbServers.FindString(ds.Tables(0).Rows(i).Item("settingName").ToString)).checked = True
            clbServers.Items.Remove(clbServers.Items(clbServers.FindString(ds.Tables(0).Rows(i).Item("settingName").ToString)))
            clbServers.Items.Add(ds.Tables(0).Rows(i).Item("settingName").ToString, True)

        Next

    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)

        db.SecureNonQueryParams("DELETE FROM whUserSettings WHERE settingGroup = 'clbServers' AND userName = @1", Environment.UserName)
        For i As Integer = 0 To clbServers.CheckedItems.Count - 1
            Dim newID As Integer = db.Query("SELECT ISNULL(max(settingID) + 1, 1) FROM whUserSettings ").Tables(0).Rows(0).Item(0)

            db.SecureInsertQueryParams("INSERT INTO [whUserSettings] " & _
           "([settingID] " & _
           ",[userName] " & _
           ",[settingName] " & _
           ",[settingGroup]) " & _
           " VALUES (@1,@2,@3,@4) ", newID, Environment.UserName, clbServers.CheckedItems(i).ToString, "clbServers")

        Next


    End Sub

    Private Sub clbDocuments_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbDocuments.SelectedIndexChanged
        Try

            Dim mainServer As String = ""
            Dim sServ(clbServers.CheckedItems.Count - 1) As String 'frequently only one server to search
            For j As Integer = 0 To clbServers.CheckedItems.Count - 1
                sServ(j) = clbServers.CheckedItems(j).ToString
                mainServer = clbServers.CheckedItems(j).ToString
            Next

            Dim sDocs As String 'dimension to the number of documents

            sDocs = clbDocuments.SelectedItem.ToString.Substring(0, clbDocuments.SelectedItem.ToString.IndexOf("(")).Trim
            'sDocs(i) = Me.applyrulesToImageNames(sDocs(i))

            Dim webAddress As String = "http://kalap10.flowserve.net/FSGImaging/GetFile.aspx?SearchProfile=PumpEng&drawing=" & sDocs & "&Sheet=1"
            Process.Start(webAddress)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

End Class
