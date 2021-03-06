﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Security;
using System.Web.Profile;
using AplicacionesGM_MVC.Models;
using AplicacionesGM_MVC.Areas.Clientes.Models;
using System.Data;
using System.Security;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class ClientesController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        BDPlanificacionTransportesEntities dbPlanificacion = new BDPlanificacionTransportesEntities();

        //
        // GET: /ClientesPdtesAprobar/
        
        public ActionResult Index(string sortOrder,string sortType, FormCollection formData)
        {
            ViewData["NoDataFound"] = "No tiene clientes pendientes de aprobar.";
            ViewData["Title"] = "Clientes Pendientes";

            //Cargo los clientes
            IEnumerable<aspnet_Clientes> Clientes = getClientes(sortOrder, sortType, ViewData["Title"].ToString(), formData);
            cargarDatosVistaListaClientes();
            return View(Clientes);
        }
        
        [HttpPost]
        public ActionResult Index(FormCollection formData)
        {
            ViewData["NoDataFound"] = "No tiene clientes pendientes de aprobar.";
            ViewData["Title"] = "Clientes Pendientes";
            IEnumerable<aspnet_Clientes> Clientes;
            ClientesModel objCliModel= new ClientesModel();
            try
            {
                
                    string[] arrClientesValidados;
                    string strClientesValidados = formData["arrGuardar"].ToString().Replace("false,", "").Replace(",false", "").Replace("false", "");
                    
                    if (strClientesValidados!=""){
                        arrClientesValidados = strClientesValidados.Split(',');
                        foreach (string strIDCliente in arrClientesValidados)
                        {
                            decimal IDCliente = Convert.ToDecimal(strIDCliente.Trim());
                            aspnet_Clientes objCliente = db.aspnet_Clientes.FirstOrDefault(item => item.ID == IDCliente);
                            if (User.IsInRole("Clientes") || User.IsInRole("Administrador"))
                            {//Responsable de clientes (Loli)
                             #region RESPONSABLE DE CLIENTES
                                volcarClienteABBDD(objCliente);
                             #endregion
                            }
                            else if (User.IsInRole("Créditos"))
                            {//Responsable de créditos
                             #region RESPONSABLE CRÉDITOS
                                //Validamos que tengamos los datos necesarios del cliente
                                
                                /*
                                Finalmente se ha decidido no utilizar esta opción
                                bool blnError = false;
                                string strMessageError = "";
                                 
                                foreach (string strEmpresa in objCliente.Empresas.Split(','))
                                {
                                    if ((strEmpresa == "003" && formData["limite_" + objCliente.QSID.ToString() + "_003"] == "") || (strEmpresa == "004" && formData["limite_" + objCliente.QSID.ToString() + "_004"] == "") || (strEmpresa == "033" && formData["limite_" + objCliente.QSID.ToString() + "_033"] == "") || (strEmpresa == "044" && formData["limite_" + objCliente.QSID.ToString() + "_044"] == "") || (strEmpresa == "006" && formData["limite_" + objCliente.QSID.ToString() + "_006"] == ""))
                                    {
                                        blnError = true;
                                        strMessageError = "No se ha podido grabar el cliente " + objCliente.Nombre + " ya que falta por asignar límite para alguna de sus empresas.";
                                        break;
                                    }
                                    else
                                    {
                                        //Asignamos el valor al campo correspondiente del cliente en función de la empresa.
                                        if (strEmpresa == "003" || strEmpresa=="033")
                                        {
                                            objCliente.LimiteAsignadoMV = Convert.ToInt32(formData["limite_" + objCliente.QSID.ToString() + "_" + strEmpresa].ToString());
                                        }
                                        else if (strEmpresa == "004" || strEmpresa == "044")
                                        {
                                            objCliente.LimiteAsignadoHMA = Convert.ToInt32(formData["limite_" + objCliente.QSID.ToString() + "_" + strEmpresa].ToString());
                                        }
                                        else if (strEmpresa == "006")
                                        {
                                            objCliente.LimiteAsignadoECA = Convert.ToInt32(formData["limite_" + objCliente.QSID.ToString() + "_" + strEmpresa].ToString());
                                        }

                                        //Volcamos la información a la empresa correspondiente de QS
                                        if (!objCliModel.setRiesgoClienteEnEmpresaQS(strEmpresa, objCliente.QSID, Convert.ToInt32(formData["limite_" + objCliente.QSID.ToString() + "_" + strEmpresa].ToString())))
                                        {
                                            blnError = true;
                                            strMessageError="No se ha podido grabar el cliente " + objCliente.Nombre + " ya que no se ha podido grabar el riesgo en alguno de las empresas.";
                                            break;
                                        }
                                    }
                                }
                                if (!blnError)
                                {
                                    if (objCliModel.setRiesgoClienteEnBDClientes(objCliente))
                                    {
                                        objCliente.UsuarioValidacionCreditos = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                                        objCliente.FechaValidacionCreditos = System.DateTime.Now;
                                    }
                                    else
                                    {
                                        blnError = true;
                                    }
                                }

                                if (blnError)
                                {
                                    //Si ha habido un error nos aseguramos de que los campos queden a null para que el cliente continue apareciendo en pendientes
                                    objCliente.LimiteAsignadoMV = null;
                                    objCliente.LimiteAsignadoHMA = null;
                                    objCliente.LimiteAsignadoECA = null;

                                    ModelState.AddModelError("", strMessageError);
                                }*/
                                
                                //Actualizamos el cliente
                                objCliente.UsuarioValidacionCreditos = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                                objCliente.FechaValidacionCreditos = System.DateTime.Now;
                                db.ObjectStateManager.ChangeObjectState(objCliente, EntityState.Modified);
                                db.SaveChanges();                                
                             #endregion
                            }
                            else if (User.IsInRole("Logística"))
                            {
                             #region RESPONSABLE LOGÍSTICA
                                if (formData["Delegacion_" + objCliente.QSID.ToString()].ToString() != "")
                                {
                                    if (objCliModel.crearExcepcionesClienteEnBDPlanificacion(objCliente))
                                    {
                                        objCliente.FechaValidacionLogistica = System.DateTime.Now;
                                        objCliente.UsuarioValidacionLogistica = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                                        db.ObjectStateManager.ChangeObjectState(objCliente, EntityState.Modified);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "No se ha podido volcar la información del cliente " + objCliente.Nombre + " al programa de Planificación de Transporte. Por favor, vuelva a intentarlo.");
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Debe de indicar la delegación a la que quiere asociar el cliente " + objCliente.Nombre);
                                }
                             #endregion
                            }
                            else if (User.IsInRole("Prevención"))
                            {
                             #region RESPONSABLE PREVENCIÓN
                                if (objCliente.CAEFirmada)
                                {
                                    if (formData["fechaOri_" + objCliente.QSID.ToString()].ToString() != "")
                                    {
                                        objCliente.CAERevisada = true;
                                        objCliente.FechaDeOriginalCAE = Convert.ToDateTime(formData["fechaOri_" + objCliente.QSID.ToString()]);
                                        objCliente.FechaValidacionPrevencion = System.DateTime.Now;
                                        objCliente.UsuarioValidacionPrevencion = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                                        db.ObjectStateManager.ChangeObjectState(objCliente, EntityState.Modified);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "El cliente " + objCliente.Nombre + " no se ha grabado ya que no se ha indicado una fecha de original.");
                                    }
                                }
                                else
                                {
                                    objCliente.CAERevisada = true;
                                    objCliente.FechaValidacionPrevencion = System.DateTime.Now;
                                    objCliente.UsuarioValidacionPrevencion = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                                    db.ObjectStateManager.ChangeObjectState(objCliente, EntityState.Modified);
                                    db.SaveChanges();
                                }
                             #endregion
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Debe marcar algún cliente como válido para poder volcarlo a las bases de datos");
                    }
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);                
            }
            finally
            {
                //Cargo los clientes
                 Clientes= getClientes("", "asc", ViewData["Title"].ToString(), formData);
                cargarDatosVistaListaClientes();
            }

            return View(Clientes);
        }
        public ActionResult LimpiarFiltros(string tab)
        {
            //Borramos la session
            Session.Clear();

            if (tab == "Clientes Pendientes")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Asociados");
            }
        }

        //Clientes/YaCreados
        public ActionResult YaCreados(string sortOrder, string sortType, FormCollection formData)
        {
            ViewData["NoDataFound"] = "No tiene Clientes aperturados.";
            ViewData["Title"] = "Clientes Aperturados";


            //Cargo los clientes
            IEnumerable<aspnet_Clientes> Clientes = getClientes(sortOrder, sortType, ViewData["Title"].ToString(), formData);
            cargarDatosVistaListaClientes();
            return View(Clientes);
        }
       
        //
        // GET: /Clientes/Create

        public ActionResult Create()
        {
            aspnet_Clientes newClient = new aspnet_Clientes();

            //Asociamos el agente del usuario actual al nuevo cliente
            string currentUserId = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
            ProfileBase profile = ProfileBase.Create(User.Identity.Name);
            if (profile["IDAgenteQSMV"].ToString() != "")
            {
                newClient.IDAgenteQSMV = Convert.ToDecimal(profile["IDAgenteQSMV"].ToString());
            }
            if (profile["IDAgenteQSHMA"].ToString() != "")
            {
                newClient.IDAgenteQSHMA = Convert.ToDecimal(profile["IDAgenteQSHMA"].ToString());
            }
            if (profile["IDAgenteQSECA"].ToString() != "")
            {
                newClient.IDAgenteQSECA = Convert.ToDecimal(profile["IDAgenteQSECA"].ToString());
            }
            
            cargaDatosCliente(newClient);
            
            return View(newClient);
        }

        //
        // POST: /Clientes/Create

        [HttpPost]
        public ActionResult Create(aspnet_Clientes newClient, FormCollection formData)
        {
            try
            {
                // TODO: Add insert logic here

               
                //Asignamos los valores al abojeto
                asignarDatosFormulario(newClient, formData);
                ValidarDatosFormulario(newClient, formData);

                if (ModelState.IsValid)
                {
                    guardarDatosCliente(newClient, formData);

                    if (newClient.EsDeExposicion)
                    {
                        //Si es un cliente de Exposición lo volcamos directamente a la BD
                        if (volcarClienteABBDD(newClient))
                        {
                            //Si el volcado ha ido bien redireccionamos al Index
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        //Si no es un cliente de Exposición lo dejamos pendiente de validar por el responsable de clientes.
                        return RedirectToAction("Index");
                    }    

                    return RedirectToAction("Index");
                }

                cargaDatosCliente(newClient);
                return View(newClient);
            }
            catch (Exception ex)
            {
                cargaDatosCliente(newClient);
                ModelState.AddModelError("", ex.Message);
                return View(newClient);
            }
        }
        
        //
        // GET: /Clientes/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_Clientes modified = db.aspnet_Clientes.FirstOrDefault(item => item.ID == id);
            cargaDatosCliente(modified);
            
            return View(modified);
        }

        //
        // POST: /Clientes/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Clientes modified, FormCollection formData)
        {
            try
            {
                // TODO: Add update logic here
                asignarDatosFormulario(modified, formData);
                ValidarDatosFormulario(modified,formData);

                if (ModelState.IsValid)
                {
                    guardarDatosCliente(modified, formData);

                    if (modified.EsDeExposicion && !modified.EsBorrador)
                    {
                        //Si es un cliente de Exposición lo volcamos directamente a la BD
                        if (volcarClienteABBDD(modified))
                        {
                            //Si el volcado ha ido bien redireccionamos al Index
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        //Si no es un cliente de Exposición lo dejamos pendiente de validar por el responsable de clientes.
                        return RedirectToAction("Index");
                    }                    
                }

                cargaDatosCliente(modified);
                return View(modified);
            }
            catch(Exception ex)
            {
                cargaDatosCliente(modified);
                ModelState.AddModelError("", ex.Message);
                return View(modified);
            }
        }

        //
        // GET: /Clientes/Delete/5
 
        public ActionResult Delete(int id)
        {
            aspnet_Clientes deleted = db.aspnet_Clientes.FirstOrDefault(item => item.ID == id);
            var empresas = db.aspnet_Empresas.Select(e => new { e.QSID, e.Nombre });
            cargaDatosCliente(deleted);

            return View(deleted);
        }

        //
        // POST: /Acciones/Delete/5

        [HttpPost]
        public ActionResult Delete(aspnet_Clientes deleted)
        {
            try
            {
                // TODO: Add delete logic here
                db.aspnet_Clientes.Attach(deleted);
                db.ObjectStateManager.ChangeObjectState(deleted, System.Data.EntityState.Deleted);
                db.aspnet_Clientes.DeleteObject(deleted);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                cargaDatosCliente(deleted);
                return View(deleted);
            }
        }

        public void ExportAccionesListToExcel(string tab)
        {
           /* var grid = new System.Web.UI.WebControls.GridView();
            string currentUserId = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("Origen");
            dtResult.Columns.Add("Departamento");
            dtResult.Columns.Add("Delegaciones");
            dtResult.Columns.Add("Responsables");
            dtResult.Columns.Add("FechaInicio");
            dtResult.Columns.Add("FechaFinPrev");
            dtResult.Columns.Add("FechaFinReal");
            dtResult.Columns.Add("FechaSeguimiento");
            dtResult.Columns.Add("Accion");
            dtResult.Columns.Add("Objetivo");
            dtResult.Columns.Add("Resultado");

            IEnumerable<aspnet_Clientes> acciones = getClientes("", "", tab, new FormCollection());

            var delegaciones = db.aspnet_Delegaciones.Select(d => new { d.DelegacionID, d.Descripcion });
            Dictionary<int,string> dicDelegaciones = delegaciones.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DelegacionID, d => d.Descripcion);

            IEnumerable<aspnet_Users> usuarios;
            if (User.IsInRole("Administrador"))
            {
                usuarios = from u in db.aspnet_Users select u;
            }
            else
            {
                usuarios = from u in db.aspnet_Users
                           from r in u.aspnet_Roles.DefaultIfEmpty()
                           where r.RoleName != "Administrador" || r.RoleName == null
                           select u;
            }
            
            Dictionary<Guid,string> dicResponsables = usuarios.AsEnumerable().OrderBy(u => u.UserName).ToDictionary(u => u.UserId, u => u.UserName); 
            
            List<aspnet_Clientes> dt = acciones.ToList();

            foreach (var accion in dt)
            {
                DataRow dr = dtResult.NewRow();
                dr["Origen"] = accion.aspnet_Origenes.Descripcion;
                dr["Departamento"] = accion.aspnet_Departamentos.Descripcion;
                
                //Obtengo las descripciones de las delegaciones
                string strDelegaciones = "";
                foreach (string delegacion in accion.Delegaciones.Split(',').ToArray<string>())
                {
                    if (delegacion != "")
                    {
                        if (strDelegaciones.Length == 0)
                        {
                            strDelegaciones += dicDelegaciones[Convert.ToInt32(delegacion)];
                        }
                        else
                        {
                            strDelegaciones += ", " + dicDelegaciones[Convert.ToInt32(delegacion)];
                        }
                    }
                }
                dr["Delegaciones"] = strDelegaciones;
                
                //Obtengo los nombres de los responsables.
                string strResponsables = "";
                foreach (string responsable in accion.Responsables.Split(',').ToArray<string>())
                {
                    if (responsable != "")
                    {
                        if (strResponsables.Length == 0)
                        {
                            strResponsables += dicResponsables[new Guid(responsable)];
                        }
                        else
                        {
                            strResponsables += ", " + dicResponsables[new Guid(responsable)];
                        }
                    }
                }
                dr["Responsables"] = strResponsables;
                dr["FechaInicio"] = String.Format("{0:d}",accion.FechaInicio);
                dr["FechaFinPrev"] = String.Format("{0:d}", accion.FechaFinPrev);
                dr["FechaFinReal"] = String.Format("{0:d}", accion.FechaFinReal);
                dr["FechaSeguimiento"] = String.Format("{0:d}", accion.FechaSeguimiento);
                dr["Accion"] = accion.Accion;
                dr["Objetivo"] = accion.Objetivo;
                dr["Resultado"] = accion.Resultado;
                dtResult.Rows.Add(dr);
            }

            grid.DataSource = dtResult;
                              
            grid.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=Acciones.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());

            Response.End();*/

        }

        private IEnumerable<aspnet_Clientes> getClientes(string sortOrder, string sortType, string tab, FormCollection formData)
        {
            //Gestiona la ordenación a aplicar
            Session["sortOrder"] = string.IsNullOrEmpty(sortOrder) ? "Origen" : sortOrder;
            Session["sortType"] = string.IsNullOrEmpty(sortType) ? "asc" : sortType;

            System.Guid currentUserId = (System.Guid) Membership.GetUser(User.Identity.Name).ProviderUserKey;
            ProfileBase profile = ProfileBase.Create(User.Identity.Name);

            IEnumerable<aspnet_Clientes> clientes = db.aspnet_Clientes;
          

            if (tab == "Clientes Pendientes" )
            {
                if (User.IsInRole("Créditos"))
                {
                    clientes = clientes.Where(c => c.QSID != null && c.UsuarioValidacionCreditos == null && c.FormaDePago!=1);
                }
                else if (User.IsInRole("Logística"))
                {
                    clientes = clientes.Where(c => c.QSID != null && c.UsuarioValidacionLogistica == null);
                }
                else if (User.IsInRole("Prevención"))
                {
                    clientes = clientes.Where(c => c.QSID != null && c.CAERevisada == false);
                }
                else
                {
                    clientes = clientes.Where(c => c.QSID == null);
                }
            }
            else //Clientes Ya Creados
            {
                if (User.IsInRole("Créditos"))
                {
                    clientes = clientes.Where(c => c.QSID != null && c.UsuarioValidacionCreditos != null);
                }
                else if (User.IsInRole("Logística"))
                {
                    clientes = clientes.Where(c => c.QSID != null  && c.UsuarioValidacionLogistica != null);
                }
                else if (User.IsInRole("Prevención"))
                {
                    clientes = clientes.Where(c => c.QSID != null && c.CAERevisada == true);
                }
                else
                {
                    clientes = clientes.Where(c => c.QSID != null);
                }
                clientes = clientes.OrderByDescending(c => c.FechaVolcadoQS);
            }

            if (User.IsInRole("Comercial"))
            {
                clientes = clientes.Where(c => (c.IDAgenteQSMV ?? -1).ToString() == profile["IDAgenteQSMV"].ToString() || (c.IDAgenteQSHMA ?? -1).ToString() == profile["IDAgenteQSHMA"].ToString() || (c.IDAgenteQSECA??-1).ToString()==profile["IDAgenteQSECA"].ToString());
            }
            else
            {
                if (!User.IsInRole("Administrador"))
                {
                    clientes = clientes.Where(c => c.EsBorrador == false || c.UsuarioDeAlta == currentUserId);
                }

                //Solo mostramos los clientes que corresponden a alguna de las delegaciones de que gestiona el usuario
                clientes = clientes.Where(c => profile["Delegaciones"].ToString().Split(',').Contains(c.DelegacionID.ToString()));
            }


            
            //Aplicamos posibles filtros que podemos recibir en el formData
            
            //Actualizamos las variables de sesion
            /*if (formData["fInicioDesde"] != null && formData["fInicioDesde"].ToString()!="")
            {
                Session["fInicioDesde"] = formData["fInicioDesde"].ToString();
            }
            if (formData["fInicioHasta"]!=null && formData["fInicioHasta"].ToString() != "")
            {
                Session["fInicioHasta"] = formData["fInicioHasta"].ToString();
            }
            if (formData["fSeguimientoDesde"]!=null && formData["fSeguimientoDesde"].ToString() != "")
            {
                Session["fSeguimientoDesde"] = formData["fSeguimientoDesde"].ToString();
            }
            if (formData["fSeguimientoHasta"]!=null && formData["fSeguimientoHasta"].ToString() != "")
            {
                Session["fSeguimientoHasta"] = formData["fSeguimientoHasta"].ToString();
            }
            if (formData["fFinDesde"]!=null && formData["fFinDesde"].ToString() != "")
            {
                Session["fFinDesde"] = formData["fFinDesde"].ToString();
            }
            if (formData["fFinHasta"]!=null && formData["fFinHasta"].ToString() != "")
            {
                Session["fFinHasta"] = formData["fFinHasta"].ToString();
            }
            if (formData["fFinRealDesde"]!=null && formData["fFinRealDesde"].ToString() != "")
            {
                Session["fFinRealDesde"] = formData["fFinRealDesde"].ToString();
            }
            if (formData["fFinRealHasta"]!=null && formData["fFinRealHasta"].ToString() != "")
            {
                Session["fFinRealHasta"] = formData["fFinRealHasta"].ToString();
            }
            if (formData["arrDelegaciones"]!=null)
            {
                Session["arrDelegaciones"] = formData["arrDelegaciones"].ToString().Replace("false,", "").Replace(",false", "").Replace("false","");
            }
            if (formData["arrOrigenes"]!=null)
            {
                Session["arrOrigenes"] = formData["arrOrigenes"].ToString().Replace("false,", "").Replace(",false", "").Replace("false", "");
            }
            if (formData["arrDepartamentos"]!=null)
            {
                Session["arrDepartamentos"] = formData["arrDepartamentos"].ToString().Replace("false,", "").Replace(",false", "").Replace("false", "");
            }
            if (formData["arrResponsables"]!=null)
            {
                Session["arrResponsables"] = formData["arrResponsables"].ToString().Replace("false,", "").Replace(",false", "").Replace("false", "");
            }
            if (formData["Accion"] != null && formData["Accion"].ToString() != "")
            {
                Session["Accion"] = formData["Accion"].ToString();
            }
            if (formData["Objetivo"] != null && formData["Objetivo"].ToString() != "")
            {
                Session["Objetivo"] = formData["Objetivo"].ToString();
            }
            if (formData["Resultado"] != null && formData["Resultado"].ToString() != "")
            {
                Session["Resultado"] = formData["Resultado"].ToString();
            }

            //Aplicamos los filtros en base a las variables de sesión
            if ((Session["fInicioDesde"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.FechaInicio >= DateTime.Parse(Session["fInicioDesde"].ToString()));
            }
            if ((Session["fInicioHasta"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.FechaInicio <= DateTime.Parse(Session["fInicioHasta"].ToString()));
            }
            if ((Session["fSeguimientoDesde"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.FechaSeguimiento >= DateTime.Parse(Session["fSeguimientoDesde"].ToString()));
            }
            if ((Session["fSeguimientoHasta"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.FechaSeguimiento <= DateTime.Parse(Session["fSeguimientoHasta"].ToString()));
            }
            if ((Session["fFinDesde"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.FechaFinPrev >= DateTime.Parse(Session["fFinDesde"].ToString()));
            }
            if ((Session["fFinHasta"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.FechaFinPrev <= DateTime.Parse(Session["fFinHasta"].ToString()));
            }
            if ((Session["fFinRealDesde"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.FechaFinReal >= DateTime.Parse(Session["fFinRealDesde"].ToString()));
            }
            if ((Session["fFinRealHasta"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.FechaFinReal <= DateTime.Parse(Session["fFinRealHasta"].ToString()));
            }
            if ((Session["Accion"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => (a.Accion ?? "").Contains(Session["Accion"].ToString()));
            }
            if ((Session["Objetivo"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => (a.Objetivo ?? "").Contains(Session["Objetivo"].ToString()));
            }
            if ((Session["Resultado"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => (a.Resultado ?? "").Contains(Session["Resultado"].ToString()));
            }
            if ((Session["arrDelegaciones"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.Delegaciones.Split(',').Intersect(Session["arrDelegaciones"].ToString().Split(',')).Any());
            }
            if ((Session["arrOrigenes"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => Session["arrOrigenes"].ToString().Split(',').Contains(a.OrigenID.ToString()));
            }
            if ((Session["arrResponsables"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => a.Responsables.Split(',').Intersect(Session["arrResponsables"].ToString().Split(',')).Any());
            }
            if ((Session["arrDepartamentos"] ?? "").ToString() != "")
            {
                acciones = acciones.Where(a => Session["arrDepartamentos"].ToString().Split(',').Contains(a.DepartamentoID.ToString()));
            }

                
            //Aplicamos la ordenación correspondiente
            if (sortOrder == Session["sortOrder"].ToString())
            {
                if (Session["sortType"].ToString() == "asc")
                {
                    Session["sortType"] = "desc";
                }
                else
                {
                    Session["sortType"] = "asc";
                }
            }
            else
            {
                Session["sortType"] = "asc";
            }


            switch (sortOrder)
            {
                case "Departamento":
                    if (Session["sortType"].ToString() == "asc")
                    {
                        acciones = acciones.OrderBy(a => a.aspnet_Departamentos.Descripcion).ThenByDescending(a => a.FechaInicio);
                    }
                    else
                    {
                        acciones = acciones.OrderByDescending(a => a.aspnet_Departamentos.Descripcion).ThenByDescending(a => a.FechaInicio);
                    }
                    break;
                case "FechaInicio":
                    if (Session["sortType"].ToString() == "asc")
                    {
                        acciones = acciones.OrderBy(a => a.FechaInicio);
                    }
                    else
                    {
                        acciones = acciones.OrderByDescending(a => a.FechaInicio);
                    }
                    break;
                case "FechaFinPrev":
                    if (Session["sortType"].ToString() == "asc")
                    {
                        acciones = acciones.OrderBy(a => a.FechaFinPrev).ThenByDescending(a => a.FechaInicio);
                    }
                    else
                    {
                        acciones = acciones.OrderByDescending(a => a.FechaFinPrev).ThenByDescending(a => a.FechaInicio);
                    }
                    break;
                case "FechaSeguimiento":
                    if (Session["sortType"].ToString() == "asc")
                    {
                        acciones = acciones.OrderBy(a => a.FechaSeguimiento).ThenByDescending(a => a.FechaInicio);
                    }
                    else
                    {
                        acciones = acciones.OrderByDescending(a => a.FechaSeguimiento).ThenByDescending(a => a.FechaInicio);
                    }
                    break;
                case "FechaFinReal":
                    if (Session["sortType"].ToString() == "asc")
                    {
                        acciones = acciones.OrderBy(a => a.FechaFinReal).ThenByDescending(a => a.FechaInicio);
                    }
                    else
                    {
                        acciones = acciones.OrderByDescending(a => a.FechaFinReal).ThenByDescending(a => a.FechaInicio);
                    }
                    break;
                default:
                    if (Session["sortType"].ToString() == "asc")
                    {
                        acciones = acciones.OrderBy(a => a.aspnet_Origenes.Descripcion).ThenByDescending(a => a.FechaInicio);
                    }
                    else
                    {
                        acciones = acciones.OrderByDescending(a => a.aspnet_Origenes.Descripcion).ThenByDescending(a => a.FechaInicio);
                    }
                    break;
            }*/

            return clientes;
        }

        private void cargarDatosVistaListaClientes()
        {
            
            var empresas = db.aspnet_Empresas.Select(e => new { e.QSID, e.Nombre });
            ViewData["empresas"] = empresas.AsEnumerable().ToDictionary(e => e.QSID, e => e.Nombre);

            IEnumerable<LocalizacionesModel> objLoc = getLocalizaciones();
            ViewData["Provincias"] = new SelectList(getProvincias(objLoc), "IDProvincia", "Provincia");
            ViewData["Paises"] = new SelectList(getPaises(objLoc), "IDPais", "Pais");
            ViewData["Zonas"] = new SelectList(getZonas(), "IDZona", "Nombre");
            ViewData["FormasDePago"] = getFormasDePago(false);
            //Causas de no firma, datos de incoterms
            Areas.Clientes.Models.DSas400TableAdapters.INSICTableAdapter ti = new Models.DSas400TableAdapters.INSICTableAdapter();
            var datosC = (from ca in ti.GetData() select ca);
            var list = new SelectList(datosC.AsEnumerable(), "ICCDG", "ICDES");
            ViewData["CausasDeNoFirma"] = list;

            if (User.IsInRole("Logística"))
            {
                ViewData["Delegaciones"] = new SelectList(dbPlanificacion.Maestro_Delegaciones.AsEnumerable(), "DELEGACION", "NOMBRE");
            }
        }

        private void cargaDatosCliente(aspnet_Clientes objCliente)
        {
            //Cargo los datos para los Checks
            //Datos de empresas
            var empresas = db.aspnet_Empresas.Select(e => new { e.QSID, e.Nombre, e.Visible }).Where(e=>e.Visible==true);
            CheckBoxModel chkEmpresas = new CheckBoxModel(empresas.AsEnumerable().OrderByDescending(e => e.Nombre).ToDictionary(e => e.QSID.ToString(), e => e.Nombre), new List<string>());
            ViewData["chkEmpresas"] = chkEmpresas;
            Dictionary <string,CheckBoxModel> lstMatXEmpresa = new Dictionary<string,CheckBoxModel> ();

            //Agentes de Moraval
            Areas.Clientes.Models.DSas400TableAdapters.AGTAG003TableAdapter taMV = new Areas.Clientes.Models.DSas400TableAdapters.AGTAG003TableAdapter();
            var agentesMV = (from a in taMV.GetData() select new { a.AGCDG, NOMBRE = a.AGNBR + " (" + a.AGCDG + ")" }).OrderBy(a => a.NOMBRE);
            SelectList list = new SelectList(agentesMV.AsEnumerable(), "AGCDG", "NOMBRE", objCliente.IDAgenteQSMV);
            ViewData["AgentesMV"] = list;


            //Agentes de Hierros
            Areas.Clientes.Models.DSas400TableAdapters.AGTAG004TableAdapter taHMA = new Areas.Clientes.Models.DSas400TableAdapters.AGTAG004TableAdapter();
            var agentesHMA = (from a in taHMA.GetData() select new { a.AGCDG, NOMBRE = a.AGNBR + " (" + a.AGCDG + ")" }).OrderBy(a => a.NOMBRE);
            list = new SelectList(agentesHMA.AsEnumerable(), "AGCDG", "NOMBRE", objCliente.IDAgenteQSHMA);
            ViewData["AgentesHMA"] = list;

            //Agentes de ECA
            Areas.Clientes.Models.DSas400TableAdapters.AGTAG006TableAdapter taECA = new Areas.Clientes.Models.DSas400TableAdapters.AGTAG006TableAdapter();
            var agentesECA = (from a in taECA.GetData() select new { a.AGCDG, NOMBRE = a.AGNBR + " (" + a.AGCDG + ")" }).OrderBy(a => a.NOMBRE);
            list = new SelectList(agentesECA.AsEnumerable(), "AGCDG", "NOMBRE", objCliente.IDAgenteQSECA);
            ViewData["AgentesECA"] = list;

            //Datos de requerimientos de calidad
            var requerimientos = db.aspnet_RequerimientosDeCalidad.Select(r => new { r.ID, r.Nombre });
            CheckBoxModel chkRequerimientosCal = new CheckBoxModel(requerimientos.AsEnumerable().OrderBy(r => r.ID).ToDictionary(r => r.ID.ToString(), r => r.Nombre), new List<string>());
            ViewData["chkRequerimientos"] = chkRequerimientosCal;

            //Datos de materiales
            foreach (var d in chkEmpresas.lstValores){
                //Cogemos los materiales asociados a la empresa
                CheckBoxModel chkMateriales = new CheckBoxModel((from mat in db.aspnet_MaterialesEmpresa where mat.QSIDEmpresa == d.Key select mat).AsEnumerable().ToDictionary(mat => mat.Nombre, mat => mat.Nombre), new List<string>());

                //Los añado al diccionario de empresas
                lstMatXEmpresa.Add(d.Value, chkMateriales);
            }
            ViewData["Materiales"] = lstMatXEmpresa;
            
            //Cargo los Desplegables
            //Cargo las Delegaciones
            ViewData["Delegaciones"]=new SelectList((db.aspnet_Delegaciones.Select(d=>new {d.DelegacionID, d.Descripcion}).AsEnumerable().OrderBy(d=>d.Descripcion)),"DelegacionID","Descripcion");

            //Cargo los tipos de vía
            var tiposDeVia = db.aspnet_TiposDeVia.Select(tc => new { tc.IDTipoVia, tc.Nombre });
            list = new SelectList(tiposDeVia.AsEnumerable().OrderBy(tc => tc.Nombre), "IDTipoVia", "Nombre");
            ViewData["TiposDeVia"] = list;
            ViewData["TiposDeViaDE"] = tiposDeVia.AsEnumerable();//Utilizados para los combos de las direcciones de envío
            
            //Datos de tipos de clientes
            var tiposDeCliente = db.aspnet_TiposDeCliente.Select(tc => new { tc.ID, tc.Nombre, tc.OrdenDeVisualizacion });
            list = new SelectList(tiposDeCliente.AsEnumerable().OrderBy(tc => tc.OrdenDeVisualizacion), "ID", "Nombre");
            ViewData["TiposDeCliente"] = list;

            //Datos de Formas de Pago
            ViewData["FormasDePago"] = getFormasDePago(objCliente.EsDeExposicion);

            //Datos de Aseguradoras
            var aseguradoras = db.aspnet_Aseguradoras;
            ViewData["Aseguradoras"]=new SelectList(aseguradoras.AsEnumerable().OrderBy(a=>a.Nombre),"ID","Nombre");

            //Datos de Municipios, Provincias y Paises
            IEnumerable<LocalizacionesModel> objLoc=getLocalizaciones(objCliente.CP);

            ViewData["Localizaciones"] = getLocalizaciones();
            ViewData["Municipios"] = new SelectList(objLoc, "Municipio", "Municipio");
            ViewData["Provincias"] = new SelectList(getProvincias(objLoc), "IDProvincia", "Provincia");
            ViewData["Paises"] = new SelectList(getPaises(objLoc), "IDPais", "Pais");
            ViewData["Zonas"] = new SelectList(getZonas(),"IDZona","Nombre");

            //Datos de Actividad
            Areas.Clientes.Models.DSas400TableAdapters.INSAETableAdapter ta = new Models.DSas400TableAdapters.INSAETableAdapter();
            var actividades = (from a in ta.GetData() select a);
            list = new SelectList(actividades.AsEnumerable(), "AECDG", "AENBR");
            ViewData["Actividades"] = list;
            
            //Datos de formas de contacto, frecuencia de visita y diás de visita
            Areas.Clientes.Models.DSas400TableAdapters.BDGTNTableAdapter td = new Models.DSas400TableAdapters.BDGTNTableAdapter();
            var datos = (from d in td.GetDataContactos() select d);
            list = new SelectList(datos.AsEnumerable(), "TNCDG", "TNNBR");
            ViewData["FormasDeContacto"] = list;
            datos = (from d in td.GetDataDias() select d);
            list = new SelectList(datos.AsEnumerable(), "TNCDG", "TNNBR");
            ViewData["DiasDeVisita"] = list;
            datos = (from d in td.GetDataFrecuencias() select d);
            list = new SelectList(datos.AsEnumerable(), "TNCDG", "TNNBR");
            ViewData["FrecuenciaDeVisita"] = list;

            //Datos de Medios de Descarga
            var mediosDeDescarga = db.aspnet_MediosDeDescarga.Select(md => new { md.ID, md.Nombre });
            list = new SelectList(mediosDeDescarga.AsEnumerable(), "ID", "Nombre");
            ViewData["MediosDeDescarga"] = list;

            //Tipos de vehículo de servicio
            var tiposDeVehiculo = db.aspnet_TiposDeVehiculo.Select(md => new { md.ID, md.Nombre });
            list = new SelectList(tiposDeVehiculo.AsEnumerable(), "ID", "Nombre");
            ViewData["TiposDeVehiculos"] = list;

            //Instrumentos de pesaje
            var instrumentosDePesaje = db.aspnet_InstrumentosDePesaje.Select(md => new { md.ID, md.Nombre });
            list = new SelectList(instrumentosDePesaje.AsEnumerable(), "ID", "Nombre");
            ViewData["InstrumentosDePesaje"] = list;

            //Causas de no firma, datos de incoterms
            Areas.Clientes.Models.DSas400TableAdapters.INSICTableAdapter ti = new Models.DSas400TableAdapters.INSICTableAdapter();
            var datosC = (from ca in ti.GetData() select ca);
            list = new SelectList(datosC.AsEnumerable(), "ICCDG", "ICDES");
            ViewData["CausasDeNoFirma"] = list;

            if (objCliente.UsuarioDeAlta != null && objCliente.aspnet_Users==null)
            {
                foreach (var objUser in (from u in db.aspnet_Users where u.UserId == objCliente.UsuarioDeAlta select u))
                {
                    aspnet_Users user = new aspnet_Users();
                    user.UserId = objUser.UserId;
                    user.UserName = objUser.UserName;

                    objCliente.aspnet_Users = user;
                }
            }

            if (objCliente.UsuarioUltimaModificacion != null && objCliente.aspnet_Users1 == null)
            {
                foreach (var objUser in (from u in db.aspnet_Users where u.UserId == objCliente.UsuarioUltimaModificacion select u))
                {
                    aspnet_Users user = new aspnet_Users();
                    user.UserId = objUser.UserId;
                    user.UserName = objUser.UserName;

                    objCliente.aspnet_Users1 = user;
                }
            }
        }

        private void ValidarDatosFormulario(aspnet_Clientes modified, FormCollection formData)
        {
            CustomValidations Validations = new CustomValidations();
            string ErrMessage = "";
            
            //Reiniciamos el estado para quitar los posibles mensajes generados por MVC
            ModelState.Clear();

            //Validación campos del cliente obligatorios
            if (modified.EsBorrador == false)
            {
                if (modified.Empresas == "")
                {
                    ModelState.AddModelError("Empresas", "Es obligatorio indicar, al menos, una empresa para el cliente.");
                }

                if (modified.DelegacionID == 0)
                {
                    ModelState.AddModelError("DelegacionID","Es obligatorio indicar la delegación a la que va a pertenecer el cliente");
                }

                if (!Validations.IsValidNIF(modified.NIF, true, (modified.TipoDocumento == 1 ? "NIF o NIE" : "CIF"), ref ErrMessage))
                {
                    ModelState.AddModelError("NIF", ErrMessage);
                }

                if ((modified.Nombre ?? "").ToString() == "")
                {
                    ModelState.AddModelError("Nombre", "Es obligatorio indicar el nombre del cliente.");
                }
                if ((modified.TipoDeVia ?? "").ToString() == "")
                {
                    ModelState.AddModelError("TipoDeVia", "Es obligatorio indicar el tipo de vía del domicilio del cliente.");
                }
                if ((modified.Domicilio ?? "").ToString() == "")
                {
                    ModelState.AddModelError("Domicilio", "Es obligatorio indicar el nombre del domicilio del cliente.");
                }
                if ((modified.Numero ?? 0) == 0 && !modified.SinNumero)
                {
                    ModelState.AddModelError("Numero", "Es obligatorio indicar el número del domicilio del cliente.");
                }
                if (modified.CP == 0)
                {
                    ModelState.AddModelError("CP", "Es obligatorio indicar el CP del domicilio del cliente.");
                }
                if ((modified.Municipio ?? "").ToString() =="")
                {
                    ModelState.AddModelError("Municipio", "Es obligatorio indicar el municipio del domicilio del cliente.");
                }
                if ((modified.IDProvinciaQS ?? 0) == 0)
                {
                    ModelState.AddModelError("IDProvinciaQS", "Es obligatorio indicar la provincia del domicilio del cliente.");
                }
                if ((modified.IDPaisQS ?? 0) == 0)
                {
                    ModelState.AddModelError("IDPaisQS", "Es obligatorio indicar el país del domicilio del cliente.");
                }
                if ((modified.Zona ?? 0) == 0)
                {
                    ModelState.AddModelError("Zona", "Es obligatorio indicar un zona.");
                }
                if ((modified.Telefono1 ?? 0) == 0)
                {
                    ModelState.AddModelError("Telefono1", "Es obligatorio indicar Teléfono1 del cliente.");
                }
                if ((modified.FormaDePago ?? 0) == 0)
                {
                    if (modified.EsDeExposicion == true)
                    {
                        ModelState.AddModelError("FormaDePago", "Es obligatorio indicar una forma de pago");
                    }
                    else if ((modified.FormaDePagoSolicitada ?? "").ToString() == "")
                    {
                        ModelState.AddModelError("FormaDePagoSolicitada", "Es obligatorio indicar la forma de pago que se solicita.");
                    }
                }

                if (modified.RecogeEnNuestrasInstalaciones == false && modified.EsClienteParticular==false )
                {
                    //Validamos la ficha logística
                    if ((modified.Horario ?? "") == "")
                    {
                        ModelState.AddModelError("Horario", "Es obligatorio indicar el horario");
                    }
                    if ((modified.IDMedioDeDescarga ?? 0) == 0)
                    {
                        ModelState.AddModelError("IDMedioDeDescarga", "Es obligatorio indicar un medio de descarga");
                    }

                    if ((modified.IDTipoVehiculoServicio ?? 0) == 0)
                    {
                        ModelState.AddModelError("IDTipoVehiculoServicio", "Es obligatorio indicar un tipo de vehículo apto para el servicio");
                    }
                    if (modified.PesaElMaterial)
                    {
                        if ((modified.IDInstrumentoDePesaje ?? 0) == 0)
                        {
                            ModelState.AddModelError("IDInstrumentoDePesaje", "Es obligatorio indicar un instrumento de pesaje");
                        }
                    }
                    if (modified.CobroDePortesPorEnvio && ((modified.ImportePortesPorEnvio ?? 0) == 0))
                    {
                        ModelState.AddModelError("ImportePortesPorEnvio", "Si  ha indicado que se cobran portes por envío, es obligatorio indicar el importe que se cobrarán al cliente por envío");
                    }
                    if (!modified.CAEFirmada)
                    {
                        if ((modified.IDCausaNoFirmaCAE ?? "") == "")
                        {
                            ModelState.AddModelError("IDCausaNoFirmaCAE", "Es obligatorio indicar la causa por la que el cliente no firma la coordinación de actividades");
                        }
                    }
                }
                if (modified.NoAdmiteFacturacionElectronica == false)
                {
                    if (!Validations.isValidEMail(modified.MailDeFacturacion, ref ErrMessage))
                    {
                        ModelState.AddModelError("MailDeFacturacion", ErrMessage);
                    }
                    else if (modified.MailDeFacturacion.Length > 50)
                    {
                        ModelState.AddModelError("MailDeFacturacion", "Longitud máxima 50 caracteres");
                    }
                }
                if (modified.EsDeExposicion == false)
                {
                    //Estos datos sólo se validan si no es un cliente de exposición
                    if ((modified.IDActividadQS ?? "") == "")
                    {
                        ModelState.AddModelError("IDActividadQS", "Es obligatorio indicar la actividad del cliente.");
                    }
                    if ((modified.FormaContacto ?? "") == "")
                    {
                        ModelState.AddModelError("FormaContacto", "Es obligatorio indicar la forma de contacto con el cliente.");
                    }
                    if (modified.TipoCliente == "")
                    {
                        ModelState.AddModelError("TipoCliente", "Es obligatorio indicar un tipo de cliente");
                    }
                    if (modified.DiasVisita == "")
                    {
                        ModelState.AddModelError("DiasVisita", "Es obligatorio indicar los de visita para el cliente");
                    }
                    if (modified.FrecuenciaVisita == "")
                    {
                        ModelState.AddModelError("FrecuenciaVisita", "Es obligatorio indicar la frecuencia de visita para el cliente");
                    }
                    if ((modified.ConsumoPotencial ?? 0) == 0)
                    {
                        ModelState.AddModelError("ConsumoPotencial", "Es obligatorio indicar el consumo potencial del cliente");
                    }
                    else
                    {
                        if (!Validations.isDecimalInRange(modified.ConsumoPotencial, 0, 99999999, ref ErrMessage))
                        {
                            ModelState.AddModelError("ConsumoPotencial", ErrMessage);
                        }
                    }
                    if ((modified.PrevisionAnual ?? 0) == 0)
                    {
                        ModelState.AddModelError("PrevisionAnual", "Es obligatorio indicar la previsión anual del cliente");
                    }
                    else
                    {
                        if (!Validations.isDecimalInRange(modified.PrevisionAnual, 0, 99999999, ref ErrMessage))
                        {
                            ModelState.AddModelError("ConsumoPotencial", ErrMessage);
                        }
                    }
                    if (modified.Consume == "")
                    {
                        ModelState.AddModelError("arrMateriales", "Debe indicar al menos un producto de consumo");
                    }
                    
                    string[] CIFSCli = formData["arrCliHabCIF1"].Split(',');
                    string[] NombresCli = formData["arrCliHabNombre1"].Split(',');

                    if (NombresCli.Length == 0)
                    {
                        ModelState.AddModelError("arrCliHabNombre1", "Es obligatorio indicar, al menos, un nombre del cliente habitual");
                    }
                    else
                    {
                        for (int i = 0; i < NombresCli.Length; i++)
                        {
                            if (CIFSCli[i] != "" && !Validations.IsValidNIF(CIFSCli[i],false,"", ref ErrMessage))
                            {
                                ModelState.AddModelError("arrCliHabCIF" + (i + 1).ToString(), ErrMessage);
                            }
                            if (NombresCli[i] == "")
                            {
                                ModelState.AddModelError("arrCliHabNombre" + (i + 1).ToString(), "Es obligatorio indicar el nombre del cliente habitual.");
                            }
                            else if (NombresCli[i].ToString().Length > 50)
                            {
                                ModelState.AddModelError("arrCliHabNombre" + (i + 1).ToString(), "Longitud máxima 50 caracteres");
                            }
                        }
                    }

                    string[] CIFSProv = formData["arrProvHabCIF1"].Split(',');
                    string[] NombresProv = formData["arrProvHabNombre1"].Split(',');

                    if (NombresProv.Length == 0)
                    {
                        ModelState.AddModelError("arrProvHabNombre1", "Es obligatori indicar, al menos, un nombre del proveedor habitual");
                    }
                    else
                    {
                        for (int i = 0; i < NombresProv.Length; i++)
                        {
                            if (CIFSProv[i] != "" && !Validations.IsValidNIF(CIFSProv[i],false,"", ref ErrMessage))
                            {
                                ModelState.AddModelError("arrProvHabCIF" + (i + 1).ToString(), ErrMessage);
                            }
                            if (NombresProv[i] == "")
                            {
                                ModelState.AddModelError("arrProvHabNombre" + (i + 1).ToString(), "Es obligatorio indicar el nombre del proveedor habitual.");
                            }
                            else if (NombresProv[i].ToString().Length > 50)
                            {
                                ModelState.AddModelError("arrProvHabNombre" + (i + 1).ToString(), "Longitud máxima 50 caracteres");
                            }

                        }
                    }
                    if (modified.NoTieneVtosFijos == false)
                    {
                        if ((modified.DiaVtoFijo1 ?? 0) == 0)
                        {
                            ModelState.AddModelError("DiaVtoFijo1", "Es obligatorio indicar, al menos, un día de vto. fijo");
                        }
                        else if (Validations.isDecimalInRange(modified.DiaVtoFijo1, 1, 31, ref ErrMessage) == false)
                        {
                            ModelState.AddModelError("DiaVtoFijo1", ErrMessage);
                        }

                        if ((modified.DiaVtoFijo2 ?? 0) != 0)
                        {
                            if (Validations.isDecimalInRange(modified.DiaVtoFijo2, 1, 31, ref ErrMessage) == false)
                            {
                                ModelState.AddModelError("DiaVtoFijo2", ErrMessage);
                            }
                        }
                        if ((modified.DiaVtoFijo3 ?? 0) != 0)
                        {
                            if (Validations.isDecimalInRange(modified.DiaVtoFijo3, 1, 31, ref ErrMessage) == false)
                            {
                                ModelState.AddModelError("DiaVtoFijo3", ErrMessage);
                            }
                        }
                    }
                } //Fin cliente de Expo
                
            } // Fin Borrador

            //Validamos longitudes máximas campos válidos            
            
            if ((modified.Nombre??"").ToString().Length > 30)
            {
                ModelState.AddModelError("Nombre", "Longitud máxima 30 caracteres.");
            }
            if ((modified.Domicilio??"").ToString().Length > 25)
            {
                ModelState.AddModelError("Domicilio", "Longitud máxima 25 caracteres.");
            }
            if ((modified.Piso??"").ToString().Length>15)
            {
                ModelState.AddModelError("Piso", "Longitud máxima 15 caracteres.");
            }
            if ((modified.Telefono1??0).ToString().Length > 10)
            {
                ModelState.AddModelError("Telefono1", "Longitud máxima 10 caracteres.");
            }
            if ((modified.Telefono2 ?? 0).ToString().Length > 10)
            {
                ModelState.AddModelError("Telefono2", "Longitud máxima 10 caracteres.");
            }
            if ((modified.Fax ?? 0).ToString().Length > 10)
            {
                ModelState.AddModelError("Fax", "Longitud máxima 10 caracteres.");
            }
            if (modified.TieneMail == true)
            {
                if (!Validations.isValidEMail(modified.MailDeContacto, ref ErrMessage))
                {
                    ModelState.AddModelError("MailDeContacto", ErrMessage);
                }
                else if (modified.MailDeContacto.ToString().Length > 40)
                {
                    ModelState.AddModelError("MailDeContacto", "Longitud máxima 40 caracteres");
                }
            }
            if ((modified.FormaDePago??0) == 0)
            {
                if ((modified.FormaDePagoSolicitada??"").Length > 30)
                {
                    ModelState.AddModelError("FormaDePagoSolicitada", "Longitud máxima 30 caracteres");
                }
            }
            //Validamos la ficha logística en las longitudes he quitado los textos fijos que se añaden en los campos.
            if ((modified.Horario ?? "").Length > 50)
            {
                ModelState.AddModelError("Horario", "Longitud máxima 50 caracteres");
            }

            if ((modified.HorarioDeVerano ?? "").Length > 40)
            {
                ModelState.AddModelError("HorarioDeVerano", "Longitud máxima 40 caracteres");
            }

            if ((modified.PersonaDeDescarga ?? "").Length > 30)
            {
                ModelState.AddModelError("PersonaDeDescarga", "Longitud máxima 30 caracteres");
            }

            if ((modified.RequerimientosDePrevencion ?? "").Length > 30)
            {
                ModelState.AddModelError("RequerimientosDePrevencion", "Longitud máxima 30 caracteres");
            }

            if (modified.EsDeExposicion == false)
            {
                if ((modified.Gerente ?? "").Length > 50)
                {
                    ModelState.AddModelError("Gerente", "Longitud máxima 50 caracteres");
                }
                if ((modified.GrupoEmpresarial ?? "").Length > 50)
                {
                    ModelState.AddModelError("GrupoEmpresarial", "Longitud máxima 50 caracteres");
                }
                if (modified.NoAdmiteFacturacionElectronica == true)
                {
                    if ((modified.CPFacturacion ?? 0) == 0)
                    {
                        ModelState.AddModelError("DomicilioFacturacion", "Si el cliente no admite facturación electrónica es obligatorio indicar una dirección postal.");
                    }
                }
                if (formData["EsSepa"] == "1")
                {
                    //Validamos el IBAN si corresponde
                    if ((modified.IBANCODE ?? "").ToString() == "" && modified.ExistePedidoEnFirme == true)
                    {
                        ModelState.AddModelError("IBANCODE", "El IBAN es obligatorio para la forma de pago seleccionada.");
                    }

                    if ((modified.IBANCODE ?? "").ToString() != "" && !Validations.isValidIBAN(modified.IBANSIGLAS.ToString() + modified.IBANCODE.ToString() + modified.IBANENTIDAD.ToString() + modified.IBANSUCURSAL.ToString() + modified.IBANDC.ToString() + modified.IBANCCC.ToString(), ref ErrMessage))
                    {
                        ModelState.AddModelError("IBANCODE", ErrMessage);
                    }

                    //Validamos los datos de Crédito
                    if ((modified.LimitePropuesto ?? 0) == 0)
                    {
                        ModelState.AddModelError("LimitePropuesto", "El límite propuesto es obligatorio para la forma de pago seleccionada.");
                    }
                }
            }
            //Fin control campos Clientes

            //Tablas vinculadas
            if (modified.TienePersonasAutorizadasRetMat)
            {
                string[] NIFPersonasAutRetMat = formData["arrNIFPersona1"].Split(',');
                string[] NombrePersonasAutRetMat = formData["arrNombrePersona1"].Split(',');

                if (NIFPersonasAutRetMat.Length == 0)
                {
                    ModelState.AddModelError("TienePersonalAutorizado", "Es necesario indicar, al menos, los datos de una persona autorizada");
                }
                else
                {
                    for (int i=0; i< NIFPersonasAutRetMat.Length;i++)
                    {
                        if (NombrePersonasAutRetMat[i] == "")
                        {
                            ModelState.AddModelError("arrNombrePersona" + (i + 1).ToString(), "Es obligatorio indicar el nombre de la persona");
                        }
                        if (NIFPersonasAutRetMat[i]!="" && !Validations.IsValidNIF(NIFPersonasAutRetMat[i],true,"NIF o NIE",ref ErrMessage)){
                            ModelState.AddModelError("arrNIFPersona" + (i+1).ToString(), ErrMessage);
                        }
                    }
                }
            }

            if (modified.TieneSocios==1)
            {
                string[] socios= formData["arrNombreSocio1"].Split(',');
                string[] CIFS = formData["arrCIFSocio1"].Split(',');
                string[] porcentajes = formData["arrPorcentaSocio1"].Split(',');

                if (socios.Length == 0)
                {
                    ModelState.AddModelError("TieneSocios", "Es necesario indicar, al menos, el nombre de uno de los socios.");
                }
                else
                {
                    for (int i = 0; i < socios.Length;i++ )
                    {
                        if (socios[i] == "")
                        {
                            ModelState.AddModelError("arrNombreSocio"+ (i+1).ToString(), "Es obligatorio indicar el nombre del socio");
                        }
                        if (CIFS[i]!="" && !Validations.IsValidNIF(CIFS[i],false,"", ref ErrMessage))
                        {
                            ModelState.AddModelError("arrCIFSocio" + (i + 1).ToString(), ErrMessage);
                        }
                        if (porcentajes[i]!="" && !Validations.isDecimalInRange(porcentajes[i],0,100, ref ErrMessage))
                        {
                            ModelState.AddModelError("arrPorcentaSocio" + (i + 1).ToString(), ErrMessage);
                        }
                    }
                }                
            }
            if (modified.TieneEmpresasVinculadas == 1)
            {
                string[] CIFS = formData["arrCIFVinc1"].Split(',');
                string[] empresas = formData["arrEmpVinc1"].Split(',');

                if (empresas.Length == 0)
                {
                    ModelState.AddModelError("arrEmpVinc1", "Es necesario indicar, al menos, el nombre de una de las empresas vinculadas.");
                }
                else
                {
                    for (int i = 0; i < empresas.Length; i++)
                    {
                        if (CIFS[i] != "" && !Validations.IsValidNIF(CIFS[i],true,"CIF", ref ErrMessage))
                        {
                            ModelState.AddModelError("arrCIFVinc" + (i + 1).ToString(), ErrMessage);
                        }
                        if (empresas[i] == "")
                        {
                            ModelState.AddModelError("arrEmpVinc" + (i + 1).ToString(), "Es obligatorio indicar el nombre de la empresa");
                        }
                        else if (empresas[i].ToString().Length > 50)
                        {
                            ModelState.AddModelError("arrEmpVinc" + (i + 1).ToString(), "Logitud máxima 50 caracteres");
                        }
                    }
                }
            }

            if (formData["EsSepa"] == "1")
            {
                //Validamos los bancos con los que trabaja
                string[] Bancos = formData["arrNombreBanco1"].Split(',');
                string[] Oficinas = formData["arrOficinaBanco1"].Split(',');

                if (Bancos.Length == 0)
                {
                    ModelState.AddModelError("arrNombreBanco1", "Es obligatorio indicar, al menos, un banco con el que trabaje el cliente.");
                }
                else
                {
                    for (int i = 0; i < Bancos.Length; i++)
                    {
                        if (Bancos[i] == "")
                        {
                            ModelState.AddModelError("arrNombreBanco" + (i+1).ToString(), "Es obligatior indicar el nombre del banco.");
                        }
                        else if(Bancos[i].ToString().Length>50)
                        {
                            ModelState.AddModelError("arrNombreBanco" + (i+1).ToString(), "Longitud máxima 50 caracteres.");
                        }

                        if (Oficinas[i] != "" && Oficinas[i].ToString().Length > 50)
                        {
                            ModelState.AddModelError("arrOficinaBanco" + (i+1).ToString(), "Longitud máxima 50 caracteres.");
                        }
                    }
                }

            }

            
            //Fin tablas vinculadas            
        }

        private void guardarDatosCliente(aspnet_Clientes modified, FormCollection formData)
        {
            //Borramos los socios vinculados con el cliente
            var sociosActuales = (from a in db.aspnet_Accionariado where a.IDCliente == modified.ID select a);
            foreach (var socio in sociosActuales)
            {
                db.aspnet_Accionariado.DeleteObject(socio);
            }
            
            //Añádimos los nuevos socios en caso de que existan
            foreach (var socio in modified.aspnet_Accionariado)
            {
                db.aspnet_Accionariado.AddObject(socio);
            }

            //Borramos los empresas vinculadas relacionadas con el cliente
            var empVincActuales = (from s in db.aspnet_SociedadesVinculadas where s.IDCliente == modified.ID select s);
            foreach (var empresa in empVincActuales)
            {
                db.aspnet_SociedadesVinculadas.DeleteObject(empresa);
            }

            //Añádimos las nuevas empresas vinculadas en caso de que existan
            foreach (var empresa in modified.aspnet_SociedadesVinculadas)
            {
                db.aspnet_SociedadesVinculadas.AddObject(empresa);
            }

            //Borramos las personas autorizadas para retirada de material
            var personasAut = (from pa in db.aspnet_PersonasRetiradaMat where pa.IDCliente == modified.ID select pa);
            foreach (var persona in personasAut)
            {
                db.aspnet_PersonasRetiradaMat.DeleteObject(persona);
            }
            
            //Añadimos las personas autorizadas para retirada de material
            foreach (var persona in modified.aspnet_PersonasRetiradaMat)
            {
                db.aspnet_PersonasRetiradaMat.AddObject(persona);
            }

            //Borramos las direcciones de envío vinculadas con el cliente
            var direccionesEnvio = (from de in db.aspnet_ClientesDirEnv where de.IDCliente == modified.ID select de);
            foreach (var direccion in direccionesEnvio)
            {
                db.aspnet_ClientesDirEnv.DeleteObject(direccion);
            }

            //Añádimos las direcciones de envío en caso de que existan
            foreach (var direccion in modified.aspnet_ClientesDirEnv)
            {
                db.aspnet_ClientesDirEnv.AddObject(direccion);
            }

            //Borramos los bancos con los que trabaja el cliente
            var bancosCliActuales=(from b in db.aspnet_BancosCliente where b.IDCliente==modified.ID select b);
            foreach (var banco in bancosCliActuales)
            {
                db.aspnet_BancosCliente.DeleteObject(banco);
            }

            //Añadimos los nuevos bancos con los que trabaja el cliente en caso de que existan
            foreach (var banco in modified.aspnet_BancosCliente)
            {
                db.aspnet_BancosCliente.AddObject(banco);
            }

            //Borramos los clientes habituales relacionadas con el cliente
            var clihabActuales = (from c in db.aspnet_ClientesHabituales where c.IDCliente == modified.ID select c);
            foreach (var cliente in clihabActuales)
            {
                db.aspnet_ClientesHabituales.DeleteObject(cliente);
            }

            //Añádimos los nuevos clientes habituales vinculadas en caso de que existan
            foreach (var cliente in modified.aspnet_ClientesHabituales)
            {
                db.aspnet_ClientesHabituales.AddObject(cliente);
            }

            //Borramos los proveeedores habituales relacionadas con el cliente

            var provhabActuales = (from c in db.aspnet_ProveedoresHabituales where c.IDCliente == modified.ID select c);
            foreach (var proveedor in provhabActuales)
            {
                db.aspnet_ProveedoresHabituales.DeleteObject(proveedor);
            }

            //Añádimos los nuevos clientes habituales vinculadas en caso de que existan
            foreach (var proveedor in modified.aspnet_ProveedoresHabituales)
            {
                db.aspnet_ProveedoresHabituales.AddObject(proveedor);
            }

            //Volcamos las modificaciones a la BD.
            if (modified.ID == 0)
            {
                modified.UsuarioDeAlta = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                modified.FechaDeAlta = DateTime.Now;
                db.AddToaspnet_Clientes(modified);
            }
            else
            {
                //Guardamos usuario y fecha de última modificación
                modified.UsuarioUltimaModificacion = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                modified.UltimaModificación = DateTime.Now;

                //Sólo es necesario si no se ha añadido ninguna de las tablas relacionadas
                if (modified.aspnet_Accionariado.Count() == 0 && modified.aspnet_SociedadesVinculadas.Count() == 0 && modified.aspnet_ClientesHabituales.Count() == 0 && modified.aspnet_ProveedoresHabituales.Count() == 0)
                {
                    db.aspnet_Clientes.Attach(modified);
                }
            
                db.ObjectStateManager.ChangeObjectState(modified, System.Data.EntityState.Modified);
            }

            db.SaveChanges();
            
        }

        private void asignarDatosFormulario(aspnet_Clientes modified, FormCollection formData)
        {
            
            //Asignamos el agente asociado al perfil
            //modified.IDAgente1QS = Convert.ToDecimal(profile["IDAgenteQS"].ToString());
            //Asignamos los valores de los checkbox
            modified.Empresas = formData["arrEmpresas"].ToString().Replace("false,", "").Replace(",false", "").Replace("false", "");
            modified.Consume = formData["arrMateriales"].ToString().Replace("false,", "").Replace(",false", "").Replace("false", "");
            modified.RequerimientosDeCalidad = formData["arrRequerimientosCal"].ToString().Replace("false,", "").Replace(",false", "").Replace("false", "");
            if (modified.CAEFirmada == false && (modified.FicheroCAE ?? "") != "")
            {
                //Si se ha marcado como que no está firmada la CAE y antés sí que teníamos fichero, nos guardamos el nombre fichero en FicheroCAEanterior por si tuviesemos que recuperarlo
                modified.FicheroCAEanterior = modified.FicheroCAE;
                modified.FicheroCAE = null;
            }
            foreach (string strFileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[strFileName];
                if (file.ContentLength > 0)
                {
                    string filePath = Path.Combine(HttpContext.Server.MapPath("~/CAEFirmadas"), Path.GetFileName(file.FileName));
                    int i = 1;
                    while(System.IO.File.Exists(filePath)){
                        filePath = Path.Combine(HttpContext.Server.MapPath("~/CAEFirmadas"), Path.GetFileNameWithoutExtension(file.FileName) +i.ToString()+Path.GetExtension(file.FileName));
                        i++;
                    }
                    modified.FicheroCAE = filePath.Replace(HttpContext.Server.MapPath("~"), "/"); //En la BD quitamos esta parte de la ruta para que puede cargarse el fichero posteriormente en el navegador.
                    modified.IDCausaNoFirmaCAE = null; //Vaciamos este campo por si tenía algún valor anterior.
                    file.SaveAs(filePath);
                }
            }
            
            //Asignamos valores de las tablas vinculadas
            if (modified.TieneSocios == 1)
            {
                modified.aspnet_Accionariado.Clear();

                string[] socios = formData["arrNombreSocio1"].Split(',');
                string[] CIFS = formData["arrCIFSocio1"].Split(',');
                string[] porcentajes = formData["arrPorcentaSocio1"].Split(',');

                if (socios.Length > 0)
                {
                    for (int i = 0; i < socios.Length; i++)
                    {
                        aspnet_Accionariado socio = new aspnet_Accionariado();
                        int intPorcentaje;
                        socio.IDCliente = modified.ID;
                        socio.Nombre = socios[i].ToString();
                        socio.CIF = CIFS[i].ToString();
                        int.TryParse(porcentajes[i].ToString(), out intPorcentaje);
                        socio.Porcentaje = intPorcentaje;

                        modified.aspnet_Accionariado.Add(socio);
                    }
                }
            }
            else
            {
                modified.aspnet_Accionariado.Clear();
            }

            if (modified.TieneEmpresasVinculadas == 1)
            {
                modified.aspnet_SociedadesVinculadas.Clear();

                string[] sociedades = formData["arrEmpVinc1"].Split(',');
                string[] CIFS = formData["arrCIFVinc1"].Split(',');

                if (sociedades.Length > 0)
                {
                    for (int i = 0; i < sociedades.Length; i++)
                    {
                        aspnet_SociedadesVinculadas sociedad = new aspnet_SociedadesVinculadas();

                        sociedad.IDCliente = modified.ID;
                        sociedad.CIF = CIFS[i].ToString();
                        sociedad.Nombre = sociedades[i].ToString();

                        modified.aspnet_SociedadesVinculadas.Add(sociedad);
                    }
                }
            }
            else
            {
                modified.aspnet_SociedadesVinculadas.Clear();
            }

            //Personas autorizadas para recoger
            modified.aspnet_PersonasRetiradaMat.Clear();
            if (modified.TienePersonasAutorizadasRetMat)
            {
                string[] dnisAutorizados = formData["arrNIFPersona1"].Split(',');
                string[] nombresAutorizados = formData["arrNombrePersona1"].Split(',');

                if (dnisAutorizados.Length > 0)
                {
                    for (int i = 0; i < dnisAutorizados.Length; i++)
                    {
                        aspnet_PersonasRetiradaMat personaAut = new aspnet_PersonasRetiradaMat();
                        personaAut.IDCliente = modified.ID;
                        personaAut.NIF = dnisAutorizados[i].ToString();
                        personaAut.Nombre = nombresAutorizados[i].ToString();

                        modified.aspnet_PersonasRetiradaMat.Add(personaAut);
                    }
                }
            }
            
            //Direcciones de envío
            modified.aspnet_ClientesDirEnv.Clear();
            if (modified.TieneDireccionesDeEnvio)
            {
                string[] nombres = formData["arrNombreDirEnv1"].Split(',');
                string[] tiposDeVia = formData["arrTipoDeViaDirEnv1"].Split(',');
                string[] domicilios = formData["arrDomicilioDirEnv1"].Split(',');
                string[] sinNumeros = formData["arrSinNumeroDirEnv1"].Split(',');
                string[] numeros = formData["arrNumeroDirEnv1"].Split(',');
                string[] pisos = formData["arrPisoDirEnv1"].Split(',');
                string[] cps = formData["arrCPDirEnv1"].Split(',');
                string[] idmunicipiosQS = formData["arrIDMunicipioQSDirEnv1"].Split(',');
                string[] municipios = formData["arrMunicipioDirEnv1"].Split(',');
                string[] idsmunicipios = formData["arrIDMunicipioQSDirEnv1"].Split(',');
                string[] provincias = formData["arrIDProvinciaQSDirEnv1"].Split(',');
                string[] paises = formData["arrIDPaisQSDirEnv1"].Split(',');

                if (tiposDeVia.Length > 0)
                {
                    for (int i = 0; i < tiposDeVia.Length; i++)
                    {
                        aspnet_ClientesDirEnv dirEnv = new aspnet_ClientesDirEnv();

                        dirEnv.IDCliente = modified.ID;
                        dirEnv.Nombre = nombres[i].ToString();  
                        dirEnv.TipoDeVía = tiposDeVia[i].ToString();
                        dirEnv.Domicilio = domicilios[i].ToString();
                        if (sinNumeros[i].ToString() == "0")
                        {
                            //Sólo asignamos el número si no está marcado el campo SinNumero
                            dirEnv.Numero = Convert.ToDecimal(numeros[i]);
                        }
                        dirEnv.Piso = pisos[i].ToString();
                        dirEnv.CP = Convert.ToInt32(cps[i].ToString());
                        dirEnv.IDMunicipioQS = Convert.ToInt32(idmunicipiosQS[i].ToString());
                        dirEnv.Municipio = municipios[i].ToString();
                        dirEnv.IDProvinciaQS = Convert.ToDecimal(provincias[i]);
                        dirEnv.IDPaisQS = Convert.ToDecimal(paises[i]);

                        modified.aspnet_ClientesDirEnv.Add(dirEnv);
                    }
                }
            }
            
            //Bancos con los que trabaja
            modified.aspnet_BancosCliente.Clear();
            if (formData["EsSepa"] == "1")
            {
                string[] Bancos = formData["arrNombreBanco1"].Split(',');
                string[] Oficinas = formData["arrOficinaBanco1"].Split(',');

                if (Bancos.Length > 0)
                {
                    for (int i = 0; i < Bancos.Length; i++)
                    {
                        aspnet_BancosCliente Banco = new aspnet_BancosCliente();

                        Banco.IDCliente = modified.ID;
                        Banco.Nombre = Bancos[i].ToString();
                        Banco.Sucursal = Oficinas[i].ToString();

                        modified.aspnet_BancosCliente.Add(Banco);
                    }
                }
            }

            modified.aspnet_ClientesHabituales.Clear();
            modified.aspnet_ProveedoresHabituales.Clear();
            if (modified.EsDeExposicion == false)
            {
                string[] CIFSCli = formData["arrCliHabCIF1"].Split(',');
                string[] NombresCli = formData["arrCliHabNombre1"].Split(',');

                if (NombresCli.Length > 0)
                {
                    for (int i = 0; i < NombresCli.Length; i++)
                    {
                        aspnet_ClientesHabituales clienteHab = new aspnet_ClientesHabituales();

                        clienteHab.IDCliente = modified.ID;
                        clienteHab.NIF = CIFSCli[i].ToString();
                        clienteHab.NombreCliente = NombresCli[i].ToString();

                        modified.aspnet_ClientesHabituales.Add(clienteHab);
                    }
                }

                string[] CIFSProv = formData["arrProvHabCIF1"].Split(',');
                string[] NombresProv = formData["arrProvHabNombre1"].Split(',');

                if (NombresProv.Length > 0)
                {
                    for (int i = 0; i < NombresProv.Length; i++)
                    {
                        aspnet_ProveedoresHabituales provHab = new aspnet_ProveedoresHabituales();

                        provHab.IDCliente = modified.ID;
                        provHab.NIF = CIFSProv[i].ToString();
                        provHab.NombreProveedor = NombresProv[i].ToString();

                        modified.aspnet_ProveedoresHabituales.Add(provHab);
                    }
                }
            }
        }

        private bool crearClienteEnQS(aspnet_Clientes objCliente,ref string errMessage){
            bool blnRes = true;
            ClientesModel objCliModel = new ClientesModel();
            try
            {
                string[] arrEmpresas = objCliente.Empresas.Split(',');

                foreach (string strEmpresa in arrEmpresas)
                {
                    if (!objCliModel.crearClienteEnQS(strEmpresa, objCliente,ref errMessage))
                    {
                        blnRes = false;
                        break;
                    }
                }

                //Si todo ha ido bien, lo creamos también en la empresa 001 - GENERICA
                if (!objCliModel.crearClienteEnQS("001", objCliente, ref errMessage))
                {
                    blnRes = false;
                }
            }
            catch (Exception ex)
            {
                blnRes = false;
                errMessage += Environment.NewLine + "- Error en Controller.crearQuienteEnQS: " + ex.Message;
            }
            return blnRes;
        }

        private void borrarClienteQS(aspnet_Clientes objCliente, ref string errMessage)
        {
            ClientesModel objCliModel = new ClientesModel();
            string[] arrEmpresas = objCliente.Empresas.Split(',');
            foreach (string strEmpresa in arrEmpresas)
            {
                objCliModel.borrarClienteEnEmpresaQS(strEmpresa, objCliente.QSID, ref errMessage);
            }
        }


        private SelectList getFormasDePago(bool blnSoloExposicion)
        {
            FormasDePagoModel objFormasDePago = new FormasDePagoModel();
            SelectList list;
            
            if (User.IsInRole("Clientes") || User.IsInRole("Administrador") || User.IsInRole("Créditos"))
            {
                //Mostramos todas las forma de pago
                list = new SelectList(objFormasDePago.getFormasDePago().OrderBy(fp => fp.Nombre), "ID", "Nombre");
            }
            else
            {
                //Solo mostramos las formas de pago marcadas como visibles en el Mto. de formas de pago.
                list = new SelectList(objFormasDePago.getFormasDePago().Where(fp => fp.Visible == true).OrderBy(fp => fp.Nombre), "ID", "Nombre");
            }
            
            if (blnSoloExposicion)
            {
                list = new SelectList(objFormasDePago.getFormasDePago().Where(fp => fp.Visible == true && fp.DisponibleExposicion == true).OrderBy(fp => fp.Nombre), "ID", "Nombre");
            }

            return list;
        }

        private IEnumerable<LocalizacionesModel> getLocalizaciones()
        {
            LocalizacionesModel objLocalizaciones = new LocalizacionesModel();

            IEnumerable<LocalizacionesModel> arrLoc = objLocalizaciones.getLocalizaciones().Distinct();

            return arrLoc;
        }

        private IEnumerable<LocalizacionesModel> getLocalizaciones(decimal decCP)
        {
            LocalizacionesModel objLocalizaciones = new LocalizacionesModel();

            IEnumerable<LocalizacionesModel> arrLoc = objLocalizaciones.getLocalizaciones().Where(loc => loc.CP == decCP || loc.CP==0).Distinct();

            return arrLoc;
        }

        private System.Collections.IEnumerable getProvincias(IEnumerable<LocalizacionesModel> objLoc)
        {
            return (from provincias in objLoc select new { IDProvincia = provincias.IDProvincia, Provincia = provincias.Provincia }).Distinct().OrderBy (prov=>prov.Provincia);
        }

        private System.Collections.IEnumerable getPaises(IEnumerable<LocalizacionesModel> objLoc)
        {
            return (from pais in objLoc select new { IDPais = pais.IDPais, Pais = pais.Pais }).Distinct().OrderBy(pais => pais.Pais);
        }

        private System.Collections.IEnumerable getZonas()
        {
            ZonasModel objZonas = new ZonasModel();
            IEnumerable<ZonasModel> arrZonas = objZonas.getZonas().Distinct();

            return arrZonas;
        }

        public ActionResult getJsonFormasDePago(bool blnSoloExposicion)
        {
            return Json(getFormasDePago(blnSoloExposicion), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getJsonDatosFormaDePago(int id)
        {
            var objFormaDePago = (new FormasDePagoModel()).getFormaDePagoById(id);
            
            return Json(objFormaDePago, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getJsonLocalizaciones(int CP)
        {
            return Json(getLocalizaciones(CP), JsonRequestBehavior.AllowGet);
        }

        public ActionResult getJsonZonas(int IDMun, int IDProv, int IDPais)
        {
            return Json((new ZonasModel()).getZonaByLoc(IDMun, IDProv, IDPais),JsonRequestBehavior.AllowGet);
        }

        public ActionResult getJsonTarifa(string strTipoCliente, string strActividad)
        {
            string strRes="";
            if (strTipoCliente.Trim() == "A" && (strActividad.Trim() == "F" || strActividad.Trim() == "AF"))
            {
                strRes="3";
            }
            else if ((strTipoCliente.Trim() == "B" || strTipoCliente.Trim() == "C") && (strActividad.Trim() == "F" || strActividad.Trim() == "AF")) 
            {
                strRes = "4";
            }
            else if ((strTipoCliente.Trim() == "P" || strTipoCliente.Trim() == "A") && (strActividad.Trim() == "PR" || strActividad.Trim() == "C" || strActividad.Trim() == "CP")) 
            {
                strRes = "21";
            }
            else if ((strTipoCliente.Trim() == "B" || strTipoCliente.Trim() == "C") && (strActividad.Trim() == "PR" || strActividad.Trim() == "C" || strActividad.Trim() == "CP"))
            {
                strRes = "22";
            }
            else if (strActividad.Trim() == "RF" || strActividad.Trim() == "MT" || strActividad.Trim() == "GE" || strActividad.Trim() == "D" || strActividad.Trim() == "PLAC")
            {
                strRes = "90";
            }
            return Json(strRes, JsonRequestBehavior.AllowGet);
        }

        public void guardarObservaciones(Int32 intIDCliente, string strObservaciones)
        {
            aspnet_Clientes objCliente = db.aspnet_Clientes.FirstOrDefault(c => c.ID == intIDCliente);

            if (objCliente != null)
            {
                objCliente.ObservacionesGestion = strObservaciones;
                db.ObjectStateManager.ChangeObjectState(objCliente, EntityState.Modified);
                db.SaveChanges();
            }
        }

        private bool volcarClienteABBDD(aspnet_Clientes objCliente){
            bool blnRes = true;
            ClientesModel objCliModel = new ClientesModel();
            string errMessage = "";
            if (objCliente.FormaDePago == null)
            {
                ModelState.AddModelError("", "El cliente " + objCliente.Nombre + " no puede volcarse a las bases de datos ya que no tiene forma de pago asignada.");
            }
            else
            {
                //Obteneoms el código de cliente
                objCliente.QSID = objCliModel.getIDCliente(objCliente.Empresas, objCliente.NIF, objCliente.EsDeExposicion);
                if (objCliente.QSID != 0)
                {
                    //Creamos el cliente en la BD de clientes
                    if (objCliModel.crearClienteEnBDClientes(objCliente,ref errMessage))
                    {
                        //Creamos el cliente en QS
                        //Asignamos fecha y usuario de volcado a QS
                        objCliente.FechaVolcadoQS = System.DateTime.Now;
                        objCliente.UsuarioVolcadoQS = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                        if (!crearClienteEnQS(objCliente, ref errMessage))
                        {
                            //Borramos el cliente de QS por si se ha podido volcar en alguna de las empresas
                            borrarClienteQS(objCliente, ref errMessage);
                            //Borramos el cliente de la BD de clientes
                            objCliModel.borrarClienteEnBDClientes(objCliente, ref errMessage);
                            //Dejamos a null el ID para que no pase a Aperturados
                            objCliente.QSID = null;
                            ModelState.AddModelError("", "El cliente " + objCliente.Nombre + " no ha podido volcarse a la base de datos de QS. Revise los datos y vuelva a intentarlo." + (errMessage == "" ? "" : Environment.NewLine + " ERRORES: " + errMessage));
                            blnRes = false;
                        }
                        else
                        {
                            //Si todo ha ido bien, volcamos la información de la ficha logística al programa de transporte
                            if (objCliModel.crearExcepcionesClienteEnBDPlanificacion(objCliente))
                            {
                                //Si todo ha ido bien grabamos el usuario y la fecha de volcado a QS
                                objCliente.FechaValidacionLogistica = System.DateTime.Now;
                                objCliente.UsuarioValidacionLogistica = (Guid)Membership.GetUser(User.Identity.Name).ProviderUserKey;
                            }
                            else
                            {
                                //Borramos el cliente de QS por si se ha podido volcar en alguna de las empresas
                                borrarClienteQS(objCliente, ref errMessage);
                                //Borramos el cliente de la BD de clientes
                                objCliModel.borrarClienteEnBDClientes(objCliente, ref errMessage);
                                //Dejamos a null el ID para que no pase a Aperturados
                                objCliente.QSID = null;
                                //Quitamos fecha y usuario de volcado a QS
                                objCliente.FechaVolcadoQS = null;
                                objCliente.UsuarioVolcadoQS = null;
                                ModelState.AddModelError("", "El cliente " + objCliente.Nombre + " no ha podido volcarse a la base de datos de transporte. Revise los datos y vuelva a intentarlo." + (errMessage =="" ? "" : Environment.NewLine + " ERRORES: " + errMessage) );
                                blnRes = false;
                            }
                                                
                        }
                    }
                    else
                    {
                        //Borramos el cliente de la BD de Clientes para liberar el código
                        objCliModel.borrarClienteEnBDClientes(objCliente, ref errMessage);
                        objCliente.QSID = null;
                        ModelState.AddModelError("", "El cliente " + objCliente.Nombre + " no ha podido volcarse a la base de datos de clientes. Revise los datos y vuelva a intentarlo." + (errMessage == "" ? "" : Environment.NewLine + " ERRORES: " + errMessage));
                        blnRes = false;
                    }
                }
                else
                {
                    objCliente.QSID = null;
                    ModelState.AddModelError("", "No se ha podido obtener código para el cliente " + objCliente.Nombre);
                    blnRes = false;
                }
            }
            //Actualizamos el cliente de la BD de AplicacionesGM para que pase a Aperturados en caso de que todo haya ido bien.
            db.SaveChanges();
            return blnRes;
        }


    }
}
