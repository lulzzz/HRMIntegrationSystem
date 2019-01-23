using NUnit.Framework;
using Shared.Contracts;
using Shared.TestCommon;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrations.Api.Tests.ControllerTests.IntegrationController
{
    [TestFixture]
    public class IntegrationCategoryControllerTests : IntegrationsApiTestsBase
    {
        [Test]
        public async Task GetCategories()
        {
            // Act
            var categories = await _client.GetAsyncAndDeserialize<IEnumerable<Code>>($"{_customerId}/integrations/categories");

            // Assert
            Assert.NotNull(categories);
        }

    }
}
