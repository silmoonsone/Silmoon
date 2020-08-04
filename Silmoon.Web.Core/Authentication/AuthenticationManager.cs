using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Silmoon.Web.Core.Authentication
{
    public class AuthenticationManager
    {
        public AuthenticationManager()
        {

        }
        public static void ConfigureAuthenticationService(IServiceCollection services, string loginPath, string accessDeniedPath)
        {
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.RequireAuthenticatedSignIn = false;
            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.LoginPath = loginPath;
                o.AccessDeniedPath = accessDeniedPath;
            });
        }
    }
}
