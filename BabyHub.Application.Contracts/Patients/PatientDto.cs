using BabyHub.Domain.Shared.Enums;

namespace BabyHub.Application.Contracts.Patients
{
    /// <summary>Represents a patient record.</summary>
    public class PatientDto
    {
        /// <summary>Patient's name details.</summary>
        public PatientNameDto Name { get; init; } = default!;

        /// <summary>Patient's gender.</summary>
        /// <example>male</example>
        public EGender? Gender { get; init; }

        /// <summary>Patient's birth date and time.</summary>
        /// <example>2024-01-13T18:25:43</example>
        public DateTime BirthDate { get; init; }

        /// <summary>Indicates whether the patient record is active.</summary>
        /// <example>true</example>
        public bool Active { get; init; }
    }
}
