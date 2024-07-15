using System.Text.Json.Serialization;

namespace AntDesign.JsInterop
{
    public class HtmlElement
    {
        [JsonPropertyName("absoluteTop")]
        public decimal AbsoluteTop { get; set; }

        [JsonPropertyName("absoluteLeft")]
        public decimal AbsoluteLeft { get; set; }

        [JsonPropertyName("offsetTop")]
        public decimal OffsetTop { get; set; }

        [JsonPropertyName("offsetLeft")]
        public decimal OffsetLeft { get; set; }

        [JsonPropertyName("offsetWidth")]
        public decimal OffsetWidth { get; set; }

        [JsonPropertyName("offsetHeight")]
        public decimal OffsetHeight { get; set; }

        [JsonPropertyName("scrollHeight")]
        public decimal ScrollHeight { get; set; }

        [JsonPropertyName("scrollWidth")]
        public decimal ScrollWidth { get; set; }

        [JsonPropertyName("scrollLeft")]
        public decimal ScrollLeft { get; set; }

        [JsonPropertyName("scrollTop")]
        public decimal ScrollTop { get; set; }

        [JsonPropertyName("clientTop")]
        public decimal ClientTop { get; set; }

        [JsonPropertyName("clientLeft")]
        public decimal ClientLeft { get; set; }

        [JsonPropertyName("clientHeight")]
        public decimal ClientHeight { get; set; }

        [JsonPropertyName("clientWidth")]
        public decimal ClientWidth { get; set; }

        [JsonPropertyName("selectionStart")]
        public decimal SelectionStart { get; set; }

        [JsonPropertyName("marginTop")]
        public decimal MarginTop { get; set; }

        [JsonPropertyName("marginBottom")]
        public decimal MarginBottom { get; set; }

        [JsonPropertyName("marginLeft")]
        public decimal MarginLeft { get; set; }

        [JsonPropertyName("marginRight")]
        public decimal MarginRight { get; set; }
    }
}
