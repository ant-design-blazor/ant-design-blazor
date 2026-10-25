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

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| ShowCollapsedTooltip | Enable tooltips for collapsed root items | bool | true |
| CollapsedTooltipPlacement | Placement of collapsed item tooltips | Placement | Right |
| PopupRender | Customize submenu popups; context contains Item, Keys and Content | RenderFragment&lt;MenuPopupContext&gt; | - |

| Param | Description | Type | Default value | Version |
| --- | --- | --- | --- | --- |
| Accordion | SubMenu accordion mode | boolean | false |
| ClassNames | Customize classes for Menu Semantic DOM paths, such as `root`, `item`, `popup.root`, and `subMenu.item` | `IReadOnlyDictionary<string, string>` | - | 6.0.0 |
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
| DisabledOverflow | Disable automatic item overflow in horizontal mode | boolean | false | 6.0.0 |
| OverflowedIndicator | Custom content of the horizontal overflow indicator | RenderFragment | Ellipsis icon | 6.0.0 |
| OverflowedIndicatorPopupClassName | CSS class of the overflow submenu popup | string | - | 6.0.0 |
| Selectable | Allows selecting menu items. When it is `false` the menu item is not selected after `OnClick`. | boolean | true |  |
| SelectedKeys | Array with the keys of currently selected menu items | string\[] |  |  |
| Style | Style of the root node | string |  |  |
| SubMenuCloseDelay | Delay time to hide submenu when mouse leaves (in seconds) | number | 0.1 | (Not implemented) |
| SubMenuOpenDelay | Delay time to show submenu when mouse enters, (in seconds) | number | 0 | (Not implemented) |
| Theme | Color theme of the menu | `light` \| `dark` | `light` |  |
| TriggerSubMenuAction | Which action can trigger submenu open/close | `hover` \| `click` | `hover` |  |
| Styles | Customize inline styles for Menu Semantic DOM paths | `IReadOnlyDictionary<string, string>` | - | 6.0.0 |

### MenuItem

| Param        | Description                                                                                                  | Type                          | Default value | Version |
|--------------|--------------------------------------------------------------------------------------------------------------|-------------------------------|---------------|---------|
| ChildContent | Set display title for collapsed item                                                                         | string                        |               |         |
| Disabled     | Whether menu item is disabled                                                                                | boolean                       | false         |         |
| Key          | Unique ID of the menu item                                                                                   | string                        |               |         |
| OnClick      | Called when a menu item is clicked                                                                           | EventCallback<MouseEventArgs> | -             |         |
| RouterLink   | Href route                                                                                                   | string                        | -             |         |
| RouterMatch  | Modifies the URL matching behavior for a NavLink                                                             | NavLinkMatch                  | -             |         |
| Style        | Additional CSS style                                                                                         | string                        | -             |         |
| Target       | `Target` attribute on `<a>` tag if `RouterLink` is specified                                                 | MenuTarget?                   | -             |         |
| Title        | Title of the menu item                                                                                       | string                        |               |         |
| Icon         | The icon of the menu item                                                                                    | string                        | -             |         |
| IconTemplate | Custom icon template, when `Icon` and `IconTemplate` are set at the same time, `IconTemplate` is preferred   | RenderFragment                | -             |         |

### SubMenu

| Property | Description | Type | Default |
| --- | --- | --- | --- |
| Theme | Override submenu theme; inherited when unspecified | MenuTheme? | - |
| PopupRender | Customize this popup, overriding Menu.PopupRender | RenderFragment&lt;MenuPopupContext&gt; | - |

| Param | Description | Type | Default value | Version |
| --- | --- | --- | --- | --- |
| ChildContent | Sub-menus or sub-menu items | RenderFragment |  |  |
| Disabled | Whether sub-menu is disabled | boolean | false |  |
| Icon | Icon displayed before the sub-menu title | string | - |  |
| IconTemplate | Custom icon content displayed before the sub-menu title | RenderFragment | - |  |
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

## Semantic DOM

Use `ClassNames` and `Styles` with these paths:

| Semantic DOM | Description |
| --- | --- |
| `root` | Root menu list |
| `item` | Menu item |
| `itemContent` | Menu item content |
| `itemIcon` | Menu item icon |
| `itemTitle` | Menu group title |
| `list` | Menu group list |
| `popup.root` | Popup menu root |
| `subMenu.item` | Submenu item |
| `subMenu.itemTitle` | Submenu title |
| `subMenu.list` | Submenu list |
| `subMenu.itemContent` | Submenu item content |
| `subMenu.itemIcon` | Submenu item icon |

See [Customize Semantic DOM styles](#components-menu-demo-semantic-style) for an interactive example.
