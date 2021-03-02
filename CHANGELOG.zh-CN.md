---
order: 6
title: 更新日志
toc: false
timeline: true
---

`antd` 严格遵循 [Semantic Versioning 2.0.0](http://semver.org/lang/zh-CN/) 语义化版本规范。

#### 发布周期

- 修订版本号：每周末会进行日常 bugfix 更新。（如果有紧急的 bugfix，则任何时候都可发布）
- 次版本号：每月发布一个带有新特性的向下兼容的版本。
- 主版本号：含有破坏性更新和新特性，不在发布周期内。

---
## 0.7.0

`2021-03-02`

- 🚫 使用Func代替反射读写数据。[#1168](https://github.com/ant-design/ant-design/pull/1168) [@Zonciu](https://github.com/Zonciu)
- 🚫 支持列表筛选。[#1178](https://github.com/ant-design/ant-design/pull/1178) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module:select): click to select on new tag。[#1162](https://github.com/ant-design/ant-design/pull/1162) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 update docs。[536ba1a](https://github.com/ant-design/ant-design/commit/536ba1a) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module: tree) selected highlight confusion。[#1161](https://github.com/ant-design/ant-design/pull/1161) [@qinhuaihe](https://github.com/qinhuaihe)
- 🐞 fix(module:row): grid gutter fix。[#1158](https://github.com/ant-design/ant-design/pull/1158) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Removed unintended console output from SetGutterStyle。[#1159](https://github.com/ant-design/ant-design/pull/1159) [@superjerry88](https://github.com/superjerry88)
- 🐞 fix(module:inputpassword): focus fix。[#1146](https://github.com/ant-design/ant-design/pull/1146) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:cascader): OnChange called twice。[#1151](https://github.com/ant-design/ant-design/pull/1151) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 增加单元格编辑和行编辑的 demo。[#1152](https://github.com/ant-design/ant-design/pull/1152) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module:datepicker):date typing, enter behavior, overlay toggle。[#1145](https://github.com/ant-design/ant-design/pull/1145) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:table): set selectedRows exception。[#1148](https://github.com/ant-design/ant-design/pull/1148) [@qinhuaihe](https://github.com/qinhuaihe)
- 🐞 导航菜单折叠无响应。[#1144](https://github.com/ant-design/ant-design/pull/1144) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 ScrollX/ScrollY 增加更多长度单位的支持。[#1137](https://github.com/ant-design/ant-design/pull/1137) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module:select): new tag item label and value fix。[#1121](https://github.com/ant-design/ant-design/pull/1121) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:menu): active parent menu for routed links。[#1134](https://github.com/ant-design/ant-design/pull/1134) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 增加overlay的边界检测和方向调整。[#1109](https://github.com/ant-design/ant-design/pull/1109) [@mutouzdl](https://github.com/mutouzdl)
- 🐞 fix(module:select): property rename to follow docs。[#1115](https://github.com/ant-design/ant-design/pull/1115) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 给Cascader增加下拉效果（集成Overlay组件）。[#1112](https://github.com/ant-design/ant-design/pull/1112) [@mutouzdl](https://github.com/mutouzdl)
- 🚫 rename the docs project。[49a2d13](https://github.com/ant-design/ant-design/commit/49a2d13) [@ElderJames](https://github.com/ElderJames)
- 🐞 docs: fix anchor and improvement。[#1107](https://github.com/ant-design/ant-design/pull/1107) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复SortModel中丢失的Sort属性值。[#1105](https://github.com/ant-design/ant-design/pull/1105) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix(module:datepicker): for not nullable - on clear set to defaults。[#1100](https://github.com/ant-design/ant-design/pull/1100) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:datepicker): incorrect date format strings fix。[#1097](https://github.com/ant-design/ant-design/pull/1097) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 Fix pressing enter not updating the value。[#1094](https://github.com/ant-design/ant-design/pull/1094) [@Hona](https://github.com/Hona)
- 🐞 New `MondayIndex` property on `DatePickerLocale.cs` class that stores Monday index in `ShortWeekDays`.。[#1054](https://github.com/ant-design/ant-design/pull/1054) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 fix(module:rangepicker): placeholder and value equals null。[#1088](https://github.com/ant-design/ant-design/pull/1088) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 removed `AllowCustomTags` and `OnCreateCustomTag` <br>    added `PrefixIcon`。[#1087](https://github.com/ant-design/ant-design/pull/1087) [@anddrzejb](https://github.com/anddrzejb)
- 🚫 增加tooltip和 submenu浮层弹出触发类型。[#1082](https://github.com/ant-design/ant-design/pull/1082) [@ElderJames](https://github.com/ElderJames)
- 🚫 update the package version。[0e23bd0](https://github.com/ant-design/ant-design/commit/0e23bd0) [@ElderJames](https://github.com/ElderJames)
- 🚫 x。[#1077](https://github.com/ant-design/ant-design/pull/1077) [@MutatePat](https://github.com/MutatePat)
- 🚫 菜单增加   inline indent 属性。[#1076](https://github.com/ant-design/ant-design/pull/1076) [@ElderJames](https://github.com/ElderJames)
- 💄 修复 Steps 进度条样式。[#1072](https://github.com/ant-design/ant-design/pull/1072) [@ElderJames](https://github.com/ElderJames)
- 🚫 chore: sync ant-design v4.12.0。[#1067](https://github.com/ant-design/ant-design/pull/1067) [@ElderJames](https://github.com/ElderJames)
- 🐞 fix: steps navigation was blocked。[#1071](https://github.com/ant-design/ant-design/pull/1071) [@Tfurrer](https://github.com/Tfurrer)
- 🐞 修复菜单不能跟随Sider侧边栏收起的问题。[#1069](https://github.com/ant-design/ant-design/pull/1069) [@ElderJames](https://github.com/ElderJames)

## 0.6.0

`2021-02-01`

- Table
  - 🆕 增加 DataIndex特性，基于路径字符串的对象属性访问。[#1056](https://github.com/ant-design-blazor/ant-design-blazor/pull/1056) [@Zonciu](https://github.com/Zonciu)
  - 🆕 增加 RowClassName 属性[#1031](https://github.com/ant-design-blazor/ant-design-blazor/pull/1031) [@mostrowski123](https://github.com/mostrowski123)
  - 🆕 支持设置排序方向以及默认排序。[#778](https://github.com/ant-design-blazor/ant-design-blazor/pull/778) [@cqgis](https://github.com/cqgis)
  - 🆕 支持多列排序。[#1019](https://github.com/ant-design-blazor/ant-design-blazor/pull/1019) [@ElderJames](https://github.com/ElderJames)
  - 🆕 增加属性 ExpandIconColumnIndex ，可指定展开按钮所在列。[#1002](https://github.com/ant-design-blazor/ant-design-blazor/pull/1002) [@fan0217](https://github.com/fan0217)
  - 🐞 设置 ScrollY 时行选择抛异常。[#1020](https://github.com/ant-design-blazor/ant-design-blazor/pull/1020) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复ExpandTemplate 为null时，空数据时的样式错误。[#985](https://github.com/ant-design-blazor/ant-design-blazor/pull/985) [@Magehernan](https://github.com/Magehernan)
  - 🐞 表格组件添加自定义比较器, 修复表格复刻例子。[#969](https://github.com/ant-design-blazor/ant-design-blazor/pull/969) [@Zonciu](https://github.com/Zonciu)
  - 🐞 修复在页面重载时抛出的异常。[#1040](https://github.com/ant-design-blazor/ant-design-blazor/pull/1040) [@anddrzejb](https://github.com/anddrzejb)

- Menu
  - 🐞 修复相同链接的死循环以及重复高亮[#1027](https://github.com/ant-design-blazor/ant-design-blazor/pull/1027) [@anddrzejb](https://github.com/anddrzejb)
  - 🆕 增加菜单分割线组件 MenuDivider。[#1017](https://github.com/ant-design-blazor/ant-design-blazor/pull/1017) [@anddrzejb](https://github.com/anddrzejb)

- Overlay
  - 🆕 弹出层支持无须 div 包裹触发元素的实现方式，但需要使用<Unbound> 模板和使用RefBack方法。[#937](https://github.com/ant-design-blazor/ant-design-blazor/pull/937) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复delElementFrom()在页面重载时的异常。[#1008](https://github.com/ant-design-blazor/ant-design-blazor/pull/1008) [@anddrzejb](https://github.com/anddrzejb)
  - 🐞 修复 getFirstChildDomInfo 方法非空判断[#989](https://github.com/ant-design-blazor/ant-design-blazor/pull/989) [@Andrzej Bakun](https://github.com/Andrzej Bakun)

- DatePicker
  - 🐞 防止时间超出DateTime范围，导致异常[#973](https://github.com/ant-design-blazor/ant-design-blazor/pull/973) [@ElderJames](https://github.com/ElderJames)
  - 🐞 修复 DatePicker 当有默认值时抛出异常[#972](https://github.com/ant-design-blazor/ant-design-blazor/pull/972) [@ElderJames](https://github.com/ElderJames)

- 🆕 增加 Image 组件[#1038](https://github.com/ant-design-blazor/ant-design-blazor/pull/1038) [@ElderJames](https://github.com/ElderJames)
- 🆕 Card 增加操作按钮组件`CardAction`，可分别设置点击事件。[#1030](https://github.com/ant-design-blazor/ant-design-blazor/pull/1030) [@ElderJames](https://github.com/ElderJames)
- 🆕 Icon 增加静态的图标类型 `IconType`。[#987](https://github.com/ant-design-blazor/ant-design-blazor/pull/987) [@porkopek](https://github.com/porkopek)
- 🐞 修复 Input/InputNumber/TextArea 丢失的 disabled 属性。[#1048](https://github.com/ant-design-blazor/ant-design-blazor/pull/1048) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Form 修复重新绑定 model 或在调用 `Reset()` 方法不能清空验证错误信息的问题[#1035](https://github.com/ant-design-blazor/ant-design-blazor/pull/1035) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Tabs 修复选中指示器的跳动问题。[#1037](https://github.com/ant-design-blazor/ant-design-blazor/pull/1037) [@ElderJames](https://github.com/ElderJames)
- 🐞 Layout 修复 Sider 在 zero-width 模式时按钮丢失的问题[#1007](https://github.com/ant-design-blazor/ant-design-blazor/pull/1007) [@ElderJames](https://github.com/ElderJames)
- 💄 BackTop 修复可见/隐藏的样式[#1005](https://github.com/ant-design-blazor/ant-design-blazor/pull/1005) [@ElderJames](https://github.com/ElderJames)
- 💄 Upload 修复文件列表的样式[#1001](https://github.com/ant-design-blazor/ant-design-blazor/pull/1001) [@ElderJames](https://github.com/ElderJames)
- 🐞 Calendar 修复关于 ChangePickerValue 的错误[#993](https://github.com/ant-design-blazor/ant-design-blazor/pull/993) [@anddrzejb](https://github.com/anddrzejb)
- 💄 Alert 修复丢失 html 结构导致的样式问题[#990](https://github.com/ant-design-blazor/ant-design-blazor/pull/990) [@anddrzejb](https://github.com/anddrzejb)
- 🐞 Cascader 当 options 更新时重新初始化[#980](https://github.com/ant-design-blazor/ant-design-blazor/pull/980) [@imhmao](https://github.com/imhmao)
- 📖 发布文档时按版本号获取静态资源，使缓存更新。[cf2d4ed](https://github.com/ant-design-blazor/ant-design-blazor/commit/cf2d4ed) [@ElderJames](https://github.com/ElderJames)
- 💄 同步 ant-design-blazor v4.11.1 样式。[#1039](https://github.com/ant-design-blazor/ant-design-blazor/pull/1039) [@ElderJames](https://github.com/ElderJames)
- 🐞 修复在多个 Modal 同时存在时下拉选择等弹出组件无法弹出的问题。[#1012](https://github.com/ant-design-blazor/ant-design-blazor/pull/1012) [@mutouzdl](https://github.com/mutouzdl)
- 🛠 更新 bUnit 版本到 1.0.0-preview-01。[#1009](https://github.com/ant-design-blazor/ant-design-blazor/pull/1009) [@anddrzejb](https://github.com/anddrzejb)
- 📖 加载后自动滚动到Url锚点[#1006](https://github.com/ant-design-blazor/ant-design-blazor/pull/1006) [@ElderJames](https://github.com/ElderJames)