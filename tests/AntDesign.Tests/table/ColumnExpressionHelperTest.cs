// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using AntDesign.core.Helpers;
using AntDesign.Internal;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class ColumnExpressionHelperTest
    {
        [Fact]
        public void EndsWithGetItem()
        {
            var t1 = new T1()
            {
                T2 = new()
                {
                    Arr = new()
                    {
                        5,
                        6,
                        7
                    }
                }
            };

            var exp = typeof(T1).BuildAccessPropertyLambdaExpression("T2.Arr[2]");

            var memberInfo = ColumnExpressionHelper.GetReturnMemberInfo(exp);
            Assert.Equal(nameof(T2.Arr), memberInfo.Name);
        }

        [Fact]
        public void NoMember()
        {
            var exp = typeof(List<Dictionary<string, string>>).BuildAccessPropertyLambdaExpression("[1][\"K2\"]");
            var memberInfo = ColumnExpressionHelper.GetReturnMemberInfo(exp);
            Assert.Null(memberInfo);
        }

        public class T1
        {
            public T2 T2 { get; set; }
        }

        public class T2
        {
            public List<int> Arr { get; set; }
        }
    }
}
