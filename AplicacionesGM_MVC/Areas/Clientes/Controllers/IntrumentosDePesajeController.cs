using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class InstrumentosDePesajeController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/InstrumentosDePesaje/

        public ActionResult Index()
        {
            var instrumentosDePesaje = db.aspnet_InstrumentosDePesaje.OrderBy(t => t.ID);
            return View(instrumentosDePesaje);
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
        public ActionResult Create(aspnet_InstrumentosDePesaje newInstrumentosDePesaje)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_InstrumentosDePesaje(newInstrumentosDePesaje);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newInstrumentosDePesaje);
                }
            }
            catch
            {
                return View(newInstrumentosDePesaje);
            }
        }
        
        //
        // GET: /Clientes/InstrumentosDePesaje/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_InstrumentosDePesaje instrumentosDePesaje = db.aspnet_InstrumentosDePesaje.FirstOrDefault(item => item.ID == id);
            return View(instrumentosDePesaje);
        }

        //
        // POST: /Clientes/InstrumentosDePesaje/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_InstrumentosDePesaje modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_InstrumentosDePesaje.Attach(modified);
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
            aspnet_InstrumentosDePesaje instrumentosDePesaje = db.aspnet_InstrumentosDePesaje.FirstOrDefault(tc => tc.ID == id);
            return View(instrumentosDePesaje);
        }

        //
        // POST: /Clientes/InstrumentosDePesaje/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_InstrumentosDePesaje deleted = db.aspnet_InstrumentosDePesaje.SingleOrDefault(tc => tc.ID == id);
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
