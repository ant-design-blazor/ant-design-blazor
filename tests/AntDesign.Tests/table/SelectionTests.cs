// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using FluentAssertions;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class SelectionTests
    {
        // https://github.com/ant-design-blazor/ant-design-blazor/issues/4834
        [Fact]
        public void Dispose_does_not_throw_when_table_selection_is_not_set()
        {
            var selection = new Selection
            {
                Table = new Table<object>(),
                IsHeader = false,
            };

            Action dispose = () => selection.Dispose();

            dispose.Should().NotThrow();
        }
    }
}
