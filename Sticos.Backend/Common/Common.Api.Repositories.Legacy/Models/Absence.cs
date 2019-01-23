using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Api.Repositories.Legacy.Models
{
    [Table("HrFravær")]
    public class Absence
    {
        public int Id { get; set; }
        
        [Column("HrAnsatt")]
        public int EmployeeId { get; set; }

        [Column("Type")]
        public int Type { get; set; }

        [Column("Undertype")]
        public int SubType { get; set; }

        [Column("Status")]
        public int Status { get; set; }
       
        [Column("FraTidspunkt")]
        public DateTime From { get; set; }
        
        [Column("TilTidspunkt")]
        public DateTime To { get; set; }

        [Column("FraOgTilTidspunktHarTid")] 
        public bool FromAndToHasTime { get; set; }

        [Column("FerieIPermisjon")]
        public bool VacationInLeave { get; set; }

        [Column("FerieFraDato")]
        public DateTime? VacationFrom { get; set; }

        [Column("FerieTilDato")]
        public DateTime? VacationTo { get; set; }

        [Column("OpprettetTidspunkt")]
        public DateTime? CreatedAt { get; set; }    
    }
}