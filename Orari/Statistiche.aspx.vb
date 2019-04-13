Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Threading

Public Class Statistiche
    Inherits System.Web.UI.Page

    Public rigaR As DataRow
    Public dttTabella As New DataTable()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente = 0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            CaricaCombo()

            If cmbSocieta.Text = "" Then
                chkSplitta.Visible = True
            Else
                chkSplitta.Visible = False
            End If

            SpegneDiv()
        End If
    End Sub

    Protected Sub CambiaSocieta()
        If cmbSocieta.Text = "" Then
            chkSplitta.Visible = True
        Else
            chkSplitta.Visible = False
            chkSplitta.Checked = True
        End If

        Select Case ModalitaStatistica
            Case "STATCOMMESSE"
                CaricaStatCommesse()
            Case "GIORNILAVORATI"
                CaricaGiorniLavorati()
            Case "GIORNIFERIE"
                CaricaGiorniFerie()
            Case "GIORNILAVOROCASA"
                CaricaGiorniLavoroCasa()
            Case "GIORNIMALATTIA"
                CaricaGiorniMalattia()
            Case "PERMESSI"
                CaricaPermessi()
            Case "OREMALATTIA"
                CaricaOreDiMalattia()
            Case "OREPERMESSO"
                CaricaOreDiPermesso()
            Case "PORTATE"
                CaricaPortate()
            Case "PASTICCHE"
                CaricaPasticche()
            Case "MEZZIANDATA"
                CaricaMezziDiAndata()
            Case "MEZZIRITORNO"
                CaricaMezziDiRitorno()
            Case "MINIMOENTRATA"
                CaricaMinimoEntrata()
            Case "STRAORDINARI"
                CaricaStraordinari()
            Case "PERIODOLAVORATIVO"
                CaricaPeriodoLavorativo()
            Case "KM"
                CaricaKM()
        End Select
    End Sub

    Private Sub grdStatistiche_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStatistiche.PageIndexChanging
        grdStatistiche.PageIndex = e.NewPageIndex
        grdStatistiche.DataBind()

        Select Case ModalitaStatistica
            Case "GIORNILAVORATI"
                CaricaStatisticheGL(SocietaSel, GiornoSel, MeseSel)
            Case "GIORNIFERIE"
                CaricaStatisticheFerie(SocietaSel, GiornoSel, MeseSel)
            Case "GIORNILAVOROCASA"
                CaricaStatisticheLavoroCasa(SocietaSel, GiornoSel, MeseSel)
            Case "GIORNIMALATTIA"
                CaricaStatisticheMalattia(SocietaSel, GiornoSel, MeseSel)
            Case "PERMESSI"
                CaricaStatistichePermessi(SocietaSel, GiornoSel, MeseSel)
            Case "OREMALATTIA"
                CaricaStatisticheOreMalattia(SocietaSel, GiornoSel, MeseSel)
            Case "OREPERMESSO"
                CaricaStatisticheOrePermesso(SocietaSel, GiornoSel, MeseSel)
            Case "PORTATE"
                CaricaStatistichePortate(GiornoSel, MeseSel)
            Case "PASTICCHE"
                CaricaStatistichePasticca(GiornoSel, MeseSel)
            Case "MEZZIANDATA"
                CaricaStatisticheMezziDiAndata(GiornoSel, MeseSel)
            Case "MEZZIRITORNO"
                CaricaStatisticheMezziDiRitorno(GiornoSel, MeseSel)
        End Select
    End Sub

    Private Sub CaricaCombo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idLavoroDefault As Integer
            Dim LavDef As String = ""

            Sql = "Select * From " & PrefissoTabelle & "LavoroDefault Where idUtente=" & idUtente & " "
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                idLavoroDefault = 2
            Else
                idLavoroDefault = Rec("idLavoro").Value
            End If
            Rec.Close()

            cmbSocieta.Items.Clear()

            Sql = "Select * From " & PrefissoTabelle & "Lavori Where idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbSocieta.Items.Add("")
            Do Until Rec.Eof
                cmbSocieta.Items.Add(Rec("Lavoro").Value)
                If Rec("idLavoro").Value = idLavoroDefault Then
                    LavDef = Rec("Lavoro").Value
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            cmbSocieta.Text = LavDef

            ConnSQL.Close()
        End If
    End Sub

    Private Sub SpegneDiv()
        ModalitaStatistica = ""
        GiornoSel = ""
        MeseSel = ""
        SocietaSel = ""

        divStatistiche.Visible = False

        divStatCommesse.Visible = False
        divGiorniLavorati.Visible = False
        divFerie.Visible = False
        divMalattia.Visible = False
        divPermessi.Visible = False
        divOreMalattia.Visible = False
        divOrePermesso.Visible = False
        divStatPranzi.Visible = False
        divStatPasticche.Visible = False
        divStatMezziAndata.Visible = False
        divStatMezziRitorno.Visible = False
        divMinimoEntrata.Visible = False
        divStraordinari.Visible = False
        divPeriodoLavorativo.Visible = False
        divKM.Visible = False
        divGradi.Visible = False
        divGradiDett.Visible = False
        divEntrate.Visible = False
        divCurriculum.Visible = False
        divLavoroACasa.Visible = False
    End Sub

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        Response.Redirect("Principale.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgStatCommesse_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatCommesse.Click
        SpegneDiv()
        ModalitaStatistica = "STATCOMMESSE"
        divStatCommesse.Visible = True
        lblTitoloMaschera.Text = "Statistica tempi commesse"

        CaricaStatCommesse()
    End Sub

    ' TEMPI COMMESSE

    Private Sub CaricaStatCommesse()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dSoc As New DataColumn("Societa")
            Dim dCom As New DataColumn("Commessa")
            Dim dOre As New DataColumn("Ore")
            Dim dGiorni As New DataColumn("Giorni")
            Dim dKm As New DataColumn("Km")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Ore As Single = 0
            Dim Giorni As Single = 0
            Dim Km As Single = 0

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dSoc)
            dttTabella.Columns.Add(dCom)
            dttTabella.Columns.Add(dOre)
            dttTabella.Columns.Add(dGiorni)
            dttTabella.Columns.Add(dKm)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And C.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "C.Lavoro, "
                Splitta2 = "C.Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta & " B.Descrizione, Sum(Quanto) As Ore, Sum(Quanto)/8 As Giorni, Sum(A.Km)*2 as Km  From " & PrefissoTabelle & "Orari A " &
                "Left Join " & PrefissoTabelle & "Commesse B On A.CodCommessa = B.Codice And B.idUtente = A.idUtente And A.idLavoro = B.idLavoro " &
                "Left Join " & PrefissoTabelle & "Lavori C On A.idUtente = C.idUtente And A.idLavoro = C.idLavoro " &
                "Where " &
                "A.idUtente = " & idUtente & " And (Quanto>0 Or Quanto=-6) " &
                " " & Altro & " " &
                "Group By " & Splitta2 & " B.Descrizione " &
                "Order By 4 Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Ore").Value Then
                    Vecchio = Rec("Ore").Value
                    Contatore += 1
                End If

                Ore += Val("" & Rec("Ore").Value.ToString.Replace(",", "."))
                Giorni += Val("" & Rec("Giorni").Value.ToString.Replace(",", "."))
                Km += Val("" & Rec("Km").Value.ToString.Replace(",", "."))

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Descrizione").Value
                rigaR(3) = ScriveNumero("" & Rec("Ore").Value)
                rigaR(4) = ScriveNumero("" & Rec("Giorni").Value)
                rigaR(5) = ScriveNumero("" & Rec("Km").Value)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            rigaR = dttTabella.NewRow()
            rigaR(0) = ""
            rigaR(1) = ""
            rigaR(2) = ""
            rigaR(3) = "------------"
            rigaR(4) = "------------"
            rigaR(5) = "------------"
            dttTabella.Rows.Add(rigaR)

            rigaR = dttTabella.NewRow()
            rigaR(0) = ""
            rigaR(1) = ""
            rigaR(2) = ""
            rigaR(3) = ScriveNumero(Ore)
            rigaR(4) = ScriveNumero(Giorni)
            rigaR(5) = ScriveNumero(Km)
            dttTabella.Rows.Add(rigaR)

            grdStatCommesse.DataSource = dttTabella
            grdStatCommesse.DataBind()
            grdStatCommesse.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdStatCommesse_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStatCommesse.PageIndexChanging
        grdStatCommesse.PageIndex = e.NewPageIndex
        grdStatCommesse.DataBind()

        CaricaStatCommesse()
    End Sub

    ' GIORNI LAVORATI

    Protected Sub imgGiorniLavorati_Click(sender As Object, e As ImageClickEventArgs) Handles imgGiorniLavorati.Click
        SpegneDiv()
        ModalitaStatistica = "GIORNILAVORATI"
        divGiorniLavorati.Visible = True
        lblTitoloMaschera.Text = "Statistica giorni lavorati"

        CaricaGiorniLavorati()
    End Sub

    Private Sub CaricaGiorniLavorati()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dGiorno As New DataColumn("Giorno")
            Dim dMese As New DataColumn("Mese")
            Dim dOre As New DataColumn("Ore")
            Dim dGiorni As New DataColumn("Giorni")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dGiorno)
            dttTabella.Columns.Add(dMese)
            dttTabella.Columns.Add(dOre)
            dttTabella.Columns.Add(dGiorni)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "B.Lavoro, "
                Splitta2 = "B.Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta & " Giorno, Mese, Sum(Quanto) As Ore, Sum(Quanto)/8 As Giorni From " &
                "" & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Where A.idUtente = " & idUtente & " And (Quanto > 0 Or Quanto = -6) " &
                " " & Altro & " " &
                "Group By " & Splitta2 & "Giorno, Mese " &
                "Order By 4 Desc, Mese, Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Ore").Value Then
                    Vecchio = Rec("Ore").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Giorno").Value
                rigaR(3) = "" & Mesi(Rec("Mese").Value - 1)
                rigaR(4) = ScriveNumero("" & Rec("Ore").Value)
                rigaR(5) = ScriveNumero("" & Rec("Giorni").Value)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdGiorniLavorati.DataSource = dttTabella
            grdGiorniLavorati.DataBind()
            grdGiorniLavorati.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdGiorniLavorati_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdGiorniLavorati.PageIndexChanging
        grdGiorniLavorati.PageIndex = e.NewPageIndex
        grdGiorniLavorati.DataBind()

        CaricaGiorniLavorati()
    End Sub

    Private Function PrendeMese(Mese As String) As String
        Select Case Mese
            Case "Gennaio"
                Return 1
            Case "Febbraio"
                Return 2
            Case "Marzo"
                Return 3
            Case "Aprile"
                Return 4
            Case "Maggio"
                Return 5
            Case "Giugno"
                Return 6
            Case "Luglio"
                Return 7
            Case "Agosto"
                Return 8
            Case "Settembre"
                Return 9
            Case "Ottobre"
                Return 10
            Case "Novembre"
                Return 11
            Case "Dicembre"
                Return 12
            Case Else
                Return -1
        End Select
    End Function

    Protected Sub StatisticheGiorniLavorati(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdGiorniLavorati.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Giorno As String = Di.Cells(2).Text
        Dim Mese As String = Di.Cells(3).Text

        GiornoSel = Giorno
        MeseSel = Mese
        SocietaSel = Societa

        CaricaStatisticheGL(Societa, Giorno, Mese)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatisticheGL(Societa As String, Giorno As String, Mese As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatistiche.Text = "Statistiche su giorni lavorati: " & Giorno & " " & Mese

            Dim sMese As String = PrendeMese(Mese)

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And B.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select *, B.Lavoro From " & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And " &
                "Giorno = " & Giorno & " And " &
                "Mese = " & sMese & " And " &
                "(Quanto>0 Or Quanto=-6) " &
                " " & Altro & " " &
                "Order By Anno Desc, Mese Desc, Giorno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = "" & Rec("Quanto").Value()
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' FERIE

    Protected Sub imgGiorniFerie_Click(sender As Object, e As ImageClickEventArgs) Handles imgGiorniFerie.Click
        SpegneDiv()
        ModalitaStatistica = "GIORNIFERIE"
        divFerie.Visible = True
        lblTitoloMaschera.Text = "Statistica giorni di ferie"

        CaricaGiorniFerie()
    End Sub

    Private Sub CaricaGiorniFerie()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dGiorno As New DataColumn("Giorno")
            Dim dMese As New DataColumn("Mese")
            Dim dOre As New DataColumn("Ore")
            Dim dGiorni As New DataColumn("Giorni")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -2
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dGiorno)
            dttTabella.Columns.Add(dMese)
            dttTabella.Columns.Add(dOre)
            dttTabella.Columns.Add(dGiorni)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "B.Lavoro, "
                Splitta2 = "B.Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta & " Giorno, Mese, Sum(8) As Ore, Sum(8)/8 As Giorni From " &
                "" & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Where A.idUtente = " & idUtente & " And Quanto = -1 " &
                " " & Altro & " " &
                "Group By " & Splitta2 & " Giorno, Mese " &
                "Order By 4 desc, Mese, Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Ore").Value Then
                    Vecchio = Rec("Ore").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Giorno").Value
                rigaR(3) = "" & Mesi(Rec("Mese").Value - 1)
                rigaR(4) = ScriveNumero("" & Rec("Ore").Value)
                rigaR(5) = ScriveNumero("" & Rec("Giorni").Value)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdFerie.DataSource = dttTabella
            grdFerie.DataBind()
            grdFerie.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdFerie_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdFerie.PageIndexChanging
        grdFerie.PageIndex = e.NewPageIndex
        grdFerie.DataBind()

        CaricaGiorniFerie()
    End Sub

    Protected Sub StatisticheGiorniFerie(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdFerie.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Giorno As String = Di.Cells(2).Text
        Dim Mese As String = Di.Cells(3).Text

        GiornoSel = Giorno
        MeseSel = Mese
        SocietaSel = Societa

        CaricaStatisticheFerie(Societa, Giorno, Mese)

        divStatistiche.Visible = True
    End Sub

    Protected Sub StatisticheGiorniLavoroCasa(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdLavoroCasa.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Giorno As String = Di.Cells(2).Text
        Dim Mese As String = Di.Cells(3).Text

        GiornoSel = Giorno
        MeseSel = Mese
        SocietaSel = Societa

        CaricaStatisticheLavoroCasa(Societa, Giorno, Mese)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatisticheFerie(Societa As String, Giorno As String, Mese As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatistiche.Text = "Statistiche su giorni di ferie: " & Giorno & " " & Mese

            Dim sMese As String = PrendeMese(Mese)

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And B.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select *, B.Lavoro From " & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And " &
                "Giorno = " & Giorno & " And " &
                "Mese = " & sMese & " And " &
                "Quanto=-1 " &
                " " & Altro & " " &
                "Order By Anno Desc, Mese Desc, Giorno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = "8"
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' GIORNI MALATTIA

    Protected Sub imgGiorniMalattia_Click(sender As Object, e As ImageClickEventArgs) Handles imgGiorniMalattia.Click
        SpegneDiv()
        ModalitaStatistica = "GIORNIMALATTIA"
        divMalattia.Visible = True
        lblTitoloMaschera.Text = "Statistica giorni di malattia"

        CaricaGiorniMalattia()
    End Sub

    Private Sub CaricaGiorniMalattia()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dGiorno As New DataColumn("Giorno")
            Dim dMese As New DataColumn("Mese")
            Dim dOre As New DataColumn("Ore")
            Dim dGiorni As New DataColumn("Giorni")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dGiorno)
            dttTabella.Columns.Add(dMese)
            dttTabella.Columns.Add(dOre)
            dttTabella.Columns.Add(dGiorni)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "B.Lavoro, "
                Splitta2 = "Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta.Replace("B.", "") & " Giorno, Mese, Sum(O) As Ore, Sum(G) As Giorni From (" &
                "Select " & Splitta & " Giorno, Mese, 8 As O, 8 As G From " &
                "" & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Where A.idUtente = " & idUtente & " And Quanto = -2 " &
                " " & Altro & " " &
                "Union All " &
                "Select " & Splitta.Replace("B.", "") & " Giorno, Mese, (CONVERT(Numeric(4,2), SUBSTRING(sMisti, 1,CHARINDEX(';',sMisti)-1))) As Ore, 8 As Giorni From (" &
                "Select " & Splitta & " A.Giorno, A.Mese, A.Quanto,  SUBSTRING (Misti,CHARINDEX ('M', Misti)+1 ,5) As sMisti From " &
                "" & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And Misti<>'' And Misti Is Not Null And Quanto=-3 " &
                " " & Altro & " " &
                ") C Where CONVERT(Numeric(4,2), SUBSTRING(sMisti, 1,CHARINDEX(';',sMisti)-1))='8' ) D " &
                "Group By " & Splitta2 & " Giorno, Mese " &
                "Order By 4 Desc, Mese, Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Ore").Value Then
                    Vecchio = Rec("Ore").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Giorno").Value
                rigaR(3) = "" & Mesi(Rec("Mese").Value - 1)
                rigaR(4) = ScriveNumero("" & Rec("Ore").Value)
                rigaR(5) = ScriveNumero("" & Rec("Giorni").Value)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdMalattia.DataSource = dttTabella
            grdMalattia.DataBind()
            grdMalattia.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdMalattia_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdMalattia.PageIndexChanging
        grdMalattia.PageIndex = e.NewPageIndex
        grdMalattia.DataBind()

        CaricaGiorniMalattia()
    End Sub

    Protected Sub StatisticheGiorniMalattia(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdMalattia.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Giorno As String = Di.Cells(2).Text
        Dim Mese As String = Di.Cells(3).Text

        GiornoSel = Giorno
        MeseSel = Mese
        SocietaSel = Societa

        CaricaStatisticheMalattia(Societa, Giorno, Mese)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatisticheMalattia(Societa As String, Giorno As String, Mese As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatistiche.Text = "Statistiche su giorni di malattia: " & Giorno & " " & Mese

            Dim sMese As String = PrendeMese(Mese)

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And B.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select * From (" &
                "Select A.*, B.Lavoro From " & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And " &
                "Giorno = " & Giorno & " And " &
                "Mese = " & sMese & " And " &
                "Quanto < 0 And Misti Like '%M8%' " &
                " " & Altro & " " &
                "Union All " &
                "Select A.*, B.Lavoro From " & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And " &
                "Giorno = " & Giorno & " And " &
                "Mese = " & sMese & " And " &
                " " & Altro & " " &
                "Quanto = -2) D " &
                "Order BY Anno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = "8"
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' GIORNI DI PERMESSO

    Protected Sub imgGiorniPermesso_Click(sender As Object, e As ImageClickEventArgs) Handles imgGiorniPermesso.Click
        SpegneDiv()
        ModalitaStatistica = "PERMESSI"
        divPermessi.Visible = True
        lblTitoloMaschera.Text = "Statistica permessi"

        CaricaPermessi()
    End Sub

    Private Sub CaricaPermessi()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dGiorno As New DataColumn("Giorno")
            Dim dMese As New DataColumn("Mese")
            Dim dOre As New DataColumn("Ore")
            Dim dGiorni As New DataColumn("Giorni")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dGiorno)
            dttTabella.Columns.Add(dMese)
            dttTabella.Columns.Add(dOre)
            dttTabella.Columns.Add(dGiorni)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "B.Lavoro, "
                Splitta2 = "Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta.Replace("B.", "") & " Giorno, Mese, SUM(CONVERT(Numeric(4,2), SUBSTRING(sMisti, 1,CHARINDEX(';',sMisti)-1))) As Ore From (" &
                 "Select " & Splitta & " A.Giorno, A.Mese, A.Quanto,  SUBSTRING (Misti,CHARINDEX ('P', Misti)+1 ,5) As sMisti From " &
                 "" & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                 "Where A.idUtente=" & idUtente & " And Misti<>'' And Misti Is Not Null And Quanto=-3 " &
                 " " & Altro & " " &
                 ") C Where CONVERT(Numeric(4,2), SUBSTRING(sMisti, 1,CHARINDEX(';',sMisti)-1))='8' " &
                 "Group By " & Splitta2 & " Giorno, Mese " &
                 "Order By Ore Desc, Mese, Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Ore").Value Then
                    Vecchio = Rec("Ore").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Giorno").Value
                rigaR(3) = "" & Mesi(Rec("Mese").Value - 1)
                rigaR(4) = ScriveNumero("" & Rec("Ore").Value)
                rigaR(5) = ScriveNumero("" & Rec("Ore").Value / 8)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdPermessi.DataSource = dttTabella
            grdPermessi.DataBind()
            grdPermessi.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdPermessi_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPermessi.PageIndexChanging
        grdPermessi.PageIndex = e.NewPageIndex
        grdPermessi.DataBind()

        CaricaPermessi()
    End Sub

    Protected Sub StatisticheGiorniPermessi(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdPermessi.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Giorno As String = Di.Cells(2).Text
        Dim Mese As String = Di.Cells(3).Text

        GiornoSel = Giorno
        MeseSel = Mese
        SocietaSel = Societa

        CaricaStatistichePermessi(Societa, Giorno, Mese)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatistichePermessi(Societa As String, Giorno As String, Mese As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatistiche.Text = "Statistiche su giorni di permesso: " & Giorno & " " & Mese

            Dim sMese As String = PrendeMese(Mese)

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And B.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select *, B.Lavoro From " & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And " &
                "Giorno = " & Giorno & " And " &
                "Mese = " & sMese & " And " &
                "Quanto < 0 And Misti Like '%P8%'  " &
                " " & Altro & " " &
                "Order By Anno Desc, Mese Desc, Giorno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = "8"
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' ORE MALATTIA

    Protected Sub imgOreMalattia_Click(sender As Object, e As ImageClickEventArgs) Handles imgOreMalattia.Click
        SpegneDiv()
        ModalitaStatistica = "OREMALATTIA"
        divOreMalattia.Visible = True
        lblTitoloMaschera.Text = "Statistica ore di malattia"

        CaricaOreDiMalattia()
    End Sub

    Private Sub CaricaOreDiMalattia()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dGiorno As New DataColumn("Giorno")
            Dim dMese As New DataColumn("Mese")
            Dim dOre As New DataColumn("Ore")
            Dim dGiorni As New DataColumn("Giorni")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dGiorno)
            dttTabella.Columns.Add(dMese)
            dttTabella.Columns.Add(dOre)
            dttTabella.Columns.Add(dGiorni)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "B.Lavoro, "
                Splitta2 = "Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta.Replace("B.", "") & " Giorno, Mese, SUM(CONVERT(Numeric(4,2), SUBSTRING(sMisti, 1,CHARINDEX(';',sMisti)-1))) As Ore From (" &
                "Select " & Splitta & "A.Giorno, A.Mese, A.Quanto,  SUBSTRING (Misti,CHARINDEX ('M', Misti)+1 ,5) As sMisti From " &
                "" & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And Misti<>'' And Misti Is Not Null And Quanto=-3 " &
                " " & Altro & " " &
                ") C Where CONVERT(Numeric(4,2), SUBSTRING(sMisti, 1,CHARINDEX(';',sMisti)-1))<>'8' " &
                "Group By " & Splitta2 & "Giorno, Mese " &
                "Order By Ore Desc, Mese, Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Ore").Value Then
                    Vecchio = Rec("Ore").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Giorno").Value
                rigaR(3) = "" & Mesi(Rec("Mese").Value - 1)
                rigaR(4) = ScriveNumero("" & Rec("Ore").Value)
                rigaR(5) = ScriveNumero("" & Rec("Ore").Value / 8)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdOreMalattia.DataSource = dttTabella
            grdOreMalattia.DataBind()
            grdOreMalattia.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdOreMalattia_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdOreMalattia.PageIndexChanging
        grdOreMalattia.PageIndex = e.NewPageIndex
        grdOreMalattia.DataBind()

        CaricaOreDiMalattia()
    End Sub

    Protected Sub StatisticheOreMalattia(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdOreMalattia.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Giorno As String = Di.Cells(2).Text
        Dim Mese As String = Di.Cells(3).Text

        GiornoSel = Giorno
        MeseSel = Mese
        SocietaSel = Societa

        CaricaStatisticheOreMalattia(Societa, Giorno, Mese)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatisticheOreMalattia(Societa As String, Giorno As String, Mese As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Malattia As String

            lblStatistiche.Text = "Statistiche su ore di malattia: " & Giorno & " " & Mese

            Dim sMese As String = PrendeMese(Mese)

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And B.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select A.*, B.Lavoro From " & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And " &
                "Giorno = " & Giorno & " And " &
                "Mese = " & sMese & " And " &
                "Quanto < 0 And Misti Like '%M%' And Misti Not Like '%M8%' And Misti Not Like '%M0%' " &
                "Order By Anno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                Malattia = Rec("Misti").Value
                Malattia = Mid(Malattia, InStr(Malattia, "M") + 1, Len(Malattia))
                Malattia = Mid(Malattia, 1, InStr(Malattia, ";") - 1)
                rigaR(2) = Malattia
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' ORE PERMESSO

    Protected Sub imgOrePermesso_Click(sender As Object, e As ImageClickEventArgs) Handles imgOrePermesso.Click
        SpegneDiv()
        ModalitaStatistica = "OREPERMESSO"
        divOrePermesso.Visible = True
        lblTitoloMaschera.Text = "Statistica ore di permesso"

        CaricaOreDiPermesso()
    End Sub

    Private Sub CaricaOreDiPermesso()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dGiorno As New DataColumn("Giorno")
            Dim dMese As New DataColumn("Mese")
            Dim dOre As New DataColumn("Ore")
            Dim dGiorni As New DataColumn("Giorni")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dGiorno)
            dttTabella.Columns.Add(dMese)
            dttTabella.Columns.Add(dOre)
            dttTabella.Columns.Add(dGiorni)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "B.Lavoro, "
                Splitta2 = "Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta.Replace("B.", "") & " Giorno, Mese, SUM(CONVERT(Numeric(4,2), SUBSTRING(sMisti, 1,CHARINDEX(';',sMisti)-1))) As Ore From (" &
                "Select " & Splitta & " A.Giorno, A.Mese, A.Quanto,  SUBSTRING (Misti,CHARINDEX ('P', Misti)+1 ,5) As sMisti From " &
                "" & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And Misti<>'' And Misti Is Not Null And Quanto=-3 " &
                " " & Altro & " " &
                ") C Where CONVERT(Numeric(4,2), SUBSTRING(sMisti, 1,CHARINDEX(';',sMisti)-1))<>'8' " &
                "Group By " & Splitta2 & " Giorno, Mese " &
                "Order By Ore Desc, Mese, Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Ore").Value Then
                    Vecchio = Rec("Ore").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Giorno").Value
                rigaR(3) = "" & Mesi(Rec("Mese").Value - 1)
                rigaR(4) = ScriveNumero("" & Rec("Ore").Value)
                rigaR(5) = ScriveNumero("" & Rec("Ore").Value / 8)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdOrePermesso.DataSource = dttTabella
            grdOrePermesso.DataBind()
            grdOrePermesso.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdOrePermesso_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdOrePermesso.PageIndexChanging
        grdOrePermesso.PageIndex = e.NewPageIndex
        grdOrePermesso.DataBind()

        CaricaOreDiPermesso()
    End Sub

    Protected Sub StatisticheOrePermesso(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdOrePermesso.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Giorno As String = Di.Cells(2).Text
        Dim Mese As String = Di.Cells(3).Text

        GiornoSel = Giorno
        MeseSel = Mese
        SocietaSel = Societa

        CaricaStatisticheOrePermesso(Societa, Giorno, Mese)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatisticheOrePermesso(Societa As String, Giorno As String, Mese As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Malattia As String

            lblStatistiche.Text = "Statistiche su ore di malattia: " & Giorno & " " & Mese

            Dim sMese As String = PrendeMese(Mese)

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And B.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select A.*, B.Lavoro From " & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And " &
                "Giorno = " & Giorno & " And " &
                "Mese = " & sMese & " And " &
                "Quanto < 0 And Misti Like '%P%' And Misti Not Like '%P8%' And Misti Not Like '%P0%' " &
                "Order By Anno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                Malattia = Rec("Misti").Value
                Malattia = Mid(Malattia, InStr(Malattia, "P") + 1, Len(Malattia))
                Malattia = Mid(Malattia, 1, InStr(Malattia, ";") - 1)
                rigaR(2) = Malattia
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    Protected Sub imgChiudeStat_Click(sender As Object, e As ImageClickEventArgs) Handles imgChiudeStat.Click
        divStatistiche.Visible = False
    End Sub

    ' PORTATE

    Protected Sub imgStatPranzi_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatPranzi.Click
        SpegneDiv()
        ModalitaStatistica = "PORTATE"
        divStatPranzi.Visible = True
        lblTitoloMaschera.Text = "Statistica portate"

        CaricaPortate()
    End Sub

    Private Sub CaricaPortate()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dPortata As New DataColumn("Portata")
            Dim dRicorrenze As New DataColumn("Ricorrenze")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dPortata)
            dttTabella.Columns.Add(dRicorrenze)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And D.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "D.Lavoro, "
                Splitta2 = "Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta & " Portata, Count(*) As Quante From " & PrefissoTabelle & "Pranzi2 A " &
                "Left Join " & PrefissoTabelle & "Portate B On A.idUtente = B.idUtente And A.idPortata = B.idPortata " &
                "Left Join " & PrefissoTabelle & "Orari C On A.idUtente = B.idUtente And A.idGiorno=C.Giorno And A.idMese=C.Mese And A.idAnno=C.Anno " &
                "Left Join " & PrefissoTabelle & "Lavori D On A.idUtente=B.idUtente And C.idlavoro=D.idLavoro  " &
                "Where A.idUtente=" & idUtente & " " &
                " " & Altro & " " &
                "Group By " & Splitta2 & " Portata " &
                "Order By 3 Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Quante").Value Then
                    Vecchio = Rec("Quante").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Portata").Value
                rigaR(3) = "" & Rec("Quante").Value
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdPranzi.DataSource = dttTabella
            grdPranzi.DataBind()
            grdPranzi.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdPranzi_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPranzi.PageIndexChanging
        grdPranzi.PageIndex = e.NewPageIndex
        grdPranzi.DataBind()

        CaricaPortate()
    End Sub

    Protected Sub StatistichePranzi(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdPranzi.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Portata As String = Di.Cells(2).Text

        GiornoSel = Portata
        MeseSel = Societa

        CaricaStatistichePortate(Portata, Societa)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatistichePortate(Portata As String, Societa As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatistiche.Text = "Statistiche su portate: " & Portata

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And D.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select A.*, B.Portata, D.Lavoro From " & PrefissoTabelle & "Pranzi2 A " &
                "Left Join " & PrefissoTabelle & "Portate B On A.idUtente = B.idUtente And A.idPortata = B.idPortata " &
                "Left Join " & PrefissoTabelle & "Orari C On A.idUtente = B.idUtente And A.idGiorno=C.Giorno And A.idMese=C.Mese And A.idAnno=C.Anno " &
                "Left Join " & PrefissoTabelle & "Lavori D On A.idUtente=B.idUtente And C.idlavoro=D.idLavoro " &
                "Where Portata='" & Portata & "' And A.idUtente=" & idUtente & " " &
                " " & Altro & " " &
                "Order By Anno Desc, Mese Desc, Giorno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("idGiorno").Value & "/" & Rec("idMese").Value & "/" & Rec("idAnno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = ""
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' PASTICCHE

    Protected Sub imgStatPasticca_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatPasticca.Click
        SpegneDiv()
        ModalitaStatistica = "PASTICCHE"
        divStatPasticche.Visible = True
        lblTitoloMaschera.Text = "Statistica pasticche"

        CaricaPasticche()
    End Sub

    Private Sub CaricaPasticche()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dPasticca As New DataColumn("Pasticca")
            Dim dRicorrenze As New DataColumn("Ricorrenze")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dPasticca)
            dttTabella.Columns.Add(dRicorrenze)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And D.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "D.Lavoro, "
                Splitta2 = "Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta & " B.descPasticca , Count(*) As Quante From " & PrefissoTabelle & "AltreInfoPasticca A " &
                "Left Join " & PrefissoTabelle & "Pasticche B On A.idUtente = B.idUtente And A.idPasticca=B.idPasticca " &
                "Left Join " & PrefissoTabelle & "Orari C On B.idUtente = C.idUtente And A.Giorno=C.Giorno And A.Mese=C.Mese And A.Anno=C.Anno " &
                "Left Join " & PrefissoTabelle & "Lavori D On C.idUtente=D.idUtente And C.idlavoro=D.idLavoro  " &
                "Where C.idUtente = " & idUtente & " " &
                " " & Altro & " " &
                "Group By " & Splitta2 & " descPasticca " &
                "Order By 3 Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Quante").Value Then
                    Vecchio = Rec("Quante").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("descPasticca").Value
                rigaR(3) = "" & Rec("Quante").Value
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdPasticche.DataSource = dttTabella
            grdPasticche.DataBind()
            grdPasticche.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdPasticche_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPasticche.PageIndexChanging
        grdPasticche.PageIndex = e.NewPageIndex
        grdPasticche.DataBind()

        CaricaPasticche()
    End Sub

    Protected Sub StatistichePasticche(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdPasticche.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Pasticca As String = Di.Cells(2).Text

        GiornoSel = Pasticca
        MeseSel = Societa

        CaricaStatistichePasticca(Pasticca, Societa)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatistichePasticca(Pasticca As String, Societa As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatistiche.Text = "Statistiche su pasticca: " & Pasticca

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And D.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select A.*, B.descPasticca, D.Lavoro From " & PrefissoTabelle & "AltreInfoPasticca A " &
                "Left Join " & PrefissoTabelle & "Pasticche B On A.idUtente = B.idUtente And A.idPasticca = B.idPasticca " &
                "Left Join " & PrefissoTabelle & "Orari C On A.idUtente = B.idUtente And A.Giorno=C.Giorno And A.Mese=C.Mese And A.Anno=C.Anno " &
                "Left Join " & PrefissoTabelle & "Lavori D On A.idUtente=B.idUtente And C.idlavoro=D.idLavoro " &
                "Where descPasticca='" & Pasticca & "' And A.idUtente=" & idUtente & " " &
                " " & Altro & " " &
                "Order By Anno Desc, Mese Desc, Giorno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = ""
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' MEZZI DI ANDATA

    Protected Sub imgStatMezziAndata_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatMezziAndata.Click
        SpegneDiv()
        ModalitaStatistica = "MEZZIANDATA"
        divStatMezziAndata.Visible = True
        lblTitoloMaschera.Text = "Statistica mezzi di andata"

        CaricaMezziDiAndata()
    End Sub

    Private Sub CaricaMezziDiAndata()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dMezzo As New DataColumn("Mezzo")
            Dim dRicorrenze As New DataColumn("Ricorrenze")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dMezzo)
            dttTabella.Columns.Add(dRicorrenze)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And D.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "D.Lavoro, "
                Splitta2 = "Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta & " B.descMezzo, B.Dettaglio, Count(*) As Quante From " & PrefissoTabelle & "AltreInfoMezzi A " &
                "Left Join " & PrefissoTabelle & "Mezzi B On A.idUtente = B.idUtente And A.idMezzo =B.idMezzo " &
                "Left Join " & PrefissoTabelle & "Orari C On B.idUtente = C.idUtente And A.Giorno=C.Giorno And A.Mese=C.Mese And A.Anno=C.Anno " &
                "Left Join " & PrefissoTabelle & "Lavori D On C.idUtente=D.idUtente And C.idlavoro=D.idLavoro  " &
                "Where C.idUtente = " & idUtente & " " &
                " " & Altro & " " &
                "Group By " & Splitta2 & " descMezzo, Dettaglio " &
                "Order By 4 Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Quante").Value Then
                    Vecchio = Rec("Quante").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                If Rec("Dettaglio").value Is DBNull.Value = False AndAlso Rec("Dettaglio").Value <> "" Then
                    Altro = " (" & Rec("Dettaglio").Value & ")"
                Else
                    Altro = ""
                End If
                rigaR(2) = "" & Rec("descMezzo").Value & Altro
                rigaR(3) = "" & Rec("Quante").Value
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdMezziAndata.DataSource = dttTabella
            grdMezziAndata.DataBind()
            grdMezziAndata.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdMezziAndata_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdMezziAndata.PageIndexChanging
        grdMezziAndata.PageIndex = e.NewPageIndex
        grdMezziAndata.DataBind()

        CaricaMezziDiAndata()
    End Sub

    Protected Sub StatisticheMezziDiAndata(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdMezziAndata.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Mezzo As String = Di.Cells(2).Text

        GiornoSel = Mezzo
        MeseSel = Societa

        CaricaStatisticheMezziDiAndata(Mezzo, Societa)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatisticheMezziDiAndata(Mezzo As String, Societa As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatistiche.Text = "Statistiche su mezzo di andata: " & Mezzo

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And D.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Dim Altro2 As String = ""

            If InStr(Mezzo, "(") > 0 Then
                Altro2 = Mid(Mezzo, InStr(Mezzo, "(") + 1, Len(Mezzo)).Trim
                Altro2 = Mid(Altro2, 1, Len(Altro2) - 1)
                Altro2 = " And Dettaglio='" & Altro2.Replace("'", "''") & "' "
                Mezzo = Mid(Mezzo, 1, InStr(Mezzo, "(") - 1).Trim
            End If

            Sql = "Select A.*, B.descMezzo, D.Lavoro From " & PrefissoTabelle & "AltreInfoMezzi A " &
                "Left Join " & PrefissoTabelle & "Mezzi B On A.idUtente = B.idUtente And A.idMezzo = B.idMezzo " &
                "Left Join " & PrefissoTabelle & "Orari C On A.idUtente = B.idUtente And A.Giorno=C.Giorno And A.Mese=C.Mese And A.Anno=C.Anno " &
                "Left Join " & PrefissoTabelle & "Lavori D On A.idUtente=B.idUtente And C.idlavoro=D.idLavoro " &
                "Where descMezzo='" & Mezzo & "' " & Altro2 & " And A.idUtente=" & idUtente & " " &
                " " & Altro & " " &
                "Order By Anno Desc, Mese Desc, Giorno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = ""
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' MEZZI DI RITORNO

    Protected Sub imgStatMezziRitorno_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatMezziRitorno.Click
        SpegneDiv()
        ModalitaStatistica = "MEZZIRITORNO"
        divStatMezziRitorno.Visible = True
        lblTitoloMaschera.Text = "Statistica mezzi di ritorno"

        CaricaMezziDiRitorno()
    End Sub

    Private Sub CaricaMezziDiRitorno()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dMezzo As New DataColumn("Mezzo")
            Dim dRicorrenze As New DataColumn("Ricorrenze")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -1
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dMezzo)
            dttTabella.Columns.Add(dRicorrenze)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And D.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "D.Lavoro, "
                Splitta2 = "Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta & " B.descMezzo, B.Dettaglio, Count(*) As Quante From " & PrefissoTabelle & "AltreInfoMezziRit A " &
                "Left Join " & PrefissoTabelle & "Mezzi B On A.idUtente = B.idUtente And A.idMezzo =B.idMezzo " &
                "Left Join " & PrefissoTabelle & "Orari C On B.idUtente = C.idUtente And A.Giorno=C.Giorno And A.Mese=C.Mese And A.Anno=C.Anno " &
                "Left Join " & PrefissoTabelle & "Lavori D On C.idUtente=D.idUtente And C.idlavoro=D.idLavoro  " &
                "Where C.idUtente = " & idUtente & " " &
                " " & Altro & " " &
                "Group By " & Splitta2 & " descMezzo, Dettaglio " &
                "Order By 4 Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Quante").Value Then
                    Vecchio = Rec("Quante").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                If Rec("Dettaglio").value Is DBNull.Value = False AndAlso Rec("Dettaglio").Value <> "" Then
                    Altro = " (" & Rec("Dettaglio").Value & ")"
                Else
                    Altro = ""
                End If
                rigaR(2) = "" & Rec("descMezzo").Value & Altro
                rigaR(3) = "" & Rec("Quante").Value
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdMezziRitorno.DataSource = dttTabella
            grdMezziRitorno.DataBind()
            grdMezziRitorno.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdMezziRitorno_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdMezziRitorno.PageIndexChanging
        grdMezziRitorno.PageIndex = e.NewPageIndex
        grdMezziRitorno.DataBind()

        CaricaMezziDiRitorno()
    End Sub

    Protected Sub StatisticheMezziDiRitorno(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdMezziRitorno.Rows(Riga)
        Dim Societa As String = Di.Cells(1).Text
        Dim Mezzo As String = Di.Cells(2).Text

        GiornoSel = Mezzo
        MeseSel = Societa

        CaricaStatisticheMezziDiRitorno(Mezzo, Societa)

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatisticheMezziDiRitorno(Mezzo As String, Societa As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatistiche.Text = "Statistiche su mezzo di ritorno: " & Mezzo

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And D.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Dim Altro2 As String = ""

            If InStr(Mezzo, "(") > 0 Then
                Altro2 = Mid(Mezzo, InStr(Mezzo, "(") + 1, Len(Mezzo)).Trim
                Altro2 = Mid(Altro2, 1, Len(Altro2) - 1)
                Altro2 = " And Dettaglio='" & Altro2.Replace("'", "''") & "' "
                Mezzo = Mid(Mezzo, 1, InStr(Mezzo, "(") - 1).Trim
            End If

            Sql = "Select A.*, B.descMezzo, D.Lavoro From " & PrefissoTabelle & "AltreInfoMezziRit A " &
                 "Left Join " & PrefissoTabelle & "Mezzi B On A.idUtente = B.idUtente And A.idMezzo = B.idMezzo " &
                 "Left Join " & PrefissoTabelle & "Orari C On A.idUtente = B.idUtente And A.Giorno=C.Giorno And A.Mese=C.Mese And A.Anno=C.Anno " &
                 "Left Join " & PrefissoTabelle & "Lavori D On A.idUtente=B.idUtente And C.idlavoro=D.idLavoro " &
                 "Where descMezzo='" & Mezzo & "' " & Altro2 & " And A.idUtente=" & idUtente & " " &
                 " " & Altro & " " &
                 "Order By Anno Desc, Mese Desc, Giorno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = ""
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    ' MINIMO ENTRATA 

    Protected Sub imgMinimoEntrata_Click(sender As Object, e As ImageClickEventArgs) Handles imgMinimoEntrata.Click
        SpegneDiv()
        ModalitaStatistica = "MINIMOENTRATA"
        divMinimoEntrata.Visible = True
        lblTitoloMaschera.Text = "Statistica entrata minima"

        CaricaMinimoEntrata()
    End Sub

    Private Sub CaricaMinimoEntrata()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dCommessa As New DataColumn("Commessa")
            Dim dData As New DataColumn("Data")
            Dim dEntrata As New DataColumn("Entrata")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As String = ""
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dCommessa)
            dttTabella.Columns.Add(dData)
            dttTabella.Columns.Add(dEntrata)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Sql = "Select Top 100 Giorno, Mese, Anno, Replace(Entrata,'.',':') +'.000' As Entrata, B.Lavoro, C.Descrizione " &
                "From " & PrefissoTabelle & "Orari A " &
                "Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Left Join " & PrefissoTabelle & "Commesse C On A.idUtente = C.idUtente And A.idLavoro = C.idLavoro And A.CodCommessa = C.Codice " &
                "Where A.Entrata Is Not Null And A.Entrata<>'' " &
                "And A.idUtente= " & idUtente & " " &
                " " & Altro & " " &
                "Order By 4"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Entrata").Value Then
                    Vecchio = Rec("Entrata").Value
                    Contatore += 1
                End If

                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Descrizione").Value
                rigaR(3) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(4) = "" & Rec("Entrata").Value
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdMinimoEntrata.DataSource = dttTabella
            grdMinimoEntrata.DataBind()
            grdMinimoEntrata.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdMinimoEntrata_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdMinimoEntrata.PageIndexChanging
        grdMinimoEntrata.PageIndex = e.NewPageIndex
        grdMinimoEntrata.DataBind()

        CaricaMinimoEntrata()
    End Sub

    ' STRAORDINARI

    Protected Sub imgMaxStarordinari_Click(sender As Object, e As ImageClickEventArgs) Handles imgMaxStarordinari.Click
        SpegneDiv()
        ModalitaStatistica = "STRAORDINARI"
        divStraordinari.Visible = True
        lblTitoloMaschera.Text = "Statistica straordinari"

        CaricaStraordinari()
    End Sub

    Private Sub CaricaStraordinari()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dCommessa As New DataColumn("Commessa")
            Dim dData As New DataColumn("Data")
            Dim dEntrata As New DataColumn("Entrata")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As String = ""
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dCommessa)
            dttTabella.Columns.Add(dData)
            dttTabella.Columns.Add(dEntrata)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Sql = "Select Top 100 Giorno,Mese, Anno, Quanto, B.Lavoro, C.Descrizione " &
                "From " & PrefissoTabelle & "Orari A " &
                "Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Left Join " & PrefissoTabelle & "Commesse C On A.idUtente = C.idUtente And A.idLavoro = C.idLavoro And A.CodCommessa = C.Codice " &
                "Where Quanto > 8 " &
                "And A.idUtente= " & idUtente & " " &
                " " & Altro & " " &
                "Order By Quanto Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> "" & Rec("Quanto").Value.ToString Then
                    Vecchio = "" & Rec("Quanto").Value.ToString
                    Contatore += 1
                End If

                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Descrizione").Value
                rigaR(3) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(4) = "" & Rec("Quanto").Value
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStraordinari.DataSource = dttTabella
            grdStraordinari.DataBind()
            grdStraordinari.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdStraordinari_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStraordinari.PageIndexChanging
        grdStraordinari.PageIndex = e.NewPageIndex
        grdStraordinari.DataBind()

        CaricaStraordinari()
    End Sub

    ' PERIODO CONTINUATIVO LAVORO

    Protected Sub imgPeriodoLavorativo_Click(sender As Object, e As ImageClickEventArgs) Handles imgPeriodoLavorativo.Click
        SpegneDiv()
        ModalitaStatistica = "PERIODOLAVORATIVO"
        divPeriodoLavorativo.Visible = True
        lblTitoloMaschera.Text = "Periodo lavorativo continuato"

        CaricaPeriodoLavorativo()
    End Sub

    Private Sub CaricaPeriodoLavorativo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            'Sql = "Delete From " & prefissotabelle & "Statistiche Where idUtente=" & idUtente
            'EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Dim DataPrecedente As Date
            Dim DataSuccessiva As Date
            Dim DataSucc As Date
            Dim DataPrec As Date
            Dim dataInizio As Date
            Dim dataPartenza() As Date
            Dim dataFine() As Date
            Dim differenza As TimeSpan
            Dim Datella As Date
            Dim Giorni As Integer = 0
            Dim numGiorni() As Integer
            Dim QuanteRicorrenze As Integer = 0
            Dim Arrivo As Single = 0
            Dim Attuale As Integer = 0

            'Sql = "Select Max((Anno*1000)+(Mese*50)+Giorno) From " & prefissotabelle & "Statistiche"
            'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            'If Rec(0).value Is DBNull.Value = False Then
            '    Arrivo = Rec(0).Value
            'End If
            'Rec.Close()

            Sql = "Delete From " & PrefissoTabelle & "Statistiche Where idUtente=" & idUtente
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into " & PrefissoTabelle & "Statistiche " &
                "Select * From " & PrefissoTabelle & "Orari " &
                "Where idUtente=" & idUtente & " And " &
                "(Quanto>0 Or Quanto=-6 Or (Quanto=-3 And Misti Not Like '%P" & OreStandard.ToString.Trim & "%')) And " &
                "(Anno*1000)+(Mese*50)+Giorno Not In " &
                "(Select (Anno*1000)+(Mese*50)+Giorno From " & PrefissoTabelle & "Statistiche Where idUtente=" & idUtente & ")"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Select * From " & PrefissoTabelle & "Statistiche " &
                "Where idUtente=" & idUtente & " " &
                "And (Anno*1000)+(Mese*50)+Giorno>" & Arrivo & " " &
                "Order By Anno, Mese, Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value

                DataPrec = Datella

                Rec.MoveNext()
            End If
            Do Until Rec.Eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value

                DataSucc = Datella

                differenza = DataSucc.Subtract(DataPrec)
                If differenza.Days > 1 Then
                    Dim dataAppoggio As Date = DataPrec.AddDays(1)

                    While dataAppoggio < DataSucc
                        If ControllaFestivo(dataAppoggio) = True Then
                            Sql = "INSERT INTO [dbo].[" & PrefissoTabelle & "Statistiche] VALUES(" &
                               " " & idUtente & ", " &
                               " " & dataAppoggio.Day & ", " &
                               " " & dataAppoggio.Month & ", " &
                               " " & dataAppoggio.Year & ", " &
                               " -99, " &
                               " '', " &
                               " '', " &
                               " '', " &
                               " '', " &
                               " 0, " &
                               " 0, " &
                               " 0)"
                            EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                        End If

                        dataAppoggio = dataAppoggio.AddDays(1)
                    End While
                End If

                DataPrec = DataSucc

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From " & PrefissoTabelle & "Statistiche " &
                "Where idUtente=" & idUtente & " " &
                "Order By (Anno*1000)+(Mese*50)+Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                DataPrecedente = Datella
                dataInizio = DataPrecedente
                Giorni = 0

                Rec.MoveNext()
            End If
            Do Until Rec.Eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value

                DataSuccessiva = Datella

                differenza = DataSuccessiva.Subtract(DataPrecedente)
                If differenza.Days > 1 Then
                    If Giorni > 15 Then
                        QuanteRicorrenze += 1
                        ReDim Preserve dataPartenza(QuanteRicorrenze)
                        ReDim Preserve dataFine(QuanteRicorrenze)
                        ReDim Preserve numGiorni(QuanteRicorrenze)

                        dataPartenza(QuanteRicorrenze) = dataInizio
                        dataFine(QuanteRicorrenze) = DataPrecedente
                        numGiorni(QuanteRicorrenze) = Giorni
                    End If

                    dataInizio = DataSuccessiva
                    'While ControllaFestivo(dataInizio) = True
                    '    dataInizio = dataInizio.AddDays(1)
                    'End While

                    Giorni = 0
                Else
                    If Val(Rec("Quanto").Value.ToString) > 0 Then
                        Giorni += 1
                    End If
                End If

                DataPrecedente = DataSuccessiva

                Rec.MoveNext()
            Loop
            Rec.Close()

            Giorni += 1

            'If Giorni > 15 Then
            QuanteRicorrenze += 1
            Dim RicorrenzaAttuale As Integer = QuanteRicorrenze

            ReDim Preserve dataPartenza(QuanteRicorrenze)
            ReDim Preserve dataFine(QuanteRicorrenze)
            ReDim Preserve numGiorni(QuanteRicorrenze)

            dataPartenza(QuanteRicorrenze) = dataInizio
            dataFine(QuanteRicorrenze) = DataSuccessiva
            numGiorni(QuanteRicorrenze) = Giorni
            'End If

            Dim dDataAppoggio As Date
            Dim Numero As Integer

            For i As Integer = 1 To QuanteRicorrenze
                While ControllaFestivo(dataPartenza(i)) = True
                    dataPartenza(i) = dataPartenza(i).AddDays(1)
                    'numGiorni(i) -= 1
                End While
                While ControllaFestivo(dataFine(i)) = True
                    dataFine(i) = dataFine(i).AddDays(-1)
                    'numGiorni(i) -= 1
                End While
            Next

            Dim dGiorni As New DataColumn("Giorni")
            Dim dDataInizio As New DataColumn("DataInizio")
            Dim dDataFine As New DataColumn("DataFine")
            Dim dSocietaIn As New DataColumn("SocietaIn")
            Dim dSocietaFi As New DataColumn("SocietaFi")

            dttTabella = New DataTable
            dttTabella.Columns.Add(dGiorni)
            dttTabella.Columns.Add(dDataInizio)
            dttTabella.Columns.Add(dDataFine)
            dttTabella.Columns.Add(dSocietaIn)
            dttTabella.Columns.Add(dSocietaFi)

            If AggiungeRicorrenza(numGiorni(RicorrenzaAttuale), dataPartenza(RicorrenzaAttuale), dataFine(RicorrenzaAttuale), ConnSQL) = True Then
                rigaR = dttTabella.NewRow()
                rigaR(0) = "-----"
                rigaR(1) = "----------------"
                rigaR(2) = "----------------"
                rigaR(3) = "----------------"
                rigaR(4) = "----------------"
                dttTabella.Rows.Add(rigaR)
            End If

            For i As Integer = 1 To QuanteRicorrenze
                For k As Integer = i + 1 To QuanteRicorrenze
                    If numGiorni(i) < numGiorni(k) Then
                        dDataAppoggio = dataPartenza(i)
                        dataPartenza(i) = dataPartenza(k)
                        dataPartenza(k) = dDataAppoggio

                        dDataAppoggio = dataFine(i)
                        dataFine(i) = dataFine(k)
                        dataFine(k) = dDataAppoggio

                        Numero = numGiorni(i)
                        numGiorni(i) = numGiorni(k)
                        numGiorni(k) = Numero
                    End If
                Next
            Next

            If QuanteRicorrenze > 20 Then QuanteRicorrenze = 20

            For i As Integer = 1 To QuanteRicorrenze
                If AggiungeRicorrenza(numGiorni(i), dataPartenza(i), dataFine(i), ConnSQL) = True Then

                End If
            Next

            grdPeriodo.DataSource = dttTabella
            grdPeriodo.DataBind()
            grdPeriodo.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Function AggiungeRicorrenza(Giorni As Integer, DataPartenza As Date, DataFine As Date, ConnSql As Object) As Boolean
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}
        Dim sDatellaI As String
        Dim sDatellaF As String
        Dim Soc As String
        Dim Ok As Boolean
        Dim Sql As String
        Dim Scritta As Boolean = False

        If Giorni > 0 Then
            sDatellaI = DataPartenza.Day & " " & Mesi(DataPartenza.Month - 1) & " " & DataPartenza.Year
            sDatellaF = DataFine.Day & " " & Mesi(DataFine.Month - 1) & " " & DataFine.Year

            rigaR = dttTabella.NewRow()
            rigaR(0) = Giorni
            rigaR(1) = MetteMaiuscole(DataPartenza.ToString("dddd")) & " " & sDatellaI
            rigaR(2) = MetteMaiuscole(DataFine.ToString("dddd")) & " " & sDatellaF

            Soc = ""
            Sql = "Select B.Lavoro, C.Descrizione  From " & PrefissoTabelle & "Statistiche A " &
                "Left Join " & PrefissoTabelle & "Lavori B On A.idLavoro = B.idLavoro And A.idUtente = B.idUtente " &
                "Left Join " & PrefissoTabelle & "Commesse C On A.idUtente = C.idUtente And B.idLavoro = C.idLavoro And A.CodCommessa = C.Codice " &
                "Where " &
                "Giorno = " & DataPartenza.Day.ToString & " And " &
                "Mese = " & DataPartenza.Month.ToString & " And " &
                "Anno = " & DataPartenza.Year.ToString
            Rec = LeggeQuery(ConnSql, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                If Rec("Lavoro").Value Is DBNull.Value = False Then
                    rigaR(3) = Rec("Lavoro").Value
                    Soc = Rec("Lavoro").Value
                Else
                    rigaR(3) = ""
                End If
                If Rec("Descrizione").Value Is DBNull.Value = False Then
                    rigaR(3) += "-" & Rec("Descrizione").Value
                End If
            Else
                rigaR(3) = ""
            End If
            Rec.Close()

            Sql = "Select B.Lavoro, C.Descrizione  From " & PrefissoTabelle & "Statistiche A " &
                "Left Join " & PrefissoTabelle & "Lavori B On A.idLavoro = B.idLavoro And A.idUtente = B.idUtente " &
                "Left Join " & PrefissoTabelle & "Commesse C On A.idUtente = C.idUtente And B.idLavoro = C.idLavoro And A.CodCommessa = C.Codice " &
                "Where " &
                "Giorno = " & DataFine.Day.ToString & " And " &
                "Mese = " & DataFine.Month.ToString & " And " &
                "Anno = " & DataFine.Year.ToString
            Rec = LeggeQuery(ConnSql, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                If Rec("Lavoro").Value Is DBNull.Value = False Then
                    rigaR(4) = Rec("Lavoro").Value
                    Soc += ";" & Rec("Lavoro").Value
                Else
                    rigaR(4) = ""
                End If
                If Rec("Descrizione").Value Is DBNull.Value = False Then
                    rigaR(4) += "-" & Rec("Descrizione").Value
                End If
            Else
                rigaR(4) = ""
            End If
            Rec.Close()

            Ok = True
            If cmbSocieta.Text <> "" Then
                If InStr(Soc, cmbSocieta.Text) = 0 Then
                    Ok = False
                    Scritta = False
                End If
            End If

            If Ok = True Then
                dttTabella.Rows.Add(rigaR)
                Scritta = True
            End If
        End If

        Return Scritta
    End Function

    ' KM

    Protected Sub imgKM_Click(sender As Object, e As ImageClickEventArgs) Handles imgKM.Click
        SpegneDiv()
        ModalitaStatistica = "KM"
        divKM.Visible = True
        lblTitoloMaschera.Text = "Statistica KM."

        CaricaKM()
    End Sub

    Private Sub CaricaKM()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "C.Descrizione, "
                Splitta2 = ", C.Descrizione "
            Else
                Splitta = "'' As Descrizione, "
                Splitta2 = ""
            End If

            Dim dSocieta As New DataColumn("Societa")
            Dim dCommessa As New DataColumn("Commessa")
            Dim dKM As New DataColumn("Km")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()

            dttTabella = New DataTable
            dttTabella.Columns.Add(dSocieta)
            dttTabella.Columns.Add(dCommessa)
            dttTabella.Columns.Add(dKM)

            Sql = "Select B.Lavoro, " & Splitta & " Sum(A.KM) As KM " &
                "From " & PrefissoTabelle & "Orari A " &
                "Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Left Join " & PrefissoTabelle & "Commesse C On A.idUtente = C.idUtente And A.idLavoro = C.idLavoro And A.CodCommessa = C.Codice " &
                "Where " &
                "A.idUtente= " & idUtente & " " &
                " " & Altro & " " &
                "And B.Lavoro Is Not Null " &
                "Group By B.Lavoro " & Splitta2 & " " &
                "Order By 3 Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Rec("Descrizione").Value Is DBNull.Value = False Then
                    rigaR = dttTabella.NewRow()
                    rigaR(0) = "" & Rec("Lavoro").Value
                    rigaR(1) = "" & Rec("Descrizione").Value
                    rigaR(2) = ScriveNumero("" & Rec("KM").Value)
                    dttTabella.Rows.Add(rigaR)
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdKM.DataSource = dttTabella
            grdKM.DataBind()
            grdKM.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub grdKM_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdKM.PageIndexChanging
        grdKM.PageIndex = e.NewPageIndex
        grdKM.DataBind()

        CaricaKM()
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub

    ' GRADI 

    Protected Sub imgGradi_Click(sender As Object, e As ImageClickEventArgs) Handles imgGradi.Click
        SpegneDiv()
        ModalitaStatistica = "GRADI"
        divGradi.Visible = True
        divGradiDett.Visible = True
        lblTitoloMaschera.Text = "Storico Gradi"

        Dim nPosizione As String
        Dim nGiornata As String

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select Top 50 Gradi, Giorno, Mese, Anno From " & PrefissoTabelle & "AltreInfoTempo " &
                "Where Gradi Is Not Null " &
                "Order By Anno Desc, Mese Desc, Giorno Desc"

            nPosizione = ""
            nGiornata = ""

            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                nPosizione += "" & Rec("Gradi").Value.ToString.Trim & ","
                nGiornata += "'" & Rec("Giorno").Value.ToString.Trim & "/" & Rec("Mese").Value.ToString.Trim & "',"

                Rec.MoveNext()
            Loop
            Rec.Close()

            ' Max/Min Gradi
            Dim MaxGradi As String = ""
            Dim DataMax As String = ""
            Dim SocMax As String = ""
            Dim MinGradi As String = ""
            Dim DataMin As String = ""
            Dim SocMin As String = ""
            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And C.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Sql = "Select A.Giorno, A.Mese, A.Anno, A.Gradi, C.Lavoro " &
                "From " & PrefissoTabelle & "AltreInfoTempo A Left Join " & PrefissoTabelle & "Orari B " &
                "On A.idutente = B.idUtente And A.Giorno = B.Giorno And A.Mese = B.Mese And A.Anno = B.Anno " &
                "Left Join " & PrefissoTabelle & "Lavori C On B.idLavoro = C.idLavoro " &
                "Where Gradi Is Not Null " &
                " " & Altro & " " &
                "Order By Cast(Gradi As int) Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                Rec.MoveFirst()
                MaxGradi = Rec("Gradi").Value
                DataMax = Format(Rec("Giorno").Value, "00") & " " & Mesi(Rec("Mese").Value - 1) & " " & Rec("Anno").Value
                SocMax = Rec("Lavoro").Value

                Do Until Rec.Eof
                    MinGradi = Rec("Gradi").Value
                    DataMin = Format(Rec("Giorno").Value, "00") & " " & Mesi(Rec("Mese").Value - 1) & " " & Rec("Anno").Value
                    SocMin = Rec("Lavoro").Value

                    Rec.MoveNext()
                Loop

                lblGradiDett.Text = "Gradi Min.:" & MinGradi & " " & DataMin & " (" & SocMin & ") / Max.: " & MaxGradi & " " & DataMax & " (" & SocMax & ")"
            Else
                lblGradiDett.Text = ""
            End If
            Rec.Close()

            nPosizione = Mid(nPosizione, 1, Len(nPosizione) - 1).Trim
            nGiornata = Mid(nGiornata, 1, Len(nGiornata) - 1).Trim

            hdnPassaggio1.Value = nGiornata
            hdnPassaggio2.Value = nPosizione

            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script type='text/javascript' language='javascript'>")
            sb.Append("     DisegnaStatistica('divGradi', 'Gradi per giornata','Gradi','Gradi','°');")
            sb.Append("</script>")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)
        End If
    End Sub

    ' ENTRATE

    Protected Sub imgEntrate_Click(sender As Object, e As ImageClickEventArgs) Handles imgEntrate.Click
        SpegneDiv()
        ModalitaStatistica = "ENTRATA"
        divEntrate.Visible = True
        lblTitoloMaschera.Text = "Storico entrate"

        Dim nPosizione As String
        Dim nGiornata As String
        Dim Entrata As String
        Dim Entr1 As String
        Dim Entr2 As String

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select Top 50 Entrata, Giorno, Mese, Anno From " & PrefissoTabelle & "Orari " &
                "Where Entrata Is Not Null And Entrata<>'' " &
                "Order By Anno Desc, Mese Desc, Giorno Desc"

            nPosizione = ""
            nGiornata = ""

            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                Entrata = Rec("Entrata").Value.ToString.Trim
                Entr1 = Val(Left(Entrata, 2)).ToString.Trim
                Entr2 = Val(Mid(Entrata, 4, 2)).ToString.Trim
                If Entr2.Length = 1 Then Entr2 = "0" & Entr2
                Entrata = Entr1 & "." & Entr2

                nPosizione += "" & Entrata & ","
                nGiornata += "'" & Rec("Giorno").Value.ToString.Trim & "/" & Rec("Mese").Value.ToString.Trim & "',"

                Rec.MoveNext()
            Loop
            Rec.Close()

            nPosizione = Mid(nPosizione, 1, Len(nPosizione) - 1).Trim
            nGiornata = Mid(nGiornata, 1, Len(nGiornata) - 1).Trim

            hdnPassaggio1.Value = nGiornata
            hdnPassaggio2.Value = nPosizione

            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb.Append("<script type='text/javascript' language='javascript'>")
            sb.Append("     DisegnaStatistica('divEntrate', 'Entrata per giornata','Entrata','Entrata','');")
            sb.Append("</script>")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)
        End If
    End Sub

    Private Sub grdPeriodo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles grdPeriodo.PageIndexChanging
        grdPeriodo.PageIndex = e.NewPageIndex
        grdPeriodo.DataBind()

        CaricaPeriodoLavorativo()
    End Sub

    ' CURRICULUM
    Protected Sub imgCurriculum_Click(sender As Object, e As ImageClickEventArgs) Handles imgCurriculum.Click
        SpegneDiv()
        ModalitaStatistica = "CURRICULUM"
        divCurriculum.Visible = True
        lblTitoloMaschera.Text = "Curriculum personale"

        CaricaCurriculum()
    End Sub

    Private Sub CaricaCurriculum()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Mesi() As String = {"", "Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim RecC As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim NomeCognome As String = ""
            Dim Nascita As Date
            Dim EMail As String = ""
            Dim Indirizzo As String = ""
            Dim Telefono As String = ""
            Dim Motto As String = ""

            Dim StileMotto As String = "font-family: Verdana; vertical-align: middle; color: #444444; font-size: 14px; font-style: italic; text-shadow: 2px 2px #e8e8e8;"
            ' Dim ClassTitoloLavoro As String = "font-family: Verdana; vertical-align: middle; color: #880000; font-size: 13px; font-weight: bold;"
            ' Dim ClassTitoloSezione As String = "font-family: Verdana;  vertical-align: middle; color: #008800; font-size: 15px; font-weight: bold; text-shadow: 2px 2px #fffdfd;"
            ' Dim ClassTitoloData As String = "font-family: Verdana; font-style:italic; vertical-align: middle; color: #550000; font-size: 11px; font-weight: bold;"
            ' Dim ClassTitoloAttivita As String = "font-family: Verdana; font-style:italic; vertical-align: middle; color: #000055; font-size: 11px; font-weight: bold;"
            ' Dim ClassTitoloTesto As String = "font-family: inherit; vertical-align: middle; color: #777777; font-size: 12px;  text-shadow: 1px 1px #efdfdf;"
            ' Dim ClasseLI As String = "list-style: none outside none; height: auto; padding-left: 25px; margin-bottom: 10px; overflow: hidden; font-family: Arial;vertical-align: middle; color: #77BBFF; font-size: 14px; font-weight: bold;"
            Dim StileEtichettaTitoloGrande As String = "font-family: Arial; color: #3b92dc; font-size: 30px; font-weight: bold; text-shadow: 2px 2px #e8e8e8;"
            Dim StileEtichettaTitoloGrande2 As String = "font-family: Arial; color: #1f6aab; font-size: 20px; font-weight: bold; text-shadow: 2px 2px #e8e8e8;"

            Dim ColoreBackgroundPari As String = "#e6e6e6"
            Dim ColoreBackgroundDispari As String = "#fbfbfb"
            Dim coloreHead As String = "#b6b9b4"

            Sql = "Select * From " & PrefissoTabelle & "Impostazioni Where idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Not Rec.Eof Then
                NomeCognome = "" & Rec("Nome").Value & " " & Rec("Cognome").Value
                Nascita = "" & Rec("DataNascita").Value
                EMail = "" & Rec("Email").Value
                Telefono = "" & Rec("Telefono").Value
                Motto = "" & Rec("Motto").Value
                Rec.Close()
            End If

            Sql = "Select * From " & PrefissoTabelle & "Indirizzi Where idUtente=" & idUtente & " Order By DataFine Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                Indirizzo = Rec("Indirizzo").Value
            End If
            Rec.Close()

            Dim DiffGiorni As Long = DateDiff(DateInterval.Year, Nascita, Now)
            Dim sPath As String = "App_Themes/Standard/images/" & idUtente.ToString.Trim & "-" & Utenza & "/"
            Dim PathImmagine As String = sPath & "Orig_Immagine.png"
            Dim sPathLogo As String = "App_Themes/Standard/images/" & idUtente.ToString.Trim & "-" & Utenza & "/Loghi/"
            Dim StringaImm1 As String
            Dim Stringa As String

            If Dir((Server.MapPath(".") & "/" & PathImmagine).Replace("/", "\")) <> "" Then
                StringaImm1 = "<img src=""" & PathImmagine & """ width=""100px"" border=""1"" bordercolor=""#666666"" />"
            Else
                StringaImm1 = ""
            End If

            Stringa = "<left><span style=""" & StileEtichettaTitoloGrande & """>Curriculum Vitae</span><br /><br />"
            Stringa &= "<table style=""width: 100%;"">"
            Stringa &= "<tr>"
            Stringa &= "<td width=90% align=""left""><span style=""" & StileEtichettaTitoloGrande2 & """>"
            Stringa &= SistemaCaratteriSpeciali(NomeCognome) & " - " & Format(Nascita.Day, "00") & " " & Mesi(Nascita.Month) & " " & Nascita.Year & " (Anni " & DiffGiorni & ")</span><br />"
            Stringa += "<span style=""" & StileEtichettaTitoloGrande2 & """>E-Mail: " & EMail & "</span><br />"
            If Indirizzo <> "" Then
                Stringa += "<span style=""" & StileEtichettaTitoloGrande2 & """>Indirizzo: " & Indirizzo & "</span><br />"
            End If
            If Telefono <> "" Then
                Stringa += "<span style=""" & StileEtichettaTitoloGrande2 & """>Telefono: " & Telefono & "</span><br />"
            End If
            Stringa += "</span>"
            Stringa &= "</td>"
            Stringa &= "<td width=10% align=""center"">"
            Stringa &= StringaImm1
            Stringa &= "</td>"
            Stringa &= "</tr>"
            If Motto <> "" Then
                Stringa &= "<tr>"
                Stringa &= "<td width=90% align=""left""><span style=""" & StileMotto & """>&ldquo;" & SistemaCaratteriSpeciali(Motto) & "&rdquo;</span></td>"
                Stringa &= "<td></td>"
                Stringa &= "</tr>"
            End If
            Stringa &= "</table>"

            Stringa &= "<hr />"

            Dim sDataInizio As String
            Dim sDataFine As String

            Dim Pari As Boolean = True

            ' Studi
            Stringa += "<center><span class=""ClassTitoloSezione"">Studi effettuati:</span></center><hr />"
            Sql = "Select Scuola, Indirizzo, DataInizio, DataFine From Studi Order By DataInizio"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Stringa &= "<Table class=""bordatosfumato"" cellpadding=""2"" cellspacing=""0"">"
            Stringa &= "<thead>"
            Stringa &= "<tr style=""background-color:" & coloreHead & "; "">"
            Stringa &= "<th style=""text-align: center; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style=""color: #166f19; font-size: 15px;"">Istituto</span></th>"
            Stringa &= "<th style=""text-align: center; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style=""color: #166f19; font-size: 15px;"">Indirizzo</span></th>"
            Stringa &= "<th align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style=""color: #166f19; font-size: 15px;"">Data inizio</span></th>"
            Stringa &= "<th align=""right"" style=""padding: 2px;""><span class=""ClassTitoloTesto"" style=""color: #166f19; font-size: 15px;"">Data fine</span></th>"
            Stringa &= "</tr>"
            Stringa &= "</thead>"
            Stringa &= "<tfoot>"
            Stringa &= "<tr><td></td></tr>"
            Stringa &= "</tfoot>"
            Stringa &= "<tbody>"
            Do Until Rec.Eof
                If Pari Then
                    Pari = False
                    Stringa &= "<tr style=""background-color: " & ColoreBackgroundDispari & "; "">"
                Else
                    Pari = True
                    Stringa &= "<tr style=""background-color: " & ColoreBackgroundPari & "; "">"
                End If

                Stringa &= "<td align=""left"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Scuola").Value.ToString.Trim) & "</span></td>"
                Stringa &= "<td align=""left"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Indirizzo").Value.ToString.Trim) & "</span></td>"
                sDataInizio = Rec("DataInizio").Value.ToString
                sDataFine = Rec("DataFine").Value.ToString
                sDataInizio = Mid(sDataInizio, 1, 2) & " " & Mesi(Val(Mid(sDataInizio, 4, 2))) & " " & Mid(sDataInizio, 7, 4)
                sDataFine = Mid(sDataFine, 1, 2) & " " & Mesi(Val(Mid(sDataFine, 4, 2))) & " " & Mid(sDataFine, 7, 4)
                Stringa &= "<td align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & sDataInizio & "</td>"
                Stringa &= "<td align=""right"" style=""padding: 2px;""><span class=""ClassTitoloTesto"">" & sDataFine & "</span></td>"
                Stringa &= "<tr>"

                Rec.MoveNext()
            Loop
            Stringa &= "</tbody>"
            Stringa &= "</table>"
            Rec.Close()

            ' Lavori
            Stringa += "<hr /><center><span class=""ClassTitoloSezione"">Sedi di lavoro:</span></center><hr />"
            Sql = "Select Lavoro, Indirizzo, DataInizio, DataFine From Lavori Where Eliminato = 'N' And idUtente = " & idUtente & " Order By DataInizio Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Stringa &= "<table class=""bordatosfumato"" cellpadding=""2"" cellspacing=""0"">"
            Stringa &= "<thead>"
            Stringa &= "<tr style=""background-color:" & coloreHead & "; "">"
            Stringa &= "<th style=""border-right: 1px solid #847f7f; padding: 2px;"" ></th>"
            Stringa &= "<th style=""text-align: center; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Sede</span></th>"
            Stringa &= "<th style=""text-align: center; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Indirizzo</span></th>"
            Stringa &= "<th align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Data inizio</span></th>"
            Stringa &= "<th align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Data fine</span></th>"
            Stringa &= "<th style=""padding: 2px;""></th>"
            Stringa &= "</tr>"
            Stringa &= "</thead>"
            Stringa &= "<tfoot>"
            Stringa &= "<tr><td></td></tr>"
            Stringa &= "</tfoot>"
            Stringa &= "<tbody>"
            Pari = True
            Do Until Rec.Eof
                If Pari Then
                    Pari = False
                    Stringa &= "<tr style=""background-color: " & ColoreBackgroundDispari & "; "">"
                Else
                    Pari = True
                    Stringa &= "<tr style=""background-color: " & ColoreBackgroundPari & "; "">"
                End If

                Dim icona As String = "<img src=""App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & Rec("Lavoro").Value.ToString.Trim & ".png"""" alt="""" width=80 height=80 />"

                Stringa &= "<td align=""center"" style=""border-right: 1px solid #847f7f; padding: 2px;"">" & icona & "</td>"
                Stringa &= "<td align=""left"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Lavoro").Value.ToString.Trim) & "</span></td>"
                Stringa &= "<td align=""left"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Indirizzo").Value.ToString.Trim) & "</span></td>"
                sDataInizio = Rec("DataInizio").Value.ToString
                sDataFine = Rec("DataFine").Value.ToString
                sDataInizio = Mid(sDataInizio, 1, 2) & " " & Mesi(Val(Mid(sDataInizio, 4, 2))) & " " & Mid(sDataInizio, 7, 4)
                sDataFine = Mid(sDataFine, 1, 2) & " " & Mesi(Val(Mid(sDataFine, 4, 2))) & " " & Mid(sDataFine, 7, 4)
                Stringa += "<td align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & sDataInizio & "</span></td>"
                Stringa &= "<td align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & sDataFine & "</span></td>"

                If Rec("DataFine").Value.ToString <> "" And Rec("DataInizio").Value.ToString <> "" Then
                    Dim d1 As Date = Rec("DataFine").Value
                    Dim d2 As Date = Rec("DataInizio").Value
                    Dim diff As Long = Math.Abs(DateDiff(DateInterval.Day, d1, d2))
                    Dim giorniCommessa As Integer = 0
                    Dim mesiCommessa As Integer = 0
                    Dim anniCommessa As Integer = 0
                    Dim giorniMese As Integer = 30

                    Do While diff > giorniMese
                        mesiCommessa += 1
                        If mesiCommessa = 12 Then
                            mesiCommessa = 1
                            anniCommessa += 1
                        End If
                        diff -= giorniMese
                        If giorniMese = 30 Then
                            giorniMese = 31
                        Else
                            giorniMese = 30
                        End If
                    Loop
                    If diff = 30 Or diff = 31 Then
                        mesiCommessa += 1
                        If mesiCommessa = 12 Then
                            mesiCommessa = 0
                            anniCommessa += 1
                        End If
                        giorniCommessa = 0
                    Else
                        giorniCommessa = diff
                    End If

                    Dim stringaTempo As String = ""
                    If giorniCommessa > 0 And mesiCommessa > 0 And anniCommessa > 0 Then
                        stringaTempo = "Anni " & anniCommessa & "<br />Mesi " & mesiCommessa & "<br />Giorni " & giorniCommessa
                    Else
                        If giorniCommessa > 0 And mesiCommessa > 0 And anniCommessa = 0 Then
                            stringaTempo = "Mesi " & mesiCommessa & "<br />Giorni " & giorniCommessa
                        Else
                            If giorniCommessa > 0 And mesiCommessa = 0 And anniCommessa = 0 Then
                                stringaTempo = "Giorni " & giorniCommessa
                            Else
                                If giorniCommessa = 0 And mesiCommessa > 0 And anniCommessa > 0 Then
                                    stringaTempo = "Anni " & anniCommessa & "<br />Mesi " & mesiCommessa
                                Else
                                    If giorniCommessa = 0 And mesiCommessa = 0 And anniCommessa > 0 Then
                                        stringaTempo = "Anni " & anniCommessa
                                    Else
                                        If giorniCommessa = 0 And mesiCommessa > 0 And anniCommessa = 0 Then
                                            stringaTempo = "Mesi " & mesiCommessa
                                        Else
                                            stringaTempo = "---"
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    Stringa &= "<td align=""right"" style=""min-width: 80px;""><span class=""ClassTitoloTesto"">" & stringaTempo & "</span></td>"
                Else
                    Stringa &= "<td align=""right"" style=""min-width: 80px;""></td>"
                End If

                Stringa &= "</tr>"

                Rec.MoveNext()
            Loop
            Stringa &= "</tbody>"
            Stringa &= "</table>"
            Rec.Close()

            ' Commesse
            Dim Lavoro As String = ""
            Dim UltimaImmagine As String = ""
            Dim UltimoIndirizzo As String = ""
            Dim UltimaCommessa As String = ""
            Dim UltimaDifferenzaTempi As String = ""

            Stringa &= "<p style=""page-break-before: always""></p>"

            Stringa &= "<hr /><center><span class=""ClassTitoloSezione"">Commesse:</span></center><hr />"
            Sql = "Select  B.Lavoro, A.Descrizione As Commessa, A.Indirizzo, A.DataInizio, A.DataFine, C.Descrizione, D.Linguaggio From Commesse A " &
                "Left Join Lavori B On A.idLavoro = B.idLavoro And A.idUtente = B.idUtente " &
                "Left Join Applicazioni C On A.idUtente = C.idUtente And C.idCommessa = A.Codice " &
                "Left Join Linguaggi D On C.idLinguaggio = D.idLinguaggio " &
                "Where Commessa<>'Sede' And A.Descrizione<>'Fermo' And A.idUtente=" & idUtente & " And A.Eliminato<>'S' And B.Eliminato<>'S' And (C.Eliminato<>'S' Or C.Eliminato Is Null) And (D.Eliminato <> 'S' Or D.Eliminato Is Null) " &
                "Order By TRY_PARSE(A.DataInizio AS DATETIME USING 'it') Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Stringa &= "<Table class=""bordatosfumato"" cellpadding=""2"" cellspacing=""0"">"
            Stringa &= "<thead>"
            Stringa &= "<tr style=""background-color:" & coloreHead & "; "">"
            Stringa &= "<th style=""text-align: center; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Sede</span></th>"
            Stringa &= "<th></th>"
            Stringa &= "<th style=""text-align: center; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Commessa</span></th>"
            Stringa &= "<th style=""text-align: center; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Indirizzo</span></th>"
            Stringa &= "<th style=""text-align: center; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Dettaglio</span></th>"
            Stringa &= "<th></th>"
            Stringa &= "<th align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Data inizio</span></th>"
            Stringa &= "<th align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Data fine</span></th>"
            Stringa &= "<th style=""padding: 2px;""></th>"
            Stringa &= "</tr>"
            Stringa &= "</thead>"
            Stringa &= "<tfoot>"
            Stringa &= "<tr><td></td></tr>"
            Stringa &= "</tfoot>"
            Stringa &= "<tbody>"
            Pari = True
            Do Until Rec.Eof
                Dim icona As String

                If UltimaImmagine <> Rec("Commessa").Value.ToString.Trim Then
                    UltimaImmagine = Rec("Commessa").Value.ToString.Trim
                    icona = "<img src=""App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & Rec("Commessa").Value.ToString.Trim & ".png"""" alt="""" width=60 height=60 />"
                    If Pari Then
                        Pari = False
                    Else
                        Pari = True
                    End If
                Else
                    icona = ""
                End If

                If Pari Then
                    'Pari = False
                    Stringa &= "<tr style=""text-align: center; vertical-align: middle; background-color: " & ColoreBackgroundDispari & "; "">"
                Else
                    'Pari = True
                    Stringa &= "<tr style=""text-align: center; vertical-align: middle; background-color: " & ColoreBackgroundPari & "; "">"
                End If

                If Lavoro <> Rec("Lavoro").Value.ToString.Trim Then
                    Lavoro = Rec("Lavoro").Value.ToString.Trim
                    Stringa &= "<td style=""text-align: center; vertical-align: middle; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Lavoro) & "</span></td>"
                Else
                    Stringa &= "<td style=""text-align: center; vertical-align: middle; border-right: 1px solid #847f7f; padding: 2px;""></td>"
                End If

                Stringa &= "<td style=""text-align: center; vertical-align: middle; border-right: 1px solid #847f7f; padding: 2px;"">" & icona & "</td>"

                If UltimaCommessa <> Rec("Commessa").Value.ToString.Trim Then
                    UltimaCommessa = Rec("Commessa").Value.ToString.Trim
                    Stringa &= "<td style=""text-align: center; vertical-align: middle; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Commessa").Value.ToString.Trim) & "</span></td>"
                Else
                    Stringa &= "<td style=""text-align: center; vertical-align: middle; border-right: 1px solid #847f7f; padding: 2px;""></td>"
                End If

                If UltimoIndirizzo <> Rec("Indirizzo").Value.ToString.Trim Then
                    UltimoIndirizzo = Rec("Indirizzo").Value.ToString.Trim
                    Stringa &= "<td style=""text-align: center; vertical-align: left; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Indirizzo").Value.ToString.Trim).Replace(",", "<br />") & "</span></td>"
                Else
                    Stringa &= "<td style=""text-align: center; vertical-align: left; border-right: 1px solid #847f7f; padding: 2px;""></td>"
                End If

                Stringa &= "<td style=""text-align: left; vertical-align: top; font-style: italic; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Descrizione").Value.ToString.Trim).Replace(".", "<br />") & "</span></td>"
                Stringa &= "<td style=""text-align: center; vertical-align: middle; border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Linguaggio").Value.ToString.Trim) & "</span></td>"
                sDataInizio = Rec("DataInizio").Value.ToString
                sDataFine = Rec("DataFine").Value.ToString
                sDataInizio = Mid(sDataInizio, 1, 2) & "/" & Mid(sDataInizio, 4, 2) & "/" & Mid(sDataInizio, 7, 4)
                sDataFine = Mid(sDataFine, 1, 2) & "/" & Mid(sDataFine, 4, 2) & "/" & Mid(sDataFine, 7, 4)
                Stringa += "<td align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & sDataInizio & "</span></td>"
                Stringa &= "<td align=""right"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & sDataFine & "</span></td>"

                If Rec("DataFine").Value.ToString <> "" And Rec("DataInizio").Value.ToString <> "" Then
                    Dim d1 As Date = Rec("DataFine").Value
                    Dim d2 As Date = Rec("DataInizio").Value
                    Dim diff As Long = Math.Abs(DateDiff(DateInterval.Day, d1, d2))
                    Dim giorniCommessa As Integer = 0
                    Dim mesiCommessa As Integer = 0
                    Dim anniCommessa As Integer = 0
                    Dim giorniMese As Integer = 30

                    Do While diff > giorniMese
                        mesiCommessa += 1
                        If mesiCommessa = 12 Then
                            mesiCommessa = 1
                            anniCommessa += 1
                        End If
                        diff -= giorniMese
                        If giorniMese = 30 Then
                            giorniMese = 31
                        Else
                            giorniMese = 30
                        End If
                    Loop
                    If diff = 30 Or diff = 31 Then
                        mesiCommessa += 1
                        If mesiCommessa = 12 Then
                            mesiCommessa = 0
                            anniCommessa += 1
                        End If
                        giorniCommessa = 0
                    Else
                        giorniCommessa = diff
                    End If

                    Dim stringaTempo As String = ""
                    If giorniCommessa > 0 And mesiCommessa > 0 And anniCommessa > 0 Then
                        stringaTempo = "Anni " & anniCommessa & "<br />Mesi " & mesiCommessa & "<br />Giorni " & giorniCommessa
                    Else
                        If giorniCommessa > 0 And mesiCommessa > 0 And anniCommessa = 0 Then
                            stringaTempo = "Mesi " & mesiCommessa & "<br />Giorni " & giorniCommessa
                        Else
                            If giorniCommessa > 0 And mesiCommessa = 0 And anniCommessa = 0 Then
                                stringaTempo = "Giorni " & giorniCommessa
                            Else
                                If giorniCommessa = 0 And mesiCommessa > 0 And anniCommessa > 0 Then
                                    stringaTempo = "Anni " & anniCommessa & "<br />Mesi " & mesiCommessa
                                Else
                                    If giorniCommessa = 0 And mesiCommessa = 0 And anniCommessa > 0 Then
                                        stringaTempo = "Anni " & anniCommessa
                                    Else
                                        If giorniCommessa = 0 And mesiCommessa > 0 And anniCommessa = 0 Then
                                            stringaTempo = "Mesi " & mesiCommessa
                                        Else
                                            stringaTempo = "---"
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If UltimaDifferenzaTempi <> stringaTempo Then
                        UltimaDifferenzaTempi = stringaTempo
                        Stringa &= "<td align=""right"" style=""min-width: 80px;""><span class=""ClassTitoloTesto"">" & stringaTempo & "</span></td>"
                    Else
                        Stringa &= "<td style=""text-align: center; vertical-align: middle; padding: 2px;""></td>"
                    End If

                Else
                    UltimaDifferenzaTempi = ""
                    Stringa &= "<td align=""right"" style=""min-width: 80px;""></td>"
                End If

                Stringa &= "</tr>"

                Rec.MoveNext()
            Loop
            Stringa &= "</tbody>"
            Stringa &= "</table>"
            Rec.Close()

            Stringa &= "<p style=""page-break-before: always""></p>"

            ' Linguaggi
            Stringa &= "<hr /><center><span class=""ClassTitoloSezione"">Linguaggi conosciuti:</span></center><hr />"
            Sql = "Select idLinguaggio, Linguaggio, B.Conoscenza From Linguaggi A " &
                "Left Join LivelloConoscenze B On A.idConoscenza=B.idConoscenza " &
                "Where Eliminato='N' And idLinguaggio<>12 And Conoscenza Is Not Null " &
                "Order By Linguaggio"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Stringa &= "<table class=""bordatosfumato"" cellpadding=""2"" cellspacing=""0"">"
            Stringa &= "<thead>"
            Stringa &= "<tr style=""background-color:" & coloreHead & "; "">"
            Stringa &= "<th style=""text-align: center;"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Linguaggio</span></th>"
            Stringa &= "<th style=""text-align: center;"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">Conoscenza</span></th>"
            Stringa &= "<th style=""text-align: center;"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"" style="" color: #166f19; font-size: 15px;"">" & SistemaCaratteriSpeciali("Num. Attività") & "</span></th>"
            Stringa &= "</tr>"
            Stringa &= "</thead>"
            Stringa &= "<tfoot>"
            Stringa &= "<tr><td></td></tr>"
            Stringa &= "</tfoot>"
            Stringa &= "<tbody>"
            Pari = True
            Do Until Rec.Eof
                If Pari Then
                    Pari = False
                    Stringa &= "<tr style=""background-color: " & ColoreBackgroundDispari & "; "">"
                Else
                    Pari = True
                    Stringa &= "<tr style=""background-color: " & ColoreBackgroundPari & "; "">"
                End If

                Dim Quanti As Integer

                Sql = "Select Count(*) As Quanti From " &
                    "Applicazioni Where " &
                    "idUtente=" & idUtente & " And " &
                    "idLinguaggio=" & Rec("idLinguaggio").Value
                RecC = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If RecC(0).Value Is DBNull.Value Then
                    Quanti = 0
                Else
                    Quanti = RecC(0).value
                End If
                RecC.Close()

                Stringa &= "<td align=""left"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Linguaggio").Value.ToString.Trim) & "</span></td>"
                Stringa &= "<td align=""left"" style=""border-right: 1px solid #847f7f; padding: 2px;""><span class=""ClassTitoloTesto"">" & SistemaCaratteriSpeciali(Rec("Conoscenza").Value.ToString.Trim) & "</span></td>"
                Stringa &= "<td align=""right"" style=""padding: 2px;""><span class=""ClassTitoloTesto"">" & Quanti & "</span></td>"
                Stringa &= "</tr>"

                Rec.MoveNext()
            Loop
            Stringa &= "</tbody>"
            Stringa &= "</table>"
            Rec.Close()

            ' Giorni lavorati
            Sql = " Select Sum(GiorniLavorati) As Lavorati, Sum(Ferie) as GiorniFerie, Sum(Malattia) As GiorniMalattia, Sum(Permessi) As GiorniPermesso, Sum(LavoroCasa) As GiorniLavoroCasa, Sum(GiorniTotali) As Totali From ( "
            Sql &= "Select Sum(Quanto) / 8 As GiorniLavorati, 0 As Ferie, 0 As Malattia, 0 As Permessi, 0 As LavoroCasa, 0 As GiorniTotali From Orari Where Quanto>0 And Entrata Is Not Null "
            Sql &= "Union All "
            Sql &= "Select Sum(Convert(NUMERIC(4, 2), SUBSTRING(Misti, CHARINDEX('N',Misti)+1,CHARINDEX(';',Misti)-2)))/8 As GiorniLavorati, 0 As Ferie, 0 As Malattia, 0 As Permessi, 0 As LavoroCasa, 0 As GiorniTotali From Orari Where Quanto=-3 And Entrata Is Not Null And SUBSTRING(Misti,1,1)='N' "
            Sql &= "Union All "
            Sql &= "Select 0 As GiorniLavorati, 0 As Ferie, 0 As Malattia, 0 As Permessi, 0 As LavoroCasa, Count(*) As GiorniTotali From Orari Where Quanto Is Not Null And Quanto>0 "
            Sql &= "Union All "
            Sql &= "Select 0 As GiorniLavorati, Count(*) As Ferie, 0 As Malattia, 0 As Permessi, 0 As LavoroCasa, 0 As GiorniTotali From Orari Where Quanto =-1 "
            Sql &= "Union All "
            Sql &= "Select 0 As GiorniLavorati, 0 As Ferie, Count(*) As Malattia, 0 As Permessi, 0 As LavoroCasa, 0 As GiorniTotali From Orari Where Quanto =-2 "
            Sql &= "Union All "
            Sql &= "Select 0 As GiorniLavorati, 0 As Ferie, 0 As Malattia, Sum(Convert(NUMERIC(4, 2), REPLACE(REPLACE(SUBSTRING(Misti, CHARINDEX('P',Misti)+1,CHARINDEX(';',Misti)-2),'M',''),';','')))/8 As Permessi, 0 As LavoroCasa, 0 As GiorniTotali From Orari Where Quanto=-3 And Misti<>'' "
            Sql &= "Union All "
            Sql &= "Select 0 As GiorniLavorati, 0 As Ferie, 0 As Malattia, 0 As Permessi, Count(*) * 8 As LavoroCasa, 0 As GiorniTotali From Orari Where Quanto=-6 "
            Sql &= "Union All "
            Sql &= "Select 0 As GiorniLavorati, 0 As Ferie, Sum(Convert(NUMERIC(4, 2), REPLACE(REPLACE(SUBSTRING(Misti, CHARINDEX('M',Misti)+1,CHARINDEX(';',Misti)-2),'R',''),';','')))/8 As Malattia, 0 As Permessi, 0 As LavoroCasa, 0 As GiorniTotali From Orari Where Quanto=-3  And Misti<>'' "
            Sql &= ") As A  "
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Not Rec.Eof Then
                Stringa &= "<p style=""page-break-before: always""></p>"

                Stringa &= "<hr /><center><span class=""ClassTitoloSezione"">Dettaglio giorni:</span></center><hr />"
                Dim v As Single = Int((Rec("Lavorati").Value.ToString.Trim) * 100) / 100
                Stringa &= "<center><span class=""ClassTitoloTesto"">Giorni lavorati: " & v.ToString.Trim & "</span><br />"
                v = Int((Rec("GiorniFerie").Value.ToString.Trim) * 100) / 100
                Stringa &= "<span class=""ClassTitoloTesto"">Giorni Ferie: " & v.ToString.Trim & "</span><br />"
                v = Int((Rec("GiorniMalattia").Value.ToString.Trim) * 100) / 100
                Stringa &= "<span class=""ClassTitoloTesto"">Giorni Malattia: " & v.ToString.Trim & "</span><br />"
                v = Int((Rec("GiorniPermesso").Value.ToString.Trim) * 100) / 100
                Stringa &= "<span class=""ClassTitoloTesto"">Giorni Permessi: " & v.ToString.Trim & "</span><br />"
                v = Rec("GiorniLavoroCasa").Value.ToString.Trim / 8
                Stringa &= "<span class=""ClassTitoloTesto"">Giorni Lavoro A Casa: " & v.ToString.Trim & "</span>"
                Stringa &= "</center><hr />"
            End If
            Rec.Close

            Dim graficiAVideo As String = ""

            graficiAVideo &= "<div id=""divGrafici"" runat=""server"" style=""width: 100%;"">"
            graficiAVideo &= "<div id=""piechartLinguaggi"" style=""width: 50%; float: left;""></div>"
            graficiAVideo &= "<div id=""piechartCommesse"" style=""width: 50%; float: right;""></div>"
            graficiAVideo &= "</div>"
            graficiAVideo &= "<div id=""divGraficiNascosti"" runat=""server"" style=""visibility: hidden;"">"
            graficiAVideo &= "<div id=""piechartCommesseIMG"" runat="""" ></div>"
            graficiAVideo &= "<div id=""piechartLinguaggiIMG"" runat="""" ></div>"
            graficiAVideo &= "</div>"

            Dim graficiDaSalvare As String = ""

            graficiDaSalvare &= "<div id=""divGrafici"" runat=""server"" class=""bordatosfumato"" style=""display: flex; height:auto;"">"
            graficiDaSalvare &= "<img id=""imgLinguaggi"" style=""width: 50%; float: left;"" src='" & "App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\stat1.jpg" & "' alt='' />"
            graficiDaSalvare &= "<img id=""imgCommesse"" style=""width: 50%; float: right;"" src='" & "App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\stat2.jpg" & "' alt='' />"
            graficiDaSalvare &= "</div>"

            lblCurriculum.Text = Stringa & graficiAVideo

            Dim gf As New GestioneFilesDirectory
            Dim url As String
            If HttpContext.Current.Request.Url.Port <> 80 Then
                url = HttpContext.Current.Request.Url.Host & ":" & HttpContext.Current.Request.Url.Port & "/" & HttpContext.Current.Request.ApplicationPath & "/"
            Else
                url = HttpContext.Current.Request.Url.Host & "/" & HttpContext.Current.Request.ApplicationPath & "/"
            End If
            Do While url.Contains("//")
                url = url.Replace("//", "/")
            Loop
            url = "http://" & url

            Stringa &= graficiDaSalvare
            Stringa = Stringa.Replace("App_Themes", url & "App_Themes")

            Dim nomeFile As String = Server.MapPath(".") & "/App_Themes/Standard/Images/" & idUtente & "-" & Utenza & "/cv" & Utenza & ".html"

            Try
                IO.File.Delete(nomeFile)
            Catch ex As Exception

            End Try

            Dim Pre As String = ""
            Dim Post As String = ""

            Pre &= "<head runat=""server"">"
            Pre &= "<style> "
            Pre &= "@page {size: 210mm 297mm; margin: 30mm;} "

            Pre &= "table {page-break-after:auto} "
            Pre &= "tr    {page-break-inside:avoid; page-break-after:auto } "
            Pre &= "td    {page-break-inside:avoid; page-break-after:auto } "
            Pre &= "thead {display:table-header-group } "
            Pre &= "tfoot {display:table-footer-group } "

            Pre &= ".ClassTitoloSezione {font-family: Verdana;  vertical-align: middle; color: #008800; font-size: 15px; font-weight: bold; text-shadow: 2px 2px #e8e8e8;}"
            Pre &= ".ClassTitoloTesto {font-family: inherit; vertical-align: middle; color: #2c3644; font-size: 12px;  text-shadow: 1px 1px #d8daff;}"

            Pre &= "html:before "
            Pre &= "{ "
            Pre &= "    height: 20px; "
            Pre &= "    left:10; right:10; top:10; "
            Pre &= "} "

            Pre &= "html:after "
            Pre &= "{ "
            Pre &= "    width:20px; "
            Pre &= "    top:10; right:10; bottom:10; "
            Pre &= "} "

            Pre &= "body:before "
            Pre &= "{ "
            Pre &= "    height:20px; "
            Pre &= "    right:10; bottom:10; left:10; "
            Pre &= "} "

            Pre &= ".bordatosfumato {"
            Pre &= "    width: 99%;"
            Pre &= "    padding: 5px;"
            Pre &= "    page-break-after:auto"
            Pre &= "    border: solid 1px #c5c3b6;"
            Pre &= "    -webkit-border-radius: 0.55em 0.55em 0.55em 0.55em;"
            Pre &= "    -moz-border-radius: 0.55em 0.55em 0.55em 0.55em;"
            Pre &= "    border-radius: 0.55em 0.55em 0.55em 0.55em;"
            Pre &= "    -webkit-box-shadow inset 2px 2px 10px 0px rgba(169, 168, 164, 0.9), inset -2px -2px 10px 0px rgba(117, 117, 117, 0.9);"
            Pre &= "    -moz-box-shadow: inset 2px 2px 10px 0px rgba(169, 168, 164, 0.9), inset -2px -2px 10px 0px rgba(117, 117, 117, 0.9);"
            Pre &= "    box-shadow: inset 2px 2px 10px 0px rgba(169, 168, 164, 0.9), inset -2px -2px 10px 0px rgba(117, 117, 117, 0.9);"
            Pre &= "}"

            Pre &= "body:after{"
            Pre &= "    width:20px; "
            Pre &= "    top: 10; bottom: 10; left: 10; "
            Pre &= "} "
            Pre &= "</style>"
            Pre &= "</head>"
            Dim url2 As String = url
            url2 = url2.Replace("\", "/")
            ' Pre &= "<body style='background: url(" & url2 & "App_Themes/Standard/Images/cartaCV.jpg);'>"
            Pre &= "<body>"

            Post &= "</body>"

            Sql = "Select Linguaggio, Count(*) As Progetti, (Sum(DATEDIFF(DAY, Convert(Date, DataInizio,103), Convert(Date, Case DataFine When '' THEN Convert(date, GETDATE(),103) Else Convert(date, DataFine, 103) END , 103)))) AS Differenza From Applicazioni A " &
        "Left Join Linguaggi B On A.idLinguaggio = B.idLinguaggio " &
        "Left Join Commesse C On A.idCommessa = C.Codice " &
        "Where Progressivo=1 " &
        "Group By Linguaggio " &
        "Order By 3 Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Dim ArrayLinguaggi As String = ""
            Dim ArrayLinguaggiValori As String = ""
            Do Until Rec.Eof
                ArrayLinguaggi &= Rec("Linguaggio").Value & " (Giorni " & Rec("Differenza").Value & ");"
                ArrayLinguaggiValori &= Rec("Differenza").Value & ";"

                Rec.MoveNext
            Loop
            Rec.Close

            Dim sb0 As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb0.Append("<script type='text/javascript' language='javascript'>")
            sb0.Append("     setPath('" & (Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\").Replace("\", "§") & "');")
            sb0.Append("</script>")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSPath", sb0.ToString(), False)

            Dim sb1 As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb1.Append("<script type='text/javascript' language='javascript'>")
            sb1.Append("     drawChart('piechartLinguaggi', 'Uso Linguaggi', '" & ArrayLinguaggi & "', '" & ArrayLinguaggiValori & "');")
            sb1.Append("</script>")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JPie1", sb1.ToString(), False)

            Sql = "Select B.Descrizione, Count(*) As Quanti From Orari A " &
        "Left Join Commesse B On A.CodCommessa = B.Codice " &
        "Where B.Descrizione Is Not Null " &
        "Group By B.Descrizione " &
        "Order By 2 Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Dim ArrayCommesse As String = ""
            Dim ArrayCommesseValori As String = ""
            Do Until Rec.Eof
                ArrayCommesse &= Rec("Descrizione").Value & " (Giorni " & Rec("Quanti").Value & ");"
                ArrayCommesseValori &= Rec("Quanti").Value & ";"

                Rec.MoveNext
            Loop
            Rec.Close

            Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb2.Append("<script type='text/javascript' language='javascript'>")
            sb2.Append("     drawChart('piechartCommesse', 'Tempi commesse', '" & ArrayCommesse & "', '" & ArrayCommesseValori & "');")
            sb2.Append("</script>")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JPie2", sb2.ToString(), False)

            Stringa = Pre & Stringa & Post

            gf.CreaAggiornaFile(nomeFile, Stringa)
            gf = Nothing

            ConnSQL.Close()

            hdnNomeFileHTML.Value = nomeFile
        End If
    End Sub

    <System.Web.Services.WebMethod()>
    Public Shared Function SalvaImmagini(ByVal Path As String, ByVal imm1 As String, ByVal imm2 As String) As String
        Dim pathDest As String = Path.Replace("§", "\")

        GetImageFromBase64(pathDest & "stat1.jpg", imm1)
        GetImageFromBase64(pathDest & "stat2.jpg", imm2)

        Return ""
    End Function

    Public Shared Sub GetImageFromBase64(ByVal NomeFile As String, ByVal Base64String As String)
        Try
            File.Delete(NomeFile)
        Catch ex As Exception

        End Try

        Base64String = Mid(Base64String, Base64String.IndexOf(",") + 2, Base64String.Length)
        Dim Righe() As String = Base64String.Split(Chr(10))
        Try
            Dim B As Byte()
            Dim c As Integer = 0
            For Each S As String In Righe
                If S.Replace(Chr(13), "").Replace(Chr(10), "").Replace(" ", "") <> "" Then
                    B = Convert.FromBase64String(S)

                    Dim fs As New FileStream(NomeFile, FileMode.Append)
                    fs.Write(B, 0, B.Length)
                    fs.Close()
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Protected Sub cmdScaricaPDF_Click(sender As Object, e As EventArgs) Handles cmdScaricaPDF.Click
        If hdnNomeFileHTML.Value <> "" Then
            Dim nomeFilePDF As String = "cv" & Utenza & ".pdf"

            Dim converter As New SelectPdf.HtmlToPdf()
            hdnNomeFileHTML.Value = hdnNomeFileHTML.Value.Replace("/", "\")
            Dim doc As SelectPdf.PdfDocument = converter.ConvertUrl(hdnNomeFileHTML.Value)
            doc.Save(Response, False, nomeFilePDF)
            doc.Close()
        End If
    End Sub

    Protected Sub imgGiorniLavoroACasa_Click(sender As Object, e As ImageClickEventArgs) Handles imgGiorniLavoroACasa.Click
        SpegneDiv()
        ModalitaStatistica = "GIORNILAVOROCASA"
        divLavoroACasa.Visible = True
        lblTitoloMaschera.Text = "Statistica giorni di lavoro a casa"

        CaricaGiorniLavoroCasa()
    End Sub

    Private Sub CaricaGiorniLavoroCasa()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim dPos As New DataColumn("Pos")
            Dim dLavoro As New DataColumn("Societa")
            Dim dGiorno As New DataColumn("Giorno")
            Dim dMese As New DataColumn("Mese")
            Dim dOre As New DataColumn("Ore")
            Dim dGiorni As New DataColumn("Giorni")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Contatore As Integer = 0
            Dim Vecchio As Single = -2
            Dim Mesi() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPos)
            dttTabella.Columns.Add(dLavoro)
            dttTabella.Columns.Add(dGiorno)
            dttTabella.Columns.Add(dMese)
            dttTabella.Columns.Add(dOre)
            dttTabella.Columns.Add(dGiorni)

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And B.Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Dim Splitta As String = ""
            Dim Splitta2 As String = ""

            If chkSplitta.Checked = True Then
                Splitta = "B.Lavoro, "
                Splitta2 = "B.Lavoro, "
            Else
                Splitta = "'' As Lavoro, "
                Splitta2 = " "
            End If

            Sql = "Select " & Splitta & " Giorno, Mese, Sum(8) As Ore, Sum(8)/8 As Giorni From " &
                "" & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente = B.idUtente And A.idLavoro = B.idLavoro " &
                "Where A.idUtente = " & idUtente & " And Quanto = -6 " &
                " " & Altro & " " &
                "Group By " & Splitta2 & " Giorno, Mese " &
                "Order By 4 desc, Mese, Giorno"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                If Vecchio <> Rec("Ore").Value Then
                    Vecchio = Rec("Ore").Value
                    Contatore += 1
                End If

                rigaR = dttTabella.NewRow()
                rigaR(0) = Contatore
                rigaR(1) = "" & Rec("Lavoro").Value
                rigaR(2) = "" & Rec("Giorno").Value
                rigaR(3) = "" & Mesi(Rec("Mese").Value - 1)
                rigaR(4) = ScriveNumero("" & Rec("Ore").Value)
                rigaR(5) = ScriveNumero("" & Rec("Giorni").Value)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdLavoroCasa.DataSource = dttTabella
            grdLavoroCasa.DataBind()
            grdLavoroCasa.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Private Sub CaricaStatisticheLavoroCasa(Societa As String, Giorno As String, Mese As String)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Malattia As String

            lblStatistiche.Text = "Statistiche su lavoro a casa: " & Giorno & " " & Mese

            Dim sMese As String = PrendeMese(Mese)

            Dim Altro As String = ""

            If Societa <> "" And Societa <> "&nbsp;" Then
                Altro = "And B.Lavoro='" & Societa.Replace("'", "''") & "' "
            End If

            Dim dCampo1 As New DataColumn("Societa")
            Dim dCampo2 As New DataColumn("Giorno")
            Dim dCampo3 As New DataColumn("Ore")
            'Dim rigaR As DataRow
            'Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCampo1)
            dttTabella.Columns.Add(dCampo2)
            dttTabella.Columns.Add(dCampo3)

            Sql = "Select A.*, B.Lavoro From " & PrefissoTabelle & "Orari A Left Join " & PrefissoTabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro " &
                "Where A.idUtente=" & idUtente & " And " &
                "Giorno = " & Giorno & " And " &
                "Mese = " & sMese & " And " &
                "Quanto = -6 " &
                "Order By Anno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Rec("Lavoro").Value
                rigaR(1) = MetteMaiuscole(Datella.ToString("dddd")) & " " & sDatella
                rigaR(2) = "8"
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

End Class