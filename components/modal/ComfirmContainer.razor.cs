using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class ComfirmContainer
    {
        [Inject]
        private ModalService ModalService { get; set; }

        [Inject]
        private ConfirmService ConfirmService { get; set; }

        private List<ConfirmRef> _modalRefs = new List<ConfirmRef>();

        protected override void OnInitialized()
        {
            ModalService.OnOpenEvent += Modal_OnOpen;
            ModalService.OnCloseEvent += Modal_OnClose;
            ModalService.OnDestroyEvent += Modal_OnDestroy;
            ModalService.OnDestroyAllEvent += Modal_OnDestroyAll;
            ModalService.OnUpdateEvent += Modal_OnUpdate;

            ConfirmService.OnOpenEvent += Modal_OnOpen;
        }

        /// <summary>
        /// 创建并打开窗体
        /// </summary>
        private async Task Modal_OnOpen(ConfirmRef confirmRef)
        {
            confirmRef.Config.Visible = true;
            if (!_modalRefs.Contains(confirmRef))
            {
                confirmRef.Config.BuildButtonsDefaultOptions();
                _modalRefs.Add(confirmRef);
            }
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private async Task Modal_OnClose(ConfirmRef confirmRef)
        {
            confirmRef.Config.Visible = false;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(250);
            if (_modalRefs.Contains(confirmRef))
            {
                _modalRefs.Remove(confirmRef);
                await JsInvokeAsync(JSInteropConstants.EnableBodyScroll);
            }
            confirmRef.OnClose?.Invoke();
        }

        private async Task Modal_OnUpdate(ConfirmRef confirmRef)
        {
            if (confirmRef.Config.Visible)
            {
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task Modal_OnDestroy(ConfirmRef confirmRef)
        {
            confirmRef.Config.Visible = false;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(250);
            if (_modalRefs.Contains(confirmRef))
            {
                _modalRefs.Remove(confirmRef);
            }
            confirmRef.OnDestroy?.Invoke();
        }

        private async Task Modal_OnDestroyAll()
        {
            foreach (var modalRef in _modalRefs)
            {
                modalRef.Config.Visible = false;
                modalRef.OnDestroy?.Invoke();
            }
            await InvokeAsync(StateHasChanged);
            await Task.Delay(250);
            _modalRefs.Clear();
        }


        protected override void Dispose(bool disposing)
        {
            ModalService.OnOpenEvent -= Modal_OnOpen;
            ModalService.OnCloseEvent -= Modal_OnClose;
            ModalService.OnDestroyEvent -= Modal_OnDestroy;
            ModalService.OnDestroyAllEvent -= Modal_OnDestroyAll;
            ModalService.OnUpdateEvent -= Modal_OnUpdate;

            ConfirmService.OnOpenEvent -= Modal_OnOpen;

            base.Dispose(disposing);
        }
    }
}
