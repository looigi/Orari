Imports System.Drawing
Imports System.Net

Public Class Mappa
    Inherits System.Web.UI.Page

    Public Class LatLon
        Public Property Latitude As Double
        Public Property Longitude As Double
        Public Sub New()
        End Sub
        Public Sub New(ByVal lat As Double, ByVal lon As Double)
            Me.Latitude = lat
            Me.Longitude = lon
        End Sub
    End Class

    Dim Markers() As String
    Dim QuantiMarkers As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente = 0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            Try
                MkDir(Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente & "-" & Utenza & "\Loghi\Resized")
            Catch

            End Try

            CaricaCombo()
            DisegnaMappa()
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
            cmbSocieta.Text = ""

            cmbCommessa.Items.Clear()

            ConnSQL.Close()
        End If
    End Sub

    Private Sub CaricaComboCommessa()
        If cmbSocieta.Text = "" Then
            cmbCommessa.Items.Clear()
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim idLavoro As String = ""

            Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                idLavoro = Rec("idLavoro").Value
            End If
            Rec.Close()

            Sql = "Select * From " & prefissotabelle & "Commesse Where idUtente=" & idUtente & " And Eliminato='N' And idLavoro=" & idLavoro & " Order By Descrizione"
            cmbCommessa.Items.Clear()
            cmbCommessa.Items.Add("")
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                cmbCommessa.Items.Add(Rec("Descrizione").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()
            cmbCommessa.Text = ""

            ConnSQL.Close()
        End If
    End Sub

    Protected Sub DisegnaMappaDiNuovo()
        If chkSocieta.Checked = False Then
            cmbSocieta.Visible = False
            cmbCommessa.Visible = False
        Else
            cmbSocieta.Visible = True
            If chkCommesse.Checked = False Then
                cmbCommessa.Text = ""
                cmbCommessa.Visible = False
            Else
                cmbCommessa.Visible = True
                CaricaComboCommessa()
            End If
        End If

        DisegnaMappa()
    End Sub

    Protected Sub DisegnaMappaDiNuovoPerSoc()
        cmbCommessa.Text = ""
        CaricaComboCommessa()

        DisegnaMappa()
    End Sub

    Protected Sub DisegnaMappaDiNuovoPerCom()
        DisegnaMappa()
    End Sub

    Private Sub DisegnaMappa()
        QuantiMarkers = 0
        Erase Markers

        If chkCommesse.Checked = True Then
            DisegnaMarkerIndirizziCommesse()
        End If
        If chkSocieta.Checked = True Then
            DisegnaMarkerIndirizziLavori()
        End If
        If chkStudi.Checked = True Then
            DisegnaMarkerIndirizziStudi()
        End If
        If chkAbitazioni.Checked = True Then
            DisegnaMarkerIndirizziCasa()
        End If

        Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
        sb2.Append("<script type='text/javascript' language='javascript'>")
        sb2.Append("     SistemaImmagine();")
        sb2.Append("</script>")

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb2.ToString(), False)

        ScriveMarkers()
    End Sub

    Private Sub ScriveMarkers()
        'Dim latlng As GLatLng
        'Dim markerOptions As GMarkerOptions
        'Dim Icon As GIcon
        'Dim Marker As GMarker
        Dim Campi() As String

        Dim dX As Double
        Dim dY As Double
        Dim ddX() As Double
        Dim ddY() As Double
        Dim Quanti As Integer = 0

        For i As Integer = 1 To QuantiMarkers
            Campi = Markers(i).Split(";")
            If Campi(0) <> "undefined" Then
                dX = Double.Parse(Campi(0).Replace(".", ","))
                If IsNumeric(Campi(1)) = True Then
                    dY = Double.Parse(Campi(1).Replace(".", ","))
                Else
                    Response.Write(Campi(1))
                    Response.End()
                End If
            End If


            Dim Ancora As Boolean = True

            While Ancora = True
                Ancora = False
                For k As Integer = 1 To Quanti
                    If dX = ddX(k) And dY = ddY(k) Then
                        Ancora = True

                        Randomize()
                        Dim x As Double = Int(Rnd(1) * 5) + 1
                        Dim y As Double = Int(Rnd(1) * 5) + 1
                        x /= 3500
                        y /= 3500
                        Dim z As Integer = Int(Rnd(1) * 4) + 1
                        Select Case z
                            Case 1
                                dX += x
                                dY += y
                            Case 2
                                dX -= x
                                dY -= y
                            Case 3
                                dX += x
                                dY -= y
                            Case 4
                                dX -= x
                                dY += y
                        End Select
                        Exit For
                    End If
                Next
            End While

            Quanti += 1
            ReDim Preserve ddX(Quanti)
            ReDim Preserve ddY(Quanti)
            ddX(Quanti) = dX
            ddY(Quanti) = dY

            'latlng = New GLatLng(dX, dY)
            'markerOptions = New GMarkerOptions()
            'markerOptions.clickable = True

            'latlng = New GLatLng(dX, dY)
            'markerOptions = New GMarkerOptions()
            'markerOptions.clickable = True

            'Icon = New GIcon()
            'Icon.image = Campi(2)
            'Icon.iconSize = New GSize(30, 30)

            'Marker = New GMarker(latlng, markerOptions)
            'markerOptions.icon = Icon
            'markerOptions.title = Mid(Campi(3), 1, 17) & "..."

            Dim sb2 As System.Text.StringBuilder = New System.Text.StringBuilder()
            sb2.Append("<script type='text/javascript' language='javascript'>")
            sb2.Append("     AggiungeMarker('" & dX.ToString.Replace(",", ".") & "', '" & dY.ToString.Replace(",", ".") & "', '" & Campi(2).Replace("'", " ") & "', '" & Campi(3).Replace("'", " ") & "', '" & Campi(4).Replace("'", " ") & "');")
            sb2.Append("</script>")

            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCRM" & i.ToString.Trim, sb2.ToString(), False)

            'GMap1.Add(Marker)
        Next
    End Sub

    Private Sub DisegnaMarkerIndirizziCasa()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim LL As String

            Dim Coord() As String

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & prefissotabelle & "Indirizzi Where idUtente=" & idUtente & " And Indirizzo Is Not Null"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                LL = Rec("LatLng").Value.ToString
                If LL.IndexOf(";") > -1 Then
                    LL = Mid(LL, InStr(LL, ";") + 1, LL.Length)
                End If
                Coord = LL.Split(",")

                If UBound(Coord) >= 1 Then
                    QuantiMarkers += 1
                    ReDim Preserve Markers(QuantiMarkers)

                    Dim dInizio As Date
                    Dim dFine As Date
                    Dim Spiegazioni As String = ""

                    If Rec("DataInizio").Value Is DBNull.Value = False AndAlso Rec("DataInizio").Value <> Date.MinValue Then
                        dInizio = Rec("DataInizio").Value
                        Spiegazioni += "Data inizio: " & MetteMaiuscole(dInizio.ToString("dddd")) & " " & dInizio.Day.ToString("00") & "/" & dInizio.Month.ToString("00") & "/" & dInizio.Year
                    End If
                    If Rec("DataFine").Value Is DBNull.Value = False AndAlso Rec("DataFine").Value <> Date.MinValue Then
                        dFine = Rec("DataFine").Value
                        Spiegazioni += "<br />Data fine: " & MetteMaiuscole(dFine.ToString("dddd")) & " " & dFine.Day.ToString("00") & "/" & dFine.Month.ToString("00") & "/" & dFine.Year
                    End If

                    Markers(QuantiMarkers) = Coord(0) & ";" & Coord(1) & ";App_Themes/Standard/Images/casa.png;ABITAZIONE: " & Rec("Indirizzo").Value & ";" & Spiegazioni & ";"
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Private Sub DisegnaMarkerIndirizziStudi()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim LL As String

            Dim Coord() As String

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & prefissotabelle & "Studi Where idUtente=" & idUtente & " And Indirizzo Is Not Null"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                LL = Rec("LatLng").Value.ToString
                If LL.IndexOf(";") > -1 Then
                    LL = Mid(LL, InStr(LL, ";") + 1, LL.Length)
                End If
                Coord = LL.Split(",")

                If UBound(Coord) >= 1 Then
                    QuantiMarkers += 1
                    ReDim Preserve Markers(QuantiMarkers)

                    Dim dInizio As Date
                    Dim dFine As Date
                    Dim Spiegazioni As String = ""

                    If Rec("DataInizio").Value Is DBNull.Value = False AndAlso Rec("DataInizio").Value <> Date.MinValue Then
                        dInizio = Rec("DataInizio").Value
                        Spiegazioni += "Data inizio: " & MetteMaiuscole(dInizio.ToString("dddd")) & " " & dInizio.Day.ToString("00") & "/" & dInizio.Month.ToString("00") & "/" & dInizio.Year
                    End If
                    If Rec("DataFine").Value Is DBNull.Value = False AndAlso Rec("DataFine").Value <> Date.MinValue Then
                        dFine = Rec("DataFine").Value
                        Spiegazioni += "<br />Data fine: " & MetteMaiuscole(dFine.ToString("dddd")) & " " & dFine.Day.ToString("00") & "/" & dFine.Month.ToString("00") & "/" & dFine.Year
                    End If

                    Markers(QuantiMarkers) = Coord(0) & ";" & Coord(1) & ";App_Themes/Standard/Images/" & idutente.tostring.trim & "-" & utenza & "/scuolapicc.png;SCUOLA: " & Rec("Scuola").Value & " - " & Rec("Indirizzo").Value & ";" & Spiegazioni & ";"
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Private Sub DisegnaMarkerIndirizziLavori()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Coord() As String
            Dim NomeIcona As String
            Dim LL As String

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Altro = "And Lavoro='" & cmbSocieta.Text.Replace("'", "''") & "' "
            End If

            Sql = "Select * From " & prefissotabelle & "Lavori Where " & _
                "idUtente=" & idUtente & " " & _
                " " & Altro & " " & _
                "And Indirizzo Is Not Null And Eliminato='N'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                LL = Rec("LatLng").Value.ToString
                If LL.IndexOf(";") > -1 Then
                    LL = Mid(LL, InStr(LL, ";") + 1, LL.Length)
                End If
                Coord = LL.Split(",")

                If UBound(Coord) >= 1 Then
                    NomeIcona = PrendeIcona("App_Themes/Standard/Images/" & idUtente.ToString.Trim & "-" & Utenza & "/Loghi/Resized/Mappa/" & Rec("Lavoro").Value & ".png")

                    QuantiMarkers += 1
                    ReDim Preserve Markers(QuantiMarkers)

                    Dim dInizio As Date
                    Dim dFine As Date
                    Dim Spiegazioni As String = ""

                    If Rec("DataInizio").Value Is DBNull.Value = False AndAlso Rec("DataInizio").Value <> Date.MinValue Then
                        dInizio = Rec("DataInizio").Value
                        Spiegazioni += "Data inizio: " & MetteMaiuscole(dInizio.ToString("dddd")) & " " & dInizio.Day.ToString("00") & "/" & dInizio.Month.ToString("00") & "/" & dInizio.Year
                    End If
                    If Rec("DataFine").Value Is DBNull.Value = False AndAlso Rec("DataFine").Value <> Date.MinValue Then
                        dFine = Rec("DataFine").Value
                        Spiegazioni += "<br />Data fine: " & MetteMaiuscole(dFine.ToString("dddd")) & " " & dFine.Day.ToString("00") & "/" & dFine.Month.ToString("00") & "/" & dFine.Year
                    End If

                    Markers(QuantiMarkers) = Coord(0) & ";" & Coord(1) & ";" & NomeIcona & ";SEDE: " & Rec("Lavoro").Value & " - " & Rec("Indirizzo").Value & ";" & Spiegazioni & ";"
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Private Sub DisegnaMarkerIndirizziCommesse()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim Coord() As String
            Dim NomeIcona As String
            Dim LL As String

            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Dim Altro As String = ""

            If cmbSocieta.Text <> "" Then
                Dim idLavoro As String = ""

                Sql = "Select * From " & prefissotabelle & "Lavori Where idUtente=" & idUtente & " And Lavoro='" & cmbSocieta.Text & "'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    idLavoro = Rec("idLavoro").Value
                End If
                Rec.Close()

                Altro = "And A.idLavoro='" & idLavoro & "' "
            End If
            If cmbCommessa.Text <> "" Then
                Altro += "And Descrizione='" & cmbCommessa.Text.Replace("'", "''") & "' "
            End If

            Sql = "Select A.Descrizione, A.Indirizzo, A.LatLng, B.Lavoro, A.DataInizio, A.DataFine From " & prefissotabelle & "Commesse A " & _
                "Left Join " & prefissotabelle & "Lavori B On A.idUtente=B.idUtente And A.idLavoro=B.idLavoro Where " & _
                "A.idUtente=" & idUtente & " " & _
                " " & Altro & " " & _
                "And A.Indirizzo Is Not Null And A.Eliminato='N'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                LL = Rec("LatLng").Value.ToString
                If LL.IndexOf(";") > -1 Then
                    LL = Mid(LL, InStr(LL, ";") + 1, LL.Length)
                End If
                Coord = LL.Split(",")

                If UBound(Coord) >= 1 Then
                    NomeIcona = PrendeIcona("App_Themes/Standard/Images/" & idUtente.ToString.Trim & "-" & Utenza & "/Loghi/Resized/Mappa/" & Rec("Descrizione").Value & ".png")

                    QuantiMarkers += 1
                    ReDim Preserve Markers(QuantiMarkers)

                    Dim Lavoro As String = Rec("Lavoro").Value.ToString.Trim
                    Dim Commessa As String = Rec("Descrizione").Value.ToString.Trim
                    Dim Indirizzo As String = Rec("Indirizzo").Value.ToString.Trim
                    Dim Stringona As String = "COMMESSA " & Lavoro & " - " & Commessa & " " & Indirizzo

                    Dim dInizio As Date
                    Dim dFine As Date
                    Dim Spiegazioni As String = ""

                    If Rec("DataInizio").Value Is DBNull.Value = False AndAlso Rec("DataInizio").Value <> "" Then
                        dInizio = Rec("DataInizio").Value
                        Spiegazioni += "Data inizio: " & MetteMaiuscole(dInizio.ToString("dddd")) & " " & dInizio.Day.ToString("00") & "/" & dInizio.Month.ToString("00") & "/" & dInizio.Year
                    End If
                    If Rec("DataFine").Value Is DBNull.Value = False AndAlso Rec("DataFine").Value <> "" Then
                        dFine = Rec("DataFine").Value
                        Spiegazioni += "<br />Data fine: " & MetteMaiuscole(dFine.ToString("dddd")) & " " & dFine.Day.ToString("00") & "/" & dFine.Month.ToString("00") & "/" & dFine.Year
                    End If

                    Markers(QuantiMarkers) = Coord(0) & ";" & Coord(1) & ";" & NomeIcona & ";" & Stringona & ";" & Spiegazioni & ";"
                End If

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Private Function PrendeIcona(Icona As String) As String
        Dim Ritorno As String
        Dim Appoggio As String = Icona.Replace("/", "\")

        Appoggio = Server.MapPath(".") & "\" & Appoggio
        If Dir(Appoggio) = "" Then
            Appoggio = Icona.Replace("/", "\")
            Appoggio = Appoggio.Replace("Resized\Mappa\", "")
            Appoggio = Server.MapPath(".") & "\" & Appoggio '.Replace(".png", ".jpg")

            If Dir(Appoggio) = "" Then
                Ritorno = "/App_Themes/Standard/Images/" & idUtente.ToString.Trim & "-" & Utenza & "/Loghi/Resized/Mappa/societa.png"
                Appoggio = Server.MapPath(".") & Ritorno.Replace("/", "\")
                Dim gf As New GestioneFilesDirectory
                Dim direct As String = gf.TornaNomeDirectoryDaPath(Appoggio) & "\"
                gf.CreaDirectoryDaPercorso(direct)
                gf = Nothing
                If Dir(Appoggio) = "" Then
                    ResizeImage(Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Loghi\societa.png", 50, 50, Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Loghi\Resized\Mappa\societa.png")
                End If
            Else
                Dim Appoggio2 As String
                Appoggio2 = Icona.Replace("/", "\")
                Appoggio2 = Server.MapPath(".") & "\" & Appoggio2 '.Replace(".png", ".jpg")

                If Dir(Appoggio2) = "" Then
                    ResizeImage(Appoggio, 50, 50, Appoggio2)
                    ArrotondaIcona(Appoggio2)

                    Ritorno = Appoggio2
                Else
                    Ritorno = Appoggio2
                End If
            End If
        Else
            Ritorno = Icona
        End If

        Ritorno = Ritorno.Replace(" ", "%20")

        Return Ritorno
    End Function

    Private Function GetLatLon(ByVal addr As String) As LatLon
        Dim url As String = "http://maps.google.com/maps/geo?output=csv&key=AIzaSyCJ6LqMv1zV5Z_-wrETyen4ltCfMubiCzI&q=" & addr
        Dim request As System.Net.WebRequest = WebRequest.Create(url)
        Dim response As HttpWebResponse = request.GetResponse()
        If response.StatusCode = HttpStatusCode.OK Then
            Dim ms As New System.IO.MemoryStream()
            Dim responseStream As System.IO.Stream = response.GetResponseStream()
            Dim buffer(2048) As Byte
            Dim count As Integer = responseStream.Read(buffer, 0, buffer.Length)
            While count > 0
                ms.Write(buffer, 0, count)
                count = responseStream.Read(buffer, 0, buffer.Length)
            End While
            responseStream.Close()
            ms.Close()
            Dim responseBytes() As Byte = ms.ToArray()
            Dim encoding As New System.Text.ASCIIEncoding()
            Dim coords As String = encoding.GetString(responseBytes)
            Dim parts() As String = coords.Split(",")
            Return New LatLon(Convert.ToDouble(parts(2)), Convert.ToDouble(parts(3)))
        End If
        Return Nothing
    End Function

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        Response.Redirect("Principale.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    'Protected Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click

    '    Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
    '    sb.Append("<script type='text/javascript' language='javascript'>")
    '    sb.Append("     calcRoute();")
    '    sb.Append("</script>")

    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "JSCR", sb.ToString(), False)

    'End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub
End Class