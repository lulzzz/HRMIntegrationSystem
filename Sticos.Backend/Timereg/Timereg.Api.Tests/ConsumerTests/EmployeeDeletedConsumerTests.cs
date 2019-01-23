using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timereg.Api.Repositories.Models;

namespace Timereg.Api.Tests.ConsumerTests
{
    [TestFixture]
    public class EmployeeDeletedConsumerTests
    {
        [Test]
        public async Task EmployeeDeleted_DeleteAbsence()
        {
            var message = UniMicroTestSetup.TimeregEmployeeDeletedMessage;
            var employeeId = message.Message.EmployeeId;
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .SetupEmployeeDeleteConsumer();
            var context = await dbFactory.CreateDbContext();
            context.AbsenceExports.AddRange(A.CollectionOfFake<Repositories.Models.AbsenceExport>(10).Select(x => {
                x.EmployeeId = employeeId;
                x.UnitId = message.Message.OrgUnitId;
                return x; }));
            context.SaveChanges();

            await consumer.Consume(message);

            Assert.AreEqual(0, context.AbsenceExports.Count(), "Employee absence exports are not deleted");
        }
     
    }
}
