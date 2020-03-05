using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntBlazor
{
    public abstract class AntComponentBase : ComponentBase, IAntComponentBase, IDisposable
    {
        [Parameter]
        public ForwardRef RefBack { get; set; }

        private readonly Queue<Func<Task>> afterRenderCallQuene = new Queue<Func<Task>>();

        protected void CallAfterRender(Func<Task> action)
        {
            afterRenderCallQuene.Enqueue(action);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await OnFirstAfterRenderAsync();
            }

            if (afterRenderCallQuene.Count > 0)
            {
                var actions = afterRenderCallQuene.ToArray();
                afterRenderCallQuene.Clear();

                foreach (var action in actions)
                {
                    if (Disposed)
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

        public virtual void Dispose()
        {
            Disposed = true;
        }

        protected bool Disposed { get; private set; }

        protected void InvokeStateHasChanged()
        {
            InvokeAsync(() =>
            {
                if (!Disposed)
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region Hack to fix https://github.com/aspnet/AspNetCore/issues/11159

        public static object CreateDotNetObjectRefSyncObj = new object();

        protected DotNetObjectReference<T> CreateDotNetObjectRef<T>(T value) where T : class
        {
            return DotNetObjectReference.Create(value);
        }

        protected void DisposeDotNetObjectRef<T>(DotNetObjectReference<T> value) where T : class
        {
            value?.Dispose();
        }

        #endregion Hack to fix https://github.com/aspnet/AspNetCore/issues/11159
    }
}