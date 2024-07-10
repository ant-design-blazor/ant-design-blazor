// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class TokenWithCommonCls : GlobalToken
    {
        public string ComponentCls
        {
            get => (string)_tokens["componentCls"];
            set => _tokens["componentCls"] = value;
        }

        public string PrefixCls
        {
            get => (string)_tokens["prefixCls"];
            set => _tokens["prefixCls"] = value;
        }

        public string IconCls
        {
            get => (string)_tokens["iconCls"];
            set => _tokens["iconCls"] = value;
        }

        public string AntCls
        {
            get => (string)_tokens["antCls"];
            set => _tokens["antCls"] = value;
        }
    }
}
