# AntDesign.Docs.MCP

AntDesign.Docs.MCP is an MCP (Model Context Protocol) server that exposes tools for querying Ant Design Blazor component documentation and demo source code. It can run locally for development, be integrated into editors (VS Code / Visual Studio) via MCP, or be packaged as a NuGet/dotnet tool for distribution.

The MCP server is built as a self-contained application and does not require the .NET runtime to be installed on the target machine.
However, since it is self-contained, it must be built for each target platform separately.
By default, the template is configured to build for:
* `win-x64`
* `win-arm64`
* `osx-arm64`
* `linux-x64`
* `linux-arm64`
* `linux-musl-x64`

If your users require more platforms to be supported, update the list of runtime identifiers in the project's `<RuntimeIdentifiers />` element.

See [aka.ms/nuget/mcp/guide](https://aka.ms/nuget/mcp/guide) for the full guide.

Please note that this template is currently in an early preview stage. If you have feedback, please take a [brief survey](http://aka.ms/dotnet-mcp-template-survey).

## Features

- Component discovery: list and search components
- Demo browsing: list demos for a component and retrieve demo source code
- CLI commands for quick lookups (independent from the MCP server runtime)
- StdIO transport for editor integrations (Copilot / Agent)

> Note: packaging & publishing remains the same (use `dotnet pack` and `dotnet nuget push` when you are ready).

## Local development

Quick start (from repository root):

- Run the MCP server (stdio transport) for editor integration:

```bash
dotnet run --project site/AntDesign.Docs.MCP/AntDesign.Docs.MCP.csproj
```

- CLI mode examples (no server startup):

```bash
# list components
dotnet run --project site/AntDesign.Docs.MCP/AntDesign.Docs.MCP.csproj -- component list

# list demos for Button
dotnet run --project site/AntDesign.Docs.MCP/AntDesign.Docs.MCP.csproj -- demo list Button

# print demo source
dotnet run --project site/AntDesign.Docs.MCP/AntDesign.Docs.MCP.csproj -- demo source Button:Icon
```

Pack & test locally:

```bash
dotnet pack -c Release -o nupkgs
# add the generated nupkgs to your workspace/local feed for dnx or tool install tests
```

Install as a dotnet tool (optional):

```powershell
# local install from workspace feed
dotnet tool install -g AntDesign.Docs.MCP --add-source ./nupkgs --version 0.1.0
# run installed tool
antdesign-docs-mcp component list
```

VS Code example (`.vscode/mcp.json`) is included in this repository for convenience.

---

## Installation & usage

Prerequisites:
- .NET SDK 10 (preview) is required for `dnx` based flows. For local development `dotnet run` works with .NET 8+.

From source (development):
1. Run the server directly for development:

```bash
dotnet run --project site/AntDesign.Docs.MCP/AntDesign.Docs.MCP.csproj
```

Pack and test NuGet-based flow locally:

```bash
dotnet pack -c Release -o nupkgs
# In VS Code, update .vscode/mcp.json to point to your local nupkgs source (already provided in this repo)
# Or run using dnx (requires .NET 10 with dnx):
dnx AntDesign.Docs.MCP@0.1.0 --add-source ${workspaceFolder}/nupkgs --yes
```

Install as a dotnet global tool (optional):

```powershell
# global install from local feed
dotnet tool install -g AntDesign.Docs.MCP --add-source ${PWD}/nupkgs --version 0.1.0
# run it
dotnet tool run andesign-docs-mcp
```

VS Code configuration (example `.vscode/mcp.json`):

```json
{
  "servers": {
    "AntDesign.Docs.MCP (local)": {
      "type": "stdio",
      "command": "dotnet",
      "args": [ "run", "--project", "site/AntDesign.Docs.MCP/AntDesign.Docs.MCP.csproj" ]
    },
    "AntDesign.Docs.MCP (nuget/dnx)": {
      "type": "stdio",
      "command": "dnx",
      "args": [ "AntDesign.Docs.MCP@0.1.0", "--add-source", "${workspaceFolder}/nupkgs", "--yes" ]
    },
    "AntDesign.Docs.MCP (dotnet tool)": {
      "type": "stdio",
      "command": "dotnet",
      "args": [ "tool", "run", "antdesign-docs-mcp" ]
    }
  }
}
```

Notes:
- `dnx` is only available in .NET 10+ SDKs; if you don't have .NET 10 installed, use the local `dotnet run` flow.
- If you publish to NuGet.org, update `.mcp/server.json` with accurate `package` metadata and inputs before publishing.

CLI commands (local tool)

- This package provides a few convenient CLI commands for exploring components and demos without starting the MCP server. Commands are implemented under `Cli/` (`ComponentCommand.cs`, `DemoCommand.cs`) and are **independent** from the MCP server runtime.

Examples (after installing the tool or running via `dotnet run`):

- List components: `antdesign-docs-mcp component list`
- Search components: `antdesign-docs-mcp component search Button,Input`
- List demos for a component: `antdesign-docs-mcp demo list Button`
- Print demo source (optionally narrow by scenario): `antdesign-docs-mcp demo source Button:Icon`

## Packaged fallback for offline usage

The MCP server prefers to download the documentation metadata from the remote site by default. If the download fails (network issues, firewall, etc.), the server will fall back to reading pre-generated JSON files shipped inside the NuGet package (under the `data/` path). You can generate these JSON files as part of your release pipeline using `AntDesign.Docs.Build.CLI` and include them in the package.

A sample GitHub Actions workflow `.github/workflows/publish-mcp.yml` is included that builds the project and attempts to run the build CLI to generate JSON files into `site/AntDesign.Docs.MCP/data` before packing and publishing the NuGet package.

## More information

.NET MCP servers use the [ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol) C# SDK. For more information about MCP:

- [Official Documentation](https://modelcontextprotocol.io/)
- [Protocol Specification](https://spec.modelcontextprotocol.io/)
- [GitHub Organization](https://github.com/modelcontextprotocol)
- [MCP C# SDK](https://modelcontextprotocol.github.io/csharp-sdk)
