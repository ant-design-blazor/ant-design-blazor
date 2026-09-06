// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AntDesign.Filters;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class TableFilterInputRenderOptionsTests
    {
        private sealed class InputRefHolder
        {
            public object? Value;
        }

        [Fact]
        public void Render_options_expose_filter_state_and_confirm_without_closing()
        {
            var filter = new TableFilter
            {
                Value = "initial",
                FilterCompareOperator = TableFilterCompareOperator.Contains,
            };
            var confirmedValues = new List<bool>();
            var options = new TableFilterInputRenderOptions(filter, new Dictionary<string, object>(), Expression.Constant(null), confirmedValues.Add);

            options.Value = "updated";
            options.Confirm();

            Assert.Equal("updated", filter.Value);
            Assert.Equal("updated", options.Value);
            Assert.Equal(TableFilterCompareOperator.Contains, options.FilterCompareOperator);
            Assert.Equal(new[] { false }, confirmedValues);
        }

        [Fact]
        public void Input_ref_reads_and_writes_captured_field()
        {
            var holder = new InputRefHolder();
            var inputRefExpression = Expression.Field(Expression.Constant(holder), nameof(InputRefHolder.Value));
            var options = new TableFilterInputRenderOptions(
                new TableFilter(),
                new Dictionary<string, object>(),
                inputRefExpression,
                _ => { });

            options.InputRef = "captured";

            Assert.Equal("captured", holder.Value);
            Assert.Equal("captured", options.InputRef);
        }

        [Fact]
        public void Input_ref_returns_null_for_non_member_expressions_and_ignores_writes()
        {
            var options = new TableFilterInputRenderOptions(
                new TableFilter(),
                new Dictionary<string, object>(),
                Expression.Constant(null),
                _ => { });

            Assert.Null(options.InputRef);
            options.InputRef = "ignored";
            Assert.Null(options.InputRef);
        }
    }
}
