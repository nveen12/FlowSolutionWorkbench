<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgComments
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgComments))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.txtComment = New System.Windows.Forms.TextBox()
        Me.lblComments = New System.Windows.Forms.Label()
        Me.txtData = New System.Windows.Forms.TextBox()
        Me.cbWarehouseUsers = New System.Windows.Forms.ComboBox()
        Me.cbDirect = New System.Windows.Forms.CheckBox()
        Me.cbAssign = New System.Windows.Forms.CheckBox()
        Me.btnDeassign = New System.Windows.Forms.Button()
        Me.rtbText = New System.Windows.Forms.RichTextBox()
        Me.cbxActions = New System.Windows.Forms.ComboBox()
        Me.lblAction = New System.Windows.Forms.Label()
        Me.ll1 = New System.Windows.Forms.LinkLabel()
        Me.ll2 = New System.Windows.Forms.LinkLabel()
        Me.ll3 = New System.Windows.Forms.LinkLabel()
        Me.btnAttachDoc = New System.Windows.Forms.Button()
        Me.ofdAttachements = New System.Windows.Forms.OpenFileDialog()
        Me.btnMail = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.pf1 = New Microsoft.VisualBasic.PowerPacks.Printing.PrintForm(Me.components)
        Me.cbxTypeOfIssue = New System.Windows.Forms.ComboBox()
        Me.txtTypeOfIssue = New System.Windows.Forms.TextBox()
        Me.txtIssueRCA = New System.Windows.Forms.TextBox()
        Me.lblIssue = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(521, 540)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(195, 36)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 4)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(89, 28)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(101, 4)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(89, 28)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'txtComment
        '
        Me.txtComment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtComment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.txtComment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.txtComment.Location = New System.Drawing.Point(20, 395)
        Me.txtComment.Margin = New System.Windows.Forms.Padding(4)
        Me.txtComment.Name = "txtComment"
        Me.txtComment.Size = New System.Drawing.Size(691, 22)
        Me.txtComment.TabIndex = 1
        '
        'lblComments
        '
        Me.lblComments.AutoSize = True
        Me.lblComments.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblComments.Location = New System.Drawing.Point(0, 0)
        Me.lblComments.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblComments.Name = "lblComments"
        Me.lblComments.Size = New System.Drawing.Size(0, 17)
        Me.lblComments.TabIndex = 2
        '
        'txtData
        '
        Me.txtData.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtData.BackColor = System.Drawing.SystemColors.Control
        Me.txtData.Location = New System.Drawing.Point(17, 15)
        Me.txtData.Margin = New System.Windows.Forms.Padding(4)
        Me.txtData.Multiline = True
        Me.txtData.Name = "txtData"
        Me.txtData.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtData.Size = New System.Drawing.Size(116, 372)
        Me.txtData.TabIndex = 3
        Me.txtData.Visible = False
        '
        'cbWarehouseUsers
        '
        Me.cbWarehouseUsers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cbWarehouseUsers.FormattingEnabled = True
        Me.cbWarehouseUsers.Location = New System.Drawing.Point(99, 423)
        Me.cbWarehouseUsers.Margin = New System.Windows.Forms.Padding(4)
        Me.cbWarehouseUsers.Name = "cbWarehouseUsers"
        Me.cbWarehouseUsers.Size = New System.Drawing.Size(435, 23)
        Me.cbWarehouseUsers.TabIndex = 6
        Me.cbWarehouseUsers.Visible = False
        '
        'cbDirect
        '
        Me.cbDirect.AutoSize = True
        Me.cbDirect.Location = New System.Drawing.Point(20, 427)
        Me.cbDirect.Margin = New System.Windows.Forms.Padding(4)
        Me.cbDirect.Name = "cbDirect"
        Me.cbDirect.Size = New System.Drawing.Size(66, 21)
        Me.cbDirect.TabIndex = 5
        Me.cbDirect.Text = "Notify"
        Me.cbDirect.UseVisualStyleBackColor = True
        '
        'cbAssign
        '
        Me.cbAssign.AutoSize = True
        Me.cbAssign.Location = New System.Drawing.Point(161, 533)
        Me.cbAssign.Margin = New System.Windows.Forms.Padding(4)
        Me.cbAssign.Name = "cbAssign"
        Me.cbAssign.Size = New System.Drawing.Size(72, 21)
        Me.cbAssign.TabIndex = 6
        Me.cbAssign.Text = "Assign"
        Me.cbAssign.UseVisualStyleBackColor = True
        Me.cbAssign.Visible = False
        '
        'btnDeassign
        '
        Me.btnDeassign.Location = New System.Drawing.Point(543, 422)
        Me.btnDeassign.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDeassign.Name = "btnDeassign"
        Me.btnDeassign.Size = New System.Drawing.Size(85, 28)
        Me.btnDeassign.TabIndex = 7
        Me.btnDeassign.Text = "Reply"
        Me.btnDeassign.UseVisualStyleBackColor = True
        '
        'rtbText
        '
        Me.rtbText.BackColor = System.Drawing.SystemColors.Control
        Me.rtbText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.rtbText.Location = New System.Drawing.Point(20, 15)
        Me.rtbText.Margin = New System.Windows.Forms.Padding(4)
        Me.rtbText.Name = "rtbText"
        Me.rtbText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.rtbText.Size = New System.Drawing.Size(707, 372)
        Me.rtbText.TabIndex = 8
        Me.rtbText.Text = ""
        '
        'cbxActions
        '
        Me.cbxActions.BackColor = System.Drawing.SystemColors.Control
        Me.cbxActions.FormattingEnabled = True
        Me.cbxActions.Items.AddRange(New Object() {"HERE  - Part should be received, Item had Proof of Delivery (POD) from supplier s" &
                "howing the item has arrived .", "THERE  - Parts are at the Subcontractor (via OSP job) and the packing slip needs " &
                "to be processed by Receiving.", "ETA  - (Estimated Time of Arrival) Used by purchasing to notify all of anticipate" &
                "d arrival at the site.", "W/S  - (Will Ship)  Used by purchasing to notify all of anticipated shipment from" &
                " supplier/manufacturer.", "W/A  - (Will Advise) Used by all parties to indicate issue is being worked on.", "QTY  - Used by Receiving department when quantity on the PO needs to be increased" &
                " to receive the items.  Used when the number of items shipped from the manufactu" &
                "rer exceed the PO quantity.", "CLOSE SHORT  - Used to indicate a PO received with an amount less than what was o" &
                "rdered.  To be entered when pcs delivered from manufacturer are less than ordere" &
                "d quantity.", "STUCK  - Used by Receiving department for PO's that cannot be received.", "UNSTUCK  - Used by Buyer to indicate PO issue is resolved and the PO is ready to " &
                "be recieved.", "SHIPPED  - Used by Buyer to indicate items that have left manufacturer/supplier a" &
                "nd will soon be delivered to the site.", "NEED POD  - Used to indicate Proof of Delivery is needed.  May be requested by ei" &
                "ther Receiving to Purchasing or vise-versa.", "ON HOLD - Indicates an item that should not be delivered to the site due to engin" &
                "eering or buyer related issues.  If material arrives at the site in this state, " &
                "buyer should be notified immediately.", "CUTDOWN - Aftermarket order sent to cutdown cell", "PAINT - Aftermarket order sent to paint booth", "MQCP DONE - Quality department indicating that the Quality Control Plan has been " &
                "created.", "RC0  MQCP is missing", "RC0  Technically unclear", "RC0  Received order endorsement", "RC0  Await order endorsement", "RC0  Document is for customer approval", "RC0  Awaiting Customer Ok.", "RC0  Awaiting supplier information", "RC0  Awaiting Kick Off Meeting", "RC0  Order on hold", "RC0  R&D JOB", "RC1 DEMAND REVIEW - Indicates that the demand for this order is not identified.", "RC1 TECHNICAL UNCLARITY - Indicates that this item is not technical clear.", "RC1 TECHNICAL REVIEW WITH SUPPLIER - Indicates that we are reviewing this item wi" &
                "th the supplier.", "RC1 RFQ PHASE WITH SUPPLIER - Indicates that we are requesting quotes.", "RC1 USE OF ALTERNATIVE COMPONENT - Indicates that we are switching material for t" &
                "his order.", "RC1 OTHER - Please explain carefully the current status of the order.", "BOM COMPLETED - Indicates that engineering has defined all components for this it" &
                "em.", "BOM POSTPONED - Indicates that engineering has posponed the completion date of th" &
                "e BOM.", "Production Stator - Anwenden, wenn der Stator gewickelt wurde", "Production Wickeln - Anwenden, wenn die Wicklung erledigt wurde", "Production Schw Dre - Anwenden, wenn Schweißen und Drehen erledigt wurde", "Production Schalten - Anwenden, wenn der Motor geschaltet wurde", "Production Motor - Anwenden, wenn der Motor montiert wurde", "Production Pumpe - Anwenden, wenn die Pumpe montiert wurde", "Production Prüfen - Anwenden, wenn geprüft wurde", "Production Konservieren - Anwenden, wenn konserviert wurde", "Production Fertig PL 6 Zoll - Anwenden, wenn eine 6 Zoll Einheit gefertigt wurde", "Production Fertig PL > 6 Zoll - Anwenden, wenn eine Einheit größer 6 Zoll geferti" &
                "gt wurde", "Production Fertig BJ - Anwenden, wenn eine Byron Jackson Einheit gefertigt wurde"})
        Me.cbxActions.Location = New System.Drawing.Point(99, 454)
        Me.cbxActions.Margin = New System.Windows.Forms.Padding(4)
        Me.cbxActions.MaxDropDownItems = 18
        Me.cbxActions.Name = "cbxActions"
        Me.cbxActions.Size = New System.Drawing.Size(612, 24)
        Me.cbxActions.TabIndex = 9
        '
        'lblAction
        '
        Me.lblAction.AutoSize = True
        Me.lblAction.Location = New System.Drawing.Point(16, 464)
        Me.lblAction.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAction.Name = "lblAction"
        Me.lblAction.Size = New System.Drawing.Size(47, 17)
        Me.lblAction.TabIndex = 10
        Me.lblAction.Text = "Action"
        '
        'll1
        '
        Me.ll1.AutoSize = True
        Me.ll1.Location = New System.Drawing.Point(187, 554)
        Me.ll1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ll1.Name = "ll1"
        Me.ll1.Size = New System.Drawing.Size(77, 17)
        Me.ll1.TabIndex = 11
        Me.ll1.TabStop = True
        Me.ll1.Text = "LinkLabel1"
        Me.ll1.Visible = False
        '
        'll2
        '
        Me.ll2.AutoSize = True
        Me.ll2.Location = New System.Drawing.Point(289, 554)
        Me.ll2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ll2.Name = "ll2"
        Me.ll2.Size = New System.Drawing.Size(77, 17)
        Me.ll2.TabIndex = 12
        Me.ll2.TabStop = True
        Me.ll2.Text = "LinkLabel2"
        Me.ll2.Visible = False
        '
        'll3
        '
        Me.ll3.AutoSize = True
        Me.ll3.Location = New System.Drawing.Point(393, 554)
        Me.ll3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ll3.Name = "ll3"
        Me.ll3.Size = New System.Drawing.Size(77, 17)
        Me.ll3.TabIndex = 13
        Me.ll3.TabStop = True
        Me.ll3.Text = "LinkLabel3"
        Me.ll3.Visible = False
        '
        'btnAttachDoc
        '
        Me.btnAttachDoc.Location = New System.Drawing.Point(99, 548)
        Me.btnAttachDoc.Margin = New System.Windows.Forms.Padding(4)
        Me.btnAttachDoc.Name = "btnAttachDoc"
        Me.btnAttachDoc.Size = New System.Drawing.Size(75, 28)
        Me.btnAttachDoc.TabIndex = 14
        Me.btnAttachDoc.Text = "Attach"
        Me.btnAttachDoc.UseVisualStyleBackColor = True
        '
        'ofdAttachements
        '
        Me.ofdAttachements.FileName = "OpenFileDialog1"
        '
        'btnMail
        '
        Me.btnMail.Location = New System.Drawing.Point(631, 422)
        Me.btnMail.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMail.Name = "btnMail"
        Me.btnMail.Size = New System.Drawing.Size(85, 28)
        Me.btnMail.TabIndex = 15
        Me.btnMail.Text = "Draft Mail"
        Me.btnMail.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnPrint.Location = New System.Drawing.Point(1, 548)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(89, 28)
        Me.btnPrint.TabIndex = 16
        Me.btnPrint.Text = "Print"
        '
        'pf1
        '
        Me.pf1.DocumentName = "document"
        Me.pf1.Form = Me
        Me.pf1.PrintAction = System.Drawing.Printing.PrintAction.PrintToPrinter
        Me.pf1.PrinterSettings = CType(resources.GetObject("pf1.PrinterSettings"), System.Drawing.Printing.PrinterSettings)
        Me.pf1.PrintFileName = Nothing
        '
        'cbxTypeOfIssue
        '
        Me.cbxTypeOfIssue.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.cbxTypeOfIssue.FormattingEnabled = True
        Me.cbxTypeOfIssue.Location = New System.Drawing.Point(99, 500)
        Me.cbxTypeOfIssue.Margin = New System.Windows.Forms.Padding(4)
        Me.cbxTypeOfIssue.Name = "cbxTypeOfIssue"
        Me.cbxTypeOfIssue.Size = New System.Drawing.Size(184, 24)
        Me.cbxTypeOfIssue.TabIndex = 17
        Me.cbxTypeOfIssue.Visible = False
        '
        'txtTypeOfIssue
        '
        Me.txtTypeOfIssue.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.txtTypeOfIssue.Location = New System.Drawing.Point(293, 500)
        Me.txtTypeOfIssue.Margin = New System.Windows.Forms.Padding(4)
        Me.txtTypeOfIssue.Name = "txtTypeOfIssue"
        Me.txtTypeOfIssue.Size = New System.Drawing.Size(52, 22)
        Me.txtTypeOfIssue.TabIndex = 18
        Me.txtTypeOfIssue.Visible = False
        '
        'txtIssueRCA
        '
        Me.txtIssueRCA.BackColor = System.Drawing.SystemColors.MenuHighlight
        Me.txtIssueRCA.Location = New System.Drawing.Point(355, 500)
        Me.txtIssueRCA.Margin = New System.Windows.Forms.Padding(4)
        Me.txtIssueRCA.Multiline = True
        Me.txtIssueRCA.Name = "txtIssueRCA"
        Me.txtIssueRCA.Size = New System.Drawing.Size(356, 24)
        Me.txtIssueRCA.TabIndex = 19
        Me.txtIssueRCA.Visible = False
        '
        'lblIssue
        '
        Me.lblIssue.AutoSize = True
        Me.lblIssue.Location = New System.Drawing.Point(16, 503)
        Me.lblIssue.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblIssue.Name = "lblIssue"
        Me.lblIssue.Size = New System.Drawing.Size(41, 17)
        Me.lblIssue.TabIndex = 20
        Me.lblIssue.Text = "Issue"
        Me.lblIssue.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(95, 480)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 17)
        Me.Label1.TabIndex = 21
        Me.Label1.Text = "Type"
        Me.Label1.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(289, 480)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 17)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Duration"
        Me.Label2.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(351, 480)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 17)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Comment"
        Me.Label3.Visible = False
        '
        'dlgComments
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(732, 580)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblIssue)
        Me.Controls.Add(Me.txtIssueRCA)
        Me.Controls.Add(Me.txtTypeOfIssue)
        Me.Controls.Add(Me.cbxTypeOfIssue)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnMail)
        Me.Controls.Add(Me.btnAttachDoc)
        Me.Controls.Add(Me.ll3)
        Me.Controls.Add(Me.ll2)
        Me.Controls.Add(Me.ll1)
        Me.Controls.Add(Me.lblAction)
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
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgComments"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Comments"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents txtComment As System.Windows.Forms.TextBox
    Friend WithEvents lblComments As System.Windows.Forms.Label
    Friend WithEvents txtData As System.Windows.Forms.TextBox
    Friend WithEvents cbWarehouseUsers As System.Windows.Forms.ComboBox
    Friend WithEvents cbDirect As System.Windows.Forms.CheckBox
    Friend WithEvents cbAssign As System.Windows.Forms.CheckBox
    Friend WithEvents btnDeassign As System.Windows.Forms.Button
    Friend WithEvents rtbText As System.Windows.Forms.RichTextBox
    Friend WithEvents cbxActions As System.Windows.Forms.ComboBox
    Friend WithEvents lblAction As System.Windows.Forms.Label
    Friend WithEvents ll1 As System.Windows.Forms.LinkLabel
    Friend WithEvents ll2 As System.Windows.Forms.LinkLabel
    Friend WithEvents ll3 As System.Windows.Forms.LinkLabel
    Friend WithEvents btnAttachDoc As System.Windows.Forms.Button
    Friend WithEvents ofdAttachements As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnMail As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents pf1 As Microsoft.VisualBasic.PowerPacks.Printing.PrintForm
    Friend WithEvents lblIssue As System.Windows.Forms.Label
    Friend WithEvents txtIssueRCA As System.Windows.Forms.TextBox
    Friend WithEvents txtTypeOfIssue As System.Windows.Forms.TextBox
    Friend WithEvents cbxTypeOfIssue As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
