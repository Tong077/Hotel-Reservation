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
    public class EmployeesConfiguration : IBaseEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(h => h.EmployeeId);

            builder.Property(h => h.EmployeeId)
                .HasColumnName("EmployeeId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.FullName)
                .HasColumnName("FullName")
                .HasConversion<string>()
               .UseCollation("Khmer_100_CI_AI_SC_UTF8")
               .HasMaxLength(100);

            builder.Property(h => h.Email)
               .HasColumnName("Email")
               .HasConversion<string>()
              .UseCollation("Khmer_100_CI_AI_SC_UTF8")
              .HasMaxLength(100);

            builder.Property(h => h.Phone)
              .HasColumnName("Phone")
              .HasConversion<string>()
             .UseCollation("Khmer_100_CI_AI_SC_UTF8")
             .HasMaxLength(20);

            builder.Property(h => h.Role)
             .HasColumnName("Role")
             .HasConversion<string>()
            .UseCollation("Khmer_100_CI_AI_SC_UTF8")
            .HasMaxLength(50);

            builder.Property(h => h.ShiftTime)
            .HasColumnName("ShiftTime")
            .HasConversion<string>()
           .UseCollation("Khmer_100_CI_AI_SC_UTF8")
           .HasMaxLength(50);

            builder.Property(h => h.HireDate)
            .HasColumnName("HireDate")
            .HasConversion<DateTime>();

        }
    }
}
