// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign;

/**
<summary>
<para>A button that floats at the top of the page.</para>

<h2>When To Use</h2>

<list type="bullet">
    <item>For global functionality on the site.</item>
    <item>Buttons that can be seen wherever you browse.</item>
</list>
</summary>
*/
[Documentation(DocumentationCategory.Components, DocumentationType.General, "https://mdn.alipayobjects.com/huamei_7uahnr/afts/img/A*tXAoQqyr-ioAAAAAAAAAAAAADrJ8AQ/original", Columns = 2, Title = "FloatButton", SubTitle = "悬浮按钮")]
public partial class FloatButton : AntDomComponentBase
{
    private const string Prefix = "ant-float-btn";

    [CascadingParameter]
    internal FloatButtonGroup Group { get; set; }

    /// <summary>
    /// Set the icon component of button
    /// </summary>
    [Parameter]
    public string Icon { get; set; }

    /// <summary>
    /// Set the icon component of button using a RenderFragment.
    /// </summary>
    [Parameter]
    public RenderFragment IconTemplate { get; set; }

    /// <summary>
    /// Please use `Content` instead.
    /// </summary>
    [Obsolete("Please use Content instead.")]
    [Parameter]
    public string Description { get; set; }

    /// <summary>
    /// Text and other
    /// </summary>
    [Parameter]
    public string Content { get; set; }

    /// <summary>
    /// The text shown in the tooltip
    /// </summary>
    [Parameter]
    public string Tooltip { get; set; }

    /// <summary>
    /// Setting button type
    /// </summary>
    /// <default value="default" />
    [Parameter]
    public FloatButtonType Type { get; set; } = FloatButtonType.Default;

    /// <summary>
    /// Setting button shape
    /// </summary>
    /// <default value="circle" />
    [Parameter]
    public FloatButtonShape Shape { get; set; } = FloatButtonShape.Circle;

    /// <summary>
    /// Set the handler to handle `click` event
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// The target of hyperlink
    /// </summary>
    [Parameter]
    public string Href { get; set; }

    /// <summary>
    /// Specifies where to display the linked URL
    /// </summary>
    [Parameter]
    public string Target { get; set; }

    /// <summary>
    /// Attach Badge to FloatButton. `status` and other props related are not supported.
    /// </summary>
    [Parameter]
    public FloatButtonBadge Badge { get; set; }

    /// <summary>
    /// Whether the button is disabled
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /**
     * @since 5.21.0
     * @default button
     */
    /// <summary>
    /// Set the original html `type` of `button`, see: https://developer.mozilla.org/en-US/docs/Web/HTML/Element/button#type
    /// </summary>
    [Parameter]
    public string HtmlType { get; set; } = "button";

    /// <summary>
    /// Set the value of the aria-label attribute
    /// </summary>
    [Parameter]
    public string AriaLabel { get; set; }

    /// <summary>
    /// Content of the button.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    /// <summary>
    /// The placement of the tooltip.
    /// </summary>
    [Parameter]
    public Placement TooltipPlacement { get; set; } = Placement.Top;

    /// <summary>Semantic class names for the root, icon, and content elements.</summary>
    [Parameter]
    public IDictionary<string, string> ClassNames { get; set; }

    /// <summary>Semantic inline styles for the root, icon, and content elements.</summary>
    [Parameter]
    public IDictionary<string, string> Styles { get; set; }

    internal bool IsIndividual => Group == null;

    internal string PrefixCls => Prefix;

    internal string GetSemanticClass(string name)
    {
        if (ClassNames != null && ClassNames.TryGetValue(name, out var ownClass))
            return ownClass;

        var isTrigger = Class?.Contains("ant-float-btn-group-trigger", StringComparison.Ordinal) == true;
        var groupName = name switch
        {
            "root" => isTrigger ? "trigger" : "item",
            "icon" => isTrigger ? "triggerIcon" : "itemIcon",
            "content" => isTrigger ? "triggerContent" : "itemContent",
            _ => name
        };
        return Group?.GetSemanticClass(groupName) ?? string.Empty;
    }

    internal string GetSemanticStyle(string name)
    {
        if (Styles != null && Styles.TryGetValue(name, out var ownStyle))
            return ownStyle;

        var isTrigger = Class?.Contains("ant-float-btn-group-trigger", StringComparison.Ordinal) == true;
        var groupName = name switch
        {
            "root" => isTrigger ? "trigger" : "item",
            "icon" => isTrigger ? "triggerIcon" : "itemIcon",
            "content" => isTrigger ? "triggerContent" : "itemContent",
            _ => name
        };
        return Group?.GetSemanticStyle(groupName) ?? string.Empty;
    }

    internal string GetElementClass(string baseClass, string name) => $"{baseClass} {GetSemanticClass(name)}".Trim();

    internal string GetElementStyle(string baseStyle, string name)
    {
        var semanticStyle = GetSemanticStyle(name);
        return string.IsNullOrWhiteSpace(semanticStyle) ? baseStyle : $"{baseStyle};{semanticStyle}";
    }

    internal bool HasContent => ChildContent != null || !string.IsNullOrWhiteSpace(Content ?? Description);

    protected override void OnInitialized()
    {
        base.OnInitialized();
        SetClass();
    }

    private void SetClass()
    {
        var shape = Group?.Shape ?? Shape;
        var individual = IsIndividual;

        ClassMapper
            .Add("ant-btn")
            .Add("ant-btn-lg")
            .Add(Prefix)
            .Add($"{Prefix}-{Type.ToString().ToLowerInvariant()}")
            .Add($"{Prefix}-{shape.ToString().ToLowerInvariant()}")
            .If($"{Prefix}-individual", () => individual)
            .If($"{Prefix}-icon-only", () => !HasContent)
            .If($"{Prefix}-disabled", () => Disabled)
            .If($"{Prefix}-rtl", () => RTL);
    }

    private async Task OnClickHandler(MouseEventArgs args)
    {
        if (Disabled)
            return;

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(args);
    }
}
