using Common.Api.Domain.Interfaces.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Api.Repositories.Legacy.Models
{
    [Table("Bruker")]
    public class User: IUser
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        [Column("EnhetId")]
        public int? UnitId { get; set; }

        [Column("Kunde")]
        public int? CustomerId { get; set; }

        [Column("Epost")]
        public string Email { get; set; }
        
        [Column("Mobiltelefon")]
        public string Mobilephone { get; set; }

        [Column("Jobbtelefon")]
        public string Workphone { get; set; }

        [Column("ErSlettet")]
        public bool IsDeleted { get; set; }

        [Column("ErAktiv")]
        public bool? IsActive { get; set; }

        [Column("Fornavn")]
        public string FirstName { get; set; }

        [Column("Etternavn")]
        public string LastName { get; set; }

        [Column("ErSA")]
        public bool IsSA { get; set; }

        [NotMapped]
        public bool IsPersonalCustomerAdmin { get; set; }
    }
}