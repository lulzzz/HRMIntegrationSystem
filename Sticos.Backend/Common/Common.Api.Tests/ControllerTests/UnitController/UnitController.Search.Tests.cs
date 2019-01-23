using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Api.Tests.Factories.Domain;
using contract = Common.Api.Contracts;
using NUnit.Framework;
using Shared.TestCommon;
using TestCommon.DataFactories;
using db = Common.Api.Repositories.Legacy.Models;

namespace Common.Api.Tests.ControllerTests
{
    [TestFixture]
    public class UnitControllerSearchTests : UnitControllerTestsBase
    {
        [Test]
        public async Task WhenSearchingWithUnitId_AndOrgNrVerificationIsVerified_ThenOrgNumbersShouldSet()
        {
            var id = new Random().Next(1, 1000);
            var url = $"{_customerId}/units";
            var legalNumber = "999999999";
            var businessNumber = "988888888";
            var unitType = db.UnitType.Avdeling;
            await AddToPersonalDb(new List<db.Unit> { new db.Unit { Id = id, Type = unitType } });
            var verificationState = db.OrgNrStatus.Verified; //not 2 which is Verified
            await AddToPersonalDb(new db.OrgUnitVerification { 
                OrgUnitId = id, 
                Status = verificationState, 
                MainId = legalNumber, 
                SubId = businessNumber});

            var units = await _client.GetAsyncAndDeserialize<IEnumerable<contract.Unit>>(url);

            Assert.IsNotNull(units);
            var unit = units.FirstOrDefault();
            Assert.AreEqual(legalNumber, unit.LegalOrganizationNumber);
            Assert.AreEqual(businessNumber, unit.BusinessOrganizationNumber);
        }
        [Test]
        public async Task WhenSearchingWithUnitId_AndOrgNrVerificationIsPending_ThenOrgNumberShouldBeEmpty()
        {
            var id = new Random().Next(1, 1000);
            var url = $"{_customerId}/units";
            var unitType = db.UnitType.Avdeling;
            await AddToPersonalDb(new List<db.Unit> {new db.Unit {Id = id, Type = unitType}});
            var verificationState = db.OrgNrStatus.Pending; //not 2 which is Verified
            await AddToPersonalDb(new db.OrgUnitVerification {OrgUnitId = id, Status = verificationState});

            var units = await _client.GetAsyncAndDeserialize<IEnumerable<contract.Unit>>(url);

            Assert.IsNotNull(units);
            var unit = units.FirstOrDefault();
            Assert.IsTrue(string.IsNullOrEmpty(unit.LegalOrganizationNumber));
            Assert.IsTrue(string.IsNullOrEmpty(unit.BusinessOrganizationNumber));
        }
    }
}
