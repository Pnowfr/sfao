Imports System.Net
Imports System.Text
Imports V12 = SFAO.X3WebReferenceV12
Public Class CAdxWebServiceXmlCCServiceBasicAuth
    Inherits V12.CAdxWebServiceXmlCCService
    Protected Overrides Function GetWebRequest(ByVal Uri As Uri) As WebRequest
        Dim webRequest As HttpWebRequest = CType(MyBase.GetWebRequest(Uri), HttpWebRequest)
        Dim credentials As NetworkCredential = CType(Me.Credentials, NetworkCredential)
        If credentials IsNot Nothing Then
            Dim authInfo As String = ""

            If credentials.Domain IsNot Nothing AndAlso credentials.Domain.Length > 0 Then
                authInfo = String.Format("{0}\{1}:{2}", credentials.Domain, credentials.UserName, credentials.Password)
            Else
                authInfo = String.Format("{0}:{1}", credentials.UserName, credentials.Password)
            End If

            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo))
            webRequest.Headers("Authorization") = "Basic " & authInfo
        End If

        Return webRequest

    End Function
End Class
