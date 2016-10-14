<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_MaterialesEmpresa>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Barrar Material
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar material</h2>

    <h3>¿Seguro que quiere borrar el material?</h3>
    <fieldset>
        <legend>Material</legend>
        
        <div class="display-label">ID</div>
        <div class="display-field"><%: Model.ID %></div>
        
        <div class="display-label">Empresa</div>
        <div class="display-field"><%: Model.QSIDEmpresa %></div>

        <div class="display-label">Nombre</div>
        <div class="display-field"><%: Model.Nombre %></div>
        
    </fieldset>
    <% using (Html.BeginForm()) { %>
        <p>
		    <%: Html.ActionLink("Voler a la lista", "Index", new {idEmpresa=Model.QSIDEmpresa})%> |
            <input type="submit" value="Borrar" />		    
        </p>
    <% } %>

</asp:Content>
