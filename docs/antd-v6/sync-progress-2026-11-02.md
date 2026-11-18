# v6 同步进度总表 — 2026-11-02

## 2026-11-09 按历史差异量从高到低：Table / Basic

- 从历史 HTML/CSS/几何差异总量最高的 ProTable（Blazor 路由实际是 Table）开始。React Basic 是固定五列；Blazor Basic 原来额外展示“Dynamic columns”复选框，并与 React 的 tags、数据地址和布局不同。已将其改为固定 Name/Age/Address/Tags/Action 五列，示例数据、标签大写/颜色、Flex 小间距和操作链接按 React 同步。
- Docs.Server net10 构建通过（0 errors）。重启 Development 服务后，浏览器确认 Blazor Basic 的五个列标题、三行内容及标签文字与 React 页面相同，复选框已移除。
- 按 React Basic 收敛表格结构和语义：普通表格不再输出多余 colgroup；列标题避免不必要的包装 span，加入 `scope="col"`；`data-row-key` 使用公开 RowKey 值；仅响应式表格输出 `data-label`。同时调整 Flex/Space class 输出与表格默认 layout、分页对齐类名。
- 最终定向 fixture `/tmp/protable-basic-parity-20261109-dom-final.json`：1 scenario、0 blocked；标准化 DOM tree 相同，选定计算样式差异 0，visual style 通过。原始 HTML 字节门禁仍因 Razor 标题模板的换行/缩进空白而不同；浏览器渲染后的 DOM 结构和文本一致。
- Docs.Server net10 构建通过（0 errors）。重载 `http://127.0.0.1:5052/en-US/components/Table#components-table-demo-basic` 后确认 5 个列标题、3 行数据及分页 class `ant-table-pagination-end`。本轮仅完成 Basic 基线，Table 其余 demos、交互状态、Design Token 与 Semantic DOM 仍需逐项核对；Dynamic demo 的远程 API 曾返回 403，因此没有把整页每个 demo 都计为通过。

### Table / Row Selection

- 新增可复用对照 fixture `tools/AntDesign.StyleGen/examples/table-row-selection-dom-style-parity.json`。移除无宽度数据列生成的空 `<col>`，省略选择单元格默认 span 与空 style；保留 React 的选择列 colgroup 和表头 `.ant-table-selection` wrapper。选择框增加 `aria-label`，将 Checkbox DOM 调整为 React v6 的直接绘制结构（span 内只保留 input）。
- 为行选择的 `Selection` 增加 `GetCheckboxProps` 回调，传入源数据记录并支持返回 checkbox 属性；Row Selection demo 用它输出与 React 相同的记录 `name`。
- net10 Docs.Server 构建通过（0 errors）。最终 fixture `/tmp/table-row-selection-parity-20261109-final.json`：1 scenario、0 blocked；标准化 DOM tree 完全相同，根/节点选定 computed styles 相同，视觉样式通过（包括选择列宽为 32px）。Raw HTML 字符串仅因属性序列化顺序不同未逐字相等。浏览器当前停留于 Table 行选择文档供检查；没有将手动勾选交互计为通过。

### Table / Pagination placement

- 将 Table 分页示例改为 React v6 的上下两组 Radio 控件、独立 top/bottom placement，并对齐同一组三行、五列数据。新增 `PaginationPlacement` 数组参数，支持独立显示上/下分页、`none` 和 placement 对应的 start/center/end 对齐；保留原有 `PaginationPosition` 用法。
- 定向 fixture `/tmp/table-current-parity-20261109-verified.json` 中 pagination placement 场景：标准化 DOM tree、选定 computed styles、视觉样式均相同；原始 HTML 仅有序列化差异。Row Selection 场景持续通过 DOM tree 和样式检查。
- net10 Docs.Server 构建通过（0 errors）。

### Table / Multiple Sorter

- 为 Multiple Sorter demo 补充稳定的 `RowKey`，并让可排序表头输出 React v6 的键盘/辅助技术语义；排序状态在 body cell 上也保留 `.ant-table-column-sort`。
- 修复多列排序优先级：按 `SorterMultiple` 从高到低应用排序，高优先级列作为主键，之后的排序器作为次级键；树表子行排序使用同一优先级顺序。
- 新增 `tools/AntDesign.StyleGen/examples/table-multiple-sorter-dom-style-parity.json`，涵盖初始状态和依次点击 Chinese/Math 两列后的交互状态。net10 Docs.Server 构建通过（0 errors）；`/tmp/table-multiple-sorter-parity-20261109-priority-fixed.json` 两个场景均无阻断，DOM tree、选定 computed styles 与 visual styles 全部一致。原始 HTML 字节仍因 Razor 空白/序列化不同而不一致。
- Fixed Columns demo 已将列数、列标题、左右固定列、数据、无分页、`max-content` 横向滚动和 RowKey 改为 React v6 的示例配置。组件同步修复 `max-content` 列宽分配、ScrollX 的空列输出、测量行高度/内容及不必要的 `font-size: 0`。
- Fixed Columns 已切换到 React v6 `fix-start/end` class、逻辑 `inset-inline-*` 与 `--z-offset*`；滚动阴影通过 `tableHelper` 切换 v6 shadow-show class。另修复 PropertyColumn 在测量行中未解析字段名的问题。通过更新本地生成 bundle 后，fixture `/tmp/table-fixed-columns-parity-20261109-v10.json` 仅剩 React 动态 `acss-lej3nf` wrapper class 的 DOM 差异，computed/visual styles 差异为 0。
- Fixed Header demo 已对齐 React 的三列、100 条数据、50 条分页、275px 固定高度和地址内容，去除多余选择列；追加响应式滚动条样式。修复 scrollbar 测量为 0 时仍渲染空白占位列，并让纵向滚动时 body colgroup 与 React 一样省略无显式宽度的流式列，header colgroup 保留该列；header/body 表格样式和滚动容器样式已对齐。
- 最新 fixture `/tmp/table-fixed-header-parity-20261109-v5.json`：1 scenario、0 blocked；根节点与选定节点 computed styles、visual styles 差异均为 0。将 React 的运行时 `acss-*` hash class 从结构比较中标准化后，剩 5 处 DOM 差异：header fluid col 的动态 inline width（最终计算宽度一致）、分页 Select 的 search/ARIA/root style，以及 Blazor 常驻的隐藏 dropdown subtree。后三类归属分页器内 Select DOM 行为，后续可在 Select/Pagination 集成检查中继续处理。
- 按标点/大小写归一化后，React Table 53 个 TSX demo 均有对应 Razor demo 文件；此处是文件覆盖核验，不代表 53 个 demo 的运行结构、交互和样式都已逐一通过。
- 开始核对 Fixed Columns and Header：Blazor demo 已从原先 11 列和两条记录改为 React 的 23 列、100 条数据、左右固定列、`max-content` 横向滚动与 275px 表头滚动。发现 ScrollY + ScrollX 同时使用时，Blazor header table 漏了 `width: max-content`；已补齐 header 宽度策略。
- 为 ScrollX + ScrollY 分离表格实现列宽同步：ResizeObserver 观察 body table，读取 body `<col>` 实际布局宽度并更新 header `<col>`，并在窗口/表格滚动时同步。未写入固定 viewport 宽度。
- 新 fixture `/tmp/table-fixed-columns-header-parity-20261109-v6.json`：computed/visual styles 差异 0，DOM 差异由最初 19 降至 6，余项都在分页 Select（show-search/ARIA、root style 和隐藏 dropdown subtree）。Fixed Header 回归 `/tmp/table-fixed-header-parity-20261109-v6.json` 仍为 computed/visual styles 差异 0、5 处分页 Select/动态 DOM 差异。
- 这轮 `dotnet build site/AntDesign.Docs.Server/AntDesign.Docs.Server.csproj --no-restore -v:q` 通过（0 errors）。`npx gulp ts` 仍因 gulp task TypeScript 检查找不到 `build/compile-styles`、`build/generate-less-vars`、`build/migration-styles` 而失败；为本地验证使用 browserify + tsify + babelify 从受跟踪的 TS 源重建了 gitignore 的测试 bundle。该 bundle 本身不作为仓库改动提交。
- `dotnet build site/AntDesign.Docs.Server/AntDesign.Docs.Server.csproj --no-restore -v:q` 通过（0 errors）。本轮首个直接 `dotnet run` 使用了不存在的 launch profile，导致 Development JS 未加载、fixture 被阻断；改用 `AntDesign.Docs.Server` profile 并固定 `ASPNETCORE_URLS=http://127.0.0.1:5000` 后，fixture 正常运行。浏览器现停留在 Table 文档。
- `dotnet build site/AntDesign.Docs.Server/AntDesign.Docs.Server.csproj --no-restore` 通过（0 errors）。`npx gulp build:library` 因当前工作区缺少 `build/compile-styles`、`build/generate-less-vars`、`build/migration-styles` 模块失败；v6 文档使用的 Light theme CSS 已包含 React 固定列样式规则。
- Table 本轮继续校准分页尺寸选择器：分页 Select 增加 `Page Size` aria-label、搜索框、body popup 容器和关闭后销毁隐藏弹层；用 LabelTemplate 与显式 selected content title 对齐 React 的 `N / page` 选中显示。Docs.Server net10 构建通过（0 errors），重启后浏览器 HTTP 200。Fixed Header 和 Fixed Columns and Header 两个 parity 场景 computed/visual styles 均 0 差异；DOM 差异分别从 3/4 降至 1/2。剩余是流体列动态 inline 宽度，以及 Fixed Columns and Header 的两个分页/模板结构差异。报告 `/tmp/table-fixed-combined-final.json`；`git diff --check` 通过。
- Table 当前已定向覆盖 Basic、Row Selection、Pagination placement、Multiple Sorter、Fixed Columns、Fixed Header、Fixed Columns and Header；后续继续检查余下 demos 的实际 DOM/CSS/交互、Design Token 与 Semantic DOM。

## 2026-11-08 主题菜单重开：浏览器运行时补齐

- 浏览器复测发现主题按钮原先并非被关闭状态锁住；Overlay 打开时抛出 `ReferenceError: regeneratorRuntime is not defined`，导致 Blazor Server circuit 断开，之后任何菜单点击都失效。
- 将 `regenerator-runtime/runtime` 显式打入 interop 主 bundle，并在根 `package.json` 声明构建依赖。重新生成 bundle 后重载文档页，控制台不再报错；真实点击“打开 → 选 Light theme 关闭 → 再点打开”成功，菜单项完整显示。页面现保持主题菜单打开供人工检查。
- Select Semantic DOM 的容器边界修复和 runner 子像素对齐调查仍在进行；继续处理 Select 的余项，然后转到 Dropdown。

## 2026-11-08 Select Semantic DOM Overlay 边界修复

- Overlay 在自定义挂载容器下曾用容器自身 `scrollHeight` 判断垂直空间；Select Semantic DOM 的容器只有 80px 高，导致弹层误翻到触发器上方。现在非 body 容器的垂直计算使用 `containerBoundarySize.bottom` 的局部坐标，与同一边界交集参与翻转判断。
- 定向 React/Blazor fixture `/tmp/select-semantic-position-fixed-20261108.json` 复测：两边均为 `bottomLeft`，popup 计算样式相同（top 84px、bottom -108px、宽 300px、高 104px），触发器与弹层间距均为 4px，所选计算样式差异为 0。严格几何项仍报告 0.671875px 子像素差：runner 把两端触发器对齐至整数 viewport y=740 后，React/Blazor 根元素各自保留不同小数部分；尝试小数目标暴露出 runner 对嵌套滚动容器重复累计偏移的问题，故这项不记为通过。
- Docs.Server net10 构建成功（0 errors；存在现有 NuGet 安全/重复引用警告）。`http://127.0.0.1:5052/en-US/components/select#components-select-demo-_semantic` Development 服务已重启并保持运行。下一步修正 parity runner 的小数坐标对齐，再核实该 geometry 项；然后继续 Select 其余 demo，并转到 Dropdown。
- 复核 runner 的 portal anchor 定义后，确认本例的 `pixel-snap-to-trigger` 会把触发器 viewport 的小数位置当作跨页面绝对坐标差异；实际实现契约是两端 popup 均与触发器下沿相距 4px（相对几何 `top=36px`）。在整数 y=740 对齐时，两侧样例根节点分别保留 0.67px 子像素差，但 popup 与各自 trigger 的间距都准确为 4px，Overlay 计算样式相同。移除对齐参数的临时重跑还触发了 Blazor 页面滚动位置异常（popup 被计算到 41,964px），说明 runner 需要在对照滚动容器/主体滚动时隔离页面滚动状态；不能将该次结果用于组件结论。
- 根因已修复：文档把 `body` 设成 `overflow:auto`，但 Overlay 又按普通裁剪祖先计算其滚动后的负 `getBoundingClientRect()`，污染了 viewport 与局部挂载容器的交集。跳过 `body/html` 的普通裁剪计算后，定向 fixture `/tmp/select-semantic-relative-anchor-result-20261108.json` 对照两端都为 `bottomLeft`，相对 trigger 的几何完全一致（left 0、top 36、宽 300、高 104），top/bottom 为 `84px/-108px`，视觉样式差异 0。Runner 若不把根锚点滚入可视区，仍会因预览容器裁剪而无法代表正常用户状态，因此后续保留显式视口对齐。文档浏览器已打开 Select 的 Flip and shift / Semantic DOM 示例区域供手工检查。

## 2026-11-08 主题切换二次打开回归

- `DocsThemeSwitch` 的 Dropdown 使用常规非受控开关；主题菜单设置 `AutoCloseDropdown="false"`，由主题项处理器关闭 Overlay，避免菜单和主题处理器重复关闭。
- `AntDesign.Docs.Server` net10 完整项目引用构建通过（0 errors）。在 Development 文档服务和 1440px 桌面 Chromium 视口中实测：触发器打开、再次点击关闭、第三次点击重新打开；选择 Dark theme 后菜单关闭，右上角触发器再次打开成功，`data-theme="dark"` 且样式表为 `dark.css`。浏览器验证通过。

## 2026-11-08 主题菜单回归与 Select Semantic DOM 复核

- 主题菜单移除与 Dropdown 内部状态重复的受控 `Open`/`_open`；主题选项仍通过 Dropdown 引用的 `Close()` 关闭。Docs.Server net10 构建成功（0 errors）；浏览器验证“选择主题后关闭→再次打开”以及“点击外部关闭→再次打开”均通过。服务保持在 `http://127.0.0.1:5050`，Select 页面标签保持打开。
- Select Semantic DOM 预览内容列按 React 的 `align-items:flex-start` 对齐。整页自动盘点报告 `/tmp/select-doc-inventory-semantic-align-20261108.json`：React/Blazor 28/51 demos，缺失 React demo 0、Blazor 额外 23；HTML/CSS/几何差异 demo 数 1/9/1，React hydration 阻塞 1 页。Semantic DOM demo 仍有 156 个序列化/结构项差异、3 个计算样式差异和 13 个几何差异；popup 仍向上偏移 36px，需继续定位容器锚点与高度。顶部对齐改动没有降低这些差异计数，不能算作该 demo 已收敛。
- Icon 五个 canonical demo 最近结果仍为 5/5 DOM、CSS、几何及视觉样式通过；842 图标列表和交互状态还没有全量验证。下一步继续 Select Semantic DOM 的 Overlay 几何差异，再处理 Select 剩余 CSS demo，之后进入 Dropdown。

## 2026-11-08 Select Responsive / Custom Tag Render 收敛

- Responsive 的 0.016px suffix 差异来自聚合标签 Razor 文本中的缩进空白；改为单个插值字符串后，定向 DOM/CSS/几何/视觉对比 1/1 通过，0 differences、0 blocked。报告：`/tmp/select-responsive-expression-fix-20261108.json`。
- Icons 生成包的 catalog SVG 根属性修复后，Select Custom Tag Render 的 `fill-rule="evenodd"` 差异消失，定向对比 1/1 通过，0 differences、0 blocked。报告：`/tmp/select-custom-tag-render-final-20261108.json`。Icon 五个 canonical groups 为 5/5 零差异：`/tmp/icon-dom-style-parity-current-20261108.json`。
- 下方全站表中的 Select HTML/CSS/几何 demo 计数仍为历史全量基线，不是本轮重扫后的全页统计；Responsive 与 Custom Tag Render 已单独收敛。下一步扫描 Select 尚余候选 demo，再转向下一个组件 Dropdown。

## 2026-11-08 Select Size / Variant / Placement 定向复核

- React Select 的 `size` 和 `placement` 示例都用公开 API 字符串值。Blazor demo 已改为相同的小写 `large`/`medium`/`small`、`topLeft`/`topRight`/`bottomLeft`/`bottomRight`，再在 demo 内映射到 .NET enum；Variant demo 的 8 个 Select 现与 React 一样使用四组变体和 `flex: 1 1 0%`。
- 使用 React 文档 `:8000` 和 Blazor Development 文档 `:5050` 重跑六个场景，Size default/large/small、Variant default、Placement default/bottomLeft 均通过：DOM tree、计算样式、几何、SVG 与 visual style 0 差异，0 blocked。报告 `/tmp/select-focused-final-20261108.json`。Placement 的 radio 位于 Select 根节点之外，因此从 Select 根语义节点计数中移除该无效断言；Radio 交互仍由真实点击场景执行。
- 外层 HTML 字节仍有 Razor 子组件之间的空白差异，不等同于运行 DOM 树差异。剩余工作：继续逐 demo 审核 Select 整页当前差异，特别是 `_semantic`、Responsive、Status、Custom Tag Render、Hide Selected、MaxCount 和自定义 token；随后继续其它组件。全局/组件 Design Token、C# 生成数据和文档覆盖仍以本文件上方 11/08 Token 覆盖审计记录为准，不把本轮 Select 专项结果当成全库完成证明。

## 2026-11-08 Token 覆盖审计与 AutoComplete 弹层锚点修复

- 重跑 `phase1-progress.mjs`：组件 Token 表 65/65、组件 Token 行 584/584、组件对全局 Token 使用行 1464/1464；React Token 描述、默认值、CSS 变量、C# IR、Token 文档行和全局使用行不匹配均为 0。Semantic DOM slot/API/文档覆盖为 423/423。该审计证明生成数据与文档覆盖，不代表全部组件运行时 HTML/CSS 已对齐。
- AutoComplete Basic 的弹层此前用内部内容节点定位，较 React 控件根节点向右 12px。现在把 AutoComplete 根 `ElementReference` 传给 `OverlayTrigger.TriggerReference`，OverlayTrigger 保留显式引用。Docs.Server net10 构建通过，0 errors。
- AutoComplete Basic DOM/CSS fixture 更新报告 `/tmp/autocomplete-basic-dom-style-parity-root-anchor-20261108.json`：3 passed、0 differences、1 blocked；DOM、计算样式、popup 几何与视觉差异均为 0。阻塞场景的 popup 已显示，但重复用例中 Blazor 未呈现第三条建议项，未计为通过。浏览器实测两端根节点 x=339.664px，弹层 x=339px、宽 200px；React y=419.5px、Blazor y=420px，子像素垂直差 0.5px，与触发器自身约 0.32px 的布局差一致。React 与 Blazor Basic 页保持打开，Blazor 弹层展开供手工复核。
- 下一组件：Select。继续逐 demo 处理 HTML/CSS/几何与 Semantic DOM 差异；Icon 的五个定向 demo 当前为 5/5 零差异，但整页和所有交互状态仍未完成完整审计。

## 2026-11-07 Select / Icon 增量复核

- Select Variant 定向复核：React 的 Select 内联样式为 `flex: 1 1 0%`，Blazor 原 demo 写成 `flex: 1`；已将两组控件同步为 React 的 flex-basis。React 浏览器 DOM 已确认目标值，Blazor Puppeteer 定向采集时仍捕获到 Skeleton 占位，不能据此认定运行时 HTML 已对齐；需在服务端 demo 完成挂载后复测。Select 当前可确认 React demo 缺失 0，11/07 最新可用稳定基线为 HTML/CSS/几何差异 demo 数 7/9/1；本轮扫描报告 `/tmp/select-doc-inventory-variant-flex-fix-20261107.json` 因 hydration/占位时序不稳定，不覆盖该基线。
- React Select 28 个 demo 均已匹配，缺失 0；Blazor 有 23 个额外 demo。最新整页运行时扫描 `/tmp/select-doc-inventory-semantic-a11y-fix-20261107.json` 汇总 HTML/CSS/几何差异为 7/9/1 个 demo；React 页面 hydration error #418 令整页汇总仍是定位线索，不能单独作为完成证明。Responsive 与 Status 已按 React 增加 `SpaceItem` 包裹，Tooltip 根节点增加 `pointer-events:none`；浏览器检查确认两个 demo 分别输出 3 个和 2 个 `.ant-space-item`，每个 Select 均铺满容器。Responsive 隐藏项采用 React 的透明、绝对定位、零高度、禁用指针事件和 `aria-hidden=true`，聚合标签显示 `+ N ...`。其专项扫描仍有 HTML/CSS/几何 3/14/0；其他当前候选差异包括 `_semantic`、Variant、Placement、StyleClass、Size、Custom Tokenization、Suffix、Hide Selected、Custom Tag Render、Status 和 MaxCount。
- `_semantic` 最新样本从 HTML/CSS/几何 168/14/20 降至 163/3/13。已修复 segmented 的 Single/Multiple 标签、ARIA role/label/orientation/tabindex 和 Flex 的 `middle` gap token；同时让该预览继承 React 文档的 14px/22px 字体行高，并移除与 React 不同的 `.ant-row flex:1`。新定向全页扫描 `/tmp/select-doc-inventory-semantic-preview-style-fix-20261107.json` 确认 CSS 差异从 9 项降为 3 项。剩余 HTML 样本含 preview 宿主类、id、React/Blazor Radio `name` 与序列化空格；popup 及选项整体仍垂直偏移 36px（React top -4，Blazor -40），下一步继续核查 popup 翻转的触发坐标与容器边界。
- Icon 最新定向扫描 `/tmp/icon-doc-inventory-select-followup-20261107.json`：React 5、Blazor 6，React demo 缺失 0、Blazor 额外 `two-tone-color` 1；匹配五个 demo 的 DOM/CSS/几何差异均为 0/0/0。Icon 页面自动整页扫描仍受 React hydration error #418 影响，当前可确认范围仅为五个定向匹配 demo。
- 上述 Select / Icon 验证使用 React `:8000`、Blazor `:5031`；Blazor Development 服务保持运行，浏览器标签保留 Select Semantic DOM 页面供人工检查。Docs.Server net10 最近一次构建通过（0 errors）。
- 第一阶段生成覆盖复核 `/tmp/phase1-progress-turn-20261107.json`：CSS IR 72/72、主题算法 3/3、Semantic DOM slots 与文档行 423/423；组件 Design Token 页面 65/65、584 行，组件引用的全局 Token 页面 64/64、1464 行，Token 元数据缺失/不匹配均为 0。该脚本核验的是生成物与文档覆盖，不替代逐 demo 的运行时 DOM/CSS/视觉对照。

## 2026-11-06 Select / Icon 当前服务复验

- React 静态站点 `:8000` 与 Blazor Development 文档 `:5002` 均返回 200。Select 尺寸 default/large/small 与 Basic popup closed/Escape-close/open 共 6/6 场景通过，DOM tree、选定计算样式、几何及视觉样式差异均为 0。报告：`/tmp/select-dom-parity-revalidated-20261106.json`、`/tmp/select-basic-popup-revalidated-20261106.json`。
- Icon 全量定向 fixture 有 4/5 场景通过；唯一失败是 `icon-font` 的重复 `#icon-facebook` SVG symbol 顺序/测量值不稳定（全 fixture 报告 `/tmp/icon-dom-parity-serial-20261106.json`）。单独运行此 demo 1/1 通过（`/tmp/icon-font-isolated-20261106.json`）；另在两边滚动到 IconFont demo 后浏览器直接测量到 Facebook use 均为 6.75×13px，符号首项相同。该差异应继续查明多资源 demo 延迟挂载/重复 symbol 对扫描时序的影响；不计为 Icon 全部通过，也暂不修改组件代码。
- 当前服务主题菜单复测：选主题关闭后重开、外部点击关闭后重开、关闭动画期间立即重开均成功；三轮外部关闭后重开也成功。现有 Overlay 过渡令牌能取消陈旧的关闭动画。React / Blazor Icon 与 Select 页面已在浏览器新标签保留，供手工检查。

## 2026-11-06 Select demo 同步增量

- 对照 React Select 全页 28 个 demo 与 Blazor 51 个 demo，React 缺失 0、Blazor 额外 23。`custom-label-render` 和 `optgroup` 原先与 React 的演示数据/选择状态不一致；现已按 React 的缺失值回填行为、选项分组和默认值改写。为支持 `LabelTemplate` 自定义未匹配值显示，Select 内容区现将默认项传入模板，未指定模板时继续显示原始回填值。
- 使用 React `:8000` 与 Development Blazor `:5010` 整页运行时重新扫描：上述两个 demo 的 DOM、选定计算样式和几何均 0 差异；Select 整页汇总差异降至 HTML/CSS/几何 8/9/3（上次 10/9/3）。报告 `/tmp/select-doc-inventory-post-demo-fixes-20261106.json`。React 页面仍报告 hydration error #418，整页汇总需结合各 demo 专项结果复核。
- 浏览器复核确认 `custom-label-render` 显示 `No option match`，`optgroup` 显示回填 `lucy`，分组和四个选项与 React 示例对应。Development 服务 `http://127.0.0.1:5010/en-US/components/select#components-select-demo-custom-label-render` 保持运行，浏览器标签保留供手工检查。当前仍未收敛的 Select 重点包括 `responsive`、`custom-tag-render`、`status` 和 `_semantic`；IconFont 多资源页面的重复 SVG symbol 采集不稳定仍待单独处理。

## 2026-11-06 Button 全 Demo 清点更新

- 重跑 `documentation-parity-inventory.mjs --only button`，React 16 个 demo、Blazor 23 个 demo，React 示例缺失 0，Blazor 额外 7。最新运行时差异计数为 HTML 4、CSS 8、几何 1（旧表 9/10/6 已过期，以本次结果作为 Button 新基线）。CSS 差异集中出现在 color-variant、icon-placement、disabled、multiple、danger、linear-gradient、custom-disabled-bg、_semantic；HTML 差异集中于 multiple、linear-gradient、custom-disabled-bg、_semantic。报告：`/tmp/button-doc-inventory-current-20261106.json`。
- 当前 React 文档 Button 路由报告 hydration error #418；因此 inventory 可用于定位仍需复核的候选项，不能单独证明剩余差异均为真实实现差异。新基线复核发现几项 computed style 的 `display/min-width/min-height` 不一致，但 Button 几何仍相同；下一步需结合 CSS-in-JS 注入顺序和各 demo 的实际 DOM，先排除扫描/宿主样式影响，再修真实差异。此轮未宣称 Button 全页完成。

## 2026-11-06 本轮增量

- **Button Gradient Button 的 Space item 结构**：对照 React 实际 DOM 后确认其每个按钮外包一层 `.ant-space-item` 块级元素；Blazor demo 原先直接把 Button 放入 flex 容器，浏览器把按钮 `inline-flex` blockify 为 `flex`。现为两个按钮显式补齐 `SpaceItem`。React/Blazor 各有两个同名包装节点，按钮计算 `display` 均为 `inline-flex`，宽高分别为 `175.421875×40px`、`82.296875×40px`，第二个按钮相对位置均为 `183.421875px`。该 demo HTML/CSS/几何差异现均为 0。随后重跑 Button 全 demo inventory：React 16、Blazor 23、缺 React demo 0；HTML/CSS/几何总差异为 3/7/1（此前 4/8/1；临时全局补偿时 3/15/3；移除补偿后 3/7/1）；报告 `/tmp/button-doc-inventory-after-spaceitem-20261106-v2.json`。对比页：React `:8000/components/button#button-demo-linear-gradient`、Blazor Development `:5002/en-US/components/button#components-button-demo-linear-gradient`。Blazor 服务必须以 Development 启动，当前 Production `:5000` 的 v6 静态样式资源返回 404，不能用于视觉/样式结论；本次 `:5002` 的 `dark.css` 与 `component-parity.css` 均返回 200。此前加入的全局 Button min-width/min-height 补偿被撤销，因为它在 Button 其他 demo 中制造了额外样式差异。
- **第一阶段生成物覆盖重审**：重跑 `phase1-progress.mjs` 输出 `/tmp/phase1-progress-current-20261106.json`。CSS IR 72/72、CSS 算法 3/3、Semantic DOM catalog 61/61，结构迁移 70/70、API/文档 Semantic slots 423/423；组件 Token 文档页 65/65、行 584/584，全局 Token 文档页 64/64、行 1464/1464，Token 元数据不匹配计数均为 0。报告另标出 10 个组件没有组件专属 token 行；核对 React 源码后可见这些组件声明的 ComponentToken 本身为空，因此不把空表判作遗漏。该覆盖不替代运行时 HTML/CSS/视觉检查。

- **Button StyleClass 语义样式对象与函数**：React 和 Blazor 原 demo 的 DOM 槽位、计算样式及 40px 按钮高度一致；差异在于 React 的 `styles` 可按 props 回调，而 Blazor 仅有对象参数，示例将动态结果硬编码。Button 新增 `ClassNamesFunc`、`StylesFunc`，回调接收当前 Button 实例；demo 的 Primary 样式现按 `Type` 实时返回，并更新中英文说明、Blazor API 清单和 React→Blazor shape note。重新生成 demo metadata、重建并重启 Development 文档服务后，浏览器确认页面说明包含两个函数参数，Function 按钮由回调得到 root 背景 `rgb(23,23,23)` 和 content 白字；Object 按钮的对象阴影保持不变。两端 DOM 均为 Flex 容器 + 两个 Button + 文本 span；两按钮宽高分别为 76.8125×40px、91.3516×40px，间距 8px，选定计算样式相同（容器 x 仅差 0.008px）。net10 构建 0 errors；React 与 Blazor StyleClass 页面保持打开供人工检查。
- **Select / Icon 定向复验**：重跑现有运行时对照 fixture。Select Basic closed、Escape-close、popup-open 共 3/3 通过；Icon 字体、资源、基础、Two Tone、Custom 共 5/5 通过，DOM tree、选定计算样式、几何与视觉样式差异为 0，阻塞为 0。原始 HTML 字节仍包含 Razor 注释/缩进差异，不将其描述为字节级 HTML 相同。报告：`/tmp/select-dom-parity-current-20261106.json`、`/tmp/icon-dom-parity-current-20261106.json`。
- **Button Icon Placement 交互值**：Radio 项改为显式小写字符串 `start`/`end`，修复 Razor 将未显式表达式值解析为变量名导致的编译错误。Docs.Server net10 构建通过（0 errors，416 warnings）。用 Puppeteer 对 React 与 Blazor 分别真实点击 `start`，两边 radio 均选中 `start`，四个 Search 按钮的图标都移到文字前方，顺序一致。当前页面保留在浏览器的 Button Icon Placement demo。
- **主题菜单无法重开 / Overlay 参数回归**：复查发现 `Overlay.UpdatePosition` 对 `updateOverlayPosition` 的调用缺少 `Trigger.Ref`，导致 JS 参数位移；补齐后 Docs.Server net10 构建通过。重新启动 Development 服务并浏览器操作：主题菜单打开后页面滚动、关闭、再次打开成功；点击 Dark theme 关闭后再次打开成功，控制台无错误。浏览器页保持打开：`http://127.0.0.1:5000/en-US/components/button?theme=light#components-button-demo-icon-placement`。此前该现象仅通过页面重连短暂恢复的判断不充分，此处以参数修复后的新构建验证为准。
- Overlay 修复后的 Select 回归：closed、Escape-close、popup-open 3/3 通过，DOM tree、选定计算样式、几何与视觉样式差异均为 0，0 blocked；报告 `/tmp/select-dom-parity-post-overlay-fix-20261106.json`。
- **Button Danger demo**：React 使用 `Flex wrap gap="small"`，Blazor 原先是裸 `div`；已改为 `Flex Wrap="FlexWrap.Wrap" Gap="FlexGap.Small"`。Docs.Server net10 构建通过（0 errors，501 warnings）。在 React/Blazor 浏览器页实测两边均为 5 个按钮、`display:flex`、`gap:8px`、`flex-wrap:wrap`；容器宽度 430.148px 与 430.141px，差约 0.008px。浏览器当前保持在 Blazor Button Danger demo。此前 Button 全页 inventory 是 11/05 基线，未因本次单 demo 检查宣称全页完成。
- **Button Color & Variant 响应式尺寸**：同步 React demo 的 `xxl ? medium : small` 行为，监听 viewport resize 并将尺寸直接绑定到按钮，未增加包装 DOM。React/Blazor 在 1280px 都渲染 36 个 24px 按钮，在 1600px 都渲染 36 个 32px 按钮；两断点所选按钮计算样式签名相同。1280px 归一化 DOM 子树都是 80 个节点、hash `ee006686`；1600px 按钮样式签名 hash 同为 `2b898802`。双页截图目视核对了颜色、变体与换行。React/Blazor 页面保持打开：`http://127.0.0.1:8000/components/button#button-demo-color-variant`、`http://127.0.0.1:5000/en-US/components/button#components-button-demo-color-variant`。
- **Button Linear Gradient 样式**：比较 React `linear-gradient.tsx` 的 ConfigProvider 注入规则和 Blazor 的 demo CSS，发现 Blazor 为渐变按钮额外设置 `isolation:isolate` 与 `overflow:hidden`。删除这两项和冗余的 `position:relative`，改由按钮组件自身的定位样式承担定位；两端浅色主题的按钮背景均为 `rgb(22,119,255)`，尺寸均为 `175.414×40px`，伪元素渐变、inset、transition 一致，悬停后伪元素 opacity 都为 0。Docs.Server net10 构建 0 errors；React 与 Blazor Linear Gradient 页面保持打开供手工检查。该专项不代表 Button 全部 demo 收敛。
- **主题菜单再次点击复测**：在当前 Development 页面以浏览器实际点击验证按钮关闭后重开，以及选择 Dark theme 关闭后重开，均成功；无控制台错误。隐藏期间 `.ant-dropdown-hidden` 暂用 `left:-9999px` 隐藏，重开时 Overlay 重算位置，最终弹层回到触发器旁。当前无法复现“关闭后第二次不打开”的结果。

## 2026-11-05 本轮增量

- **主题切换 Overlay 重复打开修复**：复现主题菜单关闭后无法再次打开时，浏览器控制台捕获到 `Overlay.updateOverlayPosition` 在上下文变化的回退分支把 DOM 节点作为选择器参数，导致 JS 读取无效 Placement 并中断 Blazor 事件。回退调用现已继续传递原始 selector/container 参数，并对无效 Placement 做防御；Overlay Show/Hide 使用递增过渡 ID，旧关闭动画不能覆盖后续打开。更新 JS bundle 并重建 Docs.Server 后，浏览器验证触发器打开/关闭/重开，以及选择主题项关闭后再次打开均成功，控制台无错误；`git diff --check` 通过。此修复覆盖共享 Overlay，不计作全部组件的运行时 parity 完成。当前手工检查页保持打开：`http://127.0.0.1:5000/en-US/components/button?theme=light`。
- **Select / Icon 已验重点场景**：当前可用报告 `/tmp/select-dom-parity-current-20261105.json` 为 3/3（Basic closed、Escape-close、popup-open）DOM tree、计算样式、视觉样式与几何门禁无差异；Select 尺寸/交互/Semantic DOM/Suffix/Big Data 另有 13/13 场景记录。Icon 报告 `/tmp/icon-dom-parity-current-20261105.json` 的字体、资源、基础、Two Tone、Custom 五类场景 5/5 通过。它们仍不代表两个组件所有 demo 均完成浏览器扫描；Select 尚余 demo 未逐项确认，Icon 列表和主题色等已有专项说明。
- **Button Wave Demo 初始结构与样式**：对照 React `button/demo/wave.tsx`，修复 Blazor 把 wave animation disabled 错写成原生按钮禁用、末项文案不一致、静态保留效果 class 三处差异；Inset/Shake class 仅在交互触发时添加，动画后清理。net10 文档构建 0 errors。浏览器确认 5 个按钮名称、顺序、启用状态符合 React；点击 Inset 可触发并清理临时效果状态。最新 Button inventory `/tmp/button-inventory-after-wave-20261105.json`：React 16 demo 全匹配、缺失 0、Blazor 额外 7；差异从 HTML/CSS/几何 9/10/6 降为 8/9/5，Wave 本项三项差异归零。尚有 8 个 Demo 存在 HTML、9 个存在样式、5 个存在几何差异，下一项处理 `style-class`/`color-variant` 中可确认的真实差异。

- **Button Multiple 与 Overlay 实际交互定位**：将 Multiple demo 补为 React 的纵向 Flex、Primary/Secondary 按钮和 `Space.Compact` 操作组，保留现有 Blazor 点击菜单功能。浏览器点开菜单后发现 body portal 的应用阶段仍按 placement 覆盖坐标轴；修正为按最终水平/垂直坐标轴应用位置，并对右对齐弹层按右边距取整。重建 JS 与文档服务后，React/Blazor 弹层 bounds 均为 `x=910.35, y=238, 90.65×104px`，菜单文本一致；页面滚动后仍锚定触发器。Button 全量 inventory 复扫：React 16 个 demo 全有 Blazor 对应项，Blazor 额外 7 个；运行时 HTML/CSS/几何差异 9/10/6（几何较上一轮减少 1）。Multiple 剩余差异仅为 Blazor 增加了无障碍 `aria-label`，Flex 根宽度差 0.016px。报告 `/tmp/button-doc-inventory-current-20261105.json`。
- **Button Ghost、Disabled 结构与样式**：Ghost demo 容器上内边距从 26px 调整为 React 的 16px；危险按钮去掉 Blazor 专有的 `--antd-wave-shadow-color` 内联样式，并将 `ant-btn-dangerous` 类顺序调整到与 React 一致。Disabled demo 移除 React 没有的 `button-disabled-ghost-wrapper` class。Ghost 与 Disabled 中 Ghost surface 的专项浏览器对照均通过：DOM/CSS/几何/视觉差异 0。Button 全部 16 个 React demo 的最新扫描为 16 个有 Blazor 对应项、缺失 0、Blazor 额外 7；运行时 HTML/CSS/几何差异为 9/10/7（较本轮最初 11/11/8 减少）。报告 `/tmp/button-doc-inventory-final-20261105.json`；当前可手工检查页为 `http://127.0.0.1:5000/en-US/components/button?theme=light#components-button-demo-ghost`。下一步继续 Button 的其余 demo 差异。
- **文档暗色内容背景与侧栏滚动修复**：复查当前构建发现 `MainLayout` 的 CSS-in-JS 将主题变量固化为浅色值（生成样式中 `.main-container` 的背景为 `#fff`），而旧 `common.less` 仅在 hover 时开放侧栏滚动，并将滚动容器限制为 `100vh`。在文档自有 `docs-layout.css` 添加暗色主题下的内容背景/文字颜色覆盖，并让侧栏始终可纵向滚动、最大高度为 `calc(100vh - 64px)`；移除侧栏的 `-40px` 上移，使侧栏首项与主内容顶部对齐。重启 `:5000` Development 服务后，在暗色 Select 页面确认主内容背景 `rgb(20,20,20)`、标题为浅色，侧栏 `overflow-y:auto`、`scrollHeight=3810/clientHeight=680`，可以滚到 `scrollTop=1000` 再回到 `0`；首个菜单项 top=108px，主标题 top=112px。文档服务 net10 构建 0 errors（21 warnings）。
- **Select 聚焦与定向交互复验**：首次 Select Basic popup-open 对照捕获到 Blazor 缺少 `ant-select-focused`，但在新鲜重跑中闭合、Escape-close、打开三个场景 3/3 通过，手动点击也确认输入框聚焦且同时存在 `ant-select-focused ant-select-open`；该差异来自旧页面的过期交互状态，没有组件代码变更。随后串行重跑 Basic 3/3、尺寸 3/3、搜索/多选/清除交互 3/3、Semantic DOM 2/2、Suffix 1/1、Big Data 虚拟列表 1/1，共 13/13 场景通过，DOM tree、选定 CSS 与视觉门禁均无差异、无阻塞。一次并行跑测因多个 Chrome runner 争用导致 blocked，已串行重跑并以串行结果为准。报告：`/tmp/select-basic-popup-rerun-20261105.json`、`/tmp/select-size-final-20261105.json`、`/tmp/select-interaction-final-20261105.json`、`/tmp/select-semantic-final-20261105.json`、`/tmp/select-suffix-final-20261105.json`、`/tmp/select-virtual-final-20261105.json`。

- **Select、Icon 重点场景复验与 Button demo 对齐**：用当前 React 静态文档 `:8000`、Blazor Development 文档 `:5000` 和本机 Chrome 重跑 `select-dom-style-parity.json`、`icon-dom-style-parity.json`。Select Basic closed/Escape-close/popup-open 为 3/3 通过；Icon 字体、资源、基础、Two Tone、Custom 为 5/5 通过，均无 DOM、计算样式、视觉样式差异或阻塞场景。Button Basic 使用现有 fixture 复验 1/1 通过；Button 全 demo inventory 发现 React 16 个示例均有对应 Blazor 示例（Blazor 有 7 个额外示例），且 React 页面报告一个 hydration error #418。修复 Button Icon demo：使用 Tooltip 的 Unbound 触发器去除 Blazor 专有包裹节点，按 React 的两层 Flex/Tooltip 结构组织，并将末项改为 Google 链接按钮；Icon 场景 DOM/CSS/几何 0 差异。随后把 Button Block demo 改为 React 的 6 个按钮和垂直 Flex 间距，补回 disabled、text 两种状态。Block 复验 DOM/CSS/几何均一致。当前 Button 16 个匹配示例总差异从修复前 HTML 14/CSS 13/几何 10，降为 HTML 12/CSS 11/几何 8。Release 文档构建通过（0 errors，501 warnings）；最新 inventory `/tmp/button-doc-inventory-current-20261105.json`，其他重点报告 `/tmp/select-dom-parity-current-20261105.json`、`/tmp/icon-dom-parity-current-20261105.json`、`/tmp/button-basic-parity-current-20261105.json`。Button Blazor 页保持在浏览器的 Block demo 锚点；下一项继续 Button 剩余差异，并排除 React hydration 错误对整体扫描的影响。
- **第一阶段覆盖重新计算**：运行 `phase1-progress.mjs` 输出到 `/tmp/phase1-progress-current.json`。CSS IR 72/72、主题文件 17/13（目标覆盖 13）、算法 3/3；Semantic DOM catalog 61/61、定义/结构迁移 70/70、API 与文档 slot 423/423；组件 Token 页面 65/65、Token 行 584/584、全局 Token 页面 64/64、全局 Token 行 1464/1464。Token 描述、默认值、CSS 变量、全局映射差异均为 0。该覆盖报告只验证生成输入及文档目录，不代替所有组件的 runtime DOM/CSS/视觉验收。
- **文档 Header 响应式与主题效果**：对照 React v6 `Header/index.tsx`、`Logo.tsx`、`Navigation.tsx`，将首页 Header 从绝对定位、透明背景和 1280px 宽度改为全站一致的 64px sticky 布局；品牌切换为 React 的 Ant Design 图标和标题，搜索框调整为 220×32px，并补入 Sponsor 头像组和对应的右侧间距。右侧版本/语言/方向/主题/GitHub 控件按 React 尺寸重新排列。窄屏 1120px 以下收起桌面工具栏，附加导航动作直接交给 Menu 的自动溢出列表承载；移除旧的固定 More 子菜单触发器，避免与 Menu 自动溢出图标重复。767.99px 以下切换移动菜单；首页窄屏仍显示搜索框，移除旧 LESS 对首页搜索列的隐藏覆盖。修复暗色导航链接/按钮颜色，Body 背景改用主题容器色，Sponsor 头像边框/底色/透明度随深色主题切换，并加载此前未引用的 `happy-work.css`。Blazor net10 Release 构建 0 errors（499 warnings）。浏览器 1512px 下 Header 与 React 均为 64px，搜索框起点 Blazor x=514 / React x=510、宽度均为 220px；窄屏下仅保留 Menu 自带溢出触发器，无旧版固定触发项；500px 下 Header 无横向溢出、搜索框与移动菜单触发按钮可见，导航面板正常展开。深色模式实测菜单文字、Sponsor 区和卡片均使用深色主题背景/浅色文字；Happy Work 在主题菜单关闭和开启后 localStorage 状态分别为 false/true，点击按钮立即生成 8 个粒子并应用目标动画。当前验证首页 Header/主题交互，未扫描全站组件。页面恢复桌面宽度供人工查看。
- **DatePicker Basic 弹层与日历首周**：React 文档采用 `_site` 静态构建并由 `npx serve -s _site -l 8000` 提供，没有运行 Dumi dev server。对照时固定 trigger 的 viewport Y 后再点击，避免长文档滚动位置影响 Placement。修复 Blazor 在 2026 年 11 月错误地从 10 月 25 日开始日历周、而 React 从 11 月 1 日开始的问题；DatePicker 与 DateTime 面板现在按配置的周起始日计算首个显示日期。Focused fixture 1/1 通过，DOM tree、计算样式、视觉样式、弹层/trigger 几何、ranges footer 和 Today 控件均匹配。报告 `/tmp/datepicker-basic-preserved-viewport-20261105.json`。此结果只覆盖 Basic 弹层场景，DatePicker 其余 demo 仍需继续扫描。
- **Menu 横向溢出弹层坐标**：将“More”子菜单改为 Overlay 内置 `BottomLeft` 定位、内容宽度测量及视口宽度限制；溢出项触发器的标题位于 `li` 内缩 16px，因此仅该弹层增加 16px 左边距，并将垂直偏移由 4px 调为 1px。Development 文档服务器中浏览器点击后与 React 的弹层 bounds 完全一致：x=248、y=294、272.953×136px，三项内容相同。Menu 自动对照 1/1 通过，DOM/CSS/视觉差异为 0。400px emulation 下示例根菜单容器宽度变为 0，该宽度暂未验收。详见 [Menu popup 定位记录](dom-style-parity.md#menu-overflow-popup-coordinates--2026-11-05)。
- **React 文档运行方式**：重建 React 静态站点 `_site` 并以 `npx serve -s _site -l 8000` 提供；Select 文档在浏览器中正常加载。没有常驻 Dumi 开发服务器。Blazor Docs.Server 需在 Development 环境运行以提供静态 Web Assets。

## 2026-11-04 本轮增量

- **Icon 搜索工具栏 Affix**：将工具栏由 CSS sticky 改为复用 Blazor `Affix`，并按 React 的 80px 顶部偏移和吸顶时才应用的底色、阴影、padding、负 margin。523px 与 820px 下，吸顶前后的尺寸和位置都与 React 对齐；修正了圆角从 8px 调为 React 对应的 6px。Release 文档构建 0 errors。详细测量见 [Icon Affix 专项记录](dom-style-parity.md#icon-gallery-affix-parity--2026-11-04)。
- **第一阶段源数据覆盖复核**：重新执行 `phase1-progress.mjs`。当前 CSS 样式 IR 为 72/72、主题样式 17/13（覆盖目标 13）、主题算法 3/3；Semantic DOM catalog 61/61、slot 定义 70/70、API slots 423/423、文档 slots 423/423、结构迁移 70/70；组件 Token 文档 65/65、Token 行 584/584、全局 Token 使用文档 64/64、全局 Token 行 1464/1464。Token 描述、默认值、CSS 变量及全局 Token 映射均无差异。该脚本验证生成输入和目录覆盖，不代表运行时样式与所有页面已完成视觉验收。完整本轮 JSON 输出在 `/tmp/phase1-current-20261104.json`。
- **Button Basic 浏览器对照**：使用 React `:8000`、Blazor `:5055` 和当前 Button fixture 重新采集。5 个按钮的归一化 DOM tree 一致，选定计算样式与视觉样式无差异，1/1 scenario 通过、0 blocked。原始 HTML 字节对比仍不同（659 vs 879 字节，包含序列化/额外标记差异），所以这里只确认归一化结构及视觉通过，不声称字节级 HTML 一致。报告在 `/tmp/button-basic-5055-current.json`；Button 页面保持打开供人工检查。
- **Button Size 与 Ghost**：全量 Button 定向 fixture 对 17 个常用 demo 扫描，Basic 与 Size 通过，15 个仍有 DOM/CSS/视觉差异，0 blocked。Size demo 初次扫描发现选中 Large 但按钮仅 32px；根因是 `FormSize` 级联 setter 覆盖显式 `Size`。现在仅在用户没有传 `Size` 时采用级联值，显式尺寸优先；示例使用 React 同名 Medium 并补齐 Divider、Flex 包装和 Circle/Round 图标按钮。独立 Size 对照 1/1 通过，Large/Default/Small 切换实测为 40/32/24px。Ghost primary 的 solid→outlined 语义和 demo 内容也已同步；Ghost 对照视觉样式 1/1 通过，DOM 还留有 Danger 按钮上的 Blazor wave 颜色 inline style 差异。构建结果 0 errors（890 warnings）。
- **Icon / Select 静态站点复核**：React 文档重新构建为 `_site`，由 `npx serve -s _site -l 8000` 提供，Dumi dev server 已停止；当前静态与 Blazor `:5000` 都可浏览。现有聚焦对照：Icon 5/5、Select 尺寸 3/3、交互 3/3、Semantic DOM 2/2、Suffix 1/1、Big Data 虚拟列表 1/1，DOM/CSS/视觉门禁全通过。Big Data 对照发现并修复隐藏虚拟 option 缺少 `aria-disabled="false"` 的实际结构差异；同时增加 runner 的 `alignRootViewportY`，在相同 viewport trigger 位置下比较 popup，避免文档长短差异引发错误翻转判断。交互 fixture 改用生产构建可见的 Basic clear 用例，popup 使用真实坐标点击、搜索使用 fill，移除了对生产站点不可见的 debug-only demo 依赖。net10.0 文档构建 0 errors。窄屏文档壳层在 React/Blazor 820px 与 523px 均通过浏览器测量：sticky header 64px、固定目录 x=viewportWidth−156px/y=92px，页面无横向溢出。Icon 画廊仍与 React 的固定 6 列和长标签裁切不同；Blazor 保持响应式列数与完整可读标签，后续需继续处理其目标差异。
- **Menu 横向溢出初测**：2026-11-04 首轮自动对照发现 root 和 overflow DOM/CSS 差异（报告 `/tmp/menu-horizontal-current-20261104-static.json`）；后续已修复并在 2026-11-05 通过自动 DOM/CSS/视觉对照及浏览器弹层坐标复核，见上方最新增量。

## 2026-11-03 本轮增量

- **2026-11-04 Icon / Select 复核**：React Icon 实际标签样式为 nowrap + `scale(0.8)`，固定 6 列并由卡片 `overflow:hidden` 裁切长名称；不是响应式缩放。Blazor 已按可读性目标响应式换列并折行。Select 的 5052 自动对照中 closed、Escape-close 通过，popup-open 阻塞（点击后 `aria-expanded=false`）；在可检查的 5046 页面直接点击同一基础 Select 后，`aria-expanded=true` 且选项列表可见。此次不据不稳定的 5052 自动点击结果改组件逻辑；Select 弹层差异仍需稳定复现后再判定。完整 71 组件扫描基线仍见下方表格。

- **Icon**：当前站点的 icon-font、icon-font-resources、basic、two-tone、custom 5 个 React 示例自动对照 5/5 通过，DOM/CSS/视觉差异均为 0。另在 558px 视口测量：React 固定 6 列，StepBackwardOutlined 卡片宽 42.5px、标签宽约 134px 并被裁切；Blazor 3 列，842 个标签均无溢出。React 与 Blazor 文档页都保持打开供手工复查。
- **Select**：本轮覆盖 Basic 开关弹层（3/3）、尺寸（3/3）、Semantic DOM（2/2）、Suffix（1/1）、搜索/多选/清除交互（3/3），共 12 个聚焦场景通过 DOM tree、选定 CSS 与视觉对照、无阻塞。修复 ConfigProvider 默认 `FormSize` 覆盖明确传入的 `Size`；浏览器确认三个 Select 随 radio 切换到 40px/24px。Virtualization 弹层的 DOM tree、CSS、视觉也一致，但原始 HTML 精确门禁仍因 Razor 缩进空白不同而报告差异。报告见 `dom-style-parity.md` 专项记录。Select 尚有 18 个 demo 未完成全量收敛。
- **Dropdown**：当前 `:5046` 重扫仍为 React 22 项、Blazor 23 项、21 个普通名称匹配、2 个 Blazor 额外项；`_semantic` 是 Semantic DOM 预览，页面无阻塞。21 个普通示例现有 HTML/CSS/几何差异计数为 21/9/9。Context Menu 已按 React 改为填充示例容器并使用主题颜色，右键三项菜单可正常打开；颜色计算值一致，宽度只差 0.016px，原始 style 属性仍因 React 序列化颜色、Blazor 保留 CSS 变量而不同。当前 DOM/CSS 和剩余 demo 差异见 [Dropdown 复核记录](dom-style-parity.md#dropdown-current-site-inventory-and-context-menu-correction-2026-11-03)。扫描报告 `/tmp/dropdown-current-site-5046.json` 及专项报告 `/tmp/dropdown-context-menu-5046-report.json` 均仅在本机。

Icon 列表筛选栏对照（2026-11-03）：Blazor 已由 RadioGroup 换为与 React 一致的 Segmented，并修正四个筛选图标。673px 视口下四个选项宽度分别与 React 相同；专项 DOM/CSS/视觉报告 1/1 通过。该对照同时修复共享 Segmented 的选中态 class 和原生 radio 分组名。浏览器交互确认筛选结果为 Outlined 445、Filled 249、Two Tone 148、All 842，且每个状态只选中一项。报告 `/tmp/icon-segmented-toolbar-5046-report.json`；完整说明见 [Icon 工具栏专项记录](dom-style-parity.md#icon-gallery-filter-toolbar-2026-11-03)。

React v6 文档/运行时对齐基线来自 [2026-11-01 71 组件扫描](runtime-parity-summary-2026-11-01.md)。本轮 API 数字来自新生成的 [React→Blazor API 映射](react-to-blazor-api-map.json)，`API 同名/别名/未映射 +形状差异`按 React 抽取的属性统计；未映射数需逐项判定是否为真实功能缺失。运行时列为已有基线数据，本轮没有对 71 个组件全部重新抓取 DOM/CSS。Token 与 Semantic DOM 总覆盖见基线报告顶部。

2026-11-03 Icon 定向跟进：React 图标网格固定 6 列，无响应式断点；图标项 `overflow:hidden`，标签单行居中并 `scale(0.8)`。超出单元宽度的名称在 React 和 Blazor 中都会被裁切。考虑到名称应完整可读，Blazor 文档画廊加入 992/767/480px 的 4/3/2 列断点并允许标签折行；此前在 641px、当前在 558px 浏览器宽度分别测量 842 个图标名称，横向或纵向溢出均为 0。该处是可读性修正，行为有意优于 React 当前的裁切实现。详细结论见 [DOM/CSS 对齐记录](dom-style-parity.md#icon-gallery-long-name-clipping-2026-11-03)。

Icon 窄屏对照复核（2026-11-03）：在同一内置浏览器以 320×900 视口查看 React v6 与 Blazor 页面。React 页面仍显示 6 列，Directional Icons 名称被左右裁切；Blazor 页面切换为 2 列，图标名称完整换行且卡片随内容增高。由此确认 React 当前有意保留旧网格裁切效果，没有按视口改变列数；Blazor 采用响应式列数及可增长卡片，是针对“名称不应被裁切”的可读性修复。React 与 Blazor 标签都保持打开，视口已恢复默认值供人工检查。

本轮续查（2026-11-03）：重新生成的 phase1 汇总仍显示 Design Token 覆盖 584/584 组件 Token 行、1464/1464 组件所用全局 Token 行，中英文文档页 65/65；Semantic DOM API slot 423/423、文档 slot 423/423。以上是生成输入/目录覆盖，不等同于全部运行时 DOM/CSS 已验收。Select Basic 弹层用例在当前 5052 新构建服务上复验时，弹层本身选定样式一致，但弹层列表结构出现状态差异（React 的启用选项 `aria-disabled` 在不同采集时序中出现与缺失），另一次采集方向从 `bottomLeft` 与 `topLeft` 间变化；由于自动化结果不稳定，本轮没有据此改组件行为，也不计为通过。针对该差异的临时源码改动已撤回，最终服务与源码一致，保持运行于 `http://127.0.0.1:5052/en-US/components/select?theme=light#components-select-demo-basic`，供浏览器手工检查。最新报告 `/tmp/select-popup-final-5052.json`；Select 弹层交互需先稳定采集条件再继续校准。

2026-11-03 Select 定向复核：修正 popup runner 的焦点选择器与点击方式，使用 Chromium 真实坐标指针事件；同时将程序化聚焦设为 `preventScroll`，避免延迟焦点回调在弹层测量后滚动页面。当前 `:5046` 站点的 closed、Escape-close、popup-open 场景 3/3 通过，DOM tree、选定计算样式和视觉样式差异均为 0；弹层相对 trigger 的坐标和尺寸与 React 一致。最新报告 `/tmp/select-basic-popup-current-5046-report.json`；详情见 [DOM/CSS 对齐记录](dom-style-parity.md#select-basic-current-site-rerun--2026-11-03)。这只是 Select Basic 专项复验，当前没有完成 71 个组件的全量复扫。

2026-11-03 Button 定向复核：`debug-color-variant` 中 React 与 Blazor 按钮尺寸一致，首个按钮因 Blazor ConfigProvider 的主题作用域包装盒而未被 Flex 父级 blockify。demo 局部将该主题包装盒设为 `display:contents`，保留主题 token 同时使 Flex 子项行为与 React 相同。重建后 Chromium 场景 1/1 通过，所选按钮 DOM/CSS/视觉差异 0；主题包装 `<div>` 仍在原始 DOM 中，因此不代表全 demo HTML 树完全一致。当前文档服务器运行于 `http://127.0.0.1:5044/en-US/components/button#components-button-demo-debug-color-variant`，Icon 原页面仍开在另一个浏览器标签。报告与细节见 [Button 对齐记录](dom-style-parity.md#button-debug-color--variant-nested-provider-layout-2026-11-03)。

当前 API 映射统计：872 同名、93 别名、898 未映射、5 个形状差异（React 抽取 1,863 个属性，71 个组件有 Blazor 对应项）。本轮 Breadcrumb 的 `items`/`params` 已映射到新增同名参数。898 项包含大量内部类型和实现属性，需按公开组件 API 逐项筛查，不能直接视为缺失功能。

运行时列：`HTML/CSS/几何差异数`来自组件扫描；`React Demo 缺失`仅按 React Demo 名称计数，Blazor 额外 Demo 不加缺口。Select Basic 当前站点 closed、Escape-close、popup-open 的 3 个场景已通过，另有 Select search/multiple/clear 的历史交互专项通过；这些专项结果不能代表 Select 全部 18 个 demo 收敛。

本次追加 Dropdown 文本选区菜单对齐：采用与 React 相同的定时读取选区、固定定位触发点、视口坐标、数据驱动菜单和消息提示；菜单开关直接由选区状态派生，避免额外的布尔状态与 React 行为分叉。设置内容宽度，避免 1px 触发点将弹层挤成细条。浏览器实测拖选后菜单贴近选区显示三个动作；点击动作后菜单关闭、原生选区清空并显示所选文本。Development 文档服务保持运行于 `http://127.0.0.1:5030/en-US/components/dropdown#components-dropdown-demo-selection`，不代表重新扫描了 Dropdown 的所有 React Demo。Space 本轮补齐 5 个被元数据漏掉的 React Demo，并扩展 Compact 主示例；专项检查确认 19 个分组、外层宽度 644px 与 React 相同，已选 CSS 无差异，但嵌套 HTML 尚有控件包装、输入值序列化和图标结构差异，详见 [Space 专项记录](dom-style-parity.md#space-compact-demos-and-focused-parity-check--2026-11-02)。Splitter 增加 `nested-in-tabs` 和 `debug` 两个 React demo；定向 size 场景复测 1/1 通过，DOM tree 与选定计算样式无差异。全部目标文档构建已于 2026-11-02 通过（net6–net10，0 errors，2399 warnings）；`IconCatalog.GetSourceSvg` 错误已解决，详见本地构建日志 `/tmp/antdesign-blazor-all-target-build-after-fixes.log`。

Breadcrumb 本轮增加 React 同名 `Items`/`Params`，新增 `withParams`、`withIcon`、`separator-component` 三个缺失 Demo，并按 React 结构修复 items 模式下链接和分隔符输出。自动对照 2/2 场景 DOM tree 一致，0 blocked；With Params 选定样式完全一致，Separator Component 只有根宽度 0.016px 子像素差异。net10 文档构建通过（0 errors）；完整对照记录见 [Breadcrumb 专项记录](dom-style-parity.md#breadcrumb-items-and-route-params--2026-11-02)。当前 Breadcrumb API 统计更新为 6/2/9，尚余 `overlay`、`debug-routes`、`component-token`、`style-class` 四个 React Demo 名称未同步。

2026-11-02 定向重跑 React `:8000` / Blazor `:5028`：Icon 5/5、Select Basic 与弹层 3/3、Semantic DOM 2/2、Suffix 1/1 均通过，DOM tree 和视觉样式差异为 0。首次 Select 交互运行发现 TreeSelect 重复声明继承来的 `DropdownStyle` 参数，造成该页面后段 demo 渲染异常；移除重复参数后重建，页面可正常渲染，Select 搜索过滤、多选、未匹配值清除交互 3/3 全通过，DOM/CSS/视觉样式差异均为 0，无阻塞用例。详情见 [DOM/CSS 对齐记录](dom-style-parity.md#fresh-targeted-rerun-2026-11-02)。浏览器保留两个可手工检查的标签：Dropdown 文本选区和 Select 文档页。

本轮追加复核：Icon 五个 React demo 重新运行仍为 5/5，DOM/CSS/视觉差异 0。Select Basic 的手工浏览器点击能打开列表；弹层单组自动对照 1/1 通过，但该次测得 Blazor 2,420ms、React 516ms。将 closed、Escape close、popup open 串联运行时，popup 用例出现采集时序波动：有一次弹层节点未捕获、一次弹层结构和样式相同但 Blazor 根节点缺少 `ant-select-focused`；增加 1.5 秒挂载等待后仍偶发，故暂不把 Select 的弹层交互稳定性标记为完全收敛。浏览器保留 Select Basic 下拉列表打开状态供手工检查；最新运行输出在 `/tmp/select-basic-popup-parity-current-rerun.json`、`/tmp/select-basic-popup-only-current-report.json` 和 `/tmp/select-basic-popup-parity-settled-report-retry.json`。

Dropdown 文本选区菜单 2026-11-02 浏览器复验：拖选文本后可打开三项菜单，菜单按选区下方定位；点击 Mask keyword 后菜单关闭、原生选区清除，并显示 `Mask keyword: <selection>` 消息。实现与 React 一致地在 mouseup 后读取 Selection/Range，使用 fixed 1px trigger、视口坐标 centerX/bottom+4、选区驱动的受控 Open 和数据驱动菜单；JS 模块在首轮 render 预加载，避免首次 mouseup 触发模块下载。Development 服务运行于 `http://127.0.0.1:5030/en-US/components/dropdown#components-dropdown-demo-selection`。

| 组件 | API 同名/别名/未映射 +形状差异 | 缺失 React Demo | HTML 差异 | CSS 差异 | 几何差异 |
|---|---:|---:|---:|---:|---:|
| Button | 17/2/8 +0形状差异（API 基线） | —（11/06 清点：缺失 0、Blazor 额外 7） | 3 | 7 | 1 |
| FloatButton | 24/2/6 +0形状差异 | — | 1 | 0 | 0 |
| Icon | React API 映射中无对应项 | —（11/07 定向清点：React 5、Blazor 6，缺失 0、额外 `two-tone-color` 1） | 0* | 0* | 0* |
| Typography | 16/2/31 +0形状差异 | —（`_semantic` 在独立 Semantic DOM 区块呈现） | 8* | 4* | 7* |
| Divider | 9/2/5 +0形状差异 | — | 2 | 6 | 5 |
| Flex | 6/0/4 +0形状差异 | — | 5 | 2 | 2 |
| Grid | 10/0/1 +0形状差异 | — | 2 | 1 | 1 |
| ProLayout | React API 映射中无对应项 | — | 9 | 9 | 9 |
| Masonry 6.0.0 | React API 映射中无对应项 | — | 6 | 4 | 4 |
| Space | 13/2/6 +0形状差异 | — | 10 | 5 | 7 |
| Splitter | 14/1/35 +0形状差异 | — | 12 | 10 | 7 |
| Anchor | 18/2/3 +0形状差异 | — | 12 | 5 | 5 |
| Breadcrumb | 6/2/9 +0形状差异 | overlay, debug-routes, component-token, style-class | 1 | 1 | 1 |
| Dropdown | 33/2/2 +4形状差异 | —（Semantic DOM 预览单独呈现） | 21 | 9 | 9 |
| Menu | 14/2/12 +0形状差异 | — | 18 | 17 | 17 |
| Pagination | 6/0/6 +0形状差异 | — | 13 | 11 | 8 |
| Steps | 12/1/15 +0形状差异 | max-count, style-class, component-token | 17 | 15 | 15 |
| Tabs | 9/2/10 +0形状差异 | — | 20 | 19 | 19 |
| AutoComplete | 4/0/10 +0形状差异 | — | 17 | 10 | 10 |
| Cascader | 18/0/6 +0形状差异 | fields-name, ellipsis-debug, custom-render, panel, render-panel | 12 | 9 | 10 |
| Checkbox | 11/2/17 +0形状差异 | — | 12 | 7 | 6 |
| ColorPicker | 11/1/28 +0形状差异 | base, text-render, presets-line-gradient, style-class, line-gradient, trigger-event, panel-render, pure-panel | 8 | 3 | 4 |
| DatePicker | 5/0/19 +0形状差异 | multiple, components, multiple-debug, render-panel | 28 | 23 | 22 |
| ProForm | React API 映射中无对应项 | — | 43 | 39 | 39 |
| Input | 30/2/9 +0形状差异 | filled-debug, search-variant-debug, style-class, align | 23 | 18 | 16 |
| InputNumber | 8/0/6 +0形状差异 | addon, digit, disabled-hover-debug, borderless-height-debug, debug-token, spinner, filled-debug, render-panel | 13 | 13 | 6 |
| Mentions | 4/1/6 +0形状差异 | popuprender, autosize, render-panel, size, allowclear, autosize-textarea-debug, style-class, component-token | 8 | 1 | 1 |
| Radio | 11/1/7 +0形状差异 | — | 15 | 16 | 10 |
| Rate | 2/0/1 +0形状差异 | — | 6 | 9 | 4 |
| Select | 28/0/1 +1形状差异 | —（11/08 清点：React 28、Blazor 51，缺失 0、Blazor 额外 23） | 1* | 9* | 1* |
| Slider | 17/1/25 +0形状差异 | — | 16 | 16 | 14 |
| Switch | 12/1/8 +0形状差异 | text, style-class, disabled, component-token | 3 | 3 | 1 |
| TimePicker | React API 映射中无对应项 | 12hours, colored-popup, render-panel, change-on-scroll | 13 | 7 | 7 |
| Transfer | 24/2/38 +0形状差异 | component-token | 12 | 11 | 10 |
| TreeSelect | 17/0/5 +0形状差异 | render-panel, component-token | 11 | 12 | 9 |
| Upload | 32/2/22 +0形状差异 | debug-disabled | 23 | 23 | 23 |
| Avatar | 11/2/8 +0形状差异 | — | 10 | 9 | 8 |
| Badge | 12/2/10 +0形状差异 | ribbon-debug, component-token, style-class, colorful-with-count-debug | 13 | 13 | 13 |
| Calendar | 16/1/10 +0形状差异 | event-range, lunar, week, style-class, component-token | 5 | 5 | 5 |
| Card | 18/2/8 +0形状差异 | — | 14 | 14 | 12 |
| Carousel | 6/1/5 +0形状差异 | autoplay, arrows, component-token, placement, fade, dot-duration | 1 | 1 | 0 |
| Collapse | 17/3/7 +0形状差异 | — | 11 | 13 | 5 |
| ProDescriptions | React API 映射中无对应项 | text, padding, size, style, style-class, jsx, component-token, block | 5 | 5 | 5 |
| Empty | 7/2/1 +0形状差异 | — | 6 | 5 | 1 |
| Image | 6/0/6 +0形状差异 | toolbarrender, preview-mask, coverplacement, preview-group-top-progress, component-token, preview-imginfo | 11 | 10 | 5 |
| ProList | React API 映射中无对应项 | — | 17 | 16 | 9 |
| Listy 6.6.0 | React API 映射中无对应项 | virtual, group, rich, drag-sorting, infinite, style-class, scroll-to | 1 | 1 | 1 |
| Popover | 4/1/3 +0形状差异 | placement, style-class, wireframe, arrow, shift, render-panel, component-token | 4 | 4 | 2 |
| QRCode | 11/1/2 +0形状差异 | — | 8 | 9 | 2 |
| Segmented | 3/0/6 +0形状差异 | style-class, size-consistent, vertical, shape, with-name, controlled-two, componenttoken | 9 | 9 | 5 |
| Statistic | 8/1/10 +0形状差异 | animated, timer, component-token, style-class | 3 | 3 | 3 |
| ProTable | React API 映射中无对应项 | — | 52 | 47 | 51 |
| Tag | 13/2/10 +0形状差异 | disabled, customize, component-token | 11 | 7 | 8 |
| Timeline | 11/2/2 +0形状差异 | pending, horizontal-debug, end, pending-legacy, semantic, component-token | 8 | 8 | 7 |
| Tooltip | 15/2/9 +0形状差异 | shift, force-align, debug, smooth-transition, destroy-on-close, render-panel, wrap-custom-component | 7 | 6 | 6 |
| Tour | 10/1/4 +0形状差异 | render-panel | 7 | 7 | 7 |
| Tree | 29/2/14 +0形状差异 | directory-debug, drag-debug, block-node, multiple-line, line-debug, scroll-to, component-token, style-class | 11 | 9 | 9 |
| Alert | 16/4/13 +0形状差异 | — | 15 | 14 | 9 |
| Drawer | 16/1/11 +0形状差异 | multi-level-drawer, style-class, scroll-debug, render-in-current, config-provider, render-panel, component-token | 11 | 8 | 7 |
| Message | 1/1/12 +0形状差异 | hooks, style-class, component-token, stack, thenable, render-panel | 2 | 2 | 2 |
| Modal | 31/1/33 +0形状差异 | dark, render-panel, wireframe | 11 | 8 | 6 |
| Notification | 1/0/29 +0形状差异 | hooks, show-with-progress, progress-color, render-panel, stack, component-token, style-class | 3 | 2 | 2 |
| Popconfirm | 13/1/6 +0形状差异 | placement, async, style-class, wireframe, shift, promise, render-panel | 4 | 0 | 1 |
| Progress | 16/1/11 +0形状差异 | circle-steps, info-position, circle-micro, size, style-class | 11 | 11 | 11 |
| Result | 8/2/2 +0形状差异 | — | 10 | 6 | 7 |
| Skeleton | 10/2/11 +0形状差异 | — | 8 | 7 | 3 |
| Spin | 10/2/6 +0形状差异 | percent, fullscreen, list-debug | 6 | 7 | 7 |
| Watermark | 9/2/6 +0形状差异 | portal, debug | 1 | 1 | 1 |
| Affix | 4/2/4 +0形状差异 | — | 3 | 0 | 3 |
| App | 5/1/3 +0形状差异 | basic, config | 0 | 0 | 0 |
| ConfigProvider | 5/1/101 +0形状差异 | locale, wave, holderrender, prefixcls, useconfig, warning, focus-outline-debug | 2 | 1 | 1 |

合计 **71 个组件**。最近完成：Drawer/Modal 增加 React `Open`/`OpenChanged` 参数名，Progress 增加 `RailColor`；旧参数仍兼容。Dropdown 已增加 `OpenClassName`；Select 增加 root/popup 类、style、debounce 和自定义 loading icon API。下一步继续按公开 React API 的真实差异逐组件补齐；提取结果中的内部属性先与公开 API 区分。

表格中的星号表示 Typography 数值来自 11/07 `title` 修复后的估算扫描；本轮另修复 Copyable 按钮节点，但尚未重新运行完整 parity 汇总，因此不把该统计当作当前最终差异数。

本轮 Dropdown 文本选区菜单以相对选区容器定位锚点，使用 Range 的视口坐标换算为容器内容坐标；页面滚动时锚点随文本移动，由 Overlay 的滚动监听重算弹层位置。浏览器实测滚动前后 Range、锚点与弹层同步移动 771px，弹层未留在原视口坐标。文档项目 net10 编译通过。整套解决方案构建仍受构建环境问题影响（并行多目标静态资源缓存锁、测试项目 assets 缺少 net10 target、样式构建脚本模块缺失）；使用 Development 环境单独启动的文档服务完成了交互验收。

复查后发现选区菜单继承默认 `InView` 边界时，滚动到选区离开视口会把弹层钉在视口边缘。本轮将该 demo 的 Overlay 边界设为 `InScroll`，并在 5030 Development 页面用浏览器实际选中文本后滚动：页面实际滚动 176.5px，选区锚点与菜单也同步移动 176.5px，二者间距保持约 4px；文档项目 net10 构建通过。

Dropdown 文本选区跟随滚动复核：为选区 Range 建立浏览器端滚动/缩放监听，capture 阶段先更新虚拟 trigger，再由 Overlay 计算弹层位置，避免 Blazor Server 往返延迟。Development 浏览器交互确认页面滚动时选区锚点与菜单同步移动，间距保持稳定；重新加载后 demo 正常呈现。`git diff --check` 通过。Dropdown 本轮问题完成，接下来恢复 Select 对齐，再推进 Icon。

文档字体复核（2026-11-03）：React 组件文档实际按区域使用不同字体，不能统一成系统字体。Blazor 的正文与组件标题使用 `AlibabaSans, -apple-system, ...`；组件标题为 32px/600，Markdown 小标题使用 Avenir，侧栏与顶栏使用 `Avenir, AlibabaSans, -apple-system, ...`。此前移除 AlibabaSans/Avenir 优先级的尝试与 React 不符，已恢复并按浏览器实际样式调整。Docs.Server net10 构建通过（0 errors）；更新后的 Icon 页已在 `http://127.0.0.1:5039/en-US/components/icon` 打开供人工复核。当前轮只复核 Icon 页面字体，没有重扫 71 个组件。

Select 复核：未受控 Select 现在保留 Overlay 回报的可见状态，避免 Blazor Server 重绘时把首次打开的弹层关闭；受控打开的 Semantic DOM 预览不再抢先聚焦。直接 Chromium 对照 React 与 Blazor 的首次打开状态，弹层 DOM、选定计算样式、几何和视觉差异均为 0，popup 顶部间距一致为 3.516px。完整页面的串行 parity runner 仍有焦点/弹层采集时序波动，因此不把该门禁计为稳定通过。详情见 [DOM/CSS 复核记录](dom-style-parity.md#select-first-open-focus-state-recheck-2026-11-03)。

Icon 图标列表长名称复核（2026-11-03）：React 源码固定 6 列、没有响应式断点，单元格高度 100px 且 `overflow:hidden`，名称为单行并缩放到 0.8，因此长名称在 React 也会被裁切。Blazor 为保证名称可读，增加 992/767/480px 的 4/3/2 列断点并允许折行；名称改用 11px 字号而非缩放 16px 行盒，减少单字符孤行。641px 页面中测量六个分类共 842 个图标，名称无横向或纵向溢出。这是针对可读性的有意增强，不是 React 原有响应式行为。详情见 [DOM/CSS 对齐记录](dom-style-parity.md#icon-gallery-long-name-clipping-2026-11-03)。

Icon 名称再次复核（2026-11-03）：固定 100px 高度仍可能裁切更多换行的名称，故改为 `min-height:100px`；在 320px、250px 与 390px 视口下，图标列表分别使用 2/2/2 列，检查 480 个图标标签均无自身横向溢出。390px 页面测得文档整体 scrollWidth 为 476px，说明页面其它区域仍有横向溢出，未归因于图标标签。本次新构建页面可在 [Icon 文档](http://127.0.0.1:5052/en-US/components/icon#components-icon-demo-list)查看。文档项目 net10 构建通过（0 errors，496 warnings）。该修复只针对 Icon 图标列表的标签容纳，不代表 Icon/Select 的全量 Demo parity 扫描已完成。

2026-11-03 Icon 文本溢出根因复核：React 的 `CopyableIcon` 将标签设为 `white-space:nowrap; transform:scale(.8)`，6 列网格没有响应式断点；单元格 `overflow:hidden` 会裁切过长名称，因此 React 并未真正消除裁切。Blazor 的 `.iconItem` 为纵向 flex 且 `align-items:center`，仅设置 `overflow-wrap:anywhere` 不会约束 flex 子项的自动宽度。本轮为 `.iconName` 增加 `width/max-width:100%` 与 `min-width:0`，确保名称在网格单元内换行。Docs.Server net10 构建通过（0 errors，496 warnings）。当前可手工检查的完整交互页仍开在 `http://127.0.0.1:5046/en-US/components/icon#components-icon-demo-list`；本次新构建因现有 Kestrel 配置占用 5000 端口，未能在新进程完成视觉复核。

Select 尺寸复核（2026-11-03）：输入组件基类不再用 ConfigProvider 的级联默认尺寸覆盖显式 `Size` 参数；React/Blazor default、large、small 对照 3/3 通过，DOM/CSS/视觉差异为 0。更新后的 Select 尺寸示例保留在浏览器供手工检查。详见 [Select 尺寸优先级记录](dom-style-parity.md#select-size-and-configprovider-default-precedence-2026-11-03)。

Card demo 覆盖（2026-11-03）：依据 React 名称补入 `no-body-debug` 和 `button-alignment-debug`，分别检查无 body 时 cover/actions 的 DOM，以及 Card 标题栏、行内按钮和 Popconfirm 图标按钮的垂直对齐。中英文描述、debug 元数据和 Razor 组件均已加入；执行 `demo2json` 更新本地生成输入，Docs.Server net10 构建通过（0 errors）。在 5040 Development 页面启用 Debug 后，浏览器确认两个 demo 都已渲染；无 body 样例为 1 个 `.ant-card`、0 个 `.ant-card-body`、1 个 `.ant-card-actions`，按钮对齐样例渲染 2 个 Card。当前仍以 2026-11-01 的 71 组件扫描为全局基线，本轮没有重扫全站。

2026-11-04 文档布局核对：同步 React 的 sticky 头部、右侧固定目录偏移/高度，并为组件文档锚点添加 80px scroll margin。1280px 浏览器验证滚动 700px 后头部仍在顶部、TOC 固定于 92px；Select Basic 直接锚点落于头部下方，手工点击弹层成功。Select 自动化单弹层用例仍报告未展开，不能计为通过，见 [DOM/CSS 复核记录](dom-style-parity.md#sticky-docs-header-right-toc-and-select-hash-target--2026-11-04)。当前全站覆盖仍以 2026-11-01 的 71 组件基线为准，本轮没有重扫全部组件。

2026-11-04 补充：移除 `MainLayout` 中 992px 以下隐藏右侧 TOC 的规则。React 在 820px 仍显示 sticky header 和右侧目录；重新构建后的 Blazor 页面在 820px 也显示 sticky header 与右侧 TOC。Button Loading 增加 `LoadingIcon` 参数，demo 与 React 的九个状态样例对齐；net10 文档构建成功（0 errors）。Button Loading 默认态自动比较 1/1 通过，点击加载的自动快照受 React 涟漪动画与 3 秒定时器的捕获时序影响，先记录为差异而不计通过。此处未重新扫描 71 个组件。

2026-11-07 Select `custom-tag-render`：新增类型化 `TagRender` RenderFragment 上下文（Item/Value/Label/Closable/OnClose），并将 React 示例改为 100% 宽度、gold/cyan 可关闭 Tag，无旧 Disabled 开关。为两个非响应式已选项循环加入稳定 key，避免删掉第一个 Tag 后复用其已关闭实例；Select OverlayTrigger 也忽略 `.ant-tag-close-icon` 点击，关闭标签不会误开下拉。以浏览器实际点击验证：2 个标签变为 1 个，下拉保持关闭。Select 全量文档盘点：React 28 / Blazor 51，缺失 React Demo 0、额外 Blazor Demo 23；全组件示例 DOM/CSS/几何差异分别为 8/9/3 个 Demo。自定义标签示例自身扫描仍有 HTML/CSS/几何差异（8/22/14 项），来自 Blazor Tag 输出/样式细节；React 页存在 hydration error #418，自动快照受限，不能宣称该示例样式已完全对齐。Docs.Server net10 构建成功（0 errors）；磁盘空间曾导致一次构建失败，随后用禁用调试符号构建组件与文档并完成交互核验。当前工作目录包含大量其他未提交修改，本轮未提交。

2026-11-07 Icon 复扫：React 5 个 Demo 与 Blazor 5 个匹配，缺失 React Demo 0，额外 Blazor 1；扫描的运行时 HTML/CSS/几何差异均为 0。Icon 页面采样仍遇 React hydration error #418，因此把该结果视为匹配样例的差异清单，不代表所有交互状态已验证。两 tone 原色保留/主色色板路径复查符合之前约定；当前转查 Typography 的首个可修差异。

2026-11-07 Typography `title`：React 原例五行分别为 `h1. Ant Design` 至 `h5. Ant Design`；Blazor 之前额外带中文标题说明，导致 DOM 文本不一致。已同步文本，编译 Docs.Server net10（0 errors），Chromium 浏览器确认五个标题文本、字号/字重/行高与 React 相同（38/30/24/20/16px，600，46/38/32/28/24px）。Typography 当前文档盘点为 React 11、Blazor 14，缺少 `_semantic` 代码盒（页面实际由独立 Semantic DOM 区块呈现），额外 Blazor Debug Demo 4；标题文本修复后估算 HTML 差异 Demo 数降至 8，CSS/几何差异 Demo 数仍为 4/7。其它差异继续按可复现用例逐项处理。

2026-11-07 Typography `copyable`：对照 React `Base/CopyBtn.tsx`，复制动作从带 `role=button` 的 `div` 改为原生 `button`，补齐 `type="button"` 与 `aria-label="Copy"`，移除 React 不输出的 `ant-typography-action` 和内联重置样式。Docs.Server net10 构建通过（0 errors）；已在文档页渲染后读取 11 个 `.ant-typography-copy` 节点，均为 `BUTTON`，且 class、aria-label、type 与 React 结构一致。该次没有重跑全组件 runtime parity，Typography 全量 CSS/几何仍待继续核对。

2026-11-07 文档主题菜单：移除 `DocsThemeSwitch` 上与 Dropdown 内部开关状态重复的受控 `Open` 参数，避免点击关闭后旧值阻止再次展开。Docs.Server net10 构建通过；浏览器实际打开、外部点击关闭、再次点击打开均成功。主题菜单以外的全局同步进度不受此项影响。

2026-11-07 主题菜单重开回归：在 `:5022` Select 文档页分别用触发按钮关闭、点击页面外部关闭及选择主题项关闭，再次点击右上角主题按钮均能重新打开菜单；当前实例三种路径复测通过。

2026-11-08 主题菜单选择关闭回归：最终采用 Dropdown 自主管理展开状态（不传受控 `Open`），选择主题/切换紧凑模式/快乐工作效果通过 Dropdown `Close()` 关闭。当前 Development 文档页复测选择 Dark theme 后重开、外部点击关闭后重开均成功。

2026-11-08 Select 定向复扫：新增 `select-responsive-dom-style-parity.json`。Responsive 的三个根节点 CSS、几何与视觉均一致，但动态 suffix 的 inline 起始位置仍差 0.016px（React 196.797px，Blazor 196.813px），保留为未对齐项。Custom Tag Render 自动复扫确认 CSS/几何/视觉相同，DOM 仍差两个 close SVG 的 `fill-rule="evenodd"` 属性，属于引用的 Icons catalog 输出差异；记录在 DOM/CSS parity 日志，后续从图标源头收敛。

同一服务下重新跑 Icon 五个 canonical groups，结果 5/5 DOM tree、计算样式、几何和视觉样式全部通过，0 blocked；报告 `/tmp/icon-dom-style-parity-current-20261108.json`。这覆盖五个文档示例，不代表 Icon Gallery 全量 842 个图标或所有交互状态均已验收。

## 2026-11-08 主题菜单重复打开：服务运行时校正

- 用户反馈菜单关闭后无法再打开。此前我重启 `:5052` 时错误地用 .NET 11 预览运行 .NET 10 页面，导致 Blazor circuit 建立失败并出现组件 `TypeLoadException`；该运行时故障会让所有服务器端按钮失去响应，不能作为主题菜单自身回归的证据。
- 使用 `/usr/local/share/dotnet` 的 .NET 10.0.9、Development 环境恢复服务后，浏览器实测 Dark、Compact、Happy Work Effect、Light 四种菜单选项均能关闭后再次打开；点击页面外部关闭后也能再次打开。控制台无新的交互错误。未改动 `DocsThemeSwitch` 的开关实现，避免用未验证的受控状态引入问题。
- 当前 `:5052` Development 文档页保持打开，供人工检查。主题菜单问题在正确运行时下未复现；本轮未据此更改组件代码。

2026-11-08 Table 首批记录（状态由 2026-11-09 定向重测更新）：Basic demo 的定向 DOM/CSS fixture 与浏览器检查结果见本文档开头；首次因 demo 懒加载导致的阻断记录已被后续有效采集取代。

2026-11-09 Table Semantic DOM 首批：为 Table 增加 `ClassNames` / `Styles` 语义槽位映射，并接入 React v6 Table 的 13 个槽位（root、section、title、footer、content、header/body wrapper/row/cell、pagination root/item）；增加 `_semantic` 文档样例，含分组表头、6 条记录、分页以及 title/footer。Docs.Server net10 编译成功（0 errors）；重启 `:5000` Development 服务后浏览器实际确认分组表头、3 条当前页记录、分页和 title/footer 均渲染。HTTP SSR 检查 13/13 语义 class 槽位齐全，style 标记 26 处，`git diff --check` 通过。当前结论仅覆盖语义 API 与此示例的运行时呈现；Table 全量 demo、Design Token 和运行 DOM/CSS 差异盘点仍未完成，下一步从 React demo 清单较多的 Table 用例继续逐项对齐。

2026-11-09 Table DOM/CSS fixture 扩展：用 6 份现有 fixture 串行跑 7 个状态，首轮发现非横向滚动表格仍将未设宽度列输出为空 `<col>`；改为仅在 ScrollX 表格或固定表头分离布局的 header colgroup 输出该占位。Pagination 跳页省略符由纯文本 `•••` 改为 React v6 的 `EllipsisOutlined` SVG，并移除无效透出的 `last` 属性。针对 Fixed Columns、Fixed Columns and Header、Row Selection 分别重跑后，三者 DOM tree、computed CSS、visual styles 均一致；多列排序和分页位置在本批首轮也均一致。Fixed Header 仍有一项未收敛：header 流体列缺少 React 运行后写入的 `width:451.328px` inline 宽度，但 computed CSS 与视觉检查一致。尝试在 Blazor 侧直接测 header cell 写入该宽度会造成约 1.2px 固定列偏差，已撤回；保留结构修正。自动化 Chrome 后续批跑曾出现 Network.enable timeout，故不将未重跑的场景计作新通过。Docs.Server net10 构建 0 errors，tableHelper.ts 定向 ESLint 通过；当前 browser tab 仍停留在 Table Semantic DOM 供手工复核。报告位于本机 `/tmp/table-current-round-parity-20261109.json`、`/tmp/table-fixed-columns-after-colgroup-20261109.json`、`/tmp/table-fixed-columns-header-after-ellipsis-20261109.json`、`/tmp/table-row-selection-after-colgroup-20261109.json`、`/tmp/table-fixed-header-reverted-dynamic-width-20261109.json`。

2026-11-09 Table Fixed Header 列宽复核收敛：不再从 header 单元格直接量宽（此前会影响固定列几何），而是用表格容器宽度扣除显式列宽后，将剩余宽度按流体列自然宽度比例分配；纵向滚动时对 header/body table、列与单元格变化做 ResizeObserver 同步。Fixed Header 与 Fixed Columns and Header 两个独立 fixture 现在均为 DOM tree、computed CSS、visual styles 全部一致；React 写入的流体列宽在 Blazor 中也生成相同 `451.328px`。parity runner 的 strict `htmlExact` 仍报告 Razor 多出的文本空白，但结构化 DOM tree 无差异。`git diff --check` 与 `npx eslint components/core/JsInterop/modules/components/tableHelper.ts` 通过；Docs.Server net10 构建 0 errors。语义 DOM 页面仍在浏览器 `:5000/en-US/components/table#semantic-dom-preview` 打开。本轮 Table 已覆盖 Semantic DOM、Fixed Header、Fixed Columns、Fixed Columns and Header、Row Selection、多列排序与分页位置；尚需继续清查其余 React demos、Design Token 和样式差异。固定表头报告 `/tmp/table-fixed-header-fluid-width-parent-20261109.json`、`/tmp/table-fixed-columns-header-fluid-sync-20261109.json`。

2026-11-10 Table Demo slug 对齐：React 有 53 个 Table markdown demos，Blazor 现有 77 个，数量差异来自 Blazor 扩展/调试示例；比对精确 slug 后唯一 React-only 项是大小写不一致的 `tree-table-preserveSelectedRowKeys`。将文档 Markdown 文件名从 kebab-case 改为 React v6 camelCase canonical slug，并运行 `demo2json` 重生成 en-US/zh-CN 元数据；结果 React-only 为 0、React/Blazor 对应项 53/53。用 .NET 10 SDK 构建 Docs.Server 成功（0 errors；现有 922 warnings），重启 Development 服务并刷新现有 Table 浏览器页。其它 24 个 Blazor-only demos 暂保留，下一轮按真实组件能力/调试用途区分；Table 其余未覆盖 React demos 及 Design Token 仍需继续核验。

2026-11-10 Form Basic 首轮对齐：清单扫描 React 44 / Blazor 57，44 个 React demo slug 均匹配，Blazor-only 13 个。新增 `form-basic-dom-style-parity.json`，采集 Basic 的初始态；对照后为 Blazor Form 补齐 React 的 `ant-form-css-var` 根类，FormItem 补齐水平布局及 CSS 变量类，Basic 表单设置 `max-width:600px` 并修正 `User Name` 为 `Username`。普通交互表单不再额外输出默认 `method=GET`（具名静态表单仍保留配置方法）。修复 FormItem 空 `Help` 被包装成空校验提示节点的问题，移除多余 explain/margin-offset DOM。Docs.Server net10 构建通过（0 errors；416 warnings）。自动对照的结构差异由初始 43 项降到 21 项（中间检查 41 项）；Form 根计算样式与根视觉样式匹配，但仍有 class 顺序、label `for`/title、输入 name/style/aria、密码可见性按钮图标等结构差异。标签列宽还差 0.016px（React 199.984px，Blazor 200px），所以本例整体仍标记为未对齐。报告 `/tmp/form-basic-before-20261110-v2.json`、`/tmp/form-basic-after-20261110-v2.json`。Form 文档已在浏览器新标签打开，Table Semantic DOM 标签保留。

2026-11-10 DatePicker Token 同步：React v6 通过 `Exclude<SharedComponentToken, 'addonBg'>` 将 `addonBg` 排除在 DatePicker Token 类型外，但原同步器仍把 token-meta 中的行并入文档，导致 Blazor 文档展示了组件 C# 类型不支持的 token。Token parser 现在解析继承以及 `Exclude`/`Omit`；重新生成 IR、强类型组件 Token 和 `llms-tokens.json` 后，DatePicker 文档表从 40 行减到 39 行。Focused parser / phase-one 测试 6/6 通过；phase-one 审计为 583/583 组件 token、0 项 metadata mismatch。浏览器读取新页面确认 `addonBg` 不存在、`presetsWidth` 存在。DatePicker Basic 弹层 parity fixture 1/1 通过，DOM tree、计算样式、几何、视觉样式均无差异，报告 `/tmp/datepicker-basic-popup-after-token-sync-20261110.json`。页面保留在 `http://127.0.0.1:5000/en-US/components/datepicker?theme=light#design-token` 供人工检查。

2026-11-10 DatePicker Multiple demos：补齐 React v6 `multiple` 与 `multiple-debug` 对应的 Blazor 示例及双语描述；Responsive MaxTagCount 使用真实的容器尺寸/标签测量，不再固定显示数量。CLI 生成的 en-US/zh-CN 元数据均已包含 `multiple-debug`（debug 标记与 slug 正确），Docs.Server net10 构建通过（0 errors；513 warnings），`git diff --check` 通过。先前浏览器交互已验证增选日期和移除标签会同步更新选择态、弹层继续打开；本轮重建后尝试重连浏览器时，Cua 浏览器桥返回 native pipe unavailable，故无法复验新生成页面的视觉/弹层状态，也没有将其记作本轮浏览器通过。当前服务已重启并监听 5000；后续恢复浏览器通道后应验证 debug 开关展示与窄宽度 MaxTagCount 响应式计数。该组件的运行 DOM/CSS parity 与 Token 已完成局部核对，尚非全量对齐。

2026-11-10 Table 运行产物与 Fixed Header 收敛：对照运行时后发现源码 `tableHelper.ts` 已有列宽同步，但文档站实际加载的 `ant-design-blazor.js` 仍为旧 bundle；此外 fluid header 列上残留的初始宽度被误当成显式宽度。现在从 body colgroup 识别明确列宽，重新 Browserify/tsify 编译主 bundle 并重建 Docs.Server。旧的 `npm run build:lib` 任务因 `scripts/build/compile-styles`、`generate-less-vars`、`migration-styles` 模块缺失而无法运行；本轮直接调用现有 Browserify/tsify 工具链验证当前 TypeScript。Table 六组 fixture 共 7/7 状态现已通过，DOM tree、计算样式、几何、视觉差异均为 0；Fixed Header fluid 列宽两侧均为 451.328px。另修复 parity 工具对隔离 `<tr>` 片段上下文错误的问题，并增加回归测试。最新报告 `/tmp/table-regression-suite-20261110-final.json`；Docs.Server net10 构建 0 errors，StyleGen HTML 结构用例 4/4 通过，`git diff --check` 通过。Cua 浏览器桥不可用，本轮由 headless Chrome 自动化完成，无法为手工检查保留可见标签。下一步继续覆盖 Table 未扫描的 React demos、Semantic DOM 与 Token，并按 demo 数量继续推进其他组件。

2026-11-10 Select/Icon parity recheck: updated the local `AntDesign.Icons` package to preserve `fill-rule="evenodd"` in generated Close SVGs; Select Custom Tag Render now matches. Select Responsive was initially blocked by the 20s runner timeout during lazy demo mounting; the same scenario with a 60s timeout passes 1/1 with zero DOM/style/geometry/visual differences. Icon canonical public demos pass 5/5 with zero differences after `CodeBox` demos were changed to initialize on approach to the viewport, preventing duplicate IconFont symbol injection. Docs.Server returns HTTP 200 on port 5000 and the Select Responsive page is open in Chrome. These targeted fixtures do not complete all Select interaction states or the full 842-icon gallery. Reports: `/tmp/select-responsive-20261110-report.json`, `/tmp/icon-dom-style-parity-lazy-20261110.json`.

2026-11-10 Select 扩展复核：Select 核心尺寸/Variant/Placement 6/6、搜索筛选/多选/Basic 弹层交互 6/6、Suffix 1/1、虚拟大数据 1/1、Semantic DOM 关闭态 1/1、Responsive 1/1 均通过；Basic 与弹层状态另有 3/3 通过。Suffix fixture 原先只等待外层锚点、不滚动到 `.code-box-demo`，导致懒加载 skeleton；调整作用域后通过。Semantic DOM 打开弹层和 Custom Tag Render 的本轮单独重跑因预览懒加载没有完成而阻塞，先前记录的成功结果仍有效，但应在下一轮单独复验。Icon 五个公共示例重跑 5/5 通过，DOM/CSS/视觉差异为 0。所有服务使用 Development 环境；当前浏览器仍保持在 Select 文档页。报告见 `/tmp/select-dom-core-20261110-dev-report.json`、`/tmp/select-basic-popup-20261110-report.json`、`/tmp/select-suffix-20261110-report.json`、`/tmp/select-expanded-suite-policy-20261110-report.json`、`/tmp/icon-dom-style-parity-after-header-20261110.json`。

2026-11-10 品牌与 Dropdown Basic：文档头部恢复 React 标志与“Ant Design”名称，并将“Blazor”以小号上标呈现。Dropdown Basic 关闭态/hover 触发器通过 2/2；Overlay 弹层改用 `block` 后，其定位几何、计算样式与视觉样式相符，但菜单项空 style 属性/runtime ID、class 顺序和 portal sentinel 仍造成 DOM 结构差异，不能标记为全对齐。Docs.Server 编译 0 错误；完整报告 `/tmp/dropdown-basic-dom-style-parity-final-20261110.json`。

2026-11-12 Icon repeat verification: rebuilt Docs.Server and restarted it in Development before comparing the five canonical React v6 Icon demos. The fresh run passes 5/5 with zero DOM, selected computed-style, geometry, or visual-style differences (`/tmp/icon-dom-style-parity-current-20261112.json`). An earlier same-day capture intermittently reported a duplicate `#icon-facebook` symbol order/geometry mismatch while the React and Blazor pages were mounting the lazy ScriptUrl demo. The fresh canonical rerun did not reproduce it, so no speculative loader change was made. The Icon Gallery responsive fixture remains 2/2 passing (`/tmp/icon-gallery-responsive-current-20261112.json`); this does not assert every one of the 842 gallery icons or all Icon interactions.

2026-11-12 Table Ellipsis demo alignment: the Blazor Ellipsis demo had incorrectly embedded custom Tooltip templates and disabled the default title behavior, overlapping the separate Ellipsis with Custom Tooltip demo. It now mirrors React v6's six columns and plain ellipsis cells, fixes the `Column.Ellipsis` documentation typo and the `Sydney` sample data, and uses React's `1`/`2`/`3` row keys. Added `table-ellipsis-dom-style-parity.json`; after the row-key correction, the 1280px browser comparison passes 1/1 with zero DOM, selected CSS, geometry, and visual-style differences (`/tmp/table-ellipsis-dom-style-parity-20261112-after-rowkey.json`). Docs.Server net10 build succeeds (0 errors; existing repository warnings remain). The Ellipsis demo has been mounted and left open at `http://127.0.0.1:5000/en-US/components/table#components-table-demo-ellipsis` for manual review.

2026-11-12 Select outside-click handling: the shared OverlayTrigger now listens to document `click` as a fallback to `mouseup`, and checks pointer coordinates against both trigger and popup bounds before dismissing. This covers click paths without a mouseup and avoids treating an interaction within the popup as an outside click. Rebuilt Docs.Server for net10 (0 errors; existing repository warnings remain), restarted it, and verified in the browser that opening a Select then clicking a separate demo closes the popup (`open: 1 → 0`, visible popup `1 → 0`). `git diff --check` passes.

2026-11-12 Select outside-click report follow-up: rebuilt the current Docs.Server output and restarted the Development server, then rechecked both the Basic demo and the Select Semantic DOM demo. In each, the popup opens and a click outside its trigger/popup closes it (`visible popup: 2 → 0` for Basic, `visible → hidden` for Semantic DOM). The current OverlayTrigger fix is active; no additional component change was needed in this verification pass.

2026-11-12 Select controlled Semantic DOM follow-up: the page's Semantic DOM Select used constant `Open=true`, so its parent kept forcing it open after an outside-click close request. Changed the Semantic DOM demo and reusable `SelectSemanticPreview` to start open with `@bind-Open`, allowing the parent state to accept close/reopen requests. Docs.Server net10 build succeeds (0 errors; existing warnings remain). Browser verified the Basic demo opens and closes on a separate card click, and the Semantic DOM Select opens then closes when clicking blank space in the preview column.

2026-11-16 Slider Basic parity: matched the React Basic example's default value and demo composition, removed the unnecessary tracks wrapper from ordinary sliders and empty marks element, added handle orientation ARIA, and aligned percentage inline styles. Docs.Server `net11.0` build succeeds with 0 errors. `slider-basic-dom-style-parity.json` passes 1/1 with normalized DOM tree, selected CSS, geometry, and visual styles equal; raw HTML serialization differs only in attribute order and Blazor render markers. Other Slider demos/API/tokens/Semantic DOM remain open. Resume Select API/docs closure, then Icon docs/token work. See `runtime-parity-summary-2026-11-12.md` and `dom-style-parity.md`.

2026-11-16 Select DefaultOpen: added an uncontrolled first-render `DefaultOpen` parameter corresponding to React `defaultOpen`, documented it in both locales, added its demo, and refreshed compiled metadata. `net11.0` Docs.Server build succeeds (0 errors; 591 existing warnings). Browser verified the initial popup is visible and outside click closes it. Since React has no matching demo, this is an API behavior check rather than a DOM parity pass. Select route inventory now has 52 Blazor demos / 28 React matches; its broad H/C/G scan remains stale. The Select page is open at port 5011. Continue Select API/interaction reconciliation, then Icon docs/tokens.

2026-11-16 Select OptionRender: added a React-compatible `OptionRender` parameter and retained `ItemTemplate` as its fallback alias; updated the existing Option Render demo and both API docs. Docs.Server `net11.0` build succeeds (0 errors). Focused fixture `/tmp/select-option-render-option-api-20261116-final.json` reports equal normalized DOM tree, computed CSS, geometry, SVG use, and visual styles; strict raw HTML differs only due Razor whitespace serialization. Browser remains open at the Blazor Option Render demo on port 5011. Continue remaining Select APIs and popup interactions, then Icon docs/tokens.

2026-11-16 Design Token value visualization: added the shared `DesignTokenValue` renderer to component/global Design Token tables; CSS colors show a circular swatch (including irregularly named component colors such as `defaultActiveBg`), while shadow tokens remain plain. `motionEase*` cubic-bezier values show the easing graph and React-matched green/red control points. Docs.Server net11 build succeeds (0 errors, 591 existing warnings). Browser confirms 47 swatches and one 160px curve preview on Button, with the computed blue curve and grid. This updates rendering only; it does not change token coverage counts or parity state. Browser left on `http://127.0.0.1:5011/en-US/components/button#design-token`.

2026-11-16 Icon 文档结构/API：将 Icon Gallery 从 Examples 中移出，作为独立的 “List of icons” 区块放在示例前，目录也单列该锚点；修正 `Role`、`Spin` XML 说明及中文翻译，API 手册补充 `OnKeyDown`。CLI 重新生成中英文组件元数据；Docs.Server net11 编译通过（0 错误，591 条现存警告）。浏览器对照确认 Blazor 与 React 均为 How to use → List of icons → Examples → API → Design Token；Blazor gallery 搜索、六个分类和 842 个图标节点可见。Icon 全 API/Token/FAQ 与所有示例的 DOM/CSS 对齐仍未完成。端口 5011 保持运行，浏览器停留在 Blazor Icon 文档，React 参考页保留在相邻标签。

2026-11-16 Select React API 名称：新增 `ListHeight`（映射旧 `PopupContainerMaxHeight`）、`Prefix`、`RemoveIcon`、`MenuItemSelectedIcon`，并保留既有 Blazor 参数和默认图标。英文/中文 API 文档和 `api-aliases` 示例已同步；CLI 更新两种语言的生成元数据，net11 Docs.Server 编译 0 错误（591 条现存警告）。Development 服务已在 5011 重启，浏览器验证 128px 列表高度、前缀、标签移除图标和选中勾选图标；点移除图标可移除选中项，控制台无错误。新 demo 只验证 API 接线，不作为与 React 同名 demo 的 DOM parity 结果。Select 现在有 54 个 Blazor demo，其中全部 28 个 React demo 名称均有对应项；其余精确 API 与整页 DOM/CSS/交互检查仍未完成。下一步继续 `fieldNames`、`optionFilterProp`、受控 `SearchValue`，再完成 Icon 文档/token 审计。

2026-11-16 Select filtering / LabelRender：浏览器对照 React 与 Blazor 的 Search and sort，输入 `i` 后两边都按 Communicated → Identified → Not Identified 排列；FilterOption=false 在输入不匹配文本后仍保留选项。新增 React 命名的 `LabelRender` 别名，现有 Custom Label Render demo 改用该名称，补齐英文/中文文档、静态翻译并重新生成组件元数据。Docs.Server net11 构建 0 错误（591 条警告）；浏览器确认选中标签显示 “No option match”，中文 API 表描述已翻译。Select 仍为 28/28 个 React demo 对应、共 54 个 Blazor demo；本轮没有新增 demo。服务器保持运行于 5011，Blazor 页停留在 Select 自定义选中标签 demo，React Select 保留在相邻标签。下步处理 `fieldNames`、`optionFilterProp` 和受控 `SearchValue`，之后继续 Select 整页核对，再回到 Icon 文档/token 差异。

2026-11-16 Select `showSearch` 配置：`ShowSearch` 支持布尔值或配置对象；配置对象接受单个/多个 `OptionFilterProp` 字段及 `FilterOption`，多个字段按 OR 匹配。顶层 `OptionFilterProp` 保留为 obsolete 兼容别名，Multi field search 示例已按 React v6 的 `showSearch={{ optionFilterProp: [...] }}` 形态更新。补齐中英文 API/demo 文案并重新生成元数据。Docs.Server net11 构建 0 错误（591 条现存警告）；浏览器两边输入 `c11` 均匹配 `a11`。定向 DOM/style fixture 1/1 通过，标准化 DOM、样式、几何及视觉检查一致；原始序列化差异仅为 Razor 空白与属性序列化。本轮未刷新全组件 H/C/G 扫描。Select 仍为 28/28 个 React demo 名称对应、共 54 个 Blazor demo；受控 `SearchValue` 和整页/API 审核待续。Icon 与其余 69 个组件状态见 71 组件总表。下一步继续 `SearchValue`，完成 Select 剩余审计后转回 Icon 文档/Token。

2026-11-16 Select 受控搜索值：新增 `SearchValue` / `SearchValueChanged`，支持 `@bind-SearchValue`；外部参数变化会同步 Select 内部搜索文本并刷新过滤结果，关闭弹层时只隐藏输入文本，不再清空受控状态。新增 Controlled search value 示例，CLI 已重新生成中英文元数据。Docs.Server net11 构建 0 错误（591 条现存警告）。浏览器把外部输入改为 `jack` 后，Select 搜索框同步为 `jack`，弹层只显示 `Jack`，控制台无错误。该项是受控交互验证，不等同 React DOM/style parity fixture。Select 当前 55 个 Blazor demo，全部 28 个 React demo 名称有对应项；整页检查与 broad H/C/G 重扫待续。71 个组件的全量进度仍见 runtime-parity-summary 总表。下一步继续 Select API/整页审核，之后回到 Icon 文档与 Token。

2026-11-17 App demo 与 Semantic DOM preview 注册：App 的 Basic/Config Markdown 描述改为 Dim Sum 要求的 `zh-CN` / `en-US` 分区；demo 回调在级联 `AppContext` 为空时使用注入服务，浏览器点击确认 message、modal、notification 均可触发且无异常。App 页面补充 Blazor `.ant-app` root 的 Semantic DOM 描述/预览，并明确 App 复用全局 Design Token。React App 文档没有 Semantic DOM 部分。

同轮重构 Semantic DOM 预览分发：Dim Sum `DemoGenerator` 编译期生成 component-key → `Type` 注册表，优先选择 `_semantic` demo，其次选择排序后的首个 demo；对 Message、Notification、Cascader、Image、Menu、Collapse、Tag、Splitter、Tooltip、Dropdown、App、QRCode 注册专用预览类型，FloatButton Group 和 Card Meta 作为生成的附加预览注册。`Components.razor` 统一通过 `SemanticDomCatalog` 查表并用 `DynamicComponent` 渲染，移除按组件名分支以及运行时反射。App 页面确认标题、描述、全局 Token 表、`.ant-app` 语义预览均输出；三个 Basic 按钮与 Config demo 按钮触发后分别出现正确 overlay。Docs.Server net11 构建成功（0 errors，641 warnings）；服务运行于 `http://127.0.0.1:5000`，Chrome 中 App 页面保持打开供手工检查。下一步复核所有组件的语义预览注册 fallback，并继续 Select 全页/API 差异审计。

Semantic DOM registry smoke verification: Chrome 自动打开 Select 与 Table 临时页，DynamicComponent 注册分别渲染了 Select（含预期打开的 popup）与 Table（3 行）；两页均无脚本异常，临时页已关闭，App 手工检查页仍保持打开。

2026-11-17 Table Drag Sorting with Handler：拖动手柄已移到首个 ActionColumn，排序脚本限定使用首列图标按钮作为指针拖动入口，与 React 的列顺序和交互结构一致。React/Blazor 定向 fixture 两组两场景全部通过，规范化 DOM、computed styles、visual styles 一致；同一逻辑目标（John Brown 拖到第二行）两边释放后均为 Jim、John、Joe。Blazor 的位移预览已直接采样；本轮未单独采样 React 的中间预览。报告 `/tmp/table-drag-sorting-handler-20261117-final.json`。本轮关闭 Table 的一个 demo；下一项为 Drag Column Sorting，之后按差异量继续剩余 Table demo 和 71 组件总审计。Docs.Server 保持在 5000，手工检查用 App 标签保留打开。

2026-11-17 Table 现行构建复核：Drag Column Sorting 已有 2026-11-15 的浏览器拖动与浮层验证记录；本轮创建的临时页面没有及时完成 React/Blazor 懒加载 hydration，不计作新增通过。Virtual List 最新 `scroll-to-index-999` fixture 1/1 通过（DOM、所选 computed styles、visual styles 均一致），报告 `/tmp/table-virtual-list-active-20261117.json`。Style/Class 的虚拟表格 fixture 仍有 17 个规范化 DOM 差异，集中在 table/column 内联宽度；所选 computed styles 与 visual styles 一致，报告 `/tmp/table-style-class-virtual-active-20261117.json`。下一步对齐这组实际测量宽度并复跑，再继续 Table 其余 demo。完整 71 组件矩阵仍未完成。

2026-11-17 Table Style/Class 虚拟表宽度复核：`tableHelper` 现在按 rc-table 的容器宽度和 fluid column floor/remainder 算法分配列宽，补上虚拟表格宽度；row `min-width:100%` 与 React virtual row 的容器适配一致。复跑 `/tmp/table-style-class-virtual-width-fixed-20261117.json` 后，规范化 DOM tree、所选 computed styles 和 geometry 均一致，先前 17 个 inline table/column width 差异已关闭。仍有 3 个重复的末列 visual computed width 记录为 185.312px（React 185.328px），即 0.016px；DOM inline width 完全相同，属于当前 Chromium flex layout 子像素量化，暂不加偏离 React inline 结构的 hack。Raw HTML 字符串差异仅 Razor whitespace。下一步按 React-only/demo 差异量继续 Table 其余 demo；完整 71 组件 API、Design Token、文档、Semantic DOM、Theme Preview 与运行时审计仍未完成。

2026-11-17 Table Ajax：对齐 React 的 MockAPI 数据源、Name/Gender/Email 列以及服务端分页、排序、筛选请求；初始查询忽略 `SortDirection.None`，避免误传降序。修正 fixture 以点击筛选菜单项的稳定 `data-menu-id` 后缀（两边 checkbox wrapper 类不同）。初始加载、Name 排序、Male 筛选三场景 3/3 通过，normalized DOM、computed CSS、geometry、visual styles 均无差异；报告 `/tmp/table-ajax-dom-style-parity-20261117-final.json`。只关闭 Ajax demo；下一步继续 Table 剩余 demo。完整 71 组件矩阵仍未完成。

2026-11-17 Semantic Preview 注册表整理：dim-sum 现生成按 component name 索引的统一预览注册表，包含组件类型、SemanticDom 包装状态及同组件的附加预览；`SemanticPreviewCatalog` 负责按 key 查询，`Components.razor` 只循环注册结果并通过 `DynamicComponent` 渲染，移除了页面中的组件特判和多份分散字典。Docs net11 编译通过（warnings 仍存在）；`git diff --check` 通过。Semantic DOM 节点数据仍由 `SemanticDomCatalog` 单独提供。

2026-11-17 Semantic DOM preview 分发表后续整理：`Components.razor` 直接从 dim-sum 生成的 `GeneratedDemoIndex.SemanticPreviewTypes` 按规范化 component name 查询预览类型；Semantic DOM 数据目录不再承担预览注册职责，补充预览注册也统一使用 Docs Components 层类型。net11 Docs 编译通过（0 errors，631 warnings）。此编译验证注册表生成与 Razor 消费可编译；没有重启服务或切换保留给手工复核的 App 浏览器页。

2026-11-17 Table Hidden Columns：net11 Docs.Server 重建后用 Development host 验证，避免 Production host 下组件静态资源 404。fixture 改为按交互状态断言 Semantic DOM 数量，并规范化等价的 inline style 声明空格；初始和隐藏 Column 2 两个状态 2/2 通过，DOM、computed CSS、geometry、visual styles 无差异。浏览器手动验证列数 8→7→8。测试：`node --test --test-name-pattern='scenario-specific semantic minimum counts' tools/AntDesign.StyleGen/test/dom-style-parity.test.mjs` 1/1 通过。完整 parity 工具测试文件另有不相关失败（Select Escape motion 与两个 JSDOM `cloneNode` 案例），不计入此 demo 的结果。Blazor 文档保持在 `http://127.0.0.1:5000/en-US/components/table#components-table-demo-hidden-columns` 供检查。下一步：Table Drag Column Sorting；然后继续 Table 余下差异，再回到 Select/Icon 全页和 Design Token/文档总审计。

2026-11-17 Table Drag Column Sorting：fixture 增加可验证重排结果的 pointer drag action，交互完成前保留 150ms 让 React dnd-kit 消化最后一次 pointermove；失败阶段现在标注 React/Blazor 端。Blazor 表头补齐 React 一致的 `cursor: move` inline style，并移除仅 Blazor 存在的 Table class。初始和 Name 拖到 Age 后两个状态 2/2 通过，normalized DOM、computed CSS、geometry、visual styles 无差异；报告 `/tmp/table-drag-column-sorting-20261117-final.json`。net11 Docs.Server build 0 errors（4204 warnings）。保留浏览器在 Blazor Table Drag Column Sorting 页面供手工检查。下一步继续 Table 剩余差异，再回到 Select/Icon 全页及 Design Token、文档、Semantic DOM 和 Theme Preview 总审计；完整 71 组件仍未完成。

2026-11-17 Table Auto Height：移除旧版 Layout/Sider/高级搜索页面，改为 React 同款 More Data 开关（30/2 行）、400px 容器、20/20/60 列宽；使用 ResizeObserver 按容器、表头和分页测量表格区高度，并同步固定表头、滚动高度及像素列宽。补齐 React 相同的 header/pagination 语义类、双语说明和 Dim Sum 生成器需要的 markdown 双语 title frontmatter。Focused fixture 两个状态 2/2 通过，normalized DOM 与 visual style 均无差异；仅对像素宽度按 0.1px 精度归一，以容纳浏览器测量序列化的千分像素差异。报告 `/tmp/table-auto-height-20261117-verified-final.json`。Docs.Server net11 构建 0 errors（595 warnings）。浏览器保留在 Blazor Auto Height 页面供手工检查。下一步继续 Table 剩余 demo 差异，并推进 Select/Icon 全页、全部 Design Token、文档、Semantic DOM 与 Theme Preview；71 组件总审计仍未完成。

2026-11-18 Table Measure Row Render：完成 measurement row 与 sticky header 分裂结构同步，修复百分比列宽测量，并对齐测量行的 Name/Age 筛选控件、Address 排序提示初始展开状态。Docs.Server net11 构建通过（0 errors，4194 条现存警告）。首次冷启动 fixture 遇到 60 秒 Blazor 导航超时；服务预热后重跑成功，1/1 场景通过，规范化 DOM、选定 computed styles、geometry 与 visual styles 一致，无差异。报告 `/tmp/table-measure-row-render-20261118-warm.json`。Blazor 和 React Table 文档页保留打开供手工检查。本轮仅关闭此 demo；Table 其余 demo 及完整 71 组件 API、Design Token、文档、Semantic DOM、Theme Preview/runtime 总审计仍未完成。下一步继续按差异量处理 Table。

2026-11-18 Table Narrow 与回归：Narrow demo 的第 13 页分页两侧页码已同步 React 的跳页窗口规则；`--columns-count` 仅在 sticky 或含固定列的表格输出，避免普通表格出现多余 DOM style。Narrow fixture 1/1 通过，normalized DOM、selected computed styles、geometry、visual styles 全部一致，报告 `/tmp/table-narrow-dom-style-parity-20261118-fixed-rerun.json`。Measure Row Render 回归 1/1 通过，报告 `/tmp/table-measure-row-render-20261118-final.json`。Docs.Server net11 构建 0 errors（3623 条现存警告）。仅完成这两个 demo 状态；其余 Table 与 71 组件 API、token、文档、Semantic DOM、Theme Preview/runtime 审计仍待完成。下一步继续 Table 剩余 demo，按当前 React/Blazor 差异量排序。浏览器保留在 React/Blazor Narrow 页供手工检查。

2026-11-18 Table Order Column 与 Filter Search：Order Column 移除 React 不存在的 Blazor-only `RowExpandable` 限制；checkbox 隐藏 input 增加 `font-weight: inherit`，并同步 15 个已生成主题 CSS bundle。Order Column fixture 1/1 通过，normalized DOM、computed styles、geometry、visual styles 无差异，报告 `/tmp/table-order-column-dom-style-parity-20261118-final.json`。Filter Search demo 补齐中英说明，行筛选改为与 React `startsWith` 一致的 ordinal 大小写敏感匹配；默认表格、打开 Name 树筛选、输入 `Category 1` 搜索选项 3/3 通过，DOM/CSS/geometry/visual styles 无差异，报告 `/tmp/table-filter-search-dom-style-parity-20261118-final.json`。Docs.Server net11 构建通过（0 errors，4211 条现存警告），浏览器确认生成说明可见。仅关闭这两个 Table demo 状态；其余 Table 与完整 71 组件 API、Design Token、文档、Semantic DOM、Theme Preview/runtime 审计继续进行。下一步继续 Table 剩余 demo，并保留 React/Blazor 对照页供手工查看。

2026-11-18 Table Grouping Headers：对齐 medium 尺寸、`calc(50% + 700px)` 横向滚动宽度、235px 纵向滚动高度、0px scrollbar gutter、行 key 和列名；跨列表头改为 `scope="colgroup"`。零宽 scrollbar 不再输出 scrollbar-width class/CSS 变量；parity normalizer 仅移除 Blazor 内部 `--columns-count` inline 声明，并采样检查其伪元素 z-index 和 geometry 效果。Grouping Headers fixture 1/1 通过，DOM、computed styles、geometry、pseudo z-index、visual styles 一致，报告 `/tmp/table-grouping-columns-dom-style-parity-20261118-complete.json`。ColSpan and RowSpan 回归 1/1 通过，报告 `/tmp/table-colspan-rowspan-dom-style-parity-20261118-regression-final.json`。Docs.Server net11 构建 0 errors（4194 warnings）。Table 其他 demo 及 71 组件 API/token/docs/Semantic DOM/Theme Preview/runtime 总审计未完成。

2026-11-18 Table Head：页面缺少 React 的 Header demo，根因是 Dim Sum 将 `Head.razor` 与无 Markdown 的旧 `Head_.razor` 归一化后碰撞，使 head.md/Razor 配对整体被跳过。生成器现优先按大小写不敏感的精确文件 stem 配对，否则只接受唯一归一化候选；补齐 head.md 中英说明后，页面已生成 `components-table-demo-head`。fixture 1/1 通过，DOM、computed styles、geometry、visual styles 一致，报告 `/tmp/table-head-dom-style-parity-20261118-rerun.json`。Dim Sum 修复提交并推送至 `codex/blazor-docs-generator`：`2be0c39`。Docs.Server net11 构建 0 errors（645 warnings）。

2026-11-18 Table JSX Composition：将泛化 parity 占位 demo 替换为 React 对应的 JSX 组合示例：两层 Name 表头、3 条数据、Tags/Flex 渲染和 Invite/Delete 操作；补齐中英说明。Fixture 检查 5 个分组表头、2 个叶子表头、18 个单元格、5 个 Tag 和 6 个操作链接，1/1 通过；归一化 DOM、computed styles、geometry、visual styles 均一致。仅 raw HTML 因 Razor 空白字符不同；React 侧采集有站点级 404/贡献者请求日志，Blazor 无导航错误。报告 `/tmp/table-jsx-dom-style-parity-20261118.json`。Docs.Server net11 构建 0 errors（600 warnings）。Table 仍未完成；71 组件 API、Design Token、文档、Semantic DOM、Theme Preview 和 runtime 总审计也仍未完成。下一步继续 Table 剩余 demo。
