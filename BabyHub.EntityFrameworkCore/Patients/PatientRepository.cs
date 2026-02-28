using BabyHub.Domain.Patients;
using BabyHub.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyHub.EntityFrameworkCore.Patients
{
    public class PatientRepository : IPatientRepository
    {
        private readonly BabyHubDbContext _dbContext;

        public PatientRepository(BabyHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Patient?> GetAsync(Guid id)
        {
            return await _dbContext.Patients
                .Include(p => p.GivenNames)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Patient>> GetListAsync(
            DateTime? birthDate,
            EDateOperator? rawOperator,
            int? count = null)
        {
            var query = _dbContext.Patients
                .Include(p => p.GivenNames)
                .AsQueryable();

            if (!birthDate.HasValue || !rawOperator.HasValue)
                return await query.ToListAsync();

            var start = birthDate.Value;
            var end = birthDate.Value;

            switch (rawOperator.Value)
            {
                case EDateOperator.Equal:
                    if (start.TimeOfDay == TimeSpan.Zero)
                    {
                        start = start.Date;
                        end = start.AddDays(1);
                    }
                    else
                    {
                        end = start.AddTicks(1);
                    }
                    query = query.Where(p => p.BirthDate >= start && p.BirthDate < end);
                    break;

                case EDateOperator.NotEqual:
                    if (start.TimeOfDay == TimeSpan.Zero)
                    {
                        start = start.Date;
                        end = start.AddDays(1);
                    }
                    else
                    {
                        end = start.AddTicks(1);
                    }
                    query = query.Where(p => p.BirthDate < start || p.BirthDate >= end);
                    break;

                case EDateOperator.LessThan:
                    query = query.Where(p => p.BirthDate < start);
                    break;

                case EDateOperator.LessOrEqual:
                    if (start.TimeOfDay == TimeSpan.Zero)
                    {
                        end = start.AddDays(1);
                    }
                    else
                    {
                        end = start.AddTicks(1);
                    }
                    query = query.Where(p => p.BirthDate < end);
                    break;

                case EDateOperator.GreaterThan:
                    query = query.Where(p => p.BirthDate > start);
                    break;

                case EDateOperator.GreaterOrEqual:
                    query = query.Where(p => p.BirthDate >= start);
                    break;

                case EDateOperator.StartsAfter:
                    if (start.TimeOfDay == TimeSpan.Zero)
                    {
                        start = start.AddDays(1);
                    }
                    query = query.Where(p => p.BirthDate >= start);
                    break;

                case EDateOperator.EndsBefore:
                    query = query.Where(p => p.BirthDate < start.Date);
                    break;

                case EDateOperator.Approximate:
                    start = start.Date;
                    end = start.AddDays(1);
                    query = query.Where(p => p.BirthDate >= start && p.BirthDate < end);
                    break;
            }

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return await query.ToListAsync();
        }

        public async Task CreateAsync(Patient patient)
        {
            await _dbContext.Patients.AddAsync(patient);
        }

        public Task DeleteAsync(Patient patient)
        {
            _dbContext.Patients.Remove(patient);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
