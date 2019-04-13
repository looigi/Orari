Public Class GestioneLinguaggi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            CaricaCombo()
            CaricaLinguaggi()
        End If
    End Sub

    Private Sub CaricaCombo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim RecC As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = "Select * From LivelloConoscenze Order By idConoscenza"

            cmbConoscenze.Items.Clear()
            cmbConoscenze.Items.Add("")
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                cmbConoscenze.Items.Add(Rec("Conoscenza").Value)

                Rec.MoveNext
            Loop
            Rec.Close
        End If
    End Sub

    Private Sub CaricaLinguaggi()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim RecC As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Linguaggio As String
            Dim Conoscenza As String

            Dim dLinguaggio As New DataColumn("Linguaggio")
            Dim dConoscenza As New DataColumn("Conoscenza")
            Dim dRicorrenze As New DataColumn("Ricorrenze")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()

            dttTabella = New DataTable
            dttTabella.Columns.Add(dLinguaggio)
            dttTabella.Columns.Add(dConoscenza)
            dttTabella.Columns.Add(dRicorrenze)

            Sql = "Select idLinguaggio, Linguaggio, B.Conoscenza From " &
                "" & PrefissoTabelle & "Linguaggi A " &
                "Left Join LivelloConoscenze B On A.idConoscenza=B.idConoscenza " &
                "Where Eliminato='N' " &
                "Order By Linguaggio"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            QuanteRighePortata = 0
            Erase idRigaPortata
            Do Until Rec.Eof
                Linguaggio = "" & Rec("Linguaggio").Value
                Conoscenza = "" & Rec("Conoscenza").Value

                QuanteRighePortata += 1
                ReDim Preserve idRigaPortata(QuanteRighePortata)
                idRigaPortata(QuanteRighePortata) = Rec("idLinguaggio").Value

                Dim Quanti As Integer

                Sql = "Select Count(*) As Quanti From " & _
                    "" & prefissotabelle & "Applicazioni Where " & _
                    "idUtente=" & idUtente & " And " & _
                    "idLinguaggio=" & Rec("idLinguaggio").Value
                RecC = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If RecC(0).Value Is DBNull.Value Then
                    Quanti = 0
                Else
                    Quanti = RecC(0).value
                End If
                RecC.Close()

                rigaR = dttTabella.NewRow()
                rigaR(0) = Linguaggio
                rigaR(1) = Conoscenza
                rigaR(2) = Quanti
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            lblQuante.Text = "Linguaggi: " & QuanteRighePortata - 1
            hdnNumero.Value = ""
            txtLinguaggio.Text = ""
            cmbConoscenze.Text = ""

            grdLinguaggi.DataSource = dttTabella
            grdLinguaggi.DataBind()
            grdLinguaggi.SelectedIndex = -1
        End If
    End Sub

    Private Sub grdLinguaggi_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdLinguaggi.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdLinguaggi.PageIndex * 10)

            hdnIdRiga.Value = idRigaPortata(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub grdLinguaggi_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdLinguaggi.PageIndexChanging
        grdLinguaggi.PageIndex = e.NewPageIndex
        grdLinguaggi.DataBind()

        CaricaLinguaggi()
    End Sub

    Protected Sub EliminaLinguaggio(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdLinguaggi.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Portata As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update " & prefissotabelle & "Linguaggi Set " & _
                "Eliminato='S' " & _
                "Where idLinguaggio=" & hdnIdRiga.Value
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            CaricaLinguaggi()

            VisualizzaMessaggioInPopup("Linguaggio eliminata", Master)
        End If
    End Sub

    Protected Sub ModificaLinguaggio(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdLinguaggi.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Linguaggio As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        txtLinguaggio.Text = Linguaggio
        hdnNumero.Value = hdnIdRiga.Value
    End Sub

    Protected Sub imgNuovo_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuovo.Click
        txtLinguaggio.Text = ""
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
            Dim idConoscenza As Integer

            If txtLinguaggio.Text = "" Then
                VisualizzaMessaggioInPopup("Inserire il linguaggio", Master)
                Exit Sub
            End If
            If cmbConoscenze.Text = "" Then
                VisualizzaMessaggioInPopup("Selezionare la conoscenza", Master)
                Exit Sub
            Else
                Sql = "Select * From LivelloConoscenze Where Conoscenza='" & cmbConoscenze.Text & "'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                idConoscenza = Rec("idConoscenza").Value
                Rec.Close
            End If

            If hdnNumero.Value = "" Then
                Dim idLinguaggio As Integer

                Sql = "Select Max(idLinguaggio)+1 From " & prefissotabelle & "Linguaggi"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).value Is DBNull.Value = False Then
                    idLinguaggio = Rec(0).Value
                Else
                    idLinguaggio = 1
                End If
                Rec.Close()

                Sql = "Insert Into " & PrefissoTabelle & "Linguaggi Values (" &
                    " " & idLinguaggio & ", " &
                    "'" & MetteMaiuscole(txtLinguaggio.Text.Replace("'", "''")) & "', " &
                    "'N', " &
                    " " & idConoscenza & " " &
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            Else
                Sql = "Update " & PrefissoTabelle & "Linguaggi Set " &
                    "Linguaggio='" & MetteMaiuscole(txtLinguaggio.Text.Replace("'", "''")) & "', " &
                    "idConoscenza=" & idConoscenza & " " &
                    "Where idLinguaggio=" & hdnNumero.Value
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            CaricaLinguaggi()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub
End Class