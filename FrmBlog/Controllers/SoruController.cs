using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrmBlog.ViewData;
using FrmBlog.Repositorys;
using FrmBlog.Models;
using FrmBlog.Filters;
using System.Text.RegularExpressions;
using FrmBlog.Services;
using FrmBlog.Helpers;
using System.Collections;
using FrmBlog.Utils;
namespace FrmBlog.Controllers
{
    //[Authorize(Roles = "Administrator", Order = 1)]
    public class SoruController : BaseController
    {
        //
        // GET: /Soru/
        RepositoryAnswers _repoAns;
        RepositoryQuestion _repoQues;
        RepositoryTag _repoTag;
        RepositoryUser _repoUser;
        RepositoryQuestionVisit _repoIst;
        RepositoryQuestionTag _repoQuesTag;
        public SoruController() {
            _repoAns = new RepositoryAnswers("",DbType.SqLite);
            _repoQues = new RepositoryQuestion("",DbType.SqLite);
            _repoTag = new RepositoryTag("",DbType.SqLite);
            _repoUser = new RepositoryUser("",DbType.SqLite);
            _repoIst = new RepositoryQuestionVisit("",DbType.SqLite);
            _repoQuesTag = new RepositoryQuestionTag("",DbType.SqLite);
        }
       
        public ActionResult Detay(long id,string title)
        {
            FrmViewData frm = new FrmViewData();
            frm.Question = _repoQues.GetById(id);
            if (frm.Question.State == State.Yayinda)
            {
                frm.Tags = _repoTag.GetListByQuestionId(id);
                frm.Answers = _repoAns.GetListByQuestionId(id);
                List<Question> benzerSorular = BulBenzerSorulari<long>(frm);
                frm.Questions = benzerSorular;
                User user = null;
               
                if (Request.IsAuthenticated)
                {
                    user = _repoUser.GetUserByEmail(HttpContext.User.Identity.Name);
                }
                 long userId=0;
                 if (user == null)
                    userId = 0;
                else
                    userId = user.UserId;
                 if (frm.Question.Title.ConvertWebUrl() == title)
                    InsertPageIstatik(userId,id);
              
                ViewBag.Title = frm.Question.Title;
                return View(frm);
            }
            else {
                TempData["mesaj"] = "Soru yayından kaldırılmış";
                return RedirectToAction("Succeed", "Soru");
            }
        }
      
       
        private List<Question> BulBenzerSorulari<T>(FrmViewData frm)
        {
            List<Question> benzerSorular = new List<Question>();
            long[] tagIds = frm.Tags.Select(x => x.TagId).ToArray();
            long[] tempIds = frm.Tags.Select(x => x.TagId).ToArray();
            benzerSorular = _repoQues.GetListByTag(25, tagIds).Where(x => x.QuestionId != frm.Question.QuestionId).ToList();
            int size=tagIds.Length-1;
            for (int i = 0; i < tempIds.Length; i++)
            {
                //tagIds.SkipWhile(x => x == tagIds[tagIds.Length - (i + 1)]).ToArray();
                if (benzerSorular.Count < 25 & size>1)
                {                   
                    List<List<long>> liste =PermatasyonUtil.Permutasyon<long>(tagIds.ToList(), size);
                    size = size - 1;
                    for (int j = 0; j < liste.Count; j++)
                    {
                        if (benzerSorular.Count < 25)
                        {
                            List<Question> temp = _repoQues.GetListByTag(25 - benzerSorular.Count, liste[j].ToArray()).Where(x => x.QuestionId != frm.Question.QuestionId).ToList();
                            benzerSorular.ConditionalAddRange(temp, "QuestionId");                          
                        }
                        else
                            break;                                
                    }                

                }
            }
            if (benzerSorular.Count < 25)
            {
                for (int j = 0; j < tagIds.Length; j++)
                {
                    if (benzerSorular.Count < 25)
                    {
                        List<Question> temp = _repoQues.GetListByTag(25 - benzerSorular.Count, tagIds[j]).Where(x => x.QuestionId != frm.Question.QuestionId).ToList();
                        benzerSorular.ConditionalAddRange(temp, "QuestionId");
                    }
                    else
                        break;

                }
            }         
            return benzerSorular;
        }
        [CaptchaValidator]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Detay(long id, string title, FormCollection frmCol, bool captchaValid)
        {
            ViewBag.isPostBack = "1";
            if (Request.IsAuthenticated)
            {

                FrmViewData frm = new FrmViewData();
                User user = _repoUser.GetUserByEmail(HttpContext.User.Identity.Name);
                bool createAnswer = true;
                if (string.IsNullOrEmpty(frmCol["description"]) || frmCol["description"].Length < 17)
                {
                    //ModelState.AddModelError("desciption", "Detaylı Açıklama Boş olamaz ve en az 10 karekter uzunluğunda olması gerekli");
                    ModelState.AddModelError("", "Detaylı Açıklama boş olamaz ve en az 10 karekter uzunluğunda olması gerekli");
                    createAnswer = false;
                }
               
                //if (!captchaValid)
                //{
                //    ModelState.AddModelError("valid", "Doğrulama kodu yanlış.");
                //    createAnswer = false;
                //}
                if(createAnswer)
                {
                    Answer answer = new Answer();
                    answer.BestReply = false;
                    answer.Date = DateTime.Now;
                    answer.IPAddress = GetIpAddress();
                    answer.Reply = frmCol["description"].Replace("'", "%t%").Replace("@", "@@");
                    answer.State = State.Yayinda;
                    answer.User = user;
                    answer.UserId = user.UserId;
                    answer.QuestionId = id;
                    _repoAns.Insert(answer);
                }
                frm.Question = _repoQues.GetById(id);
                frm.Tags = _repoTag.GetListByQuestionId(id);
                frm.Answers = _repoAns.GetListByQuestionId(id);
                List<Question> benzerSorular = BulBenzerSorulari<long>(frm);
                frm.Questions = benzerSorular;
              
                ViewBag.Title = frm.Question.Title;
                return View(frm);
            }
            else {               
                
                bool createUser = true;
                if (string.IsNullOrEmpty(frmCol["description"]) || frmCol["description"].Length < 17)
                {
                    //ModelState.AddModelError("desciption", "Detaylı Açıklama Boş olamaz ve en az 10 karekter uzunluğunda olması gerekli");
                    ModelState.AddModelError("", "Detaylı Açıklama boş olamaz ve en az 10 karekter uzunluğunda olması gerekli");
                    createUser = false;
                }
                ValiDateUser(frmCol,ref createUser);
                if (!captchaValid)
                {
                    ModelState.AddModelError("valid", "Doğrulama kodu yanlış");
                    ModelState.AddModelError("", "Doğrulama kodu yanlış");
                    createUser = false;
                }
                if (createUser)
                {
                    
                    User user = new Models.User();
                    user.Email = frmCol["RegisterModel.Email"];
                    user.RoleId = Role.Member.RoleId;
                    user.Name = frmCol["RegisterModel.UserName"];
                    user.Password = frmCol["RegisterModel.Password"];
                    user.PicturePath = new FrmBlog.Avatar.Avatar().GetRandomAvatar();
                    _repoUser.Insert(user);
                    IFormsAuthentication auth = new FormsAuthenticationWrapper();
                    auth.SetAuthCookie(user.Email, true);
                    
                    Answer answer = new Answer();
                    answer.BestReply = false;
                    answer.Date = DateTime.Now;
                    answer.IPAddress = GetIpAddress();
                    answer.Reply = frmCol["description"].Replace("'", "%t%");
                    answer.State = State.Yayinda;
                    answer.User = _repoUser.GetUserByEmail(user.Email);
                    answer.UserId = answer.User.UserId;
                    answer.QuestionId = id;
                    _repoAns.Insert(answer);

                }
                FrmViewData frm = new FrmViewData();
                frm.Question = _repoQues.GetById(id);
                frm.Tags = _repoTag.GetListByQuestionId(id);
                frm.Answers = _repoAns.GetListByQuestionId(id);
                List<Question> benzerSorular = BulBenzerSorulari<long>(frm);
                frm.Questions = benzerSorular;
                ViewBag.Title = frm.Question.Title;
                return RedirectToAction("Detay", new { id=id,title=title});
            }
        }

        private void ValiDateUser(FormCollection frmCol,ref bool createUser)
        {
           
            if (string.IsNullOrEmpty(frmCol["RegisterModel.UserName"]))
            {               
                ModelState.AddModelError("RegisterModel.UserName", "Kullanıcı isminizi giriniz");
                ModelState.AddModelError("", "İsminizi giriniz");
                createUser = false;
            }
            if (string.IsNullOrEmpty(frmCol["RegisterModel.Email"]) || !Regex.IsMatch(frmCol["RegisterModel.Email"], @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
            {
                ModelState.AddModelError("RegisterModel.Email", "Email giriniz");
                createUser = false;
            }
            if (string.IsNullOrEmpty(frmCol["RegisterModel.Password"]))
            {
                ModelState.AddModelError("RegisterModel.Password", "Şifrenizi giriniz");
                createUser = false;
            }
            if (_repoUser.IsRecordUser(frmCol["RegisterModel.Email"]))
            {
                ModelState.AddModelError("RegisterModel.Email", "Email başka bir kullanıcıya ait");
                createUser = false;
            }
           
           
        }
         [Authorize(Roles = "Administrator,Member", Order = 1)]
        public ActionResult Edit(long id)
        {
            FrmViewData frm = new FrmViewData();
            frm.Question = _repoQues.GetById(id);
            frm.Question.Title = frm.Question.Title.Replace("%t%", "'").Replace("@@","@");;
            frm.Question.Detail = frm.Question.Detail.Replace("%t%", "'").Replace("@@","@");;
            frm.Tags = _repoTag.GetListByQuestionId(id);
            string[]ary = frm.Tags.Select(x => x.TagName).ToArray();
            ViewData["tagsStr"] = string.Join(",", ary);
            return View("Sor",frm);
        }
         [Authorize(Roles = "Administrator,Member", Order = 1)]
         [ValidateInput(false)]
         [HttpPost]
        public ActionResult Edit(long id, string title, FormCollection frmCol)
        {
            bool sor = true;
            ValidateQuestion(frmCol, ref sor);
            TagValidControl(frmCol, ref sor);
           

            Question ques = _repoQues.GetById(id) ;
            if (!sor)
                return View("Sor",FrmView.Data.WithQuestion(ques));
            ques.Detail = frmCol["Question.Detail"].Replace("'", "%t%").Replace("@", "@@");           
            ques.Title = frmCol["Question.Title"].Replace("'", "%t%").Replace("@", "@@");            
            _repoQues.Update(ques);
            var NewAry = frmCol["tags"].Split(',');
            string[] OldAry = _repoQuesTag.ListByQuestionId(ques.QuestionId).Select(x => x.TagName).ToArray();
            foreach (var str in NewAry)
            {
                if (!OldAry.Contains(str))
                {
                    Tag tag = _repoTag.GetByName(str);
                    QuestionTag qt = new QuestionTag();
                    qt.TagId = tag.TagId;
                    qt.QuestionId = ques.QuestionId;
                    _repoQuesTag.Insert(qt);
                }
            }
            foreach (var str in OldAry)
            {
                if (!NewAry.Contains(str))
                {
                     Tag tag = _repoTag.GetByName(str);
                    _repoQuesTag.Delete(ques.QuestionId,tag.TagId);
                }
            }
            return RedirectToAction("Detay", "Soru", new { id=ques.QuestionId,title=ques.Title.ConvertWebUrl()});
        }

        public ActionResult Sor()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Sor(FormCollection frmCol, RegisterModel model)
        {
            if (Request.IsAuthenticated)
            {
                bool sor = true;
                User user = _repoUser.GetUserByEmail(HttpContext.User.Identity.Name);
                Question ques = new Question();
                ques.Detail = frmCol["Question.Detail"].Replace("'", "%t%").Replace("@", "@@");        
              
                ques.Title = frmCol["Question.Title"].Replace("'", "%t%").Replace("@", "@@");
                ValidateQuestion(frmCol, ref sor);
                TagValidControl(frmCol,ref sor);
                if (!sor)
                {
                    return View(FrmView.Data.WithQuestion(ques));
                }
               
                ques.Date = DateTime.Now;
                ques.Detail = frmCol["Question.Detail"].Replace("'", "%t%").Replace("@","@@");
                ques.IPAddress = GetIpAddress();
                ques.State = State.Yayinda;
                ques.Title = frmCol["Question.Title"].Replace("'", "%t%").Replace("@","@@");
                ques.User = user;
                ques.UserId = user.UserId;
                _repoQues.Insert(ques);
                var ary2 = frmCol["tags"].Split(',');
                int lastQuesId = _repoQues.LastQuestionId();
                InsertTag(ary2, lastQuesId);
                TempData["mesaj"] = "Sorunuz eklendi";
                return  RedirectToAction("Succeed","Soru");
            }           
            else
            {
                bool createUser = true;
                Question ques = new Question();
            
                ques.Detail = frmCol["Question.Detail"].Replace("'", "%t%").Replace("@", "@@");              
                ques.Title = frmCol["Question.Title"].Replace("'", "%t%").Replace("@", "@@");
                ValidateQuestion(frmCol,ref createUser);
                if (string.IsNullOrEmpty(frmCol["RegisterModel.UserName"]))
                {
                    ModelState.AddModelError("RegisterModel.UserName", "Kullanıcı isminizi giriniz");
                    ModelState.AddModelError("", "İsminizi giriniz");
                    createUser = false;
                }
                if (string.IsNullOrEmpty(frmCol["RegisterModel.Email"]) || !Regex.IsMatch(frmCol["RegisterModel.Email"], @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))
                {
                    ModelState.AddModelError("RegisterModel.Email", "Email giriniz");
                    ModelState.AddModelError("", "Email giriniz");
                    createUser = false;
                }
                if (string.IsNullOrEmpty(frmCol["RegisterModel.Password"]))
                {
                    ModelState.AddModelError("RegisterModel.Password", "Şifrenizi giriniz");
                    ModelState.AddModelError("", "Şifrenizi giriniz");
                    createUser = false;
                }
                if (_repoUser.IsRecordUser(frmCol["RegisterModel.Email"]))
                {
                    ModelState.AddModelError("RegisterModel.Email", "Email başka bir kullanıcıya ait");
                    ModelState.AddModelError("", "Email başka bir kullanıcıya ait");
                    createUser = false;
                }
                TagValidControl(frmCol,ref createUser);
                if (createUser)
                {

                    User user = new Models.User();
                    user.Email = frmCol["RegisterModel.Email"];
                    user.RoleId = Role.Member.RoleId;
                    user.Name = frmCol["RegisterModel.UserName"];
                    user.Password = frmCol["RegisterModel.Password"];
                    user.PicturePath = new FrmBlog.Avatar.Avatar().GetRandomAvatar();
                    _repoUser.Insert(user);
                    IFormsAuthentication auth = new FormsAuthenticationWrapper();
                    auth.SetAuthCookie(user.Email, true);
                 
                    ques.Date = DateTime.Now;
                    ques.Detail = frmCol["Question.Detail"].Replace("'", "%t%").Replace("@","@@");
                    ques.IPAddress = GetIpAddress();
                    ques.State = State.Yayinda;
                    ques.Title = frmCol["Question.Title"].Replace("'", "%t%").Replace("@","@@") ;
                    ques.User = _repoUser.GetUserByEmail(user.Email); ;
                    ques.UserId = ques.User.UserId;
                    _repoQues.Insert(ques);
                    TempData["mesaj"] = "Sorunuz eklendi";
                    var ary2 = frmCol["tags"].Split(',');
                    int lastQuesId = _repoQues.LastQuestionId();
                    InsertTag(ary2, lastQuesId);
                    return RedirectToAction("Succeed", "Soru");                

                }
                return View();
              
            }
        }

        private void ValidateQuestion(FormCollection frmCol,ref bool createUser)
        {
            if (string.IsNullOrEmpty(frmCol["Question.Title"]) || frmCol["Question.Title"].Length < 10)
            {                
                ModelState.AddModelError("", "Başlık boş olamaz ve en az 10 karekter uzunluğunda olması gerekli");
                createUser = false;
            }
            if (string.IsNullOrEmpty(frmCol["Question.Detail"]) || frmCol["Question.Detail"].Length < 17)
            {               
                ModelState.AddModelError("", "Detaylı Açıklama boş olamaz ve en az 10 karekter uzunluğunda olması gerekli");
                createUser = false;
            }            
        }

        private void TagValidControl(FormCollection frmCol,ref bool flag)
        {
            if (string.IsNullOrEmpty(frmCol["tags"]))
            {
                //ModelState.AddModelError("tags", "En az bir etiket girmelisiniz.");
                ModelState.AddModelError("", "En az bir etiket girmelisiniz.");
                flag = false;
            }
            if (!string.IsNullOrEmpty(frmCol["tags"]))
            {
                var ary = frmCol["tags"].Split(',');               
                ViewData["tagsStr"] = frmCol["tags"];
                if (ary.Length < 1 || ary.Length > 5)
                {
                    //ModelState.AddModelError("tags", "En az 1 en fazla 5 tane etiket olmalıdır.");
                    ModelState.AddModelError("", "En az 1 en fazla 5 tane etiket olmalıdır.");
                    flag = false;
                }
                if (ary.Length >= 1)
                {
                    string tagStr = "";
                    bool first = true;
                    foreach (var str in ary)
                    {
                        Tag tag = _repoTag.GetByName(str);
                        if (tag == null)
                        {                            
                            flag = false;
                            //ModelState.AddModelError("tags", "{0} isimli bir etiket yok.lütfen geçerli bir etiket seçiniz".With(str));
                            ModelState.AddModelError("", "{0} isimli bir etiket yok.lütfen geçerli bir etiket seçiniz".With(str));
                        }
                        else {
                            if (first)
                            {
                                tagStr = tag.TagName;
                                first = false;
                            }
                            else
                                tagStr += ",{0}".With(tag.TagName);
                        }
                    }
                    if(!first)
                        ViewData["tagsStr"] = tagStr;
                }
            }
            
        }

        private void InsertTag(string[] ary2, int lastQuesId)
        {
            foreach (var str in ary2)
            {
                Tag tag = _repoTag.GetByName(str);
                QuestionTag qt = new QuestionTag();
                if (qt != null)
                {
                    qt.QuestionId = lastQuesId;
                    qt.TagId = tag.TagId;

                    _repoQuesTag.Insert(qt);
                }
            }
        }
        public ActionResult Succeed()
        {
            ViewBag.mesaj = TempData["mesaj"];
            return View();
        }
        [Authorize(Roles = "Administrator,Member", Order = 1)]
        public ActionResult AnswerDelete(long id)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    _repoAns.Delete(id);

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
        [Authorize(Roles = "Administrator,Member", Order = 1)]
        public ActionResult SignBestReply(long id)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    _repoAns.SignBestReply(id);

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
        [Authorize(Roles = "Administrator,Member", Order = 1)]
        public ActionResult SoruyuYayindanKaldir(long id)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    _repoQues.SoruDurumunuDegistir(id,State.Durduruldu);

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
        [Authorize(Roles = "Administrator,Member", Order = 1)]
        public ActionResult CevapDuzelt(long id)
        {
            Answer ans = _repoAns.GetById(id);          
            ans.Reply = ans.Reply.Replace("%t%", "'").Replace("@@", "@");            
            return View(ans);
        }
        [Authorize(Roles = "Administrator,Member", Order = 1)]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CevapDuzelt(long id,FormCollection frmCol)
        {

            if (string.IsNullOrEmpty(frmCol["Reply"]) || frmCol["Reply"].Length < 12)
            {
                ModelState.AddModelError("reply", "Boş olamaz ve en az 5 karekter uzunluğunda olması gerekli");
                return View();
            }         

            Question ques = _repoQues.GetQuestionByAnswerId(id);
            Answer answer = _repoAns.GetById(id);
            answer.Reply = frmCol["Reply"];
            answer.Reply = answer.Reply.Replace("'", "%t%").Replace("@", "@@");
           
            _repoAns.Update(answer);          
           
            return RedirectToAction("Detay", "Soru", new { id = ques.QuestionId, title = ques.Title.ConvertWebUrl() });
        }

    }
}
