---
order: 7
title: TestKit
---

You can use the TestKit package in order to create a BUnit test context dervied from `AntDesignTestBase`. 

This will add the required AntDesign services to the test context so that you can create unit tests.


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