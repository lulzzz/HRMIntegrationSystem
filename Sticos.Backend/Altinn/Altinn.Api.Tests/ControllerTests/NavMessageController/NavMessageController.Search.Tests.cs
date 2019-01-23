using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

using Shared.TestCommon;
using Altinn.Api.Domain.Entities;
using db = Altinn.Api.Repositories.Models;
using contracts = Altinn.Api.Contratcs;

namespace Altinn.Api.Tests.ControllerTests.NavMessageController
{
    [TestFixture]
    public class NavMessageControllerSearchTests : NavMessageControllerTestsBase
    {
        [Test]
        public async Task RequestingNavMessages_FilterByOrganizationNumber()
        {
            var organizationNumber = "811291102";
            await AddToAltinnDb(new db.NavMessage { BusinessOrganizationNumber = organizationNumber });

            var url = $"/1/navmessages?BusinessOrganizationNumber={organizationNumber}";

            // Act
            var navMessageList = await _client.GetAsyncAndDeserialize<List<contracts.NavMessage>>(url);

            //Assert
            Assert.NotNull(navMessageList);
            Assert.AreEqual(1, navMessageList.Count());
        }

        [Test]
        public async Task RequestingNavMessages_FilterByIntegrationType()
        {
            var integrationType = 1;
            await AddToAltinnDb(new db.NavMessage { IntegrationType = IntegrationType.Import });

            var url = $"/1/navmessages?IntegrationType={integrationType}";

            // Act
            var navMessageList = await _client.GetAsyncAndDeserialize<List<contracts.NavMessage>>(url);
          
            //Assert
            Assert.NotNull(navMessageList);
            Assert.AreEqual(1, navMessageList.Count());
        }

        [Test]
        public async Task RequestingNavMessages_FilterByWorkState()
        {
            var workState = 1;
            await AddToAltinnDb(new db.NavMessage { WorkState = WorkState.ReadyForProcessing });

            var url = $"/1/navmessages?WorkState={workState}";

            // Act
            var navMessageList = await _client.GetAsyncAndDeserialize<List<contracts.NavMessage>>(url);
            var navMessage = navMessageList.Where(a => a.WorkState == WorkState.ReadyForProcessing).ToList();

            //Assert
            Assert.NotNull(navMessageList);
            Assert.AreEqual(1, navMessageList.Count());
        }
    }
}
