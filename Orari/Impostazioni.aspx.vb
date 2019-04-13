Public Class Impostazioni
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            CaricaValori()
        End If
    End Sub

    Private Sub CaricaValori()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String

            Sql = "Select * From " & prefissotabelle & "Utenti Where idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = False Then
                txtPassword.Text = Rec("Password").Value
                txtPassword.Attributes.Add("value", Rec("Password").Value)
                txtPassword2.Text = Rec("Password").Value
                txtPassword2.Attributes.Add("value", Rec("Password").Value)
            Else
                txtPassword.Text = ""
                txtPassword2.Text = ""
            End If
            Rec.Close()

            txtNome.Text = ""
            txtCognome.Text = ""
            txtEMail.Text = ""
            txtMatricola.Text = ""
            txtOreStd.Text = ""
            txtDataNasc.Text = ""
            txtTelefono.Text = ""
            txtMotto.Text = ""

            Sql = "Select Ore, Nome, Cognome, EMail, Matricola, DataNascita, Telefono, Motto From " & PrefissoTabelle & "Impostazioni Where idUtente=" & idUtente
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                txtOreStd.Text = "8"
            Else
                If Rec(0).Value Is DBNull.Value = False Then
                    txtOreStd.Text = Rec(0).Value
                Else
                    txtOreStd.Text = "8"
                End If
                txtNome.Text = MetteMaiuscole("" & Rec("Nome").Value)
                txtCognome.Text = MetteMaiuscole("" & Rec("Cognome").Value)
                txtEMail.Text = MetteMaiuscole("" & Rec("EMail").Value)
                txtMatricola.Text = MetteMaiuscole("" & Rec("Matricola").Value)
                txtDataNasc.Text = MetteMaiuscole("" & Rec("DataNascita").Value)
                txtTelefono.Text = "" & Rec("Telefono").Value
                txtMotto.Text = "" & Rec("Motto").Value
            End If
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        Response.Redirect("Principale.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub imgSalva_Click(sender As Object, e As ImageClickEventArgs) Handles imgSalva.Click
        If txtMatricola.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire la matricola dell'utente", Master)
            Exit Sub
        End If
        If txtNome.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire il nome dell'utente", Master)
            Exit Sub
        End If
        If txtCognome.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire il cognome dell'utente", Master)
            Exit Sub
        End If
        If txtPassword.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire una password", Master)
            Exit Sub
        End If
        If txtPassword2.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire la password di controllo", Master)
            Exit Sub
        End If
        If txtPassword.Text <> txtPassword2.Text Then
            VisualizzaMessaggioInPopup("Le due password immesse non coincidono", Master)
            Exit Sub
        End If
        If txtOreStd.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire le ore standard", Master)
            Exit Sub
        Else
            If IsNumeric(txtOreStd.Text) = False Then
                VisualizzaMessaggioInPopup("Ore standard non valide", Master)
                Exit Sub
            Else
                If Val(txtOreStd.Text) < 0 Or Val(txtOreStd.Text) > 15 Then
                    VisualizzaMessaggioInPopup("Ore standard non valide", Master)
                    Exit Sub
                End If
            End If
        End If
        If txtDataNasc.Text <> "" Then
            If IsDate(txtDataNasc.Text) = False Then
                VisualizzaMessaggioInPopup("Data di nascita non valida", Master)
                Exit Sub
            End If
        End If
        If txtTelefono.Text = "" Then
            VisualizzaMessaggioInPopup("Inserire il telefono", Master)
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Datella As Date = txtDataNasc.Text
            Dim sData As String = Datella.Year & "-" & Datella.Month & "-" & Datella.Day & " 00:00:00.000"

            Sql = "Update " & prefissotabelle & "Utenti Set " & _
                "Password='" & txtPassword.Text.Replace("'", "''") & "' " & _
                "Where idUtente=" & idUtente
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Update " & PrefissoTabelle & "Impostazioni Set " &
                "Nome='" & MetteMaiuscole(txtNome.Text.Replace("'", "''")) & "', " &
                "Cognome='" & MetteMaiuscole(txtCognome.Text.Replace("'", "''")) & "', " &
                "EMail='" & txtEMail.Text.Replace("'", "''") & "', " &
                "Ore=" & txtOreStd.Text.Replace(",", ".") & ", " &
                "DataNascita='" & sData & "', " &
                "Motto='" & txtMotto.Text.Replace("'", "''") & "', " &
                "Telefono='" & txtTelefono.Text.Replace("'", "''") & "' " &
                "Where idUtente=" & idUtente
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            OreStandard = Val(txtOreStd.Text)

            CaricaValori()

            ConnSQL.Close()

            VisualizzaMessaggioInPopup("Dati salvati", Master)
        End If
    End Sub

    Protected Sub imgCaricaFoto_Click(sender As Object, e As ImageClickEventArgs) Handles imgCaricaFoto.Click
        If FileUpload1.HasFile = False Then
            VisualizzaMessaggioInPopup("Selezionare prima una foto", Master)
        Else
            Dim sPath As String = Server.MapPath(".") & "\App_Themes\Standard\images\" & idUtente.ToString.Trim & "-" & Utenza & "\"
            Dim Nome As String = "Immagine.png"

            Try
                Kill(Nome)
            Catch ex As Exception

            End Try

            FileUpload1.SaveAs(sPath & Nome)
            FileCopy(sPath & Nome, sPath & "Orig_" & Nome)

            RuotaFoto(sPath & Nome)
        End If
    End Sub
End Class