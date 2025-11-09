using H_Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ReservationConfig : IBaseEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");
        builder.HasKey(h => h.ReservationId);

        builder.Property(h => h.ReservationId)
            .HasColumnName("ReservationId")
            .ValueGeneratedOnAdd();

        builder.Property(h => h.GuestId)
            .HasColumnName("GuestId")
            .HasConversion<int>()
           ;
        builder.Property(h => h.PaymentId)
            .HasColumnName("PaymentId")
            .HasConversion<int>();
        builder.Property(h => h.RoomId)
           .HasColumnName("RoomId")
           .HasConversion<int>();

        builder.Property(h => h.PaymentId)
            .HasColumnName("PaymentId")
            .HasConversion<int>();

        builder.Property(h => h.CheckInDate)
           .HasColumnName("CheckInDate")
           .HasConversion<DateTime>()
           ;

        builder.Property(h => h.CheckOutDate)
          .HasColumnName("CheckOutDate")
          .HasConversion<DateTime>()
          ;


        builder.Property(h => h.TotalPrice)
          .HasColumnName("TotalPrice")
          .HasConversion<decimal>();

        builder.Property(h => h.Currency)
          .HasColumnName("Currency")
          .HasConversion<string>()
          .UseCollation("Khmer_100_CI_AI_SC_UTF8")
          .HasMaxLength(50);

        builder.Property(h => h.Status)
          .HasColumnName("Status")
          .HasConversion<string>()
          .UseCollation("Khmer_100_CI_AI_SC_UTF8")
          .HasMaxLength(20);
        builder.Property(h => h.CreatedAt)
          .HasColumnName("CreatedAt")
          .HasConversion<DateTime>()
          .HasDefaultValueSql("getdate()");

    }
}
