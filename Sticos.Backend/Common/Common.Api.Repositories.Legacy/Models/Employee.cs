using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Api.Domain.Interfaces.Employees;

namespace Common.Api.Repositories.Legacy.Models
{
    [Table("HrAnsatt")]
    public class Employee : IEmployee
    {
        public int Id { get; set; }
        
        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("Enhet")]
        public int? UnitId { get; set; }

        [Column("Fornavn")]
        public string FirstName { get; set; }

        [Column("Etternavn")]
        public string LastName { get; set; }

        [Column("Tittel")]
        public string JobTitle { get; set; }

        
        [Column("Adresse")]
        public string Address { get; set; }

        [Column("Kjønn")]
        public int Sex { get; set; }


        [Column("ErSlettet")]
        public bool IsDeleted { get; set; }

        [Column("AnsettelseStartDato")]
        public DateTime EmployeeStartDate { get; set; }

        [Column("AnsettelseSluttDato")]
        public DateTime EmployeeEndDate { get; set; }

        //[Column("Bilde")]
        [NotMapped]
        public string Image { get; set; }

        //Set by joining in dbo.Bruker in repository
        [NotMapped]
        public string Email { get; set; }
        [NotMapped]
        public string Phone { get; set; }

        [NotMapped]
        public string NationalIdentificationNumber { get; set; }       
    }
}