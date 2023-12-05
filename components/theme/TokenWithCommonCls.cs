// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class TokenWithCommonCls : GlobalToken
    {
        public string ComponentCls { get; set; }
        public string PrefixCls { get; set; }
        public string IconCls { get; set; } = "anticon";
        public string AntCls { get; set; } = "ant";

        public void Merge(TokenWithCommonCls source)
        {
            base.Merge(source);
            ComponentCls = source.ComponentCls;
            PrefixCls = source.PrefixCls;
            IconCls = source.IconCls;
            AntCls = source.AntCls;
        }
    }
}
