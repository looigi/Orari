<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="GestioneTempo.aspx.vb" Inherits="Orari.GestioneTempo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCorpo" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
    <div id="barraTasti" runat="server" class="barraTasti" >
        <div id="divUscita" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('divUscita',4);" 
            onmouseout ="mouseOutImageSopraSA('divUscita',4);" >
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
            <asp:ImageButton ID="imgNuovo" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Nuovo valore" />
        </div>
    </div>

    <div class="mascheramodifica">
        <asp:HiddenField ID="hdnNumero" runat="server" />
        <div class="titolomaschera">
            <asp:Label ID="lblQuante" runat="server" Text="Tipologie" CssClass ="etichettaTitolo" style="float:right;"></asp:Label>
            <br />
            <br />
            <asp:GridView id="grdTempo" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
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

                    <asp:BoundField DataField="Tempo" HeaderText="Tempo" SortExpression="tempo" >
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
                                OnClick ="EliminaTempo" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgModifica" runat="server" width="30" 
                                ImageUrl ="App_Themes/Standard/Images/matitadx.png" ToolTip="Modifica" 
                                OnClick ="ModificaTempo" >
                            </asp:ImageButton>
                        </ItemTemplate>                                        
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <HeaderStyle CssClass="cella-testata-griglia" />
                        <ItemStyle CssClass="cella-elemento-griglia" />
                        <ItemTemplate>
                            <asp:ImageButton id="imgStatistiche" runat="server" width="30" 
                                ImageUrl ="App_Themes/Standard/Images/excel.png" ToolTip="Statistiche" 
                                OnCliCk="StatisticheTempo" >
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
                <asp:ImageButton ID="imgSalva" runat="server" ImageUrl="App_Themes/Standard/Images/icona_SALVA.png" ToolTip ="Salva i dati" style="float: right; width: 50px;"/>
                <asp:ImageButton ID="imgNuovo" runat="server" ImageUrl="App_Themes/Standard/Images/matitadx.png" ToolTip ="Nuova" style="float: right; width: 50px;" />--%>
            </li>
            <li class="li">
                <asp:Label ID="Label3" runat="server" Text="Tempo" CssClass ="etichettaTitolo"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtTempo" runat="server" CssClass ="casellatesto" text="" MaxLength="40"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Image ID="imgTempo" runat="server" Width="150px" ImageUrl="App_Themes/Standard/Images/icona_AVANTI.png"  />
            </li>
        </ul>
    </div>

    <div class="mascheraupload" id="divUpload" runat="server">
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:ImageButton ID="imgCarica" runat="server" ImageUrl="App_Themes/Standard/Images/icona_DOWNLOAD-TAG.png" ToolTip ="Upload" style=" width: 50px;" />
    </div>

    <div id="divStatistiche" runat="server" class="Statistiche">
        <asp:Label ID="lblStatPortata" runat="server" Text="Mezzo" CssClass ="etichettaTitolo"></asp:Label>
        <hr />
        <asp:GridView id="grdStatistiche" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
        PageSize="10" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="10" 
        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
            <Columns>
                <asp:BoundField DataField="Tempo" HeaderText="Tempo" SortExpression="Tempo" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <hr />
        <asp:ImageButton ID="imgChiudeStat" runat="server" width="50px" Height="50px" style="float: right; padding-top: 5px;" ImageUrl ="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip="Indietro" />
    </div>
</asp:Content>
