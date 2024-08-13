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
            var details = new FormatAnalyzer(dateFormat, DatePickerType.Date, new(), CultureInfo.InvariantCulture);
            //Act
            var actual1 = details.IsFullString(possibleDate);
            var actual2 = details.IsFullString(possibleDate);
            //Assert
            Assert.Equal(expectedResult, actual1);
            Assert.Equal(expectedResult, actual2);
        }

        public static IEnumerable<object[]> FormatAnalyzer_values_seeds => new List<object[]>
        {
            new object[] { "yyyy", "2020", true },
            new object[] { "yyyy", "20200", false },
            new object[] { "yyyy", "202", false },
            new object[] { "yyyy", "20 0", false },

            new object[] { "AAAyyyy-MM-dd", "AAA2020-01-01", true },
            new object[] { "   AAAyyyy-MM-dd", "   AAA2020-01-01", true },
            new object[] { "   AAAyyyy-MM-dd", "  AAA2020-01-01", false},
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
        [MemberData(nameof(TryParseDate_ShouldReturnCorrectBool_seed))]
        public void TryParseDate_ShouldReturnCorrectBool(
            string dateFormat, string possibleDate, bool expectedResult, DateTime expectedParsedDate)
        {
            //Arrange
            var details = new FormatAnalyzer(dateFormat, DatePickerType.Date, new(), CultureInfo.InvariantCulture);
            //Act
            var actualResult1 = details.TryPickerStringConvert(possibleDate, out DateTime actualParsedDate1);
            var actualResult2 = details.TryPickerStringConvert(possibleDate, out DateTime actualParsedDate2);
            //Assert            
            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedParsedDate, actualParsedDate1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedParsedDate, actualParsedDate2);
        }

        public static IEnumerable<object[]> TryParseDate_ShouldReturnCorrectBool_seed => new List<object[]>
        {
            new object[] { "yyyy", "2020", true, new DateTime(2020, 1, 1)  },
            new object[] { "yyyy", "20200", false, null! },
            new object[] { "yyyy", "202", false, null! },
            new object[] { "yyyy", "20 0", false, null! },

            new object[] { "AAAyyyy-MM-dd", "AAA2020-01-01", true, new DateTime(2020,1,1)  },
            new object[] { "   AAAyyyy-MM-dd", "   AAA2020-01-01", true, new DateTime(2020,1,1)  },
            new object[] { "   AAAyyyy-MM-dd", "  AAA2020-01-01", false, null!},
            new object[] { "MM/dd/yyyy", "01/12/2020", true, new DateTime(2020,1,12)  },
            new object[] { "yyyy-MM-dd", "2020-01-01", true, new DateTime(2020,1,1)  },
            new object[] { "yyyy-MM-dd", "2020-13-01", false, null! },
            new object[] { "yyyy-MM-dd", "2020-00-01", false, null! },
            new object[] { "yyyy-MM-dd", "2020-01-00", false, null! },
            new object[] { "yyyy-MM-dd", "2020-01-32", false, null! },
            new object[] { "yyyy-MM-dd", "2021-02-29", false, null! },
            new object[] { "yyyy-MM-dd", "2020-02-30", false, null! },
            new object[] { "yyyy-MM-dd", "2020-02-29", true, new DateTime(2020,2,29) },
            new object[] { "yyyy-MM-dd", "2021-02-00", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2021-02-01 24:00:00", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2021-02-01 23:60:00", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2021-02-01 23:59:60", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2021-02-01 23:59:59", true, new DateTime(2021,2,1,23,59,59)},
            new object[] { "yyyy-MM-dd", "2020-12-32", false, null! },
            new object[] { "yyyy-MM-dd", "202-01-01", false, null! },
            new object[] { "yyyy-MM-dd", "20-01-01", false, null! },
            new object[] { "yyyy-MM-dd", "2-01-01", false, null! },
            new object[] { "yyyy-MM-dd", "-01-01", false, null! },
            new object[] { "yyyy-MM-dd", "202 -01-01", false, null! },
            new object[] { "yyyy-MM-dd", "202A-01-01", false, null! },
            new object[] { "yyyy-MM-dd", "20  -01-01", false, null! },
            new object[] { "yyyy-MM-dd", "20A -01-01", false, null! },
            new object[] { "yyyy-MM-dd", "20 B-01-01", false, null! },
            new object[] { "yyyy-MM-dd", "20 -01-01", false, null! },
            new object[] { "yyyy-MM-dd", "20A-01-01", false, null! },
            new object[] { "yyyy-MM-dd", "2  -01-01", false, null! },
            new object[] { "yyyy-MM-dd", "2 A-01-01", false, null! },
            new object[] { "yyyy-MM-dd", "2020-1-01", false, null! },
            new object[] { "yyyy-MM-dd", "2020--01", false, null! },
            new object[] { "yyyy-MM-dd", "2020-01", false, null! },
            new object[] { "yyyy-MM-dd", "2020-01-0", false, null! },
            new object[] { "yyyy-MM-dd", "2020-01-1", false, null! },
            new object[] { "yyyy-MM-dd", "2020-01-1 ", false, null! },
            new object[] { "yyyy-MM-dd", "2020-01- 1", false, null! },

            new object[] { "yyyy年M月d日", "2020年01月01日", true, new DateTime(2020,1,1)  },
            new object[] { "yyyy年M月d日", "2020年01月1日", true, new DateTime(2020,1,1)  },
            new object[] { "yyyy年M月d日", "2020年1月01日", true, new DateTime(2020,1,1)  },
            new object[] { "yyyy年M月d日", "2020年1月1日", true, new DateTime(2020,1,1)  },
            new object[] { "yyyy年M月d日", "202年01月01日", false, null! },
            new object[] { "yyyy年M月d日", "20年01月01日", false, null! },
            new object[] { "yyyy年M月d日", "年01月01日", false, null! },
            new object[] { "yyyy年M月d日", "202 年01月01日", false, null! },
            new object[] { "yyyy年M月d日", "20  年01月01日", false, null! },
            new object[] { "yyyy年M月d日", "20 年01月01日", false, null! },
            new object[] { "yyyy年M月d日", "2  年01月01日", false, null! },
            new object[] { "yyyy年M月d日", "2年01月01日", false, null! },
            new object[] { "yyyy年M月d日", "2020年月01日", false, null! },
            new object[] { "yyyy年M月d日", "2020年01", false, null! },
            new object[] { "yyyy年M月d日", "2020年01月0日", false, null! },
            new object[] { "yyyy年M月d日", "2020年01月1 日", false, null! },
            new object[] { "yyyy年M月d日", "2020年01月 1日", false, null! },

            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时17分10秒", true, new DateTime(2020,4,16,21,17,10)  },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16 21时17分10秒", false, null! },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21:17:10", false, null! },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时1分10秒", false, null! },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时17分1秒", false, null! },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时 7分10秒", false, null! },
            new object[] { "yyyy年M月d日 HH时mm分ss秒", "2020年4月16日 21时17分 1秒", false, null! },


            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10:01:02", true, new DateTime(2020,1,1,10,1,2)  },

            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 1:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10:1:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10:01:2", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10: 1:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-01 10:01: 2", false, null! },

            new object[] { "yyyy-MM-dd HH:mm:ss", "202-01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20-01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "-01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "202 -01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "202A-01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20  -01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20A -01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20 B-01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20 -01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "20A-01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2  -01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2 A-01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2-01-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-1-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020--01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-0 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01-1 10:01:02", false, null! },
            new object[] { "yyyy-MM-dd HH:mm:ss", "2020-01- 1 10:01:02", false, null! },

            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 10:01:02", true, new DateTime(2020,1,1,10,1,2)  },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1:01:2", true, new DateTime(2020, 1, 1,1,1,2)  },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1:01:02", true, new DateTime(2020, 1, 1,1,1,2)  },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/1 10:01:2", true, new DateTime(2020,1,1,10,1,2)  },
            new object[] { "M/yyyy/d H:mm:s", "1/2020/01 10:01:2", true, new DateTime(2020,1,1,10,1,2)  },
            new object[] { "M/yyyy/d H:mm:s", "1/2020/1 1:01:2", true, new DateTime(2020,1,1,1,1,2)  },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 0:01:02", true, new DateTime(2020,1,1,0,1,2)  },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1:10:0", true, new DateTime(2020,1,1,1,10,0)  },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 0:00:0", true, new DateTime(2020,1,1,0,0,0)  },

            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 10:1:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1 :1:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 0 :1:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/01 1:10: 2", false, null! },

            new object[] { "M/yyyy/d H:mm:s", "01/202/01 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/20/01 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01//01 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/202 /01 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/20  /01 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/20 /01 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2  /01 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2/01 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2020 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/0 10:01:02", false, null! },
            new object[] { "M/yyyy/d H:mm:s", "01/2020/ 1 10:01:02", false, null! },
        };

        [Theory]
        [MemberData(nameof(TryParseYear_ShouldReturnCorrectBool_seed))]
        public void TryParseYear_ShouldReturnCorrectBool(
            string dateFormat, string possibleDate, bool expectedResult, DateTime expectedParsedDate)
        {
            //Arrange
            var details = new FormatAnalyzer(dateFormat, DatePickerType.Year, new(), CultureInfo.InvariantCulture);
            //Act
            var actualResult1 = details.TryPickerStringConvert(possibleDate, out DateTime actualParsedDate1);
            var actualResult2 = details.TryPickerStringConvert(possibleDate, out DateTime actualParsedDate2);
            //Assert            
            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedParsedDate, actualParsedDate1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedParsedDate, actualParsedDate2);
        }

        public static IEnumerable<object[]> TryParseYear_ShouldReturnCorrectBool_seed => new List<object[]>
        {
            new object[] { "yyyy", "2020", true, new DateTime(2020, 1, 1) },
            new object[] { "yyyy", "0020", true, new DateTime(20, 1, 1) },
            new object[] { "XXyyyyZ", "XX2020Z", true, new DateTime(2020, 1, 1) },
            new object[] { "yyyy年", "2021年", true, new DateTime(2021, 1, 1) },
            new object[] { "yyyy", "202", false, null!},
            new object[] { "yyyy", "2 20", false, null!},
            new object[] { "yyyy", "20020", false, null!},
        };

        [Theory]
        [MemberData(nameof(TryParseWeek_ShouldReturnCorrectBool_seed))]
        public void TryParseWeek_ShouldReturnCorrectBool(string yearFormat, string dateFormat,
            string possibleDate, bool expectedResult, DateTime expectedParsedDate)
        {
            //Arrange            
            var details = new FormatAnalyzer(dateFormat, DatePickerType.Week,
                new() { Lang = new() { YearFormat = yearFormat } }, CultureInfo.InvariantCulture);
            //Act
            var actualResult1 = details.TryPickerStringConvert(possibleDate, out DateTime actualParsedDate1);
            var actualResult2 = details.TryPickerStringConvert(possibleDate, out DateTime actualParsedDate2);
            //Assert            
            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedParsedDate, actualParsedDate1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedParsedDate, actualParsedDate2);
        }

        public static IEnumerable<object[]> TryParseWeek_ShouldReturnCorrectBool_seed => new List<object[]>
        {
            new object[] { "yyyy", "yyyy-1Week","2020-1Week", true, new DateTime(2020, 1, 1) },
            new object[] { "yyyy年", "yyyy年-1Week", "2020年-1Week", true, new DateTime(2020, 1, 1) },
            new object[] { "AAyyyy年", "AAyyyy年-1Week", "AA2020年-1Week", true, new DateTime(2020, 1, 1) },
            new object[] { "  AAyyyy年", "  AAyyyy年-1Week", "  AA2020年-1Week", true, new DateTime(2020, 1, 1) },
            new object[] { " AAyyyy年", " AAyyyy年-1Week", "  AA2020年-1Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","0120-1Week", true, new DateTime(120, 1, 1) },
            new object[] { "yyyy", "yyyy-1Week","2020-2Week", true, new DateTime(2020, 1, 6) },
            new object[] { "yyyy", "yyyy-1Week","2020- 1Week", true, new DateTime(2020, 1, 1) },
            new object[] { "yyyy", "yyyy-1Week","2020-12Week", true, new DateTime(2020,3, 16) },
            new object[] { "yyyy", "yyyy-1Week","2020-52Week", true, new DateTime(2020, 12, 21) },
            new object[] { "yyyy", "yyyy-1Week","2020-53Week", true, new DateTime(2020, 12, 28) },
            new object[] { "yyyy", "yyyy-1Week","2040-54Week", true, new DateTime(2040, 12, 31) }, //extremely rare, only when leap year + 1st of Jan is on Sunday
            new object[] { "yyyy", "yyyy-1Week","2020-54Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","202-12Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","2 20-12Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week"," 020-12Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","20201Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","2020/1Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","2020-1Weeks", false, null!},
            new object[] { "yyyy", "yyyy-1Week","2020-1week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","2020--1Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","2020-0Week", false, null!},
            new object[] { "yyyy", "yyyy-1Week","2020-AWeek", false, null!},
        };

        [Theory]
        [MemberData(nameof(TryParseQuarter_ShouldReturnCorrectBool_seed))]
        public void TryParseQuarter_ShouldReturnCorrectBool(string yearFormat, string dateFormat,
            string possibleDate, bool expectedResult, DateTime expectedParsedDate)
        {
            //Arrange            
            var details = new FormatAnalyzer(dateFormat, DatePickerType.Quarter,
                new() { Lang = new() { YearFormat = yearFormat } }, CultureInfo.InvariantCulture);
            //Act
            var actualResult1 = details.TryPickerStringConvert(possibleDate, out DateTime actualParsedDate1);
            var actualResult2 = details.TryPickerStringConvert(possibleDate, out DateTime actualParsedDate2);
            //Assert            
            Assert.Equal(expectedResult, actualResult1);
            Assert.Equal(expectedParsedDate, actualParsedDate1);
            Assert.Equal(expectedResult, actualResult2);
            Assert.Equal(expectedParsedDate, actualParsedDate2);
        }

        public static IEnumerable<object[]> TryParseQuarter_ShouldReturnCorrectBool_seed => new List<object[]>
        {
            new object[] { "yyyy", "yyyy-Q0", "2020-Q2", true, new DateTime(2020, 4, 1) },
            new object[] { "yyyy年", "yyyy年-Q0", "2020年-Q1", true, new DateTime(2020, 1, 1) },
            new object[] { "AAyyyy年", "AAyyyy年-Q0", "AA2020年-Q1", true, new DateTime(2020, 1, 1) },
            new object[] { "  AAyyyy年", "  AAyyyy年-Q0", "  AA2020年-Q1", true, new DateTime(2020, 1, 1) },
            new object[] { "  AAyyyy年", "  AAyyyy年-Q0", " AA2020年-Q1", false, null!},
            new object[] { "yyyy", "yyyy-Q0","2020-Q1", true, new DateTime(2020, 1, 1) },
            new object[] { "yyyy", "yyyy-Q0","2020-Q3", true, new DateTime(2020, 7, 1) },
            new object[] { "yyyy", "yyyy-Q0","2020-Q4", true, new DateTime(2020, 10, 1) },
            new object[] { "yyyy", "yyyy-Q0","0020-Q4", true, new DateTime(20, 10, 1) },
            new object[] { "yyyy", "yyyy-Q0","2020-q4", true, new DateTime(2020, 10, 1) },
            new object[] { "yyyy", "yyyy-Q0","2020-Q5", false, null!},
            new object[] { "yyyy", "yyyy-Q0","2020-Q0", false, null!},
            new object[] { "yyyy", "yyyy-Q0","2020-1", false, null!},
            new object[] { "yyyy", "yyyy-Q0","202-Q1", false, null!},
            new object[] { "yyyy", "yyyy-Q0","20 2-Q1", false, null!},
            new object[] { "yyyy", "yyyy-Q0"," 202-Q1", false, null!},
            new object[] { "yyyy", "yyyy-Q0","2020/Q1", false, null!},
            new object[] { "yyyy", "yyyy-Q0", "2020 Q1", false, null!},
        };
    }
}
