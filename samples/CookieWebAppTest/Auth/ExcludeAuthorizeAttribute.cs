using System;
using Microsoft.AspNet.Mvc.Filters;

namespace TBMMNet.Web.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExcludeAuthorizeAttribute : ActionFilterAttribute
    {
    }
}
