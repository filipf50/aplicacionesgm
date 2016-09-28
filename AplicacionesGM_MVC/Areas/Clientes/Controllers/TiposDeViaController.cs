using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class TiposDeViaController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/Empresas/

        public ActionResult Index()
        {
            var tiposDeVia = db.aspnet_TiposDeVia.OrderBy(t => t.Nombre);
            return View(tiposDeVia);
        }

        //
        // GET: /Clientes/Empresas/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Clientes/Empresas/Create

        [HttpPost]
        public ActionResult Create(aspnet_TiposDeVia newTipoDeVia)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_TiposDeVia(newTipoDeVia);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newTipoDeVia);
                }
            }
            catch
            {
                return View(newTipoDeVia);
            }
        }

        //
        // GET: /Clientes/Empresas/Edit/5

        public ActionResult Edit(string id)
        {
            aspnet_TiposDeVia tipoDeVia = db.aspnet_TiposDeVia.FirstOrDefault(item => item.IDTipoVia == id);
            return View(tipoDeVia);
        }

        //
        // POST: /Clientes/Empresas/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_TiposDeVia modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_TiposDeVia.Attach(modified);
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
        // GET: /Clientes/Empresas/Delete/5

        public ActionResult Delete(string id)
        {
            aspnet_TiposDeVia tipoDeVia = db.aspnet_TiposDeVia.FirstOrDefault(tc => tc.IDTipoVia == id);
            return View(tipoDeVia);
        }

        //
        // POST: /Clientes/Empresas/Delete/5

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_TiposDeVia deleted = db.aspnet_TiposDeVia.SingleOrDefault(tc => tc.IDTipoVia == id);
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
