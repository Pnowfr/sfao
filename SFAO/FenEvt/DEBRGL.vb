Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class DEBRGL
    Private ofop As WSOFOPInfo

    Private Sub DEBRGL_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zTailFnt As Single
        Dim fnt As Font
        Dim i As Integer
        Dim MsgErr As String = String.Empty

        Trace("Affichage fenêtre DEBRGL")

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

        'Récupération de la iste des phases 
        For Each phs As Phase In Phases.OrderBy(Function(x) x.ordre)
            If phs.evenement = CInt(Me.Tag) Then
                ComboBoxPhase.Items.Add(phs.desc)
            End If
        Next

        If ComboBoxPhase.Items.Count = 0 Then
            Trace("Aucune phase de règlage définie pour ce poste ! ", FichierTrace.niveau.alerte)
            Me.Close()
        End If

        If MTextBoxMatr.Text <> "" And MsgErr = "" Then
            MTextBoxMatr.Enabled = False
            'Sélection auto du 1er champ saisi
            ComboBoxPhase.Select()
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
    Private Sub DEBRGL_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
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
                End If
            End If
        End If
    End Sub

    Private Sub MTextBoxMatr_Validated(sender As Object, e As EventArgs) Handles MTextBoxMatr.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(MTextBoxMatr, "")
        TextBoxMsg.Text = ""
    End Sub

    Private Sub ComboBoxPhase_GotFocus(sender As Object, e As EventArgs) Handles ComboBoxPhase.GotFocus
        If ErrorProvider.GetError(ComboBoxPhase).ToString = "" Then
            TextBoxMsg.Text = ComboMsg()
        End If
        ComboBoxPhase.Select()
    End Sub

    Private Function ComboMsg() As String
        Dim MsgErr As String
        MsgErr = "Veuillez sélectionner la phase/étape de réglage : " + Environment.NewLine
        For Each phs As Phase In Phases.OrderBy(Function(x) x.ordre)
            If phs.evenement = CInt(Me.Tag) Then
                MsgErr += phs.desc + ", "
            End If
        Next
        MsgErr = MsgErr.Remove(MsgErr.LastIndexOf(", "), 2).Insert(MsgErr.LastIndexOf(", "), "")
        Return MsgErr
    End Function
    Private Sub ComboBoxPhase_Validating(sender As Object, e As CancelEventArgs) Handles ComboBoxPhase.Validating
        Dim MsgErr As String = ""
        If ComboBoxPhase.Text = "" Then
            MsgErr = ComboMsg()
        ElseIf Phases(ComboBoxPhase.SelectedIndex).phase = FenSfao.PhaseEnCours(CInt(MTextBoxMatr.Text), TextBoxOF.Text, CInt(MaskedTextBoxOP.Text)) Then
            MsgErr = "Vous êtes déjà en phase de " & Phases(ComboBoxPhase.SelectedIndex).desc
        End If

        If MsgErr <> "" Then
            'si la phase est vide : erreur
            e.Cancel = True 'zone invalide
            ErrorProvider.SetError(ComboBoxPhase, MsgErr)
            TextBoxMsg.Text = MsgErr
            System.Media.SystemSounds.Exclamation.Play() 'son erreur
            ComboBoxPhase.Select() 'sélection du contrôle
            Console.WriteLine("ComboBoxPhase_Validating : " & MsgErr)
        End If
    End Sub

    Private Sub ComboBoxPhase_Validated(sender As Object, e As EventArgs) Handles ComboBoxPhase.Validated
        'on efface les erreurs précédentes
        ErrorProvider.SetError(ComboBoxPhase, "")
        TextBoxMsg.Text = ""
        Console.WriteLine("ComboBoxPhase_Validated")
    End Sub

    Private Sub BtnFin_Click(sender As Object, e As EventArgs) Handles BtnFin.Click
        Me.Close()
    End Sub

    Private Sub BtnOk_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
        'Console.WriteLine("BtnOk_Click")
        Dim retMsg As String = String.Empty
        Dim debrgl As Integer = -1

        'Dans certains cas la validation passe même si tous les champs ne sont pas valides
        For Each ctl As Control In Me.TableLayoutPanel1.Controls
            If ErrorProvider.GetError(ctl) <> "" Then
                Exit Sub
            End If
        Next


        'tout va bien on enregistre le débutde réglage
        Try
            debrgl = X3ws.WSDEBRGL(SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.WST, SFAO.Poste.GRP1.Y_TYPOP, CInt(MTextBoxMatr.Text), CInt(Me.Tag),
                                   Phases(ComboBoxPhase.SelectedIndex).phase, TextBoxOF.Text, CInt(MaskedTextBoxOP.Text), retMsg)
        Catch ex As Exception
            GoTo ErreurDebrgl
        End Try

        Select Case debrgl
            Case -1 'Erreur du web service
                GoTo ErreurDebrgl
            Case 0 'Erreur blocage 
                Trace(retMsg, FichierTrace.niveau.avertissement) 'on affiche le message à l'utilisateur
            Case 1 'ok
                Me.DialogResult = DialogResult.OK
        End Select

        Exit Sub

ErreurDebrgl:
        Trace("Erreur d'enregistrement du début de réglage ! ", FichierTrace.niveau.alerte)
        If retMsg <> "" Then
            Trace(retMsg, FichierTrace.niveau.erreur)
        End If
        FenSfao.Etat("Erreur d'enregistrement du début de réglage ! ", 1, 5000)
        Me.DialogResult = DialogResult.Abort
        Me.Close()


    End Sub


End Class