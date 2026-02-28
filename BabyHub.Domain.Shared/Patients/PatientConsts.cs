using BabyHub.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyHub.Domain.Shared.Patients
{
    public static class PatientConsts
    {
        public const bool InitialActiveState = true;
        public const EGender DefaultGender = EGender.Unknown;
        public const int FamilyNameMaxLength = 100;
        public const int NameUsageMaxLength = 50;
        public const int GivenNameMaxLength = 50;

    }
}
