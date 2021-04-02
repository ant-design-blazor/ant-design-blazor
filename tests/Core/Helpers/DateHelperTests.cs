using System;
using System.Collections.Generic;
using Xunit;

namespace AntDesign.Tests.Core.Helpers
{
    public class DateHelperTests
    {
        [Theory]
        [MemberData(nameof(GetNextStartDateOfDecade_Values))]
        public void GetNextStartDateOfDecade(DateTime currentDateTime, DateTime exceptedDateTime)
        {
            Assert.Equal(exceptedDateTime, DateHelper.GetNextStartDateOfDecade(currentDateTime));
        }

        public static List<object[]> GetNextStartDateOfDecade_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2030-01-01 00:00:00") },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2020-01-01 00:00:00") },
            new object[] { DateTime.Parse("2015-07-24 05:34:55"), DateTime.Parse("2020-01-01 00:00:00") },
            new object[] { DateTime.Parse("1000-07-24 05:34:55"), DateTime.Parse("1010-01-01 00:00:00") },
            new object[] { DateTime.Parse("67-07-24 05:34:55"), DateTime.Parse("70-01-01 00:00:00") },
            new object[] { DateTime.MinValue, DateTime.MinValue.AddYears(10) },
            new object[] { DateTime.MaxValue, DateTime.Parse($"{DateTime.MaxValue.Year}-01-01 00:00:00") },
        };

        [Theory]
        [MemberData(nameof(GetNextStartDateOfYear_Values))]
        public void GetNextStartDateOfYear(DateTime currentDateTime, DateTime exceptedDateTime)
        {
            Assert.Equal(exceptedDateTime, DateHelper.GetNextStartDateOfYear(currentDateTime));
        }

        public static List<object[]> GetNextStartDateOfYear_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2021-01-01 00:00:00") },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2019-01-01 00:00:00") },
            new object[] { DateTime.Parse("2015-07-24 05:34:55"), DateTime.Parse("2016-01-01 00:00:00") },
            new object[] { DateTime.Parse("1000-07-24 05:34:55"), DateTime.Parse("1001-01-01 00:00:00") },
            new object[] { DateTime.Parse("67-07-24 05:34:55"), DateTime.Parse("68-01-01 00:00:00") },
            new object[] { DateTime.MinValue, DateTime.MinValue.AddYears(1) },
            new object[] { DateTime.MaxValue, DateTime.Parse($"{DateTime.MaxValue.Year}-01-01 00:00:00") },
        };


        [Theory]
        [MemberData(nameof(GetNextStartDateOfQuarter_Values))]
        public void GetNextStartDateOfQuarter(DateTime currentDateTime, DateTime exceptedDateTime)
        {
            Assert.Equal(exceptedDateTime, DateHelper.GetNextStartDateOfQuarter(currentDateTime));
        }

        public static List<object[]> GetNextStartDateOfQuarter_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2020-04-01 00:00:00") },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2018-04-01 00:00:00") },
            new object[] { DateTime.Parse("2015-07-24 05:34:55"), DateTime.Parse("2015-10-01 00:00:00") },
            new object[] { DateTime.Parse("1000-11-24 05:34:55"), DateTime.Parse("1001-01-01 00:00:00") },
            new object[] { DateTime.Parse("67-07-24 05:34:55"), DateTime.Parse("67-10-01 00:00:00") },
            new object[] { DateTime.MinValue, DateTime.MinValue.AddMonths(3) },
            new object[] { DateTime.MaxValue, DateTime.Parse($"{DateTime.MaxValue.Year}-10-01 00:00:00") },
        };

        [Theory]
        [MemberData(nameof(GetNextStartDateOfMonth_Values))]
        public void GetNextStartDateOfMonth(DateTime currentDateTime, DateTime exceptedDateTime)
        {
            Assert.Equal(exceptedDateTime, DateHelper.GetNextStartDateOfMonth(currentDateTime));
        }

        public static List<object[]> GetNextStartDateOfMonth_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2020-02-01 00:00:00") },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2018-02-01 00:00:00") },
            new object[] { DateTime.Parse("2015-07-24 05:34:55"), DateTime.Parse("2015-08-01 00:00:00") },
            new object[] { DateTime.Parse("1000-12-24 05:34:55"), DateTime.Parse("1001-01-01 00:00:00") },
            new object[] { DateTime.Parse("67-07-24 05:34:55"), DateTime.Parse("67-08-01 00:00:00") },
            new object[] { DateTime.MinValue, DateTime.MinValue.AddMonths(1) },
            new object[] { DateTime.MaxValue, DateTime.Parse($"{DateTime.MaxValue.Year}-12-01 00:00:00") },
        };

        [Theory]
        [MemberData(nameof(GetNextStartDateOfDay_Values))]
        public void GetNextStartDateOfDay(DateTime currentDateTime, DateTime exceptedDateTime)
        {
            Assert.Equal(exceptedDateTime, DateHelper.GetNextStartDateOfDay(currentDateTime));
        }

        public static List<object[]> GetNextStartDateOfDay_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2020-01-05 00:00:00") },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2018-01-05 00:00:00") },
            new object[] { DateTime.Parse("2015-07-31 05:34:55"), DateTime.Parse("2015-08-01 00:00:00") },
            new object[] { DateTime.Parse("1000-07-24 05:34:55"), DateTime.Parse("1000-07-25 00:00:00") },
            new object[] { DateTime.Parse("67-12-24 05:34:55"), DateTime.Parse("67-12-25 00:00:00") },
            new object[] { DateTime.MinValue, DateTime.MinValue.AddDays(1) },
            new object[] { DateTime.MaxValue, DateTime.Parse($"{DateTime.MaxValue.Year}-12-31 00:00:00") },
        };
    }
}
