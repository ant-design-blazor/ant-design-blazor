---
order: 55
title:
  zh-CN: 无边框
  en-US: Clear-values
---

## zh-CN

无边框样式。

## en-US

Clear-values style component.

```jsx
import { Select } from 'antd';

const { Option } = Select;

ReactDOM.render(
  <>
    <Select defaultValue="lucy" style={{ width: 120 }} bordered={false}>
      <Option value="jack">Jack</Option>
      <Option value="lucy">Lucy</Option>
      <Option value="Yiminghe">yiminghe</Option>
    </Select>
    <Select defaultValue="lucy" style={{ width: 120 }} disabled bordered={false}>
      <Option value="lucy">Lucy</Option>
    </Select>
  </>,
  mountNode,
);
```
