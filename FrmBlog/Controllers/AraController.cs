using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrmBlog.Filters;
using FrmBlog.Repositorys;
using FrmBlog.Models;
using FrmBlog.ViewData;
namespace FrmBlog.Controllers
{
    public class AraController : BaseController
    {
        //
        // GET: /Ara/

        public ActionResult Index(int?page,string q)
        {
            RepositoryQuestion _repQues = new RepositoryQuestion("",DbType.SqLite);
            RepositorySetting _repoSet = new RepositorySetting("",DbType.SqLite);
            RepositoryTag _repoTag = new RepositoryTag("",DbType.SqLite);
            int totalCount;
            if(!string.IsNullOrEmpty(q))
                q = q.Replace("'", "%t%");
            FrmViewData frm = new FrmViewData();
            frm.Questions = _repQues.GetListQueryByDate(q,PagerCount(), page ?? 1, out totalCount);
            PaginatedList pager = new PaginatedList((page ?? 1), PagerCount(), totalCount);
            ViewBag.SiteName = _repoSet[SettingKey.SiteName];
            ViewData["queryData"] = q;
            return View(frm.WithPaging(pager).WithTags(_repoTag.GetAll()));
        }

    }
}
