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
    public class RolesConfiguration : IBaseEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(100);


            builder.Property(r => r.NormalizedName)
                   .IsRequired()
                   .HasMaxLength(100);


          


            builder.Property(r => r.IsActive)
                   .HasDefaultValue(true);


            builder.HasIndex(r => r.NormalizedName)
                   .IsUnique();

        }
    }
}
