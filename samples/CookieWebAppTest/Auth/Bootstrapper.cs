
using System;
using TBMMNet.Web.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Identity.EntityFramework;
using TBMMNet.Web.Core.Identity;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Authentication;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;

namespace TBMMNet.Web.Core
{
    /// <summary>
    /// TBMMNet.Web uygulamasının ayağa kaldırılması ve core service bileşenlerinin yüklenmesinden sorumludur. 
    /// </summary>
    public static class Bootstrapper
    {

        private const string AppSettingsConfigKey = "AppSettings";
        private const string ConnectionStringsConfigKey = "ConnectionStrings";
        private const string AppNameConfigKey = "AppSettings:ApplicationName";
        private const string TimeoutConfigKey = "AppSettings:SessionTimeout";

        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Configures custom services to add to the ASP.NET MVC 6 Injection of Control (IoC) container.
        /// </summary>
        /// <param name="services">The services collection or IoC container.</param>
        public static void ConfigureCoreServices(this IServiceCollection services)
        {
            #region Antiforgery
            services.ConfigureAntiforgery(
                    antiforgeryOptions =>
                    {
                        // Rename the Anti-Forgery cookie from "__RequestVerificationToken" to "f". This adds a little 
                        // security through obscurity and also saves sending a few characters over the wire. 
                        antiforgeryOptions.CookieName = "f";

                        // Rename the form input name from "__RequestVerificationToken" to "f" for the same reason above 
                        // e.g. <input name="__RequestVerificationToken" type="hidden" value="..." />
                        antiforgeryOptions.FormFieldName = "f";

                        // If you have enabled SSL/TLS. Uncomment this line to ensure that the Anti-Forgery cookie requires 
                        // SSL /TLS to be sent across the wire. 
                        // $Start-HttpsEverywhere$
                        // antiforgeryOptions.RequireSsl = true;
                        // $End-HttpsEverywhere$
                    });
            services.AddAntiforgery();
            #endregion

            #region Configuration

            var applicationEnvironment = services.BuildServiceProvider().GetRequiredService<IApplicationEnvironment>();
            var hostingEnvironment = services.BuildServiceProvider().GetRequiredService<IHostingEnvironment>();
            //var configurationPath = Path.Combine(applicationEnvironment.ApplicationBasePath, "config.json");

            // Set up configuration sources.
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(applicationEnvironment.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true)
                //All environment variables in the process's context flow in as configuration values.
                .AddEnvironmentVariables();

            Configuration = configBuilder.Build();

            services.AddSingleton(_ => Configuration as IConfiguration);

           
            #endregion  

            #region Logging


            services.AddLogging();
            

            #endregion

           

            #region Caching
            // Adds a default in-memory implementation of IDistributedCache, which is very fast but 
            // the cache will not be shared between instances of the application. 
            // Also adds IMemoryCache.
            services.AddCaching();

            // Uncomment the following line to use the Redis implementation of      
            // IDistributedCache. This will override any previously registered IDistributedCache 
            // service. Redis is a very fast cache provider and the recommended distributed cache 
            // provider.
            // services.AddTransient<IDistributedCache, RedisCache>();

            // Uncomment the following line to use the Microsoft SQL Server implementation of 
            // IDistributedCache. Note that this would require setting up the session state database.
            // Redis is the preferred cache implementation but you can use SQL Server if you don't 
            // have an alternative.
            // services.AddSqlServerCache(o =>
            // {
            //     o.ConnectionString = 
            //       "Server=.;Database=ASPNET5SessionState;Trusted_Connection=True;";
            //     o.SchemaName = "dbo";
            //     o.TableName = "Sessions";
            // });
            #endregion

            #region Session
            services.AddSession(o =>
            {
                //o.IdleTimeout = TimeSpan.FromMinutes(Double.Parse(Configuration[TimeoutConfigKey]));
                o.IdleTimeout = TimeSpan.FromMinutes(15);
            });
            #endregion

            #region Localization
            services.AddLocalization();
            #endregion

        }

        public static void AddCoreFrameworkComponents(this IServiceCollection services)
        {
            services.AddScoped<IUser, ApplicationUser>();
            services.AddTransient(typeof(IdentityUserContext));
            services.AddTransient(typeof(IdentityRoleContext));
        }
      

        public static void AddPageAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PageAllowance", delegate (AuthorizationPolicyBuilder policy)
                 {
                     policy.Requirements.Add(new PageAuthorizationRequirement());
                     //policy.RequireRole("SystemAdministrator", "Administrator", "Editor", "RestrictedEditor", "Visitor", "RestrictedVisitor");
                 });
            });

            services.AddInstance<IAuthorizationHandler>(new PageAllowanceAuthorizationHandler());
        }

        public static void AddFormsAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication();

            services.AddIdentity<ApplicationUser, IdentityRole>(
               options =>
               {
                   options.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                   options.Cookies.ApplicationCookie.AutomaticChallenge = true;
                   options.Cookies.ApplicationCookieAuthenticationScheme = "ApplicationCookie";
                   options.Cookies.ApplicationCookie.AuthenticationScheme = IdentityCookieOptions.ApplicationCookieAuthenticationType = "ApplicationCookie";
                   options.Cookies.ApplicationCookie.LoginPath = new PathString("/User/Login");
                   options.Cookies.ApplicationCookie.LogoutPath = new PathString("/User/Logout");
                   options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                   options.Cookies.ApplicationCookie.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                   options.Cookies.ApplicationCookie.SlidingExpiration = true;
                   options.Cookies.ApplicationCookie.CookieHttpOnly = true;
                   options.Cookies.ApplicationCookie.CookieSecure = CookieSecureOption.SameAsRequest;
                   options.Cookies.ApplicationCookie.SystemClock = new SystemClock();
                   options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents();
                   options.Cookies.ApplicationCookie.CookieName = "TBMMNet";
                   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                   options.Lockout.MaxFailedAccessAttempts = 10;
                   options.Lockout.AllowedForNewUsers = false;
               })
               .AddUserStore<IdentityUserStore<ApplicationUser>>()
               .AddRoleStore<IdentityRoleStore<IdentityRole>>()
               .AddDefaultTokenProviders();
        }
      

        public static void UseFormsAuthentication(this IApplicationBuilder app)
        {
            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
                options.AuthenticationScheme = "ApplicationCookie";
                options.LoginPath = new PathString("/Account/Login");
                options.LogoutPath = new PathString("/Account/Logout");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
                options.CookieHttpOnly = true;
                options.CookieSecure = CookieSecureOption.SameAsRequest;
                options.SystemClock = new SystemClock();
                options.Events = new CookieAuthenticationEvents();
            });
        }

    }
}