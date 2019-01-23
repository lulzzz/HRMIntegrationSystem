using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;

using Timereg.Api.Domain.Models;
using Shared.Contracts;
using Shared.TestCommon;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Tests.ControllerTests.ExternalSystemController
{
    [TestFixture]
    public class ExternalSystemGetExternalDataTests// : ExternalSystemSetup
    {
        [Test]
        public async Task WhenQueryExternalDataWithUnknownCustomerId_ThenForbiddenShouldBeReturned()
        {
            var unknownCustomerId = 1234567;
            var client = new ExternalSystemSetup().BuildClient();
            var exportAbsence = await client.GetAsync($"{unknownCustomerId}/externalsystems/1/externaldata");

            Assert.NotNull(exportAbsence);
            Assert.AreEqual(HttpStatusCode.Forbidden, exportAbsence.StatusCode);
        }
        [Test]
        public async Task WhenQueryingExternalDataWithMissingCustomerId_ThenNotFoundShouldBeReturned()
        {
            var client = new ExternalSystemSetup().BuildClient();
            var exportAbsence = await client.GetAsync($"/externaldata");

            Assert.NotNull(exportAbsence);
            Assert.AreEqual(HttpStatusCode.NotFound, exportAbsence.StatusCode);
        }
        [Test]
        public async Task WhenQueryingExternalDataIncorrectUrl_ThenNotFoundShouldBeReturned()
        {
            var externalSystemId = 2;
            var client = new ExternalSystemSetup().BuildClient();
            var externalDataResult = await client.GetAsync($"{externalSystemId}/externaldata");

            Assert.NotNull(externalDataResult);
            Assert.AreEqual(HttpStatusCode.NotFound, externalDataResult.StatusCode);
        }
        
        [Test]
        public async Task WhenQueryExternalDataAndNoDataExist_ThenEmptyListShouldBeReturned()
        {
            var externalSystemId = 2;
            var entityType = (int)ExternalDataEnum.Employee;
            var unit = ExternalSystemSetup.ValidSticosUnit;
            var url = GetUrl(externalSystemId, entityType, unit.Id);
            var client = new ExternalSystemSetup().BuildClient();

            var externalDataResult = await client.GetAsyncAndDeserialize<IList<ExternalData>>(url);

            Assert.AreEqual(0, externalDataResult.Count);
        }

        [Test]
        public async Task WhenQueryExternalDataAndAllDataIsValid_ThenItShouldBeReturnedWithIdentifiers()
        {
            var entityType = (int)ExternalDataEnum.Employee;
            var externalSystemId = 2;
            var unit = ExternalSystemSetup.ValidSticosUnit;

            var client = new ExternalSystemSetup()
                .WithSticosUnit(unit)
                .WithExternalEmployees(unit.BusinessOrganizationNumber,new List<Employee>{ExternalSystemSetup.ValidEmployee1})
                .WithExternalEmployments(new List<Employment> { ExternalSystemSetup.ValidEmployment1 })
                .WithExternalWorkers(new List<Worker> { ExternalSystemSetup.ValidWorker1 })
                .WithExternalWorkerRelations(new List<WorkRelation> { ExternalSystemSetup.ValidWorkRelation1 })
                .BuildClient();
            
            var url = GetUrl(externalSystemId,entityType, unit.Id);
            var externalDataResult = await client.GetAsyncAndDeserialize<IList<ExternalData>>(url);

            Assert.NotNull(externalDataResult);
            Assert.AreEqual(1, externalDataResult.Count);
            Assert.AreEqual(4, externalDataResult[0].Identifiers.Count);
        }

        [Test]
        public async Task GetExternalDataWithoutWorkRelation()
        {
            var entityType = (int)ExternalDataEnum.Employee;
            var externalSystemId = 2;
            var unit = ExternalSystemSetup.ValidSticosUnit;

            var client = new ExternalSystemSetup()
                .WithSticosUnit(unit)
                .WithExternalEmployees(unit.BusinessOrganizationNumber, new List<Employee> { ExternalSystemSetup.ValidEmployee1 })
                .WithExternalEmployments(new List<Employment> { ExternalSystemSetup.ValidEmployment1 })
                .WithExternalWorkers(new List<Worker> { ExternalSystemSetup.ValidWorker1 })
                //.WithExternalWorkerRelations(new List<WorkRelation> { ExternalSystemSetup.ValidWorkRelation1 })
                .BuildClient();

            var url = GetUrl(externalSystemId, entityType, unit.Id);
            var externalDataResult = await client.GetAsyncAndDeserialize<IList<ExternalData>>(url);

            Assert.AreEqual("WorkRelation is not found", externalDataResult[0].NotValidReasons[0]);
            Assert.NotNull(externalDataResult);
        }

        [Test]
        public async Task GetExternalDataWithoutEmploymentRelation()
        {
            var entityType = (int)ExternalDataEnum.Employee;
            var externalSystemId = 2;
            var unit = ExternalSystemSetup.ValidSticosUnit;

            var client = new ExternalSystemSetup()
                .WithSticosUnit(unit)
                .WithExternalEmployees(unit.BusinessOrganizationNumber, new List<Employee> { ExternalSystemSetup.ValidEmployee1 })
                //.WithExternalEmployments(new List<Employment> { ExternalSystemSetup.ValidEmployment1 })
                .WithExternalWorkers(new List<Worker> { ExternalSystemSetup.ValidWorker1 })
                .WithExternalWorkerRelations(new List<WorkRelation> { ExternalSystemSetup.ValidWorkRelation1 })
                .BuildClient();

            var url = GetUrl(externalSystemId, entityType, unit.Id);
            var externalDataResult = await client.GetAsyncAndDeserialize<IList<ExternalData>>(url);

            Assert.AreEqual("Employment is not found", externalDataResult[0].NotValidReasons[0]);
            Assert.NotNull(externalDataResult);
        }
        [Test]
        public async Task GetExternalDataWithoutWorker()
        {
            var entityType = (int)ExternalDataEnum.Employee;
            var externalSystemId = 2;
            var unit = ExternalSystemSetup.ValidSticosUnit;

            var client = new ExternalSystemSetup()
                .WithSticosUnit(unit)
                .WithExternalEmployees(unit.BusinessOrganizationNumber, new List<Employee> { ExternalSystemSetup.ValidEmployee1 })
                .WithExternalEmployments(new List<Employment> { ExternalSystemSetup.ValidEmployment1 })
                //.WithExternalWorkers(new List<Worker> { ExternalSystemSetup.ValidWorker1 })
                .WithExternalWorkerRelations(new List<WorkRelation> { ExternalSystemSetup.ValidWorkRelation1 })
                .BuildClient();

            var url = GetUrl(externalSystemId, entityType, unit.Id);
            var externalDataResult = await client.GetAsyncAndDeserialize<IList<ExternalData>>(url);

            Assert.AreEqual("WorkRelation is not found", externalDataResult[0].NotValidReasons[0]);
            Assert.NotNull(externalDataResult);
        }
        [Test]
        public async Task WhenAnExistingGettingExternalystem_ThenResponseShouldNotBeNull()
        {
            var economySystemId = 2; //unimicro

            var client = new ExternalSystemSetup().BuildClient();
            var externalSystems = await client.GetAsyncAndDeserialize<Timereg.Api.Contracts.ExternalSystem>($"{ExternalSystemSetup.CustomerId}/externalsystems/{economySystemId}");

            Assert.NotNull(externalSystems);
        }
        [Test]
        public async Task WhenGettingAnExistingExternalystem_AllPropertiesShouldBeMapped()
        {
            var economySystemIdThatDoesNotExist = 999999;

            var client = new ExternalSystemSetup().BuildClient();
            var externalSystems = await client.GetAsyncAndDeserialize<Timereg.Api.Contracts.ExternalSystem>($"{ExternalSystemSetup.CustomerId}/externalsystems/{economySystemIdThatDoesNotExist}");

            Assert.IsNull(externalSystems);
        }

        private string GetUrl(int externalSystemId, int? entityType=null, int? unitId=null)
        {
            var baseUrl = $"{ExternalSystemSetup.CustomerId}/externalsystems/{externalSystemId}/externaldata";
            var url = baseUrl;
            if(entityType!=null)
            {
                url += $"?entity={entityType}";
            }
            if (unitId != null)
            {
                url += $"&unitId={unitId}";
            }

            return url;
        }
    }
}
