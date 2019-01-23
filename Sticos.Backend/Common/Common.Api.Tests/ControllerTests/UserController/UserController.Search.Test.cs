 using Common.Api.Contracts.Users;
using Common.Api.Tests.Factories.Domain;
using NUnit.Framework;
using Shared.TestCommon;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
 using TestCommon.DataFactories;

namespace Common.Api.Tests.ControllerTests.UserController
{
    [TestFixture]
    public class UserControllerSearchTest : UserControllerTestBase
    {
        [Test]
        public async Task RequestingUsers_FilterByUnitId()
        {
            var users = DbUserFactory.GetFactory(_customerId).Generate(10);
            AddUsersToDb(users);
            var userUnderTest = users.ElementAt(0);
            var url = $"{_customerId}/users?unitId={userUnderTest.UnitId}";

            var response = await _client.GetAsyncAndDeserialize<List<User>>(url);

            Assert.AreEqual(1, response.Count);
            var u = response.ElementAt(0);
            Assert.AreEqual(userUnderTest.UnitId, u.UnitId);
        }
    }
}
