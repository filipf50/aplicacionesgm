using System.Web.Mvc;

namespace AplicacionesGM_MVC.Areas.Clientes
{
    public class ClientesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Clientes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Clientes_default",
                "Clientes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
