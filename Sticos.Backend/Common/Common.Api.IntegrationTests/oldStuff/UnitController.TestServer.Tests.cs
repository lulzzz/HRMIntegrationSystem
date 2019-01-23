using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Api.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Common.Api.IntegrationTests
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class UnitControllerTestServerTests
    {
        [Test]
        public async Task GetAllCustomerUsingTestServerTest()
        {
            var client = BuildTestServer()
                .CreateClient();

            var response = await client.GetAsync("api/units");
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsAsync<List<Unit>>();
            Assert.IsTrue(responseJson.Count >= 0, "response count is :" + responseJson.Count);
        }

        private static TestServer BuildTestServer()
        {
            var projectDir = @"Common\Common.Api\";
            var webhostBuilder = new WebHostBuilder()
                .UseSolutionRelativeContentRoot(projectDir)
                .UseEnvironment("Development")
                .UseConfiguration(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build())
                .UseStartup<Common.Api.Startup>();

            return new TestServer(webhostBuilder);
        }
    }
}
