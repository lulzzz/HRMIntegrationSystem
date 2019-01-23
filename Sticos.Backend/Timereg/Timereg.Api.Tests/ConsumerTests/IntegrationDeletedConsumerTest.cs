using FakeItEasy;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timereg.Api.Tests.ConsumerTests
{
    [TestFixture]
    public class IntegrationDeletedConsumerTest
    {
        [Test]
        public async Task TimeregIntegrationDeleted_DeleteAbsenceExports()
        {
            var message = UniMicroTestSetup.TimeregIntegartionDeletedMessage;
            var unitId = message.Message.UnitId;
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .SetupIntegrationDeleteConsumer();
            var context = await dbFactory.CreateDbContext();
            context.AbsenceExports.AddRange(GenerateFakeAbsenceExports(10, unitId));
            context.AbsenceExports.AddRange(GenerateFakeAbsenceExports(10, unitId + 100));
            context.SaveChanges();

            await consumer.Consume(message);

            Assert.AreEqual(10, context.AbsenceExports.Count(), "Unit absence exports are not deleted");
            Assert.IsTrue(context.AbsenceExports.All(x => x.UnitId != unitId), "Found unit exports");
        }

        [Test]
        public async Task OtherIntegrationDeleted_NothingHappens()
        {
            var message = UniMicroTestSetup.OtherIntegartionDeletedMessage;
            var unitId = message.Message.UnitId;
            var (consumer, dbFactory) = new UniMicroTestSetup()
                .SetupIntegrationDeleteConsumer();
            var context = await dbFactory.CreateDbContext();
            context.AbsenceExports.AddRange(GenerateFakeAbsenceExports(10, unitId));
            context.AbsenceExports.AddRange(GenerateFakeAbsenceExports(10, unitId + 100));
            context.SaveChanges();

            await consumer.Consume(message);

            Assert.AreEqual(20, context.AbsenceExports.Count(), "No absence exports are deleted");
            Assert.AreEqual(10, context.AbsenceExports.Where(x => x.UnitId == unitId).Count(), "Some unit absence export deleted");
        }

        private IEnumerable<Repositories.Models.AbsenceExport> GenerateFakeAbsenceExports(int quantity, int unitId)
        {
            return A.CollectionOfFake<Repositories.Models.AbsenceExport>(quantity).Select(x => { x.UnitId = unitId; return x; });
        }
    }
}
