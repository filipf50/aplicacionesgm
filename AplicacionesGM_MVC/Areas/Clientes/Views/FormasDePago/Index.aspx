<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Areas.Clientes.Models.FormasDePagoModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Lista de Formas de pago
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Lista de Formas de pago</h2>

    <table>
        <tr>
            <th>
                ID
            </th>
            <th>
                Nombre
            </th>
            <th>
                Visible
            </th>
            <th>
                Disponible en Exposición
            </th>
            <th>
                Requiere SEPA
            </th>
            <th>
                Dto. P.P.
            </th>
            <th>
                Recargo financiero
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
                <%: item.Visible.ToString().Replace("True","Sí").Replace("False","No") %>
            </td>
            <td>
                <%: item.DisponibleExposicion.ToString().Replace("True","Sí").Replace("False","No") %>
            </td>
            <td>
                <%: item.EsSEPA.ToString().Replace("True", "Sí").Replace("False", "No")%>
            </td>
            <td>
                <%: item.DtoPP %>
            </td>
            <td>
                <%: item.RecargoFinanciero%>
            </td>
            <td>
                <%: Html.ActionLink("Modificar", "Edit", new { id = item.ID }, new { @class = "button button-index" })%>
            </td>
        </tr>
    
    <% } %>

    </table>
</asp:Content>
