// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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