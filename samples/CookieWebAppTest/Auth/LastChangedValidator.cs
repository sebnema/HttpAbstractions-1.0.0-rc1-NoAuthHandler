using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TBMMNet.Web.Core.Auth
{
    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            //// Pull database from registered DI services.
            //var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserIdentityService>();
            //var userPrincipal = context.Principal;

            //// Look for the last changed claim.
            //string lastChanged;
            //lastChanged = (from c in userPrincipal.Claims
            //               where c.Type == "LastUpdated"
            //               select c.Value).FirstOrDefault();

            //if (string.IsNullOrEmpty(lastChanged) ||
            //    !userRepository.ValidateLastChanged(userPrincipal, lastChanged))
            //{
            //    context.RejectPrincipal();
            //    await context.HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");
            //}
        }
    }

}
