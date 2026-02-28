using AutoMapper;
using BabyHub.Application.Contracts.Patients;
using BabyHub.Application.Utils;
using BabyHub.Domain.Patients;
using BabyHub.Domain.Shared.Constants;
using BabyHub.Domain.Shared.Enums;
using BabyHub.Domain.Shared.Exceptions;
using BabyHub.Domain.Shared.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyHub.Application.Patients
{
    /// <summary>
    /// Provides application-level operations for managing patients.
    /// </summary>
    /// <remarks>
    /// SaveChangesAsync is called explicitly in the service layer.
    /// If cross-repository transactions are needed in the future,
    /// consider introducing a Unit of Work pattern.
    /// </remarks>
    public class PatientAppService : IPatientAppService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientAppService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(PatientCreateDto input)
        {
            var patient = new Patient(
                input.FamilyName,
                DateTime.SpecifyKind(input.BirthDate, DateTimeKind.Unspecified), 
                input.IsActive
                );

            patient.SetGender(input.Gender ?? PatientConsts.DefaultGender);
            patient.SetNameUsage(input.Use);
            patient.SetGivenNames(input.GivenNames);

            await _patientRepository.CreateAsync(patient);
            await _patientRepository.SaveChangesAsync();

            return patient.Id;
        }

        public async Task UpdateAsync(Guid id, PatientUpdateDto input)
        {
            var patient = await GetExistingAsync(id);

            patient.UpdateFamilyName(input.FamilyName);
            patient.UpdateBirthDate(input.BirthDate);
            patient.SetGivenNames(input.GivenNames);
            patient.SetNameUsage(input.Use);
            patient.SetGender(input.Gender);
            patient.SetActiveState(input.IsActive);

            await _patientRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var patient = await GetExistingAsync(id);

            await _patientRepository.DeleteAsync(patient);
            await _patientRepository.SaveChangesAsync();
        }

        public async Task<PatientDto> GetAsync(Guid id)
        {
            var patient = await GetExistingAsync(id);
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<IReadOnlyList<PatientDto>> GetListAsync(string? birthDate,
            int? count = null)
        {
            var (dateValue, operatorValue) = FhirDateSearchParser.Parse(birthDate);
            var patients = await _patientRepository.GetListAsync(dateValue, operatorValue, count);
            return _mapper.Map<IReadOnlyList<PatientDto>>(patients);
        }

        private async Task<Patient> GetExistingAsync(Guid id)
        {
            var patient = await _patientRepository.GetAsync(id);
            return patient ?? throw new NotFoundException(nameof(Patient), id);
        }
    }
}
