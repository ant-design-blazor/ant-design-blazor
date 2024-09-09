// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    /// <summary>
    /// which the confirm button is clicked
    /// </summary>
    public enum ConfirmResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Abort = 4,
        Retry = 8,
        Ignore = 16,
        Yes = 32,
        No = 64
    }
}
