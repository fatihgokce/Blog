using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using FrmBlog.Repositorys;
using System.Runtime.InteropServices;

namespace FrmBlog.Models
{
    [ComVisible(true)]
    [Serializable]
    public class User :MarshalByRefObject, IPrincipal
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PicturePath { get; set; }
        public string DisplayName { get; set; }
        public DateTime RecordDate { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        #region IPrincipal Members
        public virtual IIdentity Identity
        {
            get
            {
                
                bool isAuthenticated = (Role.Member.RoleId==RoleId || Role.Administrator.RoleId==RoleId);
                return new Identity(isAuthenticated, this.Email);
            }
        }
        public bool IsInRole(string role)
        {
            Role rol = new Role();
            if (Role.Administrator.RoleId == this.RoleId)
                rol = Role.Administrator;
            else if (Role.Member.RoleId == this.RoleId)
                rol = Role.Member;
            return (rol.Name == role);
        }
        #endregion
        public virtual bool IsAdministrator { get { return this.RoleId == Role.AdministratorId; } }
        public virtual bool IsMember { get { return this.RoleId == Role.MemberId; } }
    }
    [ComVisible(true)]
    [Serializable]
    public class Identity : IIdentity
    {
        bool isAuthenticated;
        string name;

        public Identity(bool isAuthenticated, string name)
        {
            this.isAuthenticated = isAuthenticated;
            this.name = name;
        }

        public string AuthenticationType
        {
            get { return "Forms"; }
        }

        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
        }

        public string Name
        {
            get { return name; }
        }
    }
}