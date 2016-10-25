<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Clientes>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Modificar Cliente
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">

    <% string strTitulo = "Modificar Cliente";
       if (Model.QSID != null)
       {
           strTitulo = "Consulta Cliente";
       }
       %>
        <h2><%: strTitulo %></h2>


    <% using (Html.BeginForm("Edit", "Clientes", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
        <%: Html.ValidationSummary(true, "Error al guardar el cliente. Revise los errores y vuelva a intentarlo")%>
            <div id="lblBorrador">Borrador</div>
            <div class="chromestyle" id="chromemenu">
                <ul>
                    <li><%: Html.ActionLink("Volver", "Index")%></li>
                    <% if (Model.QSID == null || User.IsInRole("Administrador")){ %>
                        <li><a id="borrador">Guardar Borrador</a></li>
                        <li><a id="submit">Guardar</a></li>
                    <% } %>
                    
                </ul>
            </div>
        <fieldset>
            <table id="DatosGenerales">
                <caption>Datos Generales</caption>
                <tbody>
                    <tr>
                        <td class="editor-label required noborder td20">
                            <%: Html.LabelFor(model => model.Empresas)%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.HiddenFor(model => model.EsBorrador) %>
                            <%: Html.HiddenFor(model => model.QSID) %>
                            <% AplicacionesGM_MVC.Models.CheckBoxModel chkEmpresas = (AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkEmpresas"];
                               bool blnChecked = false;
                               foreach (var info in chkEmpresas.lstValores)
                               {
                                   blnChecked = false;
                                   if (Model.Empresas != null)
                                   {
                                       if (Model.Empresas.Split(',').Contains(info.Key.ToString()))
                                       {
                                           chkEmpresas.arrValoresSelected.Add(info.Key.ToString());
                                           blnChecked = true;
                                       }
                                   }
                                    %>
                                        <input type='checkbox' name='arrEmpresas' id='Empresas' class='<%: info.Value %>' value='<%:info.Key.ToString() %>' <% if (blnChecked) { %> checked='checked' <%} %> />
                                        <input type="hidden" name='arrEmpresas' id='Empresas' value='false' />
                                        <%: info.Value%>
                            <% } %>
                            <%: Html.ValidationMessageFor(model => model.Empresas, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            Agentes
                        </td>
                        <td class="editor-field noborder td80">
                            <label for="IDAgenteQSMV" class="display-label" id="lblAgenteMV">MV</label>
                            <%: Html.DropDownListFor(model => model.IDAgenteQSMV, (SelectList)ViewData["AgentesMV"], "--Sin Agente--", new { @class = "textbox20" })%>
                            <label for="FormaContacto" class="display-label" id="lblAgenteHMA">HMA</label>
                            <%: Html.DropDownListFor(model => model.IDAgenteQSHMA, (SelectList)ViewData["AgentesHMA"], "--Sin Agente--", new { @class = "textbox20" })%>
                            <label for="FormaContacto" class="display-label" id="lblAgenteECA">ECA</label>
                            <%: Html.DropDownListFor(model => model.IDAgenteQSECA, (SelectList)ViewData["AgentesECA"],"--Sin Agente--", new { @class = "textbox20" })%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                            Delegación
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.DelegacionID, (SelectList)ViewData["Delegaciones"], "--Seleccione un valor--", null)%>
                            <%: Html.ValidationMessageFor(model => model.DelegacionID, true)%>
                        </td>
                    </tr>
                    <tr id="panelExposicion">
                        <td class="editor-label required noborder td20">
                            ¿Es un cliente de exposición o mostrador?
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.EsDeExposicion, new SelectList(
                                        new[]{
                                                new{Value="false", Text="No"}, 
                                                new{Value="true", Text="Sí"},
                                             },
                                             "Value",
                                             "Text",
                                             Model), new { @class = "textbox5" })%>
                        </td>
                    </tr>
                    <tr id="panelParticular">
                        <td class="editor-label required noborder td20">
                            ¿Es un cliente particular?
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.EsClienteParticular, new SelectList(
                                        new[]{
                                                new{Value="false", Text="No"}, 
                                                new{Value="true", Text="Sí"},
                                             },
                                             "Value",
                                             "Text",
                                             Model), new { @class = "textbox5" })%>
                        </td>
                    </tr>
                    <tr id="panelRecogen">
                        <td class="editor-label required noborder td20">
                            ¿Recoge en nuestras instalaciones?
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.RecogeEnNuestrasInstalaciones, new SelectList(
                                        new[]{
                                                new{Value="false", Text="No"}, 
                                                new{Value="true", Text="Sí"},
                                             },
                                             "Value",
                                             "Text",
                                             Model), new { @class = "textbox5" })%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                            <%: Html.RadioButtonFor(model => model.TipoDocumento, 1, new { @id = "radioNIF" })%> D.N.I / N.I.F / N.I.E <%: Html.RadioButtonFor(model => model.TipoDocumento, 2, new { @id = "radioCIF" })%> C.I.F
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.NIF, new { @class = "textbox10" })%>
                            <%: Html.ValidationMessageFor(model => model.NIF, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                           <label id="lblNombre">Nombre y apellidos</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.Nombre, new { @class = "textbox100" })%>
                            <%: Html.ValidationMessageFor(model => model.Nombre, true)%>
                        </td>
                    </tr>
                    <tr class="border-top">
                        <td colspan="2" align="center" class="editor-label noborder border-bottom"><span class="title">DIRECCIÓN FISCAL (PARA DATOS FACTURA)</span></td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            Domicilio
                        </td>
                        <td class="editor-field noborder td80">
                            <label for="TipoDeVia" class="display-label required">Tipo Vía</label>  <%: Html.DropDownListFor(model => model.TipoDeVia, (SelectList)ViewData["TiposDeVia"], "--Seleccione un valor--", new { @class = "textbox15" })%>
                            <%:Html.ValidationMessageFor(model => model.TipoDeVia, true)%>
                            <label for="Domicilio" class="display-label required" >Nombre</label>  <%: Html.TextBoxFor(model => model.Domicilio, new { @class = "textbox30" })%>
                            <%:Html.ValidationMessageFor(model => model.Domicilio, true)%>
                            <label for="Numero" class="display-label required" id="lblNumero">Nº</label>  <%: Html.TextBoxFor(model => model.Numero, new { @class = "textbox5" })%>
                            <%:Html.ValidationMessageFor(model => model.Numero, true)%>
                            <%: Html.CheckBoxFor(model => model.SinNumero)%> S/N
                            <label for="Numero" class="display-label">Piso</label> <%: Html.TextBoxFor(model => model.Piso, new { @class = "textbox15" })%>
                            <%:Html.ValidationMessageFor(model => model.Piso, true)%>
                        </td>
                    </tr>
                    <tr class="border-bottom">
                        <td class="editor-label required noborder td20">
                            <label for="CP" class="display-label">CP</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.CP, new { @class = "textbox10" })%>
                            <%:Html.ValidationMessageFor(model => model.CP, true)%>
                            <label for="Municipio" class="display-label required">Municipio</label>
                            <%: Html.DropDownListFor(model => model.Municipio, (SelectList)ViewData["Municipios"], "--Seleccione un valor--", null)%>
                            <%:Html.ValidationMessageFor(model => model.Municipio, true)%>
                            <%: Html.HiddenFor(model=>model.IDMunicipioQS) %> <!-- se usa para almacenar el código del municipio y utilizarlo en el cálculo de la ruta en caso de que se modifique el código postal !-->
                            <label for="IDProvinciaQS" class="display-label required">Provincia</label>
                            <%: Html.DropDownListFor(model => model.IDProvinciaQS, (SelectList)ViewData["Provincias"], "--Seleccione un valor--", null)%>
                            <%:Html.ValidationMessageFor(model => model.IDProvinciaQS, true)%>
                            <label for="IDPaisQS" class="display-label required">País</label>
                            <%: Html.DropDownListFor(model => model.IDPaisQS, (SelectList)ViewData["Paises"], "--Seleccione un valor--", new { @class = "textbox10" })%>
                            <%:Html.ValidationMessageFor(model => model.IDPaisQS, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder required td20">
                            Ruta
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.Zona, (SelectList)ViewData["Zonas"], "--Seleccione un valor--", new { @class = "textbox40" })%>
                            <%: Html.ValidationMessageFor(model => model.Zona, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                            <%: Html.LabelFor(model => model.Telefono1)%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.Telefono1, String.Format("{0:F}", Model.Telefono1))%>
                            <%:Html.ValidationMessageFor(model => model.Telefono1, true)%>
                            <%: Html.LabelFor(model => model.Telefono2)%>
                            <%: Html.TextBoxFor(model => model.Telefono2, String.Format("{0:F}", Model.Telefono2))%>
                            <%: Html.LabelFor(model => model.Fax)%>
                            <%: Html.TextBoxFor(model => model.Fax, String.Format("{0:F}", Model.Fax))%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                            ¿Tiene Mail?
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.TieneMail, new SelectList(
                                        new[]{
                                                new{Value="true", Text="Sí"}, 
                                                new{Value="false", Text="No"}, 
                                             },
                                             "Value",
                                             "Text",
                                             Model), new { @class = "textbox5" })%>
                            <div id="panelMails" style="display:inline;">
                                <label class="editor-label required" >Mail de contacto</label>
                                <%: Html.TextBoxFor(model => model.MailDeContacto, new { @class = "textbox40" })%>
                                <%: Html.ValidationMessageFor(model => model.MailDeContacto, true)%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.LabelFor(model => model.Web)%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.Web, new { @class = "textbox100" })%>
                            <%: Html.ValidationMessageFor(model => model.Web, true)%>
                        </td>
                    </tr>
                    <tr id="panelContacto">
                        <td class="editor-label noborder td20">
                            Persona de contacto
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.Contacto, new { @class = "textbox100" })%>
                            <%: Html.ValidationMessageFor(model => model.Web, true)%>
                        </td>
                    </tr>
                    <tr id="panelActividad">
                        <td class="editor-label required noborder td20">
                            Actividad
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.IDActividadQS, (SelectList)ViewData["Actividades"], "--Seleccione un valor--", new { @class = "textbox30" })%>
                            <%: Html.ValidationMessageFor(model => model.IDActividadQS, true)%>
                            <label for="FormaContacto" class="display-label required">Forma de contacto</label>
                            <%: Html.DropDownListFor(model => model.FormaContacto, (SelectList)ViewData["FormasDeContacto"], "--Seleccione un valor--", new { @class = "textbox30" })%>
                            <%: Html.ValidationMessageFor(model => model.FormaContacto, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                            ¿Tiene personas autorizadas para recoger material?
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.TienePersonasAutorizadasRetMat, new SelectList(
                                        new[]{
                                                new{Value="true", Text="Sí"}, 
                                                new{Value="false", Text="No"}, 
                                             },
                                             "Value",
                                             "Text",
                                             Model), new { @class = "textbox5" })%>
                        </td>
                    </tr>
                    <tr >
                        <td class="noborder" colspan="2">
                            <table id="panelPersonaAutorizada">
                                <tr>
                                    <td class="editor-label noborder border-bottom td20">
                                        Personas autorizadas retirada material (máximo 2 personas)
                                    </td>
                                    <td class="editor-field noborder border-bottom td80">
                                        <span id="addPersona" class="button" title="Pulse para añadir persona autorizada" >Añadir Persona</span>
                                    </td>
                                </tr>
                                <% if (Model.aspnet_PersonasRetiradaMat.Count() == 0)
                                   { %>
                                    <tr>
                                        <td class="noborder td20">
                                            <span class="button" title="Pulse para eliminar la persona autorizada" onclick="if ($('#panelPersonaAutorizada >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Persona</span>
                                        </td>
                                        <td class="noborder td80">
                                            <label class="editor-label required">N.I.F / N.I.E</label> <%: Html.TextBox("arrNIFPersona1", "", new { @class = "textbox10 required" })%>
                                            <%: Html.ValidationMessage("arrNIFPersona1", blnAddValmsg: true)%> 
                                            <label class="editor-label required">Nombre y apellidos </label> <%: Html.TextBox("arrNombrePersona1", "", new { @class = "textbox56 required" })%>
                                            <%: Html.ValidationMessage("arrNombrePersona1", blnAddValmsg: true)%> 
                                        </td>
                                    </tr>
                                   <%}
                                   else
                                   {
                                       int i = 0;
                                       foreach (var persona in Model.aspnet_PersonasRetiradaMat)
                                       {
                                           i++;%>
                                            <tr>
                                                <td class="noborder td20">
                                                    <span class="button" title="Pulse para eliminar la persona autorizada" onclick="if ($('#panelPersonaAutorizada >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Persona</span>
                                                </td>
                                                <td class="noborder td80">
                                                    <label class="editor-label required">N.I.F / N.I.E</label> <%: Html.TextBox("arrNIFPersona1", "arrNIFPersona" + i, persona.NIF, new { @class = "textbox10" })%>
                                                    <%: Html.ValidationMessage("arrNIFPersona" + i, blnAddValmsg: true)%> 
                                                     <label class="editor-label required">Nombre y apellidos </label> <%: Html.TextBox("arrNombrePersona1", "arrNombrePersona" + i, persona.Nombre, new { @class = "textbox56" })%>
                                                    <%: Html.ValidationMessage("arrNombrePersona" + i, blnAddValmsg: true)%> 
                                                </td>
                                            </tr>
                                      <%}
                                   }%>
                            </table>            
                        </td>
                    </tr>
                    <tr id="DatosGerente">
                        <td class="editor-label noborder td20">
                            Gerente / Administrador
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.Gerente, new { @class = "textbox40" })%>
                            <%: Html.ValidationMessageFor(model => model.Gerente, true)%>
                            ¿Tiene socios?
                            <%: Html.DropDownListFor(model => model.TieneSocios, new SelectList(
                                        new[]{
                                                new{Value="", Text="--Seleccione un valor--"},         
                                                new{Value="0", Text="No"}, 
                                                new{Value="1", Text="Sí"},
                                                new{Value="2", Text="No dispongo de la información"}
                                             },
                                             "Value",
                                             "Text",
                                             Model), new { @class = "textbox30" })%>
                            <%: Html.ValidationMessageFor(model => model.TieneSocios, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder" colspan="2">
                            
                            <table id="panelSocios">
                                <tr>
                                    <td class="editor-label required noborder border-bottom td20">
                                        Socios
                                    </td>
                                    <td class="editor-field noborder border-bottom td80">
                                        <span id="addSocio" class="button" title="Pulse para añadir un nuevo socio" >Añadir Socio</span>
                                    </td>
                                </tr>
                                <% if (Model.aspnet_Accionariado.Count() == 0)
                                   { %>
                                    <tr>
                                        <td class="noborder td20">
                                            <span class="button" title="Pulse para eliminar el socio" onclick="if ($('#panelSocios >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Socio</span>
                                        </td>
                                        <td class="noborder td80">
                                            C.I.F / N.I.F <%: Html.TextBox("arrCIFSocio1", "", new { @class = "textbox10" })%>
                                            <label class="editor-label required">Nombre / Empresa </label> <%: Html.TextBox("arrNombreSocio1", "", new { @class = "textbox40" })%>
                                            Porcentaje <%: Html.TextBox("arrPorcentaSocio1", "", new { @class = "textbox10" })%>
                                        </td>
                                    </tr>
                                   <%}
                                   else
                                   {
                                       int i = 0;
                                       foreach (var socio in Model.aspnet_Accionariado)
                                       {
                                           i++;%>
                                            <tr>
                                                <td class="noborder td20">
                                                    <span class="button" title="Pulse para eliminar el socio" onclick="if ($('#panelSocios >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Socio</span>
                                                </td>
                                                <td class="noborder td80">
                                                    C.I.F <%: Html.TextBox("arrCIFSocio1", "arrCIFSocio" + i, socio.CIF, new { @class = "textbox10" })%>
                                                    <%: Html.ValidationMessage("arrCIFSocio" + i, blnAddValmsg: true)%> 
                                                     <label class="editor-label required">Nombre </label> <%: Html.TextBox("arrNombreSocio1", "arrNombreSocio" + i, socio.Nombre, new { @class = "textbox40" })%>
                                                    <%: Html.ValidationMessage("arrNombreSocio" + i, blnAddValmsg: true)%> 
                                                    Porcentaje <%: Html.TextBox("arrPorcentaSocio1", "arrPorcentaSocio" + i, socio.Porcentaje, new { @class = "textbox10" })%>
                                                    <%: Html.ValidationMessage("arrPorcentaSocio" + i, blnAddValmsg: true)%>
                                                </td>
                                            </tr>
                                      <%}
                                   }%>
                            </table>            
                        </td>
                    </tr>
                    <tr id="DatosGrupoEmpresarial">
                        <td class="editor-label noborder td20">
                            Grupo empresarial
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.GrupoEmpresarial, new { @class = "textbox40" })%>
                            <%: Html.ValidationMessageFor(model => model.GrupoEmpresarial, true)%>
                            ¿Tiene empresas vinculadas?
                            <%: Html.DropDownListFor(model => model.TieneEmpresasVinculadas, new SelectList(
                                        new[]{
                                                new{Value="", Text="--Seleccione un valor--"},     
                                                new{Value="0", Text="No"}, 
                                                new{Value="1", Text="Sí"},
                                                new{Value="2", Text="No dispongo de la información"}
                                             },
                                             "Value",
                                             "Text",
                                             Model), new { @class = "textbox30" })%>
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder" colspan="2">
                            <table id="panelEmpresasVinculadas">
                                <tr>
                                    <td class="editor-label required noborder border-bottom td20">
                                        Empresas Vinculadas
                                    </td>
                                    <td class="editor-field noborder border-bottom td80">
                                        <span id="addEmpresaVinculada" class="button" title="Pulse para vincular una nueva empresa al grupo empresarial" >Añadir Empresa Vinculada</span>
                                    </td>
                                </tr>
                                    <% if (Model.aspnet_SociedadesVinculadas.Count() == 0)
                                       {%>
                                            <tr>
                                               <td class="noborder td20">
                                                    <span class="button" title="Pulse para desvincular la empresa del grupo empresarial" onclick="if ($('#panelEmpresasVinculadas >tbody >tr').length>2) { $(this).closest('tr').remove();}" >Quitar Empresa</span>
                                               </td>
                                               <td class=" noborder td80">
                                                    C.I.F <%: Html.TextBox("arrCIFVinc1", "", new { @class = "textbox10" })%>
                                                    <%: Html.ValidationMessage("arrCIFVinc1", blnAddValmsg: true)%> 
                                                    <label class="display-label required">Nombre</label> 
                                                    <%: Html.TextBox("arrEmpVinc1", "", new { @class = "textbox70" })%>
                                                    <%: Html.ValidationMessage("arrEmpVinc1", blnAddValmsg: true)%>
                                                </td>
                                            </tr>                                                
                                      <%}
                                       else
                                       {
                                           int i = 0;
                                           foreach (var sociedad in Model.aspnet_SociedadesVinculadas)
                                           {
                                               i++;%>
                                               <tr>
                                                   <td class="noborder td20">
                                                        <span class="button" title="Pulse para desvincular la empresa del grupo empresarial" onclick="if ($('#panelEmpresasVinculadas >tbody >tr').length>2) { $(this).closest('tr').remove();}" >Quitar Empresa</span>
                                                   </td>
                                                   <td class=" noborder td80">
                                                        C.I.F <%: Html.TextBox("arrCIFVinc1", "arrCIFVinc" + i, sociedad.CIF, new { @class = "textbox10" })%>
                                                        <%: Html.ValidationMessage("arrCIFVinc" + i, blnAddValmsg: true)%> 
                                                        <label class="display-label required">Nombre</label> <%: Html.TextBox("arrEmpVinc1", "arrEmpVinc" + i, sociedad.Nombre, new { @class = "textbox70" })%>
                                                        <%: Html.ValidationMessage("arrEmpVinc" + i, blnAddValmsg: true)%>
                                                    </td>
                                                </tr>                                                
                                         <%}
                                       }%>
                            </table>            
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder" colspan="2">
                            <table id="panelFichaLogistica">
                                <tr>
                                    <td colspan="2" align="center" class="editor-label noborder border-bottom"><span class="title">Ficha logística</span></td>
                                </tr>
                                <tr>
                                    <td class="editor-label noborder required td20">
                                        <label class="editor-label">Horario</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%:Html.TextBoxFor(model => model.Horario, new { @class = "textbox100" })%>
                                        <%:Html.ValidationMessageFor(model => model.Horario, true)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="editor-label noborder td20">
                                        <label class="editor-label">Horario de verano</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%:Html.TextBoxFor(model => model.HorarioDeVerano, new { @class = "textbox100" })%>
                                        <%:Html.ValidationMessageFor(model => model.HorarioDeVerano, true)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="editor-label noborder td20">
                                        <label class="editor-label">Persona de descarga</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%:Html.TextBoxFor(model => model.PersonaDeDescarga, new { @class = "textbox100" })%>
                                        <%:Html.ValidationMessageFor(model => model.PersonaDeDescarga, true)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="editor-label required noborder td20">
                                        <label class="editor-label">Medios de descarga</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%:Html.DropDownListFor(model => model.IDMedioDeDescarga, (SelectList)ViewData["MediosDeDescarga"], "--Seleccione un valor--", new { @class = "textbox20" })%>
                                        <%:Html.ValidationMessageFor(model => model.IDMedioDeDescarga, true)%>
                                        <label class="editor-label required ">¿Necesita camión con grúa?</label>
                                        <%: Html.DropDownListFor(model => model.NecesitaCamionConPluma, new SelectList(
                                                    new[]{
                                                            new{Value="true", Text="Sí"}, 
                                                            new{Value="false", Text="No"},
                                                         },
                                                         "Value",
                                                         "Text",
                                                         Model), new { @class = "textbox5" })%>
                                        <label class="editor-label required ">Vehículo apto para servicio</label>
                                        <%: Html.DropDownListFor(model => model.IDTipoVehiculoServicio, (SelectList)ViewData["TiposDeVehiculos"], "--Seleccione un valor--", new { @class = "textbox20" })%>
                                        <%:Html.ValidationMessageFor(model => model.IDTipoVehiculoServicio, true)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="editor-label required noborder td20">
                                        <label class="editor-label">Medios de transporte propios</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%: Html.DropDownListFor(model => model.MedioDeTransportePropio, new SelectList(
                                                    new[]{
                                                            new{Value="true", Text="Sí"}, 
                                                            new{Value="false", Text="No"},
                                                         },
                                                         "Value",
                                                         "Text",
                                                         Model), new { @class = "textbox5" })%>
                                        <label class="editor-label required">¿Dispone de espacio para almacenar?</label>
                                        <%: Html.DropDownListFor(model => model.EspacioParaAlmacenar, new SelectList(
                                                    new[]{
                                                            new{Value="true", Text="Sí"}, 
                                                            new{Value="false", Text="No"},
                                                         },
                                                         "Value",
                                                         "Text",
                                                         Model), new { @class = "textbox5" })%>
                                        <label class="editor-label required ">¿El cliente pesa el material?</label>
                                        <%: Html.DropDownListFor(model => model.PesaElMaterial, new SelectList(
                                                    new[]{
                                                            new{Value="true", Text="Sí"}, 
                                                            new{Value="false", Text="No"},
                                                         },
                                                         "Value",
                                                         "Text",
                                                         Model), new { @class = "textbox5" })%>
                            
                                    </td>
                                </tr>
                                <tr id="instrumentosDePesaje">
                                    <td class="editor-label required noborder td20">
                                        <label class="editor-label">Instrumentos de pesaje</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%:Html.DropDownListFor(model => model.IDInstrumentoDePesaje, (SelectList)ViewData["InstrumentosDePesaje"], "--Seleccione un valor--", new { @class = "textbox20" })%>
                                        <%:Html.ValidationMessageFor(model => model.IDInstrumentoDePesaje, true)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="editor-label required noborder td20">
                                        <label class="editor-label">Cobro de portes por envío</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%: Html.DropDownListFor(model => model.CobroDePortesPorEnvio, new SelectList(
                                                    new[]{
                                                            new{Value="true", Text="Sí"}, 
                                                            new{Value="false", Text="No"},
                                                         },
                                                         "Value",
                                                         "Text",
                                                         Model), new { @class = "textbox5" })%>
                                        <label class="editor-label required" id="lblImportePortesPorEnvio">Importe en euros a cobrar por envío:</label>
                                        <%:Html.TextBoxFor(model => model.ImportePortesPorEnvio, new { @class = "textbox10" })%>
                                        <%:Html.ValidationMessageFor(model => model.ImportePortesPorEnvio, true)%>
                                    </td>
                                </tr>
                                <tr id="requerimientosCalidad">
                                    <td class="editor-label noborder td20">
                                        <label class="editor-label">Requerimientos especiales de calidad</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <% AplicacionesGM_MVC.Models.CheckBoxModel chkRequerimientos = (AplicacionesGM_MVC.Models.CheckBoxModel)ViewData["chkRequerimientos"];
                                           blnChecked = false;
                                           foreach (var info in chkRequerimientos.lstValores)
                                           {
                                               blnChecked = false;
                                               if (Model.RequerimientosDeCalidad != null)
                                               {
                                                   if (Model.RequerimientosDeCalidad.Split(',').Contains(info.Key.ToString()))
                                                   {
                                                       chkRequerimientos.arrValoresSelected.Add(info.Key.ToString());
                                                       blnChecked = true;
                                                   }
                                               }
                                                %>
                                                    <%: Html.CheckBox("arrRequerimientosCal", blnChecked, new { value = info.Key.ToString() })%>
                                                    <%: info.Value%>
                                        <% } %>
                                    </td>
                                </tr>
                                <tr id="requerimientosPrevencion">
                                    <td class="editor-label noborder td20">
                                        <label class="editor-label">Requerimientos especiales de prevención</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%:Html.TextBoxFor(model => model.RequerimientosDePrevencion, new { @class = "textbox100" })%>
                                        <%:Html.ValidationMessageFor(model => model.RequerimientosDePrevencion, true)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="editor-label required noborder td20">
                                        <label class="editor-label">Coordinación de actividades firmada</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%: Html.DropDownListFor(model => model.CAEFirmada, new SelectList(
                                                    new[]{
                                                            new{Value="true", Text="Sí"}, 
                                                            new{Value="false", Text="No"},
                                                         },
                                                         "Value",
                                                         "Text",
                                                         Model), new { @class = "textbox5" })%>
                                     </td>
                                </tr>
                                <tr id="panelFicheroCAE">
                                    <%: Html.HiddenFor(model => model.FicheroCAE) %>
                                    <%: Html.HiddenFor(model => model.FicheroCAEanterior) %>
                                    <% if ((Model.FicheroCAE ?? "") == "")
                                       { %>
                                        <td class="editor-label required noborder td20">
                                            <label for="fileUpload">Seleccionar fichero de coordinación de actividades: </label>
                                        </td>
                                        <td class="editor-field noborder td80">
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                         </td>
                                    <%}
                                      else
                                      { %>
                                        <td class="editor-label required noborder td20">
                                            <label>Documento firmado: </label>
                                        </td>
                                        <td class="editor-field noborder td80">
                                            <a href="<%: Model.FicheroCAE %>" target="_blank">Ver documento</a>
                                        </td>
                                    <%} %>
                                </tr>
                                <tr id="panelNoFirma">
                                    <td class="editor-label required noborder td20">
                                        <label class="editor-label">Causa de no firma</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%:Html.DropDownListFor(model => model.IDCausaNoFirmaCAE, (SelectList)ViewData["CausasDeNoFirma"], "--Seleccione un valor--", new { @class = "textbox40" })%>
                                        <%:Html.ValidationMessageFor(model => model.IDCausaNoFirmaCAE, true)%>
                                     </td>
                                </tr>
                                <tr>
                                    <td class="editor-label required noborder td20">
                                        <label class="editor-label">¿Desea indicar direcciones de envío?</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%: Html.DropDownListFor(model => model.TieneDireccionesDeEnvio, new SelectList(
                                                    new[]{
                                                            new{Value="true", Text="Sí"}, 
                                                            new{Value="false", Text="No"},
                                                         },
                                                         "Value",
                                                         "Text",
                                                         Model), new { @class = "textbox5" })%>
                                    </td>
                                </tr>
                                <tr id="panelDireccionesDeEnvio">
                                    <td class="noborder" colspan="2">
                                        <table>
                                            <tr>
                                                <td class="editor-label noborder border-bottom td20">
                                                    Direcciones de Envío
                                                </td>
                                                <td class="editor-field noborder border-bottom td80">
                                                    <span id="addDirEnv" class="button" title="Pulse para añadir una nueva dirección de envío" >Añadir Dirección</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="noborder" colspan="2">
                                                    <table id="panelDireccionEnvio" class="noborder">
                                                        <% if (Model.aspnet_ClientesDirEnv.Count() == 0)
                                                            { %>
                                                            <tbody>
                                                                <tr>
                                                                    <td class="noborder td20" rowspan="4">
                                                                        <span class="button" title="Pulse para eliminar la dirección de envío" onclick="if ($('#panelDireccionEnvio >tbody').length>1) { $(this).closest('tbody').remove();}">Quitar Dirección</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="editor-field noborder td80">
                                                                        <label class="editor-label required"> Nombre y Apellidos / Razón social </label>
                                                                        <%: Html.TextBox("arrNombreDirEnv1", "", new { @class = "textbox70" })%>
                                                                        <%: Html.ValidationMessage("arrNombreDirEnv1", blnAddValmsg: true)%> 
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="editor-field noborder td80">
                                                                        <label for="arrTipoDeViaDirEnv1" class="display-label required">Tipo Vía</label>  <%: Html.DropDownList("arrTipoDeViaDirEnv1", (SelectList)ViewData["TiposDeVia"], "--Seleccione un valor--", new { @class = "textbox15" })%>
                                                                        <%: Html.ValidationMessage("arrTipoDeViaDirEnv1", blnAddValmsg: true)%> 
                                                                        <label for="arrDomicilioDirEnv1" class="display-label required" >Nombre</label>  <%: Html.TextBox("arrDomicilioDirEnv1", "", new { @class = "textbox30" })%>
                                                                        <%: Html.ValidationMessage("arrDomicilioDirEnv1", blnAddValmsg: true)%> 
                                                                        <label for="arrNumeroDirEnv1" class="display-label required" id="arrlblNumeroDirEnv1">Nº</label>  <%: Html.TextBox("arrNumeroDirEnv1", "", new { @class = "textbox5" })%>
                                                                        <%: Html.ValidationMessage("arrNumeroDirEnv1", blnAddValmsg: true)%> 
                                                                        <%: Html.CheckBox("arrSinNumeroDirEnv1")%> <label id="arrSinNumeroDirEnv1">S/N</label>
                                                                        <label for="arrPisoDirEnv1" class="display-label">Piso</label> <%: Html.TextBox("arrPisoDirEnv1", "", new { @class = "textbox15" })%>
                                                                        <%: Html.ValidationMessage("arrPisoDirEnv1", blnAddValmsg: true)%>
                                                                    </td>
                                                                </tr>
                                                                <tr class="border-bottom">
                                                                    <td class="editor-field noborder td80">
                                                                        <label for="arrCPDirEnv1" class="display-label">CP</label>
                                                                        <%: Html.TextBox("arrCPDirEnv1", "", new { @class = "textbox10" })%>
                                                                        <%: Html.ValidationMessage("arrCPDirEnv1", blnAddValmsg: true)%>
                                                                        <label for="arrMunicipioDirEnv1" class="display-label required">Municipio</label>
                                                                        <%: Html.DropDownList("arrMunicipioDirEnv1", (SelectList)ViewData["Municipios"], "--Seleccione un valor--", null)%>
                                                                        <%: Html.ValidationMessage("arrMunicipioDirEnv1", blnAddValmsg: true)%>
                                                                        <%: Html.Hidden("arrIDMunicipioQSDirEnv1")%> <!-- se usa para almacenar el código del municipio y utilizarlo en el cálculo de la ruta en caso de que se modifique el código postal !-->
                                                                        <label for="arrIDProvinciaQSDirEnv1" class="display-label required">Provincia</label>
                                                                        <%: Html.DropDownList("arrIDProvinciaQSDirEnv1", (SelectList)ViewData["Provincias"], "--Seleccione un valor--", null)%>
                                                                        <%: Html.ValidationMessage("arrIDProvinciaQSDirEnv1", blnAddValmsg: true)%>
                                                                        <label for="arrIDPaisQSDirEnv1" class="display-label required">País</label>
                                                                        <%: Html.DropDownList("arrIDPaisQSDirEnv1", (SelectList)ViewData["Paises"], "--Seleccione un valor--", new { @class = "textbox10" })%>
                                                                        <%: Html.ValidationMessage("arrIDPaisQSDirEnv1", blnAddValmsg: true)%>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        <% }
                                                        else
                                                            { 
                                                                int i=0;
                                                                foreach (var direccion in Model.aspnet_ClientesDirEnv)
                                                                {
                                                                    i++; 
                                                                    %>
                                                                    <tbody>
                                                                        <tr>
                                                                            <td class="noborder td20" rowspan="4">
                                                                                <span class="button" title="Pulse para eliminar la dirección de envío" onclick="if ($('#panelDireccionEnvio >tbody').length>1) { $(this).closest('tbody').remove();}">Quitar Dirección</span>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="editor-field noborder td80">
                                                                                <label class="editor-label required"> Nombre y Apellidos / Razón social </label>
                                                                                <%: Html.TextBox("arrNombreDirEnv1", "arrNombreDirEnv"+i, direccion.Nombre.ToString(),  new { @class = "textbox70" })%>
                                                                                <%: Html.ValidationMessage("arrNombreDirEnv"+i, blnAddValmsg: true)%> 
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="editor-field noborder td80">
                                                                                <label for="arrTipoDeViaDirEnv1" class="display-label required">Tipo Vía</label>  <%: Html.DropDownList("arrTipoDeViaDirEnv1", new SelectList((IEnumerable)ViewData["TiposDeViaDE"],"IDTipoVia","Nombre", direccion.TipoDeVía) , "--Seleccione un valor--", new { @class = "textbox15", @id="arrTipoDeViaDirEnv" +i })%>
                                                                                <%: Html.ValidationMessage("arrTipoDeViaDirEnv" + i, blnAddValmsg: true)%> 
                                                                                <label for="arrDomicilioDirEnv1" class="display-label required" >Nombre</label>  <%: Html.TextBox("arrDomicilioDirEnv1","arrDomicilioiDirEnv" + i, direccion.Domicilio, new { @class = "textbox30" })%>
                                                                                <%: Html.ValidationMessage("arrDomicilioDirEnv" + i, blnAddValmsg: true)%> 
                                                                                <label for="arrNumeroDirEnv1" class="display-label required" id="arrlblNumeroDirEnv" + i >Nº</label>  <%: Html.TextBox("arrNumeroDirEnv1", "arrNumeroDirEnv" + i, direccion.Numero, new { @class = "textbox5" })%>
                                                                                <%: Html.ValidationMessage("arrNumeroDirEnv" + i, blnAddValmsg: true)%> 
                                                                                <%: Html.CheckBox("arrSinNumeroDirEnv" + i)%> <label id="arrSinNumeroDirEnv"+i>S/N</label>
                                                                                <label for="arrPisoDirEnv1" class="display-label">Piso</label> <%: Html.TextBox("arrPisoDirEnv1", "arrPisoDirEnv"+i, direccion.Piso, new { @class = "textbox15" })%>
                                                                                <%: Html.ValidationMessage("arrPisoDirEnv" + i, blnAddValmsg: true)%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="border-bottom">
                                                                            <td class="editor-field noborder td80">
                                                                                <label for="arrCPDirEnv1" class="display-label">CP</label>
                                                                                <%: Html.TextBox("arrCPDirEnv1","arrCPDirEnv" + i, direccion.CP, new { @class = "textbox10" })%>
                                                                                <%: Html.ValidationMessage("arrCPDirEnv" + i, blnAddValmsg: true)%>
                                                                                <label for="arrMunicipioDirEnv1" class="display-label required">Municipio</label>
                                                                                <%: Html.DropDownList("arrMunicipioDirEnv1", new SelectList((IEnumerable)ViewData["Localizaciones"],"Municipio", "Municipio",direccion.Municipio), "--Seleccione un valor--", new {@id="arrMunicipioDirEnv"+i })%>
                                                                                <%: Html.ValidationMessage("arrMunicipioDirEnv" + i, blnAddValmsg: true)%>
                                                                                <%: Html.Hidden("arrIDMunicipioQSDirEnv1", direccion.IDMunicipioQS, new { @id = "arrIDMunicipioQSDirEnv" + i })%> <!-- se usa para almacenar el código del municipio y utilizarlo en el cálculo de la ruta en caso de que se modifique el código postal !-->
                                                                                <label for="arrIDProvinciaQSDirEnv1" class="display-label required">Provincia</label>
                                                                                <%: Html.DropDownList("arrIDProvinciaQSDirEnv1", new SelectList((IEnumerable)ViewData["Localizaciones"], "IDProvincia", "Provincia", direccion.IDProvinciaQS), "--Seleccione un valor--", new { @id = "arrIDProvinciaQSDirEnv" + i })%>
                                                                                <%: Html.ValidationMessage("arrIDProvinciaQSDirEnv" + i, blnAddValmsg: true)%>
                                                                                <label for="arrIDPaisQSDirEnv1" class="display-label required">País</label>
                                                                                <%: Html.DropDownList("arrIDPaisQSDirEnv1", new SelectList((IEnumerable)ViewData["Localizaciones"], "IDPais", "Pais", direccion.IDPaisQS), "--Seleccione un valor--", new { @class = "textbox10", @id = "arrIDPaisQSDirEnv" + i })%>
                                                                                <%: Html.ValidationMessage("arrIDPaisQSDirEnv" + i, blnAddValmsg: true)%>
                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                <%  } %>
                                                            <% } %>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>            
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="DatosEmpresariales">
                <caption>Datos empresariales</caption>
                <tbody>
                    <tr>
                        <td class="editor-label required noborder td20">
                            <%: Html.Label("Tipo de cliente")%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.TipoCliente, (SelectList)ViewData["TiposDeCliente"], "--Seleccione un valor--", new { @class = "textbox20" })%>
                            <%: Html.ValidationMessage("TipoCliente", blnAddValmsg: true)%> 
                            <label for="DiasVisita" class="display-label required">Días de Visita</label>
                            <%: Html.DropDownListFor(model => model.DiasVisita, (SelectList)ViewData["DiasDeVisita"], "--Seleccione un valor--", new { @class = "textbox20" })%>
                            <%: Html.ValidationMessage("DiasVisita", blnAddValmsg: true)%> 
                            <label for="FrecuenciaVisita" class="display-label required">Frecuencia de Visita</label>
                            <%: Html.DropDownListFor(model => model.FrecuenciaVisita, (SelectList)ViewData["FrecuenciaDeVisita"], "--Seleccione un valor--", new { @class = "textbox20" })%>
                            <%: Html.ValidationMessage("FrecuenciaVisita", blnAddValmsg: true)%> 
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            Año de antigüedad
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.Antguedad, new { @class = "textbox10" })%>
                            <%: Html.ValidationMessageFor(model => model.Antguedad, true)%>
                            <label for="ConsumoPotencial" class="display-label required">Consumo Potencial</label>
                            <%: Html.TextBoxFor(model => model.ConsumoPotencial, String.Format("{0:F}", Model.ConsumoPotencial))%> €
                            <%: Html.ValidationMessageFor(model => model.ConsumoPotencial, true)%>
                            <label for="PrevisionAnual" class="display-label required">Previsión Anual</label>
                            <%: Html.TextBoxFor(model => model.PrevisionAnual, String.Format("{0:F}", Model.PrevisionAnual))%> €
                            <%: Html.ValidationMessageFor(model => model.PrevisionAnual, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                            <label class="editor-label">¿Venta sujeta a contrato?</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.VentaSujetaAContrato, new SelectList(
                                    new[]{
                                            new{Value="true", Text="Sí"}, 
                                            new{Value="false", Text="No"},
                                            },
                                            "Value",
                                            "Text",
                                            Model), new { @class = "textbox5" })%>
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder td20">
                            <label>Cia. aseguradora de ventas</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.IDAseguradora, (SelectList)ViewData["Aseguradoras"],"--Seleccione un valor si corresponde--", new { @class = "textbox70" })%>
                            <%: Html.ValidationMessageFor(model => model.IDAseguradora, true)%>
                            <label for="NumTrabajadores" class="editor-label">N<sup>o</sup>. Trabajadores</label>
                            <%: Html.TextBoxFor(model => model.NumTrabajadores, new { @class = "textbox10" })%>
                            <%: Html.ValidationMessageFor(model => model.NumTrabajadores, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder" colspan="2">
                            <table class="noborder">
                                <thead>
                                    <tr>
                                        <th class="noborder td20"></th>
                                        <th>Oficinas</th>
                                        <th>Naves</th>
                                        <th>Terrenos</th>
                                        <th>Vehículos</th>
                                        <th>Maquinaria</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td><label class="editor-label">De alquiler</label></td>
                                        <td>
                                            <label class="display-label"></label><%: Html.TextBoxFor(model => model.OficinasA, new { @class = "textbox80" })%> m<sup>2</sup>
                                            <%: Html.ValidationMessageFor(model => model.OficinasA, true)%>
                                        </td>
                                        <td>
                                            <label class="display-label"></label><%: Html.TextBoxFor(model => model.NavesA, new { @class = "textbox80" })%> m<sup>2</sup>
                                            <%: Html.ValidationMessageFor(model => model.NavesA, true)%>
                                        </td>
                                        <td>
                                            <label class="display-label"></label><%: Html.TextBoxFor(model => model.TerrenosA, new { @class = "textbox80" })%> m<sup>2</sup>
                                            <%: Html.ValidationMessageFor(model => model.TerrenosA, true)%>
                                        </td>
                                        <td>
                                            <label class="display-label "></label>N<sup>o</sup><%: Html.TextBoxFor(model => model.VehiculosA, new { @class = "textbox80" })%> 
                                            <%: Html.ValidationMessageFor(model => model.VehiculosA, true)%>
                                        </td>
                                        <td>
                                            <label class="display-label "></label>N<sup>o</sup><%: Html.TextBoxFor(model => model.MaquinariaA, new { @class = "textbox80" })%> 
                                            <%: Html.ValidationMessageFor(model => model.MaquinariaA, true)%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><label class="editor-label">En propiedad</label></td>
                                        <td>
                                            <label class="display-label "></label><%: Html.TextBoxFor(model => model.OficinasP, new { @class = "textbox80" })%> m<sup>2</sup>
                                            <%: Html.ValidationMessageFor(model => model.OficinasP, true)%>
                                        </td>
                                        <td>
                                            <label class="display-label "></label><%: Html.TextBoxFor(model => model.NavesP, new { @class = "textbox80" })%> m<sup>2</sup>
                                            <%: Html.ValidationMessageFor(model => model.NavesP, true)%>
                                        </td>
                                        <td>
                                            <label class="display-label"></label><%: Html.TextBoxFor(model => model.TerrenosP, new { @class = "textbox80" })%> m<sup>2</sup>
                                            <%: Html.ValidationMessageFor(model => model.TerrenosP, true)%>
                                        </td>
                                        <td>
                                            <label class="display-label "></label>N<sup>o</sup><%: Html.TextBoxFor(model => model.VehiculosP, new { @class = "textbox80" })%> 
                                            <%: Html.ValidationMessageFor(model => model.VehiculosP, true)%>
                                        </td>
                                        <td>
                                            <label class="display-label "></label>N<sup>o</sup><%: Html.TextBoxFor(model => model.MaquinariaP, new { @class = "textbox80" })%> 
                                            <%: Html.ValidationMessageFor(model => model.MaquinariaP, true)%>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="DatosFacturacion">
                <caption>Datos facturación</caption>
                <tbody>
                    <tr id="MailFacturacion">
                        <td class="editor-label noborder td20">
                            <label id="lblMailFacturacion" class="editor-label required" >Mail de facturación electrónica</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.MailDeFacturacion, new { @class = "textbox40" })%>
                            <%: Html.ValidationMessageFor(model => model.MailDeFacturacion, true)%>
                            <%: Html.CheckBoxFor(model => model.NoAdmiteFacturacionElectronica)%> <label id="lblNoAdmiteFacturacionElectronica">El cliente no admite facturación electrónica</label>
                        </td>
                    </tr>
                    <tr id="EnvioPostal">
                        <td class="noborder" colspan="2">
                            <table class="noborder">
                                <tr class="border-top">
                                    <td colspan="2" align="center" class="editor-label noborder border-bottom"><span class="title">DIRECCIÓN POSTAL ENVÍO FACTURA</span></td>
                                </tr>
                                <tbody>
                                    <tr id="ApartadoDeCorreos">
                                        <td class="editor-label noborder td20">
                                            ¿Tiene apartado de correos para envío de facturas?
                                        </td>
                                        <td class="editor-field noborder td80">
                                            <%: Html.DropDownListFor(model => model.TieneApartadoPostalFacturacion, new SelectList(
                                                    new[]{
                                                            new{Value="true", Text="Sí"}, 
                                                            new{Value="false", Text="No"},
                                                            },
                                                            "Value",
                                                            "Text",
                                                            Model), new { @class = "textbox5" })%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="editor-label noborder td20">
                                            <label class="editor-label required" id="lblApartadoPostal">Nº apartado de correos</label>
                                        </td>
                                        <td class="editor-field noborder td80">
                                            <%: Html.TextBoxFor(model=>model.ApatadoDeCorreosFacturacion) %>
                                        </td>
                                    </tr>
                                </tbody>
                                <tbody id="DirEnvioFacturas">
                                    <tr>
                                        <td class="editor-label noborder td20">
                                            Domicilio
                                        </td>
                                        <td class="editor-field noborder td80">
                                            <label for="TipoDeViaFacturacion" class="display-label required">Tipo Vía</label>  <%: Html.DropDownListFor(model => model.TipoDeViaFacturacion, (SelectList)ViewData["TiposDeVia"], "--Seleccione un valor--", new { @class = "textbox15" })%>
                                            <%:Html.ValidationMessageFor(model => model.TipoDeViaFacturacion, true)%>
                                            <label for="DomicilioFacturacion" class="display-label required" >Nombre</label>  <%: Html.TextBoxFor(model => model.DomicilioFacturacion, new { @class = "textbox30" })%>
                                            <%:Html.ValidationMessageFor(model => model.DomicilioFacturacion, true)%>
                                            <label for="NumeroFacturacion" class="display-label required" id="lblNumeroFacturacion">Nº</label>  <%: Html.TextBoxFor(model => model.NumeroFacturacion, new { @class = "textbox5" })%>
                                            <%:Html.ValidationMessageFor(model => model.NumeroFacturacion, true)%>
                                            <%: Html.CheckBoxFor(model => model.SinNumeroFacturacion)%> S/N
                                            <label for="PisoFacturacion" class="display-label">Piso</label> <%: Html.TextBoxFor(model => model.PisoFacturacion, new { @class = "textbox15" })%>
                                            <%:Html.ValidationMessageFor(model => model.PisoFacturacion, true)%>
                                        </td>
                                    </tr>
                                </tbody>
                                <tr class="border-bottom">
                                    <td class="editor-label required noborder td20">
                                        <label for="CPFacturacion" class="display-label">CP</label>
                                    </td>
                                    <td class="editor-field noborder td80">
                                        <%: Html.TextBoxFor(model => model.CPFacturacion, new { @class = "textbox10" })%>
                                        <%:Html.ValidationMessageFor(model => model.CPFacturacion, true)%>
                                        <label for="Municipio" class="display-label required">Municipio</label>
                                        <%: Html.DropDownListFor(model => model.MunicipioFacturacion, (SelectList)ViewData["Municipios"], "--Seleccione un valor--", null)%>
                                        <%:Html.ValidationMessageFor(model => model.MunicipioFacturacion, true)%>
                                        <%: Html.HiddenFor(model=>model.IDMunicipioQSFacturacion) %> <!-- se usa para almacenar el código del municipio y utilizarlo en el cálculo de la ruta en caso de que se modifique el código postal !-->
                                        <label for="IDProvinciaQS" class="display-label required">Provincia</label>
                                        <%: Html.DropDownListFor(model => model.IDProvinciaQSFacturacion, (SelectList)ViewData["Provincias"], "--Seleccione un valor--", null)%>
                                        <%:Html.ValidationMessageFor(model => model.IDProvinciaQSFacturacion, true)%>
                                        <label for="IDPaisQS" class="display-label required">País</label>
                                        <%: Html.DropDownListFor(model => model.IDPaisQSFacturacion, (SelectList)ViewData["Paises"], "--Seleccione un valor--", new { @class = "textbox10" })%>
                                        <%:Html.ValidationMessageFor(model => model.IDPaisQSFacturacion, true)%>
                                    </td>
                                </tr>
                                
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                            <%: Html.LabelFor(model => model.FormaDePago)%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.FormaDePago, (SelectList)ViewData["FormasDePago"], "Otra (solicitar autorización)", new { @class = "textbox56" })%>
                            <%: Html.ValidationMessageFor(model => model.FormaDePago, true)%>
                            <label id = "lblDtoPP">Dto. P.P.</label>
                            <%: Html.TextBoxFor(model => model.DtoPP, new { @class = "textbox5 percent", @disabled = "disabled" })%>
                            <label id = "lblRecargoFinanciero">RecargoFinanciero</label>
                            <%: Html.TextBoxFor(model => model.RecargoFinanciero, new { @class = "textbox5", @disabled = "disabled" })%>
                            <label id = "lblTarifa">Tarifa</label>
                            <%: Html.TextBoxFor(model => model.Tarifa, new { @class = "textbox10", @disabled = "disabled", @title="Calculada sólo para clientes de Moraval en base al Tipo de cliente y a la actividad" })%>
                            <%: Html.ValidationMessageFor(model => model.Tarifa, true)%>
                            <%: Html.Hidden("EsSepa")%>
                        </td>
                    </tr>
                    <tr id="trFormaDePagoSolicitada">
                        <td class="editor-label required noborder td20">
                            <%: Html.LabelFor(model => model.FormaDePagoSolicitada)%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.FormaDePagoSolicitada, new { @class = "textbox56 " })%>
                            <%: Html.ValidationMessageFor(model => model.FormaDePagoSolicitada, true)%>
                        </td>
                    </tr>
                    <tr id="PedidoEnFirme">
                        <td class="editor-label required noborder td20">
                            ¿Existe pedido en firme?
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.DropDownListFor(model => model.ExistePedidoEnFirme, new SelectList(
                                        new[]{
                                                new{Value="true", Text="Sí"}, 
                                                new{Value="false", Text="No"},
                                             },
                                             "Value",
                                             "Text",
                                             Model), new { @class = "textbox5" })%>
                        </td>
                    </tr>
                    <tr id="IBAN">
                        <td id="lblIBAN" class="editor-label noborder td20">
                            IBAN
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.IBANSIGLAS, new { @Size = "2", @Maxlength="2", @Value="ES", })%>
                            <%: Html.TextBoxFor(model => model.IBANCODE, new { @size = "2", @maxlength="2" })%>
                            <%: Html.ValidationMessageFor(model => model.IBANCODE, true)%>
                            <%: Html.TextBoxFor(model => model.IBANENTIDAD, new { @size = "4", @maxlength="4" })%>
                            <%: Html.TextBoxFor(model => model.IBANSUCURSAL, new { @size = "4", @maxlength="4" })%>
                            <%: Html.TextBoxFor(model => model.IBANDC, new { @size = "2", @maxlength="2" })%>
                            <%: Html.TextBoxFor(model => model.IBANCCC, new { @size = "10", @maxlength="10" })%>
                        </td>
                    </tr>
                    <tr id="Vencimientos">
                        <td class="noborder td20">
                            <label id="lblDiasVtoFijo" class="editor-label required" >Días mes Vto. fijo</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.DiaVtoFijo1, new { @class = "textbox10" })%>
                            <%: Html.ValidationMessageFor(model => model.DiaVtoFijo1, true)%>
                            <%: Html.TextBoxFor(model => model.DiaVtoFijo2, new { @class = "textbox10" })%>  
                            <%: Html.ValidationMessageFor(model => model.DiaVtoFijo2, true)%>
                            <%: Html.TextBoxFor(model => model.DiaVtoFijo3, new { @class = "textbox10" })%>
                            <%: Html.ValidationMessageFor(model => model.DiaVtoFijo3, true)%>
                            <%: Html.CheckBoxFor(model => model.NoTieneVtosFijos)%> El cliente no tiene días de pago fijos                            
                        </td>
                    </tr>
                </tbody>
             </table>
            <table id="Creditos">
                <caption>Crédito</caption>
                <tbody>
                    <tr>
                        <td class="editor-label required noborder td20">
                            <%: Html.LabelFor(model => model.LimitePropuesto)%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.LimitePropuesto, new { @class = "textbox10" })%> €
                            <%: Html.ValidationMessageFor(model => model.LimitePropuesto, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder" colspan="2">
                            <table id="panelBancos">
                                <tr>
                                    <td class="editor-label required noborder border-bottom td20">
                                        Bancos con los que trabaja
                                    </td>
                                    <td class="editor-field noborder border-bottom td80">
                                        <span id="addBanco" class="button" title="Pulse para añadir un nuevo banco" >Añadir Banco</span>
                                    </td>
                                </tr>
                                <% if (Model.aspnet_BancosCliente.Count() == 0)
                                   {%>
                                    <tr>
                                        <td class="noborder td20">
                                            <span class="button" title="Pulse para eliminar el banco" onclick="if ($('#panelBancos >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Banco</span>
                                        </td>
                                        <td class="noborder td80">
                                            <label class="editor-label required">Nombre </label> <%: Html.TextBox("arrNombreBanco1", "", new { @class = "textbox56" })%>
                                            Oficina / Localidad <%: Html.TextBox("arrOficinaBanco1", "", new { @class = "textbox10" })%>
                                        </td>
                                    </tr>
                                <%}
                                   else
                                   {
                                       int i = 0;
                                       foreach (var banco in Model.aspnet_BancosCliente)
                                       {
                                           i++; %>
                                           <tr>
                                                <td class="noborder td20">
                                                    <span class="button" title="Pulse para eliminar el banco" onclick="if ($('#panelBancos >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Banco</span>
                                                </td>
                                                <td class="noborder td80">
                                                    <label class="editor-label required">Nombre </label> <%: Html.TextBox("arrNombreBanco1", "arrNombreBanco" + i, banco.Nombre, new { @class = "textbox46" })%>
                                                    <%: Html.ValidationMessage("arrNombreBanco" + i, blnAddValmsg: true)%>
                                                    Oficina / Localidad <%: Html.TextBox("arrOficinaBanco1", "arrOficinaBanco" + i, banco.Sucursal, new { @class = "textbox30" })%>
                                                    <%: Html.ValidationMessage("arrOficinaBanco" + i, blnAddValmsg: true)%>
                                                </td>
                                            </tr>
                                    <% } %>
                                <% } %>
                            </table>            
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="Consumo">
                <caption class="required">Pdto. Consumo</caption>
                <tbody>
                    <tr>
                        <td class="editor-label noborder td20"></td>
                        <td class="editor-field noborder td80">
                                <table class="noborder">
                                    <tr>
                                       <% Dictionary<string, AplicacionesGM_MVC.Models.CheckBoxModel> lstMatEmpresa = (Dictionary<string, AplicacionesGM_MVC.Models.CheckBoxModel>)ViewData["Materiales"];
                                          foreach (var mat in lstMatEmpresa)
                                          { %>
                                                <td class="noborder" valign="top"><label class="editor-label"><%: mat.Key.ToString()%></label><br />
                                                <div class="editor-field border-top">
                                                    <% AplicacionesGM_MVC.Models.CheckBoxModel chkMat = (AplicacionesGM_MVC.Models.CheckBoxModel)mat.Value;

                                                       blnChecked = false;
                                                       foreach (var info in chkMat.lstValores)
                                                       {
                                                           if (((Model.Consume) ?? "").ToString().Split(',').Contains(info.Key.ToString()))
                                                           {
                                                               blnChecked = true;
                                                           }
                                                           else
                                                           {
                                                               blnChecked = false;
                                                           }%>
                                                            <div>
                                                
                                                                <%: Html.CheckBox("arrMateriales", blnChecked, new { value = info.Key.ToString() })%>
                                                                <%: info.Value%>
                                                            </div>
                                                    <% } %>
                                                    <%: Html.ValidationMessage("arrMateriales", blnAddValmsg: true)%>
                                                </div>
                                                </td>
                                         <% } %>
                                     </tr>
                                </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="ClientesHabituales">
                <caption>Clientes habituales</caption>
                <tbody>
                    <tr>
                        <td class="noborder" colspan="2">
                            <table id="panelClientesHab">
                                <tr>
                                    <td class="editor-label required noborder border-bottom td20">
                                        Clientes habituales
                                    </td>
                                    <td class="editor-field noborder border-bottom td80">
                                        <span id="addClienteHab" class="button" title="Pulse para añadir un nuevo cliente" >Añadir Cliente</span>
                                    </td>
                                </tr>
                                <% if (Model.aspnet_ClientesHabituales.Count() == 0)
                                   { %>
                                        <tr>
                                            <td class="noborder td20">
                                                <span id="delCliente" class="button" title="Pulse para eliminar el cliente" onclick="if ($('#panelClientesHab >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Cliente</span>
                                            </td>
                                            <td class="noborder td80">
                                                N.I.F/C.I.F <%: Html.TextBox("arrCliHabCIF1", "", new { @class = "textbox10" })%>
                                                <label class="editor-label required"> Nombre </label><%: Html.TextBox("arrCliHabNombre1", "", new { @class = "textbox56" })%>
                                                <%: Html.ValidationMessage("arrCliHabNombre1", blnAddValmsg: true)%>
                                            </td>
                                        </tr>
                                <%}
                                   else
                                   {
                                       int i = 0;
                                       foreach (var cliHab in Model.aspnet_ClientesHabituales)
                                       {
                                           i++;%>
                                            <tr>
                                                <td class="noborder td20">
                                                    <span id="delCliente" class="button" title="Pulse para eliminar el cliente" onclick="if ($('#panelClientesHab >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Cliente</span>
                                                </td>
                                                <td class="noborder td80">
                                                    N.I.F/C.I.F <%: Html.TextBox("arrCliHabCIF1", "arrCliHabCIF" + i, cliHab.NIF, new { @class = "textbox10" })%>
                                                    <%: Html.ValidationMessage("arrCliHabCIF1", blnAddValmsg: true)%>
                                                    <label class="editor-label required"> Nombre </label><%: Html.TextBox("arrCliHabNombre1", "arrCliHabNombre" + i, cliHab.NombreCliente, new { @class = "textbox56" })%>
                                                    <%: Html.ValidationMessage("arrCliHabNombre" + i, blnAddValmsg: true)%>
                                                </td>
                                            </tr>
                                    <% } %>
                                <% } %>
                            </table>            
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="ProveedoresHabituales">
                <caption>Proveedores habituales</caption>
                <tbody>
                    <tr>
                        <td class="noborder" colspan="2">
                            <table id="panelProveedoresHab">
                                <tr>
                                    <td class="editor-label required noborder border-bottom td20">
                                        Proveedores habituales
                                    </td>
                                    <td class="editor-field noborder border-bottom td80">
                                        <span id="addProveedorHab" class="button" title="Pulse para añadir un nuevo proveedor" >Añadir Proveedor</span>
                                    </td>
                                </tr>
                                <% if (Model.aspnet_ProveedoresHabituales.Count() == 0)
                                   { %>
                                        <tr>
                                            <td class="noborder td20">
                                                <span id="delProveedorHab" class="button" title="Pulse para eliminar el proveedor" onclick="if ($('#panelProveedoresHab >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Proveedor</span>
                                            </td>
                                            <td class="noborder td80">
                                                N.I.F/C.I.F <%: Html.TextBox("arrProvHabCIF1", "", new { @class = "textbox10" })%>
                                                <label class="editor-label required"> Nombre </label><%: Html.TextBox("arrProvHabNombre1", "", new { @class = "textbox56" })%>
                                                <%: Html.ValidationMessage("arrProvHabNombre1", blnAddValmsg: true)%>
                                            </td>
                                        </tr>
                               <% }
                                   else
                                   {
                                       int i = 0;
                                       foreach (var provHab in Model.aspnet_ProveedoresHabituales)
                                       {
                                           i++;%>
                                            <tr>
                                                <td class="noborder td20">
                                                    <span id="delProveedorHab" class="button" title="Pulse para eliminar el proveedor" onclick="if ($('#panelProveedoresHab >tbody >tr').length>2) { $(this).closest('tr').remove();}">Quitar Proveedor</span>
                                                </td>
                                                <td class="noborder td80">
                                                    N.I.F/C.I.F <%: Html.TextBox("arrProvHabCIF1", "arrProvHabCIF" + i, provHab.NIF, new { @class = "textbox10" })%>
                                                    <%: Html.ValidationMessage("arrProvHabCIF" + i, blnAddValmsg: true)%>
                                                    <label class="editor-label required"> Nombre </label><%: Html.TextBox("arrProvHabNombre1", "arrProvHabNombre" + i, provHab.NombreProveedor, new { @class = "textbox56" })%>
                                                    <%: Html.ValidationMessage("arrProvHabNombre" + i, blnAddValmsg: true)%>
                                                </td>
                                            </tr>
                                    <% } %>
                               <% } %>
                            </table>            
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="OtrosDatos">
                <caption>Otros datos</caption>
                <tbody>
                    <tr>
                        <td class="editor-label noborder td20">
                            <label class="editor-label">Información adicional</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextAreaFor(model => model.Observaciones)%>
                            <%: Html.ValidationMessageFor(model => model.Observaciones, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.Label("Fecha de toma de datos")%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: String.Format("{0:d}", Model.FechaDeAlta)%> 
                            <%: Html.HiddenFor(model => model.FechaDeAlta)%>                           
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.Label("Usuario de toma de datos")%>
                        </td>
                        <td class="editor-field noborder td80">
                            <label><%: Model.aspnet_Users.UserName%></label>
                            <%: Html.HiddenFor(model => model.UsuarioDeAlta)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.Label("Usuario última modificación")%>
                        </td>
                        <td class="editor-field noborder td80">
                            <label><%: (Model.aspnet_Users1 == null) ? "" : Model.aspnet_Users1.UserName%></label>
                            <%: Html.HiddenFor(model => model.UsuarioUltimaModificacion)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label noborder td20">
                            <%: Html.Label("Fecha última modificación")%>
                        </td>
                        <td class="editor-field noborder td80">
                            <label><%: Model.UltimaModificación%></label> 
                            <%: Html.HiddenFor(model => model.UltimaModificación)%>
                        </td>
                    </tr>                    
                </tbody>
            </table>
        </fieldset>
    <% } %>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#EsBorrador').val() == "True") {
                $('#lblBorrador').show();
            } else {
                $('#lblBorrador').hide();
            }
            $('#submit').click(function () {
                $('#EsBorrador').val(false);
                $("#DtoPP").removeAttr("disabled");
                $("#RecargoFinanciero").removeAttr("disabled");
                $(this).closest('form').submit();
            });
            $('#borrador').click(function () {
                $('#EsBorrador').val(true);
                $("#DtoPP").removeAttr("disabled");
                $("#RecargoFinanciero").removeAttr("disabled");
                $(this).closest('form').submit();
            });

            //Si nos indican mail de contacto lo asignamos también al mail de factura electrónica
            $("#MailDeContacto").blur(function () {
                asignarMailDeFacturacion();
            });

            //Reemplazamos todo lo que no sean ni números ni letras en los campos que contengan NIF o CIF en su id
            $("input[id*='NIF']").blur(function () {
                $(this).val($(this).val().replace(/\W/g, ''));
            });
            $("input[id*='CIF']").blur(function () {
                $(this).val($(this).val().replace(/\W/g, ''));
            });

            //Reemplaza las comas por espacios cuando los campos cuyo id comienza por 'arr' pierden el foco. Es para evitar que al convertirse 
            //en el post en una cadena separada por comas luego se pueda hacer un split correctamente.
            $("input[id^='arr']").blur(function () {
                $(this).val($(this).val().replace(/,/g, ' '));
            });

            //Sólo dejamos los números
            $("#Numero").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("#Antguedad").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("#ConsumoPotencial").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("#PrevisionAnual").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("#LimitePropuesto").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("#ImportePortesPorEnvio").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });

            $("#NumTrabajadores").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("input[id^='Telefono']").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("input[id^='Fax']").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("input[id^='DiaVtoFijo']").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("input[id^='Oficinas']").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("input[id^='Naves']").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("input[id^='Terrenos']").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("input[id^='Vehiculos']").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });
            $("input[id^='Maquinaria']").blur(function () {
                $(this).val($(this).val().replace(/\D/g, ''));
            });

            mostrarAgentes();
            mostrarPanelExposicion();
            asignarTipoDocumento();
            asignarDatosPrevencion();
            mostrarPanelMails();
            mostrarPanelPersonaAutorizada();
            mostrarEmpresasVinculadas();
            mostrarDireccionesDeEnvio();
            mostrarSocios();
            mostrarCAE();
            mostrarImportePortesPorEnvio();
            mostrarFichaExposicion();
            noAdmiteFacturacionElectronica();
            mostrarVtosFijos();
            mostrarInstrumentosDePesaje();
            mostrarNumeroFacturacion();
            mostrarNumero();

            $('.MORAVAL').click(function () {
                mostrarPanelExposicion();
                mostrarAgentes();
            });
            $('.HMA').click(function () {
                mostrarPanelExposicion();
                mostrarAgentes();
            });
            $('.ECA').click(function () {
                mostrarPanelExposicion();
                mostrarAgentes();
            });

            $('#EsDeExposicion').change(function () {
                mostrarFichaExposicion();
            });

            $('#EsClienteParticular').change(function () {
                asignarDatosPrevencion();
            });

            $('#RecogeEnNuestrasInstalaciones').change(function () {
                asignarDatosPrevencion();
            });

            $('input[name=TipoDocumento]:radio').change(function () {
                asignarTipoDocumento();
            });

            $('#SinNumero').change(function () {
                mostrarNumero();
            });

            $('#CP').change(function () {
                cargarDatosCP();
            });

            $('#Municipio').change(function () {
                calcularZona();
            });

            $('#IDProvinciaQS').change(function () {
                calcularZona();
            });

            $('#IDPaisQS').change(function () {
                calcularZona();
            });

            $('#TieneMail').change(function () {
                mostrarPanelMails();
            });

            $('#IDActividadQS').change(function () {
                calcularTarifa();
            });

            $('#TipoCliente').change(function () {
                calcularTarifa();
            });

            $('#TienePersonasAutorizadasRetMat').change(function () {
                mostrarPanelPersonaAutorizada();
            });

            $('#TieneSocios').change(function () {
                mostrarSocios();
            });

            $("input[id^='arrSinNumeroDirEnv']").change(function () {
                mostrarNumeroDirEnv($(this));
            });

            $("input[id^='arrCPDirEnv']").change(function () {

                cargarDatosCPDirEnv($(this));
            });

            $('#TieneDireccionesDeEnvio').change(function () {
                mostrarDireccionesDeEnvio();
            });

            $('#NoAdmiteFacturacionElectronica').click(function () {
                noAdmiteFacturacionElectronica();
            });

            $('#TieneApartadoPostalFacturacion').change(function () {
                mostrarDirEnvioFacturas();
            });

            $('#SinNumeroFacturacion').change(function(){
                mostrarNumeroFacturacion();
            });

            $('#CPFacturacion').change(function () {
                cargarDatosCPFacturacion();
            });

            $("#FormaDePago").change(function () {
                $("#DtoPP").val(0);
                $("#RecargoFinanciero").val(0);
                mostrarDatosFormaDePago();
            });

            $('#NoTieneVtosFijos').click(function () {
                mostrarVtosFijos();
            });

            $('#PedidoEnFirme').change(function () {

                IBANObligatorioSN();
            });

            $('#addPersona').click(function () {
                añadirPersona();
            });

            $('#addSocio').click(function () {
                añadirSocio();
            });

            $('#addDirEnv').click(function () {
                añadirDirEnvio();
            });

            $('#addClienteHab').click(function () {
                añadirClienteHab();
            });

            $('#addProveedorHab').click(function () {
                añadirProveedorHab();
            });

            $('#TieneEmpresasVinculadas').change(function () {
                mostrarEmpresasVinculadas();
            });

            $('#addEmpresaVinculada').click(function () {
                añadirEmpresaVinculada();
            });

            $('#addBanco').click(function () {
                añadirBanco();
            });

            $('#CAEFirmada').change(function () {
                mostrarCAE();
            });

            $('#PesaElMaterial').change(function () {
                mostrarInstrumentosDePesaje();
            });

            $('#CobroDePortesPorEnvio').change(function () {
                mostrarImportePortesPorEnvio();
            });

        });
    </script>
    </form>
</asp:Content>

