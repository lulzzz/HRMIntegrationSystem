using FakeItEasy;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using Shared.Contracts;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Shared.TestCommon;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Repositories.Context;
using Timereg.Api.Repositories.ContextFactory;
using Timereg.Api.Tests.ControllerTests.ExternalSystemController;

namespace Timereg.Api.UnitTests
{
    [TestFixture]
    public class ExternalSystemSearchTests : ExternalSystemSetup
    {
        [Test]
        public async Task SearchExternalSystemNotAuthorized()
        {
            var externalSystems = await _client.GetAsync("externalsystems");

            Assert.NotNull(externalSystems);
            Assert.AreEqual(HttpStatusCode.NotFound, externalSystems.StatusCode);
        }

        [Test]
        public async Task SearchExternalSystemSuccess()
        {
            var numberOfExternalSystems = 1;
            var externalSystems = await _client.GetAsyncAndDeserialize<IList<Code>>($"{CustomerId}/externalsystems");

            Assert.NotNull(externalSystems);
            Assert.AreEqual(numberOfExternalSystems, externalSystems.Count);
        }
      
    }
}
