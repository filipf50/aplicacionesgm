<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_MaterialesEmpresa>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Lista de materiales
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Lista de materiales</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","MaterialesXEmpresa", new { idEmpresa = ViewData["Empresa"] }) %>"><img class="imgLink" title="Crear Nueva Empresa" alt="NuevaCausa" longdesc="Nueva Empresa" src="../../Content/images/iconos/nuevo_registro.png"/></a>
    </div>
    <div class="chromestyle" id="chromemenu">
        <ul>
            <li><%: Html.ActionLink("Volver", "Index","Empresas") %></li>
        </ul>
    </div>
    <table>
        <tr>
            <th>
                ID
            </th>
            <th>
                QSIDEmpresa
            </th>
            <th>
                Nombre
            </th>
            <th></th>
            
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.ID %>
            </td>
            <td>
                <%: item.QSIDEmpresa %>
            </td>
            <td>
                <%: item.Nombre %>
            </td>
            <td>
                <%: Html.ActionLink("Modificar", "Edit", new { id = item.ID }, new { @class = "button button-index" })%>
                <%: Html.ActionLink("Borrar", "Delete", new { id = item.ID }, new { @class = "button button-index" })%>
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>
