<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Delegaciones>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar delegacion
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar delegación</h2>

    <h3>¿Seguro que quiere borrar la delegación?</h3>
    <fieldset>
        <legend>Delegación</legend>
        
        <div class="display-label">DelegacionID</div>
        <div class="display-field"><%: Model.DelegacionID %></div>
        
        <div class="display-label">Descripcion</div>
        <div class="display-field"><%: Model.Descripcion %></div>
        
        <div class="display-label">Siglas</div>
        <div class="display-field"><%: Model.Siglas %></div>
        
    </fieldset>
    <% using (Html.BeginForm()) { %>
        <p>
		    <%: Html.ActionLink("Volver a la lista", "Index") %> |
            <input type="submit" value="Borrar" />
        </p>
    <% } %>

</asp:Content>

