Imports System.IO
Imports System.Drawing

Public Class GestioneSocieta
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            txtSocieta.Text = ""
            txtIndirizzo.Text = ""
            txtDataInizio.Text = ""
            txtDataFine.Text = ""
            txtLatLng.Text = ""

            datiConoscenze.Visible = False
            divFoto.Visible = False
            divVisuaFoto.Visible = False
            divVisuaFotoBack.Visible = False

            hdnNomeOriginaleSocieta.Value = txtSocieta.Text
            hdnIdLavoro.Value = ""

            CaricaCombo()

            divUpload.Visible = False
        End If
    End Sub

    Private Sub CaricaCombo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            cmbSocieta.Items.Clear()

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Eliminato='N' Order By Lavoro"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbSocieta.Items.Add("")
            Do Until Rec.Eof
                cmbSocieta.Items.Add(Rec("Lavoro").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            Dim idDefault As Integer

            Sql = "Select A.*, B.Lavoro From " & prefissotabelle & "LavoroDefault A Left Join " & prefissotabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro Where A.idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                idDefault = -1
            Else
                idDefault = Rec("idLavoro").Value
                cmbSocieta.Text = Rec("Lavoro").Value
            End If
            Rec.Close()

            If idDefault <> -1 Then
                Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And idLavoro=" & idDefault
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    hdnIdLavoro.Value = Rec("idLavoro").Value

                    txtSocieta.Text = "" & Rec("Lavoro").Value
                    txtIndirizzo.Text = "" & Rec("Indirizzo").Value
                    txtDataInizio.Text = "" & Rec("DataInizio").Value
                    txtDataFine.Text = "" & Rec("DataFine").Value
                    txtLatLng.Text = "" & Rec("LatLng").Value

                    CaricaImmagineSocieta()
                    CalcolaDistanze()

                    chkDefault.Checked = True
                End If
                Rec.Close()

                CaricaConoscenze()
                CaricaFoto()
            End If

            ConnSQL.Close()
        End If
    End Sub

    Protected Sub ImpostaSocietaDefault()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idLavoro As Integer

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                idLavoro = Rec("idLavoro").Value
            End If
            Rec.Close()

            idLavoroDefault = idLavoro

            Sql = "Select * From " & prefissotabelle & "LavoroDefault Where idutente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                Sql = "Insert Into " & prefissotabelle & "LavoroDefault Values (" & _
                    " " & idUtente & ", " & _
                    " " & idLavoro & " " & _
                    ")"
            Else
                Sql = "Update " & prefissotabelle & "LavoroDefault Set idLavoro=" & idLavoro & " Where idUtente=" & idUtente
            End If
            Rec.Close()

            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            VisualizzaMessaggioInPopup("Società impostata come default", Master)
        End If
    End Sub

    Protected Sub SelezionaSocieta()
        If cmbSocieta.Text = "" Then
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idLavoro As Integer

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                txtSocieta.Text = "" & Rec("Lavoro").Value
                hdnNomeOriginaleSocieta.Value = txtSocieta.Text
                txtIndirizzo.Text = "" & Rec("Indirizzo").Value
                txtDataInizio.Text = "" & Rec("DataInizio").Value
                txtDataFine.Text = "" & Rec("DataFine").Value
                txtLatLng.Text = "" & Rec("LatLng").Value

                CaricaImmagineSocieta()
                CalcolaDistanze()

                If Rec("idLavoro").Value = idLavoroDefault Then
                    chkDefault.Checked = True
                Else
                    chkDefault.Checked = False
                End If

                idLavoro = Rec("idLavoro").Value
            End If
            Rec.Close()

            hdnIdLavoro.Value = idLavoro

            CaricaConoscenze()
            CaricaFoto()

            ConnSQL.Close()
        End If
    End Sub

    Private Sub CaricaImmagineSocieta()
        If cmbSocieta.Text = "" Then
            imgSocieta.Visible = False
        Else
            Dim PercImmagineSocieta As String = "App_Themes/Standard/Images/" & idUtente.ToString.Trim & "-" & Utenza & "/Loghi/" & cmbSocieta.Text & ".png"
            Dim PercImmagineFisico As String = Server.MapPath(".") & "/" & PercImmagineSocieta.Replace("/", "\")

            If Dir(PercImmagineFisico) <> "" Then
                imgSocieta.ImageUrl = PercImmagineSocieta
                imgSocieta.Visible = True
            Else
                imgSocieta.ImageUrl = ""
                imgSocieta.Visible = False
            End If
        End If
    End Sub

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        If Nuovo.Visible = True Then
            If Request.QueryString("Maschera") = "GestTabelle" Then
                Response.Redirect("GestioneTabelle.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
            Else
                Response.Redirect("ModificaGiorno.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
            End If
        Else
            Dim Pannello As ContentPlaceHolder = DirectCast(Master.FindControl("cphNoAjax"), ContentPlaceHolder)
            Dim divNuovo As HtmlGenericControl = DirectCast(Pannello.FindControl("Nuovo"), HtmlGenericControl)

            divNuovo.Visible = True

            divVisuaFoto.Visible = False
            divVisuaFotoBack.Visible = False
            imgVisuaFoto.ImageUrl = ""

            Nuovo.Visible = True
            Salva.Visible = True
            Elimina.Visible = True
        End If
    End Sub

    Protected Sub imgSalva_Click(sender As Object, e As ImageClickEventArgs) Handles imgSalva.Click
        Dim DInizio As Date
        Dim DFine As Date
        Dim sInizio As String = ""
        Dim sFine As String = ""

        If txtSocieta.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire il nome della società", Master)
            Exit Sub
        End If
        If txtIndirizzo.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire l'indirizzo della società", Master)
            Exit Sub
        End If
        If txtDataInizio.Text = "" Then
            'VisualizzaMessaggioInPopup("Inserire la data di inizio", Master)
            'Exit Sub
        Else
            If IsDate(txtDataInizio.Text) = False Then
                VisualizzaMessaggioInPopup("Data di inizio non valida", Master)
                Exit Sub
            Else
                DInizio = txtDataInizio.Text
                sInizio = DInizio.Year & "/" & DInizio.Month & "/" & DInizio.Day & " 00:00:00"
            End If
        End If
        If txtDataFine.Text = "" Then
            'VisualizzaMessaggioInPopup("Inserire la data di fine", Master)
            'Exit Sub
        Else
            If IsDate(txtDataInizio.Text) = False Then
                VisualizzaMessaggioInPopup("Data di fine non valida", Master)
                Exit Sub
            Else
                DFine = txtDataFine.Text
                sFine = DFine.Year & "/" & DFine.Month & "/" & DFine.Day & " 00:00:00"
            End If
        End If
        If txtLatLng.Text = "" And txtIndirizzo.Text <> "" Then
            VisualizzaMessaggioInPopup("Inserire latlng tramite l'apposito tasto", Master)
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idLavoro As Integer = -1

            If cmbSocieta.Text <> "" Then
                Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    idLavoro = Rec("idLavoro").Value
                End If
                Rec.Close()

                If idLavoro <> -1 Then
                    Sql = "Update " & prefissotabelle & "Lavori Set " & _
                        "Lavoro='" & MetteMaiuscole(txtSocieta.Text.Replace("'", "''")) & "', " & _
                        "Indirizzo='" & MetteMaiuscole(txtIndirizzo.Text.Replace("'", "''")) & "', " & _
                        "DataInizio='" & sInizio & "', " & _
                        "DataFine='" & sFine & "', " & _
                        "LatLng='" & txtLatLng.Text & "' " & _
                        "Where idUtente=" & idUtente & " And idLavoro=" & idLavoro
                    EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                    If cmbSocieta.Text <> hdnNomeOriginaleSocieta.Value And hdnNomeOriginaleSocieta.Value <> "" Then
                        Dim PercImmagineSocietaNuovo As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & cmbSocieta.Text & ".png"
                        Dim PercImmagineSocietaVecchio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & hdnNomeOriginaleSocieta.Value & ".png"

                        Try
                            Kill(PercImmagineSocietaNuovo)
                        Catch ex As Exception

                        End Try

                        Rename(PercImmagineSocietaVecchio, PercImmagineSocietaNuovo)
                    End If
                End If
            Else
                Sql = "Select Max(idLavoro)+1 From " & prefissotabelle & "Lavori Where idUtente=" & idUtente
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).Value Is DBNull.Value = True Then
                    idLavoro = 1
                Else
                    idLavoro = Rec(0).Value
                End If
                Rec.Close()

                Sql = "Insert Into " & prefissotabelle & "Lavori Values (" & _
                    " " & idUtente & ", " & _
                    " " & idLavoro & ", " & _
                    "'" & MetteMaiuscole(txtSocieta.Text.Replace("'", "''")) & "', " & _
                    "'" & MetteMaiuscole(txtIndirizzo.Text.Replace("'", "''")) & "', " & _
                    "'" & sInizio & "', " & _
                    "'" & sFine & "', " & _
                    "'N'," & _
                    "'" & txtLatLng.Text & "'" & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            cmbSocieta.Text = ""
            txtSocieta.Text = ""
            txtIndirizzo.Text = ""
            txtDataInizio.Text = ""
            txtDataFine.Text = ""
            txtLatLng.Text = ""

            datiConoscenze.Visible = False
            divFoto.Visible = False

            hdnNomeOriginaleSocieta.Value = ""
            hdnIdLavoro.Value = ""

            CaricaCombo()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Protected Sub imgNuova_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuova.Click
        cmbSocieta.Text = ""
        txtSocieta.Text = ""
        txtIndirizzo.Text = ""
        txtDataInizio.Text = ""
        txtDataFine.Text = ""
        txtLatLng.Text = ""
        imgSocieta.ImageUrl = ""
        hdnNomeOriginaleSocieta.Value = ""
        hdnIdLavoro.Value = ""
        divFoto.Visible = False
        datiConoscenze.Visible = False
    End Sub

    Protected Sub imgElimina_Click(sender As Object, e As ImageClickEventArgs) Handles imgElimina.Click
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idLavoro As Integer = -1

            If cmbSocieta.Text <> "" Then
                Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    idLavoro = Rec("idLavoro").Value
                End If
                Rec.Close()

                Sql = "Update " & prefissotabelle & "Lavori Set " & _
                     "Eliminato='S' " & _
                     "Where idUtente=" & idUtente & " And idLavoro=" & idLavoro
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                cmbSocieta.Text = ""
                txtSocieta.Text = ""
                txtIndirizzo.Text = ""
                txtDataInizio.Text = ""
                txtDataFine.Text = ""
                txtLatLng.Text = ""
                hdnIdLavoro.Value = ""

                datiConoscenze.Visible = False
                divFoto.Visible = False

                CaricaCombo()

                VisualizzaMessaggioInPopup("Società eliminata", Master)
            End If
        End If
    End Sub

    Protected Sub imgCambia_Click(sender As Object, e As ImageClickEventArgs) Handles imgCambia.Click
        divUpload.Visible = True
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divUpload.Visible = False
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If FileUpload1.HasFile = False Then
            VisualizzaMessaggioInPopup("Selezionare un file immagine", Master)
            Exit Sub
        End If
        Dim PercImmagineSocieta As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & cmbSocieta.Text & ".png"

        Try
            Kill(PercImmagineSocieta)
        Catch ex As Exception

        End Try

        FileUpload1.SaveAs(PercImmagineSocieta)

        ResizeImage(PercImmagineSocieta, 150, 150)

        CaricaImmagineSocieta()

        divUpload.Visible = False
    End Sub

    Protected Sub imgNuovaC_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuovaC.Click
        txtConoscenza.Text = ""
        txtTelefono.Text = ""
        txtEmail.Text = ""
    End Sub

    Private Sub grdConoscenze_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdConoscenze.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdConoscenze.PageIndex * 5)

            hdnIdRiga.Value = idRigaPortata(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub CaricaConoscenze()
        Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim dConoscenza As New DataColumn("Conoscenza")
        Dim rigaR As DataRow
        Dim dttTabella As New DataTable()

        dttTabella = New DataTable
        dttTabella.Columns.Add(dConoscenza)

        Sql = "Select * From " & prefissotabelle & "Conoscenze " & _
            "Where idUtente=" & idUtente & " " & _
            "And idLavoro=" & hdnIdLavoro.Value & " " & _
            "And idCommessa=-1 " & _
            "And Eliminato='N' Order By Nominativo"
        Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
        QuanteRighePortata = 0
        Erase idRigaPortata
        Do Until Rec.Eof
            QuanteRighePortata += 1
            ReDim Preserve idRigaPortata(QuanteRighePortata)
            idRigaPortata(QuanteRighePortata) = Rec("Contatore").Value

            rigaR = dttTabella.NewRow()
            rigaR(0) = Rec("Nominativo").Value
            dttTabella.Rows.Add(rigaR)

            Rec.MoveNext()
        Loop
        Rec.Close()

        grdConoscenze.DataSource = dttTabella
        grdConoscenze.DataBind()
        grdConoscenze.SelectedIndex = -1

        txtConoscenza.Text = ""
        txtTelefono.Text = ""
        txtEmail.Text = ""
        hdnIdConoscenza.Value = ""

        datiConoscenze.Visible = True
    End Sub

    Private Sub grdConoscenze_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdConoscenze.PageIndexChanging
        grdConoscenze.PageIndex = e.NewPageIndex
        grdConoscenze.DataBind()

        CaricaConoscenze()
    End Sub

    Protected Sub ModificaConoscenza(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdConoscenze.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Conoscenza As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Sql = "Select * From " & prefissotabelle & "Conoscenze Where " & _
            "idUtente=" & idUtente & " " & _
            "And idLavoro=" & hdnIdLavoro.Value & " " & _
            "And idCommessa=-1 " & _
            "And Contatore=" & hdnIdRiga.Value
        Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
        If Rec.Eof = True Then
            txtConoscenza.Text = ""
            txtTelefono.Text = ""
            txtEmail.Text = ""
            hdnIdConoscenza.Value = ""
        Else
            txtConoscenza.Text = "" & Rec("Nominativo").Value
            txtTelefono.Text = "" & Rec("Telefono").Value
            txtEmail.Text = "" & Rec("EMail").Value
            hdnIdConoscenza.Value = Rec("Contatore").Value
        End If
        Rec.Close()

        ConnSQL.Close()
    End Sub

    Protected Sub EliminaConoscenza(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdConoscenze.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Conoscenza As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update " & prefissotabelle & "Conoscenze Set " & _
                "Eliminato='S' " & _
                "Where idUtente=" & idUtente & " And idLavoro=" & hdnIdLavoro.Value & " And idCommessa=-1 And Contatore=" & hdnIdRiga.Value
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            CaricaConoscenze()

            VisualizzaMessaggioInPopup("Conoscenza eliminata", Master)
        End If
    End Sub

    Protected Sub imgSalvaC_Click(sender As Object, e As ImageClickEventArgs) Handles imgSalvaC.Click
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            If hdnIdConoscenza.Value = "" Then
                Dim idConoscenza As Integer

                Sql = "Select Max(Contatore)+1 From " & prefissotabelle & "Conoscenze " & _
                    "Where idUtente=" & idUtente & " " & _
                    "And idLavoro=" & hdnIdLavoro.Value & " " & _
                    "And idCommessa=-1"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).value Is DBNull.Value = False Then
                    idConoscenza = Rec(0).Value
                Else
                    idConoscenza = 1
                End If
                Rec.Close()

                Sql = "Insert Into " & prefissotabelle & "Conoscenze Values (" & _
                    " " & idUtente & ", " & _
                    " " & hdnIdLavoro.Value & ", " & _
                    " -1, " & _
                    " " & idConoscenza & ", " & _
                    "'" & MetteMaiuscole(txtConoscenza.Text.Replace("'", "''")) & "', " & _
                    "'" & txtTelefono.Text.Replace("'", "''") & "', " & _
                    "'" & txtEmail.Text.Replace("'", "''") & "', " & _
                    "'N'" & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            Else
                Sql = "Update " & prefissotabelle & "Conoscenze Set " & _
                    "Nominativo='" & MetteMaiuscole(txtConoscenza.Text.Replace("'", "''")) & "', " & _
                    "Telefono='" & txtTelefono.Text.Replace("'", "''") & "', " & _
                    "EMail='" & txtEmail.Text.Replace("'", "''") & "' " & _
                    "Where idUtente=" & idUtente & " And idLavoro=" & hdnIdLavoro.Value & " And Contatore=" & hdnIdConoscenza.Value
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            CaricaConoscenze()

            'VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Private Sub CaricaFoto()
        divFoto.Visible = True
        divVisuaFoto.Visible = False
        divVisuaFotoBack.Visible = False

        Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiSocieta\" & cmbSocieta.Text.Replace(" ", "")

        Try
            MkDir(Percorso)
        Catch ex As Exception

        End Try

        Dim Ritorno As String = Dir(Percorso & "\*.*")
        Dim dDescrizione As New DataColumn("Descrizione")
        Dim rigaR As DataRow
        Dim dttTabella As New DataTable()

        dttTabella = New DataTable
        dttTabella.Columns.Add(dDescrizione)

        While Ritorno <> ""
            rigaR = dttTabella.NewRow()
            rigaR(0) = Ritorno
            dttTabella.Rows.Add(rigaR)

            Ritorno = Dir()
        End While

        grdFoto.DataSource = dttTabella
        grdFoto.DataBind()
        grdFoto.SelectedIndex = -1

        txtDescrizione.Text = ""
    End Sub

    Private Sub grdFoto_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdFoto.PageIndexChanging
        grdFoto.PageIndex = e.NewPageIndex
        grdFoto.DataBind()

        CaricaFoto()
    End Sub

    Private Sub grdFoto_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdFoto.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Nome As String = e.Row.Cells(1).Text
            Dim Percorso As String = "App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/ImmaginiSocieta/" & cmbSocieta.Text.Replace(" ", "") & "/" & Nome
            Dim Imm As System.Web.UI.WebControls.Image = DirectCast(e.Row.FindControl("imgImmagine"), System.Web.UI.WebControls.Image)

            Imm.ImageUrl = Percorso
            Imm.DataBind()
        End If
    End Sub

    Protected Sub EliminaFoto(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdFoto.Rows(Riga)
        Dim Foto As String = Di.Cells(1).Text
        Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiSocieta\" & cmbSocieta.Text.Replace(" ", "") & "\" & Foto

        Try
            Kill(Percorso)

            CaricaFoto()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub VisualizzaFoto(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdFoto.Rows(Riga)
        Dim Foto As String = Di.Cells(1).Text
        Dim Percorso As String = "App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/ImmaginiSocieta/" & cmbSocieta.Text.Replace(" ", "") & "/" & Foto

        Dim Pannello As ContentPlaceHolder = DirectCast(Master.FindControl("cphNoAjax"), ContentPlaceHolder)
        Dim divNuovo As HtmlGenericControl = DirectCast(Pannello.FindControl("Nuovo"), HtmlGenericControl)

        divNuovo.Visible = False

        divVisuaFoto.Visible = True
        divVisuaFotoBack.Visible = True
        lblNomeImmagine.Text = Foto
        imgVisuaFoto.ImageUrl = Percorso
        imgVisuaFoto.DataBind()

        Nuovo.Visible = False
        Salva.Visible = False
        Elimina.Visible = False

        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb.Append("<script type='text/javascript' language='javascript'>")
        sb.Append("     SistemaImmagine();")
        sb.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)
    End Sub

    Protected Sub imgCaricaFoto_Click(sender As Object, e As ImageClickEventArgs) Handles imgCaricaFoto.Click
        If FileUpload2.HasFile = False Then
            VisualizzaMessaggioInPopup("Selezionare un'immagine", Master)
            Exit Sub
        End If
        If txtDescrizione.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire una descrizione", Master)
            Exit Sub
        End If

        Dim NomeFile As String = txtDescrizione.Text
        NomeFile = NomeFile.Replace(" ", "_")
        NomeFile = NomeFile.Replace("<", "_")
        NomeFile = NomeFile.Replace(">", "_")
        NomeFile = NomeFile.Replace("+", "_")
        NomeFile = NomeFile.Replace("\", "_")
        NomeFile = NomeFile.Replace("/", "_")

        Dim Estensione As String = FileUpload2.FileName

        For i As Integer = Len(Estensione) To 1 Step -1
            If Mid(Estensione, i, 1) = "." Then
                Estensione = Mid(Estensione, i, Len(Estensione))
                Exit For
            End If
        Next

        Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiSocieta\" & cmbSocieta.Text.Replace(" ", "") & "\" & NomeFile & Estensione

        FileUpload2.SaveAs(Percorso)

        Dim bm As Bitmap = New Bitmap(Percorso)
        'ricava dimensioni originali dell'immagine
        Dim originalX As Integer = bm.Width
        Dim originalY As Integer = bm.Height
        bm.Dispose()

        Dim Ancora As Boolean = True

        While Ancora = True
            If originalX < 1024 And originalY < 768 Then
                Ancora = False
            Else
                originalX -= 3
                originalY -= 3
            End If
        End While

        ResizeImage(Percorso, originalX, originalY)

        CaricaFoto()
    End Sub

    Private Sub CalcolaDistanze()
        Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb2.Append("<script type='text/javascript' language='javascript'>")
        sb2.Append("     calcRoute();")
        sb2.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb2.ToString(), False)
    End Sub

    Protected Sub imgCalcolaLatLng_Click(sender As Object, e As ImageClickEventArgs) Handles imgCalcolaLatLng.Click
        CalcolaDistanze()
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub
End Class