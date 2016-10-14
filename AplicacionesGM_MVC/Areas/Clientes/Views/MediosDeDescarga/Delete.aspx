<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_MediosDeDescarga>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar medio de descarga
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar medio de descarga</h2>

    <h3>¿Seguro que quiere borrar el medio de descarga?</h3>
    <fieldset>
        <legend>Medio de descarga</legend>
        
        <div class="display-label">ID</div>
        <div class="display-field"><%: Model.ID %></div>
        
        <div class="display-label">Nombre</div>
        <div class="display-field"><%: Model.Nombre %></div>
        
    </fieldset>
    <% using (Html.BeginForm()) { %>
        <p>
		    <input type="submit" value="Delete" /> |
		    <%: Html.ActionLink("Volver a lista", "Index") %>
        </p>
    <% } %>

</asp:Content>

