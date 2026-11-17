### 为什么 `tags` 模式搜索时会出现两个文本相同的选项？

当选项显示的标签与实际值不同时，可能出现这种情况。请让标签和值保持一致，或使用 `FilterExpression` 自定义搜索时匹配的字段。

### 如何控制点击 `PopupRender` 自定义内容后是否关闭下拉列表？

使用 `Open` 和 `OnOpenChange` 控制弹层状态。自定义内容需要保持 Select 打开，或在应用操作后关闭时，可以采用这种方式。

### 如何在点击弹层自定义内容时保持下拉列表打开？

阻止鼠标按下事件让焦点离开 Select。对于自定义 Razor 内容，可在需要保持弹层打开的元素上使用 Blazor 的 `@onmousedown:preventDefault` 和 `@onmousedown:stopPropagation` 修饰符。

### 为什么自定义选项内容可能导致虚拟滚动异常？

虚拟列表假定每个选项具有一致的行高。自定义模板改变行高时，滚动位置和当前激活选项的位置可能不准确。请保持选项高度一致；如果列表较小且需要可变高度内容，可设置 `Virtual="false"`。`PopupContainerMaxHeight` 用于设置弹层最大高度。

### 为什么无障碍检查可能报告缺少 `aria-*` 属性？

部分无障碍属性和辅助 listbox 节点会在操作 Select 时创建。请先打开下拉列表，再运行检查。需要显式提供可访问名称时，可设置 `AriaLabel`，或通过 `AdditionalAttributes` 提供 `aria-labelledby`。屏幕阅读器需要直接遍历实际选项元素而非虚拟列表辅助节点时，可设置 `Virtual="false"`。

### 为什么点击自定义标签的关闭图标会打开下拉列表？

Select 会监听标签上的鼠标按下事件来管理焦点。使用 `TagRender` 组件时，请在自定义标签上设置 `PreventMouseDownDefault` 和 `StopMouseDownPropagation`，避免关闭操作触发 Select。
