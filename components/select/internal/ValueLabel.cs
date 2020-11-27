using System.Text.Json.Serialization;

namespace AntDesign.Select.Internal
{
    internal class ValueLabel<TItemValue>
    {
        [JsonPropertyName("value")]
        public TItemValue Value { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
    }
}
