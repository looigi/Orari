<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="Mappa.aspx.vb" Inherits="Orari.Mappa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%--    <%@ register assembly="GMaps" namespace="Subgurim.Controles" tagprefix="cc1" %>--%>
        
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCJ6LqMv1zV5Z_-wrETyen4ltCfMubiCzI"></script>
    <script type="text/javascript">
        var map;

        function calcRoute() {
            var mapCanvas = document.getElementById(PREFISSO + 'cphCorpo_map_canvas');
            var mapOptions = {
                center: new google.maps.LatLng(41.9100711, 12.5359979),
                zoom: 11,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }
            map = new google.maps.Map(mapCanvas, mapOptions);
        }

        function AggiungeMarker(myLatLngX, myLatLngY, icona, titolo, Spiegazioni) {
            var iconBase = '';
            var myLatlng = new google.maps.LatLng(myLatLngX, myLatLngY);

            var divvinoInfo = '<div style=\'width: 350px; heigth: 250px; text-align: left; \'>' + titolo + '<hr />' + Spiegazioni + '<hr /></div>';

            var infowindow = new google.maps.InfoWindow({
                content: divvinoInfo
            });

            var marker = new google.maps.Marker({
                position: myLatlng,
                map: map,
                title: titolo,
                icon: iconBase + icona
            });
            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCorpo" runat="server">
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
    <div id="divMaschera" runat="server" class="visuaImmagineMappa">
        <ul class="ul" style="margin-left: -35px;">
            <li class="liCentro">
                <asp:CheckBox ID="chkAbitazioni" runat="server" Text="Abitazioni" CssClass="option" Checked="True" OnCheckedChanged ="DisegnaMappaDiNuovo" AutoPostBack="True" />
            </li>
            <li class="liCentro">
                <asp:CheckBox ID="chkStudi" runat="server" Text="Studi" CssClass="option" Checked="True" OnCheckedChanged ="DisegnaMappaDiNuovo" AutoPostBack="True" />
            </li>
            <li class="liCentro">
                <asp:CheckBox ID="chkSocieta" runat="server" Text="Società" CssClass="option" Checked="True" OnCheckedChanged ="DisegnaMappaDiNuovo"  AutoPostBack="True" /><br />
                <asp:DropDownList ID="cmbSocieta" runat="server" CssClass ="casellacombinata" AutoPostBack="True" OnSelectedIndexChanged ="DisegnaMappaDiNuovoPerSoc" ></asp:DropDownList>
            </li>
            <li class="liCentro">
                <asp:CheckBox ID="chkCommesse" runat="server" Text="Commesse" CssClass="option" Checked="True" OnCheckedChanged ="DisegnaMappaDiNuovo" AutoPostBack="True" /><br />
                <asp:DropDownList ID="cmbCommessa" runat="server" CssClass ="casellacombinata" AutoPostBack="True" OnSelectedIndexChanged ="DisegnaMappaDiNuovoPerCom" ></asp:DropDownList>
            </li>
        </ul>
        <div id="map_canvas" runat="server" style="z-index: 100; width: 100%; height: 87%; border: 1px solid #aaaaaa;"></div>
    </div>

    <script type="text/javascript" >
        var ritardo;

        function SistemaImmagine() {
            //initialize();
            calcRoute();

            //ritardo = setInterval(function () { SpostaDivImmagine() }, 1000);
        }

        //function SpostaDivImmagine() {
        //    var barrasuperiore = document.getElementById("divTitolo");
            
        //    var divvino = document.getElementById("cphCorpo_divMaschera");
        //    var immagine = document.getElementById("cphCorpo_map_canvas");
        //    var y = (screen.height ) - 220; // - yImm;
        //    divvino.style.width = (barrasuperiore.clientWidth-35) + "px";
        //    divvino.style.height = y + "px";
        //    immagine.style.width = (barrasuperiore.clientWidth - 55) + "px";
        //    immagine.style.height = (y-30) + "px";
        //    clearInterval(ritardo);
        //}
    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
