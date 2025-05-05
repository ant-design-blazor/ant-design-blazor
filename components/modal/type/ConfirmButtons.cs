﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{

    /// <summary>
    /// Confirm footer buttons type
    /// </summary>
#pragma warning disable CA1717 // 只有 FlagsAttribute 枚举应采用复数形式的名称
    public enum ConfirmButtons
#pragma warning restore CA1717 // 只有 FlagsAttribute 枚举应采用复数形式的名称
    {
        /// <summary>
        /// Only a OK button
        /// </summary>
        OK = 0,

        #region two buttons

        /// <summary>
        /// two buttons: OK and a Cancel
        /// </summary>
        OKCancel = 10,

        /// <summary>
        /// two buttons: Yes and No 
        /// </summary>
        YesNo = 11,

        /// <summary>
        /// two buttons: Retry and Cancel
        /// </summary>
        RetryCancel = 12,

        #endregion

        #region three buttons

        /// <summary>
        /// three buttons: Abort, Retry and Ignore
        /// </summary>
        AbortRetryIgnore = 20,

        /// <summary>
        /// three buttons: Yes, No and Cancel
        /// </summary>
        YesNoCancel = 21,

        #endregion
    }
}
