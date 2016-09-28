using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Reuniones.Controllers
{
    public class DelegacionesController : Controller
    {
        //
        // GET: /Delegaciones/
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();

        public ActionResult Index()
        {
            var delegaciones = db.aspnet_Delegaciones;
            return View(delegaciones);
        }

        //
        // GET: /Delegaciones/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Delegaciones/Create

        [HttpPost]
        public ActionResult Create(aspnet_Delegaciones newDelegacion)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_Delegaciones(newDelegacion);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newDelegacion);
                }
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Delegaciones/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_Delegaciones delegacion = db.aspnet_Delegaciones.FirstOrDefault(item => item.DelegacionID == id);
            return View(delegacion);
        }

        //
        // POST: /Delegaciones/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Delegaciones modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_Delegaciones.Attach(modified);
                db.ObjectStateManager.ChangeObjectState(modified,System.Data.EntityState.Modified);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Delegaciones/Delete/5
 
        public ActionResult Delete(int id)
        {
            aspnet_Delegaciones delegacion = db.aspnet_Delegaciones.FirstOrDefault(d => d.DelegacionID == id);
            return View(delegacion);
        }

        //
        // POST: /Delegaciones/Delete/5

        [HttpPost]
        public ActionResult Delete(int id,FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_Delegaciones deleted = db.aspnet_Delegaciones.SingleOrDefault(d => d.DelegacionID == id);
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
