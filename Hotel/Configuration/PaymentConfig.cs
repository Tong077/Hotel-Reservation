using H_Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PaymentConfig : IBaseEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");


        builder.HasKey(t => t.PaymentId);

        builder.Property(t => t.PaymentId)
            .HasColumnName("PaymentId")
            .ValueGeneratedOnAdd();


        builder.Property(t => t.ReservationId)
      .HasColumnName("ReservationId")
      .HasConversion<int>();


        //builder.Property(t => t.CurrencyId)
        //    .HasColumnName("CurrencyId")
        //     .HasConversion<int>();

        builder.Property(t => t.PaymentMethodId)
            .HasColumnName("PaymentMethodId");

        builder.Property(t => t.Amount)
            .HasColumnName("Amount")
            .HasConversion<decimal>();

        builder.Property(t => t.Currency)
            .HasColumnName("Currency")
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(t => t.TransactionId)
            .HasColumnName("TransactionId");

        builder.Property(t => t.PaymentStatus)
            .HasColumnName("PaymentStatus")
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.PaymentDate)
            .HasConversion<DateTime>();

        builder.Property(t => t.RefundAmount)
            .HasColumnName("RefundAmount")
            .HasConversion<decimal>();

        builder.Property(t => t.RefundDate)
            .HasColumnName("RefundDate")
            .HasConversion<DateTime>();


        builder
      .HasMany(p => p.Reservation)
      .WithOne(r => r.Payment)
      .HasForeignKey(r => r.PaymentId)
      .OnDelete(DeleteBehavior.Restrict);



    }
}

