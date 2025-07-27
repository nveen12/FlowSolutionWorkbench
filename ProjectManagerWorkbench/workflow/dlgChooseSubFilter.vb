Imports System.Windows.Forms

Public Class dlgChooseSubFilter

    Public Sub New(ByVal dt As DataTable, ByVal displayMember As String, ByVal valueMember As String, ByVal displayheader As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.lblDisplay.Text = displayheader

        ' Add any initialization after the InitializeComponent() call.
        Me.lbxChoosedItems.DataSource = dt
        Me.lbxChoosedItems.DisplayMember = displayMember
        Me.lbxChoosedItems.ValueMember = valueMember
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public ReadOnly Property propValueMember() As String
        Get
            Return lbxChoosedItems.SelectedValue
        End Get
    End Property

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

End Class
