Imports System
Imports System.Diagnostics
Imports System.ComponentModel

Public Class clsProgramInterfaces
    Public Sub startImageViewer(ByVal image As String(), ByVal sheet As String())
        'Dim startInfo As New ProcessStartInfo
        'startInfo.FileName = "FLSImageStart.exe"
        'startInfo.Arguments = "952_000_004,1,"""

        'Process.Start(startInfo)

        Dim imageViewer As Object
        ' Replace string "\\MyServer" with name of the remote computer.
        imageViewer = CreateObject("FSDImageViewer.clsImageDisplay")
        MessageBox.Show(imageViewer.ViewerImagesAllowed)
        imageViewer.CurrentServer = "FPD Engineering"

        imageViewer.ShowImage("952_000_004", "1", " ")

    End Sub
End Class
