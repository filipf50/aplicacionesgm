<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_MaterialesEmpresa>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modificar Material
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Modificar Material</h2>

    <% using (Html.BeginForm()) {%>
        <div class="chromestyle" id="chromemenu">
            <ul>
                <li><%: Html.ActionLink("Volver", "Index", new { idEmpresa = Model.QSIDEmpresa })%></li>
                <li><a id="submit">Guardar</a></li>
            </ul>
        </div>
        <fieldset>
            <legend>Material</legend>
            <%: Html.ValidationSummary(true) %>    

            <div class="editor-field">
                <%: Html.HiddenFor(model => model.ID) %>
                <%: Html.HiddenFor(model => model.QSIDEmpresa) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Nombre) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Nombre) %>
                <%: Html.ValidationMessageFor(model => model.Nombre) %>
            </div>
        </fieldset>
        <script type="text/javascript">
            $(document).ready(function () {
                $('#submit').click(function () {
                    $(this).closest('form').submit();
                });
            });
        </script>
    <% } %>
</asp:Content>
