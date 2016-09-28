<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_TiposDeVia>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Tipos de vía
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Tipos de vía</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","TiposDeVia") %>"><img class="imgLink" title="Crear Nuevo Tipo de Vía" alt="NuevoTipoDeVehiculo" longdesc="Nuevo Tipo de Vía" src="../../Content/images/iconos/nuevo_registro.png"/></a>
    </div>
    <table>
        <tr>
            <th>
                ID
            </th>
            <th>
                Nombre
            </th>
            <th></th>
            
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.IDTipoVia %>
            </td>
            <td>
                <%: item.Nombre %>
            </td>
            <td>
                <%: Html.ActionLink("Modificar", "Edit", new { id = item.IDTipoVia }, new { @class = "button button-index" })%>
                <%: Html.ActionLink("Borrar", "Delete", new { id = item.IDTipoVia }, new { @class = "button button-index" })%>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Crear nuevo tipo de vía", "Create") %>
    </p>

</asp:Content>
