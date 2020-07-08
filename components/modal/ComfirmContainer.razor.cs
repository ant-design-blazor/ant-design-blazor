using System;
using System.Collections.Generic;
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

        private List<ModalRef> _modalRefs = new List<ModalRef>();

        protected override void OnInitialized()
        {
            ModalService.OnOpenConfirm += Modal_OnOpenConfirm;
            ModalService.OnDestroy += Modal_OnDestroy;
            ModalService.OnDestroyAll += Modal_OnDestroyAll;
            ModalService.OnUpdate += Modal_OnUpdate;


            ModalService.OnCloseEvent += ModalService_OnClose;
            ModalService.OnOpenEvent += ModalService_OnCreate;
        }

        private List<ConfirmOptions> _confirms = new List<ConfirmOptions>();

        private async Task OnRemove(ConfirmOptions obj)
        {
            await Task.Delay(250);
            _confirms.Remove(obj);
            await InvokeAsync(StateHasChanged);
        }

        private async Task Modal_OnOpenConfirm(ConfirmOptions obj)
        {
            _confirms.Add(obj);
            obj.Visible = true;
            await InvokeAsync(StateHasChanged);
        }

        private async Task Modal_OnUpdate(ConfirmOptions props)
        {
            if (props.Visible)
            {
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task Modal_OnDestroy(ConfirmOptions props)
        {
            props.Visible = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task Modal_OnDestroyAll()
        {
            foreach (var confirm in _confirms)
            {
                confirm.Visible = false;
            }
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 创建并打开窗体
        /// </summary>
        private async Task ModalService_OnCreate(ModalRef drawerRef)
        {
            drawerRef.Config.Visible = true;
            if (!_modalRefs.Contains(drawerRef))
            {
                _modalRefs.Add(drawerRef);
            }
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private async Task ModalService_OnClose(ModalRef drawerRef)
        {
            drawerRef.Config.Visible = false;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(300);
            if (_modalRefs.Contains(drawerRef))
            {
                _modalRefs.Remove(drawerRef);
            }
        }

        protected override void Dispose(bool disposing)
        {
            ModalService.OnOpenConfirm -= Modal_OnOpenConfirm;
            ModalService.OnDestroy -= OnRemove;
            ModalService.OnDestroyAll -= Modal_OnDestroyAll;
            ModalService.OnUpdate -= Modal_OnUpdate;

            ModalService.OnCloseEvent -= ModalService_OnClose;
            ModalService.OnOpenEvent -= ModalService_OnCreate;
            base.Dispose(disposing);
        }
    }
}
