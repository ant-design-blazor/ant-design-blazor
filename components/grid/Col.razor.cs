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

        private string GutterStyle { get; set; }

        internal void RowGutterChanged((int horizontalGutter, int verticalGutter) gutter)
        {
            GutterStyle = string.Empty;
            if (gutter.horizontalGutter > 0)
            {
                GutterStyle = $"padding-left: {gutter.horizontalGutter / 2}px; padding-right: {gutter.horizontalGutter / 2}px;";
            }
        }

        private void SetHostClassMap()
        {
            var prefixCls = "ant-col";
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
            });
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

                    return $"flex: {str};";
                },
                num => $"flex: {num} {num} auto;");
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
