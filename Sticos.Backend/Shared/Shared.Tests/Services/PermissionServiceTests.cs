using NUnit.Framework;
using Shared.Domain.Enums;
using Shared.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Tests.Services
{
    public class PermissionServiceTests
    {
        [TestFixture]
        public class GetByUnitType : TestBase
        {
            private List<PermissionType> masterTypes => new List<PermissionType>
            {
                PermissionType.SkrivFilArkiv,
                PermissionType.LesPersonalHåndbok,
                PermissionType.PersonalHåndbokTilgangForslagstekster,
                PermissionType.PersonalHåndbokUnderArbeid,
                PermissionType.PersonalHåndbokVisSkjulLovForskriftSkjema,
                PermissionType.SkrivPersonalHåndbok,
                PermissionType.SkrivUtPersonalHåndbok,
                PermissionType.VedlikeholdPersonalHåndbok,
                PermissionType.HMSHåndbokTilgangForslagstekster,
                PermissionType.HMSHåndbokUnderArbeid,
                PermissionType.HMSHåndbokVisSkjulLovForskriftSkjema,
                PermissionType.LesHMSHåndbok,
                PermissionType.SkrivHMSHjul,
                PermissionType.SkrivHMSHåndbok,
                PermissionType.SkrivStoffKartotek,
                PermissionType.SkrivUtHMSHåndbok,
                PermissionType.VedlikeholdHMSHåndbok,
                PermissionType.LederHåndbokTilgangForslagstekster,
                PermissionType.LederHåndbokUnderArbeid,
                PermissionType.LederHåndbokVisSkjulLovForskriftSkjema,
                PermissionType.LesLederHåndbok,
                PermissionType.SkrivLederHåndbok,
                PermissionType.SkrivUtLederHåndbok,
                PermissionType.VedlikeholdLederHåndbok
            };

            private List<PermissionType> companyTypes => new List<PermissionType>
            {
                PermissionType.AdminHR,
                PermissionType.EndreErAktiv,
                PermissionType.SkrivKonsernRoller,
                PermissionType.SkrivPersonalAnsvar,
                PermissionType.SkrivProfil,
                PermissionType.SkrivRettigheter,
                PermissionType.SkrivTilganger,
                PermissionType.EnhetsOppsett,
                PermissionType.LesAdminInnhold,
                PermissionType.LesNyheter,
                PermissionType.LesOrganisasjonsOppsett,
                PermissionType.LesPersonalRegister,
                PermissionType.OpprettAnsatt,
                PermissionType.OpprettTilgang,
                PermissionType.SkrivInfobokser,
                PermissionType.LesHistorikk,
                PermissionType.LesProfil,
                PermissionType.SkrivFilArkiv,
                PermissionType.SkrivNyheter,
                PermissionType.LesPersonalHåndbok,
                PermissionType.PersonalHåndbokTilgangForslagstekster,
                PermissionType.PersonalHåndbokUnderArbeid,
                PermissionType.PersonalHåndbokVisSkjulLovForskriftSkjema,
                PermissionType.SkrivPersonalHåndbok,
                PermissionType.SkrivUtPersonalHåndbok,
                PermissionType.VedlikeholdPersonalHåndbok,
                PermissionType.BehandleAvvik,
                PermissionType.SkrivRisikoliste,
                PermissionType.HMSHåndbokTilgangForslagstekster,
                PermissionType.HMSHåndbokUnderArbeid,
                PermissionType.HMSHåndbokVisSkjulLovForskriftSkjema,
                PermissionType.LesRisikoliste,
                PermissionType.LesAvviksliste,
                PermissionType.LesHandlingsplan,
                PermissionType.LesHMSHjul,
                PermissionType.LesHMSHåndbok,
                PermissionType.LesStoffKartotek,
                PermissionType.OverførTilRisiko,
                PermissionType.RegistrerAvvik,
                PermissionType.SkrivAvanserteAvvik,
                PermissionType.SkrivHandlingsplan,
                PermissionType.SkrivHMSHjul,
                PermissionType.SkrivHMSHåndbok,
                PermissionType.SkrivStoffKartotek,
                PermissionType.SkrivUtHandlingsplan,
                PermissionType.SkrivUtHMSHåndbok,
                PermissionType.SkrivÅrsrapport,
                PermissionType.VedlikeholdHMSHåndbok,
                PermissionType.BehandleFravær,
                PermissionType.SkrivDokumenter,
                PermissionType.SkrivFeriebank,
                PermissionType.SkrivKompetanse,
                PermissionType.EndreKompetanseType,
                PermissionType.FølgeOppFravær,
                PermissionType.IkkeSensitiveRapporter,
                PermissionType.KompetanseRapport,
                PermissionType.LesFerieOversikt,
                PermissionType.RegistrereGodkjentFravær,
                PermissionType.RegistrereFravær,
                PermissionType.Reiseregning,
                PermissionType.LesDokumenter,
                PermissionType.LesFeriebank,
                PermissionType.LesFravær,
                PermissionType.LesKompetanse,
                PermissionType.SensitiveRapporter,
                PermissionType.LederHåndbokTilgangForslagstekster,
                PermissionType.LederHåndbokUnderArbeid,
                PermissionType.LederHåndbokVisSkjulLovForskriftSkjema,
                PermissionType.LesLederHåndbok,
                PermissionType.SkrivLederHåndbok,
                PermissionType.SkrivUtLederHåndbok,
                PermissionType.VedlikeholdLederHåndbok
            };

            private List<PermissionType> departmentTypes => new List<PermissionType>
            {
                PermissionType.BehandleFravær,
                PermissionType.SkrivDokumenter,
                PermissionType.SkrivFeriebank,
                PermissionType.SkrivKompetanse,
                PermissionType.EndreKompetanseType,
                PermissionType.FølgeOppFravær,
                PermissionType.IkkeSensitiveRapporter,
                PermissionType.KompetanseRapport,
                PermissionType.LesFerieOversikt,
                PermissionType.RegistrereGodkjentFravær,
                PermissionType.RegistrereFravær,
                PermissionType.Reiseregning,
                PermissionType.LesDokumenter,
                PermissionType.LesFeriebank,
                PermissionType.LesFravær,
                PermissionType.LesKompetanse,
                PermissionType.SensitiveRapporter
            };

            [Test]
            public async Task MasterPermissions()
            {
                var list = await PermissionService.GetByUnitType(UnitType.Master);

                Assert.AreEqual(masterTypes.Count(), list.Count());
                Assert.AreEqual(masterTypes.Count(), list.Intersect(masterTypes).Count());
            }

            [Test]
            public async Task CompanyPermissions()
            {
                var list = await PermissionService.GetByUnitType(UnitType.Company);

                Assert.AreEqual(companyTypes.Count(), list.Count());
                Assert.AreEqual(companyTypes.Count(), list.Intersect(companyTypes).Count());
            }

            [Test]
            public async Task DepartmentPermissions()
            {
                var list = await PermissionService.GetByUnitType(UnitType.Department);

                Assert.AreEqual(departmentTypes.Count(), list.Count());
                Assert.AreEqual(departmentTypes.Count(), list.Intersect(departmentTypes).Count());
            }
        }

        [TestFixture]
        public class GetImplicitByUnitType : TestBase
        {
            private List<PermissionType> masterTypes => new List<PermissionType> { PermissionType.LesNyheter };
            private List<PermissionType> companyTypes => new List<PermissionType>();
            private List<PermissionType> departmentTypes => new List<PermissionType>();

            [Test]
            public async Task MasterPermissions()
            {
                var list = await PermissionService.GetImplicitByUnitType(UnitType.Master);

                Assert.AreEqual(masterTypes.Count(), list.Count());
                Assert.AreEqual(masterTypes.Count(), list.Intersect(masterTypes).Count());
            }

            [Test]
            public async Task CompanyPermissions()
            {
                var list = await PermissionService.GetImplicitByUnitType(UnitType.Company);

                Assert.AreEqual(companyTypes.Count(), list.Count());
                Assert.AreEqual(companyTypes.Count(), list.Intersect(companyTypes).Count());
            }

            [Test]
            public async Task DepartmentPermissions()
            {
                var list = await PermissionService.GetImplicitByUnitType(UnitType.Department);

                Assert.AreEqual(departmentTypes.Count(), list.Count());
                Assert.AreEqual(departmentTypes.Count(), list.Intersect(departmentTypes).Count());
            }
        }

        public class TestBase : IDisposable
        {
            protected PermissionService PermissionService { get; set; }

            [OneTimeSetUp]
            protected virtual async Task Setup()
            {
                PermissionService = new PermissionService();
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
