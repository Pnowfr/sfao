'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[270321DEV] Test2
'------------------------------------------------------------------------------------------------------------------------

Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

'Classe qui gère les web services X3
Public Class X3WebServ
    Private WithEvents X3WSC As X3WSClass                   'classe d'appel des web services X3 (ne pas instentier imédiatement)
    Public X3WSCA As X3WebServAsync
    Public Sub New(ByVal dossier As String, ByVal version As String)
        X3WSC = New X3WSClass(dossier, version)
        X3WSCA = New X3WebServAsync
    End Sub

    'Methode qui ferme le web service
    Public Sub Close()
        X3WSC.Close()
    End Sub

    'Web service permettant la récupération de la date+heure (pour contrôler que les ws fonctionnent)
    Public Function WSDTSFAO() As String
        'Attention : ne pas enregistrer de trace si tout est ok dans cette fonction : est apelée dans un timer fréquent ! 
        Dim params As String
        Dim json As JObject
        Dim retxml As String = String.Empty
        Dim ret As String = String.Empty
        params = "{""GRP1"":{""ZRET"":""""}}"

        If X3WSC.Run("WSDTSFAO", params, retxml, False) = 1 Then
            json = JObject.Parse(retxml)
            'ret = json("GRP1")("ZRET").ToString
            ret = json.SelectToken("GRP1").SelectToken("ZRET").ToString
        End If
        Return ret
    End Function

    Public Sub WSDTSFAO_ASYNC()
        'Attention : ne pas enregistrer de trace si tout est ok dans cette fonction : est apelée dans un timer fréquent ! 
        Dim params As String
        Dim webserv As String
        Dim UserState As String
        webserv = "WSDTSFAO"
        params = "{""GRP1"":{""ZRET"":""""}}"
        UserState = webserv
        X3WSC.RunAsync(webserv, params, SfaoTest)
    End Sub


    'Web service qui récupère les paramètres X3 (site ou utilisateur)
    'lors de la 1ère lecture le paramètre est enregistré dans la collection ParamX3 pour éviter de le relire la fois suivante
    Public Function WSGETPARAM(ByVal zpar As String, ByVal zfcy As String, ByVal zusr As String) As String
        Dim retour As String = String.Empty
        Dim retxml As String = String.Empty
        Dim jsonobj As JObject
        Dim deja As Boolean = False
        'on vérifie si le parmètre a déjà été enregistré dans la collection ParamX3
        If ParamX3.Count > 0 Then
            For Each param As ParamWS In ParamX3
                If param.ZPAR = zpar AndAlso param.ZFCY = zfcy AndAlso param.ZUSR = zusr Then
                    retour = param.ZVAL
                    deja = True
                    Trace("[WSGETPARAM] résultat lecture ParamX3 : " & zpar & " = " & retour)
                    Exit For
                End If
            Next
        End If
        If deja = False Then
            Dim params As String
            Dim jsonpar As New WSParam
            Dim settings As JsonSerializerSettings

            settings = New JsonSerializerSettings() With {
                .NullValueHandling = NullValueHandling.Ignore,
                .MissingMemberHandling = MissingMemberHandling.Ignore
            }

            jsonpar.GRP1.ZPAR = zpar
            jsonpar.GRP1.ZFCY = zfcy
            jsonpar.GRP1.ZUSR = zusr

            params = ClassToJson(Of WSParam)(jsonpar, False, settings)
            If X3WSC.Run("WSGETPARAM", params, retxml, True) = 1 Then
                jsonobj = JObject.Parse(retxml)
                retour = jsonobj.SelectToken("GRP1").SelectToken("ZVAL").ToString
            End If
        End If
        Return retour
    End Function

    'Web service qui récupère les données du site (on passe un des paramètres et il récupère les autres)
    Public Function WSGETSITE(ByVal _cod As String, ByVal _desc As String, ByVal _desl As String, ByRef _doss As String, ByVal _soc As String) As WSSite
        Dim params As String
        Dim retxml As String = String.Empty
        Dim json As New WSSite
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
            .NullValueHandling = NullValueHandling.Ignore,
            .MissingMemberHandling = MissingMemberHandling.Ignore
        }

        json.GRP1.FCY = _cod
        json.GRP1.FCYNAM = _desc
        json.GRP1.FCYSHO = _desl
        json.GRP1.DOSSIER = _doss
        json.GRP1.SOCIETE = _soc

        params = ClassToJson(Of WSSite)(json, False, settings)

        If X3WSC.Run("WSGETSITE", params, retxml, True) = 1 Then
            json = JsonToClass(Of WSSite)(retxml, settings)
        End If

        Return json
    End Function

    'Web service qui récupère les données du poste
    Public Function WSGETPOSTE(ByVal _cod As String, ByVal _site As String) As WSPoste
        Dim params As String
        Dim retxml As String = String.Empty
        Dim json As New WSPoste

        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
            .NullValueHandling = NullValueHandling.Ignore,
            .MissingMemberHandling = MissingMemberHandling.Ignore
        }

        json.GRP1.WST = _cod
        json.GRP1.WCRFCY = _site

        params = ClassToJson(Of WSPoste)(json, False, settings)

        If X3WSC.Run("WSGETPOSTE", params, retxml, True) = 1 Then
            json = JsonToClass(Of WSPoste)(retxml, settings)
        End If

        Return json
    End Function

    'web service qui récupère les événements liées à un poste
    Public Function WSGETEVT(ByVal _cod As String) As WSEvt
        Dim params As String
        Dim retxml As String = String.Empty
        Dim json As New WSEvt
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
            .NullValueHandling = NullValueHandling.Ignore,
            .MissingMemberHandling = MissingMemberHandling.Ignore
        }

        json.GRP1.WST = _cod

        params = ClassToJson(Of WSEvt)(json, False, settings)
        If X3WSC.Run("WSGETEVT", params, retxml, True) = 1 Then
            json = JsonToClass(Of WSEvt)(retxml, settings)
        End If

        Return json
    End Function
    'Web service qui récupère la situation du poste
    Public Function WSGETSITPS(ByVal _cod As String, ByVal _sit As String) As WSSitPs
        Dim params As String
        Dim retxml As String = String.Empty
        Dim json As New WSSitPs
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore,
                    .DateFormatString = "yyyyMMdd"
                }
        json.GRP1.WST = _cod
        json.GRP1.FCY = _sit

        params = ClassToJson(Of WSSitPs)(json, False, settings)
        If X3WSC.Run("WSGETSITPS", params, retxml, True) = 1 Then
            json = JsonToClass(Of WSSitPs)(retxml, settings)
        End If

        Return json
    End Function
    'Web service qui récupère la situation des OF/Opé
    Public Function WSGETSITOF(ByVal _cod As String, ByVal _sit As String) As WSSitOF
        Dim params As String
        Dim retxml As String = String.Empty
        Dim json As New WSSitOF
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore,
                    .DateFormatString = "yyyyMMdd"
                }

        json.GRP1.WST = _cod
        json.GRP1.FCY = _sit

        params = ClassToJson(Of WSSitOF)(json, False, settings)
        If X3WSC.Run("WSGETSITOF", params, retxml, True) = 1 Then
            json = JsonToClass(Of WSSitOF)(retxml, settings)
        End If

        Return json
    End Function
    'Web service qui récupère la situation des composants ou palettes
    Public Function WSGETSITCP(ByVal _cod As String, ByVal _sit As String, ByVal _typop As String, ByVal _locmac As String, ByVal _locate As String) As WSSitCP
        Dim params As String
        Dim retxml As String = String.Empty
        Dim json As New WSSitCP
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore,
                    .DateFormatString = "yyyyMMdd"
                }

        json.GRP1.WST = _cod
        json.GRP1.FCY = _sit
        json.GRP1.Y_TYPOP = _typop
        json.GRP1.LOCMAC = _locmac
        json.GRP1.LOCATE = _locate

        params = ClassToJson(Of WSSitCP)(json, False, settings)
        If X3WSC.Run("WSGETSITCP", params, retxml, True) = 1 Then
            json = JsonToClass(Of WSSitCP)(retxml, settings)
        End If

        Return json
    End Function
    'Web service qui récupère les infos du matricule 
    Public Function WSGetMatr(ByVal _matr As Integer) As WSMatr
        Dim params As String
        Dim json As New WSMatr
        Dim retxml As String = String.Empty
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore,
                    .DateFormatString = "yyyyMMdd"
                }

        json.GRP1.ZEMPNUM = _matr

        params = ClassToJson(Of WSMatr)(json, False, settings)
        If X3WSC.Run("WSGETMATR", params, retxml, True) = 1 Then
            json = JsonToClass(Of WSMatr)(retxml, settings)
        End If
        Return json
    End Function

    'Web service qui enregistre une entrée d'opérateur
    Public Function WSEntOp(ByVal _site As String, ByVal _poste As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Boolean
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim json As JObject
        Dim ret As Boolean = False

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSENTOP", params, retxml, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CBool(CInt(json.SelectToken("GRP1").SelectToken("ZRET")))
        End If
        Return ret
    End Function
    'web service qui enregistre une copie de situation
    Public Function WSCopSit(ByVal _site As String, ByVal _poste As String, ByVal _empdst As Integer, ByVal _empori As Integer, ByRef _retmsg As String) As Boolean
        'Dim par As Object
        'Dim params As String
        Dim ret As Boolean = False

        Return ret
    End Function

    'Web service qui enregistre un départ d'opérateur
    Public Function WSSorOp(ByVal _site As String, ByVal _poste As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Boolean
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim json As JObject
        Dim ret As Boolean = False

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSSOROP", params, retxml, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CBool(CInt(json.SelectToken("GRP1").SelectToken("ZRET")))
        End If
        Return ret
    End Function

    'Web service qui récupère les infos de l'OF/OP 
    Public Function WSOFOPInfo(ByVal _fcy As String, ByVal _of As String, ByVal _op As Integer, ByVal _typop As String) As WSOFOPInfo
        'Dim params As String
        Dim json As New WSOFOPInfo

        Return json
    End Function
    'Web service qui récupère le dernier OF/Opé/évt du poste
    Public Function WSGETLSTEV(ByVal _site As String, ByVal _poste As String) As WSLstEvt
        'Dim params As String
        Dim json As New WSLstEvt


        Return json
    End Function
    'Web service qui enregistre un début d'opération
    Public Function WSDEBOPE(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
        'Dim par As Object
        'Dim params As String
        Dim ret As Integer

        Return ret
    End Function

    'Web service qui enregistre un début de réglage
    Public Function WSDEBRGL(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _phase As Integer, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
        'Dim par As Object
        'Dim params As String
        Dim ret As Integer

        Return ret
    End Function

    '########################################################################################################################
    'Liste des classes utilisées pour les web services
    '########################################################################################################################
    Public Function ClassToJson(Of T)(data As T, Optional isEmptyToNull As Boolean = False, Optional jsonSettings As JsonSerializerSettings = Nothing) As String

        Dim response As String = String.Empty

        If Not EqualityComparer(Of T).Default.Equals(data, Nothing) Then
            response = JsonConvert.SerializeObject(data, jsonSettings)
        End If

        Return If(isEmptyToNull, (If(response = "{}", "null", response)), response)

    End Function

    Public Function JsonToClass(Of T)(data As String, Optional jsonSettings As JsonSerializerSettings = Nothing) As T

        Dim response As T = Nothing

        If Not String.IsNullOrEmpty(data) Then
            response = If(jsonSettings Is Nothing,
                          JsonConvert.DeserializeObject(Of T)(data),
                          JsonConvert.DeserializeObject(Of T)(data, jsonSettings))
        End If

        Return response

    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

Public Class X3WebServAsync
    Public Sub RunAsyncCompleted(ByVal UserState As String, ByVal Result As String)
        Dim json As JObject
        Dim retour As String = ""

        Select Case UserState
            Case "WSDTSFAO"
                json = JObject.Parse(Result)
                retour = json("GRP1")("ZRET").ToString
                FenSfao.AnimRet(retour)
        End Select

    End Sub
End Class
