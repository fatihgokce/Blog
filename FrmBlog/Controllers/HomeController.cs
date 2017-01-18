using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrmBlog.Filters;
using FrmBlog.Repositorys;
using FrmBlog.Models;
using FrmBlog.ViewData;
using FrmBlog.Helpers;
namespace FrmBlog.Controllers
{

    public class HomeController : BaseController
    {
        RepositoryQuestion _repQues;
        RepositoryTag _repoTag;
        RepositorySetting _repoSet;
        RepositoryQuestionVisit _repoIst;
        RepositoryUser _repoUser;
        public HomeController() {
            _repQues = new RepositoryQuestion("",DbType.SqLite);
            _repoTag = new RepositoryTag("",DbType.SqLite);
            _repoSet = new RepositorySetting("",DbType.SqLite);
            _repoIst = new RepositoryQuestionVisit("",DbType.SqLite);
            _repoUser = new RepositoryUser("",DbType.SqLite);
        }
        public ActionResult Index(int?page)
        {
            FrmViewData frm = new FrmViewData();
            int totalCount;
            frm.Questions = _repQues.GetListByDate(PagerCount(),page ?? 1,out totalCount);
            PaginatedList pager = new PaginatedList((page ?? 1), PagerCount(), totalCount);
            ViewBag.SiteName = _repoSet[SettingKey.SiteName];
            //string str = Request.ServerVariables["HTTP_REFERER"];//Request.UrlReferrer.AbsoluteUri;//Request.ServerVariables["REQUEST_URI"];
           
            InsertPageIstatik(0,AnaSayfaId);
            return View(frm.WithPaging(pager).WithTags(_repoTag.GetAll()));
        }

      
       [Authorize(Roles = "Administrator", Order = 1)]
        public ActionResult ListeZiyaret(int? page)
        {
            FrmViewData frm = new FrmViewData();
            int totalCount;
            frm.QuestionVisits = _repoIst.GetList(75, page ?? 1, out totalCount);
            PaginatedList pager = new PaginatedList((page ?? 1), 75, totalCount);
            ViewBag.SiteName = _repoSet[SettingKey.SiteName];
          
            return View(frm.WithPaging(pager));
        }
       [Authorize(Roles = "Administrator", Order = 1)]
       public ActionResult DeleteVisitIstatik(string ip)
       {
           _repoIst.DeleteByIp(ip);
           return RedirectToAction("ListeZiyaret");
       }
       [Authorize(Roles = "Administrator", Order = 1)]
       public ActionResult ListeKullanici(int? page)
       {
           FrmViewData frm = new FrmViewData();
           int totalCount;
           frm.Users = _repoUser.GetListByDate(100, page ?? 1, out totalCount);
           PaginatedList pager = new PaginatedList((page ?? 1), 100, totalCount);
           ViewBag.SiteName = _repoSet[SettingKey.SiteName];
           return View(frm.WithPaging(pager));
       }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Ara()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Ara(string q)
        {
            return View("Index");
        }
        public ActionResult KullanimSartlari()
        {
            return View();
        }
        [HttpPost]
        public JsonResult FindNames(string name)
        {
            return Json(_repoTag.GetListLikeTagName(name.ConvertUtf8ToTr()));
        }

    }
}
