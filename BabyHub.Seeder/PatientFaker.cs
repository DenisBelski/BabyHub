using BabyHub.Application.Contracts.Patients;
using BabyHub.Domain.Shared.Patients;
using BabyHub.Domain.Shared.Extensions;
using Bogus;
using BabyHub.Domain.Shared.Enums;

namespace BabyHub.Seeder
{
    public static class PatientFaker
    {
        public static List<PatientCreateDto> Generate(int count = 100)
        {
            var faker = new Faker<PatientCreateDto>()
                .RuleFor(x => x.FamilyName, f => f.Name.LastName()
                    .Truncate(PatientConsts.FamilyNameMaxLength))
                .RuleFor(x => x.BirthDate, f => f.Date.Between(
                    new DateTime(2020, 1, 1),
                    new DateTime(2024, 12, 31)))
                .RuleFor(x => x.IsActive, f => f.Random.Bool())
                .RuleFor(x => x.Gender, f => f.PickRandom<EGender>())
                .RuleFor(x => x.GivenNames, f => Enumerable
                    .Range(0, f.Random.Int(1, 2))
                    .Select(_ => f.Name.FirstName()
                        .Truncate(GivenNameConsts.NameMaxLength))
                    .ToList())
                .RuleFor(x => x.Use, f => f.PickRandom(
                    new[] { GivenNameConsts.Official, GivenNameConsts.Nickname, null })
                    ?.Truncate(PatientConsts.NameUsageMaxLength));

            return faker.Generate(count);
        }
    }
}
