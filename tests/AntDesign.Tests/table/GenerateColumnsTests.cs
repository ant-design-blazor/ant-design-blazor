// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Linq;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class GenerateColumnsTests : AntDesignTestBase
    {
        private sealed class GeneratedModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public bool Hidden { get; set; }
        }

        [Fact]
        public void Generates_columns_for_properties_and_applies_definitions()
        {
            var definitions = new ConcurrentQueue<(string PropertyName, IFieldColumn Column)>();
            var cut = Context.RenderComponent<Table<GeneratedModel>>(parameters => parameters
                .Add(table => table.DataSource, new[] { new GeneratedModel { Id = 1, Name = "Alice" } })
                .Add(table => table.ChildContent, _ => builder =>
                {
                    builder.AddContent(0, new ComponentParameterCollectionBuilder<GenerateColumns<GeneratedModel>>()
                        .Add(component => component.Range, new System.Range(0, 2))
                        .Add(component => component.HideColumnsByName, new[] { nameof(GeneratedModel.Hidden) })
                        .Add(component => component.Definitions, (propertyName, column) => definitions.Enqueue((propertyName, column)))
                        .Add(component => component.StartColumnIndex, 3)
                        .Build()
                        .ToRenderFragment<GenerateColumns<GeneratedModel>>());
                }));

            var definedColumns = definitions.GroupBy(definition => definition.PropertyName).ToArray();
            Assert.Contains(definedColumns, group => group.Key == nameof(GeneratedModel.Id) && group.First().Column.ColIndex == 3);
            Assert.Contains(definedColumns, group => group.Key == nameof(GeneratedModel.Name) && group.First().Column.ColIndex == 4);
            Assert.DoesNotContain(definedColumns, group => group.Key == nameof(GeneratedModel.Hidden));
            Assert.Contains("Id", cut.Find("thead").TextContent);
            Assert.Contains("Name", cut.Find("thead").TextContent);
            Assert.DoesNotContain("Hidden", cut.Find("thead").TextContent);
        }
    }
}
