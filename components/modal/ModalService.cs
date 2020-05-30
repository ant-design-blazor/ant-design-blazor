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

        public void Confirm(ConfirmOptions props)
        {
            OnOpenConfirm?.Invoke(props);
        }

        public void Info(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.Icon = (builder) =>
            {
                builder.OpenComponent<AntIcon>(0);
                builder.AddAttribute(1, "Type", "info-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };
            options.OkCancel = false;
            options.OkText = "OK";
            options.ConfirmType = "info";
            Confirm(options);
        }

        public void Success(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.Icon = (builder) =>
            {
                builder.OpenComponent<AntIcon>(0);
                builder.AddAttribute(1, "Type", "check-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };
            options.OkCancel = false;
            options.OkText = "OK";
            options.ConfirmType = "success";
            Confirm(options);
        }

        public void Error(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.Icon = (builder) =>
            {
                builder.OpenComponent<AntIcon>(0);
                builder.AddAttribute(1, "Type", "close-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };
            options.OkCancel = false;
            options.OkText = "OK";
            options.ConfirmType = "error";
            Confirm(options);
        }

        public void Warning(ConfirmOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            options.Icon = (builder) =>
            {
                builder.OpenComponent<AntIcon>(0);
                builder.AddAttribute(1, "Type", "exclamation-circle");
                builder.AddAttribute(2, "Theme", "outline");
                builder.CloseComponent();
            };
            options.OkCancel = false;
            options.OkText = "OK";
            options.ConfirmType = "warning";
            Confirm(options);
        }

        public void Update(ConfirmOptions options)
        {
            OnUpdate?.Invoke(options);
        }

        public void Destroy(ConfirmOptions options)
        {
            OnDestroy?.Invoke(options);
        }

        public void DestroyAll()
        {
            OnDestroyAll?.Invoke();
        }
    }
}
