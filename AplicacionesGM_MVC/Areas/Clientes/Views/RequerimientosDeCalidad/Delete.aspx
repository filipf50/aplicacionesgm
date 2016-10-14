<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_RequerimientosDeCalidad>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar requerimiento de calidad
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar requerimiento de calidad</h2>

    <h3>¿Seguro que quiere borrar el requerimiento de calidad?</h3>
    <fieldset>
        <legend>Requerimiento de calidad</legend>
        
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

