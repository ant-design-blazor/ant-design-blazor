// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Threading.Tasks;

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

        internal DrawerRef(DrawerOptions options, DrawerService service)
        {
            Config = options;
            _service = service;
        }

        /// <summary>
        /// close Confirm dialog
        /// </summary>
        /// <returns></returns>
        public override async Task CloseAsync()
        {
            var e = new ModalClosingEventArgs();
            await (OnClosing?.Invoke(e) ?? Task.CompletedTask);
            if (!e.Cancel)
            {
                await _service.CloseAsync(this);
                if (OnClose != null)//before close
                    await OnClose.Invoke();
            }
        }


        /// <summary>
        /// Open Confirm dialog
        /// </summary>
        /// <returns></returns>
        public override async Task OpenAsync()
        {
            await _service.OpenAsync(this);
        }

        /// <summary>
        /// update Confirm dialog config which Visible=true
        /// </summary>
        /// <returns></returns>
        public override async Task UpdateConfigAsync()
        {
            await (_service?.UpdateAsync(this) ?? Task.CompletedTask);
        }

        /// <summary>
        /// update Confirm dialog config with a new ConfirmOptions
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
        public async Task CloseAsync(TResult result)
        {
            var closeEventArgs = new ModalClosingEventArgs();

            if (OnClosing != null)//before close
                await OnClosing.Invoke(closeEventArgs);

            if (closeEventArgs.Cancel)
                return;

            await _service.CloseAsync(this);

            if (OnClosed != null)//after close
                await OnClosed.Invoke(result);

            TaskCompletionSource?.SetResult(result);
        }
    }
}
