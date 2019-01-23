using Newtonsoft.Json;
using NUnit.Framework;
using Sticos.Personal.MessageContracts.Enums;
using System.Linq;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Tests.ConsumerTests
{
    [TestFixture]
    public class MultiDayAbsenceTests
    {
        private const int Vacation = (int)AbsenceSubType.Vacation;

        [Test]
        public async Task ValidMappings_ExportsAllAbsenceEntries_Vacation()
        {
            var message = UniMicroTestSetup.ValidMessageWeekVacation;
            var unitId = message.Message.UnitId;
            var workRelationId = "2";
            var externalVacation = 7;
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(1000)
                .WithUnitMapped(localId: 1000, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(Vacation, externalVacation, unitId)
                .WithLocalEmployeeMappedToWorkRelation(87889, workRelationId, unitId)
                .UniMicroServiceReturnsWorkItem(message.Message)
                .SetupApprovedConsumer();

            // Act
            await consumer.Consume(message);

            // Assert
            var db = await dbFactory.CreateDbContext();
            var absenceExport = db.AbsenceExports.FirstOrDefault();
            Assert.IsNotNull(absenceExport);
            Assert.AreEqual("Absence successfully exported", absenceExport.Message);
            Assert.AreEqual((int)AbsenceExportStatus.Success, absenceExport.Status);

            var absence = JsonConvert.DeserializeObject<Absence>(absenceExport.AbsenceJson);

            // Correct number of absenceEntries created in unimicro
            var externalIds = absence.AbsenceEntries.Select(x => x.ExternalId).Distinct();
            Assert.AreEqual(message.Message.AbsenceEntries.Count, externalIds.Count());

            absence.AbsenceEntries.ForEach(entry =>
            {
                Assert.AreEqual(workRelationId, entry.ExternalEntityId);
                Assert.AreEqual(externalVacation.ToString(), entry.ExternalAbsenceCode);
            });
        }

        [Test]
        public async Task ValidMappings_ExportsAllAbsenceEntries_ParentalLeave()
        {
            var message = UniMicroTestSetup.ValidMessageParentalLeave;
            var unitId = message.Message.UnitId;
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(1000)
                .WithUnitMapped(localId: 1000, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToLeaveType((int)AbsenceSubType.ParentalLeave, LeaveType.Leave_with_parental_benefit, unitId)
                .WithLocalEmployeeMappedToEmployment(87889, "9", unitId)
                .UniMicroServiceReturnsWorkItem(message.Message)
                .SetupApprovedConsumer();

            // Act
            await consumer.Consume(message);

            // Assert
            var db = await dbFactory.CreateDbContext();
            var absenceExport = db.AbsenceExports.FirstOrDefault();
            Assert.IsNotNull(absenceExport);
            Assert.AreEqual("Absence successfully exported", absenceExport.Message);
            Assert.AreEqual((int)AbsenceExportStatus.Success, absenceExport.Status);

            var absence = JsonConvert.DeserializeObject<Absence>(absenceExport.AbsenceJson);

            var entry = absence.AbsenceEntries.Single();
            Assert.AreEqual("9", entry.ExternalEntityId);
            Assert.AreEqual(((int) LeaveType.Leave_with_parental_benefit).ToString(), entry.ExternalAbsenceCode);
            Assert.AreEqual("101", entry.ExternalId);
        }

        [Test]
        public async Task ValidMappings_ParentalLeave_ExportsAllAbsenceEntries()
        {
            var message = UniMicroTestSetup.ValidMessageOneMonthParentalLeaveWithVacation;
            var unitId = message.Message.UnitId;
            var employmentId = "11";
            var workRelationId = "2";
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(1000)
                .WithUnitMapped(localId: 1000, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToLeaveType((int)AbsenceSubType.ParentalLeave, LeaveType.Leave_with_parental_benefit, unitId)
                .WithAbsenceMappedToWorkType(Vacation, 7, unitId)
                .WithLocalEmployeeMappedToWorkRelation(87889, workRelationId, unitId)
                .WithLocalEmployeeMappedToEmployment(87889, employmentId, unitId)
                .UniMicroServiceReturnsWorkItem(message.Message)
                .SetupApprovedConsumer();

            // Act
            await consumer.Consume(message);

            // Assert
            var db = await dbFactory.CreateDbContext();
            var absenceExport = db.AbsenceExports.FirstOrDefault();
            Assert.IsNotNull(absenceExport);
            Assert.AreEqual("Absence successfully exported", absenceExport.Message);
            Assert.AreEqual((int)AbsenceExportStatus.Success, absenceExport.Status);

            var absence = JsonConvert.DeserializeObject<Absence>(absenceExport.AbsenceJson);

            // Correct number of absenceEntries created in unimicro
            var externalIds = absence.AbsenceEntries.Select(x => x.ExternalId).Distinct();
            Assert.AreEqual(message.Message.AbsenceEntries.Count, externalIds.Count());

            var firstLeave = absence.AbsenceEntries[0];
            Assert.AreEqual("101", firstLeave.ExternalId);
                Assert.AreEqual(employmentId, firstLeave.ExternalEntityId);
            Assert.AreEqual(((int)LeaveType.Leave_with_parental_benefit).ToString(), firstLeave.ExternalAbsenceCode);

            var vacationEntries = absence.AbsenceEntries.Where(ae => ae.LocalAbsenceCode == Vacation).ToList();
            Assert.AreEqual(5, vacationEntries.Count);
            var externalId = 1;
            vacationEntries.ForEach(vacation =>
            {
                Assert.AreEqual(workRelationId, vacation.ExternalEntityId);
                Assert.AreEqual((externalId++).ToString(), vacation.ExternalId);
            });

            var secondLeave = absence.AbsenceEntries[6];
            Assert.AreEqual("102", secondLeave.ExternalId);
            Assert.AreEqual(employmentId, secondLeave.ExternalEntityId);
            Assert.AreEqual(((int)LeaveType.Leave_with_parental_benefit).ToString(), secondLeave.ExternalAbsenceCode);
        }
    }
}
