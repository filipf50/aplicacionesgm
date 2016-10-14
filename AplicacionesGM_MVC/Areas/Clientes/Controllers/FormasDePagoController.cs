using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AplicacionesGM_MVC.Models;
using AplicacionesGM_MVC.Areas.Clientes.Models;
using AplicacionesGM_MVC.Areas.Clientes.Models.DSas400TableAdapters;

namespace AplicacionesGM_MVC.Areas.Clientes.Controllers
{
    public class FormasDePagoController : Controller
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        

        //
        // GET: /Clientes/FormasDePago/
        
        public ActionResult Index()
        {
            FormasDePagoModel objFPago = new FormasDePagoModel();

            ViewData["NoDataFound"] = "No hay formas de pago.";
            ViewData["Title"] = "Formas de Pago";
            return View(objFPago.getFormasDePago());
        }

        //
        // GET: /Clientes/FormasDePago/Edit/5
 
        public ActionResult Edit(int id)
        {
            FormasDePagoModel objFPago = new FormasDePagoModel();

            return View(objFPago.getFormaDePagoById(id));
        }

        //
        // POST: /Clientes/FormasDePago/Edit/5

        [HttpPost]
        public ActionResult Edit(FormasDePagoModel modified)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    //Traspasamos los datos de la clase intermedia al objeto de SQL-Server
                    aspnet_FormasDePago dtFPago = new aspnet_FormasDePago();

                    dtFPago.QSID = modified.ID;
                    dtFPago.Visible = modified.Visible;
                    dtFPago.DisponibleParaExposicion = modified.DisponibleExposicion;
                    dtFPago.RequiereDocSEPA = modified.EsSEPA;
                    dtFPago.DtoPP = Convert.ToDecimal(modified.DtoPP);
                    dtFPago.RecargoFinanciero = Convert.ToDecimal(modified.RecargoFinanciero);

                    db.aspnet_FormasDePago.Attach(dtFPago);
                    db.ObjectStateManager.ChangeObjectState(dtFPago, System.Data.EntityState.Modified);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                return View(modified);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message + " " + ex.InnerException.ToString());
                return View(modified);
            }
        }

    }
}
