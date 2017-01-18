using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrmBlog.Filters;
using FrmBlog.Repositorys;
using FrmBlog.Models;
using FrmBlog.ViewData;
using FrmBlog.ModelData;
namespace FrmBlog.Controllers
{
    public class EtiketController : BaseController
    {
        //
        // GET: /Etiket/
        RepositoryQuestion _repQues;
        RepositoryTag _repoTag;
        public EtiketController() {
            _repQues = new RepositoryQuestion("", DbType.SqLite);
            _repoTag = new RepositoryTag("", DbType.SqLite);
        }
        public ActionResult Tag(string etiket,int?page)
        {
            FrmViewData frm = new FrmViewData();
            int totalCount;
            frm.Questions = _repQues.GetListByTagName(etiket, PagerCount(), page ?? 1, out totalCount);
            PaginatedList pager = new PaginatedList((page ?? 1), PagerCount(), totalCount);
            ViewData["tag"] = etiket;
            return View(frm.WithPaging(pager).WithTags(_repoTag.GetAll()));
        }
        public ActionResult Etiketler(int? page,string q)
        {
            FrmViewData frm = new FrmViewData();
            int totalCount;
            int pageCount = 40;
            List<TagInfo>liste = _repoTag.GetTagInfoByQuery(pageCount, page ?? 1,q, out totalCount);
            PaginatedList pager = new PaginatedList((page ?? 1), pageCount, totalCount);
            ViewBag.pager = pager;
            ViewData["queryData"] = q;
            return View(liste);
        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
