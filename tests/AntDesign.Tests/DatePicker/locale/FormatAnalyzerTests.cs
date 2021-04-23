using System;
using System.Collections.Generic;
using System.Globalization;
using AntDesign.Datepicker.Locale;
using Xunit;

namespace AntDesign.Tests.DatePicker.Locale
{
    public class FormatAnalyzerTests
    {
        [Theory]
        [MemberData(nameof(FormatAnalyzer_values_seeds))]
        public void IsFullString_ShouldReturnCorrectBool_WhenIsDateFullStringCalled(
            string dateFormat, string possibleDate, bool expectedResult)
        {
            //Arrange
            var details = new FormatAnalyzer(dateFormat, DatePickerType.Date, false, new());
            //Act
            var actual = details.IsFullString(possibleDate);
            //Assert
            Assert.Equal(expectedResult, actual);
        }

        public static IEnumerable<object[]> FormatAnalyzer_values_seeds => new List<object[]>
        {
            new object[] { "yyyy", "2020", true },
            new object[] { "yyyy", "20200", false },
            new object[] { "yyyy", "202", false },
            new object[] { "yyyy", "20 0", false },

            new object[] { "yyyy-MM-dd", "2020-01-01", true },
            new object[] { "yyyy-MM-dd", "202-01-01", false },
            new object[] { "yyyy-MM-dd", "20-01-01", false },
            new object[] { "yyyy-MM-dd", "2-01-01", false },
            new object[] { "yyyy-MM-dd", "-01-01", false },
            new object[] { "yyyy-MM-dd", "202 -01-01", false },
            new object[] { "yyyy-MM-dd", "202A-01-01", false },
            new object[] { "yyyy-MM-dd", "20  -01-01", false },
            new object[] { "yyyy-MM-dd", "20A -01-01", false },
            new object[] { "yyyy-MM-dd", "20 B-01-01", false },
            new object[] { "yyyy-MM-dd", "20 -01-01", false },
            new object[] { "yyyy-MM-dd", "20A-01-01", false },
            new object[] { "yyyy-MM-dd", "2  -01-01", false },
            new object[] { "yyyy-MM-dd", "2 A-01-01", false },
            new object[] { "yyyy-MM-dd", "2-01-01", false },
            new object[] { "yyyy-MM-dd", "2020-1-01", false },
            new object[] { "yyyy-MM-dd", "2020--01", false },
            new object[] { "yyyy-MM-dd", "2020-01", false },
            new object[] { "yyyy-MM-dd", "2020-01-0", false },
            new object[] { "yyyy-MM-dd", "2020-01-1", false },
            new object[] { "yyyy-MM-dd", "2020-01-1 ", false },
            new object[] { "yyyy-MM-dd", "2020-01- 1", false },

            new object[] { "yyyy年M月d日", "2020年01月01日", true },
            new object[] { "yyyy年M月d日", "2020年01月1日", true },
            new object[] { "yyyy年M月d日", "2020年1月01日", true },
            new object[] { "yyyy年M月d日", "2020年1月1日", true },
            new object[] { "yyyy年M月d日", "202年01月01日", false },
            new object[] { "yyyy年M月d日", "20年01月01日", false },
            new object[] { "yyyy年M月d日", "2年01月01日", false },
            new object[] { "yyyy年M月d日", "年01月01日", false },
            new object[] { "yyyy年M月d日", "202 年01月01日", false },
            new object[] { "yyyy年M月d日", "20  年01月01日", false },
            new object[] { "yyyy年M月d日", "20 年01月01日", false },
            new object[] { "yyyy年M月d日", "2  年01月01日", false },
            new object[] { "yyyy年M月d日", "2年01月01日", false },
            new object[] { "yyyy年M月d日", "2020年月01日", false },
            new object[] { "yyyy年M月d日", "2020年01", false },
            new object[] { "yyyy年M月d日", "2020年01月0日", false },
            new object[] { "yyyy年M月d日", "2020年01月1 日", false },
            new object[] { "yyyy年M月d日", "2020年01月 1日", false },

            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时17分10秒", true },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16 21时17分10秒", false },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21:17:10", false },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时1分10秒", false },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时17分1秒", false },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时 7分10秒", false },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时17分 1秒", false },


            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10:01:02", true },

            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 1:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10:1:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10:01:2", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10: 1:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10:01: 2", false },

            new object[] { "yyyy-MM-dd HH:mm:ss", "202-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "202 -01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "202A-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20  -01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20A -01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20 B-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20 -01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20A-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2  -01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2 A-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2-01-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-1-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020--01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-0 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-1 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-1 10:01:02", false },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01- 1 10:01:02", false },

            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 10:01:02", true },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1:01:2", true },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1:01:02", true },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/1 10:01:2", true },
            new object[] { "M/yyyy/d H:mm:s", "1/2020/01 10:01:2", true },
            new object[] { "M/yyyy/d H:mm:s", "1/2020/1 1:01:2", true },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 0:01:02", true },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1:10:0", true },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 0:00:0", true },

            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 10:1:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1 :1:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 0 :1:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1:10: 2", false },


            new object[] { "M/yyyy/d H:mm:s", "01/202/01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/20/01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2/01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01//01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/202 /01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/20  /01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/20 /01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2  /01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2/01 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2020 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/0 10:01:02", false },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/ 1 10:01:02", false },

        };

        [Theory]
        [MemberData(nameof(TryParseYear_SoulReturnCorrectBool_seed))]
        public void TryParseYear_SoulReturnCorrectBool(
            string dateFormat, string possibleDate, bool expectedResult, DateTime expectedParsedDate)
        {
            //Arrange
            var details = new FormatAnalyzer(dateFormat, DatePickerType.Year, false, new());
            //Act
            var actualResult = details.TryPickerStringConvert<DateTime>(possibleDate, out DateTime actualParsedDate, CultureInfo.CurrentCulture);
            //Assert            
            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedParsedDate, actualParsedDate);
        }

        public static IEnumerable<object[]> TryParseYear_SoulReturnCorrectBool_seed => new List<object[]>
        {
            new object[] { "yyyy", "2020", true, new DateTime(2020, 1, 1) },
            new object[] { "yyyy", "202", false, null },
            new object[] { "yyyy", "2 20", false, null },
            new object[] { "yyyy", "20020", false, null },
        };

        [Theory]
        [MemberData(nameof(TryParseWeek_SoulReturnCorrectBool_seed))]
        public void TryParseWeek_SoulReturnCorrectBool(
            string possibleDate, bool expectedResult, DateTime expectedParsedDate)
        {
            //Arrange            
            var details = new FormatAnalyzer("0000-10Week", DatePickerType.Week, false, new());
            //Act
            var actualResult = details.TryPickerStringConvert<DateTime>(possibleDate, out DateTime actualParsedDate, CultureInfo.CurrentCulture);
            //Assert            
            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedParsedDate, actualParsedDate);
        }

        public static IEnumerable<object[]> TryParseWeek_SoulReturnCorrectBool_seed => new List<object[]>
        {
            new object[] { "2020-1Week", true, new DateTime(2020, 1, 1) },
            new object[] { "0120-1Week", true, new DateTime(120, 1, 1) },
            new object[] { "2020-2Week", true, new DateTime(2020, 1, 6) },
            new object[] { "2020- 1Week", true, new DateTime(2020, 1, 1) },
            new object[] { "2020-12Week", true, new DateTime(2020,3, 16) },
            new object[] { "2020-52Week", true, new DateTime(2020, 12, 21) },
            new object[] { "2020-53Week", true, new DateTime(2020, 12, 28) },
            new object[] { "2040-54Week", true, new DateTime(2040, 12, 31) }, //extremely rare, only when leap year + 1st of Jan is on Sunday
            new object[] { "2020-54Week", false, null },
            new object[] { "202-12Week", false, null },
            new object[] { "2 20-12Week", false, null },
            new object[] { " 020-12Week", false, null },
            new object[] { "20201Week", false, null },
            new object[] { "2020/1Week", false, null },
            new object[] { "2020-1Weeks", false, null },
            new object[] { "2020-1week", false, null },
            new object[] { "2020-1week", false, null },
            new object[] { "2020--1Week", false, null },
            new object[] { "2020-0Week", false, null },
            new object[] { "2020-AWeek", false, null },

        };

        [Theory]
        [MemberData(nameof(TryParseQuarter_SoulReturnCorrectBool_seed))]
        public void TryParseQuarter_SoulReturnCorrectBool(
            string possibleDate, bool expectedResult, DateTime expectedParsedDate)
        {
            //Arrange            
            var details = new FormatAnalyzer("0000-Q0", DatePickerType.Quarter, false, new());
            //Act
            var actualResult = details.TryPickerStringConvert<DateTime>(possibleDate, out DateTime actualParsedDate, CultureInfo.CurrentCulture);
            //Assert            
            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedParsedDate, actualParsedDate);
        }

        public static IEnumerable<object[]> TryParseQuarter_SoulReturnCorrectBool_seed => new List<object[]>
        {
            new object[] { "2020-Q2", true, new DateTime(2020, 4, 1) },
            new object[] { "2020-Q1", true, new DateTime(2020, 1, 1) },
            new object[] { "2020-Q3", true, new DateTime(2020, 7, 1) },
            new object[] { "2020-Q4", true, new DateTime(2020, 10, 1) },
            new object[] { "0020-Q4", true, new DateTime(20, 10, 1) },
            new object[] { "2020-q4", true, new DateTime(2020, 10, 1) },
            new object[] { "2020-Q5", false, null },
            new object[] { "2020-Q0", false, null },
            new object[] { "2020-1", false, null },
            new object[] { "202-Q1", false, null },
            new object[] { "20 2-Q1", false, null },
            new object[] { " 202-Q1", false, null },
            new object[] { "2020/Q1", false, null },
            new object[] { "2020 Q1", false, null },
        };
    }
}
