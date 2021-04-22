'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321PNO] Ajout test si fichier trace existant
'
'------------------------------------------------------------------------------------------------------------------------
Imports System.IO

Public Class FichierTrace
    Private traceactive As Boolean
    Private tracetype As typeTrace
    Private repertoire As String
    Private fichier As String
    Private fichierNPad As String
    Private niveauTrace As niveau
    'Private cheminFichTrace As String
    Private file As IO.StreamWriter
    Private fileNpad As IO.StreamWriter
    Public Enum typeTrace
        connexion           '0
        sfao                '1
    End Enum
    Public Enum niveau
        information             '0 info dans le fichier trace
        avertissement           '1 avertissement dans le fichier trace
        erreur                  '2 erreur dans le fichier trace
        alerte                  '3 erreur dans le fichier trace + message utilisateur
        critique                '4 erreur dans le fichier trace + message utilisateur + on quitte l'appli
        toujours                '5 message toujours écrit dans le fichier trace quelque soit le niveau de trace paramètré
    End Enum
    Sub New(ByVal typedetrace As typeTrace)
        Dim rep As String = String.Empty
        tracetype = typedetrace 'CType(typedetrace, typeTrace)

        If tracetype = typeTrace.connexion Then
            'on verifie si le paramètre la trace connexion existe et est actif (par défaut on le crée actif)
            AddPar("TRACECONNEXION", "VRAI")
            If SFAO.Param("TRACECONNEXION") = "VRAI" Then
                traceactive = True
            Else
                traceactive = False
            End If

            'on verifie si la paramètre du dossier des traces connexion existe et si non on le crée (par defaut on utilise le sous-dossier "Traces")
            AddPar("REPTRACECONNEXION", "Traces")
            rep = SFAO.Param("REPTRACECONNEXION")
        ElseIf tracetype = typeTrace.sfao Then
            'on verifie si le paramètre de la trace sfao existe et est actif (par défaut on le crée actif)
            AddPar("TRACESFAO", "VRAI")
            If SFAO.Param("TRACESFAO") = "VRAI" Then
                traceactive = True
            Else
                traceactive = False
            End If

            'on verifie si la paramètre du dossier des traces sfao existe et si non on le crée (par defaut on utilise le sous-dossier "Traces")
            AddPar("REPTRACESFAO", "Traces")
            rep = SFAO.Param("REPTRACESFAO")
        End If

        If traceactive Then
            If Len(rep) >= 2 AndAlso (Strings.Left(rep, 2) = "\\" Or rep.Substring(1, 2) = ":\") Then
                'si c'est un chemin réseau : pas de création du sous-dossier
                repertoire = rep
            Else
                'si c'est un sous-dossier, il faut s'assurer qu'il existe et si non le créer
                Try
                    repertoire = Application.StartupPath.ToString & "\" & rep
                    My.Computer.FileSystem.CreateDirectory(repertoire)
                Catch ex As Exception
                    'en cas d'erreur on désactive la trace
                    Console.WriteLine("Erreur ouverture fichier trace !")
                    MsgBox("Erreur d'ouverture du fichier trace : " & repertoire & " !", MsgBoxStyle.Exclamation) 'message erreur !
                    repertoire = ""
                    traceactive = False
                End Try
            End If
        Else
            repertoire = ""
        End If

        'on s'assure que le paramètre niveau trace existe
        AddPar("NIVEAUTRACE", "0")
        niveauTrace = CType(SFAO.Param("NIVEAUTRACE"), niveau) 'donc 0 par défaut si paramètre non renseigné ok
    End Sub

    'Ouverture du fichier trace 
    Public Sub OuvreTrace()
        Dim typtr As String
        Dim typtrace As String
        Dim numfil As Integer = 0               '270321PNO.n
        Dim addfil As String = String.Empty     '270321PNO.n
        If traceactive Then

            If tracetype = typeTrace.connexion Then
                typtrace = "connexion"
                typtr = "Cnx_"
            ElseIf tracetype = typeTrace.sfao AndAlso SFAO.Poste.GRP1.WST <> "" Then
                typtrace = "Sfao"
                typtr = "Sfao_" & SFAO.Poste.GRP1.WST & "_"
            Else
                typtrace = "Sfao"
                typtr = "Sfao_"
            End If

            '270321PNO.sn
            fichier = "Trace_" & typtr & DateTime.Now.ToString("yyyyMMdd_HH_mm_ss")
            Do While My.Computer.FileSystem.FileExists(repertoire & "\" & fichier & addfil & ".tra")
                numfil += 1
                addfil = "_" & numfil.ToString
            Loop
            fichier += addfil & ".tra"
            '270321PNO.en

            Try
                file = My.Computer.FileSystem.OpenTextFileWriter(repertoire & "\" & fichier, True)
            Catch ex As Exception
                Console.WriteLine("Erreur ouverture fichier trace !")
                MsgBox("Erreur d'ouverture du fichier trace : " & repertoire & "\" & fichier & " !", MsgBoxStyle.Exclamation) 'message erreur !
                repertoire = ""
                traceactive = False
            End Try

            'Ecriture de la 1ère ligne de trace  
            Trace("Ouverture de la trace " & typtrace, FichierTrace.niveau.toujours)
            Trace(repertoire & "\" & fichier, FichierTrace.niveau.toujours)
        End If
    End Sub

    'niveau 0=information / 1= avertissement +msg info / 2=erreur / 3 alerte = erreur + msg warning / 4 erreur critique / 5=information toujours entregistré
    Public Sub Trace(message As String, niveau As FichierTrace.niveau)

        If traceactive Then
            Try
                'on écrit une nouvelle ligne de trace en fonction du niveau paramètré ou si on est en test
                If niveau >= niveauTrace Or SfaoTest Then
                    'Dans le fichier on supprime les retour chariot et les tab
                    file.WriteLine(CInt(niveau).ToString & ";" & Date.Now.ToString("yyyy/MM/dd HH:mm:ss") & ";" & message.Replace(vbCr, "").Replace(vbLf, "").Replace(vbTab, ""))
                End If
            Catch ex As Exception
                Console.WriteLine("Une erreur est survenue au cours de l'écriture de la trace !")
                Console.WriteLine(ex.Message)
            End Try
        End If

        'dans tous les cas on écrit dans la console pour le débuggage
        Console.WriteLine(CInt(niveau).ToString & " " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss") & " " & message)

        If niveau = 1 Then
            MsgBox(message, MsgBoxStyle.Information) 'message info
        ElseIf niveau = 2 Then
            FenSfao.Etat(message, 1, 10000, Color.Red) 'on affiche le message d'erreur dans la barre d'état
        ElseIf niveau = 3 Then
            MsgBox(message, MsgBoxStyle.Exclamation) 'message erreur !
        ElseIf niveau = 4 Then 'erreur critique 
            MsgBox(message, MsgBoxStyle.Critical) 'message utilisateur !
            FermeSFAO()                    'on quitte l'application ! 
            End
        End If

    End Sub

    'Méthode pour fermer l'écriture dans le fichier ajout 10/06
    'A appeler lorsque le bouton de déconnexion de la SFAO par exemple
    Public Sub FermeTrace()
        If traceactive Then
            Trace("Fermeture de la trace", FichierTrace.niveau.toujours)
            file.Close()
        End If
    End Sub

    'Méthode pour afficher la trace à l'instant T
    Public Sub AfficheTrace()
        Dim p As New Process()
        Dim FichierEnCours As String = ""

        If traceactive Then
            'Récupère une liste de fichiers
            Dim MesFichiers As String() = System.IO.Directory.GetFiles(repertoire, "*")
            'Liste triée contenant les noms des fichiers
            Dim mLst As New List(Of String)

            'Si fichiers trouvés
            If MesFichiers IsNot Nothing Then
                'Tri les fichiers par date du plus ancien au plus récent
                Dim i As Integer
                For i = 0 To MesFichiers.Length - 1
                    Dim CurrentDate As Date = New IO.FileInfo(MesFichiers(i)).LastWriteTime
                    mLst.Add(MesFichiers(i))
                Next
                FichierEnCours = mLst(MesFichiers.Length - 1).ToString
            End If

            Try
                p.StartInfo = New ProcessStartInfo("notepad.exe", FichierEnCours)
                p.Start()
            Catch ex As Exception
                Console.WriteLine("[AfficheTrace] : Exception !")
                Console.WriteLine(ex.Message)
            End Try
        End If
    End Sub

End Class
