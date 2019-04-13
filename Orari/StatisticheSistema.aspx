<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="StatisticheSistema.aspx.vb" Inherits="Orari.StatisticheSistema" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNoAjax" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCorpo" runat="server">
    <asp:HiddenField ID="hdnPassaggio1" runat="server" />
    <asp:HiddenField ID="hdnPassaggio2" runat="server" />
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

    <asp:GridView id="grdGiorniLavorati" runat="server" AllowPaging="True" AutoGenerateEditButton="False" AutoGenerateSelectButton="False" AutoGenerateColumns="False" AllowSorting="false" ShowHeaderWhenEmpty="False" 
        PageSize="20" CssClass="griglia" PagerStyle-CssClass="paginazione-griglia" RowStyle-CssClass ="riga-dispari-griglia" AlternatingRowStyle-CssClass="riga-pari-griglia" HeaderStyle-CssClass="testata-griglia" PagerSettings-PageButtonCount="20" 
        PagerSettings-Mode="NumericFirstLast" PagerSettings-Position="Bottom" PagerSettings-FirstPageImageUrl="App_Themes/Standard/Images/icona_PRIMO-RECORD.png" PagerSettings-LastPageImageUrl="App_Themes/Standard/Images/icona_ULTIMO-RECORD.png" >
        <Columns>
            <asp:BoundField DataField="Cosa" HeaderText="Descrizione" >
                <HeaderStyle CssClass="cella-testata-griglia" />
                <ItemStyle CssClass="cella-elemento-griglia" />
            </asp:BoundField>

            <asp:BoundField DataField="Data" HeaderText="Data" >
                <HeaderStyle CssClass="cella-testata-griglia" />
                <ItemStyle CssClass="cella-elemento-griglia" />
            </asp:BoundField>

            <asp:BoundField DataField="Applicazione" HeaderText="Applicazione" >
                <HeaderStyle CssClass="cella-testata-griglia" />
                <ItemStyle CssClass="cella-elemento-griglia" />
            </asp:BoundField>

            <asp:BoundField DataField="Quante" HeaderText="Quante" >
                <HeaderStyle CssClass="cella-testata-griglia" />
                <ItemStyle CssClass="cella-elemento-griglia-destra" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>

</asp:Content>
