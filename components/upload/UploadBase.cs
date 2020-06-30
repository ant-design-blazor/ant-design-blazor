using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    public partial class UploadBase : AntDomComponentBase
    {
        public delegate bool BeforeUploadDelegate(UploadFileItem file);

        [Parameter]
        public BeforeUploadDelegate BeforeUpload { get; set; }

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

        private DotNetObjectReference<UploadBase> _currentInstance;

        UploadInfo _uploadInfo = new UploadInfo();


        public int Progress { get; set; }


        public ElementReference _file;

        protected override Task OnInitializedAsync()
        {
            _currentInstance = DotNetObjectReference.Create(this);
            _uploadInfo.FileList = FileList;
            FileList.InsertRange(0, DefaultFileList);
            return base.OnInitializedAsync();
        }

        public async Task UploadClick()
        {
            if (!Disabled)
                _ = await JSRuntime.InvokeAsync<bool>(JSInteropConstants.triggerEvent, _file, "MouseEvent", "click");
        }

        public async Task FileNameChanged(ChangeEventArgs e)
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
            if (OnCompleted.HasDelegate && !FileList.Any(x => !(x.State.Equals(UploadState.Success) || x.State.Equals(UploadState.Fail))))
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
            file.Response = file.Response ?? "error";
            await InvokeAsync(StateHasChanged);
            if (OnSingleCompleted.HasDelegate)
            {
                await OnSingleCompleted.InvokeAsync(_uploadInfo);
            }
            if (OnCompleted.HasDelegate && !FileList.Any(x => !(x.State.Equals(UploadState.Success) || x.State.Equals(UploadState.Fail))))
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
