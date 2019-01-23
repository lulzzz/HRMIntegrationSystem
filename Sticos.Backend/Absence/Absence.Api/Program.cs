using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using NLog.Web;
using Shared.Logger.Extensions;

namespace Absence.Api
{
    public class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.Services.AddSticosNlog(context.Configuration.GetConnectionString("NLogConn"));
                })
                .UseNLog()
                .UseStartup<Startup>();
    }
}
