using Newtonsoft.Json;
using NUnit.Framework;
using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Tests.ControllerTests.ExternalSystemController
{
    [TestFixture]
    public class ExternalSystemMatchEntities : ExternalSystemSetup
    {
       [Test]
       public async Task MatchEntitiesNotAuthorized()
        {
            var externalSystemId = 2;
            var externalSystems = await _client.GetAsync($"externalsystems/{externalSystemId}");

            Assert.AreEqual(HttpStatusCode.NotFound, externalSystems.StatusCode);
        }

        [Test]
        public async Task MatchEntitiesSuccess()
        {
            var externalSystemId = 2;
            var entityType = ExternalDataEnum.Employee;
            var firstEmployeeId = 1;

            var employeeName = Guid.NewGuid().ToString();
            AddEmployeeServiceData(employeeName, firstEmployeeId);
            var unit = ValidSticosUnit;
            this.WithSticosUnit(unit);
            var externalMatchingDataResult = await _client.GetAsync($"{CustomerId}/externalsystems/{externalSystemId}?unitId={unit.Id}&entity={entityType}&ids={firstEmployeeId}");

            Assert.NotNull(externalMatchingDataResult);
            Assert.AreEqual(HttpStatusCode.OK, externalMatchingDataResult.StatusCode);
        }

        [Test]
        public async Task MatchAbsenceTypeSuccess()
        {
            var externalSystemId = 2;
            var leaveType = (int)LeaveType.Military_service_leave;
            
            var entityType = (int)ExternalDataEnum.AbsenceCode;
            var absenceType = 206;
            var numberOfMatchedData = 1;
            var unit = ValidSticosUnit;
            AddMilitaryLeaveType(absenceType);
            this.WithSticosUnit(unit);

            var externalMatchingDataResult = await _client.GetAsync($"{CustomerId}/externalsystems/{externalSystemId}/matchentities?unitId={unit.Id}&entity={entityType}&ids={absenceType}");
            var externalMatchData = externalMatchingDataResult.Content.ReadAsStringAsync().Result;
            var matchedData = JsonConvert.DeserializeObject<IEnumerable<EntityMatch>>(externalMatchData);

            var matchedDataLeaveType = matchedData.FirstOrDefault().ExternalData.Identifiers.Select(x => x.Value == leaveType.ToString()).FirstOrDefault();
            Assert.NotNull(externalMatchingDataResult);
            Assert.AreEqual(HttpStatusCode.OK, externalMatchingDataResult.StatusCode);
            Assert.AreEqual(numberOfMatchedData, matchedData.Count());
            Assert.True(matchedDataLeaveType);
        }
    }
}
