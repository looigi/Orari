<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="Statistiche.aspx.vb" Inherits="Orari.Statistiche" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%--<script type="text/javascript" src="Scripts/jquery-1.11.1.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/highcharts.js"></script>
    <script type="text/javascript" src="Scripts/modules/exporting.js"></script>
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>

    <script type="text/javascript">
        function DisegnaStatistica(nomeDiv, testo, TitAsseY, TitSeries, Suffisso) {
            var pos1 = document.getElementById(PREFISSO + "cphCorpo_hdnPassaggio1");
            var pos2 = document.getElementById(PREFISSO + "cphCorpo_hdnPassaggio2");

            var pp1 = pos1.value.toString().split(",");
            var pp2 = pos2.value.split(",");

            var i;

            var p1 = new Array();
            for (i = 0; i < pp1.length; i++) {
                p1[i] = pp1[i];
                while (p1[i].indexOf("'") > -1) {
                    p1[i] = p1[i].replace("'", "");
                }
            }
            var p2 = new Array();
            for (i = 0; i < pp2.length; i++) {
                p2[i] = eval(pp2[i]);
            }

            $('#' + PREFISSO + 'cphCorpo_'+nomeDiv).highcharts({
                title: {
                    text: testo,
                    x: 0 //center
                },
                subtitle: {
                    text: '',
                    x: 0
                },
                xAxis: {
                    categories: p1
                },
                yAxis: {
                    title: {
                        text: TitAsseY
                    },
                    plotLines: [{
                        value: 0,
                        width: 1,
                        color: '#808080'
                    }]
                },
                tooltip: {
                    valueSuffix: Suffisso
                },
                legend: {
                    layout: 'vertical',
                    align: 'right',
                    verticalAlign: 'middle',
                    borderWidth: 0
                },
                series: [{
                    name: TitSeries,
                    data: p2
                }]
            })
        }
    </script>

    <script src="//www.google.com/jsapi"></script>
    <script>
        google.load('visualization', '1', { packages: ['corechart'] });
    </script>
    
    <script type="text/javascript">
        // Draw the chart and set the chart values
        function drawChart(nomeDiv, Titolo, Array1, Array2) {
            if (nomeDiv!='') {
                var titoli = [];
                var inputO = Array1.split(";");
                //titoli.push(Titolo1);
                for (var i = 0; i < inputO.length; i++) {
                    titoli.push(inputO[i]);
                }

                var valori = [];
                //valori.push(Titolo2);
                var input1 = Array2.split(";");
                for (var i = 0; i < input1.length; i++) {
                    valori.push(parseInt(input1[i]));
                }

                //var data = google.visualization.arrayToDataTable(titoli, valori);

                var Combined = new Array();
                Combined[0] = ['First', 'Second'];
                for (var i = 0; i < titoli.length; i++) {
                    Combined[i + 1] = [titoli[i], valori[i]];
                }
                //alert(Combined);

                var data = google.visualization.arrayToDataTable(Combined);
                //var data = google.visualization.arrayToDataTable([
                //  ['Task', 'Hours per Day'],
                //  ['Work', 8],
                //  ['Friends', 2],
                //  ['Eat', 2],
                //  ['TV', 3],
                //  ['Gym', 2],
                //  ['Sleep', 7]
                //]);
                //
                // Optional; add a title and set the width and height of the chart
                var options = { 'title': '' + Titolo + '', 'width': '100%', 'height': '500', is3D: true };

                var chart = new google.visualization.PieChart(document.getElementById(nomeDiv));

                var my_div;

                if (Titolo == 'Tempi commesse') {
                    my_div = document.getElementById('piechartCommesseIMG');
                    google.visualization.events.addListener(chart, 'ready', function () {
                        my_div.innerHTML = '<img id="commesse" runat="server" src="' + chart.getImageURI() + '" />';
                    });

                    window.setTimeout("getImageData()", 500);
                } else {
                    my_div = document.getElementById('piechartLinguaggiIMG');
                    google.visualization.events.addListener(chart, 'ready', function () {
                        my_div.innerHTML = '<img id="linguaggi" runat="server" src="' + chart.getImageURI() + '" />';
                    });
                }

                // Display the chart inside the <div> element with id="piechart"
                chart.draw(data, options);
            }
        }

        var pathDest = '';

        function setPath(path) {
            pathDest = path;
        }

        function getImageData() {
            var i1 = document.getElementById(PREFISSO + 'head_commesse');
            var i2 = document.getElementById(PREFISSO + 'head_linguaggi');

            //var h1 = document.getElementById(PREFISSO + "cphCorpo_hdnImg1")
            //var h2 = document.getElementById(PREFISSO + "cphCorpo_hdnImg2")

            //h1.value = i1.src;
            //h2.value = i2.src;

            PageMethods.SalvaImmagini(pathDest, i1.src, i2.src, CallSuccess);
        }

        function CallSuccess(res) {
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCorpo" runat="server">
<%--    <asp:HiddenField ID="hdnImg1" runat="server" />
    <asp:HiddenField ID="hdnImg2" runat="server" />--%>
    <asp:HiddenField ID="hdnPassaggio1" runat="server" />
    <asp:HiddenField ID="hdnPassaggio2" runat="server" />
    <div id="barraTasti" runat="server" class="barraTasti" >
        <div id="divUscita" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('divUscita',2);" 
            onmouseout ="mouseOutImageSopra('divUscita',2);" >
            <asp:ImageButton ID="imgUscita" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_USCITA.png" ToolTip="Aggiorna i dati" />
        </div>
        <div id="Indietro" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Indietro',1);" 
            onmouseout ="mouseOutImageSopra('Indietro',1);" >
            <asp:ImageButton ID="imgAnnulla" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip="Indietro" />
        </div>
    </div>

    <div id="MascheroneStatistiche" runat="server" class="mascheramodificastat">
        <div id="tastiLaterali" runat="server" class="tastiLaterali">
            <ul style="margin-left: -35px;">
                <li class="listat">
                    <asp:Label ID="Label1" runat="server" Text="Società" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:DropDownList ID="cmbSocieta" runat="server" AutoPostBack ="true" OnSelectedIndexChanged ="CambiaSocieta" CssClass="casellacombinatapiccolac"></asp:DropDownList>
                </li>
                <li class="listat">
                    <hr />
                </li>
                <li class="listat">
                    <hr />
                </li>
                <li class="listat">
                    <asp:Label ID="lblStatCommesse" runat="server" Text="Tempi commesse" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgStatCommesse" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblGiorniLavorati" runat="server" Text="Giorni lavorati" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgGiorniLavorati" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblGiorniFerie" runat="server" Text="Giorni di ferie" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgGiorniFerie" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="Label3" runat="server" Text="Giorni Lavoro a casa" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgGiorniLavoroACasa" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblGiorniMalattia" runat="server" Text="Giorni di malattia" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgGiorniMalattia" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblOreMalattia" runat="server" Text="Ore di malattia" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgOreMalattia" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblPermessi" runat="server" Text="Permessi" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgGiorniPermesso" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblorePermesso" runat="server" Text="Ore di permesso" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgOrePermesso" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblStatPranzi" runat="server" Text="Portate" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgStatPranzi" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblStatPasticca" runat="server" Text="Pasticche" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgStatPasticca" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblStatMezziAndata" runat="server" Text="Mezzi di andata" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgStatMezziAndata" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblStatMezziRitorno" runat="server" Text="Mezzi di ritorno" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgStatMezziRitorno" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblMinimoEntrata" runat="server" Text="Orario minimo di entrata" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgMinimoEntrata" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblStraordinari" runat="server" Text="Straordinari" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgMaxStarordinari" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Periodo continuativo lavoro" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgPeriodoLavorativo" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblKM" runat="server" Text="Distanze" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgKM" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblGradi" runat="server" Text="Gradi" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgGradi" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="lblEntrate" runat="server" Text="Entrate" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgEntrate" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
                <li class="listat">
                    <asp:Label ID="Label2" runat="server" Text="Curriculum" CssClass="etichettanote"></asp:Label>
                </li>
                <li class="listat">
                    <asp:ImageButton ID="imgCurriculum" runat="server" width="30px" ImageUrl="App_Themes/Standard/Images/icona_CERCA.png" />
                </li>
            </ul>
        </div>
        <div id="mascheraStat" runat="server" class="mascheraStatistica">
            <div class="titolomaschera" >
                <asp:Label ID="lblTitoloMaschera" runat="server" Text="Tempi commesse" CssClass ="etichettaTitoloGrande" ></asp:Label>
            </div>
            <%--<hr />--%>
            <br /><br />
            <asp:CheckBox ID="chkSplitta" runat="server" Text ="Divide per società" Checked ="true" AutoPostBack ="true" OnCheckedChanged="CambiaSocieta" CssClass="option" />
            <div id="divStatCommesse" runat="server" class="stat" >
                <asp:GridView id="grdStatCommesse" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Commessa" HeaderText="Commessa" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ore" HeaderText="Ore" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Km" HeaderText="Km" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divGiorniLavorati" runat="server" class="stat" >
                <asp:GridView id="grdGiorniLavorati" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorno" HeaderText="Giorno" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mese" HeaderText="Mese" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ore" HeaderText="Ore" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheGiorniLavorati"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divFerie" runat="server" class="stat" >
                <asp:GridView id="grdFerie" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorno" HeaderText="Giorno" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mese" HeaderText="Mese" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ore" HeaderText="Ore" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>
 
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheGiorniFerie"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                   </Columns>
                </asp:GridView>
            </div>
            <div id="divLavoroACasa" runat="server" class="stat" >
                <asp:GridView id="grdLavoroCasa" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorno" HeaderText="Giorno" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mese" HeaderText="Mese" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ore" HeaderText="Ore" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>
 
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheGiorniLavoroCasa"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                   </Columns>
                </asp:GridView>
            </div>
            <div id="divPermessi" runat="server" class="stat" >
                <asp:GridView id="grdPermessi" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorno" HeaderText="Giorno" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mese" HeaderText="Mese" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ore" HeaderText="Ore" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheGiorniPermessi"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divMalattia" runat="server" class="stat" >
                <asp:GridView id="grdMalattia" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorno" HeaderText="Giorno" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mese" HeaderText="Mese" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ore" HeaderText="Ore" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheGiorniMalattia"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divOreMalattia" runat="server" class="stat" >
                <asp:GridView id="grdOreMalattia" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorno" HeaderText="Giorno" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mese" HeaderText="Mese" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ore" HeaderText="Ore" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheOreMalattia"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divOrePermesso" runat="server" class="stat" >
                <asp:GridView id="grdOrePermesso" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorno" HeaderText="Giorno" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mese" HeaderText="Mese" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ore" HeaderText="Ore" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheOrePermesso"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divStatPranzi" runat="server" class="stat" >
                <asp:GridView id="grdPranzi" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Portata" HeaderText="Portata" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ricorrenze" HeaderText="Ricorrenze" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatistichePranzi"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divStatPasticche" runat="server" class="stat" >
                <asp:GridView id="grdPasticche" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Pasticca" HeaderText="Pasticca" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ricorrenze" HeaderText="Ricorrenze" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatistichePasticche"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divStatMezziAndata" runat="server" class="stat" >
                <asp:GridView id="grdMezziAndata" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mezzo" HeaderText="Mezzo" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ricorrenze" HeaderText="Ricorrenze" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheMezziDiAndata"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divStatMezziRitorno" runat="server" class="stat" >
                <asp:GridView id="grdMezziRitorno" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Mezzo" HeaderText="Mezzo" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Ricorrenze" HeaderText="Ricorrenze" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                    OnClick ="StatisticheMezziDiRitorno"
                                    >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divMinimoEntrata" runat="server" class="stat" >
                <asp:GridView id="grdMinimoEntrata" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Commessa" HeaderText="Commessa" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Data" HeaderText="Data" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Entrata" HeaderText="Entrata" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divStraordinari" runat="server"  class="stat">
                <asp:GridView id="grdStraordinari" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Pos" HeaderText="Pos." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Societa" HeaderText="Società" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Commessa" HeaderText="Commessa" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Data" HeaderText="Data" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Entrata" HeaderText="Entrata" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPeriodoLavorativo" runat="server" class="stat">
                <asp:GridView id="grdPeriodo" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Giorni" HeaderText="Giorni" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="DataInizio" HeaderText="Data Inizio" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="DataFine" HeaderText="Data Fine" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SocietaIn" HeaderText="Società Inizio" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="SocietaFi" HeaderText="Società Fine" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divKM" runat="server" class="stat">
                <asp:GridView id="grdKM" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:BoundField DataField="Societa" HeaderText="Societa" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Commessa" HeaderText="Commessa" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Km" HeaderText="km." >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia-destra" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divGradi" runat="server" style="width: 100%; height: 400px; border: 1px solid #444444;">
            </div>
            <div id="divGradiDett" runat="server" style="width: 100%; height: 20px; border: 1px solid #444444; margin-top: 3px;">
                <asp:Label ID="lblGradiDett" runat="server" Text="Label" CssClass="etichettaTitolo"></asp:Label>
            </div>

            <div id="divEntrate" runat="server" style="width: 100%; height: 400px; border: 1px solid #444444;">
            </div>
            <div id="divCurriculum" runat="server" style="width: 99%; height: 600px; max-height: 600px; overflow:auto; border: 1px solid #444444; display: inline-block; text-align:left; padding: 3px;">
                <div style="float: right;"><asp:Button ID="cmdScaricaPDF" runat="server" Text="Scarica" /></div><br />
                <asp:Label ID="lblCurriculum" runat="server" Text="Label" CssClass ="etichettaCurriculum"></asp:Label><asp:HiddenField ID="hdnNomeFileHTML" runat="server" />
            </div>
        </div>
    </div>
    
    <div id="divStatistiche" runat="server" class="Statistiche">
        <asp:Label ID="lblStatistiche" runat="server" Text="Mezzo" CssClass ="etichettaTitolo"></asp:Label>
        <hr />
        <asp:GridView id="grdStatistiche" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
        PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
            <Columns>
                <asp:BoundField DataField="Societa" HeaderText="Societa" SortExpression="" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                </asp:BoundField>
                <asp:BoundField DataField="Giorno" HeaderText="Giorno" SortExpression="" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                </asp:BoundField>
                <asp:BoundField DataField="Ore" HeaderText="Ore" SortExpression="" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <hr />
        <asp:ImageButton ID="imgChiudeStat" runat="server" width="50px" Height="50px" style="float: right; padding-top: 5px;" ImageUrl ="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip="Indietro" />
    </div>

</asp:Content>
