<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_TiposDeVia>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modificar Tipo de Vía
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Modificar Tipo de Vía</h2>

    <% using (Html.BeginForm()) {%>
        <div class="chromestyle" id="chromemenu">
            <ul>
                <li><%: Html.ActionLink("Volver", "Index") %></li>
                <li><a id="submit">Guardar</a></li>
            </ul>
        </div>
        
        <fieldset>
            <legend>Tipo de Vía</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.IDTipoVia ) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.IDTipoVia)%>
                <%: Html.ValidationMessageFor(model => model.IDTipoVia)%>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Nombre) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Nombre) %>
                <%: Html.ValidationMessageFor(model => model.Nombre) %>
            </div>
           </fieldset>
        <div>
            <p>
                <%: Html.ActionLink("Volver a la lista", "Index") %> |
                <input type="submit" value="Guardar" />
            </p>
        </div>
    <% } %>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#submit').click(function () {
                $(this).closest('form').submit();
            });
        });
    </script>
</asp:Content>

