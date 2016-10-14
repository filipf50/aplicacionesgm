using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class MediosDeDescargaController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/MediosDeDescarga/

        public ActionResult Index()
        {
            var mediosDeDescarga = db.aspnet_MediosDeDescarga.OrderBy(t=>t.ID);
            return View(mediosDeDescarga);
        }

        //
        // GET: /Clientes/MediosDeDescarga/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Clientes/MediosDeDescarga/Create

        [HttpPost]
        public ActionResult Create(aspnet_MediosDeDescarga newMedioDeDescarga)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_MediosDeDescarga(newMedioDeDescarga);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newMedioDeDescarga);
                }
            }
            catch
            {
                return View(newMedioDeDescarga);
            }
        }
        
        //
        // GET: /Clientes/MediosDeDescarga/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_MediosDeDescarga medioDeDescarga = db.aspnet_MediosDeDescarga.FirstOrDefault(item => item.ID == id);
            return View(medioDeDescarga);
        }

        //
        // POST: /Clientes/MediosDeDescarga/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_MediosDeDescarga modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_MediosDeDescarga.Attach(modified);
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
        // GET: /Clientes/MediosDeDescarga/Delete/5
 
        public ActionResult Delete(int id)
        {
            aspnet_MediosDeDescarga medioDeDescarga = db.aspnet_MediosDeDescarga.FirstOrDefault(tc => tc.ID == id);
            return View(medioDeDescarga);
        }

        //
        // POST: /Clientes/MediosDeDescarga/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_MediosDeDescarga deleted = db.aspnet_MediosDeDescarga.SingleOrDefault(tc => tc.ID == id);
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
