# Ant Design v6 同步器

从 React `ant-design` 的指定 Git revision 重新生成 Blazor v6 的迁移输入、强类型 Token、React CSS 基准和颜色算法测试快照：

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
4. 保存 React 编译 CSS 基准。
5. 保存 `@ant-design/colors` 迁移测试快照。
6. 写入 `.vibedev/generated/antd-v6/sync-manifest.json`。

之后可以使用 `exact-check` 对 React 基准和 Blazor 产物执行字节级校验。
