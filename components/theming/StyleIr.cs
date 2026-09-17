using System.Collections.Generic;

namespace AntDesign.Theming
{
    public sealed class UpstreamInfo
    {
        public string Repository { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }

    public sealed class GeneratorInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }

    public sealed class ThemeIrEntry
    {
        public string Key { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Appearance { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }

    public sealed class UnsupportedIrEntry
    {
        public string Source { get; set; } = string.Empty;
        public string Feature { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    public sealed class StyleIr
    {
        public int SchemaVersion { get; set; } = 1;
        public UpstreamInfo Upstream { get; set; } = new();
        public GeneratorInfo Generator { get; set; } = new();
        public List<DesignToken> Tokens { get; set; } = new();
        public List<StyleIrEntry> Styles { get; set; } = new();
        public List<ThemeIrEntry> Themes { get; set; } = new();
        public List<UnsupportedIrEntry> Unsupported { get; set; } = new();
    }
}
