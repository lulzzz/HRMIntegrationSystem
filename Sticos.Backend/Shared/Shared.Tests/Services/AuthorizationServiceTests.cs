using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Shared.Domain.Enums;
using Shared.Domain.ValueObjects.Queries;
using Shared.Interfaces;
using Shared.Interfaces.Queries;
using Shared.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Tests.Services
{
    public class AuthorizationServiceTests
    {
        [TestFixture]
        public class GetUnitPermissions : TestBase
        {
            [Test]
            public async Task NoPermissions_ReturnsImplicitMasterPermissions()
            {
                int userId = 1;
                A.CallTo(() => PermissionService.GetImplicitByUnitType(UnitType.Master)).Returns(new List<PermissionType> { PermissionType.EndreKompetanseType });
                A.CallTo(() => AuthorizationQueries.GetUnitPermissionsByUserId(A<int>.Ignored, A<int?>.Ignored, A<PermissionType[]>.Ignored)).Returns(new List<UnitPermission>());

                var list = await AuthorizationService.GetUnitPermissions(userId, null, PermissionType.EndreKompetanseType);

                Assert.That(list != null && list.Count() == 1);
                Assert.That(list.Single().PermissionType == PermissionType.EndreKompetanseType && list.Single().UnitId == UnitConstants.MasterUnitId);
            }

            [Test]
            public async Task OneCompanyPermission_ReturnsForCompany()
            {
                int unitId = 99;
                int userId = 1;

                A.CallTo(() => PermissionService.GetImplicitByUnitType(UnitType.Master)).Returns(new List<PermissionType> { PermissionType.EndreKompetanseType });
                A.CallTo(() => AuthorizationQueries.GetUnitPermissionsByUserId(A<int>.Ignored, A<int?>.Ignored, A<PermissionType[]>.Ignored)).Returns(new List<UnitPermission> { new UnitPermission { UnitId = 99, PermissionType = PermissionType.LesNyheter } });
                A.CallTo(() => UnitQueries.GetByIdList(A<IEnumerable<int>>.Ignored)).Returns(new List<UnitWithParent> { new UnitWithParent { Id = unitId, ParentId = null, Type = UnitType.Company } });

                var list = await AuthorizationService.GetUnitPermissions(userId, null, PermissionType.EndreKompetanseType);

                Assert.That(list != null && list.Count() == 2);
                Assert.That(list.First().PermissionType == PermissionType.EndreKompetanseType && list.First().UnitId == UnitConstants.MasterUnitId);
                Assert.That(list.Last().PermissionType == PermissionType.LesNyheter && list.Last().UnitId == unitId);
            }

            [Test]
            public async Task OneDepartmentPermission_ReturnsForCompany()
            {
                int unitId = 99;
                int departmentId = 88;
                int userId = 1;

                A.CallTo(() => PermissionService.GetImplicitByUnitType(UnitType.Master)).Returns(new List<PermissionType> { PermissionType.EndreKompetanseType });
                A.CallTo(() => AuthorizationQueries.GetUnitPermissionsByUserId(A<int>.Ignored, A<int?>.Ignored, A<PermissionType[]>.Ignored)).Returns(new List<UnitPermission> { new UnitPermission { UnitId = departmentId, PermissionType = PermissionType.LesNyheter } });
                A.CallTo(() => UnitQueries.GetByIdList(A<IEnumerable<int>>.Ignored)).Returns(new List<UnitWithParent> { new UnitWithParent { Id = departmentId, ParentId = unitId, Type = UnitType.Department } });
                A.CallTo(() => PermissionService.GetByUnitType(UnitType.Department)).Returns(new List<PermissionType>());
                A.CallTo(() => UnitQueries.GetHierarchyUp(departmentId)).Returns(new List<UnitWithParent> { new UnitWithParent { Id = unitId, ParentId = null, Type = UnitType.Company } });

                var list = await AuthorizationService.GetUnitPermissions(userId, null, PermissionType.EndreKompetanseType);

                Assert.That(list != null && list.Count() == 2);
                Assert.That(list.First().PermissionType == PermissionType.EndreKompetanseType && list.First().UnitId == UnitConstants.MasterUnitId);
                Assert.That(list.Last().PermissionType == PermissionType.LesNyheter && list.Last().UnitId == unitId);
            }
        }

        public class TestBase : IDisposable
        {
            protected IAuthorizationQueries AuthorizationQueries { get; set; }
            protected IPermissionService PermissionService { get; set; }
            protected IUnitQueries UnitQueries { get; set; }
            protected ILogger<AuthorizationService> LogService { get; set; }

            protected AuthorizationService AuthorizationService { get; set; }

            [OneTimeSetUp]
            protected virtual async Task Setup()
            {
                AuthorizationQueries = A.Fake<IAuthorizationQueries>();
                PermissionService = A.Fake<IPermissionService>();
                UnitQueries = A.Fake<IUnitQueries>();
                LogService = A.Fake<ILogger<AuthorizationService>>();

                AuthorizationService = new AuthorizationService(AuthorizationQueries, PermissionService, UnitQueries, LogService);
            }

            [OneTimeTearDown]
            public async Task TearDown()
            {
            }

            public void Dispose()
            {
            }
        }
    }
}
