using AntDesign.JsInterop;
using AntDesign.Tests;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;
using AntDesign.Docs.Services;
using System.Threading.Tasks;

namespace AntDesign.Tests.Docs;

public abstract class DocumentationTestBase : AntDesignTestBase
{
    protected DocumentationTestBase()
    {
        JSInterop.Setup<HtmlElement>("AntDesign.interop.domInfoHelper.getInfo", _ => true)
            .SetResult(new HtmlElement());

        var localizer = new Mock<IStringLocalizer>();
        localizer
            .Setup(item => item[It.IsAny<string>()])
            .Returns((string key) => new LocalizedString(key, key));
        Services.AddSingleton(localizer.Object);
        var issues = new Mock<IComponentIssueCountService>();
        issues.Setup(service => service.GetCountAsync(It.IsAny<string>())).Returns(Task.FromResult<int?>(null));
        Services.AddSingleton(issues.Object);
    }
}
