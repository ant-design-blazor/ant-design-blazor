---
category: Components
type: 数据录入
title: DatePicker
subtitle: 日期选择框
---

输入或选择日期的控件。

## 何时使用

当用户需要输入一个日期，可以点击标准输入框，弹出日期面板进行选择。


已实现DatePicker组件：支持date、decade、year、month、quarter、week模式
已实现：MonthPicker、RangePicker、WeekPicker、YearPicker、QuarterPicker（对应DatePicker的各种模式）

未实现的通用属性：AllowClear、GetPopupContainer、Locale、Mode(Mode为3.0妥协的功能，不打算支持)
未实现的DatePicker属性：showTime.defaultValue、onOk
未实现的RangePicker属性：allowEmpty、ranges、separator、onCalendarChange
未实现的功能：RangePicker的操作模式和Ant Design不完全一致
