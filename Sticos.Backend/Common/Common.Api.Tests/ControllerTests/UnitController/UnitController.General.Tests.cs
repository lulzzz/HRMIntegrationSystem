using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using contract = Common.Api.Contracts;
using NUnit.Framework;
using Shared.TestCommon;
using db = Common.Api.Repositories.Legacy.Models;
using System.Linq;

namespace Common.Api.Tests.ControllerTests
{
    [TestFixture]
    public class UnitControllerGeneralTests : UnitControllerTestsBase
    {
        [Test]
        public async Task WhenRequestingUnitById_AndTypeIsUnit_ThenItShouldBeReturned()
        {
            var id = new Random().Next(1, 1000);
            var url = $"{_customerId}/units/{id}";
            var unitType = db.UnitType.Enhet; //Type = Unit
            await AddToPersonalDb(new List<db.Unit> { new db.Unit { Id = id, Type = unitType } });

            var unit = await _client.GetAsyncAndDeserialize<contract.Unit>(url);

            Assert.IsNotNull(unit);
            Assert.AreEqual(id, unit.Id);
        }

        [Test]
        public async Task WhenRequestingUnitById_AndTypeIsDepartment_ThenItShouldBeReturned()
        {
            var id = new Random().Next(1, 1000);
            var url = $"{_customerId}/units/{id}";
            var unitType = db.UnitType.Avdeling; //Type==Department
            await AddToPersonalDb(new List<db.Unit> { new db.Unit { Id = id, Type = unitType } });

            var unit = await _client.GetAsyncAndDeserialize<contract.Unit>(url);

            Assert.IsNotNull(unit, url);
            Assert.AreEqual(id, unit.Id);
        }

        [Test]
        public async Task WhenRequestingUnitById_AndOrgNrVerificationIsMissing_ThenOrgNumberShouldBeEmpty()
        {
            var id = new Random().Next(1, 1000);
            var url = $"{_customerId}/units/{id}";
            var unitType = db.UnitType.Avdeling;
            await AddToPersonalDb(new List<db.Unit> { new db.Unit { Id = id, Type = unitType } });

            var unit = await _client.GetAsyncAndDeserialize<contract.Unit>(url);

            Assert.IsNotNull(unit);
            Assert.IsTrue(string.IsNullOrEmpty(unit.LegalOrganizationNumber));
            Assert.IsTrue(string.IsNullOrEmpty(unit.BusinessOrganizationNumber));
        }

        [Test]
        public async Task WhenRequestingUnitById_AndOrgNrVerificationIsPending_ThenOrgNumberShouldBeEmpty()
        {
            var id = new Random().Next(1, 1000);
            var url = $"{_customerId}/units/{id}";
            var unitType = db.UnitType.Avdeling;
            await AddToPersonalDb(new List<db.Unit> { new db.Unit { Id = id, Type = unitType } });
            var verificationState = db.OrgNrStatus.Pending; //not 2 which is Verified
            await AddToPersonalDb(new db.OrgUnitVerification { OrgUnitId = id, Status = verificationState });

            var unit = await _client.GetAsyncAndDeserialize<contract.Unit>(url);

            Assert.IsNotNull(unit);
            Assert.IsTrue(string.IsNullOrEmpty(unit.LegalOrganizationNumber));
            Assert.IsTrue(string.IsNullOrEmpty(unit.BusinessOrganizationNumber));
        }

        [Test]
        public async Task WhenRequestingUnitById_AndOrgNrVerificationIsVerified_ThenOrgNumbersShouldBeSet()
        {
            var id = new Random().Next(1, 1000);
            var url = $"{_customerId}/units/{id}";
            var unitType = db.UnitType.Avdeling;
            await AddToPersonalDb(new List<db.Unit> { new db.Unit { Id = id, Type = unitType } });
            var verificationState = db.OrgNrStatus.Verified; //not 2 which is Verified

            var legalNumber = "999999999";
            var businessNumber = "988888888";
            await AddToPersonalDb(new db.OrgUnitVerification
            {
                OrgUnitId = id,
                Status = verificationState,
                MainId = legalNumber,
                SubId = businessNumber,
            });

            var unit = await _client.GetAsyncAndDeserialize<contract.Unit>(url);

            Assert.IsNotNull(unit);
            Assert.AreEqual(legalNumber, unit.LegalOrganizationNumber);
            Assert.AreEqual(businessNumber, unit.BusinessOrganizationNumber);
        }
    }
}