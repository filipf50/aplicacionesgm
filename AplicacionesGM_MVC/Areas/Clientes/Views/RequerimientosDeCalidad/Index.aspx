﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_RequerimientosDeCalidad>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Requerimientos de calidad
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Requerimientos de calidad</h2>
    <div id="iconos">
        <a href="<%: @Url.Action("Create","RequerimientosDeCalidad") %>"><img class="imgLink" title="Crear Nuevo Instrumento de Pesaje" alt="NuevoInstrumentoDePesaje" longdesc="Nuevo Instrumento de Pesaje" src="../../Content/images/iconos/nuevo_registro.png"/></a>
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

