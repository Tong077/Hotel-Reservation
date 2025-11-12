using H_Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace H_Domain.Configuration
{
    public class CurrencyConfiguration : IBaseEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("currencies");
            builder.HasKey(h => h.CurrencyId);

            builder.Property(h => h.CurrencyId)
                .HasColumnName("CurrencyId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.FromCurrency)
                .HasColumnName("FromCurrency")
                .HasConversion<string>()
                .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                .HasMaxLength(100);

            builder.Property(h => h.ToCurrency)
               .HasColumnName("ToCurrency")
               .HasConversion<string>()
               .UseCollation("Khmer_100_CI_AI_SC_UTF8")
               .HasMaxLength(200);

            builder.Property(h => h.ExchangeRate)
               .HasColumnName("ExchangeRate")
               .HasConversion<string>()
               .UseCollation("Khmer_100_CI_AI_SC_UTF8")
               .HasMaxLength(200);

            builder.Property(h => h.EffectiveDate)
              .HasColumnName("EffectiveDate")
              .HasConversion<DateTime>();

            builder.Property(h => h.EndDate)
              .HasColumnName("EndDate")
              .HasConversion<DateTime>();

            builder.Property(h => h.IsBaseRate)
              .HasColumnName("IsBaseRate")
              .HasConversion<bool>();
            builder.Property(h => h.CreatedAt)
          .HasColumnName("CreatedAt")
          .HasConversion<DateTime>()
          .HasDefaultValueSql("getdate()");

            builder.Property(h => h.UpdatedAt)
          .HasColumnName("UpdatedAt")
          .HasConversion<DateTime>()
          .HasDefaultValueSql("getdate()");
        }
    }
}
