---
category: Components
group: Other
type: Other
title: App
description: Application wrapper for Ant Design services and reset styles.
---

## When To Use

- Provide the `.ant-app` root styles.
- Make message, notification, and modal services available to descendants through `AntDesignAppContext`.

## Examples

<code src="./demo/Basic.razor">Basic</code>
<code src="./demo/Config.razor">Hooks config</code>

## Usage

Wrap the page or application subtree with `<App>`. Descendants can access `AntDesignAppContext` as a cascading parameter. Set `Component="@false"` to omit the root element while keeping the service context.

```razor
<App>
    <MyPage />
</App>
```

```razor
@code {
    [CascadingParameter] public AntDesignAppContext AppContext { get; set; }
}
```

## API

| Parameter | Description | Type | Default |
| --- | --- | --- | --- |
| Component | HTML tag name for the root element, or `false` to omit it | string \| bool | `div` |
| Message | Default global message configuration | MessageGlobalConfig | - |
| Notification | Default global notification configuration | NotificationGlobalConfig | - |
| PrefixCls | CSS class prefix | string | `ant-app` |
| ClassName | Additional root class | string | - |
| RootClassName | Additional root class | string | - |
| Style | Inline style for the root element | string | - |
| ChildContent | App subtree | RenderFragment | - |

`AntDesignAppContext` exposes `Message`, `Notification`, and `Modal` services. `Message` and `Notification` apply to the shared Blazor providers; as with the React guidance, use one App at the application root when setting defaults.

## Design Token

App has no component-specific tokens and uses the shared global tokens below.

<ComponentTokenTable component="App"></ComponentTokenTable>

## Semantic DOM

| Name | Description | Version |
| --- | --- | --- |
| root | The root container that wraps the subtree and applies the base typography styles. | 5.1.0 |
