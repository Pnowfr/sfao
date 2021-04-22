Imports System.Configuration
Imports System.IO
Imports System.Xml

Public Class Upd

    Private Sub Upd_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim reperSfao As String = Application.StartupPath    'SFAO-Upd.exe est dans le même répertoire que sfao.exe et le fichier .config
        Dim reperFichConfig As String = reperSfao & "SFAO.exe.config"
        Dim valParamREPUPDSFAO As String = GetParamValue(reperFichConfig, "REPUPDSFAO")
        Dim reperNouvelleVersion As String = valParamREPUPDSFAO
        'Dim valParamREPTRACECONNEXION As String = GetParamValue(reperFichConfig, "REPTRACECONNEXION")
        'Dim valParamREPTRACESFAO As String = GetParamValue(reperFichConfig, "REPTRACESFAO")
        Dim versionslist As New List(Of String)
        Dim Response As Integer
        Dim repUpdExe As String = reperSfao & "SFAO-Upd.exe"

        'Vérifier si le paramètre REPUPDSFAO existe + non vide dans SFAO.exe.config
        If valParamREPUPDSFAO <> "" Then
            'TODO Semble inutile de supprimer les traces
            'Vérifier le paramètre REPTRACECONNEXION et REPTRACESFAO et exclure ces dossiers de la mise à jour s'il s'agit de sous-dossiers de l'appli
            'If valParamREPTRACECONNEXION <> "" AndAlso valParamREPTRACESFAO <> "" Then
            'Dim CheminReptraceConnexion As String = valParamREPTRACECONNEXION
            'Dim CheminReptraceSfao As String = valParamREPTRACESFAO
            ''Vérification si CheminReptraceConnexion est dans le répertoire du projet
            'If Directory.Exists(CheminReptraceConnexion) Then
            ''Exclure ces dossiers de la mise à jour
            'For Each dir As String In Directory.GetDirectories(CheminReptraceConnexion)
            'DeleteDirectory(dir)
            'Next
            'End If
            'Vérification si CheminReptraceSfao est dans le répertoire du projet
            'If Directory.Exists(CheminReptraceSfao) Then
            ''Exclure ces dossiers de la mise à jour
            'For Each dir As String In Directory.GetDirectories(CheminReptraceSfao)
            'DeleteDirectory(dir)
            'Next
            'End If
            'End If
        Else
            Application.Exit() 'on quitte
        End If

        'Vérifier si dans le dossier Update (indiqué par le paramètre), on a une version + récente
        Dim version As String = Application.ProductVersion 'TODO récupérer la version de SFAO.exe et pas SFAO-Upd.exe (soit version de l'exe si non dans le config)
        Dim sfaoClose As String = "sfao.close"
        Dim sfaoRun As String = "sfao.run"
        Dim sfaoPath As String = Application.StartupPath 'TODO même variable que reperSfao
        Dim checkSfaoRun As String = sfaoPath & "\" & sfaoRun
        Dim checkSfaoClose As String = Path.Combine(sfaoPath, sfaoClose)
        Dim _main_Dir As New IO.DirectoryInfo(valParamREPUPDSFAO)

        'Ajout dans la liste des versions dispos dans le dossier update
        For Each _Dir In _main_Dir.GetDirectories
            versionslist.Add(_Dir.Name)
        Next

        'Vérifier si le nom du dernier dossier de la liste est le même que la version actuelle
        Dim listVersionLength As Integer = versionslist.Count
        Dim derniereVersion As String = versionslist(listVersionLength - 1)
        Dim repSFAO_UpdExePath As String = valParamREPUPDSFAO & "\" & derniereVersion & "\"


        'Vérifier si une mise à jour est disponible
        If derniereVersion.Equals(version) Then
            MsgBox(Prompt:="Pas de mise à jour disponible")
        Else
            'Verifier si mise à jour obligatoire (bouton OK) ou facultative (boutons OK/Annuler)
            If Not (derniereVersion.Substring(1).Equals(version.Substring(1))) Then
                'Mise à jour obligatoire
                Response = vbYes
            Else 'Si ce n'est pas une mise à jour obligatoire
                Response = MsgBox(Prompt:="Nouvelle version disponible: " + derniereVersion + vbNewLine + "Voulez-vous mettre à jour vers cette nouvelle version?", Buttons:=vbYesNo)

            End If
            If Response = vbYes Then
                'Vérifier si l'application sfao tourne, si OK, on crée le fichier sfao.close
                If File.Exists(checkSfaoRun) Then
                    File.Create(Path.Combine(sfaoPath, sfaoClose))
                    'TODO laisser  la SFAO le temps de se fermer '20 secondes
                    'TODO afficher une fenêtre pour patienter
                    Sleep(2000, 1000)
                End If
                MisAJourVersion(sfaoClose, reperSfao, repUpdExe, reperNouvelleVersion, repSFAO_UpdExePath, checkSfaoRun, derniereVersion)

            End If
        End If
    End Sub

    'Méthode de lecture de fichier xml
    Private Function GetParamValue(repertFichierXml As String, key As String) As String
        Dim xmlDoc As New XmlDocument
        Dim value As String = ""
        xmlDoc.Load(repertFichierXml) 'opens XML file
        Dim keyNodes = xmlDoc.GetElementsByTagName("appSettings")
        For Each singleKeyNode As XmlNode In keyNodes
            For Each keyChildNode As XmlNode In singleKeyNode.ChildNodes
                Dim valKey = keyChildNode.Attributes.Item(0).Value
                If valKey = key Then
                    value = keyChildNode.Attributes.Item(1).Value
                    Return value
                End If
            Next
        Next
        Return value
    End Function

    'Méthode appelée pour mise à jour version
    Private Sub MisAJourVersion(sfaoClose As String, repersfao As String, reperUpdexe As String, reperNouvelleVersion As String, repSFAO_UpdExePath As String, checkSfaoRun As String, derniereVersion As String)
        If File.Exists(checkSfaoRun) Then
            'TODO Message à l'utilisateur : La SFAO est toujours en cours d'exection, veuillez l'arrêter et relaner la mise à jour
            'fermer la SFAO
            Application.Exit()
        End If
        MsgBox("Mise à jour en cours.....")

        'Traiter le remplacement des fichiers
        CopieNouvelleVersion(repersfao, reperUpdexe, repSFAO_UpdExePath, reperNouvelleVersion, derniereVersion)

        'supprimer le sfao.close
        File.Delete(Path.Combine(repersfao, sfaoClose))
    End Sub

    'Méthode qui supprime les dossiers de l'ancienne version et copie la nouvelle
    Private Sub CopieNouvelleVersion(reperVersion As String, repUpdexe As String, repSFAO_UpdExePath As String, reperNouvelleVersion As String, derniereVersion As String)
        'Copie des dossiers de la nouvelle version vers l'appli client
        'Parcours de fichiers qui sont dans le répertoire de la nouvelle version
        Dim listFichiersNvVersion As New List(Of String)  'Liste qui va contenir les fichiers de la nouvelle version
        Dim listDossiersNvVersion As New List(Of String)  'Liste qui va contenir les dossiers de la nouvelle version
        Dim _dir_nvvers As New IO.DirectoryInfo(repSFAO_UpdExePath)

        'Dim fichDll As IO.FileInfo() = _dir_nvvers.GetFiles("*.dll")
        'Dim fichConfig As IO.FileInfo() = _dir_nvvers.GetFiles("*.config")
        'Dim fichXml As IO.FileInfo() = _dir_nvvers.GetFiles("*.xml")
        Dim fichall As IO.FileInfo() = _dir_nvvers.GetFiles("*.*")
        Dim fi As IO.FileInfo
        'Dim fiConf As IO.FileInfo
        'Dim fiXml As IO.FileInfo

        Try
            'Ajout des dossiers dans la liste des versions
            For Each _Dir In _dir_nvvers.GetDirectories
                listDossiersNvVersion.Add(_Dir.Name)
            Next
            'Ajout des fichiers *.*
            For Each fi In fichall
                listFichiersNvVersion.Add(fi.Name)
            Next
            'Ajout des fichiers config
            'For Each fiConf In fichConfig
            ' listFichiersNvVersion.Add(fiConf.Name)
            'Next
            'Ajout des fichiers xml
            'For Each fiXml In fichXml
            ' listFichiersNvVersion.Add(fiXml.Name)
            'Next

            'Copie des dossiers de la liste vers le répertoire de l'application actuelle
            'TODO copier aussi le dossier racine en excluant SFAO-upd.exe
            'TODO pendant la copie afficher une fenêtre avec barre de progression + affichage des fichiers+dossiers copiés
            For Each dossNv As String In listDossiersNvVersion
                Try
                    Dim sDestination As String = reperVersion & "\" & dossNv
                    Dim sSource As String = repSFAO_UpdExePath & dossNv

                    'fonction récursive pour copier les dossiers et fichiers
                    CopyDirectory(sSource, sDestination)

                Catch ex As Exception
                    MsgBox(" Erreur lors de la copie du dossier  : " & ex.Message)
                    Exit Sub
                End Try
            Next

            'Arrêt d'une application
            Application.Exit()

            'Copie des fichiers de la liste vers le répertoire de l'application actuelle
            For Each fichNv As String In listFichiersNvVersion
                Try
                    'Copie du ficher dans le répertoire en écrasant les dossiers et fichiers du même nom
                    Dim sDestination As String = reperVersion & fichNv
                    Dim sSource As String = repSFAO_UpdExePath & fichNv
                    'On procède à la copie
                    File.Copy(sSource, sDestination, overwrite:=True)

                Catch ex As Exception
                    MsgBox(" Erreur lors de la copie du fichier  : " & fichNv & ex.Message)
                    Exit Sub
                End Try
            Next
            'Copie du fichier SFAO-Upd.exe du répertoire serveur-upd vers le répertoire du projet pour exécution
            Dim reperLastVersion As String = reperNouvelleVersion & " \" & derniereVersion & "\SFAO-Upd.exe"
            File.Copy(repUpdexe, reperLastVersion, overwrite:=True)
        Catch ex As Exception
            MsgBox(" Erreur de lecture du chemin de la nouvelle version : " & repSFAO_UpdExePath & ex.Message)
            Exit Sub
        End Try
    End Sub

    'Méthode qui permet de lire un fichier csv    
    Private Function lireCsv(pathFichierCsv As String) As List(Of String)
        Dim listeFichiers As New List(Of String)()
        For Each ligne As String In File.ReadLines(pathFichierCsv)
            Console.WriteLine(ligne.ToString)
            listeFichiers.Add(ligne.ToString)
        Next ligne
        Return listeFichiers
    End Function


    'Fonction récursive pour la copie des dossiers, sous dossiers etc...
    Public Sub CopyDirectory(ByVal sourcePath As String, ByVal destinationPath As String)
        Dim sourceDirectoryInfo As New DirectoryInfo(sourcePath)

        ' Si le dossier de destination n'existe pas, le créer
        If Not Directory.Exists(destinationPath) Then
            Directory.CreateDirectory(destinationPath)
        End If

        Dim fileSystemInfo As FileSystemInfo
        For Each fileSystemInfo In sourceDirectoryInfo.GetFileSystemInfos
            Dim destinationFileName As String =
                Path.Combine(destinationPath, fileSystemInfo.Name)

            ' Maintenant, vérifier si c'est un dossier ou un fichier et faire les actions correspondantes
            If TypeOf fileSystemInfo Is System.IO.FileInfo Then
                File.Copy(fileSystemInfo.FullName, destinationFileName, True)
            Else
                ' Appeler la méthode récursivement si nécessaire
                CopyDirectory(fileSystemInfo.FullName, destinationFileName)
            End If
        Next
    End Sub

    'Méthode qui permet de supprimer tous les fichiers d'un répertoire
    Private Sub DeleteDirectory(path As String)
        If Directory.Exists(path) Then
            'Delete all child Directories
            For Each dir As String In Directory.GetDirectories(path)
                My.Computer.FileSystem.DeleteDirectory(path, FileIO.DeleteDirectoryOption.DeleteAllContents)
            Next
        End If
    End Sub
    'attend x milisecondes avec hachage de y milisecondes 
    Private Sub Sleep(ByVal time As Integer, Optional hash As Integer = 100)
        Dim i As Integer
        For i = 0 To CInt(time / hash)
            Threading.Thread.Sleep(hash)
            Application.DoEvents()
        Next
    End Sub
End Class