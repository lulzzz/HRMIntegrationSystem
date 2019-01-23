namespace Shared.Domain.Enums
{
    public enum PermissionType
    {
        Unknown = 0,

        /* Generelle ressurser*/
        AdminKunde = 2,
        AdminHR = 6,

        OpprettTilgang = 7,

        /* Enhets spesifikke ressurser  */
        EnhetsOppsett = 8,

        LesNyheter = 12,
        SkrivNyheter,

        LesOrganisasjonsOppsett,
        SkrivOrganisasjonsOppsett,

        LesPersonalRegister,

        OpprettAnsatt = 18,
        ImportereAnsatte,

        SkrivFilArkiv,
        SkrivKalender,

        /* PERSONAL */
        LesPersonalHåndbok,
        SkrivPersonalHåndbok,

        PersonalHåndbokUnderArbeid,
        VedlikeholdPersonalHåndbok,
        PersonalHåndbokVisSkjulLovForskriftSkjema,
        SkrivUtPersonalHåndbok,
        PersonalHåndbokTilgangForslagstekster,

        /* HMS */
        LesHMSHåndbok,
        SkrivHMSHåndbok,

        HMSHåndbokUnderArbeid,
        VedlikeholdHMSHåndbok,
        HMSHåndbokVisSkjulLovForskriftSkjema,
        SkrivUtHMSHåndbok,
        HMSHåndbokTilgangForslagstekster,

        /* HMS-hjul */
        LesHMSHjul,
        SkrivHMSHjul,

        /* Avvik */
        RegistrerAvvik,
        BehandleAvvik,
        LesAvviksliste,
        SkrivAvanserteAvvik,

        LesHandlingsplan,
        SkrivHandlingsplan,
        SkrivUtHandlingsplan,

        SkrivÅrsrapport = 46,

        LesStoffKartotek = 47,
        SkrivStoffKartotek = 48,

        OverførTilRisiko = 51,
        SkrivRisikoliste = 52,

        /* HR*/
        LesFerieOversikt = 54,

        /* RAPPORTER */
        KompetanseRapport,
        SensitiveRapporter,
        IkkeSensitiveRapporter,

        /* Ansatt spesifikke ressurser  */
        LesFravær,
        BehandleFravær,
        RegistrereFravær,
        FølgeOppFravær,
        SkrivFeriebank,
        LesKompetanse,
        SkrivKompetanse,
        SkrivDokumenter,
        SkrivTilganger,
        LesProfil,
        SkrivProfil,
        LesFeriebank,
        SkrivRettigheter,
        SkrivKonsernRoller = 71,
        SkrivPersonalAnsvar,
        LesHistorikk,
        LesAdminInnhold = 74,
        SkrivInfobokser,
        LesRisikoliste = 76,
        RegistrereGodkjentFravær = 77,
        EndreKompetanseType = 78,
        LesDokumenter = 79,
        EndreErAktiv = 80,

        /* PERSONAL */
        LesLederHåndbok = 81,
        SkrivLederHåndbok = 82,

        LederHåndbokUnderArbeid = 83,
        VedlikeholdLederHåndbok = 84,
        LederHåndbokVisSkjulLovForskriftSkjema = 85,
        SkrivUtLederHåndbok = 86,
        LederHåndbokTilgangForslagstekster = 87,
        Reiseregning = 88
    }
}
