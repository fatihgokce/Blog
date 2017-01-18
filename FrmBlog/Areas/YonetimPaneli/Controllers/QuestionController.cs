using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrmBlog.Models;
using FrmBlog.Repositorys;
using FrmBlog.Helpers;
using FrmBlog.ViewData;
namespace FrmBlog.Areas.YonetimPaneli.Controllers
{
    public class QuestionController : YonetimBaseController
    {
        //
        // GET: /YonetimPaneli/Question/
        RepositoryQuestion _repQues;
        RepositoryAnswers _repAns;
        RepositoryQuestionTag _repQuTag;
        public QuestionController() {
            _repQues = new RepositoryQuestion("",DbType.SqLite);
            _repAns = new RepositoryAnswers("",DbType.SqLite);
            _repQuTag = new RepositoryQuestionTag("",DbType.SqLite);
        }
        public ActionResult Index(int?page)
        {
            FrmViewData frm = new FrmViewData();
            int totalCount;

            frm.Questions = _repQues.GetAllListByDate(30, page ?? 1, out totalCount);
            PaginatedList pager = new PaginatedList((page ?? 1), 30, totalCount);

            return View(frm.WithPaging(pager));
        }
        public ActionResult SoruSil(int id)
        {
            //if (Request.IsAjaxRequest())
            //{
            //    try
            //    {
            //        _repQues.BeginTransaction();
            //        _repQuTag.Delete(id);
            //        _repAns.DeleteByQuestionId(id);
            //        _repQues.DeleteByQuestionId(id);
            //        _repQues.CommitTransaction();

            //        return Json(new JsonResponse
            //        {
            //            Success = true
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        return Json(new JsonResponse
            //        {
            //            Message = ex.Message,
            //            Success = false
            //        });
            //    }
            //}
            ConnectionManager.Instance.BeginTransaction();
            _repQuTag.Delete(id);
            _repAns.DeleteByQuestionId(id);
            _repQues.DeleteByQuestionId(id);
            ConnectionManager.Instance.CommitTransaction();
            return RedirectToAction("Index", new { page=""});
        }
        public ActionResult SoruyuYayinla(int id)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    _repQues.SoruDurumunuDegistir(id, State.Yayinda);

                    return Json(new JsonResponse
                    {
                        Success = true
                    });
                }
                catch (Exception ex)
                {
                    return Json(new JsonResponse
                    {
                        Message = ex.Message,
                        Success = false
                    });
                }
            }
            return View();
        }

    }
}
