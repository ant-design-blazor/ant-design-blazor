# Design Token and runtime parity audit progress — 2026-11-22

This checklist tracks the focused React v6 to Blazor Design Token audit requested for this synchronization effort. “Token comparison complete” means the live React/Blazor token rows and generated source were compared; it does not mean the component's API, demos, Semantic DOM, or runtime/visual parity is complete.

Scope note: This inventory tracks the 68 components in the Ant Design React v6 core. ProComponents are outside this synchronization scope.

The broader current runtime evidence is in [runtime-parity-summary-2026-11-12.md](runtime-parity-summary-2026-11-12.md).

| Component | Current progress |
|---|---|
| Button | Token 对照完成；空描述按语言保留为空或中文说明，已通过 13 项 .NET 回归测试及 React/Blazor 浏览器表格对照 |
| FloatButton | Token 对照完成（无组件专属 Token；21 个全局 Token 名称、说明和值一致，数值精度及 rgba 空格为格式差异） |
| Icon | React 无组件/全局 Token 行；Blazor 空状态已改为仅显示与 React 一致的 `Design Token` 标题；5 个 demo 与 gallery 桌面/窄屏共 7 个 DOM/CSS/几何/视觉场景通过；`Icon.TwoToneColor` 新增 React 同形的主色/次色数组支持并保留字符串参数，双色测试 25/25、Docs.Server net11 构建通过；API、FAQ 与更大范围页面 parity 仍开放 |
| Typography | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Divider | Token 对照完成（3 个组件 Token、10 个全局 Token；名称、说明、值一致，C# 类型映射正确，格式化差异可归一化） |
| Flex | Token 对照完成（无组件专属 Token；3 个全局 Token 名称、说明、类型和值完全一致） |
| Grid | Token 对照完成（React v6 与生成源均无 Grid 组件/全局 Token；Blazor 文档提供“无专属 Token”的说明段） |
| Masonry | Token 对照完成（无组件专属 Token；3 个全局动画 Token 名称、说明、类型和值完全一致） |
| Space | Token 对照完成（无组件专属 Token；3 个全局 Token 名称、说明、类型和值完全一致） |
| Splitter | Token 对照完成（3 个组件 Token、16 个全局 Token；名称、说明和值一致，类型映射及格式差异已核对） |
| Anchor | Token 对照完成（2 个组件 Token、12 个全局 Token；名称、说明和值一致，C# 类型映射及格式差异已核对） |
| Breadcrumb | Token 对照完成（7 个组件 Token、12 个全局 Token；名称、说明和值一致，类型映射及格式化差异已核对） |
| Dropdown | Token 对照完成（2 个组件 Token、36 个全局 Token；名称、说明、类型和值一致，C# 类型映射正确） |
| Menu | Token 对照完成（56 个组件 Token、30 个全局 Token；名称、说明、类型和值一致；`itemBg` 英文说明为空与 React 文档一致，格式化差异可归一化） |
| Pagination | Token 对照完成（12 个组件 Token、37 个全局 Token；名称、说明、类型和值一致，`lineHeight` 浮点精度为格式差异） |
| Steps | Token 对照完成（11 个组件 Token、43 个全局 Token；名称、说明、类型和值一致，`#fff`/`#ffffff` 与浮点精度为格式差异） |
| Tabs | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| AutoComplete | Token 对照完成（24 个组件 Token、61 个全局 Token；别名映射已修复，net11 构建、9 项回归测试及 camel-case/小写路由浏览器验证通过） |
| Cascader | Token 对照完成（8 个组件 Token、31 个全局 Token；名称、说明、类型和值一致，`#fff`/`#ffffff` 与浮点精度为格式差异） |
| Checkbox | Token 对照完成（26 个组件 Token；名称、说明、类型和值匹配，`#fff`/`#ffffff` 与浮点精度为格式差异；React 文档无独立全局 Token 表） |
| ColorPicker | Token 对照完成（React v6 无组件专属 Token 文档表；Blazor 列出 47 个全局 Token，均在 React ColorPicker 样式源中使用；描述和值沿用全局定义） |
| DatePicker | Token 对照完成（40 个组件 Token、63 个全局 Token；修复 StyleGen 对官方继承 Token `addonBg` 的错误过滤，并通过 .NET 构建及浏览器验证） |
| Input | Token 对照完成（17 个组件 Token、25 个全局 Token；名称、说明、类型和值一致，`lineHeight` 浮点精度为格式差异） |
| InputNumber | Token 对照完成（26 个组件 Token、32 个全局 Token；名称、说明、类型和值一致，`lineHeight` 浮点精度为格式差异） |
| Mentions | Token 对照完成（20 个组件 Token、44 个全局 Token；名称、说明、类型和值一致，`lineHeight` 浮点精度为格式差异） |
| Radio | Token 对照完成（14 个组件 Token、28 个全局 Token；名称、说明、类型和值一致，`lineHeight` 浮点精度为格式差异） |
| Rate | Token 对照完成（6 个组件 Token、5 个全局 Token；名称、说明、类型和值一致，`lineHeight` 浮点精度为格式差异） |
| Select | React/Blazor 组件 Token 24 行、全局 Token 61 行的名称和描述一致，值只有 rgba/阴影空格及浮点精度格式差异；局部 `Direction` 覆盖 4/4 通过；修复局部容器 Overlay 漏监听窗口滚动，Select Semantic DOM 单选/多选预览的 DOM 树、计算样式、几何与视觉样式一致，滚动监听回归 2/2 通过；单选序列化 HTML 仍有仅空白字符差异；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Slider | Token 对照完成（18 个组件 Token、12 个全局 Token；名称、说明、类型和值一致，`lineHeight` 浮点精度为格式差异） |
| Switch | Token 对照完成（13 个组件 Token、15 个全局 Token；名称、说明、类型和值一致，`#fff`、浮点精度及小数前导零为格式差异） |
| TimePicker | Token 对照完成（React 复用 DatePicker 的 40 个组件 Token、63 个全局 Token；Blazor catalog 已增加 TimePicker 别名映射，并通过 .NET 构建、回归测试及浏览器验证） |
| Transfer | Token 对照完成（6 个组件 Token、32 个全局 Token；名称、说明、类型和值一致，`lineHeight` 浮点精度为格式差异） |
| TreeSelect | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Upload | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Avatar | Token 对照完成（12 个组件 Token、11 个全局 Token；名称、描述、默认值一致） |
| Badge | Token 对照完成（9 个组件 Token、18 个全局 Token；名称、描述、类型和值一致，归一化格式差异） |
| Calendar | Token 对照完成（6 个组件 Token、33 个全局 Token；完整匹配，类型和值差异仅为可归一化格式） |
| Card | Token 对照完成（13 个组件 Token、22 个全局 Token；类型映射正确，值仅有 CSS 空白格式差异） |
| Carousel | Token 对照完成（7 个组件 Token、7 个全局 Token；类型映射和值完整匹配） |
| Collapse | Token 对照完成（10 个组件 Token、23 个全局 Token；类型映射、描述和值一致） |
| Empty | Token 对照完成（无组件专属 Token；7 个全局 Token 名称、类型、说明和值一致，数值精度与 rgba 空格为格式化差异） |
| Image | Token 对照完成（6 个组件 Token、20 个全局 Token；名称、说明、类型和值一致；颜色空格与 #fff 展开为 #ffffff 属于格式差异） |
| List | Token 对照完成（12 个组件 Token、29 个全局 Token；名称、说明和值一致，C# 类型映射正确，数值精度与 rgba 空格为格式差异） |
| Popover | Token 对照完成（2 个公开组件 Token、13 个全局 Token；名称、说明、类型和值与 React 文档一致；未公开保留项不在两侧文档表中） |
| QRCode | Token 对照完成（无组件专属 Token；11 个全局 Token 名称、说明、类型和值一致，#fff 与 #ffffff 为格式差异） |
| Segmented | Token 对照完成（8 个组件 Token、24 个全局 Token；名称、说明和值一致，C# 类型映射正确，数字/rgba 格式差异可归一化） |
| Statistic | Token 对照完成（2 个组件 Token、8 个全局 Token；名称、说明、类型和值匹配，数字精度与 rgba 空格为格式差异） |
| Table | Token 对照完成；custom filter panel placement 根因已修复，浏览器 DOM/CSS/几何对照 2/2 通过；`Size` 映射为 Blazor `Default` 对应 React `large`；`FilterOnClose`、`DefaultFilteredValue`、`FilterResetToDefaultFilteredValue`、Column `MinWidth`、`SortIcon`、`ScrollToFirstRowOnChange` 已实现；新增 `ShowExpandAll` / `OnExpandAll`（当前页）及 `OnExpandedRowsChange`；API inventory 76 components / 2,824 parameters / 353 callbacks；TableTests 17/17、API map tests 7/7、Docs net11 build 均通过；13 个 Semantic DOM slot 和 53 个 React demo 名称已有对应；其他 API、demo 和完整视觉 parity 仍开放 |
| Tag | Token 对照完成（组件 Token 文档由通用文档框架生成）；Tag API map 21 exact / 3 alias / 1 missing / 6 shape notes，新增 Disabled/Customize close demo；Tag 测试 30/30、API map 测试 8/8、组件和文档构建通过；剩余 Semantic DOM、Demo 全量和浏览器视觉 parity 开放 |
| Timeline | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Tooltip | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Tour | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Tree | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Alert | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Drawer | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Message | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Modal | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Notification | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Popconfirm | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Progress | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Result | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Skeleton | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Spin | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Watermark | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| Affix | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| App | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| ConfigProvider | Token 对照完成；API、Demo、Semantic DOM 和运行时 parity 仍开放 |
| BorderBeam | Token 对照完成（无组件专属 Token；3 个全局 Token，与 React 文档的名称、说明及值一致） |

Progress: **68/68 core component Design Token audits verified**, **0 fixes pending verification**, **0 not yet audited**. Current structural work: Theme Preview Group DOM/CSS alignment has a passing focused structural regression test; broader theme-switch assertions remain separately under investigation. Table runtime page loads through the WebApp SSR + WebAssembly host; filter close, initial default values, Reset pending-selection behavior, and current-page expand-all behavior are verified. Next: continue Table API/Demo/Semantic DOM comparison, then finish Theme Preview interaction/visual comparison.

2026-11-23 BorderBeam demo续：BorderBeam 的组件 Token 审计仍为完成（0 组件 Token，3 全局 Token）；补齐 demo 引用与 React v6 正式示例顺序，并修复 JS helper 未同步宿主圆角导致的光束轨迹偏差。JS 几何测试 1/1、BorderBeam bUnit 2/2、Docs.Server net11 build 0 errors；组件其余 API/demo/Semantic DOM/runtime parity 未完成。
