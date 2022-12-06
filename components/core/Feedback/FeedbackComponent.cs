// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Feedback Component
    /// </summary>
    /// <typeparam name="TComponentOptions"></typeparam>
    public abstract class FeedbackComponent<TComponentOptions> : TemplateComponentBase<TComponentOptions>, IModalTemplate
    {
        private IFeedbackRef _feedbackRef;

        /// <summary>
        /// The options that allow you to pass in templates from the outside
        /// </summary>
        [Parameter]
        public IFeedbackRef FeedbackRef
        {
            get => _feedbackRef;
            set
            {
                _feedbackRef = value;
                _feedbackRef.ModalTemplate ??= this;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IOkCancelRef OkCancelRef => FeedbackRef as IOkCancelRef;

        /// <summary>
        /// In order that the user can close the template through the button
        /// 为了用户可以在模板内通过按钮主动关闭
        /// </summary>
        /// <returns></returns>
        public async Task CloseFeedbackAsync()
        {
            await (FeedbackRef?.CloseAsync() ?? Task.CompletedTask);
        }

        /// <summary>
        /// Call back when OK button is triggered, which can be used to cancel closing
        /// 在 OK 按钮触发时回调，可以用来取消关闭
        /// </summary>
        /// <returns></returns>
        [Obsolete("Please replace it with OnFeedbackOkAsync")]
        public virtual Task OkAsync(ModalClosingEventArgs args)
        {
            return OnFeedbackOkAsync(args);
        }

        /// <summary>
        /// Call back when Cancel button is triggered, which can be used to cancel closing
        /// 在 Cancel 按钮触发时回调，可以用来取消关闭
        /// </summary>
        /// <returns></returns>
        [Obsolete("Please replace it with OnFeedbackCancelAsync")]
        public virtual Task CancelAsync(ModalClosingEventArgs args)
        {
            return OnFeedbackCancelAsync(args);
        }


        /// <summary>
        /// Call back when OK button is triggered, which can be used to cancel closing
        /// 在 OK 按钮触发时回调，可以用来取消关闭
        /// </summary>
        /// <returns></returns>
        public virtual Task OnFeedbackOkAsync(ModalClosingEventArgs args)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Call back when Cancel button is triggered, which can be used to cancel closing
        /// 在 Cancel 按钮触发时回调，可以用来取消关闭
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual Task OnFeedbackCancelAsync(ModalClosingEventArgs args)
        {
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Feedback Component
    /// </summary>
    /// <typeparam name="TComponentOptions"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class FeedbackComponent<TComponentOptions, TResult> : FeedbackComponent<TComponentOptions>
    {
        private IOkCancelRef<TResult> _okCancelRefWithResult;

        /// <summary>
        /// 
        /// </summary>
        public IOkCancelRef<TResult> OkCancelRefWithResult => _okCancelRefWithResult ??= FeedbackRef as IOkCancelRef<TResult>;
    }
}
