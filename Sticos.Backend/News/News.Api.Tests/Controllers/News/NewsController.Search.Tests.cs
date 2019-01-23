using NUnit.Framework;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCommon.Models;

namespace News.Api.Tests.Controllers.NewsController
{
    [TestFixture]
    public class NewsControllerSearchTests : NewsApiTestsBase
    {
        [Ignore("Can't get these up and running")]
        [Test]
        public async Task NoQuerySearchIntegrations()
        {
            int uniqueValue = new Random().Next(1000, 100000);

            await AddTodb(new List<Models.News>()
            {
                new Models.News { Id = 1 },
                new Models.News { Id = 2 },
                new Models.News { Id = 3 },
                new Models.News { Id = 4 }
            });

            // Act
            var result = await _client.GetAsyncAndDeserialize<ODataArrayModel<Models.News, int>>($"{BaseUrl}");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(4, result.Value.Count());
        }

        [Ignore("Can't get these up and running")]
        [Test]
        public async Task FilterByUnitId()
        {
            int id = 2;
            int uniqueValue = new Random().Next(1000, 100000);
            await AddTodb(new List<Models.News>()
            {
                new Models.News{},
                new Models.News{},
                new Models.News{},
                new Models.News{},
                new Models.News{},
            });

            // Act
            var result = await _client.GetAsyncAndDeserialize<ODataArrayModel<Models.News, int>>($"{BaseUrl}?$filter=unitId eq {id}");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(3, result.Value.Count());
        }

        [Ignore("Can't get these up and running")]
        [Test]
        public async Task GetSingleItem()
        {
            int id = new Random().Next(1000, 100000);
            int uniqueValue = new Random().Next(1000, 100000);
            await AddTodb(new List<Models.News>()
            {
                new Models.News{},
                new Models.News{},
                new Models.News{},
                new Models.News{},
                new Models.News{},
            });

            // Act
            var result = await _client.GetAsyncAndDeserialize<ODataModel<Models.News, int>>($"{BaseUrl}({id})");

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(id, result.Value.Id);
            //Assert.AreEqual(uniqueValue, result.UnitId);
        }

        private string BaseUrl
        {
            get { return $"odata/{_customerId}/news"; }
        }
    }
}
