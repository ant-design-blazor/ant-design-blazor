using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Sdk;

namespace AntDesign.Tests.DatePicker
{
    using static RangePickerDefaultsTests.ExpectedSource;
    public class RangePickerDefaultsTests
    {
        public enum ExpectedSource
        {
            Value,
            DefaultValue,
            DefaultPickerValue,
            Now,
            Minimum
        }

        private static DateTime _now = DateTime.Now;

        [Theory]
        [MemberData(nameof(ProcessDefaultsWithNullableArrayScenarios))]
        public void ProcessDefaults_ShouldEvaluateToExpected_WhenTypeIsNullableArray(
            DateTime?[] value, DateTime?[] defaultValue, DateTime?[] defaultPickerValue,
            ExpectedSource expectedFirstSource, ExpectedSource expectedSecondSource
            )
        {
            //Arrange            
            var pickerValues = new DateTime[] { _now, _now };
            var useDefaultPickerValue = new bool[] { false, false };
            //Act
            RangePickerDefaults.ProcessDefaults<DateTime?[]>(value, defaultValue, defaultPickerValue, pickerValues, useDefaultPickerValue);
            //Assert
            Assert.Equal(FetchExpectedNullable(value, defaultValue, defaultPickerValue, expectedFirstSource, 0), pickerValues[0]);
            Assert.Equal(FetchExpectedNullable(value, defaultValue, defaultPickerValue, expectedSecondSource, 1), pickerValues[1]);
        }

        private static DateTime? FetchExpectedNullable(DateTime?[] value, DateTime?[] defaultValue,
            DateTime?[] defaultPickerValue, ExpectedSource expectedFirstSource, int index
            )
        {
            return expectedFirstSource switch
            {
                Value => value[index],
                DefaultValue => defaultValue[index],
                DefaultPickerValue => defaultPickerValue[index],
                Minimum => DateTime.MinValue,
                Now => _now,
                _ => throw new ArgumentException(typeof(ExpectedSource).ToString())
            };
        }

        public static IEnumerable<object[]> ProcessDefaultsWithNullableArrayScenarios() => new List<object[]>
        {
            //first
            new object[] { DN("2020-11-10", "2020-11-20"), DN("2020-08-10", "2020-08-20"), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN(null!       , "2020-11-20"), DN("2020-08-10", "2020-08-20"), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN("2020-11-10", "2020-11-20"), DN("2020-08-10", "2020-08-20"), DN(null!       , "2021-12-10"), Value             , DefaultPickerValue },
            new object[] { DN(null!       , "2020-11-20"), DN("2020-08-10", "2020-08-20"), DN(null!       , "2021-12-10"), DefaultValue      , DefaultPickerValue },
            new object[] { DN("2020-11-10", "2020-11-20"), DN(null!       , "2020-08-20"), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN("2020-11-10", "2020-11-20"), DN(null!       , "2020-08-20"), DN(null!       , "2021-12-10"), Value             , DefaultPickerValue },
            new object[] { DN(null!       , "2020-11-20"), DN(null!       , "2020-08-20"), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN(null!       , "2020-11-20"), DN(null!       , "2020-08-20"), DN(null!       , "2021-12-10"), Now               , DefaultPickerValue },
            ////second
            new object[] { DN("2020-11-10", null!       ), DN("2020-08-10", "2020-08-20"), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN("2020-11-10", "2020-11-20"), DN("2020-08-10", "2020-08-20"), DN("2020-09-10", null!       ), DefaultPickerValue, Value              },
            new object[] { DN("2020-11-10", null!       ), DN("2020-08-10", "2020-08-20"), DN("2020-09-10", null!       ), DefaultPickerValue, DefaultValue       },
            new object[] { DN("2020-11-10", "2020-11-20"), DN("2020-08-10", null!       ), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN("2020-11-10", "2020-11-20"), DN("2020-08-10", null!       ), DN("2020-09-10", null!       ), DefaultPickerValue, Value              },
            new object[] { DN("2020-11-10", null!       ), DN("2020-08-10", null!       ), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN("2020-11-10", null!       ), DN("2020-08-10", null!       ), DN("2020-09-10", null!       ), DefaultPickerValue, Now                },
            ////null!Value
            new object[] { default(DateTime?[])!                    , DN("2020-08-10", "2020-08-20"), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DN("2020-08-10", "2020-08-20"), DN(null!       , "2021-12-10"), DefaultValue      , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DN("2020-08-10", "2020-08-20"), DN("2020-09-10", null!       ), DefaultPickerValue, DefaultValue       },

            new object[] { default(DateTime?[])!                    , DN("2020-08-10", null!       ), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DN("2020-08-10", null!       ), DN(null!       , "2021-12-10"), DefaultValue      , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DN("2020-08-10", null!       ), DN("2020-09-10", null!       ), DefaultPickerValue, Now                },

            new object[] { default(DateTime?[])!                    , DN(null!       , "2020-08-20"), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DN(null!       , "2020-08-20"), DN(null!       , "2021-12-10"), Now               , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DN(null!       , "2020-08-20"), DN("2020-09-10", null!       ), DefaultPickerValue, DefaultValue       },

            new object[] { default(DateTime?[])!                    , DN(null!       , null!       ), DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DN(null!       , null!       ), DN(null!       , "2021-12-10"), Now               , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DN(null!       , null!       ), DN("2020-09-10", null!       ), DefaultPickerValue, Now                },

            new object[] { default(DateTime?[])!                    , DN(null!       , null!       ), DN(null!       , null!       ), Now               , Now                },
            //null!DefaultValue
            new object[] { DN("2020-11-10", "2020-11-20"), default(DateTime?[])!                    , DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN("2020-11-10", "2020-11-20"), default(DateTime?[])!                    , DN(null!       , "2021-12-10"), Value             , DefaultPickerValue },
            new object[] { DN("2020-11-10", "2020-11-20"), default(DateTime?[])!                    , DN("2020-09-10", null!       ), DefaultPickerValue, Value              },

            new object[] { DN("2020-11-10", null!       ), default(DateTime?[])!                    , DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN("2020-11-10", null!       ), default(DateTime?[])!                    , DN(null!       , "2021-12-10"), Value             , DefaultPickerValue },
            new object[] { DN("2020-11-10", null!       ), default(DateTime?[])!                    , DN("2020-09-10", null!       ), DefaultPickerValue, Now                },

            new object[] { DN(null!       , "2020-11-20"), default(DateTime?[])!                    , DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DN(null!       , "2020-11-20"), default(DateTime?[])!                    , DN(null!       , "2021-12-10"), Now               , DefaultPickerValue },
            new object[] { DN(null!       , "2020-11-20"), default(DateTime?[])!                    , DN("2020-09-10", null!       ), DefaultPickerValue, Value              },

            new object[] { DN(null!       , null!       ), default(DateTime?[])!                    , DN(null!       , null!       ), Now               , Now                },
            //null!DefaultPickerValue            
            new object[] { DN("2020-11-10", "2020-11-20"), DN("2020-08-10", "2020-08-20"), default(DateTime?[])!                    , Value             , Value              },
            new object[] { DN("2020-11-10", "2020-11-20"), DN(null!       , "2020-08-20"), default(DateTime?[])!                    , Value             , Value              },
            new object[] { DN("2020-11-10", "2020-11-20"), DN("2020-08-10", null!       ), default(DateTime?[])!                    , Value             , Value              },

            new object[] { DN("2020-11-10", null!       ), DN("2020-08-10", "2020-08-20"), default(DateTime?[])!                    , Value             , DefaultValue       },
            new object[] { DN("2020-11-10", null!       ), DN(null!       , "2020-08-20"), default(DateTime?[])!                    , Value             , DefaultValue       },
            new object[] { DN("2020-11-10", null!       ), DN("2020-08-10", null!       ), default(DateTime?[])!                    , Value             , Now                },

            new object[] { DN(null!       , "2020-11-20"), DN("2020-08-10", "2020-08-20"), default(DateTime?[])!                    , DefaultValue      , Value              },
            new object[] { DN(null!       , "2020-11-20"), DN(null!       , "2020-08-20"), default(DateTime?[])!                    , Now               , Value              },
            new object[] { DN(null!       , "2020-11-20"), DN("2020-08-10", null!       ), default(DateTime?[])!                    , DefaultValue      , Value              },

            new object[] { DN(null!       , null!       ), DN(null!       , null!       ), default(DateTime?[])!                    , Now               , Now                },
            //null!Value && null!DefaultValue
            new object[] { default(DateTime?[])!                    , default(DateTime?[])!                    , DN("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , default(DateTime?[])!                    , DN(null!       , "2021-12-10"), Now               , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , default(DateTime?[])!                    , DN("2020-09-10", null!       ), DefaultPickerValue, Now                },

            new object[] { default(DateTime?[])!                    , default(DateTime?[])!                    , DN(null!       , null!       ), Now               , Now                },
            //null!Value && null!DefaultPickerValue
            new object[] { default(DateTime?[])!                    , DN("2020-08-10", "2020-08-20"), default(DateTime?[])!                    , DefaultValue      , DefaultValue       },
            new object[] { default(DateTime?[])!                    , DN("2020-08-10", null!       ), default(DateTime?[])!                    , DefaultValue      , Now                },
            new object[] { default(DateTime?[])!                    , DN(null!       , "2020-08-20"), default(DateTime?[])!                    , Now               , DefaultValue       },

            new object[] { default(DateTime?[])!                    , DN(null!       , null!       ), default(DateTime?[])!                    , Now               , Now                },
            //null!DefaultValue && null!DefaultPickerValue
            new object[] { DN("2020-11-10", "2020-11-20"), default(DateTime?[])!                    , default(DateTime?[])!                    , Value             , Value              },
            new object[] { DN("2020-11-10", null!       ), default(DateTime?[])!                    , default(DateTime?[])!                    , Value             , Now                },
            new object[] { DN(null!       , "2020-11-20"), default(DateTime?[])!                    , default(DateTime?[])!                    , Now               , Value              },

            new object[] { DN(null!       , null!       ), default(DateTime?[])!                    , default(DateTime?[])!                    , Now               , Now                },

        };

        [Theory]
        [MemberData(nameof(ProcessDefaultsWithNotNullableArrayScenarios))]
        public void ProcessDefaults_ShouldEvaluateToExpected_WhenTypeIsNotNullableArray(
            DateTime[] value, DateTime[] defaultValue, DateTime[] defaultPickerValue,
            ExpectedSource expectedFirstSource, ExpectedSource expectedSecondSource
            )
        {
            //Arrange            
            var pickerValues = new DateTime[] { _now, _now };
            var useDefaultPickerValue = new bool[] { false, false };
            //Act
            RangePickerDefaults.ProcessDefaults<DateTime[]>(value, defaultValue, defaultPickerValue, pickerValues, useDefaultPickerValue);
            //Assert
            Assert.Equal(FetchExpectedNotNullable(value, defaultValue, defaultPickerValue, expectedFirstSource, 0), pickerValues[0]);
            Assert.Equal(FetchExpectedNotNullable(value, defaultValue, defaultPickerValue, expectedSecondSource, 1), pickerValues[1]);

        }


        private static DateTime FetchExpectedNotNullable(DateTime[] value, DateTime[] defaultValue,
            DateTime[] defaultPickerValue, ExpectedSource expectedFirstSource, int index
            )
        {
            return expectedFirstSource switch
            {
                Value => value[index],
                DefaultValue => defaultValue[index],
                DefaultPickerValue => defaultPickerValue[index],
                Minimum => DateTime.MinValue,
                Now => _now,
                _ => throw new ArgumentException(typeof(ExpectedSource).ToString())
            };
        }

        public static IEnumerable<object[]> ProcessDefaultsWithNotNullableArrayScenarios() => new List<object[]>
        {
            //first
            new object[] { DV("2020-11-10", "2020-11-20"), DV("2020-08-10", "2020-08-20"), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV(null!       , "2020-11-20"), DV("2020-08-10", "2020-08-20"), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV("2020-11-10", "2020-11-20"), DV("2020-08-10", "2020-08-20"), DV(null!       , "2021-12-10"), Value             , DefaultPickerValue },
            new object[] { DV(null!       , "2020-11-20"), DV("2020-08-10", "2020-08-20"), DV(null!       , "2021-12-10"), DefaultValue      , DefaultPickerValue },
            new object[] { DV("2020-11-10", "2020-11-20"), DV(null!       , "2020-08-20"), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV("2020-11-10", "2020-11-20"), DV(null!       , "2020-08-20"), DV(null!       , "2021-12-10"), Value             , DefaultPickerValue },
            new object[] { DV(null!       , "2020-11-20"), DV(null!       , "2020-08-20"), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV(null!       , "2020-11-20"), DV(null!       , "2020-08-20"), DV(null!       , "2021-12-10"), Minimum           , DefaultPickerValue },
            ////second
            new object[] { DV("2020-11-10", null!       ), DV("2020-08-10", "2020-08-20"), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV("2020-11-10", "2020-11-20"), DV("2020-08-10", "2020-08-20"), DV("2020-09-10", null!       ), DefaultPickerValue, Value              },
            new object[] { DV("2020-11-10", null!       ), DV("2020-08-10", "2020-08-20"), DV("2020-09-10", null!       ), DefaultPickerValue, DefaultValue       },
            new object[] { DV("2020-11-10", "2020-11-20"), DV("2020-08-10", null!       ), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV("2020-11-10", "2020-11-20"), DV("2020-08-10", null!       ), DV("2020-09-10", null!       ), DefaultPickerValue, Value              },
            new object[] { DV("2020-11-10", null!       ), DV("2020-08-10", null!       ), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV("2020-11-10", null!       ), DV("2020-08-10", null!       ), DV("2020-09-10", null!       ), DefaultPickerValue, Minimum            },
            ////null!Value
            new object[] { default(DateTime?[])!                    , DV("2020-08-10", "2020-08-20"), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DV("2020-08-10", "2020-08-20"), DV(null!       , "2021-12-10"), DefaultValue      , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DV("2020-08-10", "2020-08-20"), DV("2020-09-10", null!       ), DefaultPickerValue, DefaultValue       },

            new object[] { default(DateTime?[])!                    , DV("2020-08-10", null!       ), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DV("2020-08-10", null!       ), DV(null!       , "2021-12-10"), DefaultValue      , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DV("2020-08-10", null!       ), DV("2020-09-10", null!       ), DefaultPickerValue, Now                },

            new object[] { default(DateTime?[])!                    , DV(null!       , "2020-08-20"), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DV(null!       , "2020-08-20"), DV(null!       , "2021-12-10"), Now               , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DV(null!       , "2020-08-20"), DV("2020-09-10", null!       ), DefaultPickerValue, DefaultValue       },

            new object[] { default(DateTime?[])!                    , DV(null!       , null!       ), DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DV(null!       , null!       ), DV(null!       , "2021-12-10"), Now               , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , DV(null!       , null!       ), DV("2020-09-10", null!       ), DefaultPickerValue, Now                },

            new object[] { default(DateTime?[])!                    , DV(null!       , null!       ), DV(null!       , null!       ), Now               , Now                },
            //null!DefaultValue
            new object[] { DV("2020-11-10", "2020-11-20"), default(DateTime?[])!                    , DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV("2020-11-10", "2020-11-20"), default(DateTime?[])!                    , DV(null!       , "2021-12-10"), Value             , DefaultPickerValue },
            new object[] { DV("2020-11-10", "2020-11-20"), default(DateTime?[])!                    , DV("2020-09-10", null!       ), DefaultPickerValue, Value              },

            new object[] { DV("2020-11-10", null!       ), default(DateTime?[])!                    , DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV("2020-11-10", null!       ), default(DateTime?[])!                    , DV(null!       , "2021-12-10"), Value             , DefaultPickerValue },
            new object[] { DV("2020-11-10", null!       ), default(DateTime?[])!                    , DV("2020-09-10", null!       ), DefaultPickerValue, Minimum            },

            new object[] { DV(null!       , "2020-11-20"), default(DateTime?[])!                    , DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { DV(null!       , "2020-11-20"), default(DateTime?[])!                    , DV(null!       , "2021-12-10"), Minimum           , DefaultPickerValue },
            new object[] { DV(null!       , "2020-11-20"), default(DateTime?[])!                    , DV("2020-09-10", null!       ), DefaultPickerValue, Value              },

            new object[] { DV(null!       , null!       ), default(DateTime?[])!                    , DV(null!       , null!       ), Minimum           , Minimum            },
            //null!DefaultPickerValue            
            new object[] { DV("2020-11-10", "2020-11-20"), DV("2020-08-10", "2020-08-20"), default(DateTime?[])!                    , Value             , Value              },
            new object[] { DV("2020-11-10", "2020-11-20"), DV(null!       , "2020-08-20"), default(DateTime?[])!                    , Value             , Value              },
            new object[] { DV("2020-11-10", "2020-11-20"), DV("2020-08-10", null!       ), default(DateTime?[])!                    , Value             , Value              },

            new object[] { DV("2020-11-10", null!       ), DV("2020-08-10", "2020-08-20"), default(DateTime?[])!                    , Value             , DefaultValue       },
            new object[] { DV("2020-11-10", null!       ), DV(null!       , "2020-08-20"), default(DateTime?[])!                    , Value             , DefaultValue       },
            new object[] { DV("2020-11-10", null!       ), DV("2020-08-10", null!       ), default(DateTime?[])!                    , Value             , Minimum            },

            new object[] { DV(null!       , "2020-11-20"), DV("2020-08-10", "2020-08-20"), default(DateTime?[])!                    , DefaultValue      , Value              },
            new object[] { DV(null!       , "2020-11-20"), DV(null!       , "2020-08-20"), default(DateTime?[])!                    , Minimum           , Value              },
            new object[] { DV(null!       , "2020-11-20"), DV("2020-08-10", null!       ), default(DateTime?[])!                    , DefaultValue      , Value              },

            new object[] { DV(null!       , null!       ), DV(null!       , null!       ), default(DateTime?[])!                    , Minimum           , Minimum            },
            //null!Value && null!DefaultValue
            new object[] { default(DateTime?[])!                    , default(DateTime?[])!                    , DV("2020-09-10", "2021-12-10"), DefaultPickerValue, DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , default(DateTime?[])!                    , DV(null!       , "2021-12-10"), Now               , DefaultPickerValue },
            new object[] { default(DateTime?[])!                    , default(DateTime?[])!                    , DV("2020-09-10", null!       ), DefaultPickerValue, Now                },

            new object[] { default(DateTime?[])!                    , default(DateTime?[])!                    , DV(null!       , null!       ), Now               , Now                },
            //null!Value && null!DefaultPickerValue
            new object[] { default(DateTime?[])!                    , DV("2020-08-10", "2020-08-20"), default(DateTime?[])!                    , DefaultValue      , DefaultValue       },
            new object[] { default(DateTime?[])!                    , DV("2020-08-10", null!       ), default(DateTime?[])!                    , DefaultValue      , Now                },
            new object[] { default(DateTime?[])!                    , DV(null!       , "2020-08-20"), default(DateTime?[])!                    , Now               , DefaultValue       },

            new object[] { default(DateTime?[])!                    , DV(null!       , null!       ), default(DateTime?[])!                    , Now               , Now                },
            //null!DefaultValue && null!DefaultPickerValue
            new object[] { DV("2020-11-10", "2020-11-20"), default(DateTime?[])!                    , default(DateTime?[])!                    , Value             , Value              },
            new object[] { DV("2020-11-10", null!       ), default(DateTime?[])!                    , default(DateTime?[])!                    , Value             , Minimum            },
            new object[] { DV(null!       , "2020-11-20"), default(DateTime?[])!                    , default(DateTime?[])!                    , Minimum           , Value              },

            new object[] { DV(null!       , null!       ), default(DateTime?[])!                    , default(DateTime?[])!                    , Minimum           , Minimum            },
        };

        #region date helpers
        private static DateTime?[] DN(string? first = null, string? second = null)
        {
            DateTime? firstDate;
            DateTime? secondDate;
            if (first != null)
                firstDate = DateTime.Parse(first);
            else
                firstDate = null;
            if (second != null)
                secondDate = DateTime.Parse(second);
            else
                secondDate = null;

            return new DateTime?[] { firstDate, secondDate };
        }

        private static DateTime[] DV(string first, string second)
        {
            DateTime firstDate;
            DateTime secondDate;
            if (first != null)
                firstDate = DateTime.Parse(first);
            else
                firstDate = DateTime.MinValue;
            if (second != null)
                secondDate = DateTime.Parse(second);
            else
                secondDate = DateTime.MinValue;

            return new DateTime[] { firstDate, secondDate };
        }
        #endregion
    }

}
