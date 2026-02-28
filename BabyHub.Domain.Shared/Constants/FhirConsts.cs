using BabyHub.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyHub.Domain.Shared.Constants
{
    public static class FhirConsts
    {
        public const string Equal = "eq";
        public const string NotEqual = "ne";
        public const string GreaterThan = "gt";
        public const string LessThan = "lt";
        public const string GreaterOrEqual = "ge";
        public const string LessOrEqual = "le";
        public const string StartsAfter = "sa";
        public const string EndsBefore = "eb";
        public const string Approximate = "ap";

        public const string DefaultDatePrefix = Equal;

        //public const string DateSearchRegex =
        //    @"^(eq|ne|gt|lt|ge|le|sa|eb|ap)?(.+)$";

        public const string DateSearchRegex =
            @"^(eq|ne|gt|lt|ge|le|sa|eb|ap)?(\d{4}(-\d{2}(-\d{2}(T\d{2}:\d{2}(:\d{2}(\.\d+)?)?(Z|[+-]\d{2}:\d{2})?)?)?)?)?$";

        public static bool TryParseOperator(string raw, out EDateOperator op)
            => DatePrefixToOperatorMap.TryGetValue(raw, out op);

        public static readonly IReadOnlyDictionary<string, EDateOperator>
            DatePrefixToOperatorMap =
                new Dictionary<string, EDateOperator>(StringComparer.OrdinalIgnoreCase)
                {
                    [Equal] = EDateOperator.Equal,
                    [NotEqual] = EDateOperator.NotEqual,
                    [GreaterThan] = EDateOperator.GreaterThan,
                    [LessThan] = EDateOperator.LessThan,
                    [GreaterOrEqual] = EDateOperator.GreaterOrEqual,
                    [LessOrEqual] = EDateOperator.LessOrEqual,
                    [StartsAfter] = EDateOperator.StartsAfter,
                    [EndsBefore] = EDateOperator.EndsBefore,
                    [Approximate] = EDateOperator.Approximate

                };
    }
}
