'------------------------------------------------------------------------------------------------------------------------
'Modifications:
' 
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
        Dim MsgErrWs As String = String.Empty
        params = "{""GRP1"":{""ZRET"":""""}}"

        If X3WSC.Run("WSDTSFAO", params, retxml, MsgErrWs, False) = 1 Then
            json = JObject.Parse(retxml)
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
        Dim MsgErrWs As String = String.Empty
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
            If X3WSC.Run("WSGETPARAM", params, retxml, MsgErrWs, True) = 1 Then
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
        Dim MsgErrWs As String = String.Empty
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

        If X3WSC.Run("WSGETSITE", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSSite)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function

    'Web service qui récupère les données du poste
    Public Function WSGETPOSTE(ByVal _cod As String, ByVal _site As String) As WSPoste
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As New WSPoste

        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
            .NullValueHandling = NullValueHandling.Ignore,
            .MissingMemberHandling = MissingMemberHandling.Ignore
        }

        json.GRP1.WST = _cod
        json.GRP1.WCRFCY = _site

        params = ClassToJson(Of WSPoste)(json, False, settings)

        If X3WSC.Run("WSGETPOSTE", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSPoste)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function

    'web service qui récupère les événements liées à un poste
    Public Function WSGETEVT(ByVal _cod As String) As WSEvt
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As New WSEvt
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
            .NullValueHandling = NullValueHandling.Ignore,
            .MissingMemberHandling = MissingMemberHandling.Ignore
        }

        json.GRP1.WST = _cod

        params = ClassToJson(Of WSEvt)(json, False, settings)
        If X3WSC.Run("WSGETEVT", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSEvt)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function

    'web service qui récupère les motifs de non solde liées à un poste
    Public Function WSGETMNS(ByVal _cod As String) As WSMns
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As New WSMns
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
            .NullValueHandling = NullValueHandling.Ignore,
            .MissingMemberHandling = MissingMemberHandling.Ignore
        }

        json.GRP1.WST = _cod

        params = ClassToJson(Of WSMns)(json, False, settings)
        If X3WSC.Run("WSGETMNS", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSMns)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function
    'Web service qui récupère la situation du poste
    Public Function WSGETSITPS(ByVal _cod As String, ByVal _sit As String) As WSSitPs
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
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
        If X3WSC.Run("WSGETSITPS", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSSitPs)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function
    'Web service qui récupère la situation des OF/Opé
    Public Function WSGETSITOF(ByVal _cod As String, ByVal _sit As String) As WSSitOF
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
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
        If X3WSC.Run("WSGETSITOF", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSSitOF)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function
    'Web service qui récupère la situation des composants ou palettes
    Public Function WSGETSITCP(ByVal _cod As String, ByVal _sit As String, ByVal _typop As String, ByVal _locmac As String, ByVal _locate As String) As WSSitCP
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
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
        If X3WSC.Run("WSGETSITCP", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSSitCP)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function
    'Web service qui récupère les infos du matricule 
    Public Function WSGetMatr(ByVal _matr As Integer) As WSMatr
        Dim params As String
        Dim json As New WSMatr
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim settings As JsonSerializerSettings

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore,
                    .DateFormatString = "yyyyMMdd"
                }

        json.GRP1.ZEMPNUM = _matr

        params = ClassToJson(Of WSMatr)(json, False, settings)
        If X3WSC.Run("WSGETMATR", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSMatr)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If
        Return json
    End Function

    'Web service qui enregistre une entrée d'opérateur
    Public Function WSEntOp(ByVal _site As String, ByVal _poste As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Boolean
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Boolean

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSENTOP", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CBool(CInt(json.SelectToken("GRP1").SelectToken("ZRET")))
        Else
            ret = False
            _retmsg = MsgErrWs
        End If
        Return ret
    End Function
    'web service qui enregistre une copie de situation
    Public Function WSCopSit(ByVal _site As String, ByVal _poste As String, ByVal _empdst As Integer, ByVal _empori As Integer, ByRef _retmsg As String) As Boolean
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Boolean

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZEMPDST = _empdst, .ZEMPORI = _empori, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSCOPSIT", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CBool(CInt(json.SelectToken("GRP1").SelectToken("ZRET")))
        Else
            ret = False
            _retmsg = MsgErrWs
        End If
        Return ret
    End Function

    'Web service qui enregistre un départ d'opérateur
    Public Function WSSorOp(ByVal _site As String, ByVal _poste As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Boolean
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Boolean

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSSOROP", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CBool(CInt(json.SelectToken("GRP1").SelectToken("ZRET")))
        Else
            ret = False
            _retmsg = MsgErrWs
        End If
        Return ret
    End Function

    'Web service qui récupère les infos de l'OF/OP 
    Public Function WSOFOPInfo(ByVal _fcy As String, ByVal _of As String, ByVal _op As Integer, ByVal _typop As String) As WSOFOPInfo
        Dim params As String
        Dim json As New WSOFOPInfo
        Dim settings As JsonSerializerSettings
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore
                }

        json.GRP1.ZFCY = _fcy
        json.GRP1.ZMFGNUM = _of
        json.GRP1.ZOPENUM = _op
        json.GRP1.ZTYPOP = _typop 'type du poste
        params = ClassToJson(Of WSOFOPInfo)(json, False, settings)

        If X3WSC.Run("WSOFOPINFO", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSOFOPInfo)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function
    'Web service qui récupère le dernier OF/Opé/évt du poste
    Public Function WSGETLSTEV(ByVal _site As String, ByVal _poste As String) As WSLstEvt
        Dim params As String
        Dim json As New WSLstEvt
        Dim settings As JsonSerializerSettings
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore
                }

        json.GRP1.FCY = _site
        json.GRP1.WST = _poste
        params = ClassToJson(Of WSLstEvt)(json, False, settings)

        If X3WSC.Run("WSGETLSTEV", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSLstEvt)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If
        Return json
    End Function

    'Web service qui enregistre un début d'opération
    Public Function WSDEBOPE(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZMFGNUM = _of, .ZOPENUM = _op, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSDEBOPE", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui enregistre un début de réglage
    Public Function WSDEBRGL(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _phase As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZPHASE = _phase, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSDEBRGL", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui enregistre un début de démontage
    Public Function WSDEBDEM(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSDEBDEM", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui enregistre un début d'interruption
    Public Function WSDEBINT(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSDEBINT", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui enregistre une fin d'opération
    Public Function WSFINOPE(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _opesld As String, ByVal _motif As String, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZOPESLD = _opesld, .ZMOTIF = _motif, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSFINOPE", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui récupère les quantités produites d'une opération
    Public Function WSGETQPRO(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByRef _qteaq As Decimal, ByRef _qteqn As Decimal, ByRef _qter As Decimal, ByRef _nbpcu As Integer, ByRef _uom As String, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZQTEAQ = _qteaq, .ZQTEQN = _qteqn, .ZQTER = _qter, .ZNBPCU = _nbpcu, .ZOPEUOM = _uom, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSGETQPRO", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _qteaq = CDec(json.SelectToken("GRP1").SelectToken("ZQTEAQ"))
            _qteqn = CDec(json.SelectToken("GRP1").SelectToken("ZQTEQN"))
            _qter = CDec(json.SelectToken("GRP1").SelectToken("ZQTER"))
            _nbpcu = CInt(json.SelectToken("GRP1").SelectToken("ZNBPCU"))
            _uom = json.SelectToken("GRP1").SelectToken("ZOPEUOM").ToString
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui récupère les quantités consommées d'une opération
    Public Function WSGETQCSO(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _stoloc As String, ByVal _empnum As Integer, ByRef _qtesup1 As Decimal, ByRef _qtesup2 As Decimal, ByRef _qteret As Decimal, ByRef _unite As String, ByRef _qteLnk1 As Decimal, ByRef _qteLnk2 As Decimal, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZSTOLOC = _stoloc, .ZEMPNUM = _empnum, .ZQTESUP1 = _qtesup1, .ZQTESUP2 = _qtesup2, .ZQTERET = _qteret, .ZSTUMAT = _unite, .ZQTYLNK1 = _qteLnk1, .ZQTYLNK2 = _qteLnk2, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSGETQCSO", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _qtesup1 = CDec(json.SelectToken("GRP1").SelectToken("ZQTESUP1"))
            _qtesup2 = CDec(json.SelectToken("GRP1").SelectToken("ZQTESUP2"))
            _qteret = CDec(json.SelectToken("GRP1").SelectToken("ZQTERET"))
            _unite = json.SelectToken("GRP1").SelectToken("ZSTUMAT").ToString
            _qteLnk1 = CDec(json.SelectToken("GRP1").SelectToken("ZQTYLNK1"))
            _qteLnk2 = CDec(json.SelectToken("GRP1").SelectToken("ZQTYLNK2"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui récupère les palettes non validées
    Public Function WSPALNVLD(ByVal _site As String, ByVal _stoloc As String, ByVal _of As String) As WSLstPal
        Dim params As String
        Dim json As New WSLstPal
        Dim settings As JsonSerializerSettings
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore
                }

        json.GRP1.ZFCY = _site
        json.GRP1.ZSTOLOC = _stoloc
        json.GRP1.ZMFGNUM = _of

        params = ClassToJson(Of WSLstPal)(json, False, settings)

        If X3WSC.Run("WSPALNVLD", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSLstPal)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function

    'Web service qui crée les consommations d'encres et vernis
    Public Function WSCSOENC(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _totenc As Decimal, ByVal _totver As Decimal, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZTOTENCRES = _totenc, .ZTOTVERNIS = _totver, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSCSOENC", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui contrôle, si cde/appel, nombre de palettes produites / nombre d'appels
    Public Function WSNBPALAPL(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByRef _nbapl As Integer, ByRef _nbpal As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZNBAPL = _nbapl, .ZNBPAL = _nbpal, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSNBPALAPL", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _nbapl = CInt(json.SelectToken("GRP1").SelectToken("ZNBAPL"))
            _nbpal = CInt(json.SelectToken("GRP1").SelectToken("ZNBPAL"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui récupère la liste des types d'étiquettes ZPL
    Public Function WSGETLETQ(ByVal _option As String) As WSTypEtq
        Dim params As String
        Dim json As New WSTypEtq
        Dim settings As JsonSerializerSettings
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore
                }

        json.GRP1.OPT = _option

        params = ClassToJson(Of WSTypEtq)(json, False, settings)

        If X3WSC.Run("WSGETLETQ", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSTypEtq)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function

    'Web service qui renvoie le type d'étiquettes ZPL par défaut
    Public Function WSGETTETQ(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByRef _typetq As String, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZTYPETQ = _typetq, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSGETTETQ", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _typetq = json.SelectToken("GRP1").SelectToken("ZTYPETQ").ToString
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui contrôle s'il faut saisir le poids des bobines
    Public Function WSSAIPDS(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByRef _repdef As String, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZREP = _repdef, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSSAIPDS", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _repdef = json.SelectToken("GRP1").SelectToken("ZREP").ToString
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui récupère le nombre de bobines filles produites
    Public Function WSGETNBBOB(ByVal _site As String, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZMFGNUM = _of, .ZMFGOPE = _op, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSGETNBBOB", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui récupère la quantité de conditionnement
    Public Function WSGETQPCU(ByVal _site As String, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZMFGNUM = _of, .ZMFGOPE = _op, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSGETQPCU", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui récupère le nombre d'unités par format (emballage)
    Public Function WSGETNBUN(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSGETNBUN", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui enregistre un début de production
    Public Function WSDEBPRO(ByVal _wsdb As WSDebPro) As WSDebPro
        Dim params As String
        Dim settings As JsonSerializerSettings
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore
                }

        params = ClassToJson(Of WSDebPro)(_wsdb, False, settings)

        If X3WSC.Run("WSDEBPRO", params, retxml, MsgErrWs, True) = 1 Then
            _wsdb = JsonToClass(Of WSDebPro)(retxml, settings)
        Else
            _wsdb.GRP1.ZRET = 0
            _wsdb.GRP1.ZMSG = MsgErrWs
        End If

        Return _wsdb
    End Function

    'Web service qui contrôle si des matières ont été ajoutées à l'OF (avec màj de la situation matières si besoin)
    Public Function WSCTRNVMAT(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSCTRNVMAT", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui contrôle si des matières ont été ajoutées à l'OF (avec màj de la situation matières si besoin)
    Public Function WSCTRCPLOT(ByVal _site As String, ByRef _art As String, ByRef _lot As String, ByRef _locatl As String, ByRef _locpst As String, ByRef _qte As Decimal, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZITMREF = _art, .ZLOTSLO = _lot, .ZLOCATL = _locatl, .ZLOCPST = _locpst, .ZQTE = _qte, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSCTRCPLOT", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            _art = json.SelectToken("GRP1").SelectToken("ZITMREF").ToString
            _lot = json.SelectToken("GRP1").SelectToken("ZLOTSLO").ToString
            _qte = CDec(json.SelectToken("GRP1").SelectToken("ZQTE"))
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

        Return ret
    End Function

    'Web service qui contrôle si des matières ont été ajoutées à l'OF (avec màj de la situation matières si besoin)
    Public Function WSLOTBPS(ByVal _art As String, ByVal _lot As String, ByRef _lotfrn As String, ByRef _bobfrn As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZITMREF = _art, .ZLOTSLO = _lot, .ZLOTFRN = _lotfrn, .ZBPSLOB = _bobfrn}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSLOTBPS", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            _lotfrn = json.SelectToken("GRP1").SelectToken("ZLOTFRN").ToString
            _bobfrn = json.SelectToken("GRP1").SelectToken("ZBPSLOB").ToString
            ret = 1
        Else
            ret = -1
        End If

        Return ret
    End Function

    'Web service qui enregistre un début de production
    Public Function WSLOTRELIQ(ByVal _site As String, ByVal _art As String, ByVal _nsup As Integer, ByVal _stoloc As String, ByVal _of As String) As WSLstLot
        Dim params As String
        Dim json As New WSLstLot
        Dim settings As JsonSerializerSettings
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty

        settings = New JsonSerializerSettings() With {
                    .NullValueHandling = NullValueHandling.Ignore,
                    .MissingMemberHandling = MissingMemberHandling.Ignore
                }

        json.GRP1.ZFCY = _site
        json.GRP1.ZITMREF = _art
        json.GRP1.ZNUMSUP = _nsup
        json.GRP1.ZSTOLOC = _stoloc
        json.GRP1.ZMFGNUM = _of
        json.GRP1.ZSTA = "A"

        params = ClassToJson(Of WSLstLot)(json, False, settings)

        If X3WSC.Run("WSLOTRELIQ", params, retxml, MsgErrWs, True) = 1 Then
            json = JsonToClass(Of WSLstLot)(retxml, settings)
        Else
            json.GRP1.ZRET = 0
            json.GRP1.ZMSG = MsgErrWs
        End If

        Return json
    End Function

    'Web service qui renvoie la liste des reliquats (lots) de matières utilisées pour un OF donné
    Public Function WSCSORELIQ(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _art As String, ByVal _nsup As Integer,
                               ByVal _stoloc As String, ByVal _lot As String, ByVal _slot As String, ByVal _qte As Decimal, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim retxml As String = String.Empty
        Dim MsgErrWs As String = String.Empty
        Dim json As JObject
        Dim ret As Integer

        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZITMREF = _art, .ZNUMSUP = _nsup,
                                              .ZSTOLOC = _stoloc, .ZLOT = _lot, .ZSLO = _slot, .ZQTE = _qte, .ZRET = 0, .ZMSG = ""}}
        params = JsonConvert.SerializeObject(par)

        If X3WSC.Run("WSCSORELIQ", params, retxml, MsgErrWs, True) = 1 Then
            json = JObject.Parse(retxml)
            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
        Else
            ret = -1
            _retmsg = MsgErrWs
        End If

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
                Try
                    json = JObject.Parse(Result)
                    retour = json("GRP1")("ZRET").ToString
                    FenSfao.AnimRet(retour)
                Catch ex As Exception
                    retour = ""
                    FenSfao.X3Anim(0)
                End Try
        End Select

    End Sub
End Class
