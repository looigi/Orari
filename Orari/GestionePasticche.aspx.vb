Public Class GestionePasticche
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            CaricaPasticche()

            divStatistiche.Visible = False
        End If
    End Sub

    Private Sub CaricaPasticche()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim RecC As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Portata As String

            Dim dPortata As New DataColumn("Pasticca")
            Dim dRicorrenze As New DataColumn("Ricorrenze")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPortata)
            dttTabella.Columns.Add(dRicorrenze)

            Sql = "Select idPasticca, descPasticca, Count(*) As Quanti From " & _
                "" & prefissotabelle & "Pasticche Where " & _
                "idUtente=" & idUtente & " And " & _
                "Eliminato='N' " & _
                "Group By idPasticca, descPasticca " & _
                "Order By descPasticca"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            QuanteRighePortata = 0
            Erase idRigaPortata
            Do Until Rec.Eof
                Portata = "" & Rec("descPasticca").Value

                QuanteRighePortata += 1
                ReDim Preserve idRigaPortata(QuanteRighePortata)
                idRigaPortata(QuanteRighePortata) = Rec("idPasticca").Value

                Dim Quanti As Integer

                Sql = "Select Count(*) As Quanti From " & _
                    "" & prefissotabelle & "AltreInfoPasticca Where " & _
                    "idUtente=" & idUtente & " And " & _
                    "idPasticca=" & Rec("idPasticca").Value
                RecC = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If RecC(0).Value Is DBNull.Value Then
                    Quanti = 0
                Else
                    Quanti = RecC(0).value
                End If
                RecC.Close()

                rigaR = dttTabella.NewRow()
                rigaR(0) = Portata
                rigaR(1) = Quanti
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            lblQuante.Text = "Pasticche: " & QuanteRighePortata - 1
            hdnNumero.Value = ""
            txtPasticca.Text = ""

            grdPasticche.DataSource = dttTabella
            grdPasticche.DataBind()
            grdPasticche.SelectedIndex = -1
        End If
    End Sub

    Private Sub grdPranzo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPasticche.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdPasticche.PageIndex * 10)

            hdnIdRiga.Value = idRigaPortata(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub grdPranzo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPasticche.PageIndexChanging
        grdPasticche.PageIndex = e.NewPageIndex
        grdPasticche.DataBind()

        CaricaPasticche()
    End Sub

    Protected Sub EliminaPasticca(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdPasticche.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Portata As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update " & prefissotabelle & "Pasticche Set " & _
                "Eliminato='S' " & _
                "Where idUtente=" & idUtente & " And idPasticca=" & hdnIdRiga.Value
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            CaricaPasticche()

            VisualizzaMessaggioInPopup("Pasticca eliminata", Master)
        End If
    End Sub

    Protected Sub ModificaPasticca(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdPasticche.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Portata As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        txtPasticca.Text = Portata
        hdnNumero.Value = hdnIdRiga.Value
    End Sub

    Protected Sub imgNuovo_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuovo.Click
        txtPasticca.Text = ""
        hdnNumero.Value = ""
    End Sub

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        If Request.QueryString("Maschera") = "GestTabelle" Then
            Response.Redirect("GestioneTabelle.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
        Else
            Response.Redirect("ModificaGiorno.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
        End If
    End Sub

    Protected Sub imgSalva_Click(sender As Object, e As ImageClickEventArgs) Handles imgSalva.Click
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            If hdnNumero.Value = "" Then
                Dim idPasticca As Integer

                Sql = "Select Max(idPasticca)+1 From " & prefissotabelle & "Pasticche Where idUtente=" & idUtente
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).value Is DBNull.Value = False Then
                    idPasticca = Rec(0).Value
                Else
                    idPasticca = 1
                End If
                Rec.Close()

                Sql = "Insert Into " & prefissotabelle & "Pasticche Values (" & _
                    " " & idUtente & ", " & _
                    " " & idPasticca & ", " & _
                    "'" & MetteMaiuscole(txtPasticca.Text.Replace("'", "''")) & "', " & _
                    "'N'" & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            Else
                Sql = "Update " & prefissotabelle & "Pasticche Set " & _
                    "descPasticca='" & MetteMaiuscole(txtPasticca.Text.Replace("'", "''")) & "' " & _
                    "Where idUtente=" & idUtente & " And idPasticca=" & hdnNumero.Value
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            CaricaPasticche()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Protected Sub StatistichePasticche(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdPasticche.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Pasticca As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        CosaSelezionato = Pasticca
        idSelezionato = hdnIdRiga.Value

        CaricaStatistiche()

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatistiche()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatPasticche.Text = "Statistiche su pasticche: " & CosaSelezionato

            Dim dMezzo As New DataColumn("Pasticca")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dMezzo)

            Sql = "Select * From " & prefissotabelle & "AltreInfoPasticca " & _
                "Where idUtente=" & idUtente & " And " & _
                "idPasticca = " & idSelezionato & " " & _
                "Order By Anno Desc, Mese Desc, Giorno Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.eof
                Datella = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value
                sDatella = Datella.Day.ToString("00") & "/" & Datella.Month.ToString("00") & "/" & Datella.Year

                rigaR = dttTabella.NewRow()
                rigaR(0) = Datella.ToString("dddd") & " " & sDatella
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdStatistiche.DataSource = dttTabella
            grdStatistiche.DataBind()
            grdStatistiche.SelectedIndex = -1
        End If
    End Sub

    Private Sub grdStatistiche_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStatistiche.PageIndexChanging
        grdStatistiche.PageIndex = e.NewPageIndex
        grdStatistiche.DataBind()

        CaricaStatistiche()
    End Sub

    Protected Sub imgChiudeStat_Click(sender As Object, e As ImageClickEventArgs) Handles imgChiudeStat.Click
        divStatistiche.Visible = False
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub
End Class