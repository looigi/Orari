<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="GestionePasticche.aspx.vb" Inherits="Orari.GestionePasticche" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            <asp:ImageButton ID="imgNuovo" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Nuovo valore" />
        </div>
    </div>

    <div class="mascheramodifica">
        <asp:HiddenField ID="hdnNumero" runat="server" />
        <div class="titolomaschera">
            <asp:Label ID="lblQuante" runat="server" Text="Portata" CssClass ="etichettaTitolo" style="float:right;"></asp:Label>
            <br />
            <br />
            <asp:GridView id="grdPasticche" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
            PageSize="10" CssClass="grigliaPiccola" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="10" 
            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnIdRiga" runat="server" />
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:BoundField DataField="Pasticca" HeaderText="Pasticca" SortExpression="Pasticca" >
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Ricorrenze" HeaderText="Ricorrenze" SortExpression="Ricorrenze" >
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia-destra" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgElimina" runat="server" width="30" 
                                ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina" 
                                OnClick ="EliminaPasticca" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgModifica" runat="server" width="30" 
                                ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica" 
                                OnClick ="ModificaPasticca" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>
 
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                OnClick="StatistichePasticche" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>
               </Columns>
            </asp:GridView>
            <br />
        </div>
        <ul class="ul">
            <li class="li">
<%--                <asp:ImageButton ID="imgAnnulla" runat="server" ImageUrl="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip ="Annulla" style="float: right; width: 50px;" />
                <asp:ImageButton ID="imgSalva" runat="server" ImageUrl="App_Themes/Standard/Images/icona_SALVA.png" ToolTip ="Salva i dati" style="float: right; width: 50px;"/>--%>
            </li>
            <li class="li">
                <asp:Label ID="Label3" runat="server" Text="Pasticca" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtPasticca" runat="server" CssClass ="casellatesto" text="" MaxLength="40"></asp:TextBox>
            </li>
            <li class="li">
                <%--<asp:ImageButton ID="imgNuovo" runat="server" ImageUrl="App_Themes/Standard/Images/matitadx.png" ToolTip ="Nuova" style="width: 50px;" />--%>
            </li>
        </ul>
    </div>

    <div id="divStatistiche" runat="server" class="Statistiche">
        <asp:Label ID="lblStatPasticche" runat="server" Text="Mezzo" CssClass ="etichettaTitolo"></asp:Label>
        <hr />
        <asp:GridView id="grdStatistiche" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
        PageSize="8" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="8" 
        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
            <Columns>
                <asp:BoundField DataField="Pasticca" HeaderText="Pasticca" SortExpression="Pasticca" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <hr />
        <asp:ImageButton ID="imgChiudeStat" runat="server" width="50px" Height="50px" style="float: right; padding-top: 5px;" ImageUrl ="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip="Indietro" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
