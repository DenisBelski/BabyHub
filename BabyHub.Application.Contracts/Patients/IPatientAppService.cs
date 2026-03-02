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
