Module GestioneDB
    Public ConnessioneMDB As String
    Public ConnessioneSQL As String
    Public ModalitaLocale As Boolean = True

    Public Function ProvaConnessione(Connessione As String) As String
        Dim Conn As Object = CreateObject("ADODB.Connection")

        Try
            Conn.Open(Connessione)
            Conn.Close()

            Conn = Nothing
            Return ""
        Catch ex As Exception
            Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
            Dim StringaPassaggio As String

            StringaPassaggio = "?Errore=Apertura DB"
            StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
            StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            StringaPassaggio = StringaPassaggio & "&Errore=" & ex.Message
            H.Response.Redirect("Errore.aspx" & StringaPassaggio)

            Return ex.Message
        End Try
    End Function

    Public Sub LeggeOreStandard(Percorso As String)
        If LeggeImpostazioniDiBase(Percorso, "SQL") = True Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = "Select Ore From " & prefissotabelle & "Impostazioni Where idUtente=" & idUtente

            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            If Rec.Eof = True Then
                OreStandard = 8
            Else
                If Rec(0).Value Is DBNull.Value = False Then
                    OreStandard = Rec(0).Value
                Else
                    OreStandard = 8
                End If
            End If
            Rec.Close()

            ConnSQL.Close()
        End If
    End Sub

    Public Function LeggeImpostazioniDiBase(ByVal Percorso As String, Tipologia As String, Optional Statistiche As Boolean = False) As Boolean
        Dim Ritorno As String
        Dim Ok As Boolean = True
        Dim CosaCercare As String
        Dim Conn As String = ""

        'If Tipologia = "MDB" Then
        '    CosaCercare = "MDBConnectionString"
        'Else
        '    If ModalitaLocale = True Then
        '        CosaCercare = "SQLConnectionStringLOCALEOrari"
        '    Else
        '        CosaCercare = "SQLConnectionStringWEB"
        '    End If
        'End If

        If Not Statistiche Then
            CosaCercare = "SQLConnectionStringLOCALEOrari"
        Else
            CosaCercare = "SQLConnectionStringLOCALEStatistiche"
        End If


        ' Impostazioni di base
        Dim ListaConnessioni As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings

        If ListaConnessioni.Count <> 0 Then
            ' Get the collection elements. 
            For Each Connessioni As ConnectionStringSettings In ListaConnessioni
                Dim Nome As String = Connessioni.Name
                Dim Provider As String = Connessioni.ProviderName
                Dim connectionString As String = Connessioni.ConnectionString

                If Nome = CosaCercare Then
                    Conn = "Provider=" & Provider & ";" & connectionString.Replace("***", Percorso)

                    Exit For
                End If
            Next
        End If

        If Conn = "" Then
            ' Response.Redirect("errore_ErroreImprevisto.aspx?Errore=Impostazioni di connessione al DB non valide&Chiamante=" & Request.CurrentExecutionFilePath.ToUpper.Trim & "&Sql=")
            Ok = False
        Else
            Ritorno = ProvaConnessione(Conn)
            If Ritorno <> "" Then
                ' Response.Redirect("errore_ErroreImprevisto.aspx?Errore=" & Ritorno & "&Chiamante=" & Request.CurrentExecutionFilePath.ToUpper.Trim & "&Sql=")
                Ok = False
            Else
                'If Tipologia = "MDB" Then
                '    ConnessioneMDB = Conn
                'Else
                ConnessioneSQL = Conn
                'End If
            End If
            ' Impostazioni di base
        End If

        Return Ok
    End Function

    Public Function ApreDB(Connessione As String) As Object
        ' Routine che apre il DB e vede se ci sono errori
        Dim Conn As Object = CreateObject("ADODB.Connection")

        Try
            Conn.Open(Connessione)
            Conn.CommandTimeout = 0
        Catch ex As Exception
            Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
            Dim StringaPassaggio As String

            StringaPassaggio = "?Errore=Apertura DB"
            StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
            StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            StringaPassaggio = StringaPassaggio & "&Sql="
            H.Response.Redirect("Errore.aspx" & StringaPassaggio)
        End Try

        Return Conn
    End Function

    Private Function ControllaAperturaConnessione(ByRef Conn As Object, Connessione As String) As Boolean
        Dim Ritorno As Boolean = False

        If Conn Is Nothing Then
            Ritorno = True
            Conn = ApreDB(Connessione)
        End If

        Return Ritorno
    End Function

    Public Function EsegueSql(ByVal Conn As Object, ByVal Sql As String, Connessione As String) As String
        Dim AperturaManuale As Boolean = ControllaAperturaConnessione(Conn, Connessione)
        Dim Ritorno As String = ""

        ' Routine che esegue una query sul db
        Try
            Conn.Execute(Sql)
        Catch ex As Exception
            Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
            Dim StringaPassaggio As String

            StringaPassaggio = "?Errore=Errore esecuzione query: " & Err.Description
            StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
            StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            StringaPassaggio = StringaPassaggio & "&Sql=" & Sql
            H.Response.Redirect("Errore.aspx" & StringaPassaggio)
        End Try

        ChiudeDB(AperturaManuale, Conn)

        Return Ritorno
    End Function

    Private Sub ChiudeDB(ByVal TipoApertura As Boolean, ByRef Conn As Object)
        If TipoApertura = True Then
            Conn.Close()
        End If
    End Sub

    Public Function LeggeQuery(ByVal Conn As Object, ByVal Sql As String, Connessione As String) As Object
        Dim AperturaManuale As Boolean = ControllaAperturaConnessione(Conn, Connessione)
        Dim Rec As Object = CreateObject("ADODB.Recordset")

        Try
            Rec.Open(Sql, Conn)
        Catch ex As Exception
            Rec = Nothing

            Dim H As HttpApplication = HttpContext.Current.ApplicationInstance
            Dim StringaPassaggio As String

            StringaPassaggio = "?Errore=Errore query: " & Err.Description
            StringaPassaggio = StringaPassaggio & "&Utente=" & H.Session("idUtente")
            StringaPassaggio = StringaPassaggio & "&Chiamante=" & H.Request.CurrentExecutionFilePath.ToUpper.Trim
            StringaPassaggio = StringaPassaggio & "&Sql=" & Sql
            H.Response.Redirect("Errore.aspx" & StringaPassaggio)
        End Try

        ChiudeDB(AperturaManuale, Conn)

        Return Rec
    End Function
End Module
