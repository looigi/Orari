<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="GestioneTabelle.aspx.vb" Inherits="Orari.GestioneTabelle" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNoAjax" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphCorpo" runat="server">
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

    <div class="mascheramodifica">
            <ul class="ul">
                <li class="li">
                    <a href="GestioneAbitazioni.aspx?Maschera=GestTabelle&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label8" runat="server" Text="Abitazioni" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgAbitazioni" runat="server" ImageUrl="App_Themes/Standard/Images/abitazione.png" ToolTip ="Tempo" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="GestioneCommesse.aspx?Maschera=GestTabelle&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label1" runat="server" Text="Commesse" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgCommesse" runat="server" ImageUrl="App_Themes/Standard/Images/Commesse.png" ToolTip ="Commesse" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="GestioneLinguaggi.aspx?Maschera=GestTabelle&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label9" runat="server" Text="Linguaggi" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgLinguaggi" runat="server" ImageUrl="App_Themes/Standard/Images/codice.jpg" ToolTip ="Tempo" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="GestioneMezzi.aspx?Maschera=GestTabelle&Tipo=Andata&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label5" runat="server" Text="Mezzi e standard di andata" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgMezziA" runat="server" ImageUrl="App_Themes/Standard/Images/Mezzi.png" ToolTip ="Mezzi di andata" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="GestioneMezzi.aspx?Maschera=GestTabelle&Tipo=Ritorno&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label7" runat="server" Text="Mezzi e standard di ritorno" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgMezziR" runat="server" ImageUrl="App_Themes/Standard/Images/Mezzi.png" ToolTip ="Mezzi di ritorno" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="GestionePasticche.aspx?Maschera=GestTabelle&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label2" runat="server" Text="Pasticche" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgPasticche" runat="server" ImageUrl="App_Themes/Standard/Images/pasticca.png" ToolTip ="Pasticche" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="GestionePortate.aspx?Maschera=GestTabelle&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label4" runat="server" Text="Portate" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgPortate" runat="server" ImageUrl="App_Themes/Standard/Images/portate.png" ToolTip ="Portate" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="GestioneSocieta.aspx?Maschera=GestTabelle&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label3" runat="server" Text="Società" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgSocieta" runat="server" ImageUrl="App_Themes/Standard/Images/societa.png" ToolTip ="Società" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="GestioneStudi.aspx?Maschera=GestTabelle&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label10" runat="server" Text="Studi" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgStudi" runat="server" ImageUrl="App_Themes/Standard/Images/scuola.png" ToolTip ="Società" style="width: 100px;" />
                </li>
                <li class="li">
                    <a href="Gestionetempo.aspx?Maschera=GestTabelle&Giorno=<%= Request.QueryString("Giorno")%>&Mese=<%= Request.QueryString("Mese")%>&Anno=<%= Request.QueryString("Anno")%>">
                    <asp:Label ID="Label6" runat="server" Text="Tempo" CssClass ="etichettaLink"></asp:Label>
                    </a>
                </li>
                <li class="li">
                    <asp:ImageButton ID="imgTempo" runat="server" ImageUrl="App_Themes/Standard/Images/tempo.png" ToolTip ="Tempo" style="width: 100px;" />
                </li>
                <li class="li">
                </li>
                <li class="li">
                </li>
            </ul>

<%--        <ul class="ul">
            <li class="li">
            </li>
            <li class="li">
            </li>
            <li class="li">
            </li>
            <li class="li">
                <asp:ImageButton ID="imgAnnulla" runat="server" ImageUrl="App_Themes/Standard/Images/icona_INDIETRO.png" ToolTip ="Annulla" style="float: right; width: 50px;" />
            </li>
        </ul>--%>
    </diV>
</asp:Content>
