// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using AntDesign.JsInterop;
using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

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

            cut.RecordedMarkupMatches();
        }

        [Fact]
        public void Cascaded_pagination_props_override_the_table_internal_pagination()
        {
            var props = new PaginationProps
            {
                ShowQuickJumper = true,
                ShowSizeChanger = true,
            };

            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new DomRect());

            var cut = Context.RenderComponent<CascadingValue<PaginationProps>>(parameters => parameters
                .Add(p => p.Value, props)
                .Add(p => p.ChildContent, builder =>
                {
                    builder.OpenComponent<Table<Person>>(0);
                    builder.AddAttribute(1, nameof(Table<Person>.DataSource), new[]
                    {
                        new Person { Id = 1, Name = "John", Surname = "Smith" },
                        new Person { Id = 2, Name = "Jane", Surname = "Doe" },
                    });
                    builder.AddAttribute(2, nameof(Table<Person>.PageSize), 20);
                    builder.AddAttribute(3, nameof(Table<Person>.ChildContent), (RenderFragment<Person>)(_ => builder => { }));
                    builder.CloseComponent();
                }));

            var pagination = cut.FindComponent<Pagination>().Instance;
            Assert.Same(props, pagination.CascadingPaginationProps);
            Assert.True(pagination.ShowQuickJumper);
            Assert.True(pagination.ShowSizeChanger);
            Assert.Equal(20, pagination.PageSize);
        }

        [Fact]
        public void Cascaded_props_collection_targets_each_table_pagination_by_key()
        {
            var props = new ComponentPropsCollection(new PaginationProps { ShowQuickJumper = true })
            {
                [Table<Person>.BottomPaginationId] = new PaginationProps { ShowSizeChanger = true },
            };

            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new DomRect());

            var cut = Context.RenderComponent<CascadingValue<ComponentPropsCollection>>(parameters => parameters
                .Add(p => p.Value, props)
                .Add(p => p.ChildContent, builder =>
                {
                    builder.OpenComponent<Table<Person>>(0);
                    builder.AddAttribute(1, nameof(Table<Person>.DataSource), new[] { new Person { Id = 1 } });
                    builder.AddAttribute(2, nameof(Table<Person>.PaginationPosition), "topRight,bottomRight");
                    builder.AddAttribute(3, nameof(Table<Person>.ChildContent), (RenderFragment<Person>)(_ => builder => { }));
                    builder.CloseComponent();
                }));

            var paginations = cut.FindComponents<Pagination>();
            Assert.Equal(2, paginations.Count);
            Assert.True(paginations[0].Instance.ShowQuickJumper);
            Assert.False(paginations[0].Instance.ShowSizeChanger);
            Assert.True(paginations[1].Instance.ShowQuickJumper);
            Assert.True(paginations[1].Instance.ShowSizeChanger);
        }

        [Fact]
        public void Component_props_provider_applies_global_and_id_specific_props()
        {
            JSInterop.Setup<DomRect>(JSInteropConstants.GetBoundingClientRect, _ => true)
                .SetResult(new DomRect());

            var cut = Context.RenderComponent<ComponentPropsProvider>(parameters => parameters
                .Add(p => p.Props, new ComponentPropsCollection(new PaginationProps { ShowQuickJumper = true }))
                .Add(p => p.ChildContent, builder =>
                {
                    builder.OpenComponent<ComponentPropsProvider>(0);
                    builder.AddAttribute(1, nameof(ComponentPropsProvider.Props), new ComponentPropsCollection { [Table<Person>.BottomPaginationId] = new PaginationProps { ShowSizeChanger = true } });
                    builder.AddAttribute(2, nameof(ComponentPropsProvider.ChildContent), (RenderFragment)(childBuilder =>
                    {
                        childBuilder.OpenComponent<Table<Person>>(0);
                        childBuilder.AddAttribute(1, nameof(Table<Person>.DataSource), new[] { new Person { Id = 1 } });
                        childBuilder.AddAttribute(2, nameof(Table<Person>.PaginationPosition), "topRight,bottomRight");
                        childBuilder.AddAttribute(3, nameof(Table<Person>.ChildContent), (RenderFragment<Person>)(_ => builder => { }));
                        childBuilder.CloseComponent();
                    }));
                    builder.CloseComponent();
                }));

            var paginations = cut.FindComponents<Pagination>();
            Assert.True(paginations[0].Instance.ShowQuickJumper);
            Assert.False(paginations[0].Instance.ShowSizeChanger);
            Assert.True(paginations[1].Instance.ShowQuickJumper);
            Assert.True(paginations[1].Instance.ShowSizeChanger);
        }

    }
}
