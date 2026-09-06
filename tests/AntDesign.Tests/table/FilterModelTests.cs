// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using AntDesign.Filters;
using AntDesign.TableModels;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class FilterModelTests
    {
        private sealed record Person(int Id, string Name);

        private sealed class NameFilterModel : FilterModel<string>
        {
            public NameFilterModel(IList<TableFilter> filters, TableFilterType filterType)
                : base(columnIndex: 0, fieldName: nameof(Person.Name), selectedValues: [], filters, filterType)
            {
            }
        }

        [Fact]
        public void Field_filter_combines_values_and_skips_null_conditions()
        {
            var filters = new List<TableFilter>
            {
                new() { FilterCompareOperator = TableFilterCompareOperator.StartsWith, FilterCondition = TableFilterCondition.Or, Value = "a" },
                new() { FilterCompareOperator = TableFilterCompareOperator.EndsWith, FilterCondition = TableFilterCondition.Or, Value = "z" },
                new() { FilterCompareOperator = TableFilterCompareOperator.Equals, FilterCondition = TableFilterCondition.Or, Value = null },
            };
            var model = new NameFilterModel(filters, TableFilterType.FieldType);
            var queryModel = new QueryModel<Person>(1, 10, 0, [], new List<ITableFilterModel> { model });

            var results = queryModel.ExecuteQuery(new[]
            {
                new Person(1, "alice"),
                new Person(2, "bob"),
                new Person(3, "charz"),
                new Person(4, "other"),
            }.AsQueryable()).Select(person => person.Name).ToArray();

            Assert.Equal(new[] { "alice", "charz" }, results);
        }

        [Fact]
        public void Field_filter_combines_conditions_with_and_when_requested()
        {
            var filters = new List<TableFilter>
            {
                new() { FilterCompareOperator = TableFilterCompareOperator.StartsWith, FilterCondition = TableFilterCondition.And, Value = "a" },
                new() { FilterCompareOperator = TableFilterCompareOperator.EndsWith, FilterCondition = TableFilterCondition.And, Value = "e" },
            };
            var model = new NameFilterModel(filters, TableFilterType.FieldType);
            var queryModel = new QueryModel<Person>(1, 10, 0, [], new List<ITableFilterModel> { model });

            var results = queryModel.ExecuteQuery(new[]
            {
                new Person(1, "alice"),
                new Person(2, "amber"),
                new Person(3, "bob"),
            }.AsQueryable()).Select(person => person.Name).ToArray();

            Assert.Equal(new[] { "alice" }, results);
        }

        [Fact]
        public void List_filter_converts_json_element_values_before_filtering()
        {
            var jsonValue = JsonSerializer.SerializeToElement("alice");
            var filters = new List<TableFilter>
            {
                new() { Value = jsonValue, Selected = true }
            };
            var model = new NameFilterModel(filters, TableFilterType.List);
            var queryModel = new QueryModel<Person>(1, 10, 0, [], new List<ITableFilterModel> { model });

            var results = queryModel.ExecuteQuery(new[]
            {
                new Person(1, "alice"),
                new Person(2, "bob"),
            }.AsQueryable()).Select(person => person.Name).ToArray();

            Assert.Equal(new[] { "alice" }, results);
        }

        [Fact]
        public void Query_model_combines_multiple_filter_expressions_with_and()
        {
            var nameFilter = new NameFilterModel(new List<TableFilter>
            {
                new() { Value = "a", Selected = true }
            }, TableFilterType.List);
            var idFilter = new FilterModel<int>(
                columnIndex: 1,
                fieldName: nameof(Person.Id),
                selectedValues: ["1"],
                filters: new List<TableFilter> { new() { Value = 1, Selected = true } },
                filterType: TableFilterType.List);
            var model = new QueryModel<Person>(1, 10, 0, [], new List<ITableFilterModel> { nameFilter, idFilter });
            var expression = model.GetFilterExpression();

            Assert.NotNull(expression);
            Assert.Single(expression.Parameters);
        }

        [Fact]
        public void Empty_filters_return_source_while_list_expression_matches_no_rows()
        {
            var emptyListModel = new NameFilterModel([], TableFilterType.List);
            var emptyFieldModel = new NameFilterModel([], TableFilterType.FieldType);
            var people = new[] { new Person(1, "alice") }.AsQueryable();

            Assert.Same(people, emptyListModel.FilterList(people));
            Assert.Same(people, emptyFieldModel.FilterList(people));
        }
    }
}
