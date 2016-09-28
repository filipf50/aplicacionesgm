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
    public class LocalizacionesModel
    {
        [DisplayName("CP")]
        public decimal? CP { get; set; }

        [DisplayName("Cod. Municipio")]
        public decimal IDMunicipio { get; set; }

        [DisplayName("Municipio")]
        public string Municipio { get; set; }

        [DisplayName("Cod. Provincia")]
        public decimal IDProvincia { get; set; }

        [DisplayName("Provincia")]
        public string Provincia { get; set; }


        [Required]
        [DisplayName("Cod. Pais")]
        public decimal IDPais { get; set; }

        [Required]
        [DisplayName("Pais")]
        public string Pais { get; set; }

        public IEnumerable<LocalizacionesModel> getLocalizaciones()
        {
            //Cargo los datos de las localizaciones de QS
            LocalizacionesTableAdapter ta = new LocalizacionesTableAdapter();
            
            IEnumerable<LocalizacionesModel> dtView = (from loc in ta.GetData()
                                                     select new LocalizacionesModel
                                                     {
                                                         CP = loc.CPCDP,
                                                         IDMunicipio=loc.MUMNC,
                                                         Municipio=loc.MUNBR.Trim(),
                                                         IDProvincia=loc.MUPRV,
                                                         Provincia=loc.PPNBR.Trim(),
                                                         IDPais=loc.NCCDG,
                                                         Pais=loc.NCNBR.Trim()
                                                     });
            return dtView;
        }
    }
}