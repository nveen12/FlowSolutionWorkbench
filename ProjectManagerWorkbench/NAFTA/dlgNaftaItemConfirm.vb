Imports System.Windows.Forms

Public Class dlgNaftaItemConfirm

    Private db As New DB.ServerDB(My.Settings.FLOWConnectionString)

    Public Sub New(ByRef dsNafta As DataSet)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Dim dc As New DataColumn("Action", System.Type.GetType("System.String"))
        dsNafta.Tables(0).Columns.Add(dc)
        ' Add any initialization after the InitializeComponent() call.
        Me.dgvDetailDisplay.DataSource = dsNafta.Tables(0)

        'Put the action information to the correct information
        For i As Integer = 0 To dgvDetailDisplay.Rows.Count - 1
            If DBNull.Value.Equals(dgvDetailDisplay.Rows(i).Cells("Qualifies").Value) Then
                dgvDetailDisplay.Rows(i).Cells("Action").Value = "Insert with preset values (Qualifies = False)"

            End If
        Next



    End Sub


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.saveChanges()

        Me.Close()
    End Sub

    Private Sub saveChanges()
        Try
            db.Connect()
       
            'Save when a change did occure
            For i As Integer = 0 To dgvDetailDisplay.Rows.Count - 1

                If dgvDetailDisplay.Rows(i).Cells("Action").Value.ToString.Contains("Update") Then
                    db.SecureNonQueryParams("UPDATE [NAFTA] " & _
                                              " SET [vendorNo]          = @1  " & _
                                              "    ,[tariff]            = @2  " & _
                                              "    ,[prefCriterion]     = @3  " & _
                                              "    ,[producer]          = @4  " & _
                                              "    ,[netCost]           = @5 " & _
                                              "    ,[countryOfOrigin]   = @6  " & _
                                              "    ,[NAFTAPart]         = @7 " & _
                                             " WHERE itemNo =  @8", dgvDetailDisplay.Rows(i).Cells("Vendor No").Value, _
                                             dgvDetailDisplay.Rows(i).Cells("Tariff").Value, dgvDetailDisplay.Rows(i).Cells("Criteria").Value, _
                                             dgvDetailDisplay.Rows(i).Cells("Producer").Value, _
                                             dgvDetailDisplay.Rows(i).Cells("Net Cost").Value, _
                                             dgvDetailDisplay.Rows(i).Cells("Country").Value, _
                                             dgvDetailDisplay.Rows(i).Cells("Qualifies").Value, dgvDetailDisplay.Rows(i).Cells("Item").Value)

                ElseIf dgvDetailDisplay.Rows(i).Cells("Action").Value.ToString.Contains("Insert") Then
                    'Insert - First make sure that the item does not exist in the table
                    Dim ds As DataSet = db.SecureQueryParams("SELECT * FROM nafta WHERE itemNo = @1", dgvDetailDisplay.Rows(i).Cells("Item").Value)
                    If ds.Tables(0).Rows.Count = 1 Then

                    Else
                        Dim bQualifies As Boolean = False
                        If Not DBNull.Value.Equals(dgvDetailDisplay.Rows(i).Cells("Qualifies").Value) Then
                            bQualifies = dgvDetailDisplay.Rows(i).Cells("Qualifies").Value
                        End If

                        db.SecureNonQueryParams(" INSERT INTO nafta " & _
                                               "([vendorNo] " & _
                                               ",[itemNo] " & _
                                               ",[tariff] " & _
                                               ",[prefCriterion] " & _
                                               ",[producer] " & _
                                               ",[netCost] " & _
                                               ",[countryOfOrigin] " & _
                                               ",[NAFTAPart]) " & _
                                               "         VALUES(@1,@2,@3,@4,@5,@6,@7,@8)", dgvDetailDisplay.Rows(i).Cells("Vendor No").Value, _
                                              dgvDetailDisplay.Rows(i).Cells("Item").Value, dgvDetailDisplay.Rows(i).Cells("Tariff").Value, dgvDetailDisplay.Rows(i).Cells("Criteria").Value, _
                                             dgvDetailDisplay.Rows(i).Cells("Producer").Value, _
                                             dgvDetailDisplay.Rows(i).Cells("Net Cost").Value, _
                                             dgvDetailDisplay.Rows(i).Cells("Country").Value, _
                                             bQualifies)

                    End If



                    'Dim ds As DataSet = db.Query("SELECT w.text1 as Item, w.text2 as [Description], w.number3 as [Pur Mfg], w.number2 as [Qty Per],w.Number8 AS Cost, n.vendorNo AS [Vendor No] ,n.tariff AS Tariff, n.prefCriterion AS [Criteria],n.netCost AS [Net Cost], n.producer AS [Producer] " & _
                    '                     " , n.countryOfOrigin AS [Country], n.NAFTAPart as Qualifies" & _
                    '                     " FROM tblWork3 w LEFT JOIN dbo.NAFTA n ON n.itemNo = w.text1 ")
                End If

            Next
        Catch ex As Exception
        Finally
            db.Disconnect()
        End Try

    End Sub


    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private bload As Boolean = True


    Private Sub dlgNaftaItemConfirm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        bload = False
    End Sub

    Private Sub dgvDetailDisplay_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles dgvDetailDisplay.CellFormatting
        'every second row will become beige
        If e.RowIndex Mod 2 = 0 Then
            e.CellStyle.BackColor = Color.Beige
        End If

        If dgvDetailDisplay.Columns(e.ColumnIndex).Name.Equals("qualifies") AndAlso DBNull.Value.Equals(e.Value) Then
            e.CellStyle.BackColor = Color.Red
        End If


    End Sub

    Private Sub dgvDetailDisplay_CellValidated(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvDetailDisplay.CellValidated
        'Need to present the action like insert when

        If Not DBNull.Value.Equals(dgvDetailDisplay.Rows(e.RowIndex).Cells("Action").Value) AndAlso Not bload Then
            If dgvDetailDisplay.Rows(e.RowIndex).Cells("Action").Value = "Insert with preset values (Qualifies = False)" Then
                dgvDetailDisplay.Rows(e.RowIndex).Cells("Action").Value = "Insert with changed values"                 'Seems to fire in the load also!
                dgvDetailDisplay.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.AliceBlue
            End If
        ElseIf Not bload AndAlso DBNull.Value.Equals(dgvDetailDisplay.Rows(e.RowIndex).Cells("Action").Value) Then
            dgvDetailDisplay.Rows(e.RowIndex).Cells("Action").Value = "Update to changed values"
            dgvDetailDisplay.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.AliceBlue
        End If


    End Sub
End Class
