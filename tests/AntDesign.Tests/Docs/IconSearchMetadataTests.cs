using AntDesign.Docs.Services;
using Xunit;

namespace AntDesign.Tests.Docs;

public class IconSearchMetadataTests
{
    [Theory]
    [InlineData("AccountBookOutlined", "ledger")]
    [InlineData("AnthropicFilled", "llm")]
    [InlineData("CheckCircleTwoTone", "success")]
    [InlineData("AlipayCircleOutlined", "payment")]
    public void Matches_react_icon_search_tags(string componentName, string query)
    {
        Assert.True(IconSearchMetadata.Matches(componentName, query));
    }

    [Fact]
    public void Matches_is_case_insensitive_and_ignores_theme_suffix()
    {
        Assert.True(IconSearchMetadata.Matches("AnthropicFilled", "ANTHROPIC"));
    }

    [Fact]
    public void Matches_returns_false_for_unmatched_or_empty_queries()
    {
        Assert.False(IconSearchMetadata.Matches("AccountBookOutlined", "volcano"));
        Assert.False(IconSearchMetadata.Matches("UnknownOutlined", "ledger"));
        Assert.False(IconSearchMetadata.Matches("AccountBookOutlined", " "));
    }
}
