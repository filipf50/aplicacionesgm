using System.Web.Mvc;

namespace AplicacionesGM_MVC.Areas.Reuniones
{
    public class ReunionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reuniones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Reuniones_default",
                "Reuniones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
