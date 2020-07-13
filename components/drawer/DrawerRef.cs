using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class DrawerRef<TResult> : DrawerRef
    {
        public new Func<TResult, Task> OnClose { get; set; }

        internal DrawerRef(DrawerOptions config, DrawerService service) : base(config, service)
        {

        }

        /// <summary>
        /// 关闭抽屉
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync(TResult result)
        {
            await _service.CloseAsync(this);
            await OnClose.Invoke(result);
        }

    }

    public class DrawerRef
    {
        public DrawerOptions Config { get; set; }
        public Drawer Drawer { get; set; }

        protected DrawerService _service;

        public Func<Task> OnOpen { get; set; }

        public Func<Task> OnClose { get; set; }

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
        /// 打开抽屉
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            await _service.OpenAsync(this);
            if (OnOpen != null)
                await OnOpen.Invoke();
        }

        /// <summary>
        /// 关闭抽屉无返回值
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            await _service.CloseAsync(this);
            if (OnClose != null)
                await OnClose.Invoke();
        }


        internal async Task HandleOnCancel()
        {
            var args = new DrawerClosingEventArgs(false);
            if (Config.OnCancel != null)
            {
                await Config.OnCancel.Invoke(args);
            }
            if (args.Cancel == false)
                await _service.CloseAsync(this);
        }
    }
}
