using JSNLog;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OData.Edm;
using News.Api.Extensions;
using Shared.Interfaces.Models;
using Shared.MessageBus;
using Shared.Middleware;
using Shared.Policies;
using Shared.Services.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;

namespace News.Api
{
    public class Startup : IConfigurableStartUp
    {
        private readonly ILogger<Startup> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public Action<IServiceCollection> PostConfigureServiceCollection { get; set; }

        public Startup(ILogger<Startup> logger, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Startup: ConfigureServices News-Api");

            services.AddMvc(
                addAuthorization: !_hostingEnvironment.IsDevelopment(),
                configuration: _configuration);

            services.AddHttpClients(_configuration);
            services.AddIocMapping();
            services.AddSharedServices();

            services.AddPolicies();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            services.AddPoliciesGlobally();

            var messageBusEnabled = _configuration.GetValue<bool>("MessageBus.Enabled");
            if (messageBusEnabled)
            {
                _logger.LogInformation("Message bus is enabled. Configuring now.");
                services.AddMessageBus(new MessageBusConfig
                {
                    HostName = _configuration.GetValue<string>("MQ_HostName"),
                    UserName = _configuration.GetValue<string>("MQ_UserName"),
                    Password = _configuration.GetValue<string>("MQ_Password"),
                    AssemblyNamesToScan = new[] { "News.Api" },
                    ApiName = typeof(Startup).Assembly.GetName().Name
                });
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Sticos News Api", Description = "Swagger Sticos News Api" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "News.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });

            PostConfigureServiceCollection?.Invoke(services);

            services.AddOData();

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_hostingEnvironment.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseExceptionHandling();

            var loggerFactory = app.ApplicationServices.GetService<ILoggerFactory>();
            var jslogconf = new JsnlogConfiguration
            {
                corsAllowedOriginsRegex = _configuration.GetValue<string>("JSNlogCorsRegex")
            };
            app.UseJSNLog(new LoggingAdapter(loggerFactory), jslogconf);

            if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsStaging()
                                                    || _hostingEnvironment.EnvironmentName == "Utv")
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "STICOS NEWS API"); });
            }

            app.UseCors("AllowAll");

            if (!_hostingEnvironment.IsDevelopment())
            {
                app.UseAuthentication();
            }

            app.UseMvc(config =>
            {
                config
                .Select()
                .Expand()
                .Filter()
                .OrderBy()
                .MaxTop(100)
                .Count();

                config.MapODataServiceRoute("odata", "odata/{customerId}", GetEdmModel());
                config.MapRoute(
                          name: "default",
                          template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Models.News>("News");
            builder.EntitySet<Models.NewsAttachment>("NewsAttachments");

            builder.EnableLowerCamelCase();

            return builder.GetEdmModel();
        }
    }
}
