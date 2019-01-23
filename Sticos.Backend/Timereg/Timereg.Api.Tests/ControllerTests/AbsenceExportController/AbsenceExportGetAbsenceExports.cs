using FakeItEasy;
using NUnit.Framework;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Timereg.Api.Contracts;
using entities = Timereg.Api.Repositories.Models;

namespace Timereg.Api.Tests.ControllerTests.AbsenceExportController
{
    [TestFixture]
    public class AbsenceExportGetAbsenceExports : AbsenceExportSetup
    {
        [Test]
        public async Task GetAbsenceExportsNotAuthorized()
        {
            var exportAbsence = await _client.GetAsync($"absenceexports");

            Assert.NotNull(exportAbsence);
            Assert.AreEqual(HttpStatusCode.NotFound, exportAbsence.StatusCode);
        }

        [Test]
        public async Task GetAbsenceExportsById()
        {
            string id = Guid.NewGuid().ToString();
            await AddRandomAbsenceExport(id, 0, 0);

            var absenceExportsFake = A.CollectionOfFake<entities.AbsenceExport>(10).ToList();

            await AddAbsenceExportRange(absenceExportsFake);
            var exportAbsence = (await _client.GetAsyncAndDeserialize<IEnumerable<AbsenceExport>>($"{_customerId}/absenceexports?Id={id}")).FirstOrDefault();

            Assert.NotNull(exportAbsence);
            Assert.AreEqual(id, exportAbsence.Id);
        }

        [Test]
        public async Task GetAbsenceExportsByUnitId()
        {
            int unitId = new Random().Next(100, 10000000);
            string firstId = Guid.NewGuid().ToString();
            string secondId = Guid.NewGuid().ToString();

            await AddRandomAbsenceExport(firstId, unitId, 0);
            await AddRandomAbsenceExport(secondId, unitId, 0);

            var absenceExportsFake = A.CollectionOfFake<entities.AbsenceExport>(10).ToList();
            await AddAbsenceExportRange(absenceExportsFake);

            var exportAbsences = (await _client.GetAsyncAndDeserialize<IEnumerable<AbsenceExport>>($"{_customerId}/absenceexports?UnitId={unitId}"));

            Assert.NotNull(exportAbsences);
            Assert.AreEqual(2, exportAbsences.Count());
        }

        [Test]
        public async Task GetAbsenceExportsByLocalId()
        {
            int localId = new Random().Next(100, 1000000);

            await AddRandomAbsenceExport("", 0, localId);

            var absenceExportsFake = A.CollectionOfFake<entities.AbsenceExport>(10).ToList();
            await AddAbsenceExportRange(absenceExportsFake);

            var exportAbsences = (await _client.GetAsyncAndDeserialize<IEnumerable<entities.AbsenceExport>>($"{_customerId}/absenceexports?LocalId={localId}")).FirstOrDefault();

            Assert.NotNull(exportAbsences);
            Assert.AreEqual(localId, exportAbsences.LocalAbsenceId);
        }

    }
}
