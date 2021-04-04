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
    /// Component reference with Ok and Cancel method
    /// </summary>
    public interface IOkCancelRef : IFeedbackRef
    {
        /// <summary>
        /// invoked when cancel button or closer click
        /// </summary>
        public Func<Task> OnCancel { get; set; }

        /// <summary>
        /// invoked when Ok button click
        /// </summary>
        public Func<Task> OnOk { get; set; }

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
    public interface IOkCancelRef<TResult> : IFeedbackRef
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
}
