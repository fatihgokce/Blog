using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FrmBlog.Services
{
    public interface IFormsAuthentication
    {
        void SignOut();
        void SetAuthCookie(string email, bool createPersistentCookie);
        string HashPasswordForStoringInConfigFile(string password);
    }
    public class FormsAuthenticationWrapper : IFormsAuthentication
    {
        #region IFormsAuthentication Members
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
        public void SetAuthCookie(string email, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(email, createPersistentCookie);
        }
        public string HashPasswordForStoringInConfigFile(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
        }
        #endregion
    }
}