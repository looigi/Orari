Public Class GestioneTempo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            CaricaTempo()

            divStatistiche.Visible = False
        End If
    End Sub

    Private Sub CaricaTempo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim RecC As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Portata As String

            Dim dPortata As New DataColumn("Tempo")
            Dim dRicorrenze As New DataColumn("Ricorrenze")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPortata)
            dttTabella.Columns.Add(dRicorrenze)

            'Sql = "Select idTempo, descTempo, Count(*) As Quanti From " & _
            '    "" & prefissotabelle & "Tempo Where " & _
            '    "idUtente=" & idUtente & " And " & _
            '    "Eliminato='N' " & _
            '    "Group By idTempo, descTempo " & _
            '    "Order By descTempo"
            Sql = "Select idTempo, descTempo From " & _
                "" & prefissotabelle & "Tempo Where " & _
                "idUtente=" & idUtente & " And " & _
                "Eliminato='N' " & _
                "Order By descTempo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            QuanteRighePortata = 0
            Erase idRigaPortata
            Do Until Rec.Eof
                Portata = "" & Rec("descTempo").Value

                QuanteRighePortata += 1
                ReDim Preserve idRigaPortata(QuanteRighePortata)
                idRigaPortata(QuanteRighePortata) = Rec("idTempo").Value

                Dim Quanti As Integer

                Sql = "Select Count(*) As Quanti From " & _
                    "" & prefissotabelle & "AltreInfoTempo Where " & _
                    "idUtente=" & idUtente & " And " & _
                    "idTempo=" & Rec("idTempo").Value
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

            lblQuante.Text = "Tipologie: " & QuanteRighePortata - 1
            hdnNumero.Value = ""
            txtTempo.Text = ""
            imgTempo.Visible = False
            divUpload.Visible = False

            grdTempo.DataSource = dttTabella
            grdTempo.DataBind()
            grdTempo.SelectedIndex = -1
        End If
    End Sub

    Private Sub grdPranzo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdTempo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdTempo.PageIndex * 10)

            hdnIdRiga.Value = idRigaPortata(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub grdPranzo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdTempo.PageIndexChanging
        grdTempo.PageIndex = e.NewPageIndex
        grdTempo.DataBind()

        CaricaTempo()
    End Sub

    Protected Sub EliminaTempo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdTempo.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Portata As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update " & prefissotabelle & "Tempo Set " & _
                "Eliminato='S' " & _
                "Where idUtente=" & idUtente & " And idTempo=" & hdnIdRiga.Value
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            CaricaTempo()

            VisualizzaMessaggioInPopup("Portata eliminata", Master)
        End If
    End Sub

    Protected Sub ModificaTempo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdTempo.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Portata As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        txtTempo.Text = Portata
        hdnNumero.Value = hdnIdRiga.Value
        DisegnaImmagineTempo()
        divUpload.Visible = True
    End Sub

    Private Sub DisegnaImmagineTempo()
        If txtTempo.Text = "" Then
            imgTempo.Visible = False
        Else
            Dim idTempo As Integer = PrendeIdDaCombo(txtTempo.Text, "" & prefissotabelle & "Tempo", "DescTempo")
            Dim PercImmagineTempo As String = "App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/Tempo/" & idTempo & ".png"

            imgTempo.ImageUrl = PercImmagineTempo
            imgTempo.Visible = True
        End If
    End Sub

    Private Function PrendeIdDaCombo(Cosa As String, Tabella As String, CampoDesc As String) As Integer
        Dim id As Integer = -1

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & Tabella & " Where idUtente=" & idUtente & " And " & CampoDesc & "='" & Cosa.Replace("'", "''") & "' "
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                id = Rec(1).Value
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Return id
    End Function

    Protected Sub imgNuovo_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuovo.Click
        txtTempo.Text = ""
        hdnNumero.Value = ""
        imgTempo.Visible = False
        divUpload.Visible = False
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
                Dim idTempo As Integer

                Sql = "Select Max(idTempo)+1 From " & prefissotabelle & "Tempo Where idUtente=" & idUtente
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).value Is DBNull.Value = False Then
                    idTempo = Rec(0).Value
                Else
                    idTempo = 1
                End If
                Rec.Close()

                Sql = "Insert Into " & prefissotabelle & "tempo Values (" & _
                    " " & idUtente & ", " & _
                    " " & idTempo & ", " & _
                    "'" & txtTempo.Text.Replace("'", "''") & "', " & _
                    "'N'" & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            Else
                Sql = "Update " & prefissotabelle & "Tempo Set " & _
                    "descTempo='" & txtTempo.Text.Replace("'", "''") & "' " & _
                    "Where idUtente=" & idUtente & " And idTempo=" & hdnNumero.Value
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            CaricaTempo()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Protected Sub imgCarica_Click(sender As Object, e As ImageClickEventArgs) Handles imgCarica.Click
        If FileUpload1.HasFile = False Then
            VisualizzaMessaggioInPopup("Selezionare un file immagine", Master)
            Exit Sub
        End If

        Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim idTempo As Integer

        Sql = "Select * From " & prefissotabelle & "Tempo Where idUtente=" & idUtente & " And descTempo='" & txtTempo.Text.Replace("'", "''") & "'"
        Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
        If Rec.Eof = False Then
            idTempo = Rec(1).Value
        Else
            idTempo = 1
        End If
        Rec.Close()

        Dim PercImmagineTempo As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\Tempo\" & idTempo & ".png"

        Try
            Kill(PercImmagineTempo)
        Catch ex As Exception

        End Try

        FileUpload1.SaveAs(PercImmagineTempo)

        ResizeImage(PercImmagineTempo, 100, 100)

        DisegnaImmagineTempo()
    End Sub

    Protected Sub StatisticheTempo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdTempo.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Tempo As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        CosaSelezionato = Tempo
        idSelezionato = hdnIdRiga.Value

        CaricaStatistiche()

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatistiche()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatPortata.Text = "Statistiche su portate: " & CosaSelezionato

            Dim dMezzo As New DataColumn("Tempo")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dMezzo)

            Sql = "Select * From " & prefissotabelle & "AltreInfoTempo " & _
                "Where idUtente=" & idUtente & " And " & _
                "idTempo = " & idSelezionato & " " & _
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