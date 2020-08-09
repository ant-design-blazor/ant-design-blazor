using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class ModalRef<TResult> : ModalRef
    {
        public Func<TResult, Task> OnCancel { get; set; }

        public Func<TResult, Task> OnOk { get; set; }

        internal ModalRef(ConfirmOptions config, ModalService service) : base(config, service)
        {

        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <returns></returns>
        public async Task TriggerOkAsync(TResult result)
        {
            await _service?.CloseAsync(this);
            await OnOk?.Invoke(result);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <returns></returns>
        public async Task TriggerCancelAsync(TResult result)
        {
            await _service?.CloseAsync(this);
            await OnCancel?.Invoke(result);
        }
    }

    public class ModalRef
    {
        public ConfirmOptions Config { get; set; }
        public Drawer Drawer { get; set; }

        protected ModalService _service;

        internal IModalTemplate ModalTemplate { get; set; }

        public Func<Task> OnOpen { get; set; }

        public Func<Task> OnClose { get; set; }

        public Func<Task> OnDestroy { get; set; }

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
            await _service?.OpenAsync(this);
        }

        /// <summary>
        /// 关闭窗体无返回值
        /// </summary>
        /// <returns></returns>
        public async Task CloseAsync()
        {
            await _service?.CloseAsync(this);
        }

        public async Task UpdateConfig()
        {
            await _service?.Update(this);
        }

        public async Task UpdateConfig(ConfirmOptions config)
        {
            Config = config;
            await _service?.Update(this);
        }

        internal TaskCompletionSource<bool> TaskCompletionSource { get; set; }
    }
}
