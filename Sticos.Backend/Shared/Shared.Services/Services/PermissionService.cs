using Shared.Domain.Enums;
using Shared.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Services.Services
{
    public class PermissionService : IPermissionService
    {
        public PermissionService()
        {
        }

        public Task<IEnumerable<PermissionType>> GetByUnitType(UnitType unitType)
        {
            List<PermissionType> list;

            switch (unitType)
            {
                case UnitType.Master:
                    list = new List<PermissionType> {
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
                    break;

                case UnitType.Company:
                    list = new List<PermissionType>
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
                    break;

                case UnitType.Department:
                    list = new List<PermissionType>
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
                    break;

                default:
                    list = new List<PermissionType>();
                    break;
            }

            return Task.FromResult(list.OfType<PermissionType>());
        }

        public Task<IEnumerable<PermissionType>> GetImplicitByUnitType(UnitType unitType)
        {
            List<PermissionType> list;

            switch (unitType)
            {
                case UnitType.Master:
                    list = new List<PermissionType> { PermissionType.LesNyheter };
                    break;

                default:
                    list = new List<PermissionType>();
                    break;
            }

            return Task.FromResult(list.OfType<PermissionType>());
        }
    }
}
