using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recaptcha;
using System.Configuration;
using System.Web.UI;
using System.IO;
using System.Web.Routing;
using mwm = Microsoft.Web.Mvc.Internal;
using Microsoft.Web.Mvc;
using System.Linq.Expressions;
using FrmBlog.Models;
using System.Text;
namespace FrmBlog.Helpers
{
    public static class HtmlHelperExtension
    {
        public static IHtmlString GenerateCaptcha(this HtmlHelper helper)
        {
            var captchaControl = new RecaptchaControl
            {
                ID = "recaptcha",
                Theme = "clean", //http://wiki.recaptcha.net/index.php/Theme
                PublicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"],
                PrivateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"]
            };
            var htmlWriter = new HtmlTextWriter(new StringWriter());
            captchaControl.RenderControl(htmlWriter);
            return helper.Raw(htmlWriter.InnerWriter.ToString());
        }
        public static string BuildUrlFromExpression<TController>(RequestContext context, RouteCollection routeCollection, Expression<Action<TController>> action) where TController : Controller
        {
            RouteValueDictionary routeValues = mwm.ExpressionHelper.GetRouteValuesFromExpression(action);
            VirtualPathData vpd = routeCollection.GetVirtualPathForArea(context, routeValues);//.GetVirtualPath(context, routeValues);
            return (vpd == null) ? null : vpd.VirtualPath;
        }
        public static string BuildUrlFromExpressionFG<TController>(this HtmlHelper helper, Expression<Action<TController>> action) where TController : Controller
        {
            return BuildUrlFromExpression<TController>(helper.ViewContext.RequestContext, helper.RouteCollection, action);
        }
        public static IHtmlString Pager(this HtmlHelper htmlHelper,
           PaginatedList pagingData, string queryData, Func<int?, HtmlHelper, string, string> fun)
        {
            StringBuilder html = new StringBuilder("");
            html.Append("<div class=\"wp-pagenavi\">");
            if (pagingData.TotalPages > 1)
            {

              
                // Previous Page

                if (pagingData.PageIndex > 1)
                {
                    if (pagingData.PageIndex == 2)
                    {

                        html.AppendFormat("<a href=\"{0}\" class=\"nextpostslink\" >", fun(null, htmlHelper, queryData));
                        html.Append(" << önceki </a>");

                    }
                    else
                    {
                        html.AppendFormat("<a href=\"{0}\" class=\"nextpostslink\" >", fun(pagingData.PageIndex - 1, htmlHelper, queryData));
                        html.Append(" << önceki </a>");
                    }

                }
                else
                {
                    //html.Append("<span class=\"last\" > << önceki</span>");
                }

                // first page
                if (pagingData.PageIndex > 1)
                {
                    html.AppendFormat("<a href=\"{0}\" class=\"page larger\"  >", fun(null, htmlHelper, queryData));
                    html.Append("1</a>");
                }
                if (pagingData.PageIndex > 4)
                    html.Append("<span class=\"extend\">...</span>");
                // current page previous 2
                for (int i = pagingData.PageIndex - 2; i < pagingData.PageIndex; i++)
                {
                    if (i > 1)
                    {
                        html.AppendFormat("<a href=\"{0}\" class=\"page larger\"  >{1}", fun(i, htmlHelper, queryData), i);
                        html.Append("</a>");
                    }
                }
                // current page
                if (pagingData.PageIndex != pagingData.TotalPages)
                    html.AppendFormat("<span class=\"current\">{0}</span>", pagingData.PageIndex);


                // current page next 2
                for (int i = pagingData.PageIndex + 1; i < pagingData.PageIndex + 3; i++)
                {
                    if (i < pagingData.TotalPages)
                    {
                        html.AppendFormat("<a href=\"{0}\" class=\"page larger\"  >{1}", fun(i, htmlHelper, queryData), i);
                        html.Append("</a>");
                    }
                }
                if ((pagingData.PageIndex + 2) < pagingData.TotalPages - 1)
                    html.Append("<span class=\"extend\">...</span>");
                // last Page
                if (pagingData.PageIndex == pagingData.TotalPages)
                {
                    html.AppendFormat("<span class=\"current\">{0}</span>", pagingData.PageIndex);

                }
                else
                {
                    html.AppendFormat("<a href=\"{0}\" class=\"page larger\"  >{1}", fun(pagingData.TotalPages, htmlHelper, queryData), pagingData.TotalPages);
                }
                // Next Page
                if (pagingData.PageIndex < pagingData.TotalPages)
                {

                    html.AppendFormat("<a href=\"{0}\" class=\"nextpostslink\" >", fun(pagingData.PageIndex + 1, htmlHelper, queryData));

                    html.Append("sonraki >> </a>");
                }
                else
                {

                    //html.Append("<span class=\"AtEnd\" > sonraki >> </span>");
                }   
               
                html.Append("</div>");
                html.Append("<br style=\"clear:both\" />");
            }
            return htmlHelper.Raw(html.ToString());

        }
        public static IHtmlString MetaDescription(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Raw(string.Format("\"{0}\"","işletim sistelerinin çözüm merkezi."));
        }
        public static IHtmlString MetaKeyboard(this HtmlHelper htmlHelper)
        {
            return htmlHelper.Raw(string.Format("\"{0}\"","windows xp,windows 7,windows vista,windows server 2003,windows server 2008,windows ce 6.0,windows phone7,pardus,ubuntu,android,macosx,symbianOs"));
        }
    }
}