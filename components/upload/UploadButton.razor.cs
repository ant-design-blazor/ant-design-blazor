using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign.Internal
{
    public partial class UploadButton : AntComponentBase
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
        public bool Disabled
        {
            get
            {
                return _disabled;
            }
            set
            {
                _disabledChanged = value != _disabled;
                _disabled = value;
            }
        }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private ElementReference _file;
        private ElementReference _btn;

        private DotNetObjectReference<UploadButton> _currentInstance;

        private string _fileId = Guid.NewGuid().ToString();

        private bool _beforeTheFirstRender;
        private bool _disabled;
        private bool _disabledChanged;

        private UploadInfo _uploadInfo = new UploadInfo();

        protected override bool ShouldRender() => Upload != null;

        protected override void OnInitialized()
        {
            _currentInstance = DotNetObjectReference.Create(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _beforeTheFirstRender = true;
            }

            if (firstRender && !Disabled)
            {
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.AddFileClickEventListener, _btn);
            }

            if (_beforeTheFirstRender && _disabledChanged)
            {
                _disabledChanged = false;
                if (Disabled)
                {
                    await JSRuntime.InvokeVoidAsync(JSInteropConstants.RemoveFileClickEventListener, _btn);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync(JSInteropConstants.AddFileClickEventListener, _btn);
                }
            }
            await base.OnAfterRenderAsync(firstRender);
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
            foreach (var fileItem in flist)
            {
                var fileName = fileItem.FileName;
                fileItem.Ext = fileItem.FileName.Substring(fileName.LastIndexOf('.'));
                var id = Guid.NewGuid().ToString();
                if (Upload.BeforeUpload != null)
                {
                    if (!Upload.BeforeUpload.Invoke(fileItem))
                    {
                        return;
                    }
                }
                fileItem.Percent = 0;
                fileItem.State = UploadState.Uploading;
                fileItem.Id = id;
                Upload.FileList.Add(fileItem);
                await Upload.FileListChanged.InvokeAsync(this.Upload.FileList);

                await InvokeAsync(StateHasChanged);
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.UploadFile, _file, index, Data, Headers, id, Action, Name, _currentInstance, "UploadChanged", "UploadSuccess", "UploadError");
                index++;
            }

            await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
        }

        [JSInvokable]
        public async Task UploadSuccess(string id, string returnData)
        {
            var file = Upload.FileList.FirstOrDefault(x => x.Id.Equals(id));
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
