'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout du séparateur //
'------------------------------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Text.RegularExpressions
Public Class MATUTL
    Private ofop As WSOFOPInfo

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
        Dim i As Integer

        'on contrôle si l'opérateur est présent sur le poste
        FenSfao.CtrlMatr(matr, MsgErr, TextBoxNom.Text)
        If MsgErr = "" Then
            'si ok on vérifie si opérateur est en opération hors OF
            FenSfao.OpHof(matr, MsgErr)
            If MsgErr = "" Then
                'si ok on vérifie si l'opérateur a déjà une opération en cours
                FenSfao.OFOpMatr(matr, numof, numop, MsgErr)
                If MsgErr = "" Then
                    'si ok on liste les matières
                    If FenSfao.WScp.GRP2.Count > 0 Then
                        If FenSfao.WScp.GRP2.Count = 1 Then
                            ComboBoxArt.Items.Add(FenSfao.WScp.GRP2(0).ZITMREF)
                            ComboBoxArt.Text = FenSfao.WScp.GRP2(0).ZITMREF
                            TextBoxDesign.Text = FenSfao.WScp.GRP2(0).ZITMDES
                            TextBoxUnité.Text = FenSfao.WScp.GRP2(0).ZUOM
                            ComboBoxArt.Enabled = False
                        Else
                            For i = 0 To FenSfao.WScp.GRP2.Count - 1
                                ComboBoxArt.Items.Add(FenSfao.WScp.GRP2(i).ZITMREF)
                            Next
                            ComboBoxArt.Enabled = True
                        End If
                    End If
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
        Dim i As Integer

        If ComboBoxArt.Text <> "" Then
            For i = 0 To FenSfao.WScp.GRP2.Count - 1
                If FenSfao.WScp.GRP2(i).ZITMREF = ComboBoxArt.Text Then
                    TextBoxDesign.Text = FenSfao.WScp.GRP2(i).ZITMDES
                    TextBoxUnité.Text = FenSfao.WScp.GRP2(i).ZUOM
                End If
            Next
        Else
            TextBoxDesign.Text = ""
        End If
    End Sub
End Class