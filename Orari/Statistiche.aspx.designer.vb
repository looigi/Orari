'------------------------------------------------------------------------------
' <generato automaticamente>
'     Codice generato da uno strumento.
'
'     Le modifiche a questo file possono causare un comportamento non corretto e verranno perse se
'     il codice viene rigenerato. 
' </generato automaticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class Statistiche
    
    '''<summary>
    '''Controllo commesse.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents commesse As Global.System.Web.UI.HtmlControls.HtmlImage
    
    '''<summary>
    '''Controllo linguaggi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents linguaggi As Global.System.Web.UI.HtmlControls.HtmlImage
    
    '''<summary>
    '''Controllo hdnPassaggio1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdnPassaggio1 As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo hdnPassaggio2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdnPassaggio2 As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo barraTasti.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents barraTasti As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo divUscita.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divUscita As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo imgUscita.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgUscita As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo Indietro.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Indietro As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo imgAnnulla.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgAnnulla As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo MascheroneStatistiche.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents MascheroneStatistiche As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo tastiLaterali.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents tastiLaterali As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo Label1.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo cmbSocieta.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmbSocieta As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Controllo lblStatCommesse.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblStatCommesse As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgStatCommesse.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgStatCommesse As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblGiorniLavorati.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblGiorniLavorati As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgGiorniLavorati.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgGiorniLavorati As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblGiorniFerie.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblGiorniFerie As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgGiorniFerie.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgGiorniFerie As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo Label3.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label3 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgGiorniLavoroACasa.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgGiorniLavoroACasa As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblGiorniMalattia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblGiorniMalattia As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgGiorniMalattia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgGiorniMalattia As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblOreMalattia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblOreMalattia As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgOreMalattia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgOreMalattia As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblPermessi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblPermessi As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgGiorniPermesso.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgGiorniPermesso As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblorePermesso.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblorePermesso As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgOrePermesso.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgOrePermesso As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblStatPranzi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblStatPranzi As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgStatPranzi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgStatPranzi As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblStatPasticca.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblStatPasticca As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgStatPasticca.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgStatPasticca As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblStatMezziAndata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblStatMezziAndata As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgStatMezziAndata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgStatMezziAndata As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblStatMezziRitorno.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblStatMezziRitorno As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgStatMezziRitorno.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgStatMezziRitorno As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblMinimoEntrata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblMinimoEntrata As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgMinimoEntrata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgMinimoEntrata As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblStraordinari.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblStraordinari As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgMaxStarordinari.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgMaxStarordinari As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblPeriodo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblPeriodo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgPeriodoLavorativo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgPeriodoLavorativo As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblKM.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblKM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgKM.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgKM As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblGradi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblGradi As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgGradi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgGradi As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo lblEntrate.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblEntrate As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgEntrate.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgEntrate As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo Label2.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents Label2 As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo imgCurriculum.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgCurriculum As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Controllo mascheraStat.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents mascheraStat As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo lblTitoloMaschera.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblTitoloMaschera As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo chkSplitta.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents chkSplitta As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Controllo divStatCommesse.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divStatCommesse As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdStatCommesse.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdStatCommesse As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divGiorniLavorati.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divGiorniLavorati As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdGiorniLavorati.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdGiorniLavorati As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divFerie.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divFerie As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdFerie.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdFerie As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divLavoroACasa.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divLavoroACasa As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdLavoroCasa.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdLavoroCasa As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divPermessi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divPermessi As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdPermessi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdPermessi As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divMalattia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divMalattia As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdMalattia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdMalattia As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divOreMalattia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divOreMalattia As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdOreMalattia.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdOreMalattia As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divOrePermesso.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divOrePermesso As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdOrePermesso.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdOrePermesso As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divStatPranzi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divStatPranzi As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdPranzi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdPranzi As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divStatPasticche.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divStatPasticche As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdPasticche.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdPasticche As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divStatMezziAndata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divStatMezziAndata As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdMezziAndata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdMezziAndata As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divStatMezziRitorno.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divStatMezziRitorno As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdMezziRitorno.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdMezziRitorno As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divMinimoEntrata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divMinimoEntrata As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdMinimoEntrata.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdMinimoEntrata As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divStraordinari.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divStraordinari As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdStraordinari.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdStraordinari As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divPeriodoLavorativo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divPeriodoLavorativo As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdPeriodo.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdPeriodo As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divKM.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divKM As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo grdKM.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdKM As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo divGradi.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divGradi As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo divGradiDett.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divGradiDett As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo lblGradiDett.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblGradiDett As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo divEntrate.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divEntrate As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo divCurriculum.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divCurriculum As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo cmdScaricaPDF.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents cmdScaricaPDF As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Controllo lblCurriculum.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblCurriculum As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo hdnNomeFileHTML.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents hdnNomeFileHTML As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Controllo divStatistiche.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents divStatistiche As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Controllo lblStatistiche.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents lblStatistiche As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Controllo grdStatistiche.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents grdStatistiche As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Controllo imgChiudeStat.
    '''</summary>
    '''<remarks>
    '''Campo generato automaticamente.
    '''Per la modifica, spostare la dichiarazione di campo dal file di progettazione al file code-behind.
    '''</remarks>
    Protected WithEvents imgChiudeStat As Global.System.Web.UI.WebControls.ImageButton
End Class
