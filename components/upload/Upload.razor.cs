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
        private bool _disabled;

        private bool _disabledChanged;

        [Parameter]
        public Func<UploadFileItem, bool> BeforeUpload { get; set; }

        [Parameter]
        public string Name { get; set; }

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

        [Parameter]
        public Dictionary<string, object> Data { get; set; }

        [Parameter]
        public string ListType { get; set; } = "text";

        [Parameter]
        public bool Directory { get; set; }

        [Parameter]
        public bool Multiple { get; set; }

        [Parameter]
        public string Accept { get; set; }

        [Parameter]
        public bool ShowUploadList { get; set; } = true;

        [Parameter]
        public List<UploadFileItem> FileList { get; set; } = new List<UploadFileItem>();

        [Parameter]
        public UploadLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Upload;

        [Parameter]
        public EventCallback<List<UploadFileItem>> FileListChanged { get; set; }

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
        public Func<UploadFileItem, Task<bool>> OnRemove { get; set; }

        [Parameter]
        public EventCallback<UploadFileItem> OnPreview { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool ShowButton { get; set; } = true;

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private bool IsText => ListType == "text";
        private bool IsPicture => ListType == "picture";
        private bool IsPictureCard => ListType == "picture-card";

        private DotNetObjectReference<Upload> _currentInstance;

        private UploadInfo _uploadInfo = new UploadInfo();

        public int Progress { get; set; }

        private ElementReference _file;

        private ElementReference _btn;

        private string _fileId = Guid.NewGuid().ToString();

        private bool _beforeTheFirstRender;

        protected override Task OnInitializedAsync()
        {
            _currentInstance = DotNetObjectReference.Create(this);
            _uploadInfo.FileList = FileList;
            FileList.InsertRange(0, DefaultFileList);
            return base.OnInitializedAsync();
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
                if (BeforeUpload != null)
                {
                    if (!BeforeUpload.Invoke(fileItem))
                    {
                        return;
                    }
                }
                fileItem.Percent = 0;
                fileItem.State = UploadState.Uploading;
                fileItem.Id = id;
                FileList.Add(fileItem);
                await this.FileListChanged.InvokeAsync(this.FileList);

                await InvokeAsync(StateHasChanged);
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.UploadFile, _file, index, Data, Headers, id, Action, Name, _currentInstance, "UploadChanged", "UploadSuccess", "UploadError");
                index++;
            }

            await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
        }

        private async Task RemoveFile(UploadFileItem item)
        {
            var canRemove = OnRemove == null || await OnRemove.Invoke(item);
            if (canRemove)
            {
                this.FileList.Remove(item);
                await this.FileListChanged.InvokeAsync(this.FileList);

                StateHasChanged();
            }
        }

        private async Task PreviewFile(UploadFileItem item)
        {
            if (item.State == UploadState.Success && OnPreview.HasDelegate)
            {
                await OnPreview.InvokeAsync(item);
            }
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
            file.Percent = 100;
            file.Response = returnData;
            _uploadInfo.File = file;
            await UploadChanged(id, 100);
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
            file.Percent = 100;
            _uploadInfo.File = file;
            file.Response ??= reponseCode;
            await UploadChanged(id, 100);
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
            file.Percent = progress;
            _uploadInfo.File = file;
            await InvokeAsync(StateHasChanged);
            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(_uploadInfo);
            }
        }

        protected override void Dispose(bool disposing)
        {
            InvokeAsync(async () => await JSRuntime.InvokeVoidAsync(JSInteropConstants.RemoveFileClickEventListener, _btn));

            base.Dispose(disposing);
        }
    }
}
