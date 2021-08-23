<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="GestioneCommesse.aspx.vb" Inherits="Orari.GestioneCommesse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCJ6LqMv1zV5Z_-wrETyen4ltCfMubiCzI"></script>--%>
    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDFjUlTVu_YExgOXkxQczGDFEO3o1sMb-A&callback=initMap"></script>

    <script type="text/javascript">
        function calcRoute() {
            document.getElementById(PREFISSO + "cphCorpo_txtLatLng").value = "";
            document.getElementById(PREFISSO + "cphCorpo_txtKm").value = "";

            var directionsService = new google.maps.DirectionsService();
            var directionDisplay;
            var map;

            var end = document.getElementById(PREFISSO + "cphCorpo_txtIndirizzo").value;
            var start = document.getElementById(PREFISSO + "cphCorpo_cmbIndirizzo").value;
            var request = {
                origin: start,
                destination: end,
                travelMode: google.maps.DirectionsTravelMode.DRIVING,
                unitSystem: google.maps.UnitSystem.METRIC
            };
            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay = new google.maps.DirectionsRenderer();

                    directionsDisplay.setDirections(response);

                    var p = response.routes[0].legs[0].end_location;
                    var lat1 = p.k;
                    var lng1 = p.D;

                    var posiz = new google.maps.LatLng(lat1, lng1);
                    var myOptions = {
                        zoom: 3,
                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                        center: posiz
                    }
                    map = new google.maps.Map(document.getElementById(PREFISSO + "cphCorpo_map_canvas"), myOptions);
                    directionsDisplay.setMap(map);

                    //alert(response.routes[0].legs[0].end_location);


                    //var n = p.indexOf(',');
                    //alert(endLocation.latlng);

                    //var lat1 = p.substr(1, p.indexOf(','));
                    //var lng1 = p.substr(p.indexOf(',')+2, p.length());

                    var ll = document.getElementById(PREFISSO + "cphCorpo_txtLatLng");
                    ll.value = p;
                    ll.value = ll.value.replace('(', '');
                    ll.value = ll.value.replace(')', '');

                    document.getElementById(PREFISSO + "cphCorpo_txtKm").value = response.routes[0].legs[0].distance.text;
                } else {
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphNoAjax" runat="server">
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
        <div id="divCambiaFoto" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('divCambiaFoto',5);" 
            onmouseout ="mouseOutImageSopraSA('divCambiaFoto',5);" >
            <asp:ImageButton ID="imgCambia" runat="server" ImageUrl="App_Themes/Standard/Images/visualizzato_tondo.png" ToolTip ="Cambia l'immagine della commessa" style="width: 50px;"/>
        </div>
        <div id="divAggiungeFoto" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('divAggiungeFoto',6);" 
            onmouseout ="mouseOutImageSopraSA('divAggiungeFoto',6);" >
            <asp:ImageButton ID="imgCaricaFoto" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_DOWNLOAD-TAG.png" ToolTip="Carica l'immagine" />
        </div>
    </div>

    <div id="divUpload" runat="server" class="popupTesto ">
        <asp:Label ID="lblTitoloUpload" runat="server" Text="File immagine della commessa" CssClass ="etichetta"></asp:Label>
        <hr />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <hr />
        <asp:Button ID="cmdOK" runat="server" Text="OK" CssClass="bottone" />
        <asp:Button ID="cmdAnnulla" runat="server" Text="Annulla" CssClass="bottone" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCorpo" runat="server">
    <asp:HiddenField ID="hdnNomeOriginalCommessa" runat="server" />
    <div class="mascheramodifica">
        <ul class="ul">
            <li class="li">
                <asp:Label ID="Label1" runat="server" Text="Società" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbSocieta" runat="server" CssClass ="casellacombinatapiccolac" OnSelectedIndexChanged ="SelezionaSocieta" AutoPostBack ="true" ></asp:DropDownList>
                <asp:Image ID="imgSocieta" runat="server" width="80" />
            </li>
<%--            <li class="liCentro">
            </li>
            <li class="li">
            </li>
        </ul>
        <ul class="ul">--%>
            <li class="li">
                <asp:Label ID="Label2" runat="server" Text="Commessa" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbCommessa" runat="server" CssClass ="casellacombinatapiccolac" OnSelectedIndexChanged ="SelezionaCommessa" AutoPostBack ="true" ></asp:DropDownList>
                <asp:Image ID="imgCommessa" runat="server" width="80" />
            </li>
<%--            <li class="liCentro">
            </li>
            <li class="li">
            </li>--%>
        </ul>
        <ul class="ul">
            <li class="li">
                <asp:Label ID="Label3" runat="server" Text="Nome Commessa" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtCommessa" runat="server" CssClass ="casellatesto" text="" MaxLength="50"></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="Label4" runat="server" Text="Codice Commessa" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtCodCommessa" runat="server" CssClass ="casellatesto" text="" MaxLength="10"></asp:TextBox>
           </li>
<%--            <li class="li">
                <asp:Label ID="Label5" runat="server" Text="Distanza in Km." CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtDistanza" runat="server" CssClass ="casellatesto" text="" MaxLength="6"></asp:TextBox>
           </li>
            <li class="li">
            </li>
            <li class="li">
           </li>--%>
            <li class="li">
                <asp:Label ID="Label13" runat="server" Text="Data Inizio" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtDataInizio" runat="server" CssClass ="casellatesto datepicker" text="" MaxLength="10"></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="Label14" runat="server" Text="Data Fine" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtDataFine" runat="server" CssClass ="casellatesto datepicker" text="" MaxLength="10"></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="Label18" runat="server" Text="LatLng" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtLatLng" runat="server" CssClass ="casellatesto" MaxLength="70" ></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="Label8" runat="server" Text="Distanza" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtKm" runat="server" CssClass ="casellatesto" MaxLength="10" ></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="Label17" runat="server" Text="Indirizzo di provenienza" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbIndirizzo" runat="server" CssClass ="casellacombinata" AutoPostBack ="true" OnSelectedIndexChanged="CDistanze"></asp:DropDownList>
            </li>
            <li class="li">
                <asp:Label ID="Label7" runat="server" Text="Indirizzo commessa" CssClass ="etichettaTitolo"></asp:Label>
                <asp:ImageButton ID="imgCalcolaLatLng" runat="server" width="40px" ImageUrl="App_Themes/Standard/Images/latlng.jpg" BorderColor="#AAAAAA" BorderStyle="Solid" BorderWidth="1" />
            </li>
            <li class="li">
                <asp:TextBox ID="txtIndirizzo" runat="server" CssClass ="casellatesto" text="" MaxLength="70"></asp:TextBox>
           </li>
            <li class="li">
                <asp:Label ID="lblDefault" runat="server" Text="Commessa di default" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:CheckBox ID="chkDefault" runat="server" CssClass="option" AutoPostBack ="true" OnCheckedChanged ="ImpostaCommessaDefault" />
            </li>
            <li class="li">
            </li>
            <li class="li">
            </li>
        </ul>
        <div id="map_canvas" runat="server" style="z-index: 100; width: 100%; height: 200px; border: 1px solid #aaaaaa;"></div>
    </div>

    <div id="MascheroneDettagli" runat="server" style="vertical-align: top;  overflow: hidden; ">
        <div id="divProgetti" runat="server" class="mascheramodificadett">
            <asp:HiddenField ID="hdnApplicazione" runat="server" />
            <asp:GridView id="grdApplicazioni" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                PageSize="5" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="5" 
                PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnIdRiga" runat="server" />
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:BoundField DataField="Applicazione" HeaderText="Applicazione" SortExpression="Applicazione" >
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Linguaggio" HeaderText="Linguaggio" SortExpression="Linguaggio" >
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgEliminaA" runat="server" width="20" 
                                ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                OnClick ="EliminaApplicazione" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgModificaA" runat="server" width="20" 
                                ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica" 
                                OnClick ="ModificaApplicazione" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Label ID="Label6" runat="server" Text="Attività" CssClass ="etichettaTitolo"></asp:Label>
            <asp:TextBox ID="txtApplicazione" runat="server" CssClass ="casellanote" text="" TextMode="MultiLine" ></asp:TextBox>
            <br />
            <asp:Label ID="Label9" runat="server" Text="Tipologia" CssClass ="etichettaTitolo"></asp:Label>
            <asp:DropDownList ID="cmbLinguaggi" runat="server"></asp:DropDownList>
            <br />
            <asp:ImageButton ID="imgSalvaL" runat="server" width="30px" Height="30px" ImageUrl ="App_Themes/Standard/Images/icona_SALVA.png" ToolTip="Salva i dati" />
            <asp:ImageButton ID="imgNuovoL" runat="server" width="30px" Height="30px" ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Nuovo valore" />
        </div>

        <div id="datiConoscenze" runat="server" class="mascheramodificadett">
            <asp:HiddenField ID="hdnIdLavoro" runat="server" />
            <asp:HiddenField ID="hdnIdConoscenza" runat="server" />
            <asp:HiddenField ID="hdnIdCommessa" runat="server" />
<%--            <ul class="ul">
                <li class="lidett">
                    <asp:Label ID="Label9" runat="server" Text="Conoscenze" CssClass ="etichettaTitolo"></asp:Label>
                    <hr />--%>
                    <asp:GridView id="grdConoscenze" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="5" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="5" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                        <Columns>
                            <asp:TemplateField HeaderText="">
                                <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                                <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnIdRiga" runat="server" />
                                </ItemTemplate>                                        
                            </asp:TemplateField>

                            <asp:BoundField DataField="Conoscenza" HeaderText="Conoscenza" SortExpression="Conoscenza" >
                                <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                                <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="">
                                <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                                <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                                <ItemTemplate>
                                    <asp:ImageButton id="imgEliminaC" runat="server" width="20" 
                                        ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                        OnClick ="EliminaConoscenza" >
                                    </asp:ImageButton>
                                </ItemTemplate>                                        
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="">
                                <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                                <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                                <ItemTemplate>
                                    <asp:ImageButton id="imgModificaC" runat="server" width="20" 
                                        ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica" 
                                        OnClick ="ModificaConoscenza" >
                                    </asp:ImageButton>
                                </ItemTemplate>                                        
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
<%--                </li>
                <li class="lidett">--%>
                    <hr />
                    <asp:Label ID="Label10" runat="server" Text="Nominativo" CssClass ="etichettaTitolo"></asp:Label>
                    <asp:TextBox ID="txtConoscenza" runat="server" CssClass ="casellatestomedia" text="" MaxLength="50"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label11" runat="server" Text="Telefono" CssClass ="etichettaTitolo"></asp:Label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass ="casellatestomedia" text="" MaxLength="50"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label12" runat="server" Text="EMail" CssClass ="etichettaTitolo"></asp:Label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass ="casellatestomedia" text="" MaxLength="50"></asp:TextBox>
                    <br />
                    <asp:ImageButton ID="imgSalvaC" runat="server" width="30px" Height="30px" ImageUrl ="App_Themes/Standard/Images/icona_SALVA.png" ToolTip="Salva i dati" />
                    <asp:ImageButton ID="imgNuovaC" runat="server" width="30px" Height="30px" ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Nuovo valore" />
<%--                </li>
            </ul>--%>
        </div>
        <div id="divFoto" runat="server" class="mascheramodificadett">
<%--            <asp:Label ID="Label15" runat="server" Text="Nome immagine" CssClass ="etichettaTitolo"></asp:Label>&nbsp;
            <asp:FileUpload ID="FileUpload2" runat="server" />
            <br />
            <asp:Label ID="Label16" runat="server" Text="Descrizione" CssClass ="etichettaTitolo"></asp:Label>
            <asp:TextBox ID="txtDescrizione" runat="server" CssClass ="casellatestomedia" text="" MaxLength="50"></asp:TextBox>
            <hr />--%>
            <asp:GridView id="grdFoto" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
            PageSize="5" CssClass="grigliaPiccola" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="5" 
            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                        <ItemTemplate>
                            <asp:Image runat="server" BorderColor="#AAAAAA" BorderStyle="Solid" BorderWidth="1" Height="70" Width="70" ID="imgImmagine"></asp:Image>  
                        </ItemTemplate>                                      
                    </asp:TemplateField>

                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" SortExpression="Descrizione" >
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgModificaF" runat="server" width="20" 
                                ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica" 
                                OnClick ="ModificaFoto" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia-piccola" />
                        <ItemStyle CssClass="cella-elemento-griglia-piccola" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgEliminaF" runat="server" width="20" 
                                ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                OnClick ="EliminaFoto" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgVisualizzaF" runat="server" width="20" 
                                ImageUrl ="App_Themes/Standard/Images/visualizzato_tondo.png" ToolTip="Visualizza foto" 
                                OnClick ="VisualizzaFoto" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <asp:HiddenField ID="hdnDescFoto" runat="server" />
            <asp:Label ID="Label5" runat="server" Text="Descrizione" CssClass ="etichettaTitolo"></asp:Label>
            <asp:TextBox ID="txtDescF" runat="server" CssClass ="casellatestomedia" text="" MaxLength="50"></asp:TextBox>
            <asp:ImageButton ID="imgModificaF" runat="server" width="30px" Height="30px" ImageUrl ="App_Themes/Standard/Images/icona_SALVA.png" ToolTip="Salva i dati" />
       </div>
    </div>

    <div id="divVisuaFotoBack" class="bloccafinestra" runat="server"></div>
    <div id="divVisuaFoto" runat="server" class="visuaImmagine">
        <asp:Label ID="lblNomeImmagine" runat="server" Text="File immagine della commessa" CssClass ="etichetta"></asp:Label>
        <br />
        <asp:Image runat="server" ImageUrl="App_Themes/Standard/Images/icona_DOWNLOAD-TAG.png" ID="imgVisuaFoto" ></asp:Image>  
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
            var divvinoCF = document.getElementById(PREFISSO + "cphNoAjax_divCambiaFoto");
            divvinoCF.style.display = come;
            var divvinoAF = document.getElementById(PREFISSO + "cphNoAjax_divAggiungeFoto");
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
            divvino.style.left = x + "px";
            divvino.style.top = y + "px";
            clearInterval(ritardo);
        }
    </script>
</asp:Content>
