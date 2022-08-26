// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign;

public interface IMessageService
{
    void Config(MessageGlobalConfig config);
    void Destroy();
    Task Error(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
    Task Info(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
    Task Loading(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
    Task Open([NotNull] MessageConfig config);
    Task Success(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
    Task Warning(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
}
