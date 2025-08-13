// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign;

public interface ITaskWrappedMessageService
{
    /// <summary>
    /// Opens a custom message after the wrapped task completes
    /// </summary>
    /// <param name="config">Message configuration</param>
    /// <returns>A task that completes when the wrapped task completes</returns>
    Task OpenAsync(MessageConfig config);

    /// <summary>
    /// Displays an error message after the wrapped task completes
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>A task that completes when the wrapped task completes</returns>
    Task ErrorAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);

    /// <summary>
    /// Displays an info message after the wrapped task completes
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>A task that completes when the wrapped task completes</returns>
    Task InfoAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);

    /// <summary>
    /// Displays a loading message after the wrapped task completes
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>A task that completes when the wrapped task completes</returns>
    Task LoadingAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);

    /// <summary>
    /// Displays a success message after the wrapped task completes
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>A task that completes when the wrapped task completes</returns>
    Task SuccessAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);

    /// <summary>
    /// Displays a warning message after the wrapped task completes
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>A task that completes when the wrapped task completes</returns>
    Task WarningAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);
}

public interface ITaskWrappedMessageService<T>
{
    /// <summary>
    /// Opens a custom message after the wrapped task completes and returns the task result
    /// </summary>
    /// <param name="config">Message configuration</param>
    /// <returns>The result of the wrapped task</returns>
    Task<T> OpenAsync(MessageConfig config);

    /// <summary>
    /// Displays an error message after the wrapped task completes and returns the task result
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>The result of the wrapped task</returns>
    Task<T> ErrorAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);

    /// <summary>
    /// Displays an info message after the wrapped task completes and returns the task result
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>The result of the wrapped task</returns>
    Task<T> InfoAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);

    /// <summary>
    /// Displays a loading message after the wrapped task completes and returns the task result
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>The result of the wrapped task</returns>
    Task<T> LoadingAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);

    /// <summary>
    /// Displays a success message after the wrapped task completes and returns the task result
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>The result of the wrapped task</returns>
    Task<T> SuccessAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);

    /// <summary>
    /// Displays a warning message after the wrapped task completes and returns the task result
    /// </summary>
    /// <param name="content">Message content, can be string, RenderFragment, or MessageConfig</param>
    /// <param name="onClose">Action to execute when the message is closed</param>
    /// <returns>The result of the wrapped task</returns>
    Task<T> WarningAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null);
}

file class TaskWrappedMessageService : ITaskWrappedMessageService
{
    private readonly IMessageService _messageService;
    private readonly Task _task;

    public TaskWrappedMessageService(IMessageService service, Task task)
    {
        _messageService = service;
        _task = task;
    }

    public async Task OpenAsync(MessageConfig config)
    {
        config.Duration = 0;
        var task = _messageService.OpenAsync(config);
        try
        {
            await _task;
        }
        finally
        {
            task.Start();
        }
    }

    public async Task ErrorAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var task = _messageService.ErrorAsync(content, 0, onClose);
        try
        {
            await _task;
        }
        finally
        {
            task.Start();
        }
    }

    public async Task InfoAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var task = _messageService.InfoAsync(content, 0, onClose);
        try
        {
            await _task;
        }
        finally
        {
            task.Start();
        }
    }

    public async Task LoadingAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var task = _messageService.LoadingAsync(content, 0, onClose);
        try
        {
            await _task;
        }
        finally
        {
            task.Start();
        }
    }

    public async Task SuccessAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var task = _messageService.SuccessAsync(content, 0, onClose);
        try
        {
            await _task;
        }
        finally
        {
            task.Start();
        }
    }

    public async Task WarningAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var task = _messageService.WarningAsync(content, 0, onClose);
        try
        {
            await _task;
        }
        finally
        {
            task.Start();
        }
    }
}

file class TaskWrappedMessageService<T> : ITaskWrappedMessageService<T>
{
    private readonly IMessageService _messageService;
    private readonly Task<T> _task;

    public TaskWrappedMessageService(IMessageService service, Task<T> task)
    {
        _messageService = service;
        _task = task;
    }

    public async Task<T> OpenAsync(MessageConfig config)
    {
        config.Duration = 0;
        var messageTask = _messageService.OpenAsync(config);
        try
        {
            return await _task;
        }
        finally
        {
            messageTask.Start();
        }
    }

    public async Task<T> ErrorAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var messageTask = _messageService.ErrorAsync(content, 0, onClose);
        try
        {
            return await _task;
        }
        finally
        {
            messageTask.Start();
        }
    }

    public async Task<T> InfoAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var messageTask = _messageService.InfoAsync(content, 0, onClose);
        try
        {
            return await _task;
        }
        finally
        {
            messageTask.Start();
        }
    }

    public async Task<T> LoadingAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var messageTask = _messageService.LoadingAsync(content, 0, onClose);
        try
        {
            return await _task;
        }
        finally
        {
            messageTask.Start();
        }
    }

    public async Task<T> SuccessAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var messageTask = _messageService.SuccessAsync(content, 0, onClose);
        try
        {
            return await _task;
        }
        finally
        {
            messageTask.Start();
        }
    }

    public async Task<T> WarningAsync(OneOf<string, RenderFragment, MessageConfig> content, Action onClose = null)
    {
        if (content.IsT2)
        {
            content.AsT2.Duration = 0;
        }
        var messageTask = _messageService.WarningAsync(content, 0, onClose);
        try
        {
            return await _task;
        }
        finally
        {
            messageTask.Start();
        }
    }
}

public static class MessageExtensions
{
    public static ITaskWrappedMessageService<T> WrappedBy<T>(
        this Task<T> task,
        IMessageService service
    ) => new TaskWrappedMessageService<T>(service, task);

    public static ITaskWrappedMessageService WrappedBy(
        this Task task,
        IMessageService service
    ) => new TaskWrappedMessageService(service, task);


    /// <summary>
    /// Displays a loading message while an asynchronous operation is being executed.
    /// </summary>
    /// <typeparam name="T">The return type of the asynchronous operation.</typeparam>
    /// <param name="messageService">The message service.</param>
    /// <param name="asyncFunc">The asynchronous operation to execute.</param>
    /// <param name="content">The message to display during loading.</param>
    /// <param name="onClose">The action to perform when the loading message is closed.</param>
    /// <returns>The result of the asynchronous operation.</returns>
    [Obsolete("Use Task.WrappedBy(messageService).LoadingAsync() instead.")]
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
    [Obsolete("Use Task.WrappedBy(messageService).LoadingAsync() instead.")]
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
