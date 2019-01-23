using NUnit.Framework;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using contracts = Integrations.Api.Contracts;
using db = Integrations.Api.Repositories.Models;

namespace Integrations.Api.Tests.ControllerTests.IntegrationController
{
    [TestFixture]
    public class IntegrationControllerSearchTests : IntegrationsApiTestsBase
    {
        [Test]
        public async Task NoQuerySearchIntegrations()
        {
            int uniqueValue = new Random().Next(1000, 100000);

            await AddTodb(new List<db.Integration>()
            {
                new db.Integration{UnitId = uniqueValue},
                new db.Integration{UnitId = uniqueValue},
                new db.Integration{UnitId = uniqueValue},
                new db.Integration{UnitId = uniqueValue},
            });
            // Act
            var integrations = await _client.GetAsyncAndDeserialize<List<contracts.Integration>>($"{_customerId}/integrations");

            // Assert
            Assert.NotNull(integrations);
            Assert.AreEqual(4, integrations.Count(i => i.UnitId == uniqueValue));
        }

        [Test]
        public async Task FilterByCategory()
        {
            var category = 2;
            int uniqueValue = new Random().Next(1000, 100000);

            await AddTodb(new List<db.Integration>()
            {
                new db.Integration{Category = 1, UnitId = uniqueValue},
                new db.Integration{Category = category, UnitId = uniqueValue},
                new db.Integration{Category = category, UnitId = uniqueValue},
                new db.Integration{Category = category, UnitId = uniqueValue},
                new db.Integration{Category = 3, UnitId = uniqueValue},
            });
            // Act
            var integrations = await _client.GetAsyncAndDeserialize<List<contracts.Integration>>($"{_customerId}/integrations?category={category}");

            // Assert
            Assert.NotNull(integrations);
            Assert.AreEqual(3, integrations.Count(i => i.UnitId == uniqueValue));
        }

        [Test]
        public async Task FilterByUnitId()
        {
            int id = 2;
            int uniqueValue = new Random().Next(1000, 100000);
            await AddTodb(new List<db.Integration>()
            {
                new db.Integration{UnitId = 1, Category = uniqueValue},
                new db.Integration{UnitId = id, Category = uniqueValue},
                new db.Integration{UnitId = id, Category = uniqueValue},
                new db.Integration{UnitId = id, Category = uniqueValue},
                new db.Integration{UnitId = 3},
            });

            // Act
            var integrations = await _client.GetAsyncAndDeserialize<List<contracts.Integration>>($"{_customerId}/integrations?unitId={id}");

            // Assert
            Assert.NotNull(integrations);
            Assert.AreEqual(3, integrations.Count(i => i.Category == uniqueValue));
        }

        [Test]
        public async Task GetSingleIntegration()
        {
            int id = new Random().Next(1000, 100000);
            int uniqueValue = new Random().Next(1000, 100000);
            await AddTodb(new List<db.Integration>()
            {
                new db.Integration{UnitId = uniqueValue},
                new db.Integration{UnitId = uniqueValue},
                new db.Integration{UnitId = uniqueValue},
                new db.Integration{Id = id, UnitId = uniqueValue},
                new db.Integration{UnitId = uniqueValue},
            });

            // Act
            var integration = await _client.GetAsyncAndDeserialize<contracts.Integration>($"{_customerId}/integrations/{id}");

            // Assert
            Assert.NotNull(integration);
            Assert.AreEqual(id, integration.Id);
            Assert.AreEqual(uniqueValue, integration.UnitId);
        }

        [Test]
        public async Task TestCreateIntegration()
        {
            int uniqueValue = new Random().Next(1000, 100000);

            var integration = new contracts.Integration
            {
                IsActivated = false,
                UnitId = 6,
                Category = 1,
                ExternalSystem = uniqueValue,
            };

            // Act
            var createdIntegration = await _client.PostAsyncAndDeserialize<contracts.Integration, contracts.Integration>($"{_customerId}/integrations", integration);

            // Assert
            Assert.NotNull(createdIntegration);
            Assert.NotZero(createdIntegration.Id);

            var integrationInDb = _db.Integrations.FirstOrDefault(x => x.ExternalSystem == uniqueValue);
            Assert.NotNull(integrationInDb);
            Assert.NotZero(integrationInDb.Id);
            Assert.AreEqual(createdIntegration.Id, integrationInDb.Id);
        }
    }
}
