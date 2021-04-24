// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using AntDesign.Core.Helpers.MemberPath;
using Xunit;

namespace AntDesign.Tests.Core
{
    public class MemberPathTest
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
                    A4 = new SA() {A1 = 133, A5 = new SB() {B1 = "CA A5", B2 = 123456}},
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

            var arrayFuncExp = PathHelper.GetLambda<CA[], string>("[1].A6[2][1].A5.B1");
            var listFuncExp = PathHelper.GetLambda<List<CA>, string>("[1].A6[2][1].A5.B1");
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
                    {1, "B1 Value"}, {3, "B2 Value"},
                };
                var exp = PathHelper.GetLambda<Dictionary<string, Dictionary<int, string>>, string>("['DSa'][3]").Compile();
                var result = exp.Invoke(dict);
                Assert.Equal(dict["DSa"][3], result);
            }
            {
                var dict = new Dictionary<string, Dictionary<string, string>>();
                dict["B"] = new Dictionary<string, string>()
                {
                    {"B1", "B1 Value"}, {"B2", "B2 Value"},
                };

                var exp = PathHelper.GetLambda<Dictionary<string, Dictionary<string, string>>, string>("['B']['B1']").Compile();
                var result = exp.Invoke(dict);
                Assert.Equal(dict["B"]["B1"], result);
            }
            {
                var dict = new Dictionary<string, Dictionary<string, string>>();
                dict["A"] = new Dictionary<string, string>() {{"A1", null},};

                var exp = PathHelper.GetLambdaDefault<Dictionary<string, Dictionary<string, string>>, string>("['A']['B1']").Compile();
                var result = exp.Invoke(dict);
                Assert.Null(result);

                var exp2 = PathHelper.GetLambdaDefault<Dictionary<string, Dictionary<string, string>>, string>("['A']['B1']").Compile();
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
            typeof(SA).TestDefaultValue("A5", new SB() {B1 = "A5555", B2 = 5555});
            typeof(SA).TestDefaultValue("A5.B1", "A5B1...");
            typeof(SA).TestDefaultValue("A5.B2", 1110);

            typeof(SA?).TestDefaultValue("A1", 5123);
            typeof(SA?).TestDefaultValue("A2", "?A1");
            typeof(SA?).TestDefaultValue("A3", (CB?)null);
            typeof(SA?).TestDefaultValue("A3.B1", -123123);
            typeof(SA?).TestDefaultValue("A4", new SB() {B1 = "SA?.A4", B2 = -1});
            typeof(SA?).TestDefaultValue("A4.B1", "A4.B1111");
            typeof(SA?).TestDefaultValue("A4.B2", (int?)null);
            typeof(SA?).TestDefaultValue("A5", (SB?)null);
            typeof(SA?).TestDefaultValue("A5.B1", (string?)null);
            typeof(SA?).TestDefaultValue("A5.B2", -1266);
        }

        [Fact]
        public void TestNotNullPropertyPath()
        {
            var x1 = PathHelper.GetDelegate("A3.B1", typeof(CA));
            var x2 = PathHelper.GetDelegate("A4.A1", typeof(CA));
            var x3 = PathHelper.GetDelegate("A4.A2", typeof(CA));
            var x4 = PathHelper.GetDelegate("A4.A3", typeof(CA));
            var x5 = PathHelper.GetDelegate("A4.A3.B1", typeof(CA));
            var x6 = PathHelper.GetDelegate("A4.A4.B1", typeof(CA));
            var x7 = PathHelper.GetDelegate("A4.A4.B2", typeof(CA));
            var x8 = PathHelper.GetDelegate("A4.A5.B1", typeof(CA));
            var x9 = PathHelper.GetDelegate("A4.A5.B2", typeof(CA));

            var y1 = PathHelper.GetDelegate("A1", typeof(SA));
            var y2 = PathHelper.GetDelegate("A2", typeof(SA));
            var y3 = PathHelper.GetDelegate("A3", typeof(SA));
            var y3_1 = PathHelper.GetDelegate("A3.B1", typeof(SA));
            var y4 = PathHelper.GetDelegate("A4", typeof(SA));
            var y4_1 = PathHelper.GetDelegate("A4.B1", typeof(SA));
            var y4_2 = PathHelper.GetDelegate("A4.B2", typeof(SA));
            var y5 = PathHelper.GetDelegate("A5", typeof(SA));
            var y6 = PathHelper.GetDelegate("A5.B1", typeof(SA));
            var y7 = PathHelper.GetDelegate("A5.B2", typeof(SA));

            var z1 = PathHelper.GetDelegate("A1", typeof(SA?));
            var z2 = PathHelper.GetDelegate("A2", typeof(SA?));
            var z3 = PathHelper.GetDelegate("A3", typeof(SA?));
            var z3_1 = PathHelper.GetDelegate("A3.B1", typeof(SA?));
            var z4 = PathHelper.GetDelegate("A4", typeof(SA?));
            var z4_1 = PathHelper.GetDelegate("A4.B1", typeof(SA?));
            var z4_2 = PathHelper.GetDelegate("A4.B2", typeof(SA?));
            var z5 = PathHelper.GetDelegate("A5", typeof(SA?));
            var z6 = PathHelper.GetDelegate("A5.B1", typeof(SA?));
            var z7 = PathHelper.GetDelegate("A5.B2", typeof(SA?));
        }

        [Fact]
        public void TestNullablePropertyPath()
        {
            var x1 = PathHelper.GetDelegateDefault("A3.B1", typeof(CA));
            var x2 = PathHelper.GetDelegateDefault("A4.A1", typeof(CA));
            var x3 = PathHelper.GetDelegateDefault("A4.A2", typeof(CA));
            var x4 = PathHelper.GetDelegateDefault("A4.A3", typeof(CA));
            var x5 = PathHelper.GetDelegateDefault("A4.A3.B1", typeof(CA));
            var x6 = PathHelper.GetDelegateDefault("A4.A4.B1", typeof(CA));
            var x7 = PathHelper.GetDelegateDefault("A4.A4.B2", typeof(CA));
            var x8 = PathHelper.GetDelegateDefault("A4.A5.B1", typeof(CA));
            var x9 = PathHelper.GetDelegateDefault("A4.A5.B2", typeof(CA));

            var y1 = PathHelper.GetDelegateDefault("A1", typeof(SA));
            var y2 = PathHelper.GetDelegateDefault("A2", typeof(SA));
            var y3 = PathHelper.GetDelegateDefault("A3", typeof(SA));
            var y3_1 = PathHelper.GetDelegateDefault("A3.B1", typeof(SA));
            var y4 = PathHelper.GetDelegateDefault("A4", typeof(SA));
            var y4_1 = PathHelper.GetDelegateDefault("A4.B1", typeof(SA));
            var y4_2 = PathHelper.GetDelegateDefault("A4.B2", typeof(SA));
            var y5 = PathHelper.GetDelegateDefault("A5", typeof(SA));
            var y6 = PathHelper.GetDelegateDefault("A5.B1", typeof(SA));
            var y7 = PathHelper.GetDelegateDefault("A5.B2", typeof(SA));

            var z1 = PathHelper.GetDelegateDefault("A1", typeof(SA?));
            var z2 = PathHelper.GetDelegateDefault("A2", typeof(SA?));
            var z3 = PathHelper.GetDelegateDefault("A3", typeof(SA?));
            var z3_1 = PathHelper.GetDelegateDefault("A3.B1", typeof(SA?));
            var z4 = PathHelper.GetDelegateDefault("A4", typeof(SA?));
            var z4_1 = PathHelper.GetDelegateDefault("A4.B1", typeof(SA?));
            var z4_2 = PathHelper.GetDelegateDefault("A4.B2", typeof(SA?));
            var z5 = PathHelper.GetDelegateDefault("A5", typeof(SA?));
            var z6 = PathHelper.GetDelegateDefault("A5.B1", typeof(SA?));
            var z7 = PathHelper.GetDelegateDefault("A5.B2", typeof(SA?));
        }
    }

    public static class PropertyAccessHelperTestTool
    {
        public static void TestDefaultValue<TValue>(this Type type, string propertyPath, TValue value)
        {
            var func = PathHelper.GetDelegateDefault(propertyPath, type);
            var obj = Activator.CreateInstance(type);
            TValue res = (TValue)(func.Invoke(obj) ?? value);
            Assert.Equal(value, res);
        }
    }
}
