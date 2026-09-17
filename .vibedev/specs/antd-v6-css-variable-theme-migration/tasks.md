# Ant Design v6 CSS 变量主题迁移实施任务

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

7. [ ] 接入核心组件生成 CSS 并完成 React/Blazor exact gate
   - 按组件迁移现有 Less 样式到生成 CSS/变量引用，保留公共参数、事件、泛型约束和组件行为。
   - 首批覆盖 Button、Alert、Modal、Card、Input、Select、Menu、Layout、Progress、Steps、Slider、ColorPicker、Notification、Switch、Radio 等主题演示使用组件。
   - 覆盖需求 6.1-6.5、4.7-4.8。

7.1. [ ] 建立代表性组件 exact CSS 测试矩阵
   - 对每个迁移组件执行 React 基准与 Blazor candidate 的组件、主题、变体、状态、RTL、紧凑尺寸、弹层和完整 CSS 比较。
   - 覆盖需求 4.1-4.8、9.2、9.4。

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

9.1. [ ] 实现首页主题切换组件和自动化覆盖
   - 在 `site/AntDesign.Docs` 中实现主题选择、当前项无障碍状态、键盘左右导航、持久化、无整页刷新切换和代表性组件预览。
   - 覆盖需求 10.1-10.6、9.4。

10. [ ] 实现 Semantic DOM 预览组件
   - 在 `site/AntDesign.Docs` 创建 Blazor `SemanticPreview`、语义路径映射、悬停/固定高亮、版本标签、代码示例和子组件 class/style 注入。
   - 使用实际 Token/CSS Variable 主题上下文，不创建独立伪样式。
   - 覆盖需求 9.1-9.5。

10.1. [ ] 编写 Semantic DOM 文档自动化测试
   - 覆盖语义列表渲染、悬停/键盘焦点/固定状态、代码示例、亮暗主题、响应式布局和控制台错误。
   - 覆盖需求 9.2-9.5、11.4。

11. [ ] 接入 CI 生成、exact gate、构建和测试门禁
   - 添加 Node/.NET 生成与测试命令，先验证 React 基准，再执行 Blazor exact CSS、确定性生成、组件/API、SSR、文档和视觉快照测试。
   - 对 managed generated files、未支持字段和无理由 diff 设置失败门禁。
   - 覆盖需求 1.2、1.4、4.6-4.8、9.1-9.7。

11.1. [ ] 添加性能阈值测试和生成提交校验
   - 将 CSS 体积、生成耗时、SSR 导出耗时和运行时变量切换耗时作为可配置测试阈值，并验证生成元数据随代码提交更新。
   - 覆盖需求 11.5-11.7。
