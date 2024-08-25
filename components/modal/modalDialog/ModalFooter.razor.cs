using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// modal footer Component
    /// </summary>
    public partial class ModalFooter
    {
        public static RenderFragment DefaultOkFooter = builder => {
            builder.OpenComponent<ModalOkFooter>(0);
            builder.CloseComponent();
        };

        public static RenderFragment DefaultCancelFooter = builder => {
            builder.OpenComponent<ModalCancelFooter>(0);
            builder.CloseComponent();
        };

        /// <summary>
        /// 
        /// </summary>
        [CascadingParameter]
        public DialogOptions ModalProps { get; set; }

        private async Task HandleCancel(MouseEventArgs e)
        {
            var onCancel = ModalProps.OnCancel;
            if (onCancel != null)
            {
                await onCancel.Invoke(e);
            }
        }

        private async Task HandleOk(MouseEventArgs e)
        {
            var onOk = ModalProps.OnOk;
            if (onOk != null)
            {
                await onOk.Invoke(e);
            }
        }
    }
}
