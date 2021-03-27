'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'
'------------------------------------------------------------------------------------------------------------------------

'########################################################################################################################
Public Class WSParam
    Public Property GRP1 As New ParamWS
End Class
Public Class ParamWS
    Public Property ZPAR As String
    Public Property ZFCY As String
    Public Property ZUSR As String
    Public Property ZVAL As String

    Public Sub New()
        ZPAR = String.Empty
        ZFCY = String.Empty
        ZUSR = String.Empty
        ZVAL = String.Empty
    End Sub
End Class
'########################################################################################################################
Public Class WSSite
    Public Property GRP1 As New Site
End Class
Public Class Site
    Public Property FCY As String
    Public Property FCYSHO As String
    Public Property FCYNAM As String
    Public Property DOSSIER As String
    Public Property SOCIETE As String
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Public Sub New()
        FCY = String.Empty
        FCYSHO = String.Empty
        FCYNAM = String.Empty
        DOSSIER = String.Empty
        SOCIETE = String.Empty
        ZRET = 0
        ZMSG = String.Empty
    End Sub
End Class
'########################################################################################################################
Public Class WSPoste
    Public Property GRP1 As New PosteWs
End Class

Public Class PosteWs
    Public Enum TypePost
        Null
        Machine
        Main_oeuvre
        Sous_traitance
    End Enum

    Public Property WST As String
    Public Property WCRFCY As String
    Public Property WSTDES As String
    Public Property Y_TYPOP As String
    Public Property STOLOC As String
    Public Property ZATELOC As String
    Public Property WSTTYP As TypePost
    Public Property ZENAFLG As Integer
    Public Property ZGRP As Integer
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Public Sub New()
        WST = String.Empty
        WCRFCY = String.Empty
        WSTDES = String.Empty
        Y_TYPOP = String.Empty
        STOLOC = String.Empty
        ZATELOC = String.Empty
        WSTTYP = TypePost.Null
        ZENAFLG = 0
        ZGRP = 0
        ZRET = 0
        ZMSG = String.Empty
    End Sub

End Class
'########################################################################################################################
Public Class WSEvtPst
    Public Property WST As String
    Public Sub New()
        WST = String.Empty
    End Sub
End Class
'########################################################################################################################
Public Class WSEvt
    Public Property GRP1 As WSEvtGRP1
    Public Property GRP2 As List(Of WSEvtGRP2)
    Sub New()
        GRP1 = New WSEvtGRP1
        GRP2 = New List(Of WSEvtGRP2)
    End Sub
End Class
Public Class WSEvtGRP1
    Public Property WST As String
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Sub New()
        WST = String.Empty
        ZMSG = String.Empty
        ZRET = 0
    End Sub
End Class
Public Class WSEvtGRP2
    Public Enum TypeMsg
        Null                    '0
        Evenement               '1
        Phase                   '2
        Motif_de_non_solde      '3
        Interruption            '4
        Interruption_MA         '5
        Interruption_MO         '6
        Interruption_MAMO       '7
        Divers                  '8
        Opération_Hors_Of       '9
        Messages_Ope_Hors_Of    '10
        Saisie_manuelle         '11
        Action                  '12
    End Enum
    Public Property MSGNUM As Integer
    Public Property DESLNG As String
    Public Property DESSHO As String
    Public Property XPHASE As Integer
    Public Property ZPHSDESL As String
    Public Property ZPHSDESC As String
    Public Property XSFAOKEY As String
    Public Property XORDRE As Integer
    Public Property ZTYPEMSG As TypeMsg
    Public Property ZTYPEMSG_LBL As String 'menu local 5201
    Sub New()
        MSGNUM = 0
        DESLNG = String.Empty
        DESSHO = String.Empty
        XPHASE = 0
        XSFAOKEY = String.Empty
        ZPHSDESL = String.Empty
        ZPHSDESC = String.Empty
        XORDRE = 0
        ZTYPEMSG = TypeMsg.Null
        ZTYPEMSG_LBL = String.Empty
    End Sub
End Class

'########################################################################################################################

Public Class WSSitPs
    Public Property GRP1 As WSSitPsGRP1
    Public Property GRP2 As List(Of WSSitPsGRP2)
    Sub New()
        GRP1 = New WSSitPsGRP1
        GRP2 = New List(Of WSSitPsGRP2)
    End Sub
End Class
Public Class WSSitPsGRP1
    Public Property WST As String
    Public Property FCY As String
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Sub New()
        WST = String.Empty
        FCY = String.Empty
        ZMSG = String.Empty
        ZRET = 0
    End Sub
End Class
Public Class WSSitPsGRP2
    Public Enum TypeMat
        Null           '0
        Opérateur      '1
        Technicien     '2
        Coloriste      '3
        Chef_equipe    '4
        Autre          '5
    End Enum
    Public Property XEMPNUM As Integer
    Public Property ZEMPDES As String
    Public Property XTYPE As TypeMat
    Public Property XTYPE_LBL As String
    Public Property XEVENT As Integer
    Public Property XPHASE As Integer
    Public Property ZEVTDES As String
    Public Property XDATEVT As Date
    Public Property XTIMEVT As Integer
    Public Property XMFGNUM As String
    Public Property XOPENUM As Integer
    Public Property ZROODES As String
    Public Property XBPCNUM As String
    Public Property XBPCNAM As String
    Public Property XDATENT As Date
    Public Property XTIMENT As Integer
    Public Property ZOPEXC As Integer
    Public Property ZTYPOPHOF As Integer

    Sub New()
        XEMPNUM = 0
        ZEMPDES = String.Empty
        XTYPE = TypeMat.Null
        XTYPE_LBL = String.Empty
        XEVENT = 0
        XPHASE = 0
        ZEVTDES = String.Empty
        XDATEVT = Nothing
        XTIMEVT = 0
        XMFGNUM = String.Empty
        XOPENUM = 0
        ZROODES = String.Empty
        XBPCNUM = String.Empty
        XBPCNAM = String.Empty
        XDATENT = Nothing
        XTIMENT = 0
        ZOPEXC = 0
        ZTYPOPHOF = 0
    End Sub
End Class

'########################################################################################################################

Public Class WSSitOF
    Public Property GRP1 As WSSitOFGRP1
    Public Property GRP2 As List(Of WSSitOFGRP2)
    Sub New()
        GRP1 = New WSSitOFGRP1
        GRP2 = New List(Of WSSitOFGRP2)
    End Sub
End Class
Public Class WSSitOFGRP1
    Public Property WST As String
    Public Property FCY As String
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Sub New()
        WST = String.Empty
        FCY = String.Empty
        ZMSG = String.Empty
        ZRET = 0
    End Sub
End Class
Public Class WSSitOFGRP2
    Public Property XMFGNUM As String
    Public Property XOPENUM As Integer
    Public Property ZLIGITM As Integer
    Public Property ZITMREF As String
    Public Property ZITMDES As String
    Public Property ZMFGMST As Integer
    Public Property XDATEVT As Date
    Public Property XTIMEVT As Integer
    Public Property ZCPLTIM As Decimal
    Public Property EXTQTY As Decimal
    Public Property UOM As String
    Public Property ZQTYA As Decimal
    Public Property ZQTYR As Decimal
    Public Property ZLSTPAL As Integer
    Sub New()
        XMFGNUM = String.Empty
        XOPENUM = 0
        ZLIGITM = 0
        ZITMREF = String.Empty
        ZITMDES = String.Empty
        ZMFGMST = 0
        XDATEVT = Nothing
        XTIMEVT = 0
        ZCPLTIM = 0
        EXTQTY = 0
        ZQTYA = 0
        ZQTYR = 0
        ZLSTPAL = 0
    End Sub
End Class

'########################################################################################################################

Public Class WSSitCP
    Public Property GRP1 As WSSitCPGRP1
    Public Property GRP2 As List(Of WSSitCPGRP2)
    Sub New()
        GRP1 = New WSSitCPGRP1
        GRP2 = New List(Of WSSitCPGRP2)
    End Sub
End Class
Public Class WSSitCPGRP1
    Public Property WST As String
    Public Property FCY As String
    Public Property Y_TYPOP As String
    Public Property LOCMAC As String
    Public Property LOCATE As String
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Sub New()
        WST = String.Empty
        FCY = String.Empty
        Y_TYPOP = String.Empty
        LOCMAC = String.Empty
        LOCATE = String.Empty
        ZMSG = String.Empty
        ZRET = 0
    End Sub
End Class
Public Class WSSitCPGRP2
    Public Property XMFGNUM As String
    Public Property XOPENUM As Integer
    Public Property ZITMREF As String
    Public Property ZITMDES As String
    Public Property ZSUPGRP As String
    Public Property ZUOM As String
    Public Property RETQTY As Decimal
    Public Property USEQTY As Decimal
    Public Property STOMAC As Decimal
    Public Property STOATE As Decimal
    Public Property USRFLD2 As String
    Sub New()
        XMFGNUM = String.Empty
        XOPENUM = 0
        ZITMREF = String.Empty
        ZITMDES = String.Empty
        ZSUPGRP = String.Empty
        ZUOM = String.Empty
        RETQTY = 0
        USEQTY = 0
        STOMAC = 0
        STOATE = 0
        USRFLD2 = String.Empty
    End Sub
End Class

'########################################################################################################################
Public Class WSLstEvt
    Public Property GRP1 As New LstEvtWs
End Class
Public Class LstEvtWs
    Public Property WST As String
    Public Property FCY As String
    Public Property ZLSTOF As String
    Public Property ZLSTOPE As Integer
    Public Property ZLSTEVT As Integer
    Public Property ZLSTEVD As String
    Public Property ZLSTPHASE As Integer
    Public Property ZLSTPHASD As String
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Public Sub New()
        WST = String.Empty
        FCY = String.Empty
        ZLSTOF = String.Empty
        ZLSTOPE = 0
        ZLSTEVT = 0
        ZLSTEVD = String.Empty
        ZLSTPHASE = 0
        ZLSTPHASD = String.Empty
        ZRET = 0
        ZMSG = String.Empty
    End Sub
End Class
'########################################################################################################################
Public Class WSMatr
    Public Property GRP1 As New Matr
End Class
Public Class Matr
    Public Property ZEMPNUM As Integer
    Public Property ZEMPDES As String
    Public Property ZEMPSHO As String
    Public Property ZENAFLG As Integer
    Public Property ZFCY As String
    Public Property ZTYPE As Integer
    Public Property ZTYPE_LBL As String
    Public Property ZPASSE As String
    Public Property ZPOSTE As String
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Public Sub New()
        ZEMPDES = String.Empty
        ZEMPSHO = String.Empty
        ZENAFLG = 0
        ZFCY = String.Empty
        ZTYPE = 0
        ZTYPE_LBL = String.Empty
        ZPASSE = String.Empty
        ZPOSTE = String.Empty
        ZRET = 0
        ZMSG = String.Empty
    End Sub
End Class
'########################################################################################################################
Public Class WSOFOPInfo
    Public Property GRP1 As New OFOPInfo
End Class
Public Class OFOPInfo
    Public Property ZFCY As String
    Public Property ZMFGNUM As String
    Public Property ZOPENUM As Integer
    Public Property ZTYPOP As String
    Public Property ZBPCNUM As String
    Public Property ZBPCNAM As String
    Public Property ZITMREF As String
    Public Property ZITMDES As String
    Public Property ZRET As Integer
    Public Property ZMSG As String
    Public Sub New()
        ZFCY = String.Empty
        ZMFGNUM = String.Empty
        ZOPENUM = 0
        ZTYPOP = String.Empty
        ZBPCNUM = String.Empty
        ZBPCNAM = String.Empty
        ZITMREF = String.Empty
        ZITMDES = String.Empty
        ZRET = 0
        ZMSG = String.Empty
    End Sub
End Class

'########################################################################################################################