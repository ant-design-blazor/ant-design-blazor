// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public sealed class GlobalTheme : EnumValue<GlobalTheme, string>
    {
        public static readonly GlobalTheme Default = new(nameof(Default).ToLowerInvariant(), V6("light"));
        public static readonly GlobalTheme Light = new(nameof(Light).ToLowerInvariant(), V6("light"));
        public static readonly GlobalTheme Dark = new(nameof(Dark).ToLowerInvariant(), V6("dark"));
        public static readonly GlobalTheme Mui = new(nameof(Mui).ToLowerInvariant(), V6("mui"));
        public static readonly GlobalTheme Shadcn = new(nameof(Shadcn).ToLowerInvariant(), V6("shadcn"));
        public static readonly GlobalTheme Bootstrap = new(nameof(Bootstrap).ToLowerInvariant(), V6("bootstrap"));
        public static readonly GlobalTheme Cartoon = new(nameof(Cartoon).ToLowerInvariant(), V6("cartoon"));
        public static readonly GlobalTheme Illustration = new(nameof(Illustration).ToLowerInvariant(), V6("illustration"));
        public static readonly GlobalTheme Glass = new(nameof(Glass).ToLowerInvariant(), V6("glass"));
        public static readonly GlobalTheme Geek = new(nameof(Geek).ToLowerInvariant(), V6("geek"));
        public static readonly GlobalTheme Lark = new(nameof(Lark).ToLowerInvariant(), V6("lark"));
        public static readonly GlobalTheme Blossom = new(nameof(Blossom).ToLowerInvariant(), V6("blossom"));
        public static readonly GlobalTheme V4 = new(nameof(V4), V6("v4"));
        public static readonly GlobalTheme Serene = new(nameof(Serene).ToLowerInvariant(), V6("serene"));

        // Retained for compatibility. Phase-one v6 preview themes are prebuilt and immutable.
        public static readonly GlobalTheme Compact = new(nameof(Compact).ToLowerInvariant(), "_content/AntDesign/css/ant-design-blazor.compact.css");
        public static readonly GlobalTheme Aliyun = new(nameof(Aliyun).ToLowerInvariant(), "_content/AntDesign/css/ant-design-blazor.aliyun.css");

        private static string V6(string key) => $"_content/AntDesign/css/antd-v6/{key}.css";

        private GlobalTheme(string name, string value) : base(name, value)
        {
        }
    }
}
