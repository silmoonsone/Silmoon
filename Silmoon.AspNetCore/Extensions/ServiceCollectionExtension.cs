using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.DependencyInjection;
using Silmoon.AspNetCore.Services;
using Silmoon.AspNetCore.Services.Interfaces;
using Silmoon.Models.Identities;
using System;

namespace Silmoon.AspNetCore.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddSilmoonDevApp<TSilmoonDevAppService>(this IServiceCollection services) where TSilmoonDevAppService : SilmoonDevAppService
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.Configure<SilmoonAppDevServiceOptions>(o => o.KeyCacheSecoundTimeout = 3600);
            services.AddSingleton<ISilmoonDevAppService, TSilmoonDevAppService>();
        }

        public static void AddSilmoonDevApp<TSilmoonDevAppService>(this IServiceCollection services, Action<SilmoonAppDevServiceOptions> configure) where TSilmoonDevAppService : SilmoonDevAppService
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.Configure(configure);
            services.AddSingleton<ISilmoonDevAppService, TSilmoonDevAppService>();
        }

        public static void AddSilmoonAuth<TSilmoonAuthService>(this IServiceCollection services) where TSilmoonAuthService : class, ISilmoonAuthService
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddHttpContextAccessor();
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddScoped<ISilmoonAuthService, TSilmoonAuthService>();
        }
    }
}
