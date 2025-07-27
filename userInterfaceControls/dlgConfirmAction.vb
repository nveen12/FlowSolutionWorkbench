Imports System.Windows.Forms

Public Class dlgConfirmAction

    Public Sub New(ByVal action As String, ByVal message As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Text = action
        txtMessage.Text = message

        ' Add any initialization after the InitializeComponent() call.

    End Sub


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgConfirmAction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
