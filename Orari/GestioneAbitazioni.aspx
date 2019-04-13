<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="GestioneAbitazioni.aspx.vb" Inherits="Orari.GestioneAbitazioni" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCJ6LqMv1zV5Z_-wrETyen4ltCfMubiCzI"></script>
    <script type="text/javascript">
        function calcRoute() {
            var address = document.getElementById(PREFISSO + "cphCorpo_txtAbitazione").value;

            var latlng = new google.maps.LatLng(41.9100711, 12.5359979);
            var geocoder = new google.maps.Geocoder();

            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    var mapOptions = {
                        zoom: 15,
                        center: latlng
                    }
                    var map = new google.maps.Map(document.getElementById(PREFISSO + "cphCorpo_map_canvas"), mapOptions);
                    map.setCenter(results[0].geometry.location);
                    var marker = new google.maps.Marker({
                        map: map,
                        position: results[0].geometry.location
                    });

                    var lat1 = results[0].geometry.location.lat();
                    var lng1 = results[0].geometry.location.lng();

                    document.getElementById(PREFISSO + "cphCorpo_txtLatLng").value = lat1 + "," + lng1;
                } else {
                    var mapOptions = {
                        zoom: 11,
                        center: latlng
                    }
                    var map = new google.maps.Map(document.getElementById(PREFISSO + "cphCorpo_map_canvas"), mapOptions);

                    document.getElementById(PREFISSO + "cphCorpo_txtLatLng").value = "";
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCorpo" runat="server">
    <div id="barraTasti" runat="server" class="barraTasti" >
        <div id="divUscita" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('divUscita',4);" 
            onmouseout ="mouseOutImageSopra('divUscita',4);" >
            <asp:ImageButton ID="imgUscita" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_USCITA.png" ToolTip="Aggiorna i dati" />
        </div>
        <div id="Indietro" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Indietro',1);" 
            onmouseout ="mouseOutImageSopra('Indietro',1);" >
            <asp:ImageButton ID="imgAnnulla" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip="Indietro" />
        </div>
        <div id="Salva" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Salva',2);" 
            onmouseout ="mouseOutImageSopra('Salva',2);" >
            <asp:ImageButton ID="imgSalva" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_SALVA.png" ToolTip="Salva i dati" />
        </div>
        <div id="Nuovo" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Nuovo',3);" 
            onmouseout ="mouseOutImageSopra('Nuovo',3);" >
            <asp:ImageButton ID="imgNuova" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Nuovo valore" />
        </div>
    </div>

    <div class="mascheramodifica">
        <ul class="ul">
            <li class="li">
                <asp:Label ID="Label1" runat="server" Text="Abitazione" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbAbitazioni" runat="server" CssClass ="casellacombinata" OnSelectedIndexChanged ="SelezionaAbitazione" AutoPostBack ="true" ></asp:DropDownList>
            </li>
            <li class="liCentro">
            </li>
            <li class="li">
            </li>
        </ul>
        <ul class="ul">
            <li class="li">
                <asp:Label ID="Label2" runat="server" Text="Abitazione" CssClass ="etichettaTitolo"></asp:Label>&nbsp;
                <asp:ImageButton ID="imgCalcolaLatLng" runat="server" width="40px" ImageUrl="App_Themes/Standard/Images/latlng.jpg" BorderColor="#AAAAAA" BorderStyle="Solid" BorderWidth="1" />
            </li>
            <li class="li">
                <asp:TextBox ID="txtAbitazione" runat="server" CssClass ="casellatesto" text="" MaxLength="100" ></asp:TextBox>
           </li>
            <li class="li">
            </li>
            <li class="li">
           </li>
            <li class="li">
                <asp:Label ID="Label4" runat="server" Text="Data inizio" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtDataInizio" runat="server" CssClass ="casellatesto datepicker" text="" MaxLength="10"></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="Label5" runat="server" Text="Data Fine" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtDataFine" runat="server" CssClass ="casellatesto datepicker" text="" MaxLength="10"></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="Label9" runat="server" Text="Lat-Lng" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtLatLng" runat="server" CssClass ="casellatesto" text="" MaxLength="70" Enabled="False"></asp:TextBox>
           </li>
            <li class="li">
            </li>
            <li class="li">
           </li>
        </ul>
        <div id="map_canvas" runat="server" style="z-index: 100; width: 100%; height: 200px; border: 1px solid #aaaaaa;"></div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
