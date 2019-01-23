using System.Threading.Tasks;
using Common.Api.Contracts;
using Common.Api.Controllers;
using Common.Api.Domain.Interfaces;
using Common.Api.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Common.Api.Tests.Unit
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class OwnerTypeUnitTests : BaseUnitTests
    {
        public IOwnerTypeService Service => ServicesProvider.GetService<IOwnerTypeService>();
        public OwnerTypeController Controller => new OwnerTypeController(Service, Mapper);

        [Test]
        public async Task NoQuerySearchOwnerType()
        {
            var query = new SearchQueryOwnerType();
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 3);
        }

        [Test]
        public async Task SearchByMaxPriority()
        {
            var query = new SearchQueryOwnerType {MaxPriority = 2};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 2);
        }

        [Test]
        public async Task SearchByMinPriority()
        {
            var query = new SearchQueryOwnerType {MinPriority = 2};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 2);
        }

        [Test]
        public async Task SearchByNameExisting()
        {
            var query = new SearchQueryOwnerType {Name = "Default"};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 1);
        }

        [Test]
        public async Task SearchByNameNoneExisting()
        {
            var query = new SearchQueryOwnerType {Name = "Test"};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 0);
        }
    }
}