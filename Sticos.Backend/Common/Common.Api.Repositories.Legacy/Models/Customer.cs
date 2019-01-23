using Common.Api.Domain.Interfaces.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Api.Repositories.Legacy.Models
{
    [Table("Kunde")]
    public class Customer
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("SticosId")]
        public int SticosCustomerId { get; set; }
    }
}