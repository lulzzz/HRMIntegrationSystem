using NUnit.Framework;
using Shared.Contracts;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Altinn.Api.Tests.ControllerTests.ExternalSystemController
{
    public class ExternalSystemGetExternalDataTests : ExternalSystemSetup
    {
        [Test]
        public async Task GetExternalDataNotAuthorized()
        {
            var externalSystemId = 1;
            var externalDataResult = await _client.GetAsync($"externalsystems/{externalSystemId}/externaldata");

            Assert.NotNull(externalDataResult);
            Assert.AreEqual(HttpStatusCode.NotFound, externalDataResult.StatusCode);
        }

        [Test]
        public async Task GetExternalDataSucces()
        {
            var externalSystemId = 1;
            var numberOfFakeExternalData = 10;
            PopulateExternalData(numberOfFakeExternalData);
            var externalDataResult = await _client.GetAsyncAndDeserialize<IEnumerable<ExternalData>>($"{_customerId}/externalsystems/{externalSystemId}/externaldata");

            Assert.NotNull(externalDataResult);
            Assert.AreEqual(numberOfFakeExternalData, externalDataResult.Count());
        }
    }
}
