'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout du séparateur //
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class MATUTL
    Dim numof As String = String.Empty
    Dim numop As String = String.Empty
    Dim qtemax As Decimal
    Dim inutile As Boolean = False 'Si la matière est déjà montée sur la machine

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
            MatrOFOP_Valid(CInt(MTextBoxMatr.Text), MsgErr)
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
                    If MsgErr = "" Then
                        'si ok on liste les matières
                        List_Mat(numof, CInt(numop), MsgErr)
                    End If
                End If
            End If
        End If
    End Sub

    'fonction qui liste les matières/composants
    Private Sub List_Mat(ByVal nof As String, ByVal nop As Integer, ByRef MsgErr As String)
        Dim nvlmat As Integer
        Dim retMsg As String = String.Empty
        Dim i As Integer

        'On vide la combo, si elle a déjà été remplie à la saisie d'un autre matricule
        ComboBoxArt.Items.Clear()

        'On contrôle si des matières ont été ajoutées à l'OF (avec màj de la situation matières si besoin)
        Try
            Trace("Appel du web service WSCTRNVMAT")
            nvlmat = X3ws.WSCTRNVMAT(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), retMsg)
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
                    If InStr("S,G", Strings.Left(FenSfao.WScp.GRP2(i).ZSUPGRP, 1)) > 0 Then
                        ComboBoxArt.Items.Add(FenSfao.WScp.GRP2(i).ZITMREF + " - " + FenSfao.WScp.GRP2(i).ZSUPGRP)
                    Else
                        ComboBoxArt.Items.Add(FenSfao.WScp.GRP2(i).ZITMREF)
                    End If
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
        Dim ctrlot As Integer
        Dim lotbps As Integer
        Dim retMsg As String = String.Empty
        Dim qte As Decimal
        Dim itmref As String

        If art <> "" Then
            itmref = Strings.Split(art, " - ").First
            For i = 0 To FenSfao.WScp.GRP2.Count - 1
                If FenSfao.WScp.GRP2(i).ZITMREF = itmref Then
                    TextBoxDesign.Text = FenSfao.WScp.GRP2(i).ZITMDES
                    TextBoxUnité.Text = FenSfao.WScp.GRP2(i).ZUOM
                    'si lot vide et matière non support
                    If TextBoxLot.Text = "" AndAlso Strings.Left(FenSfao.WScp.GRP2(i).ZSUPGRP, 1) <> "S" AndAlso FenSfao.WScp.GRP2(i).ZTCLCOD <> "EP" Then
                        'Recherche : si un seul lot, on le remplit automatiquement
                        Try
                            Trace("Appel du web service WSCTRCPLOT")
                            ctrlot = X3ws.WSCTRCPLOT(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), itmref, TextBoxLot.Text,
                                                     SFAO.Poste.GRP1.ZATELOC, SFAO.Poste.GRP1.STOLOC, qte, retMsg)
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

                    'Affichage lot et n° bobine fournisseur
                    If SFAO.Poste.GRP1.ZNUMFRN = 2 Then
                        'Si support mais pas EP, SF, SI : saisie n° bobine fournisseur
                        If Strings.Left(FenSfao.WScp.GRP2(i).ZSUPGRP, 1) = "S" AndAlso InStr("SF,SI", FenSfao.WScp.GRP2(i).ZTCLCOD) = 0 Then
                            LabelLotF.Visible = True
                            TextBoxLotF.Visible = True
                            LabelSloF.Visible = True
                            TextBoxSloF.Visible = True
                            TextBoxSloF.Enabled = False
                            TextBoxLotF.Text = ""
                            TextBoxSloF.Text = ""
                            If TextBoxLot.Text <> "" Then
                                'Recherche du lot fournisseur
                                TextBoxLotF.Text = ""
                                TextBoxSloF.Text = ""
                                Try
                                    Trace("Appel du web service WSLOTBPS")
                                    lotbps = X3ws.WSGETLOTF(itmref, TextBoxLot.Text, TextBoxLotF.Text, TextBoxSloF.Text)
                                Catch ex As Exception
                                    Trace("Exception à l'appel du web service WSLOTBPS")
                                    MsgErr = "Erreur à la recherche du lot fournisseur"
                                    Exit Sub
                                End Try
                                If lotbps = -1 Then 'Erreur du web service
                                    Trace(retMsg)
                                    MsgErr = retMsg
                                    Exit Sub
                                ElseIf TextBoxLotF.Text <> "" AndAlso TextBoxSloF.Text = "" Then
                                    TextBoxSloF.Enabled = True
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
            TextBoxUnité.Text = ""
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
        Dim itmref As String
        Dim supgrp As String = String.Empty
        Dim nsup As Integer
        Dim lotReliq As WSLstLot
        Dim msgReliq As String
        Dim result As MsgBoxResult
        Dim csorlq As Integer
        Dim matutl As Integer
        Dim lotfrn As Integer
        Dim i As Integer

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
        ElseIf ComboBoxArt.Text = "" Then
            ErrorProvider.SetError(ComboBoxArt, "Article obligatoire")
            ComboBoxArt.Select()
            Exit Sub
        ElseIf MTextBoxQté.Text = "" Then
            ErrorProvider.SetError(MTextBoxQté, "Quantité utilisée obligatoire")
            MTextBoxQté.Select()
            Exit Sub
        End If

        'tout va bien on propose la consommation des éventuels reliquats et on enregistre la matière utilisée
        itmref = Strings.Split(ComboBoxArt.Text, " - ").First
        If Strings.InStr(ComboBoxArt.Text, " - ") > 0 Then
            supgrp = Strings.Right(ComboBoxArt.Text, 2)
            If Strings.Left(supgrp, 1) = "S" Then
                nsup = CInt(Strings.Right(supgrp, 1))
            End If
        ElseIf FenSfao.CatArtCp(numof, CInt(numop), ComboBoxArt.Text) = "EP" Then
            supgrp = "EP"
        End If

        'Recherche des reliquats (lot "entamé" et qté restante < 10 %) et proposition de les consommer automatiquement
        Trace("Recherche des reliquats")
        Try
            Trace("Appel du web service WSLOTRELIQ")
            lotReliq = X3ws.WSLOTRELIQ(SFAO.Site.GRP1.FCY, itmref, nsup, SFAO.Poste.GRP1.STOLOC, numof)
        Catch ex As Exception
            GoTo ErreurMatutl
        End Try

        Select Case lotReliq.GRP1.ZRET
            Case -1 'Erreur du web service
                GoTo ErreurMatutl
            Case 0 'Erreur blocage 
                Trace(lotReliq.GRP1.ZMSG, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
            Case 1 'ok
                'S'il y a des lots en reliquats, on propose de les consommer
                If lotReliq.GRP2.Count > 0 Then
                    Trace("Consommation de reliquats proposée :")
                    For i = 0 To lotReliq.GRP2.Count - 1
                        If lotReliq.GRP2(i).ZLOT <> "" Then
                            msgReliq = "Il reste " + lotReliq.GRP2(i).ZQTE.ToString + " " + TextBoxUnité.Text + " du lot " + lotReliq.GRP2(i).ZLOT + lotReliq.GRP2(i).ZSLO
                            Trace(msgReliq)
                            msgReliq += vbNewLine + "Voulez-vous consommer le reste ?"
                            result = MsgBox(msgReliq, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
                            If result = MsgBoxResult.Yes Then
                                'Consommation du lot sur l'OF/opération dans la situation poste
                                Try
                                    Trace("Appel du web service WSCSORELIQ")
                                    csorlq = X3ws.WSCSORELIQ(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), itmref, nsup, SFAO.Poste.GRP1.STOLOC,
                                                             lotReliq.GRP2(i).ZLOT, lotReliq.GRP2(i).ZSLO, lotReliq.GRP2(i).ZQTE, retMsg)
                                Catch ex As Exception
                                    Trace("Exception à l'appel du web service WSCSORELIQ")
                                    retMsg = "Erreur de consommation du lot " + lotReliq.GRP2(i).ZLOT + lotReliq.GRP2(i).ZSLO
                                    GoTo ErreurMatutl
                                End Try

                                Select Case csorlq
                                    Case -1 'Erreur du web service
                                        GoTo ErreurMatutl
                                    Case 1 'ok

                                End Select
                            End If
                        End If
                    Next
                End If

        End Select

        'affichage le load dans 100 ms
        Call FenSfao.WaitGif(True, 100)

        'Transfert du support/encours de l'atelier vers l'emplacement machine ou consommation des autres matières
        If Not inutile Then
            Try
                Trace("Appel du web service WSMATUTL")
                matutl = X3ws.WSMATUTL(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), itmref, supgrp, SFAO.Poste.GRP1.STOLOC,
                                       TextBoxLot.Text, CDec(MTextBoxQté.Text), TextBoxUnité.Text, retMsg)
            Catch ex As Exception
                Trace("Exception à l'appel du web service WSMATUTL")
                retMsg = "Erreur de transfert du lot " + TextBoxLot.Text
                GoTo ErreurMatutl
            End Try
            If matutl <> 1 Then
                GoTo ErreurMatutl
            End If
        End If
        If TextBoxSloF.Enabled AndAlso TextBoxSloF.Text <> "" Then
            Try
                Trace("Appel du web service WSSETLOTF")
                lotfrn = X3ws.WSSETLOTF(itmref, TextBoxLot.Text, TextBoxLotF.Text, TextBoxSloF.Text, retMsg)
            Catch ex As Exception
                Trace("Exception à l'appel du web service WSSETLOTF")
                retMsg = "Erreur d'enregistrement du n° bobine fournisseur " + TextBoxSloF.Text
                GoTo ErreurMatutl
            End Try
            If lotfrn <> 1 Then
                GoTo ErreurMatutl
            End If
        End If

        'Tout est ok
        Me.DialogResult = DialogResult.OK

        Exit Sub

ErreurMatutl:
        Trace("Erreur d'enregistrement de matière utilisée ! ", FichierTrace.niveau.alerte)
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
        Dim itmref As String
        Dim ctrlot As Integer
        Dim retMsg As String = String.Empty
        Dim qte As Decimal
        Dim lot As String
        Dim MsgEp As MsgBoxResult
        Dim i As Integer

        If TextBoxLot.Text <> "" Then
            'On remet les champs à récupérer du lot à vide (en cas d'erreur du web service)
            MTextBoxQté.Text = ""
            qtemax = 0
            If SFAO.Poste.GRP1.ZNUMFRN = 2 Then
                TextBoxLotF.Text = ""
                TextBoxSloF.Text = ""
            End If
            itmref = Strings.Split(ComboBoxArt.Text, " - ").First

            'On contrôle si le lot existe et on sélectionne le code article si besoin
            'si oui, la quantité disponible
            Trace("Vérification de l'existence du lot : " + TextBoxLot.Text)
            Try
                Trace("Appel du web service WSCTRCPLOT")
                ctrlot = X3ws.WSCTRCPLOT(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), itmref, TextBoxLot.Text,
                                         SFAO.Poste.GRP1.ZATELOC, SFAO.Poste.GRP1.STOLOC, qte, retMsg)
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
                    inutile = False
                Case 2 'déjà monté sur la machine
                    inutile = True
            End Select

            qtemax = qte
            If ComboBoxArt.Text = "" Then
                ComboBoxArt.SelectedIndex = ComboBoxArt.FindString(itmref)
            End If

            'Si encours, on vérifie l'OF/opération
            If FenSfao.CatArtCp(numof, CInt(numop), Strings.Split(ComboBoxArt.Text, " - ").First) = "EP" Then
                Trace("Lot d'un encours : on vérifie l'OF et l'opération")
                If TextBoxLot.Text.Length > 5 Then
                    lot = Strings.Left(TextBoxLot.Text, TextBoxLot.Text.Length - 5)
                    i = lot.IndexOf("/")
                    If i = -1 Then
                        MsgErr = "N° de lot erroné pour un encours de production : " + lot
                        Trace(MsgErr)
                        GoTo ErreurTextBoxLot
                    ElseIf Strings.Left(lot, i) <> numof Then
                        MsgErr = "Attention, cet encours provient d'un autre OF " + Strings.Left(lot, i) + " ! Voulez-vous continuer ?"
                        Trace(MsgErr)
                        MsgEp = MsgBox(MsgErr, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
                        If MsgEp = MsgBoxResult.No Then
                            Me.DialogResult = DialogResult.Abort
                            Me.Close()
                        End If
                    ElseIf lot.Substring(i + 1) > numop Then
                        MsgErr = "Attention, cet encours provient de l'opération " + lot.Substring(i + 1) + ", après l'opération en cours ! Voulez-vous continuer ?"
                        Trace(MsgErr)
                        MsgEp = MsgBox(MsgErr, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
                        If MsgEp = MsgBoxResult.No Then
                            Me.DialogResult = DialogResult.Abort
                            Me.Close()
                        End If
                    ElseIf lot.Substring(i + 1) = numop Then
                        MsgErr = "Attention, cet encours est déjà passé par cette opération ! Voulez-vous continuer ?"
                        Trace(MsgErr)
                        MsgEp = MsgBox(MsgErr, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
                        If MsgEp = MsgBoxResult.No Then
                            Me.DialogResult = DialogResult.Abort
                            Me.Close()
                        End If
                    End If
                End If
            End If
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
        Dim MsgErr As String = String.Empty

        Aff_ITM(ComboBoxArt.Text, MsgErr)
        If MsgErr <> "" Then
            Trace(MsgErr, FichierTrace.niveau.erreur)
        Else
            MTextBoxQté.Text = qtemax.ToString("### ### ###")
        End If

        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextBoxLot, "")
        TextBoxMsg.Text = ""
    End Sub

    Private Sub MTextBoxQté_Validating(sender As Object, e As CancelEventArgs) Handles MTextBoxQté.Validating
        Dim MsgErr As String
        If CDec(MTextBoxQté.Text) > qtemax Then
            MsgErr = "Quantité supérieure au stock disponible (" + qtemax.ToString + " " + TextBoxUnité.Text + ")"
            e.Cancel = True 'zone invalide
            ErrorProvider.SetError(MTextBoxQté, MsgErr)
            TextBoxMsg.Text = MsgErr
            System.Media.SystemSounds.Exclamation.Play() 'son erreur
            MTextBoxQté.Select() 'sélection du contrôle
        End If
    End Sub

    Private Sub MTextBoxQté_Validated(sender As Object, e As EventArgs) Handles MTextBoxQté.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxQté, "")
        TextBoxMsg.Text = ""
    End Sub
End Class