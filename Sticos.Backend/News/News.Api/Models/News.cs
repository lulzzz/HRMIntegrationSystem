using Shared.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Api.Models
{
    [Table("Nyhet")]
    public class News : EntityBase<News, int>
    {
        public News()
        {
            Attachments = new HashSet<NewsAttachment>();
        }

        [Column("BildeFil")]
        public int? ImageId { get; set; }
        [Column("Enhet")]
        public int? UnitId { get; set; }

        [Required]
        [Column("Tittel")]
        public string Title { get; set; }
        [Required]
        [Column("Forfatter")]
        [MaxLength(60)]
        public string Author { get; set; }
        [Required]
        [EmailAddress]
        [Column("Epost")]
        public string Email { get; set; }
        [Required]
        [Column("Tekst")]
        public string Text { get; set; }
        [Column("GjelderFraDato")]
        public DateTime? FromDate { get; set; }
        [Column("GjelderTilDato")]
        public DateTime? ToDate { get; set; }
        [Column("Slettet")]
        public bool IsDeleted { get; set; }
        [Column("EpostErSendt")]
        public bool IsEmailSent { get; set; }

        public ICollection<NewsAttachment> Attachments { get; set; }
    }
}
