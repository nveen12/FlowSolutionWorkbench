Public Class ucFilterSetup
    Private filterStringView() As String = {"Equals", "Contains", "Does Not Equal", "Is Greater Than", "Is Greater Than Or Equal To", "Is Less Than", "Is Less Than Or Equal To", "Does Not Contain"}
    Private filterStringValues() As String = {"=", "LIKE", "<>", ">", ">=", "<", "<=", " NOT LIKE"}
    Private _detailId As Integer
    Private _column As String
    Private _entity As String
    Private _organizationId As Integer


    Public Sub New(ByVal detailId As Integer, ByVal filterId As Integer, ByVal column As String, ByVal columnHeader As String, ByVal newControl As Boolean, ByVal entity As String, ByVal organizationId As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        _column = column
        _entity = entity
        _detailId = detailId
        _organizationId = organizationId

        Me.lblColumn.Text = columnHeader
        loadFilterSetup()

        ' Add any initialization after the InitializeComponent() call.
        If newControl Then
            cbConnection.SelectedIndex = 0
            Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
            db.SecureQueryParams("INSERT INTO warehouseUserFilterDetail (subFilterId, filterId, currentColumn, connectionElements) VALUES (@1, @2, @3, 'AND')", detailId, filterId, column)
        Else
            Me.loadUserControl(detailId)
        End If



    End Sub

    ''' <summary>
    ''' Queries the sql server backend to get the type of the column.
    ''' </summary>
    ''' <param name="tableName">The table we are looking for.</param>
    ''' <returns>The value of the column.</returns>
    ''' <remarks></remarks>
    Private Function strFilteredColumn(ByVal tableName As String, ByVal columnNamer As String)
        Dim strColumnType As String = ""
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet

        ds = db.Query("SELECT   schemas.name AS [Schema],  " & _
                            "        tables.name AS [Table],     " & _
                            "    columns.name AS [Column],      " & _
                            "   CASE              WHEN columns.system_type_id = 34    THEN 'byte[]'    " & _
                            " WHEN columns.system_type_id = 35    THEN 'string'             " & _
                            " WHEN columns.system_type_id = 36    THEN 'System.Guid'             " & _
                            " WHEN columns.system_type_id = 48    THEN 'byte'             " & _
                            " WHEN columns.system_type_id = 52    THEN 'short'             " & _
                            " WHEN columns.system_type_id = 56    THEN 'int'             " & _
                            " WHEN columns.system_type_id = 58    THEN 'System.DateTime'          " & _
                            " WHEN columns.system_type_id = 59    THEN 'float'             " & _
                            " WHEN columns.system_type_id = 60    THEN 'decimal'             " & _
                            " WHEN columns.system_type_id = 61    THEN 'System.DateTime'             " & _
                            " WHEN columns.system_type_id = 62    THEN 'double'             " & _
                            " WHEN columns.system_type_id = 98    THEN 'object'             " & _
                            " WHEN columns.system_type_id = 99    THEN 'string'             " & _
                            " WHEN columns.system_type_id = 104   THEN 'bool'             " & _
                            " WHEN columns.system_type_id = 106   THEN 'decimal'             " & _
                            " WHEN columns.system_type_id = 108   THEN 'decimal'            " & _
                            " WHEN columns.system_type_id = 122   THEN 'decimal'            " & _
                            " WHEN columns.system_type_id = 127   THEN 'long'           " & _
                             "  WHEN columns.system_type_id = 165   THEN 'byte[]'             " & _
                            " WHEN columns.system_type_id = 167   THEN 'string'           " & _
                             "  WHEN columns.system_type_id = 173   THEN 'byte[]'            " & _
                            "  WHEN columns.system_type_id = 175   THEN 'string'           " & _
                             "  WHEN columns.system_type_id = 189   THEN 'long'            " & _
                            "  WHEN columns.system_type_id = 231   THEN 'string'             " & _
                            " WHEN columns.system_type_id = 239   THEN 'string'             " & _
                            " WHEN columns.system_type_id = 241   THEN 'string'            " & _
                            "  WHEN columns.system_type_id = 241   THEN 'string'         END AS [Type],         columns.is_nullable AS [Nullable]  " & _
                            "FROM              sys.tables tables    INNER JOIN    sys.schemas schemas ON (tables.schema_id = schemas.schema_id )    INNER JOIN    sys.columns columns ON (columns.object_id = tables.object_id)  WHERE     tables.name <> 'sysdiagrams'     AND   tables.name <> 'dtproperties'  " & _
                            "ORDER BY [Schema], [Table], [Column], [Type]")

        Return strColumnType

    End Function


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub loadUserControl(ByVal detail As Integer)
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim ds As New DataSet
        ds = db.SecureQueryParams("SELECT [subFilterId] " & _
      ",[filterId] " & _
      ",[currentColumn] " & _
      ",[sqlOperation] " & _
      ",[phraseValue] " & _
      ",[substring] " & _
      ",[startPosition] " & _
      ",[endPosition] " & _
      ",[connectionElements] " & _
  "FROM [warehouseUserFilterDetail] WHERE [subFilterId] = @1", _detailId)

        'Override the real values
        _column = ds.Tables(0).Rows(0).Item("currentColumn")
        Dim ds2 As DataSet
        ds2 = db.SecureQueryParams("SELECT entity FROM warehouseUserFilters WHERE filterId = @1", ds.Tables(0).Rows(0).Item("filterId"))
        _entity = ds2.Tables(0).Rows(0).Item("entity")

        Dim ds3 As DataSet = db.SecureQueryParams("SELECT column_header FROM [warehouse_grid_columns] WHERE entity_name = @1 AND database_column = @2", _entity, _column)

        If ds3.Tables(0).Rows.Count > 0 Then
            Me.lblColumn.Text = ds3.Tables(0).Rows(0).Item("column_header")
        End If


        If ds.Tables(0).Rows(0).Item("connectionElements") = "AND" Then
            Me.cbConnection.SelectedIndex = 0
        Else
            Me.cbConnection.SelectedIndex = 1
        End If

        'Find the saved sql operation

        For i As Integer = 0 To filterStringValues.Count - 1
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("sqlOperation")) Then
                If filterStringValues(i) = ds.Tables(0).Rows(0).Item("sqlOperation") Then
                    cbSQLOperation.SelectedIndex = i
                    Exit For
                End If
            End If

        Next

        If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("phraseValue")) Then
            Me.cbValues.Text = ds.Tables(0).Rows(0).Item("phraseValue")
        End If

        If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("substring")) Then
            Me.cbMiddleString.Checked = ds.Tables(0).Rows(0).Item("substring")
        End If

        If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("startPosition")) Then
            Me.txtStart.Text = ds.Tables(0).Rows(0).Item("startPosition")
        End If

        If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("endPosition")) Then
            Me.txtEnd.Text = ds.Tables(0).Rows(0).Item("endPosition")
        End If


    End Sub

    Private Sub loadFilterSetup()

        'Load the the values of the filter string view into the combobox
        For i As Integer = 0 To filterStringView.Count - 1
            cbSQLOperation.Items.Add(filterStringView(i))
        Next
    End Sub



    Private Sub cbMiddleString_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMiddleString.CheckedChanged

        If cbMiddleString.Checked Then
            lblEnd.Visible = True
            lblStart.Visible = True
            txtStart.Visible = True
            txtEnd.Visible = True
        End If

    End Sub

    Private Sub btnDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDel.Click
        Me.Visible = False
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        db.SecureNonQueryParams("DELETE FROM warehouseUserFilterDetail WHERE  subFilterId = @1", _detailId)
    End Sub

    Public Sub saveUserControl()
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))


        db.SecureNonQueryParams("UPDATE [warehouseUserFilterDetail] " & _
                "SET  " & _
                "[currentColumn] = @1 " & _
                ",[sqlOperation] = @2 " & _
                ",[phraseValue] =  @3 " & _
                ",[substring] = @4 " & _
                ",[startPosition] = @5 " & _
                ",[endPosition] = @6  " & _
                ",[connectionElements] = @7 " & _
                "WHERE subFilterId = @8 ", _column, filterStringValues(cbSQLOperation.SelectedIndex), cbValues.Text, cbMiddleString.Checked, txtStart.Text, txtEnd.Text, cbConnection.Text, _detailId)

    End Sub

    Private Sub cbValues_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles cbValues.MouseClick
        Dim db As New DB.ServerDB(My.Settings("FlowConnectionString"))
        Dim strSQL As String
        Dim ds As New DataSet

        'Get the current filter string for this column ' Problem with boolean values
        strSQL = "SELECT DISTINCT(" & _column & ") as val FROM " & _entity & " WHERE organization_id = " & _organizationId
        ds = db.Query(strSQL)

       
        Try
            If ds.Tables(0).Rows(0).Item(0) = True Or ds.Tables(0).Rows(0).Item(0) = False Then
                strSQL = "SELECT DISTINCT(  CASE WHEN [" & _column & "] = 1 THEN 'Y' ELSE 'N' END) val FROM " & _entity & " Where organization_id = " & _organizationId
                ds = db.Query(strSQL)
                Me.cbValues.DataSource = ds.Tables(0)
            Else
                Me.cbValues.DataSource = ds.Tables(0)
            End If
        Catch ex As Exception
            Me.cbValues.DataSource = ds.Tables(0)
        End Try

        cbValues.DisplayMember = "val"

    End Sub





End Class
