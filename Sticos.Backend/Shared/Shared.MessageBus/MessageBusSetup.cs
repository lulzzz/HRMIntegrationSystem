using System;
using System.Linq;
using System.Reflection;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.NLogIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sticos.Personal.MessageContracts;

namespace Shared.MessageBus
{
    public static class MessageBusSetup
    {
        public static void AddMessageBus(this IServiceCollection services, MessageBusConfig config)
        {
            var consumers = config.AssemblyNamesToScan.Select(Assembly.Load)
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(IConsumer))
                            && !t.Namespace.StartsWith("MassTransit")
                            && t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var consumer in consumers)
            {
                services.AddScoped(consumer);
            }

            services.AddMassTransit(x =>
            {
                var method = x.GetType().GetMethod("AddConsumer");

                foreach (var consumer in consumers)
                {
                    var generic = method.MakeGenericMethod(consumer);
                    generic.Invoke(x, null);
                }
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(config.HostName, "/", h =>
                {
                    h.Username(config.UserName);
                    h.Password(config.Password);
                });
                
                cfg.UseNLog();

                cfg.OverrideDefaultBusEndpointQueueName($"{config.ApiName}-{Environment.MachineName}-{Guid.NewGuid().ToString("N")}");
                
                cfg.ReceiveEndpoint(host, config.ApiName, e =>
                {
                    e.Durable = true;
                    e.AutoDelete = false;

                    // Set up EndpointConventions

                    EndpointConvention.Map<IAbsenceApproved>(e.InputAddress);

                    e.LoadFrom(provider);
                });
            }));

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            // Create request clients
            //services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<SubmitOrder>());

            services.AddSingleton<IHostedService, BusService>();
        }
    }

    public class MessageBusConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string[] AssemblyNamesToScan { get; set; }
        public string ApiName { get; set; }

    }
}