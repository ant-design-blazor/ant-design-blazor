---
order: 3
title:
  zh-CN: 自定义组件
  en-US: Custom component
additionalFiles:
  - Shared/StatusCard.razor
  - Shared/StatusCard.razor.cs

---

## zh-CN

`StatusCard` 是一个继承 `AntComponentBase` 的自定义组件。源生成器会从它的 `[Parameter]` 属性生成 `StatusCardProps`，因此自定义组件与内置组件使用完全相同的 Provider 机制。默认 Description 应用于两张卡片，`warning` 卡片再按 `Id` 覆写高亮状态和文案。

> 注意：自定义组件的 partial 类声明和 `[Parameter]` 属性必须放在 `.cs` 或 `.razor.cs` 文件中，不能只写在 `.razor` 文件的 `@code` 块里。当前源生成器只读取 C# 语法树，无法识别 Razor 文件中的类型声明。

## en-US

`StatusCard` is a custom component derived from `AntComponentBase`. The source generator creates `StatusCardProps` from its `[Parameter]` properties, so custom and built-in components use the same provider mechanism. The default description applies to both cards; the `warning` card then overrides its highlighted state and description by `Id`.

> Note: Put the custom component's partial class declaration and `[Parameter]` properties in a `.cs` or `.razor.cs` file, not only in a `.razor` `@code` block. The current source generator reads C# syntax trees and cannot discover type declarations from Razor files.
