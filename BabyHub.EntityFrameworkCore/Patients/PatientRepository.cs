using BabyHub.Domain.Patients;
using BabyHub.Domain.Shared.Enums;
using Microsoft.EntityFrameworkCore;

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

            if (birthDate.HasValue && rawOperator.HasValue)
            {
                query = ApplyDateFilter(query, birthDate.Value, rawOperator.Value);
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

        private static IQueryable<Patient> ApplyDateFilter(
            IQueryable<Patient> query,
            DateTime birthDate,
            EDateOperator op)
        {
            var start = birthDate;
            var end = birthDate;

            switch (op)
            {
                case EDateOperator.Equal:
                    (start, end) = GetDayRange(birthDate);
                    return query.Where(p => p.BirthDate >= start && p.BirthDate < end);

                case EDateOperator.NotEqual:
                    (start, end) = GetDayRange(birthDate);
                    return query.Where(p => p.BirthDate < start || p.BirthDate >= end);

                case EDateOperator.LessThan:
                    return query.Where(p => p.BirthDate < start);

                case EDateOperator.LessOrEqual:
                    end = birthDate.TimeOfDay == TimeSpan.Zero
                        ? birthDate.AddDays(1)
                        : birthDate.AddTicks(1);
                    return query.Where(p => p.BirthDate < end);

                case EDateOperator.GreaterThan:
                    return query.Where(p => p.BirthDate > start);

                case EDateOperator.GreaterOrEqual:
                    return query.Where(p => p.BirthDate >= start);

                case EDateOperator.StartsAfter:
                    start = birthDate.TimeOfDay == TimeSpan.Zero
                        ? birthDate.AddDays(1)
                        : birthDate;
                    return query.Where(p => p.BirthDate >= start);

                case EDateOperator.EndsBefore:
                    return query.Where(p => p.BirthDate < birthDate.Date);

                case EDateOperator.Approximate:
                    (start, end) = (birthDate.Date, birthDate.Date.AddDays(1));
                    return query.Where(p => p.BirthDate >= start && p.BirthDate < end);

                default:
                    return query;
            }
        }

        private static (DateTime start, DateTime end) GetDayRange(DateTime date)
        {
            var start = date.TimeOfDay == TimeSpan.Zero ? date.Date : date;
            var end = date.TimeOfDay == TimeSpan.Zero ? date.Date.AddDays(1) : date.AddTicks(1);
            return (start, end);
        }
    }
}
