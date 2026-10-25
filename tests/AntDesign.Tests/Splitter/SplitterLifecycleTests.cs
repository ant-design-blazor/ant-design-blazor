using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bunit;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Splitter;

public sealed class SplitterLifecycleTests : AntDesignTestBase
{
    [Fact]
    public async Task Responsive_unmount_keeps_the_callback_reference_alive_until_attach_finishes()
    {
        var runtime = new DelayedRuntime();
        Services.AddSingleton<IJSRuntime>(runtime);
        var cut = RenderComponent<AntDesign.Splitter>(parameters => parameters.AddChildContent(builder => {
            builder.OpenComponent<SplitterPanel>(0); builder.CloseComponent();
            builder.OpenComponent<SplitterPanel>(1); builder.CloseComponent();
        }));
        var reference = runtime.Reference;
        cut.Instance.Dispose();
        Assert.NotNull(reference.Value);
        await reference.Value.InitializeSizes(new[] { "50px", "50px" });
        var handler = new Handler();
        runtime.Attach.SetResult(handler);
        await handler.Disposed.Task.WaitAsync(TimeSpan.FromSeconds(5));
        Assert.True(handler.CleanedUp);
    }

    private sealed class DelayedRuntime : IJSRuntime
    {
        public DotNetObjectReference<AntDesign.Splitter> Reference { get; private set; }
        public TaskCompletionSource<IJSObjectReference> Attach { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        {
            if (identifier == JSInteropConstants.SplitterHelper.Attach)
            {
                Reference = (DotNetObjectReference<AntDesign.Splitter>)args[0];
                return new ValueTask<TValue>(CompleteAsync<TValue>());
            }
            return new ValueTask<TValue>(default(TValue));
        }
        private async Task<TValue> CompleteAsync<TValue>() => (TValue)(object)await Attach.Task;
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args) => InvokeAsync<TValue>(identifier, args);
    }

    private sealed class Handler : IJSObjectReference
    {
        public bool CleanedUp { get; private set; }
        public TaskCompletionSource<bool> Disposed { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args)
        { if (identifier == "dispose") CleanedUp = true; return new ValueTask<TValue>(default(TValue)); }
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args) => InvokeAsync<TValue>(identifier, args);
        public ValueTask DisposeAsync() { Disposed.TrySetResult(true); return default; }
    }
}
