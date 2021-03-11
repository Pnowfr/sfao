'Imports System.Net
'Imports V12 = SFAO.X3WebReferenceV12
'Imports Newtonsoft.Json
'Imports Newtonsoft.Json.Linq
'Partial Public Class X3WebServices
'    Public Function WSQUALANS(ByVal _codfq As String) As WSFqQst
'        Dim params As String
'        Dim json As New WSFqQst
'        Dim settings As JsonSerializerSettings

'        settings = New JsonSerializerSettings() With {
'            .NullValueHandling = NullValueHandling.Ignore,
'            .MissingMemberHandling = MissingMemberHandling.Ignore
'        }

'        json.GRP1.ZQCRD = _codfq

'        params = ClassToJson(Of WSFqQst)(json, False, settings)
'        Trace("[WSQUALANS] paramètres : " & params)
'        resultXML = x3WebSrv.run(callContext, "ZQUALANS", params)
'        If resultXML.status = 0 Then
'            'Erreur
'            Trace("[WSQUALANS] erreur du web service ! ", FichierTrace.niveau.erreur)
'            For Each cadxMessage In resultXML.messages
'                Trace("[WSQUALANS] erreur : " & cadxMessage.message, FichierTrace.niveau.erreur)
'                json.GRP1.ZMSG += cadxMessage.message
'            Next
'        Else
'            Trace("[WSQUALANS] résultat : " & resultXML.resultXml)
'            json = JsonToClass(Of WSFqQst)(resultXML.resultXml, settings)
'        End If

'        Return json
'    End Function
'End Class

'Public Class WSFqQst
'    Public Property GRP1 As WSFqQstGRP1
'    Public Property GRP2 As List(Of WSFqQstGRP2)
'    Sub New()
'        GRP1 = New WSFqQstGRP1
'        GRP2 = New List(Of WSFqQstGRP2)
'    End Sub
'End Class
'Public Class WSFqQstGRP1
'    Public Property ZQCRD As String
'    Public Property ZRET As Integer
'    Public Property ZMSG As String
'    Sub New()
'        ZQCRD = String.Empty
'        ZMSG = String.Empty
'        ZRET = 0
'    End Sub
'End Class
'Public Class WSFqQstGRP2
'    Public Enum TypeRep
'        Null                    '0
'        typeAlphanumerique      '1
'        typeNumérique           '2
'        typeDate                '3
'        typeBoolean             '4
'        typeTexte               '5
'        typeBlob                '6
'        typeClob                '7
'    End Enum
'    Public Property ZRETQST As String
'    Public Property ZQSTXT As String
'    Public Property ZQSTXT2 As String
'    Public Property ZQSTYPE As TypeRep

'    Sub New()
'        ZRETQST = String.Empty
'        ZQSTXT = String.Empty
'        ZQSTXT2 = String.Empty
'        ZQSTYPE = TypeRep.Null
'    End Sub
'End Class
