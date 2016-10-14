<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Empresas>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar Empresa
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar Empresa</h2>

    <h3>¿Seguro que quiere borrar la empresa?</h3>
    <fieldset>
        <legend>Empresa</legend>
        
        <div class="display-label">QSID</div>
        <div class="display-field"><%: Model.QSID %></div>
        
        <div class="display-label">Nombre</div>
        <div class="display-field"><%: Model.Nombre %></div>
        
    </fieldset>
    <% using (Html.BeginForm()) { %>
        <p>
		    <%: Html.ActionLink("Voler a la lista", "Index") %> |
            <input type="submit" value="Borrar" />		    
        </p>
    <% } %>

</asp:Content>

