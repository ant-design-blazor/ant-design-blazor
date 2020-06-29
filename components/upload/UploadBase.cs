using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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
        public string Url { get; set; }

        [Parameter]
        public List<UploadFileItem> FileList { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public int MaxCount { get; set; } = 6;

        [Parameter]
        public string Text { get; set; } = "Click to upload";

        [Parameter]
        public string Icon { get; set; } = "upload";

        [Parameter]
        public EventCallback<UploadFileItem> OnSingleCompleted { get; set; }

        [Parameter]
        public EventCallback<List<UploadFileItem>> OnCompleted { get; set; }


        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private DotNetObjectReference<UploadBase> _currentInstance;


        public int Progress { get; set; }


        public ElementReference _file;

        protected override Task OnInitializedAsync()
        {
            _currentInstance = DotNetObjectReference.Create(this);
            FileList = FileList ?? new List<UploadFileItem>();
            return base.OnInitializedAsync();
        }

        public async Task UploadClick()
        {
            var result = await JSRuntime.InvokeAsync<bool>(JSInteropConstants.triggerEvent, _file, "MouseEvent", "click");
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
            fileItem.ResponseText = string.Empty;
            fileItem.State = UploadState.Uploading;
            fileItem.Id = id;
            FileList.Add(fileItem);
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync(JSInteropConstants.uploadFile, _file, id, Url, Name, _currentInstance, "UploadChanged", "UploadSuccess", "UploadError");
            await JSRuntime.InvokeVoidAsync(JSInteropConstants.clearFile, _file);
        }

        [JSInvokable]
        public async Task UploadSuccess( string id, string returnData)
        {
            var file = FileList.FirstOrDefault(x => x.Id.Equals(id));
            if (file == null)
            {
                return;
            }
            file.State = UploadState.Success;
            file.Progress = 100;
            file.ResponseText = returnData;
            if (OnSingleCompleted.HasDelegate)
            {
                await OnSingleCompleted.InvokeAsync(file);
            }
            if (OnCompleted.HasDelegate && !FileList.Any(x => !(x.State.Equals(UploadState.Success) || x.State.Equals(UploadState.Fail))))
            {
                await OnCompleted.InvokeAsync(FileList);
            }
            StateHasChanged();
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
            if (OnSingleCompleted.HasDelegate)
            {
                await OnSingleCompleted.InvokeAsync(file);
            }
            if (OnCompleted.HasDelegate && !FileList.Any(x => !(x.State.Equals(UploadState.Success) || x.State.Equals(UploadState.Fail))))
            {
                await OnCompleted.InvokeAsync(FileList);
            }
            StateHasChanged();
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
            Console.WriteLine(progress);
            StateHasChanged();
        }

    }
}
