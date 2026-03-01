using Microsoft.Extensions.DependencyInjection;
using Silmoon.Extensions.Hosting.Interfaces;
using Silmoon.Extensions.Hosting.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extensions.Hosting.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddSilmoonConfigure<TSilmoonConfigureService>(this IServiceCollection services) where TSilmoonConfigureService : class, ISilmoonConfigureService
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingleton<ISilmoonConfigureService, TSilmoonConfigureService>();
        }
        public static void AddSilmoonConfigure<TSilmoonConfigureService>(this IServiceCollection services, Action<SilmoonConfigureServiceOption> option) where TSilmoonConfigureService : class, ISilmoonConfigureService
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(option);

            services.Configure(option);
            services.AddSingleton<ISilmoonConfigureService, TSilmoonConfigureService>();
        }
        public static void AddSilmoonConfigure(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);
            services.AddSingleton<ISilmoonConfigureService, SilmoonConfigureService>();
        }
        public static void AddSilmoonConfigure(this IServiceCollection services, Action<SilmoonConfigureServiceOption> option)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(option);

            services.Configure(option);
            services.AddSingleton<ISilmoonConfigureService, SilmoonConfigureService>();
        }
    }
}
