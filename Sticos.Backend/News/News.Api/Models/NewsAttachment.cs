using Shared.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Api.Models
{
    [Table("NyhetVedlegg")]
    public class NewsAttachment : EntityBase<NewsAttachment, int>
    {
        [Required]
        [Column("Nyhet")]
        public int NewsId { get; set; }
        [Required]
        [Column("Tittel")]
        public string Title { get; set; }
        [Required]
        [Column("FilId")]
        public int? FileId { get; set; }

        [ForeignKey("NewsId")]
        public News News { get; set; }
    }
}
