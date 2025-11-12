using H_Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RoomConfig : IBaseEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {

        builder.ToTable("Rooms");
        builder.HasKey(h => h.RoomId);

        builder.Property(h => h.RoomId)
            .HasColumnName("RoomId")
            .ValueGeneratedOnAdd();

        builder.Property(h => h.RoomNumber)
            .HasColumnName("RoomNumber")
            .HasConversion<string>()
            .UseCollation("Khmer_100_CI_AI_SC_UTF8")
            .HasMaxLength(10);

        builder.Property(h => h.RoomTypeId)
           .HasColumnName("RoomTypeId")
           .HasConversion<int>()
           ;
        builder.Property(h => h.Images)
          .HasColumnName("Images")
          .HasConversion<string>()

           ;
        builder.Property(h => h.Status)
           .HasColumnName("Status")
           .HasConversion<string>()
            .UseCollation("Khmer_100_CI_AI_SC_UTF8")
            .HasMaxLength(20);


        builder.Property(h => h.HotelId)
          .HasColumnName("HotelId")
          .HasConversion<int>();


        builder.HasOne(h => h.roomType)
            .WithMany(r => r.Rooms)
            .HasForeignKey(h => h.RoomTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.hotel)        // navigation on Room
     .WithMany(h => h.Rooms)         // navigation on Hotels
     .HasForeignKey(r => r.HotelId)  // FK property
     .OnDelete(DeleteBehavior.Restrict);




    }
}
