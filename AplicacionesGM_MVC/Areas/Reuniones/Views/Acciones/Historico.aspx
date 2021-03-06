﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reuniones/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_Acciones>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Historico
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2><%: ViewData["Title"]%> </h2>
        <div id="filtros">
            <div class="chromestyle" id="chromemenu2">
                <ul>
                    <li><%: Html.ActionLink("Limpiar Filtros", "LimpiarFiltros",new{tab=ViewData["Title"].ToString()})%></li>
                    <li><a id="mostrarFiltros">Mostrar / Ocultar filtros</a></li>
                </ul>
            </div>
        </div>
        <fieldset id="panelFiltros">
            <legend>Filtros</legend>
            <% using (Html.BeginForm("Historico","Acciones"))
                {%>
                <%: Html.Hidden("sortOrder",Session["sortOrder"]) %>
                <%: Html.Hidden("sortType",Session["sortType"])%>
                <div class="col60">
                    <div class="float_left">
                        <div class="editor-label">
                            Fecha Inicio
                        </div>
                        <div class="editor-label">
                            &nbsp Desde
                            <%: Html.TextBox("fInicioDesde",Session["fInicioDesde"], new { @class = "date" })%>
                        </div>
                        <br />
                        <div class="editor-label">
                            &nbsp Hasta
                            <%: Html.TextBox("fInicioHasta", Session["fInicioHasta"], new { @class = "date" })%>
                        </div>
                    </div>
                    <div class="float_left">
                        <div class="editor-label">
                            Fecha Seguimiento
                        </div>
                        <div class="editor-label">
                            &nbsp Desde
                            <%: Html.TextBox("fSeguimientoDesde", Session["fSeguimientoDesde"], new { @class = "date" })%>
                        </div>
                        <br />
                        <div class="editor-label">
                            &nbsp Hasta
                            <%: Html.TextBox("fSeguimientoHasta", Session["fSeguimientoHasta"], new { @class = "date" })%>
                        </div>
                    </div>

                    <div class="float_left">
                        <div class="editor-label">
                            Fecha Fin Prevista
                        </div>
                        <div class="editor-label">
                            &nbsp Desde
                            <%: Html.TextBox("fFinDesde", Session["fFinDesde"], new { @class = "date" })%>
                        </div>
                        <br />
                        <div class="editor-label">
                            &nbsp Hasta
                            <%: Html.TextBox("fFinHasta", Session["fFinHasta"], new { @class = "date" })%>
                        </div>
                        </div>
                        <div class="float_left">
                        <div class="editor-label">
                            Fecha Fin Real
                        </div>
                        <div class="editor-label">
                            &nbsp Desde
                            <%: Html.TextBox("fFinRealDesde", Session["fFinRealDesde"], new { @class = "date" })%>
                        </div>
                        <br />
                        <div class="editor-label">
                            &nbsp Hasta
                            <%: Html.TextBox("fFinRealHasta", Session["fFinRealHasta"], new { @class = "date" })%>
                        </div>
                    </div>
                    <div class="clear">
                        <div class="float_left">
                            <div class="editor-label">
                                Origenes
                            </div>
                            <div class="editor-field checkboxlist">
                                    <% AplicacionesGM_MVC.Models.CheckBoxModel chkOrigenes = (AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkOrigenes"];
                                foreach (var info in chkOrigenes.lstValores)
                                { %>
                                        <div>
                                            <%: Html.CheckBox("arrOrigenes", false, new { value = info.Key.ToString() })%>
                                            <%: info.Value%>
                                        </div>
                                <% } %>
                            </div>
                        </div>
                        <div class="float_left">
                            <div class="editor-label">
                                Departamentos
                            </div>
                            <div class="editor-field checkboxlist">
                                    <% AplicacionesGM_MVC.Models.CheckBoxModel chkDepartamentos = (AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkDepartamentos"];
                                foreach (var info in chkDepartamentos.lstValores)
                                { %>
                                        <div>
                                            <%: Html.CheckBox("arrDepartamentos", false, new { value = info.Key.ToString() })%>
                                            <%: info.Value%>
                                        </div>
                                <% } %>
                            </div>
                        </div>
                        <div class="float_left">
                            <div class="editor-label">
                                Delegaciones
                            </div>
                            <div class="editor-field checkboxlist">
                                <% AplicacionesGM_MVC.Models.CheckBoxModel chkDelegaciones = (AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkDelegaciones"];
                                foreach (var info in chkDelegaciones.lstValores)
                                { %>
                                        <div>
                                            <%: Html.CheckBox("arrDelegaciones", false, new { value = info.Key.ToString() })%>
                                            <%: info.Value%>
                                        </div>
                                <% } %>
                            </div>
                        </div>
                        <div class="float_left">
                            <div class="editor-label">
                                Responsables
                            </div>
                            <div class="editor-field checkboxlist">
                                <% AplicacionesGM_MVC.Models.CheckBoxModel chkResponsables = (AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkResponsables"];
                                foreach (var info in chkResponsables.lstValores)
                                { %>
                                        <div>
                                            <%: Html.CheckBox("arrResponsables", false, new { value = info.Key.ToString() })%>
                                            <%: info.Value%>
                                        </div>
                                <% } %>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col25">
                    <div class="editor-label">
                        Accion
                    </div>
                    <div class="editor-field">
                        <%: Html.TextArea("Accion",(Session["Accion"] ??"").ToString(), new { @class = "textarea-68" })%>
                    </div>
            
                    <div class="editor-label">
                        Objetivo
                    </div>
                    <div class="editor-field">
                        <%: Html.TextArea("Objetivo", (Session["Objetivo"] ??"").ToString(), new { @class = "textarea-68" })%>
                    </div>
            
                    <div class="editor-label">
                        Resultado
                    </div>
                    <div class="editor-field">
                        <%: Html.TextArea("Resultado", (Session["Resultado"]??"").ToString(), new { @class = "textarea-68" })%>
                    </div>
                </div>
                <div class="clear">
                    <p class="float_right">
                        <input type="submit" value="Filtrar" />
                    </p>
                </div>
            <% } %>
        </fieldset>
        <br />
        <div id="iconos">
            <a href="<%: @Url.Action("ExportAccionesListToExcel","Acciones", new{tab=ViewData["Title"]}) %>"><img class="imgLink" title="Exportar a Excel" alt="ExportarAExcel" longdesc="Exportar a Excel" src="../../Content/images/iconos/excel.png"/></a>
        </div>              
        <div class="chromestyle" id="chromemenu3">
            <ul>              
                <li><%: Html.ActionLink("Pendientes", "Index", "Acciones")%></li>
                <li><%: Html.ActionLink("Historico (" + @Model.Count() + ")", "Historico", "Acciones")%></li>
            </ul>
        </div>
    <% if (!Model.Any())
        { %>
            <h2><%: ViewData["NoDataFound"]%> </h2>
    <% }
        else
        { %>

            <table>
                        <tr class="textaling-center">
                            <th>
                                <%: Html.ActionLink("Origen","Historico",new {sortOrder="Origen", sortType=ViewData["sortType"]})%>
                            </th>
                            <th>
                                <%: Html.ActionLink("Dpto.", "Historico", new { sortOrder = "Departamento", sortType = ViewData["sortType"] }, new { title = "Click para ordenar" })%>
                            </th>
                            <th>
                                Delegaciones
                            </th>
                            <th>
                                Responsables
                            </th>
                            <th>
                                <%: Html.ActionLink("Fecha Inicio", "Historico", new { sortOrder = "FechaInicio", sortType = ViewData["sortType"] }, new { title = "Click para ordenar" })%>
                            </th>
                            <th>
                                <%: Html.ActionLink("Fecha Fin Prev", "Historico", new { sortOrder = "FechaFinPrev", sortType = ViewData["sortType"] }, new { title = "Click para ordenar" })%>
                            </th> 
                            <th>
                                <%: Html.ActionLink("Fecha Fin Real", "Historico", new { sortOrder = "FechaFinReal", sortType = ViewData["sortType"] }, new { title = "Click para ordenar" })%>
                            </th>
                            <th>
                                <%: Html.ActionLink("Fecha Seguimiento", "Historico", new { sortOrder = "FechaSeguimiento", sortType = ViewData["sortType"] }, new { title = "Click para ordenar" })%>
                            </th>
                            <th>
                                Accion
                            </th>
                            <th>
                                Objetivo
                            </th>
                            <th>
                                Resultado
                            </th>
                            <th></th>
                        </tr>

                        <% foreach (var item in Model)
                           { %>
    
                        <tr>
                            <td>
                                <%: item.aspnet_Origenes.Descripcion%>
                            </td>
                            <td>
                                <%: item.aspnet_Departamentos.Descripcion%>
                            </td>
                            <td>
                                <%  Dictionary<int,string> delegaciones= (Dictionary<int,string>) ViewData["Delegaciones"];
                                    string strDelegaciones = "";
                                    foreach(int intKey in delegaciones.Keys)
                                    {
                                        if (item.Delegaciones.Contains(intKey.ToString()))
                                        {
                                            if (strDelegaciones.Length == 0)
                                            {
                                                strDelegaciones += delegaciones[intKey].ToString();
                                            }
                                            else
                                            {
                                                strDelegaciones += ", " + delegaciones[intKey].ToString();
                                            }
                                        }
                                    } 
                                   %>
                                <%: strDelegaciones %>
                            </td>
                            <td>
                                <%  Dictionary<Guid, string> responsables = (Dictionary<Guid, string>)ViewData["Responsables"];
                                    string strResponsables = "";
                                    foreach (Guid intKey in responsables.Keys)
                                    {
                                        if (item.Responsables.Contains(intKey.ToString()))
                                        {
                                            if (strResponsables.Length == 0)
                                            {
                                                strResponsables += responsables[intKey].ToString();
                                            }
                                            else
                                            {
                                                strResponsables += ", " + responsables[intKey].ToString();
                                            }
                                        }
                                    } 
                                   %>
                                <%: strResponsables %>
                            </td>
                            <td>
                                <%: String.Format("{0:d}", item.FechaInicio)%>
                            </td>
                            <td>
                                <%: String.Format("{0:d}", item.FechaFinPrev)%>
                            </td>
                            <td>
                                <%: String.Format("{0:d}", item.FechaFinReal)%>
                            </td>
                            <td>
                                <%: String.Format("{0:d}", item.FechaSeguimiento)%>
                            </td>
                            <td>
                                <%: item.Accion%>
                            </td>
                            <td>
                                <%: item.Objetivo%>
                            </td>
                            <td>
                                <%: item.Resultado%>
                            </td>
                            <td>
                                <%: Html.ActionLink("Modificar", "Edit", new { id = item.AccionesID }, new { @class = "button button-index"})%>
                            </td>
                        </tr>
                    <% } %>
                </table>
            <% } %>
</asp:Content>

