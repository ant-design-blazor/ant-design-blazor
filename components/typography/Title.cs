// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class Title : TypographyBase
    {
        protected override string HtmlType => "h" + Level;

        private const int DefaultLevel = 1;

        private int _level = DefaultLevel;

        /// <summary>
        /// Level of heading (ex: h1 would be 1). Must be between 1 and 4, inclusive. If outside that range it will default back to 1.
        /// </summary>
        /// <default value="1"/>
        [Parameter]
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value < 1 || value > 4
                    ? DefaultLevel
                    : value;
            }
        }
    }
}
