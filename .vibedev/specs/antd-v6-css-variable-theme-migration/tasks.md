# Ant Design v6 CSS 变量主题迁移实施任务

## 当前阶段：第一阶段——现有组件接入 v6 CSS

第一阶段优先完成静态 CSS、默认主题及现有 13 个主题、现有组件 DOM/class/Semantic DOM 结构和文档预览。第二阶段的 Design Token/算法、React v4→v6 属性迁移、动态主题编译，以及第三阶段的 React 测试完整迁移，暂不作为当前执行目标。

### 阶段归属

- 第一阶段：任务 2、4、4.2、4.3.1、4.3.2、7、7.1、9、9.0、9.1、9.2-9.4、10、10.1-10.3，以及 CSS/DOM/class/文档站相关的必要回归门禁。
- 第二阶段：任务 3、3.1、3.2、3.3、3.4、3.5、5、5.1、6、6.1、6.2、7.2、8、8.1，以及 Design Token、算法、React v4→v6 属性迁移、动态主题和 SSR 编译能力。
- 第三阶段：任务 12 及其子任务，负责 React 测试源码、交互步骤和断言到 bUnit 的完整迁移。

第一阶段不得为了生成测试或提前处理 v4→v6 API 差异而反向修改 React API 或组件 DOM；第一阶段只保留必要的 CSS/DOM 回归校验。

1. [x] 建立上游生成器适配器和统一 Token/Style IR
   - 创建 `tools/AntDesign.StyleGen` 的 CLI 入口、参数解析、revision/lockfile 元数据和 schema 校验。
   - 实现对 `ant-design` 原生成器的 Node/TypeScript 调用适配，输出 `manifest.json`、`tokens.json`、`components.json`、`themes.json` 和 `unsupported.json`。
   - 覆盖需求 1.1-1.5、4.1-4.5。

1.1. [x] 为 IR 建立 fixture 和 schema 单元测试
   - 覆盖 Seed/Alias/Component/Semantic Token、组件样式、at-rule、来源映射和非法输入。
   - 覆盖需求 2.1、2.5、4.3。

2. [x] 实现 React CSS 基准快照生成与显式更新流程
   - 实现 `baseline update`，按组件、主题、变体、状态、RTL、弹层和 SSR 组件集合保存 React 原生成器的原始 CSS 字符串。
   - 固定 UTF-8、LF、末尾换行、Node/依赖 digest 和 manifest digest；禁止普通测试覆盖基准。
   - 覆盖需求 4.7-4.8、9.4、9.6。

2.1. [x] 为 React 基准建立完整性和确定性测试
   - 验证同一 revision 重复生成结果完全一致，revision/lockfile/manifest 变化能被检测。
   - 覆盖需求 1.2、1.5、4.7。

3. [x] 实现 .NET Token 模型和 CSS Variable 编译器
   - 在 `components` 中创建类型化 Seed/Alias/Component/Semantic Token、Resolved Token、CssVarToken 和主题算法接口。
   - 按 IR 生成稳定变量名、变量值、组件变量和共享组件 CSS，并生成受管理的 C# 文件、CSS 文件和资源 manifest。
   - 覆盖需求 2.1-2.5、8.2-8.4。

3.1. [x] 实现 Token 映射和序列化单元测试
   - 使用 `colorPrimary`、`borderRadius`、`controlHeight`、Button Component Token 等 fixture，验证 React IR 到 C# 与 CSS 的值、顺序、格式和来源映射。
   - 覆盖需求 2.2、2.4、4.7。

3.2. [x] 拆分静态组件 CSS 与 Design Token 变量值 CSS 的编译管线
   - 静态层只生成稳定 class name、selector、状态/variant、at-rule 和 `var(--ant-...)` 引用；变量值层只生成按主题和 scope 分组的 `--ant-*` 实际值。
   - 为两类产物分别生成 manifest、digest、缓存键和 managed files，并在最终打包时按 `static -> token-values -> runtime-overrides` 顺序拼接。
   - 覆盖需求 2.2-2.4、4.7-4.8、7.2-7.4。

3.3. [x] 建立可重复的 React master → Blazor v6 Token 同步脚本
   - 新增 `sync-v6`，一次调用依次生成 Style IR、组件 Token 类型、Seed/Map/Alias 类型、ThemeToken 快捷属性、React CSS 基准和颜色算法测试快照。
   - 固定 upstream revision、生成元数据和占位路径，重复运行不得因时间戳或本地绝对路径产生漂移。
   - 将生成产物、同步清单和 React 基准纳入 `.vibedev/generated/antd-v6`，并提供使用说明。
   - 覆盖需求 1.1-1.5、2.1-2.5、4.7-4.8、11.1。

3.4. [x] 将强类型 Token 快捷属性纳入同步生成
   - 自动生成 Seed、Map、Alias 及共享算法 Token 的 `ThemeToken` 快捷属性，同时保留动态索引器。
   - `ThemeConfig` 的 Seed/Map/Alias/ComponentTokens 通过同一编译链合并，并由 ConfigProvider 输出 CSS Variables。
   - 覆盖需求 2.1-2.5、3.1-3.3、8.2。

3.5. [x] 完成同步产物的结构覆盖门禁
   - 对比 React 上游 Seed/Map/Alias 字段、组件 Token 字段、Style Generator 成员与 C# 生成文件，禁止字段静默减少或退回占位类型。
   - 对 `extends`、`Pick`、联合类型、TokenMap 和嵌套 Map 建立源生成器回归 fixture。
   - 覆盖需求 1.3-1.5、2.1、2.5、11.1-11.4。

4. [x] 实现 React/Blazor CSS 字节级精确校验器
   - 创建 `StyleDiff` 核心库和 CLI，按 static layer、token-value layer 和完整拼接 CSS 三类 manifest 执行 UTF-8 字节完全相等比较，再生成首个不同字节位置和结构化诊断。
   - 支持组件级、主题级、场景级和完整拼接 CSS 比较；运行时变量覆盖单独比较；未批准差异返回非零退出码。
   - 覆盖需求 4.1-4.8、9.1、9.4。

4.1. [x] 编写 exact CSS 单元测试
   - 分别覆盖 static CSS、token-value CSS、runtime override CSS 和完整拼接 CSS 的完全相等、单字节差异、换行差异、声明顺序差异、变量值差异、缺失/新增文件、allowlist 和诊断上下文。
   - 覆盖需求 4.2-4.6、9.1。

4.2. [x] 执行基础设施 MVP 检验并设置批量迁移门禁
   - 使用 Button、Alert、Input 三个代表性组件，覆盖默认/暗色主题和一个组件 Token 覆盖，端到端运行 React baseline、IR、静态 CSS、Token 变量值 CSS、完整 CSS 和 exact tests。
   - 增加故意单字节修改必须失败的回归 fixture；MVP 未通过时阻止后续全量组件、13 主题和文档任务进入执行。
   - 覆盖需求 2.6-2.8、4.7-4.8、11.1-11.4。

4.3. [ ] 将 `sync-v6` 产物接入完整 Token/Style 校验
   - 使用同步脚本生成的 React baseline、Token 测试快照和 Blazor candidate，逐主题、逐组件比较静态 CSS、Token 值 CSS、runtime override 和完整 CSS。
   - 同步脚本成功不等于 exact gate 通过；任何未批准差异必须生成首个字节诊断并返回非零退出码。
   - 覆盖需求 4.1-4.8、9.1-9.4、11.1-11.4。

4.3.1. [x] 增加 13 个首页主题全量 CSS 逐字节比较命令与回归测试
   - `theme-exact-check` 按主题 CSS 文件逐一比较 React 与 Blazor 产物，检查缺失/额外主题，并报告首个差异字节。
   - 测试覆盖 13 个主题的成功对比和单字节变更失败场景。

4.3.2. [x] 接通 C# 主题 CSS 产物生成链
   - `AntDesign.StyleGen.DotNet` 通过 `GeneratedPreviewThemes`、`PreviewThemeConfigFactory` 和 `StaticStyleExporter` 生成每个主题的完整 CSS。
   - 支持 `--react-theme-css-dir` 对 C# 生成结果执行逐主题逐字节校验；不得以外部复制的 Blazor CSS 代替生成结果。

5. [x] 实现 Blazor 版 antd-style 样式上下文和样式工厂
   - 创建 `AntDesign.Theming` 下的 `ThemeConfig`、`IThemeContext`、`IThemeResolver`、`IStyleFactory`、`StyleDefinition`、`StyleResult` 和 `cx` 合并逻辑。
   - 支持对象式 CSS、嵌套选择器、媒体查询、CSS Variable 引用、内容 digest class 和确定性缓存。
   - 覆盖需求 3.1-3.6、2.2-2.4。

5.1. [x] 测试 antd-style Token/CssVar 双视图
   - 验证样式回调同时获得解析后的 `Token` 和 `var(--ant-...)` 形式的 `CssVar`，并验证相同输入不会生成不同 class/CSS。
   - 覆盖需求 3.2-3.6。

6. [ ] 升级 ConfigProvider 主题 API 和嵌套作用域
   - 修改 `components/config-provider/ConfigProvider.razor(.cs)`、主题上下文和服务，使 `ThemeConfig`、`Appearance`、`ThemeScope`、CSS Variables 开关和嵌套继承可用。
   - 将 `GlobalTheme`/Less 文件切换迁移为兼容入口，核心切换改为 scope class/data 属性和变量快照更新。
   - 覆盖需求 3.1-3.6、8.1-8.5。

6.1. [ ] 测试 ConfigProvider 主题解析和运行时切换
   - 覆盖默认/暗色算法、内外层 Token 合并、作用域隔离、预构建主题 class 切换、动态变量批量更新和组件 API 编译兼容。
   - 覆盖需求 3.2-3.6、8.1-8.4、10.5-10.6。

6.2. [ ] 将同步生成的强类型 Token 作为 ConfigProvider 唯一主题输入
   - 验证 Seed/Map/Alias/Component Token 的继承、覆盖顺序与 React ConfigProvider 一致。
   - 验证快捷属性、动态索引器、组件 Token 对象和 CSS Variable 输出使用同一快照。
   - 覆盖需求 2.1-2.5、3.1-3.6、8.1-8.5。

7. [x] 接入核心组件生成 CSS 并完成 React/Blazor exact gate
   - 按组件迁移现有 Less 样式到生成 CSS/变量引用，保留公共参数、事件、泛型约束和组件行为。
   - 首批覆盖 Button、Alert、Modal、Card、Input、Select、Menu、Layout、Progress、Steps、Slider、ColorPicker、Notification、Switch、Radio 等主题演示使用组件。
   - 覆盖需求 6.1-6.5、4.7-4.8。

7.1. [ ] 建立代表性组件 exact CSS 测试矩阵
   - 对每个迁移组件执行 React 基准与 Blazor candidate 的组件、主题、变体、状态、RTL、紧凑尺寸、弹层和完整 CSS 比较。
   - 覆盖需求 4.1-4.8、9.2、9.4。

7.2. [ ] 第二阶段：同步 React v6 Props 命名并完成 v4→v6 属性迁移
   - 基于 React v6 API 提取结果建立组件属性命名对照表，覆盖新增、重命名、拆分、合并、删除和语义变化的属性。
   - 将 Blazor 公共参数调整为 v6 语义和 PascalCase 命名；对继续保留的旧 v4 参数增加 `[Obsolete]`，指向替代参数并转发到同一内部状态/渲染路径。
   - 生成 API 命名差异报告和编译兼容回归测试；禁止把类型或语义不兼容的属性仅按名称相似度映射。
   - 在第二阶段文档中更新新属性用法、旧属性迁移说明和弃用时间线，并与 Design Token/API 迁移一起验证。
   - 覆盖需求 1.1、2.1-2.5、8.1、8.3、11.9。

8. [ ] 实现 SSR 静态样式导出
   - 创建 `IStaticStyleExporter`、`StaticStyleRequest`、`StaticStyleResult` 和 manifest/cache digest 实现。
   - 支持 `<style>`/外部 CSS、CSP nonce、默认/暗色/13 个首页主题、自定义 Token、组件集合和动态样式 fallback。
   - 覆盖需求 7.1-7.6。

8.1. [ ] 测试 SSR 与客户端产物一致性
   - 验证同一 Token/组件输入由 SSR 和客户端使用同一编译器生成完全一致的 CSS，并验证缓存命中与 digest 失效。
   - 覆盖需求 7.2-7.6、9.3。

9. [ ] 迁移 13 个首页主题配置
   - 创建强类型 `PreviewThemeDefinition` 注册表，导入 light、mui、shadcn、bootstrap、cartoon、dark、illustration、glass、geek、lark、blossom、v4、serene 的全局 Token、组件 Token、算法、class/style 和资源映射。
   - 增加生成器注册表遗漏检查和每主题 React/Blazor CSS 基准快照。
   - 覆盖需求 5.3、10.1-10.6、7.3。

9.0. [x] 同步 13 个首页主题的可静态解析 Token 配置
   - `sync-v6` 读取各主题源文件中的全局 `token`、组件 `components` 和算法来源，生成 `GeneratedPreviewThemes.cs`。
   - 同步解析首页 `index.ts` 中 light/dark 共用的组件 Token 及暗色覆盖项，确保默认主题不再生成空组件配置。
   - 对 `spread`/运行时引用无法静态求值的部分保留 `Unresolved` 来源清单，禁止静默丢失。
   - 覆盖需求 1.3-1.5、5.3、10.1、11.1-11.4。

9.1. [ ] 实现首页主题切换组件和自动化覆盖
   - 在 `site/AntDesign.Docs` 中实现主题选择、当前项无障碍状态、键盘左右导航、持久化、无整页刷新切换和代表性组件预览。
   - 覆盖需求 10.1-10.6、9.4。

9.2. [ ] 迁移首页 Dumi ThemePreview
   - 对照 `../ant-design/.dumi/pages/index/components/ThemePreview/` 和其引用的 `PreviewPane/Components.tsx`、`Theme/`、`Group.tsx`；先使用 v6 CSS 与 Blazor 组件搭建功能结构，再迁移 Blazor 站缺失的专属布局样式。
   - [x] 首页已加入组件/仪表盘视图、13 个主题色板、复制主题代码和主题预览入口；用真实 Blazor Button/Alert 示例，主题样式限制在预览容器。
   - [x] 浏览器验证视图切换、MUI 色板变量、复制反馈。
   - [ ] 将预览示例及主题配置与 React `PreviewPane/Components.tsx`、`Theme/` 的完整内容逐项对齐，并完成窄屏视觉回归；AI 动态生成留到第二阶段。
   - 不在此任务实现任意 Token 的动态编译。

9.3. [x] 迁移文档全局主题切换
   - 对照 `../ant-design/.dumi/theme/common/ThemeSwitch/` 和 `theme/layouts/GlobalLayout.tsx`。
   - [x] 扩展 React CSS 基准生成器，生成 `light-compact` 和 `dark-compact` 静态组合样式及来源/digest 清单，并在生成测试中校验输出字节一致。
   - [x] 支持 light/dark 互斥切换、compact 独立叠加、持久化及刷新恢复；使用预构建 CSS。
   - [x] 浏览器验证 dark + compact 组合加载 `dark-compact.css`。
   - [ ] 补齐自动化覆盖与键盘/窄屏回归。

9.4. [x] 迁移 Happy Work Effect
   - 对照 `../ant-design/.dumi/theme/layouts/GlobalLayout.tsx` 中 `HappyProvider` 的开关语义及其上游效果包实际资源；将效果限制在文档站组件交互，不改写组件业务状态。
   - [x] 支持用户显式启停、状态保存、按需导入 JS 模块，并尊重 `prefers-reduced-motion: reduce`。
   - [x] 浏览器验证开关启用后真实交互生成粒子及点击目标动画。
   - 若无法复用 React 专属运行时，使用 Blazor/CSS 实现行为等价效果并记录差异及来源。

10. [x] 实现 Semantic DOM 预览组件
   - 在 `site/AntDesign.Docs` 创建 Blazor `SemanticPreview`、语义路径映射、悬停/固定高亮、版本标签、代码示例和子组件 class/style 注入。
   - 使用实际 Token/CSS Variable 主题上下文，不创建独立伪样式。
   - [x] 浏览器验证列表渲染、键盘/悬停/点击激活与真实 DOM 高亮。
   - 覆盖需求 9.1-9.5。

10.1. [ ] 编写 Semantic DOM 文档自动化测试
   - 覆盖语义列表渲染、悬停/键盘焦点/固定状态、代码示例、亮暗主题、响应式布局和控制台错误。
   - 覆盖需求 9.2-9.5、11.4。

10.2. [ ] 迁移组件文档 GitHub 元信息入口
   - 对照 `../ant-design/.dumi/theme/builtins/ComponentMeta/`、`theme/slots/Content/DocMeta.tsx` 与 `theme/common/EditButton.tsx`。
   - [x] 在组件文档标题旁增加 Ant Design React 上游组件源码 GitHub 链接，按组件名生成 kebab-case 路径，并补充中英文 Tooltip/aria-label。
   - [x] 浏览器验证 Button 文档指向 React 上游 `components/button`。
   - [ ] 完成剩余元信息入口；页面、Demo 的编辑链接继续指向 Blazor 仓库中各自准确的文件路径，不得混用 React 与 Blazor 仓库地址，并验证链接目标。
   - 对不存在上游映射的 Blazor 专属组件隐藏或给出明确链接，不生成错误的 `components/{name}` 地址。

10.3. [ ] 建立 Dumi 文档布局样式迁移清单并逐项迁移
   - 先列出 `.dumi/theme/slots`、`theme/layouts`、`theme/common`、`pages/index/components` 所需的结构/样式来源，再分类为“已有 v6 组件 CSS”“必须迁移的站点布局 CSS”“Blazor 不适用”并记录理由。
   - 仅迁移后两类中确有必要的站点样式；保留响应式、亮暗主题和 reduced-motion 行为，避免复制整份 Dumi 全局 CSS或覆盖 v6 组件规则。
   - 为每个迁移模块建立至少一项结构或视觉回归检查。

11. [ ] 接入 CI 生成、exact gate、构建和测试门禁
   - 添加 Node/.NET 生成与测试命令，先验证 React 基准，再执行 Blazor exact CSS、确定性生成、组件/API、SSR、文档和视觉快照测试。
   - 对 managed generated files、未支持字段和无理由 diff 设置失败门禁。
   - 覆盖需求 1.2、1.4、4.6-4.8、9.1-9.7。

11.1. [ ] 添加性能阈值测试和生成提交校验
   - 将 CSS 体积、生成耗时、SSR 导出耗时和运行时变量切换耗时作为可配置测试阈值，并验证生成元数据随代码提交更新。
   - 覆盖需求 11.5-11.7。

11.2. [ ] 增加文档站迁移回归门禁
   - 对首页 ThemePreview、主题组合切换、Happy Work Effect/reduced-motion、Semantic DOM preview 和组件 GitHub 链接执行自动化构建/交互测试。
   - 确认 light/dark 与 compact 组合只加载第一阶段允许的预置 CSS/状态，不触发 Design Token 动态编译。

12. [ ] 第三阶段：从 React 测试源码完整迁移到 bUnit
   - 在第一阶段 DOM/class/HTML 结构和第二阶段 Token/主题编译稳定后，提取全部 React 测试源码为中间 JSON。
   - 保留每个 React 测试的渲染步骤、参数变化、交互动作、异步等待、断言类型、断言值和原始源码，并按组件拆分生成 Blazor 测试文件。
   - 复杂交互必须生成真实 bUnit 测试；不存在的组件或无法映射的 API 必须有明确的失败状态和迁移报告，不得使用占位字符串、`Assert.True` 或静默 Skip。
   - 支持清理并重新生成全部测试文件，验证生成文件均来自脚本且与 React 测试一一对应。
   - 覆盖需求 11.2、11.4、11.8；不作为第一阶段合并门禁。
