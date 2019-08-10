Imports System.IO

Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack = False Then
            NomeMesi(1) = "Gennaio"
            NomeMesi(2) = "Febbraio"
            NomeMesi(3) = "Marzo"
            NomeMesi(4) = "Aprile"
            NomeMesi(5) = "Maggio"
            NomeMesi(6) = "Giugno"
            NomeMesi(7) = "Luglio"
            NomeMesi(8) = "Agosto"
            NomeMesi(9) = "Settembre"
            NomeMesi(10) = "Ottobre"
            NomeMesi(11) = "Novembre"
            NomeMesi(12) = "Dicembre"

            Password = ""
            UtenzaEntrata = ""

            Dim myCookie As HttpCookie = Request.Cookies("myCookie")

            If (myCookie Is Nothing) Then
            Else
                idUtente = myCookie.Values("idUtente").ToString
                Utenza = myCookie.Values("Utenza").ToString

                Try
                    System.IO.Directory.CreateDirectory(Server.MapPath(".") & "\Opzioni")
                Catch ex As Exception

                End Try
                NomeFileMese = Server.MapPath(".") & "\Opzioni\VisMese-" & Utenza.ToString.Trim & ".txt"

                Dim g As New GestioneFilesDirectory
                If System.IO.File.Exists(NomeFileMese) = False Then
                    g.CreaAggiornaFileVisMese(NomeFileMese, "Chiuso")
                    MeseVisualizzato = False
                Else
                    If g.LeggeFile(NomeFileMese) = "Aperto" Then
                        MeseVisualizzato = True
                    Else
                        MeseVisualizzato = False
                    End If
                End If
                g = Nothing

                txtUtenza.Text = Utenza
                If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
                    Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
                    Dim Rec As Object = CreateObject("ADODB.Recordset")
                    Dim Sql As String

                    Sql = "Select * From " & prefissotabelle & "Utenti Where Utente='" & txtUtenza.Text.Replace("'", "''") & "'"
                    Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
                    If Rec.Eof = False Then
                        txtPassword.Text = Rec("Password").Value
                        txtPassword.Attributes.Add("value", Rec("Password").Value)
                        Password = Rec("Password").Value
                        UtenzaEntrata = Utenza
                    Else
                        txtPassword.Text = ""
                        Password = ""
                        UtenzaEntrata = ""
                    End If
                    Rec.Close()

                    ConnSQL.Close()

                    If Request.QueryString("Entra") <> "" Then
                        Response.Redirect("Principale.aspx")
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub cmdInvia_Click(sender As Object, e As EventArgs) Handles cmdInvia.Click
        If txtUtenza.Text <> UtenzaEntrata And UtenzaEntrata <> "" Then
            Password = ""
        End If
        If Password <> "" Then
            txtPassword.Text = Password
        End If

        If txtUtenza.Text = "" Or txtPassword.Text = "" Then
            VisualizzaMessaggioInPopup("Utenza o password non valida", Master)
            Exit Sub
        End If

        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String
            Dim Ok As Boolean = True

            idUtente = -1
            Utenza = ""

            Sql = "Select * From " & prefissotabelle & "Utenti Where Utente='" & txtUtenza.Text.Replace("'", "''") & "'"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                Ok = False
            Else
                If txtPassword.Text <> Rec("Password").Value Then
                    Ok = False
                Else
                    idUtente = Rec("idUtente").Value
                    Utenza = Rec("Utente").Value
                End If
            End If
            Rec.Close()

            ConnSQL.Close()

            If Ok = True And idUtente <> -1 Then
                Dim Eliminati As Boolean = False

                Dim Percorso As String = Server.MapPath(".") & "\App_Themes\Standard\Images\" & idUtente.ToString.Trim & "-" & Utenza & "\Giorni\"
                Dim GD As GestioneFilesDirectory = New GestioneFilesDirectory
                GD.CreaDirectoryDaPercorso(Percorso)
                GD.PrendeRoot(Server.MapPath("."))
                GD.ScansionaDirectoryVecchia(Percorso, "ELIMINA")
                Eliminati = GD.RitornaEliminati
                GD = Nothing

                If chkRicordami.Checked = True Then

                    If (Not Request.Cookies("myCookie") Is Nothing) Then
                        Dim currentUserCookie As HttpCookie = HttpContext.Current.Request.Cookies("myCookie")
                        HttpContext.Current.Response.Cookies.Remove("myCookie")
                        currentUserCookie.Expires = DateTime.Now.AddDays(-10)
                        currentUserCookie.Value = Nothing
                        currentUserCookie.Values("idUtente") = ""
                        currentUserCookie.Values("Utenza") = ""
                        HttpContext.Current.Response.SetCookie(currentUserCookie)
                    End If

                    Dim myCookie As HttpCookie = Request.Cookies("myCookie")

                    If (myCookie Is Nothing) Then
                        myCookie = New HttpCookie("myCookie")
                        myCookie.Values.Add("idUtente", idUtente.ToString().Trim)
                        myCookie.Values.Add("Utenza", Utenza.Trim)
                        myCookie.Expires = DateTime.Now.AddDays(7)
                        Response.Cookies.Add(myCookie)
                    Else
                        myCookie = New HttpCookie("myCookie")
                        If myCookie.Values("idUtente") = "" Then
                            myCookie.Values.Add("idUtente", idUtente.ToString().Trim)
                            myCookie.Values.Add("Utenza", Utenza.Trim)
                            myCookie.Expires = DateTime.Now.AddDays(7)
                            Response.Cookies.Add(myCookie)
                        End If
                    End If
                Else
                    If (Not Request.Cookies("myCookie") Is Nothing) Then
                        Dim currentUserCookie As HttpCookie = HttpContext.Current.Request.Cookies("myCookie")
                        HttpContext.Current.Response.Cookies.Remove("myCookie")
                        currentUserCookie.Expires = DateTime.Now.AddDays(-10)
                        currentUserCookie.Value = Nothing
                        currentUserCookie.Values("idUtente") = ""
                        currentUserCookie.Values("Utenza") = ""
                        HttpContext.Current.Response.SetCookie(currentUserCookie)
                    End If
                End If
                Password = ""

                If Eliminati = True Then
                    Response.Redirect("Default.aspx?Entra=1")
                Else
                    Response.Redirect("Principale.aspx")
                End If
            Else
                VisualizzaMessaggioInPopup("Utenza o password non valida", Master)
            End If
        End If
    End Sub

End Class