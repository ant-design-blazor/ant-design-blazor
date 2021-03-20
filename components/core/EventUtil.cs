// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// avoid component re-rendering caused by events to Blazor components.(pure event handlers)
    /// <para>
    ///     author: SteveSandersonMS, from <see cref="https://gist.github.com/SteveSandersonMS/8a19d8e992f127bb2d2a315ec6c5a373"/>.
    /// </para>
    /// <para>
    ///     issue: <seealso cref="https://github.com/dotnet/aspnetcore/issues/18919#issuecomment-735969441"/>.
    /// </para>
    /// </summary>
    public static class EventUtil
    {
        // The repetition in here is because of the four combinations of handlers (sync/async * with/without arg)
        public static Action AsNonRenderingEventHandler(Action callback)
            => new SyncReceiver(callback).Invoke;
        public static Action<TValue> AsNonRenderingEventHandler<TValue>(Action<TValue> callback)
            => new SyncReceiver<TValue>(callback).Invoke;
        public static Func<Task> AsNonRenderingEventHandler(Func<Task> callback)
            => new AsyncReceiver(callback).Invoke;
        public static Func<TValue, Task> AsNonRenderingEventHandler<TValue>(Func<TValue, Task> callback)
            => new AsyncReceiver<TValue>(callback).Invoke;

        // By implementing IHandleEvent, we can override the event handling logic on a per-handler basis
        // The logic here just calls the callback without triggering any re-rendering
        class ReceiverBase : IHandleEvent
        {
            public Task HandleEventAsync(EventCallbackWorkItem item, object arg) => item.InvokeAsync(arg);
        }


        class SyncReceiver : ReceiverBase
        {
            private Action callback;

            public SyncReceiver(Action callback)
            {
                this.callback = callback;
            }

            public void Invoke() => callback();
        }

        class SyncReceiver<T> : ReceiverBase
        {
            private Action<T> callback;

            public SyncReceiver(Action<T> callback)
            {
                this.callback = callback;
            }

            public void Invoke(T arg) => callback(arg);
        }

        class AsyncReceiver : ReceiverBase
        {
            private Func<Task> callback;

            public AsyncReceiver(Func<Task> callback)
            {
                this.callback = callback;
            }

            public Task Invoke() => callback();
        }

        class AsyncReceiver<T>: ReceiverBase
        {
            private Func<T, Task> callback;

            public AsyncReceiver(Func<T, Task> callback)
            {
                this.callback = callback;
            }

            public Task Invoke(T arg) => callback(arg);
        }
    }

}
