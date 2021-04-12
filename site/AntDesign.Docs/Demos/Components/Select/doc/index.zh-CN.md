---
category: Components
subtitle: 选择器
type: 数据录入
title: Select
cover: https://gw.alipayobjects.com/zos/alicdn/_0XzgOis7/Select.svg
---

下拉选择器。

## 何时使用

- 弹出一个下拉菜单给用户选择操作，用于代替原生的选择器，或者需要一个更优雅的多选器时。
- 当选项少时（少于 5 项），建议直接将选项平铺，使用 [Radio](/components/radio/) 是更好的选择。

## API


### Select props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| AllowClear | 支持清除 | Boolean | false |  |
| AutoClearSearchValue | 是否在选中项后清空搜索框，只在 `mode` 为 `multiple` 或 `tags` 时有效。 | Boolean | true |  |
| AutoFocus | 默认获取焦点 | Boolean | false |  |
| DefaultActiveFirstOption | 是否默认高亮第一个选项。 | Boolean | true |  |
| DefaultValue | 指定默认选中的条目 | string\|IEnumerable&lt;string><br />LabeledValue\|IEnumerable&lt;LabeledValue> | - |  |
| Disabled | 是否禁用 | Boolean | false |  |
| DropdownClassName | 下拉菜单的 className 属性 | string | - |  |
| DropdownMatchSelectWidth | 下拉菜单和选择器同宽。默认将设置 `min-width`。`false` 时会关闭虚拟滚动 | Boolean \| number | true |  |
| DropdownRender | 自定义下拉框内容 | (menuNode: RenderFragment, props:Properties) => RenderFragment | - |  |
| DropdownStyle | 下拉菜单的 style 属性 | string | - |  |
| FilterOption | 是否根据输入项进行筛选。当其为一个函数时，会接收 `inputValue` `option` 两个参数，当 `option` 符合筛选条件时，应返回 `true`，反之则返回 `false`。 | Boolean or function(inputValue, option) | true |  |
| GetPopupContainer | 菜单渲染父节点。默认渲染到 body 上，如果你遇到菜单滚动定位问题，试试修改为滚动的区域，并相对其定位。[示例](https://codesandbox.io/s/4j168r7jw0) | Function(triggerNode) | () => document.body |  |
| LabelInValue | 是否把每个选项的 label 包装到 value 中，会把 Select 的 value 类型从 `string` 变为 `{key: string, label: RenderFragment}` 的格式 | Boolean | false |  |
| ListHeight | 设置弹窗滚动高度 | number | 256 |  |
| MaxTagCount | 最多显示多少个 tag，响应式模式会对性能产生损耗 | int | `ResponsiveTag.Responsive` | - |  |
| MaxTagPlaceholder | 隐藏 tag 时显示的内容。 If used with `ResponsiveTag.Responsive`, implement your own handling logic. | RenderFragment<IEnumerable<TItem>>> | - |  |
| MaxTagTextLength | 隐藏 tag 时显示的内容 | int | - |  |
| TagRender | 自定义 tag 内容 render | (props:Properties) => RenderFragment | - |  |
| Mode | 设置 Select 的模式为多选或标签 | `multiple` \| `tags` | - |  |
| NotFoundContent | 当下拉列表为空时显示的内容 | RenderFragment | 'Not Found' |  |
| OptionFilterProp | 搜索时过滤对应的 option 属性，如设置为 children 表示对内嵌内容进行搜索。[示例](https://codesandbox.io/s/antd-reproduction-template-tk678) | string | value |  |
| OptionLabelProp | 回填到选择框的 SelectOption 的属性值，默认是 SelectOption 的子元素。比如在子元素需要高亮效果时，此值可以设为 `value`。 | string | `children` （combobox 模式下为 `value`） |  |
| Placeholder | 选择框默认文字 | string | - |  |
| ShowArrow | 是否显示下拉小箭头 | Boolean | true |  |
| ShowSearch | 使单选模式可搜索 | Boolean | false |  |
| Size | 选择框大小 | `large` \| `middle` \| `small` | 无 |  |
| SuffixIcon | 自定义的选择框后缀图标 | RenderFragment | - |  |
| RemoveIcon | 自定义的多选框清除图标 | RenderFragment | - |  |
| ClearIcon | 自定义的多选框清空图标 | RenderFragment | - |  |
| MenuItemSelectedIcon | 自定义多选时当前选中的条目图标 | RenderFragment | - |  |
| TokenSeparators | 在 tags 和 multiple 模式下自动分词的分隔符 | IEnumerable&lt;string> |  |  |
| Value | 指定当前选中的条目 | string\|IEnumerable&lt;string><br />LabeledValue\|IEnumerable&lt;LabeledValue> | - |  |
| virtual | 设置 `false` 时关闭虚拟滚动 | Boolean | true | 4.1.0 |
| OnBlur | 失去焦点时回调 | function | - |  |
| OnChange | 选中 option，或 input 的 value 变化（combobox 模式下）时，调用此函数 | function(value, option:SelectOption/IEnumerable&lt;SelectOption>) | - |  |
| OnDeselect | 取消选中时调用，参数为选中项的 value (或 key) 值，仅在 multiple 或 tags 模式下生效 | function(string\|number\|LabeledValue) | - |  |
| OnFocus | 获得焦点时回调 | function | - |  |
| OnInputKeyDown | 按键按下时回调 | function | - |  |
| OnMouseEnter | 鼠标移入时回调 | function | - |  |
| OnMouseLeave | 鼠标移出时回调 | function | - |  |
| OnPopupScroll | 下拉列表滚动时的回调 | function | - |  |
| OnSearch | 文本框值变化时回调 | function(value: string) |  |  |
| OnSelect | 被选中时调用，参数为选中项的 value (或 key) 值 | function(string \|LabeledValue, option:SelectOption) | - |  |
| DefaultOpen | 是否默认展开下拉菜单 | Boolean | - |  |
| Open | 是否展开下拉菜单 | Boolean | - |  |
| OnDropdownVisibleChange | 展开下拉菜单的回调 | function(open) | - |  |
| Loading | 加载中状态 | Boolean | false |  |
| Bordered | 是否有边框 | Boolean | true |  |

> 注意，如果发现下拉菜单跟随页面滚动，或者需要在其他弹层中触发 Select，请尝试使用 `GetPopupContainer={triggerNode => triggerNode.parentElement}` 将下拉弹层渲染节点固定在触发器的父元素中。

### Select Methods

| 名称    | 说明     | 版本 |
| ------- | -------- | ---- |
| Blur()  | 取消焦点 |      |
| Focus() | 获取焦点 |      |

### SelectOption props

| 参数      | 说明                                     | 类型           | 默认值 | 版本 |
| --------- | --------------------------------------- | -------------- | ------ | ---- |
| Disabled  | 是否禁用                                 | Boolean        | false  |      |
| Title     | 选中该 SelectOption 后，Select 的 title  | string         | -      |      |
| Value     | 默认根据此属性值进行筛选                  | string          | -      |      |
| ClassName | SelectOption 器类名                     | string          | -      |      |

### SelectOptGroup props

| 参数  | 说明 | 类型                  | 默认值 | 版本 |
| ----- | ---- | --------------------- | ------ | ---- |
| Key   |      | string                | -      |      |
| Label | 组名 | string                | 无     |      |

## FAQ

### 点击 `dropdownRender` 里的内容浮层关闭怎么办？

看下 [dropdownRender 例子](#components-select-demo-custom-dropdown-menu) 里的说明。

### 自定义 SelectOption 样式导致滚动异常怎么办？

这是由于虚拟滚动默认选项高度为 `32px`，如果你的选项高度小于该值则需要通过 `ListItemHeight` 属性调整，而 `ListHeight` 用于设置滚动容器高度：

```razor
<Select ListItemHeight="10" ListHeight="250" />
```

注意：`ListItemHeight` 和 `ListHeight` 为内部属性，如无必要，请勿修改该值。
