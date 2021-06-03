'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout du séparateur //
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class MATUTL

    Private Sub MATUTL_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty

        Trace("Affichage fenêtre MATUTL")

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

        'contrôle si saisie du n° lot fournisseur
        If SFAO.Poste.GRP1.ZNUMFRN <> 2 Then
            'champs de saisie lot / sous-lot fournisseur invisibles
            LabelLotF.Visible = False
            TextBoxLotF.Visible = False
            '(lot fournisseur inactif déjà non saisissable)
            LabelSloF.Visible = False
            TextBoxSloF.Visible = False
            'on désactive sous-lot/n° bobine fournisseur
            TextBoxSloF.Enabled = False
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
    Private Sub MATUTL_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
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
        Dim numof As String = String.Empty
        Dim numop As String = String.Empty

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
                    If MsgErr = "" Then
                        'si ok on liste les matières
                        List_Mat(numof, CInt(numop), MsgErr, afficheMsg)
                    End If
                End If
            End If
        End If
    End Sub

    'fonction qui liste les matières/composants
    Private Sub List_Mat(ByVal nof As String, ByVal nop As Integer, ByRef MsgErr As String, Optional ByVal afficheMsg As Boolean = True)
        Dim nvlmat As Integer = -1
        Dim retMsg As String = String.Empty
        Dim i As Integer

        'On vide la combo, si elle a déjà été remplie à la saisie d'un autre matricule
        ComboBoxArt.Items.Clear()

        'On contrôle si des matières ont été ajoutées à l'OF (avec màj de la situation matières si besoin)
        Try
            Trace("Appel du web service WSCTRNVMAT")
            nvlmat = X3ws.WSCTRNVMAT(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), retMsg)
        Catch ex As Exception
            Trace("Exception à l'appel du web service WSCTRNVMAT")
            MsgErr = "Erreur au contrôle de la saisie du poids"
            Exit Sub
        End Try

        Select Case nvlmat
            Case -1 'Erreur du web service
                Trace(retMsg)
                MsgErr = retMsg
            Case 1 'ok
                Trace("Aucun ajout de matière sur l'OF " + nof)
            Case 2 'màj matières OF
                Trace("Une ou plusieurs matières ont été ajoutées sur l'OF " + nof)
                'On met à jour la classe de situation des composants
                FenSfao.SituationCps()
        End Select

        'On remplit la combo des articles
        If FenSfao.WScp.GRP2.Count > 0 Then
            For i = 0 To FenSfao.WScp.GRP2.Count - 1
                If FenSfao.WScp.GRP2(i).XMFGNUM = nof AndAlso FenSfao.WScp.GRP2(i).XOPENUM = nop Then
                    ComboBoxArt.Items.Add(FenSfao.WScp.GRP2(i).ZITMREF)
                End If
            Next
        End If
        Select Case ComboBoxArt.Items.Count
            Case 0
                MsgErr = "Aucune matière prévue pour cette opération sur l'OF " + nof
                ComboBoxArt.Enabled = False
            Case 1
                ComboBoxArt.Enabled = False
                ComboBoxArt.SelectedIndex = 0
                Aff_ITM(ComboBoxArt.Text, MsgErr)
            Case Else
                'Si plusieurs matières, on ne remplit pas automatiquement les champs article/désignation/unité
                TextBoxDesign.Text = ""
                TextBoxUnité.Text = ""
                ComboBoxArt.Enabled = True
        End Select

    End Sub

    Private Sub Aff_ITM(ByRef art As String, ByRef MsgErr As String)
        Dim i As Integer
        Dim ctrlot As Integer = -1
        Dim lotbps As Integer = -1
        Dim retMsg As String = String.Empty
        Dim qte As Decimal

        If art <> "" Then
            For i = 0 To FenSfao.WScp.GRP2.Count - 1
                If FenSfao.WScp.GRP2(i).ZITMREF = art Then
                    TextBoxDesign.Text = FenSfao.WScp.GRP2(i).ZITMDES
                    TextBoxUnité.Text = FenSfao.WScp.GRP2(i).ZUOM
                    'si lot vide et matière non support
                    If TextBoxLot.Text = "" AndAlso Strings.Left(FenSfao.WScp.GRP2(i).ZSUPGRP, 1) <> "S" AndAlso FenSfao.WScp.GRP2(i).ZTCLCOD <> "EP" Then
                        'Recherche : si un seul lot, on le remplit automatiquement
                        Try
                            Trace("Appel du web service WSCTRCPLOT")
                            ctrlot = X3ws.WSCTRCPLOT(SFAO.Site.GRP1.FCY, ComboBoxArt.Text, TextBoxLot.Text, SFAO.Poste.GRP1.ZATELOC, SFAO.Poste.GRP1.STOLOC, qte, retMsg)
                        Catch ex As Exception
                            Trace("Exception à l'appel du web service WSCTRNVMAT")
                            MsgErr = "Erreur au contrôle du lot"
                            Exit Sub
                        End Try
                        If ctrlot = -1 Then 'Erreur du web service
                            Trace(retMsg)
                            MsgErr = retMsg
                            Exit Sub
                        End If
                    End If

                    'Si support <> EP, SF, SI : saisie n° bobine fournisseur
                    If SFAO.Poste.GRP1.ZNUMFRN = 2 Then
                        If Strings.Left(FenSfao.WScp.GRP2(i).ZSUPGRP, 1) = "S" AndAlso InStr("SF,SI", FenSfao.WScp.GRP2(i).ZTCLCOD) = 0 Then
                            LabelLotF.Visible = True
                            TextBoxLotF.Visible = True
                            LabelSloF.Visible = True
                            TextBoxSloF.Visible = True
                            TextBoxSloF.Enabled = True
                            TextBoxLotF.Text = ""
                            TextBoxSloF.Text = ""
                            If TextBoxLot.Text <> "" Then
                                'Recherche du lot fournisseur
                                TextBoxLotF.Text = ""
                                TextBoxSloF.Text = ""
                                Try
                                    Trace("Appel du web service WSLOTBPS")
                                    ctrlot = X3ws.WSLOTBPS(ComboBoxArt.Text, TextBoxLot.Text, TextBoxLotF.Text, TextBoxSloF.Text)
                                Catch ex As Exception
                                    Trace("Exception à l'appel du web service WSLOTBPS")
                                    MsgErr = "Erreur à la recherche du lot fournisseur"
                                    Exit Sub
                                End Try
                                If ctrlot = -1 Then 'Erreur du web service
                                    Trace(retMsg)
                                    MsgErr = retMsg
                                    Exit Sub
                                End If
                            End If
                        Else
                            'champs de saisie lot / sous-lot fournisseur invisibles
                            LabelLotF.Visible = False
                            TextBoxLotF.Visible = False
                            '(lot fournisseur inactif déjà non saisissable)
                            LabelSloF.Visible = False
                            TextBoxSloF.Visible = False
                            'on désactive sous-lot/n° bobine fournisseur
                            TextBoxSloF.Enabled = False
                        End If
                    End If
                End If
            Next
        Else
            TextBoxDesign.Text = ""
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
        Else
            If ComboBoxArt.Text = "" Then
                ErrorProvider.SetError(ComboBoxArt, "Article obligatoire")
                ComboBoxArt.Select()
                Exit Sub
            End If
        End If

        'tout va bien on enregistre la conso des encres/vernis et la fin d'opération

        'affichage le load dans 100 ms
        Call FenSfao.WaitGif(True, 100)



        Exit Sub

ErreurMatutl:
        Trace("Erreur d'enregistrement de fin d'opération ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        Me.DialogResult = DialogResult.Abort
        Me.Close()
        'On masque le load dans 0.5s
        Call FenSfao.WaitGif(False, 500)
    End Sub

    Private Sub ComboBoxArt_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles ComboBoxArt.SelectionChangeCommitted
        Dim MsgErr As String = String.Empty
        Aff_ITM(ComboBoxArt.Text, MsgErr)
    End Sub

    Private Sub TextBoxLot_Validating(sender As Object, e As CancelEventArgs) Handles TextBoxLot.Validating
        Dim MsgErr As String = String.Empty
        Dim ctrlot As Integer = -1
        Dim retMsg As String = String.Empty
        Dim qte As Decimal

        If TextBoxLot.Text <> "" Then
            'On contrôle si le lot existe et, si oui, la quantité disponible
            Try
                Trace("Appel du web service WSCTRCPLOT")
                ctrlot = X3ws.WSCTRCPLOT(SFAO.Site.GRP1.FCY, ComboBoxArt.Text, TextBoxLot.Text, SFAO.Poste.GRP1.ZATELOC, SFAO.Poste.GRP1.STOLOC, qte, retMsg)
            Catch ex As Exception
                Trace("Exception à l'appel du web service WSCTRNVMAT")
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

                Case 2 'déjà monté sur la machine

            End Select

            Aff_ITM(ComboBoxArt.Text, MsgErr)
            MTextBoxQté.Text = qte.ToString("### ### ###")
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
        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextBoxLot, "")
        TextBoxLot.Text = ""
    End Sub
End Class