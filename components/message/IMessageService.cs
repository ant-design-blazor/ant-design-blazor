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
    /// <summary>
    /// Configure global message options
    /// </summary>
    /// <param name="config"></param>
    void Config(MessageGlobalConfig config);

    void Destroy();
    
    /// <summary>
    /// Show error message
    /// </summary>
    /// <param name="content"></param>
    /// <param name="duration"></param>
    /// <param name="onClose"></param>
    /// <returns></returns>
    Task Error(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
    
    /// <summary>
    /// Show info message
    /// </summary>
    /// <param name="content"></param>
    /// <param name="duration"></param>
    /// <param name="onClose"></param>
    /// <returns></returns>
    Task Info(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
    
    /// <summary>
    /// Show loading message
    /// </summary>
    /// <param name="content"></param>
    /// <param name="duration"></param>
    /// <param name="onClose"></param>
    /// <returns></returns>
    Task Loading(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
    
    /// <summary>
    /// Open message with provided configuration
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    Task Open([NotNull] MessageConfig config);
    
    /// <summary>
    /// Show success message
    /// </summary>
    /// <param name="content"></param>
    /// <param name="duration"></param>
    /// <param name="onClose"></param>
    /// <returns></returns>
    Task Success(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
    
    /// <summary>
    /// Show warning message
    /// </summary>
    /// <param name="content"></param>
    /// <param name="duration"></param>
    /// <param name="onClose"></param>
    /// <returns></returns>
    Task Warning(OneOf<string, RenderFragment, MessageConfig> content, double? duration = null, Action onClose = null);
}
