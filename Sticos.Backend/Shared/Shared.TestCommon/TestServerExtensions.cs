using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using sticos = Sticos.Foundation.Login.IdentityManager.Common;

namespace Shared.TestCommon
{
    public static class TestServerExtensions
    {
        public static HttpClient CreateClientWithJwtToken(this TestServer testServer, int customerId, int userId)
        {
            var token = CreateJwtSecurityToken(testServer, customerId, userId);
            var client = testServer.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme,
                new JwtSecurityTokenHandler().WriteToken(token));
            return client;
        }

        private static JwtSecurityToken CreateJwtSecurityToken(TestServer testServer, int customerId, int userId)
        {
            var certificateLoader = testServer.Host.Services.GetService<ICertificateLoader>();

            var signingCredentials = new SigningCredentials(
                new X509SecurityKey(certificateLoader.GetSigningCertificate()),
                SecurityAlgorithms.RsaSha256);

            return new JwtSecurityToken(
                issuer: "http://tokenservice.sticos.no/",
                audience: "http://tokenservice.sticos.no/resources",
                claims: new[]
                {
                    new Claim(sticos.Constants.ClaimTypes.Kundeid, customerId.ToString()),
                    new Claim(ClaimTypes.Role,"KundeAdmin"),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
            );
        }


    }
}