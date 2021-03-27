'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'
'------------------------------------------------------------------------------------------------------------------------
Imports System.Text
Module Fnc
    'fonction qui retire tous les caractères spéciaux d'une chaine
    Public Function RemoveAccents(ByVal s As String) As String
        Dim normalizedString As String = s.Normalize(NormalizationForm.FormD)
        Dim stringBuilder As New StringBuilder()
        Dim i As Integer
        Dim c As Char
        For i = 0 To normalizedString.Length - 1
            c = normalizedString(i)
            If System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) <> System.Globalization.UnicodeCategory.NonSpacingMark Then
                stringBuilder.Append(c)
            End If
        Next
        Return stringBuilder.ToString
    End Function

    'Fonction qui affiche la date et l'heure selon le format SFAO
    Public Function AffDateHeure(ByVal _date As Date, ByVal _time As Integer) As String
        If _date = CDate("1/1/0001") OrElse _date = CDate("31/12/1599") Then 'date nulle dans X3 =  15991231
            Return String.Empty
        ElseIf _date <> Now.Date Then
            Return _date.ToString("dd/MM") + " " + ZHM(_time)
        Else
            Return ZHM(_time)
        End If
    End Function
    'Fonction qui convertie une durée en secondes en affichage HH:MM 
    Public Function ZHM(ByVal _time As Integer) As String
        Dim H As Integer
        Dim M As Integer
        H = CInt(Math.Floor(_time / 3600))
        M = CInt(Math.Floor((_time - (H * 3600)) / 60))
        Return H.ToString("00") + ":" + M.ToString("00")
    End Function

    'Fonction qui calcule la durée la date dates + time (time en secondes) et le datetime donnée (now) dans l'unité demandée
    Public Function Calc_Duree(ZDD As Date, ZTD As Integer, ZDF As DateTime, ZUNITE As Integer) As Decimal
        '                                                       ZUNITE       #1=heures, 2 = minutes, 3 = secondes
        Dim RETOUR As Decimal
        Dim ZNBJ, ZNBJS, ZDIFS, ZTF As Integer

        If Year(ZDD) > 1900 And Year(ZDF) > 1900 Then
            ZNBJ = CInt(DateDiff(DateInterval.Day, ZDD, ZDF))
            If ZNBJ < 99999 Then
                ZNBJS = ZNBJ * 86400
                ZTF = (ZDF.Hour * 3600) + (ZDF.Minute * 60) + ZDF.Second
                ZDIFS = ZTF - ZTD
                RETOUR = ZNBJS + ZDIFS
                Select Case ZUNITE
                    Case 1 '1=heures
                        RETOUR = Math.Round(RETOUR / 3600, 2)                       'On arrondi les heures à 0.01 soit à 36 secondes près
                        If RETOUR >= 10000 Then RETOUR = CDec(9999.99)              'maxi autorisé dans X3 (9999.9999)

                    Case 2 '2=minutes
                        RETOUR = Math.Round(RETOUR / 60, 1)                         'On arrondi les minutes à 0.1 soit à 6 secondes près
                        If RETOUR >= 10000 Then RETOUR = CDec(9999.9)               'maxi autorisé dans X3 (9999.9999)
                End Select
            Else
                RETOUR = CDec(9999.99)
            End If
        Else
            RETOUR = 0
        End If

        Return RETOUR
    End Function

    'Affichage d'une durée en secondes sous le format J:HH ou HH:MM
    Public Function AFF_TPS_JHM(ZTPS As Decimal) As String
        Dim RETOUR As String
        Dim NBMN, NBHR, NBJR As Integer

        NBJR = CInt(Math.Floor(ZTPS / 1440))
        NBHR = CInt(Math.Floor((ZTPS / 60) - (NBJR * 24)))
        NBMN = CInt(Math.Floor(ZTPS - NBHR * 60 - NBJR * 1440))

        If NBJR > 0 Then
            RETOUR = NBJR.ToString + "j " + NBHR.ToString + "h"
        Else
            RETOUR = NBHR.ToString + "h " + NBMN.ToString("00")
        End If

        Return RETOUR
    End Function

    'Fonction pour convertir les unités 
    Public Function AFF_UNIT(UNIT As String) As String
        Select Case UNIT
            Case "MLF"
                AFF_UNIT = "ML"
            Case "M2"
                AFF_UNIT = "M²"
            Case Else
                AFF_UNIT = UNIT
        End Select
    End Function

End Module
