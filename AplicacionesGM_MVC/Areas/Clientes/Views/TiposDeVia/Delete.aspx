<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_TiposDeVia>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar Tipo de Vía
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar Tipo de Vía</h2>

    <h3>¿Seguro que quiere borrar el tipo de vía?</h3>
    <fieldset>
        <legend>Fields</legend>
        
        <div class="display-label">IDTipoVia</div>
        <div class="display-field"><%: Model.IDTipoVia %></div>
        
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

