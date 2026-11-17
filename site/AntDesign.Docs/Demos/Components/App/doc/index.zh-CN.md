---
category: Components
group: Other
type: Other
title: App
subtitle: 应用根容器
description: Ant Design 服务和全局样式的应用容器。
---

## 何时使用

- 为 `.ant-app` 根节点提供样式。
- 通过 `AntDesignAppContext` 向后代组件提供 message、notification 和 modal 服务。

## 示例

<code src="./demo/Basic.razor">基础用法</code>
<code src="./demo/Config.razor">配置</code>

## 用法

使用 `<App>` 包裹页面或应用子树，后代组件可通过级联参数访问 `AntDesignAppContext`。设置 `Component="@false"` 可省略根 DOM，同时保留服务上下文。

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

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Component | 根元素的 HTML 标签名，或设置为 `false` 省略根元素 | string \| bool | `div` |
| Message | 全局 message 默认配置 | MessageGlobalConfig | - |
| Notification | 全局 notification 默认配置 | NotificationGlobalConfig | - |
| PrefixCls | CSS 类名前缀 | string | `ant-app` |
| ClassName | 根节点附加类名 | string | - |
| RootClassName | 根节点附加类名 | string | - |
| Style | 根节点内联样式 | string | - |
| ChildContent | App 子树 | RenderFragment | - |

`AntDesignAppContext` 提供 `Message`、`Notification` 和 `Modal` 服务。`Message` 和 `Notification` 配置应用于共享的 Blazor 服务提供器；设置默认值时应与 React 文档建议一致，在应用根部使用一个 App。

## 设计 Token

App 没有独立的组件级 Token，使用下方列出的全局 Token。

<ComponentTokenTable component="App"></ComponentTokenTable>

## 语义 DOM

| 名称 | 说明 | 版本 |
| --- | --- | --- |
| root | App 根容器，包裹子树内容并应用基础排版样式。 | 5.1.0 |
