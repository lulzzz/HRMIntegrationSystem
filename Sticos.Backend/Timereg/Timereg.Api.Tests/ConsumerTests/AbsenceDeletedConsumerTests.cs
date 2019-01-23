using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Tests.ConsumerTests
{
    [TestFixture]
    public class AbsenceDeletedConsumerTests
    {
        [Test]
        public async Task AbsenceExportExists_ValidMappings_DeletedSetsAbsenceExportDeleted()
        {
            var dbId = Guid.NewGuid().ToString();
            var message = UniMicroTestSetup.ValidDeletedMessageToday;
            var unitId = message.Message.UnitId;
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;
            var externalAbsenceIds = new List<int> { new Random().Next(1, 1000), new Random().Next(1, 1000) };
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(unitId)
                .WithUnitMapped(localId: unitId, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(localAbsenceCode, 7, unitId)
                .WithLocalEmployeeMappedToWorkRelation(employeeId, "2", unitId)
                .WithExistingAbsenceExport(unitId, message.Message.AbsenceId,localAbsenceCode, externalAbsenceIds, dbId)
                .SetupDeletedConsumer();

            var preDb = await dbFactory.CreateDbContext();
            var count = preDb.AbsenceExports.Count();

            // Act
            await consumer.Consume(message);

            // Assert
            var db = await dbFactory.CreateDbContext();
            Assert.AreEqual(count, db.AbsenceExports.Count());
            var absenceExport = db.AbsenceExports.Last();
            Assert.IsNotNull(absenceExport);
            Assert.AreEqual(AbsenceExportStatus.Success, Enum.Parse<AbsenceExportStatus>(absenceExport.Status.ToString()), absenceExport.Message);
            Assert.AreEqual(AbsenceExportAction.Delete, Enum.Parse<AbsenceExportAction>(absenceExport.Action.ToString()), absenceExport.Message);
        }

        [Test]
        public async Task AbsenceExportExists_ValidMappings_DeleteCallCrashes()
        {
            var dbId = Guid.NewGuid().ToString();
            var message = UniMicroTestSetup.ValidDeletedMessageToday;
            var unitId = message.Message.UnitId;
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;
            var externalAbsenceIds = new List<int> { new Random().Next(1, 1000), new Random().Next(1, 1000) };
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(unitId)
                .WithUnitMapped(localId: unitId, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(localAbsenceCode, 7, unitId)
                .WithLocalEmployeeMappedToWorkRelation(employeeId, "2", unitId)
                .WithExistingAbsenceExport(unitId, message.Message.AbsenceId,localAbsenceCode, externalAbsenceIds, dbId)
                .DeleteCallFails()
                .SetupDeletedConsumer();

            var preDb = await dbFactory.CreateDbContext();
            var count = preDb.AbsenceExports.Count();

            // Act
            await consumer.Consume(message);

            // Assert
            var db = await dbFactory.CreateDbContext();
            Assert.AreEqual(count, db.AbsenceExports.Count());
            var absenceExport = db.AbsenceExports.Last();
            Assert.IsNotNull(absenceExport);
            Assert.AreEqual(AbsenceExportStatus.Failed, Enum.Parse<AbsenceExportStatus>(absenceExport.Status.ToString()), absenceExport.Message);
            Assert.AreEqual(AbsenceExportAction.Delete, Enum.Parse<AbsenceExportAction>(absenceExport.Action.ToString()), absenceExport.Message);
        }
    }
}
