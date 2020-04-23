using System;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntBlazor
{
    using StringNumber = OneOf<string, int>;

    public class EmbeddedProperty
    {
        public StringNumber Span { get; set; }

        public StringNumber Pull { get; set; }

        public StringNumber Push { get; set; }

        public StringNumber Offset { get; set; }

        public StringNumber Order { get; set; }
    }

    public partial class AntCol : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public StringNumber Flex { get; set; }

        [Parameter]
        public StringNumber Span { get; set; }

        [Parameter]
        public StringNumber Order { get; set; }

        [Parameter]
        public StringNumber Offset { get; set; }

        [Parameter]
        public StringNumber Push { get; set; }

        [Parameter]
        public StringNumber Pull { get; set; }

        [Parameter]
        public OneOf<int, EmbeddedProperty> Xs { get; set; }

        [Parameter]
        public OneOf<int, EmbeddedProperty> Sm { get; set; }

        [Parameter]
        public OneOf<int, EmbeddedProperty> Md { get; set; }

        [Parameter]
        public OneOf<int, EmbeddedProperty> Lg { get; set; }

        [Parameter]
        public OneOf<int, EmbeddedProperty> Xl { get; set; }

        [Parameter]
        public OneOf<int, EmbeddedProperty> Xxl { get; set; }

        [CascadingParameter]
        public AntRow Row { get; set; }

        private string _hostFlexStyle = null;

        private string GutterStyle { get; set; }

        internal void RowGutterChanged((int horizontalGutter, int verticalGutter) gutter)
        {
            GutterStyle = "";
            if (gutter.horizontalGutter > 0)
            {
                GutterStyle = $"padding-left: {gutter.horizontalGutter / 2}px;padding-right: {gutter.horizontalGutter / 2}px;";
            }
            if (gutter.verticalGutter > 0)
            {
                GutterStyle += $"padding-top: {gutter.verticalGutter / 2}px;padding-bottom: {gutter.verticalGutter / 2}px;";
            }
        }

        private void SetHostClassMap()
        {
            string prefixCls = "ant-col";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-{this.Span.Value}", () => this.Span.Value != null)
                .If($"{prefixCls}-order-{this.Order.Value}", () => this.Order.Value != null)
                .If($"{prefixCls}-offset-{this.Offset.Value}", () => this.Offset.Value != null)
                .If($"{prefixCls}-pull-{this.Pull.Value}", () => this.Pull.Value != null)
                .If($"{prefixCls}-push-{this.Push.Value}", () => this.Push.Value != null)
                ;

            string[] listOfSizeInputName = { "xs", "sm", "md", "lg", "xl", "xxl" };
            var properties = GetType().GetProperties();
            foreach (var sizeName in listOfSizeInputName)
            {
                var property = properties.FirstOrDefault(f => f.Name.Equals(sizeName));
                if (property == null)
                    continue;

                var fieldValue = (OneOf<int, EmbeddedProperty>)property.GetValue(this);
                if (fieldValue.Value == null)
                    continue;

                fieldValue.Switch(strNum =>
                {
                    ClassMapper.If($"{prefixCls}-{sizeName}-{strNum}", () => strNum > 0);
                }, embedded =>
                {
                    ClassMapper
                        .If($"{prefixCls}-{sizeName}-order-{embedded.Order.Value}", () => embedded.Order.Value != null)
                        .If($"{prefixCls}-{sizeName}-offset-{embedded.Offset.Value}", () => embedded.Offset.Value != null)
                        .If($"{prefixCls}-{sizeName}-push-{embedded.Push.Value}", () => embedded.Push.Value != null)
                        .If($"{prefixCls}-{sizeName}-pull-{embedded.Pull.Value}", () => embedded.Pull.Value != null);
                });
            }
        }

        private void SetHostFlexStyle()
        {
            if (this.Flex.Value == null)
                return;

            this._hostFlexStyle = this.Flex.Match(str =>
                {
                    if (Regex.Match(str, "^\\d+(\\.\\d+)?(px|em|rem|%)$").Success)
                    {
                        return $"0 0 {Flex}";
                    }

                    return Flex.AsT0;
                },
                num => $"{Flex} {Flex} auto");
        }

        protected override void OnInitialized()
        {
            if (this is AntCol col)
            {
                this.Row?.Cols.Add(col);
            }
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            this.SetHostClassMap();
            this.SetHostFlexStyle();
            base.OnParametersSet();
        }

        public override void Dispose()
        {
            if (this is AntCol col)
            {
                this.Row?.Cols.Remove(col);
            }

            base.Dispose();
        }
    }
}
