Public Class clsExceptionManagement

    Private DB As New DB.ServerDB(My.Settings("FlowConnectionString"))
    ''' <summary>
    ''' Creates an error stamp in the backend.
    ''' </summary>
    ''' <param name="procedureName"></param>
    ''' <param name="exceptionMessage"></param>
    ''' <param name="currentStep" >The step of the program</param>
    ''' <remarks>We save the error line, the description and the object which caused the error</remarks>
    Public Sub createErrorLog(ByVal procedureName As String, ByVal exceptionMessage As Exception, ByVal currentStep As Integer, Optional ByVal additionalInformaiton As String = "Nothing")
        Try

            Dim invokedBy As String = Environment.UserName

            'Sometimes no inner exception exists and this may crash the program
            DB.SecureScalarQueryParams("INSERT INTO [error_log] " & _
               "([calling_object] " & _
               ",[description] " & _
               ",[error_line] " & _
               ",[NO_OF_STEPS] " & _
               ",[PROCEDURE_OR_FUNCTION] " & _
               ",[invoked_by] " & _
               ",[CREATION_DATE] " & _
               ",[causing_object] )" & _
                " VALUES(@1, @2, @3, @4, @5, @6, getDate(), @7)", MyBase.ToString.Replace("'", ""), exceptionMessage.Message.Replace("'", ""), exceptionMessage.StackTrace.Replace("'", ""), _
                currentStep, procedureName _
                , invokedBy, exceptionMessage.Source.Replace("'", ""))

        Catch ex As Exception
            EventLog.WriteEntry(ex.Source, ex.Message, EventLogEntryType.Error)
        End Try

    End Sub

    Public Sub createErrorLogAndMail(ByVal procedureName As String, ByVal exceptionMessage As Exception, ByVal currentStep As Integer, Optional ByVal additionalInformaiton As String = "Nothing")
        Try
            Dim invokedBy As String = Environment.UserName

            DB.SecureScalarQueryParams("INSERT INTO [error_log] " & _
               "([calling_object] " & _
               ",[description] " & _
               ",[error_line] " & _
               ",[NO_OF_STEPS] " & _
               ",[PROCEDURE_OR_FUNCTION] " & _
               ",[invoked_by] " & _
               ",[CREATION_DATE] " & _
               ",[causing_object], [inner_Exception] )" & _
                " VALUES(@1, @2, @3, @4, @5, @6, getDate(), @7, @8)", MyBase.ToString.Replace("'", ""), exceptionMessage.Message.Replace("'", ""), exceptionMessage.StackTrace.Replace("'", ""), _
                currentStep, procedureName _
                , invokedBy, exceptionMessage.Source.Replace("'", ""), "")

            Dim mailObject As New clsEMailReports("255")
            mailObject.mailErrorReport(exceptionMessage.Message, exceptionMessage.StackTrace, currentStep, procedureName, invokedBy, "", exceptionMessage.Source, "")

        Catch ex As Exception
            EventLog.WriteEntry(ex.Source, ex.Message, EventLogEntryType.Error)
        End Try

    End Sub
End Class
