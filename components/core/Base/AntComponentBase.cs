// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public abstract class AntComponentBase : ComponentBase, IDisposable
    {
        /// <summary>
        /// A <see cref="ForwardRef" /> instance. You can get a reference to the internal DOM by using <see cref="ForwardRef.Current" />.
        /// </summary>
        [Parameter]
        public ForwardRef RefBack { get; set; } = new ForwardRef();

        private readonly Queue<Func<Task>> _afterRenderCallQuene = new Queue<Func<Task>>();

        protected void CallAfterRender(Func<Task> action)
        {
            _afterRenderCallQuene.Enqueue(action);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await OnFirstAfterRenderAsync();
            }

            if (_afterRenderCallQuene.Count > 0)
            {
                var actions = _afterRenderCallQuene.ToArray();
                _afterRenderCallQuene.Clear();

                foreach (var action in actions)
                {
                    if (IsDisposed)
                    {
                        return;
                    }

                    await action();
                }
            }
        }

        protected virtual Task OnFirstAfterRenderAsync()
        {
            return Task.CompletedTask;
        }

        protected AntComponentBase()
        {
        }

        protected void InvokeStateHasChanged()
        {
            InvokeAsync(() =>
            {
                if (!IsDisposed)
                {
                    StateHasChanged();
                }
            });
        }

        protected async Task InvokeStateHasChangedAsync()
        {
            await InvokeAsync(() =>
            {
                if (!IsDisposed)
                {
                    StateHasChanged();
                }
            });
        }

        [Inject]
        protected IJSRuntime Js { get; set; }

        protected async Task<T> JsInvokeAsync<T>(string code, params object[] args)
        {
            try
            {
                return await Js.InvokeAsync<T>(code, args);
            }
            catch
            {
                throw;
            }
        }

        protected async Task JsInvokeAsync(string code, params object[] args)
        {
            try
            {
                await Js.InvokeVoidAsync(code, args);
            }
            catch
            {
                // Do not write to console
                throw;
            }
        }


        /// <summary>
        /// Standard Focus. From Net5 uses Blazor extension method on ElementReference.
        /// Before, uses JS implemented exactly the same as Net5 JS.
        /// </summary>
        /// <param name="target">Element that will receive focus.</param>
        /// <param name="preventScroll">Whether to scroll to focused element</param>
        protected async Task FocusAsync(ElementReference target, bool preventScroll = false)
            => await Js.FocusAsync(target, preventScroll);

        /// <summary>
        /// Focus with behaviors. Behavior will work only for elements that are
        /// HTMLInputElement or HTMLTextAreaElement. Otherwise will only focus.
        /// </summary>
        /// <param name="target">Element that will receive focus.</param>
        /// <param name="behavior">Behavior of focused element</param>
        /// <param name="preventScroll">Whether to scroll to focused element</param>
        protected async Task FocusAsync(ElementReference target, FocusBehavior behavior, bool preventScroll = false)
            => await Js.FocusAsync(target, behavior, preventScroll);

        /// <summary>
        /// Standard Blur. Uses JS interop.
        /// </summary>
        /// <param name="target">Element that will receive focus.</param>
        protected async Task BlurAsync(ElementReference target)
            => await Js.BlurAsyc(target);

        protected bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AntComponentBase()
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }
    }
}
