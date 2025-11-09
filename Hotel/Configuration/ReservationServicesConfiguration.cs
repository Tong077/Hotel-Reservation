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
    public class ReservationServicesConfiguration : IBaseEntityTypeConfiguration<ReservationService>
    {
        public void Configure(EntityTypeBuilder<ReservationService> builder)
        {
            builder.ToTable("ReservationServices");
            builder.HasKey(h => h.ReservationServiceId);

            builder.Property(h => h.ReservationServiceId)
                .HasColumnName("ReservationServiceId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.ReservationId)
                .HasColumnName("ReservationId")
                .HasConversion<int>()
               ;

            builder.Property(h => h.ServiceId)
               .HasColumnName("ServiceId")
               .HasConversion<int>();


            builder.Property(h => h.Quantity)
               .HasColumnName("Quantity ")
               .HasConversion<int>();

            builder.Property(h => h.TotalPrice)
              .HasColumnName("TotalPrice")
              .HasConversion<decimal>()
              ;
        }
    }
}
