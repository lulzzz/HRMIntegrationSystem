using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.IntegrationTests.AdapterTests.Unimicro
{
    [TestFixture]
    public class UnimicroClientTests : UnimicroTestsBase
    {
        [Test]
        public async Task WhenSignInIsSuccessful_ThenAccessTokenShouldBeReturned()
        {
            var signedInInfo = await _proxy.SignIn();
            Assert.IsNotNull(signedInInfo.AccessToken);
        }

        [Test]
        public async Task WhenFetchingGetCompanyAuthorizationInfo_BogubyggShouldBeReturnedWithKeySet()
        {
            var company = await _proxy.GetAndSetCompanyAuthorizationInfo(_legalOrganizationNumber);
            Assert.IsNotNull(company);
            Assert.AreEqual(_companyKey, company.Key);
        }

        [Test]
        public async Task WhenFetchingCompanies_BusinessShouldBeReturned()
        {
            var companies = await _proxy.GetSubEntities();
            Assert.IsNotNull(companies);
            var company = companies.FirstOrDefault(c => c.OrgNumber == _businessOrganizationNumber);
            Assert.IsNotNull(company);
        }

        [Test]
        public async Task WhenFetchingWorkers_ListShouldBeReturned()
        {
            var name = "Thor";
            var employees = await _proxy.GetEmployees(_businessOrganizationNumber);
            var employeeIds = employees.Select(e => e.Id);
            var workers = await _proxy.GetWorkers(employeeIds);

            Assert.IsNotNull(workers);
            var worker = workers.FirstOrDefault(c => c.Info.Name == name);
            Assert.IsNotNull(worker);
        }

        [Test]
        public async Task WhenFetchingUsers_ListShouldBeReturned()
        {
            var email = "thor@sticos.no";
            var userIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

            var users = await _proxy.GetUsers(userIds);

            Assert.IsNotNull(users);
            var user = users.FirstOrDefault(c => c.Email == email);
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task WhenFetchingWorkTypes_ListShouldBeReturned()
        {
            var absenceType = "Fravær";

            var workTypes = await _proxy.GetWorkTypes();

            Assert.IsNotNull(workTypes);
            var workType = workTypes.FirstOrDefault(c => c.Name == absenceType);
            Assert.IsNotNull(workType);
        }

        [Test]
        public async Task WhenFetchingHourBalance_BalanceShouldBeReturned()
        {
            var workRelationId = 11;
            var absenceType = "Fravær";

            var hourBalance = await _proxy.GetHourBalance(workRelationId);

            Assert.IsNotNull(hourBalance);
            Assert.IsTrue(hourBalance.ExpectedMinutes > 0);
        }

        [Test]
        public async Task WhenFetchingWorkRelation_WorkRelationAndWorkProfileShouldBeReturned()
        {
            var workerId = new List<int> {7,5,6};

            var workRelations = await _proxy.GetWorkRelations(workerId);
            Assert.IsNotNull(workRelations);
            Assert.IsTrue(workRelations.Count() > 0);

        }

        [Test]
        public async Task WhenFetchingEmployees_NameShouldBeSet()
        {
            var emloyeeId = 1;
            var emloyeeIds = new List<int> { emloyeeId };
            var name = "Ola Nordmann";
            var employees = await _proxy.GetEmployees(_businessOrganizationNumber);
            Assert.IsNotNull(employees);
            var employee = employees.FirstOrDefault(c => c.Id == emloyeeId);
            Assert.IsNotNull(employee);
            Assert.AreEqual(name, employee.BusinessRelationInfo.Name);
        }

        [Test]
        public async Task WhenFetchingEmployment_NameShouldBeSet()
        {
            var emloyeeId = 1;
            var emloyeeIds = new List<int> { emloyeeId };
            var employmentId = 3;
            var jobName = "ORDFØRER";

            var employments = await _proxy.GetEmployments(emloyeeIds);
            Assert.IsNotNull(employments);
            var employment = employments.FirstOrDefault(c => c.Id == employmentId);
            Assert.IsNotNull(employment);
            Assert.AreEqual(jobName, employment.JobName);
        }
        [Test]
        public async Task WhenFetchingEmploymentLeave_TypeShouldBeSet()
        {
            var employmentId = 3;
            var emloyementIds = new List<int> { employmentId };
            var employmentLeaveId = 44;

            var employmentLeaves = await _proxy.GetEmploymentLeaves(emloyementIds);
            Assert.IsNotNull(employmentLeaves);
            var employmentLeave = employmentLeaves.FirstOrDefault(c => c.Id == employmentLeaveId);
            Assert.IsNotNull(employmentLeave);
            Assert.AreEqual(LeaveType.Leave_with_parental_benefit, employmentLeave.LeaveType);
        }
    }
}