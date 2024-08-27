// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public sealed class AntMenuMode : EnumValue<AntMenuMode>
    {
        public static readonly AntMenuMode Vertical = new AntMenuMode(nameof(Vertical).ToLowerInvariant(), 1);
        public static readonly AntMenuMode Horizontal = new AntMenuMode(nameof(Horizontal).ToLowerInvariant(), 2);
        public static readonly AntMenuMode Inline = new AntMenuMode(nameof(Inline).ToLowerInvariant(), 3);

        private AntMenuMode(string name, int value) : base(name, value)
        {
        }
    }
}
