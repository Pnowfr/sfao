<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MATUTL
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.BtnOk = New System.Windows.Forms.Button()
        Me.BtnFin = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxNom = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.MTextBoxMatr = New System.Windows.Forms.MaskedTextBox()
        Me.ComboBoxArt = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelSloF = New System.Windows.Forms.Label()
        Me.TextBoxUnité = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.LabelLotF = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.MTextBoxTotEnc = New System.Windows.Forms.MaskedTextBox()
        Me.TextBoxDesign = New System.Windows.Forms.TextBox()
        Me.TextBoxLot = New System.Windows.Forms.TextBox()
        Me.TextBoxLotF = New System.Windows.Forms.TextBox()
        Me.TextBoxSloF = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel0 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.TextBoxMsg = New System.Windows.Forms.TextBox()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel0.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnOk
        '
        Me.BtnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOk.Location = New System.Drawing.Point(48, 714)
        Me.BtnOk.Name = "BtnOk"
        Me.BtnOk.Size = New System.Drawing.Size(90, 35)
        Me.BtnOk.TabIndex = 10
        Me.BtnOk.Text = "OK"
        Me.BtnOk.UseVisualStyleBackColor = True
        '
        'BtnFin
        '
        Me.BtnFin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnFin.CausesValidation = False
        Me.BtnFin.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnFin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnFin.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnFin.Location = New System.Drawing.Point(456, 714)
        Me.BtnFin.Name = "BtnFin"
        Me.BtnFin.Size = New System.Drawing.Size(90, 35)
        Me.BtnFin.TabIndex = 11
        Me.BtnFin.TabStop = False
        Me.BtnFin.Text = "FIN"
        Me.BtnFin.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 29)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Code Matricule"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(180, 29)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Nom Opérateur"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBoxNom
        '
        Me.TextBoxNom.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxNom.Enabled = False
        Me.TextBoxNom.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxNom.Location = New System.Drawing.Point(223, 62)
        Me.TextBoxNom.Name = "TextBoxNom"
        Me.TextBoxNom.Size = New System.Drawing.Size(326, 35)
        Me.TextBoxNom.TabIndex = 2
        Me.TextBoxNom.TabStop = False
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 171)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(141, 29)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Désignation"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 118)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(143, 29)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Code article"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ErrorProvider
        '
        Me.ErrorProvider.ContainerControl = Me
        '
        'MTextBoxMatr
        '
        Me.MTextBoxMatr.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.MTextBoxMatr.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ErrorProvider.SetIconPadding(Me.MTextBoxMatr, 3)
        Me.MTextBoxMatr.Location = New System.Drawing.Point(223, 9)
        Me.MTextBoxMatr.Mask = "9999"
        Me.MTextBoxMatr.Name = "MTextBoxMatr"
        Me.MTextBoxMatr.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MTextBoxMatr.Size = New System.Drawing.Size(116, 35)
        Me.MTextBoxMatr.TabIndex = 1
        Me.MTextBoxMatr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MTextBoxMatr.ValidatingType = GetType(Integer)
        '
        'ComboBoxArt
        '
        Me.ComboBoxArt.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBoxArt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxArt.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxArt.FormattingEnabled = True
        Me.ErrorProvider.SetIconPadding(Me.ComboBoxArt, 3)
        Me.ComboBoxArt.Location = New System.Drawing.Point(223, 116)
        Me.ComboBoxArt.Name = "ComboBoxArt"
        Me.ComboBoxArt.Size = New System.Drawing.Size(180, 33)
        Me.ComboBoxArt.TabIndex = 30
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.LabelSloF, 0, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBoxArt, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxUnité, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.Label7, 0, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxNom, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxMatr, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label6, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelLotF, 0, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxTotEnc, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxDesign, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxLot, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxLotF, 1, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxSloF, 1, 8)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 9
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(552, 481)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'LabelSloF
        '
        Me.LabelSloF.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelSloF.AutoSize = True
        Me.LabelSloF.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelSloF.Location = New System.Drawing.Point(3, 438)
        Me.LabelSloF.Name = "LabelSloF"
        Me.LabelSloF.Size = New System.Drawing.Size(186, 29)
        Me.LabelSloF.TabIndex = 31
        Me.LabelSloF.Text = "N° bobine fourn."
        Me.LabelSloF.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBoxUnité
        '
        Me.TextBoxUnité.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxUnité.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxUnité.Enabled = False
        Me.TextBoxUnité.Location = New System.Drawing.Point(223, 327)
        Me.TextBoxUnité.Name = "TextBoxUnité"
        Me.TextBoxUnité.Size = New System.Drawing.Size(116, 35)
        Me.TextBoxUnité.TabIndex = 29
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(3, 330)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(69, 29)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "Unité"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(3, 277)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(103, 29)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Quantité"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelLotF
        '
        Me.LabelLotF.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelLotF.AutoSize = True
        Me.LabelLotF.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelLotF.Location = New System.Drawing.Point(3, 383)
        Me.LabelLotF.Name = "LabelLotF"
        Me.LabelLotF.Size = New System.Drawing.Size(196, 29)
        Me.LabelLotF.TabIndex = 23
        Me.LabelLotF.Text = "N° lot fournisseur"
        Me.LabelLotF.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 224)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(175, 29)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "N° lot / sous-lot"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MTextBoxTotEnc
        '
        Me.MTextBoxTotEnc.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.MTextBoxTotEnc.Location = New System.Drawing.Point(223, 274)
        Me.MTextBoxTotEnc.MaximumSize = New System.Drawing.Size(116, 35)
        Me.MTextBoxTotEnc.Name = "MTextBoxTotEnc"
        Me.MTextBoxTotEnc.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MTextBoxTotEnc.Size = New System.Drawing.Size(116, 35)
        Me.MTextBoxTotEnc.TabIndex = 24
        Me.MTextBoxTotEnc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MTextBoxTotEnc.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'TextBoxDesign
        '
        Me.TextBoxDesign.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDesign.Enabled = False
        Me.TextBoxDesign.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxDesign.Location = New System.Drawing.Point(223, 168)
        Me.TextBoxDesign.Name = "TextBoxDesign"
        Me.TextBoxDesign.Size = New System.Drawing.Size(326, 35)
        Me.TextBoxDesign.TabIndex = 27
        Me.TextBoxDesign.TabStop = False
        '
        'TextBoxLot
        '
        Me.TextBoxLot.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxLot.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxLot.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxLot.Location = New System.Drawing.Point(223, 221)
        Me.TextBoxLot.MaxLength = 20
        Me.TextBoxLot.Name = "TextBoxLot"
        Me.TextBoxLot.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxLot.TabIndex = 3
        '
        'TextBoxLotF
        '
        Me.TextBoxLotF.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxLotF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxLotF.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxLotF.Location = New System.Drawing.Point(223, 380)
        Me.TextBoxLotF.MaxLength = 20
        Me.TextBoxLotF.Name = "TextBoxLotF"
        Me.TextBoxLotF.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxLotF.TabIndex = 32
        '
        'TextBoxSloF
        '
        Me.TextBoxSloF.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxSloF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxSloF.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxSloF.Location = New System.Drawing.Point(223, 435)
        Me.TextBoxSloF.MaxLength = 20
        Me.TextBoxSloF.Name = "TextBoxSloF"
        Me.TextBoxSloF.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxSloF.TabIndex = 33
        '
        'TableLayoutPanel0
        '
        Me.TableLayoutPanel0.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel0.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.TableLayoutPanel0.ColumnCount = 1
        Me.TableLayoutPanel0.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel0.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.TableLayoutPanel0.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.TableLayoutPanel0.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel0.Name = "TableLayoutPanel0"
        Me.TableLayoutPanel0.RowCount = 2
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel0.Size = New System.Drawing.Size(560, 686)
        Me.TableLayoutPanel0.TabIndex = 19
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.TextBoxMsg, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(4, 492)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(552, 190)
        Me.TableLayoutPanel2.TabIndex = 21
        '
        'TextBoxMsg
        '
        Me.TextBoxMsg.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxMsg.CausesValidation = False
        Me.TextBoxMsg.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBoxMsg.Enabled = False
        Me.TextBoxMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxMsg.Location = New System.Drawing.Point(3, 3)
        Me.TextBoxMsg.Multiline = True
        Me.TextBoxMsg.Name = "TextBoxMsg"
        Me.TextBoxMsg.ShortcutsEnabled = False
        Me.TextBoxMsg.Size = New System.Drawing.Size(546, 184)
        Me.TextBoxMsg.TabIndex = 9
        Me.TextBoxMsg.TabStop = False
        '
        'MATUTL
        '
        Me.AcceptButton = Me.BtnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(14.0!, 29.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnFin
        Me.ClientSize = New System.Drawing.Size(584, 761)
        Me.Controls.Add(Me.TableLayoutPanel0)
        Me.Controls.Add(Me.BtnFin)
        Me.Controls.Add(Me.BtnOk)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(800, 1000)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(299, 300)
        Me.Name = "MATUTL"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fin d'opération"
        Me.TopMost = True
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel0.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BtnOk As Button
    Friend WithEvents BtnFin As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxNom As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents ErrorProvider As ErrorProvider
    Friend WithEvents MTextBoxMatr As MaskedTextBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel0 As TableLayoutPanel
    Friend WithEvents Label6 As Label
    Friend WithEvents TextBoxLot As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents LabelLotF As Label
    Friend WithEvents MTextBoxTotEnc As MaskedTextBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TextBoxMsg As TextBox
    Friend WithEvents TextBoxDesign As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBoxUnité As TextBox
    Friend WithEvents ComboBoxArt As ComboBox
    Friend WithEvents LabelSloF As Label
    Friend WithEvents TextBoxLotF As TextBox
    Friend WithEvents TextBoxSloF As TextBox
End Class
