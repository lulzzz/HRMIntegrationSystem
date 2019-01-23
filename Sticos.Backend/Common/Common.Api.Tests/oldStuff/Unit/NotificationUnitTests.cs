using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Api.Contracts;
using Common.Api.Controllers;
using Common.Api.Domain.Interfaces;
using Common.Api.Tests.Factories.Domain;
using Common.Api.Tests.Helpers;
using FakeItEasy;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Common.Api.Tests.Unit
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class NotificationUnitTests : BaseUnitTests
    {
        private const string Title = "New Fanta Bambucha Commercial";
        private INotificationService Service => ServicesProvider.GetService<INotificationService>();
        private NotificationController Controller => new NotificationController(Service, Mapper);

        [TestCase(50)]
        [TestCase(200)]
        [TestCase(99)]
        [TestCase(101)]
        [TestCase(0)]
        public async Task NoQuerySearchNotification(int resultCount)
        {
            Service.Notifications = NotificationFactory.GetFactory().Generate(resultCount);

            var query = new SearchQueryNotification();
            var response = await Controller.Search(query);

            var result = CustomAssert.AssertOkResponseCount(response, Math.Min(DefaultTake, resultCount));
            if (resultCount > 0)
                Assert.AreEqual(result.First().Id, 1);
            else
                Assert.AreEqual(result.Count, 0);
        }

        [TestCase(50, 10, 10)]
        [TestCase(50, 45, 10)]
        [TestCase(10, 10, 2)]
        [TestCase(10, 12, 2)]
        [TestCase(10, 7, 2)]
        [TestCase(0, 2, 2)]
        public async Task SkipTakeNotification(int resultCount, int skip, int take)
        {
            Service.Notifications = NotificationFactory.GetFactory().Generate(resultCount);
            var expectedCount = Math.Min(resultCount - skip, take);
            expectedCount = expectedCount < 0 ? 0 : expectedCount;
            var id = Service.Notifications.Skip(skip).FirstOrDefault()?.Id;

            var query = new SearchQueryNotification {Skip = skip, Take = take};
            var response = await Controller.Search(query);

            var result = CustomAssert.AssertOkResponseCount(response, expectedCount);
            if (id.HasValue)
                Assert.AreEqual(result.First().Id, id.Value);
            else
                Assert.AreEqual(result.Count, 0);
        }

        [Test]
        public async Task FilterByTitleExisting()
        {
            var overrides = new Dictionary<string, object> {{"Title", Title}};
            Service.Notifications = NotificationFactory.GetFactory().Generate(20);
            Service.Notifications.AddRange(NotificationFactory.GetFactory(overrides, 21).Generate(30));

            var query = new SearchQueryNotification {Title = Title};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 30);
        }

        [Test]
        public async Task FilterByTitleNoneExisting()
        {
            Service.Notifications = NotificationFactory.GetFactory().Generate(40);

            var query = new SearchQueryNotification {Title = Title};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 0);
        }

        [Test]
        public async Task FilterByTitlePartial()
        {
            var overrides = new Dictionary<string, object> {{"Title", Title}};
            Service.Notifications = NotificationFactory.GetFactory().Generate(20);
            Service.Notifications.AddRange(NotificationFactory.GetFactory(overrides, 21).Generate(30));

            var query1 = new SearchQueryNotification {Title = Title.Split(" ").Last()};
            var response1 = await Controller.Search(query1);
            var query2 = new SearchQueryNotification {Title = Title.Split(" ").First()};
            var response2 = await Controller.Search(query2);
            var query3 = new SearchQueryNotification {Title = Title.Substring(2, Title.Length - 3)};
            var response3 = await Controller.Search(query3);

            CustomAssert.AssertOkResponseCount(response1, 30);
            CustomAssert.AssertOkResponseCount(response2, 30);
            CustomAssert.AssertOkResponseCount(response3, 30);
        }

        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public async Task FilterByTitleNullWhitespace(string title)
        {
            Service.Notifications = NotificationFactory.GetFactory().Generate(30);

            var query = new SearchQueryNotification {Title = title};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 30);
        }
    }
}