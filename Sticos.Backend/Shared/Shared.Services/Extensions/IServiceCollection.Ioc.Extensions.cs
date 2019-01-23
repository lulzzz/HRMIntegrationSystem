using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Services.Extensions
{
    public static class IServiceCollectionIocExtensions
    {
        public static IServiceCollection Remove<T>(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null) services.Remove(serviceDescriptor);

            return services;
        }
        public static void ReplaceTransient<TService,TImplementation>(this IServiceCollection services) where TImplementation : class,TService where TService : class
        {
            services.Remove<TService>();
            services.AddTransient<TService, TImplementation>();
        }
        public static void ReplaceScoped<TService,TImplementation>(this IServiceCollection services) where TImplementation : class,TService where TService : class
        {
            services.Remove<TService>();
            services.AddScoped<TService, TImplementation>();
        }
        public static void ReplaceScoped<TService>(this IServiceCollection services, TService impl) where TService : class
        {
            services.Remove<TService>();
            services.AddScoped<TService>(i=>impl);
        }
        public static void ReplaceSingleton<TService>(this IServiceCollection services, TService impl) where TService : class
        {
            services.Remove<TService>();
            services.AddSingleton<TService>(impl);
        }

        public static void AddJsonHttpClients(this IServiceCollection services,IDictionary<string,string> clientNameBaseUrls)
        {
            foreach (var item in clientNameBaseUrls)
            {
                services.AddHttpClient(item.Key, client =>
                {
                    client.BaseAddress = new Uri(item.Value);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                });
            }
            
        }
    }
}