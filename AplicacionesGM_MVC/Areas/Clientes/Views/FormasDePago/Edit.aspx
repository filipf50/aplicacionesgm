<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Areas.Clientes.Models.FormasDePagoModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modificar forma de pago
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Modificar forma de pago</h2>

    <% using (Html.BeginForm()) {%>
        <div class="chromestyle" id="chromemenu">
            <ul>
                <li><%: Html.ActionLink("Volver", "Index") %></li>
                <li><a id="submit">Guardar</a></li>
            </ul>
        </div>
        <fieldset>
            <legend>Forma de pago</legend>
            <%: Html.HiddenFor(model => model.ID) %>
            <%: Html.ValidationSummary(true,"Error al guardar la forma de pago. Revise los errores y vuelva a intentarlo") %>
            <table id="DatosGenerales" class="noborder">
                <tbody>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.LabelFor(model=>model.Nombre) %>
                        </td>
                        <td class="editor-label noborder td80">
                            <%: Model.Nombre.ToString() %>
                            <%: Html.HiddenFor(model=>model.Nombre) %>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.LabelFor(model=>model.Visible) %>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.CheckBoxFor (model => model.Visible) %>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.LabelFor(model=>model.DisponibleExposicion) %>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.CheckBoxFor(model => model.DisponibleExposicion)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.LabelFor(model=>model.EsSEPA) %>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.CheckBoxFor (model => model.EsSEPA) %>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.LabelFor(model=>model.DtoPP) %>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.DtoPP, new {@class="textbox5" })%> %
                            <%: Html.ValidationMessageFor(model => model.DtoPP) %>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.LabelFor(model => model.RecargoFinanciero)%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.RecargoFinanciero, new { @class = "textbox5" })%> %
                            <%: Html.ValidationMessageFor(model => model.RecargoFinanciero)%>
                        </td>
                    </tr>
                </tbody>
            </table>
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
