using Microsoft.EntityFrameworkCore;

namespace InternationalizationService.Models.DBModels
{
    public partial class InternationalizationServiceContext : DbContext
    {
        public InternationalizationServiceContext()
        {
        }

        public InternationalizationServiceContext(DbContextOptions<InternationalizationServiceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Currencies> Currencies { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Nationalities> Nationalities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.CountryId).HasName("PRIMARY");

                entity.ToTable("countries");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nvarchar(50)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("char(2)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");
            });

            modelBuilder.Entity<Currencies>(entity =>
            {
                entity.HasKey(e => e.CurrencyId).HasName("PRIMARY");

                entity.ToTable("currencies");

                entity.Property(e => e.CurrencyId).HasColumnName("currency_id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nvarchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasColumnType("char(3)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.Symbol)
                    .HasColumnName("symbol")
                    .HasColumnType("nvarchar(3)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.Country)
                    .WithOne(p => p.Currency)
                    .HasForeignKey<Currencies>(d => d.CountryId)
                    .HasConstraintName("country_currency_ibfk_1");
            });

            modelBuilder.Entity<Nationalities>(entity =>
            {
                entity.HasKey(e => e.NationalityId).HasName("PRIMARY");

                entity.ToTable("nationalities");

                entity.Property(e => e.NationalityId).HasColumnName("nationality_id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("nvarchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("timestamp");

                entity.HasOne(d => d.Country)
                    .WithOne(p => p.Nationality)
                    .HasForeignKey<Nationalities>(d => d.CountryId)
                    .HasConstraintName("country_nationality_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
