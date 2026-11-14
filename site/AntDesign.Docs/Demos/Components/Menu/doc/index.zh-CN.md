---
category: Components
cols: 1
type: 导航
title: Menu
subtitle: 导航菜单
cover: https://gw.alipayobjects.com/zos/alicdn/3XZcjGpvK/Menu.svg
---

为页面和功能提供导航的菜单列表。

## 何时使用

导航菜单是一个网站的灵魂，用户依赖导航在各个页面中进行跳转。一般分为顶部导航和侧边导航，顶部导航提供全局性的类目和功能，侧边导航提供多级结构来收纳和排列网站架构。

更多布局和导航的使用可以参考：[通用布局](/components/layout)。

## API

```html
<Menu>
  <Menu.Item>菜单项</Menu.Item>
  <SubMenu Title="子菜单">
    <Menu.Item>子菜单项</Menu.Item>
  </SubMenu>
</Menu>
```

### Menu

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Accordion | SubMenu 手风琴模式 | boolean | false |
| DefaultOpenKeys | 初始展开的 SubMenu 菜单项 key 数组 | string\[] |  |  |
| DefaultSelectedKeys | 初始选中的菜单项 key 数组 | string\[] |  |  |
| ForceSubMenuRender | 在子菜单展示之前就渲染进 DOM | boolean | false |  |
| InlineCollapsed | inline 时菜单是否收起状态 | boolean | - |  |
| InlineIndent | inline 模式的菜单缩进宽度 | number | 24 |  |
| Mode | 菜单类型，现在支持垂直、水平、和内嵌模式三种 | `vertical` \| `horizontal` \| `inline` | `vertical` |  |
| Multiple | 是否允许多选 | boolean | false |  |
| OpenKeys | 当前展开的 SubMenu 菜单项 key 数组 | string\[] |  |  |
| Selectable | 是否允许选中,当等于 `false` 时，会在触发 `OnClick` 之后不被选中 | boolean | true |  |
| SelectedKeys | 当前选中的菜单项 key 数组 | string\[] |  |  |
| Style | 根节点样式 | object |  |  |
| SubMenuCloseDelay | 用户鼠标离开子菜单后关闭延时，单位：秒 | number | 0.1 |  |
| SubMenuOpenDelay | 用户鼠标进入子菜单后开启延时，单位：秒 | number | 0 |  |
| Theme | 主题颜色 | `light` \| `dark` | `light` |  |
| TriggerSubMenuAction | SubMenu 展开/关闭的触发行为 | `hover` \| `click` | `hover` |  |
| OnClick | 点击 MenuItem 调用此函数 | function({ item, key, keyPath, domEvent }) | - |  |
| OnDeselect | 取消选中时调用，仅在 multiple 生效 | function({ item, key, keyPath, selectedKeys, domEvent }) | - |  |
| OnOpenChange | SubMenu 展开/关闭的回调 | function(openKeys: string\[]) | noop |  |
| OnSelect | 被选中时调用 | function({ item, key, keyPath, selectedKeys, domEvent }) | 无   |  |
| OverflowedIndicator | 自定义 Menu 折叠时的图标 | ReactNode | - |  |

> More options in [rc-menu](https://github.com/react-component/menu#api)

### MenuItem

| 参数          | 说明                                                       | 类型                              | 默认值   | 版本 |
|--------------|----------------------------------------------------------|---------------------------------|-------|----|
| Disabled     | 是否禁用                                                     | boolean                         | false |    |
| Key          | item 的唯一标志                                               | string                          |       |    |
| OnClick      | 当鼠标点击菜单项时触发                                              | EventCallback&lt;MouseEventArgs> | -     |    |
| RouterLink   | 路由链接，当需要让菜单自动匹配路由进行高亮时使用                                 | string                          | -     |    |
| RouterMatch  | 修改自 `NavLink`,用于选择匹配模式                                   | NavLinkMatch                    | -     |    |
| Style        | 额外的 CSS 样式                                               | string                          | -     |    |
| Target       |                                                          | MenuTarget?                     | -     |    |
| Title        | 设置收缩时展示的悬浮标题                                             | string                          |       |    |
| Icon         | 图标的类型                                                    | string                          | -     |    |
| IconTemplate | 自定义Icon模板，当`Icon`和`IconTemplate`同时设置时，优先使用`IconTemplate` | RenderFragment                  | -     |    |

### SubMenu

| 参数           | 说明           | 类型                        | 默认值 | 版本 |
| -------------- | -------------- | --------------------------- | ------ | ---- |
| ChildContent   | 放置子菜单的菜单项列表 | RenderFragment |        |      |
| Disabled       | 是否禁用       | boolean                     | false  |      |
| IsOpen         | 表示展开状态 | bool | false |  |
| Key            | 唯一标志       | string                      |        |      |
| OnTitleClick | 当标题被点击时触发的回调 | EventCallback&lt;MouseEventArgs> |  |  |
| PopupClassName | 子菜单样式     | string                      |        |      |
| Title          | 子菜单标题     | string          |        |      |
| TitleTemplate  | 子菜单标题模板  | RenderFragment          |        |      |

### MenuItemGroup

| 参数     | 说明         | 类型              | 默认值 | 版本 |
| -------- | ------------ | ----------------- | ------ | ---- |
| ChildContent | 分组的菜单项 | MenuItem\[]       |        |      |
| Style    | 额外的 CSS 样式  | string |     -          |         |
| Title    | 分组标题     | string\|ReactNode |        |      |
| TitleTemplate  | 分组标题模板  | RenderFragment          |        |      |

### Menu.Divider

菜单项分割线，只用在弹出菜单内。
