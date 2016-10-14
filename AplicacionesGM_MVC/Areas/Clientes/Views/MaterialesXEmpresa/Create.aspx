<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_MaterialesEmpresa>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Nuevo Materia
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Nuevo Material</h2>

    <% using (Html.BeginForm()) {%>
        <div class="chromestyle" id="chromemenu">
            <ul>
                <li><%: Html.ActionLink("Volver", "Index", "MaterialesXEmpresa", new {idempresa = ViewData["Empresa"].ToString() },null)%></li>
                <li><a id="submit">Guardar</a></li>
            </ul>
        </div>

        <fieldset>
            <legend>Material</legend>
            
            <div class="editor-label">
                Id. Empresa QS
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.QSIDEmpresa, new {Value = ViewData["Empresa"] })%>
                <%: Html.ValidationMessageFor(model => model.QSIDEmpresa)%>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.Nombre) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Nombre) %>
                <%: Html.ValidationMessageFor(model => model.Nombre) %>
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
