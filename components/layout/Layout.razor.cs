using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
     * <summary>
        <para>Handling the overall layout of a page.</para>

        <h2>Specification</h2>
        <h3>Size</h3>
        <para>The first level navigation is left aligned near a logo, and the secondary menu is right aligned.</para>
        <list type="bullet">
            <item>Top Navigation: the height of the first level navigation <c>64px</c>, the second level navigation
                <c>48px</c>.</item>
            <item>Top Navigation (for landing pages): the height of the first level navigation <c>80px</c>, the second
                level navigation <c>56px</c>.</item>
            <item>Calculation formula of a top navigation: <c>48+8n</c>.</item>
            <item>Calculation formula of an aside navigation: <c>200+8n</c>.</item>
        </list>
    
        <h3>Interaction rules</h3>
        <list type="bullet">
            <item>The first level navigation and the last level navigation should be distinguishable by visualization;</item>
            <item>The current item should have the highest priority of visualization;</item>
            <item>When the current navigation item is collapsed, the style of the current navigation item is applied to its
                parent level;</item>
            <item>The left side navigation bar has support for both the accordion and expanding styles; you can choose the one
                that fits your case the best.</item>
        </list>
        
        <h2>Visualization rules</h2>
        <para>Style of a navigation should conform to its level.</para>
        <list type="bullet">
            <item>
                <para><strong>Emphasis by colorblock</strong></para>
                <para>When background color is a deep color, you can use this pattern for the parent level navigation item of
                    the current page.</para>
            </item>
            <item>
                <para><strong>The highlight match stick</strong></para>
                <para>When background color is a light color, you can use this pattern for the current page navigation item; we
                    recommend using it for the last item of the navigation path.</para>
            </item>
            <item>
                <para><strong>Highlighted font</strong></para>
                <para>From the visualization aspect, a highlighted font is stronger than colorblock; this pattern is often used
                    for the parent level of the current item.</para>
            </item>
            <item>
                <para><strong>Enlarge the size of the font</strong></para>
                <para><c>12px</c>, <c>14px</c> is a standard font size of navigations, <c>14px</c> is used
                    for the first and the second level of the navigation. You can choose an appropriate font size regarding
                    the level of your navigation.</para>
            </item>
        </list>
        
        <h2>Component Overview</h2>
        <list type="bullet">
            <item><c>Layout</c>: The layout wrapper, in which <c>Header</c> <c>Sider</c>
                <c>Content</c> <c>Footer</c> or <c>Layout</c> itself can be nested, and can be placed in
                any parent container.</item>
            <item><c>Header</c>: The top layout with the default style, in which any element can be nested, and must be
                placed in <c>Layout</c>.</item>
            <item><c>Sider</c>: The sidebar with default style and basic functions, in which any element can be nested,
                and must be placed in <c>Layout</c>.</item>
            <item><c>Content</c>: The content layout with the default style, in which any element can be nested, and
                must be placed in <c>Layout</c>.</item>
            <item><c>Footer</c>: The bottom layout with the default style, in which any element can be nested, and must
                be placed in <c>Layout</c>.</item>
        </list>
        <blockquote>
            <para>Based on <c>flex layout</c>, please pay attention to the <a href="http://caniuse.com/#search=flex">compatibility</a>.</para>
        </blockquote>
     </summary>
    <seealso cref="Header"/>
    <seealso cref="Footer"/>
    <seealso cref="Content"/>
    <seealso cref="Sider"/>
     */
    [Documentation(DocumentationCategory.Components, DocumentationType.Layout, "https://gw.alipayobjects.com/zos/alicdn/hzEndUVEx/Layout.svg", Columns = 1)]
    public partial class Layout : AntDomComponentBase
    {
        /// <summary>
        /// The inner content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private bool _hasSider;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ClassMapper
                .Add("ant-layout")
                .If("ant-layout-has-sider", () => _hasSider)
                .If("ant-layout-rtl", () => RTL);
        }

        internal void HasSider()
        {
            _hasSider = true;
            StateHasChanged();
        }
    }
}
