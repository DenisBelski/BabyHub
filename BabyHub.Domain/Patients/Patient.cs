using BabyHub.Domain.Shared.Enums;
using BabyHub.Domain.Shared.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BabyHub.Domain.Patients
{
    public class Patient
    {
        public Guid Id { get; private set; }
        public string FamilyName { get; private set; }
        public ICollection<GivenName> GivenNames { get; private set; } = new List<GivenName>();
        public string? NameUsage { get; private set; }
        public EGender Gender { get; private set; }
        public DateTime BirthDate { get; private set; }
        public bool IsActive { get; private set; }

        public Patient(string familyName, DateTime birthDate, bool isActive = PatientConsts.InitialActiveState)
        {
            if (string.IsNullOrWhiteSpace(familyName))
            {
                throw new ArgumentException("Family name can not be null or empty", nameof(familyName));
            }

            Id = Guid.NewGuid();
            FamilyName = familyName;
            BirthDate = birthDate;
            IsActive = isActive;
        }

        public void SetGender(EGender gender) => Gender = gender;
        public void SetNameUsage(string? nameUsage) => NameUsage = nameUsage;
        public void UpdateFamilyName(string familyName)
        {
            if (string.IsNullOrWhiteSpace(familyName))
            {
                throw new ArgumentException("Family name cannot be empty.", nameof(familyName));
            }

            FamilyName = familyName;
        }
        public void UpdateBirthDate(DateTime birthDate)
        {
            if (birthDate == default)
            {
                throw new ArgumentException("BirthDate cannot be empty.", nameof(birthDate));
            }

            BirthDate = birthDate;
        }
        public void SetActiveState(bool isActive)
        {
            if (IsActive == isActive)
            {
                return;
            }

            IsActive = isActive;
        }

        public void SetGivenNames(List<string>? givenNames)
        {
            if (givenNames == null)
            {
                return;
            }

            var existingNames = GivenNames.Select(g => g.Value).ToHashSet();
            var namesToAdd = givenNames.Except(existingNames);
            var namesToRemove = GivenNames.Where(g => !givenNames.Contains(g.Value)).ToList();

            foreach (var name in namesToRemove)
            {
                GivenNames.Remove(name);
            }

            foreach (var name in namesToAdd)
            {
                GivenNames.Add(new GivenName(Id, name));
            }
        }
    }
}
