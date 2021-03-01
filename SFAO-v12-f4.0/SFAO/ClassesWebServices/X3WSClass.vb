
Public Class X3WSClass
    Private WSv As String
    'Private WS6 As X3WSV6
    Private WithEvents WS12 As X3WSV12

    Public Sub New(ByVal dossier As String, ByVal version As String)
        WSv = version
        Select Case WSv
            Case "V6"
                'WS6 = New X3WSV6(dossier)
            Case "V12"
                WS12 = New X3WSV12(dossier)
        End Select
    End Sub

    Public Sub Close()
        Select Case WSv
            Case "V6"
                'WS6.Close()
            Case "V12"
                WS12.Close()
        End Select
    End Sub

    Public Function Run(ByVal WebService As String, ByVal Parametres As String, ByRef Result As String, Optional WriteTrace As Boolean = False) As Integer
        Dim ret As Integer
        Select Case WSv
            Case "V6"
                'ret = WS6.Run(WebService, Parametres, Result)
            Case "V12"
                ret = WS12.Run(WebService, Parametres, Result, WriteTrace)
        End Select
        Return ret
    End Function

    Public Sub RunAsync(ByVal WebService As String, ByVal Parametres As String, Optional WriteTrace As Boolean = False)
        Select Case WSv
            Case "V6"
                'WS6.RunAsync(WebService, Parametres, WriteTrace)
            Case "V12"
                WS12.RunAsync(WebService, Parametres, WriteTrace)
        End Select
    End Sub

End Class
