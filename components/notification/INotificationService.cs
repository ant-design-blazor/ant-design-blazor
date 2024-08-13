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
    Task Close(string key);
    void Config(NotificationGlobalConfig config);
    Task<NotificationRef> CreateRefAsync([NotNull] NotificationConfig config);
    void Destroy();
    Task<NotificationRef> Error(NotificationConfig config);
    Task<NotificationRef> Info(NotificationConfig config);
    Task<NotificationRef> Open([NotNull] NotificationConfig config);
    Task<NotificationRef> Success(NotificationConfig config);
    Task UpdateAsync(string key, OneOf<string, RenderFragment> description, OneOf<string, RenderFragment>? message = null);
    Task<NotificationRef> Warn(NotificationConfig config);
    Task<NotificationRef> Warning(NotificationConfig config);
}
