using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionesGM_MVC.Models
{
    public class CheckBoxModel
    {
        public Dictionary<string,string> lstValores { get; set; }
        public List<string> arrValoresSelected { get; set; }

        public CheckBoxModel(Dictionary<string, string> lstValores, List<string> arrValoresSelected)
        {
            this.lstValores = lstValores;
            this.arrValoresSelected = arrValoresSelected;
        }
    }
}