Public Class ModificaGiorno
    Inherits System.Web.UI.Page

    Dim NomeMese() As String = {"Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno", "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre"}

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            LeggeOreStandard(Server.MapPath("."))

            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            CaricaCombo()

            Dim Datella As Date = Giorno & "/" & Mese & "/" & Anno

            Dim GiornoTestuale As String = MetteMaiuscole(Datella.ToString("dddd"))
            Dim Testo As String = GiornoTestuale & " " & Datella.Day.ToString("00") & " " & NomeMese(Datella.Month - 1) & " " & Datella.Year.ToString("0000")

            lblGiorno.Text = Testo
            txtGradi2.Visible = False

            If LeggeDatiGiornata(Giorno, Mese, Anno) = False Then
                Dim DatellaPerEntrata As Date = Now.AddMinutes(-5)

                txtOre.Text = OreStandard
                txtOrario.Text = DatellaPerEntrata.Hour.ToString("00") & ":" & DatellaPerEntrata.Minute.ToString("00") & ":" & DatellaPerEntrata.Second.ToString("00")
                txtNotelle.Text = ""
                cmbTempo.Text = "Soleggiato"
                DisegnaImmagineCommessa()
                DisegnaImmagineTempo()
                DisegnaImmagineLavoro()
                cmbPasticca.Text = ""
                cmbTempo.Text = ""
                txtGradi.Text = ""
                lblKMetri.Text = ""
                imgTempo.ImageUrl = ""
                imgTempo.Visible = False

                AggiungeMezziAndataStandard()
                AggiungeMezziRitornoStandard()

                lblGiorno.Text = Testo

                optNormale.Checked = True
                optAltro.Checked = False
                optFerie.Checked = False
                optMalattia.Checked = False

                If Giorno = Now.Day And Mese = Now.Month And Anno = Now.Year Then
                    ScaricaDatiMeteo()
                    imgwsmeteo.Visible = True
                Else
                    imgwsmeteo.Visible = False
                End If
            Else
                If Giorno = Now.Day And Mese = Now.Month And Anno = Now.Year Then
                    imgwsmeteo.Visible = True
                Else
                    imgwsmeteo.Visible = False
                End If
            End If

            ScriveRimanenzaOre()
        End If
    End Sub

    Private Sub CalcolaDistanze()
        Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb2.Append("<script type='text/javascript' language='javascript'>")
        sb2.Append("     calcRoute();")
        sb2.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb2.ToString(), False)
    End Sub

    Protected Sub LeggeKM()
        CalcolaDistanze()
    End Sub

    Private Sub ScaricaDatiMeteo()
        'Dim wsTempo As New wsMeteo.GlobalWeatherSoapClient
        'Dim Tempo As String

        'Try
        '    Tempo = wsTempo.GetWeather("Roma", "Italy")

        'Catch ex As Exception
        '    Tempo = ""
        'End Try

        'Dim Appoggio As String = Tempo
        'Dim A1 As Integer

        'txtGradi.Text = ""
        'A1 = InStr(Tempo, "<Temperature>")
        'If A1 > 0 Then
        '    Tempo = Mid(Tempo, A1 + 13, Len(Tempo))
        '    A1 = InStr(Tempo, "<")
        '    If A1 > 0 Then
        '        Tempo = Mid(Tempo, 1, A1 - 1)
        '        A1 = InStr(Tempo, "(")
        '        If A1 > 0 Then
        '            Tempo = Mid(Tempo, A1 + 1, Len(Tempo))
        '            A1 = InStr(Tempo, "C)")
        '            If A1 > 0 Then
        '                Tempo = Mid(Tempo, 1, A1 - 1)
        '                txtGradi.Text = Tempo.Trim
        '            End If
        '        End If
        '    End If
        'End If

        'Tempo = Appoggio
        'A1 = InStr(Tempo, "<SkyConditions>")
        'If A1 > 0 Then
        '    Tempo = Mid(Tempo, A1 + 15, Len(Tempo))
        '    A1 = InStr(Tempo, "<")
        '    If A1 > 0 Then
        '        Tempo = Mid(Tempo, 1, A1 - 1).ToUpper.Trim

        '        Select Case Tempo
        '            Case "MOSTLY CLOUDY", "PARTLY CLOUDY"
        '                cmbTempo.Text = "Leggermente Nuvoloso"
        '                DisegnaImmagineTempo()
        '            Case "OBSCURED"
        '                cmbTempo.Text = "Nuvoloso"
        '                DisegnaImmagineTempo()
        '            Case Else
        '                cmbTempo.Items.Add(Tempo)
        '                cmbTempo.Text = Tempo

        '                'txtGradi2.Text = Tempo
        '                'txtGradi2.Visible = True
        '        End Select
        '    End If
        'End If
    End Sub

    Private Sub AggiungeMezziAndataStandard()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Sql = "Delete " & prefissotabelle & "AltreInfoMezzi Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Select * From " & prefissotabelle & "MezziStandard Where idUtente=" & idUtente & " Order By Progressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                Sql = "Insert Into " & prefissotabelle & "AltreInfoMezzi Values (" & _
                    " " & idUtente & ", " & _
                    " " & Giorno & ", " & _
                    " " & Mese & ", " & _
                    " " & Anno & ", " & _
                    " " & Rec("Progressivo").Value & ", " & _
                    " " & Rec("idMezzo").Value & " " & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            CaricaMezziDiAndata()
        End If
    End Sub

    Private Sub AggiungeMezziRitornoStandard()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Sql = "Delete " & prefissotabelle & "AltreInfoMezziRit Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Select * From " & prefissotabelle & "MezziStandardRit Where idUtente=" & idUtente & " Order By Progressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                Sql = "Insert Into " & prefissotabelle & "AltreInfoMezziRit Values (" & _
                    " " & idUtente & ", " & _
                    " " & Giorno & ", " & _
                    " " & Mese & ", " & _
                    " " & Anno & ", " & _
                    " " & Rec("Progressivo").Value & ", " & _
                    " " & Rec("idMezzo").Value & " " & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            CaricaMezziDiRitorno()
        End If
    End Sub

    Private Sub CaricaCombo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            cmbCommessa.Items.Clear()
            cmbSocieta.Items.Clear()
            cmbTempo.Items.Clear()
            cmbPasticca.Items.Clear()
            cmbAndata.Items.Clear()
            cmbRitorno.Items.Clear()
            cmbPortata.Items.Clear()

            Sql = "Select * From " & prefissotabelle & "LavoroDefault Where idUtente=" & idUtente & " "
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                idLavoroDefault = 2
            Else
                idLavoroDefault = Rec("idLavoro").Value
            End If
            Rec.Close()

            Sql = "Select B.Descrizione From " & prefissotabelle & "CommessaDefault A " & _
                "Left Join " & prefissotabelle & "Commesse B On A.CodCommessa=B.Codice And A.idUtente=B.idUtente " & _
                "Where A.idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                CommessaDefault = ""
            Else
                CommessaDefault = Rec("Descrizione").Value
            End If
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Tempo Where idUtente=" & idUtente & " Order By DescTempo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbTempo.Items.Add("")
            Do Until Rec.Eof
                cmbTempo.Items.Add(Rec("DescTempo").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Pasticche Where idUtente=" & idUtente & " And Eliminato='N' Order By DescPasticca"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbPasticca.Items.Add("")
            Do Until Rec.Eof
                cmbPasticca.Items.Add(Rec("DescPasticca").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Eliminato='N' Order By Lavoro"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                cmbSocieta.Items.Add(Rec("Lavoro").Value.ToString)

                Rec.MoveNext()
            Loop
            Rec.Close()

            Dim Mezzo As String

            Sql = "Select * From " & PrefissoTabelle & "Mezzi Where idUtente=" & idUtente & " And Eliminato='N' Order By  descMezzo, Dettaglio"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbAndata.Items.Add("")
            cmbRitorno.Items.Add("")
            Do Until Rec.Eof
                Mezzo = "" & Rec("DescMezzo").Value
                If Rec("Dettaglio").Value.ToString.Trim <> "" Then Mezzo += " (" & Rec("Dettaglio").Value & ")"

                cmbAndata.Items.Add(Mezzo)
                cmbRitorno.Items.Add(Mezzo)

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Portate Where idUtente=" & idUtente & " And Eliminato='N' Order By Portata"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbPortata.Items.Add("")
            Do Until Rec.Eof
                cmbPortata.Items.Add(Rec("Portata").Value.ToString)

                Rec.MoveNext()
            Loop
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Indirizzi Where idUtente=" & idUtente & " Order By Indirizzo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbIndirizzo.Items.Add("")
            Do Until Rec.Eof
                cmbIndirizzo.Items.Add(Rec("Indirizzo").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()

            ImpostaCommesseLavoroRoutine()
        End If
    End Sub

    Private Sub ImpostaCommesseLavoroRoutine()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            cmbCommessa.Items.Clear()

            Sql = "Select * From " & prefissotabelle & "Commesse Where " & _
                "idUtente=" & idUtente & " And " & _
                "idLavoro=" & idLavoroDefault & " " & _
                "Order By Descrizione"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                cmbCommessa.Items.Add(Rec("Descrizione").Value.ToString)

                Rec.MoveNext()
            Loop
            Rec.Close()

            cmbCommessa.Text = CommessaDefault
        End If
    End Sub

    Private Function PrendeIdDaCombo(Cosa As String, Tabella As String, CampoDesc As String) As Integer
        Dim id As Integer = -1

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & Tabella & " Where " & _
                "idUtente=" & idUtente & " And " & _
                " " & CampoDesc & "='" & Cosa & "' "
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                id = Rec(1).Value
            End If
            Rec.Close()

            ConnSQL.Close()
        End If

        Return id
    End Function

    Protected Sub ImpostaCommesseLavoro()
        idLavoroDefault = PrendeIdDaCombo(cmbSocieta.Text, "" & prefissotabelle & "Lavori", "Lavoro")

        ImpostaCommesseLavoroRoutine()
        DisegnaImmagineLavoro()
        DisegnaImmagineCommessa()
    End Sub

    Protected Sub ImpostaImmagineTempo()
        DisegnaImmagineTempo()
    End Sub

    Protected Sub ImpostaImmagineCommessa()
        DisegnaImmagineCommessa()
    End Sub

    Private Sub DisegnaImmagineLavoro()
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

    Private Sub DisegnaImmagineTempo()
        If cmbTempo.Text = "" Then
            imgTempo.Visible = False
        Else
            Dim idTempo As Integer = PrendeIdDaCombo(cmbTempo.Text, "" & prefissotabelle & "Tempo", "DescTempo")
            Dim PercImmagineTempo As String = "App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/Tempo/" & idTempo & ".png"
            Dim PercImmagineFisico As String = Server.MapPath(".") & "/" & PercImmagineTempo.Replace("/", "\")

            If Dir(PercImmagineFisico) <> "" Then
                imgTempo.ImageUrl = PercImmagineTempo
                imgTempo.Visible = True
            Else
                imgTempo.ImageUrl = ""
                imgTempo.Visible = False
            End If
        End If
    End Sub

    Private Sub DisegnaImmagineCommessa()
        If cmbCommessa.Text = "" Then
            imgCommessa.Visible = False
            cmbIndirizzo.Text = ""
        Else
            Dim PercImmagineCommessa As String = "App_Themes/Standard/Images/" & idUtente.ToString.Trim & "-" & Utenza & "/Loghi/" & cmbCommessa.Text & ".png"
            Dim PercImmagineFisico As String = Server.MapPath(".") & "/" & PercImmagineCommessa.Replace("/", "\")

            If Dir(PercImmagineFisico) <> "" Then
                imgCommessa.ImageUrl = PercImmagineCommessa
                imgCommessa.Visible = True
            Else
                imgCommessa.ImageUrl = ""
                imgCommessa.Visible = False
            End If

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim idSocieta As Integer

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                idSocieta = Rec("idLavoro").Value
            End If
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Commesse Where " & _
                "idUtente=" & idUtente & " And " & _
                "idLavoro=" & idSocieta & " And " & _
                "Descrizione='" & cmbCommessa.Text & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                Dim idIndirizzo As String = "" & Rec("idIndirizzo").Value.ToString

                hdnIndirizzo.Value = Rec("Indirizzo").Value

                If idIndirizzo <> "" Then
                    Rec.Close()

                    Sql = "Select * From " & prefissotabelle & "Indirizzi " & _
                        "Where idUtente=" & idUtente & " And Contatore=" & idIndirizzo
                    Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                    If Rec.Eof = False Then
                        cmbIndirizzo.Text = Rec("Indirizzo").Value

                        CalcolaDistanze()
                    Else
                        hdnIndirizzo.Value = ""
                        cmbIndirizzo.Text = ""
                    End If
                Else
                    hdnIndirizzo.Value = ""
                    cmbIndirizzo.Text = ""
                End If
            Else
                hdnIndirizzo.Value = ""
                cmbIndirizzo.Text = ""
            End If
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Private Function LeggeDatiGiornata(Giorno As String, Mese As String, Anno As String) As Boolean
        Dim Ritorno As Boolean = True

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Misti As String
            Dim Sql As String
            Dim idIndirizzo As String = ""
            Dim idSocieta As String = ""

            Sql = "Select A.*, B.Descrizione As Commessa, B.KM, C.idLavoro, C.Lavoro, E.DescTempo, D.Gradi, G.DescPasticca From " & prefissotabelle & "Orari A " & _
                "Left Join " & prefissotabelle & "Commesse B On A.CodCommessa = B.Codice And A.idUtente=B.idUtente " & _
                "Left Join " & prefissotabelle & "Lavori C On A.idLavoro = C.idLavoro And A.idUtente=C.idUtente " & _
                "Left Join " & prefissotabelle & "AltreInfoTempo D On A.Giorno=D.Giorno And A.Mese=D.Mese And A.Anno=D.Anno And A.idUtente=D.idUtente " & _
                "Left Join " & prefissotabelle & "Tempo E On D.idTempo=E.idTempo And A.idUtente=E.idUtente " & _
                "Left Join " & prefissotabelle & "AltreInfoPasticca F On A.Giorno=F.Giorno And A.Mese=F.Mese And A.Anno=F.Anno And A.idUtente=F.idUtente " & _
                "Left Join " & prefissotabelle & "Pasticche G On F.idPasticca=G.idPasticca And A.idUtente=G.idUtente " & _
                "Where A.idUtente=" & idUtente & " And A.Giorno=" & Giorno & " And A.Mese=" & Mese & " And A.Anno=" & Anno
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                txtOre.Text = "" & Rec("Quanto").Value.ToString

                txtOrario.Text = "" & Rec("Entrata").Value.ToString
                txtNotelle.Text = "" & Rec("Notelle").Value.ToString
                txtGradi.Text = "" & Rec("Gradi").Value.ToString
                cmbCommessa.Text = "" & Rec("Commessa").Value.ToString
                cmbSocieta.Text = "" & Rec("Lavoro").Value.ToString
                cmbTempo.Text = "" & Rec("DescTempo").Value.ToString
                cmbPasticca.Text = "" & Rec("DescPasticca").Value.ToString
                Misti = "" & Rec("Misti").Value.ToString
                lblKMetri.Text = "" & Rec("KM").Value.ToString & " Km."
                idIndirizzo = "" & Rec("idIndirizzo").Value.ToString
                idSocieta = "" & Rec("idLavoro").Value.ToString

                Select Case txtOre.Text
                    Case "-1"
                        optFerie.Checked = True
                        optMalattia.Checked = False
                        optAltro.Checked = False
                        optNormale.Checked = False
                        optLavoroDaCasa.Checked = False

                        ulDati.Visible = False
                        lipranzo.Visible = False
                        liPranzo2.Visible = False
                        liportata.Visible = False
                        ulPasticca.Visible = False
                        ulMezzi.Visible = False

                        txtOre.Text = "0"
                        Misti = ""

                        cmbPasticca.Text = ""
                        cmbTempo.Text = ""
                    Case "-2"
                        optFerie.Checked = False
                        optMalattia.Checked = True
                        optAltro.Checked = False
                        optNormale.Checked = False
                        optLavoroDaCasa.Checked = False

                        ulDati.Visible = False
                        lipranzo.Visible = False
                        liPranzo2.Visible = False
                        liportata.Visible = False
                        ulPasticca.Visible = False
                        ulMezzi.Visible = False

                        txtOre.Text = "0"
                        'Misti = ""

                        cmbPasticca.Text = ""
                        cmbTempo.Text = ""
                    Case "-3"
                        optFerie.Checked = False
                        optMalattia.Checked = False
                        optAltro.Checked = False
                        optNormale.Checked = True
                        optLavoroDaCasa.Checked = False

                        If Misti.IndexOf("P" & OreStandard.ToString.Trim) > -1 Then
                            ulDati.Visible = False
                            lipranzo.Visible = False
                            liPranzo2.Visible = False
                            liportata.Visible = False
                            ulPasticca.Visible = False
                            ulMezzi.Visible = False
                            optGiornoPermesso.Checked = True
                            optNormale.Checked = False
                            liportata.Visible = False
                        Else
                            ulDati.Visible = True
                            lipranzo.Visible = True
                            liPranzo2.Visible = False
                            liportata.Visible = True
                            ulPasticca.Visible = True
                            ulMezzi.Visible = True
                            optGiornoPermesso.Checked = False
                            liportata.Visible = True
                        End If

                        txtOre.Text = "0"
                        'Misti = ""

                        cmbPasticca.Text = ""
                        cmbTempo.Text = ""
                    Case "-4"
                        optFerie.Checked = False
                        optMalattia.Checked = False
                        optAltro.Checked = True
                        optNormale.Checked = False
                        optLavoroDaCasa.Checked = False

                        ulDati.Visible = False
                        lipranzo.Visible = False
                        liPranzo2.Visible = False
                        liportata.Visible = False
                        ulPasticca.Visible = False
                        ulMezzi.Visible = False

                        txtOre.Text = "0"
                        Misti = ""

                        cmbPasticca.Text = ""
                        cmbTempo.Text = ""
                    Case "-6"
                        optFerie.Checked = False
                        optMalattia.Checked = False
                        optAltro.Checked = False
                        optNormale.Checked = False
                        optLavoroDaCasa.Checked = True

                        ulDati.Visible = False
                        lipranzo.Visible = False
                        liPranzo2.Visible = False
                        liportata.Visible = False
                        ulPasticca.Visible = False
                        ulMezzi.Visible = False

                        cmbPasticca.Text = ""
                        cmbTempo.Text = ""
                    Case Else
                        optFerie.Checked = False
                        optMalattia.Checked = False
                        optAltro.Checked = False
                        optNormale.Checked = True
                        optLavoroDaCasa.Checked = False

                        ulDati.Visible = True
                        lipranzo.Visible = True
                        liPranzo2.Visible = True
                        ulPasticca.Visible = True
                        ulMezzi.Visible = True
                End Select

                If Misti <> "" Then
                    Misti += ";"

                    Dim Campi() As String = Misti.Split(";")

                    txtOre.Text = Mid(Campi(0), 2, Len(Campi(0)))
                    If Left(txtOre.Text, 1) = "." Then txtOre.Text = "0." & txtOre.Text
                    txtPermesso.Text = Mid(Campi(1), 2, Len(Campi(1)))
                    If Left(txtPermesso.Text, 1) = "." Then txtPermesso.Text = "0." & txtPermesso.Text
                    txtMalattia.Text = Mid(Campi(2), 2, Len(Campi(2)))
                    If Left(txtMalattia.Text, 1) = "." Then txtMalattia.Text = "0." & txtMalattia.Text
                Else
                    txtPermesso.Text = "0"
                    txtMalattia.Text = "0"

                    If txtOre.Text.Replace(",", ".") > OreStandard Then
                        txtStraordinari.Text = (Val(txtOre.Text.Replace(",", ".")) - OreStandard).ToString.Replace(",", ".")
                        txtOre.Text = OreStandard
                    End If
                End If

                DisegnaImmagineTempo()
                DisegnaImmagineCommessa()
                DisegnaImmagineLavoro()
                CalcolaDistanze()

                Rec.Close()

                If idIndirizzo <> "" Then
                    Sql = "Select * From " & prefissotabelle & "Indirizzi Where " & _
                        "idUtente=" & idUtente & " And " & _
                        "Contatore=" & idIndirizzo
                    Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                    If Rec.Eof = False Then
                        cmbIndirizzo.Text = Rec("Indirizzo").Value
                    End If
                    Rec.Close()
                End If
            Else
                Rec.Close()

                Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And idLavoro=" & idLavoroDefault
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    cmbSocieta.Text = Rec("Lavoro").Value
                End If
                Rec.Close()

                Ritorno = False
            End If

            ConnSQL.Close()

            ' Mezzi di andata
            CaricaMezziDiAndata()
            ' Mezzi di andata

            ' Mezzi di ritorno
            CaricaMezziDiRitorno()
            ' Mezzi di ritorno

            ' Pranzo
            CaricaPranzo()
            ' Pranzo
        End If

        Return Ritorno
    End Function

    Private Sub CaricaPranzo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Portata As String

            Dim dPortata As New DataColumn("Portata")
            Dim rigaR As DataRow
            Dim dttTabella As New DataTable()

            dttTabella = New DataTable
            dttTabella.Columns.Add(dPortata)

            Sql = "Select A.idProgressivo, B.Portata From " & prefissotabelle & "Pranzi2 A " & _
                "Left Join " & prefissotabelle & "Portate B On A.idUtente=B.idUtente And A.idPortata = B.idPortata " & _
                "Where A.idUtente = " & idUtente & " And A.idGiorno = " & Giorno & " And A.idMese = " & Mese & " And A.idAnno = " & Anno & " " & _
                "Order By idProgressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            QuanteRighePortata = 0
            Do Until Rec.Eof
                Portata = "" & Rec("Portata").Value

                For i As Integer = 0 To cmbPortata.Items.Count - 1
                    If cmbPortata.Items(i).Text = Portata Then
                        cmbPortata.Items.RemoveAt(i)
                        Exit For
                    End If
                Next

                QuanteRighePortata += 1
                ReDim Preserve idRigaPortata(QuanteRighePortata)
                idRigaPortata(QuanteRighePortata) = Rec("idProgressivo").Value

                rigaR = dttTabella.NewRow()
                rigaR(0) = Portata
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdPranzo.DataSource = dttTabella
            grdPranzo.DataBind()
            grdPranzo.SelectedIndex = -1
        End If
    End Sub

    Private Sub CaricaMezziDiRitorno()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Mezzo As String

            Dim dMezzoR As New DataColumn("Mezzo")
            Dim rigaR As DataRow
            Dim dttTabellaR As New DataTable()

            dttTabellaR = New DataTable
            dttTabellaR.Columns.Add(dMezzoR)

            Sql = "Select A.Progressivo, B.descMezzo, B.Dettaglio From " & PrefissoTabelle & "AltreInfoMezziRit A " & _
                "Left Join " & PrefissoTabelle & "Mezzi B On A.idUtente=B.idUtente And A.idMezzo = B.idMezzo " & _
                "Where A.idUtente = " & idUtente & " And A.Giorno = " & Giorno & " And A.Mese = " & Mese & " And A.Anno = " & Anno & " " & _
                "Order By A.Progressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            QuanteRigheRitorno = 0
            Do Until Rec.Eof
                Mezzo = "" & Rec("DescMezzo").Value
                If Rec("Dettaglio").Value.ToString.Trim <> "" Then Mezzo += " (" & Rec("Dettaglio").Value & ")"

                For i As Integer = 0 To cmbRitorno.Items.Count - 1
                    If cmbRitorno.Items(i).Text = Mezzo Then
                        cmbRitorno.Items.RemoveAt(i)
                        Exit For
                    End If
                Next

                QuanteRigheRitorno += 1
                ReDim Preserve idRigaRit(QuanteRigheRitorno)
                idRigaRit(QuanteRigheRitorno) = Rec("Progressivo").Value

                rigaR = dttTabellaR.NewRow()
                rigaR(0) = Mezzo
                dttTabellaR.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdRitorno.DataSource = dttTabellaR
            grdRitorno.DataBind()
            grdRitorno.SelectedIndex = -1
        End If
    End Sub

    Private Sub CaricaMezziDiAndata()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Mezzo As String

            Dim dMezzo As New DataColumn("Mezzo")
            Dim riga As DataRow
            Dim dttTabellaA As New DataTable()

            dttTabellaA = New DataTable
            dttTabellaA.Columns.Add(dMezzo)

            Sql = "Select A.Progressivo, B.descMezzo, B.Dettaglio From " & PrefissoTabelle & "AltreInfoMezzi A " & _
                "Left Join " & PrefissoTabelle & "Mezzi B On A.idUtente=B.idUtente And A.idMezzo = B.idMezzo " & _
                "Where A.idUtente = " & idUtente & " And A.Giorno = " & Giorno & " And A.Mese = " & Mese & " And A.Anno = " & Anno & " " & _
                "Order By A.Progressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            QuanteRigheAndata = 0
            Do Until Rec.Eof
                Mezzo = "" & Rec("DescMezzo").Value
                If Rec("Dettaglio").Value.ToString.Trim <> "" Then Mezzo += " (" & Rec("Dettaglio").Value & ")"

                For i As Integer = 0 To cmbAndata.Items.Count - 1
                    If cmbAndata.Items(i).Text = Mezzo Then
                        cmbAndata.Items.RemoveAt(i)
                        Exit For
                    End If
                Next

                QuanteRigheAndata += 1
                ReDim Preserve idRigaAnd(QuanteRigheAndata)
                idRigaAnd(QuanteRigheAndata) = Rec("Progressivo").Value

                riga = dttTabellaA.NewRow()
                riga(0) = Mezzo
                dttTabellaA.Rows.Add(riga)

                Rec.MoveNext()
            Loop
            Rec.Close()

            grdAndata.DataSource = dttTabellaA
            grdAndata.DataBind()
            grdAndata.SelectedIndex = -1
        End If
    End Sub

    Private Sub grdAndata_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAndata.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdAndata.PageIndex * 3)

            hdnIdRiga.Value = idRigaAnd(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub grdPranzo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdPranzo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdPranzo.PageIndex * 3)

            hdnIdRiga.Value = idRigaPortata(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub grdRitorno_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdRitorno.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim hdnIdRiga As HiddenField = DirectCast(e.Row.FindControl("hdnIdRiga"), HiddenField)
            Dim NumeroRiga As Integer

            NumeroRiga = (e.Row.RowIndex + 1)
            NumeroRiga = NumeroRiga + (grdRitorno.PageIndex * 3)

            hdnIdRiga.Value = idRigaRit(NumeroRiga)
            hdnIdRiga.DataBind()
        End If
    End Sub

    Private Sub grdPranzo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdPranzo.PageIndexChanging
        grdPranzo.PageIndex = e.NewPageIndex
        grdPranzo.DataBind()

        CaricaPranzo()
    End Sub

    Private Sub grdAndata_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdAndata.PageIndexChanging
        grdAndata.PageIndex = e.NewPageIndex
        grdAndata.DataBind()

        CaricaMezziDiAndata()
    End Sub

    Private Sub grdRitorno_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdRitorno.PageIndexChanging
        grdRitorno.PageIndex = e.NewPageIndex
        grdRitorno.DataBind()

        CaricaMezziDiRitorno()
    End Sub

    Protected Sub EliminaMezzoAndata(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdAndata.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Mezzo As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Sql As String

            Sql = "Delete From " & prefissotabelle & "AltreInfoMezzi " & _
                "Where idUtente=" & idUtente & " " & _
                "And Giorno= " & Giorno & " " & _
                "And Mese=" & Mese & " " & _
                "And Anno=" & Anno & " " & _
                "And Progressivo=" & Progressivo
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            CaricaMezziDiAndata()

            cmbAndata.Items.Add(Mezzo)
        End If
    End Sub

    Protected Sub EliminaPortata(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdPranzo.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Portata As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Sql As String

            Sql = "Delete From " & prefissotabelle & "Pranzi2 " & _
                "Where idUtente=" & idUtente & " " & _
                "And idGiorno= " & Giorno & " " & _
                "And idMese=" & Mese & " " & _
                "And idAnno=" & Anno & " " & _
                "And idProgressivo=" & Progressivo
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            CaricaPranzo()

            cmbPortata.Items.Add(Portata)
        End If
    End Sub

    Protected Sub AggiungeMezzoAndata(ByVal sender As Object, ByVal e As System.EventArgs)
        If cmbAndata.Text.Trim = "" Then
            Exit Sub
        End If
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Progressivo As Integer

            Dim Mezzo As String = cmbAndata.Text.Trim
            Dim Dettaglio As String = ""
            Dim Altro As String = ""
            Dim idMezzo As Integer

            If InStr(Mezzo, "(") > 0 Then
                Dettaglio = Mid(Mezzo, InStr(Mezzo, "(") + 1, Len(Mezzo))
                Dettaglio = Mid(Dettaglio, 1, InStr(Dettaglio, ")") - 1)
                Mezzo = Mid(Mezzo, 1, InStr(Mezzo, "(") - 1)
                Altro = " And Dettaglio='" & Dettaglio & "'"
            End If
            Sql = "Select idMezzo From " & prefissotabelle & "Mezzi Where idUtente=" & idUtente & " And descMezzo='" & Mezzo & "' " & Altro
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                idMezzo = 1
            Else
                idMezzo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Select Max(Progressivo)+1 From " & prefissotabelle & "AltreInfoMezzi " & _
                "Where idUtente=" & idUtente & " " & _
                "And Giorno=" & Giorno & " " & _
                "And Mese=" & Mese & " " & _
                "And Anno=" & Anno
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo = 1
            Else
                Progressivo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Insert Into " & prefissotabelle & "AltreInfoMezzi Values(" & _
                " " & idUtente & ", " & _
                " " & Giorno & ", " & _
                " " & Mese & ", " & _
                " " & Anno & ", " & _
                " " & Progressivo & ", " & _
                " " & idMezzo & " " & _
                ")"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            For i As Integer = 0 To cmbAndata.Items.Count - 1
                If cmbAndata.Items(i).Text = cmbAndata.Text Then
                    cmbAndata.Items.RemoveAt(i)
                    Exit For
                End If
            Next

            CaricaMezziDiAndata()
        End If
    End Sub

    Protected Sub EliminaMezzoRitorno(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
        Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
        Dim Riga As Integer = Row.RowIndex
        Dim Di As GridViewRow = grdRitorno.Rows(Riga)
        Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
        Dim Mezzo As String = Di.Cells(1).Text
        Dim Progressivo As Integer = hdnIdRiga.Value

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Sql As String

            Sql = "Delete From " & prefissotabelle & "AltreInfoMezziRit " & _
                "Where idUtente=" & idUtente & " " & _
                "And Giorno= " & Giorno & " " & _
                "And Mese=" & Mese & " " & _
                "And Anno=" & Anno & " " & _
                "And Progressivo=" & Progressivo
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            CaricaMezziDiRitorno()

            cmbRitorno.Items.Add(Mezzo)
        End If
    End Sub

    Protected Sub AggiungePortata(ByVal sender As Object, ByVal e As System.EventArgs)
        If cmbPortata.Text.Trim = "" Then
            Exit Sub
        End If
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Progressivo As Integer

            Dim Portata As String = cmbPortata.Text.Trim
            Dim idPortata As Integer

            Sql = "Select idPortata From " & prefissotabelle & "Portate Where idUtente=" & idUtente & " And Portata='" & Portata & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                idPortata = 1
            Else
                idPortata = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Select Max(idProgressivo)+1 From " & prefissotabelle & "Pranzi2 " & _
                "Where idUtente=" & idUtente & " " & _
                "And idGiorno=" & Giorno & " " & _
                "And idMese=" & Mese & " " & _
                "And idAnno=" & Anno
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo = 1
            Else
                Progressivo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Insert Into " & prefissotabelle & "Pranzi2 Values(" & _
                " " & idUtente & ", " & _
                " " & Giorno & ", " & _
                " " & Mese & ", " & _
                " " & Anno & ", " & _
                " " & Progressivo & ", " & _
                " " & idPortata & " " & _
                ")"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            For i As Integer = 0 To cmbPortata.Items.Count - 1
                If cmbPortata.Items(i).Text = cmbPortata.Text Then
                    cmbPortata.Items.RemoveAt(i)
                    Exit For
                End If
            Next

            CaricaPranzo()
        End If
    End Sub
    Protected Sub AggiungeMezzoRitorno(ByVal sender As Object, ByVal e As System.EventArgs)
        If cmbRitorno.Text.Trim = "" Then
            Exit Sub
        End If
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Progressivo As Integer

            Dim Mezzo As String = cmbRitorno.Text.Trim
            Dim Dettaglio As String = ""
            Dim Altro As String = ""
            Dim idMezzo As Integer

            If InStr(Mezzo, "(") > 0 Then
                Dettaglio = Mid(Mezzo, InStr(Mezzo, "(") + 1, Len(Mezzo))
                Dettaglio = Mid(Dettaglio, 1, InStr(Dettaglio, ")") - 1)
                Mezzo = Mid(Mezzo, 1, InStr(Mezzo, "(") - 1)
                Altro = " And Dettaglio='" & Dettaglio & "'"
            End If
            Sql = "Select idMezzo From " & prefissotabelle & "Mezzi Where idUtente=" & idUtente & " And descMezzo='" & Mezzo & "' " & Altro
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                idMezzo = 1
            Else
                idMezzo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Select Max(Progressivo)+1 From " & prefissotabelle & "AltreInfoMezziRit " & _
                "Where idUtente=" & idUtente & " " & _
                "And Giorno=" & Giorno & " " & _
                "And Mese=" & Mese & " " & _
                "And Anno=" & Anno
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo = 1
            Else
                Progressivo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Insert Into " & prefissotabelle & "AltreInfoMezziRit Values(" & _
                " " & idUtente & ", " & _
                " " & Giorno & ", " & _
                " " & Mese & ", " & _
                " " & Anno & ", " & _
                " " & Progressivo & ", " & _
                " " & idMezzo & " " & _
                ")"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            For i As Integer = 0 To cmbRitorno.Items.Count - 1
                If cmbRitorno.Items(i).Text = cmbRitorno.Text Then
                    cmbRitorno.Items.RemoveAt(i)
                    Exit For
                End If
            Next

            CaricaMezziDiRitorno()
        End If
    End Sub

    Protected Sub SpostaGiuMezzoAndata(ByVal sender As Object, ByVal e As System.EventArgs)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
            Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
            Dim Riga As Integer = Row.RowIndex
            Dim Di As GridViewRow = grdAndata.Rows(Riga)
            Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
            Dim Progressivo1 As Integer = hdnIdRiga.Value

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")
            Dim Progressivo2 As Integer = hdnIdRiga.Value

            Sql = "Select Top 1 Progressivo From " & prefissotabelle & "AltreInfoMezzi Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo>" & Progressivo1 & " Order By Progressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo2 = -1
            Else
                Progressivo2 = Rec(0).Value
            End If
            Rec.Close()

            If Progressivo2 <> -1 Then
                Sql = "Update " & prefissotabelle & "AltreInfoMezzi Set Progressivo=9999 Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=" & Progressivo1
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "AltreInfoMezzi Set Progressivo=" & Progressivo1 & " Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=" & Progressivo2
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "AltreInfoMezzi Set Progressivo=" & Progressivo2 & " Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=9999"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                CaricaMezziDiAndata()
            End If
        End If
    End Sub

    Protected Sub SpostaSuMezzoAndata(ByVal sender As Object, ByVal e As System.EventArgs)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
            Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
            Dim Riga As Integer = Row.RowIndex
            Dim Di As GridViewRow = grdAndata.Rows(Riga)
            Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
            Dim Progressivo1 As Integer = hdnIdRiga.Value

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")
            Dim Progressivo2 As Integer = hdnIdRiga.Value

            Sql = "Select Top 1 Progressivo From " & prefissotabelle & "AltreInfoMezzi Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo<" & Progressivo1 & " Order By Progressivo Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo2 = -1
            Else
                Progressivo2 = Rec(0).Value
            End If
            Rec.Close()

            If Progressivo2 <> -1 Then
                Sql = "Update " & prefissotabelle & "AltreInfoMezzi Set Progressivo=9999 Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=" & Progressivo1
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "AltreInfoMezzi Set Progressivo=" & Progressivo1 & " Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=" & Progressivo2
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "AltreInfoMezzi Set Progressivo=" & Progressivo2 & " Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=9999"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                CaricaMezziDiAndata()
            End If
        End If
    End Sub

    Protected Sub SpostaGiuMezzoRitorno(ByVal sender As Object, ByVal e As System.EventArgs)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
            Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
            Dim Riga As Integer = Row.RowIndex
            Dim Di As GridViewRow = grdRitorno.Rows(Riga)
            Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
            Dim Progressivo1 As Integer = hdnIdRiga.Value

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")
            Dim Progressivo2 As Integer = hdnIdRiga.Value

            Sql = "Select Top 1 Progressivo From " & prefissotabelle & "AltreInfoMezziRit Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo>" & Progressivo1 & " Order By Progressivo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo2 = -1
            Else
                Progressivo2 = Rec(0).Value
            End If
            Rec.Close()

            If Progressivo2 <> -1 Then
                Sql = "Update " & prefissotabelle & "AltreInfoMezziRit Set Progressivo=9999 Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=" & Progressivo1
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "AltreInfoMezziRit Set Progressivo=" & Progressivo1 & " Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=" & Progressivo2
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "AltreInfoMezziRit Set Progressivo=" & Progressivo2 & " Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=9999"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                CaricaMezziDiRitorno()
            End If
        End If
    End Sub

    Protected Sub SpostaSuMezzoRitorno(ByVal sender As Object, ByVal e As System.EventArgs)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
            Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
            Dim Riga As Integer = Row.RowIndex
            Dim Di As GridViewRow = grdRitorno.Rows(Riga)
            Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
            Dim Progressivo1 As Integer = hdnIdRiga.Value

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")
            Dim Progressivo2 As Integer = hdnIdRiga.Value

            Sql = "Select Top 1 Progressivo From " & prefissotabelle & "AltreInfoMezziRit Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo<" & Progressivo1 & " Order By Progressivo Desc"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                Progressivo2 = -1
            Else
                Progressivo2 = Rec(0).Value
            End If
            Rec.Close()

            If Progressivo2 <> -1 Then
                Sql = "Update " & prefissotabelle & "AltreInfoMezziRit Set Progressivo=9999 Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=" & Progressivo1
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "AltreInfoMezziRit Set Progressivo=" & Progressivo1 & " Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=" & Progressivo2
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                Sql = "Update " & prefissotabelle & "AltreInfoMezziRit Set Progressivo=" & Progressivo2 & " Where idUtente=" & idUtente & " And Giorno=" & Giorno & " And Mese=" & Mese & " And Anno=" & Anno & " And Progressivo=9999"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                CaricaMezziDiRitorno()
            End If
        End If
    End Sub

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        Response.Redirect("Principale.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgSalva_Click(sender As Object, e As ImageClickEventArgs) Handles imgSalva.Click
        Dim Tipologia As String = ""
        Dim idTipologia As String = ""

        If optNormale.Checked = True Then
            Tipologia = "NORMALE"
            idTipologia = ""
        Else
            If optFerie.Checked = True Then
                Tipologia = "FERIE"
                idTipologia = "-1"
            Else
                If optMalattia.Checked = True Then
                    Tipologia = "MALATTIA"
                    idTipologia = "-2"
                Else
                    If optAltro.Checked = True Then
                        Tipologia = "ALTRO"
                        idTipologia = "-4"
                    Else
                        If optGiornoPermesso.Checked = True Then
                            Tipologia = "PERMESSO"
                            idTipologia = "-5"
                        Else
                            If optLavoroDaCasa.Checked = True Then
                                Tipologia = "DACASA"
                                idTipologia = "-6"
                            End If
                        End If
                    End If
                End If
            End If
        End If

        Dim OreLavorative As String = ""
        Dim sOrePermesso As String = ""
        Dim sOreMalattia As String = ""
        Dim Societa As String = ""
        Dim Commessa As String = ""
        Dim idSocieta As Integer = -1
        Dim idCommessa As Integer = -1
        Dim OrarioEntrata As String = ""

        Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
        Dim Rec As Object = CreateObject("ADODB.Recordset")
        Dim Sql As String

        Dim Giorno As String = Request.QueryString("Giorno")
        Dim Mese As String = Request.QueryString("Mese")
        Dim Anno As String = Request.QueryString("Anno")

        Dim Misti As String = ""

        If idTipologia = "" Or idTipologia = "-6" Then
            If idTipologia = "-6" Then
                OreLavorative = "-6"
                sOrePermesso = "0"
                sOreMalattia = "0"
                OrarioEntrata = "08:00"
            Else
                OreLavorative = txtOre.Text.Replace(",", ".").Trim
                sOrePermesso = txtPermesso.Text.Replace(",", ".").Trim
                sOreMalattia = txtMalattia.Text.Replace(",", ".").Trim
                OrarioEntrata = txtOrario.Text.Trim.Replace(".", ":")
            End If

            If OreLavorative = "" Or IsNumeric(OreLavorative) = False Or Val(OreLavorative) < 0.5 Or Val(OreLavorative) > 15 Then
                If sOrePermesso = "" And sOreMalattia = "" And sOrePermesso = "0" And sOreMalattia = "0" Then
                    VisualizzaMessaggioInPopup("Ore non valide", Master)
                    Exit Sub
                End If
            End If

            If OrarioEntrata = "" Then
                VisualizzaMessaggioInPopup("Orario di entrata non valido", Master)
                Exit Sub
            Else
                Dim Datella As String = "01/02/2003 " & OrarioEntrata

                If IsDate(Datella) = False Then
                    VisualizzaMessaggioInPopup("Orario di entrata non valido", Master)
                    Exit Sub
                Else
                    Dim d As Date = Datella

                    txtOrario.Text = d.Hour.ToString("00") & ":" & d.Minute.ToString("00") & ":" & d.Second.ToString("00")
                    OrarioEntrata = txtOrario.Text
                End If
            End If

            If txtGradi.Text <> "" Then
                If IsNumeric(txtGradi.Text) = False Then
                    VisualizzaMessaggioInPopup("Gradi non validi", Master)
                    Exit Sub
                End If
            End If

            Societa = cmbSocieta.Text

            If Societa.Trim = "" Then
                VisualizzaMessaggioInPopup("Società non valida", Master)
                Exit Sub
            End If

            Commessa = cmbCommessa.Text

            If Societa.Trim = "" Then
                VisualizzaMessaggioInPopup("Commessa non valida", Master)
                Exit Sub
            End If

            If lblMancanti.Visible = True Then
                VisualizzaMessaggioInPopup("Squadratura nelle ore inserite", Master)
                Exit Sub
            End If

            idSocieta = PrendeIdDaCombo(Societa, "" & PrefissoTabelle & "Lavori", "Lavoro")

            idCommessa = -1

            Sql = "Select * From " & PrefissoTabelle & "Commesse Where " &
                "idUtente=" & idUtente & " And " &
                "Descrizione='" & Commessa & "' And " &
                "idLavoro=" & idSocieta
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                idCommessa = Rec(1).Value
            End If
            Rec.Close()

            Dim orePermesso As Single = Val(txtPermesso.Text.Replace(",", "."))
            Dim oreMalattia As Single = Val(txtMalattia.Text.Replace(",", "."))

            If orePermesso > 0 Or oreMalattia > 0 Then
                If IsNumeric(orePermesso) = False Then
                    VisualizzaMessaggioInPopup("Ore permesso non valide", Master)
                    Exit Sub
                End If
                If IsNumeric(oreMalattia) = False Then
                    VisualizzaMessaggioInPopup("Ore malattia non valide", Master)
                    Exit Sub
                End If

                Misti = "N" & txtOre.Text.Replace(",", ".") & ";"
                Misti += "P" & txtPermesso.Text.Replace(",", ".") & ";"
                Misti += "M" & txtMalattia.Text.Replace(",", ".") & ";"
                Misti += "R0;"
                Misti += "S0"

                OreLavorative = "-3"

                If orePermesso = OreStandard Or oreMalattia = OreStandard Or OreLavorative = -6 Then
                    Sql = "Delete " & PrefissoTabelle & "AltreInfoMezzi Where " &
                            "idUtente=" & idUtente & " And " &
                            "Giorno=" & Giorno & " And " &
                            "Mese=" & Mese & " And " &
                            "Anno=" & Anno & " "
                    EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                    Sql = "Delete " & PrefissoTabelle & "AltreInfoMezziRit Where " &
                            "idUtente=" & idUtente & " And " &
                            "Giorno=" & Giorno & " And " &
                            "Mese=" & Mese & " And " &
                            "Anno=" & Anno & " "
                    EsegueSql(ConnSQL, Sql, ConnessioneSQL)

                    Sql = "Delete " & PrefissoTabelle & "Pranzi2 Where " &
                           "idUtente=" & idUtente & " And " &
                           "idGiorno=" & Giorno & " And " &
                           "idMese=" & Mese & " And " &
                           "idAnno=" & Anno & " "
                    EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                End If
            Else
                If txtStraordinari.Text <> "0" Then
                    OreLavorative = Val(txtStraordinari.Text.Replace(",", ".")) + Val(OreLavorative.Replace(",", "."))
                    OreLavorative = OreLavorative.Replace(",", ".")
                End If
            End If
        Else
            Sql = "Delete " & prefissotabelle & "AltreInfoMezzi Where " & _
                    "idUtente=" & idUtente & " And " & _
                    "Giorno=" & Giorno & " And " & _
                    "Mese=" & Mese & " And " & _
                    "Anno=" & Anno & " "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Delete " & prefissotabelle & "AltreInfoMezziRit Where " & _
                    "idUtente=" & idUtente & " And " & _
                    "Giorno=" & Giorno & " And " & _
                    "Mese=" & Mese & " And " & _
                    "Anno=" & Anno & " "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Delete " & prefissotabelle & "Pranzi2 Where " & _
                   "idUtente=" & idUtente & " And " & _
                   "idGiorno=" & Giorno & " And " & _
                   "idMese=" & Mese & " And " & _
                   "idAnno=" & Anno & " "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            If idTipologia <> "-5" Then
                OreLavorative = idTipologia
                Misti = ""
            Else
                OreLavorative = "-3"

                Misti = "N0;"
                Misti += "P" & OreStandard.ToString.Trim & ";"
                Misti += "M0;"
                Misti += "R0;"
                Misti += "S0"
            End If
        End If

        Dim idIndirizzo As Integer = -1

        Sql = "Select * From " & prefissotabelle & "Indirizzi Where " & _
            "idUtente=" & idUtente & " And " & _
            "Indirizzo='" & cmbIndirizzo.Text.Replace("'", "''") & "'"
        Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
        If Rec.Eof = False Then
            idIndirizzo = Rec("Contatore").Value
        End If
        Rec.Close()

        Sql = "Delete " & prefissotabelle & "Orari Where " & _
                "idUtente=" & idUtente & " And " & _
                "Giorno=" & Giorno & " And " & _
                "Mese=" & Mese & " And " & _
                "Anno=" & Anno & " "
        EsegueSql(ConnSQL, Sql, ConnessioneSQL)

        Dim Km As String = lblKMetri.Text
        If InStr(Km, " ") > 0 Then
            Km = Mid(Km, 1, InStr(Km, " ") - 1).Trim
        End If
        Km = Km.Replace(",", ".").Trim
        If Km = "" Then
            Km = "null"
        End If

        Sql = "Insert Into " & prefissotabelle & "Orari Values (" & _
            " " & idUtente & ", " & _
            " " & Giorno & ", " & _
            " " & Mese & ", " & _
            " " & Anno & ", " & _
            " " & OreLavorative & ", " & _
            " '" & txtNotelle.Text.Replace("'", "''") & "', " & _
            " '" & Misti & "', " & _
            " " & idCommessa & ", " & _
            " '" & OrarioEntrata & "', " & _
            " " & idSocieta & ", " & _
            " " & idIndirizzo & ", " & _
            " " & Km & " " & _
            ")"
        EsegueSql(ConnSQL, Sql, ConnessioneSQL)

        Dim idPasticca As Integer = PrendeIdDaCombo(cmbPasticca.Text, "" & prefissotabelle & "Pasticche", "descPasticca")

        Sql = "Delete " & prefissotabelle & "AltreInfoPasticca Where " & _
            "idUtente=" & idUtente & " And " & _
            "Giorno=" & Giorno & " And " & _
            "Mese=" & Mese & " And " & _
            "Anno=" & Anno & " "
        EsegueSql(ConnSQL, Sql, ConnessioneSQL)

        If cmbPasticca.Text <> "" Then
            Sql = "Insert Into " & prefissotabelle & "AltreInfoPasticca Values (" & _
                " " & idUtente & ", " & _
                " " & Giorno & ", " & _
                " " & Mese & ", " & _
                " " & Anno & ", " & _
                " " & idPasticca & " " & _
                ")"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)
        End If

        Dim idTempo As Integer = PrendeIdDaCombo(cmbTempo.Text, "" & prefissotabelle & "Tempo", "descTempo")

        Sql = "Delete " & prefissotabelle & "AltreInfoTempo Where " & _
            "idUtente=" & idUtente & " And " & _
            "Giorno=" & Giorno & " And " & _
            "Mese=" & Mese & " And " & _
            "Anno=" & Anno & " "
        EsegueSql(ConnSQL, Sql, ConnessioneSQL)

        If cmbTempo.Text <> "" Then
            If txtGradi.Text = "" Then
                txtGradi.Text = "null"
            End If

            Sql = "Insert Into " & prefissotabelle & "AltreInfoTempo Values (" & _
                " " & idUtente & ", " & _
                " " & Giorno & ", " & _
                " " & Mese & ", " & _
                " " & Anno & ", " & _
                " " & idTempo & ", " & _
                " " & txtGradi.Text & " " & _
                ")"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)
        End If

        Dim PercImm As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & Anno & "\" & NomeMese(Mese - 1) & "\" & Giorno & ".jpg"

        Try
            Kill(PercImm)
        Catch ex As Exception

        End Try

        VisualizzaMessaggioInPopup("Dati salvati", Master)
    End Sub

    Protected Sub cmdIndietroPermesso_Click(sender As Object, e As EventArgs) Handles cmdIndietroPermesso.Click
        Dim Ore As Single = Val(txtPermesso.Text.Replace(",", "."))
        Dim OreT As Single = Val(txtOre.Text.Replace(",", "."))

        Ore -= 0.5
        If Ore < 0 Then
            Ore = 0
        Else
            OreT += 0.5
            txtOre.Text = OreT
        End If

        txtPermesso.Text = Ore

        ScriveRimanenzaOre()
    End Sub

    Protected Sub cmdAvantiPermesso_Click(sender As Object, e As EventArgs) Handles cmdAvantipermesso.Click
        Dim Ore As Single = Val(txtPermesso.Text.Replace(",", "."))
        Dim OreT As Single = Val(txtOre.Text.Replace(",", "."))
        Dim OreM As Single = Val(txtMalattia.Text.Replace(",", "."))

        If OreT + OreM + Ore > 0 Then
            Ore += 0.5

            txtPermesso.Text = Ore

            OreT -= 0.5
            txtOre.Text = OreT
        End If

        ScriveRimanenzaOre()
    End Sub

    Protected Sub cmdIndietroMalattia_Click(sender As Object, e As EventArgs) Handles cmdIndietroMalattia.Click
        Dim Ore As Single = Val(txtMalattia.Text.Replace(",", "."))
        Dim OreT As Single = Val(txtOre.Text.Replace(",", "."))

        Ore -= 0.5
        If Ore < 0 Then
            Ore = 0
        Else
            OreT += 0.5
            txtOre.Text = OreT
        End If

        txtMalattia.Text = Ore

        ScriveRimanenzaOre()
    End Sub

    Protected Sub cmdAvantiMalattia_Click(sender As Object, e As EventArgs) Handles cmdAvantiMalattia.Click
        Dim Ore As Single = Val(txtMalattia.Text.Replace(",", "."))
        Dim OreT As Single = Val(txtOre.Text.Replace(",", "."))
        Dim OreP As Single = Val(txtPermesso.Text.Replace(",", "."))

        If OreT + OreP + Ore > 0 Then
            Ore += 0.5

            txtMalattia.Text = Ore

            OreT -= 0.5
            txtOre.Text = OreT
        End If

        ScriveRimanenzaOre()
    End Sub

    Protected Sub cmdIndietroOra_Click(sender As Object, e As EventArgs) Handles cmdIndietroOra.Click
        Dim Ore As Single = Val(txtOre.Text.Replace(",", "."))

        Ore -= 0.5
        If Ore < 0.5 Then
            Ore = 0.5
        End If

        txtOre.Text = Ore

        ScriveRimanenzaOre()
    End Sub

    Protected Sub cmdAvantiOra_Click(sender As Object, e As EventArgs) Handles cmdAvantiora.Click
        Dim Ore As Single = Val(txtOre.Text.Replace(",", "."))
        Dim OreM As Single = Val(txtMalattia.Text.Replace(",", "."))
        Dim OreP As Single = Val(txtPermesso.Text.Replace(",", "."))

        If OreM + OreP + Ore + 0.5 <= OreStandard Then
            Ore += 0.5
        End If

        txtOre.Text = Ore

        ScriveRimanenzaOre()
    End Sub

    Protected Sub cmdIndietroStra_Click(sender As Object, e As EventArgs) Handles cmdIndietroStraord.Click
        Dim Ore As Single = Val(txtStraordinari.Text.Replace(",", "."))

        Ore -= 0.5
        If Ore < 0 Then
            Ore = 0.5
        End If

        txtStraordinari.Text = Ore
    End Sub

    Protected Sub cmdAvantiStra_Click(sender As Object, e As EventArgs) Handles cmdAvantiStraord.Click
        Dim Ore As Single = Val(txtStraordinari.Text.Replace(",", "."))

        Ore += 0.5

        txtStraordinari.Text = Ore
    End Sub

    Private Sub ScriveRimanenzaOre()
        If optNormale.Checked = False Then
            lblMancanti.Text = ""
            lblMancanti.Visible = False
            Exit Sub
        End If

        Dim Ore As Single = Val(txtOre.Text.Replace(",", "."))
        Dim OreM As Single = Val(txtMalattia.Text.Replace(",", "."))
        Dim OreP As Single = Val(txtPermesso.Text.Replace(",", "."))

        If Ore + OreM + OreP <> OreStandard Then
            lblMancanti.Text = "Diff.: " & OreStandard - (Ore + OreM + OreP)
            lblMancanti.Visible = True
        Else
            lblMancanti.Text = ""
            lblMancanti.Visible = False
        End If

        If OreP = 0 And OreM = 0 Then
            lblStraord.Visible = True
            cmdIndietroStraord.Visible = True
            cmdAvantiStraord.Visible = True
            txtStraordinari.Visible = True
        Else
            txtStraordinari.Text = "0"
            lblStraord.Visible = False
            cmdIndietroStraord.Visible = False
            cmdAvantiStraord.Visible = False
            txtStraordinari.Visible = False
        End If
    End Sub

    Protected Sub optNormale_CheckedChanged(sender As Object, e As EventArgs) Handles optNormale.CheckedChanged
        optFerie.Checked = False
        optMalattia.Checked = False
        optAltro.Checked = False
        optNormale.Checked = True
        optGiornoPermesso.Checked = False
        optLavoroDaCasa.Checked = False

        ulDati.Visible = True
        lipranzo.Visible = True
        liPranzo2.Visible = True
        liportata.Visible = True
        ulPasticca.Visible = True
        ulMezzi.Visible = True

        cmbPortata.Visible = True

        Dim DatellaPerEntrata As Date = Now.AddMinutes(-5)

        If txtOre.Text = "" Or txtOre.Text = "0" Then
            txtOre.Text = OreStandard
        End If
        If txtOrario.Text = "" Then
            txtOrario.Text = DatellaPerEntrata.Hour.ToString("00") & ":" & DatellaPerEntrata.Minute.ToString("00") & ":" & DatellaPerEntrata.Second.ToString("00")
        End If

        AggiungeMezziAndataStandard()
        AggiungeMezziRitornoStandard()
    End Sub

    Protected Sub optFerie_CheckedChanged(sender As Object, e As EventArgs) Handles optFerie.CheckedChanged
        optFerie.Checked = True
        optMalattia.Checked = False
        optAltro.Checked = False
        optNormale.Checked = False
        optGiornoPermesso.Checked = False
        optLavoroDaCasa.Checked = False

        ulDati.Visible = False
        lipranzo.Visible = False
        liPranzo2.Visible = False
        liportata.Visible = False
        ulPasticca.Visible = False
        ulMezzi.Visible = False

        cmbPasticca.Text = ""
        cmbTempo.Text = ""
        cmbPortata.Visible = False
    End Sub

    Protected Sub optMalattia_CheckedChanged(sender As Object, e As EventArgs) Handles optMalattia.CheckedChanged
        optFerie.Checked = False
        optMalattia.Checked = True
        optAltro.Checked = False
        optNormale.Checked = False
        optGiornoPermesso.Checked = False
        optLavoroDaCasa.Checked = False

        ulDati.Visible = False
        lipranzo.Visible = False
        liPranzo2.Visible = False
        liportata.Visible = False
        ulPasticca.Visible = False
        ulMezzi.Visible = False

        cmbPasticca.Text = ""
        cmbTempo.Text = ""
        cmbPortata.Visible = False
    End Sub

    Protected Sub optAltro_CheckedChanged(sender As Object, e As EventArgs) Handles optAltro.CheckedChanged
        optFerie.Checked = False
        optMalattia.Checked = False
        optAltro.Checked = True
        optNormale.Checked = False
        optGiornoPermesso.Checked = False
        optLavoroDaCasa.Checked = False

        ulDati.Visible = False
        lipranzo.Visible = False
        liPranzo2.Visible = False
        liportata.Visible = False
        ulPasticca.Visible = False
        ulMezzi.Visible = False

        cmbPasticca.Text = ""
        cmbTempo.Text = ""
        cmbPortata.Visible = False
    End Sub

    Protected Sub optLavoroDaCasa_CheckedChanged(sender As Object, e As EventArgs) Handles optLavoroDaCasa.CheckedChanged
        optNormale.Checked = False
        optFerie.Checked = False
        optMalattia.Checked = False
        optAltro.Checked = False
        optNormale.Checked = False
        optGiornoPermesso.Checked = False
        optLavoroDaCasa.Checked = True

        ulDati.Visible = False
        lipranzo.Visible = False
        liPranzo2.Visible = False
        liportata.Visible = False
        ulPasticca.Visible = False
        ulMezzi.Visible = False

        cmbPasticca.Text = ""
        cmbTempo.Text = ""
        cmbPortata.Visible = False
    End Sub

    Protected Sub imgwsmeteo_Click(sender As Object, e As ImageClickEventArgs) Handles imgwsmeteo.Click
        ScaricaDatiMeteo()
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub optGiornoPermesso_CheckedChanged(sender As Object, e As EventArgs) Handles optGiornoPermesso.CheckedChanged
        optFerie.Checked = False
        optMalattia.Checked = False
        optAltro.Checked = False
        optNormale.Checked = False
        optGiornoPermesso.Checked = True

        ulDati.Visible = False
        lipranzo.Visible = False
        liPranzo2.Visible = False
        ulPasticca.Visible = False
        ulMezzi.Visible = False

        cmbPasticca.Text = ""
        cmbTempo.Text = ""
        cmbPortata.Visible = False
    End Sub

    Protected Sub ModificaMezzoAndata(ByVal sender As Object, ByVal e As System.EventArgs)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            If cmbAndata.Text = "" Then
                VisualizzaMessaggioInPopup("Selezionare un mezzo di andata", Master)
                Exit Sub
            End If

            Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
            Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
            Dim Riga As Integer = Row.RowIndex
            Dim Di As GridViewRow = grdAndata.Rows(Riga)
            Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
            Dim Progressivo As Integer = hdnIdRiga.Value

            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Mezzo As String = cmbAndata.Text.Trim
            Dim Dettaglio As String
            Dim Altro As String
            Dim Sql As String
            Dim idMezzo As Integer

            If InStr(Mezzo, "(") > 0 Then
                Dettaglio = Mid(Mezzo, InStr(Mezzo, "(") + 1, Len(Mezzo))
                Dettaglio = Mid(Dettaglio, 1, InStr(Dettaglio, ")") - 1)
                Mezzo = Mid(Mezzo, 1, InStr(Mezzo, "(") - 1)
                Altro = " And Dettaglio='" & Dettaglio & "'"
            End If

            Sql = "Select idMezzo From " & prefissotabelle & "Mezzi Where idUtente=" & idUtente & " And descMezzo='" & Mezzo & "' " & Altro
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                idMezzo = 1
            Else
                idMezzo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Update " & prefissotabelle & "AltreInfoMezzi " & _
                "Set idMezzo=" & idMezzo & " " & _
                "Where " & _
                "idUtente=" & idUtente & " And " & _
                "Giorno=" & Giorno & " And " & _
                "Mese=" & Mese & " And " & _
                "Anno=" & Anno & " And " & _
                "Progressivo=" & Progressivo
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            For i As Integer = 0 To cmbAndata.Items.Count - 1
                If cmbAndata.Items(i).Text = cmbAndata.Text Then
                    cmbAndata.Items.RemoveAt(i)
                    Exit For
                End If
            Next

            CaricaMezziDiAndata()
        End If
    End Sub

    Protected Sub ModificaMezzoRitorno(ByVal sender As Object, ByVal e As System.EventArgs)
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            If cmbRitorno.Text = "" Then
                VisualizzaMessaggioInPopup("Selezionare un mezzo di ritorno", Master)
                Exit Sub
            End If

            Dim Bottone As ImageButton = DirectCast(sender, ImageButton)
            Dim Row As GridViewRow = DirectCast(Bottone.NamingContainer, GridViewRow)
            Dim Riga As Integer = Row.RowIndex
            Dim Di As GridViewRow = grdRitorno.Rows(Riga)
            Dim hdnIdRiga As HiddenField = DirectCast(Di.FindControl("hdnIdRiga"), HiddenField)
            Dim Progressivo As Integer = hdnIdRiga.Value

            Dim Giorno As String = Request.QueryString("Giorno")
            Dim Mese As String = Request.QueryString("Mese")
            Dim Anno As String = Request.QueryString("Anno")

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Mezzo As String = cmbRitorno.Text.Trim
            Dim Dettaglio As String
            Dim Altro As String
            Dim Sql As String
            Dim idMezzo As Integer

            If InStr(Mezzo, "(") > 0 Then
                Dettaglio = Mid(Mezzo, InStr(Mezzo, "(") + 1, Len(Mezzo))
                Dettaglio = Mid(Dettaglio, 1, InStr(Dettaglio, ")") - 1)
                Mezzo = Mid(Mezzo, 1, InStr(Mezzo, "(") - 1)
                Altro = " And Dettaglio='" & Dettaglio & "'"
            End If

            Sql = "Select idMezzo From " & prefissotabelle & "Mezzi Where idUtente=" & idUtente & " And descMezzo='" & Mezzo & "' " & Altro
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec(0).Value Is DBNull.Value = True Then
                idMezzo = 1
            Else
                idMezzo = Rec(0).Value
            End If
            Rec.Close()

            Sql = "Update " & prefissotabelle & "AltreInfoMezziRit " & _
                "Set idMezzo=" & idMezzo & " " & _
                "Where " & _
                "idUtente=" & idUtente & " And " & _
                "Giorno=" & Giorno & " And " & _
                "Mese=" & Mese & " And " & _
                "Anno=" & Anno & " And " & _
                "Progressivo=" & Progressivo
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            ConnSQL.Close()

            For i As Integer = 0 To cmbRitorno.Items.Count - 1
                If cmbRitorno.Items(i).Text = cmbRitorno.Text Then
                    cmbRitorno.Items.RemoveAt(i)
                    Exit For
                End If
            Next

            CaricaMezziDiRitorno()
        End If
    End Sub

    Protected Sub imgAggiungeRit_Click(sender As Object, e As ImageClickEventArgs) Handles imgAggiungeRit.Click
        If cmbRitorno.Text = "" Then
            VisualizzaMessaggioInPopup("Selezionare un mezzo di ritorno", Master)
            Exit Sub
        End If

        AggiungeMezzoRitorno(sender, e)
    End Sub

    Protected Sub imgAggiungeAndata_Click(sender As Object, e As ImageClickEventArgs) Handles imgAggiungeAndata.Click
        If cmbAndata.Text = "" Then
            VisualizzaMessaggioInPopup("Selezionare un mezzo di andata", Master)
            Exit Sub
        End If

        AggiungeMezzoAndata(sender, e)
    End Sub
End Class