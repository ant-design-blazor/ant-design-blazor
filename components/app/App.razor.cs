using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign;

/// <summary>
/// Application root container and local cascading context for Ant Design services.
/// </summary>
[Documentation(DocumentationCategory.Components, DocumentationType.Other, "", Title = "App", SubTitle = "应用根容器")]
public class App : ComponentBase
{
    [Inject] private IMessageService MessageService { get; set; }
    [Inject] private INotificationService NotificationService { get; set; }
    [Inject] private ModalService ModalService { get; set; }

    /// <summary>CSS class prefix for the application root.</summary>
    [Parameter] public string PrefixCls { get; set; } = "ant-app";
    /// <summary>Additional class name for the root element.</summary>
    [Parameter] public string ClassName { get; set; }
    /// <summary>Additional root class name.</summary>
    [Parameter] public string RootClassName { get; set; }
    /// <summary>Inline style for the root element.</summary>
    [Parameter] public string Style { get; set; }
    /// <summary>HTML element name used for the root element.</summary>
    [Parameter] public string Component { get; set; } = "div";
    /// <summary>Content rendered inside the application root and its cascaded context.</summary>
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

    private AntDesignAppContext _appContext;

    protected override void OnInitialized()
    {
        _appContext = new AntDesignAppContext(MessageService, NotificationService, ModalService);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<AntDesignAppContext>>(0);
        builder.AddAttribute(1, "Value", _appContext);
        builder.AddAttribute(2, "IsFixed", true);
        builder.AddAttribute(3, "ChildContent", (RenderFragment)RenderRoot);
        builder.CloseComponent();
    }

    private void RenderRoot(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, string.IsNullOrWhiteSpace(Component) ? "div" : Component);
        if (AdditionalAttributes != null)
        {
            builder.AddMultipleAttributes(1, AdditionalAttributes);
        }

        var classes = string.Join(" ", new[] { PrefixCls, ClassName, RootClassName }
            .Where(value => !string.IsNullOrWhiteSpace(value)));
        builder.AddAttribute(2, "class", classes);
        if (!string.IsNullOrWhiteSpace(Style))
        {
            builder.AddAttribute(3, "style", Style);
        }

        builder.AddContent(4, ChildContent);
        builder.CloseElement();
    }
}
