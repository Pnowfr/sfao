Imports System.Net
Imports V6 = SFAO.X3WebReferenceV6
Public Class X3WSV6
    Private WithEvents x3WebSrv As V6.CAdxWebServiceXmlCCService
    Private callContext As V6.CAdxCallContext
    Private resultXML As V6.CAdxResultXml
    Private cadxMessage As V6.CAdxMessage
    Private TraceCountErr As Integer    'Variable pour limiter l'enregistrement des traces des erreurs fréquentes 
    Public Sub New(ByVal dossier As String)
        Try
            If SFAO.ParamDos("WEBSERVEURURL", dossier) <> "" Then

                x3WebSrv = New V6.CAdxWebServiceXmlCCService
                callContext = New V6.CAdxCallContext
                resultXML = New V6.CAdxResultXml

                x3WebSrv.Url = SFAO.ParamDos("WEBSERVEURURL", dossier)
                Trace("Url web services : " & x3WebSrv.Url)

                callContext.codeLang = SFAO.ParamDos("WEBSERVEURLANG", dossier)
                callContext.poolAlias = SFAO.ParamDos("WEBSERVEURPOOLALIAS", dossier)
                callContext.requestConfig = SFAO.ParamDos("WEBSERVEURPARAM", dossier)

                Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " WEBSERVEURUSER " & SFAO.ParamDos("WEBSERVEURUSER", dossier))
                Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " WEBSERVEURPWD " & SFAO.ParamDos("WEBSERVEURPWD", dossier)) '& Crypt.Decrypt(SFAO.ParamDos("WEBSERVEURPWD", dossier)))

                callContext.codeUser = SFAO.ParamDos("WEBSERVEURUSER", dossier)
                callContext.password = Crypt.Decrypt(SFAO.ParamDos("WEBSERVEURPWD", dossier))
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

    Public Function Run(ByVal WebService As String, ByVal Parametres As String, ByRef Result As String, Optional WriteTrace As Boolean = False) As Integer

        If WriteTrace Then
            Trace("[" & WebService & "] Parametres : " & Parametres)
        End If

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
            TraceCountErr = 0
            Result = resultXML.resultXml
            If WriteTrace Then
                Trace("[" & WebService & "] Resultat : " & Result)
            End If
            Return resultXML.status
        End If
    End Function


    Public Sub RunAsync(ByVal WebService As String, ByVal Parametres As String, Optional WriteTrace As Boolean = False)
        Dim UserState As Object
        UserState = WebService

        If WriteTrace Then
            Trace("[" & WebService & "] Parametres : " & Parametres)
        End If

        Try
            WsNbrAsyncInvoke += 1 'on incrémente le nombre d'appel asynchrones en attente
            x3WebSrv.runAsync(callContext, WebService, Parametres, UserState)
        Catch ex As Exception
            'Exception
            TraceCountErr += 1
            If TraceCountErr < 10 Then
                Trace("[" & WebService & "] Exception d'appel runAsync du web service !", FichierTrace.niveau.erreur)
                Trace(ex.Message, FichierTrace.niveau.erreur)
            End If
            Exit Sub
        End Try

    End Sub

    Public Sub RunAsyncCompleted(sender As Object, e As V6.runCompletedEventArgs) Handles x3WebSrv.runCompleted
        Dim result As String = ""
        Dim ParamRet As String()

        WsNbrAsyncInvoke -= 1 'on décrémente le nmbre d'appels en attente

        'Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & " RunAsyncCompleted ")

        If e.Error Is Nothing Then 'si pas d'erreur
            resultXML = e.Result    'on récupère le résultat 
            If resultXML.status = 0 Then 'on vérifie le statut
                'Statut en erreur (côté X3) 
                TraceCountErr += 1
                If TraceCountErr < 10 Then
                    Trace("[" & e.UserState.ToString & "_ASYNC] Erreur statut du web service !", FichierTrace.niveau.erreur)
                    For Each cadxMessage In resultXML.messages
                        Trace(cadxMessage.message, FichierTrace.niveau.erreur)
                    Next
                End If
            Else
                TraceCountErr = 0
                result = resultXML.resultXml
            End If
        Else 'on est en erreur (côté appel web service) 
            TraceCountErr += 1
            If TraceCountErr < 10 Then
                Trace("[" & e.UserState.ToString & "_ASYNC] Erreur d'appel du web service !", FichierTrace.niveau.erreur)
                Trace(e.Error.Message, FichierTrace.niveau.erreur)
            End If
            x3WebSrv.CancelAsync(e.UserState)
        End If
        ParamRet = {e.UserState.ToString, result}
        CallByName(X3ws.X3WSCA, "RunAsyncCompleted", Microsoft.VisualBasic.CallType.Method, ParamRet)

    End Sub

End Class
