// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Core.Style
{
    public class StyleManager : IStyleManager
    {
        private Dictionary<string,Func<GlobalToken, Dictionary<string, string>>> _cssBuilders = new();

        public event Action OnStyleChanged;

        public void AddStyleBuilder(string key,Func<GlobalToken, Dictionary<string, string>> func)
        {
            if (_cssBuilders.TryAdd(key, func))
            {
                OnStyleChanged?.Invoke();
            }
        }

        public string BuildCss()
        {
            var sb = new StringBuilder();
            foreach (var builder in _cssBuilders.Values)
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

    public interface IStyleManager
    {
        public event Action OnStyleChanged;
        public string BuildCss();
    }

}
