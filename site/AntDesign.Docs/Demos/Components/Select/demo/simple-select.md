---
order: 0
title:
  zh-CN: 简单的封装
  en-US: Simple encapsulation
---

## zh-CN 

由于泛型的写法有点复杂，所以封装了 `string` 类型的 `SimpleSelect`。还可以根据需求封装不同类型的 `Select` 组件。

```cs
    public class SimpleSelect : Select<string, string> { }

    public class SimpleSelectOption : SelectOption<string, string> { }
```

## en-US

Because the generic select is a bit complicated, we wrap a 'string' type 'SimpleSelect'. You can also wrap different types of `Select` .

```cs
    public class SimpleSelect : Select<string, string> { }

    public class SimpleSelectOption : SelectOption<string, string> { }
```