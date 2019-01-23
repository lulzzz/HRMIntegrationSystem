using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Interfaces;
using Shared.Services.Services;

namespace Shared.Services.Extensions
{
    public static class IServiceCollectionJwtExentions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var certificateLoader = new CertificateLoader(configuration);
            services.AddSingleton<ICertificateLoader>(certificateLoader);

            var signingCertificate = certificateLoader.GetSigningCertificate();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "http://tokenservice.sticos.no/",
                        ValidateIssuer = true,
                        ValidAudience = "http://tokenservice.sticos.no/resources",
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new X509SecurityKey(signingCertificate),
                        RequireSignedTokens = true
                    };
                });
        }
    }
}
