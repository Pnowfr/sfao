'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout du séparateur //
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class FINOP
    Private ofop As WSOFOPInfo

    Private Sub FINOP_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty

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
            MatrOFOP_Valid(CInt(MTextBoxMatr.Text), MsgErr, False) 'sans affichage des erreurs
            If MsgErr <> "" Then
                TextBoxMsg.Text = MsgErr
                ErrorProvider.SetError(MTextBoxMatr, MsgErr)
                System.Media.SystemSounds.Exclamation.Play()
            End If
        End If

        'Récupération de la liste des motifs de non solde d'opération
        For i = 0 To FenSfao.WSLstMns.GRP2.Count - 1
            ComboBoxMotifNS.Items.Add(FenSfao.WSLstMns.GRP2(i).DESSHO)
        Next i

        'Affichage du bilan de production/consommation de l'opération
        Call BilanOP()

        LabelMotifNS.Visible = False     'on masque le label motif non solde + champ de saisie
        ComboBoxMotifNS.Visible = False

        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            MTextBoxMatr.Enabled = False
            'Sélection auto du 1er champ saisi
            ComboBoxSoldOp.Select()
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

    'fonction qui calcul et affiche le bilan de l'opération
    Private Sub BilanOP()
        RichTextBoxInfo.Rtf = "{\rtf1\ansi\b BILAN :\b0\tab Qté produite :\line\tab\tab Qté rebut :\line\tab\tab Conso sup 1 :\line\tab \b Ecart :\b0}"
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
            MatrOFOP_Valid(CInt(MTextBoxMatr.Text), MsgErr, True) 'avec affichage des erreurs

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
    Private Sub MatrOFOP_Valid(ByVal matr As Integer, ByRef MsgErr As String, Optional ByVal afficheMsg As Boolean = True)

        'on contrôle si l'opérateur est présent sur le poste
        FenSfao.CtrlMatr(matr, MsgErr, TextBoxNom.Text)
        If MsgErr = "" Then
            'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
            FenSfao.DureeMaxPresenceDepassee(MsgErr, afficheMsg)
            If MsgErr = "" Then
                'si ok on vérifie si opérateur est en opération hors OF
                FenSfao.OpHof(matr, MsgErr)
                If MsgErr = "" Then
                    'si ok on vérifie si l'opérateur a déjà une opération en cours
                    FenSfao.OFOpMatr(matr, TextBoxOF.Text, MaskedTextBoxOP.Text, MsgErr)
                End If
            End If
        End If
    End Sub

    Private Sub MTextBoxMatr_Validated(sender As Object, e As EventArgs) Handles MTextBoxMatr.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxMatr, "")
        TextBoxMsg.Text = ""
    End Sub

    Private Sub BtnFin_Click(sender As Object, e As EventArgs) Handles BtnFin.Click
        Me.Close()
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        Dim retMsg As String = String.Empty
        Dim debop As Integer = -1

        'on déclenche la validation de la zone opération soldée
        If ComboBoxSoldOp.Text = "" Then
            ComboBoxSoldOp.Select()
            Exit Sub
        End If

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next

        'TODO WEB : si cde/appel et opération soldée, contrôle du nombre de palettes produites

        'tout va bien on enregistre le début d'opération + suivi auto du temps passé depuis le dernier évenement
        Try
            Trace("Appel du web service WSFINOPE")
            debop = X3ws.WSFINOPE(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), CInt(Me.Tag), ComboBoxSoldOp.Text, ComboBoxMotifNS.Text, retMsg)
        Catch ex As Exception
            GoTo ErreurFinop
        End Try

        Select Case debop
            Case -1 'Erreur du web service
                GoTo ErreurFinop
            Case 0 'Erreur blocage 
                Trace(retMsg, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
            Case 1 'ok
                Me.DialogResult = DialogResult.OK
        End Select

        Exit Sub

ErreurFinop:
        Trace("Erreur d'enregistrement du début d'opération ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        Me.DialogResult = DialogResult.Abort
        Me.Close()
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
            ErrorProvider.SetError(ComboBoxMotifNS, "Liste motifs")
        End If
    End Sub

    Private Sub ComboBoxMotifNS_Validated(sender As Object, e As EventArgs) Handles ComboBoxMotifNS.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(ComboBoxMotifNS, "")
        TextBoxMsg.Text = ""
    End Sub
End Class