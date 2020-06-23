using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    public class ModalService
    {
        internal event Func<ConfirmOptions, Task> OnOpenConfirm;
        internal event Func<ConfirmOptions, Task> OnUpdate;
        internal event Func<ConfirmOptions, Task> OnDestroy;
        internal event Func<Task> OnDestroyAll;

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
    }
}
