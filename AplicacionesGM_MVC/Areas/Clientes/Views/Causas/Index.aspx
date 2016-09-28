<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_Causas>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Causas
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Causas</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","Causas") %>"><img class="imgLink" title="Crear Nueva Causa" alt="NuevaCausa" longdesc="Nueva Causa" src="../../Content/images/iconos/nuevo_registro.png"/></a>
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
                <%: item.ID %>
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

