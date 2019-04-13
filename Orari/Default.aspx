<%@ Page Language="vb" AutoEventWireup="false" AspCompat="true" CodeBehind="Default.aspx.vb" MasterPageFile="Orari.Master" Inherits="Orari._Default" %>

<asp:Content id="Content2" runat="server" ContentPlaceHolderID="cphCorpo">
    <div class="login">
        <ul class="ullogin">
            <li class="lidett">
                <span class="etichettalogin">Utente</span>
            </li>
            <li class="lidett">
                <asp:TextBox id="txtUtenza" class="testo" runat="server" MaxLength="30" />
            </li>
            <li class="lidett">
                <span class="etichettalogin">Password</span>
            </li>
            <li class="lidett">
                <asp:TextBox id="txtPassword" class="testo" runat="server" MaxLength="30" TextMode="Password" />
            </li>
        </ul>
            
       <div class="clear clearALL"></div>

        <ul class="ullogin">
            <li class="lidett">
                <asp:CheckBox ID="chkRicordami" runat="server" CssClass ="option" text="Ricordami" Checked="True"/>
            </li>
            <li class="lidett">
                <asp:Button id="cmdInvia" class="bottone" runat="server" Text="Entra" />
            </li>
        </ul>
    </div>
</asp:Content>
