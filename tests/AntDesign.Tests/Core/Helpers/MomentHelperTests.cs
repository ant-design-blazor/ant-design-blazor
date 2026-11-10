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
        public void FromNow(TimeSpan age, string expectedSuffix)
        {
            // Calculate the date when the test runs, not while xUnit is
            // discovering test data, so queued tests cannot cross a time bucket.
            var date = DateTime.Now + age;
            var str = MomentHelper.FromNow(date);

            Assert.EndsWith(expectedSuffix, str, StringComparison.OrdinalIgnoreCase);
        }

        public static IEnumerable<object[]> FromNow_seeds => new List<object[]>
        {
            new object[] { TimeSpan.FromSeconds(-10), "a few seconds ago" },
            new object[] { TimeSpan.FromMinutes(-10), "minutes ago" },
            new object[] { TimeSpan.FromHours(-2), "hours ago" },
            new object[] { TimeSpan.FromDays(-2), "days ago"  },
            new object[] { TimeSpan.FromDays(-10), "weeks ago"  },
            new object[] { TimeSpan.FromDays(-62), "months ago"  },
            new object[] { TimeSpan.FromDays(-730), "years ago"  }
        };
    }
}
