// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    using StringNumber = OneOf<string, int>;

    public class EmbeddedProperty
    {
        /// <summary>
        /// Width of Col
        /// </summary>
        public StringNumber Span { get; set; }

        /// <summary>
        /// The number of Cols to pull the Col to the left
        /// </summary>
        public StringNumber Pull { get; set; }

        /// <summary>
        /// The number of Cols to push the Col to the right
        /// </summary>
        public StringNumber Push { get; set; }

        /// <summary>
        /// The number of Cols to offset Col from the left
        /// </summary>
        public StringNumber Offset { get; set; }

        /// <summary>
        /// Order of Col, used in flex mode
        /// </summary>
        public StringNumber Order { get; set; }

        /// <summary>
        /// Flex value for this breakpoint.
        /// </summary>
        public StringNumber Flex { get; set; }
    }

    public partial class Col : AntDomComponentBase
    {
        /// <summary>
        /// Content of column
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Use flex mode or not. Will not use flex mode if null.
        /// </summary>
        [Parameter]
        public StringNumber Flex { get; set; }

        /// <summary>
        /// Width of Col
        /// </summary>
        [Parameter]
        public StringNumber Span { get; set; }

        /// <summary>
        /// Order of Col, used in flex mode
        /// </summary>
        [Parameter]
        public StringNumber Order { get; set; }

        /// <summary>
        /// The number of Cols to offset Col from the left
        /// </summary>
        [Parameter]
        public StringNumber Offset { get; set; }

        /// <summary>
        /// The number of Cols to push the Col to the right
        /// </summary>
        [Parameter]
        public StringNumber Push { get; set; }

        /// <summary>
        /// The number of Cols to pull the Col to the left
        /// </summary>
        [Parameter]
        public StringNumber Pull { get; set; }

        /// <summary>
        /// &lt;576px column of grid
        /// </summary>
        [Parameter]
        public OneOf<int, EmbeddedProperty> Xs { get; set; }

        /// <summary>
        /// ≥576px column of grid
        /// </summary>
        [Parameter]
        public OneOf<int, EmbeddedProperty> Sm { get; set; }

        /// <summary>
        /// ≥768px column of grid
        /// </summary>
        [Parameter]
        public OneOf<int, EmbeddedProperty> Md { get; set; }

        /// <summary>
        /// ≥992px column of grid
        /// </summary>
        [Parameter]
        public OneOf<int, EmbeddedProperty> Lg { get; set; }

        /// <summary>
        /// ≥1200px column of grid
        /// </summary>
        [Parameter]
        public OneOf<int, EmbeddedProperty> Xl { get; set; }

        /// <summary>
        /// ≥1600px column of grid
        /// </summary>
        [Parameter]
        public OneOf<int, EmbeddedProperty> Xxl { get; set; }

        [CascadingParameter]
        public Row Row { get; set; }

        private string _hostFlexStyle;
        private string _responsiveFlexStyle;

        private string GutterStyle { get; set; }

        internal void RowGutterChanged((int horizontalGutter, int verticalGutter) gutter)
        {
            GutterStyle = string.Empty;
            if (gutter.horizontalGutter > 0)
            {
                GutterStyle = $"padding-inline: {gutter.horizontalGutter / 2}px;";
            }
        }

        internal void RowGutterChanged(string horizontalGutter)
        {
            GutterStyle = string.IsNullOrWhiteSpace(horizontalGutter)
                ? string.Empty
                : $"padding-inline: {ScaleCssLength(horizontalGutter, 0.5m)};";
        }

        private static string ScaleCssLength(string value, decimal factor)
        {
            var match = Regex.Match(value.Trim(), @"^(-?\d+(?:\.\d+)?)([a-zA-Z%]+)$");
            if (match.Success && decimal.TryParse(match.Groups[1].Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var number))
            {
                return $"calc({(number * factor).ToString("0.############################", System.Globalization.CultureInfo.InvariantCulture)}{match.Groups[2].Value})";
            }

            return $"calc(({value.Trim()}) * {factor.ToString("0.############################", System.Globalization.CultureInfo.InvariantCulture)})";
        }

        private void SetHostClassMap()
        {
            var prefixCls = "ant-col";
            ClassMapper.OriginalClass = null;
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .GetIf(() => $"{prefixCls}-{this.Span.Value}", () => this.Span.Value != null)
                .GetIf(() => $"{prefixCls}-order-{this.Order.Value}", () => this.Order.Value != null)
                .GetIf(() => $"{prefixCls}-offset-{this.Offset.Value}", () => this.Offset.Value != null)
                .GetIf(() => $"{prefixCls}-pull-{this.Pull.Value}", () => this.Pull.Value != null)
                .GetIf(() => $"{prefixCls}-push-{this.Push.Value}", () => this.Push.Value != null)
                .If($"{prefixCls}-rtl", () => RTL)
                ;

            SetSizeClassMapper(prefixCls, Xs, "xs");
            SetSizeClassMapper(prefixCls, Sm, "sm");
            SetSizeClassMapper(prefixCls, Md, "md");
            SetSizeClassMapper(prefixCls, Lg, "lg");
            SetSizeClassMapper(prefixCls, Xl, "xl");
            SetSizeClassMapper(prefixCls, Xxl, "xxl");
        }

        private string HostClass
        {
            get
            {
                var componentClasses = ClassMapper.Class;
                if (!string.IsNullOrWhiteSpace(Class))
                {
                    var mappedClasses = new System.Collections.Generic.List<string>(componentClasses.Split(' ', System.StringSplitOptions.RemoveEmptyEntries));
                    foreach (var customClass in Class.Split(' ', System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        mappedClasses.Remove(customClass);
                    }

                    componentClasses = string.Join(" ", mappedClasses);
                }

                return string.IsNullOrWhiteSpace(componentClasses)
                    ? Class
                    : string.IsNullOrWhiteSpace(Class) ? componentClasses : $"{componentClasses} {Class}";
            }
        }

        private void SetSizeClassMapper(string prefixCls, OneOf<int, EmbeddedProperty> parameter, string sizeName)
        {
            parameter.Switch(strNum =>
            {
                ClassMapper.If($"{prefixCls}-{sizeName}-{strNum}", () => strNum > 0);
            }, embedded =>
            {
                ClassMapper
                    .GetIf(() => $"{prefixCls}-{sizeName}-{embedded.Span.Value}", () => embedded.Span.Value != null)
                    .GetIf(() => $"{prefixCls}-{sizeName}-order-{embedded.Order.Value}", () => embedded.Order.Value != null)
                    .GetIf(() => $"{prefixCls}-{sizeName}-offset-{embedded.Offset.Value}", () => embedded.Offset.Value != null)
                    .GetIf(() => $"{prefixCls}-{sizeName}-push-{embedded.Push.Value}", () => embedded.Push.Value != null)
                    .GetIf(() => $"{prefixCls}-{sizeName}-pull-{embedded.Pull.Value}", () => embedded.Pull.Value != null);

                var flex = embedded.Flex.Match(value => (value, value != null), value => (value.ToString(System.Globalization.CultureInfo.InvariantCulture), true));
                if (flex.Item2)
                {
                    ClassMapper.If($"{prefixCls}-{sizeName}-flex", () => true);
                    var parsedFlex = ParseFlex(flex.Item1);
                    _responsiveFlexStyle = string.IsNullOrEmpty(_responsiveFlexStyle)
                        ? $"--ant-col-{sizeName}-flex: {parsedFlex};"
                        : $"{_responsiveFlexStyle} --ant-col-{sizeName}-flex: {parsedFlex};";
                }
            });
        }

        private static string ParseFlex(string flex)
        {
            if (string.Equals(flex, "auto", System.StringComparison.OrdinalIgnoreCase))
            {
                return "1 1 auto";
            }

            if (string.Equals(flex, "none", System.StringComparison.OrdinalIgnoreCase))
            {
                return "0 0 auto";
            }

            if (Regex.IsMatch(flex, @"^\d+(\.\d+)?(px|em|rem|%)$"))
            {
                return $"0 0 {flex}";
            }

            return flex;
        }

#if NET7_0_OR_GREATER
        [GeneratedRegex("^\\d+(\\.\\d+)?(px|em|rem|%)$")]
        private static partial Regex FlexRegex();
#else
        private static readonly Regex _flexRegex = new("^\\d+(\\.\\d+)?(px|em|rem|%)$");
#endif

        private void SetHostFlexStyle()
        {
            if (this.Flex.Value == null)
                return;
            var flexRegex =
#if NET7_0_OR_GREATER
                FlexRegex();
#else
                _flexRegex;
#endif
            this._hostFlexStyle = this.Flex.Match(str =>
                {
                    if (flexRegex.IsMatch(str))
                    {
                        return $"flex: 0 0 {str};";
                    }

                    if (string.Equals(str, "auto", System.StringComparison.OrdinalIgnoreCase))
                    {
                        return "flex: 1 1 auto;";
                    }

                    if (string.Equals(str, "none", System.StringComparison.OrdinalIgnoreCase))
                    {
                        return "flex: 0 0 auto;";
                    }

                    return $"flex: {str};";
                },
                num => $"flex: {num} {num} auto;");

            if (Row?.Wrap == false)
            {
                _hostFlexStyle += " min-width: 0px;";
            }
        }

        protected override void OnInitialized()
        {
            this.Row?.AddCol(this);

            this.SetHostClassMap();
            this.SetHostFlexStyle();

            base.OnInitialized();
        }

        protected override void Dispose(bool disposing)
        {
            this.Row?.RemoveCol(this);

            base.Dispose(disposing);
        }
    }
}
