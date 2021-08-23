<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="ModificaGiorno.aspx.vb" Inherits="Orari.ModificaGiorno" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <%--<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCJ6LqMv1zV5Z_-wrETyen4ltCfMubiCzI"></script>--%>
    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDFjUlTVu_YExgOXkxQczGDFEO3o1sMb-A&callback=initMap"></script>

    <script type="text/javascript">
        function calcRoute() {
            document.getElementById(PREFISSO + "cphCorpo_lblKMetri").value = "";

            var directionsService = new google.maps.DirectionsService();
            var directionDisplay;
            var map;

            var start = document.getElementById(PREFISSO + "cphCorpo_hdnIndirizzo").value;
            var end = document.getElementById(PREFISSO + "cphCorpo_cmbIndirizzo").value;

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

                    var position1 = response.routes[0].legs[0].start_location;
                    var lat1 = position1.k;
                    var lng1 = position1.D;

                    var posiz = new google.maps.LatLng(lat1, lng1);
                    var myOptions = {
                        zoom: 3,
                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                        center: posiz
                    }
                    map = new google.maps.Map(document.getElementById(PREFISSO + "cphCorpo_map_canvas"), myOptions);
                    directionsDisplay.setMap(map);

                    document.getElementById(PREFISSO + "cphCorpo_lblKMetri").value = response.routes[0].legs[0].distance.text;
                } else {
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCorpo" runat="server">
     <div id="map_canvas" runat="server" style="width: 1px; height: 1px; position: absolute;"></div>

    <div id="barraTasti" runat="server" class="barraTasti" >
        <div id="divUscita" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('divUscita',3);" 
            onmouseout ="mouseOutImageSopra('divUscita',3);" >
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
    </div>

    <div class="mascheramodifica">
        <ul class="ul">
            <li class="li">
                <asp:Label ID="Label1" runat="server" Text="Giorno" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:Label ID="lblGiorno" runat="server" Text="Label" CssClass ="etichettaTitolo2"></asp:Label>
            </li>
            <li class="li">
                <asp:Image ID="imgTempo" runat="server" Width="60" ImageUrl="App_Themes/Standard/Images/icona_AVANTI.png" />
                <asp:Image ID="imgSocieta" runat="server" Width="60" ImageUrl="App_Themes/Standard/Images/icona_AVANTI.png" style="width: 80px; height: 80px;" />
                <asp:Image ID="imgCommessa" runat="server" Width="60" ImageUrl="App_Themes/Standard/Images/icona_AVANTI.png" style="width: 80px; height: 80px;" />
                <asp:Label ID="lblMancanti" runat="server" Text="1" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtGradi2" runat="server" TextMode="MultiLine"></asp:TextBox>
                <%--<asp:ImageButton ID="imgAnnulla" runat="server" ImageUrl="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip ="Annulla" style="float: right; width: 50px;" />--%>
                <%--<asp:ImageButton ID="imgSalva" runat="server" ImageUrl="App_Themes/Standard/Images/icona_SALVA.png" ToolTip ="Salva i dati" style="float: right; width: 50px;"/>--%>
            </li>
            <li class="liTipoGiorno">
                <asp:RadioButton ID="optNormale" runat="server" class="option" Text="Lavoro" AutoPostBack="True" />
            </li>
            <li class="liTipoGiorno">
                <asp:RadioButton ID="optFerie" runat="server" class="option" Text="Ferie" AutoPostBack="True" />
            </li>
            <li class="liTipoGiorno">
                <asp:RadioButton ID="optGiornoPermesso" runat="server" class="option" Text="Giorno di permesso" AutoPostBack="True" />
            </li>
            <li class="liTipoGiorno">
                <asp:RadioButton ID="optMalattia" runat="server" class="option" Text="Malattia" AutoPostBack="True" />
            </li>
            <li class="liTipoGiorno">
                <asp:RadioButton ID="optAltro" runat="server" class="option" Text="Altro" AutoPostBack="True" />
            </li>
            <li class="liTipoGiorno">
                <asp:RadioButton ID="optLavoroDaCasa" runat="server" class="option" Text="Lavoro da casa" AutoPostBack="True" />
            </li>
        </ul>
        <hr />
        <ul class="ul" id="ulDati" runat="server">
            <li class="li">
                <asp:Label ID="Label2" runat="server" Text="Ore lavorative" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:Button ID="cmdIndietroOra" runat="server" Text="-" CssClass="bottone" />
                <asp:TextBox ID="txtOre" runat="server" CssClass ="casellatestopiccola" text="8" Enabled="False"></asp:TextBox>
                <asp:Button ID="cmdAvantiora" runat="server" Text="+" CssClass="bottone" />
            </li>
            <li class="li">
                <asp:Label ID="Label3" runat="server" Text="Orario d'entrata" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtOrario" runat="server" CssClass ="casellatestopiccola" text=""></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="lblPermesso" runat="server" Text="Permesso" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:Button ID="cmdIndietroPermesso" runat="server" Text="-" CssClass="bottone" />
                <asp:TextBox ID="txtPermesso" runat="server" CssClass ="casellatestopiccola" text="0" Enabled="False"></asp:TextBox>
                <asp:Button ID="cmdAvantipermesso" runat="server" Text="+" CssClass="bottone" />
            </li>
            <li class="li">
                <asp:Label ID="lblMalattia" runat="server" Text="Malattia" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:Button ID="cmdIndietroMalattia" runat="server" Text="-" CssClass="bottone" />
                <asp:TextBox ID="txtMalattia" runat="server" CssClass ="casellatestopiccola" text="0" Enabled="False"></asp:TextBox>
                <asp:Button ID="cmdAvantiMalattia" runat="server" Text="+" CssClass="bottone" />
            </li>
            <li class="li">
                <asp:Label ID="lblStraord" runat="server" Text="Straordinari" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:Button ID="cmdIndietroStraord" runat="server" Text="-" CssClass="bottone" />
                <asp:TextBox ID="txtStraordinari" runat="server" CssClass ="casellatestopiccola" text="0" Enabled="False"></asp:TextBox>
                <asp:Button ID="cmdAvantiStraord" runat="server" Text="+" CssClass="bottone" />
            </li>
            <li class="li">
                <a href="GestioneTempo.aspx?Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                <asp:Label ID="lblTempo" runat="server" Text="Tempo" CssClass ="etichettaLink"></asp:Label>
                </a>
                <asp:ImageButton ID="imgwsmeteo" runat="server" Width="30" ImageUrl="App_Themes/Standard/Images/icona_DOWNLOAD-TAG.png" style="height: 30px" />
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbTempo" runat="server" CssClass ="casellacombinata" AutoPostBack="True" OnSelectedIndexChanged="ImpostaImmagineTempo" ></asp:DropDownList>
                <br />
                <asp:Label ID="Label7" runat="server" Text="Gradi" CssClass ="etichettaTitolo"></asp:Label>
                <asp:TextBox ID="txtGradi" runat="server" CssClass ="casellatestopiccola" text="" ></asp:TextBox>
                <br />
            </li>
        </ul>
        <hr />
        <ul>
            <li class="li">
                <a href="GestioneSocieta.aspx?Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                <asp:Label ID="Label6" runat="server" Text="Società" CssClass ="etichettaLink"></asp:Label>
                </a>
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbSocieta" runat="server" CssClass ="casellacombinata" AutoPostBack="True" OnSelectedIndexChanged="ImpostaCommesseLavoro" ></asp:DropDownList>
            </li>
            <li class="li">
               <a href="GestioneCommesse.aspx?Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
               <asp:Label ID="Label5" runat="server" Text="Commessa" CssClass ="etichettaLink"></asp:Label>
               </a>
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbCommessa" runat="server" CssClass ="casellacombinata" AutoPostBack="True" OnSelectedIndexChanged="ImpostaImmagineCommessa" ></asp:DropDownList>
            </li>
            <li class="li">
                <asp:Label ID="Label12" runat="server" Text="Abitazione" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:HiddenField ID="hdnIndirizzo" runat="server" />                
                <asp:DropDownList ID="cmbIndirizzo" runat="server" CssClass ="casellacombinata" AutoPostBack="True" OnSelectedIndexChanged="LeggeKM"></asp:DropDownList>
            </li>
            <li class="li">
                <asp:Label ID="Label13" runat="server" Text="Distanza" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="lblKMetri" runat="server" CssClass ="casellatestopiccola" text="" ></asp:TextBox>
            </li>
        </ul>
        <hr />
        <ul class="ul">
           <li class="li" id="lipranzo" runat="server">
                <a href="GestionePortate.aspx?Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                <asp:Label ID="Label11" runat="server" Text="Pranzo" CssClass ="etichettaLink"></asp:Label>
                </a>
            </li>
            <li class="li" id="liportata" runat="server">
                <asp:DropDownList ID="cmbPortata" runat="server" CssClass ="casellacombinata" OnSelectedIndexChanged ="AggiungePortata" AutoPostBack ="true" ></asp:DropDownList>
            </li>
            <li class="li" id="liPranzo2" runat="server">
                <br />
                <asp:GridView id="grdPranzo" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                PageSize="3" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="3" 
                PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnIdRiga" runat="server" />
                            </ItemTemplate>                                        
                        </asp:TemplateField>

                        <asp:BoundField DataField="Portata" HeaderText="Portata" SortExpression="Portata" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgEliminaAnd" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                    OnClick ="EliminaPortata" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </li>
            <li class="li">
            </li>
        </ul>
        <ul class="ul" id="ulPasticca" runat="server">
            <li class="li">
                <a href="GestionePasticche.aspx?Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                <asp:Label ID="Label8" runat="server" Text="Pasticca" CssClass ="etichettaLink"></asp:Label>
                </a>
            </li>
            <li class="li">
                <asp:DropDownList ID="cmbPasticca" runat="server" CssClass ="casellacombinata"  ></asp:DropDownList>
            </li>
            <li class="li">
                <asp:Label ID="Label4" runat="server" Text="Note" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtNotelle" runat="server" CssClass="casellanote" TextMode="MultiLine"></asp:TextBox>
            </li>
        </ul>
        <ul class="ul" id="ulMezzi" runat="server">
            <li class="li">
                <a href="GestioneMezzi.aspx?Tipo=Andata&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                <asp:Label ID="Label9" runat="server" Text="Mezzi di andata" CssClass ="etichettaLink"></asp:Label>
                </a>
                <br />
                <asp:DropDownList ID="cmbAndata" runat="server" CssClass ="casellacombinata" ></asp:DropDownList>
                <asp:ImageButton id="imgAggiungeAndata" runat="server" width="40" 
                    ImageUrl ="App_Themes/Standard/Images/icona_SALVA.png" ToolTip="Aggiunge Andata" >
                </asp:ImageButton>
            </li>
            <li class="li">
                <br />
                <asp:GridView id="grdAndata" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                    PageSize="3" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="3" 
                    PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnIdRiga" runat="server" />
                            </ItemTemplate>                                        
                        </asp:TemplateField>

                        <asp:BoundField DataField="Mezzo" HeaderText="Mezzo" SortExpression="Mezzo" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgEliminaAnd" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                    OnClick ="EliminaMezzoAndata" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgSpostaGiuAndata" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/icona_GIU.png" ToolTip="Sposta giu" 
                                    OnClick ="SpostaGiuMezzoAndata" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgSpostaSuAndata" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/icona_SU.png" ToolTip="Sposta su" 
                                    OnClick ="SpostaSuMezzoAndata" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgModifica" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica mezzo andata" 
                                    OnClick ="ModificaMezzoAndata" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </li>
            <li class="li">
                <a href="GestioneMezzi.aspx?Tipo=Ritorno&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                <asp:Label ID="Label10" runat="server" Text="Mezzi di ritorno" CssClass ="etichettaLink"></asp:Label>
                </a>
                <br />
                <asp:DropDownList ID="cmbRitorno" runat="server" CssClass ="casellacombinata" ></asp:DropDownList>
                <asp:ImageButton id="imgAggiungeRit" runat="server" width="40" 
                    ImageUrl ="App_Themes/Standard/Images/icona_SALVA.png" ToolTip="Aggiunge Ritorno" >
                </asp:ImageButton>
            </li>
            <li class="li">
                <br />
                <asp:GridView id="grdRitorno" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
                PageSize="3" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="3" 
                PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnIdRiga" runat="server" />
                            </ItemTemplate>                                        
                        </asp:TemplateField>

                        <asp:BoundField DataField="Mezzo" HeaderText="Mezzo" SortExpression="Mezzo" >
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                        </asp:BoundField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgEliminaRit" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                    OnClick ="EliminaMezzoRitorno" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgSpostaGiuRitorno" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/icona_GIU.png" ToolTip="Sposta giu" 
                                    OnClick ="SpostaGiuMezzoRitorno" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgSpostaSuRitorno" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/icona_SU.png" ToolTip="Sposta su" 
                                    OnClick ="SpostaSuMezzoRitorno" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
 
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle CssClass="cella-testata-griglia" />
                            <ItemStyle CssClass="cella-elemento-griglia" />
                            <ItemTemplate>
                                <asp:ImageButton id="imgModifica" runat="server" width="30" 
                                    ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica mezzo ritorno" 
                                    OnClick ="ModificaMezzoRitorno" >
                                </asp:ImageButton>
                            </ItemTemplate>                                        
                        </asp:TemplateField>
                   </Columns>
                </asp:GridView>
                <br />
            </li>
        </ul>
     </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
