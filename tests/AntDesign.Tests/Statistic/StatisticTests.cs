// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using FluentAssertions;
using Xunit;

namespace AntDesign.Tests.Statistic
{
    public class StatisticTests : AntDesignTestBase
    {
        [Fact]
        public void ShouldRoundAndCarryFractionalPartToInteger()
        {
            var cut = RenderComponent<AntDesign.Statistic<decimal>>(parameters => parameters
                .Add(x => x.Value, 99.999m)
                .Add(x => x.Precision, 2));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".00");
        }

        [Fact]
        public void ShouldRoundAndCarryFractionalPartToInteger_Double()
        {
            var cut = RenderComponent<AntDesign.Statistic<double>>(parameters => parameters
                .Add(x => x.Value, 99.999)
                .Add(x => x.Precision, 2));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".00");
        }

        [Fact]
        public void ShouldRoundAndCarryFractionalPartToInteger_Float()
        {
            var cut = RenderComponent<AntDesign.Statistic<float>>(parameters => parameters
                .Add(x => x.Value, 99.9999f)
                .Add(x => x.Precision, 3));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".000");
        }

        [Fact]
        public void ShouldRoundAndCarryFractionalPartToInteger_StringValue()
        {
            var cut = RenderComponent<AntDesign.Statistic<string>>(parameters => parameters
                .Add(x => x.Value, "99.999")
                .Add(x => x.Precision, 2));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".00");
        }

        [Fact]
        public void ShouldRoundAndCarryFractionalPartToInteger_NegativeDecimal()
        {
            var cut = RenderComponent<AntDesign.Statistic<decimal>>(parameters => parameters
                .Add(x => x.Value, -99.999m)
                .Add(x => x.Precision, 2));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("-100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".00");
        }

        [Fact]
        public void ShouldRoundAndCarryFractionalPartToInteger_NegativeStringValue()
        {
            var cut = RenderComponent<AntDesign.Statistic<string>>(parameters => parameters
                .Add(x => x.Value, "-99.999")
                .Add(x => x.Precision, 2));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("-100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".00");
        }

        [Fact]
        public void ShouldRoundAndCarryFractionalPartToInteger_NegativeDouble()
        {
            var cut = RenderComponent<AntDesign.Statistic<double>>(parameters => parameters
                .Add(x => x.Value, -99.999)
                .Add(x => x.Precision, 2));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("-100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".00");
        }

        [Fact]
        public void ShouldRoundAndCarryFractionalPartToInteger_NegativeFloat()
        {
            var cut = RenderComponent<AntDesign.Statistic<float>>(parameters => parameters
                .Add(x => x.Value, -99.9999f)
                .Add(x => x.Precision, 3));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("-100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".000");
        }

        [Fact]
        public void ShouldRenderNoDecimalPartWhenPrecisionIsZero_Double()
        {
            var cut = RenderComponent<AntDesign.Statistic<double>>(parameters => parameters
                .Add(x => x.Value, 99.5)
                .Add(x => x.Precision, 0));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("100");
            cut.FindAll(".ant-statistic-content-value-decimal").Should().BeEmpty();
        }

        [Fact]
        public void ShouldRenderNoDecimalPartWhenPrecisionIsZero_Float()
        {
            var cut = RenderComponent<AntDesign.Statistic<float>>(parameters => parameters
                .Add(x => x.Value, 99.5f)
                .Add(x => x.Precision, 0));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("100");
            cut.FindAll(".ant-statistic-content-value-decimal").Should().BeEmpty();
        }

        [Fact]
        public void ShouldRenderNoDecimalPartWhenPrecisionIsZero_StringValue()
        {
            var cut = RenderComponent<AntDesign.Statistic<string>>(parameters => parameters
                .Add(x => x.Value, "99.5")
                .Add(x => x.Precision, 0));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("100");
            cut.FindAll(".ant-statistic-content-value-decimal").Should().BeEmpty();
        }

        [Fact]
        public void ShouldRenderRoundedValueWhenDecimalNotAllNines()
        {
            var cut = RenderComponent<AntDesign.Statistic<decimal>>(parameters => parameters
                .Add(x => x.Value, 99.994m)
                .Add(x => x.Precision, 2));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("99");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".99");
        }

        [Fact]
        public void ShouldRenderRoundedUpValueWhenDecimalNotAllNinesAndHalf()
        {
            var cut = RenderComponent<AntDesign.Statistic<decimal>>(parameters => parameters
                .Add(x => x.Value, 99.995m)
                .Add(x => x.Precision, 2));

            cut.Find(".ant-statistic-content-value-int").TextContent.Trim().Should().Be("100");
            cut.Find(".ant-statistic-content-value-decimal").TextContent.Trim().Should().Be(".00");
        }
    }
}
