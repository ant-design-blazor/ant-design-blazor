using System.Text.Json.Serialization;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Core.JsInterop.ObservableApi
{
    public class ResizeObserverEntry
    {
        [JsonPropertyName("borderBoxSize")]
        public BoxSize BorderBoxSize { get; set; }

        [JsonPropertyName("contentBoxSize")]
        public BoxSize ContentBoxSize { get; set; }

        [JsonPropertyName("contentRect")]
        public DomRect ContentRect { get; set; }

        [JsonPropertyName("target")]
        public ElementReference Target { get; set; }

    }
}
