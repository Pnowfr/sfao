<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CONSO
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
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.MTextBoxMatr = New System.Windows.Forms.MaskedTextBox()
        Me.ComboBoxSup = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelSup = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.MTextBoxQté = New System.Windows.Forms.MaskedTextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextBoxUnité = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TextBoxLotF = New System.Windows.Forms.TextBox()
        Me.TextBoxTrk = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxLot = New System.Windows.Forms.TextBox()
        Me.TextBoxArt = New System.Windows.Forms.TextBox()
        Me.TextBoxDesign = New System.Windows.Forms.TextBox()
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
        Me.Label1.Location = New System.Drawing.Point(3, 9)
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
        Me.Label2.Location = New System.Drawing.Point(3, 57)
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
        Me.TextBoxNom.Location = New System.Drawing.Point(223, 54)
        Me.TextBoxNom.Name = "TextBoxNom"
        Me.TextBoxNom.Size = New System.Drawing.Size(326, 35)
        Me.TextBoxNom.TabIndex = 2
        Me.TextBoxNom.TabStop = False
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 201)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(141, 29)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Désignation"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 153)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(143, 29)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Code article"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        Me.MTextBoxMatr.Location = New System.Drawing.Point(223, 6)
        Me.MTextBoxMatr.Mask = "9999"
        Me.MTextBoxMatr.Name = "MTextBoxMatr"
        Me.MTextBoxMatr.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MTextBoxMatr.Size = New System.Drawing.Size(116, 35)
        Me.MTextBoxMatr.TabIndex = 1
        Me.MTextBoxMatr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MTextBoxMatr.ValidatingType = GetType(Integer)
        '
        'ComboBoxSup
        '
        Me.ComboBoxSup.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBoxSup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSup.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxSup.FormattingEnabled = True
        Me.ErrorProvider.SetIconPadding(Me.ComboBoxSup, 3)
        Me.ComboBoxSup.Location = New System.Drawing.Point(223, 247)
        Me.ComboBoxSup.Name = "ComboBoxSup"
        Me.ComboBoxSup.Size = New System.Drawing.Size(116, 33)
        Me.ComboBoxSup.TabIndex = 43
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBoxSup, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelSup, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.Label12, 0, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxNom, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxMatr, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label8, 0, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxQté, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.Label9, 0, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxUnité, 1, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.Label11, 0, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxLotF, 1, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxTrk, 1, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxLot, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxArt, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxDesign, 1, 4)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 10
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(552, 481)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'LabelSup
        '
        Me.LabelSup.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelSup.AutoSize = True
        Me.LabelSup.Location = New System.Drawing.Point(3, 249)
        Me.LabelSup.Name = "LabelSup"
        Me.LabelSup.Size = New System.Drawing.Size(126, 29)
        Me.LabelSup.TabIndex = 42
        Me.LabelSup.Text = "N° support"
        Me.LabelSup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(3, 442)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(164, 29)
        Me.Label12.TabIndex = 37
        Me.Label12.Text = "N° déclaration"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 297)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(103, 29)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Quantité"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MTextBoxQté
        '
        Me.MTextBoxQté.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.MTextBoxQté.Location = New System.Drawing.Point(223, 294)
        Me.MTextBoxQté.MaximumSize = New System.Drawing.Size(116, 35)
        Me.MTextBoxQté.Name = "MTextBoxQté"
        Me.MTextBoxQté.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MTextBoxQté.Size = New System.Drawing.Size(116, 35)
        Me.MTextBoxQté.TabIndex = 24
        Me.MTextBoxQté.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MTextBoxQté.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(3, 345)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(69, 29)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Unité"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBoxUnité
        '
        Me.TextBoxUnité.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxUnité.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxUnité.Enabled = False
        Me.TextBoxUnité.Location = New System.Drawing.Point(223, 342)
        Me.TextBoxUnité.Name = "TextBoxUnité"
        Me.TextBoxUnité.Size = New System.Drawing.Size(116, 35)
        Me.TextBoxUnité.TabIndex = 29
        '
        'Label11
        '
        Me.Label11.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(3, 393)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(165, 29)
        Me.Label11.TabIndex = 36
        Me.Label11.Text = "N° lot fabriqué"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBoxLotF
        '
        Me.TextBoxLotF.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxLotF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxLotF.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxLotF.Location = New System.Drawing.Point(223, 390)
        Me.TextBoxLotF.MaxLength = 20
        Me.TextBoxLotF.Name = "TextBoxLotF"
        Me.TextBoxLotF.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxLotF.TabIndex = 39
        '
        'TextBoxTrk
        '
        Me.TextBoxTrk.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxTrk.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxTrk.Enabled = False
        Me.TextBoxTrk.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxTrk.Location = New System.Drawing.Point(223, 439)
        Me.TextBoxTrk.MaxLength = 20
        Me.TextBoxTrk.Name = "TextBoxTrk"
        Me.TextBoxTrk.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxTrk.TabIndex = 40
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 105)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(175, 29)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "N° lot / sous-lot"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBoxLot
        '
        Me.TextBoxLot.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxLot.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxLot.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxLot.Location = New System.Drawing.Point(223, 102)
        Me.TextBoxLot.MaxLength = 20
        Me.TextBoxLot.Name = "TextBoxLot"
        Me.TextBoxLot.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxLot.TabIndex = 3
        '
        'TextBoxArt
        '
        Me.TextBoxArt.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxArt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxArt.Enabled = False
        Me.TextBoxArt.Location = New System.Drawing.Point(223, 150)
        Me.TextBoxArt.MaxLength = 20
        Me.TextBoxArt.Name = "TextBoxArt"
        Me.TextBoxArt.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxArt.TabIndex = 41
        '
        'TextBoxDesign
        '
        Me.TextBoxDesign.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxDesign.Enabled = False
        Me.TextBoxDesign.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxDesign.Location = New System.Drawing.Point(223, 198)
        Me.TextBoxDesign.Name = "TextBoxDesign"
        Me.TextBoxDesign.Size = New System.Drawing.Size(326, 35)
        Me.TextBoxDesign.TabIndex = 27
        Me.TextBoxDesign.TabStop = False
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
        'CONSO
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
        Me.Name = "CONSO"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Consommation d'une matière"
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
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ErrorProvider As ErrorProvider
    Friend WithEvents MTextBoxMatr As MaskedTextBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel0 As TableLayoutPanel
    Friend WithEvents Label8 As Label
    Friend WithEvents TextBoxLot As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents MTextBoxQté As MaskedTextBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents TextBoxMsg As TextBox
    Friend WithEvents TextBoxDesign As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBoxUnité As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents TextBoxLotF As TextBox
    Friend WithEvents TextBoxTrk As TextBox
    Friend WithEvents TextBoxArt As TextBox
    Friend WithEvents LabelSup As Label
    Friend WithEvents ComboBoxSup As ComboBox
End Class
