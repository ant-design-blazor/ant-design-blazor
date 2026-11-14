---
category: Components
subtitle: 下拉菜单
type: 导航
title: Dropdown
cover: https://gw.alipayobjects.com/zos/alicdn/eedWN59yJ/Dropdown.svg
---

向下弹出的列表。

## 何时使用

当页面上的操作命令过多时，用此组件可以收纳操作元素。点击或移入触点，会出现一个下拉菜单。可在列表中进行选择，并执行相应的命令。

## Two types

There are 2 rendering approaches for `Dropdown`:  
1. Wraps child element (content of the `Dropdown`) with a `<div>` (default approach).
2. Child element is not wrapped with anything. This approach requires usage of `<Unbound>` tag inside `<Dropdown>` and depending on the child element type (please refer to the first example):
   - html tag: has to have its `@ref` set to `@context.Current` 
   - `Ant Design Blazor` component: has to have its `RefBack` attribute set to `@context`.

## API

### Common API

| Property | Description | Type | Default Value | Version 
| --- | --- | --- | --- |
| Arrow | 下拉框箭头是否显示 | boolean | false |  |
| ArrowPointAtCenter | 下拉框箭头是否显示并且居中 | boolean | false |  |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| ChildContent | `Dropdown` trigger (link, button, etc)         | RenderFragment    | -         |
| Class | Specifies one or more class names for an DOM element.         |  string   | -         |
| ComplexAutoCloseAndVisible |  自动关闭功能和Visible参数同时生效       | bool     | false         |
| Disabled | 菜单是否禁用         | bool    | false     |
| Id | (not used in Unbound) Id of the wrapping div.          | string    | -         |
| InlineFlexMode | (not used in Unbound) Sets wrapping div style to `display: inline-flex;`.         | bool    | false     |
| IsButton | Behave like a button. For `DropdownButton` is always `true`.        | bool    | false         |
| OnClick | Callback when `Dropdown` is clicked          | Action    | -         |
| OnMaskClick | Callback - equivalent of OnMouseUp event on the `Dropdown` trigger.         | Action    | -         |
| OnMouseEnter | Callback when mouse enters `Dropdown` boundaries.         | Action    | -         |
| OnMouseLeave | Callback when mouse leaves `Dropdown` boundaries.         | Action    | -         |
| OnOverlayHidding | Callback when overlay is hiding.         | Action<bool>    | -         |
| OnVisibleChange |  Callback when overlay visibility is changing.        | Action<bool>    | -         |
| Overlay | Overlay content (what will be rendered after `Dropdown` is triggered, the dropdown menu).          | RenderFragment    | -         |
| OverlayClassName | 下拉根元素的类名称       | string    | -         |
| OverlayEnterCls | Css class added to overlay when overlay is shown.         | string    | -         |
| OverlayHiddenCls | Css class added to overlay when overlay is hidden.         | string    | -         |
| OverlayLeaveCls | Css class added to overlay when overlay is hiding.         | string    | -         |
| OverlayStyle | 下拉根元素的样式         | string    | -         |
| Placement | The position of the `Dropdown` overlay relative to the target, which can be one of `Top` `Left` `Right` `Bottom` `TopLeft` `TopRight` `BottomLeft` `BottomRight` `LeftTop` `LeftBottom` `RightTop` `RightBottom` | PlacementType | `PlacementType.BottomLeft` |  |
| PlacementCls | Override default placement class which is based on `Placement` parameter.         | string    | -         |
| PopupContainerSelector | Define what is going to be the container of the overlay. Example use case: when overlay has to be contained in a scrollable area.         | string    | "body"       |
| Style | (not used in Unbound) Style of the wrapping div.          | string    | -         |
| Trigger | `Dropdown` trigger mode. Could be multiple by passing an array | TriggerType[] | `TriggerType.Hover` |  |
| TriggerReference | Manually set reference to element triggering `Dropdown`. | ElementReference | - |  |
| Unbound | ChildElement with `ElementReference` set to avoid wrapping div.         | RenderFragment<ForwardRef>    | -         |
| Visible | 菜单是否显示        | bool    | false         |

### DropdownButton
| Property | Description | Type | Default Value | Version 
| --- | --- | --- | --- |
| Block | 将按钮宽度调整为其父宽度的选项        | bool    | false         | 0.9
| ButtonsRender | 自定义左右两个按钮 | Func<RenderFragment, RenderFragment, RenderFragment>    | -         | 
| ButtonsClass |  Allows to set each button's css class either to the same string or separately.   | OneOf<string, (string LeftButton, string RightButton)>    | -         | 0.9
| ButtonsStyle |  Allows to set each button's style either to the same string or separately.   | OneOf<string, (string LeftButton, string RightButton)>    | -         | 0.9
| Danger | 设置危险按钮 | bool    | false         | 0.9
| Ghost | 幽灵属性，使按钮背景透明 | bool    | false         | 0.9
| Icon | 右侧的 icon | string | `ellipsis`         | 
| Loading | 设置按钮载入状态         | bool    | false         | 0.9
| Size | 按钮大小，和 [`Button`](zh-CN/components/button) 一致         | AntSizeLDSType    | `AntSizeLDSType.Default`         | 
| Type | Type of the button, the same as [`Button`](en-US/components/button). Left and right button type can be set independently.         | OneOf<string, (string LeftButton, string RightButton)>    | `ButtonType.Default` | 0.9
