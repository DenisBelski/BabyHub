using BabyHub.Domain.Shared.Patients;
using FluentValidation;

namespace BabyHub.Application.Contracts.Patients
{
    public class PatientUpdateDtoValidator : AbstractValidator<PatientUpdateDto>
    {
        public PatientUpdateDtoValidator()
        {
            RuleFor(x => x.FamilyName)
                .NotEmpty()
                .MaximumLength(PatientConsts.FamilyNameMaxLength);

            RuleFor(x => x.BirthDate)
                .NotEmpty()
                .LessThanOrEqualTo(_ => DateTime.UtcNow)
                .WithMessage("BirthDate cannot be in the future.");

            RuleFor(x => x.Use)
                .MaximumLength(PatientConsts.NameUsageMaxLength);

            RuleForEach(x => x.GivenNames)
                .NotEmpty()
                .MaximumLength(PatientConsts.GivenNameMaxLength)
                .When(x => x.GivenNames.Any());

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage("Invalid gender value.");
        }
    }
}
