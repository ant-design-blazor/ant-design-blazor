// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Bunit;
using Xunit;

#pragma warning disable CS0612, CS0618

namespace AntDesign.Tests.Table
{
    public class TableTests : AntDesignTestBase
    {
        private sealed class Person
        {
            public int Id { get; set; }
            public string Name { get; set; } = default!;
            public string Surname { get; set; } = default!;
        }

        private IRenderedComponent<Table<Person>> CreatePersonsTable(
            IReadOnlyList<Person> persons,
            Action<ComponentParameterCollectionBuilder<Table<Person>>>? callback = null,
            bool enableSelection = false)
        {
            return Context.RenderComponent<Table<Person>>(x =>
            {
                x
                    .Add(b => b.DataSource, persons)
                    .Add(b => b.ChildContent, p =>
                    {
                        var selection = new ComponentParameterCollectionBuilder<Selection>()
                                .Add(q => q.Key, p.Id.ToString())
                                .Build()
                                .ToRenderFragment<Selection>();

                        var nameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                                .Add(q => q.Field, p.Name)
                                .Build()
                                .ToRenderFragment<Column<string>>();

                        var surnameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                                .Add(q => q.Field, p.Surname)
                                .Build()
                                .ToRenderFragment<Column<string>>();

                        return builder =>
                        {
                            if (enableSelection) selection(builder);
                            nameCol(builder);
                            surnameCol(builder);
                        };
                    }
                    );

                callback?.Invoke(x);
            }
            );
        }

        [Fact]
        public void Renders_an_empty_table()
        {
            var persons = Array.Empty<Person>();

            var cut = CreatePersonsTable(persons);

            Assert.Single(cut.FindAll("tbody tr.ant-table-placeholder"));
            Assert.Contains("No Data", cut.Find("tbody").TextContent);
        }

        [Fact]
        public void Renders_a_table_with_two_rows()
        {
            var persons = new[]
            {
                new Person {Id = 1, Name = "John", Surname = "Smith"},
                new Person {Id = 2, Name = "Jane", Surname = "Doe"}
            };

            var cut = CreatePersonsTable(persons);

            var rows = cut.FindAll("tbody tr.ant-table-row");
            Assert.Equal(2, rows.Count);
            Assert.Contains("John", rows[0].TextContent);
            Assert.Contains("Jane", rows[1].TextContent);
        }

        [Fact]
        public void Can_render_after_changes_to_the_dataSource()
        {
            var persons = new List<Person>
            {
                new Person {Id = 1, Name = "John", Surname = "Smith"},
                new Person {Id = 2, Name = "Jane", Surname = "Doe"},
                new Person {Id = 3, Name = "Joe", Surname = "Doe"}
            };

            var cut = CreatePersonsTable(persons, b => b
                .Add(q => q.PageSize, 1)
                .Add(q => q.PageIndex, 3)
            );

            persons.RemoveAt(0);

            cut.SetParametersAndRender(b => b
                .Add(q => q.DataSource, persons)
                .Add(q => q.PageIndex, 2));

            var row = Assert.Single(cut.FindAll("tbody tr.ant-table-row"));
            Assert.Contains("Joe", row.TextContent);
        }

        [Fact]
        public void Set_colspan_and_rowspan()
        {
            var persons = new[]
             {
                new Person {Id = 1, Name = "John", Surname = "Smith"},
                new Person {Id = 2, Name = "Jane", Surname = "Doe"}
            };

            var cut = Context.RenderComponent<Table<Person>>(x =>
            {
                x.Add(b => b.DataSource, persons)
                .Add(b => b.ChildContent, p =>
                {
                    var selection = new ComponentParameterCollectionBuilder<Selection>()
                        .Add(q => q.Key, p.Id.ToString())
                        .Add(q => q.HeaderColSpan, 2)
                        .Add(q => q.ColSpan, 1)
                        .Add(q => q.RowSpan, 2)
                        .Build()
                        .ToRenderFragment<Selection>();

                    var nameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, p.Name)
                        .Add(q => q.HeaderColSpan, 0)
                        .Add(q => q.ColSpan, 2)
                        .Add(q => q.RowSpan, 1)
                        .Build()
                        .ToRenderFragment<Column<string>>();

                    var surnameCol = new ComponentParameterCollectionBuilder<Column<string>>()
                        .Add(q => q.Field, p.Surname)
                        .Add(q => q.HeaderColSpan, 1)
                        .Add(q => q.ColSpan, 0)
                        .Add(q => q.RowSpan, 0)
                        .Build()
                        .ToRenderFragment<Column<string>>();

                    return builder =>
                    {
                        nameCol(builder);
                        surnameCol(builder);
                    };
                });
            });

            Assert.Single(cut.FindAll("thead th"));
            var cells = cut.FindAll("tbody td");
            Assert.Equal(2, cells.Count);
            Assert.All(cells, cell => Assert.Equal("2", cell.GetAttribute("colspan")));
        }
    }
}
