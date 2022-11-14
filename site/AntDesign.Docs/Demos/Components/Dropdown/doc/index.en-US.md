---
category: Components
type: Navigation
title: Dropdown
cover: https://gw.alipayobjects.com/zos/alicdn/eedWN59yJ/Dropdown.svg
---

A dropdown list.

## When To Use

When there are more than a few options to choose from, you can wrap them in a `Dropdown`. By hovering or clicking on the trigger, a dropdown menu will appear, which allows you to choose an option and execute the relevant action.

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
| Arrow | Whether the dropdown arrow should be visible | boolean | false |  |
| ArrowPointAtCenter | Whether the dropdown arrow should be visible and point at center | boolean | false |  |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| ChildContent | `Dropdown` trigger (link, button, etc)         | RenderFragment    | -         |
| Class | Specifies one or more class names for an DOM element.         |  string   | -         |
| ComplexAutoCloseAndVisible |  Both auto-off and Visible control close        | bool     | false         |
| Disabled | Whether the `Dropdown` is disabled.         | bool    | false     |
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
| OverlayClassName | The class name of the dropdown root element         | string    | -         |
| OverlayEnterCls | Css class added to overlay when overlay is shown.         | string    | -         |
| OverlayHiddenCls | Css class added to overlay when overlay is hidden.         | string    | -         |
| OverlayLeaveCls | Css class added to overlay when overlay is hiding.         | string    | -         |
| OverlayStyle | The style of the dropdown root element.         | string    | -         |
| Placement | The position of the `Dropdown` overlay relative to the target, which can be one of `Top` `Left` `Right` `Bottom` `TopLeft` `TopRight` `BottomLeft` `BottomRight` `LeftTop` `LeftBottom` `RightTop` `RightBottom` | PlacementType | `PlacementType.BottomLeft` |  |
| PlacementCls | Override default placement class which is based on `Placement` parameter.         | string    | -         |
| PopupContainerSelector | Define what is going to be the container of the overlay. Example use case: when overlay has to be contained in a scrollable area.         | string    | "body"       |
| Style | (not used in Unbound) Style of the wrapping div.          | string    | -         |
| Trigger | `Dropdown` trigger mode. Could be multiple by passing an array | TriggerType[] | `TriggerType.Hover` |  |
| TriggerReference | Manually set reference to element triggering `Dropdown`. | ElementReference | - |  |
| Unbound | ChildElement with `ElementReference` set to avoid wrapping div.         | RenderFragment<ForwardRef>    | -         |
| Visible | Whether the `Dropdown` menu is currently visible.         | bool    | false         |

### DropdownButton
| Property | Description | Type | Default Value | Version 
| --- | --- | --- | --- |
| Block | Option to fit button width to its parent width         | bool    | false         | 0.9
| ButtonsRender |Fully customizable button.         | Func<RenderFragment, RenderFragment, RenderFragment>    | -         | 
| ButtonsClass |  Allows to set each button's css class either to the same string or separately.   | OneOf<string, (string LeftButton, string RightButton)>    | -         | 0.9
| ButtonsStyle |  Allows to set each button's style either to the same string or separately.   | OneOf<string, (string LeftButton, string RightButton)>    | -         | 0.9
| Danger | Set the danger status of button | bool    | false         | 0.9
| Ghost | Make background transparent and invert text and border colors | bool    | false         | 0.9
| Icon | Icon (appears on the right) | string | `ellipsis`         | 
| Loading | Show loading indicator. You have to write the loading logic on your own.         | bool    | false         | 0.9
| Size | Size of the button, the same as [`Button`](en-US/components/button)         | AntSizeLDSType    | `AntSizeLDSType.Default`         | 
| Type | Type of the button, the same as [`Button`](en-US/components/button). Left and right button type can be set independently.         | OneOf<string, (string LeftButton, string RightButton)>    | `ButtonType.Default` | 0.9
