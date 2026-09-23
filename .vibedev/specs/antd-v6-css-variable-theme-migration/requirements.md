# Ant Design v6 CSS 变量主题迁移需求

## 简介

本功能将 `ant-design-blazor` 的样式和 Design Token 体系迁移到最新 Ant Design v6 设计，并以本地 `ant-design` 项目作为权威来源。迁移必须通过可复用的 CLI 或脚本完成，使上游样式和 Token 更新后可以重复同步，而不需要手工重写组件样式。同时，系统必须将上游编译出的 CSS 作为基准，与 Blazor 生成的 CSS 进行差异校验。

现有组件 API 必须保持兼容；旧主题 API 和旧样式覆盖方式不需要兼容。实施顺序是先完成核心样式迁移，再完成文档演示和主题切换。最终交付包括代码修改、自动化测试、视觉对齐、性能检查、CI 门禁和 Git 提交。

## 分阶段范围与当前执行阶段

当前回到第一阶段执行，第一阶段完成后作为一个可独立合并的 Pull Request：

### 第一阶段：现有组件接入 v6 CSS

- 以 React Ant Design v6 编译出的 CSS 为唯一样式基准，通过脚本全量生成默认 CSS 和现有 13 个预置主题 CSS。
- 让现有 Blazor 组件使用最新 v6 CSS class、Semantic DOM 列表和 HTML 结构；包括文档站的 `ThemePreview` 与 `SemanticPreview` 展示。
- 迁移 v6 文档站功能。先用已同步的 v6 组件 CSS 和 Blazor 组件搭建页面结构，再逐项移植 `.dumi` 中 Blazor 站没有的专属布局样式；禁止用旧版文档样式覆盖 v6 组件样式。
- 首页迁移 Dumi `ThemePreview`，展示主题预览组件、主题配置入口和对应的交互/代码操作；文档组件页迁移 `Semantic DOM preview`、组件 GitHub 源码/元信息链接及必要的编辑入口。
- 文档全局主题切换支持 light/dark 互斥切换，并允许 compact 与 light/dark 叠加；组合 CSS 必须由 React v6 编译器预构建并纳入来源/digest 清单，提供偏好保存、刷新后恢复和键盘/无障碍状态。支持 Happy Work Effect 的开关、组件交互效果和 reduced-motion 降级。
- 完成现有组件的 Semantic DOM 结构调整和组件级高亮预览；第一阶段不实现任意 Design Token 编辑、运行时 Token 编译或新的 Design Token API。light/dark/compact 仅切换预置 CSS/作用域状态，不能借机引入动态 Token 编译。
- 第一阶段的校验重点是组件 DOM/class/主题 CSS 与 React 产物一致；React 测试源码的完整转换不属于本阶段交付。

### 第二阶段：Design Token 与动态主题

- 手动迁移 Design Token、算法和 Token→CSS 编译关系，并以 React Token/API 作为源头进行类型映射。
- 同步 React v6 组件 Props 命名，并逐项处理 v4→v6 的属性新增、重命名、拆分、合并、删除和语义变化；该工作与 Design Token/API 迁移一起进行。
- 对仍保留但已被 React v6 替代的旧 Blazor 属性增加 `[Obsolete]` 标记、替代属性说明和迁移诊断，保持兼容但不再作为新入口。
- 实现主题对象、嵌套作用域、动态 CSS Variable 覆盖、运行时编译和 ConfigProvider 主题修改。
- 完成 Design Token 与生成 CSS 的逐项对应校验。

### 第三阶段：React 测试迁移

- 从 React 测试源码提取完整 API、渲染步骤、交互步骤、属性变化和断言，转换为 bUnit 测试。
- 覆盖复杂交互、异步流程和 DOM 结构断言；禁止用占位字符串、`Assert.True` 或跳过测试替代真实迁移。
- 测试迁移必须基于第一阶段稳定的 DOM/class/HTML 结构和第二阶段稳定的 Token/主题编译结果进行。

除非明确标注为第一阶段的回归校验，第三阶段测试迁移脚本和生成测试不作为当前阶段的完成条件。

## 探索结论与来源约束

- 当前参考仓库版本为 Ant Design `6.6.4`，revision 为 `599b251da9`。生成器必须支持显式指定源码路径和 revision，不能永久写死当前 revision。
- 首页主题源码位于 `.dumi/pages/index/components/ThemePreview/previewThemes`，注册表位于 `index.ts`。必须覆盖其中全部 13 个主题：`light`/Ant Design、`mui`、`shadcn`、`bootstrap`、`cartoon`、`dark`、`illustration`、`glass`、`geek`、`lark`/Document、`blossom`、`v4`/Ant Design V4、`serene`。
- 主题配置可能包含全局 Token、组件 Token、算法、组件级 class/style、图标、背景图和组件行为。迁移到 Blazor 后必须保留可观察的视觉意图，并对无法直接迁移的字段给出诊断。
- 上游 `SemanticPreview.tsx` 通过语义路径列表、悬停/固定高亮、版本标签、`classNames`/`styles` 示例和子组件语义 class 注入来展示 Semantic DOM；Blazor 文档等价实现必须保留这些能力。
- CSS 变量方案要求稳定的 Token → 变量映射、可选的作用域隔离、预构建主题选择器以及动态主题覆盖策略；实现必须明确作用域和切换机制，不能依赖未文档化的全局 `:root` 修改。
- v6 CSS 方案包含基于变量的变体和样式覆盖、使用 `@container` 的条件兼容，以及未来使用 `@scope` 的可能性；不支持的浏览器能力必须有明确降级策略。
- `antd-style` 是主题对象和样式消费模型的重要参考：它提供基于 Ant Design Token 的 `createStyles`、`css`、`cssVar`、主题上下文和 `theme.appearance` 等能力；Blazor 版本需要提供对应的 .NET API，而不是只把主题值转换成静态字典。
- 上游 `ant-design` 已使用 `@ant-design/static-style-extract`、Token 统计脚本和 Token 元数据脚本参与生成流程；实现应优先评估复用或调用原生成器，无法直接复用时再建立兼容输入/输出的适配层。

参考资料：[CSS 变量方案](https://ant.design/docs/blog/css-var-plan-cn)、[v6 CSS 细节](https://ant.design/docs/blog/css-tricks-cn)、[ConfigProvider 主题](https://ant.design/components/config-provider-cn#config-provider-demo-theme)、[SSR 静态样式导出](https://ant.design/docs/blog/extract-ssr-cn)、[antd-style](https://github.com/ant-design/antd-style)。

## 需求

### 1. 上游样式与 Token 同步

作为组件库维护者，我希望有一个可重复执行的同步命令，能够读取指定的 `ant-design` 源码路径或 revision，从而持续同步未来的上游样式和 Token 更新。

1. 当维护者提供有效的上游源码路径或 revision 执行同步命令时，系统必须提取支持范围内的 Design Token、组件样式输入、CSS 变量定义和元数据。
2. 当上游输入未变化时，命令必须生成确定性一致的结果，不产生无关文件变化。
3. 当上游输入变化时，命令必须报告变化的 Token/组件样式范围，并只更新受管理的生成文件或明确标记全部生成文件。
4. 当源码路径、revision、必需文件、输入 schema 或生成契约无效时，命令必须以非零退出码失败，并输出可执行的诊断信息。
5. 生成产物必须记录上游 revision、生成器版本和可复现所需的元数据。

### 2. Design Token 模型与 CSS 编译

作为组件库维护者，我希望拥有类型化的 Design Token 模型和 Token → CSS 编译器，从而让 Blazor 组件使用与 Ant Design v6 一致的语义化样式模型。

1. 系统必须表示 Seed Token、Alias Token、Component Token 和 Semantic Token，并记录稳定名称、类型、作用域及上游来源映射。
2. 编译有效 Token 时，编译器必须按确定性顺序输出 CSS 自定义属性，并使用有文档说明的命名规则。
3. 编译器必须支持默认/亮色主题、暗色主题、语义覆盖、组件级覆盖和用户自定义 Token 覆盖，且不依赖旧主题 API。
4. 生成 CSS 必须保留有效 CSS 值、必要的 fallback、选择器作用域、层叠顺序以及支持范围内的 at-rule 行为。
5. 对不支持的上游语法或字段，编译器必须显式报告，不能静默生成错误 CSS。
6. CSS 构建必须区分静态组件样式产物（稳定 class name、selector、状态规则和 `var(...)` 引用）与 Design Token 变量值产物（按主题和 scope 生成的 `--ant-*` 实际值）；两类产物必须可独立构建、缓存、导出和校验。
7. 完整 CSS 必须按固定顺序拼接静态组件样式、Design Token 变量值和运行时覆盖变量，且该顺序必须进入 manifest 和精确一致性测试。
8. 在扩大到全部组件和主题前，系统必须提供一个 MVP 检验阶段，至少能够端到端执行“React 基准生成 → IR → Blazor 静态层/变量值层生成 → 分层精确比较 → 完整 CSS 比较”，并对少量代表性组件给出可重复的通过/失败结果。

### 3. Blazor 版 antd-style 主题对象与样式 API

作为 Blazor 开发者，我希望拥有对应 `antd-style` 的主题对象和样式消费 API，从而在组件和文档中以类型安全、可组合、可复用的方式使用 Ant Design Token。

1. 系统必须提供 Blazor 版主题对象，至少包含 Token、CSS 变量 Token、主题外观/模式、作用域信息和组件级 Token，并支持扩展自定义 Token。
2. 系统必须提供等价于 `createStyles` 的 .NET API，支持接收主题上下文并生成可组合的 class/style 结果；API 应覆盖对象式 CSS 和模板/字符串式 CSS 的主要使用场景。
3. 样式回调上下文必须能访问解析后的 `token`、CSS 变量形式的 `cssVar`、主题信息以及 CSS/class 组合工具，避免业务代码直接拼接变量名。
4. 生成结果必须支持缓存和确定性序列化；同一主题、样式输入和生成器版本不得重复生成不一致的 class 或 CSS。
5. 该 API 必须可被 `ConfigProvider` 等价主题 API、Semantic DOM 演示、13 个首页主题和 SSR 静态样式导出共同使用。
6. 需要区分静态样式、主题变量样式和动态运行时样式，并提供明确的客户端与 SSR 生成边界。

### 4. 原生成器复用与适配

作为组件库维护者，我希望尽可能复用 Ant Design 原有 Token/样式生成器，从而降低自行重实现算法造成的偏差。

1. 方案必须评估 `ant-design` 的 Token 元数据、Token 统计、组件样式生成和静态样式导出脚本的可复用方式。
2. 若调用 Node/TypeScript 原生成器，必须定义版本锁定、进程调用、输入输出 schema、错误传播和跨平台路径策略。
3. 若无法直接调用原生成器，Blazor 适配层必须以可测试的兼容 schema 读取其导出结果，并记录无法复用的算法或语法差异。
4. 每次生成必须保留上游 revision、原生成器版本（如有）、Blazor 适配器版本和输入摘要，便于 CSS 基准复现。
5. 原生成器或适配器升级后，必须重新执行确定性检查、CSS 差异校验和视觉回归测试。

### 5. ConfigProvider 等价主题 API

作为 Blazor 开发者，我希望使用接近 Ant Design `ConfigProvider` 的主题配置 API，从而以统一方式为组件树设置主题、Token、组件 Token 和主题切换状态。

1. 系统必须提供一个 `ConfigProvider` 等价的 Blazor 组件或服务 API，支持在组件树外围提供继承式配置。
2. 主题配置必须覆盖 Ant Design 主题演示中可观察的核心语义：全局 Token、组件 Token、亮/暗算法或等价主题算法、主题作用域以及运行时主题切换。
3. API 必须支持嵌套配置；内层配置应继承外层未覆盖的 Token，并能隔离内层主题对外层组件的影响。
4. API 必须允许预构建主题通过 class 或 data 属性切换，并允许对动态 Token 进行运行时 CSS 变量覆盖。
5. API 必须与现有 Blazor 组件 API 兼容，但不要求兼容旧主题 API、旧主题对象格式或旧样式覆盖方式。
6. 主题上下文必须能被组件和文档演示读取，并在切换时避免不必要的组件重新渲染。

### 6. CSS 基准与样式差异校验

作为维护者，我希望把上游编译出的 CSS 与 Blazor 生成的 CSS 自动比较，从而在发布前发现样式漂移。

1. 对指定 revision 执行校验时，系统必须使用相同的规范化规则分别生成上游 CSS 基准和 Blazor CSS。
2. 校验器必须比较选择器、声明、自定义属性、值、顺序敏感规则和相关 at-rule，并区分格式差异与有意义的样式差异。
3. 发现差异时，系统必须输出机器可读报告和人类可读摘要，并按组件、Token、选择器和差异类型分组。
4. 没有超出容差的有意义差异时，命令必须成功退出，并报告 revision 和比较范围。
5. 系统必须支持带原因的明确差异白名单，白名单项目必须出现在报告中。
6. 未批准的有意义差异必须导致非零退出码，并可直接用于 CI。
7. 在固定上游 revision、依赖 lockfile、运行时版本和编码协议下，系统必须先保存 React/Ant Design 原生成器为每个组件、主题和场景编译出的 CSS 字符串基准，再对 Blazor 生成结果执行 UTF-8 字节级完全相等比较；任何一个字节不同都必须失败。
8. 系统必须同时提供按组件/主题/场景拆分的精确比较和完整 CSS 拼接后的精确比较；结构化 diff 只能用于定位差异，不能用格式容差放宽完全一致门禁。

### 7. SSR 静态样式导出

作为使用 Blazor SSR 或静态站点渲染的开发者，我希望在服务端提前导出当前主题所需的静态 CSS，从而避免首屏样式缺失和主题闪烁。

1. 系统必须提供 SSR/预渲染阶段的静态样式导出能力，生成可直接插入 HTML `<style>` 或外部 CSS 文件的内容。
2. 导出的样式必须包含指定组件树实际需要的基础组件样式、CSS 变量、主题作用域和必要的 fallback，并保持确定性。
3. 导出器必须支持默认主题、暗色主题、全部 13 个首页预览主题以及显式的自定义 Token 配置。
4. SSR 导出结果必须能与客户端运行时使用相同的 Token/CSS 编译器生成，避免服务端和客户端 CSS 不一致。
5. 系统必须提供缓存键或版本指纹，至少包含上游 revision、主题配置和生成器版本；输入不变时应复用导出结果。
6. 导出流程必须支持 CSP nonce 或等价安全配置，并对无法静态导出的动态样式给出明确说明和客户端 fallback。

### 8. 组件样式迁移与 API 兼容

作为现有 Blazor 用户，我希望组件行为和公共 API 保持兼容，从而在样式迁移期间应用代码仍能编译和运行。

1. 迁移后的组件必须保留现有公共组件名、参数、事件、泛型约束和已文档化行为，除非迁移记录明确列出必要变更。
2. 在存在上游映射时，组件样式必须消费生成的 Design Token/CSS 变量层，而不是继续复制硬编码的旧值。
3. 系统必须明确说明不兼容旧主题 API 和旧样式覆盖；移除或变化的入口必须提供迁移诊断或文档。
4. CSS 变量缺失或浏览器不支持时，组件必须按照文档化策略正确渲染或降级。
5. 核心组件样式迁移及校验完成前，不得将文档主题演示视为完成。

### 9. Semantic DOM 文档演示

作为文档读者，我希望看到等价于 `SemanticPreview.tsx` 的 Semantic DOM 演示，从而理解组件语义结构、样式挂载点和可定制区域。

1. 文档必须为代表性迁移组件提供 Semantic DOM 预览，并标识稳定的语义部件、class 和 CSS 变量。
2. 预览必须展示组件结果、语义结构和 Token/样式映射，并支持语义项悬停高亮与固定高亮。
3. 预览必须展示对应的 `classNames`/`styles` 使用示例、语义描述和可选版本标签。
4. 预览必须通过组件库实际使用的 CSS 变量层实现，不能维护独立的伪样式实现。
5. 预览必须支持亮色/暗色主题，并在支持的响应式断点下保持可用。

### 10. 首页全部主题演示

作为文档读者，我希望看到等价于 `.dumi/pages/index/components/ThemePreview` 的交互演示，从而观察所有预置主题和 CSS 变量主题切换效果。

1. 首页必须在核心样式迁移完成后提供交互式主题演示。
2. 演示必须展示全部 13 个注册主题：Ant Design/light、MUI、shadcn、Bootstrap、Cartoon、Dark、Illustration、Glass、Geek、Lark/Document、Blossom、Ant Design V4、Serene。
3. 每个主题必须迁移其全局 Token、组件 Token、算法选择、组件级 class/style 定制和相关视觉资源或等价实现；遗漏或主动不支持的字段必须由工具报告。
4. 切换主题时，演示必须通过作用域 CSS 变量或主题 class/data 属性更新代表性组件，不得要求完整页面刷新。
5. 演示必须提供可访问的当前主题状态、键盘导航和持久化行为。
6. 主题切换不得产生控制台错误、SSR hydration 不一致或组件 API 行为变化。

### 11. 测试、性能与交付门禁

作为项目维护者，我希望有自动化回归和发布门禁，从而保证同步、主题和 SSR 产物长期稳定。

1. 项目必须包含 Token 提取、转换、CSS 编译、规范化、差异分类、白名单和确定性生成测试。
2. 项目必须包含代表性组件的亮色/暗色/嵌套主题渲染测试，以及公共组件 API 兼容测试。
3. 项目必须包含 ConfigProvider 等价 API、SSR 静态样式导出和缓存指纹测试。
4. 项目必须包含 React 基准快照与 Blazor 生成 CSS 的逐组件、逐主题、逐场景及完整 CSS 字符串完全一致测试。
5. 文档测试必须覆盖 Semantic DOM、全部 13 个首页主题、可访问性、键盘导航、视觉快照和控制台错误检查。
6. CI 必须先验证 React 基准，再运行 React/Blazor CSS 字符串精确比较、生成确定性检查、单元/组件测试、文档构建和支持目标框架的构建检查。
7. 必须定义生成 CSS 体积、生成耗时、SSR 导出耗时和运行时主题切换耗时阈值，并将阈值记录在设计或 CI 配置中。
8. 最终交付必须包含迁移指南、生成器/校验器/SSR 导出使用文档、上游 revision 策略、差异白名单策略，并完成包含实现和 specs 文档的 Git 提交。

9. 第二阶段必须包含 React v6 Props、Design Token 与 Blazor 公共 API 的联合校验，覆盖 v4→v6 的属性新增、重命名、拆分、合并和弃用关系；旧属性使用时必须产生编译器警告或等价的迁移诊断。

## 非目标

- 保持旧主题 API 或旧样式覆盖行为兼容。
- 在无必要时重新设计公共组件 API。
- 修改与本次 v6 样式迁移无关的组件功能。
- 让文档演示成为组件样式的事实来源。
