// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Tests.Upload
{
    public class UploadDuplicateTests : AntDesignTestBase
    {
        [Fact]
        public async Task Max_count_one_replaces_the_existing_file()
        {
            JSInterop.SetupVoid(JSInteropConstants.AddFileClickEventListener, _ => true);
            JSInterop.SetupVoid(JSInteropConstants.ClearFile, _ => true);
            var existingFile = new UploadFileItem { FileName = "old.png", State = UploadState.Success };
            var fileList = new List<UploadFileItem> { existingFile };
            var component = Context.RenderComponent<AntDesign.Upload>(parameters => parameters
                .Add(p => p.Action, "https://example.com/upload")
                .Add(p => p.MaxCount, 1)
                .Add(p => p.Defer, true)
                .Add(p => p.FileList, fileList)
                .Add(p => p.ChildContent, "upload"));
            JSInterop.Setup<List<UploadFileItem>>(JSInteropConstants.GetFileInfo, _ => true)
                .SetResult(new List<UploadFileItem> { new() { FileName = "new.png" } });

            await component.Find("input").ChangeAsync(new ChangeEventArgs { Value = "ignored" });

            component.Instance.FileList.Should().ContainSingle().Which.FileName.Should().Be("new.png");
        }

        [Fact]
        public async Task Max_count_accepts_only_the_remaining_capacity()
        {
            JSInterop.SetupVoid(JSInteropConstants.AddFileClickEventListener, _ => true);
            JSInterop.SetupVoid(JSInteropConstants.ClearFile, _ => true);
            var fileList = new List<UploadFileItem> { new() { FileName = "existing.png" } };
            var component = Context.RenderComponent<AntDesign.Upload>(parameters => parameters
                .Add(p => p.Action, "https://example.com/upload")
                .Add(p => p.MaxCount, 3)
                .Add(p => p.Defer, true)
                .Add(p => p.FileList, fileList)
                .Add(p => p.ChildContent, "upload"));
            JSInterop.Setup<List<UploadFileItem>>(JSInteropConstants.GetFileInfo, _ => true)
                .SetResult(new List<UploadFileItem>
                {
                    new() { FileName = "first.png" },
                    new() { FileName = "second.png" },
                    new() { FileName = "third.png" }
                });

            await component.Find("input").ChangeAsync(new ChangeEventArgs { Value = "ignored" });

            component.Instance.FileList.Should().HaveCount(3);
            component.Instance.FileList.Select(file => file.FileName).Should().Contain("first.png").And.Contain("second.png").And.NotContain("third.png");
        }

        [Fact]
        public void Custom_progress_options_are_applied_to_uploading_files()
        {
            var component = Context.RenderComponent<AntDesign.Upload>(parameters => parameters
                .Add(p => p.DefaultFileList, new List<UploadFileItem>
                {
                    new() { FileName = "report.zip", Percent = 68, State = UploadState.Uploading }
                })
                .Add(p => p.ProgressStrokeColor, new Dictionary<string, string>
                {
                    ["0%"] = "#108ee9",
                    ["100%"] = "#87d068"
                })
                .Add(p => p.ProgressStrokeWidth, 3)
                .Add(p => p.ProgressFormat, percent => $"{percent:0.00}%")
                .Add(p => p.ProgressShowInfo, true));

            component.Find(".ant-progress-bg").GetAttribute("style").Should().Contain("linear-gradient");
            component.Find(".ant-progress-bg").GetAttribute("style").Should().Contain("height: 3px");
            component.Find(".ant-progress-text").TextContent.Should().Be("68.00%");
        }

        [Fact]
        public async Task Transform_file_callback_receives_bytes_and_can_replace_file_metadata()
        {
            JSInterop.SetupVoid(JSInteropConstants.AddFileClickEventListener, _ => true);
            var originalBytes = new byte[] { 1, 2, 3 };
            var transformedBytes = new byte[] { 9, 8, 7 };
            UploadFileTransformContext received = null;
            string receivedFileName = null;
            var component = Context.RenderComponent<AntDesign.Upload>(parameters => parameters
                .Add(p => p.Action, "https://example.com/upload")
                .Add(p => p.FileList, new List<UploadFileItem>
                {
                    new() { Id = "file-1", FileName = "photo.png", Type = "image/png" }
                })
                .Add(p => p.TransformFile, context =>
                {
                    received = context;
                    receivedFileName = context.File.FileName;
                    return Task.FromResult(new UploadFileTransformResult
                    {
                        Content = transformedBytes,
                        FileName = "watermarked.png",
                        ContentType = "image/png"
                    });
                })
                .Add(p => p.ChildContent, "upload"));

            var uploadButton = component.FindComponent<AntDesign.Internal.UploadButton>().Instance;
            var result = await uploadButton.TransformFileContent("file-1", System.Convert.ToBase64String(originalBytes), "image/png");

            receivedFileName.Should().Be("photo.png");
            received.Content.Should().Equal(originalBytes);
            received.ContentType.Should().Be("image/png");
            result.Content.Should().Equal(transformedBytes);
            component.Instance.FileList[0].FileName.Should().Be("watermarked.png");
        }

        [Fact]
        public void Picture_circle_uses_circle_list_and_trigger_classes()
        {
            JSInterop.SetupVoid(JSInteropConstants.AddFileClickEventListener, _ => true);

            var component = Context.RenderComponent<AntDesign.Upload>(parameters => parameters
                .Add(p => p.Action, "https://example.com/upload")
                .Add(p => p.ListType, UploadListType.PictureCircle)
                .Add(p => p.DefaultFileList, new List<UploadFileItem>
                {
                    new() { Id = "avatar", FileName = "avatar.png", State = UploadState.Success, Url = "https://example.com/avatar.png" }
                })
                .Add(p => p.ChildContent, "upload"));

            component.Find(".ant-upload-picture-circle-wrapper").Should().NotBeNull();
            component.Find(".ant-upload-list-picture-circle").Should().NotBeNull();
            component.Find(".ant-upload-list-item-list-type-picture-circle").Should().NotBeNull();
            component.Find(".ant-upload-select-picture-circle").Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Update_Existing_File_When_Upload_Same_Filename()
        {
            // Arrange
            JSInterop.SetupVoid(JSInteropConstants.AddFileClickEventListener, _ => true);
            JSInterop.SetupVoid(JSInteropConstants.UploadFile, _ => true).SetCanceled();
            JSInterop.SetupVoid(JSInteropConstants.ClearFile, _ => true);

            var existingFile = new UploadFileItem
            {
                FileName = "test.txt",
                State = UploadState.Success,
                Percent = 100
            };
            var fileList = new List<UploadFileItem> { existingFile };
            var component = Context.RenderComponent<AntDesign.Upload>(parameters => parameters
                .Add(p => p.Action, "https://example.com/upload")
                .Add(p => p.FileList, fileList)
                .Add(p => p.ChildContent, "some content"));

            // Setup mock for file selection
            JSInterop.Setup<List<UploadFileItem>>(JSInteropConstants.GetFileInfo, _ => true)
                .SetResult(new List<UploadFileItem> { new UploadFileItem { FileName = "test.txt" } });

            // Act
            var inputElement = component.Find("input");
            await inputElement.ChangeAsync(new ChangeEventArgs() { Value = "ignored" });

            // Assert
            var upload = component.Instance;
            upload.FileList.Count.Should().Be(1); // Should still have only one file
            var file = upload.FileList[0];
            file.FileName.Should().Be("test.txt");
            file.State.Should().Be(UploadState.Uploading); // State should be updated to uploading
            file.Percent.Should().Be(0); // Percent should be reset
        }
    }
}
