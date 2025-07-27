Imports System.Data.SqlClient
Imports System.Data
Imports System
Imports System.IO
Imports System.Configuration


REM Imports Microsoft.Practices.EnterpriseLibrary.Logging
'Imports LumenWorks.Framework.IO.Csv

'Update for Rob
Namespace DB

    Public Class ServerDB
        Private DSNName As String
        Private conn As SqlClient.SqlConnection
        Public autoDisconnect As Boolean

        Public Sub New(ByVal DSNName As String, Optional ByVal _autoDisconnect As Boolean = True)
            ' Reset settings to reload the latest values
            My.Settings.Reset()
            Dim connectionString As String = "Data Source=denatbdbp01;Initial Catalog=FSG_IND_WORKBENCH;Persist Security Info=True;User ID=WorkBenchUser;Password=WorkBench;TrustServerCertificate=True;User Instance=False;MultiSubnetFailover=True"
            Me.conn = New SqlClient.SqlConnection(connectionString)
            autoDisconnect = _autoDisconnect
        End Sub

        Public Function Connect() As Boolean
            'Attemting the connection to the database
            If conn Is Nothing Then
                conn = New SqlClient.SqlConnection(Me.DSNName)

            End If
            If conn.State = ConnectionState.Closed Then
                conn.Open()         'Connect to Database
            End If
            Return True         'If success return true
        End Function

        Protected Function InternalDisconnect() As Boolean
            If autoDisconnect Then
                Disconnect()
            End If
        End Function

        Function Disconnect() As Boolean
            'Try
            If conn.State = ConnectionState.Open Then
                conn.Close()        'Disconnect from Database
            End If
            Return True         'If success return true
        End Function

        ' ----------------------------------------------
        ' Secure* family of functions follows
        ' ----------------------------------------------

        Function CreateParameterisedCommand(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable)
            Dim cmd As SqlCommand = New SqlCommand(str_sql, conn)
            For Each item In SubstitutionFields
                cmd.Parameters.Add("@" & item.Key, item.Value)
            Next
            Return cmd
        End Function

        Function TransformParams(ByVal SubstitutionFields As Array) As Hashtable
            Dim transformedParams As New Hashtable
            Dim paramNo As Integer = 1

            For i = LBound(SubstitutionFields) To UBound(SubstitutionFields)
                transformedParams(paramNo) = SubstitutionFields(i)
                paramNo += 1
            Next

            Return transformedParams
        End Function

        Function SecureQuery(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable) As DataSet
            Dim adapter As New SqlClient.SqlDataAdapter
            Dim result As New DataSet


            Connect()
            adapter.SelectCommand = CreateParameterisedCommand(str_sql, SubstitutionFields)
            adapter.SelectCommand.CommandTimeout = 2000

            Try
                adapter.Fill(result)
            Catch

            Finally
                InternalDisconnect()
            End Try

            Return result
        End Function

        Function SecureQueryParams(ByVal str_sql As String, ByVal ParamArray SubstitutionFields() As Object) As DataSet
            Return SecureQuery(str_sql, TransformParams(SubstitutionFields))
        End Function


        Function SecureScalarQuery(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable)
            Connect()
            Dim result
            Try
                result = CreateParameterisedCommand(str_sql, SubstitutionFields).ExecuteScalar()
            Finally
                InternalDisconnect()
            End Try
            Return result
        End Function

        Function SecureScalarQueryParams(ByVal str_sql As String, ByVal ParamArray SubstitutionFields() As Object)
            Return SecureScalarQuery(str_sql, TransformParams(SubstitutionFields))
        End Function


        Function SecureNonQuery(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable) As Int32
            Connect()
            Dim result
            Try
                result = CreateParameterisedCommand(str_sql, SubstitutionFields).ExecuteNonQuery()
            Finally
                InternalDisconnect()
            End Try
            Return result
        End Function

        Function SecureNonQueryParams(ByVal str_sql As String, ByVal ParamArray SubstitutionFields() As Object) As Int32
            Return SecureNonQuery(str_sql, TransformParams(SubstitutionFields))
        End Function

        Function Query(ByVal str_sql As String) As DataSet

            Dim adapter As New SqlClient.SqlDataAdapter
            Dim result As New DataSet

            Connect()
            Dim cmd As SqlCommand = New SqlCommand(str_sql, conn)
            cmd.CommandTimeout = 2000

            adapter.SelectCommand = cmd

            Try
                adapter.Fill(result)
            Finally
                InternalDisconnect()
            End Try

            Return result
        End Function

        Function NonQuery(ByVal str_sql As String)
            Connect()
            Dim result
            Try
                Dim cmd As SqlCommand = New SqlCommand(str_sql, conn)

                'long timeout for the transformation work.
                cmd.CommandTimeout = 2000

                result = cmd.ExecuteNonQuery()

            Finally
                InternalDisconnect()
            End Try

            Debug.Print(str_sql & " Number touched: " & result.ToString)

            Return result
        End Function

        Function SecureInsertQuery(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable) As Int32
            str_sql &= " SELECT @newlyInsertedId = SCOPE_IDENTITY()"

            Connect()

            Dim param As New SqlParameter("@newlyInsertedId", SqlDbType.BigInt)
            param.Direction = ParameterDirection.Output

            Dim cmd = CreateParameterisedCommand(str_sql, SubstitutionFields)
            cmd.Parameters.Add(param)
            cmd.CommandTimeout = 2000

            Try
                cmd.ExecuteNonQuery()
            Finally
                InternalDisconnect()
            End Try

            If DBNull.Value.Equals(cmd.Parameters("@newlyInsertedId").Value) Then
                Return -1
            Else
                Return cmd.Parameters("@newlyInsertedId").Value
            End If
        End Function

        Function SecureInsertQueryParams(ByVal str_sql As String, ByVal ParamArray SubstitutionFields() As Object) As Int32
            Return SecureInsertQuery(str_sql, TransformParams(SubstitutionFields))
        End Function

        Sub BulkInsert(ByVal destinationTableName As String, ByRef table As DataTable, ByVal columnMappings As Dictionary(Of String, String))
            Connect()
            Dim bc As SqlBulkCopy = New SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, Nothing)
            bc.BatchSize = table.Rows.Count
            bc.DestinationTableName = destinationTableName
            For Each columnMapping In columnMappings
                bc.ColumnMappings.Add(New SqlBulkCopyColumnMapping(columnMapping.Key, columnMapping.Value))
            Next
            Try
                bc.WriteToServer(table)
            Finally
                Try
                    bc.Close()
                Finally
                    InternalDisconnect()
                End Try
            End Try
        End Sub

        'Sub BulkInsertCSV(ByRef csvStream As Stream, ByVal destinationTableName As String)
        '    Connect()

        '    Dim streamReader As New StreamReader(csvStream)
        '    Dim delimiter As Char = ";"
        '    Dim csvReader As New CsvReader(streamReader, True, delimiter)
        '    csvReader.MissingFieldAction = LumenWorks.Framework.IO.Csv.MissingFieldAction.ParseError
        '    csvReader.DefaultParseErrorAction = LumenWorks.Framework.IO.Csv.ParseErrorAction.ThrowException

        '    Dim bcp As New SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock, Nothing)
        '    bcp.DestinationTableName = destinationTableName
        '    'For Each columnMapping In columnMappings
        '    'bcp.ColumnMappings.Add(New SqlBulkCopyColumnMapping(columnMapping.Key, columnMapping.Value))
        '    'Next

        '    Try
        '        bcp.WriteToServer(csvReader)
        '    Catch exc As Exception
        '        Dim msg As String = "CSV parse error at line: " & (csvReader.CurrentRecordIndex + 1)
        '        Dim rawData As String = csvReader.GetCurrentRawData()
        '        msg &= " content near error (first 600 chars): " & vbNewLine & rawData.Substring(0, Math.Min(600, rawData.Length)) & vbNewLine
        '        Throw New Exception(msg, exc)
        '    Finally
        '        Try
        '            bcp.Close()
        '        Finally
        '            InternalDisconnect()
        '        End Try
        '    End Try
        'End Sub

        Function GetQueryStringInTransaction(ByVal query As String) As String
            Return "BEGIN TRANSACTION; " & _
                   "BEGIN TRY " & _
                        query & _
                   "    COMMIT TRANSACTION; " & _
                   "END TRY " & _
                   "BEGIN CATCH " & _
                   "    ROLLBACK TRANSACTION; " & _
                   "    " & _
                   "    DECLARE @ErrorMessage NVARCHAR(4000); " & _
                   "    DECLARE @ErrorSeverity INT; " & _
                   "    DECLARE @ErrorState INT; " & _
                   "    " & _
                   "    SELECT @ErrorMessage = ERROR_MESSAGE(), " & _
                   "           @ErrorSeverity = ERROR_SEVERITY(), " & _
                   "           @ErrorState = ERROR_STATE(); " & _
                   "    " & _
                   "    RAISERROR (@ErrorMessage, " & _
                   "           @ErrorSeverity, " & _
                   "           @ErrorState " & _
                   "           );          " & _
                   "END CATCH; "
        End Function



        'Private Sub createErrorLog(ByVal procedureName As String, ByVal exceptionMessage As Exception)
        '    Try
        '        Dim db As New DB.ServerDB("")
        '        Dim invokedBy As String = Environment.UserName



        '        db.SecureScalarQueryParams("INSERT INTO [error_log] " & _
        '           "([calling_object] " & _
        '           ",[description] " & _
        '           ",[error_line] " & _
        '           ",[NO_OF_STEPS] " & _
        '           ",[PROCEDURE_OR_FUNCTION] " & _
        '           ",[invoked_by] " & _
        '           ",[CREATION_DATE] " & _
        '           ",[causing_object] )" & _
        '            " VALUES(@1, @2, @3, @4, @5, @6, getDate(), @7)", MyBase.ToString.Replace("'", ""), exceptionMessage.Message.Replace("'", ""), exceptionMessage.StackTrace.Replace("'", ""), _
        '            Me.currentStep, procedureName _
        '            , invokedBy, exceptionMessage.Source.Replace("'", ""))

        '    Catch ex As Exception

        '        EventLog.WriteEntry(ex.Source, ex.Message, EventLogEntryType.Error)

        '    End Try


        'End Sub
    End Class

    Public Class ORacleServerDB
        Private DSNName As String
        Private conn As OracleClient.OracleConnection


        Public autoDisconnect As Boolean

        Public Sub New(ByVal DSNName As String, Optional ByVal _autoDisconnect As Boolean = True)
            Me.DSNName = DSNName
            autoDisconnect = _autoDisconnect
        End Sub

        Public Function Connect() As Boolean
            'Attemting the connection to the database
            If conn Is Nothing Then
                conn = New OracleClient.OracleConnection(Me.DSNName)
            End If
            If conn.State = ConnectionState.Closed Then
                conn.Open()         'Connect to Database
            End If
            Return True         'If success return true
        End Function

        Protected Function InternalDisconnect() As Boolean
            If autoDisconnect Then
                Disconnect()
            End If
        End Function

        Function Disconnect() As Boolean
            'Try
            If conn.State = ConnectionState.Open Then
                conn.Close()        'Disconnect from Database
            End If
            Return True         'If success return true
        End Function

        ' ----------------------------------------------
        ' Secure* family of functions follows
        ' ----------------------------------------------

        Function CreateParameterisedCommand(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable)
            Dim cmd As OracleClient.OracleCommand = New OracleClient.OracleCommand(str_sql, conn)
            For Each item In SubstitutionFields
                cmd.Parameters.Add("@" & item.Key, item.Value)
            Next
            Return cmd
        End Function

        Function TransformParams(ByVal SubstitutionFields As Array) As Hashtable
            Dim transformedParams As New Hashtable
            Dim paramNo As Integer = 1

            For i = LBound(SubstitutionFields) To UBound(SubstitutionFields)
                transformedParams(paramNo) = SubstitutionFields(i)
                paramNo += 1
            Next

            Return transformedParams
        End Function

        Function SecureQuery(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable) As DataSet
            Dim adapter As New OracleClient.OracleDataAdapter
            Dim result As New DataSet

            Connect()
            adapter.SelectCommand = CreateParameterisedCommand(str_sql, SubstitutionFields)
            Try
                adapter.Fill(result)

            Finally
                InternalDisconnect()
            End Try

            Return result
        End Function

        Function SecureQueryParams(ByVal str_sql As String, ByVal ParamArray SubstitutionFields() As Object) As DataSet
            Return SecureQuery(str_sql, TransformParams(SubstitutionFields))
        End Function


        Function SecureScalarQuery(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable)
            Connect()
            Dim result
            Try
                result = CreateParameterisedCommand(str_sql, SubstitutionFields).ExecuteScalar()
            Finally
                InternalDisconnect()
            End Try
            Return result
        End Function


        Function SecureScalarQueryParams(ByVal str_sql As String, ByVal ParamArray SubstitutionFields() As Object)
            Return SecureScalarQuery(str_sql, TransformParams(SubstitutionFields))
        End Function


        Function SecureNonQuery(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable) As Int32
            Connect()
            Dim result
            Try
                result = CreateParameterisedCommand(str_sql, SubstitutionFields).ExecuteNonQuery()
            Finally
                InternalDisconnect()
            End Try
            Return result
        End Function

        Function SecureNonQueryParams(ByVal str_sql As String, ByVal ParamArray SubstitutionFields() As Object) As Int32
            Return SecureNonQuery(str_sql, TransformParams(SubstitutionFields))
        End Function

        Function Query(ByVal str_sql As String)

            Dim adapter As New OracleClient.OracleDataAdapter
            Dim result As New DataSet

            Connect()
            Dim cmd As OracleClient.OracleCommand = New OracleClient.OracleCommand(str_sql, conn)
            adapter.SelectCommand = cmd

            Try
                adapter.Fill(result)
            Finally
                InternalDisconnect()
            End Try

            Return result
        End Function

        Function NonQuery(ByVal str_sql As String)
            Connect()
            Dim result
            Try
                Dim cmd As OracleClient.OracleCommand = New OracleClient.OracleCommand(str_sql, conn)

                'long timeout for the transformation work.
                cmd.CommandTimeout = 500

                result = cmd.ExecuteNonQuery()

            Finally
                InternalDisconnect()
            End Try

            Return result
        End Function

        Function SecureInsertQuery(ByVal str_sql As String, ByVal SubstitutionFields As Hashtable) As Int32
            str_sql &= " SELECT @newlyInsertedId = SCOPE_IDENTITY()"

            Connect()

            Dim param As New SqlParameter("@newlyInsertedId", SqlDbType.BigInt)
            param.Direction = ParameterDirection.Output

            Dim cmd = CreateParameterisedCommand(str_sql, SubstitutionFields)
            cmd.Parameters.Add(param)

            Try
                cmd.ExecuteNonQuery()
            Finally
                InternalDisconnect()
            End Try

            If DBNull.Value.Equals(cmd.Parameters("@newlyInsertedId").Value) Then
                Return -1
            Else
                Return cmd.Parameters("@newlyInsertedId").Value
            End If
        End Function

        Function SecureInsertQueryParams(ByVal str_sql As String, ByVal ParamArray SubstitutionFields() As Object) As Int32
            Return SecureInsertQuery(str_sql, TransformParams(SubstitutionFields))
        End Function


    End Class

End Namespace
