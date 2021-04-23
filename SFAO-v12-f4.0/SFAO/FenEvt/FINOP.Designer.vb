<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FINOP
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
        Me.MaskedTextBoxOP = New System.Windows.Forms.MaskedTextBox()
        Me.ComboBoxSoldOp = New System.Windows.Forms.ComboBox()
        Me.ComboBoxMotifNS = New System.Windows.Forms.ComboBox()
        Me.TextBoxMsg = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TextBoxOF = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LabelMotifNS = New System.Windows.Forms.Label()
        Me.TableLayoutPanel0 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.RichTextBoxInfo = New System.Windows.Forms.RichTextBox()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel0.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnOk
        '
        Me.BtnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOk.Location = New System.Drawing.Point(48, 614)
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
        Me.BtnFin.Location = New System.Drawing.Point(456, 614)
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
        Me.Label1.Location = New System.Drawing.Point(3, 14)
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
        Me.Label2.Location = New System.Drawing.Point(3, 71)
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
        Me.TextBoxNom.Location = New System.Drawing.Point(223, 68)
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
        Me.Label4.Location = New System.Drawing.Point(3, 185)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(152, 29)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "N° Opération"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 128)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 29)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "N° OF"
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
        Me.MTextBoxMatr.Location = New System.Drawing.Point(223, 11)
        Me.MTextBoxMatr.Mask = "9999"
        Me.MTextBoxMatr.Name = "MTextBoxMatr"
        Me.MTextBoxMatr.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MTextBoxMatr.Size = New System.Drawing.Size(116, 35)
        Me.MTextBoxMatr.TabIndex = 1
        Me.MTextBoxMatr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MTextBoxMatr.ValidatingType = GetType(Integer)
        '
        'MaskedTextBoxOP
        '
        Me.MaskedTextBoxOP.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MaskedTextBoxOP.Enabled = False
        Me.MaskedTextBoxOP.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ErrorProvider.SetIconPadding(Me.MaskedTextBoxOP, 3)
        Me.MaskedTextBoxOP.Location = New System.Drawing.Point(223, 182)
        Me.MaskedTextBoxOP.Mask = "99999 "
        Me.MaskedTextBoxOP.MaximumSize = New System.Drawing.Size(116, 35)
        Me.MaskedTextBoxOP.Name = "MaskedTextBoxOP"
        Me.MaskedTextBoxOP.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MaskedTextBoxOP.Size = New System.Drawing.Size(116, 35)
        Me.MaskedTextBoxOP.TabIndex = 4
        Me.MaskedTextBoxOP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MaskedTextBoxOP.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        Me.MaskedTextBoxOP.ValidatingType = GetType(Integer)
        '
        'ComboBoxSoldOp
        '
        Me.ComboBoxSoldOp.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBoxSoldOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSoldOp.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxSoldOp.FormattingEnabled = True
        Me.ErrorProvider.SetIconPadding(Me.ComboBoxSoldOp, 3)
        Me.ComboBoxSoldOp.Items.AddRange(New Object() {"OUI", "NON"})
        Me.ComboBoxSoldOp.Location = New System.Drawing.Point(223, 240)
        Me.ComboBoxSoldOp.Name = "ComboBoxSoldOp"
        Me.ComboBoxSoldOp.Size = New System.Drawing.Size(116, 33)
        Me.ComboBoxSoldOp.TabIndex = 20
        '
        'ComboBoxMotifNS
        '
        Me.ComboBoxMotifNS.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBoxMotifNS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxMotifNS.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxMotifNS.FormattingEnabled = True
        Me.ErrorProvider.SetIconPadding(Me.ComboBoxMotifNS, 3)
        Me.ComboBoxMotifNS.Location = New System.Drawing.Point(223, 297)
        Me.ComboBoxMotifNS.Name = "ComboBoxMotifNS"
        Me.ComboBoxMotifNS.Size = New System.Drawing.Size(116, 33)
        Me.ComboBoxMotifNS.TabIndex = 21
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
        Me.TextBoxMsg.Size = New System.Drawing.Size(546, 105)
        Me.TextBoxMsg.TabIndex = 9
        Me.TextBoxMsg.TabStop = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxNom, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxMatr, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxOF, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.MaskedTextBoxOP, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelMotifNS, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBoxSoldOp, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBoxMotifNS, 1, 5)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 6
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(552, 343)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'TextBoxOF
        '
        Me.TextBoxOF.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxOF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxOF.Enabled = False
        Me.TextBoxOF.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxOF.Location = New System.Drawing.Point(223, 125)
        Me.TextBoxOF.Name = "TextBoxOF"
        Me.TextBoxOF.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxOF.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 242)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(200, 29)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Opération soldée"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelMotifNS
        '
        Me.LabelMotifNS.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelMotifNS.AutoSize = True
        Me.LabelMotifNS.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMotifNS.Location = New System.Drawing.Point(3, 299)
        Me.LabelMotifNS.Name = "LabelMotifNS"
        Me.LabelMotifNS.Size = New System.Drawing.Size(211, 29)
        Me.LabelMotifNS.TabIndex = 17
        Me.LabelMotifNS.Text = "Motif de non solde"
        Me.LabelMotifNS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        Me.TableLayoutPanel0.Controls.Add(Me.TableLayoutPanel3, 0, 2)
        Me.TableLayoutPanel0.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.TableLayoutPanel0.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel0.Name = "TableLayoutPanel0"
        Me.TableLayoutPanel0.RowCount = 3
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel0.Size = New System.Drawing.Size(560, 586)
        Me.TableLayoutPanel0.TabIndex = 19
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.TextBoxMsg, 0, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(4, 471)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(552, 111)
        Me.TableLayoutPanel3.TabIndex = 20
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.RichTextBoxInfo, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(4, 354)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(552, 110)
        Me.TableLayoutPanel2.TabIndex = 21
        '
        'RichTextBoxInfo
        '
        Me.RichTextBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBoxInfo.DetectUrls = False
        Me.RichTextBoxInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBoxInfo.Enabled = False
        Me.RichTextBoxInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBoxInfo.Location = New System.Drawing.Point(3, 3)
        Me.RichTextBoxInfo.Name = "RichTextBoxInfo"
        Me.RichTextBoxInfo.Size = New System.Drawing.Size(546, 104)
        Me.RichTextBoxInfo.TabIndex = 0
        Me.RichTextBoxInfo.TabStop = False
        Me.RichTextBoxInfo.Text = ""
        '
        'FINOP
        '
        Me.AcceptButton = Me.BtnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(14.0!, 29.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.BtnFin
        Me.ClientSize = New System.Drawing.Size(584, 661)
        Me.Controls.Add(Me.TableLayoutPanel0)
        Me.Controls.Add(Me.BtnFin)
        Me.Controls.Add(Me.BtnOk)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(800, 800)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(299, 300)
        Me.Name = "FINOP"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fin d'opération"
        Me.TopMost = True
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel0.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
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
    Friend WithEvents TextBoxMsg As TextBox
    Friend WithEvents MTextBoxMatr As MaskedTextBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel0 As TableLayoutPanel
    Friend WithEvents MaskedTextBoxOP As MaskedTextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents LabelMotifNS As Label
    Friend WithEvents TextBoxOF As TextBox
    Friend WithEvents ComboBoxSoldOp As ComboBox
    Friend WithEvents ComboBoxMotifNS As ComboBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents RichTextBoxInfo As RichTextBox
End Class
