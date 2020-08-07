using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ModalService
    {
        internal event Func<ModalRef, Task> OnOpenEvent;
        internal event Func<ModalRef, Task> OnCloseEvent;
        internal event Func<ModalRef, Task> OnUpdateEvent;
        internal event Func<ModalRef, Task> OnDestroyEvent;
        internal event Func<Task> OnDestroyAllEvent;

        #region SimpleConfirm

        public async Task<ModalRef> Confirm(ConfirmOptions props)
        {
            ModalRef modalRef = new ModalRef(props, this);
            await OnOpenEvent?.Invoke(modalRef);
            return modalRef;
        }

        public async Task<ModalRef> Info(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Icon = (builder) =>
            {
                builder.OpenComponent<Icon>(0);
                builder.AddAttribute(1, "Type", "info-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };
            options.OkCancel = false;
            options.ConfirmType = "info";
            return await Confirm(options);
        }

        public async Task<ModalRef> Success(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Icon = (builder) =>
            {
                builder.OpenComponent<Icon>(0);
                builder.AddAttribute(1, "Type", "check-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };
            options.OkCancel = false;
            options.ConfirmType = "success";
            return await Confirm(options);
        }

        public async Task<ModalRef> Error(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Icon = (builder) =>
            {
                builder.OpenComponent<Icon>(0);
                builder.AddAttribute(1, "Type", "close-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };
            options.OkCancel = false;
            options.ConfirmType = "error";
            return await Confirm(options);
        }

        public async Task<ModalRef> Warning(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.Icon = (builder) =>
            {
                builder.OpenComponent<Icon>(0);
                builder.AddAttribute(1, "Type", "exclamation-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };
            options.OkCancel = false;
            options.ConfirmType = "warning";
            return await Confirm(options);
        }

        #endregion

        public Task Update(ModalRef modalRef)
        {
            return OnUpdateEvent?.Invoke(modalRef);
        }

        public Task Destroy(ModalRef modalRef)
        {
            return OnDestroyEvent?.Invoke(modalRef);
        }

        public Task DestroyAll()
        {
            return OnDestroyAllEvent?.Invoke();
        }


        /// <summary>
        /// 创建并打开一个简单窗口
        /// </summary>
        /// <param name="config">抽屉参数</param>
        /// <returns></returns>
        public async Task<ModalRef> CreateAsync(ConfirmOptions config)
        {
            CheckIsNull(config);
            ModalRef modalRef = new ModalRef(config, this);
            await OnOpenEvent.Invoke(modalRef);
            return modalRef;
        }

        /// <summary>
        /// 创建并打开一个模板窗口
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <typeparam name="TContentParams"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="config"></param>
        /// <param name="contentParams"></param>
        /// <returns></returns>
        public async Task<ModalRef<TResult>> CreateAsync<TComponent, TContentParams, TResult>(ConfirmOptions config,
            TContentParams contentParams) where TComponent : ModalTemplate<TContentParams, TResult>
        {
            CheckIsNull(config);

            ModalRef<TResult> modalRef = new ModalRef<TResult>(config, this);
            await OnOpenEvent.Invoke(modalRef);

            RenderFragment child = (builder) =>
            {
                builder.OpenComponent<TComponent>(0);
                builder.AddAttribute(1, "ModalRef", modalRef);
                builder.AddAttribute(2, "Config", contentParams);
                builder.CloseComponent();
            };
            config.Content = child;

            return modalRef;
        }

        internal Task OpenAsync(ModalRef modalRef)
        {
            if (OnOpenEvent != null)
            {
                return OnOpenEvent.Invoke(modalRef);
            }

            return Task.CompletedTask;
        }


        internal Task CloseAsync(ModalRef modalRef)
        {
            if (OnCloseEvent != null)
            {
                return OnCloseEvent.Invoke(modalRef);
            }

            return Task.CompletedTask;
        }

        private static void CheckIsNull(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
        }
    }
}
