<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AplicacionesGM_MVC.Models.aspnet_Clientes>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Crear Nuevo Cliente
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form id="form1" runat="server">
    
    <h2>Nuevo Cliente</h2>

    <% using (Html.BeginForm("Create", "Clientes", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
        <%: Html.ValidationSummary(true, "Error al guardar el cliente. Revise los errores y vuelva a intentarlo")%>
            <div id="lblBorrador">Borrador</div>
            <div class="chromestyle" id="chromemenu">
                <ul>
                    <li><%: Html.ActionLink("Volver", "Index")%></li>
                    <li><a id="borrador">Guardar Borrador</a></li>
                    <li><a id="submit">Guardar</a></li>
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
                            <%: Html.DropDownListFor(model => model.DelegacionID, (SelectList)ViewData["Delegaciones"], "--Seleccione un valor--", new { @class = "textbox20" })%>
                            <%: Html.ValidationMessageFor(model => model.DelegacionID, true)%>
                        </td>
                    </tr>
                    <tr id="panelExposicion">
                        <td class="editor-label required noborder td20">
                            ¿Es un cliente de exposición?
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
                    <tr>
                        <td class="editor-label required noborder td20">
                            C.I.F / N.I.F
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.NIF, new { @class = "textbox10" })%>
                            <%: Html.ValidationMessageFor(model => model.NIF, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                           <%: Html.LabelFor(model => model.Nombre)%> 
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.Nombre, new { @class = "textbox100" })%>
                            <%: Html.ValidationMessageFor(model => model.Nombre, true)%>
                        </td>
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
                            <label for="Numero" class="display-label required">Nº</label>  <%: Html.TextBoxFor(model => model.Numero, new { @class = "textbox5" })%>
                            <%:Html.ValidationMessageFor(model => model.Numero, true)%>
                            <label for="Numero" class="display-label">Piso</label> <%: Html.TextBoxFor(model => model.Piso, new { @class = "textbox15" })%>
                            <%:Html.ValidationMessageFor(model => model.Piso, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="editor-label required noborder td20">
                            <label for="CP" class="display-label">CP</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.CP, new { @class = "textbox10" })%>
                            <%:Html.ValidationMessageFor(model => model.CP, true)%>
                            <label for="Municipio" class="display-label required">Municipio</label>
                            <%: Html.DropDownListFor(model => model.Municipio, (SelectList)ViewData["Municipios"], "--Seleccione un valor--", null)%>
                            <%:Html.ValidationMessageFor(model => model.Municipio, true)%>
                            <%: Html.Hidden("IDMunicipio")%> <!-- se usa para almacenar el código del municipio y utilizarlo en el cálculo de la ruta en caso de que se modifique el código postal !-->
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
                            <%: Html.LabelFor(model => model.Zona)%>
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
                    <tr id="panelPersonaAutorizada">
                        <td class="editor-label noborder td20">
                            Persona autorizada retirada material
                        </td>
                        <td class="editor-field noborder td80">
                            N.I.F
                            <%: Html.TextBoxFor(model => model.NIFPersonalAutorizadoRetiradaMaterial, new { @class = "textbox10" })%>
                            <%: Html.ValidationMessageFor(model => model.NIFPersonalAutorizadoRetiradaMaterial, true)%>
                            Nombre
                            <%: Html.TextBoxFor(model => model.NombrePersonalAutorizadoRetiradaMaterial, new { @class = "textbox76" })%>
                            <%: Html.ValidationMessageFor(model => model.NombrePersonalAutorizadoRetiradaMaterial, true)%>
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
                                            C.I.F <%: Html.TextBox("arrCIFSocio1", "", new { @class = "textbox10" })%>
                                            <label class="editor-label required">Nombre </label> <%: Html.TextBox("arrNombreSocio1", "", new { @class = "textbox56" })%>
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
                                                     <label class="editor-label required">Nombre </label> <%: Html.TextBox("arrNombreSocio1", "arrNombreSocio" + i, socio.Nombre, new { @class = "textbox56" })%>
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
                                                    <label class="display-label required">C.I.F</label> <%: Html.TextBox("arrCIFVinc1", "", new { @class = "textbox10" })%>
                                                    <label class="display-label required">Nombre</label> <%: Html.TextBox("arrEmpVinc1", "", new { @class = "textbox70" })%>
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
                                                        <label class="display-label required">C.I.F</label> <%: Html.TextBox("arrCIFVinc1", "arrCIFVinc" + i, sociedad.CIF, new { @class = "textbox10" })%>
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
                    <tr id="fichaLogística">
                        <td class="editor-label noborder td20">
                            Ficha logística
                        </td>
                        <td class="editor-field noborder td80">
                            ¿Rellenar ficha logística?
                            <%: Html.DropDownListFor(model => model.TieneFichaLogistica, new SelectList(
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
                                        <label class="editor-label required ">¿Necesita camión con pluma?</label>
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
                                        <%:Html.TextBoxFor(model => model.CobroDePortesPorEnvio, new { @class = "textbox100" })%>
                                        <%:Html.ValidationMessageFor(model => model.CobroDePortesPorEnvio, true)%>
                                    </td>
                                </tr>
                                <tr>
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
                                <tr>
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
                        <td class="editor-label required noborder td20">
                            Año de antigüedad
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.Antguedad, new { @class = "textbox10" })%>
                            <%: Html.ValidationMessageFor(model => model.Antguedad, true)%>
                            <label for="ConsumoPotencial" class="display-label required">Consumo Potencial</label>
                            <%: Html.TextBoxFor(model => model.ConsumoPotencial, String.Format("{0:F}", Model.ConsumoPotencial))%>
                            <%: Html.ValidationMessageFor(model => model.ConsumoPotencial, true)%>
                            <label for="PrevisionAnual" class="display-label required">Previsión Anual</label>
                            <%: Html.TextBoxFor(model => model.PrevisionAnual, String.Format("{0:F}", Model.PrevisionAnual))%>
                            <%: Html.ValidationMessageFor(model => model.PrevisionAnual, true)%>
                        </td>
                    </tr>
                    <tr>
                        <td class="noborder td20">
                            <label>Cia. aseguradora de ventas</label>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.CompañiaSeguroVentas, new { @class = "textbox70" })%>
                            <%: Html.ValidationMessageFor(model => model.CompañiaSeguroVentas, true)%>
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
                            <%: Html.CheckBoxFor(model => model.NoAdmiteFacturacionElectronica)%> El cliente no admite facturación electrónica                            
                        </td>
                    </tr>
                    <tr id="DirEnvioFacturas">
                        <td class="editor-label noborder required td20">
                            Dir. postal envío facturas
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.DirEnvioFactura, new { @class = "textbox100" })%>
                            <%: Html.ValidationMessageFor(model => model.DirEnvioFactura, true)%>
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
                            <%: Html.LabelFor(model => model.IBAN)%>
                        </td>
                        <td class="editor-field noborder td80">
                            <%: Html.TextBoxFor(model => model.IBAN, new { @class = "textbox40" })%>
                            <%: Html.ValidationMessageFor(model => model.IBAN, true)%>
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
                            <%: Html.TextBoxFor(model => model.LimitePropuesto, new { @class = "textbox10" })%>
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
                $(this).closest('form').submit();
            });
            $('#borrador').click(function () {
                $('#EsBorrador').val(true);
                $("#DtoPP").removeAttr("disabled");
                $("#RecargoFinanciero").removeAttr("disabled");
                $(this).closest('form').submit();
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
            mostrarPanelMails();
            mostrarEmpresasVinculadas();
            mostrarSocios();
            mostrarFichaLogistica();
            mostrarFichaExposicion();
            noAdmiteFacturacionElectronica();
            mostrarVtosFijos();
            mostrarInstrumentosDePesaje();

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

            $('#TieneSocios').change(function () {
                mostrarSocios();
            });

            $('#NoAdmiteFacturacionElectronica').click(function () {
                noAdmiteFacturacionElectronica();
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

            $('#addSocio').click(function () {
                añadirSocio();
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

            $('#TieneFichaLogistica').change(function () {
                mostrarFichaLogistica();
            });

            $('#CAEFirmada').change(function () {
                mostrarCAE();
            });

            $('#PesaElMaterial').change(function () {
                mostrarInstrumentosDePesaje();
            });


        });
    </script>
    </form>
    </asp:Content>

