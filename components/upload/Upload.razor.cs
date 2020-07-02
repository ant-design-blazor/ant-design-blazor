using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class Upload : AntDomComponentBase
    {
        [Parameter]
        public Func<UploadFileItem, bool> BeforeUpload { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public string Action { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string Accept { get; set; }

        [Parameter]
        public List<UploadFileItem> FileList { get; set; } = new List<UploadFileItem>();

        [Parameter]
        public List<UploadFileItem> DefaultFileList { get; set; } = new List<UploadFileItem>();

        [Parameter]
        public Dictionary<string, string> Headers { get; set; }

        [Parameter]
        public EventCallback<UploadInfo> OnSingleCompleted { get; set; }

        [Parameter]
        public EventCallback<UploadInfo> OnCompleted { get; set; }

        [Parameter]
        public EventCallback<UploadInfo> OnChange { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private DotNetObjectReference<Upload> _currentInstance;

        private UploadInfo _uploadInfo = new UploadInfo();

        public int Progress { get; set; }

        private ElementReference _file;

        private ElementReference _btn;

        private string _fileId = Guid.NewGuid().ToString();

        protected override Task OnInitializedAsync()
        {
            _currentInstance = DotNetObjectReference.Create(this);
            _uploadInfo.FileList = FileList;
            FileList.InsertRange(0, DefaultFileList);
            return base.OnInitializedAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if(Disabled)
            {
                JSRuntime.InvokeVoidAsync(JSInteropConstants.removeFileClickEventListener, _btn);
            }
            else
            {
                JSRuntime.InvokeVoidAsync(JSInteropConstants.addFileClickEventListener, _btn);
            }
            
            return base.OnAfterRenderAsync(firstRender);
        }

        private async Task FileNameChanged(ChangeEventArgs e)
        {
            var value = e.Value?.ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            var fileName = value.Substring(value.LastIndexOf('\\') + 1);
            var id = Guid.NewGuid().ToString();
            var fileItem = await JSRuntime.InvokeAsync<UploadFileItem>(JSInteropConstants.getFileInfo, _file);

            fileItem.Ext = fileName.Substring(fileName.LastIndexOf('.'));
            if (BeforeUpload != null)
            {
                if (!BeforeUpload.Invoke(fileItem))
                {
                    return;
                }
            }
            fileItem.Progress = 0;
            fileItem.State = UploadState.Uploading;
            fileItem.Id = id;
            FileList.Add(fileItem);
            await InvokeAsync(StateHasChanged);
            await JSRuntime.InvokeVoidAsync(JSInteropConstants.uploadFile, _file, Headers, id, Action, Name, _currentInstance, "UploadChanged", "UploadSuccess", "UploadError");
            await JSRuntime.InvokeVoidAsync(JSInteropConstants.clearFile, _file);
        }

        [JSInvokable]
        public async Task UploadSuccess(string id, string returnData)
        {
            var file = FileList.FirstOrDefault(x => x.Id.Equals(id));
            if (file == null)
            {
                return;
            }

            file.State = UploadState.Success;
            file.Progress = 100;
            file.Response = returnData;
            _uploadInfo.File = file;
            await InvokeAsync(StateHasChanged);
            if (OnSingleCompleted.HasDelegate)
            {
                await OnSingleCompleted.InvokeAsync(_uploadInfo);
            }
            if (OnCompleted.HasDelegate && FileList.All(x => (x.State.Equals(UploadState.Success) || x.State.Equals(UploadState.Fail))))
            {
                await OnCompleted.InvokeAsync(_uploadInfo);
            }
        }

        [JSInvokable]
        public async Task UploadError(string id, string reponseCode)
        {
            var file = FileList.FirstOrDefault(x => x.Id.Equals(id));
            if (file == null)
            {
                return;
            }
            file.State = UploadState.Fail;
            file.Progress = 100;
            _uploadInfo.File = file;
            file.Response ??= "error";
            await InvokeAsync(StateHasChanged);
            if (OnSingleCompleted.HasDelegate)
            {
                await OnSingleCompleted.InvokeAsync(_uploadInfo);
            }
            if (OnCompleted.HasDelegate && FileList.All(x => (x.State.Equals(UploadState.Success) || x.State.Equals(UploadState.Fail))))
            {
                await OnCompleted.InvokeAsync(_uploadInfo);
            }
        }

        [JSInvokable]
        public async Task UploadChanged(string id, int progress)
        {
            var file = FileList.FirstOrDefault(x => x.Id.Equals(id));
            if (file == null)
            {
                return;
            }
            file.Progress = progress;
            await InvokeAsync(StateHasChanged);
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(_uploadInfo);
            }
        }
    }
}
