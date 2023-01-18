﻿using Microsoft.Extensions.DependencyInjection;
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

            services.Configure<SilmoonDevAppServiceOptions>(o => o.KeyCacheSecoundTimeout = 3600);
            services.AddSingleton<ISilmoonDevAppService, TSilmoonDevAppService>();
        }

        public static void AddSilmoonDevApp<TSilmoonDevAppService>(this IServiceCollection services, Action<SilmoonDevAppServiceOptions> configure) where TSilmoonDevAppService : SilmoonDevAppService
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

        public static void AddSilmoonUser<TSilmoonUserService>(this IServiceCollection services) where TSilmoonUserService : class, ISilmoonUserService
        {
            services.AddHttpContextAccessor();
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<ISilmoonUserService, TSilmoonUserService>();
        }
    }
}
