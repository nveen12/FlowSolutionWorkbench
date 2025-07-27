Imports System.Windows.Forms
Public Class dlgCopyColumnOrder
    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)
    Private organizationId As Double = 0

    Public Sub New(ByVal Id As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.organizationId = Id
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub dlgCopyColumnOrder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Display the users in the same organization
            Dim dsUsers As DataSet = db.SecureQueryParams("SELECT ISNULL(([firstName] + ' ' + [lastName]), [user_Name]) [displayName], [user_Name] [value] " & _
                                                          "FROM warehouse_users WHERE userDefaultOrganization = @1 ORDER BY [displayName]", Me.organizationId)

            Dim dr As DataRow = dsUsers.Tables(0).NewRow()
            dr("displayName") = "Select"
            dr("value") = "0"
            dsUsers.Tables(0).Rows.InsertAt(dr, 0)
            cbUsers.DataSource = dsUsers.Tables(0)
            cbUsers.DisplayMember = "displayName"
            cbUsers.ValueMember = "value"

            'Display the views
            Dim dsEntities As DataSet = db.Query(" SELECT [displayName] " & _
                                 " ,[value] FROM warehouseEntities WHERE application = 'frmWorkbenchProjects' ORDER BY displayName ASC")
            Dim dr1 As DataRow = dsEntities.Tables(0).NewRow()
            dr1("displayName") = "Select"
            dr1("value") = "0"
            dsEntities.Tables(0).Rows.InsertAt(dr1, 0)
            cbViewtoCopy.DataSource = dsEntities.Tables(0)
            cbViewtoCopy.DisplayMember = "displayName"
            cbViewtoCopy.ValueMember = "value"

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("dlgCopyColumnOrder_Load", ex, 1)
        End Try
    End Sub

    Private Sub cbViewtoCopy_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbViewtoCopy.SelectedIndexChanged
        Try
            'Display the new order of columns from the selected user and view
            Dim dsColumnOrder As DataSet = db.SecureQueryParams("SELECT [column_header], [visibility] from whColumnUserVisibleView WHERE entity_name = @1 " & _
                                                                "AND user_name = @2 ORDER BY visibility DESC, column_order ASC", cbViewtoCopy.SelectedValue.ToString(), cbUsers.SelectedValue.ToString())
            Dim dv As DataView = dsColumnOrder.Tables(0).DefaultView
            dv.RowFilter = "visibility = 1"
            lbColumnNewOrder.DataSource = dv
            lbColumnNewOrder.DisplayMember = "column_header"
            lbColumnNewOrder.ValueMember = "column_header"
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("cbViewtoCopy_SelectedIndexChanged", ex, 1)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Try
            'Copy the new order of columns to the current user

            'Validation to select user to copy from
            If cbUsers.SelectedItem(0).ToString() = "Select" Then
                MessageBox.Show("Please select user to copy from")
                Exit Sub
            End If

            'Validation to select view to be copied
            If cbViewtoCopy.SelectedItem(0).ToString() = "Select" Then
                MessageBox.Show("Please select view to be copied")
                Exit Sub
            End If

            db.SecureNonQueryParams("UPDATE b SET b.column_order =  a.column_order FROM warehouse_grid_user_size_order a " & _
                                    " JOIN warehouse_grid_user_size_order b ON a.column_id = b.column_id AND b.user_name = @1 " & _
                                    "WHERE a.user_name = @2 AND a.entity_name = @3", Environment.UserName, cbUsers.SelectedValue.ToString(), cbViewtoCopy.SelectedValue.ToString())

            'Refresh the Grid
            refreshGrids()

            MessageBox.Show("Copied the new order for " & cbViewtoCopy.SelectedItem(0).ToString() & " columns from " & cbUsers.SelectedItem(0).ToString() & ".")

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("btnCopy_Click", ex, 1)
        End Try
    End Sub
    Private Sub refreshGrids()
        Try
            frmWorkbenchProjects.gridDisplay.loadColumnSettings()

            Select Case cbViewtoCopy.SelectedValue.ToString()
                Case "MRPShortagesView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvShortages, cbViewtoCopy.SelectedValue.ToString())
                Case "POQueueView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvWipSupplierQueue, cbViewtoCopy.SelectedValue.ToString())
                Case "salesOrderLinesView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvSalesOrders, cbViewtoCopy.SelectedValue.ToString())
                Case "supplyDemandMRPView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvShortages, cbViewtoCopy.SelectedValue.ToString())
                Case "WIPQueueView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvWipSupplierQueue, cbViewtoCopy.SelectedValue.ToString())
                Case Else
                    Exit Select
            End Select

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("refreshGrids", ex, 1)
        End Try
    End Sub

    Private Sub cbUsers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbUsers.SelectedIndexChanged
        cbViewtoCopy.SelectedIndex = 0
    End Sub
End Class