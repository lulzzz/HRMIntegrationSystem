using Microsoft.EntityFrameworkCore;

namespace News.Api.Repository
{
    public class NewsContext : DbContext
    {
        public NewsContext()
        {
        }

        public NewsContext(DbContextOptions<NewsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.News> News { get; set; }
        public virtual DbSet<Models.NewsAttachment> NewsAttachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.News>(entity =>
            {
                entity.HasIndex(e => e.ImageId).HasName("iBildeFil_Nyhet");
                entity.HasIndex(e => e.UnitId).HasName("iEnhet_Nyhet");

                entity.HasMany(n => n.Attachments).WithOne(na => na.News);

                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Author).HasMaxLength(60);
                entity.Property(e => e.FromDate).HasColumnType("datetime");
                entity.Property(e => e.ToDate).HasColumnType("datetime");
                entity.Property(e => e.Title).HasMaxLength(100);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
                entity.Property(e => e.IsEmailSent).HasDefaultValue(false);

                entity.HasQueryFilter(e => e.IsDeleted == false);
            });

            modelBuilder.Entity<Models.NewsAttachment>(entity =>
            {
                entity.HasIndex(e => e.NewsId).HasName("iNyhet_NyhetVedlegg");

                entity.Property(e => e.Title).HasMaxLength(100);

                entity.HasOne(d => d.News)
                    .WithMany(p => p.Attachments)
                    .HasForeignKey(d => d.NewsId)
                    .HasConstraintName("FK_NyhetVedlegg_Nyhet");
            });
        }
    }
}
