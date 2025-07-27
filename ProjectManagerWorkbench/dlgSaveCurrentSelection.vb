Imports System.Windows.Forms

Public Class dlgSaveCurrentSelection
    Private _entity As String


    Public Sub New(ByVal entity As String)
        InitializeComponent()
        _entity = entity

    End Sub



    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))

        If cbSelectionSave.Text = "" Then
            MessageBox.Show("Please enter" & ControlChars.CrLf & "a new user filter name")


        Else
            Dim ds As DataSet = db.SecureQueryParams("SELECT * FROM  warehouseUserFilters  WHERE userName = @1 AND filterName = @2 ", Environment.UserName, cbSelectionSave.Text)
            Dim ds3 As DataSet = db.SecureQueryParams("SELECT * FROM  warehouseFilterTemplates WHERE  filterName = @1 ", cbSelectionSave.Text)

            If ds.Tables(0).Rows.Count > 0 Or ds3.Tables(0).Rows.Count > 0 Then
                MessageBox.Show("Filter already" & ControlChars.CrLf & "exists")

            Else

                'Now save the old TEMP filter with a new one -> This is an update statement
                db.SecureNonQueryParams("UPDATE warehouseUserFilters SET filterName = @1 WHERE userName = @2 AND entity = @3 AND filterName = 'TEMP'", cbSelectionSave.Text, Environment.UserName, _entity)
               
            End If

        End If



        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()


    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgSaveCurrentSelection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim globe As New clsPublicVariables
        Me.Text = globe.nameOfTool & " - Save Current Selection"

        
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds2 As New DataSet

        ds2 = db.SecureQueryParams("SELECT filterId, filterName FROM warehouseUserFilters WHERE userName = @1", Environment.UserName)
        cbSelectionSave.DataSource = ds2.Tables(0)
        cbSelectionSave.ValueMember = "filterId"
        cbSelectionSave.DisplayMember = "filterName"

        If _entity = "salesOrderLinesView" Then
            lblEntityname.Text = "Sales Order Lines"
        ElseIf _entity = "MRPShortages" Then
            lblEntityname.Text = "MRP Shortages"
        ElseIf _entity = "POQueueView" Then
            lblEntityname.Text = "Purchase Order Queue View"
        ElseIf _entity = "WIPQueueView" Then
            lblEntityname.Text = "WIP Queue View"
        End If


    End Sub
End Class
