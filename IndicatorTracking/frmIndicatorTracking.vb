Public Class frmIndicatorTracking
    Private _orgID As Integer = 255
    Private _frmWbAutob As frmWorkbenchProjects

    Dim dbS As New DB.ServerDB(My.Settings("FlowConnectionString"))

    Public Sub New(ByRef frmWorkbench As frmWorkbenchProjects)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        _frmWbAutob = frmWorkbench
        ' Add any initialization after the InitializeComponent() call.

    End Sub


    'Mockup implementation
    Private Sub frmIndicatorTracking_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        loadKeyIndicatorTree()
        'Dim uc As New ucProdCapacity(Me._orgID, _frmWbAutob)
        'tlpMain.Controls.Add(uc)

    End Sub

    Private Sub loadKeyIndicatorTree()

        Dim ds As New DataSet
        Dim node As TreeNode

        'Load the hierarchy of the current indicators
        ds = dbS.Query(" SELECT pe.[level1], pe.[level1ID] " & _
                          " ,pe.[level2], pe.[level2ID], pe.[level3ID], pe.level4ID, pe.level3, pe.level4 " & _
                          " ,pe.organization_id " & _
                          " ,(SELECT od.organizationName FROM whOrganizationDefinition od WHERE od.organizationId = pe.organization_id) organizationName " & _
                          " FROM [performance] pe  " & _
                          " WHERE (SELECT od.organizationName FROM whOrganizationDefinition od WHERE od.organizationId = pe.organization_id) IS NOT NULL " & _
                          " AND private IS NULL " & _
                          " GROUP BY pe.[level1], pe.[level1ID] " & _
                          ",pe.[level2],pe.[level2ID], pe.organization_id, pe.[level3ID], pe.level4ID, pe.level3, pe.level4 " & _
                          " ORDER BY organizationName , pe.level1, pe.level2 ")

        tvIndicator.Nodes.Clear()

        For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
            node = Nothing
            'Load the first level
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("organization_id")) Then
                node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", ""))
                If node Is Nothing Then
                    node = tvIndicator.Nodes.Add(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", ""), "Org.: " & ds.Tables(0).Rows(i).Item("organizationName").ToString.Replace(" ", ""), ds.Tables(0).Rows(i).Item("organizationName"))
                End If
            End If
            node = Nothing
            'Load the second level of indicators
            node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level1ID").ToString.Replace(" ", ""))
            If node Is Nothing Then
                node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", "")).Nodes.Add(ds.Tables(0).Rows(i).Item("level1ID").ToString, ds.Tables(0).Rows(i).Item("level1").ToString)
            End If
            node = Nothing

            'Load the third level of tree
            node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level1ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level2ID").ToString.Replace(" ", ""))
            If node Is Nothing Then
                node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level1ID").ToString.Replace(" ", "")).Nodes.Add(ds.Tables(0).Rows(i).Item("level2ID").ToString, ds.Tables(0).Rows(i).Item("level2").ToString)
            End If
            node = Nothing

            'Level 4 and 5 just maybe do exist
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("level3ID")) Then
                'Load the fourth level of tree
                node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level1ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level2ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level3ID").ToString.Replace(" ", ""))
                If node Is Nothing Then
                    node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level1ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level2ID").ToString.Replace(" ", "")).Nodes.Add(ds.Tables(0).Rows(i).Item("level3ID").ToString, ds.Tables(0).Rows(i).Item("level3").ToString)
                End If
                node = Nothing

                'Load the fith level of the tree
                If Not DBNull.Value.Equals(ds.Tables(0).Rows(i).Item("level4ID")) Then
                    node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level1ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level2ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level3ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level4ID").ToString.Replace(" ", ""))
                    If node Is Nothing Then
                        node = tvIndicator.Nodes(ds.Tables(0).Rows(i).Item("organization_id").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level1ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level2ID").ToString.Replace(" ", "")).Nodes(ds.Tables(0).Rows(i).Item("level3ID").ToString.Replace(" ", "")).Nodes.Add(ds.Tables(0).Rows(i).Item("level4ID").ToString, ds.Tables(0).Rows(i).Item("level4").ToString)
                    End If
                End If

            End If


        Next

    End Sub

    Private Sub tvIndicator_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvIndicator.MouseDoubleClick
        Try

        

            'Load the overview panels
            If tvIndicator.SelectedNode.Level = 1 Then
                'Load the production panel if required
                If tvIndicator.SelectedNode.Name = 1 Then
                    Dim uc2 As New ucProdCapacity(Me._orgID, Me._frmWbAutob)
                    tlpMain.Controls.Add(uc2)
                End If
            End If

            If tvIndicator.SelectedNode.Level = 2 Then

                Dim organizationIdChart As Integer = tvIndicator.SelectedNode.Parent.Parent.Name
                'This is still an aggegat one
                Dim ds As New DataSet
                ds = dbS.SecureQueryParams("SELECT TOP 1000 [organization_id] " & _
                                          ",[organization_name] " & _
                                          ",[level1] " & _
                                          ",[level2] " & _
                                          ",SUM([value]) value " & _
                                          ",AVG([no_of_values]) no_of_values" & _
                                          ",[creation_date] " & _
                                          ",[created_by] " & _
                                          ",[level1ID] " & _
                                          ",[level2ID] " & _
                                          "      FROM [performance] " & _
                                          "      WHERE organization_id = @1 " & _
                                          "AND level1ID = @2 " & _
                                          "AND level2ID = @3 AND [private] IS NULL AND [creation_date] > dateadd(day,-31,getdate()) " & _
                                          " GROUP BY [organization_id],[organization_name],[level1],[level2],[creation_date],[created_by],[level1ID],[level2ID] ", tvIndicator.SelectedNode.Parent.Parent.Name, tvIndicator.SelectedNode.Parent.Name, tvIndicator.SelectedNode.Name)

                Dim strEntity As String = tvIndicator.SelectedNode.Name
                Dim workNode As TreeNode = tvIndicator.SelectedNode

                For j As Integer = 0 To tvIndicator.SelectedNode.Level - 1
                    workNode = workNode.Parent
                    strEntity += workNode.Name
                Next

                Dim noOfValuesName As String = dbS.SecureQueryParams("SELECT nameNoOfValues FROM performanceNames WHERE indicatorID = @1", tvIndicator.SelectedNode.Name).Tables(0).Rows(0).Item(0).ToString

                Dim uc As New ucIndicatorTracker(ds, tvIndicator.SelectedNode.Text, tvIndicator.SelectedNode.Parent.Parent.Text, strEntity, tvIndicator.SelectedNode.Parent.Name, _
                                                 tvIndicator.SelectedNode.Name, tvIndicator.SelectedNode.Text, tvIndicator.SelectedNode.Parent.Text, noOfValuesName, organizationIdChart, tvIndicator.SelectedNode.Name)
                tlpMain.Controls.Add(uc)

            ElseIf tvIndicator.SelectedNode.Level = 3 Then
                Dim organizationIDChart As Integer = tvIndicator.SelectedNode.Parent.Parent.Parent.Name
                Dim ds As New DataSet
                ds = dbS.SecureQueryParams("SELECT TOP 1000 [organization_id]   " & _
                                          ",[organization_name]                 " & _
                                          ",[level2]                            " & _
                                          ",[level3]                            " & _
                                          ",SUM([value]) value                  " & _
                                          ",AVG([no_of_values]) no_of_values    " & _
                                          ",[creation_date]                     " & _
                                          ",[created_by]                        " & _
                                          ",[level2ID]                          " & _
                                          ",[level3ID]                          " & _
                                          "      FROM [performance]             " & _
                                          "      WHERE organization_id = @1     " & _
                                          "AND level2ID = @2 " & _
                                          "AND level3ID = @3 AND [private] IS  NULL AND [creation_date] > dateadd(day,-31,getdate()) GROUP BY [organization_id],[organization_name],[level2],[level3],[creation_date],[created_by],[level2ID],[level3ID] ", tvIndicator.SelectedNode.Parent.Parent.Parent.Name, tvIndicator.SelectedNode.Parent.Name, tvIndicator.SelectedNode.Name)

                Dim strEntity As String = tvIndicator.SelectedNode.Name
                Dim workNode As TreeNode = tvIndicator.SelectedNode

                For j As Integer = 0 To tvIndicator.SelectedNode.Level - 1
                    workNode = workNode.Parent
                    strEntity += workNode.Name
                Next

                Dim uc As New ucIndicatorTracker(ds, tvIndicator.SelectedNode.Text, tvIndicator.SelectedNode.Parent.Parent.Text, strEntity, _
                                                 tvIndicator.SelectedNode.Parent.Name, tvIndicator.SelectedNode.Name, tvIndicator.SelectedNode.Text, tvIndicator.SelectedNode.Parent.Text, "", organizationIDChart, tvIndicator.SelectedNode.Name, 3)
                tlpMain.Controls.Add(uc)

            ElseIf tvIndicator.SelectedNode.Level = 4 Then


            End If

        Catch ex As Exception

        End Try
    End Sub

    
    
End Class