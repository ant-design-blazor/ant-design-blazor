using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

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
