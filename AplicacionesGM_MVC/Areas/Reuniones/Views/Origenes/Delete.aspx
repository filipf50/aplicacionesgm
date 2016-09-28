<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Origenes>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar origen
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar origen</h2>

    <h3>¿Seguro que quiere borrar el origen?</h3>
    <fieldset>
        <legend>Fields</legend>
        
        <div class="display-label">OrigenID</div>
        <div class="display-field"><%: Model.OrigenID %></div>
        
        <div class="display-label">Descripcion</div>
        <div class="display-field"><%: Model.Descripcion %></div>
        
    </fieldset>
    <% using (Html.BeginForm()) { %>
        <p>
		   <%: Html.ActionLink("Volver a la lista", "Index") %> |
            <input type="submit" value="Borrar" />		    
        </p>
    <% } %>

</asp:Content>

