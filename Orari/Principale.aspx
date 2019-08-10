<%@ Page Title="" Language="vb" AutoEventWireup="false" validateRequest="false" MasterPageFile="Orari.Master" CodeBehind="Principale.aspx.vb" Inherits="Orari.Principale" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCJ6LqMv1zV5Z_-wrETyen4ltCfMubiCzI"></script>
        
    <script type="text/javascript" src="Scripts/bookflip.js"></script>
    <style>#bookflip{color:#aaaaaa;}</style>

    <script type="text/javascript">
        var map;

        var x = (screen.width / 2) - 25;
        var y = screen.height - 235;

        function RidisegnaLibro(valore) {
            startingPage = parseInt(valore);
            autoFlip(startingPage);
        }

        function RiempieImmagini() {
            for (i = 0; i < 32; i++) {
                var oggetto = document.getElementById(PREFISSO+"cphCorpo_img" + i);
                var d = new Date();
                var n = d.getMilliseconds()
                var f = 'App_Themes/Standard/Images/Giorni/' + i + '.jpg?yyy=' + n;

                oggetto.src = f;
            }
        }

        var Ricorrenze = new Array();

        function RiempieRicorrenze() {
            var pos1 = document.getElementById(PREFISSO + "cphCorpo_hdnRicorrenze");
            var pp1 = pos1.value.toString().split(",");
            var i;

            for (i = 0; i < pp1.length; i++) {
                Ricorrenze[i] = pp1[i];
                while (Ricorrenze[i].indexOf("'") > -1) {
                    Ricorrenze[i] = Ricorrenze[i].replace("'", "");
                }
                while (Ricorrenze[i].indexOf("*ACAPO*") > -1) {
                    Ricorrenze[i] = Ricorrenze[i].replace("*ACAPO*", "<br />");
                }
                while (Ricorrenze[i].indexOf("**MIN***") > -1) {
                    Ricorrenze[i] = Ricorrenze[i].replace("**MIN***", "<");
                }
                while (Ricorrenze[i].indexOf("**MAX***") > -1) {
                    Ricorrenze[i] = Ricorrenze[i].replace("**MAX***", ">");
                }
            }
        }

        function VisuaRicorrenza() {
            var vistamese = document.getElementById(PREFISSO + "vistamese");
            // vistamese.style.visibility = "hidden";
            vistamese.style.opacity = ".15";
        }

        function NONVisuaRicorrenza(NomeOggetto) {
            var vistamese = document.getElementById(PREFISSO + "vistamese");
            // vistamese.style.visibility = "visible";
            vistamese.style.opacity = ".85";
        }

        function vaffammocca() {
        }

        function calcRoutePrinc() {
            var mapCanvas = document.getElementById(PREFISSO + 'cphCorpo_map_canvas');

            var mapOptions = {
                center: new google.maps.LatLng(41.9100711, 12.5359979),
                zoom: 6,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            }

            map = new google.maps.Map(mapCanvas, mapOptions);
        }

        function AggiungeMarkerPrinc(myLatLngX, myLatLngY, ore, kms) {
            var orette = [];
            var inputO = ore.split(";");
            for (var i = 0; i < inputO.length; i++) {
                orette.push(inputO[i]);
            }
            var pathsX = [];
            var inputX = myLatLngX.split(";");
            for (var i = 0; i < inputX.length; i++) {
                pathsX.push(inputX[i]);
            }
            var pathsY = [];
            var inputY = myLatLngY.split(";");
            for (var i = 0; i < inputY.length; i++) {
                pathsY.push(inputY[i]);
            }
            var coordinate = [];
            for (var j = 0; j < pathsX.length-1; j++) {
                coordinate.push(new google.maps.LatLng(pathsX[j].replace(",", "."), pathsY[j].replace(",", ".")));
            }
            var km = [];
            var kmX = kms.split(";");
            for (var i = 0; i < kmX.length; i++) {
                km.push(kmX[i]);
            }

            var fPath = new google.maps.Polyline({
                path: coordinate,
                geodesic: true,
                strokeColor: '#0000AA',
                strokeOpacity: 0.6,
                strokeWeight: 2
            });

            var markers = [];

            var bounds = new google.maps.LatLngBounds();

             for (var j = 0; j < coordinate.length; j++) {
                /*var marker = new google.maps.Marker({
                    position: coordinate[j],
                    map: map,
                    icon: {url: 'App_Themes/Standard/Images/map-pin.png', scaledSize: new google.maps.Size(25, 25) },
                    title: 'Coord.: ' + orette[j] + '\nKm: ' + km[j]
                }); */

                bounds.extend(coordinate[j]);

                // markers.push(marker);
            }

            map.fitBounds(bounds);       
            map.panToBounds(bounds);     

            fPath.setMap(map);
        }

        function aggiungeMultimedia(multimediaX, multimediaY, tipologia, filetti) {
            var tipo = [];
            var tipol = tipologia.split(";");
            for (var i = 0; i < tipol.length; i++) {
                tipo.push(tipol[i]);
            }
            var nomefile = [];
            var nfile = filetti.split(";");
            for (var i = 0; i < nfile.length; i++) {
                nomefile.push(nfile[i]);
            }
            var pathsX = [];
            var inputX = multimediaX.split(";");
            for (var i = 0; i < inputX.length; i++) {
                pathsX.push(inputX[i]);
            }
            var pathsY = [];
            var inputY = multimediaY.split(";");
            for (var i = 0; i < inputY.length; i++) {
                pathsY.push(inputY[i]);
            }
            var coordinate = [];
            for (var j = 0; j < pathsX.length - 1; j++) {
                coordinate.push(new google.maps.LatLng(pathsX[j].replace(",", "."), pathsY[j].replace(",", ".")));
            }
            var markers = [];
            // for (var j = 0; j < pathsX.length - 1; j++) {
            //     markers.push(new google.maps.LatLng(pathsX[j].replace(",", "."), pathsY[j].replace(",", ".")));
            // }
            for (var j = 0; j < coordinate.length; j++) {
                var icona;
                (function (marker) {
                    if (tipo[j] == 'I') {
                        icona = { url: nomefile[j], scaledSize: new google.maps.Size(25, 25) };
                    } else {
                        icona = { url: 'App_Themes/Standard/Images/video.png', scaledSize: new google.maps.Size(25, 25) };
                    }

                    var marker = new google.maps.Marker({
                        position: coordinate[j],
                        map: map,
                        icon: icona,
                        title: nomefile[j]
                    });

                    google.maps.event.addDomListener(marker, 'click', function () {
                        disegnaImmagine(marker.title);
                    });

                    markers.push(marker);
                })(markers[i]);
            }
        }

        function disegnaImmagine(nf) {
            //alert(nf);
            //var nf2 = anno + mese + nf + ".jpg";
            //var urlAnno = "http://looigi.no-ip.biz:12345/Orari/Pennetta/" + anno + "/" + nf2;
            //var urlYeah = "http://looigi.no-ip.biz:12345/Orari/Pennetta/Yeah/" + nf2;
            //var urlVolti = "http://looigi.no-ip.biz:12345/Orari/Pennetta/Volti/" + nf2;

            //var qualeUrl = "";
            //if (imageExists(urlAnno)) {
            //    qualeUrl = urlAnno;
            //} else {
            //    if (imageExists(urlYeah)) {
            //        qualeUrl = urlYeah;
            //    } else {
            //        if (imageExists(urlVolti)) {
            //            qualeUrl = urlVolti;
            //        }
            //    }
            //}

            //// alert(urlAnno);
            //if (qualeUrl != '') {
            //    window.open(nf);
            //} else {
            //    alert('Nessuna immagine rilevata')
            //}
            var img = document.getElementById("divImmagine");
            img.style.display = "block";
            img.style.backgroundImage = "url('" + nf + "')";;

            // var img = document.getElementById("imgImmagine");
            // img.src = nf;
        }

        // function apriImmagine() {
        //     var div = document.getElementById("divImmagine");
        //     div.style.display = "block";
        // 
        //     var img = document.getElementById("imgImmagine");
        //     img.src = "App_Themes/Standard/Images/visualizzato.png";
        // }

        function chiudiImmagine() {
            var img = document.getElementById("divImmagine");
            img.style.display = "none";
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphCorpo" runat="server">
    <asp:HiddenField ID="hdnGiorno" runat="server" />
    <asp:HiddenField ID="hdnRicorrenze" runat="server" />

    <div id="barraTasti" runat="server" class="barraTasti" >
        <div id="divUscita" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('divUscita',7);" 
            onmouseout ="mouseOutImageSopra('divUscita',7);" >
            <asp:ImageButton ID="imgUscita" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_USCITA.png" ToolTip="Aggiorna i dati" />
        </div>
        <%--<div style="position: absolute; top: 0px; left: 0px; width: 80px; height: 749px; background: url(http://localhost:63464/App_Themes/Standard/Images/black_gradientr.png) 100% 0% repeat-y transparent;"></div>--%>
        <div id="RefreshMese" runat="server"
            class="linguetta"
            onmouseover ="mouseOverImageSopra('RefreshMese',2);" 
            onmouseout ="mouseOutImageSopra('RefreshMese',2);"  >
            <asp:ImageButton ID="imgRefreshMese" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/refresh.png" ToolTip="Refresh mese" />
        </div>
        <div id="Impostazioni" runat="server"
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Impostazioni',3);" 
            onmouseout ="mouseOutImageSopra('Impostazioni',3);"  >
            <asp:ImageButton ID="imgImpostazioni" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona-EQUALIZZATORE.png" ToolTip="Impostazioni" />
        </div>
        <div id="Tabelle" runat="server"
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Tabelle',4);" 
            onmouseout ="mouseOutImageSopra('Tabelle',4);"  >
            <asp:ImageButton ID="imgTabelle" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/Tabella.png" ToolTip="Gestione tabelle" />
        </div>
        <div id="Statistiche" runat="server"
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Statistiche',5);" 
            onmouseout ="mouseOutImageSopra('Statistiche',5);"  >
            <asp:ImageButton ID="imgStatistiche" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" />
        </div>
        <div id="StatisticheSistema" runat="server"
            class="linguetta"
            onmouseover ="mouseOverImageSopra('StatisticheSistema',10);" 
            onmouseout ="mouseOutImageSopra('StatisticheSistema',10);"  >
            <asp:ImageButton ID="imgStatSistema" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/Classifiche.png" ToolTip="Statistiche Sistema" />
        </div>
        <div id="Mappa" runat="server"
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Mappa',6);" 
            onmouseout ="mouseOutImageSopra('Mappa',6);"  >
            <asp:ImageButton ID="imgMappa" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/mappa.png" ToolTip="Mappa lavori" />
        </div>
        <div id="Ricerca" runat="server"
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Ricerca',8);" 
            onmouseout ="mouseOutImageSopra('Ricerca',8);"  >
            <asp:ImageButton ID="imgRicerca" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_CERCA.png" ToolTip="Ricerca nelle note" />
        </div>
        <div id="Pulizia" runat="server"
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Pulizia',9);" 
            onmouseout ="mouseOutImageSopra('Pulizia',9);"  >
            <asp:ImageButton ID="imgPulizia" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Pulisce i mesi pregressi" />
        </div>
    </div>

    <div class="barraMesi">
        <%--<div style="margin-top: 5px; float: left; margin-left: 38%; ">--%>
            <asp:Label ID="Label1" runat="server" Text="Mese" CssClass="etichettaTitolo"></asp:Label>&nbsp;
            <asp:DropDownList ID="cmbMese" runat="server" AutoPostBack="true" ></asp:DropDownList>&nbsp;
            <asp:Label ID="Label2" runat="server" Text="Anno" CssClass="etichettaTitolo"></asp:Label>&nbsp;
            <asp:DropDownList ID="cmbAnno" runat="server" AutoPostBack="true" ></asp:DropDownList>
            <select id="flipSelect" ></select>
<%--         </div>
       <div style="float: left; margin-left: 10px; ">
            <asp:ImageButton ID="imgImpostaMeseAnno" runat="server" width="30px" Height="30px" ImageUrl ="App_Themes/Standard/Images/icona-EQUALIZZATORE.png" ToolTip="Imposta" />
        </div>--%>
    </div>

    <div style="align-content: center;">
        <div id="bookflip" runat="server" class="Quadernone" >
        </div>    
        <div class="barraRicorrenza">
            <div id="RicoS" runat="server" class="RicorrenzaS" onmouseover ="VisuaRicorrenza();" onmouseout ="NONVisuaRicorrenza();">
                <asp:Label ID="lblRicorrenzaS" runat="server" CssClass="etichettaRico" Text="Label"></asp:Label>
            </div>
            <div id="RicoD" runat="server" class="RicorrenzaD" onmouseover ="VisuaRicorrenza();" onmouseout ="NONVisuaRicorrenza();">
                <asp:Label ID="lblRicorrenzaD" runat="server" CssClass="etichettaRico" Text="Label"></asp:Label>
            </div>
        </div>
    </div>

    <div id="divBarraSin" runat="server" style="width:3px; height: 35%; top: 35%; left:0px; position: absolute; ">
        <div id="modSX" runat="server" 
            class="linguettaSin"
            onmouseover ="mouseOverImageSin('modSX',1);" 
            onmouseout ="mouseOutImageSin('modSX',1);"  >
            <asp:ImageButton ID="imgModSX" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/matitasx.png" ToolTip="Modifica" />
        </div>
        <div id="eliminaSX" runat="server" 
            class="linguettaSin"
            onmouseover ="mouseOverImageSin('eliminaSX',2);" 
            onmouseout ="mouseOutImageSin('eliminaSX',2);"  >
            <asp:ImageButton ID="imgEliSX" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/eliminasx.png" ToolTip="Elimina" />
        </div>
        <div id="noteSX" runat="server" 
            class="linguettaSin"
            onmouseover ="mouseOverImageSin('noteSX',3);" 
            onmouseout ="mouseOutImageSin('noteSX',3);"  >
            <asp:ImageButton ID="imgNoteSX" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/notesx.png" ToolTip="Gestione note" />
        </div>
        <div id="dettDaySX" runat="server" 
            class="linguettaSin"
            onmouseover ="mouseOverImageSin('dettDaySX',4);" 
            onmouseout ="mouseOutImageSin('dettDaySX',4);"  >
            <asp:ImageButton ID="imgDettaglioGiornoSX" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/latlng.jpg" ToolTip="Dettaglio giorno" />
        </div>
    </div>

    <div id="divBarraDes" runat="server" style="width:3px; height: 35%; top: 35%; left:99%; position: absolute; ">
        <div id="modDx" runat="server" 
            class="linguettaDes"
            onmouseover ="mouseOverImageDes('modDx',1);" 
            onmouseout ="mouseOutImageDes('modDx',1);"  >
            <asp:ImageButton ID="imgModDX" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica" />
        </div>
        <div id="eliminaDX" runat="server" 
            class="linguettaDes"
            onmouseover ="mouseOverImageDes('eliminaDX',2);" 
            onmouseout ="mouseOutImageDes('eliminaDX',2);"  >
            <asp:ImageButton ID="imgEliDX" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" />
        </div>
        <div id="noteDX" runat="server" 
            class="linguettaDes"
            onmouseover ="mouseOverImageDes('noteDX',3);" 
            onmouseout ="mouseOutImageDes('noteDX',3);"  >
            <asp:ImageButton ID="imgNoteDX" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/notedx.png" ToolTip="Gestione note" />
        </div>
        <div id="dettDayDX" runat="server" 
            class="linguettaDes"
            onmouseover ="mouseOverImageDes('dettDayDX',4);" 
            onmouseout ="mouseOutImageDes('dettDayDX',4);"  >
            <asp:ImageButton ID="imgDettaglioGiornoDX" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/latlng.jpg" ToolTip="Dettaglio giorno" />
        </div>
    </div>

    <div id="pages" style="width: 1px; height: 1px; overflow: hidden; ">
        <div name="Giorno 0" <%-- style="margin: 5px;" --%> >
            <img id="img0" runat="server" src="App_Themes/Standard/Images/Giorni/0.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 1" <%-- style="margin: 5px;" --%> >
            <img id="img1" runat="server" src="App_Themes/Standard/Images/Giorni/1.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 2" <%-- style="margin: 5px;" --%> >
            <img id="img2" runat="server" src="App_Themes/Standard/Images/Giorni/2.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 3" <%-- style="margin: 5px;" --%> >
            <img id="img3" runat="server" src="App_Themes/Standard/Images/Giorni/3.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 4" <%-- style="margin: 5px;" --%> >
            <img id="img4" runat="server" src="App_Themes/Standard/Images/Giorni/4.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 5" <%-- style="margin: 5px;" --%> >
            <img id="img5" runat="server" src="App_Themes/Standard/Images/Giorni/5.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 6" <%-- style="margin: 5px;" --%> >
            <img id="img6" runat="server" src="App_Themes/Standard/Images/Giorni/6.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 7" <%-- style="margin: 5px;" --%> >
            <img id="img7" runat="server" src="App_Themes/Standard/Images/Giorni/7.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 8" <%-- style="margin: 5px;" --%> >
            <img id="img8" runat="server" src="App_Themes/Standard/Images/Giorni/8.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 9" <%-- style="margin: 5px;" --%> >
            <img id="img9" runat="server" src="App_Themes/Standard/Images/Giorni/9.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 10" <%-- style="margin: 5px;" --%> >
            <img id="img10" runat="server" src="App_Themes/Standard/Images/Giorni/10.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 11" <%-- style="margin: 5px;" --%> >
            <img id="img11" runat="server" src="App_Themes/Standard/Images/Giorni/11.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 12" <%-- style="margin: 5px;" --%> >
            <img id="img12" runat="server" src="App_Themes/Standard/Images/Giorni/12.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 13" <%-- style="margin: 5px;" --%> >
            <img id="img13" runat="server" src="App_Themes/Standard/Images/Giorni/13.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 14" <%-- style="margin: 5px;" --%> >
            <img id="img14" runat="server" src="App_Themes/Standard/Images/Giorni/14.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 15" <%-- style="margin: 5px;" --%> >
            <img id="img15" runat="server" src="App_Themes/Standard/Images/Giorni/15.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 16" <%-- style="margin: 5px;" --%> >
            <img id="img16" runat="server" src="App_Themes/Standard/Images/Giorni/16.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 17" <%-- style="margin: 5px;" --%> >
            <img id="img17" runat="server" src="App_Themes/Standard/Images/Giorni/17.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 18" <%-- style="margin: 5px;" --%> >
            <img id="img18" runat="server" src="App_Themes/Standard/Images/Giorni/18.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 19" <%-- style="margin: 5px;" --%> >
            <img id="img19" runat="server" src="App_Themes/Standard/Images/Giorni/19.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 20" <%-- style="margin: 5px;" --%> >
            <img id="img20" runat="server" src="App_Themes/Standard/Images/Giorni/20.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 21" <%-- style="margin: 5px;" --%> >
            <img id="img21" runat="server" src="App_Themes/Standard/Images/Giorni/21.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 22" <%-- style="margin: 5px;" --%> >
            <img id="img22" runat="server" src="App_Themes/Standard/Images/Giorni/22.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 23" <%-- style="margin: 5px;" --%> >
            <img id="img23" runat="server" src="App_Themes/Standard/Images/Giorni/23.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 24" <%-- style="margin: 5px;" --%> >
            <img id="img24" runat="server" src="App_Themes/Standard/Images/Giorni/24.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 25" <%-- style="margin: 5px;" --%> >
            <img id="img25" runat="server" src="App_Themes/Standard/Images/Giorni/25.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 26" <%-- style="margin: 5px;" --%> >
            <img id="img26" runat="server" src="App_Themes/Standard/Images/Giorni/26.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 27" <%-- style="margin: 5px;" --%> >
            <img id="img27" runat="server" src="App_Themes/Standard/Images/Giorni/27.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 28" <%-- style="margin: 5px;" --%> >
            <img id="img28" runat="server" src="App_Themes/Standard/Images/Giorni/28.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 29" <%-- style="margin: 5px;" --%> >
            <img id="img29" runat="server" src="App_Themes/Standard/Images/Giorni/29.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 30" <%-- style="margin: 5px;" --%> >
            <img id="img30" runat="server" src="App_Themes/Standard/Images/Giorni/30.jpg" style="width: 99%; height: 99%;" />
        </div>
        <div name="Giorno 31" <%-- style="margin: 5px;" --%> >
            <img id="img31" runat="server" src="App_Themes/Standard/Images/Giorni/31.jpg" style="width: 99%; height: 99%;" />
        </div>
    </div>

    <%--<div style="position: absolute; top: 0px; left: 0px; width: 80px; height: 749px; background: url(http://localhost:63464/App_Themes/Standard/Images/black_gradientr.png) 100% 0% repeat-y transparent;"></div>--%>

    <script type="text/javascript">
        pWidth = x; //width of each page
        pHeight = y; //height of each page

        numPixels = 20;  //size of block in pixels to move each pass
        pSpeed = 20; //speed of animation, more is slower

        //startingPage = "2";//select page to start from, for last page use "e", eg. startingPage="e"
        allowAutoflipFromUrl = true; //true allows querystring in url eg bookflip.html?autoflip=5

        pageBackgroundColor = "#CCCCCC";
        pageFontColor = "#ffffff";

        pageBorderWidth = "2";
        pageBorderColor = "#3D4D5D";
        pageBorderStyle = "solid";  //dotted, dashed, solid, double, groove, ridge, inset, outset, dotted solid double dashed, dotted solid

        pageShadowLeftImgUrl = "App_Themes/Standard/Images/black_gradient.png";
        pageShadowRightImgUrl = "App_Themes/Standard/Images/black_gradientr.png";
        pageShadowWidth = 80;
        pageShadowOpacity = 60;
        pageShadow = 1 //0=shadow off, 1= shadow on left page

        allowPageClick = true; //allow page turn by clicking the page directly
        allowNavigation = true; //this builds a drop down list of pages for auto navigation.
        pageNumberPrefix = "Giorno "; //displays in the drop down list of pages if enabled

        ini();
    </script> 

    <div id="divNote" runat="server" class="popupTesto ">
        <asp:HiddenField ID="hdnGiornoNote" runat="server" />
        <asp:Label ID="Label3" runat="server" Text="Note" CssClass ="etichetta"></asp:Label>
        <hr />
        <asp:TextBox ID="txtNotelle" runat="server" CssClass="casellanote" TextMode="MultiLine"></asp:TextBox>
        <hr />
        <asp:Button ID="cmdOK" runat="server" Text="OK" CssClass="bottone" />
        <asp:Button ID="cmdAnnulla" runat="server" Text="Annulla" CssClass="bottone" />
    </div>

    <div id="divRicercaNote" runat="server" class="popupTestoRicerca ">
        <asp:Label ID="Label4" runat="server" Text="Ricerca note" CssClass ="etichetta"></asp:Label>
        <hr />
        <asp:TextBox ID="txtRicerca" runat="server" CssClass="testo" ></asp:TextBox>
        <asp:Button ID="cmdRicerca" runat="server" Text="Ricerca" CssClass="bottone" />
        <hr />
            <asp:Label ID="lblRicercati" runat="server" Text="" CssClass ="etichetta"></asp:Label>
            <asp:GridView id="grdRicerca" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                PageSize="5" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="5" 
                PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                <Columns>
                    <asp:BoundField DataField="Giorno" HeaderText="Giorno" >
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Nota" HeaderText="Nota" >
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        <hr />
        <asp:Button ID="cmdChiude" runat="server" Text="Annulla" CssClass="bottone" />
    </div>

    <div id="divBlocca" runat="server" class="bloccafinestra"></div>
    <div id="divDettaglioGiorno" runat="server" class="popupDettaglioGiorno">
        <asp:Label ID="lblDettaglio" runat="server" Text="Dettaglio giorno" CssClass ="etichetta"></asp:Label>
        &nbsp;&nbsp;&nbsp;
        <asp:Button ID="cmdChiudeDD" runat="server" Text="Chiude" CssClass="bottone" />
        <div style="width: 100%; height: 93%; margin-top: 5px;">
            <div id="map_canvas" runat="server" style="z-index: 100; width: 99%; height: 100%; border: 1px solid #aaaaaa; display: block; float: left;"></div>
<%--            <div id="divStatistiche" runat="server" style="z-index: 100; width: 49%; height: 100%; border: 1px solid #aaaaaa; display: block; float: left; margin-left: 5px;">
                <div style="width:100%; height: 100%; display: block; overflow: auto;">
                    <asp:TreeView ID="tvDati" runat="server" CssClass ="Albero">
                    </asp:TreeView>
                </div>
            </div>--%>
        </div>
<%--        <div id="divTimeline" runat="server" style="width:99%; height: 25%; border: 1px solid  #808080; overflow: auto; display: block; margin-top: 3px;">
        </div>--%>
    </div>

    <div id="divImmagine" class="popupImmagine">
        <div class="bottoneChiudi" onclick="chiudiImmagine()"></div>
        <!-- <img id="imgImmagine" style="width: 100%; height: 100%;" /> -->
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax"  runat="server">
</asp:Content>
