﻿Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

'Classe qui gère les web services X3
Public Class X3WebServ
    Private WithEvents Ws As X3WSClass                   'classe d'appel des web services X3 (ne pas instentier imédiatement)

    Public Sub New(ByVal dossier As String, ByVal version As String)
        Ws = New X3WSClass(dossier, version)
    End Sub

    'Methode qui ferme le web service
    Public Sub Close()
        Ws.Close()
    End Sub

    'Web service permettant la récupération de la date+heure (pour contrôler que les ws fonctionnent)
    Public Function WSDTSFAO() As String
        'Attention : ne pas enregistrer de trace si tout est ok dans cette fonction : est apelée dans un timer fréquent ! 
        Dim params As String
        Dim json As JObject
        Dim retxml As String = String.Empty
        Dim ret As String = String.Empty
        params = "{""GRP1"":{""ZRET"":""""}}"

        If Ws.Run("WSDTSFAO", params, retxml) = 1 Then
            json = JObject.Parse(retxml)
            ret = json("GRP1")("ZRET").ToString
        End If
        Return ret
    End Function

    Public Sub WSDTSFAO_ASYNC()

    End Sub


    'Web service qui récupère les paramètres X3 (site ou utilisateur)
    'lors de la 1ère lecture le paramètre est enregistré dans la collection ParamX3 pour éviter de le relire la fois suivante
    Public Function WSGETPARAM(ByVal zpar As String, ByVal zfcy As String, ByVal zusr As String) As String
        Dim retour As String = String.Empty

        Return retour
    End Function

    'Web service qui récupère les données du site (on passe un des paramètres et il récupère les autres)
    Public Function WSGETSITE(ByVal _cod As String, ByVal _desc As String, ByVal _desl As String, ByRef _doss As String, ByVal _soc As String) As WSSite
        Dim params As String
        Dim json As New WSSite

        Return json
    End Function

    'Web service qui récupère les données du poste
    Public Function WSGETPOSTE(ByVal _cod As String, ByVal _site As String) As WSPoste
        Dim params As String
        Dim json As New WSPoste


        Return json
    End Function

    'web service qui récupère les événements liées à un poste
    Public Function WSGETEVT(ByVal _cod As String) As WSEvt
        Dim params As String
        Dim json As New WSEvt


        Return json
    End Function
    'Web service qui récupère la situation du poste
    Public Function WSGETSITPS(ByVal _cod As String, ByVal _sit As String) As WSSitPs
        Dim params As String
        Dim json As New WSSitPs


        Return json
    End Function
    'Web service qui récupère la situation des OF/Opé
    Public Function WSGETSITOF(ByVal _cod As String, ByVal _sit As String) As WSSitOF
        Dim params As String
        Dim json As New WSSitOF


        Return json
    End Function
    'Web service qui récupère la situation des composants ou palettes
    Public Function WSGETSITCP(ByVal _cod As String, ByVal _sit As String, ByVal _typop As String, ByVal _locmac As String, ByVal _locate As String) As WSSitCP
        Dim params As String
        Dim json As New WSSitCP


        Return json
    End Function
    'Web service qui récupère les infos du matricule 
    Public Function WSGetMatr(ByVal _matr As Integer) As WSMatr
        Dim params As String
        Dim json As New WSMatr

        Return json
    End Function
    'Web service qui récupère le dernier OF/Opé/évt du poste
    Public Function WSGETLSTEV(ByVal _site As String, ByVal _poste As String) As WSLstEvt
        Dim params As String
        Dim json As New WSLstEvt


        Return json
    End Function
    'Web service qui enregistre une entrée d'opérateur
    Public Function WSEntOp(ByVal _site As String, ByVal _poste As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Boolean
        Dim par As Object
        Dim params As String
        Dim ret As Boolean = False


        Return ret
    End Function
    'web service qui enregistre une copie de situation
    Public Function WSCopSit(ByVal _site As String, ByVal _poste As String, ByVal _empdst As Integer, ByVal _empori As Integer, ByRef _retmsg As String) As Boolean
        Dim par As Object
        Dim params As String
        Dim ret As Boolean = False

        Return ret
    End Function

    'Web service qui enregistre un départ d'opérateur
    Public Function WSSorOp(ByVal _site As String, ByVal _poste As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByRef _retmsg As String) As Boolean
        Dim par As Object
        Dim params As String
        Dim ret As Boolean = False

        Return ret
    End Function

    'Web service qui récupère les infos de l'OF/OP 
    Public Function WSOFOPInfo(ByVal _fcy As String, ByVal _of As String, ByVal _op As Integer, ByVal _typop As String) As WSOFOPInfo
        Dim params As String
        Dim json As New WSOFOPInfo

        Return json
    End Function

    'Web service qui enregistre un début d'opération
    Public Function WSDEBOPE(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
        Dim ret As Integer

        Return ret
    End Function

    'Web service qui enregistre un début de réglage
    Public Function WSDEBRGL(ByVal _site As String, ByVal _poste As String, ByVal _typop As String, ByVal _empnum As Integer, ByVal _evtnum As Integer, ByVal _phase As Integer, ByVal _of As String, ByVal _op As Integer, ByRef _retmsg As String) As Integer
        Dim par As Object
        Dim params As String
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
