using System.Linq;
using AntDesign.JsInterop;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using OneOf;
using Xunit;

namespace AntDesign.Tests.Icon;

public sealed class IconFontTests : AntDesignTestBase
{
#if NET8_0_OR_GREATER
    [Fact]
    public async System.Threading.Tasks.Task Server_prerender_outputs_the_reference_without_running_script_loading()
    {
        await using var renderer = new Microsoft.AspNetCore.Components.Web.HtmlRenderer(
            Services, Services.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>());
        var html = await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await renderer.RenderComponentAsync<IconFont>(
                Microsoft.AspNetCore.Components.ParameterView.FromDictionary(new System.Collections.Generic.Dictionary<string, object>
                {
                    ["Type"] = "icon-facebook", ["ScriptUrl"] = (OneOf<string, string[]>)"font.js"
                }));
            return output.ToHtmlString();
        });
        Assert.Contains("xlink:href=\"#icon-facebook\"", html);
        Assert.DoesNotContain("<script", html);
        Assert.Empty(JSInterop.Invocations);
    }
#endif

    [Fact]
    public void Font_markup_matches_the_React_use_wrapper_and_updates_with_Type()
    {
        var cut = RenderComponent<IconFont>(p => p.Add(x => x.Type, "icon-facebook"));
        Assert.Equal("anticon", cut.Find("span").ClassName);
        var svg = cut.Find("svg");
        Assert.Equal("true", svg.GetAttribute("aria-hidden"));
        Assert.Equal("false", svg.GetAttribute("focusable"));
        Assert.Equal("1em", svg.GetAttribute("width"));
        Assert.Null(svg.GetAttribute("style"));
        var href = cut.Find("use").Attributes.Single(attribute => attribute.LocalName == "href");
        Assert.Equal("http://www.w3.org/1999/xlink", href.NamespaceUri);
        Assert.Equal("#icon-facebook", href.Value);
        cut.SetParametersAndRender(p => p.Add(x => x.Type, "icon-twitter"));
        Assert.Equal("#icon-twitter", cut.Find("use").Attributes.Single(attribute => attribute.LocalName == "href").Value);
        Assert.Empty(JSInterop.Invocations);
    }

    [Fact]
    public void Script_arrays_are_one_JS_argument_and_unchanged_resources_are_not_requested_on_rerender()
    {
        var urls = new[] { "first.js", "last.js" };
        JSInterop.SetupVoid(JSInteropConstants.CreateIconFromfontCN, _ => true).SetVoidResult();
        var cut = RenderComponent<IconFont>(p => p.Add(x => x.Type, "icon-test")
            .Add(x => x.ScriptUrl, (OneOf<string, string[]>)urls));
        var request = Assert.Single(JSInterop.Invocations[JSInteropConstants.CreateIconFromfontCN]);
        Assert.Equal(urls, Assert.IsType<string[]>(Assert.Single(request.Arguments)));
        cut.Render();
        cut.SetParametersAndRender(p => p.Add(x => x.Type, "icon-other")
            .Add(x => x.ScriptUrl, (OneOf<string, string[]>)new[] { "first.js", "last.js" }));
        Assert.Single(JSInterop.Invocations[JSInteropConstants.CreateIconFromfontCN]);
        cut.SetParametersAndRender(p => p.Add(x => x.ScriptUrl, (OneOf<string, string[]>)"next.js"));
        Assert.Equal(2, JSInterop.Invocations[JSInteropConstants.CreateIconFromfontCN].Count);
    }

    [Fact]
    public void Spin_and_rotation_apply_to_the_SVG_and_keep_custom_color_on_the_wrapper()
    {
        var cut = RenderComponent<IconFont>(p => p.Add(x => x.Type, "icon-test").Add(x => x.Spin, true)
            .Add(x => x.Rotate, 90).Add(x => x.Style, "color:#1877F2"));
        Assert.Equal("anticon", cut.Find("span").ClassName);
        Assert.Equal("anticon-spin", cut.Find("svg").ClassName);
        Assert.Contains("rotate(90deg)", cut.Find("svg").GetAttribute("style"));
        Assert.Contains("#1877F2", cut.Find("span").GetAttribute("style"));
    }
}
