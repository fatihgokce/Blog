using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace FrmBlog.Module
{
    public class AddWWWPrefixModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {

        }

        private static Regex regex = new Regex("(http|https)://www\\.",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
            //context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }
        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext ctx = ((HttpApplication)sender).Context;
            IHttpHandler handler = ctx.Handler;

            // Only worry about redirecting pages at this point
            // static files might be coming from a different domain

            Uri url = ctx.Request.Url;
            bool hasWWW = regex.IsMatch(url.ToString());
            bool isLocal = url.Host == "localhost" ? true : false;
            ctx.Response.AddHeader("Location",url.ToString());;
            if (!isLocal && !hasWWW)
            {
                UriBuilder uri = new UriBuilder(ctx.Request.Url);



                // Perform a permanent redirect - I've generally implemented this as an 
                // extension method so I can use Response.PermanentRedirect(uri)
                // but expanded here for obviousness:
                ctx.Response.RedirectPermanent(string.Format("{0}://www.{1}{2}", url.Scheme, url.Authority, url.AbsolutePath));
                //ctx.Response.AddHeader("Location",uri.Uri.ToString());
                //response.AddHeader("Location", uri);
                //response.StatusCode = 301;
                //response.StatusDescription = "Moved Permanently";
                //response.End();
                
            }

        }
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            Uri url = application.Context.Request.Url;

            bool hasWWW = regex.IsMatch(url.ToString());
            bool isLocal = url.Host == "localhost" ? true : false;
            if (!isLocal && !hasWWW)
            {
                String newUrl = regex.Replace(url.ToString(),
                    String.Format("{0}://www.", url.Scheme));
                string newUr = string.Format("{0}://{1}.{2}", url.Scheme, url.Authority, url.AbsolutePath);
                application.Context.Response.Redirect(string.Format("{0}://www.{1}{2}", url.Scheme, url.Authority, url.AbsolutePath));
                //String newUrl = regex.Replace(url.ToString(),
                //String.Format("{0}://", url.Scheme));

                //application.Context.Response.Redirect(newUrl);
            }
        }

        #endregion
    }
}