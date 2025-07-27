<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ucDiscussion
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ucDiscussion))
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnMail = New System.Windows.Forms.Button
        Me.btnAttachDoc = New System.Windows.Forms.Button
        Me.ll3 = New System.Windows.Forms.LinkLabel
        Me.ll1 = New System.Windows.Forms.LinkLabel
        Me.lblAction = New System.Windows.Forms.Label
        Me.ofdAttachements = New System.Windows.Forms.OpenFileDialog
        Me.ll2 = New System.Windows.Forms.LinkLabel
        Me.cbxActions = New System.Windows.Forms.ComboBox
        Me.rtbText = New System.Windows.Forms.RichTextBox
        Me.OK_Button = New System.Windows.Forms.Button
        Me.btnDeassign = New System.Windows.Forms.Button
        Me.pf1 = New Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(Me.components)
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.cbAssign = New System.Windows.Forms.CheckBox
        Me.cbDirect = New System.Windows.Forms.CheckBox
        Me.cbWarehouseUsers = New System.Windows.Forms.ComboBox
        Me.txtData = New System.Windows.Forms.TextBox
        Me.lblComments = New System.Windows.Forms.Label
        Me.txtComment = New System.Windows.Forms.TextBox
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnPrint.Location = New System.Drawing.Point(1, 431)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(67, 23)
        Me.btnPrint.TabIndex = 33
        Me.btnPrint.Text = "Print"
        '
        'btnMail
        '
        Me.btnMail.Location = New System.Drawing.Point(473, 379)
        Me.btnMail.Name = "btnMail"
        Me.btnMail.Size = New System.Drawing.Size(64, 23)
        Me.btnMail.TabIndex = 32
        Me.btnMail.Text = "Draft Mail"
        Me.btnMail.UseVisualStyleBackColor = True
        '
        'btnAttachDoc
        '
        Me.btnAttachDoc.Location = New System.Drawing.Point(74, 432)
        Me.btnAttachDoc.Name = "btnAttachDoc"
        Me.btnAttachDoc.Size = New System.Drawing.Size(56, 23)
        Me.btnAttachDoc.TabIndex = 31
        Me.btnAttachDoc.Text = "Attach"
        Me.btnAttachDoc.UseVisualStyleBackColor = True
        '
        'll3
        '
        Me.ll3.AutoSize = True
        Me.ll3.Location = New System.Drawing.Point(309, 438)
        Me.ll3.Name = "ll3"
        Me.ll3.Size = New System.Drawing.Size(59, 13)
        Me.ll3.TabIndex = 30
        Me.ll3.TabStop = True
        Me.ll3.Text = "LinkLabel3"
        Me.ll3.Visible = False
        '
        'll1
        '
        Me.ll1.AutoSize = True
        Me.ll1.Location = New System.Drawing.Point(154, 438)
        Me.ll1.Name = "ll1"
        Me.ll1.Size = New System.Drawing.Size(59, 13)
        Me.ll1.TabIndex = 28
        Me.ll1.TabStop = True
        Me.ll1.Text = "LinkLabel1"
        Me.ll1.Visible = False
        '
        'lblAction
        '
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(12, 413)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(37, 13)
        Me.lblAction.TabIndex = 27
        Me.lblAction.Text = "Action"
        '
        'ofdAttachements
        '
        Me.ofdAttachements.FileName = "OpenFileDialog1"
        '
        'll2
        '
        Me.ll2.AutoSize = True
        Me.ll2.Location = New System.Drawing.Point(231, 438)
        Me.ll2.Name = "ll2"
        Me.ll2.Size = New System.Drawing.Size(59, 13)
        Me.ll2.TabIndex = 29
        Me.ll2.TabStop = True
        Me.ll2.Text = "LinkLabel2"
        Me.ll2.Visible = False
        '
        'cbxActions
        '
        Me.cbxActions.BackColor = System.Drawing.SystemColors.Control
        Me.cbxActions.FormattingEnabled = True
        Me.cbxActions.Items.AddRange(New Object() {"HERE  - Part should be received, Item had Proof of Delivery (POD) from supplier s" & _
                        "howing the item has arrived .", "THERE  - Parts are at the Subcontractor (via OSP job) and the packing slip needs " & _
                        "to be processed by Receiving.", "ETA  - (Estimated Time of Arrival) Used by purchasing to notify all of anticipate" & _
                        "d arrival at the site.", "W/S  - (Will Ship)  Used by purchasing to notify all of anticipated shipment from" & _
                        " supplier/manufacturer.", "W/A  - (Will Advise) Used by all parties to indicate issue is being worked on.", "QTY  - Used by Receiving department when quantity on the PO needs to be increased" & _
                        " to receive the items.  Used when the number of items shipped from the manufactu" & _
                        "rer exceed the PO quantity.", "CLOSE SHORT  - Used to indicate a PO received with an amount less than what was o" & _
                        "rdered.  To be entered when pcs delivered from manufacturer are less than ordere" & _
                        "d quantity.", "STUCK  - Used by Receiving department for PO's that cannot be received.", "UNSTUCK  - Used by Buyer to indicate PO issue is resolved and the PO is ready to " & _
                        "be recieved.", "SHIPPED  - Used by Buyer to indicate items that have left manufacturer/supplier a" & _
                        "nd will soon be delivered to the site.", "NEED POD  - Used to indicate Proof of Delivery is needed.  May be requested by ei" & _
                        "ther Receiving to Purchasing or vise-versa.", "ON HOLD - Indicates an item that should not be delivered to the site due to engin" & _
                        "eering or buyer related issues.  If material arrives at the site in this state, " & _
                        "buyer should be notified immediately.", "CUTDOWN - Aftermarket order sent to cutdown cell", "PAINT - Aftermarket order sent to paint booth", "MQCP DONE - Quality department indicating that the Quality Control Plan has been " & _
                        "created.", "RC0  MQCP is missing", "RC0  Technically unclear", "RC0  Received order endorsement", "RC0  Await order endorsement", "RC0  Document is for customer approval", "RC0  Awaiting Customer Ok.", "RC0  Awaiting supplier information", "RC0  Awaiting Kick Off Meeting", "RC0  Order on hold", "RC0  R&D JOB", "RC1 DEMAND REVIEW - Indicates that the demand for this order is not identified.", "RC1 TECHNICAL UNCLARITY - Indicates that this item is not technical clear.", "RC1 TECHNICAL REVIEW WITH SUPPLIER - Indicates that we are reviewing this item wi" & _
                        "th the supplier.", "RC1 RFQ PHASE WITH SUPPLIER - Indicates that we are requesting quotes.", "RC1 USE OF ALTERNATIVE COMPONENT - Indicates that we are switching material for t" & _
                        "his order.", "RC1 OTHER - Please explain carefully the current status of the order.", "BOM COMPLETED - Indicates that engineering has defined all components for this it" & _
                        "em.", "BOM POSTPONED - Indicates that engineering has posponed the completion date of th" & _
                        "e BOM.", "Production Stator - Anwenden, wenn der Stator gewickelt wurde", "Production Wickeln - Anwenden, wenn die Wicklung erledigt wurde", "Production Schw Dre - Anwenden, wenn Schweißen und Drehen erledigt wurde", "Production Schalten - Anwenden, wenn der Motor geschaltet wurde", "Production Motor - Anwenden, wenn der Motor montiert wurde", "Production Pumpe - Anwenden, wenn die Pumpe montiert wurde", "Production Prüfen - Anwenden, wenn geprüft wurde", "Production Konservieren - Anwenden, wenn konserviert wurde", "Production Fertig PL 6 Zoll - Anwenden, wenn eine 6 Zoll Einheit gefertigt wurde", "Production Fertig PL > 6 Zoll - Anwenden, wenn eine Einheit größer 6 Zoll geferti" & _
                        "gt wurde", "Production Fertig BJ - Anwenden, wenn eine Byron Jackson Einheit gefertigt wurde"})
        Me.cbxActions.Location = New System.Drawing.Point(74, 405)
        Me.cbxActions.MaxDropDownItems = 18
        Me.cbxActions.Name = "cbxActions"
        Me.cbxActions.Size = New System.Drawing.Size(460, 21)
        Me.cbxActions.TabIndex = 26
        '
        'rtbText
        '
        Me.rtbText.BackColor = System.Drawing.SystemColors.Control
        Me.rtbText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtbText.Location = New System.Drawing.Point(0, 0)
        Me.rtbText.Name = "rtbText"
        Me.rtbText.ReadOnly = True
        Me.rtbText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.rtbText.Size = New System.Drawing.Size(552, 351)
        Me.rtbText.TabIndex = 25
        Me.rtbText.Text = ""
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'btnDeassign
        '
        Me.btnDeassign.Location = New System.Drawing.Point(407, 379)
        Me.btnDeassign.Name = "btnDeassign"
        Me.btnDeassign.Size = New System.Drawing.Size(64, 23)
        Me.btnDeassign.TabIndex = 24
        Me.btnDeassign.Text = "Reply"
        Me.btnDeassign.UseVisualStyleBackColor = True
        '
        'pf1
        '
        Me.pf1.DocumentName = "document"
        Me.pf1.Form = Nothing
        Me.pf1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter
        Me.pf1.PrinterSettings = CType(resources.GetObject("pf1.PrinterSettings"), System.Drawing.Printing.PrinterSettings)
        Me.pf1.PrintFileName = Nothing
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.Visible = False
        '
        'cbAssign
        '
        Me.cbAssign.AutoSize = True
        Me.cbAssign.Location = New System.Drawing.Point(122, 443)
        Me.cbAssign.Name = "cbAssign"
        Me.cbAssign.Size = New System.Drawing.Size(57, 17)
        Me.cbAssign.TabIndex = 22
        Me.cbAssign.Text = "Assign"
        Me.cbAssign.UseVisualStyleBackColor = True
        Me.cbAssign.Visible = False
        '
        'cbDirect
        '
        Me.cbDirect.AutoSize = True
        Me.cbDirect.Location = New System.Drawing.Point(15, 383)
        Me.cbDirect.Name = "cbDirect"
        Me.cbDirect.Size = New System.Drawing.Size(53, 17)
        Me.cbDirect.TabIndex = 21
        Me.cbDirect.Text = "Notify"
        Me.cbDirect.UseVisualStyleBackColor = True
        '
        'cbWarehouseUsers
        '
        Me.cbWarehouseUsers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbWarehouseUsers.FormattingEnabled = True
        Me.cbWarehouseUsers.Location = New System.Drawing.Point(74, 380)
        Me.cbWarehouseUsers.Name = "cbWarehouseUsers"
        Me.cbWarehouseUsers.Size = New System.Drawing.Size(327, 21)
        Me.cbWarehouseUsers.TabIndex = 23
        Me.cbWarehouseUsers.Visible = False
        '
        'txtData
        '
        Me.txtData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtData.BackColor = System.Drawing.SystemColors.Control
        Me.txtData.Location = New System.Drawing.Point(13, 48)
        Me.txtData.Multiline = True
        Me.txtData.Name = "txtData"
        Me.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtData.Size = New System.Drawing.Size(88, 303)
        Me.txtData.TabIndex = 20
        Me.txtData.Visible = False
        '
        'lblComments
        '
        Me.lblComments.AutoSize = True
        Me.lblComments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblComments.Location = New System.Drawing.Point(0, 0)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.Size = New System.Drawing.Size(0, 13)
        Me.lblComments.TabIndex = 19
        '
        'txtComment
        '
        Me.txtComment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtComment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.txtComment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.txtComment.Location = New System.Drawing.Point(15, 357)
        Me.txtComment.Name = "txtComment"
        Me.txtComment.Size = New System.Drawing.Size(519, 20)
        Me.txtComment.TabIndex = 18
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(391, 431)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 17
        '
        'ucDiscussion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnMail)
        Me.Controls.Add(Me.btnAttachDoc)
        Me.Controls.Add(Me.ll3)
        Me.Controls.Add(Me.ll1)
        Me.Controls.Add(Me.lblAction)
        Me.Controls.Add(Me.ll2)
        Me.Controls.Add(Me.cbxActions)
        Me.Controls.Add(Me.rtbText)
        Me.Controls.Add(Me.btnDeassign)
        Me.Controls.Add(Me.cbAssign)
        Me.Controls.Add(Me.cbDirect)
        Me.Controls.Add(Me.cbWarehouseUsers)
        Me.Controls.Add(Me.txtData)
        Me.Controls.Add(Me.lblComments)
        Me.Controls.Add(Me.txtComment)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "ucDiscussion"
        Me.Size = New System.Drawing.Size(555, 459)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnMail As System.Windows.Forms.Button
    Friend WithEvents btnAttachDoc As System.Windows.Forms.Button
    Friend WithEvents ll3 As System.Windows.Forms.LinkLabel
    Friend WithEvents ll1 As System.Windows.Forms.LinkLabel
    Friend WithEvents lblAction As System.Windows.Forms.Label
    Friend WithEvents ofdAttachements As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ll2 As System.Windows.Forms.LinkLabel
    Friend WithEvents cbxActions As System.Windows.Forms.ComboBox
    Friend WithEvents rtbText As System.Windows.Forms.RichTextBox
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents btnDeassign As System.Windows.Forms.Button
    Friend WithEvents pf1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents cbAssign As System.Windows.Forms.CheckBox
    Friend WithEvents cbDirect As System.Windows.Forms.CheckBox
    Friend WithEvents cbWarehouseUsers As System.Windows.Forms.ComboBox
    Friend WithEvents txtData As System.Windows.Forms.TextBox
    Friend WithEvents lblComments As System.Windows.Forms.Label
    Friend WithEvents txtComment As System.Windows.Forms.TextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel

End Class
