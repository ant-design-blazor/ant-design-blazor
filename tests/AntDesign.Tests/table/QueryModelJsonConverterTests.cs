// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AntDesign.Filters;
using AntDesign.TableModels;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class QueryModelJsonConverterTests
    {
        private sealed record Person(int Id, string Name, int Age);

        [Fact]
        public void QueryModel_round_trip_restores_sorts_filters_and_paging()
        {
            var sortModel = new SortModel<string>(columnIndex: 0, priority: 1, fieldName: nameof(Person.Name), SortDirection.Descending);
            var filterModel = new FilterModel<string>(
                columnIndex: 1,
                fieldName: nameof(Person.Name),
                selectedValues: new[] { "Alice" },
                filters: new List<TableFilter> { new() { Text = "Alice", Value = "Alice", Selected = true } },
                filterType: TableFilterType.List);
            var model = new QueryModel<Person>(2, 10, 10, new List<ITableSortModel> { sortModel }, new List<ITableFilterModel> { filterModel });

            var json = JsonSerializer.Serialize<QueryModel>(model);
            var restored = Assert.IsType<QueryModel<Person>>(JsonSerializer.Deserialize<QueryModel<Person>>(json));

            Assert.Equal(2, restored.PageIndex);
            Assert.Equal(10, restored.PageSize);
            Assert.Equal(10, restored.StartIndex);
            var restoredSort = Assert.Single(restored.SortModel);
            Assert.Equal(nameof(Person.Name), restoredSort.FieldName);
            Assert.Equal(SortDirection.Descending, restoredSort.SortDirection);
            var restoredFilter = Assert.Single(restored.FilterModel);
            Assert.Equal(nameof(Person.Name), restoredFilter.FieldName);

            var people = new[]
            {
                new Person(1, "Bob", 20),
                new Person(2, "Alice", 30),
                new Person(3, "Charlie", 40),
            }.AsQueryable();
            Assert.Equal(new[] { "Alice" }, restored.ExecuteQuery(people).Select(person => person.Name));
        }

        [Fact]
        public void QueryModel_deserializer_supports_multiple_sorts_and_empty_filters()
        {
            const string json = """
            {
              "pageIndex": 1,
              "pageSize": 2,
              "startIndex": 0,
              "sortModel": [
                { "$type": "AntDesign.TableModels.SortModel`1[[System.String, System.Private.CoreLib]], AntDesign", "columnIndex": 0, "priority": 0, "FieldName": "Name", "sortDirection": "Ascending" },
                { "$type": "AntDesign.TableModels.SortModel`1[[System.Int32, System.Private.CoreLib]], AntDesign", "columnIndex": 1, "priority": 1, "FieldName": "Age", "sortDirection": "Descending" }
              ],
              "filterModel": []
            }
            """;

            var restored = Assert.IsType<QueryModel<Person>>(JsonSerializer.Deserialize<QueryModel<Person>>(json));

            Assert.Equal(2, restored.SortModel.Count);
            Assert.Empty(restored.FilterModel);
            var people = new[]
            {
                new Person(1, "Bob", 30),
                new Person(2, "Alice", 20),
            }.AsQueryable();
            var result = Assert.IsAssignableFrom<IEnumerable<Person>>(restored.ExecuteQuery(people)).ToArray();
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public void QueryModel_clone_copies_sorts_and_preserves_paging()
        {
            var sortModel = new SortModel<string>(columnIndex: 0, priority: 1, fieldName: nameof(Person.Name), SortDirection.Ascending);
            var model = new QueryModel<Person>(3, 2, 4, new List<ITableSortModel> { sortModel }, []);

            var clone = Assert.IsType<QueryModel<Person>>(model.Clone());

            Assert.Equal(3, clone.PageIndex);
            Assert.Equal(2, clone.PageSize);
            Assert.Equal(4, clone.StartIndex);
            var clonedSort = Assert.Single(clone.SortModel);
            Assert.NotSame(sortModel, clonedSort);
            Assert.Equal(nameof(Person.Name), clonedSort.FieldName);
            Assert.Equal(SortDirection.Ascending, clonedSort.SortDirection);
        }

        [Fact]
        public void QueryModel_without_filters_matches_every_record_and_pages_from_start_index()
        {
            var model = new QueryModel<Person>(3, 2, 4, [], []);
            var predicate = model.GetFilterExpression().Compile();

            Assert.True(predicate(new Person(1, "Alice", 20)));

            var paged = model.CurrentPagedRecords(Enumerable.Range(1, 8).Select(id => new Person(id, $"Person {id}", id)).AsQueryable());
            Assert.Equal(new[] { 5, 6 }, paged.Select(person => person.Id));
        }

        [Fact]
        public void QueryModel_deserializer_rejects_non_object_payloads()
        {
            Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<QueryModel<Person>>("[]"));
        }
    }
}
