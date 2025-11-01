// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.TableModels;
using Bunit;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class QueryModelSerializationTests : AntDesignTestBase
    {
        [Fact]
        public void QueryModel_Should_Serialize_And_Deserialize_Correctly()
        {
            // Arrange
            var sortModels = new List<ITableSortModel>
            {
                new SortModel<string>(columnIndex: 0, priority: 0, fieldName: "Name", sortDirection: SortDirection.Ascending),
                new SortModel<int>(columnIndex: 1, priority: 1, fieldName: "Age", sortDirection: SortDirection.Descending)
            };

            var filterModels = new List<ITableFilterModel>
            {
                new FilterModel<string>(
                    columnIndex: 2,
                    fieldName: "Category",
                    selectedValues: new[] { "A", "B" },
                    filters: null,
                    filterType: TableFilterType.List)
            };

            var original = new QueryModel<TestItem>(
                pageIndex: 1,
                pageSize: 10,
                startIndex: 0,
                sortModel: sortModels,
                filterModel: filterModels);

            // Act
            var json = JsonSerializer.Serialize(original);
            var deserialized = JsonSerializer.Deserialize<QueryModel<TestItem>>(json);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(original.PageIndex, deserialized.PageIndex);
            Assert.Equal(original.PageSize, deserialized.PageSize);
            
            Assert.Equal(2, deserialized.SortModel.Count);
            Assert.Equal("Name", deserialized.SortModel[0].FieldName);
            Assert.Equal(SortDirection.Ascending, deserialized.SortModel[0].SortDirection);
            Assert.Equal("Age", deserialized.SortModel[1].FieldName);
            Assert.Equal(SortDirection.Descending, deserialized.SortModel[1].SortDirection);
            
            // Verify generic types are preserved
            Assert.IsType<SortModel<string>>(deserialized.SortModel[0]);
            Assert.IsType<SortModel<int>>(deserialized.SortModel[1]);
            
            Assert.Single(deserialized.FilterModel);
            Assert.Equal("Category", deserialized.FilterModel[0].FieldName);
            Assert.Equal(new[] { "A", "B" }, deserialized.FilterModel[0].SelectedValues);
            
            // Verify generic type is preserved
            Assert.IsType<FilterModel<string>>(deserialized.FilterModel[0]);
        }

        [Fact]
        public void QueryModel_Empty_Collections_Should_Serialize_Correctly()
        {
            // Arrange
            var original = new QueryModel<TestItem>(
                pageIndex: 1,
                pageSize: 20,
                startIndex: 0,
                sortModel: new List<ITableSortModel>(),
                filterModel: new List<ITableFilterModel>());

            // Act
            var json = JsonSerializer.Serialize(original);
            var deserialized = JsonSerializer.Deserialize<QueryModel<TestItem>>(json);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(original.PageIndex, deserialized.PageIndex);
            Assert.Equal(original.PageSize, deserialized.PageSize);
            Assert.Empty(deserialized.SortModel);
            Assert.Empty(deserialized.FilterModel);
        }

        [Fact]
        public void QueryModel_Should_Preserve_Generic_Types_DateTime()
        {
            // Arrange - Test with DateTime type
            var sortModels = new List<ITableSortModel>
            {
                new SortModel<DateTime>(columnIndex: 0, priority: 0, fieldName: "CreatedDate", sortDirection: SortDirection.Descending)
            };

            var filterModels = new List<ITableFilterModel>
            {
                new FilterModel<DateTime>(
                    columnIndex: 1,
                    fieldName: "ModifiedDate",
                    selectedValues: new[] { "2024-01-01", "2024-12-31" },
                    filters: null,
                    filterType: TableFilterType.List)
            };

            var original = new QueryModel<TestItem>(
                pageIndex: 1,
                pageSize: 10,
                startIndex: 0,
                sortModel: sortModels,
                filterModel: filterModels);

            // Act
            var json = JsonSerializer.Serialize(original);
            var deserialized = JsonSerializer.Deserialize<QueryModel<TestItem>>(json);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Single(deserialized.SortModel);
            Assert.IsType<SortModel<DateTime>>(deserialized.SortModel[0]);
            
            Assert.Single(deserialized.FilterModel);
            Assert.IsType<FilterModel<DateTime>>(deserialized.FilterModel[0]);
        }

        [Fact]
        public void QueryModel_Should_Preserve_Generic_Types_Mixed()
        {
            // Arrange - Test with multiple different generic types
            var sortModels = new List<ITableSortModel>
            {
                new SortModel<string>(columnIndex: 0, priority: 0, fieldName: "Name", sortDirection: SortDirection.Ascending),
                new SortModel<int>(columnIndex: 1, priority: 1, fieldName: "Age", sortDirection: SortDirection.Descending),
                new SortModel<decimal>(columnIndex: 2, priority: 2, fieldName: "Salary", sortDirection: SortDirection.Ascending),
                new SortModel<bool>(columnIndex: 3, priority: 3, fieldName: "IsActive", sortDirection: SortDirection.Descending)
            };

            var filterModels = new List<ITableFilterModel>
            {
                new FilterModel<string>(
                    columnIndex: 0,
                    fieldName: "Category",
                    selectedValues: new[] { "A", "B", "C" },
                    filters: null,
                    filterType: TableFilterType.List),
                new FilterModel<int>(
                    columnIndex: 1,
                    fieldName: "Level",
                    selectedValues: new[] { "1", "2", "3" },
                    filters: null,
                    filterType: TableFilterType.List)
            };

            var original = new QueryModel<TestItem>(
                pageIndex: 2,
                pageSize: 25,
                startIndex: 0,
                sortModel: sortModels,
                filterModel: filterModels);

            // Act
            var json = JsonSerializer.Serialize(original);
            var deserialized = JsonSerializer.Deserialize<QueryModel<TestItem>>(json);

            // Assert
            Assert.NotNull(deserialized);
            Assert.Equal(4, deserialized.SortModel.Count);
            Assert.IsType<SortModel<string>>(deserialized.SortModel[0]);
            Assert.IsType<SortModel<int>>(deserialized.SortModel[1]);
            Assert.IsType<SortModel<decimal>>(deserialized.SortModel[2]);
            Assert.IsType<SortModel<bool>>(deserialized.SortModel[3]);
            
            Assert.Equal(2, deserialized.FilterModel.Count);
            Assert.IsType<FilterModel<string>>(deserialized.FilterModel[0]);
            Assert.IsType<FilterModel<int>>(deserialized.FilterModel[1]);
        }

        [Fact]
        public async Task Table_Should_Restore_State_From_Serialized_QueryModel()
        {
            // Arrange: Create a table with some columns
            var dataSource = new List<TestItem>
            {
                new TestItem { Name = "Alice", Age = 30, Category = "A" },
                new TestItem { Name = "Bob", Age = 25, Category = "B" },
                new TestItem { Name = "Charlie", Age = 35, Category = "A" },
                new TestItem { Name = "David", Age = 28, Category = "C" }
            };

            var cut = Context.RenderComponent<AntDesign.Table<TestItem>>(parameters => parameters
                .Add(p => p.DataSource, dataSource)
                .Add(p => p.ChildContent, (TestItem context) =>
                {
                    var nameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Title, "Name")
                        .Add(q => q.DataIndex, "Name")
                        .Add(q => q.Sortable, true)
                        .Build()
                        .ToRenderFragment<Column<string>>();

                    var ageCol = new ComponentParameterCollectionBuilder<Column<int>>()
                        .Add(q => q.Title, "Age")
                        .Add(q => q.DataIndex, "Age")
                        .Add(q => q.Sortable, true)
                        .Build()
                        .ToRenderFragment<Column<int>>();

                    return builder =>
                    {
                        nameCol(builder);
                        ageCol(builder);
                    };
                })
            );

            var table = cut.Instance;

            // Create a QueryModel with pagination and sorting
            var queryModel = new QueryModel<TestItem>(
                pageIndex: 2,
                pageSize: 2,
                startIndex: 0,
                sortModel: new List<ITableSortModel>
                {
                    new SortModel<string>(columnIndex: 0, priority: 0, fieldName: "Name", sortDirection: SortDirection.Descending),
                    new SortModel<int>(columnIndex: 1, priority: 1, fieldName: "Age", sortDirection: SortDirection.Ascending)
                },
                filterModel: new List<ITableFilterModel>()
            );

            // Act: Serialize and deserialize
            var json = JsonSerializer.Serialize(queryModel);
            var deserialized = JsonSerializer.Deserialize<QueryModel<TestItem>>(json);

            // Call ReloadData to restore table state (must be done on dispatcher thread)
            await cut.InvokeAsync(() => table.ReloadData(deserialized));

            // Get the current state after reload
            var currentQueryModel = table.GetQueryModel();

            // Assert: Verify pagination was restored
            Assert.Equal(2, currentQueryModel.PageIndex);
            Assert.Equal(2, currentQueryModel.PageSize);

            // Assert: Verify sorting was restored with correct types
            Assert.Equal(2, currentQueryModel.SortModel.Count);

            // Verify Name sort (string type)
            var nameSort = currentQueryModel.SortModel[0];
            Assert.IsType<SortModel<string>>(nameSort);
            Assert.Equal("Name", nameSort.FieldName);
            Assert.Equal(SortDirection.Descending, nameSort.SortDirection);

            // Verify Age sort (int type)
            var ageSort = currentQueryModel.SortModel[1];
            Assert.IsType<SortModel<int>>(ageSort);
            Assert.Equal("Age", ageSort.FieldName);
            Assert.Equal(SortDirection.Ascending, ageSort.SortDirection);
        }

        private class TestItem
        {
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public string Category { get; set; } = "";
            public DateTime ModifiedDate { get; set; }
            public DateTime CreatedDate { get; set; }
            public decimal Level { get; set; }
            public decimal Salary { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
