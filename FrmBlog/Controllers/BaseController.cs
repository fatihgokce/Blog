using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FrmBlog.Filters;
using FrmBlog.Repositorys;
using FrmBlog.Models;
namespace FrmBlog.Controllers
{
    [Authenticate]
    public class BaseController :Controller
    {
        RepositorySetting _repSet;
        public BaseController()
        {
            _repSet = new RepositorySetting("",DbType.SqLite);
        }
        public string GetIpAddress()
        {
            string strIpAddress;
            strIpAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(strIpAddress))
            {
                strIpAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            strIpAddress = Request.ServerVariables["REMOTE_ADDR"];
            return strIpAddress.Trim() ;
        }
        public int GetMaxRecord
        {
            get
            {
                return 3;
            }
            private set { }
        }
        public int AnaSayfaId {
            get { return -1; }
            private set { }
        }
        protected int PagerCount()
        {
            string page = _repSet[SettingKey.PagerCount];
            if (string.IsNullOrEmpty(page))
                return 12;
            return int.Parse(page);
        }
        public bool IsBoot(string ip)
        {
            string[] boots =
            { 
                "66.249.66.229","66.249.66.41","66.249.72.99","66.249.72.20","66.249.72.206",
                "66.249.71.178","66.249.71.139","66.249.68.168","66.249.68.227","85.17.29.107","182.236.115.89","203.6.149.19","222.166.181","203.6.149.19","222.184.232.214"
               ,"62.4.73.228,202.170.66.83","209.202.9.10","216.17.53.99","140.113.65.164","140.113.65.164","182.236.115.89","222.166.181",
               "111.116.83.10","65.52.108.145","216.104.15","113.59.18.214","203.85.72.132","81.169.147.145","49.158.28.220","212.76.93.41","221.1.206.218","150.70.75.36","92.79.28.70","150.70.64.193","196.30.133.146",
               "193.13.137.25","195.117.191.119","207.47.8.11","129.187.44.153","184.73.159.120","207.255.179.8","202.143.224.8","210.196.243.121","183.181.53.248","50.17.124.244","83.230.106.30",

            };
            if (boots.Contains(ip) || ip.Contains("180.76.5") || Request.ServerVariables["HTTP_USER_AGENT"].Contains("Googlebot"))
                return true;
            return false;
        }
        protected void InsertPageIstatik(long userId, long questionId)
        {
            string ip = GetIpAddress();
            string browser = Request.Browser.Browser;
            string http_agent=Request.ServerVariables["HTTP_USER_AGENT"];
            string referr=Request.ServerVariables["HTTP_REFERER"]; 
            
            RepositoryQuestionVisit _repoIst = new RepositoryQuestionVisit("", DbType.SqLite);
            //bool referanFromAramaMoturu = (referr.Contains("mayestro.net") || referr.Contains("yandex.com") || referr.Contains("gooogle.com") || referr.Contains("yahoo.com") ||
            //    referr.Contains("bing.com"));
            bool referanFromAramaMoturu = (!string.IsNullOrEmpty(referr) || questionId==AnaSayfaId);
            bool aramaMotoruBot = (!http_agent.Contains("YandexBot") && !http_agent.Contains("bingbot") && !browser.Contains("msnbot") && !browser.Contains("Unknown"));
            bool beforeConnectWitIp= _repoIst.IsBeforeConnectedWithIp(questionId, DateTime.Today, ip);
            //bool botControl=(!beforeConnect && (string.IsNullOrEmpty(referr) && referr. ) );
           
            if (referanFromAramaMoturu && aramaMotoruBot && !IsBoot(ip)  && !beforeConnectWitIp )
            {
                //bool beforeConnect = _repoIst.IsBeforeConnected(ip, DateTime.Today);
                //if(string.IsNullOrEmpty(referr))
                //    referr="";
                //if (!beforeConnect && referr.Contains("mayestro.net"))
                //{
                //    return;
                //}
                QuestionVisit ist = new QuestionVisit();
                ist.Date = DateTime.Now;
                ist.IPAddress = GetIpAddress();
                ist.QuestionId = questionId;
                ist.UserId = userId;
                ist.Referans = Request.ServerVariables["HTTP_REFERER"];
                if (string.IsNullOrEmpty(ist.Referans))
                    ist.Referans = string.Empty;            
               
                _repoIst.Insert(ist);
            }
        }
    }
}
