using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class AseguradorasController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/Aseguradoras/

        public ActionResult Index()
        {
            var aseguradoras = db.aspnet_Aseguradoras.OrderBy(a => a.ID);
            return View(aseguradoras);
        }

        //
        // GET: /Clientes/Aseguradoras/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Clientes/Aseguradoras/Create

        [HttpPost]
        public ActionResult Create(aspnet_Aseguradoras newAseguradora)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_Aseguradoras(newAseguradora);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newAseguradora);
                }
            }
            catch
            {
                return View(newAseguradora);
            }
        }

        //
        // GET: /Clientes/Aseguradoras/Edit/5

        public ActionResult Edit(int id)
        {
            aspnet_Aseguradoras aseguradora = db.aspnet_Aseguradoras.FirstOrDefault(item => item.ID == id);
            return View(aseguradora);
        }

        //
        // POST: /Clientes/Aseguradoras/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Aseguradoras modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_Aseguradoras.Attach(modified);
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
        // GET: /Clientes/Aseguradoras/Delete/5

        public ActionResult Delete(int id)
        {
            aspnet_Aseguradoras aseguradora = db.aspnet_Aseguradoras.FirstOrDefault(a => a.ID == id);
            return View(aseguradora);
        }

        //
        // POST: /Clientes/Aseguradoras/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_Aseguradoras deleted = db.aspnet_Aseguradoras.SingleOrDefault(a => a.ID == id);
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
