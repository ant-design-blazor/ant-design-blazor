using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public class DialogOptions
    {
        internal const string PrefixCls = "ant-modal";

        public Func<Task> OnClosed { get; set; }

        public string BodyStyle { get; set; }

        public OneOf<string, RenderFragment> CancelText { get; set; } = "Cancel";

        public bool Centered { get; set; }

        public bool Closable { get; set; } = true;

        public bool Draggable { get; set; }

        public bool DragInViewport { get; set; } = true;

        internal string GetHeaderStyle()
        {
            if (Draggable)
            {
                return "cursor: move;";
            }
            return "";
        }

        private static readonly RenderFragment _defaultCloseIcon = (builder) =>
        {
            builder.OpenComponent<Icon>(0);
            builder.AddAttribute(1, "Type", "close");
            builder.AddAttribute(2, "Theme", "outline");
            builder.CloseComponent();
        };

        public RenderFragment? CloseIcon { get; set; } = _defaultCloseIcon;

        public bool ConfirmLoading { get; set; }

        public bool DestroyOnClose { get; set; }

        private static readonly RenderFragment _defaultFooter = (builder) =>
        {
            builder.OpenComponent<ModalFooter>(0);
            builder.CloseComponent();
        };

        public OneOf<string, RenderFragment>? Footer { get; set; } = _defaultFooter;

        public bool ForceRender { get; set; }

        public ElementReference? GetContainer { get; set; } = null;

        public bool Keyboard { get; set; } = true;
        public bool Mask { get; set; } = true;
        public bool MaskClosable { get; set; } = true;

        public string MaskStyle { get; set; }

        public OneOf<string, RenderFragment> OkText { get; set; } = "OK";

        public string OkType { get; set; } = ButtonType.Primary;

        public OneOf<string, RenderFragment>? Title { get; set; } = null;

        public OneOf<string, double> Width { get; set; } = 520;

        internal string GetWidth()
        {
            if (Width.IsT0)
            {
                return $"width:{Width.AsT0};";
            }
            else
            {
                return $"width:{Width.AsT1}px;";
            }
        }

        public string WrapClassName { get; set; }

        public int ZIndex { get; set; } = 1000;

        public Func<MouseEventArgs, Task> OnCancel { get; set; }

        public Func<MouseEventArgs, Task> OnOk { get; set; }

        public ButtonProps OkButtonProps { get; set; }

        public ButtonProps CancelButtonProps { get; set; }

        public RenderFragment ChildContent { get; set; }

        public bool Rtl { get; set; } = false;

        internal string GetWrapClassNameExtended()
        {
            var classNameArray = new List<string>();
            if (Centered)
            {
                classNameArray.Add($"{PrefixCls}-centered");
            }

            if (Rtl)
            {
                classNameArray.Add($"{PrefixCls}-wrap-rtl");
            }

            return string.Join(' ', classNameArray);
        }

        public string TransitionName { get; set; }
        public string MaskTransitionName { get; set; }

        public string ClassName { get; set; }
    }
}
