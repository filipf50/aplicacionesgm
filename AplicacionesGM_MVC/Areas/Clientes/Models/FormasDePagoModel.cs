using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using AplicacionesGM_MVC.Models;
using AplicacionesGM_MVC.Areas.Clientes.Models;
using AplicacionesGM_MVC.Areas.Clientes.Models.DSas400TableAdapters;


namespace AplicacionesGM_MVC.Areas.Clientes.Models
{
    public class FormasDePagoModel
    {
        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        
        [DisplayName("¿Visible para la aplicación?")]
        public bool Visible { get; set; }

        [DisplayName("¿Disponible para clientes de exposición?")]
        public bool DisponibleExposicion { get; set; }

        [DisplayName("¿Requiere documentación SEPA?")]
        public bool EsSEPA { get; set; }


        [Required]
        [DisplayName("Dto. P.P.")]
        [Range(typeof(decimal),"0","99,99")]
        public decimal DtoPP { get; set; }

        [Required]
        [DisplayName("Recargo financiero")]
        [Range(typeof(decimal), "0", "99,99")]
        public decimal RecargoFinanciero { get; set; }
        

        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();

        public IEnumerable<FormasDePagoModel> getFormasDePago()
        {
            //Cargo los datos de las formas de pago de QS.
            INSFPTableAdapter ta = new INSFPTableAdapter();
            DSas400.INSFPDataTable dt = new DSas400.INSFPDataTable();
            ta.Fill(dt);

            var dtFPago = db.aspnet_FormasDePago;

            IEnumerable<FormasDePagoModel> dtView = (from F400 in dt
                                                     join FSQL in dtFPago on F400.FPCDG equals FSQL.QSID into dtLeft
                                                     from fd in dtLeft.DefaultIfEmpty()
                                                     select new FormasDePagoModel
                                                     {
                                                         ID = (int)F400.FPCDG,
                                                         Nombre = F400.FPNBR.Trim() + " " + F400.FPNBR2.Trim(),
                                                         Visible = fd == null ? false : fd.Visible,
                                                         DisponibleExposicion = fd == null ? false : fd.DisponibleParaExposicion,
                                                         EsSEPA = fd == null ? false : fd.RequiereDocSEPA,
                                                         DtoPP = fd == null ? decimal.Parse("0,00") : fd.DtoPP,
                                                         RecargoFinanciero = fd == null ? decimal.Parse("0,00") : fd.RecargoFinanciero
                                                     });
            return dtView;
        }

        public FormasDePagoModel getFormaDePagoById(int id)
        {
            //Cargo los datos de la formas de pago de QS.
            INSFPTableAdapter ta = new INSFPTableAdapter();
            DSas400.INSFPDataTable dt = new DSas400.INSFPDataTable();
            ta.FillByID(dt, id);

            //Cargo los datos adicionales almacenados en SQL-Server
            aspnet_FormasDePago dtFPago = db.aspnet_FormasDePago.FirstOrDefault(f => f.QSID == id);

            if (dtFPago == null)
            {
                //Si la forma de pago no está registrada en el SQL-Server, la creamos
                dtFPago = new aspnet_FormasDePago();
                dtFPago.QSID = id;
                db.AddToaspnet_FormasDePago(dtFPago);
                db.SaveChanges();
            }


            //Creo un objeto de la clase FormasDePagoModels
            AplicacionesGM_MVC.Areas.Clientes.Models.FormasDePagoModel objFPago = new FormasDePagoModel();

            //Asigno los valores correspondientes
            objFPago.ID = id;
            objFPago.Nombre = dt.Rows[0]["FPNBR"].ToString() + " " + dt.Rows[0]["FPNBR2"].ToString();
            objFPago.Visible = dtFPago.Visible;
            objFPago.DisponibleExposicion = dtFPago.DisponibleParaExposicion;
            objFPago.DtoPP = dtFPago.DtoPP;
            objFPago.RecargoFinanciero = dtFPago.RecargoFinanciero;
            objFPago.EsSEPA = dtFPago.RequiereDocSEPA;

            return objFPago;
        }
    }    
}