---
order: 8
title:
  zh-CN: Value binding (CN)
  en-US: Value binding
---

## zh-CN
3 types of data bindins:
1. Each `RangeItem` is added and bounded individually.
2. Bind `Value` parameter to an object of type `IEnumerable<(double, double)>`. `MultiRangeSlider` will generate a `RangeItem` for every entry in the collection.
3. Bind `Data` parameter to a collection (`IEnumerable`) of objects that implements `AntDesign.IRangeItemData`. This interface contains properties that allow visual customization of each range.

## en-US
3 types of data bindins:
1. Each `RangeItem` is added and bounded individually.
2. Bind `Value` parameter to an object of type `IEnumerable<(double, double)>`. `MultiRangeSlider` will generate a `RangeItem` for every entry in the collection.
3. Bind `Data` parameter to a collection (`IEnumerable`) of objects that implements `AntDesign.IRangeItemData`. This interface contains properties that allow visual customization of each range.
