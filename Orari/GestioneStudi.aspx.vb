Imports System.IO

Public Class GestioneStudi
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            txtScuola.Text = ""
            'txtKM.Text = ""
            txtDataInizio.Text = ""
            txtDataFine.Text = ""
            txtLatLng.Text = ""

            CaricaCombo()
        End If
    End Sub

    Private Sub CaricaCombo()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            cmbScuole.Items.Clear()

            Sql = "Select * From " & prefissotabelle & "Studi Where idUtente=" & idUtente & " Order By Scuola"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbScuole.Items.Add("")
            Do Until Rec.Eof
                cmbScuole.Items.Add(Rec("Scuola").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Protected Sub SelezionaAbitazione()
        If cmbScuole.Text = "" Then
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & prefissotabelle & "Studi Where idUtente=" & idUtente & " And Scuola='" & cmbScuole.Text.Replace("'", "''") & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                txtScuola.Text = "" & Rec("Scuola").Value
                txtIndirizzo.Text = "" & Rec("Indirizzo").Value
                txtDataInizio.Text = "" & Rec("DataInizio").Value
                txtDataFine.Text = "" & Rec("DataFine").Value
                txtLatLng.Text = "" & Rec("LatLng").Value

                CalcolaDistanze()
            End If
            Rec.Close()
        End If
    End Sub

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        If Request.QueryString("Maschera") = "GestTabelle" Then
            Response.Redirect("GestioneTabelle.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
        Else
            Response.Redirect("ModificaGiorno.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
        End If
    End Sub

    Protected Sub imgSalva_Click(sender As Object, e As ImageClickEventArgs) Handles imgSalva.Click
        If txtScuola.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire il nome della scuola", Master)
            Exit Sub
        End If
        If txtIndirizzo.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire l'indirizzo della scuola", Master)
            Exit Sub
        End If
        If txtDataInizio.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire la data di inizio", Master)
            Exit Sub
        Else
            If IsDate(txtDataInizio.Text) = False Then
                VisualizzaMessaggioInPopup("Data di inizio non valida", Master)
                Exit Sub
            End If
        End If
        If txtDataFine.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire la data di fine", Master)
            Exit Sub
        Else
            If IsDate(txtDataInizio.Text) = False Then
                VisualizzaMessaggioInPopup("Data di fine non valida", Master)
                Exit Sub
            End If
        End If
        If txtLatLng.Text = "" And txtScuola.Text <> "" Then
            VisualizzaMessaggioInPopup("Inserire latlng tramite l'apposito tasto", Master)
            Exit Sub
        End If

        Dim DInizio As Date = txtDataInizio.Text
        Dim DFine As Date = txtDataFine.Text
        Dim sInizio As String = DInizio.Year & "/" & DInizio.Month & "/" & DInizio.Day & " 00:00:00"
        Dim sFine As String = DFine.Year & "/" & DFine.Month & "/" & DFine.Day & " 00:00:00"

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Contatore As Integer = -1

            If cmbScuole.Text <> "" Then
                Sql = "Select * From " & prefissotabelle & "Studi Where idUtente=" & idUtente & " And Scuola='" & cmbScuole.Text.Replace("'", "''") & "'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    Contatore = Rec("Contatore").Value
                End If
                Rec.Close()

                If Contatore <> -1 Then
                    Sql = "Update " & prefissotabelle & "Studi Set " & _
                        "Scuola='" & MetteMaiuscole(txtScuola.Text.Replace("'", "''")) & "', " & _
                        "Indirizzo='" & MetteMaiuscole(txtIndirizzo.Text.Replace("'", "''")) & "', " & _
                        "KM='0', " & _
                        "DataInizio='" & sInizio & "', " & _
                        "DataFine='" & sFine & "', " & _
                        "LatLng='" & txtLatLng.Text & "' " & _
                        "Where idUtente=" & idUtente & " And Contatore=" & Contatore
                    EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                End If
            Else
                Sql = "Select Max(Contatore)+1 From " & prefissotabelle & "Studi Where idUtente=" & idUtente
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).Value Is DBNull.Value = True Then
                    Contatore = 1
                Else
                    Contatore = Rec(0).Value
                End If
                Rec.Close()

                Sql = "Insert Into " & prefissotabelle & "Studi Values (" & _
                    " " & idUtente & ", " & _
                    " " & Contatore & ", " & _
                    "'" & sInizio & "', " & _
                    "'" & sFine & "', " & _
                    "'" & MetteMaiuscole(txtScuola.Text.Replace("'", "''")) & "', " & _
                    "'" & MetteMaiuscole(txtIndirizzo.Text.Replace("'", "''")) & "', " & _
                    "'0', " & _
                    "'" & txtLatLng.Text & "'" & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            cmbScuole.Text = ""
            txtScuola.Text = ""
            txtIndirizzo.Text = ""
            txtDataInizio.Text = ""
            txtDataFine.Text = ""
            txtLatLng.Text = ""

            CaricaCombo()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Protected Sub imgNuova_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuova.Click
        cmbScuole.Text = ""
        txtScuola.Text = ""
        txtIndirizzo.Text = ""
        txtDataInizio.Text = ""
        txtDataFine.Text = ""
        txtLatLng.Text = ""
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