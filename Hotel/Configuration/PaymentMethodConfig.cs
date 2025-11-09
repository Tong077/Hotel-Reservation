using H_Domain.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PaymentMethodConfig : IBaseEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");

        builder.HasKey(t => t.PaymentMethodId);


        builder.Property(t => t.PaymentMethodId)
            .HasColumnName("PaymentMethodId")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .HasColumnName("Name")
             .UseCollation("Khmer_100_CI_AI_SC_UTF8")
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(t => t.Description)
           .HasColumnName("Description")
           .HasConversion<string>()
           .HasMaxLength(200);

        builder.Property(t => t.IsActive)
          .HasColumnName("IsActive")
          .HasConversion<bool>()
          .IsRequired();

    }
}

