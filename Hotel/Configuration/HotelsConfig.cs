using H_Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class HotelsConfig : IBaseEntityTypeConfiguration<Hotels>
{
    public void Configure(EntityTypeBuilder<Hotels> builder)
    {

        builder.ToTable("Hotels");
        builder.HasKey(h => h.HotelId);

        builder.Property(h => h.HotelId)
            .HasColumnName("HotelId")
            .ValueGeneratedOnAdd();

        builder.Property(h => h.Name)
            .HasColumnName("Name")
            .HasConversion<string>()
            .UseCollation("Khmer_100_CI_AI_SC_UTF8")
            .HasMaxLength(100);

        builder.Property(h => h.Address)
           .HasColumnName("Address")
           .HasConversion<string>()
           .UseCollation("Khmer_100_CI_AI_SC_UTF8")
           .HasMaxLength(200);

        builder.Property(h => h.City)
           .HasColumnName("City")
           .HasConversion<string>()
           .UseCollation("Khmer_100_CI_AI_SC_UTF8")
           .HasMaxLength(50);

        builder.Property(h => h.Email)
            .HasColumnName("Email")
            .HasConversion<string>()
            .HasMaxLength(100);

        builder.Property(h => h.Phone)
          .HasColumnName("Phone")
          .HasConversion<string>()
          .UseCollation("Khmer_100_CI_AI_SC_UTF8")
          .HasMaxLength(20);

    }
}

