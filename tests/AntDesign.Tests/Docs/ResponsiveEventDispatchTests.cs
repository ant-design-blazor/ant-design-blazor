using System;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Bunit;
using Microsoft.JSInterop;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class ResponsiveEventDispatchTests : AntDesignTestBase
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Exclusive_trigger_listener_forwards_its_nested_control_filter(bool asynchronous)
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        using var listener = new DomEventListener(JSInterop.JSRuntime, new DomEventSubscriptionStore());
        if (asynchronous)
            listener.AddExclusive<int>("#trigger", "click", (Func<int, Task>)(_ => Task.CompletedTask), ignoreSelector: ".remove");
        else
            listener.AddExclusive<int>("#trigger", "click", (Action<int>)(_ => { }), ignoreSelector: ".remove");
        var registration = Assert.Single(JSInterop.Invocations.Where(call => call.Identifier == JSInteropConstants.AddDomEventListener));
        Assert.Equal(".remove", registration.Arguments[5]);
        Assert.Equal(false, registration.Arguments[2]);
        Assert.Equal(false, registration.Arguments[4]);
    }

    [Fact]
    public void Shared_resize_callbacks_may_replace_subscriptions_during_dispatch()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        using var listener = new DomEventListener(JSInterop.JSRuntime, new DomEventSubscriptionStore());
        var calls = new System.Collections.Generic.List<string>();
        Action<int> next = _ => calls.Add("new");
        Action<int> current = null;
        current = _ => {
            calls.Add("old");
            listener.RemoveShared("window", "resize", current);
            listener.AddShared("window", "resize", next);
        };
        listener.AddShared("window", "resize", current);
        var registration = JSInterop.Invocations.Single(call => call.Identifier == JSInteropConstants.AddDomEventListener);
        var reference = (DotNetObjectReference<Invoker<string>>)registration.Arguments[3];
        reference.Value.Invoke("390");
        Assert.Equal(new[] { "old" }, calls);
        reference.Value.Invoke("1440");
        Assert.Equal(new[] { "old", "new" }, calls);
    }

    [Fact]
    public async Task Async_resize_callbacks_may_replace_subscriptions_across_an_await()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        using var listener = new DomEventListener(JSInterop.JSRuntime, new DomEventSubscriptionStore());
        var calls = new System.Collections.Generic.List<string>();
        Func<int, Task> next = _ => { calls.Add("new"); return Task.CompletedTask; };
        Func<int, Task> current = null;
        current = async _ => {
            calls.Add("old");
            await Task.Yield();
            listener.RemoveShared("window", "resize", current);
            listener.AddShared("window", "resize", next);
        };
        listener.AddShared("window", "resize", current);
        var registration = JSInterop.Invocations.Single(call => call.Identifier == JSInteropConstants.AddDomEventListener);
        var reference = (DotNetObjectReference<AsyncInvoker<string>>)registration.Arguments[3];
        await reference.Value.Invoke("390");
        Assert.Equal(new[] { "old" }, calls);
        await reference.Value.Invoke("1440");
        Assert.Equal(new[] { "old", "new" }, calls);
    }
}
