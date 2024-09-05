// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Internal
{
    public interface ICheckboxGroup
    {
        internal bool Disabled { get; }

        internal string NameAttributeValue { get; }

        internal void AddItem(Checkbox checkbox);

        internal void OnCheckboxChange(Checkbox checkbox);

        internal void RemoveItem(Checkbox checkbox);
    }
}
