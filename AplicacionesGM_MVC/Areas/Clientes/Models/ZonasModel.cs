using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;
using AplicacionesGM_MVC.Models;
using AplicacionesGM_MVC.Areas.Clientes.Models.DSas400TableAdapters;


namespace AplicacionesGM_MVC.Areas.Clientes.Models
{
    public class ZonasModel 
    {
        [DisplayName("Cod. Zona")]
        public decimal? IDZona { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        public IEnumerable<ZonasModel> getZonas()
        {
            //Cargo los datos de las localizaciones de QS
            CLNZNTableAdapter ta = new CLNZNTableAdapter(); 

            IEnumerable<ZonasModel> dtView = (from zon in ta.GetData()
                                                       select new ZonasModel
                                                       {
                                                           IDZona = zon.ZNCDG,
                                                           Nombre = zon.ZNNBR.Trim()
                                                       });
            return dtView;
        }

        public ZonasModel getZonaByLoc(int IDMun, int IDProv, int IDPais)
        {
            RUTASTableAdapter taRU = new RUTASTableAdapter();
            ZonasModel objZona = new ZonasModel();

            var dtView = (from ru in taRU.GetDataByLoc(IDMun,IDProv,IDPais) select ru);

            if (dtView.Count() > 0)
            {
                objZona.IDZona = (decimal)dtView.ToArray()[0]["RUZON"];
                objZona.Nombre = dtView.ToArray()[0]["ZNNBR"].ToString().Trim();
            }
            else
            {
                objZona = null; 
            }

            return objZona;
        }
    }
}