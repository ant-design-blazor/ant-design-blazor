// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public sealed class GlobalTheme : EnumValue<GlobalTheme, string>
    {
        public static readonly GlobalTheme Light = new(nameof(Light).ToLowerInvariant(), "_content/AntDesign/css/ant-design-blazor.css");
        public static readonly GlobalTheme Dark = new(nameof(Dark).ToLowerInvariant(), "_content/AntDesign/css/ant-design-blazor.dark.css");
        public static readonly GlobalTheme Compact = new(nameof(Compact).ToLowerInvariant(), "_content/AntDesign/css/ant-design-blazor.compact.css");
        public static readonly GlobalTheme Aliyun = new(nameof(Aliyun).ToLowerInvariant(), "_content/AntDesign/css/ant-design-blazor.aliyun.css");

        private GlobalTheme(string name, string value) : base(name, value)
        {
        }
    }
}
