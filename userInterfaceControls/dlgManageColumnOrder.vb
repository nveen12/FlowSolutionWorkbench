Imports System.Windows.Forms
Public Class dlgManageColumnOrder
    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)
    Private organizationId As Double = 0
    Private dsColumnOrder As New DataSet
    Private m_planningType As String


    Public Sub New(ByVal Id As Integer, ByVal planningType As String)
        Me.m_planningType = planningType

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.organizationId = Id
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub dlgManageColumnOrder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'Display the views
            Dim dsEntities As DataSet = db.Query(" SELECT [displayName] " & _
                                 " ,[value] FROM warehouseEntities WHERE application = 'frmWorkbenchProjects' AND planningType = '" & _
                                 Me.m_planningType & "' ORDER BY displayName ASC")
            Dim dr As DataRow = dsEntities.Tables(0).NewRow()
            dr("displayName") = "Select"
            dr("value") = "0"
            dsEntities.Tables(0).Rows.InsertAt(dr, 0)
            cbView.DataSource = dsEntities.Tables(0)
            cbView.DisplayMember = "displayName"
            cbView.ValueMember = "value"

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("dlgManageColumnOrder_Load", ex, 1)
        End Try
    End Sub

    Private Sub cbView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbView.SelectedIndexChanged
        Try
            'Display the current order of columns for the selected view
            dsColumnOrder = db.SecureQueryParams("SELECT [column_header], [column_order], [visibility], [id] from whColumnUserVisibleView WHERE entity_name = @1 " & _
                                                "AND user_name = @2 ORDER BY visibility DESC, column_order ASC", cbView.SelectedValue.ToString(), Environment.UserName)

            Dim dv As DataView = dsColumnOrder.Tables(0).DefaultView
            dv.RowFilter = "visibility = 1"
            lbColumnOrder.DataSource = dv
            lbColumnOrder.DisplayMember = "column_header"
            lbColumnOrder.ValueMember = "column_order"

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("cbViewtoCopy_SelectedIndexChanged", ex, 1)
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            'Validation for select view
            If cbView.SelectedItem(0).ToString() = "Select" Then
                MessageBox.Show("Please select view")
                Exit Sub
            End If

            'Update the changed column order
            If dsColumnOrder.HasChanges() Then
                Me.Cursor = Cursors.WaitCursor
                For i As Int32 = 0 To dsColumnOrder.Tables(0).Rows.Count - 1
                    If Convert.ToInt32(dsColumnOrder.Tables(0).Rows(i).Item("column_order").ToString()) <> i Then
                        db.SecureNonQueryParams("UPDATE whColumnUserVisibleView SET column_order = @1 WHERE entity_name = @2 " & _
                                                "AND user_name = @3 AND id = @4", i, cbView.SelectedValue.ToString(), Environment.UserName, dsColumnOrder.Tables(0).Rows(i).Item("id").ToString())
                    End If
                Next
                Me.Cursor = Cursors.Default
            End If

            'Refresh the Grid
            RefreshGrids()

            MessageBox.Show("New order has been updated successfully.")

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("btnOK_Click", ex, 1)
        End Try
    End Sub

    ''' <summary>
    ''' Move the selected item in listbox to top of the list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTop.Click
        Try
            Dim Index As Integer = lbColumnOrder.SelectedIndex
            If dsColumnOrder.Tables.Count > 0 Then
                Dim dr As DataRow = dsColumnOrder.Tables(0).NewRow
                dr("column_header") = lbColumnOrder.SelectedItem(0).ToString()
                dr("column_order") = lbColumnOrder.SelectedValue.ToString()
                dr("visibility") = dsColumnOrder.Tables(0).Rows(Index).Item("visibility").ToString()
                dr("id") = dsColumnOrder.Tables(0).Rows(Index).Item("id").ToString()
                If Not (dr Is Nothing) And Index > 0 Then
                    dsColumnOrder.Tables(0).Rows.RemoveAt(Index)
                    dsColumnOrder.Tables(0).Rows.InsertAt(dr, 0)
                    lbColumnOrder.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("btnTop_Click", ex, 1)
        End Try
    End Sub

    ''' <summary>
    ''' Move the selected item in listbox to one up in the list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Try
            Dim Index As Integer = lbColumnOrder.SelectedIndex
            If dsColumnOrder.Tables.Count > 0 Then
                Dim dr As DataRow = dsColumnOrder.Tables(0).NewRow
                dr("column_header") = lbColumnOrder.SelectedItem(0).ToString()
                dr("column_order") = lbColumnOrder.SelectedValue.ToString()
                dr("visibility") = dsColumnOrder.Tables(0).Rows(Index).Item("visibility").ToString()
                dr("id") = dsColumnOrder.Tables(0).Rows(Index).Item("id").ToString()
                If Not (dr Is Nothing) And Index > 0 Then
                    dsColumnOrder.Tables(0).Rows.RemoveAt(Index)
                    dsColumnOrder.Tables(0).Rows.InsertAt(dr, Index - 1)
                    lbColumnOrder.SelectedIndex = Index - 1
                End If
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("btnUp_Click", ex, 1)
        End Try
    End Sub

    ''' <summary>
    ''' Move the selected item in listbox to one down in the list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        Try
            Dim Index As Integer = lbColumnOrder.SelectedIndex
            If dsColumnOrder.Tables.Count > 0 Then
                Dim dr As DataRow = dsColumnOrder.Tables(0).NewRow
                dr("column_header") = lbColumnOrder.SelectedItem(0).ToString()
                dr("column_order") = lbColumnOrder.SelectedValue.ToString()
                dr("visibility") = dsColumnOrder.Tables(0).Rows(Index).Item("visibility").ToString()
                dr("id") = dsColumnOrder.Tables(0).Rows(Index).Item("id").ToString()
                If Not (dr Is Nothing) And (Index + 1 < lbColumnOrder.Items.Count) Then
                    dsColumnOrder.Tables(0).Rows.RemoveAt(Index)
                    dsColumnOrder.Tables(0).Rows.InsertAt(dr, Index + 1)
                    lbColumnOrder.SelectedIndex = Index + 1
                End If
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("btnDown_Click", ex, 1)
        End Try
    End Sub

    ''' <summary>
    ''' Move the selected item in listbox to bottom of the list
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBottom.Click
        Try
            Dim Index As Integer = lbColumnOrder.SelectedIndex
            If dsColumnOrder.Tables.Count > 0 Then
                Dim dr As DataRow = dsColumnOrder.Tables(0).NewRow
                dr("column_header") = lbColumnOrder.SelectedItem(0).ToString()
                dr("column_order") = lbColumnOrder.SelectedValue.ToString()
                dr("visibility") = dsColumnOrder.Tables(0).Rows(Index).Item("visibility").ToString()
                dr("id") = dsColumnOrder.Tables(0).Rows(Index).Item("id").ToString()
                If Not (dr Is Nothing) And (Index + 1 < lbColumnOrder.Items.Count) Then
                    dsColumnOrder.Tables(0).Rows.RemoveAt(Index)
                    dsColumnOrder.Tables(0).Rows.InsertAt(dr, lbColumnOrder.Items.Count)
                    lbColumnOrder.SelectedIndex = lbColumnOrder.Items.Count - 1
                End If
            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("btnBottom_Click", ex, 1)
        End Try
    End Sub

    Private Sub RefreshGrids()
        Try
            frmWorkbenchProjects.gridDisplay.loadColumnSettings()

            Select Case cbView.SelectedValue.ToString()
                Case "MRPShortagesView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvShortages, cbView.SelectedValue.ToString())

                Case "POQueueView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvWipSupplierQueue, cbView.SelectedValue.ToString())
                Case "MSCPOQueueView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvWipSupplierQueue, cbView.SelectedValue.ToString())
                Case "salesOrderLinesView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvSalesOrders, cbView.SelectedValue.ToString())

                Case "supplyDemandMRPView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvShortages, cbView.SelectedValue.ToString())
                Case "MSCsupplyDemandMRPView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvShortages, cbView.SelectedValue.ToString())
                Case "WIPQueueView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvWipSupplierQueue, cbView.SelectedValue.ToString())
                Case "MSCWIPQueueView"
                    frmWorkbenchProjects.gridDisplay.column_reordering(frmWorkbenchProjects.dgvWipSupplierQueue, cbView.SelectedValue.ToString())

                Case Else
                    Exit Select
            End Select

        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("RefreshGrids", ex, 1)
        End Try
    End Sub
End Class