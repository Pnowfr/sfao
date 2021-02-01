Imports System.Data.SQLite
Imports System.Configuration
Imports System.IO

Public Class ConnexionLocaleSqlLite
    Private _connSSLite As SQLiteConnection

    Private repertoireBaseLocale As String
    Private repertoireFichiersTables As String
    Private cheminBaseSQLite As String
    Private cheminFichierBaseSQLite As String
    Private dossier As String
    Private site As String
    Private connexionString As String


    Public Sub New(Optional ByVal _repBaseLocale As String = "", Optional ByVal _site As String = "", Optional ByVal _dossier As String = "")

        Dim pos1, pos2, pos3 As Integer

        dossier = _dossier
        If dossier = "" Then
            'If ws Then
            dossier = SFAO.Site.GRP1.DOSSIER
            'Else
            'dossier = SFAO.Poste.Site.Dossier
            'End If
        End If

        site = _site
        If site = "" Then
            'If ws Then
            site = SFAO.Site.GRP1.FCYSHO.Replace(" ", "")  'on supprime les espaces au cas où
            'Else
            'site = SFAO.Poste.Site.DesCourt.Replace(" ", "")  'on supprime les espaces au cas où
            'End If
        End If

        repertoireBaseLocale = _repBaseLocale
        If repertoireBaseLocale = "" Then
            repertoireBaseLocale = SFAO.Param("REPBASELOCALE")
        End If

        Try
            'récupération des paramètres de connexion dans le fichier config
            connexionString = String.Format(ConfigurationManager.ConnectionStrings("ConnexionLocale").ConnectionString, Application.StartupPath.ToString, repertoireBaseLocale, site, dossier)
            pos1 = connexionString.IndexOf("=")
            pos2 = connexionString.LastIndexOf("\")
            cheminBaseSQLite = Mid(connexionString, pos1 + 2, pos2 - pos1 - 1)
            pos3 = connexionString.IndexOf(";")
            cheminFichierBaseSQLite = Mid(connexionString, pos1 + 2, pos3 - pos1 - 1)
            repertoireFichiersTables = Application.StartupPath.ToString & "\" & SFAO.Param("REPTBLCSV")
        Catch ex As Exception
            Trace("Erreur critique de lecture du connexionString <ConnexionLocale", FichierTrace.niveau.critique)
        End Try

    End Sub

    'Timeout par défaut de 15 secondes
    Public Function OuvrirConnexion(Optional ByVal TimeOut As Integer = 15) As Boolean

        Trace("Ouverture connexion à la base locale")

        'vérification si le fichier de base de donnée locale existe
        If My.Computer.FileSystem.FileExists(cheminFichierBaseSQLite) <> True Then
            Trace("Erreur ! Fichier de la base de donnée locale introuvable !", FichierTrace.niveau.alerte)
            OuvrirConnexion = False
            Exit Function
        End If

        If (Me._connSSLite Is Nothing) Or (Me._connSSLite IsNot Nothing AndAlso Me._connSSLite.State = ConnectionState.Closed) Then

            Dim connexionTimeOut As String = "Connection Timeout=" & TimeOut.ToString & ";"

            Try
                'tentative d'ouverture de la base locale avec la connexion string récupéré
                Me._connSSLite = New SQLiteConnection(connexionString & connexionTimeOut)
                Trace("Base locale ouverture connection avec timeout : " & Me._connSSLite.ConnectionTimeout.ToString)

                Me._connSSLite.Open()
                Trace("connexion base locale ouverte")
                Trace("connexionString : " & connexionString)
                OuvrirConnexion = True
            Catch ex As Exception
                Trace("Erreur connexion base locale avec SqlConnexionString : " & connexionString, FichierTrace.niveau.erreur)
                Trace(ex.Message, FichierTrace.niveau.erreur)
                OuvrirConnexion = False
            End Try
        Else
            Trace("Connexion base locale SQLite ouverte")
            OuvrirConnexion = True
        End If

    End Function

    Public Sub FermerConnexion()
        If Me._connSSLite IsNot Nothing Then
            Try
                Me._connSSLite.Close()
                Trace("Connexion base locale fermée")
            Catch ex As Exception
                Trace("Erreur fermeture base locale : ", FichierTrace.niveau.erreur)
                Trace(ex.Message, FichierTrace.niveau.erreur)
            End Try
        End If
    End Sub

    Public Function TestConnexionLocale() As Boolean
        FermerConnexion()
        OuvrirConnexion(10)
        If Me._connSSLite Is Nothing OrElse Not (Me._connSSLite.State = ConnectionState.Open) Then
            TestConnexionLocale = False
        Else
            TestConnexionLocale = True
        End If
    End Function

    Public Function ControleBaseLocale() As Boolean

        'Vérification si base locale existe et si non création de la base local
        If CreationBaseLocale() <> 0 Then
            'If ConnLoc.TemporaireCreationTablesLocales() = True Then       'temporaire
            If ConnLoc.CreationTablesLocales() = True Then                  'fonction qui crée les tables à partir des fichiers csv
                ControleBaseLocale = True
            Else
                ControleBaseLocale = False
            End If
        Else
            ControleBaseLocale = False
        End If
    End Function

    'Cette fonction est appelé si le fichier de base locale n'existe pas dans le sous dossier base donnée locale (\bdl)
    'création de la base locale et des tables nécessaires
    'retour : 0 = erreur critique / 1 = base déjà existante / 2 = base créé
    Private Function CreationBaseLocale() As Integer

        'Vérification si la base de données locale existe
        If Not My.Computer.FileSystem.FileExists(cheminFichierBaseSQLite) Then
            'Si non création du sous-dossier de la base locale
            My.Computer.FileSystem.CreateDirectory(cheminBaseSQLite)
            Try
                'Création de la base
                SQLiteConnection.CreateFile(cheminFichierBaseSQLite)
                Trace("[CreationBaseLocale] : Base de donnée locale créé : " & cheminFichierBaseSQLite)
                CreationBaseLocale = 2
            Catch ex As Exception
                Trace("[CreationBaseLocale] : Erreur critique de création de la base locale !", FichierTrace.niveau.critique)
                Trace(ex.Message, FichierTrace.niveau.erreur)
                CreationBaseLocale = 0
            End Try
        Else
            Trace("[CreationBaseLocale] : Base de donnée locale existante")
            CreationBaseLocale = 1 'base déjà existante
        End If

    End Function

    'Private Function TemporaireCreationTablesLocales() As Boolean
    '    Dim req As String = String.Empty
    '    SFAO.Sleep(1000) 'attente 1s
    '    OuvrirConnexion()
    '    If Me._connSSLite Is Nothing OrElse Not (Me._connSSLite.State = ConnectionState.Open) Then Return False
    '    req &= "CREATE TABLE IF NOT EXISTS CNXLOG ("
    '    req &= "IDCNX INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,"
    '    req &= "SITE VARCHAR(5) NOT NULL,"
    '    req &= "DOSSIER VARCHAR(15) NOT NULL,"
    '    req &= "POSTE VARCHAR(8) NOT NULL,"
    '    req &= "DTDEB VARCHAR(20) NOT NULL,"
    '    req &= "DEBUT INT NOT NULL,"
    '    req &= "DTFIN VARCHAR(50) NULL,"
    '    req &= "FIN INT NULL);"
    '    Try
    '        Using cmd As New SQLiteCommand(req, Me._connSSLite)
    '            cmd.ExecuteNonQuery()
    '        End Using
    '        Trace("Creation des tables locales : ok ")
    '        TemporaireCreationTablesLocales = True
    '    Catch ex As Exception
    '        Trace("Erreur critique lors de la creation des tables locales !", FichierTrace.niveau.critique)
    '        Trace(ex.Message, FichierTrace.niveau.critique)
    '        TemporaireCreationTablesLocales = False
    '    End Try
    '    Me._connSSLite.Close()    'on doit fermer la connexion après modification des tables 
    'End Function
    Private Function CreationTablesLocales() As Boolean
        Dim pathfich As New DirectoryInfo(repertoireFichiersTables)
        Dim ret As Integer
        Dim fichiercsv As String

        CreationTablesLocales = True
        Try
            If pathfich.GetFiles.Count > 0 Then
                For Each fi As System.IO.FileInfo In pathfich.GetFiles("*.csv")
                    fichiercsv = fi.FullName
                    ret = VerifTable(fichiercsv)
                    If ret = 2 Then
                        Trace("Lecture du fichier csv réussie et modification de la table réussie !")
                    ElseIf ret = 1 Then
                        Trace("Lecture du fichier csv réussie et modification de la table non nécessaire !")
                    Else
                        Trace("[CreationTablesLocales] Erreur de mise à jour de table locale !", FichierTrace.niveau.erreur)
                        CreationTablesLocales = False
                        Exit For
                    End If
                Next
                SupprTabBdd(pathfich)
            Else
                Trace("[CreationTablesLocales] Erreur aucun fichier de tables locales !", FichierTrace.niveau.avertissement)
                CreationTablesLocales = False
            End If
        Catch ex As Exception
            Trace("[CreationTablesLocales] Erreur d'accès au chemin des tables locales : " & repertoireFichiersTables & " !", FichierTrace.niveau.alerte)
            CreationTablesLocales = False
        End Try
    End Function
    'Vérification des tables pour création ou modification
    Private Function VerifTable(ByVal fichcsv As String) As Integer
        Dim i As Integer = 0
        Dim nbLignes As Integer = 0
        Dim ret As Integer
        Dim tmp_ligne() As String
        Dim req As String = ""
        Dim premiereTable As String = ""
        Dim tableTemp As String
        Dim tableTmp As String
        Dim nvTable As String = ""
        Dim siNouvTab As Boolean
        Dim siTabEgales As Boolean
        Dim listeLignes As List(Of String)
        Dim listeChamps As List(Of String)
        Dim difftable As List(Of String)
        Dim listTab As List(Of String)
        'BUG ICI!!!!!!!!!!!!!! 13/07/2020
        VerifTable = 0
        'Connexion à la base
        OuvrirConnexion()
        If Me._connSSLite Is Nothing OrElse Not (Me._connSSLite.State = ConnectionState.Open) Then Exit Function

        'Recupération du nombre de lignes avec des valeurs
        For Each ligne As String In File.ReadLines(fichcsv)
            If ligne.Trim().Length > 0 Then
                nbLignes += 1
            End If
        Next
        Trace("Recupération du nombre de lignes avec des valeurs !")

        Try
            'Récupération du nombre de lignes du fichier à lire
            listeLignes = New List(Of String)

            'Récupération de toutes les tables de la base de données
            listTab = RecupTablesBdd()

            'Parcours ligne par ligne du fichier
            Trace("Parcours ligne par ligne du fichier csv !")

            For Each ligne As String In File.ReadLines(fichcsv)
                If i < nbLignes Then
                    tmp_ligne = Split(ligne, ";")

                    If tmp_ligne(1) = "" Then  'Si la deuxième colonne est vide, la première colonne est le nom de la table
                        'tmp_ligne(0) est le nom de la table

                        'Vérification si la table existe dans la base sqlite
                        If listTab.Contains(tmp_ligne(0)) Then
                            premiereTable = tmp_ligne(0)
                            'Nouvelle table à faux
                            siNouvTab = False
                            'Début de requête de création d'une table temporaire
                            req &= "DROP TABLE IF EXISTS " & tmp_ligne(0) & "TMP; CREATE TABLE " & tmp_ligne(0) & "TMP ("
                            Trace("La table " & tmp_ligne(0) & " existe dans la base sqlite !")

                        Else
                            Trace("La table " & tmp_ligne(0) & " ne fait pas partie de la base de données ou n'existe pas encore !", FichierTrace.niveau.information)

                            'Nouvelle table à vrai
                            siNouvTab = True
                            'Création de la nouvelle table
                            nvTable = tmp_ligne(0)
                            req &= "CREATE TABLE " & nvTable & "("
                        End If

                        'La table temporaire dont le sdescriptifs viennent du fichier csv
                        tableTemp = premiereTable & "TMP"
                    Else
                        If tmp_ligne(4) <> "" Then
                            req &= tmp_ligne(0) & " " & tmp_ligne(1) & " " & tmp_ligne(2) & " " & tmp_ligne(3) & " " & tmp_ligne(4)
                        Else
                            req &= tmp_ligne(0) & " " & tmp_ligne(1) & " " & tmp_ligne(2) & " " & tmp_ligne(3)
                        End If
                        'Ajout de chaque ligne dans la liste
                        'listeChamps.Add(tmp_ligne(0) & " " & tmp_ligne(1) & " " & tmp_ligne(2) & " " & tmp_ligne(3) & " " & tmp_ligne(4))

                        If i = nbLignes - 1 Then
                            req &= ");"
                        Else
                            req &= ","
                        End If
                    End If
                End If
                i += 1
            Next ligne

            Trace(req)

            Dim tab As String() = Split(req, ",")

            'Appel à la fonction qui gère la requête avec "PK" et "UNIQUE" et qui retourne la liste avec les champs modifiés de la requête
            If siNouvTab = True Then
                listeChamps = GestionPKUnique(tab, nvTable, siNouvTab)
            Else
                listeChamps = GestionPKUnique(tab, premiereTable, siNouvTab)
            End If

            Dim champPk As String = ""
            Dim champUnique As String = ""
            'Supprimer si un champ vide existe
            For i = 0 To listeChamps.Count - 1
                If listeChamps(i) Is Nothing Then
                    listeChamps.Remove(listeChamps(i))
                End If
            Next
            Trace("Suppression de listeChamps d'un champ vide au cas où !")

            'Récuperer les champs avec PK et Un
            For i = 0 To listeChamps.Count - 1
                If listeChamps(i).Contains("PRIMARY") Then
                    'Le champ avec PK
                    champPk = listeChamps(i)
                End If
                If listeChamps(i).Contains("UNIQUE") Then
                    'Le champ avec UNIQUE
                    champUnique = listeChamps(i)
                End If
            Next
            Trace("Récupération des champs champPk avec PK et champUnique UN !")

            'On supprime les deux champs de la liste
            listeChamps.Remove(champPk)
            listeChamps.Remove(champUnique)
            Trace("Suppression des deux champs champPk,champUnique de la liste listeChamps temporairement !")

            'On récupère le début de la requête 
            Dim debutrequ As String
            If siNouvTab = False Then
                debutrequ = "DROP TABLE IF EXISTS " & premiereTable & "TMP;" & " CREATE TABLE " & premiereTable & "TMP ("
            Else
                debutrequ = "CREATE TABLE " & nvTable & "("
            End If
            Trace("Récupération du début de la requête debutrequ !")

            'On transforme la liste en chaines séparéées par "," pour reformer la requête
            req = String.Join(",", listeChamps.ToArray())
            Trace("Transformation de la liste listeChamps en chaines séparéées par ',' pour reformer la requête !")

            'On concatène le debutreq + req + les deux champs enlevés précédemment
            'Et la valeur de la requête dépend de l'existence de "champUn"
            If champUnique <> "" Then
                req = debutrequ + " " + req + ", " + champPk + ", " + champUnique + ")"
            Else
                req = debutrequ + " " + req + ", " + champPk + ")"
            End If

            Trace("On concatène le debutreq + req + les deux champs enlevés précédemment !")
            Trace(req)

            'On exécute la requête
            Try
                Using cmd As New SQLiteCommand(req, Me._connSSLite)
                    cmd.ExecuteNonQuery()
                End Using
                Trace("Creation de la table (nouvelle table ou table temporaire) réussie")
                ret = 2
            Catch ex As Exception
                Trace("[VerifTable] Erreur, requête non exécutée : " & req, FichierTrace.niveau.erreur)
                Trace(ex.Message, FichierTrace.niveau.erreur)
                ret = 0
            End Try

            tableTmp = premiereTable & "TMP"

            'Appel à la Fonction qui permet de comparer deux tables par leurs infos
            'Modif du 17/07/2020 Fidi
            If siNouvTab = False Then
                siTabEgales = CompareDeuxTables(premiereTable, tableTmp)
            End If

            'Si ce n'est pas une nouvelle table et qu'elle est différente de la table TMP
            If siNouvTab = False AndAlso siTabEgales = False Then

                'On compare les deux tables
                difftable = TableComparateur(premiereTable, listeChamps)

                'On peut remettre les deux champs avec PK et unique dans la liste, c'est utile pour la fonction Transact
                listeChamps.Add(champPk)
                If champUnique <> "" Then
                    listeChamps.Add(champUnique)
                End If
                Trace("Remise des deux champs avec PK et unique dans la liste, c'est utile pour la fonction Transact ")

                'Appel à la fonction Transact
                Trace("Appel à la fonction Transact pour la transaction, finalisation de la requête de mise à jour ")
                ret = MiseAJourTable(premiereTable, listeChamps, difftable)

            ElseIf siTabEgales Then
                ret = 1
            End If
        Catch ex As Exception
            Trace("[VerifTable] Erreur lors de la lecture ! " & ex.Message, FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)
            ret = 0
        End Try
        Return ret
    End Function
    'Fonction qui retourne la liste des tables de la base de données
    Private Function RecupTablesBdd() As List(Of String)
        Dim listTables As List(Of String)
        Dim req As String
        Dim reader As SQLiteDataReader
        listTables = New List(Of String)
        RecupTablesBdd = listTables

        'Requête pour avoir toutes les table de la base
        req = "SELECT distinct tbl_name FROM sqlite_master "
        Try
            Using cmd As New SQLiteCommand(req, Me._connSSLite)
                reader = cmd.ExecuteReader()
                While reader.Read()
                    Dim res As String = reader.GetString(0)
                    listTables.Add(res)
                End While
                Trace("Rajout de la liste des tables de la base dans la listTables !")
            End Using
            Trace("Requête pour avoir toutes les tables de la base exécutée avec réussite !")
        Catch ex As Exception
            Trace("[RecupTablesBdd] Erreur, requête pour avoir toutes les tables de la base non exécutée ", FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)
        End Try
        RecupTablesBdd = listTables
    End Function
    'Fonction qui gère la requête avec "PK" et "UNIQUE"
    Private Function GestionPKUnique(tab() As String, premtable As String, siNvTab As Boolean) As List(Of String)
        Dim j As Integer
        Dim valuePk As String = "PRIMARY KEY"
        Dim valueUn As String = "UNIQUE"
        Dim listTabReq As List(Of String) = New List(Of String)
        Dim listrequete As List(Of String)
        Dim listPkUnComplete As List(Of String) = New List(Of String)
        Dim ChampAvecPkUn As String

        If siNvTab Then
            'Si c'est une nouvelle table
            tab(0) = tab(0).Replace("CREATE TABLE " & premtable & "(", "")
            Trace("Requete pour la création de la nouvelle table " & premtable & " !")
        Else
            'Si c'est une table existante
            tab(0) = tab(0).Replace("DROP TABLE IF EXISTS " & premtable & "TMP;" & " CREATE TABLE " & premtable & "TMP (", "")
            Trace("Requete pour créer une tabl temporaire " & premtable & "TMP !")
        End If

        'Parcours du tableau pour voir si valuePK ou valueUn apparait dans une requête
        Trace("Parcours du tableau pour voir si PRIMARY KEY ou UNIQUE apparait dans une requête ")
        For j = 0 To tab.Length - 1
            'Si le champ contient "PRIMARY KEY"
            If tab(j).Contains(valuePk) Then
                'On l'enlève du champ
                tab(j) = tab(j).Replace(valuePk, "")
                'On ajoute dans la liste le champ sans l'expression "PRIMARY KEY"
                listPkUnComplete.Add(tab(j))
                'On récupère le premier mot du String
                ChampAvecPkUn = tab(j).Substring(0, tab(j).IndexOf(" "))
                'On met le(s) resultat(s) dans une liste
                listTabReq.Add("pk")
                listTabReq.Add(ChampAvecPkUn)
            End If
            'Si le champ contient "UNIQUE"
            If tab(j).Contains(valueUn) Then
                'On l'enlève du champ
                tab(j) = tab(j).Replace(valueUn, "")
                'On ajoute dans la liste le champ sans l'expression "UNIQUE"
                listPkUnComplete.Add(tab(j))
                'On récupère le premier mot du String
                ChampAvecPkUn = tab(j).Substring(0, tab(j).IndexOf(" "))
                'On met le(s) resultat(s) dans une liste
                listTabReq.Add("un")
                listTabReq.Add(ChampAvecPkUn)
            End If
        Next
        'Appel à la fonction qui insère le champ dans le tableau de requête
        listrequete = InsertionRequete(listTabReq, tab)

        Return listrequete
    End Function
    'Fonction qui insère le champ dans le tableau de requête
    Private Function InsertionRequete(listTabReq As List(Of String), tab As String()) As List(Of String)
        Dim dernrequetePK As String
        Dim dernrequeteUn As String = ""
        Dim i As Integer
        Dim listChamps As List(Of String) = New List(Of String)()

        'Si valuekey est "PRIMARY KEY"
        dernrequetePK = CreaChamp("PRIMARY KEY", "pk", listTabReq)

        'Si valuekey est "UNIQUE"
        If listTabReq.Contains("un") Then
            dernrequeteUn = CreaChamp("UNIQUE", "un", listTabReq)
        End If
        'On ajoute les requêtes dans le tableau
        'On ajoute les contenus du tableau dans une liste
        For i = 0 To tab.Count - 1
            listChamps.Add(tab(i))
        Next

        'On ajoute les champs PK et Unique dans la liste
        'On enlève d'abord les caractères ; et ) du champ précédent
        listChamps(listChamps.Count - 1) = listChamps(listChamps.Count - 1).Replace(");", "")
        listChamps.Add(dernrequetePK)
        listChamps.Add(dernrequeteUn)
        Trace("Ajout des champs PK et Unique dans listChamps")

        Return listChamps
    End Function

    'Fonction qui retourne le champ concerné
    Private Function CreaChamp(valueKey As String, abreg As String, listTabReq As List(Of String)) As String
        Dim dernrequete As String
        Dim k As Integer
        Dim j As Integer
        Dim listePkUn As List(Of String) = New List(Of String)()
        dernrequete = valueKey
        dernrequete &= " ( "
        For j = 0 To listTabReq.Count - 1
            If listTabReq(j) = abreg Then
                listePkUn.Add(listTabReq(j + 1))
            End If
        Next
        For k = 0 To listePkUn.Count - 1
            If k <> listePkUn.Count - 1 Then
                dernrequete &= listePkUn(k)
                dernrequete &= ","
            Else
                dernrequete &= listePkUn(k)
                dernrequete &= " )"
            End If
        Next
        Trace("Création du champ en format PRIMARY KEY(__,__,...), UNIQUE(_,_,...)")
        Return dernrequete
    End Function
    'Fonction qui permet de comparer deux tables
    Private Function TableComparateur(premiereTable As String, listReqDeuxTab As List(Of String)) As List(Of String)
        Dim reqPremiereTable As String
        Dim i As Integer
        Dim j As Integer
        Dim premierMot As String
        Dim listReqPremtab As List(Of String)
        Dim listReqTabPremMot As List(Of String) = New List(Of String)
        Dim resdiff As List(Of String) = New List(Of String)

        'Requetes pour connaître la structure des tables
        reqPremiereTable = "SELECT sql "
        reqPremiereTable &= "FROM sqlite_master"
        reqPremiereTable &= " WHERE name = '" & premiereTable & "';"

        'Exécution de la requête pour la première table
        listReqPremtab = Me.ExecutionRequetePremtab(reqPremiereTable, premiereTable)
        Trace("Récupération de la liste des champs de la première table dans listReqPremtab ")

        'On ne garde que le premier mot de chaque champ de la liste de la deuxième table et le mettre dans une nouvelle liste
        For i = 0 To listReqDeuxTab.Count - 1
            premierMot = listReqDeuxTab(i).Substring(0, listReqDeuxTab(i).IndexOf(" "))
            listReqTabPremMot.Add(premierMot)
        Next
        Trace("Récupération de la liste des champs de la deuxième table dans listReqTabPremMot ")

        'Comparaison des deux listes et trouver les champs communs
        For j = 0 To listReqDeuxTab.Count - 1
            If listReqPremtab.Contains(listReqTabPremMot(j)) Then 'Si trouvé
                'Rajouter dans la liste
                resdiff.Add(listReqTabPremMot(j))
            End If
        Next
        Trace("Comparaison des deux listes et trouver les champs communs ")
        Return resdiff
    End Function

    Private Function CompareDeuxTables(table1 As String, table2 As String) As Boolean
        Dim res As Boolean
        Dim reqTab1 As String
        Dim reqTab2 As String
        Dim listTab1 As List(Of String)
        Dim listTab2 As List(Of String)

        listTab1 = New List(Of String)
        listTab2 = New List(Of String)

        reqTab1 = "PRAGMA table_info ([" & table1 & "])"
        reqTab2 = "PRAGMA table_info ([" & table2 & "])"

        res = False

        'Recupération des champs de la table1 et on ajoute dans la liste listTab1
        Try
            Using cmdTab1 As New SQLiteCommand(reqTab1, Me._connSSLite)
                Dim drtab1 As SQLiteDataReader = cmdTab1.ExecuteReader
                While drtab1.Read
                    listTab1.Add(drtab1("type").ToString & "-" & drtab1("cid").ToString & "-" & drtab1("name").ToString & "-" & drtab1("notnull").ToString & "-" & drtab1("pk").ToString)
                End While
            End Using
            Trace("Récupération de chaque champ de la table1 !")
        Catch ex As Exception
            Trace("[CompareDeuxTables] Erreur lors du traitement de la requête reqTab1", FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)
        End Try
        'Recupération des champs de la table1 et on ajoute dans la liste listTab2
        Try
            Using cmdTab2 As New SQLiteCommand(reqTab2, Me._connSSLite)
                Dim drtab2 As SQLiteDataReader = cmdTab2.ExecuteReader
                While drtab2.Read
                    listTab2.Add(drtab2("type").ToString & "-" & drtab2("cid").ToString & "-" & drtab2("name").ToString & "-" & drtab2("notnull").ToString & "-" & drtab2("pk").ToString)
                End While
            End Using
            Trace("Récupération de chaque champ de la table2 !")
        Catch ex As Exception
            Trace("[CompareDeuxTables] Erreur lors du traitement de la requête reqTab2", FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)

        End Try
        'On procède à la comparaison
        'Si la taille des deux listes est différente
        Trace("On procède à la comparaison des champs des deux tables !")
        If listTab1.Count() <> listTab2.Count() Then
            res = False
        Else 'Si la taille est égale, on compare les valeurs
            If listTab1(0).Equals(listTab2(0)) AndAlso listTab1(1).Equals(listTab2(1)) AndAlso listTab1(2).Equals(listTab2(2)) AndAlso listTab1(3).Equals(listTab2(3)) AndAlso listTab1(4).Equals(listTab2(4)) Then
                res = True
            Else
                res = False
            End If
        End If
        Return res
    End Function

    'Méthode pour avoir les requêtes sur les tables
    Private Function ExecutionRequetePremtab(requete As String, premtab As String) As List(Of String)
        Dim listRequete As List(Of String) = New List(Of String)
        Dim resul As String
        Dim tabrequ As String()
        Dim i As Integer
        Dim premiermot As String
        Dim reader As SQLiteDataReader

        Dim cut_at As String = "PRIMARY"

        Try
            Using cmd As New SQLiteCommand(requete, Me._connSSLite)
                reader = cmd.ExecuteReader()
                While reader.Read()
                    resul = reader.GetString(0)

                    'txtBox.Text = resul
                    Trace("Récupération de la requête ! ")

                    Dim x As Integer = InStr(resul, cut_at)
                    'Enlever tous les caractères à droite du mot PRIMARY
                    ' enlever la dernière virgule de la requête
                    Dim requetetab1 As String = resul.Substring(0, x - 2)
                    Trace("Suppression tous les caractères à droite du mot PRIMARY ")

                    'Si la requête se termine par ","
                    If requetetab1.EndsWith(",") Then
                        requetetab1 = requetetab1.Remove(requetetab1.LastIndexOf(","))
                    End If
                    Dim string_after As String = resul.Substring(x + cut_at.Length)

                    'On met le résultat dans un tableau
                    tabrequ = requetetab1.Split(CType(",", Char()))
                    Trace("Découpage de la requête en champs séparés par un virgule et les mettre dans un tableau tabrequ ")

                    'On enlève ces caractères du premier champ du tableau
                    tabrequ(0) = tabrequ(0).Replace("CREATE TABLE ", "")
                    tabrequ(0) = tabrequ(0).Replace(premtab, "")
                    tabrequ(0) = tabrequ(0).Replace(" (", "")
                    tabrequ(0) = tabrequ(0).Replace("  ", " ")

                    'Puis du tableau vers une liste
                    For i = 0 To tabrequ.Count - 1
                        tabrequ(i) = tabrequ(i).Replace("  ", " ")
                        premiermot = tabrequ(i).Substring(0, tabrequ(i).IndexOf(" "))
                        listRequete.Add(premiermot)
                    Next
                    Trace("Transfert du tableau dans listRequete des premiers mots de chaque champ ")
                End While
            End Using
            Trace("Requête exécutée avec réussite")
        Catch ex As Exception
            Trace("[CompareDeuxTables] Erreur, requête non exécutée ", FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)
        End Try
        Return listRequete
    End Function
    'Fonction qui gère la transaction qui termine la mise à jour de la table concernée selon la lecture du fichier csv
    Private Function MiseAJourTable(premTable As String, listeChamps As List(Of String), difftab As List(Of String)) As Integer
        Dim ret As Integer
        Dim i As Integer
        Dim resulrequete As Integer
        Dim req As String
        Dim listRequPremTab As List(Of String) = New List(Of String)
        Dim listNvChamps As List(Of String) = New List(Of String)
        Dim listVar As List(Of String) = New List(Of String)

        'Enlever les espaces de listchamps
        For i = 0 To listeChamps.Count - 1
            listeChamps(i) = Replace(listeChamps(i), "  ", " ")
        Next

        'Transaction
        req = "PRAGMA foreign_keys = off;"

        req &= "BEGIN TRANSACTION;"
        req &= " DROP TABLE IF EXISTS " & premTable & "OLD;"

        'Changer la premTable et la renommer
        req &= "ALTER TABLE " & premTable & " RENAME TO " & premTable & "OLD;"

        'Re-Créer la table avec son nom d'origine
        req &= "CREATE TABLE " & premTable & " ("

        'Parcours de la liste avec les champs et les mettre dans la requête (contenus de champs MODIFIES, issus du fichier csv)
        Trace("Parcours de la liste listeChamps avec les champs et les mettre dans la requête (contenus de champs MODIFIES, issus du fichier csv)")
        For i = 0 To listeChamps.Count - 1
            req &= listeChamps(i)
            If i <> listeChamps.Count - 1 Then
                req &= ","
            Else
                req &= ");"
            End If
        Next
        Trace("Requête pour la création de la table modifiée ")

        'Ré-insertion des données de la vieille table à la nouvelle
        req &= "INSERT INTO " & premTable & " ("
        For i = 0 To difftab.Count - 1
            req &= difftab(i)
            If i <> difftab.Count - 1 Then
                req &= ","
            Else
                req &= ")"
            End If
        Next

        req &= " SELECT "
        For i = 0 To difftab.Count - 1
            req &= difftab(i)
            If i <> difftab.Count - 1 Then
                req &= ","
            End If
        Next
        req &= " From " & premTable & "OLD ;"

        req &= "COMMIT;"

        req &= "PRAGMA foreign_keys =On;"

        Trace("Insertion dans la nouvelle table " & premTable & " des données qui viennent de " & premTable & "OLD")

        'Exécution de la requète
        Try
            Using cmd As New SQLiteCommand(req, Me._connSSLite)
                resulrequete = cmd.ExecuteNonQuery()
            End Using
            Trace("Modification de la table " & premTable & " : ok ")
            ret = 2
        Catch ex As Exception
            Trace("[MiseAJourTable] Erreur lors de la mise à jour de " & premTable, FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)
            ret = 0
        End Try

        Return ret
    End Function
    'Méthode qui supprime la ou les table(s) qui sont sur la base mais pas dans le répertoire de Resources ainsi
    'que celles qui se terminent par TMP et OLD
    Public Sub SupprTabBdd(pathFich As DirectoryInfo)
        Dim i As Integer
        Dim reqSupp As String
        Dim nomTab As String
        Dim listTabSqlite As List(Of String)
        Dim listTablesCsv As List(Of String) = New List(Of String)

        'Récupération de la liste des tables de la base de données
        listTabSqlite = Me.RecupTablesBdd()
        Trace("Récupération de listTabSqlite, la liste des tables de la base de données ")

        'Supprimer les tables inutiles
        Trace("Suppression des tables OLD et TMP")
        SupprTablesTmpOld(listTabSqlite)

        'Récupération de la liste des tables du répertoire Resources
        Trace("Récupération de la liste des tables du répertoire Resources")
        For Each fi As System.IO.FileInfo In pathFich.GetFiles("*.csv")
            'On extrait le répertoire contenant le nom du fichier(qui est le nom de la table)
            Dim fichiercsv As String = fi.FullName
            'On découpe en tableau avec les champs séparés par \
            Dim fichtab As String() = Split(fichiercsv, "\")
            'On parcourt le tableau

            'Le dernier champ est le nom du fichier
            nomTab = fichtab(fichtab.Length - 1)
            nomTab = nomTab.Remove(nomTab.LastIndexOf(".csv"))
            'On l'ajoute dans la liste
            listTablesCsv.Add(nomTab)
        Next

        'Tester si une table de la base n'est pas dans la liste des tables dans Resources
        'On parcourt la liste des tables de la base
        Trace("Test si une table de la base n'est pas dans la liste des tables dans Resources et la supprimer")
        For i = 0 To listTabSqlite.Count - 1
            If Not listTablesCsv.Contains(listTabSqlite(i)) Then 'Si oui, on supprime
                reqSupp = "DROP TABLE " & listTabSqlite(i)
                Try
                    Using cmddrop As New SQLiteCommand(reqSupp, Me._connSSLite)
                        cmddrop.ExecuteNonQuery()
                    End Using
                    Trace("Suppression des tables inutiles réussie")
                Catch ex As Exception
                    Trace("[SupprTabBdd] Erreur lors de la suppression des tables inutiles", FichierTrace.niveau.erreur)
                    Trace(ex.Message, FichierTrace.niveau.erreur)
                End Try
            End If
        Next
        'Me._connSSLite.Close() 'On ferme la connexion
    End Sub
    'Méthode qui supprime les tables dont le nom se termine par "OLD" ou "TMP"
    Private Sub SupprTablesTmpOld(listTables As List(Of String))
        Dim i As Integer
        Dim reqDrop As String
        Dim listTabCsv As List(Of String) = New List(Of String)
        Dim listTabRem As List(Of String) = New List(Of String)

        'Parcours de la liste des tables
        For i = 0 To listTables.Count - 1
            'Tester si une table se termine par "OLD" ou "TMP" et la supprimer
            Try
                If listTables(i).EndsWith("TMP") Or listTables(i).EndsWith("OLD") Then
                    reqDrop = "DROP TABLE " & listTables(i)
                    Using cmddrop As New SQLiteCommand(reqDrop, Me._connSSLite)
                        cmddrop.ExecuteNonQuery()
                    End Using
                    'On l'ajoute dans la liste des tables à supprimer
                    listTabRem.Add(listTables(i))
                End If
            Catch ex As Exception
                Trace("[SupprTablesTmpOld] Erreur lors de la suppression des tables inutiles", FichierTrace.niveau.erreur)
                Trace(ex.Message)
            End Try
        Next

        'Supprimer les tables de la liste
        For i = 0 To listTabRem.Count - 1
            listTables.Remove(listTabRem(i))
        Next
    End Sub
    Public Function ConnexionPoste(ByVal _site As String, ByVal _dos As String, ByVal _poste As String) As Integer
        Dim req As String
        Dim lastrowid As Integer
        Dim cmd As SQLiteCommand
        Dim dtdeb As String
        Dim deb As Int64

        OuvrirConnexion()
        If Me._connSSLite Is Nothing OrElse Not (Me._connSSLite.State = ConnectionState.Open) Then Return 0

        req = "INSERT INTO CNXLOG (SITE,DOSSIER,POSTE,DTDEB,DEBUT) VALUES (@site,@dos,@poste,@dtdeb,@debut)"

        Try
            cmd = New SQLiteCommand(req, Me._connSSLite)
            dtdeb = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            deb = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds) 'timestamp
            With cmd.Parameters
                .Add(New SQLiteParameter("@site", _site))
                .Add(New SQLiteParameter("@dos", _dos))
                .Add(New SQLiteParameter("@poste", _poste))
                .Add(New SQLiteParameter("@dtdeb", dtdeb))
                .Add(New SQLiteParameter("@debut", deb))
            End With

            cmd.ExecuteNonQuery()
            cmd.CommandText = "SELECT last_insert_rowid()"
            lastrowid = CType(cmd.ExecuteScalar(), Integer)

            Trace("[ConnexionPoste] enregistrement connexion n° " & lastrowid.ToString & " ok")
            ConnexionPoste = lastrowid
        Catch ex As Exception
            Trace("[ConnexionPoste] erreur critique lors de l'enregistrement de la connexion ! ", FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)
            ConnexionPoste = 0
        End Try

    End Function
    Public Sub DeConnexionPoste(ByVal _site As String, ByVal _dos As String, ByVal _poste As String, ByVal _idcnx As Integer)
        Dim req As String
        Dim cmd As SQLiteCommand
        Dim dtfin As String
        Dim fin As Int64

        OuvrirConnexion()
        If Me._connSSLite Is Nothing OrElse Not (Me._connSSLite.State = ConnectionState.Open) Then Exit Sub

        req = "UPDATE CNXLOG SET DTFIN = @dtfin, FIN = @fin WHERE IDCNX=@idcnx and POSTE=@poste and SITE=@site and DOSSIER = @dos"

        Try
            cmd = New SQLiteCommand(req, Me._connSSLite)
            dtfin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            fin = CLng(DateTime.UtcNow.Subtract(New DateTime(1970, 1, 1)).TotalMilliseconds) 'timestamp
            With cmd.Parameters
                .Add(New SQLiteParameter("@dtfin", dtfin))
                .Add(New SQLiteParameter("@fin", fin))
                .Add(New SQLiteParameter("@idcnx", _idcnx))
                .Add(New SQLiteParameter("@poste", _poste))
                .Add(New SQLiteParameter("@site", _site))
                .Add(New SQLiteParameter("@dos", _dos))
            End With

            cmd.ExecuteNonQuery()

            'au cas où fin des connexions ouvertes non fermées (suite un plantage par exemple)
            cmd.CommandText = "UPDATE CNXLOG SET DTFIN = 'unknow', FIN = 0 WHERE FIN IS NULL"
            cmd.ExecuteNonQuery()
            Trace("Enregistrement déconnexion : ok ")
        Catch ex As Exception
            Trace("[DeConnexionPoste] Erreur critique lors de l'enregistrement de la déconnexion !", FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)
        End Try
    End Sub
End Class
