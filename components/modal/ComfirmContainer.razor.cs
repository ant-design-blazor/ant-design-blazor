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
            ModalService.OnOpenEvent += Modal_OnOpen;
            ModalService.OnCloseEvent += Modal_OnClose;
            ModalService.OnDestroyEvent += Modal_OnDestroy;
            ModalService.OnDestroyAllEvent += Modal_OnDestroyAll;
            ModalService.OnUpdateEvent += Modal_OnUpdate;
        }

        /// <summary>
        /// 创建并打开窗体
        /// </summary>
        private async Task Modal_OnOpen(ModalRef modalRef)
        {
            modalRef.Config.Visible = true;
            if (!_modalRefs.Contains(modalRef))
            {
                _modalRefs.Add(modalRef);
            }
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private async Task Modal_OnClose(ModalRef modalRef)
        {
            modalRef.Config.Visible = false;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(300);
            if (_modalRefs.Contains(modalRef))
            {
                _modalRefs.Remove(modalRef);
            }
        }

        //private async Task OnRemove(ConfirmOptions obj)
        //{
        //    await Task.Delay(250);
        //    _confirms.Remove(obj);
        //    await InvokeAsync(StateHasChanged);
        //}


        private async Task Modal_OnUpdate(ModalRef modalRef)
        {
            if (modalRef.Config.Visible)
            {
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task Modal_OnDestroy(ModalRef modalRef)
        {
            modalRef.Config.Visible = false;
            await InvokeAsync(StateHasChanged);
            await Task.Delay(300);
            if (_modalRefs.Contains(modalRef))
            {
                _modalRefs.Remove(modalRef);
            }
        }

        private async Task Modal_OnDestroyAll()
        {
            foreach (var modalRef in _modalRefs)
            {
                modalRef.Config.Visible = false;
            }
            await InvokeAsync(StateHasChanged);
            await Task.Delay(300);
            _modalRefs.Clear();
        }


        protected override void Dispose(bool disposing)
        {
            ModalService.OnOpenEvent -= Modal_OnOpen;
            ModalService.OnCloseEvent -= Modal_OnClose;
            ModalService.OnDestroyEvent -= Modal_OnDestroy;
            ModalService.OnDestroyAllEvent -= Modal_OnDestroyAll;
            ModalService.OnUpdateEvent -= Modal_OnUpdate;

      
            base.Dispose(disposing);
        }
    }
}
