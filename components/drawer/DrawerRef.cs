using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class DrawerRef<TResult> : IDrawerRef
    {
        public DrawerOptions Options { get; set; }

        public Drawer Drawer { get; set; }

        public Func<Task> OnOpen { get; set; }

        public Func<DrawerClosingEventArgs, Task> OnClosing { get; set; }

        public Func<TResult, Task> OnClosed { get; set; }

        private DrawerService _service;

        internal DrawerRef(DrawerOptions options)
        {
            Options = options;
        }

        internal DrawerRef(DrawerOptions options, DrawerService service)
        {
            Options = options;
            _service = service;
        }

        /// <summary>
        /// open a drawer
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            await _service.OpenAsync(this);
            if (OnOpen != null)
                await OnOpen.Invoke();
        }

        /// <summary>
        /// close the drawer without return value
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            await CloseAsync(default);
        }

        /// <summary>
        /// 关闭抽屉
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync(TResult result)
        {
            var closeEventArgs = new DrawerClosingEventArgs();

            if (OnClosing != null)//before close
                await OnClosing.Invoke(closeEventArgs);

            if (closeEventArgs.Rejected)
                return;

            await _service.CloseAsync(this);

            if (OnClosed != null)//after close
                await OnClosed.Invoke(result);

            if (TaskCompletionSource != null)//dialog close
                TaskCompletionSource.SetResult(result);
        }

        internal TaskCompletionSource<TResult> TaskCompletionSource { get; set; }
    }
}
