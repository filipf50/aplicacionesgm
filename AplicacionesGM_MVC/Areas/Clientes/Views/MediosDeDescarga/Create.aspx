<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_MediosDeDescarga>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Nuevo medio de descarga
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Nuevo medio de descarga</h2>

    <% using (Html.BeginForm()) {%>
        <div class="chromestyle" id="chromemenu">
            <ul>
                <li><%: Html.ActionLink("Volver", "Index") %></li>
                <li><a id="submit">Guardar</a></li>
            </ul>
        </div>
        <fieldset>
            <legend>Medio de descarga</legend>
            <%: Html.ValidationSummary(true,"Error al guardar el nuevo medio de descarga. Revise los errores y vuelva a intentarlo") %>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Nombre) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Nombre) %>
                <%: Html.ValidationMessageFor(model => model.Nombre,true) %>
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

