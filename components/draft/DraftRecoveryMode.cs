// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>
    /// Draft recovery mode.
    /// </summary>
    public enum DraftRecoveryMode
    {
        /// <summary>
        /// Show confirmation dialog for user to choose whether to recover.
        /// </summary>
        Confirm,

        /// <summary>
        /// Automatically recover the draft.
        /// </summary>
        Auto,

        /// <summary>
        /// Don't recover the draft (only detect existence).
        /// </summary>
        Manual
    }
}
