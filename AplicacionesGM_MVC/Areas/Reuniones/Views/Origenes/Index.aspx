<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_Origenes>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Orígenes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Orígenes</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","Origenes") %>"><img class="imgLink" title="Crear Nuevo Origen" alt="NuevaOrigen" longdesc="Nuevo Origen" src="../../Content/images/iconos/nuevo_registro.png"/></a>
    </div> 
    <table>
        <tr>
            <th>
                OrigenID
            </th>
            <th>
                Descripcion
            </th>
            <th></th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.OrigenID %>
            </td>
            <td>
                <%: item.Descripcion %>
            </td>
            <td>
                <%: Html.ActionLink("Modificar", "Edit", new { id = item.OrigenID }, new { @class = "button button-index" })%>
                <%: Html.ActionLink("Borrar", "Delete", new { id = item.OrigenID }, new { @class = "button button-index" })%>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Crear nuevo origen", "Create") %>
    </p>

</asp:Content>

