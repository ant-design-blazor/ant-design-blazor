using Bunit;
using Xunit;

namespace AntDesign.Tests.Grid
{
    public class ColTests : AntDesignTestBase
    {
        [Fact]
        public void Render_with_defaults()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col"" style="" "" id:ignore>Contents</div>
            ");
        }

        [Fact]
        public void Render_with_all_options()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Flex, 1);
                p.Add(x => x.Span, 2);
                p.Add(x => x.Order, 3);
                p.Add(x => x.Offset, 4);
                p.Add(x => x.Push, 5);
                p.Add(x => x.Pull, 6);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-2 ant-col-order-3 ant-col-offset-4 ant-col-pull-6 ant-col-push-5"" style=""flex: 1 1 auto"" id:ignore>Contents</div>
            ");
        }

        [Fact]
        public void Render_with_column_int_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Span, 2);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-2"" style="" "" id:ignore>Contents</div>
            ");
        }

        [Fact]
        public void Render_with_column_string_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Span, "3");
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-3"" style="" "" id:ignore>Contents</div>
            ");
        }

        /// <remarks>
        /// We only have to test one overlap here because we extensively test the other
        /// properties in other tests in this file.
        /// </remarks>
        [Fact]
        public void Render_with_column_span_and_xs_column_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Span, 5);
                p.Add(x => x.Xs, 6);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-5 ant-col-xs-6"" style="" "" id:ignore>Contents</div>
            ");
        }

        /// <remarks>
        /// Since all the size categories use the same EmbeddedProperty class and calls, we just have to
        /// make sure one properly emits all the variables and that will cover all the other sizes.
        /// </remarks>
        [Fact]
        public void Render_with_all_xs_options()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(
                    x => x.Xs,
                    new AntDesign.EmbeddedProperty
                    {
                        Span = 2,
                        Order = 3,
                        Offset = 4,
                        Push = 5,
                        Pull = 6,
                    });
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-xs-2 ant-col-xs-order-3 ant-col-xs-offset-4 ant-col-xs-push-5 ant-col-xs-pull-6"" style="" "" id:ignore>Contents</div>
            ");
        }

        [Fact]
        public void Render_with_xs_column_int_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Xs, 4);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-xs-4"" style="" "" id:ignore>Contents</div>
            ");
        }

        [Fact]
        public void Render_with_xs_column_embedded_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Xs, new AntDesign.EmbeddedProperty { Span = 3 });
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-xs-3"" style="" "" id:ignore>Contents</div>
            ");
        }

        /// <remarks>
        /// We don't need to do an extensive set of embedded properties with tests because
        /// we handle the XS ones which use the same logic.
        /// </remarks>
        [Fact]
        public void Render_with_sm_column_int_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Sm, 2);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-sm-2"" style="" "" id:ignore>Contents</div>
            ");
        }

        /// <remarks>
        /// We don't need to do an extensive set of embedded properties with tests because
        /// we handle the XS ones which use the same logic.
        /// </remarks>
        [Fact]
        public void Render_with_md_column_int_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Md, 2);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-md-2"" style="" "" id:ignore>Contents</div>
            ");
        }

        /// <remarks>
        /// We don't need to do an extensive set of embedded properties with tests because
        /// we handle the XS ones which use the same logic.
        /// </remarks>
        [Fact]
        public void Render_with_lg_column_int_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Lg, 2);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-lg-2"" style="" "" id:ignore>Contents</div>
            ");
        }

        /// <remarks>
        /// We don't need to do an extensive set of embedded properties with tests because
        /// we handle the XS ones which use the same logic.
        /// </remarks>
        [Fact]
        public void Render_with_xl_column_int_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Xl, 2);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-xl-2"" style="" "" id:ignore>Contents</div>
            ");
        }

        /// <remarks>
        /// We don't need to do an extensive set of embedded properties with tests because
        /// we handle the XS ones which use the same logic.
        /// </remarks>
        [Fact]
        public void Render_with_xxl_column_int_span()
        {
            var cut = Context.RenderComponent<AntDesign.Col>(p =>
            {
                p.Add(x => x.Xxl, 2);
                p.AddChildContent("Contents");
            });
            cut.MarkupMatches(@"
                <div class=""ant-col ant-col-xxl-2"" style="" "" id:ignore>Contents</div>
            ");
        }
    }
}
