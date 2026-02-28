using BabyHub.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyHub.Application.Contracts.Patients
{
    public interface IPatientAppService
    {
        Task<Guid> CreateAsync(PatientCreateDto input);
        Task UpdateAsync(Guid id, PatientUpdateDto input);
        Task DeleteAsync(Guid id);
        Task<PatientDto> GetAsync(Guid id);
        Task<IReadOnlyList<PatientDto>> GetListAsync(string? birthDate,
            int? count = null);
    }
}
