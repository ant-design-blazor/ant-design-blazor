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
        public StringNumber span { get; set; }

        public StringNumber pull { get; set; }

        public StringNumber push { get; set; }

        public StringNumber offset { get; set; }

        public StringNumber order { get; set; }
    }

    public class AntColBase : AntDomComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public StringNumber flex { get; set; }

        [Parameter] public StringNumber span { get; set; }

        [Parameter] public StringNumber order { get; set; }

        [Parameter] public StringNumber offset { get; set; }

        [Parameter] public StringNumber push { get; set; }

        [Parameter] public StringNumber pull { get; set; }

        [Parameter] public OneOf<int, EmbeddedProperty> xs { get; set; }

        [Parameter] public OneOf<int, EmbeddedProperty> sm { get; set; }

        [Parameter] public OneOf<int, EmbeddedProperty> md { get; set; }

        [Parameter] public OneOf<int, EmbeddedProperty> lg { get; set; }

        [Parameter] public OneOf<int, EmbeddedProperty> xl { get; set; }

        [Parameter] public OneOf<int, EmbeddedProperty> xxl { get; set; }

        [CascadingParameter] public AntRow Row { get; set; }

        protected string hostFlexStyle = null;

        protected string gutterStyle { get; set; }

        internal void RowGutterChanged((int horizontalGutter, int verticalGutter) gutter)
        {
            gutterStyle = "";
            if (gutter.horizontalGutter > 0)
            {
                gutterStyle = $"padding-left: {gutter.horizontalGutter / 2}px;padding-right: {gutter.horizontalGutter / 2}px;";
            }
            if (gutter.verticalGutter > 0)
            {
                gutterStyle += $"padding-top: {gutter.verticalGutter / 2}px;padding-bottom: {gutter.verticalGutter / 2}px;";
            }
        }

        private void SetHostClassMap()
        {
            string prefixCls = "ant-col";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-{this.span.Value}", () => this.span.Value != null)
                .If($"{prefixCls}-order-{this.order.Value}", () => this.order.Value != null)
                .If($"{prefixCls}-offset-{this.offset.Value}", () => this.offset.Value != null)
                .If($"{prefixCls}-pull-{this.pull.Value}", () => this.pull.Value != null)
                .If($"{prefixCls}-push-{this.push.Value}", () => this.push.Value != null)
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
                        .If($"{prefixCls}-{sizeName}-order-{embedded.order.Value}", () => embedded.order.Value != null)
                        .If($"{prefixCls}-{sizeName}-offset-{embedded.offset.Value}", () => embedded.offset.Value != null)
                        .If($"{prefixCls}-{sizeName}-push-{embedded.push.Value}", () => embedded.push.Value != null)
                        .If($"{prefixCls}-{sizeName}-pull-{embedded.pull.Value}", () => embedded.pull.Value != null);
                });
            }
        }

        private void SetHostFlexStyle()
        {
            if (this.flex.Value == null)
                return;

            this.hostFlexStyle = this.flex.Match(str =>
                {
                    if (Regex.Match(str, "^\\d+(\\.\\d+)?(px|em|rem|%)$").Success)
                    {
                        return $"0 0 {flex}";
                    }

                    return flex.AsT0;
                },
                num => $"{flex} {flex} auto");
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