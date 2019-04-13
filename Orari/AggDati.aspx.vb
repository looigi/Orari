Imports System.IO

Public Class AggDati
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente=0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If
    End Sub

    Protected Sub imgCarica_Click(sender As Object, e As ImageClickEventArgs) Handles imgCarica.Click
        If FileUpload1.HasFile <> False Then
            Dim NomeFile As String = FileUpload1.FileName

            If NomeFile.ToUpper.IndexOf(".MDB") > -1 Then
                Dim PercorsoDest As String = Server.MapPath(".") & "\App_Data\DB"

                Try
                    Kill(PercorsoDest & "\" & "Passaggio.mdb")
                Catch ex As Exception

                End Try

                Try
                    FileUpload1.SaveAs(PercorsoDest & "\" & "Passaggio.mdb")

                    FileUpload1.Visible = False
                    imgCarica.Visible = False
                    imgEsegue.Visible = True
                Catch ex As Exception
                    ' Errore
                End Try
            Else
                ' Formato file non valido
            End If
        Else
            ' Nessun file selezionato
        End If
    End Sub

    Private Function CaricaNomiTabelle() As String()
        Dim Tabelle(20) As String

        Tabelle(0) = "AltreInfoMezzi"
        Tabelle(1) = "AltreInfoMezziRit"
        Tabelle(2) = "AltreInfoPasticca"
        Tabelle(3) = "AltreInfoTempo"
        Tabelle(4) = "CommessaDefault"
        Tabelle(5) = "DatiFerie"
        Tabelle(6) = "Impostazioni"
        Tabelle(7) = "Mezzi"
        Tabelle(8) = "MezziStandard"
        Tabelle(9) = "MezziStandardRit"
        Tabelle(10) = "Orari"
        Tabelle(11) = "Pasticche"
        Tabelle(12) = "Portate"
        Tabelle(13) = "Pranzi2"
        Tabelle(14) = "Tempo"
        'Tabelle(5) = "Commesse"
        'Tabelle(8) = "Indirizzi"
        'Tabelle(9) = "Lavori"
        'Tabelle(10) = "LavoroDefault"

        Return Tabelle
    End Function

    Protected Sub imgEsegue_Click(sender As Object, e As ImageClickEventArgs) Handles imgEsegue.Click
        If LeggeImpostazioniDiBase(Server.MapPath("."), "MDB") = True Then
            If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL") = True Then
                Dim ConnMDB As Object = ApreDB(ConnessioneMDB)
                Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
                Dim NomiTabelle() As String = CaricaNomiTabelle()
                Dim Errore As String
                Dim Rec As Object = CreateObject("ADODB.Recordset")

                For i As Integer = 0 To UBound(NomiTabelle) - 1
                    If NomiTabelle(i) <> "" Then
                        Errore = AggiornaTabella(ConnMDB, ConnSQL, Rec, NomiTabelle(i))
                        If Errore.IndexOf("ERRORE") = -1 Then
                            Response.Write(Errore & "<br />")
                        Else
                            ' Errore nell'aggiornamento della tabella
                            Response.Write(Errore & "<br />")
                        End If
                    End If
                Next

                ConnSQL.Close()
                ConnMDB.Close()
            Else
                ' Problemi nella lettura delle impostazioni del DB SQL
            End If
        Else
            ' Problemi nella lettura delle impostazioni del DB MDB
        End If
    End Sub

    Private Function RitornaFormatoStringa(Cosa As Object) As String
        Dim Ritorno As String

        If Cosa Is DBNull.Value = True Then
            Ritorno = "null"
        Else
            Ritorno = Cosa

            Dim TipoCampo As String = Cosa.GetType.Name

            Select Case TipoCampo.ToUpper.Trim
                Case "INT16", "INT32", "SINGLE"
                    Ritorno = Ritorno.Replace(",", ".")
                Case "DATETIME"
                    Dim Datella As Date = Ritorno

                    Ritorno = "'" & Datella.Year & "-" & Datella.Month & "-" & Datella.Day & " " & Datella.Hour & ":" & Datella.Minute & ":" & Datella.Second & "'"
                Case "STRING"
                    Ritorno = "'" & Ritorno.Replace("'", "''") & "'"
                Case Else
                    Ritorno = "'" & Ritorno.Replace("'", "''") & "'"
            End Select

            If Ritorno = "''" Then
                Ritorno = "null"
            End If
        End If

        Return Ritorno
    End Function

    Private Function AggiornaTabella(ConnLettura As Object, ConnScrittura As Object, Rec As Object, Tabella As String) As String
        Dim Ritorno As String = ""
        Dim NomeTabellaDest As String = "" & prefissotabelle & "" & Tabella
        Dim SqlL As String
        Dim SqlS As String
        Dim Righe As Integer = 0
        Dim Ancora As Boolean
        Dim NumCampo As Integer
        Dim Campo As String

        Try
            SqlS = "Truncate Table " & NomeTabellaDest
            Ritorno = EsegueSql(ConnScrittura, SqlS, ConnessioneSQL)

            If Ritorno.IndexOf("ERRORE") = -1 Then
                SqlL = "Select * From " & Tabella
                Rec = LeggeQuery(ConnLettura, SqlL, ConnessioneMDB)
                Do Until Rec.Eof
                    Righe += 1

                    SqlS = "Insert Into " & NomeTabellaDest & " Values ( 1, "
                    NumCampo = 0
                    Ancora = True
                    Do While Ancora = True
                        Try
                            Campo = RitornaFormatoStringa(Rec(NumCampo).Value)
                            SqlS += Campo & ","
                            NumCampo += 1
                        Catch ex As Exception
                            Ancora = False
                        End Try
                    Loop
                    SqlS = Mid(SqlS, 1, Len(SqlS) - 1) & ")"
                    Ritorno = EsegueSql(ConnScrittura, SqlS, ConnessioneSQL)
                    If Ritorno.IndexOf("ERRORE") <> -1 Then
                        Exit Do
                    End If

                    Rec.MoveNext()
                Loop
                Rec.Close()

                If Ritorno.IndexOf("ERRORE") = -1 Then
                    Ritorno = "Tabella " & Tabella & ". Righe Caricate: " & Righe
                End If
            End If
        Catch ex As Exception
            Ritorno = "ERRORE: " & ex.Message
        End Try

        Return Ritorno
    End Function

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        Response.Redirect("Principale.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub
End Class