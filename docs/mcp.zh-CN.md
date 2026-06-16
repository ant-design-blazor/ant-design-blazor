---
order: 2
title: "文档 MCP"
---

AntDesign.Docs.MCP 为 AI Coding 工具提供用于查询组件文档和示例源码，有助于 AI 根据需要的场景和功能生成最准确的代码。

### 工具一览

- ListComponents() — 返回组件列表
- SearchComponents(names) — 按逗号分隔的组件名进行查找
- ListAllDemos() — 列出演示（组件/场景/描述）
- SearchComponentDemos(queries) — 按 "组件:场景" 查询演示源代码

### 配置方式

在编程工具的 mcp 配置文件中添加以下配置即可。注意：`dnx` 需要 .NET 10+。

```json
{
  "servers": {
    "AntDesign.Docs.MCP (nuget/dnx)": {
      "type": "stdio",
      "command": "dnx",
      "args": [
        "AntDesign.Docs.MCP@0.1.0",
        "--add-source",
        "${workspaceFolder}/nupkgs",
        "--yes"
      ]
    }
  }
}
```

如果你当前的环境没有安装 .NET 10, 可用先以 dotnet tool 方式安装，再用以下配置：

```bash
dotnet tool install -g antdesign-docs-mcp
```

```json
{
  "servers": {
    "AntDesign.Docs.MCP (dotnet tool)": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "tool",
        "run",
        "antdesign-docs-mcp"
      ]
    }
  }
}
```

不使用 mcp，也可用 CLI 命令速查 （可搭配 Skills 使用）

- 列出组件： `antdesign-docs-mcp component list`
- 搜索组件： `antdesign-docs-mcp component search Button,Input`
- 列出某组件的演示： `antdesign-docs-mcp demo list Button`
- 打印演示源码（可按场景匹配）： `antdesign-docs-mcp demo source Button:Icon`