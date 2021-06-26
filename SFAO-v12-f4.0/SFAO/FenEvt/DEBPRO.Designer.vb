<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DEBPRO
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
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
        Me.ComboBoxSaiPds = New System.Windows.Forms.ComboBox()
        Me.ComboBoxTypEtq = New System.Windows.Forms.ComboBox()
        Me.MTextBoxQtéUC = New System.Windows.Forms.MaskedTextBox()
        Me.MTextBoxNbUC = New System.Windows.Forms.MaskedTextBox()
        Me.MTextBoxNbUN = New System.Windows.Forms.MaskedTextBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LabelNbUN = New System.Windows.Forms.Label()
        Me.LabelNbUC = New System.Windows.Forms.Label()
        Me.LabelAmalg = New System.Windows.Forms.Label()
        Me.TextBoxOF = New System.Windows.Forms.TextBox()
        Me.LabelSaiPds = New System.Windows.Forms.Label()
        Me.LabelTypEtq = New System.Windows.Forms.Label()
        Me.LabelQtéUC = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBoxUOM = New System.Windows.Forms.TextBox()
        Me.TextAmalg = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel0 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.TextBoxMsg = New System.Windows.Forms.TextBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.DataGridPal = New System.Windows.Forms.DataGridView()
        Me.ColumnA = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnB = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnC = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnD = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnE = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnF = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnG = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnH = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnI = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColumnJ = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LabelPal = New System.Windows.Forms.Label()
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.TableLayoutPanel0.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.DataGridPal, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.Label1.Location = New System.Drawing.Point(3, 5)
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
        Me.Label2.Location = New System.Drawing.Point(3, 44)
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
        Me.TextBoxNom.Location = New System.Drawing.Point(223, 42)
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
        Me.Label4.Location = New System.Drawing.Point(3, 122)
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
        Me.Label3.Location = New System.Drawing.Point(3, 83)
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
        Me.MTextBoxMatr.Location = New System.Drawing.Point(223, 3)
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
        Me.MaskedTextBoxOP.Location = New System.Drawing.Point(223, 120)
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
        'ComboBoxSaiPds
        '
        Me.ComboBoxSaiPds.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBoxSaiPds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxSaiPds.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxSaiPds.FormattingEnabled = True
        Me.ErrorProvider.SetIconPadding(Me.ComboBoxSaiPds, 3)
        Me.ComboBoxSaiPds.Items.AddRange(New Object() {"OUI", "NON"})
        Me.ComboBoxSaiPds.Location = New System.Drawing.Point(223, 198)
        Me.ComboBoxSaiPds.Name = "ComboBoxSaiPds"
        Me.ComboBoxSaiPds.Size = New System.Drawing.Size(116, 33)
        Me.ComboBoxSaiPds.TabIndex = 20
        '
        'ComboBoxTypEtq
        '
        Me.ComboBoxTypEtq.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.ComboBoxTypEtq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxTypEtq.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxTypEtq.FormattingEnabled = True
        Me.ErrorProvider.SetIconPadding(Me.ComboBoxTypEtq, 3)
        Me.ComboBoxTypEtq.Location = New System.Drawing.Point(223, 237)
        Me.ComboBoxTypEtq.Name = "ComboBoxTypEtq"
        Me.ComboBoxTypEtq.Size = New System.Drawing.Size(180, 33)
        Me.ComboBoxTypEtq.TabIndex = 21
        '
        'MTextBoxQtéUC
        '
        Me.MTextBoxQtéUC.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MTextBoxQtéUC.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MTextBoxQtéUC.HidePromptOnLeave = True
        Me.ErrorProvider.SetIconPadding(Me.MTextBoxQtéUC, 3)
        Me.MTextBoxQtéUC.Location = New System.Drawing.Point(223, 315)
        Me.MTextBoxQtéUC.Mask = "99999999"
        Me.MTextBoxQtéUC.MaximumSize = New System.Drawing.Size(116, 35)
        Me.MTextBoxQtéUC.Name = "MTextBoxQtéUC"
        Me.MTextBoxQtéUC.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MTextBoxQtéUC.Size = New System.Drawing.Size(116, 35)
        Me.MTextBoxQtéUC.TabIndex = 29
        Me.MTextBoxQtéUC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MTextBoxQtéUC.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        Me.MTextBoxQtéUC.ValidatingType = GetType(Integer)
        '
        'MTextBoxNbUC
        '
        Me.MTextBoxNbUC.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MTextBoxNbUC.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MTextBoxNbUC.HidePromptOnLeave = True
        Me.ErrorProvider.SetIconPadding(Me.MTextBoxNbUC, 3)
        Me.MTextBoxNbUC.Location = New System.Drawing.Point(223, 354)
        Me.MTextBoxNbUC.Mask = "99"
        Me.MTextBoxNbUC.MaximumSize = New System.Drawing.Size(116, 35)
        Me.MTextBoxNbUC.Name = "MTextBoxNbUC"
        Me.MTextBoxNbUC.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MTextBoxNbUC.Size = New System.Drawing.Size(116, 35)
        Me.MTextBoxNbUC.TabIndex = 31
        Me.MTextBoxNbUC.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MTextBoxNbUC.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        Me.MTextBoxNbUC.ValidatingType = GetType(Integer)
        '
        'MTextBoxNbUN
        '
        Me.MTextBoxNbUN.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MTextBoxNbUN.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MTextBoxNbUN.HidePromptOnLeave = True
        Me.ErrorProvider.SetIconPadding(Me.MTextBoxNbUN, 3)
        Me.MTextBoxNbUN.Location = New System.Drawing.Point(223, 396)
        Me.MTextBoxNbUN.Mask = "99"
        Me.MTextBoxNbUN.MaximumSize = New System.Drawing.Size(116, 35)
        Me.MTextBoxNbUN.Name = "MTextBoxNbUN"
        Me.MTextBoxNbUN.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.MTextBoxNbUN.Size = New System.Drawing.Size(116, 35)
        Me.MTextBoxNbUN.TabIndex = 33
        Me.MTextBoxNbUN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.MTextBoxNbUN.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        Me.MTextBoxNbUN.ValidatingType = GetType(Integer)
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.LabelNbUN, 0, 10)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxNbUC, 1, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelNbUC, 0, 9)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxQtéUC, 1, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelAmalg, 0, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxNom, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxMatr, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxOF, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.MaskedTextBoxOP, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelSaiPds, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelTypEtq, 0, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBoxSaiPds, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.ComboBoxTypEtq, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.LabelQtéUC, 0, 8)
        Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.TextBoxUOM, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.TextAmalg, 1, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.MTextBoxNbUN, 1, 10)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(4, 4)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 11
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.09091!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(552, 437)
        Me.TableLayoutPanel1.TabIndex = 18
        '
        'LabelNbUN
        '
        Me.LabelNbUN.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelNbUN.AutoSize = True
        Me.LabelNbUN.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNbUN.Location = New System.Drawing.Point(3, 399)
        Me.LabelNbUN.Name = "LabelNbUN"
        Me.LabelNbUN.Size = New System.Drawing.Size(201, 29)
        Me.LabelNbUN.TabIndex = 32
        Me.LabelNbUN.Text = "Nb unités / format"
        Me.LabelNbUN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelNbUC
        '
        Me.LabelNbUC.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelNbUC.AutoSize = True
        Me.LabelNbUC.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNbUC.Location = New System.Drawing.Point(3, 356)
        Me.LabelNbUC.Name = "LabelNbUC"
        Me.LabelNbUC.Size = New System.Drawing.Size(156, 29)
        Me.LabelNbUC.TabIndex = 30
        Me.LabelNbUC.Text = "Nb UC / décl."
        Me.LabelNbUC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelAmalg
        '
        Me.LabelAmalg.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelAmalg.AutoSize = True
        Me.LabelAmalg.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelAmalg.Location = New System.Drawing.Point(3, 278)
        Me.LabelAmalg.Name = "LabelAmalg"
        Me.LabelAmalg.Size = New System.Drawing.Size(182, 29)
        Me.LabelAmalg.TabIndex = 22
        Me.LabelAmalg.Text = "Séq. amalgame"
        Me.LabelAmalg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBoxOF
        '
        Me.TextBoxOF.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxOF.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxOF.Enabled = False
        Me.TextBoxOF.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxOF.Location = New System.Drawing.Point(223, 81)
        Me.TextBoxOF.Name = "TextBoxOF"
        Me.TextBoxOF.Size = New System.Drawing.Size(283, 35)
        Me.TextBoxOF.TabIndex = 3
        '
        'LabelSaiPds
        '
        Me.LabelSaiPds.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelSaiPds.AutoSize = True
        Me.LabelSaiPds.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelSaiPds.Location = New System.Drawing.Point(3, 200)
        Me.LabelSaiPds.Name = "LabelSaiPds"
        Me.LabelSaiPds.Size = New System.Drawing.Size(207, 29)
        Me.LabelSaiPds.TabIndex = 16
        Me.LabelSaiPds.Text = "Saisie pds bobine"
        Me.LabelSaiPds.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelTypEtq
        '
        Me.LabelTypEtq.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelTypEtq.AutoSize = True
        Me.LabelTypEtq.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTypEtq.Location = New System.Drawing.Point(3, 239)
        Me.LabelTypEtq.Name = "LabelTypEtq"
        Me.LabelTypEtq.Size = New System.Drawing.Size(179, 29)
        Me.LabelTypEtq.TabIndex = 17
        Me.LabelTypEtq.Text = "Type étiquettes"
        Me.LabelTypEtq.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelQtéUC
        '
        Me.LabelQtéUC.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelQtéUC.AutoSize = True
        Me.LabelQtéUC.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelQtéUC.Location = New System.Drawing.Point(3, 317)
        Me.LabelQtéUC.Name = "LabelQtéUC"
        Me.LabelQtéUC.Size = New System.Drawing.Size(143, 29)
        Me.LabelQtéUC.TabIndex = 23
        Me.LabelQtéUC.Text = "Quantité UC"
        Me.LabelQtéUC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 161)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(186, 29)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "Unité fabrication"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextBoxUOM
        '
        Me.TextBoxUOM.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextBoxUOM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxUOM.Enabled = False
        Me.TextBoxUOM.Location = New System.Drawing.Point(223, 159)
        Me.TextBoxUOM.Name = "TextBoxUOM"
        Me.TextBoxUOM.Size = New System.Drawing.Size(116, 35)
        Me.TextBoxUOM.TabIndex = 27
        '
        'TextAmalg
        '
        Me.TextAmalg.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TextAmalg.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextAmalg.Location = New System.Drawing.Point(223, 276)
        Me.TextAmalg.Name = "TextAmalg"
        Me.TextAmalg.Size = New System.Drawing.Size(283, 35)
        Me.TextAmalg.TabIndex = 28
        '
        'TableLayoutPanel0
        '
        Me.TableLayoutPanel0.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel0.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.TableLayoutPanel0.ColumnCount = 1
        Me.TableLayoutPanel0.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel0.Controls.Add(Me.TableLayoutPanel3, 0, 2)
        Me.TableLayoutPanel0.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.TableLayoutPanel0.Controls.Add(Me.TableLayoutPanel1, 0, 0)
        Me.TableLayoutPanel0.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel0.Name = "TableLayoutPanel0"
        Me.TableLayoutPanel0.RowCount = 3
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65.0!))
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel0.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel0.Size = New System.Drawing.Size(560, 686)
        Me.TableLayoutPanel0.TabIndex = 19
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.TextBoxMsg, 0, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(4, 585)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(552, 97)
        Me.TableLayoutPanel3.TabIndex = 22
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
        Me.TextBoxMsg.Size = New System.Drawing.Size(546, 91)
        Me.TextBoxMsg.TabIndex = 10
        Me.TextBoxMsg.TabStop = False
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.DataGridPal, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.LabelPal, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(4, 448)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(552, 130)
        Me.TableLayoutPanel2.TabIndex = 21
        '
        'DataGridPal
        '
        Me.DataGridPal.AllowUserToAddRows = False
        Me.DataGridPal.AllowUserToDeleteRows = False
        Me.DataGridPal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridPal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridPal.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnA, Me.ColumnB, Me.ColumnC, Me.ColumnD, Me.ColumnE, Me.ColumnF, Me.ColumnG, Me.ColumnH, Me.ColumnI, Me.ColumnJ})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.Format = "N0"
        DataGridViewCellStyle2.NullValue = Nothing
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridPal.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridPal.Location = New System.Drawing.Point(3, 42)
        Me.DataGridPal.Name = "DataGridPal"
        Me.DataGridPal.Size = New System.Drawing.Size(546, 85)
        Me.DataGridPal.TabIndex = 0
        '
        'ColumnA
        '
        Me.ColumnA.HeaderText = "A"
        Me.ColumnA.MaxInputLength = 8
        Me.ColumnA.Name = "ColumnA"
        '
        'ColumnB
        '
        Me.ColumnB.HeaderText = "B"
        Me.ColumnB.MaxInputLength = 8
        Me.ColumnB.Name = "ColumnB"
        '
        'ColumnC
        '
        Me.ColumnC.HeaderText = "C"
        Me.ColumnC.MaxInputLength = 8
        Me.ColumnC.Name = "ColumnC"
        '
        'ColumnD
        '
        Me.ColumnD.HeaderText = "D"
        Me.ColumnD.MaxInputLength = 8
        Me.ColumnD.Name = "ColumnD"
        '
        'ColumnE
        '
        Me.ColumnE.HeaderText = "E"
        Me.ColumnE.MaxInputLength = 8
        Me.ColumnE.Name = "ColumnE"
        '
        'ColumnF
        '
        Me.ColumnF.HeaderText = "F"
        Me.ColumnF.MaxInputLength = 8
        Me.ColumnF.Name = "ColumnF"
        '
        'ColumnG
        '
        Me.ColumnG.HeaderText = "G"
        Me.ColumnG.MaxInputLength = 8
        Me.ColumnG.Name = "ColumnG"
        '
        'ColumnH
        '
        Me.ColumnH.HeaderText = "H"
        Me.ColumnH.MaxInputLength = 8
        Me.ColumnH.Name = "ColumnH"
        '
        'ColumnI
        '
        Me.ColumnI.HeaderText = "I"
        Me.ColumnI.MaxInputLength = 8
        Me.ColumnI.Name = "ColumnI"
        '
        'ColumnJ
        '
        Me.ColumnJ.HeaderText = "J"
        Me.ColumnJ.MaxInputLength = 8
        Me.ColumnJ.Name = "ColumnJ"
        '
        'LabelPal
        '
        Me.LabelPal.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LabelPal.AutoSize = True
        Me.LabelPal.Location = New System.Drawing.Point(3, 5)
        Me.LabelPal.Name = "LabelPal"
        Me.LabelPal.Size = New System.Drawing.Size(205, 29)
        Me.LabelPal.TabIndex = 1
        Me.LabelPal.Text = "Nb UC par palette"
        Me.LabelPal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DEBPRO
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
        Me.Name = "DEBPRO"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Début de production"
        Me.TopMost = True
        CType(Me.ErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.TableLayoutPanel0.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.DataGridPal, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents MaskedTextBoxOP As MaskedTextBox
    Friend WithEvents LabelSaiPds As Label
    Friend WithEvents LabelTypEtq As Label
    Friend WithEvents TextBoxOF As TextBox
    Friend WithEvents ComboBoxSaiPds As ComboBox
    Friend WithEvents ComboBoxTypEtq As ComboBox
    Friend WithEvents LabelAmalg As Label
    Friend WithEvents LabelQtéUC As Label
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBoxUOM As TextBox
    Friend WithEvents TextAmalg As TextBox
    Friend WithEvents MTextBoxNbUC As MaskedTextBox
    Friend WithEvents LabelNbUC As Label
    Friend WithEvents MTextBoxQtéUC As MaskedTextBox
    Friend WithEvents LabelNbUN As Label
    Friend WithEvents MTextBoxNbUN As MaskedTextBox
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents TextBoxMsg As TextBox
    Friend WithEvents DataGridPal As DataGridView
    Friend WithEvents ColumnA As DataGridViewTextBoxColumn
    Friend WithEvents ColumnB As DataGridViewTextBoxColumn
    Friend WithEvents ColumnC As DataGridViewTextBoxColumn
    Friend WithEvents ColumnD As DataGridViewTextBoxColumn
    Friend WithEvents ColumnE As DataGridViewTextBoxColumn
    Friend WithEvents ColumnF As DataGridViewTextBoxColumn
    Friend WithEvents ColumnG As DataGridViewTextBoxColumn
    Friend WithEvents ColumnH As DataGridViewTextBoxColumn
    Friend WithEvents ColumnI As DataGridViewTextBoxColumn
    Friend WithEvents ColumnJ As DataGridViewTextBoxColumn
    Friend WithEvents LabelPal As Label
End Class
