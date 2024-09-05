﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign
{
    internal interface IAnchor
    {
        void Add(AnchorLink anchorLink);
        void Remove(AnchorLink anchorLink);

        void Clear();

        List<AnchorLink> FlatChildren();
    }
}
