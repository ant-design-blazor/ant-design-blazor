// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public enum TableFilterCompareOperator
    {
        Equals = 1,
        Contains = 2,
        StartsWith = 3,
        EndsWith = 4,
        GreaterThan = 5,
        LessThan = 6,
        GreaterThanOrEquals = 7,
        LessThanOrEquals = 8,
        Condition = 9,
        NotEquals = 10,
        IsNull = 11,
        IsNotNull = 12,
        NotContains = 13,
        TheSameDateWith = 14,
        Between = 15
    }
}
