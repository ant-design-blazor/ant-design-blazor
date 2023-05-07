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
| SwitcherIcon | Customize toggle icon£¬the value is Icon types | string | null | |
| Selectable | Whether can be selected | boolean | true |  |
| DefaultSelectedKeys | Specifies the keys of the default selected treeNodes | string[] | null |  |
| Multiple  |  Allows selecting multiple treeNodes | boolean | false  |   |
| Checkable |  Add a Checkbox before the node | boolean  | false  |   |
| CheckStrictly  |  Check treeNode precisely; parent treeNode and children treeNodes are not associated |  boolean | false  |   |
| DefaultCheckedKeys  | Specifies the keys of the default checked treeNodes |  string[] | null |   |
| DisableCheckKeys | Disable node Checkbox |  string[] |  null |   |
| SearchValue  | search value  | string  | null  |   |
| SearchExpression  | Customized matching expression  |  Func\<TreeNode\<TItem\>, bool\> | null  |   |
| MatchedStyle  | Search for matching text styles | string  | null  |   |
| MatchedClass | The  class name of matching text | string | null |  |
| DataSource | bing datasource | List  |  null  |   |
| TitleExpression  |  Specifies a method that returns the text of the node. | Func  |   |   |
| KeyExpression |  Specifies a method that returns the key of the node. |  Func |   |   |
| IconExpression  |  Specifies a method to return the node icon. | Func  |   |   |
| IsLeafExpression  | Specifies a method that returns whether the expression is a leaf node.  | Func  |   |   |
| ChildrenExpression  | Specifies a method  to return a child node  | Func  |   |   |
| DisabledExpression  |  Specifies a method to return a disabled node | Func  |   |   |
| DefaultExpandAll  |  All tree nodes are expanded by default |  boolean  | false  |   |
| DefaultExpandParent  |  The parent node is expanded by default | boolean  | false  |   |
| DefaultExpandedKeys  |  Expand the specified tree node by default | string[]  | null |   |
| ExpandedKeys  |  (Controlled) expands the specified tree node | string[]  |  null  |   |
| AutoExpandParent | Whether to automatically expand a parent treeNode | bool | false |  |

### Bind °ó¶¨Öµ

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| @bind-SelectedKey | SelectedKey | string |   |  |
| @bind-SelectedNode | SelectedNode | TreeNode |  |  |
| @bind-SelectedData | SelectedData | string |  |  |
| @bind-SelectedKeys | SelectedKeys | string[] |  |  |
| @bind-SelectedDatas | SelectedDatas | TItem[] |  |  |
| @bind-CheckedKeys | CheckedKeys | string[] |  |  | 

### Tree EventCallback

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| OnNodeLoadDelayAsync  |   Lazy load callbacks ¡£You must use async and the return type is Task, otherwise you may experience load lag and display problems | EventCallback  |   |   |
| OnClick |  Click the tree node callback | EventCallback  |   |   |
| OnDblClick | Double-click the node callback  |  EventCallback |   |   |
| OnContextMenu  |  Right-click tree node callback | EventCallback  |  |   |
| OnCheckBoxChanged | checked the tree node callback  |  EventCallback |   |   |
| OnExpandChanged | Click the expansion tree node icon to call back  | EventCallback  |   |   |
| OnDragStart | Called when the drag and drop begins   | EventCallback  |   |   |
| OnDragEnter |  Called when drag and drop into a releasable target  | EventCallback  |   |   |
| OnDragLeave | Called when drag and drop away from a releasable target  | EventCallback  |   |   |
| OnDrop | Triggered when drag-and-drop drops succeed  | EventCallback  |   |   |
| OnDragEnd | Drag-and-drop end callback ¡£this callback method must be set  | EventCallback  |   |   |

### Tree Functions

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| ExpandAll  |  All tree nodes are expanded by default  | void  |   |   |
| CollapseAll |  The parent node is expanded by default | void  |   |   |

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
| Checked | checked  |  boolean |  false |   |
| DisableCheckbox |   |  boolean |  false |   |
| Title | title  |  string |  false |   |
| TitleTemplate | title template | RenderFragment | null |  |
| Icon | icon  |  string |  false |   |
| IconTemplate | icon template | RenderFragment | null |  |
| DataItem | dataitem |  Type |  |   | 
| SwitcherIcon | Customize node toggle icon £¬the value is Icon types  | string | null | |
| SwitcherIconTemplate | SwitcherIcon template | RenderFragment | null | |