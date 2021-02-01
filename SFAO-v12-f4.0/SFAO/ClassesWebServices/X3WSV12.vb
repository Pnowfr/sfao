﻿Imports System.Net
Imports V12 = SFAO.X3WebReferenceV12
Public Class X3WSV12
    Private WithEvents x3WebSrv As CAdxWebServiceXmlCCServiceBasicAuth
    Private callContext As V12.CAdxCallContext
    Private resultXML As V12.CAdxResultXml
    Private cadxMessage As V12.CAdxMessage
    Private TraceCountErr As Integer    'Variable pour limiter l'enregistrement des traces des erreurs fréquentes 
    Public Sub New(ByVal dossier As String)
        Try
            If SFAO.ParamDos("WEBSERVEURURL", dossier) <> "" Then

                x3WebSrv = New CAdxWebServiceXmlCCServiceBasicAuth
                callContext = New V12.CAdxCallContext
                resultXML = New V12.CAdxResultXml

                x3WebSrv.Url = SFAO.ParamDos("WEBSERVEURURL", dossier)
                Trace("Url web services : " & x3WebSrv.Url)

                callContext.codeLang = SFAO.ParamDos("WEBSERVEURLANG", dossier)
                callContext.poolAlias = SFAO.ParamDos("WEBSERVEURPOOLALIAS", dossier)
                callContext.requestConfig = SFAO.ParamDos("WEBSERVEURPARAM", dossier)

                Debug.WriteLine(Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " WEBSERVEURUSER " & SFAO.ParamDos("WEBSERVEURUSER", dossier))
                Debug.WriteLine(Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " WEBSERVEURPWD " & SFAO.ParamDos("WEBSERVEURPWD", dossier)) '& Crypt.Decrypt(SFAO.ParamDos("WEBSERVEURPWD", dossier)))

                x3WebSrv.Credentials = New NetworkCredential(SFAO.ParamDos("WEBSERVEURUSER", dossier), Crypt.Decrypt(SFAO.ParamDos("WEBSERVEURPWD", dossier)))
                x3WebSrv.PreAuthenticate = True
                x3WebSrv.Timeout = CInt(SFAO.ParamDos("WEBSERVEURTIMEOUT", dossier))
            End If
        Catch ex As Exception
            Trace("Erreur : impossible d'initialiser les web services pour le dossier " & dossier & "!", FichierTrace.niveau.erreur)
            Trace(ex.Message, FichierTrace.niveau.erreur)
        End Try
    End Sub

    'Methode qui ferme le web service
    Public Sub Close()
        x3WebSrv.Dispose()
    End Sub

    Public Function Run(ByVal WebService As String, ByVal Parametres As String, ByRef Result As String) As Integer

        'TODO à enlever :
        Debug.WriteLine(Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " " & WebService & " " & Parametres)
        Debug.WriteLine(Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " callContext " & callContext.ToString)

        Try
            resultXML = x3WebSrv.run(callContext, WebService, Parametres)
        Catch ex As Exception
            'Exception
            TraceCountErr += 1
            If TraceCountErr < 10 Then
                Trace("[" & WebService & "] Exception run du web service !", FichierTrace.niveau.erreur)
                Trace(ex.Message, FichierTrace.niveau.erreur)
            End If
            Return -1 'Exception

        End Try

        If resultXML.status = 0 Then
            'Erreur
            TraceCountErr += 1
            If TraceCountErr < 10 Then
                Trace("[" & WebService & "] Erreur statut du web service !", FichierTrace.niveau.erreur)
                For Each cadxMessage In resultXML.messages
                    Trace(cadxMessage.message, FichierTrace.niveau.erreur)
                Next
            End If
            Return 0 'erreur
        Else
            Result = resultXML.resultXml
            TraceCountErr = 0
            Return resultXML.status
        End If
    End Function
End Class
