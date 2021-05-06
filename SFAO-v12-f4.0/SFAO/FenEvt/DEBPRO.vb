'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout du séparateur //
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class DEBPRO
    Private ofop As WSOFOPInfo
    Public WSLstTypEtq As New WSTypEtq            'classe de la liste des types d'étiquettes

    Private Sub DEBPRO_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty
        Dim retMsg As String = String.Empty

        Trace("Affichage fenêtre DEBPRO")

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

        If MsgErr = "" Then
            'Récupération de la liste des types d'étiquettes
            Try
                Trace("Appel du web service WSGETTETQ")
                WSLstTypEtq = X3ws.WSGETTETQ("P")
                If WSLstTypEtq.GRP1.ZRET = 1 Then
                    For i = 0 To WSLstTypEtq.GRP2.Count - 1
                        ComboBoxTypEtq.Items.Add(WSLstTypEtq.GRP2(i).LNGDES)
                    Next i
                Else
                    MsgErr = WSLstTypEtq.GRP1.ZMSG
                    TextBoxMsg.Text = MsgErr
                    ErrorProvider.SetError(ComboBoxTypEtq, MsgErr)
                    System.Media.SystemSounds.Exclamation.Play()
                End If

            Catch ex As Exception
                MsgErr = "Erreur de récupération de la liste de types d'étiquettes ! "
                TextBoxMsg.Text = MsgErr
                ErrorProvider.SetError(ComboBoxTypEtq, MsgErr)
                System.Media.SystemSounds.Exclamation.Play()
            End Try

        End If

        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            MTextBoxMatr.Enabled = False    'on a 1 seul opérateur présent sur le poste, donc on désactive la saisie du champ matricule
            'Sélection auto du 1er champ saisi

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
    Private Sub DEBPRO_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
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
                    If MsgErr = "" Then
                        FenSfao.EventOblig(matr, MsgErr)
                        If MsgErr = "" Then
                            Infos_OFOP(TextBoxOF.Text, CInt(MaskedTextBoxOP.Text), MsgErr)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    'fonction qui remplit les infos à partir de l'OF/opération
    Private Sub Infos_OFOP(ByRef numof As String, ByRef numop As Integer, ByRef MsgErr As String)
        Dim lsttypop As String = "BOB,EMB,FAC"
        Dim saipds As Integer
        Dim retMsg As String = String.Empty
        Dim repdef As String = String.Empty

        TextBoxUOM.Text = FenSfao.UnitFab(numof, numop)
        'Si étape de production et bobinage/emballage/façonnage : saisie du poids
        If FenSfao.EtapePro(numof, numop) = True AndAlso lsttypop.IndexOf(Strings.Left(SFAO.Poste.GRP1.Y_TYPOP, 3)) <> -1 Then
            If SFAO.Poste.GRP1.Y_TYPOP = "EMB" AndAlso TextBoxUOM.Text = "UN" Then
                'Sauf à l'emballage pour les feuilles
                LabelSaiPds.Visible = False
                ComboBoxSaiPds.Enabled = False
                ComboBoxSaiPds.Visible = False
            Else 'Contrôle de l'article produit
                Try
                    Trace("Appel du web service WSSAIPDS")
                    saipds = X3ws.WSSAIPDS(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), repdef, retMsg)
                Catch ex As Exception
                    Trace("Exception à l'appel du web service WSSAIPDS")
                    MsgErr = "Erreur au contrôle de la saisie du poids"
                End Try

                Select Case saipds
                    Case -1 'Erreur du web service
                        Trace(retMsg)
                        MsgErr = retMsg
                    Case 0 'Erreur blocage 
                        Trace(retMsg)
                        MsgErr = retMsg
                    Case 1 'ok
                        If repdef = "" Then
                            LabelSaiPds.Visible = False
                            ComboBoxSaiPds.Enabled = False
                            ComboBoxSaiPds.Visible = False
                        Else
                            ComboBoxSaiPds.Text = repdef
                        End If
                End Select
            End If
        Else
            LabelSaiPds.Visible = False
            ComboBoxSaiPds.Enabled = False
            ComboBoxSaiPds.Visible = False
        End If
    End Sub

    Private Sub MTextBoxMatr_Validated(sender As Object, e As EventArgs) Handles MTextBoxMatr.Validated
        Dim MsgErr As String = String.Empty

        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxMatr, "")
        TextBoxMsg.Text = ""

    End Sub

    Private Sub BtnFin_Click(sender As Object, e As EventArgs) Handles BtnFin.Click
        Me.Close()
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        Dim retMsg As String = String.Empty
        Dim finop As Integer = -1
        Dim result As MsgBoxResult
        result = MsgBoxResult.Ok

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next

        'tout va bien on enregistre la fin d'opération + suivi auto du temps passé depuis le dernier évenement
        Try
            Trace("Appel du web service WSFINOPE")
            finop = X3ws.WSFINOPE(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), CInt(Me.Tag), ComboBoxSaiPds.Text, ComboBoxTypEtq.Text, retMsg)
        Catch ex As Exception
            GoTo ErreurFinop
        End Try

        Select Case finop
            Case -1 'Erreur du web service
                GoTo ErreurFinop
            Case 0 'Erreur blocage 
                Trace(retMsg, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
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
    End Sub

    Private Sub ComboBoxSaiPds_GotFocus(sender As Object, e As EventArgs) Handles ComboBoxSaiPds.GotFocus
        TextBoxMsg.Text = "Saisie du poids de bobines ? OUI / NON"
    End Sub

    Private Sub ComboBoxSaiPds_Validating(sender As Object, e As CancelEventArgs) Handles ComboBoxSaiPds.Validating
        If ComboBoxSaiPds.Text = "" Then
            e.Cancel = True
            ErrorProvider.SetError(ComboBoxSaiPds, "Oui/Non")
            ComboBoxSaiPds.Select()
        End If
    End Sub

    Private Sub ComboBoxSaiPds_Validated(sender As Object, e As EventArgs) Handles ComboBoxSaiPds.Validated

        'on efface les erreurs précédentes
        ErrorProvider.SetError(ComboBoxSaiPds, "")
        TextBoxMsg.Text = ""
        ComboBoxTypEtq.Select()
    End Sub

    Private Sub ComboBoxTypEtq_Validating(sender As Object, e As CancelEventArgs) Handles ComboBoxTypEtq.Validating
        If ComboBoxTypEtq.Text = "" Then
            e.Cancel = True
            ErrorProvider.SetError(ComboBoxTypEtq, "Motif de non solde obligatoire")
            ComboBoxTypEtq.Select()
        End If
    End Sub

    Private Sub ComboBoxTypEtq_Validated(sender As Object, e As EventArgs) Handles ComboBoxTypEtq.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(ComboBoxTypEtq, "")
        TextBoxMsg.Text = ""
    End Sub

End Class