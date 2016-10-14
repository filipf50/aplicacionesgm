<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_Empresas>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Empresas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Empresas</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","Empresas") %>"><img class="imgLink" title="Crear Nueva Empresa" alt="NuevaCausa" longdesc="Nueva Empresa" src="../../Content/images/iconos/nuevo_registro.png"/></a>
    </div>
    <table>
        <tr>
            <th>
                QSID
            </th>
            <th>
                Nombre
            </th>
            <th></th>
            
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.QSID %>
            </td>
            <td>
                <%: item.Nombre %>
            </td>
            <td>
                <%: Html.ActionLink("Materiales", "Index", "MaterialesXEmpresa", new { idEmpresa = item.QSID }, new { @class = "button button-index" })%>
                <%: Html.ActionLink("Modificar", "Edit", new { QSid = item.QSID }, new { @class = "button button-index" })%>
                <%: Html.ActionLink("Borrar", "Delete", new { QSid = item.QSID }, new { @class = "button button-index" })%>
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>

