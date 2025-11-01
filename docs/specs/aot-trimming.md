# AOT Trimming and AntDesign.Blazor — Spec

目标
- 让 AntDesign.Blazor 组件库以尽量小的改动支持 AOT trimming（ILLink trimming + AOT publish）场景。优先保证安全性（不破坏运行），随后迭代减少保留surface。

约束与假设
- 组件库是一个类库（`components/AntDesign.csproj`），最终由应用（例如 `site/AntDesign.Docs.Wasm`）在发布时开启 `PublishTrimmed` 与 `PublishAot`。
- 组件基于 Blazor（Razor 组件），大量类型/成员可能通过反射或 Razor 运行时被加载，因此初始阶段建议保留程序集，随后细化。
- 我们不能（也不应该）在库项目里强行打开 `PublishTrimmed` 或 `PublishAot`，因为这些是应用级别的发布设置。库的职责是提供正确的修剪描述（linker descriptor）和所需的保存注解（attributes）以便在应用端安全修剪。

设计（高层）
1. 在 `components/` 下添加 `linker.xml`（Trimmer root descriptor），当前版本保守策略：保留整个 `AntDesign` 程序集（`<assembly fullname="AntDesign"><type fullname="*" preserve="all"/></assembly>`）。
2. 在 `components/AntDesign.csproj` 中将 `linker.xml` 作为 `<TrimmerRootDescriptor Include="linker.xml" />` 并打包到 NuGet 的 `contentFiles/any/any` 中，这样引用此包的应用在发布时能够自动获取到该描述文件并交给 ILLink 使用。
3. 可选（后续）：用 `DynamicallyAccessedMembers`、`[DynamicDependency]` 或将 `linker.xml` 细化为仅保留 Razor 组件类型、以事件/反射方式访问的类型、以及本地化资源。目标是尽量减小保留量。
4. 文档/测试：在 `docs/specs` 添加本说明，并在 README 或 CI 中提供如何在 docs site 上验证 trimming 的步骤。

实现细节
- 添加文件： `components/linker.xml`（已保守实现）。
- 编辑：`components/AntDesign.csproj`
  - 添加:
    ```xml
    <ItemGroup>
      <TrimmerRootDescriptor Include="linker.xml">
        <Pack>true</Pack>
        <PackagePath>contentFiles\\any\\any</PackagePath>
      </TrimmerRootDescriptor>
    </ItemGroup>
    ```
  - 目的：当库被打包/引用时，linker 描述会和包一起被下发给最终应用（NuGet client 安装时会把 contentFiles 放在 packages 文件夹内并在 publish 时被读取）。

验证计划（快速验证）
1. 在本地（或 CI）切换到 docs WASM 项目 `site/AntDesign.Docs.Wasm`：
   - 在其项目或 `dotnet publish` 命令中启用 `PublishTrimmed=true` 与 `PublishAot=true`（或按需要在命令行传递）。
   - 示例命令（在 docs 应用根目录运行）：
     ```bash
     dotnet publish -c Release -p:PublishTrimmed=true -p:PublishAot=true
     ```
2. 观察 publish 日志：ILLink 是否加载了 `linker.xml`（日志会显示已应用的 trimmer root descriptors）；运行生成的 WASM 并检查在浏览器中组件是否正常工作。
3. 如果出现运行时缺失问题：
   - 检查运行时异常堆栈与缺失类型名。
   - 将这些类型加入到 `linker.xml`，或在源代码中为被反射使用的位置添加 `DynamicallyAccessedMembers` 注解或 `DynamicDependency`。
4. 进一步优化：当功能稳定后，逐步替换 `preserve="all"` 为更精确的 type/field/method 保留节点，减小最终二进制。

边界情况与风险
- 风险：保守策略（保留整个程序集）会阻止 trimming 带来太多体积收益，但可以保证安全性。
- 风险：把 `linker.xml` 放到 NuGet contentFiles 后，某些打包/restore 场景下应用可能不会自动加载此描述；需要在应用端 publish 日志中确认描述被拾取。若需要，也可以把 descriptor 作为 `build` targets 注入（更深入的包装）。

下一步（建议）
- 运行一次 docs site 的 `dotnet publish -p:PublishTrimmed=true -p:PublishAot=true` 来验证当前保守策略是否工作。如需我代为运行，请确认并允许我触发构建验证。
- 在后续迭代中：分析运行时使用情况（反射/组件激活路径），并逐步精简 `linker.xml`。

质量门（Build / Lint / Tests）
- Build: 确认 `components/` 项目仍可编译（`dotnet build components/AntDesign.csproj`）。
- Lint/Tests: 若有组件单元测试（`tests/`），在启用 trimming/AOT 的场景下进行一次 publish-run 验证。由于 AOT/publish 更像运行时集成验证，建议将其列为集成测试步骤并在 CI 中作为可选长跑步骤。

