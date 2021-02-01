<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Login
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBoxMdp = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBoxPoste = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxSit = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxSoc = New System.Windows.Forms.TextBox()
        Me.BtnCnxFin = New System.Windows.Forms.Button()
        Me.BtnCnxOk = New System.Windows.Forms.Button()
        Me.LblFin = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboBoxDos = New System.Windows.Forms.ComboBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(70, 270)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(158, 29)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "Mot de passe"
        '
        'TextBoxMdp
        '
        Me.TextBoxMdp.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxMdp.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxMdp.Location = New System.Drawing.Point(282, 272)
        Me.TextBoxMdp.MaxLength = 20
        Me.TextBoxMdp.Name = "TextBoxMdp"
        Me.TextBoxMdp.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.TextBoxMdp.Size = New System.Drawing.Size(266, 24)
        Me.TextBoxMdp.TabIndex = 5
        Me.TextBoxMdp.UseSystemPasswordChar = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(70, 217)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 29)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Poste"
        '
        'TextBoxPoste
        '
        Me.TextBoxPoste.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxPoste.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxPoste.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxPoste.Location = New System.Drawing.Point(282, 219)
        Me.TextBoxPoste.MaxLength = 10
        Me.TextBoxPoste.Name = "TextBoxPoste"
        Me.TextBoxPoste.Size = New System.Drawing.Size(266, 24)
        Me.TextBoxPoste.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(70, 111)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 29)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Site"
        '
        'TextBoxSit
        '
        Me.TextBoxSit.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxSit.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxSit.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxSit.Location = New System.Drawing.Point(282, 113)
        Me.TextBoxSit.MaxLength = 15
        Me.TextBoxSit.Name = "TextBoxSit"
        Me.TextBoxSit.Size = New System.Drawing.Size(266, 24)
        Me.TextBoxSit.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(70, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 29)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Société"
        '
        'TextBoxSoc
        '
        Me.TextBoxSoc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxSoc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextBoxSoc.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxSoc.Location = New System.Drawing.Point(282, 60)
        Me.TextBoxSoc.Margin = New System.Windows.Forms.Padding(5)
        Me.TextBoxSoc.MaxLength = 30
        Me.TextBoxSoc.Name = "TextBoxSoc"
        Me.TextBoxSoc.Size = New System.Drawing.Size(266, 24)
        Me.TextBoxSoc.TabIndex = 1
        '
        'BtnCnxFin
        '
        Me.BtnCnxFin.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.BtnCnxFin.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.BtnCnxFin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCnxFin.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCnxFin.ForeColor = System.Drawing.Color.White
        Me.BtnCnxFin.Location = New System.Drawing.Point(355, 349)
        Me.BtnCnxFin.Name = "BtnCnxFin"
        Me.BtnCnxFin.Size = New System.Drawing.Size(200, 40)
        Me.BtnCnxFin.TabIndex = 7
        Me.BtnCnxFin.Text = "Quitter"
        Me.BtnCnxFin.UseVisualStyleBackColor = True
        '
        'BtnCnxOk
        '
        Me.BtnCnxOk.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.BtnCnxOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCnxOk.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCnxOk.ForeColor = System.Drawing.Color.White
        Me.BtnCnxOk.Location = New System.Drawing.Point(70, 349)
        Me.BtnCnxOk.Name = "BtnCnxOk"
        Me.BtnCnxOk.Size = New System.Drawing.Size(200, 40)
        Me.BtnCnxOk.TabIndex = 6
        Me.BtnCnxOk.Text = "OK"
        Me.BtnCnxOk.UseVisualStyleBackColor = True
        '
        'LblFin
        '
        Me.LblFin.AutoSize = True
        Me.LblFin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFin.ForeColor = System.Drawing.Color.Red
        Me.LblFin.Location = New System.Drawing.Point(601, 9)
        Me.LblFin.Name = "LblFin"
        Me.LblFin.Size = New System.Drawing.Size(17, 16)
        Me.LblFin.TabIndex = 19
        Me.LblFin.Text = "X"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(70, 164)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 29)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Dossier"
        '
        'ComboBoxDos
        '
        Me.ComboBoxDos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDos.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBoxDos.FormattingEnabled = True
        Me.ComboBoxDos.ItemHeight = 25
        Me.ComboBoxDos.Location = New System.Drawing.Point(275, 162)
        Me.ComboBoxDos.Name = "ComboBoxDos"
        Me.ComboBoxDos.Size = New System.Drawing.Size(280, 33)
        Me.ComboBoxDos.TabIndex = 3
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.White
        Me.PictureBox1.Location = New System.Drawing.Point(275, 56)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(280, 33)
        Me.PictureBox1.TabIndex = 20
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.White
        Me.PictureBox2.Location = New System.Drawing.Point(275, 109)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(280, 33)
        Me.PictureBox2.TabIndex = 21
        Me.PictureBox2.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.White
        Me.PictureBox3.Location = New System.Drawing.Point(275, 215)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(280, 33)
        Me.PictureBox3.TabIndex = 22
        Me.PictureBox3.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.BackColor = System.Drawing.Color.White
        Me.PictureBox4.Location = New System.Drawing.Point(275, 268)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(280, 33)
        Me.PictureBox4.TabIndex = 23
        Me.PictureBox4.TabStop = False
        '
        'Login
        '
        Me.AcceptButton = Me.BtnCnxOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(56, Byte), Integer), CType(CType(96, Byte), Integer), CType(CType(155, Byte), Integer))
        Me.CancelButton = Me.BtnCnxFin
        Me.ClientSize = New System.Drawing.Size(630, 433)
        Me.ControlBox = False
        Me.Controls.Add(Me.ComboBoxDos)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.LblFin)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TextBoxMdp)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBoxPoste)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBoxSit)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBoxSoc)
        Me.Controls.Add(Me.BtnCnxFin)
        Me.Controls.Add(Me.BtnCnxOk)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBoxMdp As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBoxPoste As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxSit As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBoxSoc As TextBox
    Friend WithEvents BtnCnxFin As Button
    Friend WithEvents BtnCnxOk As Button
    Friend WithEvents LblFin As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents ComboBoxDos As ComboBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents PictureBox4 As PictureBox
End Class
