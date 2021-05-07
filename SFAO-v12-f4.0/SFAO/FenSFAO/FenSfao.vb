'------------------------------------------------------------------------------------------------------------------------
'Modifications:
'[280321PNO] : Rafraichissement situation par double-clique sur le label
'
'------------------------------------------------------------------------------------------------------------------------
Public Class FenSfao
    'Déclaration des variables pour le mouvement et déplacement de la form
    Private Enum TypeMouv
        Null
        Resiz
        Mouv
    End Enum
    Private MTyp As TypeMouv = TypeMouv.Null
    Private MPosX As Integer
    Private MPosY As Integer

    Private TaillePoliceBtn As Integer
    Private TaillePoliceLbl As Integer
    Private WsTimInt As Integer

    Public WSsp As WSSitPs                  'classe de la situation du poste
    Public WSof As WSSitOF                  'classe de la situation des opérations
    Public WScp As WSSitCP                  'classe de la situation des composants
    Public WSLstMns As New WSMns            'classe de la liste des motifs de non solde
    Public DtSFAO As String

    Private DblClc As Boolean
    Private _maxHeuresPresence As Decimal

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub

    Private Sub FQT_Click(sender As Object, e As EventArgs) Handles FQT.Click, LblFin.Click
        If MsgBox("Voulez-vous quitter la SFAO ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Me.Close()
        End If
    End Sub
    Private Sub FenSfao_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown, DateHeure.MouseDown, TableLayoutPanelTop.MouseDown, TableLayoutPanelHaut.MouseDown, Lab1sit.MouseDown, Lab1dos.MouseDown, Lab1pst.MouseDown, Lab2Sit.MouseDown, Lab2Dos.MouseDown, Lab2Post.MouseDown
        'si on clique avec le bouton gauche est dans le coin en bas à droite on est en redimentionnment si non en mouvement
        If e.Button = Windows.Forms.MouseButtons.Left AndAlso e.X >= (Me.Size.Width - 20) AndAlso e.Y >= (Me.Size.Height - 20) Then
            MTyp = TypeMouv.Resiz
        ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
            MTyp = TypeMouv.Mouv
            MPosX = e.X
            MPosY = e.Y
        End If
    End Sub
    Private Sub FenSfao_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp, DateHeure.MouseUp, TableLayoutPanelTop.MouseUp, TableLayoutPanelHaut.MouseUp, Lab1sit.MouseUp, Lab1dos.MouseUp, Lab1pst.MouseUp, Lab2Sit.MouseUp, Lab2Dos.MouseUp, Lab2Post.MouseUp
        'quand on relache le bouton on désactive les mouvements
        MTyp = TypeMouv.Null
        MPosX = 0
        MPosY = 0
    End Sub
    Private Sub FenSfao_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove, DateHeure.MouseMove, TableLayoutPanelTop.MouseMove, TableLayoutPanelHaut.MouseMove, Lab1sit.MouseMove, Lab1dos.MouseMove, Lab1pst.MouseMove, Lab2Sit.MouseMove, Lab2Dos.MouseMove, Lab2Post.MouseMove
        'si on est en redimentionnement de la form 
        If MTyp = TypeMouv.Resiz Then
            Dim XOffset, YOffset As Integer
            XOffset = e.X - (Me.Size.Width)
            YOffset = e.Y - (Me.Size.Height)
            Try
                Me.Size = New Size(Math.Max(Me.MaximumSize.Width, Me.Size.Width + XOffset), Math.Max(Me.MaximumSize.Height, Me.Size.Height + YOffset))
            Catch ex As Exception
                'Threading.Thread.Sleep(500)
            End Try
            'si on est en mouvement de la form
        ElseIf MTyp = TypeMouv.Mouv Then
            Me.Location = New Point(Me.Location.X + e.X - MPosX, Me.Location.Y + e.Y - MPosY)
        End If
    End Sub

    Private Sub LblRetPrec_Click(sender As Object, e As EventArgs) Handles LblRetPrec.Click
        If Me.WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
            PictureBoxCoin.Visible = True
        Else
            Me.WindowState = FormWindowState.Maximized
            PictureBoxCoin.Visible = False
        End If
    End Sub

    Private Sub LblMin_Click(sender As Object, e As EventArgs) Handles LblMin.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub FenSfao_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim typeTrt As String
        'affichage de l'icône 
        Icon = My.Resources.icon_sfao

        'version du produit
        Version.Text = Application.ProductVersion

        'affichage de la date+heure
        DateHeure.Text = Now.ToString
        With PictureLoad
            .Parent = DataGridViewSitOpe
            .Width = DataGridViewSitOpe.Height
            .Height = DataGridViewSitOpe.Height
        End With
        With PictureLoad
            .Location = New Point(CInt((DataGridViewSitOpe.Width / 2) - (PictureLoad.Width / 2)), CInt((DataGridViewSitOpe.Height / 2) - (PictureLoad.Height / 2)))
            .Visible = True
        End With


        typeTrt = SFAO.Poste.GRP1.Y_TYPOP   'type traitement du poste

        'Affichage des infos utilisateur
        If Not (SfaoTest) Then
            Lab2Soc.Text = SFAO.Site.GRP1.SOCIETE
            Lab2Sit.Text = SFAO.Site.GRP1.FCYSHO
            Lab2Dos.Text = SFAO.Site.GRP1.DOSSIER
            Lab2Post.Text = SFAO.Poste.GRP1.WSTDES
        End If

        'valeur du paramètre d'interval de test du web service
        If SFAO.ParamDos("WEBSERVEURTIMTEST", SFAO.Site.GRP1.DOSSIER) <> "" Then
            WsTimInt = CInt(SFAO.ParamDos("WEBSERVEURTIMTEST", SFAO.Site.GRP1.DOSSIER))
        End If
        If WsTimInt = 0 Then
            WsTimInt = 30000 'valeur 30s par défaut
        End If


        'Initialisation des intitulés de la situation des composants
        Trace("Initialisation des intitulés de la situation des composants")
        Call InitialiseTableauComposants(typeTrt)


        'Initialisation des événements et actions du poste
        Trace("Initialisation des événements et actions du poste")
        Call InitaliseEvenements()

        'Initialisation des listes liées au poste
        Trace("Initialisation des listes du poste")
        Call InitaliseListes()

        'Récupération et affichage de la situation du poste
        Call Situation()

        'Me.Refresh()

        'Redimentionnement de la fenêtre 
        Call Redimentionner()


    End Sub
    Private Sub FenSfao_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        'début du timer qui affiche l'heure
        TimerSec.Enabled = True
        TimerSec.Start()

        'timer qui recalule les temps passés 
        Dim time As Integer
        time = 60 - DateTime.Now.Second
        TimerMin.Interval = time * 1000
        TimerMin.Enabled = True
        TimerMin.Start()

        'timer qui vérifie la connexion au web service
        PictureBoxWS.Image = My.Resources.X3OK
        PictureBoxWS.Tag = "X3OK"
        TimerAnim.Enabled = True
        TimerAnim.Start()
        F01.Select()

    End Sub

    Private Sub FenSfao_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

    End Sub
    Private Sub InitialiseTableauComposants(ByVal typeTrt As String)
        'la situation des composants modifié en situation des palettes pour la palettisation

        If typeTrt = "PAL" Then
            'changement de titre
            Lab1Sitmat.Text = "Palettes validées à fusionner"
            'changement des intitulés colonnes
            DataGridViewSitComp.Columns(1).HeaderText = "Article"
            DataGridViewSitComp.Columns(2).HeaderText = ""
            DataGridViewSitComp.Columns(2).Visible = False
            DataGridViewSitComp.Columns(3).HeaderText = "N° Palette"
            DataGridViewSitComp.Columns(4).HeaderText = "Qté. totale"
            DataGridViewSitComp.Columns(5).HeaderText = "Unité"
            DataGridViewSitComp.Columns(6).HeaderText = "Nbr. UC"
            DataGridViewSitComp.Columns(7).HeaderText = "Empl."
            DataGridViewSitComp.Columns(8).HeaderText = "Utilisation"
        Else
            'changement de titre
            Lab1Sitmat.Text = "Situation des composants"
            'changement des intitulés colonnes
            DataGridViewSitComp.Columns(1).HeaderText = "Composant"
            DataGridViewSitComp.Columns(2).HeaderText = "N°"
            DataGridViewSitComp.Columns(2).Visible = True
            DataGridViewSitComp.Columns(3).HeaderText = "Désignation"
            DataGridViewSitComp.Columns(4).HeaderText = "Qté. besoin"
            DataGridViewSitComp.Columns(5).HeaderText = "Unité"
            DataGridViewSitComp.Columns(6).HeaderText = "Qté. conso."
            DataGridViewSitComp.Columns(7).HeaderText = "Stock mac."
            DataGridViewSitComp.Columns(8).HeaderText = "Stock ate."
        End If
    End Sub
    Private Sub InitaliseEvenements()
        Dim f As Integer
        Dim e As Integer
        Dim a As Integer
        Dim zok As Boolean

        'Affichage des évenements (boutons F1 à F12)
        'If ws Then
        For f = 1 To 12
            zok = False
            For e = 0 To SFAO.Events.GRP2.Count - 1
                'on associe chaque évt de type événenent en fonction de son raccourci au bouton correspondant
                If SFAO.Events.GRP2(e).ZTYPEMSG = WSEvtGRP2.TypeMsg.Evenement AndAlso Strings.Left(SFAO.Events.GRP2(e).XSFAOKEY, 1) = "F" AndAlso CInt(Strings.Mid(SFAO.Events.GRP2(e).XSFAOKEY, 2)) = f Then
                    ChgBtn("F", f, SFAO.Events.GRP2(e).DESLNG, SFAO.Events.GRP2(e).XSFAOKEY, SFAO.Events.GRP2(e).MSGNUM.ToString)
                    zok = True
                    Exit For
                End If
            Next e
            If Not (zok) Then
                'si aucun événement trouvé pour le bouton on le désactive
                ChgBtn("F", f, "", "", "")
            End If
        Next f

        'Actions
        For a = 1 To 20 'On peut avoir jusqu'à 20 boutons action
            zok = False
            For e = 0 To SFAO.Events.GRP2.Count - 1
                'on associe chaque action den fonction de son ordre au bouton correspondant
                If SFAO.Events.GRP2(e).ZTYPEMSG = WSEvtGRP2.TypeMsg.Action AndAlso SFAO.Events.GRP2(e).XORDRE = a Then
                    ChgBtn("A", a, SFAO.Events.GRP2(e).DESLNG, SFAO.Events.GRP2(e).XSFAOKEY, SFAO.Events.GRP2(e).MSGNUM.ToString)
                    zok = True
                    Exit For
                End If
            Next e
            If Not (zok) Then
                'si aucune action trouvée pour le bouton on le désactive
                ChgBtn("A", a, "", "", "")
            End If
        Next a

        'Récupération de la liste des phases 
        For e = 0 To SFAO.Events.GRP2.Count - 1
            If SFAO.Events.GRP2(e).ZTYPEMSG = WSEvtGRP2.TypeMsg.Phase Then
                Dim phs As New Phase
                phs.evenement = SFAO.Events.GRP2(e).MSGNUM
                phs.phase = SFAO.Events.GRP2(e).XPHASE
                phs.desc = SFAO.Events.GRP2(e).ZPHSDESC
                phs.desl = SFAO.Events.GRP2(e).ZPHSDESL
                phs.ordre = SFAO.Events.GRP2(e).XORDRE
                Phases.Add(phs)
                Trace("Phase(s) du poste : " & SFAO.Events.GRP2(e).ZPHSDESC & "(" & SFAO.Events.GRP2(e).XPHASE.ToString & ")")
            End If
        Next
    End Sub

    Private Sub InitaliseListes()
        Try
            'Récupération de la liste des motifs de non solde
            WSLstMns = X3ws.WSGETMNS(SFAO.Poste.GRP1.WST)
        Catch ex As Exception
            Trace("Erreur de connexion au web service !", FichierTrace.niveau.alerte)
            Trace("[WSGETMNS] : " & ex.Message, FichierTrace.niveau.erreur)
        End Try
    End Sub
    Public Sub Situation()

        'Situation du poste
        SituationPoste()

        'situation des OF/OP
        SituationOfOp()

        'situation des composants
        SituationCps()

        'on masque le gif load dans 1 seconde
        Call WaitGif(False, 1000)
    End Sub
    Private Sub SituationPoste()
        Dim i As Integer
        Trace("SituationPoste")
        WSsp = New WSSitPs 'on déclare la classe de la situation poste
        Try
            'on récupère la situation du poste via un web service
            WSsp = X3ws.WSGETSITPS(SFAO.Poste.GRP1.WST, SFAO.Site.GRP1.FCY)
        Catch ex As Exception
            Trace("[WSGETSITPS] Erreur du web service ! ", FichierTrace.niveau.erreur)
            Trace(ex.Message)
            X3Anim(0)
        End Try
        DataGridViewSitPost.Rows.Clear() 'on efface le tableau
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                'on ajoute les infos récupérées
                With DataGridViewSitPost
                    .Rows.Add()
                    .Rows(i).Cells("Matricule").Value = WSsp.GRP2(i).ZEMPDES + " (" + WSsp.GRP2(i).XEMPNUM.ToString + ")"
                    .Rows(i).Cells("TypeMatricule").Value = WSsp.GRP2(i).XTYPE_LBL
                    .Rows(i).Cells("Arrive").Value = AffDateHeure(CDate(WSsp.GRP2(i).XDATENT), WSsp.GRP2(i).XTIMENT)
                    .Rows(i).Cells("Evenement").Value = IIf(WSsp.GRP2(i).ZOPEXC > 0, WSsp.GRP2(i).ZROODES, WSsp.GRP2(i).ZEVTDES)
                    .Rows(i).Cells("Debut").Value = AffDateHeure(CDate(WSsp.GRP2(i).XDATEVT), WSsp.GRP2(i).XTIMEVT)
                    .Rows(i).Cells("OfOpPoste").Value = WSsp.GRP2(i).XMFGNUM + IIf(WSsp.GRP2(i).XOPENUM > 0, "/" + WSsp.GRP2(i).XOPENUM.ToString, "").ToString
                    .Rows(i).Cells("Client").Value = WSsp.GRP2(i).XBPCNAM
                End With
            Next
        Else
            'si aucun opérateur présent
            With DataGridViewSitPost
                .Rows.Add()
                .Rows(0).Cells(0).Value = "Aucun"
            End With
        End If

        DataGridViewSitPost.AutoResizeColumns()
        DataGridViewSitPost.AutoResizeRows()
    End Sub
    Private Sub SituationOfOp()
        Dim i As Integer
        Dim TotQtePrv As Integer
        Dim unit As String = ""
        Dim TotQteFab As Integer
        Dim TotQteRbu As Integer
        Dim TotTimeFab As Integer
        Dim PrcRbu As Decimal
        Dim OfOp, PrvOfOp, OfOpAff As String
        Trace("SituationOfOp")

        WSof = New WSSitOF      'on instentie la classe de la situation des opérations
        Try
            'on récupère la situation via le web service
            WSof = X3ws.WSGETSITOF(SFAO.Poste.GRP1.WST, SFAO.Site.GRP1.FCY)
        Catch ex As Exception
            Trace("[WSGETSITOF] Erreur du web service ! ", FichierTrace.niveau.erreur)
            Trace(ex.Message)
            X3Anim(0)
        End Try
        DataGridViewSitOpe.Rows.Clear() 'on efface le tableau

        TotQtePrv = 0
        TotQteFab = 0
        TotTimeFab = 0
        PrcRbu = 0
        OfOp = ""
        PrvOfOp = ""
        If WSof.GRP2.Count > 0 Then
            'On affiche la situation rècupérée
            For i = 0 To WSof.GRP2.Count - 1
                OfOp = WSof.GRP2(i).XMFGNUM + IIf(WSof.GRP2(i).XOPENUM > 0, "/" + WSof.GRP2(i).XOPENUM.ToString, "").ToString
                If OfOp <> PrvOfOp Then
                    OfOpAff = OfOp
                Else
                    OfOpAff = ""
                End If
                TotTimeFab += CInt(Calc_Duree(WSof.GRP2(i).XDATEVT, WSof.GRP2(i).XTIMEVT, Now, 2) + WSof.GRP2(i).ZCPLTIM)
                TotQtePrv += CInt(WSof.GRP2(i).EXTQTY)
                TotQteFab += CInt(WSof.GRP2(i).ZQTYA)
                TotQteRbu += CInt(WSof.GRP2(i).ZQTYR)
                unit = AFF_UNIT(WSof.GRP2(i).UOM)
                With DataGridViewSitOpe
                    .Rows.Add()
                    .Rows(i).Cells("OfOp").Value = OfOpAff
                    .Rows(i).Cells("Passe").Value = WSof.GRP2(i).ZCPLTIM
                    .Rows(i).Cells("DateDeb").Value = WSof.GRP2(i).XDATEVT
                    .Rows(i).Cells("TimeDeb").Value = WSof.GRP2(i).XTIMEVT
                    .Rows(i).Cells("Temps").Value = AFF_TPS_JHM(Calc_Duree(WSof.GRP2(i).XDATEVT, WSof.GRP2(i).XTIMEVT, Now, 2) + WSof.GRP2(i).ZCPLTIM)
                    .Rows(i).Cells("Article").Value = WSof.GRP2(i).ZITMREF
                    .Rows(i).Cells("DesArticle").Value = WSof.GRP2(i).ZITMDES
                    .Rows(i).Cells("QtePrv").Value = IIf(WSof.GRP2(i).EXTQTY > 0, WSof.GRP2(i).EXTQTY.ToString("### ### ###"), "")
                    .Rows(i).Cells("Unite").Value = AFF_UNIT(WSof.GRP2(i).UOM)
                    .Rows(i).Cells("QteFab").Value = IIf(WSof.GRP2(i).ZQTYA > 0, WSof.GRP2(i).ZQTYA.ToString("### ### ###"), "")
                    .Rows(i).Cells("QteReb").Value = IIf(WSof.GRP2(i).ZQTYR > 0, WSof.GRP2(i).ZQTYR.ToString("### ### ###"), "")
                    .Rows(i).Cells("Pal").Value = IIf(WSof.GRP2(i).ZLSTPAL > 0, WSof.GRP2(i).ZLSTPAL, "")
                End With
                PrvOfOp = WSof.GRP2(i).XMFGNUM + IIf(WSof.GRP2(i).XOPENUM > 0, "/" + WSof.GRP2(i).XOPENUM.ToString, "").ToString
            Next
        Else
            'si aucun OF/Op
            With DataGridViewSitOpe
                .Rows.Add()
                .Rows(0).Cells(0).Value = "Aucun"
            End With
        End If
        DataGridViewSitOpe.AutoResizeColumns()
        DataGridViewSitOpe.AutoResizeRows()

        'Affichage des indicateurs
        Trace("Affichage des indicateurs")
        If TotQtePrv > 0 Then
            Lab3Qth.Text = TotQtePrv.ToString("### ### ##0") & " " & unit       'qté héorique
            'TODO afficher les tolèrances de livraison
        End If
        If TotQteFab > 0 Then
            Lab3Qra.Text = TotQteFab.ToString("### ### ##0") & " " & unit       'qté réalisés
        End If

        If TotQteFab < TotQtePrv Then
            'si la qté théorique n'est pas réalisée on change la couleur en rouge
            Lab3Qra.ForeColor = Color.FromArgb(255, 128, 128)
        Else
            'si non blanc
            Lab3Qra.ForeColor = Color.White
        End If

        'on affiche la production horaire moyenne
        If TotTimeFab > 0 And TotQteFab > 0 Then
            Lab3Vit.Text = CInt((TotQteFab / TotTimeFab) * 3600).ToString & " " & unit & "/H"
        Else
            Lab3Vit.Text = ""
        End If

        'on affiche le % de gâche
        If TotQteFab > 0 And TotQteRbu > 0 Then
            PrcRbu = CDec(TotQteRbu * 100 / TotQteFab)
            Lab3Reb.Text = PrcRbu.ToString("##0.00") & " %"
        Else
            Lab3Reb.Text = ""
        End If

        If PrcRbu > 3 Then
            'si le % de gâche est > 3% on l'affiche en rouge
            Lab3Reb.ForeColor = Color.FromArgb(255, 128, 128)
        Else
            Lab3Reb.ForeColor = Color.White
        End If
    End Sub
    Private Sub SituationCps()
        Dim i As Integer
        Dim OfOp, PrvOfOp, OfOpAff As String
        Dim Art, PrvArt, ArtAff As String
        Trace("SituationCps")
        WScp = New WSSitCP  'classe de la situation des composants (ou palettes)
        Try
            WScp = X3ws.WSGETSITCP(SFAO.Poste.GRP1.WST, SFAO.Site.GRP1.FCY, SFAO.Poste.GRP1.Y_TYPOP, SFAO.Poste.GRP1.STOLOC, SFAO.Poste.GRP1.ZATELOC)
            'on récupère la situation via le web service
        Catch ex As Exception
            Trace("[WSGETSITCP] Erreur du web service ! ", FichierTrace.niveau.erreur)
            Trace(ex.Message)
            X3Anim(0)
        End Try
        DataGridViewSitComp.Rows.Clear() 'on efface le tableau
        If WScp.GRP2.Count > 0 Then
            'Affichage de la situation des palettes
            If SFAO.Poste.GRP1.Y_TYPOP = "PAL" Then
                OfOp = ""
                PrvOfOp = ""
                Art = ""
                PrvArt = ""
                For i = 0 To WScp.GRP2.Count - 1
                    OfOp = WScp.GRP2(i).XMFGNUM + IIf(WScp.GRP2(i).XOPENUM > 0, "/" + WScp.GRP2(i).XOPENUM.ToString, "").ToString
                    Art = WScp.GRP2(i).ZITMREF
                    If OfOp <> PrvOfOp Then
                        OfOpAff = OfOp
                    Else
                        OfOpAff = ""
                    End If
                    If Art <> PrvArt Then
                        ArtAff = Art
                    Else
                        ArtAff = ""
                    End If
                    With DataGridViewSitComp
                        .Rows.Add()
                        .Rows(i).Cells("OfOpc").Value = OfOpAff
                        .Rows(i).Cells("Cmp").Value = ArtAff
                        .Rows(i).Cells("Desc").Value = WScp.GRP2(i).ZITMDES
                        .Rows(i).Cells("QteBes").Value = IIf(WScp.GRP2(i).RETQTY > 0, WScp.GRP2(i).RETQTY.ToString("### ### ###"), "")
                        .Rows(i).Cells("Unitc").Value = AFF_UNIT(WScp.GRP2(i).ZUOM)
                        .Rows(i).Cells("QteCso").Value = IIf(WScp.GRP2(i).USEQTY > 0, WScp.GRP2(i).USEQTY.ToString("### ### ###"), "")
                        .Rows(i).Cells("StoMac").Value = WScp.GRP2(i).ZSUPGRP
                        .Rows(i).Cells("StoAte").Value = WScp.GRP2(i).USRFLD2
                    End With
                    PrvOfOp = WScp.GRP2(i).XMFGNUM + IIf(WScp.GRP2(i).XOPENUM > 0, "/" + WScp.GRP2(i).XOPENUM.ToString, "").ToString
                    PrvArt = WScp.GRP2(i).ZITMREF
                Next
            Else
                'Affichage de la situation des composants
                For i = 0 To WScp.GRP2.Count - 1
                    With DataGridViewSitComp
                        .Rows.Add()
                        .Rows(i).Cells("OfOpc").Value = WScp.GRP2(i).XMFGNUM + IIf(WScp.GRP2(i).XOPENUM > 0, "/" + WScp.GRP2(i).XOPENUM.ToString, "").ToString
                        .Rows(i).Cells("Cmp").Value = WScp.GRP2(i).ZITMREF
                        .Rows(i).Cells("Numc").Value = WScp.GRP2(i).ZSUPGRP
                        .Rows(i).Cells("Desc").Value = WScp.GRP2(i).ZITMDES
                        .Rows(i).Cells("QteBes").Value = IIf(WScp.GRP2(i).RETQTY > 0, WScp.GRP2(i).RETQTY.ToString("### ### ###"), "")
                        .Rows(i).Cells("Unitc").Value = AFF_UNIT(WScp.GRP2(i).ZUOM)
                        .Rows(i).Cells("QteCso").Value = IIf(WScp.GRP2(i).USEQTY > 0, WScp.GRP2(i).USEQTY.ToString("### ### ###"), "")
                        .Rows(i).Cells("StoMac").Value = IIf(WScp.GRP2(i).STOMAC > 0, WScp.GRP2(i).STOMAC.ToString("### ### ###"), "")
                        .Rows(i).Cells("StoAte").Value = IIf(WScp.GRP2(i).STOATE > 0, WScp.GRP2(i).STOATE.ToString("### ### ###"), "")
                    End With
                Next
            End If
        Else
            'si la situation est vide
            With DataGridViewSitComp
                .Rows.Add()
                .Rows(0).Cells(0).Value = "Aucun"
            End With
        End If
        DataGridViewSitComp.AutoResizeColumns()
        DataGridViewSitComp.AutoResizeRows()
    End Sub

    'Fonction qui permet d'activer ou désactiver un bouton de type événement ou action
    Private Sub ChgBtn(ByVal typ As String, ByVal bnum As Integer, ByVal btxt As String, ByVal racc As String, ByVal tag As String, Optional ByVal actif As Boolean = True)
        Dim btn As Button
        Dim posr As Integer
        Try
            btn = CType(Me.Controls.Find(typ & bnum.ToString("00"), True).ElementAt(0), Button) 'on vérifie si le bouton demandée existe
            If btn.Name = typ & bnum.ToString("00") Then
                btn.Tag = tag       'on enregistre le n° dévénement dans le tag
                If btxt <> "" Then
                    If btxt <> " " Then
                        If typ = "F" Then
                            btn.Text = btxt 'texte du bouton Fxx + titre
                            Trace("Bouton " & btn.Name & " activé pour l'événement n° " & tag & " " & btn.Text)
                        Else
                            If racc <> "" And InStr(btxt, "&") = 0 Then
                                'on insère le caractère & dans le texte du bouton action devant la première lettre du raccourcis 
                                'cela active les raccourcis
                                posr = InStr(UCase(RemoveAccents(btxt)), UCase(racc))
                                If posr > 0 Then
                                    If posr = 1 Then
                                        btxt = "&" + btxt
                                    Else
                                        btxt = Strings.Left(btxt, posr - 1) + "&" + Strings.Right(btxt, Len(btxt) - posr + 1)
                                    End If
                                Else
                                    'si non on ajoute le raccourcis après le texte 
                                    btxt += " " & "(&" & UCase(racc) & ")"
                                End If
                            End If

                            btn.Text = btxt 'texte du bouton action
                            Trace("Bouton " & btn.Name & " activé pour l'action n° " & tag & " " & btn.Text)
                        End If

                        If racc <> "" Then
                            Dim Lbl As New Label() With
                                    {
                                    .Parent = btn,
                                    .Name = "L" & btn.Name,
                                    .Text = UCase(racc),
                                    .ForeColor = Color.FromArgb(200, 200, 200),
                                    .BorderStyle = BorderStyle.None,
                                    .BackColor = Color.Transparent,
                                    .Padding = New Padding(2, 2, 0, 0),
                                    .AutoSize = True
                                   }

                            btn.Controls.Add(Lbl)
                            Lbl.BringToFront()
                        End If
                    End If

                    If btn.Visible = False Then
                        btn.Visible = True
                    End If
                    btn.Enabled = actif

                Else
                    Trace("Bouton " & btn.Name & " désactivé")
                    btn.Text = ""
                    btn.Visible = False
                    btn.Enabled = False
                End If
            End If
        Catch ex As Exception
            Trace("[ChgBtn]" & " le bouton " & typ & bnum.ToString("00") & " n'existe pas!")
        End Try
    End Sub

    'Private Sub InitGraphs()
    '    With GraphProd
    '        .DataSource = GetDataProd()
    '        .Series.Clear()
    '        .ChartAreas.Clear()
    '        .ChartAreas.Add("ChartAreaProd")
    '        '.ChartAreas("ChartAreaProd").Area3DStyle.Enable3D = True
    '        '.ChartAreas("ChartAreaProd").Area3DStyle.Inclination = 10
    '        '.ChartAreas("ChartAreaProd").Area3DStyle.Rotation = 85
    '        .Series.Add("Théorique")
    '        .Series.Add("Réalisé")

    '        .Series(0).XValueMember = "Name"
    '        .Series(0).YValueMembers = "Théorique"
    '        .Series(0).IsValueShownAsLabel = True
    '        .ChartAreas(0).AxisX.LabelStyle.Angle = -90

    '        .Series(1).XValueMember = "Name"
    '        .Series(1).YValueMembers = "Réalisé"
    '        .Series(1).IsValueShownAsLabel = True
    '    End With

    '    'Dim c(15) As Color
    '    'c = {Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.Green, Color.White, Color.Transparent}

    '    'With GraphVitesse
    '    '    .DataSource = GetDataVit()
    '    '    .Series.Clear()
    '    '    .ChartAreas.Clear()
    '    '    .ChartAreas.Add("ChartArea1Vit")
    '    '    .ChartAreas("ChartArea1Vit").Area3DStyle.Enable3D = True
    '    '    .ChartAreas("ChartArea1Vit").Area3DStyle.Rotation = 180
    '    '    .ChartAreas("ChartArea1Vit").Area3DStyle.Inclination = 20
    '    '    .ChartAreas("ChartArea1Vit").BackColor = Color.Transparent
    '    '    .PaletteCustomColors = c
    '    '    .Palette = ChartColorPalette.None
    '    '    .Series.Add("Vitesse moyenne")

    '    '    '.Series(0).XValueMember = "Name"
    '    '    .Series(0).YValueMembers = "Vitesse moyenne"
    '    '    .Series(0).IsValueShownAsLabel = False
    '    '    .Series(0).ChartType = SeriesChartType.Pie
    '    '    .Series(0).BorderDashStyle = ChartDashStyle.Solid
    '    '    .Series(0).BorderColor = Color.Black
    '    'End With


    'End Sub
    'Private Function GetDataProd() As DataTable
    '    Dim dt As New DataTable()
    '    dt.Columns.Add("Name", GetType(String))
    '    dt.Columns.Add("Théorique", GetType(Integer))
    '    dt.Columns.Add("Réalisé", GetType(Integer))
    '    dt.Rows.Add("OF1", 12, 17)
    '    dt.Rows.Add("OF2", 19, 20)
    '    Return dt
    'End Function
    'Private Function GetDataVit() As DataTable
    '    Dim dt As New DataTable()
    '    dt.Columns.Add("Vitesse moyenne", GetType(Integer))
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(10)
    '    dt.Rows.Add(50)
    '    dt.Rows.Add(180)
    '    Return dt
    'End Function

    'fonction qui gère l'appuie clavier sur les touches Fxx
    Private Sub FenSfao_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Dim btn As Button
        If e.KeyCode >= Keys.F1 And e.KeyCode <= Keys.F12 Then
            btn = CType(FlowLayoutPanelBas.Controls.Find("F" + (e.KeyCode - 111).ToString("00"), True).ElementAt(0), Button)
            btn.PerformClick()  'simulation du click

            'pour éviter que la touche de fonction soit transmise à d'autres appli
            e.Handled = True
            e.SuppressKeyPress = True
        End If
    End Sub


    Private Sub BtnAF_Enter(sender As Object, e As EventArgs) Handles A01.Enter, A02.Enter, A03.Enter, A04.Enter, A05.Enter, A06.Enter, A07.Enter, A08.Enter, A09.Enter, A10.Enter, A11.Enter, A12.Enter, A13.Enter, A14.Enter, A15.Enter, A16.Enter, A17.Enter, A18.Enter, A19.Enter, A20.Enter, F01.Enter, F02.Enter, F03.Enter, F04.Enter, F05.Enter, F06.Enter, F07.Enter, F08.Enter, F09.Enter, F10.Enter, F11.Enter, F12.Enter
        Dim btn As Button
        btn = CType(sender, Button) 'bouton entrée
        btn.BackColor = Color.FromArgb(94, 125, 174)
    End Sub

    Private Sub BtnAF_Leave(sender As Object, e As EventArgs) Handles A01.Leave, A02.Leave, A03.Leave, A04.Leave, A05.Leave, A06.Leave, A07.Leave, A08.Leave, A09.Leave, A10.Leave, A11.Leave, A12.Leave, A13.Leave, A14.Leave, A15.Leave, A16.Leave, A17.Leave, A18.Leave, A19.Leave, A20.Leave, F01.Leave, F02.Leave, F03.Leave, F04.Leave, F05.Leave, F06.Leave, F07.Leave, F08.Leave, F09.Leave, F10.Leave, F11.Leave, F12.Leave
        Dim btn As Button
        btn = CType(sender, Button) 'bouton sorti
        btn.BackColor = Color.FromArgb(56, 96, 155)
    End Sub

    'Fonction qui gère le click sur une touche de fonction Fxx
    Private Sub BtnF_Click(sender As Object, e As EventArgs) Handles F01.Click, F02.Click, F03.Click, F04.Click, F05.Click, F06.Click, F07.Click, F08.Click, F09.Click, F10.Click, F11.Click, F12.Click
        Dim btn As Button
        Dim Result As DialogResult

        btn = CType(sender, Button) 'bouton cliqué
        If btn.Enabled = True And btn.Visible = True Then  'si le bouton est actif et visible

            With btn
                .Select()
                .BackColor = Color.FromArgb(131, 155, 194)
                .Refresh()
            End With
            SFAO.Sleep(250)
            With btn
                .BackColor = Color.FromArgb(94, 125, 174)
                .Refresh()
            End With

            Trace("[BtnF_Click] bouton fonction : " & btn.Name & " " & btn.Tag.ToString)
            Result = Evenements.AfficheEvt(CInt(btn.Tag)) 'appel de la fonction d'affichage des début d'événements
            Trace("[BtnF_Click] résultat fonction : " & Result.ToString)

            If Result = DialogResult.OK Then
                'Récupération et affichage de la situation du poste
                Call Situation()
            End If
        End If
    End Sub

    'Fonction qui gère l'appuie sur les boutons action
    Private Sub BtnA_Click(sender As Object, e As EventArgs) Handles A01.Click, A02.Click, A03.Click, A04.Click, A05.Click, A06.Click, A07.Click, A08.Click, A09.Click, A10.Click, A11.Click, A12.Click, A13.Click, A14.Click, A15.Click
        Dim btn As Button
        Dim Result As DialogResult

        btn = CType(sender, Button) 'bouton cliqué
        If btn.Enabled = True And btn.Visible = True Then 'si le bouton est actif et visible
            With btn
                .Select()
                .BackColor = Color.FromArgb(131, 155, 194)
                .Refresh()
            End With
            SFAO.Sleep(250)
            With btn
                .BackColor = Color.FromArgb(94, 125, 174)
                .Refresh()
            End With

            Trace("[BtnA_Click] bouton action : " & btn.Name & " " & btn.Tag.ToString)
            Result = Evenements.AfficheEvt(CInt(btn.Tag)) 'appel de la fonction d'affichage des début d'événements
            Trace("[BtnA_Click] résultat action : " & Result.ToString)

            If Result = DialogResult.OK Then
                'Récupération et affichage de la situation du poste
                Call Situation()
            End If
        End If
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs)
        Dim labelFileName As String
        labelFileName = "test.zpl"
        Dim filename As String = System.IO.Path.GetFileName("test.zpl")
        System.IO.File.Copy(
                labelFileName,
                "\\192.168.0.156\PRBPNOZTEST1\" & filename)
    End Sub

    'Private Sub Button14_Click(sender As Object, e As EventArgs)
    '    ZPL.SendStringToPrinter("\\192.168.0.156\PRBPNOZTEST1", "^XA^LH30, 30 \ n ^ FO20, 10 ^ ADN, 90, 50 ^ AD ^ FDHello World^FS\n^XZ")
    'End Sub

    Private Sub DataGridViewSitPost_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewSitPost.SelectionChanged
        DataGridViewSitPost.ClearSelection()
        'F01.Select()
    End Sub

    Private Sub DataGridViewOpe_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewSitOpe.SelectionChanged
        DataGridViewSitOpe.ClearSelection()
        'F01.Select()
    End Sub

    Private Sub DataGridViewSitComp_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridViewSitComp.SelectionChanged
        DataGridViewSitComp.ClearSelection()
        'F01.Select()
    End Sub

    Private Sub FenSfao_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        Redimentionner()
    End Sub

    'Private Sub FenSfao_FontChanged(sender As Object, e As EventArgs) Handles Me.FontChanged
    '    Debug.WriteLine("Debug " & "FenSfao_FontChanged")
    'End Sub
    'Private Sub FenSfao_Resize(sender As Object, e As EventArgs) Handles Me.Resize
    '    'Debug.WriteLine("Debug " & "FenSfao_Resize")
    'End Sub

    'Private Sub FenSfao_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
    '    'Debug.WriteLine("Debug " & "FenSfao_Paint")
    'End Sub
    Private Sub Redimentionner()
        Dim fntBtn As Font
        Dim fntLabBtn As Font
        Dim fntAct As Font
        Dim fntLabAct As Font
        Dim fntLbl As Font
        Dim fntTit As Font
        Dim fntTitB As Font
        Dim fntTitB2 As Font
        Dim fntTbl As Font
        Dim fntMax As Font
        Dim zBtnOk As Boolean
        Dim zActOk As Boolean
        Dim zTailBtn As Single
        Dim zTailAct As Single
        Dim zTailLbl As Single
        Dim zTailTbl As Single
        Dim zTailTit As Single
        Dim zTailTitB2 As Single
        Dim zTailInd As Single
        Dim zTailMax As Single
        Dim btnTxt As String
        Dim actTxt As String
        Dim textSize As Size
        Dim textWidth As Integer
        Dim textHeight As Integer
        Dim textActSize As Size
        Dim textActWidth As Integer
        Dim textActHeight As Integer
        Dim flowHeight As Integer
        Dim flowWidth As Integer
        Dim flowLeftHeight As Integer
        Dim flowLeftWidth As Integer
        Dim btnHeight As Integer
        Dim numbtn As Integer
        Dim numact As Integer
        Dim nbtn As Integer
        Dim nact As Integer
        Dim n As Integer
        Dim z As Integer
        Dim i As Integer
        Dim btnWidth(13) As Integer
        Dim actHeight(20) As Integer    'Déclaration pour 20 boutons action maximum
        Dim totTextWidth As Integer
        Dim totTextHeight As Integer
        Dim zAdBtn As Integer
        Dim zAdAct As Integer
        Dim zRstAct As Integer
        Dim btnact As Button
        Dim zajuste As Boolean
        Dim zexit As Boolean

        If Me.Visible = True Then
            zTailBtn = 22   'taille max

            'Boutons fonctions
            flowWidth = FlowLayoutPanelBas.Width - 40
            flowHeight = FlowLayoutPanelBas.Height - 3
            Do
                totTextWidth = 0
                textHeight = 0
                textWidth = 0
                nbtn = 0
                fntBtn = New Font("Microsoft Sans Serif", zTailBtn, FontStyle.Regular)
                For Each btn As Button In FlowLayoutPanelBas.Controls.OfType(Of Button)().OrderBy(Function(x) x.Name)
                    If btn.Visible = True Then
                        btnTxt = btn.Text
                        If btnTxt <> "" Then
                            Try
                                textSize = TextRenderer.MeasureText("  " & btnTxt & "  ", fntBtn)
                                textWidth = textSize.Width + btn.Padding.Left + btn.Padding.Right
                                textHeight = Math.Max(textHeight, textSize.Height + btn.Padding.Top + btn.Padding.Bottom)
                            Catch ex As Exception
                                textWidth = btn.Width
                                textHeight = btn.Height
                            End Try
                            numbtn = CInt(Val(Strings.Right(btn.Name, 2)))
                            btnWidth(numbtn) = textWidth
                            totTextWidth += textWidth + btn.Margin.Right
                            nbtn += 1
                        End If
                    End If
                Next
                'Debug.WriteLine("Debug " & zTailBtn.ToString & " " & totTextWidth.ToString & "-" & flowWidth.ToString)
                If totTextWidth < flowWidth And textHeight <= flowHeight Then
                    zBtnOk = True
                Else
                    zBtnOk = False
                    fntBtn.Dispose()
                    zTailBtn -= Convert.ToSingle(0.5)
                End If
            Loop Until (zBtnOk Or zTailBtn < 3)

            fntLabBtn = New Font("Microsoft Sans Serif", zTailBtn + 2, FontStyle.Bold)

            'Debug.WriteLine("Debug " & zTailBtn.ToString & " " & totTextWidth.ToString & " " & flowWidth.ToString)
            If flowWidth - totTextWidth > 100 Then
                zAdBtn = CInt((flowWidth - totTextWidth - 100) / nbtn)
            End If

            For Each btn As Button In FlowLayoutPanelBas.Controls.OfType(Of Button)().OrderBy(Function(x) x.Name)
                If btn.Visible = True Then
                    With btn
                        .Font = fntBtn
                        .Width = btnWidth(CInt(Val(Strings.Right(btn.Name, 2)))) + zAdBtn
                        .Height = flowHeight
                    End With
                    If btn.Controls.Count > 0 Then
                        With btn.Controls("L" & btn.Name)
                            .Font = fntLabBtn
                        End With

                    End If
                End If
            Next
            FQT.Width = Math.Max(FQT.Width, FlowLayoutPanelBas.Width - FQT.Left - FQT.Margin.Right - 2)

            'boutons action
            zTailAct = 22 'taille max
            flowLeftWidth = FlowLayoutPanelLeft.Width - 2
            flowLeftHeight = FlowLayoutPanelLeft.Height - FlowLayoutPanelLeft.Padding.Top - FlowLayoutPanelLeft.Padding.Bottom - 20
            Do
                totTextHeight = 0
                textActWidth = 0
                textActHeight = 0
                nact = 0
                fntAct = New Font("Microsoft Sans Serif", zTailAct, FontStyle.Regular)
                For Each act As Button In FlowLayoutPanelLeft.Controls.OfType(Of Button)().OrderBy(Function(x) x.Name)
                    If act.Visible = True Then
                        actTxt = act.Text
                        If actTxt <> "" Then
                            Try
                                textActSize = TextRenderer.MeasureText(" " & actTxt & " ", fntAct)
                                textActWidth = Math.Max(textActWidth, textActSize.Width + act.Padding.Left + act.Padding.Right)
                                textActHeight = textActSize.Height + act.Padding.Top + act.Padding.Bottom
                            Catch ex As Exception
                                textActWidth = act.Width
                                textActHeight = act.Height
                            End Try
                            numact = CInt(Val(Strings.Right(act.Name, 2)))
                            actHeight(numact) = textActHeight
                            totTextHeight += textActHeight + act.Margin.Bottom
                            nact += 1
                        End If
                    End If
                Next
                'Debug.WriteLine("Debug " & zTailAct.ToString & " " & totTextHeight.ToString & "-" & flowLeftHeight.ToString)
                If totTextHeight < flowLeftHeight And textActWidth <= flowLeftWidth Then
                    zActOk = True
                Else
                    zActOk = False
                    fntAct.Dispose()
                    zTailAct -= Convert.ToSingle(0.5)
                End If
            Loop Until (zActOk Or zTailAct < 3)

            fntLabAct = New Font("Microsoft Sans Serif", zTailAct + 2, FontStyle.Bold)

            'Debug.WriteLine("Debug " & zTailAct.ToString & " " & totTextHeight.ToString & " " & flowLeftHeight.ToString)

            If nact > 0 And flowLeftHeight - totTextHeight > nact Then
                zAdAct = CInt((flowLeftHeight - totTextHeight - nact) / nact)
            End If

            For Each act As Button In FlowLayoutPanelLeft.Controls.OfType(Of Button)().OrderBy(Function(x) x.Name)
                If act.Visible = True Then
                    act.Font = fntAct
                    btnHeight = actHeight(CInt(Val(Strings.Right(act.Name, 2)))) + zAdAct
                    If nact < 10 Then
                        act.Height = Math.Min(80, btnHeight)
                        If btnHeight < 80 Then
                            zajuste = True
                        End If
                    Else
                        act.Height = btnHeight
                        zajuste = True
                    End If
                    act.Width = flowLeftWidth
                    If act.Controls.Count > 0 Then
                        With act.Controls("L" & act.Name)
                            .Font = fntLabAct
                        End With
                    End If
                End If
            Next
            If zajuste Then
                n = 0
                flowLeftHeight = FlowLayoutPanelLeft.Height - FlowLayoutPanelLeft.Padding.Top - FlowLayoutPanelLeft.Padding.Bottom
                For Each act As Button In FlowLayoutPanelLeft.Controls.OfType(Of Button)().OrderByDescending(Function(x) x.Name)
                    If act.Visible = True Then
                        n += 1
                        'Debug.WriteLine("Debug " & act.Name & " " & (act.Top + act.Height).ToString & " " & flowLeftHeight.ToString)
                        If n = 1 And act.Top + act.Height < flowLeftHeight Then
                            zRstAct = flowLeftHeight - (act.Top + act.Height)
                            If zRstAct > 0 Then
                                For z = zRstAct To 0 Step -1
                                    For i = 15 To 1 Step -1
                                        btnact = CType(FlowLayoutPanelLeft.Controls.Find("A" & i.ToString("00"), True).ElementAt(0), Button)
                                        If btnact.Visible = True Then
                                            btnact.Height += 1
                                            If act.Left > 0 Then
                                                btnact.Height -= 1
                                                zexit = True
                                                Exit For
                                            End If
                                            z -= 1
                                        End If
                                        If zexit Then
                                            Exit For
                                        End If
                                    Next i
                                Next z
                            End If
                        End If
                        Exit For
                    End If
                Next
            End If

            zTailLbl = TaillePolice(13, 6)
            zTailTit = TaillePolice(20, 9)
            zTailInd = TaillePolice(19, 9)
            zTailTitB2 = TaillePolice(17, 9)
            zTailTbl = TaillePolice(14, 7)
            zTailMax = TaillePolice(30, 14)

            fntLbl = New Font("Microsoft Sans Serif", zTailLbl, FontStyle.Regular)
            fntTbl = New Font("Microsoft Sans Serif", zTailTbl, FontStyle.Regular)
            fntTit = New Font("Microsoft Sans Serif", zTailTit, FontStyle.Regular)
            fntTitB = New Font("Microsoft Sans Serif", zTailInd, FontStyle.Bold)
            fntTitB2 = New Font("Microsoft Sans Serif", zTailTitB2, FontStyle.Bold)
            fntMax = New Font("Microsoft Sans Serif", zTailMax, FontStyle.Bold)

            Lab1soc.Font = fntLbl
            Lab1sit.Font = fntLbl
            Lab1dos.Font = fntLbl
            Lab1pst.Font = fntLbl

            Lab1qt.Font = fntLbl
            Lab1qr.Font = fntLbl
            Lab1vit.Font = fntLbl
            Lab1reb.Font = fntLbl
            Lab1Sitmat.Font = fntLbl
            Lab1SitOp.Font = fntLbl
            Lab1Sitpost.Font = fntLbl

            Lab2Soc.Font = fntTitB2
            Lab2Sit.Font = fntTitB2
            Lab2Dos.Font = fntTitB2
            Lab2Post.Font = fntTitB2

            Lab3Qra.Font = fntTitB
            Lab3Qth.Font = fntTitB
            Lab3Vit.Font = fntTitB
            Lab3Reb.Font = fntTitB

            DateHeure.Font = fntMax
            DateHeure.Left = CInt((Me.Width / 2) - (DateHeure.Width / 2))
            DateHeure.Top = CInt((TableLayoutPanel0.Top / 2) - (DateHeure.Height / 2)) + 3

            With DataGridViewSitPost
                .ColumnHeadersDefaultCellStyle.Font = fntLbl
                .RowsDefaultCellStyle.Font = fntTbl
                .AutoResizeColumns()
                .AutoResizeRows()
            End With

            With DataGridViewSitOpe
                .ColumnHeadersDefaultCellStyle.Font = fntLbl
                .RowsDefaultCellStyle.Font = fntTbl
                .AutoResizeColumns()
                .AutoResizeRows()
            End With

            With DataGridViewSitComp
                .ColumnHeadersDefaultCellStyle.Font = fntLbl
                .RowsDefaultCellStyle.Font = fntTbl
                .AutoResizeColumns()
                .AutoResizeRows()
            End With
        End If
    End Sub

    'Timer qui affiche gère l'affichage de la date + heure chaque seconde
    Private Sub TimerSec_Tick(sender As Object, e As EventArgs) Handles TimerSec.Tick
        DateHeure.Text = Now.ToString
    End Sub

    'Timer qui recalcule les temps passés chaque minute
    Private Sub TimerMin_Tick(sender As Object, e As EventArgs) Handles TimerMin.Tick
        Dim TimeTot As Decimal
        Dim TimeAff As String
        TimerMin.Interval = 60000
        For Each row As DataGridViewRow In DataGridViewSitOpe.Rows
            If row.Cells("OfOp").Value IsNot Nothing AndAlso (row.Cells("OfOp").Value.ToString <> "" And row.Cells("OfOp").Value.ToString <> "Aucun") Then
                TimeTot = Calc_Duree(CDate(row.Cells("DateDeb").Value), CInt(row.Cells("TimeDeb").Value), Now, 2)
                TimeTot += CInt(row.Cells("Passe").Value)
                If TimeTot > 0 Then
                    TimeAff = AFF_TPS_JHM(TimeTot)
                    row.Cells("Temps").Value = TimeAff
                End If
            End If
            'TODO recalculer l'indicateur de vitesse
        Next
    End Sub

    'timer pour contrôler l'accès du web service
    Private Sub TimerAnim_Tick(sender As Object, e As EventArgs) Handles TimerAnim.Tick
        AnimCal(TimerAnim.Interval)
    End Sub
    Private Sub AnimCal(ByVal TimerInterval As Integer)
        If TimerInterval <> 3601 Then
            'Lancement du web service synchrone ou asynchrone pour avoir la date-heure
            If WsNbrAsyncInvoke = 0 Then 'pas d'appel asynchrone en attente
                X3ws.WSDTSFAO_ASYNC()
            End If
        End If
    End Sub
    Public Sub AnimRet(ByVal WSDateTime As String)
        If WSDateTime > DtSFAO Then
            If DblClc Then
                Trace("Test WSDateTime : " & WSDateTime)
                DblClc = False
            End If

            If TimerAnim.Interval = 4000 Then
                X3Anim(1)
            Else
                X3Anim(2)
            End If
            DtSFAO = WSDateTime
        Else
            X3Anim(0)
            Etat("Le serveur web n'a pas répondu!", 2, 5000)
        End If
    End Sub

    'Affiche le gif animé pour savoir si le web service est ok
    Public Sub X3Anim(ByVal anim As Integer)
        If anim = 0 Then 'Erreur pour 15s
            If PictureBoxWS.Tag.ToString <> "_0" Then
                PictureBoxWS.Image = My.Resources._0
                PictureBoxWS.Tag = "_0"
            End If
            TimerAnim.Interval = WsTimInt
        ElseIf anim = 1 Then 'on affiche l'image ok pour quelques minutes
            If PictureBoxWS.Tag.ToString <> "_1" Then
                PictureBoxWS.Image = My.Resources._1
                PictureBoxWS.Tag = "_1"
            End If
            TimerAnim.Interval = WsTimInt
        ElseIf anim = 2 Then 'on affiche le gif une fois
            If PictureBoxWS.Tag.ToString <> "X3OK" Then
                PictureBoxWS.Image = My.Resources.X3OK
                PictureBoxWS.Tag = "X3OK"
            End If
            TimerAnim.Interval = 4000
        ElseIf anim > 2 Then 'On affiche le gif x fois et on arrête le contrôle pour la prochaine fois en mettant l'intervale à 3601
            If PictureBoxWS.Tag.ToString <> "X3OK" Then
                PictureBoxWS.Image = My.Resources.X3OK
                PictureBoxWS.Tag = "X3OK"
            End If
            TimerAnim.Interval = 3601
        End If
    End Sub
    Public Function TaillePolice(ByVal _max As Integer, ByVal _min As Integer) As Single
        Dim fenHeight, fenWidth, zdiv, zadd As Integer
        Dim taille As Single
        fenHeight = Me.Height
        fenWidth = Me.Width
        zdiv = CInt((2073600 - 240000) / (_max - _min))
        zadd = CInt(_max - (2073600 / zdiv))
        taille = CType(((fenWidth * fenHeight) / zdiv) + zadd, Single)
        If taille > _max Then taille = _max
        If taille < _min Then taille = _min
        Return taille
    End Function

    Public Sub Etat(ByVal message As String, ByVal num As Integer, Optional ByVal time As Integer = 0, Optional couleur As Color = Nothing)
        If TimerEtat.Enabled Then
            TimerEtat.Stop()
            If CInt(TimerEtat.Tag) = 1 Then
                LabelEtat1.Text = ""
            ElseIf CInt(TimerEtat.Tag) = 2 Then
                LabelEtat2.Text = ""
            End If
            TimerEtat.Enabled = False
        End If

        If num = 1 Then
            If couleur = Nothing Then
                LabelEtat1.ForeColor = Color.White
            Else
                LabelEtat1.ForeColor = couleur
            End If
            LabelEtat1.Text = message
        ElseIf num = 2 Then
            If couleur = Nothing Then
                LabelEtat2.ForeColor = Color.White
            Else
                LabelEtat2.ForeColor = couleur
            End If
            LabelEtat2.Text = message
        End If

        If time > 0 Then

            TimerEtat.Enabled = True
            TimerEtat.Interval = time
            TimerEtat.Tag = num.ToString
            TimerEtat.Start()
        End If
    End Sub

    Private Sub TimerEtat_Tick(sender As Object, e As EventArgs) Handles TimerEtat.Tick
        TimerEtat.Stop()
        If CInt(TimerEtat.Tag) = 1 Then
            LabelEtat1.Text = ""
        ElseIf CInt(TimerEtat.Tag) = 2 Then
            LabelEtat2.Text = ""
        End If
        TimerEtat.Enabled = False
    End Sub

    Private Sub PictureBoxWS_DoubleClick(sender As Object, e As EventArgs) Handles PictureBoxWS.DoubleClick
        Dim SvgTest As Boolean
        SvgTest = SfaoTest
        DblClc = True
        SfaoTest = True
        Trace("Test connexion X3 : ")
        AnimCal(TimerAnim.Interval)
        SfaoTest = SvgTest
    End Sub
    Public ReadOnly Property MaxHeuresPresence() As Decimal
        Get
            Dim GZTIMWRK As String
            If Me._maxHeuresPresence = 0 Then
                'on récupère le paramère X3 de la durée maximum de présence sur un poste
                GZTIMWRK = X3ws.WSGETPARAM("GZTIMWRK", SFAO.Site.GRP1.FCY, "")
                If GZTIMWRK <> "" Then
                    Me._maxHeuresPresence = CInt(GZTIMWRK)
                End If
                If Me._maxHeuresPresence = 0 Then
                    Me._maxHeuresPresence = 12 'valeur par défaut 12h
                End If
            End If
            Return Me._maxHeuresPresence
        End Get
    End Property
    'fonction qui contrôle si le matricule saisi est présent sur le poste 
    Public Sub CtrlMatr(ByVal _matr As Integer, ByRef _msgErr As String, ByRef _textBoxNom As String)
        Dim present As Boolean
        Dim i As Integer
        'On parcourt la situation du poste pour vérifier si le matricule saisi est bien présent sur ce poste
        For i = 0 To WSsp.GRP2.Count - 1
            If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                _textBoxNom = WSsp.GRP2(i).ZEMPDES
                present = True
                Exit For
            End If
        Next
        If present = False Then
            _msgErr = "Matricule n° " & _matr.ToString & " non présent sur ce poste !"
        End If
    End Sub
    'fonction qui contrôle si le matricule saisi est présent sur le poste et calcule la duree de sa présence
    Public Sub CtrlMatrDuree(ByVal _matr As Integer, ByRef _msgErr As String, ByRef _textBoxNom As String, ByRef _duree As Integer)
        Dim present As Boolean
        Dim i As Integer
        'On parcourt la situation du poste pour vérifier si le matricule saisi est bien présent sur ce poste (contrairement à la V6 on peut faire une sortie de n'importe quel type d'opérateur)
        For i = 0 To WSsp.GRP2.Count - 1
            If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                _textBoxNom = WSsp.GRP2(i).ZEMPDES
                _duree = CInt(Calc_Duree(WSsp.GRP2(i).XDATENT, WSsp.GRP2(i).XTIMENT, Now, 2))
                present = True
                Exit For
            End If
        Next
        If present = False Then
            _msgErr = "Matricule n° " & _matr.ToString & " non présent sur ce poste !"
        End If
    End Sub
    'fonction qui renvoie le type de matricule 
    Public Function TypeMatr(ByVal _matr As Integer) As Integer
        Dim typem As Integer
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    typem = WSsp.GRP2(i).XTYPE
                    Exit For
                End If
            Next
        End If
        Return typem
    End Function
    Public Sub DureeMaxPresenceDepassee(ByRef _msgErr As String, Optional ByVal _afficheMsg As Boolean = True)
        Dim i As Integer
        Dim HeuresPresence As Decimal
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 Then
                    HeuresPresence = Calc_Duree(WSsp.GRP2(i).XDATENT, WSsp.GRP2(i).XTIMENT, Now, 1)
                    If HeuresPresence >= MaxHeuresPresence Then
                        If _afficheMsg Then
                            MsgBox("L'opérateur " & WSsp.GRP2(i).ZEMPDES & " (" & WSsp.GRP2(i).XEMPNUM.ToString & ") est présent sur ce poste depuis " &
                                   "plus de " & MaxHeuresPresence.ToString & " heures. Vous devez saisir le départ !", MsgBoxStyle.Exclamation, "Temps maxi de présence dépassé !")
                        End If
                        _msgErr = "Temps maxi de présence dépassé pour le matricule " & WSsp.GRP2(i).XEMPNUM.ToString & " !"
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub
    Public Sub OpHof(ByVal _matr As Integer, ByRef _msgErr As String)
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    If WSsp.GRP2(i).XEVENT = 1300 Then
                        _msgErr = "Vous devez terminer l'opération hors OF (touche <H>) !"
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub
    Public Sub OpEnCours(ByVal _matr As Integer, ByRef _msgErr As String)
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    If WSsp.GRP2(i).XMFGNUM <> "" OrElse WSsp.GRP2(i).ZTYPOPHOF > 0 Then
                        _msgErr = "Une opération est déjà en cours : veuillez saisir d'abord la fin d'opération."
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub
    Public Sub OFOpMatr(ByVal _matr As Integer, ByRef _of As String, ByRef _op As String, ByRef _msgErr As String)
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    If WSsp.GRP2(i).XMFGNUM <> "" And WSsp.GRP2(i).XOPENUM > 0 Then
                        _of = WSsp.GRP2(i).XMFGNUM
                        _op = WSsp.GRP2(i).XOPENUM.ToString
                        Exit For
                    End If
                End If
            Next
        End If
        If _of = "" Then
            _msgErr = "Aucune opération cours !"
        End If
    End Sub
    Public Function OpExc(ByVal _matr As Integer) As Integer
        Dim nopexc As Integer
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    nopexc = WSsp.GRP2(i).ZOPEXC
                    Exit For
                End If
            Next
        End If
        Return nopexc
    End Function
    Public Function PhaseEnCours(ByVal _matr As Integer, ByVal _of As String, ByVal _op As Integer) As Integer
        Dim nphas As Integer
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    If WSsp.GRP2(i).XMFGNUM = _of And WSsp.GRP2(i).XOPENUM = _op Then
                        nphas = WSsp.GRP2(i).XPHASE
                        Exit For
                    End If
                End If
            Next
        End If
        Return nphas
    End Function
    Public Function EventEnCours(ByVal _matr As Integer) As Integer
        Dim xevent As Integer
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    If WSsp.GRP2(i).XEVENT > 0 Then
                        xevent = WSsp.GRP2(i).XEVENT
                        Exit For
                    End If
                End If
            Next
        End If
        Return xevent
    End Function
    'Fonction qui contrôle s'il y a un événement obligatoire
    Public Sub EventOblig(ByVal _matr As Integer, ByRef _msgErr As String)
        Dim xevtobl As Integer
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    If WSsp.GRP2(i).XEVENT > 0 Then
                        xevtobl = WSsp.GRP2(i).ZEVTOBL
                        Exit For
                    End If
                End If
            Next
            If xevtobl > 0 Then
                _msgErr = "Phase %1% obligatoire !"
                'Recherche du libellé dans la liste des phases du poste
                For Each phs As Phase In Phases.OrderBy(Function(x) x.ordre)
                    If phs.phase = xevtobl Then
                        _msgErr = _msgErr.Replace("%1%", phs.desc)
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub
    'Fonction qui renvoie Vrai si l'on est à une étape de production
    Public Function EtapePro(ByVal _of As String, ByVal _op As Integer) As Boolean
        Dim etppro As Boolean
        Dim i As Integer
        If WSof.GRP2.Count > 0 Then
            For i = 0 To WSof.GRP2.Count - 1
                If WSof.GRP2(i).XMFGNUM = _of AndAlso WSof.GRP2(i).XOPENUM = _op Then
                    If WSof.GRP2(i).ZMFGMST = 2 Then
                        etppro = True
                    Else
                        etppro = False
                    End If
                    Exit For
                End If
            Next
        End If
        Return etppro
    End Function
    'Fonction qui renvoie le nombre d'articles produits sur l'OF
    Public Function NbArt(ByVal _of As String, ByVal _op As Integer) As Integer
        Dim nbitm As Integer
        Dim i As Integer
        If WSof.GRP2.Count > 0 Then
            For i = 0 To WSof.GRP2.Count - 1
                If WSof.GRP2(i).XMFGNUM = _of AndAlso WSof.GRP2(i).XOPENUM = _op Then
                    If WSof.GRP2(i).ZTCLCOD <> "EP" Then
                        nbitm += 1
                    End If
                End If
            Next
        End If
        Return nbitm
    End Function
    'Fonction qui renvoie l'unité de fabrication
    Public Function UnitFab(ByVal _matr As Integer) As String
        Dim uom As String = String.Empty
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    If WSsp.GRP2(i).ZOPEXC > 0 Then
                        uom = WSsp.GRP2(i).ZPREUOM
                    Else
                        uom = WSsp.GRP2(i).ZOPEUOM
                    End If
                    Exit For
                End If
            Next
        End If
        Return uom
    End Function
    'Fonction qui convertit l'unité pour affichage dans un format lisible pour l'opérateur
    Public Function AffUnit(ByVal _unit As String) As String
        Dim unit As String = String.Empty
        Select Case _unit
            Case "MLF"
                unit = "ML"
            Case "M2"
                unit = "M²"
            Case Else
                unit = _unit
        End Select

        Return unit
    End Function
    'on contrôle si un événement en cours bloque la sortie opérateur
    Public Sub EventEnCoursSortie(ByVal _matr As Integer, ByRef _xevent As Integer, ByRef _msgErr As String)
        Dim i As Integer
        If WSsp.GRP2.Count > 0 Then
            For i = 0 To WSsp.GRP2.Count - 1
                If WSsp.GRP2(i).XEMPNUM > 0 AndAlso WSsp.GRP2(i).XEMPNUM = _matr Then
                    If WSsp.GRP2(i).XEVENT > 0 AndAlso (WSsp.GRP2(i).XEVENT <> 1000 And WSsp.GRP2(i).XEVENT <> 1100 And WSsp.GRP2(i).XEVENT <> 1090) Then
                        _msgErr = "L'évenement <" & IIf(WSsp.GRP2(i).ZOPEXC > 0, WSsp.GRP2(i).ZROODES, WSsp.GRP2(i).ZEVTDES).ToString & "> est en cours."
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub
    Public Sub WaitGif(ByVal _onoff As Boolean, Optional ByVal _wait As Integer = 10)
        If _onoff Then
            With PictureLoad
                .Width = DataGridViewSitOpe.Height
                .Height = DataGridViewSitOpe.Height
            End With
            With PictureLoad
                .Location = New Point(CInt((DataGridViewSitOpe.Width / 2) - (PictureLoad.Width / 2)), CInt((DataGridViewSitOpe.Height / 2) - (PictureLoad.Height / 2)))
                .Visible = True
            End With
            Application.DoEvents()
        Else
            TimerLoad.Interval = _wait
            TimerLoad.Start()
        End If
    End Sub

    Private Sub TimerLoad_Tick(sender As Object, e As EventArgs) Handles TimerLoad.Tick
        PictureLoad.Visible = False
        Me.TimerLoad.Stop()
        Application.DoEvents()

    End Sub

    '280321PNO.sn
    Private Sub Lab1Sitpost_DoubleClick(sender As Object, e As EventArgs) Handles Lab1Sitpost.DoubleClick
        'affichage le load dans 100 ms
        Call WaitGif(True, 100)

        'Situation du poste
        SituationPoste()

        'on masque le gif load dans 1 seconde
        Call WaitGif(False, 1000)
    End Sub

    Private Sub Lab1SitOp_DoubleClick(sender As Object, e As EventArgs) Handles Lab1SitOp.DoubleClick
        'affichage le load dans 100 ms
        Call WaitGif(True, 100)

        'situation des OF/OP
        SituationOfOp()

        'on masque le gif load dans 1 seconde
        Call WaitGif(False, 1000)
    End Sub

    Private Sub Lab1Sitmat_DoubleClick(sender As Object, e As EventArgs) Handles Lab1Sitmat.DoubleClick
        'affichage le load dans 100 ms
        Call WaitGif(True, 100)

        'situation des composants
        SituationCps()

        'on masque le gif load dans 1 seconde
        Call WaitGif(False, 1000)
    End Sub
    '280321PNO.en
End Class



