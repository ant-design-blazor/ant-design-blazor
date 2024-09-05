// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AntDesign.Docs.Build.CLI.Utils
{
    public class MenuComparer : IComparer<string>
    {
        public int Compare([AllowNull] string x, [AllowNull] string y)
        {
            if (x == null)
                return 1;

            if (y == null)
                return -1;

            var first = x[0] - y[0];
            if (first != 0)
                return first;

            var length = Math.Max(x.Length, y.Length);
            for (var i = 1; i < length - 1; i++)
            {
                if (x.Length <= i)
                    return 1;

                if (y.Length <= i)
                    return -1;

                var diff = y[i] - x[i];
                if (diff != 0)
                    return diff;
            }

            return 0;
        }
    }
}
