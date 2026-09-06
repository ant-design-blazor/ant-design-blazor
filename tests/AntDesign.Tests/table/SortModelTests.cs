// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using AntDesign.TableModels;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class SortModelTests
    {
        private sealed record Person(string Name, int Age);

        private sealed class NameColumn : Column<string>
        {
            public NameColumn()
            {
                ColIndex = 0;
            }
        }

        [Fact]
        public void Sort_list_supports_ordered_sources_and_custom_comparers()
        {
            var column = new NameColumn();
            var getField = (System.Linq.Expressions.Expression<System.Func<Person, string>>)(person => person.Name);
            var lengthComparer = (string left, string right) => left.Length.CompareTo(right.Length);
            var sortModel = new SortModel<string>(column, getField, nameof(Person.Name), 0, SortDirection.Ascending, lengthComparer);
            var people = new[]
            {
                new Person("aaa", 30),
                new Person("ab", 20),
                new Person("a", 10),
            }.AsQueryable();

            var first = ((ITableSortModel)sortModel).SortList(people.OrderBy(person => person.Age)).Select(person => person.Name).ToArray();
            Assert.Equal(new[] { "a", "ab", "aaa" }, first);

            ((ITableSortModel)sortModel).SetSortDirection(SortDirection.Descending);
            Assert.Equal(SortDirection.Descending, sortModel.SortDirection);
            var second = ((ITableSortModel)sortModel).SortList(people.OrderBy(person => person.Age)).Select(person => person.Name).ToArray();
            Assert.Equal(3, second.Length);
        }

        [Fact]
        public void Sort_list_ignores_none_and_clone_preserves_expression_and_comparer()
        {
            var column = new NameColumn();
            var getField = (System.Linq.Expressions.Expression<System.Func<Person, string>>)(person => person.Name);
            var sortModel = new SortModel<string>(column, getField, nameof(Person.Name), 0, SortDirection.None, (left, right) => string.CompareOrdinal(right, left));
            var people = new[] { new Person("B", 2), new Person("A", 1) }.AsQueryable();

            Assert.Same(people, ((ITableSortModel)sortModel).SortList(people));

            var clone = Assert.IsType<SortModel<string>>(sortModel.Clone());
            var sorted = ((ITableSortModel)clone).SortList(people).Select(person => person.Name).ToArray();
            Assert.Equal(new[] { "B", "A" }, sorted);
            Assert.Equal(nameof(Person.Name), clone.FieldName);
        }

        [Fact]
        public void Sort_model_rejects_missing_field_names_when_expression_is_not_initialized()
        {
            var sortModel = new SortModel<string>(columnIndex: 0, priority: 0, fieldName: string.Empty, SortDirection.Ascending);

            var exception = Assert.Throws<InvalidOperationException>(() => ((ITableSortModel)sortModel).BuildGetFieldExpression<Person>());
            Assert.Contains("FieldName must be set", exception.Message);
        }
    }
}
