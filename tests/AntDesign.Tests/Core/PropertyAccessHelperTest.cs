// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using AntDesign.core.Helpers;
using Xunit;

namespace AntDesign.Tests.Core
{
    public class PropertyAccessHelperTest
    {
        public class CA
        {
            public int A1 { get; set; }

            public string A2 { get; set; }

            public CB A3 { get; set; }

            public SA A4 { get; set; }

            public SA? A5 { get; set; }

            public List<SA?[]> A6 { get; set; }
        }

        public class CB
        {
            public int B1 { get; set; }
        }

        public struct SA
        {
            public int A1 { get; set; }

            public string A2 { get; set; }

            public CB A3 { get; set; }

            public SB A4 { get; set; }

            public SB? A5 { get; set; }
        }

        public struct SB
        {
            public string B1 { get; set; }

            public int B2 { get; set; }

            public int? B3 { get; set; }
        }

        [Fact]
        public void TestIndexAccess()
        {
            var cr = new CA[]
            {
                new CA() { },
                new CA()
                {
                    A1 = 111,
                    A4 = new SA()
                    {
                        A1 = 133,
                        A5 = new SB()
                        {
                            B1 = "CA A5",
                            B2 = 123456
                        }
                    },
                    A6 = new List<SA?[]>
                    {
                        new SA?[3],
                        new SA?[2],
                        new SA?[2]
                        {
                            new SA(),
                            new SA()
                            {
                                A1 = 567,
                                A2 = "222",
                                A5 = new SB() {B1 = "test index"}
                            },
                        }
                    }
                }
            };
            var cl = new List<CA>(cr);

            var arrayFuncExp = typeof(CA[]).BuildAccessNullablePropertyLambdaExpression<CA[], string>("[1].A6[2][1].A5.B1");
            var listFuncExp = typeof(List<CA>).BuildAccessNullablePropertyLambdaExpression<List<CA>, string>("[1].A6[2][1].A5.B1");
            var arrayFunc = arrayFuncExp.Compile();
            var listFunc = listFuncExp.Compile();
            var arr = arrayFunc.Invoke(cr);
            var lst = listFunc.Invoke(cl);
            Assert.Equal(arr, cr[1].A6[2][1].Value.A5.Value.B1);
            Assert.Equal(lst, cl[1].A6[2][1].Value.A5.Value.B1);

            {
                var dict = new Dictionary<string, Dictionary<int, string>>();
                dict["DSa"] = new Dictionary<int, string>()
                {
                    {1, "B1 Value"},
                    {3, "B2 Value"},
                };
                var exp = dict.GetType().BuildAccessPropertyLambdaExpression<Dictionary<string, Dictionary<int, string>>, string>("[\"DSa\"][3]").Compile();
                var result = exp.Invoke(dict);
                Assert.Equal(dict["DSa"][3], result);
            }
            {
                var dict = new Dictionary<string, Dictionary<string, string>>();
                dict["B"] = new Dictionary<string, string>()
                {
                    {"B1", "B1 Value"},
                    {"B2", "B2 Value"},
                };

                var exp = dict.GetType().BuildAccessNullablePropertyLambdaExpression<Dictionary<string, Dictionary<string, string>>, string>("[\"B\"][\"B1\"]").Compile();
                var result = exp.Invoke(dict);
                Assert.Equal(dict["B"]["B1"], result);
            }
            {
                var dict = new Dictionary<string, Dictionary<string, string>>();
                dict["A"] = new Dictionary<string, string>()
                {
                    {"A1", null},
                };

                var exp = dict.GetType().BuildAccessNullablePropertyLambdaExpression<Dictionary<string, Dictionary<string, string>>, string>("[\"A\"][\"B1\"]").Compile();
                var result = exp.Invoke(dict);
                Assert.Null(result);

                var exp2 = dict.GetType().BuildAccessNullablePropertyLambdaExpression<Dictionary<string, Dictionary<string, string>>, string>("[\"A\"][\"B1\"]").Compile();
                var result2 = exp2.Invoke(dict);
                Assert.Null(result2);
            }
        }

        [Fact]
        public void TestDefaultValue()
        {
            typeof(CA).TestDefaultValue("A3.B1", 12);
            typeof(CA).TestDefaultValue("A4.A2", "asdd");
            typeof(CA).TestDefaultValue("A4.A3", (CB?)null);
            typeof(CA).TestDefaultValue("A4.A3.B1", 145);
            typeof(CA).TestDefaultValue("A4.A4.B1", "sfasf");
            typeof(CA).TestDefaultValue("A4.A4.B3", 5967);
            typeof(CA).TestDefaultValue("A4.A5.B1", "sadas");
            typeof(CA).TestDefaultValue("A4.A5.B2", 521);

            typeof(SA).TestDefaultValue("A2", "t232");
            typeof(SA).TestDefaultValue("A3", new CB() {B1 = 6666});
            typeof(SA).TestDefaultValue("A3.B1", 5678);

            typeof(SA).TestDefaultValue("A4.B1", "BBBB1");
            typeof(SA).TestDefaultValue("A5", (SB?)null);
            typeof(SA).TestDefaultValue(
                "A5",
                new SB()
                {
                    B1 = "A5555",
                    B2 = 5555
                });
            typeof(SA).TestDefaultValue("A5.B1", "A5B1...");
            typeof(SA).TestDefaultValue("A5.B2", 1110);

            typeof(SA?).TestDefaultValue("A1", 5123);
            typeof(SA?).TestDefaultValue("A2", "?A1");
            typeof(SA?).TestDefaultValue("A3", (CB?)null);
            typeof(SA?).TestDefaultValue("A3.B1", -123123);
            typeof(SA?).TestDefaultValue(
                "A4",
                new SB()
                {
                    B1 = "SA?.A4",
                    B2 = -1
                });
            typeof(SA?).TestDefaultValue("A4.B1", "A4.B1111");
            typeof(SA?).TestDefaultValue("A4.B2", (int?)null);
            typeof(SA?).TestDefaultValue("A5", (SB?)null);
            typeof(SA?).TestDefaultValue("A5.B1", (string?)null);
            typeof(SA?).TestDefaultValue("A5.B2", -1266);
        }

        [Fact]
        public void TestNotNullPropertyPath()
        {
            var x1 = typeof(CA).AccessProperty("A3.B1").ToDelegate();
            var x2 = typeof(CA).AccessProperty("A4.A1").ToDelegate();
            var x3 = typeof(CA).AccessProperty("A4.A2").ToDelegate();
            var x4 = typeof(CA).AccessProperty("A4.A3").ToDelegate();
            var x5 = typeof(CA).AccessProperty("A4.A3.B1").ToDelegate();
            var x6 = typeof(CA).AccessProperty("A4.A4.B1").ToDelegate();
            var x7 = typeof(CA).AccessProperty("A4.A4.B2").ToDelegate();
            var x8 = typeof(CA).AccessProperty("A4.A5.B1").ToDelegate();
            var x9 = typeof(CA).AccessProperty("A4.A5.B2").ToDelegate();

            var y1 = typeof(SA).AccessProperty("A1").ToDelegate();
            var y2 = typeof(SA).AccessProperty("A2").ToDelegate();
            var y3 = typeof(SA).AccessProperty("A3").ToDelegate();
            var y3_1 = typeof(SA).AccessProperty("A3.B1").ToDelegate();
            var y4 = typeof(SA).AccessProperty("A4").ToDelegate();
            var y4_1 = typeof(SA).AccessProperty("A4.B1").ToDelegate();
            var y4_2 = typeof(SA).AccessProperty("A4.B2").ToDelegate();
            var y5 = typeof(SA).AccessProperty("A5").ToDelegate();
            var y6 = typeof(SA).AccessProperty("A5.B1").ToDelegate();
            var y7 = typeof(SA).AccessProperty("A5.B2").ToDelegate();

            var z1 = typeof(SA?).AccessProperty("A1").ToDelegate();
            var z2 = typeof(SA?).AccessProperty("A2").ToDelegate();
            var z3 = typeof(SA?).AccessProperty("A3").ToDelegate();
            var z3_1 = typeof(SA?).AccessProperty("A3.B1").ToDelegate();
            var z4 = typeof(SA?).AccessProperty("A4").ToDelegate();
            var z4_1 = typeof(SA?).AccessProperty("A4.B1").ToDelegate();
            var z4_2 = typeof(SA?).AccessProperty("A4.B2").ToDelegate();
            var z5 = typeof(SA?).AccessProperty("A5").ToDelegate();
            var z6 = typeof(SA?).AccessProperty("A5.B1").ToDelegate();
            var z7 = typeof(SA?).AccessProperty("A5.B2").ToDelegate();
        }

        [Fact]
        public void TestNullablePropertyPath()
        {
            var x1 = typeof(CA).AccessNullableProperty("A3.B1").ToDelegate();
            var x2 = typeof(CA).AccessNullableProperty("A4.A1").ToDelegate();
            var x3 = typeof(CA).AccessNullableProperty("A4.A2").ToDelegate();
            var x4 = typeof(CA).AccessNullableProperty("A4.A3").ToDelegate();
            var x5 = typeof(CA).AccessNullableProperty("A4.A3.B1").ToDelegate();
            var x6 = typeof(CA).AccessNullableProperty("A4.A4.B1").ToDelegate();
            var x7 = typeof(CA).AccessNullableProperty("A4.A4.B2").ToDelegate();
            var x8 = typeof(CA).AccessNullableProperty("A4.A5.B1").ToDelegate();
            var x9 = typeof(CA).AccessNullableProperty("A4.A5.B2").ToDelegate();

            var y1 = typeof(SA).AccessNullableProperty("A1").ToDelegate();
            var y2 = typeof(SA).AccessNullableProperty("A2").ToDelegate();
            var y3 = typeof(SA).AccessNullableProperty("A3").ToDelegate();
            var y3_1 = typeof(SA).AccessNullableProperty("A3.B1").ToDelegate();
            var y4 = typeof(SA).AccessNullableProperty("A4").ToDelegate();
            var y4_1 = typeof(SA).AccessNullableProperty("A4.B1").ToDelegate();
            var y4_2 = typeof(SA).AccessNullableProperty("A4.B2").ToDelegate();
            var y5 = typeof(SA).AccessNullableProperty("A5").ToDelegate();
            var y6 = typeof(SA).AccessNullableProperty("A5.B1").ToDelegate();
            var y7 = typeof(SA).AccessNullableProperty("A5.B2").ToDelegate();

            var z1 = typeof(SA?).AccessNullableProperty("A1").ToDelegate();
            var z2 = typeof(SA?).AccessNullableProperty("A2").ToDelegate();
            var z3 = typeof(SA?).AccessNullableProperty("A3").ToDelegate();
            var z3_1 = typeof(SA?).AccessNullableProperty("A3.B1").ToDelegate();
            var z4 = typeof(SA?).AccessNullableProperty("A4").ToDelegate();
            var z4_1 = typeof(SA?).AccessNullableProperty("A4.B1").ToDelegate();
            var z4_2 = typeof(SA?).AccessNullableProperty("A4.B2").ToDelegate();
            var z5 = typeof(SA?).AccessNullableProperty("A5").ToDelegate();
            var z6 = typeof(SA?).AccessNullableProperty("A5.B1").ToDelegate();
            var z7 = typeof(SA?).AccessNullableProperty("A5.B2").ToDelegate();
        }
    }

    public static class PropertyAccessHelperTestTool
    {
        public static void TestDefaultValue<TValue>(this Type type, string propertyPath, TValue value)
        {
            var func = type.AccessPropertyDefaultIfNull(propertyPath, value).ToDelegate();
            var obj = Activator.CreateInstance(type);
            TValue res = (TValue)func.DynamicInvoke(obj);
            Assert.Equal(value, res);
        }
    }
}
