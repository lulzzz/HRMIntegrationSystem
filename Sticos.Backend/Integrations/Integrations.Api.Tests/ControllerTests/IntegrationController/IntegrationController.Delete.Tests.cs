using FakeItEasy;
using Integrations.Api.Repositories.Context;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.MessageBus.Contracts;
using Shared.Services.Extensions;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Api.Tests.ControllerTests.IntegrationController
{
    [TestFixture]
    public class IntegrationControllerDeleteTests : IntegrationsApiTestsBase
    {
        [Test]
        public async Task Delete()
        {
            var (context, client, publishEndpoint) = await CreateIntegrationDeleteSetup();
            var integrations = A.CollectionOfFake<Repositories.Models.Integration>(2);
            context.Integrations.AddRange(integrations);
            await context.SaveChangesAsync();
            foreach (var integration in integrations)
            {
                var fakeEntityMaps = A.CollectionOfFake<Repositories.Models.EntityMap>(10)
                    .Select(x =>
                    {
                        x.IntegrationId = integration.Id;
                        return x;
                    }).ToList();
                context.EntityMaps.AddRange(fakeEntityMaps);
            }
            await context.SaveChangesAsync();
            var integrationForDelete = integrations.FirstOrDefault();
            var otherIntegrationId = integrations.LastOrDefault().Id;

            var response = await client.DeleteAsync($"{_customerId}/integrations?id={integrationForDelete.Id}");

            response.EnsureSuccessStatusCode();
            Assert.IsNull(context.Integrations.FirstOrDefault(x => x.Id == integrationForDelete.Id), "Integration not deleted");
            Assert.IsEmpty(context.EntityMaps.Where(x => x.IntegrationId == integrationForDelete.Id).ToList(), "Entity maps not deleted");
            Assert.IsNotNull(context.Integrations.FirstOrDefault(x => x.Id == otherIntegrationId), "Wrong integration deleted");
            Assert.IsNotEmpty(context.EntityMaps.Where(x => x.IntegrationId == otherIntegrationId).ToList(), "Wrong entity maps deleted");
            A.CallTo(() => publishEndpoint.Publish<IIntegrationDeleted>(
                A<object>.That.Matches(x => IntegrationMessage(x, integrationForDelete, _customerId)), A<System.Threading.CancellationToken>.Ignored))
                .MustHaveHappened();
        }

        private bool IntegrationMessage(object published, Repositories.Models.Integration integrationForDelete, int customerId)
        {
            Assert.AreEqual(integrationForDelete.Id, GetValue(published, nameof(IIntegrationDeleted.Id)), "Integration ID not set on IIntegrationDeleted message");
            Assert.AreEqual(integrationForDelete.UnitId, GetValue(published, nameof(IIntegrationDeleted.UnitId)), "Unit ID not set on IIntegrationDeleted message");
            Assert.AreEqual(integrationForDelete.ExternalSystem, GetValue(published, nameof(IIntegrationDeleted.ExternalSystem)), "ExternalSystem not set on IIntegrationDeleted message");
            Assert.AreEqual(integrationForDelete.Category, GetValue(published, nameof(IIntegrationDeleted.Category)), "Category not set on IIntegrationDeleted message");
            Assert.AreEqual(customerId, GetValue(published, nameof(IIntegrationDeleted.CustomerId)), "Customer ID not set on IIntegrationDeleted message");

            return true;
        }

        // Helper method used in when matching the messages sent in messagepublisher
        private object GetValue(object anon, string prop)
        {
            return anon.GetType().GetProperty(prop)?.GetValue(anon);
        }

        private async Task<(IntegrationDbContext, HttpClient, IPublishEndpoint)> CreateIntegrationDeleteSetup()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Common_Api_Url", "http://localhost");
            Action<IServiceCollection> actions = (sc) =>
            {
                _actions(sc);
                sc.Remove<IPublishEndpoint>();

                var fakePublishEndpoint = A.Fake<IPublishEndpoint>();
                sc.AddSingleton(fakePublishEndpoint);
            };
            var testServer = new TestServerBuilder()
                .WithPostConfigureCollection(actions)
                .WithConfigSettings(settings)
                .Build<Startup>();
            var client = testServer.CreateClientWithJwtToken(_customerId, _userId);
            var dbFactory = testServer.Host.Services.GetService<IDbContextFactory<IntegrationDbContext>>();
            var context = await dbFactory.CreateDbContext();
            var publishEndpoint = testServer.Host.Services.GetService<IPublishEndpoint>();
            return (context, client, publishEndpoint);
        }
    }
}
