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

# 从 React v6 Semantic DOM inventory 可重复生成 Blazor 预览字典。
# 已确认的 selector 会从现有生成文件保留；新路径使用 fallback selector，
# 后续通过 bUnit 结构测试确认后即可成为正式映射。
node tools/AntDesign.StyleGen/src/generate-semantic-dom-catalog.mjs \
  /Volumes/Data/GitHub/ant-design-blazor
```

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
3. 生成 `GeneratedThemeTokens.cs`。
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
