<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_Delegaciones>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Delegaciones
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Delegaciones</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","Delegaciones") %>"><img class="imgLink" title="Crear Nueva Delegación" alt="NuevaDelegacion" longdesc="Nueva Delegacion" src="../../Content/images/iconos/nuevo_registro.png"/></a>
    </div>    
    <table>
        <tr>
            <th>
                Descripcion
            </th>
            <th>
                Siglas
            </th>
            <th></th>
            
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.Descripcion %>
            </td>
            <td>
                <%: item.Siglas %>
            </td>
            <td>
                <%: Html.ActionLink("Modificar", "Edit", new { id = item.DelegacionID }, new { @class = "button button-index" })%>
                <%: Html.ActionLink("Borrar", "Delete", new { id = item.DelegacionID }, new { @class = "button button-index" })%>
            </td>
            
        </tr>
    
    <% } %>

    </table>
</asp:Content>

