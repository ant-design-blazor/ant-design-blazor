using System;
using System.Collections.Generic;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace AntDesign.Tests.Table
{
    public class TableTests : AntDesignTestBase
    {
        private class Person
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
        }

        private IRenderedComponent<Table<Person>> CreatePersonsTable(
            IReadOnlyList<Person> persons,
            Action<ComponentParameterBuilder<Table<Person>>> callback = null,
            bool enableSelection = false)
        {
            return Context.RenderComponent<Table<Person>>(x =>
                {
                    x
                        .Add(b => b.DataSource, persons)
                        .Add(b => b.ChildContent, p =>
                            {
                                var selection = new ComponentParameterBuilder<Selection>()
                                    .Add(q => q.Key, p.Id.ToString())
                                    .Build()
                                    .ToComponentRenderFragment<Selection>();

                                var nameCol = new ComponentParameterBuilder<Column<string>>()
                                    .Add(q => q.Field, p.Name)
                                    .Build()
                                    .ToComponentRenderFragment<Column<string>>();

                                var surnameCol = new ComponentParameterBuilder<Column<string>>()
                                    .Add(q => q.Field, p.Surname)
                                    .Build()
                                    .ToComponentRenderFragment<Column<string>>();

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

        //[Fact]
        public void Renders_an_empty_table()
        {
            var persons = Array.Empty<Person>();

            var cut = CreatePersonsTable(persons);

            cut.RecordedMarkupMatches();
        }

        //[Fact]
        public void Renders_a_table_with_two_rows()
        {
            var persons = new[]
            {
                new Person {Id = 1, Name = "John", Surname = "Smith"},
                new Person {Id = 2, Name = "Jane", Surname = "Doe"}
            };

            var cut = CreatePersonsTable(persons);

            cut.RecordedMarkupMatches();
        }

        //[Fact]
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

            cut.SetParametersAndRender(b => b.Add(q => q.DataSource, persons));

            cut.RecordedMarkupMatches();
        }

        //[Fact]
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
                    var selection = new ComponentParameterBuilder<Selection>()
                        .Add(q => q.Key, p.Id.ToString())
                        .Add(q => q.HeaderColSpan, 2)
                        .Add(q => q.ColSpan, 1)
                        .Add(q => q.RowSpan, 2)
                        .Build()
                        .ToComponentRenderFragment<Selection>();

                    var nameCol = new ComponentParameterBuilder<Column<string>>()
                        .Add(q => q.Field, p.Name)
                        .Add(q => q.HeaderColSpan, 0)
                        .Add(q => q.ColSpan, 2)
                        .Add(q => q.RowSpan, 1)
                        .Build()
                        .ToComponentRenderFragment<Column<string>>();

                    var surnameCol = new ComponentParameterBuilder<Column<string>>()
                        .Add(q => q.Field, p.Surname)
                        .Add(q => q.HeaderColSpan, 1)
                        .Add(q => q.ColSpan, 0)
                        .Add(q => q.RowSpan, 0)
                        .Build()
                        .ToComponentRenderFragment<Column<string>>();

                    return builder =>
                    {
                        nameCol(builder);
                        surnameCol(builder);
                    };
                });
            });

            cut.RecordedMarkupMatches();
        }
    }
}
