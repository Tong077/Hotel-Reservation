using H_Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GuestConfig : IBaseEntityTypeConfiguration<Guest>
{
    public void Configure(EntityTypeBuilder<Guest> builder)
    {
        builder.ToTable("Guests");

        builder.HasKey(t => t.GuestId);

        builder.Property(t => t.GuestId)
            .HasColumnName("GuestId")
            .ValueGeneratedOnAdd();


        builder.Property(t => t.FirstName)
               .HasColumnName("FirstName")
               .HasConversion<string>()
               .UseCollation("Khmer_100_CI_AI_SC_UTF8")
               .HasMaxLength(50);

        builder.Property(t => t.LastName)
              .HasColumnName("LastName")
              .HasConversion<string>()
              .UseCollation("Khmer_100_CI_AI_SC_UTF8")
              .HasMaxLength(50);

        builder.Property(t => t.Email)
              .HasColumnName("Email")
              .HasConversion<string>()
              .HasMaxLength(100);

        builder.Property(t => t.Phone)
             .HasColumnName("Phone")
             .HasConversion<string>()
             .HasMaxLength(20);
        builder.Property(t => t.Address)
             .HasColumnName("Address")
             .HasConversion<string>()
             .UseCollation("Khmer_100_CI_AI_SC_UTF8")
             .HasMaxLength(200);


        builder.Property(t => t.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasConversion<DateTime>()
             .HasDefaultValueSql("getdate()");

    }
}

