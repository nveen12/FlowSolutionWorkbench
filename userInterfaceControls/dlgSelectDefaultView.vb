Imports System.Windows.Forms



Public Class dlgSelectDefaultView
    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)
    Private _dsCurrentFilter As DataSet
    Private _dsCurrentTemplates As DataSet
    Private bLoad As Boolean
    Private organizationId As Double = 0

    Public Sub New(ByVal Id As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.organizationId = Id
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.saveDefaultView()
            Me.Close()
        Catch ex As Exception
            Me.DialogResult = System.Windows.Forms.DialogResult.Ignore
        End Try


        
    End Sub

    Private Sub saveDefaultView()
        '  db.SecureNonQuery("DELETE FROM ")
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgSelectDefaultView_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        bLoad = True
        Dim dsEntities As DataSet = db.Query(" SELECT [displayName] " & _
                                             " ,[value] FROM warehouseEntities WHERE application = 'frmWorkbenchProjects'")
        lbxAreas.DataSource = dsEntities.Tables(0)
        lbxAreas.DisplayMember = "displayName"
        lbxAreas.ValueMember = "value"

        bLoad = False
    End Sub

    Private Sub lbxAreas_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxAreas.SelectedIndexChanged

        If Not bLoad Then
            _dsCurrentFilter = db.SecureQueryParams("SELECT filterId, filterName FROM warehouseUserFilters WHERE userName = @1 AND entity = @2 ORDER BY filterName", Environment.UserName, lbxAreas.SelectedValue)
            _dsCurrentTemplates = db.SecureQueryParams("SELECT templateId, filterName FROM warehouseFilterTemplates WHERE (organizationId IS NULL OR organizationId = @1) AND entity = @2 ORDER BY filterName", Me.organizationId, lbxAreas.SelectedValue)

            Dim dsCurrentDefault As DataSet = db.SecureQueryParams("SELECT isDefault, filterNameUser, filterNameTemplate FROM whUserFiltersDefaultView WHERE userName = @1 AND (entityUser = @2 OR entityTemplate = @2)", Environment.UserName, lbxAreas.SelectedValue)

            clbxDefaultViews.Items.Clear()
            Try
                

                'Add the members and check if one is the default filter
                For i As Integer = 0 To _dsCurrentTemplates.Tables(0).Rows.Count - 1
                    If dsCurrentDefault.Tables(0).Rows.Count > 0 AndAlso Not DBNull.Value.Equals(dsCurrentDefault.Tables(0).Rows(0).Item("filterNameTemplate")) Then
                        If _dsCurrentTemplates.Tables(0).Rows(i).Item("filterName") = dsCurrentDefault.Tables(0).Rows(0).Item("filterNameTemplate") Then
                            clbxDefaultViews.Items.Add(_dsCurrentTemplates.Tables(0).Rows(i).Item("filterName"), True)
                        Else
                            clbxDefaultViews.Items.Add(_dsCurrentTemplates.Tables(0).Rows(i).Item("filterName"))
                        End If
                    Else
                        clbxDefaultViews.Items.Add(_dsCurrentTemplates.Tables(0).Rows(i).Item("filterName"))
                    End If

                Next

                For i As Integer = 0 To _dsCurrentFilter.Tables(0).Rows.Count - 1
                    If dsCurrentDefault.Tables(0).Rows.Count > 0 AndAlso Not DBNull.Value.Equals(dsCurrentDefault.Tables(0).Rows(0).Item("filterNameUser")) Then
                        If _dsCurrentFilter.Tables(0).Rows(i).Item("filterName") = dsCurrentDefault.Tables(0).Rows(0).Item("filterNameUser") Then
                            clbxDefaultViews.Items.Add(_dsCurrentFilter.Tables(0).Rows(i).Item("filterName"), True)
                        Else
                            clbxDefaultViews.Items.Add(_dsCurrentFilter.Tables(0).Rows(i).Item("filterName"))
                        End If
                    Else
                        clbxDefaultViews.Items.Add(_dsCurrentFilter.Tables(0).Rows(i).Item("filterName"))
                    End If

                Next

            Catch ex As Exception

            End Try
            
        End If
       


    End Sub

    Private Sub clbxDefaultViews_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clbxDefaultViews.SelectedIndexChanged
        Try

       
            If Not bLoad Then
                'Make sure, only one element is selected
                Dim currenSelection As Integer = clbxDefaultViews.SelectedIndex

                If clbxDefaultViews.CheckedItems.Count > 1 Then
                    MessageBox.Show("Please just select one view as default.")
                ElseIf clbxDefaultViews.CheckedItems.Count = 0 Then
                    'Delete deselect
                    Dim dsOldrelation As DataSet = db.SecureQueryParams("SELECT relationID FROM whUserFiltersDefaultView WHERE (entityUser = @1 OR entityTemplate = @1) AND userName = @2", lbxAreas.SelectedValue, Environment.UserName)
                    If dsOldrelation.Tables(0).Rows.Count > 0 Then
                        db.SecureNonQueryParams("DELETE FROM warehouseUserDefaultFilter WHERE relationID = @1", dsOldrelation.Tables(0).Rows(0).Item(0))
                    End If
                Else
                    'Save the changes to the backend ...
                    If clbxDefaultViews.CheckedItems.Count > 1 Then
                        MessageBox.Show("Please just select one view as default.")
                    ElseIf clbxDefaultViews.CheckedItems.Count = 1 Then

                        'See if there was an old record for this entity
                        Dim oldRelationID As Integer = 0
                        Dim dsOldrelation As DataSet = db.SecureQueryParams("SELECT relationID FROM whUserFiltersDefaultView WHERE (entityUser = @1 OR entityTemplate = @1) AND userName = @2", lbxAreas.SelectedValue, Environment.UserName)

                        'Make sure the user just enters one default view for the botton grid
                        If lbxAreas.SelectedValue = "wipQueueView" Then
                            Dim dsCheckRelation As DataSet = db.SecureQueryParams("SELECT relationID FROM whUserFiltersDefaultView WHERE (entityUser = 'POQueueView' OR entityTemplate = 'POQueueView') AND userName = @1", Environment.UserName)
                            If dsCheckRelation.Tables(0).Rows.Count > 0 Then
                                clbxDefaultViews.Items.Clear()
                                MessageBox.Show("Please deselect Purchase Order Lines default filter first.")
                                Exit Sub
                            End If
                        ElseIf lbxAreas.SelectedValue = "POQueueView" Then
                            Dim dsCheckRelation As DataSet = db.SecureQueryParams("SELECT relationID FROM whUserFiltersDefaultView WHERE (entityUser = 'wipQueueView' OR entityTemplate = 'wipQueueView') AND userName = @1", Environment.UserName)
                            If dsCheckRelation.Tables(0).Rows.Count > 0 Then
                                clbxDefaultViews.Items.Clear()
                                MessageBox.Show("Please deselect WIP Queue default filter first.")
                                Exit Sub
                            End If


                        End If

                        If dsOldrelation.Tables(0).Rows.Count > 0 Then
                            db.SecureNonQueryParams("DELETE FROM warehouseUserDefaultFilter WHERE relationID = @1", dsOldrelation.Tables(0).Rows(0).Item(0))
                        End If

                        'Gather the relevant information for the insert into statement
                        Dim relId As Integer = db.Query("SELECT ISNULL(Max(relationID),0) + 1 FROM warehouseUserDefaultFilter ").Tables(0).Rows(0).Item(0)

                        Dim userFilterId As Integer = 0
                        Dim userTemplateId As Integer = 0
                        'Gather userFilterId / template filter id
                        'Dim filterName As String = clbxDefaultViews.CheckedItems(0)
                        For i As Integer = 0 To _dsCurrentFilter.Tables(0).Rows.Count - 1
                            If _dsCurrentFilter.Tables(0).Rows(i).Item("filterName").ToString = clbxDefaultViews.CheckedItems(0).ToString Then
                                userFilterId = _dsCurrentFilter.Tables(0).Rows(i).Item("filterId")
                            End If
                        Next

                        For j As Integer = 0 To _dsCurrentTemplates.Tables(0).Rows.Count - 1
                            If _dsCurrentTemplates.Tables(0).Rows(j).Item("filterName").ToString = clbxDefaultViews.CheckedItems(0).ToString Then
                                userTemplateId = _dsCurrentTemplates.Tables(0).Rows(j).Item("templateId")
                            End If
                        Next



                        db.SecureNonQueryParams("INSERT INTO [warehouseUserDefaultFilter] " & _
                                                  " ([relationID] " & _
                                                  ",[userName] " & _
                                                  " ,[userFilterID] " & _
                                                  " ,[userTemplateID]) " & _
                                                  "          VALUES " & _
                                                  " (@1" & _
                                                  " ,@2 " & _
                                                  " ,@3 " & _
                                                  " ,@4 )", relId, Environment.UserName, userFilterId, userTemplateId)

                    End If

                End If




            End If
        Catch ex As Exception
            Dim err As New clsExceptionManagement
            err.createErrorLog("SaveDefaultView", ex, 1)
        End Try
    End Sub
End Class
