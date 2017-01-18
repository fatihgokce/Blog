using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using FrmBlog.Filters;
using System.Security.Principal;
using FrmBlog.Models;
using FrmBlog.Repositorys;
namespace FrmBlog
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());        
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
      "Sor",
     "soru/sor",
     new { controller = "Soru", action = "Sor" });
            routes.MapRoute(
       "Detay",
      "soru/{id}/{title}",
      new { controller = "Soru", action = "Detay", id = "", title = "", captchaValid="" }
      , new { id = (@"^\d+$") });
       routes.MapRoute(
            "Etiket",
            "Etiket/{etiket}",
      new { controller = "Etiket", action = "Tag", etiket = "" }
       //, new { etiket = @"[^\w\s]" }
          );
       routes.MapRoute(
      "EtiketListe",
      "Etiketler/{page}",
new { controller = "Etiket", action = "Etiketler", page = UrlParameter.Optional }
           //, new { etiket = @"[^\w\s]" }
    );
       routes.MapRoute(
           "Default", // Route name
           "{controller}/{action}/{id}", // URL with parameters
           new { controller = "Home", action = "Index",id=UrlParameter.Optional } // Parameter defaults
        
       );
       routes.MapRoute(
                "Root",
                "",
                new { controller = "Home", action = "Index", id = "" });
    
            routes.MapRoute(
               "OpenIdDiscover",
               "Auth/Discover");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new AuthenticateAttribute());
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.DefaultNamespaces.Add("FrmBlog.Controllers");
        }
        //public override void Init()
        //{

        //    this.PostAuthenticateRequest += new EventHandler(MvcApplication_PostAuthenticateRequest);
        //    base.Init();
        //}
        //void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //    HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie != null)
        //    {
        //        string encTicket = authCookie.Value;
        //        if (!String.IsNullOrEmpty(encTicket))
        //        {
        //            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encTicket);
        //            //NerdIdentity id = new NerdIdentity(ticket);
        //            string[] roles = { "" };
        //            var user = new RepositoryUser(HttpContext.Current.Server.MapPath("~/App_Data/FrmBlog.db"), DbType.SqLite).GetUserByEmail(ticket.UserData);
        //            if (user != null)
        //            {
        //                GenericPrincipal prin = new GenericPrincipal(user.Identity, null);
        //                HttpContext.Current.User = prin;
        //            }
        //        }
        //    }
        //}
    }
}