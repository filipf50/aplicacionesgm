using System.Web.Mvc;

namespace AplicacionesGM_MVC.Areas.Proveedores
{
    public class ProveedoresAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Proveedores";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Proveedores_default",
                "Proveedores/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
