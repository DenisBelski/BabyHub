using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyHub.Domain.Shared.Enums
{
    public enum EDateOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterOrEqual,
        LessOrEqual,
        StartsAfter,
        EndsBefore,
        Approximate
    }
}
