// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Xunit;

namespace AntDesign.Tests.Core.Helpers
{
    public class MomentHelperTests
    {
        [Theory]
        [MemberData(nameof(FromNow_seeds), DisableDiscoveryEnumeration = true)]
        public void FromNow(DateTime date, string expectedSuffix)
        {
            var str = MomentHelper.FromNow(date);

            Assert.EndsWith(expectedSuffix, str, StringComparison.OrdinalIgnoreCase);
        }

        public static IEnumerable<object[]> FromNow_seeds => new List<object[]>
        {
            new object[] { DateTime.Now.AddSeconds(-30), "a few seconds ago" },
            new object[] { DateTime.Now.AddMinutes(-10), "minutes ago" },
            new object[] { DateTime.Now.AddHours(-2), "hours ago" },
            new object[] { DateTime.Now.AddDays(-2), "days ago"  },
            new object[] { DateTime.Now.AddDays(-10), "weeks ago"  },
            new object[] { DateTime.Now.AddMonths(-2), "months ago"  },
            new object[] { DateTime.Now.AddYears(-2), "years ago"  }
        };
    }
}
