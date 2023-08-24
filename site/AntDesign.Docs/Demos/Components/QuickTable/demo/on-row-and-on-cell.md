---
order: 1
title:
  en-US: Set props on per row
  zh-CN: 设置行属性
---

## zh-CN

可通过传入一个返回字典的委托，来给行或者单元格设置任意属性。如 `onclick`、`style`、`class` 等。
适用于 `OnRow` `OnHeaderRow` `OnCell` `OnHeaderCell`。

对于返回字典的要求，请参加官方文档[【属性展开和任意参数】](https://docs.microsoft.com/zh-cn/aspnet/core/blazor/components/?view=aspnetcore-5.0&WT.mc_id=DT-MVP-5003987#attribute-splatting-and-arbitrary-parameters) 一节。

## en-US

You can set any property to a row or cell by passing in a delegate that returns the dictionary. Such as 'onclick', 'style', 'class' and so on.
Same as `OnRow` `OnHeaderRow` `OnCell` `OnHeaderCell`.

For the rules for returning dictionaries, please refer to the official documentation [[Attribute splatting and arbitrary parameters]](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0&WT.mc_id=DT-MVP-5003987#attribute-splatting-and-arbitrary-parameters).