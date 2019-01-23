using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Extensions;
using Common.Api.Repositories.Legacy.Factories;
using NUnit.Framework;
using Shared.Domain.Enums;
using TestCommon.Builders;

namespace Common.Api.IntegrationTests
{
    [Ignore("only for ad hoc testing")]
    [TestFixture]
    public class UnitRepositoryEfTests
    {
        [Test]
        public async Task SearchUnits()
        {
            var repository = CreateRepository();
            var units = await repository.Search(new SearchQueryUnit());
            var tesla = units.FirstOrDefault(u => u.Id ==72);
            Assert.AreEqual(62,units.Count);

            units = await repository.Search(new SearchQueryUnit {Skip = 0,Take = 100});
            Assert.AreEqual(62,units.Count);

            units = await repository.Search(new SearchQueryUnit {UnitIds = new List<int>{13}});
            Assert.AreEqual(1,units.Count);

            units = await repository.Search(new SearchQueryUnit {UnitIds = new List<int>{1}});
            Assert.AreEqual(1,units.Count);

            units = await repository.Search(new SearchQueryUnit {UnitIds = new List<int>{72}});
            Assert.AreEqual(1,units.Count);

            units = await repository.Search(new SearchQueryUnit {UnitIds = new List<int>{72,73,
                74,
                78,
                80,
                75,
                76,
                77,
                81,
                82,
                1109,
                83,
                84,
                85,
                88}});
            tesla = units.FirstOrDefault(u => u.Id ==72);
            

            units = await repository.Search(new SearchQueryUnit {UnitTypes = new List<int>{(int)UnitType.Company}});
            Assert.AreEqual(1,units.Count);
        }

        private static IUnitRepository CreateRepository()
        {
            var customerId = 125134;

            var factory = new PersonalLegacyContextFactoryBuilder()
                //.SetConnectionstringFormat("Data source=\.;Initial Catalog=SticosPersonalKunde_{0};User ID=sa;Password=xxx!")
                .WithCurrentCustomerId(customerId)
                .Build();

            var repository = new UnitCompanyLegacyDbEfRepositoryBuilder()
                .WithMapper(AutoMapperSetup.Config.CreateMapper())
                .WithDbFactory(factory)
                .Build();

            return repository;
        }
    }
}
