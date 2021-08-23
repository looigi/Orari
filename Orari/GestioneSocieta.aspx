<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="GestioneSocieta.aspx.vb" Inherits="Orari.GestioneSocieta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCJ6LqMv1zV5Z_-wrETyen4ltCfMubiCzI"></script>--%>
    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDFjUlTVu_YExgOXkxQczGDFEO3o1sMb-A&callback=initMap"></script>

    <script type="text/javascript">
        function calcRoute() {
            var address = document.getElementById(PREFISSO + "cphCorpo_txtIndirizzo").value;

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
<asp:Content ID="Content2" ContentPlaceHolderID="cphNoAjax" runat="server">
    <div id="barraTasti" runat="server" class="barraTasti" >
        <div id="divUscita" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('divUscita',7);" 
            onmouseout ="mouseOutImageSopraSA('divUscita',7);" >
            <asp:ImageButton ID="imgUscita" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_USCITA.png" ToolTip="Aggiorna i dati" />
        </div>
        <div id="Indietro" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('Indietro',1);" 
            onmouseout ="mouseOutImageSopraSA('Indietro',1);" >
            <asp:ImageButton ID="imgAnnulla" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip="Indietro" />
        </div>
        <div id="Salva" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('Salva',2);" 
            onmouseout ="mouseOutImageSopraSA('Salva',2);" >
            <asp:ImageButton ID="imgSalva" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_SALVA.png" ToolTip="Salva i dati" />
        </div>
        <div id="Nuovo" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('Nuovo',3);" 
            onmouseout ="mouseOutImageSopraSA('Nuovo',3);" >
            <asp:ImageButton ID="imgNuova" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Nuovo valore" />
        </div>
        <div id="Elimina" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('Elimina',4);" 
            onmouseout ="mouseOutImageSopraSA('Elimina',4);" >
            <asp:ImageButton ID="imgElimina" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina valore" />
        </div>
        <div id="CambiaImmagine" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('CambiaImmagine',5);" 
            onmouseout ="mouseOutImageSopraSA('CambiaImmagine',5);" >
            <asp:ImageButton ID="imgCambia" runat="server" ImageUrl="App_Themes/Standard/Images/visualizzato_tondo.png" ToolTip ="Cambia immagine società" style="width: 50px;"/>
        </div>
        <div id="divCaricaFoto" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('divCaricaFoto',6);" 
            onmouseout ="mouseOutImageSopraSA('divCaricaFoto',6);" >
            <asp:ImageButton ID="imgCaricaFoto" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_DOWNLOAD-TAG.png" ToolTip="Carica una nuova immagine" />
        </div>
    </div>

    <div id="divUpload" runat="server" class="popupTesto ">
        <asp:Label ID="Label6" runat="server" Text="File immagine della società" CssClass ="etichetta"></asp:Label>
        <hr />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <hr />
        <asp:Button ID="cmdOK" runat="server" Text="OK" CssClass="bottone" />
        <asp:Button ID="cmdAnnulla" runat="server" Text="Annulla" CssClass="bottone" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCorpo" runat="server">
    <asp:HiddenField ID="hdnNomeOriginaleSocieta" runat="server" />
    <div class="mascheramodifica">
        <ul class="ul">
            <li class="li">
                <asp:Label ID="Label1" runat="server" Text="Società" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbSocieta" runat="server" CssClass ="casellacombinata" OnSelectedIndexChanged ="SelezionaSocieta" AutoPostBack ="true" ></asp:DropDownList>
                <br />
                <asp:Label ID="Label7" runat="server" Text="Usa default" CssClass ="etichettaTitolo"></asp:Label>
                <asp:CheckBox ID="chkDefault" runat="server" CssClass="option" AutoPostBack ="true" OnCheckedChanged ="ImpostaSocietaDefault" />
            </li>
            <li class="liCentro">
                <asp:Image ID="imgSocieta" runat="server" width="80"  />
            </li>
            <li class="li">
            </li>
        </ul>
        <ul class="ul">
            <li class="li">
                <asp:Label ID="Label2" runat="server" Text="Nome Società" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtSocieta" runat="server" CssClass ="casellatesto" text="" MaxLength="30"></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="Label3" runat="server" Text="Indirizzo" CssClass ="etichettaTitolo"></asp:Label>
                <asp:ImageButton ID="imgCalcolaLatLng" runat="server" width="40px" ImageUrl="App_Themes/Standard/Images/latlng.jpg" BorderColor="#AAAAAA" BorderStyle="Solid" BorderWidth="1" />
            </li>
            <li class="li">
                <asp:TextBox ID="txtIndirizzo" runat="server" CssClass ="casellatesto" text="" MaxLength="100"></asp:TextBox>
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
                <asp:TextBox ID="txtLatLng" runat="server" CssClass ="casellatesto" MaxLength="70" ></asp:TextBox>
            </li>
            <li class="li">
            </li>
            <li class="li">
           </li>
        </ul>
        <div id="map_canvas" runat="server" style="z-index: 100; width: 100%; height: 200px; border: 1px solid #aaaaaa;"></div>
    </div>

    <br />
    <div id="MascheroneDettagli" runat="server" style="vertical-align: top;  overflow: auto; ">
        <div id="datiConoscenze" runat="server" class="mascheramodificadett">
            <asp:HiddenField ID="hdnIdLavoro" runat="server" />
            <asp:HiddenField ID="hdnIdConoscenza" runat="server" />
            <ul class="ul">
                <li class="lidett">
<%--                    <asp:Label ID="Label8" runat="server" Text="Conoscenze" CssClass ="etichettaTitolo"></asp:Label>
                    <hr />--%>
                    <asp:GridView id="grdConoscenze" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="5" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="5" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle CssClass="cella-testata-griglia" />
                                <ItemStyle CssClass="cella-elemento-griglia" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnIdRiga" runat="server" />
                                </ItemTemplate>                                        
                            </asp:TemplateField>

                            <asp:BoundField DataField="Conoscenza" HeaderText="Conoscenza" SortExpression="Conoscenza" >
                                <HeaderStyle CssClass="cella-testata-griglia" />
                                <ItemStyle CssClass="cella-elemento-griglia" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="">
                                <HeaderStyle CssClass="cella-testata-griglia" />
                                <ItemStyle CssClass="cella-elemento-griglia" />
                                <ItemTemplate>
                                    <asp:ImageButton id="imgEliminaC" runat="server" width="30" 
                                        ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                        OnClick ="EliminaConoscenza" >
                                    </asp:ImageButton>
                                </ItemTemplate>                                        
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="">
                                <HeaderStyle CssClass="cella-testata-griglia" />
                                <ItemStyle CssClass="cella-elemento-griglia" />
                                <ItemTemplate>
                                    <asp:ImageButton id="imgModificaC" runat="server" width="30" 
                                        ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica" 
                                        OnClick ="ModificaConoscenza" >
                                    </asp:ImageButton>
                                </ItemTemplate>                                        
                           </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </li>
                <li class="lidett">
                    <asp:Label ID="Label10" runat="server" Text="Nominativo" CssClass ="etichettaTitolo"></asp:Label>
                    <asp:TextBox ID="txtConoscenza" runat="server" CssClass ="casellatesto" text="" MaxLength="50"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label11" runat="server" Text="Telefono" CssClass ="etichettaTitolo"></asp:Label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass ="casellatesto" text="" MaxLength="50"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label12" runat="server" Text="EMail" CssClass ="etichettaTitolo"></asp:Label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass ="casellatesto" text="" MaxLength="50"></asp:TextBox>
                </li>
                <li class="lidett">
                    <asp:ImageButton ID="imgSalvaC" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_SALVA.png" ToolTip="Salva i dati" />
                    <asp:ImageButton ID="imgNuovaC" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Nuovo valore" />
                </li>
            </ul>
        </div>
        <div id="divFoto" runat="server" class="mascheramodificadett">
            <asp:Label ID="Label14" runat="server" Text="Nome immagine" CssClass ="etichettaTitolo"></asp:Label>
            <asp:FileUpload ID="FileUpload2" runat="server" />
            <br />
            <asp:Label ID="Label13" runat="server" Text="Descrizione" CssClass ="etichettaTitolo"></asp:Label>
            <asp:TextBox ID="txtDescrizione" runat="server" CssClass ="casellatestomedia" text="" MaxLength="50"></asp:TextBox>
            <hr />
            <asp:GridView id="grdFoto" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
            PageSize="5" CssClass="grigliaPiccola" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="5" 
            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:Image runat="server" BorderColor="#AAAAAA" BorderStyle="Solid" BorderWidth="1" Height="70" Width="70" ID="imgImmagine"></asp:Image>  
                        </ItemTemplate>                                      
                    </asp:TemplateField>

                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" SortExpression="Descrizione" >
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgEliminaF" runat="server" width="30" 
                                ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                OnClick ="EliminaFoto" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgVisualizzaF" runat="server" width="30" 
                                ImageUrl ="App_Themes/Standard/Images/visualizzato_tondo.png" ToolTip="Visualizza foto" 
                                OnClick ="VisualizzaFoto" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div id="divVisuaFotoBack" class="bloccafinestra" runat="server"></div>
    <div id="divVisuaFoto" runat="server" class="visuaImmagine">
        <asp:Label ID="lblNomeImmagine" runat="server" Text="File immagine della società" CssClass ="etichetta"></asp:Label>
        <br />
        <asp:Image runat="server" BorderColor="#AAAAAA" BorderStyle="Solid" BorderWidth="2" ID="imgVisuaFoto"></asp:Image>  
    </div>

    <script type="text/javascript" >
        var ritardo;

        function SpegneTasti(come) {
            var divvinoN = document.getElementById(PREFISSO + "cphNoAjax_Nuovo");
            divvinoN.style.display = come;
            var divvinoE = document.getElementById(PREFISSO + "cphNoAjax_Elimina");
            divvinoE.style.display = come;
            var divvinoS = document.getElementById(PREFISSO + "cphNoAjax_Salva");
            divvinoS.style.display = come;
            var divvinoCF = document.getElementById(PREFISSO + "cphNoAjax_CambiaImmagine");
            divvinoCF.style.display = come;
            var divvinoAF = document.getElementById(PREFISSO + "cphNoAjax_divCaricaFoto");
            divvinoAF.style.display = come;
        }

        function SistemaImmagine() {
            SpegneTasti("none");

            ritardo = setInterval(function () { SpostaDivImmagine() }, 3000);
        }

        function SpostaDivImmagine() {
            var divvino = document.getElementById(PREFISSO + "cphCorpo_divVisuaFoto");
            var immagine = document.getElementById(PREFISSO + "cphCorpo_divVisuaFoto");
            var xImm = immagine.clientWidth / 2;
            var yImm = immagine.clientHeight / 2;
            var x = (screen.width / 2) - xImm;
            var y = (screen.height / 2) - yImm;
            //alert(x + "-" + y);
            y -= 20;
            divvino.style.left=x+"px";
            divvino.style.top = y + "px";
            clearInterval(ritardo);
        }
    </script>
</asp:Content>
