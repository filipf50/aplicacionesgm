<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_TiposDeCliente>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modificar Tipo de Cliente
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Modificar Tipo de Cliente</h2>

    <% using (Html.BeginForm()) {%>
        <div class="chromestyle" id="chromemenu">
            <ul>
                <li><%: Html.ActionLink("Volver", "Index") %></li>
                <li><a id="submit">Guardar</a></li>
            </ul>
        </div>
        <fieldset>
            <legend>Tipo de Cliente</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ID) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.ID) %>
                <%: Html.ValidationMessageFor(model => model.ID) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Nombre) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Nombre) %>
                <%: Html.ValidationMessageFor(model => model.Nombre) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.OrdenDeVisualizacion) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.OrdenDeVisualizacion) %>
                <%: Html.ValidationMessageFor(model => model.OrdenDeVisualizacion) %>
            </div>
           </fieldset>
    <% } %>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#submit').click(function () {
                $(this).closest('form').submit();
            });
        });
    </script>

</asp:Content>

