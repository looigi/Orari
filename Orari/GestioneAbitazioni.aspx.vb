Imports System.IO

Public Class GestioneAbitazioni
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            txtAbitazione.Text = ""
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

            cmbAbitazioni.Items.Clear()

            Sql = "Select * From " & prefissotabelle & "Indirizzi Where idUtente=" & idUtente & " Order By Indirizzo"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            cmbAbitazioni.Items.Add("")
            Do Until Rec.Eof
                cmbAbitazioni.Items.Add(Rec("Indirizzo").Value)

                Rec.MoveNext()
            Loop
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Protected Sub SelezionaAbitazione()
        If cmbAbitazioni.Text = "" Then
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & prefissotabelle & "Indirizzi Where idUtente=" & idUtente & " And Indirizzo='" & cmbAbitazioni.Text.Replace("'", "''") & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                txtAbitazione.Text = "" & Rec("Indirizzo").Value
                '                txtKM.Text =  "" & Rec("KM").Value
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
        If txtAbitazione.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire il nome della società", Master)
            Exit Sub
        End If
        'If txtKM.Text = "" Then
        '    VisualizzaMessaggioInPopup("Inserire i KM", Master)
        '    Exit Sub
        'End If
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
        If txtLatLng.Text = "" And txtAbitazione.Text <> "" Then
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

            If cmbAbitazioni.Text <> "" Then
                Sql = "Select * From " & prefissotabelle & "Indirizzi Where idUtente=" & idUtente & " And Indirizzo='" & cmbAbitazioni.Text.Replace("'", "''") & "'"
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec.Eof = False Then
                    Contatore = Rec("Contatore").Value
                End If
                Rec.Close()

                If Contatore <> -1 Then
                    Sql = "Update " & prefissotabelle & "Indirizzi Set " & _
                        "Indirizzo='" & MetteMaiuscole(txtAbitazione.Text.Replace("'", "''")) & "', " & _
                        "KM='0', " & _
                        "DataInizio='" & sInizio & "', " & _
                        "DataFine='" & sFine & "', " & _
                        "LatLng='" & txtLatLng.Text & "' " & _
                        "Where idUtente=" & idUtente & " And Contatore=" & Contatore
                    EsegueSql(ConnSQL, Sql, ConnessioneSQL)
                End If
            Else
                Sql = "Select Max(Contatore)+1 From " & prefissotabelle & "Indirizzi Where idUtente=" & idUtente
                Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                If Rec(0).Value Is DBNull.Value = True Then
                    Contatore = 1
                Else
                    Contatore = Rec(0).Value
                End If
                Rec.Close()

                Sql = "Insert Into " & prefissotabelle & "Indirizzi Values (" & _
                    " " & idUtente & ", " & _
                    " " & Contatore & ", " & _
                    "'" & sInizio & "', " & _
                    "'" & sFine & "', " & _
                    "'" & MetteMaiuscole(txtAbitazione.Text.Replace("'", "''")) & "', " & _
                    "'0', " & _
                    "'" & txtLatLng.Text & "'" & _
                    ")"
                EsegueSql(ConnSQL, Sql, ConnessioneSQL)
            End If

            cmbAbitazioni.Text = ""
            txtAbitazione.Text = ""
            'txtKM.Text = ""
            txtDataInizio.Text = ""
            txtDataFine.Text = ""
            txtLatLng.Text = ""

            CaricaCombo()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Protected Sub imgNuova_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuova.Click
        cmbAbitazioni.Text = ""
        txtAbitazione.Text = ""
        'txtKM.Text = ""
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