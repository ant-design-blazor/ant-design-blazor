---
category: Components
type: Data Display
title: Tree
cover: https://gw.alipayobjects.com/zos/alicdn/Xh-oWqg9k/Tree.svg
---

A hierarchical list structure component.

## When To Use

Almost anything can be represented in a tree structure. Examples include directories, organization hierarchies, biological classifications, countries, etc. The `Tree` component is a way of representing the hierarchical relationship between these things. You can also expand, collapse, and select a treeNode within a `Tree`.

## API

### Tree props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ShowExpand | Shows an expansion icon before the node | boolean | true |  |
| ShowLine | Shows a connecting line | boolean | false |  |
| Disabled | The tree is disabled | boolean | false |  |
| ShowIcon | show treeNode icon icon | boolean | false |  |
| Draggable | Whether the node allows drag and drop | boolean | false |  |
| BlockNode | Whether treeNode fill remaining horizontal space | boolean | false |  |
| ShowLeafIcon | Displays the cotyledon icon | boolean | false |  |
| SwitcherIcon | Customize toggle icon, the value is Icon types | string | null | |
| Selectable | Whether can be selected | boolean | true |  |
| DefaultSelectedKey | Specifies the key of the default selected treeNode | string | null |  |
| DefaultSelectedKeys | Specifies the keys of the default selected treeNodes | string[] | null |  |
| Multiple  |  Allows selecting multiple treeNodes | boolean | false  |   |
| Checkable |  Add a Checkbox before the node | boolean  | false  |   |
| CheckOnClickNode |  Click title to check or uncheck the node | boolean  | true  |   |
| CheckStrictly  |  Check treeNode precisely; parent treeNode and children treeNodes are not associated |  boolean | false  |   |
| DefaultCheckedKeys  | Specifies the keys of the default checked treeNodes |  string[] | null |   |
| DisableCheckKeys | Disable node Checkbox |  string[] |  null |   |
| SearchValue  | search value  | string  | null  |   |
| SearchExpression  | Customized matching expression  |  Func\<TreeNode\<TItem\>, bool\> | null  |   |
| MatchedStyle  | Search for matching text styles | string  | null  |   |
| MatchedClass | The  class name of matching text | string | null |  |
| HideUnmatched | Hides all nodes that are not matched by the search value | bool | false |  |
| DataSource | bing datasource | List  |  null  |   |
| TitleExpression  |  Specifies a method that returns the text of the node. | Func  |   |   |
| KeyExpression |  Specifies a method that returns the key of the node. |  Func |   |   |
| IconExpression  |  Specifies a method to return the node icon. | Func  |   |   |
| IsLeafExpression  | Specifies a method that returns whether the expression is a leaf node.  | Func  |   |   |
| ChildrenExpression  | Specifies a method  to return a child node  | Func  |   |   |
| DisabledExpression  |  Specifies a method to return a disabled node | Func  |   |   |
| CheckableExpression  |  Specifies a method to return a checkable node | Func  |   |   |
| DefaultExpandAll  |  All tree nodes are expanded by default |  boolean  | false  |   |
| DefaultExpandParent  |  The parent node is expanded by default | boolean  | false  |   |
| DefaultExpandedKeys  |  Expand the specified tree nodes by default | string[]  | null |   |
| ExpandedKeys  |  (Controlled) expands the specified tree node | string[]  |  null  |   |
| AutoExpandParent | Whether to automatically expand a parent treeNode | bool | false |  |
| ExpandOnClickNode |  Click title to expand or collapse the node | boolean  | false  |   |

### Bind °ó¶¨Öµ

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| @bind-SelectedKey | Bind the selected node's Key (You can also use DefaultSelectedKey for the default value) | string |   |  |
| @bind-SelectedNode | Bind the selected node (Use DefaultSelectedKey for the default value) | TreeNode<TItem> |  |  |
| @bind-SelectedData | Bind the selected node's data (Use DefaultSelectedKey for the default value) | string |  |  |
| @bind-SelectedKeys | Bind the keys of selected nodes (You can also use DefaultSelectedKeys for the default value) | string[] |  |  |
| @bind-SelectedNodes | Bind the selected nodes (Use DefaultSelectedKeys for the default value) | TreeNode<TItem>[] |  |  |
| @bind-SelectedDatas | Bind the datas of selected nodes (Use DefaultSelectedKeys for the default value) | TItem[] |  |  |
| @bind-CheckedKeys | Bind the keys of checked nodes (You can also use DefaultCheckedKeys for the default value) | string[] |  |  | 
| @bind-ExpandedKeys | Bind the keys of expanded nodes (You can also use DefaultExpandedKeys for the default value) | string[] |  |  | 

### Tree EventCallback

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| OnNodeLoadDelayAsync  |   Lazy load callbacks. You must use async and the return type is Task, otherwise you may experience load lag and display problems | EventCallback  |   |   |
| OnClick |  Click the tree node callback | EventCallback  |   |   |
| OnDblClick | Double-click the node callback  |  EventCallback |   |   |
| OnContextMenu  |  Right-click tree node callback | EventCallback  |  |   |
| OnCheckBoxChanged | checked the tree node callback  |  EventCallback |   |   |
| OnExpandChanged | Click the expansion tree node icon to call back  | EventCallback  |   |   |
| OnDragStart | Called when the drag and drop begins   | EventCallback  |   |   |
| OnDragEnter |  Called when drag and drop into a releasable target  | EventCallback  |   |   |
| OnDragLeave | Called when drag and drop away from a releasable target  | EventCallback  |   |   |
| OnDrop | Triggered when drag-and-drop drops succeed  | EventCallback  |   |   |
| OnDragEnd | Drag-and-drop end callback. this callback method must be set  | EventCallback  |   |   |

### Tree Functions

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ExpandAll  |  Expand all the tree nodes  | void  |   |   |
| CollapseAll |  Collapse all the tree nodes | void  |   |   |
| CheckAll  |  Recursively check all the tree nodes  | void  |   |   |
| UnCheckAll |  Recursively uncheck all the tree nodes | void  |   |   |
| SelectAll  |  Select all the tree nodes  | void  |   |   |
| DeselectAll |  Deselect all the tree nodes | void  |   |   |

### Tree RenderFragment

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| IndentTemplate  |   The indentation template  | RenderFragment  |   |   |
| TitleTemplate |  Customize the header template | RenderFragment  |   |   |
| TitleIconTemplate | Customize the icon templates  |  RenderFragment |   |   |
| SwitcherIconTemplate  |  Customize toggle icon templates | RenderFragment  |  |   |



### TreeNode props

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Key  |  key  | string  |   |   |
| Disabled |  disabled | string  |   |   |
| Checkable | Checkable  |  boolean |  true |   |
| Checked | checked (Supports two-way binding)  |  boolean |  false |   |
| DisableCheckbox |   |  boolean |  false |   |
| Selected | selected (Supports two-way binding)  |  boolean |  false |   |
| Expanded | expanded (Supports two-way binding)  |  boolean |  false |   |
| Title | title  |  string |  false |   |
| TitleTemplate | title template | RenderFragment | null |  |
| Icon | icon  |  string |  false |   |
| IconTemplate | icon template | RenderFragment | null |  |
| DataItem | dataitem |  Type |  |   | 
| SwitcherIcon | Customize node toggle icon, the value is Icon types  | string | null | |
| SwitcherIconTemplate | SwitcherIcon template | RenderFragment | null | |

### TreeNode Functions
| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| CheckAllChildren  |  Recursively check all child nodes (including the current node)  | void  |   |   |
| UnCheckAllChildren |  Recursively uncheck all child nodes (including the current node) | void  |   |   |