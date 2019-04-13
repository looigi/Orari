Public Class GestioneMezzi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            CaricaCombo()
            CaricaMezzi()
            CaricaMezziStandard()
            divStatistiche.Visible = False
        End If
    End Sub

    Private Sub CaricaCombo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Mezzo As String

            Sql = "Select * From " & prefissotabelle & "Mezzi Where idUtente=" & idUtente & " And Eliminato='N' Order By DescMezzo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbStandard.Items.Add("")
            Do Until Rec.Eof
                Mezzo = "" & Rec("DescMezzo").Value
                If Rec("Dettaglio").Value.ToString.Trim <> "" Then Mezzo += " (" & Rec("Dettaglio").Value & ")"

                cmbStandard.Items.Add(Mezzo)

                Rec.MoveNext()
            Loop
            Rec.Close()
        End If
    End Sub

    Private Sub CaricaMezzi()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim RecC As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Mezzo As String

            Dim dMezzo As New DataColumn("Mezzo")
            Dim dRicorrrenze As New DataColumn("Ricorrenze")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()
            Dim Post As String = ""

            dttTabella = New DataTable
            dttTabella.Columns.Add(dMezzo)
            dttTabella.Columns.Add(dRicorrrenze)

            If Request.QueryString("Tipo") = "Andata" Then
                Post = ""
            Else
                Post = "Rit"
            End If

            Sql = "Select idMezzo, descMezzo, Dettaglio, Count(*) As Quanti From " & PrefissoTabelle & "Mezzi  " &
                "Where idUtente=" & idUtente & " And " &
                "Eliminato='N' " &
                "Group By idMezzo, descMezzo, Dettaglio " &
                "Order By descMezzo, Dettaglio"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            QuanteRighePortata = 0
            Erase idRigaPortata
            Do Until Rec.Eof
                Mezzo = "" & Rec("descMezzo").Value
                If Rec("Dettaglio").Value.ToString.Trim <> "" Then Mezzo += " (" & Rec("Dettaglio").Value & ")"

                QuanteRighePortata += 1
                ReDim Preserve idRigaPortata(QuanteRighePortata)
                idRigaPortata(QuanteRighePortata) = Rec("idMezzo").Value

                Dim Quanti As Integer

                Sql = "Select Count(*) As Quanti From " & _
                    "" & prefissotabelle & "AltreInfoMezzi" & Post & " Where " & _
                    "idUtente=" & idUtente & " And " & _
                    "idMezzo=" & Rec("idMezzo").Value
                RecC = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If RecC(0).Value Is DBNull.Value Then
                    Quanti = 0
                Else
                    Quanti = RecC(0).value
                End If
                RecC.Close()

                rigaR = dttTabella.NewRow()
                rigaR(0) = Mezzo
                rigaR(1) = Quanti
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            If Request.QueryString("Tipo") = "Andata" Then
                lblQuante.Text = "Mezzi di andata: " & QuanteRighePortata - 1
                lblSotto.Text = "Mezzi standard per l'andata"
            Else
                lblQuante.Text = "Mezzi di ritorno: " & QuanteRighePortata - 1
                lblSotto.Text = "Mezzi standard per il ritorno"
            End If
            hdnNumero.Value = ""
            txtMezzo.Text = ""

            grdMezzi.DataSource = dttTabella
            grdMezzi.DataBind()
            grdMezzi.SelectedIndex = -1

        End If
    End Sub

    Private Sub grdPranzo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdMezzi.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdMezzi.PageIndex * 5)

            hdnIdRiga.Value = idRigaPortata(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub grdPranzo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdMezzi.PageIndexChanging
        grdMezzi.PageIndex = e.NewPageIndex
        grdMezzi.DataBind()

        CaricaMezzi()
    End Sub

    Protected Sub EliminaMezzo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdMezzi.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Portata As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update " & prefissotabelle & "Mezzi Set " & _
                "Eliminato='S' " & _
                "Where idUtente=" & idUtente & " And idMezzo=" & hdnIdRiga.Value
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            CaricaMezzi()

            VisualizzaMessaggioInPopup("Mezzo eliminato", Master)
        End If
    End Sub

    Protected Sub ModificaMezzo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdMezzi.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Mezzo As String = Di.Cells(1).Text
        Dim Dettaglio As String = ""
        Dim Progressivo As Integer = hdnIdRiga.Value

        If InStr(Mezzo, "(") > 0 Then
            Dettaglio = Mid(Mezzo, InStr(Mezzo, "(") + 1, Len(Mezzo))
            Dettaglio = Mid(Dettaglio, 1, InStr(Dettaglio, ")") - 1)
            Mezzo = Mid(Mezzo, 1, InStr(Mezzo, "(") - 1).Trim
        End If
        txtMezzo.Text = Mezzo
        txtdettaglio.Text = dettaglio
        hdnNumero.Value = hdnIdRiga.Value
    End Sub

    Protected Sub imgNuovo_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuovo.Click
        txtMezzo.Text = ""
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
                Dim idMezzo As Integer

                Sql = "Select Max(idMezzo)+1 From " & prefissotabelle & "Mezzi Where idUtente=" & idUtente
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).value Is DBNull.Value = False Then
                    idMezzo = Rec(0).Value
                Else
                    idMezzo = 1
                End If
                Rec.Close()

                Sql = "Insert Into " & prefissotabelle & "Mezzi Values (" & _
                    " " & idUtente & ", " & _
                    " " & idMezzo & ", " & _
                    "'" & MetteMaiuscole(txtMezzo.Text.Replace("'", "''")) & "', " & _
                    "'" & MetteMaiuscole(txtDettaglio.Text.Replace("'", "''")) & "', " & _
                    "'N'" & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            Else
                Sql = "Update " & prefissotabelle & "Mezzi Set " & _
                    "descMezzo='" & MetteMaiuscole(txtMezzo.Text.Replace("'", "''")) & "', " & _
                    "Dettaglio='" & MetteMaiuscole(txtDettaglio.Text.Replace("'", "''")) & "' " & _
                    "Where idUtente=" & idUtente & " And idMezzo=" & hdnNumero.Value
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            CaricaMezzi()
            CaricaCombo()
            CaricaMezziStandard()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Private Sub CaricaMezziStandard()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Tipo As String = Request.QueryString("Tipo")
            Dim Suffisso As String = ""

            Select Case Tipo
                Case "Andata"
                    Suffisso = ""
                Case "Ritorno"
                    Suffisso = "Rit"
            End Select

            Dim dMezzo As New DataColumn("Mezzo")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()
            Dim Mezzo As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dMezzo)

            Sql = "Select A.Progressivo,B.descMezzo, B.Dettaglio From " & prefissotabelle & "MezziStandard" & Suffisso & " A " & _
                "Left Join " & prefissotabelle & "Mezzi B On A.idMezzo=B.idMezzo " & _
                "Where A.idUtente=" & idUtente & " Order By Progressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            QuanteRighePortata = 0
            Erase idRigaPortata
            Do Until Rec.Eof
                Mezzo = "" & Rec("descMezzo").Value
                If Rec("Dettaglio").Value.ToString.Trim <> "" Then Mezzo += " (" & Rec("Dettaglio").Value & ")"

                For i As Integer = 0 To cmbStandard.Items.Count - 1
                    If cmbStandard.Items(i).Text = Mezzo Then
                        cmbStandard.Items.RemoveAt(i)
                        Exit For
                    End If
                Next

                QuanteRighePortata += 1
                ReDim Preserve idRigaPortata(QuanteRighePortata)
                idRigaPortata(QuanteRighePortata) = Rec("Progressivo").Value

                rigaR = dttTabella.NewRow()
                rigaR(0) = Mezzo
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            grdStandard.DataSource = dttTabella
            grdStandard.DataBind()
            grdStandard.SelectedIndex = -1
        End If
    End Sub

    Protected Sub EliminaMezzoStandard(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdStandard.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Mezzo As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        Dim Tipo As String = Request.QueryString("Tipo")
        Dim Suffisso As String

        Select Case Tipo
            Case "Andata"
                Suffisso = ""
            Case "Ritorno"
                Suffisso = "Rit"
        End Select

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Sql As String

            Sql = "Delete From " & prefissotabelle & "MezziStandard" & Suffisso & " " & _
                "Where idUtente=" & idUtente & " " & _
                "And Progressivo=" & Progressivo
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            CaricaMezziStandard()

            cmbStandard.Items.Add(Mezzo)
        End If
    End Sub

    Private Sub grdStandard_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdStandard.PageIndexChanging
        grdStandard.PageIndex = e.NewPageIndex
        grdStandard.DataBind()

        CaricaMezziStandard()
    End Sub

    Private Sub grdStandard_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdStandard.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdStandard.PageIndex * 4)

            hdnIdRiga.Value = idRigaPortata(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Protected Sub AggiungeMezzoStandard(ByVal sender As Object, ByVal e As System.EventArgs)
        If cmbStandard.Text.Trim = "" Then
            Exit Sub
        End If
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Progressivo As Integer

            Dim Tipo As String = Request.QueryString("Tipo")
            Dim Suffisso As String = ""

            Select Case Tipo
                Case "Andata"
                    Suffisso = ""
                Case "Ritorno"
                    Suffisso = "Rit"
            End Select

            Dim Mezzo As String = cmbStandard.Text.Trim
            Dim Dettaglio As String = ""
            Dim Altro As String = ""
            Dim idMezzo As Integer

            If InStr(Mezzo, "(") > 0 Then
                Dettaglio = Mid(Mezzo, InStr(Mezzo, "(") + 1, Len(Mezzo))
                Dettaglio = Mid(Dettaglio, 1, InStr(Dettaglio, ")") - 1)
                Mezzo = Mid(Mezzo, 1, InStr(Mezzo, "(") - 1)
                Altro = " And Dettaglio='" & Dettaglio.Trim & "'"
            End If
            Sql = "Select idMezzo From " & prefissotabelle & "Mezzi Where idUtente=" & idUtente & " And descMezzo='" & Mezzo.Trim.Replace("'", "''") & "' " & Altro
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                idMezzo = 1
            Else
                idMezzo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Select Max(Progressivo)+1 From " & prefissotabelle & "MezziStandard" & Suffisso & " " & _
                "Where idUtente=" & idUtente & " "
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo = 1
            Else
                Progressivo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Insert Into " & prefissotabelle & "MezziStandard" & Suffisso & " Values(" & _
                " " & idUtente & ", " & _
                " " & Progressivo & ", " & _
                " " & idMezzo & " " & _
                ")"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            For i As Integer = 0 To cmbStandard.Items.Count - 1
                If cmbStandard.Items(i).Text = cmbStandard.Text Then
                    cmbStandard.Items.RemoveAt(i)
                    Exit For
                End If
            Next

            CaricaMezziStandard()
        End If
    End Sub

    Protected Sub SpostaGiuMezzoStandard(ByVal sender As Object, ByVal e As System.EventArgs)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
            Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
            Dim Riga As Integer = Row.RowIndex
            Dim Di As GridViewRow = grdStandard.Rows(Riga)
            Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
            Dim Progressivo1 As Integer = hdnIdRiga.Value

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Progressivo2 As Integer = hdnIdRiga.Value

            Dim Tipo As String = Request.QueryString("Tipo")
            Dim Suffisso As String = ""

            Select Case Tipo
                Case "Andata"
                    Suffisso = ""
                Case "Ritorno"
                    Suffisso = "Rit"
            End Select

            Sql = "Select Top 1 Progressivo From " & prefissotabelle & "MezziStandard" & Suffisso & " Where idUtente=" & idUtente & " And Progressivo>" & Progressivo1 & " Order By Progressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo2 = -1
            Else
                Progressivo2 = Rec(0).Value
            End If
            Rec.Close()

            If Progressivo2 <> -1 Then
                Sql = "Update " & prefissotabelle & "MezziStandard" & Suffisso & " Set Progressivo=9999 Where idUtente=" & idUtente & " And Progressivo=" & Progressivo1
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "MezziStandard" & Suffisso & " Set Progressivo=" & Progressivo1 & " Where idUtente=" & idUtente & " And Progressivo=" & Progressivo2
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "MezziStandard" & Suffisso & " Set Progressivo=" & Progressivo2 & " Where idUtente=" & idUtente & " And Progressivo=9999"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                CaricaMezziStandard()
            End If
        End If
    End Sub

    Protected Sub SpostaSuMezzoStandard(ByVal sender As Object, ByVal e As System.EventArgs)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
            Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
            Dim Riga As Integer = Row.RowIndex
            Dim Di As GridViewRow = grdStandard.Rows(Riga)
            Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
            Dim Progressivo1 As Integer = hdnIdRiga.Value

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Progressivo2 As Integer = hdnIdRiga.Value

            Dim Tipo As String = Request.QueryString("Tipo")
            Dim Suffisso As String = ""

            Select Case Tipo
                Case "Andata"
                    Suffisso = ""
                Case "Ritorno"
                    Suffisso = "Rit"
            End Select

            Sql = "Select Top 1 Progressivo From " & prefissotabelle & "MezziStandard" & Suffisso & " Where idUtente=" & idUtente & " And Progressivo<" & Progressivo1 & " Order By Progressivo Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo2 = -1
            Else
                Progressivo2 = Rec(0).Value
            End If
            Rec.Close()

            If Progressivo2 <> -1 Then
                Sql = "Update " & prefissotabelle & "MezziStandard" & Suffisso & " Set Progressivo=9999 Where idUtente=" & idUtente & " And Progressivo=" & Progressivo1
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "MezziStandard" & Suffisso & " Set Progressivo=" & Progressivo1 & " Where idUtente=" & idUtente & " And Progressivo=" & Progressivo2
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "MezziStandard" & Suffisso & " Set Progressivo=" & Progressivo2 & " Where idUtente=" & idUtente & " And Progressivo=9999"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                CaricaMezziStandard()
            End If
        End If
    End Sub

    Protected Sub StatisticheMezzo(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdMezzi.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Mezzo As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        CosaSelezionato = Mezzo
        idSelezionato = hdnIdRiga.Value

        CaricaStatistiche()

        divStatistiche.Visible = True
    End Sub

    Private Sub CaricaStatistiche()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            lblStatMezzo.Text = "Statistiche su mezzo: " & CosaSelezionato

            Dim Tipo As String = Request.QueryString("Tipo")
            Dim Suffisso As String = ""

            Select Case Tipo
                Case "Andata"
                    Suffisso = ""
                Case "Ritorno"
                    Suffisso = "Rit"
            End Select

            Dim dMezzo As New DataColumn("Mezzo")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()
            Dim Datella As Date
            Dim sDatella As String

            dttTabella = New DataTable
            dttTabella.Columns.Add(dMezzo)

            Sql = "Select * From " & prefissotabelle & "AltreInfomezzi" & Suffisso & " " & _
                            "Where idUtente=" & idUtente & " And " & _
                            "idMezzo = " & idSelezionato & " " & _
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