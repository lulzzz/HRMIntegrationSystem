using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Timereg.Api.Domain.Constants;
using System;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Tests.ControllerTests.AbsenceExportController
{
    [TestFixture]
    public class AbsenceExportResendTests : AbsenceExportSetup
    {

        [Test]
        public async Task WhenQueryAbsenceExportsWithUnknownCustomerId_ThenForbiddenShouldBeReturned()
        {
            var unknownCustomerId = 1234567;
            var exportAbsence = await _client.GetAsync($"{unknownCustomerId}/absenceexports");

            Assert.NotNull(exportAbsence);
            Assert.AreEqual(HttpStatusCode.Forbidden, exportAbsence.StatusCode);
        }
        [Test]
        public async Task WhenQueryingAbsenceExportsWithMissingCustomerId_ThenNotFoundShouldBeReturned()
        {
            var exportAbsence = await _client.GetAsync($"/absenceexports");

            Assert.NotNull(exportAbsence);
            Assert.AreEqual(HttpStatusCode.NotFound, exportAbsence.StatusCode);
        }

        [Test]
        public async Task WhenResendingAndAbsenceExportIdIsUnknown_ThenNotFoundShouldBeReturned()
        {
            var uniqueId = Guid.NewGuid().ToString();
            var absenceExportResult = await _client.GetAsync($"{_customerId}/absenceexports/{uniqueId}?action={AbsenceExportActions.Resend}");

            Assert.AreEqual(HttpStatusCode.NotFound, absenceExportResult.StatusCode);
        }

        [Test]
        public async Task WhenResendingAbsenceExport_ThenStateShouldBeSetToSuccess()
        {
           var absenceExport = AddInitialAbsence();
            var integrationId = AddValidIntegration(absenceExport.UnitId);
            AddValidEntityMapAbsenceCode(absenceExport.UnitId, integrationId, absenceExport.LocalAbsenceId);
            AddValidEntityMapEmployee(absenceExport.UnitId, integrationId, absenceExport.EmployeeId);

            var absenceExportResult = await _client.GetAsync($"{_customerId}/absenceexports/{absenceExport.Id}?action={AbsenceExportActions.Resend}");

            absenceExportResult.EnsureSuccessStatusCode();
            var absenceExportSaved = await GetAbsenceExport(absenceExport.Id);
            Assert.NotNull(absenceExportSaved);
            Assert.AreEqual(AbsenceExportStatus.Success, absenceExportSaved.Status);
            Assert.NotNull(absenceExportSaved.Absence.AbsenceEntries[0].ExternalId);
        }

        [Test]
        public async Task WhenResendingAbsenceExport_AndEmployeeMapIsMissing_ThenStateShouldBeFailed()
        {
            var absenceExport = AddInitialAbsence();
            var integrationId = AddValidIntegration(absenceExport.UnitId);
            AddValidEntityMapAbsenceCode(absenceExport.UnitId, integrationId, absenceExport.LocalAbsenceId);

            var absenceExportResult = await _client.GetAsync($"{_customerId}/absenceexports/{absenceExport.Id}?action={AbsenceExportActions.Resend}");

            absenceExportResult.EnsureSuccessStatusCode();

            var absenceExportSaved = await GetAbsenceExport(absenceExport.Id);

            Assert.AreEqual(AbsenceExportStatus.Failed, absenceExportSaved.Status);
            Assert.AreEqual($"Missing mapping from Employee to WorkRelation {absenceExport.EmployeeId}", absenceExportSaved.Message);
        }

        [Test]
        public async Task WhenResendingAbsenceExport_AndAbsenceCodeMapIsMissing_ThenStateShouldBeFailed()
        {
           var absenceExport = AddInitialAbsence();
            var integrationId = AddValidIntegration(absenceExport.UnitId);
            AddValidEntityMapEmployee(absenceExport.UnitId, integrationId, absenceExport.EmployeeId);

            var absenceExportResult = await _client.GetAsync($"{_customerId}/absenceexports/{absenceExport.Id}?action={AbsenceExportActions.Resend}");

            absenceExportResult.EnsureSuccessStatusCode();
            var absenceExportSaved = await GetAbsenceExport(absenceExport.Id);
            Assert.AreEqual(AbsenceExportStatus.Failed, absenceExportSaved.Status);
            Assert.AreEqual($"Missing mapping from AbsenceType to WorkType or EmploymentLeaveType {absenceExportSaved.Absence.AbsenceEntries[0].LocalAbsenceCode}", absenceExportSaved.Message);
        }
    }
}