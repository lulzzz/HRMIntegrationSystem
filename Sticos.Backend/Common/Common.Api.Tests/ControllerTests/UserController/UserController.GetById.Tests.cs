using Common.Api.Contracts.Users;
using NUnit.Framework;
using Shared.TestCommon;
using System.Collections.Generic;
using System.Threading.Tasks;
using db = Common.Api.Repositories.Legacy.Models;

namespace Common.Api.Tests.ControllerTests.UserController
{
    [TestFixture]
    public class UserControllerGetByIdTests : UserControllerTestBase
    {
        [Test]
        public async Task WhenRequestingCurrentUser_UserIdFromICurrentUserContextIsUsed()
        {
            var user = new db.User { UserId = _userId };
            AddUsersToDb(new List<db.User>() { user });
            var url = $"{_customerId}/users/{user.UserId}";

            var response = await _client.GetAsyncAndDeserialize<User>(url);

            Assert.NotNull(response);
            Assert.AreEqual(_userId, response.UserId);
        }

        [Test]
        public async Task WhenRequestingCurrentUser_AndUserHasPersonalCustomerAdminPermission_ThenPropShouldBeTrue()
        {
            var user = new db.User { UserId = _userId };
            AddUsersToDb(new List<db.User>() { user });
            var customerAdminRoleValue = 2; //CustomerAdmin
            var employeePermission = new db.EmployeePermission
            {
                ResponsibleUserId = _userId,
                PermissionType = customerAdminRoleValue,
            };
            AddUserPermissions(new List<db.EmployeePermission> { employeePermission });
            var url = $"{_customerId}/users/{user.UserId}";

            var response = await _client.GetAsyncAndDeserialize<User>(url);

            Assert.NotNull(response);
            Assert.AreEqual(true, response.IsPersonalCustomerAdmin);
        }

        [Test]
        public async Task WhenRequestingCurrentUser_AndHasOnlyOtherPermissions_ThenPropShouldBeFalse()
        {
            var user = new db.User { UserId = _userId };
            AddUsersToDb(new List<db.User>() { user });
            var customerAdminRoleValue = 1; //AnotherId !=2
            var employeePermission = new db.EmployeePermission
            {
                ResponsibleUserId = _userId,
                PermissionType = customerAdminRoleValue,
            };
            AddUserPermissions(new List<db.EmployeePermission> { employeePermission });
            var url = $"{_customerId}/users/{user.UserId}";

            var response = await _client.GetAsyncAndDeserialize<User>(url);

            Assert.NotNull(response);
            Assert.AreEqual(false, response.IsPersonalCustomerAdmin);
        }
        [Test]
        public async Task WhenRequestingCurrentUser_AndHasNoPermissions_ThenPropShouldBeFalse()
        {
            var user = new db.User { UserId = _userId };
            AddUsersToDb(new List<db.User>() { user });

            var url = $"{_customerId}/users/{user.UserId}";

            var response = await _client.GetAsyncAndDeserialize<User>(url);

            Assert.NotNull(response);
            Assert.AreEqual(false, response.IsPersonalCustomerAdmin);
        }

    }
}
