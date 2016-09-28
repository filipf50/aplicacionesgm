using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class CausasController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/InstrumentosDePesaje/

        public ActionResult Index()
        {
            var causas = db.aspnet_Causas.OrderBy(t => t.ID);
            return View(causas);
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
        public ActionResult Create(aspnet_Causas newCausa)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_Causas(newCausa);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newCausa);
                }
            }
            catch
            {
                return View(newCausa);
            }
        }
        
        //
        // GET: /Clientes/InstrumentosDePesaje/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_Causas causa = db.aspnet_Causas.FirstOrDefault(item => item.ID == id);
            return View(causa);
        }

        //
        // POST: /Clientes/InstrumentosDePesaje/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Causas modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_Causas.Attach(modified);
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
            aspnet_Causas causa = db.aspnet_Causas.FirstOrDefault(tc => tc.ID == id);
            return View(causa);
        }

        //
        // POST: /Clientes/InstrumentosDePesaje/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_Causas deleted = db.aspnet_Causas.SingleOrDefault(tc => tc.ID == id);
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
