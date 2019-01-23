using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Common.Api.Contracts;
using Common.Api.Controllers;
using Common.Api.Domain.Interfaces;
using Common.Api.Repositories.Context;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Tests.Factories.Repositories;
using Common.Api.Tests.Helpers;
using shared = Shared.Exceptions;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Shared.Services.Models;

namespace Common.Api.Tests.Unit
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class DashboardUnitTests : BaseUnitTests
    {
        [TearDown]
        public async Task Cleanup()
        {
            using (var context = await ContextFactory.CreateDbContext())
            {
                context.Dashboards.RemoveRange(context.Dashboards.AsNoTracking());
                context.SaveChanges();
            }
        }

        public IDashboardService Service => ServicesProvider.GetService<IDashboardService>();
        public ICurrentUserContext CurrentUserContext => ServicesProvider.GetService<ICurrentUserContext>();
        public IOwnerTypeService OwnerTypeService => ServicesProvider.GetService<IOwnerTypeService>();
        public DashboardController Controller => new DashboardController(Service, Mapper, CurrentUserContext, OwnerTypeService);

        public IDbContextFactory<SticosWidgetDbContext> ContextFactory =>
            ServicesProvider.GetService<IDbContextFactory<SticosWidgetDbContext>>();

        public string LongText => new Faker().Random.String(256);

        [TestCase(50, 10, 10)]
        [TestCase(50, 45, 10)]
        [TestCase(10, 10, 2)]
        [TestCase(10, 12, 2)]
        [TestCase(10, 7, 2)]
        [TestCase(0, 2, 2)]
        public async Task SkipTakeDashboards(int resultCount, int skip, int take)
        {
            var expectedCount = Math.Min(resultCount - skip, take);
            expectedCount = expectedCount < 0 ? 0 : expectedCount;
            int? id;
            var userContext = new UserContext()
            {
                UserId = 1
            };
            var currentUserContext = A.Fake<ICurrentUserContext>();
            A.CallTo(() => currentUserContext.Get()).Returns(userContext);
           
            var controller = new DashboardController(Service, Mapper, currentUserContext, OwnerTypeService);
            using (var context = await ContextFactory.CreateDbContext())
            {

                context.Dashboards.AddRange(DashboardFactory
                    .GetFactory(new Dictionary<string, object> { { "OwnerTypeId", 1 }, { "OwnerId", userContext.UserId } }).Generate(resultCount));
                context.SaveChanges();

                id = context.Dashboards.Skip(skip).FirstOrDefault()?.Id;
            }



            var response =
                await controller.Search(new SearchQueryDashboard { Skip = skip, Take = take });

            var result = CustomAssert.AssertOkResponseCount(response, expectedCount);
            if (id.HasValue)
                Assert.AreEqual(result.First().Id, id.Value);
            else
                Assert.AreEqual(result.Count, 0);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-1000)]
        public void DeleteDashboardBadRequest(int id)
        {
            Assert.ThrowsAsync<shared.ValidationException>(async () => await Controller.DeleteDashboard(id));
        }


        [Test]
        public async Task CreateDashboard()
        {
            var insertedValues = new Dictionary<string, object>
            {
                {"Title", "Inserted title"},
                {"OwnerTypeId", 2},
                {"DashboardConfig", "Inserted Dash Config"}
            };

            var response1 = await Controller.CreateDashboard(new Dashboard
            {
                Title = insertedValues["Title"] as string,
                OwnerTypeId = (int)insertedValues["OwnerTypeId"],
                DashboardConfig = insertedValues["DashboardConfig"] as string
            });

            var result1 = CustomAssert.AssertCreatedAtResponse(response1);
            Assert.AreEqual(result1.Title, insertedValues["Title"] as string);
            Assert.AreEqual(result1.OwnerTypeId, (int)insertedValues["OwnerTypeId"]);
            Assert.AreEqual(result1.DashboardConfig, insertedValues["DashboardConfig"] as string);

            // ReSharper disable once PossibleInvalidOperationException
            var response2 = await Controller.GetDashboard(result1.Id.Value);

            var result2 = CustomAssert.AssertOkResponse(response2);
            Assert.AreEqual(result2.Title, insertedValues["Title"] as string);
            Assert.AreEqual(result2.OwnerTypeId, (int)insertedValues["OwnerTypeId"]);
            Assert.AreEqual(result2.DashboardConfig, insertedValues["DashboardConfig"] as string);
        }

        [Test]
        public async Task CreateDashboardDashboardConfigValidation()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<CommonTestStartUp>());
            var client = server.CreateClient();
            var data1 = new { Title = "String", OwnerTypeId = 0, OwnerId = 0 };
            var data2 = new { Title = "String", DashboardConfig = LongText, OwnerTypeId = 1, OwnerId = 0 };

            var response1 = await client.PostAsJsonAsync("api/dashboards", data1);
            var response2 = await client.PostAsJsonAsync("api/dashboards", data2);
            var response1Body = await response1.Content.ReadAsStringAsync();

            Assert.AreEqual(response1.StatusCode, HttpStatusCode.BadRequest);
            //This is testing no limit on Dashboard config (since json can be big)
            Assert.AreEqual(response2.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(response1Body, "{\"DashboardConfig\":[\"The DashboardConfig field is required.\"]}");
        }

        [Test]
        public async Task CreateDashboardPassId()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<CommonTestStartUp>());
            var client = server.CreateClient();
            var faker = new Faker();
            var longTitle = faker.Random.String(256);
            var data = new { Id = 1, Title = "String", DashboardConfig = longTitle, OwnerTypeId = 0, OwnerId = 0 };

            var response1 = await client.PostAsJsonAsync("api/dashboards", data);

            Assert.AreEqual(response1.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateDashboardTitleValidation()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<CommonTestStartUp>());
            var client = server.CreateClient();
            var data1 = new { Id = 0, DashboardConfig = "String", OwnerTypeId = 0, OwnerId = 0 };
            var data2 = new { Id = 0, Title = LongText, DashboardConfig = "String", OwnerTypeId = 0, OwnerId = 0 };

            var response1 = await client.PostAsJsonAsync("api/dashboards", data1);
            var response2 = await client.PostAsJsonAsync("api/dashboards", data2);
            var response1Body = await response1.Content.ReadAsStringAsync();
            var response2Body = await response2.Content.ReadAsStringAsync();

            Assert.AreEqual(response1.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(response2.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(response1Body, "{\"Title\":[\"The Title field is required.\"]}");
            Assert.AreEqual(response2Body,
                "{\"Title\":[\"The field Title must be a string with a maximum length of 255.\"]}");
        }

        [Test]
        public async Task DeleteDashboard()
        {
            int id;
            using (var context = await ContextFactory.CreateDbContext())
            {
                context.Dashboards.AddRange(DashboardFactory.GetFactory().Generate());
                context.SaveChanges();

                id = context.Dashboards.First().Id;
            }

            var response1 = await Controller.DeleteDashboard(id);

            CustomAssert.AssertStatusCodeResult<NoContentResult>(response1, 204);
            Assert.ThrowsAsync<shared.NotFoundException>(async () => await Controller.GetDashboard(id));
        }

        [Test]
        public void DeleteDashboardNotFound()
        {
            Assert.ThrowsAsync<shared.NotFoundException>(async () => await Controller.DeleteDashboard(1));
        }

        [Test]
        public void DeleteDashboardRetrowException()
        {
            var service = A.Fake<IDashboardService>();
            A.CallTo(() => service.Delete(A<int>._)).Throws<Exception>();
            var controller = new DashboardController(service, Mapper, CurrentUserContext, OwnerTypeService);

            Assert.ThrowsAsync<Exception>(async () => await controller.DeleteDashboard(1));
        }

        [Test]
        public async Task UpdateDashboard()
        {
            int id;
            var overrides = new Dictionary<string, object>
            {
                {"Title", "Dashboard"},
                {"OwnerTypeId", 1},
                {"DashboardConfig", "Some dash config"}
            };
            var updatedValues = new Dictionary<string, object>
            {
                {"Title", "Updated title"},
                {"OwnerTypeId", 2},
                {"DashboardConfig", "Updated Dash Config"}
            };
            using (var context = await ContextFactory.CreateDbContext())
            {
                context.Dashboards.AddRange(DashboardFactory.GetFactory(overrides).Generate());
                context.SaveChanges();

                id = context.Dashboards.First().Id;
            }

            var response1 = await Controller.UpdateDashboard(new Dashboard
            {
                Id = id,
                Title = updatedValues["Title"] as string,
                OwnerTypeId = (int)updatedValues["OwnerTypeId"],
                DashboardConfig = updatedValues["DashboardConfig"] as string
            });
            var response2 = await Controller.GetDashboard(id);

            var result1 = CustomAssert.AssertOkResponse(response1);
            Assert.AreEqual(result1.Title, updatedValues["Title"] as string);
            Assert.AreEqual(result1.OwnerTypeId, (int)updatedValues["OwnerTypeId"]);
            Assert.AreEqual(result1.DashboardConfig, updatedValues["DashboardConfig"] as string);

            var result2 = CustomAssert.AssertOkResponse(response2);

            Assert.AreEqual(result2.Title, updatedValues["Title"] as string);
            Assert.AreEqual(result2.OwnerTypeId, (int)updatedValues["OwnerTypeId"]);
            Assert.AreEqual(result2.DashboardConfig, updatedValues["DashboardConfig"] as string);
        }

        [Test]
        public async Task UpdateDashboardDashboardConfigValidation()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<CommonTestStartUp>());
            var client = server.CreateClient();
            var faker = new Faker();
            var longTitle = faker.Random.String(256);
            var data1 = new { Id = 0, Title = "String", OwnerTypeId = 1, OwnerId = 0 };
            var data2 = new { Id = 0, Title = "String", DashboardConfig = longTitle, OwnerTypeId = 1, OwnerId = 0 };

            var response1 = await client.PutAsJsonAsync("api/dashboards", data1);
            var response2 = await client.PutAsJsonAsync("api/dashboards", data2);
            var response1Body = await response1.Content.ReadAsStringAsync();

            Assert.AreEqual(response1.StatusCode, HttpStatusCode.BadRequest);
            //This is testing no limit on Dashboard config (since json can be big)
            Assert.AreEqual(response2.StatusCode, HttpStatusCode.NotFound);
            Assert.AreEqual(response1Body, "{\"DashboardConfig\":[\"The DashboardConfig field is required.\"]}");
        }

        [Test]
        public async Task UpdateDashboardTitleValidation()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<CommonTestStartUp>());
            var client = server.CreateClient();
            var data1 = new { Id = 0, DashboardConfig = "String", OwnerTypeId = 0, OwnerId = 0 };
            var data2 = new { Id = 0, Title = LongText, DashboardConfig = "String", OwnerTypeId = 0, OwnerId = 0 };

            var response1 = await client.PutAsJsonAsync("api/dashboards", data1);
            var response2 = await client.PutAsJsonAsync("api/dashboards", data2);
            var response1Body = await response1.Content.ReadAsStringAsync();
            var response2Body = await response2.Content.ReadAsStringAsync();

            Assert.AreEqual(response1.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(response2.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(response1Body, "{\"Title\":[\"The Title field is required.\"]}");
            Assert.AreEqual(response2Body,
                "{\"Title\":[\"The field Title must be a string with a maximum length of 255.\"]}");
        }
    }
}