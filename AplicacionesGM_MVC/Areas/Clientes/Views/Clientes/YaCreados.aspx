﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Clientes/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<AplicacionesGM_MVC.Models.aspnet_Clientes>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Clientes Aperturados
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: ViewData["Title"]%> </h2>
            <br />
            <%  if (Model.Any())
                {%>
            <!--<div id="iconos">
                <a href="<%: @Url.Action("ExportAccionesListToExcel","Acciones", new{tab=ViewData["Title"]}) %>"><img class="imgLink" title="Exportar a Excel" alt="ExportarAExcel" longdesc="Exportar a Excel" src="../../Content/images/iconos/excel.png"/></a>
            </div>-->
            <% } %>
            <div class="chromestyle" id="chromemenu2">
                <ul>
                    <li><%: Html.ActionLink("Pendientes", "Index", "Clientes")%></li>
                    <li><%: Html.ActionLink("Aperturados(" + @Model.Count() + ")", "YaCreados", "Clientes")%></li>
                </ul>
            </div>
       <%  if (!Model.Any())
           {%>
           <h2><%: ViewData["NoDataFound"]%> </h2>
        <% }
           else
           { %>
        
        <table>
            <% if (User.IsInRole("Comercial"))
               {
                   //Comerciales %> 
                <tr>
                    <th>
                        Cod. Cliente
                    </th>
                    <th>
                        F. Volcado a QS
                    </th>
                    <th>
                        Empresas
                    </th>
                    <th>
                        C.I.F / N.I.F
                    </th>
                    <th>
                        Nombre
                    </th>
                    <th>
                        Domicio
                    </th>
                    <th>
                        Municipio
                    </th>
                    <th>
                        Provincia
                    </th>
                    <th>
                        CP
                    </th>
                    <th>
                        Pais
                    </th>
                    <th>
                        Zona
                    </th>
                    <th>
                        Mail / Dirección de facturación
                    </th>
                    <th>
                        Actividad
                    </th>
                    <th>
                        ¿Es de Exposición?
                    </th>
                    <th></th>
                </tr>
            <% //Fin Comerciales
               }
               else if (User.IsInRole("Créditos"))
               { // Responsable de Créditos %>
                <tr>
                    <th>
                        Nº Cliente
                    </th>
                    <th>
                        Empresas
                    </th>
                    <th>
                        C.I.F / N.I.F
                    </th>
                    <th>
                        Nombre
                    </th>
                    <th>
                        Forma de pago
                    </th>
                    <th>
                        Limite propuesto
                    </th>
                    <th>
                        Consumo potencial
                    </th>
                    <th>
                        Límite asignado por empresa
                    </th>
                    <th></th>
                </tr>
               <% //Fin responsable de créditos
                }
               else if (User.IsInRole("Clientes") || User.IsInRole("Administrador") || User.IsInRole("Gestor"))
               {
                   //Responsable de clientes (Loli)  %>
                <tr>
                    <th>
                        Cod. Cliente
                    </th>
                    <th>
                        F. Volcado a QS
                    </th>
                    <th>
                        Usuario de creación
                    </th>
                    <th>
                        Delegación
                    </th>
                    <th>
                        Empresas
                    </th>
                    <th>
                        C.I.F / N.I.F
                    </th>
                    <th>
                        Nombre
                    </th>
                    <th>
                        Forma de pago
                    </th>
                    <th>
                        Forma de pago para autorizar
                    </th>
                    <th>
                        Recargo financiero
                    </th>
                    <th>
                        Dto. P.P.
                    </th>
                    <th>
                        Mail / Dirección de facturación
                    </th>
                    <th></th>
                </tr>
            <% //Fin responsable de clientes (Loli)
               }
               else if (User.IsInRole("Logística"))
               {
                //Responsable de logística  %>
                <tr>
                    <th>
                        Nº Cliente
                    </th>
                    <th>
                        Provincia
                    </th>
                    <th>
                        Empresas
                    </th>
                    <th>
                        C.I.F / N.I.F
                    </th>
                    <th>
                        Nombre
                    </th>
                    <th>
                        Horario
                    </th>
                    <th>
                        M. descarga
                    </th>
                    <th>
                        Pluma
                    </th>
                    <th>
                        Tipo vehículo
                    </th>
                    <th>
                        Pesa Mat.
                    </th>
                    <th>
                        Espacio Alm.
                    </th>
                    <th>
                        Coste Portes
                    </th>
                    <th></th>
                </tr>
            <%} //Fin responsable de logística
              else if (User.IsInRole("Prevención"))
               {
                //Responsabel de prevención%>
                <tr>
                    <th>
                        Revisado
                    </th>
                    <th>
                        Fecha Original
                    </th>
                    <th>
                        Nº Cliente
                    </th>
                    <th>
                        Empresas
                    </th>
                    <th>
                        C.I.F / N.I.F
                    </th>
                    <th>
                        Nombre
                    </th>
                    <th>
                        CAE Firmada
                    </th>
                    <th>
                        Fichero CAE
                    </th>
                    <th>
                        Causa de no firma
                    </th>
                    <th></th>
                </tr>
            <% //Fin responsable de prevención
              }%>
        <% foreach (var item in Model)
           { %>
            <% if (User.IsInRole("Comercial"))
               { //Comerciales %> 
                <tr>
                    <td>
                        <%: item.QSID%>
                    </td>
                    <td>
                        <%: String.Format("{0:d}", item.FechaVolcadoQS)%>
                    </td>
                    <td>
                        <% 
                   Dictionary<string, string> empresas = (Dictionary<string, string>)ViewData["Empresas"];
                   string strEmpresas = "";
                   foreach (string key in empresas.Keys)
                   {
                       if (item.Empresas.Contains(key))
                       {
                           if (strEmpresas.Length == 0)
                           {
                               strEmpresas += empresas[key].ToString();
                           }
                           else
                           {
                               strEmpresas += ", " + empresas[key].ToString();
                           }
                       }
                   } %>
                        <%: strEmpresas%>
                    </td>
                    <td>
                        <%: item.NIF%>
                    </td>
                    <td>
                        <%: item.Nombre%>
                    </td>
                    <td>
                        <%: item.TipoDeVia + " " + item.Domicilio + " " + String.Format("{0:0.##}", item.Numero)%>
                    </td>
                    <td>
                        <%: item.Municipio%>
                    </td>
                    <td>
                        <% SelectList provincias = (SelectList)ViewData["Provincias"]; %>
                        <%:(item.IDProvinciaQS == null) ? "" : provincias.Where(p => p.Value == item.IDProvinciaQS.ToString()).First().Text%>
                    </td>
                    <td>
                        <%: String.Format("{0:0.##}", item.CP)%>
                    </td>
                    <td>
                        <% SelectList paises = (SelectList)ViewData["Paises"]; %>
                        <%: (item.IDPaisQS == null) ? "" : paises.Where(p => p.Value == item.IDPaisQS.ToString()).First().Text%>
                    </td>
                    <td>
                        <% SelectList zonas = (SelectList)ViewData["Zonas"]; %>
                        <%: (item.Zona == null) ? "" : zonas.Where(p => p.Value == item.Zona.ToString()).First().Text%>
                    </td>
                    <td>
                        <% if (item.NoAdmiteFacturacionElectronica == true && item.EsDeExposicion == false )
                           {%>
                            <%: item.TipoDeViaFacturacion + " " + item.DomicilioFacturacion + " Nº " + string.Format("{0:0.##}", item.NumeroFacturacion) + " " + item.PisoFacturacion + " " + item.CPFacturacion + "(" + item.MunicipioFacturacion + ") " + ((item.IDProvinciaQSFacturacion == null) ? "" : provincias.Where(p => p.Value == item.IDProvinciaQS.ToString()).First().Text) + " " + ((item.IDPaisQS == null) ? "" : paises.Where(p => p.Value == item.IDPaisQS.ToString()).First().Text) %>
                        <%}
                           else
                           { %>
                            <%: item.MailDeFacturacion%>
                        <% }%>
                    </td>
                    <td>
                        <%: item.IDActividadQS%>
                    </td>
                    <td>
                        <% if (item.EsDeExposicion == true)
                           {%>
                            Sí
                        <%}
                           else
                           { %>
                            No
                        <% }%>                  
                    </td>
                    <td>
                        <%: Html.ActionLink("Consultar", "Edit", new { id = item.ID })%>
                    </td>
                </tr>
            <% } //Fin Comerciales
               else if (User.IsInRole("Créditos"))
               { //Responsable de Créditos

                   Dictionary<string, string> empresas = (Dictionary<string, string>)ViewData["Empresas"];
               %>
               <tr>
                    <td>
                        <%: item.QSID.ToString()%>
                    </td>
                    <td>
                        <% 
                   string strEmpresas = "";
                   foreach (string key in empresas.Keys)
                   {
                       if (item.Empresas.Contains(key))
                       {
                           if (strEmpresas.Length == 0)
                           {
                               strEmpresas += empresas[key].ToString();
                           }
                           else
                           {
                               strEmpresas += ", " + empresas[key].ToString();
                           }
                       }
                   } %>
                        <%: strEmpresas%>
                    </td>
                    <td>
                        <%: item.NIF%>
                    </td>
                    <td>
                        <%: item.Nombre%>
                    </td>
                    <td>
                        <% SelectList formasDePago = (SelectList)ViewData["FormasDePago"]; %>
                        <%: (item.FormaDePago == null) ? "" : formasDePago.Where(p => p.Value == item.FormaDePago.ToString()).First().Text%>
                    </td>
                    <td>
                        <%: item.LimitePropuesto%>
                    </td>
                    <td>
                        <%: item.ConsumoPotencial%>
                    </td>
                    <td>
                        <table class="noborder">
                        <%foreach (string key in empresas.Keys)
                          {
                              if (item.Empresas.Contains(key))
                              {
                                  string strLimite = "";
                                  if (key == "003" || key == "033")
                                  {
                                      strLimite = item.LimiteAsignadoMV.ToString();
                                  }
                                  else if (key == "004" || key == "044")
                                  {
                                      strLimite = item.LimiteAsignadoHMA.ToString();
                                  }
                                  else if (key == "006")
                                  {
                                      strLimite = item.LimiteAsignadoECA.ToString();
                                  }  %>
                                <tr class="noborder">
                                    <td class="noborder"><label><%: empresas[key].ToString()%></label></td>
                                    <td class="noborder"><%: strLimite%></td>
                                </tr>
                           <% }
                          }                            
                        %>
                        </table>
                    </td>
                    <td>
                        <%: Html.ActionLink("Consultar", "Edit", new { id = item.ID })%>
                    </td>
                </tr>
               <% //Fin Responsable de Créditos
               }
               else if (User.IsInRole("Clientes") || User.IsInRole("Administrador") || User.IsInRole("Gestor"))
               {
                   //Responsable de clientes Loli %>
                 <tr>
                    <td>
                        <%: item.QSID%>
                    </td>
                    <td>
                        <%: String.Format("{0:d}", item.FechaVolcadoQS)%>
                    </td>
                    <td>
                        <%:item.aspnet_Users.UserName.ToString() %>
                    </td>
                    <td>
                        <%:item.aspnet_Delegaciones.Descripcion %>
                    </td>
                    <td>
                        <% 
                   Dictionary<string, string> empresas = (Dictionary<string, string>)ViewData["Empresas"];
                   string strEmpresas = "";
                   foreach (string key in empresas.Keys)
                   {
                       if (item.Empresas.Contains(key))
                       {
                           if (strEmpresas.Length == 0)
                           {
                               strEmpresas += empresas[key].ToString();
                           }
                           else
                           {
                               strEmpresas += ", " + empresas[key].ToString();
                           }
                       }
                   } %>
                        <%: strEmpresas%>
                    </td>
                    <td>
                        <%: item.NIF%>
                    </td>
                    <td>
                        <%: item.Nombre%>
                    </td>
                    <td>
                        <% SelectList formasDePago = (SelectList)ViewData["FormasDePago"]; %>
                        <%: (item.FormaDePago == null) ? "" : formasDePago.Where(p => p.Value == item.FormaDePago.ToString()).First().Text%>
                    </td>
                    <td>
                        <%: item.FormaDePagoSolicitada%>
                    </td>
                    <td>
                        <%: item.RecargoFinanciero%>
                    </td>
                    <td>
                        <%: item.DtoPP%>
                    </td>
                    <td>
                        <% if (item.NoAdmiteFacturacionElectronica == true && item.EsDeExposicion == false )
                           {%>
                            <% SelectList provincias = (SelectList)ViewData["Provincias"]; 
                               SelectList paises = (SelectList)ViewData["Paises"]; %>
                            <%: item.TipoDeViaFacturacion + " " + item.DomicilioFacturacion + " Nº " + string.Format("{0:0.##}", item.NumeroFacturacion) + " " + item.PisoFacturacion + " " + item.CPFacturacion + "(" + item.MunicipioFacturacion + ") " + ((item.IDProvinciaQSFacturacion == null) ? "" : provincias.Where(p => p.Value == item.IDProvinciaQS.ToString()).First().Text) + " " + ((item.IDPaisQS == null) ? "" : paises.Where(p => p.Value == item.IDPaisQS.ToString()).First().Text) %>
                        <%}
                           else
                           { %>
                            <%: item.MailDeFacturacion%>
                        <% }%>
                    </td>
                    <td>
                        <%: Html.ActionLink("Consultar", "Edit", new { id = item.ID })%>
                    </td>
                </tr>
            <% //Fin responsable de clientes (Loli)
               }
               else if (User.IsInRole("Logística"))
               {
                   //Responsable de logística %>
                 <tr>
                    <td>
                        <%: item.QSID%>
                    </td>
                    <td>
                        <% SelectList provincias = (SelectList)ViewData["Provincias"]; %>
                        <%:(item.IDProvinciaQS == null) ? "" : provincias.Where(p => p.Value == item.IDProvinciaQS.ToString()).First().Text%>
                    </td>
                    <td>
                        <% 
                           Dictionary<string, string> empresas = (Dictionary<string, string>)ViewData["Empresas"];
                           string strEmpresas = "";
                           foreach (string key in empresas.Keys)
                           {
                               if (item.Empresas.Contains(key))
                               {
                                   if (strEmpresas.Length == 0)
                                   {
                                       strEmpresas += empresas[key].ToString();
                                   }
                                   else
                                   {
                                       strEmpresas += ", " + empresas[key].ToString();
                                   }
                               }
                           } %>
                        <%: strEmpresas%>
                    </td>
                    <td>
                        <%: item.NIF%>
                    </td>
                    <td>
                        <%: item.Nombre%>
                    </td>
                    <td>
                        <%: item.Horario%>
                    </td>
                    <td>
                        <%: item.aspnet_MediosDeDescarga.Nombre%>
                    </td>
                    <td>
                        <% if (item.NecesitaCamionConPluma)
                           {%>
                                Sí
                        <% }
                           else
                           { %>
                                No
                        <%} %>
                    </td>
                    <td>
                        <%: item.aspnet_TiposDeVehiculo.Nombre%>
                    </td>
                    <td>
                        <% if (item.PesaElMaterial)
                           {%>
                                Sí
                        <% }
                           else
                           { %>
                                No
                        <%} %>
                    </td>
                    <td>
                        <% if (item.EspacioParaAlmacenar)
                           {%>
                                Sí
                        <% }
                           else
                           { %>
                                No
                        <%} %>
                    </td>
                    <td>
                        <%: item.CobroDePortesPorEnvio%>
                    </td>
                    <td>
                        <%: Html.ActionLink("Consultar", "Edit", new { id = item.ID })%>
                    </td>
                </tr>
            <% //Fin responsable de logística
               } 
               else if (User.IsInRole("Prevención"))
               { 
                //Responsable de prevención
               %>
               <tr>
                    <td>
                        <% if (item.CAERevisada)
                           {%>
                                Sí
                        <% }
                           else
                           { %>
                                No
                        <%} %>
                    </td>
                    <td>
                        <%: item.FechaDeOriginalCAE %>
                    </td>
                    <td>
                        <%: item.QSID.ToString() %>
                    </td>
                    <td>
                        <% 
                   Dictionary<string, string> empresas = (Dictionary<string, string>)ViewData["Empresas"];
                   string strEmpresas = "";
                   foreach (string key in empresas.Keys)
                   {
                       if (item.Empresas.Contains(key))
                       {
                           if (strEmpresas.Length == 0)
                           {
                               strEmpresas += empresas[key].ToString();
                           }
                           else
                           {
                               strEmpresas += ", " + empresas[key].ToString();
                           }
                       }
                   } %>
                        <%: strEmpresas%>
                    </td>
                    <td>
                        <%: item.NIF%>
                    </td>
                    <td>
                        <%: item.Nombre%>
                    </td>
                    <td>
                        <% if (item.CAEFirmada)
                           {%>
                                Sí
                        <% }
                           else
                           { %>
                                No
                        <%} %>
                    </td>
                    <td>
                        <% if (item.CAEFirmada)
                           { %>
                            <a href="<%: item.FicheroCAE %>" target="_blank">Ver documento</a>
                        <% } %>
                    </td>
                    <td>
                        <% if (!item.CAEFirmada)
                           {%>
                            <% SelectList causasNoFirma = (SelectList)ViewData["CausasDeNoFirma"]; %>
                        <%:(item.IDCausaNoFirmaCAE == null) ? "" : causasNoFirma.Where(c => c.Value == item.IDCausaNoFirmaCAE.ToString()).First().Text%>
                        <%} %>
                    </td>
                    <td>
                        <%: Html.ActionLink("Consultar", "Edit", new { id = item.ID })%>
                    </td>
                </tr>
            <% //Fin responsable de prevención
                }
           } %> 
        </table>
    <% } %>    
    
</asp:Content>

