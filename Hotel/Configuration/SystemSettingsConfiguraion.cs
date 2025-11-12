using H_Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace H_Domain.Configuration
{
    public class SystemSettingsConfiguraion : IBaseEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.ToTable("SystemSettings")
                 .HasKey(h => h.SettingId);

            builder.Property(h => h.SettingId)
                .HasColumnName("SettingId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.Key)
                .HasColumnName("Key")
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(h => h.Value)
                .HasColumnName("Value")
                .HasConversion<string>()
                .HasMaxLength(200);

            builder.Property(h => h.Description)
                .HasColumnName("Description")
                .HasConversion<string>()
                .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                .HasMaxLength(200);

            builder.Property(h => h.Category)
                .HasColumnName("Category")
                .HasConversion<string>()
                 .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                 .HasMaxLength(200);

            builder.Property(h => h.IsActive)
                .HasColumnName("IsActive")
                .HasConversion<bool>();

            builder.Property(h => h.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasConversion<DateTime>()
                 .HasDefaultValueSql("getdate()");

            builder.Property(h => h.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasConversion<DateTime>()
                .HasDefaultValueSql("getdate()");
        }
    }
}
