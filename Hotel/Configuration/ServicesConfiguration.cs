using H_Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace H_Domain.Configuration
{
    public class ServicesConfiguration : IBaseEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
            builder.ToTable("Services")
                .HasKey(h => h.ServiceId);
            builder.Property(h => h.ServiceId)
                .HasColumnName("ServiceId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.ServiceName)
                .HasColumnName("ServiceName")
                .HasConversion<string>()
                .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                .HasMaxLength(100);

            builder.Property(h => h.Category)
                .HasColumnName("Category")
                .HasConversion<string>()
                .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                .HasMaxLength(50);
            builder.Property(h => h.Price)
               .HasColumnName("Price")
               .HasConversion<decimal>();

            builder.Property(h => h.Currency)
                .HasColumnName("Currency")
                .HasConversion<string>()
                 .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                 .HasMaxLength(50);

            builder.Property(h => h.Description)
                .HasColumnName("Description")
                .HasConversion<string>()
                .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                .HasMaxLength(200);

            builder.Property(h => h.IsActive)
               .HasColumnName("IsActive")
               .HasConversion<bool>();
        }
    }
}
