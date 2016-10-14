<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_TiposDeVehiculo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar tipo de vehículo
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar tipo de vehículo</h2>

    <h3>¿Seguro que quiere borrar el tipo de vehículo?</h3>
    <fieldset>
        <legend>Tipo de vehículo</legend>
        
        <div class="display-label">ID</div>
        <div class="display-field"><%: Model.ID %></div>
        
        <div class="display-label">Nombre</div>
        <div class="display-field"><%: Model.Nombre %></div>
        
    </fieldset>
    <% using (Html.BeginForm()) { %>
        <p>
		    <%: Html.ActionLink("Volver a la lista", "Index") %> |
            <input type="submit" value="Borrar" />
        </p>
    <% } %>

</asp:Content>

