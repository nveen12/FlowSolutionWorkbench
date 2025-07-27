Public Class frmOneIndicator

    Public Sub New(ByVal organizationID As Integer, ByRef uc As ucIndicatorTracker)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.Controls.Add(uc)

        ' Add any initialization after the InitializeComponent() call.

    End Sub

End Class