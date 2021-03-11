---
order: 10
title:
  zh-CN: 获得选项的文本
  en-US: Get value of selected item
---

## zh-CN

As a default behavior, the `OnSelectedItemChanged` callback can only get the value of the selected item. The `LabelInValue` property can be used to get the label property of the selected item.

The label of the selected item will be packed as an `string (JSON)` for passing to the `OnSelectedItemChanged` callback. This function is only available when the SelectOptions are created without a DataStore.

## en-US

As a default behavior, the `OnSelectedItemChanged` callback can only get the value of the selected item. The `LabelInValue` property can be used to get the label property of the selected item.

The label of the selected item will be packed as an `string (JSON)` for passing to the `OnSelectedItemChanged` callback. This function is only available when the SelectOptions are created without a DataStore.

```jsx
import { Select } from 'antd';

const { Option } = Select;

function handleChange(value) {
  console.log(value); // { key: "lucy", label: "Lucy (101)" }
}

ReactDOM.render(
  <Select
    labelInValue
    defaultValue={{ key: 'lucy' }}
    style={{ width: 120 }}
    onChange={handleChange}
  >
    <Option value="jack">Jack (100)</Option>
    <Option value="lucy">Lucy (101)</Option>
  </Select>,
  mountNode,
);
```
