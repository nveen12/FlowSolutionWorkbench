Imports System.Windows.Forms

Public Class dlgAvailableDocuments

    Private _orgID As Integer
    Private _entityID As String
    Private _entityName As String


    ''' <summary>
    ''' Basic constructor for the attachments dialogue.
    ''' </summary>
    ''' <param name="orgID">The organization that creates the attachment.</param>
    ''' <param name="entityID">The entity id we want to attach to.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal orgID As Integer, ByVal entityID As String, ByVal entityName As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me._orgID = orgID
        Me._entityID = entityID
        Me._entityName = entityName

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'Add the attachment to the comments table
        If dgvAvailableDocuments.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row first.")
            Exit Sub
        End If

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim int As Integer = dgvAvailableDocuments.SelectedRows(0).Cells("attachementID").Value

        db.SecureNonQueryParams("INSERT INTO warehouseAttachementComments ([commentName] " & _
           ",[attachementID]) VALUES(@1,@2) ", Me._entityID, int)

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()


    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPurpose.TextChanged

    End Sub
    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.TextChanged

    End Sub

    Private Sub dlgAvailableDocuments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.loadData()
    End Sub

    Private Sub loadData(Optional ByVal filterString As String = "")
        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        Dim ds As New DataSet

        ds = db.SecureQueryParams("SELECT attachementID, " & _
                                     " attachmentName as Name, attachmentPurpose As Purpose, creationDate As [Creation Date], createdBy As [Created By] " & _
                                     "FROM warehouseAttachements WHERE organizationID = @1 " & filterString, Me._orgID)

        dgvAvailableDocuments.DataSource = ds.Tables(0)
        dgvAvailableDocuments.Columns("attachementID").Visible = False
    End Sub

    Private Sub dgvAvailableDocuments_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvAvailableDocuments.CellFormatting
        'every second row will become beige
        If e.RowIndex Mod 2 = 0 Then
            e.CellStyle.BackColor = Color.Beige
        End If

    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click

        ofdAttachFiles.ShowDialog()

    End Sub

    Public Property attachmentName() As String
        Get
            Return txtName.Text
        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Private Sub ofdAttachFiles_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ofdAttachFiles.FileOk
        If txtName.Text <> "" AndAlso txtPurpose.Text <> "" Then

            'Dim fs As New System.IO.StreamReader(ofdAttachFiles.FileName)
            Dim fInfo As New System.IO.FileInfo(ofdAttachFiles.FileName)
            Dim len As Long = fInfo.Length
            Try

                Using stream As New System.IO.FileStream(ofdAttachFiles.FileName, IO.FileMode.Open)

                    Dim imgData() As Byte = New Byte(Convert.ToInt32(len - 1)) {}
                    stream.Read(imgData, 0, fInfo.Length)

                    Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
                    Dim newID As Integer = db.Query("SELECT isnull( MAX(attachementID) + 1, 1) FROM warehouseAttachements").Tables(0).Rows(0).Item(0)

                    db.SecureInsertQueryParams("INSERT INTO [warehouseAttachements] " & _
                                                  " ([organizationID]  " & _
                                                  " ,[attachementID]   " & _
                                                  " ,[attachmentName]  " & _
                                                  " ,[attachmentPurpose] " & _
                                                  " ,[creationDate]  " & _
                                                  " ,[createdBy]  " & _
                                                  " ,[attachment], fileName) " & _
                                                  "  VALUES(@1,@2,@3, @4,@5,@6,@7,@8)", Me._orgID, newID, _
                                                  txtName.Text, txtPurpose.Text, Format(Now.Date, "yyyyMMdd"), _
                                                  Environment.UserName, imgData, ofdAttachFiles.SafeFileName)

                End Using
                Me.loadData()

            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        Else
            MessageBox.Show("Please enter name and purpose of your attachment first.")
        End If

        ofdAttachFiles.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim db As New DB.ServerDB(My.Settings.FLOWConnectionString)
        If dgvAvailableDocuments.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row first.")

        Else
            For i As Integer = 0 To dgvAvailableDocuments.SelectedRows.Count - 1
                db.SecureNonQueryParams("DELETE FROm warehouseAttachements WHERE attachementID = @1", dgvAvailableDocuments.SelectedRows(i).Cells("attachementID").Value)

            Next
        End If
        Me.loadData()

    End Sub

    Private Sub dgvAvailableDocuments_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvAvailableDocuments.CellClick
        If dgvAvailableDocuments.SelectedRows.Count > 0 Then
            txtName.Text = dgvAvailableDocuments.SelectedRows(0).Cells("Name").Value.ToString
            txtPurpose.Text = dgvAvailableDocuments.SelectedRows(0).Cells("Purpose").Value.ToString

        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim strFilter As String = ""
        strFilter = " AND ( attachmentName LIKE '%" & txtSearch.Text & "%' OR attachmentPurpose LIKE '%" & txtSearch.Text & "%' OR createdBy LIKE '%" & txtSearch.Text & "%')  "
        Me.loadData(strFilter)

    End Sub
End Class
