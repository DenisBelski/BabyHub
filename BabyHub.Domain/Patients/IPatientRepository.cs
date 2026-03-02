using BabyHub.Domain.Shared.Enums;

namespace BabyHub.Domain.Patients
{
    public interface IPatientRepository
    {
        Task<Patient?> GetAsync(Guid id);
        Task<IReadOnlyList<Patient>> GetListAsync(DateTime? birthDate, 
            EDateOperator? rawOperator, 
            int? count = null);        
        Task CreateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
        Task SaveChangesAsync();
    }
}
