using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class RequerimientosDeCalidadController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/InstrumentosDePesaje/

        public ActionResult Index()
        {
            var requerimientos = db.aspnet_RequerimientosDeCalidad.OrderBy(t => t.ID);
            return View(requerimientos);
        }

        //
        // GET: /Clientes/InstrumentosDePesaje/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Clientes/InstrumentosDePesaje/Create

        [HttpPost]
        public ActionResult Create(aspnet_RequerimientosDeCalidad newRequerimiento)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_RequerimientosDeCalidad(newRequerimiento);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newRequerimiento);
                }
            }
            catch
            {
                return View(newRequerimiento);
            }
        }
        
        //
        // GET: /Clientes/InstrumentosDePesaje/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_RequerimientosDeCalidad requerimientos = db.aspnet_RequerimientosDeCalidad.FirstOrDefault(item => item.ID == id);
            return View(requerimientos);
        }

        //
        // POST: /Clientes/InstrumentosDePesaje/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_RequerimientosDeCalidad modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_RequerimientosDeCalidad.Attach(modified);
                db.ObjectStateManager.ChangeObjectState(modified, System.Data.EntityState.Modified);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(modified);
            }
        }

        //
        // GET: /Clientes/InstrumentosDePesaje/Delete/5
 
        public ActionResult Delete(int id)
        {
            aspnet_RequerimientosDeCalidad requerimientos = db.aspnet_RequerimientosDeCalidad.FirstOrDefault(tc => tc.ID == id);
            return View(requerimientos);
        }

        //
        // POST: /Clientes/InstrumentosDePesaje/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_RequerimientosDeCalidad deleted = db.aspnet_RequerimientosDeCalidad.SingleOrDefault(tc => tc.ID == id);
                db.DeleteObject(deleted);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
