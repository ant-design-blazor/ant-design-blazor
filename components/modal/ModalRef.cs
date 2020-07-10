using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class ModalRef<TResult> : ModalRef
    {
        public new Func<TResult, Task> OnClose { get; set; }

        internal ModalRef(ConfirmOptions config, ModalService service) : base(config, service)
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

    public class ModalRef
    {
        public ConfirmOptions Config { get; set; }
        public Drawer Drawer { get; set; }

        protected ModalService _service;

        public Func<Task> OnOpen { get; set; }

        public Func<Task> OnClose { get; set; }

        internal ModalRef(ConfirmOptions config)
        {
            Config = config;
        }

        internal ModalRef(ConfirmOptions config, ModalService service)
        {
            Config = config;
            _service = service;
        }

        /// <summary>
        /// 打开窗体
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            await _service.OpenAsync(this);
            if (OnOpen != null)
                await OnOpen.Invoke();
        }

        /// <summary>
        /// 关闭窗体无返回值
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            await _service.CloseAsync(this);
            if (OnClose != null)
                await OnClose.Invoke();
        }

        public async Task UpdateConfig()
        {
            await _service.Update(this);
        }


        public async Task UpdateConfig(ConfirmOptions config)
        {
            Config = config;
            await _service.Update(this);
        }


        internal async Task HandleOnCancel()
        {
            //bool isClose = true;
            //if (Config.OnCancel != null)
            //{
            //    isClose = Config.OnCancel.Invoke() ?? true;
            //}
            //if (isClose == true)
            //    await _service.CloseAsync(this);

        }
    }
}
