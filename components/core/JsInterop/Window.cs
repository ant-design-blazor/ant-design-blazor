using System.Text.Json.Serialization;

namespace AntDesign.JsInterop
{
    public class Window
    {
        [JsonPropertyName("innerWidth")]
        public decimal InnerWidth { get; set; }

        [JsonPropertyName("innerHeight")]
        public decimal InnerHeight { get; set; }
    }
}
