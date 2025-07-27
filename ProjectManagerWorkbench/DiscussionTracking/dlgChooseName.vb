Imports System.Windows.Forms

Public Class dlgChooseName
    Private organizationID As Integer


    ''' <summary>
    ''' constructor that loads the list with all available user's
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal strHeaderText As String, Optional ByVal strAddText As String = "")

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.loadAllUsers()
        Me.Text = strHeaderText

        ' Add any initialization after the InitializeComponent() call.
        If strAddText <> "" Then
            lblFilterName.Text = strAddText
            lblFilterName.Visible = True
        End If

    End Sub


    Public Sub New(ByVal organizationID As Integer, Optional ByVal strHeaderText As String = "")
        Me.organizationID = organizationID

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.loadUsersInOrganization()

        If Not strHeaderText = "" Then
            Me.Text = strHeaderText
        End If

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

    Private Sub dlgChooseName_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
       
    End Sub

    Private Sub loadUsersInOrganization()
        Try


            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            Dim ds As New DataSet

            ds = db.SecureQueryParams("SELECT user_name, (lastName + ',' + firstName) as display  FROM warehouse_users WHERE userDefaultOrganization = @1 ORDER BY lastName", Me.organizationID)

            cbxUsers.DataSource = ds.Tables(0)
            cbxUsers.DisplayMember = "display"
            cbxUsers.ValueMember = "user_name"

        Catch ex As Exception

        End Try
    End Sub

    Private Sub loadAllUsers()

        Try
            Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
            Dim ds As New DataSet

            ds = db.Query("SELECT user_name, (lastName + ',' + firstName) as display  FROM warehouse_users WHERE lastName IS NOT NULL  ORDER BY lastName")

            cbxUsers.DataSource = ds.Tables(0)
            cbxUsers.DisplayMember = "display"
            cbxUsers.ValueMember = "user_name"

        Catch ex As Exception

        End Try
    End Sub

    Public ReadOnly Property propReturnUserName() As String
        Get
            Return Me.cbxUsers.SelectedValue
        End Get

    End Property

End Class
