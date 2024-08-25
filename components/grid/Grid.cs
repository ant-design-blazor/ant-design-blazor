namespace AntDesign
{
    /**
    <summary>
        <para>24 Grids System.</para>

        <h2>Design concept</h2>

        <para>In most business situations, Ant Design needs to solve a lot of information storage problems within the design area, so based on 12 Grids System, we divided the design area into 24 sections.</para>

        <para>
        We name the divided area 'box'. We suggest four boxes for horizontal arrangement at most, one at least.
        Boxes are proportional to the entire screen as shown in the picture above.
        To ensure a high level of visual comfort, we customize the typography inside of the box based on the box unit.
        </para>

        <h2>Outline</h2>

        <para>In the grid system, we define the frame outside the information area based on row and column, to ensure that every area can have stable arrangement.</para>

        <para>Following is a brief look at how it works:</para>

        <list type="bullet">
            <item>Establish a set of column in the horizontal space defined by row (abbreviated col)</item>
            <item>Your content elements should be placed directly in the col, and only col should be placed directly in row</item>
            <item>The column grid system is a value of 1-24 to represent its range spans. For example, three columns of equal width can be created by using a span of 8 on the columns.</item>
            <item>If the sum of col spans in a row are more than 24, then the overflowing col as a whole will start a new line arrangement.</item>
        </list>
        <para>
        Our grid systems base on Flex layout to allow the elements within the parent to be aligned horizontally - left, center, right, wide arrangement, and decentralized arrangement.
        The Grid system also supports vertical alignment - top aligned, vertically centered, bottom-aligned.
        You can also define the order of elements by using order.
        </para>

        <para>Layout uses a 24 grid layout to define the width of each "box", but does not rigidly adhere to the grid layout.</para>

        <para>The breakpoints of responsive grid follow BootStrap 4 media queries rules(not including occasionally part).</para>
    </summary>
    <seealso cref="Row" />
    <seealso cref="Col" />
    <seealso cref="EmbeddedProperty" />
    */

    [Documentation(DocumentationCategory.Components,
        DocumentationType.Layout,
        "https://gw.alipayobjects.com/zos/alicdn/5rWLU27so/Grid.svg",
        Columns = 1,
        Title = "Grid",
        OutputApi = false)]
    internal class Grid
    {
    }
}
