'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[020421PNO] : Tester existence du timer upd
'------------------------------------------------------------------------------------------------------------------------
Module Evenements
    'Fonction qui déclenche l'affichage d'un événement, par défaut c'est une événement de début. pour un événement de fin on peut indiquer l'événement appelant
    Public Function AfficheEvt(ByVal NumEvt As Integer, Optional ByVal DebFin As String = "D", Optional ByVal EvtEnCours As Integer = 0) As DialogResult
        AfficheEvt = DialogResult.None
        Dim MsgErr As String = String.Empty

        If UpdSFAO = True Then '020421PNO.n
            SFAO.UpdateTimer.Stop() 'on met en pose la vérification de mise à jour pendant l'execution des événements
        End If '020421PNO.n

        Select Case NumEvt
            Case 1000 'Arrivée opérateur
                AfficheEvt = LanceEvt(NumEvt, CType(ENTOP, Form))
            Case 1100 'départ opérateur
                SOROP.SetWstSor = SFAO.Poste.GRP1.WST                'spécifie le poste sur le quel se fera la sortie
                AfficheEvt = LanceEvt(NumEvt, CType(SOROP, Form))
            Case 1010 'Début opération 
                If DebFin = "D" Then    'début événement simple 
                    'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
                    FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                    If MsgErr = "" Then
                        'Contrôle s'il y a au moins un opérateur présent
                        If NbrMatr(True) > 0 Then
                            AfficheEvt = LanceEvt(NumEvt, CType(DEBOP, Form))
                        End If
                    End If
                Else
                    'on ne déclenche pas la fin d'opération automatiquement 
                    'seul de départ opérateur permet de déclencher une 'fin d'opération'
                    If EvtEnCours = 1100 Then 'départ opérateur
                        'TODO affichage d'une fenêtre temporaire pour déclencher la fin d'opération en cours
                    End If
                    AfficheEvt = DialogResult.OK
                End If
            Case 1020 'Matières utilisées
                'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
                FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                If MsgErr = "" Then
                    'Contrôle s'il y a au moins un opérateur présent et une opération en cours
                    If NbrMatr(True) > 0 AndAlso NbrOpe(True) > 0 Then
                        AfficheEvt = LanceEvt(NumEvt, CType(MATUTL, Form))
                    End If
                End If

            Case 1030 'Réglages
                If DebFin = "D" Then    'début événement simple
                    'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
                    FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                    If MsgErr = "" Then
                        'Contrôle s'il y a au moins un opérateur présent et une opération en cours
                        If NbrMatr(True) > 0 AndAlso NbrOpe(True) > 0 Then
                            AfficheEvt = LanceEvt(NumEvt, CType(DEBRGL, Form))
                        End If
                    End If
                Else
                    'TODO voir comment on déclanche la fin d'un réglage ????

                End If

            Case 1040 'Production
                If DebFin = "D" Then    'début événement simple
                    'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
                    FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                    If MsgErr = "" Then
                        'Contrôle s'il y a au moins un opérateur présent et une opération en cours
                        If NbrMatr(True) > 0 AndAlso NbrOpe(True) > 0 Then
                            AfficheEvt = LanceEvt(NumEvt, CType(DEBPRO, Form))
                        End If
                    End If
                Else
                    'TODO voir comment on déclanche la fin de production ????

                End If

            Case 1050 'Test déclaration
                'MsgBox("Test ZPL")
                'MsgBox("Test ZPL 2 ")
                'Dim labelFileName As String
                'labelFileName = "test.zpl"
                'Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & "début impression fichier ZPL")
                'Dim filename As String = System.IO.Path.GetFileName("test.zpl")
                'System.IO.File.Copy(
                '        labelFileName,
                '        "\\192.168.0.154\PRBPNOZ4X2\" & filename)
                'Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & "fin impression fichier ZPL")

                ''ZPL.SendStringToPrinter("\\192.168.0.154\PRBPNOZ4X2", "^XA^LH30, 30 \ n ^ FO20, 10 ^ ADN, 90, 50 ^ AD ^ FDTEST ZPL^FS\n^XZ")
                'Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & "début impression code ZPL")
                'ZPL.SendStringToPrinter("\\192.168.0.154\PRBPNOZ4X2", "^XA~TA0~JSO^LT0^MMT^MNW^MTT^PON^PMN^LH0,0^JMA^PR4,4^MD0^JUS^LRN^CI0^XZ
                '                        ^XA^LL0406
                '                        ^PW812
                '                        ^FO,24^FS
                '                        ^FT174,29^A0N,18,18^FH\^FDBRODART SAS 10700 ARCIS SUR AUBE TEL: 03.25.37.13.00^FS
                '                        ^FO35,35^GB760,0,2^FS
                '                        ^FO35,62^GB760,0,2^FS
                '                        ^FO35,35^GB0,28,2^FS
                '                        ^FO795,35^GB0,28,2^FS
                '                        ^FO,28^FS
                '                        ^FT320,59^A0N,24,24^FH\^FDFromagerie Toto^FS
                '                        ^FO,24^FS
                '                        ^FT35,85^A0N,18,18^FH\^FDR\82f cde : TEST R\82F \FD&\82'(^FS
                '                        ^BY2,3,81^FT39,346^BCN,,Y,N
                '                        ^FD>:TEST-PNO-128->51234^FS
                '                        ^PQ1,0,1,Y^XZ")
                'Debug.WriteLine("Debug " & Date.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") & "fin impression code ZPL")

            Case 1070 'Démontage 
                If DebFin = "D" Then    'début événement simple
                    'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
                    FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                    If MsgErr = "" Then
                        'Contrôle s'il y a au moins un opérateur présent et une opération en cours
                        If NbrMatr(True) > 0 AndAlso NbrOpe(True) > 0 Then
                            AfficheEvt = LanceEvt(NumEvt, CType(DEBDEM, Form))
                        End If
                    End If
                Else
                    'TODO voir comment on déclanche la fin d'une interruption ????

                End If

            Case 1080 'Interruption 
                If DebFin = "D" Then    'début événement simple
                    'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
                    FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                    If MsgErr = "" Then
                        'Contrôle s'il y a au moins un opérateur présent
                        '(opération en cours non obligatoire pour une interruption)
                        If NbrMatr(True) > 0 Then
                            AfficheEvt = LanceEvt(NumEvt, CType(DEBINT, Form))
                        End If
                    End If
                Else
                    'TODO voir comment on déclanche la fin d'une interruption ????

                End If

            Case 1090 'Fin opération
                'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
                FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                If MsgErr = "" Then
                    'Contrôle s'il y a au moins un opérateur présent et une opération en cours
                    If NbrMatr(True) > 0 AndAlso NbrOpe(True) > 0 Then
                        AfficheEvt = LanceEvt(NumEvt, CType(FINOP, Form))
                    End If
                End If

            Case 1220 'Retour Matières
                'on doit vérifier si un des opérateurs présents sur ce poste a dépasse le temps de présence autorisé
                FenSfao.DureeMaxPresenceDepassee(MsgErr, True)
                If MsgErr = "" Then
                    'Contrôle s'il y a au moins un opérateur présent et une opération en cours
                    If NbrMatr(True) > 0 AndAlso NbrOpe(True) > 0 Then
                        AfficheEvt = LanceEvt(NumEvt, CType(RETMAT, Form))
                    End If
                End If

            Case 1283
                AfficheEvt = LanceEvt(NumEvt, CType(AUTOCTRL, Form))
            Case Else
                Trace("[AfficheEvt] événement n° " & NumEvt.ToString & " " & DebFin & " non réalisé !", FichierTrace.niveau.erreur)
                AfficheEvt = DialogResult.None
        End Select

        If UpdSFAO = True Then '020421PNO.n
            SFAO.UpdateTimer.Start() 'on réactive la vérification de mise à jour  
        End If '020421PNO.n
    End Function

    Public Function LanceEvt(ByVal _numEvt As Integer, ByRef _FenEvt As Form) As DialogResult
        Dim result As DialogResult
        Trace("Affichage " & _FenEvt.Text & "(" & _FenEvt.Name & ")" & " événement n° " & _numEvt.ToString)
        FenSfao.Etat("Evénement " & _FenEvt.Text, 1, 3000)
        _FenEvt.Tag = _numEvt
        result = _FenEvt.ShowDialog()
        _FenEvt.Dispose() 'efface l'ecran de la mémoire 
        Return result
    End Function

    Private Function NbrMatr(Optional ByVal _msg As Boolean = False) As Integer
        Dim i As Integer
        NbrMatr = 0
        'on parcours la classe de la situation poste
        For i = 0 To FenSfao.WSsp.GRP2.Count - 1
            If FenSfao.WSsp.GRP2(i).XEMPNUM > 0 Then
                NbrMatr += 1
            End If
        Next
        If NbrMatr = 0 And _msg Then
            Trace("Aucun opérateur présent !", FichierTrace.niveau.avertissement)
        End If
    End Function

    Private Function NbrOpe(Optional ByVal _msg As Boolean = False) As Integer
        Dim i As Integer
        NbrOpe = 0
        'on parcours la classe de la situation opérations
        For i = 0 To FenSfao.WSof.GRP2.Count - 1
            If FenSfao.WSof.GRP2(i).XOPENUM > 0 Then
                NbrOpe += 1
            End If
        Next
        If NbrOpe = 0 And _msg Then
            Trace("Aucune opération en cours !", FichierTrace.niveau.avertissement)
        End If
    End Function
End Module
