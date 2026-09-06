// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class RelationComponentBaseTests
    {
        private sealed record Order(int Id, string Code);

        private sealed class OrderRelation : RelationComponentBase<Order, string>
        {
            public IList<string> LoadedValues { get; } = new List<string>();
            public QueryModel<Order>? LoadedQueryModel { get; private set; }
            public bool RenderedTyped { get; private set; }
            public bool RenderedWrapped { get; private set; }

            protected override Task OnLoadBatch(IEnumerable<string> fieldValues)
            {
                foreach (var value in fieldValues)
                {
                    LoadedValues.Add(value);
                }

                return Task.CompletedTask;
            }

            protected override async Task OnLoadBatch(IEnumerable<Order> dataSource, QueryModel<Order> queryModel)
            {
                LoadedQueryModel = queryModel;
                await base.OnLoadBatch(dataSource, queryModel);
            }

            protected override RenderFragment RenderContent(string fieldValue, Order rowData)
            {
                RenderedTyped = true;
                return builder => builder.AddContent(0, fieldValue);
            }

            public RenderFragment RenderWrappedCell(RowData rowData)
            {
                RenderedWrapped = true;
                return ((IRelationComponent)this).RenderCell(rowData);
            }
        }

        [Fact]
        public async Task Batch_loading_collects_distinct_field_values_and_query_model()
        {
            var relation = new OrderRelation();
            var queryModel = new QueryModel<Order>(1, 10, 0, [], []);
            var orders = new[]
            {
                new Order(1, "A"),
                new Order(2, "B"),
                new Order(3, "A"),
            };

            ((IRelationComponent<Order>)relation).SetDataSource(orders, queryModel);
            await ((IRelationComponent<Order>)relation).OnLoadBatchAsync();

            Assert.Same(queryModel, relation.LoadedQueryModel);
            var loadedValue = Assert.Single(relation.LoadedValues);
            Assert.Null(loadedValue);
        }

        [Fact]
        public void Non_generic_render_path_uses_row_data_and_typed_render_fragment()
        {
            var relation = new OrderRelation();
            var order = new Order(1, "A");
            var rowData = new RowData<Order>(new TableDataItem<Order>(order, null!));
            var fragment = relation.RenderWrappedCell(rowData);

            Assert.NotNull(fragment);
            Assert.True(relation.RenderedWrapped);
            Assert.True(relation.RenderedTyped);
        }

        [Fact]
        public async Task Batch_loading_without_cached_data_source_is_noop()
        {
            var relation = new OrderRelation();

            await ((IRelationComponent<Order>)relation).OnLoadBatchAsync();

            Assert.Empty(relation.LoadedValues);
            Assert.Null(relation.LoadedQueryModel);
        }
    }
}
