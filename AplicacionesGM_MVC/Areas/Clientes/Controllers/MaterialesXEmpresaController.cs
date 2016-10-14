using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class MaterialesXEmpresaController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        
        //
        // GET: /Clientes/MaterialesXEmpresa/

        public ActionResult Index(string idEmpresa)
        {
            IEnumerable<aspnet_MaterialesEmpresa> materiales = from m in db.aspnet_MaterialesEmpresa.AsEnumerable() select m;
            
            materiales = materiales.Where(m => m.QSIDEmpresa == idEmpresa);

            ViewData["Empresa"]=idEmpresa;

            return View(materiales);            
        }

        //
        // GET: /Clientes/MaterialesXEmpresa/Create

        public ActionResult Create(string idEmpresa)
        {
            ViewData["Empresa"] = idEmpresa;
            return View();
        }

        //
        // POST: /Clientes/MaterialesXEmpresa/Create

        [HttpPost]
        public ActionResult Create(aspnet_MaterialesEmpresa newMaterial)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_MaterialesEmpresa(newMaterial);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idEmpresa = newMaterial.QSIDEmpresa });
                }
                else
                {
                    return View(newMaterial);
                }
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Clientes/MaterialesXEmpresa/Edit/5

        public ActionResult Edit(int id)
        {
            aspnet_MaterialesEmpresa material = db.aspnet_MaterialesEmpresa.FirstOrDefault(item => item.ID == id);
            return View(material);
        }

        //
        // POST: /Clientes/MaterialesXEmpresa/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_MaterialesEmpresa modified)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add update logic here
                    db.aspnet_MaterialesEmpresa.Attach(modified);
                    db.ObjectStateManager.ChangeObjectState(modified, System.Data.EntityState.Modified);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { idEmpresa = modified.QSIDEmpresa });
                }

                return View(modified);
            }
            catch
            {
                ModelState.AddModelError("", "El nombre no puede superar los 50 caracteres");
                return View(modified);
            }
        }

        //
        // GET: /Clientes/MaterialesXEmpresa/Delete/5

        public ActionResult Delete(int id)
        {
            aspnet_MaterialesEmpresa material = db.aspnet_MaterialesEmpresa.FirstOrDefault(e => e.ID == id);
            return View(material);
        }

        //
        // POST: /Clientes/MaterialesXEmpresa/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_MaterialesEmpresa deleted = db.aspnet_MaterialesEmpresa.SingleOrDefault(e => e.ID == id);
                string strQSIDEmpresa = deleted.QSIDEmpresa;

                db.DeleteObject(deleted);
                db.SaveChanges();
                return RedirectToAction("Index", new { idEmpresa = strQSIDEmpresa});
            }
            catch
            {
                return View();
            }
        }

    }
}
