using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Api.Repositories.Legacy.Models
{
    [Table("HrAnsattStillingsprosent")]
    public class Employment
    {
        public int Id { get; set; }

        [Column("FraDato")]
        public DateTime StartDate { get; set; }

        [Column("Stillingsprosent")]
        public decimal Percentage { get; set; }
    
        [Column("HrAnsatt")]
        public int EmployeeId { get; set; }
    }
}