---
order: 7
title: 单元测试工具包
---

为了能够创建从 `AntDesignTestBase` 创建一个 `BUnit` 测试上下文，您可以使用 `AntDesign.TestKit` 测试工具包来辅助你编写测试用例。

它会包含一些必要的 AntDesign 组件服务到测试上下文中，使你能够方便地编写你的单元测试。


```cs
public class ButtonTests : AntDesignTestBase
{
    [Fact]
    public void Renders_an_empty_button()
    {
        var cut = Context.RenderComponent<AntDesign.Button>();
        cut.MarkupMatches(@"
            <button class=""ant-btn ant-btn-default"" id:ignore type=""button"" ant-click-animating-without-extra-node=""false""></button>
        ");
    }
}
```