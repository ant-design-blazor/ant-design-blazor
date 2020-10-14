using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// the options of Modal dialog box
    /// </summary>
    public class ModalOptions
    {
        internal ModalRef ModalRef;

        internal static readonly RenderFragment DefaultCloseIcon = (builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "Type", "close");
            builder.AddAttribute(2, "Theme", "outline");
            builder.CloseComponent();
        };

        internal static readonly RenderFragment DefaultFooter = (builder) =>
        {
            builder.OpenComponent<ModalFooter>(0);
            builder.CloseComponent();
        };

        public Func<Task> AfterClose { get; set; }

        public string BodyStyle { get; set; }

        public OneOf<string, RenderFragment> CancelText { get; set; } = "Cancel";

        public bool Centered { get; set; }

        public bool Closable { get; set; } = true;

        public bool Draggable { get; set; }

        public bool DragInViewport { get; set; } = true;

        public RenderFragment CloseIcon { get; set; } = DefaultCloseIcon;

        public bool ConfirmLoading { get; set; }

        public bool DestroyOnClose { get; set; }

        public OneOf<string, RenderFragment>? Footer { get; set; } = DefaultFooter;

        public bool ForceRender { get; set; }

        public ElementReference? GetContainer { get; set; } = null;

        public bool Keyboard { get; set; } = true;

        public bool Mask { get; set; } = true;

        public bool MaskClosable { get; set; } = true;

        public string MaskStyle { get; set; }

        public OneOf<string, RenderFragment> OkText { get; set; } = "OK";

        public string OkType { get; set; } = ButtonType.Primary;

        public OneOf<string, RenderFragment> Title { get; set; } = string.Empty;

        public bool Visible { get; set; } = true;

        public OneOf<string, double> Width { get; set; } = 520;

        public string WrapClassName { get; set; }

        public int ZIndex { get; set; } = 1000;

        public Func<MouseEventArgs, Task> OnCancel { get; set; }

        internal Func<MouseEventArgs, Task> GetOnCancel()
        {
            if (OnCancel != null) return OnCancel;
            return async (e) =>
            {
                await (ModalRef?.CloseAsync() ?? Task.CompletedTask);
            };
        }

        public Func<MouseEventArgs, Task> OnOk { get; set; }

        internal Func<MouseEventArgs, Task> GetOnOk()
        {
            if (OnOk != null) return OnOk;
            return async (e) =>
            {
                await (ModalRef?.CloseAsync() ?? Task.CompletedTask);
            };
        }

        public ButtonProps OkButtonProps { get; set; }

        public ButtonProps CancelButtonProps { get; set; }

        public RenderFragment Content { get; set; }

        public bool Rtl { get; set; } = false;
    }
}
