// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class DrawerRef : FeedbackRefBase
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please replace it with Config")]
        public DrawerOptions Options => Config;

        public DrawerOptions Config { get; private set; }

        public Drawer Drawer { get; set; }

        public Func<ModalClosingEventArgs, Task> OnClosing { get; set; }

        protected readonly DrawerService _service;

        private bool _closing;
        private bool _closed;
        private bool _canceling;

        internal DrawerRef(DrawerOptions options, DrawerService service)
        {
            Config = options;
            _service = service;
        }

        /// <summary>
        /// close drawer
        /// </summary>
        /// <returns></returns>
        public override Task CloseAsync()
        {
            return CloseAsyncCore(null);
        }

        /// <summary>
        /// Close the drawer after checking cancellation, then notify completion.
        /// </summary>
        /// <param name="onClosed">The callback invoked after the drawer closes.</param>
        protected async Task CloseAsyncCore(Func<Task> onClosed)
        {
            if (_closing || _closed)
                return;

            _closing = true;
            try
            {
                var e = new ModalClosingEventArgs();
                await (OnClosing?.Invoke(e) ?? Task.CompletedTask);
                if (e.Cancel)
                    return;

                await _service.CloseAsync(this);
                _closed = true;
            }
            finally
            {
                _closing = false;
            }

            // Notifications may reopen the drawer and start another close operation.
            if (OnClose != null)
                await OnClose.Invoke();
            if (onClosed != null)
                await onClosed.Invoke();
        }

        internal async Task CancelAsync(MouseEventArgs mouseEvent)
        {
            if (_canceling || _closing || _closed)
                return;

            var args = new ModalClosingEventArgs(mouseEvent, false);
            _canceling = true;
            try
            {
                var template = ((IFeedbackRef)this).ModalTemplate;
                if (template != null)
                    await template.OnFeedbackCancelAsync(args);
            }
            finally
            {
                _canceling = false;
            }

            if (!args.Cancel)
                await CloseAsync();
        }


        /// <summary>
        /// Open drawer
        /// </summary>
        /// <returns></returns>
        public override async Task OpenAsync()
        {
            _closed = false;
            await _service.OpenAsync(this);
        }

        /// <summary>
        /// update drawer config which Visible=true
        /// </summary>
        /// <returns></returns>
        public override async Task UpdateConfigAsync()
        {
            await (_service?.UpdateAsync(this) ?? Task.CompletedTask);
        }

        /// <summary>
        /// update drawer config with a new options
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task UpdateConfigAsync(DrawerOptions config)
        {
            Config = config;
            await UpdateConfigAsync();
        }
    }

    public class DrawerRef<TResult> : DrawerRef
    {
        internal TaskCompletionSource<TResult> TaskCompletionSource { get; set; }

        public Func<TResult, Task> OnClosed { get; set; }

        internal DrawerRef(DrawerOptions options, DrawerService service) : base(options, service)
        {
        }

        /// <summary>
        /// 关闭抽屉
        /// </summary>
        /// <returns></returns>
        public override Task CloseAsync()
        {
            return CloseAsync(default);
        }

        /// <summary>
        /// Close the drawer and return the result after checking cancellation.
        /// </summary>
        /// <param name="result">The result returned by the drawer.</param>
        public Task CloseAsync(TResult result)
        {
            return CloseAsyncCore(async () =>
            {
                if (OnClosed != null)
                    await OnClosed.Invoke(result);

                TaskCompletionSource?.TrySetResult(result);
            });
        }
    }
}
