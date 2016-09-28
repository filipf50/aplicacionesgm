<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Acciones>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modificando Acción
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Modificando Acción</h2>

    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>
        <div class="chromestyle" id="chromemenu2">
            <ul >
                <li><%: Html.ActionLink("Volver", "Index") %></li>
                <li><a id="submit"> Guardar</a></li>
                <li><a href="/Reuniones/Acciones/Create"  onclick="window.open('/Reuniones/Acciones/Create', 'newwindow', 'width=1340, height=760, resizable=yes, scrollbars=yes'); return false;"> Nueva Acción</a></li>                
            </ul>
        </div>
        <fieldset>
            <legend>Acción</legend>
            <div class="col20">
                <div class="editor-field">
                    <%: Html.HiddenFor(model => model.AccionesID) %>
                </div>
            
                <div class="editor-label">
                    Origen
                </div>
                <div class="editor-field">
                    <%: Html.DropDownList("OrigenID")%>
                    <%: Html.ValidationMessageFor(model => model.OrigenID) %>
                
                </div>
            
                <div class="editor-label">
                    Departamento
                </div>
                <div class="editor-field">
                    <%: Html.DropDownList("DepartamentoID")%>
                    <%: Html.ValidationMessageFor(model => model.DepartamentoID) %>
                </div>

                <div class="editor-label">
                   Delegaciones
                </div>
                <div class="editor-field checkboxlist">
                    <% AplicacionesGM_MVC.Models.CheckBoxModel chkDelegaciones=(AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkDelegaciones"];
                       bool blnChecked = false; 
                       foreach (var info in chkDelegaciones.lstValores)
                       {
                            if (Model.Delegaciones.Split(',').Contains(info.Key.ToString()))
                            {
                                chkDelegaciones.arrValoresSelected.Add(info.Key.ToString());
                                blnChecked = true;
                            } else {
                                blnChecked = false;   
                            }
                            %>
                                <div>
                                    <%: Html.CheckBox("arrDelegaciones", blnChecked, new {value=info.Key.ToString()})%>
                                    <%: info.Value %>
                                </div>
                    <% } %>
                    <%: Html.ValidationMessageFor(model => model.Delegaciones) %>
                </div>
            
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Responsables) %>
                </div>
                <div class="editor-field checkboxlist">
                    <% AplicacionesGM_MVC.Models.CheckBoxModel chkResponsables=(AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkResponsables"];
                       foreach (var info in chkResponsables.lstValores)
                       {
                            if (Model.Responsables.Split(',').Contains(info.Key.ToString()))
                            {
                                chkResponsables.arrValoresSelected.Add(info.Key.ToString());
                                blnChecked = true;
                            } else {
                                blnChecked = false;   
                            }
                            %>
                                <div>
                                    <%: Html.CheckBox("arrResponsables", blnChecked, new { value = info.Key.ToString() })%>
                                    <%: info.Value %>
                                </div>
                    <% } %>
                
                    <%: Html.ValidationMessageFor(model => model.Responsables) %>
                </div>
            
                <div class="editor-label">
                    Fecha Inicio
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("FechaInicio", String.Format("{0:d}",Model.FechaInicio), new{@class="date"})%>
                    <%: Html.ValidationMessageFor(model => model.FechaInicio) %>
                </div>
            
                <div class="editor-label">
                    Fecha Fin Prevista
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("FechaFinPrev",String.Format("{0:d}",Model.FechaFinPrev),new { @class = "date"})%>
                    <%: Html.ValidationMessageFor(model => model.FechaFinPrev) %>
                </div>
            
                <div class="editor-label">
                    Fecha Fin Real
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("FechaFinReal", String.Format("{0:d}", Model.FechaFinReal), new { @class = "date" })%>
                    <%: Html.ValidationMessageFor(model => model.FechaFinReal) %>
                </div>
            
                <div class="editor-label">
                    Fecha Seguimiento
                </div>
                <div class="editor-field">
                    <%: Html.TextBox("FechaSeguimiento", String.Format("{0:d}", Model.FechaSeguimiento), new { @class = "date" })%>
                    <%: Html.ValidationMessageFor(model => model.FechaSeguimiento) %>
                </div>
            </div>
            <div class="col74">
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Accion) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Accion) %>
                    <%: Html.ValidationMessageFor(model => model.Accion) %>
                </div>
            
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Objetivo) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Objetivo) %>
                    <%: Html.ValidationMessageFor(model => model.Objetivo) %>
                </div>
            
                <div class="editor-label">
                    <%: Html.LabelFor(model => model.Resultado) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextAreaFor(model => model.Resultado) %>
                    <%: Html.ValidationMessageFor(model => model.Resultado) %>
                </div>
            </div>
            
        </fieldset>
    <% } %>
    <script>
        $(document).ready(function () {
            $('#submit').click(function () {
                $(this).closest('form').submit();
            });
        });
    </script>
</asp:Content>

