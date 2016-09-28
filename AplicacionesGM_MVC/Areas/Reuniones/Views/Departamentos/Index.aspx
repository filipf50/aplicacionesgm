<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_Departamentos>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Departamentos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Departamentos</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","Departamentos") %>"><img class="imgLink" title="Crear Nuevo Departamento" alt="NuevaDepartamento" longdesc="Nueva Departamento" src="../../Content/images/iconos/nuevo_registro.png"/></a>
    </div> 
    <table>
        <tr>
            <th>
                ID
            </th>
            <th>
                Descripción
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.DepartamentoID %>
            </td>
            <td>
                <%: item.Descripcion %>
            </td>
            <td>
                <%: Html.ActionLink("Modificar", "Edit", new { id = item.DepartamentoID }, new { @class = "button button-index" })%>
                <%: Html.ActionLink("Borrar", "Delete", new { id = item.DepartamentoID }, new { @class = "button button-index" })%>
            </td>
            
        </tr>
    
    <% } %>

    </table>
</asp:Content>

