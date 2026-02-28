using BabyHub.Domain.Patients;
using BabyHub.EntityFrameworkCore.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BabyHub.EntityFrameworkCore
{
    public class BabyHubDbContext : DbContext
    {
        public BabyHubDbContext(DbContextOptions<BabyHubDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new GivenNameConfiguration());
        }
    }
}