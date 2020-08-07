using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public class ConfirmOptions
    {
        public string ClassName { get; set; }

        public bool Visible { get; set; }

        public OneOf<string, RenderFragment>? Title { get; set; } = null;

        public OneOf<string, RenderFragment> Content { get; set; }

        public Func<ModalClosingEventArgs, Task> OnCancel { get; set; }

        public Func<ModalClosingEventArgs, Task> OnOk { get; set; }

        public ButtonProps OkButtonProps { get; set; } = new ButtonProps() {Type = ButtonType.Primary};

        public ButtonProps CancelButtonProps { get; set; } = new ButtonProps();

        public OneOf<string, double> Width { get; set; } = 520;

        public bool Centered { get; set; }

        public bool Mask { get; set; } = true;

        public bool MaskClosable { get; set; } = false;

        public string MaskStyle { get; set; }

        public OneOf<string, RenderFragment> OkText { get; set; } = "OK";

        public string OkType { get; set; } = ButtonType.Primary;

        public OneOf<string, RenderFragment> CancelText { get; set; } = "Cancel";

        public RenderFragment? Icon { get; set; } = null;

        public int ZIndex { get; set; } = 1000;

        /// <summary>
        /// 显示Cancel按钮
        /// </summary>
        public bool OkCancel { get; set; } = true;

        public string Style { get; set; }

        public bool Keyboard { get; set; } = true;

        public ConfirmAutoFocusButton AutoFocusButton { get; set; }

        public string TransitionName { get; set; }

        public string MaskTransitionName { get; set; }

        public ElementReference? GetContainer { get; set; } = null;

        internal string ConfirmType { get; set; } = "confirm";
    }
}
