---
order: 6
title:
  zh-CN: 分组
  en-US: Option Group
---

## zh-CN

Entries can be grouped using a group indicator. The parameter `GroupName` is used for this. It is recommended to sort the entries when using the `GroupName` parameter `(SortByLabel | SortByGroup)`. Otherwise there may be problems with keyboard navigation.

## en-US

Entries can be grouped using a group indicator. The parameter `GroupName` is used for this. It is recommended to sort the entries when using the `GroupName` parameter `(SortByLabel | SortByGroup)`. Otherwise there may be problems with keyboard navigation.

```jsx
import { Select } from 'antd';

const { Option, OptGroup } = Select;

function handleChange(value) {
  console.log(`selected ${value}`);
}

ReactDOM.render(
  <Select defaultValue="lucy" style={{ width: 200 }} onChange={handleChange}>
    <OptGroup label="Manager">
      <Option value="jack">Jack</Option>
      <Option value="lucy">Lucy</Option>
    </OptGroup>
    <OptGroup label="Engineer">
      <Option value="Yiminghe">yiminghe</Option>
    </OptGroup>
  </Select>,
  mountNode,
);
```
