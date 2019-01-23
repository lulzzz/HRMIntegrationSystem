using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Interfaces.Models;
using Shared.MessageBus;
using Shared.Middleware;
using Shared.Policies;
using Shared.Services.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Timereg.Api.Extensions;

namespace Timereg.Api
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
            _logger.LogInformation("Startup: ConfigureServices Timereg-Api");

            services.AddMvc(
                addAuthorization: !_hostingEnvironment.IsDevelopment(),
                configuration: _configuration);
            
            services.AddHttpClients(_configuration);
            services.AddAutoMapper(AutoMapperSetup.Config);
            services.AddIocMapping();
            services.AddSharedServices();

            services.AddPolicies();

            var messageBusEnabled = _configuration.GetValue<bool>("MessageBus.Enabled");
            if (messageBusEnabled)
            {
                _logger.LogInformation("Message bus is enabled. Configuring now.");
                services.AddMessageBus(new MessageBusConfig
                {
                    HostName = _configuration.GetValue<string>("MQ_HostName"),
                    UserName = _configuration.GetValue<string>("MQ_UserName"),
                    Password = _configuration.GetValue<string>("MQ_Password"),
                    AssemblyNamesToScan = new[] {"Timereg.Api.MessageBus"},
                    ApiName = typeof(Startup).Assembly.GetName().Name
                });
            }

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            // Register the Swagger generator, defining one or more Swagger documents  
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "Timereg Api", Description = "Swagger core timereg Api"});

                var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"Timereg.Api.xml";
                c.IncludeXmlComments(xmlPath);
            });

            PostConfigureServiceCollection?.Invoke(services);

            return services.BuildServiceProvider();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            if (_hostingEnvironment.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseExceptionHandling();

            if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsStaging() 
                                                    || _hostingEnvironment.EnvironmentName == "Utv")
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "STICOS TIMEREG API"); });
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