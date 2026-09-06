// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AntDesign.Filters;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class FilterTypeResolverTests
    {
        private enum TestKind { Alpha }

        private sealed class UnknownType { }

        [Theory]
        [InlineData(typeof(byte), typeof(NumberFieldFilterType<byte>))]
        [InlineData(typeof(int), typeof(NumberFieldFilterType<int>))]
        [InlineData(typeof(long), typeof(NumberFieldFilterType<long>))]
        [InlineData(typeof(decimal), typeof(NumberFieldFilterType<decimal>))]
        [InlineData(typeof(double), typeof(NumberFieldFilterType<double>))]
        [InlineData(typeof(DateTime), typeof(DateTimeFieldFilterType))]
        [InlineData(typeof(string), typeof(StringFieldFilterType))]
        [InlineData(typeof(Guid), typeof(GuidFieldFilterType))]
        public void Resolver_maps_underlying_types_to_field_filter_types(Type underlyingType, Type expectedType)
        {
            var resolver = new DefaultFieldFilterTypeResolver();

            Assert.IsType(expectedType, resolver.Resolve(underlyingType));
        }

        [Fact]
        public void Resolver_maps_enums_to_enum_filter_type_and_returns_null_for_unknown_types()
        {
            var resolver = new DefaultFieldFilterTypeResolver();

            Assert.IsType<EnumFieldFilterType<TestKind>>(resolver.Resolve<TestKind>());
            Assert.Null(resolver.Resolve(typeof(UnknownType)));
        }

        [Fact]
        public void Resolver_unwraps_nullable_types()
        {
            var resolver = new DefaultFieldFilterTypeResolver();

            Assert.IsType<NumberFieldFilterType<int>>(resolver.Resolve<int?>());
        }
    }
}
