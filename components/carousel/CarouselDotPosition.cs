// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public enum CarouselDotPosition
    {
        Top,
        Bottom,
        Start,
        End,
        [System.Obsolete("Use Start instead. This value maps to the start side in v6.")]
        Left,
        [System.Obsolete("Use End instead. This value maps to the end side in v6.")]
        Right,
    }
}
