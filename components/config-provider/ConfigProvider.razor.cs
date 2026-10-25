// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AntDesign.Theming;

namespace AntDesign
{
    [Documentation(DocumentationCategory.Components, DocumentationType.Other, "", Title = "ConfigProvider", SubTitle = "全局化配置")]
    public partial class ConfigProvider : AntComponentBase
    {
        [Parameter]
        public string Direction
        {
            get => _direction;
            set
            {
                if (_direction != value)
                {
                    _direction = value;
                    _waitingDirectionUpdate = true;
                }
            }
        }

        [Parameter]
        public GlobalTheme Theme
        {
            get => _globalTheme;
            set
            {
                if (_globalTheme != value)
                {
                    _globalTheme = value;
                    _waitingGlobalThemeUpdate = true;
                }
            }
        }

        [Parameter]
        public ThemeConfig ThemeConfig { get; set; } = new ThemeConfig();

        /// <summary>
        /// Input appearance variant inherited by components such as Pagination.
        /// Supported values are outlined, filled, borderless, and underlined.
        /// </summary>
        [Parameter]
        public string Variant { get; set; }

        [CascadingParameter(Name = "AntDesignVariant")]
        private string ParentVariant { get; set; }

        public string ResolvedVariant => string.IsNullOrWhiteSpace(Variant)
            ? (string.IsNullOrWhiteSpace(ParentVariant) ? "outlined" : ParentVariant)
            : Variant;

        public bool IsWireframe => ExplicitThemeTokens.TryGetValue("wireframe", out var value) && value == "true";
        public string ThemeCssClass => IsWireframe ? "ant-theme-wireframe" : string.Empty;

        [CascadingParameter(Name = "AntDesignExplicitThemeTokens")]
        private System.Collections.Generic.IReadOnlyDictionary<string, string> ParentExplicitThemeTokens { get; set; }

        [CascadingParameter(Name = "AntDesignThemeAlgorithm")]
        private string ParentThemeAlgorithm { get; set; }

        public string ResolvedAlgorithm => ThemeConfig?.HasExplicitAlgorithm == true
            ? ThemeConfig.Algorithm : (ThemeConfig?.Inherit == false ? "default" : ParentThemeAlgorithm ?? ThemeConfig?.Algorithm ?? "default");

        public System.Collections.Generic.IReadOnlyDictionary<string, string> ExplicitThemeTokens
        {
            get
            {
                var values = ThemeConfig?.Inherit == false || ParentExplicitThemeTokens == null
                    ? new System.Collections.Generic.Dictionary<string, string>()
                    : new System.Collections.Generic.Dictionary<string, string>(ParentExplicitThemeTokens);
                if (ThemeConfig != null)
                    foreach (var token in ThemeConfig.GetExplicitTokenValues()) values[token.Key] = token.Value;
                return values;
            }
        }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IDictionary<string, string>> ResolvedComponentValues =>
            ThemeConfig.GetResolvedComponentValues(ParentThemeComponents, ParentExplicitThemeTokens, ResolvedAlgorithm);

        [CascadingParameter(Name = "AntDesignThemeComponents")]
        private System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IDictionary<string, string>> ParentThemeComponents { get; set; }

        public string ThemeScopeClass => string.IsNullOrWhiteSpace(ThemeConfig?.ThemeScope)
            ? "ant-theme"
            : ThemeConfig.ThemeScope;

        public string ThemeStyle
        {
            get
            {
                if (ThemeConfig == null || !ThemeConfig.CssVar) return string.Empty;
                var declarations = new System.Collections.Generic.List<string>();
                // Inherit explicit config, never the parent's resolved palette:
                // a light parent's derived white background must not become a
                // seed override when this provider selects the dark algorithm.
                foreach (var token in ThemeConfig.GetResolvedTokenValues(ParentExplicitThemeTokens, ResolvedAlgorithm))
                    AddCssVariable(declarations, token.Key, token.Value);
                foreach (var component in ResolvedComponentValues)
                    foreach (var token in component.Value)
                        AddCssVariable(declarations, $"{component.Key}-{token.Key}", token.Value);
                return string.Join(";", declarations);
            }
        }

        private static void AddCssVariable(System.Collections.Generic.ICollection<string> declarations, string name, string value)
        {
            // Default generated component tokens can contain an unset OneOf value
            // (serialized as "System.String:") or an empty derived token. Emitting
            // either value shadows the real v6 stylesheet variable and makes the
            // browser discard otherwise valid declarations.
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value) || value.StartsWith("System.", System.StringComparison.Ordinal))
                return;

            var formattedValue = CssVariableValueFormatter.Format(name, value);
            declarations.Add($"--ant-{TokenView.ToKebabCase(name)}:{formattedValue}");
        }

        [Parameter]
        public FormConfig Form { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Inject]
        private ConfigService ConfigService { get; set; }

        private string _direction;

        private GlobalTheme _globalTheme;

        private bool _waitingDirectionUpdate;
        private bool _waitingGlobalThemeUpdate;
        private bool _afterFirstRender;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _afterFirstRender = true;
            }

            if (_afterFirstRender)
            {
                if (_waitingDirectionUpdate)
                {
                    _waitingDirectionUpdate = false;
                    await ChangeDirection(_direction);
                }

                if (_waitingGlobalThemeUpdate)
                {
                    _waitingGlobalThemeUpdate = false;
                    await ChangeGlobalTheme(_globalTheme);
                }
            }
        }

        public async Task ChangeDirection(string direction)
        {
            _direction = direction?.ToUpperInvariant();
            await ConfigService.ChangeDirection(_direction);
            await InvokeAsync(StateHasChanged);
        }

        public async Task ChangeGlobalTheme(GlobalTheme mode)
        {
            _globalTheme = mode;
            await ConfigService.SetTheme(mode);
            await InvokeAsync(StateHasChanged);
        }
    }
}
