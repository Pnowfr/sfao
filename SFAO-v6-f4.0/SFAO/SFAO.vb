﻿Imports System.Configuration
Imports System.IO
Imports System.Text

Module SFAO
    Public Test As Boolean                              'Variable pour les tests
    Public ConnLoc As ConnexionLocaleSqlLite            'classe pour la base locale sqlite (non instentié car au 1er ancement on n'a pas la connexion string)
    Public Logo As New Logo                             'classe pour l'affichage du logo
    Public Crypt As New Crypt3Des                       'classe pour le chiffrement / déchiffrement des mots de passe
    Public X3ws As X3WebServ                            'classe des web services X3 (ne pas instentier imédiatement)
    Public WsNbrAsyncInvoke As Integer                  'Variable qui permet de compter les appels de web services asynchrones en attente 
    Public ParamX3 As New List(Of ParamWS)              'classe pour la liste des paramètres provenants des web services X3
    Public Site As New WSSite                           'classe desinfos du site connecté
    Public Poste As New WSPoste                         'classe des infos du poste connecté
    Public Events As New WSEvt                          'classe des infos des événements du poste
    Private FichTrace As FichierTrace                   'classe de gestion des traces
    Public Phases As New List(Of Phase)                 'classe des phases
    Public UpdateTimer As Timer                         'timer de recherche de mise à jour
    Private IDCnx As Integer                            'identifiant de connection à la base locale
    Private CheckUpd As Boolean = True                  'variable de gestion de mise à jour auto 
    Public Sub Main()
        Dim ResultLogin As DialogResult

        'Enregistrer fichier sfao.run
        System.IO.File.Create("sfao.run").Dispose()

        'Ouverture trace connexion
        FichTrace = New FichierTrace(FichierTrace.typeTrace.connexion)
        OuvreTrace()
        Trace("Démarrage SFAO")

        'TODO : Tests PNO  (à enlever)  
        'Test = True

        Logo.Show()                                     'Affichage du logo (sera masqué à l'affichage de la fanêtre de connexion)

        GenFichierConf()                                'Génération du fichier conf lors du 1er lancement 

        Application.DoEvents()
        SFAO.Sleep(1500)

        'TODO: Logo à réactiver
        Logo.Hide()                                     'on cache le logo avant l'affichage de la fenêtre de connexion
        Logo.Dispose()

        'Actie timer de vérification des mise à jour
        Trace("Activation du timer de recherche de mise à jour")
        UpdateTimer = New Timer()
        UpdateTimer.Interval = 3000
        AddHandler UpdateTimer.Tick, AddressOf UpdateTimer_tick
        UpdateTimer.Start()

        Trace("Lancement de la fenêtre de connexion")
        ResultLogin = Login.ShowDialog()                'la connexion à la base locale est instencié 
        Login.Dispose()

        Trace("Résultat Login : " & ResultLogin.ToString)

        'Fermeture du fichier de trace connexion
        FermeTrace()

        If ResultLogin = DialogResult.OK Then

            'Ouverture trace SFAO
            FichTrace = New FichierTrace(FichierTrace.typeTrace.sfao)
            OuvreTrace()
            Trace("Site : " & SFAO.Site.GRP1.FCY, FichierTrace.niveau.toujours)
            Trace("Dossier : " & SFAO.Site.GRP1.DOSSIER, FichierTrace.niveau.toujours)
            Trace("Poste : " & SFAO.Poste.GRP1.WST, FichierTrace.niveau.toujours)

            ConnLoc = New ConnexionLocaleSqlLite            'on instentie la connexion locale

            Trace("Fonction de création/mise à jour de la base locale Sqlite")
            If ConnLoc.ControleBaseLocale = True Then       'Création/mise à jour de la base locale Sqlite

                'Enregistrement connexion du poste dans la table CNXLOG
                Trace("Enregistrement connexion du poste dans la table CNXLOG")
                Try
                    IDCnx = ConnLoc.ConnexionPoste(SFAO.Site.GRP1.FCY, SFAO.Site.GRP1.DOSSIER, SFAO.Poste.GRP1.WST)
                Catch ex As Exception
                    Trace("Erreur de connexion à la base locale !", FichierTrace.niveau.critique)
                End Try

                If IDCnx > 0 Then
                    'sauvegarde des valeurs login saisies
                    SaveDerniereSaisi()

                    'Lancement Fenêtre SFAO
                    Trace("Lancement fenêtre SFAO", FichierTrace.niveau.toujours)
                    Application.Run(FenSfao)
                    Trace("Fermeture fenêtre SFAO", FichierTrace.niveau.toujours)

                    'Enregistrement de la déconnexion du poste dans la table CNXLOG
                    Try
                        ConnLoc.DeConnexionPoste(SFAO.Site.GRP1.FCY, SFAO.Site.GRP1.DOSSIER, SFAO.Poste.GRP1.WST, IDCnx)
                    Catch ex As Exception
                        Trace("Erreur de déconnexion de la base locale !", FichierTrace.niveau.critique)
                    End Try

                    'fermeture du web service
                    X3ws.Close()

                Else
                    Trace("Erreur de connexion à la base locale !", FichierTrace.niveau.critique)
                End If
            Else
                Trace("Erreur de la base locale !", FichierTrace.niveau.critique)
            End If

            'Fermeture du fichier de trace Sfao
            FermeTrace()
        End If

        'Supprimer sfao.run
        System.IO.File.Delete("sfao.run")
    End Sub

    'Appel à la méthode pour ouvrir le fichier de traces
    Public Sub OuvreTrace()
        FichTrace.OuvreTrace()
    End Sub

    'Appel à la méthode pour écrire les traces
    Public Sub Trace(message As String, Optional niveau As FichierTrace.niveau = FichierTrace.niveau.information)
        FichTrace.Trace(message, niveau)
    End Sub

    'Appel à la méthode pour afficher les traces
    Public Sub AfficheTrace()
        FichTrace.AfficheTrace()
    End Sub

    'Appel à la méthode pour fermer le fichier de traces
    Public Sub FermeTrace()
        FichTrace.FermeTrace()
    End Sub

    'génération du fichier de paramètres si non existant
    Private Sub GenFichierConf()
        'Enregistrement de la connexion string pour la base locale

        Dim DefaultConnectionStringLocale As String = "DataSource={0}\{1}\{2}\{3}\baselocale{3}.db;Version=3;"

        Try
            If ConfigurationManager.ConnectionStrings("ConnexionLocale") Is Nothing Then
                AddConnectionString("ConnexionLocale", DefaultConnectionStringLocale, "System.Data.SQLite")
                Trace("Création du paramètres connexionString <ConnexionLocale> dans le fichier conf")
            End If
        Catch ex As Exception

            AddConnectionString("ConnexionLocale", DefaultConnectionStringLocale, "System.Data.SQLite")
            Trace("Création du paramètres connexionString <ConnexionLocale> dans le fichier conf")

        End Try

        'ajout des paramètres 
        Try
            AddPar("SOCIETE", "BRODART PACKAGING")

            'TODO (plus tard) ajouter le site en fonction de la plage IP locale
            AddPar("SITE", "BRODART")
            'AddPar("SITE", "TILWEL")

            AddPar("POSTE", "")
            AddPar("MULTIPOSTE", "FAUX")
            AddPar("SANSMOTDEPASSE", "VRAI")
            AddPar("MOTDEPASSE", Crypt.Crypte("SFAO"))
            AddPar("REPBASELOCALE", "BDL")
            AddPar("REPTBLCSV", "Resources")
            AddPar("TRACECONNEXION", "VRAI")
            AddPar("TRACESFAO", "VRAI")
            AddPar("REPTRACECONNEXION", "Traces")
            AddPar("REPTRACESFAO", "Traces")
            AddPar("NIVEAUTRACE", "0")
            AddPar("REPUPDSFAO", "")

            'paramètres par dossier : 
            AddPar("WEBSERVEURPOOLALIAS", "GBIV6", "GBIV6")
            AddPar("WEBSERVEURVERSION", "V6", "GBIV6")
            AddPar("WEBSERVEURLANG", "FRA", "GBIV6")
            AddPar("WEBSERVEURURL", "http://192.168.1.248:28880/adxwsvc/services/CAdxWebServiceXmlCC", "GBIV6")
            AddPar("WEBSERVEURUSER", "web", "GBIV12")
            AddPar("WEBSERVEURPWD", "GiwpM0kjZZ1hNI0vCTt2wg==", "GBIV6")
            AddPar("WEBSERVEURTIMEOUT", "15000", "GBIV6")
            AddPar("WEBSERVEURPARAM", "adxwss.optreturn=JSON&adxwss.beautify=true", "GBIV6")
            AddPar("WEBSERVEURTIMTEST", "180000", "GBIV6")

            AddPar("WEBSERVEURPOOLALIAS", "GBIV12", "GBIV12")
            AddPar("WEBSERVEURVERSION", "V12", "GBIV12")
            AddPar("WEBSERVEURLANG", "FRA", "GBIV12")
            AddPar("WEBSERVEURURL", "http://192.168.0.204/soap-generic/syracuse/collaboration/syracuse/CAdxWebServiceXmlCC", "GBIV12")
            AddPar("WEBSERVEURUSER", "web", "GBIV12")
            AddPar("WEBSERVEURPWD", "GiwpM0kjZZ1hNI0vCTt2wg==", "GBIV12")
            AddPar("WEBSERVEURTIMEOUT", "15000", "GBIV12")
            AddPar("WEBSERVEURPARAM", "adxwss.optreturn=JSON&adxwss.beautify=true", "GBIV12")
            AddPar("WEBSERVEURTIMTEST", "180000", "GBIV12")

            'TODO Connexion externe pour les tests à enlever
            AddPar("WEBSERVEURPOOLALIAS", "GBIV12", "GBIV12 (ext)")
            AddPar("WEBSERVEURVERSION", "V12", "GBIV12 (ext)")
            AddPar("WEBSERVEURLANG", "FRA", "GBIV12 (ext)")
            AddPar("WEBSERVEURURL", "http://77.158.76.196/soap-generic/syracuse/collaboration/syracuse/CAdxWebServiceXmlCC", "GBIV12 (ext)")
            AddPar("WEBSERVEURUSER", "web", "REING (ext)")
            AddPar("WEBSERVEURPWD", "GiwpM0kjZZ1hNI0vCTt2wg==", "GBIV12 (ext)")
            AddPar("WEBSERVEURTIMEOUT", "15000", "GBIV12 (ext)")
            AddPar("WEBSERVEURPARAM", "adxwss.optreturn=JSON&adxwss.beautify=true", "GBIV12 (ext)")
            AddPar("WEBSERVEURTIMTEST", "180000", "GBIV12 (ext)")

            AddPar("WEBSERVEURPOOLALIAS", "REING", "REING")
            AddPar("WEBSERVEURLANG", "FRA", "REING")
            AddPar("WEBSERVEURURL", "http://192.168.0.204/soap-generic/syracuse/collaboration/syracuse/CAdxWebServiceXmlCC", "REING")
            AddPar("WEBSERVEURUSER", "web", "REING")
            AddPar("WEBSERVEURPWD", "GiwpM0kjZZ1hNI0vCTt2wg==", "REING")
            AddPar("WEBSERVEURTIMEOUT", "15000", "REING")
            AddPar("WEBSERVEURPARAM", "adxwss.optreturn=JSON&adxwss.beautify=true", "REING")
            AddPar("WEBSERVEURTIMTEST", "180000", "REING")

            'TODO Connexion externe pour les tests à enlever
            AddPar("WEBSERVEURPOOLALIAS", "REING", "REING (ext)")
            AddPar("WEBSERVEURLANG", "FRA", "REING (ext)")
            AddPar("WEBSERVEURURL", "http://77.158.76.196/soap-generic/syracuse/collaboration/syracuse/CAdxWebServiceXmlCC", "REING (ext)")
            AddPar("WEBSERVEURUSER", "web", "REING (ext)")
            AddPar("WEBSERVEURPWD", "GiwpM0kjZZ1hNI0vCTt2wg==", "REING (ext)")
            AddPar("WEBSERVEURTIMEOUT", "15000", "REING (ext)")
            AddPar("WEBSERVEURPARAM", "adxwss.optreturn=JSON&adxwss.beautify=true", "REING (ext)")
            AddPar("WEBSERVEURTIMTEST", "180000", "REING (ext)")

        Catch ex As Exception
            Trace("Erreur: impossible d'ajouter des paramètres dans le fichier conf!", FichierTrace.niveau.erreur)
        End Try
    End Sub

    Private Sub SaveDerniereSaisi()

        Try
            If SFAO.Site.GRP1.FCYSHO <> "" Then
                AddPar("LASTSITE", SFAO.Site.GRP1.FCYSHO)
            End If
            If SFAO.Site.GRP1.DOSSIER <> "" Then
                AddPar("LASTDOSSIER", SFAO.Site.GRP1.DOSSIER)
            End If
            If SFAO.Poste.GRP1.WST <> "" Then
                AddPar("LASTPOSTE", SFAO.Poste.GRP1.WST)
            End If
            Trace("Sauvegarde de la dernière saisie dans le fichier conf")
        Catch ex As Exception
            Trace("Erreur: impossible d'ajouter les paramètres de la dernière saisie dans le fichier conf!", FichierTrace.niveau.erreur)
        End Try
    End Sub
    'enregistrement d'un paramètre connexion string 
    Public Sub AddConnectionString(name As String, connectionString As String, providerName As String)
        Dim configFile As Configuration
        Dim csSettings As ConnectionStringSettings
        Dim csSection As ConnectionStringsSection
        Try
            configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            csSettings = New ConnectionStringSettings(name, connectionString, providerName)
            csSection = configFile.ConnectionStrings
            csSection.ConnectionStrings.Remove(csSettings)
            csSection.ConnectionStrings.Add(csSettings)
            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection(configFile.ConnectionStrings.SectionInformation.Name)
        Catch ex As Exception
            Trace("Erreur d'enregistrement du paramètre connection string " & name & " !", FichierTrace.niveau.erreur)
        End Try
    End Sub

    'lecture des paramètres à partir du fichier sfao.exe.config
    Public Function Param(key As String) As String
        Dim result As String = String.Empty
        Dim appSettings As Specialized.NameValueCollection
        Try
            appSettings = ConfigurationManager.AppSettings
            result = appSettings(key)
            If IsNothing(result) Then
                result = String.Empty
            End If
        Catch e As ConfigurationErrorsException
            Trace("Erreur de lecture du paramètre " & key & " !", FichierTrace.niveau.erreur)
        End Try
        Return result
    End Function

    'enregistrement des paramètres dans le fichier sfao.exe.config
    Public Sub AddParam(key As String, value As String)
        Dim configFile As Configuration
        Dim settings As KeyValueConfigurationCollection
        Try
            configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            settings = configFile.AppSettings.Settings
            If IsNothing(settings(key)) Then
                settings.Add(key, value)
            Else
                settings(key).Value = value
            End If
            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
        Catch e As ConfigurationErrorsException
            Trace("Erreur d'enregistrement du paramètre " & key & " !", FichierTrace.niveau.erreur)
        End Try
    End Sub
    'lecture des paramètres dossier à partir du fichier sfao.exe.config
    Public Function ParamDos(key As String, dossier As String) As String
        Dim result As String = String.Empty
        Dim appSettings As Specialized.NameValueCollection
        Dim keydos As String
        keydos = key & "-" & dossier
        Try
            appSettings = ConfigurationManager.AppSettings
            result = appSettings(keydos)
            If IsNothing(result) Then
                result = String.Empty
            End If
        Catch e As ConfigurationErrorsException
            Trace("Erreur de lecture du paramètre " & keydos & " !", FichierTrace.niveau.erreur)
        End Try
        Return result
    End Function
    'enregistrement des paramètres dossier dans le fichier sfao.exe.config 
    Public Sub AddParamDos(key As String, dossier As String, value As String)
        Dim configFile As Configuration
        Dim settings As KeyValueConfigurationCollection
        Dim keydos As String
        keydos = key & "-" & dossier
        Try
            configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            settings = configFile.AppSettings.Settings
            If IsNothing(settings(keydos)) Then
                settings.Add(keydos, value)
            Else
                settings(keydos).Value = value
            End If
            configFile.Save(ConfigurationSaveMode.Modified)
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name)
        Catch e As ConfigurationErrorsException
            Trace("Erreur d'enregistrement du paramètre " & keydos & " !", FichierTrace.niveau.erreur)
        End Try
    End Sub
    Public Sub AddPar(key As String, value As String, Optional dossier As String = "")
        If dossier <> "" Then
            If SFAO.ParamDos(key, dossier) = "" Then
                SFAO.AddParamDos(key, dossier, value)
            End If
        Else
            If SFAO.Param(key) = "" Then
                SFAO.AddParam(key, value)
            End If
        End If
    End Sub
    'fonction qui retire tous les caractères spéciaux d'une chaine
    Public Function RemoveAccents(ByVal s As String) As String
        Dim normalizedString As String = s.Normalize(NormalizationForm.FormD)
        Dim stringBuilder As New StringBuilder()
        Dim i As Integer
        Dim c As Char
        For i = 0 To normalizedString.Length - 1
            c = normalizedString(i)
            If System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) <> System.Globalization.UnicodeCategory.NonSpacingMark Then
                stringBuilder.Append(c)
            End If
        Next
        Return stringBuilder.ToString
    End Function
    'Fonction qui affiche la date et l'heure selon le format SFAO
    Public Function AffDateHeure(ByVal _date As Date, ByVal _time As Integer) As String
        If _date = CDate("1/1/0001") OrElse _date = CDate("31/12/1599") Then 'date nulle 15991231
            Return String.Empty
        ElseIf _date <> Now.Date Then
            Return _date.ToString("dd/MM") + " " + ZHM(_time)
        Else
            Return ZHM(_time)
        End If
    End Function
    'Fonction qui convertie une durée en secondes en affichage HH:MM 
    Public Function ZHM(ByVal _time As Integer) As String
        Dim H As Integer
        Dim M As Integer
        H = CInt(Math.Floor(_time / 3600))
        M = CInt(Math.Floor((_time - (H * 3600)) / 60))
        Return H.ToString("00") + ":" + M.ToString("00")
    End Function
    'Fonction qui calcule la durée la date dates + time (time en secondes) et le datetime donnée (now) dans l'unité demandée
    Public Function Calc_Duree(ZDD As Date, ZTD As Integer, ZDF As DateTime, ZUNITE As Integer) As Decimal
        '                                                       ZUNITE       #1=heures, 2 = minutes, 3 = secondes
        Dim RETOUR As Decimal
        Dim ZNBJ, ZNBJS, ZDIFS, ZTF As Integer

        If Year(ZDD) > 1900 And Year(ZDF) > 1900 Then
            ZNBJ = CInt(DateDiff(DateInterval.Day, ZDD, ZDF))
            If ZNBJ < 99999 Then
                ZNBJS = ZNBJ * 86400
                ZTF = (ZDF.Hour * 3600) + (ZDF.Minute * 60) + ZDF.Second
                ZDIFS = ZTF - ZTD
                RETOUR = ZNBJS + ZDIFS
                Select Case ZUNITE
                    Case 1 '1=heures
                        RETOUR = Math.Round(RETOUR / 3600, 2)                       'On arrondi les heures à 0.01 soit à 36 secondes près
                        If RETOUR >= 10000 Then RETOUR = CDec(9999.99)              'maxi autorisé dans X3 (9999.9999)

                    Case 2 '2=minutes
                        RETOUR = Math.Round(RETOUR / 60, 1)                         'On arrondi les minutes à 0.1 soit à 6 secondes près
                        If RETOUR >= 10000 Then RETOUR = CDec(9999.9)               'maxi autorisé dans X3 (9999.9999)
                End Select
            Else
                RETOUR = CDec(9999.99)
            End If
        Else
            RETOUR = 0
        End If

        Return RETOUR
    End Function

    'Affichage d'une durée en secondes sous le format J:HH ou HH:MM
    Public Function AFF_TPS_JHM(ZTPS As Decimal) As String
        Dim RETOUR As String
        Dim NBMN, NBHR, NBJR As Integer

        NBJR = CInt(Math.Floor(ZTPS / 1440))
        NBHR = CInt(Math.Floor((ZTPS / 60) - (NBJR * 24)))
        NBMN = CInt(Math.Floor(ZTPS - NBHR * 60 - NBJR * 1440))

        If NBJR > 0 Then
            RETOUR = NBJR.ToString + "j " + NBHR.ToString + "h"
        Else
            RETOUR = NBHR.ToString + "h " + NBMN.ToString("00")
        End If

        Return RETOUR
    End Function

    'Fonction pour convertir les unités 
    Public Function AFF_UNIT(UNIT As String) As String
        Select Case UNIT
            Case "MLF"
                AFF_UNIT = "ML"
            Case "M2"
                AFF_UNIT = "M²"
            Case Else
                AFF_UNIT = UNIT
        End Select
    End Function
    Public Sub Sleep(ByVal time As Integer)
        Dim i As Integer
        For i = 0 To CInt(time / 10)
            Threading.Thread.Sleep(10)
            Application.DoEvents()
        Next
    End Sub
    Private Sub UpdateTimer_tick(sender As Object, e As EventArgs)
        Dim closeFile As String
        closeFile = Application.StartupPath.ToString & "\sfao.close"
        If My.Computer.FileSystem.FileExists(closeFile) Then
            Trace("Fichier de fermeture trouvé : " & closeFile, FichierTrace.niveau.toujours)
            Trace("Fermeture de la SFAO", FichierTrace.niveau.toujours)

            My.Computer.FileSystem.DeleteFile(closeFile)

            If IDCnx > 0 Then
                Try
                    ConnLoc.DeConnexionPoste(SFAO.Site.GRP1.FCY, SFAO.Site.GRP1.DOSSIER, SFAO.Poste.GRP1.WST, IDCnx)
                Catch ex As Exception
                    'aucune exception gérée
                End Try

            End If

            FermeTrace() 'on ferme le fichier de trace
            System.IO.File.Delete("sfao.run")
            Application.Exit() 'on quitte 
            End
        Else
            If CheckUpd Then
                Call VerifUpdate()
            End If
        End If
    End Sub
    Private Sub VerifUpdate()
        Dim _Dir As DirectoryInfo
        Dim sfaoUpdateExe As String = "SFAO-Upd.exe"
        Dim sfaoPath As String = Application.StartupPath    'Le répertoire de l'application actuelle
        Dim versionslist As New List(Of String)
        Dim version As String = Application.ProductVersion  'La version actuelle
        Dim repUpdSfao As String = Param("REPUPDSFAO")      'Le répertoire des updates
        Dim proc As Process                                 'Initialisation d'un Process

        If repUpdSfao = "" Then
            CheckUpd = False 'on ne refait plus de vérifications de mise à jours dans cette session
            Trace("[VerifUpdate] La paramètre REPUPDSFAO du chemin des mise à jours est vide ! Mise à jour désactivée !", FichierTrace.niveau.erreur)
            Exit Sub
        End If

        Dim _main_Dir As New IO.DirectoryInfo(repUpdSfao)

        Try
            'Ajout dans la liste des versions dispos dans le dossier
            For Each _Dir In _main_Dir.GetDirectories
                versionslist.Add(_Dir.Name)
            Next
        Catch ex As Exception
            CheckUpd = False 'on ne refait plus de vérifications de mise à jours dans cette session
            Trace("[VerifUpdate] Erreur de lecture du chemin des mise à jours : " & repUpdSfao & " ! Mise à jour désactivée !", FichierTrace.niveau.erreur)
            Exit Sub
        End Try

        'Vérifier si dans le dossier Update (indiqué par le paramètre), on a une verssion + récente
        'Vérifier si le nom du dernier dossier de la liste est le même que la version actuelle
        Dim listVersionLength As Integer = versionslist.Count
        Dim lastVersion As String = versionslist(listVersionLength - 1)
        'Vérifier si une mise à jour est disponible
        If lastVersion.Equals(version) Then
            'MsgBox(Prompt:="Pas de version plus récente") pas de message ! le contrôle se fait toutes les 30 secondes !!! 
            Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " [VerifUpdate] Pas de version plus récente")
        Else
            Trace("[VerifUpdate] Version SFAO plus récente trouvée : " & lastVersion & " , lancement de la mise à jour.", FichierTrace.niveau.toujours)
            'Si on a une version + récente on vérifie si SFAO-Upd.exe est plus récent(??), si oui on le copie dans le dossier de l'appli client
            Dim repSFAO_UpdExePath As String = repUpdSfao & "\" & lastVersion
            Dim repSFAO_UpdExe As String = repUpdSfao & "\" & lastVersion & "\" & sfaoUpdateExe
            'Le répertoire de la version
            Dim repertoireAppliClient As String = sfaoPath & "\" & sfaoUpdateExe
            'Vérifier si le fichier SFAO-Upd.exe existe
            If File.Exists(Path.Combine(repSFAO_UpdExePath, sfaoUpdateExe)) Then
                Trace("[VerifUpdate] Nouveau SFAO-Upd.exe  trouvée, " & " , copie en local.", FichierTrace.niveau.toujours)
                'On le copie dans le dossier de l'appli client
                Try
                    FileIO.FileSystem.CopyDirectory(repSFAO_UpdExe, repertoireAppliClient)
                Catch ex As Exception
                    CheckUpd = False 'on ne refait plus de vérifications de mise à jours dans cette session
                    Trace("[VerifUpdate] Erreur de copie de SFAO-Upd.exe ! Mise à jour désactivée !", FichierTrace.niveau.erreur)
                    Exit Sub
                End Try
            End If

            'On execute SFAO-upd.exe soit le récent copié soit l'ancien déjà existant
            Try
                Trace("[VerifUpdate] Lancement de SFAO-Upd.exe", FichierTrace.niveau.toujours)
                proc.Start(sfaoPath & "\" & sfaoUpdateExe)
            Catch ex As Exception
                CheckUpd = False 'on ne refait plus de vérifications de mise à jours dans cette session
                Trace("[VerifUpdate] Erreur de lancement de SFAO-Upd.exe ! Mise à jour désactivée !", FichierTrace.niveau.erreur)
                Exit Sub
            End Try
        End If
    End Sub
End Module
