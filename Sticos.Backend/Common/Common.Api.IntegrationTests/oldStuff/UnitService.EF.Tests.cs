using System.Linq;
using System.Threading.Tasks;
using Common.Api.Domain.Interfaces;
using Common.Api.Extensions;
using Common.Api.Repositories.Legacy.Factories;
using NUnit.Framework;
using TestCommon.Builders;

namespace Common.Api.IntegrationTests
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class UnitServiceEFTests
    {
        [Test]
        public async Task GetAllCustomerTest()
        {
            var service = CreateUnitService();

            var units = await service.Search(new Domain.Entities.SearchQueryUnit());
            Assert.AreEqual(6, units.Count());
        }

        private static IUnitService CreateUnitService()
        {
            var customerId = 1;

            var factory = new PersonalLegacyContextFactoryBuilder()
                .SetConnectionstringFormat("Data Source=.;Initial Catalog=SticosPersonalKunde_{0};Integrated Security=True;")
                .WithCurrentCustomerId(customerId)
                .Build();

            var repository = new UnitCompanyLegacyDbEfRepositoryBuilder()
                .WithMapper(AutoMapperSetup.Config.CreateMapper())
                .WithDbFactory(factory)
                .Build();

            var controller = new UnitServiceBuilder()
                .WithCompanyRepository(repository)
                .Build();
            return controller;
        }
    }
}
