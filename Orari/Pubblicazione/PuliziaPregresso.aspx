<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="PuliziaPregresso.aspx.vb" Inherits="Orari.PuliziaPregresso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
        <div id="Elimina" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('Elimina',3);" 
            onmouseout ="mouseOutImageSopra('Elimina',3);" >
            <asp:ImageButton ID="imgElimina" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/eliminadx.png" ToolTip="Elimina selezionati" />
        </div>
    </div>
    <div class="mascheramodifica">
        <asp:GridView id="grdMesi" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
            PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
            PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
            <Columns>
                <asp:BoundField DataField="Mese" HeaderText="Mese" >
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia" />
                </asp:BoundField>
 
                <asp:TemplateField HeaderText="Elimina">
                    <HeaderStyle CssClass="cella-testata-griglia" />
                    <ItemStyle CssClass="cella-elemento-griglia-destra" />
                    <ItemTemplate>
                        <asp:CheckBox id="chkScelto" class="option" runat="server"></asp:CheckBox>
                    </ItemTemplate>                                        
                </asp:TemplateField>
           </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
