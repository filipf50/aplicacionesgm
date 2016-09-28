using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Security;
using System.Web.Profile;
using AplicacionesGM_MVC.Models;
using System.Data;
using System.Security;

namespace AplicacionesGM_MVC.Areas.Reuniones.Controllers
{
    public class AccionesController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        
        //
        // GET: /Acciones/
        
        public ActionResult Index(string sortOrder,string sortType, FormCollection formData)
        {
            ViewData["NoDataFound"] = "No tiene acciones pendientes.";
            ViewData["Title"] = "Acciones Pendientes";
            
            //Cargo los datos de delegaciones y responsables.
            var delegaciones =  db.aspnet_Delegaciones.Select(d => new { d.DelegacionID, d.Descripcion });
            ViewData["Delegaciones"] = delegaciones.AsEnumerable().OrderBy(d=>d.Descripcion).ToDictionary(d=>d.DelegacionID, d=>d.Descripcion);

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

            ViewData["Responsables"] = usuarios.AsEnumerable().OrderBy(u => u.UserName).Distinct().ToDictionary(u => u.UserId, u => u.UserName);

            //Cargo las acciones
            IEnumerable<aspnet_Acciones> Accion = getAcciones(sortOrder, sortType, ViewData["Title"].ToString(), formData);

            //Cargo los datos para los Checks del buscador
            var origenes = db.aspnet_Origenes.Select(o => new {o.OrigenID, o.Descripcion});
            var departamentos = db.aspnet_Departamentos.Select(d => new { d.DepartamentoID, d.Descripcion});

            CheckBoxModel chkOrigenes = new CheckBoxModel(origenes.AsEnumerable().OrderBy(o => o.Descripcion).ToDictionary(o => o.OrigenID.ToString(), o => o.Descripcion), new List<string>());
            ViewData["chkOrigenes"] = chkOrigenes;
            CheckBoxModel chkDepartamentos = new CheckBoxModel(departamentos.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DepartamentoID.ToString(), d => d.Descripcion), new List<string>());
            ViewData["chkDepartamentos"] = chkDepartamentos;
            CheckBoxModel chkDelegaciones = new CheckBoxModel(delegaciones.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DelegacionID.ToString(), d => d.Descripcion), new List<string>());
            ViewData["chkDelegaciones"] = chkDelegaciones;
            CheckBoxModel chkResponsables = new CheckBoxModel(usuarios.AsEnumerable().OrderBy(u => u.UserName).Distinct().ToDictionary(u => u.UserId.ToString(), u => u.UserName), new List<string>());
            ViewData["chkResponsables"] = chkResponsables;

            return View(Accion);
        }

        public ActionResult LimpiarFiltros(string tab)
        {
            //Nos guardamos la variable de sesión AplicacionesGM para poder restaurarla
            Array Aplicaciones = (Array)Session["AplicacionesGM"];

            //Borramos la session
            Session.Clear();

            //Restauramos AplicacionesGM
            Session["AplicacionesGM"] = Aplicaciones;

            if (tab == "Acciones Pendientes")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Historico");
            }
        }

        //Acciones/Historico
        public ActionResult Historico(string sortOrder, string sortType, FormCollection formData)
        {
            ViewData["NoDataFound"] = "No tiene acciones en finalizadas.";
            ViewData["Title"] = "Acciones finalizadas";
            IEnumerable<aspnet_Acciones> Accion = getAcciones(sortOrder, sortType, ViewData["Title"].ToString(), formData);

            //Cargo los datos de delegaciones y responsables.
            var delegaciones = db.aspnet_Delegaciones.Select(d => new { d.DelegacionID, d.Descripcion });
            ViewData["Delegaciones"] = delegaciones.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DelegacionID, d => d.Descripcion);

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

            ViewData["Responsables"] = usuarios.AsEnumerable().OrderBy(u => u.UserName).Distinct().ToDictionary(u => u.UserId, u => u.UserName);

            //Cargo los datos para los Checks del buscador
            var origenes = db.aspnet_Origenes.Select(o => new { o.OrigenID, o.Descripcion });
            var departamentos = db.aspnet_Departamentos.Select(d => new { d.DepartamentoID, d.Descripcion });

            CheckBoxModel chkOrigenes = new CheckBoxModel(origenes.AsEnumerable().OrderBy(o => o.Descripcion).ToDictionary(o => o.OrigenID.ToString(), o => o.Descripcion), new List<string>());
            ViewData["chkOrigenes"] = chkOrigenes;
            CheckBoxModel chkDepartamentos = new CheckBoxModel(departamentos.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DepartamentoID.ToString(), d => d.Descripcion), new List<string>());
            ViewData["chkDepartamentos"] = chkDepartamentos;
            CheckBoxModel chkDelegaciones = new CheckBoxModel(delegaciones.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DelegacionID.ToString(), d => d.Descripcion), new List<string>());
            ViewData["chkDelegaciones"] = chkDelegaciones;
            CheckBoxModel chkResponsables = new CheckBoxModel(usuarios.AsEnumerable().OrderBy(u => u.UserName).Distinct().ToDictionary(u => u.UserId.ToString(), u => u.UserName), new List<string>());
            ViewData["chkResponsables"] = chkResponsables;

            return View(Accion);
        }
       
        //
        // GET: /Acciones/Create

        public ActionResult Create()
        {
            var dptos = db.aspnet_Departamentos.Select(d => new { d.DepartamentoID, d.Descripcion });
            MultiSelectList list = new MultiSelectList(dptos.AsEnumerable(), "DepartamentoID", "Descripcion");
            ViewData["DepartamentoID"] = list;

            var origenes = db.aspnet_Origenes.Select(o => new { o.OrigenID, o.Descripcion });
            list = new MultiSelectList(origenes.AsEnumerable(), "OrigenID", "Descripcion");
            ViewData["OrigenID"] = list;

            var delegaciones = db.aspnet_Delegaciones.Select(d => new { d.DelegacionID, d.Descripcion });
            CheckBoxModel chkDelegaciones = new CheckBoxModel(delegaciones.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DelegacionID.ToString(), d => d.Descripcion), new List<string>());
            ViewData["chkDelegaciones"] = chkDelegaciones;

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
            CheckBoxModel chkResponsables = new CheckBoxModel(usuarios.AsEnumerable().OrderBy(u => u.UserName).Distinct().ToDictionary(u => u.UserId.ToString(), u => u.UserName), new List<string>());
            ViewData["chkResponsables"] = chkResponsables;

            return View();
        }

        //
        // POST: /Acciones/Create

        [HttpPost]
        public ActionResult Create(FormCollection formData)
        {
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {

                    aspnet_Acciones newAccion = new aspnet_Acciones();
                    
                    //Asignamos los valores al abojeto
                    newAccion.OrigenID=Convert.ToInt32(formData["OrigenID"].ToString());
                    newAccion.DepartamentoID = Convert.ToInt32(formData["DepartamentoID"].ToString());
                    newAccion.Delegaciones = formData["arrDelegaciones"].Replace("false,", "").Replace(",false", "");
                    newAccion.Responsables = formData["arrResponsables"].Replace("false,", "").Replace(",false", "");
                    if (formData["FechaInicio"].ToString().Length > 0)
                    {
                        newAccion.FechaInicio = Convert.ToDateTime(formData["FechaInicio"].ToString());
                    }
                    if (formData["FechaFinPrev"].ToString().Length > 0)
                    {
                        newAccion.FechaFinPrev = Convert.ToDateTime(formData["FechaFinPrev"].ToString());
                    }
                    if (formData["FechaFinReal"].ToString().Length > 0)
                    {
                        newAccion.FechaFinReal  = Convert.ToDateTime(formData["FechaFinReal"].ToString());
                    }
                    if (formData["FechaSeguimiento"].ToString().Length > 0)
                    {
                        newAccion.FechaSeguimiento = Convert.ToDateTime(formData["FechaSeguimiento"].ToString());
                    }
                    newAccion.Accion = formData["Accion"].ToString();
                    newAccion.Objetivo = formData["Objetivo"].ToString();
                    newAccion.Resultado = formData["Resultado"].ToString();
                        
                    db.AddToaspnet_Acciones(newAccion);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(formData);
                }
                
            }
            catch
            {
                return View(formData);
            }
        }
        
        //
        // GET: /Acciones/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_Acciones modified = db.aspnet_Acciones.FirstOrDefault(a => a.AccionesID == id);
            var dptos = db.aspnet_Departamentos.Select(d => new { d.DepartamentoID, d.Descripcion });
            SelectList list = new SelectList(dptos.AsEnumerable(), "DepartamentoID", "Descripcion",modified.DepartamentoID);
            ViewData["DepartamentoID"] = list;

            var origenes = db.aspnet_Origenes.Select(o => new { o.OrigenID, o.Descripcion });
            list = new SelectList(origenes.AsEnumerable(), "OrigenID", "Descripcion",modified.OrigenID);
            ViewData["OrigenID"] = list;
            
            var delegaciones = db.aspnet_Delegaciones.Select(d => new { d.DelegacionID, d.Descripcion });
            CheckBoxModel chkDelegaciones=new CheckBoxModel(delegaciones.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DelegacionID.ToString(), d => d.Descripcion),new List<string>());
            ViewData["chkDelegaciones"] = chkDelegaciones;

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
            CheckBoxModel chkResponsables = new CheckBoxModel(usuarios.AsEnumerable().Distinct().OrderBy(u => u.UserName).ToDictionary(u => u.UserId.ToString(), u => u.UserName),new List<string>());
            ViewData["chkResponsables"] =chkResponsables;

            
            return View(modified);
        }

        //
        // POST: /Acciones/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Acciones modified, FormCollection formData)
        {
            try
            {
                // TODO: Add update logic here
                
                modified.Delegaciones = formData["arrDelegaciones"].Replace("false,", "").Replace(",false", "").Replace("false","");
                modified.Responsables = formData["arrResponsables"].Replace("false,", "").Replace(",false", "").Replace("false", "");
                
                db.aspnet_Acciones.Attach(modified);
                db.ObjectStateManager.ChangeObjectState(modified, System.Data.EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Acciones/Delete/5
 
        public ActionResult Delete(int id)
        {
            aspnet_Acciones deleted = db.aspnet_Acciones.FirstOrDefault(a => a.AccionesID == id);
            var dptos = db.aspnet_Departamentos.Select(d => new { d.DepartamentoID, d.Descripcion });
            SelectList list = new SelectList(dptos.AsEnumerable(), "DepartamentoID", "Descripcion", deleted.DepartamentoID);
            ViewData["DepartamentoID"] = list;

            var origenes = db.aspnet_Origenes.Select(o => new { o.OrigenID, o.Descripcion });
            list = new SelectList(origenes.AsEnumerable(), "OrigenID", "Descripcion", deleted.OrigenID);
            ViewData["OrigenID"] = list;

            var delegaciones = db.aspnet_Delegaciones.Select(d => new { d.DelegacionID, d.Descripcion });
            CheckBoxModel chkDelegaciones = new CheckBoxModel(delegaciones.AsEnumerable().OrderBy(d => d.Descripcion).ToDictionary(d => d.DelegacionID.ToString(), d => d.Descripcion), new List<string>());
            ViewData["chkDelegaciones"] = chkDelegaciones;

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
            CheckBoxModel chkResponsables = new CheckBoxModel(usuarios.AsEnumerable().OrderBy(u => u.UserName).ToDictionary(u => u.UserId.ToString(), u => u.UserName), new List<string>());
            ViewData["chkResponsables"] = chkResponsables;

            return View(deleted);
        }

        //
        // POST: /Acciones/Delete/5

        [HttpPost]
        public ActionResult Delete(aspnet_Acciones deleted)
        {
            try
            {
                // TODO: Add delete logic here
                db.aspnet_Acciones.Attach(deleted);
                db.ObjectStateManager.ChangeObjectState(deleted, System.Data.EntityState.Deleted);
                db.aspnet_Acciones.DeleteObject(deleted);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(deleted);
            }
        }

        public void ExportAccionesListToExcel(string tab)
        {
            var grid = new System.Web.UI.WebControls.GridView();
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

            IEnumerable<aspnet_Acciones> acciones = getAcciones("", "", tab, new FormCollection());

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
            
            Dictionary<Guid,string> dicResponsables = usuarios.AsEnumerable().OrderBy(u => u.UserName).Distinct().ToDictionary(u => u.UserId, u => u.UserName); 
            
            List<aspnet_Acciones> dt = acciones.ToList();

            foreach (var accion in dt)
            {
                DataRow dr = dtResult.NewRow();
                dr["Origen"] = accion.aspnet_Origenes.Descripcion;
                dr["Departamento"] = accion.aspnet_Departamentos.Descripcion;
                
                //Obtengo las descripciones de las delegaciones
                string strDelegaciones = "";
                foreach (string delegacion in accion.Delegaciones.ToString().Replace("false,", "").Replace(",false", "").Replace("false", "").Split(',').ToArray<string>())
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
                foreach (string responsable in accion.Responsables.ToString().Replace("false,", "").Replace(",false", "").Replace("false", "").Split(',').ToArray<string>())
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

            Response.End();

        }

        private IEnumerable<aspnet_Acciones> getAcciones(string sortOrder, string sortType, string tab, FormCollection formData)
        {
            //Gestiona la ordenación a aplicar
            Session["sortOrder"] = string.IsNullOrEmpty(sortOrder) ? "Origen" : sortOrder;
            Session["sortType"] = string.IsNullOrEmpty(sortType) ? "asc" : sortType;

            string currentUserId = Membership.GetUser(User.Identity.Name).ProviderUserKey.ToString();
            
            IEnumerable<aspnet_Acciones> acciones = from a in db.aspnet_Acciones.Include("aspnet_departamentos").Include("aspnet_Origenes").AsEnumerable() select a;

            if (tab == "Acciones Pendientes")
            {
                acciones = acciones.Where(a => a.FechaFinReal == null);
            }
            else //En Historico
            {
                acciones = acciones.Where(a => a.FechaFinReal != null);
            }
            
            if (User.IsInRole("Responsable"))
            {
                acciones = acciones.Where(a => a.Responsables.Contains(currentUserId));
            }
            else if (User.IsInRole("Jefe de Departamento"))
            {
                ProfileBase profile = ProfileBase.Create(User.Identity.Name);

                if (profile["Subordinados"].ToString() != "")
                {
                    acciones = acciones.Where(a => a.Responsables.Split(',').Intersect(profile["Subordinados"].ToString().Split(',')).Any());
                }
                if (profile["Origenes"].ToString() != "")
                {
                    acciones = acciones.Where(a => profile["Origenes"].ToString().Split(',').Contains(a.OrigenID.ToString()) || a.Responsables.Split(',').Contains(currentUserId));
                }
            }

            //Aplicamos posibles filtros que podemos recibir en el formData
            
            //Actualizamos las variables de sesion
            if (formData["fInicioDesde"] != null && formData["fInicioDesde"].ToString()!="")
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
            }

            return acciones;
        }


    }
}
