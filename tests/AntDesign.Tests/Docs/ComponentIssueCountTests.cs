using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Docs.Services;
using AntDesign.Docs.Shared;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AntDesign.Tests.Docs;

public sealed class ComponentIssueCountTests : DocumentationTestBase
{
    private sealed class Handler : HttpMessageHandler
    {
        internal Func<HttpRequestMessage, Task<HttpResponseMessage>> Send = _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"total_count\":42,\"incomplete_results\":false}") });
        internal int Requests;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests++;
            return Send(request);
        }
    }

    [Fact]
    public async Task Search_uses_the_link_query_and_coalesces_concurrent_requests_then_expires()
    {
        var now = DateTimeOffset.UtcNow;
        var response = new TaskCompletionSource<HttpResponseMessage>();
        var handler = new Handler { Send = request => {
            var query = Uri.UnescapeDataString(request.RequestUri!.Query);
            Assert.Contains("repo:ant-design-blazor/ant-design-blazor is:issue is:open Select in:title", query);
            Assert.Contains("per_page=1", query);
            Assert.NotEmpty(request.Headers.UserAgent);
            Assert.Null(request.Headers.Authorization);
            return response.Task;
        } };
        using var service = new ComponentIssueCountService(new HttpClient(handler), () => now);
        var first = service.GetCountAsync("Select");
        Assert.Same(first, service.GetCountAsync("select"));
        response.SetResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"total_count\":42}") });
        Assert.Equal(42, await first);
        Assert.Equal(42, await service.GetCountAsync("Select"));
        Assert.Equal(1, handler.Requests);
        now = now.AddMinutes(6);
        handler.Send = _ => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"total_count\":43}") });
        Assert.Equal(43, await service.GetCountAsync("Select"));
        Assert.Equal(2, handler.Requests);
    }

    [Theory]
    [InlineData(200, "{\"total_count\":0}", 0)]
    [InlineData(200, "{\"message\":\"error\"}", null)]
    [InlineData(200, "{\"total_count\":1,\"incomplete_results\":true}", null)]
    [InlineData(200, "{\"total_count\":\"42\"}", null)]
    [InlineData(200, "not-json", null)]
    [InlineData(403, "{}", null)]
    [InlineData(429, "{}", null)]
    [InlineData(503, "{}", null)]
    public async Task Failures_and_incomplete_results_do_not_become_zero(int status, string body, int? expected)
    {
        var handler = new Handler { Send = _ => Task.FromResult(new HttpResponseMessage((HttpStatusCode)status) { Content = new StringContent(body) }) };
        using var service = new ComponentIssueCountService(new HttpClient(handler));
        Assert.Equal(expected, await service.GetCountAsync("Select"));
        Assert.Equal(expected, await service.GetCountAsync("Select"));
        Assert.Equal(1, handler.Requests);
        if (status is 403 or 429) { Assert.Null(await service.GetCountAsync("Button")); Assert.Equal(1, handler.Requests); }
        Assert.Null(await service.GetCountAsync("Select repo:other/repo"));
        Assert.Equal(1, handler.Requests);
    }

    [Fact]
    public void Navigating_components_discards_late_results_and_announces_current_count()
    {
        var select = new TaskCompletionSource<int?>();
        var issues = new Mock<IComponentIssueCountService>();
        issues.Setup(x => x.GetCountAsync("Select")).Returns(select.Task);
        issues.Setup(x => x.GetCountAsync("Icon")).ReturnsAsync(7);
        Services.AddSingleton(issues.Object);
        var cut = RenderComponent<ComponentMetadata>(p => p.Add(x => x.ComponentName, "Select").Add(x => x.ComponentKey, "select"));
        Assert.Equal("true", cut.Find(".docs-component-meta-issue-count").GetAttribute("aria-busy"));
        cut.SetParametersAndRender(p => p.Add(x => x.ComponentName, "Icon").Add(x => x.ComponentKey, "icon"));
        cut.WaitForAssertion(() => Assert.Equal("7", cut.Find(".docs-component-meta-issue-count").TextContent.Trim()));
        select.SetResult(42);
        cut.WaitForAssertion(() => Assert.Equal("7", cut.Find(".docs-component-meta-issue-count").TextContent.Trim()));
        Assert.Contains("Icon in:title", Uri.UnescapeDataString(cut.FindAll("a")[2].GetAttribute("href")!));
    }
}
