Imports System.ComponentModel
Public Class ENTOP
    Private tm As WSMatr
    Private zwslsev As WSLstEvt
    Private Sub ENTOP_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font

        Trace("Affichage fenêtre ENTOP")

        LabelCopSitChev.Visible = False     'on masque le label copie/chevauchement + combo
        ComboBoxCopSitChev.Visible = False

        'initialisation de la taille de police pour la gestion d'agrandissement de la fenêtre
        zTailFnt = FenSfao.TaillePolice(18, 9)
        fnt = New Font("Microsoft Sans Serif", zTailFnt, FontStyle.Regular)
        Me.Font = fnt

        'Sélection auto du 1er champ saisi
        MTextBoxMatr.Select()

    End Sub
    'Fonction qui gère le changement de taille des polices en fonction de la taille de la fenêtre
    Private Sub ENTOP_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
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
        Dim Matricule As Integer
        Dim AutrePoste As String
        Dim result As MsgBoxResult

        If MTextBoxMatr.Text = "" Then
            'si le matricule est vide : erreur
            e.Cancel = True 'zone invalide
            MsgErr = "Veuillez saisir ou scanner le code de votre matricule"
            ErrorProvider.SetError(MTextBoxMatr, MsgErr)
            TextBoxMsg.Text = MsgErr
        Else
            'Contrôle du code matricule saisi
            Matricule = CInt(MTextBoxMatr.Text)

            Try
                'appel web service qui récupère les infos du matricule
                tm = X3ws.WSGetMatr(Matricule)
            Catch ex As Exception

                Trace("Erreur de connexion au web service !", FichierTrace.niveau.alerte)
                Trace(ex.Message, FichierTrace.niveau.erreur)

                FenSfao.X3Anim(0)       'affichage du gif erreur
                Application.DoEvents()  'rafraichir les écrans

                Me.DialogResult = DialogResult.Abort
                Me.Close() 'on quitte l'évenement
            End Try

            If tm IsNot Nothing AndAlso tm.GRP1.ZRET = 1 Then
                'si on a un retour du web service

                AutrePoste = tm.GRP1.ZPOSTE
                TextBoxNom.Text = tm.GRP1.ZEMPDES
                TextBoxTypOpe.Text = tm.GRP1.ZTYPE_LBL
                TextBoxTypOpe.Tag = tm.GRP1.ZTYPE

                If tm.GRP1.ZENAFLG <> 2 Then        'si matricule inactif
                    MsgErr = "Matricule inactif !"
                ElseIf tm.GRP1.ZFCY <> SFAO.Site.GRP1.FCY Then 'si site matricule <> site poste
                    MsgErr = "Matricule inconnu sur le site !"
                ElseIf AutrePoste <> "" And AutrePoste = SFAO.Poste.GRP1.WST Then 'si matricule déjà présent sur ce poste
                    MsgErr = "Matricule déjà présent sur ce poste !"
                ElseIf AutrePoste <> "" And AutrePoste <> SFAO.Poste.GRP1.WST Then 'si matricule présent sur un aurtre poste
                    result = MsgBox("Matricule déjà présent sur le poste " & AutrePoste & "." & Environment.NewLine & "Voulez-vous saisir son départ ?", CType(MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MessageBoxDefaultButton.Button2, MsgBoxStyle))
                    If result = MsgBoxResult.Yes Then
                        'TODO PNO lancer la sortie du matricule d'un autre poste
                    Else
                        MsgErr = "Matricule déjà présent sur le poste " & AutrePoste & " !"
                    End If

                End If
                'si aucune erreur et matricule de type opérateur
                If MsgErr = "" And tm.GRP1.ZTYPE = 1 Then  'opérateur
                    ' on vérifie si un des opérateurs présent sur le poste dépasse le temps maxi de travail autorisé par le paramère X3 <GZTIMWRK>
                    FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                End If
            Else
                'cas de retour vide du web service 
                TextBoxNom.Text = ""
                TextBoxTypOpe.Text = ""
                MsgErr = "Matricule inconnu !"
            End If

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
    'Méthode qui contrôle si pour le patricule saisi on doit activer la copie ou le chevauchement
    Private Sub MTextBoxMatr_Validated(sender As Object, e As EventArgs) Handles MTextBoxMatr.Validated
        Dim nbEmp As Integer
        Dim nbEmpOpOf As Integer
        Dim i As Integer

        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxMatr, "")
        TextBoxMsg.Text = ""

        If Me.DialogResult <> DialogResult.Abort Then

            If CInt(TextBoxTypOpe.Tag) = 1 Then 'opérateur
                'Vérifier si on active le chevauchement ou la copie de situation d'un opérateur déjà présent
                If FenSfao.WSsp.GRP2.Count > 0 Then
                    For i = 0 To FenSfao.WSsp.GRP2.Count - 1
                        If FenSfao.WSsp.GRP2(i).XEMPNUM > 0 And FenSfao.WSsp.GRP2(i).XTYPE = 1 Then 'si opérateur présent
                            nbEmp += 1
                            'si of/opé en cours
                            If FenSfao.WSsp.GRP2(i).XMFGNUM <> "" Then
                                nbEmpOpOf += 1
                                If nbEmpOpOf = 1 Then
                                    ComboBoxCopSitChev.Tag = FenSfao.WSsp.GRP2(i).XMFGNUM & IIf(FenSfao.WSsp.GRP2(i).XOPENUM > 0, "/" & FenSfao.WSsp.GRP2(i).XOPENUM.ToString, "").ToString & " " &
                                                            IIf(FenSfao.WSsp.GRP2(i).ZOPEXC > 0, FenSfao.WSsp.GRP2(i).ZROODES, FenSfao.WSsp.GRP2(i).ZEVTDES).ToString

                                    LabelCopSitChev.Tag = FenSfao.WSsp.GRP2(i).XEMPNUM.ToString 'on mémorise le matricule d'origine
                                Else
                                    ComboBoxCopSitChev.Tag = ""
                                    LabelCopSitChev.Tag = ""
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If

                If nbEmp = 1 And nbEmpOpOf = 1 Then
                    'Chevauchement
                    LabelCopSitChev.Text = "Chevauchement ?"
                    LabelCopSitChev.Visible = True
                    ComboBoxCopSitChev.Visible = True
                    ComboBoxCopSitChev.Select()
                ElseIf nbEmp = 0 Then
                    'Si Aucun matricule
                    'Vérifier si ancien OFOP
                    'appel au web service qui récupère l'ancien OF/Opé/Et
                    Try
                        zwslsev = X3ws.WSGETLSTEV(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST)
                    Catch ex As Exception
                        'si exeption : pas d'esrreur et pas de copie de situation
                    End Try

                    If zwslsev.GRP1.ZRET = 1 Then
                        If zwslsev.GRP1.ZLSTOF <> "" Then
                            'Copie situation
                            LabelCopSitChev.Text = "Copie situation ?"
                            LabelCopSitChev.Visible = True
                            ComboBoxCopSitChev.Visible = True
                            ComboBoxCopSitChev.Tag = zwslsev.GRP1.ZLSTOF & IIf(zwslsev.GRP1.ZLSTOPE > 0, "/" & zwslsev.GRP1.ZLSTOPE.ToString, "").ToString & " " &
                                                        IIf(zwslsev.GRP1.ZLSTPHASE > 0, zwslsev.GRP1.ZLSTPHASD, zwslsev.GRP1.ZLSTEVD).ToString
                            LabelCopSitChev.Tag = 0 'on mémorise le matricule d'origine 
                            ComboBoxCopSitChev.Select()
                        End If
                    End If
                Else
                    LabelCopSitChev.Visible = False
                    ComboBoxCopSitChev.Visible = False
                End If
            Else
                LabelCopSitChev.Visible = False
                ComboBoxCopSitChev.Visible = False
            End If

            Application.DoEvents()
        End If
    End Sub

    Private Sub ComboBoxCopSitChev_GotFocus(sender As Object, e As EventArgs) Handles ComboBoxCopSitChev.GotFocus
        'Si la question copie/chevauchement est visible
        If ComboBoxCopSitChev.Visible = True Then
            If LabelCopSitChev.Text = "Chevauchement ?" Then
                TextBoxMsg.Text = "Continuer l'opération en cours : " & ComboBoxCopSitChev.Tag.ToString & " ?"
            ElseIf LabelCopSitChev.Text = "Copie situation ?" Then
                TextBoxMsg.Text = "Reprendre la dernière opération : " & ComboBoxCopSitChev.Tag.ToString & " ?"
            End If
        End If
    End Sub

    Private Sub ComboBoxCopSitChev_Validating(sender As Object, e As CancelEventArgs) Handles ComboBoxCopSitChev.Validating
        If LabelCopSitChev.Text <> "" And ComboBoxCopSitChev.Text = "" Then
            e.Cancel = True
            ErrorProvider.SetError(ComboBoxCopSitChev, "Oui/Non")
            ComboBoxCopSitChev.Select()
        End If
    End Sub

    Private Sub ComboBoxCopSitChev_Validated(sender As Object, e As EventArgs) Handles ComboBoxCopSitChev.Validated
        ErrorProvider.SetError(ComboBoxCopSitChev, "")
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        Dim retMsg As String = ""

        'on déclenche la validation dela zone copie.chevauchement
        If ComboBoxCopSitChev.Enabled() AndAlso ComboBoxCopSitChev.Visible = True AndAlso ComboBoxCopSitChev.Text = "" Then
            ComboBoxCopSitChev.Select()
            Exit Sub
        End If

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next

        'affichage du load dans 100 ms
        Call FenSfao.WaitGif(True, 100)

        Trace("Code Matricule saisi : " & MTextBoxMatr.Text)
        Trace("Nom : " & TextBoxNom.Text)
        Trace("Type : " & TextBoxTypOpe.Text)
        Trace(LabelCopSitChev.Text & " : " & ComboBoxCopSitChev.Text)

        'Enregistrer l'arrivée
        If ComboBoxCopSitChev.Text = "OUI" Then 'Copie ou chevauchement
            FenSfao.Etat("Enregistrement de la copie de la situation", 1, 5000)
            Try
                If X3ws.WSCopSit(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), CInt(LabelCopSitChev.Tag), retMsg) = True Then
                    Me.DialogResult = DialogResult.OK
                End If
            Catch ex As Exception
                GoTo ErreurEntop
            End Try
        Else
            FenSfao.Etat("Enregistrement de l'entrée opérateur", 1, 5000)
            Try
                If X3ws.WSEntOp(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, CInt(MTextBoxMatr.Text), CInt(Me.Tag), retMsg) = True Then
                    Me.DialogResult = DialogResult.OK
                End If
            Catch ex As Exception
                GoTo ErreurEntop
            End Try
        End If

        If Me.DialogResult = DialogResult.OK Then
            Me.Close()
        Else
            GoTo ErreurEntop
        End If

        Exit Sub

ErreurEntop:
        Trace("Erreur d'enregistrement de l'arrivée ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        FenSfao.Etat("Erreur d'enregistrement de l'arrivée !", 1, 5000)
        Me.DialogResult = DialogResult.Abort
        Me.Close()
        'On masque le load dans 0.5s
        Call FenSfao.WaitGif(False, 500)
    End Sub

End Class