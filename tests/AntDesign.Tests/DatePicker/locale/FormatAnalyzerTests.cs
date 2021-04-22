using System;
using System.Collections.Generic;
using AntDesign.Datepicker.Locale;
using Xunit;

namespace AntDesign.Tests.DatePicker.Locale
{
    public class FormatAnalyzerTests
    {
        [Theory]
        [MemberData(nameof(FormatAnalyzer_values_seeds))]
        public void DateFormatDetails_ShouldReturnCorrectBool_WhenIsDateFullStringCalled(
            string dateFormat, string possibleDate, bool expectedResult)
        {
            var locale = new DateLocale() { DateFormat = dateFormat };
            var details = new FormatAnalyzer(dateFormat);
            Console.WriteLine($"Possible date {possibleDate}");
            Assert.Equal(expectedResult, details.IsFullString(possibleDate));
        }

        public static IEnumerable<object[]> FormatAnalyzer_values_seeds => new List<object[]>
        {
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
    }
}
