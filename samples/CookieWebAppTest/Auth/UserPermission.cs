using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBMMNet.Web.Core.Interfaces
{
    /// <summary>
    /// Yetkileri için kullanıcı haklarını tanımlayan arayüzdür
    /// </summary>
    public class UserPermission
    {
        public PermissionTypes PermissionType { get; set; }
        public string Permission { get; set; }
    }

}
