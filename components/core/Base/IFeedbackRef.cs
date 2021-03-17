// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    /// Component reference with open and close method
    /// </summary>
    public interface IFeedbackRef
    {
        /// <summary>
        /// to get feedback inner component's event 
        /// </summary>
        internal IModalTemplate ModalTemplate { get; set; }

        /// <summary>
        /// on Feedback open
        /// </summary>
        public Func<Task> OnOpen { get; set; }

        /// <summary>
        /// on Feedback close
        /// </summary>
        public Func<Task> OnClose { get; set; }

        /// <summary>
        /// invoked when cancel button or closer click
        /// </summary>
        public Func<Task> OnCancel { get; set; }

        /// <summary>
        /// invoked when Ok button click
        /// </summary>
        public Func<Task> OnOk { get; set; }

        /// <summary>
        /// open the component
        /// </summary>
        /// <returns></returns>
        public Task OpenAsync();

        /// <summary>
        /// update the component
        /// </summary>
        /// <returns></returns>
        public Task UpdateConfigAsync();

        /// <summary>
        /// just do close feedback component, and will not trigger OkAsync or OkCancel 
        /// </summary>
        /// <returns></returns>
        public Task CloseAsync();

        /// <summary>
        ///  Trigger Ok button click
        /// </summary>
        /// <returns></returns>
        public Task OkAsync(ModalClosingEventArgs e);

        /// <summary>
        ///  Trigger Cancel button click
        /// </summary>
        /// <returns></returns>
        public Task CancelAsync(ModalClosingEventArgs e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IFeedbackRef<TResult>
    {
        /// <summary>
        /// invoke when cancel button or closer click
        /// </summary>
        public Func<TResult, Task> OnCancel { get; set; }

        /// <summary>
        /// invoke when Ok button click
        /// </summary>
        public Func<TResult, Task> OnOk { get; set; }

        /// <summary>
        /// Trigger Ok button click
        /// </summary>
        /// <returns></returns>
        public Task OkAsync(TResult result);

        /// <summary>
        /// Trigger Cancel button click
        /// </summary>
        /// <returns></returns>
        public Task CancelAsync(TResult result);
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class DefaultFeedbackRef : IFeedbackRef
    {
        /// <summary>
        /// 
        /// </summary>
        IModalTemplate IFeedbackRef.ModalTemplate { get; set; }


        /// <inheritdoc />
        public Func<Task> OnOpen { get; set; }

        /// <inheritdoc />
        public Func<Task> OnClose { get; set; }

        /// <summary>
        /// invoke when cancel button or closer click
        /// </summary>
        public Func<Task> OnCancel { get; set; }

        /// <summary>
        /// invoke when Ok button click
        /// </summary>
        public Func<Task> OnOk { get; set; }

        /// <summary>
        /// just open close feedback component
        /// </summary>
        /// <returns></returns>
        public abstract Task OpenAsync();


        /// <inheritdoc />
        public abstract Task UpdateConfigAsync();

        /// <summary>
        /// just do close feedback component, and will not trigger OkAsync or OkCancel 
        /// </summary>
        /// <returns></returns>
        public abstract Task CloseAsync();

        /// <summary>
        /// Ok button click
        /// </summary>
        /// <returns></returns>
        public async Task OkAsync(ModalClosingEventArgs e)
        {
            await CloseAsync();
            if (OnOk != null)
            {
                await OnOk();
            }
        }

        /// <summary>
        /// Cancel button click
        /// </summary>
        /// <returns></returns>
        public async Task CancelAsync(ModalClosingEventArgs e)
        {
            await CloseAsync();
            if (OnCancel != null)
            {
                await OnCancel();
            }
        }
    }
}
