using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Internal
{
    public partial class UploadButton : AntDomComponentBase
    {
        [CascadingParameter]
        public Upload Upload { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string ListType { get; set; }

        [Parameter]
        public bool ShowButton { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public bool Directory { get; set; }

        [Parameter]
        public bool Multiple { get; set; }

        [Parameter]
        public string Accept { get; set; }

        [Parameter]
        public Dictionary<string, object> Data { get; set; }

        [Parameter]
        public Dictionary<string, string> Headers { get; set; }

        [Parameter]
        public string Action { get; set; }

        [Parameter]
        public string Method { get; set; }

        [Parameter]
        public bool Disabled
        {
            get
            {
                return _disabled;
            }
            set
            {
                if (value != _disabled)
                {
                    _disabled = value;
                    _ = InvokeAsync(() => ToggleDisabled());
                }
            }
        }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private ElementReference _file;
        private ElementReference _btn;

        private DotNetObjectReference<UploadButton> _currentInstance;

        private string _fileId = Guid.NewGuid().ToString();

        private bool _disabled;

        private UploadInfo _uploadInfo = new UploadInfo();

        protected override bool ShouldRender() => Upload != null;

        protected override void OnInitialized()
        {
            _currentInstance = DotNetObjectReference.Create(this);
            var prefixCls = "ant-upload";

            ClassMapper
                .Add(prefixCls)
                .Get(() => $"{prefixCls}-select-{ListType}")
                .If($"{prefixCls}-drag", () => Upload?.Drag == true)
                .If($"{prefixCls}-drag-hover", () => Upload?._dragHover == true)
                .Add($"{prefixCls}-select")
                .If($"{prefixCls}-rtl", () => RTL);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !Disabled && Upload?.Drag == false && !string.IsNullOrWhiteSpace(Action))
            {
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.AddFileClickEventListener, _btn);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task ToggleDisabled()
        {
            if (Disabled)
            {
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.RemoveFileClickEventListener, _btn);
            }
            else
            {
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.AddFileClickEventListener, _btn);
            }
        }

        private async Task FileNameChanged(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            var flist = await JSRuntime.InvokeAsync<List<UploadFileItem>>(JSInteropConstants.GetFileInfo, _file);
            var index = 0;
            if (Upload.BeforeAllUploadAsync != null && !(await Upload.BeforeAllUploadAsync.Invoke(flist)))
            {
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
                return;
            }
            if (Upload.BeforeAllUpload != null && !Upload.BeforeAllUpload.Invoke(flist))
            {
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
                return;
            }

            foreach (var fileItem in flist)
            {
                var fileName = fileItem.FileName;
                fileItem.Ext = System.IO.Path.GetExtension(fileItem.FileName);
                var id = Guid.NewGuid().ToString();
                if (Upload.BeforeUpload != null && !Upload.BeforeUpload.Invoke(fileItem))
                {
                    await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
                    return;
                }
                fileItem.Percent = 0;
                fileItem.State = UploadState.Uploading;
                fileItem.Id = id;
                Upload.FileList.Add(fileItem);
                await Upload.FileListChanged.InvokeAsync(this.Upload.FileList);

                await InvokeAsync(StateHasChanged);
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.UploadFile, _file, index, Data, Headers, id, Action, Name, _currentInstance, "UploadChanged", "UploadSuccess", "UploadError", Method);
                index++;
            }

            await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
        }

        [JSInvokable]
        public async Task UploadSuccess(string id, string returnData)
        {
            var file = Upload.FileList.FirstOrDefault(x => x.Id.Equals(id));
            _uploadInfo.FileList = Upload.FileList;
            if (file == null)
            {
                return;
            }

            file.State = UploadState.Success;
            file.Percent = 100;
            file.Response = returnData;
            _uploadInfo.File = file;
            await UploadChanged(id, 100);
            await InvokeAsync(StateHasChanged);
            if (Upload.OnSingleCompleted.HasDelegate)
            {
                await Upload.OnSingleCompleted.InvokeAsync(_uploadInfo);
            }
            if (Upload.OnCompleted.HasDelegate && Upload.FileList.All(x => (x.State.Equals(UploadState.Success) || x.State.Equals(UploadState.Fail))))
            {
                await Upload.OnCompleted.InvokeAsync(_uploadInfo);
            }
        }

        [JSInvokable]
        public async Task UploadError(string id, string reponseCode)
        {
            var file = Upload.FileList.FirstOrDefault(x => x.Id.Equals(id));
            if (file == null)
            {
                return;
            }
            file.State = UploadState.Fail;
            file.Percent = 100;
            _uploadInfo.File = file;
            _uploadInfo.FileList = Upload.FileList;
            file.Response ??= reponseCode;
            await UploadChanged(id, 100);
            await InvokeAsync(StateHasChanged);
            if (Upload.OnSingleCompleted.HasDelegate)
            {
                await Upload.OnSingleCompleted.InvokeAsync(_uploadInfo);
            }
            if (Upload.OnCompleted.HasDelegate && Upload.FileList.All(x => (x.State.Equals(UploadState.Success) || x.State.Equals(UploadState.Fail))))
            {
                await Upload.OnCompleted.InvokeAsync(_uploadInfo);
            }
        }

        [JSInvokable]
        public async Task UploadChanged(string id, int progress)
        {
            var file = Upload.FileList.FirstOrDefault(x => x.Id.Equals(id));
            if (file == null)
            {
                return;
            }
            file.Percent = progress;
            _uploadInfo.File = file;
            _uploadInfo.FileList = Upload.FileList;
            await InvokeAsync(StateHasChanged);
            if (Upload.OnChange.HasDelegate)
            {
                await Upload.OnChange.InvokeAsync(_uploadInfo);
            }
        }

        protected override void Dispose(bool disposing)
        {
            InvokeAsync(async () => await JSRuntime.InvokeVoidAsync(JSInteropConstants.RemoveFileClickEventListener, _btn));

            base.Dispose(disposing);
        }
    }
}
