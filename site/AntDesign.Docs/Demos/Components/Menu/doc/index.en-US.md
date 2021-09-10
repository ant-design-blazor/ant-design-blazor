---
category: Components
cols: 1
type: Navigation
title: Menu
cover: https://gw.alipayobjects.com/zos/alicdn/3XZcjGpvK/Menu.svg
---

A versatile menu for navigation.

## When To Use

Navigation is an important part of any website, as a good navigation setup allows users to move around the site quickly and efficiently. Ant Design offers top and side navigation options. Top navigation provides all the categories and functions of the website. Side navigation provides the multi-level structure of the website.

More layouts with navigation: [Layout](/components/layout).

## API

```html
<Menu>
  <MenuItem>Menu</MenuItem>
  <SubMenu Title="SubMenu">
    <MenuItem>SubMenuItem</MenuItem>
  </SubMenu>
</Menu>
```

### Menu

| Param | Description | Type | Default value | Version |
| --- | --- | --- | --- | --- |
| Accordion | SubMenu accordion mode | boolean | false |
| DefaultOpenKeys | Array with the keys of default opened sub menus | string\[] |  |  |
| DefaultSelectedKeys | Array with the keys of default selected menu items | string\[] |  |  |
| ForceSubMenuRender | Render submenu into DOM before it becomes visible | boolean | false | (Not implemented) |
| InlineCollapsed | Specifies the collapsed status when menu is inline mode | boolean | - |  |
| InlineIndent | Indent (in pixels) of inline menu items on each level | number | 24 | (Not implemented) |
| Mode | Type of menu | `MenuMode.Vertical` \| `MenuMode.Horizontal` \| `MenuMode.Inline` | `MenuMode.Vertical` |  |
| Multiple | Allows selection of multiple items | boolean | false | (Not implemented) |
| OpenKeys | Array with the keys of currently opened sub-menus | string\[] |  |  |
| OnDeselect | Called when a menu item is deselected (multiple mode only) | function({ item, key, keyPath, selectedKeys, domEvent }) | - | (Not implemented) |
| OnOpenChange | Called when sub-menus are opened or closed | function(openKeys: string\[]) | noop | (Not implemented) |
| OnSelect | Called when a menu item is selected | function({ item, key, keyPath, selectedKeys, domEvent }) | none | (Not implemented) |
| OverflowedIndicator | Customized icon when menu is collapsed | RenderFragment | - | (Not implemented) |
| Selectable | Allows selecting menu items. When it is `false` the menu item is not selected after `OnClick`. | boolean | true |  |
| SelectedKeys | Array with the keys of currently selected menu items | string\[] |  |  |
| Style | Style of the root node | string |  |  |
| SubMenuCloseDelay | Delay time to hide submenu when mouse leaves (in seconds) | number | 0.1 | (Not implemented) |
| SubMenuOpenDelay | Delay time to show submenu when mouse enters, (in seconds) | number | 0 | (Not implemented) |
| Theme | Color theme of the menu | `light` \| `dark` | `light` |  |
| TriggerSubMenuAction | Which action can trigger submenu open/close | `hover` \| `click` | `hover` |  |

### MenuItem

| Param    | Description                          | Type    | Default value | Version |
| -------- | ------------------------------------ | ------- | ------------- | ------- |
| ChildContent    | Set display title for collapsed item | string  |               |         |
| Disabled | Whether menu item is disabled        | boolean | false         |         |
| Key      | Unique ID of the menu item           | string  |               |         |
| OnClick | Called when a menu item is clicked | EventCallback&lt;MouseEventArgs> | - |  |
| RouterLink    |  Href route   | string |     -          |         |
| RouterMatch    | Modifies the URL matching behavior for a NavLink  | NavLinkMatch |     -          |         |
| Style    | Additional CSS style  | string |     -          |         |
| Title | Title of the menu item | string |  |  |
| Icon | The icon of the menu item | string | - | |
| IconTemplate | Custom icon template, when `Icon` and `IconTemplate` are set at the same time, `IconTemplate` is preferred| RenderFragment | - | |

### SubMenu

| Param | Description | Type | Default value | Version |
| --- | --- | --- | --- | --- |
| ChildContent | Sub-menus or sub-menu items | RenderFragment |  |  |
| Disabled | Whether sub-menu is disabled | boolean | false |  |
| IsOpen | Open state of the SubMenu | bool | false |  |
| Key | Unique ID of the sub-menu | string |  |  |
| OnTitleClick | Callback executed when the sub-menu title is clicked | EventCallback&lt;MouseEventArgs> |  |  |
| PopupClassName | Sub-menu class name | string |  | Not implemented) |
| Title | Title of the sub-menu | string |  |  |
| TitleTemplate | Title of the sub-menu | RenderFragment | - |  |

### MenuItemGroup

| Param    | Description        | Type              | Default value | Version |
| -------- | ------------------ | ----------------- | ------------- | ------- |
| ChildContent | sub-menu items     | RenderFragment       |               |         |
| Style    | Additional CSS style  | string |     -          |         |
| Title    | Title of the group | string |               |         |
| TitleTemplate | Title of the group | RenderFragment | - |  |

### Menu.Divider

Divider line in between menu items, only used in vertical popup Menu or Dropdown Menu.