'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] : ajout vérif si si le serveur web est accessible et démarré
'
'------------------------------------------------------------------------------------------------------------------------
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.Net
Public Class Login

    'Déclaration des variables pour le mouvement et déplacement de la form
    Enum TypeMouv
        Null
        Resiz
        Mouv
    End Enum

    Dim MTyp As TypeMouv = TypeMouv.Null
    Dim MPosX As Integer
    Dim MPosY As Integer

    Private Sub FenCnx_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        'La form de connexion ne peut pas être redimentionnée
        'si on est en redimentionnement de la form 
        'If MTyp = TypeMouv.Resiz Then
        '   Dim XOffset, YOffset As Integer
        '   XOffset = e.X - (Me.Size.Width)
        '   YOffset = e.Y - (Me.Size.Height)
        '   Me.Size = New Size(Me.Size.Width + XOffset, Me.Size.Height + YOffset)
        'si on est en mouvement de la form
        'Else

        'Par contre on peut la bouger
        If MTyp = TypeMouv.Mouv Then
            Me.Location = New Point(Me.Location.X + e.X - MPosX, Me.Location.Y + e.Y - MPosY)
        End If
    End Sub
    Private Sub FenCnx_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        'si on clique avec le bouton gauche est dans le coin en bas à droite on est en redimentionnment si non en mouvement
        'If e.Button = Windows.Forms.MouseButtons.Left AndAlso e.X >= (Me.Size.Width - 20) AndAlso e.Y >= (Me.Size.Height - 20) Then
        'MTyp = TypeMouv.Resiz
        'Else

        If e.Button = Windows.Forms.MouseButtons.Left Then
            MTyp = TypeMouv.Mouv
            MPosX = e.X
            MPosY = e.Y
        End If
    End Sub
    Private Sub FenCnx_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
        'quand on relache le bouton on désactive les mouvements
        MTyp = TypeMouv.Null
        MPosX = 0
        MPosY = 0
    End Sub
    Private Sub BtnCnxFin_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub LblFin_Click(sender As Object, e As EventArgs) Handles LblFin.Click
        Me.Close()
    End Sub

    Private Sub FenCnx_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim i As Integer
        Dim nbdos As Integer
        Dim appSettings As Specialized.NameValueCollection = ConfigurationManager.AppSettings

        Me.Icon = My.Resources.icon_sfao

        TextBoxSoc.Text = SFAO.Param("SOCIETE")
        If Strings.Right(TextBoxSoc.Text, 1) = " " Then
            If SFAO.Param("LASTSOCIETE") <> "" Then
                TextBoxSoc.Text = SFAO.Param("LASTSOCIETE")
            Else
                TextBoxSoc.Text = Strings.Left(TextBoxSoc.Text, TextBoxSoc.Text.Length - 1)
            End If
        ElseIf TextBoxSoc.Text <> "" Then
            TextBoxSoc.Enabled = False
        End If


        TextBoxSit.Text = SFAO.Param("SITE")
        If Strings.Right(TextBoxSit.Text, 1) = " " Then
            If SFAO.Param("LASTSITE") <> "" Then
                TextBoxSit.Text = SFAO.Param("LASTSITE")
            Else
                TextBoxSit.Text = Strings.Left(TextBoxSoc.Text, TextBoxSit.Text.Length - 1)
            End If
        ElseIf TextBoxSit.Text <> "" Then
            TextBoxSit.Enabled = False
        End If


        'recherche des dossiers configurés pour l'accès web service
        Trace("Dossiers paramétrés : ")
        For i = 0 To ConfigurationManager.AppSettings.Count - 1
            If appSettings.Keys(i).Contains("WEBSERVEURURL") = True Then
                Trace(appSettings.Keys(i).Substring(14) & " " & appSettings(appSettings.Keys(i).ToString))
                ComboBoxDos.Items.Add(appSettings.Keys(i).Substring(14))
                nbdos += 1
            End If
        Next

        If nbdos = 0 Then
            Trace("Erreur critique : Aucun dossier dans le fichier de configuration !", FichierTrace.niveau.critique)
        End If

        If SFAO.Param("LASTDOSSIER") <> "" Then
            ComboBoxDos.SelectedIndex = ComboBoxDos.FindStringExact(SFAO.Param("LASTDOSSIER"))
        End If

        'TODO Temporaire à enlever !!!
        ComboBoxDos.SelectedIndex = 3

        TextBoxPoste.Text = SFAO.Param("POSTE")
        If SFAO.Param("MULTIPOSTE") <> "VRAI" And TextBoxPoste.Text <> "" Then
            TextBoxPoste.Enabled = False
        ElseIf SFAO.Param("LASTPOSTE") <> "" Then
            TextBoxPoste.Text = SFAO.Param("LASTPOSTE")
        End If

        If SFAO.Param("SANSMOTDEPASSE") = "VRAI" Then
            TextBoxMdp.Enabled = False
        End If

        Application.DoEvents() 'Raffraichit l'écran

        'sélection du 1er champ vide
        If TextBoxSoc.Text = "" Then
            TextBoxSoc.Select()
        ElseIf TextBoxSit.Text = "" Then
            TextBoxSit.Select()
        ElseIf ComboBoxDos.Text = "" Then
            ComboBoxDos.Select()
        ElseIf TextBoxPoste.Text = "" Then
            TextBoxPoste.Select()
        Else
            TextBoxMdp.Select()
        End If

        'TODO Temporaire à enlever : 
        ComboBoxDos.Select()
    End Sub

    Private Function Check_Champs() As Boolean
        'On vérifie si les champs sont saisis
        Check_Champs = True
        If TextBoxSoc.Text = "" Then
            MsgBox("Veuillez saisir le champ vide", MsgBoxStyle.Exclamation)
            TextBoxSoc.Select()
            Check_Champs = False
            Exit Function
        End If
        If TextBoxSit.Text = "" Then
            MsgBox("Veuillez saisir le champ vide", MsgBoxStyle.Exclamation)
            TextBoxSit.Select()
            Check_Champs = False
            Exit Function
        End If
        If ComboBoxDos.Text = "" Then
            MsgBox("Veuillez saisir le champ vide", MsgBoxStyle.Exclamation)
            ComboBoxDos.Select()
            Check_Champs = False
            Exit Function
        End If
        If TextBoxPoste.Text = "" Then
            MsgBox("Veuillez saisir le champ vide", MsgBoxStyle.Exclamation)
            TextBoxPoste.Select()
            Check_Champs = False
            Exit Function
        End If
        If SFAO.Param("SANSMOTDEPASSE") <> "VRAI" AndAlso TextBoxMdp.Text = "" Then
            MsgBox("Veuillez saisir le champ vide", MsgBoxStyle.Exclamation)
            TextBoxMdp.Select()
            Check_Champs = False
            Exit Function
        End If
    End Function

    Private Sub BtnCnxOk_Click(sender As Object, e As EventArgs) Handles BtnCnxOk.Click
        Dim dossier As String
        Dim version As String
        Dim URLweb As String
        Dim IPweb As String
        Dim s As Integer
        Dim CnxOk As Boolean

        Trace("[Login] Click Bouton OK")
        Trace("Société : " & TextBoxSoc.Text, FichierTrace.niveau.toujours)
        Trace("Site : " & TextBoxSit.Text, FichierTrace.niveau.toujours)
        Trace("Dossier : " & ComboBoxDos.Text, FichierTrace.niveau.toujours)
        Trace("Poste : " & TextBoxPoste.Text, FichierTrace.niveau.toujours)

        If TextBoxMdp.Enabled = True Then
            Trace("Mot de passe : " & "Avec", FichierTrace.niveau.toujours)
        Else
            Trace("Mot de passe : " & "Sans", FichierTrace.niveau.toujours)
        End If

        'On vérifie si les champs sont saisis
        If Not Check_Champs() Then Exit Sub

        dossier = Regex.Match(ComboBoxDos.Text, "^\S{0,}").Value    'permet d'enlever (ext) du dossier affiché
        version = ParamDos("WEBSERVEURVERSION", ComboBoxDos.Text)   'récupère la version V6 ou V12 des paramètres du dossier
        If version <> "" Then


            URLweb = ParamDos("WEBSERVEURURL", ComboBoxDos.Text)        'récupère l'URL du serveur web des paramètres du dossier
            If URLweb <> "" Then
                'L'IP est entre les premiers // et le / suivant et peut contenir un port :xxxxxx
                s = URLweb.IndexOf("//") + 1
                IPweb = URLweb.Substring(s + 1, URLweb.IndexOf("/", s + 1) - s - 1)
                If IPweb.Contains(":") Then
                    IPweb = IPweb.Substring(0, IPweb.IndexOf(":"))
                End If

                'Contrôle du ping
                Trace("Test du ping IP : " + IPweb)
                If My.Computer.Network.Ping(IPweb) Then

                    'TODO contrôler si le web service est actif 


                    Try
                        'On instencie les web services 
                        Trace("Instentiation du web service")

                        X3ws = New X3WebServ(ComboBoxDos.Text, version)

                        'La première connexion qui démarre le web serice côté X3 prends quelques secondes ! (si le pool est en démarrage auto)
                        SFAO.Sleep(1000) 'attente de 1 secondes

                        'on contrôle si le web service répond
                        If X3ws.WSDTSFAO() <> "" Then
                            CnxOk = True
                            Trace("[Login] Connexion au web service ok")
                        End If
                    Catch ex As Exception
                        Trace("Connexion au web service du dossier " & ComboBoxDos.Text & " impossible !", FichierTrace.niveau.erreur)
                        Trace(ex.Message, FichierTrace.niveau.erreur)
                    End Try

                    'Si la connexion au web service est possible
                    If CnxOk = True Then

                        Try
                            'on récupère les infos du site
                            SFAO.Site = X3ws.WSGETSITE("", "", TextBoxSit.Text, dossier, TextBoxSoc.Text)
                        Catch ex As Exception
                            Trace("Erreur de connexion au web service !", FichierTrace.niveau.alerte)
                            Trace("[WSGETSITE] : " & ex.Message, FichierTrace.niveau.erreur)
                        End Try

                        If SFAO.Site.GRP1.FCY <> "" Then
                            Try
                                'on récupère les infos du poste
                                SFAO.Poste = X3ws.WSGETPOSTE(TextBoxPoste.Text, SFAO.Site.GRP1.FCY)
                            Catch ex As Exception
                                Trace("Erreur de connexion au web service !", FichierTrace.niveau.alerte)
                                Trace("[WSGETPOSTE] : " & ex.Message, FichierTrace.niveau.erreur)
                            End Try

                            If SFAO.Poste.GRP1.ZRET = 1 Then
                                If SFAO.Poste.GRP1.ZENAFLG <> 2 Then
                                    Trace("Erreur : poste " & TextBoxPoste.Text & " inactif !", FichierTrace.niveau.alerte)
                                ElseIf SFAO.Poste.GRP1.ZGRP = 2 Then
                                    Trace("Erreur : poste de type groupe non autorisé " & TextBoxPoste.Text & " !", FichierTrace.niveau.alerte)
                                ElseIf SFAO.Poste.GRP1.WSTTYP = PosteWs.TypePost.Sous_traitance Then
                                    Trace("Erreur : poste de type sous-traitance non autorisé " & TextBoxPoste.Text & " !", FichierTrace.niveau.alerte)
                                Else
                                    Try
                                        'récupération des événements rattachés au poste
                                        SFAO.Events = X3ws.WSGETEVT(TextBoxPoste.Text)
                                    Catch ex As Exception
                                        Trace("Erreur de connexion au web service !", FichierTrace.niveau.alerte)
                                        Trace("[WSGETEVT] : " & ex.Message, FichierTrace.niveau.erreur)
                                    End Try

                                    'si on a des événements 
                                    If SFAO.Events.GRP2.Count > 0 Then
                                        'Contrôle du mot de passe si saisie avec mot de passe (géré en paramètre local)
                                        If SFAO.Param("SANSMOTDEPASSE") <> "VRAI" AndAlso SFAO.Param("MOTDEPASSE") <> "" Then
                                            If TextBoxMdp.Text.ToUpper = Crypt.Decrypt(SFAO.Param("MOTDEPASSE")).ToUpper Then
                                                Me.DialogResult = DialogResult.OK
                                            Else
                                                Trace("Mot de passe incorrect !", FichierTrace.niveau.avertissement)
                                            End If
                                        Else
                                            'Si non on autorise le login
                                            Me.DialogResult = DialogResult.OK
                                        End If

                                        If Me.DialogResult = DialogResult.OK Then
                                            Trace("[Login] Connexion ok")
                                            Me.Close()
                                        End If
                                    Else
                                        Trace("Erreur : aucun événement trouvé pour le poste " & TextBoxPoste.Text & " !", FichierTrace.niveau.alerte)
                                    End If
                                End If
                            Else
                                Trace("Erreur : poste " & TextBoxPoste.Text & " inconnu sur le site " & TextBoxSit.Text & " !", FichierTrace.niveau.alerte)
                            End If
                        Else
                            Trace("Erreur : site " & TextBoxSit.Text & " inconnu sur le dossier " & dossier & " !", FichierTrace.niveau.alerte)
                        End If
                    Else
                        Trace("Erreur de connexion au dossier " & dossier & " !", FichierTrace.niveau.alerte)
                    End If
                Else
                    Trace("Erreur du test du ping IP: " & IPweb & " !", FichierTrace.niveau.erreur)
                    Trace("Connexion au serveur du dossier " & ComboBoxDos.Text & " impossible !", FichierTrace.niveau.alerte)
                End If
            Else
                Trace("L'URL du serveur web n'est pas indiquée dans les paramètres de ce dossier ! ", FichierTrace.niveau.critique)
            End If
        Else
            Trace("La vesrion du serveur web n'est pas indiquée dans les paramètres de ce dossier ! ", FichierTrace.niveau.critique)
        End If

    End Sub

End Class
