using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrmBlog.Models
{
    
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        public const int AdministratorId = 1;
        public const int MemberId = 2;
        public const int GuestId = 3;

        // allowed roles. These must match the data in table Role
        public static Role Administrator { get { return new Role() { RoleId = AdministratorId, Name = "Administrator" }; } }
        public static Role Member { get { return new Role() { RoleId = MemberId, Name = "Member" }; } }
        public static Role Guest { get { return new Role() { RoleId = GuestId, Name = "Guest" }; } }

        public virtual bool IsAdministrator { get { return Name == Administrator.Name; } }
        public virtual bool IsMember { get { return Name == Member.Name; } }
        public virtual bool IsGuest { get { return Name == Guest.Name; } }
    }
}