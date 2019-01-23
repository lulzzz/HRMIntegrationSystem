using Common.Api.Tests.Factories.Domain;
using NUnit.Framework;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCommon.DataFactories;
using contract = Common.Api.Contracts.Employees;
using db = Common.Api.Repositories.Legacy.Models;

namespace Common.Api.Tests.ControllerTests.Employees
{
    [TestFixture]
    public class EmployeeControllerSearchTests : EmployeeControllerTestsBase
    {
        [SetUp]
        public async Task SetupForEachTest()
        {
            await AddPersonalAdminPermissionForUsers();
            var user = DbUserFactory.GetFactory(_customerId).Generate(1);
            await AddToPersonalCommonDb(user);
            AddCurrentUserToDb();
        }

        [Test]
        public async Task RequestingEmployees_FilterByFirstName()
        {
            var employees = EmployeeFactory.GetFactory().Generate(10);
            await AddToPersonalDb(employees);
            var employeeUnderTest = employees.ElementAt(0);
            var url = $"{_customerId}/employees?firstName={employeeUnderTest.FirstName}";

            var employeeList = await _client.GetAsyncAndDeserialize<List<contract.Employee>>(url);

            Assert.AreEqual(1, employeeList.Count);
            var e = employeeList.ElementAt(0);
            Assert.AreEqual(e.FirstName, employeeUnderTest.FirstName);
        }
        [Test]
        public async Task RequestingEmployees_FilterByFirstNameCasing()
        {
            var employees = EmployeeFactory.GetFactory().Generate(10);
            await AddToPersonalDb(employees);
            var employeeUnderTest = employees.ElementAt(0);
            var url = $"{_customerId}/employees?firstName={employeeUnderTest.FirstName.ToLower()}";

            var employeeList = await _client.GetAsyncAndDeserialize<List<contract.Employee>>(url);

            Assert.AreEqual(1, employeeList.Count);
            var e = employeeList.ElementAt(0);
            Assert.AreEqual(e.FirstName, employeeUnderTest.FirstName);
        }

        [Test]
        public async Task RequestingEmployees_FilterByLastName()
        {
            var employees = EmployeeFactory.GetFactory().Generate(10);
            await AddToPersonalDb(employees);
            var employeeUnderTest = employees.ElementAt(0);
            var url = $"{_customerId}/employees?lastName={employeeUnderTest.LastName}";

            var employeeList = await _client.GetAsyncAndDeserialize<List<contract.Employee>>(url);

            Assert.AreEqual(1, employeeList.Count);
            var e = employeeList.ElementAt(0);
            Assert.AreEqual(e.LastName, employeeUnderTest.LastName);
        }

        [Test]
        public async Task RequestingEmployees_FilterByLastNameCasing()
        {
            var employees = EmployeeFactory.GetFactory().Generate(10);
            await AddToPersonalDb(employees);
            var employeeUnderTest = employees.ElementAt(0);
            var url = $"{_customerId}/employees?lastName={employeeUnderTest.LastName.ToUpper()}";

            var employeeList = await _client.GetAsyncAndDeserialize<List<contract.Employee>>(url);

            Assert.AreEqual(1, employeeList.Count);
            var e = employeeList.ElementAt(0);
            Assert.AreEqual(e.LastName, employeeUnderTest.LastName);
        }

        [Test]
        public async Task RequestingEmployees_FilterByJobTitleName()
        {
            var employees = EmployeeFactory.GetFactory().Generate(10);
            await AddToPersonalDb(employees);
            var employeeUnderTest = employees.ElementAt(0);
            var url = $"{_customerId}/employees?jobTitle={employeeUnderTest.JobTitle}";

            var employeeList = await _client.GetAsyncAndDeserialize<List<contract.Employee>>(url);

            Assert.AreEqual(1, employeeList.Count);
            var e = employeeList.ElementAt(0);
            Assert.AreEqual(e.JobTitle, employeeUnderTest.JobTitle);
        }
        [Test]
        public async Task RequestingEmployees_FilterByJobTitleCasing()
        {
            var employees = EmployeeFactory.GetFactory().Generate(10);
            await AddToPersonalDb(employees);
            var employeeUnderTest = employees.ElementAt(0);
            var url = $"{_customerId}/employees?jobTitle={employeeUnderTest.JobTitle.ToLower()}";

            var employeeList = await _client.GetAsyncAndDeserialize<List<contract.Employee>>(url);

            Assert.AreEqual(1, employeeList.Count);
            var e = employeeList.ElementAt(0);
            Assert.AreEqual(e.JobTitle, employeeUnderTest.JobTitle);
        }

        [Test]
        public async Task RequestingEmployees_PropertiesOnEmployee()
        {
            var employee = EmployeeFactory.GetFactory().Generate(1).FirstOrDefault();
            var user = DbUserFactory.GetFactory(_customerId,employee.UserId.Value).Generate(1);

            await AddToPersonalDb(employee);
            await AddToPersonalCommonDb(user);

            var url = $"{_customerId}/employees";
            var employeeList = await _client.GetAsyncAndDeserialize<List<contract.Employee>>(url);
            var e = employeeList.ElementAt(0);
            Assert.IsTrue(e.Id > 0);
            Assert.IsTrue(e.UserId > 0);
            Assert.IsFalse(string.IsNullOrWhiteSpace(e.FirstName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(e.LastName));
            Assert.IsFalse(string.IsNullOrWhiteSpace(e.JobTitle));
            Assert.IsFalse(string.IsNullOrWhiteSpace(e.Email));
            Assert.IsFalse(string.IsNullOrWhiteSpace(e.Phone));
        }

        [Test]
        public async Task WhenRequestingEmployeesByUnitId_SubUnits_ThenItShouldBeReturned()
        {
            var unitId = 5;
            var subUnit1 = 10;
            var subUnit2 = 12;
            var dummyUnitId = 1337;

            // Arrange
            var emps = EmployeeFactory.GetFactory().Generate(5);
            emps[0].UnitId = unitId;
            emps[1].UnitId = subUnit1;
            emps[2].UnitId = subUnit2;
            emps[3].UnitId = subUnit2;
            emps[4].UnitId = dummyUnitId;
            
            await AddToPersonalDb(emps);

            // Act
            var url = $"{_customerId}/employees/?take=500&UnitIds={unitId}&UnitIds={subUnit1}&UnitIds={subUnit2}";
            var employees = await _client.GetAsyncAndDeserialize<List<contract.Employee>>(url);
            
            // Asserts
            Assert.IsNotNull(employees);
            Assert.AreEqual(4, employees.Count);

            Assert.IsTrue(employees.Any(x => x.UnitId == unitId));
            Assert.IsTrue(employees.Any(x => x.UnitId == subUnit1));
            Assert.AreEqual(2, employees.Count(x => x.UnitId == subUnit2));
            Assert.IsFalse(employees.Any(x => x.UnitId == dummyUnitId));
        }
    }
}
