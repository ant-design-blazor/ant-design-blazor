using AntDesign.Tests;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Xunit;

namespace AntDesign.Tests.App;

public sealed class AppTests : AntDesignTestBase
{
    [Fact]
    public void App_renders_the_default_v6_root_and_child_content()
    {
        var cut = Context.RenderComponent<AntDesign.App>(parameters => parameters
            .AddChildContent("Application content"));

        var root = cut.Find(".ant-app");
        Assert.Equal("div", root.TagName.ToLowerInvariant());
        Assert.Equal("Application content", root.TextContent);
    }

    [Fact]
    public void App_supports_a_custom_root_element_class_and_style()
    {
        var cut = Context.RenderComponent<AntDesign.App>(parameters => parameters
            .Add(x => x.Component, "section")
            .Add(x => x.ClassName, "custom-app")
            .Add(x => x.RootClassName, "root-marker")
            .Add(x => x.Style, "min-height:100vh")
            .AddChildContent("Page"));

        var root = cut.Find("section.ant-app.custom-app.root-marker");
        Assert.Equal("min-height:100vh", root.GetAttribute("style"));
        Assert.Equal("Page", root.TextContent);
    }

    [Fact]
    public void App_cascades_its_service_context_only_to_its_child_subtree()
    {
        var cut = Context.RenderComponent<AntDesign.App>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<AppContextProbe>(0);
                builder.CloseComponent();
            }));

        var probe = cut.Find("output");
        Assert.True(probe.HasAttribute("data-has-context"));
        Assert.True(probe.HasAttribute("data-has-message"));
        Assert.True(probe.HasAttribute("data-has-notification"));
        Assert.True(probe.HasAttribute("data-has-modal"));
    }

    [Fact]
    public void Each_App_instance_creates_a_distinct_cascading_context()
    {
        var cut = Context.RenderComponent<TwoAppHost>();

        Assert.NotNull(cut.Instance.FirstContext);
        Assert.NotNull(cut.Instance.SecondContext);
        Assert.NotSame(cut.Instance.FirstContext, cut.Instance.SecondContext);
    }

    private sealed class AppContextProbe : ComponentBase
    {
        [CascadingParameter] public AntDesign.AntDesignAppContext Context { get; set; }
        [Parameter] public System.Action<AntDesign.AntDesignAppContext> OnContext { get; set; }

        protected override void OnParametersSet()
        {
            OnContext?.Invoke(Context);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "output");
            builder.AddAttribute(1, "data-has-context", Context != null);
            builder.AddAttribute(2, "data-has-message", Context?.Message != null);
            builder.AddAttribute(3, "data-has-notification", Context?.Notification != null);
            builder.AddAttribute(4, "data-has-modal", Context?.Modal != null);
            builder.CloseElement();
        }
    }

    private sealed class TwoAppHost : ComponentBase
    {
        public AntDesign.AntDesignAppContext FirstContext { get; private set; }
        public AntDesign.AntDesignAppContext SecondContext { get; private set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<AntDesign.App>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(childBuilder => AddProbe(childBuilder, context => FirstContext = context)));
            builder.CloseComponent();
            builder.OpenComponent<AntDesign.App>(2);
            builder.AddAttribute(3, "ChildContent", (RenderFragment)(childBuilder => AddProbe(childBuilder, context => SecondContext = context)));
            builder.CloseComponent();
        }

        private static void AddProbe(RenderTreeBuilder builder, System.Action<AntDesign.AntDesignAppContext> capture)
        {
            builder.OpenComponent<AppContextProbe>(0);
            builder.AddAttribute(1, nameof(AppContextProbe.OnContext), capture);
            builder.CloseComponent();
        }
    }
}
