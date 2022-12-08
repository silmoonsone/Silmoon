using Microsoft.Extensions.DependencyInjection;
using Silmoon.AspNetCore.Services;
using Silmoon.AspNetCore.Services.Interfaces;
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

            services.Configure<SilmoonDevAppOptions>(o => o.KeyCacheSecoundTimeout = 3600);
            services.AddSingleton<SilmoonDevAppService, TSilmoonDevAppService>();
        }

        public static void AddSilmoonDevApp<TSilmoonDevAppService>(this IServiceCollection services, Action<SilmoonDevAppOptions> configure) where TSilmoonDevAppService : SilmoonDevAppService
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
            services.AddSingleton<SilmoonDevAppService, TSilmoonDevAppService>();
        }
    }
}
