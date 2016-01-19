using Microsoft.AspNet.Authorization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TBMMNet.Web.Core.Identity;
using TBMMNet.Web.Core.Interfaces;

namespace TBMMNet.Web.Core
{
    public class PageAuthorizationRequirement : IAuthorizationRequirement
    {
        //public PageAuthorizationRequirement(string appName)
        //{
        //    AppName = appName;
        //}

        //public string AppName { get; set; }
    }

    public class PageAllowanceAuthorizationHandler : AuthorizationHandler<PageAuthorizationRequirement>, IAuthorizationRequirement
    {
        protected override void Handle(AuthorizationContext context, PageAuthorizationRequirement requirement)
        {
            var mvcContext = context.Resource as Microsoft.AspNet.Mvc.Filters.AuthorizationContext;

            //TODO (SA) bunu base authorize attribute handle ediyormu test et.
            //if (mvcContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) ||
            //    mvcContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true))
            //{
            //    context.Succeed(requirement);
            //    return;
            //}

            //bool excludeAuthorize = mvcContext.ActionDescriptor.IsDefined(typeof(ExcludeAuthorizeAttribute), inherit: true) ||
            //   mvcContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(ExcludeAuthorizeAttribute), inherit: true);

            bool excludeAuthorize = false;

            if (!excludeAuthorize)
            {
                var currentPageRoute = GetPageRoute(context);

                if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role) && !context.User.HasClaim(c => c.Type == "AllowedPageRoute"))
                {
                    context.Fail();
                    return;
                }

                if (context.User.IsInRole(RoleTypes.Administrator.ToString()) || context.User.IsInRole(RoleTypes.SystemAdministrator.ToString()))
                {
                    context.Succeed(requirement);
                }
                else  // Kullanıcının eriş izini verilen sayfalar aranır
                {
                   
                    if (HasPageAccess(context, currentPageRoute))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                }
            }
        }

        private bool HasPageAccess(AuthorizationContext context, string currentPageRoute)
        {
            var allowedPageRoutes = context.User.FindAll(c => c.Type == "AllowedPageRoute");

            var userPermissions = new List<UserPermission>();
            foreach (var page in allowedPageRoutes)
            {
                userPermissions.Add(new UserPermission() { PermissionType = PermissionTypes.Page, Permission = page.Value });
            }

            if (userPermissions == null)
            {
                return false;
            }
            return userPermissions.Exists(t => t.PermissionType == PermissionTypes.Page && t.Permission == currentPageRoute);
        }

        private string GetPageRoute(AuthorizationContext context)
        {
            string routeData = string.Empty;
            var mvcContext = context.Resource as Microsoft.AspNet.Mvc.Filters.AuthorizationContext;

            if (mvcContext != null)
            {
                var controler = mvcContext.RouteData.Values["controller"];
                var action = mvcContext.RouteData.Values["action"];
                routeData = string.Format("{0}/{1}", controler, action);
            }

            return routeData;
        }

    }
}
