---
category: Components
subtitle: 穿梭框
type: 数据录入
cols: 1
title: Transfer
cover: https://gw.alipayobjects.com/zos/alicdn/QAXskNI4G/Transfer.svg
---

双栏穿梭选择框。

## 何时使用

- 需要在多个可选项中进行多选时。
- 比起 Select 和 TreeSelect，穿梭框占据更大的空间，可以展示可选项的更多信息。

穿梭选择框用直观的方式在两栏中移动元素，完成选择行为。

选择一个或以上的选项后，点击对应的方向键，可以把选中的选项移动到另一栏。其中，左边一栏为 `source`，右边一栏为 `target`，API 的设计也反映了这两个概念。

## API

### Transfer

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| DataSource | 数据源，其中的数据将会被渲染到左边一栏中，`targetKeys` 中指定的除外。 | List\<TransferItem> | null |  |
| Disabled | 是否禁用 | bool | false |  |
| Footer | 底部渲染函数 | OneOf<string, RenderFragment> | null |  |
| Style | 修改整个组件的样式 | string |  |  |
| Operations | 操作文案集合，顺序从上至下 | string\[] | \['right', 'left'] |  |
| Render | 每行数据渲染函数，该函数的入参为 `DataSource` 中的项，返回值为 OneOf<string, RenderFragment>。 | Func<TransferItem, OneOf<string, RenderFragment>> |  |  |
| SelectedKeys | 设置哪些项应该被选中 | string\[] | \[] |  |
| ShowSearch | 是否显示搜索框 | bool | false |  |
| ShowSelectAll | 是否展示全选勾选框 | bool | true |  |
| TargetKeys | 显示在右侧框数据的 key 集合 | string\[] | \[] |  |
| Titles | 标题集合，顺序从左至右 | string\[] | \['', ''] |  |
| Locale | 语言配置，包括过filter，空文本，项目单元等 | TransferLocale |  |  |
| ListStyle | 两个穿梭框的自定义样式 | Func<TransferDirection,string> | | |
| OnChange | 选项在两栏之间转移时的回调函数 | TransferSelectChangeArgs |  |  |
| OnScroll | 选项列表滚动时的回调函数 | TransferScrollArgs |  |  |
| OnSearch | 搜索框内容时改变时的回调函数 | TransferSearchArgs | - |  |
| OnSelectChange | 选中项发生改变时的回调函数 | TransferSelectChangeArgs |  |  |

### 自定义面板内容

`ChildContent` 模板会在左右两个面板中渲染，并接收 `TransferSidePanelContext<TransferItem>`。上下文提供当前面板的 `DataSource`、`Direction`、`SelectedKeys`、`TargetKeys`、`Disabled` 和 `DefaultContent`。自定义内容可通过 `SelectItemAsync` 更新选择状态。请参阅表格穿梭框和树穿梭框示例。

## 注意

按照 Blazor 的[规范](http://facebook.github.io/react/docs/lists-and-keys.html#keys)，所有的组件数组必须绑定 key。在 Transfer 中，`DataSource`里的数据值需要指定 `Key` 值。对于 `DataSource` 默认将每列数据的 `Key` 属性作为唯一的标识。

如果你的数据没有这个属性，务必使用 `rowKey` 来指定数据列的主键。
