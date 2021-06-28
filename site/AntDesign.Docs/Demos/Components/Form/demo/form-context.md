---
order: 8
title:
  zh-CN: 多表单联动
  en-US: Control between forms
---

## zh-CN

通过 `FormProvider` 在表单间处理数据。本例子中，Modal 的确认按钮在 Form 之外，通过 `form.Submit` 方法调用表单提交功能。反之，则推荐使用 `<Button HtmlType="submit" />` 调用 web 原生提交逻辑。

## en-US

Use `FormProvider` to process data between forms. In this case, submit button is in the Modal which is out of Form. You can use `form.Submit` to submit form. Besides, we recommend native `<Button HtmlType="submit" />` to submit a form.

