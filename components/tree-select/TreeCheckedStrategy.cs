// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable 1591 // Disable missing XML comment

namespace AntDesign
{
    public enum TreeCheckedStrategy
    {
        ShowAll, // Show all the checked nodes
        ShowParent, // Only show parent node if all the children checked
        ShowChild // Only show the child nodes if all the children checked
    }
}
