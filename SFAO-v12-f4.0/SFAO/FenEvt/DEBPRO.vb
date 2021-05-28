'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout du séparateur //
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class DEBPRO
    Private ofop As WSOFOPInfo
    Private WSLstTypEtq As New WSTypEtq            'classe de la liste des types d'étiquettes

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

        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            MTextBoxMatr.Enabled = False    'on a 1 seul opérateur présent sur le poste, donc on désactive la saisie du champ matricule
            'Sélection auto du 1er champ saisi
            If ComboBoxSaiPds.Visible = True Then
                ComboBoxSaiPds.Select()
            Else
                ComboBoxTypEtq.Select()
            End If
        Else
            'Sélection auto du 1er champ saisi
            MTextBoxMatr.Select()
        End If

        'initialisation de la taille de police pour la gestion d'agrandissement de la fenêtre
        zTailFnt = FenSfao.TaillePolice(18, 9)
        fnt = New Font("Microsoft Sans Serif", zTailFnt, FontStyle.Regular)
        Me.Font = fnt
        'TODO PNO : voir comment réduire la taille de police pour les titres de colonne

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
            'FenSfao.DureeMaxPresenceDepassee(MsgErr, afficheMsg)
            'If MsgErr = "" Then
            'si ok on vérifie si opérateur est en opération hors OF
            FenSfao.OpHof(matr, MsgErr)
            If MsgErr = "" Then
                'si ok on vérifie si l'opérateur a déjà une opération en cours
                FenSfao.OFOpMatr(matr, TextBoxOF.Text, MaskedTextBoxOP.Text, MsgErr)
                If MsgErr = "" Then
                    FenSfao.EventOblig(matr, MsgErr)
                    If MsgErr = "" Then
                        'On remplit les champs affichés
                        Infos_OFOP(matr, TextBoxOF.Text, CInt(MaskedTextBoxOP.Text), MsgErr)
                    End If
                End If
            End If
            'End If
        End If
    End Sub

    'fonction qui remplit les infos à partir de l'OF/opération
    Private Sub Infos_OFOP(ByVal matr As Integer, ByVal numof As String, ByVal numop As Integer, ByRef MsgErr As String)
        Dim lsttypop As String = "BOB,EMB,FAC"
        Dim resws As Integer
        Dim retMsg As String = String.Empty
        Dim repdef As String = String.Empty
        Dim nbbob As Integer
        Dim qtepcu As Integer
        Dim nbunit As Integer
        Dim i As Integer
        Dim typetq As String = String.Empty
        Dim titcol As String = String.Empty

        'On récupère l'unité de fabrication et on la convertit dans un format lisible pour l'opérateur
        TextBoxUOM.Text = FenSfao.AffUnit(FenSfao.UnitFab(matr))

        'Si étape de production et bobinage/emballage/façonnage : saisie du poids
        If FenSfao.EtapePro(numof, numop) = True AndAlso lsttypop.IndexOf(Strings.Left(SFAO.Poste.GRP1.Y_TYPOP, 3)) <> -1 Then
            If SFAO.Poste.GRP1.Y_TYPOP = "EMB" AndAlso TextBoxUOM.Text = "UN" Then
                'Sauf à l'emballage pour les feuilles
                LabelSaiPds.Visible = False
                ComboBoxSaiPds.Enabled = False
                ComboBoxSaiPds.Visible = False
            Else 'Contrôle de l'article produit
                Trace("Contrôle de la saisie du poids")
                Try
                    Trace("Appel du web service WSSAIPDS")
                    resws = X3ws.WSSAIPDS(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), repdef, retMsg)
                Catch ex As Exception
                    Trace("Exception à l'appel du web service WSSAIPDS")
                    MsgErr = "Erreur au contrôle de la saisie du poids"
                End Try

                Select Case resws
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

        If MsgErr = "" Then
            'Si étape de production et plus d'un article produit (hors encours)
            If FenSfao.EtapePro(numof, numop) = True AndAlso FenSfao.NbArt(numof, numop) > 1 Then
                If FenSfao.OpExc(matr) > 0 Then
                    'Si opération exceptionnelle : saisie de l'article produit
                    LabelAmalg.Text = "Article produit"
                Else
                    'Amalgame : recherche du nombre de bobines filles
                    Trace("Recherche du nombre de bobines filles (amalgame)")
                    Try
                        Trace("Appel du web service WSGETNBBOB")
                        nbbob = X3ws.WSGETNBBOB(SFAO.Site.GRP1.FCY, numof, numop, retMsg)
                    Catch ex As Exception
                        Trace("Exception à l'appel du web service WSGETNBBOB")
                        MsgErr = "Erreur à la recherche du nombre de bobines filles pour l'amalgame"
                    End Try

                    Select Case nbbob
                        Case -1 'Erreur du web service
                            Trace(retMsg)
                            MsgErr = retMsg
                        Case Else
                            If nbbob <= 1 Then
                                LabelAmalg.Text = ""
                                TextAmalg.Enabled = False
                                TextAmalg.Visible = False
                            End If
                    End Select
                End If
            Else
                LabelAmalg.Visible = False
                TextAmalg.Enabled = False
                TextAmalg.Visible = False
            End If
        End If

        If MsgErr = "" Then
            'Récupération de la liste des types d'étiquettes
            Trace("Récupération de la liste des types d'étiquettes")
            Try
                Trace("Appel du web service WSGETLETQ")
                WSLstTypEtq = X3ws.WSGETLETQ("P")
            Catch ex As Exception
                Trace("Exception à l'appel du web service WSGETLETQ")
                MsgErr = "Erreur de récupération de la liste de types d'étiquettes ! "
            End Try

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
        End If

        If MsgErr = "" Then
            'Type d'étiquettes par défaut
            Trace("Recherche du type d'étiquettes par défaut")
            Try
                Trace("Appel du web service WSGETTETQ")
                resws = X3ws.WSGETTETQ(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), typetq, retMsg)
            Catch ex As Exception
                Trace("Exception à l'appel du web service WSSAIPDS")
                MsgErr = "Erreur au contrôle de la saisie du poids"
            End Try

            Select Case resws
                Case -1 'Erreur du web service
                    Trace(retMsg)
                    MsgErr = retMsg
                Case 0 'Erreur blocage 
                    Trace(retMsg)
                    MsgErr = retMsg
                Case 1 'ok
                    If typetq <> "" Then
                        ComboBoxTypEtq.Text = typetq
                    End If
            End Select
        End If

        If MsgErr = "" Then
            'Quantité UC
            If lsttypop.IndexOf(Strings.Left(SFAO.Poste.GRP1.Y_TYPOP, 3)) = -1 OrElse FenSfao.OpExc(matr) > 0 Then
                'Si type opération <> BOB/EMB/FAC ou opération exceptionnelle : pas de saisie
                LabelQtéUC.Visible = False
                MTextBoxQtéUC.Enabled = False
                MTextBoxQtéUC.Visible = False
            Else
                Select Case Strings.Left(SFAO.Poste.GRP1.Y_TYPOP, 3)
                    Case "BOB"
                        LabelQtéUC.Text = "Métrage bobine"
                    Case "EMB"
                        If TextBoxUOM.Text = "UN" Then
                            LabelQtéUC.Text = "Nb feuilles / paq."
                        Else
                            LabelQtéUC.Text = "Métrage bobine"
                        End If
                    Case "FAC"
                        LabelQtéUC.Text = "Nb sachets / paq."
                End Select

                'Quantité conditionnement
                Trace("Recherche de la quantité de conditionnement")
                Try
                    Trace("Appel du web service WSGETQPCU")
                    qtepcu = X3ws.WSGETQPCU(SFAO.Site.GRP1.FCY, numof, numop, retMsg)
                Catch ex As Exception
                    Trace("Exception à l'appel du web service WSGETQPCU")
                    MsgErr = "Erreur à la recherche de la quantité de conditionnement"
                End Try

                Select Case qtepcu
                    Case -1 'Erreur du web service
                        Trace(retMsg)
                        MsgErr = retMsg
                    Case Else
                        If qtepcu > 0 Then
                            MTextBoxQtéUC.Text = qtepcu.ToString
                        End If
                End Select
            End If
        End If

        If MsgErr = "" Then
            'Nb UC / déclaration
            Select Case Strings.Left(SFAO.Poste.GRP1.Y_TYPOP, 3)
                Case "BOB"
                    LabelNbUC.Text = "Nb bob. / tringlée"
                Case "CCP"
                    LabelNbUC.Text = "Nb bobines / décl."
                Case "EMB"
                    If TextBoxUOM.Text = "UN" Then
                        LabelNbUC.Text = "Nb paquets / décl."
                    Else
                        LabelNbUC.Text = "Nb bobines / décl."
                    End If
                Case "FAC"
                    LabelNbUC.Text = "Nb paquets / décl."
                Case Else
                    LabelNbUC.Visible = False
                    MTextBoxNbUC.Enabled = False
                    MTextBoxNbUC.Visible = False
            End Select

            If LabelNbUC.Visible Then
                If FenSfao.OpExc(matr) > 0 Then
                    'Si opération exceptionnelle
                    MTextBoxNbUC.Text = "1"
                ElseIf LabelAmalg.Visible = False Then
                    'Si pas d'amalgamme : recherche du nombre de bobines filles
                    If nbbob = 0 Then
                        'Si webservice non exécuté précédemment
                        Trace("Recherche du nombre de bobines filles (Nb UC / déclaration)")
                        Try
                            Trace("Appel du web service WSGETNBBOB")
                            nbbob = X3ws.WSGETNBBOB(SFAO.Site.GRP1.FCY, numof, numop, retMsg)
                        Catch ex As Exception
                            Trace("Exception à l'appel du web service WSGETNBBOB")
                            MsgErr = "Erreur à la recherche du nombre de bobines filles"
                        End Try

                        Select Case nbbob
                            Case -1 'Erreur du web service
                                Trace(retMsg)
                                MsgErr = retMsg
                            Case Else

                        End Select
                    End If
                    If MsgErr = "" Then
                        If SFAO.Poste.GRP1.Y_TYPOP = "EMB" AndAlso nbbob < 10 Then
                            MTextBoxNbUC.Text = "10"
                        ElseIf nbbob > 1 Then
                            MTextBoxNbUC.Text = nbbob.ToString
                        Else
                            MTextBoxNbUC.Text = "1"
                        End If
                    End If
                End If
            End If
        End If

        If MsgErr = "" Then
            'Nb unités / format
            If Strings.Left(SFAO.Poste.GRP1.Y_TYPOP, 3) = "EMB" Then
                Trace("Recherche du nombre d'unités par format (emballage)")
                Try
                    Trace("Appel du web service WSGETNBUN")
                    nbunit = X3ws.WSGETNBUN(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), retMsg)
                Catch ex As Exception
                    Trace("Exception à l'appel du web service WSGETNBUN")
                    MsgErr = "Erreur à la recherche du nombre d'unités par format"
                End Try

                Select Case nbunit
                    Case -1 'Erreur du web service
                        Trace(retMsg)
                        MsgErr = retMsg
                    Case Else
                        MTextBoxNbUN.Text = nbunit.ToString
                End Select
            Else
                LabelNbUN.Visible = False
                MTextBoxNbUN.Enabled = False
                MTextBoxNbUN.Visible = False
            End If
        End If

        If MsgErr = "" Then
            If FenSfao.EtapePro(numof, numop) = True AndAlso FenSfao.EtapePal(numof, numop) = True AndAlso FenSfao.OpExc(matr) = 0 Then
                'Si étape de production et de palettisation, opération non exceptionnelle
                'Saisie taille des palettes par PF
                Select Case Strings.Left(SFAO.Poste.GRP1.Y_TYPOP, 3)
                    Case "EMB"
                        If TextBoxUOM.Text = "UN" Then
                            LabelPal.Text = "Nb paquets / palette"
                        Else
                            LabelPal.Text = "Nb bobines / palette"
                        End If
                    Case "FAC"
                        LabelPal.Text = "Nb paquets / palette"
                    Case Else
                        LabelPal.Text = "Nb bobines / palette"
                End Select
                If FenSfao.WSof.GRP2.Count > 0 Then
                    For i = 0 To FenSfao.WSof.GRP2.Count - 1
                        If FenSfao.WSof.GRP2(i).ZCODAMLG <> String.Empty Then
                            titcol = "Column" + FenSfao.WSof.GRP2(i).ZCODAMLG
                            DataGridPal.Columns(titcol).HeaderText = FenSfao.WSof.GRP2(i).ZITMREF
                        Else
                            titcol = "ColumnA"
                            DataGridPal.Columns(titcol).HeaderText = FenSfao.WSof.GRP2(i).ZITMREF
                        End If
                    Next
                End If
                For Each col As DataGridViewColumn In DataGridPal.Columns
                    If col.HeaderText.Length = 1 Then
                        col.Visible = False
                    End If
                Next
                'On ajoute une ligne de saisie
                DataGridPal.Rows.Add()
            Else
                LabelPal.Visible = False
                DataGridPal.Visible = False
                DataGridPal.Enabled = False
            End If
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
        Dim titcol As String
        Dim msgpal As String
        Dim WSLstDebPro As New WSDebPro
        Dim i As Integer
        Dim result As MsgBoxResult
        result = MsgBoxResult.Ok

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next
        If ComboBoxSaiPds.Visible = True AndAlso ComboBoxSaiPds.Text = "" Then
            ErrorProvider.SetError(ComboBoxSaiPds, "Oui/Non")
            ComboBoxSaiPds.Select()
            Exit Sub
        End If
        If ComboBoxTypEtq.Text = "" Then
            ErrorProvider.SetError(ComboBoxTypEtq, "Type d'étiquette obligatoire")
            ComboBoxTypEtq.Select()
            Exit Sub
        End If
        If TextAmalg.Visible = True AndAlso TextAmalg.Text = "" Then
            ErrorProvider.SetError(TextAmalg, "Séquence d'amalgame obligatoire")
            TextAmalg.Select()
            Exit Sub
        End If

        If DataGridPal.Visible = True Then
            'Contrôle saisie des tailles palettes
            If FenSfao.WSof.GRP2.Count > 0 Then
                For i = 0 To FenSfao.WSof.GRP2.Count - 1
                    If FenSfao.WSof.GRP2(i).ZCODAMLG <> String.Empty Then
                        titcol = "Column" + FenSfao.WSof.GRP2(i).ZCODAMLG
                        If DataGridPal.Item(DataGridPal.Columns(titcol).Index, 0).EditedFormattedValue.ToString = "" Then
                            msgpal = LabelPal.Text + " non spécifié : La déclaration automatique va être désactivée !"
                            Trace(msgpal)
                            msgpal += vbNewLine + "Voulez-vous continuer ?"
                            result = MsgBox(msgpal, CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
                            If result = MsgBoxResult.No Then
                                Exit Sub
                            End If
                        Else
                            Dim wsdp2 As New WSDebProGRP2
                            wsdp2.ZNPAL = FenSfao.WSof.GRP2(i).ZLIGITM
                            wsdp2.ZTPAL = CInt(DataGridPal.Item(DataGridPal.Columns(titcol).Index, 0).EditedFormattedValue.ToString)
                            WSLstDebPro.GRP2.Add(wsdp2)
                        End If
                    End If
                Next
            End If
        End If

        'tout va bien on enregistre la fin d'opération + suivi auto du temps passé depuis le dernier évenement

        'affichage le load dans 100 ms
        Call FenSfao.WaitGif(True, 100)

        WSLstDebPro.GRP1.ZFCY = SFAO.Site.GRP1.FCY
        WSLstDebPro.GRP1.ZPOSTE = SFAO.Poste.GRP1.WST
        WSLstDebPro.GRP1.ZTYPOP = SFAO.Poste.GRP1.Y_TYPOP
        WSLstDebPro.GRP1.ZEMPNUM = CInt(MTextBoxMatr.Text)
        WSLstDebPro.GRP1.ZEVTNUM = CInt(Me.Tag)
        WSLstDebPro.GRP1.ZTYPETQ = ComboBoxTypEtq.Text
        WSLstDebPro.GRP1.ZSAIPDS = ComboBoxSaiPds.Text
        WSLstDebPro.GRP1.ZAMALGAME = TextAmalg.Text
        If MTextBoxQtéUC.Text <> "" Then
            WSLstDebPro.GRP1.ZQTYPCU = CInt(MTextBoxQtéUC.Text)
        End If
        If MTextBoxNbUC.Text <> "" Then
            WSLstDebPro.GRP1.ZNBPCU = CInt(MTextBoxNbUC.Text)
        End If
        If MTextBoxNbUN.Text <> "" Then
            WSLstDebPro.GRP1.ZNBUN = CInt(MTextBoxNbUN.Text)
        End If

        Try
            Trace("Appel du web service WSDEBPRO")
            WSLstDebPro = X3ws.WSDEBPRO(WSLstDebPro)
        Catch ex As Exception
            Trace("Exception à l'appel du web service WSDEBPRO")
            GoTo ErreurDebpro
        End Try

        If WSLstDebPro.GRP1.ZRET = 1 Then
            Me.DialogResult = DialogResult.OK
        Else
            retMsg = WSLstTypEtq.GRP1.ZMSG
            GoTo ErreurDebpro
        End If

        Exit Sub

ErreurDebpro:
        Trace("Erreur d'enregistrement de début de production ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        Me.DialogResult = DialogResult.Abort
        Me.Close()
        'On masque le load dans 0.5s
        Call FenSfao.WaitGif(False, 500)

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
            ErrorProvider.SetError(ComboBoxTypEtq, "Type d'étiquette obligatoire")
            ComboBoxTypEtq.Select()
        End If
    End Sub

    Private Sub ComboBoxTypEtq_Validated(sender As Object, e As EventArgs) Handles ComboBoxTypEtq.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(ComboBoxTypEtq, "")
        TextBoxMsg.Text = ""
    End Sub

    Private Sub TextAmalg_GotFocus(sender As Object, e As EventArgs) Handles TextAmalg.GotFocus
        If ErrorProvider.GetError(TextAmalg).ToString = "" Then
            TextBoxMsg.Text = AmalgMsg()
        End If
        TextAmalg.Select()
    End Sub
    Private Function AmalgMsg() As String
        Dim MsgInf As String
        Dim i As Integer
        MsgInf = "Codes amalgames :" + vbNewLine
        If FenSfao.WSof.GRP2.Count > 0 Then
            For i = 0 To FenSfao.WSof.GRP2.Count - 1
                If FenSfao.WSof.GRP2(i).ZCODAMLG <> String.Empty Then
                    MsgInf += FenSfao.WSof.GRP2(i).ZCODAMLG + " = " + FenSfao.WSof.GRP2(i).ZITMREF + " - "
                End If
            Next
        End If

        Return MsgInf
    End Function

    Private Sub TextAmalg_Validating(sender As Object, e As CancelEventArgs) Handles TextAmalg.Validating
        Dim LstAmlg As String = String.Empty
        Dim i As Integer

        If TextAmalg.Text = "" Then
            e.Cancel = True
            ErrorProvider.SetError(TextAmalg, "Séquence d'amalgame obligatoire")
            TextAmalg.Select()
        Else
            If FenSfao.OpExc(CInt(MTextBoxMatr.Text)) > 0 Then
                'Contrôle : une seule bobine autorisée
                If TextAmalg.Text.Length > 1 Then
                    e.Cancel = True
                    ErrorProvider.SetError(TextAmalg, "Opération exceptionnelle : 1 seule bobine produite autorisée")
                    TextBoxMsg.Text = "Opération exceptionnelle : 1 seule bobine produite autorisée"
                    TextBoxMsg.Text += vbNewLine + AmalgMsg()
                    TextAmalg.Select()
                    Exit Sub
                End If
            End If
            If FenSfao.WSof.GRP2.Count > 0 Then
                For i = 0 To FenSfao.WSof.GRP2.Count - 1
                    If FenSfao.WSof.GRP2(i).ZCODAMLG <> String.Empty Then
                        LstAmlg += FenSfao.WSof.GRP2(i).ZCODAMLG
                    End If
                Next
            End If
            For Each ch As Char In TextAmalg.Text
                If LstAmlg.Contains(ch) = False Then
                    e.Cancel = True
                    ErrorProvider.SetError(TextAmalg, "Caractère <" + ch + "> incorrect dans la séquence d'amalgame")
                    TextBoxMsg.Text = "Caractère <" + ch + "> incorrect dans la séquence d'amalgame !"
                    TextBoxMsg.Text += vbNewLine + AmalgMsg()
                    TextAmalg.Select()
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Private Sub TextAmalg_Validated(sender As Object, e As EventArgs) Handles TextAmalg.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextAmalg, "")
        TextBoxMsg.Text = ""
    End Sub

    Private Sub MTextBoxNbUC_Validating(sender As Object, e As CancelEventArgs) Handles MTextBoxNbUC.Validating
        If CInt(MTextBoxNbUC.Text) > 30 Then
            e.Cancel = True
            ErrorProvider.SetError(MTextBoxNbUC, "Nombre de conditionnements <= 30")
            TextBoxMsg.Text = "Le nombre de conditionnements doit être inférieur ou égal à 30 !"
            MTextBoxNbUC.Select()
        End If
    End Sub

    Private Sub MTextBoxNbUC_Validated(sender As Object, e As EventArgs) Handles MTextBoxNbUC.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextAmalg, "")
        TextBoxMsg.Text = ""
    End Sub

    Private Sub MTextBoxNbUN_Validating(sender As Object, e As CancelEventArgs) Handles MTextBoxNbUN.Validating
        If CInt(MTextBoxNbUN.Text) = 0 Then
            e.Cancel = True
            ErrorProvider.SetError(MTextBoxNbUN, LabelNbUN.Text + " obligatoire")
            MTextBoxNbUN.Select()
        End If
    End Sub

    Private Sub MTextBoxNbUN_Validated(sender As Object, e As EventArgs) Handles MTextBoxNbUN.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(TextAmalg, "")
        TextBoxMsg.Text = ""
    End Sub

End Class