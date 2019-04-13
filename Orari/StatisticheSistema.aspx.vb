Public Class StatisticheSistema
    Inherits System.Web.UI.Page

    Public rigaR As DataRow
    Public dttTabella As New DataTable()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If idUtente = -1 Or idUtente = 0 Or Utenza = "" Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Page.IsPostBack = False Then
            EsegueProcedura()
        End If
    End Sub

    Private Sub EsegueProcedura()
        If LeggeImpostazioniDiBase(Server.MapPath("."), "SQL", True) Then
            Dim ConnSQL As Object = ApreDB(ConnessioneSQL)
            Dim Rec As Object = CreateObject("ADODB.Recordset")
            Dim Sql As String = ""

            Sql &= "If Not EXISTS(SELECT * "
            Sql &= "From INFORMATION_SCHEMA.TABLES "
            Sql &= "Where TABLE_SCHEMA = 'dbo' And TABLE_NAME = 'Stat') "
            Sql &= "Begin "
            Sql &= "Create Table Stat (Cosa Varchar(30), Data varchar(10), Applicazione varchar(50), Quante Integer "
            Sql &= "Constraint [PK_Stat] PRIMARY KEY CLUSTERED "
            Sql &= "( "
            Sql &= "[Cosa] Asc, "
            Sql &= "[Data] ASC, "
            Sql &= "[Applicazione] Asc "
            Sql &= ")WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY] "
            Sql &= ") ON [PRIMARY]"
            Sql &= "End "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Delete From Stat"
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Applicazioni' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, Applicazione, Count(*) As Quante From Applicazioni Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US'), Applicazione Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Cronologia' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Count(*) As Quante From Cronologia Group By FORMAT(Data , 'dd/MM/yyyy', 'en-US') Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Immagini Scaricate' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Count(*) As Quante From ImmaginiScaricate Group By FORMAT(Data , 'dd/MM/yyyy', 'en-US') Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Immagini Locali' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Count(*) As Quante From Locali Group By FORMAT(Data , 'dd/MM/yyyy', 'en-US') Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Messaggi' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Count(*) As Quante From Messaggi Group By FORMAT(Data , 'dd/MM/yyyy', 'en-US') Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Mp3 Ascoltati' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Count(*) As Quante From Mp3 Group By FORMAT(Data , 'dd/MM/yyyy', 'en-US') Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Notifiche' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Count(*) As Quante From Notifiche Group By FORMAT(Data , 'dd/MM/yyyy', 'en-US') Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Telefonate' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Count(*) As Quante From Telefonate Group By FORMAT(Data , 'dd/MM/yyyy', 'en-US') Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Click Sinistro' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Sum(clikSinistro) As Quante From Sistema  Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US')  Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Click Destro' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Sum(clickDestro) As Quante From Sistema  Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US')  Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Tasti Premuti' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Sum(tastiPremuti) As Quante From Sistema  Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US')  Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Distanza Mouse' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Sum(diffX+diffY) As Quante From Sistema  Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US')  Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Inattivita' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Sum(secondiInattivita) As Quante From Sistema  Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US')  Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Attivita' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Sum(secondiAttivita) As Quante From Sistema  Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US')  Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Processi Aperti' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Sum(ProcessiAperti) As Quante From Sistema  Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US')  Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Sql = "Insert Into Stat Select Top 1 'Processi Chiusi' As Cosa, FORMAT(Data, 'dd/MM/yyyy', 'en-US') As Data, '' As Applicazione, Sum(ProcessiChiusi) As Quante From Sistema  Group By FORMAT(Data, 'dd/MM/yyyy', 'en-US')  Order By 4 Desc "
            EsegueSql(ConnSQL, Sql, ConnessioneSQL)

            Dim dCosa As New DataColumn("Cosa")
            Dim dData As New DataColumn("Data")
            Dim dAppl As New DataColumn("Applicazione")
            Dim dQuan As New DataColumn("Quante")

            Dim dttTabella As New DataTable()

            dttTabella = New DataTable
            dttTabella.Columns.Add(dCosa)
            dttTabella.Columns.Add(dData)
            dttTabella.Columns.Add(dAppl)
            dttTabella.Columns.Add(dQuan)

            Sql = "Select * From Stat"
            Rec = LeggeQuery(ConnSQL, Sql, ConnessioneSQL)
            Do Until Rec.Eof
                rigaR = dttTabella.NewRow()
                rigaR(0) = "" & Rec("Cosa").Value
                rigaR(1) = "" & Rec("Data").Value
                rigaR(2) = "" & Rec("Applicazione").Value
                rigaR(3) = ScriveNumero("" & Rec("Quante").Value)
                dttTabella.Rows.Add(rigaR)

                Rec.MoveNext()
            Loop
            Rec.Close

            grdGiorniLavorati.DataSource = dttTabella
            grdGiorniLavorati.DataBind()
            grdGiorniLavorati.SelectedIndex = -1

            ConnSQL.Close()
        End If
    End Sub

    Protected Sub imgUscita_Click(sender As Object, e As ImageClickEventArgs) Handles imgUscita.Click
        idUtente = -1
        Utenza = ""
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub imgAnnulla_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnnulla.Click
        Response.Redirect("Principale.aspx?Giorno=" & Request.QueryString("Giorno") & "&Mese=" & Request.QueryString("Mese") & "&Anno=" & Request.QueryString("Anno"))
    End Sub

End Class