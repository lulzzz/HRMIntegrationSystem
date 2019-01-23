using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Timereg.Api.Domain.Models;
using System;
using System.Collections.Generic;
using Common.Api.Contracts;
using FakeItEasy;
using Sticos.Personal.MessageContracts.Enums;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Tests.ConsumerTests
{
    [TestFixture]
    public class AbsenceApprovedConsumerTests
    {
        private const int Vacation = (int)AbsenceSubType.Vacation;

        
        [Test]
        public async Task WhenIntegrationIsConnectedToTheParentOfTheAbsenceUnitid_ThenItShouldBeReturned()
        {
            var parentUnitId = 1;
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var childUnitId = message.Message.UnitId; 
            
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;

            var integration = UniMicroTestSetup.GetValidIntegration(parentUnitId);
            var unit = UniMicroTestSetup.GetValidUnit(childUnitId, parentUnitId);
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithIntegrations(new List<Integrations.Api.Contracts.Integration> {integration})
                .WithUnits(new List<Unit> {unit})
                .WithUnitMapped(parentUnitId, 36, parentUnitId)
                .WithAbsenceMappedToWorkType(localAbsenceCode, 7, parentUnitId)
                .WithLocalEmployeeMappedToWorkRelation(employeeId, "2", parentUnitId)
                .UniMicroServiceReturnsWorkItem(message.Message)
                .SetupApprovedConsumer();
            var db = await dbFactory.CreateDbContext();                    
            // Act
            await consumer.Consume(message);

            // Assert
            var absenceExport = db.AbsenceExports.FirstOrDefault();
            Assert.IsNotNull(absenceExport);
        }
        [Test]
        public async Task WhenIntegrationIsConnectedToTheParentOfParentOfTheAbsenceUnitid_ThenItShouldBeReturned()
        {
            var parentUnitId = 10;
            var parentUnitId2 = 100;
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var childUnitId = message.Message.UnitId;
            
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;

            var integration = UniMicroTestSetup.GetValidIntegration(parentUnitId);
            var unit = UniMicroTestSetup.GetValidUnit(childUnitId, parentUnitId2);
            var unit2 = UniMicroTestSetup.GetValidUnit(parentUnitId2, parentUnitId);
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithIntegrations(new List<Integrations.Api.Contracts.Integration> {integration})
                .WithUnits(new List<Unit> {unit,unit2})
                .WithUnitMapped(parentUnitId, 36, parentUnitId)
                .WithAbsenceMappedToWorkType(localAbsenceCode, 7, parentUnitId)
                .WithLocalEmployeeMappedToWorkRelation(employeeId, "2", parentUnitId)
                .UniMicroServiceReturnsWorkItem(message.Message)
                .SetupApprovedConsumer();
            var db = await dbFactory.CreateDbContext();

            // Act
            await consumer.Consume(message);

            // Assert
            var absenceExport = db.AbsenceExports.FirstOrDefault();
            Assert.IsNotNull(absenceExport);
        }

        [Test]
        public async Task ValidMappings_ExportIsSuccessful()
        {
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var unitId = message.Message.UnitId;
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;

            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(unitId)
                .WithUnitMapped(localId: unitId, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(localAbsenceCode, 7,unitId)
                .WithLocalEmployeeMappedToWorkRelation(employeeId, "2",unitId)
                .UniMicroServiceReturnsWorkItem(message.Message)
                .SetupApprovedConsumer();
            var db = await dbFactory.CreateDbContext();

            // Act
            await consumer.Consume(message);

            // Assert
            var absenceExport = db.AbsenceExports.FirstOrDefault();
            Assert.IsNotNull(absenceExport);
            Assert.AreEqual((int)AbsenceExportStatus.Success, absenceExport.Status);
            Assert.AreEqual((int)AbsenceExportAction.Create, absenceExport.Action);
        }

        [Test]
        public async Task AbsenceExportExists_ValidMappings_NewExportedWithSuccess()
        {
            var dbId = Guid.NewGuid().ToString();
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var unitId = message.Message.UnitId;
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;
            var externalAbsenceIds = new List<int> { new Random().Next(1, 1000), new Random().Next(1, 1000) };
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(unitId)
                .WithUnitMapped(localId: unitId, externalId: 36, unitId: unitId)
                .WithAbsenceMappedToWorkType(localAbsenceCode, 7,unitId)
                .WithLocalEmployeeMappedToWorkRelation(employeeId, "2",unitId)
                .WithExistingAbsenceExport(unitId, message.Message.AbsenceId,localAbsenceCode, externalAbsenceIds, dbId)
                .SetupApprovedConsumer();

            // Act
            await consumer.Consume(message);

            // Assert
            var db = await dbFactory.CreateDbContext();
            var absenceExport = db.AbsenceExports.Last();
            Assert.IsNotNull(absenceExport);
            Assert.AreEqual(AbsenceExportStatus.Success, Enum.Parse<AbsenceExportStatus>(absenceExport.Status.ToString()), absenceExport.Message);
        }

        [Test]
        public async Task AbsenceExportExists_ValidMappings_ExternalWorkItemIsDeleted()
        {
            var dbId = Guid.NewGuid().ToString();
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var unitId = message.Message.UnitId;
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;
            var externalAbsenceIds = new List<int> {new Random().Next(1, 1000), new Random().Next(1, 1000)};
            
            var (consumer, dbFactory, unimicroClient) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(unitId)
                .WithUnitMapped(localId: unitId, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(localAbsenceCode, 7, unitId)
                .WithLocalEmployeeMappedToWorkRelation(employeeId, "2", unitId)
                .WithExistingAbsenceExport(unitId, message.Message.AbsenceId, localAbsenceCode, externalAbsenceIds, dbId)
                .SetupExposingClient();

            // Act
            await consumer.Consume(message);

            // Assert
            foreach (var externalAbsenceId in externalAbsenceIds)
            {
                A.CallTo(() => unimicroClient.DeleteWorkItem(externalAbsenceId.ToString()))
                    .MustHaveHappenedOnceExactly();
                A.CallTo(() => unimicroClient.DeleteEmploymentLeave(externalAbsenceId.ToString()))
                    .MustNotHaveHappened();
            }
        }
        [Test]
        public async Task AbsenceExportExists_ValidMappings_ExternalEmployeeLeaveIsDeleted()
        {
            var dbId = Guid.NewGuid().ToString();
            var message = UniMicroTestSetup.ValidMessageParentalLeave;
            var unitId = message.Message.UnitId;
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;
            var externalAbsenceIds = new List<int> {new Random().Next(1, 1000), new Random().Next(1, 1000)};
            
            var (consumer, dbFactory, unimicroClient) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(unitId)
                .WithUnitMapped(localId: unitId, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToLeaveType(localAbsenceCode, LeaveType.Educational_leave, unitId)
                .WithLocalEmployeeMappedToEmployment(employeeId, "2", unitId)
                .WithExistingAbsenceExport(unitId, message.Message.AbsenceId,localAbsenceCode, externalAbsenceIds, dbId)
                .SetupExposingClient();

            // Act
            await consumer.Consume(message);

            // Assert
            foreach (var externalAbsenceId in externalAbsenceIds)
            {
                A.CallTo(() => unimicroClient.DeleteEmploymentLeave(externalAbsenceId.ToString()))
                    .MustHaveHappenedOnceExactly();
                A.CallTo(() => unimicroClient.DeleteWorkItem(externalAbsenceId.ToString()))
                    .MustNotHaveHappened();
            }
        }

        [Test]
        public async Task AbsenceExportExists_ValidMappings_ExistingIsMarkedObsoleteInDb()
        {
            var dbId = Guid.NewGuid().ToString();
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var unitId = message.Message.UnitId;
            var employeeId = message.Message.EmployeeId;
            var localAbsenceCode = (int)message.Message.AbsenceEntries.FirstOrDefault().AbsenceSubType;
            var externalAbsenceIds = new List<int> { new Random().Next(1, 1000), new Random().Next(1, 1000) };
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(unitId)
                .WithUnitMapped(localId: unitId, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(localAbsenceCode, 7, unitId)
                .WithLocalEmployeeMappedToWorkRelation(employeeId, "2", unitId)
                .WithExistingAbsenceExport(unitId, message.Message.AbsenceId, localAbsenceCode, externalAbsenceIds, dbId)
                .SetupApprovedConsumer();

            // Act
            await consumer.Consume(message);

            // Assert
            var db = await dbFactory.CreateDbContext();
            var obsoleteAbsenceExport = db.AbsenceExports.FirstOrDefault(ae => ae.Id == dbId);
            Assert.AreEqual(AbsenceExportStatus.Obsolete, Enum.Parse<AbsenceExportStatus>(obsoleteAbsenceExport.Status.ToString()), obsoleteAbsenceExport.Message);
            Assert.AreEqual(AbsenceExportAction.Create, Enum.Parse<AbsenceExportAction>(obsoleteAbsenceExport.Action.ToString()));

            // Assert new AbsenceExport entry is created with update action
            var newAbsenceExport = db.AbsenceExports.LastOrDefault();
            Assert.AreEqual(AbsenceExportStatus.Success, Enum.Parse<AbsenceExportStatus>(newAbsenceExport.Status.ToString()));
            Assert.AreEqual(AbsenceExportAction.Update, Enum.Parse<AbsenceExportAction>(newAbsenceExport.Action.ToString()));
        }

        [Test]
        public async Task ValidMappings_AbsenceIsSaved()
        {
            var context = UniMicroTestSetup.ValidApprovedMessageToday;
            var unitId = context.Message.UnitId;
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(1000)
                .WithUnitMapped(localId: context.Message.UnitId, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(Vacation, 7, unitId)
                .WithLocalEmployeeMappedToWorkRelation(87889, "2", unitId)
                .UniMicroServiceReturnsWorkItem(context.Message)
                .SetupApprovedConsumer();
            var db = await dbFactory.CreateDbContext();

            var exportCount = db.AbsenceExports.Count();

            // Act
            await consumer.Consume(context);

            // Assert
            Assert.AreEqual(exportCount + 1, db.AbsenceExports.Count());
            var absenceExport = db.AbsenceExports.Last();
            var absence = JsonConvert.DeserializeObject<Absence>(absenceExport.AbsenceJson);
            Assert.AreEqual(87889, absence.EmployeeId);

            Assert.AreEqual(context.Message.UnitId, absence.UnitId);
            var absenceEntry = absence.AbsenceEntries.First();
            var messageAbsenceEntry = context.Message.AbsenceEntries.First();
            Assert.AreEqual("1", absenceEntry.ExternalId);
            Assert.AreEqual((int) AbsenceSubType.Vacation, absenceEntry.LocalAbsenceCode);
            Assert.AreEqual("7", absenceEntry.ExternalAbsenceCode);
            Assert.AreEqual("2", absenceEntry.ExternalEntityId);
            Assert.AreEqual(messageAbsenceEntry.FromDate, absenceEntry.StartTime.DateTime);
            Assert.AreEqual(messageAbsenceEntry.ToDate, absenceEntry.EndTime.DateTime);
        }

        [Test]
        public async Task MissingUserMapping_ExportFailedWithCorrectMessage()
        {
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var unitId = message.Message.UnitId;
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(1000)
                .WithUnitMapped(localId: 1000, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(Vacation, 7, unitId)
                .SetupApprovedConsumer();
            var db = await dbFactory.CreateDbContext();

            var exportCount = db.AbsenceExports.ToArray().Length;

            // Act
            await consumer.Consume(message);

            // Assert
            Assert.AreEqual(exportCount + 1, db.AbsenceExports.Count());
            var absenceExport = db.AbsenceExports.Last();
            Assert.AreEqual((int)AbsenceExportStatus.Failed, absenceExport.Status);
            Assert.AreEqual($"Missing mapping from Employee to WorkRelation {message.Message.EmployeeId}", absenceExport.Message);
        }

        [Test]
        public async Task MissingAbsenceTypeMapping_ExportFailedWithCorrectMessage()
        {
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var unitId = message.Message.UnitId;
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(1000)
                .WithUnitMapped(localId: 1000, externalId: 36, unitId:unitId)
                .WithLocalEmployeeMappedToWorkRelation(87889, "2",unitId)
                .UniMicroServiceReturnsWorkItem(message.Message)
                .SetupApprovedConsumer();
            var db = await dbFactory.CreateDbContext();

            var exportCount = db.AbsenceExports.Count();

            // Act
            await consumer.Consume(message);

            // Assert
            Assert.AreEqual(exportCount + 1, db.AbsenceExports.Count());
            var absenceExport = db.AbsenceExports.Last();
            Assert.AreEqual((int)AbsenceExportStatus.Failed, absenceExport.Status);
            Assert.AreEqual($"Missing mapping from AbsenceType to WorkType or EmploymentLeaveType {Vacation}", absenceExport.Message);
        }

        [Test]
        public async Task UniMicroFails_ErrorMessageIsSaved()
        {
            var message = UniMicroTestSetup.ValidApprovedMessageToday;
            var unitId = message.Message.UnitId;
            var errorMessage = "Error requesting Unimicro-Api";
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .WithUniMicroEnabledForUnitId(1000)
                .WithUnitMapped(localId: 1000, externalId: 36, unitId:unitId)
                .WithAbsenceMappedToWorkType(Vacation, 7, unitId)
                .WithLocalEmployeeMappedToWorkRelation(87889, "2",unitId)
                .UniMicroServiceFails()
                .SetupApprovedConsumer();

            var db = await dbFactory.CreateDbContext();

            var exportCount = db.AbsenceExports.Count();

            // Act
            await consumer.Consume(message);

            // Assert
            Assert.AreEqual(exportCount + 1, db.AbsenceExports.Count());
            var absenceExport = db.AbsenceExports.Last();
            Assert.AreEqual((int)AbsenceExportStatus.Failed, absenceExport.Status);
            Assert.AreEqual($"Communication with external system failed: {errorMessage}", absenceExport.Message);
        }
    }
}