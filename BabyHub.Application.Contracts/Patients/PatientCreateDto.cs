using BabyHub.Domain.Shared.Enums;
using BabyHub.Domain.Shared.Patients;

namespace BabyHub.Application.Contracts.Patients
{
    /// <summary>Patient creation data.</summary>
    public class PatientCreateDto
    {
        /// <summary>Patient's family name.</summary>
        /// <example>Ivanov</example>
        public string FamilyName { get; set; } = string.Empty;

        /// <summary>
        /// Patient's birth date and time.
        /// Format: yyyy-MM-ddTHH:mm:ss. Timezone is not supported.
        /// </summary>
        /// <example>2024-01-13T18:25:43</example>
        public DateTime BirthDate { get; set; }

        /// <summary>Indicates whether the patient record is active.</summary>
        /// <example>true</example>
        public bool IsActive { get; set; } = PatientConsts.InitialActiveState;

        /// <summary>List of given names.</summary>
        /// <example>["Ivan", "Ivanovich"]</example>
        public List<string> GivenNames { get; set; } = new();

        /// <summary>Name usage context.</summary>
        /// <example>official</example>
        public string? Use { get; set; }

        /// <summary>Patient's gender.</summary>
        /// <example>male</example>
        public EGender? Gender { get; set; }
    }
}