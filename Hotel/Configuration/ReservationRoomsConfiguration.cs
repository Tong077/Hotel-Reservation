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
    public class ReservationRoomsConfiguration : IBaseEntityTypeConfiguration<ReservationRooms>
    {
        public void Configure(EntityTypeBuilder<ReservationRooms> builder)
        {
            builder.ToTable("ReservationRooms");
            builder.Property(x => x.Id)
                .HasColumnName("Id");
            builder.HasKey(x => x.Id);


            builder.Property(x => x.RoomId)
                .HasColumnName("RoomId")
                .HasConversion<int>();
            builder.Property(x => x.ReservatinId)
                .HasColumnName("ReservatinId")
                .HasConversion<int>();


        }
    }
}
