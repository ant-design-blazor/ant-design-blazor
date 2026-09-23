using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AntDesign.Docs.Services;

public sealed record SemanticDomNode(
    string Name,
    string Selector,
    string Description,
    string? Version = null);

public static class SemanticDomCatalog
{
    private static readonly IReadOnlyDictionary<string, IReadOnlyList<SemanticDomNode>> Nodes =
        new ReadOnlyDictionary<string, IReadOnlyList<SemanticDomNode>>(
            new Dictionary<string, IReadOnlyList<SemanticDomNode>>(StringComparer.OrdinalIgnoreCase)
            {
                ["button"] = new[]
                {
                    new SemanticDomNode("root", ".ant-btn", "按钮根元素", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-btn-icon", "按钮图标元素", "6.0.0"),
                    new SemanticDomNode("content", ".ant-btn-content", "按钮内容元素", "6.0.0"),
                },
                ["alert"] = new[]
                {
                    new SemanticDomNode("root", ".ant-alert", "提示根元素", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-alert-icon", "提示图标元素", "6.0.0"),
                    new SemanticDomNode("section", ".ant-alert-section", "提示内容区域", "6.0.0"),
                    new SemanticDomNode("title", ".ant-alert-title", "提示标题元素", "6.0.0"),
                    new SemanticDomNode("description", ".ant-alert-description", "提示描述元素", "6.0.0"),
                    new SemanticDomNode("close", ".ant-alert-close-icon", "关闭按钮元素", "6.0.0"),
                },
                ["badge"] = new[]
                {
                    new SemanticDomNode("root", ".ant-badge", "徽标根元素", "6.0.0"),
                    new SemanticDomNode("indicator", ".ant-badge-count, .ant-badge-dot, .ant-badge-status-dot", "徽标指示器元素", "6.0.0"),
                },
                ["breadcrumb"] = new[]
                {
                    new SemanticDomNode("root", ".ant-breadcrumb", "面包屑根元素", "6.0.0"),
                    new SemanticDomNode("item", ".ant-breadcrumb-item", "面包屑项目元素", "6.0.0"),
                    new SemanticDomNode("separator", ".ant-breadcrumb-separator", "面包屑分隔符元素", "6.0.0"),
                },
                ["card"] = new[]
                {
                    new SemanticDomNode("root", ".ant-card", "卡片根元素", "6.0.0"),
                    new SemanticDomNode("header", ".ant-card-head", "卡片头部元素", "6.0.0"),
                    new SemanticDomNode("title", ".ant-card-head-title", "卡片标题元素", "6.0.0"),
                    new SemanticDomNode("extra", ".ant-card-extra", "卡片额外内容元素", "6.0.0"),
                    new SemanticDomNode("cover", ".ant-card-cover", "卡片封面元素", "6.0.0"),
                    new SemanticDomNode("body", ".ant-card-body", "卡片内容元素", "6.0.0"),
                    new SemanticDomNode("actions", ".ant-card-actions", "卡片操作区元素", "6.0.0"),
                    new SemanticDomNode("section", ".ant-card-meta-section", "卡片元信息内容区域", "6.0.0"),
                    new SemanticDomNode("avatar", ".ant-card-meta-avatar", "卡片元信息头像元素", "6.0.0"),
                    new SemanticDomNode("description", ".ant-card-meta-description", "卡片元信息描述元素", "6.0.0"),
                },
                ["collapse"] = new[]
                {
                    new SemanticDomNode("root", ".ant-collapse", "折叠面板根元素", "6.0.0"),
                    new SemanticDomNode("header", ".ant-collapse-header", "折叠面板头部元素", "6.0.0"),
                    new SemanticDomNode("title", ".ant-collapse-header-text", "折叠面板标题元素", "6.0.0"),
                    new SemanticDomNode("icon", ".ant-collapse-expand-icon", "折叠面板图标元素", "6.0.0"),
                    new SemanticDomNode("body", ".ant-collapse-content-box", "折叠面板内容元素", "6.0.0"),
                },
                ["descriptions"] = new[]
                {
                    new SemanticDomNode("root", ".ant-descriptions", "描述列表根元素", "6.0.0"),
                    new SemanticDomNode("header", ".ant-descriptions-header", "描述列表头部元素", "6.0.0"),
                    new SemanticDomNode("title", ".ant-descriptions-title", "描述列表标题元素", "6.0.0"),
                    new SemanticDomNode("extra", ".ant-descriptions-extra", "描述列表额外内容元素", "6.0.0"),
                    new SemanticDomNode("label", ".ant-descriptions-item-label", "描述列表标签元素", "6.0.0"),
                    new SemanticDomNode("content", ".ant-descriptions-item-content", "描述列表内容元素", "6.0.0"),
                },
                ["divider"] = new[]
                {
                    new SemanticDomNode("root", ".ant-divider", "分割线根元素", "6.0.0"),
                    new SemanticDomNode("rail", ".ant-divider-rail", "分割线连接条元素", "6.0.0"),
                    new SemanticDomNode("content", ".ant-divider-inner-text", "分割线文本内容元素", "6.0.0"),
                },
            });

    public static IReadOnlyCollection<string> Components => Nodes.Keys
        .Concat(SemanticDomGeneratedCatalog.Nodes.Keys)
        .Concat(new[] { "qrcode" })
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public static IReadOnlyList<SemanticDomNode> Get(string component)
    {
        var normalizedComponent = string.Equals(component, "qrcode", StringComparison.OrdinalIgnoreCase)
            ? "qr-code"
            : component ?? string.Empty;
        return Nodes.TryGetValue(normalizedComponent, out var nodes)
            ? nodes
            : SemanticDomGeneratedCatalog.Nodes.TryGetValue(normalizedComponent, out nodes)
                ? nodes
                : Array.Empty<SemanticDomNode>();
    }
}
