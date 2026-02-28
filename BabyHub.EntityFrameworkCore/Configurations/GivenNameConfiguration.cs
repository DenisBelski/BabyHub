using BabyHub.Domain.Patients;
using BabyHub.Domain.Shared.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyHub.EntityFrameworkCore.Configurations
{
    public class GivenNameConfiguration : IEntityTypeConfiguration<GivenName>
    {
        public void Configure(EntityTypeBuilder<GivenName> builder)
        {
            builder.ToTable("GivenNames");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Value)
                   .HasMaxLength(GivenNameConsts.NameMaxLength);
        }
    }
}
