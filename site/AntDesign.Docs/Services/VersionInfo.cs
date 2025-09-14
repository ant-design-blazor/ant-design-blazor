using System.Text.Json.Serialization;

namespace AntDesign.Docs.Services
{
    public class VersionInfo
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("releaseDate")]
        public string ReleaseDate { get; set; }
    }
}