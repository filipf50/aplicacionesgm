<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Departamentos>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Borrar departamento
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Borrar departamento</h2>

    <h3>Seguro que quiere borrar el departamento?</h3>
    <fieldset>
        <legend>Departamento</legend>
        
        <div class="display-label">DepartamentoID</div>
        <div class="display-field"><%: Model.DepartamentoID %></div>
        
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

