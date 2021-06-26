'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class CONSO
    Dim numof As String = String.Empty
    Dim numop As String = String.Empty
    Dim qteatl As Decimal
    Dim qtemac As Decimal

    Private Sub CONSO_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty

        Trace("Affichage fenêtre CONSO")

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

        'Sélection auto du 1er champ saisi
        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            MTextBoxMatr.Enabled = False
            TextBoxLot.Select()
        Else
            MTextBoxMatr.Select()
        End If

        'initialisation de la taille de police pour la gestion d'agrandissement de la fenêtre
        zTailFnt = FenSfao.TaillePolice(18, 9)
        fnt = New Font("Microsoft Sans Serif", zTailFnt, FontStyle.Regular)
        Me.Font = fnt

    End Sub

    'Fonction qui gère le changement de taille des polices en fonction de la taille de la fenêtre
    Private Sub CONSO_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
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
                FenSfao.OFOpMatr(matr, numof, numop, MsgErr)
                If MsgErr = "" Then
                    'si ok on vérifie s'il y a un événement (phase) obligatoire
                    FenSfao.EventOblig(matr, MsgErr)
                End If
            End If
        End If
    End Sub

    Private Sub Aff_ITM(ByRef itmref As String, ByRef MsgErr As String)
        Dim i As Integer
        Dim artinfo As Integer
        Dim retMsg As String = String.Empty
        Dim catart As String = String.Empty

        TextBoxDesign.Text = ""
        TextBoxUnité.Text = ""
        ComboBoxSup.Items.Clear()
        If itmref <> "" Then
            For i = 0 To FenSfao.WScp.GRP2.Count - 1
                If FenSfao.WScp.GRP2(i).ZITMREF = itmref Then
                    TextBoxDesign.Text = FenSfao.WScp.GRP2(i).ZITMDES
                    TextBoxUnité.Text = FenSfao.WScp.GRP2(i).ZUOM
                    If FenSfao.WScp.GRP2(i).ZSUPGRP <> "" Then
                        ComboBoxSup.Items.Add(FenSfao.WScp.GRP2(i).ZSUPGRP)
                    End If
                End If
            Next
            If TextBoxDesign.Text = "" Then
                Trace("Recherche des informations de la matière non prévue : " + itmref)
                Try
                    Trace("Appel du web service WSARTINFO")
                    artinfo = X3ws.WSARTINFO(itmref, TextBoxDesign.Text, catart, TextBoxUnité.Text, retMsg)
                Catch ex As Exception
                    Trace("Exception à l'appel du web service WSQTECPLOT")
                    MsgErr = "Erreur au contrôle du lot"
                    Exit Sub
                End Try
                Select Case artinfo
                    Case -1 'Erreur du web service
                        Trace(retMsg)
                        MsgErr = retMsg
                    Case 0 'Lot inexistant ou non disponible
                        Trace(retMsg)
                        MsgErr = retMsg
                    Case 1 'ok
                        LabelSup.Text = ""
                        ComboBoxSup.Enabled = False
                End Select
            Else
                LabelSup.Text = "N° support"
                'S'il y a plusieurs supports/groupes pour le même article, on active le champ
                Select Case ComboBoxSup.Items.Count
                    Case 0
                        ComboBoxSup.Enabled = False
                    Case 1
                        ComboBoxSup.SelectedIndex = 0
                        ComboBoxSup.Enabled = False
                    Case Else
                        ComboBoxSup.Enabled = True
                End Select
            End If
        Else
            LabelSup.Text = "N° support"
            ComboBoxSup.Enabled = False
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
        Dim conso As Integer

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next

        If TextBoxLot.Text = "" Then
            ErrorProvider.SetError(TextBoxLot, "N° lot/sous-lot obligatoire")
            TextBoxLot.Select()
            Exit Sub
        ElseIf ComboBoxSup.Items.Count > 0 AndAlso ComboBoxSup.Text = "" Then
            ErrorProvider.SetError(ComboBoxSup, "N° support/groupe obligatoire")
            ComboBoxSup.Select()
            Exit Sub
        ElseIf MTextBoxQté.Text = "" Then
            ErrorProvider.SetError(MTextBoxQté, "Quantité à consommer obligatoire")
            MTextBoxQté.Select()
            Exit Sub
        End If

        'affichage le load dans 100 ms
        Call FenSfao.WaitGif(True, 100)

        Try
            Trace("Appel du web service WSCONSO")
            conso = X3ws.WSCONSO(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), TextBoxArt.Text, ComboBoxSup.Text, TextBoxLot.Text,
                                 SFAO.Poste.GRP1.STOLOC, SFAO.Poste.GRP1.ZATELOC, CDec(MTextBoxQté.Text), qteatl, TextBoxUnité.Text, TextBoxLotF.Text, TextBoxTrk.Text, retMsg)
        Catch ex As Exception
            Trace("Exception à l'appel du web service WSCONSO")
            retMsg = "Erreur de consommation du lot " + TextBoxLot.Text
            GoTo ErreurConso
        End Try
        If conso <> 1 Then
            GoTo ErreurConso
        End If

        'Tout est ok
        Me.DialogResult = DialogResult.OK

        Exit Sub

ErreurConso:
        Trace("Erreur d'enregistrement de matière utilisée ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        Me.DialogResult = DialogResult.Abort
        Me.Close()
        'On masque le load dans 0.5s
        Call FenSfao.WaitGif(False, 500)
    End Sub

    Private Sub TextBoxLot_Validating(sender As Object, e As CancelEventArgs) Handles TextBoxLot.Validating
        Dim MsgErr As String = String.Empty
        Dim ctrlot As Integer
        Dim retMsg As String = String.Empty
        Dim qtecso As Decimal
        Dim qteori As Decimal

        If TextBoxLot.Text = "" Then
            'si le lot est vide : erreur
            e.Cancel = True 'zone invalide
            MsgErr = "Veuillez saisir ou scanner le lot de la matière"
            ErrorProvider.SetError(TextBoxLot, MsgErr)
            TextBoxMsg.Text = MsgErr
        Else
            'On remet les champs à récupérer du lot à vide (en cas d'erreur du web service)
            MTextBoxQté.Text = ""
            qteatl = 0
            qtemac = 0
            qtecso = 0
            qteori = 0

            'On contrôle si le lot existe et on sélectionne le code article si besoin
            'si oui, la quantité disponible
            Trace("Vérification de l'existence du lot : " + TextBoxLot.Text)
            Try
                Trace("Appel du web service WSQTECPLOT")
                ctrlot = X3ws.WSQTECPLOT(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), CInt(Me.Tag), TextBoxArt.Text, TextBoxLot.Text,
                                         SFAO.Poste.GRP1.STOLOC, SFAO.Poste.GRP1.ZATELOC, qtemac, qteatl, qtecso, qteori, retMsg)
            Catch ex As Exception
                Trace("Exception à l'appel du web service WSQTECPLOT")
                MsgErr = "Erreur au contrôle du lot"
                GoTo ErreurTextBoxLot
            End Try

            Select Case ctrlot
                Case -1 'Erreur du web service
                    Trace(retMsg)
                    MsgErr = retMsg
                    GoTo ErreurTextBoxLot
                Case 0 'Lot inexistant ou non disponible
                    Trace(retMsg)
                    MsgErr = retMsg
                    GoTo ErreurTextBoxLot
                Case 1 'ok
                    Aff_ITM(TextBoxArt.Text, MsgErr)
                    If MsgErr <> "" Then
                        GoTo ErreurTextBoxLot
                    End If
            End Select

        End If

        Exit Sub

ErreurTextBoxLot:
        e.Cancel = True 'zone invalide
        ErrorProvider.SetError(TextBoxLot, MsgErr)
        TextBoxMsg.Text = MsgErr
        System.Media.SystemSounds.Exclamation.Play() 'son erreur
        TextBoxLot.Select() 'sélection du contrôle
    End Sub

    Private Sub TextBoxLot_Validated(sender As Object, e As EventArgs) Handles TextBoxLot.Validated
        Dim qte As Decimal

        'On remplit par défaut la quantité
        qte = qtemac + qteatl
        If qte > 0 Then
            MTextBoxQté.Text = qte.ToString("### ### ###")
        Else
            MTextBoxQté.Text = ""
        End If
        If LabelSup.Text = "" Then
            TextBoxMsg.Text = "ATTENTION !" + vbNewLine + "Matière non prévue pour l'opération en cours"
        Else
            TextBoxMsg.Text = ""
        End If
        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextBoxLot, "")
    End Sub

    Private Sub ComboBoxSup_Validating(sender As Object, e As CancelEventArgs) Handles ComboBoxSup.Validating
        If ComboBoxSup.Text = "" Then
            e.Cancel = True 'zone invalide
            ErrorProvider.SetError(ComboBoxSup, "N° support/groupe obligatoire")
            TextBoxMsg.Text = "N° support/groupe obligatoire"
            System.Media.SystemSounds.Exclamation.Play() 'son erreur
            ComboBoxSup.Select() 'sélection du contrôle
        End If
    End Sub

    Private Sub ComboBoxSup_Validated(sender As Object, e As EventArgs) Handles ComboBoxSup.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(ComboBoxSup, "")
        TextBoxMsg.Text = ""
    End Sub

    Private Sub MTextBoxQté_Validating(sender As Object, e As CancelEventArgs) Handles MTextBoxQté.Validating
        Dim MsgErr As String
        Dim qte As Decimal
        If MTextBoxQté.Text <> "" Then
            qte = qtemac + qteatl
            'Contrôle si quantitée saisie supérieure au stock disponible
            If CDec(MTextBoxQté.Text) > qte Then
                MsgErr = "Quantité supérieure au stock disponible (" + qte.ToString("### ### ###") + " " + TextBoxUnité.Text + ")"
                e.Cancel = True 'zone invalide
                ErrorProvider.SetError(MTextBoxQté, MsgErr)
                TextBoxMsg.Text = MsgErr
                System.Media.SystemSounds.Exclamation.Play() 'son erreur
                MTextBoxQté.Select() 'sélection du contrôle
            End If
        End If
    End Sub

    Private Sub MTextBoxQté_Validated(sender As Object, e As EventArgs) Handles MTextBoxQté.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxQté, "")
        TextBoxMsg.Text = ""
    End Sub

    Private Sub TextBoxLotF_Validating(sender As Object, e As CancelEventArgs) Handles TextBoxLotF.Validating
        Dim MsgErr As String
        Dim lot As String = String.Empty
        Dim slo As String = String.Empty
        Dim lotvcr As Integer
        Dim retMsg As String = String.Empty

        TextBoxTrk.Text = ""
        If TextBoxLotF.Text <> "" Then
            If TextBoxLotF.TextLength < 5 Then
                TextBoxLotF.Text = FenSfao.LotFab(numof, CInt(numop), CInt(TextBoxLotF.Text))
            Else
                If TextBoxLotF.TextLength > 5 Then
                    FenSfao.LotSlo(TextBoxLotF.Text, lot, slo)
                    If FenSfao.CtrlLotOFOp(numof, CInt(numop), lot) = False Then
                        MsgErr = "Le lot ne correspond pas à l'opération en cours"
                        GoTo ErreurTextBoxLotF
                    End If
                Else
                    MsgErr = "Lot incorrect"
                    GoTo ErreurTextBoxLotF
                End If
            End If
            'Recherche du n° de suivi de fabrication
            Try
                Trace("Appel du web service WSLOTBPS")
                lotvcr = X3ws.WSVCRNLOT(TextBoxArt.Text, TextBoxLot.Text, TextBoxTrk.Text, retMsg)
            Catch ex As Exception
                Trace("Exception à l'appel du web service WSLOTBPS")
                MsgErr = "Erreur à la recherche du lot fournisseur"
                GoTo ErreurTextBoxLotF
            End Try
            If lotvcr = -1 Then 'Erreur du web service
                Trace(retMsg)
                MsgErr = retMsg
                GoTo ErreurTextBoxLotF
            ElseIf Strings.Left(TextBoxTrk.Text, 3) <> "MTK" Then
                MsgErr = "N° de déclaration de production non trouvé"
                GoTo ErreurTextBoxLotF
            End If
        End If

        Exit Sub

ErreurTextBoxLotF:
        e.Cancel = True 'zone invalide
        ErrorProvider.SetError(TextBoxLotF, MsgErr)
        TextBoxMsg.Text = MsgErr
        System.Media.SystemSounds.Exclamation.Play() 'son erreur
        TextBoxLotF.Select() 'sélection du contrôle
    End Sub

    Private Sub TextBoxLotF_Validated(sender As Object, e As EventArgs) Handles TextBoxLotF.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextBoxLotF, "")
        TextBoxMsg.Text = ""
    End Sub
End Class