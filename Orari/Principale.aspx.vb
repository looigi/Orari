Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging

Public Class Principale
	Inherits System.Web.UI.Page

	Private NomiOperazioni() As String = {"Processo Chiuso", "Processo Aperto", "Applicazione In Chiusura", "Applicazione Aperta", "Utente Sloggato", "Windows Spento", "Windows Ripristinato", "Windows In Cambio Stato", "Windows Sospeso"}

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If idUtente = -1 Or idUtente = 0 Or Utenza = "" Then
			Response.Redirect("Default.aspx")
		End If

		'If Page.IsPostBack = False Then
		If Not Page.IsPostBack Then
			hdnGiornoMemorizzato.Value = ""

			CaricaCombo()

			LeggeOreStandard(Server.MapPath("."))

			cmbMese.Text = NomeMesi(Now.Month)
			cmbAnno.Text = Now.Year

			NumeroMese = Now.Month
			cmbMese.Text = NomeMesi(NumeroMese)
			'End If

			'If Request.QueryString("Anno") <> "" Then
			'    cmbAnno.Text = Request.QueryString("Anno")
			'Else
			cmbAnno.Text = Now.Year
			'End If
			NumeroAnno = Val(cmbAnno.Text)
		Else
			NumeroMese = RitornaNumeroMese()
			NumeroAnno = Val(cmbAnno.Text)
		End If

		CreaPagine()

		'If Request.QueryString("Mese") <> "" Then
		'    NumeroMese = Request.QueryString("Mese")
		'    cmbMese.Text = NomeMesi(NumeroMese)
		'Else

		'If Request.QueryString("Note") <> "" Then
		'    divNote.Visible = True

		'    Dim Giorno As Integer = Request.QueryString("Giorno")
		'    Dim Mese As Integer = Request.QueryString("Mese")
		'    Dim Anno As Integer = Request.QueryString("Anno")
		'    Dim NomeMese As String = Request.QueryString("NomeMese")

		'    VisualizzaNote(Giorno, Mese, Anno, NomeMese)
		'Else
		divNote.Visible = False
		'End If

		'If Request.QueryString("RicercaNote") <> "" Then
		'    divRicercaNote.Visible = True
		'Else
		divRicercaNote.Visible = False
		'End If

		lblRicercati.Text = ""
		divDettaglioGiorno.Visible = False
		divBlocca.Visible = False
		'Else
		'End If
	End Sub

	Private Sub CaricaCombo()
		cmbMese.Items.Clear()
		cmbMese.Items.Add("Gennaio")
		cmbMese.Items.Add("Febbraio")
		cmbMese.Items.Add("Marzo")
		cmbMese.Items.Add("Aprile")
		cmbMese.Items.Add("Maggio")
		cmbMese.Items.Add("Giugno")
		cmbMese.Items.Add("Luglio")
		cmbMese.Items.Add("Agosto")
		cmbMese.Items.Add("Settembre")
		cmbMese.Items.Add("Ottobre")
		cmbMese.Items.Add("Novembre")
		cmbMese.Items.Add("Dicembre")

		'For i As Integer = 0 To cmbMese.Items.Count - 1
		'    NomeMesi(i + 1) = cmbMese.Items(i).Text
		'Next

		cmbAnno.Items.Clear()

		If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
			Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
			Dim Rec As Object = CreateObject("ADODB.Recordset")
			Dim Sql As String = "Select Distinct(Anno) As sAnno From " & prefissotabelle & "Orari Where idUtente= " & idUtente & " Order By Anno Desc"
			Dim Ok As Boolean = False

			Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			Do Until Rec.Eof
				If Rec("sAnno").Value = Now.Year Then
					Ok = True
				End If
				cmbAnno.Items.Add(Rec("sAnno").Value)

				Rec.MoveNext()
			Loop
			Rec.Close()

			If Ok = False Then
				cmbAnno.Items.Add(Now.Year)
				cmbAnno.Text = Now.Year
			End If

			ConnSQL.Close()
		End If
	End Sub

	Private Sub CaricaRicorrenze(nMese As Integer)
		If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
			Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
			Dim Rec As Object = CreateObject("ADODB.Recordset")
			Dim Sql As String
			Dim Stringona As String = ""
			Dim Giorni(31) As String
			Dim s As String
			Dim a As Integer
			Dim b As Integer

			Stringona = "'',"

			Sql = "Select GG, AAAA, [Desc] From RICO_Dati " &
				"Where MM = " & nMese & " " &
				"Order By GG, Progr, AAAA"

			For i As Integer = 1 To 31
				Giorni(i) = ""
			Next
			Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			Do Until Rec.Eof
				s = Format(Rec("GG").Value, "00") & " " & NomeMesi(nMese) & " " & Rec("AAAA").Value & " " & Rec("Desc").Value.ToString
				s = s.Replace(",", " ")

				Giorni(Rec("GG").Value) += s & "*ACAPO*"

				Rec.MoveNext()
			Loop
			Rec.Close()

			For i As Integer = 1 To 31
				If Giorni(i) <> "" Then
					Giorni(i) = Mid(Giorni(i), 1, Len(Giorni(i)) - 6)
					Giorni(i) = "'" & Giorni(i) & "'"
				Else
					Giorni(i) = "''"
				End If
				'Giorni(i) = Giorni(i).Replace("***S***", "<b>")
				'Giorni(i) = Giorni(i).Replace("***F***", "</b>")

				While InStr(Giorni(i), "***S***") > 0
					a = InStr(Giorni(i), "***S***")
					If a > 0 Then
						s = Mid(Giorni(i), a + 7, Giorni(i).Length)
						b = InStr(s, "***F***")
						If b > 0 Then
							s = Mid(Giorni(i), a + 7, b - 1)
							Giorni(i) = Mid(Giorni(i), 1, a - 1) & "**MIN***a href=""" & "https://www.google.it/search?q=" & s & """ target=""_blank""**MAX***" & Mid(Giorni(i), a + 7, b - 1) & "**MIN***/a**MAX***" & Mid(Giorni(i), (a + 7) + (b - 1) + 7, Giorni(i).Length)
						Else
							Giorni(i) = Giorni(i).Replace("***S***", "")
						End If
					End If

				End While

				Stringona += Giorni(i) & ","
			Next
			Stringona = Mid(Stringona, 1, Len(Stringona) - 1)

			ConnSQL.Close()

			hdnRicorrenze.Value = Stringona

			Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
			sb.Append("<script type='text/javascript' language='javascript'>")
			sb.Append("     RiempieRicorrenze();")
			sb.Append("</script>")

			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCRRIC", sb.ToString(), False)
		End If
	End Sub

	Private Sub CreaPagine()
		If NumeroMese = 0 Or cmbAnno.Text = "" Then
			Exit Sub
		End If

		Dim QuantiGiorni As Integer = Date.DaysInMonth(cmbAnno.Text, NumeroMese)

		If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
			Dim ConnSQL As Object = ApreDB(ConnessioneSQL)

			CreaInizioMese(ConnSQL, "0.jpg", NumeroMese)

			For i As Integer = 1 To QuantiGiorni
				CreaPagina(ConnSQL, i & ".jpg", i, NumeroMese)
			Next
			For i As Integer = QuantiGiorni + 1 To 31
				Dim Salvataggio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & i & ".jpg"

				Try
					Kill(Salvataggio)
				Catch ex As Exception

				End Try
			Next

			img0.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\0.jpg")
			img1.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\1.jpg")
			img2.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\2.jpg")
			img3.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\3.jpg")
			img4.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\4.jpg")
			img5.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\5.jpg")
			img6.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\6.jpg")
			img7.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\7.jpg")
			img8.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\8.jpg")
			img9.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\9.jpg")
			img10.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\10.jpg")
			img11.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\11.jpg")
			img12.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\12.jpg")
			img13.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\13.jpg")
			img14.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\14.jpg")
			img15.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\15.jpg")
			img16.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\16.jpg")
			img17.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\17.jpg")
			img18.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\18.jpg")
			img19.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\19.jpg")
			img20.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\20.jpg")
			img21.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\21.jpg")
			img22.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\22.jpg")
			img23.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\23.jpg")
			img24.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\24.jpg")
			img25.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\25.jpg")
			img26.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\26.jpg")
			img27.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\27.jpg")
			img28.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\28.jpg")
			img29.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\29.jpg")
			img30.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\30.jpg")
			img31.Src = RitornaImmagine("App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\31.jpg")

			ConnSQL.Close()

			CaricaRicorrenze(NumeroMese)

			EsegueSpostamentoLibroAGiornoAttuale()
		Else
			' Problemi di lettura del file di configurazione db
		End If
	End Sub

	Private Function RitornaImmagine(Immagine As String) As String
		Dim Ritorno As String = Immagine

		If Dir(Server.MapPath(".") & "\" & Ritorno) = "" Then
			Ritorno = "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\standard.jpg"
		End If
		Ritorno += "?" & CreaStringaPerRefresh() & "=" & Now.Millisecond

		Return Ritorno
	End Function

	Private Sub EsegueSpostamentoLibroAGiornoAttuale()
		Dim Quando As String

		Dim Giorno As Integer = -1

		If hdnGiornoMemorizzato.Value <> "" Then
			Giorno = Val(hdnGiornoMemorizzato.Value) ' + 2
			hdnGiornoMemorizzato.Value = ""
		End If

		If Request.QueryString("Giorno") = "" Then
			If Giorno > -1 Then
				Quando = Giorno
			Else
				If NomeMesi(Now.Month) = cmbMese.Text And Now.Year = cmbAnno.Text Then
					Quando = Now.Day.ToString.Trim
					Quando -= 1
					If Quando / 2 <> Int(Quando / 2) Then
						Quando -= 1
					End If
				Else
					Quando = 0
				End If
			End If
		Else
			Quando = Request.QueryString("Giorno")
			If Quando / 2 <> Int(Quando / 2) Then
				Quando -= 1
			End If
		End If

		Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
		sb.Append("<script type='text/javascript' language='javascript'>")
		sb.Append("     RidisegnaLibro('" & Quando & "');")
		sb.Append("</script>")

		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)
	End Sub

	Private Sub CreaCartelle()
		Try
			MkDir(Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text)
		Catch ex As Exception

		End Try
		Try
			MkDir(Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text)
		Catch ex As Exception

		End Try
	End Sub

	Private Function RitornaNumeroMese() As Integer
		Dim Ritorno As Integer
		Dim MeseImpostato As String = cmbMese.Text.ToUpper.Trim

		If MeseImpostato = "" Then
			Return Now.Month
			Exit Function
		End If
		For i = 1 To 12
			If NomeMesi(i).ToUpper.Trim = MeseImpostato Then
				Ritorno = i
				Exit For
			End If
		Next

		Return Ritorno
	End Function

	Private Sub CreaInizioMese(Conn As Object, NomeFile As String, NumeroMese As Integer)
		CreaCartelle()

		If Dir(Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\" & NomeFile) = "" Then
			Dim PercImmagine As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & cmbMese.Text & ".jpg"
			'Dim PercImmagine As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idutente & "-" & utenza & "\Standard.jpg"
			Dim Salvataggio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\" & NomeFile
			Dim Scritta As String = cmbMese.Text & " " & cmbAnno.Text
			Dim Dettaglio As String = ContaGiorniLavorativi(RitornaNumeroMese(), cmbAnno.Text)
			Dim ScrittaDettaglio As String = "Giorni lavorativi: " & Mid(Dettaglio, 1, InStr(Dettaglio, "*") - 1)
			Dim ScrittaSettimane As String = Mid(Dettaglio, InStr(Dettaglio, "*") + 1, Dettaglio.Length)
			Dim Imm As System.Drawing.Image = System.Drawing.Image.FromFile(PercImmagine)
			Dim DimeXFoto As Integer = Imm.Width - 10
			Dim DimeYFoto As Integer = Imm.Height - 10
			Dim objBitmap As New Bitmap(DimeXFoto, DimeYFoto)
			Dim objGraphic As Graphics = Graphics.FromImage(objBitmap)
			Dim PosizioneFoto As PointF = New PointF(0.0F, 0.0F)
			Dim Lavorati As String = ""
			Dim Rec As Object = CreateObject("ADODB.Recordset")
			Dim Sql As String

			Sql = "Select 'Lavorati' As Tipo, Count(*) From " & prefissotabelle & "Orari Where idUtente=" & idUtente & " And Anno=" & cmbAnno.Text & " And Mese=" & NumeroMese & " And CodCommessa>0 And Quanto>0 " &
				"Union All " &
				"Select 'Ferie' As Tipo, Count(*) From " & prefissotabelle & "Orari Where idUtente=" & idUtente & " And Anno=" & cmbAnno.Text & " And Mese=" & NumeroMese & " And Quanto=-1 " &
				"Union All " &
				"Select 'Malattie' As Tipo, Count(*) From " & prefissotabelle & "Orari Where idUtente=" & idUtente & " And Anno=" & cmbAnno.Text & " And Mese=" & NumeroMese & " And Quanto=-2 " &
				"Union All " &
				"Select 'Assenze' As Tipo, Count(*) From " & prefissotabelle & "Orari Where idUtente=" & idUtente & " And Anno=" & cmbAnno.Text & " And Mese=" & NumeroMese & " And Quanto=-3 " &
				"Union All " &
				"Select 'Straordinari' As Tipo, Sum(Quanto-8) From " & prefissotabelle & "Orari Where idUtente=" & idUtente & " And Anno=" & cmbAnno.Text & " And Mese=" & NumeroMese & " And Quanto>8"
			Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
			Do Until Rec.Eof
				If Rec(1).Value Is DBNull.Value = False Then
					Lavorati += Rec(0).Value & ": " & Rec(1).Value & ";"
				Else
					Lavorati += Rec(0).Value & ": " & "0;"
				End If

				Rec.MoveNext()
			Loop
			Rec.Close()

			objGraphic.DrawImage(Imm, PosizioneFoto)

			objGraphic.DrawRectangle(Nero, 2, 2, DimeXFoto - 7, DimeYFoto - 5)

			Dim InizioY As Single = (DimeYFoto / 2) - 95
			Dim FineY As Single = 190
			Dim PosizioneScrittaMese As PointF = New PointF((DimeXFoto / 2) - (11 * Scritta.Length / 2), InizioY + 7)
			Dim PosizioneScrittaDettaglio As PointF = New PointF((DimeXFoto / 2) - (10 * ScrittaDettaglio.Length / 2), InizioY + 35)
			Dim PosizioneScrittaSettimane As PointF = New PointF((DimeXFoto / 2) - (10 * ScrittaSettimane.Length / 2), InizioY + 60)

			objGraphic.FillRectangle(grayBrush, 4, InizioY, DimeXFoto - 11, FineY)
			objGraphic.DrawRectangle(Nero, 4, InizioY, DimeXFoto - 11, FineY)
			objGraphic.DrawString(Scritta, drawFontGrosso, greenBrush, PosizioneScrittaMese)
			objGraphic.DrawLine(Nero, 8, InizioY + 33, DimeXFoto - 15, InizioY + 33)
			objGraphic.DrawString(ScrittaDettaglio, drawFontMedio, redBrush, PosizioneScrittaDettaglio)
			objGraphic.DrawString(ScrittaSettimane, drawFontMedio, redBrush, PosizioneScrittaSettimane)

			objGraphic.DrawLine(Nero, 8, InizioY + 83, DimeXFoto - 15, InizioY + 83)

			Dim ScrittaLavorati() As String = Lavorati.Split(";")
			Dim Altezza As Integer = 0
			For i As Integer = 0 To UBound(ScrittaLavorati) - 1
				Dim PosizioneScrittaLavorati As PointF = New PointF((DimeXFoto / 2) - (8 * ScrittaLavorati(i).Length / 2), InizioY + 85 + Altezza)

				objGraphic.DrawString(ScrittaLavorati(i), drawFontPiccolo, blueBrush, PosizioneScrittaLavorati)
				Altezza += 20
			Next

			Dim PercImmagineOmbra As String
			Dim PosizioneFotoOmbra As PointF
			Dim posOmbra As Integer

			PercImmagineOmbra = Server.MapPath(".") & "\App_Themes\Standard\Images\black_gradient.png"
			posOmbra = DimeXFoto - 80

			Imm = System.Drawing.Image.FromFile(PercImmagineOmbra)

			For i As Integer = 0 To DimeYFoto
				PosizioneFotoOmbra = New PointF(posOmbra, i)
				objGraphic.DrawImage(Imm, PosizioneFotoOmbra)
			Next

			objBitmap.Save(Salvataggio, ImageFormat.Jpeg)

			objBitmap = Nothing
			objGraphic = Nothing
			Imm = Nothing
		End If
	End Sub

	Private Sub CreaPagina(Conn As Object, NomeFile As String, NumeroGiorno As Integer, NumeroMese As Integer)
		If Dir(Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\" & NomeFile) = "" Then
			Dim PercImmagine As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Standard.jpg"
			Dim Salvataggio As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\" & NomeFile
			Dim PosizioneFoto As PointF = New PointF(0.0F, 0.0F)

			Dim Imm As System.Drawing.Image = System.Drawing.Image.FromFile(PercImmagine)
			Dim DimeXFoto As Integer = Imm.Width - 10
			Dim DimeYFoto As Integer = Imm.Height - 10

			Dim objBitmap As New Bitmap(DimeXFoto, DimeYFoto)
			Dim objGraphic As Graphics = Graphics.FromImage(objBitmap)

			Dim Datella As Date = NumeroGiorno & "/" & NumeroMese & "/" & cmbAnno.Text
			Dim brush As SolidBrush
			Dim penna As Pen
			Dim GiornoTestuale As String = MetteMaiuscole(Datella.ToString("dddd"))
			Dim Testo As String = GiornoTestuale & " " & NumeroGiorno & " " & cmbMese.Text & " " & cmbAnno.Text
			Dim PosizioneScrittaIntestazione As PointF
			Dim PosizioneScrittaSanto As PointF

			Dim Rec As Object = CreateObject("ADODB.Recordset")
			Dim Sql As String

			Dim OreLavorate As String = ""
			Dim Entrata As String = ""
			Dim Commessa As String = ""
			Dim Lavoro As String = ""
			Dim Notelle As String = ""
			Dim Misti As String = ""
			Dim Gradi As String = ""
			Dim idTempo As Integer = -1
			Dim DescTempo As String = ""
			Dim NonCeNulla As Boolean = False
			Dim Indirizzo As String = ""
			Dim Km As String = ""

			Sql = "Select A.*, B.Descrizione, C.Lavoro, E.idTempo, E.descTempo, D.Gradi, F.Indirizzo From " & PrefissoTabelle & "Orari A " &
				"Left Join " & PrefissoTabelle & "Commesse B On A.CodCommessa = B.Codice And A.idUtente=B.idUtente " &
				"Left Join " & PrefissoTabelle & "Lavori C On A.idLavoro = C.idLavoro And A.idUtente=C.idUtente " &
				"Left Join " & PrefissoTabelle & "AltreInfoTempo D On A.Anno=D.Anno And A.Mese=D.Mese And A.Giorno = D.Giorno And A.idUtente=D.idUtente " &
				"Left Join " & PrefissoTabelle & "Tempo E On D.idTempo = E.idTempo And A.idUtente=E.idUtente " &
				"Left Join " & PrefissoTabelle & "Indirizzi F On A.idIndirizzo=F.Contatore " &
				"Where A.Anno=" & cmbAnno.Text & " And A.Mese=" & NumeroMese & " And A.Giorno=" & NumeroGiorno & " And A.idUtente=" & idUtente
			Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
			If Rec.Eof = False Then
				OreLavorate = "" & Rec("Quanto").Value.ToString.Trim
				Entrata = "" & Rec("Entrata").Value.ToString.Trim
				Commessa = "" & Rec("Descrizione").Value.ToString.Trim
				Lavoro = "" & Rec("Lavoro").Value.ToString.Trim
				Notelle = "" & Rec("Notelle").Value.ToString.Trim.Replace("''", "'")
				Misti = "" & Rec("Misti").Value.ToString.Trim
				Gradi = "" & Rec("Gradi").Value.ToString
				Indirizzo = "" & Rec("Indirizzo").Value.ToString
				Km = "" & Rec("Km").Value.ToString

				If Gradi <> "" Then
					Gradi = " (" & Gradi & "°)"
				End If
				If Rec("idTempo").Value Is DBNull.Value = False Then
					idTempo = Rec("idTempo").Value
				Else
					idTempo = -1
				End If
				DescTempo = "" & Rec("desctempo").Value.ToString.Trim & Gradi

				If OreLavorate = "" Or Commessa = "" Then
					NonCeNulla = True
				End If
			Else
				'Rec.Close()

				'objGraphic.DrawImage(Imm, PosizioneFoto)

				'objBitmap.Save(Salvataggio, ImageFormat.Jpeg)

				'objBitmap = Nothing
				'objGraphic = Nothing
				'Imm = Nothing

				'Exit Sub
			End If
			Rec.Close()

			Dim Santo As String = ""

			Sql = "Select Nome From Santi Where Data='" & NumeroGiorno.ToString("00") & "/" & NumeroMese.ToString("00") & "'"
			Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
			If Rec.Eof = False Then
				Santo = "S. " & Rec("Nome").Value
			End If
			Rec.Close()

			If NumeroGiorno / 2 = Int(NumeroGiorno / 2) Then
				PosizioneScrittaIntestazione = New PointF(10.0F, 10.0F)
				PosizioneScrittaSanto = New PointF(DimeXFoto - (11 * (Santo.Length + 1)) - 10, 10.0F)
			Else
				PosizioneScrittaSanto = New PointF(10.0F, 10.0F)
				PosizioneScrittaIntestazione = New PointF(DimeXFoto - (10.5 * (Testo.Length + 1)) - 15, 10.0F)
			End If

			If ControllaFestivo(Datella) = True Then
				brush = redBrush
				penna = Rosso
			Else
				brush = blackBrush
				penna = Nero
			End If

			objGraphic.DrawImage(Imm, PosizioneFoto)

			For i As Integer = 0 To DimeYFoto Step 30
				objGraphic.DrawLine(Grigio, 0, i, DimeXFoto, i)
			Next

			Dim InfoVarie As String
			Dim PosizioneScrittaInfo As PointF

			InfoVarie = "Giorno dell'anno: " & Datella.DayOfYear.ToString("000")

			If NumeroGiorno / 2 = Int(NumeroGiorno / 2) Then
				PosizioneScrittaInfo = New PointF(DimeXFoto - (8 * (InfoVarie.Length + 1)) - 12, 35.0F)
			Else
				PosizioneScrittaInfo = New PointF(12.0F, 35.0F)
			End If

			objGraphic.DrawRectangle(Nero, 2, 2, DimeXFoto - 7, DimeYFoto - 7)
			objGraphic.DrawString(Testo, drawFontGrosso, brush, PosizioneScrittaIntestazione)
			objGraphic.DrawString(Santo, drawFontGrosso, redBrush, PosizioneScrittaSanto)
			objGraphic.DrawString(InfoVarie, drawFontPiccolo, greenBrush, PosizioneScrittaInfo)
			objGraphic.DrawLine(penna, 8, 55, DimeXFoto - 15, 55)

			Dim PosizioneScrittaTesto As PointF

			If idTempo <> -1 Then
				Dim PercImmagineTempo As String = Server.MapPath(".") & "\App_Themes\Standard\Images\Tempo\" & idTempo & ".png"
				Dim PosizioneFotoTempo As PointF

				Imm = System.Drawing.Image.FromFile(PercImmagineTempo)

				If NumeroGiorno / 2 = Int(NumeroGiorno / 2) Then
					PosizioneFotoTempo = New PointF(DimeXFoto - 150, 60)
					PosizioneScrittaTesto = New PointF(DimeXFoto - (7 * (DescTempo.Length + 1)) - 20, 180)
				Else
					PosizioneFotoTempo = New PointF(20, 60)
					PosizioneScrittaTesto = New PointF(20, 180)
				End If

				objGraphic.DrawImage(Imm, PosizioneFotoTempo)
				objGraphic.DrawString(DescTempo, drawFontCorsivo, blackBrush, PosizioneScrittaTesto)
			End If

			Dim PosY As Single = 90
			Dim PosX As Single = 20
			Dim Scritta As String
			Dim Scritta2 As String = ""

			If OreLavorate <> "" Or Entrata <> "" Then
				Select Case OreLavorate
					Case Is > 0
						Scritta = "Ore lavorate: " & OreLavorate
						Scritta2 = "Entrata: " & Entrata & ""
					Case -1
						Scritta = "Ferie"
						Commessa = ""
					Case -2
						Scritta = "Malattia"
						Commessa = ""
					Case -3
						'Misto
						Dim Campi() As String = Misti.Split(";")

						Scritta = "L.: " & Mid(Campi(0), 2, 3) & " P.: " & Mid(Campi(1), 2, 3) & " M.: " & Mid(Campi(2), 2, 3) & " A.: " & Mid(Campi(3), 2, 3)
						If Entrata <> "" Then Scritta2 = "Entrata: " & Entrata
					Case -4
						Scritta = "Solidarietà"
						Commessa = ""
					Case -6
						Scritta = "Ore lavorate da casa: 8"
						Scritta2 = "Entrata: " & Entrata & ""
					Case Else
						Scritta = ""
						Commessa = ""
				End Select
				If NumeroGiorno / 2 = Int(NumeroGiorno / 2) Then
					PosizioneScrittaTesto = New PointF(PosX, PosY)
				Else
					PosizioneScrittaTesto = New PointF(DimeXFoto - (12 * (Scritta.Length + 1)), PosY)
				End If
				objGraphic.DrawString(Scritta, drawFontMedio, blueBrush, PosizioneScrittaTesto)

				PosY += 40

				If NumeroGiorno / 2 = Int(NumeroGiorno / 2) Then
					PosizioneScrittaTesto = New PointF(PosX, PosY)
				Else
					PosizioneScrittaTesto = New PointF(DimeXFoto - (12 * (Scritta2.Length + 1)), PosY)
				End If
				objGraphic.DrawString(Scritta2, drawFontMedio, blueBrush, PosizioneScrittaTesto)

				PosY += 40
			End If

			If Commessa <> "" Then
				' Logo commessa
				Dim PosizioneFotologo As PointF
				Dim PosizioneFotolavoro As PointF

				Try
					MkDir(Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi")
				Catch ex As Exception

				End Try

				Dim PercImmagineLogo As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & Commessa & ".png"
				Dim PercImmagineLogoResized As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\Resized\" & Commessa & ".png"
				Dim PercImmagineLavoro As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\" & Lavoro & ".png"
				Dim PercImmagineLavoroResized As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\Resized\" & Lavoro & ".png"

				Dim gf As New GestioneFilesDirectory
				gf.CreaDirectoryDaPercorso(Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\Resized\")
				gf = Nothing

				If Dir(PercImmagineLavoro) <> "" Then
					Try
						Dim gi As New GestioneImmagini
						If Not File.Exists(PercImmagineLogoResized) Then
							gi.Ridimensiona(PercImmagineLogo, PercImmagineLogoResized, 150, 150)
						End If
						If Not File.Exists(PercImmagineLavoroResized) Then
							gi.Ridimensiona(PercImmagineLavoro, PercImmagineLavoroResized, 150, 150)
						End If
						gi = Nothing

						Dim Imm2 As System.Drawing.Image = System.Drawing.Image.FromFile(PercImmagineLavoroResized)

						Imm = System.Drawing.Image.FromFile(PercImmagineLogoResized)

						Scritta = "Commessa: " & Commessa & " per " & Lavoro
						If NumeroGiorno / 2 = Int(NumeroGiorno / 2) Then
							PosizioneScrittaTesto = New PointF(PosX, PosY)
						Else
							PosizioneScrittaTesto = New PointF(DimeXFoto - (11 * (Scritta.Length + 1)) - 10, PosY)
						End If
						objGraphic.DrawString(Scritta, drawFontMedio, blueBrush, PosizioneScrittaTesto)

						PosizioneFotolavoro = New PointF((DimeXFoto) - (Imm2.Width) - 30, (DimeYFoto / 2) - (Imm2.Height))
						PosizioneFotologo = New PointF((DimeXFoto) - (Imm.Width) - 30, (DimeYFoto / 2))

						objGraphic.DrawImage(Imm2, PosizioneFotolavoro)
						objGraphic.DrawImage(Imm, PosizioneFotologo)
					Catch ex As Exception

					End Try
				End If
				' Logo commessa

				' Indirizzo
				If Indirizzo <> "" Then
					Dim Altro As String = ""

					If Km <> "" Then
						Altro = " (Km. " & Km & ")"
					End If

					PosY += 21
					Scritta = "Da: " & Indirizzo & Altro
					If NumeroGiorno / 2 = Int(NumeroGiorno / 2) Then
						PosizioneScrittaTesto = New PointF(PosX, PosY)
					Else
						PosizioneScrittaTesto = New PointF(DimeXFoto - (6.5 * (Scritta.Length + 1)) - 8, PosY)
					End If
					objGraphic.DrawString(Scritta, drawFontPiccolissimo, Brushes.Black, PosizioneScrittaTesto)
				End If
				' Indirizzo
			End If

			If NonCeNulla = False Then
				' Barra divisione sotto il tempo
				PosY = 215
				objGraphic.DrawLine(penna, 8, PosY, DimeXFoto - 15, PosY)
			End If

			Dim ok As Boolean

			If Notelle <> "" Then
				PosY = 620
				PosizioneScrittaTesto = New PointF(20, PosY)
				objGraphic.DrawString("Note del giorno:", drawFontCorsivoGrande, redBrush, PosizioneScrittaTesto)

				' Controlla se è una partita ed eventualmente chi ha vinto
				If InStr(Notelle.ToUpper, "LAZIO") > 0 Then
					Dim Risultato As String = ""

					For i As Integer = 1 To Notelle.Length
						If IsNumeric(Mid(Notelle, i, 1)) = True Then
							Risultato = Mid(Notelle, i, Len(Notelle)).Replace(" ", "")
							Exit For
						End If
					Next
					If Risultato <> "" Then
						Dim Primo As Integer
						Dim Secondo As Integer

						Try
							Primo = Mid(Risultato, 1, 1)
							Secondo = Mid(Risultato, 3, 1)
						Catch ex As Exception

						End Try

						Dim Trattino As Integer = InStr(Notelle, "-")
						Dim Lazio As Integer = InStr(Notelle.ToUpper, "LAZIO")
						Dim Casa As Boolean
						Dim Ris As String

						If Lazio < Trattino Then
							Casa = True
							Select Case Primo - Secondo
								Case 0
									Ris = "PAREGGIO"
								Case Is < 0
									Ris = "SCONFITTA"
								Case Is > 0
									Ris = "VITTORIA"
							End Select
						Else
							Casa = False
							Select Case Primo - Secondo
								Case 0
									Ris = "PAREGGIO"
								Case Is < 0
									Ris = "VITTORIA"
								Case Is > 0
									Ris = "SCONFITTA"
							End Select
						End If

						Dim PercImmaginePuffo As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & Ris & ".png"
						Dim PosizioneFotoPuffo As PointF

						PosizioneFotoPuffo = New PointF(DimeXFoto - 210, 600)
						Imm = System.Drawing.Image.FromFile(PercImmaginePuffo)

						objGraphic.DrawImage(Imm, PosizioneFotoPuffo)
					End If
				End If
				' Controlla se è una partita ed eventualmente chi ha vinto

				PosY = 650

				Dim lunghezza As Integer = 50
				Dim testoNote As String

				While Notelle.Length > lunghezza
					testoNote = Mid(Notelle, 1, lunghezza)
					ok = False
					For i As Integer = testoNote.Length To 1 Step -1
						If Mid(testoNote, i, 1) = " " Then
							testoNote = Mid(testoNote, 1, i - 1).Trim
							Notelle = Mid(Notelle, i + 1, Len(Notelle)).Trim
							ok = True
							Exit For
						End If
					Next
					PosizioneScrittaTesto = New PointF(20, PosY)
					objGraphic.DrawString(testoNote.Replace("''", "'"), drawFontCorsivoGrande, blackBrush, PosizioneScrittaTesto)
					PosY += 28
					If ok = False Then
						Notelle = Mid(Notelle, lunghezza + 1, Len(Notelle)).Trim
						Exit While
					End If
				End While
				PosizioneScrittaTesto = New PointF(20, PosY)
				objGraphic.DrawString(Notelle, drawFontCorsivoGrande, blackBrush, PosizioneScrittaTesto)
			End If

			' Pranzo
			Dim Portate() As String
			Dim QuantePortate As Integer = 0

			Sql = "Select A.Portata From " & PrefissoTabelle & "Portate A " &
				"Left Join " & PrefissoTabelle & "Pranzi2 B On A.idPortata = B.idPortata And A.idUtente=B.idUtente " &
				"Where A.idUtente=" & idUtente & " And idGiorno = " & NumeroGiorno & " And idMese = " & NumeroMese & " And idAnno = " & cmbAnno.Text & " " &
				"Order By idProgressivo"
			Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
			Do Until Rec.Eof
				QuantePortate += 1
				ReDim Preserve Portate(QuantePortate)
				Portate(QuantePortate) = "" & Rec("Portata").Value.ToString.Trim

				Rec.MoveNext()
			Loop
			Rec.Close()
			If QuantePortate > 0 Then
				PosY = 220
				Scritta = "Oggi la casa, per pranzo, ha passato:"
				PosizioneScrittaTesto = New PointF(20, PosY)
				objGraphic.DrawString(Scritta, drawFontMedio, greenBrush, PosizioneScrittaTesto)
				For i As Integer = 1 To QuantePortate
					PosY += 20
					PosizioneScrittaTesto = New PointF(20, PosY)
					objGraphic.DrawString(Portate(i), drawFontMedio, blackBrush, PosizioneScrittaTesto)
				Next
			End If
			' Pranzo

			' Mezzi Andata
			PosY += 50
			Dim Altmezzi As Integer = PosY
			Dim MaxAltMezzi As Integer = 0

			Dim MezziAnd() As String
			Dim QuantiMezziAnd As Integer = 0

			Sql = "Select descMezzo , Dettaglio From " & PrefissoTabelle & "AltreInfoMezzi A " &
				"Left Join " & PrefissoTabelle & "Mezzi B On A.idMezzo = B.idMezzo And A.idUtente=B.idUtente " &
				"Where A.idUtente=" & idUtente & " And Giorno = " & NumeroGiorno & " And Mese = " & NumeroMese & " And Anno = " & cmbAnno.Text & " " &
				"Order By Progressivo"
			Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
			Do Until Rec.Eof
				QuantiMezziAnd += 1
				ReDim Preserve MezziAnd(QuantiMezziAnd)
				MezziAnd(QuantiMezziAnd) = "" & Rec("descMezzo").Value.ToString.Trim & " " & Rec("Dettaglio").Value.ToString.Trim

				Rec.MoveNext()
			Loop
			Rec.Close()
			If QuantiMezziAnd > 0 Then
				Scritta = "Mezzi di andata: "
				PosizioneScrittaTesto = New PointF(20, PosY)
				objGraphic.DrawString(Scritta, drawFontMedio, blueBrush, PosizioneScrittaTesto)
				For i As Integer = 1 To QuantiMezziAnd
					PosY += 20
					If PosY > MaxAltMezzi Then MaxAltMezzi = PosY

					PosizioneScrittaTesto = New PointF(20, PosY)
					objGraphic.DrawString(MezziAnd(i), drawFontMedio, blackBrush, PosizioneScrittaTesto)
				Next
			End If
			' Mezzi Andata

			' Mezzi Ritorno
			PosY += 30

			Dim MezziRit() As String
			Dim QuantiMezziRit As Integer = 0

			Sql = "Select descMezzo , Dettaglio From " & PrefissoTabelle & "AltreInfoMezziRit A " &
				"Left Join " & PrefissoTabelle & "Mezzi B On A.idMezzo = B.idMezzo And A.idUtente=B.idUtente " &
				"Where A.idUtente=" & idUtente & " And Giorno = " & NumeroGiorno & " And Mese = " & NumeroMese & " And Anno = " & cmbAnno.Text & " " &
				"Order By Progressivo"
			Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
			Do Until Rec.Eof
				QuantiMezziRit += 1
				ReDim Preserve MezziRit(QuantiMezziRit)
				MezziRit(QuantiMezziRit) = "" & Rec("descMezzo").Value.ToString.Trim & " " & Rec("Dettaglio").Value.ToString.Trim

				Rec.MoveNext()
			Loop
			Rec.Close()
			If QuantiMezziRit > 0 Then
				Scritta = "Mezzi di ritorno:"
				PosizioneScrittaTesto = New PointF(20, PosY)
				objGraphic.DrawString(Scritta, drawFontMedio, blueBrush, PosizioneScrittaTesto)
				For i As Integer = 1 To QuantiMezziRit
					PosY += 20
					If PosY > MaxAltMezzi Then MaxAltMezzi = PosY

					PosizioneScrittaTesto = New PointF(20, PosY)
					objGraphic.DrawString(MezziRit(i), drawFontMedio, blackBrush, PosizioneScrittaTesto)
				Next
			End If
			' Mezzi Ritorno

			' Pasticca
			PosY = MaxAltMezzi + 40
			Sql = "Select B.descPasticca From " & PrefissoTabelle & "AltreInfoPasticca A Left Join " &
				"" & PrefissoTabelle & "Pasticche B On A.idPasticca = B.idPasticca And A.idUtente=B.idUtente " &
				"Where A.idUtente=" & idUtente & " And Giorno = " & NumeroGiorno & " And Mese = " & NumeroMese & " And Anno = " & cmbAnno.Text & " "
			Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
			If Rec.Eof = False Then
				PosizioneScrittaTesto = New PointF(20, PosY)
				objGraphic.DrawString("E purtroppo, ho dovuto ingerire: ", drawFontMedio, redBrush, PosizioneScrittaTesto)
				PosY += 20
				PosizioneScrittaTesto = New PointF(20, PosY)
				objGraphic.DrawString("" & Rec("descPasticca").Value, drawFontMedio, blackBrush, PosizioneScrittaTesto)
			End If
			Rec.Close()
			' Pasticca

			Dim GiorniLavorati As Integer = 0
			Dim GiorniNONLavorati As Integer = 0

			If OreLavorate <> "" Or Entrata <> "" Then
				Dim Oggi As String = cmbAnno.Text & Format(NumeroMese, "00") & Format(NumeroGiorno, "00")

				' Calcolo Giorni lavorati
				Sql = "Select Count(*) As TotGiorni From " & PrefissoTabelle & "Orari " &
					"Where idUtente = " & idUtente & " And (((Misti Is Null Or Misti='') And Quanto>0) " &
					"Or (Quanto=-3 And Charindex('P8', Misti)=0) Or (Quanto=-6)) " &
					"And Cast(Ltrim(Rtrim(Str(Anno)))+" &
					"Replicate('0', 2-Len(Ltrim(Rtrim(Str(Mese)))))+Ltrim(Rtrim(Str(Mese)))+" &
					"Replicate('0', 2-Len(Ltrim(Rtrim(Str(Giorno)))))+Ltrim(Rtrim(Str(Giorno))) As Integer) " &
					"<=" & Oggi
				Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
				If Rec(0).value Is DBNull.Value = False Then
					GiorniLavorati = Rec(0).Value
				End If
				Rec.Close()

				Dim TestoGLavorati As String = "Giorni lavorati: " & Format(GiorniLavorati, "#,##0")

				' Calcolo Giorni NON lavorati
				Sql = "Select Count(*) As TotGiorni From " & PrefissoTabelle & "Orari " &
					"Where idUtente = " & idUtente & " And (Quanto<=0 " &
					"And Charindex('P8', Misti)>0) " &
					"And Cast(Ltrim(Rtrim(Str(Anno)))+" &
					"Replicate('0', 2-Len(Ltrim(Rtrim(Str(Mese)))))+Ltrim(Rtrim(Str(Mese)))+" &
					"Replicate('0', 2-Len(Ltrim(Rtrim(Str(Giorno)))))+Ltrim(Rtrim(Str(Giorno))) As Integer) " &
					"<=" & Oggi
				Rec = LeggeQuery(Conn, Sql, ConnessioneSQL)
				If Rec(0).value Is DBNull.Value = False Then
					GiorniNONLavorati = Rec(0).Value
				End If
				Rec.Close()

				Dim TestoGNonLavorati As String = "Giorni NON lavorati: " & Format(GiorniNONLavorati, "#,##0")

				Dim PosizioneScrittaGiorniLavorati As PointF
				Dim PosizioneScrittaGiorniNonLavorati As PointF

				PosizioneScrittaGiorniLavorati = New PointF(DimeXFoto - (7.5 * (TestoGLavorati.Length + 1)) - 10, DimeYFoto - 40)
				PosizioneScrittaGiorniNonLavorati = New PointF(DimeXFoto - (7.5 * (TestoGNonLavorati.Length + 1)) - 10, DimeYFoto - 60)

				objGraphic.DrawString(TestoGLavorati, drawFontCorsivo, blackBrush, PosizioneScrittaGiorniLavorati)
				objGraphic.DrawString(TestoGNonLavorati, drawFontCorsivo, blackBrush, PosizioneScrittaGiorniNonLavorati)
			End If

			Dim PercImmagineOmbra As String
			Dim PosizioneFotoOmbra As PointF
			Dim posOmbra As Integer

			If NumeroGiorno / 2 = Int(NumeroGiorno / 2) Then
				PercImmagineOmbra = Server.MapPath(".") & "\App_Themes\Standard\Images\black_gradient.png"
				posOmbra = DimeXFoto - 80
			Else
				PercImmagineOmbra = Server.MapPath(".") & "\App_Themes\Standard\Images\black_gradientr.png"
				posOmbra = 0
			End If

			Imm = System.Drawing.Image.FromFile(PercImmagineOmbra)

			For i As Integer = 0 To DimeYFoto
				PosizioneFotoOmbra = New PointF(posOmbra, i)
				objGraphic.DrawImage(Imm, PosizioneFotoOmbra)
			Next

			objBitmap.Save(Salvataggio, ImageFormat.Jpeg)

			objBitmap = Nothing
			objGraphic = Nothing
			Imm = Nothing
		End If
	End Sub

	Protected Sub imgAggiornamento_Click() ' Handles imgAggiornamento.Click
		Response.Redirect("Aggiornamento.aspx")
	End Sub

	Protected Sub imgRefreshMese_Click(sender As Object, e As ImageClickEventArgs) Handles imgRefreshMese.Click
		Dim Percorso As String = Server.MapPath(".") & "\" & "App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text

		For i As Integer = 0 To 31
			Try
				Kill(Percorso & "\" & i.ToString.Trim & ".jpg")
			Catch ex As Exception

			End Try
		Next

		CreaPagine()

		'Dim Mese As Integer = RitornaNumeroMese()

		'Response.Redirect("Principale.aspx?Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub

	'Protected Sub imgAggiornaDati_Click(sender As Object, e As ImageClickEventArgs) Handles imgAggiornaDati.Click
	'    Response.Redirect("AggDati.aspx")
	'End Sub

	Protected Sub imgModSX_Click(sender As Object, e As ImageClickEventArgs) Handles imgModSX.Click
		Dim GiornoDaModificare As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		Response.Redirect("ModificaGiorno.aspx?Giorno=" & GiornoDaModificare & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub

	Protected Sub imgModDX_Click(sender As Object, e As ImageClickEventArgs) Handles imgModDX.Click
		Dim GiornoDaModificare As Integer = Val(hdnGiorno.Value) + 2
		Dim Mese As Integer = RitornaNumeroMese()

		Response.Redirect("ModificaGiorno.aspx?Giorno=" & GiornoDaModificare & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub

	Protected Sub imgEliSX_Click(sender As Object, e As ImageClickEventArgs) Handles imgEliSX.Click
		If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
			Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
			Dim Mese As Integer = RitornaNumeroMese()
			Dim PercImm As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\" & Giorno & ".jpg"

			Try
				Kill(PercImm)
			Catch ex As Exception

			End Try

			Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
			Dim Sql As String

			Sql = "Delete " & prefissotabelle & "Orari Where " &
					"idUtente=" & idUtente & " And " &
					"Giorno=" & Giorno & " And " &
					"Mese=" & Mese & " And " &
					"Anno=" & cmbAnno.Text & " "
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "Pranzi2 Where " &
				"idUtente=" & idUtente & " And " &
				"idGiorno=" & Giorno & " And " &
				"idMese=" & Mese & " And " &
				"idAnno=" & cmbAnno.Text & " "
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "AltreInfoPasticca Where " &
				"idUtente=" & idUtente & " And " &
				"Giorno=" & Giorno & " And " &
				"Mese=" & Mese & " And " &
				"Anno=" & cmbAnno.Text & " "
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "AltreInfoTempo Where " &
				"idUtente=" & idUtente & " And " &
				"Giorno=" & Giorno & " And " &
				"Mese=" & Mese & " And " &
				"Anno=" & cmbAnno.Text & " "
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "AltreInfoMezzi " &
			   "Where idUtente=" & idUtente & " " &
			   "And Giorno=" & Giorno & " " &
			   "And Mese=" & Mese & " " &
			   "And Anno=" & cmbAnno.Text
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "AltreInfoMezziRit " &
			   "Where idUtente=" & idUtente & " " &
			   "And Giorno=" & Giorno & " " &
			   "And Mese=" & Mese & " " &
			   "And Anno=" & cmbAnno.Text
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			ConnSQL.Close()

			CreaPagine()

			' Response.Redirect("Principale.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
		End If
	End Sub

	Protected Sub imgEliDX_Click(sender As Object, e As ImageClickEventArgs) Handles imgEliDX.Click
		If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
			Dim Giorno As Integer = Val(hdnGiorno.Value) + 2
			Dim Mese As Integer = RitornaNumeroMese()
			Dim PercImm As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & cmbAnno.Text & "\" & cmbMese.Text & "\" & Giorno & ".jpg"

			Try
				Kill(PercImm)
			Catch ex As Exception

			End Try

			Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
			Dim Sql As String

			Sql = "Delete " & prefissotabelle & "Orari Where " &
					"idUtente=" & idUtente & " And " &
					"Giorno=" & Giorno & " And " &
					"Mese=" & Mese & " And " &
					"Anno=" & cmbAnno.Text & " "
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "AltreInfoPasticca Where " &
				"idUtente=" & idUtente & " And " &
				"Giorno=" & Giorno & " And " &
				"Mese=" & Mese & " And " &
				"Anno=" & cmbAnno.Text & " "
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "Pranzi2 Where " &
					"idUtente=" & idUtente & " And " &
					"idGiorno=" & Giorno & " And " &
					"idMese=" & Mese & " And " &
					"idAnno=" & cmbAnno.Text & " "
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "AltreInfoTempo Where " &
				"idUtente=" & idUtente & " And " &
				"Giorno=" & Giorno & " And " &
				"Mese=" & Mese & " And " &
				"Anno=" & cmbAnno.Text & " "
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "AltreInfoMezzi " &
			   "Where idUtente=" & idUtente & " " &
			   "And Giorno=" & Giorno & " " &
			   "And Mese=" & Mese & " " &
			   "And Anno=" & cmbAnno.Text
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			Sql = "Delete " & prefissotabelle & "AltreInfoMezziRit " &
			   "Where idUtente=" & idUtente & " " &
			   "And Giorno=" & Giorno & " " &
			   "And Mese=" & Mese & " " &
			   "And Anno=" & cmbAnno.Text
			EsegueSql(ConnSQL, Sql, ConnessioneSQL)

			ConnSQL.Close()

			CreaPagine()

			' Response.Redirect("Principale.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
		End If
	End Sub

	Private Sub VisualizzaNote(Giorno As Integer, Mese As Integer, Anno As Integer, NomeMese As String)
		Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
		Dim Rec As Object = CreateObject("ADODB.Recordset")
		Dim Sql As String = "Select Notelle From " & prefissotabelle & "Orari Where idUtente=" & idUtente & " And Anno=" & cmbAnno.Text & " And Mese=" & Mese & " And Giorno=" & Giorno

		Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
		If Rec.Eof = True Then
			txtNotelle.Text = ""
		Else
			txtNotelle.Text = Rec("Notelle").Value
		End If
		Rec.Close()

		divNote.Visible = True
	End Sub

	Protected Sub imgNoteSX_Click(sender As Object, e As ImageClickEventArgs) Handles imgNoteSX.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		divNote.Visible = True

		Dim NomeMese As String = cmbMese.Text.ToUpper.Trim

		hdnGiornoNote.Value = Giorno
		VisualizzaNote(Giorno, Mese, NumeroAnno, NomeMese)

		' Response.Redirect("Principale.aspx?Note=1&Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text & "&NomeMese=" & cmbMese.Text)
	End Sub

	Protected Sub imgNoteDX_Click(sender As Object, e As ImageClickEventArgs) Handles imgNoteDX.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 2
		Dim Mese As Integer = RitornaNumeroMese()

		divNote.Visible = True

		Dim NomeMese As String = cmbMese.Text.ToUpper.Trim

		hdnGiornoNote.Value = Giorno
		VisualizzaNote(Giorno, Mese, NumeroAnno, NomeMese)

		'Response.Redirect("Principale.aspx?Note=1&Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text & "&NomeMese=" & cmbMese.Text)
	End Sub

	Protected Sub cmdAnnulla_Click(sender As Object, e As EventArgs) Handles cmdAnnulla.Click
		divNote.Visible = False
		' Response.Redirect("Principale.aspx")
	End Sub

	Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
		Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
		Dim Sql As String
		Dim Giorno As Integer = hdnGiornoNote.Value
		Dim Mese As Integer = RitornaNumeroMese()
		Dim Anno As Integer = NumeroAnno
		Dim NomeMese As String = cmbMese.Text.ToUpper.Trim
		Dim Rec As Object = CreateObject("ADODB.Recordset")

		Sql = "Select * From " & prefissotabelle & "Orari Where idUtente=" & idUtente & " And Anno=" & cmbAnno.Text & " And Mese=" & Mese & " And Giorno=" & Giorno
		Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
		If Rec.Eof = True Then
			Sql = "Insert Into " & prefissotabelle & "Orari Values (" &
				" " & idUtente & ", " &
				" " & Giorno & ", " &
				" " & Mese & ", " &
				" " & Anno & ", " &
				" 0, " &
				" '" & txtNotelle.Text.Replace("'", "''") & "', " &
				"'', " &
				" -1, " &
				" '', " &
				" null, " &
				" null, " &
				" 0 " &
				")"
		Else
			Sql = "Update " & prefissotabelle & "Orari " &
				"Set Notelle='" & txtNotelle.Text.Replace("'", "''") & "' " &
				"From " & prefissotabelle & "Orari " &
				"Where idUtente=" & idUtente & " " &
				"And Anno=" & Anno & " " &
				"And Mese=" & Mese & " " &
				"And Giorno=" & Giorno
		End If
		Rec.Close()

		EsegueSql(ConnSQL, Sql, ConnessioneSQL)

		Dim PercImm As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\" & Anno & "\" & NomeMese & "\" & Giorno & ".jpg"

		Try
			Kill(PercImm)
		Catch ex As Exception

		End Try

		CreaPagina(ConnSQL, Giorno.ToString & ".jpg", Giorno, Mese)

		ConnSQL.Close()
		' Response.Redirect("Principale.aspx")
	End Sub

	Protected Sub imgImpostazioni_Click(sender As Object, e As ImageClickEventArgs) Handles imgImpostazioni.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		Response.Redirect("Impostazioni.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub

	Protected Sub imgStatistiche_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatistiche.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		Response.Redirect("Statistiche.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub

	Protected Sub imgTabelle_Click(sender As Object, e As ImageClickEventArgs) Handles imgTabelle.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		Response.Redirect("GestioneTabelle.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub

	Protected Sub imgMappa_Click(sender As Object, e As ImageClickEventArgs) Handles imgMappa.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		Response.Redirect("Mappa.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub

	Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
		idUtente = -1
		Utenza = ""
		Response.Redirect("Default.aspx")
	End Sub

	Protected Sub imgRicerca_Click(sender As Object, e As ImageClickEventArgs) Handles imgRicerca.Click
		'Dim Giorno As Integer = Val(hdnGiorno.Value) + 2
		'Dim Mese As Integer = RitornaNumeroMese()

		'Response.Redirect("Principale.aspx?RicercaNote=1&Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text & "&NomeMese=" & cmbMese.Text)
		divRicercaNote.Visible = True
	End Sub

	Protected Sub cmdRicerca_Click(sender As Object, e As EventArgs) Handles cmdRicerca.Click
		RicercaNote()
	End Sub

	Private Sub RicercaNote()
		If txtRicerca.Text = "" Then
			VisualizzaMessaggioInPopup("Selezionare un testo da ricercare", Master)
			Exit Sub
		End If

		Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
		Dim Rec As Object = CreateObject("ADODB.Recordset")
		Dim Sql As String = "Select * From " & prefissotabelle & "Orari Where idUtente=" & idUtente & " And Notelle Like '%" & txtRicerca.Text & "%' Order By Anno Desc, Mese Desc, Giorno Desc"

		Dim dGiorno As New DataColumn("Giorno")
		Dim dNota As New DataColumn("Nota")
		Dim rigaR As DataRow
		Dim dttTabella As New DataTable()
		Dim dataPartenza As Date
		Dim sDatellaI As String
		Dim Mesi() As String = {"Gen", "Feb", "Mar", "Apr", "Mag", "Giu", "Lug", "Ago", "Set", "Ott", "Nov", "Dic"}
		Dim Quante As Integer = 0

		dttTabella = New DataTable
		dttTabella.Columns.Add(dGiorno)
		dttTabella.Columns.Add(dNota)

		Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
		Do Until Rec.Eof
			Quante += 1

			dataPartenza = Rec("Giorno").Value & "/" & Rec("Mese").Value & "/" & Rec("Anno").Value

			sDatellaI = dataPartenza.Day & " " & Mesi(dataPartenza.Month - 1) & " " & dataPartenza.Year

			rigaR = dttTabella.NewRow()
			rigaR(0) = MetteMaiuscole(dataPartenza.ToString("dddd")).Substring(0, 3) & " " & sDatellaI
			rigaR(1) = "" & Rec("Notelle").Value
			dttTabella.Rows.Add(rigaR)

			Rec.MoveNext()
		Loop
		Rec.Close()

		lblRicercati.Text = "Ricorrenze: " & Quante

		grdRicerca.DataSource = dttTabella
		grdRicerca.DataBind()
		grdRicerca.SelectedIndex = -1

		ConnSQL.Close()

		divRicercaNote.Visible = True
	End Sub

	Private Sub grdRicerca_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdRicerca.PageIndexChanging
		grdRicerca.PageIndex = e.NewPageIndex
		grdRicerca.DataBind()

		RicercaNote()
	End Sub
	Protected Sub cmdChiude_Click(sender As Object, e As EventArgs) Handles cmdChiude.Click
		'Dim Giorno As Integer = Val(hdnGiorno.Value) + 2
		'Dim Mese As Integer = RitornaNumeroMese()

		'Response.Redirect("Principale.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text & "&NomeMese=" & cmbMese.Text)
		divRicercaNote.Visible = False
	End Sub

	Protected Sub imgPulizia_Click(sender As Object, e As ImageClickEventArgs) Handles imgPulizia.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		Response.Redirect("PuliziaPregresso.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub

	Protected Sub imgDettaglioGiornoSX_Click(sender As Object, e As ImageClickEventArgs) Handles imgDettaglioGiornoSX.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		hdnGiornoMemorizzato.Value = Giorno

		divDettaglioGiorno.Visible = True
		divBlocca.Visible = True

		DisegnaMappa(Giorno, Mese)
	End Sub

	Protected Sub imgDettaglioGiornoDX_Click(sender As Object, e As ImageClickEventArgs) Handles imgDettaglioGiornoDX.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 2
		Dim Mese As Integer = RitornaNumeroMese()

		hdnGiornoMemorizzato.Value = Giorno

		divDettaglioGiorno.Visible = True
		divBlocca.Visible = True

		DisegnaMappa(Giorno, Mese)
	End Sub

	Private Sub DisegnaMappa(Giorno As Integer, Mese As Integer)
		If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL", True) = True Then
			Dim g As String = Giorno.ToString
			Dim m As String = Mese.ToString
			If g.Length = 1 Then g = "0" & g
			If m.Length = 1 Then m = "0" & m
			Dim a As String = NumeroAnno.ToString
			Dim Datella As String = a & "-" & m & "-" & g

			lblDettaglio.Text = "Dettaglio per il giorno " & g & "/" & m & "/" & a

			Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
			Dim Rec As Object = CreateObject("ADODB.Recordset")
			Dim Sql As String

			Dim Tutto As New ArrayList
			Dim OraMinima As String = "999999"
			Dim OraMassima As String = "000000"

			'Dim KmEffettuati As New ArrayList
			Dim dx As ArrayList = New ArrayList
			Dim dy As ArrayList = New ArrayList
			'Dim km As ArrayList = New ArrayList
			Dim ddh As ArrayList = New ArrayList
			Dim mmx As ArrayList = New ArrayList
			Dim mmy As ArrayList = New ArrayList
			Dim nomeFile As ArrayList = New ArrayList
			Dim tipologia As ArrayList = New ArrayList
			'Dim maxKm As Integer = 0

			Sql = "Select * From Posizioni Where DataPos Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Quando"
			Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			Do Until Rec.Eof
				Dim o As Date = Rec("DataPos").Value
				Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

				dx.Add("" & Rec("Lat").Value)
				dy.Add("" & Rec("Lon").Value)
				'km.Add("" & Rec("Km").Value)
				'If Rec("Km").Value > maxKm Then
				'	maxKm = Rec("Km").Value
				'End If

				ddh.Add(os)

				Rec.MoveNext
			Loop
			Rec.Close

			Sql = "Select * From Multimedia Where DataPos Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Quando"
			Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			Do Until Rec.Eof
				Dim o As Date = Rec("DataPos").Value
				Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

				mmx.Add("" & Rec("Lat").Value)
				mmy.Add("" & Rec("Lon").Value)
				tipologia.Add("" & Rec("Tipologia").Value)
				nomeFile.Add("" & Rec("NomeFile").Value)

				Rec.MoveNext
			Loop
			Rec.Close

			'KmEffettuati.Add(maxKm)

			'Dim Messaggi As New ArrayList
			'Sql = "Select * From Messaggi Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

			'	Messaggi.Add(os & ";" & Rec("MittDest").Value & ":" & Rec("Contenuto").Value & ";")
			'	Tutto.Add(os & ";" & Rec("MittDest").Value & ":" & Rec("Contenuto").Value & ";2;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim CanzoniAscoltate As New ArrayList
			'Sql = "Select * From MP3 Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

			'	CanzoniAscoltate.Add(os & ";" & Rec("Cantante").Value & " " & Rec("Album").Value & " " & Rec("Canzone").Value & ";")
			'	Tutto.Add(os & ";" & Rec("Cantante").Value & " " & Rec("Album").Value & " " & Rec("Canzone").Value & ";3;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim Cronologia As New ArrayList
			'Sql = "Select * From Cronologia Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

			'	Cronologia.Add(os & ";" & Rec("Url").Value & " (" & Rec("Titolo").Value & ");")
			'	Tutto.Add(os & ";" & Rec("Url").Value & " (" & Rec("Titolo").Value & ";4;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim ImmaginiScaricate As New ArrayList
			'Sql = "Select * From ImmaginiScaricate Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

			'	ImmaginiScaricate.Add(os & ";" & Rec("Url").Value & ";")
			'	Tutto.Add(os & ";" & Rec("Url").Value & ";5;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim LogActivity As New ArrayList
			'Sql = "Select * From LogActivity Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

			'	LogActivity.Add(os & ";" & Rec("Applicazione").Value & ": " & Rec("Modifica").Value & ";")
			'	Tutto.Add(os & ";" & Rec("Applicazione").Value & ": " & Rec("Modifica").Value & ";6;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim ImmaginiLocali As New ArrayList
			'Sql = "Select * From Locali Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

			'	ImmaginiLocali.Add(os & ";" & Rec("Path").Value & ";")
			'	Tutto.Add(os & ";" & Rec("Path").Value & ";7;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim PartiteCC As New ArrayList
			'Sql = "Select * From Partite Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

			'	PartiteCC.Add(os & ";" & Rec("Categoria").Value & "-" & Rec("Avversario").Value & " " & Rec("Risultato").Value & ";")
			'	Tutto.Add(os & ";" & Rec("Categoria").Value & "-" & Rec("Avversario").Value & " " & Rec("Risultato").Value & ";10;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim Telefonate As New ArrayList
			'Sql = "Select * From Telefonate Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")
			'	Dim e As String = Rec("EntrataUscita").Value.ToString.ToUpper
			'	If e = "E" Then
			'		e = "ENTRATA"
			'	Else
			'		e = "USCITA"
			'	End If
			'	Telefonate.Add(os & ";" & e & " " & Rec("Numero").Value & ";")
			'	Tutto.Add(os & ";" & Rec("Numero").Value & ";11;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim Notifiche As New ArrayList
			'Sql = "Select * From Notifiche Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' Order By Data"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	Dim o As Date = Rec("Data").Value
			'	Dim os As String = Format(o.Hour, "00") & ":" & Format(o.Minute, "00") & ":" & Format(o.Second, "00") & "." & Format(o.Millisecond, "00")

			'	Notifiche.Add(os & ";" & Rec("Notifica").Value & ";")
			'	Tutto.Add(os & ";" & Rec("Notifica").Value & ";12;")

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'Dim Sistema As New ArrayList
			'Sql = "Select 'Movimento mouse:' As Descrizione, Sum(diffx+diffY) As Dettaglio From Sistema "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' "
			'Sql &= "Union All "
			'Sql &= "Select 'Click sinistro:' As Descrizione,  Sum(ClikSinistro) As Dettaglio From Sistema "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' "
			'Sql &= "Union All "
			'Sql &= "Select 'Click destro:' As Descrizione,  Sum(ClickDestro) As Dettaglio From Sistema "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' "
			'Sql &= "Union All "
			'Sql &= "Select 'Tasti Premuti:' As Descrizione,  Sum(tastiPremuti) As Dettaglio  From Sistema "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' "
			'Sql &= "Union All "
			'Sql &= "Select 'Processi aperti:' As Descrizione,  Sum(ProcessiAperti) As Dettaglio  From Sistema  "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' "
			'Sql &= "Union All "
			'Sql &= "Select 'Processi chiusi:' As Descrizione,  Sum(ProcessiChiusi) As Dettaglio  From Sistema "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' "
			'Sql &= "Union All "
			'Sql &= "Select 'Tempo attività:' As Descrizione,  Sum(SecondiAttivita) As Dettaglio  From Sistema "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' "
			'Sql &= "Union All "
			'Sql &= "Select 'Tempo inattività:' As Descrizione,  Sum(SecondiInattivita) As Dettaglio  From Sistema "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' And idMacchina<>2 "
			'Sql &= "Union All "
			'Sql &= "Select 'Tipologia'+CONVERT(varchar(1), Tipologia) As Descrizione, Count(*) As Dettaglio From Applicazioni "
			'Sql &= "Where Data Between '" & Datella & " 00:00:00' And '" & Datella & " 23:59:59' "
			'Sql &= "Group By Tipologia"
			'Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
			'Do Until Rec.Eof
			'	If Not Rec("Dettaglio").Value Is DBNull.Value Then
			'		Dim descr As String = Rec("Descrizione").Value
			'		Dim dett As String = Rec("Dettaglio").Value

			'		If descr.Contains("Tempo ") Then
			'			Dim s As Single = Val(dett.Replace(",", "."))
			'			Dim mi As Integer = 0
			'			Dim h As Integer = 0
			'			While s > 59
			'				mi += 1
			'				If mi > 59 Then
			'					mi -= 60
			'					h += 1
			'				End If
			'				s -= 60
			'			End While
			'			dett = Format(h, "00") & ":" & Format(mi, "00") & ":" & Format(s, "00")
			'		Else
			'			If descr.Contains("Movimento ") Then
			'				Dim x As Single = Val(dett.Replace(",", "."))
			'				If x < 10 Then
			'					dett = "Mm."
			'				Else
			'					If x < 100 Then
			'						dett = "Cm."
			'						x /= 10
			'					Else
			'						If x < 1000 Then
			'							dett = "Dm."
			'							x /= 100
			'						Else
			'							If x < 10000 Then
			'								dett = "M."
			'								x /= 1000
			'							Else
			'								If x < 100000 Then
			'									dett = "Dam."
			'									x /= 10000
			'								Else
			'									If x < 1000000 Then
			'										dett = "Hm."
			'										x /= 100000
			'									Else
			'										If x < 10000000 Then
			'											dett = "Km."
			'											x /= 1000000
			'										End If
			'									End If
			'								End If
			'							End If
			'						End If
			'					End If
			'				End If
			'				x = Int(x * 1000) / 1000
			'				dett = dett & " " & x
			'			Else
			'				If descr.Contains("Tipologia") Then
			'					descr = descr.Replace("Tipologia", "")
			'					descr = NomiOperazioni(Val(descr))
			'				End If
			'			End If
			'		End If

			'		Sistema.Add(Datella & ";" & descr & " " & dett & ";")
			'	End If

			'	Rec.MoveNext
			'Loop
			'Rec.Close

			'For Each t As String In Tutto
			'	Dim tt() As String = t.Split(";")
			'	Dim h As String = tt(0)
			'	If h.Contains(".") Then
			'		h = Mid(h, 1, h.IndexOf("."))
			'	End If
			'	h = h.Replace(":", "")

			'	If Val(h) < Val(OraMinima) Then
			'		OraMinima = h
			'	End If
			'	If h <> "235959" Then
			'		If Val(h) > Val(OraMassima) Then
			'			OraMassima = h
			'		End If
			'	End If
			'Next

			ConnSQL.Close

			Dim filetti As String = ""
			Dim mX As String = ""
			Dim mY As String = ""
			Dim tipo As String = ""
			Dim posi As Integer = 0
			Dim m2 As String = m.Trim
			If m2.Length = 1 Then m2 = "0" & m2

			Dim gf As New GestioneFilesDirectory
			gf.ScansionaDirectorySingola("G:\gDrive\Pennetta\Locali")
			Dim filettis() As String = gf.RitornaFilesRilevati
			Dim qf As Integer = gf.RitornaQuantiFilesRilevati

			Dim PercorsoPennetta As String = ConfigurationManager.AppSettings("percorsoPennetta")
			If PercorsoPennetta.Substring(PercorsoPennetta.Length - 1) <> "\" Then
				PercorsoPennetta &= "\"
			End If

			Dim PercorsoURLPennetta As String = ConfigurationManager.AppSettings("percorsoURLPennetta")
			If PercorsoURLPennetta.Substring(PercorsoURLPennetta.Length - 1) <> "/" Then
				PercorsoURLPennetta &= "/"
			End If

			'Response.Write(PercorsoPennetta)
			'Response.Write(PercorsoURLPennetta)
			'Response.End()

			For Each ff As String In nomeFile
				Dim fff As String = a & m2 & ff & ".jpg"
				Dim urlAnno As String = PercorsoURLPennetta & a & "/" + fff
				Dim urlYeah As String = PercorsoURLPennetta & "Yeah/" & fff
				Dim urlVolti As String = PercorsoURLPennetta & "Volti/" & fff

				Dim pathAnno As String = PercorsoPennetta & a & "\" + fff
				Dim pathYeah As String = PercorsoPennetta & "Yeah\" & fff
				Dim pathVolti As String = PercorsoPennetta & "Volti\" & fff
				Dim quale As String = ""

				If File.Exists(pathAnno) Then
					quale = urlAnno
				Else
					If File.Exists(pathYeah) Then
						quale = urlYeah
					Else
						If File.Exists(pathVolti) Then
							quale = urlVolti
						Else
							For ii As Integer = 1 To qf
								'Response.Write(filettis(ii) & "->" & fff)
								If filettis(ii) <> "" Then
									If filettis(ii).Contains(fff) Then
										'Response.Write("TROVATO: " & filettis(ii))
										'Response.End()
										quale = filettis(ii)
										quale = quale.Replace(PercorsoPennetta, PercorsoURLPennetta)
										quale = quale.Replace("\", "/")
										Exit For
									End If
								End If
							Next

							'If quale = "" Then
							'	Response.Write("NON TROVATO: " & fff)
							'	Response.End()
							'End If
						End If
					End If
				End If

				If quale <> "" Then
					filetti &= quale & ";"
					mX &= mmx.Item(posi) & ";"
					mY &= mmy.Item(posi) & ";"
					tipo &= tipologia.Item(posi) & ";"
				End If

				posi += 1
			Next

			Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
			sb2.Append("<script type='text/javascript' language='javascript'>")
			sb2.Append("     calcRoutePrinc();")
			sb2.Append("</script>")

			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCRPRINC", sb2.ToString(), False)

			Dim sb3 As System.Text.StringBuilder = New System.Text.StringBuilder()
			sb3.Append("<script type='text/javascript' language='javascript'>")
			sb3.Append("     aggiungeMultimedia('" & mX & "', '" & mY & "', '" & tipo & "', '" & filetti & "');")
			sb3.Append("</script>")

			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCRMARKERS", sb3.ToString(), False)
			Dim sdx As String = ""
			Dim sdy As String = ""
			'Dim skm As String = ""
			Dim sddh As String = ""

			For Each lx As String In dx
				sdx &= lx & ";"
			Next
			For Each ly As String In dy
				sdy &= ly & ";"
			Next
			'For Each lkm As String In km
			'	skm &= lkm & ";"
			'Next
			For Each ldh As String In ddh
				sddh &= ldh & ";"
			Next

			sb3 = New System.Text.StringBuilder()
			sb3.Append("<script type='text/javascript' language='javascript'>")
			sb3.Append("     AggiungeMarkerPrinc('" & sdx & "', '" & sdy & "', '" & sddh & "', '0');")
			sb3.Append("</script>")

			ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCRMPRINC", sb3.ToString(), False)

			'tvDati.Nodes.Clear()

			'Dim root = New TreeNode
			'root.Text = " Dettagli"
			'root.ImageUrl = "~/App_Themes/Standard/Images/dettagli.png"
			'tvDati.Nodes.Add(root)

			'RiempieNodi("Km. effettuati", KmEffettuati, RitornaIconaInBaseaTipologia(14))
			'RiempieNodi("Messaggi (" & Messaggi.Count & ")", Messaggi, RitornaIconaInBaseaTipologia(2))
			'RiempieNodi("Canzoni ascoltate (" & CanzoniAscoltate.Count & ")", CanzoniAscoltate, RitornaIconaInBaseaTipologia(3))
			'RiempieNodi("Cronologia (" & Cronologia.Count & ")", Cronologia, RitornaIconaInBaseaTipologia(4))
			'RiempieNodi("Immagini Scaricate (" & ImmaginiScaricate.Count & ")", ImmaginiScaricate, RitornaIconaInBaseaTipologia(5))
			'RiempieNodi("Log Activity (" & LogActivity.Count & ")", LogActivity, RitornaIconaInBaseaTipologia(6))
			'RiempieNodi("Immagini Locali (" & ImmaginiLocali.Count & ")", ImmaginiLocali, RitornaIconaInBaseaTipologia(7))
			'RiempieNodi("Partite Castelverde (" & PartiteCC.Count & ")", PartiteCC, RitornaIconaInBaseaTipologia(10))
			'RiempieNodi("Telefonate (" & Telefonate.Count & ")", Telefonate, RitornaIconaInBaseaTipologia(11))
			'RiempieNodi("Notifiche (" & Notifiche.Count & ")", Notifiche, RitornaIconaInBaseaTipologia(12))
			'RiempieNodi("Sistema (" & Sistema.Count & ")", Sistema, RitornaIconaInBaseaTipologia(13))

			'CreaTimeLine(OraMinima, OraMassima, Tutto)
		End If
	End Sub

	'Private indiceNodi As Integer = 0

	'Private Sub RiempieNodi(Cosa As String, Valori As ArrayList, Icona As String)
	'	If Valori.Count > 0 Then
	'		Dim tt As New TreeNode
	'		tt.Text = " " & Cosa
	'		tt.ImageUrl = Icona
	'		tvDati.Nodes(0).ChildNodes.Add(tt)

	'		Dim Progressivo As Integer = 0
	'		For Each c As String In Valori
	'			Dim cc() As String = c.Split(";")
	'			Dim t As New TreeNode
	'			Dim cccc As String = ""
	'			Dim primo As Boolean = True
	'			For Each ccc As String In cc
	'				If Not primo Then
	'					cccc &= ccc
	'				Else
	'					primo = False
	'				End If
	'			Next
	'			t.Text = cc(0) & ": " & cccc
	'			Progressivo += 1
	'			t.Value = indiceNodi & Progressivo
	'			If Not Cosa.Contains("Telefonate") Then
	'				t.ImageUrl = Icona
	'			Else
	'				If c.ToUpper.Contains("ENTRATA") Then
	'					t.ImageUrl = "App_Themes/Standard/Images/telefonateArr.png"
	'				Else
	'					t.ImageUrl = "App_Themes/Standard/Images/telefonateUsc.png"
	'				End If
	'			End If
	'			t.NavigateUrl = ""

	'			tvDati.Nodes(0).ChildNodes(indiceNodi).ChildNodes.Add(t)
	'		Next
	'		indiceNodi += 1
	'	End If
	'End Sub

	'Private Sub CreaTimeLine(OraMinima As String, OraMassima As String, Tutto As ArrayList)
	'	Dim s As New StringBuilder
	'	s.Append("<table>")
	'	s.Append("<tr>")

	'	Dim oraMinimaH As String = Mid(OraMinima, 1, 2)
	'	Dim oraMinimaM As String = Mid(OraMinima, 3, 2)

	'	Dim inizioH As Integer = Val(oraMinimaH)
	'	Dim inizioM As Integer = Val(oraMinimaM)
	'	Dim oldInizioH As Integer
	'	Dim oldInizioM As Integer

	'	Dim Ancora As Boolean = True
	'	Do While Ancora
	'		inizioM -= 1
	'		If inizioM / 5 = Int(inizioM / 5) Then
	'			Ancora = False
	'		End If
	'	Loop

	'	Dim oraMassimaH As Integer = Val(Mid(OraMassima, 1, 2))
	'	Dim oraMassiimaM As Integer = Val(Mid(OraMassima, 3, 2))

	'	Dim Passo As Integer = 10
	'	Dim Colonna As Integer = 0
	'	Dim Colonne() As ArrayList

	'	Ancora = True

	'	Dim Orella As String = Format(inizioH, "00") & ":" & Format(inizioM, "00")
	'	s.Append("<td style='height: 30px; border: 1px solid #777777; text-align: center; background-color: #d1a46d;'>&nbsp;<span style=""font-family: Arial; font-size: 12px; font-weight: bold;"">" & Orella & "</span>&nbsp;</td>")

	'	oldInizioH = inizioH
	'	oldInizioM = inizioM
	'	Colonna = 0
	'	ReDim Preserve Colonne(Colonna)
	'	Colonne(Colonna) = New ArrayList

	'	Do While Ancora
	'		inizioM += Passo

	'		If inizioM > 59 Then
	'			inizioM -= 60
	'			inizioH += 1
	'		End If

	'		Orella = Format(inizioH, "00") & ":" & Format(inizioM, "00")
	'		s.Append("<td style='height: 30px; border: 1px solid #777777; text-align: center; background-color: #d1a46d;'>&nbsp;<span style=""font-family: Arial; font-size: 12px; font-weight: bold;"">" & Orella & "</span>&nbsp;</td>")

	'		For Each st As String In Tutto
	'			Dim Campi() As String = st.Split(";")
	'			Dim ore() As String = Campi(0).Split(":")
	'			Dim oretta As Integer = Val(ore(0) & ore(1))
	'			If oretta >= Val(Format(oldInizioH, "00") & Format(oldInizioM, "00")) And oretta <= Val(Format(inizioH, "00") & Format(inizioM, "00")) Then
	'				Colonne(Colonna).Add(Campi(0) & ";" & Campi(1) & ";" & Campi(Campi.Length - 2) & ";")
	'			End If
	'		Next

	'		oldInizioH = inizioH
	'		oldInizioM = inizioM
	'		Colonna += 1
	'		ReDim Preserve Colonne(Colonna)
	'		Colonne(Colonna) = New ArrayList

	'		If inizioH > oraMassimaH Or (inizioH = oraMassimaH And inizioM > oraMassiimaM) Then
	'			Ancora = False
	'		End If
	'	Loop

	'	s.Append("</tr>")

	'	For i As Integer = 0 To Colonna ' - 2
	'		Dim Dati As String = ""

	'		Dati &= "<table>"
	'		Dim progr As Integer = 0
	'		For Each d As String In Colonne(i)
	'			If d.Trim <> "" Then
	'				Dim Campi2() As String = d.Split(";")
	'				Dim Colore As String = "style=""background-color: "
	'				progr += 1
	'				If progr / 2 = Int(progr / 2) Then
	'					Colore &= "#ffbc58;"""
	'				Else
	'					Colore &= "#f8ff58;"""
	'				End If
	'				Dati &= "<tr " & Colore & " title=""" & PrendeTipologia(Val(Campi2(2))) & ": " & Campi2(1).Replace("§", ";") & """>"
	'				Dati &= "<td style=""text-align: center; vertical-align: center;""><img alt="""" src=""" & RitornaIconaInBaseaTipologia(Val(Campi2(2))) & """ width=25 height=25></img></td>"
	'				Dim url As String = "#"
	'				Select Case Campi2(2)
	'					Case "4", "5"
	'						url = Campi2(1).Replace("§", ";")
	'						url = "onclick=""window.open('" & url & "', '_blank', 'location=yes,height=570,width=520,scrollbars=yes,status=yes');"""
	'					Case Else
	'						url = "href=""#"""
	'				End Select

	'				Dati &= "<td style=""text-align: right; vertical-align: center;"">&nbsp;"
	'				Dati &= "<a " & url & ">"
	'				Dati &= "<span style=""font-family: Arial; font-size: 12px;"">" & Mid(Campi2(0), 1, Campi2(0).IndexOf(".")) & "</span>"
	'				Dati &= "</a>&nbsp;"
	'				Dati &= "</td>"

	'				Dati &= "</tr>"
	'			End If
	'		Next
	'		Dati &= "</table>"

	'		s.Append("<td style='height: 30px; border: 1px solid #777777; vertical-align: top;'>" & Dati & "</td>")
	'	Next

	'	s.Append("</table>")

	'	divTimeline.InnerHtml = s.ToString
	'End Sub

	'Private Function PrendeTipologia(idTipologia As Integer) As String
	'	Select Case idTipologia
	'		Case 2
	'			Return "SMS"
	'		Case 3
	'			Return "Canzone ascoltata"
	'		Case 4
	'			Return "Cronologia"
	'		Case 5
	'			Return "Immagine scaricata"
	'		Case 6
	'			Return "Log Activity"
	'		Case 7
	'			Return "Locali"
	'		Case 10
	'			Return "Partita CC"
	'		Case 11
	'			Return "Telefonata"
	'		Case 12
	'			Return "Notifica"
	'		Case 13
	'			Return "Sistema"
	'		Case Else
	'			Return ""
	'	End Select
	'End Function

	'Private Function RitornaIconaInBaseaTipologia(idTipologia As Integer) As String
	'	Select Case idTipologia
	'		Case 2
	'			Return "App_Themes/Standard/Images/sms.png"
	'		Case 3
	'			Return "App_Themes/Standard/Images/mp3.png"
	'		Case 4
	'			Return "App_Themes/Standard/Images/cronologia.png"
	'		Case 5
	'			Return "App_Themes/Standard/Images/immscar.png"
	'		Case 6
	'			Return "App_Themes/Standard/Images/log.png"
	'		Case 7
	'			Return "App_Themes/Standard/Images/locali.png"
	'		Case 10
	'			Return "App_Themes/Standard/Images/partite.png"
	'		Case 11
	'			Return "App_Themes/Standard/Images/telefonate.png"
	'		Case 12
	'			Return "App_Themes/Standard/Images/notifica.png"
	'		Case 13
	'			Return "App_Themes/Standard/Images/Sistema.png"
	'		Case 14
	'			Return "App_Themes/Standard/Images/Km.png"
	'		Case Else
	'			Return ""
	'	End Select
	'End Function

	Protected Sub cmdChiudeDD_Click(sender As Object, e As EventArgs) Handles cmdChiudeDD.Click
		divDettaglioGiorno.Visible = False
		divBlocca.Visible = False
	End Sub

	Protected Sub imgStatSistema_Click(sender As Object, e As ImageClickEventArgs) Handles imgStatSistema.Click
		Dim Giorno As Integer = Val(hdnGiorno.Value) + 1
		Dim Mese As Integer = RitornaNumeroMese()

		Response.Redirect("StatisticheSistema.aspx?Giorno=" & Giorno & "&Mese=" & Mese & "&Anno=" & cmbAnno.Text)
	End Sub
End Class