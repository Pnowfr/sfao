'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'
'------------------------------------------------------------------------------------------------------------------------
Imports System.ComponentModel
Public Class SOROP

    Private Sub SOROP_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty

        Trace("Affichage fenêtre SOROP")

        'On rafraichit la Situation 
        Call FenSfao.Situation()

        'Si un seul opérateur présent sur le poste on prérempli le code matricule + nom et on calcule le temps de présence (contrairement à la V6 on peut faire une sortie de n'importe quel type d'opérateur)
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

        If MTextBoxMatr.Text <> "" Then
            'on contrôle le matricule et on calcule le temps de présence
            CtrlMatr(CInt(MTextBoxMatr.Text), MsgErr)
            If MsgErr <> "" Then
                TextBoxMsg.Text = MsgErr
                ErrorProvider.SetError(MTextBoxMatr, MsgErr)
                System.Media.SystemSounds.Exclamation.Play()
            End If
        End If

        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            MTextBoxMatr.Enabled = False
        Else
            'MTextBoxMatr.Text = ""
            'TextBoxNom.Text = ""
        End If

        'initialisation de la taille de police pour la gestion d'agrandissement de la fenêtre
        zTailFnt = FenSfao.TaillePolice(18, 9)
        fnt = New Font("Microsoft Sans Serif", zTailFnt, FontStyle.Regular)
        Me.Font = fnt

        'Sélection auto du 1er champ saisi
        MTextBoxMatr.Select()

    End Sub
    'Fonction qui gère le changement de taille des polices en fonction de la taille de la fenêtre
    Private Sub SOROP_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
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
    'Méthode qui contrôle la validité du matricule saisi
    Private Sub MTextBoxMatr_Validating(sender As Object, e As CancelEventArgs) Handles MTextBoxMatr.Validating
        Dim MsgErr As String = ""

        TextBoxTps.Tag = ""
        TextBoxNom.Text = ""

        If MTextBoxMatr.Text = "" Then
            'si le matricule est vide : erreur
            e.Cancel = True 'zone invalide
            MsgErr = "Veuillez saisir ou scanner le code de votre matricule"
            ErrorProvider.SetError(MTextBoxMatr, MsgErr)
            TextBoxMsg.Text = MsgErr
        Else
            'On contrôle le matricule saisi 
            CtrlMatr(CInt(MTextBoxMatr.Text), MsgErr)

            'en cas d'erreur on déclare l'erreur sur le ErrorProvider
            If MsgErr <> "" Then
                Trace("[MTextBoxMatr_Validating] : " & MsgErr)
                e.Cancel = True 'zone invalide
                MTextBoxMatr.Select(0, MTextBoxMatr.Text.Length) 'sélection du texte saisi
                ErrorProvider.SetError(MTextBoxMatr, MsgErr)
                TextBoxMsg.Text = MsgErr
                System.Media.SystemSounds.Exclamation.Play()
            End If

        End If
    End Sub

    'fonction qui contrôle le matricule saisi et calcul le temps de présence
    Private Sub CtrlMatr(ByVal matr As Integer, ByRef MsgErr As String)
        Dim TpsPres As Integer
        Dim xevent As Integer

        'On vérifie la présence de l'opérateur et la durée 
        FenSfao.CtrlMatrDuree(matr, MsgErr, TextBoxNom.Text, TpsPres)
        If MsgErr = "" Then
            TextBoxTps.Text = AFF_TPS_JHM(TpsPres)
            'si tout est ok on vérifie si un événement en cours bloque la sortie 
            'TODO PNO : voir quel événement peuvent bloquer la sortie
            'FenSfao.EventEnCoursSortie(matr, xevent, MsgErr)
            TextBoxTps.Tag = xevent
        End If

    End Sub

    'Méthode qui contrôle si pour le patricule saisi on doit activer la copie ou le chevauchement
    Private Sub MTextBoxMatr_Validated(sender As Object, e As EventArgs) Handles MTextBoxMatr.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxMatr, "")
        'TextBoxMsg.Text = ""
        Application.DoEvents()
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        Dim retMsg As String = ""
        Dim Result As DialogResult = DialogResult.OK

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next

        'affichage le load dans 100 ms
        Call FenSfao.WaitGif(True, 100)

        Trace("Code Matricule saisi : " & MTextBoxMatr.Text)
        Trace("Nom : " & TextBoxNom.Text)

        'on vérifie si on doit déclencher une fin d'un événement en cours
        If CInt(TextBoxTps.Tag) > 0 Then
            Result = Evenements.AfficheEvt(CInt(TextBoxTps.Tag), "F", CInt(Me.Tag)) 'appel de la fonction d'affichage des fin d'événements
        End If

        If Result = DialogResult.OK Then
            'fin de l'événement en cours ou aucune fin nécessaire on lance la sortie de l'opérateur
            FenSfao.Etat("Enregistrement du départ opérateur", 1, 3000)

            Try
                If X3ws.WSSorOp(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), CInt(Me.Tag), retMsg) = True Then
                    Me.DialogResult = DialogResult.OK
                End If
            Catch ex As Exception
                GoTo ErreurSorop
            End Try

            If Me.DialogResult = DialogResult.OK Then
                Trace("Sortie opérateur réalisée sans erreur.")
                Me.Close()
            Else
                GoTo ErreurSorop
            End If
        Else
            'la fin de l'événement en cours n'a pas été validée on ne peut pas faire la sortie opérateur
            Trace("Enregistrement du départ impossible ! Vous devez d'abord terminer l'événement en cours. ", FichierTrace.niveau.avertissement)
            'On masque le load dans 0.5s
            Call FenSfao.WaitGif(False, 500)
        End If
        Exit Sub

ErreurSorop:
        Trace("Erreur d'enregistrement du départ ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        Me.DialogResult = DialogResult.Abort
        Me.Close()
        'On masque le load dans 0.5s
        Call FenSfao.WaitGif(False, 500)
    End Sub

End Class