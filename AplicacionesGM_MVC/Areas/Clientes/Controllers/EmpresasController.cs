using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class EmpresasController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        //
        // GET: /Clientes/Empresas/

        public ActionResult Index()
        {
            var empresas = db.aspnet_Empresas;
            return View(empresas);
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
        public ActionResult Create(aspnet_Empresas newEmpresa)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_Empresas(newEmpresa);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newEmpresa);
                }
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Clientes/Empresas/Edit/5
 
        public ActionResult Edit(string QSid)
        {
            aspnet_Empresas empresa = db.aspnet_Empresas.FirstOrDefault(item => item.QSID == QSid);
            return View(empresa);
        }

        //
        // POST: /Clientes/Empresas/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Empresas modified)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add update logic here
                    db.aspnet_Empresas.Attach(modified);
                    db.ObjectStateManager.ChangeObjectState(modified, System.Data.EntityState.Modified);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(modified);
            }
            catch 
            {
                ModelState.AddModelError("", "El nombre no puede superar los 50 caracteres.");
                return View(modified);
            }
        }

        //
        // GET: /Clientes/Empresas/Delete/5
 
        public ActionResult Delete(string QSid)
        {
            aspnet_Empresas empresa = db.aspnet_Empresas.FirstOrDefault(e => e.QSID == QSid);
            return View(empresa);
        }

        //
        // POST: /Clientes/Empresas/Delete/5

        [HttpPost]
        public ActionResult Delete(string QSid, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_Empresas deleted = db.aspnet_Empresas.SingleOrDefault(e => e.QSID == QSid);
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
