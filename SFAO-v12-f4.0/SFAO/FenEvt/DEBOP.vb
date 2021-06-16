'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout du séparateur //
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class DEBOP
    Private ofop As WSOFOPInfo

    Private Sub DEBOP_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty

        Trace("Affichage fenêtre DEBOP")

        'Si un seul opérateur présent sur le poste on prérempli le code matricule + nom 
        For i = 0 To FenSfao.WSsp.GRP2.Count - 1
            If FenSfao.WSsp.GRP2(i).XEMPNUM > 0 Then
                If MTextBoxMatr.Text = "" Then
                    MTextBoxMatr.Text = FenSfao.WSsp.GRP2(i).XEMPNUM.ToString
                    TextBoxNom.Text = FenSfao.WSsp.GRP2(i).ZEMPDES
                Else
                    MTextBoxMatr.Text = ""
                    TextBoxNom.Text = ""
                    Exit For
                End If
            End If
        Next

        'on a un seul matricule sur le poste
        If MTextBoxMatr.Text <> "" Then
            'on doit valider le matricule
            Matr_Valid(CInt(MTextBoxMatr.Text), MsgErr, False) 'sans affichage des erreurs
            If MsgErr <> "" Then
                TextBoxMsg.Text = MsgErr
                ErrorProvider.SetError(MTextBoxMatr, MsgErr)
                System.Media.SystemSounds.Exclamation.Play()
            End If
        End If

        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            MTextBoxMatr.Enabled = False
            'Sélection auto du 1er champ saisi
            TextBoxOF.Select()
        Else
            'Sélection auto du 1er champ saisi
            MTextBoxMatr.Select()
        End If

        'initialisation de la taille de police pour la gestion d'agrandissement de la fenêtre
        zTailFnt = FenSfao.TaillePolice(18, 9)
        fnt = New Font("Microsoft Sans Serif", zTailFnt, FontStyle.Regular)
        Me.Font = fnt

    End Sub
    'Fonction qui gère le changement de taille des polices en fonction de la taille de la fenêtre
    Private Sub DEBOP_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
        For Each ctl As Control In TableLayoutPanel1.Controls
            ctl.Font = Me.Font
        Next

        TextBoxMsg.Font = New Font(Me.Font.FontFamily, Me.Font.Size - 3, FontStyle.Regular)

        For Each btn As Button In Me.Controls.OfType(Of Button)
            btn.Font = New Font(Me.Font.FontFamily, Me.Font.Size - 4, FontStyle.Bold)
        Next
    End Sub


    Private Sub MTextBoxMatr_GotFocus(sender As Object, e As EventArgs) Handles MTextBoxMatr.GotFocus
        If ErrorProvider.GetError(MTextBoxMatr).ToString = "" Then
            TextBoxMsg.Text = "Veuillez saisir ou scanner le code de votre matricule"
        End If
        MTextBoxMatr.Select(0, MTextBoxMatr.Text.Length)
    End Sub

    Private Sub MTextBoxMatr_Validating(sender As Object, e As CancelEventArgs) Handles MTextBoxMatr.Validating
        Dim MsgErr As String = ""

        TextBoxNom.Text = ""

        If MTextBoxMatr.Text = "" Then
            'si le matricule est vide : erreur
            e.Cancel = True 'zone invalide
            MsgErr = "Veuillez saisir ou scanner le code de votre matricule"
            ErrorProvider.SetError(MTextBoxMatr, MsgErr)
            TextBoxMsg.Text = MsgErr
        Else

            'on doit valider le matricule
            Matr_Valid(CInt(MTextBoxMatr.Text), MsgErr, True) 'avec affichage des erreurs

            'en cas d'erreur on déclare l'erreur sur le ErrorProvider
            If MsgErr <> "" Then
                Trace("[MTextBoxMatr_Validating] : " & MsgErr)
                e.Cancel = True 'zone invalide
                ErrorProvider.SetError(MTextBoxMatr, MsgErr) 'set erreur provider
                TextBoxMsg.Text = MsgErr 'affichage commentaire
                System.Media.SystemSounds.Exclamation.Play() 'son erreur
                MTextBoxMatr.Select(0, MTextBoxMatr.Text.Length) 'sélection du texte saisi
            End If

        End If
    End Sub


    'fonction qui contrôle le matricule (contrôle si matricule présent, si durée présence dépassé, si opération hors OF ou opération std en cours)
    Private Sub Matr_Valid(ByVal matr As Integer, ByRef MsgErr As String, Optional ByVal afficheMsg As Boolean = True)

        'on contrôle si l'opérateur est présent sur le poste
        FenSfao.CtrlMatr(matr, MsgErr, TextBoxNom.Text)
        If MsgErr = "" Then
            'si ok on vérifie si opérateur est en opération hors OF
            FenSfao.OpHof(matr, MsgErr)
            If MsgErr = "" Then
                'si ok on vérifie si l'opérateur a déjà une opération en cours
                FenSfao.OpEnCours(matr, MsgErr)
            End If
        End If
    End Sub
    Private Sub MTextBoxMatr_Validated(sender As Object, e As EventArgs) Handles MTextBoxMatr.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxMatr, "")
        TextBoxMsg.Text = ""
    End Sub

    'Méthode qui affiche le commentaire sur la zone OF
    Private Sub TextBoxOF_GotFocus(sender As Object, e As EventArgs) Handles TextBoxOF.GotFocus
        If TextBoxOF.Tag Is Nothing Then
            TextBoxMsg.Text = "Veuillez saisir ou scanner le code de l'OF/OP"
            'Console.WriteLine("TextBoxOF_GotFocus")
        End If
        'TextBoxOF.Select(0, TextBoxOF.Text.Length)
    End Sub
    Private Sub TextBoxOF_Leave(sender As Object, e As EventArgs) Handles TextBoxOF.Leave
        'Console.WriteLine("TextBoxOF_Leave : OFOP_Validating")
        'OFOP_Validating(TextBoxOF.Text, MaskedTextBoxOP.Text, "OF")

    End Sub
    'Méthode qui contrôle la saisie de l'OF
    Private Sub TextBoxOF_Validating(sender As Object, e As CancelEventArgs) Handles TextBoxOF.Validating
        'Console.WriteLine("TextBoxOF_Validating")
        'If TextBoxOF.Tag Is Nothing Then                                                   '270321PNO.o
        If TextBoxOF.Tag Is Nothing OrElse TextBoxOF.Tag.ToString = String.Empty Then       '270321PNO.n
            OFOP_Validating(TextBoxOF.Text, MaskedTextBoxOP.Text, "OF")
        End If
        If TextBoxOF.Tag IsNot Nothing AndAlso TextBoxOF.Tag.ToString <> "" Then
            e.Cancel = True 'zone invalide
            ErrorProvider.SetError(TextBoxOF, TextBoxOF.Tag.ToString)
            TextBoxMsg.Text = TextBoxOF.Tag.ToString
            System.Media.SystemSounds.Exclamation.Play() 'son erreur
            TextBoxOF.Select() 'selection de la zone OF
            TextBoxOF.Select(0, TextBoxOF.Text.Length) 'sélection du texte saisi
            TextBoxOF.Tag = Nothing
        End If

    End Sub

    'méthode qui traite l'OF saisi
    Private Sub TextBoxOF_Validated(sender As Object, e As EventArgs) Handles TextBoxOF.Validated
        'Console.WriteLine("TextBoxOF_Validated")

        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextBoxOF, "")
        TextBoxMsg.Text = ""

    End Sub

    'Méthode qui affiche le commentaire sur la zone OP
    Private Sub MaskedTextBoxOP_GotFocus(sender As Object, e As EventArgs) Handles MaskedTextBoxOP.GotFocus
        If TextBoxOF.Tag Is Nothing Then
            TextBoxMsg.Text = "Veuillez saisir ou scanner le code de l'OF/OP"
        End If
        MaskedTextBoxOP.Select(0, MaskedTextBoxOP.Text.Length)
    End Sub

    Private Sub MaskedTextBoxOP_Leave(sender As Object, e As EventArgs) Handles MaskedTextBoxOP.Leave
        'Console.WriteLine("MaskedTextBoxOP_Leave : OFOP_Validating")
        If TextBoxOF.Tag Is Nothing Then
            OFOP_Validating(TextBoxOF.Text, MaskedTextBoxOP.Text, "OP")
            Me.ValidateChildren()
        End If
    End Sub
    Private Sub MaskedTextBoxOP_Validating(sender As Object, e As CancelEventArgs) Handles MaskedTextBoxOP.Validating

    End Sub
    Private Sub OFOP_Validating(ByRef _of As String, ByRef _op As String, ByVal _zone As String)
        Dim pattern As String = "^[A-Z0-9/]*$"
        Dim m As Match
        Dim nop As Integer
        Dim p As Integer
        Dim t As String

        'On elimine les espaces éventuels
        _of = _of.Replace(" ", "")
        _op = _op.Replace(" ", "")

        TextBoxOF.Tag = Nothing 'on initialise le Tag (si non vide c'est qu'on a une erreur)

        Select Case _zone
            Case "OF"

                If _of = "" Then 'si l'OF est vide : erreur
                    TextBoxOF.Tag = "Veuillez saisir ou scanner le code de l'OF/OP"
                    'Console.WriteLine("OFOP_Validating OF vide")
                Else
                    m = Regex.Match(_of, pattern, RegexOptions.None) 'on vérifie si l'OF contient autre chose que des lettres majuscules, des chifres et slash
                    'on doit valider le code OF : pas plus long que 20c et un seul / ou un double slash // ! 
                    'If Len(_of) > 20 OrElse _of.Count(Function(c As Char) c = "/") >= 2 OrElse m.Success = False Then
                    If Len(_of) > 21 OrElse _of.Count(Function(c As Char) c = "/") >= 3 OrElse m.Success = False Then
                        TextBoxOF.Tag = "Format de l'OF incorrect !"

                    ElseIf _of.Count(Function(c As Char) c = "/") = 2 Then 'si l'of contient deux //  '270321PNO.sn
                        t = _of
                        p = InStr(t, "//") + 1
                        'Si la zone OF contient l'OF+OP sous forme OFXXXXXX//50 on sépare l'OF et on copie l'OP dans la zone OP
                        If p > 0 Then
                            _of = Strings.Left(t, p - 2)
                            _op = Strings.Right(t, Len(t) - p)
                            t = _of
                        End If

                        If Len(t) > 0 And Len(t) <= 5 And IsNumeric(t) Then 'si dans l'OF on a que des chifres
                            'si on a saisi le n° simple de l'OF (ex : 1234) on le transforme en OFB2001234 en utilisant le site et l'année en cours
                            _of = "OF" & Strings.Left(SFAO.Site.GRP1.FCY, 1) & Now.ToString("yy") & StrDup(5 - Len(t), "0") & t
                        End If
                        '270321PNO.en
                    ElseIf _of.Count(Function(c As Char) c = "/") <= 1 Then 'si l'of contient aucun ou un /
                        t = _of
                        p = InStr(t, "/")
                        'Si la zone OF contient l'OF+OP sous forme OFXXXXXX/50 on sépare l'OF et on copie l'OP dans la zone OP
                        If p > 0 Then
                            _of = Strings.Left(t, p - 1)
                            _op = Strings.Right(t, Len(t) - p)
                            t = _of
                        End If

                        If Len(t) > 0 And Len(t) <= 5 And IsNumeric(t) Then 'si dans l'OF on a que des chifres
                            'si on a saisi le n° simple de l'OF (ex : 1234) on le transforme en OFB2001234 en utilisant le site et l'année en cours
                            _of = "OF" & Strings.Left(SFAO.Site.GRP1.FCY, 1) & Now.ToString("yy") & StrDup(5 - Len(t), "0") & t
                        End If
                    End If

                End If

            Case "OP"
                Int32.TryParse(_op, nop) 'on convertit l'op en numérique
                If _op = "" OrElse nop = 0 Then 'si l'OP est vide on met l'erreur sur la zone OF
                    TextBoxOF.Tag = "Veuillez saisir ou scanner le code de l'OF/OP"
                    'Console.WriteLine("OFOP_Validating Opération vide")
                Else
                    Try
                        'on fait valider le code OF/OP par le web service
                        Trace("Lancement du web service WSOFOPInfo")
                        ofop = X3ws.WSOFOPInfo(SFAO.Site.GRP1.FCY, _of, nop, SFAO.Poste.GRP1.Y_TYPOP)

                    Catch ex As Exception
                        Trace("Erreur de connexion au web service !", FichierTrace.niveau.alerte)
                        If ex.Message <> "" Then
                            Trace(ex.Message, FichierTrace.niveau.erreur)
                        End If

                        FenSfao.X3Anim(0)       'affichage du gif erreur
                        Application.DoEvents()  'rafraichir les écrans

                        Me.DialogResult = DialogResult.Abort
                        Me.Close() 'on quitte l'évenement
                    End Try

                    If ofop IsNot Nothing AndAlso ofop.GRP1.ZRET = 1 Then
                        'si on a un retour du web service : tout est ok
                        TextBoxCodCli.Text = ofop.GRP1.ZBPCNUM
                        TextBoxNomCli.Text = ofop.GRP1.ZBPCNAM
                        TextBoxArt1.Text = ofop.GRP1.ZITMREF
                        TextBoxArtDes.Text = ofop.GRP1.ZITMDES
                    ElseIf ofop IsNot Nothing AndAlso ofop.GRP1.ZRET = 0 Then
                        'si retour du web service avec erreur
                        TextBoxCodCli.Text = ""
                        TextBoxNomCli.Text = ""
                        TextBoxArt1.Text = ""
                        TextBoxArtDes.Text = ""
                        TextBoxOF.Tag = ofop.GRP1.ZMSG
                    Else
                        'cas de retour vide du web service 
                        TextBoxCodCli.Text = ""
                        TextBoxNomCli.Text = ""
                        TextBoxArt1.Text = ""
                        TextBoxArtDes.Text = ""
                        TextBoxOF.Tag = "OF/OP inconnu !"
                    End If

                End If

        End Select

        'If TextBoxOF.Tag IsNot Nothing Then 'si on a une erreur on efface la zone opération
        '_op = ""
        'End If
    End Sub

    Private Sub BtnFin_Click(sender As Object, e As EventArgs) Handles BtnFin.Click
        Me.Close()
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        Dim retMsg As String = String.Empty
        Dim debop As Integer = -1

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next

        'tout va bien on enregistre le début d'opération + suivi auto du temps passé depuis le dernier évenement
        Try
            Trace("Appel du web service WSDEBOPE")
            debop = X3ws.WSDEBOPE(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), CInt(Me.Tag), TextBoxOF.Text, CInt(MaskedTextBoxOP.Text), retMsg)
        Catch ex As Exception
            GoTo ErreurDebop
        End Try

        Select Case debop
            Case -1 'Erreur du web service
                GoTo erreurDebop
            Case 0 'Erreur blocage 
                Trace(retMsg, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
            Case 1 'ok
                Me.DialogResult = DialogResult.OK
        End Select

        Exit Sub

ErreurDebop:
        Trace("Erreur d'enregistrement du début d'opération ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        Me.DialogResult = DialogResult.Abort
        Me.Close()
    End Sub

End Class