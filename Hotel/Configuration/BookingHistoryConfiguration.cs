using H_Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Configuration
{
    public class BookingHistoryConfiguration : IBaseEntityTypeConfiguration<BookingHistory>
    {
        public void Configure(EntityTypeBuilder<BookingHistory> builder)
        {
            builder.ToTable("BookingHistory");
            builder.HasKey(h => h.HistoryId);

            builder.Property(h => h.HistoryId)
                .HasColumnName("HistoryId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.ReservationId)
                .HasColumnName("ReservationId")
                .HasConversion<int>()
               ;

            builder.Property(h => h.GuestId)
               .HasColumnName("GuestId")
               .HasConversion<int>();


            builder.Property(h => h.RoomId)
               .HasColumnName("RoomId ")
               .HasConversion<int>()
               ;

            builder.Property(h => h.CheckInDate)
              .HasColumnName("CheckInDate")
              .HasConversion<DateTime>()
              ;
            builder.Property(h => h.CheckOutDate)
              .HasColumnName("CheckOutDate")
              .HasConversion<DateTime>()
              ;


            builder.Property(h => h.TotalAmount)
              .HasColumnName("TotalAmount")
              .HasConversion<decimal>();

            builder.Property(h => h.Status)
              .HasColumnName("Status")
              .HasConversion<string>()
              .UseCollation("Khmer_100_CI_AI_SC_UTF8")
              .HasMaxLength(20);

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
}
