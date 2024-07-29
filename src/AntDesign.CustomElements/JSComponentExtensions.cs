using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign;

namespace Microsoft.AspNetCore.Components.Web;

public static class JSComponentExtensions
{
    public static void RegisterCustomElements(this IJSComponentConfiguration rootComponents)
    {
        rootComponents.RegisterCustomElement<Affix>("ant-affix");
        rootComponents.RegisterCustomElement<Alert>("ant-alert");
        rootComponents.RegisterCustomElement<Anchor>("ant-anchor");
        rootComponents.RegisterCustomElement<AutoComplete<string>>("ant-autocomplete");
        rootComponents.RegisterCustomElement<Avatar>("ant-avatar");
        rootComponents.RegisterCustomElement<BackTop>("ant-backtop");
        rootComponents.RegisterCustomElement<Badge>("ant-badge");
        rootComponents.RegisterCustomElement<Breadcrumb>("ant-breadcrumb");
        rootComponents.RegisterCustomElement<BreadcrumbItem>("ant-breadcrumb-item");
        rootComponents.RegisterCustomElement<Button>("ant-button");
        rootComponents.RegisterCustomElement<Calendar>("ant-calendar");
        rootComponents.RegisterCustomElement<Card>("ant-card");
        rootComponents.RegisterCustomElement<Carousel>("ant-carousel");
        rootComponents.RegisterCustomElement<Cascader>("ant-cascader");
        rootComponents.RegisterCustomElement<Checkbox>("ant-checkbox");
        rootComponents.RegisterCustomElement<CheckboxGroup<string>>("ant-checkbox-group");
        rootComponents.RegisterCustomElement<Collapse>("ant-collapse");
        rootComponents.RegisterCustomElement<Panel>("ant-collapse-panel");
        rootComponents.RegisterCustomElement<Comment>("ant-comment");
        rootComponents.RegisterCustomElement<DatePicker<DateTime?>>("ant-date-picker");
        rootComponents.RegisterCustomElement<Descriptions>("ant-descriptions");
        rootComponents.RegisterCustomElement<Descriptions>("ant-descriptions");
        rootComponents.RegisterCustomElement<Divider>("ant-divider");
        rootComponents.RegisterCustomElement<Drawer>("ant-drawer");
        rootComponents.RegisterCustomElement<Dropdown>("ant-dropdown");
        rootComponents.RegisterCustomElement<DropdownButton>("ant-dropdown-button");
        rootComponents.RegisterCustomElement<Empty>("ant-empty");
        rootComponents.RegisterCustomElement<Flex>("ant-flex");
        rootComponents.RegisterCustomElement<Form<Dictionary<string, object>>>("ant-form");
        rootComponents.RegisterCustomElement<FormItem>("ant-form-item");

        rootComponents.RegisterCustomElement<Image>("ant-image");
        rootComponents.RegisterCustomElement<Input<string>>("ant-input");
        rootComponents.RegisterCustomElement<InputNumber<double>>("ant-input-number");

        rootComponents.RegisterCustomElement<Layout>("ant-layout");
        rootComponents.RegisterCustomElement<Sider>("ant-layout-sider");
        rootComponents.RegisterCustomElement<Footer>("ant-layout-footer");
        rootComponents.RegisterCustomElement<Header>("ant-layout-header");
        rootComponents.RegisterCustomElement<Content>("ant-layout-content");


        rootComponents.RegisterCustomElement<AntList<string>>("ant-list");
        rootComponents.RegisterCustomElement<ListItem>("ant-list-item");


        rootComponents.RegisterCustomElement<Table<Dictionary<string, object>>>("ant-table");

        rootComponents.RegisterCustomElement<PageHeader>("ant-page-header");

        rootComponents.RegisterCustomElement<Notification>("ant-notification");

        rootComponents.RegisterCustomElement<Popconfirm>("ant-popconfirm");

        rootComponents.RegisterCustomElement<Modal>("ant-modal");

        rootComponents.RegisterCustomElement<Menu>("ant-menu");

        rootComponents.RegisterCustomElement<Row>("ant-row");
        rootComponents.RegisterCustomElement<Col>("ant-col");

    }
}
