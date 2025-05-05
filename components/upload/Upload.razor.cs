// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Upload file by selecting or dragging.</para>

    <h2>When To Use</h2>

    <para>Uploading is the process of publishing information (web pages, text, pictures, video, etc.) to a remote server via a web page or upload tool.</para>

    <list type="bullet">
        <item>When you need to upload one or more files.</item>
        <item>When you need to show the process of uploading.</item>
        <item>When you need to upload files by dragging and dropping.</item>
    </list>
    </summary>
    <seealso cref="UploadInfo"/>
    <seealso cref="UploadFileItem"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/QaeBt_ZMg/Upload.svg", Title = "Upload", SubTitle = "上传")]
    public partial class Upload : AntDomComponentBase
    {
        /// <summary>
        /// Function which will be executed before uploading each file. 
        /// Uploading will be stopped if false is returned.
        /// Warning：This function is not supported in IE9.
        /// </summary>
        [Parameter]
        public Func<UploadFileItem, bool> BeforeUpload { get; set; }

        /// <summary>
        /// Hook function which will be executed before starting the overall upload.
        /// Uploading will be stopped if false is returned.
        /// Warning：This function is not supported in IE9.
        /// </summary>
        [Parameter]
        public Func<List<UploadFileItem>, Task<bool>> BeforeAllUploadAsync { get; set; }

        /// <summary>
        /// Hook function which will be executed before starting the overall upload.
        /// Uploading will be stopped if false is returned.
        /// Warning：This function is not supported in IE9.
        /// </summary>
        [Parameter]
        public Func<List<UploadFileItem>, bool> BeforeAllUpload { get; set; }

        /// <summary>
        /// The name of uploading file
        /// </summary>
        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// Uploading URL
        /// </summary>
        [Parameter]
        public string Action { get; set; }

        /// <summary>
        /// Disable upload button
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Additional data to be uploaded with the files
        /// </summary>
        [Parameter]
        public Dictionary<string, object> Data { get; set; }

        /// <summary>
        /// Built-in stylesheets, support for three types: <c>Text</c>, <c>Picture</c> or <c>PictureCard</c>
        /// </summary>
        /// <default value="UploadListType.Text" />
        [Parameter]
        public UploadListType ListType { get; set; } = UploadListType.Text;

        /// <summary>
        /// Support upload whole directory.
        /// See <see href="https://caniuse.com/input-file-directory">Can I Use</see> for browser support
        /// </summary>
        [Parameter]
        public bool Directory { get; set; }

        /// <summary>
        /// Whether to support selected multiple file. IE10+ supported. 
        /// When set to true, will allow selecting multiple files with holding down Ctrl key
        /// </summary>
        [Parameter]
        public bool Multiple { get; set; }

        /// <summary>
        /// File types that can be accepted. 
        /// See <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/file#accept">input's accept attribute</see>
        /// </summary>
        [Parameter]
        public string Accept { get; set; }

        /// <summary>
        /// Show the list of files uploading or not
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool ShowUploadList { get; set; } = true;

        /// <summary>
        /// List of files that have been uploaded (controlled)
        /// </summary>
        [Parameter]
        public bool ShowDownloadIcon { get; set; } = true;

        /// <summary>
        /// Show the preview icon or not
        /// </summary>
        [Parameter]
        public bool ShowPreviewIcon { get; set; } = true;

        /// <summary>
        /// Show the remove icon or not
        /// </summary>
        [Parameter]
        public bool ShowRemoveIcon { get; set; } = true;

        /// <summary>
        /// Get or set the list of files that have been uploaded
        /// </summary>
        [Parameter]
        public List<UploadFileItem> FileList { get; set; } = new List<UploadFileItem>();

        /// <summary>
        /// Locale for UI text
        /// </summary>
        [Parameter]
        public UploadLocale Locale { get; set; } = LocaleProvider.CurrentLocale.Upload;

        /// <summary>
        /// Event callback executed when the files list changes
        /// </summary>
        [Parameter]
        public EventCallback<List<UploadFileItem>> FileListChanged { get; set; }

        /// <summary>
        /// Default list of files that have been uploaded.
        /// </summary>
        [Parameter]
        public List<UploadFileItem> DefaultFileList { get; set; } = new List<UploadFileItem>();

        /// <summary>
        /// Set request headers, valid above IE10.
        /// </summary>
        [Parameter]
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// A callback function, will be executed when a single file upload has succeeded or failed
        /// </summary>
        [Parameter]
        public EventCallback<UploadInfo> OnSingleCompleted { get; set; }

        /// <summary>
        /// A callback function, will be executed when all uploads have succeeded or failed
        /// </summary>
        [Parameter]
        public EventCallback<UploadInfo> OnCompleted { get; set; }

        /// <summary>
        /// A callback function, can be executed when uploading state has changed for any reason. 
        /// The function will be called when uploading is in progress, completed or failed.
        /// </summary>
        [Parameter]
        public EventCallback<UploadInfo> OnChange { get; set; }

        /// <summary>
        /// A callback function executed when the remove file button is clicked.
        /// When true is returned, file will be removed and <see cref="FileListChanged"/> will be executed.
        /// When false is returned, the removal will be cancelled.
        /// </summary>
        [Parameter]
        public Func<UploadFileItem, Task<bool>> OnRemove { get; set; }

        /// <summary>
        /// A callback function, will be executed when file link or preview icon is clicked
        /// </summary>
        [Parameter]
        public EventCallback<UploadFileItem> OnPreview { get; set; }

        /// <summary>
        /// Callback executed when file name link is clicked in upload list.
        /// </summary>
        [Parameter]
        public EventCallback<UploadFileItem> OnDownload { get; set; }

        /// <summary>
        /// Content of the upload button
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether to show the upload button or not.
        /// </summary>
        /// <defaut value="true"/>
        [Parameter]
        public bool ShowButton { get; set; } = true;

        /// <summary>
        /// Whether to allow drag and drop of files or not.
        /// </summary>
        [Parameter]
        public bool Drag { get; set; }

        /// <summary>
        /// HTTP method of upload request
        /// </summary>
        /// <default value="post"/>
        [Parameter]
        public OneOf<HttpMethod, string> Method { get; set; } = HttpMethod.Post;

        /// <summary>
        /// Whether to upload multiple files in a single request (only for multiple file upload)
        /// </summary>
        [Parameter]
        public bool BatchUpload { get; set; }

        /// <summary>
        /// Whether to send cookies when making upload requests
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool WithCredentials { get; set; }

        private bool IsText => ListType == UploadListType.Text;

        private bool IsPictureCard => ListType == UploadListType.PictureCard;

        private readonly ClassMapper _listClassMapper = new();

        internal bool _dragHover;

        private static readonly Dictionary<UploadListType, string> _typeMap = new()
        {
            [UploadListType.Text] = "text",
            [UploadListType.Picture] = "picture",
            [UploadListType.PictureCard] = "picture-card",
        };

        protected override void OnInitialized()
        {
            base.OnInitialized();

            const string PrefixCls = "ant-upload";

            ClassMapper
                .GetIf(() => $"{PrefixCls}-picture-card-wrapper", () => IsPictureCard)
                .GetIf(() => $"{PrefixCls}-no-btn", () => ChildContent == null);

            _listClassMapper
                .Add($"{PrefixCls}-list")
                .Get(() => $"{PrefixCls}-list-{_typeMap[ListType]}")
                .If($"{PrefixCls}-list-rtl", () => RTL);

            FileList.InsertRange(0, DefaultFileList);
        }

        private void HandleDrag(DragEventArgs args)
        {
            _dragHover = args.Type == "dragover";
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

        private async Task DownloadFile(UploadFileItem item)
        {
            if (item.State == UploadState.Success && OnDownload.HasDelegate)
            {
                await OnDownload.InvokeAsync(item);
            }
        }
    }
}
