<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_TiposDeCliente>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar Tipo de Cliente
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar Tipo de Cliente</h2>

    <h3>¿Seguro que quiere borrar el tipo de cliente?</h3>
    <fieldset>
        <legend>Tipo de Cliente</legend>
        
        <div class="display-label">ID</div>
        <div class="display-field"><%: Model.ID %></div>
        
        <div class="display-label">Nombre</div>
        <div class="display-field"><%: Model.Nombre %></div>
        
    </fieldset>
    <% using (Html.BeginForm()) { %>
        <p>
		    <%: Html.ActionLink("Volver a la lista", "Index") %> |
            <input type="submit" value="Delete" /> 		    
        </p>
    <% } %>

</asp:Content>

