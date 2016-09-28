using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class TiposDeclienteController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/Empresas/

        public ActionResult Index()
        {
            var tiposDeCliente = db.aspnet_TiposDeCliente.OrderBy(t=>t.OrdenDeVisualizacion);
            return View(tiposDeCliente);
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
        public ActionResult Create(aspnet_TiposDeCliente newTipoDecliente)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_TiposDeCliente(newTipoDecliente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newTipoDecliente);
                }
            }
            catch
            {
                return View(newTipoDecliente);
            }
        }
        
        //
        // GET: /Clientes/Empresas/Edit/5
 
        public ActionResult Edit(string id)
        {
            aspnet_TiposDeCliente tipoDecliente = db.aspnet_TiposDeCliente.FirstOrDefault(item => item.ID == id);
            return View(tipoDecliente);
        }

        //
        // POST: /Clientes/Empresas/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_TiposDeCliente modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_TiposDeCliente.Attach(modified);
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
            aspnet_TiposDeCliente tipoDecliente = db.aspnet_TiposDeCliente.FirstOrDefault(tc => tc.ID == id);
            return View(tipoDecliente);
        }

        //
        // POST: /Clientes/Empresas/Delete/5

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_TiposDeCliente deleted = db.aspnet_TiposDeCliente.SingleOrDefault(tc => tc.ID == id);
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
