﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Altinn.Api.Extensions;
using Microsoft.Extensions.Logging;
using Shared.Middleware;
using Shared.Interfaces.Models;
using Shared.Policies;
using Shared.Services.Extensions;

namespace Altinn.Api
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
            _logger.LogInformation("Startup: ConfigureServices Altinn-Api");

            services.AddAutoMapper(AutoMapperSetup.Config);
            services.AddIocMapping();
            services.AddHttpClients(_configuration);
            services.AddSharedServices();

            services.AddPolicies();

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Altinn API", Version = "v1" });
            });

            services.AddMvc(
               addAuthorization: !_hostingEnvironment.IsDevelopment(),
               configuration: _configuration);

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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Altinn API V1");
                });
            }

            if (!_hostingEnvironment.IsDevelopment())
            {
                app.UseAuthentication();
            }

            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}
