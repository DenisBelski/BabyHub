namespace BabyHub.Application.Contracts.Patients
{
    /// <summary>Represents a patient's name.</summary>
    public class PatientNameDto
    {
        /// <summary>Unique identifier of the patient.</summary>
        /// <example>d8ff176f-bd0a-4b8e-b329-871952e32e1f</example>
        public Guid Id { get; init; }

        /// <summary>Name usage context.</summary>
        /// <example>official</example>
        public string? Use { get; init; }

        /// <summary>Family name.</summary>
        /// <example>Ivanov</example>
        public string Family { get; init; } = string.Empty;

        /// <summary>List of given names.</summary>
        /// <example>["Ivan", "Ivanovich"]</example>
        public List<string> Given { get; init; } = new();
    }
}
