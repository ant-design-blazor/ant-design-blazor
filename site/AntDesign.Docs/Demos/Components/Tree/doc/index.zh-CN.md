---
category: Components
type: 数据展示
title: Tree
subtitle: 树形控件
cover: https://gw.alipayobjects.com/zos/alicdn/Xh-oWqg9k/Tree.svg
---

多层次的结构列表。

## 何时使用

文件夹、组织架构、生物分类、国家地区等等，世间万物的大多数结构都是树形结构。使用 `树控件` 可以完整展现其中的层级关系，并具有展开收起选择等交互功能。

## API

### Tree props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| ShowExpand | 显示展开图标 | boolean | true |  |
| ShowLine | 显示连接线 | boolean | false |  |
| Disabled | 禁用 | boolean | false |  |
| ShowIcon | 显示节点图标 | boolean | false |  |
| Draggable | 是否允许拖拽 | boolean | false |  |
| BlockNode | 是否节点占据一行 | boolean | false |  |
| ShowLeafIcon | 显示子叶图标（如果 ShowLeafIcon 未赋值, 会等于 `ShowLine` 的值） | boolean | false |  |
| SwitcherIcon | 设置所有节点的展开图标，值 Icon 的 Type | string | null | |
| Selectable |是否可以选择 | boolean | true |  |
| Multiple  |  允许选择多个树节点 | boolean | false  |   |
| Checkable |  节点前添加 Checkbox 复选框 | boolean  | false  |   |
| CheckOnClickNode |  点击节点标题选中或取消选中节点 | boolean  | true  |   |
| CheckStrictly  |  checkable 状态下节点选择完全受控（父子节点选中状态不再关联） |  boolean | false  |   |
| DefaultSelectedKey | 默认选中的节点Key | string | null |  |
| DefaultSelectedKeys | 默认选中的多个节点的key | string[] | null | |
| DefaultCheckedKeys  | 默认勾选的多个节点的key |  string[] | null |   |
| DisableCheckKeys | 默认禁用的勾选节点 |  string[] |  null |   |
| SearchValue  | 搜索节点关键字  | string  | null  |   |
| SearchExpression  | 自定义搜索匹配方法  |  Func\<TreeNode\<TItem\>, bool\> | null  |   |
| MatchedStyle  | 搜索匹配关键字高亮样式 | string  | null  |   |
| MatchedClass  | 搜索匹配关键字高亮样式 | string  | null  |   |
| HideUnmatched | 隐藏所有不匹配关键字的节点 | bool | false |  |
| DataSource | 数据源 | List  |  null  |   |
| TitleExpression  |  指定一个方法，该表达式返回节点的文本。 | Func  |   |   |
| KeyExpression |  指定一个返回节点Key的方法。 |  Func |   |   |
| IconExpression  |  指定一个返回节点名称的方法。 | Func  |   |   |
| IsLeafExpression  | 返回一个值是否是页节点  | Func  |   |   |
| ChildrenExpression  | 返回子节点的方法  | Func  |   |   |
| DisabledExpression  |  指定一个返回禁用节点的方法 | Func  |   |   |
| CheckableExpression  |  指定一个返回可勾选节点的方法 | Func  |   |   |
| DefaultExpandAll  |  默认展开所有节点 |  boolean  | false  |   |
| DefaultExpandParent  |  默认展开顶级父节点 | boolean  | false  |   |
| DefaultExpandedKeys  |  默认展开的节点 | string[]  | null |   |
| ExpandedKeys  |  （受控）展开指定的树节点 | string[]  |  null  |   |
| AutoExpandParent | 是否自动展开父节点 | bool | false | |
| ExpandOnClickNode |  点击节点标题展开或收缩节点 | boolean  | false  |   |


### Bind 绑定值

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| @bind-SelectedKey | 绑定选择的节点的Key值 (也可使用DefaultSelectedKey设置初始值) | string |   |  |
| @bind-SelectedNode | 绑定选择的节点 (使用DefaultSelectedKey设置初始值) | TreeNode<TItem> |  |  |
| @bind-SelectedData | 绑定选择节点的数据 (使用DefaultSelectedKey设置初始值) | string |  |  |
| @bind-SelectedKeys | 绑定多个选择的节点的Key值 (也可使用DefaultSelectedKeys设置初始值) | string[] |  |  |
| @bind-SelectedNodes | 绑定选择的多个节点 (使用DefaultSelectedKeys设置初始值) | TreeNode<TItem>[] |  |  |
| @bind-SelectedDatas | 绑定选择的多个节点的数据 (使用DefaultSelectedKeys设置初始值) | TItem[] |  |  |
| @bind-CheckedKeys | 绑定多个勾选的节点的Key值 (也可使用DefaultCheckedKey设置初始值) | string[] |  |  | 
| @bind-ExpandedKeys | 绑定多个展开的节点的Key值 (也可使用DefaultExpandedKey设置初始值) | string[] |  |  | 
### Tree EventCallback

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| OnNodeLoadDelayAsync  |  异步加载书回调，方法异步使用 async  | EventCallback  |   |   |
| OnClick |  点击节点回调 | EventCallback  |   |   |
| OnDblClick | 双击节点回调  |  EventCallback |   |   |
| OnContextMenu  |  右键回调 | EventCallback  |  |   |
| OnCheckBoxChanged | 节点勾选回调  |  EventCallback |   |   |
| OnExpandChanged | 展开节点回调  | EventCallback  |   |   |
| OnDragStart | 拖拽开始回调  | EventCallback  |   |   |
| OnDragEnter | 拖拽开始进入目标节点回调  | EventCallback  |   |   |
| OnDragLeave | 拖拽离开目标节点回调  | EventCallback  |   |   |
| OnDrop | 拖拽录入目标节点回调  | EventCallback  |   |   |
| OnDragEnd | 拖拽结束回调（此回调方法必须设置）  | EventCallback  |   |   |

### Tree Functions

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| ExpandAll  |  展开所有树节点  | void  |   |   |
| CollapseAll |  折叠所有树节点 | void  |   |   |
| CheckAll  |  递归勾选所有树节点  | void  |   |   |
| UnCheckAll |  递归取消勾选所有树节点 | void  |   |   |
| SelectAll  |  选择所有树节点  | void  |   |   |
| DeselectAll |  取消选择所有树节点 | void  |   |   |

### Tree RenderFragment

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| IndentTemplate  |  自定义缩进模板  | RenderFragment  |   |   |
| TitleTemplate |  自定义标题模板 | RenderFragment  |   |   |
| TitleIconTemplate | 自定义标题Icon  |  RenderFragment |   |   |
| SwitcherIconTemplate  |  自定义展开图标 | RenderFragment  |  |   |


### TreeNode props

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Key  |  节点key  | string  |   |   |
| Disabled |  是否禁用 | string  |   |   |
| Checkable | 是否可勾选  |  boolean |  true |   |
| Checked | 是否勾选（支持双向绑定）  |  boolean |  false |   |
| DisableCheckbox | 禁用复选框  |  boolean |  false |   |
| Selected | 是否选中（支持双向绑定）  |  boolean |  false |   |
| Expanded | 是否展开（支持双向绑定）  |  boolean |  false |   |
| Title | 标题  |  string |  false |   |
| TitleTemplate | 标题模板 | RenderFragment | null |  |
| Icon | 标题前图标  |  string |  false |   |
| IconTemplate | 标题前图标模板 | RenderFragment | null |  |
| DataItem | 数据项  |  Type |  |   | 
| SwitcherIcon | 该节点的展开图标 | string | null | |
| SwitcherIconTemplate | 该节点的展开图标 | RenderFragment | null | |

### TreeNode Functions
| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| CheckAllChildren  |  递归勾选当前节点下的所有子节点（含当前节点）  | void  |   |   |
| UnCheckAllChildren |  递归取消勾选当前节点下的所有子节点（含当前节点） | void  |   |   |