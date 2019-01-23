using FakeItEasy;
using Integrations.Api.Domain.Models;
using Integrations.Api.MessageBus;
using Integrations.Api.Repositories.Context;
using Integrations.Api.Tests.ControllerTests.IntegrationController;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.TestCommon;
using Sticos.Personal.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Integrations.Api.Tests.ConsumerTests
{
    [TestFixture]
    public class EmployeeDeletedConsumerTests : IntegrationsApiTestsBase
    {
        [Test]
        public async Task EmployeeDeleted_DeleteEntityMaps()
        {
            var integrationId = 1;
            EmployeeDeleted message = new EmployeeDeleted
            {
                EmployeeId = 1,
                OrgUnitId = 12345,
                UserId = 10,
                CustomerId = 10
            };
            var fakeContext = A.Fake<ConsumeContext<IEmployeeDeleted>>();
            A.CallTo(() => fakeContext.Message).Returns(message);
            var (consumer, context) = await CreateEmployeeDeleteSetup();

            context.Integrations.Add(new Repositories.Models.Integration { Id = integrationId, UnitId = 12345 });

            context.EntityMaps.AddRange(GenerateFakeEntityMaps(10, message.EmployeeId, integrationId));
            context.SaveChanges();

            await consumer.Consume(fakeContext);

            Assert.AreEqual(0, context.EntityMaps.Count(), "Employee entity maps are not deleted");
        }

        [Test]
        public async Task OtherEmployeeDeleted_DeleteEntityMaps()
        {
            var integrationId = 2;
            EmployeeDeleted message = new EmployeeDeleted
            {
                EmployeeId = 1,
                OrgUnitId = 54321,
                UserId = 10,
                CustomerId = 10
            };
            var fakeContext = A.Fake<ConsumeContext<IEmployeeDeleted>>();
            A.CallTo(() => fakeContext.Message).Returns(message);
            var (consumer, context) = await CreateEmployeeDeleteSetup();

            context.Integrations.Add(new Repositories.Models.Integration { Id = integrationId, UnitId = 54321 });

            context.EntityMaps.AddRange(GenerateFakeEntityMaps(5, message.EmployeeId, integrationId));
            context.EntityMaps.AddRange(GenerateFakeEntityMaps(5, message.EmployeeId + 10, integrationId));

            context.SaveChanges();

            await consumer.Consume(fakeContext);

            Assert.AreEqual(5, context.EntityMaps.Count(), "Employee entity maps are not deleted");
         
        }
        private IEnumerable<Repositories.Models.EntityMap> GenerateFakeEntityMaps(int quantity, int employeeId, int integrationId)
        {
          return A.CollectionOfFake<Repositories.Models.EntityMap>(quantity).Select(x => {
              x.EntityId = employeeId;
              x.IntegrationId = integrationId;
              x.EntityName = EntityType.Employee.ToString();
              return x; });
        }
        private async Task<(IConsumer<IEmployeeDeleted>, IntegrationDbContext)> CreateEmployeeDeleteSetup()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Common_Api_Url", "http://localhost");
            Action<IServiceCollection> actions = (sc) =>
            {
                _actions(sc);
                sc.AddSingleton<IConsumer<IEmployeeDeleted>, EmployeeDeletedConsumer>();                

            };
            var testServer = new TestServerBuilder()
                .WithPostConfigureCollection(actions)
                .WithConfigSettings(settings)
                .Build<Startup>();
            var client = testServer.CreateClientWithJwtToken(_customerId, _userId);
            var dbFactory = testServer.Host.Services.GetService<IDbContextFactory<IntegrationDbContext>>();
            var context = await dbFactory.CreateDbContext();
            var consumer = testServer.Host.Services.GetService<IConsumer<IEmployeeDeleted>>();
            return (consumer, context);
        }

    }
   
}
