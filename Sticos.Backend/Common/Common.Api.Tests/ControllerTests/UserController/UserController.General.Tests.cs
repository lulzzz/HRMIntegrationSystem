﻿using Common.Api.Contracts.Users;
using Common.Api.Tests.Factories.Domain;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.TestCommon;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCommon.DataFactories;

namespace Common.Api.Tests.ControllerTests.UserController
{
    [TestFixture]
    public class UserControllerGeneralTests : UserControllerTestBase
    {
        protected const int NUMBER_OF_AUTOGENERATED_USERS = 150;

        [SetUp]
        public void SetupForEachTest()
        {
            var users = DbUserFactory.GetFactory(_customerId).Generate(NUMBER_OF_AUTOGENERATED_USERS);
            AddUsersToDb(users);
        }

        [Test]
        public async Task RequestingUsers_200OkAndList()
        {
            var url = $"{_customerId}/users";

            var response = await _client.GetAsyncAndDeserialize<List<User>>(url);

            Assert.IsNotNull(response);
            Assert.AreEqual(SearchConstants.DEFAULT_TAKE, response.Count);
        }

        [Test]
        public async Task RequestingUsers_Skip()
        {
            var skip = NUMBER_OF_AUTOGENERATED_USERS - SearchConstants.DEFAULT_TAKE / 2;
            var url = $"{_customerId}/users?skip={skip}";

            var response = await _client.GetAsyncAndDeserialize<List<User>>(url);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(SearchConstants.DEFAULT_TAKE, response.Count);
            Assert.AreEqual(SearchConstants.DEFAULT_TAKE / 2, response.Count);
        }

        [Test]
        public async Task RequestingUsers_Take()
        {
            var take = NUMBER_OF_AUTOGENERATED_USERS;
            var url = $"{_customerId}/users?take={take}";

            var response = await _client.GetAsyncAndDeserialize<List<User>>(url);

            Assert.IsNotNull(response);
            Assert.AreNotEqual(SearchConstants.DEFAULT_TAKE, response.Count);
            Assert.AreEqual(take, response.Count);
        }
    }
}
