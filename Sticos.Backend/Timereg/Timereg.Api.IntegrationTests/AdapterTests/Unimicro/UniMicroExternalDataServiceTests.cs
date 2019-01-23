using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.Api.Contracts;
using Common.Api.Contracts.Services;
using FakeItEasy;
using NUnit.Framework;
using Timereg.Api.Unimicro.Adapters;


namespace Timereg.Api.IntegrationTests.AdapterTests.Unimicro
{
    [TestFixture]
    public class UniMicroExternalDataServiceTests : UnimicroTestsBase
    {
        [Test]
        public async Task GetExternalEmployeeDataTest()
        {
            var unitId = 1337;
            var unitService = A.Fake<IUnitService>();
            A.CallTo(() => unitService.GetUnit(unitId))
                .Returns(new Unit {BusinessOrganizationNumber = _businessOrganizationNumber, LegalOrganizationNumber = _legalOrganizationNumber});

            var service = new UniMicroExternalDataService(unitService, _proxy);

            var employeeData = await service.GetExternalEmployeeData(unitId);

            var valid = employeeData.Where(e => e.ValidForUse).ToList();
            var invalid = employeeData.Where(e => !e.ValidForUse).ToList();

            foreach (var ed in invalid)
            {
                Console.WriteLine($"{ed.DataSet[0].Value}");
                Console.WriteLine($"\t\t{string.Join(",", ed.NotValidReasons)}");

            }


        }
    }
}