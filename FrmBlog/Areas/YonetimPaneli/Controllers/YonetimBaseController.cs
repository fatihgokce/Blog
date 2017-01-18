using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrmBlog.Filters;
namespace FrmBlog.Areas.YonetimPaneli.Controllers
{
    //[AdministratorsOnly]
    //[Authorize(Roles = "Member", Order = 1)]
     [Authorize(Roles = "Administrator", Order = 1)]
    public class YonetimBaseController : Controller
    {
        //public ActionResult Index()
        //{
        //    return RedirectToAction("Index","Tag");
        //}
    }
}
