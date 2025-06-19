// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;

namespace AntDesign.Internal
{
    public partial class UploadButton : AntDomComponentBase
    {
        [CascadingParameter]
        public Upload Upload { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public UploadListType ListType { get; set; }

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
        public OneOf<HttpMethod, string> Method { get; set; }

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

        private UploadInfo _uploadInfo = new();

        protected override bool ShouldRender() => Upload != null;

        private string MethodString => Method.IsT0 ? Method.AsT0.ToString().ToLowerInvariant() : Method.AsT1.ToLowerInvariant();

        private static readonly Dictionary<UploadListType, string> _typeMap = new()
        {
            [UploadListType.Text] = "text",
            [UploadListType.Picture] = "picture",
            [UploadListType.PictureCard] = "picture-card",
        };

        protected override void OnInitialized()
        {
            _currentInstance = DotNetObjectReference.Create(this);
            var prefixCls = "ant-upload";

            ClassMapper
                .Add(prefixCls)
                .Get(() => $"{prefixCls}-select-{_typeMap[ListType]}")
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
            if (string.IsNullOrWhiteSpace(e.Value?.ToString()))
            {
                return;
            }

            var flist = await JSRuntime.InvokeAsync<List<UploadFileItem>>(JSInteropConstants.GetFileInfo, _file);
            if (!await ValidateFiles(flist))
            {
                await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
                return;
            }

            // Generate IDs for all files first
            var fileIds = flist.Select(_ => Guid.NewGuid().ToString()).ToList();

            // Initialize file items
            for (var i = 0; i < flist.Count; i++)
            {
                var fileItem = flist[i];
                InitializeFileItem(fileItem, fileIds[i]);
            }

            await Upload.FileListChanged.InvokeAsync(Upload.FileList);
            await InvokeAsync(StateHasChanged);

            if (Upload.BatchUpload)
            {
                await UploadFilesBatch(fileIds);
            }
            else
            {
                await UploadFilesSequentially(fileIds);
            }

            await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
        }

        private async Task<bool> ValidateFiles(List<UploadFileItem> files)
        {
            if (Upload.BeforeAllUploadAsync != null && !await Upload.BeforeAllUploadAsync.Invoke(files))
            {
                return false;
            }
            if (Upload.BeforeAllUpload != null && !Upload.BeforeAllUpload.Invoke(files))
            {
                return false;
            }
            return true;
        }

        private void InitializeFileItem(UploadFileItem fileItem, string id)
        {
            fileItem.Ext = System.IO.Path.GetExtension(fileItem.FileName);
            fileItem.Percent = 0;
            fileItem.State = UploadState.Uploading;
            fileItem.Id = id;

            var existingFile = Upload.FileList.FirstOrDefault(f => f.FileName == fileItem.FileName);
            if (existingFile != null)
            {
                // Update existing file properties
                existingFile.Ext = fileItem.Ext;
                existingFile.Percent = fileItem.Percent;
                existingFile.State = fileItem.State;
                existingFile.Id = fileItem.Id;
            }
            else
            {
                Upload.FileList.Add(fileItem);
            }
        }

        private async Task UploadFilesBatch(List<string> fileIds)
        {
            await JSRuntime.InvokeVoidAsync(JSInteropConstants.UploadFile, new
            {
                element = _file,
                index = -1,
                data = Data,
                headers = Headers,
                fileIds = fileIds,
                url = Action,
                name = Name,
                instance = _currentInstance,
                method = MethodString,
                withCredentials = Upload.WithCredentials
            });
        }

        private async Task UploadFilesSequentially(List<string> fileIds)
        {
            for (var i = 0; i < fileIds.Count; i++)
            {
                var fileItem = Upload.FileList.First(f => f.Id == fileIds[i]);
                if (Upload.BeforeUpload != null && !Upload.BeforeUpload.Invoke(fileItem))
                {
                    await JSRuntime.InvokeVoidAsync(JSInteropConstants.ClearFile, _file);
                    return;
                }

                await JSRuntime.InvokeVoidAsync(JSInteropConstants.UploadFile, new
                {
                    element = _file,
                    index = i,
                    data = Data,
                    headers = Headers,
                    fileIds = new[] { fileIds[i] },
                    url = Action,
                    name = Name,
                    instance = _currentInstance,
                    method = MethodString,
                    withCredentials = Upload.WithCredentials
                });
            }
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
        public async Task UploadError(string id, string errorMessage)
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
            file.Response = errorMessage;

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
