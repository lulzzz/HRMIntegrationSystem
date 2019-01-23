using System.Linq;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using NUnit.Framework;
using TestCommon.Builders;

namespace Common.Api.IntegrationTests
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class EmployeeServiceEFTests
    {
        [Test]
        public async Task GetAllEmployeesTest()
        {
            var service = CreateService();

            var employees = await service.SearchEmployee(new SearchQueryEmployee {Skip = 0, Take = int.MaxValue});
            Assert.AreEqual(2, employees.Count());
        }

        private static IEmployeeService CreateService()
        {
            var customerId = 1;
            

            var factory = new PersonalLegacyContextFactoryBuilder()
                .SetConnectionstringFormat("Data Source=localhost;Initial Catalog=SticosPersonalKunde_{0};Integrated Security=True;")
                .WithCurrentCustomerId(customerId)
                .Build();

            var commonfactory = new PersonalCommonLegacyContextFactoryBuilder()
                .SetConnectionstringFormat("Data Source=localhost;Initial Catalog=SticosPersonalFelles;Integrated Security=True;")
                .Build();

            var repository = new EmployeeLegacyDbEfRepositoryBuilder()
                .WithDbFactory(factory)
                .WithDbCommonFactory(commonfactory)
                .Build();

            var controller = new EmployeeServiceBuilder()
                .WithRepository(repository)
                .Build();
            return controller;
        }
    }
}
