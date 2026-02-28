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
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.BirthDate);

            builder.Property(p => p.FamilyName)
                   .IsRequired()
                   .HasMaxLength(PatientConsts.FamilyNameMaxLength);

            builder.Property(p => p.NameUsage)
                   .HasMaxLength(PatientConsts.NameUsageMaxLength);

            builder.Property(p => p.BirthDate)
                   .IsRequired();

            builder.HasMany(p => p.GivenNames)
               .WithOne()
               .HasForeignKey(g => g.PatientId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
