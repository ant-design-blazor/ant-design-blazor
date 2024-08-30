// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Icon
{
    public class IconServiceTests : AntDesignTestBase
    {
        [Theory]
        [MemberData(nameof(IconTestData))]
        public void AllIconsShouldHaveSameWrappingSvgCode(string iconName, string type)
        {
            RenderFragment fragment = builder =>
            {
                builder.AddMarkupContent(0, IconService.GetIconImg(iconName, type));
            };

            var renderedComponent = Render(fragment);
            renderedComponent.MarkupMatches(@"<svg 
                xmlns=""http://www.w3.org/2000/svg"" 
                class=""icon"" 
                viewBox=""0 0 1024 1024"" 
                diff:ignoreChildren></svg>");
        }

        [Theory]
        [MemberData(nameof(IconTestData))]
        public void AllIconsShouldContainAtLeastOnePath(string iconName, string type)
        {
            RenderFragment fragment = builder =>
            {
                builder.AddMarkupContent(0, IconService.GetIconImg(iconName, type));
            };

            var renderedComponent = Render(fragment);
            renderedComponent.FindAll("path").Count().Should().BeGreaterOrEqualTo(1);
        }

        [Theory]
        [MemberData(nameof(InvalidIconTestData))]
        public void InvalidIconNameOrTypeShouldReturnNull(string iconName, string type)
        {
            IconService.GetIconImg(iconName, type).Should().BeNull();
        }

        [Fact]
        public void GetAllIconsShouldReturnDictionaryKeyedByThemeType()
        {
            IconService.GetAllIcons().Keys.Should().BeEquivalentTo(new[]
            {
                IconThemeType.Outline,
                IconThemeType.Fill,
                IconThemeType.Twotone
            });
        }

        [Theory]
        [InlineData(IconThemeType.Outline, "alert")]
        [InlineData(IconThemeType.Fill, "alert")]
        [InlineData(IconThemeType.Twotone, "alert")]
        public void GetAllIconsShouldReturnDictionaryValuedWithIconNames(string themeType, string iconName)
        {
            IconService.GetAllIcons()[themeType].Should().Contain(iconName);
        }

        [Theory]
        [InlineData("bad-type", "alert", false)]
        [InlineData("bad-type", "bad-icon", false)]
        [InlineData(IconThemeType.Twotone, "bad-icon", false)]
        [InlineData(IconThemeType.Twotone, "alert", true)]
        public void IconExistsShouldReturnProperValue(string themeType, string iconName, bool exists)
        {
            IconService.IconExists(themeType, iconName).Should().Be(exists);
        }

        public static IEnumerable<object[]> IconTestData()
        {
            foreach (var (type, names) in IconStore.AllIconsByTheme.Value)
            {
                foreach (var singleIconName in names)
                {
                    yield return new object[] { singleIconName, type };
                }
            }
        }

        public static IEnumerable<object[]> InvalidIconTestData()
        {
            // Bad icon, correct type
            yield return new object[] { "bad-icon-name", IconThemeType.Twotone };

            // Correct icon, bad type
            yield return new object[] { "alert", "bad-type-name" };

            // Bad icon, bad type
            yield return new object[] { "bad-icon-name", "bad-type-name" };
        }
    }
}
