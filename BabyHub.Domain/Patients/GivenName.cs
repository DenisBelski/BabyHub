namespace BabyHub.Domain.Patients
{
    public class GivenName
    {
        public Guid Id { get; private set; }
        public string Value { get; private set; } = string.Empty;
        public Guid PatientId { get; private set; }

        private GivenName() { }

        public GivenName(Guid patientId, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Given name cannot be empty.", nameof(value));
            }

            Id = Guid.NewGuid();
            PatientId = patientId;
            Value = value;
        }
    }
}