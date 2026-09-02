// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.JsInterop.ObservableApi;
using AntDesign.JsInterop;
using Bunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Xunit;

namespace AntDesign.Tests.Core.JsInterop;

public sealed class DomEventListenerCoverageTests : TestContext
{
    public DomEventListenerCoverageTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void AddExclusive_RegistersOnce_ForwardsOptionsAndInvokesCallback()
    {
        var listener = CreateListener();
        string? received = null;

        listener.AddExclusive<string>("#button", "click", value => received = value, true, true);
        listener.AddExclusive<string>("#button", "click", _ => { });

        var invocation = JSInterop.Invocations.Single(x => x.Identifier == JSInteropConstants.AddDomEventListener);
        invocation.Arguments.Take(3).Should().Equal("#button", "click", true);
        invocation.Arguments[4].Should().Be(true);

        var reference = invocation.Arguments[3].Should().BeOfType<DotNetObjectReference<Invoker<string>>>().Subject;
        reference.Value.Invoke("payload");

        received.Should().Be("payload");
    }

    [Fact]
    public async Task AddExclusive_AsyncCallbackIsAwaited_AndRemoveAllowsRegistrationAgain()
    {
        var listener = CreateListener();
        string? received = null;

        listener.AddExclusive<string>("document", "keydown", async value =>
        {
            await Task.Yield();
            received = value;
        });

        var addInvocation = JSInterop.Invocations.Single(x => x.Identifier == JSInteropConstants.AddDomEventListener);
        var reference = addInvocation.Arguments[3].Should().BeOfType<DotNetObjectReference<AsyncInvoker<string>>>().Subject;
        await reference.Value.Invoke("Enter");

        listener.RemoveExclusive("document", "keydown");
        listener.AddExclusive<string>("document", "keydown", _ => { });

        received.Should().Be("Enter");
        JSInterop.Invocations.Count(x => x.Identifier == JSInteropConstants.AddDomEventListener).Should().Be(2);
        var removeInvocation = JSInterop.Invocations.Single(x => x.Identifier == JSInteropConstants.RemoveDomEventListener);
        removeInvocation.Arguments.Take(2).Should().Equal("document", "keydown");
        removeInvocation.Arguments[2].Should().BeSameAs(reference);
    }

    [Fact]
    public void AddShared_RegistersOneJsListener_DispatchesCallbacksAndRemovesSubscriptions()
    {
        var store = new DomEventSubscriptionStore();
        var listener = CreateListener(store);
        int firstValue = 0;
        int secondValue = 0;
        Action<int> firstCallback = value => firstValue = value;
        Action<int> secondCallback = value => secondValue = value;

        listener.AddShared<int>("window", "resize", firstCallback, true);
        listener.AddShared<int>("window", "resize", secondCallback);

        var invocation = JSInterop.Invocations.Single(x => x.Identifier == JSInteropConstants.AddDomEventListener);
        invocation.Arguments.Take(3).Should().Equal("window", "resize", true);
        var reference = invocation.Arguments[3].Should().BeOfType<DotNetObjectReference<Invoker<string>>>().Subject;

        reference.Value.Invoke("42");
        firstValue.Should().Be(42);
        secondValue.Should().Be(42);

        listener.RemoveShared("window", "resize", firstCallback);
        reference.Value.Invoke("21");
        firstValue.Should().Be(42);
        secondValue.Should().Be(21);

        listener.DisposeShared();
        store.Should().BeEmpty();
        JSInterop.Invocations.Count(x => x.Identifier == JSInteropConstants.RemoveDomEventListener).Should().Be(1);
    }

    [Fact]
    public async Task AddShared_AsyncOwnerDispatchesAllCallbacks_AndDisposalRemovesJsListener()
    {
        var store = new DomEventSubscriptionStore();
        var listener = CreateListener(store);
        int syncValue = 0;
        int asyncValue = 0;
        Action<int> syncCallback = value => syncValue = value;
        Func<int, Task> asyncCallback = async value =>
        {
            await Task.Yield();
            asyncValue = value;
        };

        listener.AddShared<int>("#panel", "scroll", asyncCallback);
        listener.AddShared<int>("#panel", "scroll", syncCallback);

        var invocation = JSInterop.Invocations.Single(x => x.Identifier == JSInteropConstants.AddDomEventListener);
        var reference = invocation.Arguments[3].Should().BeOfType<DotNetObjectReference<AsyncInvoker<string>>>().Subject;
        await reference.Value.Invoke("7");

        syncValue.Should().Be(7);
        asyncValue.Should().Be(7);

        listener.RemoveShared("#panel", "scroll", syncCallback);
        listener.RemoveShared("#panel", "scroll", asyncCallback);
        listener.AddShared<int>("#panel", "scroll", syncCallback);
        listener.Dispose();
        listener.Dispose();

        store.Should().BeEmpty();
        JSInterop.Invocations.Count(x => x.Identifier == JSInteropConstants.RemoveDomEventListener).Should().Be(1);
    }

    [Fact]
    public void AddEventListenerToFirstChild_DeserializesTypedAndJsonCallbacks()
    {
        var listener = CreateListener();
        TestPayload? typedPayload = null;
        JsonElement jsonPayload = default;

        listener.AddEventListenerToFirstChild<TestPayload>("#host", "click", value => typedPayload = value, true);
        listener.AddEventListenerToFirstChild("#other", "change", value => jsonPayload = value);
        listener.AddEventListenerToFirstChild<TestPayload>("#host", "click", _ => { });

        var invocations = JSInterop.Invocations
            .Where(x => x.Identifier == JSInteropConstants.AddDomEventListenerToFirstChild)
            .ToArray();
        invocations.Should().HaveCount(2);
        invocations[0].Arguments.Take(3).Should().Equal("#host", "click", true);

        var typedReference = invocations[0].Arguments[3].Should().BeOfType<DotNetObjectReference<Invoker<string>>>().Subject;
        var jsonReference = invocations[1].Arguments[3].Should().BeOfType<DotNetObjectReference<Invoker<string>>>().Subject;
        typedReference.Value.Invoke("{\"Value\":9}");
        jsonReference.Value.Invoke("{\"name\":\"test\"}");

        typedPayload!.Value.Should().Be(9);
        jsonPayload.GetProperty("name").GetString().Should().Be("test");
    }

    [Fact]
    public async Task ResizeObserver_WhenSupported_RegistersDispatchesRemovesAndDisposes()
    {
        JSInterop.Setup<bool>(JSInteropConstants.ObserverConstants.Resize.IsResizeObserverSupported).SetResult(true);
        var store = new DomEventSubscriptionStore();
        var listener = CreateListener(store);
        var element = new ElementReference("observed");
        int syncCount = 0;
        int asyncCount = 0;
        Action<List<ResizeObserverEntry>> syncCallback = entries => syncCount = entries.Count;
        Func<List<ResizeObserverEntry>, Task> asyncCallback = async entries =>
        {
            await Task.Yield();
            asyncCount = entries.Count;
        };

        await listener.AddResizeObserver(element, asyncCallback);
        await listener.AddResizeObserver(element, syncCallback);

        var createInvocation = JSInterop.Invocations.Single(x => x.Identifier == JSInteropConstants.ObserverConstants.Resize.Create);
        var reference = createInvocation.Arguments[1]
            .Should().BeOfType<DotNetObjectReference<AsyncInvoker<string>>>().Subject;
        await reference.Value.Invoke("[{}]");

        syncCount.Should().Be(1);
        asyncCount.Should().Be(1);
        JSInterop.Invocations.Count(x => x.Identifier == JSInteropConstants.ObserverConstants.Resize.Observe).Should().Be(1);

        await listener.RemoveResizeObserver(element, syncCallback);
        await listener.RemoveResizeObserver(element, asyncCallback);
        await listener.AddResizeObserver(element, syncCallback);
        await listener.DisconnectResizeObserver(element);
        await listener.AddResizeObserver(element, syncCallback);
        await listener.DisposeResizeObserver(element);

        store.Should().BeEmpty();
        JSInterop.Invocations.Count(x => x.Identifier == JSInteropConstants.ObserverConstants.Resize.Disconnect).Should().Be(1);
        JSInterop.Invocations.Count(x => x.Identifier == JSInteropConstants.ObserverConstants.Resize.Dispose).Should().Be(1);
        JSInterop.Invocations.Count(x => x.Identifier == JSInteropConstants.ObserverConstants.Resize.IsResizeObserverSupported).Should().Be(1);
    }

    [Fact]
    public async Task ResizeObserver_WhenUnsupported_UsesSharedWindowResizeListener()
    {
        JSInterop.Setup<bool>(JSInteropConstants.ObserverConstants.Resize.IsResizeObserverSupported).SetResult(false);
        var listener = CreateListener();
        var element = new ElementReference("fallback");
        int syncCount = 0;
        int asyncCount = 0;

        await listener.AddResizeObserver(element, entries => syncCount = entries.Count);
        await listener.AddResizeObserver(element, entries =>
        {
            asyncCount = entries.Count;
            return Task.CompletedTask;
        });

        var addInvocation = JSInterop.Invocations.Single(x => x.Identifier == JSInteropConstants.AddDomEventListener);
        addInvocation.Arguments.Take(3).Should().Equal("window", "resize", false);
        var reference = addInvocation.Arguments[3].Should().BeOfType<DotNetObjectReference<Invoker<string>>>().Subject;
        reference.Value.Invoke("{}");

        syncCount.Should().Be(1);
        asyncCount.Should().Be(1);

        await listener.DisconnectResizeObserver(element);
        await listener.DisposeResizeObserver(element);
        listener.Dispose();

        JSInterop.Invocations.Should().NotContain(x => x.Identifier == JSInteropConstants.ObserverConstants.Resize.Disconnect);
        JSInterop.Invocations.Should().NotContain(x => x.Identifier == JSInteropConstants.ObserverConstants.Resize.Dispose);
        JSInterop.Invocations.Count(x => x.Identifier == JSInteropConstants.RemoveDomEventListener).Should().Be(1);
    }

    [Fact]
    public void DisposeExclusive_RemovesEveryRegisteredListenerOnlyOnce()
    {
        var listener = CreateListener();
        listener.AddExclusive<int>("#one", "click", _ => { });
        listener.AddExclusive<int>("#two", "focus", _ => { });

        listener.DisposeExclusive();
        listener.DisposeExclusive();

        var removals = JSInterop.Invocations
            .Where(x => x.Identifier == JSInteropConstants.RemoveDomEventListener)
            .ToArray();
        removals.Should().HaveCount(2);
        removals.Select(x => (x.Arguments[0], x.Arguments[1])).Should()
            .BeEquivalentTo(new[] { ((object)"#one", (object)"click"), ("#two", "focus") });
    }

    private DomEventListener CreateListener(DomEventSubscriptionStore? store = null)
    {
        return new DomEventListener(JSInterop.JSRuntime, store ?? new DomEventSubscriptionStore());
    }

    private sealed class TestPayload
    {
        public int Value { get; set; }
    }
}
