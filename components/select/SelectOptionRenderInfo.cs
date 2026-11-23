// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign.Select.Internal;

namespace AntDesign
{
    /// <summary>
    /// Context passed to <see cref="Select{TItemValue, TItem}.OptionRender"/>.
    /// </summary>
    /// <typeparam name="TItemValue">The option value type.</typeparam>
    /// <typeparam name="TItem">The option source item type.</typeparam>
    public sealed class SelectOptionRenderInfo<TItemValue, TItem>
    {
        /// <summary>The option being rendered, including its value and source item.</summary>
        public SelectOptionItem<TItemValue, TItem> Option { get; init; }

        /// <summary>The option's position in the currently rendered, filtered option list.</summary>
        public int Index { get; init; }
    }
}
