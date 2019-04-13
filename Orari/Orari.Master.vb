Imports System.IO
Imports System.Drawing

Public Class Orari
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim VisuaMeseSotto As Boolean = True

        Entrato = False

        If Page.IsPostBack = False Then
            Dim NomePagina As String = Request.CurrentExecutionFilePath.ToUpper.Trim

            NomePagina = Mid(NomePagina, 1, InStr(NomePagina, ".") - 1)
            NomePagina = Mid(NomePagina, 2, Len(NomePagina))
            NomePagina = NomePagina.Replace("ORARI/", "")

            nomeutente.Visible = True

            divPostit.Visible = True
            divPenna.Visible = True
            divFloppy.Visible = True
            divSquadretta.Visible = True
            divUsb.Visible = True
            divFoto.Visible = True
            divGraffetta.Visible = True
            vistamese.Visible = True

            Select Case NomePagina
                Case "AGGDATI"
                    lblTitoloMaschera.Text = "Aggiorna dati database"
                    divTitolo.Visible = True
                Case "DEFAULT"
                    divTitolo.Visible = False
                    nomeutente.Visible = False
                    divPostit.Visible = False
                    divPenna.Visible = False
                    divFloppy.Visible = False
                    divSquadretta.Visible = False
                    divUsb.Visible = False
                    divFoto.Visible = False
                    divGraffetta.Visible = False
                    vistamese.Visible = False
                    VisuaMeseSotto = False
                Case "PRINCIPALE"
                    divTitolo.Visible = False
                    ImpostaPosizioneFoto(15, 315, 10, 335)
                Case "GESTIONEABITAZIONI"
                    lblTitoloMaschera.Text = "Gestione tabella abitazioni"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 125, 5, 150)
                Case "GESTIONECOMMESSE"
                    lblTitoloMaschera.Text = "Gestione tabella commesse"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 125, 5, 150)
                Case "GESTIONELINGUAGGI"
                    lblTitoloMaschera.Text = "Gestione linguaggi"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 5, 5, 30)
                Case "GESTIONEMEZZI"
                    lblTitoloMaschera.Text = "Gestione tabella mezzi e standard"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 5, 5, 30)
                Case "GESTIONEPASTICCHE"
                    lblTitoloMaschera.Text = "Gestione tabella pasticche"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 5, 5, 30)
                Case "GESTIONEPORTATE"
                    lblTitoloMaschera.Text = "Gestione tabella portate"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 5, 5, 30)
                Case "GESTIONESOCIETA"
                    lblTitoloMaschera.Text = "Gestione tabella società"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 125, 5, 150)
                Case "GESTIONESTUDI"
                    lblTitoloMaschera.Text = "Gestione tabella studi"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 125, 5, 150)
                Case "GESTIONETABELLE"
                    lblTitoloMaschera.Text = "Gestione tabelle"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 125, 5, 150)
                Case "GESTIONETEMPO"
                    lblTitoloMaschera.Text = "Gestione tabella tempo"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 5, 5, 30)
                Case "IMPOSTAZIONI"
                    lblTitoloMaschera.Text = "Impostazioni"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 175, 5, 200)
                Case "MAPPA"
                    lblTitoloMaschera.Text = "Mappa"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 5, 5, 30)
                Case "MODIFICAGIORNO"
                    lblTitoloMaschera.Text = "Modifica dati giornata"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(10, 105, 5, 130)
                Case "PULIZIAPREGRESSO"
                    lblTitoloMaschera.Text = "Pulizia dati mesi pregressi"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(15, 175, 5, 200)
                Case "STATISTICHE"
                    lblTitoloMaschera.Text = "Statistiche"
                    divTitolo.Visible = True
                    ImpostaPosizioneFoto(10, 285, 6, 315)
                Case Else
                    lblTitoloMaschera.Text = NomePagina
                    divTitolo.Visible = True
            End Select

            lblNomeUtente.Text = "Utente: " & Utenza & "<hr />" & ScriveCredenziali()

            If Utenza <> "" Then
                ControllaImmagine()
            End If

            If VisuaMeseSotto = True Then
                Disegnamese()

                'vistamese.Style("height") = "31px;"
                'imgChiude.ImageUrl = "App_Themes/Standard/Images/icona_SU.png"
                'divCalendario.Visible = False
                'MeseVisualizzato = False

                CambiaVisMese()
            End If
        End If

        Entrato = True
    End Sub

    Private Sub ImpostaPosizioneFoto(Y1 As Integer, X1 As Integer, Y2 As Integer, X2 As Integer)
        divFoto.Style("left") = X1.ToString.Trim & "px"
        divFoto.Style("top") = Y1.ToString.Trim & "px"
        divGraffetta.Style("left") = X2.ToString.Trim & "px"
        divGraffetta.Style("top") = Y2.ToString.Trim & "px"
    End Sub

    Private Sub ControllaImmagine()
        Dim Nome As String = Server.MapPath(".") & "\App_Themes\Standard\images\" & idUtente.ToString.Trim & "-" & Utenza & "\Immagine.png"
        Dim NomeFisico As String = "App_Themes/Standard/Images/" & idUtente.ToString.Trim & "-" & Utenza & "/Immagine.png"

        If File.Exists(Nome) = True Then
            imgFoto.ImageUrl = NomeFisico
        Else
            FileCopy(Server.MapPath(".") & "\App_Themes\Standard\images\Sconosciuto.jpg", Nome)

            RuotaFoto(Nome)

            imgFoto.ImageUrl = NomeFisico
        End If
    End Sub

    Private Function ScriveCredenziali() As String
        Dim Ritorno As String = ""

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & prefissotabelle & "Impostazioni Where idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                Ritorno = Rec("Nome").Value & " " & Rec("Cognome").Value & " (" & Rec("EMail").Value & ")"
            End If
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Utenti Where idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                Dim Tipo As String = ""
                Dim Pre As String = ""

                If Ritorno <> "" Then
                    Pre = "<br />"
                End If

                Select Case Rec("Permessi").Value.ToString.Trim.ToUpper
                    Case "A"
                        Tipo = "Amministratore"
                    Case "U"
                        Tipo = "Utente"
                End Select

                Ritorno += Pre & Tipo
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Return Ritorno
    End Function

    Protected Sub cmdOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdOK.Click
        divBloccaFinestra.Visible = False
        divPopup.Visible = False
    End Sub

    Protected Sub imgChiude_Click(sender As Object, e As ImageClickEventArgs) Handles imgChiude.Click
        Dim Scrivi As String
        Dim g As New GestioneFilesDirectory

        If MeseVisualizzato = False Then
            Scrivi = "Aperto"
            MeseVisualizzato = True
        Else
            Scrivi = "Chiuso"
            MeseVisualizzato = False
        End If
        g.CreaAggiornaFileVisMese(NomeFileMese, Scrivi)
        g = Nothing

        CambiaVisMese()
    End Sub

    Private Sub CambiaVisMese()
        If MeseVisualizzato = False Then
            vistamese.Style("height") = "31px;"
            imgChiude.ImageUrl = "App_Themes/Standard/Images/icona_SU.png"
            MeseVisualizzato = False
            divCalendario.Visible = False
        Else
            vistamese.Style("height") = Session("AltezzaCal") & "px;"
            imgChiude.ImageUrl = "App_Themes/Standard/Images/icona_GIU.png"
            MeseVisualizzato = True
            divCalendario.Visible = True
        End If

        If Entrato = True Then
            Response.Redirect("Principale.aspx?Mese=" & NumeroMese & "&Anno=" & NumeroAnno)
        End If
    End Sub

    Private Sub Disegnamese()
        Dim CmbAnno As DropDownList = cphCorpo.FindControl("cmbAnno")
        Dim CmbMese As DropDownList = cphCorpo.FindControl("cmbMese")
        If CmbMese Is Nothing = True Then
            vistamese.Visible = False
            Exit Sub
        End If
        Dim sMese As String = CmbMese.Text
        Dim NumMese As Integer = -1
        For i As Integer = 1 To 12
            If sMese = NomeMesi(i) Then
                NumMese = i
                Exit For
            End If
        Next
        Dim Mese As Integer = NumMese
        Dim Anno As Integer = CmbAnno.Text
        Dim PrimoGiornoMese As Date = "01/" & Format(Mese, "00") & "/" & Anno.ToString.Trim
        Dim MesePrima As Integer = Mese - 1
        Dim AnnoPrima As Integer = Anno
        If MesePrima < 1 Then
            MesePrima = 12
            AnnoPrima -= 1
        End If
        Dim GiorniMesePrima As Integer = DateTime.DaysInMonth(AnnoPrima, MesePrima)
        Dim GiorniMese As Integer = DateTime.DaysInMonth(Anno, Mese)
        Dim Primo As Integer = PrimoGiornoMese.DayOfWeek + 1
        Dim NomiEtichette(42) As Label
        Dim NomiEtichetteD(42) As Label
        Dim Cella(42) As HtmlControls.HtmlTableCell

        Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String = "Select * From " & prefissotabelle & "Orari Where Mese=" & Mese & " And Anno=" & Anno
        Dim Ore(35) As Single
        Dim v As String

        Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
        Do Until Rec.Eof
            v = "" & Rec("Quanto").Value
            If v = "" Then v = "0"
            Ore(Rec("Giorno").Value) = v

            Rec.MoveNext()
        Loop
        Rec.Close()

        ConnSQL.Close()

        NomiEtichette(1) = lbl1
        NomiEtichette(2) = lbl2
        NomiEtichette(3) = lbl3
        NomiEtichette(4) = lbl4
        NomiEtichette(5) = lbl5
        NomiEtichette(6) = lbl6
        NomiEtichette(7) = lbl7

        NomiEtichette(8) = lbl8
        NomiEtichette(9) = lbl9
        NomiEtichette(10) = lbl10
        NomiEtichette(11) = lbl11
        NomiEtichette(12) = lbl12
        NomiEtichette(13) = lbl13
        NomiEtichette(14) = lbl14

        NomiEtichette(15) = lbl15
        NomiEtichette(16) = lbl16
        NomiEtichette(17) = lbl17
        NomiEtichette(18) = lbl18
        NomiEtichette(19) = lbl19
        NomiEtichette(20) = lbl20
        NomiEtichette(21) = lbl21

        NomiEtichette(22) = lbl22
        NomiEtichette(23) = lbl23
        NomiEtichette(24) = lbl24
        NomiEtichette(25) = lbl25
        NomiEtichette(26) = lbl26
        NomiEtichette(27) = lbl27
        NomiEtichette(28) = lbl28

        NomiEtichette(29) = lbl29
        NomiEtichette(30) = lbl30
        NomiEtichette(31) = lbl31
        NomiEtichette(32) = lbl32
        NomiEtichette(33) = lbl33
        NomiEtichette(34) = lbl34
        NomiEtichette(35) = lbl35

        NomiEtichette(36) = lbl36
        NomiEtichette(37) = lbl37
        NomiEtichette(38) = lbl38
        NomiEtichette(39) = lbl39
        NomiEtichette(40) = lbl40
        NomiEtichette(41) = lbl41
        NomiEtichette(42) = lbl42

        NomiEtichetteD(1) = lbl1d
        NomiEtichetteD(2) = lbl2d
        NomiEtichetteD(3) = lbl3d
        NomiEtichetteD(4) = lbl4d
        NomiEtichetteD(5) = lbl5d
        NomiEtichetteD(6) = lbl6d
        NomiEtichetteD(7) = lbl7d

        NomiEtichetteD(8) = lbl8d
        NomiEtichetteD(9) = lbl9d
        NomiEtichetteD(10) = lbl10d
        NomiEtichetteD(11) = lbl11d
        NomiEtichetteD(12) = lbl12d
        NomiEtichetteD(13) = lbl13d
        NomiEtichetteD(14) = lbl14d

        NomiEtichetteD(15) = lbl15d
        NomiEtichetteD(16) = lbl16d
        NomiEtichetteD(17) = lbl17d
        NomiEtichetteD(18) = lbl18d
        NomiEtichetteD(19) = lbl19d
        NomiEtichetteD(20) = lbl20d
        NomiEtichetteD(21) = lbl21d

        NomiEtichetteD(22) = lbl22d
        NomiEtichetteD(23) = lbl23d
        NomiEtichetteD(24) = lbl24d
        NomiEtichetteD(25) = lbl25d
        NomiEtichetteD(26) = lbl26d
        NomiEtichetteD(27) = lbl27d
        NomiEtichetteD(28) = lbl28d

        NomiEtichetteD(29) = lbl29d
        NomiEtichetteD(30) = lbl30d
        NomiEtichetteD(31) = lbl31d
        NomiEtichetteD(32) = lbl32d
        NomiEtichetteD(33) = lbl33d
        NomiEtichetteD(34) = lbl34d
        NomiEtichetteD(35) = lbl35d

        NomiEtichetteD(36) = lbl36d
        NomiEtichetteD(37) = lbl37d
        NomiEtichetteD(38) = lbl38d
        NomiEtichetteD(39) = lbl39d
        NomiEtichetteD(40) = lbl40d
        NomiEtichetteD(41) = lbl41d
        NomiEtichetteD(42) = lbl42d

        Cella(1) = cellaCal1
        Cella(2) = cellaCal2
        Cella(3) = cellaCal3
        Cella(4) = cellaCal4
        Cella(5) = cellaCal5
        Cella(6) = cellaCal6
        Cella(7) = cellaCal7

        Cella(8) = cellaCal8
        Cella(9) = cellaCal9
        Cella(10) = cellaCal10
        Cella(11) = cellaCal11
        Cella(12) = cellaCal12
        Cella(13) = cellaCal13
        Cella(14) = cellaCal14

        Cella(15) = cellaCal15
        Cella(16) = cellaCal16
        Cella(17) = cellaCal17
        Cella(18) = cellaCal18
        Cella(19) = cellaCal19
        Cella(20) = cellaCal20
        Cella(21) = cellaCal21

        Cella(22) = cellaCal22
        Cella(23) = cellaCal23
        Cella(24) = cellaCal24
        Cella(25) = cellaCal25
        Cella(26) = cellaCal26
        Cella(27) = cellaCal27
        Cella(28) = cellaCal28

        Cella(29) = cellaCal29
        Cella(30) = cellaCal30
        Cella(31) = cellaCal31
        Cella(32) = cellaCal32
        Cella(33) = cellaCal33
        Cella(34) = cellaCal34
        Cella(35) = cellaCal35

        Cella(36) = cellaCal36
        Cella(37) = cellaCal37
        Cella(38) = cellaCal38
        Cella(39) = cellaCal39
        Cella(40) = cellaCal40
        Cella(41) = cellaCal41
        Cella(42) = cellaCal42

        lblMese.Text = NomeMesi(Mese) & " " & Anno

        Dim Giorno As Integer = 0

        Giorno = GiorniMesePrima - (Primo - 1)
        For i As Integer = 1 To Primo - 1
            NomiEtichette(i).Visible = True
            NomiEtichetteD(i).Visible = True
            Giorno += 1
            NomiEtichette(i).Text = ""
            NomiEtichetteD(i).Text = Giorno
            NomiEtichetteD(i).Style("color") = "#BBBBBB;"
            Cella(i).Visible = True
        Next

        Dim Datella As Date

        Giorno = 0
        For i As Integer = Primo To Primo + GiorniMese - 1
            Giorno += 1

            Datella = Giorno.ToString.Trim & "/" & Mese.ToString.Trim & "/" & Anno.ToString.Trim

            If ControllaFestivo(Datella) = True Then
                Cella(i).Style("background-color") = "#FFAAAA;"
            Else
                Cella(i).Style("background-color") = "#DDDDDD;"
            End If

            NomiEtichetteD(i).Style("color") = "#666666;"

            NomiEtichette(i).Visible = True
            NomiEtichette(i).Text = Giorno
            NomiEtichetteD(i).Visible = True
            Cella(i).Visible = True
            Select Case Ore(Giorno)
                Case Is > 0
                    NomiEtichetteD(i).Text = Ore(Giorno)
                Case -1
                    NomiEtichetteD(i).Text = "F"
                Case -2
                    NomiEtichetteD(i).Text = "M"
                Case -3
                    NomiEtichetteD(i).Text = "P"
                Case -4
                    NomiEtichetteD(i).Text = "S"
                Case 0
                    NomiEtichetteD(i).Text = ""
            End Select
        Next

        Primo += GiorniMese

        Dim Fine As Integer
        Dim Righe As Integer

        Select Case Primo
            Case 0 To 7
                Fine = 7
                Righe = 1
            Case 8 To 14
                Fine = 14
                Righe = 2
            Case 15 To 21
                Fine = 21
                Righe = 3
            Case 22 To 28
                Fine = 28
                Righe = 4
            Case 29
                Fine = 28
                Righe = 4
            Case 30 To 35
                Fine = 35
                Righe = 5
            Case 36
                Fine = 35
                Righe = 4
            Case 37 To 42
                Fine = 42
                Righe = 6
        End Select

        Giorno = 0
        For i As Integer = Primo To Fine
            NomiEtichette(i).Visible = True
            NomiEtichetteD(i).Visible = True
            Giorno += 1
            NomiEtichette(i).Text = ""
            NomiEtichetteD(i).Text = Giorno
            NomiEtichetteD(i).Style("color") = "#BBBBBB;"
            Cella(i).Visible = True
        Next

        Session("AltezzaCal") = (Righe * 38) + 80

        For i As Integer = Fine + 1 To 42
            NomiEtichette(i).Visible = False
            NomiEtichetteD(i).Visible = False
            Cella(i).Visible = False
        Next
    End Sub
End Class