// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;
using Bunit;
using CssInCSharp;

namespace AntDesign.Tests.Styles
{
    public class StyleTestsBase : TestContext
    {
        public StyleTestsBase()
        {
            Environment.SetEnvironmentVariable("RENDER_MODE", "testing");
        }

        protected string LoadStyleHtml(string path)
        {
            var content = string.Empty;
            if (File.Exists(path))
            {
                content = File.ReadAllText(path);
                content = content.Replace(">", "&gt;").Replace("<", "&lt;");
            }
            return $"<style data-css-hash:ignore data-token-hash:ignore data-cache-path:ignore>{content}</style>";
        }
    }
}
