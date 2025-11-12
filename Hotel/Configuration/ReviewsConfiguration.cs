using H_Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace H_Domain.Configuration
{
    public class ReviewsConfiguration : IBaseEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(h => h.ReviewId);

            builder.Property(h => h.ReviewId)
                .HasColumnName("ReviewId")
                .ValueGeneratedOnAdd();

            builder.Property(h => h.GuestId)
                .HasColumnName("GuestId")
                .HasConversion<int>()
               ;

            builder.Property(h => h.ReservationId)
               .HasColumnName("ReservationId")
               .HasConversion<int>();

            builder.Property(h => h.Rating)
               .HasColumnName("Rating")
               .HasConversion<int>();

            builder.Property(h => h.Comment)
              .HasColumnName("Comment")
              .HasConversion<string>()
              .UseCollation("Khmer_100_CI_AI_SC_UTF8")
              .HasMaxLength(500);
            builder.Property(h => h.CreatedDate)
              .HasColumnName("CreatedDate")
              .HasConversion<DateTime>()
              .HasDefaultValueSql("getdate()");
        }
    }
}
