// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Core.Style;
using CssInCSharp;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Docs.Pages
{
    public partial class StyleTest : AntDomComponentBase
    {

        private CreateStyles _useStyles = (token) =>
        {
            var commonCard = new CSSObject()
            {
                BorderRadius = token.BorderRadiusLG,
                Padding = token.PaddingLG,
            };

            return new
            {
                container = new CSSObject
                {
                    BackgroundColor = token.ColorBgLayout,
                    Padding = "24px"
                },
                defaultCard = new CSSObject()
                {
                    ["..."] = commonCard,
                    Background = token.ColorBgContainer,
                    Color = token.ColorText
                },
                primaryCard = new CSSObject()
                {
                    ["..."] = commonCard,
                    Background = token.ColorPrimary,
                    Color = token.ColorTextLightSolid,
                }
            };
        };
    }
}
