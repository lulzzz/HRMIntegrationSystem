using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common.Api.Contracts;
using Common.Api.Contracts.Employees;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Common.Api.IntegrationTests
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class EmployeeControllerTestServerTests
    {
        [Test]
        public async Task SearchEmployeesTestServerTest()
        {
            var client = BuildTestServer()
                .CreateClient();

            var response = await client.GetAsync("api/employees");
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsAsync<List<Employee>>();
            Assert.IsTrue(responseJson.Count >= 0, "response count is :" + responseJson.Count);
        }
        [Test]
        public async Task SearchEmployeesValidatePropertiesTest()
        {
            var client = BuildTestServer()
                .CreateClient();

            var response = await client.GetAsync("api/employees");
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadAsAsync<List<Employee>>();
            var firstEmployee = result.FirstOrDefault();
            Assert.IsTrue(firstEmployee.Id > 0);
            Assert.IsTrue(!string.IsNullOrWhiteSpace(firstEmployee.FirstName));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(firstEmployee.LastName));
            Assert.IsTrue(!string.IsNullOrWhiteSpace(firstEmployee.Email));
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
