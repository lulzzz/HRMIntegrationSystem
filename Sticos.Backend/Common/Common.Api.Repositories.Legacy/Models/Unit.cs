using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Common.Api.Repositories.Legacy.Models
{
    [Table("Enhet-InnholdBok")]
    public class UnitContentBook
    {
        [Column("OID")]
        public int Id { get; set; }
        
        public int? OptimisticLockField { get; set; }

        [Column("Enhet")]
        public int UnitId { get; set; }
        [Column("InnholdBok")]
        public int ContentBookId { get; set; }

    }

    [Table("Enhet")]
    public class Unit
    {
        public int Id { get; set; }
        
        [Column("Navn")]
        public string Name { get; set; }

        [Column("FysiskOgKjemiskMiljø")]
        public string PhysicalAndChemicalEnvironment { get; set; }
        
        [NotMapped]
        public string BusinessOrganizationNumber  => FirstValidVerification?.SubId;
        
        [NotMapped]
        public string LegalOrganizationNumber => FirstValidVerification?.MainId;

        private OrgUnitVerification FirstValidVerification => OrgUnitVerifications?.FirstOrDefault(x => x.Status == OrgNrStatus.Verified);

        public UnitType Type { get; set; }

        [Column("Parent")]
        public int? ParentId { get; set; }
        
        [Column("ErSlettet")]
        public bool IsDeleted { get; set; }

        [Column("VisLogoHandbok")]
        public bool ShowLogoForManual { get; set; }
        
        [Column("HarTariffAvtale")]
        public bool HasCollectiveAgreement { get; set; }
                
        [Column("ErIAVirksomhet")]
        public bool IsIAVCompany { get; set; }

        [Column("HarKjørtVeileder")]
        public bool HaveRunVeileder { get; set; }
        
        [Column("VisKalenderPåIntranett")]
        public bool ShowCalendarOnIntranet { get; set; }    
        
        [Column("VisÅrsrapportHmsForBruker")]
        public bool ShowHseYearReport { get; set; }

        
        public ICollection<OrgUnitVerification> OrgUnitVerifications { get; set; }
    }

    public enum UnitType
    {
        Konsern = 1,
        Region = 2,
        Enhet = 3,
        Avdeling = 4,
        Seksjon = 5,
        Team = 6,
        Gruppe = 7
    }
}