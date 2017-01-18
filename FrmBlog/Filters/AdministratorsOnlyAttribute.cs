using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrmBlog.Filters
{
    public class AdministratorsOnlyAttribute : AuthorizeAttribute
    {
        public AdministratorsOnlyAttribute()
        {
            Roles = "Administrator";
            Order = 1; //Must come AFTER AuthenticateAttribute
        }
    }
}