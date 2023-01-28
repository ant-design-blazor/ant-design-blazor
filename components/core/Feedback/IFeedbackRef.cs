// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
    }
}
