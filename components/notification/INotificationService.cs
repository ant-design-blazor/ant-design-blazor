// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign;

public interface INotificationService
{
    void Close(string key);
    void Config(NotificationGlobalConfig config);
    NotificationRef CreateRef([NotNull] NotificationConfig config);
    void Destroy();
    NotificationRef Error(NotificationConfig config);
    Task<NotificationRef> ErrorAsync(NotificationConfig config);
    NotificationRef Info(NotificationConfig config);
    Task<NotificationRef> InfoAsync(NotificationConfig config);
    NotificationRef Open([NotNull] NotificationConfig config);
    Task<NotificationRef> OpenAsync([NotNull] NotificationConfig config);
    NotificationRef Success(NotificationConfig config);
    Task<NotificationRef> SuccessAsync(NotificationConfig config);
    void Update(string key, OneOf<string, RenderFragment> description, OneOf<string, RenderFragment>? message = null);
    NotificationRef Warn(NotificationConfig config);
    Task<NotificationRef> WarnAsync(NotificationConfig config);
    NotificationRef Warning(NotificationConfig config);
}
