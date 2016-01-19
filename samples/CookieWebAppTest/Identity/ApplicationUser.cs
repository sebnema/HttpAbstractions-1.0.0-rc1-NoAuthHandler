using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using TBMMNet.Web.Core.Interfaces;

namespace TBMMNet.Web.Core.Identity
{
    public class ApplicationUser : IdentityUser, IUser
    {
        /// <summary>
        /// Tanımlı kullanıcı hakları listesi
        /// </summary>
        public List<UserPermission> Permissions
        {
            get;
            set;
        }

        public string DepartmentCode
        {
            get;
            set;
        }

        public int OrganizationCode
        {
            get;
            set;
        }

        public List<string> Resources
        {
            get;
            set;
        }

        public string SessionId
        {
            get;
            set;
        }

        public string UserIp
        {
            get;
            set;
        }
    }

}
