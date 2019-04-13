<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="GestioneMezzi.aspx.vb" Inherits="Orari.GestioneMezzi" %>
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
        <asp:Label ID="lblQuante" runat="server" Text="Portata" CssClass ="etichettaTitolo" style="float:right;"></asp:Label>
        <br />
        <br />
        <asp:GridView id="grdMezzi" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
        PageSize="5" CssClass="grigliaPiccola" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="5" 
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
                            OnClick ="EliminaMezzo" >
                        </asp:ImageButton>
                    </ItemTemplate>                                        
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                    <ItemTemplate>
                        <asp:ImageButton id="imgModifica" runat="server" width="30" 
                            ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica" 
                            OnClick ="ModificaMezzo" >
                        </asp:ImageButton>
                    </ItemTemplate>                                        
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                    <ItemTemplate>
                        <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                            ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                            OnClick ="StatisticheMezzo" >
                        </asp:ImageButton>
                    </ItemTemplate>                                        
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <hr />
        <ul class="ul">
            <li class="li">
                <asp:Label ID="Label3" runat="server" Text="Mezzo" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtMezzo" runat="server" CssClass ="casellatesto" text="" MaxLength="30"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label1" runat="server" Text="Dettaglio" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtDettaglio" runat="server" CssClass ="casellatesto" text="" MaxLength="30"></asp:TextBox>
            </li>
            <li class="li">
            </li>
            <li class="li">
            </li>
            <li class="li">
            </li>
            <li class="li">
<%--                <asp:ImageButton ID="imgAnnulla" runat="server" ImageUrl="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip ="Annulla" style="float: right; width: 50px;" />
                <asp:ImageButton ID="imgSalva" runat="server" ImageUrl="App_Themes/Standard/Images/icona_SALVA.png" ToolTip ="Salva i dati" style="float: right; width: 50px;"/>
                <asp:ImageButton ID="imgNuovo" runat="server" ImageUrl="App_Themes/Standard/Images/matitadx.png" ToolTip ="Nuova" style="float: right; width: 50px;" />--%>
            </li>
        </ul>
        
        <hr />
        <asp:DropDownList ID="cmbStandard" runat="server" CssClass ="casellacombinatapiccola" OnSelectedIndexChanged ="AggiungeMezzoStandard" AutoPostBack ="true" style="float: right;"></asp:DropDownList>
        <asp:Label ID="lblSotto" runat="server" Text="Mezzi standard" CssClass ="etichettaTitolo" style="float:right;"></asp:Label>
        <br />
        <br />
        <asp:GridView id="grdStandard" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
        PageSize="4" CssClass="grigliaPiccola" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="4" 
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
                            OnClick ="EliminaMezzoStandard" >
                        </asp:ImageButton>
                    </ItemTemplate>                                        
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                    <ItemTemplate>
                        <asp:ImageButton id="imgSpostaGiuAndata" runat="server" width="30" 
                            ImageUrl ="App_Themes/Standard/Images/icona_GIU.png" ToolTip="Sposta giu" 
                            OnClick ="SpostaGiuMezzoStandard" >
                        </asp:ImageButton>
                    </ItemTemplate>                                        
                </asp:TemplateField>

                <asp:TemplateField HeaderText="">
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                    <ItemTemplate>
                        <asp:ImageButton id="imgSpostaSuAndata" runat="server" width="30" 
                            ImageUrl ="App_Themes/Standard/Images/icona_SU.png" ToolTip="Sposta su" 
                            OnClick ="SpostaSuMezzoStandard" >
                        </asp:ImageButton>
                    </ItemTemplate>                                        
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>

    <div id="divStatistiche" runat="server" class="Statistiche">
        <asp:Label ID="lblStatMezzo" runat="server" Text="Mezzo" CssClass ="etichettaTitolo"></asp:Label>
        <hr />
        <asp:GridView id="grdStatistiche" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
        PageSize="8" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="8" 
        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
            <Columns>
                <asp:BoundField DataField="Mezzo" HeaderText="Mezzo" SortExpression="Mezzo" >
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
