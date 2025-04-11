// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign;

public static class MessageExtensions
{
    /// <summary>
    /// Displays a loading message while an asynchronous operation is being executed.
    /// </summary>
    /// <typeparam name="T">The return type of the asynchronous operation.</typeparam>
    /// <param name="messageService">The message service.</param>
    /// <param name="asyncFunc">The asynchronous operation to execute.</param>
    /// <param name="content">The message to display during loading.</param>
    /// <param name="onClose">The action to perform when the loading message is closed.</param>
    /// <returns>The result of the asynchronous operation.</returns>
    public static async Task<T> LoadingWhen<T>(
        this IMessageService messageService,
        Func<Task<T>> asyncFunc,
        OneOf<string, RenderFragment, MessageConfig> content,
        Action onClose = null
    )
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var task = messageService.LoadingAsync(content, 0, onClose);
        try
        {
            return await asyncFunc();
        }
        finally
        {
            task.Start();
        }
    }

    /// <summary>
    /// Displays a loading message while an asynchronous operation is being executed (version without a return value).
    /// </summary>
    /// <param name="messageService">The message service.</param>
    /// <param name="asyncFunc">The asynchronous operation to execute.</param>
    /// <param name="content">The message to display during loading.</param>
    /// <param name="onClose">The action to perform when the loading message is closed.</param>
    public static async Task LoadingWhen(
        this IMessageService messageService,
        Func<Task> asyncFunc,
        OneOf<string, RenderFragment, MessageConfig> content,
        Action onClose = null
    )
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var task = messageService.LoadingAsync(content, 0, onClose);
        try
        {
            await asyncFunc();
        }
        finally
        {
            task.Start();
        }
    }
}
