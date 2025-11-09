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
    public class HousekeepingConfiguration : IBaseEntityTypeConfiguration<Housekeeping>
    {
        public void Configure(EntityTypeBuilder<Housekeeping> builder)
        {
            builder.ToTable("Housekeeping")
                .HasKey(h => h.HousekeepingId);

            builder.Property(h => h.HousekeepingId)
                .HasColumnName("HousekeepingId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.RoomId)
                .HasColumnName("RoomId")
                .HasConversion<int>();

            builder.Property(h => h.EmployeeId)
                .HasColumnName("EmployeeId")
                .HasConversion<int>();

            builder.Property(h => h.Status)
                .HasColumnName("Status")
                .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(h => h.LastCleanedDate)
                .HasColumnName("LastCleanedDate")
                .HasConversion<DateTime>();

            builder.Property(h => h.Notes)
                .HasColumnName("Notes")
                .UseCollation("Khmer_100_CI_AI_SC_UTF8")
                .HasConversion<string>()
                .HasMaxLength(200);


            builder.HasOne(h => h.Room)
               .WithMany(r => r.Housekeepings)
               .HasForeignKey(h => h.RoomId)
               .OnDelete(DeleteBehavior.Restrict); 

           
            builder.HasOne(h => h.Employee)
                .WithMany(e => e.HousekeepingTasks)
                .HasForeignKey(h => h.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
