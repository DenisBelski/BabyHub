using BabyHub.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
