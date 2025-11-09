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
    public class InvoicesConfiguration : IBaseEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.ToTable("Invoices");
            builder.HasKey(h => h.InvoiceId);

            builder.Property(h => h.InvoiceId)
                .HasColumnName("InvoiceId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.ReservationId)
                .HasColumnName("ReservationId")
                .HasConversion<int>()
               ;

            builder.Property(h => h.PaymentId)
               .HasColumnName("PaymentId")
               .HasConversion<int>();


            builder.Property(h => h.TotalAmount)
               .HasColumnName("TotalAmount")
               .HasConversion<decimal>()
               ;

            builder.Property(h => h.TaxAmount)
              .HasColumnName("TaxAmount")
              .HasConversion<decimal>()
              ;


            builder.Property(h => h.GrandTotal)
              .HasColumnName("GrandTotal")
              .HasConversion<decimal>();

            builder.Property(h => h.IssuedDate)
              .HasColumnName("IssuedDate ")
              .HasConversion<DateTime>()
              ;

            builder.Property(h => h.FilePath)
              .HasColumnName("FilePath ")
              .HasConversion<string>()
              .UseCollation("Khmer_100_CI_AI_SC_UTF8")
              .HasMaxLength(255);
           
        }
    }
}
