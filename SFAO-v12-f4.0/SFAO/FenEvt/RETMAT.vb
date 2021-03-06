'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class RETMAT
    Dim numof As String = String.Empty
    Dim numop As String = String.Empty
    Dim qteatl As Decimal
    Dim qtemac As Decimal
    Dim qtecso As Decimal
    Dim qteori As Decimal

    Private Sub RETMAT_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty

        Trace("Affichage fenêtre RETMAT")

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
    Private Sub RETMAT_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
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

    Private Sub Aff_ITM(ByRef itmref As String)
        Dim i As Integer

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
        Else
            TextBoxDesign.Text = ""
            TextBoxUnité.Text = ""
        End If
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
        Dim retmat As Integer
        Dim msgRéint As String
        Dim result As MsgBoxResult
        Dim qteret As Decimal

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
            ErrorProvider.SetError(MTextBoxQté, "Quantité retournée obligatoire")
            MTextBoxQté.Select()
            Exit Sub
        End If
        qteret = CDec(MTextBoxQté.Text)
        If qteret > qtemac + qteatl + qtecso Then
            msgRéint = "La quantité saisie (" + qteret.ToString("### ### ###") + " " + TextBoxUnité.Text + ") est supérieure à la quantité disponible (stock + consommation)."
            Trace(msgRéint)
            If qtemac + qteatl > 0 Then
                qteret -= qtemac + qteatl
            End If
            msgRéint += vbNewLine + "Confirmez-vous la réintégration de " + qteret.ToString("### ### ###") + " " + TextBoxUnité.Text
            result = MsgBox(msgRéint, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
            If result = MsgBoxResult.No Then
                ErrorProvider.SetError(MTextBoxQté, "Quantité supérieure à la quantité disponible")
                MTextBoxQté.Select()
                Exit Sub
            End If
        End If

        'affichage le load dans 100 ms
        Call FenSfao.WaitGif(True, 100)

        Try
            Trace("Appel du web service WSRETMAT")
            retmat = X3ws.WSRETMAT(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), TextBoxArt.Text, ComboBoxSup.Text, TextBoxLot.Text,
                                   SFAO.Poste.GRP1.STOLOC, SFAO.Poste.GRP1.ZATELOC, CDec(MTextBoxQté.Text), qtemac, qteatl, qtecso, TextBoxUnité.Text, retMsg)
        Catch ex As Exception
            Trace("Exception à l'appel du web service WSRETMAT")
            retMsg = "Erreur de transfert du lot " + TextBoxLot.Text
            GoTo ErreurRetmat
        End Try
        If retmat <> 1 Then
            GoTo ErreurRetmat
        End If

        'Tout est ok
        Me.DialogResult = DialogResult.OK

        Exit Sub

ErreurRetmat:
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
        Dim MsgErr As String
        Dim ctrlot As Integer
        Dim retMsg As String = String.Empty

        If TextBoxLot.Text = "" Then
            'si le lot est vide : erreur
            e.Cancel = True 'zone invalide
            MsgErr = "Veuillez saisir ou scanner le lot de la matière"
            ErrorProvider.SetError(TextBoxLot, MsgErr)
            TextBoxMsg.Text = MsgErr
        Else
            'On remet les champs à récupérer du lot à vide (en cas d'erreur du web service)
            MTextBoxQté.Text = ""
            MTextBoxQtéTh.Text = ""
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
                    MsgErr = retMsg
                    GoTo ErreurTextBoxLot
                Case 0 'Lot inexistant ou non disponible
                    MsgErr = retMsg
                    GoTo ErreurTextBoxLot
                Case 1 'ok

            End Select

        End If

        Exit Sub

ErreurTextBoxLot:
        If MsgErr <> "" Then
            Trace(MsgErr, FichierTrace.niveau.erreur)
        End If
        e.Cancel = True 'zone invalide
        ErrorProvider.SetError(TextBoxLot, MsgErr)
        TextBoxMsg.Text = MsgErr
        System.Media.SystemSounds.Exclamation.Play() 'son erreur
        TextBoxLot.Select() 'sélection du contrôle
    End Sub

    Private Sub TextBoxLot_Validated(sender As Object, e As EventArgs) Handles TextBoxLot.Validated
        Dim qte As Decimal

        Aff_ITM(TextBoxArt.Text)
        qte = qtemac + qteatl
        MTextBoxQtéTh.Text = qte.ToString("### ### ###")

        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextBoxLot, "")
        TextBoxMsg.Text = ""
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
            If FenSfao.CatArtCp(numof, CInt(numop), TextBoxArt.Text) = "EP" Then
                'Pour les encours, on ne peut retourner que la quantité sur l'emplacement machine
                If CDec(MTextBoxQté.Text) > qtemac Then
                    MsgErr = "Quantité supérieure au stock disponible (" + qtemac.ToString("### ### ###") + " " + TextBoxUnité.Text + ")"
                    e.Cancel = True 'zone invalide
                    ErrorProvider.SetError(MTextBoxQté, MsgErr)
                    TextBoxMsg.Text = MsgErr
                    System.Media.SystemSounds.Exclamation.Play() 'son erreur
                    MTextBoxQté.Select() 'sélection du contrôle
                End If
            End If
            'Contrôle que la quantité est inférieure à la quantité de création du lot
            qte = qtemac + qteatl 'On doit pouvoir retourner au moins ce qui est en stock
            If qteori > 0 AndAlso CDec(MTextBoxQté.Text) > qteori AndAlso CDec(MTextBoxQté.Text) > qte Then
                MsgErr = "Quantité supérieure à la quantité d'origine du lot ! (" + qteori.ToString("### ### ###") + " " + TextBoxUnité.Text + ")"
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
End Class