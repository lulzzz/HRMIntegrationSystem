using Common.Api.Controllers;
using Common.Api.Extensions;
using JSNLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Shared.Interfaces.Models;
using Shared.Middleware;
using Shared.Policies;
using Shared.Services.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Common.Api
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

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Startup: ConfigureServices Common-Api");

            services.AddMvc(
                addAuthorization: !_hostingEnvironment.IsDevelopment(),
                configuration: _configuration,
                globalControllers: new Type[] { typeof(CurrentUserController) });

            services.AddAutoMapper(AutoMapperSetup.Config);
            services.AddIocMapping();
            services.AddJsonHttpClients();
            services.AddSharedServices();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            services.AddPolicies();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Sticos Common Api", Description = "Swagger Sticos Common Api" });
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "Common.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });

            PostConfigureServiceCollection?.Invoke(services);
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
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "STICOS COMMON API"); });
            }

            app.UseCors("AllowAll");

            if (!_hostingEnvironment.IsDevelopment())
            {
                app.UseAuthentication();
            }

            app.UseMvc();
        }
    }
}
