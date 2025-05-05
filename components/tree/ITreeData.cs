// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign
{
    public interface ITreeData<TItem>
    {
        public string Key { get; set; }

        public string Title { get; set; }

        public TItem Value { get; }

        public IEnumerable<TItem> Children { get; set; }
    }
}
