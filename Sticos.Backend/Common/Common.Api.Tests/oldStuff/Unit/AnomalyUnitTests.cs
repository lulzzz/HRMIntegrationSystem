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
    public class AnomalyUnitTests : BaseUnitTests
    {
        private IAnomalyService Service => ServicesProvider.GetService<IAnomalyService>();
        private AnomalyController Controller => new AnomalyController(Service, Mapper);

        private const string Responsible = "Jimmy'sBrother Humunukuapua";
        private const string Location = "Kitchen";

        [TestCase(50)]
        [TestCase(200)]
        [TestCase(99)]
        [TestCase(101)]
        [TestCase(0)]
        public async Task NoQuerySearchAnomalies(int resultCount)
        {
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(resultCount);

            var query = new SearchQueryAnomaly();
            var response = await Controller.Search(query);

            var result = CustomAssert.AssertOkResponseCount(response, Math.Min(DefaultTake, resultCount));
            if (resultCount > 0)
                Assert.AreEqual(result.First().Id, 1);
            else
                Assert.AreEqual(result.Count(), 0);
        }

        [TestCase(50, 10, 10)]
        [TestCase(50, 45, 10)]
        [TestCase(10, 10, 2)]
        [TestCase(10, 12, 2)]
        [TestCase(10, 7, 2)]
        [TestCase(0, 2, 2)]
        public async Task SkipTakeAnomalies(int resultCount, int skip, int take)
        {
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(resultCount);
            var expectedCount = Math.Min(resultCount - skip, take);
            expectedCount = expectedCount < 0 ? 0 : expectedCount;
            var id = Service.Anomalies.Skip(skip).FirstOrDefault()?.Id;

            var query = new SearchQueryAnomaly {Skip = skip, Take = take};
            var response = await Controller.Search(query);

            var result = CustomAssert.AssertOkResponseCount(response, expectedCount);
            if (id.HasValue)
                Assert.AreEqual(result.First().Id, id.Value);
            else
                Assert.AreEqual(result.Count, 0);
        }

        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public async Task FilterByLocationNullWhitespace(string location)
        {
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(30);

            var query = new SearchQueryAnomaly {Location = location};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 30);
        }

        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public async Task FilterByResponsibleNullWhitespace(string responsible)
        {
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(30);

            var query = new SearchQueryAnomaly {Responsible = responsible};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 30);
        }

        [Test]
        public async Task FilterByLocationExisting()
        {
            var overrides = new Dictionary<string, object> {{"Location", Location}};
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(20);
            Service.Anomalies.AddRange(AnomalityFactory.GetFactory(overrides, 21).Generate(30));

            var query = new SearchQueryAnomaly {Location = Location};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 30);
        }

        [Test]
        public async Task FilterByLocationNoneExisting()
        {
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(40);

            var query = new SearchQueryAnomaly {Location = Location};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 0);
        }

        [Test]
        public async Task FilterByLocationPartial()
        {
            var overrides = new Dictionary<string, object> {{"Location", Location}};
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(20);
            Service.Anomalies.AddRange(AnomalityFactory.GetFactory(overrides, 21).Generate(30));

            var query1 = new SearchQueryAnomaly {Location = Location.Split(" ").Last()};
            var response1 = await Controller.Search(query1);
            var query2 = new SearchQueryAnomaly {Location = Location.Split(" ").First()};
            var response2 = await Controller.Search(query2);
            var query3 = new SearchQueryAnomaly {Location = Location.Substring(2, Location.Length - 3)};
            var response3 = await Controller.Search(query3);

            CustomAssert.AssertOkResponseCount(response1, 30);
            CustomAssert.AssertOkResponseCount(response2, 30);
            CustomAssert.AssertOkResponseCount(response3, 30);
        }

        [Test]
        public async Task FilterByResponsibleCaseInsensitive()
        {
            var overrides = new Dictionary<string, object> {{"Responsible", Responsible}};
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(20);
            Service.Anomalies.AddRange(AnomalityFactory.GetFactory(overrides, 21).Generate(30));

            var query1 = new SearchQueryAnomaly {Responsible = Responsible.ToLower()};
            var response1 = await Controller.Search(query1);
            var query2 = new SearchQueryAnomaly {Responsible = Responsible.ToUpper()};
            var response2 = await Controller.Search(query2);

            CustomAssert.AssertOkResponseCount(response1, 30);
            CustomAssert.AssertOkResponseCount(response2, 30);
        }

        [Test]
        public async Task FilterByResponsibleExisting()
        {
            var overrides = new Dictionary<string, object> {{"Responsible", Responsible}};
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(20);
            Service.Anomalies.AddRange(AnomalityFactory.GetFactory(overrides, 21).Generate(30));

            var query = new SearchQueryAnomaly {Responsible = Responsible};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 30);
        }

        [Test]
        public async Task FilterByResponsibleNoneExisting()
        {
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(40);

            var query = new SearchQueryAnomaly {Responsible = Responsible};
            var response = await Controller.Search(query);

            CustomAssert.AssertOkResponseCount(response, 0);
        }

        [Test]
        public async Task FilterByResponsiblePartial()
        {
            var overrides = new Dictionary<string, object> {{"Responsible", Responsible}};
            Service.Anomalies = AnomalityFactory.GetFactory().Generate(20);
            Service.Anomalies.AddRange(AnomalityFactory.GetFactory(overrides, 21).Generate(30));

            var query1 = new SearchQueryAnomaly {Responsible = Responsible.Split(" ").Last()};
            var response1 = await Controller.Search(query1);
            var query2 = new SearchQueryAnomaly {Responsible = Responsible.Split(" ").First()};
            var response2 = await Controller.Search(query2);
            var query3 = new SearchQueryAnomaly {Responsible = Responsible.Substring(2, Responsible.Length - 5)};
            var response3 = await Controller.Search(query3);

            CustomAssert.AssertOkResponseCount(response1, 30);
            CustomAssert.AssertOkResponseCount(response2, 30);
            CustomAssert.AssertOkResponseCount(response3, 30);
        }
    }
}