---
order: 2
title: "MCP Documentation"
---

AntDesign.Docs.MCP provides tools for AI coding to query component documentation and demo source code, helping AI produce accurate code for specific scenarios and features.

### Tools

- ListComponents() — returns the list of components
- SearchComponents(names) — searches components by comma-separated names
- ListAllDemos() — lists demos (component / scenario / description)
- SearchComponentDemos(queries) — queries demo source by "Component:Scenario"

### Configuration

Add the following to your editor's MCP configuration. Note: `dnx` requires .NET 10+.

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

If your environment does not have .NET 10 installed, you can install the dotnet tool first and use the dotnet tool configuration:

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

### CLI quick lookups

- List components: `antdesign-docs-mcp component list`
- Search components: `antdesign-docs-mcp component search Button,Input`
- List demos for a component: `antdesign-docs-mcp demo list Button`
- Print demo source (optionally narrow by scenario): `antdesign-docs-mcp demo source Button:Icon`
