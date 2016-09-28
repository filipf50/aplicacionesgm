<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Empresas>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Nueva Empresa
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Nueva Empresa</h2>

    <% using (Html.BeginForm()) {%>
        <div class="chromestyle" id="chromemenu">
            <ul>
                <li><%: Html.ActionLink("Volver", "Index") %></li>
                <li><a id="submit">Guardar</a></li>
            </ul>
        </div>

        <fieldset>
            <legend>Empresa</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.QSID) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.QSID) %>
                <%: Html.ValidationMessageFor(model => model.QSID) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Nombre) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Nombre) %>
                <%: Html.ValidationMessageFor(model => model.Nombre) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Visible) %>
            </div>
            <div class="editor-field">
                <%: Html.CheckBoxFor (model => model.Visible) %>
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

