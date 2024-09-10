---
category: Components
type: 数据录入
title: DatePicker
subtitle: 日期选择框
cover: https://gw.alipayobjects.com/zos/alicdn/RT_USzA48/DatePicker.svg
---

输入或选择日期的控件。

## 何时使用

当用户需要输入一个日期，可以点击标准输入框，弹出日期面板进行选择。

## API

日期类组件包括以下六种形式。

- DatePicker
- MonthPicker
- RangePicker
- WeekPicker
- YearPicker
- QuarterPicker

目前已经支持 4 种类型的值:

- DateTime
- DateTimeOffset
- DateOnly
- TimeOnly

### 国际化配置

使用 System.Globalization.CultureInfo 属性为 DatePicker 配置国际化选项。暂时只提供 en-US 和 zh-CN 两种内置方案。

### 共同的 API

以下 API 为 DatePicker、YearPicker、MonthPicker、RangePicker, WeekPicker 共享的 API。

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| allowClear（TODO） | 是否显示清除按钮 | boolean | true |  |
| ChangeOnClose        | 关闭时绑定输入值，为 false 时需要按回车键  | boolean | false                                                                                                       |  |
| autoFocus | 自动获取焦点 | boolean | false |  |
| BoundaryAdjustMode | `Dropdown` adjustment strategy (when for example browser resize is happening)         | TriggerBoundaryAdjustMode    | TriggerBoundaryAdjustMode.InView         |
| className | 选择器 className | string | '' |  |
| dateRender | 自定义日期单元格的内容 | function(currentDate: moment, today: moment) => React.ReactNode | - |  |
| disabled | 禁用 | boolean | false |  |
| disabledDate | 不可选择的日期 | (currentDate: moment) => boolean | 无 |  |
| dropdownClassName | 额外的弹出日历 className | string | - |  |
| getPopupContainer | 定义浮层的容器，默认为 body 上新建 div | function(trigger) | 无 |  |
| locale（TODO） | 国际化配置 | object | [默认配置](https://github.com/ant-design/ant-design/blob/master/components/date-picker/locale/example.json) |  |
| mode（TODO） | 日期面板的状态（[设置后无法选择年份/月份？](/docs/react/faq#当我指定了-DatePicker/RangePicker-的-mode-属性后，点击后无法选择年份/月份？)） | `time` \| `date` \| `month` \| `year` \| `decade` | - |  |
| open | 控制弹层是否展开 | boolean | - |  |
| picker | 设置选择器类型 | `date` \| `week` \| `month` \| `quarter` (4.1.0) \| `year` | `date` |  |
| placeholder | 输入框提示文字 | string\|RangePicker\[] | - |  |
| placement | 选择框弹出的位置 | `bottomLeft` `bottomRight` `topLeft` `topRight` | bottomLeft |  |
| popupStyle | 额外的弹出日历样式 | CSSProperties | {} |  |
| size | 输入框大小，`large` 高度为 40px，`small` 为 24px，默认是 32px | `large` \| `middle` \| `small` | 无 |  |
| bordered | 是否有边框 | Boolean | true |  |
| SuffixIcon | 自定义的选择框后缀图标 | RenderFragment | - |  |
| style | 自定义输入框样式 | CSSProperties | {} |  |
| onOpenChange | 弹出日历和关闭日历的回调 | function(open) | 无 |  |
| onPanelChange | 日历面板切换的回调 | function(value, mode) | - |  |
| OnClearClick | 清除按钮点击时的回调 | Action | - |  |
| inputReadOnly | 设置输入框为只读（避免在移动设备上打开虚拟键盘） | boolean | false |  |
| Mask | 指定输入格式，可使输入自动识别为时间值     | string  | - | |

### 共同的方法

| 名称    | 描述     | 版本 |
| ------- | -------- | ---- |
| blur()  | 移除焦点 |      |
| focus() | 获取焦点 |      |

### DatePicker

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| defaultValue | 默认日期，如果开始时间或结束时间为 `null` 或者 `undefined`，日期范围将是一个开区间 | [moment](http://momentjs.com/) | 无 |  |
| defaultPickerValue | 默认面板日期 | [moment](http://momentjs.com/) | 无 |  |
| disabledTime | 不可选择的时间 | function(date) | 无 |  |
| format | 设置日期格式，为数组时支持多格式匹配，展示以第一个为准。配置参考 [moment.js](http://momentjs.com/) | string \| string[] | "YYYY-MM-DD" |  |
| renderExtraFooter | 在面板中添加额外的页脚 | (mode) => React.ReactNode | - |  |
| showTime | 增加时间选择功能 | Object\|boolean | [TimePicker Options](/components/time-picker/#API) |  |
| showTime.defaultValue（TODO） | 设置用户选择日期时默认的时分秒，[例子](#components-date-picker-demo-disabled-date) | [moment](http://momentjs.com/) | moment() |  |
| showToday | 是否展示“今天”按钮 | boolean | true |  |
| value | 日期 | [moment](http://momentjs.com/) | 无 |  |
| onChange | 时间发生变化的回调 | function(date: moment, dateString: string) | 无 |  |
| onOk | 点击确定按钮的回调 | function() | - |  |
| onPanelChange | 日期面板变化时的回调 | function(value, mode) | - |  |

### YearPicker

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| defaultValue | 默认日期 | [moment](http://momentjs.com/) | 无 |  |
| defaultPickerValue | 默认面板日期 | [moment](http://momentjs.com/) | 无 |  |
| format | 展示的日期格式，配置参考 [moment.js](http://momentjs.com/) | string | "YYYY" |  |
| renderExtraFooter | 在面板中添加额外的页脚 | () => React.ReactNode | - |  |
| value | 日期 | [moment](http://momentjs.com/) | 无 |  |
| onChange | 时间发生变化的回调，发生在用户选择时间时 | function(date: moment, dateString: string) | - |  |

### QuarterPicker

`4.1.0` 新增。

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| defaultValue | 默认日期 | [moment](http://momentjs.com/) | 无 |  |
| defaultPickerValue | 默认面板日期 | [moment](http://momentjs.com/) | 无 |  |
| format | 展示的日期格式，配置参考 [moment.js](http://momentjs.com/) | string | "YYYY-\QQ" |  |
| renderExtraFooter | 在面板中添加额外的页脚 | () => React.ReactNode | - |  |
| value | 日期 | [moment](http://momentjs.com/) | 无 |  |
| onChange | 时间发生变化的回调，发生在用户选择时间时 | function(date: moment, dateString: string) | - |  |

### MonthPicker

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| defaultValue | 默认日期 | [moment](http://momentjs.com/) | 无 |  |
| defaultPickerValue | 默认面板日期 | [moment](http://momentjs.com/) | 无 |  |
| format | 展示的日期格式，配置参考 [moment.js](http://momentjs.com/) | string | "YYYY-MM" |  |
| monthCellRender | 自定义的月份内容渲染方法 | function(date, locale): ReactNode | - |  |
| renderExtraFooter | 在面板中添加额外的页脚 | () => React.ReactNode | - |  |
| value | 日期 | [moment](http://momentjs.com/) | 无 |  |
| onChange | 时间发生变化的回调，发生在用户选择时间时 | function(date: moment, dateString: string) | - |  |

### WeekPicker

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| defaultValue | 默认日期 | [moment](http://momentjs.com/) | - |  |
| defaultPickerValue | 默认面板日期 | [moment](http://momentjs.com/) | 无 |  |
| format | 展示的日期格式，配置参考 [moment.js](http://momentjs.com/) | string | "YYYY-wo" |  |
| value | 日期 | [moment](http://momentjs.com/) | - |  |
| onChange | 时间发生变化的回调，发生在用户选择时间时 | function(date: moment, dateString: string) | - |  |
| renderExtraFooter | 在面板中添加额外的页脚 | (mode) => React.ReactNode | - |  |

### RangePicker

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| allowEmpty（TODO） | 允许起始项部分为空 | \[boolean, boolean] | \[false, false] |  |
| defaultValue | 默认日期 | [moment](http://momentjs.com/)\[] | 无 |  |
| defaultPickerValue | 默认面板日期 | [moment](http://momentjs.com/)\[] | 无 |  |
| disabled | 禁用起始项 | `[boolean, boolean]` | 无 |  |
| disabledTime | 不可选择的时间 | function(dates: \[moment, moment\], partial: `'start'|'end'`) | 无 |  |
| format | 展示的日期格式 | string | "YYYY-MM-DD HH:mm:ss" |  |
| ranges（TODO） | 预设时间范围快捷选择 | { \[range: string]: [moment](http://momentjs.com/)\[] } \| { \[range: string]: () => [moment](http://momentjs.com/)\[] } | 无 |  |
| renderExtraFooter | 在面板中添加额外的页脚 | () => React.ReactNode | - |  |
| separator（TODO） | 设置分隔符 | string | '~' |  |
| showTime | 增加时间选择功能 | Object\|boolean | [TimePicker Options](/components/time-picker/#API) |  |
| showTime.defaultValue | 设置用户选择日期时默认的时分秒，[例子](#components-date-picker-demo-disabled-date) | [moment](http://momentjs.com/)\[] | \[moment(), moment()] |  |
| value | 日期 | [moment](http://momentjs.com/)\[] | 无 |  |
| onCalendarChange（TODO） | 待选日期发生变化的回调 | function(dates: \[moment, moment\], dateStrings: \[string, string\]) | 无 |  |
| onChange | 日期范围发生变化的回调 | function(dates: \[moment, moment\], dateStrings: \[string, string\]) | 无 |  |

<style>
.code-box-demo .ant-picker {
  margin: 0 8px 12px 0;
}
</style>

## FAQ

- [当我指定了 DatePicker/RangePicker 的 mode 属性后，点击后无法选择年份/月份？](/docs/react/faq#当我指定了-DatePicker/RangePicker-的-mode-属性后，点击后无法选择年份/月份？)

- [如何在 DatePicker 中使用自定义日期库（如 dayjs ）](/docs/react/replace-moment#DatePicker)
