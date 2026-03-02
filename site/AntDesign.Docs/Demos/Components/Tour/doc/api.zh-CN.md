## API

### Tour

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Open | 是否打开引导 | `bool` | `false` |
| Steps | 引导步骤数组 | `List<TourStep>` | `[]` |
| Current | 当前步骤索引 | `int` | `0` |
| Type | 引导类型,影响底色与文字颜色 | `TourType` | `TourType.Default` |
| Mask | 是否显示蒙层 | `bool` | `true` |
| MaskStyle | 蒙层的样式 | `string` | - |
| ZIndex | Tour 的 z-index | `int` | `1001` |
| CloseIcon | 自定义关闭图标 | `RenderFragment` | - |
| IndicatorsRender | 自定义指示器 | `RenderFragment<(int current, int total)>` | - |
| OnChange | 步骤改变时的回调 | `EventCallback<int>` | - |
| OnClose | 关闭引导时的回调 | `EventCallback` | - |
| OnFinish | 完成引导时的回调 | `EventCallback` | - |

### TourStep

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Title | 标题 | `string` | - |
| Description | 描述文字 | `string` | - |
| DescriptionTemplate | 自定义描述内容 | `RenderFragment` | - |
| Cover | 展示的图片或视频 | `RenderFragment` | - |
| Target | 目标元素,为 null 时显示在屏幕中央 | `Func<ElementReference?>` | - |
| Placement | 引导卡片相对于目标元素的位置 | `Placement` | `Placement.Bottom` |
| Arrow | 是否显示箭头 | `bool` | `true` |
| Mask | 是否启用蒙层,也可传入配置改变蒙层样式 | `bool?` | - |
| MaskStyle | 蒙层的样式 | `string` | - |
| Type | 类型,影响底色与文字颜色 | `TourType` | `TourType.Default` |
| Closable | 是否显示关闭按钮 | `bool` | `true` |
| PrevButtonProps | 上一步按钮的属性 | `TourButtonProps` | - |
| NextButtonProps | 下一步按钮的属性 | `TourButtonProps` | - |

### TourType

| 值 | 说明 |
| --- | --- |
| Default | 默认样式 |
| Primary | 主色调样式 |

### TourButtonProps

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| Text | 按钮文字 | `string` | - |
| OnClick | 点击事件 | `EventCallback<MouseEventArgs>` | - |
| Disabled | 是否禁用 | `bool` | `false` |
| Class | 自定义样式类 | `string` | - |
| Style | 自定义样式 | `string` | - |
