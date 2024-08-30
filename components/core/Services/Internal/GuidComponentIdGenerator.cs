// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    internal class GuidComponentIdGenerator : IComponentIdGenerator
    {
        public string Generate(AntDomComponentBase component) => "ant-blazor-" + Guid.NewGuid();
    }
}
