using System;
using System.Collections.Generic;

namespace TBMMNet.Web.Core.Interfaces
{
    // <summary>
    /// Kullanıcı Bilgilerini içeren ara yüzdür
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Kullanıcının IP'si
        /// </summary>
        string UserIp { get; set; }

        /// <summary>
        /// Kullanıcının Session Id'si
        /// </summary>
        string SessionId { get; set; }

        /// <summary>
        /// Kullanıcının departman kodu
        /// </summary>
        string DepartmentCode { get; set; }

        /// <summary>
        /// Tanımlı kullanıcı hakları
        /// </summary>
        List<UserPermission> Permissions { get; set; }

        /// <summary>
        /// Authorization için kullanılacak olan resource'ları içerir
        /// </summary>
        List<string> Resources { get; set; }
    }

}
