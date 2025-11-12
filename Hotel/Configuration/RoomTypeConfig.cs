using H_Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoomTypeConfig : IBaseEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("RoomTypes");
        builder.HasKey(h => h.RoomTypeId);

        builder.Property(h => h.RoomTypeId)
            .HasColumnName("RoomTypeId")
            .ValueGeneratedOnAdd();

        builder.Property(h => h.Name)
            .HasColumnName("Name")
            .HasConversion<string>()
            .UseCollation("Khmer_100_CI_AI_SC_UTF8")
            .HasMaxLength(100);

        builder.Property(h => h.Description)
           .HasColumnName("Description")
           .HasConversion<string>()
           .UseCollation("Khmer_100_CI_AI_SC_UTF8")
           .HasMaxLength(200);
        builder.Property(h => h.Currency)
          .HasColumnName("Currency")
          .HasConversion<string>()
          .UseCollation("Khmer_100_CI_AI_SC_UTF8")
          .HasMaxLength(50);
        builder.Property(h => h.PricePerNight)
           .HasColumnName("PricePerNight")
           .HasConversion<decimal>();


        builder.Property(h => h.Capacity)
          .HasColumnName("Capacity")
          .HasConversion<decimal>();

    }
}

