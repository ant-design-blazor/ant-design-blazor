// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public enum TableSize
    {
        Default = 0,
        Medium = 1,
        [System.Obsolete("Use Medium to match the Ant Design Table size API.")]
        Middle = Medium,
        Small = 2
    }
}
