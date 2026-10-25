// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using AntDesign.Theming;
using System.Linq;

namespace AntDesign
{
    public abstract class AntDomComponentBase : AntComponentBase
    {
        private static readonly System.Collections.Generic.HashSet<string> GlobalTokenNames = new(GeneratedGlobalTokens.All.Select(token => token.Name), System.StringComparer.Ordinal);
        [Inject]
        private IComponentIdGenerator ComponentIdGenerator { get; set; }

        /// <summary>
        /// ID for the component's HTML
        /// </summary>
        /// <default value="Uniquely Generated ID" />
        [Parameter]
        public string Id { get; set; }

        [CascadingParameter]
        public ConfigProvider ConfigProvider { get; set; }

        [CascadingParameter(Name = "AntDesignThemeComponents")]
        private System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IDictionary<string, string>> ThemeComponents { get; set; }

        protected bool RTL => ConfigProvider?.Direction == "RTL";

        private ElementReference _ref;

        /// <summary>
        /// Returned ElementRef reference for DOM element.
        /// It would set the value into <see cref="AntComponentBase.RefBack"/> that user can get the <see cref="ElementReference"/> outside.
        /// </summary>
        internal protected virtual ElementReference Ref
        {
            get => _ref;
            set
            {
                _ref = value;
                RefBack?.Set(value);
            }
        }

        protected ClassMapper ClassMapper { get; } = new ClassMapper();

        protected string GetComponentTokenStyle(string component)
        {
            if (string.IsNullOrWhiteSpace(component) || ThemeComponents == null || !ThemeComponents.TryGetValue(component, out var tokens))
                return string.Empty;

            var declarations = new System.Collections.Generic.List<string>();
            foreach (var token in tokens)
            {
                // Unset generated OneOf component tokens stringify to values such as
                // "System.String: ". Never emit those: they shadow the stylesheet's
                // valid CSS variable fallback (notably Button's horizontal padding).
                if (string.IsNullOrWhiteSpace(token.Key)
                    || string.IsNullOrWhiteSpace(token.Value)
                    || token.Value.StartsWith("System.", System.StringComparison.Ordinal)) continue;
                var name = $"{component}-{token.Key}";
                var value = CssVariableValueFormatter.Format(name, token.Value);
                declarations.Add($"--ant-{TokenView.ToKebabCase(name)}:{value}");
                // React permits global tokens in a component theme. The shared
                // CSS consumes their global variable names; emit these only on
                // the component root so sibling components remain unaffected.
                if (GlobalTokenNames.Contains(token.Key))
                    declarations.Add($"--ant-{TokenView.ToKebabCase(token.Key)}:{CssVariableValueFormatter.Format(token.Key, token.Value)}");
            }

            return string.Join(";", declarations);
        }

        protected string GetComponentStyle(string component, string style)
        {
            var tokenStyle = GetComponentTokenStyle(component);
            return string.IsNullOrEmpty(tokenStyle) ? style : $"{tokenStyle};{style}";
        }

        protected AntDomComponentBase()
        {
        }

        protected override void OnInitialized()
        {
            Id ??= ComponentIdGenerator.Generate(this);
            base.OnInitialized();
        }

        /// <summary>
        /// Specifies one or more class names for an DOM element.
        /// </summary>
        [Parameter]
        public string Class
        {
            get => _class;
            set
            {
                _class = value;
                ClassMapper.OriginalClass = value;
            }
        }

        /// <summary>
        /// Specifies an inline style for a DOM element.
        /// </summary>
        [Parameter]
        public string Style
        {
            get => _style;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _style = string.Empty;
                }
                else
                {
                    var trimmed = value.Trim();
                    _style = trimmed.EndsWith(';') ? trimmed : trimmed + ";";
                }
            }
        }

        protected virtual string GenerateStyle()
        {
            return Style;
        }

        private string _class;
        private string _style;
    }
}
