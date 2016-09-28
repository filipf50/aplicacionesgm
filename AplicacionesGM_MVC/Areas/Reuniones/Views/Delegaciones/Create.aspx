<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Delegaciones>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Nueva delegacion
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Nueva delegación</h2>

    <% using (Html.BeginForm()) {%>
        <div class="chromestyle" id="chromemenu">
            <ul>
                <li><%: Html.ActionLink("Volver", "Index") %></li>
                <li><a id="submit">Guardar</a></li>
            </ul>
        </div>

        <fieldset>
            <legend>Delegación</legend>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Descripcion) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Descripcion) %>
                <%: Html.ValidationMessageFor(model => model.Descripcion) %>
            </div>
            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Siglas) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Siglas) %>
                <%: Html.ValidationMessageFor(model => model.Siglas) %>
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

