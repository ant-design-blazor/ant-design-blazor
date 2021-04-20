using System.Text.Json.Serialization;

namespace AntDesign.Core.JsInterop.ObservableApi
{
    public class BoxSize
    {
        [JsonPropertyName("blockSize")]
        public decimal BlockSize { get; set; }

        [JsonPropertyName("inlineSize")]
        public decimal InlineSize { get; set; }
    }
}
