<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_TiposDeCliente>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tipos de Cliente
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Tipos de Cliente</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","TiposDeCliente") %>"><img class="imgLink" title="Crear Nuevo Tipo de Cliente" alt="NuevoTipoDeCliente" longdesc="Nuevo Tipo de Cliente" src="../../Content/images/iconos/nuevo_registro.png"/></a>
    </div>
    <table>
        <tr>
            <th>
                ID
            </th>
            <th>
                Nombre
            </th>
            <th>
                Orden de visualización
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.ID %>
            </td>
            <td>
                <%: item.Nombre %>
            </td>
            <td>
                <%: item.OrdenDeVisualizacion %>
            </td>
            <td>
                <%: Html.ActionLink("Modificar", "Edit", new { id = item.ID }, new { @class = "button button-index" })%>
                <%: Html.ActionLink("Borrar", "Delete", new { id = item.ID }, new { @class = "button button-index" })%>
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>

