using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ModalService
    {
        internal event Func<ConfirmOptions, Task> OnOpenConfirm;
        internal event Func<ConfirmOptions, Task> OnUpdate;
        internal event Func<ConfirmOptions, Task> OnDestroy;
        internal event Func<Task> OnDestroyAll;

        #region SimpleConfirm

        public Task Confirm(ConfirmOptions props)
        {
            return OnOpenConfirm?.Invoke(props);
        }

        public Task Info(ConfirmOptions options)
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
            options.OkText = "OK";
            options.ConfirmType = "info";
            return Confirm(options);
        }

        public Task Success(ConfirmOptions options)
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
            options.OkText = "OK";
            options.ConfirmType = "success";
            return Confirm(options);
        }

        public Task Error(ConfirmOptions options)
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
            options.OkText = "OK";
            options.ConfirmType = "error";
            return Confirm(options);
        }

        public Task Warning(ConfirmOptions options)
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
            options.OkText = "OK";
            options.ConfirmType = "warning";
            return Confirm(options);
        }

        #endregion

        public Task Update(ConfirmOptions options)
        {
            return OnUpdate?.Invoke(options);
        }

        public Task Destroy(ConfirmOptions options)
        {
            return OnDestroy?.Invoke(options);
        }

        public Task DestroyAll()
        {
            return OnDestroyAll?.Invoke();
        }


        internal event Func<ModalRef, Task> OnOpenEvent;

        internal event Func<ModalRef, Task> OnCloseEvent;

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
        public async Task<ModalRef<TResult>> CreateAsync<TComponent, TContentParams, TResult>(ConfirmOptions config, TContentParams contentParams) where TComponent : ModalTemplate<TContentParams, TResult>
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
