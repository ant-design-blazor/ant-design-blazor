# Ant Design v6 同步器

从 React `ant-design` 的指定 Git revision 重新生成 Blazor v6 的迁移输入、强类型 Token、React CSS 基准和颜色算法测试快照：

## React API canonical IR

React API 是迁移的唯一源头。先从 React 组件源码提取 Props、继承的 Props、原始
TypeScript 类型和 callback 签名：

```bash
node tools/AntDesign.StyleGen/src/extract-react-api-to-json.mjs \
  /Volumes/Data/GitHub/ant-design \
  docs/antd-v6/react-api.json
```

后续 Blazor 参数只作为目标映射和一致性校验输入，不能反向筛选或改写 React 场景。

第一阶段进度使用统一脚本计算，汇报时同时显示 `total`、`migrated` 和 `pending`：

```bash
node tools/AntDesign.StyleGen/src/phase1-progress.mjs \
  /Volumes/Data/GitHub/ant-design-blazor \
  docs/antd-v6/phase1-progress.json

# 可选：同时用 React v6 CSS-in-JS 原编译器生成文档站 light+compact 与
# dark+compact 组合样式；它们是静态预构建文件，不使用运行时 Token 编译。
node tools/AntDesign.StyleGen/src/cli.mjs css-baseline-update \
  --antd-path /Volumes/Data/GitHub/ant-design \
  --output /tmp/antd-v6-react-css-baseline \
  --docs-theme-output /Volumes/Data/GitHub/ant-design-blazor/components/wwwroot/css/antd-v6

# 从 React 文档构建产物中的 semantic.md 同步正式 slot、说明和抽象 DOM 到 JSON。
# 命令也会扫描 SemanticType 的 classNames/styles 路径并做差异审计；AST 清单用于发现
# 文档未呈现的类型路径，不再作为正式文档 slot 清单的替代来源。
npm install --prefix tools/AntDesign.StyleGen
node tools/AntDesign.StyleGen/src/cli.mjs sync-semantic-dom \
  --antd-path /Volumes/Data/GitHub/ant-design \
  --semantic-docs-path /Volumes/Data/GitHub/ant-design/_site \
  --blazor-path /Volumes/Data/GitHub/ant-design-blazor

# Semantic DOM 与 demo/menu/components JSON 作为 AdditionalFile 输入；AntDesign.Docs.Generator
# 在编译期生成 catalog、强类型导航/组件/demo 对象和 demo typeof 映射；运行时不再下载或反序列化这些 JSON。
dotnet build site/AntDesign.Docs/AntDesign.Docs.csproj -f net10.0
```

React 文档必须先构建/导出到 `_site`（或通过 `--semantic-docs-path` 指定其他导出目录）。
`docs/antd-v6/semantic-dom-catalog.json` 的组件与 slot 名称、说明和 React 抽象 DOM 以这些
`semantic.md` 文件为正式同步目标；`semantic-dom-inventory.json` 则通过 TypeScript AST 展开
SemanticType 的交叉继承、`SemanticAllType` 包装、嵌套 slot，以及没有 `*SemanticType` 别名时
公开 Props 上的 classNames/styles schema，用作 API 完整性对照。两边不一致时
同步仍会刷新文档 catalog，但命令以非零状态报告差异，需确认是文档缺项、类型遗漏还是路径别名。
StyleGen 自己声明 TypeScript 与 jsdom 依赖，不要求从相邻 React 仓库解析这些工具包。
selector 是 Blazor 预览的目标映射：已有映射在同步时按 component/slot 稳定保留，新 slot
使用默认类名推导；组件实现或 DOM 结构改变时，应更新 JSON 中对应 selector 并补充结构测试。

CSS 生成器会在 React `ConfigProvider` 中显式设置
`theme.cssVar.key = 'css-var-root'`，避免 React `useId()` 产生
`css-var-_R_*_` 这种随渲染树变化的编译期标识。输出文档主题 CSS 时，脚本会将
这个确定性的 React 内部 key 归一化为由
`ConfigProvider` 提供的 `.ant-theme` 主题祖先作用域；主题变量仍通过
`ThemeConfig`/`data-theme` 传递，切换主题只需替换对应主题 CSS 文件，不依赖
React 渲染次数或随机 class。重新生成主题 CSS 后，同时更新
`docs-theme-modes.manifest.json` 的 scope 和摘要值。

第二阶段将同步 React v6 的属性命名和弃用关系。弃用清单直接来自上游 v6 迁移文档，
当前只保留提取结果作为后续 Design Token/API 迁移的输入：

```bash
node tools/AntDesign.StyleGen/src/extract-react-v6-api-migrations.mjs \
  /Volumes/Data/GitHub/ant-design/docs/react/migration-v6.en-US.md \
  docs/antd-v6/react-v6-api-migrations.json
```

也可以直接比较 React v4 最后版本与 v6 最新版本的 Props API。脚本会从两个 Git tag
提取 Props IR 后再做差异，避免把实现内部变量误认为公共属性：

```bash
node tools/AntDesign.StyleGen/src/extract-react-tag-api-diff.mjs \
  /Volumes/Data/GitHub/ant-design \
  4.24.16 \
  6.6.4 \
  docs/antd-v6/react-v4-v6-api-diff.json
```

输出同时记录旧 tag、新 tag、移除属性和新增属性；属性是否属于弃用替换关系仍以
React v6 migration 文档为语义依据，不能只根据 diff 中的同名或近似名自动推断。

生成结果用于建立 Blazor 参数迁移表：新参数按 React v6 语义命名，仍保留的旧参数必须
添加 `[Obsolete("Use ... instead")]` 并转发到同一实现路径。无法保持类型或语义一致的
属性必须进入差异报告，不能按名称相似度自动映射。

先提取 Blazor 侧能力清单，再生成 React-first 映射报告：

```bash
node tools/AntDesign.StyleGen/src/extract-blazor-api-to-json.mjs \
  /Volumes/Data/GitHub/ant-design-blazor \
  docs/antd-v6/blazor-api.json

node tools/AntDesign.StyleGen/src/map-react-api-to-blazor.mjs \
  docs/antd-v6/react-api.json \
  docs/antd-v6/blazor-api.json \
  docs/antd-v6/react-to-blazor-api-map.json
```

映射报告必须保留 React IR 中的每个组件和 Props；`exact`、显式 `alias`、
`missing` 只表示 Blazor 目标侧状态。`missing` 不能删除 React 测试场景，后续
应通过新增组件适配器或明确的类型映射逐项消除。涉及语义改名的 alias 必须按组件
限定，例如 `collapse.isActive -> Active`、`progress.railColor -> TrailColor`，
不能把相似名称作为全局规则。

```bash
node tools/AntDesign.StyleGen/src/cli.mjs sync-v6 \
  --antd-path /path/to/ant-design \
  --blazor-path /path/to/ant-design-blazor \
  --react-css /path/to/antd/dist/antd.css
```

默认使用 React 仓库的 `master`，也可以指定 revision：

```bash
node tools/AntDesign.StyleGen/src/cli.mjs sync-v6 \
  --antd-path /path/to/ant-design \
  --revision master
```

脚本按以下顺序执行：

1. 从 React 项目生成组件/主题 Style IR。
2. 生成 `GeneratedComponentTokens.cs`。
3. 生成 `GeneratedThemeTokens.cs`：包含 React Seed 默认值、全局 Token 元数据，以及各组件实际使用的全局 Token 关联。组件文档通过此关联生成中英文全局 Token 表格。
   同时执行该 checkout 的真实 `getDesignToken`，生成 C# 回归测试使用的 `tests/AntDesign.Tests/Theming/Fixtures/react-theme-tokens.json`。基准包含 default/dark/compact 的默认、自定义 Seed、无动画/焦点、Map/Alias 覆盖共 12 个场景，每个场景比较全部 536 个返回值。也可单独运行 `generate-theme-reference --antd-path <path> --output <json>`。C# 测试保留全部字段，只规范等价 CSS 写法和 1e-12 以内的浮点序列化误差；这不等于任意输入或组合算法已完全对齐。
4. 生成 `ThemeTokenProperties.cs` 快捷属性。
5. 保存 React 编译 CSS 基准。
6. 从 Ant Design checkout 的 `.dumi/theme/builtins/IconSearch/fields.ts` 同步图标分类，保存在 Blazor 文档项目的生成目录，并生成文档侧 `IconSearchCategories.g.cs`；图标 NuGet 包仅提供 SVG 名称、主题和组件类型清单。
7. 保存 `@ant-design/colors` 迁移测试快照。
8. 写入 `.vibedev/generated/antd-v6/sync-manifest.json`。

之后可以使用 `exact-check` 对 React 基准和 Blazor 产物执行字节级校验。

## React snapshot 到 bUnit 测试生成

`schema/dom-case.schema.json` 定义 React 测试与 Blazor 测试之间的中间协议。
每个 case 记录 React snapshot 文件/测试 key，以及 Blazor 组件类型和参数。
生成器会先校验 JSON，再输出可编译的 bUnit 测试源码：

```bash
node tools/AntDesign.StyleGen/src/generate-bunit-cases.mjs \
  tools/AntDesign.StyleGen/examples/dom-cases.json \
  > /tmp/GeneratedReactDomCases.cs
```

生成的测试直接使用 React snapshot 作为期望 HTML，因此标签、class、属性、文本
或子节点顺序出现差异时会失败，不会静默归一化差异。

## React 测试源码到 Blazor 场景 JSON

不要从 snapshot 反推组件参数。使用 TypeScript AST 直接提取 React 测试源码：

```bash
node tools/AntDesign.StyleGen/src/extract-react-tests-to-json.mjs \
  /Volumes/Data/GitHub/ant-design \
  docs/antd-v6/react-test-cases.json

node tools/AntDesign.StyleGen/src/translate-react-test-json-to-blazor.mjs \
  docs/antd-v6/react-test-cases.json \
  docs/antd-v6/blazor-test-cases.json

node tools/AntDesign.StyleGen/src/generate-bunit-from-blazor-json.mjs \
  docs/antd-v6/blazor-test-cases.json \
  tests/AntDesign.Tests/GeneratedReactSource

node tools/AntDesign.StyleGen/src/verify-generated-bunit-tests.mjs \
  tests/AntDesign.Tests/GeneratedReactSource

dotnet build tests/AntDesign.Tests/AntDesign.Tests.csproj \
  -f net10.0 --no-restore
```

`tests/AntDesign.Tests/GeneratedReactSource` 是当前唯一权威的 React 测试迁移输出目录。
旧的 `Generated/ReactDomRedBaseline_*.g.cs` 基线输出已停用，不应重新生成；否则会把旧基线
测试重复编译进测试项目并污染迁移统计。

第一步保存每个测试的源码、渲染调用、交互调用、断言调用和 snapshot 调用；
同时按测试文件自动关联 `__snapshots__/*.snap` 中的 snapshot key 和期望 HTML。
第二步只把能确定转换的简单 JSX 生成为 Blazor candidate，其余场景标记为
`requires-component-adapter`，禁止生成错误的默认组件测试。
第三步按组件拆分生成 bUnit 测试，并把原 React 测试源码保留在每个测试方法上方。
提取器会保留 `expect(...).matcher(...)` 的完整 matcher 链；当前通用转换器支持
选择器、click/change/focus/blur/type/keyboard 以及存在性、反存在性、class、attribute、
文本和数量断言。无法确定目标元素或值时仍保留为 adapter 场景，避免生成看似通过但语义错误的测试。
连续的 `render/rerender` 在所有步骤均可解析时会生成 `SetParametersAndRender` 参数迁移；
任一步骤无法解析则回退到 adapter 场景，不会只验证初始渲染结果。
`waitFakeTimer`、`waitRaf` 和可确定的等待调用会生成 bUnit 等待步骤；输入值及正/反向
DOM matcher 也会转换为 `Change`、`Assert.Equal`、`Assert.NotEqual` 等真实断言。
回调属性会按 `callback-prop-requires-typed-event-adapter` 单独分类；由于不同组件的
`EventCallback<T>` 参数类型不同，不会生成未经类型确认的通用委托。
当前已为 `Button.OnClick`、`Switch.OnClick` 和 `BackTop.OnClick` 建立第一批类型化 adapter；
并继续补充了 `Pagination`、`Input`、`TextArea`、`Checkbox`、`Radio`、`Tabs`、`Calendar`、
`DatePicker` 和 `AutoComplete` 的已知类型。`toHaveBeenCalled` 类 matcher 会生成调用计数器
断言；其余回调需要继续补充组件参数元数据。
当 candidate 恰好关联一个 React snapshot 时，生成 `MarkupMatches` 进行 DOM 对齐校验；
没有 snapshot 或关联不唯一时只生成基础渲染断言，并保留待适配标记，避免猜测期望结构。
如果 JSX 使用的组件在 Blazor 中尚不存在，仍会生成对应测试文件和测试方法，状态为
`skipped-component-not-found`；生成的 C# 测试不会使用 Skip，而是以明确失败的 TDD 红测试记录缺少的组件映射。
无法自动翻译交互或复杂 JSX 的 `requires-component-adapter` 场景同样生成可执行的红测试，
禁止用 `Assert.True` 或 Skip 掩盖缺失实现，因此 React JSON 中每个场景都有对应的测试入口。
生成器每次运行都会根据 `.generated-manifest.json` 和自动生成文件头清理上一次生成的
`.g.cs`，再完整重建测试文件；手写文件不会被删除。manifest 会记录每个文件的 SHA-256，
校验脚本会检查文件存在、自动生成标记、哈希和未登记的 `.g.cs` 文件，确保测试确实来自脚本。

一个 case 还可以包含多个动态 `steps`。每一步可以声明 `changes`、交互
`actions` 和 `assertions`，例如修改 `Disabled` 后断言 class 与组件属性：

```json
{
  "id": "disable-button",
  "changes": [{ "parameter": "Disabled", "value": true }],
  "assertions": [
    { "type": "class", "selector": ".ant-btn", "expected": "ant-btn-disabled" },
    { "type": "property", "property": "Disabled", "expected": true }
  ]
}
```

支持的断言类型包括 `markup`、`exists`、`not-exists`、`class`、`attribute`、
`text`、`count` 和 `property`；生成器会把它们转换成对应的 bUnit/xUnit 断言。

## 从 React 生成 Icon 搜索关键词

Icon 文档搜索复用 React `.dumi/theme/builtins/IconSearch/meta` 中的关键词和别名，不要在 Blazor 侧手工复制维护。React 仓库需已安装依赖（脚本使用其中的 `tsx` 加载 TypeScript 元数据）：

```sh
node tools/AntDesign.StyleGen/src/extract-icon-search-meta.cjs \
  /path/to/ant-design \
  site/AntDesign.Docs/Services/IconSearchMetadata.g.cs

node tools/AntDesign.StyleGen/src/extract-icon-search-meta.cjs \
  /path/to/ant-design \
  site/AntDesign.Docs/Services/IconSearchMetadata.g.cs \
  --check
```

生成文件包含 `meta/index.ts` 的图标别名及标签；`IconSearchMetadataTests` 验证代表性关键词、主题后缀和大小写行为。React 更新后重新生成并运行这些测试。

## 首页响应式宽度门禁

浏览器验收还必须采集 `document.documentElement.clientWidth/scrollWidth`、导航可见性、
Header 宽度、三个背景分区边界和两个 Banner 按钮边界。仅验证卡片尺寸或媒体查询命中，
不能证明页面没有横向溢出；`innerWidth` 也不能代替扣除滚动条后的 `clientWidth`。
临界尺寸需等待导航和轮播完成断点切换后再采集，记录实际 `width` 以排除尺寸应用错标签页。

```sh
node tools/AntDesign.StyleGen/src/homepage-layout-check.mjs /path/to/browser-observations.json
node --test tools/AntDesign.StyleGen/test/homepage-layout-check.test.mjs
```

输入为浏览器采集的 observation 数组（字段见测试样例）。允许主题图标条在内部滚动，
但不允许整个文档撑宽、导航消失、拥挤态菜单换行或按钮被裁切。
该门禁的 scope 为 `homepage-layout-only`，不代表整页字符级 DOM/CSS 对齐通过。

## Icon 字体资源的 DOM/CSS 校验

共享颜色算法的参考数据由实际 React 依赖生成：

```sh
node tools/AntDesign.StyleGen/src/color-palette-reference.mjs /path/to/ant-design tests/AntDesign.Tests/Theming/Fixtures/react-color-palettes.json
```

## 色彩规范页

从 React 色彩文档组件提取基础色板顺序、中英文名称/说明、主色、中性色定义和默认展示数量：

```sh
node tools/AntDesign.StyleGen/src/cli.mjs sync-color-spec --antd-path /path/to/ant-design --output site/AntDesign.Docs/Services/ColorSpecCatalog.g.cs
# CI 校验生成目录未过期
node tools/AntDesign.StyleGen/src/cli.mjs sync-color-spec --antd-path /path/to/ant-design --output site/AntDesign.Docs/Services/ColorSpecCatalog.g.cs --check
```

`/{language}/docs/colors` 使用共享 `AntDesign.Colors` 算法生成 12×10 色板，并使用真实
Blazor ColorPicker 交互。默认中性色预览与 React 一样为 10 档，完整 13 档定义保留于目录。
另提供暗色色板工具和实际 C# 明暗主题的中性色 Token 对比。窄屏色条局部滚动，不撑宽文档。

## 编译期 LLM Markdown

`AntDesign.Docs.Generator` 在编译期从 components JSON、Semantic DOM JSON、Token JSON
和 docs Markdown 生成完整文档，打包为压缩常量以避开 CLR 字符串堆限制。服务端
`/{language}/components/{component}.md` 与 `/{language}/docs/{document}.md` 只查找、
解压并缓存已生成的文本；不在请求时转换 HTML、计算主题、读取文件、反射或请求 JSON。
组件文档包含 Blazor API、Semantic DOM、Token 和非 debug Razor 示例。原始静态说明地址保留。

Token 定义、使用关系或算法默认值变更后，先刷新生成器的输入：

```sh
dotnet run --project site/AntDesign.Docs.Build.CLI -- llms-tokens docs/antd-v6/llms-tokens.json
```

然后重新编译 Docs。静态/WASM 站点使用相同生成结果导出，中英文各 75 个当前已注册组件，
以及通用文档（含色彩页）：

```sh
dotnet run --project site/AntDesign.Docs.Build.CLI -- llms /path/to/static-site-root
```

先生成并重新编译 docs 元数据，再执行此命令；`AntDesign.Docs.Build` 的 RunCli 已按
`demo2json → menu2json → docs2html → llms-tokens → llms` 顺序接入。生成的静态 Markdown
不纳入源码；生成器输入 JSON 纳入源码。服务端响应和 CLI 导出的同一文件应逐字节一致。

## Icon 字体资源门禁细节

`examples/icon-dom-style-parity.json` 覆盖 React 的五个公共示例。字体图标的
`verifySvgUse: true` 节点会先等待对应 symbol 和非零绘制尺寸，再采集实际
`use` 边界与所有同名 symbol 的有序定义；只有空的 SVG 壳不能通过校验。
`svgUsePaintTolerancePx: 0.01` 仅容纳滚动坐标导致的亚像素边界精度差，
不放宽 CSS 值或 symbol 定义比较。

`removeUnreferencedRuntimeIds` 只移除符合显式运行时 ID 模式且未被捕获子树引用的
ID；静态 symbol ID、ARIA/HTML 引用和 SVG `href`/`url(#id)` 关系仍保留。
报告分别列出原始 HTML、规范化 DOM 和声明的 CSS/glyph 门禁，不能把后两项通过
描述为原始 HTML 字节相同，或整个组件的所有状态均已对齐。

## Select 基础状态与弹层门禁

`examples/select-basic-popup-dom-style-parity.json` 补充四个基础状态和打开弹层，
分别检查真实触发器、portaled popup、list、option/content/state。弹层使用可见
Basic 选项的 selector，不能混入页面底部常开的 Semantic DOM 示例或隐藏的其他下拉。
门禁保留默认零几何容差和完整结构比较：声明 CSS 一致不代表虚拟列表的 DOM、
ARIA 辅助树或定位已经对齐。当前报告明确保留这些差异，不忽略 role/ARIA/style 来过关。
