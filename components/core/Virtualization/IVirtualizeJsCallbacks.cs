// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NET5_0 || NET_6_0
namespace AntDesign
{
    internal interface IVirtualizeJsCallbacks
    {
        void OnBeforeSpacerVisible(float spacerSize, float spacerSeparation, float containerSize);
        void OnAfterSpacerVisible(float spacerSize, float spacerSeparation, float containerSize);
    }
}
#endif
