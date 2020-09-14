using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{



    public class DrawerRef<TResult> : IDrawerRef
    {
        public DrawerOptions Config { get; set; }
        public Drawer Drawer { get; set; }

        private DrawerService _service;

        public Func<Task> OnOpen { get; set; }

        public Func<DrawerClosingEventArgs, Task> OnClosing { get; set; }

        public Func<TResult, Task> OnClosed { get; set; }

        internal DrawerRef(DrawerOptions config)
        {
            Config = config;
        }

        internal DrawerRef(DrawerOptions config, DrawerService service)
        {
            Config = config;
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
            if (closeEventArgs.Cancel == true) return;
            await _service.CloseAsync(this);
            if (OnClosed != null)//after close 
                await OnClosed.Invoke(result);
            if (TaskCompletionSource != null)//dialog close
                TaskCompletionSource.SetResult(result);
        }

        internal TaskCompletionSource<TResult> TaskCompletionSource { get; set; }
    }
}
