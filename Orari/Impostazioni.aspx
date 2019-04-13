<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="Orari.Master" CodeBehind="Impostazioni.aspx.vb" Inherits="Orari.Impostazioni" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphCorpo" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphNoAjax" runat="server">
    <div id="barraTasti" runat="server" class="barraTasti" >
        <div id="divUscita" runat="server" 
            class="linguetta"
            onmouseover ="mouseOverImageSopraSA('divUscita',3);" 
            onmouseout ="mouseOutImageSopraSA('divUscita',3);" >
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
    </div>

    <div class="mascheramodifica">
        <ul class="ul" style="margin-left: 30px;">
            <li class="li">
                <asp:Label ID="Label6" runat="server" Text="Matricola" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtMatricola" runat="server" CssClass ="testo"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label7" runat="server" Text="E-Mail" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtEMail" runat="server" CssClass ="testo" Width="235px"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label4" runat="server" Text="Nome" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtNome" runat="server" CssClass ="testo" MaxLength ="50" Width="235px"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label5" runat="server" Text="Cognome" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtCognome" runat="server" CssClass ="testo" MaxLength ="50" Width="235px"></asp:TextBox>
            </li>

            <li class="li">
                <asp:Label ID="Label1" runat="server" Text="Password" CssClass ="etichettamenu" ></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtPassword" runat="server" CssClass ="testo" TextMode="Password" MaxLength ="30"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label3" runat="server" Text="Ripeti Password" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtPassword2" runat="server" CssClass ="testo" TextMode="Password"  MaxLength ="30" Width="235px"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label2" runat="server" Text="Ore Standard" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtOreStd" runat="server" CssClass ="testo"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label8" runat="server" Text="Data nascita" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtDataNasc" runat="server" CssClass ="testo datepicker"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label10" runat="server" Text="Telefono" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtTelefono" runat="server" CssClass ="testo"></asp:TextBox>
            </li>
            <li class="li">
            </li>
            <li class="li">
            </li>
            <li class="li">
                <asp:Label ID="Label11" runat="server" Text="Motto" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:TextBox ID="txtMotto" runat="server" CssClass ="testo" MaxLength="150" Width="357px"></asp:TextBox>
            </li>
            <li class="li">
                <asp:Label ID="Label9" runat="server" Text="Immagine" CssClass ="etichettamenu"></asp:Label>
            </li>
            <li class="li">
                <asp:FileUpload ID="FileUpload1" runat="server" />
                &nbsp;<asp:ImageButton ID="imgCaricaFoto" runat="server" ImageUrl ="App_Themes\standard\images\icona_DOWNLOAD-TAG.png" Width="35px" />
            </li>
        </ul>
    </div>
</asp:Content>
