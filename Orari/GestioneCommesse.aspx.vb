Imports System.Drawing

Public Class GestioneCommesse
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            cmbCommessa.Text = ""
            txtCommessa.Text = ""
            txtCodCommessa.Text = ""
            'txtDistanza.Text = ""
            txtIndirizzo.Text = ""
            txtLatLng.Text = ""
            txtKm.Text = ""
            hdnDescFoto.Value = ""
            txtDescF.Text = ""
            txtApplicazione.Text = ""
            hdnApplicazione.Value = ""

            datiConoscenze.Visible = False
            divFoto.Visible = False
            divVisuaFoto.Visible = False
            divVisuaFotoBack.Visible = False
            divProgetti.Visible = False

            imgCommessa.ImageUrl = ""
            imgSocieta.ImageUrl = ""

            'Dim Pannello As ContentPlaceHolder = DirectCast(Master.FindControl("cphNoAjax"), ContentPlaceHolder)
            'Dim imgC As ImageButton = DirectCast(Pannello.FindControl("imgCambia"), ImageButton)

            'imgC.ImageUrl = "App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/visualizzato_tondo.png"

            hdnNomeOriginalCommessa.Value = ""

            lblDefault.Visible = False
            chkDefault.Visible = False

            CaricaCombo()

            divUpload.Visible = False
        End If
    End Sub

    Private Sub CaricaCombo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim SocDef As String = ""

            Sql = "Select * From " & prefissotabelle & "LavoroDefault Where idUtente=" & idUtente & " "
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                idLavoroDefault = 2
            Else
                idLavoroDefault = Rec("idLavoro").Value
            End If
            Rec.Close()

            cmbLinguaggi.Items.Clear()

            Sql = "Select * From " & prefissotabelle & "Linguaggi Where Eliminato='N' Order By Linguaggio"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbLinguaggi.Items.Add("")
            Do Until Rec.Eof
                cmbLinguaggi.Items.Add(Rec("Linguaggio").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            cmbLinguaggi.Text = ""

            cmbSocieta.Items.Clear()

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Eliminato='N' Order By Lavoro"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbSocieta.Items.Add("")
            Do Until Rec.Eof
                cmbSocieta.Items.Add(Rec("Lavoro").Value)
                If idLavoroDefault = Rec("idLavoro").Value Then
                    SocDef = Rec("Lavoro").Value
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            cmbSocieta.Text = SocDef

            cmbIndirizzo.Items.Clear()

            Sql = "Select * From " & prefissotabelle & "Indirizzi Where idUtente=" & idUtente & " Order By Indirizzo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbIndirizzo.Items.Add("")
            Do Until Rec.Eof
                cmbIndirizzo.Items.Add(Rec("Indirizzo").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            SelSocieta()
        End If
    End Sub

    Private Sub SelSocieta()
        If cmbSocieta.Text = "" Then
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            cmbCommessa.Text = ""
            txtCommessa.Text = ""
            txtCodCommessa.Text = ""
            'txtDistanza.Text = ""
            txtIndirizzo.Text = ""
            txtLatLng.Text = ""
            txtKm.Text = ""
            txtDataInizio.Text = ""
            txtDataFine.Text = ""
            cmbIndirizzo.Text = ""
            imgCommessa.ImageUrl = ""
            hdnNomeOriginalCommessa.Value = ""
            hdnDescFoto.Value = ""
            txtDescF.Text = ""
            txtApplicazione.Text = ""
            cmbLinguaggi.Text = ""
            hdnApplicazione.Value = ""

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idLavoro As Integer = -1

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                CaricaImmagineSocieta()
                idLavoro = Rec("idLavoro").Value
            End If
            Rec.Close()

            If idLavoro <> -1 Then
                Sql = "Select * From " & prefissotabelle & "Commesse Where idUtente=" & idUtente & " And idLavoro=" & idLavoro & " And Eliminato='N' Order By Descrizione"
                cmbCommessa.Items.Clear()
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                cmbCommessa.Items.Add("")
                Do Until Rec.Eof
                    cmbCommessa.Items.Add(Rec("Descrizione").Value)

                    Rec.MoveNext()
                Loop
                Rec.Close()
            Else
            End If

            lblDefault.Visible = False
            chkDefault.Visible = False

            hdnIdCommessa.Value = ""
            hdnIdLavoro.Value = ""
            hdnIdConoscenza.Value = ""

            datiConoscenze.Visible = False
            divFoto.Visible = False
            divProgetti.Visible = False

            ConnSQL.Close()
        End If
    End Sub

    Protected Sub SelezionaSocieta()
        SelSocieta()
    End Sub

    Protected Sub ImpostaCommessaDefault()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idCommessa As Integer

            Sql = "Select * From " & prefissotabelle & "Commesse Where idUtente=" & idUtente & " And Descrizione='" & cmbCommessa.Text.Replace("'", "''") & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                idCommessa = Rec("Codice").Value
            End If
            Rec.Close()

            CommessaDefault = idCommessa

            Sql = "Select * From " & prefissotabelle & "CommessaDefault Where idutente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                Sql = "Insert Into " & prefissotabelle & "CommessaDefault Values (" & _
                    " " & idUtente & ", " & _
                    " " & idCommessa & " " & _
                    ")"
            Else
                Sql = "Update " & prefissotabelle & "CommessaDefault Set CodCommessa=" & idCommessa & " Where idUtente=" & idUtente
            End If
            Rec.Close()

            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            VisualizzaMessaggioInPopup("Commessa impostata come default", Master)
        End If
    End Sub

    Protected Sub SelezionaCommessa()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idCommessaDef As Integer = -1
            Dim idLavoro As Integer = -1

            Sql = "Select * From " & prefissotabelle & "Commessadefault " & _
                "Where idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                idCommessaDef = Rec("CodCommessa").Value
            End If
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                CaricaImmagineSocieta()
                idLavoro = Rec("idLavoro").Value
            End If
            Rec.Close()

            Sql = "Select A.*, B.Indirizzo As Ind2 From " & PrefissoTabelle & "Commesse A Left Join " & PrefissoTabelle & "Indirizzi B " &
                "On A.idIndirizzo=B.Contatore " &
                "Where A.idUtente=" & idUtente & " And Descrizione='" & cmbCommessa.Text.Replace("'", "''") & "' " &
                "And idLavoro=" & idLavoro & " And A.Eliminato='N'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                txtCommessa.Text = Rec("Descrizione").value
                txtCodCommessa.Text = Rec("Commessa").value
                'txtDistanza.Text = 0
                hdnNomeOriginalCommessa.Value = txtCommessa.Text
                cmbIndirizzo.Text = "" & Rec("Ind2").Value

                If idCommessaDef = Rec("Codice").Value Then
                    chkDefault.Checked = True
                Else
                    chkDefault.Checked = False
                End If

                If idLavoro = idLavoroDefault Then
                    lblDefault.Visible = True
                    chkDefault.Visible = True
                Else
                    lblDefault.Visible = False
                    chkDefault.Visible = False
                End If

                txtIndirizzo.Text = "" & Rec("Indirizzo").Value
                txtLatLng.Text = "" & Rec("LatLng").Value
                txtDataInizio.Text = "" & Rec("DataInizio").Value
                txtDataFine.Text = "" & Rec("DataFine").Value
                txtKm.Text = "" & Rec("Km").Value & " Km."

                hdnIdCommessa.Value = Rec("Codice").Value
                hdnIdLavoro.Value = idLavoro
                hdnIdConoscenza.Value = ""
                hdnApplicazione.Value = ""

                datiConoscenze.Visible = True
                divVisuaFoto.Visible = True
                divVisuaFotoBack.Visible = True
                divProgetti.Visible = True

                CaricaImmagineCommessa()
                CalcolaDistanze()

                CaricaConoscenze()
                CaricaFoto()
                caricalinguaggi()
            Else
                cmbCommessa.Text = ""
                cmbIndirizzo.Text = ""
                txtCommessa.Text = ""
                txtCodCommessa.Text = ""
                'txtDistanza.Text = ""
                txtIndirizzo.Text = ""
                txtLatLng.Text = ""
                txtDataInizio.Text = ""
                txtDataFine.Text = ""
                txtKm.Text = ""
                hdnDescFoto.Value = ""
                txtDescF.Text = ""
                txtApplicazione.Text = ""
                cmbLinguaggi.Text = ""
                hdnApplicazione.Value = ""

                imgCommessa.ImageUrl = ""
                hdnNomeOriginalCommessa.Value = ""

                divVisuaFoto.Visible = False
                divVisuaFotoBack.Visible = False

                hdnIdCommessa.Value = ""
                hdnIdLavoro.Value = ""
                hdnIdConoscenza.Value = ""

                lblDefault.Visible = False
                chkDefault.Visible = False
                divFoto.Visible = False
                divProgetti.Visible = False
                datiConoscenze.Visible = False

            End If
            Rec.Close()

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

    Private Sub CaricaImmagineCommessa()
        If cmbCommessa.Text = "" Then
            imgCommessa.Visible = False
        Else
            Dim PercImmagineCommessa As String = "App_Themes/Standard/Images/" & idUtente.ToString.Trim & "-" & Utenza & "/Loghi/" & cmbCommessa.Text & ".png"
            Dim PercImmagineFisico As String = Server.MapPath(".") & "/" & PercImmagineCommessa.Replace("/", "\")

            'Dim Pannello As ContentPlaceHolder = DirectCast(Master.FindControl("cphNoAjax"), ContentPlaceHolder)
            'Dim imgC As ImageButton = DirectCast(Pannello.FindControl("imgCambia"), ImageButton)

            If Dir(PercImmagineFisico) <> "" Then
                imgCommessa.ImageUrl = PercImmagineCommessa
                'imgC.ImageUrl = PercImmagineCommessa
                imgCommessa.Visible = True
            Else
                imgCommessa.ImageUrl = ""
                'imgC.ImageUrl = "App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/visualizzato_tondo.png"
                imgCommessa.Visible = False
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
        If cmbSocieta.Text = "" Then
            VisualizzaMessaggioInPopup("Selezionare il nome della società", Master)
            Exit Sub
        End If
        If txtCommessa.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire il nome della commessa", Master)
            Exit Sub
        End If
        'If txtCodCommessa.Text = "" Then
        '    VisualizzaMessaggioInPopup("Inserire il codice della commessa", Master)
        '    Exit Sub
        'End If
        'If txtDistanza.Text = "" Then
        '    VisualizzaMessaggioInPopup("Inserire i km. della commessa", Master)
        '    Exit Sub
        'Else
        '    If IsNumeric(txtDistanza.Text.Replace(",", ".")) = False Then
        '        VisualizzaMessaggioInPopup("Km. della commessa non validi", Master)
        '        Exit Sub
        '    End If
        'End If
        If txtDataInizio.Text <> "" Then
            If IsDate(txtDataInizio.Text) = False Then
                VisualizzaMessaggioInPopup("Data inizio non valida", Master)
                Exit Sub
            End If
        End If
        If txtDataFine.Text <> "" Then
            If IsDate(txtDataFine.Text) = False Then
                VisualizzaMessaggioInPopup("Data fine non valida", Master)
                Exit Sub
            End If
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idCommessa As Integer = -1
            Dim idIndirizzo As String = ""
            Dim Km As String = txtKm.Text
            If InStr(Km, " ") > 0 Then
                Km = Mid(Km, 1, InStr(Km, " ") - 1).Trim
            End If
            Km = Km.Replace(",", ".").Trim

            If cmbIndirizzo.Text <> "" Then
                Sql = "Select * From " & prefissotabelle & "Indirizzi Where idUtente=" & idUtente & " And Indirizzo='" & cmbIndirizzo.Text.Replace("'", "''") & "'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    idIndirizzo = "" & Rec("Contatore").Value.ToString
                End If
                Rec.Close()
            End If

            If idIndirizzo = "" Then idIndirizzo = "Null"

            Dim idSocieta As Integer = -1

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                idSocieta = "" & Rec("idLavoro").Value
            End If
            Rec.Close()

            If cmbCommessa.Text <> "" Then
                Sql = "Select * From " & PrefissoTabelle & "Commesse Where " &
                    "idUtente=" & idUtente & " And " &
                    "idLavoro=" & idSocieta & " And " &
                    "Descrizione='" & cmbCommessa.Text.Replace("'", "''") & "' And " &
                    "Eliminato='N'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    idCommessa = Rec("Codice").Value
                End If
                Rec.Close()

                If idCommessa = -1 Or idSocieta = -1 Then
                    If idCommessa = "" Then
                        VisualizzaMessaggioInPopup("Commessa nulla, Dati non salvati", Master)
                    Else
                        VisualizzaMessaggioInPopup("Società nulla, Dati non salvati", Master)
                    End If
                    ConnSQL.Close()
                    Exit Sub
                End If

                Sql = "Update " & prefissotabelle & "Commesse Set " & _
                     "Descrizione='" & MetteMaiuscole(txtCommessa.Text.Replace("'", "''")) & "', " & _
                     "Commessa='" & MetteMaiuscole(txtCodCommessa.Text.Replace("'", "''")) & "', " & _
                     "Distanza=0, " & _
                     "Indirizzo='" & MetteMaiuscole(txtIndirizzo.Text.Replace("'", "''")) & "', " & _
                     "LatLng='" & txtLatLng.Text & "', " & _
                     "DataInizio='" & txtDataInizio.Text.Replace(",", ".") & "', " & _
                     "DataFine='" & txtDataFine.Text.Replace(",", ".") & "', " & _
                     "idIndirizzo=" & idIndirizzo & ", " & _
                     "Km=" & Km & " " & _
                     "Where idUtente=" & idUtente & " And Codice=" & idCommessa & " And idLavoro=" & idSocieta
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                If txtCommessa.Text <> hdnNomeOriginalCommessa.Value And hdnNomeOriginalCommessa.Value <> "" Then
                    Dim PercImmagineCommessaNuovo As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & txtCommessa.Text & ".png"
                    Dim PercImmagineCommessaVecchio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & hdnNomeOriginalCommessa.Value & ".png"

                    Try
                        Kill(PercImmagineCommessaNuovo)
                    Catch ex As Exception

                    End Try

                    Rename(PercImmagineCommessaVecchio, PercImmagineCommessaNuovo)
                End If
            Else
                Sql = "Select Max(Codice)+1 From " & prefissotabelle & "Commesse Where idUtente=" & idUtente
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).Value Is DBNull.Value = True Then
                    idCommessa = 1
                Else
                    idCommessa = Rec(0).Value
                End If
                Rec.Close()

                Sql = "Insert into " & prefissotabelle & "Commesse Values (" & _
                    " " & idUtente & ", " & _
                    " " & idCommessa & ", " & _
                    "'" & MetteMaiuscole(txtCommessa.Text.Replace("'", "''")) & "', " & _
                    "'" & MetteMaiuscole(txtCodCommessa.Text.Replace("'", "''")) & "', " & _
                    "'" & MetteMaiuscole(txtCodCommessa.Text.Replace("'", "''")) & "', " & _
                    "Null, " & _
                    "Null, " & _
                    "'Euro', " & _
                    " 0, " & _
                    " " & idSocieta & ", " & _
                    "'N', " & _
                    " '" & MetteMaiuscole(txtIndirizzo.Text.Replace("'", "''")) & "', " & _
                    " '" & txtLatLng.Text & "', " & _
                    " '" & txtDataInizio.Text & "', " & _
                    " '" & txtDataFine.Text & "', " & _
                    " " & idIndirizzo & ", " & _
                    " " & Km & " " & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            ' Aggiorna tutti gli orari che hanno codice commessa e idlavoro con l'indirizzo appena immesso
            Sql = "Update " & prefissotabelle & "Orari Set idIndirizzo=" & idIndirizzo & " Where " & _
                "idUtente=" & idUtente & " And " & _
                "CodCommessa=" & idCommessa & " And " & _
                "idLavoro=" & idSocieta & " And " & _
                "idIndirizzo Is Null"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            ' Aggiorna tutti gli orari che hanno codice commessa e idlavoro con l'indirizzo appena immesso

            ' Aggiorna tutti gli orari che hanno destinazione, commessa e origine uguale
            Sql = "Update " & prefissotabelle & "Orari Set Km=" & Km & " Where " & _
                "idUtente=" & idUtente & " And " & _
                "CodCommessa=" & idCommessa & " And " & _
                "idLavoro=" & idSocieta & " And " & _
                "idIndirizzo=" & idIndirizzo
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            ' Aggiorna tutti gli orari che hanno destinazione, commessa e origine uguale

            cmbCommessa.Text = ""
            cmbIndirizzo.Text = ""
            txtCommessa.Text = ""
            txtCodCommessa.Text = ""
            'txtDistanza.Text = ""
            txtIndirizzo.Text = ""
            txtLatLng.Text = ""
            txtDataInizio.Text = ""
            txtDataFine.Text = ""
            txtKm.Text = ""
            txtApplicazione.Text = ""
            cmbLinguaggi.Text = ""
            hdnApplicazione.Value = ""

            hdnDescFoto.Value = ""
            txtDescF.Text = ""
            hdnIdCommessa.Value = ""
            hdnIdLavoro.Value = ""
            hdnIdConoscenza.Value = ""

            datiConoscenze.Visible = False
            divFoto.Visible = False
            divProgetti.Visible = False

            divVisuaFoto.Visible = False
            divVisuaFotoBack.Visible = False

            imgCommessa.ImageUrl = ""
            imgSocieta.ImageUrl = ""

            hdnNomeOriginalCommessa.Value = ""

            ConnSQL.Close()

            CaricaCombo()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Protected Sub imgNuova_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuova.Click
        cmbCommessa.Text = ""
        txtCommessa.Text = ""
        txtCodCommessa.Text = ""
        'txtDistanza.Text = ""
        txtIndirizzo.Text = ""
        txtLatLng.Text = ""
        txtDataInizio.Text = ""
        txtDataFine.Text = ""
        txtKm.Text = ""
        cmbIndirizzo.Text = ""
        hdnDescFoto.Value = ""
        txtDescF.Text = ""
        txtApplicazione.Text = ""
        cmbLinguaggi.Text = ""
        hdnApplicazione.Value = ""

        hdnIdCommessa.Value = ""
        hdnIdLavoro.Value = ""
        hdnIdConoscenza.Value = ""

        datiConoscenze.Visible = False
        divFoto.Visible = False
        divVisuaFoto.Visible = False
        divVisuaFotoBack.Visible = False
        divProgetti.Visible = False

        imgCommessa.ImageUrl = ""
        imgSocieta.ImageUrl = ""

        lblDefault.Visible = False
        chkDefault.Visible = False
    End Sub

    Protected Sub imgElimina_Click(sender As Object, e As ImageClickEventArgs) Handles imgElimina.Click
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idCommessa As Integer = -1

            If cmbCommessa.Text <> "" Then
                Sql = "Select * From " & prefissotabelle & "Commesse " & _
                    "Where idUtente=" & idUtente & " And Descrizione='" & cmbCommessa.Text.Replace("'", "''") & "'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    idCommessa = Rec("Codice").Value
                End If
                Rec.Close()

                Sql = "Update " & prefissotabelle & "Commesse Set " & _
                     "Eliminato='S' " & _
                     "Where idUtente=" & idUtente & " And Codice=" & idCommessa
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                cmbCommessa.Text = ""
                txtCommessa.Text = ""
                txtCodCommessa.Text = ""
                'txtDistanza.Text = ""
                txtIndirizzo.Text = ""
                txtLatLng.Text = ""
                txtDataInizio.Text = ""
                txtDataFine.Text = ""
                txtKm.Text = ""
                cmbIndirizzo.Text = ""
                hdnDescFoto.Value = ""
                txtDescF.Text = ""
                txtApplicazione.Text = ""
                cmbLinguaggi.Text = ""

                hdnIdCommessa.Value = ""
                hdnIdLavoro.Value = ""
                hdnIdConoscenza.Value = ""
                hdnApplicazione.Value = ""

                datiConoscenze.Visible = False
                divFoto.Visible = False
                divVisuaFoto.Visible = False
                divVisuaFotoBack.Visible = False
                divProgetti.Visible = False

                imgCommessa.ImageUrl = ""
                imgSocieta.ImageUrl = ""

                lblDefault.Visible = False
                chkDefault.Visible = False

                datiConoscenze.Visible = False
                divFoto.Visible = False
                divVisuaFoto.Visible = False
                divVisuaFotoBack.Visible = False

                CaricaCombo()

                VisualizzaMessaggioInPopup("Commessa eliminata", Master)
            End If
        End If
    End Sub

    Protected Sub imgCambia_Click(sender As Object, e As ImageClickEventArgs) Handles imgCambia.Click
        If cmbCommessa.Text = "" Then
            VisualizzaMessaggioInPopup("Selezionare una commessa", Master)
            Exit Sub
        End If

        divUpload.Visible = True
        lblTitoloUpload.Text = "Upload immagine commessa"
    End Sub

    Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
        divUpload.Visible = False
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If lblTitoloUpload.Text = "Upload immagine" Then
            CaricaFotoNuova()
        Else
            If FileUpload1.HasFile = False Then
                VisualizzaMessaggioInPopup("Selezionare un file immagine", Master)
                Exit Sub
            End If
            Dim PercImmagineCommessa As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & cmbCommessa.Text & ".png"

            Try
                Kill(PercImmagineCommessa)
            Catch ex As Exception

            End Try

            FileUpload1.SaveAs(PercImmagineCommessa)

            ResizeImage(PercImmagineCommessa, 150, 150)

            CaricaImmagineCommessa()

            divUpload.Visible = False
        End If
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
            "And idCommessa= " & hdnIdCommessa.Value & " " & _
            "And Eliminato='N' " & _
            "Order By Nominativo"
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
            "And idCommessa= " & hdnIdCommessa.Value & " " & _
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
                "Where idUtente=" & idUtente & " " & _
                "And idLavoro=" & hdnIdLavoro.Value & " " & _
                "And idCommessa=" & hdnIdCommessa.Value & " " & _
                "And Contatore=" & hdnIdRiga.Value
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
                    "And idCommessa=" & hdnIdCommessa.Value
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
                    " " & hdnIdCommessa.Value & ", " & _
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
                    "Where idUtente=" & idUtente & " " & _
                    "And idLavoro=" & hdnIdLavoro.Value & " " & _
                    "And idCommessa=" & hdnIdCommessa.Value & " " & _
                    "And Contatore=" & hdnIdConoscenza.Value
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

        Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "")
        Dim PercorsoThumbs As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\Thumbs"

        Try
            MkDir(Percorso)
        Catch ex As Exception

        End Try

        Try
            MkDir(PercorsoThumbs)
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

        'txtDescrizione.Text = ""
    End Sub

    Private Sub grdFoto_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdFoto.PageIndexChanging
        grdFoto.PageIndex = e.NewPageIndex
        grdFoto.DataBind()

        CaricaFoto()
    End Sub

    Private Sub grdFoto_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdFoto.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Nome As String = e.Row.Cells(1).Text
            Dim Percorso As String = "App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/ImmaginiCommessa/" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "/Thumbs/" & Nome
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
        Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\" & Foto
        Dim PercorsoThumbs As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\Thumbs\" & Foto

        Try
            Kill(Percorso)
        Catch ex As Exception

        End Try

        Try
            Kill(PercorsoThumbs)
        Catch ex As Exception

        End Try

        CaricaFoto()
    End Sub

    Protected Sub ModificaFoto(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdFoto.Rows(Riga)
        Dim Foto As String = Di.Cells(1).Text

        hdnDescFoto.Value = Foto
        txtDescF.Text = Foto
    End Sub

    Protected Sub VisualizzaFoto(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdFoto.Rows(Riga)
        Dim Foto As String = Di.Cells(1).Text
        Dim Percorso As String = "App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/ImmaginiCommessa/" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "/" & Foto

        Dim Pannello As ContentPlaceHolder = DirectCast(Master.FindControl("cphNoAjax"), ContentPlaceHolder)
        Dim divNuovo As HtmlGenericControl = DirectCast(Pannello.FindControl("Nuovo"), HtmlGenericControl)

        divNuovo.Visible = False

        divVisuaFoto.Visible = True
        divVisuaFotoBack.Visible = True
        lblNomeImmagine.Text = Foto
        imgVisuaFoto.ImageUrl = Percorso
        imgVisuaFoto.DataBind()

        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb.Append("<script type='text/javascript' language='javascript'>")
        sb.Append("     SistemaImmagine();")
        sb.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)
    End Sub

    Protected Sub imgCaricaFoto_Click(sender As Object, e As ImageClickEventArgs) Handles imgCaricaFoto.Click
        If cmbCommessa.Text = "" Then
            VisualizzaMessaggioInPopup("Selezionare una commessa", Master)
            Exit Sub
        End If

        divUpload.Visible = True
        lblTitoloUpload.Text = "Upload immagine"
    End Sub

    Private Sub CaricaFotoNuova()
        If FileUpload1.HasFile = False Then
            VisualizzaMessaggioInPopup("Selezionare un'immagine", Master)
            Exit Sub
        End If
        'If txtDescrizione.Text = "" Then
        '    VisualizzaMessaggioInPopup("Inserire una descrizione", Master)
        '    Exit Sub
        'End If

        Dim NomeFile As String = FileUpload1.FileName
        NomeFile = NomeFile.Replace(" ", "_")
        NomeFile = NomeFile.Replace("<", "_")
        NomeFile = NomeFile.Replace(">", "_")
        NomeFile = NomeFile.Replace("+", "_")
        NomeFile = NomeFile.Replace("\", "_")
        NomeFile = NomeFile.Replace("/", "_")

        'Dim Estensione As String = FileUpload1.FileName

        'For i As Integer = Len(Estensione) To 1 Step -1
        '    If Mid(Estensione, i, 1) = "." Then
        '        Estensione = Mid(Estensione, i, Len(Estensione))
        '        Exit For
        '    End If
        'Next

        Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\" & NomeFile ' & Estensione
        Dim PercorsoThumb As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\Thumbs\" & NomeFile ' & Estensione

        FileUpload1.SaveAs(Percorso)

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

        FileCopy(Percorso, PercorsoThumb)
        ResizeImage(PercorsoThumb, 40, 40)

        CaricaFoto()

        divUpload.Visible = False
    End Sub

    Private Sub CalcolaDistanze()
        Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb2.Append("<script type='text/javascript' language='javascript'>")
        sb2.Append("     calcRoute();")
        sb2.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb2.ToString(), False)
    End Sub

    Protected Sub CDistanze()
        CalcolaDistanze()
    End Sub

    Protected Sub imgCalcolaLatLng_Click(sender As Object, e As ImageClickEventArgs) Handles imgCalcolaLatLng.Click
        CalcolaDistanze()
    End Sub

    Protected Sub imgModificaF_Click(sender As Object, e As ImageClickEventArgs) Handles imgModificaF.Click
        If txtDescF.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire una descrizione", Master)
            Exit Sub
        End If

        Dim PercorsoVecchio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\" & hdnDescFoto.Value
        Dim PercorsoThumbsVecchio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\Thumbs\" & hdnDescFoto.Value

        Dim PercorsoNuovo As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\" & txtDescF.Text
        Dim PercorsoThumbsNuovo As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\ImmaginiCommessa\" & cmbSocieta.Text.Replace(" ", "") & "-" & cmbCommessa.Text.Replace(" ", "") & "\Thumbs\" & txtDescF.Text

        Rename(PercorsoVecchio, PercorsoNuovo)
        Rename(PercorsoThumbsVecchio, PercorsoThumbsNuovo)

        hdnDescFoto.Value = ""
        txtDescF.Text = ""

        CaricaFoto()
    End Sub

    Private Sub CaricaLinguaggi()
        Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String
        Dim dApplicazione As New DataColumn("Applicazione")
        Dim dLinguaggio As New DataColumn("Linguaggio")
        Dim rigaR As DataRow
        Dim dttTabella As New DataTable()

        dttTabella = New DataTable
        dttTabella.Columns.Add(dApplicazione)
        dttTabella.Columns.Add(dLinguaggio)

        Sql = "Select A.Progressivo, A.Descrizione, B.Linguaggio From " & prefissotabelle & "Applicazioni A " & _
            "Left Join " & prefissotabelle & "Linguaggi B On A.idLinguaggio=B.idLinguaggio " & _
            "Where A.idUtente=" & idUtente & " " & _
            "And A.idCommessa= " & hdnIdCommessa.Value & " " & _
            "And A.Eliminato='N' " & _
            "Order By A.Progressivo"
        Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
        QuanteRighePortata = 0
        Erase idRigaPortata
        Do Until Rec.Eof
            QuanteRighePortata += 1
            ReDim Preserve idRigaPortata(QuanteRighePortata)
            idRigaPortata(QuanteRighePortata) = Rec("Progressivo").Value

            rigaR = dttTabella.NewRow()
            rigaR(0) = Rec("Descrizione").Value
            rigaR(1) = Rec("Linguaggio").Value
            dttTabella.Rows.Add(rigaR)

            Rec.MoveNext()
        Loop
        Rec.Close()

        grdApplicazioni.DataSource = dttTabella
        grdApplicazioni.DataBind()
        grdApplicazioni.SelectedIndex = -1

        txtApplicazione.Text = ""
        cmbLinguaggi.Text = ""

        divProgetti.Visible = True
    End Sub

    Private Sub grdApplicazioni_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdApplicazioni.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdApplicazioni.PageIndex * 5)

            hdnIdRiga.Value = idRigaPortata(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub grdApplicazioni_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdApplicazioni.PageIndexChanging
        grdApplicazioni.PageIndex = e.NewPageIndex
        grdApplicazioni.DataBind()

        CaricaLinguaggi()
    End Sub

    Protected Sub ModificaApplicazione(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim hdnAppl As HiddenField = DirectCast(Row.FindControl("hdnIdRiga"), HiddenField)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdApplicazioni.Rows(Riga)
        Dim Applicazione As String = Di.Cells(1).Text
        Dim Linguaggio As String = Di.Cells(2).Text

        hdnApplicazione.Value = hdnAppl.Value
        txtApplicazione.Text = Applicazione
        cmbLinguaggi.Text = Linguaggio
    End Sub

    Protected Sub imgSalvaL_Click(sender As Object, e As ImageClickEventArgs) Handles imgSalvaL.Click
        If txtApplicazione.Text = "" Then
            VisualizzaMessaggioInPopup("Selezionare una descrizione per l'applicazione", Master)
            Exit Sub
        End If
        If cmbLinguaggi.Text = "" Then
            VisualizzaMessaggioInPopup("Selezionare un linguaggio per l'applicazione", Master)
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idLinguaggio As Integer

            Sql = "Select * From " & prefissotabelle & "Linguaggi Where Linguaggio='" & cmbLinguaggi.Text & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            idLinguaggio = Rec("idLinguaggio").Value
            Rec.Close()

            If hdnApplicazione.Value = "" Then
                Dim Progressivo As Integer = 1

                Sql = "Select Max(Progressivo)+1 From " & prefissotabelle & "Applicazioni Where idUtente=" & idUtente & " And idCommessa=" & hdnIdCommessa.Value
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).value Is DBNull.Value = False Then
                    Progressivo = Rec(0).Value
                End If
                Rec.Close()

                Sql = "Insert Into " & prefissotabelle & "Applicazioni Values (" & _
                    " " & idUtente & ", " & _
                    " " & hdnIdCommessa.Value & ", " & _
                    " " & Progressivo & ", " & _
                    " " & idLinguaggio & ", " & _
                    "'" & MetteMaiuscole(txtApplicazione.Text).Replace("'", "''") & "', " & _
                    "'N'" & _
                    ")"
            Else
                Sql = "Update " & prefissotabelle & "Applicazioni Set " & _
                    "Descrizione='" & MetteMaiuscole(txtApplicazione.Text).Replace("'", "''") & "', " & _
                    "idLinguaggio=" & idLinguaggio & " " & _
                    "Where idUtente=" & idUtente & " And " & _
                    "idCommessa=" & hdnIdCommessa.Value & " And " & _
                    "Progressivo=" & hdnApplicazione.Value
            End If

            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            CaricaLinguaggi()
        End If
    End Sub

    Protected Sub imgNuovoL_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuovoL.Click
        hdnApplicazione.Value = ""
        txtApplicazione.Text = ""
        cmbLinguaggi.Text = ""
    End Sub

    Protected Sub EliminaApplicazione(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim hdnAppl As HiddenField = DirectCast(Row.FindControl("hdnIdRiga"), HiddenField)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdApplicazioni.Rows(Riga)

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Update " & prefissotabelle & "Applicazioni Set " & _
                "Eliminato='S' " & _
                "Where idUtente=" & idUtente & " And " & _
                "idCommessa=" & hdnIdCommessa.Value & " And " & _
                "Progressivo=" & hdnAppl.Value
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            CaricaLinguaggi()
        End If
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub
End Class