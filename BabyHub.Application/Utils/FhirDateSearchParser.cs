using BabyHub.Domain.Shared.Constants;
using BabyHub.Domain.Shared.Enums;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BabyHub.Application.Utils
{
    public static class FhirDateSearchParser
    {
        public static (DateTime? date, EDateOperator? op) Parse(string? raw)
        {
            if (string.IsNullOrEmpty(raw))
                return (null, null);

            var match = Regex.Match(raw, FhirConsts.DateSearchRegex);
            if (!match.Success)
                throw new ArgumentException($"Invalid birthDate format: '{raw}'");

            var prefix = string.IsNullOrEmpty(match.Groups[1].Value)
                ? FhirConsts.DefaultDatePrefix
                : match.Groups[1].Value;

            if (!DateTime.TryParse(match.Groups[2].Value, null,
                    DateTimeStyles.RoundtripKind, out var date))
                throw new ArgumentException($"Invalid birthDate value: '{match.Groups[2].Value}'");

            if (!FhirConsts.TryParseOperator(prefix, out var op))
                throw new ArgumentException($"Unsupported date operator: '{prefix}'");

            return (date, op);
        }
    }
}