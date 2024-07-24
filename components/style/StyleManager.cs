// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Core.Style
{
    public class StyleManager
    {
        private List<Func<GlobalToken, Dictionary<string, string>>> _cssBuilders = new();

        public event Action OnStyleChanged;

        public void AddStyleBuilder(Func<GlobalToken, Dictionary<string, string>> func)
        {
            _cssBuilders.Add(func);
            OnStyleChanged?.Invoke();
        }

        public string BuildCss()
        {
            var sb = new StringBuilder();
            foreach (var builder in _cssBuilders)
            {
                var css = builder(Themes.Default.Derivative());

                foreach (var segment in css)
                {
                    sb.AppendLine($" .{segment.Key}{{ {segment.Value} }}");
                }
            }

            return sb.ToString();
        }
    }
}
