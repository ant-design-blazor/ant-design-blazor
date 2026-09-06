// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class RelationColumnAttributeTests
    {
        private sealed class TestRelationComponent : RelationComponentBase<object, string>
        {
            [Parameter]
            public bool Enabled { get; set; }

            [Parameter]
            public int Size { get; set; }

            [Parameter]
            public string? Prefix { get; set; }

            [Parameter]
            public long? OptionalLong { get; set; }

            [Parameter]
            public double Ratio { get; set; }

            [Parameter]
            public DateTime Timestamp { get; set; }

            protected override RenderFragment RenderContent(string fieldValue, object rowData) => builder => builder.AddContent(0, fieldValue);
        }

        private sealed class InvalidRelationComponent : ComponentBase
        {
        }

        [Fact]
        public void Attribute_validates_component_type()
        {
            Assert.Throws<ArgumentNullException>(() => new RelationColumnAttribute(null!));
            Assert.Throws<ArgumentException>(() => new RelationColumnAttribute(typeof(InvalidRelationComponent)));
        }

        [Fact]
        public void Attribute_creates_cached_render_fragments_and_converts_parameters()
        {
            var attribute = new RelationColumnAttribute(typeof(TestRelationComponent))
            {
                Parameters = new Dictionary<string, object>
                {
                    [nameof(TestRelationComponent.Enabled)] = "true",
                    [nameof(TestRelationComponent.Size)] = "42",
                    [nameof(TestRelationComponent.Prefix)] = "User-",
                    [nameof(TestRelationComponent.OptionalLong)] = "7",
                    [nameof(TestRelationComponent.Ratio)] = "0.5",
                    [nameof(TestRelationComponent.Timestamp)] = "2026-09-06T12:00:00",
                    ["Missing"] = "ignored",
                }
            };

            var first = attribute.CreateRelationComponentContent();
            var second = attribute.CreateRelationComponentContent();

            Assert.Same(first, second);
            first(new RenderTreeBuilder());
            Assert.NotNull(first);
        }

        [Fact]
        public void Attribute_supports_empty_parameter_cache_and_ignores_conversion_failures()
        {
            var emptyAttribute = new RelationColumnAttribute(typeof(TestRelationComponent));
            var emptyFragment = emptyAttribute.CreateRelationComponentContent();
            Assert.NotNull(emptyFragment);
            emptyFragment(new RenderTreeBuilder());

            var invalidAttribute = new RelationColumnAttribute(typeof(TestRelationComponent))
            {
                Parameters = new Dictionary<string, object>
                {
                    [nameof(TestRelationComponent.Enabled)] = new object(),
                    [nameof(TestRelationComponent.Size)] = new object(),
                }
            };

            var invalidFragment = invalidAttribute.CreateRelationComponentContent();
            Assert.NotNull(invalidFragment);
            invalidFragment(new RenderTreeBuilder());
        }
    }
}
