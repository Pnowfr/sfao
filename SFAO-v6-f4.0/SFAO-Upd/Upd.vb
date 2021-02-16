Imports System.Configuration
Imports System.IO
Imports System.Xml

Public Class Upd
    Private Sub Upd_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim sfaoPath As String = Application.StartupPath
        Dim reperUpdSfao As String = sfaoPath.ToString & "\SFAO.exe.config"
        Dim valParamREPUPDSFAO As String = GetParamValue(reperUpdSfao, "REPUPDSFAO")
        Dim valParamREPTRACECONNEXION As String = GetParamValue(reperUpdSfao, "REPTRACECONNEXION")
        Dim valParamREPTRACESFAO As String = GetParamValue(reperUpdSfao, "REPTRACESFAO")
        Dim versionslist As New List(Of String)
        Dim Response As Integer


        'Vérifier si le paramètre REPUPDSFAO existe + non vide dans SFAO.exe.config
        If valParamREPUPDSFAO <> "" Then
            'Vérifier le paramètre REPTRACECONNEXION et REPTRACESFAO et exclure ces dossiers de la mise à jour s'il s'agit de sous-dossiers de l'appli
            If valParamREPTRACECONNEXION <> "" AndAlso valParamREPTRACESFAO <> "" Then
                Dim CheminReptraceConnexion As String = valParamREPTRACECONNEXION
                Dim CheminReptraceSfao As String = valParamREPTRACESFAO
                'Vérification si CheminReptraceConnexion est dans le répertoire du projet
                If Directory.Exists(CheminReptraceConnexion) Then
                    'Exclure ces dossiers de la mise à jour
                    For Each dir As String In Directory.GetDirectories(CheminReptraceConnexion)
                        DeleteDirectory(dir)
                    Next
                End If
                'Vérification si CheminReptraceSfao est dans le répertoire du projet
                If Directory.Exists(CheminReptraceSfao) Then
                    'Exclure ces dossiers de la mise à jour
                    For Each dir As String In Directory.GetDirectories(CheminReptraceSfao)
                        DeleteDirectory(dir)
                    Next
                End If
            End If
        Else
            Application.Exit() 'on quitte
        End If

        'Vérifier si dans le dossier Update (indiqué par le paramètre), on a une version + récente
        Dim version As String = Application.ProductVersion
        Dim sfaoClose As String = "sfao.close.txt"
        Dim sfaoRun As String = "sfao.run"
        Dim checkSfaoRun As String = sfaoPath & "\" & sfaoRun
        Dim checkSfaoClose As String = Path.Combine(sfaoPath, sfaoClose)
        Dim _main_Dir As New IO.DirectoryInfo(valParamREPUPDSFAO)

        'Ajout dans la liste des versions dispos dans le dossier
        For Each _Dir In _main_Dir.GetDirectories
            versionslist.Add(_Dir.Name)
        Next

        'Vérifier si le nom du dernier dossier de la liste est le même que la version actuelle
        Dim listVersionLength As Integer = versionslist.Count
        Dim lastVersion As String = versionslist(listVersionLength - 1)

        'Pour avoir le répertoire plus précis de la nouvelle version (inclus le fichier de la version)
        Dim reperReelNvVersion = valParamREPUPDSFAO & "\" & lastVersion

        'Vérifier si l'application sfao tourne, si OK, on crée le fichier sfao.close
        If File.Exists(checkSfaoRun) Then
            File.Create(Path.Combine(sfaoPath, sfaoClose))

            'Vérifier si une mise à jour est disponible
            If lastVersion.Equals(version) Then
                MsgBox("Pas de mise à jour disponible")
            Else
                'Verifier si mise à jour obligatoire (bouton OK) ou facultative (boutons OK/Annuler)move
                If Not (lastVersion.Substring(1, 3).Equals(version.Substring(1, 3))) Then
                    Console.WriteLine(version.Substring(4, 3) & "-" & lastVersion.Substring(4, 3))
                    'Mise à jour obligatoire
                    MisAJourVersion(sfaoClose, sfaoPath, reperReelNvVersion, checkSfaoClose, lastVersion)
                ElseIf Not (lastVersion.Substring(4, 3).Equals(version.Substring(4, 3))) Then 'Si c'est une mise à jour non obligatoire
                    Response = MsgBox(Prompt:="Nouvelle version disponible: " + lastVersion + vbNewLine + "Voulez-vous mettre à jour vers cette nouvelle version?", Buttons:=vbYesNo)
                    If Response = vbYes Then
                        'Traiter le cas si on met à jour
                        'Mise à jour en cours
                        MisAJourVersion(sfaoClose, sfaoPath, reperReelNvVersion, checkSfaoClose, lastVersion)
                    End If
                End If
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
    Private Sub MisAJourVersion(sfaoClose As String, reperVieilleVersion As String, reperReelNvVersion As String, checkSfaoClose As String, newVersion As String)
        Dim pathCsv As String = reperReelNvVersion & "\" & "listUpd-" & newVersion & ".csv"
        Dim listeFichiersUpdate As New List(Of String)
        If File.Exists(checkSfaoClose) Then
            'fermer la SFAO
            Application.Exit()
        End If
        MsgBox("Mise à jour en cours.....")
        'Parcours et lecture fichier listVersionUpd
        listeFichiersUpdate = lireCsv(pathCsv)

        'Traiter le remplacement des fichiers
        CopieNouvelleVersion(listeFichiersUpdate, reperVieilleVersion, reperReelNvVersion, newVersion)

        'supprimer le sfao.close
        File.Delete(Path.Combine(reperVieilleVersion, sfaoClose))
    End Sub

    'Méthode qui supprime les dossiers de l'ancienne version et copie la nouvelle
    Private Sub CopieNouvelleVersion(listFichCsv As List(Of String), ByVal reperVersion As String, ByVal reperReelNvVersion As String, ByVal newVersion As String)
        Dim chemReelCompletFichAChanger As String = ""
        Dim chemReelCompletDestination As String = ""
        Dim repertoireFichierAChanger As String = ""
        Dim fichierAchanger As String

        'Parcourir liste qui donne les fichiers à copier de la nouvelle à l'ancienne version
        For i = 0 To listFichCsv.Count - 1
            Dim valeurfichierAchanger As String = listFichCsv.Item(i).ToString()
            Dim tabFichAchanger() As String = valeurfichierAchanger.Split(CChar("\"))
            'Vérifier si fichierAchanger se présente en chemin de fichier ou fichier tout court
            If tabFichAchanger.Count > 1 Then
                'C'est un chemin de fichier
                fichierAchanger = tabFichAchanger((tabFichAchanger.Length) - 1)
                chemReelCompletFichAChanger = reperReelNvVersion & "\" & valeurfichierAchanger
            Else
                'C'est un fichier tout court
                fichierAchanger = valeurfichierAchanger
            End If

            Dim tabReel = New String() {}
            Dim listChemin As New List(Of String)
            tabReel = chemReelCompletFichAChanger.Split(CChar("\"))
            'Le fichier de ce nouveau répertoire
            'Le but c'est d'enlever le dernier élément qui est le fichier lui-même
            For j As Integer = 0 To tabReel.Count - 2
                listChemin.Add(tabReel(j))
            Next j

            'On supprime le dernier élément de tabreel
            'On reconstitue le répertoire du fichier à changer grâce à la liste 
            repertoireFichierAChanger = ""
            For Each str As String In listChemin
                repertoireFichierAChanger = repertoireFichierAChanger & str & "\"
            Next

            chemReelCompletDestination = reperVersion & "\" & valeurfichierAchanger

            'Vérifier si le fichier dans la liste est bien dans le répertoire de la nouvelle version
            If File.Exists(Path.Combine(repertoireFichierAChanger, fichierAchanger)) Then
                'Si oui, on copie ce fichier du répertoire nouvelle version vers celle de l'ancienne version,
                Try
                    My.Computer.FileSystem.CopyFile(
                chemReelCompletFichAChanger,
                chemReelCompletDestination,
                FileIO.UIOption.AllDialogs,
                FileIO.UICancelOption.DoNothing)
                Catch ex As Exception
                    Console.WriteLine(ex.Message)
                    MsgBox("Il y a un problème dans la copie de ce fichier :" & ex.Message)
                End Try
            Else
                'Sinon, message d'erreur
                MsgBox("Le fichier " & fichierAchanger & " n'existe pas dans le répertoire de mise à jour !")
            End If
        Next
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

    'Méthode qui permet de supprimer tous les fichiers d'un répertoire
    Private Sub DeleteDirectory(path As String)
        If Directory.Exists(path) Then
            'Delete all child Directories
            For Each dir As String In Directory.GetDirectories(path)
                My.Computer.FileSystem.DeleteDirectory(path, FileIO.DeleteDirectoryOption.DeleteAllContents)
            Next
        End If
    End Sub
End Class
