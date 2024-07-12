using Bunit;
using Xunit;

namespace AntDesign.Tests.Styles
{
    public class GenerateStyleTests : StyleTestsBase
    {

        [Fact]
        public void Generate_Affix_Style()
        {
            var content = LoadStyleHtml("Styles/css/Affix.css");
            var (html, hashId) = AffixStyle.UseComponentStyle()("ant-affix");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Alert_Style()
        {
            var content = LoadStyleHtml("Styles/css/Alert.css");
            var (html, hashId) = AlertStyle.UseComponentStyle()("ant-alert");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Anchor_Style()
        {
            var content = LoadStyleHtml("Styles/css/Anchor.css");
            var (html, hashId) = AnchorStyle.UseComponentStyle()("ant-anchor");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Avatar_Style()
        {
            var content = LoadStyleHtml("Styles/css/Avatar.css");
            var (html, hashId) = AvatarStyle.UseComponentStyle()("ant-avatar");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Badge_Style()
        {
            var content = LoadStyleHtml("Styles/css/Badge.css");
            var (html, hashId) = BadgeStyle.UseComponentStyle()("ant-badge");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Breadcrumb_Style()
        {
            var content = LoadStyleHtml("Styles/css/Breadcrumb.css");
            var (html, hashId) = BreadcrumbStyle.UseComponentStyle()("ant-breadcrumb");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Button_Style()
        {
            var content = LoadStyleHtml("Styles/css/Button.css");
            var (html, hashId) = ButtonStyle.UseComponentStyle()("ant-btn");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Calendar_Style()
        {
            var content = LoadStyleHtml("Styles/css/Calendar.css");
            var (html, hashId) = CalendarStyle.UseComponentStyle()("ant-calendar");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Card_Style()
        {
            var content = LoadStyleHtml("Styles/css/Card.css");
            var (html, hashId) = CardStyle.UseComponentStyle()("ant-card");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Carousel_Style()
        {
            var content = LoadStyleHtml("Styles/css/Carousel.css");
            var (html, hashId) = CarouselStyle.UseComponentStyle()("ant-carousel");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Cascader_Style()
        {
            var content = LoadStyleHtml("Styles/css/Cascader.css");
            var (html, hashId) = CascaderStyle.UseComponentStyle()("ant-cascader");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Checkbox_Style()
        {
            var content = LoadStyleHtml("Styles/css/Checkbox.css");
            var (html, hashId) = CheckboxStyle.UseComponentStyle()("ant-checkbox");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Collapse_Style()
        {
            var content = LoadStyleHtml("Styles/css/Collapse.css");
            var (html, hashId) = CollapseStyle.UseComponentStyle()("ant-collapse");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_DatePicker_Style()
        {
            var content = LoadStyleHtml("Styles/css/DatePicker.css");
            var (html, hashId) = DatePickerStyle.UseComponentStyle()("ant-datepicker");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Descriptions_Style()
        {
            var content = LoadStyleHtml("Styles/css/Descriptions.css");
            var (html, hashId) = DescriptionsStyle.UseComponentStyle()("ant-descriptions");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Divider_Style()
        {
            var content = LoadStyleHtml("Styles/css/Divider.css");
            var (html, hashId) = DividerStyle.UseComponentStyle()("ant-divider");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Drawer_Style()
        {
            var content = LoadStyleHtml("Styles/css/Drawer.css");
            var (html, hashId) = DrawerStyle.UseComponentStyle()("ant-drawer");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Dropdown_Style()
        {
            var content = LoadStyleHtml("Styles/css/Dropdown.css");
            var (html, hashId) = DropdownStyle.UseComponentStyle()("ant-dropdown");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Empty_Style()
        {
            var content = LoadStyleHtml("Styles/css/Empty.css");
            var (html, hashId) = EmptyStyle.UseComponentStyle()("ant-empty");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Form_Style()
        {
            var content = LoadStyleHtml("Styles/css/Form.css");
            var (html, hashId) = FormStyle.UseComponentStyle()("ant-form");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_GridCol_Style()
        {
            var content = LoadStyleHtml("Styles/css/GridCol.css");
            var (html, hashId) = GridStyle.UseColStyle()("ant-gridcol");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_GridRow_Style()
        {
            var content = LoadStyleHtml("Styles/css/GridRow.css");
            var (html, hashId) = GridStyle.UseRowStyle()("ant-gridrow");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Input_Style()
        {
            var content = LoadStyleHtml("Styles/css/Input.css");
            var (html, hashId) = InputStyle.UseComponentStyle()("ant-input");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Layout_Style()
        {
            var content = LoadStyleHtml("Styles/css/Layout.css");
            var (html, hashId) = LayoutStyle.UseComponentStyle()("ant-layout");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_List_Style()
        {
            var content = LoadStyleHtml("Styles/css/List.css");
            var (html, hashId) = AntListStyle.UseComponentStyle()("ant-list");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Mentions_Style()
        {
            var content = LoadStyleHtml("Styles/css/Mentions.css");
            var (html, hashId) = MentionsStyle.UseComponentStyle()("ant-mentions");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Menu_Style()
        {
            var content = LoadStyleHtml("Styles/css/Menu.css");
            var (html, hashId) = MenuStyle.UseComponentStyle()("ant-menu");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Modal_Style()
        {
            var content = LoadStyleHtml("Styles/css/Modal.css");
            var (html, hashId) = ModalStyle.UseComponentStyle()("ant-modal");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Pagination_Style()
        {
            var content = LoadStyleHtml("Styles/css/Pagination.css");
            var (html, hashId) = PaginationStyle.UseComponentStyle()("ant-pagination");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Popconfirm_Style()
        {
            var content = LoadStyleHtml("Styles/css/Popconfirm.css");
            var (html, hashId) = PopconfirmStyle.UseComponentStyle()("ant-popconfirm");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Popover_Style()
        {
            var content = LoadStyleHtml("Styles/css/Popover.css");
            var (html, hashId) = PopoverStyle.UseComponentStyle()("ant-popover");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Progress_Style()
        {
            var content = LoadStyleHtml("Styles/css/Progress.css");
            var (html, hashId) = ProgressStyle.UseComponentStyle()("ant-progress");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Radio_Style()
        {
            var content = LoadStyleHtml("Styles/css/Radio.css");
            var (html, hashId) = RadioStyle.UseComponentStyle()("ant-radio");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Rate_Style()
        {
            var content = LoadStyleHtml("Styles/css/Rate.css");
            var (html, hashId) = RateStyle.UseComponentStyle()("ant-rate");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Result_Style()
        {
            var content = LoadStyleHtml("Styles/css/Result.css");
            var (html, hashId) = ResultStyle.UseComponentStyle()("ant-result");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Segmented_Style()
        {
            var content = LoadStyleHtml("Styles/css/Segmented.css");
            var (html, hashId) = SegmentedStyle.UseComponentStyle()("ant-segmented");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Select_Style()
        {
            var content = LoadStyleHtml("Styles/css/Select.css");
            var (html, hashId) = SelectStyle.UseComponentStyle()("ant-select");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Skeleton_Style()
        {
            var content = LoadStyleHtml("Styles/css/Skeleton.css");
            var (html, hashId) = SkeletonStyle.UseComponentStyle()("ant-skeleton");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Slider_Style()
        {
            var content = LoadStyleHtml("Styles/css/Slider.css");
            var (html, hashId) = SliderStyle.UseComponentStyle()("ant-slider");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Space_Style()
        {
            var content = LoadStyleHtml("Styles/css/Space.css");
            var (html, hashId) = SpaceStyle.UseComponentStyle()("ant-space");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Spin_Style()
        {
            var content = LoadStyleHtml("Styles/css/Spin.css");
            var (html, hashId) = SpinStyle.UseComponentStyle()("ant-spin");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Statistic_Style()
        {
            var content = LoadStyleHtml("Styles/css/Statistic.css");
            var (html, hashId) = StatisticStyle.UseComponentStyle()("ant-statistic");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Steps_Style()
        {
            var content = LoadStyleHtml("Styles/css/Steps.css");
            var (html, hashId) = StepsStyle.UseComponentStyle()("ant-steps");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Switch_Style()
        {
            var content = LoadStyleHtml("Styles/css/Switch.css");
            var (html, hashId) = SwitchStyle.UseComponentStyle()("ant-switch");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Table_Style()
        {
            var content = LoadStyleHtml("Styles/css/Table.css");
            var (html, hashId) = TableStyle.UseComponentStyle()("ant-table");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Tabs_Style()
        {
            var content = LoadStyleHtml("Styles/css/Tabs.css");
            var (html, hashId) = TabsStyle.UseComponentStyle()("ant-tabs");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Tag_Style()
        {
            var content = LoadStyleHtml("Styles/css/Tag.css");
            var (html, hashId) = TagStyle.UseComponentStyle()("ant-tag");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Timeline_Style()
        {
            var content = LoadStyleHtml("Styles/css/Timeline.css");
            var (html, hashId) = TimelineStyle.UseComponentStyle()("ant-timeline");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Tooltip_Style()
        {
            var content = LoadStyleHtml("Styles/css/Tooltip.css");
            var (html, hashId) = TooltipStyle.UseComponentStyle()("ant-tooltip");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Transfer_Style()
        {
            var content = LoadStyleHtml("Styles/css/Transfer.css");
            var (html, hashId) = TransferStyle.UseComponentStyle()("ant-transfer");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Tree_Style()
        {
            var content = LoadStyleHtml("Styles/css/Tree.css");
            var (html, hashId) = TreeStyle.UseComponentStyle()("ant-tree");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_TreeSelect_Style()
        {
            var content = LoadStyleHtml("Styles/css/TreeSelect.css");
            var (html, hashId) = TreeSelectStyle.UseComponentStyle()("ant-treeselect");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Typography_Style()
        {
            var content = LoadStyleHtml("Styles/css/Typography.css");
            var (html, hashId) = TypographyStyle.UseComponentStyle()("ant-typography");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }

        [Fact]
        public void Generate_Upload_Style()
        {
            var content = LoadStyleHtml("Styles/css/Upload.css");
            var (html, hashId) = UploadStyle.UseComponentStyle()("ant-upload");
            var cut = Render(html).Find("style:first-child");
            cut.MarkupMatches(content);
        }
    }
}
