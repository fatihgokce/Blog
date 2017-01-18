using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using FrmBlog.Models;
using System.Web.Mvc;
using FrmBlog.Repositorys;
using FrmBlog.Services;
using System.Security.Principal;
namespace FrmBlog.Filters
{
    public class AuthenticateAttribute : FilterUsingAttribute
    {
        public AuthenticateAttribute()
            : base(typeof(AuthenticateFilter))
        {
            Order = 0;
        }
    }
    public class AuthenticateFilter :IAuthorizationFilter
    {
        private RepositoryUser userRepo;
        private IFormsAuthentication formsAuth;

        public AuthenticateFilter()
        {
           // Order = 0;
            this.userRepo = new RepositoryUser(HttpContext.Current.Server.MapPath("~/App_Data/FrmBlog.db"),DbType.SqLite);
            this.formsAuth = new FormsAuthenticationWrapper();
        }
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{

        //    if (httpContext.Request.IsAuthenticated)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        public  void OnAuthorization(AuthorizationContext filterContext)
        {
            var context = filterContext.HttpContext;

            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var email = context.User.Identity.Name;
                var user = userRepo.GetUserByEmail(email);

              
                if (user == null)
                {
                    formsAuth.SignOut();
                }
                else
                {
                    GenericPrincipal gen = new GenericPrincipal(user.Identity, null);
                    AuthenticateAs(context, user);

                    return;
                }
            }

            //AuthenticateAs(context, User.Guest);
        }

        private void AuthenticateAs(HttpContextBase context, User user)
        {
            Thread.CurrentPrincipal= context.User  = user;
        }
    }
}