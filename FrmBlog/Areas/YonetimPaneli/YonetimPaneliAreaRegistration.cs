using System.Web.Mvc;

namespace FrmBlog.Areas.YonetimPaneli
{
    public class YonetimPaneliAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "YonetimPaneli";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "YonetimPaneli_default",
                "YonetimPaneli/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
