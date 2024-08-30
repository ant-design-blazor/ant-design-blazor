// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign;

public interface IConfirmService
{
    Task<ConfirmResult> Show(
         OneOf<string, RenderFragment> content,
         OneOf<string, RenderFragment> title,
         ConfirmButtons confirmButtons,
         ConfirmIcon confirmIcon,
         ConfirmButtonOptions options,
         ConfirmAutoFocusButton? autoFocusButton = ConfirmAutoFocusButton.Ok);

    Task<ConfirmResult> Show
        (OneOf<string, RenderFragment> content,
        OneOf<string, RenderFragment> title,
        ConfirmButtons confirmButtons = ConfirmButtons.OKCancel,
        ConfirmIcon confirmIcon = ConfirmIcon.Info);
}
