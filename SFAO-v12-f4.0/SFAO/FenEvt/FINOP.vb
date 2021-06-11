'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout du séparateur //
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class FINOP

    Private Sub FINOP_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty
        Dim palnvl As MsgBoxResult

        Trace("Affichage fenêtre FINOP")

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
            MatrOFOP_Valid(CInt(MTextBoxMatr.Text), MsgErr)
            If MsgErr <> "" Then
                TextBoxMsg.Text = MsgErr
                ErrorProvider.SetError(MTextBoxMatr, MsgErr)
                System.Media.SystemSounds.Exclamation.Play()
            End If
        End If

        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            'Si pas d'opération exceptionnelle ni palettisation : gestion solde d'opération
            If SFAO.Poste.GRP1.Y_TYPOP <> "PAL" And FenSfao.OpExc(CInt(MTextBoxMatr.Text)) = 0 Then
                palnvl = PalNvld(TextBoxOF.Text, MsgErr)
                If palnvl = MsgBoxResult.No Then
                    Me.DialogResult = DialogResult.Abort
                    Me.Close()
                End If

                'Affichage du bilan de production/consommation de l'opération
                Call BilanOP(CInt(MTextBoxMatr.Text), MsgErr)

                'Récupération de la liste des motifs de non solde d'opération
                For i = 0 To FenSfao.WSLstMns.GRP2.Count - 1
                    ComboBoxMotifNS.Items.Add(FenSfao.WSLstMns.GRP2(i).DESSHO)
                Next i

                'Par défaut, on masque le label motif non solde + champ de saisie
                LabelMotifNS.Visible = False
                ComboBoxMotifNS.Visible = False
            Else
                'Pas de solde si opération exceptionnelle d'OF ou opération Palettisation
                LabelSoldOp.Visible = False
                ComboBoxSoldOp.Visible = False
                ComboBoxSoldOp.Enabled = False
                LabelMotifNS.Visible = False
                ComboBoxMotifNS.Visible = False
                ComboBoxMotifNS.Enabled = False
            End If
        End If

        'Si site Tilwel et opération d'impression : saisie des encres et vernis utilisés
        If SFAO.Poste.GRP1.WCRFCY <> "TIL" Or Strings.Left(SFAO.Poste.GRP1.Y_TYPOP, 3) <> "IMP" Then
            'Sinon on masque les champs
            LabelTotEnc.Visible = False
            MTextBoxTotEnc.Visible = False
            MTextBoxTotEnc.Enabled = False
            LabelTotVer.Visible = False
            MTextBoxTotVer.Visible = False
            MTextBoxTotVer.Enabled = False
        End If

        'Sélection auto du 1er champ saisi
        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            MTextBoxMatr.Enabled = False
            If ComboBoxSoldOp.Visible Then
                ComboBoxSoldOp.Select()
            Else
                MTextBoxTotEnc.Select()
            End If
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
    Private Sub FINOP_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
        For Each ctl As Control In TableLayoutPanel1.Controls
            ctl.Font = Me.Font
        Next

        TextBoxMsg.Font = New Font(Me.Font.FontFamily, Me.Font.Size - 3, FontStyle.Regular)

        For Each btn As Button In Me.Controls.OfType(Of Button)
            btn.Font = New Font(Me.Font.FontFamily, Me.Font.Size - 4, FontStyle.Bold)
        Next
    End Sub

    'fonction qui contrôle les palettes non validées
    Private Function PalNvld(ByVal numof As String, ByRef MsgErr As String) As MsgBoxResult
        Dim LstPalNvld As WSLstPal
        Dim i As Integer
        Dim msgpal As String
        Dim sep As String = String.Empty
        Dim result As MsgBoxResult
        result = MsgBoxResult.Ok

        Trace("Contrôle des palettes non validées de l'OF : " + numof)
        If numof = "" Then
            Try
                Trace("Appel du web service WSPALNVLD")
                LstPalNvld = X3ws.WSPALNVLD(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.STOLOC, numof)
            Catch ex As Exception
                GoTo ErreurPalNvld
            End Try

            If LstPalNvld.GRP2.Count > 0 Then
                msgpal = "Il reste des palettes non validées :" + vbNewLine + "Pal. n° "
                For i = 0 To LstPalNvld.GRP2.Count - 1
                    msgpal += sep + CStr(CInt(Strings.Right(LstPalNvld.GRP2(i).ZPALNVLD, 4)))
                    sep = ", "
                Next
                Trace(msgpal)
                msgpal += vbNewLine + "Continuer quand même la fin d'opération ?"
                result = MsgBox(msgpal, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
            End If
        End If

        Return result
        Exit Function

ErreurPalNvld:
        Trace("Erreur de contrôle de palettes non validées ! ", FichierTrace.niveau.alerte)
        If MsgErr <> "" Then
            Trace(MsgErr, FichierTrace.niveau.erreur)
        End If

        Return result
    End Function

    'fonction qui calcul et affiche le bilan de l'opération
    Private Sub BilanOP(ByVal matr As Integer, ByRef MsgErr As String)
        Dim qteAQ As Decimal = 0
        Dim qteQN As Decimal = 0
        Dim qteR As Decimal = 0
        Dim nbPcu As Integer = 0
        Dim uom As String = String.Empty
        Dim qteSup1 As Decimal = 0
        Dim qteSup2 As Decimal = 0
        Dim qteRet As Decimal = 0
        Dim unite As String = String.Empty
        Dim qteLnk1 As Decimal = 0
        Dim qteLnk2 As Decimal = 0
        Dim getqpro As Integer
        Dim getqcso As Integer
        Dim qtepro As Decimal = 0
        Dim ecart As Decimal = 0

        Trace("Calcul du BILAN d'opération")

        RichTextBoxInfo.Text = "BILAN" + vbTab + "Qté produite : $QP" + vbNewLine + vbTab + vbTab + "Qté rebut : $QR"
        RichTextBoxInfo.Text += vbNewLine + vbTab + vbTab + "Conso sup 1 : $QS"
        RichTextBoxInfo.Text += vbNewLine + vbTab + vbTab + "ECART : $QE" + vbNewLine + vbNewLine + "$RM"

        RichTextBoxInfo.Find("BILAN")
        RichTextBoxInfo.SelectionFont = New Font("", RichTextBoxInfo.SelectionFont.Size, FontStyle.Bold)
        RichTextBoxInfo.SelectionColor = Color.Black

        Try
            Trace("Appel du web service WSGETQPRO")
            getqpro = X3ws.WSGETQPRO(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, matr, qteAQ, qteQN, qteR, nbPcu, uom, MsgErr)
        Catch ex As Exception
            GoTo ErreurBilanOP
        End Try

        Select Case getqpro
            Case -1 'Erreur du web service
                GoTo ErreurBilanOP
            Case 0 'Erreur blocage 
                Trace(MsgErr, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
            Case 1 'ok
                'On convertit l'unité dans un format lisible pour l'opérateur
                uom = FenSfao.AffUnit(uom)
                Trace("Affichage des quantités produites")
                RichTextBoxInfo.Find("$QP")
                If nbPcu > 1 Then
                    qtepro = (qteAQ + qteQN) / nbPcu
                    RichTextBoxInfo.SelectedText = CStr(nbPcu) + " * " + CStr(qtepro) + " " + uom
                Else
                    qtepro = qteAQ + qteQN
                    RichTextBoxInfo.SelectedText = CStr(qtepro) + " " + uom
                End If
                RichTextBoxInfo.Find("$QR")
                If qteR > 0 Then
                    RichTextBoxInfo.SelectedText = CStr(qteR) + " " + uom
                Else
                    RichTextBoxInfo.SelectedText = ""
                End If

        End Select

        Try
            Trace("Appel du web service WSGETQCSO")
            getqcso = X3ws.WSGETQCSO(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.STOLOC, matr, qteSup1, qteSup2, qteRet, unite, qteLnk1, qteLnk2, MsgErr)
        Catch ex As Exception
            GoTo ErreurBilanOP
        End Try

        Select Case getqcso
            Case -1 'Erreur du web service
                GoTo ErreurBilanOP
            Case 0 'Erreur blocage 
                Trace(MsgErr, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
            Case 1 'ok
                Trace("Affichage des quantités consommées")
                RichTextBoxInfo.Find("$QS")
                RichTextBoxInfo.SelectedText = CStr(qteSup1) + " " + unite
                If qteSup2 > 0 Then
                    RichTextBoxInfo.SelectedText += vbNewLine + vbTab + vbTab + "Conso sup 2 : " + CStr(qteSup2) + " " + unite
                End If

        End Select

        Trace("Affichage de l'écart : " + CStr(ecart) + " " + unite)
        ecart = (qtepro + qteR) * qteLnk1 - qteSup1
        If qteSup2 > 0 Then
            ecart += (qtepro + qteR) * qteLnk2 - qteSup2
        End If

        'Si l'écart est nul, on l'affiche en vert, sinon en rouge et en gras
        RichTextBoxInfo.Find("ECART : $QE")
        If ecart = 0 Then
            RichTextBoxInfo.SelectionColor = Color.Green
        Else
            RichTextBoxInfo.SelectionFont = New Font("", RichTextBoxInfo.SelectionFont.Size, FontStyle.Bold)
            RichTextBoxInfo.SelectionColor = Color.Red
        End If
        RichTextBoxInfo.Find("$QE")
        RichTextBoxInfo.SelectedText = CStr(ecart) + " " + unite

        RichTextBoxInfo.Find("$RM")
        If qteRet > 0 Then
            RichTextBoxInfo.SelectionColor = Color.Blue
            RichTextBoxInfo.SelectedText = "ATTENTION : Faire le RETOUR matières !" + vbNewLine + vbTab + vbTab + "Reste sur machine : " + CStr(qteRet) + " " + unite
            Trace("ATTENTION ! Faire le RETOUR matières : " + CStr(qteRet) + " " + unite)
        Else
            RichTextBoxInfo.SelectedText = ""
        End If

        Exit Sub

ErreurBilanOP:
        Trace("Erreur de calcul du bilan d'opération ! ", FichierTrace.niveau.alerte)
        If MsgErr <> "" Then
            Trace(MsgErr, FichierTrace.niveau.erreur)
        End If

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
            MatrOFOP_Valid(CInt(MTextBoxMatr.Text), MsgErr)

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
    Private Sub MatrOFOP_Valid(ByVal matr As Integer, ByRef MsgErr As String)

        'on contrôle si l'opérateur est présent sur le poste
        FenSfao.CtrlMatr(matr, MsgErr, TextBoxNom.Text)
        If MsgErr = "" Then
            'si ok on vérifie si opérateur est en opération hors OF
            FenSfao.OpHof(matr, MsgErr)
            If MsgErr = "" Then
                'si ok on vérifie si l'opérateur a déjà une opération en cours
                FenSfao.OFOpMatr(matr, TextBoxOF.Text, MaskedTextBoxOP.Text, MsgErr)
            End If
        End If
    End Sub

    Private Sub MTextBoxMatr_Validated(sender As Object, e As EventArgs) Handles MTextBoxMatr.Validated
        Dim MsgErr As String = String.Empty
        Dim palnvl As MsgBoxResult
        Dim i As Integer

        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxMatr, "")
        TextBoxMsg.Text = ""

        'Si pas d'opération exceptionnelle ni palettisation : gestion solde d'opération
        If SFAO.Poste.GRP1.Y_TYPOP <> "PAL" And FenSfao.OpExc(CInt(MTextBoxMatr.Text)) = 0 Then
            palnvl = PalNvld(TextBoxOF.Text, MsgErr)
            If palnvl = MsgBoxResult.No Then
                Me.DialogResult = DialogResult.Abort
                Me.Close()
            End If

            'Affichage du bilan de production/consommation de l'opération
            Call BilanOP(CInt(MTextBoxMatr.Text), MsgErr)

            'Récupération de la liste des motifs de non solde d'opération
            For i = 0 To FenSfao.WSLstMns.GRP2.Count - 1
                ComboBoxMotifNS.Items.Add(FenSfao.WSLstMns.GRP2(i).DESSHO)
            Next i

            'Par défaut, on masque le label motif non solde + champ de saisie
            LabelMotifNS.Visible = False
            ComboBoxMotifNS.Visible = False
        Else
            'Pas de solde si opération exceptionnelle d'OF ou opération Palettisation
            LabelSoldOp.Visible = False
            ComboBoxSoldOp.Visible = False
            ComboBoxSoldOp.Enabled = False
            LabelMotifNS.Visible = False
            ComboBoxMotifNS.Visible = False
            ComboBoxMotifNS.Enabled = False
        End If

    End Sub

    Private Sub BtnFin_Click(sender As Object, e As EventArgs) Handles BtnFin.Click
        Me.Close()
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        Dim retMsg As String = String.Empty
        Dim nbapl As Integer
        Dim nbpal As Integer
        Dim ctrlapl As Integer
        Dim msgapl As String
        Dim conso As Integer
        Dim finop As Integer
        Dim result As MsgBoxResult

        'Si gestion du solde d'opération
        If LabelSoldOp.Visible Then
            'on déclenche la validation de la zone opération soldée
            If ComboBoxSoldOp.Text = "" Then
                ErrorProvider.SetError(ComboBoxSoldOp, "Oui/Non")
                ComboBoxSoldOp.Select()
                Exit Sub
            End If
            'on déclenche la validation de la zone motif NS si opération non soldée
            If ComboBoxSoldOp.Text = "NON" And ComboBoxMotifNS.Text = "" Then
                ErrorProvider.SetError(ComboBoxMotifNS, "Motif de non solde obligatoire")
                ComboBoxMotifNS.Select()
                Exit Sub
            End If
        End If

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next

        'Si opération soldée, on vérifie si on est à une étape de production
        If ComboBoxSoldOp.Text = "OUI" AndAlso FenSfao.EtapePro(TextBoxOF.Text, CInt(MaskedTextBoxOP.Text)) = True Then
            Trace("Opération soldée et étape de production : contrôle si commande sur appel du nombre de palettes")
            'Si cde/appel, contrôle du nombre de palettes produites / nombre d'appels
            Try
                Trace("Appel du web service WSNBPALAPL")
                ctrlapl = X3ws.WSNBPALAPL(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), nbapl, nbpal, retMsg)
            Catch ex As Exception
                GoTo ErreurFinop
            End Try

            Select Case ctrlapl
                Case -1 'Erreur du web service
                    GoTo ErreurFinop
                Case 0 'Erreur blocage 
                    Trace(retMsg, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
                Case 1 'ok
                    If nbapl = 0 Then
                        Trace("Ce n'est pas une commande sur appel")
                    Else
                        msgapl = "Commande sur appel ok : palettes produites = " + CStr(nbpal) + " et nombre d'appels = " + CStr(nbapl)
                        Trace(msgapl)
                    End If

                Case 2
                    msgapl = "Attention, il s'agit d'une commande sur appel et il y a moins de palettes produites (" + CStr(nbpal) + ") que d'appels (" + CStr(nbapl) + ")"
                    Trace(msgapl)
                    msgapl += vbNewLine + "Voulez-vous quand même solder l'opération ?"
                    result = MsgBox(msgapl, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
                    If result = MsgBoxResult.No Then
                        Me.DialogResult = DialogResult.Abort
                        Me.Close()
                    End If
            End Select
        End If

        'tout va bien on enregistre la conso des encres/vernis et la fin d'opération

        'affichage le load dans 100 ms
        Call FenSfao.WaitGif(True, 100)

        'on enregistre la consommation des encres et vernis utilisés
        '(avant la fin d'opération, puisqu'on utilise les infos de la situation d'opération)
        If MTextBoxTotEnc.Visible AndAlso MTextBoxTotVer.Visible AndAlso (CDec(MTextBoxTotEnc.Text) > 0 Or CDec(MTextBoxTotVer.Text) > 0) Then
            Try
                Trace("Appel du web service WSCSOENC")
                conso = X3ws.WSCSOENC(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), CInt(Me.Tag), CDec(MTextBoxTotEnc.Text), CDec(MTextBoxTotVer.Text), retMsg)
            Catch ex As Exception
                GoTo ErreurFinop
            End Try

            Select Case conso
                Case -1 'Erreur du web service
                    GoTo ErreurFinop
                Case 0 'Erreur blocage 
                    Trace(retMsg, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
                    Me.DialogResult = DialogResult.Abort
                    Me.Close()
                    'On masque le load dans 0.5s
                    Call FenSfao.WaitGif(False, 500)
                Case 1 'ok
                    Me.DialogResult = DialogResult.OK
            End Select
        End If

        'on enregistre la fin d'opération + suivi auto du temps passé depuis le dernier évenement
        Try
            Trace("Appel du web service WSFINOPE")
            finop = X3ws.WSFINOPE(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), CInt(Me.Tag), ComboBoxSoldOp.Text, ComboBoxMotifNS.Text, retMsg)
        Catch ex As Exception
            GoTo ErreurFinop
        End Try

        Select Case finop
            Case -1 'Erreur du web service
                GoTo ErreurFinop
            Case 0 'Erreur blocage 
                Trace(retMsg, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
                Me.DialogResult = DialogResult.Abort
                Me.Close()
                'On masque le load dans 0.5s
                Call FenSfao.WaitGif(False, 500)
            Case 1 'ok
                Me.DialogResult = DialogResult.OK
        End Select

        Exit Sub

ErreurFinop:
        Trace("Erreur d'enregistrement de fin d'opération ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        Me.DialogResult = DialogResult.Abort
        Me.Close()
        'On masque le load dans 0.5s
        Call FenSfao.WaitGif(False, 500)
    End Sub

    Private Sub ComboBoxSoldOp_GotFocus(sender As Object, e As EventArgs) Handles ComboBoxSoldOp.GotFocus
        TextBoxMsg.Text = "Opération terminée ?"
    End Sub

    Private Sub ComboBoxSoldOp_Validating(sender As Object, e As CancelEventArgs) Handles ComboBoxSoldOp.Validating
        If ComboBoxSoldOp.Text = "" Then
            e.Cancel = True
            ErrorProvider.SetError(ComboBoxSoldOp, "Oui/Non")
            ComboBoxSoldOp.Select()
        End If
    End Sub

    Private Sub ComboBoxSoldOp_Validated(sender As Object, e As EventArgs) Handles ComboBoxSoldOp.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(ComboBoxSoldOp, "")
        TextBoxMsg.Text = ""
        ComboBoxMotifNS.Select()
    End Sub

    Private Sub ComboBoxSoldOp_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBoxSoldOp.SelectionChangeCommitted
        'on affiche le motif de non solde si "NON" sélectionné
        If ComboBoxSoldOp.Text = "NON" Then
            LabelMotifNS.Visible = True
            ComboBoxMotifNS.Visible = True
        Else
            LabelMotifNS.Visible = False
            ComboBoxMotifNS.Visible = False
        End If
    End Sub

    Private Sub ComboBoxMotifNS_GotFocus(sender As Object, e As EventArgs) Handles ComboBoxMotifNS.GotFocus
        TextBoxMsg.Text = "Pour quelle raison l'opération est-elle arrêtée mais non soldée ?"
    End Sub

    Private Sub ComboBoxMotifNS_Validating(sender As Object, e As CancelEventArgs) Handles ComboBoxMotifNS.Validating
        If ComboBoxSoldOp.Text = "NON" AndAlso ComboBoxMotifNS.Text = "" Then
            e.Cancel = True
            ErrorProvider.SetError(ComboBoxMotifNS, "Motif de non solde obligatoire")
            ComboBoxMotifNS.Select()
        End If
    End Sub

    Private Sub ComboBoxMotifNS_Validated(sender As Object, e As EventArgs) Handles ComboBoxMotifNS.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(ComboBoxMotifNS, "")
        TextBoxMsg.Text = ""
    End Sub
End Class