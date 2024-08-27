// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign
{
    public class AnchorLinkEqualityComparer : IEqualityComparer<AnchorLink>
    {
        public bool Equals(AnchorLink x, AnchorLink y)
        {
            return x.Href == y.Href;
        }

        public int GetHashCode(AnchorLink obj)
        {
            return obj.Href.GetHashCode();
        }
    }
}
