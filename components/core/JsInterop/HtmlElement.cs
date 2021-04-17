using System.Text.Json.Serialization;

namespace AntDesign.JsInterop
{
    public class HtmlElement
    {
        [JsonPropertyName("absoluteTop")]
        public int AbsoluteTop { get; set; }

        [JsonPropertyName("absoluteLeft")]
        public int AbsoluteLeft { get; set; }

        [JsonPropertyName("offsetTop")]
        public int OffsetTop { get; set; }

        [JsonPropertyName("offsetLeft")]
        public int OffsetLeft { get; set; }

        [JsonPropertyName("offsetWidth")]
        public int OffsetWidth { get; set; }

        [JsonPropertyName("offsetHeight")]
        public int OffsetHeight { get; set; }

        [JsonPropertyName("scrollHeight")]
        public int ScrollHeight { get; set; }

        [JsonPropertyName("scrollWidth")]
        public int ScrollWidth { get; set; }

        [JsonPropertyName("scrollLeft")]
        public double ScrollLeft { get; set; }

        [JsonPropertyName("scrollTop")]
        public double ScrollTop { get; set; }

        [JsonPropertyName("clientTop")]
        public int ClientTop { get; set; }

        [JsonPropertyName("clientLeft")]
        public int ClientLeft { get; set; }

        [JsonPropertyName("clientHeight")]
        public int ClientHeight { get; set; }

        [JsonPropertyName("clientWidth")]
        public int ClientWidth { get; set; }
    }
}
