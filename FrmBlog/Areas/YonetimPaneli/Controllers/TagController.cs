using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrmBlog.Models;
using FrmBlog.Repositorys;
using FrmBlog.Helpers;
namespace FrmBlog.Areas.YonetimPaneli.Controllers
{
    
    public class TagController : YonetimBaseController
    {
        //
        // GET: /YonetimPaneli/Tag/
        RepositoryTag _repTa;
        public TagController()
        {
            _repTa = new RepositoryTag("",DbType.SqLite);
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]       
        public ActionResult Index(FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Request["TagId"]) && Request["TagId"] != "0")
                {                
                    Tag tag = new Tag();
                    tag.TagName = Request["TagName"].Trim();
                    _repTa.Insert(tag);
                }
                else
                {
                    Tag tag = new Tag();
                    tag.TagName = Request["TagName"].Trim();
                    tag.TagId = Request["TagId"].ParseStruct<int>(x => int.Parse(x));
                    _repTa.Update(tag);                   
                }
            }
            return View();
        }
        public ActionResult Sil(int id)
        {
            Tag tag = new Tag();
            tag.TagId = id;
            _repTa.Delete(tag);
            return RedirectToAction("Index");
        }

    }
}
