---
category: Components
type: Data Display
cols: 1
title: Calendar
cover: https://gw.alipayobjects.com/zos/antfincdn/dPQmLq08DI/Calendar.svg
---

Container for displaying data in calendar form.

## When To Use

When data is in the form of dates, such as schedules, timetables, prices calendar, lunar calendar. This component also supports Year/Month switch.

## API

**Note:** Part of the Calendar's locale is read from `value`. So, please set the locale of `moment` correctly.

```jsx
// The default locale is en-US, if you want to use other locale, just set locale in entry file globally.
// import moment from 'moment';
// import 'moment/locale/zh-cn';
// moment.locale('zh-cn');

<Calendar
  dateCellRender={dateCellRender}
  monthCellRender={monthCellRender}
  onPanelChange={onPanelChange}
  onSelect={onSelect}
/>
```

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| dateCellRender | Customize the display of the date cell, the returned content will be appended to the cell | Func(DateTime) => RenderFragment | - |  |
| dateFullCellRender | Customize the display of the date cell, the returned content will override the cell | Func(DateTime) => RenderFragment | - |  |
| defaultValue | The date selected by default | DateTime | default date |  |
| disabledDate | Function that specifies the dates that cannot be selected | (DateTime) => boolean | - |  |
| fullscreen | Whether to display in full-screen | boolean | `true` |  |
| locale(TODO) | The calendar's locale | object | [default](https://github.com/ant-design/ant-design/blob/master/components/date-picker/locale/example.json) |  |
| mode | The display mode of the calendar | `DatePickerType.Month` \| `DatePickerType.Year` | DatePickerType.Month |  |
| monthCellRender | Customize the display of the month cell, the returned content will be appended to the cell | Func(DateTime) => RenderFragment | - |  |
| monthFullCellRender | Customize the display of the month cell, the returned content will override the cell | Func(DateTime) => RenderFragment | - |  |
| validRange | to set valid range | [DateTime, DateTime] | - |  |
| value | The current selected date | DateTime | current date |  |
| onPanelChange | Callback for when panel changes | Func(DateTime date, string mode) | - |  |
| onSelect | Callback for when a date is selected | Func(DateTime） | - |  |
| onChange | Callback for when date changes | Func(DateTime | - |  |
| headerRender | render custom header in panel | Func(CalendarHeaderRenderArgs) | - |  |
| Locale | Set custom localization. | `DatePickerLocale` | Locale for current Culture |  |
| CultureInfo | Set's the `CultureInfo` used for generate localized headers, formatting and parsing. | `CultureInfo` | CultureInfo.DefaultThreadCurrentUICulture  |  |
