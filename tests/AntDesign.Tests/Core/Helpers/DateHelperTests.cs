// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
            new object[] { DateTime.Parse("1800-07-24 05:34:55"), DateTime.Parse("1810-01-01 00:00:00") },
            new object[] { DateTime.Parse("1867-07-24 05:34:55"), DateTime.Parse("1870-01-01 00:00:00") },
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
            new object[] { DateTime.Parse("1800-07-24 05:34:55"), DateTime.Parse("1801-01-01 00:00:00") },
            new object[] { DateTime.Parse("1867-07-24 05:34:55"), DateTime.Parse("1868-01-01 00:00:00") },
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
            new object[] { DateTime.Parse("1800-11-24 05:34:55"), DateTime.Parse("1801-01-01 00:00:00") },
            new object[] { DateTime.Parse("1867-07-24 05:34:55"), DateTime.Parse("1867-10-01 00:00:00") },
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
            new object[] { DateTime.Parse("1800-12-24 05:34:55"), DateTime.Parse("1801-01-01 00:00:00") },
            new object[] { DateTime.Parse("1867-07-24 05:34:55"), DateTime.Parse("1867-08-01 00:00:00") },
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
            new object[] { DateTime.Parse("1800-07-24 05:34:55"), DateTime.Parse("1800-07-25 00:00:00") },
            new object[] { DateTime.Parse("1867-12-24 05:34:55"), DateTime.Parse("1867-12-25 00:00:00") },
            new object[] { DateTime.MinValue, DateTime.MinValue.AddDays(1) },
            new object[] { DateTime.MaxValue, DateTime.Parse($"{DateTime.MaxValue.Year}-12-31 00:00:00") },
        };

        [Theory]
        [MemberData(nameof(IsSameDate_Values))]
        public void IsSameDate(DateTime? date, DateTime? compareDate, bool result)
        {
            Assert.Equal(result, DateHelper.IsSameDate(date, compareDate));
        }

        public static List<object[]> IsSameDate_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2020-01-05 00:00:00"), false },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2018-01-05 00:00:00"), false },
            new object[] { DateTime.Parse("2015-07-31 05:34:55"), DateTime.Parse("2015-07-31 05:34:55"), true },
            new object[] { null!, null!, false },
            new object[] { DateTime.Parse("1867-12-24 05:34:55"), null!, false },
            new object[] { null!, DateTime.Parse("1867-12-24 05:34:55"), false }
        };

        [Theory]
        [MemberData(nameof(IsSameYear_Values))]
        public void IsSameYear(DateTime? date, DateTime? compareDate, bool result)
        {
            Assert.Equal(result, DateHelper.IsSameYear(date, compareDate));
        }

        public static List<object[]> IsSameYear_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2020-01-05 00:00:00"), true },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2018-01-05 00:00:00"), true },
            new object[] { DateTime.Parse("2015-07-31 05:34:55"), DateTime.Parse("2014-07-31 05:34:55"), false },
            new object[] { null!, null!, false },
            new object[] { DateTime.Parse("1867-12-24 05:34:55"), null!, false },
            new object[] { null!, DateTime.Parse("1867-12-24 05:34:55"), false }
        };

        [Theory]
        [MemberData(nameof(IsSameMonth_Values))]
        public void IsSameMonth(DateTime? date, DateTime? compareDate, bool result)
        {
            Assert.Equal(result, DateHelper.IsSameMonth(date, compareDate));
        }

        public static List<object[]> IsSameMonth_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2020-01-05 00:00:00"), true },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2018-01-05 00:00:00"), true },
            new object[] { DateTime.Parse("2015-07-31 05:34:55"), DateTime.Parse("2014-07-31 05:34:55"), false },
            new object[] { DateTime.Parse("2015-08-31 05:34:55"), DateTime.Parse("2015-07-31 05:34:55"), false },
            new object[] { null!, null!, false },
            new object[] { DateTime.Parse("1867-12-24 05:34:55"), null!, false },
            new object[] { null!, DateTime.Parse("1867-12-24 05:34:55"), false }
        };

        [Theory]
        [MemberData(nameof(IsSameDay_Values))]
        public void IsSameDay(DateTime? date, DateTime? compareDate, bool result)
        {
            Assert.Equal(result, DateHelper.IsSameDay(date, compareDate));
        }

        public static List<object[]> IsSameDay_Values => new()
        {
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2020-01-04 00:00:00"), true },
            new object[] { DateTime.Parse("2020-01-04 05:34:55"), DateTime.Parse("2020-01-05 00:00:00"), false },
            new object[] { DateTime.Parse("2018-01-04 05:34:55"), DateTime.Parse("2018-01-05 00:00:00"), false },
            new object[] { DateTime.Parse("2015-07-31 05:34:55"), DateTime.Parse("2014-07-31 05:34:55"), false },
            new object[] { DateTime.Parse("2015-08-31 05:34:55"), DateTime.Parse("2015-07-31 05:34:55"), false },
            new object[] { DateTime.Parse("2015-07-30 05:34:55"), DateTime.Parse("2015-07-31 05:34:55"), false },
            new object[] { null!, null!, false },
            new object[] { DateTime.Parse("1867-12-24 05:34:55"), null!, false },
            new object[] { null!, DateTime.Parse("1867-12-24 05:34:55"), false }
        };

        [Theory]
        [MemberData(nameof(IsSameWeek_Values))]
        public void IsSameWeek(DateTime? date, DateTime? compareDate, DayOfWeek firstDayOfWeek, bool result)
        {
            Assert.Equal(result, DateHelper.IsSameWeek(date, compareDate, firstDayOfWeek));
        }

        public static List<object[]> IsSameWeek_Values => new()
        {
            new object[] { DateTime.Parse("2021-09-05 05:34:55"), DateTime.Parse("2021-09-05 00:00:00"), DayOfWeek.Monday, true },
            new object[] { DateTime.Parse("2021-09-01 05:34:55"), DateTime.Parse("2021-09-05 00:00:00"), DayOfWeek.Monday, true },
            new object[] { DateTime.Parse("2021-09-01 05:34:55"), DateTime.Parse("2021-08-30 00:00:00"), DayOfWeek.Monday, true },
            new object[] { DateTime.Parse("2021-09-30 05:34:55"), DateTime.Parse("2021-10-03 00:00:00"), DayOfWeek.Monday, true },
            new object[] { DateTime.Parse("2021-09-01 05:34:55"), DateTime.Parse("2021-09-06 00:00:00"), DayOfWeek.Monday, false },
            new object[] { DateTime.Parse("2021-09-01 05:34:55"), DateTime.Parse("2021-09-05 00:00:00"), DayOfWeek.Sunday, false },
            new object[] { DateTime.Parse("2021-09-01 05:34:55"), DateTime.Parse("2021-08-30 00:00:00"), DayOfWeek.Sunday, true },
            new object[] { DateTime.Parse("2021-09-30 05:34:55"), DateTime.Parse("2021-10-03 00:00:00"), DayOfWeek.Sunday, false },
            new object[] { null!, null!, DayOfWeek.Monday, false },
            new object[] { DateTime.Parse("1867-12-24 05:34:55"), null!, DayOfWeek.Monday, false },
            new object[] { null!, DateTime.Parse("1867-12-24 05:34:55"), DayOfWeek.Monday, false }
        };

        [Theory]
        [MemberData(nameof(IsSameQuarter_Values))]
        public void IsSameQuarter(DateTime? date, DateTime? compareDate, bool result)
        {
            Assert.Equal(result, DateHelper.IsSameQuarter(date, compareDate));
        }

        public static List<object[]> IsSameQuarter_Values => new()
        {
            new object[] { DateTime.Parse("2021-09-05 05:34:55"), DateTime.Parse("2021-09-05 00:00:00"), true },
            new object[] { DateTime.Parse("2021-09-01 05:34:55"), DateTime.Parse("2021-08-05 00:00:00"), true },
            new object[] { DateTime.Parse("2021-09-01 05:34:55"), DateTime.Parse("2021-07-01 00:00:00"), true },
            new object[] { DateTime.Parse("2021-09-30 05:34:55"), DateTime.Parse("2021-09-30 00:00:00"), true },
            new object[] { DateTime.Parse("2021-09-01 05:34:55"), DateTime.Parse("2021-06-30 00:00:00"), false },
            new object[] { DateTime.Parse("2021-09-30 05:34:55"), DateTime.Parse("2021-10-03 00:00:00"), false },
            new object[] { null!, null!, false },
            new object[] { DateTime.Parse("1867-12-24 05:34:55"), null!, false },
            new object[] { null!, DateTime.Parse("1867-12-24 05:34:55"), false }
        };


        [Theory]
        [InlineData("2021-01-01", "Q1")]
        [InlineData("2021-03-31", "Q1")]
        [InlineData("2021-04-01", "Q2")]
        [InlineData("2021-06-30", "Q2")]
        [InlineData("2021-07-01", "Q3")]
        [InlineData("2021-09-30", "Q3")]
        [InlineData("2021-10-01", "Q4")]
        [InlineData("2021-12-31", "Q4")]
        public void GetDayOfQuarter(string date, string expected)
        {
            Assert.Equal(expected, DateHelper.GetDayOfQuarter(DateTime.Parse(date)));
        }

        [Theory]
        [InlineData("2021-01-01", 1)]
        [InlineData("2021-03-31", 14)]
        [InlineData("2021-04-01", 14)]
        [InlineData("2021-06-30", 27)]
        [InlineData("2021-07-01", 27)]
        [InlineData("2021-09-30", 40)]
        [InlineData("2021-10-01", 40)]
        [InlineData("2021-12-31", 53)]
        [InlineData("2022-01-01", 1)]
        public void GetWeekOfYear(string date, int expected)
        {
            Assert.Equal(expected, DateHelper.GetWeekOfYear(DateTime.Parse(date), DayOfWeek.Monday));
        }

        [Theory]
        [MemberData(nameof(AddYearsSafely_Values))]
        public void AddYearsSafely(DateTime date, int yearChange, DateTime expected)
        {
            Assert.Equal(expected, DateHelper.AddYearsSafely(date, yearChange));
        }

        public static List<object[]> AddYearsSafely_Values => new()
        {
            new object[] { DateTime.Parse("2022-01-01"), 1, DateTime.Parse("2023-01-01") },
            new object[] { DateTime.MinValue.AddYears(2), -4, DateTime.MinValue },
            new object[] { DateTime.MaxValue.AddYears(-2), 4, DateTime.MaxValue },
        };

        [Theory]
        [MemberData(nameof(AddMonthsSafely_Values))]
        public void AddMonthsSafely(DateTime date, int monthChange, DateTime expected)
        {
            Assert.Equal(expected, DateHelper.AddMonthsSafely(date, monthChange));
        }

        public static List<object[]> AddMonthsSafely_Values => new()
        {
            new object[] { DateTime.Parse("2022-01-01"), 1, DateTime.Parse("2022-02-01") },
            new object[] { DateTime.Parse("2022-01-01"), -1, DateTime.Parse("2021-12-01") },
            new object[] { DateTime.Parse("2022-12-31"), 3, DateTime.Parse("2023-03-31") },
            new object[] { DateTime.MinValue.AddMonths(2), -4, DateTime.MinValue },
            new object[] { DateTime.MaxValue.AddMonths(-2), 4, DateTime.MaxValue },
        };

        [Theory]
        [MemberData(nameof(AddDaysSafely_Values))]
        public void AddDaysSafely(DateTime date, int dayChange, DateTime expected)
        {
            Assert.Equal(expected, DateHelper.AddDaysSafely(date, dayChange));
        }

        public static List<object[]> AddDaysSafely_Values => new()
        {
            new object[] { DateTime.Parse("2022-01-01"), 1, DateTime.Parse("2022-01-02") },
            new object[] { DateTime.Parse("2022-01-01"), -1, DateTime.Parse("2021-12-31") },
            new object[] { DateTime.Parse("2021-12-31"), 4, DateTime.Parse("2022-01-04") },
            new object[] { DateTime.MinValue.AddDays(2), -4, DateTime.MinValue },
            new object[] { DateTime.MaxValue.AddDays(-2), 4, DateTime.MaxValue },
        };
    }
}

