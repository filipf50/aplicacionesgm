using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class TiposDeVehiculoController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/TiposDeVehiculo/

        public ActionResult Index()
        {
            var tiposDeVehiculo = db.aspnet_TiposDeVehiculo.OrderBy(t=>t.ID);
            return View(tiposDeVehiculo);
        }

        //
        // GET: /Clientes/TiposDeVehiculo/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Clientes/TiposDeVehiculo/Create

        [HttpPost]
        public ActionResult Create(aspnet_TiposDeVehiculo newTipoDeVehiculo)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_TiposDeVehiculo(newTipoDeVehiculo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newTipoDeVehiculo);
                }
            }
            catch
            {
                return View(newTipoDeVehiculo);
            }
        }
        
        //
        // GET: /Clientes/TiposDeVehiculo/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_TiposDeVehiculo tipoDeVehiculo = db.aspnet_TiposDeVehiculo.FirstOrDefault(item => item.ID == id);
            return View(tipoDeVehiculo);
        }

        //
        // POST: /Clientes/TiposDeVehiculo/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_TiposDeVehiculo modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_TiposDeVehiculo.Attach(modified);
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
        // GET: /Clientes/TiposDeVehiculo/Delete/5
 
        public ActionResult Delete(int id)
        {
            aspnet_TiposDeVehiculo tipoDeVehiculo = db.aspnet_TiposDeVehiculo.FirstOrDefault(tc => tc.ID == id);
            return View(tipoDeVehiculo);
        }

        //
        // POST: /Clientes/TiposDeVehiculo/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_TiposDeVehiculo deleted = db.aspnet_TiposDeVehiculo.SingleOrDefault(tc => tc.ID == id);
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
