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
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.Configure<SilmoonDevAppServiceOptions>(o => o.KeyCacheSecoundTimeout = 3600);
            services.AddSingleton<ISilmoonDevAppService, TSilmoonDevAppService>();
        }

        public static void AddSilmoonDevApp<TSilmoonDevAppService>(this IServiceCollection services, Action<SilmoonDevAppServiceOptions> options) where TSilmoonDevAppService : SilmoonDevAppService
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (options == null) throw new ArgumentNullException(nameof(options));

            services.Configure(options);
            services.AddSingleton<ISilmoonDevAppService, TSilmoonDevAppService>();
        }

        public static void AddSilmoonAuth<TSilmoonAuthService>(this IServiceCollection services) where TSilmoonAuthService : class, ISilmoonAuthService
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddHttpContextAccessor();
            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
            services.AddScoped<ISilmoonAuthService, TSilmoonAuthService>();
        }

        public static void AddSilmoonConfigure<TSilmoonConfigureService>(this IServiceCollection services) where TSilmoonConfigureService : class, ISilmoonConfigureService
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<ISilmoonConfigureService, TSilmoonConfigureService>();
        }
        public static void AddSilmoonConfigure<TSilmoonConfigureService>(this IServiceCollection services, Action<SilmoonConfigureServiceOption> option) where TSilmoonConfigureService : class, ISilmoonConfigureService
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (option == null) throw new ArgumentNullException(nameof(option));

            services.Configure(option);
            services.AddSingleton<ISilmoonConfigureService, TSilmoonConfigureService>();
        }
    }
}
