Public Class frmLoading

    Private m_strStatusState As String
    Private m_progress As Double

    Private Sub frmLoading_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Public Property statusBar As String
        Get
            Return m_strStatusState
        End Get
        Set(value As String)

            ssMain.Items.Clear()
            m_strStatusState = value
            ssMain.Text = value
            ssMain.Update()
            Dim myLabel As New ToolStripStatusLabel
            myLabel.LinkBehavior = LinkBehavior.HoverUnderline
            myLabel.BorderStyle = Border3DStyle.RaisedOuter
            myLabel.Text = value
            ssMain.Items.Add(myLabel)

        End Set
    End Property

    Public Property statusProgress As Double

        Get
            Return m_progress
        End Get
        Set(value As Double)
            m_progress = value
            If value < 100 Then
                pbMain.Value = value
                pbMain.Update()
            End If
        End Set

    End Property

End Class