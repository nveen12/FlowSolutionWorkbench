Imports System.Windows.Forms

Public Class dlgOutOfOfficeTime
    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If dtpStartDate.Value.DayOfYear > dtpReturnDate.Value.DayOfYear AndAlso dtpReturnDate.Value.Year = dtpStartDate.Value.Year Then
            MessageBox.Show("Please enter a start date before or equal the return date.")
            Exit Sub
        Else
            db.SecureNonQueryParams("UPDATE warehouse_users SET outOfOffice = 1, outUnitilDate = @2, outFromDate = @3 WHERE user_name = @1", Environment.UserName, dtpReturnDate.Value, dtpStartDate.Value)
        End If


        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgOutOfOfficeTime_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dtpReturnDate.MinDate = Today
        dtpStartDate.MinDate = Today

        Try

        
            Dim ds As DataSet = db.SecureQueryParams("SELECT outOfOffice ,outUnitilDate, outFromDate  FROM warehouse_users WHERE user_name = @1", Environment.UserName)
            If Not DBNull.Value.Equals(ds.Tables(0).Rows(0).Item("outOfOffice")) AndAlso ds.Tables(0).Rows(0).Item("outOfOffice") Then
                lblCurrentState.Text = "Enabled"
                dtpReturnDate.Value = ds.Tables(0).Rows(0).Item("outUnitilDate")
                dtpStartDate.Value = ds.Tables(0).Rows(0).Item("outFromDate")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnDisable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisable.Click
        db.SecureNonQueryParams("UPDATE warehouse_users SET outOfOffice = NULL, outUnitilDate = NULL, outFromDate = NULL WHERE user_name = @1", Environment.UserName)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class
