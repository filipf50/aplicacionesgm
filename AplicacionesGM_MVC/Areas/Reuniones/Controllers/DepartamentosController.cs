using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;

namespace AplicacionesGM_MVC.Areas.Reuniones.Controllers
{
    public class DepartamentosController : Controller
    {
        //
        // GET: /Departamentos/
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();


        public ActionResult Index()
        {
            var departamentos = db.aspnet_Departamentos;
            return View(departamentos);
        }

        //
        // GET: /Departamentos/Create

        public ActionResult Create()
        {
            
            return View();
        } 

        //
        // POST: /Departamentos/Create

        [HttpPost]
        public ActionResult Create(aspnet_Departamentos newDpto)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    db.AddToaspnet_Departamentos(newDpto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newDpto);
                }

            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Departamentos/Edit/5
 
        public ActionResult Edit(int id)
        {
            aspnet_Departamentos departamento = db.aspnet_Departamentos.FirstOrDefault(d => d.DepartamentoID == id);
            return View(departamento);
        }

        //
        // POST: /Departamentos/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Departamentos modified)
        {
            try
            {
                // TODO: Add update logic here
                db.aspnet_Departamentos.Attach(modified);
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
        // GET: /Departamentos/Delete/5
 
        public ActionResult Delete(int id)
        {
            aspnet_Departamentos deleted = db.aspnet_Departamentos.FirstOrDefault(d => d.DepartamentoID == id);
            return View(deleted);
        }

        //
        // POST: /Departamentos/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                aspnet_Departamentos deleted = db.aspnet_Departamentos.SingleOrDefault(d => d.DepartamentoID == id);
                db.aspnet_Departamentos.DeleteObject(deleted);
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
