'Imports System.Net
'Imports V12 = SFAO.X3WebReferenceV12
'Imports Newtonsoft.Json
'Imports Newtonsoft.Json.Linq

''Classe qui gère les web services X3
'Partial Public Class X3WebServices

'    Private WithEvents x3WebSrv As CAdxWebServiceXmlCCServiceBasicAuth
'    Private callContext As V12.CAdxCallContext
'    Private resultXML As V12.CAdxResultXml
'    Private cadxMessage As V12.CAdxMessage
'    Private TraceCountErr As Integer    'Variable pour limiter l'enregistrement des traces des erreurs fréquentes 
'    Public Sub New(ByVal dossier As String)
'        Try
'            If SFAO.ParamDos("WEBSERVEURURL", dossier) <> "" Then

'                x3WebSrv = New CAdxWebServiceXmlCCServiceBasicAuth
'                callContext = New V12.CAdxCallContext
'                resultXML = New V12.CAdxResultXml

'                callContext.codeLang = SFAO.ParamDos("WEBSERVEURLANG", dossier)
'                callContext.poolAlias = SFAO.ParamDos("WEBSERVEURPOOLALIAS", dossier)
'                x3WebSrv.Url = SFAO.ParamDos("WEBSERVEURURL", dossier)
'                Trace("Url web services : " & x3WebSrv.Url)
'                callContext.requestConfig = SFAO.ParamDos("WEBSERVEURPARAM", dossier)

'                Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " WEBSERVEURUSER " & SFAO.ParamDos("WEBSERVEURUSER", dossier))
'                Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " WEBSERVEURPWD " & SFAO.ParamDos("WEBSERVEURPWD", dossier)) '& Crypt.Decrypt(SFAO.ParamDos("WEBSERVEURPWD", dossier)))

'                x3WebSrv.Credentials = New NetworkCredential(SFAO.ParamDos("WEBSERVEURUSER", dossier), Crypt.Decrypt(SFAO.ParamDos("WEBSERVEURPWD", dossier)))
'                x3WebSrv.PreAuthenticate = True
'                x3WebSrv.Timeout = CInt(SFAO.ParamDos("WEBSERVEURTIMEOUT", dossier))
'            End If
'        Catch ex As Exception
'            Trace("Erreur : impossible d'initialiser les web services pour le dossier " & dossier & "!", FichierTrace.niveau.erreur)
'            Trace(ex.Message, FichierTrace.niveau.erreur)
'        End Try
'    End Sub

'    'Methode qui ferme le web service
'    Public Sub Close()
'        x3WebSrv.Dispose()
'    End Sub

'    'Web service permettant la récupération de la date+heure (pour contrôler que les ws fonctionnent)
'    Public Function WSDTSFAO() As String
'        'Attention : ne pas enregistrer de trace dans cette fonction : est apelée dans un timer fréquent ! 
'        Dim params As String
'        Dim json As JObject
'        Dim ret As String = ""
'        params = "{""GRP1"":{""ZRET"":""""}}"

'        Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " WSDTSFAO " & params)
'        Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " callContext " & callContext.ToString)
'        Try
'            resultXML = x3WebSrv.run(callContext, "WSDTSFAO", params)
'        Catch ex As Exception
'            TraceCountErr += 1
'            If TraceCountErr < 10 Then
'                Trace("[WSDTSFAO] Erreur run du web service !", FichierTrace.niveau.erreur)
'                Trace(ex.Message, FichierTrace.niveau.erreur)
'            End If
'            Return ret 'vide
'        End Try

'        If resultXML.status = 0 Then
'            'Erreur
'            TraceCountErr += 1
'            If TraceCountErr < 10 Then
'                Trace("[WSDTSFAO] Erreur statut du web service !", FichierTrace.niveau.erreur)
'                For Each cadxMessage In resultXML.messages
'                    Trace(cadxMessage.message, FichierTrace.niveau.erreur)
'                Next
'            End If
'        Else
'            json = JObject.Parse(resultXML.resultXml)
'            ret = json("GRP1")("ZRET").ToString
'            TraceCountErr = 0
'        End If
'        Return ret
'    End Function

'    Public Sub WSDTSFAO_ASYNC()
'        'Attention : ne pas enregistrer de trace dans cette fonction : est apelée dans un timer fréquent ! 
'        Dim params As String
'        Dim webserv As String
'        Dim UserState As Object
'        webserv = "WSDTSFAO"
'        params = "{""GRP1"":{""ZRET"":""""}}"
'        UserState = webserv
'        Try
'            WsNbrAsyncInvoke += 1 'on incrémente le nombre d'appel asynchrones en attente
'            x3WebSrv.runAsync(callContext, webserv, params, UserState)
'        Catch ex As Exception
'            TraceCountErr += 1
'            If TraceCountErr < 10 Then
'                Trace("[WSDTSFAO_ASYNC] Erreur d'appel runAsync du web service !", FichierTrace.niveau.erreur)
'                Trace(ex.Message, FichierTrace.niveau.erreur)
'            End If
'        End Try
'    End Sub

'    Public Sub RunAsyncCompleted(sender As Object, e As V12.runCompletedEventArgs) Handles x3WebSrv.runCompleted
'        Dim json As JObject
'        Dim ret As String = ""
'        WsNbrAsyncInvoke -= 1 'on décrémente le nmbre d'appels en attente
'        'Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " RunAsyncCompleted " & "test n° " & znbtest.ToString)

'        Select Case e.UserState.ToString
'            Case "WSDTSFAO"
'                If e.Error Is Nothing Then 'si pas d'erreur
'                    resultXML = e.Result    'on récupère le résultat
'                    If resultXML.status = 0 Then 'on vérifie le statut
'                        'Statut en erreur (côté X3)
'                        TraceCountErr += 1
'                        If TraceCountErr < 10 Then
'                            Trace("[WSDTSFAO_ASYNC] Erreur statut du web service !", FichierTrace.niveau.erreur)
'                            For Each cadxMessage In resultXML.messages
'                                Trace(cadxMessage.message, FichierTrace.niveau.erreur)
'                            Next
'                        End If
'                        FenSfao.AnimRet("") 'on affiche l'icône erreur
'                    Else
'                        TraceCountErr = 0
'                        json = JObject.Parse(resultXML.resultXml)
'                        ret = json("GRP1")("ZRET").ToString
'                        FenSfao.AnimRet(ret) 'on animme le globe
'                    End If
'                Else 'on est en erreur (côté appel web service)
'                    TraceCountErr += 1
'                    If TraceCountErr < 10 Then
'                        Trace("[WSDTSFAO_ASYNC] Erreur d'appel du web service !", FichierTrace.niveau.erreur)
'                        Trace(e.Error.Message, FichierTrace.niveau.erreur)
'                    End If
'                    x3WebSrv.CancelAsync(e.UserState)
'                    FenSfao.AnimRet("") 'on affiche l'icône erreur
'                End If
'        End Select
'    End Sub

'    'Web service qui récupère les paramètres X3 (site ou utilisateur)
'    'lors de la 1ère lecture le paramètre est enregistré dans la collection ParamX3 pour éviter de le relire la fois suivante
'    Public Function WSGETPARAM(ByVal zpar As String, ByVal zfcy As String, ByVal zusr As String) As String
'        Dim retour As String = String.Empty
'        Dim deja As Boolean = False
'        'on vérifie si le parmètre a déjà été enregistré dans la collection ParamX3
'        If ParamX3.Count > 0 Then
'            For Each param As ParamWS In ParamX3
'                If param.ZPAR = zpar AndAlso param.ZFCY = zfcy AndAlso param.ZUSR = zusr Then
'                    retour = param.ZVAL
'                    deja = True
'                    Trace("[WSGETPARAM] résultat lecture ParamX3 : " & zpar & " = " & retour)
'                    Exit For
'                End If
'            Next
'        End If
'        If deja = False Then
'            Dim params As String
'            Dim json As New WSParam
'            Dim settings As JsonSerializerSettings

'            settings = New JsonSerializerSettings() With {
'                .NullValueHandling = NullValueHandling.Ignore,
'                .MissingMemberHandling = MissingMemberHandling.Ignore
'            }

'            json.GRP1.ZPAR = zpar
'            json.GRP1.ZFCY = zfcy
'            json.GRP1.ZUSR = zusr

'            params = ClassToJson(Of WSParam)(json, False, settings)
'            Trace("[WSGETPARAM] paramètres : " & params)
'            resultXML = x3WebSrv.run(callContext, "WSGETPARAM", params)
'            If resultXML.status = 0 Then
'                'Erreur
'                Trace("[WSGETPARAM] erreur du web service ! ", FichierTrace.niveau.erreur)
'                For Each cadxMessage In resultXML.messages
'                    Trace("[WSGETPARAM] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'                Next
'            Else
'                Trace("[WSGETPARAM] résultat : " & resultXML.resultXml)
'                json = JsonToClass(Of WSParam)(resultXML.resultXml, settings)
'                ParamX3.Add(json.GRP1)
'            End If
'            retour = json.GRP1.ZVAL
'        End If
'        Return retour
'    End Function

'    'Web service qui récupère les données du site (on passe un des paramètres et il récupère les autres)
'    Public Function WSGETSITE(ByVal _cod As String, ByVal _desc As String, ByVal _desl As String, ByRef _doss As String, ByVal _soc As String) As WSSite
'        Dim params As String
'        Dim json As New WSSite
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore
'        }

'        json.GRP1.FCY = _cod
'        json.GRP1.FCYNAM = _desc
'        json.GRP1.FCYSHO = _desl
'        json.GRP1.DOSSIER = _doss
'        json.GRP1.SOCIETE = _soc

'        params = ClassToJson(Of WSSite)(json, False, settings)
'        Trace("[WSGETSITE] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSGETSITE", params)
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSGETSITE] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSGETSITE] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSGETSITE] résultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSSite)(resultXML.resultXml, settings)
'        End If

'        Return json
'    End Function

'    'Web service qui récupère les données du poste
'    Public Function WSGETPOSTE(ByVal _cod As String, ByVal _site As String) As WSPoste
'        Dim params As String
'        Dim json As New WSPoste
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore
'        }

'        json.GRP1.WST = _cod
'        json.GRP1.WCRFCY = _site

'        params = ClassToJson(Of WSPoste)(json, False, settings)
'        Trace("[WSGETPOSTE] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSGETPOSTE", params)
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSGETPOSTE] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSGETPOSTE] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSGETPOSTE] résultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSPoste)(resultXML.resultXml, settings)
'        End If

'        Return json
'    End Function

'    'web service qui récupère les événements liées à un poste
'    Public Function WSGETEVT(ByVal _cod As String) As WSEvt
'        Dim params As String
'        Dim json As New WSEvt
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore
'        }

'        json.GRP1.WST = _cod

'        params = ClassToJson(Of WSEvt)(json, False, settings)
'        Trace("[WSGETEVT] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSGETEVT", params)
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSGETEVT] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSGETEVT] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSGETEVT] résultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSEvt)(resultXML.resultXml, settings)
'        End If

'        Return json
'    End Function
'    'Web service qui récupère la situation du poste
'    Public Function WSGETSITPS(ByVal _cod As String, ByVal _sit As String) As WSSitPs
'        Dim params As String
'        Dim json As New WSSitPs
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore,
'            .DateFormatString = "yyyyMMdd"
'        }

'        json.GRP1.WST = _cod
'        json.GRP1.FCY = _sit

'        params = ClassToJson(Of WSSitPs)(json, False, settings)
'        Trace("[WSGETSITPS] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSGETSITPS", params)
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSGETSITPS] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSGETSITPS] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSGETSITPS] résultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSSitPs)(resultXML.resultXml, settings)
'        End If

'        Return json
'    End Function
'    'Web service qui récupère la situation des OF/Opé
'    Public Function WSGETSITOF(ByVal _cod As String, ByVal _sit As String) As WSSitOF
'        Dim params As String
'        Dim json As New WSSitOF
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore,
'            .DateFormatString = "yyyyMMdd"
'        }

'        json.GRP1.WST = _cod
'        json.GRP1.FCY = _sit

'        params = ClassToJson(Of WSSitOF)(json, False, settings)
'        Trace("[WSGETSITOF] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSGETSITOF", params)
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSGETSITOF] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSGETSITOF] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSGETSITOF] résultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSSitOF)(resultXML.resultXml, settings)
'        End If

'        Return json
'    End Function
'    'Web service qui récupère la situation des composants ou palettes
'    Public Function WSGETSITCP(ByVal _cod As String, ByVal _sit As String, ByVal _typop As String, ByVal _locmac As String, ByVal _locate As String) As WSSitCP
'        Dim params As String
'        Dim json As New WSSitCP
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore
'        }

'        json.GRP1.WST = _cod
'        json.GRP1.FCY = _sit
'        json.GRP1.Y_TYPOP = _typop
'        json.GRP1.LOCMAC = _locmac
'        json.GRP1.LOCATE = _locate

'        params = ClassToJson(Of WSSitCP)(json, False, settings)
'        Trace("[WSGETSITCP] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSGETSITCP", params)
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSGETSITCP] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSGETSITCP] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSGETSITCP] résultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSSitCP)(resultXML.resultXml, settings)
'        End If

'        Return json
'    End Function
'    'Web service qui récupère les infos du matricule 
'    Public Function WSGetMatr(ByVal _matr As Integer) As WSMatr
'        Dim params As String
'        Dim json As New WSMatr
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore
'        }

'        FenSfao.X3Anim(3) 'on tourne x fois
'        json.GRP1.ZEMPNUM = _matr
'        params = ClassToJson(Of WSMatr)(json, False, settings)
'        Trace("[WSGETMATR] paramètres : " & params)

'        'Try
'        resultXML = x3WebSrv.run(callContext, "WSGETMATR", params)
'        'resultXML = x3WebSrv.getDescription(callContext, "WSGETMATR")
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSGETMATR] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSGETMATR] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSGetMatr] resultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSMatr)(resultXML.resultXml, settings)
'            'Debug.WriteLine("Debug " & json.GRP1.ZEMPDES)
'        End If
'        FenSfao.X3Anim(2) 'on tourne 1 fois
'        Return json
'    End Function
'    'Web service qui récupère le dernier OF/Opé/évt du poste
'    Public Function WSGETLSTEV(ByVal _site As String, ByVal _poste As String) As WSLstEvt
'        Dim params As String
'        Dim json As New WSLstEvt
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore
'        }

'        json.GRP1.FCY = _site
'        json.GRP1.WST = _poste

'        params = ClassToJson(Of WSLstEvt)(json, False, settings)
'        Trace("[WSGETLSTEV] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSGETLSTEV", params)
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSGETLSTEV] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSGETLSTEV] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSGETLSTEV] résultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSLstEvt)(resultXML.resultXml, settings)
'        End If

'        Return json
'    End Function
'    'Web service qui enregistre une entrée d'opérateur
'    Public Function WSEntOp(ByVal _site As String, ByVal _poste As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Boolean
'        Dim par As Object
'        Dim params As String
'        Dim ret As Boolean = False

'        FenSfao.X3Anim(3)
'        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZRET = 0, .ZMSG = ""}}
'        params = JsonConvert.SerializeObject(par)
'        Trace("[WSENTOP] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSENTOP", params)

'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSENTOP] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                _retmsg += cadxMessage.message
'                Trace("[WSENTOP] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'            ret = False
'        Else
'            Trace("[WSENTOP] resultat : " & resultXML.resultXml)
'            Dim json As JObject = JObject.Parse(resultXML.resultXml)
'            ret = CBool(CInt(json.SelectToken("GRP1").SelectToken("ZRET")))
'        End If
'        FenSfao.X3Anim(2)
'        Return ret
'    End Function
'    'web service qui enregistre une copie de situation
'    Public Function WSCopSit(ByVal _site As String, ByVal _poste As String, ByVal _empdst As Integer, ByVal _empori As Integer, ByRef _retmsg As String) As Boolean
'        Dim par As Object
'        Dim params As String
'        Dim ret As Boolean = False
'        FenSfao.X3Anim(3)
'        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZEMPDST = _empdst, .ZEMPORI = _empori, .ZRET = 0, .ZMSG = ""}}
'        params = JsonConvert.SerializeObject(par)
'        Trace("[WSCOPSIT] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSCOPSIT", params)

'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSCOPSIT] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                _retmsg += cadxMessage.message
'                Trace("[WSCOPSIT] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'            ret = False
'        Else
'            Trace("[WSCOPSIT] resultat : " & resultXML.resultXml)
'            Dim json As JObject = JObject.Parse(resultXML.resultXml)
'            ret = CBool(CInt(json.SelectToken("GRP1").SelectToken("ZRET")))
'        End If
'        FenSfao.X3Anim(2)
'        Return ret
'    End Function

'    'Web service qui enregistre un départ d'opérateur
'    Public Function WSSorOp(ByVal _site As String, ByVal _poste As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Boolean
'        Dim par As Object
'        Dim params As String
'        Dim ret As Boolean = False

'        FenSfao.X3Anim(3)
'        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZRET = 0, .ZMSG = ""}}
'        params = JsonConvert.SerializeObject(par)
'        Trace("[WSSOROP] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSSOROP", params)

'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSSOROP] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                _retmsg += cadxMessage.message
'                Trace("[WSSOROP] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'            ret = False
'        Else
'            Trace("[WSSOROP] resultat : " & resultXML.resultXml)
'            Dim json As JObject = JObject.Parse(resultXML.resultXml)
'            ret = CBool(CInt(json.SelectToken("GRP1").SelectToken("ZRET")))
'        End If
'        FenSfao.X3Anim(2)
'        Return ret
'    End Function

'    'Web service qui récupère les infos de l'OF/OP 
'    Public Function WSOFOPInfo(ByVal _fcy As String, ByVal _of As String, ByVal _op As Integer, ByVal _typop As String) As WSOFOPInfo
'        Dim params As String
'        Dim json As New WSOFOPInfo
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore
'        }

'        FenSfao.X3Anim(3) 'on tourne x fois
'        json.GRP1.ZFCY = _fcy
'        json.GRP1.ZMFGNUM = _of
'        json.GRP1.ZOPENUM = _op
'        json.GRP1.ZTYPOP = _typop 'type du poste
'        params = ClassToJson(Of WSOFOPInfo)(json, False, settings)
'        Trace("[WSOFOPINFO] paramètres : " & params)

'        resultXML = x3WebSrv.run(callContext, "WSOFOPINFO", params)
'        'resultXML = x3WebSrv.getDescription(callContext, "WSGETOFOPINFO")
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSOFOPINFO] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSOFOPINFO] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'        Else
'            Trace("[WSOFOPINFO] resultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSOFOPInfo)(resultXML.resultXml, settings)
'            'Debug.WriteLine("Debug " & json.GRP1.ZMFGNUM)
'        End If
'        FenSfao.X3Anim(2) 'on tourne 1 fois
'        Return json
'    End Function

'    'Web service qui enregistre un début d'opération
'    Public Function WSDEBOPE(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
'        Dim par As Object
'        Dim params As String
'        Dim ret As Integer

'        FenSfao.X3Anim(3)
'        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZMFGNUM = _of, .ZOPENUM = _op, .ZRET = 0, .ZMSG = ""}}
'        params = JsonConvert.SerializeObject(par)
'        Trace("[WSDEBOPE] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSDEBOPE", params)

'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSDEBOPE] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                _retmsg += cadxMessage.message
'                Trace("[WSDEBOPE] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'            ret = -1
'        Else
'            Trace("[WSDEBOPE] resultat : " & resultXML.resultXml)
'            Dim json As JObject = JObject.Parse(resultXML.resultXml)
'            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
'            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
'        End If
'        FenSfao.X3Anim(2)
'        Return ret
'    End Function

'    'Web service qui enregistre un début de réglage
'    Public Function WSDEBRGL(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _phase As Integer, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
'        Dim par As Object
'        Dim params As String
'        Dim ret As Integer

'        FenSfao.X3Anim(3)
'        par = New With {Key .GRP1 = New With {.ZFCY = _site, .ZPOSTE = _poste, .ZTYPOP = _typop, .ZEMPNUM = _empnum, .ZEVTNUM = _evtnum, .ZPHASE = _phase, .ZMFGNUM = _of, .ZOPENUM = _op, .ZRET = 0, .ZMSG = ""}}
'        params = JsonConvert.SerializeObject(par)
'        Trace("[WSDEBRGL] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "WSDEBRGL", params)

'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSDEBRGL] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                _retmsg += cadxMessage.message
'                Trace("[WSDEBRGL] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'            Next
'            ret = -1
'        Else
'            Trace("[WSDEBRGL] resultat : " & resultXML.resultXml)
'            Dim json As JObject = JObject.Parse(resultXML.resultXml)
'            ret = CInt(json.SelectToken("GRP1").SelectToken("ZRET"))
'            _retmsg = json.SelectToken("GRP1").SelectToken("ZMSG").ToString
'        End If
'        FenSfao.X3Anim(2)
'        Return ret
'    End Function

'    '########################################################################################################################
'    'Liste des classes utilisées pour les web services
'    '########################################################################################################################
'    Public Function ClassToJson(Of T)(data As T, Optional isEmptyToNull As Boolean = False, Optional jsonSettings As JsonSerializerSettings = Nothing) As String

'        Dim response As String = String.Empty

'        If Not EqualityComparer(Of T).Default.Equals(data, Nothing) Then
'            response = JsonConvert.SerializeObject(data, jsonSettings)
'        End If

'        Return If(isEmptyToNull, (If(response = "{}", "null", response)), response)

'    End Function

'    Public Function JsonToClass(Of T)(data As String, Optional jsonSettings As JsonSerializerSettings = Nothing) As T

'        Dim response As T = Nothing

'        If Not String.IsNullOrEmpty(data) Then
'            response = If(jsonSettings Is Nothing,
'                          JsonConvert.DeserializeObject(Of T)(data),
'                          JsonConvert.DeserializeObject(Of T)(data, jsonSettings))
'        End If

'        Return response

'    End Function

'    Protected Overrides Sub Finalize()
'        MyBase.Finalize()
'    End Sub
'End Class

