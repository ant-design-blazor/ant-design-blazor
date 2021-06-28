// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class PaginationTotalContext
    {
        public int Total { get; set; }

        public (int from, int to) Range { get; set; }

        public PaginationTotalContext(int total, (int, int) range)
        {
            Total = total;
            Range = range;
        }
    }
}
