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
    /// 
    /// </summary>
    public abstract class FeedbackRefBase : IFeedbackRef
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

    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class FeedbackRefWithOkCancelBase : FeedbackRefBase, IOkCancelRef
    {
        /// <summary>
        /// invoke when cancel button or closer click
        /// </summary>
        public Func<Task> OnCancel { get; set; }

        /// <summary>
        /// invoke when Ok button click
        /// </summary>
        public Func<Task> OnOk { get; set; }

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
