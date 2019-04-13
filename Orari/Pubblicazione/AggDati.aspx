<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="AggDati.aspx.vb" Inherits="Orari.AggDati" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCorpo" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
    <div id="barraTasti" runat="server" class="barraTasti" >
        <div id="divUscita" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopra('divUscita',2);" 
            onmouseout ="mouseOutImageSopra('divUscita',2);" >
            <asp:ImageButton ID="imgUscita" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_USCITA.png" ToolTip="Aggiorna i dati" />
        </div>
        <div id="Indietro" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('Indietro',1);" 
            onmouseout ="mouseOutImageSopraSA('Indietro',1);" >
            <asp:ImageButton ID="imgAnnulla" runat="server" width="50px" Height="50px" ImageUrl ="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip="Indietro" />
        </div>
    </div>
    <asp:FileUpload ID="FileUpload1" runat="server" />
    <br />
    <asp:ImageButton ID="imgCarica" runat="server" ImageUrl="App_Themes/Standard/Images/icona_DOWNLOAD-TAG.png" Width="40px" />
    <asp:ImageButton ID="imgEsegue" runat="server" ImageUrl="App_Themes/Standard/Images/icona_ESEGUE.png" Visible="False" Width="40px" />
</asp:Content>
