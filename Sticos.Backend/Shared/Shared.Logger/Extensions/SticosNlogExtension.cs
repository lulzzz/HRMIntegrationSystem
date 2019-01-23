using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Logger.LayoutRenderers;

namespace Shared.Logger.Extensions
{
    public static class SticosNlogExtension
    {
        public static void AddSticosNlog(this IServiceCollection services, string nLogConnectionString = null)
        {
            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("sticos-server-ip", typeof(ServerIpLayoutRenderer));
            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("sticos-https", typeof(HttpsLayoutRenderer));
            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("sticos-url", typeof(UrlLayoutRenderer));
            NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("sticos-port", typeof(PortLayoutRenderer));


            if (!string.IsNullOrWhiteSpace(nLogConnectionString))
            {
                NLog.LogManager.Configuration.Variables["mynlogconnectionstring"] = nLogConnectionString;
                NLog.LogManager.KeepVariablesOnReload = true;
                NLog.LogManager.Configuration.Reload();
            }

            services.AddSingleton<ILoggerProvider>(serviceProvider =>
            {
                serviceProvider.SetupNLogServiceLocator();
                return new NLogLoggerProvider(NLogAspNetCoreOptions.Default);
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
