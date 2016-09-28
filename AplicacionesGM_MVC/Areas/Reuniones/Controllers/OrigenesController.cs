using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Reuniones.Controllers
{
    public class OrigenesController : Controller
    {
        //
        // GET: /Origenes/
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        public ActionResult Index()
        {
            var origenes = db.aspnet_Origenes;
            return View(origenes);
        }

        //
        // GET: /Origenes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Origenes/Create

        [HttpPost]
        public ActionResult Create(aspnet_Origenes newOrigen)
        {
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {
                    db.AddToaspnet_Origenes(newOrigen);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newOrigen);
                }
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Origenes/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_Origenes origen = db.aspnet_Origenes.FirstOrDefault(item => item.OrigenID == id);
            return View(origen);
        }

        //
        // POST: /Origenes/Edit/5

        
        [HttpPost]
        public ActionResult Edit(aspnet_Origenes modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_Origenes.Attach(modified);
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
        // GET: /Origenes/Delete/5
 
        public ActionResult Delete(int id)
        {
            aspnet_Origenes origen = db.aspnet_Origenes.FirstOrDefault(item => item.OrigenID == id);
            return View(origen);
        }

        //
        // POST: /Origenes/Delete/5

        [HttpPost]
        public ActionResult Delete(int id,FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var itemRemove=db.aspnet_Origenes.SingleOrDefault(o => o.OrigenID == id);
                db.DeleteObject(itemRemove);
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
