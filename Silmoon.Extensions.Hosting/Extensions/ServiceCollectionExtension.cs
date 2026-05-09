using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Silmoon.Extensions.Hosting.Interfaces;
using Silmoon.Extensions.Hosting.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silmoon.Extensions.Hosting.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSingletonStrict<TService>(this IServiceCollection services) where TService : class
        {
            if (services.Any(d => d.ServiceType == typeof(TService))) throw new InvalidOperationException($"Service '{typeof(TService).FullName}' has already been registered.");
            services.AddSingleton<TService>();
            return services;
        }
        public static IServiceCollection AddSingletonStrict<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
        {
            if (services.Any(d => d.ServiceType == typeof(TService))) throw new InvalidOperationException($"Service '{typeof(TService).FullName}' has already been registered.");
            services.AddSingleton<TService, TImplementation>();
            return services;
        }

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

        public static void AddSilmoonConfigure<TSilmoonConfigureService, TSilmoonPlatformDirectoryService>(this IServiceCollection services) where TSilmoonConfigureService : class, ISilmoonConfigureService where TSilmoonPlatformDirectoryService : class, ISilmoonPlatformDirectoryService
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddSingletonStrict<ISilmoonPlatformDirectoryService, TSilmoonPlatformDirectoryService>();
            services.AddSingleton<ISilmoonConfigureService, TSilmoonConfigureService>();
        }
        public static void AddSilmoonConfigure<TSilmoonConfigureService, TSilmoonPlatformDirectoryService>(this IServiceCollection services, Action<SilmoonConfigureServiceOption> option) where TSilmoonConfigureService : class, ISilmoonConfigureService where TSilmoonPlatformDirectoryService : class, ISilmoonPlatformDirectoryService
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(option);

            services.AddSingletonStrict<ISilmoonPlatformDirectoryService, TSilmoonPlatformDirectoryService>();
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
