// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public enum FocusBehavior
    {
        /// <summary>
        /// When focuses, cursor will move to the last character
        /// This is default behavior.
        /// </summary>
        FocusAtLast,
        /// <summary>
        /// When focuses, cursor will move to the first character
        /// </summary>
        FocusAtFirst,
        /// <summary>
        /// When focuses, the content will be selected
        /// </summary>
        FocusAndSelectAll,
        /// <summary>
        /// When focuses, content will be cleared
        /// </summary>
        FocusAndClear
    }
}
